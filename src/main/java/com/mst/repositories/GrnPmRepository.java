package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.ProcExec;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.util.*;
import java.util.concurrent.ConcurrentHashMap;
import org.springframework.http.HttpStatus;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.PreparedStatementCallback;
import org.springframework.stereotype.Repository;
import org.springframework.web.server.ResponseStatusException;

import static com.mst.repositories.PurchaseOrderPmRepository.col;
import static com.mst.repositories.PurchaseOrderPmRepository.intOf;

/**
 * Screen 500 "Goods Receipt Notes PM" - {@code GrnPackingMaterial.cs} (4,632 lines, DocumentTypeId 701).
 *
 * <pre>
 * Form method                        BLL / global                                     Procedure (@Activity)
 * ---------------------------------  -----------------------------------------------  ------------------------------------------------
 * DocumentNoDbCall            :915   InvGrn.GenerateInvGrnCode                        Sp_InvGRN_GetAllMethod (GenerateInvGrnCode)
 * VehicleTypesBind            :1051  VehicleType.GetAll - its Activity list is built   Sp_VehicleType_GetAllMethod  (NO parameters)
 *                                    and thrown away, the procedure takes none
 * DeliveryTerm                :1063  DeliveryTerm.FormHistory                         USP_DeliveryTerm_GetAllMethod (FormHistory)
 * PendingGpDataDbCall         :1138  GatePassInward.PendingGatePassforGRNPackingMat.  USP_PendingGatePassforGRNPackingMaterial
 * TransporterDtFillFromGlobal :984   feature 4 ? VendorsAndCustomersForTransporters   USP_GetVendorsAndCustomersForTransporter
 *                                              : AllAccountsWithCustomGroupId          USP_GETAllAccountsFromCustomGroups
 * GridComboBind               :2188  globalAllCities / CropYear / ItemConditions      USP_City_GetAllWithCountryAndTehsil,
 *                                                                                     Sp_InvCropYear_GetAllMethod, V_ItemCondition
 * racksWithWarehouseAndItems         GetRacksWithWarehouseByItemId(org, comp, branch) usp_getRackswithWarehouseByItemId
 * HistoryComboFill            :890   InvGrn.GetDataForDropDownFromGrn                 USP_GetDataForDropDownFromGrn
 * BindGridByOrderId           :2085  PurchaseOrder.PurchaseOrderLoaderForPackingMat.  USP_GetPurchaseOrderdLoader
 * LoadPurchaseOrderPM (dialog)       same + SupplierCustomer.GetSupplierCustomerFrom.. Sp_SupplierCustomer_GetAllMethod
 * HistoryGridFill             :1359  InvGrn.GetHisoty                                 Sp_InvGRN_GetAllMethod (GRNFormHistory)
 * ReadById / BindHistoryDetail       InvGrn.GetByID                                   Sp_InvGRN_GetAllMethod (ReadByID) +
 *                                                                                     Sp_InvGrnDetail_GetAllMethod (ReadByInvGrnIDStore)
 * btnDelete_Click             :739   InvPurchaseInvoice.RemoveByID                    Sp_InvoicesVouchersandStocksDelete
 * </pre>
 *
 * The write is DAL {@code InvGrn.SetData} (0429): header proc, one Sp_InvGrnDetail_Insert per row
 * (the update proc has already deleted every old row), attachments, Sp_InventoryTransactions_GetALLMethod,
 * then - DocumentTypeId 701 is neither 217 nor 46/48 - usp_StockInTransitUpdate_StockEvaluationAndVoucherInsertFromGrn.
 */
@Repository
public class GrnPmRepository {

    public static final int DOCUMENT_TYPE_ID = 701;
    public static final int ORDER_DOCUMENT_TYPE_ID = 700;
    public static final int GATE_PASS_DOCUMENT_TYPE_ID = 51;
    /** base.Name - ScreenName on the header and the key attachments are stored under. */
    public static final String SCREEN = "GrnPackingMaterial";

    private final JdbcTemplate jdbc;
    private final Map<String, List<String>> parameters = new ConcurrentHashMap<>();

    public GrnPmRepository(JdbcTemplate jdbc) {
        this.jdbc = jdbc;
    }

    // ================================================================== lookups

