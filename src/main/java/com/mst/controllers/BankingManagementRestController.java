package com.mst.controllers;

import java.math.BigDecimal;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.web.bind.annotation.DeleteMapping;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import com.mst.models.Bank;
import com.mst.models.BankReconciliation;
import com.mst.models.ChartofAccount;
import com.mst.repositories.IBankReconciliationRepository;
import com.mst.repositories.IBankRepository;
import com.mst.repositories.IChartofAccountRepository;

import com.mst.models.BankCharges;
import com.mst.serviceInterface.IBankChargesService;
import com.mst.serviceInterface.IDefineBankService;

@RestController
@RequestMapping("/api/banking")
public class BankingManagementRestController {

	@Autowired
	private IDefineBankService defineBankService;

	@Autowired
	private IBankChargesService bankChargesService;

	@Autowired
	private IBankRepository bankRepository;

	@Autowired
	private IBankReconciliationRepository bankReconciliationRepository;

	@Autowired
	private IChartofAccountRepository chartofAccountRepository;

	@Autowired
	private JdbcTemplate jdbcTemplate;

	// ==========================================
	// 1. DEFINE BANK ENDPOINTS
	// ==========================================

	@GetMapping("/define-bank/list")
	public ResponseEntity<List<Map<String, Object>>> getDefineBankList() {
		return ResponseEntity.ok(defineBankService.getBankList());
	}

	@PostMapping("/define-bank/save")
	public ResponseEntity<?> saveDefineBank(@RequestBody Bank bank) {
		try {
			Bank saved = defineBankService.saveBank(bank);
			return ResponseEntity.ok(saved);
		} catch (Exception e) {
			return ResponseEntity.badRequest().body("Error saving Bank: " + e.getMessage());
		}
	}

	@DeleteMapping("/define-bank/{id}")
	public ResponseEntity<?> deleteDefineBank(@PathVariable("id") Integer id) {
		try {
			defineBankService.deleteBank(id);
			Map<String, Object> resp = new HashMap<>();
			resp.put("success", true);
			resp.put("message", "Bank deleted successfully");
			return ResponseEntity.ok(resp);
		} catch (Exception e) {
			return ResponseEntity.badRequest().body("Error deleting bank: " + e.getMessage());
		}
	}

	// ==========================================
	// 2. BANK DETAIL DEFINITION ENDPOINTS
	// ==========================================

	@GetMapping("/bank-details/list")
	public ResponseEntity<List<Map<String, Object>>> getBankDetailsList() {
		String sql = "SELECT sbd.Id as id, sbd.SupCustId as bankAccountId, coa.AccountTitle as bankAccountTitle, " +
				"sbd.BankBranchName as branchName, sbd.BankAccountNo as accountNo, sbd.BankIBANNo as ibanNo, " +
				"sbd.SwiftCode as swiftCode, sbd.ContactPerson as contactPerson, sbd.ContactPhone as contactPhone, " +
				"sbd.IsActive as isActive, sbd.Remarks as remarks " +
				"FROM SupCustBankDetail sbd " +
				"LEFT JOIN ChartofAccount coa ON sbd.SupCustId = coa.Id " +
				"ORDER BY sbd.Id DESC";
		try {
			List<Map<String, Object>> list = jdbcTemplate.queryForList(sql);
			return ResponseEntity.ok(list);
		} catch (Exception e) {
			return ResponseEntity.ok(new ArrayList<>());
		}
	}

