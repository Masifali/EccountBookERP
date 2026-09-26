package com.mst.services;

import com.mst.models.InwardGatePass;
import com.mst.models.InwardGatePassDetail;
import com.mst.models.InwardGatePassPurchaseBreakUp;
import com.mst.repositories.InwardGatePassRepository;
import com.mst.repositories.InwardGatePassRecordRepository;
import com.mst.security.CurrentUserContext;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.*;

@Service
public class InwardGatePassService {

    @Autowired
    private InwardGatePassRepository repository;

    @Autowired private InwardGatePassRecordRepository records;
    @Autowired private CurrentUserContext context;

    public Map<String, Object> getDropdowns(Integer orgId, Integer compId) {
        Map<String, Object> map = new HashMap<>();
        map.put("suppliers", repository.getSuppliers(orgId, compId));
        map.put("cities", repository.getCities(orgId, compId));
        map.put("vehicleTypes", repository.getVehicleTypes(orgId, compId));
        map.put("gatePassTypes", repository.getGatePassTypes(orgId, compId));
        map.put("orderTypes", repository.getOrderTypes(orgId, compId, context.currentBranchId()));
        map.put("items", repository.getItems(orgId, compId));
        map.put("weighBridges", repository.getWeighBridges());
        map.put("packingTypes", repository.getPackingTypes());
        map.put("transitVehicles", List.of()); // Depends on the selected supplier/order.
        map.put("statuses", repository.getStatuses(orgId, compId));
        map.put("documentTypes", repository.getDocumentTypes(orgId, compId));
        return map;
    }

    public Map<String, Object> findDriverBioByCnic(String cnic) {
        return repository.findDriverBio(context.currentOrganizationId(),context.currentCompanyId(),cnic,null);
    }

    public List<Map<String,Object>> getOpenGatePasses() {
        return repository.getOpenGatePasses(context.currentOrganizationId(),context.currentCompanyId(),context.currentBranchId(),context.currentFinancialYearId());
    }

    public Map<String, Object> findDriverBioByCell(String cell) {
        return repository.findDriverBio(context.currentOrganizationId(),context.currentCompanyId(),null,cell);
    }

    public List<Map<String, Object>> getPoInfoGrid(Integer orgId, Integer compId,
            String fromDate, String toDate, Double fromDocNo, Double toDocNo,
            Integer supplierId, Integer documentTypeId, Integer expiryDays, String dateField) {
        return repository.getPoInfoGrid(orgId, compId, context.currentBranchId(), context.currentFinancialYearId(),
                fromDate, toDate, fromDocNo, toDocNo, supplierId, documentTypeId, expiryDays, dateField);
    }

    public List<Map<String,Object>> getOrderPartyItems(int number,String date,int gatePassId) {
        if (gatePassId>0) records.require(gatePassId);
        return repository.getOrderPartyItems(context.currentOrganizationId(),context.currentCompanyId(),context.currentBranchId(),context.currentFinancialYearId(),number,date,gatePassId);
    }

    public Map<String, Object> generateNextNumbers(Integer orgId, Integer compId, Integer branchId, Integer yearId, Integer docTypeId, String gatepassType) {
        if (docTypeId!=51) throw new org.springframework.web.server.ResponseStatusException(org.springframework.http.HttpStatus.BAD_REQUEST,"This form uses document type 51 only");
        Map<String, Object> res = new HashMap<>();
        Integer gpSrNo = repository.generateGpCode(orgId, compId, branchId, yearId, docTypeId);
        Integer gpTypeSrNo = repository.generateGpTypeCode(orgId, compId, branchId, yearId, gatepassType);
        res.put("gpSrNo", gpSrNo);
        res.put("gpTypeSrNo", gpTypeSrNo);
        return res;
    }

