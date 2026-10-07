package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.CmtrTransactionRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.math.BigDecimal;
import java.sql.Date;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/** DDL and BLL bridge for CmTr screen 518 (1702) and screen 519 (1704). */
@Service
public class CmtrTransactionService {
    public static final String LOADING_DIRECT = "loading-delivery-direct";
    public static final String COMMISSION_BILL = "commission-bill";

    private static final int LOADING_SCREEN = 518;
    private static final int BILL_SCREEN = 519;
    private static final int LOADING_DOCUMENT = 1702;
    private static final int BILL_DOCUMENT = 1704;
    private static final String LOADING_FORM = "CommTradeLoadingDeliveryDirect";
    private static final String BILL_FORM = "CommisionBillAgainstLoadingDelivery";

    private final CmtrTransactionRepository repository;
    private final CurrentUserContext context;
    private final DesktopReportRights rights;

    public CmtrTransactionService(CmtrTransactionRepository repository, CurrentUserContext context,
                                  DesktopReportRights rights) {
        this.repository = repository;
        this.context = context;
        this.rights = rights;
    }

    public void requireView(String type) { user(type, "View"); }

    public Map<String, Object> initialize(String type) {
        UserAccount user = user(type, "View");
        int documentType = documentType(type);
        LinkedHashMap<String, Object> dropdownParams = new LinkedHashMap<>();
        dropdownParams.put("OrganizationId", user.getOrganizationId());
        dropdownParams.put("CompanyId", user.getCompanyId());
        String dropdown = LOADING_DIRECT.equals(type)
                ? "[CmTr].[USP_GetDataForDropDownFromCommTradeLoadingDelivery]"
                : "[CmTr].[USP_GetDataForDropDownFromCommTradeTransaction]";

        Map<String, Object> result = new LinkedHashMap<>();
        result.put("lookups", repository.first(dropdown, dropdownParams));
        result.put("documentNo", nextDocumentNo(type, user, documentType));
        result.put("documentTypeId", documentType);
        result.put("screenId", screenId(type));
        result.put("today", LocalDate.now().toString());
        result.put("permissions", Map.of(
                "save", allowed(user, type, "Save"),
                "update", allowed(user, type, "Update"),
                "delete", allowed(user, type, "Delete"),
                "print", allowed(user, type, "Print"),
                "gridPrint", allowed(user, type, "Grid Print"),
                "gridExport", allowed(user, type, "Grid Export")));
        return result;
    }

    public List<Map<String, Object>> history(String type, Map<String, String> filters) {
        UserAccount user = user(type, "View");
        LinkedHashMap<String, Object> p = tenancy(user);
        p.put("DocumentTypeId", documentType(type));
        p.put("NoOfRecords", 0);
        boolean viewAll = allowed(user, type, "CanView AllRecord");
        p.put("CanViewAllRecord", viewAll);
        if (!viewAll) p.put("EntryUserId", user.getId());
        addDate(p, "FromDate", filters.get("fromDate"));
        addDate(p, "ToDate", filters.get("toDate"));
        addNumber(p, "DocNoFrom", filters.get("fromDocNo"));
        addNumber(p, "DocNoTo", filters.get("toDocNo"));
        p.put("Activity", "FormHistory");
        addScreenNameWhenSupported(type, p);
        return repository.first(getAllProcedure(type), p);
    }

    public Map<String, Object> record(String type, int id) {
        user(type, "View");
        if (id <= 0) throw new IllegalArgumentException("Select a record first.");
        LinkedHashMap<String, Object> p = tenancy(context.requireAccountingUser());
        p.put("Id", id);
        p.put("DocumentTypeId", documentType(type));
        p.put("Activity", "ReadById");
        addScreenNameWhenSupported(type, p);
        List<List<Map<String, Object>>> sets = repository.execute(getAllProcedure(type), p);
        Map<String, Object> result = new LinkedHashMap<>();
        result.put("sets", sets);
        result.put("header", sets.isEmpty() || sets.get(0).isEmpty() ? Map.of() : sets.get(0).get(0));
        result.put("details", sets.size() < 2 ? List.of() : sets.get(1));
        return result;
    }

