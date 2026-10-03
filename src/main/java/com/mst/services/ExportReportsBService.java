package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.ExportReportsBRepository;
import com.mst.repositories.ExportReportsBRepository.ComparisonFilter;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.math.BigDecimal;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.format.DateTimeParseException;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.LinkedHashSet;
import java.util.List;
import java.util.Map;
import java.util.Set;

import static com.mst.repositories.ExportReportsBRepository.ci;

/**
 * Module 17 "Export Reports", group B - the BLL side of eleven desktop report forms:
 *
 *   254  ExportPendingShipmentsFollowupReport            "5014 Shipment Tracking Follow up"
 *   261  frmAuditByWeightReport (Audit_Dashboard)         "5016 Export Shipment Weight Audit"
 *   262  ExportLoadingSheet                               "5013 Loading Sheet"
 *   266  frmExportShipingLineBookingRpt                   "5010 Shipment CRO Booking Report"
 *   269  ExImForwardingHistory                            "5012 Forwarding Report"
 *   270  ExportSalesReport                                "5004 Sale Report"
 *   759  ExportCustomerWiseComparisonsSummeryReport       "5007 Sale Comparisons Report"
 *   912  frmServiceBillHistory (Service)                  "Service Bill Register"
 *   913  ExImForwardingCostingReport                      "Export Forwarding (Costing Report)"
 *   919  frmExImShipmentCostingSummary                    "5005 Shipment Costing Summary"
 *   935  frmFinancialInstrumentAdvanceBalanceSummary      "5017 FI Utilization Report"
 *
 * Rights: DesktopReportRights.require(user, <ScreenDefinition.Id>, "View") on every read (the desktop forms
 * check no right of their own - they open from the menu, which is the View right). Organisation, company,
 * branch and financial year come from the session, never from the request. Rows are projected to the
 * desktop DataTable columns (the "dt" each form builds before binding its grid), in the desktop's order;
 * dates go out as ISO strings, the page formats them (dd-MMM-yy / ToShortDateString) as the grid did.
 */
@Service
public class ExportReportsBService {

    public static final int SCREEN_254 = 254, SCREEN_261 = 261, SCREEN_262 = 262, SCREEN_266 = 266, SCREEN_269 = 269,
            SCREEN_270 = 270, SCREEN_759 = 759, SCREEN_912 = 912, SCREEN_913 = 913, SCREEN_919 = 919, SCREEN_935 = 935;

    @Autowired private ExportReportsBRepository repo;
    @Autowired private CurrentUserContext currentUserContext;
    @Autowired private DesktopReportRights rights;

    private UserAccount user(int screenId) {
        UserAccount u = currentUserContext.requireAccountingUser();
        rights.require(u, screenId, "View");
        return u;
    }

    /** CommonServices.DateType() - a fixed DataTable on the desktop (kept fixed here). */
    public static List<Map<String, Object>> dateTypes() {
        List<Map<String, Object>> out = new ArrayList<>();
        String[] names = { "This Day", "This Week", "This Month", "This Year", "Financial Year" };
        for (int i = 0; i < names.length; i++) { Map<String, Object> m = new LinkedHashMap<>(); m.put("Id", i + 1); m.put("Parameters", names[i]); out.add(m); }
        return out;
    }

    private Map<String, Object> base(UserAccount u) {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("dateTypes", dateTypes());
        out.put("yearStart", iso(repo.financialYearStart(u, currentUserContext.currentFinancialYearId())));
        return out;
    }

    // ============================================================================ 254 Shipment Tracking Follow up

    /** Statusfill (look-up type 5), CurrentStatusfill (type 6), AllcomboBind (Customer / DestinationPort). */
    public Map<String, Object> setup254() {
        UserAccount u = user(SCREEN_254);
        Map<String, Object> out = base(u);
        put(out, "documentStatuses", () -> idName(repo.lookUpsByType(u, 5), "Id", "LookUpName"));
        put(out, "currentStatuses", () -> idName(repo.lookUpsByType(u, 6), "Id", "LookUpName"));
        put(out, "combos", () -> combos254(u));
        return out;
    }

    /** btnRefresh_Click - Statusfill, CurrentStatusfill, AllcomboBind. */
    public Map<String, Object> refresh254() { return setup254(); }

    private Map<String, Object> combos254(UserAccount u) {
        Map<String, Object> m = new LinkedHashMap<>();
        List<Map<String, Object>> customers = new ArrayList<>(), ports = new ArrayList<>();
        for (Map<String, Object> r : repo.dropDownFromExportInvoice(u)) {
            String t = text(ci(r, "ActivityType"));
            if ("Customer".equals(t)) customers.add(pair(r, "Id", "name"));
            else if ("DestinationPort".equals(t)) ports.add(pair(r, "Id", "name"));
        }
        m.put("customers", customers);
        m.put("ports", ports);
        return m;
    }

