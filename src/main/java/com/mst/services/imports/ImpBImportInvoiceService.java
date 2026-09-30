package com.mst.services.imports;

import com.mst.models.UserAccount;
import com.mst.models.imports.ImpBInvoiceDetail;
import com.mst.models.imports.ImpBInvoiceMaster;
import com.mst.models.imports.ImpBInvoiceOtherItem;
import com.mst.models.imports.ImpBInvoicePaymentTerm;
import com.mst.repositories.imports.ImpBRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.HashSet;
import java.util.List;
import java.util.Map;
import java.util.Set;

import static com.mst.services.hrm.HrmSupport.*;
import static com.mst.services.imports.ImpBSupport.*;

/**
 * 783 Import Invoice - Architecture.WinApp.Import.frmImportInvoice (ScreenName frmImportInvoice, DocumentTypeId 902),
 * BLL Architecture.BLL.Import.invoiceMaster, DAL Architecture.DAL.Import.invoiceMasterProvider (Set / GetAll).
 * Each method names the form method it reproduces; messages are the form's texts.
 */
@Service
public class ImpBImportInvoiceService {

    public static final int SCREEN = 783;
    public static final String SCREEN_NAME = "frmImportInvoice";
    public static final int DOC_TYPE = 902;

    @Autowired private ImpBRepository repo;
    @Autowired private ImpBSupport sup;
    @Autowired private ImpBProformaService proforma;

    // ------------------------------------------------------------------ combos

