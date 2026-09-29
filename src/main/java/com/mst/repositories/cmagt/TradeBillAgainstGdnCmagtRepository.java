package com.mst.repositories.cmagt;

import com.mst.models.cmagt.dto.TradeBillAgainstGdnCmagtDto;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.namedparam.MapSqlParameterSource;
import org.springframework.jdbc.core.simple.SimpleJdbcCall;
import org.springframework.stereotype.Repository;
import org.springframework.util.LinkedCaseInsensitiveMap;

import java.time.LocalDate;
import java.util.*;

@Repository
public class TradeBillAgainstGdnCmagtRepository {

    @Autowired
    private JdbcTemplate jdbcTemplate;

    /* ==========================================================================================
     * WRITE SIDE - DAL 0402_Architecture.DAL.Inventory.InvCommAgentTradeBill.SetData, step for step.
     *
     * The desktop runs everything below on ONE SqlTransaction and rolls back on any exception.
     * Here the caller's @Transactional (TradeBillAgainstGdnCmagtService.save) plays that part:
     * every statement runs through this JdbcTemplate on the transaction-bound connection, and a
     * RAISERROR from any procedure (the master's financial-year guard, USP_VoucherBalanceCheck,
     * ...) propagates and rolls the whole document back.
     *
     *   1  USP_InvCommAgentTradeBill_InsertAndUpdate      -> id; if (n > 0) Id = n else n = Id
     *   2  Sp_InvCommAgentTradeBillDetail_Insert           per row (ActionTypeId 1/2/3 - removed rows first)
     *   3  Sp_InvCommisionAgentBillPurchaseExpense_Insert
     *   4  Sp_InvCommisionAgentBillSaleExpense_Insert
     *   5  Sp_CommisionAgentBillSaleExpenseCreditToReleventAc_Insert
     *   6  USP_CommisionAgentBillPurchaseFreightExpense_Insert
     *   7  USP_CommisionAgentBillPaymentDetail_Insert
     *   8  USP_CommisionAgentBillCommissionDetail_Insert
     *   9  USP_CommisionAgentBillFreightDetail_Insert
     *  10  USP_CommisionAgentBillTaxDetail_Insert
     *      (on ActionId 2 the master itself deletes the eight child tables before these re-insert)
     *  11  Proc_DMSAttachments_Insert                      - not reached: the web sends no attachments
     *  12  Sp_Vouchers_GetMethods GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId
     *      then Sp_VoucherHead_Insert (none found) / Sp_VoucherHead_Update (found)
     *  13  Sp_VoucherDetail_Insert                         per line, VoucherHeadId + BranchesId stamped
     *  14  USP_VoucherBalanceCheck @OrganizationId @CompanyId @Id
     *  15  Sp_VoucherHead_H_Insert -> DocumentTypeIdRef, Sp_VoucherDetail_H_Insert per line
     *  16  [dbo].[USP_InvCommAgentTradeBill_GenerateSpecialApprovalLog] (DocumentTypeId == 1056)
     *      "VoucherDetail list Not Found" when MakeVoucher produced no line (DAL :203).
     * ========================================================================================== */