    /**
     * Gridfill(SortNo): @DocumentStatusIds is built by the desktop from the combo text -> ",id,id" (leading
     * comma kept, as the desktop concatenates "," + id); @CurrentStatusIds from the multi-select helper;
     * ActivityId 1 = Pending (rdNotRealized), 2 = Complete, 0 = All; StatusId 0 All / 1 / 2 / 3 by the shipment radios.
     * Quirk reproduced: the "ETADate" grid column is filled from the ETADestination source column (Gridfill :383),
     * FcyId is stored as text, the second table drives the ETA-destination bucket grid.
     */
    public Map<String, Object> show254(Map<String, Object> f) {
        UserAccount u = user(SCREEN_254);
        String docStatus = "";
        for (String s : text(f.get("documentStatusIds")).split(",")) if (!s.trim().isEmpty()) docStatus += "," + s.trim();
        List<List<Map<String, Object>>> ds = repo.pendingShipmentsFollowup(u, asInt(f.get("supplierCustomerId")), asInt(f.get("destinationPortId")),
                docStatus, text(f.get("currentStatusIds")), asInt(f.get("activityId")), asInt(f.get("statusId")), asInt(f.get("sortNo")));
        String[][] cols = {
                {"InvoiceDocumentTypeId", "InvoiceDocumentTypeId", "i"}, {"InvoiceId", "InvoiceId", "i"}, {"ContractId", "ContractId", "i"},
                {"Customer", "Customer", "s"}, {"LcOrderNo", "LcOrderNo", "s"}, {"InvoiceNo", "InvoiceNo", "s"}, {"InvoiceDate", "InvoiceDate", "dt"},
                {"FclQty", "FclQty", "d"}, {"Mtons", "Mtons", "d"}, {"PortName", "PortName", "s"}, {"InspectionDate", "InspectionDate", "dt"},
                {"CroBooking", "CroBooking", "dt"}, {"ETADate", "ETADestination", "dt"}, {"ETDDate", "ETDDate", "dt"}, {"CutOffDate", "CutOffDate", "dt"},
                {"LoadingStart", "LoadingStart", "dt"}, {"LoadingEnd", "LoadingEnd", "dt"}, {"LoadOnTrainDate", "LoadOnTrainDate", "dt"},
                {"ReachedAtLoadingPortDate", "ReachedAtLoadingPortDate", "dt"}, {"LoadedOnVesselDate", "LoadedOnVesselDate", "dt"}, {"BLDate", "BLDate", "dt"},
                {"DocumentStatus", "DocumentStatus", "s"}, {"DocToBank", "DocToBank", "dt"}, {"DocToParty", "DocToParty", "dt"}, {"CurrentStatus", "CurrentStatus", "s"},
                {"ETADestination", "ETADestination", "dt"}, {"ExpectedInTransitDays", "ExpectedInTransitDays", "i"}, {"InTransitDays", "InTransitDays", "i"},
                {"ETDFinalDate", "ETDFinalDate", "dt"}, {"FcyId", "FcyId", "s"}, {"FcyCode", "CurrencyCode", "s"}, {"InvoiceValue", "InvoiceValue", "d"},
                {"InvoiceStatus", "InvoiceStatus", "s"}, {"RealizedAmount", "RealizedAmount", "d"}, {"BalAmount", "BalAmount", "d"}, {"ShippingLine", "ShippingLine", "s"},
                {"BLNumber", "BLNumber", "s"}, {"NoOfAttachments", "NoOfAttachments", "i"} };
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rows", project(ds.get(0), cols));
        out.put("eta", project(ds.get(1), new String[][] { {"SortNo", "SortNo", "i"}, {"ETA-DestinationPort", "ETADestinationPort", "s"}, {"M.Tons", "Mtons", "d"}, {"FCL", "FCL", "d"} }));
        return out;
    }

    /** grd_LinkClicked "CroBooking" / CommonServices.BookingInfoSlip560_AgainstInvoice - the row check before the 560 print. */
    public Map<String, Object> croBookingExists(int invoiceId, int screenId) {
        UserAccount u = user(screenId);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("found", invoiceId != 0 && !repo.bookingSlipAndRegister(u, invoiceId, 0, 0, 0).isEmpty());
        return out;
    }

    // ============================================================================ 261 Export Shipment Weight Audit

    /** ParameterFill + ComboFill (InvoiceNo / ContractNo from USP_GetDataForDropDownFromDeliveryOrder). */
    public Map<String, Object> setup261() {
        UserAccount u = user(SCREEN_261);
        Map<String, Object> out = base(u);
        put(out, "combos", () -> {
            Map<String, Object> m = new LinkedHashMap<>();
            List<Map<String, Object>> inv = new ArrayList<>(), con = new ArrayList<>();
            for (Map<String, Object> r : repo.dropDownFromDeliveryOrder(u)) {
                String a = text(ci(r, "Activity"));
                if ("InvoiceNo".equals(a)) inv.add(pair(r, "Id", "ReferenceName"));
                if ("ContractNo".equals(a)) con.add(pair(r, "Id", "ReferenceName"));
            }
            m.put("invoices", inv); m.put("contracts", con);
            return m;
        });
        return out;
    }

    /**
     * GridBind: the raw report rows (one per gate pass) go to the page, which builds the "Main Information"
     * grid (first row per InvoiceNo + GpCount) and the "Detail Information" grid of the selected invoice
     * exactly as grd_SelectionChanged does. Status: All -> not sent, "In Process", "Completed".
     */
    public List<Map<String, Object>> show261(Map<String, Object> f) {
        UserAccount u = user(SCREEN_261);
        String[][] cols = {
                {"SupplierCustomerId", "SupplierCustomerId", "i"}, {"CustomerName", "CustomerName", "s"}, {"LcOrderNo", "LcOrderNo", "s"}, {"InvoiceId", "InvoiceId", "i"},
                {"InvoiceNo", "InvoiceNo", "s"}, {"InvoiceDate", "InvoiceDate", "dt"}, {"Containers", "Containers", "i"}, {"InvoiceGwTotal", "InvoiceGwTotal", "d"},
                {"DoGwTotal", "DoGwTotal", "d"}, {"WbGwTotal", "WbGwTotal", "d"}, {"ExcessWtTotal", "ExcessWtTotal", "d"}, {"LessExcess", "LessExcess", "s"},
                {"AvgLessExcess", "AvgLessExcess", "d"}, {"LessExcessAmount", "LessExcessAmount", "d"}, {"InvoiceNwTotal", "InvoiceNwTotal", "d"}, {"DoNwTotal", "DoNwTotal", "d"},
                {"InvNwDoNwDiff", "InvNwDoNwDiff", "d"}, {"InvoiceStatus", "InvoiceStatus", "s"},
                {"GpDate", "GpDate", "dt"}, {"GpId", "GpId", "i"}, {"GpSrNo", "GpSrNo", "i"}, {"DOId", "DOId", "i"}, {"DONo", "DONo", "i"}, {"Container1", "Container1", "s"},
                {"Container2", "Container2", "s"}, {"doNetWeight", "doNetWeight", "d"}, {"doPackingWeight", "doPackingWeight", "d"}, {"doGrossWeight", "doGrossWeight", "d"},
                {"doOtherWeight", "doOtherWeight", "d"}, {"WbWeight", "WbWeight", "d"}, {"ExcessWt", "ExcessWt", "d"}, {"LessExcessDoWise", "LessExcessDoWise", "s"} };
        return project(repo.salesAuditByWeight(u, asInt(f.get("invoiceId")), asInt(f.get("contractId")), sqlDate(f.get("fromDate")), sqlDate(f.get("toDate")),
                text(f.get("status"))), cols);
    }

