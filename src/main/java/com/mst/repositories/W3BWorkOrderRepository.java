package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.sql.Timestamp;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.support.DesktopProc.params;

/**
 * W3B - Mfg Production hub (module 103), the Work Order family: 593 frmWorkOrder (DocumentTypeId 1614),
 * 594 frmWorkOrderInProgress (1620), 781 frmWorkOrderForSemiFinish (1621). BLL Architecture.BLL.Mfg.WorkOrder / BomHeader /
 * WorkOrderInProgress (assembly not in the recovered source; each call is mapped to the [Mfg] procedure of the same name,
 * parameters checked against procdure_index.csv):
 *
 * <pre>
 *   WorkOrder.GenerateCode / ReadById / FormHistory / DeleteByID   [Mfg].[USP_WorkOrder_GetAllMethod]
 *        'GenerateCode' | 'ReadById' + 'ReadByHeaderId_WorkOrderOperationDetail' | '..._workOrderMaterial' |
 *        '..._WorkOrderCustomerDetail' | '..._workOrderItemDetail' | 'FormHistory' | 'DeleteById'
 *   WorkOrder.Save                 [Mfg].[USP_WorkOrder_Insert] | _Update, then [USP_WorkOrderOperation_Insert],
 *                                  [USP_workOrderMaterial_Insert], [USP_WorkOrderCustomer_Insert], [USP_workOrderItemDetail_Insert]
 *   WorkOrder.GetDataForDropDownFromWorkOrder   [Mfg].[USP_GetDataForDropDownFromWorkOrder]
 *   BomHeader.GetBomNoByItemId     [Mfg].[USP_BomHeader_BOMNameDropDown]
 *   BomHeader.ReadByHeaderId_BomOperationDetail   [Mfg].[USP_BomHeader_GetAllMethod] 'ReadByHeaderId_BomOperationDetail'
 *   SaleOrder.SaleOrder_InfoByItemId   [dbo].[USP_SaleOrder_InfoByItemId]
 *   WorkOrderInProgress ...        [Mfg].[USP_WorkOrderInProgress_GetAllMethod] / _InsertAndUpdate / [USP_WorkOrderInProgressDetail_Insert]
 * </pre>
 */
@Repository
public class W3BWorkOrderRepository {

    public static final String P_WO = "[Mfg].[USP_WorkOrder_GetAllMethod]";
    public static final String P_WIP = "[Mfg].[USP_WorkOrderInProgress_GetAllMethod]";

    private final JdbcTemplate jdbc;
    public W3BWorkOrderRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    public List<Map<String, Object>> rows(String proc, Map<String, Object> p) { return DesktopProc.rows(jdbc, proc, p); }
    public int setProc(String proc, Map<String, Object> p) { return DesktopProc.setProc(jdbc, proc, p); }

    // ------------------------------------------------------------------------------ Work Order (593 / 781)

