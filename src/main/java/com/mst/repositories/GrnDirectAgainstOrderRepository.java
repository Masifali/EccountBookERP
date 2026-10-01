package com.mst.repositories;

import com.mst.repositories.hrm.HrmProcRepository;
import com.mst.security.CurrentUserContext;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Repository;

import java.sql.Timestamp;
import java.util.*;

/**
 * Architecture.WinApp.Purchase.GRNDirectAgainstOrder (ScreenDefinition 133 "GRNDirectAgainstOrder", DocumentTypeId 169) -
 * every read the form makes, through the desktop BLL's own procedures and guards (HrmProcRepository omits a null value, as
 * the BLLs omit a zero / empty one). Writes go through the shared PurchaseGrnWriteRepository (InvGrn DAL 0429 SetData
 * contracts), used read-only. No raw SQL: every read is a procedure the desktop calls.
 *
 * Replaces the earlier fabricated repository (Sp_InvGrn_GetAllMethod 'GenerategpCode', a SELECT on SupplierCustomer with
 * CustomerGroupId 7, Sp_GatePassInward_GetAllMethod 'GatepassHistory', Sp_GatePassInward_Delete - none of which the form uses).
 *
 * Tenancy (organization / company / branch / financial year / user) is always the session's.
 */
@Repository
public class GrnDirectAgainstOrderRepository {

    public static final int DOC = 169;
    public static final int SCREEN = 133;
    /** GRN.ScreenName = base.Name and SetRightsValueInRightsObject(base.Name): dbo.ScreenDefinition(133).ScreenName. */
    public static final String SCREEN_NAME = "GRNDirectAgainstOrder";
    /** LoadPurchaseOrder.DocumentTypeId / obj.DocumentTypeId = 41 (PurchsaeOrder). */
    public static final int ORDER_DOC = 41;
    private static final String PROC = "Sp_InvGRN_GetAllMethod";

    private final HrmProcRepository db;
    private final CurrentUserContext context;

    public GrnDirectAgainstOrderRepository(HrmProcRepository db, CurrentUserContext context) { this.db = db; this.context = context; }

    public int org() { return context.currentOrganizationId(); }
    public int company() { return context.currentCompanyId(); }
    public int branch() { return context.currentBranchId(); }
    public int year() { return context.currentFinancialYearId(); }
    public int user() { return context.currentUserId(); }

