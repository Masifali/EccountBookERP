package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.ExportLcOrderRepository;
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

import static com.mst.repositories.ExportGdBreakUpRepository.ci;

/**
 * The BLL side of 210 LcOrder "Lc Order" (Architecture.WinApp.Export), DocumentTypeId 201.
 *
 * The desktop form has no rights checks; the web page requires View / Save / Update by ScreenDefinition.Id 210.
 *
 * Desktop quirks reproduced:
 *  - The Save / Update confirm comes BEFORE FormValidation (the page asks first, then this service validates).
 *  - FormValidation first compares the detail grid's Amount total with Total Fc Amount; a grid that was never bound
 *    (nothing added, no record opened) throws "Object reference not set to an instance of an object." - the page
 *    sends whether the grid is bound. Every other number check goes through Conversion.ToInt(text), so a value with
 *    decimals reads as 0 ("... Field is Required!").
 *  - The packing detail quantities, weight, rate and amount are saved through Conversion.ToInt (rounded half to even);
 *    NoOfUnit, ItemBrandId, CropYearId and ActionTypeId are never set (0) - Sp_ExImLcOrderPackingDetail_Insert raises
 *    "ActionTypeId Not Found" for such a row and the whole save rolls back, exactly as on the desktop.
 *  - ProjectsId is validated but never saved (0); LotReference, FcurrencySymbol and LcDocAttachment are "".
 *  - FCurrencyAmount / ConversionRate go through double.Parse (thousand separators accepted, other text fails).
 *  - The message after ExImLcOrder.Save is shown regardless of the returned id.
 *
 * Deviation (documented): gridFill reads a "NoOfAttachments" column that 'ReadByOrganizationCompanyId' does not
 * return, so the desktop History shows "Column 'NoOfAttachments' does not belong to table." whenever rows exist; here
 * the rows are shown with an empty No Of Attachments column. Attachments (Attachment popup) are not ported.
 */
@Service
public class ExportLcOrderService {

    public static final int SCREEN_ID = 210;
    public static final int DOCUMENT_TYPE_ID = 201;

    @Autowired private ExportLcOrderRepository repo;
    @Autowired private CurrentUserContext currentUserContext;
    @Autowired private DesktopReportRights rights;

    private UserAccount user(String action) {
        UserAccount u = currentUserContext.requireAccountingUser();
        rights.require(u, SCREEN_ID, action);
        return u;
    }

    private boolean allowed(UserAccount u, String action) {
        try { rights.require(u, SCREEN_ID, action); return true; }
        catch (AccessDeniedException ex) { return false; }
    }

    /** LcOrder_Load: every combo, the fixed lists stay on the page, DocNo. */
    public Map<String, Object> setup() {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        Map<String, Object> perm = new LinkedHashMap<>();
        perm.put("Save", allowed(u, "Save"));
        perm.put("Update", allowed(u, "Update"));
        out.put("permissions", perm);
        put(out, "branches", () -> idName(repo.branches(u), "Id", "BranchName"));
        put(out, "projects", () -> idName(repo.projects(u), "Id", "ProjectName"));
        put(out, "customers", () -> idName(repo.customers(u), "Id", "CompanyName"));
        put(out, "currencies", () -> idName(repo.currencies(u), "Id", "CurrencyCode"));
        put(out, "ports", () -> idName(repo.seaPorts(u), "Id", "PortName"));
        put(out, "items", () -> idName(repo.exportItems(u), "Id", "ItemName"));
        put(out, "packTypes", () -> idName(repo.packTypes(u), "Id", "Description"));
        put(out, "banks", () -> banks(u));
        put(out, "deliveryTerms", () -> idName(repo.deliveryTerms(), "Id", "Code"));
        put(out, "paymentTerms", () -> idName(repo.paymentTerms(u), "Id", "LcOrderTerm"));
        put(out, "docNo", () -> docNo(u));
        return out;
    }

