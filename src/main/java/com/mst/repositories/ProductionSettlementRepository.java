package com.mst.repositories;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.ConnectionCallback;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.math.BigDecimal;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.ResultSetMetaData;
import java.sql.SQLException;
import java.sql.Timestamp;
import java.sql.Types;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Screen 280, Settlement tab - Architecture.WinApp.Production/frmProductionSettlement.cs (3,962 lines).
 *
 * Every read and write the form performs, each traced form line -> BLL -> procedure. The BLL's
 * parameter guards are reproduced literally: a parameter the BLL adds only when non-zero / non-empty
 * is OMITTED here in the same case, never sent as NULL or 0.
 *
 * WRITES follow the DAL, not a re-design:
 *
 *  - InvFoodProduction.UpdateSettlement  (BLL :SetDataForUpdate, "SP_InvFoodProductionDetail_Update")
 *      one SqlTransaction:  InvProductionJobOrder_UpdateIsSettled -> usp_SettlementDeleteByJobOrderId
 *      -> details ordered by InvFoodProductionId, DeleteFlag = 1 on the first detail of each
 *      production header (0 otherwise) -> SP_InvFoodProductionDetail_Update per detail -> commit.
 *  - InvFoodProduction.InPutUpdateSettlement ("SP_InvFoodProductionDetail_InPut_Update")
 *      the same loop without the two leading procedures.
 *  - InvFoodProduction.SetDataForSettlementFinancials (Sp_VoucherHead_Insert when Id == 0, else
 *      Sp_VoucherHead_Update) -> Sp_VoucherDetail_Insert per line -> USP_VoucherBalanceCheck ->
 *      InvProductionJobOrder_UpdateIsSettled(@InvJobOrderId = voucherDetailList[0].OrderNo).
 *      "Voucher Detail Not Found" when there are no lines.
 *
 * GenericProvider.SetProc sends every non-virtual model property; a .NET null is "not supplied"
 * (the procedure default applies) and a value type is always sent, so an unassigned int/double is
 * sent as 0 and an unassigned bool as false. {@link #detailParams}, {@link #voucherHeadDefaults} and
 * {@link #voucherDetailDefaults} carry those CLR defaults; only names the procedure declares are
 * ever sent (checked against procdure.utf8.sql).
 *
 * Every EXEC goes through {@link #walk}, which drains every result/update count the way
 * ExecuteScalar/ExecuteNonQuery do, so a procedure that does or does not SELECT behaves the same,
 * and a RAISERROR anywhere in the batch surfaces as an exception.
 */
@Repository
public class ProductionSettlementRepository {

    /** CommonServices.SetRightsValueInRightsObject(base.Name) - base.Name of this form. */
    public static final String DESKTOP_SCREEN_NAME = "frmProductionSettlement";

    /** SettlementFinancials: obj.DocumentTypeId = 142; VoucherHeadIdGet(JobOrderId, 142). */
    public static final int SETTLEMENT_DOCUMENT_TYPE_ID = 142;

    @Autowired private JdbcTemplate jdbc;

    /* ============================================================================ switches */

    /**
     * GlobalVariables_Helper.GetConfigValueFromGlobal(name) - the login-time configuration list,
     * read here one row at a time exactly as the shell does
     * (Sp_ConfigrationsAllocation_GetAllMethod @Activity='GetConfigurationByOrgCompandConfigDescription').
     */
    public String configValue(int organizationId, int companyId, String configDescription) {
        List<Map<String, Object>> rows = rows("dbo.Sp_ConfigrationsAllocation_GetAllMethod",
                p("@OrganizationId", organizationId, "@CompanyId", companyId,
                  "@ConfigDescription", configDescription,
                  "@Activity", "GetConfigurationByOrgCompandConfigDescription"));
        if (rows.isEmpty()) return "";
        Object v = col(rows.get(0), "ConfigKey");
        return v == null ? "" : String.valueOf(v).trim();
    }

    /** CommonServices.GetERPFeatureById(5) - ErpFeaturesList is USP_GetERPFeaturesByCompanyId. */
    public boolean erpFeature(int organizationId, int companyId, int featureId) {
        for (Map<String, Object> r : rows("dbo.USP_GetERPFeaturesByCompanyId",
                p("@OrganizationId", organizationId, "@CompanyId", companyId))) {
            Object id = col(r, "Id");
            if (id instanceof Number && ((Number) id).intValue() == featureId) return true;
        }
        return false;
    }

    /** The grant rows behind SetRightsValueInRightsObject (Sp_tblUserRights_GetAllMethod GetByUserId). */
    public List<Map<String, Object>> userRightsForScreen(int userId, String roleName, int companyId) {
        return rows("dbo.Sp_tblUserRights_GetAllMethod",
                p("@UserId", userId, "@ScreenName", DESKTOP_SCREEN_NAME,
                  "@RightName", roleName == null ? "" : roleName,
                  "@CompanyId", companyId, "@Activity", "GetByUserId"));
    }

    /* ============================================================================= pickers */

    /**
     * stockAccoutnFill:300 -> CommonServices.CoaAllocationAccountTitleByAccountTypeIds("4,12")
     * -> COAAllocation.GetAccountTitleByAccountTypeIds -> Sp_COAAllocation_GetAllMethod.
     *
     * BLL guards: @OrganizationId, @CompanyId, @AppId always; @AccountTypeIds when not null and not
     * ""; @AccountTypeIdsNot likewise (null here - omitted); @UserId when != 0; @CostCenterId,
     * @NotReferred, @RecId when != 0 (all 0 here - omitted); @AccountClassIds / @AccountClassIdsNot
     * when not null and not "" (CommonServices passes "" - omitted). @Activity last.
     */
    public List<Map<String, Object>> accountTitlesByAccountTypeIds(int organizationId, int companyId,
                                                                   int appId, int userId,
                                                                   String accountTypeIds) {
        LinkedHashMap<String, Object> m = p("@OrganizationId", organizationId, "@CompanyId", companyId,
                                            "@AppId", appId);
        if (accountTypeIds != null && !accountTypeIds.isEmpty()) m.put("@AccountTypeIds", accountTypeIds);
        if (userId != 0) m.put("@UserId", userId);
        m.put("@Activity", "GetAccountTitleByAccountTypeIds");
        return rows("dbo.Sp_COAAllocation_GetAllMethod", m);
    }

    /**
     * JobOrderNoFillForSettlement(ActionId):317 -> InvProductionJobOrder.GetJobOrderAll
     * -> Sp_InvProductionJobOrder_GetAllMethod @Activity='GetJobOrderNoAll'.
     *
     * BLL guards: @FinancialYearId when != 0; @DocumentTypeId when != 0 (never set here - omitted);
     * @ActionId when != 0. So "All Job Orders" (ActionId 0) OMITS @ActionId, and the procedure's
     * `(@ActionId is null or IsApproved=0)` then lets approved job orders through.
     */
    public List<Map<String, Object>> jobOrdersAll(int organizationId, int companyId,
                                                  int financialYearId, int actionId) {
        LinkedHashMap<String, Object> m = p("@OrganizationId", organizationId, "@CompanyId", companyId);
        if (financialYearId != 0) m.put("@FinancialYearId", financialYearId);
        if (actionId != 0) m.put("@ActionId", actionId);
        m.put("@Activity", "GetJobOrderNoAll");
        return rows("dbo.Sp_InvProductionJobOrder_GetAllMethod", m);
    }

    /**
     * btnGeneralSettlement_Click:376 -> InvFoodProductionReports.InvFoodProductionSettlement
     * -> Sp_InvFoodProductionSettlement (four parameters, none guarded).
     * Activities used: 'InPutOutPut', 'OverHeads', 'PackingMaterial'. Note 'InPutOutPut' itself
     * EXECs Sp_InvFoodProductionSettlement_Update before selecting - the desktop's Generate writes.
     */
    public List<Map<String, Object>> settlementData(int organizationId, int companyId,
                                                    int jobOrderId, String activity) {
        return rows("dbo.Sp_InvFoodProductionSettlement",
                p("@OrganizationId", organizationId, "@CompanyId", companyId,
                  "@JobOrderId", jobOrderId, "@Activity", activity));
    }

    /**
     * ProporationFinishGoodsSettlement:1210 -> CommonServices.GetUomScheduleByItemId(ItemId)
     * -> UOMSchedule.SearchByObject -> Sp_UOMSchedule_GetAllMethod @Activity='ReadByItemID'
     * (Org, Company, ItemId, Activity - unguarded). CommonServices then keeps five columns.
     */
    public List<Map<String, Object>> uomScheduleByItemId(int organizationId, int companyId, int itemId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows("dbo.Sp_UOMSchedule_GetAllMethod",
                p("@OrganizationId", organizationId, "@CompanyId", companyId,
                  "@ItemId", itemId, "@Activity", "ReadByItemID"))) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", col(r, "Id"));
            m.put("UOMCode", col(r, "UOMCode"));
            m.put("Equivalent", col(r, "Equivalent"));
            m.put("QtyEquivalent", col(r, "QtyEquivalent"));
            m.put("BaseRateUom", col(r, "BaseRateUom"));
            out.add(m);
        }
        return out;
    }

    /**
     * SettlementFinancials:1536 -> InvProductionJobOrder.GetGlAccountsByJobOrderId
     * -> Sp_InvProductionJobOrder_GetAllMethod @OrganizationId, @CompanyId, @Id,
     * @Activity='GetGlAccountsByJobOrderId' (all unconditional).
     */
    public List<Map<String, Object>> glAccountsByJobOrderId(int organizationId, int companyId, int jobOrderId) {
        return rows("dbo.Sp_InvProductionJobOrder_GetAllMethod",
                p("@OrganizationId", organizationId, "@CompanyId", companyId, "@Id", jobOrderId,
                  "@Activity", "GetGlAccountsByJobOrderId"));
    }

    /**
     * CommonServices.VoucherHeadIdGet(Id, 142) -> VoucherHead.GetVoucherHeadIdByDocumentTypeIdandRefDocNoId
     * -> Sp_Vouchers_GetMethods @Activity='GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId',
     * the document id travelling as @DocumentTypeSrNo. Rows[0]["Id"] when there is a row, else 0.
     */
    public int voucherHeadId(int organizationId, int companyId, int documentTypeId, int documentTypeSrNo) {
        List<Map<String, Object>> r = rows("dbo.Sp_Vouchers_GetMethods",
                p("@Activity", "GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId",
                  "@OrganizationId", organizationId, "@CompanyId", companyId,
                  "@DocumentTypeId", documentTypeId, "@DocumentTypeSrNo", documentTypeSrNo));
        if (r.isEmpty()) return 0;
        return toInt(col(r.get(0), "Id"));
    }

    /** InvFoodProduction.ProductionSettlementWeightValidations -> usp_ProductionSettlementWeightValidations. */
    public List<Map<String, Object>> weightValidations(int organizationId, int companyId, int jobOrderId) {
        return rows("dbo.usp_ProductionSettlementWeightValidations",
                p("@OrganizationId", organizationId, "@CompanyId", companyId, "@JobOrderId", jobOrderId));
    }

    /** InvProductionJobOrder.CheckJobOrderOrderIsSpecialApproved (@Id, activity of that name). */
    public List<Map<String, Object>> specialApproval(int organizationId, int companyId, int jobOrderId) {
        return rows("dbo.Sp_InvProductionJobOrder_GetAllMethod",
                p("@OrganizationId", organizationId, "@CompanyId", companyId, "@Id", jobOrderId,
                  "@Activity", "CheckJobOrderOrderIsSpecialApproved"));
    }

    /** InvProductionJobOrder.RequestFromSettlementForJobOrderForSpecialApproval (five params + activity). */
    public void requestSpecialApproval(int organizationId, int companyId, int jobOrderId,
                                       int entryUser, String remarks) {
        exec("dbo.Sp_InvProductionJobOrder_GetAllMethod",
                p("@OrganizationId", organizationId, "@CompanyId", companyId, "@Id", jobOrderId,
                  "@EntryUser", entryUser, "@Remarks", remarks,
                  "@Activity", "RequestFromSettlementForJobOrderForSpecialApproval"));
    }

    /**
     * btnApproveSettlement_Click:1952 -> InvProductionJobOrder.JobOrderApproveList, one item:
     * @OrganizationId, @CompanyId, @Id, @PendingForView (= ActionTypeId, a bool never set: false),
     * @EntryUser, @ReqType (never set: a null SqlParameter value is "not supplied" -> omitted),
     * @Activity='JobOrderApprove'.
     */
    public void jobOrderApprove(int organizationId, int companyId, int jobOrderId, int entryUser) {
        exec("dbo.Sp_InvProductionJobOrder_GetAllMethod",
                p("@OrganizationId", organizationId, "@CompanyId", companyId, "@Id", jobOrderId,
                  "@PendingForView", Boolean.FALSE, "@EntryUser", entryUser,
                  "@Activity", "JobOrderApprove"));
    }

    /** Any report data source, parameters already guarded by the caller. */
    public List<Map<String, Object>> reportRows(String procedure, LinkedHashMap<String, Object> params) {
        return rows(procedure, params);
    }

    /* ============================================================================== writes */

    /** SP_InvFoodProductionDetail_Update / _InPut_Update parameters the model supplies, CLR defaults. */
    public static LinkedHashMap<String, Object> detailParams() {
        LinkedHashMap<String, Object> m = new LinkedHashMap<>();
        m.put("@Id", 0);
        m.put("@InvFoodProductionId", 0);
        m.put("@RefDocumentTypeId", 0);
        m.put("@RefDocNoId", 0);
        m.put("@InvProductionJobOrderId", 0);
        m.put("@InvProductionJobOrderNo", 0);
        m.put("@EntryType", null);
        m.put("@WarehouseId", 0);
        m.put("@ItemId", 0);
        m.put("@ItemUomId", 0);
        m.put("@CropBatch", null);
        m.put("@JobLotId", 0);
        m.put("@PackingtypeId", 0);
        m.put("@Qty", 0d);
        m.put("@PackUnit", 0);
        m.put("@Weight", 0d);
        m.put("@Rate", 0d);
        m.put("@RateUOMId", 0);
        m.put("@Amount", 0d);
        m.put("@ItemPmCost", 0d);
        m.put("@GeneralPmCost", 0d);
        m.put("@ItemOhCost", 0d);
        m.put("@GeneralOhCost", 0d);
        m.put("@NetRate", 0d);
        m.put("@TotalAmount", 0d);
        m.put("@Remarks", null);
        m.put("@VoucherHeadId", 0);
        m.put("@StockAcId", 0);
        m.put("@RefDocSubIdNo", 0);
        m.put("@JobOrderInPutId", 0);
        m.put("@OutPutIdFromJobOrder", 0);
        m.put("@DeleteFlag", null);          /* Nullable - set per row by the DAL loop */
        m.put("@LineId", 0);
        m.put("@JobOrderScheduleId", 0);
        m.put("@GrossWeight", 0d);
        m.put("@EbUnit", 0d);
        m.put("@EbTotal", 0d);
        m.put("@MoveOrderDocumentTypeId", 0);
        m.put("@MoveOrderDocId", 0);
        return m;
    }

    /**
     * DAL InvFoodProduction.SetDataForUpdate - one transaction.
     * @return the last detail call's scalar (the DAL's loc.0), 0 when there were no details.
     */
    public int updateSettlement(int organizationId, int companyId, int invJobOrderId,
                                List<LinkedHashMap<String, Object>> details) {
        return jdbc.execute((ConnectionCallback<Integer>) con -> inTransaction(con, () -> {
            LinkedHashMap<String, Object> head = p("@OrganizationId", organizationId,
                    "@CompanyId", companyId, "@InvJobOrderId", invJobOrderId);
            walk(con, "[dbo].[InvProductionJobOrder_UpdateIsSettled]", head);
            walk(con, "[dbo].[usp_SettlementDeleteByJobOrderId]", head);
            return detailLoop(con, "dbo.SP_InvFoodProductionDetail_Update", details);
        }));
    }

    /** DAL InvFoodProduction.SetDataForINPutUpdate - the detail loop alone, one transaction. */
    public int inputUpdateSettlement(List<LinkedHashMap<String, Object>> details) {
        return jdbc.execute((ConnectionCallback<Integer>) con -> inTransaction(con,
                () -> detailLoop(con, "dbo.SP_InvFoodProductionDetail_InPut_Update", details)));
    }

    /**
     * details.OrderBy(x => x.InvFoodProductionId).ToList(); prev = 0;
     * DeleteFlag = (InvFoodProductionId != prev) ? 1 : 0; call; prev = InvFoodProductionId.
     * (List.sort is stable, as LINQ OrderBy is.)
     */
    private Integer detailLoop(Connection con, String proc, List<LinkedHashMap<String, Object>> details)
            throws SQLException {
        List<LinkedHashMap<String, Object>> sorted = new ArrayList<>(details);
        sorted.sort((a, b) -> Integer.compare(toInt(a.get("@InvFoodProductionId")),
                                              toInt(b.get("@InvFoodProductionId"))));
        int prev = 0;
        int last = 0;
        for (LinkedHashMap<String, Object> d : sorted) {
            int headerId = toInt(d.get("@InvFoodProductionId"));
            d.put("@DeleteFlag", headerId != prev ? 1 : 0);
            Object r = walk(con, proc, d);
            last = toInt(r);
            prev = headerId;
        }
        return last;
    }

    /** VoucherHead CLR value-type defaults (non-virtual properties the head procedures declare). */
    public static LinkedHashMap<String, Object> voucherHeadDefaults() {
        LinkedHashMap<String, Object> m = new LinkedHashMap<>();
        m.put("@Id", 0);
        m.put("@DocumentTypeId", 0);
        m.put("@DocumentTypeSrNo", 0);
        m.put("@RefDocNoId", 0);
        m.put("@VoucherCode", 0);
        m.put("@VoucherDate", null);
        m.put("@Remarks", null);
        m.put("@VoucherAmount", 0d);
        m.put("@FinancialYearId", 0);
        m.put("@RefAccountId", 0);
        m.put("@AgainstAccountId", 0);
        m.put("@MultiCurrencyId", 0);
        m.put("@ExchangeCurrencyRate", 0d);
        m.put("@FcAmount", 0d);
        m.put("@CheqId", 0);
        m.put("@ChequePrintId", 0);
        m.put("@IsApproved", Boolean.FALSE);
        m.put("@EntryDate", null);
        m.put("@EntryUser", 0);
        m.put("@ModifyDate", null);
        m.put("@ModifyUser", 0);
        m.put("@PostUser", 0);
        m.put("@PostState", Boolean.FALSE);
        m.put("@OrganizationId", 0);
        m.put("@CompanyId", 0);
        m.put("@IncludeWHT", Boolean.FALSE);
        m.put("@BranchId", 0);
        m.put("@ProjectId", 0);
        m.put("@ManualBillNo", null);
        m.put("@BillAmount", 0d);
        m.put("@DueDays", 0);
        m.put("@ActionId", 0);
        m.put("@CostCenterAmount", 0d);
        m.put("@RefDocumentTypeId", 0);
        m.put("@CustomAccounts", Boolean.FALSE);
        m.put("@BaseDocumentTypeId", 0);
        m.put("@AdvanceTaxAccountId", 0);
        m.put("@AdvanceTaxAmount", 0d);
        m.put("@OtherChargesAccountId", 0);
        m.put("@OtherChargesAmount", 0d);
        m.put("@IsUploaded", Boolean.FALSE);
        m.put("@FixedAssetEntryTypeId", 0);
        return m;
    }

    /** VoucherDetail CLR value-type defaults (non-virtual properties Sp_VoucherDetail_Insert declares). */
    public static LinkedHashMap<String, Object> voucherDetailDefaults() {
        LinkedHashMap<String, Object> m = new LinkedHashMap<>();
        String[] ints = { "@Id", "@VoucherHeadId", "@AccountId", "@AgainstAccountId", "@JobLotId",
                "@TaxTypeId", "@DocumentTypeIdRef", "@InvoiceNoRefId", "@ItemId", "@OrderNo", "@GpNo",
                "@SupplierCustomerId", "@DMultiCurrencyId", "@ActionId", "@LineId", "@RefDocumentTypeId",
                "@RefDocNoId", "@RefDocNoDetailId", "@RefDocSubIdNo", "@SubNo", "@SubsidiaryTypeId",
                "@EmployeeId", "@SubsidiaryAccountId", "@IsCGS", "@SubsidiaryAgainstTypeId",
                "@SubsidiaryAgainstAccountId", "@ThirdCurrencyId", "@SortNo", "@InstrumentTypeId",
                "@ChequeTypeId", "@BranchesId", "@CostCenterId", "@ReferenceAccountId",
                "@LocationTypeId", "@PaymentTypeId", "@BaseFcyId" };
        String[] dbls = { "@DebitAmount", "@CreditAmount", "@TaxesTotalAmount", "@TaxPrcnt", "@QtyIn",
                "@QtyOut", "@WeightIn", "@WeightOut", "@ItemRate", "@RateCut", "@RateCutAmount",
                "@ItemAmount", "@Expenses", "@Freight", "@Journal", "@Commission",
                "@DExchangeCurrencyRate", "@DCurrencyAmount", "@AdvanceAmount", "@WhtHolding",
                "@SaleTax", "@ExTax", "@Adjustment", "@ItemCgsRate", "@TotalCreditAmount",
                "@TotalDebitAmount", "@ThirdCurrencyFcyExchangeRate", "@ThirdCurrencyHcyExchangeRate",
                "@ThirdCurrencyAmount", "@ThirdCurrencyReceiverExchangeRate",
                "@ThirdCurrencyReceiverFcyAmount", "@SBRTaxAmount", "@DiscountPercent",
                "@DiscountAmount", "@TaxAmount", "@BaseFcyExchangeRate", "@BaseFcyAmount" };
        for (String s : ints) m.put(s, 0);
        for (String s : dbls) m.put(s, 0d);
        m.put("@Comments", null);
        return m;
    }

    /**
     * DAL InvFoodProduction.SetDataForSettlementFinancials - one transaction.
     * @return the head procedure's scalar when > 0 (and it becomes the head Id), else head.Id.
     */
    public int settlementFinancials(LinkedHashMap<String, Object> head,
                                    List<LinkedHashMap<String, Object>> lines) {
        return jdbc.execute((ConnectionCallback<Integer>) con -> inTransaction(con, () -> {
            int headId = toInt(head.get("@Id"));
            String proc = headId == 0 ? "dbo.Sp_VoucherHead_Insert" : "dbo.Sp_VoucherHead_Update";
            int result = toInt(walk(con, proc, head));
            if (result > 0) {
                headId = result;
                head.put("@Id", result);
            } else {
                result = headId;
            }
            if (lines == null || lines.isEmpty()) {
                throw new IllegalStateException("Voucher Detail Not Found");
            }
            for (LinkedHashMap<String, Object> l : lines) {
                l.put("@VoucherHeadId", headId);
                walk(con, "dbo.Sp_VoucherDetail_Insert", l);
            }
            walk(con, "dbo.USP_VoucherBalanceCheck",
                    p("@OrganizationId", head.get("@OrganizationId"), "@CompanyId", head.get("@CompanyId"),
                      "@Id", headId));
            walk(con, "[dbo].[InvProductionJobOrder_UpdateIsSettled]",
                    p("@OrganizationId", head.get("@OrganizationId"), "@CompanyId", head.get("@CompanyId"),
                      "@InvJobOrderId", lines.get(0).get("@OrderNo")));
            return result;
        }));
    }

    /* ============================================================================= plumbing */

    private interface Work { Integer run() throws SQLException; }

    private static Integer inTransaction(Connection con, Work w) throws SQLException {
        boolean auto = con.getAutoCommit();
        con.setAutoCommit(false);
        try {
            Integer r = w.run();
            con.commit();
            return r;
        } catch (SQLException | RuntimeException e) {
            try { con.rollback(); } catch (SQLException ignored) { /* the original error matters */ }
            throw e;
        } finally {
            try { con.setAutoCommit(auto); } catch (SQLException ignored) { }
        }
    }

    /** EXEC with no transaction of its own (a single call is atomic on the server). */
    private void exec(String proc, LinkedHashMap<String, Object> params) {
        jdbc.execute((ConnectionCallback<Object>) con -> walk(con, proc, params));
    }

    /** The first result set as rows (DataTable fill), every later result drained. */
    private List<Map<String, Object>> rows(String proc, LinkedHashMap<String, Object> params) {
        return jdbc.execute((ConnectionCallback<List<Map<String, Object>>>) con -> {
            List<Map<String, Object>> out = new ArrayList<>();
            run(con, proc, params, out);
            return out;
        });
    }

    /** ExecuteScalar semantics: first column of the first row of the first result set, or null. */
    private static Object walk(Connection con, String proc, LinkedHashMap<String, Object> params)
            throws SQLException {
        List<Map<String, Object>> out = new ArrayList<>();
        run(con, proc, params, out);
        if (out.isEmpty()) return null;
        Map<String, Object> first = out.get(0);
        return first.isEmpty() ? null : first.values().iterator().next();
    }

    private static void run(Connection con, String proc, LinkedHashMap<String, Object> params,
                            List<Map<String, Object>> firstResult) throws SQLException {
        StringBuilder sql = new StringBuilder("EXEC ").append(proc);
        List<Object> values = new ArrayList<>();
        boolean first = true;
        for (Map.Entry<String, Object> e : params.entrySet()) {
            if (e.getValue() == null) continue;            /* .NET null value: not supplied */
            sql.append(first ? " " : ", ").append(e.getKey()).append("=?");
            values.add(e.getValue());
            first = false;
        }
        try (PreparedStatement ps = con.prepareStatement(sql.toString())) {
            for (int i = 0; i < values.size(); i++) bind(ps, i + 1, values.get(i));
            boolean isResult = ps.execute();
            boolean captured = false;
            while (true) {
                if (isResult) {
                    try (ResultSet rs = ps.getResultSet()) {
                        ResultSetMetaData md = rs.getMetaData();
                        int n = md.getColumnCount();
                        while (rs.next()) {
                            if (captured) continue;
                            Map<String, Object> row = new LinkedHashMap<>();
                            for (int c = 1; c <= n; c++) row.put(md.getColumnLabel(c), plain(rs.getObject(c)));
                            firstResult.add(row);
                        }
                    }
                    captured = true;
                } else if (ps.getUpdateCount() == -1) {
                    break;
                }
                isResult = ps.getMoreResults();
            }
        }
    }

    private static void bind(PreparedStatement ps, int i, Object v) throws SQLException {
        if (v instanceof Integer) ps.setInt(i, (Integer) v);
        else if (v instanceof Double) ps.setDouble(i, (Double) v);
        else if (v instanceof Boolean) ps.setBoolean(i, (Boolean) v);
        else if (v instanceof Timestamp) ps.setTimestamp(i, (Timestamp) v);
        else if (v instanceof String) ps.setNString(i, (String) v);
        else if (v instanceof Long) ps.setLong(i, (Long) v);
        else if (v instanceof Number) ps.setDouble(i, ((Number) v).doubleValue());
        else if (v == null) ps.setNull(i, Types.NULL);
        else ps.setObject(i, v);
    }

    /** Dates as local "yyyy-MM-ddTHH:mm:ss" text (no UTC shift), decimals as doubles. */
    private static Object plain(Object o) {
        if (o instanceof Timestamp) return ((Timestamp) o).toLocalDateTime().toString();
        if (o instanceof java.sql.Date) return ((java.sql.Date) o).toLocalDate().toString();
        if (o instanceof BigDecimal) return ((BigDecimal) o).doubleValue();
        return o;
    }

    public static LinkedHashMap<String, Object> p(Object... kv) {
        LinkedHashMap<String, Object> m = new LinkedHashMap<>();
        for (int i = 0; i + 1 < kv.length; i += 2) m.put((String) kv[i], kv[i + 1]);
        return m;
    }

    /** SQL Server column names are case-insensitive to the desktop; Map keys are not. */
    public static Object col(Map<String, Object> row, String name) {
        if (row == null) return null;
        Object v = row.get(name);
        if (v != null || row.containsKey(name)) return v;
        for (Map.Entry<String, Object> e : row.entrySet()) {
            if (e.getKey().equalsIgnoreCase(name)) return e.getValue();
        }
        return null;
    }

    /** Conversion.ToInt: null/DBNull/"" -> 0; Convert.ToInt32 (banker's rounding for doubles). */
    public static int toInt(Object o) {
        if (o == null) return 0;
        if (o instanceof Integer) return (Integer) o;
        if (o instanceof Number) return (int) Math.rint(((Number) o).doubleValue());
        if (o instanceof Boolean) return ((Boolean) o) ? 1 : 0;
        String s = String.valueOf(o).trim();
        if (s.isEmpty()) return 0;
        try { return Integer.parseInt(s); } catch (NumberFormatException e) { return 0; }
    }
}
