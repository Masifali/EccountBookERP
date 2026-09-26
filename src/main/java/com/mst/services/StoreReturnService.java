package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.dto.ContraVoucherDto;
import com.mst.models.dto.StoreReturnDto;
import com.mst.repositories.StoreIssuanceRepository;
import com.mst.repositories.support.DesktopProc;
import com.mst.security.CurrentUserContext;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.lang.reflect.Field;
import java.math.BigDecimal;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.StoreIssuanceRepository.ci;
import static com.mst.repositories.StoreIssuanceRepository.clr;
import static com.mst.repositories.StoreIssuanceRepository.str;
import static com.mst.repositories.StoreIssuanceRepository.toDouble;
import static com.mst.repositories.StoreIssuanceRepository.toInt;
import static com.mst.repositories.support.DesktopProc.params;

/**
 * Screen 330 "Store Return" — {@code StoreReturn.cs}, DocumentTypeId 140.
 *
 *   BLL 0250 Architecture.BLL.PurchaseTrading.InvStoreReturn   (Save, MakeVoucher, GetByID, FormHistory)
 *   DAL 0219 Architecture.DAL.PurchaseTrading.InvStoreReturn   (SetData, GetAll)
 *   BLL 0252 InvGsStoreIssuanceHeader                           (items, credit accounts, last rate, loader)
 *
 * ---------------------------------------------------------------------------------------------
 * DESKTOP BEHAVIOUR REPRODUCED, NOT CORRECTED (the user's call)
 * ---------------------------------------------------------------------------------------------
 *  R1  Insert():1570 shows "Grid record not found" for an empty grid and then CONTINUES — a
 *      header with no details can be saved. The page shows the same message first.
 *  R2  A row is skipped when Conversion.ToInt(ItemQty) <= 0, so a quantity below 0.5 is silently
 *      dropped (Convert.ToInt32 rounds half to even).
 *  R3  Secondary Uom, its quantity and its rate, and a Credit Account, are required on every row
 *      even when StoreIssuanceFinancialEffect is off (:1612-1621).
 *  R4  Store Return has no Delete on the desktop; none is offered here either.
 *  R5  The DAL writes no USP_InventoryValidation and no DateLock check for Store Return — neither
 *      is added.
 *  R6  (web) Doc No is read-only; the server takes it from GenerateCode on a new save and from the
 *      stored header on update, never from the request. The desktop saves the number generated at
 *      reset - the same number unless another user saved in between (a duplicate on the desktop).
 *  R7  Desktop defects not reproduced: the loader closed with X still hides the entry bar with
 *      nothing loaded (:2931), and its Search appends duplicate header rows (dtMain never cleared,
 *      :131). The page leaves the bar visible and rebuilds the list.
 */
@Service
public class StoreReturnService {

    private static final Logger LOG = LoggerFactory.getLogger(StoreReturnService.class);

    public static final int DOCUMENT_TYPE_ID = 140;
    public static final String SCREEN_NAME = "StoreReturn";
    private static final String P_GETALL = "Sp_InvStoreReturn_GetAll";

    private final StoreIssuanceRepository repo;
    private final StoreScreenRights rights;
    private final CurrentUserContext ctx;
    private final JdbcTemplate jdbc;

    public StoreReturnService(StoreIssuanceRepository repo, StoreScreenRights rights,
                              CurrentUserContext ctx, JdbcTemplate jdbc) {
        this.repo = repo;
        this.rights = rights;
        this.ctx = ctx;
        this.jdbc = jdbc;
    }

    // ============================================================================== lookups

