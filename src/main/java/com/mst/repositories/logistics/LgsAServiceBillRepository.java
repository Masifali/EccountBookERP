package com.mst.repositories.logistics;

import com.mst.models.UserAccount;
import com.mst.models.logistics.LgsAClearingAgentBill;
import com.mst.models.logistics.LgsAClearingAgentBillDetail;
import com.mst.models.logistics.LgsAVoucherDetail;
import com.mst.models.logistics.LgsAVoucherHead;
import com.mst.repositories.hrm.HrmProcRepository;
import org.springframework.stereotype.Repository;

import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.services.hrm.HrmSupport.toInt;

/**
 * DAL of 233 "Service Bill Direct" (Architecture.WinApp.Service/ExImClearingAgentBillDirect.cs, DocumentTypeId 155).
 *
 *   ExImClearingAgentBill.GenerateCode (BLL 0114 :343)   Sp_ExImClearingAgentBill_GetAll @OrganizationId,@CompanyId,@DocTypeId,@FinancialYearId,'GenerateDocNoCompanyIdOrganizationId'
 *   ExImClearingAgentBill.GetByID (:157, DAL 0106 GetDate) Sp_ExImClearingAgentBill_GetAll @Id,'ReadById' + @Id,'ReadDetailByHeaderId'
 *   ExImClearingAgentBill.FormHistoryDirect (:209)       Sp_ExImClearingAgentBill_GetAll ...,'FormHistory'
 *   ExImClearingAgentBill.DeleteByID (:416)              Sp_ExImClearingAgentBill_GetAll @DeleteUserId,@Id,'DeleteById'
 *   ExImInvoice.GetInvoiceInformationByInvoiceIdForServices (BLL 0467 :1849)  Sp_ExImInvoice_GetAllMethod @OrganizationId,@CompanyId,@Id,'GetInvoiceInformationByInvoiceIdForServices'
 *   CommonServies.GetSupplierCustomerListForFinancialEffects  Sp_SupplierCustomer_GetAllMethod 'GetGlAccountIdandCompanyNameBySupplierCustomerId'
 *   CommonServies.GetItemListForFinancialEffects         Sp_Item_GetAllMethod 'GetItemGlIdsandItemName'
 *   ExImClearingAgentBill.Save -> DAL SetDate (0106)     Sp_ExImClearingAgentBill_Insert|Update, Sp_ExImClearingAgentBillDetail_Insert,
 *                                                        Sp_VoucherHead_Insert|Update, Sp_VoucherDetail_Insert, Sp_VoucherHead_H_Insert,
 *                                                        Sp_VoucherDetail_H_Insert - one transaction.
 */
@Repository
public class LgsAServiceBillRepository {

    public static final String PROC = "Sp_ExImClearingAgentBill_GetAll";

    private final HrmProcRepository db;

    public LgsAServiceBillRepository(HrmProcRepository db) { this.db = db; }

    public int generateCode(UserAccount u, int docTypeId, int yearId) {
        List<Map<String, Object>> r = db.rows(PROC, "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocTypeId", docTypeId, "FinancialYearId", yearId, "Activity", "GenerateDocNoCompanyIdOrganizationId");
        return r.isEmpty() ? 0 : toInt(r.get(0).get("DocNo"));
    }

    public List<Map<String, Object>> header(int id) { return db.rows(PROC, "Id", id, "Activity", "ReadById"); }

    public List<Map<String, Object>> detail(int id) { return db.rows(PROC, "Id", id, "Activity", "ReadDetailByHeaderId"); }

