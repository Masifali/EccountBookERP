package com.mst.controllers;

import com.mst.services.AdmPanelService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

import java.util.LinkedHashMap;
import java.util.Map;
import java.util.function.Supplier;

/**
 * Admin Panel (module 22) screens that were not on the web:
 *
 *   /admin/define-company                              312 Templates.DefineCompany
 *   /admin/allocation-organization-template-rights     314 Templates.AllocationOrganizationTemplateRights
 *   /admin/module-allocate-to-templates                315 Templates.frmModuleAllocateToOrganizationTemplates
 *   /admin/screens-allocate-to-company                 316 Templates.frmScreensAllocateToCompany
 *   /admin/user-rights-management                      318 and 397 UserRightsManagement.UserRightsEditing
 *   /admin/user-module-admin                           319 Configurations.frmUserModuleAdmin
 *   /admin/screen-definition                           791 frmScreenDefinetion
 *
 * API under /api/adm-panel. Every call is gated on RoleName == "Admin" in AdmPanelService.admin().
 */
@Controller
public class AdmPanelController {

    private static final String API = "/api/adm-panel";

    @Autowired private AdmPanelService service;

    // ------------------------------------------------------------------ pages

    @GetMapping("/admin/define-company")
    public String defineCompany(Model model) { return page(model, "adm_panel/define_company"); }

    @GetMapping("/admin/allocation-organization-template-rights")
    public String templateRights(Model model) { return page(model, "adm_panel/template_rights"); }

    @GetMapping("/admin/module-allocate-to-templates")
    public String moduleAllocate(Model model) { return page(model, "adm_panel/module_allocate_templates"); }

    @GetMapping("/admin/screens-allocate-to-company")
    public String screensAllocate(Model model) { return page(model, "adm_panel/screens_allocate_company"); }

    @GetMapping("/admin/user-rights-management")
    public String userRights(Model model) { return page(model, "adm_panel/user_rights_management"); }

    @GetMapping("/admin/user-module-admin")
    public String userModuleAdmin(Model model) { return page(model, "adm_panel/user_module_admin"); }

    @GetMapping("/admin/screen-definition")
    public String screenDefinition(Model model) { return page(model, "adm_panel/screen_definition"); }

    private String page(Model model, String view) {
        service.admin();
        model.addAttribute("activeMenu", "apps");
        return view;
    }

    // ------------------------------------------------------------------ shared

    @GetMapping(API + "/templates") @ResponseBody
    public ResponseEntity<?> templates() { return run(service::templates, "Load failed."); }

    // ------------------------------------------------------------------ 318 UserRightsEditing

    @GetMapping(API + "/ur/setup") @ResponseBody
    public ResponseEntity<?> urSetup() { return run(service::urSetup, "Load failed."); }

    @GetMapping(API + "/ur/companies") @ResponseBody
    public ResponseEntity<?> urCompanies(@RequestParam("userId") int userId) { return run(() -> service.urCompanies(userId), "Load failed."); }

    @GetMapping(API + "/ur/rights") @ResponseBody
    public ResponseEntity<?> urRights(@RequestParam("userId") int userId, @RequestParam("companyId") int companyId) {
        return run(() -> service.urRights(userId, companyId), "Load failed.");
    }

    @PostMapping(API + "/ur/save") @ResponseBody
    public ResponseEntity<?> urSave(@RequestBody Map<String, Object> body) { return run(() -> service.urSave(body), "Save failed."); }

    // ------------------------------------------------------------------ 319 frmUserModuleAdmin

    @GetMapping(API + "/uma/users") @ResponseBody
    public ResponseEntity<?> umaUsers() { return run(service::umaUsersGrid, "Load failed."); }

    @GetMapping(API + "/uma/grids") @ResponseBody
    public ResponseEntity<?> umaGrids(@RequestParam("userId") int userId) { return run(() -> service.umaGrids(userId), "Load failed."); }

    @PostMapping(API + "/uma/save") @ResponseBody
    public ResponseEntity<?> umaSave(@RequestBody Map<String, Object> body) { return run(() -> service.umaSave(body), "Save failed."); }

