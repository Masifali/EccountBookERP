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

/**
 * Screen 498 "1002 Purchsae Order" (Packing Material) - {@code PurchsaeOrderPmNew.cs} (6,854 lines,
 * DocumentTypeId 700, BaseDocumentTypeId 2).
 *
 * Every read is the desktop's own call, parameters sent exactly when its BLL sends them:
 *
 * <pre>
 * Form method                     BLL / global                                   Procedure (@Activity)
 * ------------------------------  ---------------------------------------------  ----------------------------------------------
 * SupplierDtFillFromGlobal  :850  globalSupplierCustomer                         USP_GetVendorsAndCustomersWithCityName
 * PaymentTermDtFill...      :896  globalPaymentTerm                              Sp_InvDueTerms_GetAllMethod (GetAll)
 * CurrencyDtFill...         :947  globalMultiCurrency                            Sp_MultiCurrency_GetAllMethod (ReadAll)
 * ItemCategoryOrTypeBind    :960  getGlobalAllItems, ItemTypeOfTypeId == 14      USP_Item_AllItemsWithModal
 * CropDtFill...             :1056 globalCropYear                                 Sp_InvCropYear_GetAllMethod (ReadAll)
 * UomFromGlobalBind         :1069 globalUomSchedule by ItemId                    usp_getAllUomsByCompanyId
 * RefDocumentTypeBind       :1085 StaticColumnsService                           SpStaticColumnNames (RefDocumentTypeForPackingMaterial)
 * RefDocdtFillDbCall        :1099 PurchaseOrder.GetContractScheduleAndInvoice... [dbo].[USP_GetContractScheduleAndInvoiceDataForPM]
 * GetTaxTypeIdAndTaxPercent :2877 ItemTaxSchedule.GetTaxScheduleForItemId        Sp_ItemTaxSchedule_GetAllMethod (GetItemTaxScheduleForItemId)
 * combitem_Leave / suppname :2839 Item.GetLeadTimeBySupplierAndItem              Sp_Item_GetAllMethod (GetLeadTimeBySupplierAndItem)
 * cmbCurrency_Leave         :655  VoucherHead.GetLastExchangeRateAndCurrency...  Sp_Vouchers_GetMethods (GetMultiCurrencyAndLastRate)
 * HistoryComboDBCall        :698  PurchaseOrder.GetDataForDropDownFromPO         USP_GetDataForDropDownFromPurchaseOrder
 * gridhistoryfill           :2181 PurchaseOrder.PurchaseOrderPackingMaterial...  USp_PurchaseOrderPackingMaterialFormHistory
 * ReadById                  :1931 PurchaseOrder.GetByID                          Sp_PurchaseOrder_GetAllMethod (ReadById / ReadByPurchaseOrderHeaderId)
 * </pre>
 *
 * The header write reuses {@link PurchaseOrderHeaderRepository} - the same model, the same two
 * procedures and the same CLR defaults as screen 120. The detail and the three post-save steps of
 * DAL {@code PurchaseOrder.SetData} are here.
 */
@Repository
public class PurchaseOrderPmRepository {

    public static final int DOCUMENT_TYPE_ID = 700;
    public static final int BASE_DOCUMENT_TYPE_ID = 2;
    /** base.Name - the ScreenName attachments are keyed on. */
    public static final String SCREEN = "PurchsaeOrderPmNew";

    private final JdbcTemplate jdbc;
    private final Map<String, List<String>> parameters = new ConcurrentHashMap<>();

    public PurchaseOrderPmRepository(JdbcTemplate jdbc) {
        this.jdbc = jdbc;
    }

    // ================================================================== lookups

