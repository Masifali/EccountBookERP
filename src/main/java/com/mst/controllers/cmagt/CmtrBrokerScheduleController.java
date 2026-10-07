package com.mst.controllers.cmagt;

import com.mst.services.CmtrBrokerScheduleService;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

import java.util.Collections;
import java.util.Map;

/** Browser controller for both desktop menu entries backed by CommissionBrokerySchedule. */
@Controller
public class CmtrBrokerScheduleController {
    private final CmtrBrokerScheduleService service;

    public CmtrBrokerScheduleController(CmtrBrokerScheduleService service) { this.service = service; }

    @GetMapping("/commission/trading/broker-schedule")
    public String page(@RequestParam(defaultValue = "516") int screenId, Model model) {
        model.addAttribute("activeMenu", "commission");
        model.addAttribute("screenId", screenId);
        model.addAttribute("formTitle", screenId == 521 ? "Commission Brokery Schedule" : "Broker Schedule");
        return "cmagt/commission/broker_schedule";
    }

    @GetMapping("/commission/trading/api/broker-schedule/init")
    @ResponseBody public ResponseEntity<?> initialize(@RequestParam(defaultValue = "516") int screenId) {
        return run(() -> service.initialize(screenId));
    }

    @GetMapping("/commission/trading/api/broker-schedule/history")
    @ResponseBody public ResponseEntity<?> history(@RequestParam(defaultValue = "516") int screenId) {
        return run(() -> service.history(screenId));
    }

    @GetMapping("/commission/trading/api/broker-schedule/record")
    @ResponseBody public ResponseEntity<?> record(@RequestParam(defaultValue = "516") int screenId, @RequestParam int id) {
        return run(() -> service.record(screenId, id));
    }

    @GetMapping("/commission/trading/api/broker-schedule/rate")
    @ResponseBody public ResponseEntity<?> rate(@RequestParam(defaultValue = "516") int screenId,
                                                @RequestParam Map<String,String> query) {
        return run(() -> service.rate(screenId, query));
    }

    @PostMapping("/commission/trading/api/broker-schedule/save")
    @ResponseBody public ResponseEntity<?> save(@RequestParam(defaultValue = "516") int screenId,
                                                 @RequestBody Map<String,Object> body) {
        return run(() -> service.save(screenId, body));
    }

    @PostMapping("/commission/trading/api/broker-schedule/delete")
    @ResponseBody public ResponseEntity<?> delete(@RequestParam(defaultValue = "516") int screenId,
                                                   @RequestBody Map<String,Object> body) {
        Object id = body.get("id");
        return run(() -> service.delete(screenId, id instanceof Number n ? n.intValue() : Integer.parseInt(String.valueOf(id))));
    }

    private static ResponseEntity<?> run(Call call) {
        try { return ResponseEntity.ok(call.get()); }
        catch (IllegalArgumentException ex) { return ResponseEntity.badRequest().body(Collections.singletonMap("message", ex.getMessage())); }
        catch (org.springframework.security.access.AccessDeniedException ex) { return ResponseEntity.status(HttpStatus.FORBIDDEN).body(Collections.singletonMap("message", ex.getMessage())); }
        catch (Exception ex) {
            String message = ex.getMessage() == null || ex.getMessage().isBlank() ? "Unable to process broker schedule." : ex.getMessage();
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(Collections.singletonMap("message", message));
        }
    }

    @FunctionalInterface private interface Call { Object get() throws Exception; }
}