    public Map<String, Object> lookups() {
        UserAccount u = ctx.requireAccountingUser();
        int fy = ctx.currentFinancialYearId();
        int branchId = u.getBranchesId() == null ? 0 : u.getBranchesId();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rights", rights.of(SCREEN_NAME));
        out.put("docNo", nextDocNo(u, fy));
        out.put("storeFinancialEffects", toBool(repo.config(u, "StoreIssuanceFinancialEffect")));
        out.put("itemSearchByCode", toBool(repo.config(u, "ItemSearchByCode")));
        out.put("defaultDaysToLessFromHistoryFromDate", toInt(repo.config(u, "DefaultDaysToLessFromHistoryFromDate")));
        out.put("packingMaterialDefaultWarehouse", toInt(repo.config(u, "PackingMaterialDefaultWarehouse")));
        out.put("defaultWarehouseForStoreFlow", toInt(repo.config(u, "DefaultWarehouseForStoreFlow")));
        out.put("financialYearStart", financialYearStart(u, fy));

        /* ItemdtFillForReturntoStoreDbCall:477 — ItemCode comes from the column ItemCodeNew. */
        List<Map<String, Object>> items = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, StoreIssuanceRepository.P_HEADER_GETALL, params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "GetItemsForReturntoStore"))) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get("Id")));
            o.put("ItemName", str(r.get("ItemName")));
            o.put("ItemCode", str(r.get("ItemCodeNew")));
            o.put("ParentCategoryId", toInt(r.get("ParentCategoryId")));
            items.add(o);
        }
        out.put("items", items);

        /* CommonBindings.ItemConditionBindFromGlobal — ALL conditions, Id 4 included, unlike 321/322. */
        List<Map<String, Object>> conditions = new ArrayList<>();
        for (Map<String, Object> r : jdbc.queryForList("SELECT * FROM dbo.V_ItemCondition")) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("ItemCondition", str(ci(r, "ConditionStatus")));
            conditions.add(o);
        }
        out.put("itemConditions", conditions);

        /* CommonBindings.ItemUomFromGlobalBind — the global UOM schedule with the three base flags. */
        List<Map<String, Object>> uoms = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "usp_getAllUomsByCompanyId",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Active", 1))) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get("Id")));
            o.put("ItemId", toInt(r.get("ItemId")));
            o.put("UOMCode", str(r.get("UOMCode")));
            o.put("Equivalent", toDouble(r.get("Equivalent")));
            o.put("BasePackUom", toBoolObj(r.get("BasePackUom")));
            o.put("BaseRateUom", toBoolObj(r.get("BaseRateUom")));
            o.put("BaseSecondaryUom", toBoolObj(r.get("BaseSecondaryUom")));
            uoms.add(o);
        }
        out.put("uoms", uoms);
        out.put("racks", repo.racksWithWarehouseAndItem(u, branchId));
        out.put("departments", project(DesktopProc.rows(jdbc, "Sp_Department_GetAllMethod",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll")), "Id", "DepartmentName"));
        out.put("assets", project(DesktopProc.rows(jdbc, "Sp_FixedAssetsRegister_GetAllMethod",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll")), "Id", "AssetName"));

        /* AllCreditAccountsdtFillDbCall — GetDebitAccountForStoreReturn; @ItemId is not sent (0). */
        List<Map<String, Object>> cr = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, StoreIssuanceRepository.P_HEADER_GETALL, params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "GetDebitAccountForStoreReturn"))) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get("Id")));
            o.put("AccountTitle", str(r.get("AccountTitle")));
            o.put("ItemId", toInt(r.get("ItemId")));
            cr.add(o);
        }
        out.put("creditAccounts", cr);

        /* HistoryComboBindDbCall — five history filters from one procedure, split on ActivityType. */
        Map<String, List<Map<String, Object>>> hist = new LinkedHashMap<>();
        for (String a : new String[] { "Warehouse", "ItemName", "Department", "Asset", "CreditAccount" }) hist.put(a, new ArrayList<>());
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "USP_GetDataForDropDownFromInvStoreReturn", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "FinancialYearId", fy, "DocumentTypeIds", String.valueOf(DOCUMENT_TYPE_ID)))) {
            List<Map<String, Object>> t = hist.get(str(r.get("ActivityType")));
            if (t == null) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get("Id")));
            o.put("Name", str(r.get("ReferenceName")));
            t.add(o);
        }
        out.put("historyFilters", hist);
        return out;
    }

    /** BLL 0250 GenerateCode. */
    private int nextDocNo(UserAccount u, int fy) {
        Map<String, Object> p = params("Activity", "GenerateCode", "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(), "DocumentTypeId", DOCUMENT_TYPE_ID);
        if (fy != 0) p.put("FinancialYearId", fy);
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, P_GETALL, p);
        return r.isEmpty() ? 0 : toInt(r.get(0).get("DocNo"));
    }

    // ================================================================================ rates

    /** GetAvgRateByDocDateItemAndCondition:2266 — AvgRate only, no warehouse. */
    public double avgRate(int recId, int itemId, String docDate, int conditionId) {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> r = repo.avgRateAndStock(u, itemId, StoreIssuanceService.formDate(docDate, recId), conditionId,
                recId, recId > 0 ? DOCUMENT_TYPE_ID : 0, 0, 0);
        return r == null ? 0d : BigDecimal.valueOf(toDouble(r.get("AvgRate"))).setScale(3, java.math.RoundingMode.HALF_EVEN).doubleValue();
    }

    /** GetLastIssuanceRateByItemId:2236. */
    public double lastIssuanceRate(int itemId) {
        UserAccount u = ctx.requireAccountingUser();
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, StoreIssuanceRepository.P_HEADER_GETALL, params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "ItemId", itemId,
                "Activity", "GetLastIssuanceRateByItemId"));
        double v = r.isEmpty() ? 0d : toDouble(r.get(0).get("ItemRate"));
        return v > 0d ? v : 0d;
    }

    // =============================================================================== loader

    /**
     * frmLoadIssuanceForReturn / LoadIssuanceByRecId → BLL 0252 PendingLoaderDataForStoreReturn.
     * The dialog's FromDate is ActiveYr.Start_Period (midnight); its Todate keeps the picker's
     * construction time (DateTime.Now). LoadIssuanceByRecId sends the id and no dates.
     */
    public List<Map<String, Object>> pendingIssuances(String from, String to, int issuanceId) {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        int fy = ctx.currentFinancialYearId();
        if (fy != 0) p.put("FinancialYearId", fy);
        if (issuanceId != 0) p.put("IssuanceId", issuanceId);
        if (from != null && !from.trim().isEmpty()) p.put("FromDate", StoreIssuanceService.dateOnly(from));
        if (to != null && !to.trim().isEmpty()) p.put("ToDate", StoreIssuanceService.pickerDate(to));
        return DesktopProc.rows(jdbc, "usp_getStoreIssuanceLoaderDataForStoreReturn", p);
    }

    // ============================================================================== history

    public List<Map<String, Object>> history(String from, String to, int docNoFrom, int docNoTo, int warehouseId,
                                             int itemId, int departmentId, int assetId, int accountId) {
        UserAccount u = ctx.requireAccountingUser();
        boolean viewAll = rights.has(SCREEN_NAME, "viewAll");
        Map<String, Object> p = params("organizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocumentTypeId", DOCUMENT_TYPE_ID, "CanViewAllRecord", viewAll);
        if (!viewAll) p.put("EntryUser", u.getId());
        int fy = ctx.currentFinancialYearId();
        if (fy != 0) p.put("FinancialYearId", fy);
        if (from != null && !from.trim().isEmpty()) p.put("FromDate", StoreIssuanceService.pickerDate(from));
        if (to != null && !to.trim().isEmpty()) p.put("ToDate", StoreIssuanceService.pickerDate(to));
        if (docNoFrom != 0) p.put("DocNoFrom", docNoFrom);
        if (docNoTo != 0) p.put("DocNoTo", docNoTo);
        if (warehouseId != 0) p.put("Warehouseid", warehouseId);
        if (itemId != 0) p.put("ItemId", itemId);
        if (departmentId != 0) p.put("DepartmentId", departmentId);
        if (assetId != 0) p.put("AssetsId", assetId);
        if (accountId != 0) p.put("CrAccountId", accountId);
        p.put("Activity", "FormHistory");
        List<Map<String, Object>> out = new ArrayList<>();
        java.util.Set<Integer> seen = new java.util.HashSet<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, P_GETALL, p)) {
            int id = toInt(r.get("Id"));
            if (!seen.add(id)) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", id);
            o.put("VoucherHeadId", toInt(r.get("VoucherHeadId")));
            o.put("DocDate", r.get("DocDate"));
            o.put("DocNo", r.get("DocNo"));
            o.put("EntryDate", r.get("EntryDate"));
            o.put("EntryUserName", r.get("EntryUserName"));
            o.put("ModifyDate", r.get("ModifyDate"));
            o.put("ModifyUserName", r.get("ModifyUserName"));
            o.put("NoOfAttachments", r.get("NoOfAttachments"));
            out.add(o);
        }
        return out;
    }

    // ================================================================================= read

    public Map<String, Object> load(int id) {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> h = ownedHeader(u, id);
        if (h == null) return null;
        List<Map<String, Object>> details = DesktopProc.rows(jdbc, P_GETALL, params("Id", id, "Activity", "InvStoreReturnDetailReadById"));
        /* FillDetailFromListCommonForReadById:1767 — RecordNo numbered by first appearance of each IssuanceDetailId. */
        Map<Integer, Integer> recordNo = new HashMap<>();
        int counter = 0;
        for (Map<String, Object> d : details) {
            int k = toInt(d.get("IssuanceDetailId"));
            if (!recordNo.containsKey(k)) recordNo.put(k, ++counter);
        }
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> d : details) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("IssuanceId", toInt(d.get("IssuanceId")));
            o.put("IssuanceDetailId", toInt(d.get("IssuanceDetailId")));
            o.put("IssuanceNo", toInt(d.get("IssuanceNo")));
            o.put("ItemId", toInt(d.get("ItemId")));
            o.put("ItemCode", str(d.get("ItemCode")));
            o.put("ItemName", str(d.get("ItemName")));
            o.put("WareHouseId", toInt(d.get("Warehouseid")));
            o.put("WareHouseName", str(d.get("WareHouseName")));
            o.put("RackId", toInt(d.get("RackId")));
            o.put("RackName", str(d.get("rackName")));
            o.put("ItemConditionId", toInt(d.get("ItemConditionId")));
            o.put("ItemCondition", str(d.get("ItemCondition")));
            o.put("PackUomId", toInt(d.get("ItemUomId")));
            o.put("PackUom", str(d.get("UomCode")));
            o.put("BalanceQty", toDouble(d.get("BalQty")));
            o.put("ItemQty", toDouble(d.get("ItemQty")));
            o.put("SecondaryUomId", toInt(d.get("SecondaryUomId")));
            o.put("SecondaryUom", str(d.get("SecondaryUomCode")));
            o.put("SecondaryUomQty", toDouble(d.get("SecondaryUomQty")));
            o.put("PerItemWeight", toDouble(d.get("PerItemSecondaryUomQty")));
            o.put("SecondaryUomItemRate", toDouble(d.get("SecondaryUomItemRate")));
            o.put("ItemRate", toDouble(d.get("ItemRate")));
            o.put("IssuanceRate", toDouble(d.get("IssuanceRate")));
            o.put("ItemAmount", toDouble(d.get("ItemAmount")));
            o.put("DepartmentId", toInt(d.get("DepartmentId")));
            o.put("DepartmentName", str(d.get("DepartmentName")));
            o.put("AssetId", toInt(d.get("AssetsId")));
            o.put("AssetName", str(d.get("AssetName")));
            o.put("Remarks", str(d.get("ReamarksDetail")));
            o.put("CrAccountId", toInt(d.get("CrAccountId")));
            o.put("CreditAccount", str(d.get("CreditAc")));
            o.put("RecordNo", recordNo.get(toInt(d.get("IssuanceDetailId"))));
            rows.add(o);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("Id", id);
        out.put("DocNo", toInt(h.get("DocNo")));
        out.put("DocDate", h.get("DocDate"));
        out.put("Remarks", str(h.get("Remarks")));
        out.put("voucherHeadId", repo.voucherHeadId(u, DOCUMENT_TYPE_ID, id));
        out.put("rows", rows);
        return out;
    }

    /** SlipReturntoStore457 → BLL 0250 StoreReturnHistory with @Id only. */
    public List<Map<String, Object>> slip(int id) {
        UserAccount u = ctx.requireAccountingUser();
        if (ownedHeader(u, id) == null) return null;
        return DesktopProc.rows(jdbc, "Sp_InvStoreRetrun_SlipandRegister", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Id", id));
    }

    // ================================================================================= save

    @Transactional
    public Map<String, Object> save(StoreReturnDto dto) {
        UserAccount u = ctx.requireAccountingUser();
        int recId = dto.Id == null ? 0 : dto.Id;
        if (recId == 0 && !rights.has(SCREEN_NAME, "save")) throw new IllegalStateException("You do not have the Save right for this screen.");
        if (recId != 0 && !rights.has(SCREEN_NAME, "update")) throw new IllegalStateException("You do not have the Update right for this screen.");
        Map<String, Object> stored = recId != 0 ? ownedHeader(u, recId) : null;
        if (recId != 0 && stored == null) throw new IllegalArgumentException("Record Not Found");
        if (dto.DocNo == null || dto.DocNo == 0) throw new IllegalArgumentException("DocNo Field Required");   // FormValidation:1825
        /* R6 — txtDocNo is ReadOnly on the desktop (StoreReturn.cs:4686): the stored number on
           update, the generator's on a new save. The posted value is never trusted. */
        dto.DocNo = stored != null ? toInt(ci(stored, "DocNo")) : nextDocNo(u, ctx.currentFinancialYearId());

        /* ReadById assigns head.DocDate (midnight) — an opened document saves at midnight. */
        Timestamp docDate = StoreIssuanceService.formDate(dto.DocDate, recId);
        Timestamp now = Timestamp.valueOf(LocalDateTime.now());
        int branch = u.getBranchesId() == null ? 0 : u.getBranchesId();

        Map<String, Object> head = new LinkedHashMap<>();
        head.put("IsApproved", false);
        head.put("ApprovedDate", now);                 // :1594 ApprovedDate = DateTime.Now
        head.put("DocDate", docDate);
        head.put("EntryDate", now);
        head.put("ModifyDate", now);
        head.put("ApprovedUserId", u.getId());         // :1595
        head.put("BranchesId", branch);
        head.put("CompanyId", u.getCompanyId());
        head.put("DocNo", dto.DocNo);
        head.put("DocumentTypeId", DOCUMENT_TYPE_ID);
        head.put("EntryUser", u.getId());
        head.put("FinancialYearId", ctx.currentFinancialYearId());
        head.put("Id", recId);
        head.put("ModifyUser", u.getId());
        head.put("OrganizationId", u.getOrganizationId());
        head.put("ProjectsId", 0);                     // never set by the form
        head.put("Remarks", dto.Remarks == null ? "" : dto.Remarks);

        Map<Integer, String> conditionText = new HashMap<>();
        for (Map<String, Object> r : jdbc.queryForList("SELECT * FROM dbo.V_ItemCondition")) {
            conditionText.put(toInt(ci(r, "Id")), str(ci(r, "ConditionStatus")));
        }

        List<StoreReturnDto.Row> all = dto.rows == null ? new ArrayList<>() : dto.rows;
        List<Map<String, Object>> details = new ArrayList<>();
        for (int idx = 0; idx < all.size(); idx++) {
            StoreReturnDto.Row r = all.get(idx);
            if (Math.rint(d(r.ItemQty)) <= 0) continue;                                   // R2
            Map<String, Object> pd = new LinkedHashMap<>();
            double qty = d(r.ItemQty), rate = d(r.ItemRate);
            pd.put("ItemAmount", qty * rate);
            pd.put("ItemQty", qty);
            pd.put("ItemRate", rate);
            pd.put("AssetsId", i(r.AssetId));
            pd.put("CrAccountId", i(r.CrAccountId));
            pd.put("DepartmentId", i(r.DepartmentId));
            pd.put("Id", 0);
            pd.put("InvStoreReturnId", 0);
            pd.put("ItemId", i(r.ItemId));
            pd.put("ItemUomId", i(r.PackUomId));
            pd.put("LineId", idx + 1);
            pd.put("Warehouseid", i(r.WareHouseId));
            pd.put("ReamarksDetail", r.Remarks == null ? "" : r.Remarks);
            pd.put("ItemConditionId", i(r.ItemConditionId));
            pd.put("IssuanceId", i(r.IssuanceId));
            pd.put("IssuanceDetailId", i(r.IssuanceDetailId));
            pd.put("RackId", i(r.RackId));
            pd.put("SecondaryUomId", i(r.SecondaryUomId));
            pd.put("SecondaryUomQty", BigDecimal.valueOf(d(r.SecondaryUomQty)));
            pd.put("SecondaryUomItemRate", BigDecimal.valueOf(d(r.SecondaryUomItemRate)));

            validate(pd.get("ItemId"), "Item Name", idx);
            validate(pd.get("Warehouseid"), "Warehouse", idx);
            validate(pd.get("RackId"), "Rack Name", idx);
            validate(conditionText.getOrDefault(i(r.ItemConditionId), ""), "Item Condition", idx);
            validate(pd.get("ItemUomId"), "Item Uom", idx);
            validate(pd.get("ItemQty"), "Received Qty", idx);
            validate(pd.get("SecondaryUomId"), "Secondary Uom", idx);
            validate(pd.get("SecondaryUomQty"), "SecondaryUom Qty", idx);
            validate(pd.get("SecondaryUomItemRate"), "SecondaryUom ItemRate", idx);
            validate(pd.get("ItemRate"), "Item Rate", idx);
            validate(pd.get("ItemAmount"), "Item Amount", idx);
            validate(pd.get("DepartmentId"), "Department", idx);
            validate(pd.get("AssetsId"), "Asset", idx);
            validate(pd.get("CrAccountId"), "Credit Account", idx);

            int recordNo = i(r.RecordNo);
            int sameRecord = 0;
            double sum = 0d;
            for (StoreReturnDto.Row x : all) if (i(x.RecordNo) == recordNo) { sameRecord++; sum += d(x.ItemQty); }
            if (sameRecord > 1 && i(r.IssuanceDetailId) > 0 && sum > d(r.BalanceQty)) {
                throw new IllegalArgumentException("Total Qty of RecordNo" + recordNo
                        + " Can't Be greater than balance qty which Is " + clr(d(r.BalanceQty)));
            }
            details.add(pd);
        }

        boolean financial = toBool(repo.config(u, "StoreIssuanceFinancialEffect"));
        ContraVoucherDto.Head vh = null;
        List<ContraVoucherDto.Detail> lines = null;
        if (financial) {                                                                  // BLL Save → MakeVoucher
            vh = new ContraVoucherDto.Head();
            lines = new ArrayList<>();
            makeVoucher(u, head, details, vh, lines);
        }

        /* DAL 0219 SetData. */
        String proc = recId == 0 ? "Sp_InvStoreReturn_Insert" : "Sp_InvStoreReturn_Update";
        int num = repo.setProc(proc, head);
        if (num > 0) head.put("Id", num); else num = recId;
        if (num <= 0) throw new IllegalStateException("Save returned no document id.");   // defensive, see StoreIssuanceService
        for (Map<String, Object> pd : details) {
            pd.put("InvStoreReturnId", num);
            pd.put("Id", repo.setProc("Sp_InvStoreReturnDetail_Insert", pd));
        }
        if (financial) {
            int existing = repo.voucherHeadId(u, DOCUMENT_TYPE_ID, num);
            vh.Id = existing;
            vh.DocumentTypeSrNo = num;
            vh.RefDocNoId = num;
            int num2 = repo.setProc(existing == 0 ? "Sp_VoucherHead_Insert" : "Sp_VoucherHead_Update", model(vh));
            if (num2 > 0) vh.Id = num2;
            if (lines.isEmpty()) throw new IllegalArgumentException("VoucherDetail List Not Found");
            for (ContraVoucherDto.Detail l : lines) {
                l.VoucherHeadId = vh.Id;
                repo.setProc("Sp_VoucherDetail_Insert", model(l));
            }
            int ref = repo.setProc("Sp_VoucherHead_H_Insert", model(vh));
            for (ContraVoucherDto.Detail l : lines) {
                l.VoucherHeadId = vh.Id;
                l.DocumentTypeIdRef = ref;
                repo.setProc("Sp_VoucherDetail_H_Insert", model(l));
            }
        }
        repo.postStock(u, DOCUMENT_TYPE_ID, num);

        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", true);
        m.put("message", (recId == 0 ? "Record Save SuccessFully " : "Record Update SuccessFully ") + dto.DocNo);
        m.put("id", num);
        return m;
    }

    /** BLL 0250 MakeVoucher. */
    private void makeVoucher(UserAccount u, Map<String, Object> head, List<Map<String, Object>> details,
                             ContraVoucherDto.Head vh, List<ContraVoucherDto.Detail> lines) {
        String now = Timestamp.valueOf(LocalDateTime.now()).toString();
        vh.DocumentTypeId = DOCUMENT_TYPE_ID;
        vh.DocumentTypeSrNo = toInt(head.get("Id"));
        vh.RefDocNoId = toInt(head.get("Id"));
        vh.VoucherCode = toInt(head.get("DocNo"));
        vh.VoucherDate = String.valueOf(head.get("DocDate"));
        vh.Remarks = str(head.get("Remarks"));
        vh.RemarksOtherLingo = "";
        vh.ChequeDate = Timestamp.valueOf(LocalDate.now().atStartOfDay()).toString();
        vh.IncludeWHT = false;
        vh.BranchId = toInt(head.get("BranchesId"));
        vh.ProjectId = toInt(head.get("ProjectsId"));
        vh.ManualBillNo = String.valueOf(toInt(head.get("DocNo")));
        vh.DueDate = now;
        vh.OrganizationId = u.getOrganizationId();
        vh.CompanyId = u.getCompanyId();
        vh.FinancialYearId = toInt(head.get("FinancialYearId"));
        vh.EntryUser = u.getId();
        vh.EntryDate = now;
        vh.ModifyDate = now;
        vh.ModifyUser = u.getId();
        List<Map<String, Object>> gl = repo.itemGlAccounts(u);
        double total = 0d;
        for (Map<String, Object> item : details) {
            if (gl.isEmpty()) throw new IllegalArgumentException("Item Record Not found");
            int itemId = toInt(item.get("ItemId"));
            Map<String, Object> g = null;
            for (Map<String, Object> x : gl) if (toInt(x.get("Id")) == itemId) { g = x; break; }
            if (g == null) throw new IllegalArgumentException("Item Record Not found");
            double qty = toDouble(item.get("ItemQty")), rate = toDouble(item.get("ItemRate")), amt = toDouble(item.get("ItemAmount"));
            int cr = toInt(item.get("CrAccountId")), purchase = toInt(g.get("PurchaseGLAC"));
            String comments = "Item: " + str(g.get("ItemName")) + ",   Qty: " + clr(qty) + ",   Rate:" + clr(rate) + "  Item Amount: " + clr(amt);

            ContraVoucherDto.Detail dr = new ContraVoucherDto.Detail();
            dr.AccountId = purchase; dr.AgainstAccountId = cr; dr.Comments = comments; dr.DebitAmount = amt;
            dr.ItemId = itemId; dr.QtyIn = qty; dr.ItemRate = rate; dr.WeightIn = qty; dr.ItemAmount = amt;
            lines.add(dr);
            ContraVoucherDto.Detail c = new ContraVoucherDto.Detail();
            c.AccountId = cr; c.AgainstAccountId = purchase; c.Comments = comments; c.CreditAmount = amt;
            c.ItemId = itemId; c.QtyIn = qty; c.ItemRate = rate; c.WeightIn = qty; c.ItemAmount = amt;
            lines.add(c);
            total += c.CreditAmount;
        }
        vh.VoucherAmount = total;
        vh.BillAmount = total;
    }

    // ============================================================================== helpers

    private Map<String, Object> ownedHeader(UserAccount u, int id) {
        if (id <= 0) return null;
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, P_GETALL, params("Id", id, "Activity", "ReadById"));
        if (r.isEmpty()) return null;
        Map<String, Object> h = r.get(0);
        if (toInt(h.get("DocumentTypeId")) != DOCUMENT_TYPE_ID) return null;
        if (h.containsKey("CompanyId") && toInt(h.get("CompanyId")) != u.getCompanyId()) return null;
        if (h.containsKey("OrganizationId") && toInt(h.get("OrganizationId")) != u.getOrganizationId()) return null;
        return h;
    }

    private static Map<String, Object> model(Object o) {
        Map<String, Object> m = new LinkedHashMap<>();
        for (Field f : o.getClass().getFields()) {
            if (java.lang.reflect.Modifier.isStatic(f.getModifiers())) continue;
            if (List.class.isAssignableFrom(f.getType())) continue;
            try {
                Object v = f.get(o);
                if (v instanceof String && f.getName().matches("VoucherDate|ChequeDate|DueDate|EntryDate|ModifyDate|PostDate|DCheqDate|GpDate")) {
                    String s = (String) v;
                    v = Timestamp.valueOf(s.length() == 10 ? s + " 00:00:00" : s);
                }
                m.put(f.getName(), v);
            } catch (IllegalAccessException ignored) { }
        }
        return m;
    }

    private static void validate(Object v, String field, int rowIndex) {
        boolean bad = v == null
                || (v instanceof Integer && (Integer) v == 0)
                || (v instanceof Double && (Double) v <= 0d)
                || (v instanceof BigDecimal && ((BigDecimal) v).signum() <= 0)
                || (v instanceof String && ((String) v).trim().isEmpty());
        if (bad) throw new IllegalArgumentException(field + " is required in Detail Grid at row No: " + (rowIndex + 1));
    }

    private String financialYearStart(UserAccount u, int yearId) {
        try {
            for (Map<String, Object> r : jdbc.queryForList(
                    "EXEC dbo.Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId @OrganizationId=?, @CompanyId=?",
                    u.getOrganizationId(), u.getCompanyId())) {
                if (toInt(ci(r, "Id")) == yearId) {
                    Object v = ci(r, "Start_Period");
                    return v == null ? null : String.valueOf(v).substring(0, 10);
                }
            }
        } catch (Exception e) {
            LOG.warn("Could not read the financial year start", e);
        }
        return null;
    }

    private static List<Map<String, Object>> project(List<Map<String, Object>> rows, String id, String name) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get(id)));
            o.put("Name", str(r.get(name)));
            out.add(o);
        }
        return out;
    }

    private static int i(Integer v) { return v == null ? 0 : v; }
    private static double d(Double v) { return v == null ? 0d : v; }
    private static boolean toBool(String v) { return v != null && ("true".equalsIgnoreCase(v.trim()) || "1".equals(v.trim())); }
    private static boolean toBoolObj(Object v) {
        if (v instanceof Boolean) return (Boolean) v;
        if (v instanceof Number) return ((Number) v).intValue() != 0;
        return v != null && "true".equalsIgnoreCase(String.valueOf(v).trim());
    }
}
