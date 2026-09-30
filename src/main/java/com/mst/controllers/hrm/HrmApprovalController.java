package com.mst.controllers.hrm;

import com.mst.models.hrm.dto.HrmAdvanceApprovalDto;
import com.mst.models.hrm.dto.HrmLeaveApprovalDto;
import com.mst.models.hrm.dto.HrmLoanApprovalDto;
import com.mst.services.hrm.HrmApprovalService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

/**
 * HRM "Approval Management" (AppModules 2026) pages and API.
 *
 *   667 /hrm/loan-approval      LoanApproval.cs     API /api/hrm/approval/loan-approval/{setup|list|save}
 *   668 /hrm/leave-approval     LeaveApproval.cs    API /api/hrm/approval/leave-approval/{setup|list|employees|approve|details|detail-approve|detail-reject}
 *                               (+ PendingLeaveDetailForApproval.cs as the "Detail" modal)
 *   669 /hrm/advance-approval   AdvanceApproval.cs  API /api/hrm/approval/advance-approval/{setup|list|employees|approve}
 */
@Controller
public class HrmApprovalController {

    private static final String API = "/api/hrm/approval";

    @Autowired private HrmApprovalService service;

    // ------------------------------------------------------------------ 667 Loan Approval

    @GetMapping("/hrm/loan-approval")
    public String loanApprovalPage(Model model) { model.addAttribute("activeMenu", "apps"); return "hrm/approval/loan-approval"; }

    @GetMapping(API + "/loan-approval/setup") @ResponseBody
    public ResponseEntity<?> loanSetup() { return HrmApi.run(() -> service.loanSetup()); }

    @GetMapping(API + "/loan-approval/list") @ResponseBody
    public ResponseEntity<?> loanList() { return HrmApi.run(() -> service.pendingLoans()); }

    @PostMapping(API + "/loan-approval/save") @ResponseBody
    public ResponseEntity<?> loanSave(@RequestBody HrmLoanApprovalDto body) { return HrmApi.run(() -> service.saveLoanApproval(body)); }

    // ------------------------------------------------------------------ 668 Leave Approval

    @GetMapping("/hrm/leave-approval")
    public String leaveApprovalPage(Model model) { model.addAttribute("activeMenu", "apps"); return "hrm/approval/leave-approval"; }

    @GetMapping(API + "/leave-approval/setup") @ResponseBody
    public ResponseEntity<?> leaveSetup() { return HrmApi.run(() -> service.leaveSetup()); }

    @GetMapping(API + "/leave-approval/list") @ResponseBody
    public ResponseEntity<?> leaveList(@RequestParam(value = "employeeId", defaultValue = "0") int employeeId,
                                       @RequestParam(value = "departmentId", defaultValue = "0") int departmentId,
                                       @RequestParam(value = "designationId", defaultValue = "0") int designationId) {
        return HrmApi.run(() -> service.leaves(employeeId, departmentId, designationId));
    }

    @GetMapping(API + "/leave-approval/employees") @ResponseBody
    public ResponseEntity<?> leaveEmployees(@RequestParam(value = "departmentId", defaultValue = "0") int departmentId,
                                            @RequestParam(value = "designationId", defaultValue = "0") int designationId) {
        return HrmApi.run(() -> service.filteredEmployees(HrmApprovalService.SCREEN_LEAVE_APPROVAL, departmentId, designationId));
    }

    @PostMapping(API + "/leave-approval/approve") @ResponseBody
    public ResponseEntity<?> leaveApprove(@RequestBody HrmLeaveApprovalDto body) { return HrmApi.run(() -> service.approveLeaves(body)); }

    @GetMapping(API + "/leave-approval/details") @ResponseBody
    public ResponseEntity<?> leaveDetails(@RequestParam("employeeId") int employeeId, @RequestParam("leaveTypeProfileId") int leaveTypeProfileId) {
        return HrmApi.run(() -> service.leaveDetails(employeeId, leaveTypeProfileId));
    }

    @PostMapping(API + "/leave-approval/detail-approve") @ResponseBody
    public ResponseEntity<?> leaveDetailApprove(@RequestBody HrmLeaveApprovalDto body) { return HrmApi.run(() -> service.leaveDetailAction(body, true)); }

    @PostMapping(API + "/leave-approval/detail-reject") @ResponseBody
    public ResponseEntity<?> leaveDetailReject(@RequestBody HrmLeaveApprovalDto body) { return HrmApi.run(() -> service.leaveDetailAction(body, false)); }

    // ------------------------------------------------------------------ 669 Advance Approval

    @GetMapping("/hrm/advance-approval")
    public String advanceApprovalPage(Model model) { model.addAttribute("activeMenu", "apps"); return "hrm/approval/advance-approval"; }

    @GetMapping(API + "/advance-approval/setup") @ResponseBody
    public ResponseEntity<?> advanceSetup() { return HrmApi.run(() -> service.advanceSetup()); }

    @GetMapping(API + "/advance-approval/list") @ResponseBody
    public ResponseEntity<?> advanceList(@RequestParam(value = "employeeId", defaultValue = "0") int employeeId,
                                         @RequestParam(value = "departmentId", defaultValue = "0") int departmentId,
                                         @RequestParam(value = "designationId", defaultValue = "0") int designationId) {
        return HrmApi.run(() -> service.advances(employeeId, departmentId, designationId));
    }

    @GetMapping(API + "/advance-approval/employees") @ResponseBody
    public ResponseEntity<?> advanceEmployees(@RequestParam(value = "departmentId", defaultValue = "0") int departmentId,
                                              @RequestParam(value = "designationId", defaultValue = "0") int designationId) {
        return HrmApi.run(() -> service.filteredEmployees(HrmApprovalService.SCREEN_ADVANCE_APPROVAL, departmentId, designationId));
    }

    @PostMapping(API + "/advance-approval/approve") @ResponseBody
    public ResponseEntity<?> advanceApprove(@RequestBody HrmAdvanceApprovalDto body) { return HrmApi.run(() -> service.approveAdvance(body)); }
}
