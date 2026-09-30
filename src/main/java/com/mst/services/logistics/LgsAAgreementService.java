package com.mst.services.logistics;

import com.mst.models.UserAccount;
import com.mst.models.logistics.LgsAAgreementDetail;
import com.mst.models.logistics.LgsAAgreementHeader;
import com.mst.repositories.logistics.LgsALookupRepository;
import com.mst.repositories.logistics.LgsARateAgreementRepository;
import com.mst.services.hrm.HrmSupport;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.Set;

import static com.mst.services.hrm.HrmSupport.*;
import static com.mst.services.logistics.LgsARateNegotiationService.field;
import static com.mst.services.logistics.LgsARateNegotiationService.list;
import static com.mst.services.logistics.LgsARateNegotiationService.orNow;
import static com.mst.services.logistics.LgsARateNegotiationService.round;

/**
 * BLL of 939 "Logistics Agreement" - Architecture.WinApp.Service.lgstcm/frmLogisticAgreement.cs (ScreenName
 * "frmLogisticAgreement", DocumentTypeId 1301). Validation order and texts are the form's.
 */
@Service
public class LgsAAgreementService {

    public static final int SCREEN = 939;
    public static final int DOC = 1301;

    @Autowired private LgsARateAgreementRepository repo;
    @Autowired private LgsALookupRepository look;
    @Autowired private LgsARateNegotiationService rn;
    @Autowired private HrmSupport hrm;

    /** HistoryComboBind: ServiceType / ServiceProvider rows of USP_GetDataForDropDownFromAgreementHeader. */
    private Map<String, Object> historyCombos(UserAccount u) {
        List<Map<String, Object>> st = new ArrayList<>(), sp = new ArrayList<>();
        for (Map<String, Object> r : repo.agDropDown(u)) {
            String a = str(r.get("Activity"));
            if ("ServiceType".equals(a)) st.add(map("Id", toInt(r.get("Id")), "Name", str(r.get("ReferenceName"))));
            else if ("ServiceProvider".equals(a)) sp.add(map("Id", toInt(r.get("Id")), "Name", str(r.get("ReferenceName"))));
        }
        return map("serviceTypes", st, "serviceProviders", sp);
    }

    /**
     * AllLookUpComboBind binds ServiceType and RateBaseUom only; CurrencyBindFromGlobal, the parties (customer group 10)
     * for Bill To Party and Broker Agent / Service Provider, services items, GetConfigurationsFromGlobal(andBind).
     */
    private Map<String, Object> lists(UserAccount u) {
        Map<String, Object> lk = LgsARateNegotiationService.splitLookups(look.lookAllServices(u));
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("serviceTypes", lk.get("serviceTypes"));
        out.put("rateBases", lk.get("rateBases"));
        out.put("currencies", look.currencies(u));
        out.put("parties", look.partiesGroup10(u));
        out.put("items", look.servicesItems(u));
        out.put("config", map("DefaultDaysToLessFromHistoryFromDate", toInt(look.config(u, "DefaultDaysToLessFromHistoryFromDate")),
                "ForeignBaseCurrency", toInt(look.config(u, "ForeignBaseCurrency")),
                "FcyBaseCurrencyRate", toDouble(look.config(u, "FcyBaseCurrencyRate")),
                "FcyRateDecimals", rn.fcyRateDecimals(u)));
        return out;
    }

    public Map<String, Object> setup() {
        UserAccount u = hrm.user(SCREEN);
        Map<String, Object> out = lists(u);
        out.put("rights", rn.rights(u, SCREEN));
        out.put("docNo", repo.agGenerateCode(u, hrm.financialYearId(), DOC));
        out.put("history", historyCombos(u));
        out.put("financialYearStart", look.financialYearStart(u, hrm.financialYearId()));
        return out;
    }

    public Map<String, Object> refresh() { return lists(hrm.user(SCREEN)); }

    public Map<String, Object> refreshHistory() { return historyCombos(hrm.user(SCREEN)); }

