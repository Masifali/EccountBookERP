package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.ProductionInputRepository;
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
import static com.mst.services.SteelProductionSupport.now;
import static com.mst.services.SteelProductionSupport.params;
import static com.mst.services.SteelProductionSupport.str;
import static com.mst.services.SteelProductionSupport.toBool;
import static com.mst.services.SteelProductionSupport.toDouble;
import static com.mst.services.SteelProductionSupport.toInt;
import static com.mst.services.SteelProductionSupport.ts;

/**
 * Screen 601 "Food Production With Values New" (Production Steel, module 107) —
 * Architecture.WinApp.Steel.Production/FoodProductionWithValuesNew_St.cs.
 *
 * Five tabs, three documents:
 *   Input      DocumentTypeId 1512  InvFoodProduction.Save (BLL 0234) -> MakeVoucher + DAL 0208 SetData
 *   OutPut     DocumentTypeId 1513  same BLL, EntryTypeId per row (1 ManualRate / 2 AutoRate), Issuance = 3 on Input
 *   Over Head  DocumentTypeId 1514  InvFoodProductionOverHeads.Save (BLL 0235) -> DAL 0209 (row insert + voucher per row)
 *   Transaction History / Summery / Settlement read [ST].[USP_FoodProduction_GetAllMethod], [ST].[USP_FoodProduction_Summery_WithValues],
 *   [ST].[USP_FoodProduction_GetSummeryValues], [ST].[USP_FoodProductionSettlement]; Settlement Update = [ST].[USP_InvFoodProductionDetail_Update]
 *   (DAL 0208 SetDataForUpdate), Approve = [ST].[USP_ProductionJobOrder_GetAllMethod] @Activity=JobOrderApprove.
 *
 * Voucher procedures (Sp_VoucherHead_Insert/Update, Sp_VoucherDetail_Insert, Sp_InventoryStockEvalautionDetail_Update,
 * Sp_InventoryTransactions_GetALLMethod, USP_VoucherBalanceCheck) are the dbo ones shared with screen 280; their
 * parameter sets and CLR defaults come from ProductionInputRepository, the verified mirrors of the desktop models.
 */
@Service
public class SteelFoodProductionService {

    private static final Logger LOG = LoggerFactory.getLogger(SteelFoodProductionService.class);

    public static final String SCREEN = "FoodProductionWithValuesNew_St";
    public static final int DOC_INPUT = 1512, DOC_OUTPUT = 1513, DOC_OVERHEAD = 1514, DOC_JOB_ORDER = 1511, DOC_SETTLEMENT_VOUCHER = 178;
    private static final String FP = "[ST].[USP_FoodProduction_GetAllMethod]";
    private static final String OH = "[ST].[USP_FoodProductionOverHeads_GetAllMethod]";
    private static final String JO = "[ST].[USP_ProductionJobOrder_GetAllMethod]";

    private static final String[][] ENTRY_TYPES = { {"1","ManualRate"}, {"2","AutoRate"} };   // EntryTypeFill:1024

    static final Set<String> P_HEADER = set("Id","DocumentTypeId","DocDate","DocCode","ProductionJobOrderId","ProductionJobOrderNo",
            "WIPAccountId","WIPItemId","MainRemarks","PlantId","ApprovedUserId","ApprovedDate","IsApproved","EntryDate","EntryUserId",
            "ModifyDate","ModifyUserId","OrganizationId","CompanyId","BranchesId","ProjectsId","FinancialYearId","ActionId","PendingForView");
    static final Set<String> P_DETAIL = set("Id","RefDocumentTypeId","RefDocNoId","RefDocSubIdNo","FoodProductionId","ProductionJobOrderId",
            "JobOrderOutPutId","JobOrderInPutId","EntryTypeId","WarehouseId","ItemId","PackUomId","JobLotId","PackingtypeId","Qty","PackUnit",
            "Weight","Rate","RateUOMId","Amount","Remarks","ItemOhCost","GeneralOhCost","NetRate","TotalAmount","ActionTypeId","DeleteFlag");
    static final Set<String> P_OVERHEAD = set("Id","DocumentTypeId","DocDate","DocNo","ProductionJobOrderId","JobOrderOverHeadId","PlantId",
            "CharOfAccountId","ChargesTypeId","BrandId","BrandUomId","RateUomId","ItemQty","ItemRate","Amount","RemarksOH","ApprovedUserId",
            "ApprovedDate","IsApproved","EntryDate","EntryUserId","ModifyDate","ModifyUserId","OrganizationId","CompanyId","BranchesId",
            "ProjectsId","FinancialYearId","ActionId");

    private final SteelProductionSupport s;
    private final JdbcTemplate jdbc;

    public SteelFoodProductionService(SteelProductionSupport support, JdbcTemplate jdbc) {
        this.s = support;
        this.jdbc = jdbc;
    }

    // ================================================================================= load

