package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import com.mst.services.W5BBomProfile;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.support.DesktopProc.params;

/**
 * W5B - Bill Of Material family: 774 frmBillOfMaterial, 845 frmBillOfMaterialCasting (both [Mfg] schema) and
 * 598 frmBillOfMaterialConcrete ([pcc] schema). One repository, parameterised by {@link W5BBomProfile}.
 *
 * <pre>
 *   BomHeader.GenerateCode            [schema].[USP_BomHeader_GetAllMethod] 'GenerateCode'
 *   BomHeader.ReadById                ... 'ReadById' + 'ReadByHeaderId_BomInputDetail' / '_BomByProductDetail' / '_BomFohDetail'
 *                                         / '_BomOperationDetail' ([Mfg] 774) / '_BomWagesDetail' (774, 598)
 *   BomHeader.FormHistory             ... 'FormHistory'
 *   BomHeader.DeleteByID(user, id)    ... 'DeleteById' @Id @EntryUserId
 *   BomHeader.GetDataForDropDownFromBOM
 *                                     [Mfg].[USP_GetDataForDropDownFromBomHeader] @DocumentTypeIds  |  [pcc].[USP_GetDataForDropDownFromBomHeader]
 *   BomHeader.Save                    [schema].[USP_BomHeader_Insert] | _Update, then per row
 *                                     USP_BomInputDetail_Insert, USP_BomByProductDetail_Insert, USP_BomFohDetail_Insert,
 *                                     USP_BomOperationDetail_Insert ([Mfg] only), USP_BomWagesDetail_Insert
 * </pre>
 * The [Mfg] procedures name their key columns BomHeaderId / BomInputDetailId / BomByProductDetailId / BomFohDetailId /
 * BomOperationDetailId; the [pcc] ones use Id throughout. Callers build maps with "Id" and this class renames.
 */
@Repository
public class W5BBomRepository {

    private static final Logger LOG = LoggerFactory.getLogger(W5BBomRepository.class);

    private final JdbcTemplate jdbc;
    public W5BBomRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    private List<Map<String, Object>> rows(String proc, Map<String, Object> p) { return DesktopProc.rows(jdbc, proc, p); }

    private List<Map<String, Object>> safe(String label, String proc, Map<String, Object> p) {
        try {
            return DesktopProc.rows(jdbc, proc, p);
        } catch (RuntimeException e) {
            LOG.warn("W5B BOM lookup '{}' ({}) failed", label, proc, e);
            return new ArrayList<>();
        }
    }

    // ------------------------------------------------------------------------------ header + grids

