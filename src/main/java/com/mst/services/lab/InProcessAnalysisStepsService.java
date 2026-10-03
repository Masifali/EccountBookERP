package com.mst.services.lab;

import com.mst.repositories.lab.InProcessAnalysisStepsRepository;
import com.mst.security.CurrentUserContext;
import com.mst.services.StoreScreenRights;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.sql.Timestamp;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Screen 163 "InProcess Analysis Steps Schedule" (ModuleId 7) - desktop form
 * Architecture.WinApp.Lab/LabInProcessAnalysisStepAndParameterSchedule.cs (":n" below), BLL
 * 0396_Architecture.BLL.Lab.InvProcessAnalysisStepSchedule.cs; and the helper form its BtnStep opens,
 * Architecture.WinApp/DefineProcessStep.cs ("dps:n"), BLL 0395_Architecture.BLL.Lab.ProcessStep.cs.
 *
 * Neither form has delete, print or a history tab. Both ask "Are you sure to Save?/Update?" AFTER
 * their validations: the save methods answer {confirm:true,message} (HTTP 409 in the controller) until
 * the request carries confirm=true, so the order validation -> prompt -> write is the desktop's.
 *
 * RIGHTS. Neither form contains any rights code; the desktop gate is the menu. The web page is
 * reachable by URL, so the View right of ScreenName "LabInProcessAnalysisStepAndParameterSchedule"
 * (seed_screendef.txt Id 163) is required for every call and Save / Update for the writes.
 * DefineProcessStep has no ScreenDefinition row of its own and is only reachable from this form, so it
 * is governed by the same screen's rights. See the report, "decisions needed".
 */
@Service
public class InProcessAnalysisStepsService {

    private static final org.slf4j.Logger LOG = org.slf4j.LoggerFactory.getLogger(InProcessAnalysisStepsService.class);

    public static final int SCREEN_ID = 163;
    public static final String SCREEN_NAME = "LabInProcessAnalysisStepAndParameterSchedule";

    private final InProcessAnalysisStepsRepository repo;
    private final StoreScreenRights rights;
    private final CurrentUserContext ctx;

    public InProcessAnalysisStepsService(InProcessAnalysisStepsRepository repo, StoreScreenRights rights,
                                         CurrentUserContext ctx) {
        this.repo = repo;
        this.rights = rights;
        this.ctx = ctx;
    }

    // ------------------------------------------------------------------------------ reads

    /** InvLabAnalysisItems_Load (:422): gridfill, BindPlantName, BindProcessStepName, BindParameter, GenerateSortNo. */
    public Map<String, Object> lookups() {
        Map<String, Boolean> r = requireView();
        int org = ctx.currentOrganizationId(), company = ctx.currentCompanyId();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rights", r);
        out.put("screenId", SCREEN_ID);
        List<String> errors = new ArrayList<>();
        out.put("rows", safe("gridfill", errors, () -> repo.getAllOrById(org, company, 0)));
        out.put("plants", safe("BindPlantName", errors, () -> repo.plants(org, company)));
        out.put("steps", safe("BindProcessStepName", errors, () -> repo.processStepNames(org, company, 0)));
        out.put("parameters", safe("BindParameter", errors, () -> repo.analysisItems(org, company, 0)));
        int sortNo = 0;
        try { sortNo = repo.generateSortNo(org, company); }
        catch (org.springframework.dao.DataAccessException e) { safe("GenerateSortNo", errors, () -> { throw e; }); }
        out.put("sortNo", sortNo);
        out.put("errors", errors);
        return out;
    }

