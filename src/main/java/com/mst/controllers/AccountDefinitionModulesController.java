package com.mst.controllers;

import java.math.BigDecimal;
import java.time.LocalDate;
import java.util.ArrayList;
import java.util.HashSet;
import java.util.List;
import java.util.Set;
import java.util.stream.Collectors;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.ModelAttribute;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;

import com.mst.models.AcLookUp;
import com.mst.models.AccountOpeningBalance;
import com.mst.models.COAAllocation;
import com.mst.models.ChartofAccount;
import com.mst.models.ChequeDetail;
import com.mst.models.Company;
import com.mst.models.CustomerGroup;
import com.mst.models.SupplierCustomer;
import com.mst.models.SupplierCustomerType;
import com.mst.models.UserAccount;
import com.mst.repositories.IAccountOpeningBalanceRepository;
import com.mst.repositories.IAccountsCustomGroupRepository;
import com.mst.repositories.IAcLookUpRepository;
import com.mst.repositories.ICOAAllocationRepository;
import com.mst.repositories.IChequeDetailRepository;
import com.mst.repositories.ICompanyRepository;
import com.mst.repositories.IUserAccountRepository;
import com.mst.models.CheqBookDetail;
import com.mst.models.CheqBookHeader;
import com.mst.services.CheqBookRegistrationService;
import com.mst.serviceInterface.IChartofAccountService;
import com.mst.serviceInterface.ISupplierCustomerService;
import com.mst.serviceInterface.IAccountCustomGroupService;
import org.springframework.web.bind.annotation.ResponseBody;
import org.springframework.web.bind.annotation.RequestBody;
import java.util.Map;
import java.util.HashMap;

/**
 * Controller handling full CRUD and Business Logic for Account Definition Modules 3 to 8:
 * - Module 3: Define Supplier / Customer (/accounts/supplier)
 * - Module 4: Cheque Book Registration (/accounts/cheque_book)
 * - Module 5: User Chart Of Account Management (/accounts/user-coa-management)
 * - Module 6: Account Allocation (/accounts/allocation)
 * - Module 7: Account Opening Balance (/accounts/opening_balance)
 * - Module 8: Account Custom Group (/accounts/custom_group)
 */
@Controller
@RequestMapping("/accounts")
public class AccountDefinitionModulesController {

	@Autowired
	private ISupplierCustomerService supplierCustomerService;
	@Autowired
	private IChartofAccountService chartofAccountService;
	@Autowired
	private CheqBookRegistrationService cheqBookRegistrationService;
	@Autowired
	private IChequeDetailRepository chequeDetailRepository;
	@Autowired
	private IUserAccountRepository userAccountRepository;
	@Autowired
	private ICOAAllocationRepository coaAllocationRepository;
	@Autowired
	private ICompanyRepository companyRepository;
	@Autowired
	private IAccountOpeningBalanceRepository accountOpeningBalanceRepository;
	@Autowired
	private IAccountsCustomGroupRepository accountsCustomGroupRepository;
	@Autowired
	private IAcLookUpRepository acLookUpRepository;
	@Autowired
	private IAccountCustomGroupService accountCustomGroupService;
	@Autowired
	private org.springframework.jdbc.core.JdbcTemplate jdbcTemplate;

	// ==========================================
	// 3. DEFINE SUPPLIER / CUSTOMER (/accounts/supplier)
	// ==========================================

	@GetMapping({"/supplier", "/customer"})
	public String viewSupplierCustomer(
			@RequestParam(value = "customerTypeId", required = false) Integer customerTypeId,
			Model model) {

		List<SupplierCustomer> parties = supplierCustomerService.getByCustomerTypeId(customerTypeId);
		List<SupplierCustomerType> types = supplierCustomerService.getAllTypes();
		List<CustomerGroup> groups = supplierCustomerService.getAllGroups();
		List<ChartofAccount> glAccounts = chartofAccountService.getAllAccounts();

		model.addAttribute("activeMenu", "accounts");
		model.addAttribute("parties", parties != null ? parties : new ArrayList<>());
		model.addAttribute("types", types != null ? types : new ArrayList<>());
		model.addAttribute("groups", groups != null ? groups : new ArrayList<>());
		model.addAttribute("glAccounts", glAccounts != null ? glAccounts : new ArrayList<>());
		model.addAttribute("selCustomerTypeId", customerTypeId);
		model.addAttribute("party", new SupplierCustomer());

		return "accounts/supplier";
	}