    public int nextDocNo(UserAccount u, int financialYearId) {
        StringBuilder sql = new StringBuilder("EXEC dbo.Sp_InvGRN_GetAllMethod @OrganizationId=?, @CompanyId=?, @DocumentTypeId=?");
        List<Object> a = new ArrayList<>(List.of(u.getOrganizationId(), u.getCompanyId(), DOCUMENT_TYPE_ID));
        if (financialYearId != 0) { sql.append(", @FinancialYearId=?"); a.add(financialYearId); }
        if (u.getBranchesId() != 0) { sql.append(", @BranchesId=?"); a.add(u.getBranchesId()); }
        sql.append(", @Activity=?"); a.add("GenerateInvGrnCode");
        var rows = jdbc.queryForList(sql.toString(), a.toArray());
        return rows.isEmpty() ? 0 : intOf(col(rows.get(0), "DocNo"));
    }

    public List<Map<String, Object>> vehicleTypes() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : jdbc.queryForList("EXEC dbo.Sp_VehicleType_GetAllMethod")) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", intOf(col(r, "Id")));
            m.put("VehicleDescription", col(r, "VehicleDescription"));
            out.add(m);
        }
        return out;
    }

    public List<Map<String, Object>> deliveryTerms() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : jdbc.queryForList("EXEC [dbo].[USP_DeliveryTerm_GetAllMethod] @Activity=?", "FormHistory")) {
            out.add(two(col(r, "Id"), col(r, "Description")));
        }
        return out;
    }

    /**
     * TransporterDtFillFromGlobal :984. Columns in dtTransporter order: Id, AccountTitle, AccountCode,
     * SupplierCustomerId. With feature 4 the value member is SupplierCustomerId (:1033), otherwise Id.
     */
    public List<Map<String, Object>> transporters(UserAccount u, boolean subsidiary) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (subsidiary) {
            for (var r : jdbc.queryForList("EXEC [dbo].[USP_GetVendorsAndCustomersForTransporter] @OrganizationId=?, @CompanyId=?",
                    u.getOrganizationId(), u.getCompanyId())) {
                out.add(transporter(intOf(col(r, "GlAccountId")), col(r, "CompanyName"), col(r, "PartyCode"), intOf(col(r, "Id"))));
            }
            return out;
        }
        Set<Integer> excluded = Set.of(2, 4, 10, 11, 12, 13, 14, 15, 20, 21, 22);
        Set<Integer> seen = new HashSet<>();
        for (var r : jdbc.queryForList("EXEC [dbo].[USP_GETAllAccountsFromCustomGroups] @OrganizationId=?, @CompanyId=?",
                u.getOrganizationId(), u.getCompanyId())) {
            if (excluded.contains(intOf(col(r, "AccountTypeId")))) continue;
            int id = intOf(col(r, "ChartOfAccountId"));
            if (seen.add(id)) out.add(transporter(id, col(r, "AccountTitle"), col(r, "AccountCode"), 0));
        }
        return out;
    }

    private static Map<String, Object> transporter(int id, Object title, Object code, int supCustId) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("Id", id);
        m.put("AccountTitle", title);
        m.put("AccountCode", code);
        m.put("SupplierCustomerId", supCustId);
        return m;
    }

    public List<Map<String, Object>> cities(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : jdbc.queryForList("EXEC [dbo].[USP_City_GetAllWithCountryAndTehsil] @OrganizationId=?, @CompanyId=?",
                u.getOrganizationId(), u.getCompanyId())) {
            out.add(two(col(r, "Id"), col(r, "CityName")));
        }
        return out;
    }

    public List<Map<String, Object>> cropYears(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : jdbc.queryForList("EXEC dbo.Sp_InvCropYear_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), "ReadAll")) {
            out.add(two(col(r, "Id"), col(r, "CropYear")));
        }
        return out;
    }

    /** GridComboBind :2196 - globalItemConditions without Id 4. */
    public List<Map<String, Object>> itemConditions() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : jdbc.queryForList("SELECT * FROM dbo.V_ItemCondition")) {
            int id = intOf(col(r, "Id"));
            if (id != 4) out.add(two(id, col(r, "ConditionStatus")));
        }
        return out;
    }

    /** racksWithWarehouseAndItems - loaded for the user's branch (DatatableHelper :341, :422). */
    public List<Map<String, Object>> racks(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : jdbc.queryForList("EXEC dbo.usp_getRackswithWarehouseByItemId @OrganizationId=?, @CompanyId=?, @BranchId=?",
                u.getOrganizationId(), u.getCompanyId(), u.getBranchesId())) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", intOf(col(r, "Id")));
            m.put("RackName", col(r, "RackName"));
            m.put("WarehouseId", intOf(col(r, "invWarehouseId")));
            m.put("WareHouseName", col(r, "WareHouseName"));
            m.put("ItemId", intOf(col(r, "ItemId")));
            m.put("BaseRackId", intOf(col(r, "BaseRackId")));
            out.add(m);
        }
        return out;
    }

    /** HistoryComboFill :890 - rows whose Activity is "Supplier". FinancialYearId 0 and Activity null are not sent. */
    public List<Map<String, Object>> historySuppliers(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : jdbc.queryForList(
                "EXEC [dbo].[USP_GetDataForDropDownFromGrn] @OrganizationId=?, @CompanyId=?, @DocumentTypeIds=?, @BranchesIds=?",
                u.getOrganizationId(), u.getCompanyId(), String.valueOf(DOCUMENT_TYPE_ID), String.valueOf(u.getBranchesId()))) {
            if ("Supplier".equals(Objects.toString(col(r, "Activity"), ""))) out.add(two(col(r, "Id"), col(r, "ReferenceName")));
        }
        return out;
    }

    /** USP_GetRefDocumentsForWages with @RefDocumentTypeId omitted - GetWagesRefDocumentsStatusById(701). */
    public boolean wagesActive() {
        for (var r : jdbc.queryForList("EXEC [dbo].[USP_GetRefDocumentsForWages]")) {
            if (intOf(col(r, "RefDocumentTypeId")) == DOCUMENT_TYPE_ID) return truthy(col(r, "IsActive"));
        }
        return false;
    }

    // ================================================================== gate pass / order loaders

    /** PendingGpDataDbCall :1138 - DocumentTypeId 51; @RecId only when editing; the procedure ignores it. */
    public List<Map<String, Object>> pendingGatePasses(UserAccount u, int financialYearId, int recId) {
        StringBuilder sql = new StringBuilder("EXEC [dbo].[USP_PendingGatePassforGRNPackingMaterial] @OrganizationId=?, @CompanyId=?, @DocumentTypeId=?");
        List<Object> a = new ArrayList<>(List.of(u.getOrganizationId(), u.getCompanyId(), GATE_PASS_DOCUMENT_TYPE_ID));
        if (financialYearId != 0) { sql.append(", @FinancialYearId=?"); a.add(financialYearId); }
        if (recId != 0) { sql.append(", @RecId=?"); a.add(recId); }
        if (u.getBranchesId() != 0) { sql.append(", @BranchesId=?"); a.add(u.getBranchesId()); }
        return jdbc.queryForList(sql.toString(), a.toArray());
    }

    /** One gate pass row as the grid holds it, or null when it is no longer pending. */
    public Map<String, Object> pendingGatePass(UserAccount u, int financialYearId, int gpId) {
        for (var r : pendingGatePasses(u, financialYearId, 0)) if (intOf(col(r, "Id")) == gpId) return r;
        return null;
    }

    /**
     * PurchaseOrder.PurchaseOrderLoaderForPackingMaterial (BLL 0595 :1714). @FinancialYearId is always
     * sent; @SupplierCustomerId, @OrderId, @FromDate, @ToDate, @ActionId (ZeroBalanceType) and @GpId only when set.
     */
    public List<Map<String, Object>> orderLoader(UserAccount u, int financialYearId, int supplierId, int orderId,
                                                 java.sql.Date from, java.sql.Date to, int zeroBalanceType, int gpId) {
        StringBuilder sql = new StringBuilder("EXEC dbo.USP_GetPurchaseOrderdLoader @OrganizationId=?, @CompanyId=?, @DocumentTypeId=?, @FinancialYearId=?");
        List<Object> a = new ArrayList<>(List.of(u.getOrganizationId(), u.getCompanyId(), ORDER_DOCUMENT_TYPE_ID, financialYearId));
        if (supplierId != 0) { sql.append(", @SupplierCustomerId=?"); a.add(supplierId); }
        if (orderId != 0) { sql.append(", @OrderId=?"); a.add(orderId); }
        if (from != null) { sql.append(", @FromDate=?"); a.add(from); }
        if (to != null) { sql.append(", @ToDate=?"); a.add(to); }
        if (zeroBalanceType != 0) { sql.append(", @ActionId=?"); a.add(zeroBalanceType); }
        if (gpId != 0) { sql.append(", @GpId=?"); a.add(gpId); }
        return jdbc.queryForList(sql.toString(), a.toArray());
    }

    /** LoadPurchaseOrderPM.SupplierNameFilll - DocumentTypeId 700. */
    public List<Map<String, Object>> orderSuppliers(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : jdbc.queryForList(
                "EXEC dbo.Sp_SupplierCustomer_GetAllMethod @OrganizationId=?, @CompanyId=?, @DocumentTypeId=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), ORDER_DOCUMENT_TYPE_ID, "GetSupplierCustomerFromOrderByInvoice")) {
            out.add(two(col(r, "OrderSupCustId"), col(r, "CompanyName")));
        }
        return out;
    }

    // ================================================================== history

    public static final class HistoryFilter {
        public boolean canViewAll;
        public int financialYearId;
        public String dateMode = "doc";
        public java.sql.Date from, to;
        public int fromDocNo, toDocNo, supplierId, actionId;
    }

    /** InvGrn.GetHisoty (BLL 0576 :334) with the fields HistoryGridFill :1359 sets. */
    public List<Map<String, Object>> history(UserAccount u, HistoryFilter f) {
        StringBuilder sql = new StringBuilder("EXEC dbo.Sp_InvGRN_GetAllMethod @OrganizationId=?, @CompanyId=?, @DocumentTypeId=?, @CanViewAllRecord=?");
        List<Object> a = new ArrayList<>(List.of(u.getOrganizationId(), u.getCompanyId(), DOCUMENT_TYPE_ID, f.canViewAll));
        if (f.financialYearId != 0) { sql.append(", @FinancialYearId=?"); a.add(f.financialYearId); }
        if (u.getBranchesId() != 0) { sql.append(", @BranchesId=?"); a.add(u.getBranchesId()); }
        if (!f.canViewAll) { sql.append(", @EntryUser=?"); a.add(u.getId()); }
        String[] names = switch (f.dateMode) {
            case "entry" -> new String[]{"@EntryFromDate", "@EntryToDate"};
            case "modify" -> new String[]{"@ModifyFromDate", "@ModifyToDate"};
            case "approved" -> new String[]{"@ApprovedFromDate", "@ApprovedToDate"};
            default -> new String[]{"@fromDate", "@toDate"};
        };
        if (f.from != null) { sql.append(", ").append(names[0]).append("=?"); a.add(f.from); }
        if (f.to != null) { sql.append(", ").append(names[1]).append("=?"); a.add(f.to); }
        if (f.fromDocNo != 0) { sql.append(", @GrnNoF=?"); a.add(f.fromDocNo); }
        if (f.toDocNo != 0) { sql.append(", @GrnNoT=?"); a.add(f.toDocNo); }
        if (f.supplierId != 0) { sql.append(", @SupplierCustomerId=?"); a.add(f.supplierId); }
        if (f.actionId != 0) { sql.append(", @ActionId=?"); a.add(f.actionId); }
        sql.append(", @Activity=?"); a.add("GRNFormHistory");
        return jdbc.queryForList(sql.toString(), a.toArray());
    }

    // ================================================================== one record

    /** InvGrn.GetByID header, refused unless it is this company's type-701 GRN. */
    public Map<String, Object> header(UserAccount u, int id) {
        var rows = jdbc.queryForList("EXEC dbo.Sp_InvGRN_GetAllMethod @Id=?, @Activity=?", id, "ReadByID");
        if (rows.isEmpty()) throw new ResponseStatusException(HttpStatus.NOT_FOUND, "Record Not Found");
        Map<String, Object> h = rows.get(0);
        if (intOf(col(h, "OrganizationId")) != u.getOrganizationId() || intOf(col(h, "CompanyId")) != u.getCompanyId()
                || intOf(col(h, "DocumentTypeId")) != DOCUMENT_TYPE_ID) {
            throw new ResponseStatusException(HttpStatus.NOT_FOUND, "Record Not Found");
        }
        return h;
    }

    /** DAL GetDate :385 - DocumentTypeId 701 reads ReadByInvGrnIDStore. */
    public List<Map<String, Object>> details(int id) {
        return jdbc.queryForList("EXEC dbo.Sp_InvGrnDetail_GetAllMethod @Id=?, @Activity=?", id, "ReadByInvGrnIDStore");
    }

    public List<Map<String, Object>> attachments(UserAccount u, int id) {
        return jdbc.queryForList("EXEC dbo.Sp_DMSAttachments_GetAllMethod @ScreenName=?, @Id=?, @Activity=?", SCREEN, id, "ReadById")
                .stream()
                .filter(r -> intOf(col(r, "OrganizationId")) == u.getOrganizationId() && intOf(col(r, "CompanyId")) == u.getCompanyId())
                .toList();
    }

    // ================================================================== DAL InvGrn.SetData

    public int saveHeader(Map<String, Object> header, boolean insert) {
        return execute(insert ? "dbo.Sp_InvGrn_Insert" : "dbo.Sp_InvGrn_Update", header);
    }

    public int saveDetail(Map<String, Object> detail) {
        return execute("dbo.Sp_InvGrnDetail_Insert", detail);
    }

    /** DAL :133-137 - InventoryTransactions with every other property at its CLR default. */
    public void inventoryTransactions(UserAccount u, int id) {
        inventoryTransactions(u, DOCUMENT_TYPE_ID, id);
    }

    /** The same SetProc for another document type (Purchase Invoice Direct PM 245, DAL 0434 :524). */
    public void inventoryTransactions(UserAccount u, int documentTypeId, int id) {
        Map<String, Object> t = InventoryOpeningDefaults.transactions();
        t.put("OrganizationId", u.getOrganizationId());
        t.put("CompanyId", u.getCompanyId());
        t.put("RefDocumentTypeId", documentTypeId);
        t.put("RefDocIdNo", id);
        execute("dbo.Sp_InventoryTransactions_GetALLMethod", t);
    }

    /** DAL :279-290 - every document type other than 217. */
    public void stockInTransit(UserAccount u, int id) {
        ProcExec.run(jdbc, "EXEC dbo.usp_StockInTransitUpdate_StockEvaluationAndVoucherInsertFromGrn @OrganizationId=?, @CompanyId=?, @DocumentTypeId=?, @GrnId=?",
                u.getOrganizationId(), u.getCompanyId(), DOCUMENT_TYPE_ID, id);
    }

    /** DAL InvPurchaseInvoice.AccountandInventoryRemoveById. */
    public void delete(UserAccount u, int id) {
        ProcExec.run(jdbc, "EXEC dbo.Sp_InvoicesVouchersandStocksDelete @OrganizationId=?, @CompanyId=?, @Id=?, @DocumentTypeId=?, @UserId=?",
                u.getOrganizationId(), u.getCompanyId(), id, DOCUMENT_TYPE_ID, u.getId());
    }

    /**
     * GenericProvider.SetProc: every model property the procedure declares; a property whose value is
     * null is not sent (SqlParameter.Value = null lets the procedure default apply). First scalar back.
     */
    private int execute(String procedure, Map<String, Object> values) {
        List<String> names = parameters.computeIfAbsent(procedure, p -> jdbc.queryForList(
                "SELECT SUBSTRING(name,2,128) FROM sys.parameters WHERE object_id=OBJECT_ID(?) AND parameter_id>0 ORDER BY parameter_id",
                String.class, p));
        if (names.isEmpty()) throw new IllegalStateException("Procedure contract unavailable: " + procedure);
        TreeMap<String, Object> ci = new TreeMap<>(String.CASE_INSENSITIVE_ORDER);
        ci.putAll(values);
        List<String> selected = names.stream().filter(k -> ci.containsKey(k) && ci.get(k) != null).toList();
        String sql = "EXEC " + procedure + " " + String.join(", ", selected.stream().map(k -> "@" + k + "=?").toList());
        return jdbc.execute(sql, (PreparedStatementCallback<Integer>) st -> {
            int n = 1;
            for (String name : selected) st.setObject(n++, ci.get(name));
            return first(st);
        });
    }

    private static int first(PreparedStatement st) throws java.sql.SQLException {
        boolean result = st.execute();
        Integer first = null;
        while (true) {
            if (result) {
                try (ResultSet rs = st.getResultSet()) {
                    while (rs.next()) if (first == null && rs.getObject(1) instanceof Number n) first = n.intValue();
                }
            } else if (st.getUpdateCount() == -1) break;
            result = st.getMoreResults();
        }
        return first == null ? 0 : first;
    }

    // ================================================================== helpers

    private static Map<String, Object> two(Object id, Object name) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("Id", intOf(id));
        m.put("Description", name);
        return m;
    }

    private static boolean truthy(Object v) {
        if (v instanceof Boolean b) return b;
        if (v instanceof Number n) return n.intValue() != 0;
        return v != null && Set.of("true", "1").contains(v.toString().trim().toLowerCase(Locale.ROOT));
    }
}
