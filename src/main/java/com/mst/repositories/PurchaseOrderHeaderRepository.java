package com.mst.repositories;

import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.SqlParameterValue;
import org.springframework.stereotype.Repository;

import java.sql.Types;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * The Purchase Order HEADER write path, on the desktop's own chain.
 *
 * ---------------------------------------------------------------------------------------------
 * WHY THIS EXISTS
 * ---------------------------------------------------------------------------------------------
 * PurchaseOrderFullService used to write the header with a hand-built statement:
 *
 *   INSERT INTO PurchaseOrder (... SupplierCustomerId, CommissionAgentId ...
 *                              IsApproved, IsAproved, ... FinancialYearId ...)
 *   VALUES (?,?,?,?,?,?,?,?,?, 1, 1, ?,?,?, 1, ?, GETDATE())
 *
 * It failed with "Invalid column name 'CommissionAgentId'", and the crash was the smallest of its
 * problems:
 *
 *   - CommissionAgentId is not a column of dbo.PurchaseOrder at all. It belongs to
 *     Architecture.Model.pcc.PurchaseOrder and Architecture.Model.FeedMill.Purchase.PurchaseOrder
 *     - two OTHER modules' order models. This screen is the Inventory Purchase Order
 *     (PurchsaeOrder.cs, DocumentTypeId 41), whose commission agent is BrokerAgentSupCustId
 *     (PurchsaeOrder.cs:3323).
 *   - SupplierCustomerId was written too; the desktop model has no such property and neither
 *     procedure accepts one. The desktop writes OrderSupCustId only (:3303).
 *   - IsApproved and IsAproved were both hardcoded to 1, so every order saved from the web was
 *     born approved. The desktop sets IsAproved = false on insert and carries the EXISTING
 *     approval state on update (:3289-3293), and never touches IsApproved through this path.
 *   - FinancialYearId was hardcoded to 1 instead of the active year (:4707).
 *   - The DateLock guard, the detail-id guard, and the whole of the procedure's own business
 *     logic were bypassed.
 *
 * ---------------------------------------------------------------------------------------------
 * WHAT THE PROCEDURES DO THAT THE RAW STATEMENT DID NOT
 * ---------------------------------------------------------------------------------------------
 * Sp_PurchaseOrder_Insert is not a thin wrapper. Before it inserts anything it:
 *   - validates the document date against the active financial year through
 *     dbo.FunCheckValidFinancialYear and raises
 *     "Document Date(...) is Not Valid Against Active Financial Year!"
 *   - computes @DocNo as MAX(DocNo)+1 for this document type / org / company, honouring
 *     configuration 320 (UsePurchaseOrderBalanceInNextFinYear)
 *   - computes @BranchSrNo the same way, per branch
 *   - defaults @LocationTypeId to 1 when it arrives 0
 *   - forces DeliveryTerm to 'Load' when OrderCatagoryId is 8
 *   - raises on DocDate > DeliveryStartDate, on a missing DeliveryTerm, on a missing OrderStatus
 *   - writes a USP_UserAudit_Insert audit row
 *   - returns the new id as SELECT @RecId, where @RecId = SCOPE_IDENTITY()
 *
 * Sp_PurchaseOrder_Update additionally deletes and lets the caller re-insert the child tables
 * itself: PurchaseOrderEmptyBags, PurchaseOrderExpensesChargeToProduct, PurchaseOrderLabDeduction,
 * PurchaseOrderSupplierExpense, PurchaseOrderPaymentTermsDetail,
 * PurchaseOrderSupplierDispatchDetail and AdvancePaymentAdjustment, all WHERE PurchaseOrderId=@Id.
 *
 * None of that ran before. All of it runs now.
 *
 * ---------------------------------------------------------------------------------------------
 * PARAMETERS
 * ---------------------------------------------------------------------------------------------
 * GenericProvider.SetProc sends one parameter per NON-VIRTUAL property of
 * Architecture.Model.Inventory.PurchaseOrder, in declaration order. That model declares 73
 * properties of which 10 are virtual (the nine child collections plus OrderDetailRemoveIds), so
 * 63 parameters go to the procedure. Both procedures declare 68, every one with a default; the
 * five the model never sends - OrderStatusRemarks, PermitNo, PermitIssueDate, PermitValidityUpto,
 * UserLogId - are left to their defaults exactly as on the desktop.
 */
