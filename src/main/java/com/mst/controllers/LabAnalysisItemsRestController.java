package com.mst.controllers;

import com.mst.services.LabAnalysisItemsService;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.Map;

/**
 * Screen 156 - Item Analysis Parameter.
 *
 * Five endpoints, one per thing the desktop form does. No delete: the desktop grid sets
 * AllowDelete off (grdfrmSetting, :255) and the form has no delete button, so there is nothing to
 * port - and inventing one would let a crafted request remove a parameter that existing
 * PurchaseOrderLabDeduction and InvLabGroupAnalysisStandards rows point at.
 *
 * Nothing here accepts an organization, company or user id from the caller. Every read and write
 * derives them from the session inside the service.
 */
@RestController
@RequestMapping("/api/lab/item-analysis-parameter")
public class LabAnalysisItemsRestController {

    private final LabAnalysisItemsService service;

    public LabAnalysisItemsRestController(LabAnalysisItemsService service) {
        this.service = service;
    }

    /** gridfill() - the always-visible grid. */
    @GetMapping("/grid")
    public ResponseEntity<List<Map<String, Object>>> grid() {
        return ResponseEntity.ok(service.grid());
    }

    /**
     * The Parent Parameter combo. Served separately from the grid because the page needs it even
     * while the grid is being re-read, though both come from the same rows - see the service.
     */
    @GetMapping("/parent-parameters")
    public ResponseEntity<List<Map<String, Object>>> parentParameters() {
        return ResponseEntity.ok(service.parentParameters());
    }

    /** MasterParameters() - usp_getLabMasterParms. */
    @GetMapping("/master-parameters")
    public ResponseEntity<List<Map<String, Object>>> masterParameters() {
        return ResponseEntity.ok(service.masterParameters());
    }

    /** grdfrm_DoubleClick - open one row into the entry fields. */
    @GetMapping("/{id}")
    public ResponseEntity<Map<String, Object>> byId(@PathVariable Integer id) {
        Map<String, Object> row = service.readById(id == null ? 0 : id);
        return row == null ? ResponseEntity.notFound().build() : ResponseEntity.ok(row);
    }

    /**
     * btnsave_Click / btnupdate_Click. Both reach the desktop's single Insert(); the only
     * difference is whether RecId is 0, so both arrive here and the id in the body decides.
     */
    @PostMapping("/save")
    public ResponseEntity<Map<String, Object>> save(@RequestBody LabAnalysisItemRequest body) {
        return ResponseEntity.ok(service.save(
                body.getId(), body.getDescription(), Boolean.TRUE.equals(body.getIsSub()),
                body.getParentParameterId(), body.getMasterParId(),
                body.getMinValue(), body.getMaxValue()));
    }

    /** The posted body. Deliberately carries no tenancy fields for the server to trust. */
    public static class LabAnalysisItemRequest {
        private Integer id;
        private String description;
        private Boolean isSub;
        private Integer parentParameterId;
        private Integer masterParId;
        private Double minValue;
        private Double maxValue;

        public Integer getId() { return id; }
        public void setId(Integer id) { this.id = id; }
        public String getDescription() { return description; }
        public void setDescription(String description) { this.description = description; }
        public Boolean getIsSub() { return isSub; }
        public void setIsSub(Boolean isSub) { this.isSub = isSub; }
        public Integer getParentParameterId() { return parentParameterId; }
        public void setParentParameterId(Integer parentParameterId) { this.parentParameterId = parentParameterId; }
        public Integer getMasterParId() { return masterParId; }
        public void setMasterParId(Integer masterParId) { this.masterParId = masterParId; }
        public Double getMinValue() { return minValue; }
        public void setMinValue(Double minValue) { this.minValue = minValue; }
        public Double getMaxValue() { return maxValue; }
        public void setMaxValue(Double maxValue) { this.maxValue = maxValue; }
    }
}
