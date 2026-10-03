package com.mst.repositories;

import com.mst.models.UserAccount;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.PreparedStatementCallback;
import org.springframework.stereotype.Repository;

import java.sql.ResultSet;
import java.sql.ResultSetMetaData;
import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.TreeMap;

/**
 * Module 17 "Export Reports" - group N. One method per desktop BLL call of seven report forms; every procedure and
 * parameter was read from procdure_index.csv / procdure.utf8.sql. A bracketed parameter is one the BLL guards
 * (!= 0 / not null / date not null) and is left out when unset; a null value is never bound.
 *
 *  shared
 *   Sp_ConfigrationsAllocation_GetAllMethod @OrganizationId @CompanyId @ConfigDescription @Activity='GetConfigurationByOrgCompandConfigDescription'
 *                                              CommonServices.GetDecimalConfiguration (amount / rate / fcy-rate decimals)
 *   Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId @OrganizationId @CompanyId   clsGlobalVariables.ActiveYr (Start_Period / End_Period)
 *   Sp_ExImLcOrder_GetAllMethod             @OrganizationId @CompanyId @Activity='GetLcOrderNo'      CommonServices.GetLcOrderNo -> ExImLcOrder.GetLcOrderNo (BLL 0469 :349)
 *   Sp_ExImInvoice_GetAllMethod             @OrganizationId @CompanyId @Activity='InvoiceNo'         CommonServices.GetExportInvoiceNo -> ExImInvoice.GetInvoiceNo
 *   [dbo].[USP_GetDataForDropDownFromExportInvoice] @OrganizationId @CompanyId @DocumentTypeIds    ExImLcOrder.GetDataForDropDownFromExportInvoice (BLL 0469 :1665)
 *  237 frmExportContractSchedulePeriodic
 *   USp_ExImLcOrderShipmentSchedule_GetAllMethod @OrganizationId @CompanyId @Activity='GetLatestAttentiveLoadDate'  (BLL 0457 :283)
 *   Usp_ExportContractSchedulePeriodicA     @OrganizationId @CompanyId @BranchId [@FromDate] [@ToDate] [@PerFclTon]   (BLL 0457 :740) - 5 result sets
 *  248 ExportComparisonSummaryReport
 *   SpStaticColumnNames                     @Activity='ExportComparisonsSummery'                   GeneralReprots.StaticColumnNames
 *   Sp_SupplierCustomer_GetAllMethod        @OrganizationId @CompanyId @Activity='ReadByOrganizationCompanyIdForExport'
 *   Sp_Item_GetAllMethod                    @OrganizationId @CompanyId @Activity='GetExportItemsByOrganizationCompanyId'
 *   Sp_ItemCategory_GetAllMethod            @OrganizationId @CompanyId @Activity='ReadByOrganizationCompanyId'
 *   Sp_ItemType_GetAllMethod                @OrganizationId @CompanyId @Activity='ReadByOrganizationCompanyId'
 *   Sp_SeaPorts_GetAllMethod                @OrganizationId @CompanyId @Activity='ReadByCompanyNOrganizationId'
 *   SpExImInvoice_ExportComparisonsSummery_Report @OrganizationId @CompanyId @ReportType [@FromDate] [@ToDate] [@ItemCategoryId] [@ItemTypeId]
 *                                           [@ItemId] [@SupplierCustomerId] [@LcContractId] [@DetinationPortId]  ExportReports.ExImInvoice_ExportComparisonsSummery_Report (BLL 0138 :593)
 *  259 CommercialInvoiceAgainstForwardingPreInvoices
 *   USP_ExportInvoiceAgainstForwarding_Register @OrganizationId @CompanyId [@ContractDateFrom] [@ContractDateTo] [@CustomerId] [@ExImLcOrderId]
 *                                           [@ItemTypeId] [@ItemId]                                  ExImLcOrder.ExportInvoiceAgainstForwarding_Register (BLL 0469 :1537)
 *  264 frmBillofLadingSlipandRegister
 *   USP_GetDataForDropDownFromBillOfLading  @OrganizationId @CompanyId                              ExImBillOfLading.GetDataForDropDownFromBillOfLading (BLL 0460)
 *   Sp_ExImBillOfLading_SlipAndRegister_Rpt @OrganizationId @CompanyId [@ExImInvoiceId] [@SupplierCustomerId] [@ShippingLineId]   ExImBillOfLading.ExImBillOfLading_SlipAndRegister
 *  268 ExImContainerList
 *   SpExportDeliveryOrderInvoiceGatepassRegister_rpt @OrganizationId @CompanyId [@ContractId] [@ExportInvoiceId]   ExImLcOrder.ExportDeliveryOrderInvoiceGatepassRegister (BLL 0469 :1113)
 *  271 PreInvoiceRegister
 *   USP_ExportDetailByContract_Report       @OrganizationId @CompanyId [@ContractDateFrom] [@ContractDateTo] [@CustomerId] [@ExImLcOrderId]
 *                                           [@ItemTypeId] [@ItemId] [@ActionId]                      ExImLcOrder.ExportShipmentDetailReportByContract (BLL 0469 :1417)
 *   SpExImInvoice_ExportInvoiceRegisterByLcContract_Rpt @OrganizationId @CompanyId [@LcContractId] [@ActionId]   ExportPdfReport.ExpRptSalesContractExportRegister_523 (row 523-Print check)
 *  272 BankGdSummary
 *   USP_BankGdsSummary                      @OrganizationId @CompanyId @BranchId @FromToBalance       ExportReports.BankGdSummary (BLL 0138 :1247)
 *   USP_GetGdBreakUpsByGdId                 @Id @DocumentTypeId                                       ExImFCBankReceipts.GetGdBreakUpsByGdId (BLL 0464 :2003) - frmGDBreakUp popup
 *
 * Nothing here creates or changes a table, column or procedure; every call is a read.
 */
