package com.mst.services.sale.pcc;

import com.mst.models.UserAccount;
import com.mst.security.DesktopReportRights;
import com.mst.services.sale.engr.SaleEngrSupport;
import org.springframework.stereotype.Component;

import java.util.*;

import static com.mst.services.sale.engr.SaleEngrSupport.*;

/**
 * Lookups shared by the Sale Pcc ("Concrete") forms of module 85 (Architecture.WinApp.pcc.Sale.*). Every method is one desktop call:
 * the procedure and parameters are the BLL/DAL's, the organization / company / branch / financial year always come from CurrentUserContext
 * (through SaleEngrSupport), never from the browser.
 */
@Component
public class SalePccLookups {
    private final SaleEngrSupport sup;
    private final DesktopReportRights screenRights;

    public SalePccLookups(SaleEngrSupport sup, DesktopReportRights screenRights) { this.sup = sup; this.screenRights = screenRights; }

    public SaleEngrSupport sup() { return sup; }

    /** The desktop opens a screen only when the user holds its View right (ScreenDefinition id). */
    public UserAccount requireView(int screenId) {
        UserAccount u = sup.user();
        screenRights.require(u, screenId, "View");
        return u;
    }

    /** Save / Update / Delete buttons are disabled on the desktop without the right; the web refuses the call. */
    public void need(Map<String, Boolean> rights, String key, String what) {
        if (!Boolean.TRUE.equals(rights.get(key))) throw new IllegalArgumentException("You do not have " + what + " rights on this screen");
    }