    /** importerbank: Foreign Country -> Importer Bank, Home Country -> Exporter Bank. */
    private Map<String, Object> banks(UserAccount u) {
        List<Map<String, Object>> foreign = new ArrayList<>(), home = new ArrayList<>();
        for (Map<String, Object> r : repo.banks(u)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("name", text(ci(r, "BranchName")));
            String h = text(ci(r, "IsHomeland"));
            if ("Foreign Country".equals(h)) foreign.add(m);
            else if ("Home Country".equals(h)) home.add(m);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("importer", foreign);
        out.put("exporter", home);
        return out;
    }

    /** DocNo(): GenerateCode(201) -> Rows[0]["DocNo"] (Conversion.ToInt). */
    private int docNo(UserAccount u) {
        List<Map<String, Object>> r = repo.generateCode(u);
        if (r.isEmpty()) throw new IllegalStateException("There is no row at position 0.");
        return asInt(ci(r.get(0), "DocNo"));
    }

    /** FormReset / formResetDetail re-reads: DocNo and ItemDetailFill. */
    public Map<String, Object> refresh() {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        put(out, "docNo", () -> docNo(u));
        put(out, "items", () -> idName(repo.exportItems(u), "Id", "ItemName"));
        return out;
    }

    /** cmbitem_Leave: UOMSchedule.SearchByObject - display member Equivalent. */
    public List<Map<String, Object>> uoms(int itemId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.uomByItem(user("View"), itemId)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("name", num(ci(r, "Equivalent")));
            out.add(m);
        }
        return out;
    }

