package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.ExportReportsNRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.format.DateTimeParseException;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.ExportReportsNRepository.ci;
import static com.mst.repositories.ExportReportsNRepository.table;

/**
 * Module 17 "Export Reports", group N - the BLL side of seven desktop report forms:
 *
 *   237  frmExportContractSchedulePeriodic              "5002 Contract Schedule Report"
 *   248  ExportComparisonSummaryReport                  "Export Comparison Summary"
 *   259  CommercialInvoiceAgainstForwardingPreInvoices  "Export Sales Report (Commercial Invoice Against Forwarding/Pre Invoices)"
 *   264  frmBillofLadingSlipandRegister                 "Bill of Lading Report"
 *   268  ExImContainerList                              "Container List"
 *   271  PreInvoiceRegister                             "Pre Invoice Register"
 *   272  BankGdSummary                                  "Bank Gd Summary"
 *
 * Rights: DesktopReportRights.require(user, <ScreenDefinition.Id>, "View") on every call. Organisation, company, branch
 * and financial year come from the session, never from the request. Rows are projected to the desktop's own grid
 * DataTable columns (the dtcol / dtship / dt copies the forms build before binding).
 */
@Service
public class ExportReportsNService {

    public static final int SCREEN_237 = 237, SCREEN_248 = 248, SCREEN_259 = 259, SCREEN_264 = 264,
            SCREEN_268 = 268, SCREEN_271 = 271, SCREEN_272 = 272;

    @Autowired private ExportReportsNRepository repo;
    @Autowired private CurrentUserContext currentUserContext;
    @Autowired private DesktopReportRights rights;

    private UserAccount user(int screenId) {
        UserAccount u = currentUserContext.requireAccountingUser();
        rights.require(u, screenId, "View");
        return u;
    }

    /** CommonServices.DateType() - a fixed DataTable on the desktop (kept fixed here). */
    private static List<Map<String, Object>> dateTypes() {
        String[] t = { "This Day", "This Week", "This Month", "This Year", "Financial Year" };
        List<Map<String, Object>> out = new ArrayList<>();
        for (int i = 0; i < t.length; i++) { Map<String, Object> m = new LinkedHashMap<>(); m.put("Id", i + 1); m.put("name", t[i]); out.add(m); }
        return out;
    }

    /** clsGlobalVariables.ActiveYr Start_Period / End_Period as yyyy-MM-dd ("" when no active year). */
    private String[] activeYear(UserAccount u) {
        try {
            Map<String, Object> y = repo.activeYear(u, currentUserContext.currentFinancialYearId());
            if (y == null) return new String[] { "", "" };
            return new String[] { iso(ci(y, "Start_Period")), iso(ci(y, "End_Period")) };
        } catch (Exception e) { return new String[] { "", "" }; }
    }

    /** CommonServices.GetDecimalConfiguration: amount (stringFormatsingle/both), rate (DecimalRateFormate, 0 -> 2), fcy rate (DecimalFCYRateFormate). */
    private void decimals(UserAccount u, Map<String, Object> out) {
        int amt, rate, fcyRate;
        try { amt = asInt(repo.config(u, "Default NoofDecimal Points For Amount")); } catch (Exception e) { amt = 0; }
        try { rate = asInt(repo.config(u, "Default NoofDecimal Points For Rate")); } catch (Exception e) { rate = 0; }
        try { fcyRate = asInt(repo.config(u, "DefaultNoOfDecimalPointsForFcyRate")); } catch (Exception e) { fcyRate = 0; }
        out.put("decimalsAmount", clamp(amt));
        out.put("decimalsRate", rate == 0 ? 2 : clamp(rate));
        out.put("decimalsFcyRate", clamp(fcyRate));
    }

    private static int clamp(int v) { return v < 1 || v > 4 ? 0 : v; }

    // ================================================================================ 237 Contract Schedule Report