    /** grd_LinkClicked "InvoiceNo": ExportSalesAuditByWeight(EximInvoiceId only) -> row check before the 618 print. */
    public Map<String, Object> auditByInvoiceExists(int invoiceId) {
        UserAccount u = user(SCREEN_261);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("found", invoiceId != 0 && !repo.salesAuditByWeight(u, invoiceId, 0, null, null, null).isEmpty());
        return out;
    }

    // ============================================================================ 262 Loading Sheet

    /** AllComboBind (Invoice / Customer from USP_GetDataForDropDownFromGoodsForwarding) + Datetypefill. */
    public Map<String, Object> setup262() {
        UserAccount u = user(SCREEN_262);
        Map<String, Object> out = base(u);
        put(out, "combos", () -> {
            Map<String, Object> m = new LinkedHashMap<>();
            List<Map<String, Object>> inv = new ArrayList<>(), party = new ArrayList<>();
            for (Map<String, Object> r : repo.dropDownFromGoodsForwarding(u, null)) {
                String a = text(ci(r, "Activity"));
                if ("Invoice".equals(a)) inv.add(pair(r, "Id", "ReferenceName"));
                if ("Customer".equals(a)) party.add(pair(r, "Id", "ReferenceName"));
            }
            m.put("invoices", inv); m.put("parties", party);
            return m;
        });
        return out;
    }

    /** GridFill - From/To always sent (plain pickers). */
    public List<Map<String, Object>> show262(Map<String, Object> f) {
        UserAccount u = user(SCREEN_262);
        String[][] cols = { {"Id", "Id", "i"}, {"LoadingDate", "LoadingDate", "dt"}, {"InvoiceNo", "InvoiceNo", "s"}, {"ContainerNo", "ContainerNo", "s"},
                {"SealNo", "SealNo", "s"}, {"NoofBagsMaster", "NoofBagsMaster", "d"}, {"NetWeightBag", "NetWeightBagMaster", "d"}, {"GrossWeightBag", "GrossWeightBagMaster", "d"},
                {"NetWeight", "NetWeight", "d"}, {"GrossWeight", "GrossWeight", "d"}, {"BrandName", "BrandName", "s"} };
        return project(repo.exportLoadSheet(u, asInt(f.get("invoiceId")), asInt(f.get("supplierCustomerId")), sqlDate(f.get("fromDate")), sqlDate(f.get("toDate"))), cols);
    }

    // ============================================================================ 266 Shipment CRO Booking Report

    /** AllComboBind - Customer / InvoiceNo / DestinationPort from USP_GetDataForDropDownFromShippingBookingInfo. */
    public Map<String, Object> setup266() {
        UserAccount u = user(SCREEN_266);
        Map<String, Object> out = new LinkedHashMap<>();
        put(out, "combos", () -> {
            Map<String, Object> m = new LinkedHashMap<>();
            List<Map<String, Object>> cust = new ArrayList<>(), inv = new ArrayList<>(), port = new ArrayList<>();
            for (Map<String, Object> r : repo.dropDownFromShippingBookingInfo(u)) {
                String a = text(ci(r, "Activity"));
                if ("Customer".equals(a)) cust.add(pair(r, "Id", "ReferenceName"));
                else if ("InvoiceNo".equals(a)) inv.add(pair(r, "Id", "ReferenceName"));
                else if ("DestinationPort".equals(a)) port.add(pair(r, "Id", "ReferenceName"));
            }
            m.put("customers", cust); m.put("invoices", inv); m.put("ports", port);
            return m;
        });
        return out;
    }

    /** GridBind - the register rows made distinct by Id (group by Id -> first), then the 18 grid columns. */
    public List<Map<String, Object>> show266(Map<String, Object> f) {
        UserAccount u = user(SCREEN_266);
        List<Map<String, Object>> raw = repo.bookingSlipAndRegister(u, asInt(f.get("invoiceId")), 0, asInt(f.get("supplierCustomerId")), asInt(f.get("destinationPortId")));
        List<Map<String, Object>> distinct = new ArrayList<>();
        Set<Integer> seen = new LinkedHashSet<>();
        for (Map<String, Object> r : raw) if (seen.add(asInt(ci(r, "Id")))) distinct.add(r);
        String[][] cols = { {"Id", "Id", "i"}, {"InvoiceNo", "InvoiceNo", "s"}, {"BookingDate", "BookingDate", "dt"}, {"BookingCroNo", "BookingCroNo", "s"},
                {"CustomerName", "CustomerName", "s"}, {"ClearingAgentName", "ClearingAgentName", "s"}, {"TransporterName", "TransporterName", "s"},
                {"CarierMedium", "CarierMedium", "s"}, {"VesselName", "VesselName", "s"}, {"VoyageNo", "VoyageNo", "s"}, {"NoofContainers", "ContrsQty", "i"},
                {"ETADate", "ETADate", "dt"}, {"ETDDate", "ETDDate", "dt"}, {"CuttOFFDate", "CuttOFFDate", "dt"}, {"TransitDays", "TransitDays", "i"},
                {"DestinationPortName", "destinationPortName", "s"}, {"FreeDaysAtDestination", "FreeDaysAtDestinations", "s"}, {"BookingRate", "BookingRate", "d"} };
        return project(distinct, cols);
    }

    /** grdfrm_LinkClicked "BookingCroNo": PrintSlipandRegister(Id) row check before the 560 print. */
    public Map<String, Object> croBookingByIdExists(int id) {
        UserAccount u = user(SCREEN_266);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("found", id != 0 && !repo.bookingSlipAndRegister(u, 0, id, 0, 0).isEmpty());
        return out;
    }

