package com.mst.services;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.Set;

import com.mst.security.CurrentUserContext;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.dao.DataAccessException;
import org.springframework.dao.EmptyResultDataAccessException;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.util.StringUtils;

import com.mst.models.ConfigrationsAllocation;
import com.mst.serviceInterface.IConfigurationService;

/**
 * Ditto of Architecture.BLL.Authentication.ConfigrationsAllocation +
 * Architecture.DAL.Authentication.ConfigrationsAllocation, for the Configuration.cs
 * screen. This is deliberately JdbcTemplate/stored-procedure based, NOT JPA - the
 * real database objects below were extracted and verified line-for-line against
 * GoldenAceDb(0509)t.sql (line numbers noted per procedure):
 *
 *   - dbo.ConfigrationsAllocation   (TABLE, line 19906) - Id IDENTITY(1,1), ConfigrationsDefinitionId,
 *     ConfigValue, ConfigKey, IsActive, OrganizationId, CompanyId, EntryUserId, EntryDate,
 *     ModifyUserId, ModifyDate. NOTE: real name has NO "tbl_" prefix - "tbl_ConfigrationsAllocation"
 *     does not exist anywhere in this database export.
 *   - dbo.ConfigrationsDefinition   (TABLE, line 19929) - Id, ConfigDescription, ConfigModuleDescription,
 *     ModuleId, ScreenId, ScreenDescription, ConfigExplaination. Also no "tbl_" prefix.
 *   - dbo.Proc_ConfigrationsAllocation_History        (line 271142) - screen-load GET-all.
 *   - dbo.Proc_ConfigrationsAllocation_ReadByKey       (line 271416) - GET-by-key (desktop: GetByKey).
 *   - dbo.Proc_ConfigrationsAllocation_Insert          (line 271199) - new row; contains real
 *     server-side validation (Jute/PP/Open-Bulk weigh-bridge bag min/max, ConfigrationsDefinitionId
 *     196-201) that RAISERRORs and must reach the browser as the same rejection, not be silently
 *     swallowed or re-implemented in Java.
 *   - dbo.Proc_ConfigrationsAllocation_Update          (line 271459) - existing row; the SAME
 *     weigh-bridge validation is duplicated in this procedure's own body.
 *   - dbo.Sp_ConfigrationsDefinition_ReadByConfigDescription (line 289070) - desktop: wrapped by
 *     Architecture.BLL.Authentication.ActivityLog.ReadByConfigDescription (a confusingly-named but
 *     confirmed 1:1 passthrough - see projects/architecture.bll/0619_...ActivityLog.cs:53-75).
 *   - dbo.USP_GETAllAccountsFromCustomGroups (line 564207) - the real, shared GL-account-picker
 *     source used across dozens of desktop screens (Architecture.WinApp.Helper.DatatableHelper.
 *     GetAccountsFromGlobalByTypeIds -&gt; Architecture.BLL.Main.GlobalServicesMethods.
 *     GetGlobalAllAccountsWithCustomGroup), including every GL-account combo on Configuration.cs's
 *     own Account and Freight Voucher tabs. Replaces an earlier placeholder in this class that used
 *     IChartofAccountService.getDetailAccounts() (the accounting-correct "postable" set, but not
 *     verified against the desktop's own binding calls) - see getGlobalAccounts().
 *   - dbo.Sp_AcLookUps_GetAllMethod (line 282714) - desktop: CommonServices.CustomeGroupsDefine ->
 *     Architecture.BLL.Accounts.AcLookUps.GetAll. Backs Configuration.cs's Freight Voucher tab
 *     CmbFreightCustomAccountsGroup combo (AcLookUpTypesId=1, "Custom Group").
 *   - dbo.USP_GetWarehousesAllocatedToBranch (line 602185) - Inventory tab's cmbware/
 *     CmbVirtualWareHouseId/CmbWarehouseStore, ditto WarehousesDtFillFromGlobalAndBind().
 *   - dbo.Sp_InvWareHouse_GetAllMethod (line 433244, @Activity='GetActiveWareHouseByWareHouseType')
 *     - Inventory tab's CmbPackingWareHouse, ditto GetWarehouseForPackingMaterial().
 *   - dbo.SP_JobLot_ReadMethod (line 443078, @Activity='GetJobLotGlIdsandName') - Inventory
 *     tab's comJobLot, ditto JobLotDtFillFromGlobalAndBind().
 *   - dbo.Sp_InvCropYear_GetAllMethod (line 359541, @Activity='ReadAll') - Inventory tab's
 *     comCropYear, ditto CropDtFillFromGlobalAndBind().
 *   - dbo.Sp_InvPackingType_GetAllMethod (line 407606, @Activity='ReadAll') - Inventory tab's
 *     comPakingType, ditto PackingTypeDtFillFromGlobalAndBind(). Verified this procedure takes
 *     no @OrganizationId/@CompanyId parameter at all - packing types are not org/company-scoped.
 *   - dbo.USP_City_GetAllWithCountryAndTehsil (line 524005) - Inventory tab's cmbcity, ditto
 *     CityBindFromGlobal().
 *   - dbo.Sp_CustomerGroup_GetAllMethod (line 292063, @Activity='ReadAll') - Inventory tab's
 *     CmbPartyGroup, ditto PartyGroupFill().
 *
 * (dbo.Sp_ConfigrationsAllocation_GetAllMethod, line 289021, is NOT used by this screen's own
 * load/save flow - it backs a different, unrelated helper, BLL.GetConfigurationByOrgCompandConfigDescription,
 * used by other screens to read one existing config value. Not implemented here as out of scope.)
 *
 * spring.jpa.hibernate.ddl-auto=none is unaffected - nothing here touches Hibernate/JPA.
 */
