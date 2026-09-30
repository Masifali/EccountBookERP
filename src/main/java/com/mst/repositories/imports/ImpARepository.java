package com.mst.repositories.imports;

import com.mst.models.UserAccount;
import com.mst.models.hrm.DesktopModel;
import com.mst.repositories.hrm.HrmProcRepository;
import org.springframework.stereotype.Repository;

import java.sql.Timestamp;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.function.Supplier;

/**
 * DAL of the Import screens 525 / 526 / 221 and the Import registers 392-395. Every call is the desktop
 * BLL/DAL's own procedure with the parameters it sends (verified against procdure.utf8.sql). Nothing is created.
 *
 *   525 ImLcOrder            BLL 0419 / DAL 0476   USP_ImLcOrder_GetAllMethod (GenerateDocNo, ReadById, ReadByLcOrderHeaderId,
 *                                                  ReadImLcOrderPaymnetTerm_ByLcOrderHeaderId, FormHistory, FormHistoryDetail, DeleteById),
 *                                                  USP_ImLcOrder_Insert|Update, USP_ImLcOrderPackingDetail_Insert,
 *                                                  USP_ImLcOrderPaymnetTerm_Insert, USP_ImLcOrder_Register (231 slip)
 *   526 ImLcOrderSchedule    BLL 0413 / DAL 0470   USP_ImLcOrderSchedule_GetAllMethod (ReadById, ReadDetailByLcorderId,
 *                                                  GetAttentiveLoadDateByLcOrderId, GetNoOfContainersByScheduleAndLcOrderId,
 *                                                  GridContactsExistInScheduleOrNot), USP_ImLcOrderScheduleHeader_Insert|Update,
 *                                                  USP_ImLcOrderSchedulePackingDetail_Insert, USP_ImLcOrderSchedule_FormHistory,
 *                                                  USP_GetDataForDropDownFromImportLcOrderSchedule
 *   221 ImCommercialInvoice  BLL 0418/0415/0135   Sp_ExImLcOrderPurchaseOrder_GetAllMethod (GetPurchaseOrderNo, GetImportPurchaseOrderDetailById,
 *                                                  GETIMPORTCONTRACTDETAILBYCONTRACTID, GETIMPORTGRNDETAILBYPURCHASEORDERID),
 *                                                  USP_ImLcOrder_GetAllMethod GetLcOrderNo, Sp_ImInvoice_SlipandRegister (806)
 *   392-395 registers        BLL 0135              SP_ImInvoiceRegister_WithAvgRates, Sp_ImLcOrderNo_ImportSlip_Rpt,
 *                                                  Sp_ImGRN_SlipAndRegister, Sp_ImportPurchaseOrder_Slip_rpt
 *   shared combos            CommonServices / BLL  Sp_SupplierCustomer_GetAllMethod, Sp_ExImDeliveryTerm_GetAllMethod, Sp_ExImLcPaymentTerm_GetAllMethod,
 *                                                  Sp_SeaPorts_GetAllMethod, Sp_MultiCurrency_GetAllMethod, Sp_Bank_GetAllMethod,
 *                                                  Sp_ExImPackMaterilaType_GetAllMethod, Sp_Item_GetAllMethod, Sp_UOMSchedule_GetAllMethod,
 *                                                  Sp_InvCropYear_GetAllMethod, Sp_InvWareHouse_GetAllMethod, SP_JobLot_ReadMethod,
 *                                                  Sp_COAAllocation_GetAllMethod, Sp_Branches_GetAllMethod, Sp_Projects_GetAllMethod,
 *                                                  Sp_ConfigrationsAllocation_GetAllMethod, Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId
 */
@Repository
public class ImpARepository {

    private final HrmProcRepository db;

    public ImpARepository(HrmProcRepository db) { this.db = db; }

    private static Map<String, Object> p(Object... kv) {
        Map<String, Object> m = new LinkedHashMap<>();
        for (int i = 0; i + 1 < kv.length; i += 2) m.put(String.valueOf(kv[i]), kv[i + 1]);
        return m;
    }

    public <T> T tx(Supplier<T> work) { return db.tx(work); }

    public int set(String proc, DesktopModel m) { return db.set(proc, m); }

    // ================================================================== shared combos