    /** cmbcustomer_Leave -> PerformaInvoie: the proforma invoices whose SupplierCustomerId is the customer (Id, DocNo). */
    public List<Map<String, Object>> proformaInvoices(int customerId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.proformaInvoices(user("View"))) {
            if (asInt(ci(r, "SupplierCustomerId")) != customerId) continue;
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("name", text(ci(r, "DocNo")));
            out.add(m);
        }
        return out;
    }

    /** tabControl1_SelectedIndexChanged -> gridFill: the form's own DataTable columns. */
    public List<Map<String, Object>> history() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.history(user("View"))) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("Customer", text(ci(r, "CompanyName")));
            m.put("PerformaInvoice", text(ci(r, "ExImProformaInvoice")));
            m.put("Quotation Ref", text(ci(r, "QuotReference")));
            m.put("InquiryRef", text(ci(r, "InquiryReference")));
            m.put("CustomerCode", text(ci(r, "CustomerCode")));
            m.put("ExpiryDate", dateTimeText(ci(r, "ExpiryDate")));
            m.put("LastShipmentDate", dateTimeText(ci(r, "LastShipmentDate")));
            m.put("PartialShipment", text(ci(r, "PartialShipment")));
            m.put("LoadingPort", text(ci(r, "LoadingPort")));
            m.put("DestinationPort", text(ci(r, "DestinationPort")));
            m.put("ImporterBank", text(ci(r, "ImporterBank")));
            m.put("ExporterBank", text(ci(r, "ExporterBank")));
            m.put("ExchangeRate", num(ci(r, "ConversionRate")));
            m.put("GrossWeight", asDouble(ci(r, "GrossWeightKgs")));
            m.put("NoOfContainer", num(ci(r, "NoOfContainers")));
            m.put("NoOfAttachments", ci(r, "NoOfAttachments") == null ? "" : text(ci(r, "NoOfAttachments")));
            out.add(m);
        }
        return out;
    }

    /** ReadById(Id): GetByID(Id)[0] + the detail rows in dtdetail column order. */
    public Map<String, Object> readById(int id) {
        user("View");
        List<Map<String, Object>> h = repo.header(id);
        if (h.isEmpty()) throw new IllegalStateException("Index was out of range. Must be non-negative and less than the size of the collection.\r\nParameter name: index");
        Map<String, Object> r = h.get(0);
        Map<String, Object> hd = new LinkedHashMap<>();
        for (String k : new String[] { "BranchesId", "ExImProformaInvoice", "SupCustId", "DeliveryTermId", "PaymentTermId", "ImporterBankId",
                "ExporterBankId", "LoadingPortId", "DestinationPortId", "FcurrencyId", "DocumentTypeId" }) hd.put(k, asInt(ci(r, k)));
        for (String k : new String[] { "LcOrderNo", "ExpiryPlace", "PartialShipment", "TransShipment", "QuotReference", "InquiryReference",
                "SalesContractRef", "CustomerCode", "InsepctionRequired", "InsepctionDescription", "CommodityDetial", "ShippingMarks",
                "RemarksHeader", "LegalizationRequired", "LegalizationDescription", "Status" }) hd.put(k, ci(r, k) == null ? "" : String.valueOf(ci(r, k)));
        for (String k : new String[] { "LcOrderDate", "ExpiryDate", "LastShipmentDate", "SalesContratDate" }) hd.put(k, iso(ci(r, k)));
        for (String k : new String[] { "FCurrencyAmount", "ConversionRate", "GrossWeightKgs", "NetWeightKgs", "NoOfContainers" }) hd.put(k, num(ci(r, k)));
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("header", hd);
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> d : repo.detail(id, asInt(ci(r, "DocumentTypeId")))) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("ItemId", asInt(ci(d, "ExImItemId")));
            m.put("Item", text(ci(d, "ItemName")));
            m.put("PackTypeId", asInt(ci(d, "InvPackingMaterialTypeId")));
            m.put("PackType", text(ci(d, "ExImPackMaterialTypeName")));
            m.put("InnerQty", asDouble(ci(d, "InnerQty")));
            m.put("InnerPackUOMId", asInt(ci(d, "UOMScheduleIdInner")));
            m.put("InnerPackUOM", num(ci(d, "UOMScheduleIdInnerName")));
            m.put("OuterQty", asDouble(ci(d, "OuterQty")));
            m.put("OuterPackUOMId", asInt(ci(d, "UOMScheduleIdOuter")));
            m.put("OuterPackUOM", num(ci(d, "UOMScheduleIdOuterName")));
            m.put("Weigth", asDouble(ci(d, "NetWeight")));
            m.put("Rate", asDouble(ci(d, "RatePrice")));
            m.put("RateUOMId", asInt(ci(d, "UOMScheduleIdRate")));
            m.put("RateUOM", ci(d, "UOMScheduleIdRateName") == null ? "" : num(ci(d, "UOMScheduleIdRateName")));
            m.put("Amount", asDouble(ci(d, "TotalAmount")));
            m.put("Description", text(ci(d, "Other_Description")));
            rows.add(m);
        }
        out.put("rows", rows);
        return out;
    }

    /** btnSave_Click / btnUpdate_Click (which calls it): FormValidation then ExImLcOrder.Save. */
    public Map<String, Object> save(Map<String, Object> b) {
        int recId = asInt(b.get("recId"));
        boolean updateMode = asBool(b.get("updateMode"));
        UserAccount u = user(updateMode ? "Update" : "Save");
        Map<String, Object> h = map(b.get("header"));
        List<Map<String, Object>> rows = list(b.get("rows"));
        boolean gridBound = asBool(b.get("gridBound"));

        /* ---------------- FormValidation */
        if (!gridBound) throw new IllegalStateException("Object reference not set to an instance of an object.");
        double amountTotal = 0;
        for (Map<String, Object> r : rows) amountTotal += asDouble(r.get("Amount"));
        if (amountTotal != asDouble(h.get("txttotalfcamount"))) throw new IllegalArgumentException("Fcy Amount and Detail Amount Should be Same");
        if (toIntText(h.get("txtgrossweight")) < toIntText(h.get("txtnetweight"))) throw new IllegalArgumentException("Gross Weight Should Not be Less then Net Weight");
        requireCombo(h, "cmbbranch", "Branh Field is Required!");
        requireCombo(h, "cmbproject", "Project Field is Required!");
        requireCombo(h, "cmbcustomer", "Customer Field is Required!");
        requireCombo(h, "cmbdeliveryterm", "Delivery Term Field is Required!");
        requireCombo(h, "cmbpaymenterm", "Payment Term Field is Required!");
        requireCombo(h, "cmbimporterbank", "Importer Bank Field is Required!");
        requireCombo(h, "cmbExporterBanks", "Exporter Bank Field is Required!");
        requireCombo(h, "cmbloadingport", "Loading Port Field is Required!");
        requireCombo(h, "cmbdestinationport", "Destination Port Field is Required!");
        requireCombo(h, "cmbstatus", "Status Field is Required!");
        requireNum(h, "txtdocno", "Doc No Field is Required!");
        requireNum(h, "txttotalfcamount", "Fcy Amount Field is Required!");
        requireNum(h, "txtexchangerate", "Exchange Rate Field is Required!");
        requireNum(h, "txtlocalcurramount", "Local Amount Field is Required!");
        requireNum(h, "txtgrossweight", "Gross Weight Field is Required!");
        requireNum(h, "txtnetweight", "Net Weight Field is Required!");

        Timestamp now = new Timestamp(System.currentTimeMillis());
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("IsApproved", false);
        m.put("PostState", false);
        m.put("EntryDate", now);
        m.put("ExpiryDate", ts(h.get("txtexpirydate")));
        m.put("LastShipmentDate", ts(h.get("txtlastshipmentdate")));
        m.put("LcOrderDate", ts(h.get("txtdocdate")));
        m.put("ModifyDate", now);
        m.put("PostDate", now);
        m.put("SalesContratDate", ts(h.get("txtreferencedate")));
        m.put("ShipmentStartDate", null);
        m.put("CustomerApprovalDate", null);
        m.put("FactoryLoadingDate", null);
        m.put("ConversionRate", parseDouble(h.get("txtexchangerate")));
        m.put("EquivalentAmount", 0.0);
        m.put("FCurrencyAmount", parseDouble(h.get("txttotalfcamount")));
        m.put("GrossWeightKgs", asDouble(h.get("txtgrossweight")));
        m.put("NetWeightKgs", asDouble(h.get("txtnetweight")));
        m.put("NoOfContainers", asDouble(h.get("txtnoofcontainer")));
        m.put("BranchesId", asInt(h.get("cmbbranch")));
        m.put("CompanyId", u.getCompanyId());
        m.put("DeliveryTermId", asInt(h.get("cmbdeliveryterm")));
        m.put("DestinationPortId", asInt(h.get("cmbdestinationport")));
        m.put("EntryUser", u.getId());
        m.put("ExImProformaInvoice", asInt(h.get("cmbPerformaInvoice")));
        m.put("ExporterBankId", asInt(h.get("cmbExporterBanks")));
        m.put("ExportCompanyId", 0);
        m.put("FcurrencyId", asInt(h.get("cmbfc")));
        m.put("CareierTypeId", 0);
        m.put("Id", recId > 0 && updateMode ? recId : 0);
        m.put("ImporterBankId", asInt(h.get("cmbimporterbank")));
        m.put("LcOrderDocNo", (int) toIntText(h.get("txtdocno")));
        m.put("LcOrderStatusId", 0);
        m.put("LoadingPortId", asInt(h.get("cmbloadingport")));
        m.put("ModifyUser", u.getId());
        m.put("NoOfShipments", 0);
        m.put("NotifyPartyId", 0);
        m.put("NotifyParty2Id", 0);
        m.put("NotifyParty3Id", 0);
        m.put("OrganizationId", u.getOrganizationId());
        m.put("PaymentTermId", asInt(h.get("cmbpaymenterm")));
        m.put("PostUser", u.getId());
        m.put("ProductionSchDaysBefore", 0);
        m.put("ProjectsId", 0);
        m.put("SalesPersonId", 0);
        m.put("SampleSchDaysBefore", 0);
        m.put("ShipmentIntervalDays", 0);
        m.put("SupCustId", asInt(h.get("cmbcustomer")));
        m.put("ConsigneeId", 0);
        m.put("ContractType", 0);
        m.put("CommissionAgentId", 0);
        m.put("DeliveryDays", 0);
        m.put("ActionId", (recId > 0 && updateMode) ? 2 : 1);
        m.put("CommRate", 0.0);
        m.put("CommAmount", 0.0);
        m.put("CommodityDetial", str(h.get("txtcommoditydesc")));
        m.put("PaymentRemarks", null);
        m.put("DeliveryRemarks", null);
        m.put("CustomerCode", str(h.get("txtcustomercode")));
        m.put("ExpiryPlace", str(h.get("txtexpplace")));
        m.put("FcurrencySymbol", "");
        m.put("InquiryReference", str(h.get("txtinquiryref")));
        m.put("InsepctionDescription", str(h.get("txtinspectiondesc")));
        m.put("InsepctionRequired", str(h.get("cmbinspectionText")));
        m.put("LcDocAttachment", "");
        m.put("LcOrderNo", str(h.get("txtdocno")));
        m.put("CustomerOrderNo", null);
        m.put("LegalizationDescription", str(h.get("txtlegalizationdesc")));
        m.put("LegalizationRequired", str(h.get("cmblegalizationText")));
        m.put("LotReference", "");
        m.put("PartialShipment", str(h.get("cmbPartialShipmentText")));
        m.put("QuotReference", str(h.get("txtquorationref")));
        m.put("RemarksHeader", str(h.get("txtremarks")));
        m.put("SalesContractRef", str(h.get("txtcustomerrefno")));
        m.put("ShippingMarks", str(h.get("txtshippingmark")));
        m.put("Status", str(h.get("cmbstatusText")));
        m.put("TransShipment", str(h.get("cmbTransShipmentText")));
        m.put("InsuranceRemarks", null);
        m.put("CustomerApprovalRemarks", null);
        m.put("ContactPerson", null);
        m.put("DocumentTypeId", DOCUMENT_TYPE_ID);
        m.put("ShipedToId", 0);
        m.put("FinancialYearId", 0);
        m.put("CustomGroupId", 0);
        m.put("ProductSpecification", null);
        m.put("AddLessComments", null);
        m.put("AttachmentsValues", null);
        m.put("CustomAttachmentsValues", null);
        m.put("AddLessAmount", 0.0);
        m.put("TotalAmountWithAdls", 0.0);

        List<Map<String, Object>> details = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> d = new LinkedHashMap<>();
            d.put("InnerQty", rint(r.get("InnerQty")));
            d.put("NetWeight", rint(r.get("Weigth")));
            d.put("NoOfUnit", 0.0);
            d.put("OuterQty", rint(r.get("OuterQty")));
            d.put("RatePrice", rint(r.get("Rate")));
            d.put("TotalAmount", rint(r.get("Amount")));
            d.put("NoofContainer", 0.0);
            d.put("PackingWeight", 0.0);
            d.put("NoofBagsPerContainer", 0.0);
            d.put("PackingTotalWeight", 0.0);
            d.put("GrossWeight", 0.0);
            d.put("PalletQty", 0.0);
            d.put("ExImItemId", asInt(r.get("ItemId")));
            d.put("PmItemId", 0);
            d.put("ExImLcOrderId", 0);
            d.put("CropYearId", 0);
            d.put("Id", 0);
            d.put("AmountCalulationId", 0);
            d.put("InvPackingMaterialTypeId", asInt(r.get("PackTypeId")));
            d.put("ItemSpecificationId", 0);
            d.put("ItemBrandId", 0);
            d.put("UOMScheduleIdInner", asInt(r.get("InnerPackUOMId")));
            d.put("UOMScheduleIdOuter", asInt(r.get("OuterPackUOMId")));
            d.put("UOMScheduleIdRate", asInt(r.get("RateUOMId")));
            d.put("ActionTypeId", 0);
            d.put("ExImFarmingNTradeId", 0);
            d.put("ExImTradeTypeId", 0);
            d.put("ExImFarmingTypeId", 0);
            d.put("Other_Description", str(r.get("Description")));
            d.put("OuterPackDescription", null);
            d.put("ContainerSize", null);
            d.put("PackingExpiryDate", null);
            d.put("CommodityDetail", null);
            d.put("PackingDetail", null);
            details.add(d);
        }
        int result = repo.save(m, details);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("id", result);
        out.put("message", updateMode ? "Update SuccessFully" : "Save SuccessFully");
        return out;
    }

    // ================================================================= helpers

    private static void requireCombo(Map<String, Object> h, String id, String message) {
        if (text(h.get(id + "Text")).isEmpty() || asInt(h.get(id)) == 0) throw new IllegalArgumentException(message);
    }

    private static void requireNum(Map<String, Object> h, String id, String message) {
        if (str(h.get(id)).isEmpty() || toIntText(h.get(id)) == 0) throw new IllegalArgumentException(message);
    }

    /** Conversion.ToInt(text): Convert.ToInt32(string), failures 0. */
    private static long toIntText(Object v) {
        if (v == null) return 0;
        try { return Integer.parseInt(String.valueOf(v).trim()); }
        catch (NumberFormatException e) { return 0; }
    }

    /** double.Parse(text) - FormatException text as .NET raises it. */
    private static double parseDouble(Object v) {
        try { return Double.parseDouble(String.valueOf(v).trim().replace(",", "")); }
        catch (Exception e) { throw new IllegalArgumentException("Input string was not in a correct format."); }
    }

    /** Conversion.ToInt on a double cell - Convert.ToInt32 rounds half to even. */
    private static double rint(Object v) { return Math.rint(asDouble(v)); }

    private interface Loader { Object load() throws Exception; }

    private static void put(Map<String, Object> out, String key, Loader l) {
        try { out.put(key, l.load()); }
        catch (Exception e) {
            Throwable t = e; while (t.getCause() != null && t.getCause() != t) t = t.getCause();
            out.put(key, key.equals("banks") ? new LinkedHashMap<>() : new ArrayList<>()); out.put(key + "Error", t.getMessage());
        }
    }

    private static List<Map<String, Object>> idName(List<Map<String, Object>> rows, String id, String name) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, id)));
            m.put("name", text(ci(r, name)));
            out.add(m);
        }
        return out;
    }

    /** A double as .NET ToString() prints it (no trailing ".0"). */
    private static String num(Object v) {
        if (v == null) return "";
        double d = asDouble(v);
        if (d == Math.rint(d) && Math.abs(d) < 1e15) return String.valueOf((long) d);
        return String.valueOf(d);
    }

    private static String dateTimeText(Object v) {
        if (v == null) return "";
        if (v instanceof Timestamp) return ((Timestamp) v).toLocalDateTime().toString().replace('T', ' ');
        return String.valueOf(v);
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

    private static String iso(Object v) { return ExportEformRegistrationService.iso(v); }

    private static Timestamp ts(Object v) {
        if (v == null) return new Timestamp(System.currentTimeMillis());
        String s = String.valueOf(v).trim();
        if (s.isEmpty()) return new Timestamp(System.currentTimeMillis());
        try { return Timestamp.valueOf(LocalDate.parse(s.substring(0, 10)).atStartOfDay()); }
        catch (DateTimeParseException | StringIndexOutOfBoundsException e) {
            try { return Timestamp.valueOf(LocalDateTime.parse(s)); } catch (Exception e2) { return new Timestamp(System.currentTimeMillis()); }
        }
    }
}
