package com.mst.repositories.partyprocessing;

import com.mst.models.UserAccount;
import com.mst.models.hrm.DesktopModel;
import com.mst.repositories.hrm.HrmProcRepository;
import org.springframework.stereotype.Repository;

import java.sql.Timestamp;
import java.time.LocalDateTime;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.function.Supplier;

import static com.mst.services.hrm.HrmSupport.toInt;
import static com.mst.services.hrm.HrmSupport.str;

/**
 * DAL of the Party Processing "PpC" screens: every call is the desktop BLL/DAL's own procedure with the
 * parameters it sends (names and "only when" guards traced from architecture.bll / architecture.dal, each
 * procedure and parameter checked in procdure_index.csv). No table, column or procedure is created.
 *
 * Shared lookups (CommonServices wrappers) are at the top; then one section per screen:
 *   676 Job Order            BLL 0304 / DAL 0307   Sp_InvProductionJobOrderPartyProcessing_*
 *   677 Production           BLL 0299 / DAL 0302   Sp_InvFoodProductionPartyProcessing_*  (+ PM BLL 0300 / DAL 0303)
 *   675 Processing Bill      BLL 0302 / DAL 0305   USP_InvProductionProcessingBill_*
 *   602 Stock Conversion     BLL 0297 / DAL 0300   USP_InvStockConversionPartyProcessing_*
 *   603 Stock Transfer       BLL 0295 / DAL 0298   Sp_InvStockTransferHeaderPartyProcessing_*
 *   604 Stock Adjustment     BLL 0296 / DAL 0299   usp_InvStockAdjustmentPartyProcessing_*
 *   614 Wages Bill           BLL 0485 / DAL 0536   Sp_InvContractorWagesBillHeader_*
 * A null value is not sent (ADO.NET AddWithValue(null)). DesktopProc prefixes "dbo." unless the name starts with '['.
 */
@Repository
public class PpCRepository {

    private final HrmProcRepository db;

    public PpCRepository(HrmProcRepository db) { this.db = db; }

    public <T> T tx(Supplier<T> work) { return db.tx(work); }

    public static Map<String, Object> p(Object... kv) {
        Map<String, Object> m = new LinkedHashMap<>();
        for (int i = 0; i + 1 < kv.length; i += 2) m.put(String.valueOf(kv[i]), kv[i + 1]);
        return m;
    }

    private static int org(UserAccount u) { return toInt(u.getOrganizationId()); }
    private static int co(UserAccount u) { return toInt(u.getCompanyId()); }
    public static int branch(UserAccount u) { return toInt(u.getBranchesId()); }
    private static Timestamp ts(LocalDateTime d) { return d == null ? null : Timestamp.valueOf(d); }

    public List<Map<String, Object>> rows(String proc, Map<String, Object> p) { return db.rows(proc, p); }
    public int set(String proc, DesktopModel m) { return db.set(proc, m); }
    /** SqlCommand.ExecuteNonQuery / ExecuteScalar with the result discarded (a RAISERROR still surfaces). */
    public void exec(String proc, Map<String, Object> p) { db.scalar(proc, p); }

    // =========================================================================== shared lookups

    /** CommonServices.GetConfigurationByOrgCompandConfigDescription: Sp_ConfigrationsAllocation_GetAllMethod ConfigKey of row 0, else "". */
    public String config(UserAccount u, String description) {
        List<Map<String, Object>> r = db.rows("Sp_ConfigrationsAllocation_GetAllMethod", p("OrganizationId", org(u), "CompanyId", co(u),
                "ConfigDescription", description, "Activity", "GetConfigurationByOrgCompandConfigDescription"));
        return r.isEmpty() ? "" : str(r.get(0).get("ConfigKey")).trim();
    }

    /** clsGlobalVariables.WagesRefDocumentsStatusList - USP_GetRefDocumentsForWages (BLL 0485 GetRefDocumentsForWages, @RefDocumentTypeId omitted when 0). */
    public List<Map<String, Object>> wagesRefDocuments() { return db.rows("[dbo].[USP_GetRefDocumentsForWages]", p()); }

    /** CommonServices.GetSupplierustomerForPartyProcessing -> BLL 0600 -> Sp_SupplierCustomer_GetAllMethod 'GetSupplierustomerForPartyProcessing'. */
    public List<Map<String, Object>> stockParties(UserAccount u) {
        return db.rows("Sp_SupplierCustomer_GetAllMethod", p("OrganizationId", org(u), "CompanyId", co(u), "Activity", "GetSupplierustomerForPartyProcessing"));
    }

    /** CommonServices.WareHouseGetAllService -> BLL 0582 InvWareHouse.Getall -> Sp_InvWareHouse_GetAllMethod 'ReadByOrganizationCompanyId'. */
    public List<Map<String, Object>> warehouses(UserAccount u) {
        return db.rows("Sp_InvWareHouse_GetAllMethod", p("OrganizationId", org(u), "CompanyId", co(u), "Activity", "ReadByOrganizationCompanyId"));
    }

    /** CommonServices.getActiveWareHouse -> BLL 0582 GetActiveWareHouse. */
    public List<Map<String, Object>> activeWarehouses(UserAccount u) {
        return db.rows("Sp_InvWareHouse_GetAllMethod", p("OrganizationId", org(u), "CompanyId", co(u), "Activity", "GetActiveWareHouse"));
    }

