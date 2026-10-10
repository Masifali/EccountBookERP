package com.mst.services.sale.bk;

import com.mst.models.UserAccount;
import org.springframework.stereotype.Service;

import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalTime;
import java.util.*;

/**
 * Screen 762 OrderDashboard = Architecture.WinApp.PreBookingAndDelivery.MyOrdersStatus ("My Order's Status"), module 2042.
 * BLL: Architecture.BLL.Inventory.PreBookingOrder (0523), InvGdn (0575), InvSaleInvoice (0580), UsersWithCostCenter (0516).
 *
 *   MyOrdersStatus_Load :120               fromdate = clsGlobalVariables.ActiveYr.Start_Period; AppId == 5: cost center label + combo visible, CostCenterFill, first row, combo disabled;
 *                                          otherwise both hidden; then btnShow_Click
 *   CostCenterFill :152                    UsersWithCostCenter_AllocatedData(Org, Company, UserId) -> USP_UsersWithCostCenter_AllocatedData, CostCenterId / CostCenterName
 *   btnShow_Click :184                     PreBookingOrder_Dashboard(Org, Company, UserId, SupplierCustomerId (>0 else 0), AppId, CostCenterId) = usp_PreBookingOrder_Dashboard
 *                                          -> one card per row: Caption = Activity, Value = Total.ToString("#,##0.##")
 *   UserControl_Clicked :210               card caption: "Booking / Demand" | "Pending Orders" | "Delivery In Transit" | "Sales Bill"
 *   GetBookingDemandData :258              PreBookingOrderDashboard_Detail(Org, Company, UserId, FromDate, ToDate, AppId, CostCenterId) = usp_PreBookingOrderDashboard_Detail (DataSet)
 *   GetPendingOrdersData :538              OutstandingOrders_PreDashboard(same arguments) = usp_OutstandingOrders_PreDashboard (DataSet)
 *   GetDeliveryInTransitData :819          InvGdn.getGdnDataForMobileApp(Org, Company, FromDate, ToDate, UserId, AppId, CostCenterId) = USP_getGdnDataForMobileApp
 *   GetSaleBillsData :930                  InvSaleInvoice.getSaleDataForMobileApp(same) = USP_getSaleDataForMobileApp
 *   grdSaleOrTransit_ColumnButtonClick :1035   Confirm -> InvGdn.GdnStatusUpdate(Id, Remarks) = USP_GdnStatusUpdate; ConfirmBill -> InvSaleInvoice.InvoiceStatusUpdate = USP_InvoiceStatusUpdate
 * Slips (Slip / SlipBill) are printed by the web app's existing shared endpoints /reports/print/260-gdn-rice-slip, 301-inv-rep-sale-bill-customer,
 * 294a-sale-bill-direct-without-so (the same templates the desktop opens), called from the page script.
 */
@Service
public class SaleBkOrderStatusService {
    private final SaleBkSupport bk;

    public SaleBkOrderStatusService(SaleBkSupport bk) { this.bk = bk; }

    private static Object ci(Map<String, Object> r, String k) { return SaleBkSupport.ci(r, k); }

    private static Map<String, Object> m(Object... kv) { return SaleBkSupport.p(kv); }

    /** CostCenterFill only runs when UserAccount.AppId == 5; elsewhere CmbCostCenter is hidden and its Value converts to 0. */
    private int costCenter(int requested) { return bk.appId() == 5 ? Math.max(requested, 0) : 0; }

