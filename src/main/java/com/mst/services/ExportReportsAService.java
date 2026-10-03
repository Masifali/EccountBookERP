package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.ExportReportsARepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;

import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.format.DateTimeParseException;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.ExportReportsARepository.ci;
import static com.mst.repositories.ExportReportsARepository.table;

/**
 * Module 17 "Export Reports", group A - the BLL side of seven desktop report forms:
 *
 *   235  ExportReceivableByDueDateRegisterA   "5011 Receivable By Due Report"
 *   236  frmExportContractSchedulePeriodicB   "5003 Contract Schedule Periodic"
 *   238  ExImShipmentCosting                  "5006 Shipment Costing Detail"
 *   239  frmExportPendingWorksRegister        "5008 Shipments Document Status Report"
 *   249  ExportDetailHistoryReport            "Export Detail History (Not Use)"
 *   250  DeliveryOrderHistory                 "5015 Delivery Order Report"   (Architecture.WinApp.Inventory_Reports)
 *   251  ExImSaleContractRegister             "5001 Contract Report"
 *
 * Rights: DesktopReportRights.require(user, <ScreenDefinition.Id>, "View") on every read; the two status
 * updates (239 Complete/Open Status, 251 Complete/Open/Cancel) also require the same View right, as the
 * desktop only checks RoleName == "Admin" (239) or nothing at all (251). Organisation, company, branch and
 * user come from the session, never from the request. Rows are projected to the desktop DataTable columns.
 */
@Service
public class ExportReportsAService {

    public static final int SCREEN_235 = 235, SCREEN_236 = 236, SCREEN_238 = 238, SCREEN_239 = 239,
            SCREEN_249 = 249, SCREEN_250 = 250, SCREEN_251 = 251;

    @Autowired private ExportReportsARepository repo;
    @Autowired private CurrentUserContext currentUserContext;
    @Autowired private DesktopReportRights rights;

    private UserAccount user(int screenId) {
        UserAccount u = currentUserContext.requireAccountingUser();
        rights.require(u, screenId, "View");
        return u;
    }

    private boolean allowed(UserAccount u, int screenId) {
        try { rights.require(u, screenId, "View"); return true; }
        catch (AccessDeniedException ex) { return false; }
    }

    /** CommonServices.DateType() - a fixed DataTable on the desktop (kept fixed here). */
    private static List<Map<String, Object>> dateTypes() {
        String[][] t = { { "1", "This Day" }, { "2", "This Week" }, { "3", "This Month" }, { "4", "This Year" }, { "5", "Financial Year" } };
        List<Map<String, Object>> out = new ArrayList<>();
        for (String[] r : t) { Map<String, Object> m = new LinkedHashMap<>(); m.put("Id", Integer.parseInt(r[0])); m.put("Parameters", r[1]); out.add(m); }
        return out;
    }

    private String yearStart(UserAccount u) {
        try { return iso(repo.financialYearStart(u, currentUserContext.currentFinancialYearId())); }
        catch (Exception e) { return ""; }
    }

    // ================================================================================ shared popup

    /**
     * frmContractDetailByContractId (opened by the ContractNo link of 235 and 251): ExportContractDetailById
     * -> header labels, Item Information, Invoice Information, Schedule Information.
     */
    public Map<String, Object> contractDetail(int screenId, int contractId) {
        UserAccount u = user(screenId);
        List<List<Map<String, Object>>> ds = repo.exportContractDetailById(u, contractId);
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> h = table(ds, 0);
        Map<String, Object> hd = new LinkedHashMap<>();
        if (!h.isEmpty()) {
            Map<String, Object> r = h.get(0);
            hd.put("Id", asInt(ci(r, "Id")));
            hd.put("LcOrderNo", text(ci(r, "LcOrderNo")));
            hd.put("LcOrderDate", iso(ci(r, "LcOrderDate")));
            hd.put("CustomerName", text(ci(r, "CustomerName")));
            hd.put("LastShipmentDate", iso(ci(r, "LastShipmentDate")));
            hd.put("lcMtons", asDouble(ci(r, "lcMtons")));
            hd.put("InvoiceMtons", asDouble(ci(r, "InvoiceMtons")));
            hd.put("CurrencyCode", text(ci(r, "CurrencyCode")));
            hd.put("LcFcyAmount", asDouble(ci(r, "LcFcyAmount")));
            hd.put("InvoiceValue", asDouble(ci(r, "InvoiceValue")));
        }
        out.put("header", hd);
        List<Map<String, Object>> items = new ArrayList<>();
        for (Map<String, Object> r : table(ds, 1)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("ItemId", asInt(ci(r, "ExImItemId")));
            m.put("ItemName", text(ci(r, "ItemName")));
            m.put("TotalMTons", asDouble(ci(r, "TotalMTon")));
            m.put("ShippedMTons", asDouble(ci(r, "ShippedMton")));
            m.put("BalanceMTons", asDouble(ci(r, "BalMTon")));
            items.add(m);
        }
        out.put("items", items);
        List<Map<String, Object>> inv = new ArrayList<>();
        for (Map<String, Object> r : table(ds, 2)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("DocType", text(ci(r, "DocumentTypeCode")));
            m.put("InvoiceId", asInt(ci(r, "InvoiceId")));
            m.put("InvoiceNo", text(ci(r, "InvoiceNo")));
            m.put("InvoiceDate", iso(ci(r, "InvoiceDate")));
            m.put("BolDate", iso(ci(r, "BolDate")));
            m.put("CustomerContractNo", text(ci(r, "CustomerContractNos")));
            m.put("MTons", asDouble(ci(r, "MTons")));
            m.put("PaymentTerm", text(ci(r, "PaymentTerm")));
            m.put("DueDate", iso(ci(r, "DueDate")));
            m.put("ReceivedDate", iso(ci(r, "ReceivedDate")));
            m.put("Amount", asDouble(ci(r, "Amount")));
            m.put("Received", asDouble(ci(r, "Received")));
            m.put("Balance", asDouble(ci(r, "BalAmount")));
            inv.add(m);
        }
        out.put("invoices", inv);
        List<Map<String, Object>> sch = new ArrayList<>();
        for (Map<String, Object> r : table(ds, 3)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("ContractId", asInt(ci(r, "ContractId")));
            m.put("Id", asInt(ci(r, "Id")));
            m.put("DocumentTypeId", asInt(ci(r, "DocumentTypeId")));
            m.put("ScheduleCode", text(ci(r, "ScheduleCode")));
            m.put("CustomerName", text(ci(r, "CustomerName")));
            m.put("CustomerContractNo", text(ci(r, "CustomerContractNo")));
            m.put("LoadingDate", iso(ci(r, "LoadingDate")));
            m.put("ItemName", text(ci(r, "ItemName")));
            m.put("PackSize", text(ci(r, "PackSize")));
            m.put("NoOfBags", asDouble(ci(r, "NoOfBags")));
            m.put("MTon", asDouble(ci(r, "MTon")));
            m.put("ShippedMTon", asDouble(ci(r, "ShippedMTon")));
            m.put("BalanceMTon", asDouble(ci(r, "BalMton")));
            m.put("NoOfAttachments", asInt(ci(r, "NoOfAttachments")));
            m.put("DestinationPort", text(ci(r, "DestinationPort")));
            sch.add(m);
        }
        out.put("schedules", sch);
        return out;
    }

