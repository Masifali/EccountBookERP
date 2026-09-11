package com.mst.controllers;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import com.mst.models.ConfigrationsAllocation;
import com.mst.serviceInterface.IConfigurationService;
import com.mst.services.ConfigurationServiceImpl.ConfigSaveResult;

/**
 * Ditto of the desktop Configuration.cs screen's data operations. OrganizationId,
 * CompanyId and UserId are never taken from the request - IConfigurationService
 * resolves them from the authenticated session (CurrentUserContext) exactly like
 * UserAccount.OrganizationId/CompanyId/ID on the desktop, so a client can't save
 * against, or read, another company's/organization's configuration.
 */
@RestController
@RequestMapping("/api/configurations")
public class ConfigurationApiController {

	@Autowired
	private IConfigurationService configurationService;

	/** Ditto AcfrmDefCoa-style screen load: ConfigrationsAllocation.HistoryConfiquration(...). */
	@GetMapping("/history")
	public ResponseEntity<List<ConfigrationsAllocation>> getHistory() {
		return ResponseEntity.ok(configurationService.getHistory());
	}

	/** Same data as {@link #getHistory()}, keyed by ConfigDescription for direct control lookup. */
	@GetMapping("/map")
	public ResponseEntity<Map<String, ConfigrationsAllocation>> getHistoryMap() {
		return ResponseEntity.ok(configurationService.getHistoryMap());
	}

	/** Ditto ConfigrationsAllocation.GetByKey(configDescription, ...). */
	@GetMapping("/by-key")
	public ResponseEntity<ConfigrationsAllocation> getByKey(@RequestParam("configDescription") String configDescription) {
		ConfigrationsAllocation config = configurationService.getByKey(configDescription);
		if (config == null) {
			return ResponseEntity.notFound().build();
		}
		return ResponseEntity.ok(config);
	}

	/**
	 * Ditto every one of the desktop's per-control save handlers (Fire / FireDDL /
	 * FireRadioButton / FireTextBox / FireDateTime / FireDDLInfaragastic): one control's
	 * value, saved immediately on its change/leave event - never a full-form submit.
	 */
	@PostMapping("/save-control")
	public ResponseEntity<Map<String, Object>> saveControl(@RequestBody SaveControlRequest req) {
		ConfigSaveResult result = configurationService.saveControl(req.getConfigDescription(), req.getConfigKey());
		Map<String, Object> resp = new HashMap<>();
		resp.put("success", result.success);
		resp.put("message", result.message);
		resp.put("id", result.id);
		return ResponseEntity.ok(resp);
	}

	@GetMapping("/currencies")
	public ResponseEntity<List<Map<String, Object>>> getCurrencies() {
		return ResponseEntity.ok(configurationService.getCurrencies());
	}

	/**
	 * Ditto DatatableHelper.GetAccountsFromGlobalByTypeIds - used client-side to re-populate a
	 * combo's option list when a coupled checkbox changes the type filter (e.g. Freight Voucher
	 * tab's chkIncludeBankAccountsInFreightVoucherCreditAccount toggling
	 * CmbDefaultFreightVoucherCreditAccountId between type {2} and {2,8,15}) without a full page
	 * reload - ditto the desktop's own re-bind-in-place on that same checkbox's CheckedChanged.
	 *
	 * @param with    comma-separated AccountTypeId list to include (optional)
	 * @param without comma-separated AccountTypeId list to exclude (optional)
	 * @param title   exact (case-insensitive) AccountTitle match (optional)
	 */
	@GetMapping("/global-accounts")
	public ResponseEntity<List<Map<String, Object>>> getGlobalAccounts(
			@RequestParam(value = "with", required = false) String with,
			@RequestParam(value = "without", required = false) String without,
			@RequestParam(value = "title", required = false) String title) {
		return ResponseEntity.ok(configurationService.getGlobalAccounts(parseIntCsv(with), parseIntCsv(without), title));
	}

	/** Ditto CommonServices.CustomeGroupsDefine(typeId) -&gt; AcLookUps.GetAll. */
	@GetMapping("/ac-lookups")
	public ResponseEntity<List<Map<String, Object>>> getAcLookups(@RequestParam(value = "typeId", required = false) Integer typeId) {
		return ResponseEntity.ok(configurationService.getAcLookups(typeId));
	}

	/**
	 * Commission Agent tab (ditto Configuration.cs's CmbCustomGroupForWHTAccounts_Leave()/
	 * CmbCustomGroupForWHTAccountsSale_Leave(), both firing this same in-memory re-filter):
	 * live-refreshes CmbDefaultWhtAccountPurchaseIdForCommissionAgentPortal /
	 * CmbDefaultWhtAccountSaleIdForCommissionAgentPortal whenever the user changes
	 * CmbCustomGroupForWHTAccounts / CmbCustomGroupForWHTAccountsSale - see
	 * countx_configuration.js's applyWhtAccountsCoupling().
	 */
	@GetMapping("/accounts-by-custom-group")
	public ResponseEntity<List<Map<String, Object>>> getAccountsByCustomGroup(@RequestParam("customGroupId") int customGroupId) {
		return ResponseEntity.ok(configurationService.getAccountsByCustomGroup(customGroupId));
	}

	/** Ditto SP_JobLot_ReadMethod(@Activity='GetJobLotGlIdsandName') - Expense Voucher's (and any
	 *  other voucher's) "Job/Lot" detail-grid combo. */
	@GetMapping("/job-lots")
	public ResponseEntity<List<Map<String, Object>>> getJobLots() {
		return ResponseEntity.ok(configurationService.getJobLots());
	}

	private int[] parseIntCsv(String csv) {
		if (csv == null || csv.trim().isEmpty()) {
			return null;
		}
		String[] parts = csv.split(",");
		List<Integer> values = new ArrayList<>();
		for (String p : parts) {
			p = p.trim();
			if (!p.isEmpty()) {
				values.add(Integer.parseInt(p));
			}
		}
		int[] arr = new int[values.size()];
		for (int i = 0; i < arr.length; i++) {
			arr[i] = values.get(i);
		}
		return arr;
	}

	public static class SaveControlRequest {
		private String configDescription;
		private String configKey;

		public String getConfigDescription() { return configDescription; }
		public void setConfigDescription(String configDescription) { this.configDescription = configDescription; }
		public String getConfigKey() { return configKey; }
		public void setConfigKey(String configKey) { this.configKey = configKey; }
	}
}