@Repository
public class ExportReportsNRepository {

    private final JdbcTemplate jdbc;

    public ExportReportsNRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    // ================================================================================ shared

    /** GlobalVariables / CommonServices.GetConfigurationByOrgCompandConfigDescription - ConfigKey, "" when absent. */
    public String config(UserAccount u, String description) {
        List<Map<String, Object>> r = first("Sp_ConfigrationsAllocation_GetAllMethod", p(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "ConfigDescription", description, "Activity", "GetConfigurationByOrgCompandConfigDescription"));
        if (r.isEmpty()) return "";
        Object v = ci(r.get(0), "ConfigKey");
        return v == null ? "" : String.valueOf(v).trim();
    }

    /** clsGlobalVariables.ActiveYr - the active year row with the session's FinancialYearId (else the first). */
    public Map<String, Object> activeYear(UserAccount u, int financialYearId) {
        List<Map<String, Object>> years = first("Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId",
                p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
        for (Map<String, Object> r : years) {
            Object id = ci(r, "Id");
            if (id instanceof Number && ((Number) id).intValue() == financialYearId) return r;
        }
        return years.isEmpty() ? null : years.get(0);
    }

    /** ExImLcOrder.GetLcOrderNo (Id, LcOrderNo, ...) - FinancialYearId 0 so it is not sent. */
    public List<Map<String, Object>> lcOrderNos(UserAccount u) {
        return first("Sp_ExImLcOrder_GetAllMethod", p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "GetLcOrderNo"));
    }

