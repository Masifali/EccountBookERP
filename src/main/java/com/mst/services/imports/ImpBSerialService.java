package com.mst.services.imports;

import com.mst.models.UserAccount;
import com.mst.models.imports.ImpBMasterDocumentSerial;
import com.mst.repositories.imports.ImpBRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;
import java.util.Map;

import static com.mst.services.hrm.HrmSupport.*;
import static com.mst.services.imports.ImpBSupport.*;

/**
 * The two "Define ... No" dialogs the Proforma Invoice (782) and Import Invoice (783) forms open:
 *
 *   frmProformaDocumentSerial  - Proforma No + Date        (BtnDefinePerformaNo on 782 and 783)
 *   frmMasterDocumentSerial    - LcOrder No/Date + Invoice No/Date (BtnDefineInvoiceNo / BtnDefineContractNo on 783)
 *
 * Both write ImEx.masterDocumentSerial through BLL masterDocumentSerial.Save (@Activity "Insert" / "UPDATE",
 * AppActionId 1 / 2) and list with masterDocumentSerial.SEARCHHistory. The dialogs check no right of their own;
 * here the calling screen's View right and its Save / Update right are required (screenId = 782 or 783).
 */
@Service
public class ImpBSerialService {

    @Autowired private ImpBRepository repo;
    @Autowired private ImpBSupport sup;

    private static int screen(int screenId) {
        if (screenId != ImpBProformaService.SCREEN && screenId != ImpBImportInvoiceService.SCREEN) throw invalid("Record not found.");
        return screenId;
    }

    /** frmProformaDocumentSerial.BindHistory: SEARCHHistory with ActionId 1 (@IsPerforma 1). */
    public List<Map<String, Object>> proformaSerials(int screenId) {
        UserAccount u = sup.user(screen(screenId));
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.serialSearch(u, 1)) {
            out.add(map("Id", r.get("MasterDocumentSerialId"), "RefDocumentTypeId", r.get("RefDocumentTypeId"), "ProformaMasterId", r.get("ProformaMasterId"),
                    "ProformaNo", r.get("ProformaNo"), "ProformaDate", r.get("ProformaDate"), "CreatedUserName", r.get("CreatedUserName"),
                    "CreatedOn", r.get("CreatedOn"), "LastModifyUserName", r.get("LastModifyUserName"), "LastModifiedOn", r.get("LastModifiedOn"),
                    "RowVersionLong", r.get("RowVersionLong")));
        }
        return out;
    }

    /** frmMasterDocumentSerial.BindHistory: SEARCHHistory without ActionId (@IsPerforma not sent -> rows without a ProformaMasterId). */
    public List<Map<String, Object>> masterSerials(int screenId) {
        UserAccount u = sup.user(screen(screenId));
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.serialSearch(u, 0)) {
            out.add(map("Id", r.get("MasterDocumentSerialId"), "RefDocumentTypeId", r.get("RefDocumentTypeId"), "LocOrderMasterId", r.get("LocOrderMasterId"),
                    "LocOrderNo", r.get("LocOrderNo"), "LocOrderDate", r.get("LocOrderDate"), "InvoiceMasterId", r.get("InvoiceMasterId"),
                    "InvoiceDate", r.get("InvoiceDate"), "InvoiceNo", r.get("InvoiceNo"), "CreatedUserName", r.get("CreatedUserName"),
                    "CreatedOn", r.get("CreatedOn"), "LastModifyUserName", r.get("LastModifyUserName"), "LastModifiedOn", r.get("LastModifiedOn"),
                    "RowVersionLong", r.get("RowVersionLong")));
        }
        return out;
    }

    private ImpBMasterDocumentSerial base(UserAccount u, long id, LocalDateTime now) {
        ImpBMasterDocumentSerial m = new ImpBMasterDocumentSerial();
        m.masterDocumentSerialId = id;
        m.CompanyId = u.getCompanyId();
        m.OrganizationId = u.getOrganizationId();
        m.BranchesId = branchId(u);
        m.locationBranchId = branchId(u);
        m.createdUserId = u.getId(); m.approvedUserId = u.getId(); m.lastModifiedUserId = u.getId();
        m.createdOn = now; m.lastModifiedOn = now; m.approvedOn = now;
        m.AppActionId = id <= 0 ? 1 : 2;                              // BLL FillEntity
        return m;
    }

    private Map<String, Object> write(ImpBMasterDocumentSerial m, long id) {
        repo.tx(() -> repo.set(ImpBRepository.IMEX + "[usp_Set_masterDocumentSerial]", m, id <= 0 ? "Insert" : "UPDATE"));
        return saved((int) id, id == 0 ? "Save Successfully" : "Update Successfully");
    }

    /** frmProformaDocumentSerial.Insert(): "ProformaNo No Is Required"; proformaNo / proformaDate, locOrderDate = invoiceDate = now. */
    public Map<String, Object> saveProformaSerial(int screenId, Map<String, Object> b) {
        UserAccount u = sup.user(screen(screenId));
        long id = Math.max(0, toLong(b.get("id")));
        sup.require(u, screenId, id > 0 ? "Update" : "Save");
        String no = str(b.get("proformaNo"));
        if (no.isEmpty() || no.equals("0")) throw invalid("ProformaNo No Is Required");
        if (id > 0 && !has(repo.serialSearch(u, 1), "masterDocumentSerialId", id)) throw invalid("Record not found.");
        LocalDateTime now = LocalDateTime.now();
        ImpBMasterDocumentSerial m = base(u, id, now);
        m.proformaNo = no;
        m.proformaDate = nvl(toDate(b.get("proformaDate")), now);
        m.locOrderDate = now;
        m.invoiceDate = now;
        return write(m, id);
    }

    /**
     * frmMasterDocumentSerial.Insert(): "LcOrder No Is Required", "InvoiceNo Field Is Required",
     * "InvoiceDate Can  not Less than Lc Order Date Field Is Required" (InvoiceDate > LcOrderDate); proformaDate = now.
     */
    public Map<String, Object> saveMasterSerial(int screenId, Map<String, Object> b) {
        UserAccount u = sup.user(screen(screenId));
        long id = Math.max(0, toLong(b.get("id")));
        sup.require(u, screenId, id > 0 ? "Update" : "Save");
        String lc = str(b.get("locOrderNo")), inv = str(b.get("invoiceNo"));
        if (lc.isEmpty() || lc.equals("0")) throw invalid("LcOrder No Is Required");
        if (inv.isEmpty() || inv.equals("0")) throw invalid("InvoiceNo Field Is Required");
        LocalDateTime now = LocalDateTime.now();
        LocalDateTime invDate = nvl(toDate(b.get("invoiceDate")), now), lcDate = nvl(toDate(b.get("locOrderDate")), now);
        if (invDate.isAfter(lcDate)) throw invalid("InvoiceDate Can  not Less than Lc Order Date Field Is Required");
        if (id > 0 && !has(repo.serialSearch(u, 0), "masterDocumentSerialId", id)) throw invalid("Record not found.");
        ImpBMasterDocumentSerial m = base(u, id, now);
        m.invoiceNo = inv;
        m.invoiceDate = invDate;
        m.locOrderNo = lc;
        m.locOrderDate = lcDate;
        m.proformaDate = now;
        return write(m, id);
    }

    private static LocalDateTime nvl(LocalDateTime a, LocalDateTime b) { return a == null ? b : a; }
}
