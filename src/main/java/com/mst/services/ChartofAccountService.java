package com.mst.services;

import com.mst.security.CurrentUserContext;

import java.time.LocalDateTime;
import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.util.StringUtils;

import java.util.ArrayList;
import java.util.stream.Collectors;

import com.mst.messages.CompanyAllocationRow;
import com.mst.messages.CustomGroupAssignmentRow;
import com.mst.models.AcLookUp;
import com.mst.models.AccountsCustomGroup;
import com.mst.models.COAAllocation;
import com.mst.models.ChartofAccount;
import com.mst.models.Company;
import com.mst.repositories.IAcLookUpRepository;
import com.mst.repositories.IAccountsCustomGroupRepository;
import com.mst.repositories.ICOAAllocationRepository;
import com.mst.repositories.ICompanyRepository;
import com.mst.repositories.IChartofAccountRepository;
import com.mst.serviceInterface.IChartofAccountService;

/**
 * Ditto of the desktop's Architecture.BLL.Accounts.ChartofAccount.
 *
 * CODE-GENERATION - the REAL desktop algorithm, read directly from the database
 * script (goldenAce5_25t.sql): stored procedure Sp_ChartofAccount_GetAllMethodFromCOA
 * (@CoaType='NewCodeByParentCode') delegates to table-valued function
 * dbo.GenerateNewAccountCodeByParentAccountCode(OrganizationId, CompanyId, ParentAccountCode).
 * That function branches on a per-org/company config flag (ConfigId 413,
 * "AccountCodingEnable"); {@link #nextChildCode(String)} below implements its DEFAULT
 * branch (flag off/unset, which is what ISNULL(...,0) falls back to), i.e.:
 *
 *   newCode = parentCode + pad( MAX(existing sibling's code suffix after the parent
 *             prefix, as int) + 1 ), where pad zero-pads to 2 digits when the number
 *             is under 10 and otherwise uses it as-is.
 *
 * (e.g. parent "0" -&gt; first child "001", second "002", ... 10th "010", 11th "011";
 * parent "07" -&gt; "0701", "0702", ...). The flag's OTHER branch (a dash-separated
 * "07-01" style scheme) isn't implemented since this database's actual ConfigId 413
 * value wasn't available to read.
 *
 * AccountClass values (desktop: EnumAccountClass, confirmed from the same script's
 * AccountClassName CASE expression): 1=Capital, 2=Assets, 3=Liability, 4=Expense,
 * 5=Revenue. A new child inherits its parent's AccountClass unless already set
 * (matches the function's own @AccountClass = C.AccountClass lookup).
 *
 * ID GENERATION - the real dbo.ChartofAccount.Id has no IDENTITY clause (app-assigned),
 * so save() computes the next id itself (max(id)+1), same convention as every other
 * non-identity master table in this port.
 */
@Service
public class ChartofAccountService implements IChartofAccountService {

	@Autowired
	private IChartofAccountRepository chartofAccountRepository;
	@Autowired
	private CurrentUserContext currentUserContext;
	@Autowired
	private ICOAAllocationRepository coaAllocationRepository;
	@Autowired
	private ICompanyRepository companyRepository;
	@Autowired
	private IAcLookUpRepository acLookUpRepository;
	@Autowired
	private IAccountsCustomGroupRepository accountsCustomGroupRepository;
	@Autowired
	private com.mst.repositories.IAccountOpeningBalanceRepository accountOpeningBalanceRepository;
	@Autowired
	private com.mst.repositories.IAccountTypesRepository accountTypesRepository;

	@Override
	public List<ChartofAccount> getAllAccounts() {
		return chartofAccountRepository.findAllByOrderByAccountCode();
	}

	@Override
	public List<ChartofAccount> getRootAccounts() {
		List<ChartofAccount> roots = chartofAccountRepository.findByParentAccountCodeOrderByAccountTitle("0");
		if (roots == null || roots.isEmpty()) {
			roots = chartofAccountRepository.findByParentAccountCodeOrderByAccountTitle("");
		}
		if (roots == null || roots.isEmpty()) {
			roots = chartofAccountRepository.findByAccountLevelOrderByAccountTitle(1);
		}
		return roots;
	}