    /** BindInvoiceAndLcOrderNo(): masterDocumentSerial.ComboForInvoice(Org, Company, RecId) -> dtInvoiceAndLcNo. */
    private List<Map<String, Object>> invoiceAndLc(UserAccount u, long recId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.serialComboForInvoice(u, recId)) {
            out.add(map("InvoiceId", r.get("invoiceMasterId"), "InvoiceNo", str(r.get("invoiceNo")), "InvoiceDate", r.get("invoiceDate"),
                    "ContractId", r.get("LocOrderMasterId"), "ContractNo", str(r.get("locOrderNo")), "ContractDate", r.get("locOrderDate")));
        }
        return out;
    }

    /** ImporterandExportBankFill: Bank.GetAll split on IsHomeland ("Home Country" -> importer, "Foreign Country" -> exporter). */
    private Map<String, Object> banks(UserAccount u) {
        List<Map<String, Object>> imp = new ArrayList<>(), exp = new ArrayList<>();
        for (Map<String, Object> r : repo.banks(u)) {
            String h = str(r.get("IsHomeland"));
            if ("Home Country".equals(h)) imp.add(map("Id", r.get("Id"), "BranchName", str(r.get("BranchName"))));
            if ("Foreign Country".equals(h)) exp.add(map("Id", r.get("Id"), "BranchName", str(r.get("BranchName"))));
        }
        return map("importerBanks", imp, "exporterBanks", exp);
    }

    private Map<String, Object> combos(UserAccount u, long recId, boolean multiCompanies) {
        Map<String, Object> m = map("branches", sup.branchesOfUser(), "proformaNos", proforma.proformaNos(u, recId, 1),
                "invoiceAndLc", invoiceAndLc(u, recId), "suppliers", pick(repo.suppliersForExport(u), "Id", "CompanyName"),
                "salePersons", pick(repo.suppliersByGroup(u, "9", 0), "Id", "CompanyName"),
                "currencies", pick(sup.currencies(), "Id", "CurrencyName"), "items", proforma.items(u),
                "jobLots", pick(sup.jobLots(), "Id", "JobLotDescription"),
                "debitAccounts", pick(sup.accountsByTypes("3,6,8,10"), "Id", "AccountTitle"),
                "otherItems", pick(repo.itemsByType(u, "14"), "Id", "ItemName"),
                "exportCompanies", multiCompanies ? pick(repo.exportCompanies(u), "Id", "CompName") : new ArrayList<>());
        m.putAll(proforma.importRelated(u));
        m.putAll(banks(u));
        return m;
    }

    /** InitializeComponentMethod + ImProformaInvoice_Load (HistoryCombosFill). */
    public Map<String, Object> setup() {
        UserAccount u = sup.user(SCREEN);
        boolean multi = sup.feature(9);
        Map<String, Object> m = map("rights", sup.formRights(u, SCREEN, SCREEN_NAME), "historyDays", sup.historyDays(),
                "financialActive", toBool(sup.config("FinancialisActiveonCommercialInvoice")), "multiCompanies", multi,
                "branchFeature", sup.feature(11), "userBranchId", branchId(u), "docNo", docNo(u),
                "historyExporters", historyExporters(u));
        m.putAll(combos(u, 0, multi));
        return m;
    }

    /** btnRefresh_Click (+ ExImLcOrder.GetExportCompaniesByCompany when feature 9 is on). */
    public Map<String, Object> refresh(long recId) {
        UserAccount u = sup.user(SCREEN);
        return combos(u, Math.max(0, recId), sup.feature(9));
    }

    private int docNo(UserAccount u) { return repo.invoiceDocNo(u, sup.financialYearId(), DOC_TYPE); }

    /** FormReset(): generateCode(). (BindProformaNo / BindInvoiceAndLcOrderNo results are discarded there by the form.) */
    public Map<String, Object> reset() { return map("docNo", docNo(sup.user(SCREEN))); }

    public List<Map<String, Object>> uoms(int itemId) { return proforma.uomRows(sup.user(SCREEN), itemId); }

    /**
     * cmbSupCust_Leave: BindFinancialInstrument (ExImEFormRegistration.GetFINoForInvoice for the exporter -> {Id, EFormNo,
     * PaymenttermId, DocumnetTypeId}) and bindConsigneeAgainstCustomer (GetSupplierustomerByCustomerGroupId(null, exporter)).
     */
    public Map<String, Object> byExporter(int exporterId) {
        UserAccount u = sup.user(SCREEN);
        List<Map<String, Object>> fi = new ArrayList<>(), cons = new ArrayList<>();
        if (exporterId > 0) {
            for (Map<String, Object> r : repo.financialInstruments(u, exporterId))
                fi.add(map("Id", r.get("Id"), "EFormNo", str(r.get("EFormNo")), "PaymenttermId", r.get("PaymenttermId"), "DocumnetTypeId", r.get("DocumentTypeId")));
            cons = pick(repo.suppliersByGroup(u, null, exporterId), "Id", "CompanyName");
        }
        return map("financialInstruments", fi, "consignees", cons);
    }

    /** GetFinancialInstrumentsBalance(): Sp_ExImEFormRegistration_GetAllMethod rows[0][0] (0 when no row). */
    public Map<String, Object> fiBalance(int documentTypeId, int id) {
        UserAccount u = sup.user(SCREEN);
        List<Map<String, Object>> r = repo.financialInstrumentBalance(u, documentTypeId, id);
        Object v = r.isEmpty() || r.get(0).isEmpty() ? null : r.get(0).values().iterator().next();
        return map("balance", toDec(v));
    }

    /** HistoryCombosFill(): USP_GetDataForDropDownFrom_Invoice rows with Activity "ExporterName". */
    private List<Map<String, Object>> historyExporters(UserAccount u) { return activity(repo.invoiceDropDown(u), "ExporterName"); }

    public List<Map<String, Object>> historyCombos() { return historyExporters(sup.user(SCREEN)); }

    // ------------------------------------------------------------------ ReadById

    private Map<String, Object> own(UserAccount u, long id) {
        List<Map<String, Object>> r = repo.invoice(id);
        if (r.isEmpty()) return null;
        Map<String, Object> m = r.get(0);
        if (toInt(m.get("OrganizationId")) != toInt(u.getOrganizationId()) || toInt(m.get("CompanyId")) != toInt(u.getCompanyId())
                || toInt(m.get("documentTypeId")) != DOC_TYPE) throw invalid("Record not found.");
        return m;
    }

    private List<Map<String, Object>> details(long id) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> d : repo.invoiceChildren("usp_Get_invoiceDetail", id)) {
            double nw = toDouble(d.get("netWeightOuter"));
            out.add(map("Id", d.get("invoiceDetailId"), "ItemId", toInt(d.get("ItemId")), "ItemName", str(d.get("ItemName")),
                    "ItemCode", str(d.get("ItemCode")), "ItemDetail", str(d.get("ItemDescription")), "JobLotId", toInt(d.get("lotJobId")),
                    "JobLot", str(d.get("JobLotDescription")), "QtyMTon", nw / 1000.0, "PackSizeId", toInt(d.get("packSizeIdOuter")),
                    "PackSize", str(d.get("OuterUOM")), "PackEquivalent", toDouble(d.get("OuterEquivalent")), "NoOfBags", toDouble(d.get("qtyOuter")),
                    "NetWeight", nw, "CostMTon", toDouble(d.get("ItemRate")), "RateUOMId", toInt(d.get("uomIdRate")), "RateUOM", str(d.get("RateUOM")),
                    "RateEquivalent", toDouble(d.get("RateEquivalent")), "Amount", toDouble(d.get("FcAmount")), "Remarks", str(d.get("RemarksDetail")),
                    "OtherItemAmount", 0, "RowVersionLong", 0,                                   // ReadById shifts RowVersionLong into OtherItemAmount
                    "locationBranchId", toInt(d.get("locationBranchId")), "LocOrderMasterId", toInt(d.get("LocOrderMasterId")),
                    "locOrderDate", d.get("locOrderDate")));
        }
        return out;
    }

    /** ReadById(Id): FormReset, invoiceMaster.ReadById (header + detail + payment terms + other items), attachments. */
    public Map<String, Object> byId(long id) {
        UserAccount u = sup.user(SCREEN);
        Map<String, Object> h = own(u, id);
        if (h == null) throw invalid("Record not found.");
        List<Map<String, Object>> other = new ArrayList<>(), pay = new ArrayList<>();
        for (Map<String, Object> o : repo.invoiceChildren("usp_Get_invoiceOtherItem", id))
            other.add(map("Id", o.get("invoiceOtherItemId"), "ItemId", toInt(o.get("ItemId")), "ItemName", str(o.get("ItemName")),
                    "Qty", toDouble(o.get("ItemQty")), "Rate", toDouble(o.get("ItemRate")), "Amount", toDouble(o.get("ItemAmount")),
                    "Remarks", str(o.get("Remarks")), "RowVersionLong", toLong(o.get("RowVersionLong"))));
        for (Map<String, Object> p : repo.invoiceChildren("usp_Get_InvoicePaymentTerm", id))
            pay.add(map("Id", p.get("InvoicePaymentTermId"), "PaymentTermId", toInt(p.get("paymentTermId")), "PaymentTerm", str(p.get("PaymentTerm")),
                    "DocumentTypeId", toInt(p.get("DocumentTypeId")), "ExImEFormRegistrationId", toInt(p.get("FinancialInstrumentId")),
                    "FinancialInstrumentNo", str(p.get("FinancialInstrumentNo")), "PctOfTotal", toDouble(p.get("pctOfTotal")),
                    "FcyAmount", toDouble(p.get("FcyAmount")), "DueDays", str(p.get("DueDays")), "Remarks", str(p.get("Remarks")),
                    "RowVersionLong", toLong(p.get("RowVersionLong"))));
        long exporter = toLong(h.get("exporterId"));
        Map<String, Object> dep = byExporter((int) exporter);
        return map("header", map("invoiceMasterId", h.get("invoiceMasterId"), "docNo", toInt(h.get("docNo")), "importerId", toLong(h.get("importerId")),
                        "exporterId", exporter, "salePersonId", toInt(h.get("salePersonId")), "ConsigneeId", toLong(h.get("ConsigneeId")),
                        "proformaMasterId", toLong(h.get("proformaMasterId")), "proformaMasterDate", h.get("proformaMasterDate"),
                        "invoiceMasterNo", str(h.get("invoiceMasterNo")), "docDate", h.get("docDate"), "importerRefNo", str(h.get("importerRefNo")),
                        "incoTermId", toInt(h.get("incoTermId")), "payementTermId", toInt(h.get("payementTermId")), "currencyId", toInt(h.get("currencyId")),
                        "exchangeRate", toDec(h.get("exchangeRate")), "fcyAmountTotal", toDec(h.get("fcyAmountTotal")),
                        "NoOfPackages", toDec(h.get("NoOfPackages")), "fclTotal", toInt(h.get("fclTotal")),
                        "grossWeightTotal", toDouble(h.get("grossWeightTotal")) / 1000.0, "netWeightTotal", toDouble(h.get("netWeightTotal")) / 1000.0,
                        "bankIdImporter", toInt(h.get("bankIdImporter")), "bankIdExporter", toInt(h.get("bankIdExporter")),
                        "loadingPortId", toInt(h.get("loadingPortId")), "destinationPortId", toInt(h.get("destinationPortId")),
                        "BillOfLadingNo", str(h.get("BillOfLadingNo")), "BillOfLadingDate", h.get("BillOfLadingDate"),
                        "GoodsDeclarationNo", str(h.get("GoodsDeclarationNo")), "GoodsDeclarationDate", h.get("GoodsDeclarationDate"),
                        "GoodsDeclarationValue", toDec(h.get("GoodsDeclarationValue")), "DabitAccountId", toInt(h.get("DabitAccountId")),
                        "legalEntityId", toInt(h.get("legalEntityId"))),
                "details", details(id), "otherItems", other, "paymentTerms", pay, "proformaNos", proforma.proformaNos(u, id, 1),
                "invoiceAndLc", invoiceAndLc(u, id), "financialInstruments", dep.get("financialInstruments"), "consignees", dep.get("consignees"),
                "attachments", sup.attachments(u, SCREEN_NAME, id));
    }

    public List<Map<String, Object>> historyDetail(long id) {
        UserAccount u = sup.user(SCREEN);
        if (own(u, id) == null) throw invalid("Record not found.");
        return details(id);
    }

    public List<Map<String, Object>> attachments(long id) {
        UserAccount u = sup.user(SCREEN);
        if (own(u, id) == null) throw invalid("Record not found.");
        return sup.attachments(u, SCREEN_NAME, id);
    }

    public ImpBSupport.DesktopAttachmentStoreFile attachmentFile(long id, int attachmentId) {
        UserAccount u = sup.user(SCREEN);
        if (own(u, id) == null) throw invalid("Record not found.");
        return sup.file(u, SCREEN_NAME, id, attachmentId);
    }

    // ------------------------------------------------------------------ History

    /**
     * HistoryGridFill() -> invoiceMaster.SEARCHHistory: @OrganizationId, @CompanyId, @DocumentTypeId 902, @CanViewAllRecord;
     * the radio's date pair when ticked; @DocNoFrom / @DocNoTo when != 0; @createdUserId when the user cannot view all.
     * The Importer Name combo goes to SupplierCustomerId, which this BLL never reads - it does not filter (desktop defect).
     */
    public List<Map<String, Object>> history(Map<String, Object> f) {
        UserAccount u = sup.user(SCREEN);
        boolean all = sup.canViewAllRecord(SCREEN_NAME);
        Map<String, Object> p = map("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "DocumentTypeId", DOC_TYPE, "CanViewAllRecord", all);
        ImpBPackingDetailService.historyDates(p, f);
        int fromNo = toInt(f.get("fromDocNo")), toNo = toInt(f.get("toDocNo"));
        if (fromNo != 0) p.put("DocNoFrom", fromNo);
        if (toNo != 0) p.put("DocNoTo", toNo);
        if (!all) p.put("createdUserId", u.getId());
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.invoiceSearch(p)) {
            out.add(map("Id", r.get("InvoiceMasterId"), "DocNo", r.get("DocNo"), "InvoiceDate", r.get("DocDate"), "InvoiceNo", r.get("InvoiceMasterNo"),
                    "ProformaMasterId", r.get("ProformaMasterId"), "ProformaNo", r.get("ProformaMasterNo"), "ProformaDate", r.get("ProformaMasterDate"),
                    "LcOrderMasterId", r.get("LcOrderMasterId"), "LcOrderNo", r.get("LcOrderMasterNo"), "LcOrderDate", r.get("LcOrderMasterDate"),
                    "ExchangeRate", r.get("ExchangeRate"), "FcyAmountTotal", r.get("FcyAmountTotal"), "NoOfPackages", r.get("NoOfPackages"),
                    "FclTotal", r.get("FclTotal"), "GrossWeightTotal", r.get("GrossWeightTotal"), "NetWeightTotal", r.get("NetWeightTotal"),
                    "BLNo", r.get("BillOfLadingNo"), "BLDate", r.get("BillOfLadingDate"), "GDNo", r.get("GoodsDeclarationNo"),
                    "GDDate", r.get("GoodsDeclarationDate"), "GDValue", r.get("GoodsDeclarationValue"), "DocStatus", r.get("DocStatus"),
                    "RemarksHeader", r.get("RemarksHeader"), "EntryDate", r.get("CreatedOn"), "EntryUser", r.get("CreatedUserName"),
                    "ModifyDate", r.get("LastModifiedOn"), "ModifyUser", r.get("LastModifiedUserName"), "IsApproved", toBool(r.get("IsApproved")),
                    "ApprovedDate", r.get("ApprovedOn"), "ApprovedUser", r.get("ApprovedUserName"), "NoOfAttachments", r.get("NoOfAttachments"),
                    "RowVersionLong", r.get("RowVersionLong")));
        }
        return out;
    }

    // ------------------------------------------------------------------ Insert()

    /**
     * btnsave_Click (RecId = 0) / btnupdate_Click ("Rec Id not found..." when RecId = 0) -> Insert(): formvalidation() in the
     * form's order, the grid checks, the header exactly as the form fills it (incl. its quirks - see the report), removed rows
     * (appActionId 3) then grid rows for detail / other items / payment terms, the Σ payment = invoice amount check, and
     * invoiceMaster.Save (@Activity INSERT when AppActionId = 0 else UPDATE; children by appActionId) with the attachment
     * block, all in one transaction.
     */
    public Map<String, Object> save(Map<String, Object> b) {
        UserAccount u = sup.user(SCREEN);
        boolean update = toBool(b.get("update"));
        long recId = update ? toLong(b.get("recId")) : 0;
        if (update && recId == 0) throw invalid("Rec Id not found...");
        sup.require(u, SCREEN, update ? "Update" : "Save");
        Map<String, Object> old = recId > 0 ? own(u, recId) : null;
        if (recId > 0 && old == null) throw invalid("Record not found.");
        Map<String, Object> h = obj(b.get("header"));
        List<Map<String, Object>> rows = list(b.get("rows")), others = list(b.get("otherItems")), pays = list(b.get("paymentTerms"));
        boolean financialActive = toBool(sup.config("FinancialisActiveonCommercialInvoice"));
        boolean multi = sup.feature(9);

        if (sup.feature(11) && !has(sup.branchesOfUser(), "BranchId", toInt(h.get("branchId")))) throw invalid("BranchName Is required");
        if (str(h.get("docNo")).isEmpty() || toInt(h.get("docNo")) == 0) throw invalid("Doc No Is Required");
        String status = str(h.get("docStatus"));
        if ((update && status.isEmpty()) || status.equals("0")) throw invalid("Status Is Required");
        if (toLong(h.get("importerId")) == 0) throw invalid("Importer Is Required");
        if (toLong(h.get("exporterId")) == 0) throw invalid("Exporter Is Required");
        long invoiceId = toLong(h.get("invoiceMasterId"));
        Map<String, Object> serial = null;
        for (Map<String, Object> s : invoiceAndLc(u, recId)) if (toLong(s.get("InvoiceId")) == invoiceId) serial = s;
        boolean keepOwn = old != null && toLong(old.get("invoiceMasterId")) == invoiceId;
        if (toLong(h.get("contractId")) == 0 && !keepOwn) throw invalid("ContractNo No Is Required");
        if (invoiceId == 0 || (serial == null && !keepOwn)) throw invalid("Invoice No Is Required");
        if (toLong(h.get("ConsigneeId")) == 0) throw invalid("Consignee Is Required");
        if (toInt(h.get("payementTermId")) == 0) throw invalid("Payment Term Is Required");
        if (toInt(h.get("loadingPortId")) == 0) throw invalid("Loading Port Is Required");
        if (toInt(h.get("destinationPortId")) == 0) throw invalid("Destination Port  Is Required");
        if (toInt(h.get("currencyId")) == 0) throw invalid("Fcy Code Is Required");
        double fcy = toDouble(h.get("fcyAmountTotal"));
        if (fcy == 0.0) throw invalid("Fcy Amount  Is Required");
        if (toInt(h.get("fclTotal")) == 0) throw invalid("No.of Containers Field Is Required");
        double net = toDouble(h.get("netWeightTotal"));
        if (!(net > 0.0)) throw invalid("Net Weight  Is Required");
        if (!(toDouble(h.get("grossWeightTotal")) >= net)) throw invalid("Gross Weight Must Be Equal Or Greater Than Net Weight Thank You");
        if (!(toDouble(h.get("exchangeRate")) > 0.0)) throw invalid("ExchangeRate Field is Required");
        if (financialActive && toInt(h.get("DabitAccountId")) == 0 && !toBool(h.get("debitConfirmed"))) throw invalid("Please select Debit Ac");
        if (multi && toInt(h.get("legalEntityId")) == 0) throw invalid("Export Company Is required");
        if (rows.isEmpty()) throw invalid("Detail Grid Record Not Found. Please Check! ");
        if (pays.isEmpty()) throw invalid("Payment Detail Not Found. Please Check! ");
        long pmId = toLong(h.get("proformaMasterId"));
        if (pmId != 0 && !has(proforma.proformaNos(u, recId, 1), "Id", pmId) && !(old != null && toLong(old.get("proformaMasterId")) == pmId))
            throw invalid("Record not found.");

        Set<Long> dIds = new HashSet<>(), oIds = new HashSet<>(), pIds = new HashSet<>();
        if (recId > 0) {
            for (Map<String, Object> r : repo.invoiceChildren("usp_Get_invoiceDetail", recId)) dIds.add(toLong(r.get("invoiceDetailId")));
            for (Map<String, Object> r : repo.invoiceChildren("usp_Get_invoiceOtherItem", recId)) oIds.add(toLong(r.get("invoiceOtherItemId")));
            for (Map<String, Object> r : repo.invoiceChildren("usp_Get_InvoicePaymentTerm", recId)) pIds.add(toLong(r.get("InvoicePaymentTermId")));
        }
        LocalDateTime now = LocalDateTime.now();
        ImpBInvoiceMaster m = new ImpBInvoiceMaster();
        m.OrganizationId = u.getOrganizationId();
        m.CompanyId = u.getCompanyId();
        m.BranchesId = branchId(u);
        m.FinancialYearId = sup.financialYearId();
        m.ProjectsId = branchId(u);
        m.documentTypeId = DOC_TYPE;
        m.AppActionId = old == null ? 0 : toInt(old.get("AppActionId"));
        m.RowVersionLong = old == null ? 0 : toLong(old.get("RowVersionLong"));
        m.isApproved = false;
        m.createdOn = now; m.lastModifiedOn = now; m.approvedOn = now;
        m.createdUserId = u.getId(); m.lastModifiedUserId = u.getId();
        m.docNo = toInt(h.get("docNo"));
        m.docStatus = status;
        m.importerId = toLong(h.get("importerId"));
        m.exporterId = toLong(h.get("exporterId"));
        m.salePersonId = toInt(h.get("salePersonId"));
        m.ConsigneeId = toLong(h.get("ConsigneeId"));
        m.proformaMasterId = pmId;
        LocalDateTime pDate = ImpBProformaService.nvl(toDate(h.get("proformaMasterDate")), now);
        m.proformaMasterDate = pDate;
        m.lcOrderMasterDate = pDate;                                  // the form writes txtPerformaDate here too
        m.lcOrderMasterId = invoiceId;                                 // the form writes cmbInvoiceNo.Value here
        m.invoiceMasterNo = serial != null ? str(serial.get("InvoiceNo")).trim() : str(old.get("invoiceMasterNo")).trim();
        m.invoiceMasterId = invoiceId;
        m.docDate = ImpBProformaService.nvl(toDate(h.get("docDate")), now);
        m.importerRefNo = str(h.get("importerRefNo"));
        m.incoTermId = toInt(h.get("incoTermId"));
        m.payementTermId = toInt(h.get("payementTermId"));
        m.currencyId = toInt(h.get("currencyId"));
        m.exchangeRate = toDec(h.get("exchangeRate"));
        m.fcyAmountTotal = fcy;
        m.NoOfPackages = toDouble(str(h.get("detailNoOfBags")).trim());   // the form reads txtNoOfBags (the detail entry box)
        m.fclTotal = toInt(str(h.get("fclTotal")).trim());
        m.grossWeightTotal = toDouble(str(h.get("grossWeightTotal")).trim()) * 1000.0;
        m.netWeightTotal = net * 1000.0;
        m.bankIdImporter = toInt(h.get("bankIdImporter"));
        m.bankIdExporter = toInt(h.get("bankIdExporter"));
        m.loadingPortId = toInt(h.get("loadingPortId"));
        m.destinationPortId = toInt(h.get("destinationPortId"));
        m.BillOfLadingDate = ImpBProformaService.nvl(toDate(h.get("BillOfLadingDate")), now);
        m.BillOfLadingNo = str(h.get("BillOfLadingNo"));
        m.GoodsDeclarationDate = ImpBProformaService.nvl(toDate(h.get("GoodsDeclarationDate")), now);
        m.GoodsDeclarationNo = str(h.get("GoodsDeclarationNo"));
        m.GoodsDeclarationValue = toDouble(h.get("GoodsDeclarationValue"));
        if (financialActive) m.DabitAccountId = toInt(h.get("DabitAccountId"));
        m.legalEntityId = toInt(h.get("legalEntityId"));
        m.AttachmentsValues = old == null ? "" : str(old.get("AttachmentsValues"));
        m.CustomAttachmentsValues = old == null ? "" : str(old.get("CustomAttachmentsValues"));
        int contractId = toInt(h.get("contractId"));

        List<ImpBInvoiceDetail> dl = new ArrayList<>();
        for (Map<String, Object> r : list(b.get("removedRows"))) {
            long id = toLong(r.get("Id"));
            if (id <= 0) continue;
            if (!dIds.contains(id)) throw invalid("Record not found.");
            ImpBInvoiceDetail vd = detail(r, u, now, contractId);
            vd.invoiceDetailId = id;
            vd.netWeightInner = toDouble(r.get("QtyMTon")) / 1000.0;            // DeleteDetailGridRow divides
            vd.netWeightOuter = toDouble(r.get("QtyMTon")) / 1000.0;
            vd.NetWeightKgs = 0; vd.RowVersionLong = 0; vd.LocOrderMasterId = 0;
            vd.appActionId = 3;
            dl.add(vd);
        }
        for (Map<String, Object> r : rows) {
            ImpBInvoiceDetail vd = detail(r, u, now, contractId);
            vd.invoiceDetailId = recId == 0 ? 0 : toLong(r.get("Id"));
            if (vd.invoiceDetailId > 0 && !dIds.contains(vd.invoiceDetailId)) throw invalid("Record not found.");
            vd.appActionId = vd.invoiceDetailId <= 0 ? 1 : 2;
            dl.add(vd);
        }
        List<ImpBInvoiceOtherItem> ol = new ArrayList<>();
        for (Map<String, Object> r : list(b.get("removedOtherItems"))) {
            long id = toLong(r.get("Id"));
            if (id <= 0) continue;
            if (!oIds.contains(id)) throw invalid("Record not found.");
            ImpBInvoiceOtherItem vd = other(r, u, now);
            vd.invoiceOtherItemId = id; vd.RowVersionLong = 0; vd.appActionId = 3;
            ol.add(vd);
        }
        for (Map<String, Object> r : others) {
            ImpBInvoiceOtherItem vd = other(r, u, now);
            vd.invoiceOtherItemId = recId == 0 ? 0 : toLong(r.get("Id"));
            if (vd.invoiceOtherItemId > 0 && !oIds.contains(vd.invoiceOtherItemId)) throw invalid("Record not found.");
            vd.appActionId = vd.invoiceOtherItemId <= 0 ? 1 : 2;
            ol.add(vd);
        }
        List<ImpBInvoicePaymentTerm> pl = new ArrayList<>();
        double utilize = 0;
        for (Map<String, Object> r : list(b.get("removedPaymentTerms"))) {
            long id = toLong(r.get("Id"));
            if (id <= 0) continue;
            if (!pIds.contains(id)) throw invalid("Record not found.");
            ImpBInvoicePaymentTerm vd = pay(r, u, now);
            vd.InvoicePaymentTermId = id; vd.RowVersionLong = 0; vd.appActionId = 3;
            vd.SortNo = toInt(r.get("SortNo"));
            pl.add(vd);
        }
        int sort = 0;
        for (Map<String, Object> r : pays) {
            ImpBInvoicePaymentTerm vd = pay(r, u, now);
            vd.InvoicePaymentTermId = recId == 0 ? 0 : toLong(r.get("Id"));
            if (vd.InvoicePaymentTermId > 0 && !pIds.contains(vd.InvoicePaymentTermId)) throw invalid("Record not found.");
            vd.appActionId = vd.InvoicePaymentTermId <= 0 ? 1 : 2;
            vd.SortNo = ++sort;
            utilize += vd.FcyAmount;
            pl.add(vd);
        }
        if (round0(fcy) != round0(utilize)) throw invalid("Total Invoice Amount and Total Payment Utilize Amount Not equal Please Check");
        String activity = m.AppActionId > 0 ? "UPDATE" : "INSERT";          // BLL invoiceMaster.Save

        final ImpBSupport.AttachmentPlan[] plan = new ImpBSupport.AttachmentPlan[1];
        long saved = repo.tx(() -> {
            plan[0] = sup.prepare(u, SCREEN_NAME, recId, b);
            if (plan[0].changed && !plan[0].finalList.isEmpty()) { m.AttachmentsValues = plan[0].values(); m.CustomAttachmentsValues = plan[0].customValues(); }
            int obj2 = repo.set(ImpBRepository.IMEX + "[usp_Set_invoiceMaster]", m, activity);
            m.invoiceMasterId = obj2;
            for (ImpBInvoiceDetail vd : dl) { vd.invoiceMasterId = m.invoiceMasterId; repo.set(ImpBRepository.IMEX + "[usp_Set_invoiceDetail]", vd, act(vd.appActionId, activity)); }
            for (ImpBInvoicePaymentTerm vd : pl) { vd.invoiceMasterId = m.invoiceMasterId; repo.set(ImpBRepository.IMEX + "[usp_Set_InvoicePaymentTerm]", vd, act(vd.appActionId, activity)); }
            for (ImpBInvoiceOtherItem vd : ol) { vd.invoiceMasterId = m.invoiceMasterId; repo.set(ImpBRepository.IMEX + "[usp_Set_invoiceOtherItem]", vd, act(vd.appActionId, activity)); }
            sup.apply(plan[0], u, SCREEN_NAME, obj2, (int) m.exporterId, DOC_TYPE);
            return (long) obj2;
        });
        if (recId > 0) sup.afterUpdate(plan[0], recId, SCREEN_NAME, DOC_TYPE);
        return saved((int) saved, recId == 0 ? "Save SuccessFully" : "Update SuccessFully");
    }

    /** CommonProvider.GetActivity(appActionId, Activity): 1 INSERT, 2 UPDATE, 3 DELETE, otherwise the header's activity. */
    private static String act(int appActionId, String fallback) {
        switch (appActionId) { case 0: return "CUSTOM"; case 1: return "INSERT"; case 2: return "UPDATE"; case 3: return "DELETE"; default: return fallback; }
    }

    private static double round0(double v) { return new BigDecimal(Double.toString(v)).setScale(0, RoundingMode.HALF_EVEN).doubleValue(); }

    private static ImpBInvoiceDetail detail(Map<String, Object> r, UserAccount u, LocalDateTime now, int contractId) {
        ImpBInvoiceDetail vd = new ImpBInvoiceDetail();
        vd.LocOrderMasterId = contractId;
        vd.ItemId = toInt(r.get("ItemId"));
        vd.ItemDescription = str(r.get("ItemDetail"));
        vd.lotJobId = toInt(r.get("JobLotId"));
        vd.packSizeIdinner = toInt(r.get("PackSizeId"));
        vd.packSizeIdOuter = toInt(r.get("PackSizeId"));
        vd.qtyInner = toInt(r.get("NoOfBags"));
        vd.qtyOuter = toInt(r.get("NoOfBags"));
        double q = toDouble(r.get("QtyMTon"));
        vd.netWeightInner = q * 1000.0; vd.netWeightOuter = q * 1000.0; vd.NetWeightKgs = q * 1000.0;
        vd.ItemRate = toDouble(r.get("CostMTon"));
        vd.uomIdRate = toInt(r.get("RateUOMId"));
        vd.FcAmount = toDouble(r.get("Amount"));
        vd.RemarksDetail = str(r.get("Remarks"));
        vd.isApproved = false;
        vd.createdOn = now; vd.lastModifiedOn = now; vd.approvedOn = now;
        vd.createdUserId = u.getId(); vd.lastModifiedUserId = u.getId();
        vd.RowVersionLong = toLong(r.get("RowVersionLong"));
        return vd;
    }

    private static ImpBInvoiceOtherItem other(Map<String, Object> r, UserAccount u, LocalDateTime now) {
        ImpBInvoiceOtherItem vd = new ImpBInvoiceOtherItem();
        vd.ItemId = toInt(r.get("ItemId"));
        vd.ItemQty = toDec(r.get("Qty"));
        vd.ItemRate = toDouble(r.get("Rate"));
        vd.ItemAmount = toDec(r.get("Amount"));
        vd.Remarks = str(r.get("Remarks"));
        vd.isApproved = false;
        vd.createdOn = now; vd.lastModifiedOn = now; vd.approvedOn = now;
        vd.createdUserId = u.getId(); vd.lastModifiedUserId = u.getId();
        vd.RowVersionLong = toLong(r.get("RowVersionLong"));
        return vd;
    }

    private static ImpBInvoicePaymentTerm pay(Map<String, Object> r, UserAccount u, LocalDateTime now) {
        ImpBInvoicePaymentTerm vd = new ImpBInvoicePaymentTerm();
        vd.paymentTermId = toInt(r.get("PaymentTermId"));
        vd.DocumentTypeId = toInt(r.get("DocumentTypeId"));
        vd.FinancialInstrumentId = toInt(r.get("ExImEFormRegistrationId"));
        vd.pctOfTotal = toDouble(r.get("PctOfTotal"));
        vd.DueDays = toInt(r.get("DueDays"));
        vd.FcyAmount = toDouble(r.get("FcyAmount"));
        vd.Remarks = str(r.get("Remarks"));
        vd.isApproved = false;
        vd.createdOn = now; vd.lastModifiedOn = now; vd.approvedOn = now;
        vd.createdUserId = u.getId(); vd.lastModifiedUserId = u.getId();
        vd.RowVersionLong = toLong(r.get("RowVersionLong"));
        return vd;
    }

    /** btnPrint_Click / history Slip / Preview after save -> CommonServices.ImportInvoiceSlip902: not traceable here. */
    public Map<String, Object> print(long id) {
        UserAccount u = sup.user(SCREEN);
        sup.require(u, SCREEN, "Print");
        throw invalid("Print-902 (CommonServices.ImportInvoiceSlip902) is not available on the web: the desktop's CommonServices source is not "
                + "in the workspace, so its .rpt and procedure cannot be traced.");
    }
}