    /**
     * frmEvaulationDetailSalesReports_Load: MonthFill (every month of the active year as "MMM yyyy"), LastScheduleLoadingDate =
     * GetLatestAttentiveLoadDate (DateTime.Now when the procedure returns no row). The page applies the date arithmetic.
     */
    public Map<String, Object> scheduleASetup() {
        UserAccount u = user(SCREEN_237);
        Map<String, Object> out = new LinkedHashMap<>();
        String[] y = activeYear(u);
        List<Map<String, Object>> months = new ArrayList<>();
        try {
            if (!y[0].isEmpty() && !y[1].isEmpty()) {
                LocalDate d = LocalDate.parse(y[0]), end = LocalDate.parse(y[1]);
                while (!d.isAfter(end)) {
                    String s = MON[d.getMonthValue() - 1] + " " + d.getYear();
                    Map<String, Object> m = new LinkedHashMap<>(); m.put("Id", s); m.put("name", s); months.add(m);
                    d = d.plusMonths(1);
                }
            }
        } catch (DateTimeParseException e) { out.put("monthsError", e.getMessage()); }
        out.put("months", months);
        Object last = null;
        try { last = repo.latestAttentiveLoadDate(u); }
        catch (Exception e) { out.put("lastScheduleLoadingDateError", msg(e)); }
        out.put("lastScheduleLoadingDate", last == null ? "" : isoDateTime(last));
        out.put("now", LocalDateTime.now().withNano(0).toString());
        return out;
    }

    /**
     * btnShow_Click (ShowDataByDates: From/To date pickers) and btnShowByMonths_Click (ShowDataByMonths: the first day of
     * From Month to the last day of To Month) -> ExportContractSchedulePeriodicA -> FillAllGrids.
     */
    public Map<String, Object> scheduleAShow(Map<String, Object> b) {
        UserAccount u = user(SCREEN_237);
        Timestamp from, to;
        if ("months".equals(text(b.get("mode")))) {
            LocalDate f = parseMonth(text(b.get("fromMonth"))), t = parseMonth(text(b.get("toMonth")));
            /* DateTime.ParseExact throws on an empty / unknown month text: the form shows the exception message. */
            if (f == null || t == null) throw new IllegalArgumentException("String was not recognized as a valid DateTime.");
            from = Timestamp.valueOf(f.atStartOfDay());
            to = Timestamp.valueOf(t.withDayOfMonth(t.lengthOfMonth()).atStartOfDay());
        } else {
            from = ts(b.get("fromDate"));
            to = ts(b.get("toDate"));
        }
        List<List<Map<String, Object>>> ds = repo.contractSchedulePeriodicA(u, from, to);
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> byDate = new ArrayList<>();
        for (Map<String, Object> r : table(ds, 0)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Date", ddMMMyy(ci(r, "WeekStart")) + " to " + ddMMMyy(ci(r, "WeekEnd")));
            m.put("Fcl", asDouble(ci(r, "Fcl")));
            m.put("MTons", asDouble(ci(r, "MTon")));
            byDate.add(m);
        }
        out.put("byDate", byDate);
        out.put("byParty", three(table(ds, 1), "Customer", "CompanyName", "MTon"));
        out.put("byPorts", three(table(ds, 2), "Port", "PortName", "MTon"));
        out.put("byItem", three(table(ds, 3), "Item", "ItemName", "MTons"));
        out.put("byMonths", three(table(ds, 4), "Month", "ScheduleMonth", "MTons"));
        out.put("fromDate", iso(from));
        out.put("toDate", iso(to));
        return out;
    }

