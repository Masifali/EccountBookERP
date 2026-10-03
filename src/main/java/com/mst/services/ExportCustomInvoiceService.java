package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.ExportCustomInvoiceRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;

import java.math.BigDecimal;
import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.services.ExportBankGdSupport.*;

/**
 * The BLL side of 951 frmCustomInvoice "Custom / Bank Invoice" (Architecture.WinApp.Export), the
 * DocumentTypeId 214 variant of the export invoice (rights name "EximInvoice"). tabControl1 Form |
 * History. Form: toolStrip4 New / Save / Update(hidden) / Refresh / Print(521) / Print PL(529) /
 * 501B / Attachment / Generate Invoice Nos / Item Detail LookUp / Export Charges / ShortCut Keys and
 * the "Print Preview" check; HeaderGropBox (Branch, Project, Doc No, Doc Date, Invoice No (commercial
 * invoice combo - the master invoice's Net / Dispatch / Bal weight), Custom Invoice No, Last Invoice
 * Number, Customer, Consignee, Notify Party 1/2, Export Company (feature 9), Credit A/c
 * (FinancialisActiveonCommercialInvoice), Lot Ref, Customer Contract Nos, Delivery Term + remarks,
 * Payment Term + remarks, Importer / Exporter Bank, Loading / Destination Port, Carrier Type, E-Form
 * No & Date, Fcy Code, Fcy Amount, Ex Rate, Local Amount, Gross / Net Weight (M.Ton), No Of
 * Containers, Add/Less Amount + comments, Total Net Amount); tabControl2 Item Detail | Other Items |
 * Payment Terms | Commission (feature 15) | Other Charges, each with its own grid.
 *
 * Rights by ScreenDefinition.Id 951: Save, Update, Print. Everything a request could tamper with is
 * validated here again with the desktop texts; the arithmetic that only drives the boxes lives on
 * the page. Attachments (Attachment / AttachmentAddingFromHistory) are not part of the web port.
 */
@Service
public class ExportCustomInvoiceService {

    public static final int SCREEN_ID = 951;
    public static final int DOCUMENT_TYPE_ID = 214;

    @Autowired private ExportCustomInvoiceRepository repo;
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

    private int fy() { try { return currentUserContext.currentFinancialYearId(); } catch (RuntimeException e) { return 0; } }

    // ================================================================= load

    /** ImProformaInvoice_Load: ConfigureRights + ConfigureControls + BindData (every combo) + the item list + FI list. */
    public Map<String, Object> setup() {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        Map<String, Object> perm = new LinkedHashMap<>();
        perm.put("Save", allowed(u, "Save"));
        perm.put("Update", allowed(u, "Update"));
        perm.put("Print", allowed(u, "Print"));
        out.put("permissions", perm);
        int days;
        try { days = (int) Double.parseDouble(repo.config(u, "DefaultDaysToLessFromHistoryFromDate")); }
        catch (NumberFormatException e) { days = 0; }
        out.put("defaultDaysToLessFromHistoryFromDate", days);
        out.put("flagFin", asBool(repo.config(u, "FinancialisActiveonCommercialInvoice")));
        put(out, "allowExportMultiCompanies", () -> repo.erpFeature(u, 9));
        put(out, "saleCommissionTabFeatureWise", () -> repo.erpFeature(u, 15));
        out.putAll(combos(u, 0));
        put(out, "historyCombos", () -> historyCombos(u));
        return out;
    }

    /** BindData + ItemDetailFill + GetFINumberForCustomInvoice (also btnRefresh_Click). */
    public Map<String, Object> combos(UserAccount u, int recId) {
        Map<String, Object> out = new LinkedHashMap<>();
        put(out, "branches", () -> idName(repo.branches(u), "Id", "BranchName"));
        put(out, "projects", () -> idName(repo.projects(u), "Id", "ProjectName"));
        put(out, "docNo", () -> generateCode(u));
        put(out, "lastInvoiceNumber", () -> lastInvoiceNumber(u));
        put(out, "commercialInvoices", () -> commercialInvoices(u, recId));
        put(out, "customers", () -> idName(repo.exportCustomers(u), "Id", "CompanyName"));
        put(out, "farmingNTrade", () -> farmingRows(repo.farmingNTrade()));
        put(out, "exportCompanies", () -> idName(repo.exportCompanies(u), "Id", "CompName"));
        put(out, "deliveryTerms", () -> deliveryRows(repo.deliveryTerms()));
        put(out, "paymentTerms", () -> paymentTermRows(repo.paymentTerms(u)));
        put(out, "ports", () -> portRows(repo.seaPorts(u)));
        put(out, "banks", () -> bankRows(repo.banks(u)));
        put(out, "creditAccounts", () -> idName(repo.creditAccounts(u), "Id", "AccountTitle"));
        put(out, "items", () -> itemRows(repo.allItems(u)));
        put(out, "fis", () -> fiRows(repo.fiNos(u, recId)));
        put(out, "otherItems", () -> otherItemRows(repo.otherItems(u)));
        put(out, "currencies", () -> idName(repo.currencies(u), "Id", "CurrencyName"));
        put(out, "cropYears", () -> idName(repo.cropYears(u), "Id", "CropYear"));
        put(out, "jobLots", () -> idName(repo.jobLots(u), "Id", "JobLotDescription"));
        put(out, "packTypes", () -> idName(repo.packTypes(u), "Id", "Description"));
        put(out, "exportCharges", () -> idName(repo.exportCharges(u), "ExportChargesID", "ExportChargesName"));
        put(out, "salesPersons", () -> idName(repo.supplierCustomersByGroup(u, "9", 0), "Id", "CompanyName"));
        return out;
    }