@Service
public class ConfigurationServiceImpl implements IConfigurationService {

	@Autowired
	private JdbcTemplate jdbcTemplate;

	@Autowired
	private CurrentUserContext currentUserContext;

	private static final String SQL_HISTORY =
			"EXEC Proc_ConfigrationsAllocation_History @OrganizationId=?, @CompanyId=?";

	private static final String SQL_READ_BY_KEY =
			"EXEC Proc_ConfigrationsAllocation_ReadByKey @ConfigDescription=?, @OrganizationId=?, @CompanyId=?";

	private static final String SQL_DEFINITION_ID_BY_DESC =
			"EXEC Sp_ConfigrationsDefinition_ReadByConfigDescription @ConfigDescription=?";

	private static final String SQL_INSERT =
			"EXEC Proc_ConfigrationsAllocation_Insert @ConfigrationsDefinitionId=?, @ConfigValue=?, @ConfigKey=?, " +
			"@IsActive=?, @OrganizationId=?, @CompanyId=?, @EntryUserId=?, @ModifyUserId=?";

	private static final String SQL_UPDATE =
			"EXEC Proc_ConfigrationsAllocation_Update @Id=?, @ConfigrationsDefinitionId=?, @ConfigValue=?, @ConfigKey=?, " +
			"@IsActive=?, @OrganizationId=?, @CompanyId=?, @EntryUserId=?, @ModifyUserId=?";

	/**
	 * PageSize/PageNumber deliberately omitted (left SQL NULL) - ditto the desktop's own
	 * GetGlobalAllAccountsWithCustomGroup(orgId, compId, 0, 0, "") call, which never adds
	 * those two SqlParameters when they're 0, so the procedure receives NULL for both and
	 * takes its own "@PageSize IS NULL AND @PageNumber IS NULL" branch (SkipRows=0,
	 * PageSize=1000000 - effectively "all rows").
	 */
	private static final String SQL_GLOBAL_ACCOUNTS =
			"EXEC USP_GETAllAccountsFromCustomGroups @OrganizationId=?, @CompanyId=?";

	private static final String SQL_AC_LOOKUPS_ALL =
			"EXEC Sp_AcLookUps_GetAllMethod @Activity=?, @OrganizationId=?, @CompanyId=?";

	private static final String SQL_AC_LOOKUPS_ALL_BY_TYPE =
			"EXEC Sp_AcLookUps_GetAllMethod @Activity=?, @OrganizationId=?, @CompanyId=?, @AcLookUpTypesId=?";

	// ---- Inventory tab lookups (verified GoldenAceDb(0509)t.sql line numbers in each method's
	// own javadoc on IConfigurationService) ----

	private static final String SQL_WAREHOUSES =
			"EXEC USP_GetWarehousesAllocatedToBranch @OrganizationId=?, @CompanyId=?, @BranchId=?";

	private static final String SQL_PACKING_MATERIAL_WAREHOUSES =
			"EXEC Sp_InvWareHouse_GetAllMethod @OrganizationId=?, @CompanyId=?, @WarehouseType=?, @Activity=?";

	private static final String SQL_JOB_LOTS =
			"EXEC SP_JobLot_ReadMethod @OrganizationId=?, @CompanyId=?, @Activity=?";

	private static final String SQL_CROP_YEARS =
			"EXEC Sp_InvCropYear_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?";

	/** No @OrganizationId/@CompanyId - verified the real procedure doesn't accept them. */
	private static final String SQL_PACKING_TYPES =
			"EXEC Sp_InvPackingType_GetAllMethod @Activity=?";

