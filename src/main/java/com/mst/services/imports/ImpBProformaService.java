package com.mst.services.imports;

import com.mst.models.UserAccount;
import com.mst.models.imports.ImpBProformaDetail;
import com.mst.models.imports.ImpBProformaMaster;
import com.mst.repositories.imports.ImpBRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.math.BigDecimal;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.HashSet;
import java.util.List;
import java.util.Map;
import java.util.Set;

import static com.mst.services.hrm.HrmSupport.*;
import static com.mst.services.imports.ImpBSupport.*;

/**
 * 782 Proforma Invoice - Architecture.WinApp.Import.frmProformaInvoice (ScreenName frmProformaInvoice, DocumentTypeId 900),
 * BLL Architecture.BLL.Import.ProformaMaster, DAL Architecture.DAL.Import.ProformaMaster (SetDate / GetDate), and the
 * "Define Proforma No" dialog frmProformaDocumentSerial (BLL masterDocumentSerial). Messages are the forms' texts.
 */
@Service
public class ImpBProformaService {

    public static final int SCREEN = 782;
    public static final String SCREEN_NAME = "frmProformaInvoice";
    public static final int DOC_TYPE = 900;

    @Autowired private ImpBRepository repo;
    @Autowired private ImpBSupport sup;

    // ------------------------------------------------------------------ combos (InitializeComponentMethod)

