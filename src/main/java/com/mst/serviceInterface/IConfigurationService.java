package com.mst.serviceInterface;

import java.util.List;
import java.util.Map;

import com.mst.models.ConfigrationsAllocation;
import com.mst.services.ConfigurationServiceImpl.ConfigSaveResult;

/**
 * Ditto contract of Architecture.BLL.Authentication.ConfigrationsAllocation as used by
 * the desktop Configuration.cs screen: HistoryConfiquration (screen load), GetByKey +
 * Save (every control's Leave/CheckedChanged/ValueChanged handler). OrganizationId,
 * CompanyId and the entry/modify UserId are never accepted as parameters here - the
 * implementation resolves them from CurrentUserContext exactly once per call, the same
 * way the desktop always reads UserAccount.OrganizationId/CompanyId/ID from the signed-in
 * session rather than from screen input.
 */
public interface IConfigurationService {

	/** Ditto ConfigrationsAllocation.HistoryConfiquration(UserAccount.OrganizationId, UserAccount.CompanyId). */
	List<ConfigrationsAllocation> getHistory();

	/** {@link #getHistory()} indexed by ConfigDescription (== every bound control's AccessibleName), for one-shot form population. */
	Map<String, ConfigrationsAllocation> getHistoryMap();

	/** Ditto ConfigrationsAllocation.GetByKey(configDescription, UserAccount.OrganizationId, UserAccount.CompanyId). */
	ConfigrationsAllocation getByKey(String configDescription);

	/**
	 * Ditto the desktop's generic per-control save path (Fire / FireDDL / FireRadioButton /
	 * FireTextBox / FireDateTime / FireDDLInfaragastic all funnel to BLL.Save the same way):
	 * GetByKey first; on no existing row, resolve ConfigrationsDefinitionId via
	 * Sp_ConfigrationsDefinition_ReadByConfigDescription (desktop: ActivityLog.ReadByConfigDescription)
	 * and fail with "Configuration Not Found" if that also comes back empty; otherwise Insert
	 * (Id==0) or Update (Id&gt;0) via the real stored procedures. ConfigValue is always persisted
	 * as "" and IsActive always true, matching every one of the desktop's Fire*() bodies.
	 *
	 * @param configDescription the control's AccessibleName / ConfigrationsDefinition.ConfigDescription
	 * @param configKey the value to store, already formatted exactly as the desktop control type
	 *                  would format it (CheckBox/RadioButton: "True"/"False" via bool.ToString();
	 *                  TextBox: trimmed text; ComboBox: the selected option's value)
	 */
	ConfigSaveResult saveControl(String configDescription, String configKey);

	/**
	 * Lookup source for the Account tab's "Base Currency" combo (AccessibleName "Base Currency").
	 * Verified real table dbo.MultiCurrency (GoldenAceDb line 37251: Id, CurrencyCode, CurrencyName,
	 * CurrencyRate, CurrencySymbol, ..., OrganizationId, CompanyId). The exact desktop binding call
	 * for this specific combo was not traced (CommonServices/Currency BLL was out of this pass's
	 * scope) - this is a verified-schema read, not a stored-procedure call, pending confirmation.
	 */
	List<java.util.Map<String, Object>> getCurrencies();

	/**
	 * Ditto DatatableHelper.GetAccountsFromGlobalByTypeIds(withTypeIds, withoutTypeIds, exactAccountTitle),
	 * the shared account-picker data source used by dozens of the desktop's account combos,
	 * Configuration.cs's own AccountBindFromGlobal() among them. Backed by the real stored
	 * procedure dbo.USP_GETAllAccountsFromCustomGroups (verified GoldenAceDb(0509)t.sql line
	 * 564207): @OrganizationId, @CompanyId (PageSize/PageNumber left unset/NULL, exactly like
	 * the desktop's own GetGlobalAllAccountsWithCustomGroup(orgId, compId, 0, 0, "") call, which
	 * the procedure's own "@PageSize IS NULL AND @PageNumber IS NULL" branch turns into "return
	 * (effectively) everything"). The procedure itself already restricts to
	 * AccountGroup='Detail' AND COAAllocation.IsActive=1 for this org/company - ditto, not
	 * reproduced again in Java.
	 * <p>
	 * withTypeIds/withoutTypeIds ditto the desktop's AccountTypeId filter (verified real lookup
	 * table dbo.AccountTypes, e.g. 2=Cash Equivalent, 15=Bank Equivalent - see
	 * CONFIGURATION-PROGRESS.md for the full table); exactAccountTitle ditto the desktop's
	 * case-insensitive AccountTitle.Equals(...) exact-match filter (used by exactly one combo,
	 * CmbDayBookTemporaryAc, matched against "ADJUSTMENT AND TEMPORARY ACCOUNT"). Pass null for
	 * whichever filters don't apply, exactly as the desktop leaves the corresponding parameter
	 * null on calls that don't need it. Rows are de-duplicated by ChartOfAccountId after
	 * filtering (ditto the desktop's HashSet&lt;int&gt; dedup - the procedure's own join to
	 * AccountsCustomGroups can produce more than one row per account).
	 *
	 * @return rows shaped {Id, AccountTitle, AccountCode, ParentAccountTitle, AccountClassName}
	 */
	List<Map<String, Object>> getGlobalAccounts(int[] withTypeIds, int[] withoutTypeIds, String exactAccountTitle);

