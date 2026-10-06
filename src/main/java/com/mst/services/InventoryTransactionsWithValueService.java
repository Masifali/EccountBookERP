package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryTransactionsWithValueRequest;
import com.mst.repositories.InventoryTransactionsWithValueRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import java.util.*;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Service;
import org.springframework.web.server.ResponseStatusException;

/** Screen 293 "Transaction Report (With Value)" - desktop InventoryEvaluationItemLedger. */
@Service
public class InventoryTransactionsWithValueService {
    private static final int SCREEN_ID = 293;
    private final InventoryTransactionsWithValueRepository repository;
    private final CurrentUserContext context;
    private final DesktopReportRights rights;

    public InventoryTransactionsWithValueService(InventoryTransactionsWithValueRepository repository, CurrentUserContext context, DesktopReportRights rights) {
        this.repository = repository; this.context = context; this.rights = rights;
    }

    private UserAccount user() {
        UserAccount user = context.requireAccountingUser();
        rights.require(user, SCREEN_ID, "View");
        return user;
    }

    /** Form Load / btnRefresh: BranchesFill, the AsOnDate default and the configured decimal formats. */
    public Map<String,Object> lookups() {
        UserAccount u = user();
        Map<String,Object> result = new LinkedHashMap<>();
        result.put("branches", repository.branches(u));
        result.put("branchId", u.getBranchesId());
        String fromDate = repository.stockAsOnDate(u);
        if (fromDate != null) result.put("fromDate", fromDate);
        result.put("financialYears", repository.financialYears(u));
        result.put("amountDecimals", repository.amountDecimals(u));
        result.put("rateDecimals", repository.rateDecimals(u));
        return result;
    }

    /** ParentCategoryComboFill for the checked branches (an empty branch text clears the parent combo). */
    public List<Map<String,Object>> choices(List<Integer> branchIds) {
        UserAccount u = user();
        List<Integer> branches = allowedBranches(u, branchIds);
        if (branches.isEmpty()) return new ArrayList<>();
        return repository.choices(u, branchCsv(branches));
    }

    /** btnshow_Click. */
    public List<Map<String,Object>> load(InventoryTransactionsWithValueRequest r) {
        UserAccount u = user();
        if (r == null || r.getFromDate() == null || r.getToDate() == null)
            throw new ResponseStatusException(HttpStatus.BAD_REQUEST, "From Date and To Date are required");
        List<Integer> branches = allowedBranches(u, r.getBranchIds());
        if (branches.isEmpty())
            throw new ResponseStatusException(HttpStatus.BAD_REQUEST, "Select branch first");
        StringBuilder ids = new StringBuilder();
        if (r.getParentIds() != null) {
            for (Integer id : r.getParentIds()) {
                if (id == null || id <= 0) throw new ResponseStatusException(HttpStatus.BAD_REQUEST, "Choose valid items from the dropdown");
                ids.append(id).append(',');          // desktop: Ids = Ids + Id + ","
            }
        }
        return repository.load(u, r, branchCsv(branches), ids.toString());
    }

    /** Only branches allocated to the signed-in user (USP_GetBranchsAllocatedToUser) are accepted. */
    private List<Integer> allowedBranches(UserAccount u, List<Integer> requested) {
        List<Integer> result = new ArrayList<>();
        if (requested == null || requested.isEmpty()) return result;
        Set<Integer> allowed = new HashSet<>();
        for (Map<String,Object> row : repository.branches(u)) {
            Object id = row.get("BranchId");
            if (id instanceof Number) allowed.add(((Number) id).intValue());
        }
        for (Integer id : requested) {
            if (id == null || !allowed.contains(id))
                throw new ResponseStatusException(HttpStatus.BAD_REQUEST, "Choose valid branches from the dropdown");
            if (!result.contains(id)) result.add(id);
        }
        return result;
    }

    /** desktop: BranchIds = BranchIds + "," + BranchId  -> ",1,2". */
    private static String branchCsv(List<Integer> branches) {
        StringBuilder csv = new StringBuilder();
        for (Integer id : branches) csv.append(',').append(id);
        return csv.toString();
    }
}
