package com.mst.repositories;

import com.mst.models.UserAccount;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.util.*;
import java.util.concurrent.ConcurrentHashMap;
import org.springframework.http.HttpStatus;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.PreparedStatementCallback;
import org.springframework.stereotype.Repository;
import org.springframework.web.server.ResponseStatusException;

import static com.mst.repositories.PurchaseOrderPmRepository.col;
import static com.mst.repositories.PurchaseOrderPmRepository.dbl;
import static com.mst.repositories.PurchaseOrderPmRepository.intOf;

/**
 * Screen 504 "Store Send Receipt" - {@code StoreSendReceipt.cs} + its loader
 * {@code LoaderForPackingMaterialAndConsumeableStoreItemSendReceipts.cs}, DocumentTypeId 177.
 * BLL 0540 / DAL 0395 InvStoreSendReceipt.
 */
@Repository
public class StoreSendReceiptRepository {

    public static final int DOCUMENT_TYPE_ID = 177;
    public static final String SCREEN = "StoreSendReceipt";

    private final JdbcTemplate jdbc;
    private final Map<String, List<String>> parameters = new ConcurrentHashMap<>();

    public StoreSendReceiptRepository(JdbcTemplate jdbc) {
        this.jdbc = jdbc;
    }

    /** GenerateCode :212 -> InvStoreSendReceipt.GenerateCode. */
    public int nextDocNo(UserAccount u, int fy) {
        var rows = jdbc.queryForList("EXEC dbo.USp_InvStoreSendReceipt_GetAllMethod @OrganizationId=?, @CompanyId=?, @BranchId=?, @FinancialYearId=?, @DocumentTypeId=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), u.getBranchesId(), fy, DOCUMENT_TYPE_ID, "GenerateCode");
        return rows.isEmpty() ? 0 : intOf(col(rows.get(0), "DocNo"));
    }

    /** globalWarehousesWithBranches (USP_GetWarehousesAllocatedToBranch for the user's branch), active only, one row per warehouse. */
    public List<Map<String, Object>> warehouses(UserAccount u) {
        Map<Integer, Map<String, Object>> out = new LinkedHashMap<>();
        for (var r : jdbc.queryForList("EXEC dbo.USP_GetWarehousesAllocatedToBranch @OrganizationId=?, @CompanyId=?, @BranchId=?",
                u.getOrganizationId(), u.getCompanyId(), u.getBranchesId())) {
            Object active = col(r, "IsActive");
            if (!(Boolean.TRUE.equals(active) || "1".equals(String.valueOf(active)) || "true".equalsIgnoreCase(String.valueOf(active)))) continue;
            int id = intOf(col(r, "Id"));
            if (out.containsKey(id)) continue;
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", id);
            m.put("Description", col(r, "WareHouseName"));
            m.put("WareHouseTypeId", intOf(col(r, "WareHouseTypeId")));
            out.put(id, m);
        }
        return new ArrayList<>(out.values());
    }

    /** ItemFill :274 -> GetPackingMaterialItemsAllocateToFlow() (TransactionFlowId 1; type and category 0 are not sent). */
    public List<Map<String, Object>> items(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : jdbc.queryForList("EXEC [dbo].[USP_PackingMaterialItemsAllocateToTransactionFlow_GetForCombo] @OrganizationId=?, @CompanyId=?, @TransactionFlowId=?",
                u.getOrganizationId(), u.getCompanyId(), 1)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("ItemId", intOf(col(r, "ItemId")));
            m.put("ItemName", col(r, "ItemName"));
            out.add(m);
        }
        return out;
    }

    /** Loader :PendingInventoryTransactionsForIssuanceLoad -> GetPackingMaterialFilledToEmpty (the dates on the loader are never sent). */
    public List<Map<String, Object>> pending(UserAccount u, int fy) {
        return jdbc.queryForList("EXEC [dbo].[USP_GetPackingMaterialFilledToEmpty] @OrganizationId=?, @CompanyId=?, @BranchId=?, @FinancialYearId=?",
                u.getOrganizationId(), u.getCompanyId(), u.getBranchesId(), fy);
    }

    /** CommonServices.GetAvgRateQtyAndStockInHand(item, date, condition, recId, recId > 0 ? 177 : 0) - AvgRate of row 0. */
    public Double avgRate(UserAccount u, int itemId, java.sql.Date docDate, int conditionId, int recId) {
        StringBuilder sql = new StringBuilder("EXEC dbo.Sp_GetAvgRatesAndStockInHand_GetAllMethod @OrganizationId=?, @CompanyId=?, @ItemId=?, @DocDate=?");
        List<Object> a = new ArrayList<>(List.of(u.getOrganizationId(), u.getCompanyId(), itemId, docDate));
        if (recId > 0) { sql.append(", @DocumentTypeId=?"); a.add(DOCUMENT_TYPE_ID); }
        if (recId != 0) { sql.append(", @RecId=?"); a.add(recId); }
        if (conditionId != 0) { sql.append(", @ItemConditionId=?"); a.add(conditionId); }
        sql.append(", @Activity=?"); a.add("GetAvgRateQtyAndStockInHand");
        var rows = jdbc.queryForList(sql.toString(), a.toArray());
        return rows.isEmpty() ? null : dbl(col(rows.get(0), "AvgRate"));
    }

