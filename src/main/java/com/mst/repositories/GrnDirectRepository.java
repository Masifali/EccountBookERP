package com.mst.repositories;

import com.mst.repositories.hrm.HrmProcRepository;
import com.mst.repositories.support.ProcExec;
import com.mst.security.CurrentUserContext;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Repository;

import java.sql.Timestamp;
import java.util.*;

/**
 * Architecture.WinApp.Purchase.InvFrmGRNDirect (ScreenDefinition 129 "frmGrnDirect", DocumentTypeId 137) - every read
 * the form makes through BLL 0576 InvGrn and its neighbours, with the BLL's own guards (a zero / empty value is OMITTED,
 * HrmProcRepository drops nulls exactly as ADO.NET AddWithValue(null) does). Writes go through the shared
 * PurchaseGrnWriteRepository (InvGrn DAL 0429 SetData contracts); nothing here writes except delete().
 *
 * Tenancy (organization / company / branch / financial year / user) is always the session's.
 */
@Repository
public class GrnDirectRepository {

    public static final int DOC = 137;
    public static final int SCREEN = 129;
    /** dbo.ScreenDefinition(129).ScreenName - the key Sp_tblUserRights_GetAllMethod 'GetByUserId' matches on. */
    public static final String SCREEN_NAME = "frmGrnDirect";
    private static final String PROC = "Sp_InvGRN_GetAllMethod";

    private final HrmProcRepository db;
    private final CurrentUserContext context;

    public GrnDirectRepository(HrmProcRepository db, CurrentUserContext context) { this.db = db; this.context = context; }

    public int org() { return context.currentOrganizationId(); }
    public int company() { return context.currentCompanyId(); }
    public int branch() { return context.currentBranchId(); }
    public int year() { return context.currentFinancialYearId(); }
    public int user() { return context.currentUserId(); }

    // ------------------------------------------------------------------ rights (same mechanism as PurchaseGrnRecordRepository)

    /** tblUserRights of the signed-in user for a screen (by ScreenDefinition.ScreenName), right by name. */
    public boolean hasRight(String screenName, String right) {
        String role = Objects.toString(context.currentRoleName(), "");
        boolean admin = "Admin".equalsIgnoreCase(role) || "Administrator".equalsIgnoreCase(role);
        if (admin && !"Delete".equals(right) && !"View".equals(right)) return true;
        for (Map<String, Object> r : db.rows("Sp_tblUserRights_GetAllMethod", "UserId", user(), "ScreenName", screenName,
                "RightName", role, "CompanyId", company(), "Activity", "GetByUserId")) {
            Object v = col(r, "Value");
            if (right.equalsIgnoreCase(Objects.toString(col(r, "RightName"), "").trim())
                    && (Boolean.TRUE.equals(v) || "1".equals(Objects.toString(v, "")) || "true".equalsIgnoreCase(Objects.toString(v, "")))) return true;
        }
        return false;
    }

    public boolean hasRight(String right) { return hasRight(SCREEN_NAME, right); }

    public void requireRight(String right) {
        if (!hasRight(right)) throw new AccessDeniedException("You do not have " + right + " permission for Goods Receipt Notes Direct");
    }

    // ------------------------------------------------------------------ configuration

    /** ConfigrationsAllocation.GetMultipleConfigurationsByConfigDescriptions(csv) -> ConfigDescription / ConfigKey rows. */
    public Map<String, String> configs(String csv) {
        Map<String, String> out = new LinkedHashMap<>();
        for (String k : csv.split(",")) out.put(k, "");
        for (Map<String, Object> r : db.rows("Sp_ConfigrationsAllocation_GetAllMethod", "OrganizationId", org(), "CompanyId", company(),
                "ConfigDescription", csv, "Activity", "GetMultipleConfigurationsByConfigDescriptions")) {
            String name = Objects.toString(col(r, "ConfigDescription"), "");
            for (String k : out.keySet()) if (k.equalsIgnoreCase(name)) out.put(k, Objects.toString(col(r, "ConfigKey"), "").trim());
        }
        return out;
    }

    /**
     * GlobalVariables_Helper.GetWagesRefDocumentsStatusById(137). The WinApp helper is not in the corpus; read the same way
     * PurchaseGrnFormRepository reads it for 46: USP_GetRefDocumentsForWages, the row of this RefDocumentTypeId with IsActive.
     */
    public boolean wagesRefDocumentActive() {
        for (Map<String, Object> r : db.rows("USP_GetRefDocumentsForWages")) {
            if (toInt(col(r, "RefDocumentTypeId")) == DOC && flag(col(r, "IsActive"))) return true;
        }
        return false;
    }