    /** frmFoodProduction_Load:726 */
    public Map<String, Object> lookups() {
        UserAccount u = s.user();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("canSave", s.hasRight(SCREEN, "Save"));
        out.put("canUpdate", s.hasRight(SCREEN, "Update"));
        out.put("canPrint", s.hasRight(SCREEN, "Print"));
        out.put("canViewAllRecords", s.hasRight(SCREEN, "CanView AllRecord"));
        out.put("canRateAndAmount", s.hasRight(SCREEN, "Rate"));                   // DoHaveCanRateandAmount :753
        out.put("canSettlementTab", s.hasRight(SCREEN, "ProductionSettlement"));    // DoHaveCanSettlementtab :759 - removes Settlement AND Over Head
        out.put("branchesId", s.branchId(u));
        out.put("rateDecimals", s.rateDecimals(u));
        out.put("amountDecimals", s.amountDecimals(u));
        out.put("entryTypes", SteelProductionSupport.literal(ENTRY_TYPES));
        out.put("differenceAccounts", q("stockAccoutnFill",
                "EXEC dbo.Sp_COAAllocation_GetAllMethod @OrganizationId=?, @CompanyId=?, @AccountTypeIds=?, @AppId=?, @UserId=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), "4,12", s.ctx().currentAppId(), u.getId(), "GetAccountTitleByAccountTypeIds"));  // :877
        out.put("jobLots", s.jobLots(u));                                                  // combojoblotfill:890
        out.put("warehouses", s.allWarehouses(u));                                         // gridsettingsOutPut:2460 WareHouseGetAllService
        out.put("activeWarehouses", s.activeWarehouses(u));                                // Warehouse:860
        out.put("packingTypes", s.packingTypes());                                         // bagType:907
        out.put("jobOrders", jobOrders(u));                                                // JobOrderNoFill:926
        out.put("summaryJobOrders", summaryJobOrders(u));                                  // JobOrderNoFillForSummery:971
        out.put("uomList", q("UOMFill", "EXEC dbo.Sp_UOMSchedule_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), "ReadByOrganizationCompanyId"));    // UOMFill:1007
        out.put("docNoInput", generateCode(u, DOC_INPUT));                                 // GenerateDocNumberInput:1318
        out.put("docNoOutput", generateCode(u, DOC_OUTPUT));                               // GenerateDocNumberOutPut:2276
        out.put("docNoOverhead", generateCodeOverhead(u));                                 // GenerateCodeofOverHead:3237
        return out;
    }

    /** JobOrderNoFill:926 - InvFoodProduction.GetJobOrderNoForInvFoodProduction (DocumentTypeId 1511). */
    public List<Map<String, Object>> jobOrders(UserAccount u) {
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@DocumentTypeId", DOC_JOB_ORDER);
        p.put("@FinancialYearId", s.ctx().currentFinancialYearId());
        p.put("@Activity", "GetJobOrderNoForFoodProduction");
        return SteelProductionSupport.idName(s.query(FP, p), "Id", "ProductionJobOrderNo");
    }

    /** JobOrderNoFillForSummery:971 - InvProductionJobOrder.GetJobOrderAll (GetAllJobOrderNo). */
    public List<Map<String, Object>> summaryJobOrders(UserAccount u) {
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        if (s.branchId(u) != 0) p.put("@BranchesId", s.branchId(u));
        int fy = s.ctx().currentFinancialYearId();
        if (fy != 0) p.put("@FinancialYearId", fy);
        p.put("@Activity", "GetAllJobOrderNo");
        return SteelProductionSupport.idName(s.query(JO, p), "Id", "ProductionJobOrderNo");
    }

    /** InvFoodProduction.GenerateCode (Steel BLL 0234) - [ST].[USP_FoodProduction_GetAllMethod] GenerateCode -> DocCode. */
    public int generateCode(UserAccount u, int documentTypeId) {
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@BranchesId", s.branchId(u));
        p.put("@DocumentTypeId", documentTypeId);
        p.put("@FinancialYearId", s.ctx().currentFinancialYearId());
        p.put("@Activity", "GenerateCode");
        List<Map<String, Object>> rows = s.query(FP, p);
        return rows.isEmpty() ? 0 : toInt(ci(rows.get(0), "DocCode"));
    }

    /** InvFoodProduction.GenerateCodeOverHeaad (Steel BLL 0234:404) - [ST].[USP_FoodProductionOverHeads_GetAllMethod] GenerateCode (1514). */
    public int generateCodeOverhead(UserAccount u) {
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@BranchesId", s.branchId(u));
        p.put("@DocumentTypeId", DOC_OVERHEAD);
        p.put("@FinancialYearId", s.ctx().currentFinancialYearId());
        p.put("@Activity", "GenerateCode");
        List<Map<String, Object>> rows = s.query(OH, p);
        if (rows.isEmpty()) return 0;
        Object v = ci(rows.get(0), "DocCode");
        if (v == null) v = ci(rows.get(0), "DocNo");
        if (v == null && !rows.get(0).isEmpty()) v = rows.get(0).values().iterator().next();
        return toInt(v);
    }

    public Map<String, Object> nextCodes() {
        UserAccount u = s.user();
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("docNoInput", generateCode(u, DOC_INPUT));
        m.put("docNoOutput", generateCode(u, DOC_OUTPUT));
        m.put("docNoOverhead", generateCodeOverhead(u));
        m.put("jobOrders", jobOrders(u));
        m.put("summaryJobOrders", summaryJobOrders(u));
        return m;
    }

    /**
     * CmbJobOrderNo_Leave:1348 / CmbJobOrderNoOutput_Leave:2306 / cmbJobOrderOverHead_Leave:3267 -
     * GetGlAccountsByJobOrderId, ReadPlantDetailByJobOrderId and GetInPutTotalQtyandWeightByJobOrderId.
     */
    public Map<String, Object> jobOrderInfo(int jobOrderId) {
        UserAccount u = s.user();
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> gl = s.glAccountsByJobOrder(u, jobOrderId);
        List<Map<String, Object>> accounts = new ArrayList<>(), items = new ArrayList<>();
        double finishGoodRate = 0;
        for (Map<String, Object> r : gl) {
            Map<String, Object> a = new LinkedHashMap<>();
            a.put("Id", ci(r, "WorkInProccessAcId")); a.put("Name", ci(r, "WorkInProcessAc")); accounts.add(a);
            Map<String, Object> i = new LinkedHashMap<>();
            i.put("Id", ci(r, "WipItemId")); i.put("Name", ci(r, "ItemName")); items.add(i);
        }
        if (!gl.isEmpty()) finishGoodRate = toDouble(ci(gl.get(0), "FinishGoodRate"));
        out.put("wipAccounts", accounts);
        out.put("wipItems", items);
        out.put("finishGoodRate", finishGoodRate);
        out.put("plants", SteelProductionSupport.idName(s.plantsByJobOrder(jobOrderId), "PlantId", "PlantName"));

        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@ProductionJobOrderId", jobOrderId);
        p.put("@Activity", "GetInPutTotalQtyandWeightByJobOrderId");
        List<Map<String, Object>> totals = new ArrayList<>();
        for (Map<String, Object> r : s.query(FP, p)) {
            Map<String, Object> t = new LinkedHashMap<>();
            t.put("EntryTypeId", toInt(ci(r, "EntryTypeId")));
            t.put("ItemQty", toDouble(ci(r, "ItemQty")));
            t.put("StockWeight", toDouble(ci(r, "StockWeight")));
            totals.add(t);
        }
        out.put("totals", totals);
        return out;
    }

    // =============================================================================== loaders

    /** LoadInputDataByJobOrder:115 - [ST].[USP_LoadInputDataByInvProductionJobOrder] @Id @OrganizationId @CompanyId @ActionId=1. */
    public List<Map<String, Object>> loadInput(int jobOrderId) {
        UserAccount u = s.user();
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@ActionId", 1);
        p.put("@Id", jobOrderId);
        return s.query("[ST].[USP_LoadInputDataByInvProductionJobOrder]", p);
    }

    /** LoadOutPutDataByJobOrder:115 - [ST].[USP_LoadOuPutDataByInvProductionJobOrder]. */
    public List<Map<String, Object>> loadOutput(int jobOrderId) {
        UserAccount u = s.user();
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@ActionId", 1);
        p.put("@CompanyId", u.getCompanyId());
        p.put("@Id", jobOrderId);
        return s.query("[ST].[USP_LoadOuPutDataByInvProductionJobOrder]", p);
    }

    /** LoadOverHeadsDataByJobOrder:125 - [ST].[USP_LoadOverHeadsDataByInvProductionJobOrder] (no ActionId). */
    public List<Map<String, Object>> loadOverheads(int jobOrderId) {
        UserAccount u = s.user();
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@Id", jobOrderId);
        return s.query("[ST].[USP_LoadOverHeadsDataByInvProductionJobOrder]", p);
    }

    // =============================================================================== read

    /** InvFoodProduction.GetByID - ReadById + ReadDetailByFoodProductionId (DAL 0208 GetDate). */
    public Map<String, Object> read(int id) {
        UserAccount u = s.user();
        LinkedHashMap<String, Object> p = params();
        p.put("@Id", id);
        p.put("@Activity", "ReadById");
        List<Map<String, Object>> heads = s.query(FP, p);
        if (heads.isEmpty()) return null;
        Map<String, Object> head = heads.get(0);
        Map<String, Object> h = new LinkedHashMap<>();
        h.put("Id", id);
        h.put("DocumentTypeId", toInt(ci(head, "DocumentTypeId")));
        h.put("DocCode", ci(head, "DocCode"));
        h.put("DocDate", day(ci(head, "DocDate")));
        h.put("MainRemarks", ci(head, "MainRemarks"));
        h.put("ProductionJobOrderId", toInt(ci(head, "ProductionJobOrderId")));
        h.put("ProductionJobOrderNo", ci(head, "ProductionJobOrderNo"));
        h.put("PlantId", toInt(ci(head, "PlantId")));
        h.put("WIPAccountId", toInt(ci(head, "WIPAccountId")));
        h.put("WIPItemId", toInt(ci(head, "WIPItemId")));
        h.put("IsApproved", toBool(ci(head, "IsApproved")));

        LinkedHashMap<String, Object> pd = params();
        pd.put("@Id", id);
        pd.put("@Activity", "ReadDetailByFoodProductionId");
        List<Map<String, Object>> details = new ArrayList<>();
        for (Map<String, Object> r : s.query(FP, pd)) {
            Map<String, Object> d = new LinkedHashMap<>();
            d.put("Id", toInt(ci(r, "Id")));
            d.put("JobOrderInPutId", toInt(ci(r, "JobOrderInPutId")));
            d.put("JobOrderOutPutId", toInt(ci(r, "JobOrderOutPutId")));
            d.put("RefDocumentTypeId", toInt(ci(r, "RefDocumentTypeId")));
            d.put("RefDocNoId", toInt(ci(r, "RefDocNoId")));
            d.put("RefDocSubIdNo", toInt(ci(r, "RefDocSubIdNo")));
            d.put("EntryTypeId", toInt(ci(r, "EntryTypeId")));
            d.put("EntryType", ci(r, "EntryType"));
            d.put("WarehouseId", toInt(ci(r, "WarehouseId")));
            d.put("WareHouseName", ci(r, "WareHouseName"));
            d.put("JobLotId", toInt(ci(r, "JobLotId")));
            d.put("JobLotDescription", ci(r, "JobLotDescription"));
            d.put("JobLotCode", ci(r, "JobLotCode"));
            d.put("ItemId", toInt(ci(r, "ItemId")));
            d.put("ItemName", ci(r, "ItemName"));
            d.put("PackingtypeId", toInt(ci(r, "PackingTypeId")));
            d.put("PackingType", ci(r, "PackingType"));
            d.put("PackUomId", toInt(ci(r, "PackUomId")));
            d.put("PackUom", ci(r, "PackUom"));
            d.put("PackUomEquivalent", toDouble(ci(r, "PackUomEquivalent")));
            d.put("Qty", toDouble(ci(r, "Qty")));
            d.put("Weight", toDouble(ci(r, "Weight")));
            d.put("Rate", toDouble(ci(r, "Rate")));
            d.put("RateUOMId", toInt(ci(r, "RateUOMId")));
            d.put("RateUOM", ci(r, "RateUOM"));
            d.put("RateUomEquivalent", toDouble(ci(r, "RateUomEquivalent")));
            d.put("Amount", toDouble(ci(r, "Amount")));
            d.put("NetRate", toDouble(ci(r, "NetRate")));
            d.put("TotalAmount", toDouble(ci(r, "TotalAmount")));
            d.put("ItemOhCost", toDouble(ci(r, "ItemOhCost")));
            d.put("GeneralOhCost", toDouble(ci(r, "GeneralOhCost")));
            d.put("Remarks", ci(r, "Remarks"));
            details.add(d);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("header", h);
        out.put("details", details);
        out.put("voucherHeadId", s.voucherHeadId(u, toInt(ci(head, "DocumentTypeId")), id));   // GetVoucherHeadId:1121
        return out;
    }

    public int voucherHeadId(int id, int documentTypeId) {
        return s.voucherHeadId(s.user(), documentTypeId, id);
    }

    // ============================================================================== history

    /** InputGridHistory:1968 (1512) / OutputGridHistory:2909 (1513) / GridHistory:4203 (no type) - FormHistory. */
    public Map<String, Object> history(int documentTypeId, int noOfRecords) {
        UserAccount u = s.user();
        boolean canViewAll = s.hasRight(SCREEN, "CanView AllRecord");
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@BranchesId", s.branchId(u));
        if (documentTypeId != 0) p.put("@DocumentTypeId", documentTypeId);
        p.put("@FinancialYearId", s.ctx().currentFinancialYearId());
        if (noOfRecords != 0) p.put("@NoOfRecords", noOfRecords);
        p.put("@CanViewAllRecord", canViewAll);
        if (!canViewAll) p.put("@EntryUserId", u.getId());
        p.put("@Activity", "FormHistory");
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> r : s.query(FP, p)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("RecordNo", ci(r, "RecordNo"));
            o.put("Id", toInt(ci(r, "Id")));
            o.put("DocumentTypeId", toInt(ci(r, "DocumentTypeId")));
            o.put("EntryType", ci(r, "EntryType"));
            o.put("DocDate", day(ci(r, "DocDate")));
            o.put("DocCode", ci(r, "DocCode"));
            o.put("ProductionJobOrderId", toInt(ci(r, "ProductionJobOrderId")));
            o.put("ProductionJobOrderNo", ci(r, "ProductionJobOrderNo"));
            o.put("PlantId", toInt(ci(r, "PlantId")));
            o.put("WorkInProcessAccountTitle", ci(r, "WorkInProcessAccountTitle"));
            o.put("WipItemName", ci(r, "WipItemName"));
            o.put("EntryUserName", ci(r, "EntryUserName"));
            o.put("EntryDate", str(ci(r, "EntryDate")));
            o.put("ModifyUserName", ci(r, "ModifyUserName"));
            o.put("ModifyDate", str(ci(r, "ModifyDate")));
            o.put("IsApproved", toBool(ci(r, "IsApproved")));
            o.put("MainRemarks", ci(r, "MainRemarks"));
            rows.add(o);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("canViewAllRecords", canViewAll);
        out.put("rows", rows);
        return out;
    }

    // ================================================================================ save (Input / OutPut)

    /**
     * InsertInput:1414 (1512) and OutPutInsert:2731 (1513) -> InvFoodProduction.Save (Steel BLL 0234:275):
     * MakeVoucher, then DAL 0208 SetData: header, details ([ST].[USP_InvFoodProductionDetail_Insert]),
     * Sp_InventoryStockEvalautionDetail_Update, Sp_InventoryTransactions_GetALLMethod, the voucher
     * (existing head looked up on update) and USP_VoucherBalanceCheck - in one transaction.
     */
    @Transactional
    public Map<String, Object> save(Map<String, Object> body) {
        UserAccount u = s.user();
        int documentTypeId = toInt(body.get("DocumentTypeId"));
        if (documentTypeId != DOC_INPUT && documentTypeId != DOC_OUTPUT) throw new IllegalArgumentException("DocumentTypeId must be 1512 (Input) or 1513 (OutPut)");
        boolean input = documentTypeId == DOC_INPUT;
        int recId = toInt(body.get("Id"));
        boolean insert = recId == 0;
        if (insert && !s.hasRight(SCREEN, "Save")) throw new IllegalStateException("You do not have the Save right for this screen.");
        if (!insert && !s.hasRight(SCREEN, "Update")) throw new IllegalStateException("You do not have the Update right for this screen.");

        /* FormValidation:1379 / FormValidationOutPut:2241 */
        String docNo = str(body.get("DocCode")).trim();
        if (docNo.isEmpty() || "0".equals(docNo)) throw new IllegalArgumentException(input ? "document Number Field Required" : "DocNo Field Required");
        if (toInt(body.get("ProductionJobOrderId")) == 0) throw new IllegalArgumentException("Job Order Number Field Required");
        if (toInt(body.get("PlantId")) == 0) throw new IllegalArgumentException(input ? "Plant Name Field Required" : "Plant Name Field Is Required");
        if (toInt(body.get("WIPAccountId")) == 0) throw new IllegalArgumentException("WorkInProcess Field Required");
        if (toInt(body.get("WIPItemId")) == 0) throw new IllegalArgumentException("WIP Item Field Required");

        List<Map<String, Object>> rows = list(body.get("details"));
        if (rows.isEmpty()) throw new IllegalArgumentException("Enter Detail First ...");
        /* OutPutInsert:2758 - a NEW output takes a fresh number at save time */
        int docCode = (!input && insert) ? generateCode(u, DOC_OUTPUT) : toInt(docNo);
        if (docCode == 0) docCode = toInt(docNo);

        Map<String, Object> h = new LinkedHashMap<>();
        h.put("Id", recId);
        h.put("DocumentTypeId", documentTypeId);
        h.put("DocCode", docCode);
        h.put("DocDate", ts(body.get("DocDate")) == null ? now() : ts(body.get("DocDate")));
        h.put("MainRemarks", str(body.get("MainRemarks")));
        h.put("WIPAccountId", toInt(body.get("WIPAccountId")));
        h.put("WIPItemId", toInt(body.get("WIPItemId")));
        h.put("ProductionJobOrderId", toInt(body.get("ProductionJobOrderId")));
        h.put("ProductionJobOrderNo", str(body.get("ProductionJobOrderNo")).trim());
        h.put("PlantId", toInt(body.get("PlantId")));
        h.put("OrganizationId", u.getOrganizationId());
        h.put("CompanyId", u.getCompanyId());
        h.put("BranchesId", s.branchId(u));
        h.put("EntryDate", now());
        h.put("ModifyDate", now());
        h.put("ApprovedDate", now());
        h.put("ApprovedUserId", 0);
        h.put("IsApproved", false);
        h.put("EntryUserId", insert ? u.getId() : 0);        // BLL Save: update -> EntryUserId = 0
        h.put("ModifyUserId", insert ? 0 : u.getId());       //            insert -> ModifyUserId = 0
        h.put("FinancialYearId", s.ctx().currentFinancialYearId());
        h.put("ProjectsId", 0);
        h.put("ActionId", insert ? 1 : 2);
        h.put("PendingForView", null);

        List<Map<String, Object>> details = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            boolean removed = toInt(r.get("ActionTypeId")) == 3;
            double qty = toDouble(r.get(input ? "ItemQty" : "ItemQty")), weight = toDouble(r.get("NetWeight"));
            if (!removed && !(qty > 0 && weight > 0)) continue;   // :1464 / :2786 - rows without qty AND weight are skipped
            Map<String, Object> d = new LinkedHashMap<>();
            d.put("Id", toInt(r.get("Id")));
            d.put("ActionTypeId", removed ? 3 : (toInt(r.get("Id")) == 0 ? 1 : 2));
            if (input) {
                d.put("EntryTypeId", 3);                                        // :1469 Issuance
                d.put("JobOrderInPutId", toInt(r.get("JobOrderInPutId")));
                d.put("JobOrderOutPutId", 0);
                d.put("RefDocNoId", toInt(r.get("RefDocIdNo")));
                d.put("RefDocumentTypeId", toInt(r.get("RefDocumentTypeId")));
                d.put("RefDocSubIdNo", toInt(r.get("RefDocSubIdNo")));
                if (!removed && toInt(r.get("WareHouseId")) == 0) throw new IllegalArgumentException("WareHouse Field Require in Input Detail");
                d.put("WarehouseId", toInt(r.get("WareHouseId")));
                if (!removed && toInt(r.get("ItemId")) == 0) throw new IllegalArgumentException("Item Field Require in Input Detail");
                d.put("ItemId", toInt(r.get("ItemId")));
                if (!removed && str(r.get("JobLotId")).isEmpty()) throw new IllegalArgumentException("JobLot Field Require in Input Detail");
                d.put("JobLotId", toInt(r.get("JobLotId")));
                d.put("PackingtypeId", toInt(r.get("PackingTypeId")));
                d.put("PackUnit", toInt(r.get("PackUom")));                     // :1493 Conversion.ToInt(PackUom text) -> 0 for a code
                d.put("PackUomId", toInt(r.get("PackUomId")));
                d.put("Qty", qty);
                if (!removed && qty == 0) throw new IllegalArgumentException("ItemQty Field Required");
                d.put("Weight", weight);
                if (!removed && weight == 0) throw new IllegalArgumentException("ItemWeight Field Required");
                d.put("Rate", toDouble(r.get("ItemRate")));
                d.put("NetRate", toDouble(r.get("ItemRate")));
                d.put("Amount", toDouble(r.get("ItemAmount")));
                d.put("TotalAmount", toDouble(r.get("ItemAmount")));
                d.put("ItemOhCost", 0d);
                d.put("GeneralOhCost", 0d);
            } else {
                if (!removed && toInt(r.get("EntryTypeId")) == 0) throw new IllegalArgumentException("EntryType Field Require in Output Detail");
                d.put("EntryTypeId", toInt(r.get("EntryTypeId")));
                d.put("JobOrderOutPutId", toInt(r.get("OutPutIdFromJobOrder")));
                d.put("JobOrderInPutId", 0);
                d.put("RefDocNoId", 0);
                d.put("RefDocumentTypeId", 0);
                d.put("RefDocSubIdNo", 0);
                if (!removed && toInt(r.get("WareHouseId")) == 0) throw new IllegalArgumentException("WareHouse Field Require in Output Detail");
                d.put("WarehouseId", toInt(r.get("WareHouseId")));
                if (!removed && toInt(r.get("ItemId")) == 0) throw new IllegalArgumentException("Item Field Require in Output Detail");
                d.put("ItemId", toInt(r.get("ItemId")));
                d.put("PackUomId", toInt(r.get("PackUomId")));
                if (!removed && toInt(r.get("JobLotId")) == 0) throw new IllegalArgumentException("JobLot Field Require in Output Detail");
                d.put("JobLotId", toInt(r.get("JobLotId")));
                d.put("PackingtypeId", toInt(r.get("PackingTypeId")));
                d.put("PackUnit", 0);
                d.put("Qty", qty);
                d.put("Weight", weight);
                d.put("Rate", toDouble(r.get("ItemRate")));
                d.put("RateUOMId", toInt(r.get("RateUomId")));
                d.put("Amount", toDouble(r.get("ItemAmount")));
                d.put("NetRate", toDouble(r.get("NetRate")));
                d.put("TotalAmount", toDouble(r.get("NetAmount")));
                d.put("ItemOhCost", 0d);
                d.put("GeneralOhCost", 0d);
            }
            if (input) d.put("RateUOMId", toInt(r.get("RateUomId")));
            d.put("ProductionJobOrderId", toInt(body.get("ProductionJobOrderId")));
            d.put("Remarks", str(r.get("Remarks")));
            d.put("DeleteFlag", null);
            details.add(d);
        }
        if (details.isEmpty()) throw new IllegalArgumentException(input ? "Grid Record Not Found" : "grid Record not found please check!");

        /* ---- MakeVoucher (BLL 0234:162) ---- */
        Map<String, Object> voucher = makeVoucher(u, h, details);

        /* ---- DAL 0208 SetData ---- */
        int org = u.getOrganizationId(), comp = u.getCompanyId();
        int result = s.setProc(insert ? "[ST].[USP_InvFoodProduction_Insert]" : "[ST].[USP_InvFoodProduction_Update]", h, P_HEADER);
        int id = result > 0 ? result : recId;
        for (Map<String, Object> d : details) {
            d.put("FoodProductionId", id);
            int did = s.setProc("[ST].[USP_InvFoodProductionDetail_Insert]", d, P_DETAIL);
            if (did > 0) d.put("Id", did);
        }
        Map<String, Object> se = ProductionInputRepository.stockEvaluationDefaults();
        se.put("OrganizationId", org); se.put("CompanyId", comp); se.put("RefDocumentTypeId", documentTypeId); se.put("RefDocIdNo", id);
        s.setProc("dbo.Sp_InventoryStockEvalautionDetail_Update", se, ProductionInputRepository.P_STOCK_EVALUATION_UPDATE);
        Map<String, Object> it = ProductionInputRepository.inventoryTransactionsDefaults();
        it.put("OrganizationId", org); it.put("CompanyId", comp); it.put("RefDocumentTypeId", documentTypeId); it.put("RefDocIdNo", id);
        s.setProc("dbo.Sp_InventoryTransactions_GetALLMethod", it, ProductionInputRepository.P_INV_TRANSACTIONS);

        int existing = insert ? 0 : s.voucherHeadId(u, documentTypeId, id);
        voucher.put("Id", existing);
        voucher.put("DocumentTypeSrNo", id);
        int vr = s.setProc(existing == 0 ? "dbo.Sp_VoucherHead_Insert" : "dbo.Sp_VoucherHead_Update", voucher, ProductionInputRepository.P_VOUCHER_HEAD);
        int voucherId = vr > 0 ? vr : existing;
        List<Map<String, Object>> lines = list(voucher.get("lines"));
        if (!lines.isEmpty()) {
            for (Map<String, Object> l : lines) {
                l.put("VoucherHeadId", voucherId);
                /* RefDocSubIdNo = the detail's Id (known only after its insert) */
                Object ref = l.get("_detailIndex");
                if (ref != null && toInt(ref) >= 0 && toInt(ref) < details.size()) l.put("RefDocSubIdNo", details.get(toInt(ref)).get("Id"));
                s.setProc("dbo.Sp_VoucherDetail_Insert", l, ProductionInputRepository.P_VOUCHER_DETAIL);
            }
            if (voucherId > 0) {
                LinkedHashMap<String, Object> p = params();
                p.put("@OrganizationId", org); p.put("@CompanyId", comp); p.put("@Id", voucherId);
                s.scalar("dbo.USP_VoucherBalanceCheck", p);
            }
        }
        Map<String, Object> res = SteelProductionSupport.ok((insert ? "Record Save Successfully" : "Record Update Successfully") + docCode);  // :1530 / :1534
        res.put("id", id);
        res.put("docCode", docCode);
        return res;
    }

    /** CommonServices.GetItemGlIdsandItemName - Sp_Item_GetAllMethod @Activity=GetItemGlIdsandItemName (Id, ItemName, PurchaseGLAC). */
    private List<Map<String, Object>> itemGlAccounts(UserAccount u) {
        return jdbc.queryForList("EXEC dbo.Sp_Item_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), "GetItemGlIdsandItemName");
    }

    /** BLL 0234 MakeVoucher:162 - two lines per live detail; Issuance (3) debits WIP, output credits WIP. */
    private Map<String, Object> makeVoucher(UserAccount u, Map<String, Object> h, List<Map<String, Object>> details) {
        List<Map<String, Object>> gl = itemGlAccounts(u);
        Map<String, Object> vh = ProductionInputRepository.voucherHeadDefaults();
        vh.put("DocumentTypeSrNo", h.get("Id"));
        vh.put("VoucherDate", h.get("DocDate"));
        vh.put("VoucherCode", h.get("DocCode"));
        vh.put("DocumentTypeId", h.get("DocumentTypeId"));
        vh.put("RefAccountId", h.get("WIPAccountId"));
        vh.put("AgainstAccountId", h.get("WIPItemId"));
        vh.put("EntryDate", now());
        vh.put("ModifyDate", now());
        vh.put("EntryUser", u.getId());
        vh.put("ModifyUser", u.getId());
        vh.put("OrganizationId", u.getOrganizationId());
        vh.put("CompanyId", u.getCompanyId());
        vh.put("FinancialYearId", h.get("FinancialYearId"));
        double total = 0;
        List<Map<String, Object>> lines = new ArrayList<>();
        int wip = toInt(h.get("WIPAccountId"));
        for (int i = 0; i < details.size(); i++) {
            Map<String, Object> d = details.get(i);
            if (toInt(d.get("ActionTypeId")) == 3) continue;
            if (gl.isEmpty()) throw new IllegalArgumentException("Item GL Account Not Found");
            Map<String, Object> g = null;
            for (Map<String, Object> x : gl) if (toInt(ci(x, "Id")) == toInt(d.get("ItemId"))) { g = x; break; }
            if (g == null) throw new IllegalArgumentException("Item GL Account Not Found");
            int purchaseGl = toInt(ci(g, "PurchaseGLAC"));
            String remarks = str(d.get("Remarks"));
            String text = (!remarks.isEmpty() ? remarks + "   ItemName: " : "ItemName: ") + str(ci(g, "ItemName"))
                    + "  Qty  " + fmt(d.get("Qty")) + "   Weight  " + fmt(d.get("Weight")) + "  Rate  " + fmt(d.get("Rate"));
            double amount = toDouble(d.get("Amount")), qty = toDouble(d.get("Qty")), weight = toDouble(d.get("Weight"));
            boolean issuance = toInt(d.get("EntryTypeId")) == 3;
            int orderNo = issuance ? toInt(d.get("JobOrderInPutId")) : toInt(d.get("JobOrderOutPutId"));
            Map<String, Object> a = ProductionInputRepository.voucherDetailDefaults(), b = ProductionInputRepository.voucherDetailDefaults();
            if (issuance) {
                a.put("AccountId", wip); a.put("AgainstAccountId", purchaseGl); a.put("Comments", text); a.put("DebitAmount", amount);
                a.put("QtyIn", qty); a.put("WeightIn", weight); a.put("ItemAmount", amount); a.put("OrderNo", orderNo);
                total += amount;
                b.put("AccountId", purchaseGl); b.put("AgainstAccountId", wip); b.put("Comments", text); b.put("CreditAmount", amount);
                b.put("QtyOut", qty); b.put("WeightOut", weight); b.put("ItemId", toInt(d.get("ItemId"))); b.put("ItemAmount", amount); b.put("OrderNo", orderNo);
            } else {
                a.put("AccountId", purchaseGl); a.put("AgainstAccountId", wip); a.put("Comments", text); a.put("DebitAmount", amount);
                a.put("QtyIn", qty); a.put("WeightIn", weight); a.put("ItemAmount", amount); a.put("ItemId", toInt(d.get("ItemId"))); a.put("OrderNo", orderNo);
                b.put("AccountId", wip); b.put("AgainstAccountId", purchaseGl); b.put("Comments", text); b.put("CreditAmount", amount);
                b.put("QtyIn", qty); b.put("WeightIn", weight); b.put("ItemAmount", amount); b.put("OrderNo", orderNo);
                total += toDouble(b.get("DebitAmount"));    // the desktop sums voucherDetail4.DebitAmount, which is 0
            }
            a.put("_detailIndex", i); b.put("_detailIndex", i);
            lines.add(a); lines.add(b);
        }
        vh.put("VoucherAmount", total);
        vh.put("lines", lines);
        return vh;
    }

    private static String fmt(Object v) {
        double d = toDouble(v);
        return d == Math.rint(d) ? String.valueOf((long) d) : String.valueOf(d);
    }

    // ============================================================================== Over Head

    /** BindGridHistoryOverHead:3792 - FormHistory (1514) grouped per job order, as the LINQ in the form groups it. */
    public List<Map<String, Object>> overheadHistory(int noOfRecords) {
        UserAccount u = s.user();
        boolean canViewAll = s.hasRight(SCREEN, "CanView AllRecord");
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@BranchesId", s.branchId(u));
        p.put("@DocumentTypeId", DOC_OVERHEAD);
        p.put("@FinancialYearId", s.ctx().currentFinancialYearId());
        if (noOfRecords != 0) p.put("@NoOfRecords", noOfRecords);
        p.put("@CanViewAllRecord", canViewAll);
        if (!canViewAll) p.put("@EntryUserId", u.getId());
        p.put("@Activity", "FormHistory");
        LinkedHashMap<Integer, Map<String, Object>> groups = new LinkedHashMap<>();
        LinkedHashMap<Integer, Integer> counts = new LinkedHashMap<>();
        for (Map<String, Object> r : s.query(OH, p)) {
            int key = toInt(ci(r, "ProductionJobOrderId"));
            Map<String, Object> g = groups.get(key);
            if (g == null) {
                g = new LinkedHashMap<>();
                g.put("ProductionJobOrderId", key);
                g.put("ProductionJobOrder", ci(r, "ProductionJobOrderNo"));
                g.put("TotalItemQty", 0d); g.put("TotalExpense", 0d); g.put("AverageItemRate", 0d);
                g.put("IsApproved", str(ci(r, "JobOrderStatus")));
                groups.put(key, g); counts.put(key, 0);
            }
            g.put("TotalItemQty", toDouble(g.get("TotalItemQty")) + toDouble(ci(r, "ItemQty")));
            g.put("TotalExpense", toDouble(g.get("TotalExpense")) + toDouble(ci(r, "Amount")));
            g.put("AverageItemRate", toDouble(g.get("AverageItemRate")) + toDouble(ci(r, "ItemRate")));
            counts.put(key, counts.get(key) + 1);
        }
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map.Entry<Integer, Map<String, Object>> e : groups.entrySet()) {
            Map<String, Object> g = e.getValue();
            int n = counts.get(e.getKey());
            g.put("AverageItemRate", n == 0 ? 0 : toDouble(g.get("AverageItemRate")) / n);
            out.add(g);
        }
        return out;
    }

    /** InvFoodProductionOverHeads.GetByJobOrderID (BLL 0235:160) - [ST].[USP_FoodProduction_GetAllMethod] ReadOverHeadsDetailByFoodProductionId. */
    public List<Map<String, Object>> overheadsByJobOrder(int jobOrderId) {
        s.user();
        LinkedHashMap<String, Object> p = params();
        p.put("@Activity", "ReadOverHeadsDetailByFoodProductionId");
        p.put("@Id", jobOrderId);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : s.query(FP, p)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("JobOrderOverHeadId", toInt(ci(r, "JobOrderOverHeadId")));
            o.put("DocDate", day(ci(r, "DocDate")));
            o.put("DocNo", toInt(ci(r, "DocNo")));
            o.put("ProductionJobOrderId", toInt(ci(r, "ProductionJobOrderId")));
            o.put("PlantId", toInt(ci(r, "PlantId")));
            o.put("PlantName", ci(r, "PlantName"));
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

    /**
     * OverHeadInsert:3344 -> InvFoodProductionOverHeads.Save (BLL 0235) -> DAL 0209 SetData: per row
     * ST.USP_InvFoodProductionOverHeads_Insert, then (unless ActionId 3) its voucher: head found by
     * Sp_Vouchers_GetMethods on update, WIP account from GetGlAccountsByJobOrderId, two detail lines, balance check.
     */
    @Transactional
    public Map<String, Object> saveOverheads(Map<String, Object> body) {
        UserAccount u = s.user();
        int recOverHeadId = toInt(body.get("RecOverHeadId"));
        boolean insert = recOverHeadId == 0;
        if (insert && !s.hasRight(SCREEN, "Save")) throw new IllegalStateException("You do not have the Save right for this screen.");
        if (!insert && !s.hasRight(SCREEN, "Update")) throw new IllegalStateException("You do not have the Update right for this screen.");

        /* FormValidationOfOverHead:4020 */
        if (isBlank(str(body.get("DocNo")))) throw new IllegalArgumentException("Doc No. Field Required");
        int jobOrderId = toInt(body.get("ProductionJobOrderId"));
        if (jobOrderId == 0) throw new IllegalArgumentException("Job Order No. Field Required");
        int plantId = toInt(body.get("PlantId"));
        if (plantId == 0) throw new IllegalArgumentException("Plant Field Required");
        int docNo = insert ? generateCodeOverhead(u) : toInt(body.get("DocNo"));     // :3371 GenerateCodeofOverHead on save
        if (docNo == 0) docNo = toInt(body.get("DocNo"));
        String docDate = str(body.get("DocDate"));

        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> r : list(body.get("rows"))) {
            boolean removed = toInt(r.get("ActionTypeId")) == 3 || toInt(r.get("ActionId")) == 3;
            int coa = toInt(r.get("ChartOfAccountId"));
            double amount = toDouble(r.get("Amount"));
            if (!removed && !(coa > 0 && amount > 0)) continue;             // :3387
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
            int id = toInt(r.get("Id"));
            o.put("Id", id);
            o.put("ActionId", removed ? 3 : (id == 0 ? 1 : 2));
            o.put("PlantId", plantId);
            o.put("ProductionJobOrderId", jobOrderId);
            o.put("DocumentTypeId", DOC_OVERHEAD);
            o.put("JobOrderOverHeadId", toInt(r.get("JobOrderOverHeadId")));
            /* :3404 - every row carries its own DocNo/DocDate (0 / now on a loaded row, the saved ones on reload);
               a new row with DocNo 0 takes the generated number */
            int rowDocNo = toInt(r.get("DocNo"));
            o.put("DocNo", rowDocNo == 0 ? docNo : rowDocNo);
            Object rowDate = ts(r.get("DocDate"));
            o.put("DocDate", rowDate == null ? (ts(docDate) == null ? now() : ts(docDate)) : rowDate);
            o.put("CharOfAccountId", coa);
            o.put("ChargesTypeId", 0);
            o.put("BrandId", brandId);
            o.put("BrandUomId", brandUomId);
            o.put("RateUomId", 0);
            o.put("ItemQty", qty);
            o.put("ItemRate", rate);
            o.put("Amount", amount);
            o.put("RemarksOH", str(r.get("Remarks")));
            o.put("ApprovedUserId", 0);
            o.put("ApprovedDate", now());
            o.put("IsApproved", false);
            o.put("EntryDate", now());
            o.put("ModifyDate", now());
            o.put("EntryUserId", (id == 0 && !removed) ? u.getId() : 0);      // DAL 0209: ActionId 1 -> ModifyUserId = 0, else EntryUserId = 0
            o.put("ModifyUserId", (id == 0 && !removed) ? 0 : u.getId());
            o.put("OrganizationId", u.getOrganizationId());
            o.put("CompanyId", u.getCompanyId());
            o.put("BranchesId", s.branchId(u));
            o.put("ProjectsId", 0);
            o.put("FinancialYearId", s.ctx().currentFinancialYearId());
            rows.add(o);
        }
        if (rows.isEmpty()) throw new IllegalArgumentException("Grid Record not found please check!");

        List<Map<String, Object>> gl = s.glAccountsByJobOrder(u, jobOrderId);
        for (Map<String, Object> o : rows) {
            int num = s.setProc("ST.USP_InvFoodProductionOverHeads_Insert", o, P_OVERHEAD);
            if (num > 0) o.put("Id", num); else num = toInt(o.get("Id"));
            if (toInt(o.get("ActionId")) == 3 || num <= 0) continue;
            int existing = toInt(o.get("ModifyUserId")) > 0 ? s.voucherHeadId(u, DOC_OVERHEAD, num) : 0;
            Map<String, Object> vh = ProductionInputRepository.voucherHeadDefaults();
            vh.put("Id", existing);
            List<Map<String, Object>> lines = new ArrayList<>();
            if (!gl.isEmpty()) {
                int wip = toInt(ci(gl.get(0), "WorkInProccessAcId"));
                vh.put("DocumentTypeSrNo", num);
                vh.put("VoucherDate", o.get("DocDate"));
                vh.put("VoucherCode", o.get("DocNo"));
                vh.put("DocumentTypeId", DOC_OVERHEAD);
                vh.put("RefAccountId", o.get("CharOfAccountId"));
                vh.put("AgainstAccountId", wip);
                vh.put("EntryDate", now()); vh.put("ModifyDate", now());
                vh.put("VoucherAmount", toDouble(o.get("Amount")));
                vh.put("EntryUser", o.get("EntryUserId")); vh.put("ModifyUser", o.get("ModifyUserId"));
                vh.put("OrganizationId", u.getOrganizationId()); vh.put("CompanyId", u.getCompanyId());
                vh.put("FinancialYearId", o.get("FinancialYearId"));
                Map<String, Object> a = ProductionInputRepository.voucherDetailDefaults();
                a.put("AccountId", wip); a.put("AgainstAccountId", o.get("CharOfAccountId")); a.put("Comments", str(o.get("RemarksOH")));
                a.put("DebitAmount", toDouble(o.get("Amount"))); a.put("OrderNo", jobOrderId);
                Map<String, Object> b = ProductionInputRepository.voucherDetailDefaults();
                b.put("AccountId", o.get("CharOfAccountId")); b.put("AgainstAccountId", wip); b.put("Comments", str(o.get("RemarksOH")));
                b.put("CreditAmount", toDouble(o.get("Amount"))); b.put("OrderNo", jobOrderId);
                lines.add(a); lines.add(b);
            }
            int vr = s.setProc(existing != 0 ? "dbo.Sp_VoucherHead_Update" : "dbo.Sp_VoucherHead_Insert", vh, ProductionInputRepository.P_VOUCHER_HEAD);
            int voucherId = vr > 0 ? vr : existing;
            for (Map<String, Object> l : lines) {
                l.put("VoucherHeadId", voucherId);
                s.setProc("dbo.Sp_VoucherDetail_Insert", l, ProductionInputRepository.P_VOUCHER_DETAIL);
            }
            if (voucherId > 0) {
                LinkedHashMap<String, Object> p = params();
                p.put("@OrganizationId", u.getOrganizationId()); p.put("@CompanyId", u.getCompanyId()); p.put("@Id", voucherId);
                s.scalar("dbo.USP_VoucherBalanceCheck", p);
            }
        }
        Map<String, Object> res = SteelProductionSupport.ok(insert ? "Save Successfully" : "Update Successfully");   // :3454 / :3458
        res.put("docNo", docNo);
        return res;
    }

    // ============================================================================== Summery

    /** btnShow_Click:4468 - InvFoodProductionRecoverySummeryReport + GetSummeryValues. */
    public Map<String, Object> summary(int jobOrderId, int plantId) {
        UserAccount u = s.user();
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@BranchesId", s.branchId(u));
        p.put("@FinancialYearId", s.ctx().currentFinancialYearId());
        p.put("@ProductionJobOrderId", jobOrderId);
        if (plantId != 0) p.put("@PlantId", plantId);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("report", s.query("[ST].[USP_FoodProduction_Summery_WithValues]", p));
        out.put("values", summaryValues(u, jobOrderId, 0));
        return out;
    }

    /** InvFoodProduction.GetSummeryValues - [ST].[USP_FoodProduction_GetSummeryValues] (@FgNetWeight only when != 0). */
    private List<Map<String, Object>> summaryValues(UserAccount u, int jobOrderId, double netWeight) {
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@BranchesId", s.branchId(u));
        p.put("@FinancialYearId", s.ctx().currentFinancialYearId());
        p.put("@ProductionJobOrderId", jobOrderId);
        if (netWeight != 0) p.put("@FgNetWeight", netWeight);
        return s.query("[ST].[USP_FoodProduction_GetSummeryValues]", p);
    }

    public List<Map<String, Object>> summaryValuesFor(int jobOrderId, double netWeight) {
        return summaryValues(s.user(), jobOrderId, netWeight);
    }

    /** btnProductionRecoveryReport_Click:4736 / btnRecoveryReport_Click:5653 - "Record Not Found For DisPlay" pre-checks. */
    public Map<String, Object> summaryPrintCheck(int jobOrderId, String which) {
        UserAccount u = s.user();
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@BranchesId", s.branchId(u));
        p.put("@FinancialYearId", s.ctx().currentFinancialYearId());
        p.put("@ProductionJobOrderId", jobOrderId);
        List<Map<String, Object>> rows;
        if ("1514".equals(which)) {                       // GenerateOverHeadPrint:4089 - InvFoodProductionOverHeadReportByJobOrder
            rows = s.query("[ST].[USP_FoodProductionOverHeadJobOrderWise_Slip]", p);
        } else if ("1513A".equals(which)) {               // InvFoodProductionRecoverySummeryReport
            rows = s.query("[ST].[USP_FoodProduction_Summery_WithValues]", p);
        } else {                                          // 1513 - InvFoodProduction_Summery_WithOutValue_Rpt
            rows = s.query("[ST].[USP_FoodProductionSummery_WithOutValue]", p);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rows", rows.size());
        if (rows.isEmpty()) out.put("message", "Record Not Found For DisPlay");
        return out;
    }

    /** GenerateSlipInput_Output:1920 - ProductionReports.FoodProductionSlip ([ST].[USP_FoodProduction_Slip]) pre-check. */
    public Map<String, Object> slipPrintCheck(int id, int documentTypeId) {
        UserAccount u = s.user();
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@BranchesId", s.branchId(u));
        p.put("@DocumentTypeId", documentTypeId);
        p.put("@FinancialYearId", s.ctx().currentFinancialYearId());
        p.put("@Id", id);
        List<Map<String, Object>> rows = s.query("[ST].[USP_FoodProduction_Slip]", p);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rows", rows.size());
        if (rows.isEmpty()) out.put("message", "Record Not Found For Display");
        return out;
    }

    // ============================================================================ Settlement

    /** btnGeneralSettlement_Click:4776 - FoodProductionSettlement InPutOutPut + OverHeads, then GetSummeryValues(FG weight). */
    public Map<String, Object> settlement(int jobOrderId) {
        UserAccount u = s.user();
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@ProductionJobOrderId", jobOrderId);
        p.put("@Activity", "InPutOutPut");
        List<Map<String, Object>> io = s.query("[ST].[USP_FoodProductionSettlement]", p);
        LinkedHashMap<String, Object> p2 = params();
        p2.putAll(p); p2.put("@Activity", "OverHeads");
        List<Map<String, Object>> oh = s.query("[ST].[USP_FoodProductionSettlement]", p2);
        double netWeight = 0;
        for (Map<String, Object> r : io) {
            if ("Output".equals(str(ci(r, "EntryType"))) && toInt(ci(r, "EntryTypeId")) == 2) netWeight += toDouble(ci(r, "Weight"));
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rows", io);
        out.put("overheads", oh);
        out.put("values", summaryValues(u, jobOrderId, netWeight));
        out.put("voucherHeadId", s.voucherHeadId(u, DOC_SETTLEMENT_VOUCHER, jobOrderId));   // btnSettlementVoucher_Click:5773
        return out;
    }

    /** UpdateSettlement:5447 -> InvFoodProduction.UpdateSettlement -> DAL 0208 SetDataForUpdate ([ST].[USP_InvFoodProductionDetail_Update]). */
    @Transactional
    public Map<String, Object> updateSettlement(Map<String, Object> body) {
        UserAccount u = s.user();
        if (!s.hasRight(SCREEN, "Update")) throw new IllegalStateException("You do not have the Update right for this screen.");
        int jobOrderId = toInt(body.get("ProductionJobOrderId"));
        List<Map<String, Object>> details = new ArrayList<>();
        for (Map<String, Object> r : list(body.get("rows"))) {
            if (!(toDouble(r.get("Quantity")) > 0 && toDouble(r.get("Weight")) > 0)) continue;
            Map<String, Object> d = new LinkedHashMap<>();
            d.put("Id", toInt(r.get("DetailId")));
            d.put("FoodProductionId", toInt(r.get("Id")));
            d.put("EntryTypeId", toInt(r.get("EntryTypeId")));
            d.put("RefDocNoId", 0);
            if (toInt(r.get("WareHouseId")) == 0) throw new IllegalArgumentException("WareHouse Field Require in Output Detail");
            d.put("WarehouseId", toInt(r.get("WareHouseId")));
            if (toInt(r.get("ItemId")) == 0) throw new IllegalArgumentException("Item Field Require in Output Detail");
            d.put("ItemId", toInt(r.get("ItemId")));
            d.put("PackUomId", toInt(r.get("PackUomId")));
            if (toInt(r.get("JobLotId")) == 0) throw new IllegalArgumentException("JobLot Field Require in Output Detail");
            d.put("JobLotId", toInt(r.get("JobLotId")));
            d.put("PackingtypeId", toInt(r.get("PackingTypeId")));
            d.put("Qty", toDouble(r.get("Quantity")));
            d.put("Weight", toDouble(r.get("Weight")));
            if (toDouble(r.get("Weight")) == 0) throw new IllegalArgumentException("Weight Field Required");
            d.put("Rate", toDouble(r.get("RateWithoutExp")));
            d.put("NetRate", toDouble(r.get("NetRate")));
            if (toDouble(r.get("RateWithoutExp")) == 0) throw new IllegalArgumentException("Rate Field Required");
            if (toDouble(r.get("NetRate")) == 0) throw new IllegalArgumentException("NetRate Field Required");
            d.put("RateUOMId", toInt(r.get("RateUOMId")));
            if (toInt(r.get("RateUOMId")) == 0) throw new IllegalArgumentException("RateUom Field Required");
            d.put("ProductionJobOrderId", jobOrderId);
            d.put("Amount", toDouble(r.get("AmountWithoutExp")));
            d.put("ItemOhCost", toDouble(r.get("ItemOhCost")));
            d.put("GeneralOhCost", toDouble(r.get("GeneralOhCost")));
            d.put("TotalAmount", toDouble(r.get("TotalAmount")));
            if (toDouble(r.get("AmountWithoutExp")) == 0) throw new IllegalArgumentException("ItemAmount mustbe greater than zero Please Check");
            d.put("Remarks", str(r.get("Remarks")));
            d.put("RefDocumentTypeId", 0); d.put("RefDocSubIdNo", 0); d.put("JobOrderOutPutId", 0); d.put("JobOrderInPutId", 0);
            d.put("PackUnit", 0); d.put("ActionTypeId", 0);
            details.add(d);
        }
        /* DAL 0208 SetDataForUpdate - DeleteFlag 1 on the first row of each FoodProductionId group, 0 on the rest */
        int last = 0;
        for (Map<String, Object> d : details) {
            int fp = toInt(d.get("FoodProductionId"));
            d.put("DeleteFlag", fp != last ? 1 : 0);
            s.setProc("[ST].[USP_InvFoodProductionDetail_Update]", d, P_DETAIL);
            last = fp;
        }
        return SteelProductionSupport.ok("Record has updated Successfully");     // :5524
    }

    /** btnApproveSettlement_Click:5693 -> InvProductionJobOrder.JobOrderApproveList (Steel BLL 0236) - JobOrderApprove. */
    @Transactional
    public Map<String, Object> approveSettlement(int jobOrderId) {
        UserAccount u = s.user();
        if (jobOrderId <= 0) throw new IllegalArgumentException("JobOrder Settlement Field is Required");
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        if (s.branchId(u) != 0) p.put("@BranchesId", s.branchId(u));
        int fy = s.ctx().currentFinancialYearId();
        if (fy != 0) p.put("@FinancialYearId", fy);
        p.put("@Id", jobOrderId);
        p.put("@PendingForView", 0);                 // ApprovalList.ActionTypeId - never set by the form (0)
        p.put("@EntryUserId", u.getId());
        p.put("@Activity", "JobOrderApprove");
        s.query(JO, p);
        return SteelProductionSupport.ok("Record Approved Successfully");          // :5718
    }

    // ============================================================================== helpers

    private List<Map<String, Object>> q(String label, String sql, Object... args) {
        try {
            return jdbc.queryForList(sql, args);
        } catch (Exception e) {
            LOG.warn("Steel Food Production lookup '{}' failed: {}", label, sql, e);
            return new ArrayList<>();
        }
    }

    private static Set<String> set(String... a) { return Collections.unmodifiableSet(new LinkedHashSet<>(Arrays.asList(a))); }
}
