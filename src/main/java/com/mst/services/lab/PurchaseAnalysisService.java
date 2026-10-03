package com.mst.services.lab;

import com.mst.models.UserAccount;
import com.mst.repositories.lab.PurchaseAnalysisRepository;
import com.mst.security.CurrentUserContext;
import com.mst.services.DesktopAttachmentStore;
import com.mst.services.StoreScreenRights;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.math.BigDecimal;
import java.math.MathContext;
import java.math.RoundingMode;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.LocalTime;
import java.util.ArrayList;
import java.util.Base64;
import java.util.HashMap;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Locale;
import java.util.Map;
import java.util.Set;

import static com.mst.repositories.lab.PurchaseAnalysisRepository.ci;
import static com.mst.repositories.lab.PurchaseAnalysisRepository.str;
import static com.mst.repositories.lab.PurchaseAnalysisRepository.toDouble;
import static com.mst.repositories.lab.PurchaseAnalysisRepository.toInt;

/**
 * Lab (ModuleId 7) — screen 160 "Purchase Analysis".
 *
 * <pre>
 * Screen id        160 (dbo.ScreenDefinition; TargetUrl Architecture.WinApp.Lab.InvLabPurchaseAnalysis)
 * ScreenName       InvLabPurchaseAnalysis          (base.Name; rights key — InvLabPurchaseAnalysis.cs:557)
 * DocumentTypeId   303                             (:808, :1859, :2390)
 * Route            /quality/purchase-analysis ; API /api/lab/purchase-analysis
 * Desktop          Architecture.WinApp.Lab/InvLabPurchaseAnalysis.cs (7,490 lines, designer :3498-7490)
 *                  Architecture.WinApp.Lab/PendingGPForPurchaseLab.cs (the "Loader" dialog)
 * BLL / DAL        BLL 0403 / DAL 0359 Lab.InvLabAnalysisPurchaseHeader; BLL 0567 GatePassInward;
 *                  BLL 0400 InvLabAnalysisGroup; BLL 0406 InvLabGroupAnalysisStandards;
 *                  BLL 0407 / DAL 0362 InvLabSampleAnalysisHeader; BLL 0052 ItemPricingScheduleForRice;
 *                  BLL 0583 Item; BLL 0025 WarehousesAllocationToBranch
 * Models           0620 InvLabAnalysisPurchaseHeader, 0619 InvLabAnalysisPurchaseDetail,
 *                  0608 InvLabAnalysisPurchaseSubParamsDetail
 * Prints           653-RptInvLabPurchaseAnalysisSlip.rpt (:3157) and 257-InwardGatePassWithWbAndLabSlip.rpt
 *                  (CommonServices.cs:14268) — both through the shared print runtime (/js/print-rpt.js).
 * </pre>
 *
 * The form has NO Delete and NO approve action. Approval is decided inside the two header procedures:
 * Accepted and configuration 114 off -> IsApproved / IsAcceptedApproved 1 with the entry user; otherwise
 * both 0 (Sp_InvLabAnalysisPurchaseHeader_Insert sql:165762, _Update sql:166195). The Update procedure
 * deletes the detail and sub-parameter rows and the DAL re-inserts them (sql:166381).
 *
 * ---------------------------------------------------------------------------------------------
 * DESKTOP BEHAVIOUR REPRODUCED, NOT CORRECTED (each is the user's call to change)
 * ---------------------------------------------------------------------------------------------
 *  D1  Weight Cut is tested through Conversion.ToInt of the TEXT (:1489, :1517): "12.5" is not an
 *      integer literal, reads as 0 and passes both "cannot be greater than 10" and "Weight Cut On
 *      field required". The same holds for Weight Cut Uom (:1496).
 *  D2  Crop is saved as the combo's TEXT (:1878) — the default row saves "-- Select --".
 *  D3  "Editable After Approval" ticked in the form grid is never saved (the model property is virtual,
 *      model 0619); it comes from the group standards when a record is read.
 *  D4  The sub-parameter total is compared with the parameter's result by exact double equality (:1849).
 *  D5  A result above the parameter's own maximum / below its minimum (InvLabAnalysisItems Min/Max,
 *      "ParamsMinValue/ParamsMaxValue") refuses the save (:1834-1841); the group standard's Min/Max only
 *      colours the cell. A parameter with no maximum (0) refuses any positive result.
 *  D6  Save sends EntryDate / EntryUser = now / the signed-in user on update too; the Update procedure
 *      does not write them but uses them as the approval user / date (sql:166199).
 *  D7  The history "Update" (cuts) button returns at "Weight Cut can't be changed when it is not
 *      compulsory." even when only the Rate Cut was edited (:2606).
 *  D8  History filters: the Pest Result / Pest Status combos are never read by historygridfill (:2405-2412).
 *  D9  The loader's combos are read with OrganizationId and CompanyId swapped (PendingGPForPurchaseLab.cs:97
 *      against the BLL signature).
 *  D10 On opening a Gate Purchase record of type 110, the Rate Uom combo is emptied (:2065) and the item
 *      rate / premium boxes are not refilled (:2110 only for 106): updating it writes RateUomId 0,
 *      ItemRate 0, Premium 0.
 *  D11 Sp_InvLabGroupAnalysisStandards_GetAllMethod 'GetParametersFromGroupStandards' filters
 *      CompanyId = @OrganizationId and OrganizationId = @CompanyId (sql:167552) — the procedure's own swap.
 *
 * ---------------------------------------------------------------------------------------------
 * DEVIATIONS (web only)
 * ---------------------------------------------------------------------------------------------
 *  W1  Doc No: on a new save the server asks the generator (the Insert procedure recomputes MAX+1 anyway),
 *      on update it keeps the stored number; never taken from the request.
 *  W2  Open / update / print / cuts / parameter update only when the record is DocumentTypeId 303 and
 *      belongs to the user's organization and company (ReadById filters by Id alone). The gate pass must be
 *      one GetGpDetailByGatepassNo returns for the user's organization and company.
 *  W3  Every posted reference must be in the list the form offers: the gate pass (new: the pending list;
 *      update: the record's own), the item (the gate pass type's item list), the analysis group (the item's
 *      parent category), packing type, warehouse, job lot, rate uom, weight-cut-on, pest result / status.
 *      Supplier, order no (SupCustCode), purchase order id and the parameter rows' standard ids, limits and
 *      status ids are read by the server (gate pass / standards / stored record), not posted.
 *  W4  Net Rate is recomputed as NetRateCalculation does (:3227). On update the two "Apply ... Cut Later"
 *      ticks are the stored ones (the boxes are disabled once a record is opened, :2023).
 *  W5  With RateFromScheduleOnLabForGatePurchase on, a non-zero Item Rate of a Gate Purchase (106/110) must
 *      be a pricing-schedule rate of the item and rate uom (or the stored one on update) — the desktop box
 *      is read-only there.
 *  W6  The purchase order id of a non-PO gate pass is 0 on a new record and the stored one on update; the
 *      desktop keeps whatever the hidden box held from a previously selected PO gate pass.
 *  W7  Pictures: Browse uploads through DesktopAttachmentStore (the configured attachment folder / VPS),
 *      stored as "Inv_&lt;uuid&gt;.&lt;ext&gt;" instead of "&lt;guid&gt;.Jpg"; the Canon camera buttons are not ported.
 *  W8  The history cuts update and the "editable after approval" result update need the Update right
 *      (the desktop checks no right there).
 *  W9  A Rate Uom is saved only for a Gate Purchase (106/110) — the two types the form binds the list for
 *      (:1267) — or when it is the opened record's own; the desktop would also save a list selection left
 *      over from a previously chosen gate pass.
 *
 * Not ported: attachments (Attachment button; stored DMS attachments are untouched by an update), the
 * Canon camera capture, saved grid layouts (CtrlGrdBar).
 */
@Service
public class PurchaseAnalysisService {

    private static final Logger LOG = LoggerFactory.getLogger(PurchaseAnalysisService.class);

    public static final String SCREEN_NAME = "InvLabPurchaseAnalysis";
    public static final int SCREEN_ID = 160;
    public static final int DOCUMENT_TYPE_ID = 303;
    public static final String RPT_653 = "653-RptInvLabPurchaseAnalysisSlip.rpt";
    public static final String RPT_257 = "257-InwardGatePassWithWbAndLabSlip.rpt";

    private final PurchaseAnalysisRepository repo;
    private final StoreScreenRights rights;
    private final CurrentUserContext ctx;
    private final DesktopAttachmentStore attachments;

    public PurchaseAnalysisService(PurchaseAnalysisRepository repo, StoreScreenRights rights, CurrentUserContext ctx,
                                   DesktopAttachmentStore attachments) {
        this.repo = repo;
        this.rights = rights;
        this.ctx = ctx;
        this.attachments = attachments;
    }

    /** A desktop Yes/No prompt the page has not answered yet (HTTP 409 {confirm:true,message}). */
    public static class ConfirmRequired extends RuntimeException {
        public ConfirmRequired(String message) { super(message); }
    }

    // ======================================================================== form load