@Repository
public class PurchaseOrderHeaderRepository {

    private static final String P_INSERT   = "Sp_PurchaseOrder_Insert";
    private static final String P_UPDATE   = "Sp_PurchaseOrder_Update";
    private static final String P_DATELOCK = "Sp_DateLock_GetAllMethod";

    /** Architecture.Model.Inventory.PurchaseOrder, declaration order, virtuals excluded. */
    private static final String[] PARAMS = {
            "IsAproved", "PostState", "DeliveryStartDate", "DocDate", "EntryDate",
            "ModifyDate", "OrderDueDate", "OrderExpiryDate", "PostDate", "CommAmount",
            "CommRate", "BranchesId", "BrokerAgentSupCustId", "CatagorySrNo", "CompanyId",
            "DeliveryDays", "LocationTypeId", "DocNo", "BranchSrNo", "DocumentTypeId",
            "EntryUser", "FinancialYearId", "Id", "ModifyUser", "OrderCatagoryId",
            "BaseDocumentTypeId", "OrderDueDays", "CityId", "OrderSupCustId", "OrganizationId",
            "PaymentTermsId", "PostUser", "ProjectsId", "RefrenenceParty",
            "SupplierCustomerIdStockParty", "UomScheduleIdCmRate", "BrokerAgentId",
            "BillCalculateTypeId", "BrokeryType", "BrokeryRate", "BrokeryUom", "BrokeryAmount",
            "CommissionRemarks", "CommissionType", "DeliveryRemarks", "DeliveryTerm",
            "DeliveryTermId", "OrderStatus", "OrderTaxable", "CashFreight", "CreditFreight",
            "OrderType", "RemarksHeader", "SupplierRefNo", "OrderQty", "OrderWeight",
            "OrderAmount", "CurrencyId", "BookingPersonId", "ExchangeRate", "FcyAmount",
            "AttachmentsValues", "CustomAttachmentsValues"
    };

    private final JdbcTemplate jdbc;
    public PurchaseOrderHeaderRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    /**
     * BLL 0595 Save() step 1 - DateLock.GetByOrganizationCompanyIdAndDate, and the desktop's own
     * refusal when the document date is on or before the lock date.
     *
     * The desktop reads row 0 without checking the row count, so an empty result throws there.
     * Here an empty result simply means no lock is configured, which is the behaviour an operator
     * would expect and cannot let a bad write through.
     */
    public void assertNotDateLocked(int organizationId, int companyId, java.sql.Date docDate) {
        List<Map<String, Object>> rows = jdbc.queryForList(
                "EXEC dbo." + P_DATELOCK + " @OrganizationId=?, @CompanyId=?, @Activity=?",
                organizationId, companyId, "ReadByOrganizationCompanyIdandDate");
        if (rows.isEmpty() || docDate == null) return;
        Object d = rows.get(0).get("Date");
        if (d == null) return;
        java.sql.Date lock;
        if (d instanceof java.sql.Date)            lock = (java.sql.Date) d;
        else if (d instanceof java.util.Date)      lock = new java.sql.Date(((java.util.Date) d).getTime());
        else return;
        if (!docDate.toLocalDate().isAfter(lock.toLocalDate())) {
            throw new IllegalArgumentException("Not Insert or Update record please check lock date");
        }
    }

    // =========================================================================== numbering
    /*
     * The three numbers the desktop puts on the header the moment the form is used. The web page
     * showed PO-34 with an empty "Branch #" and "Cat No" because it had a hand-written
     * SELECT MAX(DocNo)+1 that ignored DocumentTypeId and FinancialYearId, and no generator at
     * all for the other two.
     */

