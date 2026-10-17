package com.mst.services;

import com.mst.models.UserAccount;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collections;
import java.util.LinkedHashMap;
import java.util.LinkedHashSet;
import java.util.List;
import java.util.Map;
import java.util.Set;

import static com.mst.services.SteelProductionSupport.ci;
import static com.mst.services.SteelProductionSupport.day;
import static com.mst.services.SteelProductionSupport.isBlank;
import static com.mst.services.SteelProductionSupport.list;
import static com.mst.services.SteelProductionSupport.literal;
import static com.mst.services.SteelProductionSupport.now;
import static com.mst.services.SteelProductionSupport.params;
import static com.mst.services.SteelProductionSupport.str;
import static com.mst.services.SteelProductionSupport.toDouble;
import static com.mst.services.SteelProductionSupport.toInt;
import static com.mst.services.SteelProductionSupport.ts;

/**
 * Screen 600 "Production Job Order" (Production Steel, module 107) —
 * Architecture.WinApp.Steel.Production/ProductionPreCostingAndJobOrder_St.cs, DocumentTypeId 1511.
 *
 * BLL: Architecture.BLL.Steel.Production.InvProductionJobOrder (decompiled 0236), DAL 0210:
 *   [ST].[USP_InvProductionJobOrder_Insert] / [ST].[USP_InvProductionJobOrder_Update]  (header, SetProc over the model: 33/34 @params)
 *   [ST].[USP_InvProductionJobOrderInput_Insert]      (17)   [ST].[USP_InvProductionJobOrderOutput_Insert] (21)
 *   [ST].[USP_InvProductionJobOrderPlant_Insert]      (6)    [ST].[USP_InvProductionJobOrderOverHeads_Insert] (10)
 *   [ST].[USP_ProductionJobOrder_GetAllMethod]  @Activity = GenerateCode | ReadById | ReadInputDetailByJobOrderId |
 *        ReadOutPutDetailByJobOrderId | ReadPlantDetailByJobOrderId | ReadOverHeadsDetailByJobOrderId | FormHistory
 *   [ST].[USP_ProductionJobOrder_REPORT] (print pre-check of CommonServices.ProductionJobOrderSlipSteel1563 :16748)
 * Guards that only exist in the dbo BLL (Architecture.BLL.Production.InvProductionJobOrder 0335):
 *   Sp_InvProductionJobOrder_GetAllMethod @Activity = CheckJobOrderIdRefToInvFoodProduction | CheckPlantExistinProductionagaintJobOrder
 *
 * Validation messages are the desktop's, in the desktop's order (FormValidation :1302, Insert :1349).
 * The Packing Material grid (tabPage6) is removed on load (:654) and never saved — reproduced: the
 * page has no Packing Material tab and the save ignores PM rows, exactly as Insert() does.
 */
@Service
public class SteelProductionJobOrderService {

    private static final Logger LOG = LoggerFactory.getLogger(SteelProductionJobOrderService.class);

    public static final String SCREEN = "ProductionPreCostingAndJobOrder_St";
    public static final int DOC_TYPE = 1511;
    private static final String PROC = "[ST].[USP_ProductionJobOrder_GetAllMethod]";

    /* Literal lists the form builds itself (:836 FilterTypeFill, :1661 EntryTypeFill, :2250 EntryTypeOutPut,
       :4253 PlanTypeBind, :4275 ProductionTypeBind, :4297 StatusBind). */
    private static final String[][] FILTER_TYPES = { {"1","SaleOrder"}, {"2","Export Contract"}, {"3","Item Type"}, {"4","Item Category"} };
    private static final String[][] ENTRY_TYPES = { {"1","Input"}, {"2","Output"} };
    private static final String[][] OUTPUT_ENTRY_TYPES = { {"1","ManualRate"}, {"2","AutoRate"} };
    private static final String[][] PLAN_TYPES = { {"1","Local"}, {"2","Export"}, {"3","Party"} };
    private static final String[][] PRODUCTION_TYPES = { {"1","Husking"}, {"2","Processing"}, {"3","Other"} };
    private static final String[][] STATUSES = { {"1","In Process"}, {"2","Complete"}, {"3","Cancel"} };

    /* The non-virtual properties of the Steel models - GenericProvider.SetProc sends exactly these. */
    static final Set<String> P_HEADER = set("Id","DocumentTypeId","DocDate","DocNo","ProductionJobOrderNo","PlanType",
            "PlanTypeSrNo","ProductionType","LotReference","ProductionInsturction","PackingInstructions","DeliveryInstructions",
            "OtherInstructions","StartDate","EndDate","PlanStatus","WorkInProccessAcId","WipItemId","FinishGoodRate",
            "ApprovedUserId","ApprovedDate","IsApproved","EntryDate","EntryUserId","ModifyDate","ModifyUserId","OrganizationId",
            "CompanyId","BranchesId","ProjectsId","FinancialYearId","ActionId","PendingForView");
    static final Set<String> P_INPUT = set("Id","InvProductionJobOrderId","JobLotId","ItemId","PackUomId","Qty","NetWeight",
            "WarehouseId","RemarksDetail","PackingTypeId","ItemRate","ItemAmount","RateUomId","RefDocumentTypeId","RefDocIdNo",
            "RefDocSubIdNo","ActionTypeId");
    static final Set<String> P_OUTPUT = set("Id","InvProductionJobOrderId","EntryTypeId","ItemId","InvPackingTypeId","Qty",
            "PackUomId","NetWeight","RemarksDetail","JobLotId","WarehouseId","ItemRate","ItemAmount","RateUomId","OHExpense",
            "PMExpense","OHGeneralExpense","PMGeneralExpense","NetRate","NetAmount","ActionTypeId");
    static final Set<String> P_PLANT = set("Id","InvProductionJobOrderId","PlantId","RemarksDetail","BranchesId","ActionTypeId");
    static final Set<String> P_OVERHEAD = set("Id","ProductionJobOrderId","CharOfAccountId","Amount","RemarksOH","ItemQty",
            "ItemRate","BrandId","BrandUomId","ActionTypeId");

