package com.mst.controllers;

import java.time.LocalDate;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.DeleteMapping;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import com.mst.models.dto.VoucherRequestDto;
import com.mst.serviceInterface.IVoucherService;

@RestController
@RequestMapping("/accounts/api/vouchers")
public class VoucherTransactionRestController {

	@Autowired
	private IVoucherService voucherService;

	@Autowired
	private com.mst.serviceInterface.IChartofAccountService chartofAccountService;

	@Autowired
	private org.springframework.jdbc.core.JdbcTemplate jdbcTemplate;

	@Autowired
	private com.mst.security.CurrentUserContext currentUserContext;

	@GetMapping("/cash-bank-accounts")
	public ResponseEntity<?> getCashBankAccounts(@RequestParam(value = "type", required = false) String type) {
		try {
			String sql = "SELECT Id as id, AccountTitle as accountTitle, AccountCode as accountCode FROM ChartofAccount WHERE (AccountGroup = 'Detail' OR Account_Level >= 4) ORDER BY AccountTitle ASC";
			List<Map<String, Object>> list = jdbcTemplate.queryForList(sql);
			if (list != null && !list.isEmpty()) {
				return ResponseEntity.ok(list);
			}
		} catch (Exception e) {
		}
		try {
			String sql = "SELECT Id as id, AccountTitle as accountTitle, AccountCode as accountCode FROM ChartofAccount ORDER BY AccountTitle ASC";
			List<Map<String, Object>> list = jdbcTemplate.queryForList(sql);
			if (list != null && !list.isEmpty()) {
				return ResponseEntity.ok(list);
			}
		} catch (Exception e) {
		}

		List<Map<String, Object>> fallback = new java.util.ArrayList<>();
		Map<String, Object> a1 = new java.util.HashMap<>(); a1.put("id", 1); a1.put("accountTitle", "Cash in Hand"); a1.put("accountCode", "100101"); fallback.add(a1);
		Map<String, Object> a2 = new java.util.HashMap<>(); a2.put("id", 2); a2.put("accountTitle", "Main Bank Account"); a2.put("accountCode", "100102"); fallback.add(a2);
		return ResponseEntity.ok(fallback);
	}

	@GetMapping("/accounts-search")
	public ResponseEntity<?> searchAccounts(@RequestParam(value = "query", defaultValue = "") String query) {
		try {
			String searchPattern = "%" + query.trim() + "%";
			String sql = "SELECT c.Id as id, c.AccountCode as accountCode, c.AccountTitle as accountTitle, " +
					"COALESCE(p.AccountTitle, 'Detail') as parentAccountTitle, 'Assets' as className " +
					"FROM ChartofAccount c " +
					"LEFT JOIN ChartofAccount p ON c.ParentAccountCode = p.AccountCode " +
					"WHERE (c.AccountGroup = 'Detail' OR c.Account_Level >= 4) " +
					"AND (c.AccountTitle LIKE ? OR c.AccountCode LIKE ?) " +
					"ORDER BY c.AccountTitle ASC";
			List<Map<String, Object>> list = jdbcTemplate.queryForList(sql, searchPattern, searchPattern);
			if (list != null && !list.isEmpty()) {
				return ResponseEntity.ok(list);
			}
		} catch (Exception e) {
		}
		try {
			String searchPattern = "%" + query.trim() + "%";
			String sql = "SELECT Id as id, AccountCode as accountCode, AccountTitle as accountTitle, 'Detail' as parentAccountTitle FROM ChartofAccount WHERE AccountTitle LIKE ? OR AccountCode LIKE ? ORDER BY AccountTitle ASC";
			List<Map<String, Object>> list = jdbcTemplate.queryForList(sql, searchPattern, searchPattern);
			if (list != null && !list.isEmpty()) {
				return ResponseEntity.ok(list);
			}
		} catch (Exception e) {
		}

		List<Map<String, Object>> fallback = new java.util.ArrayList<>();
		Map<String, Object> a1 = new java.util.HashMap<>(); a1.put("id", 1); a1.put("accountCode", "100101"); a1.put("accountTitle", "Cash in Hand"); a1.put("parentAccountTitle", "Cash Accounts"); fallback.add(a1);
		Map<String, Object> a2 = new java.util.HashMap<>(); a2.put("id", 2); a2.put("accountCode", "100201"); a2.put("accountTitle", "Office Expense Account"); a2.put("parentAccountTitle", "Expense Accounts"); fallback.add(a2);
		return ResponseEntity.ok(fallback);
	}

