# Configuration.cs -> Java Web Migration - Progress & Verification Log

Source of truth: `recovered_source/recovery/resolved-source/Architecture.WinApp.Configurations.Configuration.cs`
(18,033 lines, 18 tabs, 200+ settings). Pacing decision (user-approved, then explicitly
extended by the user to "continue with the remaining tabs" after the data layer was
corrected): build correct, reusable generic infra for all 18 tabs, fully implement and
verify the Account tab, correct its data sources once the real desktop binding calls were
traced, then continue tab-by-tab, per the user's explicit "continue with the remaining
tabs... do not mark a tab complete based only on source implementation - provide separate
source-level and runtime verification results" instruction (see the per-tab "Source-level vs.
runtime verification" call-outs below and the environment-limitation section, which applies
identically, and is stated separately, for every tab).

**All 18 of 18 tabs are now implemented and source-verified** (Account, Freight Voucher,
Inventory, Purchase, Sale, Quality Control, Wages, Weigh Bridge, Production, Export, Store
Management, Point Of Sale, MobileSMS, Party Processing, HRM, Commission Agent, System, and
Govt Wheat). What remains per the user's own instruction is the final full desktop-vs-Java
comparison report consolidating every tab's findings and unresolved issues - see "Source-level
vs. runtime verification status" below for the per-tab summary this final report will draw on.

## Strict Database Preservation Rule - compliance

No table, column, stored procedure, or existing data was created, renamed, or altered.
Real (verified) database objects, all confirmed against `GoldenAceDb(0509)t.sql`:

| Object | Type | Verified at |
|---|---|---|
| `dbo.ConfigrationsAllocation` | Table | line 19906 |
| `dbo.ConfigrationsDefinition` | Table | line 19929 |
| `Proc_ConfigrationsAllocation_History` | Stored proc (GET-all) | line 271142 |
| `Proc_ConfigrationsAllocation_ReadByKey` | Stored proc (GET-by-key) | line 271416 |
| `Proc_ConfigrationsAllocation_Insert` | Stored proc | line 271199 |
| `Proc_ConfigrationsAllocation_Update` | Stored proc | line 271459 |
| `Sp_ConfigrationsDefinition_ReadByConfigDescription` | Stored proc | line 289070 |
| `dbo.USP_GETAllAccountsFromCustomGroups` | Stored proc | line 564207 |
| `dbo.Sp_AcLookUps_GetAllMethod` | Stored proc | line 282714 |
| `dbo.AccountTypes` | Table (seed lookup, 22 rows) | seed `INSERT`s confirmed |
| `dbo.AcLookUps` | Table (generic named lookup) | referenced by `Sp_AcLookUps_GetAllMethod` |
| `dbo.MultiCurrency` | Table | line 37251 |
| `dbo.USP_GetWarehousesAllocatedToBranch` | Stored proc | line 602185 |
| `dbo.Sp_InvWareHouse_GetAllMethod` | Stored proc | line 433244 |
| `dbo.SP_JobLot_ReadMethod` | Stored proc | line 443078 |
| `dbo.Sp_InvCropYear_GetAllMethod` | Stored proc | line 359541 |
| `dbo.Sp_InvPackingType_GetAllMethod` | Stored proc | line 407606 |
| `dbo.USP_City_GetAllWithCountryAndTehsil` | Stored proc | line 524005 |
| `dbo.Sp_CustomerGroup_GetAllMethod` | Stored proc | line 292063 |
| `dbo.USP_GetERPFeaturesByCompanyId` | Stored proc | GoldenAceDb(0509)t.sql, verified this session |
| `dbo.USP_GetVendorsAndCustomersForTransporter` | Stored proc | line 601929 |
| `dbo.Sp_COAAllocation_GetAllMethod` | Stored proc | line 287499 |
| `dbo.ERPConfigurations` | Table (per-org/company feature toggle) | referenced by `USP_GetERPFeaturesByCompanyId` |
| `dbo.ERPFeatures` | Table (feature master/seed lookup) | referenced by `USP_GetERPFeaturesByCompanyId`; Id 4 = `SubsidiaryAccountAllownOnVouchers` |
| `dbo.COAAllocation` | Table | referenced by `Sp_COAAllocation_GetAllMethod` - `Id` is a distinct id space from `ChartOfAccountId` |
| `dbo.USP_ItemCustomGroup_GetAllMethod` | Stored proc | verified this session, full body read (`'ReadById'`/`'FormHistory'` branches) |
| `dbo.ItemCustomGroup` | Table | referenced by `USP_ItemCustomGroup_GetAllMethod` |
| `[Mfg].[USP_LookUps_GetAllMethod]` | **CONFIRMED NOT PRESENT** | exhaustively searched (regex `\[Mfg\]\.\[USP_LookUps_\w*\]`, 4 matches, all `Insert`/`Update`) and cross-checked against `DATABASE_CONTRACT.md` - only `[Mfg].[USP_LookUps_Insert]`/`[Mfg].[USP_LookUps_Update]` exist; the desktop's own `CmbDefaultProductionStageId` data source calls this non-existent read procedure - reported, not fabricated, see Production tab section below |
| `[lgstcm].[USP_Item_AllServiesItems]` | Stored proc | verified GoldenAceDb(0509)t.sql, Export tab |
| `dbo.SP_Country_ReadMethod` | Stored proc | verified GoldenAceDb(0509)t.sql, Export tab - its `'GetAll'` branch has no `WHERE` clause (org/company params ignored), preserved as-is |
| `dbo.USP_GetVendorsAndCustomersWithCityName` | Stored proc | Commission Agent tab - backs `getSupplierCustomers()` |
| `dbo.Sp_InvDueTerms_GetAllMethod` | Stored proc | Commission Agent tab - backs `getPaymentTerms()` |
| `[dbo].[USP_DeliveryTerm_GetAllMethod]` | Stored proc | Commission Agent tab - backs `getDeliveryTerms()`; verified to take only `@Activity`, no `@OrganizationId`/`@CompanyId` |
| `dbo.Sp_TaxesTypes_GetAllMethod` | Stored proc | Commission Agent tab - backs `getTaxTypes()` (`@Type=1`, `@Activity='ReadByCombo'`) |
| `Architecture.BLL.Inventory.GCT.GctQuotaPolicy` (class) / `GetMillCategory()` (method) | **CONFIRMED NOT PRESENT** | exhaustively searched (`grep -rl "GctQuotaPolicy"` across the entire recovered/decompiled source tree, all recovered BLL/DAL/Model project folders included) - no class definition exists anywhere, only two WinForms call sites; further cross-checked for "MillCategory" against both `GoldenAceDb(0509)t.sql` (UTF-16LE-decoded) and `DATABASE_CONTRACT.md` - zero matches in either, no table/column/procedure by that name exists; the desktop's own `CmbMillCategory` data source calls this unrecoverable method - reported, not fabricated, see Govt Wheat tab section below |

Note the user's assumed names (`tbl_ConfigrationsAllocation`, `tbl_ConfigrationsDefinition`,
`Proc_ConfigrationsDefinition_ReadByDesc`) do **not** exist in the real database - per the
user's own override instruction, the real names above were used instead. The pre-existing
Java code in this repo (before this task) referenced the wrong table name and non-existent
columns; that defect is fixed by this change, not introduced by it.

`Sp_ConfigrationsAllocation_GetAllMethod` (line 289021) is a real proc but is **not** used by
Configuration.cs's own load/save flow - it backs a different, unrelated BLL method
(`GetConfigurationByOrgCompandConfigDescription`) used by other screens. Not wired here
(out of scope for this screen).

`spring.jpa.hibernate.ddl-auto=none` is untouched. No Hibernate/JPA entity backs
`ConfigrationsAllocation` (plain POJO) - all reads/writes go through `JdbcTemplate` calling
the exact stored procedures above with `EXEC ... @Param=?` and positional JDBC parameters,
matching the project's established convention. The same is now true of every GL-account/
lookup read added this session (`USP_GETAllAccountsFromCustomGroups`,
`Sp_AcLookUps_GetAllMethod`) - JDBC-only, no JPA, no schema generation, no new tables.

### `dbo.AccountTypes` - verified full seed data (used to give meaning to every
AccountTypeId filter below; magic numbers are never used without this table backing them)

| Id | Name (representative) |
|---|---|
| 2 | Cash Equivalent |
| 4 | Inventory |
| 6 | Other Assets |
| 8 | Other Liabilities |
| 9 | Capital & Equity |
| 11 | Operating Expenses |
| 12 | Cost Of Sales |
| 13 | Financial Expenses |
| 10 | Revenue & Sales |
| 15 | Bank Equivalent |
| 20 | Other Expenses |
| 21 | Selling Expenses |

(Full 22-row table extracted from the SQL dump's `INSERT` statements; only the ids actually
used by a filter in this screen are listed here. See extraction script output for the
complete set if a future tab needs an id not shown above.)

## Generic infrastructure (reusable by all 18 tabs) - unchanged this session

Desktop mechanism (verified): every bound control's `AccessibleName` equals its
`ConfigDescription` database key. Load populates every control by walking the tab tree via
`FindControlByAccessibleName`; save happens immediately on the control's own
change/leave event through one of six generic handlers (`Fire`, `FireDDL`,
`FireRadioButton`, `FireDDLInfaragastic`, `FireDateTime`, `FireTextBox`), all doing:
GetByKey(AccessibleName, OrgId, CompanyId) -> UPDATE if found, else resolve
`ConfigrationsDefinitionId` via `ReadByConfigDescription` and INSERT. `ConfigValue` is
always saved as `""` and `IsActive` always `true` - the actual value lives in `ConfigKey`.

This is ported 1:1, not tab-specific, and required no changes this session:

- **Backend** (`com.mst.services.ConfigurationServiceImpl` / `IConfigurationService` /
  `ConfigurationApiController`): `getHistory()`/`getHistoryMap()` (GET-all, ditto
  `HistoryConfiquration`), `getByKey()` (ditto `GetByKey`), `saveControl()` (ditto every
  `Fire*()` body).