    /**
     * One list of the form load. On the desktop every fill (ItemFill, CityFill, GroupFill, BindPlantName ...)
     * has its own try / catch -> MessageBox.Show(ex.Message): a fill that fails leaves ITS combo empty and
     * the other fills still run. Reading all of them in one unguarded call made one failing procedure
     * empty every dropdown of the page. Here a failing list comes back empty and its message is added to
     * "errors", which the page shows one by one.
     */
    private static List<Map<String, Object>> safe(String fill, List<String> errors,
                                                  java.util.function.Supplier<List<Map<String, Object>>> read) {
        try {
            List<Map<String, Object>> rows = read.get();
            return rows == null ? new ArrayList<>() : rows;
        } catch (org.springframework.dao.DataAccessException e) {
            Throwable r = e;
            while (r.getCause() != null && r.getCause() != r) r = r.getCause();
            String text = r.getMessage() == null || r.getMessage().trim().isEmpty() ? "Database Error" : r.getMessage().trim();
            LOG.warn("{} failed: {}", fill, text);
            errors.add(fill + ": " + text);
            return new ArrayList<>();
        }
    }

    /** btnRefresh_Click (:525): BindPlantName, BindProcessStepName, BindParameter - the three lists only. */
    public Map<String, Object> combos() {
        requireView();
        int org = ctx.currentOrganizationId(), company = ctx.currentCompanyId();
        Map<String, Object> out = new LinkedHashMap<>();
        List<String> errors = new ArrayList<>();
        out.put("plants", safe("BindPlantName", errors, () -> repo.plants(org, company)));
        out.put("steps", safe("BindProcessStepName", errors, () -> repo.processStepNames(org, company, 0)));
        out.put("parameters", safe("BindParameter", errors, () -> repo.analysisItems(org, company, 0)));
        out.put("errors", errors);
        return out;
    }

    /** refresh() (:116): gridfill() + GenerateSortNo(). */
    public Map<String, Object> refresh() {
        requireView();
        int org = ctx.currentOrganizationId(), company = ctx.currentCompanyId();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rows", repo.getAllOrById(org, company, 0));
        out.put("sortNo", repo.generateSortNo(org, company));
        return out;
    }

    /** grdfrm_DoubleClick (:393) - GetAllOrById(OrganizationId, CompanyId, Id). */
    public Map<String, Object> byId(int id) {
        requireView();
        if (id == 0) return null;
        List<Map<String, Object>> rows = repo.getAllOrById(ctx.currentOrganizationId(), ctx.currentCompanyId(), id);
        return rows.isEmpty() ? null : rows.get(0);
    }

    // ------------------------------------------------------------------------------ save / update