	@GetMapping("/supplier/edit/{id}")
	public String editSupplierCustomer(@PathVariable("id") int id, Model model) {
		SupplierCustomer party = supplierCustomerService.getById(id);
		if (party == null) {
			return "redirect:/accounts/supplier";
		}
		List<SupplierCustomer> parties = supplierCustomerService.getAll();
		List<SupplierCustomerType> types = supplierCustomerService.getAllTypes();
		List<CustomerGroup> groups = supplierCustomerService.getAllGroups();
		List<ChartofAccount> glAccounts = chartofAccountService.getAllAccounts();

		model.addAttribute("activeMenu", "accounts");
		model.addAttribute("parties", parties);
		model.addAttribute("types", types);
		model.addAttribute("groups", groups);
		model.addAttribute("glAccounts", glAccounts);
		model.addAttribute("party", party);

		return "accounts/supplier";
	}

	@PostMapping("/supplier/save")
	public String saveSupplierCustomer(@ModelAttribute("party") SupplierCustomer party) {
		supplierCustomerService.addOrUpdate(party);
		return "redirect:/accounts/supplier";
	}

	@GetMapping("/supplier/delete/{id}")
	public String deleteSupplierCustomer(@PathVariable("id") int id) {
		supplierCustomerService.delete(id);
		return "redirect:/accounts/supplier";
	}

	// ==========================================
	// 4. CHEQUE BOOK REGISTRATION (/accounts/cheque_book)
	// ==========================================

	@GetMapping("/cheque_book")
	public String viewChequeBook(
			@RequestParam(value = "headerId", required = false) Long headerId,
			Model model) {

		List<ChartofAccount> bankAccounts = cheqBookRegistrationService.getBankAccounts();
		List<CheqBookHeader> registeredHeaders = cheqBookRegistrationService.getAllHeaders();
		List<CheqBookDetail> selectedDetails = new ArrayList<>();

		if (headerId != null && headerId > 0) {
			selectedDetails = cheqBookRegistrationService.getDetailsByHeaderId(headerId);
		} else if (!registeredHeaders.isEmpty()) {
			headerId = registeredHeaders.get(0).getId();
			selectedDetails = cheqBookRegistrationService.getDetailsByHeaderId(headerId);
		}

		CheqBookHeader chequeForm = new CheqBookHeader();
		chequeForm.setDocNo(cheqBookRegistrationService.getNextDocNo());
		chequeForm.setDocDate(LocalDate.now());

		model.addAttribute("activeMenu", "accounts");
		model.addAttribute("bankAccounts", bankAccounts != null ? bankAccounts : new ArrayList<>());
		model.addAttribute("registeredHeaders", registeredHeaders != null ? registeredHeaders : new ArrayList<>());
		model.addAttribute("selectedDetails", selectedDetails != null ? selectedDetails : new ArrayList<>());
		model.addAttribute("selHeaderId", headerId);
		model.addAttribute("chequeForm", chequeForm);

		return "accounts/cheque_book";
	}

	@PostMapping("/cheque_book/save")
	public String saveChequeBook(@ModelAttribute("chequeForm") CheqBookHeader chequeForm, Model model) {
		String result = cheqBookRegistrationService.saveChequeBook(chequeForm);
		if ("SUCCESS".equalsIgnoreCase(result)) {
			return "redirect:/accounts/cheque_book";
		}
		model.addAttribute("errorMessage", result);
		return viewChequeBook(null, model);
	}

	@GetMapping("/cheque_book/details/{headerId}")
	@ResponseBody
	public List<CheqBookDetail> getChequeBookDetails(@PathVariable("headerId") Long headerId) {
		return cheqBookRegistrationService.getDetailsByHeaderId(headerId);
	}

