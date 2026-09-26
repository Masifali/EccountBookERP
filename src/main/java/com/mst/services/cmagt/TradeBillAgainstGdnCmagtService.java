package com.mst.services.cmagt;

import com.mst.models.cmagt.dto.TradeBillAgainstGdnCmagtDto;
import com.mst.repositories.cmagt.TradeBillAgainstGdnCmagtRepository;
import com.mst.security.CurrentUserContext;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

@Service
public class TradeBillAgainstGdnCmagtService {

    @Autowired
    private TradeBillAgainstGdnCmagtRepository repository;

    @Autowired
    private CurrentUserContext currentUserContext;

    /**
     * Organization, company, branch, financial year and the user come from the SESSION, never
     * from the request body.
     *
     * They used to be read straight off the posted JSON and, when absent, defaulted to 1 in the
     * repository. Two separate faults in one line: a crafted payload could file this document
     * against another company, and an omitted value silently filed it against company 1 - the
     * same fabricated default that made the Purchase Order screen read company 1 and show PO-1
     * where the desktop showed PO-493. entryUserId/modifyUserId are the sharper edge: those are
     * authorship, and a client must never choose them.
     *
     * The desktop reads UserAccount and clsGlobalVariables.ActiveYr for exactly these five and
     * gives the operator no way to override either.
     */
    public Map<String, Object> saveOrUpdate(TradeBillAgainstGdnCmagtDto dto) {
        dto.setOrganizationId(currentUserContext.currentOrganizationId());
        dto.setCompanyId(currentUserContext.currentCompanyId());
        dto.setBranchId(currentUserContext.currentBranchId());
        dto.setFinancialYearId(currentUserContext.currentFinancialYearId());
        dto.setEntryUserId(currentUserContext.currentUserId());
        dto.setModifyUserId(currentUserContext.currentUserId());
        return repository.saveOrUpdate(dto);
    }

    /** frmCommissionAgentTradeBillAgainstGdn line 882. */
    public static final int DOCUMENT_TYPE_ID = 1056;

    /** frmCommissionAgentTradeBillAgainstGdn line 883 - the key its rights are resolved against. */
    public static final String DESKTOP_SCREEN_NAME = "frmCommissionAgentTradeBillAgainstGdn";

    /* Right names exactly as CommonServices.SetRightsValueInRightsObject compares them
       (Architecture.WinApp.Common/CommonServices.cs 17596-17660) - note the space. */
    private static final String RIGHT_CAN_VIEW_ALL_RECORDS = "CanView AllRecord";
    private static final String RIGHT_DELETE = "Delete";

    @Autowired
    private com.mst.repositories.cmagt.SaleOrderCmagtRepository rightsRepo;

    /**
     * HistoryGridFill (5943-6039): org, company, branch, year and DocumentTypeId from the
     * session; CanViewAllRecord from the screen's rights; EntryUser pinned to the current user
     * when that right is absent.
     */
    public List<Map<String, Object>> getHistory(String fromDate, String toDate,
                                                Integer docNoFrom, Integer docNoTo,
                                                Integer tradingAccountId, Integer supplierId,
                                                Integer customerId) {
        boolean all = hasRight(RIGHT_CAN_VIEW_ALL_RECORDS);
        return repository.formHistory(
                currentUserContext.currentOrganizationId(),
                currentUserContext.currentCompanyId(),
                currentUserContext.currentBranchId(),
                currentUserContext.currentFinancialYearId(),
                DOCUMENT_TYPE_ID, all,
                all ? null : currentUserContext.currentUserId(),
                fromDate, toDate, docNoFrom, docNoTo, tradingAccountId, supplierId, customerId);
    }

    /**
     * ReadById + the nine child collections. The procedure reads by @Id alone; the desktop can
     * only reach ids its own company's history returned, the web can be sent any id - so the
     * row's OrganizationId/CompanyId/DocumentTypeId are checked against the session, and a
     * user without "CanView AllRecord" may open only their own bills (the same set History
     * shows them).
     */
    public Map<String, Object> getById(Integer id) {
        Map<String, Object> result = new LinkedHashMap<>();
        Map<String, Object> header = id == null ? null : repository.readById(id);
        if (header == null || !belongsToSession(header)) {
            result.put("status", "ERROR");
            result.put("message", "Record not found");
            return result;
        }
        if (!hasRight(RIGHT_CAN_VIEW_ALL_RECORDS)
                && toInt(header.get("EnteryUserId")) != currentUserContext.currentUserId()) {
            result.put("status", "ERROR");
            result.put("message", "You do not have permission to open Trade Bills entered by another user.");
            return result;
        }
        result.put("status", "SUCCESS");
        result.put("data", header);
        return result;
    }

