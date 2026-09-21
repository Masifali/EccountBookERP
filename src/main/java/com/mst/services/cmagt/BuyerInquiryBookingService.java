package com.mst.services.cmagt;

import com.mst.models.cmagt.dto.BuyerInquiryBookingDto;
import com.mst.repositories.cmagt.BuyerInquiryBookingRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.*;

@Service
public class BuyerInquiryBookingService {

    @Autowired
    private BuyerInquiryBookingRepository repository;

    public int generateNextDocNo(int orgId, int companyId, int branchId, int yearId, int docTypeId) {
        return repository.generateCode(orgId, companyId, branchId, yearId, docTypeId);
    }

    @Transactional
    public Map<String, Object> saveRecord(BuyerInquiryBookingDto dto) {
        Map<String, Object> res = new HashMap<>();
        try {
            if (dto.getBuyerId() == null || dto.getBuyerId() <= 0) {
                res.put("success", false);
                res.put("message", "Buyer Name Field is Required!");
                return res;
            }

            boolean isNew = (dto.getInquiryBookingMasterId() == null || dto.getInquiryBookingMasterId() == 0);
            if (isNew) {
                if (dto.getInquiryBookingNo() == null || dto.getInquiryBookingNo() == 0) {
                    dto.setInquiryBookingNo(repository.generateCode(dto.getOrganizationId(), dto.getCompanyId(), dto.getBranchId(), dto.getFinancialYearId(), dto.getDocumentTypeId()));
                }
                dto.setActionId(1);
            } else {
                dto.setActionId(2);
            }

            int masterId = repository.saveMaster(dto);
            if (masterId <= 0) {
                masterId = dto.getInquiryBookingMasterId();
            }

            if (masterId > 0) {
                if (dto.getInquiryBookingDetailList() != null) {
                    for (BuyerInquiryBookingDto.BuyerInquiryDetailDto detail : dto.getInquiryBookingDetailList()) {
                        detail.setInquiryBookingMasterId(masterId);
                        repository.saveDetailRow(detail);
                    }
                }
                if (dto.getInquiryBookingPartyDetailList() != null) {
                    for (BuyerInquiryBookingDto.BuyerInquiryPartyDetailDto partyDetail : dto.getInquiryBookingPartyDetailList()) {
                        partyDetail.setInquiryBookingMasterId(masterId);
                        repository.savePartyDetailRow(partyDetail);
                    }
                }
            }

            res.put("success", true);
            res.put("id", masterId);
            res.put("message", "Record Saved Successfully! Document No: " + dto.getInquiryBookingNo());
        } catch (Exception e) {
            e.printStackTrace();
            res.put("success", false);
            res.put("message", "Error saving Buyer Inquiry Booking: " + e.getMessage());
        }
        return res;
    }

    public Map<String, Object> getById(int id) {
        Map<String, Object> res = new HashMap<>();
        List<Map<String, Object>> header = repository.readHeaderById(id);
        if (header != null && !header.isEmpty()) {
            res.put("header", header.get(0));
            res.put("details", repository.readDetailByHeaderId(id));
            res.put("partyDetails", repository.readPartyDetailByHeaderId(id));
            res.put("success", true);
        } else {
            res.put("success", false);
            res.put("message", "Record not found");
        }
        return res;
    }

    public Map<String, Object> deleteRecord(int entryUserId, int id) {
        Map<String, Object> res = new HashMap<>();
        try {
            repository.deleteById(entryUserId, id);
            res.put("success", true);
            res.put("message", "Record Deleted Successfully!");
        } catch (Exception e) {
            res.put("success", false);
            res.put("message", "Error deleting record: " + e.getMessage());
        }
        return res;
    }

    public List<Map<String, Object>> getHistory(int orgId, int companyId, int branchId, int yearId,
                                                boolean canViewAllRecords, int entryUserId,
                                                String fromDate, String toDate,
                                                Integer fromDocNo, Integer toDocNo, Integer id,
                                                Integer commissionAgentId, Integer buyerId, Integer itemId) {
        return repository.formHistory(orgId, companyId, branchId, yearId, canViewAllRecords, entryUserId,
                fromDate, toDate, fromDocNo, toDocNo, id, commissionAgentId, buyerId, itemId);
    }
}