	@PostMapping("/cheque_book/status")
	@ResponseBody
	public java.util.Map<String, Object> updateChequeStatus(
			@RequestParam("detailId") Long detailId,
			@RequestParam("status") String status,
			@RequestParam(value = "remarks", required = false) String remarks) {
		boolean updated = cheqBookRegistrationService.updateChequeStatus(detailId, status, remarks);
		java.util.Map<String, Object> res = new java.util.HashMap<>();
		res.put("success", updated);
		return res;
	}

	// ==========================================
	// 5. USER CHART OF ACCOUNT MANAGEMENT (/accounts/user-coa-management)
	// ==========================================

	@GetMapping("/user-coa-management")
	public String viewUserCoaManagement(
			@RequestParam(value = "userId", required = false, defaultValue = "0") Integer userId,
			Model model) {

		List<UserAccount> users = userAccountRepository.findAllByOrderByUserName();
		List<ChartofAccount> allAccounts = chartofAccountService.getAllAccounts();
		Set<String> allocatedAccountCodes = new HashSet<>();

		if (userId != null && userId > 0) {
			try {
				List<String> codes = jdbcTemplate.queryForList(
						"SELECT AccountCode FROM UserChartOfAccount WHERE UserId = ?", String.class, userId);
				if (codes != null && !codes.isEmpty()) {
					allocatedAccountCodes.addAll(codes);
				}
			} catch (Exception e) {
				try {
					jdbcTemplate.execute("IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'UserChartOfAccount') " +
							"CREATE TABLE UserChartOfAccount (UserId INT, AccountCode VARCHAR(50), PRIMARY KEY (UserId, AccountCode))");
					List<String> codes = jdbcTemplate.queryForList(
							"SELECT AccountCode FROM UserChartOfAccount WHERE UserId = ?", String.class, userId);
					if (codes != null && !codes.isEmpty()) {
						allocatedAccountCodes.addAll(codes);
					}
				} catch (Exception ignored) {}
			}
		}

		model.addAttribute("activeMenu", "accounts");
		model.addAttribute("users", users);
		model.addAttribute("selUserId", userId);
		model.addAttribute("allAccounts", allAccounts);
		model.addAttribute("allocatedAccountCodes", allocatedAccountCodes);

		return "accounts/user_coa_management";
	}

	@PostMapping("/user-coa-management/save")
	public String saveUserCoaManagement(
			@RequestParam("userId") Integer userId,
			@RequestParam(value = "allocatedAccountCodes", required = false) List<String> allocatedAccountCodes) {

		if (userId != null && userId > 0) {
			try {
				jdbcTemplate.execute("IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'UserChartOfAccount') " +
						"CREATE TABLE UserChartOfAccount (UserId INT, AccountCode VARCHAR(50), PRIMARY KEY (UserId, AccountCode))");
				jdbcTemplate.update("DELETE FROM UserChartOfAccount WHERE UserId = ?", userId);
				if (allocatedAccountCodes != null && !allocatedAccountCodes.isEmpty()) {
					for (String code : allocatedAccountCodes) {
						if (code != null && !code.isBlank()) {
							jdbcTemplate.update("INSERT INTO UserChartOfAccount (UserId, AccountCode) VALUES (?, ?)", userId, code.trim());
						}
					}
				}
			} catch (Exception e) {
				org.slf4j.LoggerFactory.getLogger(AccountDefinitionModulesController.class)
						.error("Error saving user COA allocation: {}", e.getMessage());
			}
		}

		return "redirect:/accounts/user-coa-management?userId=" + userId;
	}

	// ==========================================
	// 6. ACCOUNT ALLOCATION (/accounts/allocation)
	// ==========================================

