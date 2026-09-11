package com.mst.services;

import com.mst.repositories.PaddyGrnLoaderRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.*;

@Service
public class PaddyGrnLoaderService {

    @Autowired
    private PaddyGrnLoaderRepository repository;

    public List<Map<String, Object>> getSuppliers(int orgId, int compId) {
        return repository.getSuppliers(orgId, compId);
    }

    public List<Map<String, Object>> getOrdersBySupplier(int orgId, int compId, int supplierId) {
        return repository.getOrdersBySupplierId(orgId, compId, supplierId);
    }

    public List<Map<String, Object>> getPendingPaddyGrns(int orgId, int compId, int docTypeId, int yearId,
                                                         String fromDate, String toDate, Integer orderId) {
        return repository.getPendingPaddyGrns(orgId, compId, docTypeId, yearId, fromDate, toDate, orderId);
    }

    /**
     * Validates selected rows matching btnLoadOnInvoice_Click_1 of frmLoadGRNForPaddyPurchase.cs:
     * - Checks same SupplierCustomerId
     * - Checks same RefDocumentTypeId
     * - Attaches OrderId if active
     */
    public Map<String, Object> validateAndSelectGrns(List<Map<String, Object>> selectedRows, Integer selectedOrderId, int documentTypeId) {
        Map<String, Object> result = new HashMap<>();

        if (selectedRows == null || selectedRows.isEmpty()) {
            result.put("success", false);
            result.put("message", "Check the row first");
            return result;
        }

        int firstSupplierId = 0;
        int firstRefDocTypeId = 0;
        int firstDocId = 1;

        List<Map<String, Object>> validatedGrns = new ArrayList<>();

        for (Map<String, Object> r : selectedRows) {
            int supplierCustomer = r.get("SupplierCustomerId") != null ? Integer.parseInt(r.get("SupplierCustomerId").toString()) : 0;
            int refDocTypeId = r.get("RefDocumentTypeId") != null ? Integer.parseInt(r.get("RefDocumentTypeId").toString()) : 0;
            int docId = r.get("Id") != null ? Integer.parseInt(r.get("Id").toString()) : 0;

            if (supplierCustomer != 0) {
                if (firstSupplierId == 0) firstSupplierId = supplierCustomer;
                if (firstRefDocTypeId == 0) firstRefDocTypeId = refDocTypeId;
                if (firstDocId == 0) firstDocId = docId;

                if (documentTypeId == 166 && docId != firstDocId) {
                    result.put("success", false);
                    result.put("message", "Only single GRN select allowed");
                    return result;
                }

                if (firstSupplierId != supplierCustomer) {
                    result.put("success", false);
                    result.put("message", "Sorry the GRNs should be of the same Supplier");
                    return result;
                }

                if (firstRefDocTypeId != refDocTypeId) {
                    result.put("success", false);
                    result.put("message", "Sorry the GRNs should be of the same PurchaseAgainst type");
                    return result;
                }

                Map<String, Object> item = new HashMap<>();
                item.put("Id", docId);
                item.put("GrnType", 3);
                item.put("SupplierId", supplierCustomer);
                item.put("OrderId", selectedOrderId != null ? selectedOrderId : 0);
                validatedGrns.add(item);
            }
        }

        result.put("success", true);
        result.put("grns", validatedGrns);
        result.put("supplierId", firstSupplierId);
        result.put("orderId", selectedOrderId != null ? selectedOrderId : 0);
        return result;
    }
}
