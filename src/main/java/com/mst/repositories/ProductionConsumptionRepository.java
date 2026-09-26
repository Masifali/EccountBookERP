package com.mst.repositories;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.ConnectionCallback;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.math.BigDecimal;
import java.math.MathContext;
import java.math.RoundingMode;
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
 * Screen 280 "Production Against (Job Order)" - the Consumption tab page, the Consumption
 * History tab page and the Transaction History tab page, all of which are implemented by
 * FoodProductionWithValues.cs itself (DocumentTypeId 181), plus the two loader forms that
 * page opens (LoadConsumptionPendingforRates.cs, LoadavailableTransactionsForIssuanceOnConsumption.cs).
 *
 * Every call below names its BLL/DAL source. Parameters the BLL adds only when set are added
 * here only when set ({@link #put} drops a null, and the guards are written out at each call).
 *
 * ---------------------------------------------------------------------------------------------
 * THE WRITE PATH (InvFoodProduction.Save -> DAL InvFoodProduction.SetData)
 * ---------------------------------------------------------------------------------------------
 * The DAL writes through GenericProvider.SetProc, which sends "@" + every non-virtual property
 * of the model with AddWithValue. A property whose value is a C# null is not sent at all; an
 * int/double/bool property is sent with its value (0 / false when the form never set it). The
 * procedures declare exactly the model properties they receive, so each writer below sends
 * "procedure parameters that are model properties", with the desktop's value or the type's
 * default. Parameters that the procedure declares but the model does not have (UserLogId,
 * BranchSrNo, hId, ...) are NOT sent, exactly as on the desktop.
 *
 * The posting steps run in the caller's transaction (ProductionConsumptionService wraps them in a
 * TransactionTemplate, the SqlTransaction the DAL opens).
 */
@Repository
public class ProductionConsumptionRepository {

    /** InvFoodProduction.DocumentTypeId for a consumption (FoodProductionWithValues.cs:2199). */
    public static final int DOC_TYPE_CONSUMPTION = 181;

    @Autowired private JdbcTemplate jdbc;

    // ===================================================================================== plumbing

    /** A parameter map that keeps the BLL's order; {@link #put} skips C# nulls. */
    public static Map<String, Object> params() { return new LinkedHashMap<>(); }

    /** AddWithValue(name, null) sends nothing - a null value is dropped here for the same reason. */
    public static void put(Map<String, Object> p, String name, Object v) { if (v != null) p.put(name, v); }

    private static String sql(String proc, Map<String, Object> p) {
        StringBuilder sb = new StringBuilder("EXEC ").append(proc);
        boolean first = true;
        for (String k : p.keySet()) {
            sb.append(first ? " " : ", ").append(k).append("=?");
            first = false;
        }
        return sb.toString();
    }

    private static Object jdbcValue(Object v) {
        if (v instanceof LocalDateTime) return Timestamp.valueOf((LocalDateTime) v);
        if (v instanceof LocalDate) return Timestamp.valueOf(((LocalDate) v).atStartOfDay());
        return v;
    }

    /** A reading call - the first result set, as the BLL's DataTable. */
    public List<Map<String, Object>> query(String proc, Map<String, Object> p) {
        Object[] args = p.values().stream().map(ProductionConsumptionRepository::jdbcValue).toArray();
        return jdbc.queryForList(sql(proc, p), args);
    }

    /**
     * ExecuteScalar / ExecuteNonQuery - runs the procedure, drains every result so a RAISERROR
     * that comes after a SELECT still surfaces (SqlDataReader.Close does the same on the desktop),
     * and returns the first column of the first row of the first result set, or null.
     */
    public Object scalar(String proc, Map<String, Object> p) {
        final String s = sql(proc, p);
        final Object[] args = p.values().stream().map(ProductionConsumptionRepository::jdbcValue).toArray();
        return jdbc.execute((ConnectionCallback<Object>) con -> {
            try (PreparedStatement ps = con.prepareStatement(s)) {
                for (int i = 0; i < args.length; i++) ps.setObject(i + 1, args[i]);
                boolean isRs = ps.execute();
                Object first = null;
                boolean got = false;
                while (true) {
                    if (isRs) {
                        try (ResultSet rs = ps.getResultSet()) {
                            while (rs.next()) {
                                if (!got) { first = rs.getObject(1); got = true; }
                            }
                        }
                    } else if (ps.getUpdateCount() == -1) {
                        break;
                    }
                    isRs = ps.getMoreResults();
                }
                return first;
            }
        });
    }

    /** Several result sets of one call (not needed by the desktop calls here, kept private). */
    @SuppressWarnings("unused")
    private List<Map<String, Object>> rows(ResultSet rs) throws java.sql.SQLException {
        List<Map<String, Object>> out = new ArrayList<>();
        ResultSetMetaData md = rs.getMetaData();
        while (rs.next()) {
            Map<String, Object> m = new LinkedHashMap<>();
            for (int i = 1; i <= md.getColumnCount(); i++) m.put(md.getColumnLabel(i), rs.getObject(i));
            out.add(m);
        }
        return out;
    }

    public static Object col(Map<String, Object> row, String name) {
        return ProductionAgainstJobOrderRepository.col(row, name);
    }

    public static int intOf(Object v) {
        if (v == null) return 0;
        if (v instanceof Number) return ((Number) v).intValue();
        if (v instanceof Boolean) return ((Boolean) v) ? 1 : 0;
        String s = String.valueOf(v).trim();
        if (s.isEmpty()) return 0;
        try { return Integer.parseInt(s); } catch (NumberFormatException e) { /* fall through */ }
        try { return new BigDecimal(s).setScale(0, RoundingMode.HALF_EVEN).intValue(); }
        catch (NumberFormatException e) { return 0; }
    }

    public static double dbl(Object v) {
        if (v == null) return 0d;
        if (v instanceof Number) return ((Number) v).doubleValue();
        String s = String.valueOf(v).trim().replace(",", "");
        if (s.isEmpty()) return 0d;
        try { return Double.parseDouble(s); } catch (NumberFormatException e) { return 0d; }
    }

    public static String str(Object v) { return v == null ? "" : String.valueOf(v); }

    public static boolean bool(Object v) {
        if (v instanceof Boolean) return (Boolean) v;
        if (v instanceof Number) return ((Number) v).intValue() != 0;
        String s = str(v).trim();
        return "1".equals(s) || "true".equalsIgnoreCase(s);
    }

    /**
     * .NET Framework double.ToString() - the "G" format, 15 significant digits. Used wherever the
     * desktop concatenates a double into text (voucher comments, error messages).
     */
    public static String netG(double d) {
        if (Double.isNaN(d)) return "NaN";
        if (d == 0d) return "0";
        BigDecimal b = new BigDecimal(d).round(new MathContext(15, RoundingMode.HALF_EVEN)).stripTrailingZeros();
        double a = Math.abs(d);
        if (a >= 1e15 || a < 1e-5) {
            String e = String.format(java.util.Locale.ROOT, "%.14E", d);
            String[] parts = e.split("E");
            String mant = parts[0].contains(".") ? parts[0].replaceAll("0+$", "").replaceAll("\\.$", "") : parts[0];
            int exp = Integer.parseInt(parts[1]);
            return mant + "E" + (exp < 0 ? "-" : "+") + String.format(java.util.Locale.ROOT, "%02d", Math.abs(exp));
        }
        return b.toPlainString();
    }

    /** Math.Round(x, digits) - .NET rounds half to even by default. */
    public static double round(double x, int digits) {
        if (Double.isNaN(x) || Double.isInfinite(x)) return x;
        return new BigDecimal(x).setScale(digits, RoundingMode.HALF_EVEN).doubleValue();
    }

    // ================================================================================ Consumption reads

    /**
     * GenerateDocConsumption:1300 -> InvFoodProduction.GetSerialNumber (BLL :5199):
     * Sp_InvFoodProduction_GetAllMethod @OrganizationId, @CompanyId, @DocumentTypeId,
     * [@FinancialYearId when non-zero], [@BranchesId when non-zero], @Activity='ReadSerialNumber'.
     */
    public int serialNumber(int org, int comp, int documentTypeId, int financialYearId, int branchId) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", org);
        p.put("@CompanyId", comp);
        p.put("@DocumentTypeId", documentTypeId);
        if (financialYearId != 0) p.put("@FinancialYearId", financialYearId);
        if (branchId != 0) p.put("@BranchesId", branchId);
        p.put("@Activity", "ReadSerialNumber");
        List<Map<String, Object>> r = query("Sp_InvFoodProduction_GetAllMethod", p);
        return r.isEmpty() ? 0 : intOf(col(r.get(0), "DocCode"));
    }

    /**
     * InvFoodProduction.DropDownForConsumptionForm(org, comp, activityType, branchesIds) (BLL :6574)
     * -> [dbo].[USP_DropDownForConsumptionForm]. @ActivityType is always added - the desktop passes
     * null, which SqlClient does not send. @BranchesIds is added unless it is "" (a null is again
     * not sent): the main form passes UserAccount.BranchesId.ToString(), the issuance loader null.
     */
    public List<Map<String, Object>> dropDownForConsumptionForm(int org, int comp, String branchesIds) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", org);
        p.put("@CompanyId", comp);
        if (branchesIds != null && !branchesIds.isEmpty()) p.put("@BranchesIds", branchesIds);
        return query("[dbo].[USP_DropDownForConsumptionForm]", p);
    }

    /**
     * CommonServices.GetWeightCurrStockByItem -> InvSaleInvoice.GetWeightCurrStockByItem (BLL
     * Inventory :0x179fe4): [Sp_InvSaleInvoice_GetAllMethod] @OrganizationId, @CompanyId, @ItemId,
     * @ToDate, [@WarehouseId], [@JobLotId], [@CropYear when != ""], @Activity='GetWeightCurrStockByItem'.
     */
    public List<Map<String, Object>> weightCurrStockByItem(int org, int comp, int itemId, LocalDateTime toDate,
                                                           int warehouseId, int jobLotId, String cropYear) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", org);
        p.put("@CompanyId", comp);
        p.put("@ItemId", itemId);
        p.put("@ToDate", toDate);
        if (warehouseId != 0) p.put("@WarehouseId", warehouseId);
        if (jobLotId != 0) p.put("@JobLotId", jobLotId);
        if (cropYear != null && !cropYear.isEmpty()) p.put("@CropYear", cropYear);
        p.put("@Activity", "GetWeightCurrStockByItem");
        return query("[Sp_InvSaleInvoice_GetAllMethod]", p);
    }

    /**
     * CommonServices.AvgRateOnlyForCGS -> GetAvgRatesAndStockInHand.AvgRateOnlyForCGS (BLL):
     * Sp_GetAvgRatesAndStockInHand_GetAllMethod @OrganizationId, @CompanyId, @ItemId, @DocDate,
     * [@DocumentTypeId], [@RecId], [@JobLotId], [@CropYearId], [@CropYear when not null/empty],
     * [@WarehouseId], [@BranchesId], @Activity='GetOnlyAvgRateForCGS'. Returns AvgRate of row 0.
     */
    public double avgRateOnlyForCgs(int org, int comp, int itemId, LocalDateTime docDate, int documentTypeId,
                                    int recId, int jobLotId, int cropYearId, String cropYear, int warehouseId,
                                    int branchesId) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", org);
        p.put("@CompanyId", comp);
        p.put("@ItemId", itemId);
        p.put("@DocDate", docDate);
        if (documentTypeId != 0) p.put("@DocumentTypeId", documentTypeId);
        if (recId != 0) p.put("@RecId", recId);
        if (jobLotId != 0) p.put("@JobLotId", jobLotId);
        if (cropYearId != 0) p.put("@CropYearId", cropYearId);
        if (cropYear != null && !cropYear.isEmpty()) p.put("@CropYear", cropYear);
        if (warehouseId != 0) p.put("@WarehouseId", warehouseId);
        if (branchesId != 0) p.put("@BranchesId", branchesId);
        p.put("@Activity", "GetOnlyAvgRateForCGS");
        List<Map<String, Object>> r = query("Sp_GetAvgRatesAndStockInHand_GetAllMethod", p);
        return r.isEmpty() ? 0d : dbl(col(r.get(0), "AvgRate"));
    }

    /**
     * GetAvgRatesAndStockInHand.GetStockByFifoMethod (BLL) and CommonServices.FIFOImplemention
     * (DAL) - both USP_GetStockByFifoMethod with @OrganizationId, @CompanyId, @ItemId, @DocDate and
     * the rest only when set. The DAL form also sends @FIFOXML (the reservations already taken by
     * earlier rows of the same document) when it has any.
     */
    public List<Map<String, Object>> stockByFifo(int org, int comp, int itemId, LocalDateTime docDate, int stockUom,
                                                 int warehouseId, int cropYearId, int jobLotId, int packingTypeId,
                                                 String cropYear, int documentTypeId, int id, String fifoXml,
                                                 boolean bllOrder) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", org);
        p.put("@CompanyId", comp);
        p.put("@ItemId", itemId);
        if (bllOrder) {
            /* BLL order: stockUOM, DocDate, Warehouse, JobLot, PackingType, CropYearId, CropYear, DocType, Id. */
            if (stockUom != 0) p.put("@PackUomId", stockUom);
            p.put("@DocDate", docDate);
            if (warehouseId != 0) p.put("@WarehouseId", warehouseId);
            if (jobLotId != 0) p.put("@JobLotId", jobLotId);
            if (packingTypeId != 0) p.put("@PackingTypeId", packingTypeId);
            if (cropYearId != 0) p.put("@CropYearId", cropYearId);
            if (cropYear != null && !cropYear.isEmpty()) p.put("@CropYear", cropYear);
        } else {
            /* DAL order: DocDate, stockUOM, Warehouse, CropYearId, JobLot, PackingType, CropYear. */
            p.put("@DocDate", docDate);
            if (stockUom != 0) p.put("@PackUomId", stockUom);
            if (warehouseId != 0) p.put("@WarehouseId", warehouseId);
            if (cropYearId != 0) p.put("@CropYearId", cropYearId);
            if (jobLotId != 0) p.put("@JobLotId", jobLotId);
            if (packingTypeId != 0) p.put("@PackingTypeId", packingTypeId);
            if (cropYear != null && !cropYear.isEmpty()) p.put("@CropYear", cropYear);
        }
        if (documentTypeId != 0) p.put("@DocumentTypeId", documentTypeId);
        if (id != 0) p.put("@Id", id);
        if (fifoXml != null) p.put("@FIFOXML", fifoXml);
        return query("USP_GetStockByFifoMethod", p);
    }

    /**
     * CommonServices.GetUomScheduleByItemId (WinApp :4797) -> UOMSchedule.SearchByObject:
     * Sp_UOMSchedule_GetAllMethod @OrganizationId, @CompanyId, @ItemId, @Activity='ReadByItemID'.
     */
    public List<Map<String, Object>> uomScheduleByItemId(int org, int comp, int itemId) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", org);
        p.put("@CompanyId", comp);
        p.put("@ItemId", itemId);
        p.put("@Activity", "ReadByItemID");
        return query("Sp_UOMSchedule_GetAllMethod", p);
    }

    /**
     * InvFoodProduction.GetByID (BLL :5161) -> DAL GetDate: Sp_InvFoodProduction_GetAllMethod
     * @Id, @Activity='GetById'; then for the header Sp_InvFoodProductionDetail_GetALlMethod and
     * USP_InvFoodProductionConsumptionDetail_GetALlMethod, each @Id, @Activity='InvFoodProductionId'.
     * Returns null when the header is not found (the desktop's `SC == null`).
     */
    public Map<String, Object> getById(int id) {
        Map<String, Object> p = params();
        p.put("@Id", id);
        p.put("@Activity", "GetById");
        List<Map<String, Object>> h = query("Sp_InvFoodProduction_GetAllMethod", p);
        if (h.isEmpty()) return null;
        Map<String, Object> out = new LinkedHashMap<>(h.get(0));
        Map<String, Object> d = params();
        d.put("@Id", id);
        d.put("@Activity", "InvFoodProductionId");
        out.put("invFoodProductionDetails", query("Sp_InvFoodProductionDetail_GetALlMethod", d));
        Map<String, Object> c = params();
        c.put("@Id", id);
        c.put("@Activity", "InvFoodProductionId");
        out.put("InvFoodProductionConsumptionDetailslist", query("USP_InvFoodProductionConsumptionDetail_GetALlMethod", c));
        return out;
    }

    /**
     * InvFoodProduction.ProductionFormHistory (BLL :5452): Sp_InvFoodProduction_GetAllMethod
     * @OrganizationId, @CompanyId, [@FinancialYearId], [@BranchesId], [@FromDate], [@ToDate],
     * [@DocNoFrom when != 0], [@DocNoTo when != 0], [@InvJobOrderId], [@PlantId],
     * [@DocumentTypeId <- RefDocumentTypeId, never set by this form], @Activity='ReadAll'.
     */
    public List<Map<String, Object>> formHistory(int org, int comp, int financialYearId, int branchesId,
                                                 LocalDateTime from, LocalDateTime to, double docNoFrom,
                                                 double docNoTo, int jobOrderId, int plantId) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", org);
        p.put("@CompanyId", comp);
        if (financialYearId != 0) p.put("@FinancialYearId", financialYearId);
        if (branchesId != 0) p.put("@BranchesId", branchesId);
        put(p, "@FromDate", from);
        put(p, "@ToDate", to);
        if (docNoFrom != 0d) p.put("@DocNoFrom", docNoFrom);
        if (docNoTo != 0d) p.put("@DocNoTo", docNoTo);
        if (jobOrderId != 0) p.put("@InvJobOrderId", jobOrderId);
        if (plantId != 0) p.put("@PlantId", plantId);
        p.put("@Activity", "ReadAll");
        return query("Sp_InvFoodProduction_GetAllMethod", p);
    }

    /**
     * InvFoodProduction.GetDataForDropDownFromFoodProduction (BLL :6821):
     * USP_GetDataForDropDownFromFoodProduction @OrganizationId, @CompanyId, [@FinancialYearId],
     * [@Activity when not empty], [@DocumentTypeIds when not empty], [@BranchesIds when not empty],
     * [@ActionId when > 0 - never set here].
     */
    public List<Map<String, Object>> dropDownFromFoodProduction(int org, int comp, int financialYearId,
                                                                String activity, String documentTypeIds,
                                                                String branchesIds) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", org);
        p.put("@CompanyId", comp);
        if (financialYearId != 0) p.put("@FinancialYearId", financialYearId);
        if (activity != null && !activity.isEmpty()) p.put("@Activity", activity);
        if (documentTypeIds != null && !documentTypeIds.isEmpty()) p.put("@DocumentTypeIds", documentTypeIds);
        if (branchesIds != null && !branchesIds.isEmpty()) p.put("@BranchesIds", branchesIds);
        return query("USP_GetDataForDropDownFromFoodProduction", p);
    }

    /**
     * LoadConsumptionPendingforRates.JobOrderFill:113 -> InvProductionJobOrder.GetJobOrderNoForInvFoodProduction:
     * Sp_InvProductionJobOrder_GetAllMethod @OrganizationId, @CompanyId, [@DocumentTypeId = 403],
     * [@FinancialYearId], [@PlanStatus - Status is never set], @Activity='GetJobOrderNoForInvFoodProduction'.
     */
    public List<Map<String, Object>> jobOrderNoForInvFoodProduction(int org, int comp, int documentTypeId,
                                                                    int financialYearId) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", org);
        p.put("@CompanyId", comp);
        if (documentTypeId != 0) p.put("@DocumentTypeId", documentTypeId);
        if (financialYearId != 0) p.put("@FinancialYearId", financialYearId);
        p.put("@Activity", "GetJobOrderNoForInvFoodProduction");
        return query("Sp_InvProductionJobOrder_GetAllMethod", p);
    }

    /**
     * LoadConsumptionPendingforRates.OutputGridHistory:189 -> InvFoodProduction.GetProductionConsumptionDataForPendingRates
     * (BLL :6632): [dbo].[USP_GetProductionConsumptionDataForPendingRates] @OrganizationId,
     * @CompanyId, @BranchId, [@InvProductionJobOrderId], [@FromDate], [@ToDate], [@FromDocNo],
     * [@ToDocNo] - the form never sets the doc-no pair, so they are never sent.
     */
    public List<Map<String, Object>> consumptionPendingForRates(int org, int comp, int branchId, int jobOrderId,
                                                                LocalDateTime from, LocalDateTime to) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", org);
        p.put("@CompanyId", comp);
        p.put("@BranchId", branchId);
        if (jobOrderId != 0) p.put("@InvProductionJobOrderId", jobOrderId);
        put(p, "@FromDate", from);
        put(p, "@ToDate", to);
        return query("[dbo].[USP_GetProductionConsumptionDataForPendingRates]", p);
    }

    /**
     * LoadavailableTransactionsForIssuanceOnConsumption.PendingInventoryTransactionsForIssuanceLoad:289
     * -> InventoryStockEvalautionDetail.GetavailableTransactionsForIssuanceOnConsumption (BLL):
     * [dbo].[USP_GetAvailableTransactionsForIssuanceOnProductionCosumption] @OrganizationId,
     * @CompanyId and each filter only when set, in the BLL's order.
     */
    public List<Map<String, Object>> availableTransactionsForIssuance(int org, int comp, LocalDateTime from,
                                                                      LocalDateTime to, int supplierCustomerId,
                                                                      int warehouseId, int itemId,
                                                                      int refDocumentTypeId, int jobLotId,
                                                                      int inventoryParentCategories,
                                                                      int itemCategoryId, int itemTypeId,
                                                                      String cropYear) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", org);
        p.put("@CompanyId", comp);
        put(p, "@DateFrom", from);
        put(p, "@DateTo", to);
        if (supplierCustomerId != 0) p.put("@SupplierCustomerId", supplierCustomerId);
        if (warehouseId != 0) p.put("@WarehouseId", warehouseId);
        if (itemId != 0) p.put("@ItemId", itemId);
        if (refDocumentTypeId != 0) p.put("@ReferenceDocumentTypeId", refDocumentTypeId);
        if (jobLotId != 0) p.put("@JobLotId", jobLotId);
        if (inventoryParentCategories != 0) p.put("@InventoryParentCategories", inventoryParentCategories);
        if (itemCategoryId != 0) p.put("@ItemCategoryId", itemCategoryId);
        if (itemTypeId != 0) p.put("@ItemTypeId", itemTypeId);
        if (cropYear != null && !cropYear.isEmpty()) p.put("@CropYear", cropYear);
        /* @WarehouseIds / @ItemIds: the form never sets them. */
        return query("[dbo].[USP_GetAvailableTransactionsForIssuanceOnProductionCosumption]", p);
    }

    // ============================================================================ posting-engine reads

    /** DAL CommonServices.GetConfigurationFromAllocation - ConfigKey of the named row, "" when absent. */
    public String configFromAllocation(int org, int comp, String configDescription) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", org);
        p.put("@CompanyId", comp);
        p.put("@ConfigDescription", configDescription);
        p.put("@Activity", "GetConfigurationByOrgCompandConfigDescription");
        List<Map<String, Object>> r = query("Sp_ConfigrationsAllocation_GetAllMethod", p);
        return r.isEmpty() ? "" : str(col(r.get(0), "ConfigKey")).trim();
    }

    /** DAL CommonServices.GetERPFeaturesByCompanyId(org, comp, featureId). */
    public boolean erpFeature(int org, int comp, int featureId) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", org);
        p.put("@CompanyId", comp);
        for (Map<String, Object> r : query("USP_GetERPFeaturesByCompanyId", p)) {
            if (intOf(col(r, "Id")) == featureId) return true;
        }
        return false;
    }

    /**
     * GetItemGlIdsandItemName - BLL CommonServies.GetItemListForFinancialEffects (MakeVoucher) and
     * DAL CommonServices.GetItemGlIdsandItemName (SetData) are the same call:
     * Sp_Item_GetAllMethod @OrganizationId, @CompanyId, @Activity='GetItemGlIdsandItemName'.
     * Columns: Id, ItemName, PurchaseGLAC, SaleGLAC, COGSGLAC.
     */
    public List<Map<String, Object>> itemGlIdsAndItemName(int org, int comp) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", org);
        p.put("@CompanyId", comp);
        p.put("@Activity", "GetItemGlIdsandItemName");
        return query("Sp_Item_GetAllMethod", p);
    }

    /** DAL CommonServices.GetJobLotGlIdsandName: [dbo].[SP_JobLot_ReadMethod] @OrganizationId, @CompanyId, @Activity. */
    public List<Map<String, Object>> jobLotGlIdsAndName(int org, int comp) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", org);
        p.put("@CompanyId", comp);
        p.put("@Activity", "GetJobLotGlIdsandName");
        return query("[dbo].[SP_JobLot_ReadMethod]", p);
    }

    /**
     * DAL CommonServices.GetEqvilentByItemIdAndUomScheduleId: Sp_Item_GetAllMethod @OrganizationId,
     * @CompanyId, @ItemId, @ScheduleId, @Activity='GetEqvilentByItemIdAndUomScheduleId' -> Equivalent.
     */
    public double equivalentByItemAndSchedule(int org, int comp, int itemId, int scheduleId) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", org);
        p.put("@CompanyId", comp);
        p.put("@ItemId", itemId);
        p.put("@ScheduleId", scheduleId);
        p.put("@Activity", "GetEqvilentByItemIdAndUomScheduleId");
        List<Map<String, Object>> r = query("Sp_Item_GetAllMethod", p);
        return r.isEmpty() ? 0d : dbl(col(r.get(0), "Equivalent"));
    }

    /**
     * DAL CommonServices.GetVoucherHeadId(org, comp, docType, id): Sp_Vouchers_GetMethods
     * @Activity='GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId', @OrganizationId,
     * @CompanyId, @DocumentTypeId, @DocumentTypeSrNo.
     */
    public int voucherHeadIdForDocument(int org, int comp, int documentTypeId, int id) {
        Map<String, Object> p = params();
        p.put("@Activity", "GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId");
        p.put("@OrganizationId", org);
        p.put("@CompanyId", comp);
        p.put("@DocumentTypeId", documentTypeId);
        p.put("@DocumentTypeSrNo", id);
        List<Map<String, Object>> r = query("Sp_Vouchers_GetMethods", p);
        return r.isEmpty() ? 0 : intOf(col(r.get(0), "Id"));
    }

    // ================================================================================= posting writes

    /** GenericProvider.SetProc - ExecuteScalar with the model's properties. */
    public int setProc(String proc, Map<String, Object> modelParams) {
        Map<String, Object> p = params();
        for (Map.Entry<String, Object> e : modelParams.entrySet()) put(p, e.getKey(), e.getValue());
        return intOf(scalar(proc, p));
    }

    /** SqlCommand with explicit AddWithValue parameters - ExecuteNonQuery / ExecuteScalar. */
    public void command(String proc, Map<String, Object> p) {
        Map<String, Object> q = params();
        for (Map.Entry<String, Object> e : p.entrySet()) put(q, e.getKey(), e.getValue());
        scalar(proc, q);
    }

    /**
     * WagesDeleteByReferenceIds -> InvContractorWagesBillHeader.ContractorWagesRemovebyReferenceIds:
     * usp_ContractorWagesRemovebyReferenceIds @OrganizationId, @CompanyId, @DocumentTypeId, @Id.
     * NOT called by the web save (see ProductionConsumptionService.save) - kept for completeness.
     */
    public void contractorWagesRemoveByReferenceIds(int org, int comp, int documentTypeId, int id) {
        Map<String, Object> p = params();
        p.put("@OrganizationId", org);
        p.put("@CompanyId", comp);
        p.put("@DocumentTypeId", documentTypeId);
        p.put("@Id", id);
        scalar("usp_ContractorWagesRemovebyReferenceIds", p);
    }
}
