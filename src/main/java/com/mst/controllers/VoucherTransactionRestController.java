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

	/**
	 * The account pickers on every voucher screen.
	 *
	 * -----------------------------------------------------------------------------------------
	 * WHAT THIS USED TO DO, AND WHY IT HAD TO CHANGE
	 * -----------------------------------------------------------------------------------------
	 * Both this method and {@link #searchAccounts} used to run hand-written SQL over
	 * dbo.ChartofAccount with an invented filter ("AccountGroup = 'Detail' OR Account_Level >= 4"),
	 * NO organization or company predicate, NO per-user account allocation, exceptions swallowed
	 * with an empty catch, and — when everything failed — a hardcoded list of INVENTED accounts
	 * ("Cash in Hand" id 1, "Main Bank Account" id 2, "Office Expense Account" id 2) returned to the
	 * operator as if they were real chart-of-accounts rows. Picking one of those would have posted a
	 * voucher against whatever account happened to hold id 1 or 2 in the live database.
	 *
	 * The desktop never queries the table. ContraVoucher.AccountsComboBind():1101 calls
	 * CommonServices.Accounts_GetAccountTitleByAccountTypeIds -> COAAllocation, which is
	 *
	 *     [dbo].[USP_Accounts_GetAccountTitleByAccountTypeIds]
	 *         @OrganizationId @CompanyId @AppId @UserId [@AccountTypeIds] [@AccountTypeIdsNot]
	 *         [@AccountClassIds] [@AccountClassIdsNot] [@CostCenterId]
	 *
	 * so that is what runs here, with the signed-in user's own tenancy and application. Optional
	 * parameters are omitted when unset, exactly as the BLL's own guards do. A failure is reported
	 * and rethrown rather than answered with invented rows.
	 *
	 * -----------------------------------------------------------------------------------------
	 * accountTypeIds IS PER SCREEN
	 * -----------------------------------------------------------------------------------------
	 * Contra Voucher passes "2,15" (ContraVoucher.cs:1107) and both of its combos — Credit and
	 * Debit — bind that ONE result set. Other voucher screens pass their own values, which still
	 * have to be traced form by form; until each is, those pages send nothing and the procedure
	 * returns the user's full allowed list. That is a superset of the right answer rather than a
	 * wrong subset, and it is never fabricated.
	 */
	@GetMapping("/cash-bank-accounts")
	public ResponseEntity<?> getCashBankAccounts(
			@RequestParam(value = "accountTypeIds", required = false) String accountTypeIds,
			@RequestParam(value = "accountTypeIdsNot", required = false) String accountTypeIdsNot,
			@RequestParam(value = "costCenterId", required = false, defaultValue = "0") int costCenterId) {
		return ResponseEntity.ok(accountTitles(accountTypeIds, accountTypeIdsNot, costCenterId));
	}

	/**
	 * The same list, filtered by what the operator typed. The desktop's combo filters the bound
	 * DataTable in the control rather than asking the database again, so the filtering happens here
	 * in memory over the same rows for the same reason: a LIKE predicate of my own invention would
	 * match a different set from the one the desktop shows.
	 */
	@GetMapping("/accounts-search")
	public ResponseEntity<?> searchAccounts(
			@RequestParam(value = "query", defaultValue = "") String query,
			@RequestParam(value = "accountTypeIds", required = false) String accountTypeIds,
			@RequestParam(value = "accountTypeIdsNot", required = false) String accountTypeIdsNot,
			@RequestParam(value = "costCenterId", required = false, defaultValue = "0") int costCenterId) {
		List<Map<String, Object>> rows = accountTitles(accountTypeIds, accountTypeIdsNot, costCenterId);
		String q = query == null ? "" : query.trim().toLowerCase();
		if (q.isEmpty()) return ResponseEntity.ok(rows);
		List<Map<String, Object>> hit = new java.util.ArrayList<>();
		for (Map<String, Object> r : rows) {
			String title = String.valueOf(r.get("accountTitle") == null ? "" : r.get("accountTitle")).toLowerCase();
			String code  = String.valueOf(r.get("accountCode")  == null ? "" : r.get("accountCode")).toLowerCase();
			if (title.contains(q) || code.contains(q)) hit.add(r);
		}
		return ResponseEntity.ok(hit);
	}

	/**
	 * One call to the desktop's procedure, projected to the keys the voucher pages already read.
	 * The source columns are the ones DetailAccounts():1132 copies into its own DataTable:
	 * Id, AccountTitle, AccountCode, ParentAccountTitle, AccountClass.
	 */
	private List<Map<String, Object>> accountTitles(String accountTypeIds, String accountTypeIdsNot,
												   int costCenterId) {
		com.mst.models.UserAccount u = currentUserContext.requireAccountingUser();
		java.util.LinkedHashMap<String, Object> p = new java.util.LinkedHashMap<>();
		p.put("OrganizationId", u.getOrganizationId());
		p.put("CompanyId",      u.getCompanyId());
		/* CommonServices.Accounts_GetAccountTitleByAccountTypeIds passes
		   clsGlobalVariables.UserAccount.AppId straight through, with no check that the application
		   is one allocated to the user. currentAppId() adds that check and REFUSES when the user has
		   several applications and none chosen — which would leave every voucher screen with an empty
		   account picker where the desktop shows a full one. The raw value is used here for that
		   reason: same input, same rows. */
		p.put("AppId",          u.getAppId() == null ? 0 : u.getAppId());
		if (accountTypeIds    != null && !accountTypeIds.trim().isEmpty())    p.put("AccountTypeIds",    accountTypeIds.trim());
		if (accountTypeIdsNot != null && !accountTypeIdsNot.trim().isEmpty()) p.put("AccountTypeIdsNot", accountTypeIdsNot.trim());
		if (u.getId() != null && u.getId() != 0) p.put("UserId", u.getId());
		if (costCenterId != 0) p.put("CostCenterId", costCenterId);

		StringBuilder sql = new StringBuilder("EXEC [dbo].[USP_Accounts_GetAccountTitleByAccountTypeIds] ");
		List<Object> args = new java.util.ArrayList<>();
		boolean first = true;
		for (Map.Entry<String, Object> e : p.entrySet()) {
			if (!first) sql.append(", ");
			first = false;
			sql.append('@').append(e.getKey()).append("=?");
			args.add(e.getValue());
		}

		List<Map<String, Object>> out = new java.util.ArrayList<>();
		for (Map<String, Object> r : jdbcTemplate.queryForList(sql.toString(), args.toArray())) {
			Map<String, Object> o = new java.util.LinkedHashMap<>();
			o.put("id",                 ci(r, "Id"));
			o.put("accountTitle",       ci(r, "AccountTitle"));
			o.put("accountCode",        ci(r, "AccountCode"));
			o.put("parentAccountTitle", ci(r, "ParentAccountTitle"));
			o.put("accountClass",       ci(r, "AccountClass"));
			/* The pages that already read className keep working without another round-trip. */
			o.put("className",          ci(r, "AccountClass"));
			out.add(o);
		}
		return out;
	}

	private static Object ci(Map<String, Object> row, String name) {
		if (row == null) return null;
		if (row.containsKey(name)) return row.get(name);
		for (Map.Entry<String, Object> e : row.entrySet()) {
			if (e.getKey() != null && e.getKey().equalsIgnoreCase(name)) return e.getValue();
		}
		return null;
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

	@GetMapping("/by-code")
	public ResponseEntity<?> getVoucherByCode(
			@RequestParam("documentTypeId") int documentTypeId,
			@RequestParam("voucherCode") int voucherCode) {
		Map<String, Object> voucher = voucherService.getVoucherByCode(documentTypeId, voucherCode);
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