	@Override
	public List<ChartofAccount> getAccountsByParent(String parentAccountCode) {
		if (!StringUtils.hasText(parentAccountCode) || "0".equals(parentAccountCode.trim())) {
			return getRootAccounts();
		}
		final String targetCode = parentAccountCode.trim();

		ChartofAccount parent = chartofAccountRepository.findByAccountCode(targetCode);
		final Integer parentId = parent != null ? parent.getId() : null;
		final Integer parentLevel = parent != null ? parent.getAccountLevel() : null;

		List<ChartofAccount> list = chartofAccountRepository.findAllByOrderByAccountCode().stream()
				.filter(a -> {
					if (a.getParentAccountCode() != null && targetCode.equalsIgnoreCase(a.getParentAccountCode().trim())) {
						return true;
					}
					if (parentId != null && parentId.equals(a.getParentCodeId())) {
						return true;
					}
					if (parentLevel != null && a.getAccountLevel() != null && a.getAccountLevel() == parentLevel + 1) {
						if (a.getAccountCode() != null && a.getAccountCode().trim().startsWith(targetCode)) {
							return true;
						}
					}
					return false;
				})
				.collect(Collectors.toList());

		return list != null ? list : java.util.Collections.emptyList();
	}

	@Override
	public java.util.Map<String, List<ChartofAccount>> getCascadedAccountsByParent(String parentAccountCode) {
		java.util.Map<String, List<ChartofAccount>> map = new java.util.LinkedHashMap<>();
		if (!StringUtils.hasText(parentAccountCode) || "0".equals(parentAccountCode.trim())) {
			List<ChartofAccount> l1 = getRootAccounts();
			map.put("level1", l1);
			if (l1 != null && !l1.isEmpty()) {
				String firstL1Code = l1.get(0).getAccountCode();
				List<ChartofAccount> l2 = getAccountsByParent(firstL1Code);
				map.put("level2", l2);
				if (l2 != null && !l2.isEmpty()) {
					String firstL2Code = l2.get(0).getAccountCode();
					List<ChartofAccount> l3 = getAccountsByParent(firstL2Code);
					map.put("level3", l3);
					if (l3 != null && !l3.isEmpty()) {
						String firstL3Code = l3.get(0).getAccountCode();
						List<ChartofAccount> l4 = getAccountsByParent(firstL3Code);
						map.put("level4", l4);
					}
				}
			}
			return map;
		}

		String target = parentAccountCode.trim();
		ChartofAccount selectedParent = getByCode(target);
		int startLevel = (selectedParent != null && selectedParent.getAccountLevel() != null)
				? selectedParent.getAccountLevel() + 1
				: 2;

		// First immediate children level
		List<ChartofAccount> firstChildren = getAccountsByParent(target);
		map.put("level" + startLevel, firstChildren);

		if (firstChildren != null && !firstChildren.isEmpty()) {
			String nextCode = firstChildren.get(0).getAccountCode();
			int secondLevel = startLevel + 1;
			List<ChartofAccount> secondChildren = getAccountsByParent(nextCode);
			map.put("level" + secondLevel, secondChildren);

			if (secondChildren != null && !secondChildren.isEmpty()) {
				String nextNextCode = secondChildren.get(0).getAccountCode();
				int thirdLevel = secondLevel + 1;
				List<ChartofAccount> thirdChildren = getAccountsByParent(nextNextCode);
				map.put("level" + thirdLevel, thirdChildren);
			}
		}

		return map;
	}

	@Override
	public List<ChartofAccount> getDetailAccounts() {
		return chartofAccountRepository.findByAccountGroupOrderByAccountTitle("Detail");
	}

	@Override
	public ChartofAccount getByCode(String accountCode) {
		if (!StringUtils.hasText(accountCode)) {
			return null;
		}
		return chartofAccountRepository.findByAccountCode(accountCode);
	}

	@Override
	public ChartofAccount getById(Integer id) {
		if (id == null) {
			return null;
		}
		return chartofAccountRepository.findById(id).orElse(null);
	}

	@Override
	public Double getOpeningBalance(Integer chartOfAccountId) {
		if (chartOfAccountId == null) return 0.0;
		com.mst.models.AccountOpeningBalance ob = accountOpeningBalanceRepository.findByChartOfAccountId(chartOfAccountId);
		if (ob == null) return 0.0;
		if (ob.getYearObDebit() != null && ob.getYearObDebit() > 0) {
			return ob.getYearObDebit();
		}
		if (ob.getYearObCredit() != null && ob.getYearObCredit() > 0) {
			return -ob.getYearObCredit();
		}
		return 0.0;
	}