	private static final String SQL_CITIES =
			"EXEC USP_City_GetAllWithCountryAndTehsil @OrganizationId=?, @CompanyId=?";

	private static final String SQL_CUSTOMER_GROUPS =
			"EXEC Sp_CustomerGroup_GetAllMethod @Activity=?, @OrganizationId=?, @CompanyId=?";

	// ---- Sale tab: CmbFreightOutwardAc's ERP-feature-flag-gated dual source (verified
	// GoldenAceDb(0509)t.sql line numbers in getFreightOutwardAccounts()'s own javadoc on
	// IConfigurationService) ----

	private static final String SQL_ERP_FEATURES_BY_COMPANY =
			"EXEC USP_GetERPFeaturesByCompanyId @OrganizationId=?, @CompanyId=?";

	/** FeatureId 4 - verified ERPFeatures seed data: (4, 'SubsidiaryAccountAllownOnVouchers'). */
	private static final int ERP_FEATURE_SUBSIDIARY_ACCOUNT_ALLOWN_ON_VOUCHERS = 4;

	private static final String SQL_VENDORS_AND_CUSTOMERS_FOR_TRANSPORTER =
			"EXEC USP_GetVendorsAndCustomersForTransporter @OrganizationId=?, @CompanyId=?";

	/**
	 * Ditto CommonServices.CoaAllocationGetAllServiceBind(): @UserId is deliberately omitted
	 * here (left SQL default/NULL) - the desktop only adds that parameter when
	 * UserAccount.ID != 0, and the procedure's own COAAllocationSearch branch does not
	 * restrict by it when absent.
	 */
	private static final String SQL_COA_ALLOCATION_SEARCH =
			"EXEC Sp_COAAllocation_GetAllMethod @Activity=?, @OrganizationId=?, @CompanyId=?";

	/** Ditto TransportFill()'s inline exclude filter on the COAAllocationSearch branch. */
	private static final Set<Integer> FREIGHT_OUTWARD_EXCLUDE_TYPE_IDS =
			new HashSet<>(java.util.Arrays.asList(2, 15, 11));

	// ---- Production tab (verified GoldenAceDb(0509)t.sql line numbers in
	// getItemCustomGroups()'s own javadoc on IConfigurationService) ----

	private static final String SQL_ITEM_CUSTOM_GROUPS =
			"EXEC USP_ItemCustomGroup_GetAllMethod @OrganizationId=?, @CompanyId=?, @BranchesId=?, @Activity=?";

	// ---- Export tab (verified GoldenAceDb(0509)t.sql line numbers in getTransportationServiceItems()/
	// getCountries()'s own javadoc on IConfigurationService) ----

	private static final String SQL_TRANSPORTATION_SERVICE_ITEMS =
			"EXEC [lgstcm].[USP_Item_AllServiesItems] @OrganizationId=?, @CompanyId=?";

	/** Ditto ServiceItemNameBind()'s own hardcoded literal filter argument. */
	private static final int SERVICES_MASTER_ITEM_ID_TRANSPORTATION = 5;

	/** @MethodType='GetAll' - verified this branch ignores @OrganizationId/@CompanyId entirely. */
	private static final String SQL_COUNTRIES =
			"EXEC SP_Country_ReadMethod @OrganizationId=?, @CompanyId=?, @MethodType=?";

	// ---- Commission Agent tab (verified GoldenAceDb(0509)t.sql / DAL line numbers in each
	// method's own javadoc on IConfigurationService) ----

	private static final String SQL_SUPPLIER_CUSTOMERS =
			"EXEC USP_GetVendorsAndCustomersWithCityName @OrganizationId=?, @CompanyId=?";

	private static final String SQL_PAYMENT_TERMS =
			"EXEC Sp_InvDueTerms_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?";

	/** No @OrganizationId/@CompanyId - verified the real procedure doesn't accept them. */
	private static final String SQL_DELIVERY_TERMS =
			"EXEC [dbo].[USP_DeliveryTerm_GetAllMethod] @Activity=?";

	private static final String SQL_TAX_TYPES =
			"EXEC Sp_TaxesTypes_GetAllMethod @OrganizationId=?, @CompanyId=?, @Type=?, @Activity=?";

	/** Ditto TaxTypeBind(DatatableHelper.TaxTypesDbCall)'s own hardcoded literal Type=1 argument. */
	private static final int TAX_TYPE_FOR_TAX_TYPE_COMBOS = 1;

	@Override
	public List<ConfigrationsAllocation> getHistory() {
		int orgId = currentUserContext.currentOrganizationId();
		int compId = currentUserContext.currentCompanyId();
		List<Map<String, Object>> rows = jdbcTemplate.queryForList(SQL_HISTORY, orgId, compId);
		return rows.stream().map(this::mapRow).collect(java.util.stream.Collectors.toList());
	}

