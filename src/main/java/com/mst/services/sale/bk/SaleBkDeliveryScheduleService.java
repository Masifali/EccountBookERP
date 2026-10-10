package com.mst.services.sale.bk;

import com.mst.models.UserAccount;
import org.springframework.stereotype.Service;

import java.sql.Date;
import java.util.*;

/**
 * Screen 865 DeliveryScheduleCustomer = Architecture.WinApp.Sale.frmDeliveryScheduleCustomer ("Pending Delivery Schedule Of Customer").
 *   CombosFill :117                 DeliveryScheduleCustomer.GetDataForDropDownFromDeliveryScheduleCustomer (dbo.USP_GetDataForDropDownFromDeliveryScheduleCustomer
 *                                   @OrganizationId, @CompanyId; DocumentTypeIds / Activity are null and not sent), Activity Customer -> CmbSupplier, Item -> CmbItem
 *   PendingOrderLoad :170           DeliveryScheduleCustomer.GetPendingDeliveryScheduleCustomerDetailForDeliveryOrder
 *                                   (dbo.USP_GetPendingDeliveryScheduleCustomerDetailForDeliveryOrder @OrganizationId, @CompanyId, @FromDate, @ToDate,
 *                                   @SupplierCustomerId when != 0, @ItemId when != 0; @Ids / @OrderDetailIds only when the caller form supplies them, never from this screen)
 */
@Service
public class SaleBkDeliveryScheduleService {
    private final SaleBkSupport bk;

    public SaleBkDeliveryScheduleService(SaleBkSupport bk) { this.bk = bk; }

    public Map<String, Object> lookups() {
        UserAccount u = bk.user();
        List<Map<String, Object>> all = bk.steel().table("USP_GetDataForDropDownFromDeliveryScheduleCustomer",
                SaleBkSupport.p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId())).rows;
        Map<String, Object> d = bk.basics();
        d.put("customers", SaleBkSupport.activity(all, "Customer", "Id", "ReferenceName"));
        d.put("items", SaleBkSupport.activity(all, "Item", "Id", "ReferenceName"));
        return d;
    }

    /** PendingOrderLoad: the grid table is rebuilt from the procedure rows with the 17 columns the form declares. */
    public List<Map<String, Object>> rows(Map<String, String> q) {
        UserAccount u = bk.user();
        Map<String, Object> p = SaleBkSupport.p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        Date f = SaleBkSupport.day(q.get("fromDate")), t = SaleBkSupport.day(q.get("toDate"));
        if (f != null) p.put("FromDate", f);
        if (t != null) p.put("ToDate", t);
        SaleBkSupport.nz(p, "SupplierCustomerId", SaleBkSupport.i(q.get("supplierCustomerId")));
        SaleBkSupport.nz(p, "ItemId", SaleBkSupport.i(q.get("itemId")));
        List<Map<String, Object>> src = bk.steel().table("USP_GetPendingDeliveryScheduleCustomerDetailForDeliveryOrder", p).rows;
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : src) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", SaleBkSupport.ci(r, "Id"));
            o.put("deliveryScheduleDetailId", SaleBkSupport.ci(r, "deliveryScheduleDetailId"));
            o.put("DocDate", SaleBkSupport.ci(r, "DocDate"));
            o.put("DocNo", SaleBkSupport.ci(r, "DocNo"));
            o.put("SaleOrderId", SaleBkSupport.ci(r, "SaleOrderId"));
            o.put("SaleOrderDetailId", SaleBkSupport.ci(r, "SaleOrderDetailId"));
            o.put("SupplierCustomerId", SaleBkSupport.ci(r, "SupplierCustomerId"));
            o.put("CustomerName", SaleBkSupport.ci(r, "CustomerName"));
            o.put("ItemId", SaleBkSupport.ci(r, "ItemId"));
            o.put("ItemName", SaleBkSupport.ci(r, "ItemName"));
            o.put("PackUom", SaleBkSupport.ci(r, "PackUom"));
            o.put("JobLot", SaleBkSupport.ci(r, "JobLotDescription"));
            o.put("PackingType", SaleBkSupport.ci(r, "PackTypeDesc"));
            o.put("ItemQty", SaleBkSupport.ci(r, "ItemQty"));
            o.put("NetWeight", SaleBkSupport.ci(r, "NetWeight"));
            o.put("ItemRate", SaleBkSupport.ci(r, "ItemRate"));
            o.put("RemarksHeader", SaleBkSupport.ci(r, "RemarksHeader"));
            out.add(o);
        }
        return out;
    }
}
