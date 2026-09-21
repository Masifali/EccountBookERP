package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.LicenseKeyRepository;
import com.mst.security.CurrentUserContext;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import javax.crypto.Cipher;
import javax.crypto.SecretKey;
import javax.crypto.SecretKeyFactory;
import javax.crypto.spec.IvParameterSpec;
import javax.crypto.spec.PBEKeySpec;
import javax.crypto.spec.SecretKeySpec;
import java.nio.charset.StandardCharsets;
import java.util.Arrays;
import java.util.Base64;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * System Utilities -> "License Key" (Architecture.WinApp.LicenseKey, 263 lines).
 *
 * Two operations only: FillDate() on load, and btnSubmit_Click.
 *
 * ---------------------------------------------------------------------------------------------
 * ReturnKey - Architecture.Common.LicenseKey.ReturnKey, ported exactly
 * ---------------------------------------------------------------------------------------------
 *     password "AMIR2SPSCS44602"
 *     salt     { 73,118,97,110,32,77,101,100,118,101,100,101,118 }   // "Ivan Medvedev"
 *     Rfc2898DeriveBytes(password, salt)        -> PBKDF2-HMAC-SHA1, 1000 iterations (the .NET
 *                                                  default for this constructor)
 *     aes.Key = GetBytes(32);  aes.IV = GetBytes(16)
 *
 * GetBytes reads from ONE continuous derived stream, so 32 then 16 is the first 48 bytes split
 * 32/16 - which is what {@link #deriveKeyAndIv()} does. Aes.Create() defaults to CBC with PKCS7
 * padding, and the plaintext is UTF-16LE (Encoding.Unicode), so all three are matched below.
 * Any failure is reported with the desktop's own wording, "License Key Not Match...".
 *
 * ---------------------------------------------------------------------------------------------
 * THE GATE
 * ---------------------------------------------------------------------------------------------
 * System Utilities carries no Visible=false on the desktop, so every signed-in user reaches this
 * form - unlike the Admin Panel items. No role check is therefore applied here, because adding one
 * would make the web stricter than the desktop. What IS enforced is that the organization and
 * company come from the session, never from the request: the desktop passes
 * UserAccount.OrganizationId and UserAccount.CompanyId, so a posted pair must not be able to
 * relicense somebody else's company.
 */
@Service
public class LicenseKeyService {

    private static final String PASSWORD = "AMIR2SPSCS44602";
    private static final byte[] SALT = {
            73, 118, 97, 110, 32, 77, 101, 100, 118, 101, 100, 101, 118
    };
    private static final int ITERATIONS = 1000;      // Rfc2898DeriveBytes(string, byte[]) default

    private final CurrentUserContext context;
    private final LicenseKeyRepository repo;

    public LicenseKeyService(CurrentUserContext context, LicenseKeyRepository repo) {
        this.context = context;
        this.repo = repo;
    }

    // =================================================================================== read

    /**
     * FillDate(). The desktop swallows a failure into MessageBox.Show and leaves the picker at
     * today; here the message is returned so the page can show the same text in the same place,
     * rather than silently displaying a date that is not the licence's.
     */
    public Map<String, Object> load() {
        UserAccount u = context.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        try {
            out.put("licenseDate", expiryDate(u));
            out.put("error", null);
        } catch (RuntimeException e) {
            out.put("licenseDate", null);
            out.put("error", e.getMessage());
        }
        return out;
    }

    private String expiryDate(UserAccount u) {
        int orgId  = u.getOrganizationId() == null ? 0 : u.getOrganizationId();
        int compId = u.getCompanyId() == null ? 0 : u.getCompanyId();

        List<Map<String, Object>> rows = repo.storedLicense(orgId, compId);
        if (rows.isEmpty()) {
            throw new IllegalStateException("License Not Found!");   // BLL 0087's own message
        }
        Object stored = rows.get(0).get("LicenseKey");
        String key = stored == null ? "" : String.valueOf(stored);
        if (!key.isEmpty()) {
            key = returnKey(key);
        }
        Map<String, Object> e = repo.expiry(key);
        Object d = e == null ? null : e.get("LicenseDate");
        return d == null ? null : String.valueOf(d);
    }

    // ================================================================================== write

    /**
     * btnSubmit_Click, in its order:
     *   empty box            -> "Enter License Please"
     *   CLV.SaveLicense      -> ReturnKey then Insert_CLV
     *   success              -> "License Updated Successfully...", then FillDate()
     */
    @Transactional
    public Map<String, Object> submit(String license) {
        UserAccount u = context.requireAccountingUser();

        String text = license == null ? "" : license;
        if (text.isEmpty()) {
            throw new IllegalArgumentException("Enter License Please");
        }
        String trimmed = text.trim();                       // txtLicense.Text.Trim()

        int orgId  = u.getOrganizationId() == null ? 0 : u.getOrganizationId();
        int compId = u.getCompanyId() == null ? 0 : u.getCompanyId();

        /* BLL 0061: the converted key is derived BEFORE the insert, so a key that will not
           decrypt is refused here with "License Key Not Match..." and never reaches the
           database - exactly the desktop's order. */
        String converted = returnKey(trimmed);

        repo.insert(trimmed, orgId, compId, converted);

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("message", "License Updated Successfully...");
        out.put("licenseDate", expiryDate(u));              // FillDate()
        return out;
    }

    // ================================================================================== crypto

    /** Architecture.Common.LicenseKey.ReturnKey. */
    public static String returnKey(String key) {
        try {
            byte[] cipherText = Base64.getDecoder().decode(key);
            byte[][] ki = deriveKeyAndIv();
            Cipher cipher = Cipher.getInstance("AES/CBC/PKCS5Padding");   // == .NET PKCS7 for AES
            cipher.init(Cipher.DECRYPT_MODE,
                    new SecretKeySpec(ki[0], "AES"), new IvParameterSpec(ki[1]));
            byte[] plain = cipher.doFinal(cipherText);
            return new String(plain, StandardCharsets.UTF_16LE);          // Encoding.Unicode
        } catch (Exception e) {
            /* catch (Exception) { throw new Exception("License Key Not Match..."); } */
            throw new IllegalArgumentException("License Key Not Match...");
        }
    }

    private static byte[][] deriveKeyAndIv() throws Exception {
        SecretKeyFactory f = SecretKeyFactory.getInstance("PBKDF2WithHmacSHA1");
        SecretKey dk = f.generateSecret(
                new PBEKeySpec(PASSWORD.toCharArray(), SALT, ITERATIONS, 48 * 8));
        byte[] b = dk.getEncoded();
        return new byte[][] { Arrays.copyOfRange(b, 0, 32), Arrays.copyOfRange(b, 32, 48) };
    }
}
