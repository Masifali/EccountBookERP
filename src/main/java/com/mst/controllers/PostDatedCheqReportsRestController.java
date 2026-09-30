package com.mst.controllers;

import com.mst.services.AccountReportsDesktopService;
import com.mst.services.PostDatedCheqReportsService;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.*;

/**
 * Account Reports (module 3) - PostDatedCheqInformation.cs (see PostDatedCheqReportsService).
 * Filter values arrive in the body; tenancy never does. Errors come back as {success:false, message},
 * the shape countx_acc_rpt_f.js pages already read.
 */
@RestController
@RequestMapping("/accounts/api/reports-pdc")
public class PostDatedCheqReportsRestController {

    @Autowired
    private PostDatedCheqReportsService service;

    @PostMapping("/post-dated-cheq-information")
    public ResponseEntity<?> postDatedCheqInformation(@RequestBody Map<String, Object> r) {
        try {
            Object a = r.get("accountId");
            Integer accountId = null;
            if (a != null && !a.toString().trim().isEmpty() && !"null".equals(a.toString().trim())) {
                accountId = (int) Double.parseDouble(a.toString().trim());
            }
            Object t = r.get("toDate");
            java.sql.Date toDate = AccountReportsDesktopService.date(t == null ? null : t.toString());
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("success", true);
            m.put("data", service.postDatedCheqByAccountId(accountId, toDate));
            return ResponseEntity.ok(m);
        } catch (Exception e) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("success", false);
            Throwable c = e;
            while (c.getCause() != null && c.getCause() != c) c = c.getCause();
            String msg = c.getMessage() != null ? c.getMessage() : e.getMessage();
            m.put("message", msg == null ? e.getClass().getSimpleName() : msg);
            return ResponseEntity.status(e instanceof IllegalArgumentException ? 200 : 500).body(m);
        }
    }
}
