package com.mst.services.sale.steel;

import com.mst.services.sale.engr.SaleEngrSupport;
import org.springframework.stereotype.Component;

import java.util.*;

import static com.mst.services.sale.engr.SaleEngrSupport.*;

/**
 * The combo fills the Sale Steel forms share (CommonServices / Inventory BLL calls the desktop repeats on every Steel sale form).
 * Each method names the BLL call it reproduces; procedure, parameters and filters are the desktop's.
 */
@Component
public class SaleSteelLookups {
    private final SaleEngrSupport sup;

    public SaleSteelLookups(SaleEngrSupport sup) { this.sup = sup; }

    /** CommonServices.SupplierCustomerGetforComboServiceBind = SupplierCustomer.GetforComboBinding. */
    public List<Map<String, Object>> customers() {
        return sup.rows("Sp_SupplierCustomer_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "ReadByOrganizationIdCompanyIdForBinding");
    }

    /** CommonServices.getActiveWareHouse = InvWareHouse.GetActiveWareHouse. */
    public List<Map<String, Object>> warehouses() {
        return sup.rows("Sp_InvWareHouse_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "GetActiveWareHouse");
    }

    /** CommonServices.AllItemsBindForSteel = Item.AllItemsBindForSteel. */
    public List<Map<String, Object>> items() {
        return sup.rows("Sp_Item_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "AllItemsBindForSteel");
    }

    /** VehicleType.GetAll(). */
    public List<Map<String, Object>> vehicleTypes() {
        return sup.rows("Sp_VehicleType_GetAllMethod", "Activity", "ReadAll");
    }

    /** CommonServices.JobLotGetAllService = jobLot.GetAll. */
    public List<Map<String, Object>> jobLots() {
        return sup.rows("SP_JobLot_ReadMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "GetAll");
    }

    /** InvPackingType.Getall(). */
    public List<Map<String, Object>> packingTypes() {
        return sup.rows("Sp_InvPackingType_GetAllMethod", "Activity", "ReadAll");
    }

    /** ReferenceParties.GetAll(org, company). */
    public List<Map<String, Object>> referenceParties() {
        return sup.rows("Sp_ReferenceParties_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "ReadAll");
    }

    /** CommonServices.GetUomScheduleByItemId: Id, UOMCode, Equivalent, QtyEquivalent, BaseRateUom. */
    public List<Map<String, Object>> uoms(int itemId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("Sp_UOMSchedule_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "ItemId", itemId, "Activity", "ReadByItemID"))
            out.add(row("Id", r.get("Id"), "UOMCode", r.get("UOMCode"), "Equivalent", r.get("Equivalent"), "QtyEquivalent", r.get("QtyEquivalent"), "BaseRateUom", r.get("BaseRateUom")));
        return out;
    }

    /** clsGlobalVariables.stringFormatsingle: "#,##0." + (1..4 zeros of 'Default NoofDecimal Points For Amount'). */
    public int amountDecimals() {
        int n = toInt(sup.config("Default NoofDecimal Points For Amount"));
        return n >= 1 && n <= 4 ? n : 0;
    }

    /** GetConfigurationByOrgCompandConfigDescription("DefaultDaysToLessFromHistoryFromDate"). */
    public int historyDays() {
        int days = toInt(sup.config("DefaultDaysToLessFromHistoryFromDate"));
        return days > 0 ? days : 3;
    }
}
