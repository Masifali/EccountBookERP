package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.dto.ContraVoucherDto;
import com.mst.repositories.ExportPreInvoiceRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;

import java.lang.reflect.Field;
import java.math.BigDecimal;
import java.math.RoundingMode;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.LocalTime;
import java.time.format.DateTimeFormatter;
import java.time.format.DateTimeParseException;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.LinkedHashSet;
import java.util.List;
import java.util.Map;
import java.util.Set;
import java.util.TreeMap;

/**
 * BLL side of two desktop forms that are one piece of code with a different document type:
 *
 *   192 "Pre Invoice"            Architecture.WinApp.Export.PreCommiercialInvoice              DocumentTypeId 209
 *   194 "Export Opening Balance" Architecture.WinApp.Export.CommiercialInvoiceForOpeningBalance DocumentTypeId 212
 *
 * Differences reproduced from the two .cs files (diff of everything before InitializeComponent):
 *   - 212 sends @BranchesId to GenerateDocNo; 209 does not. 209 generates the Invoice No (GenerateInvoiceNo,
 *     txtinvoiceno read-only); 212 has no generator, the Invoice No is typed and its Leave copies it into an empty GD No.
 *   - 212 has a Credit Account combo (CoaAllocationAccountTitleByAccountTypeIds("3,6,8,10")), validated after Customer
 *     ("Please select CreditAc"), saved as CreditAccountId; it also requires the GD No ("GD No Is Required").
 *   - 212 removes the Other Items and Payment Detail tabs (the rows the loader puts into Other Items are still saved).
 *   - 209 header BranchesId = the hidden Branch combo (its first row); 212 keeps UserAccount.BranchesId.
 *   - 212's Save builds a voucher (BLL ExImInvoice.Save -> MakeVoucherForExImInvoice) - 209 builds none.
 *   - The loader fills the grid from USP_GetProformaDataForPreInvoices (209) / USP_GetProformaDataForInvoices (212).
 *   - "Bank Print-546" prints the plain 546 slip on 209 (desktop quirk) and the bank slip on 212.
 *
 * Rights: DesktopReportRights with the screen's ScreenDefinition.Id (View / Save / Update / Print). Organisation,
 * company, user, branch and financial year come from the session, never from the request.
 */
@Service
public class ExportPreInvoiceService {

    /** The two screens. */
    public enum Variant {
        PRE(192, 209, "PreCommiercialInvoice", "Pre Invoice"),
        OPENING(194, 212, "CommiercialInvoiceForOpeningBalance", "Invoice For Opening");
        public final int screenId, documentTypeId;
        public final String screenName, title;
        Variant(int s, int d, String n, String t) { screenId = s; documentTypeId = d; screenName = n; title = t; }
        public boolean pre() { return this == PRE; }
    }

    @Autowired private ExportPreInvoiceRepository repo;
    @Autowired private CurrentUserContext currentUserContext;
    @Autowired private DesktopReportRights rights;

    private UserAccount user(Variant v, String action) {
        UserAccount u = currentUserContext.requireAccountingUser();
        rights.require(u, v.screenId, action);
        return u;
    }

    private boolean allowed(UserAccount u, Variant v, String action) {
        try { rights.require(u, v.screenId, action); return true; } catch (AccessDeniedException e) { return false; }
    }

    private int fy() { return currentUserContext.currentFinancialYearId(); }

    private static int branch(UserAccount u) { return u.getBranchesId() == null ? 0 : u.getBranchesId(); }

    // ================================================================= load

    /** ImProformaInvoice_Load (both forms). */
    public Map<String, Object> setup(Variant v) {
        UserAccount u = user(v, "View");
        Map<String, Object> out = new LinkedHashMap<>();
        Map<String, Object> perm = new LinkedHashMap<>();
        perm.put("Save", allowed(u, v, "Save"));
        perm.put("Update", allowed(u, v, "Update"));
        perm.put("Print", allowed(u, v, "Print"));
        out.put("permissions", perm);
        out.put("documentTypeId", v.documentTypeId);
        out.putAll(codes(u, v));
        out.putAll(lookups(u, v));
        int days;
        try { days = (int) Double.parseDouble(repo.config(u, "DefaultDaysToLessFromHistoryFromDate")); } catch (Exception e) { days = 0; }
        out.put("defaultDaysToLessFromHistoryFromDate", days);
        out.put("historyCustomers", historyCustomers(u));
        return out;
    }

    /** generateCode (+ GenerateInvoiceNo on 209). */
    private Map<String, Object> codes(UserAccount u, Variant v) {
        Map<String, Object> m = new LinkedHashMap<>();
        int code = repo.generateDocNo(u, v.documentTypeId, fy(), v.pre() ? 0 : branch(u));
        m.put("docNo", code > 0 ? String.valueOf(code) : "");
        if (v.pre()) {
            String inv = repo.generateInvoiceNo(u, v.documentTypeId, fy());
            m.put("invoiceNo", inv);
        }
        return m;
    }

    /** FormReset's server part. */
    public Map<String, Object> newCodes(Variant v) { return codes(user(v, "View"), v); }

    /** btnrefersh_Click: branches, projects, customers, delivery / payment terms, ports, currencies, banks, carrier, pack types, crop years. */
    public Map<String, Object> refresh(Variant v) { return lookups(user(v, "View"), v); }