	@PostMapping("/bank-details/save")
	public ResponseEntity<?> saveBankDetail(@RequestBody Map<String, Object> req) {
		try {
			Integer id = req.get("id") != null ? Integer.parseInt(req.get("id").toString()) : null;
			Integer bankAccountId = req.get("bankAccountId") != null ? Integer.parseInt(req.get("bankAccountId").toString()) : 0;
			String branchName = (String) req.getOrDefault("branchName", "");
			String accountNo = (String) req.getOrDefault("accountNo", "");
			String ibanNo = (String) req.getOrDefault("ibanNo", "");
			String swiftCode = (String) req.getOrDefault("swiftCode", "");
			String contactPerson = (String) req.getOrDefault("contactPerson", "");
			String contactPhone = (String) req.getOrDefault("contactPhone", "");
			Boolean isActive = req.get("isActive") != null ? Boolean.parseBoolean(req.get("isActive").toString()) : true;
			String remarks = (String) req.getOrDefault("remarks", "");

			if (id == null || id <= 0) {
				String maxSql = "SELECT COALESCE(MAX(Id), 0) FROM SupCustBankDetail";
				Integer maxId = jdbcTemplate.queryForObject(maxSql, Integer.class);
				id = (maxId == null ? 0 : maxId) + 1;

				String insertSql = "INSERT INTO SupCustBankDetail (Id, SupCustId, BankBranchName, BankAccountNo, BankIBANNo, SwiftCode, ContactPerson, ContactPhone, IsActive, Remarks) " +
						"VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";
				jdbcTemplate.update(insertSql, id, bankAccountId, branchName, accountNo, ibanNo, swiftCode, contactPerson, contactPhone, isActive, remarks);
			} else {
				String updateSql = "UPDATE SupCustBankDetail SET SupCustId = ?, BankBranchName = ?, BankAccountNo = ?, BankIBANNo = ?, SwiftCode = ?, ContactPerson = ?, ContactPhone = ?, IsActive = ?, Remarks = ? WHERE Id = ?";
				jdbcTemplate.update(updateSql, bankAccountId, branchName, accountNo, ibanNo, swiftCode, contactPerson, contactPhone, isActive, remarks, id);
			}

			Map<String, Object> resp = new HashMap<>();
			resp.put("success", true);
			resp.put("id", id);
			resp.put("message", "Bank Detail saved successfully");
			return ResponseEntity.ok(resp);
		} catch (Exception e) {
			return ResponseEntity.badRequest().body("Error saving Bank Detail: " + e.getMessage());
		}
	}

	@DeleteMapping("/bank-details/{id}")
	public ResponseEntity<?> deleteBankDetail(@PathVariable("id") Integer id) {
		try {
			jdbcTemplate.update("DELETE FROM SupCustBankDetail WHERE Id = ?", id);
			Map<String, Object> resp = new HashMap<>();
			resp.put("success", true);
			resp.put("message", "Bank Detail deleted successfully");
			return ResponseEntity.ok(resp);
		} catch (Exception e) {
			return ResponseEntity.badRequest().body("Error deleting Bank Detail: " + e.getMessage());
		}
	}

	// ==========================================
	// 3. BANK RECONCILIATION ENDPOINTS
	// ==========================================

	@GetMapping("/reconciliation/vouchers")
	public ResponseEntity<List<Map<String, Object>>> getReconciliationVouchers(
			@RequestParam(value = "bankAccountId", required = false) Integer bankAccountId,
			@RequestParam(value = "fromDate", required = false) String fromDate,
			@RequestParam(value = "toDate", required = false) String toDate) {
		
		StringBuilder sql = new StringBuilder();
		sql.append("SELECT vh.Id as voucherHeadId, vh.VoucherNo as voucherNo, vh.VoucherDate as voucherDate, ")
		   .append("dt.DocumentTypeName as docType, coa.AccountTitle as accountTitle, vh.ChequeNo as chequeNo, ")
		   .append("vd.Debit as debit, vd.Credit as credit, br.Id as reconciliationId, ")
		   .append("br.IsReconciled as isReconciled, br.ReconcileDate as reconcileDate, br.Remarks as remarks ")
		   .append("FROM VoucherHead vh ")
		   .append("INNER JOIN VoucherDetail vd ON vh.Id = vd.VoucherHeadId ")
		   .append("LEFT JOIN DocumentType dt ON vh.DocumentTypeId = dt.Id ")
		   .append("LEFT JOIN ChartofAccount coa ON vd.ChartofAccountId = coa.Id ")
		   .append("LEFT JOIN BankReconciliation br ON (vh.Id = br.VoucherHeadId AND vd.Id = br.VoucherDetailId) ")
		   .append("WHERE 1=1 ");

		List<Object> params = new ArrayList<>();
		if (bankAccountId != null && bankAccountId > 0) {
			sql.append("AND vd.ChartofAccountId = ? ");
			params.add(bankAccountId);
		}
		if (fromDate != null && !fromDate.trim().isEmpty()) {
			sql.append("AND vh.VoucherDate >= ? ");
			params.add(fromDate);
		}
		if (toDate != null && !toDate.trim().isEmpty()) {
			sql.append("AND vh.VoucherDate <= ? ");
			params.add(toDate);
		}
		sql.append("ORDER BY vh.VoucherDate DESC, vh.Id DESC");

		try {
			List<Map<String, Object>> list = jdbcTemplate.queryForList(sql.toString(), params.toArray());
			return ResponseEntity.ok(list);
		} catch (Exception e) {
			return ResponseEntity.ok(new ArrayList<>());
		}
	}

