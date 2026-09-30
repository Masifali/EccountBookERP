package com.mst.services.logistics;

import com.mst.models.UserAccount;
import com.mst.models.logistics.LgsBPurchaseOrderDetail;
import com.mst.models.logistics.LgsBPurchaseOrderHeader;
import com.mst.repositories.logistics.LgsBRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;
import java.util.Map;

import static com.mst.services.hrm.HrmSupport.*;
import static com.mst.services.logistics.LgsBSupport.*;

/**
 * 949 Logistic Purchase Order - Architecture.WinApp.Service.lgstcm.frmLogisticPurchaseOrder (DocumentTypeId 1303),
 * with its loader frmLoadLogisticAgreementForPO. BLL lgstcm.PurchaseOrderHeader (0389), DAL (0346).
 *
 * Save = DAL SetData in one transaction: [lgstcm].[USP_PurchaseOrderHeader_InsertAndUpdate] (ActionId 1 / 2),
 * [lgstcm].[USP_PurchaseOrderDetail_Insert] per row (removed rows first, ActionTypeId 3), and when an export invoice
 * is chosen [lgstcm].[USP_Purchase_ValidateWithInvoice]. Attachments are not part of this port: the page sends none,
 * and with an empty AttachmentsList the DAL skips that block; the record's existing AttachmentsValues are kept.
 */
@Service
public class LgsBPurchaseOrderService {

    public static final int SCREEN = 949;
    public static final String SCREEN_NAME = "frmLogisticPurchaseOrder";
    public static final int DOC_TYPE = 1303;
    /** EximServicesDefine (btnDefineItem) - the web route of screen 234. */
    public static final int SCREEN_DEFINE_ITEM = 234;

    @Autowired private LgsBSupport sup;
    @Autowired private LgsBRepository repo;

    // ================================================================== load

    /**
     * InitializeComponentMethod: rights, document no, LookUpAllServices (service type / payment term / location type /
     * rate base), export invoices (ActionId 2), suppliers of group 10 (Bill To Party and Service Provider), currencies,
     * service items, configuration, history combos.
     */
    public Map<String, Object> setup(int recId) {
        UserAccount u = sup.user(SCREEN);
        Map<String, Object> out = lists(u, recId);
        out.put("rights", sup.rights(u, SCREEN, SCREEN_NAME));
        out.put("docNo", repo.poGenerateCode(u, sup.fy(), DOC_TYPE));
        out.put("historyCombos", repo.poDropDown(u));
        out.put("yearStart", sup.yearStart(u));
        return out;
    }

    /** btnRefresh_Click: the global lists and configuration again. */
    public Map<String, Object> refresh(int recId) { return lists(sup.user(SCREEN), recId); }

    private Map<String, Object> lists(UserAccount u, int recId) {
        return map("lookups", sup.lookUps(u), "invoices", invoices(u, recId), "suppliers", sup.suppliers(u, true),
                "currencies", sup.currencies(u), "items", sup.serviceItems(u), "cities", sup.cities(u), "config", config(u));
    }

    /** dtExportInvoiceDbCall: ExImInvoice.getExportInvoicePendingAndAll (ActionId 2, RecId). */
    private List<Map<String, Object>> invoices(UserAccount u, int recId) {
        return repo.exportInvoices(u, 2, recId);
    }

    /** GetConfigurationsFromGlobal / GetConfigurationsFromGlobalandBind + CommonServices.GetERPFeatureById(6). */
    private Map<String, Object> config(UserAccount u) {
        Map<String, Object> c = sup.formats(u);
        c.put("defaultDays", toInt(sup.config(u, "DefaultDaysToLessFromHistoryFromDate")));
        c.put("itemSearchByCode", toBool(sup.config(u, "ItemSearchByCode")));
        c.put("foreignBaseCurrency", toInt(sup.config(u, "ForeignBaseCurrency")));
        c.put("fcyBaseCurrencyRate", toDouble(sup.config(u, "FcyBaseCurrencyRate")));
        c.put("taxPercentEditable", toBool(sup.config(u, "TaxPercentEditable")));
        c.put("multiCurrency", sup.feature(u, 6));
        return c;
    }