    /** :850 - SubsidiaryAccountAllownOnVouchers (feature 4) keeps PartyTypeId == 1 only. */
    public List<Map<String, Object>> suppliers(UserAccount u, boolean subsidiaryFeature) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : jdbc.queryForList("EXEC dbo.USP_GetVendorsAndCustomersWithCityName @OrganizationId=?, @CompanyId=?",
                u.getOrganizationId(), u.getCompanyId())) {
            if (subsidiaryFeature && intOf(col(r, "PartyTypeId")) != 1) continue;
            Map<String, Object> m = new LinkedHashMap<>();          // dtSupplier :418-424, cols 3 and 4 hidden
            m.put("Id", intOf(col(r, "Id")));
            m.put("CompanyName", col(r, "CompanyName"));
            m.put("PartyCode", col(r, "PartyCode"));
            m.put("CityName", col(r, "CityName"));
            m.put("MobileNo", col(r, "MobilePersonal"));
            out.add(m);
        }
        return out;
    }

    public List<Map<String, Object>> paymentTerms(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : jdbc.queryForList("EXEC dbo.Sp_InvDueTerms_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), "GetAll")) {
            out.add(two(col(r, "Id"), col(r, "TermsDescription")));
        }
        return out;
    }

    public List<Map<String, Object>> currencies(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : jdbc.queryForList("EXEC dbo.Sp_MultiCurrency_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), "ReadAll")) {
            out.add(two(col(r, "Id"), col(r, "CurrencyName")));
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

    /** getGlobalAllItems where ItemTypeOfTypeId == 14 (:964, :1019). */
    public List<Map<String, Object>> items(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : jdbc.queryForList("EXEC dbo.USP_Item_AllItemsWithModal @OrganizationId=?, @CompanyId=?",
                u.getOrganizationId(), u.getCompanyId())) {
            if (intOf(col(r, "ItemTypeOfTypeId")) != 14) continue;
            Map<String, Object> m = new LinkedHashMap<>();          // dtitem :425-429 + the two filter keys
            m.put("Id", intOf(col(r, "Id")));
            m.put("ItemName", col(r, "ItemName"));
            m.put("ItemCode", col(r, "ItemCode"));
            m.put("LeadTimeDay", intOf(col(r, "LeadTimeDay")));
            m.put("WeightCapacity", col(r, "WeightCapacity"));
            m.put("ItemCategoryId", intOf(col(r, "ItemCategoryId")));
            m.put("ItemCategory", col(r, "ItemCategory"));
            m.put("ItemTypeId", intOf(col(r, "ItemTypeId")));
            m.put("ItemType", col(r, "ItemType"));
            out.add(m);
        }
        return out;
    }

    /** globalUomSchedule, loaded once; the form filters it by ItemId (dtUomFromGloablUomScheduleByItemId). */
    public List<Map<String, Object>> uoms(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : jdbc.queryForList("EXEC dbo.usp_getAllUomsByCompanyId @OrganizationId=?, @CompanyId=?",
                u.getOrganizationId(), u.getCompanyId())) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", intOf(col(r, "Id")));
            m.put("UOMCode", col(r, "UOMCode"));
            m.put("Equivalent", dbl(col(r, "Equivalent")));
            m.put("BaseRateUom", truthy(col(r, "BaseRateUom")));
            m.put("BasePackUom", truthy(col(r, "BasePackUom")));
            m.put("ItemId", intOf(col(r, "ItemId")));
            out.add(m);
        }
        return out;
    }

    public List<Map<String, Object>> uomsForItem(UserAccount u, int itemId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (var m : uoms(u)) if (intOf(m.get("ItemId")) == itemId) out.add(m);
        return out;
    }

    public List<Map<String, Object>> refDocumentTypes() {
        return jdbc.queryForList("EXEC dbo.SpStaticColumnNames @Activity=?", "RefDocumentTypeForPackingMaterial");
    }

    /** RefDocdtFillDbCall :1099 - @SupplierCustomerId is never set by this form, so never sent. */
    public List<Map<String, Object>> refDocs(UserAccount u, int refDocumentTypeId, int recId) {
        if (refDocumentTypeId <= 0) return List.of();
        StringBuilder sql = new StringBuilder("EXEC [dbo].[USP_GetContractScheduleAndInvoiceDataForPM] "
                + "@OrganizationId=?, @CompanyId=?, @DocumentTypeId=?, @RefDocumentTypeId=?");
        List<Object> a = new ArrayList<>(List.of(u.getOrganizationId(), u.getCompanyId(), DOCUMENT_TYPE_ID, refDocumentTypeId));
        if (recId != 0) { sql.append(", @RecId=?"); a.add(recId); }
        return jdbc.queryForList(sql.toString(), a.toArray());
    }

    /** ItemTaxSchedule.GetTaxScheduleForItemId - @ItemId when non-zero, @EffectedDate = DocDate. */
    public List<Map<String, Object>> taxes(UserAccount u, int itemId, java.sql.Date effected) {
        StringBuilder sql = new StringBuilder("EXEC dbo.Sp_ItemTaxSchedule_GetAllMethod @OrganizationId=?, @CompanyId=?");
        List<Object> a = new ArrayList<>(List.of(u.getOrganizationId(), u.getCompanyId()));
        if (itemId != 0) { sql.append(", @ItemId=?"); a.add(itemId); }
        if (effected != null) { sql.append(", @EffectedDate=?"); a.add(effected); }
        sql.append(", @Activity=?"); a.add("GetItemTaxScheduleForItemId");
        return jdbc.queryForList(sql.toString(), a.toArray());
    }

    /** Item.GetLeadTimeBySupplierAndItem - all four parameters always sent; column 0 of row 0. */
    public int leadTime(UserAccount u, int supplierId, int itemId) {
        var rows = jdbc.queryForList(
                "EXEC dbo.Sp_Item_GetAllMethod @OrganizationId=?, @CompanyId=?, @SupplierCustomerId=?, @ItemId=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), supplierId, itemId, "GetLeadTimeBySupplierAndItem");
        if (rows.isEmpty()) return 0;
        Iterator<Object> it = rows.get(0).values().iterator();
        return it.hasNext() ? intOf(it.next()) : 0;
    }

    /** cmbCurrency_Leave :671 - last rate this document type used for that currency. */
    public Double lastExchangeRate(UserAccount u, int currencyId) {
        var rows = jdbc.queryForList(
                "EXEC dbo.Sp_Vouchers_GetMethods @OrganizationId=?, @CompanyId=?, @DocumentTypeIds=?, @DMultiCurrencyIds=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), String.valueOf(DOCUMENT_TYPE_ID), String.valueOf(currencyId),
                "GetMultiCurrencyAndLastRate");
        return rows.isEmpty() ? null : dbl(col(rows.get(0), "LastExchRate"));
    }

    // ================================================================== configuration / features

    public String config(UserAccount u, String name) {
        var rows = jdbc.queryForList(
                "EXEC dbo.Sp_ConfigrationsAllocation_GetAllMethod @OrganizationId=?, @CompanyId=?, @ConfigDescription=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), name, "GetConfigurationByOrgCompandConfigDescription");
        return rows.isEmpty() ? "" : Objects.toString(col(rows.get(0), "ConfigKey"), "").trim();
    }

    /** CommonServices.GetConfigurationsByDefinitionIds - "1" base currency, "160" base rate (:610-611). */
    public String configByDefinition(UserAccount u, String definitionId) {
        var rows = jdbc.queryForList(
                "EXEC dbo.Sp_ConfigrationsAllocation_GetAllMethod @OrganizationId=?, @CompanyId=?, @DefinitionIds=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), definitionId, "GetConfigurationsByDefinitionIds");
        return rows.isEmpty() ? "" : Objects.toString(col(rows.get(0), "ConfigKey"), "").trim();
    }

    public Set<Integer> features(UserAccount u) {
        Set<Integer> out = new HashSet<>();
        for (var r : jdbc.queryForList("EXEC dbo.USP_GetERPFeaturesByCompanyId @OrganizationId=?, @CompanyId=?",
                u.getOrganizationId(), u.getCompanyId())) out.add(intOf(col(r, "Id")));
        return out;
    }

    // ================================================================== history

    /** HistoryComboDBCall :698 - one call, rows split on their Activity column. @Activity is not sent. */
    public Map<String, List<Map<String, Object>>> historyCombos(UserAccount u) {
        List<Map<String, Object>> suppliers = new ArrayList<>(), branches = new ArrayList<>();
        for (var r : jdbc.queryForList(
                "EXEC dbo.USP_GetDataForDropDownFromPurchaseOrder @OrganizationId=?, @CompanyId=?, @DocumentTypeIds=?, @BranchesIds=?",
                u.getOrganizationId(), u.getCompanyId(), String.valueOf(DOCUMENT_TYPE_ID), String.valueOf(u.getBranchesId()))) {
            String act = Objects.toString(col(r, "Activity"), "");
            if ("Supplier".equals(act)) suppliers.add(two(col(r, "Id"), col(r, "ReferenceName")));
            if ("Branch".equals(act)) branches.add(two(col(r, "Id"), col(r, "ReferenceName")));
        }
        Map<String, List<Map<String, Object>>> out = new LinkedHashMap<>();
        out.put("suppliers", suppliers);
        out.put("branches", branches);
        return out;
    }

    /** History filter as ReportsParameters carries it; nulls/zeros are the unset state. */
    public static final class HistoryFilter {
        public boolean canViewAll;
        public int financialYearId;
        public String dateMode = "doc";
        public java.sql.Date from, to;
        public int fromDocNo, toDocNo, supplierId;
    }

    /**
     * BLL PurchaseOrderPackingMaterialFormHistory (:1795). NOTE: the form also fills
     * {@code obj.BranchesIds} (:2257), but this BLL method never sends a branch parameter, so
     * the Branch filter selects nothing - ported as the BLL is written.
     */
    public List<Map<String, Object>> history(UserAccount u, HistoryFilter f) {
        StringBuilder sql = new StringBuilder("EXEC dbo.USp_PurchaseOrderPackingMaterialFormHistory "
                + "@OrganizationId=?, @CompanyId=?, @DocumentTypeId=?, @CanViewAllRecord=?");
        List<Object> a = new ArrayList<>(List.of(u.getOrganizationId(), u.getCompanyId(), DOCUMENT_TYPE_ID, f.canViewAll));
        if (f.financialYearId != 0) { sql.append(", @FinancialYearId=?"); a.add(f.financialYearId); }
        sql.append(", @BaseDocumentTypeId=?"); a.add(BASE_DOCUMENT_TYPE_ID);
        if (!f.canViewAll) { sql.append(", @EntryUser=?"); a.add(u.getId()); }
        if (f.fromDocNo != 0) { sql.append(", @FromDocNo=?"); a.add(f.fromDocNo); }
        if (f.toDocNo != 0) { sql.append(", @ToDocNo=?"); a.add(f.toDocNo); }
        String[] names = switch (f.dateMode) {
            case "entry" -> new String[]{"@EntryFromDate", "@EntryToDate"};
            case "modify" -> new String[]{"@ModifyFromDate", "@ModifyToDate"};
            case "approved" -> new String[]{"@ApprovedFromDate", "@ApprovedToDate"};
            default -> new String[]{"@DocDateFrom", "@DocDateTo"};
        };
        if (f.from != null) { sql.append(", ").append(names[0]).append("=?"); a.add(f.from); }
        if (f.to != null) { sql.append(", ").append(names[1]).append("=?"); a.add(f.to); }
        if (f.supplierId != 0) { sql.append(", @OrderSupCustId=?"); a.add(f.supplierId); }
        return jdbc.queryForList(sql.toString(), a.toArray());
    }

    // ================================================================== one record

    /** PurchaseOrder.GetByID header, refused unless it is this company's type-700 order. */
    public Map<String, Object> header(UserAccount u, int id) {
        var rows = jdbc.queryForList("EXEC dbo.Sp_PurchaseOrder_GetAllMethod @Id=?, @Activity=?", id, "ReadById");
        if (rows.isEmpty()) throw new ResponseStatusException(HttpStatus.NOT_FOUND, "Purchase Order not found");
        Map<String, Object> h = rows.get(0);
        if (intOf(col(h, "OrganizationId")) != u.getOrganizationId() || intOf(col(h, "CompanyId")) != u.getCompanyId()
                || intOf(col(h, "DocumentTypeId")) != DOCUMENT_TYPE_ID) {
            throw new ResponseStatusException(HttpStatus.NOT_FOUND, "Purchase Order not found in this company");
        }
        return h;
    }

    public List<Map<String, Object>> details(int id) {
        return jdbc.queryForList("EXEC dbo.Sp_PurchaseOrder_GetAllMethod @Id=?, @Activity=?", id, "ReadByPurchaseOrderHeaderId");
    }

    public List<Map<String, Object>> attachments(UserAccount u, int id) {
        return jdbc.queryForList("EXEC dbo.Sp_DMSAttachments_GetAllMethod @ScreenName=?, @Id=?, @Activity=?", SCREEN, id, "ReadById")
                .stream()
                .filter(r -> intOf(col(r, "OrganizationId")) == u.getOrganizationId() && intOf(col(r, "CompanyId")) == u.getCompanyId())
                .toList();
    }

    // ================================================================== DAL SetData, after the header

    /** One Sp_PurchaseOrderDetail_Insert per row (DAL :59-64); the procedure branches on @Id. */
    public void saveDetail(Map<String, Object> detail) {
        execute("dbo.Sp_PurchaseOrderDetail_Insert", detail);
    }

    /** DAL :143-155 - only on update, only when rows were removed. */
    public void deleteRemovedDetails(UserAccount u, int orderId, String removeIds) {
        ProcExec.run(jdbc, "EXEC dbo.USP_DeletePurchaseOrderDetailIfNotExistInGrn @OrganizationId=?, @CompanyId=?, @OrderId=?, @UserId=?, @OrderDetailIds=?",
                u.getOrganizationId(), u.getCompanyId(), orderId, u.getId(), removeIds);
    }

    /** DAL :156-165 - on every update. */
    public void weightValidation(UserAccount u, int orderId) {
        ProcExec.run(jdbc, "EXEC dbo.Sp_PurchaseOrder_GetAllMethod @OrganizationId=?, @CompanyId=?, @Id=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), orderId, "PoWeightAndGpWeightValidation");
    }

    /** DAL :167-175 - on insert only; Architecture.Model.DocumentApprovalDetail has these five properties. */
    public void approvalDetail(UserAccount u, int orderId, java.math.BigDecimal limitAmount) {
        ProcExec.run(jdbc, "EXEC [DAW].[USp_DocumentApprovalDetail_Insert] @OrganizationId=?, @CompanyId=?, @DocumentTypeId=?, @Id=?, @LimitAmount=?",
                u.getOrganizationId(), u.getCompanyId(), DOCUMENT_TYPE_ID, orderId, limitAmount);
    }

    /** GenericProvider.SetProc: the model properties the procedure declares, first scalar back. */
    private int execute(String procedure, Map<String, Object> values) {
        List<String> names = parameters.computeIfAbsent(procedure, p -> jdbc.queryForList(
                "SELECT SUBSTRING(name,2,128) FROM sys.parameters WHERE object_id=OBJECT_ID(?) AND parameter_id>0 ORDER BY parameter_id",
                String.class, p));
        if (names.isEmpty()) throw new IllegalStateException("Procedure contract unavailable: " + procedure);
        TreeMap<String, Object> ci = new TreeMap<>(String.CASE_INSENSITIVE_ORDER);
        ci.putAll(values);
        List<String> selected = names.stream().filter(ci::containsKey).toList();
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

    public static Object col(Map<String, Object> r, String name) {
        if (r == null) return null;
        if (r.containsKey(name)) return r.get(name);
        for (var e : r.entrySet()) if (e.getKey().equalsIgnoreCase(name)) return e.getValue();
        return null;
    }

    public static int intOf(Object v) {
        if (v == null) return 0;
        if (v instanceof Number n) return n.intValue();
        if (v instanceof Boolean b) return b ? 1 : 0;
        try { return (int) Double.parseDouble(v.toString().replace(",", "").trim()); } catch (NumberFormatException e) { return 0; }
    }

    public static double dbl(Object v) {
        if (v == null) return 0d;
        if (v instanceof Number n) return n.doubleValue();
        try { return Double.parseDouble(v.toString().replace(",", "").trim()); } catch (NumberFormatException e) { return 0d; }
    }

    private static boolean truthy(Object v) {
        if (v instanceof Boolean b) return b;
        if (v instanceof Number n) return n.intValue() != 0;
        return v != null && Set.of("true", "1").contains(v.toString().trim().toLowerCase(Locale.ROOT));
    }
}