    /**
     * DocumentNoFill() -> CommonServices.PurchaseOrderGenerateCode(41)
     *                  -> BLL 0595 PurchaseOrder.GenerateCode.
     * The desktop scopes the number by organization, company, DOCUMENT TYPE and FINANCIAL YEAR.
     */
    public int nextDocNo(int organizationId, int companyId, int documentTypeId, int financialYearId) {
        return firstNumber(jdbc.queryForList(
                "EXEC dbo.Sp_PurchaseOrder_GetAllMethod @OrganizationId=?, @CompanyId=?, "
              + "@DocumentTypeId=?, @FinancialYearId=?, @Activity=?",
                organizationId, companyId, documentTypeId, financialYearId,
                "GenerateDocNoByDocumentTypeId"));
    }

    /**
     * BranchSrNoFill() -> BLL 0595 GeneratePurchaseOrderBranchCodeByDocId.
     * Same as above plus the branch. The BLL omits @FinancialYearId when it is 0, so this does too.
     */
    public int nextBranchSrNo(int organizationId, int companyId, int documentTypeId,
                              int branchesId, int financialYearId) {
        if (financialYearId != 0) {
            return firstNumber(jdbc.queryForList(
                    "EXEC dbo.Sp_PurchaseOrder_GetAllMethod @OrganizationId=?, @CompanyId=?, "
                  + "@DocumentTypeId=?, @BranchesId=?, @FinancialYearId=?, @Activity=?",
                    organizationId, companyId, documentTypeId, branchesId, financialYearId,
                    "GeneratePurchaseOrderBranchCodeByDocId"));
        }
        return firstNumber(jdbc.queryForList(
                "EXEC dbo.Sp_PurchaseOrder_GetAllMethod @OrganizationId=?, @CompanyId=?, "
              + "@DocumentTypeId=?, @BranchesId=?, @Activity=?",
                organizationId, companyId, documentTypeId, branchesId,
                "GeneratePurchaseOrderBranchCodeByDocId"));
    }

    /**
     * combordercat_Leave -> CommonServices.GenerateOrderCategoryCodebyId(categoryId)
     *                    -> BLL 0578 InvOrderCategory.GenerateOrderCategoryCodebyId.
     *
     * A DIFFERENT procedure and a different table from the two above, and it fires when the
     * Category combo loses focus - not on New. That cascade is why "Cat No" was blank on the web.
     */
    public int nextCategorySrNo(int organizationId, int companyId, int orderCategoryId, int financialYearId) {
        if (financialYearId != 0) {
            return firstNumber(jdbc.queryForList(
                    "EXEC dbo.Sp_InvOrderCategory_GetAllMethod @OrganizationId=?, @CompanyId=?, "
                  + "@OrderCatagoryId=?, @FinancialYearId=?, @Activity=?",
                    organizationId, companyId, orderCategoryId, financialYearId,
                    "GenerateOrderCategoryCodeById"));
        }
        return firstNumber(jdbc.queryForList(
                "EXEC dbo.Sp_InvOrderCategory_GetAllMethod @OrganizationId=?, @CompanyId=?, "
              + "@OrderCatagoryId=?, @Activity=?",
                organizationId, companyId, orderCategoryId, "GenerateOrderCategoryCodeById"));
    }

    /**
     * GlobalVariables_Helper.GetConfigValueFromGlobal(name) - the per organization+company
     * configuration value, read through the same procedure the rest of this port uses.
     *
     * The Purchase Order screen reads three of these on New, and the web page had all three
     * hard-coded instead:
     *     WeightCutForJuteBags     -> JuteBag Cut  (desktop showed 2.25, web showed 0.00)
     *     WeightCutForPPBags       -> PPBag Cut    (desktop showed 1.25, web showed 0.00)
     *     OrderDefaultDeliveryDays -> Delivery Days (desktop showed 1, web hard-coded 7)
     */
    public String config(int organizationId, int companyId, String name) {
        List<Map<String, Object>> rows = jdbc.queryForList(
                "EXEC dbo.Sp_ConfigrationsAllocation_GetAllMethod @OrganizationId=?, @CompanyId=?, "
              + "@ConfigDescription=?, @Activity=?",
                organizationId, companyId, name,
                "GetConfigurationByOrgCompandConfigDescription");
        if (rows.isEmpty()) return null;
        Object v = rows.get(0).get("ConfigKey");
        return v == null ? null : String.valueOf(v).trim();
    }

