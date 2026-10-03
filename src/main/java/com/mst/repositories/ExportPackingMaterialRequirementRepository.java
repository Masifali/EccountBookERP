package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import java.util.List;
import java.util.Map;

import static com.mst.repositories.support.DesktopProc.params;

/**
 * 219 "Packing Material Requirement (Not Use)" - Architecture.WinApp.Export.PackingMaterialRequirement.
 * BLL ExImLcOrderMrpHeader (0459) / DAL ExImLcOrderMrpHeader (0516), plus CommonServices helpers. Procedures and
 * parameters as read from procdure.utf8.sql (01-Oct-2026):
 *
 *   Sp_Item_GetAllMethod 'GetItemByItemTypeId' @OrganizationId @CompanyId @LookupTypeIds='14'          ItemBind (cmbPMitem: Id, ItemName)
 *   Sp_SupplierCustomer_GetAllMethod 'ReadByOrganizationCompanyIdForExport' @OrganizationId @CompanyId CustomerBind (Id, CompanyName)
 *   Sp_ExImLcOrder_GetAllMethod 'GetLcOrderNoBySupplierCustomerId' @OrganizationId @CompanyId [@FinancialYearId] [@SupCustId]
 *                                                                                                     SalesContractBind (Id, LcOrderNo)
 *   Sp_ExImLcOrder_GetAllMethod 'GetItemsbySalesContract' @OrganizationId @CompanyId @Id                SalesContractDetailByContractId
 *   Sp_UOMSchedule_GetAllMethod 'ReadByItemID' @OrganizationId @CompanyId @ItemId                      bindRateUomAndItemPackUom (Id, UOMCode, Equivalent...)
 *   Sp_ExImLcOrderMrpHeader_GetAllMethod 'ReadAll' @OrganizationId @CompanyId                          HistoryBind (Id, LcOrderNo, EntryDate, UserName)
 *   Sp_ExImLcOrderMrpHeader_GetAllMethod 'ReadById' | 'ReadDetailByHeaderId' @Id                       GetByID + DAL GetDate detail
 *   Sp_ExImLcOrderMrpHeader_Insert / _Update   @ItemQty @CompanyId @ExImLcOrderId @Id @ItemId @ItemUomId @OrganizationId
 *                                              @EntryUser @ModifyUser @EntryDate @ModifyDate              DAL SetDate header
 *   Sp_ExImLcOrderMrpDetail_Insert             @ReqQty @ExImLcOrderId @ExImLcOrderMrpHeaderId @Id @ItemId @ItemIdPm
 *                                              @ItemUomId @ItemUomIdPm @SortNo [@RemarksDetail never set -> not sent]
 *
 * One transaction per save, rolled back on any error. No table, column or procedure is created or changed.
 */
@Repository
public class ExportPackingMaterialRequirementRepository {

    private final JdbcTemplate jdbc;

    public ExportPackingMaterialRequirementRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    private static Map<String, Object> tenant(UserAccount u) {
        return params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
    }

    public List<Map<String, Object>> pmItems(UserAccount u) {
        Map<String, Object> p = tenant(u); p.put("LookupTypeIds", "14"); p.put("Activity", "GetItemByItemTypeId");
        return DesktopProc.rows(jdbc, "Sp_Item_GetAllMethod", p);
    }

    public List<Map<String, Object>> customers(UserAccount u) {
        Map<String, Object> p = tenant(u); p.put("Activity", "ReadByOrganizationCompanyIdForExport");
        return DesktopProc.rows(jdbc, "Sp_SupplierCustomer_GetAllMethod", p);
    }

    /** CommonServices.GetLcOrderNoBySupplierCustomerId - FinancialYearId = the active year, SupCustId only when non-zero. */
    public List<Map<String, Object>> salesContracts(UserAccount u, int financialYearId, int supplierCustomerId) {
        Map<String, Object> p = tenant(u);
        if (financialYearId != 0) p.put("FinancialYearId", financialYearId);
        if (supplierCustomerId != 0) p.put("SupCustId", supplierCustomerId);
        p.put("Activity", "GetLcOrderNoBySupplierCustomerId");
        return DesktopProc.rows(jdbc, "Sp_ExImLcOrder_GetAllMethod", p);
    }

    public List<Map<String, Object>> itemsBySalesContract(UserAccount u, int contractId) {
        Map<String, Object> p = tenant(u); p.put("Id", contractId); p.put("Activity", "GetItemsbySalesContract");
        return DesktopProc.rows(jdbc, "Sp_ExImLcOrder_GetAllMethod", p);
    }

    public List<Map<String, Object>> uomByItem(UserAccount u, int itemId) {
        Map<String, Object> p = tenant(u); p.put("ItemId", itemId); p.put("Activity", "ReadByItemID");
        return DesktopProc.rows(jdbc, "Sp_UOMSchedule_GetAllMethod", p);
    }

    public List<Map<String, Object>> history(UserAccount u) {
        Map<String, Object> p = tenant(u); p.put("Activity", "ReadAll");
        return DesktopProc.rows(jdbc, "Sp_ExImLcOrderMrpHeader_GetAllMethod", p);
    }

    public List<Map<String, Object>> header(int id) {
        return DesktopProc.rows(jdbc, "Sp_ExImLcOrderMrpHeader_GetAllMethod", params("Id", id, "Activity", "ReadById"));
    }

    public List<Map<String, Object>> detail(int id) {
        return DesktopProc.rows(jdbc, "Sp_ExImLcOrderMrpHeader_GetAllMethod", params("Id", id, "Activity", "ReadDetailByHeaderId"));
    }

    /** DAL SetDate: header (Insert when Id == 0), then every detail with ExImLcOrderMrpHeaderId = the header id. */
    @Transactional(rollbackFor = Exception.class)
    public int save(Map<String, Object> header, List<Map<String, Object>> details) {
        int id = ((Number) header.get("Id")).intValue();
        int num = DesktopProc.setProc(jdbc, id == 0 ? "Sp_ExImLcOrderMrpHeader_Insert" : "Sp_ExImLcOrderMrpHeader_Update", header);
        if (num > 0) id = num; else num = id;
        for (Map<String, Object> d : details) {
            d.put("ExImLcOrderMrpHeaderId", id);
            DesktopProc.scalar(jdbc, "Sp_ExImLcOrderMrpDetail_Insert", d);
        }
        return num;
    }
}
