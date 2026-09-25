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

    /** Rights grid reader shared by the CMAGT screens (same lookup PO / Sale Order use). */
    @Autowired
    private com.mst.repositories.cmagt.SaleOrderCmagtRepository rightsRepo;

    public static final String DESKTOP_SCREEN_NAME = "frmGrnLoadingChallanCmagt";
    private static final String RIGHT_CAN_VIEW_ALL_RECORDS = "CanView AllRecord";
    private static final java.util.regex.Pattern VEHICLE_NO =
            java.util.regex.Pattern.compile("^[A-Z]{1,6}-\\d{1,6}$");

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

        /* entryDate/modifyDate/approvedDate are DateTime.Now on the desktop, isApproved false. */
        String now = java.time.LocalDate.now().toString();
        if (dto.getEntryDate() == null || dto.getEntryDate().isEmpty()) dto.setEntryDate(now);
        dto.setModifyDate(now);
        dto.setApprovedDate(now);
        dto.setIsApproved(false);

        /* Model properties frmGrnLoadingChallanCmagt.Insert() (:2593-2625) never assigns, so they
           travel as CLR defaults (null / 0 / false). The page has inputs for some of them that
           the desktop form does not have; they are not taken from the request. */
        dto.setSupplierRefDocNo(null);
        dto.setDeliveryStartDate(null);
        dto.setDeliveryDays(0);
        dto.setApprovalRemarks(null);
        dto.setIsSupplierOtherChargesAllowed(false);
        dto.setApprovedUserId(0);
        dto.setProjectId(0);
        dto.setAttachmentsValues(null);
        dto.setCustomAttachmentsValues(null);

        /* Freight block is assigned only when Total Freight > 0 (:2606-2613); otherwise the
           transporter and all three freight amounts stay at their defaults. */
        if (dto.getTotalFreight() == null || dto.getTotalFreight().signum() <= 0) {
            dto.setTransporterId(0);
            dto.setTransporterName(null);
            dto.setBiltyFreight(java.math.BigDecimal.ZERO);
            dto.setOtherAdLesCharges(java.math.BigDecimal.ZERO);
            dto.setTotalFreight(java.math.BigDecimal.ZERO);
        }

        validate(dto);

        return repository.saveOrUpdate(dto);
    }

    /**
     * The form's own save-time refusals, Insert() :2551-2590 and :2658-2665 / :2745-2748.
     * The two Yes/No confirmations (short vehicle number, bilty date differing from doc date)
     * are the page's to ask; the hard refusals are enforced here.
     */
    private void validate(GrnLoadingChallanCmagtDto dto) {
        List<GrnLoadingChallanCmagtDto.DetailDto> rows = dto.getGrnSupplierLoadingDetailList();
        boolean anyLive = false;
        if (rows != null) for (GrnLoadingChallanCmagtDto.DetailDto d : rows) {
            if (d.getActionTypeId() == null || d.getActionTypeId() != 3) { anyLive = true; break; }
        }
        if (!anyLive) throw new IllegalArgumentException("Detail Record Not Found");
        req(dto.getCompanyId(), "Company Name");
        req(dto.getBranchId(), "Branch Name");
        req(dto.getDocNo(), "Doc No");
        req(dto.getCommissionAgentId(), "Commission Agent");
        req(dto.getSupplierId(), "Supplier Name");
        req(dto.getLoadingCityId(), "Loading City");
        req(dto.getUnloadingCityId(), "Un-Loading City");
        req(dto.getVehicleTypeId(), "Vehicle Type");
        if (blank(dto.getVehicleNo())) throw new IllegalArgumentException("Vehicle No Field Required");
        if (blank(dto.getBiltyNo())) throw new IllegalArgumentException("Bilty No Field Required");
        req(dto.getDeliveryTermId(), "Delivery Term");
        if (dto.getBiltyQty() == null || dto.getBiltyQty().signum() <= 0)
            throw new IllegalArgumentException("Vehicle Qty Field Required");
        if (dto.getScaleNetWeight() == null || dto.getScaleNetWeight().signum() <= 0)
            throw new IllegalArgumentException("Net Wb Weight Field Required");
        if (dto.getTotalFreight() != null && dto.getTotalFreight().signum() > 0
                && (dto.getTransporterId() == null || dto.getTransporterId() == 0)
                && blank(dto.getTransporterName())) {
            throw new IllegalArgumentException("Please Select Transporter or Fill Transporter Name");
        }
        String vno = dto.getVehicleNo().trim().toUpperCase();
        if (!VEHICLE_NO.matcher(vno).matches()) {
            throw new IllegalArgumentException("Vehicle no is not valid. Please check!");
        }
        dto.setVehicleNo(dto.getVehicleNo().trim());
        dto.setBiltyNo(dto.getBiltyNo().trim());

        java.math.BigDecimal gross = java.math.BigDecimal.ZERO;
        int i = 0;
        for (GrnLoadingChallanCmagtDto.DetailDto d : rows) {
            i++;
            if (d.getActionTypeId() != null && d.getActionTypeId() == 3) continue;
            reqRow(d.getInventoryParentCategoryId(), "Parent Category", i);
            reqRow(d.getItemId(), "Item Name", i);
            reqRow(d.getCropYearId(), "Crop Year", i);
            reqRow(d.getPackingTypeId(), "Packing Type", i);
            reqRow(d.getPackUomId(), "Pack Uom", i);
            reqRow(d.getLoadingQty(), "Loading Qty", i);
            reqRow(d.getWbGrossWeight(), "Gross Weight", i);
            reqRow(d.getNetBillWeight(), "Net Bill Weight", i);
            gross = gross.add(d.getWbGrossWeight());
        }
        if (dto.getScaleNetWeight().compareTo(gross) != 0) {
            throw new IllegalArgumentException("Header Net Weight:" + dto.getScaleNetWeight().stripTrailingZeros().toPlainString()
                    + " not equal to detail gross weight:" + gross.stripTrailingZeros().toPlainString() + ". please Check!");
        }
    }

    private static boolean blank(String v) { return v == null || v.trim().isEmpty(); }
    private static void req(Integer v, String label) {
        if (v == null || v == 0) throw new IllegalArgumentException(label + " Field Required");
    }
    private static void reqRow(Object v, String label, int row) {
        boolean bad = v == null
                || (v instanceof Integer && (Integer) v == 0)
                || (v instanceof java.math.BigDecimal && ((java.math.BigDecimal) v).signum() == 0);
        if (bad) throw new IllegalArgumentException(label + " is required in detail row " + row);
    }

    /** FormHistory with @CanViewAllRecord read from this screen's rights, as HistoryFill :3703. */
    public List<Map<String, Object>> getHistory(String fromDate, String toDate) {
        boolean all = canViewAllRecords();
        return repository.formHistory(
                currentUserContext.currentOrganizationId(),
                currentUserContext.currentCompanyId(),
                currentUserContext.currentBranchId(),
                currentUserContext.currentFinancialYearId(),
                all, currentUserContext.currentUserId(), fromDate, toDate);
    }

    public boolean canViewAllRecords() {
        String role = currentUserContext.currentRoleName();
        if ("Admin".equalsIgnoreCase(role) || "Administrator".equalsIgnoreCase(role)) return true;
        try {
            for (Map<String, Object> r : rightsRepo.userRightsForScreen(
                    currentUserContext.currentUserId(), DESKTOP_SCREEN_NAME, role,
                    currentUserContext.currentCompanyId())) {
                Object name = pick(r, "RightName");
                if (name != null && RIGHT_CAN_VIEW_ALL_RECORDS.equalsIgnoreCase(name.toString().trim())) {
                    Object v = pick(r, "Value");
                    if (v instanceof Boolean) return (Boolean) v;
                    if (v instanceof Number)  return ((Number) v).intValue() != 0;
                    return v != null && ("1".equals(v.toString().trim())
                            || "true".equalsIgnoreCase(v.toString().trim()));
                }
            }
        } catch (Exception ignored) { }
        return false;
    }

    private static Object pick(Map<String, Object> row, String key) {
        if (row.containsKey(key)) return row.get(key);
        for (Map.Entry<String, Object> e : row.entrySet())
            if (e.getKey().equalsIgnoreCase(key)) return e.getValue();
        return null;
    }

    /** DocumentNoDbCall (:752) -> BLL GenerateCode with this form's DocumentTypeId. */
    public int generateNextDocNo() {
        return repository.generateCode(
                currentUserContext.currentOrganizationId(),
                currentUserContext.currentCompanyId(),
                currentUserContext.currentBranchId(),
                currentUserContext.currentFinancialYearId(),
                DOCUMENT_TYPE_ID);
    }

    /** btnDelete_Click (:2874) -> BLL DeleteByID(UserAccount.ID, RecId). */
    public Map<String, Object> deleteById(int id) {
        if (!repository.belongsTo(id, currentUserContext.currentOrganizationId(),
                currentUserContext.currentCompanyId())) {
            throw new IllegalArgumentException("No record found to Delete");
        }
        repository.deleteById(currentUserContext.currentUserId(), id);
        Map<String, Object> out = new java.util.LinkedHashMap<>();
        out.put("status", "SUCCESS");
        return out;
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
        return repository.getById(id, currentUserContext.currentOrganizationId(),
                currentUserContext.currentCompanyId());
    }
}