	@GetMapping("/allocation")
	public String viewAccountAllocation(
			@RequestParam(value = "companyId", required = false, defaultValue = "0") Integer companyId,
			Model model) {

		List<Company> companies = companyRepository.findAll();
		List<ChartofAccount> allAccounts = chartofAccountService.getAllAccounts();
		Set<Integer> allocatedAccountIds = new HashSet<>();

		if (companyId != null && companyId > 0) {
			List<COAAllocation> allocs = coaAllocationRepository.findByCompanyId(companyId);
			if (allocs != null) {
				allocatedAccountIds = allocs.stream()
						.map(COAAllocation::getChartofAccountId)
						.filter(id -> id != null)
						.collect(Collectors.toSet());
			}
		}

		model.addAttribute("activeMenu", "accounts");
		model.addAttribute("companies", companies);
		model.addAttribute("selCompanyId", companyId);
		model.addAttribute("allAccounts", allAccounts);
		model.addAttribute("allocatedAccountIds", allocatedAccountIds);

		return "accounts/account_allocation";
	}

	@PostMapping("/allocation/save")
	public String saveAccountAllocation(
			@RequestParam("companyId") Integer companyId,
			@RequestParam(value = "allocatedAccountIds", required = false) List<Integer> allocatedAccountIds) {

		if (companyId != null && companyId > 0) {
			coaAllocationRepository.deleteByCompanyId(companyId);

			if (allocatedAccountIds != null && !allocatedAccountIds.isEmpty()) {
				for (Integer coaId : allocatedAccountIds) {
					COAAllocation ca = new COAAllocation();
					ca.setCompanyId(companyId);
					ca.setChartofAccountId(coaId);
					ca.setIsActive(true);
					coaAllocationRepository.save(ca);
				}
			}
		}

		return "redirect:/accounts/allocation?companyId=" + companyId;
	}

	// ==========================================
	// 7. ACCOUNT OPENING BALANCE (/accounts/opening_balance)
	// ==========================================

	@GetMapping("/opening_balance")
	public String viewOpeningBalance(Model model) {
		OpeningBalanceForm form = new OpeningBalanceForm();
		List<OpeningBalanceRow> rows = new ArrayList<>();

		try {
			String sql = "SELECT coa.ID as id, coa.AccountCode as accountCode, coa.AccountTitle as accountTitle, " +
					"ISNULL(aob.YearObDebit, 0) as yearObDebit, ISNULL(aob.YearObCredit, 0) as yearObCredit, " +
					"ISNULL(aob.OpeningBalance, 0) as openingBalance " +
					"FROM ChartofAccount coa " +
					"LEFT JOIN AccountsOpeningBalances aob ON (aob.ChartOfAccountId = coa.ID OR aob.AccountCode = coa.AccountCode) " +
					"WHERE (coa.AccountGroup = 'Detail' OR coa.Account_Level >= 4) " +
					"ORDER BY coa.AccountCode";

			List<java.util.Map<String, Object>> dbRows = jdbcTemplate.queryForList(sql);
			for (java.util.Map<String, Object> r : dbRows) {
				OpeningBalanceRow row = new OpeningBalanceRow();
				row.setAccountCode(r.get("accountCode") != null ? r.get("accountCode").toString() : "");
				row.setAccountTitle(r.get("accountTitle") != null ? r.get("accountTitle").toString() : "");

				double debit = r.get("yearObDebit") != null ? ((Number) r.get("yearObDebit")).doubleValue() : 0.0;
				double credit = r.get("yearObCredit") != null ? ((Number) r.get("yearObCredit")).doubleValue() : 0.0;
				double opBal = r.get("openingBalance") != null ? ((Number) r.get("openingBalance")).doubleValue() : 0.0;

				if (debit == 0.0 && credit == 0.0 && opBal != 0.0) {
					if (opBal > 0) debit = opBal;
					else credit = Math.abs(opBal);
				}

				row.setOpeningDebit(debit);
				row.setOpeningCredit(credit);
				rows.add(row);
			}
		} catch (Exception e) {
			List<ChartofAccount> detailAccounts = chartofAccountService.getDetailAccounts();
			for (ChartofAccount acc : detailAccounts) {
				OpeningBalanceRow r = new OpeningBalanceRow();
				r.setAccountCode(acc.getAccountCode());
				r.setAccountTitle(acc.getAccountTitle());
				r.setOpeningDebit(0.0);
				r.setOpeningCredit(0.0);
				rows.add(r);
			}
		}

		form.setRows(rows);
		model.addAttribute("activeMenu", "accounts");
		model.addAttribute("form", form);

		return "accounts/opening_balance";
	}

