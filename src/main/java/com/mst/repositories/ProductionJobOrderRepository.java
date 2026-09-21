package com.mst.repositories;


import com.mst.models.UserAccount;
import com.mst.models.dto.ProductionJobOrderDto;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.SqlParameterValue;

import java.sql.Types;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Propagation;
import org.springframework.transaction.annotation.Transactional;

import java.text.SimpleDateFormat;
import java.util.*;

/**
 * Production Job Order - the real procedures the desktop calls, with the desktop's own parameters.
 *
 * ---------------------------------------------------------------------------------------------
 * SAVE IS ONE TRANSACTION, HEADER THEN CHILDREN THEN THE AUTO JOB LOT
 * ---------------------------------------------------------------------------------------------
 * DAL 0273 SetData opens a SqlTransaction and, inside it:
 *
 *     num = GenericProvider.SetProc(trn, obj, ProcName);      // Insert or Update, returns the Id
 *     if (num > 0) obj.Id = num; else num = obj.Id;
 *     foreach (input  in obj.invProductionJobOrderInput)  { input.InvProductionJobOrderId  = obj.Id;
 *                 SetProc(trn, input,  "Sp_InvProductionJobOrderInput_Insert");  }
 *     foreach (output in obj.invProductionJobOrderOutput) { output.InvProductionJobOrderId = obj.Id;
 *                 SetProc(trn, output, "Sp_InvProductionJobOrderOutput_Insert"); }
 *     if (AutoJobLotCreateOnJobOrder && obj.ActionId == 1)  SetProc(trn, jobLot, "Proc_JobLot_Insert");
 *
 * Reproduced exactly, under a single Spring transaction. Note `if (num > 0) ... else num = obj.Id`
 * - on UPDATE the procedure returns 0 and the existing Id is kept.
 *
 * The parameter lists are the model's non-virtual properties (see the DTO for why). They are built
 * by reflection over the DTO here for the same reason the desktop does it: so the two can never
 * drift apart field by field.
 *
 * The other child collections SetData loops over - Plant, OverHeads, Packing Material, Rate
 * Schedule, Order Allocation, Lab Standard - are NOT written here, because frmProductionJobOrder
 * never fills them. The desktop's loops run zero times on this screen; so do the absent loops here.
 */
@Repository
public class ProductionJobOrderRepository {

    private static final Logger LOG = LoggerFactory.getLogger(ProductionJobOrderRepository.class);

    private static final String P_GET   = "Sp_InvProductionJobOrder_GetAllMethod";
    private static final String P_INS   = "Sp_InvProductionJobOrder_Insert";
    private static final String P_UPD   = "Sp_InvProductionJobOrder_Update";
    private static final String P_IN    = "Sp_InvProductionJobOrderInput_Insert";
    private static final String P_OUT   = "Sp_InvProductionJobOrderOutput_Insert";
    private static final String P_IN_R  = "Sp_InvProductionJobOrderInput_GetAllMethod";
    private static final String P_OUT_R = "Sp_InvProductionJobOrderOutput_GetAllMethod";

    /** The referential guard lives in the PartyProcessing procedure, not this screen's own. */
    private static final String P_REF   = "Sp_InvProductionJobOrderPartyProcessing_GetAllMethod";

    private final JdbcTemplate jdbc;
    public ProductionJobOrderRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    // =============================================================================== save

    @Transactional(propagation = Propagation.REQUIRED)
    public int save(ProductionJobOrderDto dto) {
        boolean insert = dto.Id == null || dto.Id <= 0;
        dto.ActionId = insert ? 1 : 2;                      // BLL 0335 Save

        Integer returned = execReturningId(insert ? P_INS : P_UPD, dto, HEADER_SKIP);
        int id = (returned != null && returned > 0) ? returned : (dto.Id == null ? 0 : dto.Id);
        if (id <= 0) throw new IllegalStateException("Job Order save returned no Id");
        dto.Id = id;

        for (ProductionJobOrderDto.Input row : dto.invProductionJobOrderInput) {
            row.InvProductionJobOrderId = id;
            execReturningId(P_IN, row, Collections.emptySet());
        }
        for (ProductionJobOrderDto.Output row : dto.invProductionJobOrderOutput) {
            row.InvProductionJobOrderId = id;
            execReturningId(P_OUT, row, Collections.emptySet());
        }

        /* DAL 0273:138 - on INSERT only, and only when the company's own configuration says so,
           the job order also creates a Job Lot. Omitting it would leave the web app one row short
           of the desktop on every new job order in a company that has the switch on, which is the
           kind of silent difference that only shows up weeks later in a Job Lot picker. */
        if (insert && autoJobLotEnabled(dto.OrganizationId, dto.CompanyId)) {
            insertAutoJobLot(dto, id);
        }
        return id;
    }

