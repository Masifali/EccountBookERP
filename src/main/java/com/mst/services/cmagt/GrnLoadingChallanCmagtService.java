package com.mst.services.cmagt;

import com.mst.models.cmagt.dto.GrnLoadingChallanCmagtDto;
import com.mst.repositories.cmagt.GrnLoadingChallanCmagtRepository;
import com.mst.security.CurrentUserContext;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.List;
import java.util.Map;

@Service
public class GrnLoadingChallanCmagtService {

    /** frmGrnLoadingChallanCmagt's own document type. */
    private static final int DOCUMENT_TYPE_ID = 1054;

    @Autowired
    private GrnLoadingChallanCmagtRepository repository;

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
    public Map<String, Object> saveOrUpdate(GrnLoadingChallanCmagtDto dto) {
        dto.setOrganizationId(currentUserContext.currentOrganizationId());
        dto.setCompanyId(currentUserContext.currentCompanyId());
        dto.setBranchId(currentUserContext.currentBranchId());
        dto.setFinancialYearId(currentUserContext.currentFinancialYearId());
        dto.setEntryUserId(currentUserContext.currentUserId());
        dto.setModifyUserId(currentUserContext.currentUserId());

        /* BLL grnSupplierLoadingMaster.Save:  actionId = masterId == 0 ? 1 : 2.
           The procedure branches on it, so an update sent without it was not reliably an
           update. Derived here, never taken from the request. */
        Integer id = dto.getGrnSupplierLoadingMasterId();
        dto.setActionId((id == null || id == 0) ? 1 : 2);

        /* DocumentTypeId belongs to THIS form, not to the model's name - 1054 is the value
           frmGrnLoadingChallanCmagt carries. It is set server-side for the same reason
           tenancy is. */
        dto.setDocumentTypeId(DOCUMENT_TYPE_ID);

        /* entryDate/modifyDate are DateTime.Now on the desktop (:2566, :2569). */
        String now = java.time.LocalDate.now().toString();
        if (dto.getEntryDate() == null || dto.getEntryDate().isEmpty()) dto.setEntryDate(now);
        dto.setModifyDate(now);

        return repository.saveOrUpdate(dto);
    }

    public List<Map<String, Object>> getHistory(Integer companyId, Integer organizationId, String fromDate, String toDate) {
        return repository.getHistory(companyId, organizationId, fromDate, toDate);
    }

    /** Tenancy for the loader comes from the session, as it does for every other call. */
    public List<Map<String, Object>> pendingPurchaseOrders(
            String fromDate, String toDate, Integer fromDocNo, Integer toDocNo,
            Integer recId, Integer commissionAgentId, Integer supplierId, Integer itemId,
            Integer deliveryToPartyId, String shipToAddress) {
        return repository.pendingPurchaseOrdersForGrn(
                currentUserContext.currentOrganizationId(),
                currentUserContext.currentCompanyId(),
                currentUserContext.currentBranchId(),
                currentUserContext.currentFinancialYearId(),
                fromDate, toDate, fromDocNo, toDocNo, recId,
                commissionAgentId, supplierId, itemId, deliveryToPartyId, shipToAddress);
    }

    /**
     * The full loader payload - all five result sets the desktop copies out of the DataSet
     * (frmGrnLoadingChallanCmagt.cs:910-915), not just the first.
     *
     * The screen needs every one of them: the pending rows carry the linkage ids a GRN detail
     * row cannot exist without (purchaseOrderDetailId, purchaseOrderSaleOrderMappingId,
     * saleOrderMasterId, saleOrderDetailId, buyerId, DeliverToPartyId, DeliverToAddressId),
     * and the other four populate the empty-bag, weight-cut and expense grids for the chosen
     * order. None of these can be typed by an operator.
     */
    public Map<String, List<Map<String, Object>>> pendingPurchaseOrderTables(
            String fromDate, String toDate, Integer fromDocNo, Integer toDocNo,
            Integer recId, Integer commissionAgentId, Integer supplierId, Integer itemId,
            Integer deliveryToPartyId, String shipToAddress) {
        return repository.pendingPurchaseOrderTables(
                currentUserContext.currentOrganizationId(),
                currentUserContext.currentCompanyId(),
                currentUserContext.currentBranchId(),
                currentUserContext.currentFinancialYearId(),
                fromDate, toDate, fromDocNo, toDocNo, recId,
                commissionAgentId, supplierId, itemId, deliveryToPartyId, shipToAddress);
    }

    public Map<String, Object> getById(Integer id) {
        return repository.getById(id);
    }
}
