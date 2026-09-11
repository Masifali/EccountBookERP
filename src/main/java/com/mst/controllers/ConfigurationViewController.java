package com.mst.controllers;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;

import com.mst.serviceInterface.IConfigurationService;

/**
 * Serves the ditto copy of the desktop Configuration.cs screen.
 *
 * The page itself loads every tab's current values through GET /api/configurations/map
 * (client-side, on document ready) rather than server-rendered, so every control's
 * initial state goes through the exact same code path as a later live change - see
 * countx_configuration.js. The GL-account/lookup combos below ARE rendered server-side
 * since their option lists rarely change per request; each is the exact same
 * type-filtered (or exact-title-filtered) call the desktop's own AccountBindFromGlobal()/
 * CmbDefaultFreightVoucherCreditAccountFillFromGlobal() make against the real
 * USP_GETAllAccountsFromCustomGroups / Sp_AcLookUps_GetAllMethod procedures - see
 * IConfigurationService#getGlobalAccounts and CONFIGURATION-PROGRESS.md for the verified
 * AccountTypeId table these filters are built from.
 */
@Controller
@RequestMapping("/configurations")
public class ConfigurationViewController {

	@Autowired
	private IConfigurationService configurationService;

	// AccountTypes.Id (verified GoldenAceDb(0509)t.sql AccountTypes seed data):
	// 2=Cash Equivalent, 4=Inventory, 6=Other Assets, 8=Other Liabilities, 9=Capital & Equity,
	// 11=Operating Expenses, 12=Cost Of Sales, 13=Financial Expenses, 15=Bank Equivalent,
	// 20=Other Expenses, 21=Selling Expenses.
	private static final int[] TYPE_CASH_EQUIVALENT = { 2 };
	private static final int[] TYPE_CAPITAL_EQUITY = { 9 };
	private static final int[] TYPE_OTHER_ASSETS_LIABILITIES = { 6, 8 };
	private static final int[] TYPE_EXPENSE_ACCOUNTS = { 11, 13, 20, 21 };
	private static final int[] TYPE_EXCLUDE_FOR_BANK_RECONCILIATION = { 2, 4, 11, 12, 15 };
	private static final int[] TYPE_FREIGHT_CREDIT_DEFAULT = { 2 };
	private static final String TITLE_DAY_BOOK_TEMPORARY_ACCOUNT = "ADJUSTMENT AND TEMPORARY ACCOUNT";
	private static final int AC_LOOKUP_TYPE_CUSTOM_GROUP = 1;

	// Inventory tab GL-account filters (verified AccountBindFromGlobal() local variables):
	private static final int[] TYPE_INVENTORY_CONTROL = { 4 };
	private static final int[] TYPE_OPERATING_COST_OF_SALES = { 11, 12 };
	private static final int[] TYPE_EXCLUDE_STOCK_ACCOUNTS = { 2, 10, 11, 12, 15 };

	// Purchase tab GL-account filter (verified AccountBindFromGlobal() local variable
	// accountsFromGlobalByTypeIds10 - CmbAdjustmentAcForRateCutAmountInCaseOfAccessWeightReceived).
	// CmbFreightInwardAc on the same tab reuses TYPE_EXCLUDE_STOCK_ACCOUNTS above (verified
	// identical to accountsFromGlobalByTypeIds, the same exclude-set variable Inventory's
	// CmbAgainstStockAccount/CmbDifferenceBalanceAc use).
	private static final int[] TYPE_COST_OF_SALES = { 12 };

	// Sale tab GL-account filters (verified AccountBindFromGlobal() local variables):
	// CmbFOCInventoryExpenseAccount and CmbZakatInventoryExpensesAccount. CmbFreightOutwardAc
	// is NOT a simple type-filter combo - see getFreightOutwardAccounts() below.
	private static final int[] TYPE_FOC_INVENTORY_EXPENSE = { 11, 20, 21 };
	private static final int[] TYPE_ZAKAT_INVENTORY_EXPENSE = { 8, 11, 20, 21 };

	// WeighBridge tab GL-account filter (verified AccountBindFromGlobal() local variable
	// accountsFromGlobalByTypeIds9 - CmbOtherIncomeWithWeighbridgeAccount). CmbCashWithWeighbridgeAccount
	// on the same tab reuses TYPE_CASH_EQUIVALENT below (verified identical to
	// accountsFromGlobalByTypeIds2, the same {2} filter cmbCCA/cmbBCA/CmbCashAccount use).
	private static final int[] TYPE_REVENUE_SALES = { 10 };

