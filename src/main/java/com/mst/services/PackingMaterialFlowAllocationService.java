package com.mst.services;

import com.mst.repositories.PackingMaterialFlowAllocationRepository;
import com.mst.security.CurrentUserContext;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import org.springframework.web.server.ResponseStatusException;
import org.springframework.http.HttpStatus;

import java.sql.Timestamp;
import java.time.LocalDateTime;
import java.util.*;

/** BLL parity for the desktop's packing-material-to-transaction-flow allocation screen. */
@Service
public class PackingMaterialFlowAllocationService {
    private final PackingMaterialFlowAllocationRepository repository;
    private final CurrentUserContext user;

    public PackingMaterialFlowAllocationService(PackingMaterialFlowAllocationRepository repository,
                                                CurrentUserContext user) {
        this.repository = repository;
        this.user = user;
    }

    public Map<String, Object> lookups() {
        Map<String, List<Map<String, Object>>> filters = repository.itemFilters(
                user.currentOrganizationId(), user.currentCompanyId());
        return Map.of("transactionFlows", repository.transactionFlows(),
                "categories", filters.get("categories"), "types", filters.get("types"));
    }

    public List<Map<String, Object>> items(int flowId, int typeId, int categoryId, int actionId) {
        requireFlow(flowId);
        if (actionId != 1 && actionId != 2)
            throw new ResponseStatusException(HttpStatus.BAD_REQUEST, "Invalid allocation list action");
        return repository.items(user.currentOrganizationId(), user.currentCompanyId(), flowId,
                Math.max(0, typeId), Math.max(0, categoryId), actionId);
    }

    /** btnAllocateItems_Click / BLL.Save. Writes only checked pending items and commits as one unit. */
    @Transactional
    public int allocate(int flowId, List<Integer> requestedIds) {
        List<Integer> ids = cleanIds(requestedIds);
        requireFlow(flowId);
        if (ids.isEmpty()) throw new ResponseStatusException(HttpStatus.BAD_REQUEST,
                "Checked Row's first To Allocate Items");
        Set<Integer> pending = ids(repository.items(user.currentOrganizationId(), user.currentCompanyId(),
                flowId, 0, 0, 1));
        if (!pending.containsAll(ids)) throw new ResponseStatusException(HttpStatus.CONFLICT,
                "One or more selected items are no longer pending for allocation");
        Timestamp now = Timestamp.valueOf(LocalDateTime.now());
        int inserted = 0;
        for (int itemId : ids) {
            repository.insert(itemId, flowId, now, user.currentUserId(),
                    user.currentOrganizationId(), user.currentCompanyId());
            inserted++;
        }
        return inserted;
    }

    /** btnDeAllocate_Click / BLL.DeleteById. Tenant-checks the selected allocation before deletion. */
    @Transactional
    public int deallocate(int flowId, List<Integer> requestedIds) {
        List<Integer> ids = cleanIds(requestedIds);
        requireFlow(flowId);
        if (ids.isEmpty()) throw new ResponseStatusException(HttpStatus.BAD_REQUEST,
                "Checked Row's first To Un-Allocate Items");
        Set<Integer> allocated = ids(repository.items(user.currentOrganizationId(), user.currentCompanyId(),
                flowId, 0, 0, 2));
        if (!allocated.containsAll(ids)) throw new ResponseStatusException(HttpStatus.CONFLICT,
                "One or more selected items are no longer allocated to this flow");
        repository.delete(flowId, String.join(",", ids.stream().map(String::valueOf).toList()));
        return ids.size();
    }

    private void requireFlow(int flowId) {
        if (flowId <= 0 || repository.transactionFlows().stream().noneMatch(r ->
                number(r.get("Id")) == flowId || number(r.get("ID")) == flowId)) {
            throw new ResponseStatusException(HttpStatus.BAD_REQUEST, "Select TransactionFlow First");
        }
    }
    private static Set<Integer> ids(List<Map<String, Object>> rows) {
        Set<Integer> out = new HashSet<>();
        for (Map<String, Object> row : rows) out.add(number(row.get("ItemId")));
        return out;
    }
    private static List<Integer> cleanIds(List<Integer> input) {
        if (input == null) return List.of();
        LinkedHashSet<Integer> ids = new LinkedHashSet<>();
        for (Integer id : input) {
            if (id == null || id <= 0) throw new ResponseStatusException(HttpStatus.BAD_REQUEST, "Invalid item selection");
            ids.add(id);
        }
        return List.copyOf(ids);
    }
    private static int number(Object value) {
        if (value instanceof Number n) return n.intValue();
        try { return Integer.parseInt(String.valueOf(value)); } catch (Exception ignored) { return 0; }
    }
}