    /** CommonServices.GetActiveWareHouseByWareHouseType(type) -> Sp_InvWareHouse_GetAllMethod @WarehouseType. */
    public List<Map<String, Object>> warehousesByType(UserAccount u, int type) {
        return db.rows("Sp_InvWareHouse_GetAllMethod", p("OrganizationId", org(u), "CompanyId", co(u), "WarehouseType", type, "Activity", "GetActiveWareHouseByWareHouseType"));
    }

    /** BLL 0583 Item.ReadAllForItemsForPartyProcessing(org, comp, ParentCategoryIds): @ParentIds only when non-empty. */
    public List<Map<String, Object>> itemsForPartyProcessing(UserAccount u, String parentIds) {
        Map<String, Object> m = p("OrganizationId", org(u), "CompanyId", co(u));
        if (parentIds != null && !parentIds.isEmpty()) m.put("ParentIds", parentIds);
        m.put("Activity", "ReadAllForItemsForPartyProcessing");
        return db.rows("Sp_Item_GetAllMethod", m);
    }

    /** BLL 0583 Item.ReadAllForItemsForPartyProcessingPM. */
    public List<Map<String, Object>> itemsForPartyProcessingPm(UserAccount u) {
        return db.rows("Sp_Item_GetAllMethod", p("OrganizationId", org(u), "CompanyId", co(u), "Activity", "ReadAllForItemsForPartyProcessingPM"));
    }

    /** CommonServices.CropYearGetAllService -> BLL 0571 InvCropYear.Getall 'ReadAll'. */
    public List<Map<String, Object>> cropYears(UserAccount u) {
        return db.rows("Sp_InvCropYear_GetAllMethod", p("OrganizationId", org(u), "CompanyId", co(u), "Activity", "ReadAll"));
    }

    /** BLL jobLot.AllJobLotForPartyProcessing -> SP_JobLot_ReadMethod 'AllJobLotForPartyProcessing'. */
    public List<Map<String, Object>> jobLots(UserAccount u) {
        return db.rows("SP_JobLot_ReadMethod", p("OrganizationId", org(u), "CompanyId", co(u), "Activity", "AllJobLotForPartyProcessing"));
    }

    /** BLL 0579 InvPackingType.Getall - @Activity only. */
    public List<Map<String, Object>> packingTypes() { return db.rows("Sp_InvPackingType_GetAllMethod", p("Activity", "ReadAll")); }

    /** CommonServices.GetUomScheduleByItemId -> UOMSchedule.SearchByObject -> Sp_UOMSchedule_GetAllMethod 'ReadByItemID'. */
    public List<Map<String, Object>> uomSchedule(UserAccount u, int itemId) {
        return db.rows("Sp_UOMSchedule_GetAllMethod", p("OrganizationId", org(u), "CompanyId", co(u), "ItemId", itemId, "Activity", "ReadByItemID"));
    }

    /** CommonServices.StaticColumnsService(activity) -> SpStaticColumnNames. */
    public List<Map<String, Object>> staticColumns(String activity) { return db.rows("SpStaticColumnNames", p("Activity", activity)); }

    /** CommonServices.ReferencePartyServiceBind -> [Sp_ReferenceParties_GetAllMethod] 'ReadAll'. */
    public List<Map<String, Object>> referenceParties(UserAccount u) {
        return db.rows("Sp_ReferenceParties_GetAllMethod", p("OrganizationId", org(u), "CompanyId", co(u), "Activity", "ReadAll"));
    }

    /** CommonServices.MultiLanguagesGetAll -> Sp_MultiLanguages_GetAll @MethodType='ReadAll'. */
    public List<Map<String, Object>> languages(UserAccount u) {
        return db.rows("Sp_MultiLanguages_GetAll", p("MethodType", "ReadAll", "OrganizationId", org(u), "CompanyId", co(u)));
    }

    /** InvProductionPlant.GetAll -> Sp_InvProductionPlant_GetAllMethod 'GetALL'. */
    public List<Map<String, Object>> plants(UserAccount u) {
        return db.rows("Sp_InvProductionPlant_GetAllMethod", p("OrganizationId", org(u), "CompanyId", co(u), "Activity", "GetALL"));
    }

    /** BLL 0327 ProductionType.GetAll(new ProductionType()) - no parameter (Id 0). */
    public List<Map<String, Object>> productionTypes() { return db.rows("Sp_ProductionType_GetAllMethod", p()); }

    /** CommonServices.CoaAllocationAccountTitleByAccountTypeIds(ids) -> BLL 0648 GetAccountTitleByAccountTypeIds. */
    public List<Map<String, Object>> accountsByTypeIds(UserAccount u, String accountTypeIds) {
        Map<String, Object> m = p("OrganizationId", org(u), "CompanyId", co(u), "AppId", u.getAppId() == null ? 0 : u.getAppId());
        if (accountTypeIds != null && !accountTypeIds.isEmpty()) m.put("AccountTypeIds", accountTypeIds);
        if (u.getId() != null && u.getId() != 0) m.put("UserId", u.getId());
        m.put("Activity", "GetAccountTitleByAccountTypeIds");
        return db.rows("Sp_COAAllocation_GetAllMethod", m);
    }

    /** BLL UOM.UOMStaticAll -> [dbo].[USP_UOMStatic_GetAllMethod] (no parameter). */
    public List<Map<String, Object>> uomStatic() { return db.rows("[dbo].[USP_UOMStatic_GetAllMethod]", p()); }

