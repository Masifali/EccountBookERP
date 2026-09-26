package com.mst.controllers;

import com.mst.models.UserAccount;
import com.mst.repositories.IUserAccountRepository;
import com.mst.security.LoginContext;
import com.mst.security.LoginContextResolver;
import com.mst.security.desktop.DesktopLoginContextService;
import com.mst.security.desktop.DesktopUserAccountDal;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.core.Authentication;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestParam;

import javax.servlet.http.HttpServletRequest;
import java.util.ArrayList;
import java.util.List;
import java.util.Map;

/**
 * The desktop's "Continue" step, as a page: company, branch, financial year and application.
 *
 * Only reached when at least one of them has more than one option - a user allocated to a single
 * company and branch never sees it, because {@code LoginContextSuccessHandler} has already chosen
 * and gone to the dashboard. That is LoginNew.cs's own rule, not a shortcut.
 *
 * Every list here is re-read for the signed-in user on both GET and POST, and the submitted ids
 * are checked against it inside {@link LoginContextResolver#applyChoice}. The page is a
 * convenience; the allocation is the authority.
 */
@Controller
public class LoginContextController {

    @Autowired private DesktopLoginContextService lists;
    @Autowired private LoginContextResolver resolver;
    @Autowired private IUserAccountRepository userAccountRepository;

    /**
     * @param companyId when the operator changes the company, so branch / year / application are
     *                  re-read for it. Validated against the allocation before it is used - a
     *                  company id in the query string is a request, not a fact.
     */
    @GetMapping("/login/context")
    public String page(Authentication authentication, HttpServletRequest request, Model model,
                       @RequestParam(required = false) Integer companyId,
                       @RequestParam(required = false) String message) {
        UserAccount user = requireUser(authentication);
        if (user == null) return "redirect:/login";

        int organizationId = user.getOrganizationId() == null ? 0 : user.getOrganizationId();
        int userId = user.getId() == null ? 0 : user.getId();

        LoginContext current = LoginContext.of(request, authentication.getName());
        if (current != null && current.isComplete()) return "redirect:/dashboard";

        List<Map<String, Object>> companies = lists.companies(organizationId, userId);
        if (companies.isEmpty()) {
            return "redirect:/login?error=true&message=" + encode(LoginContextResolver.NO_COMPANY);
        }

        /* Which company the dependent lists belong to: the one asked for if it is allocated,
           else the one already chosen, else the only one there is. */
        int selected = 0;
        if (companyId != null && companyId > 0
                && lists.isAllocatedCompany(organizationId, userId, companyId)) {
            selected = companyId;
        } else if (current != null && current.getCompanyId() > 0) {
            selected = current.getCompanyId();
        } else {
            selected = DesktopLoginContextService.onlyId(companies, "CompanyId");
        }

        model.addAttribute("companies", companies);
        model.addAttribute("selectedCompanyId", selected);
        model.addAttribute("branches", selected > 0
                ? lists.branches(organizationId, selected, userId) : new ArrayList<>());
        model.addAttribute("financialYears", selected > 0
                ? lists.financialYears(organizationId, selected) : new ArrayList<>());
        model.addAttribute("applications", selected > 0
                ? lists.applications(selected, userId) : new ArrayList<>());
        model.addAttribute("userName", user.getUserName());
        model.addAttribute("message", message);
        return "login_context";
    }

    @PostMapping("/login/context")
    public String choose(Authentication authentication, HttpServletRequest request,
                         @RequestParam(required = false) Integer companyId,
                         @RequestParam(required = false) Integer branchId,
                         @RequestParam(required = false) Integer financialYearId,
                         @RequestParam(required = false) Integer appId) {
        UserAccount user = requireUser(authentication);
        if (user == null) return "redirect:/login";

        LoginContext context;
        try {
            context = resolver.applyChoice(user, companyId, branchId, financialYearId, appId);
        } catch (IllegalArgumentException e) {
            /* Something was not chosen, or was not allocated - stay on the page and say so. */
            return "redirect:/login/context?message=" + encode(e.getMessage())
                 + (companyId != null && companyId > 0 ? "&companyId=" + companyId : "");
        } catch (IllegalStateException e) {
            /* "Company record not found" / "Branch record not found" - back to the login form,
               the same as the desktop. */
            return "redirect:/login?error=true&message=" + encode(e.getMessage());
        }

        LoginContext.store(request, context);

        try {
            resolver.licenseCheck(user, context, request);
        } catch (RuntimeException e) {
            return "redirect:/login?error=true&message=" + encode(e.getMessage());
        }
        return "redirect:/dashboard";
    }

    private UserAccount requireUser(Authentication authentication) {
        if (authentication == null || authentication.getName() == null) return null;
        return userAccountRepository.findByUserName(authentication.getName());
    }

    private static String encode(String s) {
        try {
            return java.net.URLEncoder.encode(s == null ? "" : s, "UTF-8");
        } catch (java.io.UnsupportedEncodingException e) {
            return "";
        }
    }

    /** Exposed for the template so a row's id and label read the same way the desktop binds them. */
    public static int idOf(Map<String, Object> row, String column) {
        return DesktopUserAccountDal.intOf(row, column);
    }
}