    public Map<String, Object> docNo() {
        UserAccount u = hrm.user(SCREEN);
        return map("docNo", repo.agGenerateCode(u, hrm.financialYearId(), DOC));
    }

    /**
     * HistoryGridFill: FormHistory with the ticked date kind (the Approved branch sends only @ApprovedFromDate - the
     * BLL has no @ApprovedToDate), doc range when != 0, ServiceTypeId / ServiceProviderId when != 0.
     */
    public List<Map<String, Object>> history(Map<String, String> q) {
        UserAccount u = hrm.user(SCREEN);
        Map<String, Object> f = LgsARateNegotiationService.dateFilters(q, false);
        if (toInt(q.get("fromDocNo")) != 0) f.put("FromDocNo", toInt(q.get("fromDocNo")));
        if (toInt(q.get("toDocNo")) != 0) f.put("ToDocNo", toInt(q.get("toDocNo")));
        if (toInt(q.get("serviceTypeId")) != 0) f.put("ServiceTypeId", toInt(q.get("serviceTypeId")));
        if (toInt(q.get("serviceProviderId")) != 0) f.put("ServiceProviderId", toInt(q.get("serviceProviderId")));
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.agHistory(u, hrm.financialYearId(), hrm.can(u, SCREEN, "CanView AllRecord"), f)) {
            out.add(map("Id", toInt(r.get("AgreementHeaderId")), "DocumentTypeId", toInt(r.get("DocumentTypeId")),
                    "DocumentNo", toInt(r.get("DocumentNo")), "DocumentDate", r.get("DocumentDate"), "ServiceTypeId", toInt(r.get("serviceTypeId")),
                    "ServiceType", str(r.get("ServiceType")), "BillToPartyId", toInt(r.get("BrokerAgentId")), "BillToPartyName", str(r.get("BrokerAgentName")),
                    "ServiceProviderId", toInt(r.get("serviceProviderId")), "BrokerAgentOrServiceProviderName", str(r.get("ServiceProviderName")),
                    "TransactionCurrencyId", toInt(r.get("transactionCurrencyId")), "TcyCode", str(r.get("TcyCode")),
                    "TransactionExchangeRate", r.get("transactionExchangeRate"), "FcyCurrencyId", toInt(r.get("fcyCurrencyId")),
                    "FcyCode", str(r.get("FcyCode")), "FcyExchangeRate", r.get("fcyExchangeRate"), "EffectiveDateFrom", r.get("effectiveDateFrom"),
                    "EffectiveDateTo", r.get("effectiveDateTo"), "IsActive", toBool(r.get("isActive")), "RemarksHeader", str(r.get("RemarksHeader")),
                    "EntryDate", r.get("EntryDate"), "EntryUserName", str(r.get("EntryUserName")), "ModifyDate", r.get("ModifyDate"),
                    "ModifyUserName", str(r.get("ModifyUserName")), "IsApproved", toBool(r.get("IsApproved")), "ApprovedDate", r.get("ApprovedDate"),
                    "ApprovalUserName", str(r.get("ApprovalUserName")), "NoOfAttachments", toInt(r.get("NoOfAttachments"))));
        }
        return out;
    }

    private Map<String, Object> owned(UserAccount u, int id) {
        List<Map<String, Object>> h = id > 0 ? repo.agHeader(id) : new ArrayList<>();
        if (h.isEmpty()) throw invalid("No Record Found");
        Map<String, Object> r = h.get(0);
        if (toInt(r.get("OrganizationId")) != u.getOrganizationId() || toInt(r.get("CompanyId")) != u.getCompanyId()
                || toInt(r.get("DocumentTypeId")) != DOC) throw invalid("No Record Found");
        return r;
    }

    /** FillDetailFromListCommonForReadById. */
    private List<Map<String, Object>> detailRows(int id) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> d : repo.agDetail(id)) {
            out.add(map("Id", toInt(d.get("agreementDetailId")), "PartyRefDocNo", str(d.get("partyRefDocNo")), "PartyRefDocDate", d.get("partyRefDocDate"),
                    "ItemId", toInt(d.get("serviceItemId")), "ItemCode", str(d.get("ItemCode")), "ItemName", str(d.get("ItemName")),
                    "TransactionCurrencyId", toInt(d.get("transactionCurrencyId")), "TransactionCurrency", str(d.get("TransactionCurrencyCode")),
                    "ServiceRate", toDouble(d.get("serviceRate")), "RateBaseId", toInt(d.get("serviceRateBaseUomId")), "RateBase", str(d.get("RateBaseUom")),
                    "RateBaseEquivalent", toDouble(d.get("RateBaseUomEquivalent")), "TcyAmount", toDouble(d.get("tcyAmount")),
                    "LcyAmount", toDouble(d.get("lcyAmount")), "Remarks", str(d.get("Remarks"))));
        }
        return out;
    }

    /** ReadById(Id). The Edit path needs the Update right ("you don't have update rights..."); Save As does not. */
    public Map<String, Object> read(int id, boolean edit) {
        UserAccount u = hrm.user(SCREEN);
        if (edit && !hrm.can(u, SCREEN, "Update")) throw invalid("you don't have update rights...");
        Map<String, Object> h = owned(u, id);
        return map("Id", toInt(h.get("AgreementHeaderId")), "DocumentNo", toInt(h.get("DocumentNo")), "DocumentDate", h.get("DocumentDate"),
                "serviceTypeId", toInt(h.get("serviceTypeId")), "BrokerAgentId", toInt(h.get("BrokerAgentId")),
                "serviceProviderId", toInt(h.get("serviceProviderId")), "transactionCurrencyId", toInt(h.get("transactionCurrencyId")),
                "transactionExchangeRate", toDouble(h.get("transactionExchangeRate")), "fcyCurrencyId", toInt(h.get("fcyCurrencyId")),
                "fcyExchangeRate", toDouble(h.get("fcyExchangeRate")), "effectiveDateFrom", h.get("effectiveDateFrom"),
                "effectiveDateTo", h.get("effectiveDateTo"), "isActive", toBool(h.get("isActive")), "RemarksHeader", str(h.get("RemarksHeader")),
                "rows", detailRows(id));
    }

    public List<Map<String, Object>> historyDetail(int id) {
        UserAccount u = hrm.user(SCREEN);
        owned(u, id);
        return detailRows(id);
    }

    /**
     * Insert(): "Detail Record Not Found", ValidateControls (Doc No, Service Type, Bill To Party, Broker Agent Or Service
     * Provider, Transaction Currency, Tcy Exchange Rate, Foreign Currency, Fcy Rate), then per row ValidateField and
     * AgreementHeader.Save.
     */
    @SuppressWarnings("unchecked")
    public Map<String, Object> save(Map<String, Object> b) {
        UserAccount u = hrm.user(SCREEN);
        int recId = Math.max(0, toInt(b.get("id")));
        hrm.require(u, SCREEN, recId == 0 ? "Save" : "Update");
        Map<String, Object> stored = recId > 0 ? owned(u, recId) : null;
        List<Map<String, Object>> rows = list(b.get("rows"));
        if (rows.isEmpty()) throw invalid("Detail Record Not Found");
        int yearId = hrm.financialYearId();
        int docNo = stored != null ? toInt(stored.get("DocumentNo")) : repo.agGenerateCode(u, yearId, DOC);
        if (docNo == 0) throw invalid("Doc No must be a non-zero number");
        Map<String, Object> lk = LgsARateNegotiationService.splitLookups(look.lookAllServices(u));
        int serviceType = toInt(b.get("serviceTypeId"));
        if (serviceType == 0 || !owns((List<Map<String, Object>>) lk.get("serviceTypes"), "Id", serviceType)) throw invalid("Service Type field is required");
        Set<Integer> parties = new HashSet<>();
        for (Map<String, Object> p : look.partiesGroup10(u)) parties.add(toInt(p.get("Id")));
        int billTo = toInt(b.get("brokerAgentId"));
        if (billTo == 0 || !parties.contains(billTo)) throw invalid("Bill To Party field is required");
        int provider = toInt(b.get("serviceProviderId"));
        if (provider == 0 || !parties.contains(provider)) throw invalid("Broker Agent Or Service Provider field is required");
        List<Map<String, Object>> currencies = look.currencies(u);
        int tcy = toInt(b.get("transactionCurrencyId"));
        if (tcy == 0 || !owns(currencies, "Id", tcy)) throw invalid("Transaction Currency field is required");
        double tcyRate = toDouble(b.get("transactionExchangeRate"));
        if (tcyRate == 0.0) throw invalid("Tcy Exchange Rate must be a non-zero number");
        int fcy = toInt(b.get("fcyCurrencyId"));
        if (fcy == 0 || !owns(currencies, "Id", fcy)) throw invalid("Foreign Currency field is required");
        double fcyRate = toDouble(b.get("fcyExchangeRate"));
        if (fcyRate == 0.0) throw invalid("Fcy Rate must be a non-zero number");

        LocalDateTime now = LocalDateTime.now();
        LgsAAgreementHeader h = new LgsAAgreementHeader();
        h.AgreementHeaderId = recId;
        h.CompanyId = u.getCompanyId();
        h.BranchesId = toInt(u.getBranchesId());
        h.ProjectsId = toInt(u.getBranchesId());
        h.OrganizationId = u.getOrganizationId();
        h.FinancialYearId = yearId;
        h.DocumentTypeId = DOC;
        h.DocumentDate = LgsAServiceBillService.pickerValue(b.get("docDate"), stored == null ? null : LgsAServiceBillService.anyDate(stored.get("DocumentDate")));
        h.DocumentNo = docNo;
        h.serviceTypeId = serviceType;
        h.BrokerAgentId = billTo;
        h.serviceProviderId = provider;
        h.fcyCurrencyId = fcy;
        h.fcyExchangeRate = fcyRate;
        h.transactionCurrencyId = tcy;
        h.transactionExchangeRate = tcyRate;
        h.effectiveDateFrom = orNow(LgsAServiceBillService.anyDate(b.get("effectiveDateFrom")));
        h.effectiveDateTo = orNow(LgsAServiceBillService.anyDate(b.get("effectiveDateTo")));
        h.isActive = toBool(b.get("isActive"));
        h.RemarksHeader = str(b.get("remarksHeader"));
        h.EntryUserId = u.getId(); h.EntryDate = now; h.ModifyDate = now; h.ModifyUserId = u.getId();
        h.ApprovedUserId = u.getId(); h.ApprovedDate = now; h.IsApproved = false;
        h.AttachmentsValues = stored == null ? "" : str(stored.get("AttachmentsValues"));
        h.CustomAttachmentsValues = stored == null ? "" : str(stored.get("CustomAttachmentsValues"));
        h.ActionId = recId == 0 ? 1 : 2;                                                                   // BLL Save

        Set<Integer> storedIds = new HashSet<>();
        if (recId > 0) for (Map<String, Object> d : repo.agDetail(recId)) storedIds.add(toInt(d.get("agreementDetailId")));
        int dec = rn.fcyRateDecimals(u);
        Set<Integer> items = new HashSet<>();
        for (Map<String, Object> i : look.servicesItems(u)) items.add(toInt(i.get("Id")));
        Map<Integer, Double> equivalents = new HashMap<>();
        for (Map<String, Object> r : (List<Map<String, Object>>) lk.get("rateBases")) equivalents.put(toInt(r.get("Id")), toDouble(r.get("Equivalent")));

        List<LgsAAgreementDetail> details = new ArrayList<>();
        if (recId > 0) {
            for (Map<String, Object> r : list(b.get("removedRows"))) {                                     // lstRemoveRecordcustomerDetail
                int did = toInt(r.get("Id"));
                if (did <= 0 || !storedIds.contains(did)) continue;
                LgsAAgreementDetail d = detail(r, h, dec, equivalents);
                d.agreementDetailId = did;
                d.ActionTypeId = 3;
                details.add(d);
            }
        }
        String remarks = h.RemarksHeader;
        for (int i = 0; i < rows.size(); i++) {
            Map<String, Object> r = rows.get(i);
            LgsAAgreementDetail d = detail(r, h, dec, equivalents);
            int did = recId != 0 ? toInt(r.get("Id")) : 0;
            if (did > 0 && !storedIds.contains(did)) did = 0;
            d.agreementDetailId = did;
            d.ActionTypeId = did <= 0 ? 1 : 2;
            field(d.partyRefDocNo != null && !d.partyRefDocNo.trim().isEmpty(), "Party Ref DocNo", i, null);
            field(d.serviceItemId != 0 && items.contains(d.serviceItemId), "Service Item", i, null);
            field(d.transactionCurrencyId != 0 && owns(currencies, "Id", d.transactionCurrencyId), "Transaction Currency", i, null);
            field(d.serviceRate > 0, "Service Rate", i, null);
            field(d.serviceRateBaseUomId != 0 && equivalents.containsKey(d.serviceRateBaseUomId), "Rate Base", i, null);
            field(d.tcyAmount > 0, "Tcy amount", i, null);
            field(d.lcyAmount > 0, "Lcy amount", i, null);
            if (remarks == null || remarks.trim().isEmpty()) remarks = d.Remarks;
            details.add(d);
        }
        h.RemarksHeader = remarks;
        int id = repo.agSave(h, details);
        String msg = recId > 0 ? "Data Update Successfully....  " + docNo : "Data Save Successfully....  " + docNo;
        return map("success", true, "id", id, "message", msg);
    }

    /** FillDetailListCommonForInsertAndDelete(vd, r): header effective dates / active flag on every row. */
    private LgsAAgreementDetail detail(Map<String, Object> r, LgsAAgreementHeader h, int dec, Map<Integer, Double> equivalents) {
        LgsAAgreementDetail d = new LgsAAgreementDetail();
        d.partyRefDocNo = str(r.get("PartyRefDocNo"));
        d.partyRefDocDate = orNow(LgsAServiceBillService.anyDate(r.get("PartyRefDocDate")));
        d.serviceItemId = toInt(r.get("ItemId"));
        d.transactionCurrencyId = toInt(r.get("TransactionCurrencyId"));
        d.transactionExchangeRate = h.transactionExchangeRate;
        d.serviceRate = toDouble(r.get("ServiceRate"));
        d.serviceRateBaseUomId = toInt(r.get("RateBaseId"));
        double eq = equivalents.getOrDefault(d.serviceRateBaseUomId, 0.0);
        double tcyAmount = round(d.serviceRate * eq, dec);                           // CalculateAmountsInDetail
        d.tcyAmount = tcyAmount;
        d.lcyAmount = h.transactionExchangeRate > 0 ? round(tcyAmount * h.transactionExchangeRate, dec) : 0.0;
        d.fcyAmount = h.fcyExchangeRate > 0.0 ? d.lcyAmount / h.fcyExchangeRate : 0.0;
        d.effectiveDateFrom = h.effectiveDateFrom;
        d.effectiveDateTo = h.effectiveDateTo;
        d.isActive = h.isActive;
        d.Remarks = str(r.get("Remarks"));
        return d;
    }

    public Map<String, Object> delete(int id) {
        UserAccount u = hrm.user(SCREEN);
        hrm.require(u, SCREEN, "Delete");
        if (id <= 0) throw invalid("No record found to Delete");
        owned(u, id);
        repo.agDelete(u.getId(), id);
        return map("success", true, "message", "Delete Record Successfully");
    }

    /** AgreementHeader_Slip(PrintId): Print right and a document of this company. */
    public Map<String, Object> slipCheck(int id) {
        UserAccount u = hrm.user(SCREEN);
        hrm.require(u, SCREEN, "Print");
        if (id <= 0) throw invalid("No Record Found For Display");
        owned(u, id);
        return map("id", id);
    }
}
