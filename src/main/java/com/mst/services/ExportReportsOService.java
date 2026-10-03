package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.ExportReportsORepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.sql.Date;
import java.time.LocalDate;
import java.time.format.DateTimeParseException;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.LinkedHashSet;
import java.util.List;
import java.util.Map;
import java.util.Set;

import static com.mst.repositories.ExportReportsORepository.ci;

/**
 * Export Reports - group O: the BLL side of seven desktop report forms of Architecture.WinApp.ExportReports that have
 * NO dbo.ScreenDefinition row (they open from code: the Export dashboard cards, frmStockReservedAgainstThirdPartyInspection,
 * or nothing at all):
 *
 *   ExImCommercialInvoiceHistory    /export/commercial-invoice-history   (ExportDashboardHeaderInfo "Total Export" / "TotalExportByCurrentMonth")
 *   ExImShipmentScheduleHistory     /export/shipment-schedule-history
 *   ExportReportSummaries           /export/report-summaries
 *   PerformaInvoiceRegister         /export/performa-invoice-register
 *   frmExportInvoiceSummaryByMonth  /export/invoice-summary-by-month
 *   frmStockReservedRegister        /export/stock-reserved-register      (frmStockReservedAgainstThirdPartyInspection.BtnStockReservedRegisterForm)
 *   ExportContractReports           /export/contract-reports             (tab launcher)
 *
 * Rights: the desktop forms check no right themselves. DesktopReportRights.require(user, SCREEN_*, "View") is called
 * with the constants below; while no ScreenDefinition row exists they are 0, which has no CompanyRights / ScreenRights
 * rows, so the check only enforces an authenticated accounting user (set the real id once a row is added).
 * Organisation, company, branch, financial year and user always come from the session, never from the request.
 */
@Service
public class ExportReportsOService {

    /* No dbo.ScreenDefinition row exists for any of these forms (checked against the 05-Sep-2026 database script). */
    public static final int SCREEN_COMMERCIAL_INVOICE_HISTORY = 0;
    public static final int SCREEN_SHIPMENT_SCHEDULE_HISTORY = 0;
    public static final int SCREEN_REPORT_SUMMARIES = 0;
    public static final int SCREEN_PERFORMA_INVOICE_REGISTER = 0;
    public static final int SCREEN_INVOICE_SUMMARY_BY_MONTH = 0;
    public static final int SCREEN_STOCK_RESERVED_REGISTER = 0;
    public static final int SCREEN_CONTRACT_REPORTS = 0;

    @Autowired private ExportReportsORepository repo;
    @Autowired private CurrentUserContext currentUserContext;
    @Autowired private DesktopReportRights rights;
    @Autowired private DashboardModuleService dashboardModuleService;

    private UserAccount user(int screenId) {
        UserAccount u = currentUserContext.requireAccountingUser();
        rights.require(u, screenId, "View");
        return u;
    }

    private int branchId(UserAccount u) { return u.getBranchesId() == null ? 0 : u.getBranchesId(); }

    private int financialYearId() {
        try { return currentUserContext.currentFinancialYearId(); } catch (Exception e) { return 0; }
    }

    /** clsGlobalVariables.ActiveYr.Start_Period / End_Period as yyyy-MM-dd ("" when unavailable). */
    private Map<String, Object> year(UserAccount u) {
        Map<String, Object> m = new LinkedHashMap<>();
        String start = "", end = "";
        try {
            Map<String, Object> r = repo.activeYear(u, financialYearId());
            if (r != null) { start = iso(ci(r, "Start_Period")); end = iso(ci(r, "End_Period")); }
        } catch (Exception ignored) { /* the desktop would show the picker's default */ }
        m.put("yearStart", start);
        m.put("yearEnd", end);
        return m;
    }