    // ------------------------------------------------------------------ numbering / lookups

    /** GenerateCode() :617 -> InvGrn.GenerateInvGrnCode: Org, Company, DocumentTypeId 137, FinancialYearId; BranchesId is not set on the
     *  desktop object (0) so the BLL omits it. */
    public int nextDocNo() {
        List<Map<String, Object>> r = db.rows(PROC, "OrganizationId", org(), "CompanyId", company(), "DocumentTypeId", DOC,
                "FinancialYearId", nz0(year()), "Activity", "GenerateInvGrnCode");
        return r.isEmpty() ? 0 : toInt(col(r.get(0), "DocNo"));
    }

    /** EmptyBagsGridRefresh :1656 -> CommonServices.GetPackingMaterialItemsAllocateToFlow() (defaults: TransactionFlowId 1, the zero
     *  ItemTypeId / ItemCategoryId omitted) -> ItemId / ItemName. */
    public List<Map<String, Object>> emptyBagItems() {
        return db.rows("USP_PackingMaterialItemsAllocateToTransactionFlow_GetForCombo", "OrganizationId", org(), "CompanyId", company(),
                "TransactionFlowId", 1);
    }

    /** HistoryComboFill :2843 -> InvGrn.GetDataForDropDownFromGrn(org, company, "137", null, null, 0) -> USP_GetDataForDropDownFromGrn. */
    public List<Map<String, Object>> historyDropDown() {
        return db.rows("[dbo].[USP_GetDataForDropDownFromGrn]", "OrganizationId", org(), "CompanyId", company(), "DocumentTypeIds", String.valueOf(DOC));
    }

    /** DeductionPolicyForGrn :2367 -> DeductionPolicyForGrn.GetPolicyForGrn(org, company, year, branch, DocDate, diff). */
    public Map<String, Object> deductionPolicy(Timestamp docDate, double difference) {
        List<Map<String, Object>> r = db.rows("[dbo].[USP_DeductionPolicyForGrn_GetAllMethod]", "OrganizationId", org(), "CompanyId", company(),
                "BranchesId", branch(), "FinancialYearId", year(), "EffectedDATE", docDate, "Difference", difference, "Activity", "GetPolicyForGrn");
        return r.isEmpty() ? null : r.get(0);
    }

    // ------------------------------------------------------------------ history / read

    /** HistoryGridFill :2911 -> InvGrn.GetHisoty (BLL 0576:334) 'GRNFormHistory' with the BLL's guards. */
    public List<Map<String, Object>> history(Map<String, Object> p) {
        return db.rows(PROC, p);
    }

    /** InvGrn.GetByID :168 -> 'ReadByID' header. */
    public Map<String, Object> header(int id) {
        List<Map<String, Object>> r = db.rows(PROC, "Id", id, "Activity", "ReadByID");
        return r.isEmpty() ? null : r.get(0);
    }

    /** DAL 0429 GetDate: DocumentTypeId 137 -> Sp_InvGrnDetail_GetAllMethod 'ReadByInvGrnIdDirect'. */
    public List<Map<String, Object>> details(int id) {
        return db.rows("Sp_InvGrnDetail_GetAllMethod", "Id", id, "Activity", "ReadByInvGrnIdDirect");
    }

    /** DAL 0429 GetDate: DocumentTypeId 137 -> 'ReadByInvGrnIdEmptyBagsDetail'. */
    public List<Map<String, Object>> emptyBags(int id) {
        return db.rows("Sp_InvGrnDetail_GetAllMethod", "Id", id, "Activity", "ReadByInvGrnIdEmptyBagsDetail");
    }

    /** The stored header row, only when it is a 137 GRN of the session's organization, company, branch and financial year. */
    public Map<String, Object> scoped(int id) {
        List<Map<String, Object>> r = db.jdbc().queryForList("SELECT * FROM dbo.InvGrn WHERE Id=? AND DocumentTypeId=? AND OrganizationId=? AND CompanyId=? AND BranchesId=? AND FinancialYearId=?",
                id, DOC, org(), company(), branch(), year());
        return r.isEmpty() ? null : r.get(0);
    }