	/**
	 * Ditto CommonServices.CustomeGroupsDefine(TypeId) -&gt; Architecture.BLL.Accounts.AcLookUps.GetAll,
	 * backed by the real stored procedure dbo.Sp_AcLookUps_GetAllMethod (verified GoldenAceDb
	 * line 282714) called with @Activity='ReadAll', @OrganizationId, @CompanyId, and
	 * @AcLookUpTypesId only when typeId is non-null/non-zero (ditto the desktop's own
	 * "if (obj.TypeId != 0)" guard). Used by Configuration.cs's FreightVoucher tab for
	 * CmbFreightCustomAccountsGroup with typeId=1 ("Custom Group").
	 *
	 * @return rows shaped {Id, AcLookUpsDescription, AcLookUpTypesId}
	 */
	List<Map<String, Object>> getAcLookups(Integer typeId);

	/**
	 * Ditto Configuration.cs's WarehousesDtFillFromGlobalAndBind(), which binds cmbware
	 * ("Default WareHouse") among others from clsGlobalVariables.globalWarehousesWithBranches,
	 * itself populated by GlobalServicesMethods.getGlobalActiveWarehouse -&gt; the real stored
	 * procedure dbo.USP_GetWarehousesAllocatedToBranch (verified BLL source), called with
	 * @OrganizationId, @CompanyId, @BranchId - ditto the desktop's own per-branch scoping
	 * (CurrentUserContext#currentBranchId(), the Java equivalent of UserAccount.BranchesId).
	 *
	 * @return rows shaped {Id, WarehouseName}
	 */
	List<Map<String, Object>> getWarehouses();

	/**
	 * Ditto Configuration.cs's GetWarehouseForPackingMaterial(), which binds
	 * CmbPackingWareHouse ("Default PM WareHouse") from
	 * CommonServices.GetActiveWareHouseByWareHouseType(3) -&gt;
	 * Architecture.BLL.Inventory.InvWareHouse.GetActiveWareHouseByWareHouseType -&gt; the real
	 * stored procedure dbo.Sp_InvWareHouse_GetAllMethod, called with @OrganizationId,
	 * @CompanyId, @WarehouseType=3 (the packing-material warehouse type - verified from the
	 * desktop's own literal "3" argument), @Activity="GetActiveWareHouseByWareHouseType".
	 *
	 * @return rows shaped {Id, WareHouseName}
	 */
	List<Map<String, Object>> getPackingMaterialWarehouses();

	/**
	 * Ditto Configuration.cs's JobLotDtFillFromGlobalAndBind(), which binds comJobLot
	 * ("Default Job/Lot") from clsGlobalVariables.globalJobLot, populated by
	 * CommonServices.GetJobLotGlIdsandName -&gt; the real stored procedure
	 * dbo.SP_JobLot_ReadMethod, called with @OrganizationId, @CompanyId,
	 * @Activity="GetJobLotGlIdsandName".
	 *
	 * @return rows shaped {Id, JobLotDescription}
	 */
	List<Map<String, Object>> getJobLots();

	/**
	 * Ditto Configuration.cs's CropDtFillFromGlobalAndBind(), which binds comCropYear
	 * ("Default Crop Year") from clsGlobalVariables.globalCropYear, populated by
	 * GlobalServicesMethods.getGlobalAllCropYear -&gt; the real stored procedure
	 * dbo.Sp_InvCropYear_GetAllMethod, called with @OrganizationId, @CompanyId,
	 * @Activity="ReadAll".
	 *
	 * @return rows shaped {Id, CropYear}
	 */
	List<Map<String, Object>> getCropYears();