    /** CommonServices.SupplierCustomerGetforComboServiceBind / GetVendorsAndCustomers(2) when ERP feature 4 (subsidiary accounts on vouchers) is on. */
    public List<Map<String, Object>> customers() {
        if (sup.erpFeature(4)) return sup.rows("USP_GetVendorsAndCustomers", "OrganizationId", sup.org(), "CompanyId", sup.company(), "PartyTypeId", 2);
        return sup.rows("Sp_SupplierCustomer_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "ReadByOrganizationIdCompanyIdForBinding");
    }

    /** CommonServices.getActiveWareHouse. */
    public List<Map<String, Object>> warehouses() {
        return sup.rows("Sp_InvWareHouse_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "GetActiveWareHouse");
    }

    /** CommonServices.JobLotGetAllService. */
    public List<Map<String, Object>> jobLots() {
        return sup.rows("SP_JobLot_ReadMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "GetAll");
    }

    /** CommonServices.CityGetAllService = City.GetAll. */
    public List<Map<String, Object>> cities() {
        return sup.rows("SP_City_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "MethodType", "GetAll");
    }

    /** VehicleType.GetAll(). */
    public List<Map<String, Object>> vehicleTypes() { return sup.rows("Sp_VehicleType_GetAllMethod"); }

    /** DefineVehicleWeight.ReadAll (no financial year) - the vehicle numbers of the weighbridge definitions. */
    public List<Map<String, Object>> vehicles() {
        return sup.rows("USp_DefineVehicleWeight_FormHistory", "OrganizationId", sup.org(), "CompanyId", sup.company());
    }

    /** pcc LookUps.GetByLookUpTypeId: 1 = Building Height, 2 = Building Storey. */
    public List<Map<String, Object>> buildingLookups(int typeId) {
        return sup.rows("[pcc].[USP_LookUps_GetAllMethod]", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Id", typeId, "Activity", "GetByLookUpTypeId");
    }

    /** ReferenceParties.ReferencePArtyByReferencePartyTypeIdOrSupplierCustomerId with ReferencePartyTypeId = 1. */
    public List<Map<String, Object>> referenceParties() {
        return sup.rows("Sp_ReferenceParties_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "ReferencePartyTypeId", 1,
                "Activity", "ReferencePArtyByReferencePartyTypeIdOrSupplierCustomerId");
    }

    /** ItemAttributeVarient.GetAllForCombo. */
    public List<Map<String, Object>> varients(int itemId) {
        return sup.rows("[dbo].[USP_ItemAttributeVarient_GetAllMethod]", "OrganizationId", sup.org(), "CompanyId", sup.company(), "ItemId", itemId, "Activity", "GetAllForCombo");
    }

    /** pcc SaleOrder.SaleOrderIdandNoGetForSupplierCustomerId (DocumentTypeId 1852). */
    public List<Map<String, Object>> orderNos(int customerId, int documentTypeId) {
        return sup.rows("[pcc].[USP_SaleOrder_GetAllMethod]", "OrganizationId", sup.org(), "CompanyId", sup.company(), "SupplierCustomerId", customerId,
                "DocumentTypeId", documentTypeId, "Activity", "GetOrderIdAndNoBySupplierCustomerId");
    }

    /** pcc SaleOrder.GetSaleOrderDetailbyOrderId. */
    public List<Map<String, Object>> orderItems(int orderId) {
        return sup.rows("[pcc].[USP_SaleOrder_GetAllMethod]", "OrderId", orderId, "Activity", "GetSaleOrderDetailbyOrderId");
    }

    /** GetAvgRatesAndStockInHand.GetStockInHandAndAvgRateFromEvaluationConcrete: QtyInHand of the first row (0 when none). */
    public double stockInHand(int itemId, java.time.LocalDate docDate, int warehouseId, int jobLotId, int uomId) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", sup.org()); p.put("CompanyId", sup.company()); p.put("ItemId", itemId); p.put("DocDate", docDate);
        if (warehouseId != 0) p.put("WarehouseId", warehouseId);
        if (jobLotId != 0) p.put("JobLotId", jobLotId);
        if (uomId != 0) p.put("ItemUomId", uomId);
        p.put("Activity", "GetStockInHandAndAvgRateFromEvaluation");
        List<Map<String, Object>> r = com.mst.repositories.support.DesktopProc.rows(sup.jdbc(), "Sp_GetAvgRatesAndStockInHand_GetAllMethod", p);
        return r.isEmpty() ? 0.0 : toDecimal(r.get(0).get("QtyInHand")).doubleValue();
    }

    /** StocksReport.StockReportComboByOrganizationAndCompanyId (loader dialogs): rows with ActivityType + Id + name. */
    public List<Map<String, Object>> stockCombo() {
        return sup.rows("Sp_Inventory_InventoryTransactions_DropDownAndLists", "OrganizationId", sup.org(), "CompanyId", sup.company());
    }

    /** The loader dialogs' five filter combos, split by ActivityType (ItemTypes, ItemCategories, Items, Supplier_Customer, ParentCategories). */
    public Map<String, Object> loaderCombos() {
        List<Map<String, Object>> type = new ArrayList<>(), cat = new ArrayList<>(), item = new ArrayList<>(), cust = new ArrayList<>(), par = new ArrayList<>();
        for (Map<String, Object> r : stockCombo()) {
            Map<String, Object> o = row("Id", r.get("Id"), "name", r.get("name"));
            switch (str(ci(r, "ActivityType"))) {
                case "ItemTypes" -> type.add(o);
                case "ItemCategories" -> cat.add(o);
                case "Items" -> item.add(o);
                case "Supplier_Customer" -> cust.add(o);
                case "ParentCategories" -> par.add(o);
                default -> { }
            }
        }
        return row("itemTypes", type, "categories", cat, "items", item, "customers", cust, "parentCategories", par);
    }

    /** clsGlobalVariables.ActiveYr.Start_Period (loader dialogs open with FromDate = start of the financial year). */
    public String fyStart() {
        for (Map<String, Object> r : sup.rows("Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId", "OrganizationId", sup.org(), "CompanyId", sup.company()))
            if (toInt(r.get("Id")) == sup.fy()) return str(r.get("Start_Period"));
        return "";
    }

    /** CommonServices.DateType(): the history tab's date-range combo. */
    public static List<Map<String, Object>> dateTypes() {
        return List.of(row("Id", 1, "Parameters", "This Day"), row("Id", 2, "Parameters", "This Week"), row("Id", 3, "Parameters", "This Month"),
                row("Id", 4, "Parameters", "This Year"), row("Id", 5, "Parameters", "Financial Year"));
    }

    public static java.time.LocalDate parseDate(String s) {
        if (s == null || s.isBlank()) return null;
        try { return java.time.LocalDate.parse(s.trim().substring(0, Math.min(10, s.trim().length()))); } catch (RuntimeException e) { return null; }
    }
}
