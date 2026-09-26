package com.mst.services;

import com.mst.repositories.QualityControlRepository;
import com.mst.serviceInterface.QualityControlService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.List;
import java.util.Map;

@Service
public class QualityControlServiceImpl implements QualityControlService {

    @Autowired
    private QualityControlRepository qcRepository;

    private static final int DEFAULT_ORG_ID = 1;
    private static final int DEFAULT_COMPANY_ID = 1;
    private static final int DEFAULT_BRANCH_ID = 1;

    @Override
    public List<Map<String, Object>> getLabs() {
        return qcRepository.getLabs(DEFAULT_ORG_ID, DEFAULT_COMPANY_ID, DEFAULT_BRANCH_ID);
    }

    @Override
    public List<Map<String, Object>> getAnalysisParameters() {
        return qcRepository.getAnalysisParameters(DEFAULT_ORG_ID, DEFAULT_COMPANY_ID, DEFAULT_BRANCH_ID);
    }

    @Override
    public List<Map<String, Object>> getAnalysisGroups() {
        return qcRepository.getAnalysisGroups(DEFAULT_ORG_ID, DEFAULT_COMPANY_ID, DEFAULT_BRANCH_ID);
    }

    @Override
    public List<Map<String, Object>> getSampleLogHistory() {
        return qcRepository.getSampleLogHistory(DEFAULT_ORG_ID, DEFAULT_COMPANY_ID, DEFAULT_BRANCH_ID);
    }

    @Override
    public List<Map<String, Object>> getSampleAnalysisHistory() {
        return qcRepository.getSampleAnalysisHistory(DEFAULT_ORG_ID, DEFAULT_COMPANY_ID, DEFAULT_BRANCH_ID);
    }

    @Override
    public List<Map<String, Object>> getPurchaseAnalysisHistory() {
        return qcRepository.getPurchaseAnalysisHistory(DEFAULT_ORG_ID, DEFAULT_COMPANY_ID, DEFAULT_BRANCH_ID);
    }

    @Override
    public List<Map<String, Object>> getInProcessAnalysisHistory() {
        return qcRepository.getInProcessAnalysisHistory(DEFAULT_ORG_ID, DEFAULT_COMPANY_ID, DEFAULT_BRANCH_ID);
    }

    @Override
    public Map<String, Object> getSampleLogById(int id) {
        return qcRepository.getSampleLogById(id);
    }

    @Override
    public Map<String, Object> getSampleAnalysisById(int id) {
        return qcRepository.getSampleAnalysisById(id);
    }

    @Override
    public Map<String, Object> getPurchaseAnalysisById(int id) {
        return qcRepository.getPurchaseAnalysisById(id);
    }

    @Override
    public Map<String, Object> getInProcessAnalysisById(int id) {
        return qcRepository.getInProcessAnalysisById(id);
    }
}