	/**
	 * Ditto Configuration.cs's PackingTypeDtFillFromGlobalAndBind(), which binds comPakingType
	 * ("Default Paking Type") from clsGlobalVariables.globalInvPackingType, populated by
	 * GlobalServicesMethods.getGlobalAllPackingType -&gt; the real stored procedure
	 * dbo.Sp_InvPackingType_GetAllMethod, called with @Activity="ReadAll" only - verified the
	 * desktop's own call passes no @OrganizationId/@CompanyId for this proc (packing types are
	 * not org/company-scoped in the real schema).
	 *
	 * @return rows shaped {Id, PackTypeDesc}
	 */
	List<Map<String, Object>> getPackingTypes();

	/**
	 * Ditto Configuration.cs's CityBindFromGlobal(), which binds cmbcity ("Default City Area")
	 * from clsGlobalVariables.globalAllCities, populated by GlobalServicesMethods.getGlobalAllCity
	 * -&gt; the real stored procedure dbo.USP_City_GetAllWithCountryAndTehsil, called with
	 * @OrganizationId, @CompanyId.
	 *
	 * @return rows shaped {Id, CityName}
	 */
	List<Map<String, Object>> getCities();

	/**
	 * Ditto Configuration.cs's PartyGroupFill(), which binds CmbPartyGroup ("Default Party
	 * Group") from Architecture.BLL.Inventory.CustomerGroup.GetAll -&gt; the real stored
	 * procedure dbo.Sp_CustomerGroup_GetAllMethod, called with @Activity="ReadAll",
	 * @OrganizationId, @CompanyId. The desktop re-projects the raw result into an "Id"/
	 * "Description" pair before binding - the real proc's result set already carries columns
	 * literally named "Id" and "Description", ditto here.
	 *
	 * @return rows shaped {Id, Description}
	 */
	List<Map<String, Object>> getCustomerGroups();

	/**
	 * Ditto Configuration.cs's TransportFill(), which binds Sale tab's CmbFreightOutwardAc
	 * ("Freight Outward Ac"). Unlike every other account combo on this screen, its data
	 * source is chosen at runtime by an ERP feature flag, NOT a fixed AccountTypeId filter -
	 * verified chain:
	 * <pre>
	 *   SubsidiaryAccountAllownOnVouchers (private bool field, set from
	 *   CommonServices.GetERPFeatureById(4) -&gt; clsGlobalVariables.ErpFeaturesList, itself
	 *   populated once per session by GetCompanyFeaturesList() -&gt;
	 *   CompanyFeatures.GetERPFeaturesByCompanyId(CompanyId, OrganizationId) -&gt; the real
	 *   stored procedure dbo.USP_GetERPFeaturesByCompanyId (verified GoldenAceDb(0509)t.sql):
	 *     SELECT DISTINCT c.FeaturesId AS Id, f.ERPFeatures
	 *     FROM ERPConfigurations c INNER JOIN ERPFeatures f ON c.FeaturesId = f.Id
	 *     WHERE c.OrganizationId=@OrganizationId AND c.CompanyId=@CompanyId AND c.IsActive=1
	 *   FeatureId 4 is verified (ERPFeatures seed data) to literally be named
	 *   'SubsidiaryAccountAllownOnVouchers' - i.e. "is this org+company's ERPConfigurations
	 *   row for feature 4 active".
	 * </pre>
	 * Branch when the flag IS set (feature active for this org/company): the real stored
	 * procedure dbo.USP_GetVendorsAndCustomersForTransporter (verified GoldenAceDb line
	 * 601929), @OrganizationId, @CompanyId - result columns Id (a transporter/party id,
	 * NOT ChartOfAccountId), CompanyName.
	 * <p>
	 * Branch when the flag is NOT set (default/most common case): dbo.Sp_COAAllocation_GetAllMethod
	 * (verified GoldenAceDb line 287499) @Activity='COAAllocationSearch', @OrganizationId,
	 * @CompanyId, @UserId=0 (omitted - desktop's own CoaAllocationGetAllServiceBind() only adds
	 * @UserId when UserAccount.ID != 0, and this screen's context never needs the per-user
	 * restriction branch), then filtered client-side to AccountTypeId NOT IN (2, 15, 11)
	 * ("Cash Equivalent", "Bank Equivalent", "Operating Expenses" excluded) - ditto the
	 * desktop's own inline LINQ-free for-loop filter in TransportFill(). CRITICAL: this
	 * branch's "Id" column is <b>COAAllocation.Id</b>, not ChartofAccount.Id/ChartOfAccountId -
	 * a different id space than every other account combo on this screen, preserved exactly
	 * (not normalized) per the ditto rule.
	 * <p>
	 * Both branches are re-shaped into the same {Id, AccountTitle} pair before returning so
	 * the Thymeleaf/JS side can render either one identically, exactly as the desktop's own
	 * TransportFill() builds one common two-column DataTable("Id","AccountTitle") regardless
	 * of which branch supplied the rows.
	 *
	 * @return rows shaped {Id, AccountTitle}
	 */
	List<Map<String, Object>> getFreightOutwardAccounts();