	@PostMapping("/reconciliation/save")
	public ResponseEntity<?> saveReconciliationBatch(@RequestBody List<Map<String, Object>> items) {
		try {
			int count = 0;
			for (Map<String, Object> item : items) {
				Integer reconId = item.get("reconciliationId") != null ? Integer.parseInt(item.get("reconciliationId").toString()) : null;
				Integer voucherHeadId = item.get("voucherHeadId") != null ? Integer.parseInt(item.get("voucherHeadId").toString()) : null;
				Integer bankAccountId = item.get("bankAccountId") != null ? Integer.parseInt(item.get("bankAccountId").toString()) : null;
				Boolean isReconciled = item.get("isReconciled") != null ? Boolean.parseBoolean(item.get("isReconciled").toString()) : false;
				String remarks = (String) item.getOrDefault("remarks", "");
				String reconcileDateStr = (String) item.get("reconcileDate");

				if (reconId != null && reconId > 0) {
					String updateSql = "UPDATE BankReconciliation SET IsReconciled = ?, Remarks = ?, ReconcileDate = ? WHERE Id = ?";
					jdbcTemplate.update(updateSql, isReconciled, remarks, reconcileDateStr != null ? reconcileDateStr : LocalDate.now().toString(), reconId);
				} else if (voucherHeadId != null) {
					int maxId = bankReconciliationRepository.findMaxId();
					BankReconciliation br = new BankReconciliation();
					br.setId(maxId + 1);
					br.setVoucherHeadId(voucherHeadId);
					br.setBankAccountId(bankAccountId);
					br.setIsReconciled(isReconciled);
					br.setRemarks(remarks);
					br.setOrganizationId(1);
					br.setCompanyId(1);
					br.setBranchesId(1);
					br.setProjectsId(1);
					br.setFinancialYearId(1);
					br.setEntryUserId(1);
					br.setEntryDate(LocalDateTime.now());
					br.setReconcileDate(LocalDateTime.now());
					bankReconciliationRepository.save(br);
				}
				count++;
			}
			Map<String, Object> resp = new HashMap<>();
			resp.put("success", true);
			resp.put("count", count);
			resp.put("message", count + " reconciliation items updated successfully");
			return ResponseEntity.ok(resp);
		} catch (Exception e) {
			return ResponseEntity.badRequest().body("Error saving reconciliation batch: " + e.getMessage());
		}
	}

	// ==========================================
	// 4. PDC MANAGEMENT ENDPOINTS
	// ==========================================

	@GetMapping("/pdc/list")
	public ResponseEntity<List<Map<String, Object>>> getPdcList() {
		String sql = "SELECT p.Id as id, p.ChequeNo as chequeNo, p.ChequeDate as chequeDate, " +
				"p.DepositDate as depositDate, p.ClearingDate as clearingDate, p.Amount as amount, " +
				"p.Status as status, p.ChequeType as chequeType, p.Remarks as remarks, " +
				"p.BankAccountId as bankAccountId, b.BranchName as bankName, " +
				"p.PartyAccountId as partyAccountId, coa.AccountTitle as partyAccountTitle " +
				"FROM PdcBank p " +
				"LEFT JOIN Bank b ON p.BankAccountId = b.Id " +
				"LEFT JOIN ChartofAccount coa ON p.PartyAccountId = coa.Id " +
				"ORDER BY p.Id DESC";
		try {
			List<Map<String, Object>> list = jdbcTemplate.queryForList(sql);
			return ResponseEntity.ok(list);
		} catch (Exception e) {
			return ResponseEntity.ok(new ArrayList<>());
		}
	}

