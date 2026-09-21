package com.mst.controllers;

import com.mst.services.CompanyProfileService;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.ResponseBody;

import javax.servlet.http.HttpSession;
import java.util.LinkedHashMap;
import java.util.Map;
import java.util.UUID;

/**
 * Admin Panel -> "Company Profile" (frmCompanyProfile).
 *
 * The route sits under /configurations because that is the desktop namespace the form lives in
 * (Architecture.WinApp.Configurations), and because the gear menu links to it by path.
 */
@Controller
public class CompanyProfileController {

    private static final String TOKEN = "companyProfileCsrf";

    private final CompanyProfileService service;
    public CompanyProfileController(CompanyProfileService service) { this.service = service; }

    @GetMapping("/configurations/company-profile")
    public String page(Model model) {
        model.addAttribute("canOpen", service.canOpen());
        return "configurations/company_profile";
    }

    /**
     * OrganizationFill() + LocationsBind().
     *
     * Also mints the per-session token the save below requires. Spring Security's own CSRF
     * protection is disabled application-wide here, so a state-changing admin endpoint would
     * otherwise accept a cross-site POST made with the operator's cookie. DesktopUserRightsController
     * already solves this the same way for the rights screen; the same pattern is used rather than
     * a new one. It changes nothing about what the desktop does - it only makes the web request
     * prove it came from this page.
     */
    @GetMapping("/configurations/company-profile/api/load")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> load(HttpSession session) {
        try {
            Map<String, Object> body = new LinkedHashMap<>(service.load());
            synchronized (session) {
                if (session.getAttribute(TOKEN) == null) {
                    session.setAttribute(TOKEN, UUID.randomUUID().toString());
                }
                body.put("token", session.getAttribute(TOKEN));
            }
            return ResponseEntity.ok(body);
        } catch (AccessDeniedException e) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(e.getMessage()));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(e)));
        }
    }

    /**
     * Insert() - Save when id is absent or 0, Update when it names a row of the grid.
     *
     * 400 carries a validation refusal in the desktop's own wording. 500 carries whatever the
     * procedure raised - including Proc_Company_Insert's licensed-company-count error - passed
     * through rather than replaced.
     */
    @PostMapping("/configurations/company-profile/api/save")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> save(@RequestBody Map<String, Object> form,
                                                    HttpSession session) {
        try {
            Object expected = session.getAttribute(TOKEN);
            Object supplied = form.get("token");
            if (expected == null || !expected.equals(supplied)) {
                return ResponseEntity.status(HttpStatus.FORBIDDEN)
                        .body(fail("This page has expired. Refresh it and try again."));
            }
            return ResponseEntity.ok(service.save(form));
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(e.getMessage()));
        } catch (AccessDeniedException e) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(e.getMessage()));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(msg(e)));
        }
    }

    private static String msg(Exception e) {
        String m = e.getMessage();
        return (m == null || m.trim().isEmpty()) ? "Save failed." : m;
    }

    private static Map<String, Object> fail(String message) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        m.put("message", message);
        return m;
    }
}
