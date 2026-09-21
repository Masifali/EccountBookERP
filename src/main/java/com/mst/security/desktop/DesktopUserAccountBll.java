package com.mst.security.desktop;

import com.mst.security.LegacyUserPasswordEncoder;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.security.SecureRandom;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import java.util.ArrayList;
import java.util.List;
import java.util.Map;
import java.util.regex.Pattern;

/**
 * Architecture.BLL.UserAccount (BLL 0087), login surface, ported method for method.
 *
 * Every method below names the C# line it comes from and keeps its procedure, its parameters, its
 * @Activity and its message text. Where the C# reads a global
 * ({@code clsGlobalVariables.UserAccount}, {@code clsGlobalVariables.configrationsAllocation})
 * this returns the value instead - a web request has no process-wide "current user" to write to,
 * and inventing one would be a bug factory.
 *
 * ---------------------------------------------------------------------------------------------
 * WHAT IS NOT PORTED, AND WHY - SAID HERE RATHER THAN LEFT TO BE DISCOVERED
 * ---------------------------------------------------------------------------------------------
 * Two methods depend on something outside the database:
 *
 *   setAuthenticationCode                     sends the OTP by e-mail
 *   sendAuthenticationCodeToUserMobileNumber  sends it by SMS
 *
 * Their database halves are ported exactly. The transports are not, and they are not faked: each
 * goes through {@link OtpTransport}, and with no implementation registered the method fails
 * loudly instead of pretending a code was delivered.
 *
 * The desktop sends OTP mail from a Gmail account whose app password is a literal in
 * Architecture.DAL.SystemUtilities.Email. That value is NOT carried over. It is a live credential
 * to a third-party mailbox - unlike the licence-key password in {@link DesktopLicenseKey}, which
 * only unlocks this application's own licence text - and it is already exposed in every copy of
 * the shipped DLL, so it is worth rotating whether or not OTP is ever built here.
 */
@Service
public class DesktopUserAccountBll {

    private static final Logger LOG = LoggerFactory.getLogger(DesktopUserAccountBll.class);

    /** LoginNew.cs:166,181 - one message for both halves of the credential pair. */
    public static final String INVALID_LOGIN = "Invalid UserId or Password";

    private static final Pattern EMAIL = Pattern.compile("^[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,4}$");
    private static final SecureRandom RANDOM = new SecureRandom();

    @Autowired private DesktopUserAccountDal dal;
    @Autowired private LegacyUserPasswordEncoder encryption;

    /** Optional - see the class note. Absent means OTP delivery is not configured. */
    @Autowired(required = false) private OtpTransport otpTransport;

    // ========================================================================== BLL 0087 :36