    // ================================================================================ 235 Receivable By Due Date

    /** frmExportShipingLineBookingRpt_Load: all six combos + ActiveYr.Start_Period; the desktop then binds the grid once. */
    public Map<String, Object> receivableSetup() {
        UserAccount u = user(SCREEN_235);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("yearStart", yearStart(u));
        out.putAll(receivableCombos());
        return out;
    }

    /** btnRefresh_Click: InvoiceNoGetAll, CustomerGetAll, ContractNoGetAll, DeliveryTermFill, PaymentTermsFill, SeaPortBind. */
    public Map<String, Object> receivableCombos() {
        UserAccount u = user(SCREEN_235);
        Map<String, Object> out = new LinkedHashMap<>();
        put(out, "invoices", () -> pick(repo.exportInvoiceNos(u), "Id", "InvoiceNo"));
        put(out, "customers", () -> pick(repo.exportCustomersFromVouchers(u), "Id", "CustomerName"));
        put(out, "contracts", () -> pick(repo.lcOrderNos(u), "Id", "LcOrderNo"));
        put(out, "deliveryTerms", () -> pick(repo.deliveryTerms(), "Id", "Code"));
        put(out, "paymentTerms", () -> pick(repo.paymentTerms(u), "Id", "LcOrderTerm"));
        put(out, "ports", () -> pick(repo.seaPorts(u), "Id", "PortName"));
        return out;
    }

    /** GridBind(SortNo): the six DataTables projected to the desktop's grid columns. */
    public Map<String, Object> receivableShow(Map<String, Object> b) {
        UserAccount u = user(SCREEN_235);
        List<List<Map<String, Object>>> ds = repo.receivableRegister(u,
                asInt(b.get("customerId")), asInt(b.get("contractId")), asInt(b.get("invoiceId")),
                asBool(b.get("fromChecked")) ? ts(b.get("fromDate")) : null,
                asBool(b.get("toChecked")) ? ts(b.get("toDate")) : null,
                asInt(b.get("paymentTermId")), asInt(b.get("deliveryTermId")), asInt(b.get("destinationPortId")),
                asBool(b.get("skipZero")) ? 1 : 0, asInt(b.get("sortNo")));
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> reg = new ArrayList<>();
        for (Map<String, Object> r : table(ds, 0)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Customer", text(ci(r, "Customer")));
            m.put("BankName", text(ci(r, "BankName")));
            m.put("ContractIds", text(ci(r, "ContractId")));
            m.put("ContractNo", text(ci(r, "ContractNo")));
            m.put("InvoiceId", asInt(ci(r, "InvoiceId")));
            m.put("InvoiceNo", text(ci(r, "InvoiceNo")));
            m.put("PaymentTerm", text(ci(r, "PaymentTerm")));
            m.put("Fcy", text(ci(r, "Fcy")));
            m.put("InvoiceValue", asDouble(ci(r, "InvoiceValue")));
            m.put("AdvanceReceipt", asDouble(ci(r, "AdvanceReceipt")));
            m.put("NetReceivable", asDouble(ci(r, "NetReceivable")));
            m.put("DueDate", iso(ci(r, "DueDate")));
            m.put("InvoiceDate", iso(ci(r, "InvoiceDate")));
            m.put("BolDate", iso(ci(r, "BolDate")));
            m.put("DueDays", asDouble(ci(r, "DueDays")));
            m.put("OverDueDays", asInt(ci(r, "OverDueDays")));
            m.put("ETADestination", iso(ci(r, "ETADestination")));
            m.put("ReachedAtDestination", iso(ci(r, "ReachedAtDestination")));
            m.put("FCL", text(ci(r, "FCL")));
            m.put("MTons", asDouble(ci(r, "MTons")));
            m.put("SaleMan", text(ci(r, "SaleMan")));
            m.put("ShippedOnBoard", iso(ci(r, "ShippedOnBoard")));
            m.put("InTransitDays", asDouble(ci(r, "InTransitDays")));
            m.put("IncoTerm", text(ci(r, "IncoTerm")));
            m.put("Advance", asDouble(ci(r, "Advance")));
            m.put("Receipt", asDouble(ci(r, "Receipt")));
            m.put("DestinationPort", text(ci(r, "DestinationPort")));
            m.put("ExportReceivablesAging", text(ci(r, "ExportReceivablesAging")));
            m.put("InvoiceStatus", text(ci(r, "InvoiceStatus")));
            reg.add(m);
        }
        out.put("register", reg);
        out.put("bank", share(table(ds, 1), "BankName"));
        out.put("customer", share(table(ds, 2), "Customer"));
        out.put("saleMan", share(table(ds, 3), "SaleMan"));
        out.put("fcy", share(table(ds, 4), null));
        List<Map<String, Object>> eta = new ArrayList<>();
        for (Map<String, Object> r : table(ds, 5)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("SortNo", asInt(ci(r, "SortNo")));
            m.put("ETADestinationPort", text(ci(r, "ETADestinationPort")));
            m.put("FcyAmount", asDouble(ci(r, "FcyAmount")));
            m.put("PkrAmount", asDouble(ci(r, "PkrAmount")));
            eta.add(m);
        }
        out.put("eta", eta);
        return out;
    }