    public int saveWithVoucher(TradeBillAgainstGdnCmagtDto.Bill obj,
                               com.mst.models.dto.ContraVoucherDto.Head head,
                               List<com.mst.models.dto.ContraVoucherDto.Detail> voucherDetails) {
        int num = toInt(scalar("dbo.USP_InvCommAgentTradeBill_InsertAndUpdate", paramsOf(obj)));
        if (num > 0) obj.Id = num; else num = obj.Id;

        for (TradeBillAgainstGdnCmagtDto.Detail d : obj.InvCommAgentTradeBillDetailslist) {
            d.InvCommAgentTradeBillId = obj.Id;
            scalar("dbo.Sp_InvCommAgentTradeBillDetail_Insert", paramsOf(d));
        }
        for (TradeBillAgainstGdnCmagtDto.PurchaseExpense x : obj.InvCommAgentTradePurchaseExpList) {
            x.InvCommAgentBillTradeId = obj.Id;
            scalar("dbo.Sp_InvCommisionAgentBillPurchaseExpense_Insert", paramsOf(x));
        }
        for (TradeBillAgainstGdnCmagtDto.SaleExpense x : obj.InvCommAgentTradeSaleExpList) {
            x.InvCommAgentBillTradeId = obj.Id;
            scalar("dbo.Sp_InvCommisionAgentBillSaleExpense_Insert", paramsOf(x));
        }
        for (TradeBillAgainstGdnCmagtDto.SaleExpenseCreditToReleventAc x : obj.CommisionAgentBillSaleExpenseCreditToReleventAcsList) {
            x.InvCommAgentBillTradeId = obj.Id;
            scalar("dbo.Sp_CommisionAgentBillSaleExpenseCreditToReleventAc_Insert", paramsOf(x));
        }
        for (TradeBillAgainstGdnCmagtDto.PurchaseFreightExpense x : obj.InvCommAgentTradeFreightExpList) {
            x.InvCommAgentBillTradeId = obj.Id;
            scalar("dbo.USP_CommisionAgentBillPurchaseFreightExpense_Insert", paramsOf(x));
        }
        for (TradeBillAgainstGdnCmagtDto.PaymentDetail x : obj.CommisionAgentBillPaymentDetailList) {
            x.InvCommAgentBillTradeId = obj.Id;
            scalar("dbo.USP_CommisionAgentBillPaymentDetail_Insert", paramsOf(x));
        }
        for (TradeBillAgainstGdnCmagtDto.CommissionDetail x : obj.CommisionAgentBillCommissionDetailList) {
            x.InvCommAgentBillTradeId = obj.Id;
            scalar("dbo.USP_CommisionAgentBillCommissionDetail_Insert", paramsOf(x));
        }
        for (TradeBillAgainstGdnCmagtDto.FreightDetail x : obj.CommisionAgentBillFreightDetailList) {
            x.InvCommAgentBillTradeId = obj.Id;
            scalar("dbo.USP_CommisionAgentBillFreightDetail_Insert", paramsOf(x));
        }
        for (TradeBillAgainstGdnCmagtDto.TaxDetail x : obj.CommisionAgentBillTaxDetailList) {
            x.InvCommAgentBillTradeId = obj.Id;
            scalar("dbo.USP_CommisionAgentBillTaxDetail_Insert", paramsOf(x));
        }

        /* Step 12 - CommonServices.GetVoucherHeadId (DAL 0205:60-100). */
        int num3 = 0;
        List<Map<String, Object>> vh = firstResultSet(
                "EXEC dbo.Sp_Vouchers_GetMethods @Activity=?, @OrganizationId=?, @CompanyId=?, "
              + "@DocumentTypeId=?, @DocumentTypeSrNo=?",
                "GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId",
                obj.OrganizationId, obj.CompanyId, obj.DocumentTypeId, obj.Id);
        if (!vh.isEmpty()) {
            num3 = toInt(ci(vh.get(0), "Id"));
            head.Id = num3;
        }
        head.DocumentTypeSrNo = obj.Id;
        int num2 = toInt(scalar(num3 == 0 ? "dbo.Sp_VoucherHead_Insert" : "dbo.Sp_VoucherHead_Update",
                paramsOf(head)));
        if (num2 > 0) head.Id = num2; else num2 = head.Id;

        if (voucherDetails == null || voucherDetails.isEmpty()) {
            throw new IllegalStateException("VoucherDetail list Not Found");
        }
        for (com.mst.models.dto.ContraVoucherDto.Detail vd : voucherDetails) {
            vd.VoucherHeadId = head.Id;
            vd.BranchesId = obj.BranchesId;
            scalar("dbo.Sp_VoucherDetail_Insert", paramsOf(vd));
        }
        firstResultSet("EXEC dbo.USP_VoucherBalanceCheck @OrganizationId=?, @CompanyId=?, @Id=?",
                obj.OrganizationId, obj.CompanyId, head.Id);
        int documentTypeIdRef = toInt(scalar("dbo.Sp_VoucherHead_H_Insert", paramsOf(head)));
        for (com.mst.models.dto.ContraVoucherDto.Detail vd : voucherDetails) {
            vd.VoucherHeadId = head.Id;
            vd.DocumentTypeIdRef = documentTypeIdRef;
            vd.BranchesId = obj.BranchesId;
            scalar("dbo.Sp_VoucherDetail_H_Insert", paramsOf(vd));
        }
        if (obj.DocumentTypeId != null && obj.DocumentTypeId == 1056) {
            firstResultSet("EXEC [dbo].[USP_InvCommAgentTradeBill_GenerateSpecialApprovalLog] "
                         + "@OrganizationId=?, @CompanyId=?, @DocumentTypeId=?, @DocId=?, @UserId=?",
                    obj.OrganizationId, obj.CompanyId, obj.DocumentTypeId, obj.Id,
                    (obj.EnteryUserId != null && obj.EnteryUserId > 0) ? obj.EnteryUserId : obj.ModifyUserId);
        }
        return num;
    }