    /** Loads candidate 1702 documents for the desktop 1704 "Load From Loading Delivery" action. */
    public List<Map<String, Object>> pendingLoading(String type, Map<String, String> filters) {
        UserAccount user = user(type, "View");
        if (!COMMISSION_BILL.equals(type)) throw new IllegalArgumentException("Loading documents are only available on the Commission Bill form.");
        LinkedHashMap<String, Object> p = tenancy(user);
        p.put("DocumentTypeId", LOADING_DOCUMENT);
        p.put("Id", number(filters.get("id")));
        addDate(p, "FromDate", filters.get("fromDate"));
        addDate(p, "ToDate", filters.get("toDate"));
        addNumber(p, "FromDocNo", filters.get("fromDocNo"));
        addNumber(p, "ToDocNo", filters.get("toDocNo"));
        addNumber(p, "ItemId", filters.get("itemId"));
        if (notBlank(filters.get("loadingDeliveryIds"))) p.put("LoadingDeliveryIds", filters.get("loadingDeliveryIds"));
        return repository.first("[CmTr].[USP_PendingCommTradeLoadingDeliveryForBill]", p);
    }

    @Transactional
    public Map<String, Object> save(String type, Map<String, Object> body) {
        int id = number(body.get("id"));
        UserAccount user = user(type, id > 0 ? "Update" : "Save");
        Map<String, Object> header = object(body.get("header"));
        List<Map<String, Object>> details = rows(body.get("details"));
        if (details.isEmpty()) throw new IllegalArgumentException("Grid Record Not Found");
        require(header.get("docDate"), "Document Date");
        if (COMMISSION_BILL.equals(type)) {
            if (number(header.get("commissionAgentId")) <= 0) throw new IllegalArgumentException("Commission Agent Field is Required");
            if (decimal(header.get("billAmount")).signum() <= 0) throw new IllegalArgumentException("Bill Amount Field is Required");
        } else {
            for (int i = 0; i < details.size(); i++) {
                Map<String, Object> row = details.get(i);
                if (number(row.get("itemId")) <= 0 || number(row.get("supplierId")) <= 0 || number(row.get("customerId")) <= 0) {
                    throw new IllegalArgumentException("Select Item, Supplier and Customer in detail row " + (i + 1) + ".");
                }
            }
        }

        int savedId = LOADING_DIRECT.equals(type)
                ? saveLoadingDelivery(user, id, header, details, body)
                : saveCommissionBill(user, id, header, details, body);
        return Map.of("success", true, "id", savedId,
                "message", id == 0 ? "Data Save Successfully.... " : "Data Update Successfully.... ");
    }

    public Map<String, Object> delete(String type, int id) {
        UserAccount user = user(type, "Delete");
        if (id <= 0) throw new IllegalArgumentException("Select a record first.");
        LinkedHashMap<String, Object> p = new LinkedHashMap<>();
        p.put("EntryUserId", user.getId());
        p.put("Id", id);
        p.put("DocumentTypeId", documentType(type));
        p.put("Activity", "DeleteById");
        addScreenNameWhenSupported(type, p);
        repository.execute(getAllProcedure(type), p);
        return Map.of("success", true, "message", "Data Deleted Successfully.... ");
    }