    // ============================================================================ 269 Forwarding Report / 913 Forwarding Costing Report

    /** Load of both forwarding forms: Customer / Item / ContractNo / Warehouse / WarehouseTo activities, invoices (no customer), date types. */
    public Map<String, Object> setupForwarding(int screenId) {
        UserAccount u = user(screenId);
        Map<String, Object> out = base(u);
        put(out, "customers", () -> idName(repo.dropDownFromGoodsForwarding(u, "Customer"), "Id", "ReferenceName"));
        put(out, "invoices", () -> idName(repo.exportInvoiceNos(u, 0), "Id", "InvoiceNo"));
        put(out, "items", () -> idName(repo.dropDownFromGoodsForwarding(u, "Item"), "Id", "ReferenceName"));
        put(out, "contracts", () -> idName(repo.dropDownFromGoodsForwarding(u, "ContractNo"), "Id", "ReferenceName"));
        put(out, "warehousesFrom", () -> idName(repo.dropDownFromGoodsForwarding(u, "Warehouse"), "Id", "ReferenceName"));
        put(out, "warehousesTo", () -> idName(repo.dropDownFromGoodsForwarding(u, "WarehouseTo"), "Id", "ReferenceName"));
        return out;
    }

    /** btnRefresh_Click: CustomerGetAll, ContractNoFill, ItemGetAll. */
    public Map<String, Object> refreshForwarding(int screenId) {
        UserAccount u = user(screenId);
        Map<String, Object> out = new LinkedHashMap<>();
        put(out, "customers", () -> idName(repo.dropDownFromGoodsForwarding(u, "Customer"), "Id", "ReferenceName"));
        put(out, "contracts", () -> idName(repo.dropDownFromGoodsForwarding(u, "ContractNo"), "Id", "ReferenceName"));
        put(out, "items", () -> idName(repo.dropDownFromGoodsForwarding(u, "Item"), "Id", "ReferenceName"));
        return out;
    }

    /** cmbSupplierName_Leave -> InvoiceNoFill(CmbSupplierName.Value). */
    public List<Map<String, Object>> forwardingInvoices(int screenId, int supplierCustomerId) {
        return idName(repo.exportInvoiceNos(user(screenId), supplierCustomerId), "Id", "InvoiceNo");
    }

    /**
     * GridBind of 269 (@Activity 'Detail', 46 columns) and 913 (@Activity 'Summary', 42 columns). From/To and the
     * GP dates only when their pickers are ticked; GP serial numbers only when non-zero.
     */
    public List<Map<String, Object>> showForwarding(int screenId, Map<String, Object> f) {
        UserAccount u = user(screenId);
        boolean detail = screenId == SCREEN_269;
        List<Map<String, Object>> raw = repo.forwardingSlipAndRegister(u, asInt(f.get("invoiceId")), sqlDate(f.get("fromDate")), sqlDate(f.get("toDate")),
                sqlDate(f.get("gpDateFrom")), sqlDate(f.get("gpDateTo")), asInt(f.get("gpSrNoFrom")), asInt(f.get("gpSrNoTo")),
                asInt(f.get("supplierCustomerId")), asInt(f.get("itemId")), asInt(f.get("contractId")), asInt(f.get("fromWarehouseId")),
                asInt(f.get("toWarehouseId")), detail ? "Detail" : "Summary");
        List<String[]> cols = new ArrayList<>();
        cols.add(new String[] {"Id", "Id", "i"}); cols.add(new String[] {"ContractNo", "ContractNo", "s"}); cols.add(new String[] {"InvoiceNo", "InvoiceNo", "s"});
        cols.add(new String[] {"DocNo", "DocNo", "i"}); cols.add(new String[] {"DocDate", "DocDate", "dt"}); cols.add(new String[] {"CustomerName", "CustomerName", "s"});
        cols.add(new String[] {"DeliveryTerm", "DeliveryTerm", "s"}); cols.add(new String[] {"FcyCode", "FcyCode", "s"}); cols.add(new String[] {"WarehouseFrom", "WareHouseFrom", "s"});
        cols.add(new String[] {"WarehouseTo", "WareHouseTo", "s"}); cols.add(new String[] {"ItemName", "ItemName", "s"}); cols.add(new String[] {"CropYear", "CropYear", "s"});
        cols.add(new String[] {"JobLot", "JobLot", "s"}); cols.add(new String[] {"PackingType", "PackingType", "s"});
        if (detail) {
            cols.add(new String[] {"ItemQty", "ItemQty", "d"}); cols.add(new String[] {"UOM", "PackUom", "s"}); cols.add(new String[] {"InnerQty", "InnerQty", "d"});
            cols.add(new String[] {"InnerUomCode", "InnerUomCode", "s"}); cols.add(new String[] {"InnerEbUnit", "InnerEbUnit", "d"}); cols.add(new String[] {"InnerEbTotal", "InnerEbTotal", "d"});
        } else {
            cols.add(new String[] {"UOM", "PackUom", "s"}); cols.add(new String[] {"ItemQty", "ItemQty", "d"});
        }
        cols.add(new String[] {"GrossWeight", "GrossWeight", "d"}); cols.add(new String[] {"EbUnit", "EbUnit", "d"}); cols.add(new String[] {"EbTotal", "EbTotal", "d"});
        cols.add(new String[] {"AdLs", "AdLsWeight", "d"}); cols.add(new String[] {"NetWeight", "NetWeight", "d"}); cols.add(new String[] {"StockWeight", "StockWeight", "d"});
        cols.add(new String[] {"MTon", "MTon", "d"}); cols.add(new String[] {"CostingContribution", "CostingContribution", "d"}); cols.add(new String[] {"ProfitLoss", "ProfitLoss", "d"});
        cols.add(new String[] {"NoOfContainer", "NoOfContainer", "i"}); cols.add(new String[] {"LoadingName", "LoadingPortName", "s"}); cols.add(new String[] {"DestinationName", "DestinationPortName", "s"});
        cols.add(new String[] {"Continent", "ContinentName", "s"}); cols.add(new String[] {"Container", "Container", "s"}); cols.add(new String[] {"SealNo", "SealNo", "s"});
        if (detail) { cols.add(new String[] {"InspectionLotNo", "InspectionNo", "s"}); cols.add(new String[] {"InspectionSubLotNo", "InspectionSubLotNo", "s"}); }
        cols.add(new String[] {"GpDate", "GpDate", "dt"}); cols.add(new String[] {"GpNo", "GpNo", "s"}); cols.add(new String[] {"DoNo", "DoNo", "s"}); cols.add(new String[] {"DoDate", "DoDate", "dt"});
        cols.add(new String[] {"VehicleNo", "VehicleNo", "s"}); cols.add(new String[] {"BiltyNo", "BiltyNo", "s"}); cols.add(new String[] {"TransporterName", "TransporterName", "s"});
        cols.add(new String[] {"Freight", "Freight", "d"}); cols.add(new String[] {"DriverName", "DriverName", "s"}); cols.add(new String[] {"DriverCellNo", "DriverCellNo", "s"});
        cols.add(new String[] {"DriverCnicNo", "DriverCnicNo", "s"});
        return project(raw, cols.toArray(new String[0][]));
    }

