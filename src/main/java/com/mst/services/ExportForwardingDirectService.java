package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.ExportForwardingDirectRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.ExportGdBreakUpRepository.ci;

/**
 * The BLL side of two desktop forms that write ExImForwarding:
 *
 *   195 EximForwardingDirect             "Export Goods Forwarding Direct"  DocumentTypeId 206 (direct=true)
 *   196 EximForwardingWithoutWeighBridge "Export Goods Forwarding"         DocumentTypeId 205 (direct=false)
 *
 * Rights: CommonServices.SetRightsValueInRightsObject(ScreenName) on the desktop -> DesktopReportRights by
 * ScreenDefinition.Id here: View to load, Save / Update / Print as the buttons are enabled.
 *
 * Desktop quirks reproduced:
 *  - Insert(): "Please Check Detail Grid" (empty grid) and FormValidation come BEFORE the Save / Update confirm; the
 *    row checks ("... Required In Grid") come AFTER it. The page calls /validate, confirms, then /save.
 *  - The grid values are read from the cells' formatted Text: GrossWeight, StockWeight, NoOfBags, EbUnit, EbTotal,
 *    AddLess go through "#,##0.##" (2 decimals) and NetWeight through "#,##0.###" (3 decimals) before they are saved.
 *  - No packing detail row ever gets an ActionTypeId (0): Sp_ExImForwardingPackingDetail_Insert raises
 *    "ActionTypeId Not Found" and the whole save rolls back, exactly as the desktop does today.
 *  - 196: GpNo is always "0"; ExportSoNoId and LCOrderId both take the Contract No value; LotCompletedDate is the
 *    Completed Date picker, or - with "Date Active" ticked - the picker value captured when the box was ticked.
 *  - 196 ContractORDERNo binds the invoice's DESTINATION port into the Loading Port combo and its LOADING port into the
 *    Destination Port combo.
 *  - 196 history ('ExImForwardingFormHistory') sends no DocumentTypeId: every forwarding type of the year is listed.
 *  - 195: the Other Items list is never filled on this form; ExImLcOrderId of every row is LCOrderId = 0.
 *  - 195 Reference grid "Load": ReadById, then Save mode, a new Doc No, GP Date today, GP No / driver / bilty / vehicle
 *    cleared and every grid row's weights, bags, description, seal and container set to "0" (RefereshForReferenceGridLoad).
 *
 * Not ported: Attachment (DMS file copy popup), the "Sea Port" / "Carrier Type" buttons (they open the Sea Ports and
 * Inventory Lookup definition forms - the page links to nothing), the 195 Register "Print" drop-down that lists .rpt
 * files from a desktop folder (CommonServices.DynamicReportsLoad("ForwardingDirectRegister")).
 */
@Service
public class ExportForwardingDirectService {

    public static final int SCREEN_DIRECT = 195;
    public static final int SCREEN_WITHOUT_WB = 196;

    @Autowired private ExportForwardingDirectRepository repo;
    @Autowired private CurrentUserContext currentUserContext;
    @Autowired private DesktopReportRights rights;

    private static int screen(boolean direct) { return direct ? SCREEN_DIRECT : SCREEN_WITHOUT_WB; }
    private static int docType(boolean direct) { return direct ? 206 : 205; }

    private UserAccount user(boolean direct, String action) {
        UserAccount u = currentUserContext.requireAccountingUser();
        rights.require(u, screen(direct), action);
        return u;
    }

    private boolean allowed(UserAccount u, boolean direct, String action) {
        try { rights.require(u, screen(direct), action); return true; }
        catch (AccessDeniedException ex) { return false; }
    }

    private int year() {
        try { return currentUserContext.currentFinancialYearId(); } catch (Exception e) { return 0; }
    }

    // ================================================================= load

    /** ExpfrmForwarding_Load. */
    public Map<String, Object> setup(boolean direct) {
        UserAccount u = user(direct, "View");
        Map<String, Object> out = new LinkedHashMap<>();
        Map<String, Object> perm = new LinkedHashMap<>();
        perm.put("Save", allowed(u, direct, "Save"));
        perm.put("Update", allowed(u, direct, "Update"));
        perm.put("Print", allowed(u, direct, "Print"));
        out.put("permissions", perm);
        out.putAll(combos(u, direct, true));
        if (direct) {
            Object start = repo.financialYearStart(u, year());
            out.put("registerFrom", ExportEformRegistrationService.iso(start));
        }
        return out;
    }