    /** ExImInvoice.GetInvoiceNo (Id, InvoiceNo) - SupplierCustomerId 0 / FinancialYearId 0 so neither is sent. */
    public List<Map<String, Object>> exportInvoiceNos(UserAccount u) {
        return first("Sp_ExImInvoice_GetAllMethod", p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "InvoiceNo"));
    }

    /** ExImLcOrder.GetDataForDropDownFromExportInvoice (Id, name, ActivityType) - @Activity null (not sent), @DocumentTypeIds as given. */
    public List<Map<String, Object>> dropDownFromExportInvoice(UserAccount u, String documentTypeIds) {
        return first("[dbo].[USP_GetDataForDropDownFromExportInvoice]",
                p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "DocumentTypeIds", documentTypeIds));
    }

    // ================================================================================ 237

    /** ExImLcOrderShipmentSchedule.GetLatestAttentiveLoadDate - Rows[0][0], null when no row (the BLL then keeps DateTime.Now). */
    public Object latestAttentiveLoadDate(UserAccount u) {
        List<Map<String, Object>> r = first("USp_ExImLcOrderShipmentSchedule_GetAllMethod",
                p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "GetLatestAttentiveLoadDate"));
        if (r.isEmpty() || r.get(0).isEmpty()) return null;
        return r.get(0).values().iterator().next();
    }

    /**
     * ExImLcOrderShipmentSchedule.ExportContractSchedulePeriodicA - Usp_ExportContractSchedulePeriodicA, 5 result sets:
     * [0] week buckets, [1] party, [2] port, [3] item, [4] schedule month. @BranchId is always sent (UserAccount.BranchesId);
     * @PerFclTon is NetWeight, never set by the form, so it is not sent.
     */
    public List<List<Map<String, Object>>> contractSchedulePeriodicA(UserAccount u, Timestamp from, Timestamp to) {
        Map<String, Object> m = p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "BranchId", u.getBranchesId() == null ? 0 : u.getBranchesId());
        if (from != null) m.put("FromDate", from);
        if (to != null) m.put("ToDate", to);
        return sets("Usp_ExportContractSchedulePeriodicA", m);
    }

    // ================================================================================ 248

    /** GeneralReprots.StaticColumnNames(Activity) - SpStaticColumnNames sends only @Activity. */
    public List<Map<String, Object>> staticColumnNames(String activity) {
        return first("SpStaticColumnNames", p("Activity", activity));
    }

    /** SupplierCustomer.ReadByOrganizationCompanyIdForExport (Id, CompanyName). */
    public List<Map<String, Object>> exportCustomers(UserAccount u) {
        return first("Sp_SupplierCustomer_GetAllMethod",
                p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadByOrganizationCompanyIdForExport"));
    }

    /** Item.GetExportItemsByOrganizationCompanyId (Id, ItemName). */
    public List<Map<String, Object>> exportItems(UserAccount u) {
        return first("Sp_Item_GetAllMethod",
                p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "GetExportItemsByOrganizationCompanyId"));
    }

    /** ItemCategory.Getall (Id, CategoryDescription) - CategoryCode "" so no category filter is sent. */
    public List<Map<String, Object>> itemCategories(UserAccount u) {
        return first("Sp_ItemCategory_GetAllMethod",
                p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadByOrganizationCompanyId"));
    }

    /** ItemType.Getall (Id, TypeDescription) - Type 0 and no parent categories. */
    public List<Map<String, Object>> itemTypes(UserAccount u) {
        return first("Sp_ItemType_GetAllMethod",
                p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadByOrganizationCompanyId"));
    }

    /** SeaPorts.Getall (Id, PortName). */
    public List<Map<String, Object>> seaPorts(UserAccount u) {
        return first("Sp_SeaPorts_GetAllMethod",
                p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadByCompanyNOrganizationId"));
    }

    /**
     * ExportReports.ExImInvoice_ExportComparisonsSummery_Report - all result sets ([0] rows, [1] company logo).
     * @ReportType is always sent right after the tenancy pair; the rest only when set.
     */
    public List<List<Map<String, Object>>> comparisonsSummary(UserAccount u, String reportType, Timestamp from, Timestamp to,
                                                              int itemCategoryId, int itemTypeId, int itemId, int supplierCustomerId,
                                                              int lcContractId, int destinationPortId) {
        Map<String, Object> m = p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "ReportType", reportType);
        if (from != null) m.put("FromDate", from);
        if (to != null) m.put("ToDate", to);
        if (itemCategoryId != 0) m.put("ItemCategoryId", itemCategoryId);
        if (itemTypeId != 0) m.put("ItemTypeId", itemTypeId);
        if (itemId != 0) m.put("ItemId", itemId);
        if (supplierCustomerId != 0) m.put("SupplierCustomerId", supplierCustomerId);
        if (lcContractId != 0) m.put("LcContractId", lcContractId);
        if (destinationPortId != 0) m.put("DetinationPortId", destinationPortId);
        return sets("SpExImInvoice_ExportComparisonsSummery_Report", m);
    }

    // ================================================================================ 259 / 271

    /** ExImLcOrder.ExportInvoiceAgainstForwarding_Register (first table). @BranchesId / @ProjectsId / doc-no range are never set by the form. */
    public List<Map<String, Object>> invoiceAgainstForwardingRegister(UserAccount u, Timestamp from, Timestamp to, int customerId,
                                                                      int lcOrderId, int itemTypeId, int itemId) {
        Map<String, Object> m = p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        contractFilters(m, from, to, customerId, lcOrderId, itemTypeId, itemId);
        return first("USP_ExportInvoiceAgainstForwarding_Register", m);
    }

    /** ExImLcOrder.ExportShipmentDetailReportByContract (first table); @ActionId = ZeroBalanceType (1 when Skip Zero is ticked). */
    public List<Map<String, Object>> shipmentDetailByContract(UserAccount u, Timestamp from, Timestamp to, int customerId,
                                                              int lcOrderId, int itemTypeId, int itemId, int zeroBalanceType) {
        Map<String, Object> m = p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        contractFilters(m, from, to, customerId, lcOrderId, itemTypeId, itemId);
        if (zeroBalanceType != 0) m.put("ActionId", zeroBalanceType);
        return first("USP_ExportDetailByContract_Report", m);
    }

    private static void contractFilters(Map<String, Object> m, Timestamp from, Timestamp to, int customerId, int lcOrderId, int itemTypeId, int itemId) {
        if (from != null) m.put("ContractDateFrom", from);
        if (to != null) m.put("ContractDateTo", to);
        if (customerId != 0) m.put("CustomerId", customerId);
        if (lcOrderId != 0) m.put("ExImLcOrderId", lcOrderId);
        if (itemTypeId != 0) m.put("ItemTypeId", itemTypeId);
        if (itemId != 0) m.put("ItemId", itemId);
    }

    /** ExportPdfReport.ExpRptSalesContractExportRegister_523 with ExImLcOrderId + ActionId 1 (the 523-Print row button). */
    public List<Map<String, Object>> salesContractExportRegister523(UserAccount u, int lcContractId, int actionId) {
        Map<String, Object> m = p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (lcContractId != 0) m.put("LcContractId", lcContractId);
        if (actionId != 0) m.put("ActionId", actionId);
        return first("SpExImInvoice_ExportInvoiceRegisterByLcContract_Rpt", m);
    }

    // ================================================================================ 264

    /** ExImBillOfLading.GetDataForDropDownFromBillOfLading (Id, ReferenceName, Activity) - Activity null, not sent. */
    public List<Map<String, Object>> dropDownFromBillOfLading(UserAccount u) {
        return first("USP_GetDataForDropDownFromBillOfLading", p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    /**
     * ExImBillOfLading.ExImBillOfLading_SlipAndRegister. organizationId / companyId are passed explicitly because the desktop
     * ShowSlip() builds its ReportsParameters WITHOUT them (both 0 are still sent - the BLL does not guard them).
     */
    public List<Map<String, Object>> billOfLadingSlipAndRegister(int organizationId, int companyId, int invoiceId, int supplierCustomerId, int shippingLineId) {
        Map<String, Object> m = p("OrganizationId", organizationId, "CompanyId", companyId);
        if (invoiceId != 0) m.put("ExImInvoiceId", invoiceId);
        if (supplierCustomerId != 0) m.put("SupplierCustomerId", supplierCustomerId);
        if (shippingLineId != 0) m.put("ShippingLineId", shippingLineId);
        return first("Sp_ExImBillOfLading_SlipAndRegister_Rpt", m);
    }

    // ================================================================================ 268

    /** ExImLcOrder.ExportDeliveryOrderInvoiceGatepassRegister. */
    public List<Map<String, Object>> deliveryOrderInvoiceGatepassRegister(UserAccount u, int contractId, int invoiceId) {
        Map<String, Object> m = p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (contractId != 0) m.put("ContractId", contractId);
        if (invoiceId != 0) m.put("ExportInvoiceId", invoiceId);
        return first("SpExportDeliveryOrderInvoiceGatepassRegister_rpt", m);
    }

    // ================================================================================ 272

    /**
     * ExportReports.BankGdSummary - USP_BankGdsSummary. The BLL names the last parameter "@FromToBalance " (trailing space);
     * it is sent here as @FromToBalance with the double the form converts from the text box (0 when blank; the proc
     * turns 0 into NULL itself).
     */
    public List<Map<String, Object>> bankGdSummary(UserAccount u, double fromToBalance) {
        return first("USP_BankGdsSummary", p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "BranchId", u.getBranchesId() == null ? 0 : u.getBranchesId(), "FromToBalance", fromToBalance));
    }

    /** ExImFCBankReceipts.GetGdBreakUpsByGdId (frmGDBreakUp) - @Id, @DocumentTypeId. */
    public List<Map<String, Object>> gdBreakUpsByGdId(int gdId, int documentTypeId) {
        return first("USP_GetGdBreakUpsByGdId", p("Id", gdId, "DocumentTypeId", documentTypeId));
    }

    // ================================================================================ plumbing

    private static Map<String, Object> p(Object... kv) {
        Map<String, Object> m = new LinkedHashMap<>();
        for (int i = 0; i + 1 < kv.length; i += 2) m.put(String.valueOf(kv[i]), kv[i + 1]);
        return m;
    }

    /** GenericProvider.GetDataTableProc: the first result set (DataAdapter.Fill), update counts of temp-table work skipped. */
    public List<Map<String, Object>> first(String proc, Map<String, Object> m) {
        List<List<Map<String, Object>>> s = sets(proc, m);
        return s.isEmpty() ? new ArrayList<>() : s.get(0);
    }

    /** GenericProvider.GetDataSetProc: every result set, in order, as case-insensitive rows. A null value is never bound. */
    public List<List<Map<String, Object>>> sets(String proc, Map<String, Object> m) {
        List<Object> args = new ArrayList<>();
        StringBuilder sb = new StringBuilder("EXEC ");
        sb.append(proc.startsWith("[") ? proc : "dbo." + proc);
        boolean firstArg = true;
        for (Map.Entry<String, Object> e : m.entrySet()) {
            if (e.getValue() == null) continue;
            sb.append(firstArg ? " " : ", ").append('@').append(e.getKey()).append("=?");
            args.add(e.getValue());
            firstArg = false;
        }
        return jdbc.execute(sb.toString(), (PreparedStatementCallback<List<List<Map<String, Object>>>>) ps -> {
            for (int i = 0; i < args.size(); i++) ps.setObject(i + 1, args.get(i));
            List<List<Map<String, Object>>> out = new ArrayList<>();
            boolean isRs = ps.execute();
            while (true) {
                if (isRs) {
                    try (ResultSet rs = ps.getResultSet()) {
                        List<Map<String, Object>> t = new ArrayList<>();
                        ResultSetMetaData md = rs.getMetaData();
                        int n = md.getColumnCount();
                        while (rs.next()) {
                            Map<String, Object> row = new TreeMap<>(String.CASE_INSENSITIVE_ORDER);
                            for (int c = 1; c <= n; c++) {
                                String label = md.getColumnLabel(c);
                                if (label == null || label.isEmpty()) label = "Column" + c;
                                if (!row.containsKey(label)) row.put(label, rs.getObject(c));
                            }
                            t.add(row);
                        }
                        out.add(t);
                    }
                } else if (ps.getUpdateCount() == -1) {
                    break;
                }
                isRs = ps.getMoreResults();
            }
            return out;
        });
    }

    /** Case-insensitive column read. */
    public static Object ci(Map<String, Object> row, String name) {
        if (row == null) return null;
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet()) if (e.getKey().equalsIgnoreCase(name)) return e.getValue();
        return null;
    }

    /** Tables[i] of a data set, an empty list when the procedure returned fewer sets. */
    public static List<Map<String, Object>> table(List<List<Map<String, Object>>> s, int i) {
        return s != null && s.size() > i ? s.get(i) : new ArrayList<>();
    }
}