    /** BindProformaNo(): GetProformaNoFromMasterDocumentSerial(Org, Company, RecId) -> {Id, ProformaNo, ProformaDate}. */
    List<Map<String, Object>> proformaNos(UserAccount u, long recId, int isForInvoice) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.proformaNos(u, recId, isForInvoice))
            out.add(map("Id", r.get("proformaMasterId"), "ProformaNo", str(r.get("proformaNo")), "ProformaDate", r.get("proformaDate")));
        return out;
    }

    /** ImportRelatedComboFill(): IncoTerm / PaymentTermTitle / SeaAirport rows as {Id, Name}. */
    Map<String, Object> importRelated(UserAccount u) {
        List<Map<String, Object>> dt = repo.importRelated(u);
        return map("incoTerms", named(activity(dt, "IncoTerm")), "paymentTerms", named(activity(dt, "PaymentTermTitle")),
                "ports", named(activity(dt, "SeaAirport")));
    }

    static List<Map<String, Object>> named(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) out.add(map("Id", r.get("Id"), "Name", r.get("name")));
        return out;
    }

    /** CommonServices.ItemGetForComboServiceBind(): {Id, ItemName, ItemCategory, ItemCode, InventoryParentCategoriesId}. */
    List<Map<String, Object>> items(UserAccount u) {
        return pick(repo.itemsForCombo(u), "Id", "ItemName", "ItemCategory", "ItemCode");
    }

    List<Map<String, Object>> uomRows(UserAccount u, int itemId) {
        if (itemId <= 0) return new ArrayList<>();
        return pick(repo.uoms(u, itemId), "Id", "UOMCode", "Equivalent", "QtyEquivalent");
    }

    private int docNo(UserAccount u) { return repo.proformaDocNo(u, sup.financialYearId()); }

    /** InitializeComponentMethod + ImProformaInvoice_Load (HistoryCombosFill). */
    public Map<String, Object> setup() {
        UserAccount u = sup.user(SCREEN);
        Map<String, Object> m = map("rights", sup.formRights(u, SCREEN, SCREEN_NAME), "historyDays", sup.historyDays(),
                "branchFeature", sup.feature(11), "docNo", docNo(u), "branches", sup.branchesOfUser(), "userBranchId", branchId(u),
                "proformaNos", proformaNos(u, 0, 0), "suppliers", pick(repo.suppliersForExport(u), "Id", "CompanyName"),
                "currencies", pick(sup.currencies(), "Id", "CurrencyName"), "items", items(u),
                "jobLots", pick(sup.jobLots(), "Id", "JobLotDescription"), "countries", pick(repo.countries(u), "Id", "Description"),
                "historyImporters", historyImporters(u));
        m.putAll(importRelated(u));
        return m;
    }

    /** btnRefresh_Click: branches, Proforma No, importer / exporter, ImportRelated, currency, items, job lots, goods origin. */
    public Map<String, Object> refresh(long recId) {
        UserAccount u = sup.user(SCREEN);
        Map<String, Object> m = map("branches", sup.branchesOfUser(), "proformaNos", proformaNos(u, Math.max(0, recId), 0),
                "suppliers", pick(repo.suppliersForExport(u), "Id", "CompanyName"), "currencies", pick(sup.currencies(), "Id", "CurrencyName"),
                "items", items(u), "jobLots", pick(sup.jobLots(), "Id", "JobLotDescription"), "countries", pick(repo.countries(u), "Id", "Description"));
        m.putAll(importRelated(u));
        return m;
    }

    /** FormReset(): generateCode() + ProformaNoFill(BindProformaNo()). */
    public Map<String, Object> reset() {
        UserAccount u = sup.user(SCREEN);
        return map("docNo", docNo(u), "proformaNos", proformaNos(u, 0, 0));
    }

    public List<Map<String, Object>> uoms(int itemId) { return uomRows(sup.user(SCREEN), itemId); }

    /** HistoryCombosFill(): USP_GetDataForDropDownFrom_ProformaMaster rows with Activity "ImporterName". */
    private List<Map<String, Object>> historyImporters(UserAccount u) { return activity(repo.proformaDropDown(u), "ImporterName"); }

    public List<Map<String, Object>> historyCombos() { return historyImporters(sup.user(SCREEN)); }

    // ------------------------------------------------------------------ ReadById

    Map<String, Object> own(UserAccount u, long id) {
        List<Map<String, Object>> r = repo.proforma(id);
        if (r.isEmpty()) return null;
        Map<String, Object> m = r.get(0);
        if (toInt(m.get("OrganizationId")) != toInt(u.getOrganizationId()) || toInt(m.get("CompanyId")) != toInt(u.getCompanyId()))
            throw invalid("Record not found.");
        return m;
    }

    /** DAL GetDate: the ProformaDetailList rows as the form's dtdetail (Qty/M.Ton = netWeightOuter / 1000). */
    List<Map<String, Object>> details(long id) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> d : repo.proformaDetails(id)) {
            double nw = toDouble(d.get("netWeightOuter"));
            out.add(map("Id", d.get("proformaDetailId"), "ItemId", toInt(d.get("ItemId")), "ItemCode", str(d.get("ItemCode")),
                    "ItemName", str(d.get("ItemName")), "ItemDetail", str(d.get("ItemDescription")), "JobLotId", toInt(d.get("lotJobId")),
                    "JobLot", str(d.get("JobLotDescription")), "QtyMTon", nw / 1000.0, "PackSizeId", toInt(d.get("packSizeIdOuter")),
                    "PackSize", str(d.get("OuterUOM")), "PackEquivalent", toDouble(d.get("OuterEquivalent")), "NoOfBags", toDouble(d.get("qtyOuter")),
                    "NetWeight", nw, "CostMTon", toDouble(d.get("ItemRate")), "RateUOMId", toInt(d.get("uomIdRate")), "RateUOM", str(d.get("RateUOM")),
                    "RateEquivalent", toDouble(d.get("RateEquivalent")), "Amount", toDouble(d.get("FcAmount")), "Remarks", str(d.get("RemarksDetail")),
                    "RowVersionLong", toLong(d.get("RowVersionLong")), "locationBranchId", toInt(d.get("locationBranchId"))));
        }
        return out;
    }

    /** ReadById(Id): ProformaMaster.GetByID (header + detail), ProformaNoFill(BindProformaNo()) for this RecId, attachments. */
    public Map<String, Object> byId(long id) {
        UserAccount u = sup.user(SCREEN);
        Map<String, Object> h = own(u, id);
        if (h == null) throw invalid("Record not found.");
        List<Map<String, Object>> d = details(id);
        return map("header", map("proformaMasterId", h.get("proformaMasterId"), "docNo", toInt(h.get("docNo")), "validityUpto", h.get("validityUpto"),
                        "importerId", toLong(h.get("importerId")), "exporterId", toLong(h.get("exporterId")), "proformaNo", str(h.get("proformaNo")),
                        "docDate", h.get("docDate"), "GoodsOriginId", toInt(h.get("GoodsOriginId")), "loadingPortId", toInt(h.get("loadingPortId")),
                        "destinationPortId", toInt(h.get("destinationPortId")), "incoTermId", toInt(h.get("incoTermId")),
                        "grossWeightTotal", toDec(h.get("grossWeightTotal")), "netWeightTotal", toDec(h.get("netWeightTotal")),
                        "fclTotal", toDec(h.get("fclTotal")), "paymentTermId", toInt(h.get("paymentTermId")), "currencyId", toInt(h.get("currencyId")),
                        "fcyAmountTotal", toDec(h.get("fcyAmountTotal")), "TermsConditions", str(h.get("TermsConditions")),
                        "branchId", d.isEmpty() ? 0 : d.get(0).get("locationBranchId")),
                "details", d, "proformaNos", proformaNos(u, id, 0), "attachments", sup.attachments(u, SCREEN_NAME, id));
    }

    /** GetDetailByHeaderId(rr): the history's lower grid. */
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
     * HistoryGridFill() -> ProformaMaster.SearchHistory: @OrganizationId, @CompanyId, @documentTypeId 900, @CanViewAllRecord
     * always; the radio's date pair when ticked; @DocNoFrom / @DocNoTo when != 0; @createdUserId when the user cannot view
     * all. The form puts the Importer filter in SupplierCustomerId, which this BLL never reads, so the Importer combo does
     * not filter (desktop defect, reproduced).
     */
    public List<Map<String, Object>> history(Map<String, Object> f) {
        UserAccount u = sup.user(SCREEN);
        boolean all = sup.canViewAllRecord(SCREEN_NAME);
        Map<String, Object> p = map("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "documentTypeId", DOC_TYPE,
                "CanViewAllRecord", all);
        ImpBPackingDetailService.historyDates(p, f);
        int fromNo = toInt(f.get("fromDocNo")), toNo = toInt(f.get("toDocNo"));
        if (fromNo != 0) p.put("DocNoFrom", fromNo);
        if (toNo != 0) p.put("DocNoTo", toNo);
        if (!all) p.put("createdUserId", u.getId());
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.proformaSearch(p)) {
            out.add(map("Id", r.get("proformaMasterId"), "DocNo", r.get("docNo"), "ValidityUpto", r.get("validityUpto"),
                    "Importer", r.get("ImporterName"), "Exporter", r.get("ExporterName"), "ProformaNo", r.get("proformaNo"),
                    "ProformaDate", r.get("docDate"), "GoodsOrigin", r.get("GoodsOrigin"), "LoadingPort", r.get("LoadingPortName"),
                    "DestinationPort", r.get("DestinationPortName"), "IncoTerm", r.get("incoTermName"), "GrossWeight", r.get("grossWeightTotal"),
                    "NetWeight", r.get("netWeightTotal"), "FclQty", r.get("fclTotal"), "PaymentTerm", r.get("PaymentTermCode"),
                    "Currency", r.get("CurrencyCode"), "Amount", r.get("fcyAmountTotal"), "TermsAndConditions", r.get("TermsConditions"),
                    "EntryDate", r.get("CreatedOn"), "EntryUser", r.get("CreatedUserName"), "ModifyDate", r.get("LastModifiedOn"),
                    "ModifyUser", r.get("LastModifiedUserName"), "IsApproved", toBool(r.get("IsApproved")), "ApprovedDate", r.get("ApprovedOn"),
                    "ApprovedUser", r.get("ApprovedUserName"), "NoOfAttachments", r.get("NoOfAttachments")));
        }
        return out;
    }

    // ------------------------------------------------------------------ Insert()

    /**
     * btnsave_Click (RecId = 0) / btnupdate_Click ("Rec Id not found..." when RecId = 0) -> Insert(): formvalidation() in the
     * form's order, "Detail Grid Record Not Found. Please Check! ", the header exactly as the form fills it (proformaMasterId =
     * the chosen Proforma No's ProformaMasterId, Net Weight = Σ Qty/M.Ton × 1000), removed detail rows (appActionId 3) then the
     * grid rows (Id 0 when saving new), ProformaMaster.Save (@Activity INSERT when appActionId = 0, UPDATE otherwise) and the
     * DAL's attachment block, all in one transaction.
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
        List<Map<String, Object>> rows = list(b.get("rows"));

        boolean branchFeature = sup.feature(11);
        if (branchFeature && !has(sup.branchesOfUser(), "BranchId", toInt(h.get("branchId")))) throw invalid("BranchName Is required");
        if (str(h.get("docNo")).isEmpty() || toInt(h.get("docNo")) == 0) throw invalid("Doc No Is Required");
        if (toLong(h.get("importerId")) == 0) throw invalid("Importer Is Required");
        if (toLong(h.get("exporterId")) == 0) throw invalid("Exporter Is Required");
        long pmId = toLong(h.get("proformaMasterId"));
        Map<String, Object> serial = null;
        for (Map<String, Object> s : proformaNos(u, recId, 0)) if (toLong(s.get("Id")) == pmId) serial = s;
        if (pmId == 0 || serial == null) throw invalid("Proforma No Is Required");
        if (toInt(h.get("GoodsOriginId")) == 0) throw invalid("Goods Origin Is Required");
        if (toInt(h.get("loadingPortId")) == 0) throw invalid("Loading Port Is Required");
        if (toInt(h.get("destinationPortId")) == 0) throw invalid("Destination Port Is Required");
        if (toInt(h.get("incoTermId")) == 0) throw invalid("Inco Term Is Required");
        double mton = 0, amount = 0;
        for (Map<String, Object> r : rows) { mton += toDouble(r.get("QtyMTon")); amount += toDouble(r.get("Amount")); }
        double net = mton * 1000.0;
        if (!(net > 0.0)) throw invalid("Net Weight  Is Required");
        if (!(toDouble(h.get("grossWeightTotal")) >= net)) throw invalid("Gross Weight Must Be Equal Or Greater Than Net Weight Thank You");
        if (toDouble(h.get("fclTotal")) == 0.0) throw invalid("Fcl Qty Field Is Required");
        if (toInt(h.get("paymentTermId")) == 0) throw invalid("Payment Term Is Required");
        if (toInt(h.get("currencyId")) == 0) throw invalid("Currency Code Is Required");
        if (round3(amount) == 0.0) throw invalid("Fcy Amount  Is Required");
        if (rows.isEmpty()) throw invalid("Detail Grid Record Not Found. Please Check! ");

        Set<Long> existing = new HashSet<>();
        if (recId > 0) for (Map<String, Object> d : repo.proformaDetails(recId)) existing.add(toLong(d.get("proformaDetailId")));
        LocalDateTime now = LocalDateTime.now();
        ImpBProformaMaster m = new ImpBProformaMaster();
        m.OrganizationId = u.getOrganizationId();
        m.CompanyId = u.getCompanyId();
        m.BranchesId = branchId(u);
        m.FinancialYearId = sup.financialYearId();
        m.ProjectsId = branchId(u);
        m.documentTypeId = DOC_TYPE;
        m.appActionId = old == null ? 0 : toInt(old.get("appActionId"));     // AppActionId from ReadById, 0 after New
        m.isApproved = false;
        m.createdUserId = u.getId(); m.lastModifiedUserId = u.getId();
        m.createdOn = now; m.lastModifiedOn = now; m.dateOfIssue = now; m.lastShipmentDate = now; m.approvedOn = now;
        m.docNo = toInt(h.get("docNo"));
        m.validityUpto = nvl(toDate(h.get("validityUpto")), now);
        m.importerId = toLong(h.get("importerId"));
        m.exporterId = toLong(h.get("exporterId"));
        m.proformaNo = str(serial.get("ProformaNo"));
        m.proformaMasterId = pmId;
        m.docDate = nvl(toDate(h.get("docDate")), now);
        m.GoodsOriginId = toInt(h.get("GoodsOriginId"));
        m.loadingPortId = toInt(h.get("loadingPortId"));
        m.destinationPortId = toInt(h.get("destinationPortId"));
        m.incoTermId = toInt(h.get("incoTermId"));
        m.netWeightTotal = toDec(fmt3(net));                       // txtNetWeightHeader = (MTonTotal*1000).ToString("#,##0.###")
        m.grossWeightTotal = toDec(str(h.get("grossWeightTotal")).trim());
        m.fclTotal = BigDecimal.valueOf(toInt(str(h.get("fclTotal")).trim()));
        m.paymentTermId = toInt(h.get("paymentTermId"));
        m.currencyId = toInt(h.get("currencyId"));
        m.fcyAmountTotal = toDec(fmt3(amount));                    // txtfcyAmountHeader = Σ Amount .ToString("#,##0.###")
        m.TermsConditions = str(h.get("TermsConditions"));
        m.RowVersionLong = old == null ? 0 : toLong(old.get("RowVersionLong"));
        m.AttachmentsValues = old == null ? "" : str(old.get("AttachmentsValues"));
        m.CustomAttachmentsValues = old == null ? "" : str(old.get("CustomAttachmentsValues"));

        List<ImpBProformaDetail> details = new ArrayList<>();
        for (Map<String, Object> r : list(b.get("removed"))) {                 // LstRemoveRecordDetail (appActionId 3)
            long did = toLong(r.get("Id"));
            if (did <= 0) continue;
            if (!existing.contains(did)) throw invalid("Record not found.");
            ImpBProformaDetail vd = detail(r, u, now);
            vd.proformaDetailId = did;
            vd.netWeightInner = toDouble(r.get("QtyMTon")) / 1000.0;             // DeleteDetailGridRow divides here
            vd.netWeightOuter = toDouble(r.get("QtyMTon")) / 1000.0;
            vd.NetWeightKgs = 0;
            vd.RowVersionLong = 0;
            vd.appActionId = 3;
            details.add(vd);
        }
        for (Map<String, Object> r : rows) {
            ImpBProformaDetail vd = detail(r, u, now);
            vd.proformaDetailId = recId == 0 ? 0 : toLong(r.get("Id"));
            if (vd.proformaDetailId > 0 && !existing.contains(vd.proformaDetailId)) throw invalid("Record not found.");
            vd.appActionId = vd.proformaDetailId <= 0 ? 1 : 2;
            details.add(vd);
        }
        String activity = m.appActionId > 0 ? "UPDATE" : "INSERT";              // BLL ProformaMaster.Save

        final ImpBSupport.AttachmentPlan[] plan = new ImpBSupport.AttachmentPlan[1];
        long saved = repo.tx(() -> {
            plan[0] = sup.prepare(u, SCREEN_NAME, recId, b);
            if (plan[0].changed && !plan[0].finalList.isEmpty()) { m.AttachmentsValues = plan[0].values(); m.CustomAttachmentsValues = plan[0].customValues(); }
            long num = repo.set(ImpBRepository.IMEX + "[usp_Set_ProformaMaster]", m, activity);
            if (num > 0) m.proformaMasterId = num; else num = m.proformaMasterId;
            for (ImpBProformaDetail vd : details) {
                vd.proformaMasterId = m.proformaMasterId;
                String a = vd.appActionId == 2 ? "UPDATE" : vd.appActionId == 1 ? "INSERT" : vd.appActionId == 3 ? "DELETE" : "";
                repo.set(ImpBRepository.IMEX + "[usp_Set_ProformaDetail]", vd, a);
            }
            sup.apply(plan[0], u, SCREEN_NAME, num, (int) m.exporterId, DOC_TYPE);
            return num;
        });
        if (recId > 0) sup.afterUpdate(plan[0], recId, SCREEN_NAME, DOC_TYPE);
        return saved((int) saved, recId == 0 ? "Save SuccessFully" : "Update SuccessFully");
    }

    /** One grid row as Insert() fills ProformaDetail (qty = ToInt(NoOfBags), weights = Qty/M.Ton × 1000). */
    private static ImpBProformaDetail detail(Map<String, Object> r, UserAccount u, LocalDateTime now) {
        ImpBProformaDetail vd = new ImpBProformaDetail();
        vd.ItemId = toInt(r.get("ItemId"));
        vd.ItemDescription = str(r.get("ItemDetail"));
        vd.lotJobId = toInt(r.get("JobLotId"));
        vd.packSizeIdinner = toInt(r.get("PackSizeId"));
        vd.packSizeIdOuter = toInt(r.get("PackSizeId"));
        vd.qtyInner = toInt(r.get("NoOfBags"));
        vd.qtyOuter = toInt(r.get("NoOfBags"));
        double q = toDouble(r.get("QtyMTon"));
        vd.netWeightInner = q * 1000.0;
        vd.netWeightOuter = q * 1000.0;
        vd.NetWeightKgs = q * 1000.0;
        vd.ItemRate = toDouble(r.get("CostMTon"));
        vd.uomIdRate = toInt(r.get("RateUOMId"));
        vd.FcAmount = toDouble(r.get("Amount"));
        vd.RemarksDetail = str(r.get("Remarks"));
        vd.RowVersionLong = toLong(r.get("RowVersionLong"));
        vd.isApproved = false;
        vd.createdOn = now; vd.lastModifiedOn = now; vd.approvedOn = now;
        vd.createdUserId = u.getId(); vd.lastModifiedUserId = u.getId();
        return vd;
    }

    static LocalDateTime nvl(LocalDateTime a, LocalDateTime b) { return a == null ? b : a; }

    static double round3(double v) { return new BigDecimal(v).setScale(3, java.math.RoundingMode.HALF_UP).doubleValue(); }

    /** decimal.ToString("#,##0.###") read back by Conversion.ToDecimal: the value rounded to 3 places. */
    static String fmt3(double v) { return new BigDecimal(Double.toString(v)).setScale(3, java.math.RoundingMode.HALF_UP).stripTrailingZeros().toPlainString(); }

    /** btnPrint_Click / history Slip -> CommonServices.ProformaInvoiceSlip900: not traceable here (see the report). */
    public Map<String, Object> print(long id) {
        UserAccount u = sup.user(SCREEN);
        sup.require(u, SCREEN, "Print");
        throw invalid("Print-900 (CommonServices.ProformaInvoiceSlip900) is not available on the web: the desktop's CommonServices source is not "
                + "in the workspace, so its .rpt and procedure cannot be traced.");
    }
}