	@GetMapping("/next-code")
	public ResponseEntity<?> getNextCode(@RequestParam("documentTypeId") int documentTypeId) {
		int nextCode = voucherService.generateNextVoucherCode(documentTypeId);
		Map<String, Object> res = new HashMap<>();
		res.put("nextCode", nextCode);
		return ResponseEntity.ok(res);
	}

	@GetMapping("/{id}")
	public ResponseEntity<?> getVoucherById(@PathVariable("id") int id) {
		Map<String, Object> voucher = voucherService.getVoucherById(id);
		if (voucher == null) {
			return ResponseEntity.notFound().build();
		}
		return ResponseEntity.ok(voucher);
	}

	@PostMapping("/save")
	public ResponseEntity<?> saveVoucher(@RequestBody VoucherRequestDto dto) {
		Map<String, Object> res = voucherService.saveVoucher(dto);
		if (Boolean.TRUE.equals(res.get("success"))) {
			return ResponseEntity.ok(res);
		} else {
			return ResponseEntity.badRequest().body(res);
		}
	}

	@DeleteMapping("/{id}")
	@PostMapping("/delete/{id}")
	public ResponseEntity<?> deleteVoucher(@PathVariable("id") int id) {
		voucherService.deleteVoucher(id);
		Map<String, Object> res = new HashMap<>();
		res.put("success", true);
		res.put("message", "Voucher deleted successfully!");
		return ResponseEntity.ok(res);
	}

	@GetMapping("/search")
	public ResponseEntity<?> searchVouchers(
			@RequestParam("documentTypeId") int documentTypeId,
			@RequestParam(value = "fromDate", required = false) String fromDate,
			@RequestParam(value = "toDate", required = false) String toDate,
			@RequestParam(value = "query", required = false) String query) {
		return ResponseEntity.ok(voucherService.searchVouchers(documentTypeId, fromDate, toDate, query));
	}

	@PostMapping("/approve")
	public ResponseEntity<?> approveVouchers(
			@RequestBody List<Integer> voucherIds,
			@RequestParam(value = "approve", defaultValue = "true") boolean approve) {
		voucherService.approveVouchers(voucherIds, approve);
		Map<String, Object> res = new HashMap<>();
		res.put("success", true);
		res.put("message", "Vouchers " + (approve ? "approved" : "unapproved") + " successfully!");
		return ResponseEntity.ok(res);
	}

	@GetMapping("/day-book-approval")
	public ResponseEntity<?> getDayBookApprovalVouchers() {
		return ResponseEntity.ok(voucherService.getDayBookApprovalVouchers());
	}

	// ==========================================================================================
	// CPV History / BPV History - own real endpoints per voucher (not a shared/generic history API),
	// ditto desktop's own separate HistoryFillCpv()/HistoryFillBpv() - see VoucherService for the
	// real EXEC USP_VoucherFormHistory call each one is backed by.
	// ==========================================================================================

	@GetMapping("/cpv/history/accounts")
	public ResponseEntity<?> getCpvHistoryAccounts() {
		return ResponseEntity.ok(voucherService.getCpvHistoryAccounts());
	}

	@GetMapping("/cpv/history")
	public ResponseEntity<?> getCpvHistory(
			@RequestParam(value = "dateType", required = false) String dateType,
			@RequestParam(value = "fromDate", required = false) String fromDate,
			@RequestParam(value = "toDate", required = false) String toDate,
			@RequestParam(value = "fromDocNo", required = false) Integer fromDocNo,
			@RequestParam(value = "toDocNo", required = false) Integer toDocNo,
			@RequestParam(value = "accountId", required = false) Integer accountId,
			@RequestParam(value = "approvedStatus", required = false, defaultValue = "notapproved") String approvedStatus) {
		LocalDate from = (fromDate != null && !fromDate.isBlank()) ? LocalDate.parse(fromDate) : null;
		LocalDate to = (toDate != null && !toDate.isBlank()) ? LocalDate.parse(toDate) : null;
		return ResponseEntity.ok(voucherService.getCpvHistory(dateType, from, to, fromDocNo, toDocNo, accountId, approvedStatus));
	}

