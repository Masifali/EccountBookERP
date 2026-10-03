package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import java.util.List;
import java.util.Map;

import static com.mst.repositories.support.DesktopProc.params;

/**
 * Data layer of two pop-up forms opened from 952 GD / Bank Invoice Mapping (Architecture.WinApp.Export):
 *
 * GdContainerBreakUp (BLL 0453 GDBreakUpHeader, DAL 0505)
 *   usp_getGdsForGdContainerBreakUp           @CompanyId [@Id when GdIdFromBreakUp != 0]   Gd No combo (Id, GDNO, ExImInvoiceId, InvoiceNo)
 *   usp_getGdContainerBreakUpAgainstGd        @CompanyId @Id                               grid on CmbGdNo Leave
 *   usp_DeleteGdContainerBreakUpAgainstGd     @Id (= first row's GdId)                     SetDataGdContainerBreakUp, then
 *   USP_GdContainerBreakup_Insert             the 7 model properties per row (SetProc), one transaction
 *
 * frmFIOpening (BLL 0445 FIOpening, DAL 0498; BLL 0057 Bank; BLL 0076 MultiCurrency; GlobalServicesMethods)
 *   Sp_FIOpening_ReadAll                      @OrganizationId @CompanyId @Activity='ReadAll'      history grid
 *   USP_FIOpening_Insert                      the 19 non-virtual model properties (SetProc)
 *   Sp_MultiCurrency_GetAllMethod             @OrganizationId @CompanyId @Activity='ReadAll'      Fcy Code combo
 *   Sp_Bank_GetAllMethod                      @OrganizationId @CompanyId @Activity='ReadAll'      Bank combo (Home Country only)
 *   USP_GetVendorsAndCustomersWithCityName    @OrganizationId @CompanyId                        clsGlobalVariables.globalAllSupplierCustomer (Consignee: CustomerGroupId 7)
 *
 * No table, column or procedure is created or changed.
 */
@Repository
public class ExportGdFiPopupsRepository {

    private final JdbcTemplate jdbc;

    public ExportGdFiPopupsRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    // ------------------------------------------------------------------ GdContainerBreakUp

    public List<Map<String, Object>> gdsForContainerBreakUp(UserAccount u, int recId) {
        Map<String, Object> p = params("CompanyId", u.getCompanyId());
        if (recId != 0) p.put("Id", recId);
        return DesktopProc.rows(jdbc, "[dbo].[usp_getGdsForGdContainerBreakUp]", p);
    }

    public List<Map<String, Object>> containerBreakUpAgainstGd(UserAccount u, int gdId) {
        return DesktopProc.rows(jdbc, "[dbo].[usp_getGdContainerBreakUpAgainstGd]", params("CompanyId", u.getCompanyId(), "Id", gdId));
    }

    /**
     * DAL GDBreakUpHeader.SetDataGdContainerBreakUp: empty list -> "Container BreakUp List not found"; delete
     * every row of the FIRST item's GdId; insert each item; commit; the last insert's Id.
     */
    @Transactional(rollbackFor = Exception.class)
    public int saveContainerBreakUp(List<Map<String, Object>> items) {
        if (items == null || items.isEmpty()) throw new IllegalStateException("Container BreakUp List not found");
        DesktopProc.rows(jdbc, "usp_DeleteGdContainerBreakUpAgainstGd", params("Id", items.get(0).get("GdId")));
        int result = 0;
        for (Map<String, Object> item : items) {
            Integer v = DesktopProc.scalar(jdbc, "USP_GdContainerBreakup_Insert", item);
            result = v == null ? 0 : v;
        }
        return result;
    }

    // ------------------------------------------------------------------ frmFIOpening

    public List<Map<String, Object>> fiOpeningHistory(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_FIOpening_ReadAll",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll"));
    }

    /** DAL FIOpening.SetData: the proc's scalar, or obj.Id when it returns nothing / 0; rollback on error. */
    @Transactional(rollbackFor = Exception.class)
    public int saveFiOpening(Map<String, Object> model) {
        Integer v = DesktopProc.scalar(jdbc, "USP_FIOpening_Insert", model);
        int num = v == null ? 0 : v;
        return num > 0 ? num : (Integer) model.get("Id");
    }

    public List<Map<String, Object>> currencies(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_MultiCurrency_GetAllMethod",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll"));
    }

    public List<Map<String, Object>> banks(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_Bank_GetAllMethod",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll"));
    }

    /** GlobalServicesMethods.getGlobalSupplierCustomer(org, comp, 0, 0, 0, "") - optional parameters omitted. */
    public List<Map<String, Object>> supplierCustomers(UserAccount u) {
        return DesktopProc.rows(jdbc, "USP_GetVendorsAndCustomersWithCityName",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }
}
