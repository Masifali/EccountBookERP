package com.mst.services.partyprocessing;

import com.mst.models.UserAccount;

import java.util.Map;

import static com.mst.services.hrm.HrmSupport.toInt;

/**
 * Tenancy guard for ids the page sends back: the desktop reads a record by id only (ReadById procedures take
 * @Id alone), so the web port additionally refuses a row whose OrganizationId / CompanyId (when the procedure
 * returns them) is not the signed-in user's.
 */
public final class PpCScreens {

    private PpCScreens() { }

    public static boolean owns(UserAccount u, Map<String, Object> row) {
        if (row == null) return false;
        if (row.containsKey("OrganizationId") && toInt(row.get("OrganizationId")) != toInt(u.getOrganizationId())) return false;
        if (row.containsKey("CompanyId") && toInt(row.get("CompanyId")) != toInt(u.getCompanyId())) return false;
        return true;
    }
}