    /** DocumentNoFill - GenerateCode(Org, Comp, Branch, DocType, ActiveYr) -> DocNo. */
    public int generateCode(W5BBomProfile p, UserAccount u, int branchId, int financialYearId) {
        List<Map<String, Object>> r = rows(p.getProc(), params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "BranchesId", branchId,
                "DocumentTypeId", p.docType, "FinancialYearId", financialYearId, "Activity", "GenerateCode"));
        if (r.isEmpty() || r.get(0).isEmpty()) return 0;
        Object v = r.get(0).get("DocNo");
        if (v == null) v = r.get(0).values().iterator().next();
        return v instanceof Number ? ((Number) v).intValue() : 0;
    }

    public Map<String, Object> header(W5BBomProfile p, int id) {
        List<Map<String, Object>> r = rows(p.getProc(), params("Id", id, "Activity", "ReadById"));
        return r.isEmpty() ? null : r.get(0);
    }

    public List<Map<String, Object>> inputRows(W5BBomProfile p, int id) { return rows(p.getProc(), params("Id", id, "Activity", "ReadByHeaderId_BomInputDetail")); }
    public List<Map<String, Object>> byProductRows(W5BBomProfile p, int id) { return rows(p.getProc(), params("Id", id, "Activity", "ReadByHeaderId_BomByProductDetail")); }
    public List<Map<String, Object>> fohRows(W5BBomProfile p, int id) { return rows(p.getProc(), params("Id", id, "Activity", "ReadByHeaderId_BomFohDetail")); }
    public List<Map<String, Object>> operationRows(W5BBomProfile p, int id) { return rows(p.getProc(), params("Id", id, "Activity", "ReadByHeaderId_BomOperationDetail")); }
    public List<Map<String, Object>> wagesRows(W5BBomProfile p, int id) { return rows(p.getProc(), params("Id", id, "Activity", "ReadByHeaderId_BomWagesDetail")); }

    /**
     * AllCombobind - one result set split on the Activity column by the service. [Mfg] takes @DocumentTypeIds
     * (the desktop sends '1617' / '1664'); [pcc] takes only Organization + Company.
     */
    public List<Map<String, Object>> dropDowns(W5BBomProfile p, UserAccount u) {
        Map<String, Object> q = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (p.mfgSchema) q.put("DocumentTypeIds", String.valueOf(p.docType));
        return safe("dropDowns", "[" + p.schema + "].[USP_GetDataForDropDownFromBomHeader]", q);
    }

    /** HistoryGridFill - every ReportsParameters value the form sets (BOMNo is the key the procedures actually filter on). */
    public List<Map<String, Object>> formHistory(W5BBomProfile p, UserAccount u, int branchId, int financialYearId,
                                                 int fromDocNo, int toDocNo, Timestamp from, Timestamp to, int bomId,
                                                 String bomType, int itemId, boolean canViewAll, int entryUserId) {
        Map<String, Object> q = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "BranchesId", branchId, "FinancialYearId", financialYearId, "DocumentTypeId", p.docType);
        if (fromDocNo != 0) q.put("FromDocNo", fromDocNo);
        if (toDocNo != 0) q.put("ToDocNo", toDocNo);
        if (from != null) q.put("FromDate", from);
        if (to != null) q.put("ToDate", to);
        if (bomType != null && !bomType.isEmpty()) q.put("BOMType", bomType);
        if (bomId != 0) q.put("BOMNo", bomId);
        if (itemId != 0) q.put("ItemId", itemId);
        q.put("CanViewAllRecord", canViewAll);
        if (!canViewAll) q.put("EntryUserId", entryUserId);
        q.put("Activity", "FormHistory");
        return rows(p.getProc(), q);
    }

    /** btnDelete_Click - BomHeader.DeleteByID(UserAccount.ID, RecId). */
    public void deleteById(W5BBomProfile p, int userId, int id) {
        rows(p.getProc(), params("Id", id, "EntryUserId", userId, "Activity", "DeleteById"));
    }

    // ------------------------------------------------------------------------------ save

    private static Map<String, Object> renamed(Map<String, Object> src, String from, String to) {
        if (from.equals(to) || !src.containsKey(from)) return src;
        Map<String, Object> m = new LinkedHashMap<>();
        for (Map.Entry<String, Object> e : src.entrySet()) m.put(from.equals(e.getKey()) ? to : e.getKey(), e.getValue());
        return m;
    }

    /** BomHeader.Save - Insert returns the new key (SELECT @Id); Update returns nothing (0). */
    public int saveHeader(W5BBomProfile p, boolean insert, Map<String, Object> head) {
        String proc = "[" + p.schema + "]." + (insert ? "[USP_BomHeader_Insert]" : "[USP_BomHeader_Update]");
        return DesktopProc.setProc(jdbc, proc, p.mfgSchema ? renamed(head, "Id", "BomHeaderId") : head);
    }
    public void insertInput(W5BBomProfile p, Map<String, Object> d) {
        DesktopProc.setProc(jdbc, "[" + p.schema + "].[USP_BomInputDetail_Insert]", p.mfgSchema ? renamed(d, "Id", "BomInputDetailId") : d);
    }
    public void insertByProduct(W5BBomProfile p, Map<String, Object> d) {
        DesktopProc.setProc(jdbc, "[" + p.schema + "].[USP_BomByProductDetail_Insert]", p.mfgSchema ? renamed(d, "Id", "BomByProductDetailId") : d);
    }
    public void insertFoh(W5BBomProfile p, Map<String, Object> d) {
        DesktopProc.setProc(jdbc, "[" + p.schema + "].[USP_BomFohDetail_Insert]", p.mfgSchema ? renamed(d, "Id", "BomFohDetailId") : d);
    }
    public void insertWages(W5BBomProfile p, Map<String, Object> d) {
        DesktopProc.setProc(jdbc, "[" + p.schema + "].[USP_BomWagesDetail_Insert]", d);
    }
    /** [Mfg].[USP_BomOperationDetail_Insert] (774 only; the [pcc] form never saves operations). */
    public void insertOperation(Map<String, Object> d) {
        DesktopProc.setProc(jdbc, "[Mfg].[USP_BomOperationDetail_Insert]", renamed(d, "Id", "BomOperationDetailId"));
    }

    // ------------------------------------------------------------------------------ lookups

    /** CommonServices.ItemGetForComboServiceBindForEngr - Sp_Item_GetAllMethod 'AllItemsBindForEngr' (Id, ItemName, ItemCodeNew). */
    public List<Map<String, Object>> itemsForEngr(UserAccount u) {
        return safe("itemsForEngr", "Sp_Item_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "AllItemsBindForEngr"));
    }

    /** UOM.Getall(org, comp) - Sp_UOM_GetAllMethod 'ReadByOrganizationCompanyId' (Id, UOMCode). */
    public List<Map<String, Object>> uomMaster(UserAccount u) {
        return safe("uomMaster", "Sp_UOM_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadByOrganizationCompanyId"));
    }

    /** BindCastingType - [Mfg].[USP_Lookups_GetAllMethod] 'GetDataByTypeId' @Id=1 (Id, LookupName). */
    public List<Map<String, Object>> castingTypes() {
        return safe("castingTypes", "[Mfg].[USP_Lookups_GetAllMethod]", params("Id", 1, "Activity", "GetDataByTypeId"));
    }

    /** InvLookUp.GetLookupsByTypeIdDt(InvLookupTypeId = 6) - Sp_InvLookup_GetAllMethod 'ReadByInvlookTypeId'. */
    public List<Map<String, Object>> invLookupType6(UserAccount u) {
        return safe("invLookupType6", "Sp_InvLookup_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "InvLookupTypeId", 6, "Activity", "ReadByInvlookTypeId"));
    }

    /** InvProductionPlant.GetAll - Sp_InvProductionPlant_GetAllMethod 'GetALL' (Id, Code, Description). */
    public List<Map<String, Object>> plants(UserAccount u) {
        return safe("plants", "Sp_InvProductionPlant_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "GetALL"));
    }

    /**
     * ContractorWagesRateSchedule.GetServiceActivityByItemId(1, ItemId, DocDate, 0, 0) -
     * [pcc].[USP_ContractorWagesRateSchedule_GetAllMethod] (ContractorWagesRateScheduleId, WagesActivity, Rate, Qty ...).
     */
    public List<Map<String, Object>> serviceActivities(int itemId, Timestamp effectedDate) {
        return rows("[pcc].[USP_ContractorWagesRateSchedule_GetAllMethod]", params(
                "ItemId", itemId, "RefDocumentTypeId", 1, "EffectedDATE", effectedDate, "Activity", "GetServiceActivityByItemId"));
    }
}
