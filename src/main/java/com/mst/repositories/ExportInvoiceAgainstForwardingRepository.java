package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.support.DesktopProc.params;

/**
 * "Export Invoice Against Forwarding" - Architecture.WinApp.Export.ExportInvoiceAgainstForwarding (InvSaleInvoice with
 * DocumentTypeId 208) and its loader LoadExportForwarding. The sale-invoice data layer (BLL 0580 / DAL 0433, SaleInvoiceFinancial
 * MakeVoucherForSaleInvoice) is the one SaleInvoiceRepository / SaleInvoiceFinancial already implement and is reused; this
 * class holds the form's own reads (procdure.utf8.sql, 03-Oct-2026):
 *
 *   Sp_SupplierCustomer_GetAllMethod           @OrganizationId @CompanyId @Activity='ReadByOrganizationCompanyIdForExport'   SupplierNameFilll
 *   Sp_SupplierCustomer_GetAllMethod           @OrganizationId @CompanyId @CustomerGroupIds='7' @Activity='GetSupplierustomerByCustomerGroupId'  loader party
 *   Sp_ExImForwarding_GetAllMethod             @OrganizationId @CompanyId @DocumentTypeId=206 @Activity='GetItemsFromForwarding'   loader item
 *   USP_EximforwardingPendingForSaleInvoice    @OrganizationId @CompanyId @DocumentTypeId=206 @FinancialYearId [@SupplierCustomerId]
 *                                              [@Ids] [@FromDate] [@ToDate]                                     loader grid / LoadDataDetailGridAgainstForwarding
 *   [Sp_InvSaleInvoice_GetAllMethod]           FormHistory: @OrganizationId @CompanyId @DocumentTypeId=208 @CanViewAllRecord
 *                                              @FinancialYearId [@NoOfRecords] [@EntryUser] @Activity='FormHistory'    GetAll
 *   USP_GetStockByFifoMethod                   @OrganizationId @CompanyId @ItemId @DocDate [@PackUomId] [@WarehouseId] [@JobLotId]
 *                                              [@PackingTypeId] [@CropYear] [@DocumentTypeId @Id on update] [@FIFOXML]   CommonServices.FIFOImplemention
 */
@Repository
public class ExportInvoiceAgainstForwardingRepository {

    private final JdbcTemplate jdbc;

    public ExportInvoiceAgainstForwardingRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    public List<Map<String, Object>> customersForExport(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_SupplierCustomer_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadByOrganizationCompanyIdForExport"));
    }

    public List<Map<String, Object>> partiesByGroup(UserAccount u, String groupIds) {
        return DesktopProc.rows(jdbc, "Sp_SupplierCustomer_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "CustomerGroupIds", groupIds, "Activity", "GetSupplierustomerByCustomerGroupId"));
    }

    public List<Map<String, Object>> itemsFromForwarding(UserAccount u, int documentTypeId) {
        return DesktopProc.rows(jdbc, "Sp_ExImForwarding_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocumentTypeId", documentTypeId, "Activity", "GetItemsFromForwarding"));
    }

    /** ExImForwarding.forwardingPendingForSaleInvoice: SupplierCustomerId when != 0, Ids when not empty, dates when set. */
    public List<Map<String, Object>> forwardingPending(UserAccount u, int financialYearId, int supplierCustomerId,
                                                       String ids, Timestamp from, Timestamp to) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocumentTypeId", 206, "FinancialYearId", financialYearId);
        if (supplierCustomerId != 0) p.put("SupplierCustomerId", supplierCustomerId);
        if (ids != null && !ids.isEmpty()) p.put("Ids", ids);
        if (from != null) p.put("FromDate", from);
        if (to != null) p.put("ToDate", to);
        return DesktopProc.rows(jdbc, "USP_EximforwardingPendingForSaleInvoice", p);
    }

    /** InvSaleInvoice.FormHistory as GetAll(NoOfRecords) fills it: no dates, no doc numbers, no customer. */
    public List<Map<String, Object>> history(UserAccount u, int financialYearId, boolean canViewAll, int noOfRecords) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocumentTypeId", 208, "CanViewAllRecord", canViewAll);
        if (financialYearId != 0) p.put("FinancialYearId", financialYearId);
        if (noOfRecords != 0) p.put("NoOfRecords", noOfRecords);
        if (!canViewAll) p.put("EntryUser", u.getId());
        p.put("Activity", "FormHistory");
        return DesktopProc.rows(jdbc, "[Sp_InvSaleInvoice_GetAllMethod]", p);
    }

    /** CommonServices.FIFOImplemention's procedure call. */
    public List<Map<String, Object>> stockByFifo(UserAccount u, int itemId, Timestamp docDate, int packUomId, int warehouseId,
                                                 int jobLotId, int packingTypeId, String cropYear, int documentTypeId, int id, String fifoXml) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "ItemId", itemId, "DocDate", docDate);
        if (packUomId != 0) p.put("PackUomId", packUomId);
        if (warehouseId != 0) p.put("WarehouseId", warehouseId);
        if (jobLotId != 0) p.put("JobLotId", jobLotId);
        if (packingTypeId != 0) p.put("PackingTypeId", packingTypeId);
        if (cropYear != null && !cropYear.isEmpty()) p.put("CropYear", cropYear);
        if (documentTypeId != 0) p.put("DocumentTypeId", documentTypeId);
        if (id != 0) p.put("Id", id);
        if (fifoXml != null) p.put("FIFOXML", fifoXml);
        return new ArrayList<>(DesktopProc.rows(jdbc, "USP_GetStockByFifoMethod", p));
    }
}
