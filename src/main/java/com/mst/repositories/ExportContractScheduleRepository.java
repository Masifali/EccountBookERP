package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import java.util.LinkedHashSet;
import java.util.List;
import java.util.Map;
import java.util.Set;

import static com.mst.repositories.support.DesktopProc.params;

/**
 * 216 "Export Contract Schedule" - Architecture.WinApp.Export.frmSaleContractSchedule (rights screen
 * name FrmExportSalesContractSchedule, DocumentTypeId 244). BLL ExImLcOrderShipmentSchedule (0457),
 * ExImLcOrderShipmentScheduleHeader (0468), ExImLcContractScheduleDepartment (0449), ExportContractSchedule
 * (0443), ExImLcOrder (0469), InvProductionPlant (0336), ClientCustomGroup (0288); DAL
 * ExImLcOrderShipmentSchedule (0509). Procedures, from procdure.utf8.sql (01-Oct-2026):
 *
 *   USp_ExImLcOrderShipmentSchedule_GetAllMethod 'GridContactsExistInScheduleOrNot' @OrganizationId @CompanyId [@ActionId] [@NoOfRecords]   pending contracts grid
 *   USp_ExImLcOrderShipmentSchedule_GetAllMethod 'ReadById' @Id (= contract id)                       schedule main rows of a contract
 *   USp_ExImLcOrderShipmentSchedule_GetAllMethod 'ReadDetailByLcorderId' @Id                          schedule packing detail rows
 *   USp_ExImLcOrderShipmentSchedule_GetAllMethod 'GetAttentiveLoadDateByLcOrderId' @Id [@DetailRecIds]   Loading Date combo
 *   USp_ExImLcOrderShipmentSchedule_GetAllMethod 'GetNoOfContainersByScheduleAndLcOrderId' @Id @ScheduleId
 *   USp_ExImLcOrderShipmentSchedule_GetAllMethod 'GetLatestAttentiveLoadDate' @OrganizationId @CompanyId
 *   Sp_ExImLcOrder_GetAllMethod 'GetItemsbySalesContract' @OrganizationId @CompanyId @Id              Item / Brand combo
 *   USP_ExImLcOrderShipmentSchedule_Insert / _Update  the 29 non-virtual ExImLcOrderShipmentSchedule properties (DAL SetData: Id > 0 -> Update)
 *   USP_ExImLcOrderShipmentSchedulePackingDetail_Insert the 12 non-virtual ExImLcOrderShipmentSchedulePackingDetail properties,
 *     then [dbo].[usp_PackingMaterialRequirementSchedule_PlanningAutoInsert] @ContractScheduleId @ContractId and
 *     [dbo].[USP_ExImScheduleDetailWithPm_UpdateStatus] @ContractScheduleId @ContractId once per distinct (schedule, contract)
 *   [dbo].[USP_ExImLcContractScheduleDepartment_Insert] the 13 non-virtual ExImLcContractScheduleDepartment properties
 *   [dbo].[USP_GetDatForContractScheduleDepartment] @OrganizationId @CompanyId [@FromDate] [@ToDate]
 *   USP_ContractSchedule_FormHistory @OrganizationId @CompanyId [@FromDate] [@ToDate] [@SupplierCustomerId] [@DestinationPortId] [@ItemId]
 *   USP_ContractSchedule_StatusReport (print 551_01 - through ReportRegistry)
 *   USP_GetDataForDropDownFromShipmentSchedule @OrganizationId @CompanyId                            history Port / Customer / Item combos
 *   [sdt].[USP_ShipmentDocumentSchedule_AutoInsert] @RefDocumentTypeId @RefDocId @CustomGroupId      "Document Custom Group Save" grid button
 *   [sdt].[USP_GetCustomGroupAllocatedToCustomerBySupplierCustomerId] @OrganizationId @CompanyId @SupplierCustomerId
 *   Sp_SeaPorts_GetAllMethod 'ReadByCompanyNOrganizationId' | Sp_InvCropYear_GetAllMethod 'ReadAll' | Sp_ExImPackMaterilaType_GetAllMethod 'ReadAll'
 *   Sp_UOMSchedule_GetAllMethod 'ReadByItemID' @ItemId | Sp_InvProductionPlant_GetAllMethod 'GetALL'
 *
 * No table, column or procedure is created or changed. The attachment file copy of the DAL is not
 * part of this port.
 */