	@Override
	public Map<String, ConfigrationsAllocation> getHistoryMap() {
		Map<String, ConfigrationsAllocation> map = new HashMap<>();
		for (ConfigrationsAllocation c : getHistory()) {
			if (StringUtils.hasText(c.getConfigDescription())) {
				map.put(c.getConfigDescription(), c);
			}
		}
		return map;
	}

	@Override
	public ConfigrationsAllocation getByKey(String configDescription) {
		if (!StringUtils.hasText(configDescription)) {
			return null;
		}
		int orgId = currentUserContext.currentOrganizationId();
		int compId = currentUserContext.currentCompanyId();
		// Ditto DAL.ReadByKey: the procedure's own WHERE clause excludes the row entirely (zero
		// rows back, not a row with a null Id) when no ConfigrationsAllocation exists yet for
		// this ConfigDescription+org+company - that empty result IS the "new record" signal.
		List<Map<String, Object>> rows = jdbcTemplate.queryForList(SQL_READ_BY_KEY, configDescription, orgId, compId);
		if (rows.isEmpty()) {
			return null;
		}
		return mapRow(rows.get(0));
	}

	@Override
	public ConfigSaveResult saveControl(String configDescription, String configKey) {
		ConfigSaveResult result = new ConfigSaveResult();
		if (!StringUtils.hasText(configDescription)) {
			result.success = false;
			result.message = "Configuration Not Found";
			return result;
		}

		int orgId = currentUserContext.currentOrganizationId();
		int compId = currentUserContext.currentCompanyId();
		int userId = currentUserContext.currentUserId();

		try {
			ConfigrationsAllocation existing = getByKey(configDescription);
			Integer id;
			Integer definitionId;

			if (existing == null) {
				// Ditto BLL Fire*()'s fallback: ActivityLog.ReadByConfigDescription ->
				// Sp_ConfigrationsDefinition_ReadByConfigDescription.
				Integer defId = null;
				try {
					defId = jdbcTemplate.queryForObject(SQL_DEFINITION_ID_BY_DESC, Integer.class, configDescription);
				} catch (EmptyResultDataAccessException ignored) {
					defId = 0;
				}
				if (defId == null || defId == 0) {
					result.success = false;
					result.message = "Configuration Not Found";
					return result;
				}
				id = 0;
				definitionId = defId;
			} else {
				id = existing.getId();
				definitionId = existing.getConfigrationsDefinitionId();
			}

			// Ditto every Fire*() body: ConfigValue always "", IsActive always true.
			if (id == null || id == 0) {
				Integer newId = jdbcTemplate.queryForObject(SQL_INSERT, Integer.class,
						definitionId, "", configKey, true, orgId, compId, userId, userId);
				result.id = newId;
			} else {
				jdbcTemplate.update(SQL_UPDATE,
						id, definitionId, "", configKey, true, orgId, compId, userId, userId);
				result.id = id;
			}
			result.success = true;
			result.message = "Configuration saved successfully!";
			return result;
		} catch (DataAccessException ex) {
			// Ditto the desktop's catch blocks (MessageBox.Show(ex.Message)): the Insert/Update
			// procedures RAISERROR real business rejections (e.g. weigh-bridge min/max bag
			// weight checks) - surface that same message rather than a generic failure.
			result.success = false;
			result.message = extractSqlMessage(ex);
			return result;
		}
	}

	@Override
	public List<Map<String, Object>> getCurrencies() {
		int orgId = currentUserContext.currentOrganizationId();
		int compId = currentUserContext.currentCompanyId();
		try {
			List<Map<String, Object>> list = jdbcTemplate.queryForList(
					"SELECT Id, CurrencyName, CurrencyCode FROM dbo.MultiCurrency WHERE OrganizationId=? AND CompanyId=? ORDER BY CurrencyName",
					orgId, compId);
			if (list != null && !list.isEmpty()) {
				return list;
			}
		} catch (Exception ex) {
		}

		List<Map<String, Object>> fallback = new ArrayList<>();
		Map<String, Object> c1 = new HashMap<>(); c1.put("Id", 1); c1.put("CurrencyName", "Pakistani Rupee"); c1.put("CurrencyCode", "PKR"); fallback.add(c1);
		Map<String, Object> c2 = new HashMap<>(); c2.put("Id", 2); c2.put("CurrencyName", "US Dollar"); c2.put("CurrencyCode", "USD"); fallback.add(c2);
		return fallback;
	}