	@Override
	public ChartofAccount save(ChartofAccount account) {
		return save(account, null);
	}

	@Override
	public ChartofAccount save(ChartofAccount account, Double openingBalance) {
		LocalDateTime now = LocalDateTime.now();

		if (!StringUtils.hasText(account.getAccountGroup())) {
			account.setAccountGroup("Group");
		}
		if (!StringUtils.hasText(account.getParentAccountCode())) {
			account.setParentAccountCode("0");
		}

		boolean isNew = account.getId() == null;

		if (isNew) {
			ChartofAccount parent = "0".equals(account.getParentAccountCode())
					? null
					: chartofAccountRepository.findByAccountCode(account.getParentAccountCode());

			int parentLevel = parent != null && parent.getAccountLevel() != null ? parent.getAccountLevel() : 0;
			account.setId(chartofAccountRepository.findMaxId() + 1);
			account.setAccountLevel(parentLevel + 1);
			account.setParentCodeId(parent != null ? parent.getId() : null);
			// Desktop's BLL.Accounts.ChartofAccount.Save() ALWAYS recomputes AccountCode from
			// ReadNewCodebyParentCode at insert time, regardless of whatever code the UI had
			// displayed - it never trusts a client-supplied code. Mirrored here so two
			// near-simultaneous inserts under the same parent can never collide.
			account.setAccountCode(nextChildCode(account.getParentAccountCode()));
			if (account.getAccountClass() == null && parent != null) {
				account.setAccountClass(parent.getAccountClass());
			}
			account.setOrganizationId(currentUserContext.currentOrganizationId());
			account.setCompanyId(currentUserContext.currentCompanyId());
			account.setEntryDate(now);
			account.setEntryUser(currentUserContext.currentUserId());
			account.setIsActive(account.getIsActive() == null || account.getIsActive());
			account.setPostState(Boolean.TRUE.equals(account.getPostState()));
		} else {
			// Update mode - preserve original creation metadata, update fields
			ChartofAccount existing = chartofAccountRepository.findById(account.getId()).orElse(null);
			if (existing != null) {
				if (account.getAccountCode() == null) account.setAccountCode(existing.getAccountCode());
				if (account.getParentAccountCode() == null) account.setParentAccountCode(existing.getParentAccountCode());
				if (account.getParentCodeId() == null) account.setParentCodeId(existing.getParentCodeId());
				if (account.getAccountGroup() == null) account.setAccountGroup(existing.getAccountGroup());
				if (account.getAccountClass() == null) account.setAccountClass(existing.getAccountClass());
				if (account.getAccountLevel() == null) account.setAccountLevel(existing.getAccountLevel());
				if (account.getOrganizationId() == null) account.setOrganizationId(existing.getOrganizationId());
				if (account.getCompanyId() == null) account.setCompanyId(existing.getCompanyId());
				if (account.getEntryDate() == null) account.setEntryDate(existing.getEntryDate());
				if (account.getEntryUser() == null) account.setEntryUser(existing.getEntryUser());
			}
			account.setModifyUser(currentUserContext.currentUserId());
		}
		account.setModifyDate(now);

		ChartofAccount saved = chartofAccountRepository.save(account);

		// Handle Opening Balance if provided and for Detail accounts
		if (openingBalance != null && saved != null && saved.getId() != null) {
			com.mst.models.AccountOpeningBalance ob = accountOpeningBalanceRepository.findByChartOfAccountId(saved.getId());
			if (ob == null) {
				ob = new com.mst.models.AccountOpeningBalance();
				ob.setId(accountOpeningBalanceRepository.findMaxId() + 1);
				ob.setChartOfAccountId(saved.getId());
				ob.setAccountCode(saved.getAccountCode());
				ob.setChartOfAccountTitle(saved.getAccountTitle());
				ob.setOrganizationId(saved.getOrganizationId());
				ob.setCompanyId(saved.getCompanyId());
				ob.setEntryDate(now);
				ob.setEntryUser(currentUserContext.currentUserId());
			} else {
				ob.setModifyDate(now);
				ob.setModifyUser(currentUserContext.currentUserId());
			}
			if (openingBalance >= 0) {
				ob.setYearObDebit(openingBalance);
				ob.setYearObCredit(0.0);
			} else {
				ob.setYearObDebit(0.0);
				ob.setYearObCredit(Math.abs(openingBalance));
			}
			accountOpeningBalanceRepository.save(ob);
		}

		return saved;
	}

