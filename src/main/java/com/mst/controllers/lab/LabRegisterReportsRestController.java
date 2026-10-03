package com.mst.controllers.lab;

import com.mst.services.lab.LabRegisterReportsService;
import java.util.List;
import java.util.Map;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

/**
 * Lab Report (module 1011) API. Desktop MessageBox texts are IllegalArgumentException (HTTP 400,
 * ApiExceptionAdvice); a missing View grant is AccessDeniedException (403).
 *
 *   /sample-register/*     629 + 632  InvLabSampleRegister ("screen" = 629 or 632 picks the rights row only)
 *   /inprocess-register/*  631        InProcessLabAnalysisRegister
 */
@RestController
@RequestMapping("/api/lab/reports")
public class LabRegisterReportsRestController {

    private final LabRegisterReportsService service;

    public LabRegisterReportsRestController(LabRegisterReportsService service) { this.service = service; }

    // ---------------------------------------------------------------- 629 / 632
    @GetMapping("/sample-register/init")
    public Map<String, Object> sampleInit(@RequestParam int screen) { return service.sampleInit(screen); }

    @PostMapping("/sample-register/rows")
    public Map<String, Object> sampleRows(@RequestBody Map<String, Object> body) { return service.sampleRows(body); }

    // ---------------------------------------------------------------- 631
    @GetMapping("/inprocess-register/init")
    public Map<String, Object> inProcessInit() { return service.inProcessInit(); }

    @GetMapping("/inprocess-register/lookups")
    public Map<String, Object> inProcessLookups() { return service.inProcessLookups(); }

    @GetMapping("/inprocess-register/dates")
    public List<Map<String, Object>> inProcessDates(@RequestParam(defaultValue = "0") int plantId,
                                                    @RequestParam(defaultValue = "0") int jobOrderId,
                                                    @RequestParam(defaultValue = "0") int itemId) {
        return service.inProcessDates(plantId, jobOrderId, itemId);
    }

    @PostMapping("/inprocess-register/rows")
    public Map<String, Object> inProcessRows(@RequestBody Map<String, Object> body) { return service.inProcessRows(body); }
}
