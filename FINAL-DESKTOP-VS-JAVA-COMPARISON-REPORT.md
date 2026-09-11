# Configuration Screen: Desktop-vs-Java Final Comparison Report

**Scope:** `Architecture.WinApp.Configurations.Configuration.cs` (18,033 lines, 18 tabs,
200+ settings) migrated to the Java Spring Boot / Thymeleaf page at
`src/main/resources/templates/configurations/configuration.html`, its controller/service
layer under `com.mst.*`, and `src/main/resources/static/build/js/countx_configuration.js`.

**Status: all 18 of 18 tabs implemented and source-verified.** This report consolidates the
per-tab findings recorded progressively in `CONFIGURATION-PROGRESS.md` (the full, line-by-line
verification log this report summarizes - consult it for exact source line numbers, full
method bodies, and the complete reasoning behind every finding below) into one final
desktop-vs-Java comparison, per the governing instruction: verify every tab against
`Configuration.cs` line by line (controls, database keys, dropdowns, events, validation, GET/
INSERT/UPDATE behavior), report source-level and runtime verification separately, and list
every unresolved issue rather than marking the task silently "done."

---

## 1. Database preservation - compliance summary

**No table, column, stored procedure, or existing data was created, renamed, or altered.**
Every object this migration reads or writes was independently confirmed to exist, with its
real name and shape, against `GoldenAceDb(0509)t.sql` (the real SQL Server schema/procedure/
seed-data export) and, where applicable, `DATABASE_CONTRACT.md`. The two confirmed-missing
dependencies below are the exception that proves the rule: rather than invent a plausible
substitute, both were reported and implemented as manual-entry fallbacks preserving the same
`AccessibleName`/`ConfigKey`, per the user's explicit "stop-and-report if something required
is missing rather than inventing it" instruction.

All persisted settings on every tab flow through the same four real, verified objects:

| Object | Role |
|---|---|
| `dbo.ConfigrationsAllocation` | The one table every setting's value (`ConfigKey`) lives in, keyed by `ConfigDescription` (= each control's `AccessibleName`) + `OrganizationId` + `CompanyId` |
| `dbo.ConfigrationsDefinition` | Resolves `ConfigrationsDefinitionId` for a first-time INSERT |
| `Proc_ConfigrationsAllocation_History` / `Proc_ConfigrationsAllocation_ReadByKey` | GET-all / GET-by-key |
| `Proc_ConfigrationsAllocation_Insert` / `Proc_ConfigrationsAllocation_Update` | Save (never both - insert-then-update-forever, ditto the desktop) |

`spring.jpa.hibernate.ddl-auto=none` is untouched throughout. No Hibernate/JPA entity backs
`ConfigrationsAllocation` (a plain POJO) - every read/write, on every tab, goes through
`JdbcTemplate` calling the exact stored procedures identified below with
`EXEC ... @Param=?` and positional JDBC parameters, matching this project's established
convention.

Note: the user's originally-assumed names (`tbl_ConfigrationsAllocation`,
`tbl_ConfigrationsDefinition`, `Proc_ConfigrationsDefinition_ReadByDesc`) do **not** exist in
the real database. Per the user's own override instruction, the real names above were used
instead - correcting a pre-existing defect in this repo's Java code (wrong table name,
non-existent columns), not introducing one.

### New real, verified database objects this migration reads (cumulative across all 18 tabs)