    private final SteelProductionSupport s;
    private final JdbcTemplate jdbc;

    public SteelProductionJobOrderService(SteelProductionSupport support, JdbcTemplate jdbc) {
        this.s = support;
        this.jdbc = jdbc;
    }

    // ================================================================================= load

    /** frmProductionJobOrder_Load:647 - rights and every picker, plus the generated plan code. */
    public Map<String, Object> lookups() {
        UserAccount u = s.user();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("canSave", s.hasRight(SCREEN, "Save"));                   // :658
        out.put("canUpdate", s.hasRight(SCREEN, "Update"));               // :659
        out.put("canPrint", s.hasRight(SCREEN, "Print"));                 // :660
        out.put("canViewAllRecords", s.hasRight(SCREEN, "CanView AllRecord"));
        out.put("canRateAndAmount", s.hasRight(SCREEN, "Rate"));          // grdDetailPackingMaterialSettings:3341
        out.put("branchesId", s.branchId(u));
        out.put("rateDecimals", s.rateDecimals(u));
        out.put("amountDecimals", s.amountDecimals(u));

        out.put("wipItems", q("ItemFill", "EXEC dbo.Sp_Item_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), "AllItemsBindForSteel"));                   // :1171
        out.put("jobLots", s.jobLots(u));                                                              // :4066
        out.put("accounts", q("AccountFills", "EXEC dbo.Sp_COAAllocation_GetAllMethod @OrganizationId=?, @CompanyId=?, @UserId=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), u.getId(), "COAForCombobindig"));             // :1125
        out.put("planTypes", literal(PLAN_TYPES));
        out.put("productionTypes", literal(PRODUCTION_TYPES));
        out.put("statuses", literal(STATUSES));
        out.put("entryTypes", literal(ENTRY_TYPES));
        out.put("outputEntryTypes", literal(OUTPUT_ENTRY_TYPES));
        out.put("filterTypes", literal(FILTER_TYPES));
        out.put("planCode", nextCode(u));                                                              // :1137
        out.put("plants", q("PlantFeader", "EXEC dbo.Sp_InvProductionPlant_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), "GetALL"));                                   // :820
        out.put("warehouses", s.activeWarehouses(u));                                                  // :4005
        out.put("items", allItems(u));                                                                 // :4031
        out.put("packingTypes", s.packingTypes());                                                     // :4238
        out.put("wagesAccounts", q("WagesAccountFill",
                "EXEC dbo.Sp_InvConractorWagesAccounts_GetAllMethod @OrganizationId=?, @CompanyId=?, @ActionId=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), 1, "GetWagesItemsByWagesTypeIds"));          // :4319 (invisible combos)
        out.put("packingMaterialItems", q("PackingMaterialItemBind",
                "EXEC dbo.Sp_Item_GetAllMethod @OrganizationId=?, @CompanyId=?, @LookupTypeIds=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), "14", "GetItemByItemTypeId"));                // :4336
        out.put("overheadAccounts", overheadAccounts(u));                                              // :2679
        return out;
    }

    /** ChartOfAccountOverHead:2679 - COAAllocation.GetAll, rows whose AccountTypeId is not 2 or 11. */
    private List<Map<String, Object>> overheadAccounts(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : q("ChartOfAccountOverHead",
                "EXEC dbo.Sp_COAAllocation_GetAllMethod @OrganizationId=?, @CompanyId=?, @UserId=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), u.getId(), "COAAllocationSearch")) {
            int t = toInt(ci(r, "AccountTypeId"));
            if (t != 2 && t != 11) {
                Map<String, Object> o = new LinkedHashMap<>();
                o.put("Id", ci(r, "Id"));
                o.put("AccountTitle", ci(r, "AccountTitle"));
                out.add(o);
            }
        }
        return out;
    }

    private List<Map<String, Object>> allItems(UserAccount u) {
        return q("ItemDetailFill", "EXEC dbo.Sp_Item_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), "ReadAllItems");
    }

    /** GeneratePlanCode:1137 - GetGenerateCode, DocumentTypeId 1511. */
    public int nextCode(UserAccount u) {
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@BranchesId", s.branchId(u));
        p.put("@DocumentTypeId", DOC_TYPE);
        p.put("@FinancialYearId", s.ctx().currentFinancialYearId());
        p.put("@Activity", "GenerateCode");
        List<Map<String, Object>> rows = s.query(PROC, p);
        return rows.isEmpty() ? 0 : toInt(ci(rows.get(0), "DocNo"));
    }

    /** cmbFiltersType_Leave:855 - the "Doc No / Other" list for the chosen filter type. */
    public List<Map<String, Object>> docNos(String filterType) {
        UserAccount u = s.user();
        switch (filterType == null ? "" : filterType.trim()) {
            case "SaleOrder":                    // :1053 SaleOrder.GetSaleOrderNo -> Sp_SaleOrder_GetAllMethod ReadSaleOrderNo
                return SteelProductionSupport.idName(q("GetLocalSaleOrderNo",
                        "EXEC dbo.Sp_SaleOrder_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",
                        u.getOrganizationId(), u.getCompanyId(), "ReadSaleOrderNo"), "Id", "DocNo");
            case "Export Contract":              // :1029 CommonServices.GetLcOrderNo -> Sp_ExImLcOrder_GetAllMethod GetLcOrderNo
                return SteelProductionSupport.idName(q("GetExportContractNo",
                        "EXEC dbo.Sp_ExImLcOrder_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",
                        u.getOrganizationId(), u.getCompanyId(), "GetLcOrderNo"), "Id", "LcOrderNo");
            case "Item Type":                    // :1081 ItemType.Getall
                return SteelProductionSupport.idName(q("GetItemTypes",
                        "EXEC dbo.Sp_ItemType_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",
                        u.getOrganizationId(), u.getCompanyId(), "ReadByOrganizationCompanyId"), "Id", "TypeDescription");
            case "Item Category":                // :1105 ItemCategory.Getall
                return SteelProductionSupport.idName(q("GetItemCategory",
                        "EXEC dbo.Sp_ItemCategory_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",
                        u.getOrganizationId(), u.getCompanyId(), "ReadByOrganizationCompanyId"), "Id", "CategoryDescription");
            default:
                return new ArrayList<>();
        }
    }

    /**
     * cmbDocNo_Leave:884 - the item list of cmbItem (Output) or cmbItemInput (Input) for the filter.
     * Item Type / Item Category are filtered in memory on ItemTypeId / ItemCategoryId, as the desktop does;
     * the Output grid filters CommonServices.ItemGetAllServiceBind (ReadByOrganizationCompanyId), the Input
     * grid filters Item.ReadAllItems. No filter -> ItemDetailFill (:4023).
     */
    public List<Map<String, Object>> filteredItems(String filterType, int docNo, String entryType) {
        UserAccount u = s.user();
        String f = filterType == null ? "" : filterType.trim();
        boolean output = "Output".equals(entryType);
        if (f.isEmpty() || docNo <= 0) return SteelProductionSupport.idName(allItems(u), "Id", "ItemName");
        if ("Item Category".equals(f) || "Item Type".equals(f)) {
            List<Map<String, Object>> src = output
                    ? q("ItemGetAllServiceBind", "EXEC dbo.Sp_Item_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",
                        u.getOrganizationId(), u.getCompanyId(), "ReadByOrganizationCompanyId")
                    : allItems(u);
            String col = "Item Category".equals(f) ? "ItemCategoryId" : "ItemTypeId";
            List<Map<String, Object>> out = new ArrayList<>();
            for (Map<String, Object> r : src) {
                if (toInt(ci(r, col)) == docNo) {
                    Map<String, Object> o = new LinkedHashMap<>();
                    o.put("Id", ci(r, "Id"));
                    o.put("Name", ci(r, "ItemName"));
                    out.add(o);
                }
            }
            return out;
        }
        if ("SaleOrder".equals(f)) {             // :4105 SaleOrder.ReadByPurchaseOrderIDNOrderItemId - key OrderItemId
            return SteelProductionSupport.idName(q("ItemDetailFillBySaleOrder",
                    "EXEC dbo.Sp_SaleOrder_GetAllMethod @SaleOrderId=?, @Activity=?", docNo, "ReadBySaleOrderIDNOrderItemId"),
                    "OrderItemId", "ItemName");
        }
        if ("Export Contract".equals(f)) {       // :4136 ExImLcOrder.ReadByExportSaleOrderIDNOrderItemId - key ExImItemId
            return SteelProductionSupport.idName(q("ItemDetailFillByExportSaleContractId",
                    "EXEC dbo.Sp_ExImLcOrder_GetAllMethod @OrganizationId=?, @CompanyId=?, @Id=?, @Activity=?",
                    u.getOrganizationId(), u.getCompanyId(), docNo, "ReadByExportSaleOrderIDNOrderItemId"), "ExImItemId", "ItemName");
        }
        return SteelProductionSupport.idName(allItems(u), "Id", "ItemName");
    }

    /** bindPackSizeInput:1713 / bindRateUomAndItemPackUom:4079 - CommonServices.GetUomScheduleByItemId. */
    public List<Map<String, Object>> uomSchedule(int itemId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : s.uomSchedule(s.user(), itemId)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", ci(r, "Id"));
            o.put("UOMCode", ci(r, "UOMCode"));
            o.put("Equivalent", toDouble(ci(r, "Equivalent")));
            out.add(o);
        }
        return out;
    }

    /**
     * GetAvgRate:2183 (DocumentTypeId 1511) and BtnGenerateInPutRates_Click:5226 (401) -
     * CommonServices.AvgRateOnlyForCGS -> Sp_GetAvgRatesAndStockInHand_GetAllMethod @Activity=GetOnlyAvgRateForCGS,
     * the AvgRate column of the first row. Zero-valued optional parameters are omitted as the BLL omits them.
     * Also returns the item's UOM-schedule row whose Equivalent == 1 (the rate UOM the desktop picks).
     */
    public Map<String, Object> avgRate(int itemId, String docDate, int documentTypeId, int recId, int jobLotId, int warehouseId) {
        UserAccount u = s.user();
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@ItemId", itemId);
        p.put("@DocDate", ts(docDate) == null ? now() : ts(docDate));
        if (documentTypeId != 0) p.put("@DocumentTypeId", documentTypeId);
        if (recId != 0) p.put("@RecId", recId);
        if (jobLotId != 0) p.put("@JobLotId", jobLotId);
        if (warehouseId != 0) p.put("@WarehouseId", warehouseId);
        if (s.branchId(u) != 0) p.put("@BranchesId", s.branchId(u));
        p.put("@Activity", "GetOnlyAvgRateForCGS");
        double rate = 0;
        List<Map<String, Object>> rows = s.query("dbo.Sp_GetAvgRatesAndStockInHand_GetAllMethod", p);
        if (!rows.isEmpty()) rate = toDouble(ci(rows.get(0), "AvgRate"));
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rate", rate);
        int rateUomId = 0; String rateUom = ""; double eq = 0;
        for (Map<String, Object> r : s.uomSchedule(u, itemId)) {
            if (toDouble(ci(r, "Equivalent")) == 1.0) { rateUomId = toInt(ci(r, "Id")); rateUom = str(ci(r, "UOMCode")); eq = 1.0; break; }
        }
        out.put("rateUomId", rateUomId);
        out.put("rateUom", rateUom);
        out.put("rateUomEquivalent", eq);
        return out;
    }

    // ================================================================================= read

    /** DataGridHistory_DoubleClick:3641 - InvProductionJobOrder.GetByID (ReadById + the four detail reads). */
    public Map<String, Object> load(int id) {
        UserAccount u = s.user();
        LinkedHashMap<String, Object> p = params();
        p.put("@Id", id);
        p.put("@Activity", "ReadById");
        List<Map<String, Object>> heads = s.query(PROC, p);
        if (heads.isEmpty()) return null;
        Map<String, Object> head = heads.get(0);

        Map<String, Object> h = new LinkedHashMap<>();
        h.put("Id", id);
        h.put("DocDate", day(ci(head, "DocDate")));
        h.put("DocNo", ci(head, "DocNo"));
        h.put("WorkInProccessAcId", ci(head, "WorkInProccessAcId"));
        h.put("WipItemId", ci(head, "WipItemId"));
        h.put("PlanType", ci(head, "PlanType"));
        h.put("ProductionJobOrderNo", ci(head, "ProductionJobOrderNo"));
        h.put("ProductionType", ci(head, "ProductionType"));
        h.put("StartDate", day(ci(head, "StartDate")));
        h.put("EndDate", day(ci(head, "EndDate")));
        h.put("PlanStatus", ci(head, "PlanStatus"));
        h.put("ProductionInsturction", ci(head, "ProductionInsturction"));
        h.put("DeliveryInstructions", ci(head, "DeliveryInstructions"));
        h.put("PackingInstructions", ci(head, "PackingInstructions"));
        h.put("OtherInstructions", ci(head, "OtherInstructions"));
        h.put("IsApproved", ci(head, "IsApproved"));

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("header", h);
        out.put("inputs", inputRows(id));
        out.put("outputs", outputRows(id));
        out.put("overheads", overheadRows(id));
        out.put("plants", plantRows(id));
        out.put("packingMaterials", new ArrayList<>());   // tabPage6 is removed on load (:654); nothing is read for it
        return out;
    }

    private List<Map<String, Object>> detail(int id, String activity) {
        LinkedHashMap<String, Object> p = params();
        p.put("@Id", id);
        p.put("@Activity", activity);
        return s.query(PROC, p);
    }

    /** dtInputDetail columns (:730) from ReadInputDetailByJobOrderId (:3675). */
    private List<Map<String, Object>> inputRows(int id) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : detail(id, "ReadInputDetailByJobOrderId")) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", ci(r, "Id"));
            o.put("RefDocumentTypeId", toInt(ci(r, "RefDocumentTypeId")));
            o.put("RefDocIdNo", toInt(ci(r, "RefDocIdNo")));
            o.put("RefDocSubIdNo", toInt(ci(r, "RefDocSubIdNo")));
            o.put("WareHouseId", toInt(ci(r, "WarehouseId")));
            o.put("WareHouse", ci(r, "WareHouseName"));
            o.put("JobLotId", toInt(ci(r, "JobLotId")));
            o.put("JobLot", ci(r, "JobLotCode"));
            o.put("ItemId", toInt(ci(r, "ItemId")));
            o.put("ItemName", ci(r, "ItemName"));
            o.put("PackTypeId", toInt(ci(r, "PackingTypeId")));
            o.put("PackType", ci(r, "PackingType"));
            o.put("BalanceQty", toDouble(ci(r, "Qty")));
            o.put("BalanceWeight", toDouble(ci(r, "NetWeight")));
            o.put("Qty", toDouble(ci(r, "Qty")));
            o.put("PackSizeId", toInt(ci(r, "PackUomId")));
            o.put("PackSize", ci(r, "PackUom"));
            o.put("PackSizeEquivalent", toDouble(ci(r, "PackUomEquivalent")));
            o.put("Weight", toDouble(ci(r, "NetWeight")));
            o.put("ItemRate", toDouble(ci(r, "ItemRate")));
            o.put("RateUomId", toInt(ci(r, "RateUomId")));
            o.put("RateUom", ci(r, "RateUom"));
            o.put("RateUomEquivalent", toDouble(ci(r, "RateUomEquivalent")));
            o.put("ItemAmount", toDouble(ci(r, "ItemAmount")));
            o.put("Remarks", ci(r, "RemarksDetail"));
            out.add(o);
        }
        return out;
    }

    /** dtPlannedOutput columns (:699) from ReadOutPutDetailByJobOrderId (:3679). */
    private List<Map<String, Object>> outputRows(int id) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : detail(id, "ReadOutPutDetailByJobOrderId")) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", ci(r, "Id"));
            o.put("EntryTypeId", toInt(ci(r, "EntryTypeId")));
            o.put("EntryType", ci(r, "EntryType"));
            o.put("WareHouseId", toInt(ci(r, "WarehouseId")));
            o.put("WareHouseName", ci(r, "WareHouseName"));
            o.put("JobLotId", toInt(ci(r, "JobLotId")));
            o.put("JobLotDescription", ci(r, "JobLotDescription"));
            o.put("ItemId", toInt(ci(r, "ItemId")));
            o.put("ItemName", ci(r, "ItemName"));
            o.put("PackingTypeId", toInt(ci(r, "InvPackingTypeId")));
            o.put("PackingTypeName", ci(r, "PackTypeDesc"));
            o.put("QtyOuter", toDouble(ci(r, "Qty")));
            o.put("OuterPackSizeId", toInt(ci(r, "PackUomId")));
            o.put("OuterPackSizeName", ci(r, "PackUomCode"));
            o.put("EquivalentPack", toDouble(ci(r, "PackUomCodeEquivalent")));
            o.put("Weight", toDouble(ci(r, "NetWeight")));
            o.put("ItemRate", toDouble(ci(r, "ItemRate")));
            o.put("RateUomId", toInt(ci(r, "RateUomId")));
            o.put("RateUom", ci(r, "RateUom"));
            o.put("EquivalentRate", toDouble(ci(r, "Equivalent")));
            o.put("ItemAmount", toDouble(ci(r, "ItemAmount")));
            o.put("OHExpense", toDouble(ci(r, "OHExpense")));
            o.put("PMExpense", toDouble(ci(r, "PMExpense")));
            o.put("OHGeneralExpense", toDouble(ci(r, "OHGeneralExpense")));
            o.put("PMGeneralExpense", toDouble(ci(r, "PMGeneralExpense")));
            o.put("NetRate", toDouble(ci(r, "NetRate")));
            o.put("NetAmount", toDouble(ci(r, "NetAmount")));
            o.put("Remarks", ci(r, "RemarksDetail"));
            out.add(o);
        }
        return out;
    }

    /** dtOH columns (:758) from ReadOverHeadsDetailByJobOrderId (:3683). */
    private List<Map<String, Object>> overheadRows(int id) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : detail(id, "ReadOverHeadsDetailByJobOrderId")) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", ci(r, "Id"));
            o.put("ChartOfAccountId", toInt(ci(r, "CharOfAccountId")));
            o.put("ChartOfAccount", ci(r, "AccountTitle"));
            o.put("BrandId", toInt(ci(r, "BrandId")));
            o.put("Brand", ci(r, "BrandName"));
            o.put("BrandUomId", toInt(ci(r, "BrandUomId")));
            o.put("BrandUom", ci(r, "BrandUom"));
            o.put("ItemQty", toDouble(ci(r, "ItemQty")));
            o.put("ItemRate", toDouble(ci(r, "ItemRate")));
            o.put("Amount", toDouble(ci(r, "Amount")));
            o.put("Remarks", ci(r, "RemarksOH"));
            out.add(o);
        }
        return out;
    }

    /** dtPlantdetail (:786) from ReadPlantDetailByJobOrderId (:3687). */
    private List<Map<String, Object>> plantRows(int id) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : detail(id, "ReadPlantDetailByJobOrderId")) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", ci(r, "Id"));
            o.put("PlantId", toInt(ci(r, "PlantId")));
            o.put("PlantName", ci(r, "PlantName"));
            o.put("RemarksDetail", ci(r, "RemarksDetail"));
            out.add(o);
        }
        return out;
    }

    // ============================================================================== history

    /** BindHistoryGrid:3545 - FormHistory (DocumentTypeId 1511, NoOfRecords 50 on tab change, 0 = Load All). */
    public Map<String, Object> history(int noOfRecords) {
        UserAccount u = s.user();
        boolean canViewAll = s.hasRight(SCREEN, "CanView AllRecord");
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@BranchesId", s.branchId(u));
        p.put("@DocumentTypeId", DOC_TYPE);
        p.put("@FinancialYearId", s.ctx().currentFinancialYearId());
        if (noOfRecords != 0) p.put("@NoOfRecords", noOfRecords);
        p.put("@CanViewAllRecord", canViewAll);
        if (!canViewAll) p.put("@EntryUserId", u.getId());
        p.put("@Activity", "FormHistory");
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> r : s.query(PROC, p)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", ci(r, "Id"));
            o.put("DocDate", day(ci(r, "DocDate")));
            o.put("DocNo", ci(r, "DocNo"));
            o.put("PlanType", ci(r, "PlanType"));
            o.put("JobOrderNo", ci(r, "ProductionJobOrderNo"));
            o.put("ProductionType", ci(r, "PlanType"));          // :3589 - the desktop puts PlanType in this column too
            o.put("StartDate", day(ci(r, "StartDate")));
            o.put("EndDate", day(ci(r, "EndDate")));
            o.put("PlanStatus", ci(r, "PlanStatus"));
            o.put("PackingInstruction", ci(r, "PackingInstructions"));
            o.put("DeliveryInstruction", ci(r, "DeliveryInstructions"));
            o.put("ProductionInsturction", ci(r, "ProductionInsturction"));
            o.put("OtherInstruction", ci(r, "OtherInstructions"));
            o.put("NoOfAttachments", ci(r, "NoOfAttachments"));
            rows.add(o);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("canViewAllRecords", canViewAll);
        out.put("rows", rows);
        return out;
    }

    // =============================================================================== guards

    /** grdInputDetail_DoubleClick:1950 (80) / grdPlannedOutput_DoubleClick:2488 (112) - CheckJobOrderIdRefToInvFoodProduction. */
    public Map<String, Object> refCheck(int documentTypeId, int jobOrderId) {
        UserAccount u = s.user();
        int n = 0;
        if (jobOrderId > 0) {
            LinkedHashMap<String, Object> p = params();
            p.put("@OrganizationId", u.getOrganizationId());
            p.put("@CompanyId", u.getCompanyId());
            p.put("@InvJobOrderId", jobOrderId);
            p.put("@DocumentTypeId", documentTypeId);
            p.put("@Activity", "CheckJobOrderIdRefToInvFoodProduction");
            n = toInt(s.scalar("dbo.Sp_InvProductionJobOrder_GetAllMethod", p));
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("count", n);
        if (n > 0) {
            out.put("message", documentTypeId == 80
                    ? "Record Not Update Because Record has exist in Production InPut"
                    : "Record Not Update Because Record has exist in Production OutPut");
        }
        return out;
    }

    /** grdPlantdetail_ColumnButtonClick:5128 - CheckPlantExistinProductionagaintJobOrder. */
    public Map<String, Object> plantDeletable(int jobOrderId, int plantId, String plantName) {
        UserAccount u = s.user();
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@Id", jobOrderId);
        p.put("@PlantId", plantId);
        p.put("@Activity", "CheckPlantExistinProductionagaintJobOrder");
        List<Map<String, Object>> rows = s.query("dbo.Sp_InvProductionJobOrder_GetAllMethod", p);
        boolean used = !rows.isEmpty() && SteelProductionSupport.toBool(rows.get(0).values().iterator().next());
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("deletable", !used);
        if (used) {
            String n = plantName == null ? "" : plantName;
            out.put("message", "You cannot be delete " + n + " because " + n + " Has been refered to production");
        }
        return out;
    }

    // ================================================================================= save

    /**
     * Insert:1349 - FormValidation, the confirm (done on the page), btnGenerate_Click (the costing is recomputed
     * on the page and sent), the four grid loops with their messages, then InvProductionJobOrder.Save:
     * header Insert/Update by Id and the four child inserts inside one transaction (DAL 0210 SetData).
     * Removed rows arrive with ActionTypeId 3, as RemovedInputList / RemovedOutputList / ... carry them.
     */
    @Transactional
    public Map<String, Object> save(Map<String, Object> body) {
        UserAccount u = s.user();
        int recId = toInt(body.get("Id"));
        boolean insert = recId == 0;
        if (insert && !s.hasRight(SCREEN, "Save")) throw new IllegalStateException("You do not have the Save right for this screen.");
        if (!insert && !s.hasRight(SCREEN, "Update")) throw new IllegalStateException("You do not have the Update right for this screen.");

        /* ---- FormValidation:1302 ---- */
        int docNo = toInt(body.get("DocNo"));
        if (docNo == 0) throw new IllegalArgumentException("Plan Code Field Required");
        String jobOrderNo = str(body.get("ProductionJobOrderNo"));
        if (jobOrderNo.isEmpty() || "0".equals(jobOrderNo)) throw new IllegalArgumentException("Mannual ReportNo Field Required");
        if (isBlank(str(body.get("ProductionType")))) throw new IllegalArgumentException("Production Type Field Required");
        if (toInt(body.get("WorkInProccessAcId")) == 0) throw new IllegalArgumentException("WorkInProcess Field Required");
        if (toInt(body.get("WipItemId")) == 0) throw new IllegalArgumentException("WIPItem Field Required");
        if (isBlank(str(body.get("PlanType")))) throw new IllegalArgumentException("Plan Type Field Required");
        if (isBlank(str(body.get("PlanStatus")))) throw new IllegalArgumentException("Status Field Required");

        List<Map<String, Object>> outputs = list(body.get("outputs"));
        List<Map<String, Object>> inputs = list(body.get("inputs"));
        List<Map<String, Object>> overheads = list(body.get("overheads"));
        List<Map<String, Object>> plants = list(body.get("plants"));

        /* :1409 / :1413 - live rows only (removed rows are carried with ActionTypeId 3) */
        if (count(outputs) == 0) throw new IllegalArgumentException("OutPut Grid Record Not Found");
        if (count(inputs) == 0) throw new IllegalArgumentException("Input Grid Record Not Found");

        /* ---- header (:1381) ---- */
        Map<String, Object> h = new LinkedHashMap<>();
        h.put("Id", recId);
        h.put("DocumentTypeId", DOC_TYPE);
        h.put("DocDate", ts(body.get("DocDate")) == null ? now() : ts(body.get("DocDate")));
        h.put("DocNo", docNo);
        h.put("PlanType", str(body.get("PlanType")));
        h.put("PlanTypeSrNo", 0);
        h.put("ProductionJobOrderNo", jobOrderNo);
        h.put("ProductionType", str(body.get("ProductionType")));
        h.put("LotReference", null);
        h.put("ProductionInsturction", str(body.get("ProductionInsturction")));
        h.put("PackingInstructions", str(body.get("PackingInstructions")));
        h.put("DeliveryInstructions", str(body.get("DeliveryInstructions")));
        h.put("OtherInstructions", str(body.get("OtherInstructions")));
        /* txtstartDate / txtEndDate are DateTimePickers - they always carry a date (:1389) */
        h.put("StartDate", ts(body.get("StartDate")) == null ? now() : ts(body.get("StartDate")));
        h.put("EndDate", ts(body.get("EndDate")) == null ? now() : ts(body.get("EndDate")));
        h.put("PlanStatus", str(body.get("PlanStatus")));
        h.put("WorkInProccessAcId", toInt(body.get("WorkInProccessAcId")));
        h.put("WipItemId", toInt(body.get("WipItemId")));
        h.put("FinishGoodRate", 0d);                        // :1403
        h.put("ApprovedUserId", null);
        h.put("ApprovedDate", null);
        h.put("IsApproved", false);                        // :1396
        h.put("EntryDate", now());
        h.put("EntryUserId", u.getId());
        h.put("ModifyDate", now());
        h.put("ModifyUserId", u.getId());
        h.put("OrganizationId", u.getOrganizationId());
        h.put("CompanyId", u.getCompanyId());
        h.put("BranchesId", s.branchId(u));
        h.put("ProjectsId", 0);
        h.put("FinancialYearId", s.ctx().currentFinancialYearId());
        h.put("ActionId", insert ? 1 : 2);                 // BLL Save:0236
        h.put("PendingForView", null);

        /* ---- output rows (:1424) ---- */
        List<Map<String, Object>> outRows = new ArrayList<>();
        for (Map<String, Object> r : outputs) {
            boolean removed = toInt(r.get("ActionTypeId")) == 3;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get("Id")));
            o.put("ActionTypeId", removed ? 3 : (toInt(r.get("Id")) == 0 ? 1 : 2));
            o.put("ItemId", toInt(r.get("ItemId")));
            o.put("InvPackingTypeId", toInt(r.get("PackingTypeId")));
            o.put("Qty", toDouble(r.get("QtyOuter")));
            o.put("PackUomId", toInt(r.get("OuterPackSizeId")));
            o.put("NetWeight", toDouble(r.get("Weight")));
            o.put("RemarksDetail", str(r.get("Remarks")));
            if (!removed && toInt(r.get("WareHouseId")) == 0) throw new IllegalArgumentException("WareHouse Field Required in Planned Output Detail");
            o.put("WarehouseId", toInt(r.get("WareHouseId")));
            if (!removed && toInt(r.get("JobLotId")) == 0) throw new IllegalArgumentException("JobLot Field Required in Planned Output Detail");
            o.put("JobLotId", toInt(r.get("JobLotId")));
            o.put("ItemRate", toDouble(r.get("ItemRate")));
            o.put("RateUomId", toInt(r.get("RateUomId")));
            o.put("ItemAmount", toDouble(r.get("ItemAmount")));
            o.put("OHExpense", toDouble(r.get("OHExpense")));
            o.put("PMExpense", toDouble(r.get("PMExpense")));
            o.put("OHGeneralExpense", toDouble(r.get("OHGeneralExpense")));
            o.put("PMGeneralExpense", toDouble(r.get("PMGeneralExpense")));
            o.put("NetRate", toDouble(r.get("NetRate")));
            o.put("NetAmount", toDouble(r.get("NetAmount")));
            o.put("EntryTypeId", toInt(r.get("EntryTypeId")));
            outRows.add(o);
        }

        /* ---- input rows (:1468) ---- */
        List<Map<String, Object>> inRows = new ArrayList<>();
        for (Map<String, Object> r : inputs) {
            boolean removed = toInt(r.get("ActionTypeId")) == 3;
            Map<String, Object> o = new LinkedHashMap<>();
            if (!removed) {
                if (toInt(r.get("ItemId")) == 0) throw new IllegalArgumentException("Item Field Require in Planned Input Detail");
                if (toInt(r.get("JobLotId")) == 0) throw new IllegalArgumentException("JobLot Field Required in Planned Input Detail");
                if (toDouble(r.get("Qty")) == 0) throw new IllegalArgumentException("Quantity Field Require in Planned Input Detail");
                if (toInt(r.get("PackSizeId")) == 0) throw new IllegalArgumentException("PackSize Field Required in Planned Input Detail");
                if (toDouble(r.get("Weight")) == 0) throw new IllegalArgumentException("Weight Field Required in Planned Input Detail");
                if (toInt(r.get("WareHouseId")) == 0) throw new IllegalArgumentException("WareHouse Field Require in Planned Input Detail");
                if (toInt(r.get("PackTypeId")) == 0) throw new IllegalArgumentException("Packing Type Field Require in Planned Input Detail");
            }
            o.put("Id", toInt(r.get("Id")));
            o.put("ActionTypeId", removed ? 3 : (toInt(r.get("Id")) == 0 ? 1 : 2));
            o.put("ItemId", toInt(r.get("ItemId")));
            o.put("JobLotId", toInt(r.get("JobLotId")));
            o.put("Qty", toDouble(r.get("Qty")));
            o.put("PackUomId", toInt(r.get("PackSizeId")));
            o.put("NetWeight", toDouble(r.get("Weight")));
            o.put("WarehouseId", toInt(r.get("WareHouseId")));
            o.put("PackingTypeId", toInt(r.get("PackTypeId")));
            o.put("ItemRate", toDouble(r.get("ItemRate")));
            o.put("RateUomId", toInt(r.get("RateUomId")));
            o.put("ItemAmount", toDouble(r.get("ItemAmount")));
            o.put("RemarksDetail", str(r.get("Remarks")));
            o.put("RefDocumentTypeId", toInt(r.get("RefDocumentTypeId")));
            o.put("RefDocIdNo", toInt(r.get("RefDocIdNo")));
            o.put("RefDocSubIdNo", toInt(r.get("RefDocSubIdNo")));
            inRows.add(o);
        }

        /* ---- overhead rows (:1531) - only rows with Amount > 0 and an account are saved ---- */
        List<Map<String, Object>> ohRows = new ArrayList<>();
        for (Map<String, Object> r : overheads) {
            boolean removed = toInt(r.get("ActionTypeId")) == 3;
            double amount = toDouble(r.get("Amount"));
            int coa = toInt(r.get("ChartOfAccountId"));
            if (!removed && !(amount > 0 && coa > 0)) continue;
            int brandId = toInt(r.get("BrandId")), brandUomId = toInt(r.get("BrandUomId"));
            double qty = toDouble(r.get("ItemQty")), rate = toDouble(r.get("ItemRate"));
            if (!removed) {
                if (coa == 0) throw new IllegalArgumentException("OverHead Account Field Require in Detail");
                if (brandId == 0 && brandUomId != 0) throw new IllegalArgumentException("OutPut Item Field Require in Detail");
                if (brandUomId == 0 && brandId != 0) throw new IllegalArgumentException("OutPut UomField Require in Detail");
                if (qty == 0 && rate != 0) throw new IllegalArgumentException("ItemQty Required in Detail");
                if (rate == 0 && qty != 0) throw new IllegalArgumentException("ItemRate Required in Detail");
                if (amount == 0) throw new IllegalArgumentException("Amount Required in Detail");
            }
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get("Id")));
            o.put("ActionTypeId", removed ? 3 : (toInt(r.get("Id")) == 0 ? 1 : 2));
            o.put("CharOfAccountId", coa);
            o.put("BrandId", brandId);
            o.put("BrandUomId", brandUomId);
            o.put("ItemQty", qty);
            o.put("ItemRate", rate);
            o.put("Amount", amount);
            o.put("RemarksOH", str(r.get("Remarks")));
            ohRows.add(o);
        }

        /* ---- plant rows (:1589) ---- */
        if (count(plants) == 0) throw new IllegalArgumentException("At Least One Detail Record Is Required In Plant Detail Grid");
        List<Map<String, Object>> plantRows = new ArrayList<>();
        for (Map<String, Object> r : plants) {
            boolean removed = toInt(r.get("ActionTypeId")) == 3;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get("Id")));
            o.put("ActionTypeId", removed ? 3 : (toInt(r.get("Id")) == 0 ? 1 : 2));
            o.put("PlantId", toInt(r.get("PlantId")));
            o.put("RemarksDetail", str(r.get("RemarksDetail")));
            o.put("BranchesId", null);                 // the form never sets InvProductionJobOrderPlant.BranchesId
            plantRows.add(o);
        }

        /* ---- DAL 0210 SetData, one transaction ---- */
        int result = s.setProc(insert ? "[ST].[USP_InvProductionJobOrder_Insert]" : "[ST].[USP_InvProductionJobOrder_Update]", h, P_HEADER);
        int id = result > 0 ? result : recId;
        for (Map<String, Object> o : inRows) {
            o.put("InvProductionJobOrderId", id);
            s.setProc("[ST].[USP_InvProductionJobOrderInput_Insert]", o, P_INPUT);
        }
        for (Map<String, Object> o : outRows) {
            o.put("InvProductionJobOrderId", id);
            s.setProc("[ST].[USP_InvProductionJobOrderOutput_Insert]", o, P_OUTPUT);
        }
        for (Map<String, Object> o : plantRows) {
            o.put("InvProductionJobOrderId", id);
            s.setProc("[ST].[USP_InvProductionJobOrderPlant_Insert]", o, P_PLANT);
        }
        for (Map<String, Object> o : ohRows) {
            o.put("ProductionJobOrderId", id);
            s.setProc("[ST].[USP_InvProductionJobOrderOverHeads_Insert]", o, P_OVERHEAD);
        }
        /* Attachments (FormHelper.UpdateAttachmentsForObject / DMS) are not ported - the desktop Attachment dialog has no web form. */

        Map<String, Object> res = SteelProductionSupport.ok((insert ? "Save Successfully" : "Update Successfully") + docNo);  // :1609 / :1613
        res.put("id", id);
        res.put("docNo", docNo);
        return res;
    }

    private static int count(List<Map<String, Object>> rows) {
        int n = 0;
        for (Map<String, Object> r : rows) if (toInt(r.get("ActionTypeId")) != 3) n++;
        return n;
    }

    // ================================================================================ print

    /** CommonServices.ProductionJobOrderSlipSteel1563:16748 - JobOrderPrintNew first; "No Record Found For Display" when empty. */
    public Map<String, Object> printCheck(int id) {
        UserAccount u = s.user();
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@FinancialYearId", s.ctx().currentFinancialYearId());
        p.put("@DocumentTypeId", DOC_TYPE);
        p.put("@BranchesId", s.branchId(u));
        p.put("@Id", id);
        List<Map<String, Object>> rows = s.query("[ST].[USP_ProductionJobOrder_REPORT]", p);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rows", rows.size());
        out.put("branchesId", s.branchId(u));
        if (rows.isEmpty()) out.put("message", "No Record Found For Display");
        return out;
    }

    /** DataGridHistory_LinkClicked:3733 - DMSAttachments.GetByID(id, base.Name). */
    public List<Map<String, Object>> attachments(int id) {
        s.user();
        return s.attachments(SCREEN, id);
    }

    // ===================================================== Load Invoices (LoadavailableTransactionsForIssuance)

    /** StockComboFill:197 - USP_Inventory_StockEvalautionDetail_DropDownAndLists, split on ActivityType. */
    public Map<String, Object> loaderCombos() {
        UserAccount u = s.user();
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@BranchesIds", String.valueOf(s.branchId(u)));
        List<Map<String, Object>> all = s.query("dbo.USP_Inventory_StockEvalautionDetail_DropDownAndLists", p);
        Map<String, Object> out = new LinkedHashMap<>();
        String[][] keys = { {"parentCategories","ParentCategories"}, {"itemCategories","ItemCategory"}, {"itemTypes","ItemType"},
                {"jobLots","JobLot"}, {"cropYears","CropYear"}, {"warehouses","Warehouse"}, {"docTypes","DocumentType"},
                {"parties","Party"}, {"items","Item"} };
        for (String[] k : keys) {
            List<Map<String, Object>> l = new ArrayList<>();
            for (Map<String, Object> r : all) {
                if (k[1].equals(str(ci(r, "ActivityType")).trim())) {
                    Map<String, Object> o = new LinkedHashMap<>();
                    o.put("Id", ci(r, "Id"));
                    o.put("Name", ci(r, "name"));
                    l.add(o);
                }
            }
            out.put(k[0], l);
        }
        return out;
    }

    /** PendingInventoryTransactionsForIssuanceLoad:319 - InventoryStockEvalautionDetail.GetAvailableTransactionsForIssuance. */
    public List<Map<String, Object>> loaderRows(String from, String to, int parentCategoryId, int itemCategoryId, int itemTypeId,
                                                int jobLotId, String cropYear, int warehouseId, int refDocumentTypeId,
                                                int supplierCustomerId, int itemId) {
        UserAccount u = s.user();
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        if (s.branchId(u) != 0) p.put("@BranchesId", s.branchId(u));
        p.put("@DateFrom", ts(from) == null ? now() : ts(from));
        p.put("@DateTo", ts(to) == null ? now() : ts(to));
        if (supplierCustomerId != 0) p.put("@SupplierCustomerId", supplierCustomerId);
        if (warehouseId != 0) p.put("@WarehouseId", warehouseId);
        if (itemId != 0) p.put("@ItemId", itemId);
        if (refDocumentTypeId != 0) p.put("@ReferenceDocumentTypeId", refDocumentTypeId);
        if (jobLotId != 0) p.put("@JobLotId", jobLotId);
        if (parentCategoryId != 0) p.put("@InventoryParentCategories", parentCategoryId);
        if (itemCategoryId != 0) p.put("@ItemCategoryId", itemCategoryId);
        if (itemTypeId != 0) p.put("@ItemTypeId", itemTypeId);
        p.put("@CropYear", isBlank(cropYear) ? null : cropYear);
        return s.query("dbo.SpInventoryTransactionEvaluation_GetAvailableTransactionsForIssuance", p);
    }

    // ============================================================================== helpers

    private List<Map<String, Object>> q(String label, String sql, Object... args) {
        try {
            return jdbc.queryForList(sql, args);
        } catch (Exception e) {
            LOG.warn("Steel Job Order lookup '{}' failed: {}", label, sql, e);
            return new ArrayList<>();
        }
    }

    private static Set<String> set(String... a) { return Collections.unmodifiableSet(new LinkedHashSet<>(Arrays.asList(a))); }
}
