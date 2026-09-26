package com.mst.repositories;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.jdbc.core.ConnectionCallback;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.ResultSetMetaData;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Contractor Labour Wages bill - desktop Architecture.WinApp.Contractor_Wages/frmwagesBillHeader.cs
 * (DocumentTypeId 101). Screen 280 opens it as a dialog after an Input (80), Output (112) or
 * Consumption (181) save, and Stock Conversion (66) after its save; it also runs standalone.
 *
 * Every call below was traced three ways: the form line that fills the model / ReportsParameters,
 * the BLL IL that decides which values become parameters (and under which guard), and the
 * procedure declaration in procdure.utf8.sql. A parameter the BLL guards is OMITTED when it is
 * unset - never sent as NULL.
 *
 * WRITES reproduce Architecture.DAL.ContractorWages.InvContractorWagesBillHeader.SetData. Those
 * run inside the caller's transaction (ProductionWagesBillService wraps them in a
 * TransactionTemplate - the SqlTransaction SetData opens), because every call here goes through
 * JdbcTemplate/DataSourceUtils and so joins the bound connection.
 */
@Repository
public class ProductionWagesBillRepository {

    private static final Logger LOG = LoggerFactory.getLogger(ProductionWagesBillRepository.class);

    /** InvContractorWagesBillHeader.DocumentTypeId - Insert():2752 and GenerateDocNo():479. */
    public static final int DOC_TYPE_WAGES = 101;

    /** CommonServices.SetRightsValueInRightsObject(base.Name) - Load:344. */
    public static final String SCREEN_NAME = "frmwagesBillHeader";

    private final JdbcTemplate jdbc;

    public ProductionWagesBillRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    // =================================================================================== plumbing

    public static LinkedHashMap<String, Object> params() { return new LinkedHashMap<>(); }

    /** EXEC proc @A=?, @B=? ... and every row of the FIRST result set. Nulls are omitted. */
    public List<Map<String, Object>> query(String proc, Map<String, Object> p) {
        return jdbc.execute((ConnectionCallback<List<Map<String, Object>>>) con -> run(con, proc, p, false));
    }

    /** ExecuteScalar: the first column of the first row of the first result set, or null. */
    public Object scalar(String proc, Map<String, Object> p) {
        List<Map<String, Object>> rows = jdbc.execute(
                (ConnectionCallback<List<Map<String, Object>>>) con -> run(con, proc, p, true));
        if (rows == null || rows.isEmpty()) return null;
        Map<String, Object> r = rows.get(0);
        return r.isEmpty() ? null : r.values().iterator().next();
    }

    private static List<Map<String, Object>> run(Connection con, String proc, Map<String, Object> p,
                                                 boolean firstRowOnly) throws java.sql.SQLException {
        StringBuilder sql = new StringBuilder("EXEC ").append(proc);
        List<Object> values = new ArrayList<>();
        boolean first = true;
        for (Map.Entry<String, Object> e : p.entrySet()) {
            if (e.getValue() == null) continue;           // AddWithValue(name, null) -> the proc default
            sql.append(first ? " " : ", ").append(e.getKey()).append("=?");
            values.add(e.getValue());
            first = false;
        }
        List<Map<String, Object>> out = new ArrayList<>();
        try (PreparedStatement ps = con.prepareStatement(sql.toString())) {
            for (int i = 0; i < values.size(); i++) {
                Object v = values.get(i);
                if (v instanceof LocalDateTime) v = Timestamp.valueOf((LocalDateTime) v);
                if (v instanceof LocalDate) v = Timestamp.valueOf(((LocalDate) v).atStartOfDay());
                ps.setObject(i + 1, v);
            }
            boolean isRs = ps.execute();
            while (true) {
                if (isRs) {
                    try (ResultSet rs = ps.getResultSet()) {
                        ResultSetMetaData md = rs.getMetaData();
                        int n = md.getColumnCount();
                        while (rs.next()) {
                            Map<String, Object> row = new LinkedHashMap<>();
                            for (int c = 1; c <= n; c++) row.put(md.getColumnLabel(c), rs.getObject(c));
                            out.add(row);
                            if (firstRowOnly) break;
                        }
                    }
                    /* The desktop reads the first table only; the rest is drained so a RAISERROR
                       raised after the first SELECT still surfaces, as it does in ADO.NET. */
                    drain(ps);
                    return out;
                }
                if (ps.getUpdateCount() == -1) break;
                isRs = ps.getMoreResults();
            }
        }
        return out;
    }

