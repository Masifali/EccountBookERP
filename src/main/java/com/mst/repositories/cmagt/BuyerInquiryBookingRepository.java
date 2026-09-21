package com.mst.repositories.cmagt;

import com.mst.models.cmagt.dto.BuyerInquiryBookingDto;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.sql.Timestamp;
import java.sql.Types;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.List;
import java.util.Map;

@Repository
public class BuyerInquiryBookingRepository {

    @Autowired
    private JdbcTemplate jdbc;

    private static final String SQL_MASTER_SAVE =
            "EXEC [cmagt].[USP_inquiryBookingMaster_InsertAndUpdate] "
            + "@isApproved=?, @approvedDate=?, @deliveryStartDate=?, @entryDate=?, @expiryDate=?, "
            + "@inquiryBookingDate=?, @modifyDate=?, @validityDate=?, @actionId=?, @analysisGroupId=?, "
            + "@approvedUserId=?, @branchId=?, @buyerId=?, @commissionAgentId=?, @inquiryStatusId=?, "
            + "@companyId=?, @deliveryDays=?, @deliveryTermId=?, @documentTypeId=?, @entryUserId=?, "
            + "@financialYearId=?, @inquiryBookingMasterId=?, @inquiryBookingNo=?, @modifyUserId=?, "
            + "@organizationId=?, @projectId=?, @validityDays=?, @ShipToAddress=?, @shipToAddressId=?, "
            + "@DeliveryToPartyId=?, @remarksHeader=?, @approvalRemarks=?, @attachmentsValues=?, "
            + "@customAttachmentsValues=?, @QualitySpecifications=?";

    private static final String SQL_DETAIL_SAVE =
            "EXEC [cmagt].[USP_inquiryBookingDetail_Insert] "
            + "@buyerAmount=?, @buyerRate=?, @itemQty=?, @itemWeight=?, @SupplierAmount=?, "
            + "@supplierRate=?, @actionTypeId=?, @cropYearId=?, @inquiryBookingDetailId=?, "
            + "@inquiryBookingMasterId=?, @inventoryParentCategoryId=?, @itemId=?, @packingTypeId=?, "
            + "@packUomId=?, @rateUomId=?, @sortNo=?, @cropYear=?, @remarks=?, @QualitySpecifications=?";

    private static final String SQL_PARTY_DETAIL_SAVE =
            "EXEC [cmagt].[USP_InquiryBookingPartyDetail_Insert] "
            + "@inquiryBookingPartyDetailId=?, @inquiryBookingMasterId=?, @SubPartyId=?, @itemQty=?, "
            + "@Rate=?, @rateUomId=?, @Amount=?, @sortNo=?, @remarks=?, @actionTypeId=?";

    private static final String SQL_READ_BY_ID =
            "EXEC [cmagt].[USP_inquiryBookingMaster_GetAllMethod] @Id=?, @Activity=?";

    private static final String SQL_GENERATE_CODE =
            "EXEC [cmagt].[USP_inquiryBookingMaster_GetAllMethod] @OrganizationId=?, @CompanyId=?, "
            + "@BranchesId=?, @FinancialYearId=?, @DocumentTypeId=?, @Activity=?";

    private static final String SQL_DELETE_BY_ID =
            "EXEC [cmagt].[USP_inquiryBookingMaster_GetAllMethod] @EntryUserId=?, @Id=?, @Activity=?";

    private static final String SQL_FORM_HISTORY =
            "EXEC [cmagt].[USP_inquiryBookingMaster_FormHistory] "
            + "@OrganizationId=?, @CompanyId=?, @BranchesId=?, @FinancialYearId=?, "
            + "@CanViewAllRecord=?, @EntryUserId=?, @FromDate=?, @ToDate=?, "
            + "@EntryFromDate=?, @EntryToDate=?, @ModifyFromDate=?, @ModifyToDate=?, "
            + "@ApprovedFromDate=?, @ApprovedToDate=?, @ValidityDateFrom=?, @ValidityDateTo=?, "
            + "@FromDocNo=?, @ToDocNo=?, @Id=?, @CommissionAgentId=?, @BuyerId=?, @ItemId=?, "
            + "@ParentItemIds=?";

    public int saveMaster(BuyerInquiryBookingDto h) {
        return scalarInt(SQL_MASTER_SAVE,
                bool(h.getIsApproved()),
                ts(h.getApprovedDate()),
                ts(h.getDeliveryStartDate()),
                ts(h.getEntryDate()),
                ts(h.getExpiryDate()),
                ts(h.getInquiryBookingDate()),
                ts(h.getModifyDate()),
                ts(h.getValidityDate()),
                i(h.getActionId()),
                i(h.getAnalysisGroupId()),
                i(h.getApprovedUserId()),
                i(h.getBranchId()),
                i(h.getBuyerId()),
                i(h.getCommissionAgentId()),
                i(h.getInquiryStatusId()),
                i(h.getCompanyId()),
                i(h.getDeliveryDays()),
                i(h.getDeliveryTermId()),
                i(h.getDocumentTypeId()),
                i(h.getEntryUserId()),
                i(h.getFinancialYearId()),
                i(h.getInquiryBookingMasterId()),
                i(h.getInquiryBookingNo()),
                i(h.getModifyUserId()),
                i(h.getOrganizationId()),
                i(h.getProjectId()),
                i(h.getValidityDays()),
                s(h.getShipToAddress()),
                i(h.getShipToAddressId()),
                i(h.getDeliveryToPartyId()),
                s(h.getRemarksHeader()),
                s(h.getApprovalRemarks()),
                s(h.getAttachmentsValues()),
                s(h.getCustomAttachmentsValues()),
                s(h.getQualitySpecifications())
        );
    }

