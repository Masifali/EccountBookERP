package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.PayablesReportRepository;
import com.mst.security.CurrentUserContext;
import java.time.LocalDate;
import java.util.*;
import org.springframework.stereotype.Service;

@Service
public class PayablesReportService {
    private final PayablesReportRepository repository;
    private final CurrentUserContext context;

    public PayablesReportService(PayablesReportRepository repository, CurrentUserContext context) {
        this.repository = repository;
        this.context = context;
    }

    public Map<String, Object> lookups() {
        UserAccount user = getUserSafely();
        return repository.lookups(user);
    }

    public Map<String,Object> invoiceLookups() {
        return repository.invoiceLookups(context.requireAccountingUser(), context.currentFinancialYearId());
    }

    public Map<String,Object> invoiceWise(LocalDate from, LocalDate to, int party, int agent, int parent, int group, int action) {
        if (action < 0 || action > 2) throw new IllegalArgumentException("Choose All, Paid or Balance");
        List<List<Map<String,Object>>> sets = repository.invoiceWise(context.requireAccountingUser(),from,to,party,agent,parent,group,action);
        return Map.of("data",sets.isEmpty() ? Collections.emptyList() : sets.get(0));
    }

    public Map<String, Object> load(LocalDate fromDate, LocalDate toDate, int controlAccountId, int accountId, int customGroupId, int inventoryGroupId, int cityId, double closingFrom, double closingTo, boolean onlyCredit, boolean onlyDebit, boolean tradeParties, boolean approvedTransactions, String classification, String typeNature, String sortField, String sortOrder) {
        UserAccount user = getUserSafely();
        List<List<Map<String, Object>>> sets = repository.load(user, fromDate, toDate, controlAccountId, accountId, customGroupId, inventoryGroupId, cityId, closingFrom, closingTo, onlyCredit, onlyDebit, tradeParties, approvedTransactions, classification, typeNature, sortField, sortOrder);

        Map<String, Object> res = new LinkedHashMap<>();
        if (sets != null && !sets.isEmpty()) {
            res.put("data", sets.get(0));
            res.put("vouchers", sets.size() > 1 ? sets.get(1) : Collections.emptyList());
        } else {
            res.put("data", Collections.emptyList());
            res.put("vouchers", Collections.emptyList());
        }
        return res;
    }

    private UserAccount getUserSafely() {
        return context.requireAccountingUser();
    }
}
