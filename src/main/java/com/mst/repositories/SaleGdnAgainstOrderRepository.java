package com.mst.repositories;

import com.mst.models.UserAccount;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.util.*;

/**
 * Screen 142 "Goods Dispatch Notes" - Architecture.WinApp.Sale.frmGdnAgainstSaleOrder, DocumentTypeId 170.
 * Saving, reading one record, deleting and the stock chain are the generic GDN backend in
 * {@link SaleGdnPurchaseReturnRepository} (InvGdn.Save -> DAL 0428 SetData); this class carries what is specific to the form:
 * its own lists (the form's *Fill methods), the filtered history (HistoryGridFill :1818), the stock label (AvailableStock :2370)
 * and the sale-order loader (frmLoadSaleOrder).
 */
@Repository("saleGdnAgainstOrderRepository")
public class SaleGdnAgainstOrderRepository extends SaleGdnPurchaseReturnRepository {
    public static final int DOC = 170;
    public static final String SCREEN = "frmGdnAgainstSaleOrder";

    public SaleGdnAgainstOrderRepository(JdbcTemplate j) { super(j); }

    @Override protected int documentTypeId() { return DOC; }
    @Override protected String screenName() { return SCREEN; }
    @Override protected String recordLabel() { return "GDN Against Sale Order"; }

    /** CommonServices.InvGdnGenerateCode(170) - GenerateCode(). */
    public Object nextNo(UserAccount u, int year) {
        var no = q("EXEC dbo.Sp_InvGdn_GetAllMethod @OrganizationId=?,@CompanyId=?,@DocumentTypeId=?,@FinancialYearId=?,@BranchesId=?,@Activity='GenerateInvGdnCode'",
                u.getOrganizationId(), u.getCompanyId(), DOC, year, u.getBranchesId());
        return no.isEmpty() ? 1 : no.get(0).get("DocNo");
    }