| Object | Type | Backs |
|---|---|---|
| `dbo.USP_GETAllAccountsFromCustomGroups` | Stored proc | All 8 Account-tab GL-account combos, plus every other tab's GL-account combo (Freight Voucher, Inventory, Purchase, Sale, Production, Commission Agent's stock-account combos and WHT-account live-refresh, Govt Wheat's custom-group combo) |
| `dbo.Sp_AcLookUps_GetAllMethod` | Stored proc | Every "Custom Group"-style combo (`AcLookUpTypesId=1`) reused across Production, Commission Agent, Govt Wheat |
| `dbo.AccountTypes` | Table (22-row seed lookup) | Gives meaning to every `AccountTypeId` filter above - never used as a magic number without this table backing it |
| `dbo.AcLookUps` | Table | Backs `Sp_AcLookUps_GetAllMethod` |
| `dbo.MultiCurrency` | Table | `cmbBaseCurrency` |
| `dbo.USP_GetWarehousesAllocatedToBranch` / `dbo.Sp_InvWareHouse_GetAllMethod` | Stored procs | Inventory-tab warehouse combos |
| `dbo.SP_JobLot_ReadMethod` | Stored proc | Inventory-tab job-lot combos |
| `dbo.Sp_InvCropYear_GetAllMethod` | Stored proc | Crop-year combos (Inventory, Commission Agent, Govt Wheat) |
| `dbo.Sp_InvPackingType_GetAllMethod` | Stored proc | Packing-type combos (Inventory, Commission Agent) |
| `dbo.USP_City_GetAllWithCountryAndTehsil` | Stored proc | City combos (Inventory, Commission Agent) |
| `dbo.Sp_CustomerGroup_GetAllMethod` | Stored proc | Customer-group combos |
| `dbo.USP_GetERPFeaturesByCompanyId` / `dbo.ERPConfigurations` / `dbo.ERPFeatures` | Stored proc + 2 tables | Sale tab's ERP-feature-flag-gated dual-source Freight Outward Account combo |
| `dbo.USP_GetVendorsAndCustomersForTransporter` | Stored proc | Freight Voucher transporter combo |
| `dbo.Sp_COAAllocation_GetAllMethod` / `dbo.COAAllocation` | Stored proc + table | Account-tab allocation combo |
| `dbo.USP_ItemCustomGroup_GetAllMethod` / `dbo.ItemCustomGroup` | Stored proc + table | Production tab's item custom-group combo |
| `[lgstcm].[USP_Item_AllServiesItems]` | Stored proc | Export tab's service-item combo |
| `dbo.SP_Country_ReadMethod` | Stored proc | Export tab's country combo |
| `dbo.USP_GetVendorsAndCustomersWithCityName` | Stored proc | Commission Agent's 3 supplier/customer combos |
| `dbo.Sp_InvDueTerms_GetAllMethod` | Stored proc | Commission Agent's payment-term combo |
| `[dbo].[USP_DeliveryTerm_GetAllMethod]` | Stored proc | Commission Agent's delivery-term combo |
| `dbo.Sp_TaxesTypes_GetAllMethod` | Stored proc | Commission Agent's 2 tax-type combos |

### Confirmed-missing dependencies (reported, not fabricated) - the two unresolved data-source gaps on this screen

| Control | Desktop's own (unreachable) data source | Verification performed | Fallback implemented |
|---|---|---|---|
| Production tab's `CmbDefaultProductionStageId` ("Default Production Stage") | `Architecture.BLL.Mfg.LookUps.GetDataByTypeId(3)` -> `[Mfg].[USP_LookUps_GetAllMethod]` (`@Id=3, @Activity='GetDataByTypeId'`) | Exhaustive regex search of the full `GoldenAceDb(0509)t.sql` export (`\[Mfg\]\.\[USP_LookUps_\w*\]`, 4 matches - all `Insert`/`Update`) plus cross-check against `DATABASE_CONTRACT.md`. Procedure confirmed absent from the real database. | Manual-entry text field bound to the same `AccessibleName`/`ConfigKey` (`DefaultProductionStageId`) - any value already saved is shown/editable, GET/INSERT/UPDATE otherwise identical to every other textbox |
| Govt Wheat tab's `CmbMillCategory` ("Mill Category") | `Architecture.BLL.Inventory.GCT.GctQuotaPolicy.GetMillCategory()` | Exhaustive `grep -rl "GctQuotaPolicy"` across the ENTIRE recovered/decompiled source tree (every recovered BLL/DAL/Model project folder, not just the Configuration screen) - no class definition found anywhere, only 2 WinForms call sites. Independently cross-checked "MillCategory" against both the UTF-16LE-decoded SQL dump and `DATABASE_CONTRACT.md` - zero matches in either. | Manual-entry text field bound to the same `AccessibleName`/`ConfigKey` (`DefaultMillCategory`) - identical fallback pattern |