    // ============================================================================ 270 Sale Report

    /** InvoiceBind, CustomerGetAll, GetExportItemsFromVouchers, GetExportJobLotsFromVouchers - all with the active FinancialYearId. */
    public Map<String, Object> setup270() {
        UserAccount u = user(SCREEN_270);
        int fy = currentUserContext.currentFinancialYearId();
        Map<String, Object> out = base(u);
        put(out, "invoices", () -> idName(repo.fromVouchers(u, "USP_GetExportInvoicesFromVouchers", fy), "Id", "InvoiceNo"));
        put(out, "customers", () -> idName(repo.fromVouchers(u, "USP_GetExportCustomerFromVouchers", fy), "Id", "CustomerName"));
        put(out, "items", () -> idName(repo.fromVouchers(u, "USP_GetExportItemsFromVouchers", fy), "Id", "ItemName"));
        put(out, "jobLots", () -> idName(repo.fromVouchers(u, "USP_GetExportJobLotsFromVouchers", fy), "Id", "JobLot"));
        return out;
    }

    /** GridBind - From/To only when ticked, ActionId 1 Not Realized / 2 Realized / 0 All; FinancialYearId always. */
    public List<Map<String, Object>> show270(Map<String, Object> f) {
        UserAccount u = user(SCREEN_270);
        String[][] cols = { {"VoucherCode", "VoucherCode", "i"}, {"VoucherDate", "VoucherDate", "dt"}, {"DueDate", "DueDate", "dt"}, {"BLDate", "BLDate", "dt"},
                {"InvoiceNo", "InvoiceNo", "s"}, {"CustomerName", "CustomerName", "s"}, {"ItemName", "ItemName", "s"}, {"MTon", "MTon", "d"}, {"JobLot", "JobLot", "s"},
                {"FcAmount", "FcAmount", "d"}, {"FcyCode", "CurrencyCode", "s"}, {"ExchangeRate", "ExchangeRate", "d"}, {"Amount", "VoucherAmount", "d"},
                {"RealizedAmount", "RealizedAmount", "d"}, {"InvoiceStatus", "InvoiceStatus", "s"} };
        return project(repo.exportSales(u, currentUserContext.currentFinancialYearId(), asInt(f.get("supplierCustomerId")), asInt(f.get("invoiceId")),
                sqlDate(f.get("fromDate")), sqlDate(f.get("toDate")), asInt(f.get("itemId")), asInt(f.get("jobLotId")), asInt(f.get("actionId"))), cols);
    }

    // ============================================================================ 759 Sale Comparisons Report

    /** ReportTypeBind (SpStaticColumnNames 'ExportComparisonsSummeryNew') + ComboFill (eight ActivityType families). */
    public Map<String, Object> setup759() {
        UserAccount u = user(SCREEN_759);
        Map<String, Object> out = base(u);
        put(out, "reportTypes", () -> idName(repo.staticColumnNames("ExportComparisonsSummeryNew"), "ReportId", "ReportName"));
        put(out, "combos", () -> combos759(u));
        return out;
    }

    private Map<String, Object> combos759(UserAccount u) {
        String[] families = { "ItemCategory", "ItemType", "Items", "ContractNo", "Customer", "SalesPerson", "DestinationPort", "Invoice" };
        Map<String, List<Map<String, Object>>> m = new LinkedHashMap<>();
        for (String k : families) m.put(k, new ArrayList<>());
        for (Map<String, Object> r : repo.dropDownFromExportInvoiceVoucher(u)) {
            List<Map<String, Object>> l = m.get(text(ci(r, "ActivityType")));
            if (l != null) l.add(pair(r, "Id", "name"));
        }
        return new LinkedHashMap<>(m);
    }