	// Export tab GL-account filter (verified AccountBindFromGlobal()'s inner
	// CmbDefaultFreightVoucherCreditAccountFillFromGlobal() local variable
	// accountsFromGlobalByTypeIds={3} - CmbChargesToAccountForFreightVoucherOutward). Type 3 is
	// "Receivables & Payables" (AP/AR) per the verified AccountTypes seed data.
	private static final int[] TYPE_RECEIVABLES_PAYABLES = { 3 };

	@GetMapping
	public String viewConfiguration(Model model) {
		model.addAttribute("currencies", configurationService.getCurrencies());

		// ---- Account tab GL-account combos (ditto Configuration.cs's AccountBindFromGlobal()) ----
		// cmbCCA, cmbBCA and CmbCashAccount all bind to the SAME type-{2} ("Cash Equivalent")
		// list in the real desktop source, even though cmbBCA is labeled "Bank Control Account" -
		// verified directly against AccountBindFromGlobal() (accountsFromGlobalByTypeIds2 and
		// accountsFromGlobalByTypeIds3 are both GetAccountsFromGlobalByTypeIds(new int[1]{2})).
		// Preserved exactly as found, not "corrected" to type 15 (Bank Equivalent).
		model.addAttribute("cashEquivalentAccounts",
				configurationService.getGlobalAccounts(TYPE_CASH_EQUIVALENT, null, null));
		// CmbDayBookTemporaryAc: not a type filter at all in the desktop - an exact
		// (case-insensitive) AccountTitle match, so this list will contain 0 or 1 rows.
		model.addAttribute("dayBookTemporaryAccounts",
				configurationService.getGlobalAccounts(null, null, TITLE_DAY_BOOK_TEMPORARY_ACCOUNT));
		model.addAttribute("capitalEquityAccounts",
				configurationService.getGlobalAccounts(TYPE_CAPITAL_EQUITY, null, null));
		model.addAttribute("otherAssetsLiabilitiesAccounts",
				configurationService.getGlobalAccounts(TYPE_OTHER_ASSETS_LIABILITIES, null, null));
		model.addAttribute("expenseAccounts",
				configurationService.getGlobalAccounts(TYPE_EXPENSE_ACCOUNTS, null, null));
		model.addAttribute("bankReconciliationAdjustmentAccounts",
				configurationService.getGlobalAccounts(null, TYPE_EXCLUDE_FOR_BANK_RECONCILIATION, null));

		// ---- Freight Voucher tab (ditto CmbFreightCustomAccountsGroup / CmbDefaultFreightVoucherCreditAccountId) ----
		model.addAttribute("freightCustomGroups", configurationService.getAcLookups(AC_LOOKUP_TYPE_CUSTOM_GROUP));
		// Default/unchecked-state list (type {2} only); countx_configuration.js re-fetches with
		// {2,8,15} via GET /api/configurations/global-accounts when
		// chkIncludeBankAccountsInFreightVoucherCreditAccount loads/changes to checked - ditto
		// CmbDefaultFreightVoucherCreditAccountFillFromGlobal()'s own checkbox-driven re-bind.
		model.addAttribute("freightCreditAccountsDefault",
				configurationService.getGlobalAccounts(TYPE_FREIGHT_CREDIT_DEFAULT, null, null));

		// ---- Inventory tab (ditto AccountBindFromGlobal()'s Inventory-tab combos + the 7
		// dedicated *FillFromGlobalAndBind()/Fill() methods for the non-account lookups) ----
		model.addAttribute("inventoryControlAccounts",
				configurationService.getGlobalAccounts(TYPE_INVENTORY_CONTROL, null, null));
		model.addAttribute("operatingCostOfSalesAccounts",
				configurationService.getGlobalAccounts(TYPE_OPERATING_COST_OF_SALES, null, null));
		model.addAttribute("stockAccountsExcludingCoreTypes",
				configurationService.getGlobalAccounts(null, TYPE_EXCLUDE_STOCK_ACCOUNTS, null));
		model.addAttribute("warehouses", configurationService.getWarehouses());
		model.addAttribute("packingMaterialWarehouses", configurationService.getPackingMaterialWarehouses());
		model.addAttribute("jobLots", configurationService.getJobLots());
		model.addAttribute("cropYears", configurationService.getCropYears());
		model.addAttribute("packingTypes", configurationService.getPackingTypes());
		model.addAttribute("cities", configurationService.getCities());
		model.addAttribute("customerGroups", configurationService.getCustomerGroups());

		// ---- Purchase tab (ditto AccountBindFromGlobal()'s 2 Purchase-tab GL-account combos;
		// no non-account lookups on this tab, no ControlEventHandler coupling on any of its
		// 59 visible controls across its 4 sub-tabs) ----
		model.addAttribute("costOfSalesAccounts",
				configurationService.getGlobalAccounts(TYPE_COST_OF_SALES, null, null));

		// ---- Sale tab (ditto AccountBindFromGlobal()'s 2 simple Sale-tab GL-account combos,
		// plus CmbFreightOutwardAc's ERP-feature-flag-gated TransportFill(); no non-account
		// lookups on this tab, no ControlEventHandler coupling on any of its 51 visible
		// controls; txtAsOnDateForDoCompulasoryOnGDN is a DateTimePicker, handled entirely
		// client-side - see countx_configuration.js and CONFIGURATION-PROGRESS.md) ----
		model.addAttribute("focInventoryExpenseAccounts",
				configurationService.getGlobalAccounts(TYPE_FOC_INVENTORY_EXPENSE, null, null));
		model.addAttribute("zakatInventoryExpenseAccounts",
				configurationService.getGlobalAccounts(TYPE_ZAKAT_INVENTORY_EXPENSE, null, null));
		model.addAttribute("freightOutwardAccounts", configurationService.getFreightOutwardAccounts());

		// ---- WeighBridge tab (ditto AccountBindFromGlobal()'s 2 WeighBridge-tab GL-account
		// combos; no non-account lookups on this tab, no ControlEventHandler coupling on any
		// of its 26 visible controls) ----
		model.addAttribute("revenueSalesAccounts",
				configurationService.getGlobalAccounts(TYPE_REVENUE_SALES, null, null));

		// ---- Production tab (ditto CastingType()/BindCustomGroupAccounts()/ItemCustomGroupFill()/
		// WarehousesDtFillFromGlobalAndBind() - no GL-account combos on this tab, 5 UltraCombos
		// total. 2 of the 5 are pure reuses of infra already built for other tabs (no new model
		// attributes needed): CmbDefaultCastingWarehouse reuses the existing "warehouses"
		// attribute above (verified bound inside the exact same WarehousesDtFillFromGlobalAndBind()
		// call as Inventory's cmbware), and CmbDefaultProductionOverHeadCustomGroupId reuses the
		// existing "freightCustomGroups" attribute above (verified Sp_AcLookUps_GetAllMethod's
		// 'ReadByAcLookUpId' branch returns the same dbo.AcLookUps rows as the 'ReadAll' branch
		// that attribute already uses, for AcLookUpTypesId=1). One is a genuinely new dependency:
		// CmbDefaultProductionInputItemCustomGroupId and CmbDefaultProductionScrapItemCustomGroupId
		// both bind to this SAME new "itemCustomGroups" list (verified: both call the desktop's
		// identical ItemCustomGroupFill() with no different filter). The 5th, CmbDefaultProduction-
		// StageId ("Casting Type"), has NO model attribute here - see the HTML comment on that
		// control and CONFIGURATION-PROGRESS.md's "Confirmed missing dependency" section: its
		// desktop data source (CastingType() -> Architecture.BLL.Mfg.LookUps.GetDataByTypeId(3) ->
		// [Mfg].[USP_LookUps_GetAllMethod]) is verified NOT PRESENT in GoldenAceDb(0509)t.sql
		// (only Mfg.USP_LookUps_Insert/Update exist) - reported per the Strict Preservation Rule
		// rather than fabricated, and implemented as a plain manual-entry text field instead of a
		// populated dropdown so any already-saved ConfigKey value is still preserved/editable. ----
		model.addAttribute("itemCustomGroups", configurationService.getItemCustomGroups());

		// ---- Export tab (ditto AccountBindFromGlobal()/CmbDefaultFreightVoucherCreditAccountFillFromGlobal()'s
		// Export-tab GL-account combos, plus 2 genuinely new non-account lookups). 3 of the 6
		// UltraCombos reuse infra already built for other tabs, verified by reading the desktop's
		// own local-variable definitions rather than guessing from caption similarity:
		// CmbExportReturnStockInTransit reuses "inventoryControlAccounts" above (verified: both it
		// and Inventory's cmbInventoryControlAccount are bound from the exact same
		// accountsFromGlobalByTypeIds11 = type {4} variable); CmbForeignExchangeGainLossAccount
		// reuses "bankReconciliationAdjustmentAccounts" above (verified: both it and Account's
		// cmbBankReconcilationAdjustmentAccountId are bound from the exact same
		// accountsFromGlobalByTypeIds15 = withoutTypeIds {2,4,11,12,15} variable); cmbForeignBaseCurrency
		// reuses "currencies" above (verified: cmbCurrency() binds both it and Account's
		// cmbBaseCurrency from the identical Architecture.BLL.MultiCurrency.GetAll() DataTable, no
		// separate query). CmbChargesToAccountForFreightVoucherOutward is a new type filter (type
		// {3}, "Receivables & Payables" per the verified AccountTypes seed row); getTransportationServiceItems()/
		// getCountries() are 2 new non-account lookups (see their own javadoc on
		// IConfigurationService). ----
		model.addAttribute("receivablesPayablesAccounts",
				configurationService.getGlobalAccounts(TYPE_RECEIVABLES_PAYABLES, null, null));
		model.addAttribute("transportationServiceItems", configurationService.getTransportationServiceItems());
		model.addAttribute("countries", configurationService.getCountries());

		// ---- Commission Agent tab (ditto Configuration.cs's GBComm GroupBox) ----
		// CmbDefaultTradingAccountIdForCommissionAgentPortal / CmbDefaultTaxAccountIdForCommissionAgentPortal
		// both reuse "stockAccountsExcludingCoreTypes" above (verified: AccountBindFromGlobal()'s
		// own un-suffixed "accountsFromGlobalByTypeIds" local - GetAccountsFromGlobalByTypeIds(null,
		// new[]{2,10,11,12,15}) - is byte-for-byte the same exclude-type-id array as
		// TYPE_EXCLUDE_STOCK_ACCOUNTS above). CmbDefaultCropYearIdForCommissionAgentPortal /
		// CmbDefaultPackingTypeIdForCommissionAgentPortal / CmbDefaultLoadingCityIdForCommissionAgentPortal /
		// CmbDefaultUnloadingCityIdForCommissionAgentPortal reuse "cropYears"/"packingTypes"/"cities"
		// above (verified: CropDtFillFromGlobalAndBind()/PackingTypeDtFillFromGlobalAndBind()/
		// CityBindFromGlobal() each bind their own Inventory-tab combo AND this tab's combo from the
		// exact same "source" DataTable in the same method body). CmbCustomGroupForWHTAccounts /
		// CmbCustomGroupForWHTAccountsSale reuse "freightCustomGroups" above (verified:
		// BindCustomGroupAccounts()'s own AcLookUpTypesId=1 call - the same "Account Group" list
		// CmbDefaultProductionOverHeadCustomGroupId and CmbFreightCustomAccountsGroup already use).
		// CmbDefaultWhtAccountPurchaseIdForCommissionAgentPortal / ...SaleIdForCommissionAgentPortal
		// are deliberately NOT rendered here - both start with an empty option list server-side and
		// are populated live by countx_configuration.js's applyWhtAccountsCoupling(), which
		// reproduces the desktop's own verified load-time quirk (Purchase side refreshed via
		// CmbCustomGroupForWHTAccounts_Leave(null,null) called twice in the constructor; Sale side
		// never refreshed on load at all - see IConfigurationService#getAccountsByCustomGroup's own
		// javadoc) - see GET /api/configurations/accounts-by-custom-group.
		model.addAttribute("commissionAgentSupplierCustomers", configurationService.getSupplierCustomers());
		model.addAttribute("paymentTerms", configurationService.getPaymentTerms());
		model.addAttribute("deliveryTerms", configurationService.getDeliveryTerms());
		model.addAttribute("taxTypes", configurationService.getTaxTypes());

		return "configurations/configuration";
	}
}