Both would throw a runtime error on the real desktop if their code path were ever exercised
against this exact database - this migration does not reproduce a crash, it preserves the
underlying setting as an editable value.

### Other documented (non-blocking) source-fidelity notes

- `cmbBaseCurrency` (Account tab) is bound to the existing, already-verified
  `dbo.MultiCurrency` query, but the desktop's own exact binding call for this one combo was
  not independently re-traced this session (an inherited, not newly-introduced, gap - `MultiCurrency`
  is a confirmed real table, so this is a verified-schema read, just not proven to be the
  *exact* same call path the desktop uses).
- `Sp_ConfigrationsAllocation_GetAllMethod` is a real stored procedure but is **not** used by
  `Configuration.cs`'s own load/save flow (it backs an unrelated BLL method used by other
  screens) - correctly left unwired here, out of scope for this screen.
- Export tab's `dbo.SP_Country_ReadMethod`: its `'GetAll'` branch has no `WHERE` clause (the
  `@OrganizationId`/`@CompanyId` parameters it accepts are effectively ignored by that branch)
  - a real, verified desktop query-scope characteristic, preserved as-is rather than narrowed.

---

## 2. Source-level vs. runtime verification status - per tab

Per the user's explicit instruction, no tab is marked "complete" on source-level verification
alone; the two results are tracked and reported separately.

| # | Tab | Source-level verification | Runtime verification |
|---|---|---|---|
| 1 | Account | **PASS** - 53/57 controls (4 hidden, excluded); GL-account combo data sources traced to real desktop binding calls and corrected from an earlier placeholder | **NOT POSSIBLE** |
| 2 | Freight Voucher | **PASS** - 5/5 controls, 1 live-refresh coupling verified | **NOT POSSIBLE** |
| 3 | Inventory | **PASS** - 33/34 controls (1 hidden, excluded); 7 stored procedures independently verified against the SQL dump's own `CREATE PROCEDURE` bodies | **NOT POSSIBLE** |
| 4 | Purchase | **PASS** - 59/65 controls (6 hidden, excluded); nested-sub-tab structure verified; dense-cluster control/label pairings hand-corrected | **NOT POSSIBLE** |
| 5 | Sale | **PASS** - 51/55 controls (4 hidden, excluded); ERP-feature-flag dual-source chain fully traced; screen's only `DateTimePicker` load-bug verified and preserved (see §3) | **NOT POSSIBLE** |
| 6 | Quality Control (QA) | **PASS** - 14/14 controls; correct symmetric radio negation verified and preserved | **NOT POSSIBLE** |
| 7 | Wages | **PASS** - 37/38 controls; 1 out-of-scope document-approval sub-feature identified and excluded, not silently dropped (see §3) | **NOT POSSIBLE** |
| 8 | Weigh Bridge | **PASS** - 26/26 controls; 1 new GL-account type filter verified; browse-button correctly excluded | **NOT POSSIBLE** |
| 9 | Production | **PASS** - 31/33 controls (2 hidden, excluded); 1 asymmetric radio-pair bug verified and preserved (see §3); 1 confirmed-missing dependency reported (see §1) | **NOT POSSIBLE** |
| 10 | Export | **PASS** - 36/36 controls (0 hidden); 2 stored procedures traced (1 with a verified query-scope characteristic preserved as-is, see §1); a correct (non-bug) radio pattern preserved | **NOT POSSIBLE** |
| 11 | Store Management | **PASS** - 6/6 controls, no couplings, no new lookups | **NOT POSSIBLE** |
| 12 | Point Of Sale | **PASS** - 9/9 controls, all 7 combos reuse pre-existing model attributes | **NOT POSSIBLE** |
| 13 | MobileSMS | **PASS** - 5/5 controls, no couplings, no new lookups | **NOT POSSIBLE** |
| 14 | Party Processing | **PASS** - 8/8 controls; a 5-way priority-chain/first-member-excluded radio pattern verified and preserved | **NOT POSSIBLE** |
| 15 | HRM | **PASS** - 7/7 controls; nested single-page `TabControl` resolved; a shared-single-key numeric radio group verified and preserved | **NOT POSSIBLE** |
| 16 | Commission Agent | **PASS** - 29/29 persisted settings (6 direct + 2 radio groups + 17 combos) + 2 correctly-excluded non-persisted UI-only radios; 4 stored procedures traced; a forced-true-fallback radio pattern and an asymmetric load-time bug both verified and preserved (see §3) | **NOT POSSIBLE** |
| 17 | System | **PASS** - 22/24 controls (2 hidden, excluded); a 2nd occurrence of the 5-way priority-chain radio pattern verified and preserved; 5 browse-button-paired folder-path textboxes correctly excluding their local-only Browse buttons | **NOT POSSIBLE** |
| 18 | Govt Wheat | **PASS** - 7/7 direct controls (0 hidden), 6/6 persisted settings; caption correction ("Govt. Wheat", with a period) applied; 1 confirmed-missing dependency reported (see §1) | **NOT POSSIBLE** |