	@PostMapping("/opening_balance/save")
	public String saveOpeningBalance(@ModelAttribute("form") OpeningBalanceForm form) {
		if (form != null && form.getRows() != null) {
			for (OpeningBalanceRow r : form.getRows()) {
				if (r.getAccountCode() != null && !r.getAccountCode().trim().isEmpty()) {
					AccountOpeningBalance ob = accountOpeningBalanceRepository.findByAccountCode(r.getAccountCode());
					if (ob == null) {
						Integer maxId = accountOpeningBalanceRepository.findMaxId();
						ob = new AccountOpeningBalance();
						ob.setId(maxId != null ? maxId + 1 : 1);
						ob.setAccountCode(r.getAccountCode());
					}
					ob.setYearObDebit(r.getOpeningDebit() != null ? r.getOpeningDebit() : 0.0);
					ob.setYearObCredit(r.getOpeningCredit() != null ? r.getOpeningCredit() : 0.0);
					accountOpeningBalanceRepository.save(ob);
				}
			}
		}
		return "redirect:/accounts/opening_balance";
	}

	@PostMapping("/opening_balance/update-single")
	@ResponseBody
	public java.util.Map<String, Object> updateSingleOpeningBalance(
			@RequestParam("accountCode") String accountCode,
			@RequestParam(value = "debitAmount", defaultValue = "0.0") Double debitAmount,
			@RequestParam(value = "creditAmount", defaultValue = "0.0") Double creditAmount) {
		java.util.Map<String, Object> res = new java.util.HashMap<>();
		try {
			AccountOpeningBalance ob = accountOpeningBalanceRepository.findByAccountCode(accountCode);
			if (ob == null) {
				Integer maxId = accountOpeningBalanceRepository.findMaxId();
				ob = new AccountOpeningBalance();
				ob.setId(maxId != null ? maxId + 1 : 1);
				ob.setAccountCode(accountCode);
			}
			ob.setYearObDebit(debitAmount != null ? debitAmount : 0.0);
			ob.setYearObCredit(creditAmount != null ? creditAmount : 0.0);
			accountOpeningBalanceRepository.save(ob);
			res.put("success", true);
		} catch (Exception e) {
			res.put("success", false);
			res.put("message", e.getMessage());
		}
		return res;
	}

	@GetMapping("/opening_balance/print")
	@ResponseBody
	public java.util.Map<String, Object> printOpeningBalance(
			@RequestParam(value = "organizationId", required = false, defaultValue = "1") Integer organizationId,
			@RequestParam(value = "companyId", required = false, defaultValue = "1") Integer companyId,
			@RequestParam(value = "financialYearId", required = false, defaultValue = "1") Integer financialYearId) {

		java.util.Map<String, Object> res = new java.util.HashMap<>();
		org.slf4j.Logger logger = org.slf4j.LoggerFactory.getLogger(AccountDefinitionModulesController.class);

		try {
			List<java.util.Map<String, Object>> data = accountOpeningBalanceRepository
					.getOpeningBalanceReportData(organizationId, companyId, financialYearId);

			int resultCount = (data != null) ? data.size() : 0;

			logger.info("[129-PRINT] Report: 129-OpeningBalanceRpt.rpt, StoredProc: Sp_AccountsOpeningBalance_Slip, OrgId: {}, CompId: {}, FinancialYrId: {}, ResultCount: {}",
					organizationId, companyId, financialYearId, resultCount);

			if (data == null || data.isEmpty()) {
				res.put("success", false);
				res.put("resultCount", 0);
				res.put("message", "Record Not Found For Display");
				return res;
			}

			res.put("success", true);
			res.put("resultCount", resultCount);
			res.put("message", "Success");
			res.put("printUrl", "/accounts/opening_balance/print-view?organizationId=" + organizationId
					+ "&companyId=" + companyId + "&financialYearId=" + financialYearId);
		} catch (Exception e) {
			logger.error("[129-PRINT ERROR] Error executing Sp_AccountsOpeningBalance_Slip: {}", e.getMessage(), e);
			res.put("success", false);
			res.put("resultCount", 0);
			res.put("message", "Database Error: " + e.getMessage());
		}
		return res;
	}

