package com.mst.serviceInterface;

import java.time.LocalDate;
import java.util.List;
import java.util.Map;

import com.mst.models.VoucherHead;
import com.mst.models.dto.VoucherRequestDto;

public interface IVoucherService {
	int generateNextVoucherCode(int documentTypeId);

	List<VoucherHead> getVouchersByDocumentType(int documentTypeId);

	Map<String, Object> getVoucherById(int voucherHeadId);

	Map<String, Object> saveVoucher(VoucherRequestDto dto);

	void deleteVoucher(int voucherHeadId);

	List<Map<String, Object>> searchVouchers(int documentTypeId, String fromDate, String toDate, String query);

	void approveVouchers(List<Integer> voucherIds, boolean approve);

	List<Map<String, Object>> getDayBookApprovalVouchers();

	// CPV History / BPV History - see VoucherService for the real USP_VoucherFormHistory ditto-copy.
	List<Map<String, Object>> getCpvHistoryAccounts();

	List<Map<String, Object>> getBpvHistoryAccounts();

	Map<String, Object> getCpvHistory(String dateType, LocalDate fromDate, LocalDate toDate,
			Integer fromDocNo, Integer toDocNo, Integer accountId, String approvedStatus);

	Map<String, Object> getBpvHistory(String dateType, LocalDate fromDate, LocalDate toDate,
			Integer fromDocNo, Integer toDocNo, Integer accountId, String approvedStatus);

	// CRV/BRV/JV/Contra/Expense/Party Receipt/Party Payment/PDC Payment History - see VoucherService
	// for each voucher's real desktop-ditto USP_VoucherFormHistory call and column set.
	List<Map<String, Object>> getCrvHistoryAccounts();

	List<Map<String, Object>> getBrvHistoryAccounts();

	List<Map<String, Object>> getContraHistoryAccounts();

	List<Map<String, Object>> getExpenseHistoryAccounts();

	// Expense Voucher's "Project"/"Location Type" combos - real shared master-data lookups (ditto
	// CommonServices.ProjectServiceBind()/VoucherHead.GetLocationType(), themselves used across many
	// desktop voucher forms, not a "History" query) - see VoucherService for the exact procs.
	List<Map<String, Object>> getProjects();

	List<Map<String, Object>> getLocationTypes();

	List<Map<String, Object>> getPartyReceiptHistoryAccounts();

	List<Map<String, Object>> getPartyPaymentHistoryAccounts();

	Map<String, Object> getCrvHistory(String dateType, LocalDate fromDate, LocalDate toDate,
			Integer fromDocNo, Integer toDocNo, Integer accountId, String approvedStatus);

	Map<String, Object> getBrvHistory(String dateType, LocalDate fromDate, LocalDate toDate,
			Integer fromDocNo, Integer toDocNo, Integer accountId, String approvedStatus);

	// JV History has no Account-Title filter in desktop - no accountId parameter here.
	Map<String, Object> getJvHistory(String dateType, LocalDate fromDate, LocalDate toDate,
			Integer fromDocNo, Integer toDocNo, String approvedStatus);

	Map<String, Object> getContraHistory(String dateType, LocalDate fromDate, LocalDate toDate,
			Integer fromDocNo, Integer toDocNo, Integer accountId, String approvedStatus);

	Map<String, Object> getExpenseHistory(String dateType, LocalDate fromDate, LocalDate toDate,
			Integer fromDocNo, Integer toDocNo, Integer accountId, String approvedStatus);

	Map<String, Object> getPartyReceiptHistory(String dateType, LocalDate fromDate, LocalDate toDate,
			Integer fromDocNo, Integer toDocNo, Integer accountId, String approvedStatus);

	Map<String, Object> getPartyPaymentHistory(String dateType, LocalDate fromDate, LocalDate toDate,
			Integer fromDocNo, Integer toDocNo, Integer accountId, String approvedStatus);

	// PDC Payment History has NO filter UI in desktop at all - always loads every visible record.
	Map<String, Object> getPdcPaymentHistory();

	// CPV/BPV (PaymentVoucherNew.cs) real master-data lookups - see VoucherService for the exact
	// procs (Payment Type/Financial Instrument/Cheque Type/Tax Type combos).
	List<Map<String, Object>> getPaymentTypes();

	List<Map<String, Object>> getFinancialInstrumentTypes();

	List<Map<String, Object>> getChequeTypes();

	List<Map<String, Object>> getTaxTypes();

	// Payment By Invoice (PaymentByInvoiceVoucherNew.cs) real master-data lookups - see
	// VoucherService for the exact procs (Company/Branch/Invoice-No/Invoice-Balance/History).
	List<Map<String, Object>> getCompanies();

	List<Map<String, Object>> getBranches();

	List<Map<String, Object>> getInvoicesForPaymentByInvoice(int accountId);

	List<Map<String, Object>> getInvoiceBalanceForPaymentByInvoice(int accountId, int invoiceId);

	Map<String, Object> getPaymentByInvoiceHistory();
}
