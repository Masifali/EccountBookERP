package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.models.dto.StockConversionDto;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

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
 * READ IS COMPLETE. WRITE IS NOT, AND SAYS SO.
 * ---------------------------------------------------------------------------------------------
 * The desktop's SetData is a posting engine: besides the header and its three child tables it
 * writes inventory stock evaluation, inventory transactions, an accounting voucher with a balance
 * check, and contractor wages bills, behind four guard procedures. Three pieces of that cannot be
 * reproduced from the call sites alone — the InventoryTransactions model's parameter set, the
 * VoucherHead/VoucherDetail the desktop composes, and the wages bill — so {@link #save} is not
 * implemented here. Writing the header and details WITHOUT the rest would leave a conversion with
 * no stock movement and no voucher: worse than refusing.
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

    // =================================================================================== write

    /**
     * Not implemented, deliberately.
     *
     * DAL 0275 SetData writes, in ONE transaction: the header, packing materials, expenses and
     * details; inventory stock evaluation; <b>inventory transactions</b>; an accounting
     * <b>voucher head and details</b> followed by USP_VoucherBalanceCheck; and <b>contractor
     * wages bills</b> — behind usp_StockConversionValidation and USP_InventoryValidation.
     *
     * Three of those cannot be reproduced from the desktop call sites alone:
     *
     *   1. Sp_InventoryTransactions_GetALLMethod is invoked through SetProc, so its parameters are
     *      the InventoryTransactions MODEL's property names - and despite the GetALLMethod name it
     *      is the write path, chosen by an @Activity the model carries.
     *   2. The VoucherHead / VoucherDetail rows the desktop composes (accounts, amounts, sign).
     *   3. The contractor wages bill header and its details.
     *
     * Writing the header and its three child tables without those would leave a conversion with no
     * stock movement and no voucher - a document that looks saved and is not. Refusing is the
     * safer failure, and it is loud.
     */
    public int save(StockConversionDto dto) {
        throw new UnsupportedOperationException(
                "Stock Conversion Save is not implemented yet. Its desktop Save also writes "
              + "inventory transactions, an accounting voucher and contractor wages bills; those "
              + "three need Sp_InventoryTransactions_GetALLMethod, Sp_VoucherHead_Insert/"
              + "Sp_VoucherDetail_Insert and the wages bill traced before anything may post.");
    }

    private static int firstInt(Map<String, Object> row) {
        if (row == null || row.isEmpty()) return 0;
        Object v = row.values().iterator().next();
        if (v instanceof Number) return ((Number) v).intValue();
        try { return Integer.parseInt(String.valueOf(v).trim()); }
        catch (NumberFormatException e) { return 0; }
    }
}
