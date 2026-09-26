package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.StoreIssuanceRepository.ci;
import static com.mst.repositories.StoreIssuanceRepository.str;
import static com.mst.repositories.StoreIssuanceRepository.toDouble;
import static com.mst.repositories.StoreIssuanceRepository.toInt;
import static com.mst.repositories.support.DesktopProc.params;

/**
 * Screen 349 "Department Request To Consumable Store" (DepartmentRequestToConsumableStore.cs,
 * DocumentTypeId 1615) — every procedure call the form makes, with parameter names, order and
 * conditions exactly as the desktop BLL builds them.
 *
 *   BLL 0068 Architecture.BLL.DepartmentRequest      GenerateDocNo / GetIdByDocNo / GetByID / Save /
 *                                                     DepartmentRequestHistory (history + slip) / DeleteByID
 *   DAL 0062 Architecture.DAL.DepartmentRequest      SetData (one transaction) / GetDate (header + detail)
 *   Model 0073 DepartmentRequest, 0074 DepartmentRequestDetail  (SetProc parameter order)
 *   BLL 0379 GlobalServicesMethods.getGlobalActiveWarehouse   USP_GetWarehousesAllocatedToBranch
 *   BLL 0078 Projects.GetAlldt                       Sp_Projects_GetAllMethod @MethodType 'GetAll'
 *   BLL 0583 Item.GetItemByItemTypeId / GetItemIdByBarcodeNo   Sp_Item_GetAllMethod
 *   BLL 0610 UOMSchedule.SearchByObject              Sp_UOMSchedule_GetAllMethod 'ReadByItemID'
 *   BLL 0223 FixedAssetsRegister.GetAll              Sp_FixedAssetsRegister_GetAllMethod 'ReadAll'
 *   BLL 0368 Mfg.WorkOrder.GetWorkOrderNoByWorkStationId  [Mfg].[USP_WorkOrder_GetAllMethod]
 *   BLL 0056 GetAvgRatesAndStockInHand.GetStockInHandFromInventoryTrasactions
 *                                                     Sp_GetAvgRatesAndStockInHand_GetAllMethod
 *
 * Nothing is shared with {@link DepartmentRequestRepository} (screen 338) on purpose: that form uses
 * newer BLL signatures (FormHistory, branch-aware numbering) this form never calls.
 */
@Repository
public class DeptRequestToConsumableRepository {

    public static final String P_GETALL = "Sp_DepartmentRequest_GetAllMethod";

    private final JdbcTemplate jdbc;
    public DeptRequestToConsumableRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    // ============================================================================ numbering