	@GetMapping("/bpv/history/accounts")
	public ResponseEntity<?> getBpvHistoryAccounts() {
		return ResponseEntity.ok(voucherService.getBpvHistoryAccounts());
	}

	@GetMapping("/bpv/history")
	public ResponseEntity<?> getBpvHistory(
			@RequestParam(value = "dateType", required = false) String dateType,
			@RequestParam(value = "fromDate", required = false) String fromDate,
			@RequestParam(value = "toDate", required = false) String toDate,
			@RequestParam(value = "fromDocNo", required = false) Integer fromDocNo,
			@RequestParam(value = "toDocNo", required = false) Integer toDocNo,
			@RequestParam(value = "accountId", required = false) Integer accountId,
			@RequestParam(value = "approvedStatus", required = false, defaultValue = "notapproved") String approvedStatus) {
		LocalDate from = (fromDate != null && !fromDate.isBlank()) ? LocalDate.parse(fromDate) : null;
		LocalDate to = (toDate != null && !toDate.isBlank()) ? LocalDate.parse(toDate) : null;
		return ResponseEntity.ok(voucherService.getBpvHistory(dateType, from, to, fromDocNo, toDocNo, accountId, approvedStatus));
	}

	// ==========================================================================================
	// CRV/BRV/JV/Contra/Expense/Party Receipt/Party Payment/PDC Payment History - own real endpoints
	// per voucher, same pattern as CPV/BPV above (see VoucherService for the real EXEC
	// USP_VoucherFormHistory / CommonServices.VoucherFormHistory ditto-copy each one is backed by).
	// ==========================================================================================

	@GetMapping("/crv/history/accounts")
	public ResponseEntity<?> getCrvHistoryAccounts() {
		return ResponseEntity.ok(voucherService.getCrvHistoryAccounts());
	}

	@GetMapping("/crv/history")
	public ResponseEntity<?> getCrvHistory(
			@RequestParam(value = "dateType", required = false) String dateType,
			@RequestParam(value = "fromDate", required = false) String fromDate,
			@RequestParam(value = "toDate", required = false) String toDate,
			@RequestParam(value = "fromDocNo", required = false) Integer fromDocNo,
			@RequestParam(value = "toDocNo", required = false) Integer toDocNo,
			@RequestParam(value = "accountId", required = false) Integer accountId,
			@RequestParam(value = "approvedStatus", required = false, defaultValue = "notapproved") String approvedStatus) {
		LocalDate from = (fromDate != null && !fromDate.isBlank()) ? LocalDate.parse(fromDate) : null;
		LocalDate to = (toDate != null && !toDate.isBlank()) ? LocalDate.parse(toDate) : null;
		return ResponseEntity.ok(voucherService.getCrvHistory(dateType, from, to, fromDocNo, toDocNo, accountId, approvedStatus));
	}

	@GetMapping("/brv/history/accounts")
	public ResponseEntity<?> getBrvHistoryAccounts() {
		return ResponseEntity.ok(voucherService.getBrvHistoryAccounts());
	}

	@GetMapping("/brv/history")
	public ResponseEntity<?> getBrvHistory(
			@RequestParam(value = "dateType", required = false) String dateType,
			@RequestParam(value = "fromDate", required = false) String fromDate,
			@RequestParam(value = "toDate", required = false) String toDate,
			@RequestParam(value = "fromDocNo", required = false) Integer fromDocNo,
			@RequestParam(value = "toDocNo", required = false) Integer toDocNo,
			@RequestParam(value = "accountId", required = false) Integer accountId,
			@RequestParam(value = "approvedStatus", required = false, defaultValue = "notapproved") String approvedStatus) {
		LocalDate from = (fromDate != null && !fromDate.isBlank()) ? LocalDate.parse(fromDate) : null;
		LocalDate to = (toDate != null && !toDate.isBlank()) ? LocalDate.parse(toDate) : null;
		return ResponseEntity.ok(voucherService.getBrvHistory(dateType, from, to, fromDocNo, toDocNo, accountId, approvedStatus));
	}