**Runtime verification is, and remains, "NOT POSSIBLE" for every one of the 18 tabs** - a
fixed environment limitation, not a per-tab gap, confirmed independently in two separate
sandboxes:

1. **This cloud container:** no JDK, no Maven, no network route to the configured SQL Server.
2. **The device-linked shell on the user's own machine** (an isolated Linux VM the Claude
   desktop app exposes, not the user's native Windows environment): only OpenJDK 11 is
   installed (the project needs 17); no root/sudo available to install a newer JDK; Maven
   Central is blocked by the egress allowlist; no network route from that VM to the real SQL
   Server; and the project's `mvnw`/`mvnw.cmd` wrapper scripts have CRLF line endings that
   break execution under this VM's shell even via `sh mvnw`.

Everything in this report and in `CONFIGURATION-PROGRESS.md` is **source-level verification
only** - reading and cross-checking the decompiled desktop source and the real database
schema/procedure/data dump, control by control, key by key. It is **not** a compiled build, a
running server, or a database round-trip. Before this is production-ready, a real environment
(the user's actual Windows machine, outside any sandboxed VM, with JDK 17 and Maven installed,
and network access to the real SQL Server) must run the checklist already documented in
`CONFIGURATION-PROGRESS.md`'s "NOT tested" section: compile clean, load `/configurations`,
verify INSERT-then-UPDATE behavior per control for both an empty-config and an existing-config
org/company, verify multi-tenant isolation, and specifically re-exercise every coupling/bug
listed in §3 below against real data.

---

## 3. Verified desktop quirks and bugs - all preserved exactly, none "fixed"

The user's instruction was to migrate the desktop screen faithfully, not to improve it. Where
the desktop itself has an inconsistency, this migration reproduces it rather than silently
correcting it. Six distinct radio-group patterns and two other load/save quirks were found and
verified across the 18 tabs:

1. **QA tab's Lab Compulsory pair** - a correct, symmetric negation (not a bug). Reproduced
   with a plain `pickWinner()` priority call.
2. **Production tab's Packing Material Stop/Warning pair - a genuine, data-losing bug.**
   `rdPackingMaterialCompulsoryOnStockConversionForWarning` is excluded from `BindForm`'s
   generic load-assignment, so the very next normalization line always takes its "else" branch
   and stomps the Stop radio back to `true` on every single load, regardless of what was
   actually saved for either radio. Net effect: Stop always wins on screen load. Reproduced as
   an unconditional force (not a priority list, which would incorrectly let a saved Warning
   win).
3. **Export tab's inspection-lot-mapping pair** - correct, with a harmless anomaly-only
   tie-break for the rare case both were somehow saved `true` simultaneously. Reproduced
   narrowly (not via `pickWinner`, which would incorrectly supply an always-checked default
   even when neither was ever saved).