    private static List<Map<String, Object>> three(List<Map<String, Object>> rows, String key, String src, String mtCol) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put(key, text(ci(r, src)));
            m.put("Fcl", asDouble(ci(r, "Fcl")));
            m.put("MTons", asDouble(ci(r, mtCol)));
            out.add(m);
        }
        return out;
    }

    private static LocalDate parseMonth(String s) {
        String[] p = s.trim().split("\\s+");
        if (p.length != 2) return null;
        for (int i = 0; i < 12; i++) {
            if (MON[i].equals(p[0])) {
                try { return LocalDate.of(Integer.parseInt(p[1]), i + 1, 1); } catch (RuntimeException e) { return null; }
            }
        }
        return null;
    }

    // ================================================================================ 248 Export Comparison Summary

    /** ExportComparisonSummaryReport_Load: datFromDate = ActiveYr.Start_Period, ReportTypeBind + the six combo binds. */
    public Map<String, Object> comparisonSetup() {
        UserAccount u = user(SCREEN_248);
        Map<String, Object> out = comparisonCombos(u);
        put(out, "reportTypes", () -> pick(repo.staticColumnNames("ExportComparisonsSummery"), "ReportType", "ReportName"));
        out.put("yearStart", activeYear(u)[0]);
        decimals(u, out);
        return out;
    }

    /** btnRefreshSummary_Click: CustomerGetAll, ContractNoGetAll, ItemBind, ItemCategoryBind, ItemTypeBind, SeaPortBind. */
    public Map<String, Object> comparisonRefresh() { return comparisonCombos(user(SCREEN_248)); }

    private Map<String, Object> comparisonCombos(UserAccount u) {
        Map<String, Object> out = new LinkedHashMap<>();
        put(out, "customers", () -> pick(repo.exportCustomers(u), "Id", "CompanyName"));
        put(out, "contracts", () -> pick(repo.lcOrderNos(u), "Id", "LcOrderNo"));
        put(out, "items", () -> pick(repo.exportItems(u), "Id", "ItemName"));
        put(out, "itemCategories", () -> pick(repo.itemCategories(u), "Id", "CategoryDescription"));
        put(out, "itemTypes", () -> pick(repo.itemTypes(u), "Id", "TypeDescription"));
        put(out, "seaPorts", () -> pick(repo.seaPorts(u), "Id", "PortName"));
        return out;
    }

    /**
     * GridBind: "Report Type filed is required" without a report type; @ReportType = the combo TEXT (ReportName), dates only
     * when their check boxes are ticked; dtcol projection (AvgPrice -> AvgRate, FcrAvgRate -> ExchangeRate, PKR_Amount -> LcyAmount).
     */
    public Map<String, Object> comparisonShow(Map<String, Object> b) {
        UserAccount u = user(SCREEN_248);
        String reportType = text(b.get("reportTypeText"));
        if (asInt(b.get("reportTypeId")) == 0 || reportType.isEmpty()) throw new IllegalArgumentException("Report Type filed is required");
        List<List<Map<String, Object>>> ds = repo.comparisonsSummary(u, reportType,
                asBool(b.get("fromChecked")) ? ts(b.get("fromDate")) : null, asBool(b.get("toChecked")) ? ts(b.get("toDate")) : null,
                asInt(b.get("itemCategoryId")), asInt(b.get("itemTypeId")), asInt(b.get("itemId")), asInt(b.get("supplierCustomerId")),
                asInt(b.get("lcContractId")), asInt(b.get("destinationPortId")));
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> r : table(ds, 0)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("GroupCaption", text(ci(r, "GroupCaption")));
            m.put("GroupName", text(ci(r, "GroupName")));
            m.put("MTons", asDouble(ci(r, "MTons")));
            m.put("FcyCode", text(ci(r, "FcyCode")));
            m.put("AvgRate", asDouble(ci(r, "AvgPrice")));
            m.put("Fcy_Amount", asDouble(ci(r, "Fcy_Amount")));
            m.put("ExchangeRate", asDouble(ci(r, "FcrAvgRate")));
            m.put("LcyAmount", asDouble(ci(r, "PKR_Amount")));
            m.put("TotalMTons", text(ci(r, "TotalMTons")));
            m.put("PrctExport", asDouble(ci(r, "PrctExport")));
            rows.add(m);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rows", rows);
        return out;
    }

    // ================================================================================ 259 / 271 shared Load

    /** ParameterFill + ComboFill: GetDataForDropDownFromExportInvoice(@DocumentTypeIds) split by ActivityType. */
    private Map<String, Object> exportInvoiceCombos(UserAccount u, String docTypes) {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("dateTypes", dateTypes());
        List<Map<String, Object>> itemTypes = new ArrayList<>(), items = new ArrayList<>(), contracts = new ArrayList<>(), customers = new ArrayList<>();
        try {
            for (Map<String, Object> r : repo.dropDownFromExportInvoice(u, docTypes)) {
                String a = text(ci(r, "ActivityType"));
                Map<String, Object> m = new LinkedHashMap<>();
                m.put("Id", asInt(ci(r, "Id")));
                m.put("name", text(ci(r, "name")));
                if ("ItemType".equals(a)) itemTypes.add(m);
                if ("Items".equals(a)) items.add(m);
                if ("ContractNo".equals(a)) contracts.add(m);
                if ("Customer".equals(a)) customers.add(m);
            }
        } catch (Exception e) { out.put("combosError", msg(e)); }
        out.put("itemTypes", itemTypes);
        out.put("items", items);
        out.put("contracts", contracts);
        out.put("customers", customers);
        out.put("yearStart", activeYear(u)[0]);
        return out;
    }

    // ================================================================================ 259 Invoice Against Forwarding / Pre Invoices

    public Map<String, Object> forwardingSetup() {
        UserAccount u = user(SCREEN_259);
        Map<String, Object> out = exportInvoiceCombos(u, "211");
        decimals(u, out);
        return out;
    }

    /** GridBind -> ExportInvoiceAgainstForwarding_Register, dtship projection (32 columns, ItemCommodityDetail / HealthPermitNoDetail renamed). */
    public Map<String, Object> forwardingShow(Map<String, Object> b) {
        UserAccount u = user(SCREEN_259);
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> r : repo.invoiceAgainstForwardingRegister(u, ts(b.get("fromDate")), ts(b.get("toDate")),
                asInt(b.get("supplierCustomerId")), asInt(b.get("exImLcOrderId")), asInt(b.get("itemTypeId")), asInt(b.get("itemId")))) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("CustomerName", text(ci(r, "CustomerName")));
            m.put("ContractNo", text(ci(r, "ContractNo")));
            m.put("ContractDate", iso(ci(r, "ContractDate")));
            m.put("InvoiceNo", text(ci(r, "InvoiceNo")));
            m.put("InvoiceDate", iso(ci(r, "InvoiceDate")));
            m.put("FcyCode", text(ci(r, "FcyCode")));
            m.put("ExchangeRate", asDouble(ci(r, "ExchangeRate")));
            m.put("CommissionAgent", text(ci(r, "CommissionAgent")));
            m.put("Comm%", asDouble(ci(r, "Comm%")));
            m.put("CommissionAmount", asDouble(ci(r, "CommissionAmount")));
            m.put("HsCode", text(ci(r, "HsCode")));
            m.put("ItemName", text(ci(r, "ItemName")));
            m.put("CropYear", text(ci(r, "CropYear")));
            m.put("PackingType", text(ci(r, "PackingType")));
            m.put("NoofContainers", asDouble(ci(r, "NoofContainers")));
            m.put("NoofBagsPerContainer", asDouble(ci(r, "NoofBagsPerContainer")));
            m.put("PackUom", text(ci(r, "PackUom")));
            m.put("MTon", asDouble(ci(r, "MTon")));
            m.put("NoofBags", asDouble(ci(r, "NoofBags")));
            m.put("NetWeight", asDouble(ci(r, "NetWeight")));
            m.put("PackingWeight", asDouble(ci(r, "PackingWeight")));
            m.put("GrossWeight", asDouble(ci(r, "GrossWeight")));
            m.put("RatePrice", asDouble(ci(r, "RatePrice")));
            m.put("RateUom", text(ci(r, "RateUom")));
            m.put("FcAmount", asDouble(ci(r, "FcAmount")));
            m.put("BankRate", asDouble(ci(r, "BankRate")));
            m.put("BankAmount", asDouble(ci(r, "BankAmount")));
            m.put("OtherHsCode", text(ci(r, "OtherHsCode")));
            m.put("PackingExpiryDate", text(ci(r, "PackingExpiryDate")));
            m.put("ProductionNo", text(ci(r, "ProductionNo")));
            m.put("ItemCommodity", text(ci(r, "ItemCommodityDetail")));
            m.put("HealthPermit", text(ci(r, "HealthPermitNoDetail")));
            rows.add(m);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rows", rows);
        return out;
    }

    // ================================================================================ 271 Pre Invoice Register

    public Map<String, Object> preInvoiceSetup() { return exportInvoiceCombos(user(SCREEN_271), "209"); }

    /** GridBind -> ExportShipmentDetailReportByContract (ZeroBalanceType 1 when Skip Zero is ticked), dtship projection. */
    public Map<String, Object> preInvoiceShow(Map<String, Object> b) {
        UserAccount u = user(SCREEN_271);
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> r : repo.shipmentDetailByContract(u, ts(b.get("fromDate")), ts(b.get("toDate")),
                asInt(b.get("supplierCustomerId")), asInt(b.get("exImLcOrderId")), asInt(b.get("itemTypeId")), asInt(b.get("itemId")),
                asBool(b.get("skipZero")) ? 1 : 0)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("CustomerName", text(ci(r, "CustomerName")));
            m.put("ContractId", asInt(ci(r, "ContractId")));
            m.put("ContractNo", text(ci(r, "ContractNo")));
            m.put("ContractDate", iso(ci(r, "ContractDate")));
            m.put("InvoiceNo", text(ci(r, "InvoiceNo")));
            m.put("InvoiceDate", iso(ci(r, "InvoiceDate")));
            m.put("ItemName", text(ci(r, "ItemName")));
            for (String k : new String[] { "FclQty", "NoofBags", "ShipBags", "BalBags", "MTon", "ShipMTon", "BalMTon", "InvoiceWeight", "DispatchWeight", "BalWeight" })
                m.put(k, asDouble(ci(r, k)));
            rows.add(m);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rows", rows);
        return out;
    }

    /** grdShipmentDetail_ColumnButtonClick "Print": ExpRptSalesContractExportRegister_523 (ContractId, ActionId 1) must return rows. */
    public Map<String, Object> preInvoiceContractCheck(int contractId) {
        UserAccount u = user(SCREEN_271);
        List<Map<String, Object>> r = repo.salesContractExportRegister523(u, contractId, 1);
        if (r.isEmpty()) throw new IllegalArgumentException("Not Record Found For Display");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("count", r.size());
        return out;
    }

    // ================================================================================ 264 Bill of Lading Report

    /** frmBillofLadingSlipandRegister_Load: AllComboBind + GridBind (the page calls show after setup). */
    public Map<String, Object> bolSetup() {
        UserAccount u = user(SCREEN_264);
        Map<String, Object> out = bolCombos(u);
        decimals(u, out);
        return out;
    }

    /** btnRefresh_Click -> AllComboBind. */
    public Map<String, Object> bolRefresh() { return bolCombos(user(SCREEN_264)); }

    /**
     * AllComboBind: USP_GetDataForDropDownFromBillOfLading split by Activity. Quirk kept: the procedure's "Customer" rows carry
     * the BILL OF LADING Id (d.Id), and that value is what the form sends as @SupplierCustomerId.
     */
    private Map<String, Object> bolCombos(UserAccount u) {
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> customers = new ArrayList<>(), invoices = new ArrayList<>(), lines = new ArrayList<>();
        try {
            for (Map<String, Object> r : repo.dropDownFromBillOfLading(u)) {
                String a = text(ci(r, "Activity"));
                Map<String, Object> m = new LinkedHashMap<>();
                m.put("Id", asInt(ci(r, "Id")));
                m.put("name", text(ci(r, "ReferenceName")));
                if ("Customer".equals(a)) customers.add(m);
                else if ("InvoiceNo".equals(a)) invoices.add(m);
                else if ("ShippingLine".equals(a)) lines.add(m);
            }
        } catch (Exception e) { out.put("combosError", msg(e)); }
        out.put("customers", customers);
        out.put("invoices", invoices);
        out.put("shippingLines", lines);
        return out;
    }

    /** GridBind -> ExImBillOfLading_SlipAndRegister, dt projection (25 columns). */
    public Map<String, Object> bolShow(Map<String, Object> b) {
        UserAccount u = user(SCREEN_264);
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> r : repo.billOfLadingSlipAndRegister(u.getOrganizationId(), u.getCompanyId(),
                asInt(b.get("invoiceId")), asInt(b.get("supplierCustomerId")), asInt(b.get("shippingLineId")))) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("InvoiceNo", text(ci(r, "InvoiceNo")));
            m.put("DocDate", iso(ci(r, "DocDate")));
            m.put("EFormNo", text(ci(r, "EFormNo")));
            m.put("EFormDate", iso(ci(r, "EFormDate")));
            m.put("EFormValueTotal", asDouble(ci(r, "EFormValueTotal")));
            m.put("CustomerBuyerName", text(ci(r, "CustomerBuyerName")));
            m.put("Address1", text(ci(r, "Address1")));
            m.put("notifyPartyName", text(ci(r, "notifyPartyName")));
            m.put("ToTheOrderOfName", text(ci(r, "ToTheOrderOfName")));
            m.put("BLNumber", text(ci(r, "BLNumber")));
            m.put("BLDate", iso(ci(r, "BLDate")));
            m.put("VesselNo", text(ci(r, "VesselNo")));
            m.put("VoyageNo", text(ci(r, "VoyageNo")));
            m.put("CarierType", text(ci(r, "CarierType")));
            m.put("BookingCroNo", text(ci(r, "BookingCroNo")));
            m.put("BookingDate", iso(ci(r, "BookingDate")));
            m.put("FreightType", text(ci(r, "FreightType")));
            Object cert = ci(r, "ShipingCertNo");
            m.put("ShipingCertNo", cert == null || text(cert).isEmpty() ? null : asInt(cert));
            m.put("ShipingCertDate", iso(ci(r, "ShipingCertDate")));
            m.put("ItemsGoodsDesc", text(ci(r, "ItemsGoodsDesc")));
            m.put("ShippingLineName", text(ci(r, "ShippingLineName")));
            m.put("ClearingAgentName", text(ci(r, "ClearingAgentName")));
            m.put("TransporterName", text(ci(r, "TransporterName")));
            m.put("Remarks", text(ci(r, "Remarks")));
            m.put("ExImInvoiceId", asInt(ci(r, "ExImInvoiceId")));
            rows.add(m);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rows", rows);
        return out;
    }

    // ================================================================================ 268 Container List

    /** frmExportShipingLineBookingRpt_Load: InvoiceNoGetAll, ContractNoGetAll (GridBind follows from the page). */
    public Map<String, Object> containerSetup() {
        UserAccount u = user(SCREEN_268);
        Map<String, Object> out = new LinkedHashMap<>();
        put(out, "invoices", () -> pick(repo.exportInvoiceNos(u), "Id", "InvoiceNo"));
        put(out, "contracts", () -> pick(repo.lcOrderNos(u), "Id", "LcOrderNo"));
        return out;
    }

    /** GridBind -> ExportDeliveryOrderInvoiceGatepassRegister; RetrieveStructure shows every procedure column in order. */
    public Map<String, Object> containerShow(Map<String, Object> b) {
        UserAccount u = user(SCREEN_268);
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> r : repo.deliveryOrderInvoiceGatepassRegister(u, asInt(b.get("contractId")), asInt(b.get("invoiceId")))) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Customer", text(ci(r, "Customer")));
            m.put("ContractNo", text(ci(r, "ContractNo")));
            m.put("InvoiceNo", text(ci(r, "InvoiceNo")));
            m.put("InvoiceDate", iso(ci(r, "InvoiceDate")));
            m.put("DoNo", text(ci(r, "DoNo")));
            m.put("DoDate", iso(ci(r, "DoDate")));
            m.put("ItemName", text(ci(r, "ItemName")));
            m.put("LotNo", text(ci(r, "LotNo")));
            m.put("DoWeight", asDouble(ci(r, "DoWeight")));
            m.put("GpNo", text(ci(r, "GpNo")));
            m.put("GpDate", iso(ci(r, "GpDate")));
            m.put("VehicleNo", text(ci(r, "VehicleNo")));
            m.put("BiltyNo", text(ci(r, "BiltyNo")));
            m.put("ContainerNo1", text(ci(r, "ContainerNo1")));
            m.put("Container2", text(ci(r, "Container2")));
            rows.add(m);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rows", rows);
        return out;
    }

    // ================================================================================ 272 Bank Gd Summary

    public Map<String, Object> bankGdSetup() {
        UserAccount u = user(SCREEN_272);
        Map<String, Object> out = new LinkedHashMap<>();
        decimals(u, out);
        return out;
    }

    /** GridFill: Amount = Conversion.ToDouble(txtzerBalance.Text) -> BankGdSummary, dtcol projection (13 columns). */
    public Map<String, Object> bankGdShow(Map<String, Object> b) {
        UserAccount u = user(SCREEN_272);
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> r : repo.bankGdSummary(u, asDouble(b.get("fromBalance")))) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("GdId", asInt(ci(r, "GdId")));
            m.put("DocumentTypeId", asInt(ci(r, "DocumentTypeId")));
            m.put("CustomerName", text(ci(r, "CustomerName")));
            m.put("InvoiceNo", text(ci(r, "InvoiceNo")));
            m.put("InvoiceDate", text(ci(r, "InvoiceDate")));
            m.put("InvoiceAmount", asDouble(ci(r, "InvoiceAmount")));
            m.put("Mtons", text(ci(r, "Mtons")));
            m.put("GdNo", text(ci(r, "GdNo")));
            m.put("GdAmount", asDouble(ci(r, "GdAmount")));
            m.put("CommAmount", asDouble(ci(r, "CommAmount")));
            m.put("RealizedAmount", asDouble(ci(r, "RealizedAmount")));
            m.put("CommRealizeTotal", asDouble(ci(r, "CommRealizeTotal")));
            m.put("GdBalance", asDouble(ci(r, "GdBalance")));
            rows.add(m);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rows", rows);
        return out;
    }

    /** grdfrm_LinkClicked GdNo -> frmGDBreakUp (GdId, DocumentTypeId) -> GetGdBreakUpsByGdId; RetrieveStructure, Id hidden. */
    public Map<String, Object> gdBreakUp(int gdId, int documentTypeId) {
        user(SCREEN_272);
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> r : repo.gdBreakUpsByGdId(gdId, documentTypeId)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("DocDate", iso(ci(r, "DocDate")));
            m.put("DocNo", text(ci(r, "DocNo")));
            m.put("InvoiceNo", text(ci(r, "InvoiceNo")));
            m.put("GDNO", text(ci(r, "GDNO")));
            m.put("CustomerName", text(ci(r, "CustomerName")));
            m.put("DebitAccount", text(ci(r, "DebitAccount")));
            m.put("BankFbpNo", text(ci(r, "BankFbpNo")));
            m.put("FcGrossAmount", asDouble(ci(r, "FcGrossAmount")));
            m.put("CurrencyCode", text(ci(r, "CurrencyCode")));
            m.put("ExchangeRate", asDouble(ci(r, "ExchangeRate")));
            m.put("RealizedAmount", asDouble(ci(r, "RealizedAmount")));
            m.put("CommAmount", asDouble(ci(r, "CommAmount")));
            m.put("TotalRealized", asDouble(ci(r, "TotalRealized")));
            rows.add(m);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rows", rows);
        return out;
    }

    // ================================================================================ helpers

    private interface Loader { Object load() throws Exception; }

    /** The desktop binds each combo in its own try/catch; one failing bind is reported beside the others. */
    private static void put(Map<String, Object> out, String key, Loader l) {
        try { out.put(key, l.load()); }
        catch (Exception e) { out.put(key, new ArrayList<>()); out.put(key + "Error", msg(e)); }
    }

    private static List<Map<String, Object>> pick(List<Map<String, Object>> rows, String idCol, String textCol) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            Object id = ci(r, idCol);
            m.put("Id", id instanceof Number ? ((Number) id).intValue() : text(id));
            m.put("name", text(ci(r, textCol)));
            out.add(m);
        }
        return out;
    }

    private static String msg(Exception e) {
        Throwable t = e;
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        return t.getMessage() == null ? String.valueOf(e.getMessage()) : t.getMessage();
    }

    static String text(Object v) { return v == null ? "" : String.valueOf(v).trim(); }

    static int asInt(Object v) {
        if (v == null) return 0;
        if (v instanceof Number) return ((Number) v).intValue();
        if (v instanceof Boolean) return (Boolean) v ? 1 : 0;
        String s = String.valueOf(v).trim().replace(",", "");
        try { return Integer.parseInt(s); }
        catch (NumberFormatException e) {
            try { return (int) Double.parseDouble(s); } catch (NumberFormatException e2) { return 0; }
        }
    }

    /** Conversion.ToDouble: 0 for blank or non-numeric text. */
    static double asDouble(Object v) {
        if (v == null) return 0;
        if (v instanceof Number) return ((Number) v).doubleValue();
        try { return Double.parseDouble(String.valueOf(v).trim().replace(",", "")); }
        catch (NumberFormatException e) { return 0; }
    }

    private static boolean asBool(Object v) {
        if (v == null) return false;
        if (v instanceof Boolean) return (Boolean) v;
        String s = String.valueOf(v).trim().toLowerCase();
        return "true".equals(s) || "1".equals(s) || "on".equals(s);
    }

    /** A DATETIME parameter from the page's yyyy-MM-dd; null when blank. */
    private static Timestamp ts(Object v) {
        if (v instanceof Timestamp) return (Timestamp) v;
        if (v == null) return null;
        String s = String.valueOf(v).trim();
        if (s.isEmpty()) return null;
        try { return Timestamp.valueOf(LocalDateTime.parse(s.length() == 10 ? s + "T00:00:00" : s.length() == 16 ? s + ":00" : s)); }
        catch (DateTimeParseException e) {
            try { return Timestamp.valueOf(s.replace('T', ' ')); } catch (IllegalArgumentException e2) { return null; }
        }
    }

    static String iso(Object v) {
        if (v == null) return "";
        if (v instanceof Timestamp) return ((Timestamp) v).toLocalDateTime().toLocalDate().toString();
        if (v instanceof java.sql.Date) return ((java.sql.Date) v).toLocalDate().toString();
        if (v instanceof java.util.Date) return new Timestamp(((java.util.Date) v).getTime()).toLocalDateTime().toLocalDate().toString();
        if (v instanceof LocalDateTime) return ((LocalDateTime) v).toLocalDate().toString();
        if (v instanceof LocalDate) return v.toString();
        String s = String.valueOf(v).trim();
        return s.length() >= 10 ? s.substring(0, 10) : s;
    }

    static String isoDateTime(Object v) {
        if (v == null) return "";
        if (v instanceof Timestamp) return ((Timestamp) v).toLocalDateTime().withNano(0).toString();
        if (v instanceof java.util.Date) return new Timestamp(((java.util.Date) v).getTime()).toLocalDateTime().withNano(0).toString();
        if (v instanceof LocalDateTime) return ((LocalDateTime) v).withNano(0).toString();
        if (v instanceof LocalDate) return ((LocalDate) v).atStartOfDay().toString();
        return String.valueOf(v).trim();
    }

    private static final String[] MON = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

    /** ToString("dd-MMM-yy"). */
    private static String ddMMMyy(Object v) {
        String s = iso(v);
        if (s.length() < 10) return s;
        try {
            LocalDate d = LocalDate.parse(s);
            return String.format("%02d-%s-%02d", d.getDayOfMonth(), MON[d.getMonthValue() - 1], d.getYear() % 100);
        } catch (DateTimeParseException e) { return s; }
    }
}