    public Map<String, Object> init() {
        UserAccount u = bk.user();
        Map<String, Object> d = new LinkedHashMap<>(bk.basics());
        int appId = bk.appId();
        d.put("appId", appId);
        List<Map<String, Object>> cc = new ArrayList<>();
        if (appId == 5) {
            Map<String, Object> p = m("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
            if (u.getId() != null && u.getId() > 0) p.put("UserId", u.getId());
            for (Map<String, Object> r : bk.steel().table("[dbo].[USP_UsersWithCostCenter_AllocatedData]", p).rows)
                cc.add(m("Id", ci(r, "CostCenterId"), "name", ci(r, "CostCenterName")));
        }
        d.put("costCenters", cc);
        return d;
    }

    /** btnShow_Click: the cards. */
    public List<Map<String, Object>> cards(int costCenterId) {
        UserAccount u = bk.user();
        Map<String, Object> p = m("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "UserId", u.getId());
        Integer sc = u.getSupplierCustomerId();
        SaleBkSupport.nz(p, "SupplierCustomerId", sc != null && sc > 0 ? sc : 0);
        SaleBkSupport.nz(p, "AppId", bk.appId());
        SaleBkSupport.nz(p, "CostCenterId", costCenter(costCenterId));
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : bk.steel().table("usp_PreBookingOrder_Dashboard", p).rows)
            out.add(m("caption", SaleBkSupport.s(ci(r, "Activity")), "value", SaleBkSupport.dbl(ci(r, "Total"))));
        return out;
    }

    /** DateTimePicker.Value as the BLL receives it: the picked day with the time of day the picker holds (the page sends the time it was opened at). */
    private static Timestamp stamp(String day, String time) {
        if (day == null || day.trim().length() < 10) return null;
        LocalDate dt = LocalDate.parse(day.trim().substring(0, 10));
        LocalTime t = LocalTime.MIDNIGHT;
        if (time != null && time.length() >= 8) {
            try { t = LocalTime.parse(time.substring(0, 8)); } catch (RuntimeException ignored) { /* midnight */ }
        }
        return Timestamp.valueOf(dt.atTime(t));
    }

    private Map<String, Object> reportParams(Map<String, String> q, boolean withPaging) {
        UserAccount u = bk.user();
        Map<String, Object> p = m("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "UserId", u.getId());
        Timestamp from = stamp(q.get("fromDate"), q.get("fromTime")), to = stamp(q.get("toDate"), q.get("toTime"));
        if (from != null) p.put("FromDate", from);
        if (to != null) p.put("ToDate", to);
        SaleBkSupport.nz(p, "AppId", bk.appId());
        SaleBkSupport.nz(p, "CostCenterId", costCenter(SaleBkSupport.i(q.get("costCenterId"))));
        return p;
    }

    private static List<Map<String, Object>> set(List<List<Map<String, Object>>> ds, int i) { return i < ds.size() ? ds.get(i) : new ArrayList<>(); }

    private static Map<String, Object> row(Object... kv) { return SaleBkSupport.p(kv); }

    /**
     * GetBookingDemandData (booking = true) / GetPendingOrdersData: the six grids as the DataTables the form builds.
     * Booking:  CustomerItem {ItemId, ItemName, SuppCustId, CustomerName, Qty, Weight, Amount} from Tables[4]; ItemCustomer is the same rows (Tables[4]);
     * Pending:  CustomerItem / ItemCustomer {ItemId, SuppCustId, ItemName, CustomerName, ...} from Tables[4] and its copy; PackItem is a copy of Tables[3].
     */
    public Map<String, Object> detail(boolean booking, Map<String, String> q) {
        Map<String, Object> p = reportParams(q, false);
        List<List<Map<String, Object>>> ds = bk.tables(booking ? "usp_PreBookingOrderDashboard_Detail" : "usp_OutstandingOrders_PreDashboard", p);
        Map<String, Object> out = new LinkedHashMap<>();
        if (ds.isEmpty()) { out.put("empty", true); return out; }          // ds.Tables.Count <= 0 -> return
        List<Map<String, Object>> head = set(ds, 0);
        if (!head.isEmpty()) {
            Map<String, Object> h = head.get(0);
            out.put("header", row("customers", SaleBkSupport.i(ci(h, "TotalCustomers")), "qty", SaleBkSupport.dbl(ci(h, "Qty")),
                    "weight", SaleBkSupport.dbl(ci(h, "Weight")), "amount", SaleBkSupport.dbl(ci(h, "Amount"))));
        }
        List<Map<String, Object>> item = new ArrayList<>(), customer = new ArrayList<>(), itemPack = new ArrayList<>(), packItem = new ArrayList<>(),
                customerItem = new ArrayList<>(), itemCustomer = new ArrayList<>();
        for (Map<String, Object> r : set(ds, 1))
            item.add(row("Id", ci(r, "OrderItemId"), "ItemName", ci(r, "ItemName"), "Qty", ci(r, "Qty"), "Weight", ci(r, "Weight"), "Amount", ci(r, "Amount")));
        for (Map<String, Object> r : set(ds, 2))
            customer.add(row("Id", ci(r, "OrderSupCustId"), "CustomerName", ci(r, "CustomerName"), "Qty", ci(r, "Qty"), "Weight", ci(r, "Weight"), "Amount", ci(r, "Amount")));
        for (Map<String, Object> r : set(ds, 3)) {
            itemPack.add(row("ItemId", ci(r, "OrderItemId"), "ItemName", ci(r, "ItemName"), "UomId", ci(r, "OrderItemUOMId"), "PackUom", ci(r, "PackUom"),
                    "Qty", ci(r, "Qty"), "Weight", ci(r, "Weight"), "Amount", ci(r, "Amount")));
            packItem.add(row("ItemId", ci(r, "OrderItemId"), "ItemName", ci(r, "ItemName"), "UomId", ci(r, "OrderItemUOMId"), "PackUom", ci(r, "PackUom"),
                    "Qty", ci(r, "Qty"), "Weight", ci(r, "Weight"), "Amount", ci(r, "Amount")));
        }
        for (Map<String, Object> r : set(ds, 4)) {
            Map<String, Object> o = booking
                    ? row("ItemId", ci(r, "OrderItemId"), "ItemName", ci(r, "ItemName"), "SuppCustId", ci(r, "OrderSupCustId"), "CustomerName", ci(r, "CustomerName"),
                          "Qty", ci(r, "Qty"), "Weight", ci(r, "Weight"), "Amount", ci(r, "Amount"))
                    : row("ItemId", ci(r, "OrderItemId"), "SuppCustId", ci(r, "OrderSupCustId"), "ItemName", ci(r, "ItemName"), "CustomerName", ci(r, "CustomerName"),
                          "Qty", ci(r, "Qty"), "Weight", ci(r, "Weight"), "Amount", ci(r, "Amount"));
            customerItem.add(o);
            itemCustomer.add(new LinkedHashMap<>(o));
        }
        out.put("item", item); out.put("customer", customer); out.put("itemCustomer", itemCustomer);
        out.put("customerItem", customerItem); out.put("itemPack", itemPack); out.put("packItem", packItem);
        return out;
    }

    /** GetDeliveryInTransitData: the transit grid rows ({} columns as the DataTable). No rows -> ClearStructure (empty list). */
    public List<Map<String, Object>> transit(Map<String, String> q) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : bk.steel().table("USP_getGdnDataForMobileApp", reportParams(q, true)).rows)
            out.add(row("Id", ci(r, "Id"), "SupplierCustomerId", ci(r, "SupplierCustomerId"), "DocDate", ci(r, "DocDate"), "OutDatetime", ci(r, "OutTime"),
                    "GpNo", ci(r, "GpNo"), "DriverName", ci(r, "DriverName"), "DriverCell", ci(r, "DriverCellNo"), "CarriageAmount", ci(r, "CarriageAmount"),
                    "VehicleNo", ci(r, "VehicleNo"), "Qty", ci(r, "ItemQty"), "Weight", ci(r, "NetWeight"), "GdnStatus", ci(r, "GdnStatus"), "CustomerRemarks", ""));
        return out;
    }