    // ------------------------------------------------------------------ rights (the mechanism of PurchaseGrnRecordRepository / GrnDirectRepository)

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
        if (!hasRight(right)) throw new AccessDeniedException("You do not have " + right + " permission for GRN Direct Against Order");
    }

    // ------------------------------------------------------------------ numbering / header combos

    /** GenerateCode() :931 -> InvGrn.GenerateInvGrnCode (BLL 0576:192): Org, Company, DocumentTypeId 169, FinancialYearId when != 0;
     *  BranchesId is not set on the desktop object (0) so the BLL omits it. */
    public int nextDocNo() {
        List<Map<String, Object>> r = db.rows(PROC, "OrganizationId", org(), "CompanyId", company(), "DocumentTypeId", DOC,
                "FinancialYearId", nz0(year()), "Activity", "GenerateInvGrnCode");
        return r.isEmpty() ? 0 : toInt(col(r.get(0), "DocNo"));
    }

    /** BranchFill() :956 -> CommonServices.BrancheServiceBind -> Branches.GetAll (BLL 0058:29): Sp_Branches_GetAllMethod 'GetAll'. */
    public List<Map<String, Object>> branches() {
        return db.rows("Sp_Branches_GetAllMethod", "OrganizationId", org(), "CompanyId", company(), "Activity", "GetAll");
    }

    /** ProjectFill() :973 -> CommonServices.ProjectServiceBind -> Projects.GetAlldt (BLL 0078:58): Sp_Projects_GetAllMethod @MethodType 'GetAll'. */
    public List<Map<String, Object>> projects() {
        return db.rows("Sp_Projects_GetAllMethod", "OrganizationId", org(), "CompanyId", company(), "MethodType", "GetAll");
    }

    // ------------------------------------------------------------------ purchase order reads

    /** ItemNameFill() :1064 -> PurchaseOrder.ReadByPurchaseOrderIDNOrderItemId (BLL 0595:746): Sp_PurchaseOrderDetail_GetAllMethod
     *  @PurchaseOrderId (always sent, 0 included) 'ReadByPurchaseOrderIDNOrderItemId'. */
    public List<Map<String, Object>> orderItems(int purchaseOrderId) {
        return db.rows("Sp_PurchaseOrderDetail_GetAllMethod", "PurchaseOrderId", purchaseOrderId, "Activity", "ReadByPurchaseOrderIDNOrderItemId");
    }

    /**
     * cmbOrderNo_Leave :2680 -> SupplierCustomer.GetSupplierByPurchaseOrderNo (BLL 0600:535): @PurchaseOrderId = ToInt(cmbOrderNo.Text)
     * (the order's DocNo - the procedure compares it with PurchaseOrder.DocNo), org, company, DocumentTypeId 41 always;
     * FinancialYearId / BranchesId (UserAccount.BranchesId) when != 0; Id / DocDate not set. RAISERRORs of the procedure propagate.
     */
    public List<Map<String, Object>> supplierByOrderNo(int orderNo) {
        return db.rows("Sp_SupplierCustomer_GetAllMethod", "PurchaseOrderId", orderNo, "OrganizationId", org(), "CompanyId", company(),
                "DocumentTypeId", ORDER_DOC, "FinancialYearId", nz0(year()), "BranchesId", nz0(branch()), "Activity", "SupplierByPurchaseOrderNo");
    }

    /**
     * The LoadPurchaseOrder dialog (namespace Architecture.WinApp.PurchaseTrading, source NOT in the corpus) opened with DocumentTypeId 41.
     * Its list is read, as the Stock In Transit port does, through BLL 0595 GetPurchaseOrderForPurchaseInvoice (:891): org, company always;
     * FromDocNo / ToDocNo / DocumentTypeId / OrderSupCustId only when != 0, DocDateFrom / DocDateTo only when set.
     */
    public List<Map<String, Object>> pendingOrders(int supplierId, Timestamp fromDate, Timestamp toDate, int fromDocNo, int toDocNo) {
        return db.rows("Sp_PurchaseOrder_GetAllMethod", "OrganizationId", org(), "CompanyId", company(),
                "FromDocNo", nz0(fromDocNo), "ToDocNo", nz0(toDocNo), "DocumentTypeId", ORDER_DOC, "OrderSupCustId", nz0(supplierId),
                "DocDateFrom", fromDate, "DocDateTo", toDate, "Activity", "GetPurchaseOrderForPurchaseInvoice");
    }

    /** LoadInGridDetail :2515 -> LoadPurchaseOrderDataForGrn :2586 -> InvPurchaseInvoice.PurchaseOrderLoadForPurchaseInvoice (BLL 0581:1256):
     *  org, company, DocumentTypeId 41 (!= 0), @POIds always ("," + ids), 'PurchaseOrderLoadForPurchaseInvoice'. */
    public List<Map<String, Object>> orderLoad(String poIds) {
        return db.rows("[Sp_InvPurchaseInvoice_GetAllMethod]", "OrganizationId", org(), "CompanyId", company(), "DocumentTypeId", ORDER_DOC,
                "POIds", poIds, "Activity", "PurchaseOrderLoadForPurchaseInvoice");
    }

    /** GetEmptyBagsInformationFromOrder :2546 -> PurchaseOrder.GetPurchaseOrderEmptyBagsDetailByOrderId(id, "") (BLL 0595:1336):
     *  @Id when != 0, @Ids omitted (blank), 'GetPurchaseOrderEmptyBagsDetailByOrderId'. */
    public List<Map<String, Object>> orderEmptyBags(int purchaseOrderId) {
        return db.rows("Sp_PurchaseOrder_GetAllMethod", "Id", nz0(purchaseOrderId), "Activity", "GetPurchaseOrderEmptyBagsDetailByOrderId");
    }

    // ------------------------------------------------------------------ history / read

    /** HistoryGridFill :1743 -> InvGrn.GetHisoty (BLL 0576:334) 'GRNFormHistory' (the parameters are built by the service with the BLL's guards). */
    public List<Map<String, Object>> history(Map<String, Object> p) {
        return db.rows(PROC, p);
    }

    /** InvGrn.GetByID (BLL 0576:168) -> 'ReadByID' header. */
    public Map<String, Object> header(int id) {
        List<Map<String, Object>> r = db.rows(PROC, "Id", id, "Activity", "ReadByID");
        return r.isEmpty() ? null : r.get(0);
    }

    /** DAL 0429 GetDate: DocumentTypeId 169 -> Sp_InvGrnDetail_GetAllMethod @Id 'ReadByInvGrnID'. */
    public List<Map<String, Object>> details(int id) {
        return db.rows("Sp_InvGrnDetail_GetAllMethod", "Id", id, "Activity", "ReadByInvGrnID");
    }

    /** DAL 0429 GetDate: DocumentTypeId 169 -> Sp_InvGrnDetail_GetAllMethod @Id 'ReadByInvGrnIdEmptyBagsDetail'. */
    public List<Map<String, Object>> emptyBags(int id) {
        return db.rows("Sp_InvGrnDetail_GetAllMethod", "Id", id, "Activity", "ReadByInvGrnIdEmptyBagsDetail");
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