    /** BLL 0056 GetBalQtyAndWeightForPartyProcessing: @ItemId, @StockPartyId, @WarehouseId always. */
    public List<Map<String, Object>> balQtyAndWeight(UserAccount u, int itemId, int stockPartyId, int warehouseId) {
        return db.rows("Sp_GetAvgRatesAndStockInHand_GetAllMethod", p("OrganizationId", org(u), "CompanyId", co(u), "ItemId", itemId,
                "StockPartyId", stockPartyId, "WarehouseId", warehouseId, "Activity", "GetBalQtyAndWeightForPartyProcessing"));
    }

    /** BLL 0056 GetStockInHandFromInventoryTrasactions - Rows[0][0], 0 when no row. */
    public double stockInHand(UserAccount u, int itemId, LocalDateTime docDate, int jobLotId, int warehouseId, String cropYear) {
        Map<String, Object> m = p("OrganizationId", org(u), "CompanyId", co(u), "ItemId", itemId, "DocDate", ts(docDate));
        if (warehouseId != 0) m.put("WarehouseId", warehouseId);
        if (jobLotId != 0) m.put("JobLotId", jobLotId);
        if (cropYear != null && !cropYear.isEmpty()) m.put("CropYear", cropYear);
        m.put("Activity", "GetStockInHandFromInventoryTrasactions");
        List<Map<String, Object>> r = db.rows("Sp_GetAvgRatesAndStockInHand_GetAllMethod", m);
        if (r.isEmpty()) return 0d;
        Object first = r.get(0).values().isEmpty() ? null : r.get(0).values().iterator().next();
        return com.mst.services.hrm.HrmSupport.toDouble(first);
    }

    /** CommonServices.BrancheServiceBind -> Branches.GetAll: Sp_Branches_GetAllMethod @Activity='GetAll'. */
    public List<Map<String, Object>> branches(UserAccount u) {
        return db.rows("Sp_Branches_GetAllMethod", p("OrganizationId", org(u), "CompanyId", co(u), "Activity", "GetAll"));
    }

    /** CommonServices.ProjectServiceBind -> Projects.GetAlldt -> Sp_Projects_GetAllMethod @MethodType='GetAll'. */
    public List<Map<String, Object>> projects(UserAccount u) {
        return db.rows("Sp_Projects_GetAllMethod", p("OrganizationId", org(u), "CompanyId", co(u), "MethodType", "GetAll"));
    }

    /** CommonServices.GetWeightCurrStockByItem -> InvSaleInvoice.GetWeightCurrStockByItem. */
    public List<Map<String, Object>> weightCurrStockByItem(UserAccount u, int itemId, LocalDateTime toDate, int warehouseId, int jobLotId, String cropYear) {
        Map<String, Object> m = p("OrganizationId", org(u), "CompanyId", co(u), "ItemId", itemId, "ToDate", ts(toDate));
        if (warehouseId != 0) m.put("WarehouseId", warehouseId);
        if (jobLotId != 0) m.put("JobLotId", jobLotId);
        if (cropYear != null && !cropYear.isEmpty()) m.put("CropYear", cropYear);
        m.put("Activity", "GetWeightCurrStockByItem");
        return db.rows("[Sp_InvSaleInvoice_GetAllMethod]", m);
    }

    /** CommonServices.VoucherHeadIdGet(Id, DocumentTypeId) -> Sp_Vouchers_GetMethods: rows[0]["Id"] or 0. */
    public int voucherHeadId(UserAccount u, int documentTypeId, int id) {
        List<Map<String, Object>> r = db.rows("Sp_Vouchers_GetMethods", p("Activity", "GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId",
                "OrganizationId", org(u), "CompanyId", co(u), "DocumentTypeId", documentTypeId, "DocumentTypeSrNo", id));
        return r.isEmpty() ? 0 : toInt(r.get(0).get("Id"));
    }

    /** CommonServies.GetSupplierCustomerListForFinancialEffects. */
    public List<Map<String, Object>> supplierCustomerGl(UserAccount u) {
        return db.rows("Sp_SupplierCustomer_GetAllMethod", p("OrganizationId", org(u), "CompanyId", co(u), "Activity", "GetGlAccountIdandCompanyNameBySupplierCustomerId"));
    }

    /** CommonServies.GetItemListForFinancialEffects / CommonServices.GetItemGlIdsandItemName (same call). */
    public List<Map<String, Object>> itemGl(UserAccount u) {
        return db.rows("Sp_Item_GetAllMethod", p("OrganizationId", org(u), "CompanyId", co(u), "Activity", "GetItemGlIdsandItemName"));
    }

    /** CommonServies.GetPartyProcessingItemListForFinancialEffects. */
    public List<Map<String, Object>> partyProcessingItemGl(UserAccount u) {
        return db.rows("Sp_Item_GetAllMethod", p("OrganizationId", org(u), "CompanyId", co(u), "Activity", "GetPartyProcessingItemGlIdsandItemName"));
    }

    /** DAL InvConractorWagesAccounts.GetData 'ReadAll'. */
    public List<Map<String, Object>> wagesAccountsAll(UserAccount u) {
        return db.rows("Sp_InvConractorWagesAccounts_GetAllMethod", p("OrganizationId", org(u), "CompanyId", co(u), "Activity", "ReadAll"));
    }

    // =========================================================================== issuance loader (LoadavailableTransactionsForIssuancePartyProcessing)

    /** ComboFill: StocksReport.InventoryTransactionsPartyProcessing_DropDownAndList - @ActivityType not set by the loader. */
    public List<Map<String, Object>> loaderCombos(UserAccount u) {
        return db.rows("USP_InventoryTransactionsPartyProcessing_DropDownAndLists", p("OrganizationId", org(u), "CompanyId", co(u)));
    }

