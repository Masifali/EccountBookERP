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
 * 907 "Multi Invoices Allocate To Gdn" - Architecture.WinApp.Export.frmMultiInvoicesAllocateToGdn (DocumentTypeId 2).
 * Data layer: Architecture.BLL.Export.GDBreakUpHeader (BLL 0453 / DAL 0505, models 0814 + 0813), ExImInvoice (BLL 0467),
 * Bank.GetAll (BLL 0057). Procedures:
 *
 *   Sp_ConfigrationsAllocation_GetAllMethod 'GetConfigurationByOrgCompandConfigDescription'   DefaultDaysToLessFromHistoryFromDate
 *   Sp_Bank_GetAllMethod 'ReadAll'                                 @OrganizationId @CompanyId @Activity           (form keeps IsHomeland == 'Home Country')
 *   USP_GetBankInvoicesForGdBreakUp                                @OrganizationId @CompanyId [@Id = RecId]       (Id, InvoiceNo, BankInvoiceAmount)
 *   [dbo].[USP_GetDataForDropDownFromGDBreakUpHeader]              @OrganizationId @CompanyId @Activity='Bank'    (Id, name)
 *   USP_GDBreakUpHeader_GetAllMethod 'ReadById' / 'ReadDetailByHeaderId'   @Id @Activity
 *   USP_GDBreakUpHeader_GetAllMethod 'FormHistory'                 @OrganizationId @CompanyId [dates] [@Id] [@BankId] @Activity
 *   USP_GDBreakUpHeader_Insert                                     the 27 non-virtual header properties (SetProc; ActionId 1 insert / 2 update)
 *   USP_GDBreakUpDetail_Insert                                     the 6 detail properties, one call per row (removed rows first, ActionTypeId 3)
 *   USP_GDBreakUpHeaderValidation                                  @Id                                            (DAL, after the rows, same transaction)
 */
@Repository
public class ExportMultiInvoicesAllocateRepository {

    private final JdbcTemplate jdbc;

    public ExportMultiInvoicesAllocateRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    public String config(UserAccount u, String description) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_ConfigrationsAllocation_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "ConfigDescription", description, "Activity", "GetConfigurationByOrgCompandConfigDescription"));
        if (r.isEmpty()) return "";
        Object v = r.get(0).get("ConfigKey");
        return v == null ? "" : String.valueOf(v).trim();
    }

    public List<Map<String, Object>> banks(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_Bank_GetAllMethod",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll"));
    }

    public List<Map<String, Object>> bankInvoices(UserAccount u, int recId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (recId != 0) p.put("Id", recId);
        return DesktopProc.rows(jdbc, "USP_GetBankInvoicesForGdBreakUp", p);
    }

    public List<Map<String, Object>> historyBanks(UserAccount u) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetDataForDropDownFromGDBreakUpHeader]",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "Bank"));
    }

    public List<Map<String, Object>> byId(int id) {
        return DesktopProc.rows(jdbc, "USP_GDBreakUpHeader_GetAllMethod", params("Id", id, "Activity", "ReadById"));
    }

    public List<Map<String, Object>> detailsByHeaderId(int id) {
        return DesktopProc.rows(jdbc, "USP_GDBreakUpHeader_GetAllMethod", params("Id", id, "Activity", "ReadDetailByHeaderId"));
    }

    public List<Map<String, Object>> formHistory(UserAccount u, Map<String, Object> dates, int bankId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        p.putAll(dates);
        if (bankId != 0) p.put("BankId", bankId);
        p.put("Activity", "FormHistory");
        return DesktopProc.rows(jdbc, "USP_GDBreakUpHeader_GetAllMethod", p);
    }

    /** DAL SetData: header (0 back -> obj.Id), every detail row, USP_GDBreakUpHeaderValidation @Id; one transaction. */
    @Transactional(rollbackFor = Exception.class)
    public int save(Map<String, Object> header, List<Map<String, Object>> details) {
        int id = header.get("Id") == null ? 0 : ((Number) header.get("Id")).intValue();
        Integer n = DesktopProc.scalar(jdbc, "USP_GDBreakUpHeader_Insert", header);
        int headerId = (n == null || n == 0) ? id : n;
        for (Map<String, Object> d : details) {
            d.put("GDBreakUpHeaderId", headerId);
            DesktopProc.scalar(jdbc, "USP_GDBreakUpDetail_Insert", d);
        }
        DesktopProc.rows(jdbc, "USP_GDBreakUpHeaderValidation", params("Id", headerId));
        return headerId;
    }
}