	/**
	 * Ditto Configuration.cs's Production-tab ItemCustomGroupFill(), which binds BOTH
	 * CmbDefaultProductionInputItemCustomGroupId ("Default Production Input Item CustomGroup")
	 * and CmbDefaultProductionScrapItemCustomGroupId ("Default Production Scrap Item
	 * CustomGroup") from the exact same call -
	 * Architecture.BLL.Inventory.ItemCustomGroup.FormHistory(OrganizationId, CompanyId,
	 * BranchesId) - backed by the real stored procedure dbo.USP_ItemCustomGroup_GetAllMethod
	 * (verified GoldenAceDb(0509)t.sql), called with @Activity='FormHistory':
	 *   SELECT Id, GroupCode, GroupName, EntryDate, EntryUser, ModifyDate, ModifyUser,
	 *          OrganizationId, CompanyId, BranchesId
	 *   FROM dbo.ItemCustomGroup
	 *   WHERE OrganizationId=@OrganizationId AND CompanyId=@CompanyId AND BranchesId=@BranchesId
	 * Not to be confused with getAcLookups(1) ("Custom Group" AcLookUps, type 1), which backs
	 * CmbDefaultProductionOverHeadCustomGroupId on this same tab and the Freight Voucher tab's
	 * CmbFreightCustomAccountsGroup - a completely different table (dbo.AcLookUps, not
	 * dbo.ItemCustomGroup) despite the similar English name, preserved exactly as the desktop
	 * itself keeps them as two unrelated data sources.
	 *
	 * @return rows shaped {Id, GroupCode, GroupName}
	 */
	List<Map<String, Object>> getItemCustomGroups();

	/**
	 * Ditto Configuration.cs's Export-tab ServiceItemNameBind(), which binds
	 * CmbDefaultTranspotationServiceItemId ("Default Transpotation Service Item") from
	 * clsGlobalVariables.getGlobalAllSerivesItems.Where(r =&gt; r.ServicesMasterItemId == 5) - an
	 * in-memory session-cached list itself populated once by
	 * Architecture.BLL.Main.GlobalServicesMethods.Item_AllServiesItems(OrganizationId, CompanyId),
	 * backed by the real stored procedure [lgstcm].[USP_Item_AllServiesItems] (verified
	 * GoldenAceDb(0509)t.sql), called with @OrganizationId, @CompanyId only. This Java port reads
	 * the same procedure per-request instead of caching, then applies the identical
	 * ServicesMasterItemId==5 filter server-side (the literal "5" is the desktop's own hardcoded
	 * argument, preserved as-is - it corresponds to whichever dbo/lgstcm lookup row is seeded for
	 * that id, not independently renamed here).
	 *
	 * @return rows shaped {Id, ItemName}
	 */
	List<Map<String, Object>> getTransportationServiceItems();

	/**
	 * Ditto Configuration.cs's Export-tab OriginAndInspectionCountryBind(), which binds
	 * CmbDefaultExportInspectionCountryOfOrigin ("Default Export Inspection CountryOfOrigin")
	 * from Architecture.BLL.country.GetAll(new Country{OrganizationId, CompanyId}) - backed by the
	 * real stored procedure dbo.SP_Country_ReadMethod (verified GoldenAceDb(0509)t.sql), called
	 * with @OrganizationId, @CompanyId, @MethodType='GetAll'. CRITICAL, verified by reading the
	 * procedure's own body (not just the call site): the 'GetAll' branch has <b>no WHERE
	 * clause at all</b> - it returns every row in dbo.Country globally, completely ignoring the
	 * @OrganizationId/@CompanyId parameters it declares and is passed. This is ditto a
	 * pre-existing pattern already seen on Sp_CustomerGroup_GetAllMethod's 'ReadAll' branch -
	 * preserved exactly, not "fixed" to add an org/company filter that the real procedure does not
	 * apply. Desktop's own "Description" column is exposed here as "Name" to match the combo's own
	 * client-side DataTable column rename (DDL.BindDDL(..., "Id", "Name", ...)).
	 *
	 * @return rows shaped {Id, Name}
	 */
	List<Map<String, Object>> getCountries();

