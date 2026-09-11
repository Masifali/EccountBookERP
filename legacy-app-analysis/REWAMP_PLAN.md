# EccountBookERP Web Rewamp — Master Plan

Source of truth for porting the legacy Windows app (`ECCOUNTBOOKERP.exe` +
`Architecture.BLL/Model/DAL/Common/WebHelper.dll`) into this Spring Boot /
Thymeleaf web app, screen-by-screen, matching each legacy screen's fields.

Legacy code reference (already extracted, in `legacy-app-analysis/`):
- `extracted_dlls.zip` — real BLL/Model/DAL/Common/WebHelper assemblies (open in ILSpy/dotPeek for actual C#)
- `extracted_disasm.zip` — IL disassembly of those assemblies, by namespace
- `il_by_namespace_part1-4.zip` — IL disassembly of the exe's own UI layer (WinForms), by namespace

Scope: rice-mill-relevant modules only. Steel, FeedMill, Salt trading, Cmagt/CmTr
(commission agent), precast-concrete (`pcc`), fingerprint/biometric, DSLR camera
capture, GSM/SMS-hardware, and similar industry-specific or hardware-integration
modules are explicitly **excluded** per your direction — flag if any of these
turn out to be in active use and I'll add them back in.

Status legend: `[ ]` not started · `[~]` partially done · `[x]` done

## Already covered (prior sessions, Tasks #1-9)
- [x] Banking Management hub, Bank Reconciliation
- [x] Account/Party Custom Group allocation (+ Account Type filter)
- [x] User Chart Of Account Management
- [x] Accounts Allocation (corrected to document-type based)
- [x] Ledger screens: Multi Accounts, Bank Summary, Customer (initial pass)
- [~] Voucher entry — web app already has a **unified** voucher screen
      (`vouchers/new_voucher.html` + `VouchersController`) covering
      Contra/Expense/Payment/Receipt/Journal etc. via a `voucherType` selector,
      vs. the legacy app's one-screen-per-voucher-type design. Field-parity audit
      done against Contra/Expense/FCYVoucherEntry/PaymentVoucherNew/
      ReceiptsVoucherNew/JournalVoucher_New/PartyPaymentVoucher/PartyReceiptVoucher:
      - [x] Foreign currency fields (cmbCurrency/txtExchangeRate/txtFcyAmount) —
            added `currency`/`exchangeRate`/`fcyAmount` to `VoucherEntry`, wired
            into the entry grid (new_voucher.html + countx_vouchers.js row
            add/delete reindexing). These were present on nearly every legacy
            voucher screen and completely absent before.
      - [x] Cost center allocation per line (ExpenseVoucher's
            dtGridCostCenterDetail/dtAllSubCostCenters) — added `CostCenter`
            master-data entity/repo/service/controller/template (list +
            modal, self-referencing parent, `/accounts/cost_centers*`,
            sidebar link under VOUCHERS), then wired a `costCenter` FK onto
            `VoucherEntry` and a per-line select2 column on
            `new_voucher.html` (+ `countx_vouchers.js` row add/delete
            reindexing), mirroring the FCY-fields pattern exactly.
      - [ ] Document type dropdown per voucher (cmbdoctype) — check whether this
            duplicates the existing `voucherType` concept or is a distinct field.
      - [ ] Approval workflow status (cmbApproveStatus) — no ApprovalStatus
            entity/workflow exists yet; likely belongs with the Tier 4
            ApprovalDashboard module rather than being bolted onto vouchers alone.
      - [ ] Invoice-level payment/schedule detail grid (frmPartyPaymentVoucher's
            dtDetail/dtSchedule, sale-tax checkbox+account, percent-based calc) —
            bigger feature, needs its own scoping pass.
      - [ ] FreightVoucher — structurally a different, much more complex
            transaction (GP number, vehicle, weight tolerance/shortage calc,
            driver info) not a simple voucher-type variant; treat as its own
            screen under Tier 2, not part of this unified-voucher audit.

## Tier 1 — Foundational / master data (do first, everything else depends on it)
**Tier 1 status: fully triaged as of this pass.** Every Tier-1 module has
been inspected class-by-class against the legacy IL dumps: real gaps were
built (see checked sub-items throughout this section), screens that
duplicate existing web-app coverage were confirmed and left alone, and
genuine standalone clusters needing their own dedicated scoping pass were
explicitly deferred with written reasoning rather than rushed or silently
skipped. Remaining open items in this tier (Tax_Definition compliance
cluster, the granular per-user-per-screen permissions system, the item-
allocation/discount-policy pricing engine, the approval-workflow
document/policy cluster, AcfrmDefineLots/GatePassGeneral/Stock_Closing) are
each flagged inline with why they were deferred - proceeding to Tier 2 next.

- [x] Account_Definition (63 classes) — remaining voucher/PDC/cost-center screens (all triaged - built where a real gap existed, explicitly deferred with a documented reason where a screen duplicates an already-covered feature or is a genuine standalone module for later scoping):
      - [x] frmCostCenter — see CostCenter master-data screen above.
      - [x] frmUsersWithCostCenter — added `UserCostCenterAccess` join entity/repo/
            service/controller (pending/allocated/allocate/unallocate, mirroring
            the existing CustomGroup/UserAccountAccess allocation patterns) +
            `accounts/users_with_cost_center.html` (pick a cost center on the
            left, two-grid allocate/un-allocate Users on the right) +
            `users_with_cost_center.js`, sidebar link under ACCOUNTS.
      - [x] CheqbookStatus - already fully covered by the existing
            `/vouchers/cheque-books/view-checks/{id}` screen
            (`cheque_book_details.html`): per-leaf grid (USED/CANCELLED/
            VOID/AVAILABLE, payee, amount, used-by/date) generated from
            `ChequeBookService.getChequeBookDetails`, with a cancel/void
            action + reason field matching the legacy chkstatus/txtRemarks
            controls. No new work needed.
      - [x] AcfrmChequebookRegistration - already covered by the existing
            cheque-books add/edit modal (`cheque_books.html` +
            `ChequeBookController`); closed the one real field gap by adding
            `prefix`/`remarks` to `ChequeBook` (legacy txtprefix/txtremarks)
            and the matching modal inputs. (This was also tracked as the
            standalone "Task #20" gap - now folded in here, done.)
      - [x] AcfrmDefPdcManagment - field-for-field the same screen as the
            PDC register already built (cmbbank/cmbcheqstatus/txtcheqno/
            txtcheqamount/txtcheqdate/cmbcheqtype/CmbAccountTitleDebit+Credit/
            chkapproved/txtComments all map onto `PostDatedCheque` +
            `post_dated_cheques.html`). No new work needed.
      - [x] BsPlSettingForm - added `FinancialStatementNote` master data
            (entity/repo/service/controller/template, mirroring CostCenter)
            for the legacy dtNoteTile/grdNoteChange "Note" grouping, plus
            `financialStatementType`/`bsClassification`/`financialNote`
            fields on `Account` (radBS/radPL, radAssets/radLiablities), and
            a bulk classification grid (`accounts/bs_pl_setting.html` +
            `BsPlSettingController`, DataTable-paginated, saves all rows via
            one AJAX POST) matching the legacy grdBsPl screen. (Generating
            the actual printed BS/PL statement from this classification is
            report-rendering work, out of scope here - belongs with Tier 3
            Account_Reports.)
      - [x] DayBook / frmDayBook - functionally a "cash book" voucher screen
            (fixed cmbCashAccount, credit grid = receipts, debit grid =
            payments); this is exactly what the existing unified voucher
            screen already does via voucherType (Contra/Payment/Receipt)
            with the cash account picked manually per your earlier
            instruction to extend the unified voucher screen rather than
            build separate per-type screens. No new screen needed.
      - [x] frmFinancialYearClose - added `closed`/`closingDate` to
            `FinancialYear`, a P&L net-movement aggregate query
            (`IVoucherEntryRepository.getPlAccountMovements`, driven by the
            `Account.financialStatementType` classification from the
            BsPlSettingForm work above), and `accounts/financial_year_close.html`
            + `FinancialYearCloseController`/`FinancialYearCloseService`:
            preview grid of every PL account's net movement for a
            branch/year, then a "Close" action that posts ONE balancing
            Journal Voucher (via the existing, already-tested
            `VoucherService.addOrUpdateVoucher`) zeroing each PL account
            into a chosen equity account and marks the year closed. Judgment
            call: this auto-posts a real GL entry, so I routed it through
            the proven voucher-save pipeline rather than hand-rolled
            posting, and it lands the admin on the created voucher
            (`/vouchers/{id}`) immediately after for review - flag if you'd
            rather it stop at a preview-only report instead.
      - [~] frmBillsPayables/frmBillsReceivables - inspected: this is the
            SAME underlying feature already flagged above as "Invoice-level
            payment/schedule detail grid (frmPartyPaymentVoucher's
            dtDetail/dtSchedule...)" - a full bill/invoice entry screen with
            qty/rate detail lines, tax account+percent+amount, a payment
            SCHEDULE grid, subsidiary/project/job-lot allocation and FCY
            fields. This is a genuine standalone AP/AR sub-ledger module,
            not a small gap - deliberately deferred rather than rushing a
            partial build; needs its own scoping pass (probably Tier 2,
            alongside PartyProcessing).
      - [x] ExchangeRateForwardBooking - new `ExchangeRateForwardBooking`
            register entity/repo/service/controller/template
            (`/accounts/exchange_rate_forward_bookings*`), mirroring the
            `PostDatedCheque` register pattern exactly: bank (Account FK),
            branch, docNo/docDate, contractRef, HCY/FCY code+amount,
            booking rate, optional/fixed/final maturity dates and day
            counts, remarks. Sidebar link added under ACCOUNTS.
      - [x] InLandFreightAgreement - new `InLandFreightAgreement` register
            entity/repo/service/controller/template
            (`/accounts/inland_freight_agreements*`), mirroring the same
            PostDatedCheque-style register pattern: district (FK to
            existing `District`), transporter/party (Account FKs), loading/
            unloading location, freight type/rate/uom, pack size, min
            amount, effective date, remarks. Judgment call / simplification:
            the legacy screen has a header + multi-line detail grid (many
            loading/unloading rate rows per agreement); simplified to one
            rate row per agreement entity here, with multiple locations
            recorded as separate agreement rows instead - flag if a true
            nested grid is needed. Sidebar link added under ACCOUNTS.
      - [x] BankBalanceManualEntry - new `BankBalanceManual` entity/repo/
            service/controller/template (`/accounts/bank_balance_manual_entry*`):
            pick a date, SHOW loads every bank account (`IAccountService.
            getAllBankAccounts`) with its system/ERP balance as of that date
            (new `IVoucherEntryRepository.getAccountBalanceAsOfDate` query),
            key in the actual bank-statement balance/source/remarks per
            account, SAVE inserts one history row per non-zero entry
            (mirrors legacy btnSave_Click's per-row `BankBalanceManual`
            inserts) - a running history log below shows every past entry
            with the ERP-vs-actual difference and a delete action. Judgment
            call: legacy's `SourceBy` field is a generic named-list lookup
            (`StaticColumnsService("SourceBy")`) - simplified to a free-text
            Source field here rather than building the generic lookup-list
            infrastructure; the GL drill-down link (grdHistory_LinkClicked)
            was also dropped as a nice-to-have. Sidebar link added under
            ACCOUNTS.
      - [x] AccountBudget - new `AccountBudgetHeader`/`AccountBudgetDetail`
            entity pair (OneToMany cascade ALL, mirroring the Voucher/
            VoucherEntry pattern) + repo/service/controller/template
            (`/accounts/account_budgets*`): header (branch, expense
            account, financial year, doc no/date, from/to month, total
            amount) with monthly detail lines. A GENERATE button spreads
            the total evenly across the month range (editable after), and
            save is validated server-side exactly like the legacy grid-
            total checks - monthly Percent% must sum to 100 and monthly
            Amount must sum to the header Total Amount. Judgment call /
            simplification: dropped the legacy screen's Project allocation
            (no Project master-data module exists yet in this app - would
            belong with Tier 2 PartyProcessing/Production) and its
            attachments (tracked separately below) and its input-history
            filter/reprint panel. Sidebar link added under ACCOUNTS.
      - [x] frmInvoiceWiseOpening - new `InvoiceWiseOpening` entity/repo/
            service/controller/template (`/accounts/invoice_wise_openings*`):
            per-invoice opening balance for a party account (invoice no/
            date/due date/amount, adjusted-so-far amount, computed debit/
            credit split by SALE vs PURCHASE type exactly mirroring the
            legacy Insert method's branching, and a computed Balance Amount
            that must be > 0), for seeding AR/AP aging with per-invoice
            detail instead of one lump account-level opening balance.
            Mirrors the ExchangeRateForwardBooking modal-form pattern.
            Sidebar link added under ACCOUNTS.
      - [~] WhtTaxChallanDeposit - inspected: `btnsave_Click` builds a plain
            `VoucherHead`/`VoucherDetail` (DocumentTypeId=25) - Dr the
            Debit-GL (WHT payable) account, Cr the Credit-GL (bank) account,
            one VoucherDetail line per checked grid row with its amount and
            an `InvoiceNoRefId` back-reference. This is functionally just a
            Journal/Payment voucher (debit WHT account, credit bank, doc
            no/date/remarks) - already achievable today via the existing
            unified voucher screen per your standing instruction to extend
            it rather than build per-type screens. The one missing piece is
            the convenience of picking outstanding WHT-liable invoice
            references from a history grid to auto-fill the lines - that's
            the SAME "invoice-level payment/detail grid" feature already
            deferred above for frmPartyPaymentVoucher and frmBillsPayables/
            Receivables, so it needs the same future scoping pass rather
            than a one-off duplicate here.
      - [x] Attachment(s) - inspected `Attachment`/`AttachmentAddingFromHistory`/
            `AttachmentForDetailRows` (embedded across dozens of legacy
            screens as a reusable "attach files to this document"
            sub-control, backed by `DMSAttachments`/`AttachmentsList` on
            each document's header model). Ported as a generic, non-
            hardware `DocumentAttachment` entity/repo/service/controller
            (`/attachments/list|upload|download/{id}|delete/{id}`,
            generalizing the app's own existing per-voucher-entry
            `VoucherEntryDocument` upload pattern to any (documentType,
            documentId) pair) plus a reusable Thymeleaf fragment
            (`fragments/attachments :: attachments_panel(documentType,
            documentId)`) and `attachments_panel.js` widget any screen can
            drop in. Judgment call / simplification: deliberately excludes
            the legacy control's DSLR/live-camera capture and driver-image
            features, consistent with this rewamp's explicit exclusion of
            camera/biometric hardware modules - only plain upload/list/
            download/delete is ported. Wired into the Account Budget modal
            as a proof of concept; wiring the fragment into the other
            screens built this session (PDC, forward bookings, freight
            agreements, invoice-wise opening, etc.) is left as
            incremental follow-up since each is a one-line `th:replace`
            once a screen needs it, not a blocking gap.
- [~] Inventory_Definition (43 classes) — audited against `InvDefrmAddItem`/
      `InvDeffrmItemCatagory`/`InvDeffrmItemType`/`InvDeffrmItemUomSchedule`/
      `InvDeffrmWarehouse`/`frmRackDefine`/`DefineBrand`/`DefineItemGroup`/
      `ProductType`/`AcfrmDefineLots`/`frmOpeningStockBlancing`/
      `frmStoreOpeningStockBalancing`/`GatePassGeneral(Outward)`/
      `Stock_Closing.*` plus the large item-allocation/discount-policy
      cluster:
      - [x] Item / Item Category / Item Sub-Category / Opening Stock -
            already fully covered by the existing
            `/stocks/stock_opening_form` screen (`StocksController` +
            `ItemDef`/`ItemCategory`/`ItemSubCategory` + `stock_opening_
            form.html`), which already combines exactly what
            `InvDefrmAddItem` + `InvDeffrmItemCatagory` + the sub-category
            level + `frmOpeningStockBlancing`/`frmStoreOpeningStockBalancing`
            (opening qty/rate fields already on `ItemDef`) do in the legacy
            app, one screen instead of four. No new screen needed.
      - [x] InvDeffrmWarehouse, frmRackDefine, DefineBrand, DefineItemGroup,
            ProductType - five real gaps: simple master-data entities
            (`Warehouse`, `Rack`, `Brand`, `ItemGroup`, `ProductType`) +
            repos/services (one shared `InventoryDefinitionController` for
            brevity, list+modal CRUD mirroring the Cost Center pattern) +
            templates under `templates/inventory/`, sidebar links added
            under the existing INVENTORY menu. Then wired all five as
            optional FKs onto the existing `ItemDef` entity and added
            matching dropdowns to `stock_opening_form.html`'s item form
            (mirroring its existing `itemSubCategory` select pattern),
            so every item can now optionally be tagged with a warehouse,
            rack, brand, item group and product type.
      - [x] InvDeffrmItemItemUom / InvDeffrmItemUomSchedule / InvDeffrmItemType -
            `ItemDef` already carries simple inline `unitName`/`unitValue`/
            `conversionName`/`conversionValue` fields covering the common
            case; judged adequate as-is rather than building a separate UOM
            master table, consistent with how simple inline fields were
            preferred elsewhere this session when the legacy screen's
            complexity wasn't justified by real reporting need. Flag if a
            true many-UOM-per-item conversion table turns out to be needed.
      - [ ] AcfrmDefineLots (lot/batch tracking) - a genuine standalone
            feature (batch numbers with expiry/traceability across
            purchase/production/sale) that touches the transactional
            modules, not just master data - needs its own scoping pass
            alongside Tier 2 Purchase/Production/Sale rather than a
            standalone master-data screen here.
      - [ ] The item-allocation/discount-policy cluster (ItemAllocationTo
            SupplierCustomer, ItemMinMaxRateSchedule, SupplierCustomerAllocate
            ToDiscounType, RevExpAcAllocationToProductAndDiscountType,
            ItemAllocateToProductType, ItemAllocationToCustomGroup,
            ItemCustomGroup, AttributeVarientAllocationToItems,
            ItemAllocateToWarehouse, frmAssignGroupToItem,
            frmItemsCustomizedGroups, frmItemsReorderSchedule,
            InvAllocateBrandItemToDiscType, InvAllocateDiscItemToDiscType,
            InvDiscountItem, InvDiscountType) - inspected: this is a large
            pricing/discount-policy engine (which supplier/customer gets
            which discount, by item/brand/product-type/custom-group, with
            min/max rate schedules), the SAME kind of "genuine standalone
            module" as the invoice-level payment/detail-grid cluster
            deferred under Account_Definition - needs its own dedicated
            scoping pass, most likely alongside Tier 2 Sale/Purchase, not a
            one-off partial build here. `ItemDef.reorderLevel` already
            covers the simple single-level reorder case, so
            frmItemsReorderSchedule's multi-level schedule is the only
            piece of this cluster with an existing simple fallback.
      - [ ] GatePassGeneral / GatePassGeneralOutward (goods in/out without a
            sale/purchase) - a real, self-contained feature not yet built;
            deferred for a dedicated pass (not complex, just not reached
            yet in this session).
      - [ ] Stock_Closing (StockCloseRun, MonthlyStockClosing) - a period-end
            stock valuation/closing run, analogous to Financial Year Close
            but for inventory; deferred for a dedicated pass given the
            valuation-method risk (needs to get FIFO/weighted-average right
            against the real chart of accounts) rather than a rushed build.
      - excluded per scope: frmAddItemConcrete, frmOpeningStockBlancing_Engr,
            AttributeVarientAllocationToItemsEngr, frmAddItem_Engr,
            ItemAttributeVarient_Engr (precast-concrete/"Engr" variants -
            already-excluded pcc module), frmPackingMaterialItemsAllocateTo
            TransactionFlow, frmSupplierCustomerUnAllocateFromDiscounPolicy
            ForPackingCharges (already-excluded PackingMaterial_Store
            module), InvAddItemsPOS (point-of-sale - out of scope for a
            rice mill, no POS module in this rewamp)
- [x] Configurations (19 classes) — audited against DefineCompany,
      frmCompanyProfile, frmBranches, frmFinancialYear, frmUserModuleAdmin,
      UserProfileInfo, ScreensAllocateToCompany, Configuration,
      frmCompanyReport, frmReportConfig, UserRightsByCompany, frmUserRights,
      UserRight, frmDefineAppModules, CreateSMSTemplate, SMSContacts,
      SMSParameters, ScreenMessageReceivers, CustomerRegistration_MobileApp:
      - [x] DefineCompany / frmCompanyProfile / frmBranches / frmFinancialYear /
            basic user CRUD - already fully covered by the existing
            `/utilities/utilities` screen (four panels: USER, COMPANY,
            BRANCH, FINANCIAL YEAR), which already has all the fields the
            legacy screens have (name/address/contact/email/website/
            proprietor info/NTN for company+branch; name/from-to date for
            financial year; name/login/email/admin-flag/active for users).
            No new screen needed.
      - [ ] frmUserModuleAdmin / ScreensAllocateToCompany / UserRightsByCompany /
            frmUserRights / UserRight / frmDefineAppModules - inspected: the
            legacy app has a full granular per-user-per-screen rights system
            (which modules/screens a company can see, which rights - view/
            add/edit/delete/print - each user has per screen). This web app
            instead uses a simpler Spring Security role-based model
            (`@PreAuthorize("hasAuthority('ADMIN')")`, binary admin/non-admin
            on the User panel) that every screen built in this whole rewamp
            so far relies on. Replacing/extending that with the legacy's
            granular per-screen rights model is a genuine standalone,
            security-sensitive undertaking that touches the app's entire
            access-control architecture - deliberately deferred for its own
            dedicated scoping pass rather than a rushed bolt-on, flagged
            here rather than silently skipped.
      - excluded per scope: CreateSMSTemplate, SMSContacts, SMSParameters,
            ScreenMessageReceivers (GSM/SMS-hardware - already-excluded
            module), CustomerRegistration_MobileApp (companion mobile app
            registration, not a core rice-mill ERP workflow)
      - [ ] frmCompanyReport, frmReportConfig - inspected (IL field dump):
            these map report short-name/file-name/stored-procedure-name
            triples per company - a registry for the legacy app's own
            Crystal Reports/stored-procedure report engine. Not portable
            business master data; this web app builds each report as its
            own Thymeleaf page directly (see Tier 3 Account_Reports/
            Inventory_Reports), so there is no equivalent "report registry"
            concept to port. No `Configuration` class found as a distinct
            screen (may have been the frmCompanyReport/frmReportConfig pair,
            or a settings singleton with no standalone form) - not building
            for this rewamp; flag if a specific configurable setting from it
            turns out to be needed.
- [x] Lookups (35 classes) — audited all 35; most are either dependent on a
      Tier 2 module not yet built, or already excluded per scope, not
      standalone Tier-1 master data:
      - [x] frmDefineReasons - built as `Reason` entity/repo/service/
            controller/template (`/lookups/reasons*`), a simple reusable
            reason-code list (name + free-text category, e.g. "Stock
            Adjustment"/"Rejection"/"Cancellation") other screens can
            reference. Sidebar link added under INVENTORY.
      - excluded per scope: ItemQcGrade (QCL/LabAnalysis module),
            ScaleKartDefine + VehicleWeightLookUp (WeighBridgeGeneral
            Lookups module), ExImVCICategory + ExImVCIParameter +
            GenerateExportInvoiceNos (ExImShipmentDocuments module) - all
            already-excluded per the top-level exclusion list.
      - [ ] Tier-2-dependent, deferred until their owning module is scoped:
            frmLookUpDefineAsset (FixedAsset), ProductionRoutingName +
            CommodityDetailItemCustomerWise + ConsumptionItems (Production),
            frmJobLotsAllocationToBranch + frmPlantsAllocationToBranch +
            frmContractorsAllocationToBranch (Production/Contractor_Wages -
            Contractor_Wages itself is already excluded) + frmWarehouses
            AllocationToBranch + frmWarehousesAllocationToPlant (Warehouse
            now exists per Inventory_Definition above, but Plant does not
            yet), frmItemAllocateToManufacturer (needs a Manufacturer
            master not yet built), WagesExemptItemSchedule + Define_Department
            (HRM), frmBranchesAllocationToUser (the deferred granular
            permissions cluster from Configurations above).
      - [ ] Part of the already-deferred item-allocation/pricing-policy
            cluster (see Inventory_Definition above): frmItemPricingSchedule,
            frmItemPricingScheduleForRice, PackingChangePriceSchedule,
            frmAttributeVarient, ItemAttributeVarient.
      - [ ] Part of the already-deferred Tax_Definition cluster:
            SubsidiaryAccountsDefine, TaxLookup.
      - [ ] Multi-language label overrides (ItemTypeMultiLanguage,
            ItemCategoryMultiLanguage, SupplierCustomerMultiLanguage,
            ItemMultiLanguage) - no i18n requirement identified for this
            rewamp; deferred as low priority, flag if translation support
            is actually needed.
      - [~] SeasonYearSchedule - inspected (IL field dump): SeasonStartDate/
            SeasonEndDate/IsActive - a date-range per season, distinct from
            the now-built name-only `CropYear` (DefineCropYear). Real,
            small gap: `CropYear` should probably gain optional
            `startDate`/`endDate` fields. Not done in this pass because
            `CropYear` currently shares the generic `lookups/define_list.html`
            template with 7 other name-only entities - adding date fields
            means splitting it into its own dedicated template first. Left
            as a documented follow-up rather than disrupting the shared
            template for one entity.
      - [ ] MoistureSlabs (rice-quality grading by moisture - genuinely
            rice-mill-relevant, but ties into Purchase pricing so belongs
            with a Tier 2 Purchase scoping pass rather than standalone),
            frmInvLookup + frmInvLookUpType (looks like a generic named-
            lookup-list framework, similar in spirit to the legacy
            "SourceBy" static-columns lookup noted under BankBalanceManual
            Entry - worth a dedicated look as it might replace several
            small Define* screens at once, but not inspected in enough
            depth yet to build).
- [ ] Tax_Definition (5 classes) — inspected all 5
      (frmTaxTypeAndBusinessTypeMapping, DefineTaxItem,
      SuppCustAllocateToTaxSuppCust, RegularItemsAllocateToTaxItem,
      DefineTaxableSupplierCustomer): this is a real sales-tax/WHT
      compliance cluster, not simple master data - `DefineTaxItem` embeds
      an entire item-tax-schedule sub-system (its own UOM/category/type/
      tax-schedule tabs) and `DefineTaxableSupplierCustomer` is close to a
      full FBR-style taxable-party registration form (CNIC, city/province,
      subsidiary-account flags, company/town, office mobile, ~40+ fields) -
      likely overlapping with fields the existing `Account`/party master
      may need extending for rather than a standalone new entity. Given the
      compliance stakes of getting tax configuration wrong, deliberately
      deferred as a genuine standalone module needing its own dedicated
      scoping pass (likely alongside the already-deferred WhtTaxChallanDeposit
      and the item-allocation/discount-policy cluster) rather than a rushed
      partial build.
- [x] ~30 single-screen `Define*` modules — triaged all of them:
      - [x] DefineCropYear, DefineBusinessType, DefineBoilerName, DefinePackingType,
            DeliveryTerms, PaymentTerms, VehicleType — 7 simple name-only master-data
            screens, all sharing one generic `lookups/define_list.html` template
            (parameterized via `title`/`saveUrl`/`deleteUrlPrefix`/`icon` model
            attributes) and one shared `DefineListsController`, mirroring the
            Reason/Brand pattern exactly. Sidebar links added under INVENTORY.
      - [x] DefineCountry — same generic-template pattern, added as an 8th entity
            on `DefineListsController` (`/lookups/countries`).
      - [x] DefineMultiCurrency — built as its own `Currency` entity (code/name/
            symbol) + `CurrencyController` + `lookups/currencies.html`
            (`/lookups/currencies`), since it needed an extra field the generic
            name-only template doesn't have. Voucher/ExchangeRateForwardBooking
            FCY fields (hcyCode/fcyCode/currency) intentionally stay free-text
            codes (this codebase's established pattern, confirmed by re-checking
            `VoucherEntry`/`ExchangeRateForwardBooking`) — this list is a
            reference/pick-list, not a new FK relationship, so no migration risk.
      - [x] DefineProvince, DefineDistrict — discovered `Province`/`District`/
            `City` entities already existed (used read-only elsewhere via
            `IProvinceRepository`/`IDistrictRepository`) but only City had a
            management screen (`AccountsController#newCity` /
            `accounts/city.html`). Added `GeographyController` +
            `accounts/provinces.html` (flat list+modal) and
            `accounts/districts.html` (list+modal with a Province select2,
            matching the existing `parentCode`-by-id hierarchy) to close the
            gap. Hard delete (no `active` flag on these entities, matching
            their existing plain-JpaRepository shape - not changed). Sidebar
            links added next to "ADD NEW CITY".
      - [x] DefineCity — already fully covered by the existing
            `AccountsController#newCity` / `accounts/city.html`. No new work.
      - [ ] DefineTehsil — deferred. Legacy hierarchy would be
            Province > District > Tehsil > City, but this codebase's existing
            `City.parentCode` already points directly at District (confirmed
            via the geography screens above), so inserting Tehsil as a new
            level would mean migrating every existing City row's parentCode
            and every screen that reads the Province/District/City chain -
            real risk for a level nothing else in Tier 1 currently references.
            Low priority; flag if any downstream module actually needs it.
      - [ ] DateLock (period-lock: block postings before a cut-off date per
            branch/financial year) — genuinely simple in isolation, but its
            correct enforcement point is inside the voucher-save pipeline
            (`VoucherService.addOrUpdateVoucher`) alongside the already-built
            `FinancialYearCloseController`/`closed` flag, not a standalone
            master-data screen. Deferred to be scoped together with that
            existing closing-flow code rather than bolted on separately.
      - [ ] DefineMasterItems — inspected: overlaps the already-covered
            `ItemDef`/`stock_opening_form.html` (see Inventory_Definition).
            No separate screen needed; any real field gap belongs in that
            existing item form, not a new screen.
      - [ ] DefineOtherParties, DefineReferenceParties — both are variations on
            the party/Account master (secondary contacts / reference persons
            for an Account) - likely additional fields on `Account` or a
            child entity, not a standalone lookup. Deferred pending a proper
            Account-master field audit (out of scope for a quick lookup pass).
      - [ ] DefineProcessStep — Production module master data (process/stage
            definitions for a production order); genuinely Tier-2-dependent
            (Production (42) hasn't been scoped yet), deferred to that tier.
      - [ ] DefineApprovalDocument, DocumentGroup, DocumentApprovalPolicy,
            DocumentApprovalLevel — all part of the approval-workflow cluster
            already flagged as deferred under both the unified-voucher audit
            ("Approval workflow status") and Tier 4 ApprovalDashboard (36
            classes) - grouping document types under an approval policy only
            makes sense once that workflow engine itself is scoped. Deferred
            together.
      - [ ] SeaPortsDefine — Export/Import module master data (sea port lookup
            for shipment documents); Tier-2 (Import (15)) / explicitly-excluded
            ExImShipmentDocuments-adjacent. Deferred to that tier.
      - [ ] BlockListVehicles — vehicle blacklist for gate/weighbridge screens;
            depends on the not-yet-built StoreManagement/GatePass flow
            (already flagged as deferred under Inventory_Definition). Deferred
            together.
      - [ ] CustomerDiscountPolicy, DiscountPolicyForParty, ItemWiseCommissionSchedule,
            ItemsAllocationToCompany, ItemsActiveOrInActiveByCompany — all part
            of the already-deferred item-allocation/discount-policy cluster
            (see Inventory_Definition) - a real pricing-engine feature
            (percent/slab rules, per-party/per-company/per-item overrides),
            not simple master data. Needs its own dedicated scoping pass.
      - [ ] PurchaseOrderHistoryStatus — Purchase module status/history lookup;
            Tier-2-dependent (Purchase (25) not yet scoped). Deferred.
      - [ ] ChequePrinting — cheque print-layout/positioning settings (physical
            print template per bank cheque design); a print-layout designer,
            not master data - genuinely different kind of feature, deferred as
            its own scoping pass (would live alongside the cheque-book/PDC
            screens already built).
      - [ ] BackUpDatabase(New) — infra/ops feature (DB backup scheduling), out
            of scope for an ERP business-data rewamp; already listed under
            Tier 5 SystemUtilities/BackUp*. No action here.
      - [ ] PdcBank — inspected: the existing PDC register
            (`post_dated_cheques.html`, cmbbank field) already lets a PDC be
            tied to any `Account`-typed bank, which is the same underlying
            data PdcBank would restrict/list. No separate screen needed
            unless a bank-specific restriction (e.g. "only banks flagged for
            PDC") turns out to be required - flag if so.
      - [ ] FcYBankChargesTypes — FCY bank-charges type lookup for the
            BankBalanceManualEntry/ExchangeRateForwardBooking cluster; a real
            gap but low-traffic and easy to fold into a future `Currency`-
            adjacent pass (same shape as Currency/Reason) rather than build in
            isolation right now. Deferred, low priority.

## Tier 2 — Core transactional modules
**Tier 2 status: fully triaged as of this pass** (StoreManagement, Sale,
Purchase, SaleTrading, PurchaseTrading, Production, PartyProcessing, Import,
Service, AccountToAccountTransfer, FixedAsset/AssetSchema). The headline
finding: this app already has a mature core transactional engine (Purchase/
Sale Journal Vouchers with FIFO-costed `ItemStock`/`ItemStockEntry`, Purchase/
Sale Orders, Purchase/Sale Return Vouchers, the `kanta` weighbridge
subsystem) that covers the overwhelming majority of what the legacy
Purchase/Sale/StoreManagement/Trading namespaces do - so most of Tier 2
turned out to be confirming existing coverage and excluding already-scoped-
out verticals (Steel/`_Engr`, Paddy_Purchase/Ghalla-Mandi, PackingMaterial_
Store, ExImShipmentDocuments/LogisticManagement), not new building. Real
new master data was built where it was genuinely missing and low-risk
(ProductionPlant/ProductionPlanType, AssetCategory/FixedAsset register).
Two categories were deliberately NOT built despite being real gaps: (1) any
screen that would post a *new kind* of stock movement (transfer,
adjustment, internal issuance, job-order costing) - the `ItemStock` ledger
only supports IN/OUT/PURCHASE_RETURN/SALE_RETURN today, and adding a type
without being able to compile/test against its FIFO balance logic is a real
risk to real inventory data; (2) HRM (64 classes) and the PartyProcessing
contract-milling module - each large enough, and entangled enough with
compliance (payroll tax/EOBI) or its own accounting relationship, to need
its own dedicated scoping pass rather than a rushed slice. Both are flagged
inline with full reasoning rather than silently skipped.

- [x] StoreManagement (51 classes) — triaged all 51 against the existing
      web app. Key finding before the detail: this module sits on the
      legacy `ItemStock`/`ItemStockEntry` ledger (FIFO rate, bag-weight
      deduction, bardana/gisahi/silai/moisture/freight fields - genuine
      rice-mill-specific accounting, not generic inventory), which today
      only supports two transaction types
      (`MSTConstants.ITEM_STOCK_ENTRY_PURCHASE_IN`/`_SALE_OUT`, plus
      `_PURCHASE_RETURN`/`_SALE_RETURN` used by the already-existing PJV/
      SJV/PRV/SRV voucher screens). Any screen whose job is to post a new
      *kind* of stock movement (transfer, adjustment, internal issuance)
      would need a new transaction type wired through
      `ItemStockService`'s balance/FIFO logic - genuine core-ledger surgery,
      not a simple CRUD screen, and too risky to guess at without being able
      to compile/test in this environment. Deliberately deferred rather than
      rushed; the DayBook/pricing-engine precedent from Tier 1 applies here
      even more strongly given real money/stock is at stake.
      - [x] Already fully covered by existing screens - no new work:
            frmPurchaseInvoiceDirectStore / PurchaseInvoiceStoreManagement /
            GrnStore / LoadGrnForStore (= the existing Purchase Journal
            Voucher flow, `PurchaseQtyController` + `new_pjv*.html`, which
            already posts `ITEM_STOCK_ENTRY_PURCHASE_IN`); PurchaseInvoiceReturn_Store /
            StoreReturn (= existing PRV, `PayablesController` + `new_prv.html`,
            `ITEM_STOCK_ENTRY_PURCHASE_RETURN`); GoodsDispatchNotes_Store (=
            existing SJV/SaleOrder flow, `ReceivablesController`/
            `SaleQtyController` + `new_sjv*.html`/`sale_order_form.html`);
            frmPendingPurchaseOrderStoreLoader / frmPendingPurchasePreBillLoader
            (= existing `purchase_order_list.html`/`view_pre_purchase.html`);
            frmItemCatagoryStore / AddItemStore (= existing `ItemCategory`/
            `ItemDef` + `stock_opening_form.html`); ItemAllocationToStoreRack
            (= the `warehouse`/`rack` FKs already added to `ItemDef` in the
            Inventory_Definition pass above).
      - excluded per scope (PackingMaterial_Store): AddItemPM,
            DeliveryOrderPackingMaterial, StockAdjustmentForPM,
            PartyToPartyPackingMaterialTransfer,
            InvfrmInvSaleInvoiceDirectPackingMaterial, frmLoadGdnStoreAndPm,
            frmSaleOrderStoreAndPm, frmGoodsDispatchNoteStoreAndPm,
            PendingDoPmForIssuance
      - [ ] Part of the already-deferred item-attribute/variant cluster (see
            Inventory_Definition above): frmAttributeVariantAllocationToItem,
            frmAttributeVarient, frmAttributeAllocationToItem,
            frmPurchaseInvoiceDirectStoreWithVariant
      - [ ] Core-ledger-risk, needs a dedicated scoping pass alongside
            Purchase/Sale/Production (all share the same `ItemStock`/
            `ItemStockEntry` ledger) before any of these can be built safely:
            frmStockAdjustment + LoadStockForAdjustment, frmStockTransfer +
            frmStockTransferManual + frmStockTransferMultiBranch +
            frmStockTransferStore, frmStoreIssuanceToCosumableStore +
            StoreIssuanceDirect + StoreIssuanceFinancial + frmGSIssuance +
            frmGSIssuanceWithoutLoader + frmLoadIssuanceForReturn,
            frmGrnEmptyBags (bag-count reconciliation, tied to the same
            bardana/bag-weight fields on `ItemStockEntry`).
      - [ ] Low priority / likely redundant with existing PurchaseOrder flow -
            not blocking: frmPurchaseDemand + frmPendingPurchaseDemand (an
            internal pre-PO requisition step; the existing `PurchaseOrder`
            screen already covers "what to buy" - a demand/requisition layer
            in front of it is a nice-to-have, not a gap), DepartmentRequest +
            DepartmentRequestToConsumableStore + LoadDepRequestToConsumableStore
            (internal consumption request - HRM/Production-tier adjacent,
            deferred to that scoping pass), frmDeliveryChallanAgainstPurchasePreBill
            + frmPendingDeliveryChallanLoader (delivery-challan matching
            against the existing pre-bill/PO screens - a paperwork/matching
            convenience, not a distinct transaction).
      - excluded (internal code, not a screen): FromWise_Helper_Methods.*
            (helper classes for the two frmPurchasePreBill/
            frmDeliveryChallanAgainstPurchasePreBill screens above, no
            separate UI).
- [x] Sale (30 classes) + Purchase (25) + SaleTrading (20) + PurchaseTrading (18)
      — 93 classes total, triaged together since they're the same underlying
      concept (goods in/out + invoice) repeated per business-flow variant.
      Found a mature, already-working equivalent for essentially all of it:
      - [x] Core purchase flow already covered: `InvfrmPurchaseInvoice` /
            InvFrmGRN / frmLoadGRN / GRNDirectAgainstOrder /
            PurchaseInvoiceAgainstGrnOrder / frmPurchaseInvoiceAgaintGrnDirect /
            InvFrmGRNDirect = the existing Purchase Journal Voucher flow
            (`PurchaseQtyController`, `PurchaseJournalVoucherService`,
            `new_pjv*.html`/`view_pjv*.html`) which already posts
            `ITEM_STOCK_ENTRY_PURCHASE_IN` and already supports both
            "direct" and "against a Purchase Order" modes.
            `PurchsaeOrder`/`SupplyOrderPurchase`/`frmSupplierDispatchPreBill`
            = existing `PurchaseOrder`/`PrePurchase`
            (`purchase_order_form.html`/`pre_purchase_form.html`).
            `InvfrmInvPurchaseInvoiceReturn`/`frmLoadPurchaseInvoiceForReturn` =
            existing PRV (`PayablesController` + `new_prv.html`,
            `ITEM_STOCK_ENTRY_PURCHASE_RETURN`).
      - [x] Core sale flow already covered: `InvfrmSaleInvoice` / InvFrmGDN /
            frmLoadGDN / frmGdnAgainstSaleOrder / frmSaleInvoiceDirect(+variants)
            = the existing Sale Journal Voucher flow (`SaleQtyController`,
            `SaleJournalVoucherService`, `new_sjv*.html`/`view_sjv*.html`,
            `ITEM_STOCK_ENTRY_SALE_OUT`). `SaleOrder`/`BookingOrder`/
            `LoadPreBookingOrder` = existing `SaleOrder`
            (`sale_order_form.html`). `InvfrmSaleInvoiceReturn`/
            `LoadSaleInvoiceForReturn` = existing SRV (`ReceivablesController`/
            `SaleQtyController` + `new_srv.html`, `ITEM_STOCK_ENTRY_SALE_RETURN`).
      - [x] Gate pass / weighbridge already covered: `InwardGatePass`,
            `OutwardGatePass`, and all the Trading-namespace gate-pass
            variants (`InwardGatePassTrade`, `OutwardGatePassTrading`) map
            onto the app's existing, comprehensive `kanta` weighbridge
            subsystem (`com.mst.kanta.*` - `WeightEntry` first/second
            weighing, `ProductTravelRecord`, `daily_weight_list.html` etc.),
            which already captures vehicle in/out + gross/tare/net weight.
            `ItemStock.gateInwardNo`/`gateInwardDate` already links a
            purchase back to a gate/weighbridge event. No new work needed.
      - [x] `PurchaseTrading`/`SaleTrading` namespaces as a whole - these are
            the *same* Order/GRN/Invoice/GDN concepts above
            ("WithQty"/"WithTax" variants) under a differently-branded
            business-flow, not a distinct feature; the existing unified
            PJV/SJV/PurchaseOrder/SaleOrder screens already cover the same
            ground for every branch/company in this single-tenant web app.
            No separate "Trading division" screens built - flag if a real
            need for parallel document series/numbering per division
            surfaces later.
      - excluded per scope (Steel/engineering-goods vertical - `_Engr`
            suffix, matches the already-excluded Steel module):
            GoodsReceivingNote_Engr, frmLoadDeliveryOrderOnGdn_Engr,
            DeliveryOrder_Engr, frmGDN_Engr, frmSaleInvoiceReturn_Engr,
            LoadGdnTrading_Engr, SaleInvoiceTrading_Engr,
            GoodsDispatchNotes_Engr, frmLoadSaleInvoiceForReturnEngr
      - excluded per scope (Paddy_Purchase/Ghalla-Mandi market-purchase
            variant, already on the exclusion list): frmLoadGrnForGhallaMandi,
            frmLoadGRNForPaddyPurchase, MarketGrn, MarketPurchaseOrder
      - [ ] Core-ledger-risk / needs a dedicated scoping pass alongside the
            StoreManagement deferral above (return-goods-to-stock,
            adjustment-adjacent, no matching transaction type exists yet):
            SaleReturnGrn, frmGdnForPurchaseReturn +
            frmLoadGdnForPurchaseReturn + frmLoadGrnForGdnPurchaseReturn,
            frmLoadGdnForGrnSaleReturn
      - [ ] Lower-priority scheduling/logistics conveniences, not blocking -
            defer to a Tier 3/logistics pass: frmDeliveryScheduleCustomer,
            frmDriverBioDefinition + frmDriverBio, DeliveryOrderTransfer,
            DeliveryOrder, PartyProcessingGdnLoad +
            SaleInvoiceAgainstPartyProcessingGdn (PartyProcessing-tier
            dependent), frmSupplierDispatch, frmSupplySchedule +
            InvfrmLoadSupplySchedule, frmDeliverySchedule +
            InvfrmLoadDeliverySchedule, FrmDeliveryChallan,
            LoadavailableTransactionsForGDN, LoadSaleOrderForIssuanceWithAvailableStock
      - excluded (loader/staging helper classes for screens already listed
            above, no distinct UI beyond their parent screen):
            LoadPendingGatePassForGrn, frmUnLoadingInformation,
            frmLoadPendingReturnableGdnForReceiving, LoadDemond_Engr,
            frmLoadPurchaseOrderForGatePass, LoadPurchaseOrderForGrn,
            LoadPurchaseOrderWithQty, frmLoadGRNTrading, LoadPurchaseOrderTrading,
            LoadPurchaseOrder, SaleOrderLoadForDo, LoadSaleOrderWithQty,
            LoadGdnTrading_Engr, frmLoadGDNTrading, LoadSaleOrder
- [x] Production (42 classes) — triaged all 42. Found the core "convert raw
      material into output" flow already covered: `ProductionController` +
      `production_form.html`/`newRecipeForm.html` (recipe/BOM-driven
      production voucher, already posting `ITEM_STOCK_ENTRY_PURCHASE_IN`/
      `_SALE_OUT` per the earlier grep). Real, safe master-data gaps closed:
      - [x] DefineProductionPlant - built `ProductionPlant` (code/name/
            branch FK) + `ProductionPlantController` + `production/plants.html`
            (list+modal, branch select2, mirroring the Cost Center/District
            pattern). Distinct from `Warehouse` (a storage location) - a
            plant is a physical production line/unit within a branch.
            Sidebar link added under PRODUCTION.
      - [x] DefineProductionPlanType - built as a 9th entity on the shared
            `DefineListsController`/`lookups/define_list.html` (name-only).
      - [ ] DefineProductionType - inspected: beyond a name, this legacy
            screen stores five behavior-toggle flags
            (IsManualEntryOnInputNotAllowed/IssuanceByLoader/
            OutputItemsByJobOrderRateSchedule/SaleCostingJobOrderWise/
            WagesCompulsoryOnProduction) that configure the job-order
            costing engine below. Not built as a bare master-data shell,
            since those flags would do nothing until that engine exists -
            deferred together with it rather than shipping a screen whose
            fields are silently inert.
      - [ ] Job-order costing engine, core-ledger-risk (same `ItemStock`/
            `ItemStockEntry` ledger flagged under StoreManagement/Sale/
            Purchase, plus real per-job cost allocation - FOH, input/output/
            overhead by job order): frmProductionJobOrderMain +
            frmProductionJobOrder + frmGenerateJobOrderNos,
            ProductionPreCostingAndJobOrder + ProductionAgainstPreCostingJobOrder,
            frmProductionInput/Output/Overhead/Consumption/PackingMaterial,
            frmProductionCumPackMaterialConsumption, frmProductionSettlement,
            frmCreateProductionCostingReport + Regular.frmProductionFohReportCreate
            (Tier 3 report), LoadInputDataByJobOrder/LoadOutPutDataByJobOrder/
            LoadOverHeadsDataByJobOrder/LoadPackingMaterialDataByJobOrder/
            LoadConsumptionPendingforRates/LoadOutPutPendingforRates,
            StoreStockConversion + invfrmStockConversionProduction +
            LoadavailableTransactionsForConversionStore,
            LoadavailableTransactionsForIssuance(OnConsumption),
            LoadavailableTransactionsForStockReleaseFromFumigation,
            frmBillOfMaterial + frmBillOfMaterial_Manufacturing +
            BillOfMaterialWiseProduction (may overlap the existing recipe
            system above - worth checking first when this cluster is
            scoped), frmDailyPlantConsumedHours, frmBoilerConsumption
            (references the `BoilerName` master data already built in
            Tier 1), frmPendingMoveOrderDocuments, frmLoadStockShortFallForSales,
            LoadExportScheduleForProduction (Import-tier dependent),
            ProductionOutputAllocationWithExportInvoice (Import-tier
            dependent), LoadInvoices.
      - [ ] FoodProductionWithValues(New) - inspected: appears to be an
            alternate/simplified production-entry variant (possibly for a
            different industry vertical given the "FoodProduction" naming,
            distinct from the rice-specific recipe system already built) -
            not enough signal to tell if it duplicates the existing
            `production_form.html` or is a genuine gap; folded into the
            job-order-engine scoping pass above rather than guessed at.
- [~] PartyProcessing (30 classes) — revisited with a dedicated scoping
      pass (per your choice to prioritize this over HRM). This is
      contract/job-work milling: a third party's raw material comes to the
      mill for processing and the output goes back to them - genuinely
      rice-mill relevant, not on the exclusion list. The key unlock from
      re-reading the field-level IL dumps: this module's stock is a third
      party's goods, not the mill's own inventory, so it was always meant
      to live in its OWN segregated ledger - it does NOT need to touch the
      mill's `ItemStock`/`ItemStockEntry` ledger at all. That removes the
      core-ledger risk that caused the earlier blanket deferral: I could
      design a brand-new ledger schema and transaction types from scratch
      instead of extending one I can't compile/test against.
      - [x] Master data built (all simple, safe, mirror the established
            Tier 1 Define*/master-data pattern): `PartyProcessingItemCategory`
            (DefItemCatagoryPartyProcessing), `PartyProcessingItemType`
            (DefPartyProcessingItemType), `PartyProcessingItem`
            (DefineItemPartyProcessing - own catalog, kept separate from
            the mill's own `ItemDef` so a customer's items never mix with
            the mill's own catalog), `PartyProcessingJobLot`
            (AcfrmDefineJobLotPartyProcessing - the batch id that ties a
            customer's goods together through GRN -> conversion -> GDN),
            `PartyProcessingSupplier` (frmPartyProcessingDefineSupplier -
            kept as its own light identity record rather than folded into
            `Account`, since most of these parties only need real ledger
            billing when a Wages Bill is actually raised; `glAccount` links
            to the existing `Account` for that step). All five via
            `PartyProcessingDefinitionsController` +
            `party_processing/{item_categories,item_types,items,job_lots,
            suppliers}.html`. `InvDeffrmWarehousePartyProcessing` -
            deliberately NOT duplicated; reuses the existing `Warehouse`
            master from Tier 1 Inventory_Definition instead of a parallel
            entity.
      - [x] The new segregated ledger: `PartyProcessingTransaction` (header:
            type/docNo/date/supplier/jobLot/warehouse/branch/vehicle no/
            gate pass no/bilty no/gross+net+factory+stock+additional
            weight/empty bags used+total/carriage amount/addWages flag/
            remarks) + `PartyProcessingTransactionLine` (item + quantity +
            remarks), mirroring this codebase's own established header/
            detail pattern (`AccountBudgetHeader`/`Detail`,
            `ItemStock`/`ItemStockEntry`). Five fresh transaction-type
            constants defined (`MSTConstants.PARTY_PROCESSING_TXN_*`: GRN/
            GDN/ADJ/OPN/CIN/COUT) and a balance query
            (`IPartyProcessingTransactionRepository.getBalance`) that nets
            them per item/supplier/job-lot - all new code, none of it touches
            `ITEM_STOCK_ENTRY_*` or the mill's own ledger.
      - [x] GRNForPurchaseFromPartyProcessing (goods received from the
            third party) and GDNForSaleToPartyProcessing (goods dispatched
            back to them) built as the two foundational transactions -
            `PartyProcessingStockController` + `party_processing/grn.html`/
            `gdn.html`, header+multi-line-item form (plain form POST with
            indexed `lines[i].*` binding, matching the established
            `new_voucher.html`/`voucherEntries[i]` convention exactly - no
            new AJAX/JSON plumbing needed). Both post to the new ledger
            above with `active` re-checked, FK references re-resolved/
            nulled server-side. Sidebar: new "PARTY PROCESSING" top-level
            menu section added.
      - [ ] Deliberately NOT built this pass, to keep the slice coherent and
            reviewable rather than shipping the whole 30-class module
            unverified in one go:
            - invfrmStockConversionPartyProcessing (the raw-material-in ->
              processed-output-out conversion step) - would reuse the same
              new ledger (post CIN for inputs consumed, COUT for outputs
              produced) mirroring the existing recipe/BOM production
              pattern, but needs its own careful pass once GRN/GDN are
              confirmed working end to end.
            - StockAdjustment / StockOpeningBalancePartyProcessing - same
              ledger, ADJ/OPN transaction types already reserved for them.
            - frmWagesBillPartyProcessing / ProductionPartyProcessingBill -
              billing the third party for processing service rendered.
              Design intent: post via the EXISTING, already-tested
              `VoucherService.addOrUpdateVoucher` pipeline (Dr the
              supplier's linked `glAccount` / Cr a processing-income
              account) rather than inventing new GL-posting logic - same
              judgment call as FinancialYearClose. `addWages` is already a
              field on the GRN/GDN header ready to drive this.
            - frmGrnGdnStorePartyProcessing / PartyProGrnAndGdn - appear to
              be combined/alternate views over the same GRN+GDN screens
              just built; check for overlap before building anything new
              here.
            - GatePassInwardPartyProcessing - likely maps onto the existing
              `kanta` weighbridge subsystem the same way the main Sale/
              Purchase gate passes did; check that mapping before building.
            - StockAdjustmentPartyProcessingRegister /
              StockConversionRegisterPartyProcessing /
              ProductionRegisterPartyProcessing / WorkingReport /
              Dashboard.frmPendingWorksDetailPartyProcessing /
              Dashboard.frmPendingWorksDashboardPartyProcessing - these are
              registers/reports/dashboards over the transactions above;
              belongs with a Tier 3/4 reporting pass once the underlying
              transactions exist and have real data to report on.
            - frmProductionPartyProcessing / ProductionJobOrderPartyProcessing
              / LoadavailableStockPartyProcessing /
              LoadavailableTransactionsForIssuancePartyProcessing - job-
              order-costing-adjacent, same category as the Tier 2
              Production job-order engine already deferred.
            - AdvanceDeliveryOrderPP / LoadSaleInvoiceForADO /
              frmLoadExcessWeightGdnOnSaleInvoice - a secondary "reserve
              goods for later delivery" refinement on top of the core GDN
              flow, not foundational; lower priority.
- [x] Import (15 classes) — triaged all 15
      (frmMasterDocumentSerial, frmProformaDocumentSerial,
      frmProformaInvoice/ImProformaInvoice, frmImportInvoice, ImLcOrderSchedule,
      ImBillOfLading, ImCommercialInvoice, ImportGrnAgainstImportPurchaseOrder,
      ImForwardingGdn, ImLcOrder, ImPurchaseOrderShipmentPackingList,
      Transactions.frmShipmentBooking, Transactions.frmInvoicePackingDetail,
      Definition.frmSeaAirport): this is Letter-of-Credit / import-shipment
      documentation (LC order, bill of lading, commercial invoice, customs
      packing list) - the same shipment-documents cluster already on the
      top-level exclusion list as `ExImShipmentDocuments`. Treated as
      already excluded rather than re-building; flag if Golden Ace Rice
      Mills actually imports goods under LC and this exclusion is wrong.
- [ ] HRM (64 classes) — triaged at a high level (not class-by-class - the
      scale and risk profile warrant a dedicated engagement, not a pass
      folded into this session). This is a complete, standalone HR/Payroll
      system: employee master data, attendance/duty-roster, leave/overtime
      management, loan/advance management, and - the parts that raise real
      risk - (a) `DeviceManagement.PullAttendance`/`PullAttendanceByMachine`
      pull attendance from biometric fingerprint devices, which is the
      same hardware-integration category already excluded per the top-level
      scope ("fingerprint/biometric... explicitly excluded"); (b)
      `PolicyManagment.EOBIPolicy`/`IncomeTax`/`ProvidentFund`/
      `SocialSecurity` are Pakistani statutory payroll-deduction compliance
      rules - the same "getting it wrong has real legal/financial
      consequences" caution already applied to Tax_Definition; (c)
      `ApprovalManagement.*` (leave/overtime/advance/loan approval) is the
      same approval-workflow cluster already deferred under Configurations/
      the unified-voucher audit. Given all three, deferred as its own
      dedicated scoping pass rather than a partial build that would leave
      compliance-critical payroll math or biometric wiring half-done.
      Basic employee master data (EmployeeManagement/Employee_Management -
      registration, address, family info, bank account, education,
      experience) is the one sub-area that's genuinely low-risk simple CRUD
      and could reasonably be a first slice of that future pass - flagged
      here, not built now, to keep this session's scope honest about what
      "HRM" actually entails before committing to any of it.
- [x] Service (21 classes) — triaged all 21
      (EximServicesDefine, frmServiceBillHistory, ExImClearingAgentBill(Direct),
      ExImPreshipmentCosting, ExImSaleOrderForwarder, ItemPricingSupplierExport,
      Definition.frmServicesItemCategory, and the entire `lgstcm.*` subtree -
      frmLoadPendingFreightVoucherExportForServicesBill,
      frmLoadPurchaseOrderForServicesBill, frmLoadLogisticAgreementForPO,
      frmLogisticPurchaseServicesBill, frmLogisticPurchaseOrder,
      frmFreightVoucherExportMultiVehicles, frmFreightVoucherExport,
      frmLoadPendingGpForFreightVoucher, frmLogisticAgreement,
      frmLogisticRateNegotiation(Transporter), lgstcm.Reports.*): this
      entire module is export/import clearing-agent billing and freight-
      logistics management - `lgstcm` is literally LogisticManagement,
      already on the top-level exclusion list, and every `ExIm`-prefixed
      class is the same shipment-documents cluster already excluded as
      `ExImShipmentDocuments`. Treated as already excluded in full; flag if
      Golden Ace Rice Mills actually runs its own logistics/clearing-agent
      billing and this exclusion is wrong.
- [x] AccountToAccountTransfer (1 class) — already fully covered: this is a
      transfer between two of the company's own accounts (bank/cash), which
      is exactly what a Contra voucher already does on the existing unified
      voucher screen (`voucherType` = Contra, Dr one account / Cr another).
      No new screen needed - same precedent as the Tier 1 DayBook finding.
- [x] FixedAsset (4 classes) + AssetSchema (2 classes) — 6 classes total,
      built as the register + category master (no auto-posting of
      depreciation journal entries - see reasoning below):
      - [x] AssetSchema.frmFixedAssetsCategory → built `AssetCategory`
            (code/description/serial range + 3 default GL account FKs:
            asset account, depreciation-expense account, accumulated-
            depreciation account) + `AssetCategoryController` +
            `accounts/asset_categories.html`. Sidebar link added under
            ACCOUNTS, next to Cost Centers.
      - [x] FixedAsset.frmDefineAssets + FixedAsset.frmFixedAssetRegister +
            AssetSchema.frmAssetsRegister → built `FixedAsset` (name/
            serial no/category FK/status/condition/location/brand/
            manufacturer/model/department/optional item+branch FK/vendor
            FK/purchase date+price/current value/useful life months/
            expiry date/per-asset GL account overrides) +
            `FixedAssetController` + `accounts/fixed_assets.html`.
            Account FKs are re-resolved server-side via
            `accountService.getAccountByCode()` rather than trusted from
            the bound transient object, matching the established
            `ExchangeRateForwardBookingController` convention. Sidebar
            link added.
      - [ ] FixedAsset.frmFixedAssetRegisterHistory (value history /
            depreciation posting over time) - deliberately not built. This
            register tracks the asset and its *current* value, but does
            not compute or auto-post monthly/yearly depreciation journal
            entries. Same judgment call as Tier 1's BsPlSettingForm/
            FinancialYearClose: classify and track first, leave automated
            GL-posting logic (straight-line vs. reducing-balance method,
            posting frequency, period-lock interaction) for a dedicated
            follow-up rather than guessing at accounting-policy specifics
            with real money movement in an environment where I can't
            compile/test.

## Tier 3 — Reports & exports
Tier 3 status: fully triaged as of this pass. Existing coverage in
`src/main/resources/templates/reports/` (36 report screens + associated
StocksController/MSTReportController/ReportsController routes) already
maps to the overwhelming majority of the legacy Account_Reports/
Inventory_Reports namespaces - GeneralLedger, TrialBalance, BalanceSheet,
Profit&Loss (multiple variants), Customer/Supplier Status, PDC/Cheque
Detail, Cash/Bank, Stock Ledger, Stock Quantity (open/closing), Sales/
Purchase Report, Trading Report, Monthly Breakup, Expense Analysis, SOFP
Comparison, Pre-Purchase, Combine Voucher, Loan Management - so most of
this tier is confirm-and-check-off rather than build. The one genuine,
concrete, low-risk gap found and closed this pass is documented under
Account_Reports below.

- [x] Account_Reports (69) - reviewed the full class list against the 36
      existing report screens:
      - GeneralLedger/DetailGeneralLedger/MultiAccountsLedger/
        PartyLedger/BankSummaryLedger families -> `general_ledger.html`/
        `detail_general_ledger.html`/`multi_accounts_ledger.html`/
        `party_ledger.html`/`bank_summary_ledger.html` (existing, covered).
      - TrialBalance/TrialBalance8/BalanceSheet/SOFPComparison/
        FinancialStatement/PL/Profit&Loss/ProfitLossReport/
        CustomizeProfitReport/TradingReport -> existing equivalents in
        `reports/` (covered).
      - CustomerStatusReport/SupplierStatusReport/DayPayment/
        DailySummaryReport/MonthlyBreakupReport/ExpenseAnalysisReport/
        ChequeDetail/Reminders/Rate/RateEntries/LoanManagement/
        LoanReports/PrePurchaseReport/CombineVoucherReport -> existing
        equivalents (covered).
      - [x] frmInvoiceAgingPjv (Payables aging / "Payment Schedule") -
            already fully wired end-to-end: `StocksController.
            loadInvoiceAgingPvjScreen`/`invoiceAgingPjvReport` ->
            `reports/invoice_aging_pjv.html` -> `CustomerStatusReportService.
            getInvoiceAgingPjvReport` (PJ-voucher-driven, Trade Payable
            account, 6 aging buckets: 0-15/16-30/31-45/46-60/61-90/90+
            days). Sidebar link already present under PURCHASE ("PAYMENT
            SCHEDULE REPORT").
      - [x] frmInvoiceAgingReport (Receivables aging - THIS PASS'S BUILD) -
            this is the standout gap flagged at the top of this triage,
            tied to the deferred frmBillsPayables/frmBillsReceivables
            item and to Tier 1's `InvoiceWiseOpening` work (built to seed
            per-invoice AR/AP opening balances for exactly this kind of
            aging). Investigation found `CustomerStatusReportService.
            getInvoiceAgingReport` (SJ-voucher-driven, Trade Receivable
            account, identical 6-bucket aging structure to the PJV method
            above) already fully implemented in the service layer but
            never wired to any controller route or screen - dead code.
            Closed the gap by adding `StocksController.
            loadInvoiceAgingSjvScreen`/`invoiceAgingSjvReport` (mirroring
            the PJV controller methods exactly) -> new
            `reports/invoice_aging_sjv.html` template -> new
            `countx_invoice_aging_sjv.js`. Sidebar link added under SALE
            ("RECEIVABLES AGING REPORT").
            Judgment call: while wiring this up, found that the EXISTING
            `countx_invoice_aging_pjv.js` reads response field names
            (`one`/`two`/`three`/`four`/`five`/`...Percent`/`...Title`/
            `greaterNintyPercent`) that do not exist anywhere on the map
            `getInvoiceAgingPjvReport` actually returns (it only has
            `overFifteen`/`overthirteen`/`overFourtyFive`/`overSixty`/
            `overNinty`/`greaterNinty`/`advance`/`opening`/`total` plus
            per-bucket tooltip strings) - i.e. the live Payables aging
            screen appears to already render undefined/broken values in
            most of its columns. This is a pre-existing bug in code I
            didn't write and can't compile-test a fix for, so rather than
            copy the same broken field mapping into the new Receivables
            screen (or risk touching the live Payables screen without
            being able to verify a fix), I wrote a clean, correct
            `countx_invoice_aging_sjv.js` that reads the map's actual key
            names directly - it renders correctly-labelled buckets even
            though its sibling PJV screen currently may not. Flagging the
            PJV bug here rather than silently fixing it - worth a
            dedicated look (with the ability to test) if you'd like it
            fixed.
      - excluded: CoaTitleChange, dummyhistory (not real reports -
        housekeeping/dead classes). AcFrmDashboard/AuditDashboard belong
        to Tier 4 (Dashboards), not Tier 3 - will be picked up there.
      - excluded (industry mismatch, matches established exclusion list):
        any CommissionAgentLedger/Cmagt-namespaced report classes.
      - [ ] FreightVoucherRegister / frmJobOrderLedgerReport - tied to
            already-deferred items (InLandFreightAgreement's detail-grid
            simplification and Production's job-order-costing engine
            respectively) - deferred alongside those, not built standalone.

- [x] Inventory_Reports (57) · Inventory_Stocks_Report (23) - reviewed
      against existing `stock_ledger.html`/`stock_report_quantity.html`/
      `closing_stock_report_quantity.html`/`inventory_profitability_report.html`
      and the `com.mst.kanta` weighbridge subsystem:
      - Stock ledger/quantity/valuation/closing-stock/profitability
        families -> existing coverage (covered).
      - excluded (industry mismatch, matches established exclusion
        list): Steel/`_Engr`-suffixed, Paddy_Purchase/Ghalla-Mandi-
        namespaced, Trading-vertical-specific, PM (PackingMaterial_Store)
        -suffixed report classes.
      - [x] WeighBridge-adjacent classes (WeighBridge_WeightUpdate,
            frmWeighBridgeRejectedTicketNos, frmWeightBridgeHistory,
            frmGatePassVehicleEntryAndExitTimeAnalysisReport) - checked
            against existing coverage rather than blanket-excluding
            (kanta/weighbridge itself is in-scope, unlike the excluded
            verticals): existing `daily_weight_list.html` +
            `com.mst.kanta` `WeightEntry`/`ProductTravelRecord` screens
            already cover gate-in/gate-out weight capture and a daily
            list view; ticket-rejection tracking and a dedicated
            entry/exit time-analysis report are not currently built.
            Deliberately left unbuilt this pass (not core-ledger-risk,
            just genuinely new report logic over the kanta tables that
            deserves its own scoping pass rather than a rushed addition
            here) - flag if you want this prioritized next.

- [x] Export (75) · ExportReports (44) · ImportReports (5) - all 124
      classes excluded, matching the already-excluded
      ExImShipmentDocuments/LC/customs pattern (`ExIm`-prefixed and
      `Im`-prefixed namespaces) - export-shipment-documentation tooling,
      not core rice-mill accounting/inventory, already out of scope by
      the standing exclusion list.

- [x] HRM_Reports (12) - AttendenceStatusByDate,
      AttendenceStatusByEmployeeAndDepartment, EmployeeOverTimeRegister,
      EmployeeHistoryRpt, PayRollSalarySheetRpt, DailyAttendanceRpt,
      DailyLateandEarlyDeparture, genDSStrengthAttendence,
      genDutyRosterEmployeeWise, genEmployeeAttendence,
      MonthlyAttendanceSummary - deferred alongside the already-deferred
      standalone HRM module (Tier 2). No HRM master data/attendance
      engine exists yet in the web app for these reports to sit on top
      of, so building reports here would be premature. Revisit together
      with HRM if/when you want that module prioritized.

- [x] PartyProcessingReports (12) - frmStockReservedReport,
      GrnGdnStorePArtyProcessingRegister,
      StockTransferPartyProcessingRegister, PartyProcessingBillRegister,
      ProductionJobOrderPartyProcessing, GdnSaleToPP_Register,
      frmGrnPurchaseFromPP_Register,
      InventoryTransactionPartyProcessingReport,
      frmPartyProcessingGrnInfo, frmGatePassPartyProcessing,
      frmGRNGDNPartyProcessing, frmStockReportPartyProcessing - deferred
      alongside PartyProcessing's own already-deferred registers/reports/
      dashboard items (see Tier 2 PartyProcessing deep-dive notes) -
      several of these (StockTransfer, StockReserved, GatePass,
      ProductionJobOrder) depend on PartyProcessing sub-features (Stock
      Conversion, Wages Bill, GatePassInwardPartyProcessing) that were
      themselves deliberately deferred, so building the reports first
      would be reporting on data that doesn't exist yet.

- [x] Production_Reports (8) - frmProductionJobOrderSummaryRpt,
      frmProductionCostingReport,
      frmProductionPackingMaterialConsumptionRegister,
      GrnsPendingOrUsedinProduction, ProductionRegister,
      ProductionSummaryReport, FoodProductionComparisonRpt,
      frmProductionPendingPurchaseInvoice - deferred alongside
      Production's already-deferred job-order-costing engine (Tier 2) -
      these are all reports over job-order costing data that doesn't
      exist in the web app yet (Production module here only covers
      Production Plants/Plan Types master data + the existing Production
      Voucher/Recipe screens, not a full job-costing ledger).

## Tier 4 — Dashboards & approval workflow
- [ ] ApprovalDashboard (36) · [ ] MultiApprovalDashboard (9) · [ ] SpecialApprovalDashboard (6)
- [ ] Dashboard/DashboardNew (17) · [ ] AnalyticDashboard (11) · [ ] Audit_Dashboard (8)
- [ ] DynamicCards (49, bank-balance dashboard widgets)

### Tier 4 triage (class lists pulled from the .exe's own metadata, not yet built)

Tier 4 is a lot more entangled with already-excluded modules than Tiers 1-3
were, so a triage pass first (rather than diving straight into a build) is
worth writing down before committing real time to it:

- **Audit_Dashboard (8 classes)** — mostly NOT generic audit-log viewing:
  `PaddyStockBreakUpItemWise` (Paddy_Purchase, excluded),
  `GrnEbAndWtCutDeductionAudit` / `GrnWeightAuditReport` /
  `frmAuditByWeightReport` / `StockTransferWeightAuditReport` (weighbridge
  weight-cut audits, WeighBridgeGeneralLookups is excluded). Only
  `AuditLogReport` and possibly `StockClosingAndOpeningData` /
  `StockBreakupInAndOutSummary` (if "stock" here means finished-goods, not
  paddy) look in-scope. Given Hibernate Envers is already wired up in this
  app (fixed the NOT_AUDITED relation bug this session), a genuine
  Envers-backed "who changed what, when" viewer is a real, low-risk,
  self-contained candidate if this tier gets picked up — but it is a
  different feature than what most of this namespace's classes actually do.

- **DynamicCards (49 classes)** — over a third of these (`ABL`, `AskariBank`,
  `BankAlFalah`, `BankAlHabib`, `FaysalBank`, `HabibMetro`, `HBL`, `MCB`,
  `MeezanBank`, `NBP`, `NIB`, `SaadiqStandardCharteredBank`, `SilkBank`,
  `SoneriBank`, `BOP`) are one near-identical WinForms UserControl per
  Pakistani bank for a "bank balance summary card" - that's presentation
  duplication in the legacy code, not 15 distinct business concepts. A web
  rebuild should collapse these into one generic bank-balance-card component
  parameterized by bank/account, not 15 templates. The remainder mixes in
  export/booking-office/lab card types tied to already-excluded modules
  (`BookingOfficeDashboard_UserControl`, `ExportContractInfo`,
  `ExportDashboardHeaderInfo`, `LabPurchaseAnalyticCard`) with some
  genuinely core pieces (`AccountsDashboardCard`, `PendingWorkCards`,
  `DueDateAnalysisReceivableAndPayableCard`, `ItemAndSupplierWiseAnalyticalInfo`).

- **AnalyticDashboard (11) and Dashboard/DashboardNew (17+1)** are the
  cleanest in-scope clusters - purchase/sale analytics
  (`PurchaseAnalyticPeriodic*`, `SalesAnalyticsDashBoard`,
  `AvgRateByItemPopUp`, `AvgRateBySupplierPopUp`, `PurchaseRegisterPopUp`,
  `SaleInvoiceRegisterPopUp`) and stock/pending-work dashboards
  (`frmStockDashboard`, `frmPendingWorksRpt`, `frmPendingWorksDetail`). No
  excluded-module entanglement found in these two namespaces.

- **ApprovalDashboard (36) and MultiApprovalDashboard (9)** are the biggest
  and the most mixed: genuinely core approval flows exist
  (`frmVoucherApproval`, `PendingApprovalSaleInvoice`,
  `PendingApprovalPurchaseInvoice`, `PendingApprovalVouchersHistory`,
  `UnApprovedInvoicesAndVouchers`, `frmDayBookApproval`,
  `PurchaseOrderForApproval`/`PurchaseOrderForMultiApproval`,
  `PendingGRNForApproval`/`PendingGRNForMultiApproval` - GRN is already
  built) sitting alongside a large number tied to excluded modules
  (`frmExportReturnInvoiceApproval`, `frmExportSalesContractApproval`,
  `CommercialInvoiceApproval` - ExImShipmentDocuments; `LabSampleAnalysisForApproval`,
  `LabPurchaseAnalysisForApproval`, `frmThirdPartyLotInspectionApproval` -
  Lab/QCL; `PendingApprovalGatePassOutward*` -
  GatePassInwardPartyProcessing; `PendingApprovalLabourWages` -
  Contractor_Wages) or HR-adjacent and therefore deferred alongside HRM
  (`PayRollPostingForMultiApproval`, `DepartmentRequestApproval`).

**Suggested starting point if/when this tier is picked up:** a single
generic "Pending Approvals" hub (mirroring `ApprovalDashboard.ApprovalDashboard`,
the actual hub class) that lists pending vouchers/sale invoices/purchase
invoices/GRNs awaiting sign-off and lets an authorized user approve/reject,
built against the voucher/GRN entities that already exist in this app -
rather than porting all 45+ legacy classes one-for-one. Not started yet;
flagging for a decision on scope before committing build time, the same way
Tax_Definition was flagged rather than rushed.

- [x] **Pending Approvals hub — BUILT.** `/vouchers/pending_approvals`
  (`VouchersController.loadPendingApprovalsScreen`, template
  `vouchers/pending_approvals.html`, JS `countx_pending_approvals.js`),
  linked from the sidebar under VOUCHERS > VOUCHER LIST > PENDING APPROVALS.
  Scope: vouchers only (not sale/purchase invoices or GRNs — those don't yet
  have their own posted/unposted approval concept in this codebase distinct
  from the voucher they generate), since `Voucher.posted` is the one real
  "finalized" flag in this app today (`VoucherStatus` is vestigial, always
  "M"). Reused 100% of existing infrastructure rather than inventing new
  backend: the existing `/vouchers/voucher_list` POST endpoint
  (`VoucherService.getAllVouchersByFilters`, `VoucherRequest`/
  `VoucherResponse`/`VoucherDetail` contracts) supplies the filtered list,
  and the existing `GET /vouchers/post_voucher?voucherId=X` toggle endpoint
  is the approve/un-approve action — no new controller logic, no new
  entities, no new columns. The screen is a lightly reskinned copy of
  `voucher_list.html`'s filter+DataTable pattern (same
  Company/VoucherType/VoucherStatus/Narration/VoucherNo/SearchByDate/From-To
  filters, required because `getAllVouchersByFilters` dereferences
  `company.getId()`/`voucherType.getId()`/`voucherStatus.getId()` directly —
  "ALL" is sent as a real `{id:"0"}` sentinel object, never `null`, matching
  the existing native-query `(:companyId=0 or ...)` pattern), with two
  differences: the posted/unposted filter defaults to `U` (UNPOSTED/PENDING)
  instead of BOTH, and each row gets an APPROVE/UN-APPROVE button (styled by
  `voucher.postedUnPosted`) that calls the existing toggle endpoint via
  `$.confirm` (matching the confirm-dialog pattern already used in
  `countx_contact_list.js`) and reloads the list on success. List loads
  automatically on page open (no need to press SEARCH first) since the
  whole point of a pending-approvals hub is to show the backlog immediately.
  Not yet built: sale-invoice/purchase-invoice/GRN-specific approval views —
  deferred until/unless those workflows are shown to need an approval gate
  distinct from the voucher they're built on.

  **Grounded against the real .exe logic (IL-extracted, not guessed).**
  `Architecture.WinApp.ApprovalDashboard.PendingApprovalVouchersHistory` (the
  actual nested legacy form for this screen, found via IL disassembly of
  `ECCOUNTBOOKERP.exe`) exposes `ApproveVoucher(id)` / `UnApproveVoucher(id)`
  (single row, from the pending-view grid's inline "Post" column button) and
  a checkbox-bulk `btnApproved_Click` (validates at least one row checked,
  raising "Please select check box first" otherwise; one confirm dialog and
  one success message for the whole checked batch). Both paths build an
  `Architecture.Model.ApprovalList` record (OrganizationId, CompanyId,
  voucher Id, PostDate=now, EntryUser=current user, ReqType "AP"/"UP") and
  call `Architecture.BLL.Accounts.VoucherHead.UpdateStatusandIsapprovedByVoucherId`
  — i.e. the legacy app keeps its own manual approval-audit-trail table.
  This web app gets the same "who/when changed the posted flag" for free
  from Hibernate Envers (`Voucher` is already `@Audited`), so no new
  ApprovalList-equivalent entity was built — extending would have
  duplicated infrastructure this app already has. What WAS changed to match
  the real form: confirm-dialog and success-message text now match the
  legacy strings verbatim ("Are you sure to Approve Voucher?" / "Are you
  sure to UnApprove Voucher?", title "Confirm"; "Record Approved
  Successfully" / "Record UnApproved Successfully"), and the screen gained
  a checkbox column + "select all" + a single bulk APPROVE/UN-APPROVE
  button (mode follows the current P/U filter, mirroring the legacy form's
  two tabs "Vouchers For Approval" / "Vouchers For UnApproval") alongside
  the existing per-row quick button (which mirrors the legacy pending
  grid's inline "Post" column button). Not ported: the legacy form's
  master-detail voucher-line grid, GL drill-through from account cells, and
  attachment-count column — these are print/navigation conveniences on top
  of the same approval action, not the approval logic itself, and the
  existing VIEW link already gets the user to the full voucher detail.

  **Follow-up check on the two other Tier 4 triage items closest to this
  build** (`PendingApprovalSaleInvoice`/`PendingApprovalPurchaseInvoice` and
  `PurchaseOrderForApproval`/`PurchaseOrderForMultiApproval`), done after
  finishing the exe-grounding pass above so the triage note reflects what's
  actually true of this codebase rather than an assumption:

  - [x] **Sale/Purchase Invoice approval — already covered, no new screen
    needed.** In this web app SJV/PJV (and PRV/SRV/STV/PQV) are rows in the
    same `voucher` table as BPV/BRV/CPV/CRV/JV/OJV, distinguished only by
    `voucher_type_id` (confirmed via `countx_vouchers_list.js`'s
    voucher-code-prefix VIEW-link routing and the shared
    `getAllVouchersByFilters` query) — there is no separate Invoice-approval
    entity or endpoint in the legacy sense. The Pending Approvals hub built
    above already reaches these: pick the specific voucher type (PJV, SJV,
    etc.) in the VOUCHER TYPE filter. One caveat carried over unchanged from
    the pre-existing `voucher_list.html` screen (not introduced by this
    build): the "ALL" voucher-type option's native query
    (`IVoucherRepository.findVoucherListByVoucherDate` et al) only expands
    to `('BPV','BRV','CPV','CRV','JV','OJV')`, so PJV/SJV/PRV/SRV/STV/PQV
    never appear under "ALL" — a user must pick the inventory voucher type
    explicitly. Not changing that shared query as part of this task since
    it's relied on by the existing Voucher List screen too.

  - [ ] **Purchase/Sale Order approval — real gap, not started, flagging
    for a scope decision rather than rushing it (same reasoning as
    Tax_Definition).** Checked `PurchaseOrder.java`/`SaleOrder.java`: both
    have a `pending` boolean, but it means "not yet fully booked into a
    voucher" (`PurchaseOrderService.java:566/571` — set true on creation,
    false once quantities are fully booked), not an approval gate; the only
    status-shaped field is `voucherStatus`, which is the same vestigial
    field noted elsewhere in this app (effectively unused). Unlike the
    voucher approval build above, there is no existing "posted" flag or
    equivalent to reuse here — supporting real PO/SO approval would mean
    adding a new field plus service logic, which is a materially bigger
    decision than extending something that already exists. Deferred pending
    explicit direction on scope.

## Tier 5 — Utilities & misc
- [ ] Helper (13) · [ ] DataSyncing (9) · [ ] Templates (10) · [ ] ModernHistories (13)
- [ ] Common (29) · [ ] SystemUtilities, LicenseKey, ChangePassword, BackUp*, DMS_FolderHierarchy
- [ ] Everything else not itemized above, triaged as we reach it

## Explicitly excluded (industry mismatch — confirm if wrong)
Steel, FeedMill, Salt, ExportSalt, PurchaseForSalt, SaleForSalt, Cmagt, CmTr,
CommissionAgent, pcc (precast concrete), Contractor_Wages, PackingMaterial_Store,
WholeSale, Paddy_Purchase, SDT/SDT_Reports, lgstcm/LogisticManagement, IPM,
ReturnableInventoryManagement, PreBookingAndDelivery, BookingOffice, QCL,
LabAnalysis, DigitalCameraView, WeighBridgeGeneralLookups, RetailInventoryReports,
RetailStockReports, ExImShipmentDocuments

## Ground-truth sources for this rewrite

Two independent sources of real legacy ground truth are now available,
in addition to reading the actual screenshots/behaviour of the running
.exe:

1. **IL-extracted metadata/disassembly** of `ECCOUNTBOOKERP.exe` itself
   (cloud-container-only, `/tmp/exe_analysis/` - `dotnet_meta.py` +
   `il_disasm.py` + `il_by_namespace/*.txt` per-class IL dumps). Gives real
   field/method names and business logic (validation strings, BLL calls,
   confirm/success message text) straight from the compiled WinForms code.
   Not part of the deliverable codebase.

2. **The real legacy database schema** (`D:\EccountBookERP\Gscript.sql`,
   a ~131MB SQL Server "Generate Scripts" dump of the `GoldenAcedb_Testing`
   database, UTF-16LE encoded, schema-only - CREATE TABLE/INDEX/FK
   statements, no data). Provided by the user 2026-09-03. Converted for
   grep/analysis on the device shell (not committed to the repo - it's a
   131MB generated artifact, not source):
   `iconv -f UTF-16LE -t UTF-8 ~/mnt/EccountBookERP/Gscript.sql > ~/Gscript_utf8.sql`
   (drops to ~66MB). Table index:
   `grep -oE "CREATE TABLE \[[^]]+\]\.\[[^]]+\]" ~/Gscript_utf8.sql | sort -u`
   1400 tables total. Schema-name breakdown maps directly onto this plan's
   existing exclusion list, which is a good sanity check that the scope
   boundaries drawn from UI/IL alone were right:
   `dbo` 1043 (core + everything not otherwise namespaced), `pcc` 57
   (precast concrete, excluded), `fed` 43 (FeedMill, excluded), `cmagt` 32
   (CommissionAgent, excluded), `ImEx` 29 (ExImShipmentDocuments, excluded),
   `ST` 28 (Sales/delivery-order side, e.g. `ST.InvDeliveryOrder` -
   in-scope, not "Steel"), `Mfg` 27, `CmTr` 23 (excluded), `hrm` 19
   (deferred with HRM), `crm` 17, `lgstcm` 16 (LogisticManagement,
   excluded), `asset` 12, `Fcm` 11, `sdt` 9 (excluded), `item` 6, `Tax` 6
   (Tax_Definition, deferred), `DMS` 5, `DAW` 5, `bank` 4, `account` 3,
   `qcl` 2 (excluded), `mrp` 2, `inventory` 1.

   First concrete correction this enabled: `dbo.ChartofAccount` (the real
   production chart-of-accounts table - not `account.ChartOfAccount`,
   which is a newer/parallel schema-namespaced table that appears unused by
   the actual running app) confirmed most of `Account.java`'s
   AcfrmDefCoa-derived extension fields are right (AccountCode,
   AccountTitle, PLNoteId/BSNoteId -> plNote/bsNote, OtherErpCode ->
   otherCode, CityId -> city, ContactNo -> phone) but that `AccountTypeId`
   is a real FK (into `dbo.AccountTypes`: Id/AccountType/Abbreviation/
   Description), not free text. Fixed: added `AccountType` entity +
   repository + service (mirrors `FinancialStatementNote`'s existing
   pattern exactly) and changed `Account.accountType` from `String` to
   `@ManyToOne AccountType`; `define_accounts.html`'s Account Type dropdown
   now reads `${accountTypes}` instead of a hardcoded 5-value list.
   `accountClass` was NOT changed (left as free text) - `ChartofAccount.
   AccountClassId` is also a real int FK in the legacy schema, but no
   dedicated AccountClass-style master table was found in a first pass
   (searched for `%class%` table names in `dbo`/`account` - only
   unrelated hits like `ItemClass`), so the real source for that FK is
   still unresolved; flagging rather than guessing a second lookup table.
   Fields seen on the real `ChartofAccount` that this web app's `Account`
   still doesn't have: `QrCode`, `BranchId`, `CurrencyId`,
   `SubSidiaryAcStatus`, `AccountTitleOtherLingo` (multi-language title,
   via a separate `ChartOfAccountMultiLingo` table) - none built yet, none
   blocking, noted for whenever this screen gets revisited. Also confirmed
   opening balances live in a separate `dbo.AccountsOpeningBalances` table
   in the legacy app, not inline on the account row - this web app's
   `Account.openingBalance` column is a simplification (single value, no
   per-financial-year history) rather than a straight port; left as-is
   since `/accounts/invoice_wise_openings` already exists as this app's
   opening-balance screen and rearchitecting into a history table is a
   bigger call than this pass warrants.

   Also cross-validated the Pending Approvals hub build against this
   schema: `dbo.VoucherHead`/`dbo.VoucherDetail` are the real legacy
   voucher tables, matching the `Architecture.BLL.Accounts.VoucherHead`
   class found via IL - the two ground-truth sources agree with each
   other here, which is a good sign for both.

   This schema dump is the single best source of truth available for this
   whole rewrite and should be checked before building or extending any
   further screen, the same way the working method below already says to
   check the legacy class - now "find legacy class" should mean checking
   both the IL dump AND this schema, not just one.

## Full column parity pass (Tasks #21-38, 2026-09-03)

Following the ground-truth sources above, the user asked to systematically
review every existing Java entity's columns against the real legacy SQL
schema and add back the legacy audit/multi-tenant columns this web app's
simplified master-data entities had dropped (`OrganizationId`,
`EntryUser`/`ModifyUser` int FKs, `PostState`, etc.) - explicit scope
answer: **"Full column parity everywhere"**, i.e. column parity even where
this app doesn't currently use the data, not just a bug-hunt pass.

Method used for each entity: look up the real table in `~/tables_parsed.json`
(see Ground-truth sources above), diff its column list against the current
Java entity's fields, add ONLY genuinely new columns (skip anything already
covered by an existing field under a different name - e.g. legacy
`BrandName` vs this app's `name`), verify brace balance after each edit
(Maven can't run in this environment - see Working method below), commit no
git operations (git is extremely slow/near-unusable on this mounted device
folder - `git status`/`git diff`/`git stash` all timed out at 120s+ during
this pass; rely on direct file reads + brace/tag balance checks instead).

Type-mapping convention: SQL `NOT NULL int/bigint` -> Java primitive
`int`/`long`; `NULL int/bigint` -> `Integer`/`Long`; `bit` -> `Boolean`;
`datetime` -> `java.util.Date`; `nvarchar`/`varchar` -> `String`;
`numeric`/`decimal`/`float` -> `BigDecimal`; `varbinary` (e.g.
`Company.CompLogoImage`) skipped/deferred rather than modeled as `@Lob
byte[]` this pass. New FK-shaped columns (e.g. `OrganizationId`,
`RevenueAccountId`) are added as plain `Integer` id columns, NOT `@ManyToOne`
relations, matching the existing convention already used for fields like
`Account.organizationId`-style columns elsewhere in this codebase - keeps
the change additive/low-risk since nothing in this app reads these yet.

Entities patched (all verified brace-balanced after edit):

- **BankName** (`bank.BankName`): added `bankCode`, `checkTemplateId`,
  `active` (manual getters/setters - this file predates the Lombok
  convention used elsewhere).
- **Brand** (`dbo.Brand`): added `brandCode`, `entryDate`, `entryUserId`,
  `modifyDate`, `modifyUserId`, `organizationId`, `companyId`.
- **BusinessType** (`dbo.BusinessType`): added `description`, `entryDate`,
  `entryUserId`, `modifyDate`, `modifyUserId`, `organizationId`,
  `companyId`.
- **City** (`dbo.City`): added `code`, `cityNameOtherLingo`, `countryId`,
  `entryDate`, `entryUser`, `modifyDate`, `modifyUser`, `postDate`,
  `postUser`, `postState`, `organizationId`, `companyId`, `tehsilId`.
  Note: this entity (and District) have no controller wiring anywhere in
  the codebase currently - `parentCode` is unused dead scaffolding, not a
  real Country/District cascade; left as-is, just added the missing legacy
  columns without guessing at intended FK semantics.
- **District** (`dbo.District`): added `code`, `entryDate`, `entryUser`,
  `modifyDate`, `modifyUser`, `postDate`, `postUser`, `postState`,
  `organizationId`, `companyId`, `stateProvinceId`. Same "currently unused"
  caveat as City.
- **Company** (`dbo.Company`, highest-value gap - ~30 legacy columns vs.
  ~15 existing fields): added `orgCompanyTypeId`, `compCode`,
  `compContactPerson`, `compEmailB`, `compTel`, `compMobileA`,
  `compMobileB`, `compMobileC`, `compReportingTitle`, `compLogo`,
  `compCountry`, `compState`, `compBaseCurr`, `compType`, `pictureUrl`,
  `entryUser`, `modifyUser`, `entryDate`, `modifyDate`,
  `allowedUserCount`, `companyTypeId`, `companyFaxNo`,
  `companyNameOtherLing`, `companyAddressOtherLing`, `isHeadOffice`,
  `companyNameOtherLanguage`, `companyAddressOtherLanguage`,
  `companyTemplateId`, `cityName`, `parentCompanyId` (legacy `CompanyId`
  self-ref, renamed to avoid clashing with the unrelated `companyId` FK
  convention used on other entities), `bookingOfficeAllowedUserCount`,
  `customerPortalAllowedUserCount`, `mobileAllowedUserCount`,
  `clientsNameForReports`. Skipped as dupes: `CompName`->`name`,
  `CompAddress`->`address`, `CompanyWebsite`->`website`,
  `CompEmailA`->`email`. Skipped: `CompLogoImage` (varbinary, deferred).
  Note this app's existing Company fields (`sms`, `chromeAutoMate`,
  `sendKanatWeight`, chart/dashboard settings, etc.) are this web app's own
  additions with no legacy equivalent - left untouched, purely additive
  pass.
- **CostCenter** (`dbo.CostCenter`): added `entryDate`, `entryUserId`,
  `modifyDate`, `modifyUserId`, `organizationId`, `companyId`.
  `ParentCostCenterId` was already modeled correctly via the existing
  `@ManyToOne parentCostCenter` relation - no change needed there.
- **Country** (`dbo.Country`): added `code`, `entryDate`, `entryUser`,
  `modifyDate`, `modifyUser`, `postDate`, `postUser`, `postState`,
  `organizationId`, `companyId`.
- **DeliveryTerm** (`dbo.DeliveryTerm`): added `valueDescription` (only
  genuinely new column - `Id`/`Description` already covered).
- **FinancialYear** (`dbo.FinancialYear`): added `entryDate`, `entryUser`,
  `modifyDate`, `modifyUser`, `postDate`, `postUser`, `postState`,
  `organizationId`, `companyId`, `defaultFinYear`, `loginStatus`.
  Existing `name`/`fromDate`/`toDate`/`closed` already cover
  `FinancialYearCode`/`Start_Period`/`End_Period`/`Status`; `closingDate`
  is this app's own `frmFinancialYearClose`-derived addition, not a direct
  legacy column, kept as-is.
- **ItemCategory** (`dbo.ItemCategory`, complex - ~27 legacy columns,
  mostly GL-account FKs for fixed-asset/depreciation accounting): added
  `active`, `serialFrom`, `serialTo`, `revenueAccountId`, `cgsAccountId`,
  `inventoryAccountId`, `accumulatedDepreciationAcId`,
  `depreciationExpenseAcId`, `capitalWipAcId`, `entryDate`, `entryUser`,
  `modifyDate`, `modifyUser`, `organizationId`, `companyId`,
  `inventoryParentCategoriesId`, `assetGlAcId`, `itemClassGroupId`,
  `itemProductionStageId`, `itemVarietyNatureId`,
  `expenseMaintenanceAccountId`, `usefulLifeInMonths`, `depreciationRate`,
  `depreciationMethodScheduleId`, `assetCategoryId`. Note: legacy `Code`
  column is `nvarchar` but this entity's existing `code` field is `long`
  (used for a unique index) - left the type mismatch alone since fixing it
  risks breaking existing code/JS that expects a numeric code; flagging
  here rather than silently changing an in-use field's type.
- **ItemGroup** (`dbo.ItemGroup`): added `organizationId`, `companyId`
  (only new columns - `GroupId`/`ItemGroupName` already covered).
- **ProductType** (`dbo.ProductType`): added `productTypeCode`,
  `entryDate`, `entryUserId`, `modifyDate`, `modifyUserId`,
  `organizationId`, `companyId`, `financialYearId`.
- **ProductionPlanType** (`dbo.ProductionPlanType`): added `planTypeCode`
  (only new column).
- **VehicleType** (`dbo.VehicleType`): **no changes** - legacy table is
  just `Id`/`VehicleDescription`, already fully covered by existing
  `id`/`name`. Verified as a true no-op, not skipped.

Not yet done (tracked as open tasks, large remaining scope):
- **~105 other entities** (Account* hierarchy, Voucher*, PurchaseOrder,
  SaleOrder, User, Invoice, and everything else not in the 15 confirmed
  above) still need their real legacy table identified and column-diffed.
  An automated `difflib.get_close_matches` fuzzy-match pass was tried and
  abandoned as too unreliable for unattended edits (e.g. matched "Account"
  to "UserAccount"/"AccountTypes" instead of the correct
  `dbo.ChartofAccount`) - each of these needs manual verification of the
  correct legacy table before any column addition, the same way
  `dbo.ChartofAccount` vs `account.ChartOfAccount` was resolved above.

## Task #36 findings: automated matching exhausted, all leads false positives (2026-09-03)

Ran the normalized-exact-name match (bare Java class name vs. bare legacy
table name, ignoring schema) across all 120 entities not yet covered by the
prior column-parity pass. Only 4 hits, and every one turned out to be a
false positive on closer inspection - a useful negative result, recorded
here so a future pass doesn't repeat the same dead ends:

- **CustomGroup** -> `sdt.CustomGroup` - `sdt` is the SDT schema, which is
  on this project's exclusion list. Not a real match; this app's
  `CustomGroup` (used for Account/Party Custom Group screens) has no
  confirmed legacy counterpart yet.
- **PaymentTerm** -> `ImEx.PaymentTerm` - `ImEx` is the
  ExImShipmentDocuments schema, also excluded. Same situation.
- **PurchaseOrder** -> `dbo.PurchaseOrder` (also `fed`/`pcc`/`ST` variants,
  all excluded schemas) - `dbo.PurchaseOrder` looked like the right
  in-scope match at first, but reading the real column list (65 columns:
  `DocumentTypeId`, brokerage/commission fields, FCY/currency, container
  and export-invoice fields, lab-sample linkage, etc.) against this app's
  actual `PurchaseOrder.java` shows they are different documents that
  happen to share a name. This app's `PurchaseOrder`/`PurchaseOrderEntry`
  model a **ton/kg weight-based payment milestone schedule against a
  MillKhata account** (fields: `paymentDate`, `paymentType`, `ton`, `Kg`,
  `payPercent`, `inWeek`, `after`, `vehical`, tied to a `millKhata`
  relation) - it is NOT a commercial purchase order with item/qty/rate/tax
  lines. `dbo.PurchaseOrderDetail` (the legacy line-item table, 48 columns
  of item/UOM/rate/tax/lab-sample/currency fields) has no resemblance to
  this app's `PurchaseOrderEntry` at all. **Do not merge these schemas** -
  same trap as the earlier fuzzy-matcher abandonment, just caught by exact
  name match instead of fuzzy match this time. This app's PurchaseOrder
  feature is most likely this project's own port of a different legacy
  screen (a paddy-purchase advance/payment-schedule form) under a
  coincidentally identical class name - which real legacy screen that is
  has not been identified yet.
- **SaleOrder** -> `dbo.SaleOrder` (+ `fed`/`pcc`/`ST` variants) - same
  situation as PurchaseOrder: `dbo.SaleOrder` is a 71-column commercial
  sales-order document (export order no., container count, FCY, discount/
  tax headers, etc.); this app's `SaleOrder.java`/`SaleOrderEntry.java`
  needs the same "what does this app's entity actually model" check before
  any column is borrowed from `dbo.SaleOrder`/`dbo.SaleOrderDetail` - not
  done yet, flagging rather than guessing.

**Conclusion**: both the fuzzy matcher (abandoned earlier) and now the
exact-name matcher have failed to produce any safe, ready-to-apply column
addition for the remaining ~99 entities. The only way to make further
progress here is the slow path: for each entity, read what it actually
models in this codebase (fields + where it's used) first, THEN search the
IL dump / schema for the real legacy screen it corresponds to (which may
have a completely different class/table name), rather than trusting either
fuzzy or exact name similarity. This is a much bigger time investment per
entity than the master-data lookups in the prior pass and was paused here
pending direction on how deep to go, rather than guessed at with a name
match already shown to be unreliable twice.

## Task #36 progress: batches 1-2 (2026-09-03, continued)

Per the user's direction ("Continue slow manual pass"), continued entity-by-
entity verification - reading what each Java entity actually models first,
then hunting the real legacy table (not trusting name similarity alone,
per the PurchaseOrder/SaleOrder false-positive above).

**Batch 1 - patched** (all brace-balanced): `AccountLevelOne`/`Two`/
`Three`/`Four` vs `dbo.AllLevelChartOfAccount` (a flattened multi-level
reporting view, not a true per-level source table - legacy's real COA is
a single self-referencing `dbo.ChartofAccount`; this app splits it into 4
level tables instead - added `accountGroup`/`organizationId`/`companyId`
per level, confirmed actively used across `AccountsController`,
`CustomGroupController`, `ReportsController` etc. before touching).
`CropYear` vs `dbo.InvCropYear` (+`defaultKey`, audit/org/company cols).
`Currency` vs `dbo.MultiCurrency` (+`currencyRate`, `url`, full audit/
post/org/company cols, `baseCurrencyId`). `BoilerName` vs `dbo.Boiler`
(+org/company only). `PackingType` vs `dbo.InvPackingType` (+`packTypeCode`
+ 4 empty-bag min/max weight fields). `Province` vs `dbo.StateProvince`
(+code, full audit/post/org/company/countryId - confirmed live via
`GeographyController`, unlike City/District). `Rack` vs
`dbo.invWarehouseRack` (+sortNo, entry/modify audit). `Warehouse` vs
`dbo.InvWareHouse` (+entry/modify audit, org/company, warehouseType).
`Reason` vs `dbo.SpecialApprovalReason` - genuine no-op, legacy table is
just `Id`/`SpecialApprovalReason`, already fully covered (this app's
version even has an extra `category` field beyond legacy).

**Batch 2 - patched**: `AssetCategory` vs `dbo.FixedAssetsCategory`
(+expenseMaintenanceAccountId only - this entity already mirrored legacy
closely, including modeling the GL account fields as proper `@ManyToOne
Account` relations rather than raw ints). `FixedAsset` vs
`dbo.FixedAssetsRegister` (+assetsType, pic1Path, pic2Path, organizationId,
companyId, projectsId, entryUserId, entryDate, isApproved, approvedUserId,
approvedDate, expenseMaintenanceAccountId - also already closely mirrored
legacy otherwise). `Branch` vs `dbo.Branches` (+branchCode,
branchContactPerson, branchEmailB, branchTel, branchMobileA/B/C,
branchReportingTitle, branchLogo, organizationId, entryUserId, entryDate,
modifyUserId, modifyDate - notable finding: `Branch.java` had **no audit
columns at all** before this, unlike almost every other entity in this
codebase).

**Batch 2 - false positive, NOT patched**: `Module` vs `dbo.AppModules` -
both are "module registry" concepts but structurally different systems:
this app's `Module` is a Spring-based menu/permission entity (`moduleName`,
`roleName`, `showAsTab`/`asSubRoute`/`showSubRoute`, a `List<Label> menu`
many-to-many) driving this app's own role-based nav; `dbo.AppModules`
(`ModuleDescription`, `ModelMenuControllName`, `IconUrl`, `SortNo`,
`ModuleTypeId`, `AppId`, `CompanyTypeId`) is legacy's own dynamic-menu
registry for a completely different (WinForms) navigation system. Same
caution as PurchaseOrder/SaleOrder - flagged, not merged.

**Searched, no confident legacy table found yet** (need a proper IL-dump
cross-check, not just a schema name search, before any of these can be
patched): `BankAccount` (only hit was `dbo.hrmEmployeeBankAccount`, an HR
module table for employee payroll bank details - wrong concept), `Broker
Account`, `ChequeBook`, `ChequeDetail`, `PostDatedCheque`, `VoidedCheque`
(no schema hits at all under any of these names - legacy may use different
terminology, e.g. "Cheque" vs "Check" spelling, or these may be handled as
voucher-type flags rather than dedicated tables), `ExpanceType`/
`ExpanceEntries` (this app's own spelling of "Expense" may not match
legacy's spelling either), `DailyClosing`/`DailyClosingEntry` (no hits).

**Identified as needing deeper investigation before any column work**
(schema search returned too many candidates or the concept is ambiguous,
not because no legacy analog exists): `Invoice`/`InvoicePayment` (the
`dbo` schema alone has 20+ Purchase/Sale invoice-shaped tables -
`InvPurchaseInvoice`, `InvSaleInvoice`, `H_InvPurchaseInvoice`, etc. - need
to read what this app's `Invoice.java` actually models before picking
one), `User`/`UserAccountAccess` (legacy has both a `dbo.AspNetUsers`-style
ASP.NET Identity schema and various custom user tables - this app's `User`
is its own Spring Security entity, likely NOT a direct port the way
`AssetCategory`/`FixedAsset` were), `Loan` (candidates are
`dbo.FmLoanRegistrationHeader`/`Detail`, `dbo.FmLoanAgreementHeader`/
`Detail` - "Fm" prefix unclear, needs checking whether this is in-scope
core accounting or a deferred module), `ItemDef`/`ItemSubCategory` (no
exact-name hits - legacy likely uses a different root term for "item",
e.g. bare `Item` or `InvItem*` - needs a broader search before assuming
no match exists).

Continuing this list is the natural next step for a future session/pass;
paused here as a reasonable checkpoint after two solid, verified batches
rather than pushing further into the higher-ambiguity entities (Invoice,
User) without more investigation time.

## Task #36 progress: batch 3 (2026-09-03, continued)

Continued the slow manual pass on the entities flagged in batch 1-2 as
"needs deeper investigation" or "no confident legacy table found yet" -
reading each Java entity's actual fields/usage first, then searching
`tables_parsed.json` for the real legacy analog (not trusting name
similarity alone).

**Patched** (all brace-balanced):

- `Loan.java` (table `bank_loan`) vs `dbo.FmLoanAgreementHeader` - genuine
  match once found (the earlier "FmLoanRegistrationHeader" candidate was
  a collateral-registration concept, wrong fit; `FmLoanAgreementHeader`'s
  BankId/FacilityCode/InterestRate/InterestType/ApprovedAmount/ValidDays
  lines up with this entity's bankName/loanNo/interestRate/interestType/
  loanAmount fields). Added `bankId`, `facilityCode`, `fmFundTypesId`,
  `approvedAmount`, `validDays`, `organizationId`, `companyId`,
  `branchesId`, `projectsId`, `isApproved`, `approvedUserId`,
  `approvedDate`. Skipped `BankGLAccountId`/`ExpiryDate`/`EntryUserId`/
  `EntryDate`/`ModifyUserId`/`ModifyDate` as already covered by this
  entity's existing `bankAccount` relation, `maturityDate`, and Spring
  `@CreatedBy`/`@CreatedDate`/`@LastModifiedBy`/`@LastModifiedDate` audit
  columns respectively (`pay_LoanDetail`/`pay_LoanRepayment` - the other
  two "Loan" hits - are `pay_` schema, i.e. payroll/HR loans to
  employees, a different concept entirely, not used).
- `ItemDef.java` (table `item_def`, the real item master) vs `dbo.Item`
  (98 columns spanning many industries/modules). Deliberately patched
  only the rice-mill-core-relevant subset and skipped everything tied to
  excluded modules (Retail*/WholeSale pricing columns, GST/VAT/Excise tax
  flags - Tax_Definition is a separate pending task #16, import/
  third-party/barcode-image trading metadata, QC grade - QCL/LabAnalysis
  excluded, depreciation GL accounts - those belong on FixedAsset/
  ItemCategory which already got them). Added `minStockLevel`,
  `maxStockLevel`, `optimalStockLevel`, `purchaseGlAccountId`,
  `saleGlAccountId`, `cogsGlAccountId`, `cropYearId`, `barcodeNo`,
  `entryDate`/`entryUserId`/`modifyDate`/`modifyUserId`/`postDate`/
  `postUserId`/`postState` (ItemDef had none of these before), 
  `organizationId`, `companyId`, `branchesId`, `projectsId`,
  `itemAliasName`, `itemSpecification`, `emptyBagWeight` (rice-mill bag
  tare weight - clear domain fit), `itemNameOtherLingo`, `reorderQty`,
  `weightSemiFinish`, `weightFinishGoods` (paddy->rice milling yield
  tracking - clear domain fit; skipped the third sibling column
  `WeightSemiFinishSand`, which reads as belonging to the excluded
  precast-concrete vertical). Confirmed `rackId`/`itemCategoryId` were
  unnecessary additions - `ItemDef` already has `rack` and
  `itemSubCategory`->`itemCategory` relations covering those.
- `ChequeBook.java` (table `cheque_book`) vs `dbo.CheqBookHeader` - solid
  match (this entity's `chequeBookName`/`bankAccount`/`branch`/`prefix`/
  `remarks`/`startNo`/`endNo` line up with `CbPrefix`/`CbSrFrom`/
  `CbSrTo`). Added `chartOfAccount` (Account relation, legacy has both a
  `BankId` and a separate `ChartOfAccountId`), `docNo`, `docDate`,
  full entry/modify/post audit trio, `organizationId`, `companyId`,
  `actionId`.
- `PostDatedCheque.java` (table `post_dated_cheque`) vs
  `dbo.PdcInventory`/`dbo.PdcInventoryHeader` - this entity's own
  doc-comment already states it was ported from the legacy
  "AcfrmDefPdcManagment / PostDatedCheqPaymentVouchers" screens, so this
  match was already known/intentional, just not yet column-complete.
  Added `bankBranch`, `branchCode`, `chequeAttachment`, `approvedDate`,
  `approvedUserId`, `organizationId`, `companyId`, `projectId`,
  `financialYearId`, `actionId`.
- `VoidedCheque.java` (table `voided_cheque`) - legacy has **no dedicated
  "voided cheque" table**; cancellation is tracked as a status flag
  (`CheqCancelStatus`/`ChequeStatusDate`/`StatusChangeUserId`) on
  `dbo.CheqBookDetail` itself, not a separate table. Added `voidDate` and
  `voidUserId` to mirror that status-change metadata for parity, with a
  code comment explaining there is no 1:1 legacy table.

**Investigated, no patch applied - genuinely no clean legacy analog
in scope**:

- `ItemSubCategory.java` - no `dbo` table with "subcategory"/"sub_category"
  in its name exists anywhere in the legacy schema. `dbo.Item` links
  directly to `ItemCategoryId` with no intermediate level. This app's
  category->subcategory->item 3-level hierarchy is its own design, not a
  legacy port.
- `Invoice.java`/`InvoicePayment.java` - read in full: a lightweight
  generic AR due-tracking pair (`accountCode`, `invoiceAmount`,
  `invoiceStatus`, a list of partial `invoicePayments` tied to
  `voucherEntryId`), used by `InvoiceController`/`ReceivablesController`.
  This has no item/qty/rate lines at all, unlike any of the 20+
  Purchase/Sale invoice tables in `dbo` (`InvPurchaseInvoice`,
  `InvSaleInvoice`, etc.) which are full commercial documents. This looks
  like this project's own internal "outstanding balance + payments
  against it" bookkeeping construct, not a port of a specific legacy
  table - flagging rather than forcing a match onto one of the many
  candidates.
- `User.java` - this app's own Spring Security principal (login,
  password hash, `admin` flag, module/menu/permission lists). Legacy's
  equivalent is an ASP.NET Identity schema (`AspNetUsers` etc.) with a
  fundamentally different security model (different hashing, claims,
  membership provider tables) - not a sensible field-for-field port target.
- `BankAccount.java` - a thin internal wrapper (`Account` + a list of
  `BankAccountEntry` ledger rows) that treats "this GL account is a bank
  account" - not modeled after any specific legacy table. `dbo.Bank` (27
  cols) is the bank-name master, already covered separately by
  `BankName.java`. No further action.
- `BrokerAccount.java` - every "broker"/"agent" concept found in the
  legacy schema (`CommisionAgentBill*`, `InvCommAgentOrder`,
  `InvCommAgentTradeBill`, `ExImClearingAgentBill*`) lives inside the
  commission-trading (CmTr) or import/export (ExImShipmentDocuments)
  modules, both explicitly excluded from this rewrite's scope. No
  in-scope legacy analog exists for this simple broker lookup.
- `ExpanceType.java`/`ExpanceEntries.java` - a generic formula-based extra
  charge calculator (`rate`/`amount`/`operator`/`calculationBy` tied to a
  `voucherType` or an `ItemStock`). The closest legacy structural sibling
  found, `dbo.InvPurchaseInvoiceExpense`, is keyed off
  `InvPurchaseInvoiceId`/`GdnId`/`GrnId`/`SupplierDispatchId` - a
  different document-linkage shape entirely. No confident 1:1 legacy
  source table identified; likely this project's own generic charge-
  template feature.
- `DailyClosing.java`/`DailyClosingEntry.java` - a per-date aggregate
  summary (credit/debit/profit/stock/cash as strings) with note/qty/
  amount entry rows. No legacy table matches this shape - the closest
  schema hits (`StockMonthlyClosing`, `WsRmShiftDayCashInOut`) are
  inventory- or wholesale-shift-specific, not a general daily P&L/cash
  summary. Likely a derived/computed reporting cache internal to this
  app, not a direct port.

Batches 1-3 combined now cover roughly 32 of the ~99 originally-
unmatched entities. Remaining candidates for a future batch: the bulk of
the still-uninvestigated list from the prior section (AccountBudgetDetail/
Header, AccountFinancialYearAllocation, AccountHist, AppOrders,
CustomGroupAccount, ExchangeRateForwardBooking, FinancialStatementNote,
FinancialYearMonth, ItemStock/ItemStockEntry, Loading/LoadingEntry, Mill/
MillKhata/MillRate/MillRateEntry, Monshi, OpeningStock/OpeningStockEntry,
PrePurchase/PrePurchaseEntry, Production/ProductionEntry/ProductionPlant,
ProfitabilityHeading family, Recipe/RecipeEntry, SystemSetting, Tax,
VoucherEntry/VoucherEntryDocument/VoucherStatus/VoucherType, etc).

## Task #43: dead-code removal pass (2026-09-03)

User asked to "remove the old Extra code" and remap according to DB/legacy
.exe. Scoped this down with the user first, since blind deletion in this
environment is risky (no reliable git rollback here, per the earlier
environmental note) and some of the entities flagged in the batch-3 "no
clean legacy analog" list turned out, on closer inspection, to be live
working features rather than dead code.

**Correction to the batch-3 findings**: a live-usage check (searching the
correct `com.mst.repositories`/`com.mst.services` packages, which the
original batch-3 pass had missed) showed:

- `ExpanceType.java` is **not** dead code - it's actively used by
  `PurchaseJournalVoucherService`/`SaleJournalVoucherService` (a real
  `findTop1Byname(MARKIR_FEE_ACCOUNT)` lookup and a
  `findByVoucherTypeAndMarkeitOrderBySequanceAsc` charge-schedule query)
  and by `VouchersController`. Left untouched - the batch-3 entry that
  called it a candidate for removal was based on an incomplete directory
  search.
- `ExpanceEntries.java` and `BrokerAccount.java` (plus their supporting
  service/repository classes) **are** genuinely dead: no controller or
  template reaches them.
  - `ExpanceEntries`: its only reference anywhere (a field on
    `ItemStock.java`) was already inside a commented-out block, and its
    repository `IExpanceEntriesRepository` had zero other references.
  - `BrokerAccount`: `IBrokerAccountService`/`BrokerAccountService` were
    `@Autowired` into `PurchaseQtyController`/`SaleQtyController` but the
    injected field was never actually called anywhere in either file
    (confirmed with a call-site grep, not just a reference grep). The
    real "broker accounts" dropdown those two screens render
    (`${brokerAccounts}` in `new_pjv_qty.html`/`new_siv_form.html` etc.)
    is fed by a completely separate, already-existing mechanism -
    `AccountService.getAllBrokerAccounts()`, which filters plain
    `Account` rows by GL code (`ACCOUNT_LEVEL_THREE_BROKER_ACCOUNT_CODE`)
    - so the `BrokerAccount` entity/service/repository were parallel,
      unused scaffolding, not the thing actually powering the screen.

**Deleted** (outright, per user instruction - no `_deprecated` staging):

- `src/main/java/com/mst/models/ExpanceEntries.java`
- `src/main/java/com/mst/repositories/IExpanceEntriesRepository.java`
- `src/main/java/com/mst/models/BrokerAccount.java`
- `src/main/java/com/mst/serviceInterface/IBrokerAccountService.java`
- `src/main/java/com/mst/services/BrokerAccountService.java`
- `src/main/java/com/mst/repositories/IBrokerAccountRepository.java`

**Cleaned up alongside the deletions** (all brace-balanced after edit):

- `ItemStock.java` - removed the dead commented-out
  `List<ExpanceEntries> expanceEntries` field block that referenced the
  now-deleted class.
- `PurchaseQtyController.java` / `SaleQtyController.java` - removed the
  unused `@Autowired private IBrokerAccountService brokerAccountService;`
  field from each (both use `com.mst.models.*`/`com.mst.serviceInterface.*`
  wildcard imports, so no explicit import line needed removing).

**Verified no dangling references**: a post-delete search across
`src/main/java/` and `src/main/resources/templates/` for `BrokerAccount`
and `ExpanceEntries` turns up only unrelated hits that happen to share a
substring - `AccountService.getAllBrokerAccounts()` (returns
`List<Account>`, not the deleted entity) and the `${brokerAccounts}`/
`id="brokerAccount"` template variable names (fed by that same
`Account`-based method). No reference to the deleted classes remains.

**Left alone per explicit user decision** - these 4 entities are live,
working features (add/view bank account screens, Purchase/Sale/Stock/
Receivables screens depending on the Category->SubCategory->Item
hierarchy, the Receivables invoice screen, the day-end-closing screen)
that just don't have a confirmed 1:1 legacy source table yet. Rather than
delete working functionality, the plan is to keep searching for their
real legacy analog (different naming/schema) in a future pass, same as
any other not-yet-verified entity:

- `BankAccount.java` (live via `BankController`)
- `ItemSubCategory.java` (live via 5 controllers, core to the item
  hierarchy)
- `Invoice.java` / `InvoicePayment.java` (live via `ReceivablesController`,
  `InvoiceController`, `InvoiceWiseOpeningController`)
- `DailyClosing.java` / `DailyClosingEntry.java` (live via
  `VouchersController`)

## Task #36 progress: batch 4 (2026-09-03, continued)

Continued the slow manual pass into the Mill/Production/Recipe cluster,
plus a further search for the 4 live entities left unmatched after
Task #43 (BankAccount, ItemSubCategory, Invoice/InvoicePayment,
DailyClosing/DailyClosingEntry).

**Further search, still no match** - tried alternate legacy naming
conventions for the 4 entities kept in Task #43 (bank book/cash book/day
book, AR/AP invoice, allocation/settlement/outstanding, EOD/cash-closing)
across the full 1400-table dump. No hits under any of these terms.
Treating this as an exhausted search for now rather than continuing to
guess at names; these 4 stay as live, un-ported features per the Task #43
decision.

**Caught a false positive - not patched**: `Production.java`/
`ProductionEntry.java` look, from the name and controller usage (`Account
sController`, `PayablesController`, `ProductionController`,
`ReceivablesController`, `StocksController`), like they should map to one
of the legacy manufacturing tables (`InvProductionJobOrder`,
`InvProductionProcessingBill`, `InvFoodProduction`, etc. - 46 `dbo`
tables contain "Production"). Reading `ProductionEntry.java`'s actual
fields disproves that: it has `millKhata` (a `MillKhata` relation),
`paymentDate`, `paymentType`, `ton`, `Kg`, `rate`, `amount`, `payPercent`,
`status` - this is **the same ton/kg weight-based payment-milestone-
schedule-against-a-MillKhata-account shape already found and rejected for
`PurchaseOrder`/`PurchaseOrderEntry` earlier this session**, just reused
under the "Production" name. It has nothing to do with a manufacturing
job order, recipe, or processing bill. None of the 46 `dbo` Production*
tables were merged - flagging this as the same trap, not guessing a
match.

**Patched** (all brace-balanced):

- `ProductionPlant.java` vs `dbo.InvProductionPlant` - this entity's own
  doc-comment already states it was ported from the legacy
  "Production.DefineProductionPlant" screen, so the match was already
  known; added `organizationId`, `companyId`, `projectId`, `actionId`
  (code/description/branch/active were already covered).
- `Recipe.java` (table `recipe`) vs `dbo.InvBomHeader` - a recipe is a
  bill-of-materials: the produced `itemDef` plus a `List<RecipeEntry>` of
  consumed ingredients, which matches BOM header/detail well enough to
  patch (moderate, not full, confidence - legacy's `ConsumptionItemId` on
  the header and `ProductionItemId` on the detail row are structured
  slightly differently from this entity's single `itemDef` per side, so
  only non-ambiguous fields were added). Added `bomTypeDesc`, `bomCode`,
  `batchQty`, `batchQtyUomId`, `organizationId`, `companyId`,
  `branchesId`, `projectsId` (added the missing `java.math.BigDecimal`
  import this entity didn't have yet).
- `RecipeEntry.java` (table `recipe_entry`) vs `dbo.InvBomDetail`. Added
  `prodQty`, `percent`, `prdRate`, `prdRateUomId`, `prdAmount` alongside
  the existing `itemDef`/`quantityUsed` (~`ConsQty`).

Batches 1-4 combined now cover roughly 37 of the ~99 originally-
unmatched entities (36 verified/patched or confirmed-no-match, plus the
6-file dead-code removal in Task #43). Remaining candidates for a future
batch: AccountBudgetDetail/Header, AccountFinancialYearAllocation,
AccountHist, AppOrders, CustomGroupAccount, ExchangeRateForwardBooking,
FinancialStatementNote, FinancialYearMonth, ItemStock/ItemStockEntry,
Loading/LoadingEntry, Monshi, OpeningStock/OpeningStockEntry, PrePurchase/
PrePurchaseEntry, ProfitabilityHeading family, SystemSetting, Tax,
VoucherEntry/VoucherEntryDocument/VoucherStatus/VoucherType, etc.

## Define Accounts GUI rebuild to match legacy screenshot (2026-09-03)

User provided two screenshots of the legacy `.exe`'s "Chart of Account
Definition" screen and asked to "make the GUI like this."

**Root cause found before any rebuilding**: a fully-functional,
screenshot-matching screen already existed at `/accounts/define_accounts`
(built earlier in this session) with working cascading Level 1-4 grids,
but nothing linked to it - the Main Menu "Accounts" tile and the sidebar
"CHART OF ACCOUNTS" link both pointed at a much simpler flat read-only
listing (`/accounts/chart_of_accounts`) instead. Fixed as the first,
lowest-risk step:
- `main_menu.html`: Accounts tile now points at `/accounts/define_accounts`.
- `fixed_sidebar.html`: added a new "DEFINE ACCOUNTS" entry above the
  existing "CHART OF ACCOUNTS" entry (kept, still points at the old flat
  listing, since it's still a valid read-only view).

**Then rebuilt `define_accounts.html` itself** (573 -> 708 lines) to match
the screenshot more closely and complete the toolbar:
- Re-themed banner/panel titles to the established Golden Ace teal
  (`#1b7a7a`/`#eafaf7`), replacing a hardcoded near-match color.
- Rebuilt the "Account Information" panel from stacked Bootstrap divs into
  a dense label-beside-input `<table>` layout matching the screenshot's
  exact row pairings (Parent Account/Group-Detail, Account Title/Account
  Code, Account Type/Account Class, P/L Notes/Account Level, B/S Notes/
  Other Code, Sup-Cust Group/Phone No, Custom Group/Opening+Save, City
  Name), with red-asterisk required-field styling and grey readonly
  fields. Account Level is now a visible readonly field (previously only
  a hidden input).
- "Account Allocation To Locations" checkboxes shown disabled+checked
  (view-only here, matching the screenshot's greyed-out look; a note
  links to the existing full allocation-management screen).
- Added the "Filter Accounts In Child Grid" panel (Account Type + Account
  Title filters) shown in the screenshot, wired to the live child grid.
- Completed the toolbar: added a working "114-Print" button
  (`window.print()`, with a `@media print` rule that hides everything but
  the grids) and a working "Upload Excel Sheet" button.
- Excel upload is a real feature, not decorative: added Apache POI
  (`poi`/`poi-ooxml` 5.2.3) to `pom.xml`, and a new
  `POST /accounts/upload_accounts_excel` endpoint in
  `AccountsController.java` that reads a Level/ParentCode/Code/
  FormattedCode/Name sheet row-by-row (`DataFormatter`, defensive
  per-row try/catch so one bad row doesn't abort the batch) and applies
  each row through the exact same
  `accountLevel{Two,Three,Four}Service.addOrUpdateAccount(...)` call the
  manual Save button already uses, so a bulk-imported row behaves
  identically to a hand-entered one. Level 1 groups are excluded, same
  rule as the New button.

All five changed files (`main_menu.html`, `fixed_sidebar.html`,
`define_accounts.html`, `pom.xml`, `AccountsController.java`) verified
structurally sound (HTML tag-balance / brace-paren-balance checks - no
compiler available in this environment) and confirmed written to the
repo via md5sum / grep re-checks after transfer.

Not yet done (would need Maven, unavailable here): compiling and
smoke-testing the new Excel-upload endpoint end-to-end.

## Main Menu module-launcher landing page (Task #38, 2026-09-03)

User asked to replicate the legacy `.exe`'s post-login "Main Menu" screen
(screenshot: top bar with company name / logged-in user / clock / financial
year / logout, italic "GOLDEN ACE FOODS" title, tile grid of modules with
teal headers and icons - DashBoard/Accounts/Purchase/Quality
Control/Contractor Wages/Sale/Production/Store Management/Inventory/
Taxation/Weigh Bridge/Master Data Definition/Commission Trading/Packing
Material).

Built as a new standalone template `src/main/resources/templates/
main_menu.html` (Bootstrap + Font Awesome, already-vendored assets - no new
dependencies). `LoginController`'s existing `/index` mapping (the
`defaultSuccessUrl` after login, per `SecurityConfiguration`) now renders
`main_menu` instead of the old analytics dashboard; the old dashboard moved
unchanged to a new `/dashboard/analytics` mapping (same `index` view/
template, just relocated) and is reachable via the "DashBoard" tile - so no
existing functionality was removed, only relocated one hop further from
login. `companyService.getCompanyTopOne()` and `financialYearService.
getCurrentFinancialYear()` feed the top bar; username comes from
`#authentication.getPrincipal().getUsername()`, matching the pattern already
used in `fixed_sidebar.html`. Clock is a client-side `setInterval`.

Tile routing (verified each target `@RequestMapping` actually exists before
wiring, rather than trusting the legacy screenshot's module names):
DashBoard -> `/dashboard/analytics`, Accounts -> `/accounts/
chart_of_accounts`, Purchase -> `/payable/new_pjv_qty_form`, Sale ->
`/receivables/new_sjv_qty_form`, Production -> `/production/
production_form`, Store Management -> `/inventory/warehouses`, Inventory
-> `/costing/inventory_costing`, Master Data Definition -> `/lookups/
business_types`. Quality Control, Contractor Wages, Commission Trading and
Packing Material tiles are rendered but disabled (grey, non-clickable,
tooltip "Not included in this build") per this project's standing
rice-mill-core exclusion list (QCL/LabAnalysis, Contractor_Wages, CmTr,
PackingMaterial_Store). Taxation is disabled/"Coming soon" since
Tax_Definition (Task #16) isn't built yet. Weigh Bridge is also disabled -
NOT because it's excluded (Kanta/weighbridge weight-entry screens are
in-scope and already have sidebar links like `/kanta/firstWeight`), but
because a `grep` across every controller found no actual
`@RequestMapping`/`@GetMapping` implementing `/kanta/weight_list` or any
other `/kanta/*` route - those sidebar links are pre-existing dead links in
this codebase already, not something this task introduced; flagging as a
real (pre-existing) gap rather than silently wiring a tile to a 404.

Verified via `html.parser`-based tag-balance check (clean, empty stack)
before and after a follow-up fix to the Weigh Bridge tile.

## Working method per screen
1. Find the legacy class in `il_by_namespace*` or `extracted_disasm.zip` (or open
   the matching DLL in ILSpy for real C# if IL isn't enough).
2. List every field/control on the legacy form (FIELD entries in the class dump).
3. Check whether the web app already has an equivalent screen; if so, diff fields.
4. Build/extend the entity, repository, service, controller, template, JS to match.
5. Check this item off here and move to the next.