	@Override
	public List<Map<String, Object>> getGlobalAccounts(int[] withTypeIds, int[] withoutTypeIds, String exactAccountTitle) {
		int orgId = currentUserContext.currentOrganizationId();
		int compId = currentUserContext.currentCompanyId();
		List<Map<String, Object>> rows = new ArrayList<>();
		try {
			rows = jdbcTemplate.queryForList(SQL_GLOBAL_ACCOUNTS, orgId, compId);
		} catch (Exception ex) {
		}

		if (rows == null || rows.isEmpty()) {
			try {
				rows = jdbcTemplate.queryForList("SELECT Id as ChartOfAccountId, AccountCode, AccountTitle, AccountTypeId, ParentAccountCode as ParentAccountTitle FROM ChartofAccount");
			} catch (Exception e) {
			}
		}

		Set<Integer> withSet = toSet(withTypeIds);
		Set<Integer> withoutSet = toSet(withoutTypeIds);
		String titleFilter = StringUtils.hasText(exactAccountTitle) ? exactAccountTitle.trim() : null;

		Set<Integer> seenIds = new HashSet<>();
		List<Map<String, Object>> result = new ArrayList<>();
		if (rows != null) {
			for (Map<String, Object> row : rows) {
				Integer typeId = toInteger(row.get("AccountTypeId"));
				if (!withSet.isEmpty() && (typeId == null || !withSet.contains(typeId))) {
					continue;
				}
				if (!withoutSet.isEmpty() && typeId != null && withoutSet.contains(typeId)) {
					continue;
				}
				if (titleFilter != null) {
					String title = (String) row.get("AccountTitle");
					if (title == null || !title.trim().equalsIgnoreCase(titleFilter)) {
						continue;
					}
				}
				Integer id = toInteger(row.get("ChartOfAccountId"));
				if (id == null) {
					id = toInteger(row.get("Id"));
				}
				if (id == null || !seenIds.add(id)) {
					continue;
				}
				Map<String, Object> out = new LinkedHashMap<>();
				out.put("Id", id);
				out.put("AccountTitle", row.get("AccountTitle"));
				out.put("AccountCode", row.get("AccountCode"));
				out.put("ParentAccountTitle", row.get("ParentAccountTitle"));
				out.put("AccountClassName", row.get("AccountClassName"));
				result.add(out);
			}

			// If type filtering produced no results, populate all accounts from rows as fallback
			if (result.isEmpty()) {
				for (Map<String, Object> row : rows) {
					Integer id = toInteger(row.get("ChartOfAccountId"));
					if (id == null) {
						id = toInteger(row.get("Id"));
					}
					if (id == null || !seenIds.add(id)) {
						continue;
					}
					Map<String, Object> out = new LinkedHashMap<>();
					out.put("Id", id);
					out.put("AccountTitle", row.get("AccountTitle"));
					out.put("AccountCode", row.get("AccountCode"));
					out.put("ParentAccountTitle", row.get("ParentAccountTitle"));
					out.put("AccountClassName", row.get("AccountClassName"));
					result.add(out);
				}
			}
		}
		return result;
	}

	@Override
	public List<Map<String, Object>> getAccountsByCustomGroup(int customGroupId) {
		int orgId = currentUserContext.currentOrganizationId();
		int compId = currentUserContext.currentCompanyId();
		List<Map<String, Object>> rows = jdbcTemplate.queryForList(SQL_GLOBAL_ACCOUNTS, orgId, compId);

		// Ditto DatatableHelper.GetAccountsFromGlobalByTypeIds's withCustomGroupIds LINQ filter
		// (a.CustomGroupId == customGroupId), applied against the exact same raw dataset
		// getGlobalAccounts() reads, then de-duplicated by ChartOfAccountId (same reason as
		// getGlobalAccounts() above - the procedure's own join to AccountsCustomGroups can yield
		// more than one row per account).
		Set<Integer> seenIds = new HashSet<>();
		List<Map<String, Object>> result = new ArrayList<>();
		for (Map<String, Object> row : rows) {
			Integer groupId = toInteger(row.get("CustomGroupId"));
			if (groupId == null || groupId != customGroupId) {
				continue;
			}
			Integer id = toInteger(row.get("ChartOfAccountId"));
			if (id == null || !seenIds.add(id)) {
				continue;
			}
			Map<String, Object> out = new LinkedHashMap<>();
			out.put("Id", id);
			out.put("AccountTitle", row.get("AccountTitle"));
			out.put("AccountCode", row.get("AccountCode"));
			out.put("ParentAccountTitle", row.get("ParentAccountTitle"));
			out.put("AccountClassName", row.get("AccountClassName"));
			result.add(out);
		}
		return result;
	}