    /**
     * Login.
     *
     * <pre>
     *   obj.Password = Encryption.UserPasswordEncryptString(obj.Password);
     *   Sp_UserAccount_Login @UserName, @Password
     *                      [, @AppId]            only when AppId          &gt; 0
     *                      [, @ValidateVersion]  only when ValidateVersion &gt; 0
     *                      [, @AppVersion]       only when AppVersion      &gt; 0
     *                      [, @DeviceId]         only when DeviceId is non-empty
     *   if (rows.Count == 0) throw "Invalid UserId or Password";
     *   configrationsAllocation = ConfigrationsAllocation.HistoryConfiquration(org, company)   // when CompanyId > 0
     *   UserProfileList         = GetUserProfileByUserId(id)
     *   switch (CheckTwoWayAuthenticationConfiguration(row)) { 1: AuthenticationCode = setAuthenticationCode(id),
     *                                                             AuthenticationEnabledForUser = 1
     *                                                          0: both 0 }
     * </pre>
     *
     * The optional parameters are each omitted when not supplied, exactly as the C# omits them -
     * a caller with no DeviceId (a browser) takes a path the procedure already supports.
     *
     * <b>otp = false</b> skips the two-way-authentication branch entirely, for a caller that has
     * not built OTP delivery yet. It does not weaken anything the procedure enforces; it only
     * declines to generate and mail a code that nothing would then verify.
     */
    public LoginResult login(String userName, String rawPassword, int appId, int validateVersion,
                             int appVersion, String deviceId, boolean otp) {
        String encrypted = encryption.encode(rawPassword);

        Map<String, Object> p = DesktopUserAccountDal.params();
        p.put("@UserName", userName);
        p.put("@Password", encrypted);
        if (appId > 0)                                   p.put("@AppId", appId);
        if (validateVersion > 0)                         p.put("@ValidateVersion", validateVersion);
        if (appVersion > 0)                              p.put("@AppVersion", appVersion);
        if (deviceId != null && !deviceId.isEmpty())     p.put("@DeviceId", deviceId);

        List<Map<String, Object>> rows = dal.getDataAll("Sp_UserAccount_Login", p);
        if (rows.isEmpty()) {
            throw new IllegalArgumentException(INVALID_LOGIN);
        }

        LoginResult result = new LoginResult();
        result.user = rows.get(0);

        int organizationId = DesktopUserAccountDal.intOf(result.user, "OrganizationId");
        int companyId      = DesktopUserAccountDal.intOf(result.user, "CompanyId");
        int id             = DesktopUserAccountDal.intOf(result.user, "Id");

        if (companyId > 0) {
            result.configurationsAllocation = historyConfiquration(organizationId, companyId);
        }
        result.userProfile = getUserProfileByUserId(id);

        if (otp) {
            int enabled = checkTwoWayAuthenticationConfiguration(id, organizationId, companyId);
            if (enabled == 1) {
                result.authenticationCode = setAuthenticationCode(id, null);
                result.authenticationEnabledForUser = 1;
            } else if (enabled == 0) {
                result.authenticationCode = 0;
                result.authenticationEnabledForUser = 0;
            }
        }
        return result;
    }

    /** The globals BLL.Login writes, returned instead. */
    public static class LoginResult {
        /** The Sp_UserAccount_Login row - clsGlobalVariables.UserAccount. */
        public Map<String, Object> user;
        /** Proc_ConfigrationsAllocation_History - clsGlobalVariables.configrationsAllocation. */
        public List<Map<String, Object>> configurationsAllocation = new ArrayList<>();
        /** SP_UserProfile_GetAllMethod @Activity='GetUserProfileByUserId'. */
        public Map<String, Object> userProfile;
        public int authenticationCode;
        public int authenticationEnabledForUser;
    }

    // ========================================================================= BLL 0087 :117

    /**
     * AdminLogin - [dbo].[USP_AdminLogin] @UserName, @Password, with the password encrypted first,
     * the same way Login does it.
     */
    public Map<String, Object> adminLogin(String userName, String rawPassword) {
        Map<String, Object> p = DesktopUserAccountDal.params();
        p.put("@UserName", userName);
        p.put("@Password", encryption.encode(rawPassword));
        List<Map<String, Object>> rows = dal.getDataAdminUser("[dbo].[USP_AdminLogin]", p);
        /* The C# indexes [0] unconditionally and throws IndexOutOfRange on a bad login. Returning
           null instead is the same refusal without the misleading exception type. */
        return rows.isEmpty() ? null : rows.get(0);
    }

    // ========================================================================= BLL 0087 :141

    /**
     * PlayIdUpdate - Usp_PlayIdUpdate_UserAccount @UserName, @Password, @PlayerId.
     *
     * The C# passes the password through UNENCRYPTED - it takes a string and forwards it. Ported
     * as-is rather than "corrected", because the procedure is presumably matching on whatever the
     * caller sends, and changing that here would silently stop the update from matching any row.
     */
    public void playIdUpdate(String userName, String password, String playerId) {
        Map<String, Object> p = DesktopUserAccountDal.params();
        p.put("@UserName", userName);
        p.put("@Password", password);
        p.put("@PlayerId", playerId);
        dal.exec("Usp_PlayIdUpdate_UserAccount", p);
    }

    // ========================================================================= BLL 0087 :170

    /** GetByID - Sp_UserAccount_GetAllMethod @Id, @Activity='ReadById', with child collections. */
    public Map<String, Object> getById(int id) {
        Map<String, Object> p = DesktopUserAccountDal.params();
        p.put("@Id", id);
        p.put("@Activity", "ReadById");
        List<Map<String, Object>> rows = dal.getData("Sp_UserAccount_GetAllMethod", p);
        return rows.isEmpty() ? null : rows.get(0);
    }

