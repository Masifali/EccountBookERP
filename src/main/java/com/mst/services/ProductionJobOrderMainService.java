package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.dto.ProductionJobOrderMainDto;
import com.mst.repositories.ProductionJobOrderMainRepository;
import com.mst.security.CurrentUserContext;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Calendar;
import java.util.Date;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Locale;
import java.util.Map;

/**
 * Screen 281 "Production Job Order" — {@code frmProductionJobOrderMain.cs}, DocumentTypeId 403.
 *
 * ---------------------------------------------------------------------------------------------
 * VALIDATION IS THE DESKTOP'S, MESSAGE FOR MESSAGE AND IN ITS ORDER
 * ---------------------------------------------------------------------------------------------
 * Insert():1678 runs FormValidation() (eight header checks), then the two date comparisons, then
 * the two effective-date comparisons, then "At Least One Detail Record Is Required", then the two
 * rate-grid checks. The same order is reproduced below so the first message an operator sees is
 * the same message on both apps, with the desktop's own spelling — "Mannual ReportNo",
 * "workinprocess field required", "By Finish Goods Rate Schedule" — left exactly as it is.
 *
 * ---------------------------------------------------------------------------------------------
 * DOCUMENT TYPE, TENANCY AND USER COME FROM THE SERVER
 * ---------------------------------------------------------------------------------------------
 * DocumentTypeId is fixed at 403 by the form itself (Insert():1737). Organization, Company,
 * Branch, FinancialYear, EntryUser and ModifyUser are read from the signed-in user. None is taken
 * from the request body: a client-supplied DocumentTypeId would let this screen write another
 * screen's documents, and a client-supplied user id would forge authorship.
 *
 * ---------------------------------------------------------------------------------------------
 * THE FIELDS THIS SCREEN LEAVES ALONE
 * ---------------------------------------------------------------------------------------------
 * Insert() never assigns ProjectsId, RefSalesOrderId, JobLotId, PendingForView,
 * DeliveryInstructions, PackingInstructions or ProductionInsturction. They are forced back to
 * their defaults here rather than passed through, so a request cannot write columns the desktop
 * screen has no control for. AssumedPlantCapacity (plant rows) and Remarks (rate rows) are the
 * same case: the grids have no such column.
 */
@Service
public class ProductionJobOrderMainService {

    private static final Logger LOG = LoggerFactory.getLogger(ProductionJobOrderMainService.class);

    /** frmProductionJobOrderMain.cs:1737 — invProductionJobOrder.DocumentTypeId = 403. */
    public static final int DOCUMENT_TYPE_ID = 403;

    /** The desktop screen name, for the per-screen grant lookup. */
    private static final String SCREEN_NAME = "frmProductionJobOrderMain";

    private static final String SQL_USER_RIGHTS =
            "EXEC dbo.Sp_tblUserRights_GetAllMethod @UserId=?, @ScreenName=?, @RightName=?, "
          + "@CompanyId=?, @Activity=?";

    /** StatusBind():746 builds this table in the form — it is not a database list on the desktop
     *  either, so it is a literal here rather than an invented table. */
    private static final String[][] STATUSES = {
            { "1", "In Process" }, { "2", "Complete" }, { "3", "Cancel" }
    };

    /** ParameterFill():547 — CommonServices.DateType(), likewise a literal on the desktop. */
    private static final String[][] DATE_TYPES = {
            { "1", "This Day" }, { "2", "This Week" }, { "3", "This Month" },
            { "4", "This Year" }, { "5", "Financial Year" }
    };

    @Autowired private ProductionJobOrderMainRepository repo;
    @Autowired private CurrentUserContext currentUserContext;
    @Autowired private JdbcTemplate jdbcTemplate;

    // =============================================================================== load