    // ================================================================== lookups used by the save

    /**
     * CommonServies.GetSupplierCustomerListForFinancialEffects (BLL 0267:90-115):
     * Sp_SupplierCustomer_GetAllMethod @Activity='GetGlAccountIdandCompanyNameBySupplierCustomerId'.
     * Returns Id -> GlAccountId in the procedure's order (the BLL takes the first match).
     */
    public Map<Integer, Integer> glAccountsForFinancialEffects(int organizationId, int companyId) {
        Map<Integer, Integer> out = new LinkedHashMap<>();
        for (Map<String, Object> r : firstResultSet(
                "EXEC dbo.Sp_SupplierCustomer_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",
                organizationId, companyId, "GetGlAccountIdandCompanyNameBySupplierCustomerId")) {
            out.putIfAbsent(toInt(ci(r, "Id")), toInt(ci(r, "GlAccountId")));
        }
        return out;
    }

    /**
     * dtSupplier - the form's party table (CommonBindings.SupplierDtFillFromGlobal over
     * clsGlobalVariables.globalAllSupplierCustomer = USP_GetVendorsAndCustomersWithCityName).
     * Insert() resolves party GL ids through it (DatatableHelper.GetFirstColumnValue).
     */
    public Map<Integer, Integer> partyGlAccounts(int organizationId, int companyId) {
        Map<Integer, Integer> out = new LinkedHashMap<>();
        for (Map<String, Object> r : firstResultSet(
                "EXEC dbo.USP_GetVendorsAndCustomersWithCityName @OrganizationId=?, @CompanyId=?",
                organizationId, companyId)) {
            out.putIfAbsent(toInt(ci(r, "Id")), toInt(ci(r, "GlAccountId")));
        }
        return out;
    }

    /** GlobalVariables_Helper.GetConfigValueFromGlobal - ConfigKey, or null when not configured. */
    public String configValue(int organizationId, int companyId, String configDescription) {
        List<Map<String, Object>> rows = firstResultSet(
                "EXEC dbo.Sp_ConfigrationsAllocation_GetAllMethod @OrganizationId=?, @CompanyId=?, "
              + "@ConfigDescription=?, @Activity=?",
                organizationId, companyId, configDescription,
                "GetConfigurationByOrgCompandConfigDescription");
        if (rows.isEmpty()) return null;
        Object v = ci(rows.get(0), "ConfigKey");
        return v == null ? null : String.valueOf(v).trim();
    }

    /** clsGlobalVariables.AllAccountsWithCustomGroupId (GlobalServicesMethods:609). */
    public List<Map<String, Object>> allAccountsWithCustomGroup(int organizationId, int companyId) {
        return firstResultSet("EXEC dbo.USP_GETAllAccountsFromCustomGroups @OrganizationId=?, @CompanyId=?",
                organizationId, companyId);
    }

    /** DatatableHelper.TaxTypesDbCall -> TaxesTypes.GetForComboBind (Type = 1, 'ReadByCombo'). */
    public List<Map<String, Object>> taxTypes(int organizationId, int companyId) {
        return firstResultSet("EXEC dbo.Sp_TaxesTypes_GetAllMethod @OrganizationId=?, @CompanyId=?, @Type=?, @Activity=?",
                organizationId, companyId, 1, "ReadByCombo");
    }