@Repository
public class ExportContractScheduleRepository {

    private static final String SCH = "USp_ExImLcOrderShipmentSchedule_GetAllMethod";

    private final JdbcTemplate jdbc;

    public ExportContractScheduleRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    private static Map<String, Object> tenant(UserAccount u) {
        return params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
    }

    /** ExImLcOrderShipmentSchedule.GridContactsExistInScheduleOrNot(org, comp, ActionId, NoOfRecords) - zeros are not sent. */
    public List<Map<String, Object>> pendingContracts(UserAccount u, int actionId, int noOfRecords) {
        Map<String, Object> p = tenant(u);
        if (actionId != 0) p.put("ActionId", actionId);
        if (noOfRecords != 0) p.put("NoOfRecords", noOfRecords);
        p.put("Activity", "GridContactsExistInScheduleOrNot");
        return DesktopProc.rows(jdbc, SCH, p);
    }

    /** ExImLcOrderShipmentSchedule.GetScheduleByContractId. */
    public List<Map<String, Object>> scheduleByContract(int contractId) {
        return DesktopProc.rows(jdbc, SCH, params("Id", contractId, "Activity", "ReadById"));
    }

    /** ExImLcOrderShipmentSchedule.GetPackingDetailByContractId. */
    public List<Map<String, Object>> packingDetailByContract(int contractId) {
        return DesktopProc.rows(jdbc, SCH, params("Id", contractId, "Activity", "ReadDetailByLcorderId"));
    }

    /** ExImLcOrderShipmentSchedule.GetAttentiveLoadDateByLcOrderId(Id, DetailRecIds) - @DetailRecIds only when not blank. */
    public List<Map<String, Object>> attentiveLoadDates(int contractId, String detailRecIds) {
        Map<String, Object> p = params("Id", contractId);
        if (detailRecIds != null && !detailRecIds.trim().isEmpty()) p.put("DetailRecIds", detailRecIds);
        p.put("Activity", "GetAttentiveLoadDateByLcOrderId");
        return DesktopProc.rows(jdbc, SCH, p);
    }

    /** ExImLcOrderShipmentSchedule.GetNoOfContainersByScheduleAndLcOrderId - NoOfContainer, NetWeight. */
    public List<Map<String, Object>> containersBySchedule(int contractId, int scheduleId) {
        return DesktopProc.rows(jdbc, SCH, params("Id", contractId, "ScheduleId", scheduleId, "Activity", "GetNoOfContainersByScheduleAndLcOrderId"));
    }