    private static void drain(PreparedStatement ps) throws java.sql.SQLException {
        while (true) {
            boolean more = ps.getMoreResults();
            if (!more && ps.getUpdateCount() == -1) return;
        }
    }

    public static Object ci(Map<String, Object> row, String name) {
        if (row == null) return null;
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet()) {
            if (e.getKey().equalsIgnoreCase(name)) return e.getValue();
        }
        return null;
    }

    /** Conversion.ToInt - a number rounds half to even (Convert.ToInt32); unparsable is 0. */
    public static int toInt(Object v) {
        if (v == null) return 0;
        if (v instanceof Boolean) return ((Boolean) v) ? 1 : 0;
        if (v instanceof Integer || v instanceof Long || v instanceof Short) return ((Number) v).intValue();
        double d;
        if (v instanceof Number) d = ((Number) v).doubleValue();
        else {
            String s = String.valueOf(v).trim().replace(",", "");
            if (s.isEmpty()) return 0;
            try { d = Double.parseDouble(s); } catch (NumberFormatException e) { return 0; }
        }
        if (Double.isNaN(d) || Double.isInfinite(d)) return 0;
        return new BigDecimal(d).setScale(0, RoundingMode.HALF_EVEN).intValue();
    }

    /** Conversion.ToDouble - unparsable is 0. */
    public static double toDouble(Object v) {
        if (v == null) return 0d;
        if (v instanceof Number) return ((Number) v).doubleValue();
        if (v instanceof Boolean) return ((Boolean) v) ? 1d : 0d;
        String s = String.valueOf(v).trim().replace(",", "");
        if (s.isEmpty()) return 0d;
        try { return Double.parseDouble(s); } catch (NumberFormatException e) { return 0d; }
    }

    /** Conversion.ToBool - True/1. */
    public static boolean toBool(Object v) {
        if (v instanceof Boolean) return (Boolean) v;
        if (v instanceof Number) return ((Number) v).intValue() != 0;
        String s = v == null ? "" : String.valueOf(v).trim();
        return "1".equals(s) || "true".equalsIgnoreCase(s);
    }

    public static String str(Object v) { return v == null ? "" : String.valueOf(v); }

    // ============================================================ configuration, features, rights

    /**
     * GlobalVariables_Helper.GetConfigValueFromGlobal(name) / DAL CommonServices
     * .GetConfigurationFromAllocation: Sp_ConfigrationsAllocation_GetAllMethod
     * @Activity='GetConfigurationByOrgCompandConfigDescription'. "" when the company has no row.
     */
    public String config(int org, int comp, String description) {
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", org);
        p.put("@CompanyId", comp);
        p.put("@ConfigDescription", description);
        p.put("@Activity", "GetConfigurationByOrgCompandConfigDescription");
        List<Map<String, Object>> rows = query("dbo.Sp_ConfigrationsAllocation_GetAllMethod", p);
        if (rows.isEmpty()) return "";
        Object v = ci(rows.get(0), "ConfigKey");
        return v == null ? "" : String.valueOf(v).trim();
    }

    /** CommonServices.GetERPFeatureById(11) (Load:348) - USP_GetERPFeaturesByCompanyId. */
    public boolean erpFeature(int org, int comp, int featureId) {
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", org);
        p.put("@CompanyId", comp);
        for (Map<String, Object> r : query("dbo.USP_GetERPFeaturesByCompanyId", p)) {
            Object id = ci(r, "Id");
            if (id instanceof Number && ((Number) id).intValue() == featureId) return true;
        }
        return false;
    }

    /**
     * tblUserRights.GetByUserId (BLL): @UserId, @ScreenName, @RightName (the role name),
     * @CompanyId (the model's, else clsGlobalVariables.UserAccount.CompanyId), @Activity='GetByUserId'.
     */
    public List<Map<String, Object>> userRights(int userId, String roleName, int companyId) {
        LinkedHashMap<String, Object> p = params();
        p.put("@UserId", userId);
        p.put("@ScreenName", SCREEN_NAME);
        p.put("@RightName", roleName == null ? "" : roleName);
        p.put("@CompanyId", companyId);
        p.put("@Activity", "GetByUserId");
        return query("dbo.Sp_tblUserRights_GetAllMethod", p);
    }

    // ==================================================================================== Load

    /** Load:349 - InvContractorWagesBillHeader.WagesTypeIdsAgainstDocumentType_GetAll (no parameters). */
    public List<Map<String, Object>> wagesTypeIdsAgainstDocumentType() {
        return query("[dbo].[USP_WagesTypeIdsAgainstDocumentType_GetAll]", params());
    }

    /**
     * suppliercustomer():506 (MultiBranch feature off) - SupplierCustomer
     * .ReadByOrganizationCompanyIdForContractorWages: @OrganizationId, @CompanyId, @Activity.
     */
    public List<Map<String, Object>> contractorsForWages(int org, int comp) {
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", org);
        p.put("@CompanyId", comp);
        p.put("@Activity", "ReadByOrganizationCompanyIdForContractorWages");
        return query("dbo.Sp_SupplierCustomer_GetAllMethod", p);
    }

    /**
     * suppliercustomer():514 (MultiBranch feature on) - ContractorsAllocationToBranch
     * .GetContractorsAllocatedToBranch: @OrganizationId, @CompanyId, @BranchId (unconditional).
     */
    public List<Map<String, Object>> contractorsAllocatedToBranch(int org, int comp, int branchId) {
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", org);
        p.put("@CompanyId", comp);
        p.put("@BranchId", branchId);
        return query("[dbo].[USP_GetContractorsAllocatedToBranch]", p);
    }

    /**
     * CommonServices.GetWagesAccount(Ids, WagesActivityId, ActionId) -> InvConractorWagesAccounts
     * .GetWagesItemsByWagesTypeIds: @WagesLookupIds only when Ids is non-null and non-empty,
     * @WagesActivityId only when &gt; 0, @ActionId only when &gt; 0, @Activity='GetWagesItemsByWagesTypeIds'.
     */
    public List<Map<String, Object>> wagesAccounts(int org, int comp, String ids, int wagesActivityId, int actionId) {
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", org);
        p.put("@CompanyId", comp);
        if (ids != null && !ids.isEmpty()) p.put("@WagesLookupIds", ids);
        if (wagesActivityId > 0) p.put("@WagesActivityId", wagesActivityId);
        if (actionId > 0) p.put("@ActionId", actionId);
        p.put("@Activity", "GetWagesItemsByWagesTypeIds");
        return query("dbo.Sp_InvConractorWagesAccounts_GetAllMethod", p);
    }

    /**
     * GenerateDocNo():475 - InvContractorWagesBillHeader.GenerateCode: @OrganizationId, @CompanyId,
     * @RefDocumentTypeId=101, @FinancialYearId, @Activity='GenerateCode'. DocNo of row 0, else 0.
     */
    public int generateCode(int org, int comp, int financialYearId) {
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", org);
        p.put("@CompanyId", comp);
        p.put("@RefDocumentTypeId", DOC_TYPE_WAGES);
        p.put("@FinancialYearId", financialYearId);
        p.put("@Activity", "GenerateCode");
        List<Map<String, Object>> rows = query("dbo.Sp_InvContractorWagesBillHeader_GetAllMethod", p);
        return rows.isEmpty() ? 0 : toInt(ci(rows.get(0), "DocNo"));
    }

    /**
     * Load:395 / LoadDataForWages:2186 - GetIdByRefDocTypeIdAndRefDocId: @OrganizationId,
     * @CompanyId, @RefDocumentTypeId, @RefDocNoId, @FinancialYearId always; @ReqType only when
     * RefDocument is non-null and non-empty; @Activity. Id of row 0, else 0.
     */
    public int idByReference(int org, int comp, int refDocumentTypeId, int refDocNoId, int financialYearId,
                             String refDocument) {
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", org);
        p.put("@CompanyId", comp);
        p.put("@RefDocumentTypeId", refDocumentTypeId);
        p.put("@RefDocNoId", refDocNoId);
        p.put("@FinancialYearId", financialYearId);
        if (refDocument != null && !refDocument.isEmpty()) p.put("@ReqType", refDocument);
        p.put("@Activity", "GetIdByRefDocTypeIdAndRefDocId");
        List<Map<String, Object>> rows = query("dbo.Sp_InvContractorWagesBillHeader_GetAllMethod", p);
        return rows.isEmpty() ? 0 : toInt(ci(rows.get(0), "Id"));
    }

    /**
     * DocumentTypeFillForCombo():582 - ComboAgainstContractorWages: @OrganizationId, @CompanyId;
     * @Activity, @DocumentTypeIds, @BranchesIds each only when non-null and non-empty.
     * The form sets Activity='RefDocumentType' and BranchesIds=UserAccount.BranchesId.ToString().
     */
    public List<Map<String, Object>> comboAgainstContractorWages(int org, int comp, String activity, String branchesIds) {
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", org);
        p.put("@CompanyId", comp);
        if (activity != null && !activity.isEmpty()) p.put("@Activity", activity);
        if (branchesIds != null && !branchesIds.isEmpty()) p.put("@BranchesIds", branchesIds);
        return query("[dbo].[Usp_AllComboAgainstContractorWages]", p);
    }

    /**
     * InvContractorWagesBillHeader.GetByID(id): Sp_InvContractorWagesBillHeader_GetAllMethod
     * @Id, @Activity='ReadById' mapped to headers, then DAL GetData loads each header's details with
     * @Id, @Activity='ReadDetailByHeaderId'; the BLL returns element [0]. Null when there is no row
     * (the desktop would throw ArgumentOutOfRange there).
     */
    public Map<String, Object> getById(int id) {
        LinkedHashMap<String, Object> p = params();
        p.put("@Id", id);
        p.put("@Activity", "ReadById");
        List<Map<String, Object>> heads = query("dbo.Sp_InvContractorWagesBillHeader_GetAllMethod", p);
        if (heads.isEmpty()) return null;
        Map<String, Object> head = new LinkedHashMap<>(heads.get(0));
        LinkedHashMap<String, Object> d = params();
        d.put("@Id", toInt(ci(head, "Id")));
        d.put("@Activity", "ReadDetailByHeaderId");
        head.put("invContractWagesBillDateil", query("dbo.Sp_InvContractorWagesBillHeader_GetAllMethod", d));
        return head;
    }

    /**
     * RetreivalDetailGridDeletedData():2343 - ContractorWagesBillHeader_ReadPreviousRecords:
     * @OrganizationId, @CompanyId, @RefDocumentTypeId, @RefDocNoId, @FinancialYearId (all always).
     */
    public List<Map<String, Object>> readPreviousRecords(int org, int comp, int financialYearId,
                                                         int refDocumentTypeId, int refDocNoId) {
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", org);
        p.put("@CompanyId", comp);
        p.put("@RefDocumentTypeId", refDocumentTypeId);
        p.put("@RefDocNoId", refDocNoId);
        p.put("@FinancialYearId", financialYearId);
        return query("[dbo].[USP_ContractorWagesBillHeader_ReadPreviousRecords]", p);
    }

    /**
     * PendingGrnAndGdn():1985 - InvGrn.GetPendingGrnAndGdnForConractorWagesByRefIds:
     * @OrganizationId, @CompanyId, @DocumentTypeId, @Id always; @ReqType is guarded and the form
     * never sets it, so it is omitted; @Activity.
     */
    public List<Map<String, Object>> pendingByReference(int org, int comp, int documentTypeId, int id) {
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", org);
        p.put("@CompanyId", comp);
        p.put("@DocumentTypeId", documentTypeId);
        p.put("@Id", id);
        p.put("@Activity", "GetPendingGrnAndGdnForConractorWagesByRefIds");
        return query("dbo.Sp_InvContractorWagesBillHeader_GetAllMethod", p);
    }

    /**
     * PendingTicket():2052 - InvGrn.GetAllPendingRecordsForConractorWages: @OrganizationId,
     * @CompanyId always; @DocumentTypeId, @Id, @BranchesId each only when non-zero.
     */
    public List<Map<String, Object>> pendingAll(int org, int comp, int branchId, int documentTypeId, int id) {
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", org);
        p.put("@CompanyId", comp);
        if (documentTypeId != 0) p.put("@DocumentTypeId", documentTypeId);
        if (id != 0) p.put("@Id", id);
        if (branchId != 0) p.put("@BranchesId", branchId);
        return query("[dbo].[USP_GetAllPendingRecordsForConractorWages]", p);
    }

    /**
     * LoadDataForWages():2209 - InvGrn.GetGrnDetialForContractorWages: @OrganizationId, @CompanyId,
     * @Id, @DocumentTypeId always; @ReqType when (ReqType != "") - a null ReqType becomes a
     * parameter with a null value, which ADO.NET does not send, so it is omitted as well.
     */
    public List<Map<String, Object>> referenceDetail(int org, int comp, int id, int documentTypeId, String reqType) {
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", org);
        p.put("@CompanyId", comp);
        p.put("@Id", id);
        p.put("@DocumentTypeId", documentTypeId);
        if (reqType != null && !reqType.isEmpty()) p.put("@ReqType", reqType);
        p.put("@Activity", "GetGrnGdnTransferDetailForContractorWagesByDocumentTypeIdAndId");
        return query("dbo.Sp_InvContractorWagesBillHeader_GetAllMethod", p);
    }

    /**
     * CommonServices.CheckItemsFreeofcostforWages -> InvContractorWagesSchedule
     * .CheckItemsFreeofcostforWages: USP_CheckItemsFreeofcostforWages @OrganizationId, @CompanyId,
     * @ItemId, @DocDate, @RefDocumentTypeId, @WagesAccountId (all always). IsFreeocCost of row 0.
     */
    public boolean freeOfCost(int org, int comp, LocalDateTime docDate, int refDocumentTypeId, int itemId,
                              int wagesAccountId) {
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", org);
        p.put("@CompanyId", comp);
        p.put("@ItemId", itemId);
        p.put("@DocDate", docDate);
        p.put("@RefDocumentTypeId", refDocumentTypeId);
        p.put("@WagesAccountId", wagesAccountId);
        List<Map<String, Object>> rows = query("USP_CheckItemsFreeofcostforWages", p);
        return !rows.isEmpty() && toBool(ci(rows.get(0), "IsFreeocCost"));
    }

    /**
     * CommonServices.GetWagesRate -> InvContractorWagesSchedule
     * .GetWagesScheduleRateByEffectiveDateWagesAccountIdandPackSize: @OrganizationId, @CompanyId,
     * @InvConractorWagesAccountsId, @ContractorId, @EffectedDate, @PackUomFrom always; @PackUomTo only
     * when non-zero (never set here); @Activity. WageRate / Id of row 0, else 0 / 0.
     */
    public double[] wagesRate(int org, int comp, LocalDateTime date, double packUom, int wagesAccountId,
                              int contractorId) {
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", org);
        p.put("@CompanyId", comp);
        p.put("@InvConractorWagesAccountsId", wagesAccountId);
        p.put("@ContractorId", contractorId);
        p.put("@EffectedDate", date);
        p.put("@PackUomFrom", packUom);
        p.put("@Activity", "GetWagesScheduleRateByEffectiveDateWagesAccountIdandPackSize");
        List<Map<String, Object>> rows = query("dbo.Sp_InvContractorWagesSchedule_GetAllMethod", p);
        if (rows.isEmpty()) return new double[]{0d, 0d};
        return new double[]{toDouble(ci(rows.get(0), "WageRate")), toInt(ci(rows.get(0), "Id"))};
    }

    /**
     * ValidationOnformClose():2614/2626 - InvContractorWagesBillHeader.WagesDeleteByRefDocTypeAndId:
     * USP_WagesDeleteByRefDocTypeAndId @OrganizationId, @CompanyId, @RefDocumentTypeId, @RefDocId.
     * (The BLL spells the last name "@RefDocId " with a trailing space; SQL Server ignores trailing
     * blanks when it matches the name, so it binds to @RefDocId.)
     */
    public void wagesDeleteByRefDocTypeAndId(int org, int comp, int refDocumentTypeId, int refDocId) {
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", org);
        p.put("@CompanyId", comp);
        p.put("@RefDocumentTypeId", refDocumentTypeId);
        p.put("@RefDocId", refDocId);
        scalar("USP_WagesDeleteByRefDocTypeAndId", p);
    }

    /**
     * bindHistory():3270 - FormHistoryNew: @OrganizationId, @CompanyId, @DocumentTypeId always;
     * @FinancialYearId, @BranchesId, @RefDocumentTypeId, @NoOfRecords when non-zero; @FromDate /
     * @ToDate when set; @FromDocNo / @ToDocNo when non-zero.
     */
    public List<Map<String, Object>> formHistory(int org, int comp, int financialYearId, int branchId,
                                                 LocalDateTime from, LocalDateTime to, int fromDocNo, int toDocNo,
                                                 int refDocumentTypeId) {
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", org);
        p.put("@CompanyId", comp);
        p.put("@DocumentTypeId", DOC_TYPE_WAGES);
        if (financialYearId != 0) p.put("@FinancialYearId", financialYearId);
        if (branchId != 0) p.put("@BranchesId", branchId);
        if (from != null) p.put("@FromDate", from);
        if (to != null) p.put("@ToDate", to);
        if (fromDocNo != 0) p.put("@FromDocNo", (double) fromDocNo);
        if (toDocNo != 0) p.put("@ToDocNo", (double) toDocNo);
        if (refDocumentTypeId != 0) p.put("@RefDocumentTypeId", refDocumentTypeId);
        return query("[dbo].[USP_ContractorWagesBillHeader_FormHistory]", p);
    }

    /**
     * BtnCancelPendingRecords_Click:4073 - UpdateContractorWagesRefDocumentStatus(ReportsParameters):
     * per checked row, [dbo].[USP_ContractorWagesCancelRecords_InsertAndUpdate] with @Id=0,
     * @RefDocumentTypeId, @RefDocId, @OrganizationId, @CompanyId, @EntryUserId, @ModifyUserId
     * (never set by the form, so 0). The caller runs the rows in one transaction.
     */
    public void cancelPendingRecord(int org, int comp, int refDocumentTypeId, int refDocId, int entryUser) {
        LinkedHashMap<String, Object> p = params();
        p.put("@Id", 0);
        p.put("@RefDocumentTypeId", refDocumentTypeId);
        p.put("@RefDocId", refDocId);
        p.put("@OrganizationId", org);
        p.put("@CompanyId", comp);
        p.put("@EntryUserId", entryUser);
        p.put("@ModifyUserId", 0);
        scalar("[dbo].[USP_ContractorWagesCancelRecords_InsertAndUpdate]", p);
    }

    /**
     * CommonServices.VoucherHeadIdGet(id, 101) -> VoucherHead.GetVoucherHeadIdByDocumentTypeIdandRefDocNoId
     * and DAL SetData:0x0142 (the same call): Sp_Vouchers_GetMethods @Activity, @OrganizationId,
     * @CompanyId, @DocumentTypeId, @DocumentTypeSrNo. Id of row 0, else 0.
     */
    public int voucherHeadId(int org, int comp, int documentTypeId, int id) {
        LinkedHashMap<String, Object> p = params();
        p.put("@Activity", "GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId");
        p.put("@OrganizationId", org);
        p.put("@CompanyId", comp);
        p.put("@DocumentTypeId", documentTypeId);
        p.put("@DocumentTypeSrNo", id);
        List<Map<String, Object>> rows = query("Sp_Vouchers_GetMethods", p);
        return rows.isEmpty() ? 0 : toInt(ci(rows.get(0), "Id"));
    }

    /** Remove the old bills of a reference document - kept for the callers (see the service). */
    public void contractorWagesRemoveByReferenceIds(int org, int comp, int documentTypeId, int id) {
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", org);
        p.put("@CompanyId", comp);
        p.put("@DocumentTypeId", documentTypeId);
        p.put("@Id", id);
        scalar("usp_ContractorWagesRemovebyReferenceIds", p);
    }

    // ================================================================= MakeVoucher look-ups (BLL)

    /** MakeVoucher:0x03b8 - Sp_InvProductionJobOrder_GetAllMethod 'GetWipAccountsByProductionId'. */
    public List<Map<String, Object>> wipAccountsByProductionId(int org, int comp, int id) {
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", org);
        p.put("@CompanyId", comp);
        p.put("@Id", id);
        p.put("@Activity", "GetWipAccountsByProductionId");
        return query("Sp_InvProductionJobOrder_GetAllMethod", p);
    }

    /** MakeVoucher:0x04b7 - Sp_InvProductionJobOrder_GetAllMethod 'GetGlAccountsByJobOrderId'. */
    public List<Map<String, Object>> glAccountsByJobOrderId(int org, int comp, int jobOrderId) {
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", org);
        p.put("@CompanyId", comp);
        p.put("@Id", jobOrderId);
        p.put("@Activity", "GetGlAccountsByJobOrderId");
        return query("Sp_InvProductionJobOrder_GetAllMethod", p);
    }

    /** CommonServies.GetSupplierCustomerListForFinancialEffects - 'GetGlAccountIdandCompanyNameBySupplierCustomerId'. */
    public List<Map<String, Object>> supplierCustomerGlAccounts(int org, int comp) {
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", org);
        p.put("@CompanyId", comp);
        p.put("@Activity", "GetGlAccountIdandCompanyNameBySupplierCustomerId");
        return query("Sp_SupplierCustomer_GetAllMethod", p);
    }

    /** CommonServies.GetItemListForFinancialEffects - Sp_Item_GetAllMethod 'GetItemGlIdsandItemName'. */
    public List<Map<String, Object>> itemGlIdsAndItemName(int org, int comp) {
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", org);
        p.put("@CompanyId", comp);
        p.put("@Activity", "GetItemGlIdsandItemName");
        return query("Sp_Item_GetAllMethod", p);
    }

    /** MakeVoucher:0x0609 - usp_getExportInvoiceDataByForwardingId @OrganizationId, @CompanyId, @Id. */
    public List<Map<String, Object>> exportInvoiceDataByForwardingId(int org, int comp, int id) {
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", org);
        p.put("@CompanyId", comp);
        p.put("@Id", id);
        return query("usp_getExportInvoiceDataByForwardingId", p);
    }

    /** MakeVoucher:0x06f8 - DAL InvConractorWagesAccounts.GetData 'ReadAll' (@OrganizationId, @CompanyId). */
    public List<Map<String, Object>> contractorWagesAccountsAll(int org, int comp) {
        LinkedHashMap<String, Object> p = params();
        p.put("@OrganizationId", org);
        p.put("@CompanyId", comp);
        p.put("@Activity", "ReadAll");
        return query("Sp_InvConractorWagesAccounts_GetAllMethod", p);
    }

    // ============================================================================ SetData writes

    /** GenericProvider.SetProc - ExecuteScalar with the model's properties, Convert.ToInt32 of it. */
    public int setProc(String proc, Map<String, Object> modelParams) {
        Object v = scalar(proc, modelParams);
        return toInt(v);
    }

    /** SqlCommand with explicit AddWithValue parameters - ExecuteNonQuery. */
    public void command(String proc, Map<String, Object> p) {
        scalar(proc, p);
    }

    static void log(String m, Object... a) { LOG.debug(m, a); }
}