    /**
     * Read-only. Why Sp_PurchaseOrder_GetAllMethod @Activity='GenerateDocNoByDocumentTypeId'
     * returned 1 where the desktop shows 493.
     *
     * That activity is MAX(DocNo)+1 over dbo.PurchaseOrder filtered on
     * DocumentTypeId + OrganizationId + CompanyId + FinancialYearId. A result of 1 means the
     * filter matched nothing, so this relaxes it one predicate at a time: the first level that
     * starts returning rows is the predicate that does not match.
     *
     * It also lists the FinancialYearIds that actually carry rows for this type/org/company, so
     * the right year is visible rather than inferred, and reads configuration 320 because when
     * that is on the year predicate is bypassed entirely.
     */
    public Map<String, Object> docNoDiagnostics(int organizationId, int companyId,
                                                int documentTypeId, int financialYearId) {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("filter", "DocumentTypeId=" + documentTypeId + ", OrganizationId=" + organizationId
                        + ", CompanyId=" + companyId + ", FinancialYearId=" + financialYearId);
        out.put("rows_type_org_company_year", one(
                "SELECT COUNT(1) FROM PurchaseOrder WHERE DocumentTypeId=? AND OrganizationId=? AND CompanyId=? AND FinancialYearId=?",
                documentTypeId, organizationId, companyId, financialYearId));
        out.put("rows_type_org_company", one(
                "SELECT COUNT(1) FROM PurchaseOrder WHERE DocumentTypeId=? AND OrganizationId=? AND CompanyId=?",
                documentTypeId, organizationId, companyId));
        out.put("rows_type_org", one(
                "SELECT COUNT(1) FROM PurchaseOrder WHERE DocumentTypeId=? AND OrganizationId=?",
                documentTypeId, organizationId));
        out.put("rows_type", one(
                "SELECT COUNT(1) FROM PurchaseOrder WHERE DocumentTypeId=?", documentTypeId));
        out.put("maxDocNo_type_org_company", one(
                "SELECT ISNULL(MAX(CONVERT(int,DocNo)),0) FROM PurchaseOrder WHERE DocumentTypeId=? AND OrganizationId=? AND CompanyId=?",
                documentTypeId, organizationId, companyId));
        out.put("financialYears_with_rows", jdbc.queryForList(
                "SELECT FinancialYearId, COUNT(1) AS Rows, MAX(CONVERT(int,DocNo)) AS MaxDocNo "
              + "FROM PurchaseOrder WHERE DocumentTypeId=? AND OrganizationId=? AND CompanyId=? "
              + "GROUP BY FinancialYearId ORDER BY FinancialYearId",
                documentTypeId, organizationId, companyId));
        out.put("config320_UsePurchaseOrderBalanceInNextFinYear", one(
                "SELECT ISNULL(dbo.GetConfigurationByConfigId(?,?,320),0)", organizationId, companyId));
        return out;
    }

    private Integer one(String sql, Object... args) {
        try { return jdbc.queryForObject(sql, Integer.class, args); }
        catch (Exception e) { return null; }
    }

    private static int firstNumber(List<Map<String, Object>> rows) {
        if (rows.isEmpty()) return 0;
        for (Object v : rows.get(0).values()) {
            if (v instanceof Number) return ((Number) v).intValue();
        }
        return 0;
    }

    /** BLL 0595 Save() - the procedure is chosen by Id, exactly as on the desktop. */
    public int save(Map<String, Object> model) {
        int id = intOf(model.get("Id"));
        Integer returned = exec(id == 0 ? P_INSERT : P_UPDATE, model);
        /* DAL: if (num > 0) obj.Id = num; else num = obj.Id; */
        return (returned != null && returned > 0) ? returned : id;
    }