    /** PurchaseTaxCalculation (9747) -> TaxScheduleMain.ReadTaxSchedule 'GetTaxPercentInTaxSchedule'. */
    public List<Map<String, Object>> taxSchedulePercent(int organizationId, int companyId,
                                                        java.sql.Date effectedDate, int taxNameId) {
        return firstResultSet("EXEC dbo.Sp_TaxSchedule_GetAllMehtod @OrganizationId=?, @CompanyId=?, "
                            + "@EffectedDate=?, @TaxNameId=?, @Activity=?",
                organizationId, companyId, effectedDate, taxNameId, "GetTaxPercentInTaxSchedule");
    }

    /** SaleTaxTypesBind (1244) -> SupplierCustomerTaxSchedule.TaxSchedule_GetBySupplierCustomer. */
    public List<Map<String, Object>> taxScheduleBySupplierCustomer(int organizationId, int companyId,
                                                                   int supplierCustomerId, java.sql.Date docDate) {
        return firstResultSet("EXEC dbo.USP_TaxSchedule_GetBySupplierCustomer @OrganizationId=?, @CompanyId=?, "
                            + "@SupplierCustomerId=?, @DocDate=?",
                organizationId, companyId, supplierCustomerId, docDate);
    }

    /**
     * HistoryComboDbCall (6228) -> BLL GetDataForDropDownFromCommOrder: only @OrganizationId and
     * @CompanyId (DocumentTypeIds / Activity are null, so every activity and document type).
     * Rows {Id, ReferenceName, Activity}: TradingAccount / Supplier / Customer ...
     */
    public List<Map<String, Object>> historyCombos(int organizationId, int companyId) {
        return firstResultSet("EXEC [dbo].[USP_DropDownFillFromInvCommAgentTradeBill] @OrganizationId=?, @CompanyId=?",
                organizationId, companyId);
    }

    /** clsGlobalVariables.ActiveYr.Start_Period (cmbperemeter_ValueChanged case 5). */
    public Object financialYearStart(int organizationId, int companyId, int financialYearId) {
        List<Map<String, Object>> years = firstResultSet(
                "EXEC dbo.Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId @OrganizationId=?, @CompanyId=?",
                organizationId, companyId);
        Map<String, Object> row = null;
        for (Map<String, Object> r : years) {
            Object id = null;
            for (Map.Entry<String, Object> e : r.entrySet()) if ("Id".equalsIgnoreCase(e.getKey())) id = e.getValue();
            if (id instanceof Number && ((Number) id).intValue() == financialYearId) { row = r; break; }
        }
        if (row == null && !years.isEmpty()) row = years.get(0);
        if (row == null) return null;
        for (Map.Entry<String, Object> e : row.entrySet()) if ("Start_Period".equalsIgnoreCase(e.getKey())) return e.getValue();
        return null;
    }

    // ====================================================================== GDN loader (1055)

    /**
     * frmPendingGdnLoader.ComboDbCall -> gdnBuyerDispatchMaster.GetDataForDropDown: only
     * @OrganizationId and @CompanyId are sent (@Activity is empty, so it is omitted).
     */
    public List<Map<String, Object>> loaderDropdowns(int organizationId, int companyId) {
        return firstResultSet("EXEC [cmagt].[USP_GetDataForDropDownFromgdnBuyerDispatchMaster] "
                            + "@OrganizationId=?, @CompanyId=?", organizationId, companyId);
    }