    /** ExImLcOrderShipmentSchedule.GetLatestAttentiveLoadDate - the first cell of the first row. */
    public Object latestAttentiveLoadDate(UserAccount u) {
        Map<String, Object> p = tenant(u); p.put("Activity", "GetLatestAttentiveLoadDate");
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, SCH, p);
        return r.isEmpty() ? null : r.get(0).values().iterator().next();
    }

    /** ExImLcOrder.GetItemsbySalesContract(org, comp, Id). */
    public List<Map<String, Object>> itemsBySalesContract(UserAccount u, int contractId) {
        Map<String, Object> p = tenant(u); p.put("Id", contractId); p.put("Activity", "GetItemsbySalesContract");
        return DesktopProc.rows(jdbc, "Sp_ExImLcOrder_GetAllMethod", p);
    }

    /**
     * DAL ExImLcOrderShipmentSchedule.SetData: each list row (removed ActionId 3 rows first, then the grid
     * rows) through USP_ExImLcOrderShipmentSchedule_Update when Id > 0 else _Insert; empty list ->
     * "No new record found for save-update.Because either list is empty or all record is referred!".
     */
    @Transactional(rollbackFor = Exception.class)
    public int saveScheduleMain(List<Map<String, Object>> rows) {
        if (rows == null || rows.isEmpty())
            throw new IllegalStateException("No new record found for save-update.Because either list is empty or all record is referred!");
        int result = 0;
        for (Map<String, Object> r : rows) {
            int id = r.get("Id") instanceof Number ? ((Number) r.get("Id")).intValue() : 0;
            result = DesktopProc.setProc(jdbc, id > 0 ? "USP_ExImLcOrderShipmentSchedule_Update" : "USP_ExImLcOrderShipmentSchedule_Insert", r);
        }
        return result;
    }

    /**
     * DAL ExImLcOrderShipmentSchedule.SetDataForPackingDetail: every row through
     * USP_ExImLcOrderShipmentSchedulePackingDetail_Insert, then the two planning procedures once per
     * distinct (ExImLcOrderShipmentScheduleId, ExImLcOrderId); empty list -> the DAL's message.
     */
    @Transactional(rollbackFor = Exception.class)
    public int saveScheduleDetail(List<Map<String, Object>> rows) {
        if (rows == null || rows.isEmpty())
            throw new IllegalStateException("No new detail record found. Because either list is empty or record is referred");
        int result = 0;
        Set<String> keys = new LinkedHashSet<>();
        for (Map<String, Object> r : rows) {
            result = DesktopProc.setProc(jdbc, "USP_ExImLcOrderShipmentSchedulePackingDetail_Insert", r);
            keys.add(r.get("ExImLcOrderShipmentScheduleId") + "|" + r.get("ExImLcOrderId"));
        }
        for (String k : keys) {
            String[] kv = k.split("\\|");
            DesktopProc.rows(jdbc, "[dbo].[usp_PackingMaterialRequirementSchedule_PlanningAutoInsert]",
                    params("ContractScheduleId", Integer.parseInt(kv[0]), "ContractId", Integer.parseInt(kv[1])));
        }
        for (String k : keys) {
            String[] kv = k.split("\\|");
            DesktopProc.rows(jdbc, "[dbo].[USP_ExImScheduleDetailWithPm_UpdateStatus]",
                    params("ContractScheduleId", Integer.parseInt(kv[0]), "ContractId", Integer.parseInt(kv[1])));
        }
        return result;
    }

    /** ExportContractSchedule.CustomGroupDocumentSave(244, Id, CustomGroupId). */
    @Transactional(rollbackFor = Exception.class)
    public void customGroupDocumentSave(int documentTypeId, int id, int customGroupId) {
        DesktopProc.rows(jdbc, "[sdt].[USP_ShipmentDocumentSchedule_AutoInsert]",
                params("RefDocumentTypeId", documentTypeId, "RefDocId", id, "CustomGroupId", customGroupId));
    }

    /** ExImLcContractScheduleDepartment.Save - one SetProc, Id 0 -> ActionTypeId 1 else 2. */
    @Transactional(rollbackFor = Exception.class)
    public int saveDepartment(Map<String, Object> model) {
        int num = DesktopProc.setProc(jdbc, "[dbo].[USP_ExImLcContractScheduleDepartment_Insert]", model);
        return num > 0 ? num : (Integer) model.get("Id");
    }

    /** ExImLcContractScheduleDepartment.GetDatForContractScheduleDepartment - dates only when ticked. */
    public List<Map<String, Object>> departmentRows(UserAccount u, java.sql.Date from, java.sql.Date to) {
        Map<String, Object> p = tenant(u);
        if (from != null) p.put("FromDate", from);
        if (to != null) p.put("ToDate", to);
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetDatForContractScheduleDepartment]", p);
    }

    /** ExImLcOrderShipmentSchedule.ContractSchedule_FormHistory - zero / null filters are not sent (NoOfRecords is never set by the form). */
    public List<Map<String, Object>> formHistory(UserAccount u, java.sql.Date from, java.sql.Date to, int supplierCustomerId, int destinationPortId, int itemId) {
        Map<String, Object> p = tenant(u);
        if (from != null) p.put("FromDate", from);
        if (to != null) p.put("ToDate", to);
        if (supplierCustomerId != 0) p.put("SupplierCustomerId", supplierCustomerId);
        if (destinationPortId != 0) p.put("DestinationPortId", destinationPortId);
        if (itemId != 0) p.put("ItemId", itemId);
        return DesktopProc.rows(jdbc, "USP_ContractSchedule_FormHistory", p);
    }

    /** ExImLcOrderShipmentScheduleHeader.GetDataForDropDownFromShipmentSchedule(org, comp, null) - Id, ReferenceName, Activity. */
    public List<Map<String, Object>> historyDropDowns(UserAccount u) {
        return DesktopProc.rows(jdbc, "USP_GetDataForDropDownFromShipmentSchedule", tenant(u));
    }

    public List<Map<String, Object>> seaPorts(UserAccount u) {
        Map<String, Object> p = tenant(u); p.put("Activity", "ReadByCompanyNOrganizationId");
        return DesktopProc.rows(jdbc, "Sp_SeaPorts_GetAllMethod", p);
    }

    public List<Map<String, Object>> cropYears(UserAccount u) {
        Map<String, Object> p = tenant(u); p.put("Activity", "ReadAll");
        return DesktopProc.rows(jdbc, "Sp_InvCropYear_GetAllMethod", p);
    }

    public List<Map<String, Object>> packTypes(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_ExImPackMaterilaType_GetAllMethod", params("Activity", "ReadAll", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    /** CommonServices.GetUomScheduleByItemId - Id, UOMCode, Equivalent, QtyEquivalent, BaseRateUom. */
    public List<Map<String, Object>> uomByItem(UserAccount u, int itemId) {
        Map<String, Object> p = tenant(u); p.put("ItemId", itemId); p.put("Activity", "ReadByItemID");
        return DesktopProc.rows(jdbc, "Sp_UOMSchedule_GetAllMethod", p);
    }

    /** InvProductionPlant.GetAll - Sp_InvProductionPlant_GetAllMethod 'GetALL'. */
    public List<Map<String, Object>> productionPlants(UserAccount u) {
        Map<String, Object> p = tenant(u); p.put("Activity", "GetALL");
        return DesktopProc.rows(jdbc, "Sp_InvProductionPlant_GetAllMethod", p);
    }

    public List<Map<String, Object>> customGroupsByCustomer(UserAccount u, int supplierCustomerId) {
        Map<String, Object> p = tenant(u); p.put("SupplierCustomerId", supplierCustomerId);
        return DesktopProc.rows(jdbc, "[sdt].[USP_GetCustomGroupAllocatedToCustomerBySupplierCustomerId]", p);
    }

    /** GlobalVariables_Helper.GetConfigValueFromGlobal(name). */
    public String config(UserAccount u, String description) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_ConfigrationsAllocation_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "ConfigDescription", description, "Activity", "GetConfigurationByOrgCompandConfigDescription"));
        if (r.isEmpty()) return "";
        Object v = r.get(0).get("ConfigKey");
        return v == null ? "" : String.valueOf(v).trim();
    }

    public boolean erpFeature(UserAccount u, int featureId) {
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "USP_GetERPFeaturesByCompanyId", tenant(u))) {
            Object id = r.get("Id");
            if (id instanceof Number && ((Number) id).intValue() == featureId) return true;
            if (id != null && String.valueOf(id).trim().equals(String.valueOf(featureId))) return true;
        }
        return false;
    }
}
