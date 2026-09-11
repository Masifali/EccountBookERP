package com.mst.controllers;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.stream.Collectors;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import com.mst.messages.CompanyAllocationRow;
import com.mst.models.ChartofAccount;
import com.mst.models.City;
import com.mst.serviceInterface.IChartofAccountService;

import java.util.Arrays;
import org.springframework.util.StringUtils;

@RestController
@RequestMapping("/api/accounts")
public class ChartOfAccountApiController {

	@Autowired
	private IChartofAccountService chartofAccountService;

	@GetMapping("/search")
	public ResponseEntity<List<Map<String, Object>>> searchAccounts(@RequestParam(value = "query", required = false, defaultValue = "") String query) {
		List<ChartofAccount> list = chartofAccountService.searchAccounts(query);
		List<Map<String, Object>> result = list.stream().map(a -> {
			Map<String, Object> map = new HashMap<>();
			map.put("id", a.getId());
			map.put("accountCode", a.getAccountCode());
			map.put("accountTitle", a.getAccountTitle());
			map.put("accountClass", getAccountClassName(a.getAccountClass()));
			map.put("accountType", a.getAccountGroup() != null ? a.getAccountGroup() : "Detail");
			map.put("accountLevel", a.getAccountLevel());
			map.put("parentAccountCode", a.getParentAccountCode());
			return map;
		}).collect(Collectors.toList());
		return ResponseEntity.ok(result);
	}

	@GetMapping("/parents")
	public ResponseEntity<List<Map<String, Object>>> getParents() {
		List<ChartofAccount> parents = chartofAccountService.getParentLookup();
		List<Map<String, Object>> result = parents.stream().map(a -> {
			Map<String, Object> map = new HashMap<>();
			map.put("id", a.getId());
			map.put("accountCode", a.getAccountCode());
			map.put("accountTitle", a.getAccountTitle());
			map.put("accountClass", getAccountClassName(a.getAccountClass()));
			map.put("accountLevel", a.getAccountLevel());
			return map;
		}).collect(Collectors.toList());
		return ResponseEntity.ok(result);
	}

	@GetMapping("/level")
	public ResponseEntity<List<ChartofAccount>> getLevelAccounts(
			@RequestParam(value = "parentCode", required = false, defaultValue = "0") String parentCode,
			@RequestParam(value = "accountType", required = false) String accountType,
			@RequestParam(value = "accountTitle", required = false) String accountTitle) {
		List<ChartofAccount> list = chartofAccountService.getFilteredChildAccounts(parentCode, accountType, accountTitle);
		return ResponseEntity.ok(list);
	}

	@GetMapping("/cascade-levels")
	public ResponseEntity<Map<String, List<ChartofAccount>>> getCascadeLevels(
			@RequestParam(value = "parentCode", required = false, defaultValue = "0") String parentCode) {
		Map<String, List<ChartofAccount>> map = chartofAccountService.getCascadedAccountsByParent(parentCode);
		return ResponseEntity.ok(map);
	}

	@GetMapping("/next-code")
	public ResponseEntity<ChartofAccount> getNextCode(@RequestParam(value = "parentCode", required = false, defaultValue = "0") String parentCode) {
		ChartofAccount next = chartofAccountService.getNextChildCodeDetails(parentCode);
		return ResponseEntity.ok(next);
	}

	@GetMapping("/locations")
	public ResponseEntity<List<CompanyAllocationRow>> getLocations() {
		List<CompanyAllocationRow> list = chartofAccountService.getAllocations(null);
		return ResponseEntity.ok(list);
	}

	@GetMapping("/{id}")
	public ResponseEntity<Map<String, Object>> getAccountById(@PathVariable("id") Integer id) {
		ChartofAccount account = chartofAccountService.getById(id);
		if (account == null) {
			return ResponseEntity.notFound().build();
		}
		Double openingBalance = chartofAccountService.getOpeningBalance(id);
		Map<String, Object> resp = new HashMap<>();
		resp.put("account", account);
		resp.put("openingBalance", openingBalance);
		return ResponseEntity.ok(resp);
	}

	@GetMapping("/{id}/allocations")
	public ResponseEntity<List<CompanyAllocationRow>> getAllocations(@PathVariable("id") Integer id) {
		List<CompanyAllocationRow> list = chartofAccountService.getAllocations(id);
		return ResponseEntity.ok(list);
	}

	@PostMapping("/save")
	public ResponseEntity<Map<String, Object>> saveAccount(@RequestBody SaveAccountRequest req) {
		ChartofAccount account = req.getAccount();
		if (account == null) {
			account = new ChartofAccount();
		}
		ChartofAccount saved = chartofAccountService.save(account, req.getOpeningBalance());

		if (req.getAllocations() != null && saved != null && saved.getId() != null) {
			for (CompanyAllocationRow row : req.getAllocations()) {
				chartofAccountService.setAllocation(saved.getId(), row.getCompanyId(), row.isAllocated());
			}
		}

		Map<String, Object> resp = new HashMap<>();
		resp.put("success", true);
		resp.put("account", saved);
		resp.put("message", "Account saved successfully!");
		return ResponseEntity.ok(resp);
	}