    /** PendingInventoryTransactionsForIssuanceLoad: InventoryStockEvalautionDetail.GetAvailableTransactionsForIssuanceForPartyProcessing. */
    public List<Map<String, Object>> availableForIssuance(UserAccount u, LocalDateTime from, LocalDateTime to, int stockPartyId, int refPartyId,
                                                          int refDocumentTypeId, int refWarehouseId, int warehouseId, int jobLotId, int itemId,
                                                          int packingTypeId, int cropYearId) {
        Map<String, Object> m = p("OrganizationId", org(u), "CompanyId", co(u), "FromDate", ts(from), "ToDate", ts(to));
        if (stockPartyId != 0) m.put("StockPartyId", stockPartyId);
        if (refPartyId != 0) m.put("ReferencePartyId", refPartyId);
        if (refDocumentTypeId != 0) m.put("RefDocumentTypeId", refDocumentTypeId);
        if (refWarehouseId != 0) m.put("RefWarehouseId", refWarehouseId);
        if (warehouseId != 0) m.put("WarehouseId", warehouseId);
        if (jobLotId != 0) m.put("JobLotId", jobLotId);
        if (itemId != 0) m.put("ItemId", itemId);
        if (packingTypeId != 0) m.put("PackingTypeId", packingTypeId);
        if (cropYearId != 0) m.put("CropYearId", cropYearId);
        return db.rows("SpInventoryTransactionsPartyProcessing_GetAvailableTransactionsForIssuance", m);
    }

    // =========================================================================== inventory posting (party processing)

    /** DAL CommonServices.FIFOImplementionForPartyProcessing: USP_GetStockByFifoMethodPartyProcessing. */
    public List<Map<String, Object>> stockByFifo(UserAccount u, int itemId, int stockPartyId, LocalDateTime docDate, int packUomId, int warehouseId,
                                                 int jobLotId, int packingTypeId, int cropYearId, int documentTypeId, int id, String fifoXml) {
        Map<String, Object> m = p("OrganizationId", org(u), "CompanyId", co(u), "ItemId", itemId, "StockPartyId", stockPartyId, "DocDate", ts(docDate));
        if (packUomId != 0) m.put("PackUomId", packUomId);
        if (warehouseId != 0) m.put("WarehouseId", warehouseId);
        if (jobLotId != 0) m.put("JobLotId", jobLotId);
        if (packingTypeId != 0) m.put("PackingTypeId", packingTypeId);
        if (cropYearId != 0) m.put("CropYearId", cropYearId);
        if (documentTypeId != 0) m.put("DocumentTypeId", documentTypeId);
        if (id != 0) m.put("Id", id);
        if (fifoXml != null) m.put("FIFOXML", fifoXml);
        return db.rows("USP_GetStockByFifoMethodPartyProcessing", m);
    }

    /** [dbo].[USP_InventoryPartyProcessingQtyReverseAndDeleteByReferenceId] (SqlCommand, ExecuteNonQuery). */
    public void reverseAndDelete(UserAccount u, int refDocumentTypeId, int refDocIdNo) {
        exec("[dbo].[USP_InventoryPartyProcessingQtyReverseAndDeleteByReferenceId]", p("OrganizationId", org(u), "CompanyId", co(u),
                "RefDocumentTypeId", refDocumentTypeId, "RefDocIdNo", refDocIdNo));
    }

    /** USP_InventoryValidationPartyProcessing (SqlCommand, ExecuteNonQuery) - the caller builds the exact AddWithValue list. */
    public void inventoryValidation(Map<String, Object> params) { exec("USP_InventoryValidationPartyProcessing", params); }

    // =========================================================================== 676 production job order (BLL 0304)

    public int jobOrderCode(UserAccount u, int financialYearId) {
        List<Map<String, Object>> r = db.rows("Sp_InvProductionJobOrderPartyProcessing_GetAllMethod", p("OrganizationId", org(u), "CompanyId", co(u),
                "BranchesId", branch(u), "FinancialYearId", financialYearId, "DocumentTypeId", 120, "Activity", "GenerateCode"));
        return r.isEmpty() ? 0 : toInt(r.get(0).get("DocNo"));
    }

    /** ReadById: @Id, @Activity (header only; the input/output lists are never used by this form). */
    public List<Map<String, Object>> jobOrder(int id) {
        return db.rows("Sp_InvProductionJobOrderPartyProcessing_GetAllMethod", p("Id", id, "Activity", "ReadById"));
    }

    /** DeleteByID(EntryUserId, Id) via GetDataTableProc. */
    public void jobOrderDelete(int entryUserId, int id) {
        db.rows("Sp_InvProductionJobOrderPartyProcessing_GetAllMethod", p("EntryUserId", entryUserId, "Id", id, "Activity", "DeleteById"));
    }

    /** FormHistory(ReportsParameters) - the caller passes the already-guarded filter map. */
    public List<Map<String, Object>> jobOrderHistory(Map<String, Object> m) { return db.rows("Sp_InvProductionJobOrderPartyProcessing_GetAllMethod", m); }

    /** GetDataForDropDownFromProductionJobOrderPartyProcessing: @Activity / @DocumentTypeIds never set by the form. */
    public List<Map<String, Object>> jobOrderDropDown(UserAccount u) {
        return db.rows("USP_GetDataForDropDownFromProductionJobOrderPartyProcessing", p("OrganizationId", org(u), "CompanyId", co(u)));
    }