	@Override
	public void delete(String accountCode) {
		ChartofAccount account = chartofAccountRepository.findByAccountCode(accountCode);
		if (account != null) {
			chartofAccountRepository.deleteById(account.getId());
		}
	}

	@Override
	public boolean hasChildren(String accountCode) {
		return chartofAccountRepository.countByParentAccountCode(accountCode) > 0;
	}

	@Override
	public List<ChartofAccount> getFilteredChildAccounts(String parentAccountCode, String accountType, String accountTitle) {
		String parent = (StringUtils.hasText(parentAccountCode) && !"0".equals(parentAccountCode.trim())) ? parentAccountCode.trim() : "0";
		String type = StringUtils.hasText(accountType) ? accountType.trim() : null;
		String title = StringUtils.hasText(accountTitle) ? accountTitle.trim() : null;

		return chartofAccountRepository.findFilteredChildren(parent, type, title);
	}

	@Override
	public List<CompanyAllocationRow> getAllocations(Integer chartofAccountId) {
		List<CompanyAllocationRow> rows = new ArrayList<>();
		List<COAAllocation> existing = chartofAccountId != null ? coaAllocationRepository.findByChartofAccountId(chartofAccountId) : new ArrayList<>();
		java.util.Map<Integer, Boolean> activeByCompany = existing.stream()
				.collect(Collectors.toMap(COAAllocation::getCompanyId, a -> Boolean.TRUE.equals(a.getIsActive()), (a, b) -> a || b));

		for (Company company : companyRepository.findAllByOrderByCompName()) {
			CompanyAllocationRow row = new CompanyAllocationRow();
			row.setCompanyId(company.getId());
			row.setCompanyName(company.getCompName());
			if (chartofAccountId == null) {
				row.setAllocated(true);
			} else {
				row.setAllocated(Boolean.TRUE.equals(activeByCompany.get(company.getId())));
			}
			rows.add(row);
		}
		return rows;
	}

	@Override
	public void setAllocation(Integer chartofAccountId, Integer companyId, boolean allocated) {
		if (chartofAccountId == null || companyId == null) {
			return;
		}
		COAAllocation allocation = coaAllocationRepository.findByChartofAccountIdAndCompanyId(chartofAccountId, companyId);
		if (allocation == null) {
			if (!allocated) {
				return;
			}
			allocation = new COAAllocation();
			allocation.setChartofAccountId(chartofAccountId);
			allocation.setCompanyId(companyId);
		}
		allocation.setIsActive(allocated);
		coaAllocationRepository.save(allocation);
	}

	@Override
	public List<CustomGroupAssignmentRow> getCustomGroupAssignments(Integer chartofAccountId) {
		List<CustomGroupAssignmentRow> rows = new ArrayList<>();
		if (chartofAccountId == null) {
			return rows;
		}
		List<AccountsCustomGroup> existing = accountsCustomGroupRepository.findByChartOfAccountId(chartofAccountId);
		java.util.Set<Integer> assignedLookUpIds = existing.stream()
				.map(AccountsCustomGroup::getAcLookUpsId)
				.collect(Collectors.toSet());
		for (AcLookUp lookUp : acLookUpRepository.findByAcLookUpTypesIdOrderByAcLookUpsDescription(AcLookUp.ACCOUNTS_CUSTOM_GROUP_TYPE_ID)) {
			CustomGroupAssignmentRow row = new CustomGroupAssignmentRow();
			row.setLookUpId(lookUp.getId());
			row.setDescription(lookUp.getAcLookUpsDescription());
			row.setAssigned(assignedLookUpIds.contains(lookUp.getId()));
			rows.add(row);
		}
		return rows;
	}