    /** GetSaleBillsData. */
    public List<Map<String, Object>> bills(Map<String, String> q) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : bk.steel().table("USP_getSaleDataForMobileApp", reportParams(q, true)).rows)
            out.add(row("Id", ci(r, "Id"), "DocumentTypeId", ci(r, "DocumentTypeId"), "BillNo", ci(r, "DocNo"), "BillDate", ci(r, "DocDate"), "DueDays", ci(r, "DueDays"),
                    "DueDate", ci(r, "DueDate"), "Qty", ci(r, "ItemQty"), "Weight", ci(r, "ItemWeight"), "BillAmount", ci(r, "BillAmount"),
                    "ApprovedStatus", ci(r, "ApprovedStatus"), "CustomerRemarks", ""));
        return out;
    }

    /** Confirm / ConfirmBill: "Remarks Required!" when empty, then USP_GdnStatusUpdate / USP_InvoiceStatusUpdate (@Id, @CustomerRemarks). */
    public String confirm(boolean bill, int id, String remarks) {
        if (remarks == null || remarks.isEmpty()) throw new IllegalArgumentException("Remarks Required!");
        bk.steel().table(bill ? "USP_InvoiceStatusUpdate" : "USP_GdnStatusUpdate", m("Id", id, "CustomerRemarks", remarks));
        return bill ? "Bill Confirmed Successfully." : "Transit Confirmed Successfully.";
    }
}