    /** GetJobOrderNoForInvFoodProduction (DocumentTypeId 120). */
    public List<Map<String, Object>> jobOrdersForProduction(UserAccount u, int financialYearId) {
        return db.rows("Sp_InvProductionJobOrderPartyProcessing_GetAllMethod", p("OrganizationId", org(u), "CompanyId", co(u), "BranchesId", branch(u),
                "FinancialYearId", financialYearId, "DocumentTypeId", 120, "Activity", "GetJobOrderNoForInvFoodProduction"));
    }

    /** GetJobOrderNoForConversion: @StockPartyId only when non-zero. */
    public List<Map<String, Object>> jobOrdersForConversion(UserAccount u, int financialYearId, int stockPartyId) {
        Map<String, Object> m = p("OrganizationId", org(u), "CompanyId", co(u), "BranchesId", branch(u), "FinancialYearId", financialYearId, "DocumentTypeId", 120);
        if (stockPartyId != 0) m.put("StockPartyId", stockPartyId);
        m.put("Activity", "GetJobOrderNoForConversion");
        return db.rows("Sp_InvProductionJobOrderPartyProcessing_GetAllMethod", m);
    }

    /** GetGlAccountsByJobOrderId (WipItemId / ItemName / StockPartyId / StockParty ...). */
    public List<Map<String, Object>> jobOrderGl(UserAccount u, int financialYearId, int jobOrderId) {
        return db.rows("Sp_InvProductionJobOrderPartyProcessing_GetAllMethod", p("OrganizationId", org(u), "CompanyId", co(u), "BranchesId", branch(u),
                "FinancialYearId", financialYearId, "Id", jobOrderId, "Activity", "GetGlAccountsByJobOrderId"));
    }

    // =========================================================================== 677 production (BLL 0299 / 0300)

    public int productionCode(UserAccount u, int financialYearId, int documentTypeId) {
        List<Map<String, Object>> r = db.rows("Sp_InvFoodProductionPartyProcessing_GetAllMathod", p("OrganizationId", org(u), "CompanyId", co(u),
                "FinancialYearId", financialYearId, "DocumentTypeId", documentTypeId, "Activity", "GenerateCode"));
        return r.isEmpty() ? 0 : toInt(r.get(0).get("DocNo"));
    }

    public List<Map<String, Object>> production(int id) {
        return db.rows("Sp_InvFoodProductionPartyProcessing_GetAllMathod", p("Id", id, "Activity", "ReadById"));
    }

    public List<Map<String, Object>> productionDetails(int id) {
        return db.rows("Sp_InvFoodProductionPartyProcessing_GetAllMathod", p("Id", id, "Activity", "ReadDetailByHeaderId"));
    }

    /** DAL DeleteById: @Id, @EntryUser, @Activity (ExecuteScalar in its own transaction). */
    public void productionDelete(int id, int entryUserId) {
        exec("Sp_InvFoodProductionPartyProcessing_GetAllMathod", p("Id", id, "EntryUser", entryUserId, "Activity", "DeleteById"));
    }

    public List<Map<String, Object>> productionHistory(Map<String, Object> m) { return db.rows("USP_InvFoodProductionPartyProcessing_FormHistory", m); }

    /** GetDataForDropDownFromInvFoodProductionPartyProcessing: @DocumentTypeIds only when non-empty. */
    public List<Map<String, Object>> productionDropDown(UserAccount u, String documentTypeIds) {
        Map<String, Object> m = p("OrganizationId", org(u), "CompanyId", co(u));
        if (documentTypeIds != null && !documentTypeIds.isEmpty()) m.put("DocumentTypeIds", documentTypeIds);
        return db.rows("USP_GetDataForDropDownFromInvFoodProductionPartyProcessing", m);
    }

    public List<Map<String, Object>> inputTotalsByJobOrder(UserAccount u, int jobOrderId) {
        return db.rows("Sp_InvFoodProductionPartyProcessing_GetAllMathod", p("OrganizationId", org(u), "CompanyId", co(u), "InvJobOrderId", jobOrderId,
                "Activity", "GetInPutTotalQtyandWeightByJobOrderId"));
    }

    public int pmCode(UserAccount u, int financialYearId) {
        List<Map<String, Object>> r = db.rows("Sp_InvFoodProductionPartyProcessingPackingMaterial_GetAllMethod", p("OrganizationId", org(u), "CompanyId", co(u),
                "DocumentTypeId", 119, "FinancialYearId", financialYearId, "Activity", "GenerateCode"));
        return r.isEmpty() ? 0 : toInt(r.get(0).get("DocNo"));
    }

    public List<Map<String, Object>> pm(int id) {
        return db.rows("Sp_InvFoodProductionPartyProcessingPackingMaterial_GetAllMethod", p("Id", id, "Activity", "ReadById"));
    }

    public void pmDelete(int id, int entryUserId) {
        exec("Sp_InvFoodProductionPartyProcessingPackingMaterial_GetAllMethod", p("Id", id, "EntryUser", entryUserId, "Activity", "DeleteById"));
    }

    public List<Map<String, Object>> pmHistory(Map<String, Object> m) { return db.rows("USP_InvFoodProductionPartyProcessingPackingMaterial_FormHistory", m); }

    public List<Map<String, Object>> pmDropDown(UserAccount u) {
        return db.rows("[dbo].[USP_GetDataForDropDownFromInvFoodProductionPartyProcessingPackingMaterial]", p("OrganizationId", org(u), "CompanyId", co(u)));
    }