    /** DocumentNoDbCall (1149) and BranchSrNoDbCall (1175). */
    public Map<String, Object> generateCodes() {
        int org = currentUserContext.currentOrganizationId();
        int co = currentUserContext.currentCompanyId();
        int yr = currentUserContext.currentFinancialYearId();
        int br = currentUserContext.currentBranchId();
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("docNo", repository.generateCode("GenerateCode", org, co, DOCUMENT_TYPE_ID, yr, br));
        r.put("branchSrNo", repository.generateCode("GenerateBranchSrCode", org, co, DOCUMENT_TYPE_ID, yr, br));
        return r;
    }

    /**
     * BtnDelete_Click (5843-5864) -> BLL DeleteByID(UserAccount.ID, RecId), inside one
     * transaction as the BLL opens one. The button is enabled by formright.DoHaveCanDelete (953).
     */
    @org.springframework.transaction.annotation.Transactional
    public Map<String, Object> deleteById(Integer id) {
        Map<String, Object> result = new LinkedHashMap<>();
        if (id == null || id == 0) {
            result.put("status", "ERROR");
            result.put("message", "Record Id Not Found");
            return result;
        }
        if (!hasRight(RIGHT_DELETE)) {
            result.put("status", "ERROR");
            result.put("message", "You do not have Delete rights on this screen.");
            return result;
        }
        Map<String, Object> header = repository.readById(id);
        if (header == null || !belongsToSession(header)) {
            result.put("status", "ERROR");
            result.put("message", "Record not found");
            return result;
        }
        repository.deleteById(currentUserContext.currentUserId(), id);
        result.put("status", "SUCCESS");
        return result;
    }

    private boolean belongsToSession(Map<String, Object> h) {
        return toInt(h.get("OrganizationId")) == currentUserContext.currentOrganizationId()
                && toInt(h.get("CompanyId")) == currentUserContext.currentCompanyId()
                && toInt(h.get("DocumentTypeId")) == DOCUMENT_TYPE_ID;
    }

    /**
     * SetRightsValueInRightsObject: role "Admin" starts with every right; then each row of the
     * user's grant grid overrides. For "CanView AllRecord" an Admin stays true; for "Delete"
     * the row wins even for Admin (the desktop has no Admin guard on that branch).
     */
    private boolean hasRight(String rightName) {
        String role = currentUserContext.currentRoleName();
        boolean admin = "Admin".equalsIgnoreCase(role) || "Administrator".equalsIgnoreCase(role);
        boolean value = admin;
        try {
            for (Map<String, Object> r : rightsRepo.userRightsForScreen(
                    currentUserContext.currentUserId(), DESKTOP_SCREEN_NAME, role,
                    currentUserContext.currentCompanyId())) {
                Object name = pick(r, "RightName");
                if (name != null && rightName.equalsIgnoreCase(name.toString().trim())) {
                    if (admin && RIGHT_CAN_VIEW_ALL_RECORDS.equals(rightName)) return true;
                    value = toBool(pick(r, "Value"));
                    break;
                }
            }
        } catch (Exception ignored) {
            // An unreadable grant grid must not become an implicit grant.
            return admin && RIGHT_CAN_VIEW_ALL_RECORDS.equals(rightName);
        }
        return value;
    }

    private static Object pick(Map<String, Object> row, String key) {
        if (row.containsKey(key)) return row.get(key);
        for (Map.Entry<String, Object> e : row.entrySet())
            if (e.getKey().equalsIgnoreCase(key)) return e.getValue();
        return null;
    }

    private static boolean toBool(Object v) {
        if (v instanceof Boolean) return (Boolean) v;
        if (v instanceof Number) return ((Number) v).intValue() != 0;
        return v != null && ("1".equals(v.toString().trim()) || "true".equalsIgnoreCase(v.toString().trim()));
    }

    private static int toInt(Object v) {
        if (v instanceof Number) return ((Number) v).intValue();
        try { return v == null ? 0 : Integer.parseInt(v.toString().trim()); } catch (Exception e) { return 0; }
    }
}
