package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.dto.SaleDeliveryOrderRequest;
import com.mst.repositories.SaleDeliveryOrderRepository;
import com.mst.security.CurrentUserContext;
import java.util.List;
import java.util.Map;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

@Service
public class SaleDeliveryOrderService {
    private final SaleDeliveryOrderRepository repository;
    private final CurrentUserContext context;

    public SaleDeliveryOrderService(SaleDeliveryOrderRepository repository, CurrentUserContext context) {
        this.repository = repository;
        this.context = context;
    }

    public Map<String,Object> initial() {
        UserAccount user = context.requireAccountingUser();
        int year = context.currentFinancialYearId();
        return Map.of(
                "documentTypeId", SaleDeliveryOrderRepository.DOCUMENT_TYPE_ID,
                "nextDocNo", repository.nextCode(user, year),
                "pendingOrders", repository.pendingOrders(user, year),
                "branches", repository.branches(user),
                "vehicleTypes", repository.vehicleTypes(user),
                "saleTypes", List.of(Map.of("Id",1,"Name","Weigh Bridge Based Billing"), Map.of("Id",2,"Name","Pack Size Based Billing"))
        );
    }

    public List<Map<String,Object>> orderLines(int orderId) { return repository.orderLines(context.requireAccountingUser(), orderId); }
    public List<Map<String,Object>> history() { return repository.history(context.requireAccountingUser(), context.currentFinancialYearId(), true); }
    public Map<String,Object> record(int id) { return repository.record(context.requireAccountingUser(), id); }

    @Transactional
    public Map<String,Object> save(SaleDeliveryOrderRequest request) {
        return repository.save(context.requireAccountingUser(), context.currentFinancialYearId(), request);
    }

    @Transactional
    public void delete(int id) { repository.delete(context.requireAccountingUser(), id); }
}