    // ------------------------------------------------------------------ 315 frmModuleAllocateToOrganizationTemplates

    @GetMapping(API + "/mat/modules") @ResponseBody
    public ResponseEntity<?> matModules(@RequestParam("templateId") int templateId) { return run(() -> service.matModules(templateId), "Load failed."); }

    @GetMapping(API + "/mat/screens") @ResponseBody
    public ResponseEntity<?> matScreens(@RequestParam("templateId") int templateId, @RequestParam(value = "moduleId", defaultValue = "0") int moduleId) {
        return run(() -> service.matScreens(templateId, moduleId), "Load failed.");
    }

    @PostMapping(API + "/mat/save-modules") @ResponseBody
    public ResponseEntity<?> matSaveModules(@RequestBody Map<String, Object> body) { return run(() -> service.matSaveModules(body), "Save failed."); }

    @PostMapping(API + "/mat/save-screens") @ResponseBody
    public ResponseEntity<?> matSaveScreens(@RequestBody Map<String, Object> body) { return run(() -> service.matSaveScreens(body), "Save failed."); }

    // ------------------------------------------------------------------ 316 frmScreensAllocateToCompany

    @GetMapping(API + "/sac/companies") @ResponseBody
    public ResponseEntity<?> sacCompanies(@RequestParam("templateId") int templateId) { return run(() -> service.sacCompanies(templateId), "Load failed."); }

    @GetMapping(API + "/sac/modules") @ResponseBody
    public ResponseEntity<?> sacModules(@RequestParam("templateId") int templateId) { return run(() -> service.sacModules(templateId), "Load failed."); }

    @GetMapping(API + "/sac/screens") @ResponseBody
    public ResponseEntity<?> sacScreens(@RequestParam("templateId") int templateId, @RequestParam("companyId") int companyId,
                                        @RequestParam("moduleId") int moduleId) {
        return run(() -> service.sacScreens(templateId, companyId, moduleId), "Load failed.");
    }

    @PostMapping(API + "/sac/save") @ResponseBody
    public ResponseEntity<?> sacSave(@RequestBody Map<String, Object> body) { return run(() -> service.sacSave(body), "Save failed."); }

    // ------------------------------------------------------------------ 314 AllocationOrganizationTemplateRights

    @GetMapping(API + "/tr/setup") @ResponseBody
    public ResponseEntity<?> trSetup() { return run(service::trSetup, "Load failed."); }

    @GetMapping(API + "/tr/screens") @ResponseBody
    public ResponseEntity<?> trScreens(@RequestParam("templateId") int templateId, @RequestParam(value = "moduleId", defaultValue = "0") int moduleId) {
        return run(() -> service.trScreens(templateId, moduleId), "Load failed.");
    }

    @PostMapping(API + "/tr/allocate") @ResponseBody
    public ResponseEntity<?> trAllocate(@RequestBody Map<String, Object> body) { return run(() -> service.trAllocate(body), "Save failed."); }

    @PostMapping(API + "/tr/deallocate") @ResponseBody
    public ResponseEntity<?> trDeallocate(@RequestBody Map<String, Object> body) { return run(() -> service.trDeallocate(body), "Save failed."); }

    // ------------------------------------------------------------------ 312 DefineCompany

    @GetMapping(API + "/dc/setup") @ResponseBody
    public ResponseEntity<?> dcSetup() { return run(service::dcSetup, "Load failed."); }

    @GetMapping(API + "/dc/history") @ResponseBody
    public ResponseEntity<?> dcHistory() { return run(service::dcHistory, "Load failed."); }

    @GetMapping(API + "/dc/organization") @ResponseBody
    public ResponseEntity<?> dcOrganization(@RequestParam("id") int id) { return run(() -> service.dcOrganization(id), "Load failed."); }

    @GetMapping(API + "/dc/company") @ResponseBody
    public ResponseEntity<?> dcCompany(@RequestParam("id") int id) { return run(() -> service.dcCompany(id), "Load failed."); }