    /**
     * GenerateCode (Form.cs:293) → BLL 0068 GenerateDocNo with org, company, DocTypeId and the active
     * year. The model's BranchId is never set (0), so @BranchId is not sent.
     */
    public int generateDocNo(UserAccount u, int documentTypeId, int financialYearId) {
        Map<String, Object> p = params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "DocumentTypeId", documentTypeId);
        if (financialYearId != 0) p.put("FinancialYearId", financialYearId);
        p.put("Activity", "GenerateDocNo");
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, P_GETALL, p);
        return r.isEmpty() ? 0 : toInt(ci(r.get(0), "DocNo"));
    }

    /** txtdocno_Leave (Form.cs:873) → BLL 0068 GetIdByDocNo (no @BranchId: never in that BLL). */
    public int idByDocNo(UserAccount u, int documentTypeId, int docNo, int financialYearId) {
        Map<String, Object> p = params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "DocumentTypeId", documentTypeId,
                "DocNo", docNo);
        if (financialYearId != 0) p.put("FinancialYearId", financialYearId);
        p.put("Activity", "GetIdByDocNo");
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, P_GETALL, p);
        return r.isEmpty() ? 0 : toInt(ci(r.get(0), "Id"));
    }

    // ============================================================================== combos

    /**
     * DeprtmentFill (Form.cs:318) — clsGlobalVariables.globalWarehousesWithBranches, loaded by
     * DatatableHelper "WarehousesWithBranches" → getGlobalActiveWarehouse(org, comp, BranchesId)
     * → USP_GetWarehousesAllocatedToBranch; kept where IsActive and WareHouseTypeId == 4, projected
     * to Id (WarehouseId) / Description (WarehouseName) by PopulateDataTableAndReturn. No distinct.
     */
    public List<Map<String, Object>> departments(UserAccount u, int branchesId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "USP_GetWarehousesAllocatedToBranch", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "BranchId", branchesId))) {
            if (!bool(ci(r, "IsActive"))) continue;
            if (toInt(ci(r, "WareHouseTypeId")) != 4) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("Description", str(ci(r, "WarehouseName")));
            out.add(o);
        }
        return out;
    }

    /** ProjectFill (Form.cs:334) → CommonServices.ProjectServiceBind → BLL 0078 Projects.GetAlldt. */
    public List<Map<String, Object>> projects(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "Sp_Projects_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "MethodType", "GetAll"))) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("ProjectName", str(ci(r, "ProjectName")));
            out.add(o);
        }
        return out;
    }

    /** ItemNameFill (Form.cs:352) → CommonServices.GetItemByItemTypeId("14,17") → BLL 0583. */
    public List<Map<String, Object>> items(UserAccount u, String lookupTypeIds) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "Sp_Item_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "LookupTypeIds", lookupTypeIds,
                "Activity", "GetItemByItemTypeId"))) {
            Map<String, Object> o = new LinkedHashMap<>();                     // dtitem: Id, ItemName, ItemCode(=ItemCodeNew)
            o.put("Id", toInt(ci(r, "Id")));
            o.put("ItemName", str(ci(r, "ItemName")));
            o.put("ItemCode", str(ci(r, "ItemCodeNew")));
            out.add(o);
        }
        return out;
    }

    /** ItemUOMFill (Form.cs:397) → BLL 0610 UOMSchedule.SearchByObject (org, company, item). */
    public List<Map<String, Object>> uoms(UserAccount u, int itemId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "Sp_UOMSchedule_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "ItemId", itemId,
                "Activity", "ReadByItemID"))) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("UOMCode", str(ci(r, "UOMCode")));
            out.add(o);
        }
        return out;
    }

    /** FixedAssest (Form.cs:440) → BLL 0223 FixedAssetsRegister.GetAll. */
    public List<Map<String, Object>> assets(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "Sp_FixedAssetsRegister_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "Activity", "ReadAll"))) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("AssetName", str(ci(r, "AssetName")));
            out.add(o);
        }
        return out;
    }

    /**
     * WorkOrderFill (Form.cs:478) → BLL 0368 WorkOrder.GetWorkOrderNoByWorkStationId(org, comp, 0):
     * @Id is 0 so not sent. The procedure joins the work-order operations without DISTINCT, so an
     * order with several operations is listed several times — kept.
     */
    public List<Map<String, Object>> workOrders(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "[Mfg].[USP_WorkOrder_GetAllMethod]", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "Activity", "GetWorkOrderNoByWorkStationId"))) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("WorkOrderNo", str(ci(r, "WorkOrderNo")));
            out.add(o);
        }
        return out;
    }

    /** BalanceStock (Form.cs:1353) → BLL 0056 GetStockInHandFromInventoryTrasactions: Rows[0][0], else 0. */
    public double stockInHand(UserAccount u, int itemId, java.sql.Timestamp docDate) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_GetAvgRatesAndStockInHand_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "ItemId", itemId,
                "DocDate", docDate,
                "Activity", "GetStockInHandFromInventoryTrasactions"));
        return r.isEmpty() ? 0d : toDouble(ci(r.get(0), "CurrStockByItem"));   // the only column (Rows[0][0])
    }

    /** GetItemIdByBarcode (Form.cs:1445) → BLL 0583 Item.GetItemIdByBarcodeNo. */
    public int itemIdByBarcode(UserAccount u, String barcode) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_Item_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "BarcodeNo", barcode,
                "Activity", "GetItemIdByBarcodeNo"));
        return r.isEmpty() ? 0 : toInt(ci(r.get(0), "Id"));
    }

    // ============================================================================ history / slip

    /**
     * BLL 0068 DepartmentRequestHistory — HistoryGridFill (Form.cs:929) and CommonServices
     * .DepartmentRequestSlip1615 both fill only org, company, DocumentTypeId (and Id for the slip)
     * with ApprovedFilter "All"; every other ReportsParameters field is 0/null and the BLL skips it.
     * The BLL has no NoOfRecords parameter, so the form's HistoryGridFill(50) is never sent.
     */
    public List<Map<String, Object>> history(UserAccount u, int documentTypeId, int id) {
        Map<String, Object> p = params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId());
        if (documentTypeId != 0) p.put("DocumentTypeId", documentTypeId);
        if (id != 0)             p.put("Id", id);
        return DesktopProc.rows(jdbc, "Sp_DepartmentRequest_History", p);
    }

    // ================================================================================= read

    /** BLL 0068 GetByID → DAL 0062 GetDate: the header (ReadById, ActionId &lt;&gt; 3) or null. */
    public Map<String, Object> header(int id) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, P_GETALL, params("Id", id, "Activity", "ReadById"));
        return r.isEmpty() ? null : r.get(0);
    }

    /** DAL 0062 GetDate — Sp_DepartmentRequestDetail_GetByDepartmentRequestId(@DeptReqId). */
    public List<Map<String, Object>> details(int deptReqId) {
        return DesktopProc.rows(jdbc, "Sp_DepartmentRequestDetail_GetByDepartmentRequestId", params("DeptReqId", deptReqId));
    }

    // ================================================================================ write

    /** GenericProvider.SetProc — Convert.ToInt32(ExecuteScalar()). */
    public int setProc(String proc, Map<String, Object> model) {
        return DesktopProc.setProc(jdbc, proc, model);
    }

    /** BLL 0068 DeleteByID — GetDataTableProc on its own connection (no transaction). */
    public void delete(int entryUser, int id) {
        DesktopProc.rows(jdbc, "[dbo].[" + P_GETALL + "]", params(
                "EntryUser", entryUser, "Id", id, "Activity", "DeleteById"));
    }

    private static boolean bool(Object v) {
        if (v instanceof Boolean) return (Boolean) v;
        if (v instanceof Number) return ((Number) v).intValue() != 0;
        String s = v == null ? "" : String.valueOf(v).trim();
        return "true".equalsIgnoreCase(s) || "1".equals(s);
    }
}