	// ---- Commission Agent tab (verified Configuration.cs's GBComm GroupBox: 17 UltraCombos +
	// the non-persisted RadNickName/RadBusinessName display toggle - see
	// CONFIGURATION-PROGRESS.md for the full per-control reuse/new-lookup breakdown) ----

	/**
	 * Ditto Configuration.cs's SupplierDtFillFromGlobal() + CommonBindings' CommissionAndBuyerBind(),
	 * which together feed THREE combos from the exact same list -
	 * CmbDefaultCommissionAgentIdForCommissionAgentPortal ("Commission Agent"),
	 * CmbDefaultSubCommissionAccountIdForCommissionAgentPortal ("Sub-Commission Account") and
	 * CmbDefaultSubBrokerageAccountIdForCommissionAgentPortal ("Sub-Brokery Account") - backed by
	 * clsGlobalVariables.globalAllSupplierCustomer, itself populated once per session by
	 * Architecture.BLL.Main.GlobalServicesMethods.getGlobalSupplierCustomer(OrganizationId,
	 * CompanyId), the real stored procedure dbo.USP_GetVendorsAndCustomersWithCityName
	 * (@OrganizationId, @CompanyId, no PartyTypeId/PageSize/PageNumber/Keyword - ditto
	 * DatatableHelper.PopulateGlobalVariables' own 2-arg call).
	 * <p>
	 * CRITICAL, verified: RadNickName and RadBusinessName (the "Nick Name"/"Business Name" radio
	 * pair on this same GroupBox) have <b>no AccessibleName set anywhere in InitializeComponent</b>
	 * - they are NOT persisted ConfigrationsAllocation rows at all (unlike every other radio on
	 * this whole 18-tab screen). ControlEventHandler's own RadioButton branch explicitly special-
	 * cases them (by .Name, not .AccessibleName) to call RadBusinessName_CheckedChanged() instead
	 * of FireRadioButton() - confirming no save ever happens for this pair. It is a pure
	 * client-side display toggle (WinForms designer default: RadBusinessName.Checked=true, i.e.
	 * "Business Name" shown by default) controlling which single text column
	 * SupplierDtFillFromGlobal() bakes into the DataTable it hands to all three combos: each
	 * row's CompanyName when RadBusinessName is checked, its NickName otherwise - and, for a
	 * sub-supplier/customer (IsSubSupCust=true with a resolvable ParentsSupCustId), that same
	 * mode's text is additionally prefixed with "{parent's own same-mode text} / " (verified: a
	 * Dictionary&lt;int,getGlobalAllSupplierCustomer&gt; keyed by Id, looked up once per row - not
	 * recursive, so a grandparent's text is never chased).
	 * <p>
	 * Since the real toggle needs no server round-trip, this Java port precomputes BOTH text
	 * variants (BusinessText/NickText, hierarchy prefix already applied to each) once per row here
	 * server-side, rather than exposing the toggle as a live endpoint - countx_configuration.js
	 * swaps the &lt;option&gt; text of all three combos between the two precomputed fields on
	 * RadNickName/RadBusinessName's own (unsaved) change event.
	 *
	 * @return rows shaped {Id, BusinessText, NickText}
	 */
	List<Map<String, Object>> getSupplierCustomers();

	/**
	 * Ditto Configuration.cs's PaymentTermsFill(), which binds
	 * CmbDefaultPaymentTermIdForCommissionAgentPortal ("Payment Term") from
	 * clsGlobalVariables.globalPaymentTerm, itself populated once per session by
	 * Architecture.BLL.Main.GlobalServicesMethods.getPaymentTermlist(OrganizationId, CompanyId) -&gt;
	 * Architecture.DAL.Inventory.InvDueTerms().GetAll("Sp_InvDueTerms_GetAllMethod", ...) with
	 * @OrganizationId, @CompanyId, @Activity='GetAll'. Bound directly against the raw
	 * "TermsDescription" column (ditto getCropYears()'s own raw "CropYear" column - GenericProvider's
	 * GetDataTableProc/GetProc bind purely off the SQL SELECT's own column names, verified by
	 * reading GenericProvider.GetDataTableProc&lt;T&gt;'s body: the generic T is unused for shaping,
	 * only for an unrelated internal cast - so the desktop's own PopulateDataTableAndReturn(...,
	 * x=&gt;x.Id, x=&gt;x.TermsDescription) remap it binds against afterward tells us the real
	 * property/column name, not the display column name of that synthetic table).
	 *
	 * @return rows shaped {Id, TermsDescription}
	 */
	List<Map<String, Object>> getPaymentTerms();

