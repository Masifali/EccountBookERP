package com.mst.repositories;

import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.SqlParameterValue;
import org.springframework.stereotype.Repository;

import java.sql.Types;
import java.util.List;
import java.util.Map;

/**
 * System Utilities -> "License Key" (Architecture.WinApp.LicenseKey).
 *
 * ---------------------------------------------------------------------------------------------
 * THE DESKTOP CHAIN
 * ---------------------------------------------------------------------------------------------
 *   FillDate()
 *     BLL 0087 UserAccount.GetLicensCompanyAndExpiryDate(OrganizationId, CompanyId)
 *       1  Sp_UserAccount_GetAllMethod @OrganizationId, @CompanyId, @Activity='GetLicenseKey'
 *          -> SELECT * FROM dbo.tblCLV WHERE OrganizationId=@ AND CompanyId=@
 *          -> no rows  => throw "License Not Found!"
 *       2  the stored LicenseKey is run through Common.LicenseKey.ReturnKey (AES decrypt)
 *       3  USP_GetLicenseExpiryDateByLicense @LicenseKey=<decrypted>
 *          -> splits it on '(' into LicenseCompanyName and LicenseDate
 *
 *   btnSubmit_Click
 *     BLL 0061 CLV.SaveLicense(obj)
 *       ConvertedLicenseKey = ReturnKey(LicenseKey)      // bad key => "License Key Not Match..."
 *       DAL 0059 CLV.Insert -> [dbo].[Insert_CLV]
 *
 * Insert_CLV is the one that decides insert vs update, and it validates: when a row already
 * exists for the organization and company it takes the company name out of the DECRYPTED key
 * (everything left of the first '(') and refuses with RAISERROR 'Invalid Key!' unless a tblCLV
 * row already carries that CompanyName. That check is the procedure's, and is left to it.
 *
 * The DAL passes exactly four parameters by name - LicenseKey, OrganizationId, CompanyId,
 * ConvertedLicenseKey - and never @Id or @CompanyName, so neither is sent here either. Both
 * default to null in the procedure, and on the INSERT branch that is what makes the new row's
 * CompanyName null, exactly as on the desktop.
 */
@Repository
public class LicenseKeyRepository {

    private static final String P_GET_KEY = "Sp_UserAccount_GetAllMethod";
    private static final String P_EXPIRY  = "USP_GetLicenseExpiryDateByLicense";
    private static final String P_INSERT  = "Insert_CLV";

    private final JdbcTemplate jdbc;
    public LicenseKeyRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    /** Step 1 - the tblCLV row for this organization and company. */
    public List<Map<String, Object>> storedLicense(int organizationId, int companyId) {
        return jdbc.queryForList(
                "EXEC dbo." + P_GET_KEY + " @OrganizationId=?, @CompanyId=?, @Activity=?",
                organizationId, companyId, "GetLicenseKey");
    }

    /** Step 3 - the procedure splits the decrypted key; no parsing is done on this side. */
    public Map<String, Object> expiry(String decryptedKey) {
        List<Map<String, Object>> rows = jdbc.queryForList(
                "EXEC dbo." + P_EXPIRY + " @LicenseKey=?", decryptedKey);
        return rows.isEmpty() ? null : rows.get(0);
    }

    /**
     * DAL 0059 CLV.Insert. ExecuteScalar, so the value the procedure selects comes back - and a
     * RAISERROR ('Invalid Key!') surfaces as an exception, which is what the desktop shows.
     */
    public int insert(String licenseKey, int organizationId, int companyId, String convertedKey) {
        List<Map<String, Object>> rows = jdbc.queryForList(
                "EXEC dbo." + P_INSERT + " @LicenseKey=?, @OrganizationId=?, @CompanyId=?, @ConvertedLicenseKey=?",
                licenseKey,
                organizationId,
                companyId,
                convertedKey == null ? new SqlParameterValue(Types.NVARCHAR, null) : convertedKey);
        if (rows.isEmpty()) return 0;
        for (Object v : rows.get(0).values()) {
            if (v instanceof Number) return ((Number) v).intValue();
        }
        return 0;
    }
}