	@GetMapping("/opening_balance/print-view")
	public String viewOpeningBalanceReport(
			@RequestParam(value = "organizationId", required = false, defaultValue = "1") Integer organizationId,
			@RequestParam(value = "companyId", required = false, defaultValue = "1") Integer companyId,
			@RequestParam(value = "financialYearId", required = false, defaultValue = "1") Integer financialYearId,
			Model model) {

		List<java.util.Map<String, Object>> reportData = new ArrayList<>();
		try {
			List<java.util.Map<String, Object>> rawData = accountOpeningBalanceRepository
					.getOpeningBalanceReportData(organizationId, companyId, financialYearId);
			if (rawData != null) {
				reportData = rawData;
			}
		} catch (Exception e) {
			org.slf4j.LoggerFactory.getLogger(AccountDefinitionModulesController.class)
					.error("[129-PRINT VIEW ERROR] Error: {}", e.getMessage());
		}

		double totalDebit = 0.0;
		double totalCredit = 0.0;
		String companyName = "Golden Ace Rice Mills (Pvt) Ltd.";
		String companyAddress = "Factory / Head Office Address";
		String reportingRemarks = "System Generated Opening Balance Slip";

		if (!reportData.isEmpty()) {
			java.util.Map<String, Object> firstRow = reportData.get(0);
			if (firstRow.containsKey("CompAddress") && firstRow.get("CompAddress") != null) {
				companyAddress = firstRow.get("CompAddress").toString();
			}
			if (firstRow.containsKey("ReportingRemarks") && firstRow.get("ReportingRemarks") != null) {
				reportingRemarks = firstRow.get("ReportingRemarks").toString();
			}
			for (java.util.Map<String, Object> row : reportData) {
				if (row.get("YearObDebit") != null) {
					totalDebit += ((Number) row.get("YearObDebit")).doubleValue();
				}
				if (row.get("YearObCredit") != null) {
					totalCredit += ((Number) row.get("YearObCredit")).doubleValue();
				}
			}
		}

		try {
			Company comp = companyRepository.findById(companyId).orElse(null);
			if (comp != null && comp.getCompanyName() != null) {
				companyName = comp.getCompanyName();
			}
		} catch (Exception ignored) {}

		model.addAttribute("organizationId", organizationId);
		model.addAttribute("companyId", companyId);
		model.addAttribute("financialYearId", financialYearId);
		model.addAttribute("reportData", reportData);
		model.addAttribute("totalDebit", totalDebit);
		model.addAttribute("totalCredit", totalCredit);
		model.addAttribute("companyName", companyName);
		model.addAttribute("companyAddress", companyAddress);
		model.addAttribute("reportingRemarks", reportingRemarks);

		return "accounts/reports/129_opening_balance_report";
	}