    /** SupplierCustomer.ReadByOrganizationCompanyIdForExport (BLL 0600). */
    public List<Map<String, Object>> suppliersForExport(UserAccount u) {
        return db.rows("Sp_SupplierCustomer_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "Activity", "ReadByOrganizationCompanyIdForExport");
    }

    /** CommonServices.GetSupplierustomerByCustomerGroupId("9") -> SupplierCustomer.GetSupplierustomerByCustomerGroupId (ParentId unset). */
    public List<Map<String, Object>> suppliersByGroupIds(UserAccount u, String groupIds) {
        return db.rows("Sp_SupplierCustomer_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "CustomerGroupIds", groupIds, "Activity", "GetSupplierustomerByCustomerGroupId");
    }

    /** ExImDeliveryTerm.Getall(): @Activity 'ReadAll' only. */
    public List<Map<String, Object>> deliveryTerms() {
        return db.rows("Sp_ExImDeliveryTerm_GetAllMethod", "Activity", "ReadAll");
    }

    /** ExImLcPaymentTerm.Getall(obj). */
    public List<Map<String, Object>> lcPaymentTerms(UserAccount u) {
        return db.rows("Sp_ExImLcPaymentTerm_GetAllMethod", "Activity", "ReadAll", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
    }

    /** SeaPorts.Getall(obj) (BLL 0597). */
    public List<Map<String, Object>> seaPorts(UserAccount u) {
        return db.rows("Sp_SeaPorts_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "Activity", "ReadByCompanyNOrganizationId");
    }

    /** MultiCurrency.GetAll(obj) (BLL 0076). */
    public List<Map<String, Object>> currencies(UserAccount u) {
        return db.rows("Sp_MultiCurrency_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll");
    }

    /** Bank.GetAll(obj) (BLL 0057). */
    public List<Map<String, Object>> banks(UserAccount u) {
        return db.rows("Sp_Bank_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll");
    }

    /** ExImPackMaterilaType.Getall(obj) (BLL 0475). */
    public List<Map<String, Object>> packMaterialTypes(UserAccount u) {
        return db.rows("Sp_ExImPackMaterilaType_GetAllMethod", "Activity", "ReadAll", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
    }

    /** Item.ReadAllForExportCombo(obj) (BLL 0583). */
    public List<Map<String, Object>> itemsForExport(UserAccount u) {
        return db.rows("Sp_Item_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAllForExportCombo");
    }

    /** CommonServices.ItemGetForComboServiceBind -> Item.GetAllbyCombobind ('ReadAllForComboTwoColumns'). */
    public List<Map<String, Object>> itemsTwoColumns(UserAccount u) {
        return db.rows("Sp_Item_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAllForComboTwoColumns");
    }

    /** CommonServices.GetUomScheduleByItemId -> UOMSchedule.SearchByObject ('ReadByItemID'). */
    public List<Map<String, Object>> uomsByItem(UserAccount u, int itemId) {
        return db.rows("Sp_UOMSchedule_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "ItemId", itemId,
                "Activity", "ReadByItemID");
    }

    /** CommonServices.CropYearGetAllService -> InvCropYear.Getall ('ReadAll'). */
    public List<Map<String, Object>> cropYears(UserAccount u) {
        return db.rows("Sp_InvCropYear_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll");
    }

    /** CommonServices.getActiveWareHouse -> InvWareHouse.GetActiveWareHouse. */
    public List<Map<String, Object>> activeWarehouses(UserAccount u) {
        return db.rows("Sp_InvWareHouse_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "GetActiveWareHouse");
    }

    /** CommonServices.JobLotGetAllService -> jobLot.GetAll. */
    public List<Map<String, Object>> jobLots(UserAccount u) {
        return db.rows("SP_JobLot_ReadMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "GetAll");
    }

    /** CommonServices.CoaAllocationGetAllServiceBind -> COAAllocation.GetAll ('COAAllocationSearch', @UserId when set). */
    public List<Map<String, Object>> coaAllocations(UserAccount u) {
        Integer uid = u.getId();
        return db.rows("Sp_COAAllocation_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "UserId", uid != null && uid != 0 ? uid : null, "Activity", "COAAllocationSearch");
    }

    /** CommonServices.BrancheServiceBind -> Branches.GetAll. */
    public List<Map<String, Object>> branches(UserAccount u) {
        return db.rows("Sp_Branches_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "GetAll");
    }

    /** CommonServices.ProjectServiceBind -> Projects.GetAlldt. */
    public List<Map<String, Object>> projects(UserAccount u) {
        return db.rows("Sp_Projects_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "MethodType", "GetAll");
    }

    /** CommonServices.GetConfigurationByOrgCompandConfigDescription: rows[0].ConfigKey or "". */
    public String config(UserAccount u, String description) {
        List<Map<String, Object>> r = db.rows("Sp_ConfigrationsAllocation_GetAllMethod", "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(), "ConfigDescription", description, "Activity", "GetConfigurationByOrgCompandConfigDescription");
        if (r.isEmpty() || r.get(0).get("ConfigKey") == null) return "";
        return String.valueOf(r.get(0).get("ConfigKey"));
    }

    /** clsGlobalVariables.ActiveYr.Start_Period: the active year's row of Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId. */
    public Object financialYearStart(UserAccount u, int yearId) {
        for (Map<String, Object> r : db.rows("Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId",
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId())) {
            Object id = r.get("Id");
            if (id instanceof Number && ((Number) id).intValue() == yearId) return r.get("Start_Period");
        }
        return null;
    }

    // ================================================================== 525 ImLcOrder

    /** ImLcOrder.GenerateCode: rows[0].DocNo. */
    public List<Map<String, Object>> lcOrderGenerateCode(UserAccount u, int branchesId, int financialYearId, int documentTypeId) {
        return db.rows("[dbo].[USP_ImLcOrder_GetAllMethod]", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "BranchesId", branchesId, "FinancialYearId", financialYearId, "DocumentTypeId", documentTypeId, "Activity", "GenerateDocNo");
    }

    /** ImLcOrder.ReadById -> DAL GetDate: header ('ReadById'). */
    public List<Map<String, Object>> lcOrderById(int id) {
        return db.rows("[dbo].[USP_ImLcOrder_GetAllMethod]", "Id", id, "Activity", "ReadById");
    }

    /** DAL GetDate detail: 'ReadByLcOrderHeaderId' for DocumentTypeId 231, else 'ReadByLcOrderHeaderIdForPurchaseContractExport'. */
    public List<Map<String, Object>> lcOrderPackingDetail(int id, int documentTypeId) {
        return db.rows("USP_ImLcOrder_GetAllMethod", "Id", id,
                "Activity", documentTypeId == 231 ? "ReadByLcOrderHeaderId" : "ReadByLcOrderHeaderIdForPurchaseContractExport");
    }

    /** DAL GetDate payment terms: 'ReadImLcOrderPaymnetTerm_ByLcOrderHeaderId'. */
    public List<Map<String, Object>> lcOrderPaymentTerms(int id) {
        return db.rows("USP_ImLcOrder_GetAllMethod", "Id", id, "Activity", "ReadImLcOrderPaymnetTerm_ByLcOrderHeaderId");
    }

    /** ImLcOrder.FormHistory(ReportsParameters): @FinancialYearId / @NoOfRecords only when != 0, @EntryUserId only when !CanViewAllRecord. */
    public List<Map<String, Object>> lcOrderHistory(UserAccount u, int branchesId, int financialYearId, int noOfRecords, boolean canViewAll) {
        Map<String, Object> m = p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "BranchesId", branchesId,
                "DocumentTypeId", 231);
        if (financialYearId != 0) m.put("FinancialYearId", financialYearId);
        if (noOfRecords != 0) m.put("NoOfRecords", noOfRecords);
        m.put("CanViewAllRecord", canViewAll);
        if (!canViewAll) m.put("EntryUserId", u.getId());
        m.put("Activity", "FormHistory");
        return db.rows("[dbo].[USP_ImLcOrder_GetAllMethod]", m);
    }

    /** ImLcOrder.FormHistoryDetail(Id). */
    public List<Map<String, Object>> lcOrderHistoryDetail(int id) {
        return db.rows("[dbo].[USP_ImLcOrder_GetAllMethod]", "Id", id, "Activity", "FormHistoryDetail");
    }

    /** ImLcOrder.DeleteByID(EntryUserId, Id): GetDataTableProc (the RAISERRORs surface as exceptions). */
    public void lcOrderDelete(int entryUserId, int id) {
        db.rows("[dbo].[USP_ImLcOrder_GetAllMethod]", "EntryUserId", entryUserId, "Id", id, "Activity", "DeleteById");
    }

    /** ImLcOrder.ImLcOrder_SlipAndRegister (231 slip): the guarded filters are never set by this caller except @LcOrderId. */
    public List<Map<String, Object>> lcOrderSlip(UserAccount u, int branchesId, int financialYearId, int id) {
        Map<String, Object> m = p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "BranchesId", branchesId,
                "FinancialYearId", financialYearId);
        if (id != 0) m.put("LcOrderId", id);
        return db.rows("[dbo].[USP_ImLcOrder_Register]", m);
    }

    /** ImLcOrder.GetImportLcOrderItems(Org, Comp, Id) (526 item combo). */
    public List<Map<String, Object>> lcOrderItems(UserAccount u, int id) {
        return db.rows("[dbo].[USP_ImLcOrder_GetAllMethod]", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Id", id,
                "Activity", "GetImportLcOrderItems");
    }

    /** ImLcOrder.GetLcOrderNo(obj) (221 contract combo; DocumentTypeId 231). */
    public List<Map<String, Object>> lcOrderNos(UserAccount u, int documentTypeId) {
        return db.rows("[dbo].[USP_ImLcOrder_GetAllMethod]", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocumentTypeId", documentTypeId, "Activity", "GetLcOrderNo");
    }

    // ================================================================== 526 ImLcOrderSchedule

    /** ImLcOrderScheduleHeader.GridContactsExistInScheduleOrNot(Org, Comp, ActionId, NoOfRecords) - @NoOfRecords only when != 0. */
    public List<Map<String, Object>> scheduleContracts(UserAccount u, int actionId, int noOfRecords) {
        Map<String, Object> m = p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "ActionId", actionId);
        if (noOfRecords != 0) m.put("NoOfRecords", noOfRecords);
        m.put("Activity", "GridContactsExistInScheduleOrNot");
        return db.rows("USP_ImLcOrderSchedule_GetAllMethod", m);
    }

    /** ImLcOrderScheduleHeader.GetScheduleByContractId(Id) ('ReadById'). */
    public List<Map<String, Object>> scheduleByContract(int contractId) {
        return db.rows("USP_ImLcOrderSchedule_GetAllMethod", "Id", contractId, "Activity", "ReadById");
    }

    /** ImLcOrderScheduleHeader.GetPackingDetailByContractId(Id) ('ReadDetailByLcorderId'). */
    public List<Map<String, Object>> schedulePackingByContract(int contractId) {
        return db.rows("USP_ImLcOrderSchedule_GetAllMethod", "Id", contractId, "Activity", "ReadDetailByLcorderId");
    }

    /** ImLcOrderScheduleHeader.GetAttentiveLoadDateByLcOrderId(Id). */
    public List<Map<String, Object>> attentiveLoadDates(int contractId) {
        return db.rows("USP_ImLcOrderSchedule_GetAllMethod", "Id", contractId, "Activity", "GetAttentiveLoadDateByLcOrderId");
    }

    /** ImLcOrderScheduleHeader.GetNoOfContainersByScheduleAndLcOrderId(ContractId, ScheduleId). */
    public List<Map<String, Object>> scheduleWeight(int contractId, int scheduleId) {
        return db.rows("USP_ImLcOrderSchedule_GetAllMethod", "Id", contractId, "ScheduleId", scheduleId, "Activity", "GetNoOfContainersByScheduleAndLcOrderId");
    }

    /** ImLcOrderScheduleHeader.ContractSchedule_FormHistory: dates / ids / @NoOfRecords only when set. */
    public List<Map<String, Object>> scheduleHistory(UserAccount u, Timestamp from, Timestamp to, int supplierId, int portId, int itemId, int noOfRecords) {
        Map<String, Object> m = p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (from != null) m.put("FromDate", from);
        if (to != null) m.put("ToDate", to);
        if (supplierId != 0) m.put("SupplierCustomerId", supplierId);
        if (portId != 0) m.put("DestinationPortId", portId);
        if (itemId != 0) m.put("ItemId", itemId);
        if (noOfRecords != 0) m.put("NoOfRecords", noOfRecords);
        return db.rows("[USP_ImLcOrderSchedule_FormHistory]", m);
    }

    /** ImLcOrderScheduleHeader.GetDataForDropDownFromShipmentSchedule(Org, Comp, null) - @Activity not sent. */
    public List<Map<String, Object>> scheduleHistoryCombos(UserAccount u) {
        return db.rows("USP_GetDataForDropDownFromImportLcOrderSchedule", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
    }

    // ================================================================== 221 ImCommercialInvoice

    /** ExImLcOrderPurchaseOrder.GetPurchaseOrderNo(obj) (DocumentTypeId 232). */
    public List<Map<String, Object>> purchaseOrderNos(UserAccount u, int documentTypeId) {
        return db.rows("Sp_ExImLcOrderPurchaseOrder_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocumentTypeId", documentTypeId, "Activity", "GetPurchaseOrderNo");
    }

    /** ExImLcOrderPurchaseOrder.GetImportPurchaseOrderDetailById(obj). */
    public List<Map<String, Object>> purchaseOrderHeader(UserAccount u, int id, int documentTypeId) {
        return db.rows("Sp_ExImLcOrderPurchaseOrder_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "Id", id, "DocumentTypeId", documentTypeId, "Activity", "GetImportPurchaseOrderDetailById");
    }

    /** ExImLcOrderPurchaseOrder.GETIMPORTCONTRACTDETAILBYCONTRACTID (237) / GETIMPORTGRNDETAILBYPURCHASEORDERID (233). */
    public List<Map<String, Object>> purchaseOrderDetail(UserAccount u, int id, boolean contract) {
        return db.rows("Sp_ExImLcOrderPurchaseOrder_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "Id", id, "Activity", contract ? "GETIMPORTCONTRACTDETAILBYCONTRACTID" : "GETIMPORTGRNDETAILBYPURCHASEORDERID");
    }

    /** ImportReports.ImInvoiceSlip_806: @Id only when != 0; ApprovedFilter "All" -> @IsApproved not sent. */
    public List<Map<String, Object>> invoiceSlip(UserAccount u, int id) {
        Map<String, Object> m = p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (id != 0) m.put("Id", id);
        return db.rows("Sp_ImInvoice_SlipandRegister", m);
    }

    // ================================================================== 392-395 registers

    /** ImportReports.ImInvoiceRegister_WithAvgRates. */
    public List<Map<String, Object>> invoiceRegister(UserAccount u, Timestamp from, Timestamp to, int supplierId, int itemId) {
        Map<String, Object> m = p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "FromDate", from, "ToDate", to);
        if (supplierId != 0) m.put("SupplierCustomerId", supplierId);
        if (itemId != 0) m.put("ItemId", itemId);
        return db.rows("SP_ImInvoiceRegister_WithAvgRates", m);
    }

    /**
     * ImportReports.ImportContractSlipAndRegister. The BLL also sends @IsApproved (ApprovedFilter is null, so != "All"), a parameter
     * Sp_ImLcOrderNo_ImportSlip_Rpt does not declare - the desktop call fails with "too many arguments"; it is left out here.
     */
    public List<Map<String, Object>> contractRegister(UserAccount u, Timestamp from, Timestamp to, int supplierId, int itemId) {
        Map<String, Object> m = p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "FromDate", from, "ToDate", to);
        if (supplierId != 0) m.put("SupplierCustomerId", supplierId);
        if (itemId != 0) m.put("ItemId", itemId);
        return db.rows("Sp_ImLcOrderNo_ImportSlip_Rpt", m);
    }

    /** ImportReports.ImportGrnSlipAndRegister (DocumentTypeId 234). */
    public List<Map<String, Object>> grnRegister(UserAccount u, Timestamp from, Timestamp to, int supplierId, int itemId) {
        Map<String, Object> m = p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "DocumentTypeId", 234,
                "FromDate", from, "ToDate", to);
        if (supplierId != 0) m.put("SupplierCustomerId", supplierId);
        if (itemId != 0) m.put("ItemId", itemId);
        return db.rows("Sp_ImGRN_SlipAndRegister", m);
    }

    /** ImportReports.ImportPurchaseOrderSlip (DocumentTypeId 236; ApprovedFilter null -> @IsApproved = false is sent). */
    public List<Map<String, Object>> purchaseRegister(UserAccount u, Timestamp from, Timestamp to, int supplierId, int itemId) {
        Map<String, Object> m = p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "DocumentTypeId", 236,
                "FromDate", from, "ToDate", to);
        if (supplierId != 0) m.put("SupplierCustomerId", supplierId);
        if (itemId != 0) m.put("ItemId", itemId);
        m.put("IsApproved", Boolean.FALSE);
        return db.rows("Sp_ImportPurchaseOrder_Slip_rpt", m);
    }
}
