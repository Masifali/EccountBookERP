package com.mst.services.sale.bk;

import com.mst.models.UserAccount;
import org.springframework.stereotype.Service;

import java.sql.Date;
import java.util.*;

/**
 * Screen 765 BookingReport = Architecture.WinApp.PreBookingAndDelivery.PreBookingOrderRegister ("PreBooking Order Register").
 *   frmStockWithSupplierHistory_Load :112   Datetypefill, CostCenterFill, (AppId 5: first cost center, combo disabled), ComboFill(cost center), CmbDateType Rows[1] ("This Week")
 *   CostCenterFill :215                     Projects.GetSubCostCenters(Org, Company, UserId, AppId, 0) = usp_getCostCenters
 *   ComboFill :250                          PreBookingOrder.GetDataForDropDownFromPreBookingOrder = USP_GetDataForDropDownFromPreBookingOrder
 *                                           (@OrganizationId, @CompanyId, @AppId, @UserId, @DocumentTypeIds '129', @CostCenterId when != 0); Activity Item / Customer
 *   GridFill :378                           PreBookingOrder.PreBookingOrderSlipAndRegister = USP_PreBookingOrderSlipAndRegister; the 25 column dtHistory
 *   btnPrint_Click :555                     "274_1-PreBookinRegister.rpt" over the last Show's table
 *   grdHistory_ColumnButtonClick :520       CommonServices.PreBookingSlip(Id) = usp_PreBookingOrder_Slip (Org, Company, Id) -> "274_PreBookingOrder_Slip.rpt"
 */
@Service
public class SaleBkBookingRegisterService {
    private static final String P_REG = "USP_PreBookingOrderSlipAndRegister";
    private final SaleBkSupport bk;

    public SaleBkBookingRegisterService(SaleBkSupport bk) { this.bk = bk; }

    private static Object ci(Map<String, Object> r, String k) { return SaleBkSupport.ci(r, k); }

    public Map<String, Object> lookups() {
        Map<String, Object> d = bk.basics();
        d.put("appId", bk.appId());
        d.put("dateTypes", SaleBkSupport.dateTypes(false));
        List<Map<String, Object>> cc = new ArrayList<>();
        for (Map<String, Object> r : bk.subCostCenters(0)) cc.add(SaleBkSupport.p("Id", ci(r, "Id"), "name", ci(r, "CostCenterName")));
        d.put("costCenters", cc);
        return d;
    }

    /** ComboFill(CostCenterId): customers and items of the pre-booking documents (document type 129). */
    public Map<String, Object> combos(int costCenterId) {
        UserAccount u = bk.user();
        Map<String, Object> p = SaleBkSupport.p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "AppId", bk.appId(), "UserId", u.getId(),
                "DocumentTypeIds", "129");
        SaleBkSupport.nz(p, "CostCenterId", costCenterId);
        List<Map<String, Object>> all = bk.steel().table("USP_GetDataForDropDownFromPreBookingOrder", p).rows;
        Map<String, Object> d = new LinkedHashMap<>();
        d.put("customers", SaleBkSupport.activity(all, "Customer", "Id", "ReferenceName"));
        d.put("items", SaleBkSupport.activity(all, "Item", "Id", "ReferenceName"));
        return d;
    }

    /** The procedure call of GridFill; also used by the print. */
    public List<Map<String, Object>> raw(Map<String, String> q) {
        UserAccount u = bk.user();
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", u.getOrganizationId());
        p.put("CompanyId", u.getCompanyId());
        p.put("FinancialYearId", bk.sup().fy());
        p.put("AppId", bk.appId());
        p.put("UserId", u.getId());
        Date f = SaleBkSupport.day(q.get("fromDate")), t = SaleBkSupport.day(q.get("toDate"));
        if (f != null) p.put("FromDate", f);
        if (t != null) p.put("ToDate", t);
        SaleBkSupport.nz(p, "SupplierCustomerId", SaleBkSupport.i(q.get("supplierCustomerId")));
        SaleBkSupport.nz(p, "ItemId", SaleBkSupport.i(q.get("itemId")));
        p.put("DocumentTypeIds", "129");
        p.put("IsApproved", Boolean.FALSE);            // ApprovedFilter is never set on this form (null != "All"), so the BLL sends IsApproved = false
        int action = "1".equals(q.get("referred")) ? 1 : "2".equals(q.get("referred")) ? 2 : 0;
        SaleBkSupport.nz(p, "ActionId", action);
        SaleBkSupport.nz(p, "CostCenterId", SaleBkSupport.i(q.get("costCenterId")));
        return bk.steel().table(P_REG, p).rows;
    }

    private static final String[][] MAP = {
        {"Id", "Id"}, {"DetailId", "PoDId"}, {"DocNo", "DocNo"}, {"DocDate", "DocDate"}, {"CustomerName", "CustomerName"}, {"ItemId", "OrderItemId"},
        {"ItemName", "ItemName"}, {"PackUom", "PackUom"}, {"ItemQty", "OrderItemQty"}, {"DispatchQty", "DispatchQty"}, {"BalQty", "BalQty"}, {"Weight", "NetWeight"},
        {"DispatchWeight", "DispatchWeight"}, {"BalWeight", "BalWeight"}, {"ItemRate", "OrderItemRate"}, {"RateUom", "RateUom"}, {"ItemAmount", "ItemAmount"},
        {"DispatchAmount", "Dispatchamount"}, {"BalAmount", "BalAmount"}, {"EntryUser", "UserNameEusr"}, {"EntryDate", "EntryDate"}, {"ModifyUser", "UserNameMusr"},
        {"ModifyDate", "ModifyDate"}, {"DetailRemarks", "DetailRemarks"}, {"RemarksHeader", "RemarksHeader"}};

    /** GridFill: the rows copied into the 25 column dtHistory. */
    public List<Map<String, Object>> show(Map<String, String> q) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : raw(q)) {
            Map<String, Object> o = new LinkedHashMap<>();
            for (String[] m : MAP) o.put(m[0], ci(r, m[1]));
            out.add(o);
        }
        return out;
    }

    /** CommonServices.PreBookingSlip -> PreBookingOrder_Slip (usp_PreBookingOrder_Slip @OrganizationId, @CompanyId, @Id). */
    public List<Map<String, Object>> slip(int id) {
        UserAccount u = bk.user();
        return bk.steel().table("usp_PreBookingOrder_Slip", SaleBkSupport.p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Id", id)).rows;
    }
}