	@GetMapping({"/opening_balance/pdf", "/opening_balance/129-OpeningBalanceRpt.pdf"})
	public org.springframework.http.ResponseEntity<byte[]> exportOpeningBalancePdf(
			@RequestParam(value = "organizationId", required = false, defaultValue = "1") Integer organizationId,
			@RequestParam(value = "companyId", required = false, defaultValue = "1") Integer companyId,
			@RequestParam(value = "financialYearId", required = false, defaultValue = "1") Integer financialYearId) {

		try {
			List<java.util.Map<String, Object>> reportData = accountOpeningBalanceRepository
					.getOpeningBalanceReportData(organizationId, companyId, financialYearId);

			if (reportData == null) {
				reportData = new ArrayList<>();
			}

			String companyName = "Golden Ace Rice Mills (Pvt) Ltd.";
			String companyAddress = "Factory / Head Office Address";
			try {
				Company comp = companyRepository.findById(companyId).orElse(null);
				if (comp != null && comp.getCompanyName() != null) {
					companyName = comp.getCompanyName();
				}
			} catch (Exception ignored) {}

			if (!reportData.isEmpty()) {
				java.util.Map<String, Object> firstRow = reportData.get(0);
				if (firstRow.containsKey("CompAddress") && firstRow.get("CompAddress") != null) {
					companyAddress = firstRow.get("CompAddress").toString();
				}
			}

			java.util.Map<String, Object> params = new java.util.HashMap<>();
			params.put("CompanyName", companyName);
			params.put("CompanyAddress", companyAddress);

			net.sf.jasperreports.engine.data.JRMapCollectionDataSource dataSource =
					new net.sf.jasperreports.engine.data.JRMapCollectionDataSource((java.util.Collection) reportData);

			java.io.InputStream reportStream = getClass().getResourceAsStream("/jasper/129-OpeningBalanceRpt.jrxml");
			if (reportStream == null) {
				reportStream = getClass().getResourceAsStream("/jasper/129-OpeningBalanceRpt.jasper");
			}

			byte[] pdfBytes;
			if (reportStream != null) {
				net.sf.jasperreports.engine.JasperReport jasperReport =
						net.sf.jasperreports.engine.JasperCompileManager.compileReport(reportStream);
				net.sf.jasperreports.engine.JasperPrint jasperPrint =
						net.sf.jasperreports.engine.JasperFillManager.fillReport(jasperReport, params, dataSource);
				pdfBytes = net.sf.jasperreports.engine.JasperExportManager.exportReportToPdf(jasperPrint);
			} else {
				pdfBytes = "Report template 129-OpeningBalanceRpt.jrxml not found".getBytes(java.nio.charset.StandardCharsets.UTF_8);
			}

			org.springframework.http.HttpHeaders headers = new org.springframework.http.HttpHeaders();
			headers.setContentType(org.springframework.http.MediaType.APPLICATION_PDF);
			headers.setContentDispositionFormData("inline", "129-OpeningBalanceRpt.pdf");

			return new org.springframework.http.ResponseEntity<>(pdfBytes, headers, org.springframework.http.HttpStatus.OK);
		} catch (Exception e) {
			org.slf4j.LoggerFactory.getLogger(AccountDefinitionModulesController.class)
					.error("[129-PRINT PDF ERROR] Jasper export failed: {}", e.getMessage(), e);
			return org.springframework.http.ResponseEntity.status(org.springframework.http.HttpStatus.INTERNAL_SERVER_ERROR)
					.body(("Error generating PDF report: " + e.getMessage()).getBytes(java.nio.charset.StandardCharsets.UTF_8));
		}
	}

	// ==========================================
	// 8. ACCOUNT CUSTOM GROUP (/accounts/custom_group)
	// ==========================================

	@GetMapping("/custom_group")
	public String viewCustomGroup(Model model) {
		List<AcLookUp> groups = accountCustomGroupService.getAllGroups();
		model.addAttribute("activeMenu", "accounts");
		model.addAttribute("groups", groups != null ? groups : new ArrayList<>());
		model.addAttribute("accountTypes", accountCustomGroupService.getAccountTypes());
		model.addAttribute("parentAccounts", accountCustomGroupService.getParentAccounts(0));
		model.addAttribute("group", new AcLookUp());
		return "accounts/custom_group";
	}

	@PostMapping("/custom_group/save")
	@ResponseBody
	public Map<String, Object> saveCustomGroup(@RequestParam(value = "id", required = false) Integer id,
	                                           @RequestParam("acLookUpsDescription") String acLookUpsDescription) {
		Map<String, Object> response = new HashMap<>();
		try {
			if (acLookUpsDescription == null || acLookUpsDescription.trim().isEmpty()) {
				response.put("success", false);
				response.put("message", "Group Name Required");
				return response;
			}
			AcLookUp group = new AcLookUp();
			group.setId(id != null && id > 0 ? id : null);
			group.setAcLookUpsDescription(acLookUpsDescription.trim());
			accountCustomGroupService.addOrUpdateGroup(group);
			response.put("success", true);
			response.put("message", id != null && id > 0 ? "Record Update Successfully" : "Record Saved Successfully");
		} catch (Exception ex) {
			response.put("success", false);
			response.put("message", ex.getMessage());
		}
		return response;
	}

