package com.mst.services;

import com.mst.models.InwardGatePass;
import com.mst.models.InwardGatePassDetail;
import com.mst.models.InwardGatePassPurchaseBreakUp;
import com.mst.repositories.InwardGatePassRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.*;

@Service
public class InwardGatePassService {

    @Autowired
    private InwardGatePassRepository repository;

    public Map<String, Object> getDropdowns(Integer orgId, Integer compId) {
        Map<String, Object> map = new HashMap<>();
        map.put("suppliers", repository.getSuppliers(orgId, compId));
        map.put("cities", repository.getCities(orgId, compId));
        map.put("vehicleTypes", repository.getVehicleTypes(orgId, compId));
        map.put("gatePassTypes", repository.getGatePassTypes());
        map.put("items", repository.getItems(orgId, compId));
        map.put("weighBridges", repository.getWeighBridges());
        map.put("packingTypes", repository.getPackingTypes());
        return map;
    }

    public Map<String, Object> generateNextNumbers(Integer orgId, Integer compId, Integer branchId, Integer yearId, Integer docTypeId, String gatepassType) {
        Map<String, Object> res = new HashMap<>();
        Integer gpSrNo = repository.generateGpCode(orgId, compId, branchId, yearId, docTypeId);
        Integer gpTypeSrNo = repository.generateGpTypeCode(orgId, compId, branchId, yearId, gatepassType);
        res.put("gpSrNo", gpSrNo);
        res.put("gpTypeSrNo", gpTypeSrNo);
        return res;
    }

    @Transactional
    public Map<String, Object> saveRecord(InwardGatePass obj) {
        Map<String, Object> res = new HashMap<>();
        try {
            // Deskop validation rules
            if (obj.getSupplierCustomerId() == null || obj.getSupplierCustomerId() <= 0) {
                res.put("success", false);
                res.put("message", "Supplier Name Field is Required!");
                return res;
            }
            if (obj.getCityId() == null || obj.getCityId() <= 0) {
                res.put("success", false);
                res.put("message", "City Name Field is Required!");
                return res;
            }

            // Defaults
            if (obj.getGpDate() == null) obj.setGpDate(new Date());
            if (obj.getInDateTimeStamp() == null) obj.setInDateTimeStamp(new Date());
            if (obj.getOutDateTimeStamp() == null) obj.setOutDateTimeStamp(new Date());

            // Auto-calculate Difference Weight
            double supplierWt = obj.getSupplierWeight() != null ? obj.getSupplierWeight() : 0.0;
            double factoryWt = obj.getFactoryWeight() != null ? obj.getFactoryWeight() : 0.0;
            obj.setDifferenceWeight(supplierWt - factoryWt);

            // Driver Biodata save/link
            if (obj.getDriverName() != null && !obj.getDriverName().trim().isEmpty()) {
                Integer bioId = repository.saveDriverBio(obj);
                if (bioId != null && bioId > 0) {
                    obj.setDriverBioDataId(bioId);
                }
            }

            // Generate codes if new
            if (obj.getId() == null || obj.getId() == 0) {
                obj.setEntryDate(new Date());
                if (obj.getGpSrNo() == null || obj.getGpSrNo() == 0) {
                    obj.setGpSrNo(repository.generateGpCode(obj.getOrganizationId(), obj.getCompanyId(), obj.getBranchesId(), obj.getFinancialYearId(), obj.getDocumentTypeId()));
                }
                if (obj.getGpTypeSrNo() == null || obj.getGpTypeSrNo() == 0) {
                    obj.setGpTypeSrNo(repository.generateGpTypeCode(obj.getOrganizationId(), obj.getCompanyId(), obj.getBranchesId(), obj.getFinancialYearId(), obj.getGatepassType()));
                }
            } else {
                obj.setModifyDate(new Date());
            }

            // Save Header
            Integer headerId = repository.saveHeader(obj);
            obj.setId(headerId);

            // Save Details
            if (obj.getId() != null && obj.getId() > 0) {
                repository.deleteDetailsByHeaderId(headerId);
                if (obj.getGatePassInwardDetails() != null) {
                    for (InwardGatePassDetail detail : obj.getGatePassInwardDetails()) {
                        detail.setGatePassInwardId(headerId);
                        if (detail.getSupplierCustomerId() == null) detail.setSupplierCustomerId(obj.getSupplierCustomerId());
                        if (detail.getCityId() == null) detail.setCityId(obj.getCityId());
                        repository.saveDetail(detail);
                    }
                }

                // Save Purchase BreakUp
                repository.deletePurchaseBreakUpsByHeaderId(headerId);
                if (obj.getGatePassInwardPurchaseBreakUpList() != null) {
                    for (InwardGatePassPurchaseBreakUp breakUp : obj.getGatePassInwardPurchaseBreakUpList()) {
                        breakUp.setInwardGatePassId(headerId);
                        repository.savePurchaseBreakUp(breakUp);
                    }
                }
            }

            res.put("success", true);
            res.put("igpId", headerId);
            res.put("message", "Record Saved Successfully! Document No: " + obj.getGpSrNo());
        } catch (Exception e) {
            e.printStackTrace();
            res.put("success", false);
            res.put("message", "Error saving Inward Gate Pass: " + e.getMessage());
        }
        return res;
    }

    public Map<String, Object> getById(Integer id) {
        Map<String, Object> res = new HashMap<>();
        Map<String, Object> header = repository.getHeaderById(id);
        if (header != null) {
            List<Map<String, Object>> details = repository.getDetailsByHeaderId(id);
            List<Map<String, Object>> breakUps = repository.getPurchaseBreakUpsByHeaderId(id);
            res.put("header", header);
            res.put("details", details);
            res.put("purchaseBreakUps", breakUps);
            res.put("success", true);
        } else {
            res.put("success", false);
            res.put("message", "Record not found");
        }
        return res;
    }

    public Map<String, Object> deleteRecord(Integer id, Integer orgId, Integer compId) {
        Map<String, Object> res = new HashMap<>();
        try {
            repository.deleteRecord(id, orgId, compId);
            res.put("success", true);
            res.put("message", "Record Deleted Successfully!");
        } catch (Exception e) {
            res.put("success", false);
            res.put("message", "Error deleting record: " + e.getMessage());
        }
        return res;
    }

    public List<Map<String, Object>> getHistory(Integer orgId, Integer compId, Integer branchId, Integer yearId,
                                                Integer docTypeId, String fromDate, String toDate,
                                                Double fromDocNo, Double toDocNo, Integer supplierId) {
        return repository.getHistory(orgId, compId, branchId, yearId, docTypeId, fromDate, toDate, fromDocNo, toDocNo, supplierId);
    }
}
