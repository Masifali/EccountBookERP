package com.mst.controllers;

import com.mst.services.PlBsNotesService;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;

import javax.servlet.http.HttpSession;
import java.util.UUID;

/** Admin Panel -> "PL & BS Notes" (BsPlSettingForm). Page only; data via PlBsNotesRestController. */
@Controller
public class PlBsNotesViewController {

    static final String TOKEN = "plBsNotesCsrf";

    private final PlBsNotesService service;
    public PlBsNotesViewController(PlBsNotesService service) { this.service = service; }

    @GetMapping("/admin/pl-bs-notes")
    public String page(Model model, HttpSession session) {
        boolean can = service.canOpen();
        model.addAttribute("canOpen", can);
        if (can) {
            /* Spring's CSRF filter is disabled app-wide; the page carries its own token (as /configurations/define-reports). */
            synchronized (session) {
                if (session.getAttribute(TOKEN) == null) session.setAttribute(TOKEN, UUID.randomUUID().toString());
                model.addAttribute("token", session.getAttribute(TOKEN));
            }
        }
        return "admin/pl_bs_notes";
    }
}
