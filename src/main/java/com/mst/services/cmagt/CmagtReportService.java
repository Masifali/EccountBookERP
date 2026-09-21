package com.mst.services.cmagt;

import com.mst.repositories.cmagt.CmagtReportRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.List;
import java.util.Map;

@Service
public class CmagtReportService {

    @Autowired
    private CmagtReportRepository repository;

    public List<Map<String, Object>> getSaleOrderReport(Integer companyId, Integer organizationId, String fromDate, String toDate, Integer buyerId, Integer agentId) {
        return repository.getSaleOrderReport(companyId, organizationId, fromDate, toDate, buyerId, agentId);
    }

    public List<Map<String, Object>> getPurchaseOrderReport(Integer companyId, Integer organizationId, String fromDate, String toDate, Integer supplierId, Integer agentId) {
        return repository.getPurchaseOrderReport(companyId, organizationId, fromDate, toDate, supplierId, agentId);
    }

    public List<Map<String, Object>> getGrnSupplierLoadingReport(Integer companyId, Integer organizationId, String fromDate, String toDate, Integer supplierId) {
        return repository.getGrnSupplierLoadingReport(companyId, organizationId, fromDate, toDate, supplierId);
    }

    public List<Map<String, Object>> getGdnBuyerDispatchReport(Integer companyId, Integer organizationId, String fromDate, String toDate, Integer buyerId) {
        return repository.getGdnBuyerDispatchReport(companyId, organizationId, fromDate, toDate, buyerId);
    }

    public List<Map<String, Object>> getAgentTradeBillRegister(Integer companyId, Integer organizationId, String fromDate, String toDate, Integer agentId) {
        return repository.getAgentTradeBillRegister(companyId, organizationId, fromDate, toDate, agentId);
    }
}