    private int saveLoadingDelivery(UserAccount user, int id, Map<String, Object> h,
                                    List<Map<String, Object>> details, Map<String, Object> body) {
        Timestamp now = new Timestamp(System.currentTimeMillis());
        LinkedHashMap<String, Object> p = baseHeader(user, id, h, LOADING_DOCUMENT, LOADING_FORM, now);
        p.put("ManualRefNo", value(h, "manualRefNo"));
        int savedId = executeHeader(id, "[CmTr].[USP_CommTradeLoadingDelivery_Insert]",
                "[CmTr].[USP_CommTradeLoadingDelivery_Update]", p);

        int sort = 1;
        for (Map<String, Object> row : details) {
            LinkedHashMap<String, Object> d = new LinkedHashMap<>();
            map(d, row, "Id", "id"); d.put("CommTradeLoadingDeliveryId", savedId);
            map(d,row,"DocDateDt","docDateDt");map(d,row,"VehicleNo","vehicleNo");map(d,row,"BiltyNo","biltyNo");map(d,row,"BiltyDate","biltyDate");
            map(d,row,"VehicleGrossWeight","vehicleGrossWeight");map(d,row,"VehicleQty","vehicleQty");map(d,row,"CommissionAgentId","commissionAgentId");
            map(d,row,"BrokerId","brokerId");map(d,row,"TransporterId","transporterId");map(d,row,"BiltyFreight","biltyFreight");map(d,row,"BiltyExpense","biltyExpense");
            d.put("SortNo", number(value(row,"sortNo")) == 0 ? sort : number(value(row,"sortNo")));
            map(d,row,"OrderId","orderId");map(d,row,"OrderDetailId","orderDetailId");map(d,row,"ItemId","itemId");map(d,row,"UomId","uomId");
            map(d,row,"CropYearId","cropYearId");map(d,row,"PackingTypeId","packingTypeId");map(d,row,"ItemSupQty","itemSupQty");map(d,row,"ItemSupWeight","itemSupWeight");
            map(d,row,"Qty","qty");map(d,row,"SupplierId","supplierId");map(d,row,"SupGrossWeight","supGrossWeight");map(d,row,"SupPckMtrlWeight","supPckMtrlWeight");
            map(d,row,"SupAdLsWt","supAdLsWt");map(d,row,"SupNetWeight","supNetWeight");map(d,row,"SupRemarks","supRemarks");map(d,row,"CustomerId","customerId");
            map(d,row,"CusGrossWeight","cusGrossWeight");map(d,row,"CusPckMtrlWeight","cusPckMtrlWeight");map(d,row,"CusAdLsWt","cusAdLsWt");map(d,row,"CusNetWeight","cusNetWeight");
            map(d,row,"CusRemarks","cusRemarks");d.put("ActionTypeId", actionType(row,id));map(d,row,"RowVersion","rowVersion");map(d,row,"CusRate","cusRate");map(d,row,"SupRate","supRate");
            repository.execute("[CmTr].[USP_CommTradeLoadingDeliveryDetail_Insert]", d); sort++;
        }
        insertRows("[CmTr].[USP_CommTradeLoadingDeliveryRevExp_Insert]", savedId, rows(body.get("revenueExpenses")), "CommTradeLoadingDeliveryId",
                new String[][]{{"Id","id"},{"AccountId","accountId"},{"Qty","qty"},{"Rate","rate"},{"DebitAmt","debitAmt"},{"CreditAmt","creditAmt"},{"ReRemarks","remarks"},{"SortNo","sortNo"},{"SupplierCustomerId","supplierCustomerId"},{"SupplierCustomerIdDr","supplierCustomerIdDr"},{"ActionTypeId","actionTypeId"},{"RowVersion","rowVersion"}});
        insertRows("[CmTr].[USP_CommTradeLoadingDeliverySupplierExp_Insert]", savedId, rows(body.get("supplierExpenses")), "CommTradeLoadingDeliveryId",
                new String[][]{{"Id","id"},{"ExpenseAccountId","expenseAccountId"},{"Qty","qty"},{"ExpenseRate","expenseRate"},{"Remarks","remarks"},{"SortNo","sortNo"},{"SupplierCustomerId","supplierCustomerId"}});
        insertRows("[CmTr].[USP_CommTradeLoadingDeliveryPaymentTermsDetail_Insert]", savedId, rows(body.get("paymentTerms")), "CommTradeLoadingDeliveryId",
                new String[][]{{"Id","id"},{"SortNo","sortNo"},{"SupplierCustomerTypeId","supplierCustomerTypeId"},{"SupplierCustomerId","supplierCustomerId"},{"PaymentTermId","paymentTermId"},{"DueDays","dueDays"},{"DueDate","dueDate"},{"PrcntOfTotal","percentOfTotal"},{"PaymentRemarks","remarks"}});
        insertRows("[CmTr].[USP_CommTradeLoadingDeliveryPmOther_Insert]", savedId, rows(body.get("packingMaterials")), "CommTradeLoadingDeliveryId",
                new String[][]{{"Id","id"},{"ItemId","itemId"},{"UomId","uomId"},{"PackingTypeId","packingTypeId"},{"ItemSize","itemSize"},{"ItemWeight","itemWeight"},{"Qty","qty"},{"Rate","rate"},{"AdLs","adLs"},{"Amount","amount"},{"CreditAccountId","creditAccountId"},{"DebitAccountId","debitAccountId"},{"StockTypeId","stockTypeId"},{"WarehouseId","warehouseId"}});
        return savedId;
    }