    /** CommonServices.GetUomScheduleByItemId -> UOMSchedule.SearchByObject (Sp_UOMSchedule_GetAllMethod 'ReadByItemID'). */
    public List<Map<String, Object>> uomsByItem(UserAccount u, int itemId) {
        return jdbc.queryForList("EXEC dbo.Sp_UOMSchedule_GetAllMethod @OrganizationId=?, @CompanyId=?, @ItemId=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), itemId, "ReadByItemID");
    }

    /** ComboBindForHistory :287 -> GetDataForDropDown (Activity not set, so not sent). */
    public List<Map<String, Object>> historyCombos(UserAccount u, int fy) {
        StringBuilder sql = new StringBuilder("EXEC [dbo].[USP_GetDataForDropDownFromStoreSendReceipt] @OrganizationId=?, @CompanyId=?");
        List<Object> a = new ArrayList<>(List.of(u.getOrganizationId(), u.getCompanyId()));
        if (fy != 0) { sql.append(", @FinancialYearId=?"); a.add(fy); }
        return jdbc.queryForList(sql.toString(), a.toArray());
    }

    public static final class HistoryFilter {
        public int financialYearId, fromDocNo, toDocNo, senderId, receiverId;
        public java.sql.Date from, to;
    }

    /** GridHistoryBind :748 -> InvStoreSendReceipt.FormHistory. */
    public List<Map<String, Object>> history(UserAccount u, HistoryFilter f) {
        StringBuilder sql = new StringBuilder("EXEC dbo.USp_InvStoreSendReceipt_GetAllMethod @OrganizationId=?, @CompanyId=?, @DocumentTypeId=?, @BranchId=?");
        List<Object> a = new ArrayList<>(List.of(u.getOrganizationId(), u.getCompanyId(), DOCUMENT_TYPE_ID, u.getBranchesId()));
        if (f.financialYearId != 0) { sql.append(", @FinancialYearId=?"); a.add(f.financialYearId); }
        if (f.from != null) { sql.append(", @FromDate=?"); a.add(f.from); }
        if (f.to != null) { sql.append(", @ToDate=?"); a.add(f.to); }
        if (f.fromDocNo != 0) { sql.append(", @DocNoFrom=?"); a.add(f.fromDocNo); }
        if (f.toDocNo != 0) { sql.append(", @DocNoTo=?"); a.add(f.toDocNo); }
        if (f.senderId != 0) { sql.append(", @SenderWarehouseId=?"); a.add(f.senderId); }
        if (f.receiverId != 0) { sql.append(", @ReceiverWarehouseId=?"); a.add(f.receiverId); }
        sql.append(", @Activity=?"); a.add("FormHistory");
        return jdbc.queryForList(sql.toString(), a.toArray());
    }

    /** InvStoreSendReceipt.GetByID header, refused unless it is this company's 177 document. */
    public Map<String, Object> header(UserAccount u, int id) {
        var rows = jdbc.queryForList("EXEC dbo.USp_InvStoreSendReceipt_GetAllMethod @InvStoreSendReceiptId=?, @Activity=?", id, "ReadById");
        if (rows.isEmpty()) throw new ResponseStatusException(HttpStatus.NOT_FOUND, "Record Not Found");
        Map<String, Object> h = rows.get(0);
        if (intOf(col(h, "OrganizationId")) != u.getOrganizationId() || intOf(col(h, "CompanyId")) != u.getCompanyId()
                || intOf(col(h, "DocumentTypeId")) != DOCUMENT_TYPE_ID) {
            throw new ResponseStatusException(HttpStatus.NOT_FOUND, "Record Not Found");
        }
        return h;
    }

    /** DAL GetData - ReadByHeaderId. */
    public List<Map<String, Object>> details(int id) {
        return jdbc.queryForList("EXEC dbo.USp_InvStoreSendReceipt_GetAllMethod @InvStoreSendReceiptId=?, @Activity=?", id, "ReadByHeaderId");
    }

    public List<Map<String, Object>> attachments(UserAccount u, int id) {
        return jdbc.queryForList("EXEC dbo.Sp_DMSAttachments_GetAllMethod @ScreenName=?, @Id=?, @Activity=?", SCREEN, id, "ReadById")
                .stream()
                .filter(r -> intOf(col(r, "OrganizationId")) == u.getOrganizationId() && intOf(col(r, "CompanyId")) == u.getCompanyId())
                .toList();
    }

    /**
     * GenericProvider.SetProc: every model property the procedure declares; a null property is not sent.
     * First scalar back.
     */
    public int execute(String procedure, Map<String, Object> values) {
        List<String> names = parameters.computeIfAbsent(procedure, p -> jdbc.queryForList(
                "SELECT SUBSTRING(name,2,128) FROM sys.parameters WHERE object_id=OBJECT_ID(?) AND parameter_id>0 ORDER BY parameter_id",
                String.class, p));
        if (names.isEmpty()) throw new IllegalStateException("Procedure contract unavailable: " + procedure);
        TreeMap<String, Object> ci = new TreeMap<>(String.CASE_INSENSITIVE_ORDER);
        ci.putAll(values);
        List<String> selected = names.stream().filter(k -> ci.containsKey(k) && ci.get(k) != null).toList();
        String sql = "EXEC " + procedure + " " + String.join(", ", selected.stream().map(k -> "@" + k + "=?").toList());
        return jdbc.execute(sql, (PreparedStatementCallback<Integer>) st -> {
            int n = 1;
            for (String name : selected) st.setObject(n++, ci.get(name));
            return first(st);
        });
    }

    private static int first(PreparedStatement st) throws java.sql.SQLException {
        boolean result = st.execute();
        Integer first = null;
        while (true) {
            if (result) {
                try (ResultSet rs = st.getResultSet()) {
                    while (rs.next()) if (first == null && rs.getObject(1) instanceof Number n) first = n.intValue();
                }
            } else if (st.getUpdateCount() == -1) break;
            result = st.getMoreResults();
        }
        return first == null ? 0 : first;
    }
}
