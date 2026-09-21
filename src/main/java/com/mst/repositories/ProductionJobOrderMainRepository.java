package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.models.dto.ProductionJobOrderMainDto;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.SqlParameterValue;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Propagation;
import org.springframework.transaction.annotation.Transactional;

import java.sql.Types;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Collections;
import java.util.Date;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.Set;

/**
 * Screen 281 "Production Job Order" — the procedures
 * {@code Architecture.WinApp.Production.frmProductionJobOrderMain} actually calls, with the
 * desktop's own parameters.
 *
 * ---------------------------------------------------------------------------------------------
 * WHY THIS SITS BESIDE ProductionJobOrderRepository
 * ---------------------------------------------------------------------------------------------
 * The other repository was written against {@code frmProductionJobOrder.cs}. dbo.ScreenDefinition
 * row 281 names {@code frmProductionJobOrderMain} in TargetUrl, and TargetUrl is what the desktop
 * menu reflects on. Both forms share DAL 0273, so the SAVE path below is the same class — but the
 * two forms fill different child lists, read history through a different procedure, and bind a
 * different set of dropdowns. Merging them would make one screen silently behave like the other.
 *
 * ---------------------------------------------------------------------------------------------
 * SAVE IS ONE TRANSACTION — DAL 0273 SetData, loops in the DAL's own order
 * ---------------------------------------------------------------------------------------------
 *     num = SetProc(trn, obj, "Sp_InvProductionJobOrder_Insert" | "_Update");
 *     if (num > 0) obj.Id = num; else num = obj.Id;          // UPDATE returns 0, Id is kept
 *     foreach plant       in InvProductionJobOrderPlantslist   -> Sp_InvProductionJobOrderPlant_Insert
 *     foreach rate        in JobOrderRateSchedulelist          -> USP_JobOrderRateSchedule_Insert
 *     foreach allocation  in ...AndOrderAllocationList         -> USP_InvProductionJobOrderAndOrderAllocation_Insert_Update
 *     if (AutoJobLotCreateOnJobOrder && ActionId == 1)         -> Proc_JobLot_Insert
 *
 * The DAL also loops over Input, Output, Packing Material, OverHeads and Lab Standard. Screen 281
 * has no grid for any of them and initialises all five EMPTY (Insert():1764), so those loops run
 * zero times on the desktop. They therefore do not appear here: a job order saved from this screen
 * must not be able to carry rows the screen cannot produce.
 *
 * The parent key differs per child and is set by the DAL, never by the client:
 *     Plant       -> InvProductionJobOrderId
 *     RateSchedule-> JobOrderId                  (DAL 0273:85 — the odd one out)
 *     Allocation  -> InvProductionJobOrderId
 *
 * ---------------------------------------------------------------------------------------------
 * PARAMETER LISTS COME FROM THE MODEL, BY REFLECTION
 * ---------------------------------------------------------------------------------------------
 * {@code GenericProvider.SetProc} sends one parameter per NON-VIRTUAL property, in declaration
 * order. The DTO mirrors those models field for field and in the same order, and the writer below
 * reflects over it for the same reason the desktop does: so the two cannot drift apart silently.
 */
@Repository
public class ProductionJobOrderMainRepository {

    private static final Logger LOG = LoggerFactory.getLogger(ProductionJobOrderMainRepository.class);

    private static final String P_GET        = "Sp_InvProductionJobOrder_GetAllMethod";
    private static final String P_INS        = "Sp_InvProductionJobOrder_Insert";
    private static final String P_UPD        = "Sp_InvProductionJobOrder_Update";
    private static final String P_PLANT_INS  = "Sp_InvProductionJobOrderPlant_Insert";
    private static final String P_RATE_INS   = "USP_JobOrderRateSchedule_Insert";
    private static final String P_ALLOC_INS  = "USP_InvProductionJobOrderAndOrderAllocation_Insert_Update";
    /* Unqualified: every EXEC below prepends "dbo.", so a name that already carried [dbo].
       produced "EXEC dbo.[dbo].[...]" — which SQL Server reads as database "dbo" and refuses
       with "Database 'dbo' does not exist". */
    private static final String P_ALLOC_GET  = "Sp_InvProductionJobOrderAndOrderAllocation_GetAllMethod";
    private static final String P_HISTORY    = "USP_ProductionJobOrder_FormHistory";
    private static final String P_CONFIG     = "Sp_ConfigrationsAllocation_GetAllMethod";