	@PostMapping("/custom_group/delete-group/{id}")
	@ResponseBody
	public Map<String, Object> deleteCustomGroup(@PathVariable("id") int id) {
		Map<String, Object> response = new HashMap<>();
		try {
			accountCustomGroupService.deleteGroup(id);
			response.put("success", true);
			response.put("message", "Group deleted successfully");
		} catch (Exception ex) {
			response.put("success", false);
			response.put("message", ex.getMessage());
		}
		return response;
	}

	@GetMapping("/custom_group/show")
	@ResponseBody
	public Map<String, Object> showCustomGroupData(
			@RequestParam("groupId") int groupId,
			@RequestParam(value = "accountTypeId", required = false) Integer accountTypeId,
			@RequestParam(value = "parentAccountId", required = false) Integer parentAccountId) {
		Map<String, Object> result = new HashMap<>();
		result.put("unallocated", accountCustomGroupService.getUnAllocatedData(groupId, accountTypeId, parentAccountId));
		result.put("allocated", accountCustomGroupService.getAllocatedData(groupId, accountTypeId, parentAccountId));
		return result;
	}

	@PostMapping("/custom_group/allocate")
	@ResponseBody
	public Map<String, Object> allocateAccounts(
			@RequestParam("groupId") int groupId,
			@RequestBody List<Integer> chartOfAccountIds) {
		Map<String, Object> response = new HashMap<>();
		try {
			accountCustomGroupService.allocateAccounts(groupId, chartOfAccountIds);
			response.put("success", true);
			response.put("message", "Saved Successfully");
		} catch (Exception ex) {
			response.put("success", false);
			response.put("message", ex.getMessage());
		}
		return response;
	}

	@PostMapping("/custom_group/unallocate")
	@ResponseBody
	public Map<String, Object> unallocateAccounts(
			@RequestParam("groupId") int groupId,
			@RequestBody List<Integer> chartOfAccountIds) {
		Map<String, Object> response = new HashMap<>();
		try {
			accountCustomGroupService.unAllocateAccounts(groupId, chartOfAccountIds);
			response.put("success", true);
			response.put("message", "Record Remove successfully");
		} catch (Exception ex) {
			response.put("success", false);
			response.put("message", ex.getMessage());
		}
		return response;
	}

	@GetMapping("/custom_group/parent-accounts")
	@ResponseBody
	public List<ChartofAccount> getParentAccounts(@RequestParam(value = "accountTypeId", required = false) Integer accountTypeId) {
		return accountCustomGroupService.getParentAccounts(accountTypeId);
	}

	// Form Helper Classes for Batch Opening Balance Submission
	public static class OpeningBalanceForm {
		private List<OpeningBalanceRow> rows = new ArrayList<>();
		public List<OpeningBalanceRow> getRows() { return rows; }
		public void setRows(List<OpeningBalanceRow> rows) { this.rows = rows; }
	}

	public static class OpeningBalanceRow {
		private String accountCode;
		private String accountTitle;
		private Double openingDebit = 0.0;
		private Double openingCredit = 0.0;

		public String getAccountCode() { return accountCode; }
		public void setAccountCode(String accountCode) { this.accountCode = accountCode; }
		public String getAccountTitle() { return accountTitle; }
		public void setAccountTitle(String accountTitle) { this.accountTitle = accountTitle; }
		public Double getOpeningDebit() { return openingDebit; }
		public void setOpeningDebit(Double openingDebit) { this.openingDebit = openingDebit; }
		public Double getOpeningCredit() { return openingCredit; }
		public void setOpeningCredit(Double openingCredit) { this.openingCredit = openingCredit; }
	}
}