    /**
     * Insert() (:221) - btnsave_Click sets RecId = 0 first (:285), btnupdate_Click keeps it (:294).
     *
     *  1. formvalidation (:93): the three combos must have an active row.
     *  2. "This Schedule Already Exits In Grid" (:228-242): the combo TEXTS against the grid cells
     *     AnalysisParameterDescription / PlantName / ProcessStepName. Reproduced exactly, including two
     *     desktop facts: (a) the check also runs on Update, so an unchanged row is refused; (b) the Plant
     *     combo displays InvProductionPlant.Description (BindPlantName :160) while the grid's PlantName is
     *     InvProductionPlant.Code (procedure 'ReadAll'), so the check only ever matches where a plant's Code
     *     equals its Description. The desktop compares against the rows currently shown in the grid (a
     *     filter-row filter hides rows from it); the server compares against all rows.
     *  3. "Are you sure to Update?" / "Are you sure to Save?" (:246 / :252).
     *  4. One procedure call. On Insert the procedure replaces @Id and @SortNo with its own MAX+1.
     */
    @Transactional
    public Map<String, Object> save(Map<String, Object> body) {
        Map<String, Boolean> r = requireView();
        int id = toInt(body.get("id"));
        if (id == 0 && !Boolean.TRUE.equals(r.get("save")))
            throw new AccessDeniedException("You do not have the Save right for this screen.");
        if (id != 0 && !Boolean.TRUE.equals(r.get("update")))
            throw new AccessDeniedException("You do not have the Update right for this screen.");

        int org = ctx.currentOrganizationId(), company = ctx.currentCompanyId();
        int plantId = toInt(body.get("plantId"));
        int stepId = toInt(body.get("stepId"));
        int parameterId = toInt(body.get("parameterId"));

        // 1. formvalidation - ActiveRow == null; the active row is a row of the list the combo was bound to.
        String plantText = plantId == 0 ? null : findText(repo.plants(org, company), plantId, "Description");
        if (plantText == null) throw new IllegalArgumentException("Plant Name Field is Required");
        String stepText = stepId == 0 ? null : findText(repo.processStepNames(org, company, stepId), stepId, "ProcessStepName");
        if (stepText == null) throw new IllegalArgumentException("Process Step Field is Required");
        String parameterText = parameterId == 0 ? null
                : findText(repo.analysisItems(org, company, parameterId), parameterId, "AnalysisParameterDescription");
        if (parameterText == null) throw new IllegalArgumentException("Parent Parameter Field is Required");

        // 2. duplicate in grid
        for (Map<String, Object> row : repo.getAllOrById(org, company, 0)) {
            if (parameterText.equals(str(row.get("AnalysisParameterDescription")))
                    && plantText.equals(str(row.get("PlantName")))
                    && stepText.equals(str(row.get("ProcessStepName")))) {
                throw new IllegalArgumentException("This Schedule Already Exits In Grid");
            }
        }

        // 3. Yes/No prompt
        if (!toBool(body.get("confirm"))) {
            return confirm(id > 0 ? "Are you sure to Update?" : "Are you sure to Save?");
        }

        // 4. write
        int sortNo;
        if (id != 0) {
            // Web-only tenancy guard (the Update procedure filters on Id alone). The desktop sends the
            // SortNo it loaded from this same row into txtSortNo (:410); it is read here instead of
            // being taken from the browser.
            List<Map<String, Object>> current = repo.getAllOrById(org, company, id);
            if (current.isEmpty()) throw new AccessDeniedException("This record does not belong to the current company.");
            sortNo = toInt(current.get(0).get("SortNo"));
        } else {
            sortNo = toInt(body.get("sortNo"));          // txtSortNo (:256); the Insert procedure overrides it
        }
        // obj.EnteryUserId = UserAccount.EntryUserId on the desktop (:262) - see the report; the signed-in
        // user's id is written here. obj.EnteryDate = DateTime.Now (:263).
        Integer saved = repo.saveSchedule(id, plantId, stepId, parameterId, sortNo, org, company,
                ctx.currentUserId(), new Timestamp(System.currentTimeMillis()));

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("id", saved);
        out.put("message", id == 0 ? "Save Successfully" : "Update Successfully");      // (:265-272)
        return out;
    }

    // ------------------------------------------------------------------------------ DefineProcessStep

