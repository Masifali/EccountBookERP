package com.mst.controllers.lab;

import com.mst.services.lab.ExportPreShipmentAnalysisService;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.dao.DataAccessException;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.ExceptionHandler;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import java.util.LinkedHashMap;
import java.util.Map;

/**
 * JSON for screen 168 "Export Pre Shipment Analysis" (EximPreProductionLab.cs). No parameter carries tenancy.
 * There is no delete endpoint (the form has no Delete button) and no print endpoint (the "Print" tool
 * strip button has no Click handler on the desktop).
 *
 * Refusals are IllegalArgumentException (400, ApiExceptionAdvice) carrying the text the desktop shows in
 * MessageBox(ex.Message); rights are AccessDeniedException (403). A database error is answered as a 400
 * with its text for the same reason.
 */
@RestController
@RequestMapping("/api/lab/export-pre-shipment-analysis")
public class ExportPreShipmentAnalysisRestController {

    private static final Logger LOG = LoggerFactory.getLogger(ExportPreShipmentAnalysisRestController.class);

    private final ExportPreShipmentAnalysisService service;

    public ExportPreShipmentAnalysisRestController(ExportPreShipmentAnalysisService service) {
        this.service = service;
    }

    /** FrmPreProductionLotInspection_Load: rights + both combo lists + grid. */
    @GetMapping("/init")
    public Map<String, Object> init() { return service.init(); }

    /** btnRefresh_Click: ItemNameFill + LabInspectionNameFill. */
    @GetMapping("/combos")
    public Map<String, Object> combos() { return service.combos(); }

    /** ReadAll(). */
    @GetMapping("/grid")
    public Map<String, Object> grid() { return service.grid(); }

    /** ReadById(ID). */
    @GetMapping("/{id}")
    public Map<String, Object> byId(@PathVariable int id) { return service.byId(id); }

    /** btnsave_Click / btnUpdate_Click -> Insert(). */
    @PostMapping("/save")
    public Map<String, Object> save(@RequestBody SaveRequest body) {
        return service.save(body.getId(), body.getInspectedByLabId(), body.getReportDate(), body.getReportRefNo(),
                body.getLotRefNo(), body.getItemId(), body.getQtyKgs(), body.getInspectionRemarks());
    }

    @ExceptionHandler(DataAccessException.class)
    public ResponseEntity<Map<String, Object>> database(DataAccessException e) {
        Throwable r = e;
        while (r.getCause() != null && r.getCause() != r) r = r.getCause();
        String text = r.getMessage() == null || r.getMessage().trim().isEmpty() ? "Database Error" : r.getMessage().trim();
        LOG.warn("Export Pre Shipment Analysis database refusal: {}", text);
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        m.put("status", "ERROR");
        m.put("message", text);
        m.put("error", text);
        return ResponseEntity.badRequest().body(m);
    }

    /** The posted body — the eight values Insert() takes from the form. qtyKgs is the text box's text. */
    public static class SaveRequest {
        private Integer id;
        private Integer inspectedByLabId;
        private String reportDate;
        private String reportRefNo;
        private String lotRefNo;
        private Integer itemId;
        private String qtyKgs;
        private String inspectionRemarks;

        public Integer getId() { return id; }
        public void setId(Integer id) { this.id = id; }
        public Integer getInspectedByLabId() { return inspectedByLabId; }
        public void setInspectedByLabId(Integer inspectedByLabId) { this.inspectedByLabId = inspectedByLabId; }
        public String getReportDate() { return reportDate; }
        public void setReportDate(String reportDate) { this.reportDate = reportDate; }
        public String getReportRefNo() { return reportRefNo; }
        public void setReportRefNo(String reportRefNo) { this.reportRefNo = reportRefNo; }
        public String getLotRefNo() { return lotRefNo; }
        public void setLotRefNo(String lotRefNo) { this.lotRefNo = lotRefNo; }
        public Integer getItemId() { return itemId; }
        public void setItemId(Integer itemId) { this.itemId = itemId; }
        public String getQtyKgs() { return qtyKgs; }
        public void setQtyKgs(String qtyKgs) { this.qtyKgs = qtyKgs; }
        public String getInspectionRemarks() { return inspectionRemarks; }
        public void setInspectionRemarks(String inspectionRemarks) { this.inspectionRemarks = inspectionRemarks; }
    }
}