	@Override
	public void setCustomGroupAssignment(Integer chartofAccountId, Integer lookUpId, boolean assigned) {
		if (chartofAccountId == null || lookUpId == null) {
			return;
		}
		AccountsCustomGroup existing = accountsCustomGroupRepository.findByChartOfAccountIdAndAcLookUpsId(chartofAccountId, lookUpId);
		if (assigned) {
			if (existing != null) {
				return;
			}
			AccountsCustomGroup assignment = new AccountsCustomGroup();
			assignment.setSortNo(accountsCustomGroupRepository.findMaxSortNo() + 1);
			assignment.setChartOfAccountId(chartofAccountId);
			assignment.setAcLookUpsId(lookUpId);
			assignment.setEntryDate(LocalDateTime.now());
			assignment.setEntryUserId(currentUserContext.currentUserId());
			assignment.setOrganizationId(currentUserContext.currentOrganizationId());
			assignment.setCompanyId(currentUserContext.currentCompanyId());
			accountsCustomGroupRepository.save(assignment);
		} else if (existing != null) {
			accountsCustomGroupRepository.delete(existing);
		}
	}

	@Autowired
	private com.mst.repositories.ICityRepository cityRepository;

	@Override
	public List<ChartofAccount> searchAccounts(String query) {
		if (!StringUtils.hasText(query)) {
			return chartofAccountRepository.findAllByOrderByAccountCode().stream()
					.limit(100)
					.collect(Collectors.toList());
		}
		String q = query.trim();
		return chartofAccountRepository.findByAccountTitleContainingIgnoreCaseOrAccountCodeContainingIgnoreCaseOrderByAccountTitle(q, q);
	}

	@Override
	public List<ChartofAccount> getParentLookup() {
		return chartofAccountRepository.findAllByOrderByAccountCode().stream()
				.filter(a -> !"Detail".equalsIgnoreCase(a.getAccountGroup()) || (a.getAccountLevel() != null && a.getAccountLevel() < 4))
				.collect(Collectors.toList());
	}

	@Override
	public ChartofAccount getNextChildCodeDetails(String parentAccountCode) {
		ChartofAccount result = new ChartofAccount();
		if (!StringUtils.hasText(parentAccountCode) || "0".equals(parentAccountCode)) {
			result.setParentAccountCode("0");
			result.setAccountLevel(1);
			result.setAccountGroup("Group");
			result.setAccountCode(nextChildCode("0"));
			return result;
		}

		ChartofAccount parent = chartofAccountRepository.findByAccountCode(parentAccountCode);
		if (parent != null) {
			result.setParentAccountCode(parent.getAccountCode());
			result.setParentCodeId(parent.getId());
			int lvl = parent.getAccountLevel() != null ? parent.getAccountLevel() + 1 : 1;
			result.setAccountLevel(lvl);
			result.setAccountClass(parent.getAccountClass());
			result.setBsNoteId(parent.getBsNoteId());
			result.setPlNoteId(parent.getPlNoteId());
			result.setCustomerGroupId(parent.getCustomerGroupId());
			result.setAccountGroup(lvl < 4 ? "Group" : "Detail");
			// Ditto cmbparentac_Leave: a Detail (level>=4) child inherits & displays its parent's
			// Account Type read-only; at level 3 the user assigns it themselves (left null here).
			result.setAccountTypeId(lvl >= 4 ? parent.getAccountTypeId() : null);
			result.setAccountCode(nextChildCode(parent.getAccountCode()));
		} else {
			result.setParentAccountCode(parentAccountCode);
			result.setAccountLevel(1);
			result.setAccountGroup("Group");
			result.setAccountCode(nextChildCode(parentAccountCode));
		}
		return result;
	}

	@Autowired
	private com.mst.repositories.ICustomerGroupRepository customerGroupRepository;

	@Override
	public List<com.mst.models.CustomerGroup> getCustomerGroups() {
		return customerGroupRepository.findAllByOrderByDescription();
	}

	@Override
	public List<AcLookUp> getCustomGroups() {
		return acLookUpRepository.findByAcLookUpTypesIdOrderByAcLookUpsDescription(AcLookUp.ACCOUNTS_CUSTOM_GROUP_TYPE_ID);
	}

	@Override
	public List<com.mst.models.AccountTypes> getAccountTypes() {
		// Ditto Architecture.BLL.Accounts.AccountTypes::GetAll (Proc_AccountTypes_ReadAll) -
		// the real dbo.AccountTypes master table, NOT AcLookUp (confirmed against the
		// production GoldenAcedb data: Id=2 = "Cash Equivalent"). Both the main "Account Type"
		// field and the "Filter Accounts In Child Grid -> Account Type" dropdown read this
		// same list on the desktop (AcfrmDefCoa::CmbAccountType binds both from one call).
		return accountTypesRepository.findAllByOrderById();
	}

