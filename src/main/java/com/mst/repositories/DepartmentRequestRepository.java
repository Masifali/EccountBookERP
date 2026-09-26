package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.StoreIssuanceRepository.str;
import static com.mst.repositories.StoreIssuanceRepository.toDouble;
import static com.mst.repositories.StoreIssuanceRepository.toInt;
import static com.mst.repositories.support.DesktopProc.params;

/**
 * Screen 338 "Department Request" (frmDepartmentRequest.cs, DocumentTypeId 450) — every procedure
 * call the form makes, with parameter names, order and conditions exactly as the desktop builds them.
 *
 *   BLL 0068 Architecture.BLL.DepartmentRequest       Save / FormHistory / GetByID / GenerateDocNo /
 *                                                      DepartmentRequestHistory (slip) / DeleteByID
 *   DAL 0062 Architecture.DAL.DepartmentRequest       SetData (one transaction) / GetDate (ReadById + detail)
 *   Model 0073 DepartmentRequest, 0074 DepartmentRequestDetail  (SetProc parameter order)
 *   BLL 0067 Department.GetAll                        Sp_Department_GetAllMethod 'ReadAll'
 *   BLL 0223 FixedAssetsRegister.GetAll               Sp_FixedAssetsRegister_GetAllMethod 'ReadAll'
 *   BLL 0583 Item.GetItemIdByBarcodeNo                Sp_Item_GetAllMethod 'GetItemIdByBarcodeNo'
 *   clsGlobalVariables.getGlobalAllItems              USP_Item_AllItemsWithModal
 *   clsGlobalVariables.globalUomSchedule              usp_getAllUomsByCompanyId (@Active = 1)
 *
 * Shared reads (item conditions, configuration, stock in hand) are reused from
 * {@link StoreIssuanceRepository}; nothing is duplicated here.
 */
@Repository
public class DepartmentRequestRepository {

    public static final String P_GETALL = "Sp_DepartmentRequest_GetAllMethod";

    private final JdbcTemplate jdbc;
    public DepartmentRequestRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    // ============================================================================ numbering