4. **Party Processing's / System's 5-way priority chains** (`groupBox11` / `groupBox9`) - the
   SAME pattern occurring twice, independently verified both times: each group's first member
   is itself in `BindForm`'s generic-assignment exclude list, so it can never win on load no
   matter its own saved value. Reproduced by forcing that first member `checked=false` before
   calling `pickWinner()` on the remaining 4.
5. **HRM's shared-single-`AccessibleName` numeric group** (`DaysForAdjacentHolidayAbsenceRule`)
   - three radios that all read and write the SAME database key (not three independent
   settings) - required a new `data-config-type="radio-numeric"` load/save contract, not a
   simple `pickWinner()`.
6. **Commission Agent's `GBForLateVehicleArrivalAfterPoExpiry`** - a 6th, distinct pattern: two
   independently-persisted members (Warning/Block) plus a third member (NoValidation) that ALSO
   has its own saved row, yet is unconditionally forced `Checked=true` whenever neither of the
   other two is checked - overriding its own just-loaded value. Reproduced as one extra
   conditional after the plain per-radio load, not a `pickWinner()`.

Plus two non-radio load/save quirks:

7. **Sale tab's `txtAsOnDateForDoCompulasoryOnGDN` (the screen's only `DateTimePicker`)** -
   `BindForm()`'s control-population loop has no `DateTimePicker` branch at all (confirmed by
   scanning every `DateTimePicker` occurrence in the file). The desktop **saves** this field's
   changes but **never redisplays the saved value** - it always shows `DateTime.Now` on every
   screen open. Preserved exactly: the Java page's `"date"` control type deliberately never
   reads this field back from the saved config map, resetting to today's date on every load,
   while still saving changes normally.
8. **Commission Agent's WHT Purchase/Sale Account combos** - the desktop constructor calls
   `CmbCustomGroupForWHTAccounts_Leave(null,null)` TWICE and never calls
   `CmbCustomGroupForWHTAccountsSale_Leave()` at all. Net effect: the Purchase account combo is
   pre-filtered by whatever custom group is already saved; the Sale account combo's option list
   is always empty on open, regardless of what's saved, until the user changes the Sale custom
   group in that session. Reproduced with an asymmetric `refreshOnLoad` flag (`true` for
   Purchase, `false` for Sale) on an otherwise-shared live-refresh helper.

**Also verified and correctly excluded (not a bug, not persisted):** Commission Agent's
`RadNickName`/`RadBusinessName` pair has no `AccessibleName` anywhere in
`InitializeComponent` - the only unpersisted radio pair discovered anywhere on this 18-tab
screen. `ControlEventHandler` special-cases it by `.Name` to call a dedicated
`RadBusinessName_CheckedChanged()` handler instead of the generic `FireRadioButton()`,
confirming no save ever happens. Implemented as a pure client-side text-swap toggle with no
`data-config-description`/`data-config-type` attributes, deliberately excluded from the
generic save/load dispatch.

---

## 4. Controls verified as intentionally out of scope or hidden (not silently dropped)

- **Wages tab's `grdWagesRefDocuments` / `btnStatusUpdate`** ("Document Wise Wages
  Configuration") - a reference-documents status grid and its Status Update button, backed by
  `InvContractorWagesBillHeader`, an entirely separate document-approval workflow, not a
  `ConfigrationsAllocation` setting. Verified out of scope for this migration, documented
  rather than fabricated or silently dropped.
- **Hidden (`Visible=false`) controls, correctly excluded on every tab where found:** Account
  (4), Inventory (1), Purchase (6), Sale (4), Export (1 pair -
  `chkStockHoldForLabApprovalAutoChecked`/`label393`), Production (2), System (2 -
  `label120`/`txtModuleIconPath`, the "Modules Icon Path" setting). Every exclusion was
  confirmed via an exhaustive `Visible = false` sweep of that tab's full control range, not
  inferred from naming.