    /**
     * frmPendingGdnLoader.PendingDataDbCall -> gdnBuyerDispatchMaster.PendingDataLoaderForBill
     * (BLL :612-735). Tenancy, branch, year and DocumentTypeId 1055 always; dates only when set;
     * every other filter only when non-zero / non-empty, exactly as the BLL adds them. All NINE
     * result sets are returned - the form keeps the DataSet (LoadedDataSetToGetExpenseAndEbDetail)
     * and reads Tables[1..8] for empty bags, expenses, commission and payment terms.
     */
    public List<List<Map<String, Object>>> pendingGdnForBill(int organizationId, int companyId, int branchesId,
                                                             int financialYearId, int documentTypeId,
                                                             java.sql.Date fromDate, java.sql.Date toDate,
                                                             int fromDocNo, int toDocNo, int commissionAgentId,
                                                             int supplierId, int buyerId, int itemId,
                                                             int deliverToPartyId, String shipToAddress) {
        List<String> names = new ArrayList<>(Arrays.asList("@OrganizationId", "@CompanyId", "@BranchesId",
                "@FinancialYearId", "@DocumentTypeId"));
        List<Object> args = new ArrayList<>(Arrays.asList(organizationId, companyId, branchesId,
                financialYearId, documentTypeId));
        if (fromDate != null) { names.add("@FromDate"); args.add(fromDate); }
        if (toDate != null)   { names.add("@ToDate");   args.add(toDate); }
        if (fromDocNo != 0)   { names.add("@FromDocNo"); args.add(fromDocNo); }
        if (toDocNo != 0)     { names.add("@ToDocNo");   args.add(toDocNo); }
        if (commissionAgentId != 0) { names.add("@CommissionAgentId"); args.add(commissionAgentId); }
        if (supplierId != 0)  { names.add("@supplierId"); args.add(supplierId); }
        if (buyerId != 0)     { names.add("@BuyerId");    args.add(buyerId); }
        if (itemId != 0)      { names.add("@ItemId");     args.add(itemId); }
        if (deliverToPartyId != 0) { names.add("@DeliveryToPartyId"); args.add(deliverToPartyId); }
        if (shipToAddress != null && !shipToAddress.isEmpty()) { names.add("@ShipToAddress"); args.add(shipToAddress); }
        StringBuilder sql = new StringBuilder("EXEC [cmagt].[USP_gdnBuyerDispatchMaster_PendingDataLoaderForBill] ");
        for (int i = 0; i < names.size(); i++) {
            if (i > 0) sql.append(", ");
            sql.append(names.get(i)).append("=?");
        }
        return allResultSets(sql.toString(), args.toArray());
    }

    // =============================================================================== plumbing

    /**
     * GenericProvider.SetProc: every non-virtual property as @Name. A null value is left out -
     * SqlClient does not send a parameter whose value is null, so the procedure's default
     * applies; that is what the desktop does with a null string or a null DateTime?.
     */
    static List<Object[]> paramsOf(Object model) {
        List<Object[]> out = new ArrayList<>();
        for (java.lang.reflect.Field f : model.getClass().getFields()) {
            if (java.lang.reflect.Modifier.isStatic(f.getModifiers())) continue;
            if (f.isAnnotationPresent(TradeBillAgainstGdnCmagtDto.NotParam.class)) continue;
            if (List.class.isAssignableFrom(f.getType()) || Map.class.isAssignableFrom(f.getType())) continue;
            Object v;
            try { v = f.get(model); } catch (IllegalAccessException e) { continue; }
            if (v == null) continue;
            out.add(new Object[]{f.getName(), v});
        }
        return out;
    }

    /** ExecuteScalar: first column of the first row of the first result set, or null. */
    private Object scalar(String proc, List<Object[]> params) {
        StringBuilder sql = new StringBuilder("EXEC ").append(proc).append(' ');
        Object[] args = new Object[params.size()];
        for (int i = 0; i < params.size(); i++) {
            if (i > 0) sql.append(", ");
            sql.append('@').append(params.get(i)[0]).append("=?");
            args[i] = params.get(i)[1];
        }
        List<Map<String, Object>> rows = firstResultSet(sql.toString(), args);
        if (rows.isEmpty() || rows.get(0).isEmpty()) return null;
        return rows.get(0).values().iterator().next();
    }

    private List<Map<String, Object>> firstResultSet(String sql, Object... args) {
        List<List<Map<String, Object>>> all = run(sql, true, args);
        return all.isEmpty() ? new ArrayList<>() : all.get(0);
    }

    private List<List<Map<String, Object>>> allResultSets(String sql, Object... args) {
        return run(sql, false, args);
    }