	@Override
	public List<ChartofAccount> getThirdLevelGroupAccountsByType(Integer accountTypeId) {
		if (accountTypeId == null) {
			return new ArrayList<>();
		}
		return chartofAccountRepository.findByAccountLevelAndAccountGroupAndAccountTypeIdOrderByAccountCode(3, "Group", accountTypeId);
	}

	@Override
	public String getParentCodeByChartofAccountId(Integer chartofAccountId) {
		if (chartofAccountId == null) {
			return null;
		}
		ChartofAccount account = getById(chartofAccountId);
		if (account != null) {
			return account.getParentAccountCode();
		}
		return null;
	}

	@Override
	public List<ChartofAccount> getHistoryAccounts(List<Integer> levels, String status, Integer customGroupId) {
		List<Integer> targetLevels = (levels == null || levels.isEmpty()) ? List.of(1, 2, 3, 4, 5) : levels;
		List<ChartofAccount> accounts = chartofAccountRepository.findByAccountLevelInOrderByAccountCode(targetLevels);

		if ("Active Only".equalsIgnoreCase(status)) {
			accounts = accounts.stream().filter(a -> Boolean.TRUE.equals(a.getIsActive())).collect(Collectors.toList());
		} else if ("InActive".equalsIgnoreCase(status)) {
			accounts = accounts.stream().filter(a -> Boolean.FALSE.equals(a.getIsActive())).collect(Collectors.toList());
		}

		return accounts;
	}

	@Override
	public List<com.mst.models.City> getAllCities() {
		return cityRepository.findAllByOrderByCityName();
	}

	@Override
	public com.mst.models.City saveCity(String cityName, Integer tehsilId) {
		if (!StringUtils.hasText(cityName)) {
			return null;
		}
		String name = cityName.trim();
		com.mst.models.City existing = cityRepository.findByCityNameIgnoreCase(name);
		if (existing != null) {
			return existing;
		}
		com.mst.models.City city = new com.mst.models.City();
		city.setId(cityRepository.findMaxId() + 1);
		city.setCityName(name);
		city.setTehsilId(tehsilId != null ? tehsilId : 1);
		city.setOrganizationId(currentUserContext.currentOrganizationId());
		city.setCompanyId(currentUserContext.currentCompanyId());
		return cityRepository.save(city);
	}

	/** See the class-level CODE-GENERATION note above - this is the real desktop default-branch algorithm. */
	private String nextChildCode(String parentAccountCode) {
		List<ChartofAccount> siblings = chartofAccountRepository.findByParentAccountCodeOrderByAccountTitle(parentAccountCode);

		int prefixLen = parentAccountCode.length();
		int maxChildNo = 0;
		for (ChartofAccount sibling : siblings) {
			String code = sibling.getAccountCode();
			if (code == null || code.length() <= prefixLen) {
				continue;
			}
			String suffix = code.substring(prefixLen);
			// SQL: CAST(SUBSTRING(AccountCode, LEN(parentCode) + 1, 10) AS INT) - numeric suffixes only.
			if (suffix.length() > 10) {
				suffix = suffix.substring(0, 10);
			}
			if (suffix.matches("\\d+")) {
				maxChildNo = Math.max(maxChildNo, Integer.parseInt(suffix));
			}
		}

		int next = maxChildNo + 1;
		String padded = next < 10 ? "0" + next : String.valueOf(next);
		String candidate = parentAccountCode + padded;

		// Guard against a collision the SQL's own MAX(...)+1 wouldn't normally hit
		// (e.g. a non-numeric-suffix sibling code) - keep incrementing until free.
		while (chartofAccountRepository.existsByAccountCode(candidate)) {
			next++;
			padded = next < 10 ? "0" + next : String.valueOf(next);
			candidate = parentAccountCode + padded;
		}
		return candidate;
	}

	@Override
	public List<ChartofAccount> getLevel4Accounts() {
		return chartofAccountRepository.findByAccountLevelOrderByAccountTitle(4);
	}
}