    @GetMapping(API + "/dc/logo")
    public ResponseEntity<?> dcLogo(@RequestParam("id") int id) {
        try {
            byte[] bytes = service.dcLogo(id);
            MediaType type = MediaType.APPLICATION_OCTET_STREAM;
            if (bytes.length > 3 && (bytes[0] & 0xFF) == 0x89 && bytes[1] == 'P') type = MediaType.IMAGE_PNG;
            else if (bytes.length > 2 && (bytes[0] & 0xFF) == 0xFF && (bytes[1] & 0xFF) == 0xD8) type = MediaType.IMAGE_JPEG;
            else if (bytes.length > 2 && bytes[0] == 'G' && bytes[1] == 'I') type = MediaType.IMAGE_GIF;
            return ResponseEntity.ok().contentType(type).header("Cache-Control", "no-store").body(bytes);
        } catch (IllegalArgumentException e) {
            return ResponseEntity.notFound().build();
        } catch (Exception e) {
            return error(e, "Picture could not be read.");
        }
    }

    @PostMapping(API + "/dc/save") @ResponseBody
    public ResponseEntity<?> dcSave(@RequestBody Map<String, Object> body) { return run(() -> service.dcSave(body), "Save failed."); }

    @PostMapping(API + "/dc/update") @ResponseBody
    public ResponseEntity<?> dcUpdate(@RequestBody Map<String, Object> body) { return run(() -> service.dcUpdate(body), "Update failed."); }

    // ------------------------------------------------------------------ 791 frmScreenDefinetion

    @GetMapping(API + "/sd/setup") @ResponseBody
    public ResponseEntity<?> sdSetup() { return run(service::sdSetup, "Load failed."); }

    @GetMapping(API + "/sd/screens") @ResponseBody
    public ResponseEntity<?> sdScreens() { return run(service::sdScreens, "Load failed."); }

    @GetMapping(API + "/sd/by-id") @ResponseBody
    public ResponseEntity<?> sdById(@RequestParam("id") int id) { return run(() -> service.sdById(id), "Load failed."); }

    @PostMapping(API + "/sd/save") @ResponseBody
    public ResponseEntity<?> sdSave(@RequestBody Map<String, Object> body) { return run(() -> service.sdSave(body), "Save failed."); }

    @PostMapping(API + "/sd/sorting") @ResponseBody
    public ResponseEntity<?> sdSorting(@RequestBody Map<String, Object> body) { return run(() -> service.sdSorting(body), "Update failed."); }

    @GetMapping(API + "/sd/document-types") @ResponseBody
    public ResponseEntity<?> sdDocumentTypes() { return run(service::sdDocumentTypes, "Load failed."); }

    @PostMapping(API + "/sd/document-type") @ResponseBody
    public ResponseEntity<?> sdSaveDocType(@RequestBody Map<String, Object> body) { return run(() -> service.sdSaveDocType(body), "Save failed."); }

    @GetMapping(API + "/sd/methods") @ResponseBody
    public ResponseEntity<?> sdMethods() { return run(service::sdMethods, "Load failed."); }

    @PostMapping(API + "/sd/method") @ResponseBody
    public ResponseEntity<?> sdSaveMethod(@RequestBody Map<String, Object> body) { return run(() -> service.sdSaveMethod(body), "Save failed."); }

    // ------------------------------------------------------------------ plumbing

    private static ResponseEntity<?> run(Supplier<Object> call, String fallback) {
        try {
            return ResponseEntity.ok(call.get());
        } catch (IllegalArgumentException | IllegalStateException e) {
            return ResponseEntity.badRequest().body(fail(root(e, fallback)));
        } catch (Exception e) {
            return error(e, fallback);
        }
    }

    /** The desktop shows ex.Message - for a RAISERROR that is the procedure's own text. */
    private static String root(Throwable e, String fallback) {
        Throwable t = e;
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        String m = t.getMessage();
        if (m == null || m.trim().isEmpty()) m = e.getMessage();
        return m == null || m.trim().isEmpty() ? fallback : m;
    }

    private static ResponseEntity<Map<String, Object>> error(Exception e, String fallback) {
        if (e instanceof AccessDeniedException) return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(root(e, "Access denied.")));
        return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(root(e, fallback)));
    }

    private static Map<String, Object> fail(String message) {
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("success", false);
        r.put("message", message);
        return r;
    }
}