    /**
     * GridBind: "Report Type filed is required" without a report type; "Detail" -> usp_ExportSaleDetail_report (34 grid
     * columns), anything else -> SpExImInvoice_ExportComparisonsSummery_Report projected to GroupCaption / GroupName
     * [/ SecondGroupCaption / SecondGroupName] + the seven measures. Which print buttons show is decided by the page
     * from the same report-type text.
     */
    public Map<String, Object> show759(Map<String, Object> f) {
        UserAccount u = user(SCREEN_759);
        String reportType = text(f.get("reportType"));
        if (asInt(f.get("reportTypeId")) == 0 || reportType.isEmpty()) throw new IllegalArgumentException("Report Type filed is required");
        ComparisonFilter cf = new ComparisonFilter();
        cf.from = sqlDate(f.get("fromDate")); cf.to = sqlDate(f.get("toDate"));
        cf.itemCategoryId = asInt(f.get("itemCategoryId")); cf.itemTypeId = asInt(f.get("itemTypeId")); cf.itemId = asInt(f.get("itemId"));
        cf.supplierCustomerId = asInt(f.get("supplierCustomerId")); cf.salePersonId = asInt(f.get("salePersonId")); cf.lcContractId = asInt(f.get("contractId"));
        cf.exImInvoiceId = asInt(f.get("invoiceId")); cf.destinationPortId = asInt(f.get("destinationPortId")); cf.itemStockAccountId = asInt(f.get("stockAccountId"));
        cf.reportType = reportType;
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("reportType", reportType);
        if ("Detail".equals(reportType)) {
            String[][] cols = { {"VoucherDate", "DocDate", "dt"}, {"VoucherNo", "DocNo", "i"}, {"InvoiceDocTypeId", "InvoiceDocTypeId", "i"}, {"ExImInvoiceId", "ExImInvoiceId", "i"},
                    {"InvoiceNo", "InvoiceNo", "s"}, {"InvoiceDate", "InvoiceDate", "dt"}, {"ContractId", "ContractId", "i"}, {"ContractNo", "ContractNo", "s"}, {"ContractDate", "ContractDate", "dt"},
                    {"EFormNo", "EFormNo", "s"}, {"EFormDate", "EFormDate", "dt"}, {"CustomerName", "CustomerName", "s"}, {"ItemId", "ItemId", "i"}, {"ItemName", "ItemName", "s"},
                    {"JobLot", "JobLotCode", "s"}, {"PackingType", "PackingType", "s"}, {"Qty", "Qty", "d"}, {"MTons", "MTons", "d"}, {"NoOfContainers", "NoOfContainers", "i"},
                    {"PackingSize", "PackingSize", "s"}, {"RatePrice", "RatePrice", "d"}, {"FcyCode", "CurrencySymbol", "s"}, {"RateUom", "RateUom", "s"}, {"FcyAmount", "Fcy_Amount", "d"},
                    {"ExchangeRate", "ExchangeRate", "d"}, {"LcyAmount", "Lcy_Amount", "d"}, {"PaymentTerms", "PaymentTerms", "s"}, {"DestinationPort", "DestinationPort", "s"},
                    {"BLNumber", "BLNumber", "s"}, {"ContainerNos", "ContainerNos", "s"}, {"DocStatus", "DocStatus", "s"}, {"PaymentStatus", "PaymentStatus", "s"},
                    {"ExImFarmingTypes", "ExImFarmingTypes", "s"}, {"NoOfAttachments", "NoOfAttachments", "i"} };
            out.put("rows", project(repo.exportSaleDetail(u, cf), cols));
        } else {
            boolean second = "Customer And Item Wise Comparison".equals(reportType) || "Item And Customer Wise Comparison".equals(reportType);
            List<String[]> cols = new ArrayList<>();
            if (second) {
                cols.add(new String[] {"GroupCaption", "FirstGroupCaption", "s"}); cols.add(new String[] {"GroupName", "FirstGroupName", "s"});
                cols.add(new String[] {"SecondGroupCaption", "SecondGroupCaption", "s"}); cols.add(new String[] {"SecondGroupName", "SecondeGroupName", "s"});
            } else {
                cols.add(new String[] {"GroupCaption", "GroupCaption", "s"}); cols.add(new String[] {"GroupName", "GroupName", "s"});
            }
            for (String[] c : new String[][] { {"MTons", "MTons", "d"}, {"PrctExport", "PrctExport", "d"}, {"FcyCode", "FcyCode", "s"}, {"AvgPrice", "AvgPrice", "d"},
                    {"Fcy_Amount", "Fcy_Amount", "d"}, {"FcrAvgRate", "FcrAvgRate", "d"}, {"PKR_Amount", "PKR_Amount", "d"} }) cols.add(c);
            out.put("hasSecondGroup", second);
            out.put("rows", project(repo.comparisonsSummary(u, cf), cols.toArray(new String[0][])));
        }
        return out;
    }

    // ============================================================================ 912 Service Bill Register

    /** ParameterFill + AllComboBind (Customer / Invoice / LoadingPort / DestinationPort / Items). */
    public Map<String, Object> setup912() {
        UserAccount u = user(SCREEN_912);
        Map<String, Object> out = base(u);
        put(out, "combos", () -> {
            Map<String, List<Map<String, Object>>> m = new LinkedHashMap<>();
            for (String k : new String[] { "Customer", "Invoice", "LoadingPort", "DestinationPort", "Items" }) m.put(k, new ArrayList<>());
            for (Map<String, Object> r : repo.dropDownFromClearingAgentBill(u)) {
                List<Map<String, Object>> l = m.get(text(ci(r, "ActivityType")));
                if (l != null) l.add(pair(r, "Id", "name"));
            }
            return new LinkedHashMap<>(m);
        });
        return out;
    }

    /** gridHisory - From/To always sent. */
    public List<Map<String, Object>> show912(Map<String, Object> f) {
        UserAccount u = user(SCREEN_912);
        String[][] cols = { {"Id", "Id", "i"}, {"DocNo", "DocNo", "i"}, {"DocDate", "DocDate", "dt"}, {"InvoiceNo", "InvoiceNo", "s"}, {"PartyName", "PartyName", "s"},
                {"RefBillNo", "RefBillNo", "s"}, {"ContractNo", "ContractNo", "s"}, {"NoOfContainer", "NoOfContainer", "d"}, {"MTon", "MTon", "d"}, {"BookingRate", "BookingRate", "d"},
                {"ItemName", "ItemName", "s"}, {"UomCode", "Uom", "s"}, {"Qty", "Qty", "d"}, {"ChargesRate", "ChargesRate", "d"}, {"CurrencyCode", "CurrencyCode", "s"},
                {"FcyAmount", "FcyAmount", "d"}, {"ExhangeRate", "ExhangeRate", "d"}, {"Amount", "Amount", "d"}, {"OtherChargesAmount", "OtherChargesAmount", "d"},
                {"TotalAmount", "TotalAmount", "d"}, {"Description", "Description", "s"}, {"LoadingPort", "LoardingPort", "s"}, {"DestinationPort", "DestinationPort", "s"},
                {"DebitAccount", "DebitAccount", "s"}, {"DueDays", "DueDays", "i"}, {"DueDate", "DueDate", "dt"} };
        return project(repo.servicesBillSlipAndRegister(u, 0, asInt(f.get("invoiceId")), asInt(f.get("supplierCustomerId")), asInt(f.get("itemId")),
                sqlDate(f.get("fromDate")), sqlDate(f.get("toDate")), asInt(f.get("loadingPortId")), asInt(f.get("destinationPortId"))), cols);
    }

