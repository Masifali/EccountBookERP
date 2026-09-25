package com.mst.repositories;

import com.mst.models.UserAccount;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.jdbc.core.ConnectionCallback;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;
import org.springframework.util.LinkedCaseInsensitiveMap;

import java.math.BigDecimal;
import java.math.MathContext;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.ResultSetMetaData;
import java.sql.SQLException;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;

import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Stock Conversion — the procedures the desktop calls, with its own parameters.
 *
 * Header DocTypeId is <b>66</b> (GetGenerateCode, and the branch in DAL 0275 GetData). The 101/66
 * pair at invfrmStockConversionProduction.cs:5034 belongs to the contractor-wages sub-document,
 * not to this header.
 *
 * ---------------------------------------------------------------------------------------------
 * THE SCHEMA'S OWN SPELLINGS ARE KEPT
 * ---------------------------------------------------------------------------------------------
 *   @DocTypeId    on the GetAllMethod activities, NOT @DocumentTypeId
 *   @BranchedId   NOT @BranchesId
 *   @DocNo        on ReadByDocNo, carrying the model's DocSrNo
 *
 * They look like typos. They are the parameter names the procedures declare, so "fixing" one
 * makes the call fail. No table, column or procedure is created by this class.
 *
 * ---------------------------------------------------------------------------------------------
 * READ AND WRITE
 * ---------------------------------------------------------------------------------------------
 * {@link #save} is DAL InvStockConversion.SetData step for step in one transaction: header,
 * packing materials, expenses, details, stock evaluation, Sp_InventoryTransactions_GetALLMethod
 * (the procedure rebuilds the stock rows itself), the removed-row delete, the two validation
 * procedures, the voucher MakeVoucher-style composition with USP_VoucherBalanceCheck, and the
 * contractor wages bills. {@link #delete} is InvPurchaseInvoice.RemoveByID for DocumentTypeId 66.
 * Every write procedure receives exactly the parameters it declares, in the model's spelling.
 */
@Repository
public class StockConversionRepository {

    private static final Logger LOG = LoggerFactory.getLogger(StockConversionRepository.class);

    public static final int DOC_TYPE_ID = 66;

    private static final String P_HEAD    = "Sp_InvStockConversion_GetAllMethod";
    private static final String P_DETAIL  = "Sp_InvStockConversionDetail_GetAllMethod";
    private static final String P_PACKING = "Sp_InvStockConversionPackingMaterial_GetAllMethod";
    private static final String P_EXPENSE = "Sp_InvStockConversionAddExpense_GetAllMethod";
    private static final String P_HISTORY = "USP_InvStockConversion_FormHistory";
    private static final String P_CONFIG  = "Sp_ConfigrationsAllocation_GetAllMethod";

    private final JdbcTemplate jdbc;
    public StockConversionRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    // =============================================================================== plumbing

    private static Map<String, Object> params() { return new LinkedHashMap<>(); }

    private List<Map<String, Object>> exec(String proc, Map<String, Object> p) {
        StringBuilder sql = new StringBuilder("EXEC dbo.").append(proc);
        List<Object> values = new ArrayList<>();
        boolean first = true;
        for (Map.Entry<String, Object> e : p.entrySet()) {
            sql.append(first ? " " : ", ").append(e.getKey()).append("=?");
            values.add(e.getValue());
            first = false;
        }
        return jdbc.queryForList(sql.toString(), values.toArray());
    }

    private static int intOf(Map<String, Object> row, String name) {
        if (row == null) return 0;
        Object v = row.get(name);
        if (v == null) {
            for (Map.Entry<String, Object> e : row.entrySet())
                if (e.getKey().equalsIgnoreCase(name)) { v = e.getValue(); break; }
        }
        if (v instanceof Number) return ((Number) v).intValue();
        if (v == null) return 0;
        try { return Integer.parseInt(String.valueOf(v).trim()); }
        catch (NumberFormatException e) { return 0; }
    }

    private static boolean set(Integer v) { return v != null && v != 0; }
    private static boolean set(String v)  { return v != null && !v.trim().isEmpty(); }

    // =================================================================================== reads

    /** GenerateCode — BLL 0337 :35. @FinancialYearId and @BranchedId only when non-zero. */
    public int nextCode(UserAccount u, int financialYearId) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@DocTypeId", DOC_TYPE_ID);
        if (financialYearId != 0) p.put("@FinancialYearId", financialYearId);
        if (set(u.getBranchesId())) p.put("@BranchedId", u.getBranchesId());
        p.put("@Activity", "GenerateCode");
        List<Map<String, Object>> rows = exec(P_HEAD, p);
        return rows.isEmpty() ? 0 : firstInt(rows.get(0));
    }

    /** ReadById — BLL 0337 :90. */
    public Map<String, Object> header(int id) {
        Map<String, Object> p = params();
        p.put("@Activity", "ReadById");
        p.put("@Id", id);
        List<Map<String, Object>> rows = exec(P_HEAD, p);
        return rows.isEmpty() ? null : rows.get(0);
    }

    /** ReadByDocNo — BLL 0337 :114. @DocNo carries the model's DocSrNo. */
    public int idByDocNo(UserAccount u, int docSrNo, int financialYearId) {
        Map<String, Object> p = params();
        p.put("@Activity", "ReadByDocNo");
        p.put("@DocNo", docSrNo);
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        if (financialYearId != 0) p.put("@FinancialYearId", financialYearId);
        List<Map<String, Object>> rows = exec(P_HEAD, p);
        return rows.isEmpty() ? 0 : intOf(rows.get(0), "Id");
    }

    /**
     * Details — DAL 0275 GetData. The activity depends on the header's own DocTypeId:
     * 66 and 808 read ReadByHeaderId, 67 reads ReadByHeaderIdForTrading. Passed in rather than
     * assumed, because a header loaded by id carries its own type.
     */
    public List<Map<String, Object>> details(int headerId, int docTypeId) {
        Map<String, Object> p = params();
        p.put("@Id", headerId);
        p.put("@Activity", docTypeId == 67 ? "ReadByHeaderIdForTrading" : "ReadByHeaderId");
        return exec(P_DETAIL, p);
    }

    public List<Map<String, Object>> packings(int headerId) {
        Map<String, Object> p = params();
        p.put("@Id", headerId);
        p.put("@Activity", "ReadByHeaderId");
        return exec(P_PACKING, p);
    }

    public List<Map<String, Object>> expenses(int headerId) {
        Map<String, Object> p = params();
        p.put("@Id", headerId);
        p.put("@Activity", "ReadByHeaderId");
        return exec(P_EXPENSE, p);
    }

    /**
     * FormHistoryNew — BLL 0337 :210, USP_InvStockConversion_FormHistory.
     *
     * Every optional filter is sent only when it is set, exactly as the C# does: the desktop tests
     * each date with Conversion.CheckDateTimeNull and each number for zero before adding it.
     * Sending a NULL where the desktop sends nothing is not the same thing to a procedure that
     * branches on the parameter's presence.
     *
     * canViewAllRecord and entryUser are derived from the signed-in user before they get here and
     * are never accepted from a caller — @EntryUser is only sent when CanViewAllRecord is false,
     * as in the C#.
     */
    public List<Map<String, Object>> history(UserAccount u, int financialYearId,
                                             boolean canViewAllRecord, int entryUser,
                                             String fromDate, String toDate,
                                             String entryFromDate, String entryToDate,
                                             String modifyFromDate, String modifyToDate,
                                             String approvedDateFrom, String approvedDateTo,
                                             Integer docNoFrom, Integer docNoTo) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@BranchedId", u.getBranchesId());
        p.put("@DocumentTypeId", DOC_TYPE_ID);
        p.put("@FinancialYearId", financialYearId);
        p.put("@CanViewAllRecord", canViewAllRecord);
        if (!canViewAllRecord)        p.put("@EntryUser", entryUser);
        if (set(fromDate))            p.put("@FromDate", fromDate);
        if (set(toDate))              p.put("@ToDate", toDate);
        if (set(entryFromDate))       p.put("@EntryFromDate", entryFromDate);
        if (set(entryToDate))         p.put("@EntryToDate", entryToDate);
        if (set(modifyFromDate))      p.put("@ModifyFromDate", modifyFromDate);
        if (set(modifyToDate))        p.put("@ModifyToDate", modifyToDate);
        if (set(approvedDateFrom))    p.put("@ApprovedDateFrom", approvedDateFrom);
        if (set(approvedDateTo))      p.put("@ApprovedDateTo", approvedDateTo);
        if (set(docNoFrom))           p.put("@DocNoFrom", docNoFrom);
        if (set(docNoTo))             p.put("@DocNoTo", docNoTo);
        try {
            return exec(P_HISTORY, p);
        } catch (Exception e) {
            LOG.warn("{} failed", P_HISTORY, e);
            throw e;
        }
    }

    // ------------------------------------------------------------------- stock helper reads

    /** SpInventoryTransactions_Filters — BLL 0337 :341. @Activity comes from the caller. */
    public List<Map<String, Object>> stockFilter(UserAccount u, String activity,
                                                 Integer itemCategoryId, String docDateTo,
                                                 Integer warehouseId, String cropYear,
                                                 Integer itemId, Integer jobLotId) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        if (set(itemCategoryId)) p.put("@InventoryParentCategoryId", itemCategoryId);
        if (set(docDateTo))      p.put("@DocDateTo", docDateTo);
        if (set(warehouseId))    p.put("@WarehouseId", warehouseId);
        if (set(cropYear))       p.put("@CropYear", cropYear);
        if (set(itemId))         p.put("@ItemId", itemId);
        if (set(jobLotId))       p.put("@JobLotId", jobLotId);
        p.put("@Activity", activity);
        return exec("SpInventoryTransactions_Filters", p);
    }

    /** SpInventoryTransactionEvaluation_GetAvailableQtyStockForIssuance — BLL 0337 :417. */
    public List<Map<String, Object>> availableStock(UserAccount u, Integer itemCategoryId,
                                                    String dateTo, Integer warehouseId,
                                                    Integer itemId, Integer jobLotId) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        if (set(itemCategoryId)) p.put("@InventoryParentCategories", itemCategoryId);
        if (set(dateTo))         p.put("@DateTo", dateTo);
        if (set(warehouseId))    p.put("@WarehouseId", warehouseId);
        if (set(itemId))         p.put("@ItemId", itemId);
        if (set(jobLotId))       p.put("@JobLotId", jobLotId);
        return exec("SpInventoryTransactionEvaluation_GetAvailableQtyStockForIssuance", p);
    }

    /** usp_getStoreAndPMItemsWithStockInHand — BLL 0337 :480. */
    public List<Map<String, Object>> storeAndPmItems(UserAccount u) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        return exec("usp_getStoreAndPMItemsWithStockInHand", p);
    }

    // ============================================================================ edit-side reads

    /**
     * Statement plumbing for the write side and the new lookups: one EXEC on the Spring-managed
     * connection (so inside {@link #save} every call shares the one transaction, exactly as every
     * SetData call shares the desktop's SqlTransaction). A null value is OMITTED - the desktop's
     * SqlParameter with a null Value is "not supplied" and the procedure's default applies.
     * Every result set is drained so a RAISERROR raised after a SELECT still surfaces.
     */
    public List<Map<String, Object>> rows(String proc, Map<String, Object> params) {
        StringBuilder sql = new StringBuilder("EXEC ").append(proc.startsWith("[") ? proc : "dbo." + proc);
        List<Object> values = new ArrayList<>();
        boolean first = true;
        for (Map.Entry<String, Object> e : params.entrySet()) {
            if (e.getValue() == null) continue;
            sql.append(first ? " " : ", ").append(e.getKey()).append("=?");
            values.add(e.getValue());
            first = false;
        }
        final String text = sql.toString();
        return jdbc.execute((ConnectionCallback<List<Map<String, Object>>>) con -> {
            try (PreparedStatement ps = con.prepareStatement(text)) {
                for (int i = 0; i < values.size(); i++) bind(ps, i + 1, values.get(i));
                boolean isRs = ps.execute();
                List<Map<String, Object>> out = null;
                while (true) {
                    if (isRs) {
                        try (ResultSet rs = ps.getResultSet()) {
                            if (out == null) out = read(rs);
                            else while (rs.next()) { /* drain */ }
                        }
                    } else if (ps.getUpdateCount() == -1) {
                        break;
                    }
                    isRs = ps.getMoreResults();
                }
                return out == null ? new ArrayList<>() : out;
            }
        });
    }

    /** ExecuteScalar: first column of the first row of the first result set, or null. */
    public Object scalar(String proc, Map<String, Object> params) {
        List<Map<String, Object>> r = rows(proc, params);
        if (r.isEmpty() || r.get(0).isEmpty()) return null;
        return r.get(0).values().iterator().next();
    }

    private static void bind(PreparedStatement ps, int i, Object v) throws SQLException {
        if (v instanceof LocalDateTime) ps.setTimestamp(i, Timestamp.valueOf((LocalDateTime) v));
        else if (v instanceof LocalDate) ps.setTimestamp(i, Timestamp.valueOf(((LocalDate) v).atStartOfDay()));
        else if (v instanceof java.util.Date && !(v instanceof Timestamp))
            ps.setTimestamp(i, new Timestamp(((java.util.Date) v).getTime()));
        else ps.setObject(i, v);
    }

    private static List<Map<String, Object>> read(ResultSet rs) throws SQLException {
        List<Map<String, Object>> out = new ArrayList<>();
        ResultSetMetaData md = rs.getMetaData();
        int n = md.getColumnCount();
        while (rs.next()) {
            Map<String, Object> row = new LinkedCaseInsensitiveMap<>(n);
            for (int c = 1; c <= n; c++) {
                String name = md.getColumnLabel(c);
                if (name == null || name.isEmpty()) name = md.getColumnName(c);
                if (name == null || name.isEmpty()) name = "Column" + c;
                Object v = rs.getObject(c);
                if (v instanceof Timestamp) v = ((Timestamp) v).toLocalDateTime().toString();
                else if (v instanceof java.sql.Date) v = ((java.sql.Date) v).toLocalDate().toString();
                row.put(name, v);
            }
            out.add(row);
        }
        return out;
    }

    /** Reads that must not break the page: logged with the procedure, empty on failure. */
    private List<Map<String, Object>> safe(String label, String proc, Map<String, Object> p) {
        try {
            return rows(proc, p);
        } catch (Exception e) {
            LOG.warn("Stock Conversion lookup '{}' failed: {}", label, proc, e);
            return new ArrayList<>();
        }
    }

    /**
     * GetAvgRate:1162 -> CommonServices.AvgRateOnlyForCGS -> GetAvgRatesAndStockInHand.AvgRateOnlyForCGS
     * (BLL): Org, Company, @ItemId, @DocDate always; @DocumentTypeId, @RecId, @JobLotId,
     * @CropYearId, @WarehouseId, @BranchesId when != 0; @CropYear when not empty;
     * @Activity='GetOnlyAvgRateForCGS'. AvgRate of row 0, else 0.
     */
    public double avgRateOnlyForCgs(UserAccount u, int itemId, LocalDateTime docDate, int documentTypeId,
                                    int recId, int jobLotId, int cropYearId, String cropYear,
                                    int warehouseId, int branchesId) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@ItemId", itemId);
        p.put("@DocDate", docDate);
        if (documentTypeId != 0) p.put("@DocumentTypeId", documentTypeId);
        if (recId != 0) p.put("@RecId", recId);
        if (jobLotId != 0) p.put("@JobLotId", jobLotId);
        if (cropYearId != 0) p.put("@CropYearId", cropYearId);
        if (set(cropYear)) p.put("@CropYear", cropYear);
        if (warehouseId != 0) p.put("@WarehouseId", warehouseId);
        if (branchesId != 0) p.put("@BranchesId", branchesId);
        p.put("@Activity", "GetOnlyAvgRateForCGS");
        List<Map<String, Object>> r = rows("Sp_GetAvgRatesAndStockInHand_GetAllMethod", p);
        return r.isEmpty() ? 0d : dbl(ci(r.get(0), "AvgRate"));
    }

    /**
     * grdPackingMaterial_CellUpdated:2864 / AvgRateUpdateOnDocDateChangeForPm:7662 ->
     * CommonServices.GetAvgRateQtyAndStockInHand -> GetAvgRatesAndStockInHand.GetAvgRateQtyAndStockInHand
     * (BLL): Org, Company, @ItemId, @DocDate always; @DocumentTypeId, @RecId, @ItemConditionId,
     * @WarehouseId, @StoreRackId only when != 0 (this form never passes a warehouse or rack).
     */
    public List<Map<String, Object>> pmAvgRate(UserAccount u, int itemId, LocalDateTime docDate,
                                               int itemConditionId, int recId, int documentTypeId) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@ItemId", itemId);
        p.put("@DocDate", docDate);
        if (documentTypeId != 0) p.put("@DocumentTypeId", documentTypeId);
        if (recId != 0) p.put("@RecId", recId);
        if (itemConditionId != 0) p.put("@ItemConditionId", itemConditionId);
        p.put("@Activity", "GetAvgRateQtyAndStockInHand");
        return rows("Sp_GetAvgRatesAndStockInHand_GetAllMethod", p);
    }

    /** GridPmDropdownBind:2924 -> Item.GetItemByItemTypeId: Org, Company, @LookupTypeIds (always),
     *  @Activity='GetItemByItemTypeId'. Ids = "14,17" for Conversion Type 5, else "14". */
    public List<Map<String, Object>> pmItems(UserAccount u, String ids) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@LookupTypeIds", ids);
        p.put("@Activity", "GetItemByItemTypeId");
        return safe("pmItems", "Sp_Item_GetAllMethod", p);
    }

    /** clsGlobalVariables.globalItemConditions - GlobalServicesMethods.GetItemCondtions: the desktop's
     *  own literal "SELECT * FROM dbo.V_ItemCondition" (Id, ConditionStatus). */
    public List<Map<String, Object>> itemConditions() {
        try {
            return jdbc.queryForList("SELECT * FROM dbo.V_ItemCondition");
        } catch (Exception e) {
            LOG.warn("V_ItemCondition failed", e);
            return new ArrayList<>();
        }
    }

    /** ScheduleNoDbCall:2896 -> ExportContractSchedule.ExportContractSchedule_GetPending (BLL): Org,
     *  Company; @RecId only when != 0 (the form passes RecId); ActionId never set. */
    public List<Map<String, Object>> pendingSchedules(UserAccount u, int recId) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        if (recId != 0) p.put("@RecId", recId);
        return safe("pendingSchedules", "[dbo].[usp_ExportContractSchedule_GetPending]", p);
    }

    /** GridOverHeadDropdownBind:2671 -> CommonServices.CoaAllocationGetAllServiceBind -> COAAllocation.GetAll
     *  (BLL): Org, Company, @UserId when != 0, @Activity='COAAllocationSearch'. Id / AccountTitle. */
    public List<Map<String, Object>> overheadAccounts(UserAccount u) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        if (u.getId() != null && u.getId() != 0) p.put("@UserId", u.getId());
        p.put("@Activity", "COAAllocationSearch");
        return safe("overheadAccounts", "Sp_COAAllocation_GetAllMethod", p);
    }

    /** clsGlobalVariables.racksWithWarehouseAndItems - usp_getRackswithWarehouseByItemId @OrganizationId,
     *  @CompanyId, @BranchId (the global list, every item's racks). Used by the PM grid's F1 pickers. */
    public List<Map<String, Object>> racksWithWarehouseAndItems(UserAccount u, int branchId) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@BranchId", branchId);
        return safe("racks", "usp_getRackswithWarehouseByItemId", p);
    }

    /** Load:623 -> InvContractorWagesBillHeader.WagesTypeIdsAgainstDocumentType_GetAll: no parameters. */
    public List<Map<String, Object>> wagesTypeIdsAgainstDocumentType() {
        return safe("wagesTypeIds", "[dbo].[USP_WagesTypeIdsAgainstDocumentType_GetAll]", params());
    }

    /** CommonServices.GetWagesAccount(Ids, activity, action) -> InvConractorWagesAccounts
     *  .GetWagesItemsByWagesTypeIds (BLL): Org, Company; @WagesLookupIds when not null/""; @WagesActivityId
     *  and @ActionId when > 0; @Activity='GetWagesItemsByWagesTypeIds'. Id / WagesAccountName. */
    public List<Map<String, Object>> wagesAccounts(UserAccount u, String ids, int wagesActivityId, int actionId) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        if (ids != null && !ids.isEmpty()) p.put("@WagesLookupIds", ids);
        if (wagesActivityId > 0) p.put("@WagesActivityId", wagesActivityId);
        if (actionId > 0) p.put("@ActionId", actionId);
        p.put("@Activity", "GetWagesItemsByWagesTypeIds");
        return safe("wagesAccounts", "Sp_InvConractorWagesAccounts_GetAllMethod", p);
    }

    /**
     * suppliercustomer():3172 - without ERP feature 11: SupplierCustomer.ReadByOrganizationCompanyId
     * ForContractorWages (Org, Company, @Activity); with it: ContractorsAllocationToBranch
     * .GetContractorsAllocatedToBranch (Org, Company, @BranchId always). Id / CompanyName.
     */
    public List<Map<String, Object>> contractors(UserAccount u, boolean multiBranch, int branchId) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        if (!multiBranch) {
            p.put("@Activity", "ReadByOrganizationCompanyIdForContractorWages");
            return safe("contractors", "Sp_SupplierCustomer_GetAllMethod", p);
        }
        p.put("@BranchId", branchId);
        return safe("contractors", "[dbo].[USP_GetContractorsAllocatedToBranch]", p);
    }

    /**
     * CommonServices.GetWagesRate -> InvContractorWagesSchedule.GetWagesScheduleRateByEffectiveDate
     * WagesAccountIdandPackSize (BLL): Org, Company, @InvConractorWagesAccountsId, @ContractorId,
     * @EffectedDate, @PackUomFrom always; @PackUomTo only when set (never here). Row 0's WageRate / Id,
     * else null (no schedule).
     */
    public Map<String, Object> wagesRate(UserAccount u, LocalDateTime docDate, double packUom,
                                         int wagesAccountId, int contractorId) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@InvConractorWagesAccountsId", wagesAccountId);
        p.put("@ContractorId", contractorId);
        p.put("@EffectedDate", docDate);
        p.put("@PackUomFrom", packUom);
        p.put("@Activity", "GetWagesScheduleRateByEffectiveDateWagesAccountIdandPackSize");
        List<Map<String, Object>> r = rows("Sp_InvContractorWagesSchedule_GetAllMethod", p);
        if (r.isEmpty()) return null;
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("WagesRate", dbl(ci(r.get(0), "WageRate")));
        m.put("ScheduleId", intOf(r.get(0), "Id"));
        return m;
    }

    /** CommonServices.CheckItemsFreeofcostforWages -> USP_CheckItemsFreeofcostforWages: Org, Company,
     *  @ItemId, @DocDate, @RefDocumentTypeId, @WagesAccountId (all always). Row 0's IsFreeocCost. */
    public boolean wagesFreeOfCost(UserAccount u, LocalDateTime docDate, int refDocumentTypeId,
                                   int itemId, int wagesAccountId) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@ItemId", itemId);
        p.put("@DocDate", docDate);
        p.put("@RefDocumentTypeId", refDocumentTypeId);
        p.put("@WagesAccountId", wagesAccountId);
        List<Map<String, Object>> r = rows("USP_CheckItemsFreeofcostforWages", p);
        return !r.isEmpty() && bool(ci(r.get(0), "IsFreeocCost"));
    }

    /** WagesDetailReadbyId:5285 -> InvContractorWagesBillHeader_DetailByRefDocument: Org, Company,
     *  @FinancialYearId, @RefDocumentTypeId (66), @RefDocId (the BLL spells it "@RefDocId " with a
     *  trailing blank; SqlClient sends the name as written, the server binds @RefDocId). */
    public List<Map<String, Object>> wagesDetailByRefDocument(UserAccount u, int financialYearId, int id) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@FinancialYearId", financialYearId);
        p.put("@RefDocumentTypeId", DOC_TYPE_ID);
        p.put("@RefDocId", id);
        return rows("USP_InvContractorWagesBillHeader_DetailByRefDocument", p);
    }

    /** clsGlobalVariables.WagesRefDocumentsStatusList - USP_GetRefDocumentsForWages, @RefDocumentTypeId
     *  omitted (the global list). Load:620 picks the row with RefDocumentTypeId 66. */
    public List<Map<String, Object>> wagesRefDocuments() {
        return safe("wagesRefDocuments", "[dbo].[USP_GetRefDocumentsForWages]", params());
    }

    /** tblUserRights.GetByUserId - Sp_tblUserRights_GetAllMethod @Activity='GetByUserId'. */
    public List<Map<String, Object>> userRights(int userId, String screenName, String roleName, int companyId) {
        Map<String, Object> p = params();
        p.put("@UserId", userId);
        p.put("@ScreenName", screenName);
        p.put("@RightName", roleName == null ? "" : roleName);
        p.put("@CompanyId", companyId);
        p.put("@Activity", "GetByUserId");
        return rows("Sp_tblUserRights_GetAllMethod", p);
    }

    /** clsGlobalVariables.ActiveYr.Start_Period - Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId. */
    public Object financialYearStart(UserAccount u, int financialYearId) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        List<Map<String, Object>> years = safe("financialYear", "Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId", p);
        Map<String, Object> row = null;
        for (Map<String, Object> r : years) if (intOf(r, "Id") == financialYearId) { row = r; break; }
        if (row == null && !years.isEmpty()) row = years.get(0);
        return row == null ? null : ci(row, "Start_Period");
    }

    // ------------------------------------------------------ LoadavailableTransactionsForIssuance

    /** StockComboFill:197 -> StocksReport.Inventory_StockEvalautionDetail_DropDownAndLists (BLL): Org,
     *  Company; @BranchesIds = BranchesId.ToString() (never ""); DocumentTypeIds, ActivityType and
     *  InventoryParentCategory unset and omitted. */
    public List<Map<String, Object>> issuanceDropDowns(UserAccount u) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@BranchesIds", String.valueOf(u.getBranchesId() == null ? 0 : u.getBranchesId()));
        return safe("issuanceDropDowns", "USP_Inventory_StockEvalautionDetail_DropDownAndLists", p);
    }

    /**
     * PendingInventoryTransactionsForIssuanceLoad:319 -> InventoryStockEvalautionDetail
     * .GetAvailableTransactionsForIssuance (BLL): every filter guarded (int != 0, dates by
     * CheckDateTimeNull, strings by IsNullOrEmpty); RefDocumentTypeId travels as
     * @ReferenceDocumentTypeId. JobOrderItems is never set by Stock Conversion, so @ItemIds is omitted.
     */
    public List<Map<String, Object>> availableTransactionsForIssuance(
            UserAccount u, LocalDateTime fromDate, LocalDateTime toDate, int inventoryParentCategories,
            int itemCategoryId, int itemTypeId, int jobLotId, String cropYear, int warehouseId,
            int refDocumentTypeId, int supplierCustomerId, int itemId) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        int branch = u.getBranchesId() == null ? 0 : u.getBranchesId();
        if (branch != 0) p.put("@BranchesId", branch);
        if (fromDate != null) p.put("@DateFrom", fromDate);
        if (toDate != null) p.put("@DateTo", toDate);
        if (supplierCustomerId != 0) p.put("@SupplierCustomerId", supplierCustomerId);
        if (warehouseId != 0) p.put("@WarehouseId", warehouseId);
        if (itemId != 0) p.put("@ItemId", itemId);
        if (refDocumentTypeId != 0) p.put("@ReferenceDocumentTypeId", refDocumentTypeId);
        if (jobLotId != 0) p.put("@JobLotId", jobLotId);
        if (inventoryParentCategories != 0) p.put("@InventoryParentCategories", inventoryParentCategories);
        if (itemCategoryId != 0) p.put("@ItemCategoryId", itemCategoryId);
        if (itemTypeId != 0) p.put("@ItemTypeId", itemTypeId);
        if (set(cropYear)) p.put("@CropYear", cropYear);
        return rows("SpInventoryTransactionEvaluation_GetAvailableTransactionsForIssuance", p);
    }

    // ------------------------------------------ LoadavailableTransactionsForStockReleaseFromFumigation

    /**
     * PendingInventoryTransactionsForIssuanceLoad (fumigation loader :325) -> labIPmActivityLog
     * .HoldStockForFumigation (BLL, IPM): @OrganizationId, @CompanyId always; @BranchesId, @SupplierCustomerId,
     * @WarehouseId, @ItemId, @ReferenceDocumentTypeId, @JobLotId, @InventoryParentCategories,
     * @ItemCategoryId, @ItemTypeId when != 0; @DateFrom / @DateTo by CheckDateTimeNull; @CropYear when
     * not null/empty. RefWarehouseId, PackingTypeId, ItemUomId, WarehouseIds, ItemIds (JobOrderItems is
     * never set by Stock Conversion) and ParentCategoryIds are never set, so never sent.
     * AvailableForFumigation is never set by the caller (0): the form then sets
     * StockReleaseFromFumigation = 1 (sent) and AvailableForFumigation = 0 (guarded out).
     */
    public List<Map<String, Object>> holdStockForFumigation(
            UserAccount u, LocalDateTime fromDate, LocalDateTime toDate, int inventoryParentCategories,
            int itemCategoryId, int itemTypeId, int jobLotId, String cropYear, int warehouseId,
            int refDocumentTypeId, int supplierCustomerId, int itemId) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        int branch = u.getBranchesId() == null ? 0 : u.getBranchesId();
        if (branch != 0) p.put("@BranchesId", branch);
        if (fromDate != null) p.put("@DateFrom", fromDate);
        if (toDate != null) p.put("@DateTo", toDate);
        if (supplierCustomerId != 0) p.put("@SupplierCustomerId", supplierCustomerId);
        if (warehouseId != 0) p.put("@WarehouseId", warehouseId);
        if (itemId != 0) p.put("@ItemId", itemId);
        if (refDocumentTypeId != 0) p.put("@ReferenceDocumentTypeId", refDocumentTypeId);
        if (jobLotId != 0) p.put("@JobLotId", jobLotId);
        if (inventoryParentCategories != 0) p.put("@InventoryParentCategories", inventoryParentCategories);
        if (itemCategoryId != 0) p.put("@ItemCategoryId", itemCategoryId);
        if (itemTypeId != 0) p.put("@ItemTypeId", itemTypeId);
        if (set(cropYear)) p.put("@CropYear", cropYear);
        p.put("@StockReleaseFromFumigation", 1);
        return rows("[dbo].[getHoldStockForFumigation]", p);
    }

    // ------------------------------------------------------------- frmLoadStockShortFallForSales

    /**
     * PendingOrderLoad (frmLoadStockShortFallForSales :224) -> InventoryStockEvalautionDetail
     * .StockConversion_BalanceSalesForStock (BLL): @OrganizationId, @CompanyId always; @DocDate by
     * CheckDateTimeNull; @WarehouseId, @InventoryParentCategoryId, @ItemId, @JobLotId when != 0;
     * @CropYear when != string.Empty. The form also sets FinancialYearId, which the BLL does not send;
     * ItemUomId (@PackUomId) and PackingTypeId are never set by the form.
     */
    public List<Map<String, Object>> balanceSalesForStock(UserAccount u, LocalDateTime docDate, int warehouseId,
                                                          int inventoryParentCategoryId, int itemId, int jobLotId,
                                                          String cropYear) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        if (docDate != null) p.put("@DocDate", docDate);
        if (warehouseId != 0) p.put("@WarehouseId", warehouseId);
        if (inventoryParentCategoryId != 0) p.put("@InventoryParentCategoryId", inventoryParentCategoryId);
        if (itemId != 0) p.put("@ItemId", itemId);
        if (jobLotId != 0) p.put("@JobLotId", jobLotId);
        if (cropYear != null && !cropYear.isEmpty()) p.put("@CropYear", cropYear);
        return rows("[dbo].[USP_StockConversion_BalanceSalesForStock]", p);
    }

    // =================================================================================== write

    /**
     * btnDelete_Click:4967 -> InvPurchaseInvoice.RemoveByID (BLL) -> DAL InvPurchaseInvoice
     * .AccountandInventoryRemoveById: Sp_InvoicesVouchersandStocksDelete @OrganizationId, @CompanyId,
     * @Id, @DocumentTypeId (66), @UserId (= EntryUser = the signed-in user). The procedure's
     * "IF @DocumentTypeId = 66 -- CONVERSION" branch removes the conversion, its stock and voucher.
     */
    @Transactional(rollbackFor = Exception.class)
    public void delete(UserAccount u, int id, int userId) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", u.getOrganizationId());
        p.put("@CompanyId", u.getCompanyId());
        p.put("@Id", id);
        p.put("@DocumentTypeId", DOC_TYPE_ID);
        p.put("@UserId", userId);
        rows("Sp_InvoicesVouchersandStocksDelete", p);
    }

    /** One contractor wages bill (InvContractorWagesBillHeader) and its lines, keyed by model property. */
    public static class WagesBill {
        public Map<String, Object> header = new LinkedHashMap<>();
        public List<Map<String, Object>> lines = new ArrayList<>();
    }

    /** InvStockConversion as the form builds it in Insert():4365, keyed by model property name. */
    public static class SaveModel {
        public Map<String, Object> header = new LinkedHashMap<>();
        public List<Map<String, Object>> details = new ArrayList<>();
        public List<Map<String, Object>> packings = new ArrayList<>();
        public List<Map<String, Object>> expenses = new ArrayList<>();
        public List<WagesBill> wagesBills = new ArrayList<>();
        public String inputDetailRowsRemoveIds;
    }

    /* The exact parameter lists the write procedures declare (procdure.sql), in the model's spelling.
       SetProc sends every non-virtual model property; each one below is a model property the
       procedure declares, and nothing else is sent. */
    private static final String[] W_HEADER = {"Id", "DocTypeId", "DocSrNo", "DocDate", "ProductionNo",
            "DocManualRef", "Remarks", "EntryDate", "EntryUser", "ModifyDate", "ModifyUser", "PostDate",
            "PostUser", "PostState", "OrganizationId", "CompanyId", "BranchedId", "ProjectsId", "ActionId",
            "parentCategoryId", "FinancialYearId", "GainLossId", "EBDepartmentId", "ConversionTypeId",
            "DifferenceAccountId"};
    private static final String[] W_DETAIL = {"Id", "InvStockConversionId", "EntryType", "WarehouseId",
            "ItemId", "ItemUomId", "CropBatch", "JobLotId", "PackingtypeId", "Qty", "PackUnit", "Weight",
            "Rate", "RateUOMId", "Amount", "ProjectId", "VoucherHeadId", "Remarks", "ExpenseAmount",
            "PackingMaterialAmount", "Moisture", "MoistureSlabId", "RefDocumentTypeId", "RefDocNoId",
            "RefDocSubId", "LineId", "WagesAmount", "ItemPmCost", "ItemOhCost", "ItemConditionId", "RackId",
            "SortNo", "labIPmActivityLogId", "IsOnHold"};
    private static final String[] W_PACKING = {"Id", "InvStockConversionId", "ItemId", "ItemSchUOM",
            "ItemQty", "ItemRate", "ItemAmount", "ChargeTo", "WarehouseId", "LineId", "BrandItemId",
            "BrandItemUomId", "ItemConditionId", "ContractScheduleId", "ExImInvoiceId", "RackId"};
    private static final String[] W_EXPENSE = {"Id", "InvStockConversionId", "ChartOfAccountId",
            "LedgerRemarks", "ExpAmount", "ChargeTo", "BrandItemId", "BrandItemUomId"};
    private static final String[] W_WAGES_HEADER = {"Id", "DocumentTypeId", "DocNo", "DocDate",
            "RefDocumentTypeId", "RefDocNoId", "RefDocNo", "WeightTotal", "QtyTotal", "ScaleSlipNo",
            "OtherRemarks", "EntryDate", "EntryUser", "ModifyDate", "ModifyUser", "IsAproved", "ApprovedDate",
            "ApprovedUserId", "OrganizationId", "CompanyId", "BranchesId", "ProjectsId", "RefDocument",
            "FinancialYearId", "JobOrderId", "StockPartyId"};
    private static final String[] W_WAGES_DETAIL = {"Id", "InvContractorWagesBillHeaderId", "ContractorId",
            "ItemId", "Crop", "JobLotId", "InvPackingTypeId", "WbTransactionsIdDt", "InvConractorWagesAccountsId",
            "Weight", "PackSize", "Qty", "WageRate", "WagesAmount", "RemarksDetail", "WareHouseFromId",
            "WareHouseToId", "BillQty", "WeightCut", "BillWeight", "RefDocDate", "WagesTypeId", "FreeOfCost",
            "RefDocumentTypeId", "JobOrderId", "IsCompany", "RateAddLess", "RefDocQty", "RefDocWeight",
            "RefLineId", "InvContractorWagesScheduleId"};

    private static Map<String, Object> pick(Map<String, Object> model, String[] names) {
        Map<String, Object> p = params();
        for (String n : names) p.put("@" + n, model.get(n));
        return p;
    }

    private static Map<String, Object> withAt(Map<String, Object> defaults) {
        Map<String, Object> m = new LinkedHashMap<>();
        for (Map.Entry<String, Object> e : defaults.entrySet())
            m.put(e.getKey().startsWith("@") ? e.getKey() : "@" + e.getKey(), e.getValue());
        return m;
    }

    /**
     * DAL InvStockConversion.SetData (Architecture.DAL.Production, rva 0x3d8f0), step for step, in ONE
     * transaction; BLL InvStockConversion.Save has already set ActionId (1 insert / 2 update) and zeroed
     * ModifyUser (insert) or EntryUser (update). Any RAISERROR rolls the whole document back, as the
     * desktop's SqlTransaction.Rollback does. Returns SetData's num (the conversion Id).
     *
     * Reproduced as the desktop does it, not corrected:
     *   - the detail and PM LineIds are overwritten 1..n here, whatever the grid carried;
     *   - Sp_InventoryStockEvalautionDetail_Update runs for DocTypeId 66 only when ActionId == 1,
     *     i.e. on INSERT (the BLL sets it; the form does not);
     *   - the type-3 difference line is credited with a NEGATIVE amount when outputs exceed inputs,
     *     so USP_VoucherBalanceCheck raises and the save rolls back (user decision: reproduce);
     *   - an empty wages bill (e.g. the "Issue" bill with no lines) is still inserted.
     */
    @Transactional(rollbackFor = Exception.class)
    public int save(SaveModel m) {
        Map<String, Object> h = m.header;
        int org = intOf(h, "OrganizationId");
        int comp = intOf(h, "CompanyId");
        int docTypeId = intOf(h, "DocTypeId");

        /* IL_0041 */
        if (m.details == null || m.details.isEmpty()) throw new IllegalStateException("Detail list not found");

        /* IL_0062 - config ContractWagesChargetoProductForStockConversion; IL_007f - ERP feature 5. */
        boolean contractWages = toBoolNet(configKey(org, comp, "ContractWagesChargetoProductForStockConversion"));
        boolean fifo = erpFeature(org, comp, 5);

        /* IL_0093 - SetProc(header, _Insert / _Update); num > 0 ? obj.Id = num : num = obj.Id. */
        boolean insert = intOf(h, "Id") == 0;
        int id = toInt(scalar(insert ? "Sp_InvStockConversion_Insert" : "Sp_InvStockConversion_Update",
                pick(h, W_HEADER)));
        if (id > 0) h.put("Id", id); else id = intOf(h, "Id");

        /* IL_00c0 - packing rows, LineId 1..n, parent Id stamped, new Id read back. */
        int pmLine = 0;
        for (Map<String, Object> pm : m.packings) {
            pmLine++;
            pm.put("LineId", pmLine);
            pm.put("InvStockConversionId", id);
            pm.put("Id", toInt(scalar("Sp_InvStockConversionPackingMaterial_Insert", pick(pm, W_PACKING))));
        }
        /* IL_0133 - expense rows. */
        for (Map<String, Object> ex : m.expenses) {
            ex.put("InvStockConversionId", id);
            scalar("Sp_InvStockConversionAddExpense_Insert", pick(ex, W_EXPENSE));
        }

        /* IL_018a - the skip-voucher flag (local 10). */
        boolean skipVoucher = toBoolNet(configKey(org, comp, "InventoryFinancialsEffectsInActive"));
        if (!fifo && toBoolNet(configKey(org, comp, "AllowCoaAccountForJobLots"))) {
            /* details.GroupBy(JobLotId).Any(g => g.Select(EntryType).Distinct().Count() > 1) */
            Map<Integer, java.util.Set<String>> byLot = new LinkedHashMap<>();
            for (Map<String, Object> d : m.details)
                byLot.computeIfAbsent(intOf(d, "JobLotId"), k -> new java.util.HashSet<>()).add(str(d.get("EntryType")));
            for (java.util.Set<String> s : byLot.values()) if (s.size() > 1) { skipVoucher = true; break; }
        }

        /* IL_021b - detail rows: LineId = 1..n (overwrites the grid's), parent Id, new Id read back. */
        int line = 1;
        for (Map<String, Object> d : m.details) {
            d.put("LineId", line);
            d.put("InvStockConversionId", id);
            d.put("Id", toInt(scalar("Sp_InvStockConversionDetail_Insert", pick(d, W_DETAIL))));
            line++;
        }

        if (docTypeId == DOC_TYPE_ID) {
            /* IL_029b - only when ActionId == 1 (the BLL's insert marker). */
            if (intOf(h, "ActionId") == 1) {
                Map<String, Object> ev = withAt(InventoryOpeningDefaults.evaluation());
                ev.put("@OrganizationId", org);
                ev.put("@CompanyId", comp);
                ev.put("@RefDocumentTypeId", docTypeId);
                ev.put("@RefDocIdNo", id);
                scalar("Sp_InventoryStockEvalautionDetail_Update", ev);
            }
            /* IL_02f3 */
            Map<String, Object> pe = params();
            pe.put("@OrganizationId", org);
            pe.put("@CompanyId", comp);
            pe.put("@DocumentTypeId", docTypeId);
            pe.put("@Id", id);
            rows("Sp_InventoryStockEvalautionDetail_Insert_InvStockConversionPackingMaterial", pe);
        }

        /* IL_0390 - InventoryTransactions model with Org, Company, RefDocumentTypeId, RefDocIdNo set and
           every other value-type property at its default; the procedure rebuilds the stock rows. */
        Map<String, Object> t = withAt(InventoryOpeningDefaults.transactions());
        t.put("@OrganizationId", org);
        t.put("@CompanyId", comp);
        t.put("@RefDocumentTypeId", docTypeId);
        t.put("@RefDocIdNo", id);
        scalar("Sp_InventoryTransactions_GetALLMethod", t);

        /* DAL 0275 :101 - for 67 / 808 (Store Stock Conversion, screen 502) the evaluation update runs
           after the transactions rebuild, on insert and update alike. */
        if (docTypeId == 67 || docTypeId == 808) {
            Map<String, Object> ev = withAt(InventoryOpeningDefaults.evaluation());
            ev.put("@OrganizationId", org);
            ev.put("@CompanyId", comp);
            ev.put("@RefDocumentTypeId", docTypeId);
            ev.put("@RefDocIdNo", id);
            scalar("Sp_InventoryStockEvalautionDetail_Update", ev);
        }

        if (docTypeId == DOC_TYPE_ID) {
            /* IL_0453 - removed input rows, when the list is non-empty. */
            if (m.inputDetailRowsRemoveIds != null && !m.inputDetailRowsRemoveIds.isEmpty()) {
                Map<String, Object> r = params();
                r.put("@OrganizationId", org);
                r.put("@CompanyId", comp);
                r.put("@DocumentTypeId", docTypeId);
                r.put("@Id", id);
                r.put("@DetailIds", m.inputDetailRowsRemoveIds);
                scalar("[dbo].[USP_InvStockConversionDetailRowsDeleteByIds]", r);
            }
            /* IL_0526 */
            Map<String, Object> v = params();
            v.put("@OrganizationId", org);
            v.put("@CompanyId", comp);
            v.put("@DocumentTypeId", docTypeId);
            v.put("@Id", id);
            rows("usp_StockConversionValidation", v);
        }

        /* IL_05c4 - USP_InventoryValidation per Issue ("Issue" or "1") detail. @ItemConditionId only
           for 67/808; @RackId always. */
        Object docDate = h.get("DocDate");
        for (Map<String, Object> d : m.details) {
            String et = str(d.get("EntryType"));
            if (!"Issue".equals(et) && !"1".equals(et)) continue;
            Map<String, Object> v = params();
            v.put("@OrganizationId", org);
            v.put("@CompanyId", comp);
            v.put("@DocumentTypeId", docTypeId);
            v.put("@DocDate", docDate);
            v.put("@ItemId", d.get("ItemId"));
            v.put("@WarehouseId", d.get("WarehouseId"));
            v.put("@JobLotId", d.get("JobLotId"));
            v.put("@CropYear", d.get("CropBatch"));
            v.put("@InvPackingTypeId", d.get("PackingtypeId"));
            v.put("@PackUomId", d.get("ItemUomId"));
            v.put("@RefDocumentTypeId", d.get("RefDocumentTypeId"));
            v.put("@RefDocNoId", d.get("RefDocNoId"));
            v.put("@RefDocSubIdNo", d.get("RefDocSubId"));
            v.put("@NetWeight", d.get("Weight"));
            if (docTypeId == 67 || docTypeId == 808) v.put("@ItemConditionId", d.get("ItemConditionId"));
            v.put("@RackId", d.get("RackId"));
            rows("USP_InventoryValidation", v);
        }
        /* IL_0843 - and once per PM row with @Activity='PM' (no @DocumentTypeId). */
        for (Map<String, Object> pm : m.packings) {
            Map<String, Object> v = params();
            v.put("@OrganizationId", org);
            v.put("@CompanyId", comp);
            v.put("@DocDate", docDate);
            v.put("@ItemId", pm.get("ItemId"));
            v.put("@WarehouseId", pm.get("WarehouseId"));
            v.put("@NetWeight", pm.get("ItemQty"));
            v.put("@ItemConditionId", pm.get("ItemConditionId"));
            v.put("@RackId", pm.get("RackId"));
            v.put("@Activity", "PM");
            rows("USP_InventoryValidation", v);
        }

        if (skipVoucher) {
            /* IL_18be - no voucher; an existing one is deleted. */
            List<Map<String, Object>> vh = voucherHeadRows(org, comp, docTypeId, id);
            if (!vh.isEmpty()) {
                Map<String, Object> dp = params();
                dp.put("@Id", intOf(vh.get(0), "Id"));
                rows("usp_VoucherDeleteByVoucherId", dp);
            }
        } else {
            makeVoucher(m, h, id, org, comp, docTypeId, contractWages);
        }

        /* IL_194f - wages bills, only with the configuration on. */
        if (contractWages) {
            Map<String, Object> wd = params();
            wd.put("@OrganizationId", org);
            wd.put("@CompanyId", comp);
            wd.put("@RefDocumentTypeId", docTypeId);
            wd.put("@RefDocId", id);
            rows("USP_WagesDeleteByRefDocTypeAndIdForConversion", wd);
            for (WagesBill b : m.wagesBills) {
                b.header.put("RefDocNoId", id);
                wagesSave(b);
            }
        }
        return id;
    }

    /**
     * InvStockConversion.WagesSave -> DAL InvContractorWagesBillHeader.SetData(bill,
     * "Sp_InvContractorWagesBillHeader_Insert", transaction): header, each line, then - because this
     * form never sets VoucherHeadForContractorWages - usp_ContractorWagesVoucherDeleteByWagesId.
     * usp_WagesProportionateToStockEvaluation runs only for RefDocumentTypeId 806/68, never for 66.
     */
    private void wagesSave(WagesBill b) {
        Map<String, Object> h = b.header;
        int billId = toInt(scalar("Sp_InvContractorWagesBillHeader_Insert", pick(h, W_WAGES_HEADER)));
        if (billId > 0) h.put("Id", billId); else billId = intOf(h, "Id");
        for (Map<String, Object> l : b.lines) {
            l.put("InvContractorWagesBillHeaderId", billId);
            scalar("Sp_InvContractorWagesBillDetail_Insert", pick(l, W_WAGES_DETAIL));
        }
        Map<String, Object> p = params();
        p.put("@OrganizationId", h.get("OrganizationId"));
        p.put("@CompanyId", h.get("CompanyId"));
        p.put("@DocumentTypeId", h.get("DocumentTypeId"));
        p.put("@Id", billId);
        rows("usp_ContractorWagesVoucherDeleteByWagesId", p);
    }

    /** CommonServices.GetVoucherHeadId -> Sp_Vouchers_GetMethods
     *  'GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId'. */
    private List<Map<String, Object>> voucherHeadRows(int org, int comp, int docTypeId, int id) {
        Map<String, Object> p = params();
        p.put("@Activity", "GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId");
        p.put("@OrganizationId", org);
        p.put("@CompanyId", comp);
        p.put("@DocumentTypeId", docTypeId);
        p.put("@DocumentTypeSrNo", id);
        return rows("Sp_Vouchers_GetMethods", p);
    }

    /** The voucher SetData composes (IL_09b9 - IL_194e). */
    private void makeVoucher(SaveModel m, Map<String, Object> h, int id, int org, int comp,
                             int docTypeId, boolean contractWages) {
        /* CommonServices.GetItemGlIdsandItemName, read at the top of SetData. */
        Map<String, Object> ip = params();
        ip.put("@OrganizationId", org);
        ip.put("@CompanyId", comp);
        ip.put("@Activity", "GetItemGlIdsandItemName");
        List<Map<String, Object>> itemGl = rows("Sp_Item_GetAllMethod", ip);

        /* IL_09b9 - diff = Sum(Issue.Amount) - Sum(non-Issue.Amount). */
        double sumIssue = 0, sumOut = 0;
        for (Map<String, Object> d : m.details) {
            if ("Issue".equals(str(d.get("EntryType")))) sumIssue += dbl(d.get("Amount"));
            else sumOut += dbl(d.get("Amount"));
        }
        double diff = sumIssue - sumOut;

        Map<String, Object> vh = withAt(InventoryOpeningDefaults.voucher());
        int vhId = 0;
        boolean exists = false;
        List<Map<String, Object>> found = voucherHeadRows(org, comp, docTypeId, id);
        if (!found.isEmpty()) { vhId = intOf(found.get(0), "Id"); exists = true; }
        LocalDateTime now = LocalDateTime.now();
        vh.put("@Id", vhId);
        vh.put("@DocumentTypeId", docTypeId);
        vh.put("@DocumentTypeSrNo", id);
        vh.put("@VoucherCode", h.get("DocSrNo"));
        vh.put("@VoucherDate", h.get("DocDate"));
        vh.put("@RemarksOtherLingo", "");
        vh.put("@FinancialYearId", h.get("FinancialYearId"));
        vh.put("@CheqId", 0);
        vh.put("@ChequeNo", "");
        vh.put("@ChequeDate", LocalDate.now().atStartOfDay());
        vh.put("@PayTitle", "");
        vh.put("@Remarks", "");                 /* set to Remarks first, then overwritten with "" */
        vh.put("@BankBranch", "");
        vh.put("@ChequePrintId", 0);
        vh.put("@Source", "");
        vh.put("@DrCrNoteType", "");
        vh.put("@IsApproved", false);
        vh.put("@EntryDate", now);
        vh.put("@EntryUser", h.get("EntryUser"));
        vh.put("@ModifyUser", h.get("ModifyUser"));
        vh.put("@ModifyDate", now);
        vh.put("@PostDate", now);
        vh.put("@PostUser", 0);
        vh.put("@PostState", false);
        vh.put("@OrganizationId", org);
        vh.put("@CompanyId", comp);
        vh.put("@IncludeWHT", false);
        vh.put("@BranchId", 0);
        vh.put("@ProjectId", 0);
        vh.put("@BillAmount", 0d);
        vh.put("@ManualBillNo", h.get("ProductionNo"));
        vh.put("@DueDate", h.get("DocDate"));
        vh.put("@DueDays", 0);

        Object branch = h.get("BranchedId");
        List<Map<String, Object>> lines = new ArrayList<>();
        if (!m.details.isEmpty()) {
            Map<String, Object> jp = params();
            jp.put("@OrganizationId", org);
            jp.put("@CompanyId", comp);
            jp.put("@Activity", "GetJobLotGlIdsandName");
            List<Map<String, Object>> lotGl = rows("[dbo].[SP_JobLot_ReadMethod]", jp);
            for (Map<String, Object> d : m.details) {
                if (itemGl.isEmpty()) throw new IllegalStateException("Item record not found");
                Map<String, Object> item = null;
                for (Map<String, Object> g : itemGl) if (intOf(g, "Id") == intOf(d, "ItemId")) { item = g; break; }
                if (item == null) continue;
                Map<String, Object> lot = null;
                for (Map<String, Object> g : lotGl) if (intOf(g, "Id") == intOf(d, "JobLotId")) { lot = g; break; }
                int account = (lot != null && intOf(lot, "AccountId") > 0)
                        ? intOf(lot, "AccountId") : intOf(item, "PurchaseGLAC");
                Map<String, Object> vd = withAt(InventoryOpeningDefaults.detail());
                vd.put("@LineId", d.get("LineId"));
                vd.put("@AccountId", account);
                vd.put("@AgainstAccountId", account);
                String et = str(d.get("EntryType"));
                if ("Issue".equals(et) || "1".equals(et)) {
                    String rem = str(d.get("Remarks"));
                    String c = String.format("ItemName: %s, Qty: %s", str(ci(item, "ItemName")), csDouble(dbl(d.get("Qty"))));
                    if (docTypeId == DOC_TYPE_ID) c += ", Weight: " + csDouble(dbl(d.get("Weight")));
                    c += ", Rate: " + csDouble(dbl(d.get("Rate"))) + ", ItemAmount: " + csDouble(dbl(d.get("Amount")));
                    vd.put("@DocumentTypeIdRef", d.get("RefDocumentTypeId"));
                    vd.put("@InvoiceNoRefId", d.get("RefDocNoId"));
                    vd.put("@RefInvoiceNo", String.valueOf(intOf(d, "RefDocSubId")));
                    vd.put("@Comments", (rem.trim().isEmpty() ? "" : rem + ", ") + c);
                    vd.put("@DebitAmount", 0d);
                    vd.put("@CreditAmount", dbl(d.get("Amount")));
                    vd.put("@QtyOut", dbl(d.get("Qty")));
                    vd.put("@WeightOut", dbl(d.get("Weight")));
                } else {
                    vd.put("@Comments", "");
                    vd.put("@CreditAmount", 0d);
                    vd.put("@DebitAmount", dbl(d.get("Amount")));
                    vd.put("@QtyIn", dbl(d.get("Qty")));
                    vd.put("@WeightIn", dbl(d.get("Weight")));
                }
                vd.put("@ItemRate", dbl(d.get("Rate")));
                vd.put("@ItemAmount", dbl(d.get("Amount")));
                vd.put("@ItemId", d.get("ItemId"));
                vd.put("@JobLotId", d.get("JobLotId"));
                vd.put("@IsCGS", 1);
                lines.add(vd);
            }
            /* IL_1122 - the difference line, type 3 only. Debit when diff > 0; when diff < 0 the
               NEGATIVE diff is put in CreditAmount (desktop defect, reproduced). */
            if (diff != 0d && intOf(h, "ConversionTypeId") == 3) {
                Map<String, Object> vd = withAt(InventoryOpeningDefaults.detail());
                int acc = intOf(h, "DifferenceAccountId");
                vd.put("@AccountId", acc);
                vd.put("@AgainstAccountId", acc);
                vd.put("@Comments", "Stock Difference " + csDouble(diff));
                vd.put("@DebitAmount", diff > 0 ? diff : 0d);
                vd.put("@CreditAmount", diff < 0 ? diff : 0d);
                lines.add(vd);
            }
        }
        /* IL_11df - packing materials: credit the item's stock account. */
        for (Map<String, Object> pm : m.packings) {
            if (itemGl.isEmpty()) continue;
            Map<String, Object> item = null;
            for (Map<String, Object> g : itemGl) if (intOf(g, "Id") == intOf(pm, "ItemId")) { item = g; break; }
            if (item == null) continue;
            Map<String, Object> vd = withAt(InventoryOpeningDefaults.detail());
            int acc = intOf(item, "PurchaseGLAC");
            vd.put("@LineId", pm.get("LineId"));
            vd.put("@AccountId", acc);
            vd.put("@AgainstAccountId", acc);
            vd.put("@CreditAmount", dbl(pm.get("ItemAmount")));
            vd.put("@Comments", "ItemName: " + str(ci(item, "ItemName")) + ", "
                    + "Qty: " + csDouble(dbl(pm.get("ItemQty"))) + ", "
                    + "Rate: " + csDouble(dbl(pm.get("ItemRate"))) + ", "
                    + "ItemAmount: " + csDouble(dbl(pm.get("ItemAmount"))));
            vd.put("@DebitAmount", 0d);
            vd.put("@ItemId", pm.get("ItemId"));
            vd.put("@IsCGS", 1);
            lines.add(vd);
        }
        /* IL_1399 - overheads. */
        for (Map<String, Object> ex : m.expenses) {
            Map<String, Object> vd = withAt(InventoryOpeningDefaults.detail());
            vd.put("@AccountId", intOf(ex, "ChartOfAccountId"));
            Object lr = ex.get("LedgerRemarks");
            vd.put("@Comments", !"".equals(lr) ? str(lr) : "Labour Charge to Product");
            vd.put("@CreditAmount", dbl(ex.get("ExpAmount")));
            vd.put("@DebitAmount", 0d);
            lines.add(vd);
        }
        /* IL_147e - wages lines (not free of cost), credited to the contractor's GL account. */
        if (contractWages && m.wagesBills != null && !m.wagesBills.isEmpty()) {
            List<Map<String, Object>> sup = null;
            for (WagesBill b : m.wagesBills) {
                for (Map<String, Object> l : b.lines) {
                    if (bool(l.get("FreeOfCost"))) continue;
                    if (sup == null) {
                        Map<String, Object> sp = params();
                        sp.put("@OrganizationId", org);
                        sp.put("@CompanyId", comp);
                        sp.put("@Activity", "GetGlAccountIdandCompanyNameBySupplierCustomerId");
                        sup = rows("Sp_SupplierCustomer_GetAllMethod", sp);
                    }
                    Map<String, Object> s = null;
                    for (Map<String, Object> g : sup) if (intOf(g, "Id") == intOf(l, "ContractorId")) { s = g; break; }
                    if (s == null) continue;
                    int gl = intOf(s, "GlAccountId");
                    if (gl == 0) throw new IllegalStateException("Contractor GlAccountId Not found");
                    Map<String, Object> vd = withAt(InventoryOpeningDefaults.detail());
                    vd.put("@AccountId", gl);
                    vd.put("@AgainstAccountId", gl);
                    vd.put("@Comments", str(b.header.get("RefDocument")) + "  :" + str(l.get("WagesAccountName"))
                            + " Item Name :" + str(l.get("ItemName"))
                            + "  Wages Qty :  " + csDouble(dbl(l.get("Qty")))
                            + "  Wages Rate :  " + csDouble(dbl(l.get("WageRate")))
                            + "  Wages Amount :  " + csDouble(dbl(l.get("WagesAmount")))
                            + "  BillWeight :  " + csDouble(dbl(l.get("BillWeight"))));
                    vd.put("@CreditAmount", dbl(l.get("WagesAmount")));
                    vd.put("@ItemAmount", dbl(l.get("WagesAmount")));
                    vd.put("@ItemRate", dbl(l.get("WageRate")));
                    vd.put("@OrderNo", b.header.get("JobOrderId"));
                    lines.add(vd);
                }
            }
        }

        /* IL_16fb - head insert, or update when one exists; num > 0 ? Id = num : num = Id. */
        int ret = toInt(scalar(exists ? "Sp_VoucherHead_Update" : "Sp_VoucherHead_Insert", vh));
        if (ret > 0) vhId = ret;
        for (Map<String, Object> vd : lines) {
            int ln = toInt(vd.get("@LineId"));
            int itemId = toInt(vd.get("@ItemId"));
            /* b__11 then b__12: the detail, then the PM row, whose LineId and ItemId match (LineId > 0);
               a PM match overwrites a detail match. */
            for (Map<String, Object> d : m.details)
                if (intOf(d, "LineId") == ln && intOf(d, "ItemId") == itemId && ln > 0) { vd.put("@RefDocSubIdNo", d.get("Id")); break; }
            for (Map<String, Object> pm : m.packings)
                if (intOf(pm, "LineId") == ln && intOf(pm, "ItemId") == itemId && ln > 0) { vd.put("@RefDocSubIdNo", pm.get("Id")); break; }
            vd.put("@VoucherHeadId", vhId);
            vd.put("@BranchesId", branch);
            scalar("Sp_VoucherDetail_Insert", vd);
        }
        if (vhId > 0) {
            Map<String, Object> bc = params();
            bc.put("@OrganizationId", org);
            bc.put("@CompanyId", comp);
            bc.put("@Id", vhId);
            scalar("USP_VoucherBalanceCheck", bc);
        }
    }

    /** GetConfigurationFromAllocation (DAL): the ConfigKey of the row, or "" when there is none. */
    private String configKey(int org, int comp, String name) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", org);
        p.put("@CompanyId", comp);
        p.put("@ConfigDescription", name);
        p.put("@Activity", "GetConfigurationByOrgCompandConfigDescription");
        List<Map<String, Object>> r = rows(P_CONFIG, p);
        if (r.isEmpty()) return "";
        Object v = ci(r.get(0), "ConfigKey");
        return v == null ? "" : String.valueOf(v);
    }

    /** GetERPFeaturesByCompanyId(org, comp, featureId): is the feature among USP_GetERPFeaturesByCompanyId's rows. */
    private boolean erpFeature(int org, int comp, int featureId) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", org);
        p.put("@CompanyId", comp);
        for (Map<String, Object> r : rows("USP_GetERPFeaturesByCompanyId", p))
            if (intOf(r, "Id") == featureId) return true;
        return false;
    }

    /** Conversion.ToBool(string): Convert.ToBoolean ("True"/"False"), else Convert.ToInt32 != 0, else false. */
    public static boolean toBoolNet(String s) {
        if (s == null) return false;
        String t = s.trim();
        if ("true".equalsIgnoreCase(t)) return true;
        if ("false".equalsIgnoreCase(t)) return false;
        try { return Integer.parseInt(t) != 0; } catch (NumberFormatException e) { return false; }
    }

    /** C# double.ToString() on .NET Framework: "G" with 15 significant digits. */
    public static String csDouble(double v) {
        if (Double.isNaN(v)) return "NaN";
        if (Double.isInfinite(v)) return v > 0 ? "Infinity" : "-Infinity";
        if (v == 0) return "0";
        BigDecimal bd = new BigDecimal(v).round(new MathContext(15)).stripTrailingZeros();
        return bd.toPlainString();
    }

    public static Object ci(Map<String, Object> row, String name) {
        if (row == null) return null;
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet()) if (e.getKey().equalsIgnoreCase(name)) return e.getValue();
        return null;
    }

    private static String str(Object o) { return o == null ? "" : String.valueOf(o); }

    public static double dbl(Object o) {
        if (o == null) return 0d;
        if (o instanceof Number) return ((Number) o).doubleValue();
        try { return Double.parseDouble(String.valueOf(o).replace(",", "").trim()); }
        catch (NumberFormatException e) { return 0d; }
    }

    private static boolean bool(Object o) {
        if (o == null) return false;
        if (o instanceof Boolean) return (Boolean) o;
        if (o instanceof Number) return ((Number) o).intValue() != 0;
        return toBoolNet(String.valueOf(o));
    }

    private static int toInt(Object v) {
        if (v == null) return 0;
        if (v instanceof Number) return ((Number) v).intValue();
        try { return (int) Math.round(Double.parseDouble(String.valueOf(v).trim())); }
        catch (NumberFormatException e) { return 0; }
    }

    private static int firstInt(Map<String, Object> row) {
        if (row == null || row.isEmpty()) return 0;
        Object v = row.values().iterator().next();
        if (v instanceof Number) return ((Number) v).intValue();
        try { return Integer.parseInt(String.valueOf(v).trim()); }
        catch (NumberFormatException e) { return 0; }
    }

    /**
     * cmbEntryType hides "Issue" when the IssuanceByLoader configuration is on.
     *
     * invfrmStockConversionProduction.CmbEntryTypeFill():840 reads
     * clsGlobalVariables.configrationsAllocation - the per-org/company list loaded at login by
     * ConfigrationsAllocation.History(OrganizationId, CompanyId) - and adds row 1 only when the
     * matching ConfigKey parses as false. This reads that same list one row at a time through
     * @Activity='GetConfigurationByOrgCompandConfigDescription', the pattern already used by
     * ProductionJobOrderMainRepository.config.
     *
     * NO LONGER USED by entryTypes(). It folds a missing row into "false", but the desktop's
     * guard (list.Count > 0 && !bool.Parse(key)) does NOT add "Issue" when the row is missing.
     * StockConversionService.entryTypes now reads StockConversionLookupsRepository.configKey,
     * which keeps "absent" distinct. Kept only so nothing else that may call it breaks.
     */
    public boolean config(Integer organizationId, Integer companyId, String name) {
        List<Map<String, Object>> rows = jdbc.queryForList(
                "EXEC dbo." + P_CONFIG + " @OrganizationId=?, @CompanyId=?, @ConfigDescription=?, @Activity=?",
                organizationId, companyId, name, "GetConfigurationByOrgCompandConfigDescription");
        if (rows.isEmpty()) return false;
        Object v = rows.get(0).get("ConfigKey");
        if (v == null) return false;
        String t = String.valueOf(v).trim();
        return "1".equals(t) || "true".equalsIgnoreCase(t) || "yes".equalsIgnoreCase(t);
    }
}
