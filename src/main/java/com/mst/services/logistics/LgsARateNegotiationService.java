package com.mst.services.logistics;

import com.mst.models.UserAccount;
import com.mst.models.logistics.LgsARateNegotiationDetail;
import com.mst.models.logistics.LgsARateNegotiationHeader;
import com.mst.models.logistics.LgsARateNegotiationSourceDocument;
import com.mst.repositories.logistics.LgsALookupRepository;
import com.mst.repositories.logistics.LgsARateAgreementRepository;
import com.mst.services.hrm.HrmSupport;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.Set;

import static com.mst.services.hrm.HrmSupport.*;

/**
 * BLL of the two rate-negotiation forms (one class family on the desktop, two forms):
 *
 *   937  Architecture.WinApp.Service.lgstcm/frmLogisticRateNegotiation.cs            DocumentTypeId 1300, ports
 *   941  Architecture.WinApp.Service.lgstcm/frmLogisticRateNegotiationTransporter.cs DocumentTypeId 1302, cities
 *
 * The transporter form is the same form without the vessel fields (routing type / description, cargo cut-off,
 * ETD, transit days, free days), with City From / City To instead of the ports, Service Type defaulted to 3,
 * no "Source Document Detail Record Not Found" check and no broker mismatch check. Validation order and texts are
 * the forms' (FormHelper.ValidateControls / ValidateField / the forms' own MessageBox texts).
 */
@Service
public class LgsARateNegotiationService {

    public static final int SCREEN_RN = 937;
    public static final int SCREEN_TR = 941;
    public static final int DOC_RN = 1300;
    public static final int DOC_TR = 1302;

    @Autowired private LgsARateAgreementRepository repo;
    @Autowired private LgsALookupRepository look;
    @Autowired private HrmSupport hrm;

    private static int screen(boolean tr) { return tr ? SCREEN_TR : SCREEN_RN; }
    private static int doc(boolean tr) { return tr ? DOC_TR : DOC_RN; }

    // ================================================================== load

    Map<String, Object> rights(UserAccount u, int screen) {
        Map<String, Object> r = hrm.rights(u, screen);
        r.put("canViewAllRecord", hrm.can(u, screen, "CanView AllRecord"));
        return r;
    }