	@PostMapping("/pdc/save")
	public ResponseEntity<?> savePdc(@RequestBody Map<String, Object> req) {
		try {
			Integer id = req.get("id") != null ? Integer.parseInt(req.get("id").toString()) : null;
			Integer bankAccountId = req.get("bankAccountId") != null ? Integer.parseInt(req.get("bankAccountId").toString()) : 0;
			Integer partyAccountId = req.get("partyAccountId") != null ? Integer.parseInt(req.get("partyAccountId").toString()) : 0;
			String chequeNo = (String) req.getOrDefault("chequeNo", "");
			String chequeDate = (String) req.getOrDefault("chequeDate", "");
			String depositDate = (String) req.getOrDefault("depositDate", "");
			String clearingDate = (String) req.getOrDefault("clearingDate", "");
			BigDecimal amount = req.get("amount") != null ? new BigDecimal(req.get("amount").toString()) : BigDecimal.ZERO;
			String status = (String) req.getOrDefault("status", "Pending");
			String chequeType = (String) req.getOrDefault("chequeType", "Received");
			String remarks = (String) req.getOrDefault("remarks", "");

			if (id == null || id <= 0) {
				String maxSql = "SELECT COALESCE(MAX(Id), 0) FROM PdcBank";
				Integer maxId = jdbcTemplate.queryForObject(maxSql, Integer.class);
				id = (maxId == null ? 0 : maxId) + 1;

				String insertSql = "INSERT INTO PdcBank (Id, BankAccountId, PartyAccountId, ChequeNo, ChequeDate, DepositDate, ClearingDate, Amount, Status, ChequeType, Remarks, EntryDate) " +
						"VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, GETDATE())";
				jdbcTemplate.update(insertSql, id, bankAccountId, partyAccountId, chequeNo, chequeDate, depositDate, clearingDate, amount, status, chequeType, remarks);
			} else {
				String updateSql = "UPDATE PdcBank SET BankAccountId = ?, PartyAccountId = ?, ChequeNo = ?, ChequeDate = ?, DepositDate = ?, ClearingDate = ?, Amount = ?, Status = ?, ChequeType = ?, Remarks = ? WHERE Id = ?";
				jdbcTemplate.update(updateSql, bankAccountId, partyAccountId, chequeNo, chequeDate, depositDate, clearingDate, amount, status, chequeType, remarks, id);
			}

			Map<String, Object> resp = new HashMap<>();
			resp.put("success", true);
			resp.put("id", id);
			resp.put("message", "PDC record saved successfully");
			return ResponseEntity.ok(resp);
		} catch (Exception e) {
			return ResponseEntity.badRequest().body("Error saving PDC: " + e.getMessage());
		}
	}

	@DeleteMapping("/pdc/{id}")
	public ResponseEntity<?> deletePdc(@PathVariable("id") Integer id) {
		try {
			jdbcTemplate.update("DELETE FROM PdcBank WHERE Id = ?", id);
			Map<String, Object> resp = new HashMap<>();
			resp.put("success", true);
			resp.put("message", "PDC record deleted successfully");
			return ResponseEntity.ok(resp);
		} catch (Exception e) {
			return ResponseEntity.badRequest().body("Error deleting PDC: " + e.getMessage());
		}
	}

	// ==========================================
	// 5. CHEQUE PRINTING ENDPOINTS
	// ==========================================

	@GetMapping("/cheque-printing/list")
	public ResponseEntity<List<Map<String, Object>>> getChequePrintingList() {
		String sql = "SELECT cp.Id as id, cp.BankAccountId as bankAccountId, coa.AccountTitle as bankAccountTitle, " +
				"cp.PayeeName as payeeName, cp.ChequeDate as chequeDate, cp.Amount as amount, " +
				"cp.AmountInWords as amountInWords, cp.ChequeNo as chequeNo, cp.TemplateName as templateName, " +
				"cp.IsCrossed as isCrossed " +
				"FROM ChequePrinting cp " +
				"LEFT JOIN ChartofAccount coa ON cp.BankAccountId = coa.Id " +
				"ORDER BY cp.Id DESC";
		try {
			List<Map<String, Object>> list = jdbcTemplate.queryForList(sql);
			return ResponseEntity.ok(list);
		} catch (Exception e) {
			return ResponseEntity.ok(new ArrayList<>());
		}
	}

