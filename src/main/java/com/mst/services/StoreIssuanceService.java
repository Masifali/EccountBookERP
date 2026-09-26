package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.dto.ContraVoucherDto;
import com.mst.models.dto.StoreIssuanceDto;
import com.mst.repositories.StoreIssuanceRepository;
import com.mst.repositories.support.DesktopProc;
import com.mst.security.CurrentUserContext;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.lang.reflect.Field;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.LocalTime;
import java.util.ArrayList;
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
 * Store Management — Store Issuance (screen 322, {@code frmGSIssuance.cs}, DocumentTypeId 451)
 * and Store Issuance Direct (screen 321, {@code StoreIssuanceDirect.cs}, DocumentTypeId 452).
 *
 * Both forms save through the same BLL/DAL pair (BLL 0252 / DAL 0221
 * Architecture.*.PurchaseTrading.InvGsStoreIssuanceHeader), which is why they share this class.
 * What differs is kept in {@link Screen} and in the two save methods; nothing is unified that the
 * desktop keeps apart.
 *
 * ---------------------------------------------------------------------------------------------
 * DESKTOP BEHAVIOUR REPRODUCED, NOT CORRECTED (each is the user's call to change)
 * ---------------------------------------------------------------------------------------------
 *  D1  322 saves the literal text "Remarks" into every detail row's ReamarksDetail
 *      (frmGSIssuance.cs:479 {@code Conversion.ToString((object)"Remarks")}) — the value the
 *      operator typed in the grid's Remarks cell is never saved.
 *  D2  321's MakeVoucher (BLL 0252:54) iterates EVERY detail, including rows the operator deleted
 *      on update (ActionTypeId 3). Their amounts are posted to the voucher.
 *  D3  321's voucher Debit/Credit amounts pass through {@code Conversion.ToSingle} (BLL 0252:68,81)
 *      — float precision — before landing in a double column.
 *  D4  321 validates ContractScheduleId only when IssuanceTypeId == 2 (StoreIssuanceDirect.cs:1478),
 *      but IssuanceTypeId holds an InvLookup id (57 / 58), so that check never fires; the entry
 *      bar's own "Contract Schedule No Required." check (Category1 == 57) is what applies.
 *  D5  322's "Issue Qty grater than Row Balance Qty" message concatenates the row index and 1 as
 *      text ("Row no 01" for the first row) — reproduced verbatim.
 *  D7  (web) Doc No / Branch Sr No are read-only on both forms; the server takes them from the
 *      generator on a new save and from the stored header on update, never from the request.
 *      The desktop saves the number shown when the form was reset — the same number unless another
 *      user saved in between, where the desktop would write a duplicate.
 *  D6  321 on update with removed rows: removed rows go first with LineId 0, grid rows carry
 *      RowIndex+1, then the DAL renumbers every detail 1..n and matches voucher lines to details by
 *      LineId — so voucher lines' RefDocSubIdNo can point at a removed row's new detail id.
 *
 * DEVIATIONS (web-only, stated): a document is only opened, updated or deleted when it belongs to the signed-in user's company and
 * this screen's document type; a save whose header procedure returns no id is refused.
 */
@Service
public class StoreIssuanceService {

    private static final Logger LOG = LoggerFactory.getLogger(StoreIssuanceService.class);

    /** The two screens. ScreenName is the rights key and the attachment screen name. */
    public enum Screen {
        ISSUANCE(451, "frmGSIssuance", 0),
        DIRECT(452, "StoreIssuanceDirect", 0);   // BaseDocumentTypeId 0 for ScreenName "StoreIssuanceDirect"

        public final int documentTypeId;
        public final String screenName;
        public final int baseDocumentTypeId;

        Screen(int documentTypeId, String screenName, int baseDocumentTypeId) {
            this.documentTypeId = documentTypeId;
            this.screenName = screenName;
            this.baseDocumentTypeId = baseDocumentTypeId;
        }
    }

    /** StoreIssuanceDirect.ItemCategoryOrTypeBind — ItemTypeOfTypeId allowed for "StoreIssuanceDirect". */
    private static final int[] DIRECT_ITEM_TYPE_OF_TYPES = { 14, 17 };

    private final StoreIssuanceRepository repo;
    private final StoreScreenRights rights;
    private final CurrentUserContext ctx;
    private final JdbcTemplate jdbc;

    public StoreIssuanceService(StoreIssuanceRepository repo, StoreScreenRights rights,
                                CurrentUserContext ctx, JdbcTemplate jdbc) {
        this.repo = repo;
        this.rights = rights;
        this.ctx = ctx;
        this.jdbc = jdbc;
    }

    // ====================================================================================== load

    public Map<String, Object> lookups(Screen s) {
        UserAccount u = ctx.requireAccountingUser();
        int branchId = branch(u);
        int fy = ctx.currentFinancialYearId();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rights", rights.of(s.screenName));
        out.put("docNo", repo.nextDocNo(u, fy, s.documentTypeId));
        out.put("branchSrNo", repo.nextBranchSrNo(u, branchId, fy, s.documentTypeId));
        out.put("itemConditions", repo.itemConditions());
        out.put("racks", repo.racksWithWarehouseAndItem(u, branchId));
        out.put("packingMaterialDefaultWarehouse", toInt(repo.config(u, "PackingMaterialDefaultWarehouse")));
        out.put("defaultWarehouseForStoreFlow", toInt(repo.config(u, "DefaultWarehouseForStoreFlow")));
        out.put("financialYearStart", financialYearStart(u, fy));
        if (s == Screen.DIRECT) directLookups(u, out);
        return out;
    }

    /** StoreIssuanceDirect.InitializeComponentMethod:393 — the pickers only 321 has. */
    private void directLookups(UserAccount u, Map<String, Object> out) {
        out.put("defaultDaysToLessFromHistoryFromDate", toInt(repo.config(u, "DefaultDaysToLessFromHistoryFromDate")));
        out.put("itemSearchByCode", toBool(repo.config(u, "ItemSearchByCode")));

        /* ItemDtsFillFromGlobal — clsGlobalVariables.getGlobalAllItems (USP_Item_AllItemsWithModal),
           filtered to ItemTypeOfTypeId 14 / 17 in memory, exactly as the form filters it. */
        List<Map<String, Object>> items = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "[dbo].[USP_Item_AllItemsWithModal]",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()))) {
            int tot = toInt(r.get("ItemTypeOfTypeId"));
            boolean allowed = false;
            for (int a : DIRECT_ITEM_TYPE_OF_TYPES) if (a == tot) allowed = true;
            if (!allowed) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get("Id")));
            o.put("ItemName", str(r.get("ItemName")));
            o.put("ItemCode", str(r.get("ItemCode")));
            o.put("ItemCategoryId", toInt(r.get("ItemCategoryId")));
            o.put("ItemCategory", str(r.get("ItemCategory")));
            o.put("ItemTypeId", toInt(r.get("ItemTypeId")));
            o.put("ItemType", str(r.get("ItemType")));
            o.put("ParentCategoryId", toInt(r.get("InventoryParentCategoriesId")));
            items.add(o);
        }
        out.put("items", items);

        /* UomFromGlobalBind — clsGlobalVariables.globalUomSchedule, loaded with Active = 1
           (DatatableHelper "UomSchedule": getAllUomsByCompanyId(org, comp, 0, 1)). */
        List<Map<String, Object>> uoms = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "usp_getAllUomsByCompanyId",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Active", 1))) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get("Id")));
            o.put("ItemId", toInt(r.get("ItemId")));
            o.put("UOMCode", str(r.get("UOMCode")));
            o.put("Equivalent", toDouble(r.get("Equivalent")));
            uoms.add(o);
        }
        out.put("uoms", uoms);

        /* DepartmentDbCall — BLL 0067 Department.GetAll, @Activity='ReadAll'. */
        out.put("departments", project(DesktopProc.rows(jdbc, "Sp_Department_GetAllMethod",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll")),
                "Id", "DepartmentName"));
        /* FixedAssetsDbCall — BLL 0223 FixedAssetsRegister.GetAll, @Activity='ReadAll'. */
        out.put("assets", project(DesktopProc.rows(jdbc, "Sp_FixedAssetsRegister_GetAllMethod",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll")),
                "Id", "AssetName"));
        /* IssuanceAgainstExportInvoices is hard-coded TRUE in the constructor, so Category1 and the
           schedule picker are always present: CommonServices.GetLookupsByTypeIdDt(20) → BLL 0577. */
        out.put("issuanceTypes", project(DesktopProc.rows(jdbc, "Sp_InvLookup_GetAllMethod",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                        "InvLookupTypeId", 20, "Activity", "ReadByInvlookTypeId")),
                "Id", "LookupName"));
        out.put("schedules", pendingSchedules(u));

        /* DebitAcDtFillFromGlobal — AccountsWithCustomGroupId filtered by AccountTypeId:
           Category1 57 → {21}, 58 → {11}. Both sets are sent; the page picks by Category1. */
        List<Map<String, Object>> accounts = DesktopProc.rows(jdbc, "[dbo].[USP_GETAllAccountsFromCustomGroups]",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
        out.put("debitAccounts57", accountsOfType(accounts, 21));
        out.put("debitAccounts58", accountsOfType(accounts, 11));
    }

    private List<Map<String, Object>> pendingSchedules(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "[dbo].[usp_ExportContractSchedule_GetPending]",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()))) {
            Map<String, Object> o = new LinkedHashMap<>(r);
            out.add(o);
        }
        return out;
    }

    /** DatatableHelper.GetAccountsFromGlobalByTypeIds — distinct by ChartOfAccountId, first wins. */
    private static List<Map<String, Object>> accountsOfType(List<Map<String, Object>> all, int typeId) {
        List<Map<String, Object>> out = new ArrayList<>();
        java.util.Set<Integer> seen = new java.util.HashSet<>();
        for (Map<String, Object> a : all) {
            if (toInt(a.get("AccountTypeId")) != typeId) continue;
            int id = toInt(a.get("ChartOfAccountId"));
            if (!seen.add(id)) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", id);
            o.put("AccountTitle", str(a.get("AccountTitle")));
            o.put("AccountCode", str(a.get("AccountCode")));
            out.add(o);
        }
        return out;
    }

    // =================================================================================== stock

    /**
     * The desktop's pair of GetAvgRateQtyAndStockInHand calls: QtyInHand with warehouse and rack,
     * AvgRate without them. Rounded as the forms round them (2 and 3 places).
     */
    public Map<String, Object> stock(Screen s, int recId, int itemId, String docDate,
                                     int itemConditionId, int warehouseId, int rackId) {
        UserAccount u = ctx.requireAccountingUser();
        Timestamp d = formDate(docDate, recId);                     // an opened document's picker is at midnight
        int docType = recId > 0 ? s.documentTypeId : 0;
        Map<String, Object> q = repo.avgRateAndStock(u, itemId, d, itemConditionId, recId, docType, warehouseId, rackId);
        Map<String, Object> r = repo.avgRateAndStock(u, itemId, d, itemConditionId, recId, docType, 0, 0);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("qtyInHand", q == null ? 0d : round(toDouble(q.get("QtyInHand")), 2));
        out.put("avgRate", r == null ? 0d : round(toDouble(r.get("AvgRate")), 3));
        return out;
    }

    public List<Map<String, Object>> lastThreeRates(int itemId, int itemConditionId) {
        UserAccount u = ctx.requireAccountingUser();
        if (itemId <= 0) return new ArrayList<>();
        return repo.lastThreeRates(u, itemId, itemConditionId);
    }

    /** StoreIssuanceDirect.GetItemIdByBarcode → BLL 0583 Item.GetItemIdByBarcodeNo. */
    public int itemIdByBarcode(String barcode) {
        UserAccount u = ctx.requireAccountingUser();
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_Item_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "BarcodeNo", barcode == null ? "" : barcode.trim(), "Activity", "GetItemIdByBarcodeNo"));
        return r.isEmpty() ? 0 : toInt(r.get(0).get("Id"));
    }

    // ================================================================================= loaders

    /** PendingDoPmForIssuance.PendingDeliverOrdertLoad — FromDate/ToDate are .Value.Date. */
    public List<Map<String, Object>> pendingDeliveryOrders(String from, String to, int docNoFrom, int docNoTo) {
        UserAccount u = ctx.requireAccountingUser();
        return repo.pendingDeliveryOrdersPm(u, ctx.currentFinancialYearId(), branch(u),
                dateOnly(from), dateOnly(to), docNoFrom, docNoTo);
    }

    public Map<String, Object> departmentRequestLookups() {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, List<Map<String, Object>>> byActivity = new LinkedHashMap<>();
        for (String a : new String[] { "Item", "DepartmentTo", "DepartmentFrom" }) byActivity.put(a, new ArrayList<>());
        for (Map<String, Object> r : repo.departmentRequestDropDowns(u)) {
            String activity = str(r.get("Activity"));
            List<Map<String, Object>> t = byActivity.get(activity);
            if (t == null) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get("Id")));
            o.put("Name", str(r.get("ReferenceName")));
            t.add(o);
        }
        return new LinkedHashMap<>(byActivity);
    }

    /** LoadDepRequestToConsumableStore.PendingDepRequestLoad with DocumentTypeId = 450 (frmGSIssuance:964). */
    public List<Map<String, Object>> pendingDepartmentRequests(String from, String to, int docNoFrom, int docNoTo,
                                                               int itemId, int departmentFromId, int departmentToId) {
        UserAccount u = ctx.requireAccountingUser();
        /* Both pickers hold midnight values on this dialog: FromDate = ActiveYr.Start_Period, and
           the "This Month" parameter it activates on load sets ToDate = DateTime.Today. */
        return repo.pendingDepartmentRequests(u, ctx.currentFinancialYearId(), 450,
                dateOnly(from), dateOnly(to), docNoFrom, docNoTo, itemId, departmentFromId, departmentToId);
    }

    // ================================================================================= history

    public List<Map<String, Object>> history(Screen s, String from, String to, int fromDocNo, int toDocNo) {
        UserAccount u = ctx.requireAccountingUser();
        boolean viewAll = rights.has(s.screenName, "viewAll");
        List<Map<String, Object>> rows = repo.formHistory(u, s.documentTypeId, ctx.currentFinancialYearId(),
                s.baseDocumentTypeId, branch(u), viewAll, u.getId(),
                isBlank(from) ? null : pickerDate(from), isBlank(to) ? null : pickerDate(to), fromDocNo, toDocNo);
        /* Both forms drop duplicate Ids (HashSet existingIds) and project their own columns. */
        List<Map<String, Object>> out = new ArrayList<>();
        java.util.Set<Integer> seen = new java.util.HashSet<>();
        for (Map<String, Object> r : rows) {
            int id = toInt(r.get("Id"));
            if (!seen.add(id)) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", id);
            o.put("DocNo", r.get("DocNo"));
            o.put("DocDate", r.get("DocDate"));
            if (s == Screen.ISSUANCE) {
                o.put("IssuanceType", toInt(r.get("RefDocumentTypeId")) == 85 ? "DeliveryOrder" : "DepartmentRequest");
            } else {
                o.put("Type", r.get("Type"));
                o.put("ManualNo", r.get("ManualNo"));
            }
            o.put("Remarks", r.get("Remarks"));
            o.put("EntryDate", r.get("EntryDate"));
            o.put("EntryUser", r.get("EntryUserName"));
            o.put("ModifyDate", r.get("ModifyDate"));
            o.put("ModifyUser", r.get("ModifyUserName"));
            o.put("NoOfAttachments", r.get("NoOfAttachments"));
            o.put("BranchSrNo", r.get("BranchSrNo"));
            out.add(o);
        }
        return out;
    }

    // ==================================================================================== read

    /** ReadById — header plus the detail rows, refused when the document is not this screen's. */
    public Map<String, Object> load(Screen s, int id) {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> h = ownedHeader(u, s, id);
        if (h == null) return null;
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> d : repo.details(h)) rows.add(detailRow(s, d));
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("Id", toInt(h.get("Id")));
        out.put("DocNo", toInt(h.get("DocNo")));
        out.put("BranchSrNo", toInt(h.get("BranchSrNo")));
        out.put("DocDate", h.get("DocDate"));
        out.put("Remarks", str(h.get("Remarks")));
        out.put("ManualNo", str(h.get("ManualNo")));
        out.put("RefDocumentTypeId", toInt(h.get("RefDocumentTypeId")));
        out.put("RequestType", toInt(h.get("RefDocumentTypeId")) == 85 ? "DeliveryOrder" : "DepartmentRequest");
        out.put("IsApproved", h.get("IsApproved"));
        out.put("rows", rows);
        if (s == Screen.DIRECT) out.put("voucherHeadId", repo.voucherHeadId(u, s.documentTypeId, toInt(h.get("Id"))));
        return out;
    }

    /** FilldtDetailFromListCommonForReadById — the model's properties, by name. */
    private static Map<String, Object> detailRow(Screen s, Map<String, Object> d) {
        Map<String, Object> o = new LinkedHashMap<>();
        o.put("Id", toInt(d.get("Id")));
        o.put("DepartmentRequestId", toInt(d.get("DepartRequestId")));
        o.put("DepartmentRequestDetailId", toInt(d.get("DepartRequestDetailId")));
        o.put("DepartmentRequestNo", toInt(d.get("DepartmentRequestNo")));
        o.put("DoDocumentTypeId", toInt(d.get("RefDocumentTypeId")));
        o.put("DoId", toInt(d.get("RefDocId")));
        o.put("DoDetailId", toInt(d.get("RefDocDetailId")));
        o.put("DoNo", toInt(d.get("RefDocNo")));
        o.put("SupplierCustomerId", toInt(d.get("SupplierCustomerId")));
        o.put("SupplierName", str(d.get("CompanyName")));
        o.put("ItemId", toInt(d.get("ItemId")));
        o.put("ItemCode", str(d.get("ItemCode")));
        o.put("ItemName", str(d.get("ItemName")));
        o.put("WarehouseId", toInt(d.get("Warehouseid")));
        o.put("WarehouseName", str(d.get("WareHouseName")));
        o.put("RackId", toInt(d.get("RackId")));
        o.put("RackName", str(d.get("rackName")));
        o.put("ItemConditionId", toInt(d.get("ItemConditionId")));
        o.put("ItemCondition", str(d.get("ItemCondition")));
        int bag = toInt(d.get("BagTypeId"));
        o.put("BagTypeId", bag);
        o.put("BagType", bag == 1 ? "Normal" : "Retain");                 // :558
        o.put("UnitId", toInt(d.get("ItemUomSchId")));
        o.put("Unit", str(d.get("UomCode")));
        o.put("Equivalent", toDouble(d.get("Equivalent")));
        o.put("ItemRate", toDouble(d.get("ItemRate")));
        o.put("IssueQty", toDouble(d.get("IssueQty")));
        o.put("ItemAmount", toDouble(d.get("ItemAmount")));
        o.put("RowBalanceQty", toDouble(d.get("RowBalanceQty")));
        o.put("BalanceStock", toDouble(d.get("QtyBal")));
        o.put("DepartmentId", toInt(d.get("DepartmentId")));
        o.put("Department", str(d.get("DepartmentName")));
        o.put("AssetId", toInt(d.get("AssetsId")));
        o.put("Asset", str(d.get("AssetName")));
        o.put("IssuanceTypeId", toInt(d.get("IssuanceTypeId")));
        o.put("IssuanceType", str(d.get("IssuanceType")));
        o.put("ContractScheduleId", toInt(d.get("ContractScheduleId")));
        o.put("ContractScheduleNo", str(d.get("ContractScheduleNo")));
        o.put("DrAcId", toInt(d.get("DrAccountId")));
        o.put("DebitAc", str(d.get("DebitAc")));
        o.put("Remarks", str(d.get("ReamarksDetail")));
        return o;
    }

    /** Print — CommonServices.StoreIssuanceSlip452 → BLL 0252 StoreIssuanceHistory (@Id only). */
    public List<Map<String, Object>> slip(Screen s, int id) {
        UserAccount u = ctx.requireAccountingUser();
        if (ownedHeader(u, s, id) == null) return null;
        return DesktopProc.rows(jdbc, "Sp_InvGsStoreIssuanceHeader_SlipandRegister", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Id", id));
    }

    // ================================================================================== delete

    @Transactional
    public Map<String, Object> delete(Screen s, int id) {
        UserAccount u = ctx.requireAccountingUser();
        if (!rights.has(s.screenName, "delete")) {
            throw new IllegalStateException("You do not have the Delete right for this screen.");
        }
        if (id <= 0) throw new IllegalArgumentException("Record Not Found");
        if (ownedHeader(u, s, id) == null) throw new IllegalArgumentException("Record Not Found");
        if (s == Screen.ISSUANCE) {
            repo.deleteIssuance(u.getId(), id);                                   // :590
        } else {
            repo.removeInvoiceVoucherAndStock(u, s.documentTypeId, id, u.getId()); // :1665
        }
        return ok("Delete Record Successfully", id);
    }

    // ==================================================================================== save

    @Transactional
    public Map<String, Object> save(Screen s, StoreIssuanceDto dto) {
        UserAccount u = ctx.requireAccountingUser();
        int recId = dto.Id == null ? 0 : dto.Id;
        if (recId == 0 && !rights.has(s.screenName, "save")) {
            throw new IllegalStateException("You do not have the Save right for this screen.");
        }
        if (recId != 0 && !rights.has(s.screenName, "update")) {
            throw new IllegalStateException("You do not have the Update right for this screen.");
        }
        Map<String, Object> existing = null;
        if (recId != 0) {
            existing = ownedHeader(u, s, recId);
            if (existing == null) throw new IllegalArgumentException("Record id not found...");
        }
        return s == Screen.ISSUANCE ? saveIssuance(u, dto, recId, existing) : saveDirect(u, dto, recId, existing);
    }

    /** frmGSIssuance.Insert():296. */
    private Map<String, Object> saveIssuance(UserAccount u, StoreIssuanceDto dto, int recId, Map<String, Object> existing) {
        final Screen s = Screen.ISSUANCE;
        if (dto.rows == null || dto.rows.isEmpty()) throw new IllegalArgumentException("Grid record not found");
        if (dto.DocNo == null || dto.DocNo == 0) throw new IllegalArgumentException("document Number Field Required");
        /* :312 — asked for the new document too (RECID 0), exactly as the form asks it. */
        if (repo.voucherHeadId(u, s.documentTypeId, recId) > 0) {
            throw new IllegalArgumentException("Record Not Update because Record has exist In voucher");
        }
        Timestamp docDate = formDate(dto.DocDate, recId);

        /* RequestType — set by whichever loader filled the grid; on an opened document it is
           RefDocumentTypeId == 85 (:532). */
        String requestType;
        if (existing != null) {
            requestType = toInt(existing.get("RefDocumentTypeId")) == 85 ? "DeliveryOrder" : "DepartmentRequest";
        } else {
            boolean anyDo = false;
            for (StoreIssuanceDto.Row r : dto.rows) if (i(r.DoId) > 0 || i(r.DoDocumentTypeId) > 0) anyDo = true;
            requestType = anyDo ? "DeliveryOrder" : "DepartmentRequest";
        }

        Map<String, Object> head = header(u, s, dto, recId, docDate, existing);
        head.put("ManualNo", null);                               // 322 never sets ManualNo
        head.put("TypeId", 0);
        List<Map<String, Object>> details = new ArrayList<>();
        int refDocType = 0, refDocNo = 0;
        for (int idx = 0; idx < dto.rows.size(); idx++) {
            StoreIssuanceDto.Row r = dto.rows.get(idx);
            int id = recId != 0 ? i(r.Id) : 0;
            Map<String, Object> pd = issuanceDetail(u, r, id, id <= 0 ? 1 : 2, docDate, recId, s);
            if (d(pd.get("IssueQty")) > d(r.RowBalanceQty)) {
                /* D5 — C# "..." + r.RowIndex + 1 is string concatenation. */
                throw new IllegalArgumentException("Issue Qty grater than Row Balance Qty Please Check in Row no " + idx + "1");
            }
            if (!"DeliveryOrder".equals(requestType)) {
                validate(pd.get("DepartRequestId"), "DepartRequestNo", idx);
                validate(pd.get("DepartRequestDetailId"), "DepartRequestDetailId", idx);
            } else {
                validate(pd.get("RefDocumentTypeId"), "DoDocumentType", idx);
                validate(pd.get("RefDocId"), "DoNo", idx);
                validate(pd.get("RefDocDetailId"), "DoDetailId", idx);
                validate(pd.get("SupplierCustomerId"), "Party Name", idx);
            }
            validate(pd.get("Warehouseid"), "WareHouse", idx);
            validate(pd.get("RackId"), "Rack Name", idx);
            validate(pd.get("ItemConditionId"), "Item Condition", idx);
            validate(pd.get("ItemId"), "Item", idx);
            validate(pd.get("ItemUomSchId"), "Unit", idx);
            validate(pd.get("IssueQty"), "IssueQty", idx);
            details.add(pd);
            if (refDocType == 0) {                                                 // :378
                int rdt = toInt(pd.get("RefDocumentTypeId"));
                refDocType = rdt > 0 ? rdt : 450;
                int rid = toInt(pd.get("RefDocId"));
                refDocNo = rid > 0 ? rid : toInt(pd.get("DepartRequestId"));
            }
        }
        head.put("RefDocumentTypeId", refDocType);
        head.put("RefDocNoId", refDocNo);
        if (recId != 0 && dto.removed != null) {
            for (StoreIssuanceDto.Row r : dto.removed) {
                if (i(r.Id) <= 0) continue;
                details.add(issuanceDetail(u, r, i(r.Id), 3, docDate, recId, s));
            }
        }
        int code = persist(u, s, head, details, null);
        return ok(recId == 0 ? "Record Save SuccessFully" + dto.DocNo : "Record Update SuccessFully" + dto.DocNo, code);
    }

    /** FillDetailListCommonForInsertAndDelete (frmGSIssuance.cs:434) + Id/ActionTypeId. */
    private Map<String, Object> issuanceDetail(UserAccount u, StoreIssuanceDto.Row r, int id, int actionTypeId,
                                               Timestamp docDate, int recId, Screen s) {
        double issueQty = d(r.IssueQty);
        double rate = d(r.ItemRate);
        int itemId = i(r.ItemId);
        if (itemId > 0 && rate <= 0d) {                                            // :461
            Map<String, Object> row = repo.avgRateAndStock(u, itemId, docDate, i(r.ItemConditionId), recId,
                    recId > 0 ? s.documentTypeId : 0, 0, 0);
            double avg = row == null ? 0d : round(toDouble(row.get("AvgRate")), 3);
            if (avg > 0d) rate = avg;
        }
        Map<String, Object> pd = detailModel();
        pd.put("IssueQty", issueQty);
        pd.put("AssetsId", i(r.AssetId));
        pd.put("DepartmentId", i(r.DepartmentId));
        pd.put("Id", id);
        pd.put("ItemId", itemId);
        pd.put("ItemUomSchId", i(r.UnitId));
        pd.put("ItemConditionId", i(r.ItemConditionId));
        pd.put("Warehouseid", i(r.WarehouseId));
        pd.put("SupplierCustomerId", i(r.SupplierCustomerId));
        pd.put("BagTypeId", i(r.BagTypeId));
        pd.put("ItemRate", rate);
        pd.put("ItemAmount", issueQty * rate);
        pd.put("ReamarksDetail", "Remarks");                                       // D1
        pd.put("RefDocumentTypeId", i(r.DoDocumentTypeId));
        pd.put("RefDocId", i(r.DoId));
        pd.put("RefDocDetailId", i(r.DoDetailId));
        pd.put("ActionTypeId", actionTypeId);
        pd.put("DepartRequestDetailId", i(r.DepartmentRequestDetailId));
        pd.put("DepartRequestId", i(r.DepartmentRequestId));
        pd.put("RackId", i(r.RackId));
        return pd;
    }

    /** StoreIssuanceDirect.Insert():1418. */
    private Map<String, Object> saveDirect(UserAccount u, StoreIssuanceDto dto, int recId, Map<String, Object> existing) {
        final Screen s = Screen.DIRECT;
        if (dto.rows == null || dto.rows.isEmpty()) throw new IllegalArgumentException("Grid Record Not Found");
        /* ValidateControls(txtDocNo, String) — only a BLANK box is refused; "0" passes. */
        if (dto.DocNo == null) throw new IllegalArgumentException("Doc No field is required");
        Timestamp docDate = formDate(dto.DocDate, recId);

        Map<String, Object> head = header(u, s, dto, recId, docDate, existing);
        head.put("ManualNo", dto.ManualNo == null ? "" : dto.ManualNo);
        head.put("TypeId", 1);
        head.put("RefDocNoId", 0);
        head.put("RefDocumentTypeId", 0);

        /* dtScheduleData, as ScheduleNoDbCall loads it on form open (RecId 0). */
        List<Map<String, Object>> schedules = pendingSchedules(u);

        List<Map<String, Object>> details = new ArrayList<>();
        /* :1450 — removed rows go FIRST, then the grid. */
        if (dto.removed != null && recId > 0) {
            for (StoreIssuanceDto.Row r : dto.removed) {
                if (i(r.Id) <= 0) continue;
                Map<String, Object> pd = directDetail(r, schedules);
                pd.put("Id", i(r.Id));
                pd.put("ActionTypeId", 3);
                details.add(pd);
            }
        }
        for (int idx = 0; idx < dto.rows.size(); idx++) {
            StoreIssuanceDto.Row r = dto.rows.get(idx);
            Map<String, Object> pd = directDetail(r, schedules);
            pd.put("LineId", idx + 1);
            pd.put("SupplierCustomerId", 0);
            pd.put("EmployeeId", 0);
            int id = recId != 0 ? i(r.Id) : 0;
            pd.put("Id", id);
            pd.put("ActionTypeId", id <= 0 ? 1 : 2);
            validate(pd.get("ItemId"), "Item Name", idx);
            validate(pd.get("Warehouseid"), "Warehouse", idx);
            validate(pd.get("RackId"), "Rack Name", idx);
            validate(pd.get("ItemConditionId"), "Item Condition", idx);
            validate(pd.get("IssueQty"), "Issue Quantity", idx);
            validate(pd.get("ItemRate"), "Item Rate", idx);
            validate(pd.get("ItemAmount"), "Item Amount", idx);
            validate(pd.get("IssuanceTypeId"), "Issuance Type", idx);             // IssuanceAgainstExportInvoices == true
            if (toInt(pd.get("IssuanceTypeId")) == 2) {                            // D4
                validate(pd.get("ContractScheduleId"), "Contract Schedule No", idx);
            }
            details.add(pd);
        }

        /* BLL Save: StoreIssuanceFinancialEffect && ActionForVoucherId == 1 → MakeVoucher. */
        Map<String, Object> voucher = null;
        if (toBool(repo.config(u, "StoreIssuanceFinancialEffect"))) {
            voucher = makeVoucher(u, head, details, s);
        }
        int code = persist(u, s, head, details, voucher);
        return ok(recId == 0 ? "Record Save SuccessFully" + dto.DocNo : "Record Update SuccessFully" + dto.DocNo, code);
    }

    /** StoreIssuanceDirect.FillDetailListCommonForInsertAndDelete:1532. */
    private Map<String, Object> directDetail(StoreIssuanceDto.Row r, List<Map<String, Object>> schedules) {
        Map<String, Object> pd = detailModel();
        double qty = d(r.IssueQty);
        double rate = d(r.ItemRate);
        pd.put("ItemId", i(r.ItemId));
        pd.put("Warehouseid", i(r.WarehouseId));
        pd.put("RackId", i(r.RackId));
        pd.put("ItemConditionId", i(r.ItemConditionId));
        pd.put("ItemUomSchId", i(r.UnitId));
        pd.put("IssueQty", qty);
        pd.put("ItemRate", rate);
        pd.put("ItemAmount", qty * rate);
        pd.put("DepartmentId", i(r.DepartmentId));
        pd.put("AssetsId", i(r.AssetId));
        pd.put("IssuanceTypeId", i(r.IssuanceTypeId));
        pd.put("DrAccountId", i(r.DrAcId));
        int sched = i(r.ContractScheduleId);
        pd.put("ContractScheduleId", sched);
        for (Map<String, Object> row : schedules) {
            if (toInt(row.get("Id")) == sched) { pd.put("ExImInvoiceId", toInt(row.get("InvoiceId"))); break; }
        }
        pd.put("ReamarksDetail", r.Remarks == null ? "" : r.Remarks);
        return pd;
    }

    /** The header both forms build, in the model's declaration order. */
    private Map<String, Object> header(UserAccount u, Screen s, StoreIssuanceDto dto, int recId,
                                       Timestamp docDate, Map<String, Object> existing) {
        Timestamp now = Timestamp.valueOf(LocalDateTime.now());
        Map<String, Object> h = new LinkedHashMap<>();
        h.put("IsApproved", false);
        h.put("ApprovedDate", null);
        h.put("DocDate", docDate);
        h.put("EntryDate", now);
        h.put("ModifyDate", now);
        h.put("ApprovedUserId", 0);
        h.put("BranchesId", branch(u));
        /* D7 — txtDocNo / txtBranchSrNo are ReadOnly on the desktop (frmGSIssuance.cs:2155,
           StoreIssuanceDirect.cs:4418): the number is the generator's (new) or the loaded header's
           (update). The posted values are never trusted. */
        int docNo, branchSrNo;
        if (existing != null) {
            docNo = toInt(ci(existing, "DocNo"));
            branchSrNo = toInt(ci(existing, "BranchSrNo"));
        } else {
            int fy = ctx.currentFinancialYearId();
            docNo = repo.nextDocNo(u, fy, s.documentTypeId);
            branchSrNo = repo.nextBranchSrNo(u, branch(u), fy, s.documentTypeId);
        }
        dto.DocNo = docNo;
        dto.BranchSrNo = branchSrNo;
        h.put("BranchSrNo", branchSrNo);
        h.put("CompanyId", u.getCompanyId());
        h.put("DocNo", docNo);
        h.put("EntryUser", u.getId());
        h.put("Id", recId);
        h.put("ModifyUser", u.getId());
        h.put("OrganizationId", u.getOrganizationId());
        h.put("ProjectsId", branch(u));                              // ProjectsId = UserAccount.BranchesId
        h.put("RefDocNoId", 0);
        h.put("RefDocumentTypeId", 0);
        h.put("DocumentTypeId", s.documentTypeId);
        h.put("FinancialYearId", ctx.currentFinancialYearId());
        h.put("ExImInvoiceId", 0);
        h.put("Remarks", dto.Remarks == null ? "" : dto.Remarks);
        h.put("ManualNo", null);
        /* AttachmentsValues / CustomAttachmentsValues — "" on a new document, the loaded
           header's values on an opened one (ReadById :533). Never taken from the request. */
        h.put("AttachmentsValues", existing == null ? "" : str(ci(existing, "AttachmentsValues")));
        h.put("CustomAttachmentsValues", existing == null ? "" : str(ci(existing, "CustomAttachmentsValues")));
        h.put("ActionId", recId == 0 ? 1 : 2);
        h.put("TypeId", 0);
        h.put("BaseDocumentTypeId", s.baseDocumentTypeId);
        h.put("IsUploaded", false);
        return h;
    }

    /** Architecture.Model.PurchaseTrading.InvGsStoreIssuanceDetail — non-virtual properties, defaults. */
    private static Map<String, Object> detailModel() {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("IssueQty", 0d);
        p.put("AssetsId", 0);
        p.put("IssuanceTypeId", 0);
        p.put("ExImInvoiceId", 0);
        p.put("DepartmentId", 0);
        p.put("Id", 0);
        p.put("LineId", 0);
        p.put("InvGsStoreIssuanceHeaderId", 0);
        p.put("ItemId", 0);
        p.put("ItemUomSchId", 0);
        p.put("ItemConditionId", 0);
        p.put("Warehouseid", 0);
        p.put("SupplierCustomerId", 0);
        p.put("EmployeeId", 0);
        p.put("DrAccountId", 0);
        p.put("BagTypeId", 0);
        p.put("ItemRate", 0d);
        p.put("ItemAmount", 0d);
        p.put("ReamarksDetail", null);
        p.put("RefDocumentTypeId", 0);
        p.put("RefDocId", 0);
        p.put("RefDocDetailId", 0);
        p.put("ActionTypeId", 0);
        p.put("DepartmentToId", 0);
        p.put("DepartRequestDetailId", 0);
        p.put("DepartRequestId", 0);
        p.put("WorkOrderIdId", 0);
        p.put("WorkStationFromId", 0);
        p.put("WorkStationToId", 0);
        p.put("ContractScheduleId", 0);
        p.put("RackId", 0);
        return p;
    }

    /**
     * BLL 0252 Save + DAL 0221 SetData, for 451 and 452, in one transaction.
     *
     *   BLL  DateLock refusal
     *   DAL  header (Insert | Update) → id
     *        details, LineId 1..n over ALL details (removed ones included), header id stamped
     *        Sp_InventoryTransactions_GetALLMethod, Sp_InventoryStockEvalautionDetail_Update
     *        USP_InventoryValidation per detail
     *        [voucher] head Insert|Update, details (RefDocSubIdNo by LineId), H mirrors
     */
    private int persist(UserAccount u, Screen s, Map<String, Object> head,
                        List<Map<String, Object>> details, Map<String, Object> voucher) {
        Timestamp docDate = (Timestamp) head.get("DocDate");
        Timestamp lock = repo.dateLock(u);
        if (lock != null && !docDate.after(lock)) {
            throw new IllegalArgumentException("Not Insert or Update record please check lock date");
        }
        int recId = toInt(head.get("Id"));
        String proc = recId == 0 ? "Sp_InvGsStoreIssuanceHeader_Insert" : "Sp_InvGsStoreIssuanceHeader_Update";
        int num = repo.setProc(proc, head);
        if (num > 0) head.put("Id", num); else num = recId;
        /* DEVIATION (defensive): the DAL would carry on with Id 0 and write orphan details. */
        if (num <= 0) throw new IllegalStateException("Save returned no document id.");

        int line = 1;
        for (Map<String, Object> d : details) {
            d.put("LineId", line++);
            d.put("InvGsStoreIssuanceHeaderId", num);
            d.put("Id", repo.setProc("Sp_InvGsStoreIssuanceDetail_Insert", d));
        }
        repo.postStock(u, s.documentTypeId, num);
        for (Map<String, Object> d : details) {
            repo.inventoryValidation(u, s.documentTypeId, docDate, toInt(d.get("ItemId")),
                    toInt(d.get("ItemConditionId")), toInt(d.get("Warehouseid")), toDouble(d.get("IssueQty")));
        }
        if (voucher != null) writeVoucher(u, s, num, voucher, details);
        return num;
    }

    // ================================================================================== voucher

    /** BLL 0252 MakeVoucher, for DocumentTypeId 452. */
    @SuppressWarnings("unchecked")
    private Map<String, Object> makeVoucher(UserAccount u, Map<String, Object> head,
                                            List<Map<String, Object>> details, Screen s) {
        ContraVoucherDto.Head vh = new ContraVoucherDto.Head();
        vh.DocumentTypeId = s.documentTypeId;
        vh.DocumentTypeSrNo = toInt(head.get("Id"));
        vh.RefAccountId = toInt(head.get("RefDocNoId"));
        vh.VoucherCode = toInt(head.get("DocNo"));
        vh.VoucherDate = String.valueOf(head.get("DocDate"));
        String now = Timestamp.valueOf(LocalDateTime.now()).toString();
        vh.ChequeDate = Timestamp.valueOf(LocalDate.now().atStartOfDay()).toString();   // DateTime.Today
        vh.DueDate = now;                                                                // DateTime.Now
        vh.EntryDate = now;
        vh.ModifyDate = now;
        vh.Remarks = str(head.get("Remarks"));
        vh.RemarksOtherLingo = "";
        vh.IncludeWHT = false;
        vh.BranchId = toInt(head.get("BranchesId"));
        vh.ProjectId = toInt(head.get("ProjectsId"));
        vh.DueDays = 0;
        vh.OrganizationId = u.getOrganizationId();
        vh.CompanyId = u.getCompanyId();
        vh.FinancialYearId = toInt(head.get("FinancialYearId"));
        vh.EntryUser = u.getId();
        vh.ModifyUser = u.getId();

        List<Map<String, Object>> gl = repo.itemGlAccounts(u);
        List<ContraVoucherDto.Detail> lines = new ArrayList<>();
        for (Map<String, Object> item : details) {                                  // D2: all of them
            vh.RefAccountId = 0;                                                     // (451 ? DrAccountId : 0)
            if (gl.isEmpty()) throw new IllegalArgumentException("Item Record Not found");
            Map<String, Object> g = null;
            int itemId = toInt(item.get("ItemId"));
            for (Map<String, Object> x : gl) if (toInt(x.get("Id")) == itemId) { g = x; break; }
            if (g == null) throw new IllegalArgumentException("Item Purchase GL Account Not found");
            double qty = toDouble(item.get("IssueQty"));
            double rate = toDouble(item.get("ItemRate"));
            double amount = toDouble(item.get("ItemAmount"));
            int drAc = toInt(item.get("DrAccountId"));
            int cogs = toInt(g.get("COGSGLAC"));
            int purchase = toInt(g.get("PurchaseGLAC"));
            String comments = str(head.get("Remarks")) + "  Qty: " + clr(qty) + "   " + str(g.get("ItemName")) + "   Rate:" + clr(rate);

            ContraVoucherDto.Detail dr = new ContraVoucherDto.Detail();
            dr.LineId = toInt(item.get("LineId"));
            dr.AccountId = drAc > 0 ? drAc : cogs;
            dr.AgainstAccountId = purchase;
            dr.Comments = comments;
            dr.DebitAmount = (double) (float) amount;                               // D3
            dr.CreditAmount = 0d;
            dr.ItemId = itemId;
            dr.QtyOut = qty;
            dr.ItemRate = rate;
            dr.ItemAmount = amount;
            dr.BranchesId = toInt(head.get("BranchesId"));
            lines.add(dr);

            ContraVoucherDto.Detail cr = new ContraVoucherDto.Detail();
            cr.LineId = toInt(item.get("LineId"));
            cr.AccountId = purchase;
            cr.AgainstAccountId = drAc > 0 ? drAc : cogs;
            cr.Comments = comments;
            cr.CreditAmount = (double) (float) amount;                              // D3
            cr.DebitAmount = 0d;
            cr.ItemId = itemId;
            cr.QtyOut = qty;
            cr.ItemRate = rate;
            cr.ItemAmount = amount;
            cr.BranchesId = toInt(head.get("BranchesId"));
            lines.add(cr);
        }
        Map<String, Object> v = new LinkedHashMap<>();
        v.put("head", vh);
        v.put("lines", lines);
        return v;
    }

    /** DAL 0221 :171-218. */
    @SuppressWarnings("unchecked")
    private void writeVoucher(UserAccount u, Screen s, int docId, Map<String, Object> voucher,
                              List<Map<String, Object>> details) {
        ContraVoucherDto.Head vh = (ContraVoucherDto.Head) voucher.get("head");
        List<ContraVoucherDto.Detail> lines = (List<ContraVoucherDto.Detail>) voucher.get("lines");
        int existingId = repo.voucherHeadId(u, s.documentTypeId, docId);
        vh.Id = existingId;
        vh.DocumentTypeSrNo = docId;
        int num2 = repo.setProc(existingId == 0 ? "Sp_VoucherHead_Insert" : "Sp_VoucherHead_Update", model(vh));
        if (num2 > 0) vh.Id = num2;
        if (lines == null || lines.isEmpty()) throw new IllegalArgumentException("VoucherDetail List Not Found");
        for (ContraVoucherDto.Detail l : lines) {
            for (Map<String, Object> d : details) {
                if (toInt(d.get("LineId")) == (l.LineId == null ? 0 : l.LineId)) { l.RefDocSubIdNo = toInt(d.get("Id")); break; }
            }
            l.VoucherHeadId = vh.Id;
            repo.setProc("Sp_VoucherDetail_Insert", model(l));
        }
        int documentTypeIdRef = repo.setProc("Sp_VoucherHead_H_Insert", model(vh));
        for (ContraVoucherDto.Detail l : lines) {
            l.VoucherHeadId = vh.Id;
            l.DocumentTypeIdRef = documentTypeIdRef;
            repo.setProc("Sp_VoucherDetail_H_Insert", model(l));
        }
    }

    /** Public fields in declaration order — the DTOs mirror the desktop models field for field. */
    private static Map<String, Object> model(Object o) {
        Map<String, Object> m = new LinkedHashMap<>();
        for (Field f : o.getClass().getFields()) {
            if (java.lang.reflect.Modifier.isStatic(f.getModifiers())) continue;
            if (List.class.isAssignableFrom(f.getType())) continue;
            try {
                Object v = f.get(o);
                if (v != null && ("VoucherDate".equals(f.getName()) || "ChequeDate".equals(f.getName())
                        || "DueDate".equals(f.getName()) || "EntryDate".equals(f.getName())
                        || "ModifyDate".equals(f.getName()) || "PostDate".equals(f.getName())
                        || "DCheqDate".equals(f.getName()) || "GpDate".equals(f.getName()))) {
                    v = Timestamp.valueOf(String.valueOf(v).length() == 10 ? v + " 00:00:00" : String.valueOf(v));
                }
                m.put(f.getName(), v);
            } catch (IllegalAccessException ignored) { }
        }
        return m;
    }

    // ================================================================================= helpers

    /** The header must exist, be this screen's document type, and belong to the user's company. */
    private Map<String, Object> ownedHeader(UserAccount u, Screen s, int id) {
        if (id <= 0) return null;
        Map<String, Object> h = repo.header(id);
        if (h == null) return null;
        if (toInt(h.get("DocumentTypeId")) != s.documentTypeId) return null;
        if (h.containsKey("CompanyId") && toInt(h.get("CompanyId")) != u.getCompanyId()) return null;
        if (h.containsKey("OrganizationId") && toInt(h.get("OrganizationId")) != u.getOrganizationId()) return null;
        return h;
    }

    /** FormHelper.ValidateField — 0, non-positive and blank are "required". */
    private static void validate(Object v, String field, int rowIndex) {
        boolean bad = v == null
                || (v instanceof Integer && (Integer) v == 0)
                || (v instanceof Double && (Double) v <= 0d)
                || (v instanceof String && ((String) v).trim().isEmpty());
        if (bad) throw new IllegalArgumentException(field + " is required in Detail Grid at row No: " + (rowIndex + 1));
    }

    /**
     * A DateTimePicker's Value carries the time of day the form was opened; the desktop saves and
     * queries with it. The page sends a date, so the current time is attached the same way.
     */
    static Timestamp pickerDate(String yyyyMMdd) {
        LocalDate d = isBlank(yyyyMMdd) ? LocalDate.now() : LocalDate.parse(yyyyMMdd.trim().substring(0, 10));
        return Timestamp.valueOf(LocalDateTime.of(d, LocalTime.now().withNano(0)));
    }

    /**
     * txtDocdate.Value at save time. ReadById assigns {@code head.DocDate} (a DATE column, so
     * midnight) and a picker keeps that time when the day is changed, so an opened document saves —
     * and meets the BLL's lock-date check {@code DocDate <= lock} — at midnight. A new document
     * carries the time of day the form was opened.
     */
    static Timestamp formDate(String yyyyMMdd, int recId) {
        if (recId > 0) {
            LocalDate d = isBlank(yyyyMMdd) ? LocalDate.now() : LocalDate.parse(yyyyMMdd.trim().substring(0, 10));
            return Timestamp.valueOf(d.atStartOfDay());
        }
        return pickerDate(yyyyMMdd);
    }

    /** {@code .Value.Date} — midnight. */
    static Timestamp dateOnly(String yyyyMMdd) {
        if (isBlank(yyyyMMdd)) return null;
        return Timestamp.valueOf(LocalDate.parse(yyyyMMdd.trim().substring(0, 10)).atStartOfDay());
    }

    private String financialYearStart(UserAccount u, int yearId) {
        try {
            List<Map<String, Object>> years = jdbc.queryForList(
                    "EXEC dbo.Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId @OrganizationId=?, @CompanyId=?",
                    u.getOrganizationId(), u.getCompanyId());
            for (Map<String, Object> r : years) {
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

    private static int branch(UserAccount u) { return u.getBranchesId() == null ? 0 : u.getBranchesId(); }
    private static int i(Integer v) { return v == null ? 0 : v; }
    private static double d(Object v) { return toDouble(v); }
    private static boolean isBlank(String s) { return s == null || s.trim().isEmpty(); }
    private static double round(double v, int places) {
        return java.math.BigDecimal.valueOf(v).setScale(places, java.math.RoundingMode.HALF_EVEN).doubleValue();
    }
    static boolean toBool(String v) {
        if (v == null) return false;
        String t = v.trim();
        return "true".equalsIgnoreCase(t) || "1".equals(t);
    }

    private static Map<String, Object> ok(String message, int id) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", true);
        m.put("message", message);
        m.put("id", id);
        return m;
    }
}
