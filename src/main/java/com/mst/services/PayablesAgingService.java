package com.mst.services;

import com.mst.repositories.PayablesAgingRepository;
import com.mst.security.CurrentUserContext;
import com.mst.models.UserAccount;
import java.math.BigDecimal;
import java.time.LocalDate;
import java.util.*;
import org.springframework.stereotype.Service;

@Service
public class PayablesAgingService {
    private final PayablesAgingRepository repository;
    private final CurrentUserContext context;

    public PayablesAgingService(PayablesAgingRepository repository, CurrentUserContext context) {
        this.repository = repository;
        this.context = context;
    }

    public Map<String, Object> lookups() {
        return repository.lookups(context.requireAccountingUser(), context.currentFinancialYearId());
    }

    /** PayablesAging_New.GridFill. Unlike Receivables_New, the payables form has no cost centre and
     *  no "Cost Center Not Found" check. */
    public Map<String, Object> load(LocalDate date, int days, int report, String controlAccounts, int account, int custom, String branches) {
        if (date == null || days < 0 || !Set.of(0, 1, 2).contains(report) || account < 0 || custom < 0) {
            throw new IllegalArgumentException("Invalid aging filters");
        }
        for (String ids : new String[] {controlAccounts, branches}) {
            if (ids != null && !ids.isBlank() && !ids.matches("[0-9]+(,[0-9]+)*")) throw new IllegalArgumentException("Invalid selected accounts or branches");
        }
        UserAccount u = context.requireAccountingUser();
        List<List<Map<String, Object>>> sets = repository.load(u, date, days, report, controlAccounts, account, custom, branches);
        if (sets.size() < 5) {
            throw new IllegalStateException("The desktop aging procedure must return five result sets");
        }
        Map<String, Object> result = new LinkedHashMap<>();
        result.put("rows", sets.get(0));
        result.put("overdue", sets.get(3));
        result.put("notYetDue", sets.get(4));

        Map<String, BigDecimal> totals = new LinkedHashMap<>();
        for (String column : List.of("IstIntervale", "ScnInterval", "TrdIntarval", "Above", "CurrentBalance", "OverDue", "NotYetDue")) {
            BigDecimal sum = BigDecimal.ZERO;
            for (Map<String, Object> row : sets.get(0)) {
                Object value = row.get(column);
                if (value != null) {
                    try {
                        sum = sum.add(new BigDecimal(value.toString()));
                    } catch (Exception ignored) {}
                }
            }
            totals.put(column, sum);
        }
        result.put("totals", totals);
        return result;
    }
}