    private int saveCommissionBill(UserAccount user, int id, Map<String, Object> h,
                                   List<Map<String, Object>> details, Map<String, Object> body) {
        Timestamp now = new Timestamp(System.currentTimeMillis());
        LinkedHashMap<String, Object> p = baseHeader(user, id, h, BILL_DOCUMENT, BILL_FORM, now);
        p.put("ManualBillNo", value(h,"manualBillNo"));p.put("CommissionAgentId", number(value(h,"commissionAgentId")));
        p.put("CalculationParameterId", number(value(h,"calculationParameterId")));p.put("CommRate", decimal(value(h,"commRate")));
        p.put("CommAmount", decimal(value(h,"commAmount")));p.put("BrokeryRate", decimal(value(h,"brokeryRate")));
        p.put("BrokeryAmount", decimal(value(h,"brokeryAmount")));p.put("SubBrokerId", number(value(h,"subBrokerId")));
        p.put("SubBrokeryRate", decimal(value(h,"subBrokeryRate")));p.put("SubBrokeryAmount", decimal(value(h,"subBrokeryAmount")));
        p.put("BiltyNos", value(h,"biltyNos"));p.put("BiltyDates", value(h,"biltyDates"));p.put("VehicleNos", value(h,"vehicleNos"));
        p.put("CurrencyId", number(value(h,"currencyId")));p.put("ExchangeRate", decimalOr(value(h,"exchangeRate"), BigDecimal.ONE));
        p.put("FcyAmount", decimal(value(h,"fcyAmount")));p.put("BillAmount", decimal(value(h,"billAmount")));
        p.put("BrokerAgentId", number(value(h,"brokerAgentId")));p.put("DifferenceAccountId", number(value(h,"differenceAccountId")));
        p.put("PurchaseTaxCreditAccountId", number(value(h,"purchaseTaxCreditAccountId")));
        p.put("SaleTaxCreditAccountId", number(value(h,"saleTaxCreditAccountId")));
        int savedId = executeHeader(id, "[CmTr].[USP_CommTradeTransactionHeader_Insert]",
                "[CmTr].[USP_CommTradeTransactionHeader_Update]", p);

        insertRows("[CmTr].[USP_CommTradeTransactionDetail_Insert]", savedId, details, "CommTradeTransactionHeaderId",
                new String[][]{{"Id","id"},{"dDocDate","dDocDate"},{"CommTradeOrderId","commTradeOrderId"},{"CommTradeOrderDetailId","commTradeOrderDetailId"},{"CommTradeLoadingDeliveryId","commTradeLoadingDeliveryId"},{"CommTradeLoadingDeliveryDetailId","commTradeLoadingDeliveryDetailId"},{"SortNo","sortNo"},{"ItemId","itemId"},{"UomId","uomId"},{"CropYearId","cropYearId"},{"PackingTypeId","packingTypeId"},{"Qty","qty"},{"CustomerId","customerId"},{"CusGrossWeight","cusGrossWeight"},{"CusPckMtrlWeight","cusPckMtrlWeight"},{"CusAdLsWt","cusAdLsWt"},{"CusNetWeight","cusNetWeight"},{"CusPaymentTermsId","cusPaymentTermsId"},{"CusDueDays","cusDueDays"},{"CusDeliveryTerm","cusDeliveryTerm"},{"CusRateWithoutAddLess","cusRateWithoutAddLess"},{"CusRateAddLess","cusRateAddLess"},{"CusRate","cusRate"},{"CusRateUomId","cusRateUomId"},{"CusAmount","cusAmount"},{"CustFcyAmount","custFcyAmount"},{"CusTaxNameId","cusTaxNameId"},{"CusTaxPercent","cusTaxPercent"},{"CusTaxAmount","cusTaxAmount"},{"CusNetAmount","cusNetAmount"},{"CusRemarks","cusRemarks"},{"SupplierId","supplierId"},{"SupGrossWeight","supGrossWeight"},{"SupPckMtrlWeight","supPckMtrlWeight"},{"SupAdLsWt","supAdLsWt"},{"SupNetWeight","supNetWeight"},{"SupDueDays","supDueDays"},{"SupRateWithoutAddLess","supRateWithoutAddLess"},{"SupRateAddLess","supRateAddLess"},{"SupRate","supRate"},{"SupRateUomId","supRateUomId"},{"SupAmount","supAmount"},{"SupFcyAmount","supFcyAmount"},{"SupTaxNameId","supTaxNameId"},{"SupTaxPercent","supTaxPercent"},{"SupTaxAmount","supTaxAmount"},{"SupNetAmount","supNetAmount"},{"SupRemarks","supRemarks"},{"CurrencyId","currencyId"},{"ExchangeRate","exchangeRate"},{"ActionTypeId","actionTypeId"},{"RowVersion","rowVersion"},{"BiltyNo","biltyNo"},{"BiltyDate","biltyDate"},{"VehicleNo","vehicleNo"},{"VehicleQty","vehicleQty"},{"VehicleGrossWeight","vehicleGrossWeight"},{"BiltyFreight","biltyFreight"}});
        insertRows("[CmTr].[USP_CommTradeTransactionPmOther_Insert]", savedId, rows(body.get("packingMaterials")), "CommTradeTransactionHeaderId",
                new String[][]{{"Id","id"},{"LoadingDeliveryId","loadingDeliveryId"},{"LoadingDeliveryPmOtherId","loadingDeliveryPmOtherId"},{"ItemId","itemId"},{"UomId","uomId"},{"PackingMaterialId","packingMaterialId"},{"ItemSize","itemSize"},{"ItemWeight","itemWeight"},{"Qty","qty"},{"Rate","rate"},{"AdLs","adLs"},{"Amount","amount"},{"CreditAccountId","creditAccountId"},{"DebitAccountId","debitAccountId"},{"StockTypeId","stockTypeId"},{"WarehouseId","warehouseId"},{"ItemConditionId","itemConditionId"}});
        insertRows("[CmTr].[USP_CommTradeTransactionRevExp_Insert]", savedId, rows(body.get("revenueExpenses")), "CommTradeTransactionHeaderId",
                new String[][]{{"Id","id"},{"AccountId","accountId"},{"Qty","qty"},{"Rate","rate"},{"DebitAmt","debitAmt"},{"CreditAmt","creditAmt"},{"ReRemarks","remarks"},{"SortNo","sortNo"},{"SupplierCustomerId","supplierCustomerId"},{"LoadingDeliveryId","loadingDeliveryId"},{"LoadingDeliveryRevExpId","loadingDeliveryRevExpId"},{"TypeId","typeId"}});
        insertRows("[CmTr].[USP_CommTradeTransactionCommBrokery_Insert]", savedId, rows(body.get("commissionBrokery")), "CommTradeTransactionHeaderId",
                new String[][]{{"Id","id"},{"CommissionAgentId","commissionAgentId"},{"ChargesIncentiveId","chargesIncentiveId"},{"RevenueExpenseId","revenueExpenseId"},{"CalcParameterId","calculationParameterId"},{"Rate","rate"},{"Amount","amount"},{"SupplierCustomerId","supplierCustomerId"},{"SortNo","sortNo"},{"CommissionAgentGLId","commissionAgentGlId"}});
        insertRows("[CmTr].[USP_CommTradeTransactionSupplierSummary_Insert]", savedId, rows(body.get("supplierSummary")), "CommTradeTransactionHeaderId",
                new String[][]{{"Id","id"},{"SortNo","sortNo"},{"SupplierId","supplierId"},{"Weight","weight"},{"ItemAmount","itemAmount"},{"PackingAmount","packingAmount"},{"OtherExpenses","otherExpenses"},{"BillAmount","billAmount"}});
        List<Map<String, Object>> billPaymentRows = new ArrayList<>(rows(body.get("paymentTerms")));
        billPaymentRows.addAll(rows(body.get("supplierPaymentTerms")));
        insertRows("[CmTr].[USP_CommTradeTransactionPaymentTermsDetail_Insert]", savedId, billPaymentRows, "CommTradeTransactionHeaderId",
                new String[][]{{"Id","id"},{"SortNo","sortNo"},{"SupplierCustomerTypeId","supplierCustomerTypeId"},{"SupplierCustomerId","supplierCustomerId"},{"PaymentTermId","paymentTermId"},{"DueDays","dueDays"},{"DueDate","dueDate"},{"PrcntOfTotal","percentOfTotal"},{"Amount","amount"},{"PaymentRemarks","remarks"},{"CommTradeLoadingDeliveryId","commTradeLoadingDeliveryId"},{"LoadingDeliveryPaymentDetailId","loadingDeliveryPaymentDetailId"}});
        insertRows("[CmTr].[USP_CommTradeTransactionVehicleFreightDetail_Insert]", savedId, rows(body.get("vehicleFreight")), "CommTradeTransactionHeaderId",
                new String[][]{{"Id","id"},{"SortNo","sortNo"},{"LoadingDeliveryId","loadingDeliveryId"},{"CustomerId","customerId"},{"CreditAccountId","creditAccountId"},{"VehicleNo","vehicleNo"},{"BiltyNo","biltyNo"},{"VehicleQty","vehicleQty"},{"BiltyFreight","biltyFreight"},{"NetPaid","netPaid"},{"Remarks","remarks"}});
        return savedId;
    }