	@PostMapping("/cheque-printing/save")
	public ResponseEntity<?> saveChequePrinting(@RequestBody Map<String, Object> req) {
		try {
			Integer id = req.get("id") != null ? Integer.parseInt(req.get("id").toString()) : null;
			Integer bankAccountId = req.get("bankAccountId") != null ? Integer.parseInt(req.get("bankAccountId").toString()) : 0;
			String payeeName = (String) req.getOrDefault("payeeName", "");
			String chequeDate = (String) req.getOrDefault("chequeDate", "");
			BigDecimal amount = req.get("amount") != null ? new BigDecimal(req.get("amount").toString()) : BigDecimal.ZERO;
			String amountInWords = (String) req.getOrDefault("amountInWords", "");
			String chequeNo = (String) req.getOrDefault("chequeNo", "");
			String templateName = (String) req.getOrDefault("templateName", "Standard");
			Boolean isCrossed = req.get("isCrossed") != null ? Boolean.parseBoolean(req.get("isCrossed").toString()) : true;

			if (id == null || id <= 0) {
				String maxSql = "SELECT COALESCE(MAX(Id), 0) FROM ChequePrinting";
				Integer maxId = jdbcTemplate.queryForObject(maxSql, Integer.class);
				id = (maxId == null ? 0 : maxId) + 1;

				String insertSql = "INSERT INTO ChequePrinting (Id, BankAccountId, PayeeName, ChequeDate, Amount, AmountInWords, ChequeNo, TemplateName, IsCrossed, PrintDate) " +
						"VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, GETDATE())";
				jdbcTemplate.update(insertSql, id, bankAccountId, payeeName, chequeDate, amount, amountInWords, chequeNo, templateName, isCrossed);
			} else {
				String updateSql = "UPDATE ChequePrinting SET BankAccountId = ?, PayeeName = ?, ChequeDate = ?, Amount = ?, AmountInWords = ?, ChequeNo = ?, TemplateName = ?, IsCrossed = ? WHERE Id = ?";
				jdbcTemplate.update(updateSql, bankAccountId, payeeName, chequeDate, amount, amountInWords, chequeNo, templateName, isCrossed, id);
			}

			Map<String, Object> resp = new HashMap<>();
			resp.put("success", true);
			resp.put("id", id);
			resp.put("message", "Cheque record saved successfully");
			return ResponseEntity.ok(resp);
		} catch (Exception e) {
			return ResponseEntity.badRequest().body("Error saving Cheque record: " + e.getMessage());
		}
	}

	// ==========================================
	// 6. BANK CHARGES ENDPOINTS
	// ==========================================

	@GetMapping("/bank-charges/list")
	public ResponseEntity<List<Map<String, Object>>> getBankChargesList() {
		return ResponseEntity.ok(bankChargesService.getBankChargesList());
	}

	@PostMapping("/bank-charges/save")
	public ResponseEntity<?> saveBankCharges(@RequestBody Map<String, Object> req) {
		try {
			Integer id = req.get("id") != null ? Integer.parseInt(req.get("id").toString()) : null;
			Integer bankAccountId = req.get("bankAccountId") != null ? Integer.parseInt(req.get("bankAccountId").toString()) : 0;
			String chargeDate = (String) req.getOrDefault("chargeDate", "");
			String chargeType = (String) req.getOrDefault("chargeType", "");
			BigDecimal amount = req.get("amount") != null ? new BigDecimal(req.get("amount").toString()) : BigDecimal.ZERO;
			BigDecimal taxAmount = req.get("taxAmount") != null ? new BigDecimal(req.get("taxAmount").toString()) : BigDecimal.ZERO;
			BigDecimal totalAmount = req.get("totalAmount") != null ? new BigDecimal(req.get("totalAmount").toString()) : amount.add(taxAmount);
			String refNo = (String) req.getOrDefault("refNo", "");
			String remarks = (String) req.getOrDefault("remarks", "");

			BankCharges bc = new BankCharges();
			bc.setId(id);
			bc.setBankAccountId(bankAccountId);
			bc.setChargeDate(chargeDate);
			bc.setChargeType(chargeType);
			bc.setAmount(amount);
			bc.setTaxAmount(taxAmount);
			bc.setTotalAmount(totalAmount);
			bc.setRefNo(refNo);
			bc.setRemarks(remarks);

			BankCharges saved = bankChargesService.saveBankCharges(bc);
			Map<String, Object> resp = new HashMap<>();
			resp.put("success", true);
			resp.put("id", saved.getId());
			resp.put("message", "Bank Charges saved successfully");
			return ResponseEntity.ok(resp);
		} catch (Exception e) {
			return ResponseEntity.badRequest().body("Error saving Bank Charges: " + e.getMessage());
		}
	}