    /**
     * AllLookUpComboBind(LookUps.LookUpAllServices): the Activity column splits the rows into ServiceType, DealStatus,
     * RoutingType, RateBaseUom (OtherReference = Equivalent) and PaymentTerm lists of (Id, Name[, Equivalent]).
     */
    static Map<String, Object> splitLookups(List<Map<String, Object>> rows) {
        List<Map<String, Object>> st = new ArrayList<>(), ds = new ArrayList<>(), rt = new ArrayList<>(), uom = new ArrayList<>(), pt = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            String a = str(r.get("Activity"));
            Map<String, Object> m = map("Id", toInt(r.get("Id")), "Name", str(r.get("ReferenceName")));
            switch (a) {
                case "ServiceType": st.add(m); break;
                case "DealStatus": ds.add(m); break;
                case "RoutingType": rt.add(m); break;
                case "RateBaseUom": m.put("Equivalent", toDouble(r.get("OtherReference"))); uom.add(m); break;
                case "PaymentTerm": pt.add(m); break;
                default: break;
            }
        }
        return map("serviceTypes", st, "dealStatuses", ds, "routingTypes", rt, "rateBases", uom, "paymentTerms", pt);
    }

    /** HistoryComboBind: ServiceType / DealStatus rows of the drop-down procedure. */
    private Map<String, Object> historyCombos(UserAccount u) {
        List<Map<String, Object>> st = new ArrayList<>(), ds = new ArrayList<>();
        for (Map<String, Object> r : repo.rnDropDown(u)) {
            String a = str(r.get("Activity"));
            if ("ServiceType".equals(a)) st.add(map("Id", toInt(r.get("Id")), "Name", str(r.get("ReferenceName"))));
            else if ("DealStatus".equals(a)) ds.add(map("Id", toInt(r.get("Id")), "Name", str(r.get("ReferenceName"))));
        }
        return map("serviceTypes", st, "dealStatuses", ds);
    }

    /** DecimalFCYRateFormate - "#,#0." + N zeros; N = config "DefaultNoOfDecimalPointsForFcyRate" (1-10), 4 when unset. */
    int fcyRateDecimals(UserAccount u) {
        int n = toInt(look.config(u, "DefaultNoOfDecimalPointsForFcyRate"));
        return n >= 1 && n <= 10 ? n : 4;
    }

    /**
     * InitializeComponentMethod: rights, HistoryComboDbCall, DocumentNoDbCall, LookUpAllServices, ports (937) / cities
     * (941), dtExportInvoiceDbCall, GetConfigurationsFromGlobal(andBind): ForeignBaseCurrency / FcyBaseCurrencyRate,
     * DefaultDaysToLessFromHistoryFromDate; currencies, parties (customer group 10), services items.
     */
    public Map<String, Object> setup(boolean tr) {
        int screen = screen(tr);
        UserAccount u = hrm.user(screen);
        Map<String, Object> out = lists(u, tr);
        out.put("rights", rights(u, screen));
        out.put("docNo", repo.rnGenerateCode(u, hrm.financialYearId(), doc(tr)));
        out.put("history", historyCombos(u));
        out.put("financialYearStart", look.financialYearStart(u, hrm.financialYearId()));
        return out;
    }

    /** btnRefresh_Click. */
    public Map<String, Object> refresh(boolean tr) { return lists(hrm.user(screen(tr)), tr); }

    /** BtnRefreshHistory_Click -> HistoryComboBind(HistoryComboDbCall()). */
    public Map<String, Object> refreshHistory(boolean tr) { return historyCombos(hrm.user(screen(tr))); }

    private Map<String, Object> lists(UserAccount u, boolean tr) {
        Map<String, Object> out = new LinkedHashMap<>(splitLookups(look.lookAllServices(u)));
        if (tr) out.put("cities", look.cities(u)); else out.put("ports", look.ports(u));
        out.put("currencies", look.currencies(u));
        out.put("parties", look.partiesGroup10(u));
        out.put("items", look.servicesItems(u));
        out.put("invoices", look.exportInvoices(u));
        out.put("config", map("DefaultDaysToLessFromHistoryFromDate", toInt(look.config(u, "DefaultDaysToLessFromHistoryFromDate")),
                "ForeignBaseCurrency", toInt(look.config(u, "ForeignBaseCurrency")),
                "FcyBaseCurrencyRate", toDouble(look.config(u, "FcyBaseCurrencyRate")),
                "FcyRateDecimals", fcyRateDecimals(u)));
        return out;
    }

    /** FromReset -> DocumentNoDbCall. */
    public Map<String, Object> docNo(boolean tr) {
        UserAccount u = hrm.user(screen(tr));
        return map("docNo", repo.rnGenerateCode(u, hrm.financialYearId(), doc(tr)));
    }

    // ================================================================== history

    /**
     * HistoryGridFill: FormHistory with the ticked date kind (doc / entry / modify / approved), each date only when its
     * box is ticked, doc range when != 0, ServiceTypeId / DealStatusId when != 0.
     */
    public List<Map<String, Object>> history(boolean tr, Map<String, String> q) {
        int screen = screen(tr);
        UserAccount u = hrm.user(screen);
        Map<String, Object> f = dateFilters(q, true);
        if (toInt(q.get("fromDocNo")) != 0) f.put("FromDocNo", toInt(q.get("fromDocNo")));
        if (toInt(q.get("toDocNo")) != 0) f.put("ToDocNo", toInt(q.get("toDocNo")));
        if (toInt(q.get("serviceTypeId")) != 0) f.put("ServiceTypeId", toInt(q.get("serviceTypeId")));
        if (toInt(q.get("dealStatusId")) != 0) f.put("DealStatusId", toInt(q.get("dealStatusId")));
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.rnHistory(u, hrm.financialYearId(), doc(tr), hrm.can(u, screen, "CanView AllRecord"), f)) {
            out.add(map("Id", toInt(r.get("logisticRateNegotiationHeaderId")), "DocumentTypeId", toInt(r.get("documentTypeId")),
                    "DocumentNo", toInt(r.get("documentNo")), "DocumentDate", r.get("documentDate"), "ServiceTypeId", toInt(r.get("ServiceTypeId")),
                    "ServiceType", str(r.get("ServiceType")), "DealStatusId", toInt(r.get("DealStatusId")), "DealStatus", str(r.get("DealStatus")),
                    "TransactionCurrencyId", toInt(r.get("transactionCurrencyId")), "TcyCode", str(r.get("TcyCode")),
                    "TransactionExchangeRate", r.get("transactionExchangeRate"), "FcyCurrencyId", toInt(r.get("fcyCurrencyId")),
                    "FcyCode", str(r.get("FcyCode")), "FcyExchangeRate", r.get("fcyExchangeRate"), "RemarksHeader", str(r.get("RemarksHeader")),
                    "EntryDate", r.get("EntryDate"), "EntryUserName", str(r.get("EntryUserName")), "ModifyDate", r.get("ModifyDate"),
                    "ModifyUserName", str(r.get("ModifyUserName")), "IsApproved", toBool(r.get("IsApproved")), "ApprovedDate", r.get("ApprovedDate"),
                    "ApprovalUserName", str(r.get("ApprovalUserName")), "NoOfAttachments", toInt(r.get("NoOfAttachments"))));
        }
        return out;
    }

    /** The date branch of HistoryGridFill (drdocdate / rdentrydate / rdmodifydate / rdapproveddate). */
    static Map<String, Object> dateFilters(Map<String, String> q, boolean approvedTo) {
        Map<String, Object> f = new LinkedHashMap<>();
        String kind = str(q.get("dateKind"));
        LocalDateTime from = toDate(q.get("fromDate")), to = toDate(q.get("toDate"));
        String fk, tk;
        switch (kind) {
            case "entry": fk = "EntryFromDate"; tk = "EntryToDate"; break;
            case "modify": fk = "ModifyFromDate"; tk = "ModifyToDate"; break;
            case "approved": fk = "ApprovedFromDate"; tk = approvedTo ? "ApprovedToDate" : null; break;
            default: fk = "FromDate"; tk = "ToDate"; break;
        }
        if (from != null) f.put(fk, ts(from));
        if (to != null && tk != null) f.put(tk, ts(to));
        return f;
    }

    // ================================================================== read

    private Map<String, Object> owned(UserAccount u, boolean tr, int id) {
        List<Map<String, Object>> h = id > 0 ? repo.rnHeader(id) : new ArrayList<>();
        if (h.isEmpty()) throw invalid("No Record Found");
        Map<String, Object> r = h.get(0);
        if (toInt(r.get("OrganizationId")) != u.getOrganizationId() || toInt(r.get("CompanyId")) != u.getCompanyId()
                || toInt(r.get("documentTypeId")) != doc(tr)) throw invalid("No Record Found");
        return r;
    }

    /** FillDetailFromListCommonForReadById. */
    private List<Map<String, Object>> detailRows(int id) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> d : repo.rnDetail(id)) {
            out.add(map("Id", toInt(d.get("logisticRateNegotiationDetailId")), "BrokerAgentId", toInt(d.get("partyIdAgent")),
                    "BrokerAgent", str(d.get("BrokerAgentName")), "ServiceProviderId", toInt(d.get("serviceProviderId")),
                    "ServiceProviderName", str(d.get("ServiceProviderName")), "PaymentTermId", toInt(d.get("paymentTermId")),
                    "PaymentTerm", str(d.get("PaymentTerm")), "DueDays", toInt(d.get("DueDays")),
                    "VesselRoutingTypeId", toInt(d.get("VesselRoutingTypeId")), "VesselRoutingType", str(d.get("VesselRoutingType")),
                    "VesselRoutingDescription", str(d.get("VesselRoutingDescription")), "ItemId", toInt(d.get("ServiceItemId")),
                    "ItemCode", str(d.get("ItemCode")), "ItemName", str(d.get("ItemName")), "TransactionCurrencyId", toInt(d.get("tcyCurrencyId")),
                    "TransactionCurrency", str(d.get("TransactionCurrencyCode")), "ServiceRate", toDouble(d.get("serviceRate")),
                    "RateBaseId", toInt(d.get("serviceRateBaseUomId")), "RateBase", str(d.get("RateBaseUom")),
                    "RateBaseEquivalent", toDouble(d.get("RateBaseUomEquivalent")), "TcyAmount", toDouble(d.get("tcyAmount")),
                    "LcyAmount", toDouble(d.get("lcyAmount")), "OfferValidityDays", toInt(d.get("OffervalidityDays")),
                    "OfferValidityDate", d.get("OfferValidityDate"), "CargoCutOffDate", d.get("cutOffDate"), "EtdSealingDate", d.get("etdPOL"),
                    "TransitsTimeDays", toInt(d.get("vesselTransitDays")), "FreeDaysAtPOD", toInt(d.get("freeDaysAtPOD")),
                    "Remarks", str(d.get("Remarks")), "portOfDischargeId", toInt(d.get("portOfDischargeId")),
                    "portOfLoadingId", toInt(d.get("portOfLoadingId")), "CityFromId", toInt(d.get("CityFromId")), "CityToId", toInt(d.get("CityToId"))));
        }
        return out;
    }

    /**
     * ReadById(Id): header, detail and source documents. The Edit path (double-click / Edit button / Ctrl+Enter)
     * needs the Update right ("you don't have update rights..."); Save As does not (edit=false).
     */
    public Map<String, Object> read(boolean tr, int id, boolean edit) {
        int screen = screen(tr);
        UserAccount u = hrm.user(screen);
        if (edit && !hrm.can(u, screen, "Update")) throw invalid("you don't have update rights...");
        Map<String, Object> h = owned(u, tr, id);
        List<Map<String, Object>> docs = new ArrayList<>();
        for (Map<String, Object> s : repo.rnSourceDocuments(id)) {
            docs.add(map("Id", toInt(s.get("logisticRateNegotiationSourceDocumentId")), "DocumentTypeId", toInt(s.get("SourceDocumentTypeId")),
                    "DocumentId", toInt(s.get("SourceDocumentId")), "NoOfContainers", toInt(s.get("SourceDocumentNoOfContainers")),
                    "MTon", toDouble(s.get("SourceDocumentMTon")), "BrokerAgentId", toInt(s.get("BrokerAgentNameId")),
                    "ServiceProviderId", toInt(s.get("ServiceProviderId")), "Remarks", str(s.get("Remarks"))));
        }
        return map("Id", toInt(h.get("logisticRateNegotiationHeaderId")), "documentNo", toInt(h.get("documentNo")),
                "documentDate", h.get("documentDate"), "ServiceTypeId", toInt(h.get("ServiceTypeId")),
                "transactionCurrencyId", toInt(h.get("transactionCurrencyId")), "transactionExchangeRate", toDouble(h.get("transactionExchangeRate")),
                "fcyCurrencyId", toInt(h.get("fcyCurrencyId")), "fcyExchangeRate", toDouble(h.get("fcyExchangeRate")),
                "DealStatusId", toInt(h.get("DealStatusId")), "RemarksHeader", str(h.get("RemarksHeader")), "IsApproved", toBool(h.get("IsApproved")),
                "rows", detailRows(id), "sourceDocuments", docs);
    }

    /** DatagridHistory_SelectionChanged -> GetDetailGrdByHeadId. */
    public List<Map<String, Object>> historyDetail(boolean tr, int id) {
        UserAccount u = hrm.user(screen(tr));
        owned(u, tr, id);
        return detailRows(id);
    }

    // ================================================================== save

    /**
     * Insert() of the form: the checks in its order, then logisticRateNegotiationHeader.Save. body: id, docDate,
     * serviceTypeId, loadingPortId / destinationPortId (937) or cityFromId / cityToId (941), transactionCurrencyId,
     * transactionExchangeRate, fcyCurrencyId, fcyExchangeRate, dealStatusId, remarksHeader, rows[], removedRows[],
     * sourceDocuments[], removedSourceDocuments[].
     */
    @SuppressWarnings("unchecked")
    public Map<String, Object> save(boolean tr, Map<String, Object> b) {
        int screen = screen(tr);
        UserAccount u = hrm.user(screen);
        int recId = Math.max(0, toInt(b.get("id")));
        hrm.require(u, screen, recId == 0 ? "Save" : "Update");
        Map<String, Object> stored = recId > 0 ? owned(u, tr, recId) : null;
        List<Map<String, Object>> rows = list(b.get("rows"));
        List<Map<String, Object>> docs = list(b.get("sourceDocuments"));
        if (rows.isEmpty()) throw invalid("Broker Agent Detail Record Not Found");
        if (!tr && docs.isEmpty()) throw invalid("Source Document Detail Record Not Found");

        int yearId = hrm.financialYearId();
        int docNo = stored != null ? toInt(stored.get("documentNo")) : repo.rnGenerateCode(u, yearId, doc(tr));
        if (docNo == 0) throw invalid("Doc No must be a non-zero number");
        Map<String, Object> lk = splitLookups(look.lookAllServices(u));
        int serviceType = toInt(b.get("serviceTypeId"));
        if (serviceType == 0 || !owns((List<Map<String, Object>>) lk.get("serviceTypes"), "Id", serviceType)) throw invalid("Service Type field is required");
        int destination = 0, loading = 0, cityFrom = 0, cityTo = 0;
        if (tr) {
            List<Map<String, Object>> cities = look.cities(u);
            cityFrom = toInt(b.get("cityFromId"));
            if (cityFrom == 0 || !owns(cities, "Id", cityFrom)) throw invalid("City From field is required");
            cityTo = toInt(b.get("cityToId"));
            if (cityTo == 0 || !owns(cities, "Id", cityTo)) throw invalid("City To field is required");
        } else {
            List<Map<String, Object>> ports = look.ports(u);
            destination = toInt(b.get("destinationPortId"));
            if (destination == 0 || !owns(ports, "Id", destination)) throw invalid("Destination Port field is required");
            loading = toInt(b.get("loadingPortId"));
            if (loading == 0 || !owns(ports, "Id", loading)) throw invalid("Loading Port field is required");
        }
        List<Map<String, Object>> currencies = look.currencies(u);
        int tcy = toInt(b.get("transactionCurrencyId"));
        if (tcy == 0 || !owns(currencies, "Id", tcy)) throw invalid("Transaction Currency field is required");
        double tcyRate = toDouble(b.get("transactionExchangeRate"));
        if (tcyRate == 0.0) throw invalid("Tcy Exchange Rate must be a non-zero number");
        int fcy = toInt(b.get("fcyCurrencyId"));
        if (fcy == 0 || !owns(currencies, "Id", fcy)) throw invalid("Foreign Currency field is required");
        double fcyRate = toDouble(b.get("fcyExchangeRate"));
        if (fcyRate == 0.0) throw invalid("Fcy Rate must be a non-zero number");

        Map<Integer, String> parties = new HashMap<>();
        for (Map<String, Object> p : look.partiesGroup10(u)) parties.put(toInt(p.get("Id")), str(p.get("CompanyName")));
        validateBrokerDuplication(rows, parties);                                                          // ValidateBrokerDuplicationInDetailGrid
        if (!tr) validateBrokerAgentMismatch(rows, docs, parties);                                         // ValidateBrokerAgentMismatch

        LocalDateTime now = LocalDateTime.now();
        LgsARateNegotiationHeader h = new LgsARateNegotiationHeader();
        h.logisticRateNegotiationHeaderId = recId;
        h.CompanyId = u.getCompanyId();
        h.BranchesId = toInt(u.getBranchesId());
        h.ProjectsId = toInt(u.getBranchesId());
        h.OrganizationId = u.getOrganizationId();
        h.FinancialYearId = yearId;
        h.documentTypeId = doc(tr);
        h.documentDate = LgsAServiceBillService.pickerValue(b.get("docDate"), stored == null ? null : LgsAServiceBillService.anyDate(stored.get("documentDate")));
        h.documentNo = docNo;
        h.ServiceTypeId = serviceType;
        if (tr) { h.CityFromId = cityFrom; h.CityToId = cityTo; }
        h.fcyCurrencyId = fcy;
        h.fcyExchangeRate = fcyRate;
        h.transactionCurrencyId = tcy;
        h.transactionExchangeRate = tcyRate;
        h.DealStatusId = toInt(b.get("dealStatusId"));
        h.RemarksHeader = str(b.get("remarksHeader"));
        h.EntryUserId = u.getId(); h.EntryDate = now; h.ModifyDate = now; h.ModifyUserId = u.getId();
        h.ApprovedUserId = u.getId(); h.ApprovedDate = now; h.IsApproved = false;
        h.AttachmentsValues = stored == null ? "" : str(stored.get("AttachmentsValues"));
        h.CustomAttachmentsValues = stored == null ? "" : str(stored.get("CustomAttachmentsValues"));
        h.ActionId = recId == 0 ? 1 : 2;                                                                   // BLL Save

        Set<Integer> storedDetailIds = new HashSet<>(), storedDocIds = new HashSet<>();
        if (recId > 0) {
            for (Map<String, Object> d : repo.rnDetail(recId)) storedDetailIds.add(toInt(d.get("logisticRateNegotiationDetailId")));
            for (Map<String, Object> d : repo.rnSourceDocuments(recId)) storedDocIds.add(toInt(d.get("logisticRateNegotiationSourceDocumentId")));
        }
        int dec = fcyRateDecimals(u);
        Set<Integer> items = new HashSet<>();
        for (Map<String, Object> i : look.servicesItems(u)) items.add(toInt(i.get("Id")));
        Map<Integer, Double> equivalents = new HashMap<>();
        for (Map<String, Object> r : (List<Map<String, Object>>) lk.get("rateBases")) equivalents.put(toInt(r.get("Id")), toDouble(r.get("Equivalent")));

        List<LgsARateNegotiationDetail> details = new ArrayList<>();
        if (recId > 0) {
            for (Map<String, Object> r : list(b.get("removedRows"))) {                                     // lstRemoveRecordcustomerDetail
                int did = toInt(r.get("Id"));
                if (did <= 0 || !storedDetailIds.contains(did)) continue;
                LgsARateNegotiationDetail d = detail(tr, r, h, loading, destination, dec, equivalents);
                d.logisticRateNegotiationDetailId = did;
                d.ActionTypeId = 3;
                details.add(d);
            }
        }
        String remarks = h.RemarksHeader;
        for (int i = 0; i < rows.size(); i++) {
            Map<String, Object> r = rows.get(i);
            LgsARateNegotiationDetail d = detail(tr, r, h, loading, destination, dec, equivalents);
            int did = recId != 0 ? toInt(r.get("Id")) : 0;
            if (did > 0 && !storedDetailIds.contains(did)) did = 0;
            d.logisticRateNegotiationDetailId = did;
            d.ActionTypeId = did <= 0 ? 1 : 2;
            field(d.partyIdAgent != 0 && parties.containsKey(d.partyIdAgent), "Broker Agent", i, null);
            field(d.serviceProviderId != 0 && parties.containsKey(d.serviceProviderId), "ServiceProvider", i, null);
            field(d.paymentTermId != 0 && owns((List<Map<String, Object>>) lk.get("paymentTerms"), "Id", d.paymentTermId), "Payment Term", i, null);
            field(d.DueDays != 0, "DueDays", i, null);
            if (!tr) field(d.VesselRoutingTypeId != 0 && owns((List<Map<String, Object>>) lk.get("routingTypes"), "Id", d.VesselRoutingTypeId), "VesselRouting Type", i, null);
            field(d.ServiceItemId != 0 && items.contains(d.ServiceItemId), "Service Item", i, null);
            field(d.tcyCurrencyId != 0 && owns(currencies, "Id", d.tcyCurrencyId), "Transaction Currency", i, null);
            field(d.serviceRate > 0, "Service Rate", i, null);
            field(d.serviceRateBaseUomId != 0 && equivalents.containsKey(d.serviceRateBaseUomId), "Rate Base", i, null);
            field(d.tcyAmount > 0, "Tcy amount", i, null);
            field(d.lcyAmount > 0, "Lcy amount", i, null);
            field(d.OffervalidityDays != 0, "Offer Validity Days", i, null);
            if (remarks == null || remarks.trim().isEmpty()) remarks = d.Remarks;
            details.add(d);
        }
        h.RemarksHeader = remarks;

        Map<Integer, Integer> invoiceTypes = new HashMap<>();
        for (Map<String, Object> inv : look.exportInvoices(u)) invoiceTypes.put(toInt(inv.get("Id")), toInt(inv.get("DocumentTypeId")));
        List<LgsARateNegotiationSourceDocument> sources = new ArrayList<>();
        if (recId > 0) {
            for (Map<String, Object> r : list(b.get("removedSourceDocuments"))) {                         // lstRemoveRecordSourceDocument
                int sid = toInt(r.get("Id"));
                if (sid <= 0 || !storedDocIds.contains(sid)) continue;
                LgsARateNegotiationSourceDocument s = source(r);
                s.logisticRateNegotiationSourceDocumentId = sid;
                s.ActionTypeId = 3;
                sources.add(s);
            }
        }
        for (int i = 0; i < docs.size(); i++) {
            Map<String, Object> r = docs.get(i);
            if (toInt(r.get("DocumentTypeId")) <= 0) continue;                                               // only rows with a document type
            LgsARateNegotiationSourceDocument s = source(r);
            int sid = recId != 0 ? toInt(r.get("Id")) : 0;
            if (sid > 0 && !storedDocIds.contains(sid)) sid = 0;
            s.logisticRateNegotiationSourceDocumentId = sid;
            s.ActionTypeId = sid <= 0 ? 1 : 2;
            Integer invType = invoiceTypes.get(s.SourceDocumentId);
            if (invType != null) s.SourceDocumentTypeId = invType;                                           // DocumentTypeId comes from the invoice row
            field(s.SourceDocumentTypeId != 0, "Document Type", i, "Source Document Detail");
            field(s.SourceDocumentId != 0 && invType != null, "Source Document No", i, "Source Document Detail");
            field(s.BrokerAgentNameId != 0 && parties.containsKey(s.BrokerAgentNameId), "Broker Agent Name", i, "Source Document Detail");
            field(s.ServiceProviderId != 0 && parties.containsKey(s.ServiceProviderId), "Service Provider", i, "Source Document Detail");
            sources.add(s);
        }
        int id = repo.rnSave(h, details, sources);
        String msg = recId > 0 ? "Data Update Successfully....  " + docNo : "Data Save Successfully....  " + docNo;
        return map("success", true, "id", id, "message", msg);
    }

    /** FillDetailListCommonForInsertAndDelete(vd, r). */
    private LgsARateNegotiationDetail detail(boolean tr, Map<String, Object> r, LgsARateNegotiationHeader h, int loading, int destination,
                                             int dec, Map<Integer, Double> equivalents) {
        LgsARateNegotiationDetail d = new LgsARateNegotiationDetail();
        d.serviceTypeId = h.ServiceTypeId;
        d.partyIdAgent = toInt(r.get("BrokerAgentId"));
        d.serviceProviderId = toInt(r.get("ServiceProviderId"));
        if (tr) { d.CityFromId = h.CityFromId; d.CityToId = h.CityToId; }
        else { d.portOfDischargeId = destination; d.portOfLoadingId = loading; }
        d.paymentTermId = toInt(r.get("PaymentTermId"));
        d.DueDays = toInt(r.get("DueDays"));
        if (!tr) {
            d.VesselRoutingTypeId = toInt(r.get("VesselRoutingTypeId"));
            d.VesselRoutingDescription = str(r.get("VesselRoutingDescription"));
        }
        d.ServiceItemId = toInt(r.get("ItemId"));
        d.tcyCurrencyId = toInt(r.get("TransactionCurrencyId"));
        d.tcyExchangeRate = h.transactionExchangeRate;
        d.serviceRate = toDouble(r.get("ServiceRate"));
        d.serviceRateBaseUomId = toInt(r.get("RateBaseId"));
        /* CalculateAmountsInDetail: Tcy = ServiceRate x the rate base's Equivalent, Lcy = Tcy x exchange rate (0 when the
           rate is not > 0), each shown with DecimalFCYRateFormate and read back. */
        double eq = equivalents.getOrDefault(d.serviceRateBaseUomId, 0.0);
        double tcyAmount = round(d.serviceRate * eq, dec);
        d.tcyAmount = tcyAmount;
        d.lcyAmount = h.transactionExchangeRate > 0 ? round(tcyAmount * h.transactionExchangeRate, dec) : 0.0;
        d.fcyAmount = h.fcyExchangeRate > 0.0 ? d.lcyAmount / h.fcyExchangeRate : 0.0;
        d.OffervalidityDays = toInt(r.get("OfferValidityDays"));
        LocalDateTime now = LocalDateTime.now();
        d.OfferValidityDate = orNow(LgsAServiceBillService.anyDate(r.get("OfferValidityDate")));
        if (tr) {
            d.cutOffDate = now;                                                                       // transporter form: DateTime.Now
            d.etdPOL = now;
        } else {
            d.cutOffDate = orNow(LgsAServiceBillService.anyDate(r.get("CargoCutOffDate")));
            d.etdPOL = orNow(LgsAServiceBillService.anyDate(r.get("EtdSealingDate")));
            d.vesselTransitDays = toInt(r.get("TransitsTimeDays"));
            d.freeDaysAtPOD = toInt(r.get("FreeDaysAtPOD"));
        }
        d.Remarks = str(r.get("Remarks"));
        return d;
    }

    /** FillSourceDocumentDetailListCommonForInsertAndDelete(vd, r) - NoOfContainers / MTon are not stored (virtual). */
    private static LgsARateNegotiationSourceDocument source(Map<String, Object> r) {
        LgsARateNegotiationSourceDocument s = new LgsARateNegotiationSourceDocument();
        s.SourceDocumentTypeId = toInt(r.get("DocumentTypeId"));
        s.SourceDocumentId = toInt(r.get("DocumentId"));
        s.BrokerAgentNameId = toInt(r.get("BrokerAgentId"));
        s.ServiceProviderId = toInt(r.get("ServiceProviderId"));
        s.Remarks = str(r.get("Remarks"));
        return s;
    }

    /** ValidateBrokerDuplicationInDetailGrid: one line per duplicated agent, rows numbered from 1. */
    static void validateBrokerDuplication(List<Map<String, Object>> rows, Map<Integer, String> parties) {
        Map<Integer, List<Integer>> seen = new LinkedHashMap<>();
        for (int i = 0; i < rows.size(); i++) {
            int a = toInt(rows.get(i).get("BrokerAgentId"));
            if (a > 0) seen.computeIfAbsent(a, k -> new ArrayList<>()).add(i + 1);
        }
        List<String> lines = new ArrayList<>();
        for (Map.Entry<Integer, List<Integer>> e : seen.entrySet()) {
            if (e.getValue().size() < 2) continue;
            List<String> nos = new ArrayList<>();
            for (Integer n : e.getValue()) nos.add(String.valueOf(n));
            lines.add("BrokerAgent '" + parties.getOrDefault(e.getKey(), "") + "' is duplicated in row(s): " + String.join(", ", nos) + " in Broker Detail");
        }
        if (!lines.isEmpty()) throw invalid(String.join("\n", lines));
    }

    /** ValidateBrokerAgentMismatch(dtCustomerDetail, dtSourceDocument) with its default grid names and checkSpecific 0. */
    static void validateBrokerAgentMismatch(List<Map<String, Object>> rows, List<Map<String, Object>> docs, Map<Integer, String> parties) {
        Set<Integer> target = new java.util.LinkedHashSet<>(), source = new java.util.LinkedHashSet<>();
        Map<Integer, String> targetNames = new HashMap<>();
        for (Map<String, Object> r : rows) {
            int a = toInt(r.get("BrokerAgentId"));
            if (a > 0) { target.add(a); targetNames.putIfAbsent(a, str(r.get("BrokerAgent"))); }
        }
        List<Integer> sourceOrder = new ArrayList<>();
        for (Map<String, Object> r : docs) {
            int a = toInt(r.get("BrokerAgentId"));
            if (a > 0) { source.add(a); sourceOrder.add(a); }
        }
        List<String> missingInTarget = new ArrayList<>();
        for (Integer a : sourceOrder) {
            String n = parties.getOrDefault(a, "");
            if (!target.contains(a) && !missingInTarget.contains(n)) missingInTarget.add(n);
        }
        if (!missingInTarget.isEmpty()) throw invalid("BrokerAgent '" + String.join(", ", missingInTarget)
                + "' found in 'Source Document Detail' Grid but not in 'Broker agent Detail' Grid.");
        List<String> missingInSource = new ArrayList<>();
        for (int i = 0; i < rows.size(); i++) {
            int a = toInt(rows.get(i).get("BrokerAgentId"));
            if (a <= 0) continue;
            String n = str(rows.get(i).get("BrokerAgent"));
            if (!source.contains(a) && !missingInSource.contains(n)) missingInSource.add(n);
        }
        if (!missingInSource.isEmpty()) throw invalid("BrokerAgent " + String.join(", '", missingInSource)
                + "' found in 'Broker agent Detail' Grid but not in 'Source Document Detail' Grid.");
    }

    /** FormHelper.ValidateField: "{field} is required in {GridName} at row No: {rowIndex + 1}" (GridName default "Detail Grid"). */
    static void field(boolean ok, String name, int rowIndex, String grid) {
        if (!ok) throw invalid(name + " is required in " + (grid == null ? "Detail Grid" : grid) + " at row No: " + (rowIndex + 1));
    }

    @SuppressWarnings("unchecked")
    static List<Map<String, Object>> list(Object o) {
        return o instanceof List ? (List<Map<String, Object>>) o : new ArrayList<>();
    }

    static LocalDateTime orNow(LocalDateTime d) { return d == null ? LocalDateTime.now() : d; }

    /** double.ToString("#,#0." + N zeros) read back by Conversion.ToDouble (MidpointRounding.AwayFromZero). */
    static double round(double v, int places) { return BigDecimal.valueOf(v).setScale(places, RoundingMode.HALF_UP).doubleValue(); }

    // ================================================================== delete / print

    /** btnDelete_Click: "No record found to Delete" without a record; DeleteByID(UserAccount.ID, RecId). */
    public Map<String, Object> delete(boolean tr, int id) {
        int screen = screen(tr);
        UserAccount u = hrm.user(screen);
        hrm.require(u, screen, "Delete");
        if (id <= 0) throw invalid("No record found to Delete");
        owned(u, tr, id);
        repo.rnDelete(u.getId(), id);
        return map("success", true, "message", "Delete Record Successfully");
    }

    /** LogisticRateNegotiationSlip(PrintId): the Print right and a document of this company. */
    public Map<String, Object> slipCheck(boolean tr, int id) {
        int screen = screen(tr);
        UserAccount u = hrm.user(screen);
        hrm.require(u, screen, "Print");
        if (id <= 0) throw invalid("No Record Found For Display");
        owned(u, tr, id);
        return map("id", id);
    }
}