    private Map<String, Object> lookups(UserAccount u, Variant v) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("branches", pick(repo.branches(u), "Id", "BranchName"));
        m.put("projects", pick(repo.projects(u), "Id", "ProjectName"));
        if (!v.pre()) m.put("creditAccounts", pick(repo.coaByAccountTypes(u, "3,6,8,10"), "Id", "AccountTitle"));
        m.put("customers", pick(repo.exportCustomers(u), "Id", "CompanyName"));
        m.put("deliveryTerms", pick(repo.deliveryTerms(), "Id", "Code"));
        m.put("paymentTerms", pick(repo.paymentTerms(u), "Id", "LcOrderTerm"));
        m.put("ports", pick(repo.seaPorts(u), "Id", "PortName"));
        m.put("currencies", pick(repo.currencies(u), "Id", "CurrencyName"));
        List<Map<String, Object>> exporter = new ArrayList<>(), importer = new ArrayList<>();
        for (Map<String, Object> b : repo.banks(u)) {
            String home = text(ci(b, "IsHomeland"));
            Map<String, Object> r = row("Id", asInt(ci(b, "Id")), "BranchName", text(ci(b, "BranchName")));
            if ("Foreign Country".equals(home)) importer.add(r);
            if ("Home Country".equals(home)) exporter.add(r);
        }
        /* ImporterandExportBankFill binds both only when the home list has rows. */
        m.put("exporterBanks", exporter.isEmpty() ? new ArrayList<>() : exporter);
        m.put("importerBanks", exporter.isEmpty() ? new ArrayList<>() : importer);
        m.put("packTypes", pick(repo.packTypes(u), "Id", "Description", "PackingWeight"));
        m.put("cropYears", pick(repo.cropYears(u), "Id", "CropYear"));
        return m;
    }

    /** HistoryCombosFill: GetDataForDropDownFromExportInvoice rows with ActivityType "Customer". */
    private List<Map<String, Object>> historyCustomers(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.dropDownsFromExportInvoice(u)) {
            if ("Customer".equals(text(ci(r, "ActivityType")))) out.add(row("Id", asInt(ci(r, "Id")), "Customer", text(ci(r, "name"))));
        }
        return out;
    }

    public List<Map<String, Object>> historyCombos(Variant v) { return historyCustomers(user(v, "View")); }

    /** bindRateUomAndItemPackUom: CommonServices.GetUomScheduleByItemId (Id, UOMCode, Equivalent, QtyEquivalent, BaseRateUom). */
    public List<Map<String, Object>> uoms(Variant v, int itemId) {
        UserAccount u = user(v, "View");
        return uomRows(repo.uomsForItem(u, itemId));
    }

    static List<Map<String, Object>> uomRows(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            out.add(row("Id", asInt(ci(r, "Id")), "UOMCode", text(ci(r, "UOMCode")), "Equivalent", asDouble(ci(r, "Equivalent")),
                    "QtyEquivalent", asDouble(ci(r, "QtyEquivalent")), "BaseRateUom", ci(r, "BaseRateUom")));
        }
        return out;
    }

    /** BindFinancialInstrument (cmbSupCust_Leave and after ReadById): usp_GetFINoForInvoice. */
    public List<Map<String, Object>> financialInstruments(Variant v, int supplierCustomerId) {
        UserAccount u = user(v, "View");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.finoForInvoice(u, supplierCustomerId)) {
            out.add(row("Id", asInt(ci(r, "Id")), "EFormNo", text(ci(r, "EFormNo")),
                    "DocumentTypeId", asInt(ci(r, "DocumentTypeId")), "PaymenttermId", asInt(ci(r, "PaymenttermId"))));
        }
        return out;
    }

    /** GetFinancialInstrumentsBalance (CmbFinInstruments ValueChanged). */
    public Map<String, Object> fiBalance(Variant v, int documentTypeId, int id) {
        UserAccount u = user(v, "View");
        Map<String, Object> m = new LinkedHashMap<>();
        Object b = repo.fiBalance(u, documentTypeId, id);
        m.put("balance", b == null ? 0 : (b instanceof Number ? ((Number) b).doubleValue() : asDouble(b)));
        return m;
    }

    // ================================================================= loader (LoadExportPerformaInvoice)

    /**
     * LoadExportPerformaInvoice (the popup btnLoaderForm opens): its combos (GetSupplierustomerByCustomerGroupId("7"),
     * MultiCurrency, ItemGetForComboServiceBind()) and ExportInvoicesLoad - GetProformaDataForPreInvoices with
     * DocumentTypeIds "151,202", ZeroBalanceType 1 and the chosen Party / Item / Currency. The popup's From / To
     * dates are never passed by the desktop and are not here either.
     */
    public Map<String, Object> loaderSetup(Variant v) {
        UserAccount u = user(v, "View");
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("parties", pick(repo.customersByGroup(u, "7"), "Id", "CompanyName"));
        m.put("currencies", pick(repo.currencies(u), "Id", "CurrencyName"));
        m.put("items", pick(repo.itemsComboTwoColumns(u, 0), "Id", "ItemName"));
        return m;
    }

    public List<Map<String, Object>> loaderRows(Variant v, int partyId, int itemId, int currencyId) {
        UserAccount u = user(v, "View");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.proformaData(true, u, branch(u), fy(), "151,202", null, partyId, currencyId, itemId, 1)) {
            Map<String, Object> m = new LinkedHashMap<>();
            for (String k : new String[] { "Id", "DocumentTypeId", "DocNo", "ProformaDate", "ProformaNo", "SalesContratDate", "SupCustId",
                    "CustomerName", "SalesPersonId", "SalesMan", "CommissionAgentId", "CommissionAgent", "CommRate", "CommAmount", "FCurrencyId",
                    "CurrencyCode", "DeliveryTermId", "DeliveryTerm", "PaymentTermId", "PaymentTerm", "LoadingPortId", "LoadingPort",
                    "DestinationPortId", "DestinationPort", "InsuranceRemarks", "RemarksHeader", "EntryUser", "EntryDate", "ModifyUser",
                    "ModifyDate", "NoOfAttachments", "ContractDetailId", "ExImItemId", "ItemName", "PackingType", "PackingWeight", "CropYear",
                    "NoofContainerDetail", "ShipContainer", "BalContainers", "NoofBagsPerContainer", "PackUom", "M_Ton", "ShipMTon", "BalMTon",
                    "NoOfBags", "ShipBags", "BalNoofBags", "PackingTotalWeight", "NetWeight", "ShipWeight", "BalWeight", "RatePrice", "Amount",
                    "ShipAmount", "BalShipAmount", "PackingExpiryDate", "ContainerSize", "Other_Description", "OuterPackDescription" }) {
                Object val = ci(r, k);
                m.put(k, isDate(val) ? isoDateTime(val) : val);
            }
            out.add(m);
        }
        return out;
    }

    /**
     * LoadInGridDetail: InvoiceIds = "," + id per checked proforma (the leading comma is the desktop's), then
     * LoadDataDetailGridAgainstPerformaInvoice (header values from the first row, one grid row per detail row,
     * Id 0) and LoadOtherItemsData(InvoiceIds).
     */
    public Map<String, Object> loadProformas(Variant v, List<Object> ids) {
        UserAccount u = user(v, "View");
        StringBuilder sb = new StringBuilder();
        for (Object o : ids) sb.append(',').append(asInt(o));
        String invoiceIds = sb.toString();
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> grid = new ArrayList<>();
        if (!invoiceIds.isEmpty()) {
            List<Map<String, Object>> rows = repo.proformaData(v.pre(), u, branch(u), fy(), "151,202", invoiceIds, 0, 0, 0, 1);
            if (!rows.isEmpty()) {
                Map<String, Object> r0 = rows.get(0);
                Map<String, Object> h = new LinkedHashMap<>();
                for (String k : new String[] { "SupCustId", "FCurrencyId", "LoadingPortId", "DestinationPortId", "DeliveryTermId", "NotifyPartyId",
                        "PaymentTermId", "FCurrencyAmount", "NoOfContainers", "LotReference", "CommissionAgentId", "CommRate", "CommAmount",
                        "RemarksHeader", "InsuranceRemarks" }) h.put(k, ci(r0, k));
                out.put("header", h);
                for (Map<String, Object> r : rows) {
                    Map<String, Object> d = emptyDetailRow();
                    d.put("PerformaId", asInt(ci(r, "Id")));
                    d.put("PerformaDetailId", asInt(ci(r, "ContractDetailId")));
                    d.put("PerformaDocNo", asInt(ci(r, "DocNo")));
                    d.put("PerformaDate", iso(ci(r, "SalesContratDate")));
                    d.put("PriRefNo", text(ci(r, "ProformaNo")));
                    d.put("ItemId", asInt(ci(r, "ExImItemId")));
                    d.put("ItemName", text(ci(r, "ItemName")));
                    d.put("ItemHSCode", "");
                    d.put("PackTypeId", asInt(ci(r, "InvPackingMaterialTypeId")));
                    d.put("PackType", text(ci(r, "PackingType")));
                    d.put("PackingWeight", asDouble(ci(r, "PackingWeight")));
                    d.put("CropYearId", asInt(ci(r, "CropYearId")));
                    d.put("CropYear", text(ci(r, "CropYear")));
                    d.put("NoOfContainers", asDouble(ci(r, "BalContainers")));
                    d.put("NoOfBagsCntnr", asDouble(ci(r, "NoofBagsPerContainer")));
                    d.put("PackSizeId", asInt(ci(r, "PackUomId")));
                    d.put("PackSize", text(ci(r, "PackUom")));
                    d.put("QtyMTon", asDouble(ci(r, "BalMTon")));
                    d.put("NoOfBags", asDouble(ci(r, "BalNoofBags")));
                    d.put("ValidateBags", asDouble(ci(r, "BalNoofBags")));
                    d.put("NetWeight", asDouble(ci(r, "BalWeight")));
                    d.put("ValidateNetWeight", asDouble(ci(r, "BalWeight")));
                    d.put("TotalPackingWeight", asDouble(ci(r, "PackingTotalWeight")));
                    d.put("GrossWeight", asDouble(ci(r, "GrossWeight")));
                    d.put("CostMTon", asDouble(ci(r, "RatePrice")));
                    d.put("ContractRate", asDouble(ci(r, "RatePrice")));
                    d.put("RateUOMId", asInt(ci(r, "RateUomId")));
                    d.put("RateUOM", text(ci(r, "RateUom")));
                    d.put("Amount", asDouble(ci(r, "BalShipAmount")));
                    d.put("OtherRate", 0d);
                    d.put("OtherAmount", 0d);
                    d.put("OtherHSCode", "");
                    d.put("PackingExpiryDate", text(ci(r, "PackingExpiryDate")));
                    d.put("ItemCommodityDetail", "");
                    d.put("HealthPermitNo", "");
                    grid.add(d);
                }
            }
        }
        out.put("rows", grid);
        List<Map<String, Object>> other = new ArrayList<>();
        for (Map<String, Object> r : repo.otherItemsByProformaIds(invoiceIds)) {
            other.add(row("ItemId", asInt(ci(r, "otherItemId")), "ItemName", text(ci(r, "ItemName")), "Qty", asDouble(ci(r, "oItemQty")),
                    "Rate", asDouble(ci(r, "oItemRate")), "Amount", asDouble(ci(r, "oItemAmount")), "Remarks", text(ci(r, "OtherItemRemarks"))));
        }
        out.put("otherItems", other);
        return out;
    }

    private static Map<String, Object> emptyDetailRow() {
        Map<String, Object> d = new LinkedHashMap<>();
        d.put("Id", 0);
        return d;
    }

    // ================================================================= ReadById

    /** ReadById(Id): header, detail grid, other items, payment terms, InvoiceUpdateFlag, BindOtherItems, BindFinancialInstrument. */
    public Map<String, Object> readById(Variant v, int id) {
        UserAccount u = user(v, "View");
        Map<String, Object> h = repo.invoiceById(id);
        if (h == null) throw new IllegalArgumentException("Index was outside the bounds of the array.");
        if (asInt(ci(h, "CompanyId")) != u.getCompanyId() || asInt(ci(h, "OrganizationId")) != u.getOrganizationId())
            throw new AccessDeniedException("Record belongs to another company");
        Map<String, Object> out = new LinkedHashMap<>();
        Map<String, Object> hd = new LinkedHashMap<>();
        for (String k : new String[] { "Id", "BranchesId", "ProjectsId", "DocCode", "InvoiceNo", "SupplierCustomerId", "CommissionAgentId",
                "LcOrderNoId", "NotifyParty1", "NotifyParty2", "LotNoRef", "PaymentTermId", "ImporterBankId", "ExporteBankId", "DeliveryTermId",
                "LoadingPortId", "DestinationPortId", "OtherCustomerId", "OtherDestinationPortId", "CarierType", "FcurrencyId", "EFormNo",
                "Certificate1", "Certificate2", "AddLessComments", "CreditAccountId", "DocumentTypeId" }) hd.put(k, plain(ci(h, k)));
        for (String k : new String[] { "CommissionPercentage", "CommissionAmount", "FCurrencyAmount", "ConversionRate", "EquivalentAmount",
                "GrossWeight", "NetWeight", "NoOfContainers", "AddLessAmount", "TotalAmount" }) hd.put(k, asDouble(ci(h, k)));
        hd.put("DocDate", iso(ci(h, "DocDate")));
        hd.put("EFormDate", iso(ci(h, "EFormDate")));
        out.put("header", hd);
        List<Map<String, Object>> rows = new ArrayList<>();
        int flag = 0;
        boolean first = true;
        for (Map<String, Object> d : repo.invoiceDetail(id)) {
            if (first) { flag = asInt(ci(d, "ContractType")); first = false; }
            rows.add(detailFromDb(d));
        }
        out.put("rows", rows);
        out.put("invoiceUpdateFlag", flag);
        List<Map<String, Object>> other = new ArrayList<>();
        for (Map<String, Object> r : repo.invoiceOtherItems(id)) {
            other.add(row("ItemId", asInt(ci(r, "otherItemId")), "ItemName", text(ci(r, "ItemName")), "Qty", asDouble(ci(r, "oItemQty")),
                    "Rate", asDouble(ci(r, "oItemRate")), "Amount", asDouble(ci(r, "oItemAmount")), "Remarks", text(ci(r, "OtherItemRemarks"))));
        }
        out.put("otherItems", other);
        List<Map<String, Object>> pts = new ArrayList<>();
        for (Map<String, Object> r : repo.invoicePaymentTerms(id)) {
            pts.add(row("Id", asInt(ci(r, "Id")), "DocumentTypeId", asInt(ci(r, "DocumentTypeId")), "ExImEFormRegistrationId",
                    asInt(ci(r, "ExImEFormRegistrationId")), "FinancialInstrumentNo", text(ci(r, "FinancialInstrumentNo")),
                    "PaymentTermId", asInt(ci(r, "PaymentTermId")), "PaymentTerm", text(ci(r, "PaymentTerm")),
                    "PrcntOfTotal", asDouble(ci(r, "PrcntOfTotal")), "FcyAmount", asDouble(ci(r, "FcyAmount")), "DueDays", asInt(ci(r, "DueDays"))));
        }
        out.put("paymentTerms", pts);
        /* BindOtherItems (only ReadById binds the Other Items combo): ItemId, ItemName and the third column oItemRate
           (CmbOtherItem_TextChanged copies Cells[2] into the rate). Desktop quirk kept: the form passes Id 0 and the
           procedure filters ex.Id = @Id, so the list is always empty. */
        List<Map<String, Object>> oi = new ArrayList<>();
        for (Map<String, Object> r : repo.otherItemsByContract(u)) {
            oi.add(row("ItemId", asInt(ci(r, "ItemId")), "ItemName", text(ci(r, "ItemName")), "Rate", asDouble(ci(r, "oItemRate"))));
        }
        out.put("otherItemList", oi);
        out.put("financialInstruments", financialInstrumentsFor(u, asInt(ci(h, "SupplierCustomerId"))));
        return out;
    }

    private List<Map<String, Object>> financialInstrumentsFor(UserAccount u, int cust) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.finoForInvoice(u, cust)) {
            out.add(row("Id", asInt(ci(r, "Id")), "EFormNo", text(ci(r, "EFormNo")),
                    "DocumentTypeId", asInt(ci(r, "DocumentTypeId")), "PaymenttermId", asInt(ci(r, "PaymenttermId"))));
        }
        return out;
    }

    /** One dtdetail row from ReadExImInvoicePackingDetailByHeaderId (ReadById's dtdetail.Rows.Add). */
    static Map<String, Object> detailFromDb(Map<String, Object> d) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("Id", asInt(ci(d, "Id")));
        m.put("PerformaId", asInt(ci(d, "ContractId")));
        m.put("PerformaDetailId", asInt(ci(d, "ContractDetailId")));
        m.put("PerformaDocNo", asInt(ci(d, "ProformaDocNo")));
        m.put("PerformaDate", iso(ci(d, "ProformaDocDate")));
        m.put("PriRefNo", text(ci(d, "PriRefNo")));
        m.put("ItemId", asInt(ci(d, "ItemId")));
        m.put("ItemName", text(ci(d, "ItemName")));
        m.put("ItemHSCode", text(ci(d, "HsCode")));
        m.put("PackTypeId", asInt(ci(d, "PackingMaterialTypeId")));
        m.put("PackType", text(ci(d, "PackMaterilaType")));
        m.put("PackingWeight", asDouble(ci(d, "PackingWeight")));
        m.put("CropYearId", asInt(ci(d, "CropYearId")));
        m.put("CropYear", text(ci(d, "CropYear")));
        m.put("NoOfContainers", asDouble(ci(d, "NoofContainers")));
        m.put("NoOfBagsCntnr", asDouble(ci(d, "NoofBagsPerContainer")));
        m.put("PackSizeId", asInt(ci(d, "OuterQtyUomId")));
        m.put("PackSize", text(ci(d, "OuterUOM")));
        m.put("QtyMTon", asDouble(ci(d, "MTon")));
        m.put("NoOfBags", asDouble(ci(d, "OuterQty")));
        m.put("ValidateBags", asDouble(ci(d, "OuterQty")));
        m.put("NetWeight", asDouble(ci(d, "NetWeight")));
        m.put("ValidateNetWeight", asDouble(ci(d, "NetWeight")));
        m.put("TotalPackingWeight", asDouble(ci(d, "TotalPackingWeight")));
        m.put("GrossWeight", asDouble(ci(d, "GrossWeight")));
        m.put("CostMTon", asDouble(ci(d, "RatePrice")));
        m.put("ContractRate", asDouble(ci(d, "ContractRate")));
        m.put("RateUOMId", asInt(ci(d, "RateUomId")));
        m.put("RateUOM", text(ci(d, "RateUOM")));
        m.put("Amount", asDouble(ci(d, "FcAmount")));
        m.put("OtherRate", asDouble(ci(d, "OtherRate")));
        m.put("OtherAmount", asDouble(ci(d, "OtherAmount")));
        m.put("OtherHSCode", text(ci(d, "OtherHsCode")));
        m.put("PackingExpiryDate", text(ci(d, "PackingExpiryDate")));
        m.put("ItemCommodityDetail", text(ci(d, "ItemCommodityDetail")));
        m.put("HealthPermitNo", text(ci(d, "HealthPermitNoDetail")));
        return m;
    }

    /** GetDetailByHeaderId - the lower history grid (ExImInvoice.GetByID(...).ExImInvoiceDetail). */
    public List<Map<String, Object>> historyDetail(Variant v, int id) {
        user(v, "View");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> d : repo.invoiceDetail(id)) out.add(detailFromDb(d));
        return out;
    }

    // ================================================================= history

    /**
     * HistoryGridFill: FinancialYearId, Ids = "209" / "212", the ticked From / To into the checked radio's pair
     * (Doc / Entry / Modify / Approved date), FromDocNo / ToDocNo / SupplierCustomerId when non-zero; ApprovedFilter
     * "All" (no @IsApproved); no @Id, @NoOfRecords, paging, @ActionId or @IsForSpecialApproval.
     */
    public List<Map<String, Object>> history(Variant v, Map<String, Object> f) {
        UserAccount u = user(v, "View");
        Map<String, Object> g = new LinkedHashMap<>();
        String by = text(f.get("dateBy"));
        String fromKey = "FromDate", toKey = "ToDate";
        if ("entry".equals(by)) { fromKey = "EntryFromDate"; toKey = "EntryToDate"; }
        else if ("modify".equals(by)) { fromKey = "ModifyFromDate"; toKey = "ModifyToDate"; }
        else if ("approved".equals(by)) { fromKey = "ApprovedFromDate"; toKey = "ApprovedToDate"; }
        LocalDate from = asBool(f.get("fromChecked")) ? asDate(f.get("fromDate")) : null;
        LocalDate to = asBool(f.get("toChecked")) ? asDate(f.get("toDate")) : null;
        if (from != null) g.put(fromKey, Timestamp.valueOf(LocalDateTime.of(from, LocalTime.now().withNano(0))));
        if (to != null) g.put(toKey, Timestamp.valueOf(LocalDateTime.of(to, LocalTime.now().withNano(0))));
        int cust = asInt(f.get("customerId"));
        if (cust != 0) g.put("SupplierCustomerId", cust);
        g.put("DocumentTypeIds", String.valueOf(v.documentTypeId));
        int fromDoc = cTextInt(f.get("fromDocNo")), toDoc = cTextInt(f.get("toDocNo"));
        if (fromDoc != 0) g.put("DocNoFrom", fromDoc);
        if (toDoc != 0) g.put("DocNoTo", toDoc);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.history(u, fy(), g)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("InvoiceNo", text(ci(r, "InvoiceNo")));
            m.put("DocCode", asInt(ci(r, "DocCode")));
            m.put("DocDate", iso(ci(r, "DocDate")));
            m.put("CustomerName", text(ci(r, "Customer")));
            m.put("NoOfContainers", asDouble(ci(r, "NoOfContainers")));
            m.put("GrossWeight", asDouble(ci(r, "GrossWeight")));
            m.put("NetWeight", asDouble(ci(r, "NetWeight")));
            m.put("FcyAmount", asDouble(ci(r, "FCurrencyAmount")));
            m.put("AddLess", asDouble(ci(r, "AddLessAmount")));
            m.put("TotalAmount", asDouble(ci(r, "TotalAmount")));
            m.put("LoadingPort", text(ci(r, "LoadingPort")));
            m.put("DestinationPort", text(ci(r, "DestinationPort")));
            m.put("EntryUser", text(ci(r, "EntryUserName")));
            m.put("EntryDate", iso(ci(r, "EntryDate")));
            m.put("ModifyUser", text(ci(r, "ModifyUserName")));
            m.put("ModifyDate", iso(ci(r, "ModifyDate")));
            m.put("ApprovedStatus", text(ci(r, "ApprovedStatus")));
            m.put("ApprovedUser", text(ci(r, "ApprovedUserName")));
            m.put("ApprovedDate", iso(ci(r, "ApprovedDate")));
            m.put("NoOfAttachments", text(ci(r, "NoOfAttachments")));
            out.add(m);
        }
        return out;
    }

    // ================================================================= save

    /**
     * btnsave_Click / btnupdate_Click (Update calls Save). Order as the desktop: grid empty -> "Grid Record not
     * found"; (the page asks Save / Update); txtNetWeight = sum(M.Ton) * 1000; formvalidation(); header and
     * lists built exactly as the form does; TotalAmount = grid Amount + Add/Less + Other Items (Qty > 0 only);
     * payment rows present and their total != TotalAmount -> "Total Invoice Amount and Total Payment Utilize
     * Amount Not equal Please Check"; ExImInvoice.Save (212 builds the voucher); "Save SuccessFully" /
     * "Update SuccessFully".
     */
    public Map<String, Object> save(Variant v, Map<String, Object> body) {
        int recId = asInt(body.get("recId"));
        UserAccount u = user(v, recId > 0 ? "Update" : "Save");
        Map<String, Object> h = map(body.get("header"));
        List<Map<String, Object>> rows = list(body.get("rows"));
        List<Map<String, Object>> removed = list(body.get("removed"));
        List<Map<String, Object>> others = list(body.get("otherItems"));
        List<Map<String, Object>> payments = v.pre() ? list(body.get("paymentTerms")) : new ArrayList<>();
        if (rows.isEmpty()) throw new IllegalArgumentException("Grid Record not found");
        if (recId > 0) {
            Map<String, Object> old = repo.invoiceById(recId);
            if (old == null || asInt(ci(old, "CompanyId")) != u.getCompanyId() || asInt(ci(old, "OrganizationId")) != u.getOrganizationId()
                    || asInt(ci(old, "DocumentTypeId")) != v.documentTypeId)
                throw new IllegalArgumentException("Record not found");
        }
        double mton = 0;
        for (Map<String, Object> r : rows) mton += asDouble(r.get("QtyMTon"));
        double netWeightKg = mton * 1000.0;
        h.put("txtNetWeight", clr(netWeightKg));
        validate(v, h);

        Map<String, Object> m = invoiceModel();
        if (recId > 0) m.put("Id", recId);
        Timestamp now = Timestamp.valueOf(LocalDateTime.now().withNano(0));
        m.put("OrganizationId", u.getOrganizationId());
        m.put("CompanyId", u.getCompanyId());
        m.put("BranchesId", branch(u));
        m.put("IsApproved", false);
        m.put("EntryDate", now);
        m.put("EntryUser", u.getId());
        m.put("ModifyDate", now);
        m.put("ModifyUser", u.getId());
        m.put("ApprovedDate", now);
        m.put("DocumentTypeId", v.documentTypeId);
        m.put("FinancialYearId", fy());
        if (v.pre()) m.put("BranchesId", asInt(h.get("cmbbranches")));       // the hidden Branch combo (first row)
        m.put("ProjectsId", asInt(h.get("cmbproject")));
        m.put("DocCode", cTextInt(h.get("txtdocno")));
        m.put("DocDate", ts(h.get("txtDocDate")));
        m.put("InvoiceNo", text(h.get("txtinvoiceno")));
        m.put("SupplierCustomerId", asInt(h.get("cmbSupCust")));
        if (!v.pre()) m.put("CreditAccountId", asInt(h.get("CmbCreditAccount")));
        m.put("NotifyParty1", asInt(h.get("cmbNotifyParty1")));
        m.put("NotifyParty2", asInt(h.get("cmbNotifyParty2")));
        m.put("LotNoRef", raw(h.get("txtLotRef")));
        m.put("PaymentTermId", asInt(h.get("cmbPaymentTermsNew")));
        m.put("ImporterBankId", asInt(h.get("cmbimporterBankNew")));
        m.put("ExporteBankId", asInt(h.get("cmbexporterBankNew")));
        m.put("DeliveryTermId", asInt(h.get("cmbdeliverytermnew")));
        m.put("LoadingPortId", asInt(h.get("cmbLoadingPort")));
        m.put("DestinationPortId", asInt(h.get("cmbDestinationPort")));
        m.put("OtherDestinationPortId", asInt(h.get("cmbOtherDestinationPort")));
        m.put("OtherCustomerId", asInt(h.get("cmbOtherCustomer")));
        m.put("CarierType", raw(h.get("cmbcareiertypeText")));
        m.put("CommissionAgentId", asInt(h.get("cmbCommissionAgent")));
        m.put("CommissionPercentage", cDbl(h.get("txtCommPercent")));
        m.put("CommissionAmount", cDbl(h.get("txtCommAmount")));
        m.put("FcurrencyId", asInt(h.get("cmbfcycode")));
        m.put("FCurrencyAmount", cDbl(h.get("txtfcyAmount")));
        m.put("ConversionRate", cDbl(h.get("txtExRate")));
        m.put("EquivalentAmount", cDbl(h.get("txtLocalAmt")));
        m.put("GrossWeight", cDbl(h.get("txtGrossWeight")));
        m.put("NetWeight", cDbl(h.get("txtNetWeight")));
        m.put("NoOfContainers", (double) cTextInt(h.get("txtNoOfContainer")));   // Conversion.ToInt(text): "2.5" -> 0
        m.put("EFormNo", text(h.get("txtIformNo")));
        m.put("EFormDate", ts(h.get("txtIformDate")));
        m.put("Certificate2", text(h.get("txtcertificateOfOrigion")));
        m.put("Certificate1", text(h.get("txtcertificate")));
        m.put("AddLessComments", raw(h.get("txtaddlesscommnets")));
        m.put("AddLessAmount", cDbl(h.get("txtaddlessamount")));
        m.put("TotalAmount", cDbl(h.get("txtTotalNetAmount")));
        /* Desktop quirk: RemarksHeader is read from txtoRemarks (the Other Items "Remarks" box), not txtremarksheader. */
        m.put("RemarksHeader", text(h.get("txtoRemarks")));

        List<Map<String, Object>> details = new ArrayList<>();
        if (recId > 0) for (Map<String, Object> r : removed) {
            Map<String, Object> d = detailModel(r);
            d.put("ActionTypeId", 3);
            details.add(d);
        }
        double totalFc = 0;
        List<String> cIds = new ArrayList<>(), cDet = new ArrayList<>(), cNos = new ArrayList<>(), cDates = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> d = detailModel(r);
            d.put("ActionTypeId", asInt(d.get("Id")) <= 0 ? 1 : 2);
            totalFc += asDouble(d.get("FcAmount"));
            if (v.pre()) {
                cIds.add(String.valueOf(asInt(r.get("PerformaId"))));
                cDet.add(String.valueOf(asInt(r.get("PerformaDetailId"))));
                cNos.add(text(r.get("PriRefNo")));
                cDates.add(shortDate(r.get("PerformaDate")));
            }
            details.add(d);
        }
        if (v.pre()) {
            m.put("ContractIds", distinctJoin(cIds));
            m.put("ContractDetailIds", distinctJoin(cDet));
            m.put("ContractNos", distinctJoin(cNos));
            m.put("ContractDates", distinctJoin(cDates));
        }
        double otherAmount = 0;
        List<Map<String, Object>> otherModels = new ArrayList<>();
        for (Map<String, Object> o : others) {
            BigDecimal qty = dec(o.get("Qty"));
            if (qty.compareTo(BigDecimal.ZERO) > 0) {
                Map<String, Object> om = new LinkedHashMap<>();
                om.put("oItemAmount", dec(o.get("Amount")));
                om.put("oItemQty", qty);
                om.put("oItemWeightKgs", BigDecimal.ZERO);
                om.put("oItemRate", asDouble(o.get("Rate")));
                om.put("ExImInvoiceId", 0);
                om.put("Id", 0);
                om.put("otherItemId", asInt(o.get("ItemId")));
                om.put("OtherItemRemarks", text(o.get("Remarks")));
                otherAmount += dec(o.get("Amount")).doubleValue();
                otherModels.add(om);
            }
        }
        double utilize = 0;
        List<Map<String, Object>> ptModels = new ArrayList<>();
        int sort = 0;
        for (Map<String, Object> p : payments) {
            sort++;
            Map<String, Object> pm = new LinkedHashMap<>();
            pm.put("FcyAmount", asDouble(p.get("FcyAmount")));
            pm.put("PrcntOfTotal", asDouble(p.get("PrcntOfTotal")));
            pm.put("DueDays", asInt(Math.rint(asDouble(p.get("DueDays")))));
            pm.put("EntryUserId", 0);
            pm.put("ModifyUserId", 0);
            pm.put("ExImEFormRegistrationId", asInt(p.get("ExImEFormRegistrationId")));
            pm.put("ExImInvoiceId", 0);
            pm.put("RefDocumentTypeId", 0);
            pm.put("Id", asInt(p.get("Id")));
            pm.put("PaymentTermId", asInt(p.get("PaymentTermId")));
            pm.put("DocumentTypeId", asInt(p.get("DocumentTypeId")));
            pm.put("SortNo", sort);
            pm.put("FinancialInstrumentNo", text(p.get("FinancialInstrumentNo")));
            pm.put("PaymentRemarks", null);
            utilize += asDouble(pm.get("FcyAmount"));
            ptModels.add(pm);
        }
        double totalAmount = totalFc + asDouble(m.get("AddLessAmount")) + otherAmount;
        m.put("TotalAmount", totalAmount);
        if (!payments.isEmpty() && totalAmount != utilize)
            throw new IllegalArgumentException("Total Invoice Amount and Total Payment Utilize Amount Not equal Please Check");

        Map<String, Object> vh = null;
        List<Map<String, Object>> vds = new ArrayList<>();
        if (!v.pre()) {
            Object[] voucher = makeVoucher(u, m, otherModels);
            vh = model(voucher[0]);
            for (Object d : (List<?>) voucher[1]) vds.add(model(d));
        }
        int id = repo.saveInvoice(m, recId == 0 ? "Sp_ExImInvoice_Insert" : "Sp_ExImInvoice_Update", details, ptModels, otherModels, u, vh, vds);
        if (id <= 0) id = recId;
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("id", id);
        out.put("message", recId == 0 ? "Save SuccessFully" : "Update SuccessFully");
        return out;
    }

    /** formvalidation() - the desktop's order and texts (212 adds Credit Account and GD No). */
    private static void validate(Variant v, Map<String, Object> h) {
        String docNo = raw(h.get("txtdocno"));
        if (docNo.isEmpty() || cTextInt(docNo) == 0) throw new IllegalArgumentException("Doc No Is Required");
        String inv = raw(h.get("txtinvoiceno"));
        if (inv.isEmpty() || inv.trim().equals("0")) throw new IllegalArgumentException("Invoice No Is Required");
        if (asInt(h.get("cmbSupCust")) == 0) throw new IllegalArgumentException("Customer Is Required");
        if (!v.pre() && asInt(h.get("CmbCreditAccount")) == 0) throw new IllegalArgumentException("Please select CreditAc");
        if (asInt(h.get("cmbPaymentTermsNew")) == 0) throw new IllegalArgumentException("Payment Term Is Required");
        if (asInt(h.get("cmbdeliverytermnew")) == 0) throw new IllegalArgumentException("Delivery Term Is Required");
        if (asInt(h.get("cmbLoadingPort")) == 0) throw new IllegalArgumentException("Loading Port Is Required");
        if (asInt(h.get("cmbDestinationPort")) == 0) throw new IllegalArgumentException("Destination Port  Is Required");
        if (raw(h.get("cmbDestinationPortText")).trim().equals(raw(h.get("cmbLoadingPortText")).trim()))
            throw new IllegalArgumentException("Destination Port and Loaing Port cannot be same...");
        if (raw(h.get("cmbcareiertypeText")).trim().isEmpty()) throw new IllegalArgumentException("Carier Type  Is Required");
        String lot = raw(h.get("txtLotRef"));
        if (lot.isEmpty() || lot.equals("0")) throw new IllegalArgumentException("Lot Ref No Is Required");
        if (asInt(h.get("cmbfcycode")) == 0) throw new IllegalArgumentException("Fcy Code Is Required");
        if (cDbl(h.get("txtfcyAmount")) == 0.0) throw new IllegalArgumentException("Fcy Amount  Is Required");
        if (!(cDbl(h.get("txtNetWeight")) > 0.0)) throw new IllegalArgumentException("Net Weight  Is Required");
        if (!(cDbl(h.get("txtGrossWeight")) >= cDbl(h.get("txtNetWeight"))))
            throw new IllegalArgumentException("Gross Weight Must Be Equal Or Greater Than Net Weight Thank You");
        boolean agent = asInt(h.get("cmbCommissionAgent")) != 0;
        if (cDbl(h.get("txtCommPercent")) > 0.0 && !agent)
            throw new IllegalArgumentException("As you have entered 'Commission %', the 'Commission Agent' field is required. ");
        if (cDbl(h.get("txtCommPercent")) == 0.0 && agent)
            throw new IllegalArgumentException("As you have selected 'Commission agent', please enter the commission percentage.");
        if (!v.pre()) {
            String gd = raw(h.get("txtIformNo"));
            if (gd.isEmpty() || gd.equals("0")) throw new IllegalArgumentException("GD No Is Required");
        }
        if (!(cDbl(h.get("txtExRate")) > 0.0)) throw new IllegalArgumentException("ExchangeRate Field is Required");
        if (!(cDbl(h.get("txtTotalNetAmount")) > 0.0)) throw new IllegalArgumentException("Total Amount  Is Required");
    }

    /** Split(',').Distinct() of ",a,b,a" then Join(",").TrimStart(',') - what the form stores in ContractIds etc. */
    private static String distinctJoin(List<String> parts) {
        Set<String> s = new LinkedHashSet<>();
        s.add("");
        s.addAll(parts);
        String j = String.join(",", s);
        int i = 0;
        while (i < j.length() && j.charAt(i) == ',') i++;
        return j.substring(i);
    }

    /**
     * BLL ExImInvoice.MakeVoucherForExImInvoice for DocumentTypeId 212 (OB): the customer's GL from
     * GetSupplierCustomerListForFinancialEffects ("Customer GLAccountId not Found" when the list has rows but
     * not this customer; RefAccountId stays 0 when the list is empty, as on the desktop), one debit / credit pair
     * for the whole invoice (TotalAmount * ConversionRate) and a pair per Other Item. The commission pair needs
     * CommissionDebitAcId, which this form never sets, so it is never built. ActionId stays 1 and RefDocNoId is
     * the Id before saving (0 for a new invoice) - both exactly as the BLL leaves them.
     */
    private Object[] makeVoucher(UserAccount u, Map<String, Object> obj, List<Map<String, Object>> otherItems) {
        ContraVoucherDto.Head vh = new ContraVoucherDto.Head();
        List<ContraVoucherDto.Detail> lines = new ArrayList<>();
        int id = asInt(obj.get("Id"));
        vh.DocumentTypeId = asInt(obj.get("DocumentTypeId"));
        vh.DocumentTypeSrNo = id;
        vh.RefDocNoId = id;
        vh.VoucherCode = asInt(obj.get("DocCode"));
        vh.VoucherDate = dt(obj.get("DocDate"));
        vh.Remarks = text(obj.get("RemarksHeader"));
        vh.RemarksOtherLingo = "";
        vh.VoucherAmount = asDouble(obj.get("EquivalentAmount"));
        vh.FcAmount = asDouble(obj.get("TotalAmount"));
        vh.ExchangeCurrencyRate = asDouble(obj.get("ConversionRate"));
        vh.MultiCurrencyId = asInt(obj.get("FcurrencyId"));
        int customer = asInt(obj.get("SupplierCustomerId"));
        List<Map<String, Object>> gl = repo.customerGlAccounts(u);
        if (!gl.isEmpty()) {
            Map<String, Object> hit = null;
            for (Map<String, Object> g : gl) if (asInt(ci(g, "Id")) == customer) { hit = g; break; }
            if (hit == null) throw new IllegalArgumentException("Customer GLAccountId not Found");
            vh.RefAccountId = asInt(ci(hit, "GlAccountId"));
            int agent = asInt(obj.get("CommissionAgentId"));
            if (agent > 0) {
                boolean found = false;
                for (Map<String, Object> g : gl) if (asInt(ci(g, "Id")) == agent) { found = true; break; }
                if (!found) throw new IllegalArgumentException("Commission Agent GLAccountId not Found");
            }
        }
        vh.ChequeDate = LocalDate.now() + " 00:00:00";
        vh.IncludeWHT = false;
        vh.BranchId = asInt(obj.get("BranchesId"));
        vh.ProjectId = asInt(obj.get("ProjectsId"));
        vh.BillAmount = asDouble(obj.get("TotalAmount"));
        vh.ManualBillNo = text(obj.get("InvoiceNo"));
        vh.DueDate = dt(obj.get("EFormDate"));
        vh.DueDays = 0;
        vh.OrganizationId = asInt(obj.get("OrganizationId"));
        vh.CompanyId = asInt(obj.get("CompanyId"));
        vh.FinancialYearId = asInt(obj.get("FinancialYearId"));
        vh.EntryUser = asInt(obj.get("EntryUser"));
        String now = LocalDateTime.now().withNano(0).format(DateTimeFormatter.ofPattern("yyyy-MM-dd HH:mm:ss"));
        vh.EntryDate = now;
        vh.ModifyDate = now;
        vh.ModifyUser = asInt(obj.get("ModifyUser"));
        vh.ActionId = 1;
        int credit = asInt(obj.get("CreditAccountId"));
        double net = asDouble(obj.get("NetWeight"));
        double total = asDouble(obj.get("TotalAmount"));
        double rate = asDouble(obj.get("ConversionRate"));
        String text2 = " M.Ton: " + clr(net / 1000.0) + " NetWeight: " + clr(net) + " FcAmount: " + clr(asDouble(obj.get("FCurrencyAmount")))
                + " AddLess: " + clr(asDouble(obj.get("AddLessAmount"))) + " TotalFcyAmount: " + clr(total);
        if (credit == 0) throw new IllegalArgumentException("CreditAccount Not Found please Check");
        double num6 = total * rate;
        ContraVoucherDto.Detail d3 = new ContraVoucherDto.Detail();
        d3.AccountId = vh.RefAccountId; d3.AgainstAccountId = credit; d3.Comments = text2; d3.DebitAmount = num6; d3.CreditAmount = 0d;
        d3.QtyOut = net / 1000.0; d3.WeightOut = net; d3.ItemAmount = num6; d3.DCurrencyAmount = total; d3.DExchangeCurrencyRate = rate;
        d3.DMultiCurrencyId = vh.MultiCurrencyId; d3.SupplierCustomerId = customer; d3.DocumentTypeIdRef = 1; d3.RefInvoiceNo = "SALE";
        lines.add(d3);
        ContraVoucherDto.Detail d4 = new ContraVoucherDto.Detail();
        d4.AccountId = credit; d4.AgainstAccountId = vh.RefAccountId; d4.Comments = text2; d4.CreditAmount = num6;
        d4.QtyOut = net / 1000.0; d4.WeightOut = net; d4.ItemAmount = num6; d4.DCurrencyAmount = total; d4.DExchangeCurrencyRate = rate;
        d4.DMultiCurrencyId = vh.MultiCurrencyId; d4.SupplierCustomerId = customer; d4.DocumentTypeIdRef = 1; d4.RefInvoiceNo = "SALE";
        lines.add(d4);
        for (Map<String, Object> it : otherItems) {
            double amt = ((BigDecimal) it.get("oItemAmount")).doubleValue();
            double num8 = amt * rate;
            double qty = ((BigDecimal) it.get("oItemQty")).doubleValue();
            ContraVoucherDto.Detail d7 = new ContraVoucherDto.Detail();
            d7.AccountId = vh.RefAccountId; d7.AgainstAccountId = credit; d7.Comments = text(it.get("OtherItemRemarks")); d7.DebitAmount = num8;
            d7.CreditAmount = 0d; d7.ItemId = asInt(it.get("otherItemId")); d7.QtyOut = qty; d7.ItemRate = asDouble(it.get("oItemRate"));
            d7.WeightOut = qty; d7.ItemAmount = num8; d7.DCurrencyAmount = amt; d7.DExchangeCurrencyRate = rate;
            d7.DMultiCurrencyId = vh.MultiCurrencyId; d7.DocumentTypeIdRef = 1; d7.RefInvoiceNo = "SALE";
            lines.add(d7);
            ContraVoucherDto.Detail d8 = new ContraVoucherDto.Detail();
            d8.AccountId = credit; d8.AgainstAccountId = vh.RefAccountId; d8.Comments = text(it.get("OtherItemRemarks")); d8.CreditAmount = num8;
            d8.DebitAmount = 0d; d8.ItemId = asInt(it.get("otherItemId")); d8.QtyOut = qty; d8.ItemRate = asDouble(it.get("oItemRate"));
            d8.WeightOut = qty; d8.ItemAmount = num8; d8.DCurrencyAmount = amt; d8.DExchangeCurrencyRate = rate;
            d8.DMultiCurrencyId = vh.MultiCurrencyId;
            /* BLL quirk: the credit line's DocumentTypeIdRef / RefInvoiceNo are written onto the debit line again, so
               the credit line keeps 0 / null. */
            lines.add(d8);
        }
        return new Object[] { vh, lines };
    }

    // ================================================================= model maps (GenericProvider.SetProc order)

    /** Architecture.Model.Export.ExImInvoice - the 91 non-virtual properties with their CLR defaults. */
    static Map<String, Object> invoiceModel() {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("IsApproved", false);
        for (String k : new String[] { "ApprovedDate", "DocDate", "EntryDate", "ModifyDate" }) m.put(k, null);
        for (String k : new String[] { "BuyerGLValueRs", "ConversionRate", "EquivalentAmount", "FCurrencyAmount", "FobValue", "GrossWeight",
                "InvoiceCommercialValue", "NetWeight", "NoOfContainers", "AddLessAmount", "CommissionPercentage", "CommissionAmount", "TotalAmount" }) m.put(k, 0d);
        m.put("GrossMton", BigDecimal.ZERO);
        m.put("NetMton", BigDecimal.ZERO);
        m.put("AddLessComments", null);
        for (String k : new String[] { "CommercialInvoiceId", "ApprovedUser", "BranchesId", "CompanyId", "PrefixTypeId", "DeliveryTermId",
                "ContinentId", "OriginCountryId", "ImporterCountryId", "PlaceOfDeliveryId", "DestinationPortId", "DocCode", "DocumentTypeId",
                "EFormId", "EntryUser", "ExporteBankId", "ExportCompanyId", "FcurrencyId", "Id", "ImporterBankId", "LcOrderNoId", "LoadingPortId",
                "ModifyUser", "OrganizationId", "PaymentTermId", "ProjectsId", "SupplierCustomerId", "CommissionAgentId", "FinancialYearId",
                "PreInvoiceId", "CommissionDebitAcId", "OtherCustomerId", "OtherDestinationPortId", "ConsigneeId", "ActionId" }) m.put(k, 0);
        for (String k : new String[] { "CarierType", "InvoiceNo", "InvStatus", "LotNoRef", "OtherInstructions", "RemarksHeader", "SupplierCode",
                "TermsConditions", "EFormNo", "ContractIds", "ContractDetailIds", "ContractNos", "ContractScheduleIds", "ContractScheduleNos",
                "ContractDates", "DocumentaryCreditNo" }) m.put(k, null);
        m.put("DocumentaryCreditNoIssueDate", null);
        m.put("PaymentRemarks", null);
        m.put("DeliveryRemarks", null);
        m.put("TCPRegNo", null);
        for (String k : new String[] { "NotifyParty1", "NotifyParty2", "NotifyParty3", "CreditAccountId", "SubContractExpiryDays" }) m.put(k, 0);
        for (String k : new String[] { "Certificate1", "Certificate2", "Remarks1", "Remarks2", "OtherRemarks1", "OtherRemarks2",
                "CustomerContractNos", "AttachmentsValues", "CustomAttachmentsValues" }) m.put(k, null);
        m.put("EFormDate", null);
        return m;
    }

    /** ExImInvoicePackingDetail (56 non-virtual properties) filled as btnsave_Click / grdDetail_ColumnButtonClick fill it. */
    static Map<String, Object> detailModel(Map<String, Object> r) {
        Map<String, Object> d = new LinkedHashMap<>();
        for (String k : new String[] { "FcAmount", "NetWeight", "OuterQty", "RatePrice", "RateAddLess", "RateWithoutAddLess", "PackingWeight",
                "TotalPackingWeight", "GrossWeight", "NoofContainers", "NoofBagsPerContainer", "MTon", "OtherRate", "OtherAmount", "AddLessAmount" }) d.put(k, 0d);
        d.put("OtherHsCode", null);
        for (String k : new String[] { "ExImFarmingNTradeId", "ExImTradeTypeId", "ExImFarmingTypeId", "LabInspectionId" }) d.put(k, 0);
        d.put("HsCode", null);
        for (String k : new String[] { "PmItemId", "BrandId", "ExImInvoiceId", "ContractId", "ContractDetailId", "Id" }) d.put(k, 0);
        d.put("InnerQty", 0d);
        for (String k : new String[] { "InnerQtyUomId", "ItemId", "ItemUOMId", "CropYearId", "OuterQtyUomId", "PackingMaterialTypeId", "RateUomId",
                "JobLotId", "ItemSpicificationId", "PreInvoiceId", "PreInvoiceDetailId", "LineId", "ContractScheduleId", "ContractScheduleDetailId",
                "ActionTypeId", "AmountCalulationId" }) d.put(k, 0);
        for (String k : new String[] { "ItemDescriptionManual", "PackingExpiryDate", "ProductionNo", "OuterPackDescription", "HealthPermitNoDetail",
                "ItemCommodityDetail", "BuyerLotNo", "BuyerHSCode", "ProductCode", "ContractNoShipmentWise", "Remarks" }) d.put(k, null);
        d.put("PalletQty", 0d);
        d.put("Id", asInt(r.get("Id")));
        d.put("ContractId", asInt(r.get("PerformaId")));
        d.put("ContractDetailId", asInt(r.get("PerformaDetailId")));
        d.put("ItemId", asInt(r.get("ItemId")));
        d.put("HsCode", text(r.get("ItemHSCode")));
        d.put("PackingMaterialTypeId", asInt(r.get("PackTypeId")));
        d.put("PackingWeight", asDouble(r.get("PackingWeight")));
        d.put("CropYearId", asInt(r.get("CropYearId")));
        d.put("NoofContainers", asDouble(r.get("NoOfContainers")));
        d.put("NoofBagsPerContainer", asDouble(r.get("NoOfBagsCntnr")));
        d.put("OuterQtyUomId", asInt(r.get("PackSizeId")));
        d.put("InnerQtyUomId", asInt(r.get("PackSizeId")));
        d.put("MTon", asDouble(r.get("QtyMTon")));
        d.put("OuterQty", asDouble(r.get("NoOfBags")));
        d.put("InnerQty", (double) cvInt(asDouble(r.get("NoOfBags"))));            // Conversion.ToInt(double) - banker's rounding
        d.put("NetWeight", (double) cvInt(asDouble(r.get("NetWeight"))));
        d.put("TotalPackingWeight", asDouble(r.get("TotalPackingWeight")));
        d.put("GrossWeight", asDouble(r.get("GrossWeight")));
        d.put("RatePrice", asDouble(r.get("CostMTon")));
        d.put("RateUomId", asInt(r.get("RateUOMId")));
        d.put("FcAmount", asDouble(r.get("Amount")));
        d.put("OtherRate", asDouble(r.get("OtherRate")));
        d.put("OtherAmount", asDouble(r.get("OtherAmount")));
        d.put("PackingExpiryDate", text(r.get("PackingExpiryDate")));
        d.put("OtherHsCode", text(r.get("OtherHSCode")));
        d.put("HealthPermitNoDetail", text(r.get("HealthPermitNo")));
        d.put("ItemCommodityDetail", text(r.get("ItemCommodityDetail")));
        return d;
    }

    /** Public fields in declaration order (as StockAdjustmentService.model) - the voucher DTOs; dates to Timestamp. */
    static Map<String, Object> model(Object o) {
        Map<String, Object> m = new LinkedHashMap<>();
        for (Field f : o.getClass().getFields()) {
            if (java.lang.reflect.Modifier.isStatic(f.getModifiers())) continue;
            if (List.class.isAssignableFrom(f.getType())) continue;
            try {
                Object v = f.get(o);
                String n = f.getName();
                if (v != null && (n.equals("VoucherDate") || n.equals("ChequeDate") || n.equals("DueDate") || n.equals("EntryDate")
                        || n.equals("ModifyDate") || n.equals("PostDate") || n.equals("DCheqDate") || n.equals("GpDate"))) {
                    String s = String.valueOf(v);
                    v = Timestamp.valueOf(s.length() == 10 ? s + " 00:00:00" : s);
                }
                m.put(n, v);
            } catch (IllegalAccessException ignored) { }
        }
        return m;
    }

    // ================================================================= helpers (shared by the Export forms of this port)

    static Map<String, Object> row(Object... kv) {
        Map<String, Object> m = new LinkedHashMap<>();
        for (int i = 0; i + 1 < kv.length; i += 2) m.put(String.valueOf(kv[i]), kv[i + 1]);
        return m;
    }

    /** Projects rows to the named columns (first = value, the rest as they come). */
    static List<Map<String, Object>> pick(List<Map<String, Object>> rows, String... cols) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            for (int i = 0; i < cols.length; i++) {
                Object v = ci(r, cols[i]);
                if (i == 0) m.put(cols[i], asInt(v));
                else m.put(cols[i], v instanceof Number ? v : text(v));
            }
            out.add(m);
        }
        return out;
    }

    static Object ci(Map<String, Object> row, String name) {
        if (row == null) return null;
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet()) if (e.getKey().equalsIgnoreCase(name)) return e.getValue();
        return null;
    }

    static Object plain(Object v) {
        if (v instanceof Number) return v;
        return v == null ? "" : String.valueOf(v).trim();
    }

    @SuppressWarnings("unchecked")
    static Map<String, Object> map(Object v) {
        if (v instanceof Map) return new TreeMap<String, Object>((Map<String, Object>) v);
        return new TreeMap<>();
    }

    @SuppressWarnings("unchecked")
    static List<Map<String, Object>> list(Object v) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (v instanceof List) for (Object o : (List<Object>) v) if (o instanceof Map) out.add((Map<String, Object>) o);
        return out;
    }

    static String text(Object v) { return v == null ? "" : String.valueOf(v).trim(); }

    static String raw(Object v) { return v == null ? "" : String.valueOf(v); }

    static int asInt(Object v) {
        if (v == null) return 0;
        if (v instanceof Number) return ((Number) v).intValue();
        String s = String.valueOf(v).trim();
        try { return Integer.parseInt(s); }
        catch (NumberFormatException e) {
            try { return (int) Double.parseDouble(s.replace(",", "")); } catch (NumberFormatException e2) { return 0; }
        }
    }

    /** Conversion.ToInt(string): Convert.ToInt32 - an integer string only, anything else (e.g. "2.5", "1,000") is 0. */
    static int cTextInt(Object v) {
        if (v == null) return 0;
        if (v instanceof Number) return cvInt(((Number) v).doubleValue());
        String s = String.valueOf(v).trim();
        if (s.isEmpty()) return 0;
        try { return Integer.parseInt(s.startsWith("+") ? s.substring(1) : s); } catch (NumberFormatException e) { return 0; }
    }

    /** Convert.ToInt32(double) - round half to even. */
    static int cvInt(double d) { return (int) Math.rint(d); }

    static double asDouble(Object v) {
        if (v == null) return 0;
        if (v instanceof Number) return ((Number) v).doubleValue();
        try { return Double.parseDouble(String.valueOf(v).trim().replace(",", "")); } catch (NumberFormatException e) { return 0; }
    }

    /** Conversion.ToDouble(text) - Convert.ToDouble with thousands separators allowed; failure is 0. */
    static double cDbl(Object v) { return asDouble(v); }

    static BigDecimal dec(Object v) {
        double d = asDouble(v);
        return d == 0 ? BigDecimal.ZERO : new BigDecimal(String.valueOf(d));
    }

    static boolean asBool(Object v) {
        if (v == null) return false;
        if (v instanceof Boolean) return (Boolean) v;
        String s = String.valueOf(v).trim().toLowerCase();
        return "true".equals(s) || "1".equals(s) || "on".equals(s);
    }

    static LocalDate asDate(Object v) {
        if (v == null) return null;
        String s = String.valueOf(v).trim();
        if (s.isEmpty()) return null;
        if (s.length() > 10) s = s.substring(0, 10);
        try { return LocalDate.parse(s); } catch (DateTimeParseException e) { return null; }
    }

    /** A DateTimePicker value: the chosen date with the current time of day (what .Value carries). */
    static Timestamp ts(Object v) {
        LocalDate d = asDate(v);
        if (d == null) d = LocalDate.now();
        return Timestamp.valueOf(LocalDateTime.of(d, LocalTime.now().withNano(0)));
    }

    static String dt(Object v) {
        if (v instanceof Timestamp) return ((Timestamp) v).toLocalDateTime().withNano(0).format(DateTimeFormatter.ofPattern("yyyy-MM-dd HH:mm:ss"));
        LocalDate d = asDate(v);
        return (d == null ? LocalDate.now() : d) + " 00:00:00";
    }

    /** DateTime.ToShortDateString() on the desktop's en-US culture: M/d/yyyy. */
    static String shortDate(Object v) {
        LocalDate d = asDate(v);
        if (d == null) d = LocalDate.of(1, 1, 1);
        return d.getMonthValue() + "/" + d.getDayOfMonth() + "/" + d.getYear();
    }

    static boolean isDate(Object v) {
        return v instanceof java.util.Date || v instanceof java.time.temporal.Temporal;
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
        return String.valueOf(v);
    }

    /** double.ToString() - integral values without a decimal part, otherwise up to 15 significant digits. */
    static String clr(double d) {
        if (Double.isNaN(d) || Double.isInfinite(d)) return "0";
        if (d == Math.rint(d) && Math.abs(d) < 1e15) return String.valueOf((long) d);
        BigDecimal b = new BigDecimal(d).round(new java.math.MathContext(15, RoundingMode.HALF_EVEN)).stripTrailingZeros();
        return b.toPlainString();
    }
}
