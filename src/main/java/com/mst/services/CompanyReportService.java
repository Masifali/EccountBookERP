package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.CompanyReportRepository;
import com.mst.security.CurrentUserContext;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * "Report Allocate to Company" (frmCompanyReport, 42,657 bytes).
 *
 * ---------------------------------------------------------------------------------------------
 * THE GATE
 * ---------------------------------------------------------------------------------------------
 * frmCompanyReport has no ScreenDefinition row and checks no rights itself. It is reachable only
 * from Define Reports, which DashboardNew draws for RoleName "Admin" (DashboardNew.cs:1193). The
 * same comparison is applied server-side here, because a URL is reachable whether or not a link
 * was drawn.
 *
 * ---------------------------------------------------------------------------------------------
 * THE ONLY VALIDATION THE DESKTOP HAS
 * ---------------------------------------------------------------------------------------------
 * Insert() checks exactly one thing: grd.GetCheckedRows().Count() > 0, else
 * "Please Check the Reports to save". There is no check on Company and none on Report Header, so
 * the desktop will happily write CompanyId 0 and ReportHeaderId 0. Reproduced as found - adding
 * a requirement here would make the web refuse rows the desktop accepts.
 *
 * ---------------------------------------------------------------------------------------------
 * WARNING - UPDATE WRITES EVERY CHECKED ROW ONTO THE SAME RECORD
 * ---------------------------------------------------------------------------------------------
 * Insert() loops the checked rows and sends CompanyReportId = RecId on EVERY iteration:
 *
 *     foreach (GridEXRow r in checkedRows)
 *         CompanyReport.Save(new CompanyReport {
 *             CompanyReportId = RecId,                      // <- constant across the loop
 *             ReportConfigId  = r.Cells["ReportConfigId"]   // <- varies
 *         });
 *
 * btnsave_Click sets RecId = 0 first, so Save inserts one row per checked report: correct.
 * btnupdate_Click does NOT reset RecId, so with N reports checked the SAME row is updated N
 * times and keeps only the LAST one's ReportConfigId. The other N-1 allocations are silently
 * discarded.
 *
 * This is reproduced, because both sides must write the same row. The one addition is that the
 * page WARNS before an update with more than one report checked. A warning refuses nothing and
 * changes nothing that is written; it only stops the loss being silent.
 */
@Service
public class CompanyReportService {

    private final CurrentUserContext context;
    private final CompanyReportRepository repo;

    public CompanyReportService(CurrentUserContext context, CompanyReportRepository repo) {
        this.context = context;
        this.repo = repo;
    }

    public boolean canOpen() {
        try { return "Admin".equals(context.currentRoleName()); }
        catch (RuntimeException e) { return false; }
    }

    private UserAccount admin() {
        UserAccount u = context.requireAccountingUser();
        if (!"Admin".equals(context.currentRoleName())) {
            throw new AccessDeniedException("Report Allocate to Company is reached from the desktop's "
                    + "Admin Panel, which is shown only to the Admin role.");
        }
        return u;
    }

    // ================================================================================== read

    /** frmCompanyReport_Load: grdFill(), CompanyFill(), ReportHeaderFill() in that order. */
    public Map<String, Object> load() {
        admin();
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("reports",   repo.reportsForSelection());
        m.put("companies", repo.companies());
        m.put("headers",   repo.reportHeaders());
        return m;
    }

    /** The History tab. */
    public List<Map<String, Object>> history() {
        admin();
        return repo.all();
    }

    /**
     * RetrivedData(Id) - grdhistory_DoubleClick.
     *
     * NOTE what the desktop does NOT do here: it sets RecId, switches to the Form tab and swaps
     * Save for Update, and fills NOTHING. cmbCompany, cmbReportHeader and the grid's check marks
     * are all left exactly as they were. The row is returned so the page can show which record is
     * loaded, but no field is populated from it, because the desktop populates none.
     */
    public Map<String, Object> byId(int id) {
        admin();
        Map<String, Object> r = repo.byId(id);
        if (r == null) throw new IllegalArgumentException("Company report not found.");
        return r;
    }

    // ================================================================================== write

    /** Insert(). One Save per checked report, with CompanyReportId constant across the loop. */
    @Transactional
    public Map<String, Object> save(Map<String, Object> form) {
        admin();

        int recId = CompanyReportRepository.intOf(form.get("id"));
        if (recId > 0 && repo.byId(recId) == null) {
            throw new IllegalArgumentException("Company report not found.");
        }

        List<Integer> reportConfigIds = new ArrayList<>();
        Object raw = form.get("reportConfigIds");
        if (raw instanceof List) {
            for (Object o : (List<?>) raw) {
                int id = CompanyReportRepository.intOf(o);
                if (id > 0) reportConfigIds.add(id);
            }
        }
        /* grd.GetCheckedRows().Count() > 0 - the desktop's only refusal. */
        if (reportConfigIds.isEmpty()) {
            throw new IllegalArgumentException("Please Check the Reports to save");
        }

        int companyId      = CompanyReportRepository.intOf(form.get("companyId"));
        int reportHeaderId = CompanyReportRepository.intOf(form.get("reportHeaderId"));

        int lastId = recId;
        for (Integer reportConfigId : reportConfigIds) {
            Map<String, Object> m = CompanyReportRepository.blankModel();
            m.put("CompanyReportId", recId);          // constant across the loop, as the desktop sends it
            m.put("CompanyId",       companyId);
            m.put("ReportConfigId",  reportConfigId);
            m.put("ReportHeaderId",  reportHeaderId);
            lastId = repo.save(m);
        }

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("id", lastId);
        out.put("written", reportConfigIds.size());
        out.put("message", recId > 0 ? "Update Successfully" : "Save Successfully");
        if (recId > 0 && reportConfigIds.size() > 1) {
            out.put("warning", "The desktop sends the same CompanyReportId for every checked report on "
                    + "an update, so record " + recId + " was written " + reportConfigIds.size()
                    + " times and kept only the last one. This is the desktop's behaviour, reproduced.");
        }
        out.put("rows", repo.all());
        return out;
    }
}