    public List<Integer> storedDetailIds(int id) {
        return db.jdbc().queryForList("SELECT Id FROM dbo.InvGrnDetail WHERE InvGrnId=?", Integer.class, id);
    }

    // ------------------------------------------------------------------ Load Order (BtnLoadOrder_Click :4027)

    /**
     * LoadPurchaseOrder (DocumentTypeId 41) lists the orders through BLL 0595 GetPurchaseOrderForPurchaseInvoice
     * (Sp_PurchaseOrder_GetAllMethod): org, company always; FromDocNo / ToDocNo / DocumentTypeId / OrderSupCustId only
     * when non-zero; DocDateFrom / DocDateTo only when set (null is dropped). The dialog source itself is not in the
     * desktop tree (Architecture.WinApp.PurchaseTrading) - its filters are these BLL parameters.
     */
    public List<Map<String, Object>> purchaseOrdersForLoader(int supplierId, Timestamp from, Timestamp to, int fromDocNo, int toDocNo) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", org());
        p.put("CompanyId", company());
        if (fromDocNo != 0) p.put("FromDocNo", fromDocNo);
        if (toDocNo != 0) p.put("ToDocNo", toDocNo);
        p.put("DocumentTypeId", 41);
        if (supplierId != 0) p.put("OrderSupCustId", supplierId);
        p.put("DocDateFrom", from);
        p.put("DocDateTo", to);
        p.put("Activity", "GetPurchaseOrderForPurchaseInvoice");
        return db.rows("Sp_PurchaseOrder_GetAllMethod", p);
    }

    /** LoadPurchaseOrderDataForInvoice :4087 -> BLL 0581 PurchaseOrderLoadForPurchaseInvoice (org, company, DocumentTypeId 41, @POIds). */
    public List<Map<String, Object>> purchaseOrderLines(String orderIds) {
        return db.rows("[Sp_InvPurchaseInvoice_GetAllMethod]", "OrganizationId", org(), "CompanyId", company(), "DocumentTypeId", 41,
                "POIds", orderIds, "Activity", "PurchaseOrderLoadForPurchaseInvoice");
    }

    /** GetEmptyBagsInformationFromOrder :4137 -> BLL 0595 GetPurchaseOrderEmptyBagsDetailByOrderId(Id). */
    public List<Map<String, Object>> purchaseOrderEmptyBags(int orderId) {
        return db.rows("Sp_PurchaseOrder_GetAllMethod", "Id", orderId, "Activity", "GetPurchaseOrderEmptyBagsDetailByOrderId");
    }

    // ------------------------------------------------------------------ delete

    /** btnDelete_Click :2186 -> InvPurchaseInvoice.RemoveByID -> DAL 0434 AccountandInventoryRemoveById -> Sp_InvoicesVouchersandStocksDelete. */
    public void delete(int id) {
        ProcExec.run(db.jdbc(), "EXEC dbo.Sp_InvoicesVouchersandStocksDelete @OrganizationId=?,@CompanyId=?,@Id=?,@DocumentTypeId=?,@UserId=?",
                org(), company(), id, DOC, user());
    }

    // ------------------------------------------------------------------ helpers

    public static Object col(Map<String, Object> r, String k) {
        if (r == null) return null;
        if (r.containsKey(k)) return r.get(k);
        for (Map.Entry<String, Object> e : r.entrySet()) if (e.getKey().equalsIgnoreCase(k)) return e.getValue();
        return null;
    }
    public static int toInt(Object v) {
        if (v == null) return 0;
        if (v instanceof Number) return ((Number) v).intValue();
        if (v instanceof Boolean) return ((Boolean) v) ? 1 : 0;
        String s = v.toString().trim();
        if (s.isEmpty()) return 0;
        try { return Integer.parseInt(s); } catch (NumberFormatException e) {
            try { return new java.math.BigDecimal(s.replace(",", "")).intValue(); } catch (NumberFormatException e2) { return 0; }
        }
    }
    public static boolean flag(Object v) {
        if (v instanceof Boolean) return (Boolean) v;
        if (v instanceof Number) return ((Number) v).intValue() != 0;
        String s = Objects.toString(v, "").trim();
        return "1".equals(s) || "true".equalsIgnoreCase(s);
    }
    public static Integer nz0(int v) { return v != 0 ? v : null; }
}
