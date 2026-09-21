package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.PayablesPaymentScheduleRepository;
import com.mst.security.CurrentUserContext;
import java.time.LocalDate;
import java.util.*;
import org.springframework.stereotype.Service;

@Service
public class PayablesPaymentScheduleService {
    private final PayablesPaymentScheduleRepository repository;
    private final CurrentUserContext context;

    public PayablesPaymentScheduleService(PayablesPaymentScheduleRepository repository, CurrentUserContext context) {
        this.repository = repository;
        this.context = context;
    }

    public Map<String, Object> lookups() {
        UserAccount user = getUserSafely();
        return repository.lookups(user);
    }

    public Map<String, Object> load(LocalDate fromDate, LocalDate toDate, LocalDate dueFrom, LocalDate dueUpTo, LocalDate purchaseFrom, LocalDate purchaseTo, int intervalDays, int agingDays, int reportId, int parentId, int accountId, int customGroupId, int costCenterId, int customerGroupId, String controlAccount, String branches) {
        UserAccount user = getUserSafely();
        List<List<Map<String, Object>>> sets = repository.load(user, fromDate, toDate, dueFrom, dueUpTo, purchaseFrom, purchaseTo, intervalDays, agingDays, reportId, parentId, accountId, customGroupId, costCenterId, customerGroupId, controlAccount, branches);

        Map<String, Object> res = new LinkedHashMap<>();

        if (sets != null && !sets.isEmpty()) {
            res.put("data", sets.get(0));
            res.put("summaryMatrix", sets.size() > 1 ? sets.get(1) : Collections.emptyList());
            res.put("vouchers", sets.size() > 2 ? sets.get(2) : Collections.emptyList());
        } else {
            res.put("data", Collections.emptyList());
            res.put("summaryMatrix", Collections.emptyList());
            res.put("vouchers", Collections.emptyList());
        }

        return res;
    }

    private UserAccount getUserSafely() {
        return context.requireAccountingUser();
    }
}
