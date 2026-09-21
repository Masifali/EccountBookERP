package com.mst.serviceInterface;

import java.util.List;
import java.util.Map;

public interface QualityControlService {

    List<Map<String, Object>> getLabs();
    List<Map<String, Object>> getAnalysisParameters();
    List<Map<String, Object>> getAnalysisGroups();
    
    List<Map<String, Object>> getSampleLogHistory();
    List<Map<String, Object>> getSampleAnalysisHistory();
    List<Map<String, Object>> getPurchaseAnalysisHistory();
    List<Map<String, Object>> getInProcessAnalysisHistory();

    Map<String, Object> getSampleLogById(int id);
    Map<String, Object> getSampleAnalysisById(int id);
    Map<String, Object> getPurchaseAnalysisById(int id);
    Map<String, Object> getInProcessAnalysisById(int id);
}
