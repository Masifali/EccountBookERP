package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.SalePccRptRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import java.util.*;
import org.springframework.stereotype.Service;

/**
 * Group P - Sales Pcc Reports (module 88): 451 StockTranfserRegister, 561 Sales_EvaulationDetailReports, 562 SalesWages_Register.
 * Tenancy (Organization / Company / User / Branch / Financial Year) always comes from CurrentUserContext, rights are the desktop's ScreenId "View" right
 * (DesktopReportRights). Nothing here edits a shared service.
 */
@Service
public class SalePccRptService {
    /** dbo.ScreenDefinition ids, keyed by the route segment. */
    public static final Map<String, Integer> SCREENS;
    static {
        Map<String, Integer> m = new LinkedHashMap<>();
        m.put("stock-transfer-register", 451);
        m.put("sales-evaluation-detail", 561);
        m.put("sales-wages-register", 562);
        SCREENS = Collections.unmodifiableMap(m);
    }

    private final SalePccRptRepository repository;
    private final CurrentUserContext context;
    private final DesktopReportRights rights;

    public SalePccRptService(SalePccRptRepository repository, CurrentUserContext context, DesktopReportRights rights) {
        this.repository = repository;
        this.context = context;
        this.rights = rights;
    }

    private UserAccount user(String key) {
        Integer screen = SCREENS.get(key);
        if (screen == null) throw new IllegalArgumentException("Unknown report " + key);
        UserAccount u = context.requireAccountingUser();
        rights.require(u, screen, "View");
        return u;
    }

    private int appId(UserAccount u) {
        Integer a = u.getAppId();
        return a != null && a > 0 ? a : context.currentAppId();
    }

    public UserAccount currentUser(String key) { return user(key); }

    /** The form's Load / Refresh lists. */
    public Map<String, Object> lookups(String key) {
        UserAccount u = user(key);
        int fy = context.currentFinancialYearId();
        switch (key) {
            case "stock-transfer-register": return repository.stockTransferLookups(u, fy);
            case "sales-evaluation-detail": return repository.evaluationLookups(u, fy, appId(u));
            case "sales-wages-register": return repository.wagesLookups(u, fy);
            default: throw new IllegalArgumentException("Unknown report " + key);
        }
    }

    /** The Show buttons. 562: q.activity 1 = Detail, 2 = Summary (anything else is the desktop's "Please Select Activity First..."). */
    public List<Map<String, Object>> rows(String key, Map<String, String> q) {
        UserAccount u = user(key);
        switch (key) {
            case "stock-transfer-register": return repository.stockTransferRegister(u, q);
            case "sales-evaluation-detail": return repository.evaluation(u, q);
            case "sales-wages-register": {
                int activity = SalePccRptRepository.i(q.get("activity"));
                if (activity == 0) throw new IllegalArgumentException("Please Select Activity First...");
                if (activity != 1 && activity != 2) return new ArrayList<>();
                return repository.wagesRegister(u, activity == 2, q);
            }
            default: throw new IllegalArgumentException("Unknown report " + key);
        }
    }
}