    public List<Map<String, Object>> invoiceData(UserAccount u, int invoiceId) {
        return db.rows("Sp_ExImInvoice_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "Id", invoiceId, "Activity", "GetInvoiceInformationByInvoiceIdForServices");
    }

    /**
     * FormHistoryDirect(ReportsParameters): @OrganizationId, @CompanyId, @DocTypeId, @CanViewAllRecord, @EntryUser when
     * !CanViewAllRecord, @FinancialYearId when != 0, the dates the form set (null = not sent), @DocNoFrom / @DocNoTo when
     * != 0, @Activity 'FormHistory'.
     */
    public List<Map<String, Object>> history(UserAccount u, int docTypeId, boolean canViewAll, int yearId, Map<String, Object> dates,
                                             int docFrom, int docTo) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", u.getOrganizationId());
        p.put("CompanyId", u.getCompanyId());
        p.put("DocTypeId", docTypeId);
        p.put("CanViewAllRecord", canViewAll);
        if (!canViewAll) p.put("EntryUser", u.getId());
        if (yearId != 0) p.put("FinancialYearId", yearId);
        p.putAll(dates);
        if (docFrom != 0) p.put("DocNoFrom", docFrom);
        if (docTo != 0) p.put("DocNoTo", docTo);
        p.put("Activity", "FormHistory");
        return db.rows(PROC, p);
    }

    /** BLL DeleteByID: SqlCommand ExecuteNonQuery in its own transaction. */
    public void delete(int userId, int id) {
        db.tx(() -> db.rows(PROC, "DeleteUserId", userId, "Id", id, "Activity", "DeleteById"));
    }

    public List<Map<String, Object>> partyGlAccounts(UserAccount u) {
        return db.rows("Sp_SupplierCustomer_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "Activity", "GetGlAccountIdandCompanyNameBySupplierCustomerId");
    }

    public List<Map<String, Object>> itemGlAccounts(UserAccount u) {
        return db.rows("Sp_Item_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "Activity", "GetItemGlIdsandItemName");
    }

    /** DAL ExImClearingAgentBill.SetDate(obj, ProcName) step for step (attachments are not ported: the lists are empty). */
    public int save(LgsAClearingAgentBill obj, List<LgsAClearingAgentBillDetail> details, LgsAVoucherHead vh, List<LgsAVoucherDetail> vds) {
        return db.tx(() -> {
            String proc = obj.Id == 0 ? "Sp_ExImClearingAgentBill_Insert" : "Sp_ExImClearingAgentBill_Update";
            int num3 = db.set(proc, obj);
            if (num3 > 0) obj.Id = num3; else num3 = obj.Id;
            for (LgsAClearingAgentBillDetail d : details) {
                d.BillHeaderId = obj.Id;
                db.set("Sp_ExImClearingAgentBillDetail_Insert", d);
            }
            // TaxDetail / OtherChargesDetail are never filled by this form.
            int existing = voucherHeadId(obj.OrganizationId, obj.CompanyId, obj.DocTypeId, obj.Id);
            int num2;
            vh.DocumentTypeSrNo = obj.Id;
            vh.RefDocNoId = obj.Id;
            if (existing == 0) {
                num2 = db.set("Sp_VoucherHead_Insert", vh);
            } else {
                vh.Id = existing;
                num2 = db.set("Sp_VoucherHead_Update", vh);
            }
            if (num2 > 0) vh.Id = num2;
            if (vds == null || vds.isEmpty()) throw new IllegalStateException("VoucherDetail List Not Found");
            for (LgsAVoucherDetail d : vds) {
                d.VoucherHeadId = vh.Id;
                db.set("Sp_VoucherDetail_Insert", d);
            }
            int documentTypeIdRef = db.set("Sp_VoucherHead_H_Insert", vh);
            for (LgsAVoucherDetail d : vds) {
                d.VoucherHeadId = vh.Id;
                d.DocumentTypeIdRef = documentTypeIdRef;
                db.set("Sp_VoucherDetail_H_Insert", d);
            }
            return num3;
        });
    }

    private int voucherHeadId(int org, int comp, int docType, int id) {
        List<Map<String, Object>> r = db.rows("Sp_Vouchers_GetMethods", "Activity", "GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId",
                "OrganizationId", org, "CompanyId", comp, "DocumentTypeId", docType, "DocumentTypeSrNo", id);
        return r.isEmpty() ? 0 : toInt(r.get(0).get("Id"));
    }

}