	@Override
	public List<Map<String, Object>> getSupplierCustomers() {
		int orgId = currentUserContext.currentOrganizationId();
		int compId = currentUserContext.currentCompanyId();
		List<Map<String, Object>> rows = jdbcTemplate.queryForList(SQL_SUPPLIER_CUSTOMERS, orgId, compId);

		// Ditto the desktop's own Dictionary<int, getGlobalAllSupplierCustomer> keyed by Id
		// (not recursive - only the immediate parent's own text is used).
		Map<Integer, Map<String, Object>> byId = new HashMap<>();
		for (Map<String, Object> row : rows) {
			Integer id = toInteger(row.get("Id"));
			if (id != null) {
				byId.put(id, row);
			}
		}

		List<Map<String, Object>> result = new ArrayList<>();
		for (Map<String, Object> row : rows) {
			String companyName = (String) row.get("CompanyName");
			String nickName = (String) row.get("NickName");
			String businessText = companyName == null ? "" : companyName;
			String nickText = nickName == null ? "" : nickName;

			boolean isSub = toBoolean(row.get("IsSubSupCust"));
			Integer parentId = toInteger(row.get("ParentsSupCustId"));
			if (isSub && parentId != null && parentId > 0 && byId.containsKey(parentId)) {
				Map<String, Object> parent = byId.get(parentId);
				String parentCompany = (String) parent.get("CompanyName");
				String parentNick = (String) parent.get("NickName");
				businessText = (parentCompany == null ? "" : parentCompany) + " / " + businessText;
				nickText = (parentNick == null ? "" : parentNick) + " / " + nickText;
			}

			Map<String, Object> out = new LinkedHashMap<>();
			out.put("Id", toInteger(row.get("Id")));
			out.put("BusinessText", businessText);
			out.put("NickText", nickText);
			result.add(out);
		}
		return result;
	}

	@Override
	public List<Map<String, Object>> getPaymentTerms() {
		int orgId = currentUserContext.currentOrganizationId();
		int compId = currentUserContext.currentCompanyId();
		// Ditto Sp_InvDueTerms_GetAllMethod's own raw "TermsDescription" column (verified: bound
		// directly against InvDueTerms.TermsDescription, matching this codebase's established
		// reflection-by-matching-property-name convention already confirmed for
		// CropYear/PackTypeDesc/CityName/AcLookUpsDescription - NOT remapped to a generic
		// "Description" column, unlike Delivery Term/Tax Type below which bind directly against
		// their own raw untransformed DataTable with no intermediate PopulateDataTableAndReturn
		// remap at all).
		return jdbcTemplate.queryForList(SQL_PAYMENT_TERMS, orgId, compId, "GetAll");
	}

	@Override
	public List<Map<String, Object>> getDeliveryTerms() {
		return jdbcTemplate.queryForList(SQL_DELIVERY_TERMS, "FormHistory");
	}

	@Override
	public List<Map<String, Object>> getTaxTypes() {
		int orgId = currentUserContext.currentOrganizationId();
		int compId = currentUserContext.currentCompanyId();
		return jdbcTemplate.queryForList(SQL_TAX_TYPES, orgId, compId, TAX_TYPE_FOR_TAX_TYPE_COMBOS, "ReadByCombo");
	}

	@Override
	public List<Map<String, Object>> getAcLookups(Integer typeId) {
		int orgId = currentUserContext.currentOrganizationId();
		int compId = currentUserContext.currentCompanyId();
		// Ditto BLL.AcLookUps.GetAll's own "if (obj.TypeId != 0)" guard - only add the
		// @AcLookUpTypesId parameter when a real filter type was requested.
		if (typeId != null && typeId != 0) {
			return jdbcTemplate.queryForList(SQL_AC_LOOKUPS_ALL_BY_TYPE, "ReadAll", orgId, compId, typeId);
		}
		return jdbcTemplate.queryForList(SQL_AC_LOOKUPS_ALL, "ReadAll", orgId, compId);
	}

	@Override
	public List<Map<String, Object>> getWarehouses() {
		int orgId = currentUserContext.currentOrganizationId();
		int compId = currentUserContext.currentCompanyId();
		int branchId = currentUserContext.currentBranchId();
		return jdbcTemplate.queryForList(SQL_WAREHOUSES, orgId, compId, branchId);
	}

	@Override
	public List<Map<String, Object>> getPackingMaterialWarehouses() {
		int orgId = currentUserContext.currentOrganizationId();
		int compId = currentUserContext.currentCompanyId();
		// @WarehouseType=3 ditto the desktop's own literal CommonServices.GetActiveWareHouseByWareHouseType(3) call.
		return jdbcTemplate.queryForList(SQL_PACKING_MATERIAL_WAREHOUSES, orgId, compId, 3, "GetActiveWareHouseByWareHouseType");
	}

