package com.mst.services.cmagt;

import com.mst.models.cmagt.dto.TradeBillAgainstGdnCmagtDto;
import com.mst.repositories.cmagt.TradeBillAgainstGdnCmagtRepository;
import com.mst.security.CurrentUserContext;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.List;
import java.util.Map;

@Service
public class TradeBillAgainstGdnCmagtService {

    @Autowired
    private TradeBillAgainstGdnCmagtRepository repository;

    @Autowired
    private CurrentUserContext currentUserContext;

    /**
     * Organization, company, branch, financial year and the user come from the SESSION, never
     * from the request body.
     *
     * They used to be read straight off the posted JSON and, when absent, defaulted to 1 in the
     * repository. Two separate faults in one line: a crafted payload could file this document
     * against another company, and an omitted value silently filed it against company 1 - the
     * same fabricated default that made the Purchase Order screen read company 1 and show PO-1
     * where the desktop showed PO-493. entryUserId/modifyUserId are the sharper edge: those are
     * authorship, and a client must never choose them.
     *
     * The desktop reads UserAccount and clsGlobalVariables.ActiveYr for exactly these five and
     * gives the operator no way to override either.
     */
    public Map<String, Object> saveOrUpdate(TradeBillAgainstGdnCmagtDto dto) {
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