    private final JdbcTemplate jdbc;
    public ProductionJobOrderMainRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    // =============================================================================== save

    @Transactional(propagation = Propagation.REQUIRED)
    public int save(ProductionJobOrderMainDto dto) {
        boolean insert = dto.Id == null || dto.Id <= 0;
        dto.ActionId = insert ? 1 : 2;                       // BLL 0335 Save

        Integer returned = execReturningId(insert ? P_INS : P_UPD, dto, Collections.<String>emptySet());
        int id = (returned != null && returned > 0) ? returned : (dto.Id == null ? 0 : dto.Id);
        if (id <= 0) throw new IllegalStateException("Job Order save returned no Id");
        dto.Id = id;

        for (ProductionJobOrderMainDto.Plant row : dto.InvProductionJobOrderPlantslist) {
            row.InvProductionJobOrderId = id;
            execReturningId(P_PLANT_INS, row, PLANT_SKIP);
        }
        for (ProductionJobOrderMainDto.RateSchedule row : dto.JobOrderRateSchedulelist) {
            row.JobOrderId = id;                             // NOT InvProductionJobOrderId
            execReturningId(P_RATE_INS, row, RATE_SKIP);
        }
        for (ProductionJobOrderMainDto.OrderAllocation row : dto.InvProductionJobOrderAndOrderAllocationList) {
            row.InvProductionJobOrderId = id;
            execReturningId(P_ALLOC_INS, row, ALLOC_SKIP);
        }

        /* DAL 0273:138 — on INSERT only, and only when the company's configuration says so, the
           job order also creates a Job Lot. Omitting it would leave the web app one row short of
           the desktop on every new job order in a company that has the switch on. */
        if (insert && autoJobLotEnabled(dto.OrganizationId, dto.CompanyId)) {
            insertAutoJobLot(dto, id);
        }
        return id;
    }

    /* The virtual, display-only properties SetProc skips. Sending them would add parameters the
       procedures do not declare. */
    private static final Set<String> PLANT_SKIP = Collections.singleton("PlantName");
    private static final Set<String> RATE_SKIP  = Collections.singleton("ItemName");
    private static final Set<String> ALLOC_SKIP =
            new java.util.HashSet<>(java.util.Arrays.asList("ItemName", "ContractNo", "ScheduleNo"));

    /**
     * The desktop reads {@code GlobalVariables_Helper.GetConfigValueFromGlobal(name)}, which looks
     * the name up in {@code clsGlobalVariables.configrationsAllocation} — a list loaded at login by
     * {@code ConfigrationsAllocation.History(OrganizationId, CompanyId)}. This reads one row of
     * that same per-org/company list rather than caching it, which is the pattern already used
     * elsewhere in this port.
     */
    public boolean config(Integer organizationId, Integer companyId, String name) {
        List<Map<String, Object>> rows = jdbc.queryForList(
                "EXEC dbo." + P_CONFIG + " @OrganizationId=?, @CompanyId=?, @ConfigDescription=?, @Activity=?",
                organizationId, companyId, name, "GetConfigurationByOrgCompandConfigDescription");
        if (rows.isEmpty()) return false;
        Object v = rows.get(0).get("ConfigKey");
        if (v == null) return false;
        String s = String.valueOf(v).trim();
        return "1".equals(s) || "true".equalsIgnoreCase(s) || "yes".equalsIgnoreCase(s);
    }