    public Map<String, Object> refresh(int recId) { return combos(user("View"), recId); }

    public int generateCode(UserAccount u) {
        List<Map<String, Object>> r = repo.generateCode(u, fy());
        return r.isEmpty() ? 0 : asInt(ci(r.get(0), "DocNo"));
    }

    public String lastInvoiceNumber(UserAccount u) {
        List<Map<String, Object>> r = repo.lastCustomInvoiceNumber(u);
        return r.isEmpty() ? "" : text(ci(r.get(0), "InvoiceNo"));
    }

    /** generateCode(): DocNo (shown when > 0) + txtLastInvoiceNumber. */
    public Map<String, Object> generateCode() {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("docNo", generateCode(u));
        out.put("lastInvoiceNumber", lastInvoiceNumber(u));
        return out;
    }

    /** BindCommercialInvoiceNo -> dtInvoiceCombo (ExImInvoiceId, InvoiceNo, InvoiceDate, ContractNo, CreditAccountId, CreditAccount, CustomerName, DestinationPort, weights, amounts, SupplierCustomerId). */
    public List<Map<String, Object>> commercialInvoices(UserAccount u, int recId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.invoicesForCustomInvoice(u, fy(), recId)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("ExImInvoiceId", asInt(ci(r, "ExImInvoiceId")));
            m.put("InvoiceNo", text(ci(r, "InvoiceNo")));
            m.put("InvoiceDate", iso(ci(r, "InvoiceDate")));
            m.put("ContractNo", text(ci(r, "ContractNo")));
            m.put("CreditAccountId", asInt(ci(r, "CreditAccountId")));
            m.put("CreditAccount", text(ci(r, "CreditAccount")));
            m.put("CustomerName", text(ci(r, "CustomerName")));
            m.put("DestinationPort", text(ci(r, "DestinationPort")));
            m.put("InvoiceQty", asDouble(ci(r, "InvoiceQty")));
            m.put("UsedQty", asDouble(ci(r, "UsedQty")));
            m.put("BalQty", asDouble(ci(r, "BalQty")));
            m.put("InvoiceWeight", asDouble(ci(r, "InvoiceWeight")));
            m.put("UsedWeight", asDouble(ci(r, "UsedWeight")));
            m.put("BalWeight", asDouble(ci(r, "BalWeight")));
            m.put("InvoiceAmount", asDouble(ci(r, "InvoiceAmount")));
            m.put("UsedAmount", asDouble(ci(r, "UsedAmount")));
            m.put("BalAmount", asDouble(ci(r, "BalAmount")));
            m.put("SupplierCustomerId", asInt(ci(r, "SupplierCustomerId")));
            out.add(m);
        }
        return out;
    }

    public List<Map<String, Object>> commercialInvoices(int recId) { return commercialInvoices(user("View"), recId); }

    private static List<Map<String, Object>> idName(List<Map<String, Object>> rows, String idCol, String nameCol) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, idCol)));
            m.put("Name", text(ci(r, nameCol)));
            out.add(m);
        }
        return out;
    }

    private static List<Map<String, Object>> farmingRows(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("FarmingNTrade", text(ci(r, "FarmingNTrade")));
            m.put("ExImFarmingTypeId", asInt(ci(r, "ExImFarmingTypeId")));
            m.put("ExImTradeTypeId", asInt(ci(r, "ExImTradeTypeId")));
            out.add(m);
        }
        return out;
    }

    private static List<Map<String, Object>> deliveryRows(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("Code", text(ci(r, "Code")));
            m.put("Description", text(ci(r, "Description")));
            out.add(m);
        }
        return out;
    }

    private static List<Map<String, Object>> paymentTermRows(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("lcOrderTerm", text(ci(r, "lcOrderTerm")));
            m.put("Description", text(ci(r, "lcOrderTermDesc")));
            out.add(m);
        }
        return out;
    }

    /** LoadingPortFill: PortType "Loading" / "Destination". */
    private static List<Map<String, Object>> portRows(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("PortName", text(ci(r, "PortName")));
            m.put("PortType", text(ci(r, "PortType")));
            out.add(m);
        }
        return out;
    }

    /** ImporterandExportBankFill: IsHomeland "Home Country" -> exporter, "Foreign Country" -> importer. */
    private static List<Map<String, Object>> bankRows(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("BranchName", text(ci(r, "BranchName")));
            m.put("IsHomeland", text(ci(r, "IsHomeland")));
            out.add(m);
        }
        return out;
    }

    /** ItemdtFillFromGlobal: getGlobalAllItems without ItemTypeOfTypeId 14 / 17. */
    private static List<Map<String, Object>> itemRows(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            int tot = asInt(ci(r, "ItemTypeOfTypeId"));
            if (tot == 14 || tot == 17) continue;
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("ItemName", text(ci(r, "ItemName")));
            m.put("ItemCode", text(ci(r, "ItemCode")));
            m.put("ItemCategory", text(ci(r, "ItemCategory")));
            m.put("ItemType", text(ci(r, "ItemType")));
            out.add(m);
        }
        return out;
    }

    private static List<Map<String, Object>> fiRows(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("FINo", text(ci(r, "FINo")));
            m.put("DocumentTypeId", asInt(ci(r, "DocumentTypeId")));
            m.put("FIBalance", asDouble(ci(r, "FIBalance")));
            out.add(m);
        }
        return out;
    }

    /** BindOtherItems: Id / ItemName (+ a third column, the rate, when the procedure returns one - CmbOtherItem_TextChanged reads cell [2]). */
    private static List<Map<String, Object>> otherItemRows(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("ItemName", text(ci(r, "ItemName")));
            int i = 0; Object third = null;
            for (Object v : r.values()) { if (i == 2) { third = v; break; } i++; }
            m.put("ThirdColumn", third == null ? null : text(third));
            m.put("ColumnCount", r.size());
            out.add(m);
        }
        return out;
    }

    /** GetFINumberForCustomInvoice(RecId) - DetailFormReset re-reads it. */
    public List<Map<String, Object>> fis(int recId) { return fiRows(repo.fiNos(user("View"), recId)); }

    /** bindConsigneeAgainstCustomer: GetSupplierustomerByCustomerGroupId(null, customerId). */
    public List<Map<String, Object>> consignees(int customerId) {
        UserAccount u = user("View");
        if (customerId <= 0) return new ArrayList<>();
        return idName(repo.supplierCustomersByGroup(u, null, customerId), "Id", "CompanyName");
    }

    /** bindRateUomAndItemPackUom: GetUomScheduleByItemId (Id, UOMCode, Equivalent). */
    public List<Map<String, Object>> uoms(int itemId) {
        UserAccount u = user("View");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.uomSchedule(u, itemId)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("UOMCode", text(ci(r, "UOMCode")));
            m.put("Equivalent", asDouble(ci(r, "Equivalent")));
            out.add(m);
        }
        return out;
    }

    /** HSCodeBind: CommodityDetailItemCustomerWise.GetRemarks(item, customer) -> Id / HSCode. */
    public List<Map<String, Object>> hsCodes(int itemId, int customerId) {
        UserAccount u = user("View");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.hsCodes(u, itemId, customerId)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("HSCode", text(ci(r, "HSCode")));
            out.add(m);
        }
        return out;
    }

    /** HistoryCombosFill: ActivityType Customer -> CmbCustomerHistory, Invoice -> CmbInvoiceNoHistory. */
    public Map<String, Object> historyCombos(UserAccount u) {
        List<Map<String, Object>> customers = new ArrayList<>(), invoices = new ArrayList<>();
        for (Map<String, Object> r : repo.historyDropDowns(u)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("Name", text(ci(r, "name")));
            String t = text(ci(r, "ActivityType"));
            if ("Customer".equals(t)) customers.add(m);
            else if ("Invoice".equals(t)) invoices.add(m);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("customers", customers);
        out.put("invoices", invoices);
        return out;
    }

    public Map<String, Object> historyCombos() { return historyCombos(user("View")); }

    // ================================================================= history

    /** HistoryGridFill -> the dt columns; DocDate is "dd-MMM-yyyy" text on the desktop, kept ISO here. */
    public List<Map<String, Object>> history(Map<String, Object> body) {
        UserAccount u = user("View");
        String kind = text(body.get("dateKind"));
        java.sql.Date from = asBool(body.get("fromChecked")) ? sqlDate(body.get("fromDate")) : null;
        java.sql.Date to = asBool(body.get("toChecked")) ? sqlDate(body.get("toDate")) : null;
        int actionId = asInt(body.get("actionId"));
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.formHistory(u, fy(), kind.isEmpty() ? "doc" : kind, from, to,
                asInt(body.get("customerId")), asInt(body.get("invoiceId")), asInt(body.get("fromDocNo")), asInt(body.get("toDocNo")), actionId)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("InvoiceNo", text(ci(r, "InvoiceNo")));
            m.put("DocCode", asInt(ci(r, "DocCode")));
            m.put("DocDate", iso(ci(r, "DocDate")));
            m.put("CustomerName", text(ci(r, "Customer")));
            m.put("ContractNos", text(ci(r, "ContractNos")));
            m.put("NoOfContainers", asDouble(ci(r, "NoOfContainers")));
            m.put("GrossWeight", asDouble(ci(r, "GrossWeight")));
            m.put("NetWeight", asDouble(ci(r, "NetWeight")));
            m.put("FcyAmount", asDouble(ci(r, "FCurrencyAmount")));
            m.put("AddLess", asDouble(ci(r, "AddLessAmount")));
            m.put("TotalAmount", asDouble(ci(r, "TotalAmount")));
            m.put("JobLots", text(ci(r, "JobLots")));
            m.put("LoadingPort", text(ci(r, "LoadingPort")));
            m.put("DestinationPort", text(ci(r, "DestinationPort")));
            m.put("EntryUser", text(ci(r, "EntryUserName")));
            m.put("EntryDate", isoDateTime(ci(r, "EntryDate")));
            m.put("ModifyUser", text(ci(r, "ModifyUserName")));
            m.put("ModifyDate", isoDateTime(ci(r, "ModifyDate")));
            m.put("NoOfAttachments", asInt(ci(r, "NoOfAttachments")));
            out.add(m);
        }
        return out;
    }

    /** GetDetailGrdByHeadId -> the dtdetail rows of the selected history row. */
    public List<Map<String, Object>> historyDetail(int id) { user("View"); return packingRows(repo.packingDetail(id)); }

    // ================================================================= ReadById

    /** ReadById(Id): GetByID = header + the five child lists. */
    public Map<String, Object> readById(int id) {
        user("View");
        List<Map<String, Object>> h = repo.headerById(id);
        if (h.isEmpty()) throw new IllegalStateException("Index was out of range. Must be non-negative and less than the size of the collection.");
        Map<String, Object> r = h.get(0);
        Map<String, Object> o = new LinkedHashMap<>();
        o.put("Id", asInt(ci(r, "Id")));
        o.put("BranchesId", asInt(ci(r, "BranchesId")));
        o.put("ProjectsId", asInt(ci(r, "ProjectsId")));
        o.put("DocCode", asInt(ci(r, "DocCode")));
        o.put("DocDate", iso(ci(r, "DocDate")));
        o.put("CommercialInvoiceId", asInt(ci(r, "CommercialInvoiceId")));
        o.put("InvoiceNo", text(ci(r, "InvoiceNo")));
        o.put("SupplierCustomerId", asInt(ci(r, "SupplierCustomerId")));
        o.put("ConsigneeId", asInt(ci(r, "ConsigneeId")));
        o.put("NotifyParty1", asInt(ci(r, "NotifyParty1")));
        o.put("NotifyParty2", asInt(ci(r, "NotifyParty2")));
        o.put("LotNoRef", text(ci(r, "LotNoRef")));
        o.put("ImporterBankId", asInt(ci(r, "ImporterBankId")));
        o.put("ExporteBankId", asInt(ci(r, "ExporteBankId")));
        o.put("DeliveryTermId", asInt(ci(r, "DeliveryTermId")));
        o.put("LoadingPortId", asInt(ci(r, "LoadingPortId")));
        o.put("DestinationPortId", asInt(ci(r, "DestinationPortId")));
        o.put("CarierType", text(ci(r, "CarierType")));
        o.put("FcurrencyId", asInt(ci(r, "FcurrencyId")));
        o.put("FCurrencyAmount", asDouble(ci(r, "FCurrencyAmount")));
        o.put("EquivalentAmount", asDouble(ci(r, "EquivalentAmount")));
        o.put("ExportCompanyId", asInt(ci(r, "ExportCompanyId")));
        o.put("GrossMton", asDouble(ci(r, "GrossMton")));
        o.put("GrossWeight", asDouble(ci(r, "GrossWeight")));
        o.put("NetMton", asDouble(ci(r, "NetMton")));
        o.put("NetWeight", asDouble(ci(r, "NetWeight")));
        o.put("NoOfContainers", asDouble(ci(r, "NoOfContainers")));
        o.put("EFormNo", text(ci(r, "EFormNo")));
        o.put("EFormDate", iso(ci(r, "EFormDate")));
        o.put("PaymentRemarks", text(ci(r, "PaymentRemarks")));
        o.put("DeliveryRemarks", text(ci(r, "DeliveryRemarks")));
        o.put("AddLessComments", text(ci(r, "AddLessComments")));
        o.put("AddLessAmount", asDouble(ci(r, "AddLessAmount")));
        o.put("TotalAmount", asDouble(ci(r, "TotalAmount")));
        o.put("CreditAccountId", asInt(ci(r, "CreditAccountId")));
        o.put("CustomerContractNos", text(ci(r, "CustomerContractNos")));
        o.put("ConversionRate", asDouble(ci(r, "ConversionRate")));
        o.put("ExportVoucherId", asInt(ci(r, "ExportVoucherId")));
        o.put("PaymentTermId", asInt(ci(r, "PaymentTermId")));
        o.put("detail", packingRows(repo.packingDetail(id)));
        List<Map<String, Object>> oi = new ArrayList<>();
        for (Map<String, Object> d : repo.otherItemsDetail(id)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("ItemId", asInt(ci(d, "otherItemId")));
            m.put("ItemName", text(ci(d, "ItemName")));
            m.put("Qty", asDouble(ci(d, "oItemQty")));
            m.put("Rate", asDouble(ci(d, "oItemRate")));
            m.put("Amount", asDouble(ci(d, "oItemAmount")));
            m.put("Remarks", text(ci(d, "OtherItemRemarks")));
            oi.add(m);
        }
        o.put("otherItems", oi);
        List<Map<String, Object>> pt = new ArrayList<>();
        for (Map<String, Object> d : repo.paymentTermsDetail(id)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(d, "Id")));
            m.put("PaymentTermId", asInt(ci(d, "PaymentTermId")));
            m.put("PaymentTerm", text(ci(d, "PaymentTerm")));
            m.put("DocumentTypeId", asInt(ci(d, "DocumentTypeId")));
            m.put("ExImEFormRegistrationId", asInt(ci(d, "ExImEFormRegistrationId")));
            m.put("FinancialInstrumentNo", text(ci(d, "FinancialInstrumentNo")));
            m.put("PrcntOfTotal", asDouble(ci(d, "PrcntOfTotal")));
            m.put("FcyAmount", asDouble(ci(d, "FcyAmount")));
            m.put("DueDays", asInt(ci(d, "DueDays")));
            m.put("Remarks", text(ci(d, "PaymentRemarks")));
            pt.add(m);
        }
        o.put("paymentTerms", pt);
        List<Map<String, Object>> cm = new ArrayList<>();
        for (Map<String, Object> d : repo.commissionDetail(id)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(d, "exImInvoiceSaleManCommissionId")));
            m.put("SalesPersonId", asInt(ci(d, "SalesPersonId")));
            m.put("SalesPerson", text(ci(d, "SalesPerson")));
            m.put("CommissionTypeId", asInt(ci(d, "commissionTypeId")));
            m.put("CommissionType", text(ci(d, "CommissionType")));
            m.put("Rate", asDouble(ci(d, "commissionRate")));
            m.put("RateUom", text(ci(d, "rateUom")));
            m.put("FcyAmount", asDouble(ci(d, "fcyAmount")));
            m.put("ExchangeRate", asDouble(ci(d, "exchangeRate")));
            m.put("LcyAmount", asDouble(ci(d, "lcyAmount")));
            m.put("Remarks", text(ci(d, "Remarks")));
            cm.add(m);
        }
        o.put("commissions", cm);
        List<Map<String, Object>> oc = new ArrayList<>();
        for (Map<String, Object> d : repo.otherChargesDetail(id)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(d, "Id")));
            m.put("ContractId", asInt(ci(d, "ContractId")));
            m.put("ContractOtherChargesDetailId", asInt(ci(d, "ContractOtherChargesDetailId")));
            m.put("ChargesItemId", asInt(ci(d, "ChargesItemId")));
            m.put("ChargesItem", text(ci(d, "ChargesItemName")));
            m.put("AddAmount", asDouble(ci(d, "AddAmount")));
            m.put("LessAmount", asDouble(ci(d, "LessAmount")));
            m.put("Remarks", text(ci(d, "Remarks")));
            oc.add(m);
        }
        o.put("otherCharges", oc);
        return o;
    }

    /** dtdetail columns from an ExImInvoicePackingDetail row. */
    private static List<Map<String, Object>> packingRows(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> d : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(d, "Id")));
            m.put("ItemId", asInt(ci(d, "ItemId")));
            m.put("ItemName", text(ci(d, "ItemName")));
            m.put("PackTypeId", asInt(ci(d, "PackingMaterialTypeId")));
            m.put("PackType", text(ci(d, "PackMaterilaType")));
            m.put("CropYearId", asInt(ci(d, "CropYearId")));
            m.put("CropYear", text(ci(d, "CropYear")));
            m.put("JobLotId", asInt(ci(d, "JobLotId")));
            m.put("JobLot", text(ci(d, "JobLotDescription")));
            m.put("QtyMTon", asDouble(ci(d, "MTon")));
            m.put("PackSizeId", asInt(ci(d, "OuterQtyUomId")));
            m.put("PackSize", text(ci(d, "OuterUOM")));
            m.put("NoOfBags", asDouble(ci(d, "OuterQty")));
            m.put("CostMTon", asDouble(ci(d, "RatePrice")));
            m.put("RateUOMId", asInt(ci(d, "RateUomId")));
            m.put("RateUOM", text(ci(d, "RateUOM")));
            m.put("RateEquivalent", asDouble(ci(d, "RateEquivalent")));
            m.put("Amount", asDouble(ci(d, "FcAmount")));
            m.put("HSCode", text(ci(d, "HsCode")));
            m.put("ItemDetail", text(ci(d, "ItemDescriptionManual")));
            m.put("PackingDetail", text(ci(d, "OuterPackDescription")));
            m.put("OtherItemAmount", asDouble(ci(d, "OtherAmount")));
            m.put("AddLessAmount", asDouble(ci(d, "AddLessAmount")));
            m.put("ExImFarmingNTradeId", asInt(ci(d, "ExImFarmingNTradeId")));
            m.put("FarmingNTrade", text(ci(d, "FarmingNTrade")));
            m.put("ExImFarmingTypeId", asInt(ci(d, "ExImFarmingTypeId")));
            m.put("ExImTradeTypeId", asInt(ci(d, "ExImTradeTypeId")));
            out.add(m);
        }
        return out;
    }

    // ================================================================= save

    /**
     * btnsave_Click after the page's own checks: header validation (formvalidation, desktop order and
     * texts), the payment-term / other-charges row checks, the "Total Invoice Amount and Total Payment
     * Utilize Amount Not equal" rule, then ExImInvoice.Save (Insert when recId 0, Update otherwise).
     * The header carries EVERY non-virtual ExImInvoice property (the ones the form never sets go as 0 /
     * omitted, as GenericProvider.SetProc sends them).
     */
    public Map<String, Object> save(Map<String, Object> body) {
        int recId = asInt(body.get("recId"));
        UserAccount u = user(recId == 0 ? "Save" : "Update");
        Map<String, Object> h = map(body.get("header"));
        List<Map<String, Object>> detail = list(body.get("detail"));
        List<Map<String, Object>> removed = list(body.get("removed"));
        List<Map<String, Object>> otherItems = list(body.get("otherItems"));
        List<Map<String, Object>> paymentTerms = list(body.get("paymentTerms"));
        List<Map<String, Object>> commissions = list(body.get("commissions"));
        List<Map<String, Object>> otherCharges = list(body.get("otherCharges"));
        boolean allowExportMultiCompanies = asBool(h.get("allowExportMultiCompanies"));
        boolean flagFin = asBool(repo.config(u, "FinancialisActiveonCommercialInvoice"));

        if (detail.isEmpty()) throw new IllegalArgumentException("Grid Record not found");
        /* formvalidation */
        if (allowExportMultiCompanies && asInt(h.get("ExportCompanyId")) == 0) throw new IllegalArgumentException("Export Company Is required");
        if (asInt(h.get("DocCode")) == 0) throw new IllegalArgumentException("Doc No Is Required");
        if (asInt(h.get("CommercialInvoiceId")) == 0) throw new IllegalArgumentException("Invoice No Is Required");
        if (text(h.get("InvoiceNo")).isEmpty()) throw new IllegalArgumentException("Custom Invoice No Is Required");
        if (asInt(h.get("SupplierCustomerId")) == 0) throw new IllegalArgumentException("Customer Is Required");
        if (flagFin && asInt(h.get("CreditAccountId")) == 0 && !asBool(h.get("creditToSaleConfirmed"))) throw new IllegalArgumentException("Please select CreditAc");
        if (asInt(h.get("DeliveryTermId")) == 0) throw new IllegalArgumentException("Delivery Term Is Required");
        if (asInt(h.get("LoadingPortId")) == 0) throw new IllegalArgumentException("Loading Port Is Required");
        if (asInt(h.get("DestinationPortId")) == 0) throw new IllegalArgumentException("Destination Port  Is Required");
        if (asInt(h.get("CarierTypeId")) == 0) throw new IllegalArgumentException("Carrier Type  Is Required");
        if (asInt(h.get("FcurrencyId")) == 0) throw new IllegalArgumentException("Fcy Code Is Required");
        if (asDouble(h.get("FCurrencyAmount")) == 0) throw new IllegalArgumentException("Fcy Amount  Is Required");
        if (asInt(h.get("NoOfContainers")) == 0) throw new IllegalArgumentException("No.of Containers Field Is Required");
        double netWeight = asDouble(h.get("NetMton"));
        if (!(netWeight > 0)) throw new IllegalArgumentException("Net Weight  Is Required");
        if (!(asDouble(h.get("GrossMton")) >= netWeight)) throw new IllegalArgumentException("Gross Weight Must Be Equal Or Greater Than Net Weight Thank You");
        double balKg = asDouble(h.get("InvoiceBalWeight")) / 1000.0;
        if (netWeight > balKg) throw new IllegalArgumentException("Net MTon:" + num(netWeight) + " must be equal to or Less than Balance MTon of Selected Invoice :" + num(balKg) + ". Thank you.");
        if (!(asDouble(h.get("ConversionRate")) > 0)) throw new IllegalArgumentException("ExchangeRate Field is Required");
        if (!(asDouble(h.get("TotalAmount")) > 0)) throw new IllegalArgumentException("Total Amount  Is Required");

        Timestamp now = new Timestamp(System.currentTimeMillis());
        Map<String, Object> pih = header(u, recId, h, now);
        pih.put("FinancialYearId", fy());
        double totalFc = 0, otherAmount = 0, utilize = 0;
        BigDecimal netMton = BigDecimal.ZERO; double netWeightKg = 0;
        double mtonPerContainer = netWeight / asDouble(h.get("NoOfContainers"));

        List<Map<String, Object>> packing = new ArrayList<>();
        if (recId > 0) for (Map<String, Object> r : removed) { Map<String, Object> d = packingModel(r); d.put("ActionTypeId", 3); packing.add(d); }
        for (Map<String, Object> r : detail) {
            Map<String, Object> d = packingModel(r);
            d.put("ActionTypeId", asInt(r.get("Id")) <= 0 ? 1 : 2);
            totalFc += asDouble(d.get("FcAmount"));
            netWeightKg += asDouble(d.get("NetWeight"));
            netMton = netMton.add(BigDecimal.valueOf(asDouble(d.get("MTon"))));
            d.put("NoofContainers", asDouble(d.get("MTon")) / mtonPerContainer);
            packing.add(d);
        }
        pih.put("NetWeight", netWeightKg);
        pih.put("NetMton", netMton);

        List<Map<String, Object>> oi = new ArrayList<>();
        for (Map<String, Object> r : otherItems) {
            if (!(dec(r.get("Qty")).signum() > 0)) continue;
            Map<String, Object> d = new LinkedHashMap<>();
            d.put("Id", 0);
            d.put("ExImInvoiceId", 0);
            d.put("otherItemId", asInt(r.get("ItemId")));
            d.put("oItemQty", dec(r.get("Qty")));
            d.put("oItemWeightKgs", BigDecimal.ZERO);
            d.put("oItemRate", asDouble(r.get("Rate")));
            d.put("oItemAmount", dec(r.get("Amount")));
            otherAmount += asDouble(r.get("Amount"));
            d.put("OtherItemRemarks", text(r.get("Remarks")));
            oi.add(d);
        }
        if (paymentTerms.isEmpty()) throw new IllegalArgumentException("Payment Detail Not Found. Please Check! ");
        List<Map<String, Object>> pt = new ArrayList<>();
        int sort = 0;
        for (Map<String, Object> r : paymentTerms) {
            Map<String, Object> d = new LinkedHashMap<>();
            d.put("Id", 0);
            d.put("ExImInvoiceId", 0);
            d.put("DocumentTypeId", asInt(r.get("DocumentTypeId")));
            d.put("ExImEFormRegistrationId", asInt(r.get("ExImEFormRegistrationId")));
            d.put("PaymentTermId", asInt(r.get("PaymentTermId")));
            d.put("RefDocumentTypeId", DOCUMENT_TYPE_ID);
            if (asInt(r.get("PaymentTermId")) == 1 && asInt(r.get("ExImEFormRegistrationId")) == 0)
                throw new IllegalArgumentException("Financial Instrument number field is required against advance in payment term detail...");
            d.put("FinancialInstrumentNo", text(r.get("FinancialInstrumentNo")));
            d.put("PrcntOfTotal", asDouble(r.get("PrcntOfTotal")));
            d.put("DueDays", asInt(r.get("DueDays")));
            d.put("FcyAmount", asDouble(r.get("FcyAmount")));
            d.put("PaymentRemarks", text(r.get("Remarks")));
            utilize += asDouble(r.get("FcyAmount"));
            d.put("SortNo", ++sort);
            d.put("EntryUserId", u.getId());
            d.put("ModifyUserId", 0);
            pt.add(d);
        }
        pih.put("PaymentTermId", asInt(paymentTerms.get(0).get("PaymentTermId")));
        List<Map<String, Object>> cm = new ArrayList<>();
        for (Map<String, Object> r : commissions) {
            Map<String, Object> d = new LinkedHashMap<>();
            d.put("exImInvoiceSaleManCommissionId", 0);
            d.put("exImInvoiceId", 0);
            d.put("SalesPersonId", asInt(r.get("SalesPersonId")));
            d.put("commissionTypeId", asInt(r.get("CommissionTypeId")));
            d.put("ActionTypeId", 1);
            d.put("commissionRate", asDouble(r.get("Rate")));
            d.put("rateUom", text(r.get("RateUom")));
            d.put("fcyAmount", asDouble(r.get("FcyAmount")));
            d.put("exchangeRate", asDouble(h.get("ConversionRate")));
            d.put("lcyAmount", asDouble(r.get("LcyAmount")));
            d.put("Remarks", text(r.get("Remarks")));
            cm.add(d);
        }
        List<Map<String, Object>> oc = new ArrayList<>();
        int i = 0;
        for (Map<String, Object> r : otherCharges) {
            Map<String, Object> d = new LinkedHashMap<>();
            d.put("Id", asInt(r.get("Id")));
            d.put("ExImInvoiceId", 0);
            d.put("ChargesItemId", asInt(r.get("ChargesItemId")));
            d.put("AddAmount", asDouble(r.get("AddAmount")));
            d.put("LessAmount", asDouble(r.get("LessAmount")));
            d.put("Remarks", text(r.get("Remarks")));
            d.put("ContractId", asInt(r.get("ContractId")));
            d.put("ContractOtherChargesDetailId", asInt(r.get("ContractOtherChargesDetailId")));
            validateField(asInt(d.get("ChargesItemId")), "Charges Item", i, "Other Charges");
            if (asDouble(d.get("AddAmount")) == 0 && asDouble(d.get("LessAmount")) == 0)
                throw new IllegalArgumentException("AddAmount Or Less Amount is required in Other Charges at row No: " + (i + 1));
            oc.add(d);
            i++;
        }
        double totalAmount = totalFc + asDouble(h.get("AddLessAmount")) + otherAmount;
        pih.put("TotalAmount", totalAmount);
        if (Math.abs(Math.round(totalAmount) - Math.round(utilize)) >= 1)
            throw new IllegalArgumentException("Total Invoice Amount and Total Payment Utilize Amount Not equal Please Check");

        int id = repo.save(pih, packing, pt, oc, oi, cm);
        return ok(id, recId == 0 ? "Save SuccessFully" : "Update SuccessFully");
    }

    /** ExImInvoice non-virtual properties in the desktop's assignment (the rest default: 0 / null / false). */
    private static Map<String, Object> header(UserAccount u, int recId, Map<String, Object> h, Timestamp now) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("Id", recId > 0 ? recId : 0);
        p.put("SupplierCustomerId", asInt(h.get("SupplierCustomerId")));
        p.put("DocCode", asInt(h.get("DocCode")));
        p.put("DocDate", ts(h.get("DocDate")));
        p.put("DocumentTypeId", DOCUMENT_TYPE_ID);
        p.put("InvoiceNo", text(h.get("InvoiceNo")));
        p.put("SupplierCode", null);
        p.put("LotNoRef", text(h.get("LotNoRef")));
        p.put("CarierType", text(h.get("CarierType")));
        p.put("LoadingPortId", asInt(h.get("LoadingPortId")));
        p.put("DestinationPortId", asInt(h.get("DestinationPortId")));
        BigDecimal grossMton = dec(h.get("GrossMton"));
        p.put("GrossWeight", grossMton.multiply(BigDecimal.valueOf(1000)).doubleValue());
        p.put("NetWeight", 0d);
        p.put("NoOfContainers", (double) asInt(h.get("NoOfContainers")));
        p.put("FcurrencyId", asInt(h.get("FcurrencyId")));
        p.put("FCurrencyAmount", asDouble(h.get("FCurrencyAmount")));
        p.put("ConversionRate", asDouble(h.get("ConversionRate")));
        p.put("EquivalentAmount", asDouble(h.get("EquivalentAmount")));
        p.put("InvoiceCommercialValue", 0d);
        p.put("OtherInstructions", null);
        p.put("TermsConditions", null);
        p.put("RemarksHeader", null);
        p.put("InvStatus", null);
        p.put("IsApproved", false);
        p.put("EntryDate", now);
        p.put("EntryUser", u.getId());
        p.put("ModifyDate", now);
        p.put("ModifyUser", u.getId());
        p.put("ApprovedDate", now);
        p.put("ApprovedUser", 0);
        p.put("OrganizationId", u.getOrganizationId());
        p.put("CompanyId", u.getCompanyId());
        p.put("BranchesId", u.getBranchesId() == null ? 0 : u.getBranchesId());
        p.put("ProjectsId", asInt(h.get("ProjectsId")));
        p.put("BuyerGLValueRs", 0d);
        p.put("ImporterBankId", asInt(h.get("ImporterBankId")));
        p.put("ExporteBankId", asInt(h.get("ExporteBankId")));
        p.put("LcOrderNoId", 0);
        p.put("PaymentTermId", 0);
        p.put("DeliveryTermId", asInt(h.get("DeliveryTermId")));
        p.put("EFormId", 0);
        p.put("FobValue", 0d);
        p.put("EFormNo", text(h.get("EFormNo")));
        p.put("NotifyParty1", asInt(h.get("NotifyParty1")));
        p.put("NotifyParty2", asInt(h.get("NotifyParty2")));
        p.put("EFormDate", ts(h.get("EFormDate")));
        p.put("Certificate1", null);
        p.put("Certificate2", null);
        p.put("AddLessAmount", asDouble(h.get("AddLessAmount")));
        p.put("AddLessComments", text(h.get("AddLessComments")));
        p.put("TotalAmount", asDouble(h.get("TotalAmount")));
        p.put("FinancialYearId", asInt(h.get("FinancialYearId")));
        p.put("CreditAccountId", asInt(h.get("CreditAccountId")));
        p.put("CommissionAgentId", 0);
        p.put("CommissionPercentage", 0d);
        p.put("CommissionAmount", 0d);
        p.put("OtherDestinationPortId", 0);
        p.put("OtherCustomerId", 0);
        p.put("CommissionDebitAcId", 0);
        p.put("PreInvoiceId", 0);
        p.put("ConsigneeId", asInt(h.get("ConsigneeId")));
        p.put("ActionId", recId == 0 ? 1 : 2);
        p.put("NetMton", BigDecimal.ZERO);
        p.put("GrossMton", grossMton);
        p.put("PaymentRemarks", text(h.get("PaymentRemarks")));
        p.put("DeliveryRemarks", text(h.get("DeliveryRemarks")));
        p.put("ContractIds", null);
        p.put("ContractDetailIds", null);
        p.put("ContractNos", null);
        p.put("ContractScheduleIds", null);
        p.put("ContractScheduleNos", null);
        p.put("ContractDates", null);
        p.put("PrefixTypeId", 0);
        p.put("ExportCompanyId", asInt(h.get("ExportCompanyId")));
        p.put("CustomerContractNos", text(h.get("CustomerContractNos")));
        p.put("AttachmentsValues", text(h.get("AttachmentsValues")).isEmpty() ? null : text(h.get("AttachmentsValues")));
        p.put("CustomAttachmentsValues", text(h.get("CustomAttachmentsValues")).isEmpty() ? null : text(h.get("CustomAttachmentsValues")));
        p.put("DocumentaryCreditNo", null);
        p.put("DocumentaryCreditNoIssueDate", null);
        p.put("NotifyParty3", 0);
        p.put("TCPRegNo", null);
        p.put("OriginCountryId", 0);
        p.put("ImporterCountryId", 0);
        p.put("PlaceOfDeliveryId", 0);
        p.put("Remarks1", null);
        p.put("Remarks2", null);
        p.put("OtherRemarks1", null);
        p.put("OtherRemarks2", null);
        p.put("ContinentId", 0);
        p.put("CommercialInvoiceId", asInt(h.get("CommercialInvoiceId")));
        p.put("SubContractExpiryDays", 0);
        return p;
    }

    /** ExImInvoicePackingDetail non-virtual properties as btnsave_Click / the Delete button fill them. */
    private static Map<String, Object> packingModel(Map<String, Object> r) {
        Map<String, Object> d = new LinkedHashMap<>();
        d.put("Id", asInt(r.get("Id")));
        d.put("ExImInvoiceId", 0);
        d.put("ItemId", asInt(r.get("ItemId")));
        d.put("ItemUOMId", 0);
        d.put("ItemDescriptionManual", text(r.get("ItemDetail")));
        d.put("BrandId", 0);
        d.put("PackingMaterialTypeId", asInt(r.get("PackTypeId")));
        d.put("InnerQty", (double) asInt(r.get("NoOfBags")));
        d.put("InnerQtyUomId", asInt(r.get("PackSizeId")));
        d.put("OuterQty", asDouble(r.get("NoOfBags")));
        d.put("OuterQtyUomId", asInt(r.get("PackSizeId")));
        d.put("NetWeight", asDouble(r.get("QtyMTon")) * 1000.0);
        d.put("RatePrice", asDouble(r.get("CostMTon")));
        d.put("RateUomId", asInt(r.get("RateUOMId")));
        d.put("OuterPackDescription", text(r.get("PackingDetail")));
        d.put("FcAmount", asDouble(r.get("Amount")));
        d.put("ItemSpicificationId", 0);
        d.put("CropYearId", asInt(r.get("CropYearId")));
        d.put("JobLotId", asInt(r.get("JobLotId")));
        d.put("ContractDetailId", 0);
        d.put("ContractId", 0);
        d.put("PackingWeight", 0d);
        d.put("TotalPackingWeight", 0d);
        d.put("GrossWeight", 0d);
        d.put("PackingExpiryDate", null);
        d.put("ItemCommodityDetail", null);
        d.put("HealthPermitNoDetail", null);
        d.put("MTon", asDouble(r.get("QtyMTon")));
        d.put("NoofContainers", 0d);
        d.put("NoofBagsPerContainer", 0d);
        d.put("OtherRate", 0d);
        d.put("OtherAmount", asDouble(r.get("OtherItemAmount")));
        d.put("HsCode", text(r.get("HSCode")));
        d.put("OtherHsCode", null);
        d.put("PreInvoiceId", 0);
        d.put("PreInvoiceDetailId", 0);
        d.put("ProductionNo", null);
        d.put("LineId", 0);
        d.put("ContractScheduleId", 0);
        d.put("ActionTypeId", 0);
        d.put("ContractNoShipmentWise", null);
        d.put("AmountCalulationId", 0);
        d.put("RateAddLess", 0d);
        d.put("RateWithoutAddLess", 0d);
        d.put("ExImTradeTypeId", asInt(r.get("ExImTradeTypeId")));
        d.put("ExImFarmingTypeId", asInt(r.get("ExImFarmingTypeId")));
        d.put("ExImFarmingNTradeId", asInt(r.get("ExImFarmingNTradeId")));
        d.put("ContractScheduleDetailId", 0);
        d.put("PalletQty", 0d);
        d.put("LabInspectionId", 0);
        d.put("BuyerLotNo", null);
        d.put("ProductCode", null);
        d.put("PmItemId", 0);
        d.put("Remarks", null);
        d.put("BuyerHSCode", null);
        d.put("AddLessAmount", asDouble(r.get("AddLessAmount")));
        return d;
    }

    private static String num(double v) {
        if (v == Math.rint(v) && Math.abs(v) < 1e15) return String.valueOf((long) v);
        return String.valueOf(v);
    }
}