    @Transactional
    public Map<String, Object> saveRecord(InwardGatePass obj) {
        if (obj.getDocumentTypeId()!=null && obj.getDocumentTypeId()!=51)
            throw new org.springframework.web.server.ResponseStatusException(org.springframework.http.HttpStatus.BAD_REQUEST,"This form saves Inward Gate Pass document type 51 only");
        obj.setDocumentTypeId(51);
        obj.setOrganizationId(context.currentOrganizationId()); obj.setCompanyId(context.currentCompanyId());
        obj.setBranchesId(context.currentBranchId()); obj.setFinancialYearId(context.currentFinancialYearId());
        obj.setModifyUser(context.currentUserId());
        boolean updating=obj.getId()!=null && obj.getId()>0;
        if (updating) {
            Map<String,Object> previous=records.require(obj.getId());
            obj.setEntryUser(((Number)previous.get("EntryUser")).intValue());
            obj.setEntryDate((Date)previous.get("EntryDate"));
            obj.setIsApproved(Boolean.TRUE.equals(previous.get("IsApproved")));
            obj.setPostState(Boolean.TRUE.equals(previous.get("PostState")));
            obj.setPostDate((Date)previous.get("PostDate"));
            obj.setPostUser(previous.get("PostUser") instanceof Number?((Number)previous.get("PostUser")).intValue():0);
        } else {
            obj.setEntryUser(context.currentUserId()); obj.setEntryDate(new Date());
            obj.setIsApproved(false); obj.setPostState(false); obj.setPostUser(0); obj.setPostDate(null);
        }
        Map<String, Object> res = new HashMap<>();
        try {
            // Desktop validation rules
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

            // Calculate and validate noOfPackages (NoofBags) required by Sp_GatePassInward_Insert
            if (obj.getNoOfPackages() == null || obj.getNoOfPackages() <= 0) {
                int bags = 0;
                if (obj.getGatePassInwardDetails() != null && !obj.getGatePassInwardDetails().isEmpty()) {
                    for (InwardGatePassDetail detail : obj.getGatePassInwardDetails()) {
                        if (detail.getItemQty() != null && detail.getItemQty() > 0) {
                            bags += detail.getItemQty().intValue();
                        }
                    }
                }
                if (bags <= 0 && obj.getGatePassInwardPurchaseBreakUpList() != null && !obj.getGatePassInwardPurchaseBreakUpList().isEmpty()) {
                    for (InwardGatePassPurchaseBreakUp breakUp : obj.getGatePassInwardPurchaseBreakUpList()) {
                        if (breakUp.getQty() != null && breakUp.getQty() > 0) {
                            bags += breakUp.getQty().intValue();
                        }
                    }
                }
                if (bags <= 0) {
                    res.put("success", false);
                    res.put("message", "NoofBags Field Required! Please enter Item Qty / No. of Bags.");
                    return res;
                }
                obj.setNoOfPackages(bags);
            }

            // Defaults
            if (obj.getGpDate() == null) obj.setGpDate(new Date());
            if (obj.getInDateTimeStamp() == null) obj.setInDateTimeStamp(new Date());
            if (obj.getOutDateTimeStamp() == null) obj.setOutDateTimeStamp(new Date());

            // Auto-calculate Difference Weight
            double supplierWt = obj.getSupplierWeight() != null ? obj.getSupplierWeight() : 0.0;
            double factoryWt = obj.getFactoryWeight() != null ? obj.getFactoryWeight() : 0.0;
            obj.setDifferenceWeight(Math.abs(supplierWt - factoryWt));

            // Driver Biodata save/link
            if ((obj.getDriverBioDataId()==null || obj.getDriverBioDataId()==0) && obj.getDriverName() != null && !obj.getDriverName().trim().isEmpty()) {
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
                // Sp_GatePassInward_Update already rebuilds both child collections.
                if (obj.getGatePassInwardDetails() != null) {
                    for (InwardGatePassDetail detail : obj.getGatePassInwardDetails()) {
                        detail.setGatePassInwardId(headerId);
                        if (detail.getSupplierCustomerId() == null) detail.setSupplierCustomerId(obj.getSupplierCustomerId());
                        if (detail.getCityId() == null) detail.setCityId(obj.getCityId());
                        repository.saveDetail(detail);
                    }
                }

                // Save Purchase BreakUp
                if (obj.getGatePassInwardPurchaseBreakUpList() != null) {
                    for (InwardGatePassPurchaseBreakUp breakUp : obj.getGatePassInwardPurchaseBreakUpList()) {
                        breakUp.setInwardGatePassId(headerId);
                        repository.savePurchaseBreakUp(breakUp);
                    }
                }
            }

            res.put("success", true);
            res.put("igpId", headerId);
            Map<String,Object> persisted=repository.getHeaderById(headerId);
            res.put("gpSrNo",persisted.get("GpSrNo")); res.put("gpTypeSrNo",persisted.get("GpTypeSrNo"));
            res.put("message", "Record Saved Successfully! Document No: " + persisted.get("GpSrNo"));
        } catch (Exception e) {
            org.springframework.transaction.interceptor.TransactionAspectSupport.currentTransactionStatus().setRollbackOnly();
            res.put("success", false);
            res.put("message", "Error saving Inward Gate Pass: " + e.getMessage());
        }
        return res;
    }

    public Map<String, Object> getById(Integer id) {
        records.require(id);
        Map<String, Object> res = new HashMap<>();
        Map<String, Object> header = repository.getHeaderById(id);
        if (header != null) {
            List<Map<String, Object>> details = repository.getDetailsByHeaderId(id);
            List<Map<String, Object>> breakUps = repository.getPurchaseBreakUpsByHeaderId(id);
            res.put("header", header);
            res.put("details", details);
            res.put("purchaseBreakUps", breakUps);
            res.put("weighBridgeWeights", repository.getWeighBridgeWeights(context.currentOrganizationId(),context.currentCompanyId(),id));
            res.put("success", true);
        } else {
            res.put("success", false);
            res.put("message", "Record not found");
        }
        return res;
    }

    public List<Map<String,Object>> getTransitVehicles(int supplier,int order,int gatePass) {
        if (gatePass>0) records.require(gatePass);
        return repository.getTransitVehicles(context.currentOrganizationId(),context.currentCompanyId(),supplier,order,gatePass);
    }

    public Map<String, Object> deleteRecord(Integer id, Integer orgId, Integer compId) {
        // Desktop btnDelete_Click is empty and InitializeComponent hides the button.
        throw new org.springframework.web.server.ResponseStatusException(org.springframework.http.HttpStatus.METHOD_NOT_ALLOWED,"The desktop Inward Gate Pass form does not support header deletion");
    }

    public List<Map<String, Object>> getHistory(Integer orgId, Integer compId, Integer branchId, Integer yearId,
                                                Integer docTypeId, String fromDate, String toDate,
                                                Double fromDocNo, Double toDocNo, Integer supplierId) {
        return getHistory(fromDate,toDate,fromDocNo,toDocNo,supplierId,"docDate");
    }

    public List<Map<String,Object>> getHistory(String fromDate,String toDate,Double fromDocNo,Double toDocNo,Integer supplierId,String dateField) {
        return repository.getHistory(context.currentOrganizationId(), context.currentCompanyId(), context.currentBranchId(), context.currentFinancialYearId(), 51, fromDate, toDate, fromDocNo, toDocNo, supplierId, records.canViewAll(), context.currentUserId(),dateField);
    }
}
