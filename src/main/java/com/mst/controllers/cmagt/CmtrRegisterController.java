package com.mst.controllers.cmagt;

import com.mst.services.CmtrRegisterService;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

import java.util.Collections;
import java.util.Map;

/** Commission Trading register screens and their desktop DDL-backed report calls. */
@Controller
public class CmtrRegisterController {
    private final CmtrRegisterService service;

    public CmtrRegisterController(CmtrRegisterService service) {
        this.service = service;
    }

    @GetMapping("/commission/trading/loading-delivery-register")
    public String loadingDeliveryRegister(Model model) {
        model.addAttribute("activeMenu", "commission");
        model.addAttribute("moduleTitle", "Commission Delivery Loading Register");
        model.addAttribute("registerType", CmtrRegisterService.LOADING_DELIVERY);
        model.addAttribute("registerTitle", "Commission Delivery Loading Register");
        return "cmagt/reports/cmtr_register";
    }

    @GetMapping("/commission/trading/transaction-register")
    public String transactionRegister(Model model) {
        model.addAttribute("activeMenu", "commission");
        model.addAttribute("moduleTitle", "Commission Transaction Register");
        model.addAttribute("registerType", CmtrRegisterService.TRANSACTION);
        model.addAttribute("registerTitle", "Commission Transaction Register");
        return "cmagt/reports/cmtr_register";
    }

    @GetMapping("/commission/trading/api/register/init")
    @ResponseBody
    public ResponseEntity<?> initialize(@RequestParam String type) {
        return run(() -> service.initialize(type));
    }

    @GetMapping("/commission/trading/api/register/show")
    @ResponseBody
    public ResponseEntity<?> show(@RequestParam String type, @RequestParam Map<String, String> filters) {
        return run(() -> service.report(type, filters));
    }

    private static ResponseEntity<?> run(Call call) {
        try { return ResponseEntity.ok(call.get()); }
        catch (IllegalArgumentException ex) {
            return ResponseEntity.badRequest().body(Collections.singletonMap("message", ex.getMessage()));
        } catch (org.springframework.security.access.AccessDeniedException ex) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(Collections.singletonMap("message", ex.getMessage()));
        } catch (Exception ex) {
            String message = ex.getMessage() == null || ex.getMessage().isBlank() ? "Unable to load this register." : ex.getMessage();
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(Collections.singletonMap("message", message));
        }
    }

    @FunctionalInterface private interface Call { Object get() throws Exception; }
}
