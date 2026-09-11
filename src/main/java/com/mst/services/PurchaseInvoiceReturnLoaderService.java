package com.mst.services;

import com.mst.repositories.PurchaseInvoiceReturnLoaderRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.*;

@Service
public class PurchaseInvoiceReturnLoaderService {

    @Autowired
    private PurchaseInvoiceReturnLoaderRepository repository;

    public List<Map<String, Object>> getUserBranches(int orgId, int compId, int userId) {
        return repository.getUserBranches(orgId, compId, userId);
    }

    public List<Map<String, Object>> getPendingInvoicesForReturn(int orgId, int compId, int yearId,
                                                                 String fromDate, String toDate, String branchIds) {
        return repository.getPendingInvoicesForReturn(orgId, compId, yearId, fromDate, toDate, branchIds);
    }

    /**
     * Validates selected rows matching BtnLoad of frmLoadPurchaseInvoiceForReturn.cs:
     * - Same SupplierCustomerId
     * - Same DocumentTypeId
     * - Same BranchId (if branchWise)
     */
    public Map<String, Object> validateAndSelectInvoices(List<Map<String, Object>> selectedRows, boolean branchImplemented) {
        Map<String, Object> result = new HashMap<>();

        if (selectedRows == null || selectedRows.isEmpty()) {
            result.put("success", false);
            result.put("message", "Check the row first");
            return result;
        }

        int firstSupplierId = 0;
        int firstDocTypeId = 0;
        int firstBranchId = 0;

        for (Map<String, Object> r : selectedRows) {
            int supplierCustomer = r.get("SupplierCustomerId") != null ? Integer.parseInt(r.get("SupplierCustomerId").toString()) : 0;
            int docTypeId = r.get("DocumentTypeId") != null ? Integer.parseInt(r.get("DocumentTypeId").toString()) : 0;
            int branchId = r.get("BranchId") != null ? Integer.parseInt(r.get("BranchId").toString()) : 0;

            if (supplierCustomer != 0) {
                if (firstSupplierId == 0) firstSupplierId = supplierCustomer;
                if (firstSupplierId != supplierCustomer) {
                    result.put("success", false);
                    result.put("message", "Sorry!. The Selected Invoices are not of same Supplier");
                    return result;
                }
            }

            if (docTypeId != 0) {
                if (firstDocTypeId == 0) firstDocTypeId = docTypeId;
                if (firstDocTypeId != docTypeId) {
                    result.put("success", false);
                    result.put("message", "Sorry!. The Selected Invoices are not of same DocumentType");
                    return result;
                }
            }

            if (branchImplemented && branchId != 0) {
                if (firstBranchId == 0) firstBranchId = branchId;
                if (firstBranchId != branchId) {
                    result.put("success", false);
                    result.put("message", "Sorry!. The Selected Invoices are not of same Branch");
                    return result;
                }
            }
        }

        result.put("success", true);
        result.put("selectedInvoices", selectedRows);
        result.put("supplierCustomerId", firstSupplierId);
        result.put("documentTypeId", firstDocTypeId);
        result.put("branchId", firstBranchId);
        return result;
    }
}