    private boolean autoJobLotEnabled(Integer organizationId, Integer companyId) {
        try {
            return config(organizationId, companyId, "AutoJobLotCreateOnJobOrder");
        } catch (Exception e) {
            /* Rethrown, not defaulted: a failed read must not silently decide the company does not
               want the Job Lot. The whole save rolls back rather than committing a job order that
               is missing its lot. */
            LOG.warn("AutoJobLotCreateOnJobOrder configuration read failed", e);
            throw e;
        }
    }

    /** Proc_JobLot_Insert with the DAL's own field values (0273:138-157). */
    private void insertAutoJobLot(ProductionJobOrderMainDto dto, int jobOrderId) {
        String now = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss").format(new Date());
        LinkedHashMap<String, Object> p = new LinkedHashMap<>();
        p.put("IsApproved",        Boolean.FALSE);
        p.put("PostState",         Boolean.FALSE);
        p.put("IsCompany",         Boolean.FALSE);
        p.put("IsThirdParty",      Boolean.FALSE);
        p.put("EndDate",           dto.EndDate);
        p.put("EntryDate",         now);
        p.put("ModifyDate",        now);
        p.put("PostDate",          null);
        p.put("StartDate",         dto.StartDate);
        p.put("CompanyId",         dto.CompanyId);
        p.put("EntryUser",         dto.EntryUser);
        p.put("Id",                0);
        p.put("ModifyUser",        dto.EntryUser);
        p.put("OrganizationId",    dto.OrganizationId);
        p.put("PostUser",          0);
        p.put("JobLotCode",        dto.RefInvoiceNo);
        p.put("JobLotDescription", dto.RefInvoiceNo);
        p.put("JobStatus",         "InComplete");
        p.put("JobTypeId",         22);
        p.put("AccountId",         0);
        p.put("RefDocumentTypeId", dto.DocumentTypeId);
        p.put("RefDocNoId",        jobOrderId);

        StringBuilder sql = new StringBuilder("EXEC dbo.Proc_JobLot_Insert ");
        List<Object> v = new ArrayList<>();
        boolean first = true;
        for (Map.Entry<String, Object> e : p.entrySet()) {
            if (!first) sql.append(", ");
            first = false;
            sql.append('@').append(e.getKey()).append("=?");
            v.add(e.getValue() == null ? new SqlParameterValue(Types.VARCHAR, null) : e.getValue());
        }
        jdbc.queryForList(sql.toString(), v.toArray());
    }

    /**
     * A null with no type attached is sent by the SQL Server driver as an INTEGER null, and a
     * procedure that declares the parameter as a date then fails with
     * "Operand type clash: int is incompatible with date" (error 206) before it runs at all.
     * Every null therefore carries the type of the field it came from.
     */
    private static SqlParameterValue typedNull(Class<?> type) {
        if (type == Integer.class || type == int.class
         || type == Long.class    || type == long.class
         || type == Short.class   || type == short.class)   return new SqlParameterValue(Types.INTEGER, null);
        if (type == Double.class  || type == double.class
         || type == Float.class   || type == float.class)   return new SqlParameterValue(Types.DOUBLE, null);
        if (type == java.math.BigDecimal.class)             return new SqlParameterValue(Types.NUMERIC, null);
        if (type == Boolean.class || type == boolean.class) return new SqlParameterValue(Types.BIT, null);
        /* Dates travel as strings on these DTOs ("2026-09-20"), and SQL Server converts a NULL
           varchar to a NULL date without complaint — unlike a NULL int. */
        return new SqlParameterValue(Types.VARCHAR, null);
    }