    /** btnRefresh_Click / BtnNew_Click re-reads. */
    public Map<String, Object> refresh(boolean direct) { return combos(user(direct, "View"), direct, true); }

    private Map<String, Object> combos(UserAccount u, boolean direct, boolean withDocNo) {
        Map<String, Object> out = new LinkedHashMap<>();
        if (withDocNo) put(out, "docNo", () -> docNo(u, direct));
        put(out, "packTypes", () -> idName(repo.packTypes(u), "Id", "Description"));
        put(out, "jobLots", () -> idName(repo.jobLots(u), "Id", "JobLotDescription"));
        put(out, "projects", () -> idName(repo.projects(u), "Id", "ProjectName"));
        put(out, "branches", () -> idName(repo.branches(u), "Id", "BranchName"));
        put(out, "warehouses", () -> idName(repo.warehouses(u), "Id", "WareHouseName"));
        put(out, "cropYears", () -> idName(repo.cropYears(u), "Id", "CropYear"));
        put(out, "ports", () -> idName(repo.seaPorts(u), "Id", "PortName"));
        put(out, "shippingParties", () -> idName(repo.shippingParties(u), "Id", "CompanyName"));
        if (direct) {
            put(out, "carrierTypes", () -> idName(repo.carrierTypes(u), "Id", "LookupName"));
            put(out, "customers", () -> idName(repo.customersForExport(u), "Id", "CompanyName"));
            put(out, "items", () -> idName(repo.itemsForCombo(u), "Id", "ItemName"));
        } else {
            put(out, "invoices", () -> idName(repo.invoicesForForwarding(u), "Id", "InvoiceNo"));
        }
        return out;
    }

    /** generateCode: shown only when > 0. */
    private int docNo(UserAccount u, boolean direct) {
        List<Map<String, Object>> r = repo.generateCode(u, year(), docType(direct));
        if (r.isEmpty()) throw new IllegalStateException("There is no row at position 0.");
        return asInt(ci(r.get(0), "DocNo"));
    }