- **Browse-button controls, correctly excluded on every tab where found** (WeighBridge,
  System's 5 folder-path GroupBoxes): each opens a local-only WinForms
  `FolderBrowserDialog`/`OpenFileDialog` with no server-side counterpart, no `AccessibleName`,
  no persisted setting of its own.

---

## 5. Reusable UI/data patterns established (for reference, not gaps)

- **GL-account combos:** every GL-account-filtered combo across all 18 tabs (Account, Freight
  Voucher, Inventory, Purchase, Sale, Production, Commission Agent, Govt Wheat) is a filtered
  view of the SAME one dataset (`USP_GETAllAccountsFromCustomGroups`), filtered in-memory with
  the desktop's own include/exclude/exact-title LINQ logic reproduced 1:1 in Java rather than
  pushed into SQL (the real procedure accepts no type-filter parameters, matching the desktop).
- **"Custom Group" combos:** every one (Production, Commission Agent x2, Govt Wheat) reuses the
  same `Sp_AcLookUps_GetAllMethod` (`AcLookUpTypesId=1`) dataset and the same
  `freightCustomGroups` Java model attribute.
- **Nested sub-tab `TabControl` pattern:** built once for Purchase, reused verbatim for HRM
  (`win-sub-tabs`/`cfg-subtab-pane`/`switchConfigSubTab`).
- **Generic load/save infrastructure** (unchanged across all 18 tabs): every control carrying
  `data-config-description` (= its real `AccessibleName`) and `data-config-type` is
  automatically loaded from `GET /api/configurations/map` and automatically wired to
  `POST /api/configurations/save-control` - adding a tab's controls needed no new generic JS,
  only markup with the right attributes, except where a tab-specific coupling (a radio quirk, a
  live-refresh combo) required one, all listed in §3 above.

---

## 6. Files changed/created (cumulative, all 18 tabs)

- `src/main/java/com/mst/serviceInterface/IConfigurationService.java`
- `src/main/java/com/mst/services/ConfigurationServiceImpl.java`
- `src/main/java/com/mst/controllers/ConfigurationApiController.java`
- `src/main/java/com/mst/controllers/ConfigurationViewController.java`
- `src/main/resources/templates/configurations/configuration.html`
- `src/main/resources/static/build/js/countx_configuration.js`
- `src/main/java/com/mst/repositories/IConfigrationsAllocationRepository.java` (removed - a
  broken JPA repository querying a non-existent table/columns, moved to `_to_delete/` since
  this sandbox cannot delete files directly in the connected folder)

**47** file commits to the device (`D:\CShapEccorErp\EccountingERP\...`), each independently
MD5-checksum-verified to match the build workspace exactly post-commit. See
`CONFIGURATION-PROGRESS.md`'s "Files changed/created" section for the full per-tab commit
breakdown.

## 7. APIs / endpoints (all under `/api/configurations`, all org/company-scoped server-side)

- `GET /history` - GET-all
- `GET /map` - same data, keyed by `ConfigDescription`
- `GET /by-key?configDescription=...`
- `POST /save-control` `{configDescription, configKey}` - the one save path every control uses
- `GET /currencies` - `dbo.MultiCurrency` lookup
- `GET /global-accounts?with=&without=&title=` - the shared GL-account filter endpoint
- `GET /ac-lookups?typeId=` - the shared "Custom Group" lookup endpoint
- `GET /accounts-by-custom-group?customGroupId=` - Commission Agent's WHT-account live refresh
  (deliberately asymmetric, see §3.8)
- `GET /configurations` (view controller) - renders the page and every tab's server-rendered
  lookup lists

---

## 8. Summary

All 18 tabs of the Configuration screen are implemented, and every control, database key,
dropdown data source, event coupling, and load/save behavior has been verified line-by-line
against the decompiled desktop source and cross-checked against the real database schema.
Two data-source dependencies referenced by the desktop code do not exist anywhere in the
recovered source tree or the real database (§1) and are reported here rather than silently
worked around. Eight distinct desktop quirks - one of them a genuine data-losing bug - are
preserved exactly rather than "fixed" (§3), and every intentionally-excluded control (hidden,
out-of-scope, or browse-button-only) is documented, not silently dropped (§4). Runtime
verification (compiling, starting the application, and exercising it against the real
database) remains impossible in this task's environment (§2) and must be the first step taken
in a real environment before this migration is considered production-ready.