    /** FoodProductionPartyProcessingPackingMaterial_SlipAndRegister (the 601_02 print's rows): @DocumentTypeId always (0 from GenerateReportPM). */
    public List<Map<String, Object>> pmSlip(UserAccount u, int financialYearId, int id) {
        Map<String, Object> m = p("OrganizationId", org(u), "CompanyId", co(u), "FinancialYearId", financialYearId, "DocumentTypeId", 0);
        if (id != 0) m.put("Id", id);
        return db.rows("[dbo].[USP_FoodProductionPartyProcessingPackingMaterial_SlipAndRegister]", m);
    }

    /** ProductionReports.InvFoodProductionPartyProcessingRecoverySummeryReport (608). */
    public List<Map<String, Object>> summary608(UserAccount u, int jobOrderId, int languageId) {
        Map<String, Object> m = p("OrganizationId", org(u), "CompanyId", co(u), "JobOrderId", jobOrderId);
        if (languageId != 0) m.put("LanguageId", languageId);
        return db.rows("Sp_InvFoodProductionPartyProcessing_Summery_Rpt", m);
    }

    /** ProductionReports.FoodProductionPartyProcessingIssuanceGrnWiseByJobOrderId_609. */
    public List<Map<String, Object>> issuance609(UserAccount u, int jobOrderId) {
        return db.rows("Sp_InvFoodProductionPartyProcessingIssuanceGrnWiseByJobOrderId_rpt", p("OrganizationId", org(u), "CompanyId", co(u), "InvJobOrderId", jobOrderId));
    }

    // =========================================================================== 675 processing bill (BLL 0302)

    public int billCode(UserAccount u, int financialYearId) {
        List<Map<String, Object>> r = db.rows("USP_InvProductionProcessingBill_GetAllMethod", p("OrganizationId", org(u), "CompanyId", co(u),
                "FinancialYearId", financialYearId, "DocumentTypeId", 123, "Activity", "GenerateCode"));
        return r.isEmpty() ? 0 : toInt(r.get(0).get("BillNo"));
    }

    public List<Map<String, Object>> bill(int id) { return db.rows("USP_InvProductionProcessingBill_GetAllMethod", p("Id", id, "Activity", "ReadById")); }
    public List<Map<String, Object>> billChild(int id, String activity) { return db.rows("USP_InvProductionProcessingBill_GetAllMethod", p("Id", id, "Activity", activity)); }

    public void billDelete(int entryUserId, int id) {
        db.rows("USP_InvProductionProcessingBill_GetAllMethod", p("EntryUserId", entryUserId, "Id", id, "Activity", "DeleteById"));
    }

    public List<Map<String, Object>> billHistory(Map<String, Object> m) { return db.rows("[dbo].[USP_InvProductionProcessingBill_FormHistory]", m); }

    /** GetDataForDropDownFromInvProductionProcessingBill: the BLL's (x != "" || x != null) guards are always true -> null values are omitted. */
    public List<Map<String, Object>> billDropDown(UserAccount u) {
        return db.rows("[dbo].[USP_GetDataForDropDownFromInvProductionProcessingBill]", p("OrganizationId", org(u), "CompanyId", co(u)));
    }

    public List<Map<String, Object>> billInputs(UserAccount u, int financialYearId, int jobOrderId) {
        return db.rows("USP_InvProductionProcessingBill_GetAllMethod", p("OrganizationId", org(u), "CompanyId", co(u), "FinancialYearId", financialYearId,
                "DocumentTypeId", 117, "InvJobOrderId", jobOrderId, "Activity", "GetInPutForProcessingBill"));
    }

    public List<Map<String, Object>> billOutputs(UserAccount u, int financialYearId, int jobOrderId, String entryType) {
        Map<String, Object> m = p("OrganizationId", org(u), "CompanyId", co(u), "FinancialYearId", financialYearId, "DocumentTypeId", 118, "InvJobOrderId", jobOrderId);
        if (entryType != null && !entryType.isEmpty()) m.put("EntryType", entryType);
        m.put("Activity", "GetOutPutForProcessingBillByJobOrder");
        return db.rows("USP_InvProductionProcessingBill_GetAllMethod", m);
    }

    // =========================================================================== 602 stock conversion (BLL 0297)

    public int conversionCode(UserAccount u, int financialYearId) {
        Map<String, Object> m = p("OrganizationId", org(u), "CompanyId", co(u), "DocumentTypeId", 176, "BranchedId", branch(u));
        if (financialYearId != 0) m.put("FinancialYearId", financialYearId);
        List<Map<String, Object>> r = db.rows("USP_InvStockConversionPartyProcessing_GenerateCode", m);
        return r.isEmpty() ? 0 : toInt(r.get(0).get("DocNo"));
    }

    public List<Map<String, Object>> conversion(int id) { return db.rows("USP_InvStockConversionPartyProcessing_ReadById", p("Id", id)); }
    public List<Map<String, Object>> conversionDetails(int id) { return db.rows("USP_InvStockConversionPartyProcessingDetail_ReadById", p("Id", id)); }
    public List<Map<String, Object>> conversionPm(int id) { return db.rows("[USP_InvStockConversionPartyProcessingPackingMaterial_ReadbyId]", p("Id", id)); }

    public void conversionDelete(int entryUserId, int id) {
        db.rows("usp_InvStockConversionPartyProcessingDeleteById", p("EntryUserId", entryUserId, "Id", id));
    }

    public List<Map<String, Object>> conversionHistory(Map<String, Object> m) { return db.rows("USP_InvStockConversionPartyProcessing_FormHistory", m); }

