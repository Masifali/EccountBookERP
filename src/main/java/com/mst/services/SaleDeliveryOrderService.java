package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.dto.SaleDeliveryOrderRequest;
import com.mst.repositories.SaleDeliveryOrderRepository;
import com.mst.security.CurrentUserContext;
import com.mst.repositories.DeliveryOrderTransferRepository;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

@Service
public class SaleDeliveryOrderService {
    private final SaleDeliveryOrderRepository repository;
    private final CurrentUserContext context;
    private final DeliveryOrderTransferRepository lookups;

    public SaleDeliveryOrderService(SaleDeliveryOrderRepository repository, CurrentUserContext context, DeliveryOrderTransferRepository lookups) {
        this.lookups = lookups;
        this.repository = repository;
        this.context = context;
    }

    public Map<String,Object> initial() {
        UserAccount user = context.requireAccountingUser();
        int year = context.currentFinancialYearId();
        Map<String,Object> m = new LinkedHashMap<>();
        m.put("documentTypeId", SaleDeliveryOrderRepository.DOCUMENT_TYPE_ID);
        m.put("nextDocNo", repository.nextCode(user, year));
        m.put("pendingOrders", repository.pendingOrders(user, year));
        m.put("branches", repository.branches(user));
        m.put("vehicleTypes", repository.vehicleTypes(user));
        m.put("saleTypes", List.of(Map.of("Id",1,"Name","Weigh Bridge Based Billing"), Map.of("Id",2,"Name","Pack Size Based Billing")));
        m.put("deliveryOrderTypes", List.of(Map.of("Id",1,"Name","Local"), Map.of("Id",2,"Name","StockTransfer")));
        /* DeliveryOrder.cs:848 SupplierDtFillFromGlobal: with ERP feature 4 only PartyTypeId 2 rows. */
        boolean feature4 = lookups.erpFeature(user, 4);
        int showBoth = truthy(repository.config(user, "ShowBothVendorAndCustomerOnSalesPurchase")) ? 1 : 0;
        m.put("customers", lookups.suppliers(user, showBoth).stream().filter(r -> !feature4 || Integer.valueOf(2).equals(r.get("PartyTypeId"))).toList());
        m.put("warehouses", lookups.warehousesWithBranches(user, user.getBranchesId()));
        m.put("items", lookups.allItems(user).stream().filter(r -> !Integer.valueOf(14).equals(r.get("ItemTypeOfTypeId")) && !Integer.valueOf(17).equals(r.get("ItemTypeOfTypeId"))).toList());
        m.put("uoms", lookups.uomSchedule(user));
        m.put("crops", lookups.cropYears(user));
        m.put("jobLots", lookups.jobLots(user));
        m.put("packingTypes", lookups.packingTypes());
        m.put("otherItems", lookups.otherItems(user));
        m.put("referenceParties", lookups.referenceParties(user));
        /* GetConfigurationsFromGlobal (:705) and GetConfigurationsFromGlobalAndBindValuesInColumns (:740) */
        Map<String,Object> cfg = new LinkedHashMap<>();
        cfg.put("stockReservedPerParty", truthy(repository.config(user, "IsStockReservedPerParty")));
        cfg.put("itemSearchByCode", truthy(repository.config(user, "ItemSearchByCode")));
        cfg.put("warnPackingWeight", truthy(repository.config(user, "WarningMessageOnDOForPackingWeight")));
        cfg.put("jobLot", repository.config(user, "Job/Lot"));
        cfg.put("cropYear", repository.config(user, "Default Crop Year"));
        cfg.put("packingType", repository.config(user, "Paking Type"));
        cfg.put("warehouse", repository.config(user, "Warehouse"));
        String days = repository.config(user, "DefaultDaysToLessFromHistoryFromDate");
        cfg.put("historyDays", days.matches("\\d+") && Integer.parseInt(days) > 0 ? Integer.parseInt(days) : 3);
        m.put("config", cfg);
        return m;

    }

    private static boolean truthy(String v) { return "true".equalsIgnoreCase(v) || "1".equals(v); }
    public List<Map<String,Object>> orderLines(int orderId, int saleTypeId) { return repository.orderLines(context.requireAccountingUser(), orderId, saleTypeId); }
    public List<Map<String,Object>> ordersOfCustomer(int customerId, int recId) { return repository.ordersOfCustomer(context.requireAccountingUser(), customerId, recId); }
    public List<Map<String,Object>> orderItems(int orderId) { return repository.orderItems(orderId); }
    public double orderBalanceWeight(int orderId, int detailId, int itemId) { return repository.orderBalanceWeight(context.requireAccountingUser(), orderId, detailId, itemId); }
    public List<Map<String,Object>> orderExpenses(String ids) { return repository.orderExpenses(ids); }

    public List<Map<String,Object>> history() {
        UserAccount user=context.requireAccountingUser();
        return repository.history(user,context.currentFinancialYearId(),repository.canViewAllRecords(user,context.currentRoleName()));
    }
    public Map<String,Object> record(int id) { return repository.record(context.requireAccountingUser(), id); }

    @Transactional
    public Map<String,Object> save(SaleDeliveryOrderRequest request) {
        return repository.save(context.requireAccountingUser(), context.currentFinancialYearId(), request);
    }

    @Transactional
    public void delete(int id) { repository.delete(context.requireAccountingUser(), id); }
}
