package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.support.DesktopProc.params;

/**
 * Export report screens (App 8 "Export", module 133 "Export Customise Report" + 203 of module 11 +
 * 859 of module 136), agent C. One method per BLL call of the eleven desktop forms; every procedure
 * and every parameter name below was read from procdure.utf8.sql / procdure_index.csv on 30-Sep-2026.
 *
 *  241 FcyReceiptsSummaryRegister (Account_Reports)
 *    USP_GetDataForDropDownFromFcyBankReceipts @OrganizationId @CompanyId                 Sender/Receiver/Invoice/PaymentTerm combos (Activity column)
 *    USP_FcyReceiptsSummaryRegister  @OrganizationId @CompanyId [@FromDate] [@ToDate] [@SenderAccountId] [@ReceiverAccountId] [@InvoiceId] [@PaymentTerm]
 *  242 ShipmentdataForBrokeryTax
 *    usp_ShipmentdataForBrokeryTax  @OrganizationId @CompanyId @FromDate @ToDate
 *  243 PackingListRegister_Export
 *    USP_GetDataForDropDownFromExportPackingList @OrganizationId @CompanyId                 8 combos (ActivityType column)
 *    USP_PackingListRegister_Export @OrganizationId @CompanyId [@FromDate] [@ToDate] [@ItemId] [@InvoiceId] [@SupplierCustomerId] [@CropYearId] [@JobLotId] [@WarehouseId] [@PackingTypeId] [@DestinationPortId]
 *  244 PendingForwardingForCommercialInvoice
 *    USP_GetDataForDropDownFromGoodsForwarding @OrganizationId @CompanyId                   Proforma/Customer/Item (Activity column)
 *    Sp_MultiCurrency_GetAllMethod @OrganizationId @CompanyId @Activity='ReadAll'             Currency
 *    USP_getPackingExpiryFromForwarding @OrganizationId @CompanyId                            Packing Expiry Date
 *    USP_getProductionNoFromForwarding @OrganizationId @CompanyId                             Production No
 *    USP_GetPreInvoicesAndForwardingDateForCommercialInvoices @OrganizationId @CompanyId @BranchesId @FinancialYearId [@ContractId] [@Ids] [@SupplierCustomerId] [@FcurrencyId] [@ItemId] [@PackingExpiryDate] [@ProductionNo] [@SkipZero]
 *  245 GDBreakUpandRealized_Register
 *    [dbo].[USP_GetDataForDropDownFromExportGDBreakUps] @OrganizationId @CompanyId (@Activity omitted: CLR null)   Invoice/Bank (ActivityType column)
 *    USP_GDBreakUpandRealized_Register @OrganizationId @CompanyId [@FromDate] [@ToDate] [@BankId] [@InvoiceId] [@SkipZero]
 *  246 FIBalanceSummary (+ popup FIBalanceDetailByFI)
 *    usp_getFIBalanceSummary @OrganizationId @CompanyId [@SkipZero]
 *    usp_getFIBalanceDetailByFI @OrganizationId @CompanyId @Id
 *  256 CommissionAgentFcyLedger / 258 Commercial_Invoice_Shipments / 859 (combos)
 *    [dbo].[USP_GetDataForDropDownFromExportInvoice] @OrganizationId @CompanyId [@Activity] [@DocumentTypeIds]
 *    usp_CommissionAgentFcyLedger @OrganizationId @CompanyId @FromDate @ToDate [@CommissionAgentId]
 *    usp_CommercialInvoice_Shipments @OrganizationId @CompanyId [@FromDate] [@ToDate] [@SupplierCustomerId]
 *  257 EEReport_ExportGD
 *    [dbo].[USP_GetBanksFromGDBreakUps] @OrganizationId @CompanyId                            GD Bank (BankId/BankName)
 *    USP_GetDataForDropDownFromFcyBankReceipts @OrganizationId @CompanyId                     Realized Bank (Activity == 'RealizedBank_Gd')
 *    USP_EEReport_ExportGD @OrganizationId @CompanyId [@BankId] [@RealizedBankId] [@DateFrom] [@DateTo] @Activity
 *    USP_EEStatement_InsertAndUpdate  the 17 non-virtual EEStatement properties (SetProc)
 *  203 frmshippedConsignment_followup
 *    Sp_ExImInvoice_GetAllMethod @OrganizationId @CompanyId [@SupplierCustomerId] [@FinancialYearId] @Activity='InvoiceNo'
 *    Sp_ExImLookUps_GetAllMethod @Activity='ReadByOrganizationCompanyIdNExImLookUpTypeId' @OrganizationId @CompanyId [@ExImLookUptypesId]
 *    Sp_ExImShippedConsignmentFollowUps_GetAllMethod @Id @Activity ('ReadById' | 'GetIdByInvoiceId')
 *    Sp_ExImShippedConsignmentFollowUps_SlipAndRegister_Rpt @OrganizationId @CompanyId [@ExImInvoiceId] [@DocumentStatusIds]
 *    Sp_ExImShippedConsignmentFollowUps_Insert / sp_ExImShippedConsignmentFollowUps_Update  the 21 non-virtual model properties (SetProc)
 *    Proc_ExImExportShipingLineBooking_SlipAndRegister_Rpt @OrganizationId @CompanyId [@ExIminvoiceId]  (CRO slip data check)
 *  859 frmThirdPartyInspectionLotTrackingReport
 *    [USP_InvLabPreProductionExportLotInspection_LotInspectionNoFoCombo] @OrganizationId @CompanyId
 *    [dbo].[USP_ThirdPartyInspection_LotTrackingReport] @OrganizationId @CompanyId [@FromDate] [@ToDate] [@ContractScheduleId] [@LotTrackingId] [@CustomerId] [@ItemId] [@Status] [@PageNumber] [@PageSize] [@ActionId] [@SkipZero]
 *    SELECT * FROM [dbo].[V_LotTrackingCurrentStage]  (ThirdPartyType.LotTrackingCurrentStage - plain SQL text on the desktop too)
 *
 * A Java null is omitted from the EXEC (ADO.NET AddWithValue with a CLR null), never bound as NULL.
 * No table, column or procedure is created or changed.
 */