    /** The form's Load (:341-:420) lists, in the order it fills them. */
    @Override
    public Map<String, Object> initial(UserAccount u, int year) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("nextNo", nextNo(u, year));
        boolean sub = feature(u, 4);                                                    /* SubsidiaryAccountAllownOnVouchers :350 */
        m.put("subsidiary", sub);
        /* SupplierNameFilll :506/:515 */
        m.put("customers", sub
                ? q("EXEC dbo.USP_GetVendorsAndCustomers @OrganizationId=?,@CompanyId=?,@PartyTypeId=2", u.getOrganizationId(), u.getCompanyId())
                : q("EXEC dbo.Sp_SupplierCustomer_GetAllMethod @OrganizationId=?,@CompanyId=?,@Activity='ReadByOrganizationIdCompanyIdForBinding'", u.getOrganizationId(), u.getCompanyId()));
        /* TransportFill :596 - feature 4: GetVendorsAndCustomersForTransporter; else CoaAllocationGetAllServiceBind without AccountTypeId 2, 15, 11 */
        m.put("transporters", sub ? transporterParties(u, false) : coaAccounts(u, t -> t != 2 && t != 15 && t != 11));
        /* WarehouseFill :690 - CommonServices.getActiveWareHouse */
        m.put("warehouses", q("EXEC dbo.Sp_InvWareHouse_GetAllMethod @OrganizationId=?,@CompanyId=?,@Activity='GetActiveWareHouse'", u.getOrganizationId(), u.getCompanyId()));
        m.put("items", items(u, 0));
        /* cropyear :775 - CommonServices.CropYearGetAllService */
        m.put("cropYears", q("EXEC dbo.Sp_InvCropYear_GetAllMethod @OrganizationId=?,@CompanyId=?,@Activity='ReadAll'", u.getOrganizationId(), u.getCompanyId()));
        /* JobLotFill :805 - CommonServices.JobLotGetAllService */
        m.put("jobLots", q("EXEC dbo.SP_JobLot_ReadMethod @OrganizationId=?,@CompanyId=?,@Activity='GetAll'", u.getOrganizationId(), u.getCompanyId()));
        m.put("packingTypes", q("EXEC dbo.Sp_InvPackingType_GetAllMethod @Activity='ReadAll'"));                 /* :840 */
        /* CityFill :886 - City.GetAll */
        m.put("cities", q("EXEC dbo.SP_City_GetAllMethod @OrganizationId=?,@CompanyId=?,@MethodType='GetAll'", u.getOrganizationId(), u.getCompanyId()));
        m.put("deliveryTerms", List.of(term(1, "Load"), term(2, "Ponch")));                                      /* StaticColumnsService("DeliveryTerm") */
        Map<String, Object> cfg = new LinkedHashMap<>();
        cfg.put("itemSearchByCode", truthy(config(u, "ItemSearchByCode")));
        cfg.put("defaultCity", config(u, "City Area"));
        cfg.put("defaultJobLot", config(u, "Job/Lot"));
        cfg.put("defaultCropYear", config(u, "Default Crop Year"));
        cfg.put("defaultPackingType", config(u, "Paking Type"));
        cfg.put("defaultWarehouse", config(u, "Warehouse"));
        String days = config(u, "DefaultDaysToLessFromHistoryFromDate");
        int d = 0; try { d = (int) Double.parseDouble(days); } catch (Exception ignored) { /* not a number */ }
        cfg.put("historyDays", d);
        cfg.put("branchWiseOrders", truthy(config(u, "SaleOrderBranchWise")));
        m.put("config", cfg);
        return m;
    }

    private static Map<String, Object> term(int id, String type) {
        Map<String, Object> x = new LinkedHashMap<>(); x.put("Id", id); x.put("type", type); return x;
    }

    /** ReadAllItems (Item.ReadAllItems) - Id, ItemName, ItemCode from ItemCodeNew (:727). */
    @Override
    public List<Map<String, Object>> items(UserAccount u, int ignored) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : q("EXEC dbo.Sp_Item_GetAllMethod @OrganizationId=?,@CompanyId=?,@Activity='ReadAllItems'", u.getOrganizationId(), u.getCompanyId())) {
            Map<String, Object> x = new LinkedHashMap<>();
            x.put("Id", r.get("Id")); x.put("ItemName", r.get("ItemName")); x.put("ItemCode", r.get("ItemCodeNew"));
            out.add(x);
        }
        return out;
    }

    /** PackUom :866 - CommonServices.GetUomScheduleByItemId = Sp_UOMSchedule_GetAllMethod 'ReadByItemID'. */
    @Override
    public List<Map<String, Object>> uoms(UserAccount u, int itemId) {
        return q("EXEC dbo.Sp_UOMSchedule_GetAllMethod @OrganizationId=?,@CompanyId=?,@ItemId=?,@Activity='ReadByItemID'", u.getOrganizationId(), u.getCompanyId(), itemId);
    }

    /** AvailableStock :2370 - BLL 0056 GetStockInHandFromInventoryTrasactions; zero parameters are not sent; Rows[0][0] else 0. */
    public double stock(UserAccount u, int itemId, String cropYear, Object docDate, int jobLotId, int warehouseId) {
        StringBuilder sql = new StringBuilder("EXEC dbo.Sp_GetAvgRatesAndStockInHand_GetAllMethod @OrganizationId=?,@CompanyId=?,@ItemId=?,@DocDate=?");
        List<Object> a = new ArrayList<>(Arrays.asList(u.getOrganizationId(), u.getCompanyId(), itemId, docDate));
        if (warehouseId != 0) { sql.append(",@WarehouseId=?"); a.add(warehouseId); }
        if (jobLotId != 0) { sql.append(",@JobLotId=?"); a.add(jobLotId); }
        if (cropYear != null && !cropYear.isEmpty()) { sql.append(",@CropYear=?"); a.add(cropYear); }
        sql.append(",@Activity='GetStockInHandFromInventoryTrasactions'");
        var rows = q(sql.toString(), a.toArray());
        if (rows.isEmpty()) return 0d;
        Object v = rows.get(0).values().iterator().next();
        return v instanceof Number ? ((Number) v).doubleValue() : 0d;
    }

    /** HistoryCombosFill :1770 - USP_GetDataForDropDownFromGdn, the 'Supplier' rows (Id, ReferenceName). */
    public List<Map<String, Object>> historyCustomers(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : q("EXEC dbo.USP_GetDataForDropDownFromGdn @OrganizationId=?,@CompanyId=?", u.getOrganizationId(), u.getCompanyId())) {
            if (!"Supplier".equals(String.valueOf(r.get("Activity")))) continue;
            Map<String, Object> x = new LinkedHashMap<>();
            x.put("Id", r.get("Id")); x.put("Customer", r.get("ReferenceName"));
            out.add(x);
        }
        return out;
    }

    /** HistoryGridFill :1818 - BLL 0575 InvGdn.GetHisoty ('GDNFormHistory'); each parameter only when the form sends it. */
    public List<Map<String, Object>> historyFiltered(UserAccount u, int year, boolean canViewAll, String mode, String from, String to,
                                                     int fromNo, int toNo, int customerId) {
        StringBuilder sql = new StringBuilder("EXEC dbo.Sp_InvGdn_GetAllMethod @OrganizationId=?,@CompanyId=?,@DocumentTypeId=?");
        List<Object> a = new ArrayList<>(Arrays.asList(u.getOrganizationId(), u.getCompanyId(), DOC));
        if (year != 0) { sql.append(",@FinancialYearId=?"); a.add(year); }
        if (u.getBranchesId() != 0) { sql.append(",@BranchesId=?"); a.add(u.getBranchesId()); }
        String fk, tk;
        switch (mode == null ? "" : mode) {
            case "entry": fk = "EntryFromDate"; tk = "EntryToDate"; break;
            case "modify": fk = "ModifyFromDate"; tk = "ModifyToDate"; break;
            case "approved": fk = "ApprovedFromDate"; tk = "ApprovedToDate"; break;
            default: fk = "FromDate"; tk = "ToDate"; break;
        }
        if (from != null && !from.isBlank()) { sql.append(",@").append(fk).append("=?"); a.add(from); }
        if (to != null && !to.isBlank()) { sql.append(",@").append(tk).append("=?"); a.add(to); }
        if (fromNo != 0) { sql.append(",@DocNoFrom=?"); a.add(fromNo); }
        if (toNo != 0) { sql.append(",@DocNoTo=?"); a.add(toNo); }
        if (customerId != 0) { sql.append(",@SupplierCustomerId=?"); a.add(customerId); }
        sql.append(",@CanViewAllRecord=?"); a.add(canViewAll ? 1 : 0);
        if (!canViewAll) { sql.append(",@EntryUser=?"); a.add(u.getId()); }
        sql.append(",@Activity='GDNFormHistory'");
        return q(sql.toString(), a.toArray());
    }

    // ----------------------------------------------------------------------------------------- frmLoadSaleOrder

    /** BranchFill: with config SaleOrderBranchWise only the user's own branch; else USP_GetBranchsAllocatedToUserFromSaleOrder (DocumentTypeId 81). */
    public List<Map<String, Object>> loaderBranches(UserAccount u, boolean branchWise) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (branchWise) {
            Map<String, Object> x = new LinkedHashMap<>();
            x.put("Id", u.getBranchesId()); x.put("BranchName", branchName(u));
            out.add(x);
            return out;
        }
        for (var r : q("EXEC dbo.USP_GetBranchsAllocatedToUserFromSaleOrder @OrganizationId=?,@CompanyId=?,@UserId=?,@DocumentTypeId=81", u.getOrganizationId(), u.getCompanyId(), u.getId())) {
            Map<String, Object> x = new LinkedHashMap<>();
            x.put("Id", r.get("BranchId")); x.put("BranchName", r.get("BranchName"));
            out.add(x);
        }
        return out;
    }

    private String branchName(UserAccount u) {
        var r = q("EXEC dbo.Sp_Branches_GetAllMethod @OrganizationId=?,@CompanyId=?,@Activity='GetAll'", u.getOrganizationId(), u.getCompanyId());
        for (var b : r) if (number(b.get("Id")) == u.getBranchesId()) return String.valueOf(b.get("BranchName"));
        return "";
    }

    /** HistoryCombosFill of the loader: USP_GetDataForDropDownFromSaleOrder (DocumentTypeIds '81'); 'Customer' and 'Item' rows. */
    public Map<String, Object> loaderCombos(UserAccount u, String branchIds, boolean branchWise) {
        String ids = branchWise ? String.valueOf(u.getBranchesId()) : (branchIds == null ? "" : branchIds);
        List<Map<String, Object>> cust = new ArrayList<>(), item = new ArrayList<>();
        for (var r : q("EXEC dbo.USP_GetDataForDropDownFromSaleOrder @OrganizationId=?,@CompanyId=?,@AppId=?,@UserId=?,@DocumentTypeIds='81',@BranchesIds=?",
                u.getOrganizationId(), u.getCompanyId(), u.getAppId(), u.getId(), ids)) {
            String act = String.valueOf(r.get("Activity"));
            Map<String, Object> x = new LinkedHashMap<>();
            x.put("Id", r.get("Id")); x.put("Name", r.get("ReferenceName"));
            if ("Customer".equals(act)) cust.add(x); else if ("Item".equals(act)) item.add(x);
        }
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("customers", cust); m.put("items", item);
        return m;
    }

    /** GridSecondViewFill (MainDetail false): BLL 0596 GetPendingSaleOrderDetailForDeliveryOrder = USP_GetPendingSaleOrderDetailForDeliveryOrder. */
    public List<Map<String, Object>> loaderPending(UserAccount u, String branchIds, int customerId, int itemId, String from, String to) {
        StringBuilder sql = new StringBuilder("EXEC dbo.USP_GetPendingSaleOrderDetailForDeliveryOrder @OrganizationId=?,@CompanyId=?");
        List<Object> a = new ArrayList<>(Arrays.asList(u.getOrganizationId(), u.getCompanyId()));
        if (customerId != 0) { sql.append(",@SupplierCustomerId=?"); a.add(customerId); }
        if (itemId != 0) { sql.append(",@ItemId=?"); a.add(itemId); }
        sql.append(",@BranchesIds=?"); a.add(branchIds);
        if (from != null && !from.isBlank()) { sql.append(",@FromDate=?"); a.add(from); }
        if (to != null && !to.isBlank()) { sql.append(",@ToDate=?"); a.add(to); }
        return q(sql.toString(), a.toArray());
    }

    /** LoadInGridDetail :2576 - BLL 0596 SaleOrder.LoadSaleOrderForGoodsDispatchNotes. */
    public List<Map<String, Object>> orderForGdn(UserAccount u, int orderId) {
        return q("EXEC dbo.Sp_SaleOrder_GetAllMethod @Id=?,@OrganizationId=?,@CompanyId=?,@Activity='LoadSaleOrderForGoodsDispatchNotes'", orderId, u.getOrganizationId(), u.getCompanyId());
    }

    /** GlobalVariables_Helper.GetConfigValueFromGlobal. */
    public String configValue(UserAccount u, String description) { return config(u, description); }

    /** InvPurchaseInvoice.RemoveByID(DocumentTypeId 170, Id) - the generic GDN delete. */
    public void remove(UserAccount u, int id) { delete(u, id); }
}