    /**
     * {@code EXEC <proc> @Field=?, ...} over every public field of the object, in declaration
     * order, mirroring SetProc's reflection. ExecuteScalar's value is the new Id, so the statement
     * is run as a query and the first column of the first row is read back.
     */
    private Integer execReturningId(String proc, Object obj, Set<String> skip) {
        List<String> names = new ArrayList<>();
        List<Object> values = new ArrayList<>();
        for (java.lang.reflect.Field f : obj.getClass().getFields()) {
            if (java.lang.reflect.Modifier.isStatic(f.getModifiers())) continue;
            if (skip.contains(f.getName())) continue;
            if (List.class.isAssignableFrom(f.getType())) continue;
            Object v;
            try { v = f.get(obj); } catch (IllegalAccessException e) { continue; }
            names.add(f.getName());
            values.add(v == null ? typedNull(f.getType()) : v);
        }
        StringBuilder sql = new StringBuilder("EXEC dbo.").append(proc).append(' ');
        for (int i = 0; i < names.size(); i++) {
            if (i > 0) sql.append(", ");
            sql.append('@').append(names.get(i)).append("=?");
        }
        List<Map<String, Object>> rows = jdbc.queryForList(sql.toString(), values.toArray());
        if (rows.isEmpty()) return 0;
        Object first = rows.get(0).values().iterator().next();
        return (first instanceof Number) ? ((Number) first).intValue() : 0;
    }

    // =============================================================================== reads

    /** BLL 0335 GetByID — @Id @Activity='GetById'. */
    public Map<String, Object> header(int id) {
        List<Map<String, Object>> rows = jdbc.queryForList(
                "EXEC dbo." + P_GET + " @Id=?, @Activity=?", id, "GetById");
        return rows.isEmpty() ? null : rows.get(0);
    }

    /** DAL 0273:216 — @Id @Activity='GetPlantDetailByHeaderId'. */
    public List<Map<String, Object>> plantRows(int headerId) {
        return jdbc.queryForList("EXEC dbo." + P_GET + " @Id=?, @Activity=?",
                headerId, "GetPlantDetailByHeaderId");
    }

    /** DAL 0273:259 — @Id @Activity='GetJobScheduleByHeaderId'. Both rate grids, one list. */
    public List<Map<String, Object>> rateScheduleRows(int headerId) {
        return jdbc.queryForList("EXEC dbo." + P_GET + " @Id=?, @Activity=?",
                headerId, "GetJobScheduleByHeaderId");
    }

    /** DAL 0273:274 — a different procedure, @Activity='ReadById'. */
    public List<Map<String, Object>> allocationRows(int headerId) {
        return jdbc.queryForList("EXEC dbo." + P_ALLOC_GET + " @Id=?, @Activity=?",
                headerId, "ReadById");
    }