@Repository
public class ExportReportsCRepository {

    private final JdbcTemplate jdbc;

    public ExportReportsCRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    private Map<String, Object> base(UserAccount u) {
        return params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
    }

    /** clsGlobalVariables.ActiveYr.Start_Period - the chosen year's row of the login year list. */
    public String yearStart(UserAccount u, int yearId) {
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId", base(u))) {
            Object id = r.get("Id");
            int v = id instanceof Number ? ((Number) id).intValue() : 0;
            if (v == yearId) return r.get("Start_Period") == null ? "" : String.valueOf(r.get("Start_Period"));
        }
        return "";
    }

    // ------------------------------------------------------------------ 241

    /** ExImFCBankReceipts.GetDataForDropDownFromFcyBankReceipts(org, comp) - no Activity. */
    public List<Map<String, Object>> fcyReceiptDropDowns(UserAccount u) {
        return DesktopProc.rows(jdbc, "USP_GetDataForDropDownFromFcyBankReceipts", base(u));
    }

    /** ExImFCBankReceipts.FcyReceiptsSummaryRegister - BranchesId is never set by the form. */
    public List<Map<String, Object>> fcyReceiptsSummaryRegister(UserAccount u, java.sql.Date from, java.sql.Date to,
                                                                int senderId, int receiverId, int invoiceId, String paymentTerm) {
        Map<String, Object> p = base(u);
        if (from != null) p.put("FromDate", from);
        if (to != null) p.put("ToDate", to);
        if (senderId != 0) p.put("SenderAccountId", senderId);
        if (receiverId != 0) p.put("ReceiverAccountId", receiverId);
        if (invoiceId != 0) p.put("InvoiceId", invoiceId);
        if (paymentTerm != null && !paymentTerm.isEmpty()) p.put("PaymentTerm", paymentTerm);
        return DesktopProc.rows(jdbc, "USP_FcyReceiptsSummaryRegister", p);
    }

    // ------------------------------------------------------------------ 242

    /** ExImInvoice.ShipmentdataForBrokeryTax - both dates always sent. */
    public List<Map<String, Object>> shipmentDataForBrokeryTax(UserAccount u, java.sql.Date from, java.sql.Date to) {
        Map<String, Object> p = base(u);
        p.put("FromDate", from);
        p.put("ToDate", to);
        return DesktopProc.rows(jdbc, "usp_ShipmentdataForBrokeryTax", p);
    }

    // ------------------------------------------------------------------ 243

    /** ExportReports.DropDownFillFromPackingListRegister(org, comp) - no Activity. */
    public List<Map<String, Object>> packingListDropDowns(UserAccount u) {
        return DesktopProc.rows(jdbc, "USP_GetDataForDropDownFromExportPackingList", base(u));
    }

    /**
     * ExportReports.ExportPackingListRegister. The BLL names the crop parameter "@CropYearId " (trailing
     * blank); SqlClient trims parameter names, so the procedure's @CropYearId receives it - sent as
     * @CropYearId here.
     */
    public List<Map<String, Object>> packingListRegister(UserAccount u, java.sql.Date from, java.sql.Date to, int itemId, int invoiceId,
                                                         int customerId, int cropYearId, int jobLotId, int warehouseId, int packingTypeId, int destinationPortId) {
        Map<String, Object> p = base(u);
        if (from != null) p.put("FromDate", from);
        if (to != null) p.put("ToDate", to);
        if (itemId != 0) p.put("ItemId", itemId);
        if (invoiceId != 0) p.put("InvoiceId", invoiceId);
        if (customerId != 0) p.put("SupplierCustomerId", customerId);
        if (cropYearId != 0) p.put("CropYearId", cropYearId);
        if (jobLotId != 0) p.put("JobLotId", jobLotId);
        if (warehouseId != 0) p.put("WarehouseId", warehouseId);
        if (packingTypeId != 0) p.put("PackingTypeId", packingTypeId);
        if (destinationPortId != 0) p.put("DestinationPortId", destinationPortId);
        return DesktopProc.rows(jdbc, "USP_PackingListRegister_Export", p);
    }

    // ------------------------------------------------------------------ 244

    /** ExImForwarding.GetDataForDropDownFromGoodsForwarding(org, comp) - BranchesId 0 and Activity null: not sent. */
    public List<Map<String, Object>> forwardingDropDowns(UserAccount u) {
        return DesktopProc.rows(jdbc, "USP_GetDataForDropDownFromGoodsForwarding", base(u));
    }

    /** MultiCurrency.GetAll - Sp_MultiCurrency_GetAllMethod @Activity='ReadAll'. */
    public List<Map<String, Object>> currencies(UserAccount u) {
        Map<String, Object> p = base(u);
        p.put("Activity", "ReadAll");
        return DesktopProc.rows(jdbc, "Sp_MultiCurrency_GetAllMethod", p);
    }

    public List<Map<String, Object>> packingExpiryDates(UserAccount u) {
        return DesktopProc.rows(jdbc, "USP_getPackingExpiryFromForwarding", base(u));
    }

    public List<Map<String, Object>> productionNos(UserAccount u) {
        return DesktopProc.rows(jdbc, "USP_getProductionNoFromForwarding", base(u));
    }

    /** ExImInvoice.GetPreInvoicesAndForwardingDateForCommercialInvoices - BranchesId and FinancialYearId always sent. */
    public List<Map<String, Object>> pendingForwarding(UserAccount u, int branchesId, int financialYearId, int contractId, int customerId,
                                                       int fcyId, int itemId, String packingExpiryDate, String productionNo, int skipZero) {
        Map<String, Object> p = base(u);
        p.put("BranchesId", branchesId);
        p.put("FinancialYearId", financialYearId);
        if (contractId != 0) p.put("ContractId", contractId);
        if (customerId != 0) p.put("SupplierCustomerId", customerId);
        if (fcyId != 0) p.put("FcurrencyId", fcyId);
        if (itemId != 0) p.put("ItemId", itemId);
        if (packingExpiryDate != null && !packingExpiryDate.isEmpty()) p.put("PackingExpiryDate", packingExpiryDate);
        if (productionNo != null && !productionNo.isEmpty()) p.put("ProductionNo", productionNo);
        if (skipZero != 0) p.put("SkipZero", skipZero);
        return DesktopProc.rows(jdbc, "USP_GetPreInvoicesAndForwardingDateForCommercialInvoices", p);
    }

    // ------------------------------------------------------------------ 245

    /** ExImInvoice.GetDataForDropDownFromExportGdBreakup(org, comp, null) - the null Activity is not sent. */
    public List<Map<String, Object>> gdBreakUpDropDowns(UserAccount u) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetDataForDropDownFromExportGDBreakUps]", base(u));
    }

    /** ExportReports.GDBreakUpandRealized_Register (GetDataTableProcSetTime - same call, longer timeout). */
    public List<Map<String, Object>> gdBreakUpAndRealized(UserAccount u, java.sql.Date from, java.sql.Date to, int bankId, int invoiceId, int skipZero) {
        Map<String, Object> p = base(u);
        if (from != null) p.put("FromDate", from);
        if (to != null) p.put("ToDate", to);
        if (bankId != 0) p.put("BankId", bankId);
        if (invoiceId != 0) p.put("InvoiceId", invoiceId);
        if (skipZero != 0) p.put("SkipZero", skipZero);
        return DesktopProc.rows(jdbc, "USP_GDBreakUpandRealized_Register", p);
    }

    // ------------------------------------------------------------------ 246

    /** ExportReports.GetFIBalanceSummary - BranchesId is set on the object but the BLL never sends it. */
    public List<Map<String, Object>> fiBalanceSummary(UserAccount u, int skipZero) {
        Map<String, Object> p = base(u);
        if (skipZero != 0) p.put("SkipZero", skipZero);
        return DesktopProc.rows(jdbc, "usp_getFIBalanceSummary", p);
    }

    /** ExportReports.GetFIBalanceDetailByFI (popup FIBalanceDetailByFI). */
    public List<Map<String, Object>> fiBalanceDetailByFI(UserAccount u, int id) {
        Map<String, Object> p = base(u);
        p.put("Id", id);
        return DesktopProc.rows(jdbc, "usp_getFIBalanceDetailByFI", p);
    }

    // ------------------------------------------------------------------ 256 / 258 / 859 combos

    /** ExImLcOrder.GetDataForDropDownFromExportInvoice - @Activity / @DocumentTypeIds only when given. */
    public List<Map<String, Object>> exportInvoiceDropDowns(UserAccount u, String activity, String documentTypeIds) {
        Map<String, Object> p = base(u);
        if (activity != null && !activity.isEmpty()) p.put("Activity", activity);
        if (documentTypeIds != null && !documentTypeIds.isEmpty()) p.put("DocumentTypeIds", documentTypeIds);
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetDataForDropDownFromExportInvoice]", p);
    }

    /** ExImInvoice.CommissionAgentFcyLedger - both dates always sent. */
    public List<Map<String, Object>> commissionAgentFcyLedger(UserAccount u, java.sql.Date from, java.sql.Date to, int commissionAgentId) {
        Map<String, Object> p = base(u);
        p.put("FromDate", from);
        p.put("ToDate", to);
        if (commissionAgentId != 0) p.put("CommissionAgentId", commissionAgentId);
        return DesktopProc.rows(jdbc, "usp_CommissionAgentFcyLedger", p);
    }

    /** ExImInvoice.CommercialInvoiceShipments. */
    public List<Map<String, Object>> commercialInvoiceShipments(UserAccount u, java.sql.Date from, java.sql.Date to, int customerId) {
        Map<String, Object> p = base(u);
        if (from != null) p.put("FromDate", from);
        if (to != null) p.put("ToDate", to);
        if (customerId != 0) p.put("SupplierCustomerId", customerId);
        return DesktopProc.rows(jdbc, "usp_CommercialInvoice_Shipments", p);
    }

    // ------------------------------------------------------------------ 257

    /** GDBreakUpHeader.GetBanksForDropDownFromGDBreakUps. */
    public List<Map<String, Object>> gdBanks(UserAccount u) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetBanksFromGDBreakUps]", base(u));
    }

    /** GDBreakUpHeader.RealizedBankFromDropdown(org, comp) - no activity; the form keeps Activity == 'RealizedBank_Gd'. */
    public List<Map<String, Object>> realizedBankDropDowns(UserAccount u) {
        return DesktopProc.rows(jdbc, "USP_GetDataForDropDownFromFcyBankReceipts", base(u));
    }

    /** ExportReports.EEReport_ExportGD - @Activity 'Detail' | 'Summary' always sent. */
    public List<Map<String, Object>> eeReport(UserAccount u, java.sql.Date from, java.sql.Date to, int bankId, int realizedBankId, String activity) {
        Map<String, Object> p = base(u);
        if (bankId != 0) p.put("BankId", bankId);
        if (realizedBankId != 0) p.put("RealizedBankId", realizedBankId);
        if (from != null) p.put("DateFrom", from);
        if (to != null) p.put("DateTo", to);
        p.put("Activity", activity);
        return DesktopProc.rows(jdbc, "USP_EEReport_ExportGD", p);
    }

    /**
     * DAL EEStatement.SetData: every list item through USP_EEStatement_InsertAndUpdate in one
     * transaction (the form always sends exactly one item, the edited row).
     */
    @Transactional(rollbackFor = Exception.class)
    public int saveEeStatements(List<Map<String, Object>> items) {
        int result = 0;
        for (Map<String, Object> item : items) result = scalarOrZero("USP_EEStatement_InsertAndUpdate", item);
        return result;
    }

    // ------------------------------------------------------------------ 203

    /** CommonServices.GetExportInvoiceNo() -> ExImInvoice.GetInvoiceNo: SupplierCustomerId 0 / FinancialYearId 0 -> not sent. */
    public List<Map<String, Object>> exportInvoiceNos(UserAccount u) {
        Map<String, Object> p = base(u);
        p.put("Activity", "InvoiceNo");
        return DesktopProc.rows(jdbc, "Sp_ExImInvoice_GetAllMethod", p);
    }

    /** ExImLookUps.GetByLookTypeId(ExImLookUptypesId) - 5 = Document Status, 6 = Current Status. */
    public List<Map<String, Object>> lookUpsByType(UserAccount u, int lookUpTypeId) {
        Map<String, Object> p = params("Activity", "ReadByOrganizationCompanyIdNExImLookUpTypeId",
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (lookUpTypeId != 0) p.put("ExImLookUptypesId", lookUpTypeId);
        return DesktopProc.rows(jdbc, "Sp_ExImLookUps_GetAllMethod", p);
    }

    /** ExImShippedConsignmentFollowUps.GetByID(InvoiceId) - 'ReadById' filters R.ExImInvoiceId = @Id. */
    public List<Map<String, Object>> followUpByInvoiceId(int invoiceId) {
        return DesktopProc.rows(jdbc, "Sp_ExImShippedConsignmentFollowUps_GetAllMethod", params("Id", invoiceId, "Activity", "ReadById"));
    }

    /** ExImShippedConsignmentFollowUps.GetIdByInvoiceId - first cell of the first row, 0 when none. */
    public int followUpIdByInvoiceId(int invoiceId) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_ExImShippedConsignmentFollowUps_GetAllMethod",
                params("Id", invoiceId, "Activity", "GetIdByInvoiceId"));
        if (r.isEmpty()) return 0;
        Object v = r.get(0).values().iterator().next();
        return v instanceof Number ? ((Number) v).intValue() : toInt(v);
    }

    /** ExImShippedConsignmentFollowUps.ExImShippedConsignmentFollowUps_Slipandregister - invoice 0 / empty status not sent. */
    public List<Map<String, Object>> followUpRegister(UserAccount u, int invoiceId, String documentStatusIds) {
        Map<String, Object> p = base(u);
        if (invoiceId != 0) p.put("ExImInvoiceId", invoiceId);
        if (documentStatusIds != null && !documentStatusIds.isEmpty()) p.put("DocumentStatusIds", documentStatusIds);
        return DesktopProc.rows(jdbc, "Sp_ExImShippedConsignmentFollowUps_SlipAndRegister_Rpt", p);
    }

    /** BLL Save: Id == 0 -> Sp_ExImShippedConsignmentFollowUps_Insert, else sp_ExImShippedConsignmentFollowUps_Update; one transaction. */
    @Transactional(rollbackFor = Exception.class)
    public int saveFollowUp(Map<String, Object> model) {
        int id = toInt(model.get("Id"));
        return scalarOrZero(id == 0 ? "Sp_ExImShippedConsignmentFollowUps_Insert" : "sp_ExImShippedConsignmentFollowUps_Update", model);
    }

    /** DAL SetDataMultiple (MultiSave): each item, Insert when Id == 0 else Update, one transaction. */
    @Transactional(rollbackFor = Exception.class)
    public int saveFollowUps(List<Map<String, Object>> models) {
        int result = 0;
        for (Map<String, Object> m : models) {
            int id = toInt(m.get("Id"));
            int r = scalarOrZero(id == 0 ? "Sp_ExImShippedConsignmentFollowUps_Insert" : "sp_ExImShippedConsignmentFollowUps_Update", m);
            if (r > 0) result = r;
        }
        return result;
    }

    /** ExImExportShipingLineBooking.PrintSlipandRegister(org, comp, invoice) - the CRO slip's data (row-count check). */
    public List<Map<String, Object>> croSlipData(UserAccount u, int invoiceId) {
        Map<String, Object> p = base(u);
        if (invoiceId != 0) p.put("ExIminvoiceId", invoiceId);
        return DesktopProc.rows(jdbc, "Proc_ExImExportShipingLineBooking_SlipAndRegister_Rpt", p);
    }

    // ------------------------------------------------------------------ 859

    /** InvLabPreProductionExportLotInspectionHeader.GetData_LotInspectionNoForCombo. */
    public List<Map<String, Object>> lotTrackingNos(UserAccount u) {
        return DesktopProc.rows(jdbc, "[USP_InvLabPreProductionExportLotInspection_LotInspectionNoFoCombo]", base(u));
    }

    /** InvLabPreProductionExportLotInspectionHeader.ThirdPartyInspection_LotTrackingReport - BranchesId set on the object, never sent. */
    public List<Map<String, Object>> lotTrackingReport(UserAccount u, java.sql.Date from, java.sql.Date to, int customerId, int itemId,
                                                       long recId, int actionId, int skipZero) {
        Map<String, Object> p = base(u);
        if (from != null) p.put("FromDate", from);
        if (to != null) p.put("ToDate", to);
        if (recId != 0L) p.put("LotTrackingId", recId);
        if (customerId != 0) p.put("CustomerId", customerId);
        if (itemId != 0) p.put("ItemId", itemId);
        if (actionId != 0) p.put("ActionId", actionId);
        if (skipZero != 0) p.put("SkipZero", skipZero);
        return DesktopProc.rows(jdbc, "[dbo].[USP_ThirdPartyInspection_LotTrackingReport]", p);
    }

    /** ThirdPartyType.LotTrackingCurrentStage() - the desktop's own text query. */
    public List<Map<String, Object>> lotTrackingCurrentStages() {
        return jdbc.queryForList("SELECT * FROM [dbo].[V_LotTrackingCurrentStage]");
    }

    // ------------------------------------------------------------------ plumbing

    private int scalarOrZero(String proc, Map<String, Object> p) {
        Integer v = DesktopProc.scalar(jdbc, proc, p);
        return v == null ? 0 : v;
    }

    private static int toInt(Object o) {
        if (o instanceof Number) return ((Number) o).intValue();
        if (o == null) return 0;
        try { return (int) Double.parseDouble(String.valueOf(o).trim()); } catch (NumberFormatException e) { return 0; }
    }

    /** Case-insensitive column read for callers that project rows. */
    public static Object ci(Map<String, Object> row, String name) {
        if (row == null) return null;
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet()) if (e.getKey().equalsIgnoreCase(name)) return e.getValue();
        return null;
    }

    public static List<Map<String, Object>> copy(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) out.add(new LinkedHashMap<>(r));
        return out;
    }
}
