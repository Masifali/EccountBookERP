package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.dto.SaleDriverBioRequest;
import com.mst.repositories.InwardGatePassRepository;
import com.mst.repositories.SaleDriverBioRepository;
import com.mst.security.CurrentUserContext;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.*;
import org.springframework.http.HttpStatus;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import org.springframework.web.server.ResponseStatusException;

/**
 * frmDriverBio (Architecture.WinApp.Sale/frmDriverBio.cs) opened by InwardGatePass with RefDocumentTypeId = 51
 * (BtnDriverForm_Click :5321, btnsave_Click :3585). frmDriverBio_Load turns RefDocumentTypeId 51 into ScreenName
 * "frmDriverBioForInWard", and every place that used 91 uses RefDocumentTypeId instead: the pending gate-pass loader,
 * the GP-number lookup, the history, the saved RefDocumentTypeId and ScreenName, and the rights object. The same
 * repository serves /sale/driver-bio (91) through its original methods, which are unchanged.
 */
@Service
public class DriverBioInwardService {
    public static final int REF_DOCUMENT_TYPE_ID = 51;
    public static final String SCREEN_NAME = "frmDriverBioForInWard";

    private final SaleDriverBioRepository repository;
    private final InwardGatePassRepository gatePasses;
    private final CurrentUserContext context;

    public DriverBioInwardService(SaleDriverBioRepository repository, InwardGatePassRepository gatePasses, CurrentUserContext context) {
        this.repository = repository; this.gatePasses = gatePasses; this.context = context;
    }

    private boolean right(UserAccount u, String name) { return repository.hasRight(u, context.currentRoleName(), SCREEN_NAME, name); }

    /** InwardGatePass: "You Don't Have View-right Of Of This Driver-Bio For Inward.." when the View right is missing. */
    private UserAccount viewer() {
        UserAccount u = context.requireAccountingUser();
        if (!right(u, "View")) throw new AccessDeniedException("You Don't Have View-right Of Of This Driver-Bio For Inward..");
        return u;
    }

    /** frmDriverBio_Load: rights, pendingGatepass(), DefaultDaysToLessFromHistoryFromDate, known drivers (globalAllDriverBioInfo). */
    public Map<String, Object> initial() {
        UserAccount u = viewer();
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("documentTypeId", SaleDriverBioRepository.DOCUMENT_TYPE_ID);
        m.put("refDocumentTypeId", REF_DOCUMENT_TYPE_ID);
        m.put("screenName", SCREEN_NAME);
        m.put("canSave", right(u, "Save"));
        m.put("canUpdate", right(u, "Update"));
        m.put("canPrint", right(u, "Print"));
        m.put("pendingGatePasses", repository.pending(u, context.currentFinancialYearId(), REF_DOCUMENT_TYPE_ID));
        m.put("knownDrivers", repository.knownDrivers(u));
        String days = gatePasses.config(u.getOrganizationId(), u.getCompanyId(), "DefaultDaysToLessFromHistoryFromDate");
        int d = days.matches("\\d+") ? Integer.parseInt(days) : 0;
        m.put("historyDays", d > 0 ? d : 3);
        return m;
    }

    /** btnRefresh_Click / FormReset: pendingGatepass() and driverBiodata.ReadAll again. */
    public Map<String, Object> refresh() {
        UserAccount u = viewer();
        return Map.of("pendingGatePasses", repository.pending(u, context.currentFinancialYearId(), REF_DOCUMENT_TYPE_ID),
                "knownDrivers", repository.knownDrivers(u));
    }

    /** cmbGatePass_TextChanged: ReadByGpNoForExportDriverInformation with DocumentTypeId 51 and the GP number text. */
    public List<Map<String, Object>> gatePassHeader(int gpSrNo) {
        UserAccount u = viewer();
        return repository.gatePassByNo(u, context.currentFinancialYearId(), REF_DOCUMENT_TYPE_ID, gpSrNo);
    }

    /** HistoryGridFill() */
    public List<Map<String, Object>> history(String dateField, String from, String to, int fromDoc, int toDoc) {
        UserAccount u = viewer();
        return repository.history(u, context.currentFinancialYearId(), REF_DOCUMENT_TYPE_ID, right(u, "CanView AllRecord"),
                dateField == null || dateField.isBlank() ? "docDate" : dateField, date(from), date(to), fromDoc, toDoc);
    }

    /** ReadById(Id): GatePassOutwardDriverInfo.GetByID; rows of another company / another reference type are refused. */
    public Map<String, Object> record(int id) {
        UserAccount u = viewer();
        Map<String, Object> row = repository.record(u, id);
        if (!String.valueOf(REF_DOCUMENT_TYPE_ID).equals(Objects.toString(row.get("RefDocumentTypeId"), "")))
            throw new ResponseStatusException(HttpStatus.NOT_FOUND, "Driver information not found for Inward Gate Pass");
        return row;
    }

    /** btnsave_Click / btnUpdate_Click -> Insert(): FormValidation (verbatim), confirmation is asked by the page. */
    @Transactional
    public Map<String, Object> save(SaleDriverBioRequest request, String gpDate) {
        UserAccount u = viewer();
        boolean updating = request.id > 0;
        if (!right(u, updating ? "Update" : "Save"))
            throw new AccessDeniedException("You do not have " + (updating ? "Update" : "Save") + " permission for Driver Bio (Inward)");
        String cnic = text(request.cnicNo);
        if (cnic.equals("-       -") || cnic.length() < 15 || !cnic.matches("\\d{5}-\\d{7}-\\d")) throw new IllegalArgumentException("CNIC Field Required");
        if (!text(request.driverCellNo).matches("\\d{4}-\\d{3}-\\d{7}")) throw new IllegalArgumentException("Cell No. Field Required");
        if (text(request.driverName).isEmpty()) throw new IllegalArgumentException("Driver Name  Field Required");
        if (text(request.fatherName).isEmpty()) throw new IllegalArgumentException("Father Name  Field Required");
        if (text(request.forwarderName).isEmpty()) throw new IllegalArgumentException("Forwarder Name Field Required");
        // cmbGatePass.Value may be empty on the desktop (GatePassOutwardId = 0 is saved); a chosen one must be this company's inward pass.
        if (request.gatePassOutwardId > 0 && repository.inwardGatePass(u, request.gatePassOutwardId) == null)
            throw new IllegalArgumentException("Select a valid pending inward gate pass");
        LocalDateTime pickerValue = gpDate == null || gpDate.isBlank() ? LocalDateTime.now()
                : (gpDate.length() > 10 ? LocalDateTime.parse(gpDate.substring(0, 19)) : LocalDate.parse(gpDate).atTime(java.time.LocalTime.now()));
        int id = repository.saveForReference(u, request, REF_DOCUMENT_TYPE_ID, SCREEN_NAME, pickerValue);
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", true);
        m.put("id", id);
        m.put("message", (updating ? "Data Update Successfully....  " : "Data Save Successfully....  ") + id);
        return m;
    }

    private static LocalDate date(String v) { return v == null || v.isBlank() ? null : LocalDate.parse(v.substring(0, 10)); }
    private static String text(String v) { return v == null ? "" : v.trim(); }
}