    /** The child lists are not parameters - they are the properties SetProc skips as virtual. */
    private static final Set<String> HEADER_SKIP =
            new HashSet<>(Arrays.asList("invProductionJobOrderInput", "invProductionJobOrderOutput"));

    /**
     * Sp_ConfigrationsAllocation_GetAllMethod @Activity=GetConfigurationByOrgCompandConfigDescription,
     * reading the ConfigKey column - CommonServies.GetConfigurationFromAllocation.
     */
    private boolean autoJobLotEnabled(Integer organizationId, Integer companyId) {
        try {
            List<Map<String, Object>> rows = jdbc.queryForList(
                    "EXEC dbo.Sp_ConfigrationsAllocation_GetAllMethod @OrganizationId=?, @CompanyId=?, "
                  + "@ConfigDescription=?, @Activity=?",
                    organizationId, companyId, "AutoJobLotCreateOnJobOrder",
                    "GetConfigurationByOrgCompandConfigDescription");
            if (rows.isEmpty()) return false;
            Object v = rows.get(0).get("ConfigKey");
            if (v == null) return false;
            String s = String.valueOf(v).trim();
            return "1".equals(s) || "true".equalsIgnoreCase(s) || "yes".equalsIgnoreCase(s);
        } catch (Exception e) {
            /* Reported, not assumed either way: a failed read must not silently decide that the
               company does not want the Job Lot. It is rethrown so the whole save rolls back
               rather than committing a job order that is missing its lot. */
            LOG.warn("AutoJobLotCreateOnJobOrder configuration read failed", e);
            throw e;
        }
    }

    /** Proc_JobLot_Insert with the DAL's own field values (0273:138-157). */
    private void insertAutoJobLot(ProductionJobOrderDto dto, int jobOrderId) {
        String now = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss").format(new Date());
        LinkedHashMap<String, Object> p = new LinkedHashMap<>();
        p.put("IsApproved",       Boolean.FALSE);
        p.put("PostState",        Boolean.FALSE);
        p.put("IsCompany",        Boolean.FALSE);
        p.put("IsThirdParty",     Boolean.FALSE);
        p.put("EndDate",          dto.EndDate);
        p.put("EntryDate",        now);
        p.put("ModifyDate",       now);
        p.put("PostDate",         null);
        p.put("StartDate",        dto.StartDate);
        p.put("CompanyId",        dto.CompanyId);
        p.put("EntryUser",        dto.EntryUser);
        p.put("Id",               0);
        p.put("ModifyUser",       dto.EntryUser);
        p.put("OrganizationId",   dto.OrganizationId);
        p.put("PostUser",         0);
        p.put("JobLotCode",       dto.RefInvoiceNo);
        p.put("JobLotDescription",dto.RefInvoiceNo);
        p.put("JobStatus",        "InComplete");
        p.put("JobTypeId",        22);
        p.put("AccountId",        0);
        p.put("RefDocumentTypeId",dto.DocumentTypeId);
        p.put("RefDocNoId",       jobOrderId);

        StringBuilder sql = new StringBuilder("EXEC dbo.Proc_JobLot_Insert ");
        List<Object> v = new ArrayList<>();
        boolean first = true;
        for (Map.Entry<String, Object> e : p.entrySet()) {
            if (!first) sql.append(", ");
            first = false;
            sql.append('@').append(e.getKey()).append("=?");
            v.add(e.getValue());
        }
        jdbc.queryForList(sql.toString(), v.toArray());
    }