    public List<Map<String, Object>> referencePartiesByStockParty(UserAccount u, int stockPartyId) {
        return db.rows("[Sp_ReferenceParties_GetAllMethod]", p("OrganizationId", org(u), "CompanyId", co(u), "StockPartyId", stockPartyId,
                "Activity", "GellAllReferencePartiesStockPartyWise"));
    }

    // =========================================================================== 603 stock transfer (BLL 0295)

    public int transferCode(UserAccount u, int financialYearId) {
        List<Map<String, Object>> r = db.rows("Sp_InvStockTransferHeaderPartyProcessing_GetAllMethod", p("OrganizationId", org(u), "CompanyId", co(u),
                "BranchesId", branch(u), "DocumentTypeId", 220, "FinancialYearId", financialYearId, "Activity", "GenerateCode"));
        return r.isEmpty() ? 0 : toInt(r.get(0).get("DocNo"));
    }

    public List<Map<String, Object>> transfer(int id) { return db.rows("Sp_InvStockTransferHeaderPartyProcessing_GetAllMethod", p("Id", id, "Activity", "ReadById")); }
    public List<Map<String, Object>> transferDetails(int id) {
        return db.rows("Sp_InvStockTransferHeaderPartyProcessing_GetAllMethod", p("HeaderId", id, "Activity", "ReadDetailByHeaderId"));
    }

    public List<Map<String, Object>> transferHistory(Map<String, Object> m) { return db.rows("usp_InvStockTransferPartyProcessing_FormHistory", m); }

    /** WbTransactions.GetTicketNoGrnForPurchaseFromPartyProcessing: @DocumentTypeId / @Id only when RecId != 0. */
    public List<Map<String, Object>> ticketNos(UserAccount u, int documentTypeId, int recId) {
        Map<String, Object> m = p("OrganizationId", org(u), "CompanyId", co(u));
        if (recId != 0) { m.put("DocumentTypeId", documentTypeId); m.put("Id", recId); }
        return db.rows("usp_getTicketNoGrnForPurchase", m);
    }

    // =========================================================================== 604 stock adjustment (BLL 0296)

    public int adjustmentCode(UserAccount u, int financialYearId) {
        List<Map<String, Object>> r = db.rows("usp_InvStockAdjustmentPartyProcessing_GetAllMethod", p("OrganizationId", org(u), "CompanyId", co(u),
                "BranchesId", branch(u), "DocumentTypeId", 158, "FinancialYearId", financialYearId, "Activity", "GenerateCode"));
        return r.isEmpty() ? 0 : toInt(r.get(0).get("DocNo"));
    }

    public List<Map<String, Object>> adjustment(int id) { return db.rows("usp_InvStockAdjustmentPartyProcessing_GetAllMethod", p("Id", id, "Activity", "ReadById")); }
    public List<Map<String, Object>> adjustmentDetails(int id) {
        return db.rows("usp_InvStockAdjustmentPartyProcessing_GetAllMethod", p("Id", id, "Activity", "ReadDetailByHeaderId"));
    }

    /** DAL DeleteById: @OrganizationId, @CompanyId, @DocumentTypeId, @Id, @EntryUser, @Activity (ExecuteNonQuery). */
    public void adjustmentDelete(UserAccount u, int id, int entryUserId) {
        exec("usp_InvStockAdjustmentPartyProcessing_GetAllMethod", p("OrganizationId", org(u), "CompanyId", co(u), "DocumentTypeId", 158, "Id", id,
                "EntryUser", entryUserId, "Activity", "DeleteById"));
    }

    public List<Map<String, Object>> adjustmentHistory(Map<String, Object> m) { return db.rows("usp_InvStockAdjustmentPartyProcessing_FormHistory", m); }

    /** GetDataForDropDownFromStockAdjustment(org, comp, null): the BLL's always-true guard sends @Activity = null -> not sent. */
    public List<Map<String, Object>> adjustmentDropDown(UserAccount u) {
        return db.rows("[dbo].[USP_GetDataForDropDownFromAdjustmentPartyProcessing]", p("OrganizationId", org(u), "CompanyId", co(u)));
    }

    // =========================================================================== 614 wages bill (BLL 0485 ContractorWages)

    public int wagesCode(UserAccount u, int financialYearId) {
        List<Map<String, Object>> r = db.rows("Sp_InvContractorWagesBillHeader_GetAllMethod", p("OrganizationId", org(u), "CompanyId", co(u),
                "RefDocumentTypeId", 219, "FinancialYearId", financialYearId, "Activity", "GenerateCode"));
        return r.isEmpty() ? 0 : toInt(r.get(0).get("DocNo"));
    }

    public int wagesIdByReference(UserAccount u, int refDocumentTypeId, int refDocNoId, int financialYearId, String refDocument) {
        Map<String, Object> m = p("OrganizationId", org(u), "CompanyId", co(u), "RefDocumentTypeId", refDocumentTypeId, "RefDocNoId", refDocNoId,
                "FinancialYearId", financialYearId);
        if (refDocument != null && !refDocument.isEmpty()) m.put("ReqType", refDocument);
        m.put("Activity", "GetIdByRefDocTypeIdAndRefDocId");
        List<Map<String, Object>> r = db.rows("Sp_InvContractorWagesBillHeader_GetAllMethod", m);
        return r.isEmpty() ? 0 : toInt(r.get(0).get("Id"));
    }

    public List<Map<String, Object>> wages(int id) { return db.rows("Sp_InvContractorWagesBillHeader_GetAllMethod", p("Id", id, "Activity", "ReadById")); }
    public List<Map<String, Object>> wagesDetails(int id) {
        return db.rows("Sp_InvContractorWagesBillHeader_GetAllMethod", p("Id", id, "Activity", "ReadDetailByHeaderId"));
    }

