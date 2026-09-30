package com.mst.repositories;

import com.mst.models.UserAccount;
import org.springframework.jdbc.core.ConnectionCallback;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;
import org.springframework.util.LinkedCaseInsensitiveMap;

import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.ResultSetMetaData;
import java.sql.SQLException;
import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * The DAL side of three more Taxation screens (dbo.ScreenDefinition ModuleId 9):
 *
 *   179 RegularItemsAllocateToTaxItem (Architecture.WinApp.Tax_Definition)
 *       -> Architecture.BLL.Tax.Definitions.RegularItemsAllocateToTaxItem
 *   175 frmSaleTaxSummaryRpt (Architecture.WinApp.Taxation)
 *       -> Architecture.BLL.Inventory.TaxSalesNotesGLMaping.SaleTaxSummary
 *   174 frmWhtTaxChallanDeposit (Architecture.WinApp.Account_Definition)
 *       -> Architecture.BLL.Accounts.VoucherHead (the lookups here; the voucher itself is saved by VoucherService)
 *
 * Procedures, read from procdure.utf8.sql on 30-Sep-2026:
 *
 *   Sp_Item_GetAllMethod @OrganizationId @CompanyId @Activity='ReadAllItemsTaxable'      (Item.ReadAllItemsTaxable - the Tax Item combo)
 *   Sp_ItemCategory_GetAllMethod @OrganizationId @CompanyId @Activity='ReadByOrganizationCompanyId' (CommonServices.GetItemCategoryForComboServiceBind)
 *   Sp_ItemType_GetAllMethod @OrganizationId @CompanyId @Activity='ReadByOrganizationCompanyId'     (CommonServices.GetItemTypeForComboServiceBind)
 *   USP_ItemsAllocatedOrUnAllocatedToTaxItem @OrganizationId @CompanyId @ActionId @TaxItemId [@ItemTypeId] [@ItemCategoryId]
 *       ActionId 1 = items allocated to NO tax item (pending), 2 = items allocated to THIS tax item; the two
 *       optional filters are sent only when non-zero, as the BLL does
 *   USP_RegularItemsAllocateToTaxItem_Insert  the model's 11 properties, one call per checked row inside one
 *       transaction (DAL SetData); the procedure itself upserts on (ItemId, TaxItemId)
 *   USp_RegularItemsAllocateToTaxItem_DeleteById @TaxItemId @ItemIds (CSV, split by dbo.fnSplitString)
 *   Sp_Accounts_GenerateSalesTaxSummery_Rpt @OrganizationId @CompanyId @FromDate @ToDate [@ProjectsId] [@BranchesId]
 *       (TaxSalesNotesGLMaping.SaleTaxSummary - the form never sets Projects/Branches, so they are omitted)
 *   Sp_Vouchers_GetMethods @Activity='GetDebitAcBySupplierCustomerGLAcIdForChallanDeposit' @OrganizationId @CompanyId @RefAccountId
 *   Sp_Vouchers_GetMethods @OrganizationId @CompanyId @BranchesId @ProjectsId @RefAccountId @Activity='GetPendingWhtChallan'
 *   Sp_COAAllocation_GetAllMethod @OrganizationId @CompanyId @UserId @Activity='COAAllocationSearch' (CommonServices.CoaAllocationGetAllServiceBind)
 *   Sp_Company_GetAllMethod @OrgCompanyTypeId @Activity='ReadByOrganizationId'          (CommonServices.CompanyServiceBind)
 *   Sp_Branches_GetAllMethod @OrganizationId @CompanyId @Activity='GetAll'              (CommonServices.BrancheServiceBind)
 *   Sp_Projects_GetAllMethod @OrganizationId @CompanyId @MethodType='GetAll'            (CommonServices.ProjectServiceBind)
 *   Sp_VoucherHead_WHTTaxChallanDeposit_Update @Id  (DAL VoucherHead.VoucherDetailIdUpdate - stamps the SOURCE
 *       voucher's VoucherDetail.InvoiceNoRefId = its own head Id, which is what drops it from GetPendingWhtChallan)
 *
 * No table, column or procedure is created or changed.
 */
@Repository
public class TaxationScreensRepository {

    private final JdbcTemplate jdbc;

    public TaxationScreensRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    // ============================================================ 179 Items Allocate To Tax Item

    /** Item.ReadAllItemsTaxable (BLL Item :2530). */
    public List<Map<String, Object>> taxableItems(UserAccount u) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@Activity", "ReadAllItemsTaxable");
        return rows("Sp_Item_GetAllMethod", p);
    }

    /** CommonServices.GetItemCategoryForComboServiceBind() -> ItemCategory.Getall (no parent filter). */
    public List<Map<String, Object>> itemCategories(UserAccount u) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@Activity", "ReadByOrganizationCompanyId");
        return rows("Sp_ItemCategory_GetAllMethod", p);
    }

    /** CommonServices.GetItemTypeForComboServiceBind() -> ItemType.Getall (no type / parent filter). */
    public List<Map<String, Object>> itemTypes(UserAccount u) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@Activity", "ReadByOrganizationCompanyId");
        return rows("Sp_ItemType_GetAllMethod", p);
    }

    /** RegularItemsAllocateToTaxItem.GetAllocatedAndUnAllocatedItems - @ItemTypeId / @ItemCategoryId only when != 0. */
    public List<Map<String, Object>> allocatedOrUnallocated(UserAccount u, int actionId, int taxItemId, int itemTypeId, int itemCategoryId) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@ActionId", actionId);
        p.put("@TaxItemId", taxItemId);
        if (itemTypeId != 0) p.put("@ItemTypeId", itemTypeId);
        if (itemCategoryId != 0) p.put("@ItemCategoryId", itemCategoryId);
        return rows("USP_ItemsAllocatedOrUnAllocatedToTaxItem", p);
    }

    /** RegularItemsAllocateToTaxItem.Save - one Insert per row, all in one transaction (DAL SetData). */
    @Transactional(rollbackFor = Exception.class)
    public int allocate(UserAccount u, int taxItemId, List<Integer> itemIds) {
        Timestamp now = new Timestamp(System.currentTimeMillis());
        int last = 0;
        for (int itemId : itemIds) {
            Map<String, Object> p = new LinkedHashMap<>();
            p.put("@Id", 0);
            p.put("@ItemId", itemId);
            p.put("@TaxItemId", taxItemId);
            p.put("@EntryDate", now);
            p.put("@EntryUserId", u.getId());
            p.put("@ModifyDate", now);
            p.put("@ModifyUserId", u.getId());
            p.put("@OrganizationId", u.getOrganizationId());
            p.put("@CompanyId", u.getCompanyId());
            p.put("@FinancialYearId", 0);
            p.put("@IsActive", true);
            List<Map<String, Object>> r = rows("USP_RegularItemsAllocateToTaxItem_Insert", p);
            if (!r.isEmpty() && !r.get(0).isEmpty()) {
                Object v = r.get(0).values().iterator().next();
                if (v instanceof Number) last = ((Number) v).intValue();
            }
        }
        return last;
    }

    /** RegularItemsAllocateToTaxItem.DeleteById(TaxItemId, "id,id,...,") - the CSV as the form builds it (trailing comma kept). */
    @Transactional(rollbackFor = Exception.class)
    public void deallocate(int taxItemId, String itemIdsCsv) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("@TaxItemId", taxItemId);
        p.put("@ItemIds", itemIdsCsv);
        rows("USp_RegularItemsAllocateToTaxItem_DeleteById", p);
    }

    // ============================================================ 175 Sale Tax Summary

    /** TaxSalesNotesGLMaping.SaleTaxSummary - ProjectsId / BranchesId sent only when != 0 (the form sets neither). */
    public List<Map<String, Object>> saleTaxSummary(UserAccount u, java.sql.Date from, java.sql.Date to) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@FromDate", from);
        p.put("@ToDate", to);
        return rows("Sp_Accounts_GenerateSalesTaxSummery_Rpt", p);
    }

    // ============================================================ 174 WHT Challan Deposit

    /** CommonServices.CoaAllocationGetAllServiceBind -> COAAllocation.GetAll 'COAAllocationSearch' (+@UserId). */
    public List<Map<String, Object>> coaAllocation(UserAccount u) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        if (u.getId() != 0) p.put("@UserId", u.getId());
        p.put("@Activity", "COAAllocationSearch");
        return rows("Sp_COAAllocation_GetAllMethod", p);
    }

    /** CommonServices.CompanyServiceBind -> Company.GetAlldt 'ReadByOrganizationId'. */
    public List<Map<String, Object>> companies(UserAccount u) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("@OrgCompanyTypeId", u.getOrganizationId());
        p.put("@Activity", "ReadByOrganizationId");
        return rows("Sp_Company_GetAllMethod", p);
    }

    /** CommonServices.BrancheServiceBind -> Branches.GetAll 'GetAll'. */
    public List<Map<String, Object>> branches(UserAccount u) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@Activity", "GetAll");
        return rows("Sp_Branches_GetAllMethod", p);
    }

    /** CommonServices.ProjectServiceBind -> Projects.GetAlldt @MethodType='GetAll'. */
    public List<Map<String, Object>> projects(UserAccount u) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@MethodType", "GetAll");
        return rows("Sp_Projects_GetAllMethod", p);
    }

    /** VoucherHead.GetDebitAcBySupplierCustomerGLAcIdForChallanDeposit - rows (RefDocNoId, AccountTitle). */
    public List<Map<String, Object>> challanDebitAccounts(UserAccount u, int refAccountId) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("@Activity", "GetDebitAcBySupplierCustomerGLAcIdForChallanDeposit");
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@RefAccountId", refAccountId);
        return rows("Sp_Vouchers_GetMethods", p);
    }

    /** VoucherHead.GetAllVoucherForChallanDeposit - 'GetPendingWhtChallan' (hId, InvoiceNoRefId, DocumentTypeId, Code, VoucherDate, VoucherCode, AccountId, AccountTitle, TaxTypeId, TaxDescription, CreditAmount). */
    public List<Map<String, Object>> pendingWhtChallan(UserAccount u, int branchId, int projectId, int refAccountId) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@BranchesId", branchId);
        p.put("@ProjectsId", projectId);
        p.put("@RefAccountId", refAccountId);
        p.put("@Activity", "GetPendingWhtChallan");
        return rows("Sp_Vouchers_GetMethods", p);
    }

    /** DAL VoucherHead.VoucherDetailIdUpdate(InvoiceNoRefId) - Sp_VoucherHead_WHTTaxChallanDeposit_Update @Id. */
    public void markSourceVoucherDeposited(int sourceVoucherHeadId) {
        jdbc.update("EXEC dbo.Sp_VoucherHead_WHTTaxChallanDeposit_Update @Id=?", sourceVoucherHeadId);
    }

    // ============================================================================= plumbing

    /** One EXEC with named parameters; the first result set is read, the rest are drained. */
    private List<Map<String, Object>> rows(String proc, Map<String, Object> params) {
        StringBuilder sql = new StringBuilder("EXEC dbo.").append(proc);
        List<Object> values = new ArrayList<>();
        boolean first = true;
        for (Map.Entry<String, Object> e : params.entrySet()) {
            sql.append(first ? " " : ", ").append(e.getKey()).append("=?");
            values.add(e.getValue());
            first = false;
        }
        final String text = sql.toString();
        return jdbc.execute((ConnectionCallback<List<Map<String, Object>>>) con -> {
            try (PreparedStatement ps = con.prepareStatement(text)) {
                for (int i = 0; i < values.size(); i++) ps.setObject(i + 1, values.get(i));
                boolean isRs = ps.execute();
                List<Map<String, Object>> out = null;
                while (true) {
                    if (isRs) {
                        try (ResultSet rs = ps.getResultSet()) {
                            if (out == null) out = read(rs);
                            else while (rs.next()) { /* drain */ }
                        }
                    } else if (ps.getUpdateCount() == -1) {
                        break;
                    }
                    isRs = ps.getMoreResults();
                }
                return out == null ? new ArrayList<>() : out;
            }
        });
    }

    private static List<Map<String, Object>> read(ResultSet rs) throws SQLException {
        List<Map<String, Object>> out = new ArrayList<>();
        ResultSetMetaData md = rs.getMetaData();
        int n = md.getColumnCount();
        while (rs.next()) {
            Map<String, Object> row = new LinkedCaseInsensitiveMap<>(n);
            for (int c = 1; c <= n; c++) {
                String name = md.getColumnLabel(c);
                if (name == null || name.isEmpty()) name = md.getColumnName(c);
                if (name == null || name.isEmpty()) name = "Column" + c;
                Object v = rs.getObject(c);
                if (v instanceof Timestamp) v = ((Timestamp) v).toLocalDateTime().toString();
                else if (v instanceof java.sql.Date) v = ((java.sql.Date) v).toLocalDate().toString();
                row.put(name, v);
            }
            out.add(row);
        }
        return out;
    }
}
