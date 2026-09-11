package com.mst.services;

import com.mst.repositories.GhallaMandiGrnLoaderRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.*;

@Service
public class GhallaMandiGrnLoaderService {

    @Autowired
    private GhallaMandiGrnLoaderRepository repository;

    public Map<String, Object> getDropdowns(int orgId, int compId) {
        Map<String, Object> res = new HashMap<>();
        res.put("loadingContractors", repository.getLoadingContractors(orgId, compId));
        res.put("ghallaMandis", repository.getGhallaMandiList(orgId, compId));
        return res;
    }

    public List<Map<String, Object>> getPendingGhallaMandiGrns(int orgId, int compId, int docTypeId, int yearId,
                                                               String fromDate, String toDate,
                                                               Integer supplierCustomerId, Integer ghallaMandiId) {
        return repository.getPendingGhallaMandiGrns(orgId, compId, docTypeId, yearId, fromDate, toDate, supplierCustomerId, ghallaMandiId);
    }

    /**
     * Validates selected Ghalla Mandi GRN rows matching btnLoadOnInvoice_Click_1 of frmLoadGrnForGhallaMandi.cs:
     * - Must belong to same GhallaMandiId
     * - Must belong to same LoadingContractorId (SupplierCustomerId)
     */
    public Map<String, Object> validateAndSelectGrns(List<Map<String, Object>> selectedRows) {
        Map<String, Object> result = new HashMap<>();

        if (selectedRows == null || selectedRows.isEmpty()) {
            result.put("success", false);
            result.put("message", "Check the row first");
            return result;
        }

        int firstGhallaMandiId = 0;
        int firstLoadingContractorId = 0;
        List<Map<String, Object>> validatedGrns = new ArrayList<>();

        for (Map<String, Object> r : selectedRows) {
            int ghallaMandiId = r.get("GhallaMandiId") != null ? Integer.parseInt(r.get("GhallaMandiId").toString()) : 0;
            int loadingContractorId = r.get("SupplierCustomerId") != null ? Integer.parseInt(r.get("SupplierCustomerId").toString()) : 0;
            int id = r.get("Id") != null ? Integer.parseInt(r.get("Id").toString()) : 0;

            if (ghallaMandiId != 0) {
                if (firstGhallaMandiId == 0) firstGhallaMandiId = ghallaMandiId;
                if (firstGhallaMandiId != ghallaMandiId) {
                    result.put("success", false);
                    result.put("message", "Sorry the Ghalla_Mandi should be of the same");
                    return result;
                }

                if (firstLoadingContractorId == 0) firstLoadingContractorId = loadingContractorId;
                if (firstLoadingContractorId != loadingContractorId) {
                    result.put("success", false);
                    result.put("message", "Sorry the LoadingContractor should be of the same");
                    return result;
                }

                Map<String, Object> item = new HashMap<>();
                item.put("Id", id);
                item.put("GrnType", 2);
                validatedGrns.add(item);
            }
        }

        result.put("success", true);
        result.put("grns", validatedGrns);
        result.put("ghallaMandiId", firstGhallaMandiId);
        result.put("loadingContractorId", firstLoadingContractorId);
        return result;
    }
}