	@DeleteMapping("/bank-charges/{id}")
	public ResponseEntity<?> deleteBankCharges(@PathVariable("id") Integer id) {
		try {
			bankChargesService.deleteBankCharges(id);
			Map<String, Object> resp = new HashMap<>();
			resp.put("success", true);
			resp.put("message", "Bank Charges record deleted successfully");
			return ResponseEntity.ok(resp);
		} catch (Exception e) {
			return ResponseEntity.badRequest().body("Error deleting Bank Charges: " + e.getMessage());
		}
	}

	// ==========================================
	// 7. COA ALLOCATE COST CENTER ENDPOINTS
	// ==========================================

	@GetMapping("/coa-cost-center/list")
	public ResponseEntity<List<Map<String, Object>>> getCoaCostCenterList(
			@RequestParam(value = "accountId", required = false) Integer accountId) {
		StringBuilder sql = new StringBuilder();
		sql.append("SELECT ca.Id as id, ca.ChartOfAccountId as accountId, coa.AccountTitle as accountTitle, ")
		   .append("ca.CostCenterId as costCenterId, cc.CostCenterName as costCenterName, ")
		   .append("ca.AllocationPercentage as allocationPercentage, ca.IsActive as isActive ")
		   .append("FROM COAAllocation ca ")
		   .append("LEFT JOIN ChartofAccount coa ON ca.ChartOfAccountId = coa.Id ")
		   .append("LEFT JOIN CostCenter cc ON ca.CostCenterId = cc.Id ")
		   .append("WHERE 1=1 ");

		List<Object> params = new ArrayList<>();
		if (accountId != null && accountId > 0) {
			sql.append("AND ca.ChartOfAccountId = ? ");
			params.add(accountId);
		}
		sql.append("ORDER BY ca.Id DESC");

		try {
			List<Map<String, Object>> list = jdbcTemplate.queryForList(sql.toString(), params.toArray());
			return ResponseEntity.ok(list);
		} catch (Exception e) {
			return ResponseEntity.ok(new ArrayList<>());
		}
	}

	@PostMapping("/coa-cost-center/save")
	public ResponseEntity<?> saveCoaCostCenterAllocation(@RequestBody Map<String, Object> req) {
		try {
			Integer id = req.get("id") != null ? Integer.parseInt(req.get("id").toString()) : null;
			Integer accountId = req.get("accountId") != null ? Integer.parseInt(req.get("accountId").toString()) : 0;
			Integer costCenterId = req.get("costCenterId") != null ? Integer.parseInt(req.get("costCenterId").toString()) : 0;
			BigDecimal allocationPercentage = req.get("allocationPercentage") != null ? new BigDecimal(req.get("allocationPercentage").toString()) : BigDecimal.valueOf(100);
			Boolean isActive = req.get("isActive") != null ? Boolean.parseBoolean(req.get("isActive").toString()) : true;

			if (id == null || id <= 0) {
				String maxSql = "SELECT COALESCE(MAX(Id), 0) FROM COAAllocation";
				Integer maxId = jdbcTemplate.queryForObject(maxSql, Integer.class);
				id = (maxId == null ? 0 : maxId) + 1;

				String insertSql = "INSERT INTO COAAllocation (Id, ChartOfAccountId, CostCenterId, AllocationPercentage, IsActive) VALUES (?, ?, ?, ?, ?)";
				jdbcTemplate.update(insertSql, id, accountId, costCenterId, allocationPercentage, isActive);
			} else {
				String updateSql = "UPDATE COAAllocation SET ChartOfAccountId = ?, CostCenterId = ?, AllocationPercentage = ?, IsActive = ? WHERE Id = ?";
				jdbcTemplate.update(updateSql, accountId, costCenterId, allocationPercentage, isActive, id);
			}

			Map<String, Object> resp = new HashMap<>();
			resp.put("success", true);
			resp.put("id", id);
			resp.put("message", "COA Cost Center Allocation saved successfully");
			return ResponseEntity.ok(resp);
		} catch (Exception e) {
			return ResponseEntity.badRequest().body("Error saving COA Allocation: " + e.getMessage());
		}
	}

