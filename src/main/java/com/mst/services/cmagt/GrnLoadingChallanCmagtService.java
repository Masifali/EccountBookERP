package com.mst.services.cmagt;

import com.mst.models.cmagt.dto.GrnLoadingChallanCmagtDto;
import com.mst.repositories.cmagt.GrnLoadingChallanCmagtRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.List;
import java.util.Map;

@Service
public class GrnLoadingChallanCmagtService {

    @Autowired
    private GrnLoadingChallanCmagtRepository repository;

    public Map<String, Object> saveOrUpdate(GrnLoadingChallanCmagtDto dto) {
        return repository.saveOrUpdate(dto);
    }

    public List<Map<String, Object>> getHistory(Integer companyId, Integer organizationId, String fromDate, String toDate) {
        return repository.getHistory(companyId, organizationId, fromDate, toDate);
    }

    public Map<String, Object> getById(Integer id) {
        return repository.getById(id);
    }
}
