package com.mst.services;

import com.mst.repositories.GrnLoaderRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.*;

@Service
public class GrnLoaderService {

    @Autowired
    private GrnLoaderRepository repository;

    public List<Map<String, Object>> getPendingGrns(int orgId, int compId, int docTypeId, int yearId,
                                                    String fromDate, String toDate, String branchIds,
                                                    Integer supplierCustomerId, Integer orderId) {
        return repository.getPendingGrns(orgId, compId, docTypeId, yearId, fromDate, toDate, branchIds, supplierCustomerId, orderId);
    }

    public List<Map<String, Object>> getPendingMarketGrns(int orgId, int compId, int docTypeId,
                                                          String fromDate, String toDate,
                                                          Integer supplierCustomerId) {
        return repository.getPendingMarketGrns(orgId, compId, docTypeId, fromDate, toDate, supplierCustomerId);
    }

    public List<Map<String, Object>> getGrnDetails(int grnId) {
        return repository.getGrnDetails(grnId);
    }

    public List<Map<String, Object>> getUserBranches(int orgId, int compId, int userId, int docTypeId) {
        return repository.getUserBranches(orgId, compId, userId, docTypeId);
    }

    /**
     * Validates selected GRN rows as done in btnLoadOnInvoice_Click_1 of frmLoadGRN.cs:
     * - Must belong to same Supplier
     * - Must belong to same PurchaseAgainst (RefDocumentTypeId)
     * - Must belong to same DeliveryTerm
     * - Must belong to same PurchaseOrder
     * - Checks GrnStatusId == 1 approval requirement
     */
    public Map<String, Object> validateAndSelectGrns(List<Map<String, Object>> selectedRows, int documentTypeId, boolean acceptAccessWtHold) {
        Map<String, Object> result = new HashMap<>();

        if (selectedRows == null || selectedRows.isEmpty()) {
            result.put("success", false);
            result.put("message", "Check the row first");
            return result;
        }

        int firstSupplierId = 0;
        int firstRefDocTypeId = 0;
        int firstOrderId = 1;
        int firstDocId = 0;
        String firstDeliveryTerm = "";

        List<Map<String, Object>> validatedGrns = new ArrayList<>();

        for (Map<String, Object> r : selectedRows) {
            int supplierCustomer = r.get("SupplierCustomerId") != null ? Integer.parseInt(r.get("SupplierCustomerId").toString()) : 0;
            int orderId = r.get("PurchaseOrderId") != null ? Integer.parseInt(r.get("PurchaseOrderId").toString()) : 0;
            int refDocTypeId = r.get("RefDocumentTypeId") != null ? Integer.parseInt(r.get("RefDocumentTypeId").toString()) : 0;
            int docId = r.get("Id") != null ? Integer.parseInt(r.get("Id").toString()) : 0;
            int grnStatusId = r.get("GrnStatusId") != null ? Integer.parseInt(r.get("GrnStatusId").toString()) : 0;
            String dTerm = r.get("DeliveryTerm") != null ? r.get("DeliveryTerm").toString() : "";
            String purchaseAgainst = r.get("PurchaseAgainst") != null ? r.get("PurchaseAgainst").toString() : "";

            if (grnStatusId == 1) {
                String msg = "Approval is required because Bill weight exceeds the stock weight OR Empty Bags Deduction equal to zero";
                if (acceptAccessWtHold) {
                    msg = "Approval is required because \nBill weight exceeds Balance Order Weight \n                           OR\nBill weight exceeds stock Weight \n                           OR\nEmpty Bags Deduction equal to zero";
                }
                result.put("success", false);
                result.put("message", msg);
                return result;
            }

            if (supplierCustomer != 0) {
                if (firstSupplierId == 0) firstSupplierId = supplierCustomer;
                if (firstRefDocTypeId == 0) firstRefDocTypeId = refDocTypeId;
                if (firstOrderId == 1) firstOrderId = orderId;
                if (firstDocId == 0) firstDocId = docId;
                if (firstDeliveryTerm.isEmpty()) firstDeliveryTerm = dTerm;

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

                if (refDocTypeId != 46 && !firstDeliveryTerm.equals(dTerm)) {
                    result.put("success", false);
                    result.put("message", "Sorry the GRNs should be of the same Delivery Term");
                    return result;
                }

                if (firstOrderId != orderId) {
                    result.put("success", false);
                    result.put("message", "Sorry the GRNs should be of the same PurchaseOrder");
                    return result;
                }

                int grnType = 1;
                if ("PurchaseOrder".equalsIgnoreCase(purchaseAgainst)) grnType = 1;
                else if ("Market Purchase".equalsIgnoreCase(purchaseAgainst)) grnType = 2;
                else if ("Gate Purchase".equalsIgnoreCase(purchaseAgainst)) grnType = 3;
                else if ("Purchase From Party Processing".equalsIgnoreCase(purchaseAgainst)) grnType = 4;

                Map<String, Object> item = new HashMap<>();
                item.put("Id", docId);
                item.put("GrnType", grnType);
                validatedGrns.add(item);
            }
        }

        result.put("success", true);
        result.put("grns", validatedGrns);
        result.put("supplierCustomerId", firstSupplierId);
        result.put("purchaseOrderId", firstOrderId);
        return result;
    }
}