	/**
	 * Ditto Configuration.cs's DeliveryTermFill(), which binds
	 * CmbDefaultDeliveryTermIdForCommissionAgentPortal ("Delivery Term") from
	 * Architecture.BLL.Inventory.DeliveryTerm.FormHistory() - the real stored procedure
	 * [dbo].[USP_DeliveryTerm_GetAllMethod], called with @Activity='FormHistory' only (no
	 * @OrganizationId/@CompanyId parameter at all - verified, preserved exactly even though every
	 * sibling lookup on this tab takes both). Bound directly against the raw result's own "Id"/
	 * "Description" columns (verified: the desktop's own BindAndRetainSelection(...,"Id",
	 * "Description",...) call targets DeliveryTerm.FormHistory()'s return value directly, with no
	 * intermediate PopulateDataTableAndReturn remap in between, unlike Payment Term above - so
	 * unlike that one, "Description" here really is the real, raw SQL column name).
	 *
	 * @return rows shaped {Id, Description}
	 */
	List<Map<String, Object>> getDeliveryTerms();

	/**
	 * Ditto Configuration.cs's TaxTypeBind(DatatableHelper.TaxTypesDbCall(UserAccount)), which
	 * binds BOTH CmbDefaultTaxTypePurchaseeIdForCommissionAgentPortal ("Tax Type (Purchase)") and
	 * CmbDefaultTaxTypeSaleIdForCommissionAgentPortal ("Tax Type (Sale)") from the exact same call -
	 * Architecture.BLL.Inventory.TaxesTypes.GetForComboBind(new TaxesTypes{OrganizationId,
	 * CompanyId, Type=1}) - the real stored procedure Sp_TaxesTypes_GetAllMethod, called with
	 * @OrganizationId, @CompanyId, @Type=1, @Activity='ReadByCombo'. Bound directly against the raw
	 * "TaxName" column (ditto the desktop's own BindAndRetainSelection(..., "Id", "TaxName", ...)
	 * call - not remapped to "Description" like the other lookups on this tab, preserved exactly).
	 *
	 * @return rows shaped {Id, TaxName}
	 */
	List<Map<String, Object>> getTaxTypes();

	/**
	 * Ditto Configuration.cs's CmbCustomGroupForWHTAccounts_Leave()/CmbCustomGroupForWHTAccountsSale_Leave()
	 * (called when the user leaves CmbCustomGroupForWHTAccounts / CmbCustomGroupForWHTAccountsSale
	 * with a value &gt; 0): re-filters the exact same getGlobalAccounts() dataset
	 * (dbo.USP_GETAllAccountsFromCustomGroups) down to accounts belonging to ONE selected
	 * AcLookUps custom group id, entirely in-memory - ditto
	 * DatatableHelper.GetAccountsFromGlobalByTypeIds's withCustomGroupIds LINQ filter
	 * (a.CustomGroupId == customGroupId) - not a separate stored procedure.
	 * <p>
	 * CRITICAL, verified desktop load-time quirk (Configuration.cs's tabCommissionAgent
	 * constructor/Load path): CmbCustomGroupForWHTAccounts_Leave(null,null) is called <b>TWICE</b>
	 * on initial form load, and CmbCustomGroupForWHTAccountsSale_Leave() is <b>never called at
	 * all</b> there. Net effect: CmbDefaultWhtAccountPurchaseIdForCommissionAgentPortal's option
	 * list IS pre-filtered by whatever custom group is already saved for
	 * CmbCustomGroupForWHTAccounts on screen open; CmbDefaultWhtAccountSaleIdForCommissionAgentPortal's
	 * option list is always empty on open, no matter what is saved for
	 * CmbCustomGroupForWHTAccountsSale or for the Sale WHT account itself, until the user actually
	 * changes/leaves CmbCustomGroupForWHTAccountsSale in the browser session. Reproduced exactly
	 * (see ConfigurationViewController/countx_configuration.js) rather than "fixed" to also
	 * pre-populate the Sale side.
	 *
	 * @return rows shaped {Id, AccountTitle, AccountCode, ParentAccountTitle, AccountClassName}
	 */
	List<Map<String, Object>> getAccountsByCustomGroup(int customGroupId);
}