    /**
     * frmProductionJobOrder_Load:367 — rights, the two configuration flags, the twelve pickers,
     * and the last saved WIP account/item/warehouse.
     */
    public Map<String, Object> lookups() {
        UserAccount u = currentUserContext.requireAccountingUser();
        int branchId = u.getBranchesId() == null ? 0 : u.getBranchesId();
        int appId = currentUserContext.currentAppId();

        Map<String, Object> out = new LinkedHashMap<>();

        /* :372-375 — Save, Update and Print are enabled from the grant row. */
        out.put("canSave",   hasRight("Save"));
        out.put("canUpdate", hasRight("Update"));
        out.put("canPrint",  hasRight("Print"));

        /* :377-378 — both come from the company's configuration, not from the client. */
        boolean byProductRateEditable = configFlag(u, "ByProductRateEditableIsAllow");
        boolean generateJobOrderNo    = configFlag(u, "GenerateJobOrderNo");
        out.put("byProductRateEditableIsAllow", byProductRateEditable);
        out.put("generateJobOrderNo", generateJobOrderNo);

        out.put("plants",          repo.plantsAllocatedToBranch(u));
        out.put("wipItems",        repo.wipItems(u));
        /* One call, three pickers — CmbWorkInProcess, CmbFinishGoodsAc and CmbByProductionAc all
           bind the same DataTable on the desktop (AccountFills():664). */
        out.put("accounts",        repo.accountsByType(u, appId, "4"));
        out.put("planTypes",       repo.planTypes());
        out.put("productionTypes", repo.productionTypes());
        out.put("warehouses",      repo.warehousesAllocatedToBranch(u, branchId));
        out.put("items",           repo.allItems(u));
        out.put("statuses",        literal(STATUSES, "Status"));
        out.put("dateTypes",       literal(DATE_TYPES, "Parameters"));
        out.put("jobOrderNos",     jobOrderNoFilter(repo.jobOrderNoDropDown(u)));
        out.put("generatedJobOrderNos",
                generateJobOrderNo ? repo.generatedJobOrderNos(u, DOCUMENT_TYPE_ID, null)
                                   : new ArrayList<Map<String, Object>>());

        /* GeneratePlanCode():682 — the next plan code fills the form on open. */
        out.put("planCode", repo.nextCode(u, DOCUMENT_TYPE_ID, currentUserContext.currentFinancialYearId()));

        /* GetLastSaveAccount():568 — three ids the form restores from the last saved job order. */
        Map<String, Object> last = repo.lastWipAccount(u, DOCUMENT_TYPE_ID);
        out.put("lastWipAccountId",   last == null ? 0 : intOf(ci(last, "AccountId")));
        out.put("lastWipItemId",      last == null ? 0 : intOf(ci(last, "WipItemId")));
        out.put("lastWipWareHouseId", last == null ? 0 : intOf(ci(last, "WipWareHouseId")));

        /* cmbperemeter_ValueChanged:2228, option 5 "Financial Year" — the desktop reads
           clsGlobalVariables.ActiveYr.Start_Period. */
        out.put("financialYearStart", financialYearStart(u));
        return out;
    }

    /**
     * ActiveYr.Start_Period for the signed-in user's financial year. Read from the active-year
     * procedure rather than guessed, and returned as-is so the page formats it.
     */
    private String financialYearStart(UserAccount u) {
        int yearId = currentUserContext.currentFinancialYearId();
        try {
            List<Map<String, Object>> years = jdbcTemplate.queryForList(
                    "EXEC dbo.Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId "
                  + "@OrganizationId=?, @CompanyId=?",
                    u.getOrganizationId(), u.getCompanyId());
            Map<String, Object> row = null;
            for (Map<String, Object> r : years) {
                if (intOf(ci(r, "Id")) == yearId) { row = r; break; }
            }
            if (row == null && !years.isEmpty()) row = years.get(0);
            if (row != null) {
                Object v = ci(row, "Start_Period");
                if (v != null) return day(v);
            }
        } catch (Exception e) {
            LOG.warn("Could not read the active financial year's start period", e);
        }
        return null;
    }