    /**
     * Executes a procedure that may interleave update counts with result sets (temp tables,
     * SET NOCOUNT OFF) and returns its result sets in order - the equivalent of a DataSet fill.
     * A procedure that returns nothing yields an empty list instead of the
     * "statement did not return a result set" failure queryForList raises.
     */
    private List<List<Map<String, Object>>> run(String sql, boolean firstOnly, Object... args) {
        return jdbcTemplate.execute(sql, (java.sql.PreparedStatement ps) -> {
            for (int i = 0; i < args.length; i++) ps.setObject(i + 1, args[i]);
            List<List<Map<String, Object>>> sets = new ArrayList<>();
            boolean isRs = ps.execute();
            while (true) {
                if (isRs) {
                    List<Map<String, Object>> rows = new ArrayList<>();
                    try (java.sql.ResultSet rs = ps.getResultSet()) {
                        java.sql.ResultSetMetaData md = rs.getMetaData();
                        int n = md.getColumnCount();
                        while (rs.next()) {
                            Map<String, Object> row = new LinkedCaseInsensitiveMap<>();
                            for (int c = 1; c <= n; c++) {
                                String label = md.getColumnLabel(c);
                                if (label == null || label.isEmpty()) label = "col" + c;
                                row.put(label, rs.getObject(c));
                            }
                            rows.add(row);
                        }
                    }
                    sets.add(rows);
                    if (firstOnly) {
                        /* Drain the rest so a RAISERROR raised after the first SELECT still
                           surfaces, as it does through SqlCommand. */
                        try { while (ps.getMoreResults() || ps.getUpdateCount() != -1) { } }
                        catch (java.sql.SQLException e) { throw e; }
                        return sets;
                    }
                } else if (ps.getUpdateCount() == -1) {
                    break;
                }
                isRs = ps.getMoreResults();
            }
            return sets;
        });
    }