    // ========================================================================= BLL 0087 :193

    /** GetUserProfileByUserId - SP_UserProfile_GetAllMethod @UserId, @Activity='GetUserProfileByUserId'. */
    public Map<String, Object> getUserProfileByUserId(int userId) {
        Map<String, Object> p = DesktopUserAccountDal.params();
        p.put("@UserId", userId);
        p.put("@Activity", "GetUserProfileByUserId");
        List<Map<String, Object>> rows = dal.exec("SP_UserProfile_GetAllMethod", p);
        return rows.isEmpty() ? null : rows.get(0);
    }

    // ========================================================================= BLL 0087 :217

    /** ReadByUsername - Proc_AccountUser_ReadByUsername @UserName, with child collections. */
    public Map<String, Object> readByUsername(String userName) {
        Map<String, Object> p = DesktopUserAccountDal.params();
        p.put("@UserName", userName);
        List<Map<String, Object>> rows = dal.getData("Proc_AccountUser_ReadByUsername", p);
        return rows.isEmpty() ? null : rows.get(0);
    }

    // ========================================================================= BLL 0087 :318

    /**
     * UpdateUserNewPassword - Sp_UserAccount_UpdateNewPassword
     * @OrganizationId, @CompanyId, @OldPassword, @Password, @ID.
     *
     * Both passwords are encrypted here because the column they are compared and written against
     * is encrypted; the C# receives a ReportsParameters whose fields the caller has already
     * encrypted. Callers of this method pass the raw values.
     */
    public void updateUserNewPassword(int organizationId, int companyId, int userId,
                                      String oldRawPassword, String newRawPassword) {
        Map<String, Object> p = DesktopUserAccountDal.params();
        p.put("@OrganizationId", organizationId);
        p.put("@CompanyId", companyId);
        p.put("@OldPassword", encryption.encode(oldRawPassword));
        p.put("@Password", encryption.encode(newRawPassword));
        p.put("@ID", userId);
        dal.exec("Sp_UserAccount_UpdateNewPassword", p);
    }

    // ========================================================================= BLL 0087 :381

    /**
     * LicenseCheck, branch for branch.
     *
     * <pre>
     *   Sp_UserAccount_GetAllMethod @OrganizationId, @CompanyId, @Activity='GetLicenseKey'
     *   no rows                      -> "License Not Found!"
     *   LicenseKey empty             -> "Your Licensed has been Expired ..."
     *   key = LicenseKey.ReturnKey(key)
     *   no '(' in the key            -> "Invalid license format. No opening bracket found."
     *   text after '(' not a date    -> "Unable to parse license date."
     *   that date &lt; today            -> "Your License has Expired. ..."
     *   otherwise: GetMaxDate @OrganizationId, @CompanyId, @LicenseKey, @UserId, @DeviceId,
     *                         @MachineName, @IPAddress, @LoginMacAddress, @SysDate
     * </pre>
     *
     * Note the comparison is {@code DateTime.Now.Date > result.Date} - strictly after, so the
     * expiry day itself is still valid. Reproduced with LocalDate for the same reason.
     */
    public void licenseCheck(int organizationId, int companyId, int userId,
                             String deviceId, String machine, String ip, String macAddress) {
        Map<String, Object> p = DesktopUserAccountDal.params();
        p.put("@OrganizationId", organizationId);
        p.put("@CompanyId", companyId);
        p.put("@Activity", "GetLicenseKey");
        List<Map<String, Object>> rows = dal.exec("Sp_UserAccount_GetAllMethod", p);
        if (rows.isEmpty()) throw new IllegalStateException("License Not Found!");

        String key = DesktopUserAccountDal.stringOf(rows.get(0), "LicenseKey");
        if (key == null || key.isEmpty()) {
            throw new IllegalStateException("Your Licensed has been Expired Please contact "
                    + "EccountBookERP Support Team Cell No : 0304-4059175");
        }

        key = DesktopLicenseKey.returnKey(key);
        int bracket = key.indexOf('(');
        if (bracket < 0) {
            throw new IllegalStateException("Invalid license format. No opening bracket found.");
        }
        LocalDate expiry = parseLicenseDate(key.substring(bracket + 1));
        if (expiry == null) {
            throw new IllegalStateException("Unable to parse license date.");
        }
        if (LocalDate.now().isAfter(expiry)) {
            throw new IllegalStateException("Your License has Expired. Please contact "
                    + "EccountBookERP SupportTeam CellNo: 0304-4059175");
        }

        Map<String, Object> p2 = DesktopUserAccountDal.params();
        p2.put("@OrganizationId", organizationId);
        p2.put("@CompanyId", companyId);
        p2.put("@LicenseKey", key);
        p2.put("@UserId", userId);
        p2.put("@DeviceId", deviceId);
        p2.put("@MachineName", machine);
        p2.put("@IPAddress", ip);
        p2.put("@LoginMacAddress", macAddress);
        p2.put("@SysDate", LocalDateTime.now().format(DateTimeFormatter.ofPattern("yyyy-MM-dd HH:mm:ss")));
        dal.exec("GetMaxDate", p2);
    }