    /**
     * `EXEC <proc> @Field=?, ...` over every public field of the object, in declaration order,
     * mirroring SetProc's reflection. ExecuteScalar's value is the new Id, so the statement is run
     * as a query and the first column of the first row is read back.
     */
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
           varchar to a NULL date without complaint - unlike a NULL int. */
        return new SqlParameterValue(Types.VARCHAR, null);
    }

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

    /** Sp_InvProductionJobOrder_GetAllMethod @Id @Activity='GetById' (BLL 0335 GetByID). */
    public Map<String, Object> header(int id) {
        List<Map<String, Object>> rows = jdbc.queryForList(
                "EXEC dbo." + P_GET + " @Id=?, @Activity=?", id, "GetById");
        return rows.isEmpty() ? null : rows.get(0);
    }

    /**
     * The two child grids, exactly as DAL 0273 GetDate loads them: one procedure each, taking the
     * header Id and nothing else - no @Activity and no entry-type parameter.
     */
    public List<Map<String, Object>> inputRows(int headerId) {
        return jdbc.queryForList("EXEC dbo." + P_IN_R + " @Id=?", headerId);
    }

    public List<Map<String, Object>> outputRows(int headerId) {
        return jdbc.queryForList("EXEC dbo." + P_OUT_R + " @Id=?", headerId);
    }

    /**
     * FormHistory, with the desktop's own 13 parameters (BLL 0335 FormHistory).
     *
     * Every parameter is sent on every call, including the ones BindHistoryGrid never sets, and
     * with the values it leaves them at: FinancialYearId 0, the dates null, DocNoFrom/DocNoTo and
     * InvJobOrderId 0. Sending the signed-in user's real financial year instead would be a
     * defensible improvement and would return a DIFFERENT set of rows from the desktop, which is
     * the one thing this screen must not do.
     *
     * canViewAllRecord and entryUser are derived from the signed-in user and the screen's grant
     * grid before they get here. They are NOT request parameters: accepting either from the caller
     * would let anyone read every other user's job orders.
     */
    public List<Map<String, Object>> history(UserAccount u, int documentTypeId,
                                             boolean canViewAllRecord, int entryUser,
                                             Integer noOfRecords) {
        String sql = "EXEC dbo." + P_GET + " @OrganizationId=?, @CompanyId=?, @DocumentTypeId=?, "
                   + "@FinancialYearId=?, @NoOfRecords=?, @CanViewAllRecord=?, @EntryUser=?, "
                   + "@FromDate=?, @ToDate=?, @DocNoFrom=?, @DocNoTo=?, @InvJobOrderId=?, @Activity=?";
        /* @FromDate and @ToDate are NULL here, as BindHistoryGrid leaves them - but a bare
           null in the argument array carries no type, and the SQL Server driver then sends it
           as an INTEGER null. The procedure declares them as dates, so the call failed with
           "Operand type clash: int is incompatible with date" (error 206) and the History tab
           could never load. SqlParameterValue attaches the type the procedure expects. */
        Object[] args = new Object[] {
                u.getOrganizationId(), u.getCompanyId(), documentTypeId,
                0,                                        // ReportsParameters default - see above
                noOfRecords == null ? 0 : noOfRecords,
                canViewAllRecord, entryUser,
                new SqlParameterValue(Types.DATE, null),
                new SqlParameterValue(Types.DATE, null),
                0, 0, 0,
                "FormHistory"
        };
        try {
            return jdbc.queryForList(sql, args);
        } catch (Exception e) {
            /* Reported, never swallowed: an empty history grid must not be able to mean
               "the procedure failed" - that ambiguity is what made the inventory pickers
               impossible to diagnose. */
            LOG.warn("{} @Activity=FormHistory failed", P_GET, e);
            throw e;
        }
    }

    /** GenerateCode - the next plan code for this document type and year (BLL 0335). */
    public int nextCode(UserAccount u, int documentTypeId, int financialYearId) {
        List<Map<String, Object>> rows = jdbc.queryForList(
                "EXEC dbo." + P_GET + " @OrganizationId=?, @CompanyId=?, @DocumentTypeId=?, "
              + "@FinancialYearId=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), documentTypeId, financialYearId, "GenerateCode");
        return firstInt(rows, null);
    }

    /**
     * GeneratePlanTypeCode - the per-plan-type serial beside Plan Type. The desktop reads the
     * PlanCode COLUMN of row 0, not the first column, so this reads it by name.
     */
    public int nextPlanTypeCode(UserAccount u, String planType, int financialYearId) {
        List<Map<String, Object>> rows = jdbc.queryForList(
                "EXEC dbo." + P_GET + " @OrganizationId=?, @CompanyId=?, @PlanType=?, "
              + "@FinancialYearId=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), planType, financialYearId, "GeneratePlanTypeCode");
        return firstInt(rows, "PlanCode");
    }

    // =============================================================== referential guards

    /**
     * CheckJobOrderIdRefToInvFoodProduction - the guard the desktop runs when a grid row is opened
     * for editing, NOT on save. It lives in the PartyProcessing procedure, and the document type
     * it is asked about is the PRODUCTION document that might already consume this job order:
     *
     *     grdPlannedOutput double-click  ->  112   "exist in Production OutPut"
     *     grdInputDetail   double-click  ->   80   "exist in Production InPut"
     *
     * Neither is 401. Passing this screen's own document type would ask a question the desktop
     * never asks and would answer it wrongly.
     */
    public int referencedByProduction(UserAccount u, int jobOrderId, int productionDocumentTypeId) {
        List<Map<String, Object>> rows = jdbc.queryForList(
                "EXEC dbo." + P_REF + " @OrganizationId=?, @CompanyId=?, @InvJobOrderId=?, "
              + "@DocumentTypeId=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), jobOrderId, productionDocumentTypeId,
                "CheckJobOrderIdRefToInvFoodProduction");
        return firstInt(rows, null);
    }

    private static int firstInt(List<Map<String, Object>> rows, String column) {
        if (rows == null || rows.isEmpty()) return 0;
        Map<String, Object> r = rows.get(0);
        Object v = (column != null && r.containsKey(column)) ? r.get(column)
                 : (r.isEmpty() ? null : r.values().iterator().next());
        if (v instanceof Number) return ((Number) v).intValue();
        if (v == null) return 0;
        try { return Integer.parseInt(String.valueOf(v).trim()); } catch (NumberFormatException e) { return 0; }
    }
}