    /** LocationdtFill(16): SeaPorts.Getall (17 = the global city list the page already holds). */
    public List<Map<String, Object>> ports() { return sup.ports(sup.user(SCREEN)); }

    /** TaxTypeDbCall(ItemId, DocDate): the first schedule row -> Id, TaxType, TaxPrcnt. */
    public List<Map<String, Object>> tax(int itemId, String docDate) {
        return taxOf(repo, sup.user(SCREEN), itemId, docDate);
    }

    static List<Map<String, Object>> taxOf(LgsBRepository repo, UserAccount u, int itemId, String docDate) {
        List<Map<String, Object>> out = new ArrayList<>();
        LocalDateTime d = toDate(docDate);
        List<Map<String, Object>> r = repo.taxForItem(u, itemId, d == null ? LocalDateTime.now() : d);
        if (!r.isEmpty()) out.add(map("Id", str(r.get(0).get("TaxNameId")), "TaxType", str(r.get(0).get("TaxName")), "TaxPrcnt", str(r.get(0).get("TaxPercent"))));
        return out;
    }

    /** txtdocdate_ValueChanged: CommonServices.GetTaxScheduleDetailbyItemIds(",1,2...", DocDate). */
    public List<Map<String, Object>> taxByItems(String itemIds, String docDate) {
        UserAccount u = sup.user(SCREEN);
        LocalDateTime d = toDate(docDate);
        return repo.taxForItems(u, itemIds, d == null ? LocalDateTime.now() : d);
    }

    /** ShowInvoiceInfo: ExImInvoice.GetInvoiceInformationByInvoiceIdForServices (only this company's invoices). */
    public Map<String, Object> invoiceInfo(int invoiceId) {
        UserAccount u = sup.user(SCREEN);
        List<Map<String, Object>> r = repo.invoiceInfo(u, invoiceId);
        return r.isEmpty() ? map("found", false) : map("found", true, "row", r.get(0));
    }

    // ================================================================== read

    /** ReadById(Id): PurchaseOrderHeader.ReadById (header + detail) - only a record of this organization / company. */
    public Map<String, Object> read(int id) {
        UserAccount u = sup.user(SCREEN);
        Map<String, Object> h = ownHeader(u, id);
        return map("header", h, "detail", detailRows(repo.poDetail(id)));
    }

    private Map<String, Object> ownHeader(UserAccount u, int id) {
        List<Map<String, Object>> r = repo.poHeader(id);
        if (r.isEmpty() || !ownRow(u, r.get(0))) throw invalid("No Record Found");
        return r.get(0);
    }