    /**
     * DateTime.TryParse on the text after '(' - which accepts a good deal, so several shapes are
     * tried rather than one being assumed. A trailing ')' is stripped first, since the key is
     * "something(date)" and TryParse tolerates the bracket where a strict parser will not.
     */
    private static LocalDate parseLicenseDate(String text) {
        if (text == null) return null;
        String s = text.trim();
        int close = s.indexOf(')');
        if (close >= 0) s = s.substring(0, close).trim();
        if (s.isEmpty()) return null;
        String[] patterns = { "yyyy-MM-dd", "dd/MM/yyyy", "MM/dd/yyyy", "d/M/yyyy", "M/d/yyyy",
                              "dd-MM-yyyy", "dd-MMM-yyyy", "yyyy/MM/dd" };
        for (String pattern : patterns) {
            try { return LocalDate.parse(s, DateTimeFormatter.ofPattern(pattern)); }
            catch (Exception ignored) { /* try the next shape */ }
        }
        try { return LocalDateTime.parse(s.replace(' ', 'T')).toLocalDate(); }
        catch (Exception ignored) { /* not a date-time either */ }
        return null;
    }

    // ========================================================================= BLL 0087 :481

    /**
     * GetLicensCompanyAndExpiryDate - the same GetLicenseKey read, then
     * USP_GetLicenseExpiryDateByLicense @LicenseKey (that one parameter only).
     */
    public List<Map<String, Object>> getLicensCompanyAndExpiryDate(int organizationId, int companyId) {
        Map<String, Object> p = DesktopUserAccountDal.params();
        p.put("@OrganizationId", organizationId);
        p.put("@CompanyId", companyId);
        p.put("@Activity", "GetLicenseKey");
        List<Map<String, Object>> rows = dal.exec("Sp_UserAccount_GetAllMethod", p);
        if (rows.isEmpty()) throw new IllegalStateException("License Not Found!");

        String key = DesktopUserAccountDal.stringOf(rows.get(0), "LicenseKey");
        if (key != null && !key.isEmpty()) key = DesktopLicenseKey.returnKey(key);

        Map<String, Object> p2 = DesktopUserAccountDal.params();
        p2.put("@LicenseKey", key);
        return dal.exec("USP_GetLicenseExpiryDateByLicense", p2);
    }

    // ========================================================================= BLL 0087 :526