    private Integer exec(String proc, Map<String, Object> model) {
        StringBuilder sql = new StringBuilder("EXEC dbo.").append(proc).append(' ');
        List<Object> args = new ArrayList<>();
        for (int i = 0; i < PARAMS.length; i++) {
            if (i > 0) sql.append(", ");
            String name = PARAMS[i];
            sql.append('@').append(name).append("=?");
            args.add(typed(name, model.get(name)));
        }
        /* -------------------------------------------------------------------------------------
         * WHY ProcExec AND NOT queryForList
         * -------------------------------------------------------------------------------------
         * This used queryForList, which goes through executeQuery() and therefore REQUIRES the
         * statement to produce a result set. Sp_PurchaseOrder_Insert ends with a SELECT that
         * hands back the new Id, so inserting worked. Sp_PurchaseOrder_Update returns nothing,
         * so every Update died with
         *
         *     com.microsoft.sqlserver.jdbc.SQLServerException:
         *     The statement did not return a result set.
         *
         * - before writing anything. Save worked, Update could not.
         *
         * The desktop has no such split. GenericProvider.SetProc ends every one of these calls
         * with ExecuteScalar(), which tolerates BOTH shapes: it takes the first column of the
         * first row when there is one, and returns null when there is not. ProcExec is this
         * port's equivalent - it walks the whole result/update-count chain and returns the first
         * scalar it finds, or null.
         *
         * This is the same defect class as A-RESULT-SET-WAS-GENERATED-FOR-UPDATE (25 call
         * sites), seen from the other side: there, update() was used on a procedure that DOES
         * return rows; here, queryForList() was used on one that does NOT. Both sweeps missed
         * this call because it matched neither `jdbcTemplate.update(` nor an inline "EXEC ..."
         * string - the SQL is built with a StringBuilder.
         * ------------------------------------------------------------------------------------- */
        return com.mst.repositories.support.ProcExec.call(jdbc, sql.toString(), args.toArray());
    }

    private static Object typed(String name, Object value) {
        if (value != null) return value;
        switch (name) {
            case "DeliveryStartDate": case "DocDate": case "EntryDate": case "ModifyDate":
            case "OrderDueDate": case "OrderExpiryDate": case "PostDate":
                return new SqlParameterValue(Types.TIMESTAMP, null);
            case "IsAproved": case "PostState": case "OrderTaxable":
            case "CashFreight": case "CreditFreight":
                return new SqlParameterValue(Types.BIT, null);
            case "CommAmount": case "CommRate":
            case "BrokeryRate": case "BrokeryUom": case "BrokeryAmount":
                return new SqlParameterValue(Types.FLOAT, null);
            case "OrderQty": case "OrderWeight": case "OrderAmount":
            case "ExchangeRate": case "FcyAmount":
                return new SqlParameterValue(Types.DECIMAL, null);
            case "BrokeryType": case "CommissionRemarks": case "CommissionType":
            case "DeliveryRemarks": case "DeliveryTerm": case "OrderStatus": case "OrderType":
            case "RemarksHeader": case "SupplierRefNo":
            case "AttachmentsValues": case "CustomAttachmentsValues":
                return new SqlParameterValue(Types.NVARCHAR, null);
            default:
                return new SqlParameterValue(Types.INTEGER, null);
        }
    }

    public static int intOf(Object v) {
        if (v instanceof Number) return ((Number) v).intValue();
        if (v == null) return 0;
        try { return Integer.parseInt(String.valueOf(v).trim()); } catch (NumberFormatException e) { return 0; }
    }