    private LinkedHashMap<String, Object> baseHeader(UserAccount user, int id, Map<String, Object> h,
                                                      int documentType, String formName, Timestamp now) {
        LinkedHashMap<String, Object> p = new LinkedHashMap<>();
        p.put("DocumentTypeId", documentType);p.put("Id", id);p.put("DocDate", date(value(h,"docDate"),"Document Date"));
        p.put("DocNo", number(value(h,"docNo")));p.put("RemarksHeader", value(h,"remarksHeader"));
        p.put("OrganizationId", user.getOrganizationId());p.put("CompanyId", user.getCompanyId());p.put("BranchesId", user.getBranchesId());
        p.put("FinancialYearId", context.currentFinancialYearId());p.put("EntryUserId", user.getId());p.put("EntryDate", now);
        p.put("ModifyUserId", user.getId());p.put("ModifyDate", now);p.put("IsApproved", false);p.put("ApprovedUserId", 0);
        p.put("ApprovedDate", null);p.put("ActionId", id == 0 ? 1 : 2);p.put("RowVersion", null);p.put("ScreenName", formName);
        return p;
    }

    private int executeHeader(int id, String insertProcedure, String updateProcedure, LinkedHashMap<String, Object> parameters) {
        List<Map<String, Object>> rows = repository.first(id == 0 ? insertProcedure : updateProcedure, parameters);
        if (id > 0) return id;
        if (rows.isEmpty() || rows.get(0).isEmpty()) throw new IllegalStateException("The desktop procedure did not return the saved record id.");
        int saved = number(rows.get(0).values().iterator().next());
        if (saved <= 0) throw new IllegalStateException("The desktop procedure did not return the saved record id.");
        return saved;
    }