    /** WorkOrder.GenerateCode: DocNo of row 0 ('GenerateCode'). */
    public int generateCode(UserAccount u, int branchId, int documentTypeId, int financialYearId) {
        List<Map<String, Object>> r = rows(P_WO, params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "BranchesId", branchId, "DocumentTypeId", documentTypeId, "FinancialYearId", financialYearId, "Activity", "GenerateCode"));
        if (r.isEmpty() || r.get(0).isEmpty()) return 0;
        Object v = r.get(0).get("DocNo");
        if (v == null) v = r.get(0).values().iterator().next();
        return v instanceof Number ? ((Number) v).intValue() : 0;
    }

    public Map<String, Object> woHeader(int id) {
        List<Map<String, Object>> r = rows(P_WO, params("Id", id, "Activity", "ReadById"));
        return r.isEmpty() ? null : r.get(0);
    }
    public List<Map<String, Object>> woOperations(int id) { return rows(P_WO, params("Id", id, "Activity", "ReadByHeaderId_WorkOrderOperationDetail")); }
    public List<Map<String, Object>> woMaterials(int id) { return rows(P_WO, params("Id", id, "Activity", "ReadByHeaderId_workOrderMaterial")); }
    public List<Map<String, Object>> woCustomers(int id) { return rows(P_WO, params("Id", id, "Activity", "ReadByHeaderId_WorkOrderCustomerDetail")); }
    public List<Map<String, Object>> woItemDetails(int id) { return rows(P_WO, params("Id", id, "Activity", "ReadByHeaderId_workOrderItemDetail")); }

    /** WorkOrder.FormHistory(ReportsParameters). A zero doc-no filter is not sent (the BLL leaves unset values out). */
    public List<Map<String, Object>> woHistory(UserAccount u, int branchId, int financialYearId, int documentTypeId, int fromDocNo, int toDocNo,
                                               Timestamp from, Timestamp to, int id, int itemId, boolean canViewAll, int entryUserId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "BranchesId", branchId,
                "FinancialYearId", financialYearId, "DocumentTypeId", documentTypeId);
        if (fromDocNo != 0) p.put("FromDocNo", fromDocNo);
        if (toDocNo != 0) p.put("ToDocNo", toDocNo);
        if (from != null) p.put("FromDate", from);
        if (to != null) p.put("ToDate", to);
        if (id != 0) p.put("Id", id);
        if (itemId != 0) p.put("ItemId", itemId);
        p.put("CanViewAllRecord", canViewAll ? 1 : 0);
        p.put("EntryUserId", entryUserId);
        p.put("Activity", "FormHistory");
        return rows(P_WO, p);
    }

    public void woDelete(int userId, int id) { rows(P_WO, params("Id", id, "EntryUserId", userId, "Activity", "DeleteById")); }

    public List<Map<String, Object>> woDropDowns(UserAccount u) {
        return rows("[Mfg].[USP_GetDataForDropDownFromWorkOrder]", params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    public int woSaveHeader(boolean insert, Map<String, Object> p) {
        return setProc(insert ? "[Mfg].[USP_WorkOrder_Insert]" : "[Mfg].[USP_WorkOrder_Update]", p);
    }
    public void woSaveOperation(Map<String, Object> p) { setProc("[Mfg].[USP_WorkOrderOperation_Insert]", p); }
    public void woSaveMaterial(Map<String, Object> p) { setProc("[Mfg].[USP_workOrderMaterial_Insert]", p); }
    public void woSaveCustomer(Map<String, Object> p) { setProc("[Mfg].[USP_WorkOrderCustomer_Insert]", p); }
    public void woSaveItemDetail(Map<String, Object> p) { setProc("[Mfg].[USP_workOrderItemDetail_Insert]", p); }

    public List<Map<String, Object>> bomNamesByItem(UserAccount u, int itemId) {
        return rows("[Mfg].[USP_BomHeader_BOMNameDropDown]", params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "ItemId", itemId));
    }

    public List<Map<String, Object>> bomOperationDetail(int bomId) {
        return rows("[Mfg].[USP_BomHeader_GetAllMethod]", params("Id", bomId, "Activity", "ReadByHeaderId_BomOperationDetail"));
    }

    public List<Map<String, Object>> saleOrderInfoByItem(UserAccount u, int itemId) {
        return rows("[dbo].[USP_SaleOrder_InfoByItemId]", params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "ItemId", itemId));
    }

    // ------------------------------------------------------------------------------ Work Order In Progress (594)

    public int wipGenerateCode(UserAccount u, int branchId, int documentTypeId, int financialYearId) {
        List<Map<String, Object>> r = rows(P_WIP, params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "BranchesId", branchId, "DocumentTypeId", documentTypeId, "FinancialYearId", financialYearId, "Activity", "GenerateCode"));
        if (r.isEmpty() || r.get(0).isEmpty()) return 0;
        Object v = r.get(0).get("DocNo");
        if (v == null) v = r.get(0).values().iterator().next();
        return v instanceof Number ? ((Number) v).intValue() : 0;
    }
}
