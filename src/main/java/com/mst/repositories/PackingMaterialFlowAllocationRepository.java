package com.mst.repositories;

import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/** Data access for frmPackingMaterialItemsAllocateToTransactionFlow. */
@Repository
public class PackingMaterialFlowAllocationRepository {
    private final JdbcTemplate jdbc;

    public PackingMaterialFlowAllocationRepository(JdbcTemplate jdbc) {
        this.jdbc = jdbc;
    }

    /** TransactionFlowFill / BLL.TransactionFlow: SELECT * FROM dbo.VTransactionFlow. */
    public List<Map<String, Object>> transactionFlows() {
        return jdbc.queryForList("SELECT * FROM dbo.VTransactionFlow");
    }

    /**
     * ComboBind builds distinct category and type lists from the desktop's global all-items list,
     * which is USP_Item_AllItemsWithModal for the signed-in organization and company.
     */
    public Map<String, List<Map<String, Object>>> itemFilters(int organizationId, int companyId) {
        List<Map<String, Object>> categories = new ArrayList<>();
        List<Map<String, Object>> types = new ArrayList<>();
        Map<Integer, String> seenCategories = new LinkedHashMap<>();
        Map<Integer, String> seenTypes = new LinkedHashMap<>();
        List<Map<String, Object>> rows = jdbc.queryForList(
                "EXEC dbo.USP_Item_AllItemsWithModal @OrganizationId=?, @CompanyId=?",
                organizationId, companyId);
        for (Map<String, Object> row : rows) {
            int categoryId = number(value(row, "ItemCategoryId"));
            String category = string(value(row, "ItemCategory"));
            if (categoryId > 0 && !category.isBlank()) seenCategories.putIfAbsent(categoryId, category);
            int typeId = number(value(row, "ItemTypeId"));
            String type = string(value(row, "ItemType"));
            if (typeId > 0 && !type.isBlank()) seenTypes.putIfAbsent(typeId, type);
        }
        seenCategories.forEach((id, name) -> categories.add(Map.of("Id", id, "Name", name)));
        seenTypes.forEach((id, name) -> types.add(Map.of("Id", id, "Name", name)));
        return Map.of("categories", categories, "types", types);
    }

    /**
     * BLL.GetAllocatedAndUnAllocatedItems: optional type/category parameters are omitted when 0.
     * Action 1 is pending/unallocated; action 2 is already allocated.
     */
    public List<Map<String, Object>> items(int organizationId, int companyId, int flowId,
                                           int typeId, int categoryId, int actionId) {
        StringBuilder sql = new StringBuilder(
                "EXEC dbo.USP_PackingMaterialItemsAllocatedOrUnAllocatedToItemTransactionFlow "
                        + "@OrganizationId=?, @CompanyId=?, @TransactionFlowId=?");
        List<Object> args = new ArrayList<>(List.of(organizationId, companyId, flowId));
        if (typeId > 0) { sql.append(", @ItemTypeId=?"); args.add(typeId); }
        if (categoryId > 0) { sql.append(", @ItemCategoryId=?"); args.add(categoryId); }
        sql.append(", @ActionId=?");
        args.add(actionId);
        return jdbc.queryForList(sql.toString(), args.toArray());
    }

    /** BLL.Save -> DAL.SetData -> USP_PackingMaterialItemsAllocateToTransactionFlow_Insert. */
    public int insert(int itemId, int flowId, java.sql.Timestamp at, int userId,
                      int organizationId, int companyId) {
        Integer id = jdbc.queryForObject(
                "EXEC dbo.USP_PackingMaterialItemsAllocateToTransactionFlow_Insert "
                        + "@Id=?, @ItemId=?, @TransactionFlowId=?, @EntryDate=?, @EntryUserId=?, "
                        + "@ModifyDate=?, @ModifyUserId=?, @OrganizationId=?, @CompanyId=?",
                Integer.class, 0, itemId, flowId, at, userId, at, userId, organizationId, companyId);
        return id == null ? 0 : id;
    }

    /** BLL.DeleteById -> USp_PackingMaterialItemsAllocateToTransactionFlowDeleteById. */
    public void delete(int flowId, String itemIds) {
        jdbc.update("EXEC dbo.USp_PackingMaterialItemsAllocateToTransactionFlowDeleteById "
                        + "@TransactionFlowId=?, @ItemIds=?", flowId, itemIds);
    }

    private static Object value(Map<String, Object> row, String key) {
        for (Map.Entry<String, Object> e : row.entrySet())
            if (e.getKey().equalsIgnoreCase(key)) return e.getValue();
        return null;
    }
    private static int number(Object value) {
        if (value instanceof Number n) return n.intValue();
        try { return Integer.parseInt(String.valueOf(value)); } catch (Exception ignored) { return 0; }
    }
    private static String string(Object value) { return value == null ? "" : String.valueOf(value).trim(); }
}
