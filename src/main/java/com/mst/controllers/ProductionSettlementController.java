package com.mst.controllers;

import com.mst.services.ProductionSettlementService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.dao.DataAccessException;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

import javax.servlet.http.HttpSession;
import java.sql.SQLException;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * 280 Production Against (Job Order) - Settlement tab - Architecture.WinApp.Production/frmProductionSettlement.cs
 *
 * The page the shell frames into its Settlement tab (FoodProductionWithValues.tabControl1_SelectedIndexChanged
 * constructs `new frmProductionSettlement(UserAccount) { OpenInProductionTab = true }`). It also works on
 * its own, which is the desktop's non-tab behaviour.
 *
 * No parameter carries tenancy, financial year, branch or user: all are server-derived.
 */
@Controller
public class ProductionSettlementController {

    private static final String API = "/api/production/production-against-job-order/settlement";

    @Autowired private ProductionSettlementService service;

    @GetMapping("/production/production-against-job-order/settlement")
    public String page(Model model) {
        model.addAttribute("activeMenu", "production");
        return "production/p280_settlement";
    }

    /** frmFoodProduction_Load:262. */
    @GetMapping(API + "/load")
    @ResponseBody
    public ResponseEntity<?> load() {
        try { return ResponseEntity.ok(service.load()); } catch (Exception e) { return error(e); }
    }

    /** JobOrderNoFillForSettlement(all ? 0 : 1). */
    @GetMapping(API + "/job-orders")
    @ResponseBody
    public ResponseEntity<?> jobOrders(@RequestParam(defaultValue = "false") boolean all) {
        try { return ResponseEntity.ok(service.jobOrders(all ? 0 : 1)); } catch (Exception e) { return error(e); }
    }

    /** btnGeneralSettlement_Click:376. */
    @GetMapping(API + "/generate")
    @ResponseBody
    public ResponseEntity<?> generate(@RequestParam(defaultValue = "0") int jobOrderId, HttpSession session) {
        try { return ResponseEntity.ok(service.generate(jobOrderId, session)); } catch (Exception e) { return error(e); }
    }

    /** GetUomScheduleByItemId (ProporationFinishGoodsSettlement:1210). */
    @GetMapping(API + "/uom-by-item")
    @ResponseBody
    public ResponseEntity<?> uomByItem(@RequestParam(defaultValue = "0") int itemId) {
        try { return ResponseEntity.ok(service.uomByItem(itemId)); } catch (Exception e) { return error(e); }
    }

    /** UpdateSettlement():1327 - {result:true} | {result:false, message, caption, icon} | 400 {message}. */
    @PostMapping(API + "/update-output")
    @ResponseBody
    public ResponseEntity<?> updateOutput(@RequestBody Map<String, Object> body, HttpSession session) {
        try {
            service.updateOutput(intOf(body.get("jobOrderId")), rowsOf(body.get("rows")), session);
            return ok(true);
        } catch (ProductionSettlementService.Refusal r) {
            return refusal(r);
        } catch (Exception e) { return error(e); }
    }

    /** InPutUpdateSettlement():1431. */
    @PostMapping(API + "/update-input")
    @ResponseBody
    public ResponseEntity<?> updateInput(@RequestBody Map<String, Object> body, HttpSession session) {
        try {
            service.updateInput(intOf(body.get("jobOrderId")), rowsOf(body.get("rows")), session);
            return ok(true);
        } catch (Exception e) { return error(e); }
    }

    /** SettlementFinancials():1505 - the checks before the write. */
    @PostMapping(API + "/financials/gate")
    @ResponseBody
    public ResponseEntity<?> financialsGate(@RequestBody Map<String, Object> body) {
        try {
            return ResponseEntity.ok(service.financialsGate(intOf(body.get("jobOrderId")),
                                                            intOf(body.get("differenceAccountId"))));
        } catch (Exception e) { return error(e); }
    }