	@DeleteMapping("/coa-cost-center/{id}")
	public ResponseEntity<?> deleteCoaCostCenterAllocation(@PathVariable("id") Integer id) {
		try {
			jdbcTemplate.update("DELETE FROM COAAllocation WHERE Id = ?", id);
			Map<String, Object> resp = new HashMap<>();
			resp.put("success", true);
			resp.put("message", "COA Allocation deleted successfully");
			return ResponseEntity.ok(resp);
		} catch (Exception e) {
			return ResponseEntity.badRequest().body("Error deleting COA Allocation: " + e.getMessage());
		}
	}

	// ==========================================
	// HELPER DROPDOWNS
	// ==========================================

	@GetMapping("/dropdowns/bank-accounts")
	public ResponseEntity<List<ChartofAccount>> getBankAccountsDropdown() {
		try {
			List<ChartofAccount> list = chartofAccountRepository.findBankAccounts();
			return ResponseEntity.ok(list);
		} catch (Exception e) {
			return ResponseEntity.ok(new ArrayList<>());
		}
	}

	@GetMapping("/dropdowns/countries")
	public ResponseEntity<List<Map<String, Object>>> getCountriesDropdown() {
		try {
			String sql = "SELECT Id as id, CountryName as countryName FROM Country ORDER BY CountryName ASC";
			return ResponseEntity.ok(jdbcTemplate.queryForList(sql));
		} catch (Exception e) {
			return ResponseEntity.ok(new ArrayList<>());
		}
	}

	@GetMapping("/dropdowns/cities")
	public ResponseEntity<List<Map<String, Object>>> getCitiesDropdown() {
		try {
			String sql = "SELECT Id as id, CityName as cityName FROM City ORDER BY CityName ASC";
			return ResponseEntity.ok(jdbcTemplate.queryForList(sql));
		} catch (Exception e) {
			return ResponseEntity.ok(new ArrayList<>());
		}
	}

	@GetMapping("/dropdowns/cost-centers")
	public ResponseEntity<List<Map<String, Object>>> getCostCentersDropdown() {
		try {
			String sql = "SELECT Id as id, CostCenterName as costCenterName FROM CostCenter ORDER BY CostCenterName ASC";
			return ResponseEntity.ok(jdbcTemplate.queryForList(sql));
		} catch (Exception e) {
			return ResponseEntity.ok(new ArrayList<>());
		}
	}

	// ==========================================
	// 8. BANK MANUAL BALANCE ENDPOINTS
	// ==========================================

	@GetMapping("/bank-manual-balance/list")
	public ResponseEntity<List<Map<String, Object>>> getBankManualBalanceList() {
		String sql = "SELECT bmb.Id as id, bmb.BankAccountId as bankAccountId, coa.AccountTitle as accountTitle, " +
				"bmb.BalanceDate as balanceDate, bmb.ManualBalance as manualBalance, bmb.BalanceTime as balanceTime, " +
				"bmb.Source as sourceBy, bmb.ConfirmBy as confirmedBy, bmb.Remarks as remarks " +
				"FROM BankManualBalance bmb " +
				"LEFT JOIN ChartofAccount coa ON bmb.BankAccountId = coa.Id " +
				"ORDER BY bmb.Id DESC";
		try {
			List<Map<String, Object>> list = jdbcTemplate.queryForList(sql);
			return ResponseEntity.ok(list);
		} catch (Exception e) {
			try {
				String altSql = "SELECT bmb.Id as id, bmb.ChartOfAccountId as bankAccountId, coa.AccountTitle as accountTitle, " +
						"bmb.BalanceDate as balanceDate, bmb.ManualBalance as manualBalance, bmb.BalanceTime as balanceTime, " +
						"bmb.Source as sourceBy, bmb.ConfirmBy as confirmedBy, bmb.Remarks as remarks " +
						"FROM tbl_BankManualBalance bmb " +
						"LEFT JOIN ChartofAccount coa ON bmb.ChartOfAccountId = coa.Id " +
						"ORDER BY bmb.Id DESC";
				return ResponseEntity.ok(jdbcTemplate.queryForList(altSql));
			} catch (Exception ex) {
				return ResponseEntity.ok(new ArrayList<>());
			}
		}
	}