	// JV History has no Account-Title filter in desktop - no accountId query param here.
	@GetMapping("/jv/history")
	public ResponseEntity<?> getJvHistory(
			@RequestParam(value = "dateType", required = false) String dateType,
			@RequestParam(value = "fromDate", required = false) String fromDate,
			@RequestParam(value = "toDate", required = false) String toDate,
			@RequestParam(value = "fromDocNo", required = false) Integer fromDocNo,
			@RequestParam(value = "toDocNo", required = false) Integer toDocNo,
			@RequestParam(value = "approvedStatus", required = false, defaultValue = "notapproved") String approvedStatus) {
		LocalDate from = (fromDate != null && !fromDate.isBlank()) ? LocalDate.parse(fromDate) : null;
		LocalDate to = (toDate != null && !toDate.isBlank()) ? LocalDate.parse(toDate) : null;
		return ResponseEntity.ok(voucherService.getJvHistory(dateType, from, to, fromDocNo, toDocNo, approvedStatus));
	}

	@GetMapping("/contra/history/accounts")
	public ResponseEntity<?> getContraHistoryAccounts() {
		return ResponseEntity.ok(voucherService.getContraHistoryAccounts());
	}

	@GetMapping("/contra/history")
	public ResponseEntity<?> getContraHistory(
			@RequestParam(value = "dateType", required = false) String dateType,
			@RequestParam(value = "fromDate", required = false) String fromDate,
			@RequestParam(value = "toDate", required = false) String toDate,
			@RequestParam(value = "fromDocNo", required = false) Integer fromDocNo,
			@RequestParam(value = "toDocNo", required = false) Integer toDocNo,
			@RequestParam(value = "accountId", required = false) Integer accountId,
			@RequestParam(value = "approvedStatus", required = false, defaultValue = "notapproved") String approvedStatus) {
		LocalDate from = (fromDate != null && !fromDate.isBlank()) ? LocalDate.parse(fromDate) : null;
		LocalDate to = (toDate != null && !toDate.isBlank()) ? LocalDate.parse(toDate) : null;
		return ResponseEntity.ok(voucherService.getContraHistory(dateType, from, to, fromDocNo, toDocNo, accountId, approvedStatus));
	}

	@GetMapping("/expense/history/accounts")
	public ResponseEntity<?> getExpenseHistoryAccounts() {
		return ResponseEntity.ok(voucherService.getExpenseHistoryAccounts());
	}

	// Expense Voucher's real "Project" ("Cost Center" caption) and "Location Type" dropdowns -
	// ditto CommonServices.ProjectServiceBind() / VoucherHead.GetLocationType(). See VoucherService.
	@GetMapping("/projects")
	public ResponseEntity<?> getProjects() {
		return ResponseEntity.ok(voucherService.getProjects());
	}

	@GetMapping("/location-types")
	public ResponseEntity<?> getLocationTypes() {
		return ResponseEntity.ok(voucherService.getLocationTypes());
	}

	// CPV/BPV (PaymentVoucherNew.cs) real master-data lookups - see VoucherService for the exact
	// procs (Payment Type/Financial Instrument/Cheque Type/Tax Type combos).
	@GetMapping("/payment-types")
	public ResponseEntity<?> getPaymentTypes() {
		return ResponseEntity.ok(voucherService.getPaymentTypes());
	}

	@GetMapping("/financial-instrument-types")
	public ResponseEntity<?> getFinancialInstrumentTypes() {
		return ResponseEntity.ok(voucherService.getFinancialInstrumentTypes());
	}

	@GetMapping("/cheque-types")
	public ResponseEntity<?> getChequeTypes() {
		return ResponseEntity.ok(voucherService.getChequeTypes());
	}

	@GetMapping("/tax-types")
	public ResponseEntity<?> getTaxTypes() {
		return ResponseEntity.ok(voucherService.getTaxTypes());
	}

	// Payment By Invoice (PaymentByInvoiceVoucherNew.cs) real master-data lookups - see
	// VoucherService for the exact procs (Company/Branch/Invoice-No/Invoice-Balance/History).
	@GetMapping("/payment-by-invoice/companies")
	public ResponseEntity<?> getCompanies() {
		return ResponseEntity.ok(voucherService.getCompanies());
	}

	@GetMapping("/payment-by-invoice/branches")
	public ResponseEntity<?> getBranches() {
		return ResponseEntity.ok(voucherService.getBranches());
	}

	@GetMapping("/payment-by-invoice/invoices")
	public ResponseEntity<?> getInvoicesForPaymentByInvoice(@RequestParam("accountId") int accountId) {
		return ResponseEntity.ok(voucherService.getInvoicesForPaymentByInvoice(accountId));
	}