    /** cmbItem_Leave: UOMSchedule.SearchByObjectList -> (Id, Equivalent). */
    public List<Map<String, Object>> uoms(boolean direct, int itemId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.uomByItem(user(direct, "View"), itemId)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("name", num(ci(r, "Equivalent")));
            out.add(m);
        }
        return out;
    }

    // ================================================================= 196 invoice leave

    /**
     * cmbInvoiceNo_Leave: shippinglineAgentTransporterBind, ContractORDERNo, getIvnoicesDetailByInvoicId - each in its own
     * try/catch on the desktop, so each part carries its own error here.
     */
    public Map<String, Object> invoiceLeave(int invoiceId) {
        UserAccount u = user(false, "View");
        Map<String, Object> out = new LinkedHashMap<>();
        try {
            List<Map<String, Object>> s = repo.shippingBookingByInvoice(u, invoiceId);
            if (!s.isEmpty()) {
                Map<String, Object> m = new LinkedHashMap<>();
                m.put("ShippingAgentId", asInt(ci(s.get(0), "ShippingAgentId")));
                m.put("ShippingLineId", asInt(ci(s.get(0), "ShippingLineId")));
                m.put("TransporterId", asInt(ci(s.get(0), "TransporterId")));
                out.put("booking", m);
            } else out.put("booking", null);
        } catch (Exception e) { out.put("bookingError", msg(e)); }
        try {
            if (invoiceId > 0) {
                List<Map<String, Object>> data = repo.invoiceData(u, year());
                if (!data.isEmpty()) {
                    List<Map<String, Object>> lc = new ArrayList<>(), cust = new ArrayList<>(), dest = new ArrayList<>(), load = new ArrayList<>();
                    for (Map<String, Object> r : data) {
                        if (asInt(ci(r, "Id")) != invoiceId) continue;
                        lc.add(pair(ci(r, "LcOrderNoId"), ci(r, "LcOrderNo")));
                        cust.add(pair(ci(r, "SupplierCustomerId"), ci(r, "Customer")));
                        dest.add(pair(ci(r, "DestinationPortId"), ci(r, "DestinationPort")));
                        load.add(pair(ci(r, "LoadingPortId"), ci(r, "LoadingPort")));
                    }
                    Map<String, Object> c = new LinkedHashMap<>();
                    c.put("contracts", lc);
                    c.put("customers", cust);
                    c.put("loadingPortCombo", dest);       /* DestinationPort rows into cmbLoadingPort (desktop) */
                    c.put("destinationPortCombo", load);   /* LoadingPort rows into cmbDestinationPort (desktop) */
                    out.put("contract", c);
                }
            }
        } catch (Exception e) { out.put("contractError", msg(e)); }
        try {
            List<Map<String, Object>> rows = new ArrayList<>();
            for (Map<String, Object> r : repo.invoiceDetail(u, invoiceId)) {
                Map<String, Object> m = new LinkedHashMap<>();
                m.put("ItemId", asInt(ci(r, "ItemId")));
                m.put("ItemName", text(ci(r, "ItemName")));
                m.put("PackTypeId", asInt(ci(r, "PackingMaterialTypeId")));
                m.put("Packtype", text(ci(r, "PackMaterilaType")));
                m.put("ItemDescription", text(ci(r, "ItemDescriptionManual")));
                m.put("NoOfBags", asDouble(ci(r, "OuterQty")));
                m.put("OuterUOMId", asInt(ci(r, "OuterQtyUomId")));
                m.put("OuterUOM", text(ci(r, "OuterUOM")));
                m.put("NetWeight", asDouble(ci(r, "NetWeight")));
                m.put("EbUnit", 0d);
                m.put("EbTotal", 0d);
                m.put("AddLess", 0d);
                m.put("GrossWeight", asDouble(ci(r, "NetWeight")));
                m.put("StockWeight", asDouble(ci(r, "NetWeight")));
                m.put("WarehouseId", 0);
                m.put("Warehouse", "");
                m.put("Container#", "");
                m.put("Seal#", "");
                m.put("JobLotId", asInt(ci(r, "JobLotId")));
                m.put("JobLot", text(ci(r, "JobLotDescription")));
                m.put("CropYearId", asInt(ci(r, "CropYearId")));
                m.put("CropYear", text(ci(r, "CropYear")));
                rows.add(m);
            }
            out.put("rows", rows);
            if (!rows.isEmpty()) {
                out.put("otherItemCombo", idName(repo.otherItemCombo(u, invoiceId), "ItemId", "ItemName"));
                List<Map<String, Object>> h = repo.invoiceHeader(invoiceId);
                if (h.isEmpty()) throw new IllegalStateException("Index was out of range. Must be non-negative and less than the size of the collection.\r\nParameter name: index");
                List<Map<String, Object>> others = new ArrayList<>();
                for (Map<String, Object> r : repo.invoiceOtherItems(invoiceId)) {
                    Map<String, Object> m = new LinkedHashMap<>();
                    m.put("ItemId", asInt(ci(r, "otherItemId")));
                    m.put("ItemName", text(ci(r, "ItemName")));
                    m.put("Qty", asDouble(ci(r, "oItemQty")));
                    m.put("Rate", asDouble(ci(r, "oItemRate")));
                    m.put("Amount", asDouble(ci(r, "oItemAmount")));
                    m.put("ContainerNo", null);   /* the page puts txtOContainer.Text here, as the desktop does */
                    m.put("Remarks", text(ci(r, "OtherItemRemarks")));
                    others.add(m);
                }
                out.put("otherItems", others);
            }
        } catch (Exception e) { out.put("detailError", msg(e)); }
        return out;
    }

    // ================================================================= history / reference / register

    public List<Map<String, Object>> history(boolean direct, int noOfRecords) {
        return dateRows(repo.history(user(direct, "View"), direct, noOfRecords, year()));
    }

    public List<Map<String, Object>> referenceHistory(String referenceNo) {
        return dateRows(repo.referenceHistory(user(true, "View"), referenceNo == null ? "" : referenceNo, year()));
    }

    /** btnShow_Click: ExportPdfReport.FarwardingByReferenceNoRegister - every filter only when set. */
    public List<Map<String, Object>> register(Map<String, Object> f) {
        UserAccount u = user(true, "View");
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", u.getOrganizationId());
        p.put("CompanyId", u.getCompanyId());
        String ref = text(f.get("referenceNo"));
        if (toIntText(ref) != 0) p.put("ReferencNo", ref);
        LocalDate from = date(f.get("fromDate")), to = date(f.get("toDate"));
        if (from != null) p.put("FromDate", java.sql.Date.valueOf(from));
        if (to != null) p.put("ToDate", java.sql.Date.valueOf(to));
        p.put("DocumentTypeId", 206);
        guarded(p, "SupplierCustomerId", f.get("customerId"));
        guarded(p, "ItemId", f.get("itemId"));
        guarded(p, "CarrierTypId", f.get("carrierTypeId"));
        guarded(p, "ShippingLineId", f.get("shippingLineId"));
        guarded(p, "ShippingAgentId", f.get("shippingAgentId"));
        guarded(p, "TransporterId", f.get("transporterId"));
        guarded(p, "DestinationPortId", f.get("destinationPortId"));
        guarded(p, "loadingPortId", f.get("loadingPortId"));
        return dateRows(repo.register(p));
    }

    private static void guarded(Map<String, Object> p, String name, Object v) { int i = asInt(v); if (i != 0) p.put(name, i); }

    // ================================================================= read

    /** 196 ReadById: GetByID returns null when nothing matches -> FormReset. 195 GetByIdDirectFarwardng()[0] throws. */
    public Map<String, Object> readById(boolean direct, int id) {
        UserAccount u = user(direct, "View");
        List<Map<String, Object>> h = repo.header(id, direct);
        Map<String, Object> out = new LinkedHashMap<>();
        if (h.isEmpty()) {
            if (direct) throw new IllegalStateException("Index was out of range. Must be non-negative and less than the size of the collection.\r\nParameter name: index");
            out.put("notFound", true);
            return out;
        }
        Map<String, Object> r = h.get(0);
        Map<String, Object> hd = new LinkedHashMap<>();
        for (String k : new String[] { "Id", "DocNo", "ExImInvoiceId", "LCOrderId", "SupplierCustomerId", "DestinationPortId", "LoadingPortId",
                "ProjectsId", "BranchesId", "ShippingLineId", "ShippingAgentId", "TransporterId", "CarrierTypeId", "NoOfContainer" }) hd.put(k, asInt(ci(r, k)));
        for (String k : new String[] { "InvoiceNo", "LcOrderNo", "Customer", "DestinationPort", "LoadingPort", "DriverCellNo", "DriverName",
                "DriverCnicNo", "VehicleNo", "BiltyNo", "GpNo", "ReferenceNo" }) hd.put(k, ci(r, k) == null ? "" : String.valueOf(ci(r, k)));
        hd.put("DocDate", ExportEformRegistrationService.iso(ci(r, "DocDate")));
        hd.put("GPDate", ExportEformRegistrationService.iso(ci(r, "GPDate")));
        hd.put("LotCompletedDate", ExportEformRegistrationService.iso(ci(r, "LotCompletedDate")));
        hd.put("FreightAmt", num(ci(r, "FreightAmt")));
        out.put("header", hd);
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> d : repo.detail(id)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("ItemId", asInt(ci(d, "ItemId")));
            m.put("ItemName", text(ci(d, "ItemName")));
            m.put("PackTypeId", asInt(ci(d, "PackingMaterialId")));
            m.put("Packtype", text(ci(d, "Packtype")));
            m.put("ItemDescription", text(ci(d, "ItemDescription")));
            m.put("NoOfBags", asDouble(ci(d, "OuterQty")));
            m.put("OuterUOMId", asInt(ci(d, "UOMScheduleIdOuter")));
            m.put("OuterUOM", text(ci(d, "OuterUOM")));
            m.put("NetWeight", asDouble(ci(d, "NetWeight")));
            m.put("EbUnit", asDouble(ci(d, "EbUnit")));
            m.put("EbTotal", asDouble(ci(d, "EbTotal")));
            m.put("AddLess", asDouble(ci(d, "AdLsWeight")));
            m.put("GrossWeight", asDouble(ci(d, "GrossWeight")));
            m.put("StockWeight", asDouble(ci(d, "StockWeight")));
            m.put("WarehouseId", asInt(ci(d, "WarehouseId")));
            m.put("Warehouse", text(ci(d, "WareHouseName")));
            m.put("Container#", text(ci(d, "ContainerNo")));
            m.put("Seal#", text(ci(d, "SealNo")));
            m.put("JobLotId", asInt(ci(d, "LotJobId")));
            m.put("JobLot", text(ci(d, "JobLotName")));
            m.put("CropYearId", asInt(ci(d, "CropYearId")));
            m.put("CropYear", text(ci(d, "CropYear")));
            rows.add(m);
        }
        out.put("rows", rows);
        List<Map<String, Object>> others = new ArrayList<>();
        for (Map<String, Object> o : repo.otherItems(id)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("ItemId", asInt(ci(o, "otherItemId")));
            m.put("ItemName", text(ci(o, "ItemName")));
            m.put("Qty", asDouble(ci(o, "oItemQty")));
            m.put("Rate", asDouble(ci(o, "oItemRate")));
            m.put("Amount", asDouble(ci(o, "oItemAmount")));
            m.put("ContainerNo", text(ci(o, "ContainerNo")));
            m.put("Remarks", text(ci(o, "OtherItemRemarks")));
            others.add(m);
        }
        out.put("otherItems", others);
        if (!direct) {
            int invoiceId = asInt(ci(r, "ExImInvoiceId"));
            try { out.put("otherItemCombo", idName(repo.otherItemCombo(u, invoiceId), "ItemId", "ItemName")); }
            catch (Exception e) { out.put("otherItemComboError", msg(e)); }
        }
        return out;
    }

    // ================================================================= validate / save

    /** Insert() up to the confirm: "Please Check Detail Grid", then formValidation. */
    public Map<String, Object> validate(boolean direct, Map<String, Object> b) {
        UserAccount u = user(direct, asInt(b.get("recId")) > 0 ? "Update" : "Save");
        preConfirm(direct, b);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("confirm", asInt(b.get("recId")) > 0 ? "Are you sure to Update?" : "Are you sure to Save?");
        return out;
    }

    private void preConfirm(boolean direct, Map<String, Object> b) {
        Map<String, Object> h = map(b.get("header"));
        if (list(b.get("rows")).isEmpty()) throw new IllegalArgumentException("Please Check Detail Grid");
        String docNo = str(h.get("txtdocno"));
        if (docNo.isEmpty() || "0".equals(docNo)) throw new IllegalArgumentException("Please Check Doc No");
        if (direct) {
            String ref = str(h.get("txtReferenceNo"));
            if (ref.trim().isEmpty() || "0".equals(ref)) throw new IllegalArgumentException("Please Insert  Reference No");
            requireCombo(h, "cmbCustomer", "Please Select Customer");
            if (str(h.get("txtGpNo")).trim().isEmpty() || toIntText(h.get("txtGpNo")) == 0) throw new IllegalArgumentException("Please Enter GP No");
            requireCombo(h, "cmbLoadingPort", "Please Select Loading Port");
            requireCombo(h, "cmbDestinationPort", "Please Select Destination Port");
            if (str(h.get("txtNoOfContainer")).trim().isEmpty() || toIntText(h.get("txtNoOfContainer")) == 0) throw new IllegalArgumentException("Please Check No Of Container");
            if (str(h.get("txtVehicleNo")).trim().isEmpty() || "0".equals(str(h.get("txtVehicleNo")))) throw new IllegalArgumentException("Please Enter Vehicle No");
            if (str(h.get("txtBilityNo")).trim().isEmpty() || "0".equals(str(h.get("txtBilityNo")))) throw new IllegalArgumentException("Please Enter Bilty No");
        } else {
            requireCombo(h, "cmbInvoiceNo", "Please Select Invoice No");
            requireCombo(h, "cmbContractNo", "Please Select Contract No");
            requireCombo(h, "cmbCustomer", "Please Select Customer");
            requireCombo(h, "cmbShippingLIne", "Please Select Shippping Line");
            requireCombo(h, "cmbShippingAgent", "Please Select Shippping Agent");
            requireCombo(h, "cmbtransporter", "Please Select Transporter");
            requireCombo(h, "cmbLoadingPort", "Please Select Loading Port");
            requireCombo(h, "cmbDestinationPort", "Please Select Destination Port");
            if (str(h.get("txtNoOfContainer")).trim().isEmpty() || toIntText(h.get("txtNoOfContainer")) == 0) throw new IllegalArgumentException("Please Check No Of Container");
        }
    }

    /** Insert() after the confirm: the row checks, the model, ExImForwarding.Save. */
    public Map<String, Object> save(boolean direct, Map<String, Object> b) {
        int recId = asInt(b.get("recId"));
        UserAccount u = user(direct, recId > 0 ? "Update" : "Save");
        preConfirm(direct, b);
        Map<String, Object> h = map(b.get("header"));
        List<Map<String, Object>> rows = list(b.get("rows"));
        int lcOrderId = direct ? 0 : asInt(h.get("cmbContractNo"));

        List<Map<String, Object>> details = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            if (asInt(r.get("ItemId")) == 0) throw new IllegalArgumentException("Item Required In Grid");
            if (asInt(r.get("PackTypeId")) == 0) throw new IllegalArgumentException("Packing Type Required In Grid");
            if (asDouble(r.get("NetWeight")) == 0) throw new IllegalArgumentException("NetWeight Required In Grid");
            if (asDouble(r.get("GrossWeight")) == 0) throw new IllegalArgumentException("Gross Weight Required In Grid");
            if (asInt(r.get("OuterUOMId")) == 0) throw new IllegalArgumentException("Pack UOM Required In Grid");
            if (asDouble(r.get("NoOfBags")) == 0) throw new IllegalArgumentException("No Of Bags Required In Grid");
            if (asInt(r.get("CropYearId")) == 0) throw new IllegalArgumentException("CropYear Required In Grid");
            if (asInt(r.get("WarehouseId")) == 0) throw new IllegalArgumentException("Warehouse Required In Grid");
            if (str(r.get("Container#")).isEmpty()) throw new IllegalArgumentException("Container Required In Grid");
            if (str(r.get("Seal#")).isEmpty()) throw new IllegalArgumentException("Seal No Required In Grid");
            if (asInt(r.get("JobLotId")) == 0) throw new IllegalArgumentException("Job Lot Required In Grid");
            Map<String, Object> d = new LinkedHashMap<>();
            d.put("AdLsWeight", fmt(r.get("AddLess"), 2));
            d.put("GrossWeight", fmt(r.get("GrossWeight"), 2));
            d.put("InnerQty", fmt(r.get("NoOfBags"), 2));
            d.put("NetWeight", fmt(r.get("NetWeight"), 3));
            d.put("StockWeight", fmt(r.get("StockWeight"), 2));
            d.put("OuterQty", fmt(r.get("NoOfBags"), 2));
            d.put("EbUnit", fmt(r.get("EbUnit"), 2));
            d.put("EbTotal", fmt(r.get("EbTotal"), 2));
            d.put("InnerEbUnit", 0d);
            d.put("InnerEbTotal", 0d);
            d.put("TotalPackingWeight", 0d);
            d.put("ExImForwardingId", 0);
            d.put("ExImLcOrderId", lcOrderId);
            d.put("DoDetailId", 0);
            d.put("Id", 0);
            d.put("ItemId", asInt(r.get("ItemId")));
            d.put("LotJobId", asInt(r.get("JobLotId")));
            d.put("CropYearId", asInt(r.get("CropYearId")));
            d.put("PackingMaterialId", asInt(r.get("PackTypeId")));
            d.put("UOMScheduleIdInner", asInt(r.get("OuterUOMId")));
            d.put("UOMScheduleIdOuter", asInt(r.get("OuterUOMId")));
            d.put("WarehouseId", asInt(r.get("WarehouseId")));
            d.put("RefDocumentTypeId", 0);
            d.put("RefDocIdNo", 0);
            d.put("RefDocSubIdNo", 0);
            d.put("WarehouseToId", 0);
            d.put("ContractDetailId", 0);
            d.put("InvoiceId", 0);
            d.put("InvoiceDetailId", 0);
            d.put("LineId", 0);
            d.put("ActionTypeId", 0);
            d.put("ContainerId", 0);
            d.put("ContainerNo", str(r.get("Container#")).trim());
            d.put("ItemDescription", str(r.get("ItemDescription")).trim());
            d.put("LabReportNo", null);
            d.put("OtherInfo", null);
            d.put("OuterPackDescription", null);
            d.put("SealNo", str(r.get("Seal#")).trim());
            d.put("ProductionNo", null);
            d.put("PackingExpiryDate", null);
            d.put("_CropYear", null);   /* the model's virtual CropYear is never set by Insert() */
            details.add(d);
        }
        List<Map<String, Object>> others = new ArrayList<>();
        if (!direct) {
            for (Map<String, Object> o : list(b.get("otherItems"))) {
                Map<String, Object> m = new LinkedHashMap<>();
                m.put("oItemAmount", BigDecimal.valueOf(asDouble(o.get("Amount"))));
                m.put("oItemQty", BigDecimal.valueOf(asDouble(o.get("Qty"))));
                m.put("oItemWeightKgs", BigDecimal.ZERO);
                m.put("oItemRate", asDouble(o.get("Rate")));
                m.put("ExImForwardingId", 0);
                m.put("Id", 0);
                m.put("otherItemId", asInt(o.get("ItemId")));
                m.put("InvoiceOtherItemsDetailId", 0);
                m.put("ExImInvoiceId", 0);
                m.put("WareHouseFromId", 0);
                m.put("WarehouseToId", 0);
                m.put("ContainerNo", str(o.get("ContainerNo")));
                m.put("OtherItemRemarks", str(o.get("Remarks")));
                others.add(m);
            }
        }

        Timestamp now = new Timestamp(System.currentTimeMillis());
        boolean update = recId > 0;
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("IsApproved", false);
        m.put("ApprovedDate", null);
        m.put("DocDate", ts(h.get("DocDate")));
        m.put("EntryDate", now);
        m.put("GPDate", ts(h.get("txtGPDate")));
        Object lot = null;
        if (!direct) lot = asBool(h.get("chkdateisactive")) ? ts(h.get("lotcompletedate")) : ts(h.get("txtCompletedDate"));
        m.put("LotCompletedDate", lot);
        m.put("ModifyDate", now);
        m.put("FreightAmt", asDouble(h.get("txtFreight")));
        m.put("OtherCharges", 0d);
        m.put("FactoryWeight", 0d);
        m.put("ApprovedUser", 0);
        m.put("BranchesId", asInt(h.get("combranches")));
        m.put("CompanyId", u.getCompanyId());
        m.put("DestinationPortId", asInt(h.get("cmbDestinationPort")));
        m.put("DocNo", toIntText(str(h.get("txtdocno")).trim()));
        m.put("DocumentTypeId", docType(direct));
        m.put("EntryUser", update ? 0 : u.getId());
        m.put("ExImInvoiceId", direct ? 0 : asInt(h.get("cmbInvoiceNo")));
        m.put("ExportSoNoId", direct ? 0 : lcOrderId);
        m.put("GatePassOutwardId", 0);
        m.put("Id", update ? recId : 0);
        m.put("LCOrderId", lcOrderId);
        m.put("LoadingPortId", asInt(h.get("cmbLoadingPort")));
        m.put("ModifyUser", update ? u.getId() : 0);
        m.put("NoOfContainer", toIntText(h.get("txtNoOfContainer")));
        m.put("OrganizationId", u.getOrganizationId());
        m.put("ProjectsId", asInt(h.get("comproject")));
        m.put("ShippedContainer", 0);
        m.put("ShippingAgentId", asInt(h.get("cmbShippingAgent")));
        m.put("AutoUpdateId", 0);
        m.put("ShippingLineId", asInt(h.get("cmbShippingLIne")));
        m.put("SupplierCustomerId", asInt(h.get("cmbCustomer")));
        m.put("TransporterId", asInt(h.get("cmbtransporter")));
        m.put("CarrierTypeId", direct ? asInt(h.get("cmbcareiertype")) : 0);
        m.put("FinancialYearId", year());
        m.put("ActionId", update ? 2 : 1);
        m.put("BiltyNo", str(h.get("txtBilityNo")));
        m.put("Container", "");
        m.put("Container1", "");
        m.put("DocAttachment", null);
        m.put("DriverCellNo", str(h.get("txtDriverCellNo")));
        m.put("DriverCnicNo", str(h.get("txtCNICNO")));
        m.put("DriverName", str(h.get("txtDriverName")));
        m.put("GpNo", direct ? str(h.get("txtGpNo")) : "0");
        m.put("LotStatus", null);
        m.put("OtherRemarks", null);
        m.put("Status", null);
        m.put("VehicleNo", str(h.get("txtVehicleNo")));
        m.put("ReferenceNo", direct ? str(h.get("txtReferenceNo")) : null);

        int success = repo.save(u, m, details, others);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("id", success);
        out.put("message", update ? "Record Update SuccessFully" : "Record Save SuccessFully");
        return out;
    }

    // ================================================================= helpers

    private static void requireCombo(Map<String, Object> h, String id, String message) {
        if (str(h.get(id + "Text")).trim().isEmpty() || asInt(h.get(id)) == 0) throw new IllegalArgumentException(message);
    }

    /** The cell's FormatString text read back through Conversion.ToDouble (custom format rounds half away from zero). */
    private static double fmt(Object v, int decimals) {
        return BigDecimal.valueOf(asDouble(v)).setScale(decimals, RoundingMode.HALF_UP).doubleValue();
    }

    private interface Loader { Object load() throws Exception; }

    private static void put(Map<String, Object> out, String key, Loader l) {
        try { out.put(key, l.load()); }
        catch (Exception e) { out.put(key, new ArrayList<>()); out.put(key + "Error", msg(e)); }
    }

    private static Map<String, Object> pair(Object id, Object name) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("Id", asInt(id));
        m.put("name", name == null ? "" : String.valueOf(name));
        return m;
    }

    private static List<Map<String, Object>> idName(List<Map<String, Object>> rows, String id, String name) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) out.add(pair(ci(r, id), ci(r, name)));
        return out;
    }

    private static List<Map<String, Object>> dateRows(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            for (Map.Entry<String, Object> e : r.entrySet()) {
                Object v = e.getValue();
                if (v instanceof Timestamp) v = ((Timestamp) v).toLocalDateTime().toString().replace('T', ' ');
                else if (v instanceof java.util.Date) v = v.toString();
                else if (v instanceof BigDecimal) v = ((BigDecimal) v).doubleValue();
                m.put(e.getKey(), v);
            }
            out.add(m);
        }
        return out;
    }

    private static String num(Object v) {
        if (v == null) return "";
        double d = asDouble(v);
        if (d == Math.rint(d) && Math.abs(d) < 1e15) return String.valueOf((long) d);
        return String.valueOf(d);
    }

    private static int toIntText(Object v) {
        if (v == null) return 0;
        try { return Integer.parseInt(String.valueOf(v).trim()); } catch (NumberFormatException e) { return 0; }
    }

    private static String msg(Exception e) {
        Throwable t = e;
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        return t.getMessage() == null ? String.valueOf(e) : t.getMessage();
    }

    @SuppressWarnings("unchecked")
    private static Map<String, Object> map(Object v) { return v instanceof Map ? (Map<String, Object>) v : new LinkedHashMap<>(); }

    @SuppressWarnings("unchecked")
    private static List<Map<String, Object>> list(Object v) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (v instanceof List) for (Object o : (List<Object>) v) if (o instanceof Map) out.add((Map<String, Object>) o);
        return out;
    }

    private static String text(Object v) { return v == null ? "" : String.valueOf(v).trim(); }

    private static String str(Object v) { return v == null ? "" : String.valueOf(v); }

    private static int asInt(Object v) { return ExportEformRegistrationService.asInt(v); }

    private static double asDouble(Object v) { return ExportEformRegistrationService.asDouble(v); }

    private static boolean asBool(Object v) {
        if (v == null) return false;
        if (v instanceof Boolean) return (Boolean) v;
        String s = String.valueOf(v).trim().toLowerCase();
        return "true".equals(s) || "1".equals(s);
    }

    private static LocalDate date(Object v) {
        String s = v == null ? "" : String.valueOf(v).trim();
        if (s.length() < 10) return null;
        try { return LocalDate.parse(s.substring(0, 10)); } catch (Exception e) { return null; }
    }

    private static Timestamp ts(Object v) {
        LocalDate d = date(v);
        if (d == null) return new Timestamp(System.currentTimeMillis());
        LocalDateTime t = d.atTime(LocalDateTime.now().toLocalTime());
        return Timestamp.valueOf(t);
    }
}