    /**
     * LogisticsPurchaseOrder_Helper.FillDetailFromListCommonForReadById: the detail DataTable columns the form reads back
     * (Id, AgreementHeaderId, AgreementDetailId, AgreementNo, PartyRefDocNo ... LocationFrom/To by LocationTypeId 16 = ports).
     */
    static List<Map<String, Object>> detailRows(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> d : rows) {
            int lt = toInt(d.get("LocationTypeId"));
            out.add(map("Id", toInt(d.get("PurchaseOrderDetailId")), "AgreementHeaderId", toInt(d.get("AgreementHeaderId")),
                    "AgreementDetailId", toInt(d.get("AgreementDetailId")), "AgreementNo", toInt(d.get("AgreementDocNo")),
                    "PartyRefDocNo", str(d.get("partyRefDocNo")), "PartyRefDocDate", d.get("partyRefDocDate"),
                    "ItemId", toInt(d.get("serviceItemId")), "ServiceItemCode", str(d.get("ItemCode")), "ServiceItemName", str(d.get("ItemName")),
                    "TransactionCurrencyId", toInt(d.get("transactionCurrencyId")), "TransactionCurrency", str(d.get("TransactionCurrencyCode")),
                    "TransactionExchangeRate", toDouble(d.get("transactionExchangeRate")), "ServiceRate", toDouble(d.get("OrderRate")),
                    "RateBaseId", toInt(d.get("RateBaseUomId")), "RateBase", str(d.get("RateBaseUom")), "RateBaseEquivalent", toDouble(d.get("RateBaseUomEquivalent")),
                    "Qty", toDouble(d.get("Qty")), "NetWeight", toDouble(d.get("NetWeight")),
                    "TcyAmount", toDouble(d.get("tcyAmount")), "LcyAmount", toDouble(d.get("lcyAmount")),
                    "TaxNameId", toInt(d.get("TaxNameId")), "TaxName", str(d.get("TaxName")), "Tax%", toDouble(d.get("TaxPercent")),
                    "TaxAmount", toDouble(d.get("TaxAmount")), "TotalTcyAmount", toDouble(d.get("TotalAmount")),
                    "LocationTypeId", lt, "LocationType", str(d.get("LocationType")),
                    "LocationFromId", toInt(d.get(lt == 16 ? "LoadingPortId" : "FromCityId")), "LocationFrom", str(d.get(lt == 16 ? "LoadingPort" : "FromCity")),
                    "LocationToId", toInt(d.get(lt == 16 ? "DestinationPortId" : "ToCityId")), "LocationTo", str(d.get(lt == 16 ? "DestinationPort" : "ToCity")),
                    "Remarks", str(d.get("Remarks"))));
        }
        return out;
    }

    // ================================================================== save

    /**
     * Insert(): the validation of the form in its order and wording, then the model exactly as the form fills it and
     * PurchaseOrderHeader.Save (ActionId = RecId == 0 ? 1 : 2) -> DAL SetData.
     */
    public Map<String, Object> save(Map<String, Object> b) {
        UserAccount u = sup.user(SCREEN);
        int recId = Math.max(0, i(b, "recId"));
        sup.require(u, SCREEN, recId > 0 ? "Update" : "Save");
        List<Map<String, Object>> rows = list(b, "rows");
        List<Map<String, Object>> removed = list(b, "removed");
        if (rows.isEmpty()) throw invalid("Detail Record Not Found");
        nonZeroInt(s(b, "docNo"), "Doc No");
        combo(i(b, "serviceTypeId"), "Service Type");
        combo(i(b, "brokerAgentId"), "Bill To Party");
        combo(i(b, "serviceProviderId"), "Broker Agent Or Service Provider");
        combo(i(b, "paymentTermId"), "Payment Term");
        nonZeroInt(s(b, "dueDays"), "Due Days");
        combo(i(b, "fcyCurrencyId"), "Foreign Currency");
        nonZeroDouble(s(b, "fcyExchangeRate"), "Fcy Rate");
        boolean multi = sup.feature(u, 6);
        if (multi) {
            combo(i(b, "glcyCurrencyId"), "Glcy Currency");
            nonZeroDouble(s(b, "glcyRate"), "Glcy Rate");
            nonZeroDouble(s(b, "glcyAmount"), "Glcy Amount");
        }
        LocalDateTime docDate = toDate(b.get("docDate"));
        LocalDateTime validity = toDate(b.get("validityDate"));
        if (docDate == null) docDate = LocalDateTime.now();
        if (validity == null) validity = docDate;
        if (validity.toLocalDate().isBefore(docDate.toLocalDate())) throw invalid("Order Validity Date cannot be less than Document Date");

        Map<String, Object> existing = recId > 0 ? ownHeader(u, recId) : null;
        LocalDateTime now = LocalDateTime.now();
        LgsBPurchaseOrderHeader h = new LgsBPurchaseOrderHeader();
        h.PurchaseOrderHeaderId = recId;
        h.CompanyId = u.getCompanyId();
        h.BranchesId = toInt(u.getBranchesId());
        h.ProjectsId = toInt(u.getBranchesId());
        h.OrganizationId = u.getOrganizationId();
        h.FinancialYearId = sup.fy();
        h.DocumentTypeId = DOC_TYPE;
        h.DocumentDate = docDate;
        h.DocumentNo = toInt(s(b, "docNo"));
        h.ExportInvoiceId = i(b, "exportInvoiceId");
        h.serviceTypeId = i(b, "serviceTypeId");
        h.brokerAgentId = i(b, "brokerAgentId");
        h.serviceProviderId = i(b, "serviceProviderId");
        h.paymentTermId = i(b, "paymentTermId");
        h.DueDays = toInt(s(b, "dueDays"));
        h.OrderValidityDate = validity;
        h.fcyCurrencyId = i(b, "fcyCurrencyId");
        h.fcyExchangeRate = d(b, "fcyExchangeRate");
        h.RemarksHeader = s(b, "remarks");
        h.GlcyCurrencyId = i(b, "glcyCurrencyId");
        h.GlcyExchangeRate = d(b, "glcyRate");
        h.GlcyAmount = d(b, "glcyAmount");
        h.EntryUserId = u.getId();
        h.EntryDate = now;
        h.ModifyDate = now;
        h.ModifyUserId = u.getId();
        h.ApprovedUserId = u.getId();
        h.ApprovedDate = now;
        h.IsApproved = false;
        h.ActionId = recId == 0 ? 1 : 2;
        /* AttachmentsValues = the form's fields (read back on ReadById); no attachment is added or removed here. */
        h.AttachmentsValues = existing == null ? "" : str(existing.get("AttachmentsValues"));
        h.CustomAttachmentsValues = existing == null ? "" : str(existing.get("CustomAttachmentsValues"));

        List<LgsBPurchaseOrderDetail> details = new ArrayList<>();
        if (recId > 0) {
            List<Integer> own = new ArrayList<>();
            for (Map<String, Object> d : repo.poDetail(recId)) own.add(toInt(d.get("PurchaseOrderDetailId")));
            for (Map<String, Object> r : removed) {
                int did = i(r, "Id");
                if (did <= 0 || !own.contains(did)) continue;
                LgsBPurchaseOrderDetail d = detail(b, r);
                d.PurchaseOrderDetailId = did;
                d.ActionTypeId = 3;
                details.add(d);
            }
        }
        double totalGlcy = 0;
        for (int n = 0; n < rows.size(); n++) {
            Map<String, Object> r = rows.get(n);
            LgsBPurchaseOrderDetail vd = detail(b, r);
            vd.PurchaseOrderDetailId = recId != 0 ? i(r, "Id") : 0;
            vd.ActionTypeId = vd.PurchaseOrderDetailId <= 0 ? 1 : 2;
            field(vd.partyRefDocNo, "Party Ref DocNo", n);
            field(vd.serviceItemId, "Service Item", n);
            field(vd.transactionCurrencyId, "Transaction Currency", n);
            field(vd.transactionExchangeRate, "Transaction Exchange Rate", n);
            field(vd.OrderRate, "Rate", n);
            field(vd.RateBaseUomId, "Rate Base", n);
            field(vd.Qty, "Qty", n);
            field(vd.NetWeight, "NetWeight", n);
            field(vd.tcyAmount, "Tcy amount", n);
            field(vd.lcyAmount, "Lcy amount", n);
            if (vd.TaxNameId > 0 || vd.TaxPercent > 0.0 || vd.TaxAmount > 0.0) {
                field(vd.TaxNameId, "Tax Name", n);
                field(vd.TaxPercent, "Tax Percent", n);
                field(vd.TaxAmount, "Tax Amount", n);
            }
            field(vd.TotalAmount, "Total Tcy Amount", n);
            field(vd.LocationTypeId, "Location Type", n);
            if (vd.LocationTypeId == 16) {
                field(vd.LoadingPortId, "Loading Port", n);
                field(vd.DestinationPortId, "Destination Port", n);
                if (vd.LoadingPortId == vd.DestinationPortId)
                    throw invalid("Loading Port And Destination Port Can't be Same in Detail  at row No: " + (n + 1));
            }
            if (vd.LocationTypeId == 17) {
                field(vd.FromCityId, "From City", n);
                field(vd.ToCityId, "To City", n);
                if (vd.FromCityId == vd.ToCityId) throw invalid("FromCity And ToCity Can't be Same in Detail  at row No: " + (n + 1));
            }
            if (h.RemarksHeader == null || h.RemarksHeader.trim().isEmpty()) h.RemarksHeader = vd.Remarks;
            if (h.transactionCurrencyId == 0) {
                h.transactionCurrencyId = vd.transactionCurrencyId;
                h.transactionExchangeRate = vd.transactionExchangeRate;
            }
            totalGlcy += vd.GlcyAmount;
            details.add(vd);
        }
        h.GlcyAmount = totalGlcy;
        if (recId > 0) {
            for (LgsBPurchaseOrderDetail d : details) {
                if (d.ActionTypeId == 2 && !ownDetail(recId, d.PurchaseOrderDetailId)) throw invalid("No Record Found");
            }
        }
        final int invoiceId = h.ExportInvoiceId;
        int id = repo.tx(() -> {
            if (details.isEmpty()) throw invalid("Detail list not found");
            int n = repo.set("[lgstcm].[USP_PurchaseOrderHeader_InsertAndUpdate]", h);
            h.PurchaseOrderHeaderId = n > 0 ? n : h.PurchaseOrderHeaderId;
            for (LgsBPurchaseOrderDetail d : details) {
                d.PurchaseOrderHeaderId = h.PurchaseOrderHeaderId;
                repo.exec("[lgstcm].[USP_PurchaseOrderDetail_Insert]", d);
            }
            if (invoiceId > 0) {
                repo.exec("[lgstcm].[USP_Purchase_ValidateWithInvoice]", LgsBRepository.p("ExportInvoiceId", invoiceId, "DocumentTypeId", DOC_TYPE,
                        "CurrentDocId", h.PurchaseOrderHeaderId > 0 ? h.PurchaseOrderHeaderId : null));
            }
            return h.PurchaseOrderHeaderId;
        });
        return map("success", true, "id", id, "message", (recId > 0 ? "Data Update Successfully....  " : "Data Save Successfully....  ") + h.DocumentNo,
                "docNo", repo.poGenerateCode(u, sup.fy(), DOC_TYPE));
    }

    private boolean ownDetail(int recId, int detailId) {
        for (Map<String, Object> d : repo.poDetail(recId)) if (toInt(d.get("PurchaseOrderDetailId")) == detailId) return true;
        return false;
    }

    /** FillDetailListCommonForInsertAndDelete(vd, r): the grid row plus the header Fcy / Glcy currency and rates. */
    private static LgsBPurchaseOrderDetail detail(Map<String, Object> b, Map<String, Object> r) {
        LgsBPurchaseOrderDetail vd = new LgsBPurchaseOrderDetail();
        vd.AgreementHeaderId = i(r, "AgreementHeaderId");
        vd.AgreementDetailId = i(r, "AgreementDetailId");
        vd.partyRefDocNo = s(r, "PartyRefDocNo");
        LocalDateTime rd = toDate(r.get("PartyRefDocDate"));
        vd.partyRefDocDate = rd == null ? LocalDateTime.now() : rd;
        vd.serviceItemId = i(r, "ItemId");
        vd.transactionCurrencyId = i(r, "TransactionCurrencyId");
        vd.transactionExchangeRate = d(r, "TransactionExchangeRate");
        vd.OrderRate = d(r, "ServiceRate");
        vd.RateBaseUomId = i(r, "RateBaseId");
        vd.Qty = d(r, "Qty");
        vd.NetWeight = d(r, "NetWeight");
        vd.tcyAmount = d(r, "TcyAmount");
        vd.lcyAmount = d(r, "LcyAmount");
        vd.FcyCurrencyId = i(b, "fcyCurrencyId");
        vd.FcyExchangeRate = d(b, "fcyExchangeRate");
        vd.fcyAmount = vd.FcyExchangeRate > 0.0 ? vd.lcyAmount / vd.FcyExchangeRate : 0.0;
        vd.GlcyCurrencyId = i(b, "glcyCurrencyId");
        vd.GlcyExchangeRate = d(b, "glcyRate");
        vd.GlcyAmount = vd.GlcyExchangeRate > 0.0 ? vd.lcyAmount / vd.GlcyExchangeRate : 0.0;
        vd.TaxNameId = i(r, "TaxNameId");
        vd.TaxPercent = d(r, "Tax%");
        vd.TaxAmount = d(r, "TaxAmount");
        vd.TotalAmount = d(r, "TotalTcyAmount");
        vd.LocationTypeId = i(r, "LocationTypeId");
        if (vd.LocationTypeId == 16) {
            vd.LoadingPortId = i(r, "LocationFromId");
            vd.DestinationPortId = i(r, "LocationToId");
        } else {
            vd.FromCityId = i(r, "LocationFromId");
            vd.ToCityId = i(r, "LocationToId");
        }
        vd.Remarks = s(r, "Remarks");
        return vd;
    }

    // ================================================================== delete / history / print

    /** btnDelete_Click: PurchaseOrderHeader.DeleteByID(UserId, RecId). */
    public Map<String, Object> delete(int id) {
        UserAccount u = sup.user(SCREEN);
        sup.require(u, SCREEN, "Delete");
        if (id <= 0) throw invalid("No record found to Delete");
        ownHeader(u, id);
        repo.tx(() -> { repo.poDelete(u.getId(), id); return 0; });
        return map("success", true, "message", "Delete Record Successfully", "docNo", repo.poGenerateCode(u, sup.fy(), DOC_TYPE));
    }

    /** HistoryComboBind / BtnRefreshHistory: GetDataForDropDownFromPurchaseOrderHeader. */
    public List<Map<String, Object>> historyCombos() { return repo.poDropDown(sup.user(SCREEN)); }

    /** HistoryGridFill: PurchaseOrderHeader.FormHistory with @ServiceTypeId / @ServiceProviderId when chosen. */
    public List<Map<String, Object>> history(Map<String, Object> f) {
        UserAccount u = sup.user(SCREEN);
        Map<String, Object> p = sup.historyParams(u, DOC_TYPE, sup.canViewAll(u, SCREEN_NAME), f);
        int st = i(f, "serviceTypeId"), sp = i(f, "serviceProviderId");
        if (st != 0) p.put("ServiceTypeId", st);
        if (sp != 0) p.put("ServiceProviderId", sp);
        return repo.formHistory("[lgstcm].[USP_PurchaseOrderHeader_GetAllMethod]", p);
    }

    /** DatagridHistory_SelectionChanged -> GetDetailGrdByHeadId(Id). */
    public List<Map<String, Object>> historyDetail(int id) {
        UserAccount u = sup.user(SCREEN);
        ownHeader(u, id);
        return detailRows(repo.poDetail(id));
    }

    /** Print (CrystalReportPrint_Helper.PurchaseOrderHeader_Slip_1303): only this company's record. */
    public Map<String, Object> printCheck(int id) {
        UserAccount u = sup.user(SCREEN);
        sup.require(u, SCREEN, "Print");
        ownHeader(u, id);
        return map("id", id);
    }

    // ================================================================== Load Agreement (frmLoadLogisticAgreementForPO)

    /** CombosFill(ComboDbCall()): GetDataForDropDownFromAgreementHeader rows (ServiceType / ServiceProvider / BrokerAgent / ServiceItem). */
    public List<Map<String, Object>> agreementCombos() { return repo.agreementDropDown(sup.user(SCREEN)); }

    /** PendingDataDbCall: AgreementHeader.Agreement_DataForPurchaseOrder (DocumentTypeId 1301). */
    public List<Map<String, Object>> agreements(Map<String, Object> f) {
        UserAccount u = sup.user(SCREEN);
        return repo.agreementForPo(u, sup.fy(), toDate(f.get("fromDate")), toDate(f.get("toDate")), i(f, "fromDocNo"), i(f, "toDocNo"),
                i(f, "serviceTypeId"), i(f, "serviceProviderId"), i(f, "brokerAgentId"), i(f, "itemId"));
    }

    // ================================================================== FormHelper validation (texts)

    static void combo(int v, String name) { if (v == 0) throw invalid(name + " field is required"); }

    static void text(String v, String name) { if (v == null || v.trim().isEmpty()) throw invalid(name + " field is required"); }

    static void nonZeroInt(String v, String name) { if (toInt(v) == 0) throw invalid(name + " must be a non-zero number"); }

    static void nonZeroDouble(String v, String name) { if (toDouble(v) == 0.0) throw invalid(name + " must be a non-zero number"); }

    /** FormHelper.ValidateField: "{field} is required in Detail Grid at row No: {row + 1}". */
    static void field(Object v, String name, int row) {
        boolean bad = v == null
                || (v instanceof Integer && (Integer) v == 0)
                || (v instanceof Double && (Double) v <= 0.0)
                || (v instanceof java.math.BigDecimal && ((java.math.BigDecimal) v).signum() <= 0)
                || (v instanceof String && ((String) v).trim().isEmpty());
        if (bad) throw invalid(name + " is required in Detail Grid at row No: " + (row + 1));
    }
}