    // ============================================================================ 919 Shipment Costing Summary

    /** ComboFill - Customer / Invoice / DestinationPort from USP_GetDataForDropDownFromExportInvoiceVoucher. */
    public Map<String, Object> setup919() {
        UserAccount u = user(SCREEN_919);
        Map<String, Object> out = base(u);
        put(out, "combos", () -> {
            Map<String, List<Map<String, Object>>> m = new LinkedHashMap<>();
            for (String k : new String[] { "Customer", "Invoice", "DestinationPort" }) m.put(k, new ArrayList<>());
            for (Map<String, Object> r : repo.dropDownFromExportInvoiceVoucher(u)) {
                List<Map<String, Object>> l = m.get(text(ci(r, "ActivityType")));
                if (l != null) l.add(pair(r, "Id", "name"));
            }
            return new LinkedHashMap<>(m);
        });
        return out;
    }

    /** GridBind - 34 columns; the page hides the eight charge columns whose total is 0 as GridSetting does. */
    public List<Map<String, Object>> show919(Map<String, Object> f) {
        UserAccount u = user(SCREEN_919);
        String[][] cols = { {"SupplierCustomerId", "SupplierCustomerId", "i"}, {"CustomerName", "CustomerName", "s"}, {"ContractIds", "ContractIds", "s"}, {"ContractNo's", "ContractNos", "s"},
                {"InvoiceId", "InvoiceId", "i"}, {"InvoiceNo", "InvoiceNo", "s"}, {"PaymentTerms", "PaymentTerms", "s"}, {"DeliveryTerm", "DeliveryTerm", "s"}, {"InvoiceDate", "InvoiceDate", "dt"},
                {"InvoiceWeight", "InvoiceNetWeight", "d"}, {"InvoiceMTon", "InvoiceNetMTon", "d"}, {"FcyCode", "FcyCode", "s"}, {"ExchangeRate", "ExchangeRate", "d"},
                {"InvoiceAmount", "InvoiceTotalAmount", "d"}, {"RealizedAmount", "RealizedAmount", "d"}, {"InvoiceBalance", "InvoiceBalance", "d"}, {"SaleValue", "SalesValueTotal", "d"},
                {"CommissionValue", "CommissionValueTotal", "d"}, {"StockValue", "StockValueTotal", "d"}, {"PackingValue", "PackingMaterialValueTotal", "d"}, {"LogisticsValue", "LogisticsValue", "d"},
                {"CustomClearing", "CustomClearing", "d"}, {"ForwarderCharges", "ForwarderCharges", "d"}, {"Transportation", "Transportation", "d"}, {"OtherCharges", "OtherCharges", "d"},
                {"PortAndClearing", "PortAndClearing", "d"}, {"OceanFreight", "OceanFreight", "d"}, {"InspectionAndTesting", "InspectionAndTesting", "d"}, {"Insurance", "Insurance", "d"},
                {"GainLossValue", "GainLossValueTotal", "d"}, {"RealizedLcyAmount", "RealizedLcyAmount", "d"}, {"PLValue", "PLValue", "d"}, {"PlPercent", "PLPercent", "d"},
                {"ShipmentWeight", "ShipmentWeight", "d"} };
        return project(repo.shipmentCostingSummary(u, sqlDate(f.get("fromDate")), sqlDate(f.get("toDate")), asInt(f.get("invoiceId")),
                asInt(f.get("supplierCustomerId")), asInt(f.get("destinationPortId"))), cols);
    }

    // ============================================================================ 935 FI Utilization Report

    /** suppliercustomer + BankNameFill ("ReceiverAccount") + ParameterFill. */
    public Map<String, Object> setup935() {
        UserAccount u = user(SCREEN_935);
        Map<String, Object> out = base(u);
        put(out, "customers", () -> idName(repo.customersByLcOrderAndFcReceipt(u), "Id", "CompanyName"));
        put(out, "banks", () -> idName(repo.dropDownFromFcyBankReceipts(u, "ReceiverAccount"), "Id", "ReferenceName"));
        return out;
    }

    /** gridHisory - From/To always, SkipZero 1 when ticked, ActionId 1 Party Export / 2 Bank Export. */
    public List<Map<String, Object>> show935(Map<String, Object> f) {
        UserAccount u = user(SCREEN_935);
        String[][] cols = { {"Id", "Id", "i"}, {"DocNo", "DocNo", "s"}, {"DocDate", "DocDate", "dt"}, {"DocType", "DocType", "s"}, {"SupplierCustomerId", "SupplierCustomerId", "i"},
                {"CustomerName", "CustomerName", "s"}, {"Consignee", "Consignee", "s"}, {"ContractNo", "ContractNo", "s"}, {"FI NO", "FINO", "s"}, {"FIDate", "FIDate", "dt"},
                {"FIExpiryDate", "FIExpiryDate", "dt"}, {"BankAccountId", "BankAccountId", "i"}, {"BankName", "BankName", "s"}, {"TotalAmount", "TotalAmount", "d"},
                {"AdjustedAmount", "AdjustedAmount", "d"}, {"AdjustedAgainstGdAmount", "AdjustedAgainstGdAmount", "d"}, {"BalanceAmount", "BalanceAmount", "d"}, {"FcyCode", "FcyCode", "s"},
                {"ExchangeRate", "ExchangeRate", "d"}, {"LcyAmount", "LcyAmount", "d"}, {"InvoiceNo", "InvoiceNo", "s"}, {"PaymenttermId", "PaymenttermId", "i"},
                {"DocumentTypeId", "DocumentTypeId", "i"}, {"DestinationPort", "DestinationPort", "s"} };
        return project(repo.fiAdvanceBalanceSummary(u, asInt(f.get("supplierCustomerId")), asInt(f.get("bankId")), sqlDate(f.get("fromDate")), sqlDate(f.get("toDate")),
                asBool(f.get("skipZero")) ? 1 : 0, asInt(f.get("actionId"))), cols);
    }