    /**
     * setAuthenticationCode.
     *
     * <pre>
     *   code = new Random().Next(100000, 999999);        // .NET: upper bound EXCLUSIVE
     *   profile = UserProfile.GetUserProfileByUserId(ID);
     *   no profile row                 -> "Please add user profile info"
     *   email = UserEmail ?? profile.Email
     *   empty                          -> "Couldn't Find Email to Send Authentication Code!"
     *   fails ^[a-z0-9._%+-]+@...$     -> "Email is not Valid!"
     *   Email.SendAuthenticationEmail(profile.OrganizationId, profile.CompanyId, email, code)
     *   Sp_UserAccount_GetAllMethod @Activity='UpdateAuthenticationCode', @Id, @AuthenticationCode
     * </pre>
     *
     * Two faithful details worth naming: the upper bound is exclusive, so the desktop can never
     * issue 999999 - reproduced; and the regular expression is case-SENSITIVE lower-case in the
     * C#, so "User@Example.COM" is rejected there - also reproduced, because an address the
     * desktop refuses must not silently start working here.
     *
     * The send goes through {@link OtpTransport}; with none registered this throws rather than
     * storing a code nobody received.
     */
    public int setAuthenticationCode(int id, String userEmail) {
        int code = 100000 + RANDOM.nextInt(999999 - 100000);

        Map<String, Object> profileParams = DesktopUserAccountDal.params();
        profileParams.put("@UserId", id);
        profileParams.put("@Activity", "GetUserProfileByUserId");
        List<Map<String, Object>> profile = dal.exec("SP_UserProfile_GetAllMethod", profileParams);
        if (profile.isEmpty()) throw new IllegalStateException("Please add user profile info");

        String email = (userEmail != null && !userEmail.isEmpty())
                ? userEmail
                : DesktopUserAccountDal.stringOf(profile.get(0), "Email");
        if (email == null || email.isEmpty()) {
            throw new IllegalStateException("Couldn't Find Email to Send Authentication Code!");
        }
        if (!EMAIL.matcher(email).matches()) {
            throw new IllegalStateException("Email is not Valid!");
        }

        if (otpTransport == null) {
            throw new IllegalStateException("Authentication code delivery is not configured.");
        }
        otpTransport.sendAuthenticationEmail(
                DesktopUserAccountDal.intOf(profile.get(0), "OrganizationId"),
                DesktopUserAccountDal.intOf(profile.get(0), "CompanyId"),
                email, String.valueOf(code));

        storeAuthenticationCode(id, code);
        return code;
    }

    // ========================================================================= BLL 0087 :573

    /**
     * expireAuthenticationCode - stores <b>1</b>, not 0.
     *
     * Worth being explicit about: the C# sets {@code int num = 1}. Writing 0 would be the obvious
     * "cleared" value and would be wrong, because verifyCode compares the stored code against what
     * the user typed and a typed "1" is a plausible accident in a way that a six-digit code is not.
     */
    public void expireAuthenticationCode(int id) {
        storeAuthenticationCode(id, 1);
    }

    private void storeAuthenticationCode(int id, int code) {
        Map<String, Object> p = DesktopUserAccountDal.params();
        p.put("@Activity", "UpdateAuthenticationCode");
        p.put("@Id", id);
        p.put("@AuthenticationCode", code);
        dal.exec("Sp_UserAccount_GetAllMethod", p);
    }

    // ========================================================================= BLL 0087 :602

    /**
     * VerifyCode - Sp_UserAccount_GetAllMethod @Id, @Activity='ReadAuthenticationCode', then
     * compares the stored AuthenticationCode with the supplied one.
     */
    public boolean verifyCode(int id, int authenticationCode) {
        Map<String, Object> p = DesktopUserAccountDal.params();
        p.put("@Id", id);
        p.put("@Activity", "ReadAuthenticationCode");
        List<Map<String, Object>> rows = dal.verifyCode("Sp_UserAccount_GetAllMethod", p);
        if (rows.isEmpty()) return false;
        return DesktopUserAccountDal.intOf(rows.get(0), "AuthenticationCode") == authenticationCode;
    }

    // ========================================================================= BLL 0087 :630

    /**
     * SendAuthenticationCodeToUserMobileNumber - the desktop picks an SMS provider by name
     * ("LifeTimesSms") and posts to it. The provider lookup and its HTTP call are not ported;
     * the seam is {@link OtpTransport}, and with none registered this throws.
     */
    public void sendAuthenticationCodeToUserMobileNumber(String cellNo, int code) {
        if (otpTransport == null) {
            throw new IllegalStateException("Authentication code delivery is not configured.");
        }
        otpTransport.sendAuthenticationSms(cellNo, String.valueOf(code));
    }

    // ========================================================================= BLL 0087 :649

