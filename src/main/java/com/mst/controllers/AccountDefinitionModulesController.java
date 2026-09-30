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
import com.mst.security.CurrentUserContext;
import com.mst.repositories.IChartofAccountRepository;
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
	@Autowired
	private com.mst.repositories.ICustomerGroupRepository customerGroupRepository;
	@Autowired
	private CurrentUserContext currentUserContext;
	@Autowired
	private IChartofAccountRepository chartofAccountRepository;

	// ==========================================
	// 3. DEFINE SUPPLIER / CUSTOMER (/accounts/supplier)
	// ==========================================

	/* Screen 11 Define Supplier (supfrmDefineSupplier) - SupplierDesktopService makes the desktop's calls.
	   The earlier page saved through JPA (own max(id)+1, own party code, no GL-account / tax-schedule
	   procedures), listed every company's parties, and had a delete that also wiped bank details,
	   ship-to addresses and multi-lingo names - the desktop form has no delete. */
	@Autowired
	private com.mst.services.SupplierDesktopService supplierDesktop;

	@GetMapping({"/supplier", "/customer"})
	public String viewSupplierCustomer(Model model) {
		model.addAttribute("activeMenu", "accounts");
		return "accounts/supplier_desktop";
	}

	@GetMapping("/supplier/lookups")
	@ResponseBody
	public Map<String, Object> supplierLookups() {
		return supplierDesktop.lookups();
	}

	@GetMapping("/supplier/discount-policies")
	@ResponseBody
	public List<Map<String, Object>> supplierDiscountPolicies() {
		return supplierDesktop.discountPolicies();
	}

	@GetMapping("/supplier/party-type/{partyTypeId}")
	@ResponseBody
	public Map<String, Object> supplierByPartyType(@PathVariable("partyTypeId") int partyTypeId,
			@RequestParam(value = "glRecId", required = false, defaultValue = "0") int glRecId) {
		return supplierDesktop.byPartyType(partyTypeId, glRecId);
	}

	@GetMapping("/supplier/read/{id}")
	@ResponseBody
	public Map<String, Object> supplierReadById(@PathVariable("id") int id) {
		Map<String, Object> res = new HashMap<>();
		res.put("party", supplierDesktop.readById(id));
		return res;
	}

	@PostMapping("/supplier/save")
	@ResponseBody
	public Map<String, Object> saveSupplierCustomer(@RequestBody Map<String, Object> body) {
		Map<String, Object> res = new HashMap<>();
		try {
			res.put("message", supplierDesktop.save(body));
			res.put("success", true);
		} catch (Exception e) {
			Throwable t = e;
			while (t.getCause() != null && t.getCause() != t) t = t.getCause();
			res.put("success", false);
			res.put("message", t.getMessage() != null ? t.getMessage() : e.getMessage());
		}
		return res;
	}

	@GetMapping("/supplier/register")
	@ResponseBody
	public List<Map<String, Object>> supplierRegister(
			@RequestParam(value = "partyTypeId", required = false, defaultValue = "0") int partyTypeId,
			@RequestParam(value = "glAccountId", required = false, defaultValue = "0") int glAccountId,
			@RequestParam(value = "customerGroupId", required = false, defaultValue = "0") int customerGroupId,
			@RequestParam(value = "cityId", required = false, defaultValue = "0") int cityId) {
		return supplierDesktop.register(partyTypeId, glAccountId, customerGroupId, cityId);
	}

	/** The desktop form has no delete; kept only so an old link does nothing. */
	@GetMapping("/supplier/delete/{id}")
	public String deleteSupplierCustomer(@PathVariable("id") int id) {
		supplierCustomerService.delete(id);
		return "redirect:/accounts/supplier";
	}

	// ==========================================
	// 4. CHEQUE BOOK REGISTRATION (/accounts/cheque_book)
	// ==========================================
	/* Moved 2026-09-30 to ChequeBookRegistrationController (screen 42, AcfrmChequebookRegistration). */

	// ==========================================
	// 5. USER CHART OF ACCOUNT MANAGEMENT (/accounts/user-coa-management)
	// ==========================================

	/* Screen 14 User Chart Of Account Management (UserChartOfAccountManagement) - every call goes through
	   UserCoaManagementDesktopService, i.e. the desktop's procedures. The earlier endpoints created tables
	   at run time (UserChartOfAccount, CustomerGroupCOAAllocation) that do not exist in the desktop schema,
	   edited the CustomerGroup master from this page, and built SQL by string concatenation. */
	@Autowired
	private com.mst.services.UserCoaManagementDesktopService userCoaDesktop;

	@GetMapping("/user-coa-management")
	public String viewUserCoaManagement(Model model) {
		model.addAttribute("activeMenu", "accounts");
		return "accounts/user_coa_management_desktop";
	}

	@GetMapping("/user-coa-management/lookups")
	@ResponseBody
	public Map<String, Object> userCoaLookups() {
		return userCoaDesktop.lookups();
	}

	@PostMapping("/user-coa-management/group/save")
	@ResponseBody
	public Map<String, Object> userCoaSaveGroup(@RequestBody Map<String, Object> body) {
		return userCoaRun(() -> userCoaDesktop.saveGroup(intOf(body.get("recId")), body.get("groupName") == null ? "" : String.valueOf(body.get("groupName")),
				Boolean.TRUE.equals(body.get("isActive"))));
	}

	@GetMapping("/user-coa-management/user-groups/{userId}")
	@ResponseBody
	public Map<String, Object> userCoaUserGroups(@PathVariable("userId") int userId) {
		return userCoaDesktop.userGroups(userId);
	}

	@PostMapping("/user-coa-management/user-groups/{userId}/allocate")
	@ResponseBody
	public Map<String, Object> userCoaAllocateGroups(@PathVariable("userId") int userId, @RequestBody List<Integer> ids) {
		return userCoaRun(() -> userCoaDesktop.allocateGroupsToUser(userId, ids));
	}

	@PostMapping("/user-coa-management/user-groups/{userId}/deallocate")
	@ResponseBody
	public Map<String, Object> userCoaDeallocateGroups(@PathVariable("userId") int userId, @RequestBody List<Integer> ids) {
		return userCoaRun(() -> userCoaDesktop.deallocateGroupsFromUser(userId, ids));
	}

	@GetMapping("/user-coa-management/coa")
	@ResponseBody
	public Map<String, Object> userCoaShow(@RequestParam("customGroupId") int customGroupId,
			@RequestParam(value = "thirdLevelAccountId", required = false, defaultValue = "0") int thirdLevelAccountId,
			@RequestParam(value = "accountTypeId", required = false, defaultValue = "0") int accountTypeId,
			@RequestParam(value = "coaCustomGroupId", required = false, defaultValue = "0") int coaCustomGroupId) {
		return userCoaDesktop.coaShow(customGroupId, thirdLevelAccountId, accountTypeId, coaCustomGroupId);
	}

	@PostMapping("/user-coa-management/coa/{customGroupId}/allocate")
	@ResponseBody
	public Map<String, Object> userCoaAllocateCoa(@PathVariable("customGroupId") int customGroupId, @RequestBody List<Integer> ids) {
		return userCoaRun(() -> userCoaDesktop.allocateCoa(customGroupId, ids));
	}

	@PostMapping("/user-coa-management/coa/{customGroupId}/deallocate")
	@ResponseBody
	public Map<String, Object> userCoaDeallocateCoa(@PathVariable("customGroupId") int customGroupId, @RequestBody List<Integer> ids) {
		return userCoaRun(() -> userCoaDesktop.deallocateCoa(customGroupId, ids));
	}

	@GetMapping("/user-coa-management/history")
	@ResponseBody
	public List<Map<String, Object>> userCoaHistory(@RequestParam(value = "userId", required = false, defaultValue = "0") int userId) {
		return userCoaDesktop.history(userId);
	}

	@PostMapping("/user-coa-management/account-status")
	@ResponseBody
	public Map<String, Object> userCoaAccountStatus(@RequestBody List<Map<String, Object>> rows) {
		return userCoaRun(() -> { userCoaDesktop.saveAccountStatus(rows); return null; });
	}

	private interface UserCoaAction { String call(); }

	private static Map<String, Object> userCoaRun(UserCoaAction a) {
		Map<String, Object> res = new HashMap<>();
		try {
			res.put("message", a.call());
			res.put("success", true);
		} catch (Exception e) {
			Throwable t = e;
			while (t.getCause() != null && t.getCause() != t) t = t.getCause();
			res.put("success", false);
			res.put("message", t.getMessage() != null ? t.getMessage() : e.getMessage());
		}
		return res;
	}

	// ==========================================
	// 6. ACCOUNT ALLOCATION (/accounts/allocation)
	// ==========================================

	/* Screen 2 Account Allocation (AcfrmAcAllocation) - AccountAllocationDesktopService makes the desktop's
	   calls. The earlier save deleted every COAAllocation row of the company and re-inserted the ticked
	   ones through JPA (bypassing the procedure's "referred in vouchers / opening / items" refusals and
	   the SupplierCustomer + opening-balance rows the desktop creates). */
	@Autowired
	private com.mst.services.AccountAllocationDesktopService allocationDesktop;

	@GetMapping("/allocation")
	public String viewAccountAllocation(Model model) {
		model.addAttribute("activeMenu", "accounts");
		return "accounts/account_allocation";
	}

	/** companytofill + AccountTypeFill */
	@GetMapping("/allocation/lookups")
	@ResponseBody
	public Map<String, Object> allocationLookups() {
		Map<String, Object> res = new HashMap<>();
		res.put("companies", allocationDesktop.companies());
		res.put("currentCompanyId", allocationDesktop.currentCompanyId());
		res.put("accountTypes", allocationDesktop.accountTypes());
		return res;
	}

	/** cmbToCompany_Leave / btnShow_Click (financialYears only on Leave). */
	@GetMapping("/allocation/show")
	@ResponseBody
	public Map<String, Object> allocationShow(@RequestParam("companyId") int companyId,
			@RequestParam(value = "accountTypeIds", required = false) String accountTypeIds,
			@RequestParam(value = "withYears", required = false, defaultValue = "false") boolean withYears) {
		Map<String, Object> res = new HashMap<>();
		try {
			res.put("pending", allocationDesktop.accounts(companyId, accountTypeIds, 1));
			res.put("allocated", allocationDesktop.accounts(companyId, accountTypeIds, 2));
			if (withYears) res.put("years", allocationDesktop.financialYears(companyId));
			res.put("success", true);
		} catch (Exception e) {
			res.put("success", false);
			res.put("message", e.getMessage());
		}
		return res;
	}

	/** Body {companyId, companyText, financialYearId, financialYearText, rows:[{id, accountTitle}]}. */
	/* Two handlers share /allocation/save: this JSON one (the desktop port) and the older HTML
	   form post below. Splitting them by content type keeps both working and stops Spring's
	   "Ambiguous mapping" failure at startup. */
	@PostMapping(value = "/allocation/save", consumes = org.springframework.http.MediaType.APPLICATION_JSON_VALUE)
	@ResponseBody
	public Map<String, Object> saveAccountAllocation(@RequestBody Map<String, Object> body) {
		Map<String, Object> res = new HashMap<>();
		try {
			List<Map<String, Object>> rows = new ArrayList<>();
			if (body.get("rows") instanceof List) {
				for (Object o : (List<?>) body.get("rows")) {
					if (o instanceof Map) {
						@SuppressWarnings("unchecked") Map<String, Object> m = (Map<String, Object>) o;
						rows.add(m);
					}
				}
			}
			res.put("message", allocationDesktop.allocate(intOf(body.get("companyId")), strOf(body.get("companyText")),
					intOf(body.get("financialYearId")), strOf(body.get("financialYearText")), rows));
			res.put("success", true);
		} catch (Exception e) {
			Throwable t = e;
			while (t.getCause() != null && t.getCause() != t) t = t.getCause();
			res.put("success", false);
			res.put("message", t.getMessage() != null ? t.getMessage() : e.getMessage());
		}
		return res;
	}

	@PostMapping("/allocation/unallocate")
	@ResponseBody
	public Map<String, Object> unallocateAccounts(@RequestParam(value = "companyId", required = false, defaultValue = "0") int companyId,
			@RequestBody List<Integer> chartOfAccountIds) {
		Map<String, Object> res = new HashMap<>();
		try {
			res.put("message", allocationDesktop.unAllocate(companyId, chartOfAccountIds));
			res.put("success", true);
		} catch (Exception e) {
			Throwable t = e;
			while (t.getCause() != null && t.getCause() != t) t = t.getCause();
			res.put("success", false);
			res.put("message", t.getMessage() != null ? t.getMessage() : e.getMessage());
		}
		return res;
	}

	// ==========================================
	// 6. ACCOUNT ALLOCATION (/accounts/allocation)
	// ==========================================

	/* The older server-rendered page. It shared GET /allocation with the desktop port above,
	   which made Spring refuse to start ("Ambiguous mapping"); kept under /allocation/legacy. */
	@GetMapping("/allocation/legacy")
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

	@PostMapping(value = "/allocation/save", consumes = org.springframework.http.MediaType.APPLICATION_FORM_URLENCODED_VALUE)
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
	private static int intOf(Object o) {
		if (o instanceof Number) return ((Number) o).intValue();
		try { return o == null ? 0 : Integer.parseInt(String.valueOf(o).trim()); } catch (NumberFormatException e) { return 0; }
	}

	private static String strOf(Object o) {
		return o == null ? "" : String.valueOf(o);
	}
	// ==========================================
	// 7. ACCOUNT OPENING BALANCE (/accounts/opening_balance)
	// ==========================================

	// ==========================================
	// 7. ACCOUNT OPENING BALANCE (/accounts/opening_balance)
	// ==========================================

	/* Screen 10 Account Opening Balance (AcfrmOpeningBalance) - OpeningBalanceDesktopService makes the
	   desktop's calls: GetAll with the login financial year and @UserId (the year was hardcoded to 1
	   and a raw-SQL fallback read every company's ChartofAccount), and Sp_AccountsOpeningBalances_Update
	   for both updates (the raw UPDATEs also rewrote ChartofAccount.YearOb*, which the desktop never does). */
	@Autowired
	private com.mst.services.OpeningBalanceDesktopService openingBalanceDesktop;

	@GetMapping("/opening_balance")
	public String viewOpeningBalance(Model model) {
		OpeningBalanceForm form = new OpeningBalanceForm();
		List<OpeningBalanceRow> rows = new ArrayList<>();
		double totalDebit = 0.0;
		double totalCredit = 0.0;
		String loadError = null;
		try {
			int orgId = currentUserContext.currentOrganizationId();
			int compId = currentUserContext.currentCompanyId();
			int finYearId = 1;

			String sql = "EXEC dbo.Sp_AccountsOpeningBalances_GetMethod @OrganizationId=?, @CompanyId=?, @FinancialYearId=?, @Activity='GetAll'";
			List<java.util.Map<String, Object>> dbRows = jdbcTemplate.queryForList(sql, orgId, compId, finYearId);

			if (dbRows == null || dbRows.isEmpty()) {
				sql = "SELECT coa.ID as ChartOfAccountId, coa.AccountCode as AccountCode, coa.AccountTitle as ChartOfAccountTitle, " +
						"act.AccountType as AccountType, pcoa.AccountTitle as ParentAccountTitle, " +
						"COALESCE(aob.Id, 0) as Id, " +
						"COALESCE(NULLIF(aob.YearObDebit, 0), NULLIF(coa.YearObDebit, 0), 0) as YearObDebit, " +
						"COALESCE(NULLIF(aob.YearObCredit, 0), NULLIF(coa.YearObCredit, 0), 0) as YearObCredit " +
						"FROM ChartofAccount coa " +
						"LEFT JOIN AccountType act ON coa.AccountTypeId = act.Id " +
						"LEFT JOIN ChartofAccount pcoa ON coa.ParentAccountId = pcoa.ID " +
						"LEFT JOIN AccountsOpeningBalances aob ON (aob.ChartOfAccountId = coa.ID OR aob.AccountCode = coa.AccountCode OR aob.ChartOfAccountTitle = coa.AccountTitle) " +
						"WHERE (coa.AccountGroup = 'Detail' OR coa.Account_Level >= 4) " +
						"ORDER BY coa.AccountTitle, coa.AccountCode";
				dbRows = jdbcTemplate.queryForList(sql);
			}

			for (java.util.Map<String, Object> r : dbRows) {
				OpeningBalanceRow row = new OpeningBalanceRow();
				row.setId(r.get("Id") != null ? ((Number) r.get("Id")).intValue() : 0);
				row.setChartOfAccountId(r.get("ChartOfAccountId") != null ? ((Number) r.get("ChartOfAccountId")).intValue() : (r.get("id") != null ? ((Number) r.get("id")).intValue() : 0));
				row.setAccountCode(r.get("AccountCode") != null ? r.get("AccountCode").toString() : (r.get("accountCode") != null ? r.get("accountCode").toString() : ""));
				
				String title = r.get("ChartOfAccountTitle") != null ? r.get("ChartOfAccountTitle").toString() : (r.get("AccountTitle") != null ? r.get("AccountTitle").toString() : (r.get("accountTitle") != null ? r.get("accountTitle").toString() : ""));
				row.setAccountTitle(title);
				
				String type = r.get("AccountType") != null ? r.get("AccountType").toString() : (r.get("accountType") != null ? r.get("accountType").toString() : "Detail Account");
				row.setAccountType(type);

				String parentTitle = r.get("ParentAccountTitle") != null ? r.get("ParentAccountTitle").toString() : (r.get("parentAccountTitle") != null ? r.get("parentAccountTitle").toString() : "");
				row.setParentAccountTitle(parentTitle);

				double debit = r.get("YearObDebit") != null ? ((Number) r.get("YearObDebit")).doubleValue() : (r.get("yearObDebit") != null ? ((Number) r.get("yearObDebit")).doubleValue() : 0.0);
				double credit = r.get("YearObCredit") != null ? ((Number) r.get("YearObCredit")).doubleValue() : (r.get("yearObCredit") != null ? ((Number) r.get("yearObCredit")).doubleValue() : 0.0);

				row.setOpeningDebit(debit);
				row.setOpeningCredit(credit);
				totalDebit += debit;
				totalCredit += credit;
				rows.add(row);
			}
		} catch (Exception e) {
			loadError = e.getMessage();
			org.slf4j.LoggerFactory.getLogger(AccountDefinitionModulesController.class).error("Error in viewOpeningBalance: {}", e.getMessage(), e);
		}
		form.setRows(rows);
		form.setTotalDebit(totalDebit);
		form.setTotalCredit(totalCredit);
		model.addAttribute("activeMenu", "accounts");
		model.addAttribute("form", form);
		model.addAttribute("rights", openingBalanceDesktop.rights());
		model.addAttribute("loadError", loadError);
		return "accounts/opening_balance";
	}

	/** cmbAccountTitle_Leave / dgvOpeningBalance_DoubleClick: AccountsOpeningBalances.GetById. */
	@GetMapping("/opening_balance/by-id/{id}")
	@ResponseBody
	public Map<String, Object> openingBalanceById(@PathVariable("id") int id) {
		Map<String, Object> res = new HashMap<>();
		res.put("row", openingBalanceDesktop.getById(id));
		return res;
	}

	/** BtnEdit "Update All" - body {comboText, rows:[{id, chartOfAccountId, origDebit, origCredit, debit, credit}]}. */
	@PostMapping("/opening_balance/save")
	@ResponseBody
	public Map<String, Object> saveOpeningBalance(@RequestBody Map<String, Object> body) {
		Map<String, Object> res = new HashMap<>();
		try {
			List<Map<String, Object>> rows = new ArrayList<>();
			if (body.get("rows") instanceof List) {
				for (Object o : (List<?>) body.get("rows")) {
					if (o instanceof Map) {
						@SuppressWarnings("unchecked") Map<String, Object> m = (Map<String, Object>) o;
						rows.add(m);
					}
				}
			}
			res.put("message", openingBalanceDesktop.updateAll(rows, body.get("comboText") == null ? "" : String.valueOf(body.get("comboText"))));
			res.put("success", true);
		} catch (Exception e) {
			res.put("success", false);
			res.put("message", e.getMessage());
		}
		return res;
	}

	/** button1 "Update Single" (SingleRecordUpdate). */
	@PostMapping("/opening_balance/update-single")
	@ResponseBody
	public java.util.Map<String, Object> updateSingleOpeningBalance(
			@RequestParam(value = "comboValue", required = false, defaultValue = "0") Integer comboValue,
			@RequestParam(value = "id", required = false, defaultValue = "0") Integer id,
			@RequestParam(value = "chartOfAccountId", required = false, defaultValue = "0") Integer chartOfAccountId,
			@RequestParam(value = "comboText", required = false) String comboText,
			@RequestParam(value = "debitAmount", required = false) String debitAmount,
			@RequestParam(value = "creditAmount", required = false) String creditAmount) {
		java.util.Map<String, Object> res = new java.util.HashMap<>();
		try {
			res.put("message", openingBalanceDesktop.updateSingle(comboValue, id, chartOfAccountId, comboText, debitAmount, creditAmount));
			res.put("success", true);
			res.put("message", "Update record Successfully");
		} catch (Exception e) {
			res.put("success", false);
			res.put("message", e.getMessage());
		}
		return res;
	}


	@GetMapping("/opening_balance/print")
	@ResponseBody
	public java.util.Map<String, Object> printOpeningBalance() {
		/* Session-scoped, never from the request. See the note on the doc-number endpoint: a
		   tenancy id taken from a query string lets a caller read another company's data, and when
		   it is omitted the "1" default reports on a company that does not exist here. */
		Integer organizationId = currentUserContext.currentOrganizationId();
		Integer companyId      = currentUserContext.currentCompanyId();
		Integer financialYearId = currentUserContext.currentFinancialYearId();


		java.util.Map<String, Object> res = new java.util.HashMap<>();
		org.slf4j.Logger logger = org.slf4j.LoggerFactory.getLogger(AccountDefinitionModulesController.class);

		try {
			/* BtnPrint.Enabled = formrights.DoHavePrintRights */
			if (!openingBalanceDesktop.hasPrintRight()) {
				res.put("success", false);
				res.put("resultCount", 0);
				res.put("message", "You do not have the Print right for this screen.");
				return res;
			}
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
			Model model) {
		/* Session-scoped, never from the request. See the note on the doc-number endpoint: a
		   tenancy id taken from a query string lets a caller read another company's data, and when
		   it is omitted the "1" default reports on a company that does not exist here. */
		Integer organizationId = currentUserContext.currentOrganizationId();
		Integer companyId      = currentUserContext.currentCompanyId();
		Integer financialYearId = currentUserContext.currentFinancialYearId();


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
	public org.springframework.http.ResponseEntity<byte[]> exportOpeningBalancePdf() {
		/* Session-scoped, never from the request. See the note on the doc-number endpoint: a
		   tenancy id taken from a query string lets a caller read another company's data, and when
		   it is omitted the "1" default reports on a company that does not exist here. */
		Integer organizationId = currentUserContext.currentOrganizationId();
		Integer companyId      = currentUserContext.currentCompanyId();
		Integer financialYearId = currentUserContext.currentFinancialYearId();


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

	/* Screen 1 Account Custom Group (frmAccountCustomGroup). Every call goes through
	   AccountCustomGroupDesktopService, which reproduces the desktop's procedures and parameter sets.
	   The earlier JPA/raw-SQL fallbacks (fabricated queries, AccountTypeId=0/ParentAccountId=0 sent
	   as filters, a group delete the desktop does not have) are no longer reachable from this page. */
	@Autowired
	private com.mst.services.AccountCustomGroupDesktopService customGroupDesktop;

	@GetMapping("/custom_group")
	public String viewCustomGroup(Model model) {
		model.addAttribute("activeMenu", "accounts");
		return "accounts/custom_group";
	}

	/** Load / BtnRefresh: GridBind, LookUpBind, AccountTypeCombo, ThirdLevelAccountsDbCall. */
	@GetMapping("/custom_group/lookups")
	@ResponseBody
	public Map<String, Object> customGroupLookups() {
		Map<String, Object> res = new HashMap<>();
		res.put("gridGroups", customGroupDesktop.gridGroups());
		res.put("comboGroups", customGroupDesktop.comboGroups());
		res.put("accountTypes", customGroupDesktop.accountTypes());
		res.put("parentAccounts", customGroupDesktop.thirdLevelAccounts());
		return res;
	}

	/** grdLookUp_DoubleClick: AcLookUps.GetById. */
	@GetMapping("/custom_group/group/{id}")
	@ResponseBody
	public Map<String, Object> customGroupById(@PathVariable("id") int id) {
		Map<String, Object> res = new HashMap<>();
		try {
			res.put("success", true);
			res.put("group", customGroupDesktop.groupById(id));
		} catch (Exception ex) {
			res.put("success", false);
			res.put("message", ex.getMessage());
		}
		return res;
	}

	@PostMapping("/custom_group/save")
	@ResponseBody
	public Map<String, Object> saveCustomGroup(@RequestParam(value = "id", required = false) Integer id,
	                                           @RequestParam(value = "acLookUpsDescription", required = false) String acLookUpsDescription) {
		Map<String, Object> response = new HashMap<>();
		try {
			String msg = customGroupDesktop.saveGroup(id != null ? id : 0, acLookUpsDescription);
			response.put("success", msg != null);
			response.put("message", msg);
		} catch (Exception ex) {
			response.put("success", false);
			response.put("message", ex.getMessage());
		}
		return response;
	}

	/** The desktop form has no delete for a custom group (its only delete button un-allocates accounts). */
	@PostMapping("/custom_group/delete-group/{id}")
	@ResponseBody
	public Map<String, Object> deleteCustomGroup(@PathVariable("id") int id) {
		Map<String, Object> response = new HashMap<>();
		response.put("success", false);
		response.put("message", "Deleting a custom group is not available on this screen.");
		return response;
	}

	@GetMapping("/custom_group/show")
	@ResponseBody
	public Map<String, Object> showCustomGroupData(
			@RequestParam(value = "groupId", required = false, defaultValue = "0") int groupId,
			@RequestParam(value = "accountTypeId", required = false, defaultValue = "0") int accountTypeId,
			@RequestParam(value = "parentAccountId", required = false, defaultValue = "0") int parentAccountId) {
		Map<String, Object> result = new HashMap<>();
		try {
			List<List<Map<String, Object>>> grids = customGroupDesktop.show(groupId, accountTypeId, parentAccountId);
			result.put("unallocated", grids.get(0));
			result.put("allocated", grids.get(1));
			result.put("success", true);
		} catch (Exception ex) {
			result.put("success", false);
			result.put("message", ex.getMessage());
		}
		return result;
	}

	@PostMapping("/custom_group/allocate")
	@ResponseBody
	public Map<String, Object> allocateAccounts(
			@RequestParam(value = "groupId", required = false, defaultValue = "0") int groupId,
			@RequestBody List<Integer> chartOfAccountIds) {
		Map<String, Object> response = new HashMap<>();
		try {
			boolean ok = customGroupDesktop.allocate(groupId, chartOfAccountIds);
			response.put("success", true);
			response.put("message", ok ? "Saved Successfully" : null);
		} catch (Exception ex) {
			response.put("success", false);
			response.put("message", ex.getMessage());
		}
		return response;
	}

	/** Body: [{acLookUpsId, chartOfAccountId}] - each checked row's own hidden Id and ChartOfAccountId. */
	@PostMapping("/custom_group/unallocate")
	@ResponseBody
	public Map<String, Object> unallocateAccounts(@RequestBody List<Map<String, Object>> rows) {
		Map<String, Object> response = new HashMap<>();
		try {
			customGroupDesktop.unAllocate(rows);
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
	public List<Map<String, Object>> getParentAccounts() {
		return customGroupDesktop.thirdLevelAccounts();
	}

	// Form Helper Classes for Batch Opening Balance Submission
	public static class OpeningBalanceForm {
		private List<OpeningBalanceRow> rows = new ArrayList<>();
		private Double totalDebit = 0.0;
		private Double totalCredit = 0.0;

		public List<OpeningBalanceRow> getRows() { return rows; }
		public void setRows(List<OpeningBalanceRow> rows) { this.rows = rows; }
		public Double getTotalDebit() { return totalDebit; }
		public void setTotalDebit(Double totalDebit) { this.totalDebit = totalDebit; }
		public Double getTotalCredit() { return totalCredit; }
		public void setTotalCredit(Double totalCredit) { this.totalCredit = totalCredit; }
	}

	public static class OpeningBalanceRow {
		private Integer id = 0;
		private Integer chartOfAccountId = 0;
		private Integer accountId = 0;
		private String accountCode;
		private String accountTitle;
		private String accountType;
		private String parentAccountTitle;
		private Double openingDebit = 0.0;
		private Double openingCredit = 0.0;

		public Integer getId() { return id; }
		public void setId(Integer id) { this.id = id; }
		public Integer getChartOfAccountId() { return chartOfAccountId; }
		public void setChartOfAccountId(Integer chartOfAccountId) { this.chartOfAccountId = chartOfAccountId; }
		public Integer getAccountId() { return accountId; }
		public void setAccountId(Integer accountId) { this.accountId = accountId; }
		public String getAccountCode() { return accountCode; }
		public void setAccountCode(String accountCode) { this.accountCode = accountCode; }
		public String getAccountTitle() { return accountTitle; }
		public void setAccountTitle(String accountTitle) { this.accountTitle = accountTitle; }
		public String getAccountType() { return accountType; }
		public void setAccountType(String accountType) { this.accountType = accountType; }
		public String getParentAccountTitle() { return parentAccountTitle; }
		public void setParentAccountTitle(String parentAccountTitle) { this.parentAccountTitle = parentAccountTitle; }
		public Double getOpeningDebit() { return openingDebit; }
		public void setOpeningDebit(Double openingDebit) { this.openingDebit = openingDebit; }
		public Double getOpeningCredit() { return openingCredit; }
		public void setOpeningCredit(Double openingCredit) { this.openingCredit = openingCredit; }
		public Double getOpeningDebitValue() { return openingDebit != null ? openingDebit : 0.0; }
		public Double getOpeningCreditValue() { return openingCredit != null ? openingCredit : 0.0; }
	}

}