    /** frmFinancialInstrumentUtilizedDetailByFI.BindGdbreakUp - the popup grid (21 columns, 7 hidden on the page). */
    public List<Map<String, Object>> fiUtilizeDetail(int fiId, int documentTypeId) {
        user(SCREEN_935);
        String[][] cols = { {"FIId", "FIId", "i"}, {"FINo", "FINo", "s"}, {"FIDate", "FIDate", "dt"}, {"EximInvoiceId", "EximInvoiceId", "i"}, {"InvoiceNo", "InvoiceNo", "s"},
                {"InvoiceDate", "InvoiceDate", "dt"}, {"GDId", "GDId", "i"}, {"RefDocumentTypeId", "RefDocumentTypeId", "i"}, {"GdNo", "GdNo", "s"}, {"SupplierCustomerId", "SupplierCustomerId", "i"},
                {"CustomerName", "CustomerName", "s"}, {"ConsigneeId", "ConsigneeId", "i"}, {"Consignee", "Consignee", "s"}, {"BankId", "BankId", "i"}, {"BankAccount", "BankAccount", "s"},
                {"UtilizeAmount", "UtilizeAmount", "d"}, {"FcyCode", "FcyCode", "s"}, {"ExchangeRate", "ExchangeRate", "d"}, {"LcyAmount", "LcyAmount", "d"},
                {"InvoiceMTon", "InvoiceMTon", "d"}, {"InvoiceAmount", "InvoiceAmount", "d"} };
        return project(repo.fiUtilizeInfoByFIId(fiId, documentTypeId), cols);
    }

    // ============================================================================ helpers

    private interface Loader { Object load(); }

    /** Each desktop fill has its own try/catch + MessageBox; a failing one does not stop the others. */
    private static void put(Map<String, Object> out, String key, Loader l) {
        try { out.put(key, l.load()); }
        catch (Exception e) { out.put(key, new ArrayList<>()); out.put(key + "Error", root(e)); }
    }

    private static String root(Throwable e) {
        Throwable t = e;
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        return t.getMessage() == null ? e.toString() : t.getMessage();
    }

    /** DataTable projection: {outName, sourceColumn, type i|d|s|dt}. */
    private static List<Map<String, Object>> project(List<Map<String, Object>> rows, String[][] cols) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            for (String[] c : cols) {
                Object v = ci(r, c[1]);
                switch (c[2]) {
                    case "i": m.put(c[0], asInt(v)); break;
                    case "d": m.put(c[0], asDouble(v)); break;
                    case "dt": m.put(c[0], iso(v)); break;
                    default: m.put(c[0], text(v));
                }
            }
            out.add(m);
        }
        return out;
    }

    private static List<Map<String, Object>> idName(List<Map<String, Object>> rows, String idCol, String nameCol) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) out.add(pair(r, idCol, nameCol));
        return out;
    }

    private static Map<String, Object> pair(Map<String, Object> r, String idCol, String nameCol) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("Id", asInt(ci(r, idCol)));
        m.put("Name", text(ci(r, nameCol)));
        return m;
    }

    private static String text(Object v) { return v == null ? "" : String.valueOf(v).trim(); }

    private static int asInt(Object v) {
        if (v == null) return 0;
        if (v instanceof Number) return ((Number) v).intValue();
        if (v instanceof Boolean) return ((Boolean) v) ? 1 : 0;
        try { return (int) Double.parseDouble(String.valueOf(v).trim().replace(",", "")); } catch (NumberFormatException e) { return 0; }
    }

    private static double asDouble(Object v) {
        if (v == null) return 0d;
        if (v instanceof BigDecimal) return ((BigDecimal) v).doubleValue();
        if (v instanceof Number) return ((Number) v).doubleValue();
        try { return Double.parseDouble(String.valueOf(v).trim().replace(",", "")); } catch (NumberFormatException e) { return 0d; }
    }

    private static boolean asBool(Object v) {
        if (v == null) return false;
        if (v instanceof Boolean) return (Boolean) v;
        String s = String.valueOf(v).trim().toLowerCase();
        return s.equals("1") || s.equals("true") || s.equals("yes") || s.equals("on");
    }

    /** Timestamp / Date / ISO text -> "yyyy-MM-dd" (with the time when present), "" when null or unparsable. */
    private static String iso(Object v) {
        if (v == null) return "";
        if (v instanceof Timestamp) { LocalDateTime t = ((Timestamp) v).toLocalDateTime(); return t.toLocalDate().toString() + (t.toLocalTime().toSecondOfDay() == 0 ? "" : "T" + t.toLocalTime()); }
        if (v instanceof java.sql.Date) return ((java.sql.Date) v).toLocalDate().toString();
        if (v instanceof java.util.Date) return new Timestamp(((java.util.Date) v).getTime()).toLocalDateTime().toLocalDate().toString();
        if (v instanceof LocalDateTime) return ((LocalDateTime) v).toLocalDate().toString();
        if (v instanceof LocalDate) return v.toString();
        String s = String.valueOf(v).trim();
        return s.length() >= 10 ? s.substring(0, 10) : s;
    }

    private static java.sql.Date sqlDate(Object v) {
        if (v == null) return null;
        String s = String.valueOf(v).trim();
        if (s.isEmpty()) return null;
        try { return java.sql.Date.valueOf(LocalDate.parse(s.length() > 10 ? s.substring(0, 10) : s)); }
        catch (DateTimeParseException e) { return null; }
    }
}