    /** Id / name rows for a combo. */
    private static List<Map<String, Object>> combo(List<Map<String, Object>> rows, String idCol, String textCol) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", ci(r, idCol));
            m.put("name", text(ci(r, textCol)));
            out.add(m);
        }
        return out;
    }

    /** One loader result, or the desktop's MessageBox text of a failed loader (each loader has its own try/catch). */
    private interface Loader { Object load(); }
    private static void put(Map<String, Object> out, String key, Loader l, List<String> errors) {
        try { out.put(key, l.load()); }
        catch (Exception e) { out.put(key, new ArrayList<>()); errors.add(root(e)); }
    }

    // =============================================================================== ExImShipmentScheduleHistory

    /** Load: txtdateto = ActiveYr.End_Period; CustomerGetAll; ContractNoGetAll (then GridBind with the defaults). */
    public Map<String, Object> shipmentSetup() {
        UserAccount u = user(SCREEN_SHIPMENT_SCHEDULE_HISTORY);
        Map<String, Object> out = new LinkedHashMap<>(year(u));
        List<String> errors = new ArrayList<>();
        put(out, "customers", () -> combo(repo.customersForExport(u), "Id", "CompanyName"), errors);
        put(out, "contracts", () -> combo(repo.lcOrderNos(u), "Id", "LcOrderNo"), errors);
        out.put("errors", errors);
        return out;
    }

    /** btnnew_Click: ContractNoGetAll + CustomerGetAll. */
    public Map<String, Object> shipmentCombos() {
        UserAccount u = user(SCREEN_SHIPMENT_SCHEDULE_HISTORY);
        Map<String, Object> out = new LinkedHashMap<>();
        List<String> errors = new ArrayList<>();
        put(out, "customers", () -> combo(repo.customersForExport(u), "Id", "CompanyName"), errors);
        put(out, "contracts", () -> combo(repo.lcOrderNos(u), "Id", "LcOrderNo"), errors);
        out.put("errors", errors);
        return out;
    }

    private static final String[] SHIPMENT_COLS = { "LcOrderId", "Id", "LcOrderDocNo", "CustomerName", "ContractNo", "ContractDate",
            "DeliveryTerms", "ContractWeight", "ShippedWeight", "BalanceWeight", "LcStatus", "ItemName", "NoOfCntr", "ShpSchQtyMtons",
            "ShipmentSchDate", "ScheduleMonth", "PackingRemarks", "LotNo", "LineStatus", "DestinationPort" };

    /** GridBind(): ExportShipmentsHistory with contract (OrderId), customer and both dates; the grid is RetrieveStructure. */
    public List<Map<String, Object>> shipmentShow(Map<String, Object> b) {
        UserAccount u = user(SCREEN_SHIPMENT_SCHEDULE_HISTORY);
        return project(repo.shipmentsHistory(u, i(b, "contractId"), i(b, "customerId"), d(b, "fromDate"), d(b, "toDate")), SHIPMENT_COLS);
    }

    // =============================================================================== ExportReportSummaries

    /** Load: ReportTypeBind ('ExportSummary'), CustomerGetAll, ContractNoGetAll, ItemBind, SeaPortBind. */
    public Map<String, Object> summariesSetup() {
        UserAccount u = user(SCREEN_REPORT_SUMMARIES);
        Map<String, Object> out = new LinkedHashMap<>();
        List<String> errors = new ArrayList<>();
        put(out, "reportTypes", () -> combo(repo.staticColumnNames("ExportSummary"), "ReportType", "ReportName"), errors);
        put(out, "customers", () -> combo(repo.customersForExport(u), "Id", "CompanyName"), errors);
        put(out, "contracts", () -> combo(repo.lcOrderNos(u), "Id", "LcOrderNo"), errors);
        put(out, "items", () -> combo(repo.exportItems(u), "Id", "ItemName"), errors);
        put(out, "ports", () -> combo(repo.seaPorts(u), "Id", "PortName"), errors);
        out.put("errors", errors);
        return out;
    }

    /** The procedure's final SELECT order (the grid is RetrieveStructure; CompLogoImage is binary and is left out). */
    private static final String[] SUMMARY_PROC_COLS = { "Description", "FcyCode", "M_Tons", "FcyAmount", "PKR_Amount", "CompCountry",
            "CompContactPerson", "CompMobileA", "CompMobileB", "CompMobileC", "CompEmailA", "CompEmailB", "CompanyWebsite",
            "CompanyFaxNo", "OrgReportingRemarks" };

    /** GridBind(): ExportReports.ExportSummariesReport; ReportType = Conversion.ToString(CmbReportType.Value). */
    public List<Map<String, Object>> summariesShow(Map<String, Object> b) {
        UserAccount u = user(SCREEN_REPORT_SUMMARIES);
        return project(repo.exportSummaries(u, i(b, "customerId"), i(b, "portId"), i(b, "itemId"), 0, 0, i(b, "contractId"),
                d(b, "fromDate"), d(b, "toDate"), s(b, "reportType"), 0, 0), SUMMARY_PROC_COLS);
    }

    // =============================================================================== ExImCommercialInvoiceHistory

    /**
     * Load: DateTypeFill (fixed), ComboFill (Invoice / Customer / TradeType / FarmingType), ParameterFill (fixed),
     * ComboFillExportReport (ItemType / Items / ContractNo / Customer / ItemCategory / DestinationPort / TradeType /
     * FarmingType) and ReportTypeBind ('ExportDetailHistory'). Both ComboFill methods read the same procedure.
     */
    public Map<String, Object> invoiceHistorySetup() {
        UserAccount u = user(SCREEN_COMMERCIAL_INVOICE_HISTORY);
        Map<String, Object> out = new LinkedHashMap<>(year(u));
        List<String> errors = new ArrayList<>();
        put(out, "combos", () -> dropDownGroups(u), errors);
        put(out, "reportTypes", () -> reportTypes(), errors);
        out.put("errors", errors);
        return out;
    }

    /** btnRefreshInvoicesHistory (ComboFill) / btnRefreshExportHistory (ParameterFill, ReportTypeBind, ComboFillExportReport). */
    public Map<String, Object> invoiceHistoryCombos(boolean withReportTypes) {
        UserAccount u = user(SCREEN_COMMERCIAL_INVOICE_HISTORY);
        Map<String, Object> out = new LinkedHashMap<>();
        List<String> errors = new ArrayList<>();
        put(out, "combos", () -> dropDownGroups(u), errors);
        if (withReportTypes) put(out, "reportTypes", () -> reportTypes(), errors);
        out.put("errors", errors);
        return out;
    }

    /** SpStaticColumnNames 'ExportDetailHistory' - ReportType values repeat (2), so the row index keys the option. */
    private List<Map<String, Object>> reportTypes() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.staticColumnNames("ExportDetailHistory")) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("ReportType", ci(r, "ReportType"));
            m.put("ReportName", text(ci(r, "ReportName")));
            out.add(m);
        }
        return out;
    }

    /** The (Id, name, ActivityType) rows split per ActivityType, in procedure order. */
    private Map<String, Object> dropDownGroups(UserAccount u) {
        Map<String, Object> g = new LinkedHashMap<>();
        for (String k : new String[] { "Invoice", "Customer", "TradeType", "FarmingType", "ItemType", "Items", "ContractNo",
                "ItemCategory", "DestinationPort" }) g.put(k, new ArrayList<Map<String, Object>>());
        for (Map<String, Object> r : repo.dropDownFromExportInvoice(u)) {
            String t = text(ci(r, "ActivityType"));
            @SuppressWarnings("unchecked")
            List<Map<String, Object>> list = (List<Map<String, Object>>) g.get(t);
            if (list == null) continue;
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", ci(r, "Id"));
            m.put("name", text(ci(r, "name")));
            list.add(m);
        }
        return g;
    }

    private static final String[] REGISTER_COLS = { "Id", "InvoiceNo", "InvoiceDate", "CustomerName", "ContractNos", "ContractDates",
            "PaymentTerm", "FcyCode", "InvoiceValue", "FcyReceived", "FcyBalance" };

    /**
     * GridBindForInvoicesHistory(): EximInvoiceRegister - @InvoiceNo is cmbInvoiceNo.Text.Trim(), ActionId 0 / 1 / 2 from
     * All / Pending / Completed. The rows are projected to the form's own DataTable (InvoiceDate as ToShortDateString).
     */
    public List<Map<String, Object>> invoiceHistoryInvoices(Map<String, Object> b) {
        UserAccount u = user(SCREEN_COMMERCIAL_INVOICE_HISTORY);
        String invoiceNo = s(b, "invoiceNo").trim();
        return project(repo.invoiceRegister(u, d(b, "fromDate"), d(b, "toDate"), invoiceNo, i(b, "customerId"), i(b, "actionId"),
                i(b, "tradeTypeId"), i(b, "farmingTypeId")), REGISTER_COLS);
    }

    private static final String[][] DETAIL_COLS = { { "CustomGroupName", "CustomGroupName" }, { "CustomerName", "CustomerName" },
            { "DocCode", "DocCode" }, { "DocDate", "DocDate" }, { "InvoiceNo", "InvoiceNo" }, { "ContractNos", "ContractNos" },
            { "ContractDates", "ContractDates" }, { "ItemCategory", "ItemCategory" }, { "ItemType", "ItemType" }, { "ItemName", "ItemName" },
            { "CropYear", "CropYear" }, { "PackingType", "PackingType" }, { "FcyCode", "FcyCode" }, { "RatePrice", "RatePrice" },
            { "NoOfPack", "NoOfPack" }, { "PackSize", "PackSize" }, { "NetWeight", "NetWeight" }, { "FcAmount", "FcAmount" },
            { "M_Tons", "M_Tons" }, { "FarmingType", "FarmingTypeDesc" }, { "TradeType", "TradeTypeDesc" } };
    private static final Set<String> DETAIL_DOUBLES = new LinkedHashSet<>(java.util.Arrays.asList("RatePrice", "NoOfPack", "NetWeight", "FcAmount", "M_Tons"));

    /**
     * GridBindForExportHistoryReport(): "Summary By Customer / Item / Port" -> ExportSummariesReport (ReportType =
     * CmbReportType.Value); any other text -> ExportInvoiceHistoryDetailReport with @Activity = the text. Returns
     * {mode: "summary" | "detail", rows}; the page applies the per-text grid settings and the desktop's quirks.
     */
    public Map<String, Object> invoiceHistoryExport(Map<String, Object> b) {
        UserAccount u = user(SCREEN_COMMERCIAL_INVOICE_HISTORY);
        String activity = s(b, "activity").trim();
        Map<String, Object> out = new LinkedHashMap<>();
        if ("Summary By Customer".equals(activity) || "Summary By Item".equals(activity) || "Summary By Port".equals(activity)) {
            List<Map<String, Object>> rows = new ArrayList<>();
            for (Map<String, Object> r : repo.exportSummaries(u, i(b, "customerId"), i(b, "portId"), i(b, "itemId"), i(b, "itemTypeId"),
                    i(b, "itemCategoryId"), i(b, "contractId"), d(b, "fromDate"), d(b, "toDate"), s(b, "reportType"),
                    i(b, "tradeTypeId"), i(b, "farmingTypeId"))) {
                Map<String, Object> m = new LinkedHashMap<>();       // dtExportsummary column order
                m.put("Description", text(ci(r, "Description")));
                m.put("FcyCode", text(ci(r, "FcyCode")));
                m.put("M_Tons", asDouble(ci(r, "M_Tons")));
                m.put("PKR_Amount", asDouble(ci(r, "PKR_Amount")));
                m.put("FcyAmount", asDouble(ci(r, "FcyAmount")));
                rows.add(m);
            }
            out.put("mode", "summary");
            out.put("rows", rows);
            return out;
        }
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> r : repo.exportHistoryDetail(u, activity, i(b, "itemTypeId"), i(b, "itemCategoryId"), i(b, "itemId"),
                d(b, "fromDate"), d(b, "toDate"), i(b, "contractId"), i(b, "customerId"), i(b, "portId"), i(b, "tradeTypeId"),
                i(b, "farmingTypeId"))) {
            Map<String, Object> m = new LinkedHashMap<>();
            for (String[] c : DETAIL_COLS) {
                Object v = ci(r, c[1]);
                if ("DocDate".equals(c[0])) m.put(c[0], iso(v));
                else if (DETAIL_DOUBLES.contains(c[0])) m.put(c[0], asDouble(v));
                else m.put(c[0], v == null ? "" : String.valueOf(v));
            }
            rows.add(m);
        }
        out.put("mode", "detail");
        out.put("rows", rows);
        return out;
    }

    // =============================================================================== frmExportInvoiceSummaryByMonth

    public Map<String, Object> monthSetup() {
        UserAccount u = user(SCREEN_INVOICE_SUMMARY_BY_MONTH);
        return year(u);
    }

    private static final String[][] MONTH_COLS = { { "Customer", "Customer" }, { "FcyCode", "FCY_CODE" }, { "FcyJuly", "FCY_JULY" },
            { "PkrJuly", "PKR_JULY" }, { "FcyAug", "Fcy_Aug" }, { "PkrAug", "Pkr_Aug" }, { "FcySep", "Fcy_Sep" }, { "PkrSep", "Pkr_Sep" },
            { "FcyOct", "Fcy_Oct" }, { "PkrOct", "Pkr_Oct" }, { "FcyNov", "Fcy_Nov" }, { "PkrNov", "Pkr_Nov" }, { "FcyDec", "Fcy_Dec" },
            { "PkrDec", "Pkr_Dec" }, { "FcyJan", "Fcy_Jan" }, { "PkrJan", "Pkr_Jan" }, { "FcyFeb", "Fcy_Feb" }, { "PkrFeb", "Pkr_Feb" },
            { "FcyMar", "Fcy_Mar" }, { "PkrMar", "Pkr_Mar" }, { "FcyApr", "Fcy_Apr" }, { "PkrApr", "Pkr_Apr" }, { "FcyMay", "Fcy_May" },
            { "PkrMay", "Pkr_May" }, { "FcyJune", "Fcy_Jun" }, { "PkrJune", "Pkr_Jun" }, { "FcyTotal", "Fcy_Total" }, { "PkrTotal", "Pkr_Total" } };

    /** GridBind(): SpExImInvoiceSummaryByMonth projected to the form's DataTable (Customer, FcyCode, Fcy/Pkr per month, totals). */
    public List<Map<String, Object>> monthShow(Map<String, Object> b) {
        UserAccount u = user(SCREEN_INVOICE_SUMMARY_BY_MONTH);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.summaryByMonth(u, d(b, "fromDate"), d(b, "toDate"))) {
            Map<String, Object> m = new LinkedHashMap<>();
            for (String[] c : MONTH_COLS) {
                Object v = ci(r, c[1]);
                m.put(c[0], c[0].equals("Customer") || c[0].equals("FcyCode") ? (v == null ? "" : String.valueOf(v)) : (v == null ? null : asDouble(v)));
            }
            out.add(m);
        }
        return out;
    }

    // =============================================================================== PerformaInvoiceRegister

    /** Load: FromDate = ActiveYr.Start_Period, ExportInvoicesLoad (page), PartyNameFill ("7"), CurrencyFill, ItemFill. */
    public Map<String, Object> performaSetup() {
        UserAccount u = user(SCREEN_PERFORMA_INVOICE_REGISTER);
        Map<String, Object> out = new LinkedHashMap<>(year(u));
        List<String> errors = new ArrayList<>();
        put(out, "parties", () -> combo(repo.customersByGroup(u, "7"), "Id", "CompanyName"), errors);
        put(out, "currencies", () -> combo(repo.currencies(u), "Id", "CurrencyName"), errors);
        put(out, "items", () -> combo(repo.itemsForCombo(u), "Id", "ItemName"), errors);
        out.put("errors", errors);
        return out;
    }

    /** btnReset_Click: PartyNameFill, ItemFill, CurrencyFill. */
    public Map<String, Object> performaCombos() {
        Map<String, Object> m = performaSetup();
        m.remove("yearStart");
        m.remove("yearEnd");
        return m;
    }

    private static final String[][] PERFORMA_DETAILED = { { "Id", "Id" }, { "DocumentTypeId", "DocumentTypeId" }, { "DocNo", "DocNo" },
            { "DocDate", "ProformaDate" }, { "Pri_Ref#", "ProformaNo" }, { "Pri_RefDate", "SalesContratDate" }, { "SupplierCustomerId", "SupCustId" },
            { "PartyName", "CustomerName" }, { "SalesPersonId", "SalesPersonId" }, { "SalesMan", "SalesMan" }, { "CommissionAgentId", "CommissionAgentId" },
            { "CommissionAgent", "CommissionAgent" }, { "Comm%", "CommRate" }, { "CommAmount", "CommAmount" }, { "CurrencyCode", "CurrencyCode" },
            { "DeliveryTerm", "DeliveryTerm" }, { "PaymentTerm", "PaymentTerm" }, { "LoadingPort", "LoadingPort" }, { "DestinationPort", "DestinationPort" },
            { "ItemId", "ExImItemId" }, { "ItemName", "ItemName" }, { "PackingType", "PackingType" }, { "PackingWeight", "PackingWeight" },
            { "CropYear", "CropYear" }, { "NoOfContainers", "NoofContainerDetail" }, { "NoOfBags/Cntnr", "NoofBagsPerContainer" }, { "PackUom", "PackUom" },
            { "MTon", "M_Ton" }, { "ShipMTon", "ShipMTon" }, { "BalMTon", "BalMTon" }, { "NoOfBags", "NoOfBags" }, { "ShipBags", "ShipBags" },
            { "BalBags", "BalNoofBags" }, { "PackingTotalWeight", "PackingTotalWeight" }, { "NetWeight", "NetWeight" }, { "ShipWeight", "ShipWeight" },
            { "BalWeight", "BalWeight" }, { "ItemRate", "RatePrice" }, { "Amount", "Amount" }, { "ShipAmount", "ShipAmount" }, { "BalAmount", "BalShipAmount" },
            { "PackingExpiryDate", "PackingExpiryDate" }, { "ContainerSize", "ContainerSize" }, { "OtherDescription", "Other_Description" },
            { "OuterPackUomDescription", "OuterPackDescription" }, { "InsuranceRemarks", "InsuranceRemarks" }, { "RemarksHeader", "RemarksHeader" },
            { "EntryUser", "EntryUser" }, { "EntryDate", "EntryDate" }, { "ModifyUser", "ModifyUser" }, { "ModifyDate", "ModifyDate" },
            { "NoOfAttachments", "NoOfAttachments" } };

    private static final String[][] PERFORMA_MAIN = { { "Id", "Id" }, { "DocumentTypeId", "DocumentTypeId" }, { "DocNo", "DocNo" },
            { "DocDate", "ProformaDate" }, { "Pri_Ref#", "ProformaNo" }, { "Pri_RefDate", "SalesContratDate" }, { "SupplierCustomerId", "SupCustId" },
            { "PartyName", "CustomerName" }, { "SalesPersonId", "SalesPersonId" }, { "SalesMan", "SalesMan" }, { "CommissionAgentId", "CommissionAgentId" },
            { "CommissionAgent", "CommissionAgent" }, { "Comm%", "CommRate" }, { "CommAmount", "CommAmount" }, { "FCurrencyId", "FCurrencyId" },
            { "CurrencyCode", "CurrencyCode" }, { "DeliveryTermId", "DeliveryTermId" }, { "DeliveryTerm", "DeliveryTerm" }, { "PaymentTermId", "PaymentTermId" },
            { "PaymentTerm", "PaymentTerm" }, { "LoadingPortId", "LoadingPortId" }, { "LoadingPort", "LoadingPort" }, { "DestinationPortId", "DestinationPortId" },
            { "DestinationPort", "DestinationPort" }, { "InsuranceRemarks", "InsuranceRemarks" }, { "RemarksHeader", "RemarksHeader" },
            { "EntryUser", "EntryUser" }, { "EntryDate", "EntryDate" }, { "ModifyUser", "ModifyUser" }, { "ModifyDate", "ModifyDate" },
            { "NoOfAttachments", "NoOfAttachments" } };

    private static final String[][] PERFORMA_DETAIL = { { "PerformaDetailId", "ContractDetailId" }, { "ItemId", "ExImItemId" },
            { "ItemName", "ItemName" }, { "PackingType", "PackingType" }, { "PackingWeight", "PackingWeight" }, { "CropYear", "CropYear" },
            { "NoOfContainers", "NoofContainerDetail" }, { "NoOfBags/Cntnr", "NoofBagsPerContainer" }, { "PackUom", "PackUom" }, { "MTon", "M_Ton" },
            { "ShipMTon", "ShipMTon" }, { "BalMTon", "BalMTon" }, { "NoOfBags", "NoOfBags" }, { "ShipBags", "ShipBags" }, { "BalBags", "BalNoofBags" },
            { "PackingTotalWeight", "PackingTotalWeight" }, { "NetWeight", "NetWeight" }, { "ShipWeight", "ShipWeight" }, { "BalWeight", "BalWeight" },
            { "ItemRate", "RatePrice" }, { "Amount", "Amount" }, { "ShipAmount", "ShipAmount" }, { "BalAmount", "BalShipAmount" },
            { "PackingExpiryDate", "PackingExpiryDate" }, { "ContainerSize", "ContainerSize" }, { "OtherDescription", "Other_Description" },
            { "OuterPackUomDescription", "OuterPackDescription" } };

    private static final Set<String> PERFORMA_DATES = new LinkedHashSet<>(java.util.Arrays.asList("DocDate", "Pri_RefDate", "EntryDate", "ModifyDate"));

    private static Map<String, Object> map(Map<String, Object> r, String[][] cols, Set<String> dateCols) {
        Map<String, Object> m = new LinkedHashMap<>();
        for (String[] c : cols) {
            Object v = ci(r, c[1]);
            m.put(c[0], dateCols.contains(c[0]) ? iso(v) : plain(v));
        }
        return m;
    }

    /**
     * ExportInvoicesLoad(): GetProformaDataForInvoices (DocumentTypeIds "151", SkipZero -> ZeroBalanceType 1) and the
     * three DataTables the form builds from it: GridMainDetail (one row per detail), grd (distinct Id, first row wins)
     * and, per Id, the rows grd_SelectionChanged copies into grddetail.
     */
    public Map<String, Object> performaShow(Map<String, Object> b) {
        UserAccount u = user(SCREEN_PERFORMA_INVOICE_REGISTER);
        List<Map<String, Object>> rows = repo.proformaData(u, branchId(u), financialYearId(), i(b, "partyId"), i(b, "currencyId"),
                i(b, "itemId"), bool(b, "skipZero") ? 1 : 0);
        List<Map<String, Object>> detailed = new ArrayList<>(), main = new ArrayList<>();
        Map<String, List<Map<String, Object>>> byId = new LinkedHashMap<>();
        Set<Integer> seen = new LinkedHashSet<>();
        for (Map<String, Object> r : rows) {
            detailed.add(map(r, PERFORMA_DETAILED, PERFORMA_DATES));
            int id = asInt(ci(r, "Id"));
            if (seen.add(id)) main.add(map(r, PERFORMA_MAIN, PERFORMA_DATES));
            byId.computeIfAbsent(String.valueOf(id), k -> new ArrayList<>()).add(map(r, PERFORMA_DETAIL, PERFORMA_DATES));
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("detailed", detailed);
        out.put("main", main);
        out.put("detailById", byId);
        return out;
    }

    /** CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, 151): AttachmentName / CustomName / EntryDate (AttachmentView). */
    public List<Map<String, Object>> performaAttachments(int id) {
        user(SCREEN_PERFORMA_INVOICE_REGISTER);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.attachmentsByRefDocumentType(id, 151)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("AttachmentName", text(ci(r, "Attachment")));
            m.put("CustomName", text(ci(r, "UploadedFileCustomName")));
            m.put("EntryDate", isoDateTime(ci(r, "EntryDate")));
            out.add(m);
        }
        return out;
    }

    // =============================================================================== frmStockReservedRegister

    /** InitializeComponentCustom (RefFromDate = ActiveYr.Start_Period) + InitializeComponentMethod (ComboDbCall). */
    public Map<String, Object> stockSetup() {
        UserAccount u = user(SCREEN_STOCK_RESERVED_REGISTER);
        Map<String, Object> out = new LinkedHashMap<>(year(u));
        try { out.put("combos", stockGroups(u)); }
        catch (Exception e) { out.put("combos", null); out.put("comboError", "Error occurred during database call."); }
        return out;
    }

    /** btnRefresh_Click: StockComboFill(ComboDbCall()) - an error is the plain exception message here (no Task wrapper). */
    public Map<String, Object> stockCombos() {
        UserAccount u = user(SCREEN_STOCK_RESERVED_REGISTER);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("combos", stockGroups(u));
        return out;
    }

    private Map<String, Object> stockGroups(UserAccount u) {
        Map<String, Object> g = new LinkedHashMap<>();
        for (String k : new String[] { "WareHouse", "CropYear", "JobLot", "ItemName", "Supplier", "RefRefDocumentType", "RefDocNo",
                "ItemParentCategory", "ItemClassGroup" }) g.put(k, new ArrayList<Map<String, Object>>());
        for (Map<String, Object> r : repo.stockReservedCombos(u)) {
            String t = text(ci(r, "ActivityType"));
            @SuppressWarnings("unchecked")
            List<Map<String, Object>> list = (List<Map<String, Object>>) g.get(t);
            if (list == null || t.isEmpty()) continue;
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", ci(r, "Id"));
            m.put("name", ci(r, "name") == null ? "" : String.valueOf(ci(r, "name")));
            list.add(m);
        }
        return g;
    }

    private static final String[][] STOCK_COLS = { { "StockStatus", "StockStatus" }, { "RefDocumentTypeId", "RefDocumentTypeId" },
            { "RefDocIdNo", "RefDocIdNo" }, { "RefDocSubIdNo", "RefDocSubIdNo" }, { "ThirdPartyAnalysisDate", "RefDocDate" }, { "LotRefNo", "LotRefNo" },
            { "RefDocNo", "DocCodeNo" }, { "RefDocDate", "DocDate" }, { "ReportStatus", "ReportStatus" }, { "SupplierCustomerId", "SupplierCustomerId" },
            { "PartyName", "SupplierCustomerName" }, { "VehicleNo", "VehicleNo" }, { "BiltyNo", "BiltyNo" }, { "GpNo", "GpNo" },
            { "WareHouseName", "WareHouseCode" }, { "ItemId", "ItemId" }, { "ItemName", "ItemName" }, { "ItemCode", "ItemCode" }, { "ItemUom", "ItemUom" },
            { "CropYear", "CropBatch" }, { "JobLot", "JobLotCode" }, { "PackingType", "PackingType" }, { "Aflatoxins", "Aflatoxins" }, { "InQty", "QtyIn" },
            { "OutQty", "QtyOut" }, { "BalQty", "BalStockQty" }, { "InWeight", "WeightIn" }, { "OutWeight", "StockWeightOut" },
            { "BalWeight", "BalStockWeight" }, { "ReservedQty", "ReservedQty" }, { "ReservedWeight", "ReservedStockWeight" }, { "ItemRate", "AVgRate" },
            { "RateUom", "RateUom" }, { "Amount", "Amount" }, { "TranRemarks", "TranRemarks" } };
    private static final Set<String> STOCK_DATES = new LinkedHashSet<>(java.util.Arrays.asList("ThirdPartyAnalysisDate", "RefDocDate"));

    /**
     * PendingTransactions(): RefFromDate / RefTodate only when their check box is ticked; RefDocNoFrom / To; combos;
     * ActionId 1 Reserved / 2 Non-Reserved / 0 All; StatusId 1 Sampling Pending / 2 Complete / 0 All; SkipZero; the
     * parent categories as "id1,id2," (names matched back to ids, trailing comma kept); ClassGroup.
     */
    public List<Map<String, Object>> stockShow(Map<String, Object> b) {
        UserAccount u = user(SCREEN_STOCK_RESERVED_REGISTER);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.stockReservedRegister(u, branchId(u), financialYearId(), i(b, "refDocId"),
                i(b, "refRefDocumentTypeId"), d(b, "fromDate"), d(b, "toDate"), num(b, "docNoFrom"), num(b, "docNoTo"),
                i(b, "customerId"), i(b, "warehouseId"), i(b, "itemId"), i(b, "jobLotId"), i(b, "actionId"), i(b, "statusId"),
                i(b, "skipZero"), i(b, "classGroupId"), s(b, "parentCategoryIds"))) {
            out.add(map(r, STOCK_COLS, STOCK_DATES));
        }
        return out;
    }

    // =============================================================================== ExportContractReports

    /**
     * ExportContractReports_Load: nothing unless a ScreenViewReights row of module "Export Reports" has Value; then
     * "Contract Register" when a row has ScreenName "SaleContractHistory" and "Contracts Current Status" when a row has
     * ScreenName "frmContractCurrentStatusReport" (clsGlobalVariables.ScreenViewReights = USP_GetUserRightsForViewbyUserId,
     * read through DashboardModuleService.viewRights(), which keeps only Value rows as the desktop list is filtered).
     */
    public Map<String, Object> contractReportTabs() {
        user(SCREEN_CONTRACT_REPORTS);
        boolean module = false, register = false, current = false;
        for (Map<String, Object> r : dashboardModuleService.viewRights()) {
            if ("Export Reports".equals(text(ci(r, "ModuleDescription")))) module = true;
            String sn = text(ci(r, "ScreenName"));
            if ("SaleContractHistory".equals(sn)) register = true;
            if ("frmContractCurrentStatusReport".equals(sn)) current = true;
        }
        List<Map<String, Object>> tabs = new ArrayList<>();
        if (module) {
            if (register) {
                Map<String, Object> t = new LinkedHashMap<>();
                t.put("title", "Contract Register");
                t.put("form", "ExImSaleContractRegister");
                t.put("url", "/export/contract-register");
                tabs.add(t);
            }
            if (current) {
                Map<String, Object> t = new LinkedHashMap<>();
                t.put("title", "Contracts Current Status");
                t.put("form", "frmContractCurrentStatusReport");
                t.put("url", "");
                tabs.add(t);
            }
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("tabs", tabs);
        out.put("moduleRight", module);
        return out;
    }

    // =============================================================================== helpers

    /** Projection in the given column order; dates (any java.util.Date / LocalDate*) become yyyy-MM-dd. */
    private static List<Map<String, Object>> project(List<Map<String, Object>> rows, String[] cols) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            for (String c : cols) m.put(c, plain(ci(r, c)));
            out.add(m);
        }
        return out;
    }

    private static Object plain(Object v) {
        if (v == null) return null;
        if (v instanceof java.util.Date || v instanceof java.time.LocalDate || v instanceof java.time.LocalDateTime) return iso(v);
        if (v instanceof byte[]) return null;
        if (v instanceof java.math.BigDecimal) return ((java.math.BigDecimal) v).doubleValue();
        return v;
    }

    static String text(Object v) { return v == null ? "" : String.valueOf(v).trim(); }

    static int asInt(Object v) {
        if (v == null) return 0;
        if (v instanceof Number) return ((Number) v).intValue();
        try { return (int) Double.parseDouble(String.valueOf(v).trim()); } catch (NumberFormatException e) { return 0; }
    }

    static double asDouble(Object v) {
        if (v == null) return 0d;
        if (v instanceof Number) return ((Number) v).doubleValue();
        try { return Double.parseDouble(String.valueOf(v).trim().replace(",", "")); } catch (NumberFormatException e) { return 0d; }
    }

    static String iso(Object v) {
        if (v == null) return "";
        if (v instanceof java.sql.Timestamp) return ((java.sql.Timestamp) v).toLocalDateTime().toLocalDate().toString();
        if (v instanceof java.sql.Date) return ((java.sql.Date) v).toLocalDate().toString();
        if (v instanceof java.util.Date) return new java.sql.Timestamp(((java.util.Date) v).getTime()).toLocalDateTime().toLocalDate().toString();
        if (v instanceof java.time.LocalDateTime) return ((java.time.LocalDateTime) v).toLocalDate().toString();
        if (v instanceof java.time.LocalDate) return v.toString();
        String s = String.valueOf(v).trim();
        return s.length() >= 10 ? s.substring(0, 10) : s;
    }

    static String isoDateTime(Object v) {
        if (v == null) return "";
        if (v instanceof java.sql.Timestamp) return ((java.sql.Timestamp) v).toLocalDateTime().withNano(0).toString();
        if (v instanceof java.util.Date) return new java.sql.Timestamp(((java.util.Date) v).getTime()).toLocalDateTime().withNano(0).toString();
        if (v instanceof java.time.LocalDateTime) return ((java.time.LocalDateTime) v).withNano(0).toString();
        return String.valueOf(v).trim();
    }

    private static int i(Map<String, Object> b, String k) { return b == null ? 0 : asInt(b.get(k)); }
    private static double num(Map<String, Object> b, String k) { return b == null ? 0d : asDouble(b.get(k)); }
    private static String s(Map<String, Object> b, String k) { Object v = b == null ? null : b.get(k); return v == null ? "" : String.valueOf(v); }
    private static boolean bool(Map<String, Object> b, String k) {
        Object v = b == null ? null : b.get(k);
        return v instanceof Boolean ? (Boolean) v : "true".equalsIgnoreCase(String.valueOf(v)) || "1".equals(String.valueOf(v));
    }

    /** yyyy-MM-dd -> java.sql.Date; blank -> null (the BLL's CheckDateTimeNull guard leaves it out). */
    private static Date d(Map<String, Object> b, String k) {
        String v = s(b, k).trim();
        if (v.isEmpty()) return null;
        try { return Date.valueOf(LocalDate.parse(v.length() > 10 ? v.substring(0, 10) : v)); }
        catch (DateTimeParseException e) { throw new IllegalArgumentException("Invalid date: " + v); }
    }

    private static String root(Throwable e) {
        Throwable t = e;
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        return t.getMessage() == null ? "Error occurred during database call." : t.getMessage();
    }
}