    /** BLL 0068 GenerateDocNo — FinancialYearId / BranchId only when non-zero; reads DocNo. */
    public int generateDocNo(UserAccount u, int documentTypeId, int financialYearId, int branchId) {
        Map<String, Object> p = params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "DocumentTypeId", documentTypeId);
        if (financialYearId != 0) p.put("FinancialYearId", financialYearId);
        if (branchId != 0)        p.put("BranchId", branchId);
        p.put("Activity", "GenerateDocNo");
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, P_GETALL, p);
        return r.isEmpty() ? 0 : toInt(r.get(0).get("DocNo"));
    }

    // ============================================================================== combos

    /** frmDepartmentRequest.DepartmentDbCall:367 → BLL 0067 Department.GetAll. */
    public List<Map<String, Object>> departments(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "Sp_Department_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "Activity", "ReadAll"))) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get("Id")));
            o.put("DepartmentName", str(r.get("DepartmentName")));
            out.add(o);
        }
        return out;
    }

    /** frmDepartmentRequest.FixedAssestDbCall:498 → BLL 0223 FixedAssetsRegister.GetAll. */
    public List<Map<String, Object>> assets(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "Sp_FixedAssetsRegister_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "Activity", "ReadAll"))) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get("Id")));
            o.put("AssetName", str(r.get("AssetName")));
            out.add(o);
        }
        return out;
    }

    /**
     * frmDepartmentRequest.ItemDtsFillFromGlobal:402 — clsGlobalVariables.getGlobalAllItems
     * (USP_Item_AllItemsWithModal) kept when ItemTypeOfTypeId is 17, or 14/17 when the
     * configuration PackingMaterialItemsNotIncludeOnStore is off. Columns: Id, ItemName, ItemCode.
     */
    public List<Map<String, Object>> items(UserAccount u, boolean packingMaterialNotIncluded) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "[dbo].[USP_Item_AllItemsWithModal]",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()))) {
            int tot = toInt(r.get("ItemTypeOfTypeId"));
            boolean allowed = packingMaterialNotIncluded ? tot == 17 : (tot == 14 || tot == 17);
            if (!allowed) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get("Id")));
            o.put("ItemName", str(r.get("ItemName")));
            o.put("ItemCode", str(r.get("ItemCode")));
            out.add(o);
        }
        return out;
    }

    /**
     * clsGlobalVariables.globalUomSchedule (DatatableHelper "UomSchedule":
     * getAllUomsByCompanyId(org, comp, 0, 1)) — CommonServices.dtUomFromGloablUomScheduleByItemId
     * filters it by ItemId on the page.
     */
    public List<Map<String, Object>> uoms(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "usp_getAllUomsByCompanyId",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Active", 1))) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get("Id")));
            o.put("ItemId", toInt(r.get("ItemId")));
            o.put("UOMCode", str(r.get("UOMCode")));
            o.put("Equivalent", toDouble(r.get("Equivalent")));
            out.add(o);
        }
        return out;
    }

    /** frmDepartmentRequest.GetItemIdByBarcode:1589 → BLL 0583 Item.GetItemIdByBarcodeNo. */
    public int itemIdByBarcode(UserAccount u, String barcode) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_Item_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "BarcodeNo", barcode,
                "Activity", "GetItemIdByBarcodeNo"));
        return r.isEmpty() ? 0 : toInt(r.get(0).get("Id"));
    }

    // ============================================================================== history

    /** BLL 0068 FormHistory — every optional parameter under the BLL's own condition. */
    public List<Map<String, Object>> formHistory(UserAccount u, int documentTypeId, int financialYearId, int branchesId,
                                                 Timestamp fromDate, Timestamp toDate,
                                                 Timestamp entryFrom, Timestamp entryTo,
                                                 Timestamp modifyFrom, Timestamp modifyTo,
                                                 Timestamp approvedFrom, Timestamp approvedTo,
                                                 int docNoFrom, int docNoTo) {
        Map<String, Object> p = params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "DocumentTypeId", documentTypeId);
        if (financialYearId != 0) p.put("FinancialYearId", financialYearId);
        if (branchesId != 0)      p.put("BranchId", branchesId);
        if (fromDate != null)     p.put("FromDate", fromDate);
        if (toDate != null)       p.put("ToDate", toDate);
        if (entryFrom != null)    p.put("EntryFromDate", entryFrom);
        if (entryTo != null)      p.put("EntryToDate", entryTo);
        if (modifyFrom != null)   p.put("ModifyFromDate", modifyFrom);
        if (modifyTo != null)     p.put("ModifyToDate", modifyTo);
        if (approvedFrom != null) p.put("ApprovedFromDate", approvedFrom);
        if (approvedTo != null)   p.put("ApprovedToDate", approvedTo);
        if (docNoFrom != 0)       p.put("DocNoFrom", docNoFrom);
        if (docNoTo != 0)         p.put("DocNoTo", docNoTo);
        p.put("Activity", "ReadAll");
        return DesktopProc.rows(jdbc, P_GETALL, p);
    }

    // ================================================================================= read

    /** BLL 0068 GetByID → DAL 0062 GetDate: the header (ReadById), or null when none. */
    public Map<String, Object> header(int id) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, P_GETALL, params("Id", id, "Activity", "ReadById"));
        return r.isEmpty() ? null : r.get(0);
    }

    /** DAL 0062 GetDate — Sp_DepartmentRequestDetail_GetByDepartmentRequestId(@DeptReqId). */
    public List<Map<String, Object>> details(int deptReqId) {
        return DesktopProc.rows(jdbc, "Sp_DepartmentRequestDetail_GetByDepartmentRequestId", params("DeptReqId", deptReqId));
    }

    /**
     * CommonServices.DepartmentSlip451 → BLL 0068 DepartmentRequestHistory with Id, DocumentTypeId 450
     * and ApprovedFilter "All". Every other ReportsParameters field is 0, so only @Id (when non-zero)
     * and @DocumentTypeId go with org/company.
     */
    public List<Map<String, Object>> slip(UserAccount u, int documentTypeId, int id) {
        Map<String, Object> p = params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId());
        if (documentTypeId != 0) p.put("DocumentTypeId", documentTypeId);
        if (id != 0)             p.put("Id", id);
        return DesktopProc.rows(jdbc, "Sp_DepartmentRequest_History", p);
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
}