	@GetMapping("/history")
	public ResponseEntity<List<Map<String, Object>>> getHistory(
			@RequestParam(value = "levels", required = false) String levelsStr,
			@RequestParam(value = "status", required = false, defaultValue = "All") String status,
			@RequestParam(value = "customGroupId", required = false) Integer customGroupId) {
		List<Integer> levels = null;
		if (StringUtils.hasText(levelsStr)) {
			try {
				levels = Arrays.stream(levelsStr.split(","))
						.map(String::trim)
						.filter(s -> !s.isEmpty())
						.map(Integer::parseInt)
						.collect(Collectors.toList());
			} catch (Exception ignored) {}
		}
		List<ChartofAccount> list = chartofAccountService.getHistoryAccounts(levels, status, customGroupId);
		List<Map<String, Object>> result = list.stream().map(a -> {
			Map<String, Object> map = new HashMap<>();
			map.put("id", a.getId());
			map.put("parentAccountId", a.getParentCodeId());
			map.put("parentAccountCode", a.getParentAccountCode());
			map.put("accountTitle", a.getAccountTitle());
			map.put("accountCode", a.getAccountCode());
			map.put("otherErpCode", a.getOtherErpCode());
			map.put("accountGroup", a.getAccountGroup());
			map.put("accountLevel", a.getAccountLevel());
			map.put("isActive", Boolean.TRUE.equals(a.getIsActive()) ? "True" : "False");
			map.put("accountClass", getAccountClassName(a.getAccountClass()));
			map.put("accountType", a.getAccountTypeId() != null ? "Type " + a.getAccountTypeId() : "");
			return map;
		}).collect(Collectors.toList());
		return ResponseEntity.ok(result);
	}

	@GetMapping("/cities")
	public ResponseEntity<List<City>> getCities() {
		return ResponseEntity.ok(chartofAccountService.getAllCities());
	}

	@GetMapping("/customer-groups")
	public ResponseEntity<List<com.mst.models.CustomerGroup>> getCustomerGroups() {
		return ResponseEntity.ok(chartofAccountService.getCustomerGroups());
	}

	@GetMapping("/custom-groups")
	public ResponseEntity<List<com.mst.models.AcLookUp>> getCustomGroups() {
		return ResponseEntity.ok(chartofAccountService.getCustomGroups());
	}

	@GetMapping("/account-types")
	public ResponseEntity<List<com.mst.models.AccountTypes>> getAccountTypes() {
		return ResponseEntity.ok(chartofAccountService.getAccountTypes());
	}

	/**
	 * Ditto of CmbAccountTypeFilter_Leave -> Sp_COAAllocation_GetAllMethod
	 * @Activity='Get3rdLevelGroupAccounts': REPLACES the Child Of Selected Parent grid with the
	 * level-3 Group account(s) of this Account Type, independent of the selected Parent Account.
	 */
	@GetMapping("/third-level-by-type/{typeId}")
	public ResponseEntity<List<ChartofAccount>> getThirdLevelByType(@PathVariable("typeId") Integer typeId) {
		return ResponseEntity.ok(chartofAccountService.getThirdLevelGroupAccountsByType(typeId));
	}

	/**
	 * Ditto of CmbAccountTitle_Leave -> Sp_COAAllocation_GetAllMethod
	 * @Activity='GetParentCodeByChartofAccountId': Retrieves parent account code for a selected account ID.
	 */
	@GetMapping("/parent-code-by-account-id/{chartofAccountId}")
	public ResponseEntity<Map<String, Object>> getParentCodeByChartofAccountId(@PathVariable("chartofAccountId") Integer chartofAccountId) {
		String parentCode = chartofAccountService.getParentCodeByChartofAccountId(chartofAccountId);
		Map<String, Object> resp = new HashMap<>();
		resp.put("chartofAccountId", chartofAccountId);
		resp.put("parentAccountCode", parentCode);
		return ResponseEntity.ok(resp);
	}

	@GetMapping("/level4-accounts")
	public ResponseEntity<List<Map<String, Object>>> getLevel4Accounts() {
		List<ChartofAccount> list = chartofAccountService.getLevel4Accounts();
		List<com.mst.models.AccountTypes> types = chartofAccountService.getAccountTypes();
		Map<Integer, String> typeMap = types.stream().collect(Collectors.toMap(com.mst.models.AccountTypes::getId, com.mst.models.AccountTypes::getAccountType, (a, b) -> a));

		List<Map<String, Object>> result = list.stream().map(a -> {
			Map<String, Object> map = new HashMap<>();
			map.put("id", a.getId());
			map.put("accountCode", a.getAccountCode());
			map.put("accountTitle", a.getAccountTitle());
			map.put("parentAccountCode", a.getParentAccountCode());
			map.put("accountClass", getAccountClassName(a.getAccountClass()));
			map.put("accountType", a.getAccountTypeId() != null && typeMap.containsKey(a.getAccountTypeId()) ? typeMap.get(a.getAccountTypeId()) : (a.getAccountGroup() != null ? a.getAccountGroup() : "Detail"));
			return map;
		}).collect(Collectors.toList());
		return ResponseEntity.ok(result);
	}

	@PostMapping("/city/save")
	public ResponseEntity<City> saveCity(@RequestBody Map<String, String> body) {
		String cityName = body.get("cityName");
		City city = chartofAccountService.saveCity(cityName, 1);
		return ResponseEntity.ok(city);
	}

	private String getAccountClassName(Integer accountClass) {
		if (accountClass == null) return "";
		switch (accountClass) {
			case 1: return "Capital";
			case 2: return "Assets";
			case 3: return "Liabilities";
			case 4: return "Expenses";
			case 5: return "Revenue";
			default: return "";
		}
	}

	public static class SaveAccountRequest {
		private ChartofAccount account;
		private Double openingBalance;
		private List<CompanyAllocationRow> allocations = new ArrayList<>();

		public ChartofAccount getAccount() { return account; }
		public void setAccount(ChartofAccount account) { this.account = account; }
		public Double getOpeningBalance() { return openingBalance; }
		public void setOpeningBalance(Double openingBalance) { this.openingBalance = openingBalance; }
		public List<CompanyAllocationRow> getAllocations() { return allocations; }
		public void setAllocations(List<CompanyAllocationRow> allocations) { this.allocations = allocations; }
	}
}