    /** InvLabPurchaseAnalysis_Load (:544) — everything the form binds on open. */
    public Map<String, Object> lookups() {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Boolean> r = rights.of(SCREEN_NAME);
        if (!Boolean.TRUE.equals(r.get("view"))) throw new AccessDeniedException("You do not have the View right for Purchase Analysis.");
        int fy = ctx.currentFinancialYearId();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rights", r);
        out.put("screenId", SCREEN_ID);
        out.put("documentTypeId", DOCUMENT_TYPE_ID);
        putConfigs(u, out);                                                 // GetConfiguration (:681)
        out.put("gatePasses", gatePasses(u, fy));                          // GpNoFill (:753)
        out.put("cropYears", cropYears(u));                                // CropBindFromGlobal (:1008)
        out.put("packingTypes", packingTypes());                           // PackingTypeBindFromGlobal (:1021)
        out.put("jobLots", jobLots(u));                                    // JobLotBindFromGlobal (:1034)
        out.put("docNo", repo.generateCode(u, fy, DOCUMENT_TYPE_ID));      // GenerateCode (:793)
        out.put("warehouses", warehouses(u));                              // WarehouseFill (:727)
        out.put("weightCutOn", weightCutOn());                             // WeightCutOnFill (:1127)
        out.put("pestResults", pestResults());                             // PestResult (:1101)
        out.put("pestStatuses", pestStatuses());                           // PestStatus (:1114)
        out.put("history", historyCombos(u));                              // HistoryComboFill (:597)
        return out;
    }

    /** btnRefresh_Click (:3441) — globals re-read, the lists rebound, the configured defaults applied again. */
    public Map<String, Object> formRefresh() {
        UserAccount u = requireView();
        int fy = ctx.currentFinancialYearId();
        Map<String, Object> out = new LinkedHashMap<>();
        putConfigs(u, out);
        out.put("gatePasses", gatePasses(u, fy));
        out.put("cropYears", cropYears(u));
        out.put("packingTypes", packingTypes());
        out.put("jobLots", jobLots(u));
        out.put("docNo", repo.generateCode(u, fy, DOCUMENT_TYPE_ID));
        out.put("warehouses", warehouses(u));
        out.put("weightCutOn", weightCutOn());
        return out;
    }

    /** refresh() (:2185) — GenerateCode, GpNoFill, GetConfiguration, the configured defaults. */
    public Map<String, Object> formNew() {
        UserAccount u = requireView();
        int fy = ctx.currentFinancialYearId();
        Map<String, Object> out = new LinkedHashMap<>();
        putConfigs(u, out);
        out.put("docNo", repo.generateCode(u, fy, DOCUMENT_TYPE_ID));
        out.put("gatePasses", gatePasses(u, fy));
        return out;
    }

    /** btnRefreshHistory_Click (:2305). */
    public Map<String, Object> historyRefresh() {
        UserAccount u = requireView();
        return historyCombos(u);
    }

    /**
     * GetConfiguration (:681) + GetConfigurationsFromGlobalAndBindValuesInColumns (:696). The attachment
     * folder itself never leaves the server; the page only learns whether one is configured.
     */
    private void putConfigs(UserAccount u, Map<String, Object> out) {
        out.put("rateFromSchedule", rateFromSchedule(u));
        out.put("attachmentPathSet", !repo.config(u, "Attachment Folder Path").trim().isEmpty());
        out.put("defaultJobLot", cvInt(repo.config(u, "Job/Lot")));
        out.put("defaultCropYear", cvInt(repo.config(u, "Default Crop Year")));
        out.put("defaultPackingType", cvInt(repo.config(u, "Paking Type")));
        out.put("defaultWarehouse", cvInt(repo.config(u, "Warehouse")));
    }

    private boolean rateFromSchedule(UserAccount u) { return toBoolText(repo.config(u, "RateFromScheduleOnLabForGatePurchase")); }

    // ============================================================================ lists