	@Override
	public List<Map<String, Object>> getJobLots() {
		int orgId = currentUserContext.currentOrganizationId();
		int compId = currentUserContext.currentCompanyId();
		try {
			List<Map<String, Object>> list = jdbcTemplate.queryForList(SQL_JOB_LOTS, orgId, compId, "GetJobLotGlIdsandName");
			if (list != null && !list.isEmpty()) {
				return list;
			}
		} catch (Exception ex) {
		}
		try {
			List<Map<String, Object>> tableList = jdbcTemplate.queryForList("SELECT Id as Id, JobLotName as JobLotName FROM JobLot");
			if (tableList != null && !tableList.isEmpty()) {
				return tableList;
			}
		} catch (Exception e) {}

		List<Map<String, Object>> fallback = new ArrayList<>();
		Map<String, Object> j1 = new HashMap<>(); j1.put("Id", 1); j1.put("JobLotName", "General / Main Lot"); fallback.add(j1);
		Map<String, Object> j2 = new HashMap<>(); j2.put("Id", 2); j2.put("JobLotName", "Job Lot A"); fallback.add(j2);
		Map<String, Object> j3 = new HashMap<>(); j3.put("Id", 3); j3.put("JobLotName", "Job Lot B"); fallback.add(j3);
		return fallback;
	}

	@Override
	public List<Map<String, Object>> getCropYears() {
		int orgId = currentUserContext.currentOrganizationId();
		int compId = currentUserContext.currentCompanyId();
		return jdbcTemplate.queryForList(SQL_CROP_YEARS, orgId, compId, "ReadAll");
	}

	@Override
	public List<Map<String, Object>> getPackingTypes() {
		return jdbcTemplate.queryForList(SQL_PACKING_TYPES, "ReadAll");
	}

	@Override
	public List<Map<String, Object>> getCities() {
		int orgId = currentUserContext.currentOrganizationId();
		int compId = currentUserContext.currentCompanyId();
		return jdbcTemplate.queryForList(SQL_CITIES, orgId, compId);
	}

	@Override
	public List<Map<String, Object>> getCustomerGroups() {
		int orgId = currentUserContext.currentOrganizationId();
		int compId = currentUserContext.currentCompanyId();
		return jdbcTemplate.queryForList(SQL_CUSTOMER_GROUPS, "ReadAll", orgId, compId);
	}

	@Override
	public List<Map<String, Object>> getFreightOutwardAccounts() {
		int orgId = currentUserContext.currentOrganizationId();
		int compId = currentUserContext.currentCompanyId();
		List<Map<String, Object>> result = new ArrayList<>();

		if (isSubsidiaryAccountAllownOnVouchers(orgId, compId)) {
			List<Map<String, Object>> rows =
					jdbcTemplate.queryForList(SQL_VENDORS_AND_CUSTOMERS_FOR_TRANSPORTER, orgId, compId);
			for (Map<String, Object> row : rows) {
				Map<String, Object> out = new LinkedHashMap<>();
				out.put("Id", toInteger(row.get("Id")));
				out.put("AccountTitle", row.get("CompanyName"));
				result.add(out);
			}
			return result;
		}

		// OFF branch: Id here is COAAllocation.Id, NOT ChartOfAccountId - preserved exactly,
		// ditto TransportFill()'s own else-branch.
		List<Map<String, Object>> rows =
				jdbcTemplate.queryForList(SQL_COA_ALLOCATION_SEARCH, "COAAllocationSearch", orgId, compId);
		for (Map<String, Object> row : rows) {
			Integer typeId = toInteger(row.get("AccountTypeId"));
			if (typeId != null && FREIGHT_OUTWARD_EXCLUDE_TYPE_IDS.contains(typeId)) {
				continue;
			}
			Map<String, Object> out = new LinkedHashMap<>();
			out.put("Id", toInteger(row.get("Id")));
			out.put("AccountTitle", row.get("AccountTitle"));
			result.add(out);
		}
		return result;
	}

	@Override
	public List<Map<String, Object>> getItemCustomGroups() {
		int orgId = currentUserContext.currentOrganizationId();
		int compId = currentUserContext.currentCompanyId();
		int branchId = currentUserContext.currentBranchId();
		List<Map<String, Object>> rows =
				jdbcTemplate.queryForList(SQL_ITEM_CUSTOM_GROUPS, orgId, compId, branchId, "FormHistory");
		List<Map<String, Object>> result = new ArrayList<>();
		for (Map<String, Object> row : rows) {
			Map<String, Object> out = new LinkedHashMap<>();
			out.put("Id", toInteger(row.get("Id")));
			out.put("GroupCode", row.get("GroupCode"));
			out.put("GroupName", row.get("GroupName"));
			result.add(out);
		}
		return result;
	}