	@PostMapping("/bank-manual-balance/save")
	public ResponseEntity<?> saveBankManualBalance(@RequestBody Map<String, Object> req) {
		try {
			Integer id = req.get("id") != null ? Integer.parseInt(req.get("id").toString()) : null;
			Integer bankAccountId = req.get("bankAccountId") != null ? Integer.parseInt(req.get("bankAccountId").toString()) : 0;
			String balanceDate = (String) req.getOrDefault("balanceDate", LocalDate.now().toString());
			BigDecimal manualBalance = req.get("manualBalance") != null ? new BigDecimal(req.get("manualBalance").toString()) : BigDecimal.ZERO;
			String balanceTime = (String) req.getOrDefault("balanceTime", "");
			String sourceBy = (String) req.getOrDefault("sourceBy", "");
			String confirmedBy = (String) req.getOrDefault("confirmedBy", "");
			String remarks = (String) req.getOrDefault("remarks", "");

			jdbcTemplate.execute("IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'BankManualBalance') " +
					"CREATE TABLE BankManualBalance (" +
					"Id INT PRIMARY KEY, " +
					"BankAccountId INT, " +
					"BalanceDate VARCHAR(20), " +
					"ManualBalance DECIMAL(18,2), " +
					"BalanceTime VARCHAR(50), " +
					"Source VARCHAR(200), " +
					"ConfirmBy VARCHAR(200), " +
					"Remarks VARCHAR(500), " +
					"EntryDate DATETIME DEFAULT GETDATE())");

			if (id == null || id <= 0) {
				String maxSql = "SELECT COALESCE(MAX(Id), 0) FROM BankManualBalance";
				Integer maxId = jdbcTemplate.queryForObject(maxSql, Integer.class);
				id = (maxId == null ? 0 : maxId) + 1;

				String insertSql = "INSERT INTO BankManualBalance (Id, BankAccountId, BalanceDate, ManualBalance, BalanceTime, Source, ConfirmBy, Remarks) " +
						"VALUES (?, ?, ?, ?, ?, ?, ?, ?)";
				jdbcTemplate.update(insertSql, id, bankAccountId, balanceDate, manualBalance, balanceTime, sourceBy, confirmedBy, remarks);
			} else {
				String updateSql = "UPDATE BankManualBalance SET BankAccountId = ?, BalanceDate = ?, ManualBalance = ?, BalanceTime = ?, Source = ?, ConfirmBy = ?, Remarks = ? WHERE Id = ?";
				jdbcTemplate.update(updateSql, bankAccountId, balanceDate, manualBalance, balanceTime, sourceBy, confirmedBy, remarks, id);
			}

			Map<String, Object> resp = new HashMap<>();
			resp.put("success", true);
			resp.put("id", id);
			resp.put("message", "Bank Manual Balance saved successfully");
			return ResponseEntity.ok(resp);
		} catch (Exception e) {
			return ResponseEntity.badRequest().body("Error saving Bank Manual Balance: " + e.getMessage());
		}
	}

	@DeleteMapping("/bank-manual-balance/{id}")
	public ResponseEntity<?> deleteBankManualBalance(@PathVariable("id") Integer id) {
		try {
			jdbcTemplate.update("DELETE FROM BankManualBalance WHERE Id = ?", id);
			Map<String, Object> resp = new HashMap<>();
			resp.put("success", true);
			resp.put("message", "Bank Manual Balance deleted successfully");
			return ResponseEntity.ok(resp);
		} catch (Exception e) {
			return ResponseEntity.badRequest().body("Error deleting Bank Manual Balance: " + e.getMessage());
		}
	}
}