    private void insertRows(String procedure, int parentId, List<Map<String, Object>> rows, String parentKey, String[][] fields) {
        int sort = 1;
        for (Map<String, Object> row : rows) {
            LinkedHashMap<String, Object> p = new LinkedHashMap<>();
            p.put(parentKey, parentId);
            for (String[] field : fields) map(p, row, field[0], field[1]);
            if (containsParameter(fields, "ActionTypeId") && !p.containsKey("ActionTypeId")) {
                p.put("ActionTypeId", actionType(row, parentId));
            }
            if (!p.containsKey("SortNo")) p.put("SortNo", sort);
            repository.execute(procedure, p);
            sort++;
        }
    }

    private int nextDocumentNo(String type, UserAccount user, int documentType) {
        LinkedHashMap<String, Object> p = tenancy(user);p.put("DocumentTypeId", documentType);p.put("Activity", "GenerateCode");
        List<Map<String, Object>> rows = repository.first(getAllProcedure(type), p);
        if (rows.isEmpty()) return 0;
        return number(value(rows.get(0), "DocNo"));
    }

    private LinkedHashMap<String, Object> tenancy(UserAccount user) {
        LinkedHashMap<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", user.getOrganizationId());p.put("CompanyId", user.getCompanyId());
        p.put("BranchesId", user.getBranchesId());p.put("FinancialYearId", context.currentFinancialYearId());
        return p;
    }

    private UserAccount user(String type, String action) {
        int screen = screenId(type);
        UserAccount user = context.requireAccountingUser();
        rights.require(user, screen, action);
        return user;
    }

    private boolean allowed(UserAccount user, String type, String action) {
        try { rights.require(user, screenId(type), action);return true; }
        catch (AccessDeniedException ex) { return false; }
    }