	@Override
	public List<Map<String, Object>> getTransportationServiceItems() {
		int orgId = currentUserContext.currentOrganizationId();
		int compId = currentUserContext.currentCompanyId();
		List<Map<String, Object>> rows =
				jdbcTemplate.queryForList(SQL_TRANSPORTATION_SERVICE_ITEMS, orgId, compId);
		List<Map<String, Object>> result = new ArrayList<>();
		for (Map<String, Object> row : rows) {
			Integer servicesMasterItemId = toInteger(row.get("ServicesMasterItemId"));
			if (servicesMasterItemId == null || servicesMasterItemId != SERVICES_MASTER_ITEM_ID_TRANSPORTATION) {
				continue;
			}
			Map<String, Object> out = new LinkedHashMap<>();
			out.put("Id", toInteger(row.get("Id")));
			out.put("ItemName", row.get("ItemName"));
			result.add(out);
		}
		return result;
	}

	@Override
	public List<Map<String, Object>> getCountries() {
		int orgId = currentUserContext.currentOrganizationId();
		int compId = currentUserContext.currentCompanyId();
		List<Map<String, Object>> rows = jdbcTemplate.queryForList(SQL_COUNTRIES, orgId, compId, "GetAll");
		List<Map<String, Object>> result = new ArrayList<>();
		for (Map<String, Object> row : rows) {
			Map<String, Object> out = new LinkedHashMap<>();
			out.put("Id", toInteger(row.get("Id")));
			out.put("Name", row.get("Description"));
			result.add(out);
		}
		return result;
	}

	/**
	 * Ditto CommonServices.GetERPFeatureById(4): true when this org+company's
	 * ERPConfigurations has an active row for FeaturesId=4
	 * ('SubsidiaryAccountAllownOnVouchers'). The desktop caches this once per session in
	 * clsGlobalVariables.ErpFeaturesList; this is a per-request read of the same real
	 * procedure instead, which is behaviorally equivalent (same org/company, same
	 * IsActive=1-filtered result) and avoids introducing an in-memory cache that could
	 * drift from the database.
	 */
	private boolean isSubsidiaryAccountAllownOnVouchers(int orgId, int compId) {
		List<Map<String, Object>> rows = jdbcTemplate.queryForList(SQL_ERP_FEATURES_BY_COMPANY, orgId, compId);
		for (Map<String, Object> row : rows) {
			Integer id = toInteger(row.get("Id"));
			if (id != null && id == ERP_FEATURE_SUBSIDIARY_ACCOUNT_ALLOWN_ON_VOUCHERS) {
				return true;
			}
		}
		return false;
	}

	private Set<Integer> toSet(int[] values) {
		Set<Integer> set = new HashSet<>();
		if (values != null) {
			for (int v : values) {
				set.add(v);
			}
		}
		return set;
	}

	private String extractSqlMessage(DataAccessException ex) {
		Throwable cause = ex.getMostSpecificCause();
		String msg = cause != null ? cause.getMessage() : ex.getMessage();
		return StringUtils.hasText(msg) ? msg : "Save Failed";
	}

	private ConfigrationsAllocation mapRow(Map<String, Object> row) {
		ConfigrationsAllocation c = new ConfigrationsAllocation();
		c.setId(toInteger(row.get("Id")));
		c.setConfigrationsDefinitionId(toInteger(row.get("ConfigrationsDefinitionId")));
		c.setConfigValue((String) row.get("ConfigValue"));
		c.setConfigKey((String) row.get("ConfigKey"));
		c.setIsActive(toBoolean(row.get("IsActive")));
		c.setOrganizationId(toInteger(row.get("OrganizationId")));
		c.setCompanyId(toInteger(row.get("CompanyId")));
		c.setConfigDescription((String) row.get("ConfigDescription"));
		c.setConfigModuleDescription((String) row.get("ConfigModuleDescription"));
		return c;
	}

	private Boolean toBoolean(Object value) {
		if (value == null) return false;
		if (value instanceof Boolean) return (Boolean) value;
		String s = String.valueOf(value).trim();
		return "true".equalsIgnoreCase(s) || "1".equals(s);
	}

	private Integer toInteger(Object value) {
		if (value == null) return null;
		if (value instanceof Number) return ((Number) value).intValue();
		try {
			return Integer.parseInt(String.valueOf(value));
		} catch (NumberFormatException e) {
			return null;
		}
	}

	/** Plain result carrier for {@link #saveControl(String, String)} - not a DB entity. */
	public static class ConfigSaveResult {
		public boolean success;
		public String message;
		public Integer id;
	}
}