    public void saveDetailRow(BuyerInquiryBookingDto.BuyerInquiryDetailDto d) {
        scalarInt(SQL_DETAIL_SAVE,
                num(d.getBuyerAmount()),
                num(d.getBuyerRate()),
                num(d.getItemQty()),
                num(d.getItemWeight()),
                num(d.getSupplierAmount()),
                num(d.getSupplierRate()),
                i(d.getActionTypeId()),
                i(d.getCropYearId()),
                i(d.getInquiryBookingDetailId()),
                i(d.getInquiryBookingMasterId()),
                i(d.getInventoryParentCategoryId()),
                i(d.getItemId()),
                i(d.getPackingTypeId()),
                i(d.getPackUomId()),
                i(d.getRateUomId()),
                i(d.getSortNo()),
                s(d.getCropYear()),
                s(d.getRemarks()),
                s(d.getQualitySpecifications())
        );
    }

    public void savePartyDetailRow(BuyerInquiryBookingDto.BuyerInquiryPartyDetailDto p) {
        scalarInt(SQL_PARTY_DETAIL_SAVE,
                i(p.getInquiryBookingPartyDetailId()),
                i(p.getInquiryBookingMasterId()),
                i(p.getSubPartyId()),
                num(p.getItemQty()),
                num(p.getRate()),
                i(p.getRateUomId()),
                num(p.getAmount()),
                i(p.getSortNo()),
                s(p.getRemarks()),
                i(p.getActionTypeId())
        );
    }

    public List<Map<String, Object>> readHeaderById(int id) {
        return jdbc.queryForList(SQL_READ_BY_ID, id, "ReadById");
    }

    public List<Map<String, Object>> readDetailByHeaderId(int id) {
        return jdbc.queryForList(SQL_READ_BY_ID, id, "ReadByHeaderId_inquiryBookingDetail");
    }

    public List<Map<String, Object>> readPartyDetailByHeaderId(int id) {
        return jdbc.queryForList(SQL_READ_BY_ID, id, "ReadByHeaderId_InquiryBookingPartyDetail");
    }

    public int generateCode(int orgId, int companyId, int branchId, int financialYearId, int documentTypeId) {
        List<Map<String, Object>> rows = jdbc.queryForList(SQL_GENERATE_CODE,
                orgId, companyId, branchId, financialYearId, documentTypeId, "GenerateCode");
        if (rows.isEmpty()) return 1;
        Object v = rows.get(0).get("DocumentNo");
        if (v == null) v = rows.get(0).get("DocNo");
        return (v instanceof Number) ? ((Number) v).intValue() : 1;
    }

    public void deleteById(int entryUserId, int id) {
        jdbc.update(SQL_DELETE_BY_ID, entryUserId, id, "DeleteById");
    }

    public List<Map<String, Object>> formHistory(int orgId, int companyId, int branchId, int financialYearId,
                                                 boolean canViewAllRecords, int entryUserId,
                                                 String fromDate, String toDate,
                                                 Integer fromDocNo, Integer toDocNo, Integer id,
                                                 Integer commissionAgentId, Integer buyerId, Integer itemId) {
        return jdbc.queryForList(SQL_FORM_HISTORY,
                orgId, companyId, branchId, financialYearId,
                (canViewAllRecords ? 1 : 0), entryUserId,
                dateOrNull(fromDate), dateOrNull(toDate),
                null, null, null, null, null, null, null, null,
                intOrNull(fromDocNo), intOrNull(toDocNo), intOrNull(id),
                intOrNull(commissionAgentId), intOrNull(buyerId), intOrNull(itemId), null);
    }

    private int scalarInt(String sql, Object... args) {
        Integer v = jdbc.query(sql, rs -> {
            if (rs.next()) {
                Object o = rs.getObject(1);
                if (o instanceof Number) return ((Number) o).intValue();
                if (o != null) {
                    try { return Integer.valueOf(o.toString().trim()); } catch (Exception ignored) {}
                }
            }
            return 0;
        }, args);
        return v == null ? 0 : v;
    }

    private static Object s(String v) { return v == null ? "" : v; }
    private static Object i(Integer v) { return v == null ? 0 : v; }
    private static Object num(java.math.BigDecimal v) { return v == null ? java.math.BigDecimal.ZERO : v; }
    private static Object bool(Boolean v) { return v != null && v; }

    private static Object ts(String iso) {
        LocalDateTime t = parse(iso);
        return Timestamp.valueOf(t == null ? LocalDateTime.now() : t);
    }

    private static Object intOrNull(Integer v) {
        return (v == null || v == 0)
                ? new org.springframework.jdbc.core.SqlParameterValue(Types.INTEGER, null)
                : v;
    }

    private static Object dateOrNull(String iso) {
        LocalDateTime t = parse(iso);
        return t == null ? new org.springframework.jdbc.core.SqlParameterValue(Types.TIMESTAMP, null)
                : Timestamp.valueOf(t);
    }

    private static LocalDateTime parse(String iso) {
        if (iso == null || iso.trim().isEmpty()) return null;
        String v = iso.trim();
        try {
            if (v.length() <= 10) return LocalDate.parse(v).atStartOfDay();
            return LocalDateTime.parse(v.replace(' ', 'T').substring(0, Math.min(19, v.length())));
        } catch (Exception ex) {
            return null;
        }
    }
}