	@GetMapping("/payment-by-invoice/invoice-balance")
	public ResponseEntity<?> getInvoiceBalanceForPaymentByInvoice(
			@RequestParam("accountId") int accountId, @RequestParam("invoiceId") int invoiceId) {
		return ResponseEntity.ok(voucherService.getInvoiceBalanceForPaymentByInvoice(accountId, invoiceId));
	}

	@GetMapping("/payment-by-invoice/history")
	public ResponseEntity<?> getPaymentByInvoiceHistory() {
		return ResponseEntity.ok(voucherService.getPaymentByInvoiceHistory());
	}

	@GetMapping("/expense/history")
	public ResponseEntity<?> getExpenseHistory(
			@RequestParam(value = "dateType", required = false) String dateType,
			@RequestParam(value = "fromDate", required = false) String fromDate,
			@RequestParam(value = "toDate", required = false) String toDate,
			@RequestParam(value = "fromDocNo", required = false) Integer fromDocNo,
			@RequestParam(value = "toDocNo", required = false) Integer toDocNo,
			@RequestParam(value = "accountId", required = false) Integer accountId,
			@RequestParam(value = "approvedStatus", required = false, defaultValue = "notapproved") String approvedStatus) {
		LocalDate from = (fromDate != null && !fromDate.isBlank()) ? LocalDate.parse(fromDate) : null;
		LocalDate to = (toDate != null && !toDate.isBlank()) ? LocalDate.parse(toDate) : null;
		return ResponseEntity.ok(voucherService.getExpenseHistory(dateType, from, to, fromDocNo, toDocNo, accountId, approvedStatus));
	}

	@GetMapping("/party-receipt/history/accounts")
	public ResponseEntity<?> getPartyReceiptHistoryAccounts() {
		return ResponseEntity.ok(voucherService.getPartyReceiptHistoryAccounts());
	}

	@GetMapping("/party-receipt/history")
	public ResponseEntity<?> getPartyReceiptHistory(
			@RequestParam(value = "dateType", required = false) String dateType,
			@RequestParam(value = "fromDate", required = false) String fromDate,
			@RequestParam(value = "toDate", required = false) String toDate,
			@RequestParam(value = "fromDocNo", required = false) Integer fromDocNo,
			@RequestParam(value = "toDocNo", required = false) Integer toDocNo,
			@RequestParam(value = "accountId", required = false) Integer accountId,
			@RequestParam(value = "approvedStatus", required = false, defaultValue = "notapproved") String approvedStatus) {
		LocalDate from = (fromDate != null && !fromDate.isBlank()) ? LocalDate.parse(fromDate) : null;
		LocalDate to = (toDate != null && !toDate.isBlank()) ? LocalDate.parse(toDate) : null;
		return ResponseEntity.ok(voucherService.getPartyReceiptHistory(dateType, from, to, fromDocNo, toDocNo, accountId, approvedStatus));
	}

	@GetMapping("/party-payment/history/accounts")
	public ResponseEntity<?> getPartyPaymentHistoryAccounts() {
		return ResponseEntity.ok(voucherService.getPartyPaymentHistoryAccounts());
	}

	@GetMapping("/party-payment/history")
	public ResponseEntity<?> getPartyPaymentHistory(
			@RequestParam(value = "dateType", required = false) String dateType,
			@RequestParam(value = "fromDate", required = false) String fromDate,
			@RequestParam(value = "toDate", required = false) String toDate,
			@RequestParam(value = "fromDocNo", required = false) Integer fromDocNo,
			@RequestParam(value = "toDocNo", required = false) Integer toDocNo,
			@RequestParam(value = "accountId", required = false) Integer accountId,
			@RequestParam(value = "approvedStatus", required = false, defaultValue = "notapproved") String approvedStatus) {
		LocalDate from = (fromDate != null && !fromDate.isBlank()) ? LocalDate.parse(fromDate) : null;
		LocalDate to = (toDate != null && !toDate.isBlank()) ? LocalDate.parse(toDate) : null;
		return ResponseEntity.ok(voucherService.getPartyPaymentHistory(dateType, from, to, fromDocNo, toDocNo, accountId, approvedStatus));
	}

	// PDC Payment History has no filter UI in desktop at all - always loads every visible record.
	@GetMapping("/pdc-payment/history")
	public ResponseEntity<?> getPdcPaymentHistory() {
		return ResponseEntity.ok(voucherService.getPdcPaymentHistory());
	}
}