    /** DefineProcessStep_Load (dps:65) / formReFresh (dps:237): bindGrid() + GenerateSortNo(). */
    public Map<String, Object> processSteps() {
        requireView();
        int org = ctx.currentOrganizationId(), company = ctx.currentCompanyId();
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> row : repo.processStepHistory(org, company)) {
            Map<String, Object> m = new LinkedHashMap<>(row);
            Object d = m.get("EntryDate");
            if (d instanceof java.util.Date) m.put("EntryDate", new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss").format((java.util.Date) d));
            rows.add(m);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rows", rows);
        out.put("sortNo", repo.processStepSortNo(org, company));
        return out;
    }

    /** grdProcessStep_DoubleClick (dps:193) - ProcessStep.GetByID(RecId): ProcessStepName, SortNo. */
    public Map<String, Object> processStep(int id) {
        requireView();
        Map<String, Object> row = ownProcessStep(id);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("Id", id);
        out.put("ProcessStepName", row.get("ProcessStepName"));
        out.put("SortNo", row.get("SortNo"));
        return out;
    }

    /**
     * DefineProcessStep.Insert() (dps:104): FormValidation (dps:214) -> Yes/No prompt -> ProcessStep.Save.
     * EntryUserId = UserAccount.ID, EntryDate = DateTime.Now (dps:131-132).
     */
    @Transactional
    public Map<String, Object> saveProcessStep(Map<String, Object> body) {
        Map<String, Boolean> r = requireView();
        int id = toInt(body.get("id"));
        if (id == 0 && !Boolean.TRUE.equals(r.get("save")))
            throw new AccessDeniedException("You do not have the Save right for this screen.");
        if (id != 0 && !Boolean.TRUE.equals(r.get("update")))
            throw new AccessDeniedException("You do not have the Update right for this screen.");

        String name = body.get("processStepName") == null ? "" : String.valueOf(body.get("processStepName")).trim();
        if (name.isEmpty()) throw new IllegalArgumentException("Process Name Field Required");     // dps:218

        if (!toBool(body.get("confirm"))) {
            return confirm(id > 0 ? "Are you sure to Update?" : "Are you sure to Save?");        // dps:117 / dps:123
        }

        int org = ctx.currentOrganizationId(), company = ctx.currentCompanyId();
        int sortNo = id != 0
                ? toInt(ownProcessStep(id).get("SortNo"))      // txtSortNo = obj.SortNo of the loaded row (dps:206)
                : toInt(body.get("sortNo"));                   // txtSortNo (dps:130); the Insert procedure overrides it
        Integer saved = repo.saveProcessStep(id, name, sortNo, org, company,
                ctx.currentUserId(), new Timestamp(System.currentTimeMillis()));

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("id", id != 0 ? Integer.valueOf(id) : saved);
        out.put("message", id > 0 ? "Record Update Successfully..." : "Record Save Successfully...");   // dps:134-141
        return out;
    }

    /**
     * 'ReadById' filters on Id alone; the desktop can only hold an Id from its own company's grid. The
     * row's OrganizationId / CompanyId are therefore checked here (web-only tenancy guard).
     */
    private Map<String, Object> ownProcessStep(int id) {
        List<Map<String, Object>> rows = id == 0 ? new ArrayList<>() : repo.processStepById(id);
        if (rows.isEmpty()
                || toInt(rows.get(0).get("OrganizationId")) != ctx.currentOrganizationId()
                || toInt(rows.get(0).get("CompanyId")) != ctx.currentCompanyId()) {
            throw new AccessDeniedException("This record does not belong to the current company.");
        }
        return rows.get(0);
    }

    // ------------------------------------------------------------------------------ helpers

    private Map<String, Boolean> requireView() {
        ctx.requireAccountingUser();
        Map<String, Boolean> r = rights.of(SCREEN_NAME);
        if (!Boolean.TRUE.equals(r.get("view")))
            throw new AccessDeniedException("You do not have the View right for InProcess Analysis Steps Schedule.");
        return r;
    }

    private static Map<String, Object> confirm(String message) {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("confirm", true);
        out.put("message", message);
        return out;
    }

    /** The display text of the list row whose Id is {@code id}; null when the list has no such row. */
    private static String findText(List<Map<String, Object>> rows, int id, String displayMember) {
        for (Map<String, Object> row : rows) {
            if (toInt(row.get("Id")) == id) return str(row.get(displayMember));
        }
        return null;
    }

    /** Conversion.ToString - null is "". */
    private static String str(Object v) { return v == null ? "" : String.valueOf(v); }

    /** Architecture.Common.Conversion.ToInt - anything that is not a number is 0. */
    static int toInt(Object v) {
        if (v instanceof Number) return ((Number) v).intValue();
        if (v == null) return 0;
        try { return Integer.parseInt(String.valueOf(v).trim()); }
        catch (NumberFormatException e) {
            try { return (int) Math.round(Double.parseDouble(String.valueOf(v).trim())); }
            catch (NumberFormatException e2) { return 0; }
        }
    }

    static boolean toBool(Object v) {
        if (v instanceof Boolean) return (Boolean) v;
        if (v instanceof Number) return ((Number) v).intValue() != 0;
        if (v == null) return false;
        String s = String.valueOf(v).trim();
        return "true".equalsIgnoreCase(s) || "1".equals(s);
    }
}