    /*
     * -----------------------------------------------------------------------------------------
     * CLR DEFAULTS - why this map exists
     * -----------------------------------------------------------------------------------------
     * PurchsaeOrder.cs:3285 begins the save with `PurchaseOrder po = new PurchaseOrder();` and
     * then assigns only the fields the form owns. GenericProvider.SetProc
     * (0207_Architecture.DAL.Common.GenericProvider.cs:293-302) afterwards sends EVERY non-virtual
     * property through SqlCommand.Parameters.AddWithValue("@" + Name, GetValue(obj)). There is no
     * null handling there and no "skip if unset" branch.
     *
     * So a property the form never assigns is still sent, carrying the CLR default of its DECLARED
     * TYPE: a double is 0.0, an int is 0, a bool is false. Only reference types (string) and the
     * single nullable property (DateTime? PostDate) are genuinely null on the desktop.
     *
     * This map previously blank-filled null for all 63 parameters, which is NOT what the desktop
     * sends. That is what produced, on the first real save:
     *
     *     Cannot insert the value NULL into column 'CommRate',
     *     table 'GoldenAcedb.dbo.PurchaseOrder'; column does not allow nulls. INSERT fails.
     *
     * PurchsaeOrder.cs:3322-3326 writes CommRate/CommAmount only inside `if (agent > 0)`, but
     * `double CommRate` is 0.0 from the constructor either way, and dbo.PurchaseOrder declares
     * CommRate and CommAmount as [float] NOT NULL. The guard decides whether to OVERWRITE the
     * default - it never suppresses the parameter.
     *
     * The same divergence - NULL stored where the desktop stores 0 - was reaching ten further
     * columns silently, because those happen to be nullable: CityId, RefrenenceParty,
     * SupplierCustomerIdStockParty, BillCalculateTypeId, BaseDocumentTypeId, PostUser,
     * BrokerAgentId, BrokeryRate, BrokeryUom, BrokeryAmount.
     *
     * Types are Architecture.Model.Inventory.PurchaseOrder
     * (1058_Architecture.Model.Inventory.PurchaseOrder.cs), declaration order preserved:
     *     bool      x5   -> false
     *     DateTime  x6   -> null, see the note below
     *     DateTime? x1   -> null   (PostDate)
     *     double    x5   -> 0.0d
     *     decimal   x5   -> BigDecimal.ZERO
     *     int       x30  -> 0
     *     string    x11  -> null
     *
     * The six non-nullable DateTime properties are deliberately left null here instead of being
     * given C#'s DateTime.MinValue: 0001-01-01 lies outside the range of a SQL Server [datetime]
     * column, so the desktop could not store it either. All six are assigned unconditionally by
     * the form (:3300, :3309, :3310, :3313, :3348, :3351), so their CLR default is never a value
     * the desktop actually sends. Null here is a guard that fails loudly if a caller forgets one.
     */
    private static final Map<String, Object> CLR_DEFAULTS = clrDefaults();

    private static Map<String, Object> clrDefaults() {
        Map<String, Object> m = new LinkedHashMap<>();
        for (String p : PARAMS) m.put(p, Integer.valueOf(0));            // int x30 - the majority
        for (String p : new String[] {                                    // bool x5
                "IsAproved", "PostState", "OrderTaxable", "CashFreight", "CreditFreight" })
            m.put(p, Boolean.FALSE);
        for (String p : new String[] {                                    // double x5
                "CommAmount", "CommRate", "BrokeryRate", "BrokeryUom", "BrokeryAmount" })
            m.put(p, Double.valueOf(0d));
        for (String p : new String[] {                                    // decimal x5
                "OrderQty", "OrderWeight", "OrderAmount", "ExchangeRate", "FcyAmount" })
            m.put(p, java.math.BigDecimal.ZERO);
        for (String p : new String[] {                                    // string x11
                "BrokeryType", "CommissionRemarks", "CommissionType", "DeliveryRemarks",
                "DeliveryTerm", "OrderStatus", "OrderType", "RemarksHeader", "SupplierRefNo",
                "AttachmentsValues", "CustomAttachmentsValues" })
            m.put(p, null);
        for (String p : new String[] {                                    // DateTime x6 + DateTime? x1
                "DeliveryStartDate", "DocDate", "EntryDate", "ModifyDate",
                "OrderDueDate", "OrderExpiryDate", "PostDate" })
            m.put(p, null);
        return java.util.Collections.unmodifiableMap(m);
    }

    /**
     * A fresh `new PurchaseOrder()` as the desktop constructs it at PurchsaeOrder.cs:3285 - every
     * value-type property already carrying its CLR default, not null.
     */
    public static Map<String, Object> blankModel() {
        return new LinkedHashMap<>(CLR_DEFAULTS);
    }
}
