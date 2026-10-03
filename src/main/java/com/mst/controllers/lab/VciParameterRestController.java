package com.mst.controllers.lab;

import com.mst.services.lab.VciParameterService;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.dao.DataAccessException;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.ExceptionHandler;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * JSON for screen 809 "VCI Parameter" (ExImVCIParameter.cs). No parameter carries tenancy.
 * There is no delete and no print endpoint: the desktop toolstrip (:445) is btnnew, btnRefresh, btnsave,
 * btnUpdate, btnVCICategoryDefine.
 *
 * Validation refusals are IllegalArgumentException (400, ApiExceptionAdvice); rights are
 * AccessDeniedException (403). A database error is what the desktop shows as MessageBox(ex.Message), so it
 * is answered as a 400 with that text.
 */
@RestController
@RequestMapping("/api/lab/vci-parameter")
public class VciParameterRestController {

    private static final Logger LOG = LoggerFactory.getLogger(VciParameterRestController.class);

    private final VciParameterService service;

    public VciParameterRestController(VciParameterService service) {
        this.service = service;
    }

    /** ExImVCIParameter_Load: rights + VCI Category list + history rows. */
    @GetMapping("/init")
    public Map<String, Object> init() { return service.init(); }

    /** ParameterCategoryfill() — btnRefresh_Click. */
    @GetMapping("/categories")
    public List<Map<String, Object>> categories() { return service.categories(); }

    /** FormHistory() — categoryId is Conversion.ToInt(cmbvciCategory.Value); 0 = every category. */
    @GetMapping("/history")
    public List<Map<String, Object>> history(@RequestParam(name = "categoryId", defaultValue = "0") int categoryId) {
        return service.history(categoryId);
    }

    /** btnsave_Click (id 0) / btnUpdate_Click (id = RecId). */
    @PostMapping("/save")
    public Map<String, Object> save(@RequestBody SaveRequest body) {
        return service.save(body.getId(), body.getCategoryId(), body.getDescription());
    }

    @ExceptionHandler(DataAccessException.class)
    public ResponseEntity<Map<String, Object>> database(DataAccessException e) {
        Throwable r = e;
        while (r.getCause() != null && r.getCause() != r) r = r.getCause();
        String text = r.getMessage() == null || r.getMessage().trim().isEmpty() ? "Database Error" : r.getMessage().trim();
        LOG.warn("VCI Parameter database refusal: {}", text);
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        m.put("status", "ERROR");
        m.put("message", text);
        m.put("error", text);
        return ResponseEntity.badRequest().body(m);
    }

    /** The posted body — the model's own fields (Model 0049) minus tenancy, dates and SortNo. */
    public static class SaveRequest {
        private Integer id;
        private Integer categoryId;
        private String description;

        public Integer getId() { return id; }
        public void setId(Integer id) { this.id = id; }
        public Integer getCategoryId() { return categoryId; }
        public void setCategoryId(Integer categoryId) { this.categoryId = categoryId; }
        public String getDescription() { return description; }
        public void setDescription(String description) { this.description = description; }
    }
}
