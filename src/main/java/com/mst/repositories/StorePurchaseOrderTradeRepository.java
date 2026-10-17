package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.ProcExec;
import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.Objects;
import org.springframework.http.HttpStatus;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;
import org.springframework.web.server.ResponseStatusException;

import static com.mst.repositories.PurchaseOrderPmRepository.col;
import static com.mst.repositories.PurchaseOrderPmRepository.intOf;

/**
 * Screen 328 "Purchase Order Store" - Architecture.WinApp.PurchaseTrading.PurchsaeOrderTrade, DocumentTypeId 42
 * (the non-taxable run of the form; the taxable run is Tag "frmPurchaseOrderWithTax", DocumentTypeId 132, a different screen).
 *
 * <pre>
 * Form method                         BLL / global                                      Procedure (@Activity)
 * ----------------------------------  ------------------------------------------------  ------------------------------------------------
 * SupplierNameFill          :618      CommonServices.SupplierCustomerGetforComboServiceBind   Sp_SupplierCustomer_GetAllMethod (ReadByOrganizationIdCompanyIdForBinding)
 * PaymentTerms              :673      CommonServices.GetDueTermServiceBind               Sp_InvDueTerms_GetAllMethod (GetAll)
 * ItemDetailFill            :811      CommonServices.GetItemByItemTypeId("14,17")        Sp_Item_GetAllMethod (GetItemByItemTypeId, @LookupTypeIds)
 * bindRateUomAndItemPackUom :855      CommonServices.dtUomFromGloablUomScheduleByItemId  usp_getAllUomsByCompanyId, filtered by ItemId
 * combojoblotfill           :871      jobLot.GetList(org, company)                       SP_JobLot_ReadMethod (GetAll)
 * HistoryBranchComboFill    :2059     PurchaseOrder.GetBranchesAllocatedToUserFromPurchaseOrder   USP_GetBranchsAllocatedToUserFromPurchaseOrder
 * HistoryComboFill          :2010     PurchaseOrder.GetDataForDropDownFromPurchaseOrder  USP_GetDataForDropDownFromPurchaseOrder
 * gridhistoryfill           :2121     PurchaseOrder.Getall                               Sp_PurchaseOrder_GetAllMethod (PurchaseOrderFormHistory)
 * ReadById                  :1723     PurchaseOrder.GetByID                              Sp_PurchaseOrder_GetAllMethod (ReadById / ReadByPurchaseOrderHeaderId)
 * </pre>
 *
 * The header write is {@link PurchaseOrderHeaderRepository}; the detail row, the removed-row procedure and
 * the weight validation are {@link PurchaseOrderPmRepository} (document-type agnostic). Only the approval row
 * is repeated here because the Pm one has 700 fixed.
 */
@Repository
public class StorePurchaseOrderTradeRepository {

    public static final int DOCUMENT_TYPE_ID = 42;
    /** base.Name - the ScreenName rights are keyed on (PurchsaeOrder_Load :435-439). */
    public static final String SCREEN = "PurchsaeOrderTrade";

    private final JdbcTemplate jdbc;

    public StorePurchaseOrderTradeRepository(JdbcTemplate jdbc) {
        this.jdbc = jdbc;
    }

    // ================================================================== combos