    private static int screenId(String type) {
        if (LOADING_DIRECT.equals(type)) return LOADING_SCREEN;
        if (COMMISSION_BILL.equals(type)) return BILL_SCREEN;
        throw new IllegalArgumentException("Unknown Commission Trading transaction.");
    }

    private static int documentType(String type) { return LOADING_DIRECT.equals(type) ? LOADING_DOCUMENT : BILL_DOCUMENT; }
    private static String formName(String type) { return LOADING_DIRECT.equals(type) ? LOADING_FORM : BILL_FORM; }
    private static String getAllProcedure(String type) {
        return LOADING_DIRECT.equals(type) ? "[CmTr].[USP_CommTradeLoadingDelivery_GetAllMethod]"
                : "[CmTr].[USP_CommTradeTransaction_GetAllMethod]";
    }

    private static void addScreenNameWhenSupported(String type, Map<String, Object> parameters) {
        // The legacy transaction header procedure has no @ScreenName parameter.
        // The loading/delivery procedure does, and the desktop passes the form name there.
        if (LOADING_DIRECT.equals(type)) parameters.put("ScreenName", formName(type));
    }

    private static void map(Map<String, Object> target, Map<String, Object> source, String parameter, String key) {
        Object value = value(source, key);
        if (value != null && !(value instanceof String && ((String) value).isBlank())) target.put(parameter, value);
    }

    private static boolean containsParameter(String[][] fields, String parameter) {
        for (String[] field : fields) if (parameter.equals(field[0])) return true;
        return false;
    }

    private static void addDate(Map<String, Object> p, String key, String raw) {
        if (notBlank(raw)) p.put(key, date(raw, key));
    }

    private static void addNumber(Map<String, Object> p, String key, String raw) {
        if (notBlank(raw)) p.put(key, number(raw));
    }

    private static Object value(Map<?, ?> map, String key) {
        if (map == null) return null;
        if (map.containsKey(key)) return map.get(key);
        for (Map.Entry<?, ?> entry : map.entrySet()) if (String.valueOf(entry.getKey()).equalsIgnoreCase(key)) return entry.getValue();
        return null;
    }

    @SuppressWarnings("unchecked") private static Map<String, Object> object(Object value) {
        return value instanceof Map<?, ?> ? (Map<String, Object>) value : Map.of();
    }

    private static List<Map<String, Object>> rows(Object value) {
        List<Map<String, Object>> result = new ArrayList<>();
        if (value instanceof List<?> list) for (Object row : list) if (row instanceof Map<?, ?>) result.add(object(row));
        return result;
    }

    private static void require(Object value, String label) {
        if (value == null || String.valueOf(value).isBlank()) throw new IllegalArgumentException(label + " Field Required");
    }

    private static Date date(Object value, String label) {
        if (value instanceof Date) return (Date) value;
        if (value == null || String.valueOf(value).isBlank()) throw new IllegalArgumentException(label + " is required.");
        try { return Date.valueOf(String.valueOf(value).substring(0, 10)); }
        catch (RuntimeException ex) { throw new IllegalArgumentException(label + " is invalid."); }
    }

    private static int number(Object value) {
        if (value instanceof Number) return ((Number) value).intValue();
        try { return value == null || String.valueOf(value).isBlank() ? 0 : Integer.parseInt(String.valueOf(value)); }
        catch (NumberFormatException ex) { return 0; }
    }

    private static BigDecimal decimal(Object value) {
        if (value instanceof BigDecimal) return (BigDecimal) value;
        try { return value == null || String.valueOf(value).isBlank() ? BigDecimal.ZERO : new BigDecimal(String.valueOf(value)); }
        catch (NumberFormatException ex) { throw new IllegalArgumentException("A numeric value is invalid."); }
    }

    private static BigDecimal decimalOr(Object value, BigDecimal fallback) {
        return value == null || String.valueOf(value).isBlank() ? fallback : decimal(value);
    }

    private static int actionType(Map<String, Object> row, int parentId) {
        int action = number(value(row,"actionTypeId"));
        if (action != 0) return action;
        return number(value(row,"id")) > 0 && parentId > 0 ? 2 : 1;
    }

    private static boolean notBlank(String value) { return value != null && !value.isBlank(); }
}