    /**
     * BLL 0335 FormHistoryNew — {@code [dbo].[USP_ProductionJobOrder_FormHistory]}.
     *
     * This screen does NOT use the @Activity='FormHistory' path the other job-order form uses.
     * Two consequences worth stating, because both look like bugs and neither is mine to correct:
     *
     *  - BindHistoryGrid:2030 sets {@code joborder.DocumentTypeId = 403}, but FormHistoryNew reads
     *    {@code obj.DocumentTypeIds} (plural, a string) which nothing ever sets. So @DocumentTypeIds
     *    is NOT sent, and the procedure is left to decide. Sending 403 "because that is obviously
     *    what was meant" would return a different set of rows from the desktop.
     *  - Every optional parameter is omitted when unset, exactly as the BLL's {@code if} guards do,
     *    rather than sent as a null or a zero.
     *
     * canViewAllRecord and entryUser are derived from the signed-in user and this screen's grant
     * row before they get here. They are NOT request parameters: accepting either from the caller
     * would let anyone read every other user's job orders.
     */
    public List<Map<String, Object>> history(UserAccount u, int financialYearId,
                                             boolean canViewAllRecord, int entryUser,
                                             String dateMode, String fromDate, String toDate,
                                             int docNoFrom, int docNoTo, int invJobOrderId) {
        LinkedHashMap<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId",  u.getOrganizationId());
        p.put("CompanyId",       u.getCompanyId());
        p.put("FinancialYearId", financialYearId);
        p.put("CanViewAllRecord", canViewAllRecord);
        if (!canViewAllRecord) p.put("EntryUser", entryUser);

        /* drdocdate / rdentrydate / rdmodifydate / rdapproveddate — one radio, one pair of
           parameter names (BindHistoryGrid:2035-2077). */
        String mode = (dateMode == null || dateMode.trim().isEmpty()) ? "doc" : dateMode.trim().toLowerCase();
        String fromName, toName;
        if ("entry".equals(mode))         { fromName = "EntryFromDate";    toName = "EntryToDate"; }
        else if ("modify".equals(mode))   { fromName = "ModifyFromDate";   toName = "ModifyToDate"; }
        else if ("approved".equals(mode)) { fromName = "ApprovedFromDate"; toName = "ApprovedToDate"; }
        else                              { fromName = "FromDate";         toName = "ToDate"; }
        if (fromDate != null && !fromDate.trim().isEmpty()) p.put(fromName, fromDate.trim());
        if (toDate   != null && !toDate.trim().isEmpty())   p.put(toName,   toDate.trim());

        if (docNoFrom   != 0) p.put("DocNoFrom",    docNoFrom);
        if (docNoTo     != 0) p.put("DocNoTo",      docNoTo);
        if (invJobOrderId != 0) p.put("InvJobOrderId", invJobOrderId);

        StringBuilder sql = new StringBuilder("EXEC dbo." + P_HISTORY + " ");
        List<Object> v = new ArrayList<>();
        boolean first = true;
        for (Map.Entry<String, Object> e : p.entrySet()) {
            if (!first) sql.append(", ");
            first = false;
            sql.append('@').append(e.getKey()).append("=?");
            v.add(e.getValue());
        }
        try {
            return jdbc.queryForList(sql.toString(), v.toArray());
        } catch (Exception e) {
            /* Reported, never swallowed: an empty history grid must not be able to mean
               "the procedure failed". */
            LOG.warn("{} failed", P_HISTORY, e);
            throw e;
        }
    }

