package com.mst.services;

import com.mst.repositories.ReceivablesAgingRepository;
import com.mst.security.CurrentUserContext;
import com.mst.models.UserAccount;
import java.math.BigDecimal;
import java.time.LocalDate;
import java.util.*;
import org.springframework.stereotype.Service;

@Service
public class ReceivablesAgingService {
    private final ReceivablesAgingRepository repository;
    private final CurrentUserContext context;

    public ReceivablesAgingService(ReceivablesAgingRepository repository, CurrentUserContext context) {
        this.repository = repository;
        this.context = context;
    }

    public Map<String, Object> lookups() {
        return repository.lookups(context.requireAccountingUser());
    }

    public Map<String, Object> load(LocalDate date, int days, int report, int parent, int account, int custom, int cost, String branches) {
        if (date == null || days < 0 || !Set.of(0, 1, 2).contains(report) || parent < 0 || account < 0 || custom < 0 || cost < 0) {
            throw new IllegalArgumentException("Invalid aging filters");
        }
        UserAccount u = context.requireAccountingUser();
        if (Integer.valueOf(5).equals(u.getAppId()) && cost == 0) {
            throw new IllegalArgumentException("Cost Center Not Found");
        }
        List<List<Map<String, Object>>> sets = repository.load(u, date, days, report, parent, account, custom, cost, branches);
        if (sets == null || sets.size() < 5) {
            throw new IllegalStateException("The desktop aging procedure must return five result sets");
        }

        Map<String, Object> result = new LinkedHashMap<>();
        List<Map<String, Object>> rows = sets.get(0) != null ? sets.get(0) : Collections.emptyList();
        List<Map<String, Object>> overdue = sets.size() > 3 && sets.get(3) != null ? sets.get(3) : Collections.emptyList();
        List<Map<String, Object>> notYetDue = sets.size() > 4 && sets.get(4) != null ? sets.get(4) : Collections.emptyList();

        result.put("rows", rows);
        result.put("overdue", overdue);
        result.put("notYetDue", notYetDue);

        Map<String, BigDecimal> totals = new LinkedHashMap<>();
        for (String column : List.of("IstIntervale", "ScnInterval", "TrdIntarval", "Above", "CurrentBalance", "OverDue", "NotYetDue")) {
            BigDecimal sum = BigDecimal.ZERO;
            for (Map<String, Object> row : rows) {
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