    /** GpNoFill (:770-783): Id, GpSrNo, VehicleNo, OrderType, SupplierName, LabType. */
    private List<Map<String, Object>> gatePasses(UserAccount u, int fy) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.gatePassesForAnalysis(u, fy, 0, 0)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("GpSrNo", str(ci(r, "GpSrNo")));
            o.put("VehicleNo", str(ci(r, "VehicleNo")));
            o.put("OrderType", str(ci(r, "OrderType")));
            o.put("SupplierName", str(ci(r, "SupplierName")));
            o.put("LabType", str(ci(r, "LabType")));
            out.add(o);
        }
        return out;
    }

    private List<Map<String, Object>> cropYears(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.cropYears(u)) out.add(idName(toInt(ci(r, "Id")), str(ci(r, "CropYear"))));
        return out;
    }

    private List<Map<String, Object>> packingTypes() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.packingTypes()) out.add(idName(toInt(ci(r, "Id")), str(ci(r, "PackTypeDesc"))));
        return out;
    }

    private List<Map<String, Object>> jobLots(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.jobLots(u)) out.add(idName(toInt(ci(r, "Id")), str(ci(r, "JobLotDescription"))));
        return out;
    }

    /** WarehouseFill (:744): the whole procedure row set is bound; value Id, display WareHouseName. */
    private List<Map<String, Object>> warehouses(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.warehouses(u)) {
            Map<String, Object> o = idName(toInt(ci(r, "Id")), str(ci(r, "WareHouseName")));
            o.put("BranchId", str(ci(r, "BranchId")));
            o.put("BranchName", str(ci(r, "BranchName")));
            o.put("WareHouseTypeId", str(ci(r, "WareHouseTypeId")));
            o.put("WareHouseType", str(ci(r, "WareHouseType")));
            o.put("IsActive", toBool(ci(r, "IsActive")));
            o.put("PlantId", str(ci(r, "PlantId")));
            o.put("PlantName", str(ci(r, "PlantName")));
            out.add(o);
        }
        return out;
    }

    private List<Map<String, Object>> weightCutOn() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.weightCutOn()) out.add(idName(toInt(ci(r, "Id")), str(ci(r, "type"))));
        return out;
    }

    private List<Map<String, Object>> pestResults() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.pestResults()) out.add(idName(toInt(ci(r, "Id")), str(ci(r, "PestResult"))));
        return out;
    }

    private List<Map<String, Object>> pestStatuses() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.pestStatuses()) out.add(idName(toInt(ci(r, "Id")), str(ci(r, "PestStatus"))));
        return out;
    }

    /** HistoryComboFill (:626-660) — the one row set split by its Activity column. */
    private Map<String, Object> historyCombos(UserAccount u) {
        String[][] map = {
                { "Supplier", "suppliers" }, { "Item", "items" }, { "AnaylstStatus", "analystStatuses" },
                { "ApprovedStatus", "approvedStatuses" }, { "GpNo", "gpNos" }, { "VehicleNo", "vehicles" },
                { "PestResult", "pestResults" }, { "PestStatus", "pestStatuses" } };
        Map<String, Object> out = new LinkedHashMap<>();
        Map<String, List<Map<String, Object>>> byActivity = new HashMap<>();
        for (String[] m : map) {
            List<Map<String, Object>> l = new ArrayList<>();
            byActivity.put(m[0], l);
            out.put(m[1], l);
        }
        for (Map<String, Object> r : repo.historyCombos(u)) {
            List<Map<String, Object>> l = byActivity.get(str(ci(r, "Activity")));
            if (l != null) l.add(idName(toInt(ci(r, "Id")), str(ci(r, "ReferenceName"))));
        }
        return out;
    }

    /** ItemsBind (:980) — Id, ItemName, PoDetailId 0, ParentcategoryId, Parentcategory. */
    private List<Map<String, Object>> itemsBind(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.itemsByParentCategories(u, "1,2,4")) out.add(itemRow(r));
        return out;
    }

    /** ItemsBindFromPricingSchedule (:1047) — same five columns. */
    private List<Map<String, Object>> itemsFromSchedule(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.itemsFromPricingSchedule(u)) out.add(itemRow(r));
        return out;
    }

    private static Map<String, Object> itemRow(Map<String, Object> r) {
        Map<String, Object> o = new LinkedHashMap<>();
        o.put("Id", toInt(ci(r, "Id")));
        o.put("ItemName", str(ci(r, "ItemName")));
        o.put("PoDetailId", 0);
        o.put("ParentcategoryId", toInt(ci(r, "ParentcategoryId")));
        o.put("Parentcategory", str(ci(r, "Parentcategory")));
        return o;
    }

    /**
     * The item list txtgatepassno_Leave (:851-912) binds for a gate pass type:
     * 41 the purchase order's lines; 105/111 ItemsBind; 106/110/98/52/700/175 the pricing-schedule items
     * when RateFromScheduleOnLabForGatePurchase is on, else ItemsBind; any other type binds nothing.
     */
    private List<Map<String, Object>> itemsFor(UserAccount u, int refDocumentTypeId, List<Map<String, Object>> gpRows) {
        if (refDocumentTypeId == 41) {
            List<Map<String, Object>> out = new ArrayList<>();
            for (Map<String, Object> r : gpRows) {
                Map<String, Object> o = new LinkedHashMap<>();
                o.put("Id", toInt(ci(r, "OrderItemId")));
                o.put("ItemName", str(ci(r, "ItemName")));
                o.put("PoDetailId", toInt(ci(r, "PoDetailId")));
                o.put("SupplierCustomerId", toInt(ci(r, "SupplierCustomerId")));
                o.put("PartyName", str(ci(r, "PartyName")));
                o.put("ParentcategoryId", toInt(ci(r, "ParentcategoryId")));
                o.put("Parentcategory", str(ci(r, "Parentcategory")));
                out.add(o);
            }
            return out;
        }
        if (refDocumentTypeId == 105 || refDocumentTypeId == 111) return itemsBind(u);
        if (refDocumentTypeId == 106 || refDocumentTypeId == 110 || refDocumentTypeId == 98 || refDocumentTypeId == 52
                || refDocumentTypeId == 700 || refDocumentTypeId == 175) {
            return rateFromSchedule(u) ? itemsFromSchedule(u) : itemsBind(u);
        }
        return new ArrayList<>();
    }

    // ===================================================================== form events

    /**
     * txtgatepassno_Leave (:823): the gate pass rows (GetGpDetailByGatepassNo), the item list of its type
     * and — for a purchase-order gate pass — the first line's moisture (:891-894).
     */
    public Map<String, Object> gatePass(int gatePassId) {
        UserAccount u = requireView();
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> rows = gatePassId <= 0 ? new ArrayList<>() : repo.gatePassDetail(u, gatePassId);
        List<Map<String, Object>> plain = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("OrderNo", str(ci(r, "OrderNo")));
            o.put("BiltyNo", str(ci(r, "BiltyNo")));
            o.put("BiltyDate", plain(ci(r, "BiltyDate")));
            o.put("VehicleNo", str(ci(r, "VehicleNo")));
            o.put("CityName", str(ci(r, "CityName")));
            o.put("RefDocumentTypeId", toInt(ci(r, "RefDocumentTypeId")));
            o.put("OrderType", str(ci(r, "OrderType")));
            o.put("GpQty", netText(ci(r, "GpQty")));                         // Conversion.ToString(dtgpsr.Rows[0]["GpQty"]) (:849)
            o.put("SupplierCustomerId", str(ci(r, "SupplierCustomerId")));
            o.put("PartyName", str(ci(r, "PartyName")));
            o.put("PurchaseOrderId", str(ci(r, "PurchaseOrderId")));
            o.put("OrderItemId", toInt(ci(r, "OrderItemId")));
            o.put("ItemName", str(ci(r, "ItemName")));
            o.put("PoDetailId", toInt(ci(r, "PoDetailId")));
            o.put("ParentcategoryId", toInt(ci(r, "ParentcategoryId")));
            o.put("Parentcategory", str(ci(r, "Parentcategory")));
            o.put("Crop", str(ci(r, "Crop")));
            o.put("InvLabSampleAnalysisHeaderId", toInt(ci(r, "InvLabSampleAnalysisHeaderId")));
            o.put("SampleNo", str(ci(r, "SampleNo")));
            plain.add(o);
        }
        out.put("rows", plain);
        if (rows.isEmpty()) { out.put("items", new ArrayList<>()); return out; }
        int ref = toInt(ci(rows.get(0), "RefDocumentTypeId"));
        List<Map<String, Object>> items = itemsFor(u, ref, rows);
        out.put("items", items);
        if (ref == 41 && !items.isEmpty()) {
            out.put("moisture", repo.moisture(u, toInt(items.get(0).get("Id")), toInt(ci(rows.get(0), "PurchaseOrderId"))));
        }
        return out;
    }

    /** cmbitem_Leave (:1262-1265) — CommonServices.GetMoistureByItemIdAndPurchaseOrderId. */
    public Map<String, Object> moisture(int itemId, int purchaseOrderId) {
        UserAccount u = requireView();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("moisture", repo.moisture(u, itemId, purchaseOrderId));
        return out;
    }

    /**
     * cmbitem_Leave (:1269-1311): the Rate Uom list — GetRateUomFromItemPricingSchedule (value RateUomId,
     * display RateUom) when RateFromScheduleOnLabForGatePurchase is on, else the item's rows of the global
     * UOM schedule (value Id, display UOMCode).
     */
    public Map<String, Object> rateUoms(int itemId) {
        UserAccount u = requireView();
        Map<String, Object> out = new LinkedHashMap<>();
        boolean fromSchedule = rateFromSchedule(u);
        out.put("fromSchedule", fromSchedule);
        out.put("rows", rateUomRows(u, itemId, fromSchedule));
        return out;
    }

    private List<Map<String, Object>> rateUomRows(UserAccount u, int itemId, boolean fromSchedule) {
        List<Map<String, Object>> rows = new ArrayList<>();
        if (itemId <= 0) return rows;
        if (fromSchedule) {
            for (Map<String, Object> r : repo.rateUomFromPricingSchedule(u, itemId)) {
                Map<String, Object> o = idName(toInt(ci(r, "RateUomId")), str(ci(r, "RateUom")));
                rows.add(o);
            }
        } else {
            for (Map<String, Object> r : repo.uomSchedule(u)) {
                if (toInt(ci(r, "ItemId")) != itemId) continue;
                Map<String, Object> o = idName(toInt(ci(r, "Id")), str(ci(r, "UOMCode")));
                o.put("Equivalent", toDouble(ci(r, "Equivalent")));
                o.put("BaseRateUom", toBool(ci(r, "BaseRateUom")));
                o.put("BasePackUom", toBool(ci(r, "BasePackUom")));
                rows.add(o);
            }
        }
        return rows;
    }

    /** CmbRateUom_Leave (:1391-1410) — the first row of GetItemRateByItemIdAndUomId, or found:false. */
    public Map<String, Object> itemRate(int itemId, int rateUomId) {
        UserAccount u = requireView();
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> r = repo.itemRateByItemAndUom(u, itemId, rateUomId);
        out.put("found", !r.isEmpty());
        if (!r.isEmpty()) {
            out.put("ItemRate", clr(toDouble(ci(r.get(0), "ItemRate"))));          // dt.Rows[0]["ItemRate"].ToString()
            out.put("Id", toInt(ci(r.get(0), "Id")));
        }
        return out;
    }

    /** AnalysisGroup(ParentcategoryId) (:936): Id, AnalysisGroupDescription, GroupTypeId (GroupType), GroupType (InvParentCateDescription). */
    public List<Map<String, Object>> analysisGroups(int parentCategoryId) {
        UserAccount u = requireView();
        return analysisGroupsFor(u, parentCategoryId);
    }

    private List<Map<String, Object>> analysisGroupsFor(UserAccount u, int parentCategoryId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.analysisGroups(u, parentCategoryId)) {
            Map<String, Object> o = idName(toInt(ci(r, "Id")), str(ci(r, "AnalysisGroupDescription")));
            o.put("GroupTypeId", toInt(ci(r, "GroupType")));
            o.put("GroupType", str(ci(r, "InvParentCateDescription")));
            out.add(o);
        }
        return out;
    }

    /** cmbanalysisgroup_Leave (:1141) — the parameter rows (and their sub-parameters) of a group's standards. */
    public List<Map<String, Object>> parameters(int groupId) {
        UserAccount u = requireView();
        return parameterRows(u, groupId);
    }

    /**
     * :1160-1215. Sub-parameters are the standards rows with SubParamsId &gt; 0; the parameters are the
     * distinct InvLabAnalysisItemsId in row order, each carrying the sub-parameters whose
     * InvParentParameterId is that parameter.
     */
    private List<Map<String, Object>> parameterRows(UserAccount u, int groupId) {
        List<Map<String, Object>> dt = repo.parametersFromGroupStandards(u, groupId);
        List<Map<String, Object>> subs = new ArrayList<>();
        for (Map<String, Object> r : dt) {
            if (toInt(ci(r, "SubParamsId")) > 0) {
                Map<String, Object> s = new LinkedHashMap<>();
                s.put("Id", 0);
                s.put("InvParentParameterId", toInt(ci(r, "ParentParameterId")));
                s.put("SubParameterId", toInt(ci(r, "SubParamsId")));
                s.put("SubParameterName", str(ci(r, "SubParameterName")));
                s.put("ResultValue", 0d);
                subs.add(s);
            }
        }
        List<Map<String, Object>> out = new ArrayList<>();
        Set<Integer> seen = new HashSet<>();
        for (Map<String, Object> r : dt) {
            int itemsId = toInt(ci(r, "InvLabAnalysisItemsId"));
            if (!seen.add(itemsId)) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", 0);
            o.put("InvLabAnalysisItemsId", itemsId);
            o.put("InvLabGroupAnalysisStandardsId", toInt(ci(r, "Id")));
            o.put("AnalysisStutusId", 0);
            o.put("AnalysisGroupDescription", str(ci(r, "AnalysisGroupDescription")));
            o.put("AnalysisParameterDescription", str(ci(r, "AnalysisParameterDescription")));
            o.put("MinValue", toDouble(ci(r, "MinValue")));
            o.put("MaxValue", toDouble(ci(r, "MaxValue")));
            o.put("IsEditableAfterApproval", false);                         // never assigned in :1202-1211
            o.put("InAnalysisResult", 0d);
            o.put("RemarksDetail", null);
            o.put("ParamsMinValue", toDouble(ci(r, "ParamsMinValue")));
            o.put("ParamsMaxValue", toDouble(ci(r, "ParamsMaxValue")));
            o.put("PmLabStatus", null);
            List<Map<String, Object>> mine = new ArrayList<>();
            for (Map<String, Object> s : subs) if (toInt(s.get("InvParentParameterId")) == itemsId) mine.add(new LinkedHashMap<>(s));
            o.put("subs", mine);
            out.add(o);
        }
        return out;
    }

    /**
     * CmbSampleAnaylsis_Leave (:1335) — InvLabSampleAnalysisHeader.GetByID: the sample's detail rows, each
     * with its sub-parameters, and the flat list of sub-parameters whose ResultValue &gt; 0 (:1349-1361).
     */
    public Map<String, Object> sample(int id) {
        UserAccount u = requireView();
        Map<String, Object> out = new LinkedHashMap<>();
        Map<String, Object> h = id <= 0 ? null : repo.sampleHeader(id);
        if (h == null || toInt(ci(h, "OrganizationId")) != i(u.getOrganizationId()) || toInt(ci(h, "CompanyId")) != i(u.getCompanyId())) {
            out.put("found", false);                                           // obj == null -> return (:1345)
            return out;
        }
        List<Map<String, Object>> subs = new ArrayList<>();
        for (Map<String, Object> s : repo.sampleSubParams(id)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(s, "Id")));
            o.put("InvParentParameterId", toInt(ci(s, "InvParentParameterId")));
            o.put("SubParameterId", toInt(ci(s, "SubParameterId")));
            o.put("ParentParameterName", str(ci(s, "ParentParameterName")));
            o.put("SubParameterName", str(ci(s, "SubParameterName")));
            o.put("ResultValue", toDouble(ci(s, "ResultValue")));
            subs.add(o);
        }
        List<Map<String, Object>> details = new ArrayList<>();
        List<Map<String, Object>> flat = new ArrayList<>();
        for (Map<String, Object> d : repo.sampleDetails(id)) {
            Map<String, Object> o = new LinkedHashMap<>();
            int itemsId = toInt(ci(d, "InvLabAnalysisItemsId"));
            o.put("Id", toInt(ci(d, "Id")));
            o.put("InvLabAnalysisItemsId", itemsId);
            o.put("AnalysisGroupDescription", str(ci(d, "AnalysisGroupDescription")));
            o.put("AnalysisParameterDescription", str(ci(d, "AnalysisParameterDescription")));
            o.put("MinValue", toDouble(ci(d, "MinValue")));
            o.put("MaxValue", toDouble(ci(d, "MaxValue")));
            o.put("ResultValue", toDouble(ci(d, "ResultValue")));
            o.put("RemarksDetail", str(ci(d, "RemarksDetail")));
            List<Map<String, Object>> mine = new ArrayList<>();
            for (Map<String, Object> s : subs) {
                if (toInt(s.get("InvParentParameterId")) != itemsId) continue;
                mine.add(s);
                if (toDouble(s.get("ResultValue")) > 0d) flat.add(s);
            }
            o.put("subs", mine);
            details.add(o);
        }
        out.put("found", true);
        out.put("details", details);
        out.put("subParams", flat);
        return out;
    }

    // =========================================================================== loader

    /** PendingGPForPurchaseLab.CombosFill (:93) — the "Supplier" and "OrderType" rows. */
    public Map<String, Object> loaderCombos() {
        UserAccount u = requireView();
        List<Map<String, Object>> suppliers = new ArrayList<>(), orderTypes = new ArrayList<>();
        for (Map<String, Object> r : repo.loaderCombos(u)) {
            String a = str(ci(r, "Activity"));
            if ("Supplier".equals(a)) suppliers.add(idName(toInt(ci(r, "Id")), str(ci(r, "ReferenceName"))));
            else if ("OrderType".equals(a)) orderTypes.add(idName(toInt(ci(r, "Id")), str(ci(r, "ReferenceName"))));
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("suppliers", suppliers);
        out.put("orderTypes", orderTypes);
        return out;
    }

    /** PendingGPForPurchaseLab.FillGrid (:186) — the 13 columns the dialog builds (:204-219). */
    public List<Map<String, Object>> loader(int supplierId, int orderTypeId) {
        UserAccount u = requireView();
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.gatePassesForAnalysis(u, ctx.currentFinancialYearId(), supplierId, orderTypeId)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("GpNo", toInt(ci(r, "GpSrNo")));
            o.put("GpDate", plain(ci(r, "GpDate")));
            o.put("VehicleType", str(ci(r, "VehicleType")));
            o.put("VehicleNo", str(ci(r, "VehicleNo")));
            o.put("RefDocumentTypeId", toInt(ci(r, "RefDocumentTypeId")));
            o.put("PartyName", str(ci(r, "SupplierName")));
            o.put("BiltyNo", str(ci(r, "BiltyNo")));
            o.put("City", str(ci(r, "City")));
            o.put("InTime", plain(ci(r, "InDateTimeStamp")));
            o.put("EntryUser", str(ci(r, "EntryUserName")));
            o.put("EntryDate", plain(ci(r, "EntryDate")));
            o.put("OrderType", str(ci(r, "OrderType")));
            out.add(o);
        }
        return out;
    }

    // ========================================================================== history

    /** historygridfill (:2380) — the table the form builds from GetAll (:2422-2461). */
    public List<Map<String, Object>> history(boolean fromChecked, String from, boolean toChecked, String to,
                                             String fromDocNo, String toDocNo, int supplierId, int itemId, int gpId,
                                             String approvedStatus, String analystStatus, String vehicleNo) {
        UserAccount u = requireView();
        boolean viewAll = rights.has(SCREEN_NAME, "viewAll");
        Timestamp f = fromChecked && !isBlank(from) ? pickerDate(from) : null;
        Timestamp t = toChecked && !isBlank(to) ? pickerDate(to) : null;
        List<Map<String, Object>> rows = repo.readAll(u, ctx.currentFinancialYearId(), DOCUMENT_TYPE_ID, viewAll, f, t,
                cvInt(fromDocNo), cvInt(toDocNo), gpId, supplierId, itemId, nz(analystStatus), nz(approvedStatus), nz(vehicleNo));
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("LabType", str(ci(r, "LabType")));
            o.put("DocNo", toInt(ci(r, "DocNo")));
            o.put("DocDate", plain(ci(r, "DocDate")));
            o.put("GatePassNo", toInt(ci(r, "GpSrNo")));                       // column "GatePass#"
            o.put("SupplierCode", str(ci(r, "SupCustCode")));
            o.put("PartyName", str(ci(r, "PartyName")));
            o.put("BiltyNo", str(ci(r, "BiltyNo")));
            o.put("VehicleNo", str(ci(r, "VehicleNo")));
            o.put("CropYear", str(ci(r, "Crop")));
            o.put("WarehouseName", str(ci(r, "WareHouseName")));
            o.put("AnalysisItem", str(ci(r, "ItemName")));
            o.put("WeightCutOn", str(ci(r, "WeightCutOn")));
            o.put("WeightCutCompulsory", toBool(ci(r, "IsWeightCutCompulsory")));
            o.put("WeightCut", dbText(ci(r, "DeductionWeight")));
            o.put("RateCutCompulsory", toBool(ci(r, "IsRateCutCompulsory")));
            o.put("RateCut", dbText(ci(r, "DeductionRate")));
            o.put("AnalystName", str(ci(r, "AnalystName")));
            o.put("AnalystStatus", str(ci(r, "AnalystStatus")));
            o.put("ApprovedStatus", str(ci(r, "ApprovedStatus")));
            o.put("PestResult", str(ci(r, "PestResult")));
            o.put("PestStatus", str(ci(r, "PestStatus")));
            o.put("EntryUser", str(ci(r, "UserName")));
            o.put("EntryDate", plain(ci(r, "EntryDate")));
            o.put("ApprovedUser", str(ci(r, "ApprovedUser")));
            o.put("ApprovedDate", plain(ci(r, "ApprovedDate")));
            o.put("AnalysisPic", str(ci(r, "Pic1Pathe")).isEmpty() ? 0 : 1);
            o.put("CookingPic", str(ci(r, "CookingPic")).isEmpty() ? 0 : 1);
            o.put("Attachments", toInt(ci(r, "NoOfAttachments")));
            o.put("Remarks", str(ci(r, "RemarksHeader")));
            o.put("WeightCutBefore", dbText(ci(r, "DeductionWeight")));
            o.put("RateCutBefore", dbText(ci(r, "DeductionRate")));
            out.add(o);
        }
        return out;
    }

    /**
     * grdhistory_ColumnButtonClick "Update" (:2598-2624). Both cuts are the cell texts (Conversion.ToDouble).
     * The compulsory flags are the stored record's (the grid's flag columns are read-only copies of them).
     */
    @Transactional
    public Map<String, Object> updateCuts(int id, PurchaseAnalysisDto.Cuts dto) {
        UserAccount u = ctx.requireAccountingUser();
        if (!rights.has(SCREEN_NAME, "update")) throw new AccessDeniedException("You do not have the Update right for this screen.");   // W8
        Map<String, Object> h = ownedHeader(u, id);                                                  // W2
        if (h == null) throw new IllegalArgumentException("Record Not Found");
        double weightCut = cvDouble(dto == null ? null : dto.WeightCut);
        double rateCut = cvDouble(dto == null ? null : dto.RateCut);
        if (!toBool(ci(h, "IsWeightCutCompulsory"))) throw new IllegalArgumentException("Weight Cut can't be changed when it is not compulsory.");   // D7
        if (!toBool(ci(h, "IsRateCutCompulsory"))) throw new IllegalArgumentException("Rate Cut can't be changed when it is not compulsory.");
        repo.cutsUpdate(id, u.getId(), rateCut, weightCut);
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", true);
        m.put("message", "Data updated successfully.");
        return m;
    }

    /**
     * grdhistory_SelectionChanged (:2653-2693): the "Editable After Approval" rows of the detail grid whose
     * ResultValue changed are written with UpdatePurchaseAnalysisParameterResultValue, one call per row in
     * one transaction (bll:1052).
     */
    @Transactional
    public Map<String, Object> updateParameterResults(int id, List<PurchaseAnalysisDto.ParamResult> list) {
        UserAccount u = ctx.requireAccountingUser();
        if (!rights.has(SCREEN_NAME, "update")) throw new AccessDeniedException("You do not have the Update right for this screen.");   // W8
        if (ownedHeader(u, id) == null) throw new IllegalArgumentException("Record Not Found");    // W2
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", true);
        if (list == null || list.isEmpty()) { m.put("updated", 0); return m; }
        Map<Integer, Map<String, Object>> stored = new HashMap<>();
        for (Map<String, Object> d : repo.details(id)) stored.put(toInt(ci(d, "Id")), d);
        for (PurchaseAnalysisDto.ParamResult p : list) {
            Map<String, Object> d = stored.get(i(p.Id));
            if (d == null || !toBool(ci(d, "IsEditableAfterApproval"))) {
                throw new IllegalArgumentException("Analysis Parameter is not editable after approval");   // W3
            }
        }
        for (PurchaseAnalysisDto.ParamResult p : list) repo.updateParameterResult(i(p.Id), d(p.ResultValue), nz(p.Remarks));
        m.put("updated", list.size());
        m.put("message", "Data Updated Successfully");
        return m;
    }

    // ============================================================================= read

    /**
     * ReadById (:1989) and the History detail (:2695-2718) — the header (GenericProvider.Get of the
     * 'ReadById' row), LabSampleDetail ('ReadDetailByHeaderId') and each row's sub-parameters
     * ('ReadSubParamsDetailByHeaderId', InvParentParameterId = InvLabAnalysisItemsId — DAL 0359).
     */
    public Map<String, Object> load(int id) {
        UserAccount u = requireView();
        Map<String, Object> h = ownedHeader(u, id);
        if (h == null) throw new IllegalArgumentException("Record Not Found");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("Id", id);
        out.put("DocNo", toInt(ci(h, "DocNo")));
        out.put("DocDate", plain(ci(h, "DocDate")));
        out.put("BiltyNo", str(ci(h, "BiltyNo")));
        out.put("BiltyDate", plain(ci(h, "BiltyDate")));
        out.put("CityName", str(ci(h, "CityName")));
        out.put("PurchaseType", str(ci(h, "PurchaseType")));
        out.put("UserNameAusr", str(ci(h, "UserNameAusr")));
        out.put("ApprovalStatus", str(ci(h, "ApprovalStatus")));
        out.put("VehicleNo", str(ci(h, "VehicleNo")));
        out.put("PurchaseOrderId", toInt(ci(h, "PurchaseOrderId")));
        out.put("SupCustCode", str(ci(h, "SupCustCode")));
        out.put("NoofBagsInspection", clr(toDouble(ci(h, "NoofBagsInspection"))));
        out.put("RefDocumentTypeId", toInt(ci(h, "RefDocumentTypeId")));
        out.put("WeightCutOnId", toInt(ci(h, "WeightCutOnId")));
        out.put("WeightCutUom", clr(toDouble(ci(h, "WeightCutUom"))));
        out.put("IsRateCutCompulsory", toBool(ci(h, "IsRateCutCompulsory")));
        out.put("IsWeightCutCompulsory", toBool(ci(h, "IsWeightCutCompulsory")));
        out.put("GatePassInwardId", toInt(ci(h, "GatePassInwardId")));
        out.put("GpSrNo", str(toInt(ci(h, "GpSrNo"))));
        out.put("PartyLotRefNo", str(ci(h, "PartyLotRefNo")));
        out.put("ItemId", toInt(ci(h, "ItemId")));
        out.put("ItemName", str(ci(h, "ItemName")));
        out.put("PoDetailId", toInt(ci(h, "PoDetailId")));
        out.put("SupplierCustomerId", toInt(ci(h, "SupplierCustomerId")));     // the gate pass's (sql:165091)
        out.put("PartyName", str(ci(h, "PartyName")));
        out.put("ParentcategoryId", toInt(ci(h, "ParentcategoryId")));
        out.put("Parentcategory", str(ci(h, "Parentcategory")));
        out.put("RateUomId", toInt(ci(h, "RateUomId")));
        out.put("RateUom", str(ci(h, "RateUom")));
        out.put("InvLabSampleLogRegisterId", toInt(ci(h, "InvLabSampleLogRegisterId")));
        out.put("LabSampleNo", str(toInt(ci(h, "LabSampleNo"))));
        out.put("IsAccepted", toBool(ci(h, "IsAccepted")));
        out.put("Crop", str(ci(h, "Crop")));
        out.put("WarehouseId", toInt(ci(h, "WarehouseId")));
        out.put("PackingTypeId", toInt(ci(h, "PackingTypeId")));
        out.put("JobLotId", toInt(ci(h, "JobLotId")));
        out.put("PestResultId", toInt(ci(h, "PestResultId")));
        out.put("PestStatusId", toInt(ci(h, "PestStatusId")));
        out.put("InvLabAnalysisGroup", toInt(ci(h, "InvLabAnalysisGroup")));
        out.put("RemarksHeader", str(ci(h, "RemarksHeader")));
        out.put("Premium", clr(toDouble(ci(h, "Premium"))));
        out.put("DeductionRate", clr(toDouble(ci(h, "DeductionRate"))));
        out.put("ItemRate", clr(toDouble(ci(h, "ItemRate"))));
        out.put("DeductionWeight", clr(toDouble(ci(h, "DeductionWeight"))));
        out.put("QtyForWtCut", clr(toDouble(ci(h, "QtyForWtCut"))));
        out.put("AnalystName", str(ci(h, "AnalystName")));
        out.put("PricingScheduleId", str(toInt(ci(h, "PricingScheduleId"))));
        out.put("HasAnalysisPic", !str(ci(h, "Pic1Pathe")).isEmpty());
        out.put("HasCookingPic", !str(ci(h, "CookingPic")).isEmpty());
        out.put("rows", storedRows(id));
        return out;
    }

    /** LabSampleDetail with InvLabAnalysisPurchaseSubParamsDetailslist, as DAL 0359 GetData builds them. */
    private List<Map<String, Object>> storedRows(int id) {
        List<Map<String, Object>> subs = new ArrayList<>();
        for (Map<String, Object> s : repo.subParams(id)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(s, "Id")));
            o.put("InvParentParameterId", toInt(ci(s, "InvParentParameterId")));
            o.put("SubParameterId", toInt(ci(s, "SubParameterId")));
            o.put("SubParameterName", str(ci(s, "SubParameterName")));
            o.put("ResultValue", toDouble(ci(s, "ResultValue")));
            subs.add(o);
        }
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.details(id)) {
            int itemsId = toInt(ci(r, "InvLabAnalysisItemsId"));
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("InvLabAnalysisItemsId", itemsId);
            o.put("InvLabGroupAnalysisStandardsId", toInt(ci(r, "InvLabGroupAnalysisStandardsId")));
            o.put("AnalysisStutusId", toInt(ci(r, "AnalysisStutusId")));
            o.put("AnalysisGroupDescription", str(ci(r, "AnalysisGroupDescription")));
            o.put("AnalysisParameterDescription", str(ci(r, "AnalysisParameterDescription")));
            o.put("MinValue", toDouble(ci(r, "MinValue")));
            o.put("MaxValue", toDouble(ci(r, "MaxValue")));
            o.put("IsEditableAfterApproval", toBool(ci(r, "IsEditableAfterApproval")));
            o.put("InAnalysisResult", toDouble(ci(r, "InAnalysisResult")));
            Object remarks = ci(r, "RemarksDetail");
            o.put("RemarksDetail", remarks == null ? null : String.valueOf(remarks));
            o.put("ParamsMinValue", toDouble(ci(r, "ParamsMinValue")));
            o.put("ParamsMaxValue", toDouble(ci(r, "ParamsMaxValue")));
            Object pm = ci(r, "PmLabStatus");
            o.put("PmLabStatus", pm == null ? null : String.valueOf(pm));
            List<Map<String, Object>> mine = new ArrayList<>();
            for (Map<String, Object> s : subs) if (toInt(s.get("InvParentParameterId")) == itemsId) mine.add(new LinkedHashMap<>(s));
            o.put("subs", mine);
            out.add(o);
        }
        return out;
    }

    /**
     * btnprint / history Print / the "Peview" tick after a save: the record must be the user's (W2).
     * Only the toolbar button is tied to the Print right on the desktop (btnprint.Enabled, :559).
     */
    public Map<String, Object> checkPrint(int id, boolean toolbar) {
        UserAccount u = ctx.requireAccountingUser();
        if (toolbar && !rights.has(SCREEN_NAME, "print")) throw new AccessDeniedException("You do not have the Print right for this screen.");
        if (id <= 0 || ownedHeader(u, id) == null) throw new IllegalArgumentException("Record Not Found For Display");   // :3175
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", true);
        m.put("id", id);
        m.put("template", RPT_653);
        return m;
    }

    /** The stored Analysis Pic (which = "analysis") or Cooking Pic ("cooking") — {name, bytes}; null when there is none. */
    public Object[] picture(int id, String which) {
        UserAccount u = requireView();
        Map<String, Object> h = ownedHeader(u, id);
        if (h == null) throw new IllegalArgumentException("Record Not Found");
        String name = str(ci(h, "cooking".equals(which) ? "CookingPic" : "Pic1Pathe")).trim();
        if (name.isEmpty()) return null;
        return new Object[] { name, attachments.read(u, name) };
    }

    // ============================================================================= save

    /** btnSave_Click (:1964) / btnUpdate_Click (:1977) -> Insert() (:1799). */
    @Transactional
    public Map<String, Object> save(PurchaseAnalysisDto dto) {
        UserAccount u = ctx.requireAccountingUser();
        int recId = i(dto.Id);
        if (recId == 0 && !rights.has(SCREEN_NAME, "save")) throw new AccessDeniedException("You do not have the Save right for this screen.");
        if (recId != 0 && !rights.has(SCREEN_NAME, "update")) throw new AccessDeniedException("You do not have the Update right for this screen.");
        Map<String, Object> existing = null;
        if (recId != 0) {
            existing = ownedHeader(u, recId);                                                        // W2
            if (existing == null) throw new IllegalArgumentException("Record Not Found");
        }
        int fy = ctx.currentFinancialYearId();
        boolean rateFromSchedule = rateFromSchedule(u);

        // ------------------------------------------------------------ the gate pass (txtgatepassno_Leave, :823)
        int gpId = existing != null ? toInt(ci(existing, "GatePassInwardId")) : i(dto.GatePassInwardId);
        if (dto.GatePassInwardId == null || i(dto.GatePassInwardId) == 0 || gpId != i(dto.GatePassInwardId)) {
            throw new IllegalArgumentException("Please GpNo GatePass No");                           // :1428
        }
        List<Map<String, Object>> gpRows = repo.gatePassDetail(u, gpId);
        if (gpRows.isEmpty()) throw new IllegalArgumentException("Please GpNo GatePass No");         // W2
        if (existing == null) {                                                                      // W3: LimitToList over GpNoFill
            boolean offered = false;
            for (Map<String, Object> g : repo.gatePassesForAnalysis(u, fy, 0, 0)) if (toInt(ci(g, "Id")) == gpId) { offered = true; break; }
            if (!offered) throw new IllegalArgumentException("Please GpNo GatePass No");
        }
        Map<String, Object> gp0 = gpRows.get(0);
        int ref = toInt(ci(gp0, "RefDocumentTypeId"));                                               // :847
        List<Map<String, Object>> items = itemsFor(u, ref, gpRows);

        // ----------------------------------------------------------------- formvalidation() (:1424-1542)
        int itemId = i(dto.ItemId);
        Map<String, Object> item = null;
        if (dto.ItemId != null && itemId != 0) {
            for (Map<String, Object> it : items) {
                if (toInt(it.get("Id")) != itemId) continue;
                if (ref == 41 && toInt(it.get("PoDetailId")) != i(dto.PoDetailId)) continue;         // the selected PO line
                item = it;
                break;
            }
        }
        int groupId = i(dto.AnalysisGroupId);
        if (dto.AnalysisGroupId == null || groupId == 0) throw new IllegalArgumentException("Please Select Analysis Group No");
        if (item == null) throw new IllegalArgumentException("Item Field Required");
        boolean groupOk = false;
        for (Map<String, Object> g : analysisGroupsFor(u, toInt(item.get("ParentcategoryId")))) {
            if (toInt(g.get("Id")) == groupId) { groupOk = true; break; }
        }
        if (!groupOk) throw new IllegalArgumentException("Please Select Analysis Group No");         // W3
        if (cvInt(dto.NoofBagsInspection) == 0) throw new IllegalArgumentException("NoofBags Field Required");
        if (dto.PackingTypeId == null) throw new IllegalArgumentException("Packing Type Field is Required");
        int packingTypeId = i(dto.PackingTypeId);
        if (packingTypeId != 0 && !contains(packingTypes(), packingTypeId)) throw new IllegalArgumentException("Packing Type Field is Required");   // W3

        double itemRate = cvDouble(dto.ItemRate);
        double addRate = cvDouble(dto.AddRate);
        double lessRate = cvDouble(dto.LessRate);
        double netRate = netRate(itemRate, addRate, lessRate);                                       // W4, NetRateCalculation (:3227)
        int rateUomId = i(dto.RateUomId);
        if (ref == 106 && rateFromSchedule) {
            if (dto.RateUomId == null || rateUomId == 0) throw new IllegalArgumentException("RateUom Field Required");
            if (itemRate == 0d) throw new IllegalArgumentException("ItemRate Field Required");
            if (netRate == 0d) throw new IllegalArgumentException("NetRate Field Required");
        }
        if (itemRate > 0d && netRate == 0d) throw new IllegalArgumentException("NetRate Field Required");
        String analystName = nz(dto.AnaylstName);
        if (analystName.trim().isEmpty()) throw new IllegalArgumentException("AnaylstName field required");
        int weightCutInt = cvInt(dto.WeightCut);                                                     // D1
        if (weightCutInt != 0 && weightCutInt > 10) throw new IllegalArgumentException("WeightCut / Touch Cannot Be Greater Than 10");
        int weightCutUomInt = cvInt(dto.WeightCutUom);                                               // D1
        if (weightCutUomInt > 0 && (weightCutUomInt < 40 || weightCutUomInt > 105)) throw new IllegalArgumentException("WeightCutUom Must Be Between 40 and 105");
        BigDecimal gpQty = cvDecimal(netText(ci(gp0, "GpQty")));                                     // txtGpQty (:849)
        BigDecimal qtyForWtCut = cvDecimal(dto.QtyForWtCut);
        if (qtyForWtCut.signum() > 0 && qtyForWtCut.compareTo(gpQty) > 0) {
            throw new IllegalArgumentException("Qty For Weight Cut / Touch Cannot Be Greater Than GatePass Qty");
        }
        if (dto.WarehouseId == null) throw new IllegalArgumentException("Warehouse field required");
        int warehouseId = i(dto.WarehouseId);
        if (!contains(warehouses(u), warehouseId)) throw new IllegalArgumentException("Warehouse field required");      // W3
        int weightCutOnId = i(dto.WeightCutOnId);
        if (weightCutInt > 0 && (dto.WeightCutOnId == null || weightCutOnId == 0)) throw new IllegalArgumentException("Weight Cut On field required");
        if (weightCutOnId != 0 && !contains(weightCutOn(), weightCutOnId)) throw new IllegalArgumentException("Weight Cut On field required");   // W3
        if (dto.LabStatusId == null) throw new IllegalArgumentException("Status field required");
        if (dto.PestResultId == null) throw new IllegalArgumentException("Pest Result field required");
        if (dto.PestStatusId == null) throw new IllegalArgumentException("Pest Status field required");
        int pestResultId = i(dto.PestResultId), pestStatusId = i(dto.PestStatusId);
        if (!contains(pestResults(), pestResultId)) throw new IllegalArgumentException("Pest Result field required");   // W3
        if (!contains(pestStatuses(), pestStatusId)) throw new IllegalArgumentException("Pest Status field required");  // W3
        int jobLotId = i(dto.JobLotId);
        if (jobLotId != 0 && !contains(jobLots(u), jobLotId)) throw new IllegalArgumentException("Job Lot Not Found");   // W3
        boolean accepted;
        if ("Accepted".equals(nz(dto.LabStatusText))) accepted = true;                               // :1867
        else if ("Rejected".equals(nz(dto.LabStatusText))) accepted = false;
        else throw new IllegalArgumentException("Status field required");                            // W3 (LimitToList)

        // ------------------------------------------------------------------- the parameter rows (detaillst)
        int storedGroup = existing == null ? 0 : toInt(ci(existing, "InvLabAnalysisGroup"));
        /* cmbanalysisgroup_Leave (:1154): an opened record keeps its stored rows while the group is unchanged. */
        List<Map<String, Object>> authoritative = existing != null && storedGroup == groupId ? storedRows(recId) : parameterRows(u, groupId);
        Map<Integer, PurchaseAnalysisDto.Row> posted = new HashMap<>();
        List<PurchaseAnalysisDto.Row> postedRows = dto.rows == null ? new ArrayList<>() : dto.rows;
        Set<Integer> known = new HashSet<>();
        for (Map<String, Object> a : authoritative) known.add(toInt(a.get("InvLabAnalysisItemsId")));
        for (PurchaseAnalysisDto.Row r : postedRows) {
            if (r == null) continue;
            int pid = i(r.InvLabAnalysisItemsId);
            if (!known.contains(pid) || posted.put(pid, r) != null) throw new IllegalArgumentException("Analysis Parameter Not Found");   // W3
        }
        List<Map<String, Object>> detaillst = new ArrayList<>();
        for (Map<String, Object> a : authoritative) {
            PurchaseAnalysisDto.Row r = posted.get(toInt(a.get("InvLabAnalysisItemsId")));
            if (r == null) continue;                                                                 // the row was deleted from the grid
            Map<String, Object> row = new LinkedHashMap<>(a);
            row.put("InAnalysisResult", d(r.InAnalysisResult));
            row.put("RemarksDetail", r.RemarksDetail);
            List<Map<String, Object>> subs = new ArrayList<>();
            Map<Integer, Double> postedSubs = new HashMap<>();
            if (r.subs != null) for (PurchaseAnalysisDto.Sub s : r.subs) if (s != null) postedSubs.put(i(s.SubParameterId), d(s.ResultValue));
            @SuppressWarnings("unchecked")
            List<Map<String, Object>> aSubs = (List<Map<String, Object>>) a.get("subs");
            for (Map<String, Object> s : aSubs) {
                Map<String, Object> sub = new LinkedHashMap<>(s);
                Double v = postedSubs.get(toInt(s.get("SubParameterId")));
                if (v != null) sub.put("ResultValue", v);
                subs.add(sub);
            }
            row.put("subs", subs);
            detaillst.add(row);
        }
        if (detaillst.isEmpty()) throw new IllegalArgumentException("Grid Record Not Found");       // :1811

        // ------------------------------------------------------------------------ the Yes/No prompt (:1816/:1822)
        if (!Boolean.TRUE.equals(dto.confirm)) throw new ConfirmRequired(recId > 0 ? "Are you sure to Update?" : "Are you sure to Save?");

        // --------------------------------------------------------------------------------- :1826-1857
        double result = 0d;
        for (Map<String, Object> row : detaillst) {
            double totalValueParent = toDouble(row.get("InAnalysisResult"));
            double paramsMin = toDouble(row.get("ParamsMinValue"));
            double paramsMax = toDouble(row.get("ParamsMaxValue"));
            String name = str(row.get("AnalysisParameterDescription"));
            if (totalValueParent > 0d) {                                                             // D5
                if (totalValueParent > paramsMax) throw new IllegalArgumentException("Analysis Result is not Greater than Maximum Value " + name + ": " + clr(paramsMax));
                if (totalValueParent < paramsMin) throw new IllegalArgumentException("Analysis Result is not Less than Minimum Value " + name + ": " + clr(paramsMin));
            }
            result += totalValueParent;
            double totalValueSub = 0d;
            @SuppressWarnings("unchecked")
            List<Map<String, Object>> subs = (List<Map<String, Object>>) row.get("subs");
            for (Map<String, Object> s : subs) totalValueSub += toDouble(s.get("ResultValue"));
            if (totalValueSub > 0d && totalValueParent != totalValueSub && !subs.isEmpty()) {       // D4
                throw new IllegalArgumentException("Total Value of all sub parameters for " + name + " must be equal to value of " + name);
            }
        }
        if (result == 0d) throw new IllegalArgumentException("ResultValue cannot be equal to zero");

        // -------------------------------------------------------------------- references the form binds (W3/W5/W6)
        if (rateUomId != 0) {
            boolean ok = existing != null && toInt(ci(existing, "RateUomId")) == rateUomId;
            if (!ok && (ref == 106 || ref == 110)) {                                                 // cmbitem_Leave binds the list for these two only (:1267)
                ok = contains(rateUomRows(u, itemId, rateFromSchedule), rateUomId);
                if (!ok) throw new IllegalArgumentException("RateUom Field Required");
            }
            if (!ok) rateUomId = 0;                                                                  // W9: a list left over from another gate pass
        }
        int scheduleId = cvInt(dto.ScheduleId);
        if (rateFromSchedule && (ref == 106 || ref == 110) && itemRate != 0d) {                      // W5
            boolean ok = existing != null && toDouble(ci(existing, "ItemRate")) == itemRate && toInt(ci(existing, "PricingScheduleId")) == scheduleId;
            if (!ok) {
                for (Map<String, Object> s : repo.itemRateByItemAndUom(u, itemId, rateUomId)) {
                    if (toInt(ci(s, "Id")) == scheduleId && toDouble(ci(s, "ItemRate")) == itemRate) { ok = true; break; }
                }
            }
            if (!ok) throw new IllegalArgumentException("ItemRate is not the Item Pricing Schedule rate of this Item and Rate Uom");
        }
        int sampleId = i(dto.SampleAnalysisId);
        if (sampleId != 0) {
            boolean ok = existing != null && toInt(ci(existing, "InvLabSampleLogRegisterId")) == sampleId;
            if (!ok && ref == 41) {
                for (Map<String, Object> g : gpRows) {
                    if (toInt(ci(g, "PoDetailId")) == toInt(item.get("PoDetailId")) && toInt(ci(g, "InvLabSampleAnalysisHeaderId")) == sampleId) { ok = true; break; }
                }
            }
            if (!ok) throw new IllegalArgumentException("Sample Analysis Not Found");
        }
        int purchaseOrderId = ref == 41 ? toInt(ci(gp0, "PurchaseOrderId"))                          // :862
                : existing != null ? toInt(ci(existing, "PurchaseOrderId")) : 0;                     // W6

        // ------------------------------------------------------------------------------ pictures (W7)
        String pic1 = picture(u, dto.AnalysisPic, existing == null ? "" : str(ci(existing, "Pic1Pathe")));
        String cookingPic = picture(u, dto.CookingPic, existing == null ? "" : str(ci(existing, "CookingPic")));

        // ------------------------------------------------------------------------ the header (:1858-1917)
        Timestamp now = Timestamp.valueOf(LocalDateTime.now().withNano(0));
        int docNo = existing != null ? toInt(ci(existing, "DocNo")) : repo.generateCode(u, fy, DOCUMENT_TYPE_ID);        // W1
        double deductionRate = ref == 106 ? lessRate : cvDouble(dto.RateCut);                        // :1884
        double deductionWeight = cvDouble(dto.WeightCut);
        boolean wtLater = existing != null ? toBool(ci(existing, "IsWeightCutCompulsory")) : Boolean.TRUE.equals(dto.WtCutWillApply);      // W4
        boolean rateLater = existing != null ? toBool(ci(existing, "IsRateCutCompulsory")) : Boolean.TRUE.equals(dto.RateCutWillApply);   // W4
        Map<String, Object> head = new LinkedHashMap<>();                                            // model 0620, declaration order
        head.put("IsAccepted", accepted);
        head.put("IsApproved", false);
        head.put("IsAcceptedApproved", false);
        head.put("ApprovedDate", now);
        head.put("DocDate", pickerDate(dto.DocDate));
        head.put("EntryDate", now);
        head.put("ModifyDate", now);
        head.put("DeductionRate", deductionRate);
        head.put("DeductionWeight", deductionWeight);
        head.put("QtyForWtCut", cvDouble(dto.QtyForWtCut));
        head.put("ItemRate", itemRate);
        head.put("NoofBagsInspection", cvDouble(dto.NoofBagsInspection));
        head.put("Premium", addRate);
        head.put("WeightCutUom", cvDouble(dto.WeightCutUom));
        head.put("WeightCutOnId", weightCutOnId);
        head.put("ApprovedUserId", 0);
        head.put("DocumentTypeId", DOCUMENT_TYPE_ID);
        head.put("BranchesId", PurchaseAnalysisRepository.branch(u));
        head.put("CompanyId", u.getCompanyId());
        head.put("DocNo", docNo);
        head.put("EntryUser", u.getId());
        head.put("GatePassInwardId", gpId);
        head.put("Id", recId);
        head.put("InvLabAnalysisGroup", groupId);
        head.put("InvLabSampleLogRegisterId", sampleId);
        head.put("ItemId", itemId);
        head.put("ModifyUser", u.getId());
        head.put("OrganizationId", u.getOrganizationId());
        head.put("ProjectsId", 0);
        head.put("PurchaseOrderId", purchaseOrderId);
        head.put("PoDetailId", toInt(item.get("PoDetailId")));                                       // :1877
        head.put("SupplierCustomerId", toInt(ci(gp0, "SupplierCustomerId")));                        // cmbPartyName.Value (:1861)
        head.put("PricingScheduleId", scheduleId);
        head.put("RateUomId", rateUomId);
        head.put("PackingTypeId", packingTypeId);
        head.put("WarehouseId", warehouseId);
        head.put("FinancialYearId", fy);
        head.put("JobLotId", jobLotId);
        head.put("AnalystName", analystName);
        head.put("CookingPic", cookingPic);
        head.put("Crop", nz(dto.Crop));                                                              // D2
        head.put("Pic1Pathe", pic1);
        head.put("RemarksHeader", nz(dto.Remarks));
        head.put("SupCustCode", str(ci(gp0, "OrderNo")));                                            // txtOrderNo (:842)
        head.put("PartyLotRefNo", nz(dto.PartyLotRefNo));
        head.put("IsWeightCutCompulsory", deductionWeight == 0d && wtLater);                         // :1910
        head.put("IsRateCutCompulsory", deductionRate == 0d && rateLater);                           // :1914
        head.put("RateApplied", false);
        head.put("RateAppliedFromId", 0);
        head.put("RateAppliedUserId", 0);
        head.put("WeightApplied", false);
        head.put("WeightAppliedFromId", 0);
        head.put("WeightAppliedUserId", 0);
        head.put("PremiumLessPercentOnRate", 0d);
        head.put("RateCutPercentOnOrderRate", 0d);
        head.put("PestResultId", pestResultId);
        head.put("PestStatusId", pestStatusId);

        int id = setData(head, detaillst, recId == 0 ? "Sp_InvLabAnalysisPurchaseHeader_Insert" : "Sp_InvLabAnalysisPurchaseHeader_Update");
        LOG.debug("Purchase Analysis {} saved (gate pass {})", id, gpId);
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", true);
        m.put("message", recId > 0 ? "Update Successfully" : "Save Successfully");                  // :1940 / :1945
        m.put("id", id);
        m.put("gatePassId", gpId);
        return m;
    }

    /**
     * DAL 0359 SetData — the header procedure, then per parameter row Sp_InvLabAnalysisPurchaseDetail_Insert
     * and, for each of its sub-parameters, Sp_InvLabAnalysisPurchaseSubParamsDetail_Insert; one transaction.
     * The Update procedure returns no row, so the id stays the record's (num = obj.Id).
     */
    private int setData(Map<String, Object> head, List<Map<String, Object>> detaillst, String procName) {
        int recId = toInt(head.get("Id"));
        int num = repo.setProc(procName, head);
        int id = num > 0 ? num : recId;
        if (id <= 0) throw new IllegalStateException("Save returned no document id.");
        for (Map<String, Object> row : detaillst) {
            Map<String, Object> d = new LinkedHashMap<>();                                           // model 0619, declaration order
            d.put("Id", toInt(row.get("Id")));
            d.put("InvLabAnalysisPurchaseHeaderId", id);
            d.put("InvLabGroupAnalysisStandardsId", toInt(row.get("InvLabGroupAnalysisStandardsId")));
            d.put("InvLabAnalysisItemsId", toInt(row.get("InvLabAnalysisItemsId")));
            d.put("AnalysisStutusId", toInt(row.get("AnalysisStutusId")));
            d.put("InAnalysisResult", toDouble(row.get("InAnalysisResult")));
            d.put("RemarksDetail", row.get("RemarksDetail"));                                        // CLR null -> not sent
            int detailId = repo.setProc("Sp_InvLabAnalysisPurchaseDetail_Insert", d);
            @SuppressWarnings("unchecked")
            List<Map<String, Object>> subs = (List<Map<String, Object>>) row.get("subs");
            if (subs == null || subs.isEmpty()) continue;
            for (Map<String, Object> s : subs) {
                Map<String, Object> p = new LinkedHashMap<>();                                       // model 0608, declaration order
                p.put("Id", toInt(s.get("Id")));
                p.put("InvLabAnalysisPurchaseDetailId", detailId);
                p.put("InvLabAnalysisPurchaseHeaderId", id);
                p.put("InvParentParameterId", toInt(s.get("InvParentParameterId")));
                p.put("SubParameterId", toInt(s.get("SubParameterId")));
                p.put("ResultValue", toDouble(s.get("ResultValue")));
                repo.setProc("Sp_InvLabAnalysisPurchaseSubParamsDetail_Insert", p);
            }
        }
        return id;
    }

    /**
     * UniqFileName / UniqFileName2 (:1881-1882): "keep" the stored name, "clear" (Reset, :2941 / :2984) or a
     * newly browsed .jpg / .jpeg / .png (TakePhotoButton_Click :2861, btnCapturecooking_Click :2887).
     */
    private String picture(UserAccount u, PurchaseAnalysisDto.Picture p, String stored) {
        String action = p == null || p.action == null ? "keep" : p.action;
        if ("clear".equals(action)) return "";
        if (!"new".equals(action)) return stored == null ? "" : stored;
        String name = nz(p.fileName).trim();
        String lower = name.toLowerCase(Locale.ROOT);
        if (!(lower.endsWith(".jpg") || lower.endsWith(".jpeg") || lower.endsWith(".png"))) {
            throw new IllegalArgumentException("Only .jpg, .jpeg and .png pictures can be attached");
        }
        if (repo.config(u, "Attachment Folder Path").trim().isEmpty() && !toBoolText(repo.config(u, "IsVpsAttachmentsServiceOn"))) {
            throw new IllegalArgumentException("Attachment FilePath Not Configure Please Check!");   // :2879
        }
        byte[] bytes;
        try { bytes = Base64.getDecoder().decode(nz(p.dataBase64)); }
        catch (IllegalArgumentException e) { throw new IllegalArgumentException("The picture could not be read"); }
        return attachments.store(u, name, bytes);
    }

    // ======================================================================== arithmetic

    /** NetRateCalculation (:3227): (rate + add - less).ToString("#,#") read back — a whole number, away from zero. */
    private static double netRate(double itemRate, double addRate, double lessRate) {
        double v = itemRate + addRate - lessRate;
        if (!Double.isFinite(v)) return 0d;
        return new BigDecimal(clr(v)).setScale(0, RoundingMode.HALF_UP).doubleValue();
    }

    // =========================================================================== helpers

    private UserAccount requireView() {
        UserAccount u = ctx.requireAccountingUser();
        if (!rights.has(SCREEN_NAME, "view")) throw new AccessDeniedException("You do not have the View right for Purchase Analysis.");
        return u;
    }

    /** The header must exist, be DocumentTypeId 303 and belong to the user's organization and company. */
    private Map<String, Object> ownedHeader(UserAccount u, int id) {
        if (id <= 0) return null;
        Map<String, Object> h = repo.header(id);
        if (h == null) return null;
        if (toInt(ci(h, "DocumentTypeId")) != DOCUMENT_TYPE_ID) return null;
        if (toInt(ci(h, "CompanyId")) != i(u.getCompanyId())) return null;
        if (toInt(ci(h, "OrganizationId")) != i(u.getOrganizationId())) return null;
        return h;
    }

    private static Map<String, Object> idName(int id, String name) {
        Map<String, Object> o = new LinkedHashMap<>();
        o.put("Id", id);
        o.put("Name", name);
        return o;
    }

    private static boolean contains(List<Map<String, Object>> rows, int id) {
        for (Map<String, Object> r : rows) if (toInt(r.get("Id")) == id) return true;
        return false;
    }

    /** A picker's Value — the chosen day at the current time of day. */
    private static Timestamp pickerDate(String day) {
        LocalDate d = isBlank(day) ? LocalDate.now() : LocalDate.parse(day.trim().substring(0, 10));
        return Timestamp.valueOf(LocalDateTime.of(d, LocalTime.now().withNano(0)));
    }

    /** Date values leave as local "yyyy-MM-dd HH:mm:ss" text, never as a JSON timestamp. */
    static Object plain(Object v) {
        if (v instanceof Timestamp) return ((Timestamp) v).toLocalDateTime().withNano(0).toString().replace('T', ' ');
        if (v instanceof java.sql.Date) return ((java.sql.Date) v).toLocalDate().toString() + " 00:00:00";
        if (v instanceof java.util.Date) return new Timestamp(((java.util.Date) v).getTime()).toLocalDateTime().withNano(0).toString().replace('T', ' ');
        if (v instanceof LocalDateTime) return ((LocalDateTime) v).withNano(0).toString().replace('T', ' ');
        if (v instanceof LocalDate) return v.toString() + " 00:00:00";
        return v;
    }

    /** A float column as the text an untyped DataTable column shows it (double.ToString()); NULL is "". */
    private static String dbText(Object v) {
        if (v == null) return "";
        if (v instanceof Number) return clr(((Number) v).doubleValue());
        return String.valueOf(v);
    }

    /** Conversion.ToString of a database value: an integer as is, a decimal with its scale, a float as double.ToString(). */
    private static String netText(Object v) {
        if (v == null) return "";
        if (v instanceof BigDecimal) return ((BigDecimal) v).toPlainString();
        if (v instanceof Double || v instanceof Float) return clr(((Number) v).doubleValue());
        return String.valueOf(v);
    }

    /** .NET double.ToString() — 15 significant digits, no trailing zeros. */
    static String clr(double v) {
        if (v == 0d) return "0";
        if (Double.isNaN(v) || Double.isInfinite(v)) return String.valueOf(v);
        return new BigDecimal(v).round(new MathContext(15)).stripTrailingZeros().toPlainString();
    }

    private static boolean toBool(Object v) {
        if (v instanceof Boolean) return (Boolean) v;
        if (v instanceof Number) return ((Number) v).intValue() != 0;
        String t = v == null ? "" : String.valueOf(v).trim();
        return "true".equalsIgnoreCase(t) || "1".equals(t);
    }

    /** Conversion.ToBool on a configuration value ("1" / "True"). */
    private static boolean toBoolText(String v) { return toBool(v); }

    /** Conversion.ToInt(text) = Convert.ToInt32(string): an Int32 literal (no separators, no decimals) or 0. */
    private static int cvInt(String s) {
        if (s == null) return 0;
        try { return Integer.parseInt(s.trim()); } catch (NumberFormatException e) { return 0; }
    }

    /** Conversion.ToDouble(text) — thousands separators allowed; anything unparseable (or infinite) is 0. */
    private static double cvDouble(String s) {
        if (s == null || s.trim().isEmpty()) return 0d;
        try { double v = Double.parseDouble(s.trim().replace(",", "")); return Double.isFinite(v) ? v : 0d; }
        catch (NumberFormatException e) { return 0d; }
    }

    /** Conversion.ToDecimal(text) — thousands separators allowed; anything unparseable is 0. */
    private static BigDecimal cvDecimal(String s) {
        if (s == null || s.trim().isEmpty()) return BigDecimal.ZERO;
        String t = s.trim().replace(",", "");
        if (!t.matches("[+-]?(\\d+(\\.\\d*)?|\\.\\d+)")) return BigDecimal.ZERO;
        try { return new BigDecimal(t); } catch (NumberFormatException e) { return BigDecimal.ZERO; }
    }

    private static int i(Integer v) { return v == null ? 0 : v; }
    private static double d(Double v) { return v == null || !Double.isFinite(v) ? 0d : v; }
    private static boolean isBlank(String s) { return s == null || s.trim().isEmpty(); }
    private static String nz(String s) { return s == null ? "" : s; }
}