    /**
     * JobOrderNofill():508 keeps only the rows whose Activity column reads "JobOrderNo" and shows
     * the ReferenceName column. Done in memory here because that is where the desktop does it —
     * adding a WHERE clause would ask the procedure a question the desktop never asks.
     */
    private List<Map<String, Object>> jobOrderNoFilter(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Object activity = ci(r, "Activity");
            if (activity != null && "JobOrderNo".equals(String.valueOf(activity).trim())) {
                Map<String, Object> o = new LinkedHashMap<>();
                o.put("Id",   ci(r, "Id"));
                o.put("Name", ci(r, "ReferenceName"));
                out.add(o);
            }
        }
        return out;
    }

    /**
     * ReadById(ID):1905 — the header, the plant grid, the two rate grids split on TransTypeId,
     * and the Export Schedule grid.
     */
    public Map<String, Object> load(int id) {
        currentUserContext.requireAccountingUser();
        Map<String, Object> head = repo.header(id);
        if (head == null) return null;

        Map<String, Object> h = new LinkedHashMap<>();
        h.put("Id",                 ci(head, "Id"));
        h.put("PlanDate",           day(ci(head, "PlanDate")));
        h.put("PlanCode",           ci(head, "PlanCode"));
        h.put("WorkInProccessAcId", ci(head, "WorkInProccessAcId"));
        h.put("FinishGoodsAcId",    ci(head, "FinishGoodsAcId"));
        h.put("ByProductacId",      ci(head, "ByProductacId"));
        h.put("WipItemId",          ci(head, "WipItemId"));
        h.put("WipWareHouseId",     ci(head, "WipWareHouseId"));
        h.put("PlanType",           ci(head, "PlanType"));
        h.put("PlanTypeSrNo",       ci(head, "PlanTypeSrNo"));
        h.put("RefInvoiceNo",       ci(head, "RefInvoiceNo"));
        h.put("LotReference",       ci(head, "LotReference"));
        h.put("ProductionType",     ci(head, "ProductionType"));
        h.put("StartDate",          day(ci(head, "StartDate")));
        h.put("EndDate",            day(ci(head, "EndDate")));
        h.put("PlanStatus",         ci(head, "PlanStatus"));
        h.put("FinishGoodRate",     ci(head, "FinishGoodRate"));
        h.put("OtherInstructions",  ci(head, "OtherInstructions"));
        /* InvProductionPlantId is the header's own Plant/Feader picker — the desktop does not
           restore it in ReadById, so it is reported here but the page leaves it as the form does. */
        h.put("InvProductionPlantId", ci(head, "InvProductionPlantId"));

        List<Map<String, Object>> plants = new ArrayList<>();
        for (Map<String, Object> r : repo.plantRows(id)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id",            ci(r, "Id"));
            o.put("PlantId",       ci(r, "PlantId"));
            o.put("PlantName",     ci(r, "PlantName"));
            o.put("RemarksDetail", ci(r, "RemarksDetail"));
            plants.add(o);
        }

        /* :1952 — one list, split on TransTypeId: 1 = By Product, 2 = Finish Goods. */
        List<Map<String, Object>> byProduct = new ArrayList<>();
        List<Map<String, Object>> finishGoods = new ArrayList<>();
        for (Map<String, Object> r : repo.rateScheduleRows(id)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id",            ci(r, "Id"));
            o.put("EffectiveDate", day(ci(r, "EffectiveDate")));
            o.put("ItemId",        ci(r, "ItemId"));
            o.put("ItemName",      ci(r, "ItemName"));
            o.put("Rate40Kg",      ci(r, "ItemRate"));
            int trans = intOf(ci(r, "TransTypeId"));
            if (trans == 1) byProduct.add(o);
            else if (trans == 2) finishGoods.add(o);
            /* Any other value is dropped, exactly as the desktop's if/else-if does — it is not
               silently filed under one of the two grids. */
        }

        List<Map<String, Object>> schedule = new ArrayList<>();
        for (Map<String, Object> r : repo.allocationRows(id)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id",           ci(r, "Id"));
            o.put("ContractId",   ci(r, "OrderId"));
            o.put("SaleContract", ci(r, "ContractNo"));
            o.put("ScheduleId",   ci(r, "ScheduleId"));
            o.put("ScheduleNo",   ci(r, "ScheduleNo"));
            o.put("ItemId",       ci(r, "ItemId"));
            o.put("Item",         ci(r, "ItemName"));
            o.put("MTon",         ci(r, "MTon"));
            o.put("Remarks",      ci(r, "Remarks"));
            schedule.add(o);
        }

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("header",      h);
        out.put("plants",      plants);
        out.put("byProduct",   byProduct);
        out.put("finishGoods", finishGoods);
        out.put("schedule",    schedule);
        return out;
    }

    // =============================================================================== save

    @Transactional
    public Map<String, Object> save(ProductionJobOrderMainDto dto) {
        UserAccount u = currentUserContext.requireAccountingUser();

        boolean insert = dto.Id == null || dto.Id <= 0;
        /* :372-374 — the desktop simply disables the buttons. Refusing here is the same rule made
           enforceable: a disabled button is not a permission check. */
        if (insert && !hasRight("Save")) {
            throw new IllegalStateException("You do not have the Save right for this screen.");
        }
        if (!insert && !hasRight("Update")) {
            throw new IllegalStateException("You do not have the Update right for this screen.");
        }

        boolean byProductRateEditable = configFlag(u, "ByProductRateEditableIsAllow");
        validate(dto, byProductRateEditable);

        /* Server-owned, every time — whatever the request body said is discarded. */
        dto.DocumentTypeId  = DOCUMENT_TYPE_ID;
        dto.OrganizationId  = u.getOrganizationId();
        dto.CompanyId       = u.getCompanyId();
        dto.BranchesId      = u.getBranchesId() == null ? 0 : u.getBranchesId();
        dto.FinancialYearId = currentUserContext.currentFinancialYearId();
        dto.EntryUser       = u.getId();
        dto.ModifyUser      = u.getId();
        dto.SupplierCustomerId = 0;                 // :1756 — the form always sends 0
        dto.IsApproved      = Boolean.FALSE;        // :1755 — the form saves false
        dto.PendingForView  = Boolean.FALSE;

        /* Columns this screen has no control for. Forced back rather than passed through. */
        dto.ProjectsId             = 0;
        dto.RefSalesOrderId        = 0;
        dto.JobLotId               = 0;
        dto.DeliveryInstructions   = null;
        dto.PackingInstructions    = null;
        dto.ProductionInsturction  = null;

        String now = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss").format(new Date());
        dto.EntryDate  = now;
        dto.ModifyDate = now;

        for (ProductionJobOrderMainDto.Plant p : dto.InvProductionJobOrderPlantslist) {
            /* GridDetail carries Id, PlantId and RemarksDetail and nothing else (:1789). */
            p.AssumedPlantCapacity = java.math.BigDecimal.ZERO;
            p.PlantName = null;
        }
        for (ProductionJobOrderMainDto.RateSchedule r : dto.JobOrderRateSchedulelist) {
            /* :1798 / :1816 set TransTypeId themselves; the grid has no Remarks column. */
            r.Remarks  = null;
            r.ItemName = null;
        }
        for (ProductionJobOrderMainDto.OrderAllocation a : dto.InvProductionJobOrderAndOrderAllocationList) {
            a.OrderTypeId = 1;                      // :1825 — fixed by the form
            a.ItemName = null; a.ContractNo = null; a.ScheduleNo = null;
        }

        int id = repo.save(dto);

        Map<String, Object> res = new LinkedHashMap<>();
        res.put("success", true);
        res.put("id", id);
        res.put("planCode", dto.PlanCode);
        /* The desktop's own confirmation text (:1847 / :1851). */
        res.put("message", (insert ? "Save Successfully" : "Update Successfully")
                         + (dto.PlanCode == null ? "" : dto.PlanCode));
        return res;
    }

    /** Insert():1678 and FormValidation():1616, in that order, with the desktop's wording. */
    private void validate(ProductionJobOrderMainDto dto, boolean byProductRateEditable) {
        /* ---- FormValidation():1616 ---- */
        if (dto.PlanCode == null || dto.PlanCode == 0) {
            throw new IllegalArgumentException("Plan Code Field Required");
        }
        if (isBlank(dto.RefInvoiceNo) || "0".equals(dto.RefInvoiceNo.trim())) {
            throw new IllegalArgumentException("Mannual ReportNo Field Required");
        }
        if (isBlank(dto.ProductionType)) {
            throw new IllegalArgumentException("Production Type Field Required");
        }
        if (isBlank(dto.PlanType)) {
            throw new IllegalArgumentException("Plan Type Field Required");
        }
        if (isBlank(dto.PlanStatus)) {
            throw new IllegalArgumentException("Status Field Required");
        }
        if (dto.WorkInProccessAcId == null || dto.WorkInProccessAcId == 0) {
            throw new IllegalArgumentException("workinprocess field required");
        }
        if (dto.WipItemId == null || dto.WipItemId == 0) {
            throw new IllegalArgumentException("WIPItem Field Required");
        }
        if (dto.WipWareHouseId == null || dto.WipWareHouseId == 0) {
            throw new IllegalArgumentException("WIP Warehouse Field Required");
        }
        /* FinishGoodsAc and ByProductionAc are deliberately NOT required — the desktop does not
           check them, and adding a check would refuse job orders the desktop accepts. */

        /* The two rate grids are one list here, told apart by TransTypeId. The desktop sets it
           itself per grid (:1798 / :1816), so anything other than 1 or 2 could only come from a
           hand-made request and would land the row in neither grid on reload. */
        for (ProductionJobOrderMainDto.RateSchedule r : dto.JobOrderRateSchedulelist) {
            int t = r.TransTypeId == null ? 0 : r.TransTypeId;
            if (t != 1 && t != 2) {
                throw new IllegalArgumentException(
                        "Rate schedule rows must be By Product (1) or Finish Goods (2)");
            }
        }

        /* ---- Insert():1696 — the desktop compares .Date, so the time of day is dropped. ---- */
        Date plan  = dayOnly(parseDay(dto.PlanDate));
        Date start = dayOnly(parseDay(dto.StartDate));
        Date end   = dayOnly(parseDay(dto.EndDate));
        if (plan != null && start != null && plan.after(start)) {
            throw new IllegalArgumentException("Plan Date Cannot Be Greater Than StartDate");
        }
        if (start != null && end != null && start.after(end)) {
            throw new IllegalArgumentException("Start Date Cannot Be Greater Than End Date");
        }

        /* :1708 / :1714 — the two rate grids may not be dated before the plan date. */
        for (ProductionJobOrderMainDto.RateSchedule r : dto.JobOrderRateSchedulelist) {
            if (r.TransTypeId != null && r.TransTypeId == 1) {
                Date d = dayOnly(parseDay(r.EffectiveDate));
                if (plan != null && d != null && d.before(plan)) {
                    throw new IllegalArgumentException(
                            "By Product Effective Date Cannot Be Less Than Plan Date");
                }
            }
        }
        for (ProductionJobOrderMainDto.RateSchedule r : dto.JobOrderRateSchedulelist) {
            if (r.TransTypeId != null && r.TransTypeId == 2) {
                Date d = dayOnly(parseDay(r.EffectiveDate));
                if (plan != null && d != null && d.before(plan)) {
                    throw new IllegalArgumentException(
                            "Finish Goods Effective Date Cannot Be Less Than Plan Date");
                }
            }
        }

        /* :1734 — the plant grid. */
        if (dto.InvProductionJobOrderPlantslist.isEmpty()) {
            throw new IllegalArgumentException("At Least One Detail Record Is Required");
        }

        int byProduct = 0, finishGoods = 0;
        for (ProductionJobOrderMainDto.RateSchedule r : dto.JobOrderRateSchedulelist) {
            if (r.TransTypeId != null && r.TransTypeId == 1) byProduct++;
            else if (r.TransTypeId != null && r.TransTypeId == 2) finishGoods++;
        }
        /* :1797 — By Product rows are required ONLY when the configuration says the rate is not
           editable. The flag comes from the company configuration, never from the request. */
        if (!byProductRateEditable && byProduct == 0) {
            throw new IllegalArgumentException(
                    "At Least One Record Is Required in By Product Rate Schedule");
        }
        /* :1812 — the desktop's own wording, "By Finish Goods", is kept. */
        if (finishGoods == 0) {
            throw new IllegalArgumentException(
                    "At Least One Record Is Required in By Finish Goods Rate Schedule");
        }
    }

    // ============================================================================ history

    /**
     * BindHistoryGrid():2022 — FormHistoryNew, and the 18 columns it projects.
     *
     * The caller chooses only the date MODE and the filter values the desktop offers.
     * canViewAllRecord and the user id are derived here from the signed-in user's grant row.
     */
    public Map<String, Object> history(String dateMode, String fromDate, String toDate,
                                       int docNoFrom, int docNoTo, int jobOrderId) {
        UserAccount u = currentUserContext.requireAccountingUser();
        boolean canViewAll = hasRight("CanView AllRecord");

        List<Map<String, Object>> raw = repo.history(
                u, currentUserContext.currentFinancialYearId(), canViewAll, u.getId(),
                dateMode, fromDate, toDate, docNoFrom, docNoTo, jobOrderId);

        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> r : raw) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id",             ci(r, "Id"));
            o.put("Date",           day(ci(r, "PlanDate")));
            o.put("Code",           ci(r, "PlanCode"));
            o.put("Type",           ci(r, "PlanType"));
            o.put("Production#",    ci(r, "RefInvoiceNo"));
            o.put("PType",          ci(r, "ProductionType"));
            o.put("StartDate",      day(ci(r, "StartDate")));
            o.put("EndDate",        day(ci(r, "EndDate")));
            o.put("PlanStatus",     ci(r, "PlanStatus"));
            o.put("OtherInst",      ci(r, "OtherInstructions"));
            o.put("EntryUser",      ci(r, "EntryUserName"));
            o.put("EntryDate",      day(ci(r, "EntryDate")));
            o.put("ModifyUser",     ci(r, "ModifyUserName"));
            o.put("ModifyDate",     day(ci(r, "ModifyDate")));
            o.put("ApprovedStatus", ci(r, "ApprovalStatus"));
            o.put("ApprovedUser",   ci(r, "ApprovedUserName"));
            /* The desktop fills its ApprovedDate column from the PostDate column of the result
               set (:2104). Reproduced, not corrected. */
            o.put("ApprovedDate",   day(ci(r, "PostDate")));
            o.put("Attachments",    ci(r, "NoOfAttachments"));
            rows.add(o);
        }

        Map<String, Object> res = new LinkedHashMap<>();
        res.put("canViewAllRecords", canViewAll);
        res.put("rows", rows);
        return res;
    }

    // ================================================================ export schedule loader

    /**
     * LoadExportScheduleForProduction — the dialog's four pickers.
     *
     * ScheduleComboBind():196 splits ONE result set into three lists on its Activity column and
     * ThirdPartyFill():258 filters a second on ActivityType. Both splits happen in memory on the
     * desktop, so both happen in memory here rather than as extra @Activity round-trips.
     */
    public Map<String, Object> schedulePickerLookups() {
        UserAccount u = currentUserContext.requireAccountingUser();
        List<Map<String, Object>> all = repo.shipmentScheduleDropDown(u);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("contracts", splitOn(all, "Activity", "ContractNo", "ReferenceName"));
        out.put("customers", splitOn(all, "Activity", "Customer",   "ReferenceName"));
        out.put("items",     splitOn(all, "Activity", "Item",       "ReferenceName"));
        out.put("thirdParties", splitOn(repo.thirdPartyDropDown(u, "TrackingNo"),
                                        "ActivityType", "TrackingNo", "name"));
        /* InitializeComponentMethod():140 opens the dialog at today - 7. */
        out.put("defaultFromDate", isoDaysAgo(7));
        return out;
    }

    /** {Id, Name} for the rows whose discriminator column matches, in the source's own order. */
    private static List<Map<String, Object>> splitOn(List<Map<String, Object>> rows,
                                                     String column, String value, String nameColumn) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Object v = ci(r, column);
            if (v != null && value.equals(String.valueOf(v).trim())) {
                Map<String, Object> o = new LinkedHashMap<>();
                o.put("Id",   ci(r, "Id"));
                o.put("Name", ci(r, nameColumn));
                out.add(o);
            }
        }
        return out;
    }

    public List<Map<String, Object>> schedule(String fromDate, String toDate, int itemId,
                                              int contractId, int supplierCustomerId,
                                              int thirdPartyId) {
        UserAccount u = currentUserContext.requireAccountingUser();
        return repo.exportSchedule(u, fromDate, toDate, itemId, contractId,
                                   supplierCustomerId, thirdPartyId);
    }

    // ================================================================= per-row delete guards

    /**
     * GridDetail_ColumnButtonClick:1016 — a plant row already saved on this job order may be
     * removed only if no Production document has consumed that plant.
     *
     * A row being removed from an UNSAVED job order never reaches here: the desktop deletes it
     * from the grid without asking, and so does the page.
     */
    public Map<String, Object> plantDeletable(int jobOrderId, int plantId, String plantName) {
        UserAccount u = currentUserContext.requireAccountingUser();
        boolean used = repo.plantUsedInProduction(u, jobOrderId, plantId);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("deletable", !used);
        if (used) {
            String name = plantName == null ? "" : plantName;
            /* The desktop's sentence, doubled plant name and all (:1073). */
            out.put("message", "You cannot be delete " + name + " because " + name
                             + " Has been refered to production");
        }
        return out;
    }

    /**
     * grdByProduct / grdFinishGoods delete — removing a SAVED rate-schedule row deletes it at
     * once (:1226 / :1428), because USP_JobOrderRateSchedule_Insert only ever inserts.
     *
     * {@code USP_JoBOrderScheduleDeleteById} takes an Id and nothing else — no parent, no
     * organization, no company. Handed a stray Id it would delete another job order's row, so the
     * row is first proved to belong to the job order being edited, and the Update right is
     * required. Neither check exists on the desktop; both are the minimum that makes an
     * id-only delete safe to expose over HTTP.
     */
    @Transactional
    public Map<String, Object> deleteRateScheduleRow(int jobOrderId, int detailId) {
        currentUserContext.requireAccountingUser();
        if (!hasRight("Update")) {
            throw new IllegalStateException("You do not have the Update right for this screen.");
        }
        if (jobOrderId <= 0 || detailId <= 0) {
            throw new IllegalArgumentException("Record Not Found");
        }
        boolean belongs = false;
        for (Map<String, Object> r : repo.rateScheduleRows(jobOrderId)) {
            if (intOf(ci(r, "Id")) == detailId) { belongs = true; break; }
        }
        if (!belongs) {
            throw new IllegalArgumentException("That rate schedule row is not on this job order.");
        }
        repo.deleteRateScheduleRow(detailId);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        return out;
    }

    // ============================================================================ helpers

    /** cmbPlanType_Leave:768 — the per-plan-type serial beside Plan Type. */
    public int nextPlanTypeCode(String planType) {
        UserAccount u = currentUserContext.requireAccountingUser();
        if (isBlank(planType)) return 0;
        return repo.nextPlanTypeCode(u, planType.trim(), currentUserContext.currentFinancialYearId());
    }

    /** GeneratePlanCode:682. */
    public int nextCode() {
        UserAccount u = currentUserContext.requireAccountingUser();
        return repo.nextCode(u, DOCUMENT_TYPE_ID, currentUserContext.currentFinancialYearId());
    }

    private boolean configFlag(UserAccount u, String name) {
        try {
            return repo.config(u.getOrganizationId(), u.getCompanyId(), name);
        } catch (Exception e) {
            /* Reported and treated as off — which is what the desktop does with a missing row,
               since GetConfigValueFromGlobal returns "" and Conversion.ToBool("") is false. */
            LOG.warn("Configuration '{}' could not be read; treating as off", name, e);
            return false;
        }
    }

    /**
     * CommonServices.SetRightsValueInRightsObject:17565 — the grant row for THIS screen, with the
     * desktop's own Admin short-circuit (:17582: a user whose RoleName is "Admin" gets Save,
     * Update, Print, Delete and CanView AllRecord regardless of the stored rows).
     */
    private boolean hasRight(String rightName) {
        String role = currentUserContext.currentRoleName();
        if ("Admin".equals(role)) return true;
        try {
            List<Map<String, Object>> rights = jdbcTemplate.queryForList(
                    SQL_USER_RIGHTS,
                    currentUserContext.currentUserId(),
                    SCREEN_NAME,
                    role == null ? "" : role,
                    currentUserContext.currentCompanyId(),
                    "GetByUserId");
            for (Map<String, Object> r : rights) {
                Object name = ci(r, "RightName");
                if (name != null && rightName.equals(String.valueOf(name).trim())) {
                    return toBool(ci(r, "Value"));
                }
            }
        } catch (Exception e) {
            LOG.warn("Could not read the '{}' right for {}; denying", rightName, SCREEN_NAME, e);
        }
        return false;
    }

    private static List<Map<String, Object>> literal(String[][] pairs, String nameColumn) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (String[] p : pairs) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", Integer.valueOf(p[0]));
            o.put(nameColumn, p[1]);
            out.add(o);
        }
        return out;
    }

    private static boolean isBlank(String s) { return s == null || s.trim().isEmpty(); }

    private static Object ci(Map<String, Object> row, String name) {
        if (row == null) return null;
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet()) {
            if (e.getKey() != null && e.getKey().equalsIgnoreCase(name)) return e.getValue();
        }
        return null;
    }

    private static int intOf(Object v) {
        if (v instanceof Number) return ((Number) v).intValue();
        if (v == null) return 0;
        try { return Integer.parseInt(String.valueOf(v).trim()); }
        catch (NumberFormatException e) { return 0; }
    }

    private static boolean toBool(Object v) {
        if (v instanceof Boolean) return (Boolean) v;
        if (v instanceof Number)  return ((Number) v).intValue() != 0;
        if (v == null) return false;
        String s = String.valueOf(v).trim();
        return "1".equals(s) || "true".equalsIgnoreCase(s) || "yes".equalsIgnoreCase(s);
    }

    /** "yyyy-MM-dd" for the page's date inputs, whatever shape the driver handed back. */
    private static String day(Object v) {
        Date d = parseDay(v);
        if (d == null) return v == null ? "" : String.valueOf(v);
        return new SimpleDateFormat("yyyy-MM-dd", Locale.ENGLISH).format(d);
    }

    private static Date parseDay(Object v) {
        if (v == null) return null;
        if (v instanceof Date) return (Date) v;
        String s = String.valueOf(v).trim();
        if (s.isEmpty()) return null;
        for (String pattern : Arrays.asList(
                "yyyy-MM-dd'T'HH:mm:ss", "yyyy-MM-dd HH:mm:ss.S", "yyyy-MM-dd HH:mm:ss",
                "yyyy-MM-dd", "MMM d yyyy hh:mma", "dd/MM/yyyy")) {
            try {
                SimpleDateFormat f = new SimpleDateFormat(pattern, Locale.ENGLISH);
                f.setLenient(false);
                return f.parse(s);
            } catch (java.text.ParseException ignored) { /* try the next shape */ }
        }
        return null;
    }

    /** Conversion.ToDateTime(x).Date — midnight, so a comparison is day against day. */
    private static Date dayOnly(Date d) {
        if (d == null) return null;
        Calendar c = Calendar.getInstance();
        c.setTime(d);
        c.set(Calendar.HOUR_OF_DAY, 0);
        c.set(Calendar.MINUTE, 0);
        c.set(Calendar.SECOND, 0);
        c.set(Calendar.MILLISECOND, 0);
        return c.getTime();
    }

    private static String isoDaysAgo(int days) {
        Calendar c = Calendar.getInstance();
        c.add(Calendar.DAY_OF_MONTH, -days);
        return new SimpleDateFormat("yyyy-MM-dd", Locale.ENGLISH).format(c.getTime());
    }
}
