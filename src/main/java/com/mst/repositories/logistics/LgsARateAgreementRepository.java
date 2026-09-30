package com.mst.repositories.logistics;

import com.mst.models.UserAccount;
import com.mst.models.logistics.LgsAAgreementDetail;
import com.mst.models.logistics.LgsAAgreementHeader;
import com.mst.models.logistics.LgsARateNegotiationDetail;
import com.mst.models.logistics.LgsARateNegotiationHeader;
import com.mst.models.logistics.LgsARateNegotiationSourceDocument;
import com.mst.repositories.hrm.HrmProcRepository;
import org.springframework.stereotype.Repository;

import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.services.hrm.HrmSupport.toInt;

/**
 * DAL of the lgstcm documents ported here:
 *
 * 937 / 941  frmLogisticRateNegotiation (DocumentTypeId 1300) / frmLogisticRateNegotiationTransporter (1302)
 *   BLL 0391 logisticRateNegotiationHeader, DAL 0348
 *     GenerateCode   [lgstcm].[USP_logisticRateNegotiationHeader_GetAllMethod] @OrganizationId,@CompanyId,@BranchesId,@FinancialYearId,@DocumentTypeId,'GenerateCode'
 *     ReadById       ... @Id,'ReadById' + 'ReadByHeaderId_logisticRateNegotiationDetail' + 'ReadByHeaderId_logisticRateNegotiationSourceDocument'
 *     FormHistory    ... 'FormHistory'
 *     DeleteByID     ... @EntryUserId,@Id,'DeleteById'
 *     GetDataForDropDownFromlogisticRateNegotiationHeader  [lgstcm].[USP_GetDataForDropDownFromlogisticRateNegotiationHeader] @OrganizationId,@CompanyId
 *     Save -> SetData  [lgstcm].[USP_logisticRateNegotiationHeader_InsertAndUpdate], [lgstcm].[USP_logisticRateNegotiationDetail_Insert] per row,
 *                      [lgstcm].[USP_logisticRateNegotiationSourceDocument_Insert] per row - one transaction.
 *
 * 939  frmLogisticAgreement (DocumentTypeId 1301) - BLL 0390 AgreementHeader, DAL 0347
 *     GenerateCode / ReadById ('ReadByHeaderId_AgreementDetail') / FormHistory / DeleteById on [lgstcm].[USP_AgreementHeader_GetAllMethod],
 *     [lgstcm].[USP_GetDataForDropDownFromAgreementHeader] @OrganizationId,@CompanyId,
 *     Save -> [lgstcm].[USP_AgreementHeader_InsertAndUpdate] + [lgstcm].[USP_AgreementDetail_Insert] per row.
 */
@Repository
public class LgsARateAgreementRepository {

    public static final String RN = "[lgstcm].[USP_logisticRateNegotiationHeader_GetAllMethod]";
    public static final String AG = "[lgstcm].[USP_AgreementHeader_GetAllMethod]";

    private final HrmProcRepository db;

    public LgsARateAgreementRepository(HrmProcRepository db) { this.db = db; }

    // ------------------------------------------------------------------ rate negotiation