    private static Object ci(Map<String, Object> row, String name) {
        if (row == null) return null;
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet())
            if (e.getKey() != null && e.getKey().equalsIgnoreCase(name)) return e.getValue();
        return null;
    }

    private static int toInt(Object v) {
        if (v instanceof Number) return ((Number) v).intValue();
        if (v == null) return 0;
        try { return (int) Double.parseDouble(String.valueOf(v).trim()); } catch (Exception e) { return 0; }
    }


    /* ------------------------------------------------------------------------------------------
     * READ SIDE - BLL 0547_Architecture.BLL.Inventory.InvCommAgentTradeBill.cs and DAL
     * 0402_Architecture.DAL.Inventory.InvCommAgentTradeBill.cs, procedure
     * [dbo].[USP_InvCommAgentTradeBill_GetAllMethods] (23 params, every one defaults to NULL).
     *
     * The previous body sent @Activity='ReadBySearch_InvCommAgentTradeBill' (history) and
     * 'ReadById_InvCommAgentTradeBill' (open). Neither string is an IF branch of the procedure -
     * its branches are GenerateCode, GenerateBranchSrCode, ReadById, nine ReadByHeaderId_*,
     * FormHistory and DeleteById - so the procedure matched nothing and returned no result set:
     * History was always empty and every open said "Record not found".
     * ------------------------------------------------------------------------------------------ */

    private static final String PROC = "USP_InvCommAgentTradeBill_GetAllMethods";

    /**
     * The nine child reads DAL GetDate performs after ReadById (0402 lines 229-272), in order,
     * keyed by the model property the DAL fills. Each is @Id + @Activity only (GetDetail).
     */
    private static final String[][] CHILD_READS = {
            {"InvCommAgentTradeBillDetailslist",                      "ReadByHeaderId_InvCommAgentTradeBillDetail"},
            {"InvCommAgentTradePurchaseExpList",                      "ReadByHeaderId_CommisionAgentBillPurchaseExpense"},
            {"InvCommAgentTradeFreightExpList",                       "ReadByHeaderId_CommisionAgentBillPurchaseFreightExpense"},
            {"InvCommAgentTradeSaleExpList",                          "ReadByHeaderId_CommisionAgentBillSaleExpense"},
            {"CommisionAgentBillSaleExpenseCreditToReleventAcsList",  "ReadByHeaderId_CommisionAgentBillSaleExpenseCreditToReleventAc"},
            {"CommisionAgentBillCommissionDetailList",                "ReadByHeaderId_CommisionAgentBillCommissionDetail"},
            {"CommisionAgentBillPaymentDetailList",                   "ReadByHeaderId_CommisionAgentBillPaymentDetail"},
            {"CommisionAgentBillFreightDetailList",                   "ReadByHeaderId_CommisionAgentBillFreightDetail"},
            {"CommisionAgentBillTaxDetailList",                       "ReadByHeaderId_CommisionAgentBillTaxDetail"},
    };

    /**
     * BLL FormHistory (0547 lines 910-1087), called by frmCommissionAgentTradeBillAgainstGdn
     * HistoryGridFill (5943-6039).
     *
     * Always sent: @OrganizationId, @CompanyId, @DocumentTypeId, @FinancialYearId,
     * @CanViewAllRecord, @Activity. @BranchesId is sent when non-zero (it always is on the
     * desktop). @EntryUserId only when the user lacks "CanView AllRecord". The dates and the
     * doc-no / trading-account / supplier / customer filters only when set.
     *
     * @CanViewAllRecord is the load-bearing one: the procedure's WHERE ends with
     * (@CanViewAllRecord = 1 or (@CanViewAllRecord = 0 and h.EnteryUserId = @EntryUserId)),
     * so a NULL there returns zero rows for everybody. It is declared INT - bound as 1/0.
     */
    public List<Map<String, Object>> formHistory(int organizationId, int companyId, int branchId,
                                                 int financialYearId, int documentTypeId,
                                                 boolean canViewAllRecord, Integer entryUserId,
                                                 String fromDate, String toDate,
                                                 Integer docNoFrom, Integer docNoTo,
                                                 Integer tradingAccountId, Integer supplierId,
                                                 Integer customerId) {
        return formHistory(organizationId, companyId, branchId, financialYearId, documentTypeId, canViewAllRecord,
                entryUserId, "doc", fromDate, toDate, docNoFrom, docNoTo, tradingAccountId, supplierId, customerId);
    }

    /**
     * HistoryGridFill (5959-5991): the one date pair is sent as @FromDate/@ToDate (drdocdate),
     * @EntryFromDate/@EntryToDate (rdentrydate) or @ModifyFromDate/@ModifyToDate (rdmodifydate) -
     * BLL FormHistory parameter names, all declared by USP_InvCommAgentTradeBill_GetAllMethods.
     */
    public List<Map<String, Object>> formHistory(int organizationId, int companyId, int branchId,
                                                 int financialYearId, int documentTypeId,
                                                 boolean canViewAllRecord, Integer entryUserId,
                                                 String dateKind, String fromDate, String toDate,
                                                 Integer docNoFrom, Integer docNoTo,
                                                 Integer tradingAccountId, Integer supplierId,
                                                 Integer customerId) {
        MapSqlParameterSource p = new MapSqlParameterSource();
        p.addValue("OrganizationId", organizationId);
        p.addValue("CompanyId", companyId);
        p.addValue("DocumentTypeId", documentTypeId);
        p.addValue("FinancialYearId", financialYearId);
        if (branchId != 0) p.addValue("BranchesId", branchId);
        p.addValue("CanViewAllRecord", canViewAllRecord ? 1 : 0);
        if (!canViewAllRecord) p.addValue("EntryUserId", entryUserId);
        java.sql.Date f = parseDate(fromDate);
        java.sql.Date t = parseDate(toDate);
        String fromName = "FromDate", toName = "ToDate";
        if ("entry".equalsIgnoreCase(dateKind)) { fromName = "EntryFromDate"; toName = "EntryToDate"; }
        else if ("modify".equalsIgnoreCase(dateKind)) { fromName = "ModifyFromDate"; toName = "ModifyToDate"; }
        if (f != null) p.addValue(fromName, f);
        if (t != null) p.addValue(toName, t);
        if (nz(docNoFrom))        p.addValue("DocNoFrom", docNoFrom);
        if (nz(docNoTo))          p.addValue("DocNoTo", docNoTo);
        if (nz(tradingAccountId)) p.addValue("TradingAccountId", tradingAccountId);
        if (nz(supplierId))       p.addValue("SupplierId", supplierId);
        if (nz(customerId))       p.addValue("CustomerId", customerId);
        p.addValue("Activity", "FormHistory");
        return call(p);
    }

    /**
     * BLL GetByID (0547 lines 761-783): @Id + @Activity='ReadById', then DAL GetDate reads the
     * nine child collections. Returns null when the procedure yields no row (deleted - the
     * branch filters ActionId <> 3 - or not found).
     *
     * The ReadById SELECT carries placeholder columns ('' AS InvCommAgentTradeBillDetailslist,
     * ... ) for every child list; the header is kept case-insensitive so the real lists replace
     * those placeholders instead of sitting beside them under a second casing.
     */
    public Map<String, Object> readById(int id) {
        MapSqlParameterSource p = new MapSqlParameterSource();
        p.addValue("Id", id);
        p.addValue("Activity", "ReadById");
        List<Map<String, Object>> rows = call(p);
        if (rows.isEmpty()) return null;

        Map<String, Object> header = new LinkedCaseInsensitiveMap<>();
        header.putAll(rows.get(0));
        for (String[] child : CHILD_READS) {
            MapSqlParameterSource cp = new MapSqlParameterSource();
            cp.addValue("Id", id);
            cp.addValue("Activity", child[1]);
            header.put(child[0], call(cp));
        }
        return header;
    }

    /**
     * BLL GenerateCode / GenerateBranchSrCode (0547 lines 810-908). Both read column DocNo.
     * DocumentNoDbCall / BranchSrNoDbCall in the form (1149-1199).
     */
    public int generateCode(String activity, int organizationId, int companyId,
                            int documentTypeId, int financialYearId, int branchId) {
        MapSqlParameterSource p = new MapSqlParameterSource();
        p.addValue("OrganizationId", organizationId);
        p.addValue("CompanyId", companyId);
        p.addValue("DocumentTypeId", documentTypeId);
        p.addValue("FinancialYearId", financialYearId);
        p.addValue("BranchesId", branchId);
        p.addValue("Activity", activity);
        List<Map<String, Object>> rows = call(p);
        if (rows.isEmpty()) return 0;
        Object v = rows.get(0).get("DocNo");
        return v instanceof Number ? ((Number) v).intValue() : 0;
    }

    /**
     * BLL DeleteByID (0547 lines 785-808): @EntryUserId, @Id, @Activity='DeleteById', one
     * transaction. The procedure itself refuses an approved bill (RAISERROR), soft-deletes the
     * header and detail (ActionId / ActionTypeId = 3), removes attachments, inventory rows and
     * the VoucherHead/VoucherDetail for this document, and writes the user-audit row.
     */
    public void deleteById(int entryUserId, int id) {
        MapSqlParameterSource p = new MapSqlParameterSource();
        p.addValue("EntryUserId", entryUserId);
        p.addValue("Id", id);
        p.addValue("Activity", "DeleteById");
        new com.mst.repositories.support.LenientJdbcCall(jdbcTemplate).withProcedureName(PROC).execute(p);
    }

    private List<Map<String, Object>> call(MapSqlParameterSource p) {
        return extractList(new com.mst.repositories.support.LenientJdbcCall(jdbcTemplate).withProcedureName(PROC).execute(p));
    }

    @SuppressWarnings("unchecked")
    private List<Map<String, Object>> extractList(Map<String, Object> out) {
        for (Object val : out.values()) {
            if (val instanceof List) {
                return (List<Map<String, Object>>) val;
            }
        }
        return Collections.emptyList();
    }

    private static boolean nz(Integer v) { return v != null && v != 0; }

    /**
     * A blank date is OMITTED, as the desktop omits an unchecked date picker. It used to become
     * "today", silently narrowing the history to one day; an unparseable value did the same.
     */
    private static java.sql.Date parseDate(String s) {
        if (s == null || s.trim().isEmpty()) return null;
        try {
            return java.sql.Date.valueOf(LocalDate.parse(s.trim().substring(0, Math.min(10, s.trim().length()))));
        } catch (Exception e) {
            throw new IllegalArgumentException("Invalid date '" + s + "' - expected yyyy-MM-dd");
        }
    }
}