    /** SupplierNameFill :618 - dtSupplier bound to Id / CompanyName. */
    public List<Map<String, Object>> suppliers(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : jdbc.queryForList(
                "EXEC dbo.Sp_SupplierCustomer_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), "ReadByOrganizationIdCompanyIdForBinding")) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", intOf(col(r, "Id")));
            m.put("CompanyName", col(r, "CompanyName"));
            m.put("PartyCode", col(r, "PartyCode"));
            out.add(m);
        }
        return out;
    }

    /** PaymentTerms :673 - Id / TermsDescription. */
    public List<Map<String, Object>> dueTerms(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : jdbc.queryForList("EXEC dbo.Sp_InvDueTerms_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), "GetAll")) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", intOf(col(r, "Id")));
            m.put("TermsDescription", col(r, "TermsDescription"));
            out.add(m);
        }
        return out;
    }

    /** ItemDetailFill :811 - dtitem (Id, ItemName, ItemCode = ItemCodeNew). */
    public List<Map<String, Object>> items(UserAccount u, String lookupTypeIds) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : jdbc.queryForList(
                "EXEC dbo.Sp_Item_GetAllMethod @OrganizationId=?, @CompanyId=?, @LookupTypeIds=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), lookupTypeIds, "GetItemByItemTypeId")) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", intOf(col(r, "Id")));
            m.put("ItemName", col(r, "ItemName"));
            m.put("ItemCode", col(r, "ItemCodeNew"));
            out.add(m);
        }
        return out;
    }

    /** combojoblotfill :871 - Id / JobLotDescription. */
    public List<Map<String, Object>> jobLots(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : jdbc.queryForList("EXEC dbo.SP_JobLot_ReadMethod @OrganizationId=?, @CompanyId=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), "GetAll")) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", intOf(col(r, "Id")));
            m.put("JobLotDescription", col(r, "JobLotDescription"));
            out.add(m);
        }
        return out;
    }

    /** UserAccount.BranchName (a login-time virtual property) - read from the user's allocated branches. */
    public String branchName(UserAccount u, int branchId) {
        for (var r : jdbc.queryForList("EXEC dbo.USP_GetBranchsAllocatedToUser @OrganizationId=?, @CompanyId=?, @UserId=?",
                u.getOrganizationId(), u.getCompanyId(), u.getId())) {
            if (intOf(col(r, "BranchId")) == branchId) return Objects.toString(col(r, "BranchName"), "");
        }
        return "";
    }

    /** HistoryBranchComboFill :2059 (not branch-wise) - BranchId / BranchName. */
    public List<Map<String, Object>> branchesFromPurchaseOrder(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : jdbc.queryForList(
                "EXEC dbo.USP_GetBranchsAllocatedToUserFromPurchaseOrder @OrganizationId=?, @CompanyId=?, @UserId=?, @DocumentTypeId=?",
                u.getOrganizationId(), u.getCompanyId(), u.getId(), DOCUMENT_TYPE_ID)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", intOf(col(r, "BranchId")));
            m.put("BranchName", col(r, "BranchName"));
            out.add(m);
        }
        return out;
    }

    /** HistoryComboFill :2010 - rows whose Activity is "Supplier". BranchesIds is the leading-comma string the form builds. */
    public List<Map<String, Object>> historySuppliers(UserAccount u, String branchIds) {
        List<Map<String, Object>> out = new ArrayList<>();
        StringBuilder sql = new StringBuilder("EXEC dbo.USP_GetDataForDropDownFromPurchaseOrder @OrganizationId=?, @CompanyId=?, @DocumentTypeIds=?");
        List<Object> a = new ArrayList<>(List.of(u.getOrganizationId(), u.getCompanyId(), String.valueOf(DOCUMENT_TYPE_ID)));
        if (branchIds != null && !branchIds.isEmpty()) { sql.append(", @BranchesIds=?"); a.add(branchIds); }
        for (var r : jdbc.queryForList(sql.toString(), a.toArray())) {
            if (!"Supplier".equals(Objects.toString(col(r, "Activity"), ""))) continue;
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", intOf(col(r, "Id")));
            m.put("Supplier", col(r, "ReferenceName"));
            out.add(m);
        }
        return out;
    }

    // ================================================================== history

    /** History filter as ReportsParameters carries it; nulls/zeros are the unset state. */
    public static final class HistoryFilter {
        public boolean canViewAll;
        public int financialYearId;
        public String dateMode = "doc";
        public java.sql.Date from, to;
        public int fromDocNo, toDocNo, supplierId;
        public String branchIds;
    }

    /** gridhistoryfill :2121 -> BLL PurchaseOrder.Getall (BLL 0595:119) - every guarded parameter omitted when unset. */
    public List<Map<String, Object>> history(UserAccount u, HistoryFilter f) {
        StringBuilder sql = new StringBuilder("EXEC dbo.Sp_PurchaseOrder_GetAllMethod @OrganizationId=?, @CompanyId=?, @DocumentTypeId=?, @CanViewAllRecord=?");
        List<Object> a = new ArrayList<>(List.of(u.getOrganizationId(), u.getCompanyId(), DOCUMENT_TYPE_ID, f.canViewAll));
        if (f.financialYearId != 0) { sql.append(", @FinancialYearId=?"); a.add(f.financialYearId); }
        if (!f.canViewAll) { sql.append(", @EntryUser=?"); a.add(u.getId()); }
        String[] names = switch (f.dateMode) {
            case "entry" -> new String[]{"@EntryFromDate", "@EntryToDate"};
            case "modify" -> new String[]{"@ModifyFromDate", "@ModifyToDate"};
            default -> new String[]{"@DocDateFrom", "@DocDateTo"};
        };
        if (f.from != null) { sql.append(", ").append(names[0]).append("=?"); a.add(f.from); }
        if (f.to != null) { sql.append(", ").append(names[1]).append("=?"); a.add(f.to); }
        if (f.fromDocNo != 0) { sql.append(", @FromDocNo=?"); a.add(f.fromDocNo); }
        if (f.toDocNo != 0) { sql.append(", @ToDocNo=?"); a.add(f.toDocNo); }
        if (f.supplierId != 0) { sql.append(", @OrderSupCustId=?"); a.add(f.supplierId); }
        if (f.branchIds != null && !f.branchIds.isEmpty()) { sql.append(", @BranchesIds=?"); a.add(f.branchIds); }
        sql.append(", @Activity=?"); a.add("PurchaseOrderFormHistory");
        return jdbc.queryForList(sql.toString(), a.toArray());
    }

    // ================================================================== one record

    /** PurchaseOrder.GetByID header, refused unless it is this company's type-42 order. */
    public Map<String, Object> header(UserAccount u, int id) {
        var rows = jdbc.queryForList("EXEC dbo.Sp_PurchaseOrder_GetAllMethod @Id=?, @Activity=?", id, "ReadById");
        if (rows.isEmpty()) throw new ResponseStatusException(HttpStatus.NOT_FOUND, "Purchase Order not found");
        Map<String, Object> h = rows.get(0);
        if (intOf(col(h, "OrganizationId")) != u.getOrganizationId() || intOf(col(h, "CompanyId")) != u.getCompanyId()
                || intOf(col(h, "DocumentTypeId")) != DOCUMENT_TYPE_ID) {
            throw new ResponseStatusException(HttpStatus.NOT_FOUND, "Purchase Order not found in this company");
        }
        return h;
    }

    public List<Map<String, Object>> details(int id) {
        return jdbc.queryForList("EXEC dbo.Sp_PurchaseOrder_GetAllMethod @Id=?, @Activity=?", id, "ReadByPurchaseOrderHeaderId");
    }

    /** DAL SetData, insert only - the approval row for THIS document type. */
    public void approvalDetail(UserAccount u, int orderId, java.math.BigDecimal limitAmount) {
        ProcExec.run(jdbc, "EXEC [DAW].[USp_DocumentApprovalDetail_Insert] @OrganizationId=?, @CompanyId=?, @DocumentTypeId=?, @Id=?, @LimitAmount=?",
                u.getOrganizationId(), u.getCompanyId(), DOCUMENT_TYPE_ID, orderId, limitAmount);
    }

    public static Timestamp now() { return new Timestamp(System.currentTimeMillis()); }
}