    /** dtBank / dtCustomer / dtSaleMan / dtFcy: (name), Fcy, NetReceivable, TotalReceivable, PrcntOfTotal. */
    private static List<Map<String, Object>> share(List<Map<String, Object>> rows, String nameCol) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            if (nameCol != null) m.put(nameCol, text(ci(r, nameCol)));
            m.put("Fcy", text(ci(r, "Fcy")));
            m.put("NetReceivable", asDouble(ci(r, "NetReceivable")));
            m.put("TotalReceivable", asDouble(ci(r, "TotalReceivable")));
            m.put("PrcntOfTotal", asDouble(ci(r, "PrcntOfTotal")));
            out.add(m);
        }
        return out;
    }

    // ================================================================================ 236 Contract Schedule Periodic

    /**
     * frmEvaulationDetailSalesReports_Load: LastScheduleLoadingDate = GetLatestAttentiveLoadDate; when it is in the
     * future Add Days = (Last - Now).Days, else From Date = Last; To Date = Last. The page repeats that arithmetic.
     */
    public Map<String, Object> scheduleSetup() {
        UserAccount u = user(SCREEN_236);
        Map<String, Object> out = new LinkedHashMap<>();
        Object d = null;
        try { d = repo.latestAttentiveLoadDate(u); } catch (Exception e) { out.put("lastScheduleLoadingDateError", msg(e)); }
        out.put("lastScheduleLoadingDate", d == null ? "" : iso(d));
        return out;
    }

    /**
     * ShowDataByDates(SortNo): Per FCL MTon "" -> 24, < 18 -> "Minimum Value Of MTon is 18 .", > 30 -> "Maximum Value
     * Of MTon is 30."; From/To only when ticked; @DaysInterval = Interval Days; @ReferredStatusId 1 when Not Referred.
     */
    public Map<String, Object> scheduleShow(Map<String, Object> b) {
        UserAccount u = user(SCREEN_236);
        String pct = text(b.get("perFclTon"));
        if (pct.isEmpty()) pct = "24";
        int perFclTon = asInt(pct);
        if (perFclTon < 18) throw new IllegalArgumentException("Minimum Value Of MTon is 18 .");
        if (perFclTon > 30) throw new IllegalArgumentException("Maximum Value Of MTon is 30.");
        int sortNo = asInt(b.get("sortNo"));
        List<List<Map<String, Object>>> ds = repo.contractSchedulePeriodicB(u,
                asBool(b.get("fromChecked")) ? ts(b.get("fromDate")) : null,
                asBool(b.get("toChecked")) ? ts(b.get("toDate")) : null,
                perFclTon, asInt(b.get("intervalDays")), sortNo, asBool(b.get("notReferred")) ? 1 : 0);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("perFclTon", pct);
        List<Map<String, Object>> t1 = new ArrayList<>();
        for (Map<String, Object> r : table(ds, 1)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("GroupTitle", text(ci(r, "GroupTitle")));
            m.put("ItemId", asInt(ci(r, "ItemId")));
            m.put("LoadingDate", iso(ci(r, "LoadingDate")));
            m.put("ItemName", text(ci(r, "ItemName")));
            m.put("Fcl", asDouble(ci(r, "Flc")));
            m.put("MTons", asDouble(ci(r, "MTons")));
            t1.add(m);
        }
        out.put("loadingDateItem", t1);
        List<Map<String, Object>> t2 = new ArrayList<>();
        for (Map<String, Object> r : table(ds, 2)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("GroupTitle", text(ci(r, "GroupTitle")));
            m.put("ScheduleId", asInt(ci(r, "Id")));
            m.put("ExImLcOrderId", asInt(ci(r, "ExImLcOrderId")));
            m.put("LoadingDate", iso(ci(r, "LoadingDate")));
            m.put("ScheduleCode", text(ci(r, "ScheduleCode")));
            m.put("SupplierCustomerId", asInt(ci(r, "SupplierCustomerId")));
            m.put("Customer", text(ci(r, "Customer")));
            m.put("CustomerContractNo", text(ci(r, "CustomerContractNo")));
            m.put("ItemId", asInt(ci(r, "ItemId")));
            m.put("ItemName", text(ci(r, "ItemName")));
            m.put("PackSize", text(ci(r, "Packsize")));
            m.put("NoOfBags", asDouble(ci(r, "NoOfBags")));
            m.put("MTons", asDouble(ci(r, "MTons")));
            m.put("Fcl", asDouble(ci(r, "Flc")));
            m.put("ReferredStatus", text(ci(r, "ReferredStatus")));
            m.put("DestinationPort", text(ci(r, "DestinationPort")));
            m.put("NoOfAttachments", asInt(ci(r, "NoOfAttachments")));
            m.put("GroupCaption", ddMMMyyyy(ci(r, "LoadingDate")) + " " + text(ci(r, "DestinationPort")) + " " + text(ci(r, "ScheduleCode")));
            t2.add(m);
        }
        out.put("loadingDateItemCustomer", t2);
        /* the periodic (aging) grid is only rebuilt when SortNo == 0, as FillAllGrids returns before it otherwise */
        if (sortNo == 0) {
            List<Map<String, Object>> t3 = new ArrayList<>();
            for (Map<String, Object> r : table(ds, 3)) {
                Map<String, Object> m = new LinkedHashMap<>();
                m.put("SortNo", asInt(ci(r, "SortNo")));
                m.put("PeriodDescription", text(ci(r, "Description")));
                m.put("FCL", asDouble(ci(r, "FCL")));
                m.put("MTons", asDouble(ci(r, "MTons")));
                t3.add(m);
            }
            out.put("periodic", t3);
        }
        return out;
    }

    // ================================================================================ 238 Shipment Costing

    /** ExImShipmentCosting_Load: invoice combo + the desktop's amount / fcy decimal formats (GetDecimalConfiguration). */
    public Map<String, Object> costingSetup() {
        UserAccount u = user(SCREEN_238);
        Map<String, Object> out = new LinkedHashMap<>();
        put(out, "invoices", () -> pick(repo.exportInvoicesFromVouchers(u), "Id", "InvoiceNo"));
        int amt, fcy;
        try { amt = asInt(repo.config(u, "Default NoofDecimal Points For Amount")); } catch (Exception e) { amt = 0; }
        try { fcy = asInt(repo.config(u, "DefaultNoOfDecimalPointsForFcyAmount")); } catch (Exception e) { fcy = 0; }
        out.put("decimalsAmount", Math.max(0, Math.min(4, amt)));
        out.put("decimalsFcy", Math.max(0, Math.min(4, fcy)));
        return out;
    }

    /** GridBind(): "Please select Invoice No first!" without an invoice; the rows in dt's column order + the summary boxes. */
    public Map<String, Object> costingShow(int invoiceId) {
        UserAccount u = user(SCREEN_238);
        if (invoiceId == 0) throw new IllegalArgumentException("Please select Invoice No first!");
        List<Map<String, Object>> rows = repo.shipmentCosting(u, invoiceId);
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> grid = new ArrayList<>();
        Map<String, Object> sum = new LinkedHashMap<>();
        /* the boxes keep the LAST row whose total is > 0, exactly as the desktop loop overwrites them */
        for (Map<String, Object> r : rows) {
            if (asDouble(ci(r, "StockValueTotal")) > 0) sum.put("StockValue", asDouble(ci(r, "StockValueTotal")));
            if (asDouble(ci(r, "SalesValueTotal")) > 0) sum.put("SaleValue", asDouble(ci(r, "SalesValueTotal")));
            if (asDouble(ci(r, "ClearForwardValueTotal")) > 0) sum.put("LogisticsValue", asDouble(ci(r, "ClearForwardValueTotal")));
            if (asDouble(ci(r, "PackingMaterialValueTotal")) > 0) sum.put("PackingMaterial", asDouble(ci(r, "PackingMaterialValueTotal")));
            if (asDouble(ci(r, "GainLossValueTotal")) > 0) sum.put("GainLoss", asDouble(ci(r, "GainLossValueTotal")));
            if (asDouble(ci(r, "CommissionValueTotal")) > 0) sum.put("CommissionValue", asDouble(ci(r, "CommissionValueTotal")));
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("SortNo", asInt(ci(r, "SortNo")));
            m.put("ExImInvoiceId", asInt(ci(r, "ExImInvoiceId")));
            m.put("SubSortNo", asInt(ci(r, "SubSortNo")));
            m.put("MasterType", text(ci(r, "MasterType")));
            m.put("VDate", iso(ci(r, "VDate")));
            m.put("VType", text(ci(r, "VType")));
            m.put("VNo", asInt(ci(r, "VNo")));
            m.put("InvoiceNo", text(ci(r, "InvoiceNo")));
            m.put("ItemName", text(ci(r, "ItemName")));
            m.put("JobLot", text(ci(r, "JobLot")));
            m.put("TranRemarks", text(ci(r, "TranRemarks")));
            m.put("ItemRate", asDouble(ci(r, "ItemRate")));
            m.put("Qty", (int) asDouble(ci(r, "Qty")));          /* dt column Qty is typeof(int) */
            m.put("NetWeight", asDouble(ci(r, "NetWeight")));
            m.put("FcyCode", text(ci(r, "FcyCode")));
            m.put("ExchangeRate", asDouble(ci(r, "ExchangeRate")));
            m.put("FcyAmount", asDouble(ci(r, "FcyAmount")));
            m.put("TranAmount", asDouble(ci(r, "TranAmount")));
            m.put("TotalTranAmount", asDouble(ci(r, "TotalTranAmount")));
            m.put("ClearForwardValueTotal", asDouble(ci(r, "ClearForwardValueTotal")));
            m.put("PackingMaterialValueTotal", asDouble(ci(r, "PackingMaterialValueTotal")));
            m.put("SalesValueTotal", asDouble(ci(r, "SalesValueTotal")));
            m.put("GainLossValueTotal", asDouble(ci(r, "GainLossValueTotal")));
            m.put("ShipmentWeight", asDouble(ci(r, "ShipmentWeight")));
            grid.add(m);
        }
        out.put("rows", grid);
        out.put("summary", sum);
        return out;
    }

    // ================================================================================ 239 Shipments Document Status

    /** frmExportShipingLineBookingRpt_Load: date types, ComboFill, then GridBind. */
    public Map<String, Object> pendingSetup() {
        UserAccount u = user(SCREEN_239);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("isAdmin", "Admin".equals(currentUserContext.currentRoleName()));
        out.put("yearStart", yearStart(u));
        out.put("dateTypes", dateTypes());
        out.putAll(contractCombos(u));
        return out;
    }

    /** ComboFill(): GetDataForDropDownFromExportContract split by ActivityType. */
    public Map<String, Object> pendingCombos() { return contractCombos(user(SCREEN_239)); }

    private Map<String, Object> contractCombos(UserAccount u) {
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> contracts = new ArrayList<>(), customers = new ArrayList<>(), items = new ArrayList<>(),
                trade = new ArrayList<>(), farm = new ArrayList<>(), sale = new ArrayList<>();
        try {
            for (Map<String, Object> r : repo.dropDownFromExportContract(u)) {
                String t = text(ci(r, "ActivityType"));
                Map<String, Object> m = new LinkedHashMap<>();
                m.put("Id", asInt(ci(r, "Id")));
                m.put("name", text(ci(r, "name")));
                if ("ContractNo".equals(t)) contracts.add(m);
                else if ("Customer".equals(t)) customers.add(m);
                else if ("Items".equals(t)) items.add(m);
                else if ("TradeType".equals(t)) trade.add(m);
                else if ("FarmingType".equals(t)) farm.add(m);
                else if ("SalePerson".equals(t)) sale.add(m);
            }
        } catch (Exception e) { out.put("combosError", msg(e)); }
        out.put("contracts", contracts);
        out.put("customers", customers);
        out.put("items", items);
        out.put("tradeTypes", trade);
        out.put("farmingTypes", farm);
        out.put("salePersons", sale);
        return out;
    }

    /** GridBind(): From only when ticked, To always; ActionId 1 In Process / 2 Complete / 0 All. */
    public List<Map<String, Object>> pendingShow(Map<String, Object> b) {
        UserAccount u = user(SCREEN_239);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.pendingWorkExportRegister(u, asInt(b.get("contractId")),
                asBool(b.get("fromChecked")) ? ts(b.get("fromDate")) : null, ts(b.get("toDate")),
                asInt(b.get("supplierCustomerId")), asInt(b.get("actionId")))) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("InvoiceDocumentTypeId", asInt(ci(r, "InvoiceDocumentTypeId")));
            m.put("InvoiceId", asInt(ci(r, "InvoiceId")));
            m.put("InvoiceNo", text(ci(r, "InvoiceNo")));
            m.put("ContractId", asInt(ci(r, "ContractId")));
            m.put("ContractScheduleId", asInt(ci(r, "ContractScheduleId")));
            m.put("ContractNo", text(ci(r, "ContractNo")));
            m.put("SupplierCustomerId", asInt(ci(r, "SupplierCustomerId")));
            m.put("CustomerName", text(ci(r, "CustomerName")));
            m.put("Product", text(ci(r, "Product")));
            m.put("Fcl", asInt(ci(r, "NoOfContainer")));
            m.put("MTons", asDouble(ci(r, "MTon")));
            m.put("InspectionId", asInt(ci(r, "InspectionId")));
            m.put("Inspection", text(ci(r, "PreShipmentInspectionStatus")));
            m.put("CROBooking", text(ci(r, "BookingInfoStatus")));
            m.put("DeliveryOrder", text(ci(r, "DOStatus")));
            m.put("Forwarding", text(ci(r, "ForwardingStatus")));
            m.put("BillOfLading", text(ci(r, "BillOfLadingStatus")));
            m.put("DocToParty", text(ci(r, "DocToParty")));
            m.put("GoodsDeclaration", text(ci(r, "GoodsDeclaration")));
            m.put("PortPayment", text(ci(r, "PortPayment")));
            m.put("PackingMaterial", text(ci(r, "PackingMateialStatus")));
            m.put("ShipmentExpenses", text(ci(r, "LogasticsStatus")));
            m.put("ShipmentClose", text(ci(r, "ShipmentClose")));
            m.put("InvoiceStatus", text(ci(r, "FinalInvoiceStatus")));
            m.put("ExportVoucherId", asInt(ci(r, "ExportVoucherId")));
            m.put("NoOfAttachments", asInt(ci(r, "NoOfAttachments")));
            out.add(m);
        }
        return out;
    }

    /** grdfrm_ColumnButtonClick CompleteStatus / OpenStatus (Admin only): ExportInvoice_StatusUpdate(InvoiceId, UserAccount.ID, status). */
    public Map<String, Object> pendingStatusUpdate(int invoiceId, String status) {
        user(SCREEN_239);
        if (!"Admin".equals(currentUserContext.currentRoleName())) throw new AccessDeniedException("Access denied.");
        if (!"Complete".equals(status) && !"Open".equals(status)) throw new IllegalArgumentException("Unknown status.");
        if (invoiceId == 0) throw new IllegalArgumentException("Invoice not found.");
        repo.exportInvoiceStatusUpdate(invoiceId, currentUserContext.currentUserId(), status);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("message", "Update " + status + " successfully...");
        return out;
    }

    // ================================================================================ 249 Export Detail History

    /** frmExportShipingLineBookingRpt_Load: eight combos (no blank row on this form), then GridBind. */
    public Map<String, Object> detailHistorySetup() {
        UserAccount u = user(SCREEN_249);
        Map<String, Object> out = new LinkedHashMap<>();
        put(out, "reportTypes", () -> pick(repo.staticColumnNames("ExportDetailHistory"), "ReportType", "ReportName"));
        put(out, "customers", () -> pick(repo.exportCustomers(u), "Id", "CompanyName"));
        put(out, "contracts", () -> pick(repo.lcOrderNos(u), "Id", "LcOrderNo"));
        put(out, "items", () -> pick(repo.exportItems(u), "Id", "ItemName"));
        put(out, "categories", () -> pick(repo.itemCategories(u), "Id", "CategoryDescription"));
        put(out, "types", () -> pick(repo.itemTypes(u), "Id", "TypeDescription"));
        put(out, "ports", () -> pick(repo.seaPorts(u), "Id", "PortName"));
        put(out, "jobLots", () -> pick(repo.jobLots(u), "Id", "JobLotDescription"));
        return out;
    }

    /** GridBind(): the procedure's own columns (the grid is bound to dtdetail directly); dates are always sent. */
    public List<Map<String, Object>> detailHistoryShow(Map<String, Object> b) {
        UserAccount u = user(SCREEN_249);
        List<Map<String, Object>> rows = repo.exportHistoryDetail(u, ts(b.get("fromDate")), ts(b.get("toDate")),
                asInt(b.get("itemId")), asInt(b.get("supplierCustomerId")), asInt(b.get("lcOrderId")),
                asInt(b.get("itemTypeId")), asInt(b.get("itemCategoryId")), asInt(b.get("destinationPortId")));
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            for (Map.Entry<String, Object> e : r.entrySet()) m.put(e.getKey(), plain(e.getValue()));
            out.add(m);
        }
        return out;
    }

    // ================================================================================ 250 Delivery Order Report

    /** frmStockWithSupplierHistory_Load: Datetypefill (row 1 active), StatusFill (All active), ComboFill. */
    public Map<String, Object> doSetup() {
        UserAccount u = user(SCREEN_250);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("yearStart", yearStart(u));
        out.put("dateTypes", dateTypes());
        out.putAll(doCombos(u));
        return out;
    }

    public Map<String, Object> doCombosRefresh() { return doCombos(user(SCREEN_250)); }

    /** ComboFill(): GetDataForDropDownFromDeliveryOrder split by Activity (Item, Customer, DeliveryOrderType, InvoiceNo). */
    private Map<String, Object> doCombos(UserAccount u) {
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> items = new ArrayList<>(), cust = new ArrayList<>(), types = new ArrayList<>(), inv = new ArrayList<>();
        try {
            for (Map<String, Object> r : repo.dropDownFromDeliveryOrder(u)) {
                String a = text(ci(r, "Activity"));
                Map<String, Object> m = new LinkedHashMap<>();
                m.put("Id", asInt(ci(r, "Id")));
                m.put("name", text(ci(r, "ReferenceName")));
                if ("Item".equals(a)) items.add(m);
                else if ("Customer".equals(a)) cust.add(m);
                else if ("DeliveryOrderType".equals(a)) types.add(m);
                else if ("InvoiceNo".equals(a)) inv.add(m);
            }
        } catch (Exception e) { out.put("combosError", msg(e)); }
        out.put("items", items);
        out.put("customers", cust);
        out.put("deliveryTypes", types);
        out.put("invoices", inv);
        return out;
    }

    /**
     * GridFill(): DocumentTypeId is the form's static field (0 unless another form set it - never on this route),
     * Status text "Approved" -> @IsApproved 1, "Not Approved" -> 0, "All" -> omitted, blank -> @IsApproved 0
     * (the BLL sends IsApproved whenever ApprovedFilter != "All"); Referred 1 / Not Referred 2; weights 1 / 2.
     */
    public List<Map<String, Object>> doShow(Map<String, Object> b) {
        UserAccount u = user(SCREEN_250);
        String statusText = text(b.get("statusText"));
        Boolean isApproved;
        if ("Approved".equals(statusText)) isApproved = Boolean.TRUE;
        else if ("Not Approved".equals(statusText)) isApproved = Boolean.FALSE;
        else if ("All".equals(statusText)) isApproved = null;
        else isApproved = Boolean.FALSE;
        String ref = text(b.get("referred"));
        int skipZero = "referred".equals(ref) ? 1 : "not".equals(ref) ? 2 : 0;
        String wb = text(b.get("weight"));
        int actionId = "first".equals(wb) ? 1 : "second".equals(wb) ? 2 : 0;
        List<List<Map<String, Object>>> ds = repo.deliveryOrderForApproval(u, asInt(b.get("documentTypeId")),
                asInt(b.get("supplierCustomerId")), asInt(b.get("itemId")), asInt(b.get("invoiceId")), actionId, skipZero,
                ts(b.get("fromDate")), ts(b.get("toDate")), isApproved,
                asInt(b.get("deliveryTypeId")) > 0 ? text(b.get("deliveryTypeText")) : null);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : table(ds, 0)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("DocumentTypeId", asInt(ci(r, "DocumentTypeId")));
            m.put("DocNo", asInt(ci(r, "DocNo")));
            m.put("DocDate", iso(ci(r, "DocDate")));
            m.put("DType", text(ci(r, "DeliveryOrderType")));
            m.put("GpId", asInt(ci(r, "GpId")));
            m.put("GpNo", asInt(ci(r, "GpNo")));
            m.put("InvoiceNo", text(ci(r, "InvoiceNo")));
            m.put("SaleOrderId", asInt(ci(r, "SaleOrderId")));
            m.put("SaleOrderNo", text(ci(r, "SaleOrderNo")));
            m.put("CustomerName", text(ci(r, "CustomerName")));
            m.put("ItemName", text(ci(r, "ItemName")));
            m.put("UOM", text(ci(r, "PackUOM")));
            m.put("ItemQty", asDouble(ci(r, "LoadingQty")));
            m.put("NetWeight", asDouble(ci(r, "LoadingWeight")));
            m.put("EbUnit", asDouble(ci(r, "EbUnit")));
            m.put("EbTotal", asDouble(ci(r, "TotalPackingWeight")));
            m.put("GrossWeight", asDouble(ci(r, "GrossWeight")));
            m.put("EntryUser", text(ci(r, "EntryUserName")));
            m.put("EntryDate", isoDateTime(ci(r, "EntryDate")));
            m.put("ModifyUser", text(ci(r, "ModifyUserName")));
            m.put("ModifyDate", isoDateTime(ci(r, "ModifyDate")));
            m.put("IsApproved", text(ci(r, "IsApproved")));
            m.put("Remarks", text(ci(r, "LoadingRemarks")));
            m.put("Container", text(ci(r, "Container")));
            m.put("NoOfAttachments", asInt(ci(r, "NoOfAttachments")));
            out.add(m);
        }
        return out;
    }

    // ================================================================================ 251 Contract Register

    /**
     * frmExportShipingLineBookingRpt_Load: date types, ComboFill, OrderStatusBind (fixed), GetDataForExportInfo,
     * GridBind, GridBindProductWise. The embedded tabs (Contracts Current Status, Contracts Schedule, Pre Shipment
     * Planning) appear when the user has the View right on those screens; only 236 has a web page.
     */
    public Map<String, Object> registerSetup() {
        UserAccount u = user(SCREEN_251);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("yearStart", yearStart(u));
        out.put("dateTypes", dateTypes());
        out.put("scheduleTab", allowed(u, SCREEN_236));
        out.putAll(contractCombos(u));
        return out;
    }

    public Map<String, Object> registerCombos() { return contractCombos(user(SCREEN_251)); }

    /** GetDataForExportInfo(): the card grid; ReqType 1 Financial Year / 2 Export Year / 3 Selected Criteria. */
    public List<Map<String, Object>> registerCard(Map<String, Object> b) {
        UserAccount u = user(SCREEN_251);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.exportInfoCard(u,
                asBool(b.get("fromChecked")) ? ts(b.get("fromDate")) : null,
                asBool(b.get("toChecked")) ? ts(b.get("toDate")) : null, text(b.get("dateType")))) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Descriptions", text(ci(r, "Descriptions")));
            m.put("Fcl", asDouble(ci(r, "Fcl")));
            m.put("MTons", asDouble(ci(r, "MTons")));
            m.put("FcyAmount", asDouble(ci(r, "FcyAmount")));
            m.put("PrctOfTot", asDouble(ci(r, "PrctOfTot")));
            out.add(m);
        }
        return out;
    }

    /** GridBind() (Summary) - EximLcOrder_Register projected to dtHistory. */
    public List<Map<String, Object>> registerSummary(Map<String, Object> b) {
        UserAccount u = user(SCREEN_251);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.eximLcOrderRegister(u, asInt(b.get("contractId")), asInt(b.get("supplierCustomerId")),
                asInt(b.get("itemId")), asInt(b.get("tradeTypeId")), asInt(b.get("farmingTypeId")),
                asBool(b.get("fromChecked")) ? ts(b.get("fromDate")) : null, asBool(b.get("toChecked")) ? ts(b.get("toDate")) : null,
                asBool(b.get("skipZero")) ? 1 : 0, status(b), asInt(b.get("salePersonId")))) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("DocNo", asInt(ci(r, "LcOrderDocNo")));
            m.put("DocDate", iso(ci(r, "ContractDate")));
            m.put("ContractNo", text(ci(r, "ContractNo")));
            m.put("FarmingTrade", text(ci(r, "FarmingTrade")));
            m.put("CustomerName", text(ci(r, "CustomerName")));
            m.put("DeliveryTerms", text(ci(r, "DeliveryTerms")));
            m.put("PaymentTerms", text(ci(r, "PaymentTerms")));
            m.put("Fcy", text(ci(r, "Fcy")));
            m.put("FcyAmount", asDouble(ci(r, "FcyAmount")));
            m.put("FcyShipped", asDouble(ci(r, "FcyShipped")));
            m.put("FcyBalance", asDouble(ci(r, "FcyBalance")));
            m.put("NetWeight", asDouble(ci(r, "NetWeight")));
            m.put("ShippedWeight", asDouble(ci(r, "ShippedWeight")));
            m.put("BalanceWeight", asDouble(ci(r, "BalanceWeight")));
            m.put("Fcl", asInt(ci(r, "NoOfContainersHeader")));
            m.put("ShippedFcl", asInt(ci(r, "ShippedNoOfContainersHeader")));
            m.put("BalanceFcl", asInt(ci(r, "BalNoofContainerHeader")));
            m.put("DestinationPort", text(ci(r, "DestinationPort")));
            m.put("Status", text(ci(r, "LcStatus")));
            m.put("ActionRemarks", "");
            m.put("NoOfAttachments", asInt(ci(r, "NoOfAttachments")));
            out.add(m);
        }
        return out;
    }

    /** GridBindProductWise() - ExImLcOrder_ExportRegisterItemWise projected to dtProductWise. */
    public List<Map<String, Object>> registerProductWise(Map<String, Object> b) {
        UserAccount u = user(SCREEN_251);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.eximLcOrderRegisterItemWise(u, asInt(b.get("contractId")), asInt(b.get("supplierCustomerId")),
                asInt(b.get("itemId")), asInt(b.get("tradeTypeId")), asInt(b.get("farmingTypeId")),
                asBool(b.get("fromChecked")) ? ts(b.get("fromDate")) : null, asBool(b.get("toChecked")) ? ts(b.get("toDate")) : null,
                asBool(b.get("skipZero")) ? 1 : 0, status(b), asInt(b.get("salePersonId")))) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("DocNo", asInt(ci(r, "LcOrderDocNo")));
            m.put("DocDate", iso(ci(r, "ContractDate")));
            m.put("ContractNo", text(ci(r, "ContractNo")));
            m.put("CustomerName", text(ci(r, "CustomerName")));
            m.put("DeliveryTerms", text(ci(r, "DeliveryTerms")));
            m.put("PaymentTerms", text(ci(r, "PaymentTerms")));
            m.put("ItemName", text(ci(r, "ItemName")));
            m.put("PackUom", text(ci(r, "PackUom")));
            m.put("Fcy", text(ci(r, "Fcy")));
            m.put("RatePrice", asDouble(ci(r, "RatePrice")));
            m.put("FcyAmount", asDouble(ci(r, "FcyAmount")));
            m.put("FcyShipped", asDouble(ci(r, "FcyShipped")));
            m.put("FcyBalance", asDouble(ci(r, "FcyBalance")));
            m.put("NetWeight", asDouble(ci(r, "NetWeight")));
            m.put("ShippedWeight", asDouble(ci(r, "ShippedWeight")));
            m.put("BalanceWeight", asDouble(ci(r, "BalanceWeight")));
            m.put("Fcl", asInt(ci(r, "NoofContainer")));
            m.put("ShippedFcl", asInt(ci(r, "ShippedContrainer")));
            m.put("BalanceFcl", asInt(ci(r, "BalNoofContainer")));
            m.put("CommodityDetail", text(ci(r, "CommodityDetial")));
            m.put("Status", text(ci(r, "LcStatus")));
            out.add(m);
        }
        return out;
    }

    /** rdOpen / rdComplete / rdCancel -> "Open" / "Complete" / "Cancel"; rdAll -> nothing. */
    private static String status(Map<String, Object> b) {
        String s = text(b.get("status"));
        return "Open".equals(s) || "Complete".equals(s) || "Cancel".equals(s) ? s : null;
    }

    /**
     * grdfrm_ColumnButtonClick Complete / Open / Cancel -> ContractStatusChange: "Action Remarks Field is Compulsory."
     * without remarks; Cancel with FcyShipped > 0 -> "You cannot cancel this record because the shipped weight is
     * greater than zero. You should mark it as complete."; then ExportContractStatusUpdate(DocumentTypeId 202).
     */
    public Map<String, Object> registerStatusChange(Map<String, Object> b) {
        UserAccount u = user(SCREEN_251);
        String reqType = text(b.get("reqType"));
        if (!"Open".equals(reqType) && !"Complete".equals(reqType) && !"Cancel".equals(reqType)) throw new IllegalArgumentException("Unknown action.");
        if ("Cancel".equals(reqType) && asDouble(b.get("fcyShipped")) > 0.0)
            throw new IllegalArgumentException("You cannot cancel this record because the shipped weight is greater than zero. You should mark it as complete.");
        String remarks = text(b.get("actionRemarks"));
        if (remarks.isEmpty()) throw new IllegalArgumentException("Action Remarks Field is Compulsory.");
        int id = asInt(b.get("id"));
        if (id == 0) throw new IllegalArgumentException("Contract not found.");
        repo.exportContractStatusUpdate(u, currentUserContext.currentUserId(), 202, id, reqType, remarks);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("message", reqType + " Successfully");
        return out;
    }

    // ================================================================================ helpers

    private interface Loader { Object load() throws Exception; }

    /** The desktop Load handlers run each combo bind in its own try/catch; one failing bind is reported beside the others. */
    private static void put(Map<String, Object> out, String key, Loader l) {
        try { out.put(key, l.load()); }
        catch (Exception e) { out.put(key, new ArrayList<>()); out.put(key + "Error", msg(e)); }
    }

    /** Two columns of a combo source as (Id, name) rows. */
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
        try { return Integer.parseInt(String.valueOf(v).trim()); }
        catch (NumberFormatException e) {
            try { return (int) Double.parseDouble(String.valueOf(v).trim().replace(",", "")); }
            catch (NumberFormatException e2) { return 0; }
        }
    }

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

    /** A JSON-safe copy of a driver value (dates -> yyyy-MM-dd, decimals -> double). */
    private static Object plain(Object v) {
        if (v == null) return null;
        if (v instanceof java.util.Date || v instanceof java.time.temporal.Temporal) return iso(v);
        if (v instanceof java.math.BigDecimal) return ((java.math.BigDecimal) v).doubleValue();
        if (v instanceof Number || v instanceof Boolean) return v;
        return String.valueOf(v);
    }

    /** A DATETIME parameter from the page's yyyy-MM-dd; null when blank (the caller decides whether to send it). */
    private static Timestamp ts(Object v) {
        if (v instanceof Timestamp) return (Timestamp) v;
        if (v == null) return null;
        String s = String.valueOf(v).trim();
        if (s.isEmpty()) return null;
        try { return Timestamp.valueOf(LocalDateTime.parse(s.length() == 10 ? s + "T00:00:00" : s.length() == 16 ? s + ":00" : s)); }
        catch (DateTimeParseException e) {
            try { return Timestamp.valueOf(s.replace('T', ' ')); }
            catch (IllegalArgumentException e2) { return null; }
        }
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

    /** yyyy-MM-ddTHH:mm:ss for the "dd-MMM-yy hh:mm tt" columns. */
    static String isoDateTime(Object v) {
        if (v == null) return "";
        if (v instanceof java.sql.Timestamp) return ((java.sql.Timestamp) v).toLocalDateTime().withNano(0).toString();
        if (v instanceof java.util.Date) return new java.sql.Timestamp(((java.util.Date) v).getTime()).toLocalDateTime().withNano(0).toString();
        if (v instanceof java.time.LocalDateTime) return ((java.time.LocalDateTime) v).withNano(0).toString();
        return String.valueOf(v).trim();
    }

    private static final String[] MON = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

    /** ToString("dd-MMM-yyyy") for the GroupCaption of 236. */
    private static String ddMMMyyyy(Object v) {
        String s = iso(v);
        if (s.length() < 10) return s;
        try {
            LocalDate d = LocalDate.parse(s);
            return String.format("%02d-%s-%d", d.getDayOfMonth(), MON[d.getMonthValue() - 1], d.getYear());
        } catch (DateTimeParseException e) { return s; }
    }
}