    /**
     * GeneratePlanCode — BLL 0335 GetGenerateCode, PlanCode column of row 0.
     * @FinancialYearId is sent only when non-zero, as the BLL's own guard does.
     */
    public int nextCode(UserAccount u, int documentTypeId, int financialYearId) {
        List<Map<String, Object>> rows = (financialYearId != 0)
            ? jdbc.queryForList(
                "EXEC dbo." + P_GET + " @OrganizationId=?, @CompanyId=?, @DocumentTypeId=?, "
              + "@FinancialYearId=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), documentTypeId, financialYearId, "GenerateCode")
            : jdbc.queryForList(
                "EXEC dbo." + P_GET + " @OrganizationId=?, @CompanyId=?, @DocumentTypeId=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), documentTypeId, "GenerateCode");
        return firstInt(rows, "PlanCode");
    }

    /** cmbPlanType_Leave — BLL 0335 GetGenerateCodePlanType, PlanCode column of row 0. */
    public int nextPlanTypeCode(UserAccount u, String planType, int financialYearId) {
        List<Map<String, Object>> rows = (financialYearId != 0)
            ? jdbc.queryForList(
                "EXEC dbo." + P_GET + " @OrganizationId=?, @CompanyId=?, @PlanType=?, "
              + "@FinancialYearId=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), planType, financialYearId, "GeneratePlanTypeCode")
            : jdbc.queryForList(
                "EXEC dbo." + P_GET + " @OrganizationId=?, @CompanyId=?, @PlanType=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), planType, "GeneratePlanTypeCode");
        return firstInt(rows, "PlanCode");
    }

    /** GetLastSaveAccount:568 — restores WIP account, item and warehouse on a fresh form. */
    public Map<String, Object> lastWipAccount(UserAccount u, int documentTypeId) {
        List<Map<String, Object>> rows = jdbc.queryForList(
                "EXEC dbo." + P_GET + " @OrganizationId=?, @CompanyId=?, @DocumentTypeId=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), documentTypeId, "GetLastWipAccount");
        return rows.isEmpty() ? null : rows.get(0);
    }

    // ================================================================ per-row delete guards

    /**
     * GridDetail_ColumnButtonClick:1016 — in UPDATE mode the desktop asks whether the plant has
     * already been used in a Production document before letting the row go, and refuses with
     * "You cannot be delete X because X Has been refered to production".
     *
     * BLL 0335 CheckPlantExistinProductionagaintJobOrder — returns column 0 of row 0 as a bool.
     */
    public boolean plantUsedInProduction(UserAccount u, int jobOrderId, int plantId) {
        List<Map<String, Object>> rows = jdbc.queryForList(
                "EXEC dbo." + P_GET + " @OrganizationId=?, @CompanyId=?, @Id=?, @PlantId=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), jobOrderId, plantId,
                "CheckPlantExistinProductionagaintJobOrder");
        if (rows.isEmpty()) return false;
        Map<String, Object> r = rows.get(0);
        if (r.isEmpty()) return false;
        Object v = r.values().iterator().next();
        if (v instanceof Boolean) return (Boolean) v;
        if (v instanceof Number)  return ((Number) v).intValue() != 0;
        if (v == null) return false;
        String s = String.valueOf(v).trim();
        return "1".equals(s) || "true".equalsIgnoreCase(s);
    }

    /**
     * grdByProduct_ColumnButtonClick:1210 / grdFinishGoods_ColumnButtonClick:1412 — removing a
     * SAVED rate-schedule row in update mode deletes it immediately, by its own Id.
     * BLL 0335 JoBOrderDetailScheduleDeleteById -> USP_JoBOrderScheduleDeleteById @Id.
     *
     * The caller passes a detail Id; the service checks it belongs to the job order being edited
     * before this runs, because the procedure itself takes no parent and no tenancy.
     */
    public void deleteRateScheduleRow(int detailId) {
        jdbc.queryForList("EXEC dbo.USP_JoBOrderScheduleDeleteById @Id=?", detailId);
    }

    // ============================================================== the twelve dropdowns

    /* A lookup that fails is reported and returns empty — it never pretends the list is empty
       without saying so in the log. The screen still opens; the log names the procedure. */
    private List<Map<String, Object>> q(String label, String sql, Object... args) {
        try {
            return jdbc.queryForList(sql, args);
        } catch (Exception e) {
            LOG.warn("Screen 281 lookup '{}' failed: {}", label, sql, e);
            return new ArrayList<>();
        }
    }

    /** cmbPlantFeader — PlantsAllocationToBranch.GetPlantsAllocatedToBranch (BLL 0020). */
    public List<Map<String, Object>> plantsAllocatedToBranch(UserAccount u) {
        return q("plantsAllocatedToBranch",
                "EXEC [dbo].[USP_GetPlantsAllocatedToBranch] @OrganizationId=?, @CompanyId=?",
                u.getOrganizationId(), u.getCompanyId());
    }

    /**
     * CmbWipItem — {@code CommonServices.ItemGetForComboServiceBind(6)}.
     *
     * The 6 is a parent inventory category and reaches the procedure as
     * {@code @InventoryParentCategoriesId} (Item.GetAllbyCombobind). The other job-order form
     * calls the same helper with NO argument, so it sends no category at all and gets a longer
     * list. Passing 6 here is the difference between the two screens, not a copy-paste slip.
     */
    public List<Map<String, Object>> wipItems(UserAccount u) {
        return q("wipItems",
                "EXEC dbo.Sp_Item_GetAllMethod @OrganizationId=?, @CompanyId=?, "
              + "@InventoryParentCategoriesId=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), 6, "ReadAllForComboTwoColumns");
    }

    /**
     * CmbWorkInProcess / CmbFinishGoodsAc / CmbByProductionAc — all three bind the SAME table:
     * {@code CoaAllocationAccountTitleByAccountTypeIds("4")} ->
     * COAAllocation.GetAccountTitleByAccountTypeIds, @Activity='GetAccountTitleByAccountTypeIds'.
     *
     * Note this is NOT the @Activity='COAForCombobindig' list the other job-order form uses.
     * @AppId and @UserId are part of the contract; CostCenterId, AccountClassIds, NotReferred and
     * RecId are left unset by this screen and so are not sent.
     */
    public List<Map<String, Object>> accountsByType(UserAccount u, int appId, String accountTypeIds) {
        return q("accountsByType",
                "EXEC dbo.Sp_COAAllocation_GetAllMethod @OrganizationId=?, @CompanyId=?, @AppId=?, "
              + "@AccountTypeIds=?, @UserId=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), appId, accountTypeIds, u.getId(),
                "GetAccountTitleByAccountTypeIds");
    }

    /** cmbPlanType — ProductionPlanType.GetAll, no parameters when Id is 0. */
    public List<Map<String, Object>> planTypes() {
        return q("planTypes", "EXEC dbo.Sp_ProductionPlanType_GetAllMethod");
    }

    /** cmbProductionType — ProductionType.GetAll, no parameters when Id is 0. */
    public List<Map<String, Object>> productionTypes() {
        return q("productionTypes", "EXEC dbo.Sp_ProductionType_GetAllMethod");
    }

    /**
     * CmbProductionNo — the pre-generated job-order numbers, bound only when the
     * GenerateJobOrderNo configuration is on (BindJobOrderNo:466).
     * @InvoiceNo is sent only when a specific number is being re-selected, as the BLL's guard does.
     */
    public List<Map<String, Object>> generatedJobOrderNos(UserAccount u, int documentTypeId,
                                                          String invoiceNo) {
        if (invoiceNo != null && !invoiceNo.trim().isEmpty()) {
            return q("generatedJobOrderNos",
                    "EXEC [dbo].[USP_ExportInvoiceNos_GetAllMethod] @OrganizationId=?, @CompanyId=?, "
                  + "@DocumentTypeId=?, @InvoiceNo=?, @Activity=?",
                    u.getOrganizationId(), u.getCompanyId(), documentTypeId, invoiceNo.trim(),
                    "GetFinalInvoiceNosForJobOrder");
        }
        return q("generatedJobOrderNos",
                "EXEC [dbo].[USP_ExportInvoiceNos_GetAllMethod] @OrganizationId=?, @CompanyId=?, "
              + "@DocumentTypeId=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), documentTypeId,
                "GetFinalInvoiceNosForJobOrder");
    }

    /**
     * CmbWipWarehouse — the desktop binds {@code clsGlobalVariables.globalWarehousesWithBranches},
     * a login-time cache of {@code USP_GetWarehousesAllocatedToBranch} for the user's own branch.
     * Read here rather than cached, from the same procedure with the same three parameters.
     */
    public List<Map<String, Object>> warehousesAllocatedToBranch(UserAccount u, int branchId) {
        return q("warehousesAllocatedToBranch",
                "EXEC dbo.USP_GetWarehousesAllocatedToBranch @OrganizationId=?, @CompanyId=?, @BranchId=?",
                u.getOrganizationId(), u.getCompanyId(), branchId);
    }

    /**
     * xmbjoborderno (the History tab's filter) — JobOrderNofill:508. The procedure returns several
     * kinds of row and the desktop keeps only those whose Activity column reads "JobOrderNo",
     * displaying the ReferenceName column. The filtering stays in the service, in memory, because
     * that is where the desktop does it.
     */
    public List<Map<String, Object>> jobOrderNoDropDown(UserAccount u) {
        return q("jobOrderNoDropDown",
                "EXEC [dbo].[USP_GetDataForDropDownFromJobOrder] @OrganizationId=?, @CompanyId=?",
                u.getOrganizationId(), u.getCompanyId());
    }

    /** CmbItemByProduct / CmbItemFinish — Item.ReadAllItems, the unfiltered list. */
    public List<Map<String, Object>> allItems(UserAccount u) {
        return q("allItems",
                "EXEC dbo.Sp_Item_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), "ReadAllItems");
    }

    // ==================================================== Export Schedule loader (Screen 281)

    /**
     * LoadExportScheduleForProduction — {@code usp_getExportScheduleForProduction}. Every filter
     * is optional and omitted when unset, exactly as BLL 0335 GetExportScheduleForProduction does;
     * the dialog opens with FromDate = today - 7 and nothing else set.
     */
    public List<Map<String, Object>> exportSchedule(UserAccount u, String fromDate, String toDate,
                                                    int itemId, int contractId,
                                                    int supplierCustomerId, int thirdPartyId) {
        LinkedHashMap<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", u.getOrganizationId());
        p.put("CompanyId",      u.getCompanyId());
        if (fromDate != null && !fromDate.trim().isEmpty()) p.put("FromDate", fromDate.trim());
        if (toDate   != null && !toDate.trim().isEmpty())   p.put("ToDate",   toDate.trim());
        if (itemId             != 0) p.put("ItemId",             itemId);
        if (contractId         != 0) p.put("ContractId",         contractId);
        if (supplierCustomerId != 0) p.put("SupplierCustomerId", supplierCustomerId);
        if (thirdPartyId       != 0) p.put("ThirdPartyId",       thirdPartyId);

        StringBuilder sql = new StringBuilder("EXEC dbo.usp_getExportScheduleForProduction ");
        List<Object> v = new ArrayList<>();
        boolean first = true;
        for (Map.Entry<String, Object> e : p.entrySet()) {
            if (!first) sql.append(", ");
            first = false;
            sql.append('@').append(e.getKey()).append("=?");
            v.add(e.getValue());
        }
        return q("exportSchedule", sql.toString(), v.toArray());
    }

    /**
     * The loader's Contract / Customer / Item pickers — ScheduleComboDbCall():251 ->
     * ExImLcOrderShipmentScheduleHeader.GetDataForDropDownFromShipmentSchedule, which is
     * {@code USP_GetDataForDropDownFromImportLcOrderSchedule}. ONE call returns all three lists
     * in one result set, told apart by the Activity column ("ContractNo" | "Customer" | "Item")
     * and displayed from ReferenceName. The dialog passes no @Activity, so none is sent.
     *
     * Note this is NOT {@code usp_getContractFromScheduleForDropDown} /
     * {@code usp_getItemsFromScheduleForDropDown}. Those two exist in BLL 0335, and their names
     * make them look like the obvious source, but this dialog never calls them — using them would
     * put a different set of contracts and items in front of the operator.
     */
    public List<Map<String, Object>> shipmentScheduleDropDown(UserAccount u) {
        return q("shipmentScheduleDropDown",
                "EXEC dbo.USP_GetDataForDropDownFromImportLcOrderSchedule @OrganizationId=?, @CompanyId=?",
                u.getOrganizationId(), u.getCompanyId());
    }

    /**
     * The loader's Third Party Inspection picker — ThirdPartyDbCall("TrackingNo"):236.
     * The rows are filtered again in memory on the ActivityType column, as ThirdPartyFill does,
     * and displayed from the lower-case "name" column.
     */
    public List<Map<String, Object>> thirdPartyDropDown(UserAccount u, String activity) {
        return q("thirdPartyDropDown",
                "EXEC [dbo].[USP_GetDataForDropDownFrom_LabPreExportLotInspection] "
              + "@OrganizationId=?, @CompanyId=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), activity);
    }

    // ----------------------------------------------------------------------------- helpers

    private static int firstInt(List<Map<String, Object>> rows, String column) {
        if (rows == null || rows.isEmpty()) return 0;
        Map<String, Object> r = rows.get(0);
        Object v = (column != null && r.containsKey(column)) ? r.get(column)
                 : (r.isEmpty() ? null : r.values().iterator().next());
        if (v instanceof Number) return ((Number) v).intValue();
        if (v == null) return 0;
        try { return Integer.parseInt(String.valueOf(v).trim()); }
        catch (NumberFormatException e) { return 0; }
    }
}