- **Frontend** (`countx_configuration.js`): any element anywhere in the page carrying
  `data-config-description` (= verbatim `AccessibleName`) and `data-config-type`
  (`checkbox` | `radio` | `textbox` | `select`) is automatically loaded from
  `GET /api/configurations/map` and automatically wired to
  `POST /api/configurations/save-control` on the matching event. Adding a new tab's
  controls requires no new generic JS, only markup with the right `data-config-*`
  attributes - a tab-specific coupling (like Freight Voucher's, below) is the only thing
  that needs new JS.

## Account tab (`tabAccount`) - fully implemented, GL-account sources now verified (not placeholder)

Verified against `Configuration.cs` lines 6152-7746, 53 controls implemented (of 57
declared - 4 are `Visible=false` in the desktop and correctly excluded, see prior notes
preserved below).

### Correction made this session: GL-account combo data source

Previously flagged as an open gap and implemented against a placeholder
(`IChartofAccountService.getDetailAccounts()`). This session traced the desktop's actual
binding call and replaced the placeholder with the real one:

- **Desktop call chain (verified):** `Configuration.cs.AccountBindFromGlobal()` (lines
  2345-2417) calls `DatatableHelper.GetAccountsFromGlobalByTypeIds(withTypeIds,
  withoutTypeIds, exactAccountTitle)` (`Architecture.WinApp.Helper/DatatableHelper.cs`
  lines 109-172) for every one of the 8 combos, each with its own filter arguments. That
  helper itself is a LINQ filter (include types -> exclude types -> exact title match ->
  dedup by `ChartOfAccountId`) over `GlobalServicesMethods.GetGlobalAllAccountsWithCustomGroup`
  (`Architecture.BLL.Main.GlobalServicesMethods.cs` lines 609-675), which calls
  `GenericProvider.GetDataTableProc<...>("[dbo].[USP_GETAllAccountsFromCustomGroups]", ...)`
  - a real, verified stored procedure (line 564207), called with `@OrganizationId`,
  `@CompanyId` only (`PageSize`/`PageNumber` left `NULL`, exactly like the desktop's own
  `GetGlobalAllAccountsWithCustomGroup(orgId, compId, 0, 0, "")` call).
- **Per-combo filter, each traced individually from `AccountBindFromGlobal()`:**

  | Combo (`AccessibleName`) | Filter | Notes |
  |---|---|---|
  | `cmbCCA` (Cash Control Account) | `withTypeIds={2}` | Cash Equivalent |
  | `cmbBCA` (Bank Control Account) | `withTypeIds={2}` | **Same as cmbCCA** - verified, not a typo in this port. The desktop itself filters "Bank Control Account" by AccountTypeId 2 (Cash Equivalent), not 15 (Bank Equivalent). Preserved exactly, flagged in code comment + HTML `title` attribute rather than "corrected". |
  | `CmbCashAccount` | `withTypeIds={2}` | Cash Equivalent |
  | `CmbDayBookTemporaryAc` | `exactAccountTitle="ADJUSTMENT AND TEMPORARY ACCOUNT"` | Not a type filter at all - exact case-insensitive title match; list will contain 0 or 1 rows |
  | `CmbUnAppropriatedProfitAndLoss` | `withTypeIds={9}` | Capital & Equity |
  | `CmbDefaultRBAccount` | `withTypeIds={6,8}` | Other Assets, Other Liabilities |
  | `CmbDefaultDiscountAccount` | `withTypeIds={11,13,20,21}` | Operating/Financial/Other/Selling Expenses |
  | `cmbBankReconcilationAdjustmentAccountId` | `withoutTypeIds={2,4,11,12,15}` | Excludes Cash Equivalent, Inventory, Operating Expenses, Cost Of Sales, Bank Equivalent - an exclude-filter, not an include-filter |

- **Java port:** `IConfigurationService#getGlobalAccounts(int[] withTypeIds, int[]
  withoutTypeIds, String exactAccountTitle)` / `ConfigurationServiceImpl` - calls
  `EXEC USP_GETAllAccountsFromCustomGroups @OrganizationId=?, @CompanyId=?` via
  `JdbcTemplate`, then reproduces the exact same in-memory filter chain (include -> exclude
  -> exact title -> dedup by `ChartOfAccountId`) the desktop's LINQ does, rather than
  pushing the filter into SQL (the real procedure doesn't accept type-filter parameters -
  filtering happens client-side on the desktop too). `ConfigurationViewController`
  server-renders each combo's specific filtered list as its own model attribute
  (`cashEquivalentAccounts`, `dayBookTemporaryAccounts`, `capitalEquityAccounts`,
  `otherAssetsLiabilitiesAccounts`, `expenseAccounts`, `bankReconciliationAdjustmentAccounts`).
- New endpoint `GET /api/configurations/global-accounts?with=&without=&title=` exposes the
  same method for client-side re-filtering (used by the Freight Voucher tab's dynamic
  coupling below).

### `cmbBaseCurrency` - unchanged, still a documented gap

Bound to the existing `getCurrencies()` / `dbo.MultiCurrency` query (already used elsewhere
in this codebase) - the desktop's own currency-combo binding call was not traced this
session either (out of scope; `MultiCurrency` is a verified real table, so this remains a
verified-schema read, not a guessed one, just not proven to be the *exact* same call the
desktop makes).

### Radio groups, `groupBox1`/`5`/`10`, `GroupBoxAutoRemarks`, hidden controls - unchanged

All couplings, load-time radio tie-break normalization, and the 4 `Visible=false` excluded
controls (`CmbMonthlyBudget`, `CmbYearlyBudget`, `ChkApplyExp`, `ChkBoxStockConversionFinancial`)
are unchanged from the prior verified pass - see `final_controls.json` for the authoritative
per-control list and the git history of this file for the full original write-up.

## Freight Voucher tab (`tabFreightVoucher`) - fully implemented, source-verified (2nd of 18)

Verified against `Configuration.cs` lines 7731-7936 (`this.tabFreightVoucher.Controls.Add(...)`
through `tabInventory`'s own `Location` assignment at ~7937-8010, confirming the tab
boundary). **5 controls implemented, all 5 declared for this tab** (no hidden controls on
this tab).

| Control (`AccessibleName`) | Type | Caption (verified label) |
|---|---|---|
| `TolerancePercentForDiscountonFreightVoucher` | TextBox (`KeyPressDecimalOnly`) | Tolerance Percent For Discount On Freight Voucher |
| `DefaultFreightVoucherCreditAccountId` | UltraCombo | Default Freight Voucher Credit Account |
| `FreightVoucherNotCompulsoryBeforeGrn` | CheckBox | Freight Voucher Not Compulsory Before Grn |
| `IncludeBankAccountsInFreightVoucherCreditAccount` | CheckBox | Include Bank Accounts In Freight-Voucher's CreditAccount |
| `FreightCustomAccountsGroup` | UltraCombo | Freight Custom Accounts Group |

**Verified coupling (`ControlEventHandler`, CheckBox branch, line ~3140-3149):** toggling
`chkIncludeBankAccountsInFreightVoucherCreditAccount` calls
`CmbDefaultFreightVoucherCreditAccountFillFromGlobal()` (lines 2403-2417), which re-binds
`CmbDefaultFreightVoucherCreditAccountId` between `withTypeIds={2}` (unchecked) and
`withTypeIds={2,8,15}` (checked) - Cash Equivalent only, vs. Cash Equivalent + Other
Liabilities + Bank Equivalent - using the same `BindAndRetainSelection` pattern as every
other combo in `AccountBindFromGlobal()` (retain the current selection if it's still valid
in the new list, otherwise clear it).

**Java/JS port:**
- Server-rendered default (unchecked-state) list: `freightCreditAccountsDefault` model
  attribute (`getGlobalAccounts({2}, null, null)`).
- `countx_configuration.js`'s new `applyFreightVoucherCoupling(isInitialLoad)`: runs once on
  page load (using the checkbox's just-loaded saved state) and again on every change of
  `chkIncludeBankAccountsInFreightVoucherCreditAccount`. Unchecked: re-applies the
  already-server-rendered list's saved selection (ditto `BindAndRetainSelection`, no
  network call needed since the narrower list is already in the DOM). Checked: calls
  `GET /api/configurations/global-accounts?with=2,8,15`, rebuilds the `<option>` list, and
  restores the saved/current selection if it's present in the new list - ditto
  `BindAndRetainSelection` exactly, including the "clear if no longer valid" behavior (an
  unmatched `$combo.val(...)` naturally leaves the select on its placeholder).
- `CmbFreightCustomAccountsGroup`: bound to `freightCustomGroups` model attribute
  (`getAcLookups(1)` - AcLookUpTypesId 1 = "Custom Group", ditto
  `CommonServices.CustomeGroupsDefine(TypeId)` -> `AcLookUps.GetAll` ->
  `Sp_AcLookUps_GetAllMethod` with `@Activity='ReadAll'`, `@AcLookUpTypesId=1`).

## Inventory tab (`tabInventory`) - fully implemented, source-verified (3rd of 18)

Verified against `Configuration.cs` lines 7937-9311 (`this.tabInventory.Controls.Add(...)`
through `tabPurchase`'s own `Controls.Add(this.panel4)` at line 9312, confirming the tab
boundary). **34 controls declared, 1 excluded, 33 implemented:**
`chkStockTransferFinancialEffectIsActive` (and its label `label115`) is `Visible = false` in
the desktop - excluded ditto the Account tab's own 4 hidden controls, not rendered.

No `ControlEventHandler` coupling references any control on this tab (checked all 33
control names against the dispatcher) - no tab-specific JS was needed, only the already-
generic `data-config-description`/`data-config-type` markup contract.

### GL-account combos (3 of the 9 UltraCombos on this tab) - same `AccountBindFromGlobal()` method, new filters

| Combo (`AccessibleName`) | Filter | Notes |
|---|---|---|
| `cmbInventoryControlAccount` | `withTypeIds={4}` | Inventory |
| `CmbFluctuationDifferenceAccount` | `withTypeIds={11,12}` | Operating Expenses, Cost Of Sales |
| `CmbExpensesLabourAc` | `withTypeIds={11,12}` | Same set as above - verified independently, not assumed identical |
| `CmbAgainstStockAccount` | `withoutTypeIds={2,10,11,12,15}` | Excludes Cash Equivalent, Revenue & Sales, Operating Expenses, Cost Of Sales, Bank Equivalent |
| `CmbDifferenceBalanceAc` | `withoutTypeIds={2,10,11,12,15}` | Same exclude set as `CmbAgainstStockAccount` |

These reuse the existing `getGlobalAccounts()` method/endpoint added for the Account tab -
no new backend method needed for the account-picker combos themselves.

### Non-account lookup combos (6 of the 9 UltraCombos) - new data sources traced and verified this session

Every one of these was an *undocumented* dependency before this session (the Account tab's
only prior gap was the GL-account combos) - each is a separate real stored procedure, traced
from the tab's own dedicated `*FillFromGlobalAndBind()`/`*Fill()` method down to the
underlying `GenericProvider.GetDataTableProc` call, then independently re-verified against
`GoldenAceDb(0509)t.sql`'s own `CREATE PROCEDURE` text (not just the C# call site):

| Combo (`AccessibleName`) | Desktop method | Real procedure | Params | Result columns used |
|---|---|---|---|---|
| `cmbware` ("Default WareHouse") | `WarehousesDtFillFromGlobalAndBind()` | `USP_GetWarehousesAllocatedToBranch` | `@OrganizationId, @CompanyId, @BranchId` | `Id, WareHouseName` |
| `CmbVirtualWareHouseId` | same | same | same | same |
| `CmbWarehouseStore` | same | same | same | same |
| `CmbPackingWareHouse` ("Default PM WareHouse") | `GetWarehouseForPackingMaterial()` | `Sp_InvWareHouse_GetAllMethod` | `@OrganizationId, @CompanyId, @WarehouseType=3, @Activity='GetActiveWareHouseByWareHouseType'` | `Id, WareHouseName` |
| `comJobLot` ("Default Job/Lot") | `JobLotDtFillFromGlobalAndBind()` | `SP_JobLot_ReadMethod` | `@OrganizationId, @CompanyId, @Activity='GetJobLotGlIdsandName'` | `Id, JobLotDescription` |
| `comCropYear` ("Default Crop Year") | `CropDtFillFromGlobalAndBind()` | `Sp_InvCropYear_GetAllMethod` | `@OrganizationId, @CompanyId, @Activity='ReadAll'` | `Id, CropYear` |
| `comPakingType` ("Default Paking Type") | `PackingTypeDtFillFromGlobalAndBind()` | `Sp_InvPackingType_GetAllMethod` | `@Activity='ReadAll'` only - verified this procedure has **no** `@OrganizationId`/`@CompanyId` parameter at all | `Id, PackTypeDesc` |
| `cmbcity` ("Default City Area") | `CityBindFromGlobal()` | `USP_City_GetAllWithCountryAndTehsil` | `@OrganizationId, @CompanyId` | `Id, CityName` |
| `CmbPartyGroup` ("Default Party Group") | `PartyGroupFill()` -> `Architecture.BLL.Inventory.CustomerGroup.GetAll` | `Sp_CustomerGroup_GetAllMethod` | `@Activity='ReadAll', @OrganizationId, @CompanyId` | `Id, Description` |

`cmbware`/`CmbVirtualWareHouseId`/`CmbWarehouseStore` are branch-scoped in the real desktop
(`GlobalServicesMethods.getGlobalActiveWarehouse(orgId, compId, BranchesId)`) - ported via
`CurrentUserContext#currentBranchId()`, the existing Java equivalent of
`UserAccount.BranchesId` already used elsewhere in this codebase (not a new concept
introduced for this tab).

### Remaining controls (24 CheckBoxes + 3 TextBoxes)

All are plain generic `data-config-type="checkbox"`/`"textbox"` controls with no coupling
and no external data source - captions resolved the same scripted label-matching way as the
Account tab (all 34 labels matched their control with zero left unmatched, a strong
correctness signal for this tab's extraction).

## Purchase tab (`tabPurchase`) - fully implemented, source-verified (4th of 18)

Verified against `Configuration.cs` lines 9312-10505 (`this.tabPurchase.Controls.Add(this.panel4)`
through `tabSale`'s own `Controls.Add(...)` start). **First tab whose only direct child is a
second, nested WinForms `TabControl`** (`panel4` -> `tabControl2`, 4 sub-tabs: `tabPage1`
"Purchase Order" TabIndex 0, `tabPage2` "GatePass Inward" TabIndex 1, `tabPage3` "Purchase
Invoice" TabIndex 2, `tabPage4` "Goods Receiving Notes" TabIndex 3 - declared in source order
1,2,4,3 but rendered here in TabIndex/visual order). 65 controls declared across the 4
sub-tabs combined (20 + 11 + 26 + 8), 6 excluded (`Visible=false` - `ChkWbRequiredForGpInward`,
`ChkBoxPoCompusloryforGrn`, `ChkBoxWeighBridgeManualWeight`, `ChkBoxPoInGP`, `ChkBoxFreight`,
`ChkGPWeightCheckInGRN`, all on the Purchase Order sub-tab, plus their labels), **59
implemented** (14 + 11 + 26 + 8). No `ControlEventHandler` coupling references any of the 59.

**New reusable nested-subtab UI pattern** (needed again by HRM, task #15 - see backlog): CSS
classes `.win-sub-tabs`/`.win-sub-tab-content`/`.cfg-subtab-pane`/`.cfg-subtab-pane.active`
(a visually smaller variant of the existing `.win-nav-tabs`/`.cfg-tab-pane` top-level pattern)
plus `window.switchConfigSubTab(groupId, subTabId)` in `countx_configuration.js`, which scopes
its pane-toggling to `$group.closest(".cfg-tab-pane")` so each top-level tab's own sub-tab
group is independent of any other tab's.

**Dense-cluster label mismatch caught and hand-corrected:** the Purchase Order sub-tab's
`GbBranchWiseConfigPurchase` GroupBox has label/control y-coordinates only 3-8px apart -
automated proximity-based label matching (the heuristic used for every prior tab) mismatched
6 of these 10 pairs on the first pass. Every pairing in this fieldset was re-verified by hand
directly against the raw `Location` values (not trusted from the automated matcher) - see the
HTML comment on that fieldset. This finding changed practice going forward (see Sale tab
below): any dense cluster is now re-verified against raw source, not the matcher's output.

**2 GL-account combos**, same `AccountBindFromGlobal()` method as every prior tab:

| Combo (`AccessibleName`) | Filter | Notes |
|---|---|---|
| `CmbFreightInwardAc` | `withoutTypeIds={2,10,11,12,15}` | Same exclude set as Inventory's `CmbAgainstStockAccount`/`CmbDifferenceBalanceAc` - verified independently, not assumed |
| `CmbAdjustmentAcForRateCutAmountInCaseOfAccessWeightReceived` | `withTypeIds={12}` | Cost Of Sales |

Both reuse the existing `getGlobalAccounts()` method/endpoint - no new backend method needed.

## Sale tab (`tabSale`) - fully implemented, source-verified (5th of 18)

Verified against `Configuration.cs` lines 10611-11570 (`this.tabSale.Controls.Add(...)`
through `tabQA`'s own `Controls.Add(...)` start). Single flat tab (no nested `TabControl`),
but a dense 3-column layout - 55 controls declared, 4 excluded (`Visible=false` -
`ChkBoxSoCompulsoryinGPO`, `ChkBoxWeightChkGDN`, `ChkBoxSoCompulsoryforGdn`,
`ChkBoxSaleOrderLoadCustomerWise`, plus their labels `label35`/`label32`/`label47`/`label54`),
**51 implemented**. No `ControlEventHandler` coupling references any of the 51.

**Every one of the 51 controls' label pairing was re-verified by hand** directly against raw
`Location` values (continuing the practice change from Purchase), including one apparent
`AccessibleName`/caption mismatch that turned out to be a genuine, verified desktop quirk
rather than an extraction error: `ChkBoxSaleOrderQty`'s `AccessibleName` (the database key) is
`"Open Sale Order Quantity"`, but its own declared neighbor `label33` - at the identical
`(8, 32)` designer position, declared immediately after it in the source - reads `"Validate
Sale Order Weight On Delivery Order"`. Confirmed by direct inspection of the raw designer
code (not just proximity) that this is the real, intentional visual caption for that
checkbox. Preserved verbatim per the ditto rule - the visible label does **not** match the
database key's own name, and this is not "fixed."

### `CmbFreightOutwardAc` - a third GL-account data-source pattern, not a fixed type filter

Every GL-account combo implemented so far (Account, Freight Voucher, Inventory, Purchase
tabs) uses a fixed `AccountTypeId` include/exclude filter through the same
`AccountBindFromGlobal()` -> `USP_GETAllAccountsFromCustomGroups` pipeline.
`CmbFreightOutwardAc` (Sale tab's `TransportFill()` method, `Configuration.cs` lines
~2754-2810) does not - it is gated at runtime by an **ERP feature flag**, a completely
different subsystem from the `ConfigrationsAllocation` table this whole screen otherwise
manages:

- **Flag chain (fully traced and verified this session):** the private field
  `SubsidiaryAccountAllownOnVouchers` is set from `CommonServices.GetERPFeatureById(4)`,
  which checks the in-memory `clsGlobalVariables.ErpFeaturesList` cache, populated once per
  session by `DashboardNew.cs`'s `GetCompanyFeaturesList()` ->
  `CompanyFeatures.GetERPFeaturesByCompanyId(CompanyId, OrganizationId)`
  (`0054_Architecture.BLL.CompanyFeatures.cs` lines 69-96) -> the real stored procedure
  `dbo.USP_GetERPFeaturesByCompanyId` (verified `GoldenAceDb(0509)t.sql`):
  ```sql
  CREATE PROC [dbo].[USP_GetERPFeaturesByCompanyId] @OrganizationId int, @CompanyId int AS
  BEGIN
    SELECT DISTINCT C.FeaturesId AS Id, f.ERPFeatures
    FROM ERPConfigurations c INNER JOIN ERPFeatures f ON c.FeaturesId = f.Id
    WHERE C.OrganizationId = @OrganizationId AND C.CompanyId = @CompanyId AND C.IsActive = 1
  END
  ```
  FeatureId 4 is verified (`ERPFeatures` seed data: `INSERT ... VALUES (4,
  N'SubsidiaryAccountAllownOnVouchers')`) to literally be this same-named feature - i.e. the
  question being asked is "does this org+company have an active `ERPConfigurations` row for
  feature 4". **Not to be confused** with the same-named checkbox
  `chkSubsidiaryAccountAllownOnVouchers` on the Account tab, which is an ordinary
  generically-saved `ConfigrationsAllocation` control, unrelated to this runtime flag/table.
- **Branch when the flag IS active for this org/company:** real stored procedure
  `dbo.USP_GetVendorsAndCustomersForTransporter` (verified `GoldenAceDb(0509)t.sql` line
  601929), `@OrganizationId, @CompanyId` - result columns `Id` (a transporter/party id,
  **not** `ChartOfAccountId`), `CompanyName`.
- **Branch when the flag is NOT active (the default/most common case):** real stored
  procedure `dbo.Sp_COAAllocation_GetAllMethod` (verified `GoldenAceDb(0509)t.sql` line
  287499), `@Activity='COAAllocationSearch'`, `@OrganizationId, @CompanyId` (`@UserId`
  omitted, ditto the desktop's own `CoaAllocationGetAllServiceBind()` which only adds it when
  `UserAccount.ID != 0`), filtered client-side to `AccountTypeId NOT IN (2, 15, 11)` -
  ditto `TransportFill()`'s own inline for-loop filter, not reproduced as a SQL `WHERE`
  clause since the desktop doesn't push it into SQL either. **Critical, preserved exactly:**
  this branch's `Id` column is `COAAllocation.Id`, **not** `ChartofAccount.Id`/
  `ChartOfAccountId` (a separate column in the same result set) - a different id space than
  every other account combo on this entire screen. Not normalized to `ChartOfAccountId`.
- **Java port:** `IConfigurationService#getFreightOutwardAccounts()` /
  `ConfigurationServiceImpl` - a per-request read of `USP_GetERPFeaturesByCompanyId` (rather
  than porting the desktop's in-memory session cache, which would introduce staleness risk
  with no real benefit in a stateless web request model) decides the branch, then re-shapes
  either branch's rows into the same `{Id, AccountTitle}` pair before returning, ditto the
  desktop's own `TransportFill()` building one common two-column `DataTable` regardless of
  which branch supplied the rows. Rendered via a new `freightOutwardAccounts` model
  attribute, display-text-only (`AccountTitle`, no `AccountCode` prefix) - ditto the
  desktop's own `DDL.BindDDL(dataTable, CmbFreightOutwardAc, "Id", "AccountTitle", ...)` call,
  which (unlike every `AccountBindFromGlobal()` combo) does not concatenate `AccountCode`.

**2 other GL-account combos** on this tab ARE simple `AccountBindFromGlobal()`-style filters,
new type sets not seen on any prior tab:

| Combo (`AccessibleName`) | Filter | Notes |
|---|---|---|
| `CmbFOCInventoryExpenseAccount` | `withTypeIds={11,20,21}` | Operating Expenses, Other Expenses, Selling Expenses |
| `CmbZakatInventoryExpensesAccount` | `withTypeIds={8,11,20,21}` | Other Liabilities, Operating Expenses, Other Expenses, Selling Expenses |

Both reuse the existing `getGlobalAccounts()` method/endpoint.

### `txtAsOnDateForDoCompulasoryOnGDN` - first `DateTimePicker` on this screen, and a verified desktop load-bug preserved as-is

The only `DateTimePicker` control anywhere in the 18-tab screen. Two things were traced and
verified before implementing it:

1. **Save side (`FireDateTime()`, `Configuration.cs` lines 3057-3093):** ditto every other
   `Fire*()` handler (`GetByKey` -> UPDATE, or resolve `ConfigrationsDefinitionId` -> INSERT;
   `ConfigValue=""`, `IsActive=true`), with `ConfigKey =
   Conversion.ToDateTime(DateTimePicker.Value).ToString()` - .NET's own default (culture-
   dependent, en-US-equivalent in this codebase) `DateTime.ToString()`. `ConfigKey` is a
   free-text `nvarchar` column (no schema/type constraint), so the exact string format is a
   presentation convention, not a database change - ported as `M/d/yyyy 12:00:00 AM` (the
   HTML5 `<input type="date">` supplies no time-of-day, and the picker's own `Value` is never
   time-adjusted anywhere in `Configuration.cs`, so midnight is the correct equivalent).
2. **Load side - a genuine, verified desktop quirk, not an extraction gap:** `BindForm()`
   (the load-time control-population loop, lines ~2006-2170) has an `if`/`else if` chain
   that only handles `CheckBox`, `TextBox`, `ComboBox`, `UltraCombo`, and `RadioButton` -
   there is **no `DateTimePicker` branch anywhere in that method**. Confirmed by scanning the
   entire method body for every `DateTimePicker` occurrence in the file (`FireDateTime`'s own
   definition and the two `ControlEventHandler`/`AssignEventHandlers` dispatch branches are
   the *only* three matches - none of them populate the control on load). This means the
   desktop **saves** this field's changes but **never redisplays the saved value** - the
   control always shows `DateTime.Now` (its WinForms construction-time default) every time
   the screen is opened, regardless of what was previously saved. This is preserved exactly,
   not "fixed": `countx_configuration.js`'s new `"date"` case in `populateAllControls()`
   deliberately does not read `configMap` for this control type, and
   `applyDateTimePickerDefaults()` unconditionally resets the `<input type=date>` to today's
   date on every load, ditto the desktop's own always-shows-today behavior. Change is still
   saved (wired to the input's `change` event, ditto `ValueChanged` -> `ControlEventHandler`
   -> `FireDateTime`) - only the *redisplay* is intentionally absent, matching the desktop.

## QA tab (`tabQA`) - fully implemented, source-verified (6th of 18)

14/14 controls implemented (0 hidden). Verified against `Configuration.cs` lines 11571-11790.

### `rdLabCompulsoryBeforFirstWeight` / `rdLabCompulsoryAfterFirstWeight` - verified, correctly-behaving desktop coupling

A genuine live `ControlEventHandler` pairing (source lines ~3201-3213), and a `BindForm()`
load-time normalization (lines ~2062-2075): `rdLabCompulsoryAfterFirstWeight`'s
`AccessibleName` ("Lab Compulsory After First Weight") is in `BindForm`'s generic-assignment
exclude list, so it never reads its own saved `ConfigKey` - instead, every load,
`rdLabCompulsoryBeforFirstWeight`'s own correctly-loaded value is read and `After` is
force-set to its exact negation (`if (Before.Checked) After=false; else After=true`). Unlike
the Production-tab pair documented below, this **is** a correct, symmetric 2-way negation -
`Before`'s own saved state always wins and `After` is always its logical complement, so no
information is lost across reloads. Implemented with one additional
`pickWinner(["rdLabCompulsoryBeforFirstWeight", "rdLabCompulsoryAfterFirstWeight"])` call
appended to the existing `applyRadioGroupDefaults()` in `countx_configuration.js` - the live
click-time coupling needs no new save-path code, since plain HTML radio grouping
(`name="labCompulsoryTiming"`) plus the pre-existing generic radio dual-fire save handler
already reproduce it exactly.

## Wages tab (`tabWages`) - fully implemented, source-verified (7th of 18)

37/38 controls implemented (1 hidden `GroupBox` sub-feature excluded, see below). Verified
against `Configuration.cs` lines 11790-12539. This tab has 7 `GroupBox`es, several reusing
overlapping local y-ranges - the per-GroupBox extraction method (below) was built here and
reused for WeighBridge/Production: for each container, find its own
`this.<Container>.Controls.Add(this.<Child>)` list, extract each child's own
`Location`/`Text`/`AccessibleName`/`Visible`, and sort/pair *within* that container by its own
relative `y` - never globally, since two different GroupBoxes on this tab independently reuse
the same relative y-coordinates for unrelated controls.

GroupBoxes implemented: `groupBox19` "Comparison by Activity For" (6), `groupBox14` "Regular
Wages (Rice)" (13), `GBCommonWagesPartyProcessing` "Common (Party Processing)" (2),
`GBRegularWagesPartyProcessing` "Regular Wages (Party Processing)" (7), `groupBox16` "Common
(Rice)" (5), `groupBox15` "Other Wages (Rice)" (2 visible), `GBOtherWagesPartyProcessing`
"Other Wages (Party Processing)" (2 visible).

### `grdWagesRefDocuments` / `btnStatusUpdate` - verified out of scope, not fabricated or silently dropped

`PanelWages02`'s reference-documents grid and its Status Update button (source lines
2830-2861, 3729-3752) call `InvContractorWagesBillHeader.GetRefDocumentsForWages()` /
`UpdateContractorWagesRefDocumentStatus()` - a separate document-approval workflow against a
transactional table, not a `ConfigrationsAllocation` setting at all. Documented in the HTML
with an explicit `cfg-not-implemented`-styled info box rather than omitted without comment.

## WeighBridge tab (`tabWeighBridge`) - fully implemented, source-verified (8th of 18)

26/26 controls implemented (0 hidden). Verified against `Configuration.cs` lines 12540-13072.
5 direct-child controls + `groupBox17` "Serial Port" (4 textboxes) + `groupBox6` "Cameras" (16
camera fields + 1 path textbox + 1 excluded browse button), extracted per-container ditto
Wages.

- `CmbCashWithWeighbridgeAccount` reuses the existing `cashEquivalentAccounts` filter
  (type `{2}`, verified identical to `accountsFromGlobalByTypeIds2`).
- `CmbOtherIncomeWithWeighbridgeAccount` is a new type filter, `accountsFromGlobalByTypeIds9`
  = `{10}` ("Revenue - Sales"), added as `revenueSalesAccounts`.
- `groupBox6`'s `btnCamerPath` ("Choose Path") is a `FolderBrowserDialog` trigger, not a
  persisted setting - excluded ditto every other browse-button on this screen. Its target,
  `txtCamerPicturesPath` (`AccessibleName` "CameraPictureBox" - preserved verbatim despite the
  misleading "PictureBox" name), is implemented as a plain text field.

## Production tab (`tabProduction`) - fully implemented, source-verified (9th of 18)

31/33 controls implemented (2 hidden - a checkbox+label pair - excluded). Verified against
`Configuration.cs` lines 13073-13871 (`this.tabProduction.Controls.Add(...)` through
`tabExport`'s own `Controls.Add` start at line 13872). Direct children (21, 2-column layout) +
`groupBox18` "Weight Tolerance" (9 textboxes + 3 column headers + 3 row labels, rendered as a
grid table ditto WeighBridge's camera grid) + `groupBox13` "Packing Material Compulsory on
Stock Conversion" (2 radios).

### Hidden control confirmed and excluded

`chkStockHoldForLabApprovalAutoChecked` / `label393` ("Stock Hold For Lab Approval Auto
Checked") - both `Visible=false` in `InitializeComponent`, excluded ditto every other hidden
control on this screen.

### `rdPackingMaterialCompulsoryOnStockConversionForWarning`/`ForStop` - a MORE SEVERE, asymmetric desktop bug (not a simple negation)

Unlike the QA tab's Lab Compulsory pair (a correct symmetric negation, documented above), this
pair is verified to be a one-sided, data-losing bug:

1. `BindForm()`'s generic-assignment exclude list (line ~2064) excludes only
   `"PackingMaterialCompulsoryOnStockConversionForWarning"` - **`ForStop` is NOT excluded**, so
   `ForStop`'s own saved `ConfigKey` *is* correctly read by the generic loop.
2. Immediately after (lines ~2114-2120), an unconditional normalization block runs:
   `if (Warning.Checked) { Stop=false; } else { Stop=true; }`. `Warning.Checked` is never set
   from its own saved value (excluded above) and its `InitializeComponent` designer default is
   `false` (verified - no `.Checked = true` assignment exists anywhere for it), so this
   condition is **always false**, and the `else` branch **always** runs.
3. Net effect, verified: **`Stop` always ends up `true` on every single screen load, regardless
   of what was actually saved for either radio** - even overwriting `Stop`'s own just-correctly-
   loaded value from step 1. This is strictly worse than the Lab Compulsory pair, which at
   least always reflects `Before`'s real saved state.
4. The live click-time coupling (`ControlEventHandler`, lines ~3252-3262) is a normal, correct
   same-group mutual exclusion - not part of the bug, and reproduced for free by plain HTML
   radio grouping (`name="packingMaterialCompulsoryOnStockConversion"`) plus the existing
   generic radio dual-fire save handler.

Implemented in `countx_configuration.js`'s `applyRadioGroupDefaults()` as an **unconditional
force** (`Stop.checked = true; Warning.checked = false;` every load) - deliberately *not* a
`pickWinner(...)` priority-list call, since `pickWinner` reads each radio's real saved/current
state and would incorrectly let a saved `Warning=True` win, which the real desktop never
allows on load.

### `CmbDefaultProductionStageId` ("Default Production Stage") - CONFIRMED MISSING DEPENDENCY, reported per the Strict Preservation Rule

Traced call chain: `CastingType()` -> (via `using Architecture.BLL.Mfg;`)
`Architecture.BLL.Mfg.LookUps.GetDataByTypeId(3)` (`projects/architecture.bll/`
`0370_Architecture.BLL.Mfg.LookUps.cs`, lines ~85-115) -> stored procedure
`[Mfg].[USP_LookUps_GetAllMethod]` with `@Id=3, @Activity='GetDataByTypeId'`.

**This procedure does not exist in the real database.** Verified two independent ways:

1. Exhaustive regex search of the full `GoldenAceDb(0509)t.sql` export for
   `\[Mfg\]\.\[USP_LookUps_\w*\]` returns exactly 4 matches, **all** `Mfg.USP_LookUps_Insert`
   or `Mfg.USP_LookUps_Update` references - no `GetAllMethod` (or any other read) variant
   exists under the `Mfg` schema for `LookUps`.
2. Cross-checked against `recovered_source/DATABASE_CONTRACT.md` (a pre-existing project
   object-index reference), which likewise lists only `Mfg.USP_LookUps_Insert`/
   `Mfg.USP_LookUps_Update` with no `GetAllMethod` entry.

Per the Strict Database Preservation Rule's explicit "stop-and-report if something required is
missing rather than inventing it" mandate, **no substitute query/table was fabricated**. The
desktop's own combo would throw a SQL "invalid object name" error if this exact code path were
ever exercised against this database - it is not a Java-port gap, it is a pre-existing gap in
the desktop application itself relative to this specific database export. Implemented as a
plain manual-entry text field bound to the same `AccessibleName`/`ConfigKey`
(`DefaultProductionStageId`), with a prominent inline warning note in the HTML - so any value
already saved under this key is still shown/preserved/editable, and GET/INSERT/UPDATE behavior
is otherwise identical to every other textbox on this screen (no schema/data invented, no
functionality silently dropped).

### `getItemCustomGroups()` - new stored procedure, new Java method

`CmbDefaultProductionInputItemCustomGroupId` and `CmbDefaultProductionScrapItemCustomGroupId`
both bind to the desktop's `ItemCustomGroupFill()` (identical call, no differing filter),
backed by `Architecture.BLL.Inventory.ItemCustomGroup.FormHistory()` ->
`dbo.USP_ItemCustomGroup_GetAllMethod` (verified `GoldenAceDb(0509)t.sql`, params `@Id,
@OrganizationId, @CompanyId, @BranchesId, @Activity`), `@Activity='FormHistory'` branch:

```sql
SELECT Id, GroupCode, GroupName, EntryDate, EntryUser, ModifyDate, ModifyUser,
       OrganizationId, CompanyId, BranchesId
FROM dbo.ItemCustomGroup
WHERE OrganizationId=@OrganizationId AND CompanyId=@CompanyId AND BranchesId=@BranchesId
```

Not to be confused with `getAcLookups(1)` (`dbo.AcLookUps`, a completely different table
despite the similar English name) - `CmbDefaultProductionOverHeadCustomGroupId` on this same
tab correctly reuses `getAcLookups(1)`/`freightCustomGroups` instead, verified via
`Sp_AcLookUps_GetAllMethod`'s `'ReadByAcLookUpId'` branch (used by
`AcLookUps.GetByProfileTypeId`) returning the same underlying rows as the `'ReadAll'` branch
that attribute already uses.

### `CmbDefaultCastingWarehouse` - reuses existing Inventory-tab infra, zero new code

Verified bound inside the same `WarehousesDtFillFromGlobalAndBind()` method already
implemented for Inventory's `cmbware`/`CmbVirtualWareHouseId`/`CmbWarehouseStore` - reuses the
existing `warehouses` model attribute directly.

### General infra correction applied this pass (retroactive to all 8 prior tabs)

`countx_configuration.js`'s `populateAllControls()` had `case "textbox": $el.val(configKey !=
null ? configKey : "");` - this **destructively overwrote** any HTML-authored default value
with `""` whenever no saved config row exists yet, which does not match `BindForm()`'s real
behavior (its load loop only ever assigns `txtbx.Text` when a matching saved row exists;
otherwise the control's `InitializeComponent`-authored default `Text` is what actually shows).
This was invisible on every tab implemented so far because none of their textboxes had a
non-blank designer default - it surfaced on Production's `groupBox18` "Weight Tolerance"
textboxes, which do (`txtMinPercentForStockConversion.Text="1"`,
`txtMaxPercentForStockConversion.Text="5"`, etc.). Fixed to
`if (configKey != null) { $el.val(configKey); }` (omitting the `else` branch), and Production's
HTML `<input>` elements carry matching `value="..."` attributes for their designer defaults.

## Export tab (`tabExport`) - fully implemented, source-verified (10th of 18)

36/36 controls implemented (0 hidden - exhaustive `Visible = false` sweep of the full line
range 13872-14855 found no matches). Verified against `Configuration.cs` lines 13872-14855.

**Structurally unique among all tabs so far: `tabExport` has NO real `GroupBox` anywhere in
the source.** It is one flat `TabPage` with 70 direct children (34 real inputs + 34 labels + 2
sub-containers: `panel5`, a small unlabeled `Panel` holding 2 radios, and `PanelMessage`, a
purely decorative `Panel` holding one `ToolStrip`/`ToolStripButton` pair with no
`AccessibleName` and no `ConfigrationsAllocation` key - excluded ditto every other UI-only
control on this screen). The single `<fieldset>` in the HTML is a neutral layout wrapper only
(named after the tab, ditto the Freight Voucher tab's own fieldset) - not a claim that a
corresponding `GroupBox` exists in `Configuration.cs`.

### GL-account combos - 3 of 6 reuse existing infra, 1 is a new type filter

| Combo (`AccessibleName`) | Source | Notes |
|---|---|---|
| `CmbExportReturnStockInTransit` | `accountsFromGlobalByTypeIds11` = `{4}` | Verified identical to Inventory's `cmbInventoryControlAccount` filter - reuses `inventoryControlAccounts` |
| `CmbForeignExchangeGainLossAccount` | `accountsFromGlobalByTypeIds15` = `withoutTypeIds {2,4,11,12,15}` | Verified identical to Account's `cmbBankReconcilationAdjustmentAccountId` filter - reuses `bankReconciliationAdjustmentAccounts` |
| `cmbForeignBaseCurrency` | `Architecture.BLL.MultiCurrency.GetAll()` | Verified: `cmbCurrency()` binds this AND Account's `cmbBaseCurrency` from the exact same `DataTable` - reuses `currencies` |
| `CmbChargesToAccountForFreightVoucherOutward` | new filter `{3}` | "Receivables & Payables" (AP/AR) per the verified `AccountTypes` seed row - new `receivablesPayablesAccounts` attribute |

### `CmbDefaultTranspotationServiceItemId` - new non-account lookup, in-memory desktop cache replaced with a per-request read

Desktop binds this from `clsGlobalVariables.getGlobalAllSerivesItems.Where(r =>
r.ServicesMasterItemId == 5)` - a session-scoped in-memory cache populated once by
`Architecture.BLL.Main.GlobalServicesMethods.Item_AllServiesItems(OrganizationId, CompanyId)`,
backed by the real stored procedure `[lgstcm].[USP_Item_AllServiesItems]` (verified
`GoldenAceDb(0509)t.sql`), called with `@OrganizationId, @CompanyId` only (joins
`lgstcm.vServiceItem`, `ItemAllocation`, `lgstcm.lookUp` for the type-name column). Ported as a
per-request read with the identical `ServicesMasterItemId == 5` filter applied server-side
(the literal `5` is the desktop's own hardcoded argument - its display name from
`lgstcm.lookUp` seed data was not independently confirmed, but the filter value itself is
copied verbatim from source, not reinterpreted).

### `CmbDefaultExportInspectionCountryOfOrigin` - new non-account lookup, verified desktop query gap preserved

Desktop binds this from `Architecture.BLL.country.GetAll(new Country{OrganizationId,
CompanyId})`, backed by `dbo.SP_Country_ReadMethod` (verified `GoldenAceDb(0509)t.sql`),
`@MethodType='GetAll'`. **Verified by reading the procedure's own body**, not just the call
site: the `'GetAll'` branch has no `WHERE` clause at all - it returns every row in
`dbo.Country` globally, completely ignoring the `@OrganizationId`/`@CompanyId` parameters it
declares and is passed. This is ditto a pattern already found on
`Sp_CustomerGroup_GetAllMethod`'s `'ReadAll'` branch (Inventory tab) - preserved exactly here
too, not "fixed" to add a filter the real procedure does not apply.

### `chkToleranceWeightForDifferenceInDispatchedAndInvoice` ("Export Voucher Weight Tolerance") - a CheckBox despite its name

Verified: this is genuinely a `CheckBox` in `Configuration.cs`, not a numeric/percentage
`TextBox` as its caption might suggest - preserved verbatim as a checkbox, ditto the
`ChkBoxSaleOrderQty` naming-vs-label surprise already found and preserved on the Sale tab.

### `panel5`'s radio pair - a THIRD distinct radio pattern (correct, not a bug)

`rdIsInspectionLotMappedOnDeliveryOrderByInvoice`/`ByItem` (`BindForm` lines ~2186-2192,
`ControlEventHandler` lines ~3405-3415) differ from both radio patterns found on QA and
Production: **neither** `AccessibleName` is in `BindForm`'s generic-assignment exclude list, so
both radios correctly read their own saved `ConfigKey` on load via the normal path. The
desktop's own extra normalization (`if (ByInvoice.Checked) ByItem=false; else
ByInvoice=false;`) only changes anything in the never-supposed-to-happen case where *both* were
somehow saved `True` (an inconsistent-data tie-break, `ByInvoice` wins) - when at most one is
true, or both are false, it is a no-op and each radio's real saved state is faithfully
preserved (unlike Production's Warning/Stop pair, this is not a data-losing bug). Implemented
as plain HTML radio grouping + the generic radio dual-fire save handler for the live click
coupling, plus one small anomaly-only tie-break in `countx_configuration.js` (fires only if
both were somehow saved `True`) - neither a `pickWinner()` priority list nor a Production-style
unconditional force, since neither matches this pair's actually-correct default behavior.

### `PanelMessage`/`ToolStripMessage` - decorative, excluded

A small `Panel` containing one `ToolStrip` with a single `ToolStripButton` (`BtnMessage`) - no
`AccessibleName`, no reference anywhere in `BindForm`/`ControlEventHandler`/the save `Fire*()`
handlers. Verified UI-only (most likely an info/help icon rendered next to the "Export Voucher
Weight Tolerance" field) and excluded, ditto every other browse-button/decoration already
excluded on prior tabs.

**Device connectivity note (resolved):** the device link dropped mid-pass after this tab's 5
files were delivered to the user but before `device_commit_files` could run. The link came
back in the next session turn; all 5 files were then committed to
`D:\CShapEccorErp\EccountingERP\...` and their MD5 checksums verified to match the build
workspace exactly (same checksums as originally computed - the content did not change while
disconnected). This tab is now fully closed out, ditto every other implemented tab.

## Store Management tab (`tabStore`) - fully implemented, source-verified (11th of 18)

6/6 controls implemented (0 hidden), verified against `Configuration.cs` lines 14855-14957. No
`GroupBox`, no couplings found in `ControlEventHandler`. Simplest tab so far - no new lookups.

## Point Of Sale tab (`tabPointOfSale`) - fully implemented, source-verified (12th of 18)

9/9 controls implemented (0 hidden), verified against `Configuration.cs` lines 14957-15504. No
`GroupBox`. **All 7 UltraCombos reuse pre-existing model attributes** - zero new Java code
needed: `CmbCashInHandPos`/`CmbWsRmDiscountReceived`/`CmbPosDiscountReceived` reuse
`cashEquivalentAccounts`; `CmbLocationAccount` reuses `operatingCostOfSalesAccounts`;
`CmbWsRmDiscountAllowed`/`CmbPosDiscountAllowed` reuse `revenueSalesAccounts`;
`CmbWareHouseNamePos` reuses `warehouses` (verified: `WarehousesDtFillFromGlobalAndBind()` binds
this from the exact same call as `cmbware`/`CmbDefaultCastingWarehouse`/`CmbVirtualWareHouseId`/
`CmbWarehouseStore`). Self-caught documentation-accuracy error this pass: an inline HTML comment
initially said "6 UltraCombos reuse infra" while the structural check found 7 `<select>`
elements - cross-checked against the original extraction and corrected to "7" (no functional/
data defect, a comment-text-only catch).

## MobileSMS tab (`tabMobileSMS`) - fully implemented, source-verified (13th of 18)

5/5 controls implemented (0 hidden), verified against `Configuration.cs` lines 15504-15579. No
`GroupBox`, no couplings, no new lookups.

## Party Processing tab (`tabPartyProcessing`) - fully implemented, source-verified (14th of 18)

8/8 controls implemented (0 hidden): 3 direct (2 checkboxes + 1 textbox) + `groupBox11`'s 5
radios ("Mange Stock By"). Verified against `Configuration.cs` lines 15579-15688.

### `groupBox11` "Mange Stock By" - a FOURTH distinct radio pattern (5-way priority chain, first member excluded)

`rdCheckStockPartyItemWareHouse`/`...CropYear`/`...CropYearJobLot`/`...CropYearJobLotPackUom`/
`...CropYearJobLotPackingTypeUom` (`BindForm` lines ~2122-2153). Structurally like the Account
tab's existing 3-way/2-way `pickWinner()` groups, **except its first member,
`rdCheckStockPartyItemWareHouse`, is itself in `BindForm`'s generic-assignment exclude list**
(`AccessibleName` "CheckStockPartyItemWareHouse", verified line ~2064) - so unlike every
`pickWinner()` group before it (where every member reads its own real saved value), this one can
NEVER win on load no matter what its own `ConfigKey` says. Implemented by forcing it `false`
immediately before calling `pickWinner([...])` (ditto Production's Warning/Stop force, so
`pickWinner()`'s own "is it checked" test can never see it as a candidate) - the remaining 4
members all correctly read their own real saved values, with `CropYearJobLotPackingTypeUom` as
the verified final-else default winner.

**Noted for System tab:** a second, parallel, non-"Party" 5-way radio group
(`rdItemWarehouse`/`...CropYear`/`...CropYearJobLot`/`rdItemWarhouseCropJobPackUOM`/
`rdCheckStockItemWareHouseCropYearJobLotPackingTypeUom`, `AccessibleName`s
"CheckStockItemWareHouse"/etc., no "Party") was spotted nearby in `BindForm`'s own normalization
block during this pass. **Resolved this session (Commission Agent pass):** its container,
`groupBox9`, is confirmed to belong to `tabSystem`, not Commission Agent or Party Processing -
`groupBox9.Location` appears immediately after `tabSystem.Location` in `InitializeComponent`
(line ~17408, i.e. past Commission Agent's own line-17258 boundary). BindForm's own logic for it
(`BindForm` lines ~2153-2182, read in full this session) is structurally identical to
`groupBox11` above - a 5-way `pickWinner`-style chain with `rdItemWarehouse` itself excluded from
the generic assignment (same exclude-list treatment as `rdCheckStockPartyItemWareHouse`) - ready
to implement directly by the same pattern when System tab is reached.

## HRM tab (`tabHRM` / nested `tabControlHRM` -> `tabAttendance`) - fully implemented, source-verified (15th of 18)

`tabHRM`'s only direct child is a nested `TabControl`, `tabControlHRM` (declared line 5854),
confirmed via `this.tabControlHRM.Controls.Add(...)` to hold exactly ONE page, `tabAttendance`
("Attendance") - unlike Purchase's 4-page nested `TabControl`, a single-page one has no visible
sub-tab strip to replicate, so its controls are rendered directly without the
`win-sub-tabs`/`switchConfigSubTab` machinery. 7/7 direct children of `tabAttendance` implemented
(0 hidden), verified against `Configuration.cs` lines 15688-15783: 2 independent checkboxes
(`chkOneDayAdjacentHolidayAbsenceRuleEitherPreviousOrNext`/`...IfBothPreviousAndNext`,
no coupling to each other or the radio group below), a 3-radio group, `chkPayRollPostingWithoutApproval`,
and 1 label.

### "Days For Adjacent Holiday Absence Rule" - a FIFTH, wholly new radio pattern (shared single numeric ConfigKey)

`RadValueZero`/`radValueTwo`/`radValuethree` ("Off"/"2 Days"/"3 Days") **share the IDENTICAL
`AccessibleName`** ("DaysForAdjacentHolidayAbsenceRule") - verified `BindForm` (lines ~2048-2062)
and `ControlEventHandler` (lines ~3175-3197): there is exactly ONE saved
`ConfigrationsAllocation` row for the whole group, whose `ConfigKey` is a literal numeral string
("0"/"2"/"3"), not "True"/"False" - saved via a throwaway `TextBox` routed through `FireTextBox`,
not the boolean per-radio dual-fire every other radio group on this screen uses. Implemented with
a new `data-config-type="radio-numeric"` contract (population guarded on `configKey != null` so
the HTML default, `RadValueZero`/"Off" per `InitializeComponent`'s own `Checked = true`, survives
until a value is actually saved; each radio's own `data-radio-value` compared against the one
shared `configKey`), and a dedicated single-save change handler restricted to
`data-config-type='radio-numeric'` (the existing boolean-radio selector was narrowed to
`data-config-type='radio'` so the two never cross-fire).

## Commission Agent tab (`tabCommissionAgent`) - fully implemented, source-verified (16th of 18)

Verified against `Configuration.cs` lines 15783-17258 (boundary confirmed:
`this.tabSystem.Controls.Add(...)` begins immediately at line 17258/17260). 15 direct children:
6 simple controls (2 checkboxes + 4 `Tag="double"` tolerance-percent textboxes, no
`ControlEventHandler` coupling found for any of the 6) + 2 decorative labels + 3 `GroupBox`es
(`GBForLateVehicleArrivalAfterPoExpiry`, `groupBox20`, `GBComm`). No hidden (`Visible=false`)
controls found across the full range (exhaustive sweep). Largest single-tab control count so far:
6 direct + 3 (`groupBox20`) + 3 (`GBForLateVehicleArrivalAfterPoExpiry`) + 17 combos + 2
non-persisted display-toggle radios (`GBComm`) = **29 persisted settings + 2 UI-only controls**.

### `groupBox20` "GRN Loading Challan Doc Date And Bilty Date" - a normal 3-way priority chain

`radGrnLoadingAllowDifferentBiltyDate`/`...Same`/`...NoValidation` (`BindForm` lines
~2194-2207). Verified none of the 3 `AccessibleName`s are in `BindForm`'s generic-assignment
exclude list (the same exclude list checked for every prior tab, re-read in full this pass, line
~2064), so all 3 correctly read their own saved `ConfigKey` - structurally identical to the
Account tab's existing `pickWinner()` groups, priority
`AllowDifferentBiltyDate > Same > NoValidation` (implicit default winner, never force-set).
`ControlEventHandler`'s own save path (lines ~3418-3436) is a more convoluted partial-fire
sequence than a plain `pickWinner` group's net effect (some sibling saves are skipped depending
on which specific radio triggered the handler) - but its converged end-state is exactly what the
existing generic `data-config-type="radio"` change handler already produces (it saves every
member of the same `name="..."` group on any one member's change), so no new save-path code was
needed, only the load-side `pickWinner([...])` call.

### `GBForLateVehicleArrivalAfterPoExpiry` "Late Vehicle Arrival After Po Expiry Date" - a SIXTH, new radio pattern (forced-true fallback over independently-persisted members)

`RadShowWarningForLateVehicleArrivalAfterPoExpiryCommissionAgentPortal`/
`RadBlockEntryForLateVehicleArrivalAfterPoExpiryCommissionAgentPortal`/
`RadNoValidationForLateVehicleArrivalAfterPoExpiryCommissionAgentPortal` (`BindForm` lines
~2209-2219, read in full this pass). Verified: Warning and Block are each independently
persisted (each has its own `AccessibleName`/saved row, read via an explicit
`if (radioButton.AccessibleName == "...")` branch inside the same generic per-item loop -
functionally equivalent outcome to the plain unexcluded case, just written differently).
NoValidation ALSO has its own `AccessibleName`/saved row (also not excluded) **yet `BindForm`
unconditionally overrides it to `Checked=true` whenever NEITHER Warning nor Block ends up
checked** (line ~2217-2219) - forcibly ignoring its own just-loaded value, unlike `groupBox20`'s
fallback member above (which is never forced, it simply keeps whatever its own saved value is).
There is no priority tie-break between Warning and Block themselves (both read independently, no
`if/else-if` between them) - only the "neither is true" fallback is special, so this is
implemented as one extra conditional in `applyRadioGroupDefaults()` after the plain per-radio
load already ran, not a `pickWinner()` call. All 3 still individually save via the plain radio
dual-fire handler (`ControlEventHandler`'s own save path, lines ~3441-3461, read in full -
functionally a normal 3-way mutual exclusion with `FireRadioButton` calls, already covered by the
existing generic save handler).

### `GBComm` "Defaul Values" - 17 UltraCombos + a non-persisted display-mode toggle

The largest `GroupBox` on the whole screen. Every combo's data source traced directly against
`Configuration.cs` (not guessed from caption text):

| Combo (`AccessibleName`) | Source | Reuse or new? |
|---|---|---|
| `DefaultCommissionAgentIdForCommissionAgentPortal` / `DefaultSubCommissionAccountIdForCommissionAgentPortal` / `DefaultSubBrokerageAccountIdForCommissionAgentPortal` | `SupplierDtFillFromGlobal()` -> `CommonBindings.CommissionAndBuyerBind()`, lines ~2298-2301 | **New** - `getSupplierCustomers()`, backed by `dbo.USP_GetVendorsAndCustomersWithCityName` |
| `DefaultTradingAccountIdForCommissionAgentPortal` / `DefaultTaxAccountIdForCommissionAgentPortal` | `AccountBindFromGlobal()`'s own un-suffixed `accountsFromGlobalByTypeIds` = `GetAccountsFromGlobalByTypeIds(null, {2,10,11,12,15})` | Reuse `stockAccountsExcludingCoreTypes` (byte-for-byte the same exclude array as `TYPE_EXCLUDE_STOCK_ACCOUNTS`) |
| `DefaultPaymentTermIdForCommissionAgentPortal` | `PaymentTermsFill()` -> `Sp_InvDueTerms_GetAllMethod` | **New** - `getPaymentTerms()` |
| `DefaultDeliveryTermIdForCommissionAgentPortal` | `DeliveryTermFill()` -> `[dbo].[USP_DeliveryTerm_GetAllMethod]` | **New** - `getDeliveryTerms()` |
| `DefaultCropYearIdForCommissionAgentPortal` | `CropDtFillFromGlobalAndBind()` (same `source` as Inventory's `comCropYear`) | Reuse `cropYears` |
| `DefaultPackingTypeIdForCommissionAgentPortal` | `PackingTypeDtFillFromGlobalAndBind()` (same `source` as Inventory's `comPakingType`) | Reuse `packingTypes` |
| `DefaultLoadingCityIdForCommissionAgentPortal` / `DefaultUnloadingCityIdForCommissionAgentPortal` | `CityBindFromGlobal()` (same `source` as Inventory's `cmbcity`) | Reuse `cities` |
| `CustomGroupForWHTAccounts` / `CustomGroupForWHTAccountsSale` | `BindCustomGroupAccounts()`'s own `AcLookUpTypesId = 1` call | Reuse `freightCustomGroups` |
| `DefaultTaxTypePurchaseeIdForCommissionAgentPortal` / `DefaultTaxTypeSaleIdForCommissionAgentPortal` | `TaxTypeBind(DatatableHelper.TaxTypesDbCall(UserAccount))` -> `Sp_TaxesTypes_GetAllMethod @Type=1` | **New** - `getTaxTypes()` |
| `DefaultWhtAccountPurchaseIdForCommissionAgentPortal` / `DefaultWhtAccountSaleIdForCommissionAgentPortal` | `CmbCustomGroupForWHTAccounts_Leave()` / `...Sale_Leave()`, an in-memory `CustomGroupId` re-filter of the same `USP_GETAllAccountsFromCustomGroups` dataset (verified: `getGlobalAllAccountsWithCustomGroup.CustomGroupId` is a real column, confirmed by reading the model class) | **New** - `getAccountsByCustomGroup(int)`, live endpoint (not server-rendered - see verified load-time bug below) |

**Verified desktop load-time bug, reproduced exactly (not "fixed"):** `Configuration.cs`'s own
constructor/load path calls `CmbCustomGroupForWHTAccounts_Leave(null,null)` **TWICE** and never
calls `CmbCustomGroupForWHTAccountsSale_Leave()` at all. Net effect on the real desktop: the WHT
Purchase Account combo IS pre-filtered by whatever custom group is already saved for
`CmbCustomGroupForWHTAccounts` on screen open; the WHT Sale Account combo's option list is always
empty on open no matter what is saved for `CmbCustomGroupForWHTAccountsSale` or the Sale WHT
account itself, until the user actually changes/leaves `CmbCustomGroupForWHTAccountsSale` in that
session. Reproduced via `countx_configuration.js`'s `applyWhtAccountsCoupling()`: called with
`refreshOnLoad=true` for the Purchase side (fires a live `GET
/api/configurations/accounts-by-custom-group` immediately if a custom group is already saved,
matching the desktop's guaranteed-call-on-open) and `refreshOnLoad=false` for the Sale side (only
wires the live-refresh `change` handler, never fires it on load) - both server-rendered option
lists start empty (ditto Freight Voucher's own checked-branch precedent of a combo populated only
via live fetch, not server-render, when its full list isn't render-cheap).

**`RadNickName`/`RadBusinessName` - NOT a persisted setting, a pure client-side display toggle.**
Verified: **neither radio has an `AccessibleName` set anywhere in `InitializeComponent`** - the
only unpersisted radio pair discovered anywhere on this 18-tab screen (every other radio,
including HRM's shared-`AccessibleName` numeric group, has at least one real database-backed
key). `ControlEventHandler`'s own `RadioButton` branch explicitly special-cases this pair (by
`.Name`, not `.AccessibleName`) to call `RadBusinessName_CheckedChanged()` instead of
`FireRadioButton()` - confirming no save ever happens. It toggles which single text column
(`CompanyName` vs `NickName`, each additionally prefixed `"{parent} / {self}"` for a resolvable
sub-supplier/customer - verified via `SupplierDtFillFromGlobal()`'s own
`Dictionary<int,getGlobalAllSupplierCustomer>` lookup, not recursive) is shown for the 3
supplier/customer combos above; default is "Business Name"
(`RadBusinessName.Checked = true` in `InitializeComponent`). Implemented by precomputing BOTH
text variants server-side per row (`getSupplierCustomers()` returns `{Id, BusinessText,
NickText}`) and swapping each `<option>`'s displayed text client-side on this pair's own
(unsaved) `change` event - no AJAX call, no `data-config-description`/`data-config-type`
attribute on either radio at all (deliberately excluded from the generic save/load dispatch).

## System tab (`tabSystem`) - fully implemented, source-verified (17th of 18)

Verified against `Configuration.cs` lines 17258-17587 (boundary confirmed:
`this.tabSystem.Controls.Add(...)` begins at line 17258, immediately after Commission Agent's own
closing boundary). 24 direct children extracted via a targeted grep of the full
`Location`/`AccessibleName` block: 5 checkboxes (`chkIsVpsAttachmentsServiceOn`,
`ChkIsVpsWeighbridgeImagesServiceOn`, `chkCrystalAllowFromFTPServer`, `chkTwoWayAuthentication`,
`chkGenerateDocNoMonthWise`) + 3 visible plain textboxes (`txtRootFilePath`,
`txtDefaultDaysToLessFromHistoryFromDate`, `txtCrystalPrintPreviewZoomDefault` - none has a `Tag`
in source, so none gets an `inputmode` hint) + 1 hidden textbox + 9 decorative labels + 6
`GroupBox`es (`groupBox9`, `groupBox8`, `groupBox7`, `groupBox4`, `groupBox3`, `groupBox2`). A
full `Visible=false` sweep of the whole range confirmed exactly 2 hidden controls: `label120` and
`txtModuleIconPath` ("Modules Icon Path" / `ModuleIcons` setting) - both correctly excluded from
the HTML, not fabricated as visible. No `ControlEventHandler` coupling exists for any direct
control on this tab except `groupBox9`'s own radio group (see below) - confirmed by re-reading
every `AccessibleName`/`.Name` reference in `ControlEventHandler` that could plausibly touch a
System-tab control; none did beyond the one already-documented radio group.

### `groupBox9` "Mange Stock By" - the SECOND occurrence of the 5-way priority-chain/first-member-excluded pattern

`ChkBoxIsMinusStockBalance`/`IsMinusStockAllowed` (a plain checkbox, no coupling) plus a 5-way
radio group: `rdItemWarehouse`/`CheckStockItemWareHouse`,
`rdItemWarehouseCropYear`/`CheckStockItemWareHouseCropYear`,
`rdItemWarehouseCropYearJobLot`/`CheckStockItemWareHouseCropYearJobLot`,
`rdItemWarhouseCropJobPackUOM`/`CheckStockItemWareHouseCropYearJobLotPackUom`,
`rdCheckStockItemWareHouseCropYearJobLotPackingTypeUom`/`CheckStockItemWareHouseCropYearJobLotPackingTypeUom`
(`BindForm` lines ~2153-2182, read in full this pass). Structurally identical to Party
Processing's `groupBox11` block (14th tab): the first member's `AccessibleName`
(`CheckStockItemWareHouse`) is itself in `BindForm`'s generic-assignment exclude list, confirmed
by re-reading the exclude list at line ~2064 (`... != "CheckStockPartyItemWareHouse" && ... !=
"CheckStockItemWareHouse"` - both entries present side by side), so it can never win on load no
matter what its own `ConfigKey` says. The remaining 4 members all correctly read their own real
saved values via the generic radio case, with `CheckStockItemWareHouseCropYearJobLotPackingTypeUom`
as the verified final-else default winner when none of the other 4 (including the always-false
first member) are checked. This resolves the open question carried over from the Party
Processing/Commission Agent passes about which tab `groupBox9` belongs to: confirmed definitively
via a large `InitializeComponent` grep dump (its `Location` line appears immediately after
`tabSystem.Location`, i.e. past Commission Agent's own line-17258 boundary) - it belongs to
`tabSystem`, not Commission Agent or Party Processing. `ControlEventHandler`'s own save path for
this group (line ~3347, read in full) is a normal dual-fire radio save, already covered by the
existing generic `data-config-type="radio"` change handler - only the load-side `pickWinner([...])`
call (with `rdItemWarehouse` forced `checked=false` first, ditto `groupBox11`'s own pattern) was
new JS.

### Five simple browse-button-paired textbox `GroupBox`es

`groupBox2` ("Attachment Path", `txtpath`/`Attachment Folder Path`), `groupBox3` ("Database
Backup", `txtdatabasepath`/`Backup Folder Path`), `groupBox4` ("Report Folder", `txtreportPath`/
`Report Folder Path`), `groupBox7` ("ServerFileURL", `txtServerFileURL`/`ServerFileURL`),
`groupBox8` ("ShareLocalDrivePath", `txtShareLocalDrivePath`/`ShareLocalDrivePath`) - each a
single plain textbox setting plus a `Browse...` button. Each `Browse...` button opens a WinForms
`FolderBrowserDialog`/`OpenFileDialog` local to the desktop process only (no server-side
counterpart, no `AccessibleName`, no persisted setting of its own) - correctly excluded from the
HTML, consistent with the same browse-button exclusion already established and documented for the
WeighBridge tab (8th of 18). Legends taken directly from each `GroupBox`'s own (trimmed) `.Text`
value, not guessed from the child textbox's caption.

## Govt Wheat tab (`tabGovtWheat`) - fully implemented, source-verified (18th and FINAL of 18)

Verified against `Configuration.cs` lines 17581-17810 (`this.tabGovtWheat.Controls.Add(...)`
begins at line 17581, immediately after System's own line-17587 boundary; the range ends at
`label398`, right before `InitializeComponent` moves on to the shared `panel2`/`lblCaption`
form-level chrome - confirming this is genuinely the last tab). Caption **re-confirmed** this pass
at line 17593: `this.tabGovtWheat.Text = "Govt. Wheat"` (WITH a period), not "Govt Wheat" -
matching the earlier-segment finding exactly, now applied. The smallest tab on the whole screen: 7
direct children, all visible (exhaustive `Visible=false` sweep of the full range found none) - 1
`GroupBox` (`groupBox21`) + 2 `UltraCombo`s + 1 plain textbox + 3 labels = 6 persisted settings (3
inside `groupBox21` + 3 direct) + 3 caption-only labels. No `ControlEventHandler` coupling exists
for any control on this tab (grep of every `AccessibleName`/`.Name` reference across the whole
range found zero matches beyond the `InitializeComponent`/`BindForm`/save-path sites already
covered here) - every setting uses the plain generic load/save dispatch, no new JS needed.

- `CmbGovernmentWheatAdvanceAccountsCustomGroup` (`GovernmentWheatAdvanceAccountsCustomGroup`) is
  bound by `BindCustomGroupAccounts()` (full method body re-read, lines ~2536-2555) using the
  EXACT SAME `byProfileTypeId` `AcLookUps` dataset (`AcLookUpTypesId=1`) as
  `CmbDefaultProductionOverHeadCustomGroupId`, `CmbCustomGroupForWHTAccounts`, and
  `CmbCustomGroupForWHTAccountsSale` - all four calls share one method body, one dataset, one bind
  signature (`"Id"`/`"AcLookUpsDescription"`). Reuse of the already-implemented
  `freightCustomGroups` model attribute - zero new Java code.
- `txtNoOfBodyForGovtWheatQuotaPolicy` (`NoOfBodyForGovtWheatQuotaPolicy`) - a plain textbox, no
  `Tag` set in source (no `inputmode` hint), no coupling.
- `groupBox21` "Empty Bags Deduction Information" - 3 weight-cut settings (`textBox8` /
  `GovtPurchaseWeightCutForJuteBags`, `textBox11` / `GovtPurchaseWeightCutForPPBags`, `textBox12`
  / `GovtPurchaseWeightCutForOpenBulk`), no `Tag` set on any, captions taken from each row's own
  paired label matched by `Location.Y` proximity within the container (`label402` "Weight" is a
  column-header label only, no paired input, correctly excluded as decorative).

### `CmbMillCategory` (`DefaultMillCategory`) - a SECOND confirmed missing dependency, reported per the Strict Preservation Rule

The desktop's own data source is `MillCategory()` (full method body re-read, line ~2525) ->
`Architecture.BLL.Inventory.GCT.GctQuotaPolicy.GetMillCategory()`. Verified by an exhaustive
search of the ENTIRE recovered/decompiled source tree (every `.cs` file under this project's
recovery output, not just the Configuration screen, plus the full `architecture.bll`/
`architecture.dal`/`architecture.model` recovered-projects trees) that no class or method
definition for `Architecture.BLL.Inventory.GCT.GctQuotaPolicy` exists anywhere - only two WinForms
call sites reference it (this screen and
`Architecture.WinApp.Purchase.frmGovernmentWheatQuotaPolicy.cs`, which independently corroborates
the same `{Id, Value}` bind shape and confirms `DefaultMillCategory` is genuinely the real config
key other screens read back via `GlobalVariables_Helper.GetConfigValueFromGlobal`). Cross-checked
further against BOTH the real `GoldenAceDb(0509)t.sql` export (decoded as UTF-16LE, regex search)
AND `DATABASE_CONTRACT.md`: zero matches for "MillCategory" in either - no table, column, or
stored procedure by that name exists in the real database at all. The desktop's own combo would
throw at runtime if this code path were ever exercised against this exact database - exactly the
same class of confirmed-missing dependency as the Production tab's `CmbDefaultProductionStageId`
(9th of 18, see that tab's section above), implemented identically: a plain manual-entry text
field bound to the SAME `AccessibleName`/`ConfigKey`, so any value already saved under this key is
still shown/preserved/editable, with GET/INSERT/UPDATE behavior otherwise identical to every other
textbox on this screen - not fabricated, not silently dropped.

**All 18 tabs are now implemented and source-verified.** The `tabControlHRM` placeholder markers
(`cfg-not-implemented`) no longer appear anywhere in `configuration.html` except the one
deliberately-documented Wages tab out-of-scope note (the document-approval status grid).

## Source-level vs. runtime verification status - per tab (explicit, not combined)

Per the user's explicit instruction, a tab is never marked "complete" on source-level
verification alone - the two results are tracked and reported separately for every tab.

| Tab | Source-level verification | Runtime verification |
|---|---|---|
| Account (1/18) | **PASS** - 53/57 controls (4 hidden, excluded), all captions/keys/filters/couplings re-verified against `Configuration.cs` and `GoldenAceDb(0509)t.sql` | **NOT POSSIBLE** - environment limitation (see below) |
| Freight Voucher (2/18) | **PASS** - 5/5 controls, 1 coupling verified | **NOT POSSIBLE** - environment limitation (see below) |
| Inventory (3/18) | **PASS** - 33/34 controls (1 hidden, excluded), 7 new stored procedures independently verified against the SQL dump's own `CREATE PROCEDURE` bodies | **NOT POSSIBLE** - environment limitation (see below) |
| Purchase (4/18) | **PASS** - 59/65 controls (6 hidden, excluded), nested-sub-tab structure verified, dense-cluster mismatches hand-corrected | **NOT POSSIBLE** - environment limitation (see below) |
| Sale (5/18) | **PASS** - 51/55 controls (4 hidden, excluded), ERP-feature-flag dual-source chain fully traced to real procedures/tables, DateTimePicker load-bug verified and preserved | **NOT POSSIBLE** - environment limitation (see below) |
| QA (6/18) | **PASS** - 14/14 controls, correct symmetric radio negation verified and preserved | **NOT POSSIBLE** - environment limitation (see below) |
| Wages (7/18) | **PASS** - 37/38 controls (1 out-of-scope document-approval sub-feature excluded and documented, not silently dropped) | **NOT POSSIBLE** - environment limitation (see below) |
| WeighBridge (8/18) | **PASS** - 26/26 controls, 1 new GL-account type filter verified, browse-button correctly excluded | **NOT POSSIBLE** - environment limitation (see below) |
| Production (9/18) | **PASS** - 31/33 controls (2 hidden, excluded), 1 new stored procedure traced/verified, 1 asymmetric radio-pair bug verified and preserved, 1 confirmed-missing stored procedure reported per the Strict Preservation Rule (not fabricated) | **NOT POSSIBLE** - environment limitation (see below) |
| Export (10/18) | **PASS** - 36/36 controls (0 hidden), 2 new stored procedures traced/verified (one with a verified query-scope gap preserved as-is), a 3rd distinct (correct, non-bug) radio pattern verified and preserved | **NOT POSSIBLE** - environment limitation (see below) |
| Store Management (11/18) | **PASS** - 6/6 controls, no couplings, no new lookups | **NOT POSSIBLE** - environment limitation (see below) |
| Point Of Sale (12/18) | **PASS** - 9/9 controls, all 7 combos confirmed to reuse pre-existing model attributes | **NOT POSSIBLE** - environment limitation (see below) |
| MobileSMS (13/18) | **PASS** - 5/5 controls, no couplings, no new lookups | **NOT POSSIBLE** - environment limitation (see below) |
| Party Processing (14/18) | **PASS** - 8/8 controls, a 4th distinct (5-way priority chain, first member excluded) radio pattern verified and preserved | **NOT POSSIBLE** - environment limitation (see below) |
| HRM (15/18) | **PASS** - 7/7 controls, single-page nested TabControl resolved, a 5th wholly new radio pattern (shared numeric ConfigKey) verified and preserved | **NOT POSSIBLE** - environment limitation (see below) |
| Commission Agent (16/18) | **PASS** - 29/29 persisted settings (6 direct + 3+3 radio groups + 17 combos) + 2 correctly-excluded non-persisted UI-only radios, 4 new stored procedures traced/verified, a 6th distinct (forced-true fallback) radio pattern and a verified asymmetric load-time bug both preserved | **NOT POSSIBLE** - environment limitation (see below) |
| System (17/18) | **PASS** - 22/24 controls (2 hidden, excluded), no new Java/lookup code needed, a 2nd occurrence of the 5-way priority-chain/first-member-excluded radio pattern (`groupBox9`) verified and preserved, 5 browse-button-paired folder-path textboxes correctly excluding their local-only Browse buttons | **NOT POSSIBLE** - environment limitation (see below) |
| Govt Wheat (18/18) | **PASS** - 7/7 direct controls (0 hidden), 6/6 persisted settings implemented, caption correction ("Govt. Wheat") re-confirmed and applied, 1 combo reusing `freightCustomGroups`, a 2nd confirmed-missing stored-procedure dependency (`GctQuotaPolicy.GetMillCategory()`) reported per the Strict Preservation Rule and reproduced as a manual-entry field, not fabricated | **NOT POSSIBLE** - environment limitation (see below) |

**Runtime verification is, and will remain, "NOT POSSIBLE" for every tab in this task**, not
a per-tab gap - the reason is a fixed environment limitation (no compiler/runtime/database
access in either available sandbox channel), re-confirmed below, not something that changes
tab to tab. This is reported honestly here rather than a compiled-and-tested claim being
implied by "implemented." Source-level verification is the exhaustive line-by-line
cross-check against the decompiled desktop source (`Configuration.cs`) and the real database
schema/procedure/seed-data dump (`GoldenAceDb(0509)t.sql`) - every control, `AccessibleName`
database key, dropdown data source, `ControlEventHandler` coupling, hidden-control exclusion,
and the GET/INSERT/UPDATE code paths in `ConfigurationServiceImpl`/`ConfigurationApiController`
that back them. It does **not** include compiling, starting the application, or a live
database round-trip - see "NOT tested" below for exactly what that would require and why it
cannot be done in this task.

## Verification performed this session (source-level only - see "Not tested" below)

- Both new stored procedures (`USP_GETAllAccountsFromCustomGroups`,
  `Sp_AcLookUps_GetAllMethod`) extracted verbatim from `GoldenAceDb(0509)t.sql` (UTF-16LE)
  and their parameter lists cross-checked against the BLL methods that call them.
- `dbo.AccountTypes`'s full 22-row seed data extracted from the dump's own `INSERT`
  statements (not assumed/typed from memory).
- Every one of the 8 Account-tab GL-account combos' filter arguments re-verified line by
  line directly against `AccountBindFromGlobal()`'s own local-variable definitions (not
  re-derived from the combo's caption/name, which is what led to the `cmbBCA` naming
  surprise being caught rather than missed).
- All 7 new Inventory-tab stored procedures independently re-verified against the SQL
  dump's own `CREATE PROCEDURE` bodies (not just the C# BLL call sites) - parameter lists
  and result-set column names both confirmed, including two non-obvious findings caught
  only by reading the actual procedure text: `Sp_InvPackingType_GetAllMethod` takes no
  `@OrganizationId`/`@CompanyId` at all, and `Sp_CustomerGroup_GetAllMethod`'s `ReadAll`
  branch has no `WHERE` clause (returns all organizations' customer groups - ported
  identically, not "fixed", per the preservation rule).
- `node --check countx_configuration.js` - passes (syntax only, no runtime).
- HTML structural check (scripted): **91/91 unique** `data-config-description` values across
  all three implemented tabs (53 Account + 5 Freight Voucher + 33 Inventory), no duplicates;
  all 18 nav-tab targets match 18 rendered tab-pane ids; balanced div (123/123) /
  fieldset (6/6) / select (25/25) / label (91/91) tag counts.
- All 18 `th:each` Thymeleaf collection references in the HTML cross-checked one-for-one
  against `ConfigurationViewController`'s `model.addAttribute(...)` calls - no orphaned
  template variable, no unused controller attribute.
- Java brace/paren balance check on all 4 touched/created Java files (`IConfigurationService`,
  `ConfigurationServiceImpl`, `ConfigurationApiController`, `ConfigurationViewController`) -
  all balanced.
- File-transfer integrity: MD5 checksums of every committed file matched exactly between
  the build workspace and the device's copy after each `device_commit_files` call (no
  corruption/truncation in transit) - 6 files after the Freight Voucher pass, 4 more
  (`IConfigurationService`, `ConfigurationServiceImpl`, `ConfigurationViewController`,
  `configuration.html`) after the Inventory pass, 1 (`configuration.html`) after the Purchase
  pass, and 5 (`ConfigurationViewController`, `IConfigurationService`,
  `ConfigurationServiceImpl`, `countx_configuration.js`, `configuration.html`) after the Sale
  pass.
- Purchase and Sale tabs: every `ControlEventHandler`-extracted control name (59 for
  Purchase, 51 for Sale) individually checked against the dispatcher's full body - zero
  couplings found on either tab.
- Sale tab: the `USP_GetERPFeaturesByCompanyId`, `USP_GetVendorsAndCustomersForTransporter`,
  and `Sp_COAAllocation_GetAllMethod` procedure bodies were each read in full from the SQL
  dump (not just grepped for existence) to confirm parameter lists, result-set column names,
  and - critically - which column (`Id` vs `ChartofAccountId`) is the one actually bound as
  the combo's value.
- HTML structural check (scripted) re-run after both passes: **201/201 unique**
  `data-config-description` values across all five implemented tabs (53 Account + 5 Freight
  Voucher + 33 Inventory + 59 Purchase + 51 Sale), no duplicates; all 18 nav-tab targets
  match 18 rendered tab-pane ids; all 4 Purchase sub-tab targets match 4 rendered sub-tab-pane
  ids; balanced div (225/225) / fieldset (12/12) / select (30/30) / table (1/1) / label
  (189/189) tag counts.
- `node --check countx_configuration.js` re-run after adding `switchConfigSubTab` and the
  `"date"` type support - passes.
- Java brace balance re-checked on `IConfigurationService`/`ConfigurationServiceImpl`/
  `ConfigurationViewController` after both passes - all balanced (parenthesis counts inside
  multi-line Javadoc prose are intentionally uneven across line-wraps and do not affect
  compilation - Java does not parse comment contents).
- QA/Wages/WeighBridge/Production pass: `node --check countx_configuration.js` re-run after
  adding the Packing Material force-override and the textbox-default-preservation fix -
  passes. HTML structural check (scripted) re-run across all nine implemented tabs: **309/309
  unique** `data-config-description` values (53 Account + 5 Freight Voucher + 33 Inventory +
  59 Purchase + 51 Sale + 14 QA + 37 Wages + 26 WeighBridge + 31 Production), no duplicates;
  balanced div (306/306) / fieldset (29/29) / select (36/36) / table (3/3) / label (272/272)
  tag counts. Java brace balance re-checked on all three touched Java files after the
  Production pass - all balanced (`ConfigurationServiceImpl` 57/57, `IConfigurationService`
  14/14, `ConfigurationViewController` 20/20). Production tab's control count (31 implemented
  + 2 hidden = 33) independently cross-checked against the raw per-container extraction dump
  before writing any HTML, confirming no control was missed or duplicated.
- File-transfer integrity, QA/Wages/WeighBridge/Production passes: MD5 checksums of every
  committed file matched exactly between the build workspace and the device's copy after each
  `device_commit_files` call - 2 files after QA, 1 after Wages, 2 after WeighBridge, 5
  (`configuration.html`, `countx_configuration.js`, `ConfigurationServiceImpl.java`,
  `IConfigurationService.java`, `ConfigurationViewController.java`) after Production.
- Export pass: `node --check countx_configuration.js` re-run after adding the
  inspection-lot-mapping tie-break - passes. HTML structural check (scripted) re-run across all
  ten implemented tabs: **345/345 unique** `data-config-description` values (309 from the prior
  nine tabs + 36 Export), no duplicates; Export tab's own sub-count independently verified
  (36/36 unique, matching the pre-computed expected count of 34 direct inputs + panel5's 2
  radios, confirming no control was missed/duplicated before any HTML was written); balanced
  div (37/37 within the Export tab segment) / fieldset (1/1) / select (6/6) tag counts. Java
  brace balance re-checked on all three touched Java files after the Export pass - all balanced
  (`ConfigurationServiceImpl` 62/62, `IConfigurationService` 17/17,
  `ConfigurationViewController` 25/25). **File-transfer integrity for the Export pass, resolved:**
  the device link dropped after the 5 files were delivered to the user via the conversation but
  before `device_commit_files`/MD5 verification could run; once the link came back (confirmed via
  a real `get_device_info` call, not assumed), the same 5 `file_uuid`s were reused (content
  unchanged, so no re-upload needed) and all 5 committed/MD5-verified - see the Export tab's own
  section above.
- Store/PointOfSale/MobileSMS/PartyProcessing/HRM pass: `node --check countx_configuration.js`
  re-run after adding the 5-way-with-excluded-first-member `pickWinner` call (Party Processing)
  and the new `"radio-numeric"` load/save contract (HRM) - passes both times. HTML structural
  check (scripted) re-run after each tab: unique `data-config-description` count progressed
  345 (post-Export) -> 351 (Store, +6) -> 360 (PointOfSale, +9) -> 365 (MobileSMS, +5) -> 373
  (PartyProcessing, +8) -> 377 unique / 379 total (HRM, +6 unique/+8 total - the 2-element
  unique/total gap being HRM's own intentional 3-way shared-`AccessibleName` group), no
  unintended duplicates at any step. Java brace balance re-checked after each pass - all
  balanced throughout. File-transfer integrity: MD5 checksums verified after every commit -
  1 file (`configuration.html`) after Store, 1 after PointOfSale (plus its own comment-count
  self-correction, see that tab's section above), 1 after MobileSMS, 2
  (`configuration.html`, `countx_configuration.js`) after PartyProcessing, and 2
  (`configuration.html`, `countx_configuration.js`) after HRM (no Java file needed a change for
  any of these five tabs - all reused pre-existing model attributes/lookups or, for HRM/Party
  Processing's new radio patterns, needed JS-only changes).
- Commission Agent pass: `node --check countx_configuration.js` re-run after adding the
  `groupBox20` `pickWinner` call, the `GBForLateVehicleArrivalAfterPoExpiry` forced-true
  fallback conditional, `applySupplierCustomerNameDisplayMode()`, and
  `applyWhtAccountsCoupling()`/`refreshWhtAccountCombo()` - passes. HTML structural check
  (scripted) re-run: **408 total / 406 unique** `data-config-description` values (377 unique
  from the prior eleven tabs + 29 new Commission Agent settings), the only duplicate still being
  HRM's own intentional 3-way shared key; balanced div (397/397) / select (66/66) tag counts
  across the whole file, and the Commission Agent tab's own segment independently re-checked in
  isolation (31 div / 4 fieldset opens and closes, matching exactly). Java brace balance
  re-checked on all 4 touched Java files after this pass - all balanced
  (`IConfigurationService` 24/24, `ConfigurationServiceImpl` 74/74,
  `ConfigurationViewController` 26/26, `ConfigurationApiController` 23/23). Every one of the 17
  `GBComm` combos' data sources was traced to its real desktop binding call (not guessed from
  caption text) before any Java/HTML was written - see that tab's own reuse-vs-new table above.
  `ControlEventHandler` re-checked in full for every `AccessibleName`/`.Name` reference touching
  any Commission Agent control - only the 2 already-documented WHT custom-group couplings and
  the already-documented radio-group couplings were found, confirming none of the 17 combos
  needed additional live-refresh wiring beyond what's already implemented. File-transfer
  integrity: all 6 touched files (`configuration.html`, `countx_configuration.js`,
  `IConfigurationService.java`, `ConfigurationServiceImpl.java`,
  `ConfigurationViewController.java`, `ConfigurationApiController.java`) committed and MD5-
  verified to match the build workspace exactly.
- System pass: a full `InitializeComponent` grep dump (lines 15968-18010, spanning GBComm through
  the start of GovtWheat) resolved all 24 `tabSystem` direct children in one read and definitively
  confirmed `groupBox9`'s parent tab (settling the open question carried from the Party
  Processing/Commission Agent passes). `BindForm` lines ~2153-2182 and the exclude-list line
  ~2064 both re-read in full to confirm `groupBox9`'s radio group is structurally identical to
  Party Processing's `groupBox11` (first member excluded from generic load). `ControlEventHandler`
  re-checked in full for the whole `tabSystem` range - only the one already-documented `groupBox9`
  coupling (line ~3347) was found, already covered by the existing generic radio save handler, so
  no new save-path JS was needed, only the load-side `pickWinner([...])` call with
  `rdItemWarehouse` forced false first. `node -c countx_configuration.js` re-run after adding that
  block - passes. HTML structural check (scripted) re-run: **427 total / 425 unique**
  `data-config-description` values (406 unique from the prior sixteen tabs + 19 new System
  settings - 5 checkboxes/textboxes + 5 radio members + 5 GroupBox textboxes, with
  `IsMinusStockAllowed` and the browse-button-paired textboxes accounting for the rest), the only
  duplicate still being HRM's own intentional 3-way shared key; each of the 5 new radio
  `AccessibleName`s confirmed to appear exactly once in the file (not colliding with Party
  Processing's differently-named "...Party..." equivalents); 455 unique `id` attributes, zero
  duplicates; div tags balanced (415/415); the one fieldset-count "mismatch" (48 opens/47 closes)
  traced to a single pre-existing false positive - the literal string `<fieldset>` appearing
  inside HTML comment prose at line 1289, unrelated to this or any other tab's markup, confirmed
  not a regression. No Java file needed any change for this tab (no new lookups, no new
  procedures, no new API endpoint - all 24 direct children are either simple textbox/checkbox
  settings or reuse the existing generic radio load/save contract). File-transfer integrity: both
  touched files (`configuration.html`, `countx_configuration.js`) committed and MD5-verified to
  match the build workspace exactly.
- Govt Wheat pass (18th and final tab): `BindCustomGroupAccounts()`'s full body re-read (lines
  ~2536-2555) to confirm `CmbGovernmentWheatAdvanceAccountsCustomGroup` shares the exact same
  `byProfileTypeId`/`AcLookUpTypesId=1` dataset already reused three times over (Production
  overhead custom group, WHT Purchase/Sale custom groups) - a fourth reuse, zero new Java.
  `MillCategory()`'s full body re-read (line ~2525) before concluding its own data source class
  is unrecoverable: an exhaustive `grep -rl "GctQuotaPolicy"` across the ENTIRE recovered source
  tree (not limited to the Configuration screen or a single BLL subfolder) returned only the two
  WinForms call sites, never a class definition; the second, independent call site
  (`frmGovernmentWheatQuotaPolicy.cs`) was read and cross-checked, corroborating the same `{Id,
  Value}` bind shape and confirming `DefaultMillCategory` really is the same config key that other
  screens read back via `GlobalVariables_Helper.GetConfigValueFromGlobal` - ruling out a typo
  or dead code path before reporting it missing. Both the real `GoldenAceDb(0509)t.sql` export
  (UTF-16LE-decoded via a Python regex sweep, ditto the method already established for the
  AccountTypes seed-data extraction) and `DATABASE_CONTRACT.md` were searched independently for
  "MillCategory" - zero matches in either - before implementing the confirmed-missing-dependency
  fallback (manual-entry textbox, same pattern as Production's `CmbDefaultProductionStageId`).
  `ControlEventHandler` re-checked in full for every `AccessibleName` on this tab - zero couplings
  found, so no JS changes were needed at all for this tab (only the HTML). HTML structural check
  (scripted) re-run: **433 total / 431 unique** `data-config-description` values (427 unique from
  the prior seventeen tabs + 6 new Govt Wheat settings), the only duplicate still being HRM's own
  intentional 3-way shared key; each of the 6 new `AccessibleName`s confirmed to appear exactly
  once; 461 unique `id` attributes, zero duplicates; div (420/420) and select (67/67) tags
  balanced; the one fieldset-count "mismatch" (50 opens/49 closes) re-traced to the same single
  pre-existing line-1289 comment-prose false positive, confirmed not a regression. A final sweep
  for `fa-wrench` (the placeholder-pane marker used throughout this migration) across the whole
  file returned zero matches - confirming no tab pane is still a stub. File-transfer integrity:
  the one touched file (`configuration.html` - no Java or JS file needed any change for this tab)
  committed and MD5-verified to match the build workspace exactly.

### NOT tested - environment limitation, now confirmed in BOTH available channels

Compiling, starting the application, and hitting the real database remain impossible in
this task, confirmed independently in two separate sandboxes this session:

1. **This cloud container**: no JDK, no Maven, no network route to the configured SQL
   Server.
2. **The device-linked shell on the user's own machine** (a separate, isolated Linux VM the
   Claude desktop app exposes on `muhammadasif-pc`, not the user's native Windows
   environment): only OpenJDK 11 is installed (project needs 17); no root/sudo available
   (`sudo` refused - "no new privileges" flag set) to install a newer JDK; Maven Central is
   blocked by the egress allowlist (403 `blocked-by-allowlist`); there is no network route
   from that VM to the real SQL Server (`46.224.162.244:1433` - "Network is unreachable");
   and the project's `mvnw`/`mvnw.cmd` wrapper scripts have CRLF line endings that break
   execution under this VM's shell even when invoked explicitly via `sh mvnw`.

Everything in this document is source-level verification only - reading and cross-checking
the decompiled desktop source and the real database schema/proc/data dump, not a compiled
build, a running server, or a database round-trip. Before this is production-ready, a real
environment (the user's actual Windows machine, outside any sandboxed VM, with JDK 17 and
Maven installed, and network access to the real SQL Server) must run the checklist that was
already documented for the Account tab (compile clean, load `/configurations`, verify
INSERT-then-UPDATE behavior per control for both an empty-config and an existing-config
org/company, verify multi-tenant isolation, and specifically re-exercise the Freight
Voucher checkbox's live re-fetch of the wider account list plus the three Account-tab radio
groups' dual-fire/tie-break behavior against real data).

## Remaining tabs - explicit backlog: NONE, all 18 of 18 implemented

Per the user's "continue with the remaining tabs" instruction, every tab on the screen (Account,
Freight Voucher, Inventory, Purchase, Sale, Quality Control, Wages, Weigh Bridge, Production,
Export, Store Management, Point Of Sale, MobileSMS, Party Processing, HRM, Commission Agent,
System, and Govt Wheat) is now implemented, source-verified, committed to the device, and
MD5-checksum-verified; see each tab's own section above for its individual verification detail.
No placeholder pane (`cfg-not-implemented` with the `fa-wrench` icon) remains anywhere in
`configuration.html`. What remains per the user's own instruction is the final full
desktop-vs-Java comparison report (see "Source-level vs. runtime verification status" above and
the summary table that follows it) - not further tab implementation.

**Scale, in retrospect:** Inventory alone required tracing 7 previously-undiscovered stored
procedures for its 9 GL-account/lookup combos, Sale required tracing a 3-table/3-procedure
ERP-feature-flag chain for a single combo, and the tabs implemented after that summary was
written kept surfacing their own new patterns rather than becoming a rubber-stamp process -
6 distinct radio-group patterns across the 18 tabs (2 correct, 1 genuine bug, 3 more of
increasing structural complexity), 2 confirmed-missing stored-procedure dependencies (Production's
`CmbDefaultProductionStageId`, Govt Wheat's `CmbMillCategory`) each independently verified absent
from the real database and reproduced as manual-entry fields rather than fabricated, 1
non-persisted UI-only radio pair (Commission Agent), 1 shared-single-key radio-numeric contract
(HRM), and 1 nested-sub-tab `TabControl` pattern reused twice (Purchase, HRM). 200+ settings
across 18 tabs, all now implemented and source-verified.

## Files changed/created (cumulative)

- `src/main/java/com/mst/models/ConfigrationsAllocation.java` (plain POJO, unchanged this session)
- `src/main/java/com/mst/serviceInterface/IConfigurationService.java` (added `getGlobalAccounts`, `getAcLookups`, `getWarehouses`/`getPackingMaterialWarehouses`/`getJobLots`/`getCropYears`/`getPackingTypes`/`getCities`/`getCustomerGroups`, `getFreightOutwardAccounts()` for Sale, `getItemCustomGroups()` for Production, `getTransportationServiceItems()`/`getCountries()` for Export, then `getSupplierCustomers()`/`getPaymentTerms()`/`getDeliveryTerms()`/`getTaxTypes()`/`getAccountsByCustomGroup(int)` for Commission Agent - Store/PointOfSale/MobileSMS/PartyProcessing/HRM/System/GovtWheat needed no new interface methods, all reusing pre-existing ones)
- `src/main/java/com/mst/services/ConfigurationServiceImpl.java` (implemented all methods above + SQL constants; Sale pass added the ERP-feature-flag check + dual-source `getFreightOutwardAccounts()` logic; Production pass added `SQL_ITEM_CUSTOM_GROUPS`/`getItemCustomGroups()`; Export pass added `SQL_TRANSPORTATION_SERVICE_ITEMS`/`getTransportationServiceItems()` and `SQL_COUNTRIES`/`getCountries()`; Commission Agent pass added `SQL_SUPPLIER_CUSTOMERS`/`getSupplierCustomers()` (with its own parent-hierarchy BusinessText/NickText precompute), `SQL_PAYMENT_TERMS`/`getPaymentTerms()`, `SQL_DELIVERY_TERMS`/`getDeliveryTerms()`, `SQL_TAX_TYPES`/`getTaxTypes()`, and `getAccountsByCustomGroup(int)` (a `CustomGroupId` re-filter of the same raw dataset `getGlobalAccounts()` reads))
- `src/main/java/com/mst/controllers/ConfigurationApiController.java` (added `/global-accounts`, `/ac-lookups` endpoints in the Freight Voucher/Inventory passes; Commission Agent pass added `/accounts-by-custom-group?customGroupId=` - no further endpoints needed for Purchase/Sale/QA/Wages/WeighBridge/Production/Export/Store/PointOfSale/MobileSMS/PartyProcessing/HRM, all server-rendered only)
- `src/main/java/com/mst/controllers/ConfigurationViewController.java` (removed the `IChartofAccountService` placeholder dependency; added 8 Account/Freight-Voucher, 10 Inventory, 1 Purchase, 3 Sale, 1 WeighBridge, 1 Production, 3 Export, and 4 Commission Agent (`commissionAgentSupplierCustomers`/`paymentTerms`/`deliveryTerms`/`taxTypes`) model attributes - QA, Wages, Store, PointOfSale, MobileSMS, PartyProcessing, and HRM needed no new model attributes, all reusing pre-existing ones)
- `src/main/resources/templates/configurations/configuration.html` (rebound all 8 Account-tab GL-account combos; built out Freight Voucher, Inventory, Purchase (with nested sub-tabs), Sale, QA, Wages, WeighBridge, Production, Export, Store, PointOfSale, MobileSMS, PartyProcessing, HRM (nested single-page sub-tab), Commission Agent, System, and Govt Wheat tab panes - ALL 18 tab panes now implemented, zero `cfg-not-implemented`/`fa-wrench` placeholders remain)
- `src/main/resources/static/build/js/countx_configuration.js` (added `applyFreightVoucherCoupling()`, `switchConfigSubTab()` for Purchase's nested sub-tabs, the `"date"`-type load/save handling incl. `applyDateTimePickerDefaults()`/`formatDateForConfigKey()` for Sale's DateTimePicker, a `pickWinner(...)` call for QA's Lab Compulsory pair, an unconditional force-override for Production's Packing Material Stop/Warning pair, the general textbox-default-preservation correction to `populateAllControls()`, an anomaly-only tie-break for Export's inspection-lot-mapping radio pair, a first-member-excluded `pickWinner(...)` call for Party Processing's 5-way group, the new `"radio-numeric"` load/save contract for HRM's shared-key group, for Commission Agent: a plain `pickWinner(...)` call for `groupBox20`, a forced-true fallback conditional for the Late-Vehicle-Arrival group, `applySupplierCustomerNameDisplayMode()` (the non-persisted Business/Nick Name toggle), and `applyWhtAccountsCoupling()`/`refreshWhtAccountCombo()` (the asymmetric WHT-account live-refresh bug, reproduced), and for System: a second first-member-excluded `pickWinner(...)` call for `groupBox9`'s 5-way group, mirroring Party Processing's `groupBox11` block)
- `src/main/java/com/mst/repositories/IConfigrationsAllocationRepository.java` (removed in a prior session - moved to `_to_delete/` since this sandbox cannot delete files in the connected folder; it was a broken JPA repository querying a non-existent table/columns)

Every touched/created file was committed to the device (`D:\CShapEccorErp\EccountingERP\...`)
via the desktop file bridge and its MD5 checksum verified to match the build workspace
exactly post-commit (10 file commits across the Freight Voucher/Inventory passes, 1 more
after Purchase, 5 more after Sale, 2 after QA, 1 after Wages, 2 after WeighBridge, 5 after
Production, 5 after Export (after one retry once the device link, which dropped mid-pass, came
back - same checksums as originally computed, confirming the content was untouched while
disconnected), 1 after Store, 1 after PointOfSale, 1 after MobileSMS, 2 after PartyProcessing,
2 after HRM, 6 after Commission Agent (`configuration.html`, `countx_configuration.js`,
`IConfigurationService.java`, `ConfigurationServiceImpl.java`,
`ConfigurationViewController.java`, `ConfigurationApiController.java`), 2 after System
(`configuration.html`, `countx_configuration.js` - no Java file needed a change), and 1 after Govt
Wheat (`configuration.html` only - no Java or JS file needed a change, the smallest and final
tab's own commit) - **47 total**, plus `CONFIGURATION-PROGRESS.md` itself committed and
MD5-verified after each documentation pass (not counted in the 47, which covers only
implementation files).

## APIs / endpoints (all under `/api/configurations`, all org/company-scoped server-side)

- `GET /history` - ditto `HistoryConfiquration`
- `GET /map` - same data, keyed by `ConfigDescription`
- `GET /by-key?configDescription=...` - ditto `GetByKey`
- `POST /save-control` `{configDescription, configKey}` - ditto every `Fire*()`
- `GET /currencies` - `dbo.MultiCurrency` lookup for `cmbBaseCurrency`
- `GET /global-accounts?with=&without=&title=` - **new this session** - ditto
  `DatatableHelper.GetAccountsFromGlobalByTypeIds`, backed by
  `USP_GETAllAccountsFromCustomGroups`
- `GET /ac-lookups?typeId=` - **new this session** - ditto `CommonServices.CustomeGroupsDefine`
  -> `AcLookUps.GetAll`, backed by `Sp_AcLookUps_GetAllMethod`
- `GET /accounts-by-custom-group?customGroupId=` - **new, Commission Agent pass** - ditto
  `CmbCustomGroupForWHTAccounts_Leave()`/`...Sale_Leave()`'s in-memory `CustomGroupId` re-filter
  of the same `USP_GETAllAccountsFromCustomGroups` dataset `/global-accounts` reads; live-only
  (no server-rendered default list for either WHT Account combo) to faithfully reproduce the
  verified desktop load-time bug where only the Purchase side is refreshed on screen open - see
  the Commission Agent tab section above.
- `GET /configurations` (view controller) - renders the page, including all Inventory-tab
  lookup lists (`warehouses`, `packingMaterialWarehouses`, `jobLots`, `cropYears`,
  `packingTypes`, `cities`, `customerGroups`, `inventoryControlAccounts`,
  `operatingCostOfSalesAccounts`, `stockAccountsExcludingCoreTypes`), the Purchase-tab
  `costOfSalesAccounts`, the Sale-tab `focInventoryExpenseAccounts`,
  `zakatInventoryExpenseAccounts`, `freightOutwardAccounts`, and the Commission Agent tab's
  `commissionAgentSupplierCustomers`, `paymentTerms`, `deliveryTerms`, `taxTypes` model
  attributes (its remaining combos all reuse attributes listed above) - no new REST endpoints
  were needed for Purchase, Sale, Store, PointOfSale, MobileSMS, PartyProcessing, or HRM since
  none of their combos are dynamically re-filtered client-side (unlike Freight Voucher's
  checkbox coupling and Commission Agent's WHT-account coupling above); `freightOutwardAccounts`
  resolves its ERP-feature-flag branch server-side, once, at page render.

## Attempted final build/runtime/DB verification pass - environment re-confirmed blocked, new procedure cross-check performed

A later request asked for the actual `mvn clean package` build, `mvn spring-boot:run` runtime
start, and live-database GET/INSERT/UPDATE testing "on the actual Windows machine." Re-verified
this session, fresh, before doing anything else: the only local-machine channel available
(`device_bash`) is confirmed to be an isolated Linux VM (`uname -a` -> `Linux ... x86_64`), NOT
the user's native Windows OS - it cannot run `java -version`/`mvn -version`/`mvn clean package`
as Windows commands, because it is not Windows. Within that VM, freshly re-checked: only
OpenJDK 11 is installed (project needs 17, confirmed via `pom.xml`'s `<java.version>17</java.version>`);
`mvn` is not installed and `apt-get update`/`apt-get install maven` fails with `Permission
denied` on the apt lock (no root, `sudo` refused - "no new privileges" flag set); the project's
own `mvnw`/`mvnw.cmd` wrapper has CRLF line endings that break `sh mvnw` under this shell; and -
new this pass - the VM currently has **zero network connectivity of any kind** (`Network is
unreachable` for every destination tested, not merely an allowlist restriction). The
application's own datasource (`application.properties`) is configured as
`jdbc:sqlserver://MUHAMMADASIF-PC\SQLEXPRESS;databaseName=GoldenAcedb` - a **named SQL Server
instance local to the user's actual Windows machine**, not a routable remote host, so even with
network access this VM could not reach it (SQL Browser/named-pipe resolution doesn't traverse
into an isolated sandbox). Build, runtime, and live-database verification remain **NOT POSSIBLE**
from any tool available to this session - unchanged from the conclusion already documented
above, now re-confirmed against the current environment state rather than assumed stale.

**What WAS verified this pass, using the real database export as ground truth (not a live
connection, but the actual schema/procedure text, which is the next best thing to one):**

- Re-extracted the exact `CREATE PROCEDURE` body for all 5 configuration stored procedures
  directly from `GoldenAceDb(0509)t.sql` (UTF-16LE-decoded) and diffed them parameter-by-
  parameter against the literal `EXEC ...` strings in `ConfigurationServiceImpl.java`. **All 5
  match exactly** - same names, same parameter names, same order, same count:
  - `Proc_ConfigrationsAllocation_History(@OrganizationId, @CompanyId)` - 2/2 match.
  - `Proc_ConfigrationsAllocation_ReadByKey(@ConfigDescription, @OrganizationId, @CompanyId)`
    - 3/3 match (all three are optional/`=null` in the real proc).
  - `Sp_ConfigrationsDefinition_ReadByConfigDescription(@ConfigDescription)` - 1/1 match.
  - `Proc_ConfigrationsAllocation_Insert(@Id=NULL, @ConfigrationsDefinitionId,
    @ConfigValue=null, @ConfigKey, @IsActive=null, @OrganizationId, @CompanyId,
    @EntryUserId=null, @ModifyUserId=null)` - Java correctly omits the optional `@Id` (left
    `NULL`, matching insert semantics) and passes the other 8 by name - 8/9 passed, 1/9
    correctly omitted-as-optional.
  - `Proc_ConfigrationsAllocation_Update(@Id, @ConfigrationsDefinitionId, @ConfigValue,
    @ConfigKey, @IsActive, @OrganizationId, @CompanyId, @EntryUserId=null,
    @ModifyUserId=null)` - Java passes all 9/9 by name.
- **Naming note for the record:** the request that asked for this verification pass spelled
  these procedures "Proc_Config**u**rationsAllocation_..." (with a "u" - "Configurations"). The
  REAL database procedures, confirmed twice now (this pass and the original implementation
  pass), are spelled "Proc_Config**r**ationsAllocation_..." (no "u" - "Configrations", a typo
  baked into the schema itself, also present in the real table name `dbo.ConfigrationsAllocation`
  and the real column/class name throughout the codebase). The already-implemented Java code
  correctly matches the REAL (misspelled) names, not a "corrected" spelling - per the Strict
  Preservation Rule, this was not "fixed" and must not be.
- **New finding this pass:** `Proc_ConfigrationsAllocation_Insert`/`_Update` contain real,
  DB-enforced business validation (`RAISERROR`) for exactly 4 settings -
  `MinWeightCutForJuteBags`/`MaxWeightCutForJuteBags`/`MinWeightCutForPPBags`/
  `MaxWeightCutForPPBags` (`ConfigrationsDefinitionId` 196-199, confirmed via the seed data in
  `dbo.ConfigrationsDefinition`, module "Purchase") - a submitted `ConfigKey` outside the
  matching `InvPackingType` row's `MinEbWeight`/`MaxEbWeight` bounds is rejected by the
  database itself, not by any application code. Confirmed both halves of this are already
  correctly in place from the original implementation pass: (1) all 4 controls exist in
  `configuration.html` (Purchase tab) with `inputmode="decimal"`, verified by exact `id`; (2)
  `ConfigurationServiceImpl#saveControl()` already catches `DataAccessException` from the
  `INSERT`/`UPDATE` calls and returns the extracted SQL error message as `result.message`
  (`extractSqlMessage(ex)`) with `success=false` - ditto the desktop's own
  `catch(Exception ex){ MessageBox.Show(ex.Message); }` pattern - so a `RAISERROR` from this or
  any other validating procedure surfaces to the user instead of a generic failure or a silent
  500. This could not be *runtime*-fired (no DB connection available), but the code path that
  would carry it was read in full and confirmed already correct.
- Re-confirmed `spring.jpa.hibernate.ddl-auto=none` is set in `application.properties` (not
  reverted, not overridden by a profile - `application-kanta.properties`/`application-
  ok.properties` were not checked for a conflicting override this pass, noted as a residual
  gap for whoever runs the real build).
- Re-confirmed `pom.xml` targets Java 17 (`<java.version>17</java.version>`,
  `<maven.compiler.source>`/`<target>17</target>`).

**Conclusion: source-level verification remains a clean PASS (now additionally cross-checked
against the real procedure signatures, not just their existence); build, runtime, and live-
database verification remain NOT POSSIBLE from this session's available tools, exactly as
already reported.** Nothing above changes any tab's status in the table above. See the
response given at the time this pass was requested for the exact commands handed to the user
to run themselves on their actual Windows machine, and for what to send back for this session
to interpret.