    public List<Map<String, Object>> wagesDocumentTypes(UserAccount u) {
        return db.rows("Sp_InvContractorWagesBillHeader_GetAllMethod", p("OrganizationId", org(u), "CompanyId", co(u), "Activity", "GetDocumentTypesFromContractorWages"));
    }

    public List<Map<String, Object>> wagesHistory(Map<String, Object> m) { return db.rows("[dbo].[USP_ContractorWagesBillHeader_FormHistory]", m); }

    /** SupplierCustomer.ReadByOrganizationCompanyIdForContractorWages. */
    public List<Map<String, Object>> contractors(UserAccount u) {
        return db.rows("Sp_SupplierCustomer_GetAllMethod", p("OrganizationId", org(u), "CompanyId", co(u), "Activity", "ReadByOrganizationCompanyIdForContractorWages"));
    }

    /** CommonServices.GetWagesAccount(Ids, 0, 1) -> InvConractorWagesAccounts.GetWagesItemsByWagesTypeIds (@WagesActivityId 0 not sent, @ActionId 1). */
    public List<Map<String, Object>> wagesAccounts(UserAccount u, String ids) {
        Map<String, Object> m = p("OrganizationId", org(u), "CompanyId", co(u));
        if (ids != null && !ids.isEmpty()) m.put("WagesLookupIds", ids);
        m.put("ActionId", 1);
        m.put("Activity", "GetWagesItemsByWagesTypeIds");
        return db.rows("Sp_InvConractorWagesAccounts_GetAllMethod", m);
    }

    /** CommonServices.GetWagesRate -> GetWagesScheduleRateByEffectiveDateWagesAccountIdandPackSize (@PackUomTo never set). */
    public double wagesRate(UserAccount u, LocalDateTime date, double packSize, int wagesAccountId, int contractorId) {
        List<Map<String, Object>> r = db.rows("Sp_InvContractorWagesSchedule_GetAllMethod", p("OrganizationId", org(u), "CompanyId", co(u),
                "InvConractorWagesAccountsId", wagesAccountId, "ContractorId", contractorId, "EffectedDate", ts(date), "PackUomFrom", packSize,
                "Activity", "GetWagesScheduleRateByEffectiveDateWagesAccountIdandPackSize"));
        return r.isEmpty() ? 0d : com.mst.services.hrm.HrmSupport.toDouble(r.get(0).get("WageRate"));
    }

    /** CommonServices.CheckItemsFreeofcostforWages -> USP_CheckItemsFreeofcostforWages. */
    public boolean freeOfCost(UserAccount u, LocalDateTime docDate, int refDocumentTypeId, int itemId, int wagesAccountId) {
        List<Map<String, Object>> r = db.rows("USP_CheckItemsFreeofcostforWages", p("OrganizationId", org(u), "CompanyId", co(u), "ItemId", itemId,
                "DocDate", ts(docDate), "RefDocumentTypeId", refDocumentTypeId, "WagesAccountId", wagesAccountId));
        return !r.isEmpty() && com.mst.services.hrm.HrmSupport.toBool(r.get(0).get("IsFreeocCost"));
    }

    /** InvGrn.GetPendingDocumentsForConractorWagesPartyProcessingByRefIds (@ReqType never set by the form). */
    public List<Map<String, Object>> wagesPendingByRef(UserAccount u, int documentTypeId, int id) {
        return db.rows("Sp_InvContractorWagesBillHeader_GetAllMethod", p("OrganizationId", org(u), "CompanyId", co(u), "DocumentTypeId", documentTypeId,
                "Id", id, "Activity", "GetPendingDocumentsForConractorWagesPartyProcessingByRefIds"));
    }

    /** InvGrn.GetAllPendingRecordsForConractorWagesPartyProcessing: @DocumentTypeId / @Id only when non-zero. */
    public List<Map<String, Object>> wagesPendingAll(UserAccount u, int documentTypeId, int id) {
        Map<String, Object> m = p("OrganizationId", org(u), "CompanyId", co(u));
        if (documentTypeId != 0) m.put("DocumentTypeId", documentTypeId);
        if (id != 0) m.put("Id", id);
        return db.rows("[dbo].[USP_GetAllPendingRecordsForConractorWagesPartyProcessing]", m);
    }

    /** InvGrn.GetDetailForContractorWagesPartyProcessingByDocumentTypeIdAndId: @ReqType only when non-empty. */
    public List<Map<String, Object>> wagesReferenceDetail(UserAccount u, int id, int documentTypeId, String reqType) {
        Map<String, Object> m = p("OrganizationId", org(u), "CompanyId", co(u), "Id", id, "DocumentTypeId", documentTypeId);
        if (reqType != null && !reqType.isEmpty()) m.put("ReqType", reqType);
        m.put("Activity", "GetDetailForContractorWagesPartyProcessingByDocumentTypeIdAndId");
        return db.rows("Sp_InvContractorWagesBillHeader_GetAllMethod", m);
    }

    /** WagesDeleteByRefDocTypeAndId (GetDataTableProc; the BLL spells "@RefDocId " - SQL Server matches it to @RefDocId). */
    public void wagesDeleteByRef(UserAccount u, int refDocumentTypeId, int refDocId) {
        db.rows("USP_WagesDeleteByRefDocTypeAndId", p("OrganizationId", org(u), "CompanyId", co(u), "RefDocumentTypeId", refDocumentTypeId, "RefDocId", refDocId));
    }
}