    public int rnGenerateCode(UserAccount u, int yearId, int docType) {
        List<Map<String, Object>> r = db.rows(RN, "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "BranchesId", toInt(u.getBranchesId()), "FinancialYearId", yearId, "DocumentTypeId", docType, "Activity", "GenerateCode");
        return r.isEmpty() ? 0 : toInt(r.get(0).get("DocumentNo"));
    }

    public List<Map<String, Object>> rnHeader(int id) { return db.rows(RN, "Id", id, "Activity", "ReadById"); }

    public List<Map<String, Object>> rnDetail(int id) { return db.rows(RN, "Id", id, "Activity", "ReadByHeaderId_logisticRateNegotiationDetail"); }

    public List<Map<String, Object>> rnSourceDocuments(int id) {
        return db.rows(RN, "Id", id, "Activity", "ReadByHeaderId_logisticRateNegotiationSourceDocument");
    }

    public List<Map<String, Object>> rnDropDown(UserAccount u) {
        return db.rows("[lgstcm].[USP_GetDataForDropDownFromlogisticRateNegotiationHeader]", "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId());
    }

    /** FormHistory(ReportsParameters) with the guards of BLL 0391 :127-280; the filters come prepared by the service. */
    public List<Map<String, Object>> rnHistory(UserAccount u, int yearId, int docType, boolean canViewAll, Map<String, Object> filters) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("Activity", "FormHistory");
        p.put("OrganizationId", u.getOrganizationId());
        p.put("CompanyId", u.getCompanyId());
        p.put("BranchesId", toInt(u.getBranchesId()));
        p.put("FinancialYearId", yearId);
        p.put("DocumentTypeId", docType);
        p.put("CanViewAllRecord", canViewAll);
        if (!canViewAll) p.put("EntryUserId", u.getId());
        p.putAll(filters);
        return db.rows(RN, p);
    }

    public void rnDelete(int userId, int id) {
        db.tx(() -> db.rows(RN, "EntryUserId", userId, "Id", id, "Activity", "DeleteById"));
    }

    /** DAL logisticRateNegotiationHeader.SetData (attachments not ported - the lists are empty). */
    public int rnSave(LgsARateNegotiationHeader h, List<LgsARateNegotiationDetail> details, List<LgsARateNegotiationSourceDocument> docs) {
        return db.tx(() -> {
            if (details != null && details.isEmpty()) throw new IllegalStateException("Detail list not found");
            int num = db.set("[lgstcm].[USP_logisticRateNegotiationHeader_InsertAndUpdate]", h);
            h.logisticRateNegotiationHeaderId = num > 0 ? num : h.logisticRateNegotiationHeaderId;
            for (LgsARateNegotiationDetail d : details) {
                d.logisticRateNegotiationHeaderId = h.logisticRateNegotiationHeaderId;
                db.set("[lgstcm].[USP_logisticRateNegotiationDetail_Insert]", d);
            }
            for (LgsARateNegotiationSourceDocument s : docs) {
                s.logisticRateNegotiationHeaderId = h.logisticRateNegotiationHeaderId;
                db.set("[lgstcm].[USP_logisticRateNegotiationSourceDocument_Insert]", s);
            }
            return h.logisticRateNegotiationHeaderId;
        });
    }

    // ------------------------------------------------------------------ agreement

    public int agGenerateCode(UserAccount u, int yearId, int docType) {
        List<Map<String, Object>> r = db.rows(AG, "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "BranchesId", toInt(u.getBranchesId()), "FinancialYearId", yearId, "DocumentTypeId", docType, "Activity", "GenerateCode");
        return r.isEmpty() ? 0 : toInt(r.get(0).get("DocumentNo"));
    }

    public List<Map<String, Object>> agHeader(int id) { return db.rows(AG, "Id", id, "Activity", "ReadById"); }

    public List<Map<String, Object>> agDetail(int id) { return db.rows(AG, "Id", id, "Activity", "ReadByHeaderId_AgreementDetail"); }

    public List<Map<String, Object>> agDropDown(UserAccount u) {
        return db.rows("[lgstcm].[USP_GetDataForDropDownFromAgreementHeader]", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
    }

    /** FormHistory(ReportsParameters) with the guards of BLL 0390 :127-280 (no @DocumentTypeId, no @ApprovedToDate). */
    public List<Map<String, Object>> agHistory(UserAccount u, int yearId, boolean canViewAll, Map<String, Object> filters) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("Activity", "FormHistory");
        p.put("OrganizationId", u.getOrganizationId());
        p.put("CompanyId", u.getCompanyId());
        p.put("BranchesId", toInt(u.getBranchesId()));
        p.put("FinancialYearId", yearId);
        p.put("CanViewAllRecord", canViewAll);
        if (!canViewAll) p.put("EntryUserId", u.getId());
        p.putAll(filters);
        return db.rows(AG, p);
    }

    public void agDelete(int userId, int id) {
        db.tx(() -> db.rows(AG, "EntryUserId", userId, "Id", id, "Activity", "DeleteById"));
    }

    /** DAL AgreementHeader.SetData (attachments not ported). */
    public int agSave(LgsAAgreementHeader h, List<LgsAAgreementDetail> details) {
        return db.tx(() -> {
            if (details != null && details.isEmpty()) throw new IllegalStateException("Detail list not found");
            int num = db.set("[lgstcm].[USP_AgreementHeader_InsertAndUpdate]", h);
            h.AgreementHeaderId = num > 0 ? num : h.AgreementHeaderId;
            for (LgsAAgreementDetail d : details) {
                d.AgreementHeaderId = h.AgreementHeaderId;
                db.set("[lgstcm].[USP_AgreementDetail_Insert]", d);
            }
            return h.AgreementHeaderId;
        });
    }
}