    /** SettlementFinancials():1627 - RequestFromSettlementForJobOrderForSpecialApproval. */
    @PostMapping(API + "/financials/request-special-approval")
    @ResponseBody
    public ResponseEntity<?> requestSpecialApproval(@RequestBody Map<String, Object> body) {
        try {
            service.requestSpecialApproval(intOf(body.get("jobOrderId")), intOf(body.get("differenceAccountId")));
            return ok(true);
        } catch (Exception e) { return error(e); }
    }

    /** SettlementFinancials():1642 - InvFoodProduction.SetDataForSettlementFinancials. */
    @PostMapping(API + "/financials/save")
    @ResponseBody
    public ResponseEntity<?> saveFinancials(@RequestBody Map<String, Object> body) {
        try {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("result", service.saveFinancials(body));
            return ResponseEntity.ok(m);
        } catch (Exception e) { return error(e); }
    }

    /** btnApproveSettlement_Click:1905. */
    @PostMapping(API + "/approve")
    @ResponseBody
    public ResponseEntity<?> approve(@RequestBody Map<String, Object> body) {
        try {
            service.approve(intOf(body.get("jobOrderId")));
            return ok(true);
        } catch (Exception e) { return error(e); }
    }

    /** btnSettlementVoucher_Click:1975 - VoucherHeadIdGet(JobOrderId, 142). */
    @GetMapping(API + "/voucher-head")
    @ResponseBody
    public ResponseEntity<?> voucherHead(@RequestParam(defaultValue = "0") int jobOrderId) {
        try {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("voucherHeadId", service.voucherHeadId(jobOrderId));
            return ResponseEntity.ok(m);
        } catch (Exception e) { return error(e); }
    }

    /** Rows behind a print button, so the page can say "Record Not Found For DisPlay" as the desktop does. */
    @GetMapping(API + "/print-rows")
    @ResponseBody
    public ResponseEntity<?> printRows(@RequestParam String report,
                                       @RequestParam(defaultValue = "0") int jobOrderId,
                                       @RequestParam(defaultValue = "0") int actionId,
                                       @RequestParam(defaultValue = "0") int voucherHeadId) {
        try {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("rows", service.printRowCount(report, jobOrderId, actionId, voucherHeadId));
            return ResponseEntity.ok(m);
        } catch (Exception e) { return error(e); }
    }

    /* ------------------------------------------------------------------------------ helpers */

    private static ResponseEntity<?> ok(boolean v) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("result", v);
        return ResponseEntity.ok(m);
    }

    private static ResponseEntity<?> refusal(ProductionSettlementService.Refusal r) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("result", false);
        m.put("message", r.getMessage());
        m.put("caption", r.caption);
        m.put("icon", r.icon);
        return ResponseEntity.ok(m);
    }

    /**
     * The desktop shows ex.Message. A procedure's RAISERROR text is that message, so it is unwrapped
     * from Spring's DataAccessException rather than replaced by a generic one.
     */
    private static ResponseEntity<?> error(Exception e) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        String msg = e.getMessage();
        HttpStatus status = HttpStatus.BAD_REQUEST;
        if (e instanceof DataAccessException) {
            Throwable t = e;
            while (t.getCause() != null && !(t instanceof SQLException)) t = t.getCause();
            msg = t.getMessage();
        } else if (!(e instanceof IllegalArgumentException) && !(e instanceof IllegalStateException)) {
            status = HttpStatus.INTERNAL_SERVER_ERROR;
        }
        m.put("message", msg == null ? e.getClass().getSimpleName() : msg);
        return ResponseEntity.status(status).body(m);
    }

    private static int intOf(Object o) {
        if (o instanceof Number) return ((Number) o).intValue();
        try { return Integer.parseInt(String.valueOf(o).trim()); } catch (Exception e) { return 0; }
    }

    @SuppressWarnings("unchecked")
    private static List<Map<String, Object>> rowsOf(Object o) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (o instanceof List) {
            for (Object r : (List<?>) o) if (r instanceof Map) out.add((Map<String, Object>) r);
        }
        return out;
    }
}