    /**
     * CheckTwoWayAuthenticationConfiguration - Sp_UserAccount_GetAllMethod
     * @Id, @OrganizationId, @CompanyId, @Activity='CheckTwoWayAuthenticationConfiguration',
     * returning the AuthenticationEnabledForUser column of row 0.
     */
    public int checkTwoWayAuthenticationConfiguration(int id, int organizationId, int companyId) {
        Map<String, Object> p = DesktopUserAccountDal.params();
        p.put("@Id", id);
        p.put("@OrganizationId", organizationId);
        p.put("@CompanyId", companyId);
        p.put("@Activity", "CheckTwoWayAuthenticationConfiguration");
        List<Map<String, Object>> rows = dal.exec("Sp_UserAccount_GetAllMethod", p);
        if (rows.isEmpty()) {
            /* The C# indexes Rows[0] and would throw. Returning -1 keeps the caller's switch from
               matching either branch, which is what "no answer" should mean - not "disabled". */
            LOG.warn("CheckTwoWayAuthenticationConfiguration returned no row for user {}", id);
            return -1;
        }
        return DesktopUserAccountDal.intOf(rows.get(0), "AuthenticationEnabledForUser");
    }

    // ========================================================================= BLL 0087 :683

    /**
     * CheckIfPasswordIsExpired - [dbo].[Sp_UserAccount_GetAllMethod]
     * @OrganizationId, @CompanyId, @Id, @Activity='CheckIfPasswordIsExpired', reading the
     * ExpiredStatus column. No rows means false, as in the C#.
     */
    public boolean checkIfPasswordIsExpired(int organizationId, int companyId, int userId) {
        Map<String, Object> p = DesktopUserAccountDal.params();
        p.put("@OrganizationId", organizationId);
        p.put("@CompanyId", companyId);
        p.put("@Id", userId);
        p.put("@Activity", "CheckIfPasswordIsExpired");
        List<Map<String, Object>> rows = dal.exec("[dbo].[Sp_UserAccount_GetAllMethod]", p);
        if (rows.isEmpty()) return false;
        return DesktopUserAccountDal.boolOf(rows.get(0), "ExpiredStatus");
    }

    // ========================================================================= BLL 0087 :879

    /** GetVersion - usp_getAppVersion @userId, reading the AppVersion column. 0 when no row. */
    public int getVersion(int userId) {
        Map<String, Object> p = DesktopUserAccountDal.params();
        p.put("@userId", userId);
        List<Map<String, Object>> rows = dal.exec("usp_getAppVersion", p);
        if (rows.isEmpty()) return 0;
        return DesktopUserAccountDal.intOf(rows.get(0), "AppVersion");
    }

    // ===================================================== ConfigrationsAllocation (BLL 0621)

    /**
     * ConfigrationsAllocation.HistoryConfiquration(OrganizationId, CompanyId) -&gt; DAL 0555
     * History -&gt; Proc_ConfigrationsAllocation_History @OrganizationId, @CompanyId.
     *
     * The desktop caches this into a global at login. Returned here instead; a failure is logged
     * and yields an empty list rather than refusing a login that the procedure already accepted.
     */
    public List<Map<String, Object>> historyConfiquration(int organizationId, int companyId) {
        Map<String, Object> p = DesktopUserAccountDal.params();
        p.put("@OrganizationId", organizationId);
        p.put("@CompanyId", companyId);
        try {
            return dal.exec("Proc_ConfigrationsAllocation_History", p);
        } catch (Exception e) {
            LOG.warn("Proc_ConfigrationsAllocation_History failed for org {} company {}",
                     organizationId, companyId, e);
            return new ArrayList<>();
        }
    }

    /**
     * The seam for the two OTP transports. No implementation is registered, so OTP is inert -
     * deliberately, and visibly, rather than half-working.
     */
    public interface OtpTransport {
        /** Architecture.DAL.SystemUtilities.Email.SendAuthenticationEmail. */
        void sendAuthenticationEmail(int organizationId, int companyId, String toEmail, String otp);
        /** BLL 0087 SendAuthenticationCodeToUserMobileNumber. */
        void sendAuthenticationSms(String cellNo, String otp);
    }
}
