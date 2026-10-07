package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.AcRpt1Repository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import java.time.LocalDate;
import java.util.*;
import org.springframework.stereotype.Service;

/**
 * Group R1 - Account Reports 48 / 63 / 70 / 68. Tenancy (Organization / Company / User / Financial Year) always comes from
 * CurrentUserContext, rights are the desktop's ScreenId "View" right (DesktopReportRights). Nothing here edits a shared service.
 */
@Service
public class AcRpt1Service {
    /** dbo.ScreenDefinition ids: Payables, PayablesByDueDate, ReceiveablesByDueDate, ReceivablesByDueDatesNew. */
    public static final int S_PAYABLES = 48, S_PAYABLES_DUE = 63, S_RECEIVABLES_DUE = 70, S_RECEIVABLES_NEW = 68;

    private final AcRpt1Repository repository;
    private final CurrentUserContext context;
    private final DesktopReportRights rights;

    public AcRpt1Service(AcRpt1Repository repository, CurrentUserContext context, DesktopReportRights rights) {
        this.repository = repository;
        this.context = context;
        this.rights = rights;
    }

    private UserAccount user(int screen) {
        UserAccount u = context.requireAccountingUser();
        rights.require(u, screen, "View");
        return u;
    }

    // 48
    public Map<String, Object> payablesLookups() { UserAccount u = user(S_PAYABLES); return repository.payablesLookups(u, context.currentFinancialYearId()); }

    public List<Map<String, Object>> payables(LocalDate from, LocalDate to, int actionId, int cityId, String branchesIds, String controlAccountIds,
            int customGroupId, String customerGroupIds, double balanceFrom, double balanceTo, int showAssetLiability) {
        UserAccount u = user(S_PAYABLES);
        return repository.payables(u, context.currentFinancialYearId(), from, to, actionId, cityId, branchesIds, controlAccountIds, customGroupId,
                customerGroupIds, balanceFrom, balanceTo, showAssetLiability);
    }

    // 63
    public Map<String, Object> payablesDueLookups() { UserAccount u = user(S_PAYABLES_DUE); return repository.payablesDueLookups(u, context.currentFinancialYearId()); }
    public Map<String, Object> payablesDueRefresh() { return repository.payablesDueRefresh(user(S_PAYABLES_DUE)); }
    public List<Map<String, Object>> payablesDue(LocalDate dueTo, String ids, int fromDocNo, int toDocNo, int languageId) {
        return repository.payablesDue(user(S_PAYABLES_DUE), dueTo, ids, fromDocNo, toDocNo, languageId);
    }

    // 70
    public Map<String, Object> receivablesDueLookups() { UserAccount u = user(S_RECEIVABLES_DUE); return repository.receivablesDueLookups(u, context.currentFinancialYearId()); }
    public List<Map<String, Object>> receivablesDue(LocalDate dueTo, int fromDocNo, int toDocNo, int languageId, int customGroupId, String ids, String parentAccountCode) {
        return repository.receivablesDue(user(S_RECEIVABLES_DUE), dueTo, fromDocNo, toDocNo, languageId, customGroupId, ids, parentAccountCode);
    }

    // 68
    public Map<String, Object> receivablesNewLookups() { return repository.receivablesNewLookups(user(S_RECEIVABLES_NEW)); }
    public List<Map<String, Object>> receivablesNew(LocalDate from, LocalDate to, int customGroupId, int parentId) {
        return repository.receivablesNew(user(S_RECEIVABLES_NEW), from, to, customGroupId, parentId);
    }
}
