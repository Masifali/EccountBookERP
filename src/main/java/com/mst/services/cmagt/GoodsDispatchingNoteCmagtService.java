package com.mst.services.cmagt;

import com.mst.models.cmagt.dto.GoodsDispatchingNoteCmagtDto;
import com.mst.repositories.cmagt.GoodsDispatchingNoteCmagtRepository;
import com.mst.security.CurrentUserContext;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.List;
import java.util.Map;

@Service
public class GoodsDispatchingNoteCmagtService {

    @Autowired
    private GoodsDispatchingNoteCmagtRepository repository;

    @Autowired
    private CurrentUserContext currentUserContext;

    /**
     * Organization, company, branch, financial year and the user come from the SESSION, never
     * from the request body. They used to be read straight off the posted JSON and default to 1
     * in the repository, so a crafted payload could file a dispatch note against another
     * company, or under another user's id. The desktop reads UserAccount and
     * clsGlobalVariables.ActiveYr for exactly these five and gives the operator no way to
     * override them (:1877-1883).
     */
    public Map<String, Object> saveOrUpdate(GoodsDispatchingNoteCmagtDto dto) {
        dto.setOrganizationId(currentUserContext.currentOrganizationId());
        dto.setCompanyId(currentUserContext.currentCompanyId());
        dto.setBranchId(currentUserContext.currentBranchId());
        dto.setFinancialYearId(currentUserContext.currentFinancialYearId());
        dto.setEntryUserId(currentUserContext.currentUserId());
        dto.setModifyUserId(currentUserContext.currentUserId());
        return repository.saveOrUpdate(dto);
    }

    public List<Map<String, Object>> getHistory(Integer companyId, Integer organizationId, String fromDate, String toDate) {
        return repository.getHistory(companyId, organizationId, fromDate, toDate);
    }

    public Map<String, Object> getById(Integer id) {
        return repository.getById(id);
    }
}
