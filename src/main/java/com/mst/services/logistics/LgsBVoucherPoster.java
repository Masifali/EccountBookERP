package com.mst.services.logistics;

import com.mst.models.logistics.LgsBVoucherDetail;
import com.mst.models.logistics.LgsBVoucherHead;
import com.mst.repositories.logistics.LgsBRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Component;

import java.util.List;

/**
 * The accounting-voucher tail shared by DAL lgstcm.ServicesBillHeader.SetData (0345) and
 * DAL lgstcm.FreightVoucherOutward.SetData (0344), run inside the caller's transaction:
 *
 *   Sp_Vouchers_GetMethods 'GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId' (DocumentTypeId, DocumentTypeSrNo)
 *   0 -> Sp_VoucherHead_Insert, else Sp_VoucherHead_Update (DocumentTypeSrNo = RefDocNoId = the document id)
 *   "VoucherDetail List Not Found" when there are no lines
 *   Sp_VoucherDetail_Insert per line (VoucherHeadId set)
 *   [0344 only] USP_VoucherBalanceCheck @OrganizationId, @CompanyId, @Id
 *   Sp_VoucherHead_H_Insert, then Sp_VoucherDetail_H_Insert per line with DocumentTypeIdRef = its result
 */
@Component
public class LgsBVoucherPoster {

    @Autowired private LgsBRepository repo;

    /** BLL MakeVoucher's result: the VoucherHead and its voucherDetailList. */
    public static final class Voucher {
        public final LgsBVoucherHead head;
        public final List<LgsBVoucherDetail> lines;
        public Voucher(LgsBVoucherHead head, List<LgsBVoucherDetail> lines) { this.head = head; this.lines = lines; }
    }

    /** Must be called inside LgsBRepository.tx. The line list is already complete; per-line fields are set by the caller. */
    public int post(com.mst.models.UserAccount u, LgsBVoucherHead vh, List<LgsBVoucherDetail> lines, int documentId, boolean balanceCheck) {
        int existing = repo.voucherHeadId(u, vh.DocumentTypeId, documentId);
        if (existing != 0) vh.Id = existing;
        vh.DocumentTypeSrNo = documentId;
        vh.RefDocNoId = documentId;
        int n = repo.set(existing == 0 ? "Sp_VoucherHead_Insert" : "Sp_VoucherHead_Update", vh);
        if (n > 0) vh.Id = n;
        if (lines == null || lines.isEmpty()) throw new IllegalArgumentException("VoucherDetail List Not Found");
        for (LgsBVoucherDetail l : lines) {
            l.VoucherHeadId = vh.Id;
            repo.set("Sp_VoucherDetail_Insert", l);
        }
        if (balanceCheck) {
            repo.exec("USP_VoucherBalanceCheck", LgsBRepository.p("OrganizationId", vh.OrganizationId, "CompanyId", vh.CompanyId, "Id", vh.Id));
        }
        int documentTypeIdRef = repo.set("Sp_VoucherHead_H_Insert", vh);
        for (LgsBVoucherDetail l : lines) {
            l.VoucherHeadId = vh.Id;
            l.DocumentTypeIdRef = documentTypeIdRef;
            repo.set("Sp_VoucherDetail_H_Insert", l);
        }
        return vh.Id;
    }
}
