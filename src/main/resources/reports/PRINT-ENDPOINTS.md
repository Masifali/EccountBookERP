# ERP print controllers

Controllers: `src/main/java/com/mst/controllers/ERPPrint`. Request models are in its `requests` folder.
Start debugging in the module method listed below, then follow `printReport` into `ReportPrintSupport` for data, parameters, JRXML compilation, filling and PDF export.
Covers 1064 templates and all 1063 executable SQL seed contracts. Existing URLs are preserved.
Every named action accepts GET query parameters or a POST JSON body. Company and organization come from the session.

## Controller index

| Module | Controller | Named reports |
|---|---|---|
| Accounts | `AccountsPrintController.java` | 114 |
| Banking | `BankingPrintController.java` | 17 |
| CommissionAgent | `CommissionAgentPrintController.java` | 8 |
| CommissionTrading | `CommissionTradingPrintController.java` | 41 |
| Concrete | `ConcretePrintController.java` | 152 |
| ContractorWages | `ContractorWagesPrintController.java` | 4 |
| Dashboard | `DashboardPrintController.java` | 5 |
| Export | `ExportPrintController.java` | 99 |
| FeedMill | `FeedMillPrintController.java` | 9 |
| Hrm | `HrmPrintController.java` | 24 |
| Import | `ImportPrintController.java` | 13 |
| Inventory | `InventoryPrintController.java` | 10 |
| Lab | `LabPrintController.java` | 17 |
| Logistics | `LogisticsPrintController.java` | 8 |
| Manufacturing | `ManufacturingPrintController.java` | 17 |
| PackingMaterial | `PackingMaterialPrintController.java` | 17 |
| PartyProcessing | `PartyProcessingPrintController.java` | 38 |
| Pos | `PosPrintController.java` | 4 |
| Production | `ProductionPrintController.java` | 35 |
| Purchase | `PurchasePrintController.java` | 111 |
| Sale | `SalePrintController.java` | 128 |
| Salt | `SaltPrintController.java` | 2 |
| Steel | `SteelPrintController.java` | 88 |
| Stocks | `StocksPrintController.java` | 56 |
| Store | `StorePrintController.java` | 36 |
| Tax | `TaxPrintController.java` | 6 |
| Weighbridge | `WeighbridgePrintController.java` | 5 |

CommonPrintController handles legacy template/key URLs, grid PDFs and print metadata.

## Accounts

| Template | GET / POST endpoint | Procedure | Request |
|---|---|---|---|
| 114-AcRptChartOfAccounts.rpt | `/reports/print/114-chart-of-accounts` | Sp_ChartOfAccounts_Rpt | `AccountsPrintRequests.Rpt114ChartOfAccountsRequest` |
| 129-OpeningBalanceRpt.rpt | `/reports/print/129-opening-balance` | Sp_AccountsOpeningBalance_Slip | `AccountsPrintRequests.Rpt129OpeningBalanceRequest` |
| 920_BankReconciliation_Slip.rpt | `/reports/print/920-bank-reconciliation-slip` | [dbo].[USP_BankReconciliation_SlipAndRegister] | `AccountsPrintRequests.Rpt920BankReconciliationSlipRequest` |
| 110-AcRptActivitySummery.rpt | `/reports/print/110-activity-summery` | Sp_ActivityReportSummery_Rpt | `AccountsPrintRequests.Rpt110ActivitySummeryRequest` |
| 110A-AcRptActivitySummery.rpt | `/reports/print/110a-activity-summery` | Sp_ActivityReportSummery_Rpt | `AccountsPrintRequests.Rpt110AActivitySummeryRequest` |
| 110B-AcRptActivitySummery.rpt | `/reports/print/110b-activity-summery` | Sp_ActivityReportSummeryDocumentTypeWise_Rpt | `AccountsPrintRequests.Rpt110BActivitySummeryRequest` |
| 153-BalanceSheetStatementRpt.rpt | `/reports/print/153-balance-sheet-statement` | SpAccounts_BalanceSheetFormatA_Report | `AccountsPrintRequests.Rpt153BalanceSheetStatementRequest` |
| 593-CashBankBalancesSummery_Rpt.rpt | `/reports/print/593-cash-bank-balances-summery` | Sp_Accounts_CashBankBalancesSummery_Rpt | `AccountsPrintRequests.Rpt593CashBankBalancesSummeryRequest` |
| 593_01_CashBankBalancesSummeryReport.rpt | `/reports/print/593-01-cash-bank-balances-summery-report` | Sp_Accounts_CashBankBalancesSummery_Rpt | `AccountsPrintRequests.Rpt59301CashBankBalancesSummeryReportRequest` |
| 590-BankBalances.rpt | `/reports/print/590-bank-balances` | Sp_Accounts_BankBalances_Rpt | `AccountsPrintRequests.Rpt590BankBalancesRequest` |
| 590_01_BankBalancesWithSummary.rpt | `/reports/print/590-01-bank-balances-with-summary` | Sp_Accounts_BankBalances_Rpt | `AccountsPrintRequests.Rpt59001BankBalancesWithSummaryRequest` |
| 141-AccountPayablesWithLastBillAmountAndDate.rpt | `/reports/print/141-account-payables-with-last-bill-amount-and-date` | Sp_Accounts_PayablesWithLastBillAndPaidAmount_Rpt | `AccountsPrintRequests.Rpt141AccountPayablesWithLastBillAmountAndDateRequest` |
| 592-CashBalances.rpt | `/reports/print/592-cash-balances` | Sp_Accounts_CashBalances_Rpt | `AccountsPrintRequests.Rpt592CashBalancesRequest` |
| 157-CommissionAndBrokary.rpt | `/reports/print/157-commission-and-brokary` | Sp_CommissionAndBrokery_Report | `AccountsPrintRequests.Rpt157CommissionAndBrokaryRequest` |
| 001-ContractorWagesRegister.rpt | `/reports/print/001-contractor-wages-register` | USP_GetSummaryWagesByRefDocumentsAndActivities | `AccountsPrintRequests.Rpt001ContractorWagesRegisterRequest` |
| 130-RptAcSupplierCustomerQuantativeGL.rpt | `/reports/print/130-ac-supplier-customer-quantative-gl` | Sp_GeneralLedger_SupplierCustomerQuantative | `AccountsPrintRequests.Rpt130AcSupplierCustomerQuantativeGLRequest` |
| 105-AcRptGeneralLedger.rpt | `/reports/print/105-general-ledger` | Sp_Accounts_GeneralLedger_Rpt | `AccountsPrintRequests.Rpt105GeneralLedgerRequest` |
| 106-AcRptGeneralLedgerB.rpt | `/reports/print/106-general-ledger-b` | Sp_Accounts_GeneralLedger_Rpt | `AccountsPrintRequests.Rpt106GeneralLedgerBRequest` |
| 107-AcRptQuantativeLedger.rpt | `/reports/print/107-quantative-ledger` | Sp_Accounts_GeneralLedger_Rpt | `AccountsPrintRequests.Rpt107QuantativeLedgerRequest` |
| 108-AcRptGeneralLedgerSummery.rpt | `/reports/print/108-general-ledger-summery` | Sp_VouchersAccountsGeneralLedgerSummery | `AccountsPrintRequests.Rpt108GeneralLedgerSummeryRequest` |
| 105A-GeneralLedgerSummary2.rpt | `/reports/print/105a-general-ledger-summary-2` | SpAccounts_GeneralLedger2Format_Rpt | `AccountsPrintRequests.Rpt105AGeneralLedgerSummary2Request` |
| 109-AcRptGeneralLedgerStatement.rpt | `/reports/print/109-general-ledger-statement` | Sp_GeneralLedgerStatement_Rpt | `AccountsPrintRequests.Rpt109GeneralLedgerStatementRequest` |
| 134-InventoryPayablesandReceivables.rpt | `/reports/print/134-inventory-payablesand-receivables` | SpAccounts_InventoryPayablesandReceivables_Rpt | `AccountsPrintRequests.Rpt134InventoryPayablesandReceivablesRequest` |
| 134A-InventoryPayablesandReceivables.rpt | `/reports/print/134a-inventory-payablesand-receivables` | SpAccounts_InventoryPayablesandReceivables_Rpt | `AccountsPrintRequests.Rpt134AInventoryPayablesandReceivablesRequest` |
| 121_01-ReceivablesByDueDates.rpt | `/reports/print/121-01-receivables-by-due-dates` | [dbo].[Usp_ReceivablesByDueDates] | `AccountsPrintRequests.Rpt12101ReceivablesByDueDatesRequest` |
| 122-AcRptAccounts_ReceivablesWithStatus.rpt | `/reports/print/122-accounts-receivables-with-status` | Sp_Accounts_Receivables_Rpt | `AccountsPrintRequests.Rpt122AccountsReceivablesWithStatusRequest` |
| 124_Payables_New.rpt | `/reports/print/124-payables-new` | UPS_PayablesAging_New | `AccountsPrintRequests.Rpt124PayablesNewRequest` |
| 127-SupplierAgingReport_DocumentWise.rpt | `/reports/print/127-supplier-aging-report-document-wise` | usp_SupplierAgingReport_DocumentWise | `AccountsPrintRequests.Rpt127SupplierAgingReportDocumentWiseRequest` |
| 123_Receivables_New.rpt | `/reports/print/123-receivables-new` | UPS_ReceivablesAging_New | `AccountsPrintRequests.Rpt123ReceivablesNewRequest` |
| 126-CustomerAgingReport_DocumentWise.rpt | `/reports/print/126-customer-aging-report-document-wise` | usp_CustomerAgingReport_DocumentWise | `AccountsPrintRequests.Rpt126CustomerAgingReportDocumentWiseRequest` |
| 113A-AcRptTrialBalanceSelectedGroupAc.rpt | `/reports/print/113a-trial-balance-selected-group-ac` | SpAccounts_TrialBalanceSelectedNew_Report | `AccountsPrintRequests.Rpt113ATrialBalanceSelectedGroupAcRequest` |
| 375-GetSubsidiaryAccountFromVouchers.rpt | `/reports/print/375-get-subsidiary-account-from-vouchers` | USP_GetSubsidiaryPayablesReceivables | `AccountsPrintRequests.Rpt375GetSubsidiaryAccountFromVouchersRequest` |
| 111A-AcRptTrialBalances.rpt | `/reports/print/111a-trial-balances` | Sp_TrialBalance_Rpt | `AccountsPrintRequests.Rpt111ATrialBalancesRequest` |
| 118-AcRptVoucherSlip.rpt | `/reports/print/118-voucher-slip` | Sp_Accounts_VouchersValidation_Rpt | `AccountsPrintRequests.Rpt118VoucherSlipRequest` |
| 118A-VoucherReport.rpt | `/reports/print/118a-voucher-report` | Sp_Accounts_VouchersValidation_Rpt | `AccountsPrintRequests.Rpt118AVoucherReportRequest` |
| 102-BankPaymentVoucher.rpt | `/reports/print/102-bank-payment-voucher` | SpVouchers_PaymentReceiptVoucherSlipNew_Rpt | `AccountsPrintRequests.Rpt102BankPaymentVoucherRequest` |
| 901_VoucherInvoicesAdjustment_Slip.rpt | `/reports/print/901-voucher-invoices-adjustment-slip` | [dbo].[USP_VoucherInvoicesAdjustment_ReadByVoucherHeadId] | `AccountsPrintRequests.Rpt901VoucherInvoicesAdjustmentSlipRequest` |
| 102_01_BillsPayablesOrReceiveableVoucher.rpt | `/reports/print/102-01-bills-payables-or-receiveable-voucher` | SpVouchers_PaymentReceiptVoucherSlipNew_Rpt | `AccountsPrintRequests.Rpt10201BillsPayablesOrReceiveableVoucherRequest` |
| 144-DayBookSlip.rpt | `/reports/print/144-day-book-slip` | Sp_DayBookSlip | `AccountsPrintRequests.Rpt144DayBookSlipRequest` |
| 002-ContractorWagesSlip.rpt | `/reports/print/002-contractor-wages-slip` | Sp_InvContractorWagesBillHeader_SlipandRegister | `AccountsPrintRequests.Rpt002ContractorWagesSlipRequest` |
| 004-ContractorWagesBillManualSlip.rpt | `/reports/print/004-contractor-wages-bill-manual-slip` | Sp_InvContractorWagesBillHeader_SlipandRegister | `AccountsPrintRequests.Rpt004ContractorWagesBillManualSlipRequest` |
| 102B-SpVouchers_PartyPaymentAndReceiptSummary.rpt | `/reports/print/102b-sp-vouchers-party-payment-and-receipt-summary` | SpVouchers_PartyPaymentAndReceiptSummary_Rpt | `AccountsPrintRequests.Rpt102BSpVouchersPartyPaymentAndReceiptSummaryRequest` |
| 115-AcRptVoucherValidation.rpt | `/reports/print/115-voucher-validation` | Sp_Accounts_VouchersValidation_Rpt | `AccountsPrintRequests.Rpt115VoucherValidationRequest` |
| 119-AcRptVoucherValidation2.rpt | `/reports/print/119-voucher-validation-2` | Sp_Accounts_VouchersValidation_Rpt | `AccountsPrintRequests.Rpt119VoucherValidation2Request` |
| 594-CashBalancesInOutFlow.rpt | `/reports/print/594-cash-balances-in-out-flow` | Sp_Accounts_CashAndBank_InflowOutFlow_Rpt | `AccountsPrintRequests.Rpt594CashBalancesInOutFlowRequest` |
| 595-Cash&BankPaymentReciept.rpt | `/reports/print/595-cash-bank-payment-reciept` | Sp_Accounts_DialyCashAndBankBalances_Rpt | `AccountsPrintRequests.Rpt595CashBankPaymentRecieptRequest` |
| 599_Cash&BankPaymentRecieptWithAllJv.rpt | `/reports/print/599-cash-bank-payment-reciept-with-all-jv` | Sp_Accounts_DialyCashAndBankBalances_Rpt | `AccountsPrintRequests.Rpt599CashBankPaymentRecieptWithAllJvRequest` |
| 241-FreightVoucherSlip.rpt | `/reports/print/241-freight-voucher-slip` | SP_FreightVoucherSlipAndRegister | `AccountsPrintRequests.Rpt241FreightVoucherSlipRequest` |
| 102-ANewAcRptPaymentReceiptsVoucherSlip.rpt | `/reports/print/102-a-new-ac-payment-receipts-voucher-slip` | SpVouchers_PaymentReceiptVoucherSlipNew_Rpt | `AccountsPrintRequests.Rpt102ANewAcPaymentReceiptsVoucherSlipRequest` |
| 122A-CityWise-AcRptAccounts_ReceivablesWithStatus.rpt | `/reports/print/122a-city-wise-ac-accounts-receivables-with-status` | Sp_Accounts_Receivables_Rpt | `AccountsPrintRequests.Rpt122ACityWiseAcAccountsReceivablesWithStatusRequest` |
| 126-PdcInventoryPendingSummery.rpt | `/reports/print/126-pdc-inventory-pending-summery` | Sp_PdcInventory_ReceiptsSummery_Rpt | `AccountsPrintRequests.Rpt126PdcInventoryPendingSummeryRequest` |
| 125-PdcInventoryPending.rpt | `/reports/print/125-pdc-inventory-pending` | Sp_PdcInventory_SlipAndRegister_Rpt | `AccountsPrintRequests.Rpt125PdcInventoryPendingRequest` |
| 1870-WagesDetail.rpt | `/reports/print/1870-wages-detail` | [pcc].[USP_WagesReportWithActivities] | `AccountsPrintRequests.Rpt1870WagesDetailRequest` |
| 1881_WagesDetailWithoutItem.rpt | `/reports/print/1881-wages-detail-without-item` | [pcc].[USP_WagesReportWithActivities] | `AccountsPrintRequests.Rpt1881WagesDetailWithoutItemRequest` |
| 1880-dtSummaryByContractor.rpt | `/reports/print/1880-dt-summary-by-contractor` | [pcc].[USP_WagesReportWithActivities] | `AccountsPrintRequests.Rpt1880DtSummaryByContractorRequest` |
| 1871-SummaryByItem.rpt | `/reports/print/1871-summary-by-item` | [pcc].[USP_WagesReportWithActivities] | `AccountsPrintRequests.Rpt1871SummaryByItemRequest` |
| 1872-SummaryByItemandContractor.rpt | `/reports/print/1872-summary-by-itemand-contractor` | [pcc].[USP_WagesReportWithActivities] | `AccountsPrintRequests.Rpt1872SummaryByItemandContractorRequest` |
| 1873-SummaryByItemandServiceActivity.rpt | `/reports/print/1873-summary-by-itemand-service-activity` | [pcc].[USP_WagesReportWithActivities] | `AccountsPrintRequests.Rpt1873SummaryByItemandServiceActivityRequest` |
| 1874-SummaryByContractorandServiceActivity.rpt | `/reports/print/1874-summary-by-contractorand-service-activity` | [pcc].[USP_WagesReportWithActivities] | `AccountsPrintRequests.Rpt1874SummaryByContractorandServiceActivityRequest` |
| 1875-SummaryByItemContractorandServiceActivity.rpt | `/reports/print/1875-summary-by-item-contractorand-service-activity` | [pcc].[USP_WagesReportWithActivities] | `AccountsPrintRequests.Rpt1875SummaryByItemContractorandServiceActivityRequest` |
| 1876-SummaryByWagesGroup.rpt | `/reports/print/1876-summary-by-wages-group` | [pcc].[USP_WagesReportWithActivities] | `AccountsPrintRequests.Rpt1876SummaryByWagesGroupRequest` |
| 1877-SummaryByWagesGroupandServiceActivity.rpt | `/reports/print/1877-summary-by-wages-groupand-service-activity` | [pcc].[USP_WagesReportWithActivities] | `AccountsPrintRequests.Rpt1877SummaryByWagesGroupandServiceActivityRequest` |
| 1878-SummaryByWagesGroupandContractor.rpt | `/reports/print/1878-summary-by-wages-groupand-contractor` | [pcc].[USP_WagesReportWithActivities] | `AccountsPrintRequests.Rpt1878SummaryByWagesGroupandContractorRequest` |
| 1879-SummaryByWagesGroupContractorandServiceActivity.rpt | `/reports/print/1879-summary-by-wages-group-contractorand-service-activity` | [pcc].[USP_WagesReportWithActivities] | `AccountsPrintRequests.Rpt1879SummaryByWagesGroupContractorandServiceActivityRequest` |
| 102-CashPaymentVoucher.rpt | `/reports/print/102-cash-payment-voucher` | SpVouchers_PaymentReceiptVoucherSlipNew_Rpt | `AccountsPrintRequests.Rpt102CashPaymentVoucherRequest` |
| 132-PaymentByInvoiceSlipNew_Report.rpt | `/reports/print/132-payment-by-invoice-slip-new-report` | SpVouchers_Payment&ReceipteByInvoiceVoucherSlipNew_Rpt | `AccountsPrintRequests.Rpt132PaymentByInvoiceSlipNewReportRequest` |
| 250_PdcPaymentTransactionsSlip.rpt | `/reports/print/250-pdc-payment-transactions-slip` | [Bank].[USP_pdcTransaction_SlipAndRegister] | `AccountsPrintRequests.Rpt250PdcPaymentTransactionsSlipRequest` |
| 100-PartyWisePaymentReceiptsVoucherSlip.rpt | `/reports/print/100-party-wise-payment-receipts-voucher-slip` | Usp_VouchersPaymentReceiptSlip_Report | `AccountsPrintRequests.Rpt100PartyWisePaymentReceiptsVoucherSlipRequest` |
| 102-CashReceiptVoucher.rpt | `/reports/print/102-cash-receipt-voucher` | SpVouchers_PaymentReceiptVoucherSlipNew_Rpt | `AccountsPrintRequests.Rpt102CashReceiptVoucherRequest` |
| 105_002-AcRptGeneralLedger.rpt | `/reports/print/105-00-general-ledger` | usp_AccountsLedgerByJobOrder | `AccountsPrintRequests.Rpt10500GeneralLedgerRequest` |
| 106A_GeneralLedgerSummary2.rpt | `/reports/print/106a-general-ledger-summary-2` | [dbo].[USP_InvFoodProduction_GetJobOrderIdByProduction] | `AccountsPrintRequests.Rpt106AGeneralLedgerSummary2Request` |
| 107A_GeneralLedger.rpt | `/reports/print/107a-general-ledger` | [dbo].[USP_InvFoodProduction_GetJobOrderIdByProduction] | `AccountsPrintRequests.Rpt107AGeneralLedgerRequest` |
| 108A_GeneralLedgerSummery.rpt | `/reports/print/108a-general-ledger-summery` | [dbo].[USP_InvFoodProduction_GetJobOrderIdByProduction] | `AccountsPrintRequests.Rpt108AGeneralLedgerSummeryRequest` |
| 11-AccountsBudgetSlip.rpt | `/reports/print/11-accounts-budget-slip` | [dbo].[USP_AccountsBudgetSlipAndRegister] | `AccountsPrintRequests.Rpt11AccountsBudgetSlipRequest` |
| 111-AcRptReceivablesAging.rpt | `/reports/print/111-receivables-aging` | Sp_Accounts_ReceivablesAging_Rpt | `AccountsPrintRequests.Rpt111ReceivablesAgingRequest` |
| 114_01-AcRptChartOfAccounts.rpt | `/reports/print/114-01-chart-of-accounts` | USP_Organization_ChartOfAccountTemplate_GetAll | `AccountsPrintRequests.Rpt11401ChartOfAccountsRequest` |
| 116-AcRptAccountsActivityDetail.rpt | `/reports/print/116-accounts-activity-detail` | Sp_Accounts_ActivityDetail_Rpt | `AccountsPrintRequests.Rpt116AccountsActivityDetailRequest` |
| 117-AcRptTrialBalanceSelectedCurrent.rpt | `/reports/print/117-trial-balance-selected-current` | Sp_PaymentByInvoiceSummery_Register | `AccountsPrintRequests.Rpt117TrialBalanceSelectedCurrentRequest` |
| 120-AcRptPayablesAging.rpt | `/reports/print/120-payables-aging` | SpAccounts_LedgerAging | `AccountsPrintRequests.Rpt120PayablesAgingRequest` |
| 120A-CapitalOwnerEquityReport.rpt | `/reports/print/120a-capital-owner-equity-report` | SpAccounts_TrialBalanceSelectedNew_Report | `AccountsPrintRequests.Rpt120ACapitalOwnerEquityReportRequest` |
| 121-Accounts_Payables_Rpt.rpt | `/reports/print/121-accounts-payables` | Sp_Accounts_Payables_Rpt | `AccountsPrintRequests.Rpt121AccountsPayablesRequest` |
| 121_01_PayablesAndPaymentSchedule.rpt | `/reports/print/121-01-payables-and-payment-schedule` | usp_PayablesAndPaymentSchedule | `AccountsPrintRequests.Rpt12101PayablesAndPaymentScheduleRequest` |
| 131-AccountsTrialBalance.rpt | `/reports/print/131-accounts-trial-balance` | Sp_Accounts_TrialBalances_Rpt | `AccountsPrintRequests.Rpt131AccountsTrialBalanceRequest` |
| 133-PaymentbyInvoice_Summary&Detail.rpt | `/reports/print/133-paymentby-invoice-summary-detail` | Sp_COAAllocation_GetAllMethod | `AccountsPrintRequests.Rpt133PaymentbyInvoiceSummaryDetailRequest` |
| 134_PayablesAndReceivablesWithPaymentAndReceipts.rpt | `/reports/print/134-payables-and-receivables-with-payment-and-receipts` | usp_getPayablesAndReceivablesWithPaymentAndReceipts | `AccountsPrintRequests.Rpt134PayablesAndReceivablesWithPaymentAndReceiptsRequest` |
| 142-DueByDatePayablesAndReceivables.rpt | `/reports/print/142-due-by-date-payables-and-receivables` | USp_AccountsPayablesByDuedateBetweenPeriod | `AccountsPrintRequests.Rpt142DueByDatePayablesAndReceivablesRequest` |
| 143_02_ProfitLossBreakUp.rpt | `/reports/print/143-02-profit-loss-break-up` | usp_Accounts_COGS_BreakUp | `AccountsPrintRequests.Rpt14302ProfitLossBreakUpRequest` |
| 151-AcRptAccountsBalanceSheetStandard.rpt | `/reports/print/151-accounts-balance-sheet-standard` | Sp_Accounts_BalanceSheetStandard_Rpt | `AccountsPrintRequests.Rpt151AccountsBalanceSheetStandardRequest` |
| 152-AcRptAccountsProfitLoss.rpt | `/reports/print/152-accounts-profit-loss` | Sp_Accounts_ProfitLoassStandard_Rpt | `AccountsPrintRequests.Rpt152AccountsProfitLossRequest` |
| 154-Payables&Receiveableaging-Rpt.rpt | `/reports/print/154-payables-receiveableaging` | SpAccounts_PayablesReceivablesAging_Rpt | `AccountsPrintRequests.Rpt154PayablesReceiveableagingRequest` |
| 154-Profit&Loss.rpt | `/reports/print/154-profit-loss` | SpAccounts_ProfitLoassFormatA_Report | `AccountsPrintRequests.Rpt154ProfitLossRequest` |
| 159-PayablesAndReceivablebetweenPeriod.rpt | `/reports/print/159-payables-and-receivablebetween-period` | USp_AccountsPayablesByDuedateBetweenPeriod | `AccountsPrintRequests.Rpt159PayablesAndReceivablebetweenPeriodRequest` |
| 164-AcRptQuantativeLedger.rpt | `/reports/print/164-quantative-ledger` | Sp_VoucherHead_GLQuantitativeLedgerTradePro | `AccountsPrintRequests.Rpt164QuantativeLedgerRequest` |
| 166-ProfitLossNewReport.rpt | `/reports/print/166-profit-loss-new-report` | SpAccounts_ProfitLoss_ForCrystal | `AccountsPrintRequests.Rpt166ProfitLossNewReportRequest` |
| 168-CommisionTradeBillAndSlip.rpt | `/reports/print/168-commision-trade-bill-and-slip` | Sp_Vouchers_GetMethods | `AccountsPrintRequests.Rpt168CommisionTradeBillAndSlipRequest` |
| 1865-SalesWages_Register.rpt | `/reports/print/1865-sales-wages-register` | UPS_ReceivablesAging_New | `AccountsPrintRequests.Rpt1865SalesWagesRegisterRequest` |
| 240-FreightVoucherRegister.rpt | `/reports/print/240-freight-voucher-register` | USP_FreightVoucher_PendingForApproval | `AccountsPrintRequests.Rpt240FreightVoucherRegisterRequest` |
| 242-InLandFreightAgreementSlip.rpt | `/reports/print/242-in-land-freight-agreement-slip` | SP_InLandFreightAgreementSlipAndRegister | `AccountsPrintRequests.Rpt242InLandFreightAgreementSlipRequest` |
| 243-InLandFreightAgreementRegister.rpt | `/reports/print/243-in-land-freight-agreement-register` | SP_InLandFreightAgreementSlipAndRegister | `AccountsPrintRequests.Rpt243InLandFreightAgreementRegisterRequest` |
| 291-SaleInvoice_AutoRated.rpt | `/reports/print/291-sale-invoice-auto-rated` | Sp_Vouchers_GetMethods | `AccountsPrintRequests.Rpt291SaleInvoiceAutoRatedRequest` |
| 380-DueByDateReceivables.rpt | `/reports/print/380-due-by-date-receivables` | [dbo].[USP_DueByDateReceivables] | `AccountsPrintRequests.Rpt380DueByDateReceivablesRequest` |
| 381-SaleInvoicewiseProfitablity.rpt | `/reports/print/381-sale-invoicewise-profitablity` | SpInventory_EvaulationDetailSalesReports | `AccountsPrintRequests.Rpt381SaleInvoicewiseProfitablityRequest` |
| 405-RptEBGLBySupplier.rpt | `/reports/print/405-ebgl-by-supplier` | Sp_InventoryEBBalancesByPartyAndItem_Rpt | `AccountsPrintRequests.Rpt405EBGLBySupplierRequest` |
| 420_ProfitLossReport.rpt | `/reports/print/420-profit-loss-report` | usp_ProfitAndLoss_New | `AccountsPrintRequests.Rpt420ProfitLossReportRequest` |
| 901_01_VoucherInvoicesAdjustment_Slip.rpt | `/reports/print/901-01-voucher-invoices-adjustment-slip` | [dbo].[USP_VoucherInvoicesAdjustment_ReadByVoucherHeadId] | `AccountsPrintRequests.Rpt90101VoucherInvoicesAdjustmentSlipRequest` |
| 910A_StockEvaluationCgsReport.rpt | `/reports/print/910a-stock-evaluation-cgs-report` | [dbo].[USP_SaleReportWithGrossProfitsAndLossNew] | `AccountsPrintRequests.Rpt910AStockEvaluationCgsReportRequest` |
| ActivitySummeryCreditSubReportForB.rpt | `/reports/print/activity-summery-credit-sub-report-for-b` | Sp_ActivityReportSummeryDocumentTypeWise_Rpt | `AccountsPrintRequests.RptActivitySummeryCreditSubReportForBRequest` |
| ActivitySummeryDebitSubReportForB.rpt | `/reports/print/activity-summery-debit-sub-report-for-b` | Sp_ActivityReportSummeryDocumentTypeWise_Rpt | `AccountsPrintRequests.RptActivitySummeryDebitSubReportForBRequest` |
| DayBookCreditSubReport.rpt | `/reports/print/day-book-credit-sub-report` | Sp_DayBookSlip | `AccountsPrintRequests.RptDayBookCreditSubReportRequest` |
| DayBookDebitSubReport.rpt | `/reports/print/day-book-debit-sub-report` | Sp_DayBookSlip | `AccountsPrintRequests.RptDayBookDebitSubReportRequest` |
| RptCWBInward.rpt | `/reports/print/cwb-inward` | Sp_Vouchers_GetMethods | `AccountsPrintRequests.RptCWBInwardRequest` |
| rptGRN_Print.rpt | `/reports/print/grn-print` | Sp_Vouchers_GetMethods | `AccountsPrintRequests.RptGRNPrintRequest` |
| RptInvBOMSlipA.rpt | `/reports/print/inv-bom-slip-a` | Sp_Vouchers_GetMethods | `AccountsPrintRequests.RptInvBOMSlipARequest` |
| RptInvSupplySchedule.rpt | `/reports/print/inv-supply-schedule` | Sp_Vouchers_GetMethods | `AccountsPrintRequests.RptInvSupplyScheduleRequest` |

## Banking

| Template | GET / POST endpoint | Procedure | Request |
|---|---|---|---|
| 01-FcyExRateForwwardBookingSlip.rpt | `/reports/print/01-fcy-ex-rate-forwward-booking-slip` | [dbo].[USP_FcyExRateForwardBooking_SlipAndRegister] | `BankingPrintRequests.Rpt01FcyExRateForwwardBookingSlipRequest` |
| 102-AcRptPaymentReceiptsVoucherSlip.rpt | `/reports/print/102-payment-receipts-voucher-slip` | Sp_Vouchers_PaymentReceiptVoucherSlip_Rpt | `BankingPrintRequests.Rpt102PaymentReceiptsVoucherSlipRequest` |
| 102-BankReceiptVoucher.rpt | `/reports/print/102-bank-receipt-voucher` | SpVouchers_PaymentReceiptVoucherSlipNew_Rpt | `BankingPrintRequests.Rpt102BankReceiptVoucherRequest` |
| 105_01_BankSummaryLedger.rpt | `/reports/print/105-01-bank-summary-ledger` | USP_BankSummaryWithAgainstAccounts | `BankingPrintRequests.Rpt10501BankSummaryLedgerRequest` |
| 105_02_BankSummaryLedger.rpt | `/reports/print/105-02-bank-summary-ledger` | USP_BankSummaryWithAgainstAccounts | `BankingPrintRequests.Rpt10502BankSummaryLedgerRequest` |
| 105C-AcRptFcyGeneralLedger.rpt | `/reports/print/105c-fcy-general-ledger` | Usp_Accounts_FcyGeneralLedger_Rpt | `BankingPrintRequests.Rpt105CFcyGeneralLedgerRequest` |
| 105D-AcRptFcyGeneralLedger.rpt | `/reports/print/105d-fcy-general-ledger` | Usp_Accounts_FcyGeneralLedger_Rpt | `BankingPrintRequests.Rpt105DFcyGeneralLedgerRequest` |
| 123-AcRptPdcInventorySlip.rpt | `/reports/print/123-pdc-inventory-slip` | Sp_PdcInventoryHeader_rpt | `BankingPrintRequests.Rpt123PdcInventorySlipRequest` |
| 127-PdcInventoryPendingGuriWise.rpt | `/reports/print/127-pdc-inventory-pending-guri-wise` | Sp_PdcInventory_SlipAndRegister_Rpt | `BankingPrintRequests.Rpt127PdcInventoryPendingGuriWiseRequest` |
| 153-FCYPayablesAndReceivablesRpt.rpt | `/reports/print/153-fcy-payables-and-receivables` | SPU_Accounts_FCYPayablesAndReceivables_Rpt | `BankingPrintRequests.Rpt153FCYPayablesAndReceivablesRequest` |
| 156-FcyGeneralLedger.rpt | `/reports/print/156-fcy-general-ledger` | SPU_Accounts_FCYCustomerLedger_Rpt | `BankingPrintRequests.Rpt156FcyGeneralLedgerRequest` |
| 176-FcyAdjustmentVoucherSlip.rpt | `/reports/print/176-fcy-adjustment-voucher-slip` | USp_FcyAdjustmentVouchers_Rpt | `BankingPrintRequests.Rpt176FcyAdjustmentVoucherSlipRequest` |
| 1801_LoanRegistration_Slip.rpt | `/reports/print/1801-loan-registration-slip` | [Fcm].[USP_LoanAgreementSlipAndRegister] | `BankingPrintRequests.Rpt1801LoanRegistrationSlipRequest` |
| 350_FcyJournalVoucherSlip.rpt | `/reports/print/350-fcy-journal-voucher-slip` | Sp_Vouchers_PaymentReceiptVoucherSlip_Rpt | `BankingPrintRequests.Rpt350FcyJournalVoucherSlipRequest` |
| 481-StockReconcilationRegister.rpt | `/reports/print/481-stock-reconcilation-register` | USP_StockReconcilation | `BankingPrintRequests.Rpt481StockReconcilationRegisterRequest` |
| 598-PaymentAndReceipts_CustomersAndSupplirWise.rpt | `/reports/print/598-payment-and-receipts-customers-and-supplir-wise` | USP_PaymentAndReceipts_CustomersAndSupplirWise | `BankingPrintRequests.Rpt598PaymentAndReceiptsCustomersAndSupplirWiseRequest` |
| DocumentTypeId-Loan Facility Agreement_Slip.rpt | `/reports/print/document-type-id-loan-facility-agreement-slip` | [Fcm].[USP_LoanAgreementSlipAndRegister] | `BankingPrintRequests.RptDocumentTypeIdLoanFacilityAgreementSlipRequest` |

## CommissionAgent

| Template | GET / POST endpoint | Procedure | Request |
|---|---|---|---|
| 1050_BuyerInquiryBookingSlip.rpt | `/reports/print/1050-buyer-inquiry-booking-slip` | [cmagt].[USP_inquiryBookingMaster_Slip] | `CommissionAgentPrintRequests.Rpt1050BuyerInquiryBookingSlipRequest` |
| 1055_GdnBuyerDispatchSlip.rpt | `/reports/print/1055-gdn-buyer-dispatch-slip` | [cmagt].[USP_gdnBuyerDispatchMaster_Slip] | `CommissionAgentPrintRequests.Rpt1055GdnBuyerDispatchSlipRequest` |
| 1054_GrnSupplierLoadingSlip.rpt | `/reports/print/1054-grn-supplier-loading-slip` | [cmagt].[usp_grnSupplierLoadingMaster_Slip] | `CommissionAgentPrintRequests.Rpt1054GrnSupplierLoadingSlipRequest` |
| 1054_01_GrnSupplierLoadingChallanSlip.rpt | `/reports/print/1054-01-grn-supplier-loading-challan-slip` | [cmagt].[usp_grnSupplierLoadingMaster_Slip] | `CommissionAgentPrintRequests.Rpt105401GrnSupplierLoadingChallanSlipRequest` |
| 1052_PurchaseOrderSlip.rpt | `/reports/print/1052-purchase-order-slip` | [cmagt].[USP_purchaseOrderMaster_Slip] | `CommissionAgentPrintRequests.Rpt1052PurchaseOrderSlipRequest` |
| 1053_saleOrderSlip.rpt | `/reports/print/1053-sale-order-slip` | [cmagt].[usp_saleOrderMaster_Slip] | `CommissionAgentPrintRequests.Rpt1053SaleOrderSlipRequest` |
| 1050A_BuyerInquiryBookingSlip.rpt | `/reports/print/1050a-buyer-inquiry-booking-slip` | [cmagt].[USP_inquiryBookingMaster_Slip] | `CommissionAgentPrintRequests.Rpt1050ABuyerInquiryBookingSlipRequest` |
| 1051_SupplierOfferSlip.rpt | `/reports/print/1051-supplier-offer-slip` | [cmagt].[USP_purchaseOrderMaster_Slip] | `CommissionAgentPrintRequests.Rpt1051SupplierOfferSlipRequest` |

## CommissionTrading

| Template | GET / POST endpoint | Procedure | Request |
|---|---|---|---|
| 1056_01_CommissionAgentTradeBillPurchaseSlip.rpt | `/reports/print/1056-01-commission-agent-trade-bill-purchase-slip` | Sp_InvCommAgentTradeBill_SlipandRegister | `CommissionTradingPrintRequests.Rpt105601CommissionAgentTradeBillPurchaseSlipRequest` |
| 1056_02_CommissionAgentTradeBillSaleSlip.rpt | `/reports/print/1056-02-commission-agent-trade-bill-sale-slip` | Sp_InvCommAgentTradeBill_SlipandRegister | `CommissionTradingPrintRequests.Rpt105602CommissionAgentTradeBillSaleSlipRequest` |
| 1056_03_CommissionAgentTradeBillSaleSlipII.rpt | `/reports/print/1056-03-commission-agent-trade-bill-sale-slip-ii` | Sp_InvCommAgentTradeBill_SlipandRegister | `CommissionTradingPrintRequests.Rpt105603CommissionAgentTradeBillSaleSlipIIRequest` |
| 1056_CommissionAgentTradeBillSlip.rpt | `/reports/print/1056-commission-agent-trade-bill-slip` | Sp_InvCommAgentTradeBill_SlipandRegister | `CommissionTradingPrintRequests.Rpt1056CommissionAgentTradeBillSlipRequest` |
| 1056A_CommissionBillIncomeSlip.rpt | `/reports/print/1056a-commission-bill-income-slip` | USP_CommissionBillSupplierCustomerAndIncomeTransactions | `CommissionTradingPrintRequests.Rpt1056ACommissionBillIncomeSlipRequest` |
| 1702-CommissionTradeLoadingDelivery_Register.rpt | `/reports/print/1702-commission-trade-loading-delivery-register` | [CmTr].[USP_CommTradeLoadingDelivery_Register] | `CommissionTradingPrintRequests.Rpt1702CommissionTradeLoadingDeliveryRegisterRequest` |
| 1702-CommissionTradeLoadingDelivery_Slip.rpt | `/reports/print/1702-commission-trade-loading-delivery-slip` | [CmTr].[USP_CommTradeLoadingDelivery_Register] | `CommissionTradingPrintRequests.Rpt1702CommissionTradeLoadingDeliverySlipRequest` |
| 1702A-CommissionTradeLoadingDelivery_Slip.rpt | `/reports/print/1702a-commission-trade-loading-delivery-slip` | [CmTr].[USP_CommTradeLoadingDelivery_Register] | `CommissionTradingPrintRequests.Rpt1702ACommissionTradeLoadingDeliverySlipRequest` |
| 1704-CommissionTransaction_Register.rpt | `/reports/print/1704-commission-transaction-register` | [CmTr].[USP_CommTradeTransactionHeader_Register] | `CommissionTradingPrintRequests.Rpt1704CommissionTransactionRegisterRequest` |
| 1704-CommTradeTransactionCustomerExpense_SubReport.rpt | `/reports/print/1704-comm-trade-transaction-customer-expense-sub-report` | [CmTr].[USP_CommTradeTransactionCustomerExpense_SubReport] | `CommissionTradingPrintRequests.Rpt1704CommTradeTransactionCustomerExpenseSubReportRequest` |
| 1704-CommTradeTransactionHeader_Slip.rpt | `/reports/print/1704-comm-trade-transaction-header-slip` | [CmTr].[USP_CommTradeTransactionHeader_Register] | `CommissionTradingPrintRequests.Rpt1704CommTradeTransactionHeaderSlipRequest` |
| 1704_01_CommTradeTransaction_SlipWithSupplier.rpt | `/reports/print/1704-01-comm-trade-transaction-slip-with-supplier` | [CmTr].[USP_CommTradeTransactionHeader_Register] | `CommissionTradingPrintRequests.Rpt170401CommTradeTransactionSlipWithSupplierRequest` |
| 1704_01_CommTradeTransactionSupplierSummary_SubReport.rpt | `/reports/print/1704-01-comm-trade-transaction-supplier-summary-sub-report` | [CmTr].[USP_CommTradeTransactionSupplierSummary_SubReport] | `CommissionTradingPrintRequests.Rpt170401CommTradeTransactionSupplierSummarySubReportRequest` |
| 1705-CommissionOrder_Register.rpt | `/reports/print/1705-commission-order-register` | [CmTr].[USP_CommTradeOrder_Register] | `CommissionTradingPrintRequests.Rpt1705CommissionOrderRegisterRequest` |
| 1705-CommissionTradeOrder_Slip.rpt | `/reports/print/1705-commission-trade-order-slip` | [CmTr].[USP_CommTradeOrder_Register] | `CommissionTradingPrintRequests.Rpt1705CommissionTradeOrderSlipRequest` |
| 1706-CommissionBillSupplierCustomerAndIncomeTransactions.rpt | `/reports/print/1706-commission-bill-supplier-customer-and-income-transactions` | USP_CommissionBillSupplierCustomerAndIncomeTransactions | `CommissionTradingPrintRequests.Rpt1706CommissionBillSupplierCustomerAndIncomeTransactionsRequest` |
| 238-InvPurchaseInvoice_StoreBillWithTax.rpt | `/reports/print/238-inv-purchase-invoice-store-bill-with-tax` | SP_CommisionAgentBillOtherExpense_SubRpt | `CommissionTradingPrintRequests.Rpt238InvPurchaseInvoiceStoreBillWithTaxRequest` |
| 477-InventoryTransaction.rpt | `/reports/print/477-inventory-transaction` | [CmTr].[USP_InventoryTransactionReport] | `CommissionTradingPrintRequests.Rpt477InventoryTransactionRequest` |
| 478-EvaluationTransaction.rpt | `/reports/print/478-evaluation-transaction` | [CmTr].[USP_EvaluationTransactionReport] | `CommissionTradingPrintRequests.Rpt478EvaluationTransactionRequest` |
| 479_01_ItemStockSummary.rpt | `/reports/print/479-01-item-stock-summary` | [CmTr].[USP_ItemStockReportWithValues] | `CommissionTradingPrintRequests.Rpt47901ItemStockSummaryRequest` |
| 479_02_ItemandWarehouseStockSummary.rpt | `/reports/print/479-02-itemand-warehouse-stock-summary` | [CmTr].[USP_ItemStockReportWithValues] | `CommissionTradingPrintRequests.Rpt47902ItemandWarehouseStockSummaryRequest` |
| 479_03_WarehouseAndItemStockSummary.rpt | `/reports/print/479-03-warehouse-and-item-stock-summary` | [CmTr].[USP_ItemStockReportWithValues] | `CommissionTradingPrintRequests.Rpt47903WarehouseAndItemStockSummaryRequest` |
| 479_04_JobLotandItemStockSummary.rpt | `/reports/print/479-04-job-lotand-item-stock-summary` | [CmTr].[USP_ItemStockReportWithValues] | `CommissionTradingPrintRequests.Rpt47904JobLotandItemStockSummaryRequest` |
| 479_05_WarehouseandJoblotandItemStockSummary.rpt | `/reports/print/479-05-warehouseand-joblotand-item-stock-summary` | [CmTr].[USP_ItemStockReportWithValues] | `CommissionTradingPrintRequests.Rpt47905WarehouseandJoblotandItemStockSummaryRequest` |
| 479_06_WarehouseandItemandJoblotStockSummary.rpt | `/reports/print/479-06-warehouseand-itemand-joblot-stock-summary` | [CmTr].[USP_ItemStockReportWithValues] | `CommissionTradingPrintRequests.Rpt47906WarehouseandItemandJoblotStockSummaryRequest` |
| 479_07_ItemandPackSizeStockSummary.rpt | `/reports/print/479-07-itemand-pack-size-stock-summary` | [CmTr].[USP_ItemStockReportWithValues] | `CommissionTradingPrintRequests.Rpt47907ItemandPackSizeStockSummaryRequest` |
| 479_08_ItemandItemAttributeVarientStockSummary.rpt | `/reports/print/479-08-itemand-item-attribute-varient-stock-summary` | [CmTr].[USP_ItemStockReportWithValues] | `CommissionTradingPrintRequests.Rpt47908ItemandItemAttributeVarientStockSummaryRequest` |
| 479_09_ItemandPackSizeandItemAttributeVarientStockSummary.rpt | `/reports/print/479-09-itemand-pack-sizeand-item-attribute-varient-stock-summary` | [CmTr].[USP_ItemStockReportWithValues] | `CommissionTradingPrintRequests.Rpt47909ItemandPackSizeandItemAttributeVarientStockSummaryRequest` |
| 480_01_ItemStockSummary.rpt | `/reports/print/480-01-item-stock-summary` | [CmTr].[USP_StockReportWithOutValues] | `CommissionTradingPrintRequests.Rpt48001ItemStockSummaryRequest` |
| 480_02_ItemandWarehouseStockSummary.rpt | `/reports/print/480-02-itemand-warehouse-stock-summary` | [CmTr].[USP_StockReportWithOutValues] | `CommissionTradingPrintRequests.Rpt48002ItemandWarehouseStockSummaryRequest` |
| 480_03_WarehouseAndItemStockSummary.rpt | `/reports/print/480-03-warehouse-and-item-stock-summary` | [CmTr].[USP_StockReportWithOutValues] | `CommissionTradingPrintRequests.Rpt48003WarehouseAndItemStockSummaryRequest` |
| 480_06_JobLotandItemStockSummary.rpt | `/reports/print/480-06-job-lotand-item-stock-summary` | [CmTr].[USP_StockReportWithOutValues] | `CommissionTradingPrintRequests.Rpt48006JobLotandItemStockSummaryRequest` |
| 480_07_ItemWarehouseAndJoblotStockSummary.rpt | `/reports/print/480-07-item-warehouse-and-joblot-stock-summary` | [CmTr].[USP_StockReportWithOutValues] | `CommissionTradingPrintRequests.Rpt48007ItemWarehouseAndJoblotStockSummaryRequest` |
| 480_08_ItemandPackSizeStockSummary.rpt | `/reports/print/480-08-itemand-pack-size-stock-summary` | [CmTr].[USP_StockReportWithOutValues] | `CommissionTradingPrintRequests.Rpt48008ItemandPackSizeStockSummaryRequest` |
| 480_09_ItemandItemAttributeVarientStockSummary.rpt | `/reports/print/480-09-itemand-item-attribute-varient-stock-summary` | [CmTr].[USP_StockReportWithOutValues] | `CommissionTradingPrintRequests.Rpt48009ItemandItemAttributeVarientStockSummaryRequest` |
| 480_10_ItemandPackSizeandItemAttributeVarientStockSummary.rpt | `/reports/print/480-10-itemand-pack-sizeand-item-attribute-varient-stock-summary` | [CmTr].[USP_StockReportWithOutValues] | `CommissionTradingPrintRequests.Rpt48010ItemandPackSizeandItemAttributeVarientStockSummaryRequest` |
| 556-CommissionTradeLoadingDelivery_Slip.rpt | `/reports/print/556-commission-trade-loading-delivery-slip` | [CmTr].[USP_CommTradeLoadingDelivery_Register] | `CommissionTradingPrintRequests.Rpt556CommissionTradeLoadingDeliverySlipRequest` |
| CommTradeLoadingDeliveryExpense_SubReport.rpt | `/reports/print/comm-trade-loading-delivery-expense-sub-report` | [CmTr].[USP_CommTradeLoadingDeliveryExpense_SubReport] | `CommissionTradingPrintRequests.RptCommTradeLoadingDeliveryExpenseSubReportRequest` |
| CommTradeLoadingDeliveryPMExpense_SubReport.rpt | `/reports/print/comm-trade-loading-delivery-pm-expense-sub-report` | [CmTr].[USP_CommTradeLoadingDeliveryPMExpense_SubReport] | `CommissionTradingPrintRequests.RptCommTradeLoadingDeliveryPMExpenseSubReportRequest` |
| CommTradeTransactionPM_SubReport.rpt | `/reports/print/comm-trade-transaction-pm-sub-report` | [CmTr].[USP_CommTradeTransactionPM_SubReport] | `CommissionTradingPrintRequests.RptCommTradeTransactionPMSubReportRequest` |
| InvCommissionAgentBillOtherExp.rpt | `/reports/print/inv-commission-agent-bill-other-exp` | SP_CommisionAgentBillOtherExpense_SubRpt | `CommissionTradingPrintRequests.RptInvCommissionAgentBillOtherExpRequest` |

## Concrete

| Template | GET / POST endpoint | Procedure | Request |
|---|---|---|---|
| 1104_PurchaseInvoiceDirectItemSlip.rpt | `/reports/print/1104-purchase-invoice-direct-item-slip` | [pcc].[USP_InvPurchaseInvoice_DirectSlip] | `ConcretePrintRequests.Rpt1104PurchaseInvoiceDirectItemSlipRequest` |
| 1104A_PurchaseInvoiceDirectPartySlip.rpt | `/reports/print/1104a-purchase-invoice-direct-party-slip` | [pcc].[USP_InvPurchaseInvoice_DirectSlip] | `ConcretePrintRequests.Rpt1104APurchaseInvoiceDirectPartySlipRequest` |
| 1120_Recipe_Slip.rpt | `/reports/print/1120-recipe-slip` | [pcc].[USP_BillOfMaterial_Register] | `ConcretePrintRequests.Rpt1120RecipeSlipRequest` |
| 1120_Recipe_SubReport.rpt | `/reports/print/1120-recipe-sub-report` | [pcc].[USP_BillOfMaterialAllDetails_SubReport] | `ConcretePrintRequests.Rpt1120RecipeSubReportRequest` |
| 1121_Production_Slip.rpt | `/reports/print/1121-production-slip` | [pcc].[USP_Production_SlipAndRegister] | `ConcretePrintRequests.Rpt1121ProductionSlipRequest` |
| 1122_Production_Slip.rpt | `/reports/print/1122-production-slip` | [pcc].[USP_Production_SlipAndRegister] | `ConcretePrintRequests.Rpt1122ProductionSlipRequest` |
| 126_01-SalesDetailRegisterStoreAndPm.rpt | `/reports/print/126-01-sales-detail-register-store-and-pm` | pcc.USP_GetDataForDropDownFromGdn | `ConcretePrintRequests.Rpt12601SalesDetailRegisterStoreAndPmRequest` |
| 126_02-SalesCustomerAndItemWiseRegisterStoreAndPm.rpt | `/reports/print/126-02-sales-customer-and-item-wise-register-store-and-pm` | pcc.USP_GetDataForDropDownFromGdn | `ConcretePrintRequests.Rpt12602SalesCustomerAndItemWiseRegisterStoreAndPmRequest` |
| 126_03-SalesItemAndConditionWiseRegisterStoreAndPm.rpt | `/reports/print/126-03-sales-item-and-condition-wise-register-store-and-pm` | pcc.USP_GetDataForDropDownFromGdn | `ConcretePrintRequests.Rpt12603SalesItemAndConditionWiseRegisterStoreAndPmRequest` |
| 1605_01_OrderRegister.rpt | `/reports/print/1605-01-order-register` | [pcc].[USP_SaleOrderSummaryRegister] | `ConcretePrintRequests.Rpt160501OrderRegisterRequest` |
| 1605_02_OrderSummaryByItem&PackSize.rpt | `/reports/print/1605-02-order-summary-by-item-pack-size` | [pcc].[USP_SaleOrderSummaryRegister] | `ConcretePrintRequests.Rpt160502OrderSummaryByItemPackSizeRequest` |
| 1605_03_OrderSummaryByItem,PackSize&City.rpt | `/reports/print/1605-03-order-summary-by-item-pack-size-city` | [pcc].[USP_SaleOrderSummaryRegister] | `ConcretePrintRequests.Rpt160503OrderSummaryByItemPackSizeCityRequest` |
| 1605_04_OrderSummaryByCustomer&PackSize.rpt | `/reports/print/1605-04-order-summary-by-customer-pack-size` | [pcc].[USP_SaleOrderSummaryRegister] | `ConcretePrintRequests.Rpt160504OrderSummaryByCustomerPackSizeRequest` |
| 1605_05_OrderSummaryByItem.rpt | `/reports/print/1605-05-order-summary-by-item` | [pcc].[USP_SaleOrderSummaryRegister] | `ConcretePrintRequests.Rpt160505OrderSummaryByItemRequest` |
| 1605_06_OrderSummaryByItem&City.rpt | `/reports/print/1605-06-order-summary-by-item-city` | [pcc].[USP_SaleOrderSummaryRegister] | `ConcretePrintRequests.Rpt160506OrderSummaryByItemCityRequest` |
| 1605_07_OrderSummaryByCustomer.rpt | `/reports/print/1605-07-order-summary-by-customer` | [pcc].[USP_SaleOrderSummaryRegister] | `ConcretePrintRequests.Rpt160507OrderSummaryByCustomerRequest` |
| 1605_08_OrderSummaryByCustomer&City.rpt | `/reports/print/1605-08-order-summary-by-customer-city` | [pcc].[USP_SaleOrderSummaryRegister] | `ConcretePrintRequests.Rpt160508OrderSummaryByCustomerCityRequest` |
| 1605_09_OrderSummaryByCustomer&Item.rpt | `/reports/print/1605-09-order-summary-by-customer-item` | [pcc].[USP_SaleOrderSummaryRegister] | `ConcretePrintRequests.Rpt160509OrderSummaryByCustomerItemRequest` |
| 1605_10_OrderSummaryByCustomer,Item&City.rpt | `/reports/print/1605-10-order-summary-by-customer-item-city` | [pcc].[USP_SaleOrderSummaryRegister] | `ConcretePrintRequests.Rpt160510OrderSummaryByCustomerItemCityRequest` |
| 1605_11_OrderSummaryByCustomer&Order.rpt | `/reports/print/1605-11-order-summary-by-customer-order` | [pcc].[USP_SaleOrderSummaryRegister] | `ConcretePrintRequests.Rpt160511OrderSummaryByCustomerOrderRequest` |
| 1617_USP_BillOfMaterial_Slip.rpt | `/reports/print/1617-usp-bill-of-material-slip` | [pcc].[USP_BillOfMaterial_Register] | `ConcretePrintRequests.Rpt1617USPBillOfMaterialSlipRequest` |
| 1633_USP_BillOfMaterial_Slip.rpt | `/reports/print/1633-usp-bill-of-material-slip` | [pcc].[USP_BillOfMaterial_Register] | `ConcretePrintRequests.Rpt1633USPBillOfMaterialSlipRequest` |
| 1634_BillOfMaterialAllDetails_SubReport.rpt | `/reports/print/1634-bill-of-material-all-details-sub-report` | [pcc].[USP_BillOfMaterialAllDetails_SubReport] | `ConcretePrintRequests.Rpt1634BillOfMaterialAllDetailsSubReportRequest` |
| 1664_BillOfMaterialCasting_Slip.rpt | `/reports/print/1664-bill-of-material-casting-slip` | [pcc].[USP_BillOfMaterial_Register] | `ConcretePrintRequests.Rpt1664BillOfMaterialCastingSlipRequest` |
| 1850_Production_Slip.rpt | `/reports/print/1850-production-slip` | [pcc].[USP_Production_SlipAndRegister] | `ConcretePrintRequests.Rpt1850ProductionSlipRequest` |
| 1851_USP_BillOfMaterial_Slip.rpt | `/reports/print/1851-usp-bill-of-material-slip` | [pcc].[USP_BillOfMaterial_Register] | `ConcretePrintRequests.Rpt1851USPBillOfMaterialSlipRequest` |
| 1852-ApprovalRegister.rpt | `/reports/print/1852-approval-register` | [pcc].[USP_SaleOrderApprovalHistory] | `ConcretePrintRequests.Rpt1852ApprovalRegisterRequest` |
| 1852-InvRptSaleOrderSlip.rpt | `/reports/print/1852-sale-order-slip` | [pcc].[USP_SaleOrderSlipAndRegister] | `ConcretePrintRequests.Rpt1852SaleOrderSlipRequest` |
| 1852-SaleOrderRegister.rpt | `/reports/print/1852-sale-order-register` | [pcc].[USP_SaleOrderSlipAndRegister] | `ConcretePrintRequests.Rpt1852SaleOrderRegisterRequest` |
| 1852_01-OrderRegister.rpt | `/reports/print/1852-01-order-register` | [pcc].[USP_SaleOrderSummaryRegister] | `ConcretePrintRequests.Rpt185201OrderRegisterRequest` |
| 1852_02-OrderSummaryByItem.rpt | `/reports/print/1852-02-order-summary-by-item` | [pcc].[USP_SaleOrderSummaryRegister] | `ConcretePrintRequests.Rpt185202OrderSummaryByItemRequest` |
| 1852_03-OrderSummaryByItem&Varient.rpt | `/reports/print/1852-03-order-summary-by-item-varient` | [pcc].[USP_SaleOrderSummaryRegister] | `ConcretePrintRequests.Rpt185203OrderSummaryByItemVarientRequest` |
| 1852_04-OrderSummaryByItem&City.rpt | `/reports/print/1852-04-order-summary-by-item-city` | [pcc].[USP_SaleOrderSummaryRegister] | `ConcretePrintRequests.Rpt185204OrderSummaryByItemCityRequest` |
| 1852_05-OrderSummaryByItem_Varient&City.rpt | `/reports/print/1852-05-order-summary-by-item-varient-city` | [pcc].[USP_SaleOrderSummaryRegister] | `ConcretePrintRequests.Rpt185205OrderSummaryByItemVarientCityRequest` |
| 1852_06-OrderSummaryByCustomer.rpt | `/reports/print/1852-06-order-summary-by-customer` | [pcc].[USP_SaleOrderSummaryRegister] | `ConcretePrintRequests.Rpt185206OrderSummaryByCustomerRequest` |
| 1852_07-OrderSummaryByCustomerItem.rpt | `/reports/print/1852-07-order-summary-by-customer-item` | [pcc].[USP_SaleOrderSummaryRegister] | `ConcretePrintRequests.Rpt185207OrderSummaryByCustomerItemRequest` |
| 1852_08-OrderSummaryByCustomer&City.rpt | `/reports/print/1852-08-order-summary-by-customer-city` | [pcc].[USP_SaleOrderSummaryRegister] | `ConcretePrintRequests.Rpt185208OrderSummaryByCustomerCityRequest` |
| 1852_09-OrderSummaryByCustomer_Item&City.rpt | `/reports/print/1852-09-order-summary-by-customer-item-city` | [pcc].[USP_SaleOrderSummaryRegister] | `ConcretePrintRequests.Rpt185209OrderSummaryByCustomerItemCityRequest` |
| 1852_10-OrderSummaryByCustomer&Varient.rpt | `/reports/print/1852-10-order-summary-by-customer-varient` | [pcc].[USP_SaleOrderSummaryRegister] | `ConcretePrintRequests.Rpt185210OrderSummaryByCustomerVarientRequest` |
| 1852_11-OrderSummaryByCustomer_ItemVarient.rpt | `/reports/print/1852-11-order-summary-by-customer-item-varient` | [pcc].[USP_SaleOrderSummaryRegister] | `ConcretePrintRequests.Rpt185211OrderSummaryByCustomerItemVarientRequest` |
| 1852_12-OrderSummaryByCustomer&ReferenceParty.rpt | `/reports/print/1852-12-order-summary-by-customer-reference-party` | [pcc].[USP_SaleOrderSummaryRegister] | `ConcretePrintRequests.Rpt185212OrderSummaryByCustomerReferencePartyRequest` |
| 1852_13-OrderSummaryByCustomerItem&ReferenceParty.rpt | `/reports/print/1852-13-order-summary-by-customer-item-reference-party` | [pcc].[USP_SaleOrderSummaryRegister] | `ConcretePrintRequests.Rpt185213OrderSummaryByCustomerItemReferencePartyRequest` |
| 1852_14-OrderSummaryByCustomerVarient&ReferenceParty.rpt | `/reports/print/1852-14-order-summary-by-customer-varient-reference-party` | [pcc].[USP_SaleOrderSummaryRegister] | `ConcretePrintRequests.Rpt185214OrderSummaryByCustomerVarientReferencePartyRequest` |
| 1852_15-OrderSummaryByCustomer_Item_Varient&ReferenceParty.rpt | `/reports/print/1852-15-order-summary-by-customer-item-varient-reference-party` | [pcc].[USP_SaleOrderSummaryRegister] | `ConcretePrintRequests.Rpt185215OrderSummaryByCustomerItemVarientReferencePartyRequest` |
| 1852_16-OrderSummaryByReferenceParty.rpt | `/reports/print/1852-16-order-summary-by-reference-party` | [pcc].[USP_SaleOrderSummaryRegister] | `ConcretePrintRequests.Rpt185216OrderSummaryByReferencePartyRequest` |
| 1852_17-OrderSummaryByReferenceParty&Item.rpt | `/reports/print/1852-17-order-summary-by-reference-party-item` | [pcc].[USP_SaleOrderSummaryRegister] | `ConcretePrintRequests.Rpt185217OrderSummaryByReferencePartyItemRequest` |
| 1852_18-OrderSummaryByReferenceParty&Varient.rpt | `/reports/print/1852-18-order-summary-by-reference-party-varient` | [pcc].[USP_SaleOrderSummaryRegister] | `ConcretePrintRequests.Rpt185218OrderSummaryByReferencePartyVarientRequest` |
| 1852_19-OrderSummaryByReferenceParty_Item&Varient.rpt | `/reports/print/1852-19-order-summary-by-reference-party-item-varient` | [pcc].[USP_SaleOrderSummaryRegister] | `ConcretePrintRequests.Rpt185219OrderSummaryByReferencePartyItemVarientRequest` |
| 1852_20-OrderSummaryByReferenceParty&City.rpt | `/reports/print/1852-20-order-summary-by-reference-party-city` | [pcc].[USP_SaleOrderSummaryRegister] | `ConcretePrintRequests.Rpt185220OrderSummaryByReferencePartyCityRequest` |
| 1852_21-dtOrderSummaryByOrderNoAndCustomer.rpt | `/reports/print/1852-21-dt-order-summary-by-order-no-and-customer` | [pcc].[USP_SaleOrderSummaryRegister] | `ConcretePrintRequests.Rpt185221DtOrderSummaryByOrderNoAndCustomerRequest` |
| 1852_22-OrderSummaryByOrderNoAndSalesMan.rpt | `/reports/print/1852-22-order-summary-by-order-no-and-sales-man` | [pcc].[USP_SaleOrderSummaryRegister] | `ConcretePrintRequests.Rpt185222OrderSummaryByOrderNoAndSalesManRequest` |
| 1852A-SaleOrder_Register.rpt | `/reports/print/1852a-sale-order-register` | [pcc].[USP_SaleOrderSlipAndRegister] | `ConcretePrintRequests.Rpt1852ASaleOrderRegisterRequest` |
| 1853-DeliveryOrderSlip.rpt | `/reports/print/1853-delivery-order-slip` | [pcc].[USP_InvDeliveryOrder_Slip] | `ConcretePrintRequests.Rpt1853DeliveryOrderSlipRequest` |
| 1853A-DeliveryOrderSlip.rpt | `/reports/print/1853a-delivery-order-slip` | [pcc].[USP_InvDeliveryOrder_Slip] | `ConcretePrintRequests.Rpt1853ADeliveryOrderSlipRequest` |
| 1855-GDNRegister.rpt | `/reports/print/1855-gdn-register` | [pcc].[USP_GdnRegister] | `ConcretePrintRequests.Rpt1855GDNRegisterRequest` |
| 1855-InvGdn_Slip.rpt | `/reports/print/1855-inv-gdn-slip` | [pcc].[USP_InvGdn_Slip] | `ConcretePrintRequests.Rpt1855InvGdnSlipRequest` |
| 1855A-GDNRegister.rpt | `/reports/print/1855a-gdn-register` | [pcc].[USP_GdnRegister] | `ConcretePrintRequests.Rpt1855AGDNRegisterRequest` |
| 1856A-SaleInvoiceDirect_Slip.rpt | `/reports/print/1856a-sale-invoice-direct-slip` | pcc.USP_InvSaleInvoice_DirectSlip | `ConcretePrintRequests.Rpt1856ASaleInvoiceDirectSlipRequest` |
| 1859-PurchaseInvoice_ApprovalRegister.rpt | `/reports/print/1859-purchase-invoice-approval-register` | [pcc].[USP_PurchaseInvoice_ApprovalHistory] | `ConcretePrintRequests.Rpt1859PurchaseInvoiceApprovalRegisterRequest` |
| 1859_01-PurchaseRegisterSummary.rpt | `/reports/print/1859-01-purchase-register-summary` | [pcc].[USP_Purchase_EvaulationDetailReports] | `ConcretePrintRequests.Rpt185901PurchaseRegisterSummaryRequest` |
| 1859_02-PurchaseRegisterSummaryByItem.rpt | `/reports/print/1859-02-purchase-register-summary-by-item` | [pcc].[USP_Purchase_EvaulationDetailReports] | `ConcretePrintRequests.Rpt185902PurchaseRegisterSummaryByItemRequest` |
| 1859_03-PurchaseSummaryByItem&City.rpt | `/reports/print/1859-03-purchase-summary-by-item-city` | [pcc].[USP_Purchase_EvaulationDetailReports] | `ConcretePrintRequests.Rpt185903PurchaseSummaryByItemCityRequest` |
| 1859_04-PurchaseRegisterSummaryByItem&Varient.rpt | `/reports/print/1859-04-purchase-register-summary-by-item-varient` | [pcc].[USP_Purchase_EvaulationDetailReports] | `ConcretePrintRequests.Rpt185904PurchaseRegisterSummaryByItemVarientRequest` |
| 1859_05-PurchaseRegisterSummaryByItem&Warehouse.rpt | `/reports/print/1859-05-purchase-register-summary-by-item-warehouse` | [pcc].[USP_Purchase_EvaulationDetailReports] | `ConcretePrintRequests.Rpt185905PurchaseRegisterSummaryByItemWarehouseRequest` |
| 1859_06-PurchaseSummaryByItemPackSize&City.rpt | `/reports/print/1859-06-purchase-summary-by-item-pack-size-city` | [pcc].[USP_Purchase_EvaulationDetailReports] | `ConcretePrintRequests.Rpt185906PurchaseSummaryByItemPackSizeCityRequest` |
| 1859_07-PurchaseRegisterSummaryBySupplier.rpt | `/reports/print/1859-07-purchase-register-summary-by-supplier` | [pcc].[USP_Purchase_EvaulationDetailReports] | `ConcretePrintRequests.Rpt185907PurchaseRegisterSummaryBySupplierRequest` |
| 1859_08-PurchaseSummaryBySupplier&Item.rpt | `/reports/print/1859-08-purchase-summary-by-supplier-item` | [pcc].[USP_Purchase_EvaulationDetailReports] | `ConcretePrintRequests.Rpt185908PurchaseSummaryBySupplierItemRequest` |
| 1859_09-PurchaseRegisterSummaryBySupplier&City.rpt | `/reports/print/1859-09-purchase-register-summary-by-supplier-city` | [pcc].[USP_Purchase_EvaulationDetailReports] | `ConcretePrintRequests.Rpt185909PurchaseRegisterSummaryBySupplierCityRequest` |
| 1859_10-PurchaseSummaryBySupplierItem&City.rpt | `/reports/print/1859-10-purchase-summary-by-supplier-item-city` | [pcc].[USP_Purchase_EvaulationDetailReports] | `ConcretePrintRequests.Rpt185910PurchaseSummaryBySupplierItemCityRequest` |
| 1859_11-PurchaseSummaryBySupplier&Varient.rpt | `/reports/print/1859-11-purchase-summary-by-supplier-varient` | [pcc].[USP_Purchase_EvaulationDetailReports] | `ConcretePrintRequests.Rpt185911PurchaseSummaryBySupplierVarientRequest` |
| 1859_12-PurchaseSummaryByCity.rpt | `/reports/print/1859-12-purchase-summary-by-city` | [pcc].[USP_Purchase_EvaulationDetailReports] | `ConcretePrintRequests.Rpt185912PurchaseSummaryByCityRequest` |
| 1859_13-PurchaseSummaryByParentCategory.rpt | `/reports/print/1859-13-purchase-summary-by-parent-category` | [pcc].[USP_Purchase_EvaulationDetailReports] | `ConcretePrintRequests.Rpt185913PurchaseSummaryByParentCategoryRequest` |
| 1859_14-PurchaseSummaryByParentCategory&Item.rpt | `/reports/print/1859-14-purchase-summary-by-parent-category-item` | [pcc].[USP_Purchase_EvaulationDetailReports] | `ConcretePrintRequests.Rpt185914PurchaseSummaryByParentCategoryItemRequest` |
| 1859_15-PurchaseSummaryByParentCategory&Supplier.rpt | `/reports/print/1859-15-purchase-summary-by-parent-category-supplier` | [pcc].[USP_Purchase_EvaulationDetailReports] | `ConcretePrintRequests.Rpt185915PurchaseSummaryByParentCategorySupplierRequest` |
| 1859_InvPurchaseInvoice_DirectSlip.rpt | `/reports/print/1859-inv-purchase-invoice-direct-slip` | [pcc].[USP_InvPurchaseInvoice_DirectSlip] | `ConcretePrintRequests.Rpt1859InvPurchaseInvoiceDirectSlipRequest` |
| 1859A_InvPurchaseInvoice_DirectSlip.rpt | `/reports/print/1859a-inv-purchase-invoice-direct-slip` | [pcc].[USP_InvPurchaseInvoice_DirectSlip] | `ConcretePrintRequests.Rpt1859AInvPurchaseInvoiceDirectSlipRequest` |
| 1860-StockSummaryDetail.rpt | `/reports/print/1860-stock-summary-detail` | [pcc].[USP_ItemStockReportWithOutValues] | `ConcretePrintRequests.Rpt1860StockSummaryDetailRequest` |
| 1860-StockTransferManual-Slip.rpt | `/reports/print/1860-stock-transfer-manual-slip` | [Mfg].[USP_InvStockTransferSlipRegister] | `ConcretePrintRequests.Rpt1860StockTransferManualSlipRequest` |
| 1860-StockTransferManual_Register.rpt | `/reports/print/1860-stock-transfer-manual-register` | [pcc].[USP_InvStockTransferSlipRegister] | `ConcretePrintRequests.Rpt1860StockTransferManualRegisterRequest` |
| 1861-ItemStockSummary.rpt | `/reports/print/1861-item-stock-summary` | [pcc].[USP_ItemStockReportWithOutValues] | `ConcretePrintRequests.Rpt1861ItemStockSummaryRequest` |
| 1861-SaleInvoice_Slip.rpt | `/reports/print/1861-sale-invoice-slip` | [pcc].[USP_InvSaleInvoice_Slip] | `ConcretePrintRequests.Rpt1861SaleInvoiceSlipRequest` |
| 1861-SaleInvoiceApprovalRegister.rpt | `/reports/print/1861-sale-invoice-approval-register` | [pcc].[USP_SaleInvoice_ApprovalHistory] | `ConcretePrintRequests.Rpt1861SaleInvoiceApprovalRegisterRequest` |
| 1861A-SaleInvoice_Slip.rpt | `/reports/print/1861a-sale-invoice-slip` | [pcc].[USP_InvSaleInvoice_Slip] | `ConcretePrintRequests.Rpt1861ASaleInvoiceSlipRequest` |
| 1862-ItemandWarehouseStockSummary.rpt | `/reports/print/1862-itemand-warehouse-stock-summary` | [pcc].[USP_ItemStockReportWithOutValues] | `ConcretePrintRequests.Rpt1862ItemandWarehouseStockSummaryRequest` |
| 1863-WarehouseAndItemStockSummary.rpt | `/reports/print/1863-warehouse-and-item-stock-summary` | [pcc].[USP_ItemStockReportWithOutValues] | `ConcretePrintRequests.Rpt1863WarehouseAndItemStockSummaryRequest` |
| 1863_01-OrderRegister.rpt | `/reports/print/1863-01-order-register` | [pcc].[USP_PurchaseOrderSummaryRegister] | `ConcretePrintRequests.Rpt186301OrderRegisterRequest` |
| 1863_02-OrderSummaryByItem.rpt | `/reports/print/1863-02-order-summary-by-item` | [pcc].[USP_PurchaseOrderSummaryRegister] | `ConcretePrintRequests.Rpt186302OrderSummaryByItemRequest` |
| 1863_03-OrderSummaryByItem&Varient.rpt | `/reports/print/1863-03-order-summary-by-item-varient` | [pcc].[USP_PurchaseOrderSummaryRegister] | `ConcretePrintRequests.Rpt186303OrderSummaryByItemVarientRequest` |
| 1863_04-OrderSummaryByItem&City.rpt | `/reports/print/1863-04-order-summary-by-item-city` | [pcc].[USP_PurchaseOrderSummaryRegister] | `ConcretePrintRequests.Rpt186304OrderSummaryByItemCityRequest` |
| 1863_05-OrderSummaryByItem_Varient&City.rpt | `/reports/print/1863-05-order-summary-by-item-varient-city` | [pcc].[USP_PurchaseOrderSummaryRegister] | `ConcretePrintRequests.Rpt186305OrderSummaryByItemVarientCityRequest` |
| 1863_06-OrderSummaryBySupplier.rpt | `/reports/print/1863-06-order-summary-by-supplier` | [pcc].[USP_PurchaseOrderSummaryRegister] | `ConcretePrintRequests.Rpt186306OrderSummaryBySupplierRequest` |
| 1863_07-OrderSummaryBySupplierItem.rpt | `/reports/print/1863-07-order-summary-by-supplier-item` | [pcc].[USP_PurchaseOrderSummaryRegister] | `ConcretePrintRequests.Rpt186307OrderSummaryBySupplierItemRequest` |
| 1863_08-OrderSummaryBySupplier&City.rpt | `/reports/print/1863-08-order-summary-by-supplier-city` | [pcc].[USP_PurchaseOrderSummaryRegister] | `ConcretePrintRequests.Rpt186308OrderSummaryBySupplierCityRequest` |
| 1863_09-OrderSummaryBySupplier_Item&City.rpt | `/reports/print/1863-09-order-summary-by-supplier-item-city` | [pcc].[USP_PurchaseOrderSummaryRegister] | `ConcretePrintRequests.Rpt186309OrderSummaryBySupplierItemCityRequest` |
| 1863_10-OrderSummaryBySupplier&Varient.rpt | `/reports/print/1863-10-order-summary-by-supplier-varient` | [pcc].[USP_PurchaseOrderSummaryRegister] | `ConcretePrintRequests.Rpt186310OrderSummaryBySupplierVarientRequest` |
| 1863_11-OrderSummaryBySupplier_ItemVarient.rpt | `/reports/print/1863-11-order-summary-by-supplier-item-varient` | [pcc].[USP_PurchaseOrderSummaryRegister] | `ConcretePrintRequests.Rpt186311OrderSummaryBySupplierItemVarientRequest` |
| 1863_PurchaseOrderApprovalRegister.rpt | `/reports/print/1863-purchase-order-approval-register` | [pcc].[USP_PurchaseOrderApprovalHistory] | `ConcretePrintRequests.Rpt1863PurchaseOrderApprovalRegisterRequest` |
| 1863A-PuchaseOrder_Register.rpt | `/reports/print/1863a-puchase-order-register` | [pcc].[USP_PurchaseOrder_SlipAndRegister] | `ConcretePrintRequests.Rpt1863APuchaseOrderRegisterRequest` |
| 1864-GrnRegister.rpt | `/reports/print/1864-grn-register` | [pcc].[USP_InvGrn_SlipAndRegister] | `ConcretePrintRequests.Rpt1864GrnRegisterRequest` |
| 1864-ItemandPackSizeStockSummary.rpt | `/reports/print/1864-itemand-pack-size-stock-summary` | [pcc].[USP_ItemStockReportWithOutValues] | `ConcretePrintRequests.Rpt1864ItemandPackSizeStockSummaryRequest` |
| 1864_GoodsRecieptNotesFinish.rpt | `/reports/print/1864-goods-reciept-notes-finish` | [pcc].[USP_InvGrn_SlipAndRegister] | `ConcretePrintRequests.Rpt1864GoodsRecieptNotesFinishRequest` |
| 1865_01_SalesWages_SummaryRegister.rpt | `/reports/print/1865-01-sales-wages-summary-register` | pcc.usp_SalesWages_Register_Summary | `ConcretePrintRequests.Rpt186501SalesWagesSummaryRegisterRequest` |
| 1866-InvGdnDirect_Slip.rpt | `/reports/print/1866-inv-gdn-direct-slip` | [pcc].[USP_InvGdn_Slip] | `ConcretePrintRequests.Rpt1866InvGdnDirectSlipRequest` |
| 1867_Conversion_Slip.rpt | `/reports/print/1867-conversion-slip` | [pcc].[USP_Conversion_SlipAndRegister] | `ConcretePrintRequests.Rpt1867ConversionSlipRequest` |
| 1868_ContractorWagesBillSlip.rpt | `/reports/print/1868-contractor-wages-bill-slip` | [pcc].[USP_ContractorWagesBill_Slip] | `ConcretePrintRequests.Rpt1868ContractorWagesBillSlipRequest` |
| 1869_ContractorWagesBillSlip.rpt | `/reports/print/1869-contractor-wages-bill-slip` | [pcc].[USP_ContractorWagesBill_Slip] | `ConcretePrintRequests.Rpt1869ContractorWagesBillSlipRequest` |
| 1870_01_WorkOrderDetailReport.rpt | `/reports/print/1870-01-work-order-detail-report` | [pcc].[USP_WorkOrder_Report] | `ConcretePrintRequests.Rpt187001WorkOrderDetailReportRequest` |
| 1870_WorkOrderConcrete_Slip.rpt | `/reports/print/1870-work-order-concrete-slip` | [pcc].[USP_WorkOrderHeader_Slip] | `ConcretePrintRequests.Rpt1870WorkOrderConcreteSlipRequest` |
| 1880-SalesRegisterSummary.rpt | `/reports/print/1880-sales-register-summary` | [pcc].[USP_Sales_EvaulationDetailReports] | `ConcretePrintRequests.Rpt1880SalesRegisterSummaryRequest` |
| 1881-SalesSummaryByParentCategory.rpt | `/reports/print/1881-sales-summary-by-parent-category` | [pcc].[USP_Sales_EvaulationDetailReports] | `ConcretePrintRequests.Rpt1881SalesSummaryByParentCategoryRequest` |
| 1882-SalesSummaryByParentCategory&Item.rpt | `/reports/print/1882-sales-summary-by-parent-category-item` | [pcc].[USP_Sales_EvaulationDetailReports] | `ConcretePrintRequests.Rpt1882SalesSummaryByParentCategoryItemRequest` |
| 1883-SalesSummaryByParentCategory&Customter.rpt | `/reports/print/1883-sales-summary-by-parent-category-customter` | [pcc].[USP_Sales_EvaulationDetailReports] | `ConcretePrintRequests.Rpt1883SalesSummaryByParentCategoryCustomterRequest` |
| 1884-SalesSummaryByVehicles.rpt | `/reports/print/1884-sales-summary-by-vehicles` | [pcc].[USP_Sales_EvaulationDetailReports] | `ConcretePrintRequests.Rpt1884SalesSummaryByVehiclesRequest` |
| 1885-SalesSummaryByCustomerItemandVareient.rpt | `/reports/print/1885-sales-summary-by-customer-itemand-vareient` | [pcc].[USP_Sales_EvaulationDetailReports] | `ConcretePrintRequests.Rpt1885SalesSummaryByCustomerItemandVareientRequest` |
| 1886-StockReportWithDocumentWise.rpt | `/reports/print/1886-stock-report-with-document-wise` | pcc.USP_StockReportWithDocumentWise | `ConcretePrintRequests.Rpt1886StockReportWithDocumentWiseRequest` |
| 1886_01_StockReportWithDocumentWiseIncludeOrders.rpt | `/reports/print/1886-01-stock-report-with-document-wise-include-orders` | pcc.USP_StockReportWithDocumentWiseIncludeOrders | `ConcretePrintRequests.Rpt188601StockReportWithDocumentWiseIncludeOrdersRequest` |
| 1887-GetReportByFiFo.rpt | `/reports/print/1887-get-report-by-fi-fo` | [pcc].[USP_GetStockByFifo_Report] | `ConcretePrintRequests.Rpt1887GetReportByFiFoRequest` |
| 1889-SalesRegisterSummaryByItem.rpt | `/reports/print/1889-sales-register-summary-by-item` | [pcc].[USP_Sales_EvaulationDetailReports] | `ConcretePrintRequests.Rpt1889SalesRegisterSummaryByItemRequest` |
| 1890-SalesSummaryByItem&City.rpt | `/reports/print/1890-sales-summary-by-item-city` | [pcc].[USP_Sales_EvaulationDetailReports] | `ConcretePrintRequests.Rpt1890SalesSummaryByItemCityRequest` |
| 1891-SalesRegisterSummaryByItem&Varient.rpt | `/reports/print/1891-sales-register-summary-by-item-varient` | [pcc].[USP_Sales_EvaulationDetailReports] | `ConcretePrintRequests.Rpt1891SalesRegisterSummaryByItemVarientRequest` |
| 1892-SalesRegisterSummaryByItem&Warehouse.rpt | `/reports/print/1892-sales-register-summary-by-item-warehouse` | [pcc].[USP_Sales_EvaulationDetailReports] | `ConcretePrintRequests.Rpt1892SalesRegisterSummaryByItemWarehouseRequest` |
| 1893-SalesSummaryByItemPackSize&City.rpt | `/reports/print/1893-sales-summary-by-item-pack-size-city` | [pcc].[USP_Sales_EvaulationDetailReports] | `ConcretePrintRequests.Rpt1893SalesSummaryByItemPackSizeCityRequest` |
| 1894-SalesRegisterSummaryByCustomer.rpt | `/reports/print/1894-sales-register-summary-by-customer` | [pcc].[USP_Sales_EvaulationDetailReports] | `ConcretePrintRequests.Rpt1894SalesRegisterSummaryByCustomerRequest` |
| 1895-SalesSummaryByCustomer&Item.rpt | `/reports/print/1895-sales-summary-by-customer-item` | [pcc].[USP_Sales_EvaulationDetailReports] | `ConcretePrintRequests.Rpt1895SalesSummaryByCustomerItemRequest` |
| 1896-SalesRegisterSummaryByCustomer&City.rpt | `/reports/print/1896-sales-register-summary-by-customer-city` | [pcc].[USP_Sales_EvaulationDetailReports] | `ConcretePrintRequests.Rpt1896SalesRegisterSummaryByCustomerCityRequest` |
| 1897-SalesSummaryByCustomer&Varient.rpt | `/reports/print/1897-sales-summary-by-customer-varient` | [pcc].[USP_Sales_EvaulationDetailReports] | `ConcretePrintRequests.Rpt1897SalesSummaryByCustomerVarientRequest` |
| 1898-SalesSummaryByCustomerItem&City.rpt | `/reports/print/1898-sales-summary-by-customer-item-city` | [pcc].[USP_Sales_EvaulationDetailReports] | `ConcretePrintRequests.Rpt1898SalesSummaryByCustomerItemCityRequest` |
| 1899-SalesSummaryByCity.rpt | `/reports/print/1899-sales-summary-by-city` | [pcc].[USP_Sales_EvaulationDetailReports] | `ConcretePrintRequests.Rpt1899SalesSummaryByCityRequest` |
| 347-OrderRegister.rpt | `/reports/print/347-order-register` | [pcc].[USP_SaleOrderSummaryRegister] | `ConcretePrintRequests.Rpt347OrderRegisterRequest` |
| 348-OrderSummaryByItem&PackSize.rpt | `/reports/print/348-order-summary-by-item-pack-size` | [pcc].[USP_SaleOrderSummaryRegister] | `ConcretePrintRequests.Rpt348OrderSummaryByItemPackSizeRequest` |
| 349-OrderSummaryByItem,PackSize&City.rpt | `/reports/print/349-order-summary-by-item-pack-size-city` | [pcc].[USP_SaleOrderSummaryRegister] | `ConcretePrintRequests.Rpt349OrderSummaryByItemPackSizeCityRequest` |
| 350-OrderSummaryByCustomer&PackSize.rpt | `/reports/print/350-order-summary-by-customer-pack-size` | [pcc].[USP_SaleOrderSummaryRegister] | `ConcretePrintRequests.Rpt350OrderSummaryByCustomerPackSizeRequest` |
| 351-OrderSummaryByItem.rpt | `/reports/print/351-order-summary-by-item` | [pcc].[USP_SaleOrderSummaryRegister] | `ConcretePrintRequests.Rpt351OrderSummaryByItemRequest` |
| 352-OrderSummaryByItem&City.rpt | `/reports/print/352-order-summary-by-item-city` | [pcc].[USP_SaleOrderSummaryRegister] | `ConcretePrintRequests.Rpt352OrderSummaryByItemCityRequest` |
| 353-OrderSummaryByCustomer.rpt | `/reports/print/353-order-summary-by-customer` | [pcc].[USP_SaleOrderSummaryRegister] | `ConcretePrintRequests.Rpt353OrderSummaryByCustomerRequest` |
| 354-OrderSummaryByCustomer&City.rpt | `/reports/print/354-order-summary-by-customer-city` | [pcc].[USP_SaleOrderSummaryRegister] | `ConcretePrintRequests.Rpt354OrderSummaryByCustomerCityRequest` |
| 355-OrderSummaryByCustomer&Item.rpt | `/reports/print/355-order-summary-by-customer-item` | [pcc].[USP_SaleOrderSummaryRegister] | `ConcretePrintRequests.Rpt355OrderSummaryByCustomerItemRequest` |
| 356-OrderSummaryByCustomer,Item&City.rpt | `/reports/print/356-order-summary-by-customer-item-city` | [pcc].[USP_SaleOrderSummaryRegister] | `ConcretePrintRequests.Rpt356OrderSummaryByCustomerItemCityRequest` |
| 357-OrderSummaryByCustomerandReferenceParty.rpt | `/reports/print/357-order-summary-by-customerand-reference-party` | [pcc].[USP_SaleOrderSummaryRegister] | `ConcretePrintRequests.Rpt357OrderSummaryByCustomerandReferencePartyRequest` |
| 358-OrderSummaryByCustomer&Item&ReferenceParty.rpt | `/reports/print/358-order-summary-by-customer-item-reference-party` | [pcc].[USP_SaleOrderSummaryRegister] | `ConcretePrintRequests.Rpt358OrderSummaryByCustomerItemReferencePartyRequest` |
| 359-OrderSummaryByReferenceParty.rpt | `/reports/print/359-order-summary-by-reference-party` | [pcc].[USP_SaleOrderSummaryRegister] | `ConcretePrintRequests.Rpt359OrderSummaryByReferencePartyRequest` |
| 360-OrderSummaryByReferenceParty&City.rpt | `/reports/print/360-order-summary-by-reference-party-city` | [pcc].[USP_SaleOrderSummaryRegister] | `ConcretePrintRequests.Rpt360OrderSummaryByReferencePartyCityRequest` |
| 361-OrderSummaryByReferenceParty&Item.rpt | `/reports/print/361-order-summary-by-reference-party-item` | [pcc].[USP_SaleOrderSummaryRegister] | `ConcretePrintRequests.Rpt361OrderSummaryByReferencePartyItemRequest` |
| BillOfMaterialAllDetails_SubReport.rpt | `/reports/print/bill-of-material-all-details-sub-report` | [pcc].[USP_BillOfMaterialAllDetails_SubReport] | `ConcretePrintRequests.RptBillOfMaterialAllDetailsSubReportRequest` |
| BillOfMaterialDetails_SubReport.rpt | `/reports/print/bill-of-material-details-sub-report` | [pcc].[USP_WorkOrder_BomDetail_Report] | `ConcretePrintRequests.RptBillOfMaterialDetailsSubReportRequest` |
| ConversionAllDetails_SubReport.rpt | `/reports/print/conversion-all-details-sub-report` | [pcc].[USP_ConversionAllDetails_SubReport] | `ConcretePrintRequests.RptConversionAllDetailsSubReportRequest` |
| InvGdn_SubReport.rpt | `/reports/print/inv-gdn-sub-report` | [pcc].[USP_InvGdnWagesDetail_SubReport] | `ConcretePrintRequests.RptInvGdnSubReportRequest` |
| InvPurchaseInvoice_SupplierBillExpense_SubReport.rpt | `/reports/print/inv-purchase-invoice-supplier-bill-expense-sub-report` | [pcc].[USP_InvPurchaseInvoice_SupplierBillExpense_SubReport] | `ConcretePrintRequests.RptInvPurchaseInvoiceSupplierBillExpenseSubReportRequest` |
| ProductionAllDetails_SubReport.rpt | `/reports/print/production-all-details-sub-report` | [pcc].[USP_ProductionAllDetails_SubReport] | `ConcretePrintRequests.RptProductionAllDetailsSubReportRequest` |
| SaleInvoice_ItemExpenseSubReport.rpt | `/reports/print/sale-invoice-item-expense-sub-report` | [pcc].[USP_InvSaleInvoiceItemExpense_SubReport] | `ConcretePrintRequests.RptSaleInvoiceItemExpenseSubReportRequest` |
| SaleInvoice_SubReport.rpt | `/reports/print/sale-invoice-sub-report` | pcc.USP_InvSaleInvoice_SubReport | `ConcretePrintRequests.RptSaleInvoiceSubReportRequest` |
| WorkOrder_InputSubReport.rpt | `/reports/print/work-order-input-sub-report` | [pcc].[USP_WorkOrder_InputSubReport] | `ConcretePrintRequests.RptWorkOrderInputSubReportRequest` |

## ContractorWages

| Template | GET / POST endpoint | Procedure | Request |
|---|---|---|---|
| 002_01_ContractorWagesPartyProcessingSlip.rpt | `/reports/print/002-01-contractor-wages-party-processing-slip` | Sp_InvContractorWagesBillHeader_SlipandRegister | `ContractorWagesPrintRequests.Rpt00201ContractorWagesPartyProcessingSlipRequest` |
| 003-tSummaryWagesByRefDocumentsAndActivities.rpt | `/reports/print/003-t-summary-wages-by-ref-documents-and-activities` | USP_GetSummaryWagesByRefDocumentsAndActivities | `ContractorWagesPrintRequests.Rpt003TSummaryWagesByRefDocumentsAndActivitiesRequest` |
| 003_A_WagesReportByContractor.rpt | `/reports/print/003-a-wages-report-by-contractor` | USP_GetSummaryWagesByRefDocumentsAndActivities | `ContractorWagesPrintRequests.Rpt003AWagesReportByContractorRequest` |
| InvContractorWagesSubReport.rpt | `/reports/print/inv-contractor-wages-sub-report` | Sp_InvContractorWagesBillHeader_SlipandRegister | `ContractorWagesPrintRequests.RptInvContractorWagesSubReportRequest` |

## Dashboard

| Template | GET / POST endpoint | Procedure | Request |
|---|---|---|---|
| 121_02_ReceivablesAndReceiptSchedule.rpt | `/reports/print/121-02-receivables-and-receipt-schedule` | usp_ReceivablesAndReceiptsSchedule | `DashboardPrintRequests.Rpt12102ReceivablesAndReceiptScheduleRequest` |
| 842_ItemAndPMItemMapSlip.rpt | `/reports/print/842-item-and-pm-item-map-slip` | [MRP].[USP_ItemAndPMItemMap_SlipAndRegister] | `DashboardPrintRequests.Rpt842ItemAndPMItemMapSlipRequest` |
| 121_03_PurchaseAnalyticsItemWise.rpt | `/reports/print/121-03-purchase-analytics-item-wise` | [dbo].[USP-PurchaseAnalyticsDashBoard_Report] | `DashboardPrintRequests.Rpt12103PurchaseAnalyticsItemWiseRequest` |
| 121_04_PurchaseAnalyticsSupplierWise.rpt | `/reports/print/121-04-purchase-analytics-supplier-wise` | [dbo].[USP-PurchaseAnalyticsDashBoard_Report] | `DashboardPrintRequests.Rpt12104PurchaseAnalyticsSupplierWiseRequest` |
| AuditActiviyReport.rpt | `/reports/print/audit-activiy-report` | [dbo].[USP_ReportsMethod_GetAllMethod] | `DashboardPrintRequests.RptAuditActiviyReportRequest` |

## Export

| Template | GET / POST endpoint | Procedure | Request |
|---|---|---|---|
| 529A_ExportInvoiceSlipPackingList.rpt | `/reports/print/529a-export-invoice-slip-packing-list` | usp_ExImInvoicePackingList_Rpt | `ExportPrintRequests.Rpt529AExportInvoiceSlipPackingListRequest` |
| 529B_ExportInvoiceSlipPackingList.rpt | `/reports/print/529b-export-invoice-slip-packing-list` | usp_ExImInvoicePackingList_Rpt | `ExportPrintRequests.Rpt529BExportInvoiceSlipPackingListRequest` |
| 529C_ExportInvoiceSlipPackingList.rpt | `/reports/print/529c-export-invoice-slip-packing-list` | usp_ExImInvoicePackingList_Rpt | `ExportPrintRequests.Rpt529CExportInvoiceSlipPackingListRequest` |
| 02_GDBreakUpHeader_Slip.rpt | `/reports/print/02-gd-break-up-header-slip` | [dbo].[USP_GDBreakUpHeader_Slip] | `ExportPrintRequests.Rpt02GDBreakUpHeaderSlipRequest` |
| 102-ANewAcRptExportInvoiceVoucherSlip.rpt | `/reports/print/102-a-new-ac-export-invoice-voucher-slip` | SpVouchers_ExportInvoiceVoucherSlipNew_Rpt | `ExportPrintRequests.Rpt102ANewAcExportInvoiceVoucherSlipRequest` |
| 102-ExportReturnInvoiceVoucherSlip.rpt | `/reports/print/102-export-return-invoice-voucher-slip` | SpVouchers_ExportReturnInvoiceVoucherSlipNew_Rpt | `ExportPrintRequests.Rpt102ExportReturnInvoiceVoucherSlipRequest` |
| 104-AcRptGeneralJournalAcAndInventoryDetailSlip.rpt | `/reports/print/104-general-journal-ac-and-inventory-detail-slip` | Sp_Vouchers_GeneralJournalAcAndInventoryDetailSlip_Rpt | `ExportPrintRequests.Rpt104GeneralJournalAcAndInventoryDetailSlipRequest` |
| 174-ExImForwardingDirect_SlipAndRegister.rpt | `/reports/print/174-ex-im-forwarding-direct-slip-and-register` | Sp_ExImForwardingDirect_SlipAndregisterRpt | `ExportPrintRequests.Rpt174ExImForwardingDirectSlipAndRegisterRequest` |
| 175-ForwardingByReferenceNoRegister.rpt | `/reports/print/175-forwarding-by-reference-no-register` | USP_FarwardingByReferenceNoRegister | `ExportPrintRequests.Rpt175ForwardingByReferenceNoRegisterRequest` |
| 216-ExportReturnInvoiceSlip.rpt | `/reports/print/216-export-return-invoice-slip` | [dbo].[USp_ExportReturnInvoice_SlipAndRegister] | `ExportPrintRequests.Rpt216ExportReturnInvoiceSlipRequest` |
| 244-GatePassInspectionSlip.rpt | `/reports/print/244-gate-pass-inspection-slip` | USP_ExImVCITransaction_SlipAndRegister | `ExportPrintRequests.Rpt244GatePassInspectionSlipRequest` |
| 263-InvDeliveryOrderForApproval.rpt | `/reports/print/263-inv-delivery-order-for-approval` | Proc_ExImExportShipingLineBooking_SlipAndRegister_Rpt | `ExportPrintRequests.Rpt263InvDeliveryOrderForApprovalRequest` |
| 283-FcyBankChargesRegister.rpt | `/reports/print/283-fcy-bank-charges-register` | [dbo].[usp_FcyBankCharges_Register] | `ExportPrintRequests.Rpt283FcyBankChargesRegisterRequest` |
| 288-FcyReceiptsSummaryRegister.rpt | `/reports/print/288-fcy-receipts-summary-register` | USP_FcyReceiptsSummaryRegister | `ExportPrintRequests.Rpt288FcyReceiptsSummaryRegisterRequest` |
| 289-BankGdsSummaryRegister.rpt | `/reports/print/289-bank-gds-summary-register` | USP_BankGdsSummary | `ExportPrintRequests.Rpt289BankGdsSummaryRegisterRequest` |
| 300_01_FISummaryRegister.rpt | `/reports/print/300-01-fi-summary-register` | usp_getFIBalanceSummary | `ExportPrintRequests.Rpt30001FISummaryRegisterRequest` |
| 300_02-FIAdvanceBalanceSummary.rpt | `/reports/print/300-02-fi-advance-balance-summary` | usp_FinancialInstrumentAdvanceBalanceSummary | `ExportPrintRequests.Rpt30002FIAdvanceBalanceSummaryRequest` |
| 319-ExportSaleInvoiceSlip.rpt | `/reports/print/319-export-sale-invoice-slip` | Sp_Vouchers_GetMethods | `ExportPrintRequests.Rpt319ExportSaleInvoiceSlipRequest` |
| 319A-ExportSaleInvoiceSlip.rpt | `/reports/print/319a-export-sale-invoice-slip` | Sp_InvSaleInvoice_GetAllMethod | `ExportPrintRequests.Rpt319AExportSaleInvoiceSlipRequest` |
| 359-GetExportSales.rpt | `/reports/print/359-get-export-sales` | USP_GetExportSales | `ExportPrintRequests.Rpt359GetExportSalesRequest` |
| 395-LcOrderShipmentScheduleLcOrderWise_Slip.rpt | `/reports/print/395-lc-order-shipment-schedule-lc-order-wise-slip` | SpExImInvoice_ExportInvoiceRegisterByLcContract_Rpt | `ExportPrintRequests.Rpt395LcOrderShipmentScheduleLcOrderWiseSlipRequest` |
| 396-ExportDeliveryOrderSlip.rpt | `/reports/print/396-export-delivery-order-slip` | Sp_InvDeliveryOrder_Slip | `ExportPrintRequests.Rpt396ExportDeliveryOrderSlipRequest` |
| 466-ExportGoodsReceiptsAtPortRegister.rpt | `/reports/print/466-export-goods-receipts-at-port-register` | Sp_ExImGoodsReceiptsAtPort_GetAllMetohd | `ExportPrintRequests.Rpt466ExportGoodsReceiptsAtPortRegisterRequest` |
| 501-ExportSalesContractExportNew.rpt | `/reports/print/501-export-sales-contract-export-new` | Sp_ExImLcOrderNo_ExportSlip_Rpt | `ExportPrintRequests.Rpt501ExportSalesContractExportNewRequest` |
| 501-ExpRptSalesContractExport.rpt | `/reports/print/501-sales-contract-export` | Sp_ExImLcOrderNo_ExportSlip_Rpt | `ExportPrintRequests.Rpt501SalesContractExportRequest` |
| 501A_ExportSalesContractExportNew.rpt | `/reports/print/501a-export-sales-contract-export-new` | Sp_ExImLcOrderNo_ExportSlip_Rpt | `ExportPrintRequests.Rpt501AExportSalesContractExportNewRequest` |
| 501B-ExpRptSalesContractExport.rpt | `/reports/print/501b-sales-contract-export` | Sp_ExportContractByInvoice_Rpt | `ExportPrintRequests.Rpt501BSalesContractExportRequest` |
| 501B_ExportSalesContractExportNew.rpt | `/reports/print/501b-export-sales-contract-export-new` | Sp_ExImLcOrderNo_ExportSlip_Rpt | `ExportPrintRequests.Rpt501BExportSalesContractExportNewRequest` |
| 505-ExImBillOfLading_Slip.rpt | `/reports/print/505-ex-im-bill-of-lading-slip` | Sp_ExImBillOfLading_SlipAndRegister_Rpt | `ExportPrintRequests.Rpt505ExImBillOfLadingSlipRequest` |
| 506-ExImBillOfLading_register.rpt | `/reports/print/506-ex-im-bill-of-lading-register` | Sp_ExImBillOfLading_SlipAndRegister_Rpt | `ExportPrintRequests.Rpt506ExImBillOfLadingRegisterRequest` |
| 507-ExImForwarding_Slip.rpt | `/reports/print/507-ex-im-forwarding-slip` | Sp_ExImForwarding_Rpt | `ExportPrintRequests.Rpt507ExImForwardingSlipRequest` |
| 507_01_ExImForwardingDirect_Slip.rpt | `/reports/print/507-01-ex-im-forwarding-direct-slip` | Sp_ExImForwarding_Rpt | `ExportPrintRequests.Rpt50701ExImForwardingDirectSlipRequest` |
| 508-ExImForwarding_Register.rpt | `/reports/print/508-ex-im-forwarding-register` | Proc_ExImExportShipingLineBooking_SlipAndRegister_Rpt | `ExportPrintRequests.Rpt508ExImForwardingRegisterRequest` |
| 508_01_Forwarding_Customised_Register.rpt | `/reports/print/508-01-forwarding-customised-register` | Proc_ExImExportShipingLineBooking_SlipAndRegister_Rpt | `ExportPrintRequests.Rpt50801ForwardingCustomisedRegisterRequest` |
| 511-ExImShippedConsignmentFollowUps_Slip.rpt | `/reports/print/511-ex-im-shipped-consignment-follow-ups-slip` | Sp_ExImBillOfLading_SlipAndRegister_Rpt | `ExportPrintRequests.Rpt511ExImShippedConsignmentFollowUpsSlipRequest` |
| 512-ExImShippedConsignmentFollowUps_Register.rpt | `/reports/print/512-ex-im-shipped-consignment-follow-ups-register` | Sp_ExImBillOfLading_SlipAndRegister_Rpt | `ExportPrintRequests.Rpt512ExImShippedConsignmentFollowUpsRegisterRequest` |
| 513-invLabPreProductionSlip.rpt | `/reports/print/513-inv-lab-pre-production-slip` | Sp_InvLabPreProductionExportLotInspectionHeader_SlipandRegister | `ExportPrintRequests.Rpt513InvLabPreProductionSlipRequest` |
| 514_01_InventoryStockReservedSlip.rpt | `/reports/print/514-01-inventory-stock-reserved-slip` | [dbo].[USP_InventoryStockReserved_SlipAndRegister] | `ExportPrintRequests.Rpt51401InventoryStockReservedSlipRequest` |
| 514_01_ThirdPartyInspectionLotTrackingReport.rpt | `/reports/print/514-01-third-party-inspection-lot-tracking-report` | [dbo].[usp_ThirdPartyInspectionData_ForApproval] | `ExportPrintRequests.Rpt51401ThirdPartyInspectionLotTrackingReportRequest` |
| 514_ThirdPartyInspectionSlip.rpt | `/reports/print/514-third-party-inspection-slip` | [dbo].[usp_InvLabPreThirdPartyInspection_Slip] | `ExportPrintRequests.Rpt514ThirdPartyInspectionSlipRequest` |
| 516-ExImRptFCBankReceipts.rpt | `/reports/print/516-fc-bank-receipts` | Sp_Vouchers_PaymentReceiptVoucherSlip_Rpt | `ExportPrintRequests.Rpt516FCBankReceiptsRequest` |
| 518-ExpRptSalesContractExportRegister.rpt | `/reports/print/518-sales-contract-export-register` | Sp_ExImLcOrderNo_ExportSlip_Rpt | `ExportPrintRequests.Rpt518SalesContractExportRegisterRequest` |
| 521-ExportInvoiceSlip.rpt | `/reports/print/521-export-invoice-slip` | Sp_ExImInvoice_SlipAndRegister_Rpt | `ExportPrintRequests.Rpt521ExportInvoiceSlipRequest` |
| 522-ExImInvoiceRegister.rpt | `/reports/print/522-ex-im-invoice-register` | Sp_ExImInvoice_ReceivedAndOutstandingHistory_Rpt | `ExportPrintRequests.Rpt522ExImInvoiceRegisterRequest` |
| 523-ExpRptSalesContractWiseInvoiceRegister.rpt | `/reports/print/523-sales-contract-wise-invoice-register` | SpExImInvoice_ExportInvoiceRegisterByLcContract_Rpt | `ExportPrintRequests.Rpt523SalesContractWiseInvoiceRegisterRequest` |
| 526-SaleContractRegister.rpt | `/reports/print/526-sale-contract-register` | Sp_ExImLcOrder_ExportRegister_Rpt | `ExportPrintRequests.Rpt526SaleContractRegisterRequest` |
| 527-ExImLcOrderShipmentScheduleDetailRegister.rpt | `/reports/print/527-ex-im-lc-order-shipment-schedule-detail-register` | Sp_ExImLcOrderNo_ExportSlip_Rpt | `ExportPrintRequests.Rpt527ExImLcOrderShipmentScheduleDetailRegisterRequest` |
| 527-ExImLcOrderShipmentScheduleDetailRegisterCustomerWise.rpt | `/reports/print/527-ex-im-lc-order-shipment-schedule-detail-register-customer-wise` | SpExImInvoice_ExportInvoiceRegisterByLcContract_Rpt | `ExportPrintRequests.Rpt527ExImLcOrderShipmentScheduleDetailRegisterCustomerWiseRequest` |
| 527-ExportDeliveryOrderByInvoiceIdSlip.rpt | `/reports/print/527-export-delivery-order-by-invoice-id-slip` | Sp_InvDeliveryOrder_ExportSlipByInvoice | `ExportPrintRequests.Rpt527ExportDeliveryOrderByInvoiceIdSlipRequest` |
| 529-ExportInvoiceSlipPackingList.rpt | `/reports/print/529-export-invoice-slip-packing-list` | Sp_ExImInvoice_SlipAndRegister_Rpt | `ExportPrintRequests.Rpt529ExportInvoiceSlipPackingListRequest` |
| 529-SubRpt-ContainerListByInvoice.rpt | `/reports/print/529-sub-container-list-by-invoice` | SpExImInvoiceGetContainersList | `ExportPrintRequests.Rpt529SubContainerListByInvoiceRequest` |
| 530-EximBillOfLadingSlip.rpt | `/reports/print/530-exim-bill-of-lading-slip` | Sp_ExImInvoice_SlipAndRegister_Rpt | `ExportPrintRequests.Rpt530EximBillOfLadingSlipRequest` |
| 531-ExImInvoiceSummeryByMonth.rpt | `/reports/print/531-ex-im-invoice-summery-by-month` | SpExImInvoiceSummaryByMonthly_Report | `ExportPrintRequests.Rpt531ExImInvoiceSummeryByMonthRequest` |
| 537-ExportDetailHistoryReport.rpt | `/reports/print/537-export-detail-history-report` | SpExImInvoice_ExportHistoryDetail_Report | `ExportPrintRequests.Rpt537ExportDetailHistoryReportRequest` |
| 538-ExportSummariesReport.rpt | `/reports/print/538-export-summaries-report` | SpExImInvoice_ExportsSummery_Reports | `ExportPrintRequests.Rpt538ExportSummariesReportRequest` |
| 538-ExportSummaryByCustomer.rpt | `/reports/print/538-export-summary-by-customer` | SpExImInvoice_ExportsSummery_Reports | `ExportPrintRequests.Rpt538ExportSummaryByCustomerRequest` |
| 538_01-ExportSummaryByItem.rpt | `/reports/print/538-01-export-summary-by-item` | SpExImInvoice_ExportsSummery_Reports | `ExportPrintRequests.Rpt53801ExportSummaryByItemRequest` |
| 538_02-ExportSummaryByPort.rpt | `/reports/print/538-02-export-summary-by-port` | SpExImInvoice_ExportsSummery_Reports | `ExportPrintRequests.Rpt53802ExportSummaryByPortRequest` |
| 540-RptServiceBillSlip.rpt | `/reports/print/540-service-bill-slip` | Sp_ExImClearingAgentBill_Rpt | `ExportPrintRequests.Rpt540ServiceBillSlipRequest` |
| 541-ServiceBillRegister.rpt | `/reports/print/541-service-bill-register` | Sp_ExImClearingAgentBill_Rpt | `ExportPrintRequests.Rpt541ServiceBillRegisterRequest` |
| 542-ShipmentCostingRegister.rpt | `/reports/print/542-shipment-costing-register` | SpExport_ShipmentCosting_Report | `ExportPrintRequests.Rpt542ShipmentCostingRegisterRequest` |
| 542_01-ShipmentCostingSummary.rpt | `/reports/print/542-01-shipment-costing-summary` | USP_ExportShipmentCosting_SummaryReport | `ExportPrintRequests.Rpt54201ShipmentCostingSummaryRequest` |
| 543-USP_ExportDetailByContract_ReportWithContainers.rpt | `/reports/print/543-usp-export-detail-by-contract-report-with-containers` | USP_ExportDetailByContract_Report | `ExportPrintRequests.Rpt543USPExportDetailByContractReportWithContainersRequest` |
| 544-USP_ExportDetailByContract_ReportWithoutContainers.rpt | `/reports/print/544-usp-export-detail-by-contract-report-without-containers` | USP_ExportDetailByContract_Report | `ExportPrintRequests.Rpt544USPExportDetailByContractReportWithoutContainersRequest` |
| 546-ExportPreInvoiceSlip.rpt | `/reports/print/546-export-pre-invoice-slip` | [dbo].[USP_PreInvoice_SlipAndRegister_Rpt] | `ExportPrintRequests.Rpt546ExportPreInvoiceSlipRequest` |
| 546-ExportPreInvoiceSlipForBank.rpt | `/reports/print/546-export-pre-invoice-slip-for-bank` | [dbo].[USP_PreInvoice_SlipAndRegister_Rpt] | `ExportPrintRequests.Rpt546ExportPreInvoiceSlipForBankRequest` |
| 548-ExportCommercialInvoiceSlipForBank.rpt | `/reports/print/548-export-commercial-invoice-slip-for-bank` | [dbo].[USP_PreInvoice_SlipAndRegister_Rpt] | `ExportPrintRequests.Rpt548ExportCommercialInvoiceSlipForBankRequest` |
| 548A-ExportCommercialInvoiceSlipForBank.rpt | `/reports/print/548a-export-commercial-invoice-slip-for-bank` | [dbo].[USP_PreInvoice_SlipAndRegister_Rpt] | `ExportPrintRequests.Rpt548AExportCommercialInvoiceSlipForBankRequest` |
| 549-ExportCommercialInvoiceSlip.rpt | `/reports/print/549-export-commercial-invoice-slip` | [dbo].[USP_PreInvoice_SlipAndRegister_Rpt] | `ExportPrintRequests.Rpt549ExportCommercialInvoiceSlipRequest` |
| 549A-ExportCommercialInvoiceSlip.rpt | `/reports/print/549a-export-commercial-invoice-slip` | [dbo].[USP_PreInvoice_SlipAndRegister_Rpt] | `ExportPrintRequests.Rpt549AExportCommercialInvoiceSlipRequest` |
| 550-CommercialInvoicePackingDetailList_Slip.rpt | `/reports/print/550-commercial-invoice-packing-detail-list-slip` | [dbo].[USP_CommercialInvoicePackingDetailList_Slip] | `ExportPrintRequests.Rpt550CommercialInvoicePackingDetailListSlipRequest` |
| 551-ContractSchedule_FormHistory.rpt | `/reports/print/551-contract-schedule-form-history` | USP_ContractSchedule_FormHistory | `ExportPrintRequests.Rpt551ContractScheduleFormHistoryRequest` |
| 551_01_ContractSchedule_StatusReport.rpt | `/reports/print/551-01-contract-schedule-status-report` | USP_ContractSchedule_StatusReport | `ExportPrintRequests.Rpt55101ContractScheduleStatusReportRequest` |
| 551_02_ExportContractScheduleLoadingDateItemWise_Slip.rpt | `/reports/print/551-02-export-contract-schedule-loading-date-item-wise-slip` | Usp_ExportContractSchedulePeriodicB | `ExportPrintRequests.Rpt55102ExportContractScheduleLoadingDateItemWiseSlipRequest` |
| 551_03_ExportContractScheduleLoadingDateCustomerItemWise_Slip.rpt | `/reports/print/551-03-export-contract-schedule-loading-date-customer-item-wise-slip` | Usp_ExportContractSchedulePeriodicB | `ExportPrintRequests.Rpt55103ExportContractScheduleLoadingDateCustomerItemWiseSlipRequest` |
| 555-PendingWorkExportRegister.rpt | `/reports/print/555-pending-work-export-register` | [dbo].[USP_PendingWorkExportRegister] | `ExportPrintRequests.Rpt555PendingWorkExportRegisterRequest` |
| 556-ExportLoadSheet.rpt | `/reports/print/556-export-load-sheet` | [dbo].[USP_ExportLoadSheet] | `ExportPrintRequests.Rpt556ExportLoadSheetRequest` |
| 557-ExImLcOrderNo_ExportSlip.rpt | `/reports/print/557-ex-im-lc-order-no-export-slip` | Sp_ExImLcOrderNo_ExportSlip_Rpt | `ExportPrintRequests.Rpt557ExImLcOrderNoExportSlipRequest` |
| 558-ExportInvoiceAgainstForwarding_Register.rpt | `/reports/print/558-export-invoice-against-forwarding-register` | USP_ExportInvoiceAgainstForwarding_Register | `ExportPrintRequests.Rpt558ExportInvoiceAgainstForwardingRegisterRequest` |
| 559-GDBreakUpandRealized_Register.rpt | `/reports/print/559-gd-break-upand-realized-register` | USP_GDBreakUpandRealized_Register | `ExportPrintRequests.Rpt559GDBreakUpandRealizedRegisterRequest` |
| 560-ExBooking Info(CRO).rpt | `/reports/print/560-ex-booking-info-cro` | Proc_ExImExportShipingLineBooking_SlipAndRegister_Rpt | `ExportPrintRequests.Rpt560ExBookingInfoCRORequest` |
| 560-GDBreakUpBankRequest_Slip.rpt | `/reports/print/560-gd-break-up-bank-request-slip` | USP_GDBreakUpBankRequest_SlipAndRegister | `ExportPrintRequests.Rpt560GDBreakUpBankRequestSlipRequest` |
| 561-SaleContractRegisterItemWise.rpt | `/reports/print/561-sale-contract-register-item-wise` | Sp_ExImLcOrder_ExportRegisterItemWise_Rpt | `ExportPrintRequests.Rpt561SaleContractRegisterItemWiseRequest` |
| 563-PackingListRegister_Export.rpt | `/reports/print/563-packing-list-register-export` | USP_PackingListRegister_Export | `ExportPrintRequests.Rpt563PackingListRegisterExportRequest` |
| 567-EEReport_ExportGD.rpt | `/reports/print/567-ee-report-export-gd` | USP_EEReport_ExportGD | `ExportPrintRequests.Rpt567EEReportExportGDRequest` |
| 567_01-EEReport_ExportGD.rpt | `/reports/print/567-01-ee-report-export-gd` | USP_EEReport_ExportGD | `ExportPrintRequests.Rpt56701EEReportExportGDRequest` |
| 567_02-EEReport_ExportGD.rpt | `/reports/print/567-02-ee-report-export-gd` | USP_EEReport_ExportGD | `ExportPrintRequests.Rpt56702EEReportExportGDRequest` |
| 612-InvRptProductionDetailWithExpValues.rpt | `/reports/print/612-production-detail-with-exp-values` | [dbo].[usp_ProductionOutputAllocationWithExportInvoice_SlipAndRegister] | `ExportPrintRequests.Rpt612ProductionDetailWithExpValuesRequest` |
| 618-DoWeightWbWeightDiff.rpt | `/reports/print/618-do-weight-wb-weight-diff` | SpEximInvoice_DoWeightWbWeightDiff_Rpt | `ExportPrintRequests.Rpt618DoWeightWbWeightDiffRequest` |
| 901_02_VoucherInvoicesAdjustment_Slip.rpt | `/reports/print/901-02-voucher-invoices-adjustment-slip` | [dbo].[USP_VoucherInvoicesAdjustment_ReadByVoucherHeadId] | `ExportPrintRequests.Rpt90102VoucherInvoicesAdjustmentSlipRequest` |
| 903-ImExShipmentBookingInfo.rpt | `/reports/print/903-im-ex-shipment-booking-info` | [Imex].[USP_Get_ShipmentBookingReport] | `ExportPrintRequests.Rpt903ImExShipmentBookingInfoRequest` |
| ExImLcContractPackingMaterialDetail_SubRpt.rpt | `/reports/print/ex-im-lc-contract-packing-material-detail-sub` | USP_ExImLcContractPackingMaterialDetail_SubRpt | `ExportPrintRequests.RptExImLcContractPackingMaterialDetailSubRequest` |
| ExImLcOrderPaymentTermsDetail_SubRpt.rpt | `/reports/print/ex-im-lc-order-payment-terms-detail-sub` | USP_ExImLcOrderPaymentTermsDetail_SubRpt | `ExportPrintRequests.RptExImLcOrderPaymentTermsDetailSubRequest` |
| ExportInvoiceCustomExpense_SubRpt.rpt | `/reports/print/export-invoice-custom-expense-sub` | [ExportInvoiceCustomExpense_SubRpt] | `ExportPrintRequests.RptExportInvoiceCustomExpenseSubRequest` |
| ExportInvoiceOtherExpense_SubRpt.rpt | `/reports/print/export-invoice-other-expense-sub` | [ExportInvoiceOtherExpense_SubRpt] | `ExportPrintRequests.RptExportInvoiceOtherExpenseSubRequest` |
| LcorderOtherItem_SubRpt.rpt | `/reports/print/lcorder-other-item-sub` | USP_LcorderOtherItem_SubRpt | `ExportPrintRequests.RptLcorderOtherItemSubRequest` |
| LcOrderShipmentScheduleLcOrderWise_SubReport1.rpt | `/reports/print/lc-order-shipment-schedule-lc-order-wise-sub-report-1` | [dbo].[USP_LcOrderShipmentScheduleLcOrderWise_Slip] | `ExportPrintRequests.RptLcOrderShipmentScheduleLcOrderWiseSubReport1Request` |
| LcOrderShipmentScheduleLcOrderWiseDetail_SubReport.rpt | `/reports/print/lc-order-shipment-schedule-lc-order-wise-detail-sub-report` | [dbo].[USP_LcOrderShipmentScheduleLcOrderWiseDetail_SubReport] | `ExportPrintRequests.RptLcOrderShipmentScheduleLcOrderWiseDetailSubReportRequest` |
| PreCommercialInvoiceExpenseSubReport.rpt | `/reports/print/pre-commercial-invoice-expense-sub-report` | [ExportInvoiceOtherExpense_SubRpt] | `ExportPrintRequests.RptPreCommercialInvoiceExpenseSubReportRequest` |

## FeedMill

| Template | GET / POST endpoint | Procedure | Request |
|---|---|---|---|
| 1100_PurchaseOrderSlip.rpt | `/reports/print/1100-purchase-order-slip` | [fed].[USP_PurchaseOrder_SlipAndRegister] | `FeedMillPrintRequests.Rpt1100PurchaseOrderSlipRequest` |
| 1110-SaleOrderSlip.rpt | `/reports/print/1110-sale-order-slip` | [fed].[usp_SaleOrderSlip] | `FeedMillPrintRequests.Rpt1110SaleOrderSlipRequest` |
| 1110_CustomerDiscountPolicySlip.rpt | `/reports/print/1110-customer-discount-policy-slip` | [fed].[USP_CustomerDiscountPolicySlip] | `FeedMillPrintRequests.Rpt1110CustomerDiscountPolicySlipRequest` |
| 1115-InvRepSaleBillCustomer.rpt | `/reports/print/1115-inv-rep-sale-bill-customer` | fed.usp_InvSaleInvoiceDirectSlip | `FeedMillPrintRequests.Rpt1115InvRepSaleBillCustomerRequest` |
| 1863-InvRptPurchaseOrderSlip.rpt | `/reports/print/1863-purchase-order-slip` | [fed].[USP_PurchaseOrder_SlipAndRegister] | `FeedMillPrintRequests.Rpt1863PurchaseOrderSlipRequest` |
| 481-ItemPricingScheduleListGroupWise.rpt | `/reports/print/481-item-pricing-schedule-list-group-wise` | fed.usp_GetLastItemRateByItemIdRateUomAndGroupId | `FeedMillPrintRequests.Rpt481ItemPricingScheduleListGroupWiseRequest` |
| InvPurchaseInvoice_ItemExpense_SubReport.rpt | `/reports/print/inv-purchase-invoice-item-expense-sub-report` | [fed].[USP_InvPurchaseInvoice_ItemExpense_SubReport] | `FeedMillPrintRequests.RptInvPurchaseInvoiceItemExpenseSubReportRequest` |
| PurchaseInvoice_SupplierExpense_SubReport.rpt | `/reports/print/purchase-invoice-supplier-expense-sub-report` | [fed].[USP_InvPurchaseInvoice_SupplierExpense_SubReport] | `FeedMillPrintRequests.RptPurchaseInvoiceSupplierExpenseSubReportRequest` |
| SaleOrderPaymentTermsDetail_SubReport.rpt | `/reports/print/sale-order-payment-terms-detail-sub-report` | [fed].[usp_SaleOrderPaymentTermsDetail_SubReport] | `FeedMillPrintRequests.RptSaleOrderPaymentTermsDetailSubReportRequest` |

## Hrm

| Template | GET / POST endpoint | Procedure | Request |
|---|---|---|---|
| 1003-DailyAttendance.rpt | `/reports/print/1003-daily-attendance` | Sp_genDailyAttandance_rpt | `HrmPrintRequests.Rpt1003DailyAttendanceRequest` |
| 1112-Employee_Registration_Slip.rpt | `/reports/print/1112-employee-registration-slip` | Sp_genEmployee_SlipandRegister | `HrmPrintRequests.Rpt1112EmployeeRegistrationSlipRequest` |
| 1008-EmployeeAdvanceSlip.rpt | `/reports/print/1008-employee-advance-slip` | dbo.USP_EmployeeAdvanceSlip | `HrmPrintRequests.Rpt1008EmployeeAdvanceSlipRequest` |
| 1010-OverTime_Slip.rpt | `/reports/print/1010-over-time-slip` | [hrm].[USP_OverTimeRequest_SlipAndRegister] | `HrmPrintRequests.Rpt1010OverTimeSlipRequest` |
| 1113-DailyLateandEarlyDeparture.rpt | `/reports/print/1113-daily-lateand-early-departure` | Sp_hrmDailyLED_rpt | `HrmPrintRequests.Rpt1113DailyLateandEarlyDepartureRequest` |
| 1114-DutyRosterEmployeeWise.rpt | `/reports/print/1114-duty-roster-employee-wise` | Sp_genDutyRosterEmployeeWise_rpt | `HrmPrintRequests.Rpt1114DutyRosterEmployeeWiseRequest` |
| 1004-EmployeeAttendance.rpt | `/reports/print/1004-employee-attendance` | Sp_genEmployeeAttendence_rpt | `HrmPrintRequests.Rpt1004EmployeeAttendanceRequest` |
| 1005-MonthlyAttendanceRegister_New.rpt | `/reports/print/1005-monthly-attendance-register-new` | Sp_MonthlyAttendanceRegister_Rpt | `HrmPrintRequests.Rpt1005MonthlyAttendanceRegisterNewRequest` |
| 1001-EmployeeAttendenceMonthly.rpt | `/reports/print/1001-employee-attendence-monthly` | Sp_genEmployeeMonthlyAttendence_rpt | `HrmPrintRequests.Rpt1001EmployeeAttendenceMonthlyRequest` |
| 1002-AttendanceRegister.rpt | `/reports/print/1002-attendance-register` | Sp_AttendanceRegister_Rpt | `HrmPrintRequests.Rpt1002AttendanceRegisterRequest` |
| 1101-EmployeeListDepartmentWiseActiveInactive.rpt | `/reports/print/1101-employee-list-department-wise-active-inactive` | Sp_genEmployeeHistory_Rpt | `HrmPrintRequests.Rpt1101EmployeeListDepartmentWiseActiveInactiveRequest` |
| 1101_01-EmployeeListDepartmentWiseActiveInactive.rpt | `/reports/print/1101-01-employee-list-department-wise-active-inactive` | Sp_genEmployeeHistory_Rpt | `HrmPrintRequests.Rpt110101EmployeeListDepartmentWiseActiveInactiveRequest` |
| 1011-EmployeeOverTimeSlip.rpt | `/reports/print/1011-employee-over-time-slip` | [hrm].[USP_EmployeeOverTime_SlipAndRegister] | `HrmPrintRequests.Rpt1011EmployeeOverTimeSlipRequest` |
| 1003_01-DailyAttendance.rpt | `/reports/print/1003-01-daily-attendance` | Sp_genDailyAttandance_rpt | `HrmPrintRequests.Rpt100301DailyAttendanceRequest` |
| 1006-AttendanceSummery.rpt | `/reports/print/1006-attendance-summery` | Sp_genEmployeeAttendanceSummery_rpt | `HrmPrintRequests.Rpt1006AttendanceSummeryRequest` |
| 1007-DailyShiftStrengthAttendenceDpt.rpt | `/reports/print/1007-daily-shift-strength-attendence-dpt` | Sp_genDSStrengthAttendence_rpt | `HrmPrintRequests.Rpt1007DailyShiftStrengthAttendenceDptRequest` |
| 1009-EmployeeOverTimeRegister.rpt | `/reports/print/1009-employee-over-time-register` | [dbo].[usp_ActualOverTimeLoaderForRequest] | `HrmPrintRequests.Rpt1009EmployeeOverTimeRegisterRequest` |
| 1100-EmployeeSalarySheet.rpt | `/reports/print/1100-employee-salary-sheet` | Sp_GetEmployeePostedSalary | `HrmPrintRequests.Rpt1100EmployeeSalarySheetRequest` |
| 1110-EmployeeSalarySlip.rpt | `/reports/print/1110-employee-salary-slip` | Sp_EmployeeSalarySlip_Rpt | `HrmPrintRequests.Rpt1110EmployeeSalarySlipRequest` |
| 1110B_EmployeeSalarySlip.rpt | `/reports/print/1110b-employee-salary-slip` | Sp_EmployeeSalarySlip_Rpt | `HrmPrintRequests.Rpt1110BEmployeeSalarySlipRequest` |
| 1111-Employee_Registration_Register.rpt | `/reports/print/1111-employee-registration-register` | Sp_genEmployee_SlipandRegister | `HrmPrintRequests.Rpt1111EmployeeRegistrationRegisterRequest` |
| 1115-EmployeeSalarySheet.rpt | `/reports/print/1115-employee-salary-sheet` | Sp_GetEmployeePostedSalary | `HrmPrintRequests.Rpt1115EmployeeSalarySheetRequest` |
| 1116_PayrollPostingForMultiApprovalReport.rpt | `/reports/print/1116-payroll-posting-for-multi-approval-report` | [dbo].[USP_PayrollPostingForMultiApproval] | `HrmPrintRequests.Rpt1116PayrollPostingForMultiApprovalReportRequest` |
| EmployeeAttendenceMonthly.rpt | `/reports/print/employee-attendence-monthly` | Sp_genEmployeeMonthlyAttendence_rpt | `HrmPrintRequests.RptEmployeeAttendenceMonthlyRequest` |

## Import

| Template | GET / POST endpoint | Procedure | Request |
|---|---|---|---|
| 231-ImLcOrder-Slip.rpt | `/reports/print/231-im-lc-order-slip` | [dbo].[USP_ImLcOrder_Register] | `ImportPrintRequests.Rpt231ImLcOrderSlipRequest` |
| 803-ImGRnSlip.rpt | `/reports/print/803-im-g-rn-slip` | Sp_ImGRN_SlipAndRegister | `ImportPrintRequests.Rpt803ImGRnSlipRequest` |
| 804-PurchaseOrderSlip.rpt | `/reports/print/804-purchase-order-slip` | Sp_ExImLcOrderPurchaseOrder_Slip | `ImportPrintRequests.Rpt804PurchaseOrderSlipRequest` |
| 806-ImInvoiceSlip.rpt | `/reports/print/806-im-invoice-slip` | Sp_ImInvoice_SlipandRegister | `ImportPrintRequests.Rpt806ImInvoiceSlipRequest` |
| 807-ImInvoiceRegister_WithAvgRates.rpt | `/reports/print/807-im-invoice-register-with-avg-rates` | SP_ImInvoiceRegister_WithAvgRates | `ImportPrintRequests.Rpt807ImInvoiceRegisterWithAvgRatesRequest` |
| 808-ImportContractRegister.rpt | `/reports/print/808-import-contract-register` | Sp_ImLcOrderNo_ImportSlip_Rpt | `ImportPrintRequests.Rpt808ImportContractRegisterRequest` |
| 809-ImportPackingListRegister.rpt | `/reports/print/809-import-packing-list-register` | Sp_ExImLcOrderPurchaseOrder_Slip | `ImportPrintRequests.Rpt809ImportPackingListRegisterRequest` |
| 810-ImportPurchaseOrder.rpt | `/reports/print/810-import-purchase-order` | Sp_ImportPurchaseOrder_Slip_rpt | `ImportPrintRequests.Rpt810ImportPurchaseOrderRequest` |
| 811-ImportGrnRegister.rpt | `/reports/print/811-import-grn-register` | Sp_ImGRN_SlipAndRegister | `ImportPrintRequests.Rpt811ImportGrnRegisterRequest` |
| 900_ProformaInvoiceSlip.rpt | `/reports/print/900-proforma-invoice-slip` | [ImEx].[usp_Get_ProformaMaster_Report] | `ImportPrintRequests.Rpt900ProformaInvoiceSlipRequest` |
| 902_ImportInvoiceSlip.rpt | `/reports/print/902-import-invoice-slip` | [ImEx].[usp_Get_invoiceMasterReport] | `ImportPrintRequests.Rpt902ImportInvoiceSlipRequest` |
| ImLcOrderPaymentDetail_SubReport.rpt | `/reports/print/im-lc-order-payment-detail-sub-report` | [dbo].[USP_ImLcOrderPaymentDetail_SubReport] | `ImportPrintRequests.RptImLcOrderPaymentDetailSubReportRequest` |
| ImportInvoice_SubReport.rpt | `/reports/print/import-invoice-sub-report` | [ImEx].[usp_Get_Invoice_SubReport] | `ImportPrintRequests.RptImportInvoiceSubReportRequest` |

## Inventory

| Template | GET / POST endpoint | Procedure | Request |
|---|---|---|---|
| 326-GetItemsFromMinAndMaxRateSchedule.rpt | `/reports/print/326-get-items-from-min-and-max-rate-schedule` | usp_GetItemsFromMinAndMaxRateSchedule | `InventoryPrintRequests.Rpt326GetItemsFromMinAndMaxRateScheduleRequest` |
| 326_01_ItemsFromMinAndMaxRateSchedule_FormHistory.rpt | `/reports/print/326-01-items-from-min-and-max-rate-schedule-form-history` | usp_GetLastItemMinAndMaxRateByItemIdAndUomId | `InventoryPrintRequests.Rpt32601ItemsFromMinAndMaxRateScheduleFormHistoryRequest` |
| 417-InventoryStockTransactionsReport.rpt | `/reports/print/417-inventory-stock-transactions-report` | [pcc].[USP-EvaluationStockTransactionsReport] | `InventoryPrintRequests.Rpt417InventoryStockTransactionsReportRequest` |
| 200-InvRptItemsList.rpt | `/reports/print/200-items-list` | SP_Item_List_Rpt | `InventoryPrintRequests.Rpt200ItemsListRequest` |
| 255-GatePassGeneralRpt.rpt | `/reports/print/255-gate-pass-general` | Sp_GatePassGeneral_Register | `InventoryPrintRequests.Rpt255GatePassGeneralRequest` |
| 274_1-PreBookinRegister.rpt | `/reports/print/274-1-pre-bookin-register` | USP_PreBookingOrderSlipAndRegister | `InventoryPrintRequests.Rpt2741PreBookinRegisterRequest` |
| 292-InvRptSupplierRegister.rpt | `/reports/print/292-supplier-register` | Sp_SupplierCustomerHistory_rpt | `InventoryPrintRequests.Rpt292SupplierRegisterRequest` |
| 292_01-InvRptSupplierRegister.rpt | `/reports/print/292-01-supplier-register` | Sp_SupplierCustomerHistory_rpt | `InventoryPrintRequests.Rpt29201SupplierRegisterRequest` |
| 293-InvRptSupplierSlip.rpt | `/reports/print/293-supplier-slip` | Sp_SupplierCustomerHistory_rpt | `InventoryPrintRequests.Rpt293SupplierSlipRequest` |
| BarCodeReport.rpt | `/reports/print/bar-code-report` | Sp_Item_GetAllMethod | `InventoryPrintRequests.RptBarCodeReportRequest` |

## Lab

| Template | GET / POST endpoint | Procedure | Request |
|---|---|---|---|
| 256-GatePassInward_WithDetailSlip.rpt | `/reports/print/256-gate-pass-inward-with-detail-slip` | Sp_GatePassInward_SlipAndRegister_WithDetail_Rpt | `LabPrintRequests.Rpt256GatePassInwardWithDetailSlipRequest` |
| 291_01_GatePassVehicleEntryAndExitTimeAnalysisReport.rpt | `/reports/print/291-01-gate-pass-vehicle-entry-and-exit-time-analysis-report` | USP_GatePass_VehicleEntryAndExitTime_AnalysisReport | `LabPrintRequests.Rpt29101GatePassVehicleEntryAndExitTimeAnalysisReportRequest` |
| 657-RptInvLabSampleAnalysisSlipA.rpt | `/reports/print/657-inv-lab-sample-analysis-slip-a` | Sp_InvLabSampleAnalysisHeader_RiceSlipAndRegister_Rpt | `LabPrintRequests.Rpt657InvLabSampleAnalysisSlipARequest` |
| 658-RptInvLabSaleAnalysisSlip.rpt | `/reports/print/658-inv-lab-sale-analysis-slip` | SP_InvLabAnalysisSale_Slip_Rpt | `LabPrintRequests.Rpt658InvLabSaleAnalysisSlipRequest` |
| 659-RptInvLabInProcessAnalysisSlip.rpt | `/reports/print/659-inv-lab-in-process-analysis-slip` | USp_InvLabAnalysisInProcessHeader_Slip | `LabPrintRequests.Rpt659InvLabInProcessAnalysisSlipRequest` |
| 660-LabSInProcessAnalysisRegister.rpt | `/reports/print/660-lab-s-in-process-analysis-register` | USP_InProcessAnalysisRegister | `LabPrintRequests.Rpt660LabSInProcessAnalysisRegisterRequest` |
| 661-LabPurchaseAnalysisRegitser.rpt | `/reports/print/661-lab-purchase-analysis-regitser` | USP_LabPurchaseAnalysis_Register | `LabPrintRequests.Rpt661LabPurchaseAnalysisRegitserRequest` |
| 662-LabSaleAnalysisRegitser.rpt | `/reports/print/662-lab-sale-analysis-regitser` | [USP_LabSaleAnalysis_Register] | `LabPrintRequests.Rpt662LabSaleAnalysisRegitserRequest` |
| 664-LabSampleAnalysisRegister.rpt | `/reports/print/664-lab-sample-analysis-register` | [USP_LabSampleAnalysis_Register] | `LabPrintRequests.Rpt664LabSampleAnalysisRegisterRequest` |
| 665-LabDataVehicleWiseByParentIdRegister.rpt | `/reports/print/665-lab-data-vehicle-wise-by-parent-id-register` | usp_getLabDataVehicleWiseByParentId | `LabPrintRequests.Rpt665LabDataVehicleWiseByParentIdRegisterRequest` |
| InvLabInProcessGroupAnalysisSub.rpt | `/reports/print/inv-lab-in-process-group-analysis-sub` | USp_InvLabAnalysisGroup_SubReport | `LabPrintRequests.RptInvLabInProcessGroupAnalysisSubRequest` |
| InvLabInProcessStepAnalysisSub.rpt | `/reports/print/inv-lab-in-process-step-analysis-sub` | USp_InvLabAnalysisStep_SubReport | `LabPrintRequests.RptInvLabInProcessStepAnalysisSubRequest` |
| InvLabPurchaseAnalysisSubForGP.rpt | `/reports/print/inv-lab-purchase-analysis-sub-for-gp` | Sp_InvLabAnalysisPurchaseSlip_Rpt | `LabPrintRequests.RptInvLabPurchaseAnalysisSubForGPRequest` |
| LabAnalysisPurchaseByGPSubReport.rpt | `/reports/print/lab-analysis-purchase-by-gp-sub-report` | USP_InvLabAnalysisPurchaseByGP_SubReport | `LabPrintRequests.RptLabAnalysisPurchaseByGPSubReportRequest` |
| LabDataVehicleWiseSummary_Report.rpt | `/reports/print/lab-data-vehicle-wise-summary-report` | usp_getLabDataVehicleWiseByParentId | `LabPrintRequests.RptLabDataVehicleWiseSummaryReportRequest` |
| LabPurchaseAnalysisSubParameterReport.rpt | `/reports/print/lab-purchase-analysis-sub-parameter-report` | Sp_InvLabAnalysisPurchaseHeader_GetAllMethod | `LabPrintRequests.RptLabPurchaseAnalysisSubParameterReportRequest` |
| LabSampleAnalysisSubParameterReport.rpt | `/reports/print/lab-sample-analysis-sub-parameter-report` | Sp_InvLabSampleAnalysisHeader_GetAllMethod | `LabPrintRequests.RptLabSampleAnalysisSubParameterReportRequest` |

## Logistics

| Template | GET / POST endpoint | Procedure | Request |
|---|---|---|---|
| 1300_LogisticRateNegotiationSlip_Slip.rpt | `/reports/print/1300-logistic-rate-negotiation-slip-slip` | [lgstcm].[USP_logisticRateNegotiationHeader_Slip] | `LogisticsPrintRequests.Rpt1300LogisticRateNegotiationSlipSlipRequest` |
| 1301_AgreementHeader_Slip.rpt | `/reports/print/1301-agreement-header-slip` | [lgstcm].[USP_AgreementHeader_Slip] | `LogisticsPrintRequests.Rpt1301AgreementHeaderSlipRequest` |
| 1302_LogisticRateNegotiationTransporter_Slip.rpt | `/reports/print/1302-logistic-rate-negotiation-transporter-slip` | [lgstcm].[USP_logisticRateNegotiationHeader_Slip] | `LogisticsPrintRequests.Rpt1302LogisticRateNegotiationTransporterSlipRequest` |
| 1303_PurchaseOrderHeader_Slip.rpt | `/reports/print/1303-purchase-order-header-slip` | [lgstcm].[USP_PurchaseOrderHeader_Slip] | `LogisticsPrintRequests.Rpt1303PurchaseOrderHeaderSlipRequest` |
| 1304_ServicesBillHeader_Slip.rpt | `/reports/print/1304-services-bill-header-slip` | [lgstcm].[USP_ServicesBillHeader_Slip] | `LogisticsPrintRequests.Rpt1304ServicesBillHeaderSlipRequest` |
| 1305_FreightVoucherOutward_Slip.rpt | `/reports/print/1305-freight-voucher-outward-slip` | [lgstcm].[USP_FreightVoucherOutward_Slip] | `LogisticsPrintRequests.Rpt1305FreightVoucherOutwardSlipRequest` |
| logisticRateNegotiation_SourceDocumentSubReport.rpt | `/reports/print/logistic-rate-negotiation-source-document-sub-report` | [lgstcm].[USP_logisticRateNegotiation_SourceDocumentSubReport] | `LogisticsPrintRequests.RptLogisticRateNegotiationSourceDocumentSubReportRequest` |
| logisticRateNegotiationTransporter_SourceDocumentSubReport.rpt | `/reports/print/logistic-rate-negotiation-transporter-source-document-sub-report` | [lgstcm].[USP_logisticRateNegotiation_SourceDocumentSubReport] | `LogisticsPrintRequests.RptLogisticRateNegotiationTransporterSourceDocumentSubReportRequest` |

## Manufacturing

| Template | GET / POST endpoint | Procedure | Request |
|---|---|---|---|
| 1614_WorkOrder_Slip.rpt | `/reports/print/1614-work-order-slip` | [Mfg].[USP_WorkOrder_SlipAndRegister] | `ManufacturingPrintRequests.Rpt1614WorkOrderSlipRequest` |
| 1619-Production_Slip.rpt | `/reports/print/1619-production-slip` | [Mfg].[USP_ProductionSlipAndRegister_Engr] | `ManufacturingPrintRequests.Rpt1619ProductionSlipRequest` |
| 1620_WorkOrderInProgress_Slip.rpt | `/reports/print/1620-work-order-in-progress-slip` | [Mfg].[USP_WorkOrderInProgress_SlipAndRegister] | `ManufacturingPrintRequests.Rpt1620WorkOrderInProgressSlipRequest` |
| 1621_WorkOrderSemiFinish_Slip.rpt | `/reports/print/1621-work-order-semi-finish-slip` | [Mfg].[USP_WorkOrderForSemiFinish_SlipAndRegister] | `ManufacturingPrintRequests.Rpt1621WorkOrderSemiFinishSlipRequest` |
| 1656_SaleOrderDetailRegister.rpt | `/reports/print/1656-sale-order-detail-register` | [Mfg].[USP_SaleOrderDetailRegister_Eng] | `ManufacturingPrintRequests.Rpt1656SaleOrderDetailRegisterRequest` |
| 1657_01_DeliveryOrderRegister_Eng.rpt | `/reports/print/1657-01-delivery-order-register-eng` | [Mfg].[USP_DeliveryOrderRegister_Eng] | `ManufacturingPrintRequests.Rpt165701DeliveryOrderRegisterEngRequest` |
| 1659_01_GdnRegister_Eng.rpt | `/reports/print/1659-01-gdn-register-eng` | [Mfg].[USP_GdnRegister_Eng] | `ManufacturingPrintRequests.Rpt165901GdnRegisterEngRequest` |
| 1659_GdnSlip_Engr.rpt | `/reports/print/1659-gdn-slip-engr` | [dbo].[USP_GdnRegister_Eng] | `ManufacturingPrintRequests.Rpt1659GdnSlipEngrRequest` |
| 1663_StockTransfer_Slip.rpt | `/reports/print/1663-stock-transfer-slip` | [Mfg].[USP_InvStockTransferSlipRegister] | `ManufacturingPrintRequests.Rpt1663StockTransferSlipRequest` |
| 1665_StockTransferForProductionRejection_Slip.rpt | `/reports/print/1665-stock-transfer-for-production-rejection-slip` | [Mfg].[USP_InvStockTransferSlipRegister] | `ManufacturingPrintRequests.Rpt1665StockTransferForProductionRejectionSlipRequest` |
| 1670_ProductionEngr_Slip.rpt | `/reports/print/1670-production-engr-slip` | [Mfg].[USP_ProductionHeader_Slip] | `ManufacturingPrintRequests.Rpt1670ProductionEngrSlipRequest` |
| 1671_ItemStockSummary_Engr.rpt | `/reports/print/1671-item-stock-summary-engr` | [mfg].[usp_ItemStockReportWithValues] | `ManufacturingPrintRequests.Rpt1671ItemStockSummaryEngrRequest` |
| 1672_01_ItemEvaluationLedger.rpt | `/reports/print/1672-01-item-evaluation-ledger` | [Mfg].[usp_ItemLedgerFromStockEvaluations] | `ManufacturingPrintRequests.Rpt167201ItemEvaluationLedgerRequest` |
| 1672_EvaluationTransactionReportWithValues_Engr.rpt | `/reports/print/1672-evaluation-transaction-report-with-values-engr` | [Mfg].[usp_EvaluationTransactionReportWithValues] | `ManufacturingPrintRequests.Rpt1672EvaluationTransactionReportWithValuesEngrRequest` |
| 207-RecipeSlip.rpt | `/reports/print/207-recipe-slip` | USp_Recipe_SlipandRegister | `ManufacturingPrintRequests.Rpt207RecipeSlipRequest` |
| 271A-InvRptSalesOrderRegister.rpt | `/reports/print/271a-sales-order-register` | [Mfg].[USP_SaleOrderDetailRegister_Eng] | `ManufacturingPrintRequests.Rpt271ASalesOrderRegisterRequest` |
| RecipeExpenseSubReport.rpt | `/reports/print/recipe-expense-sub-report` | USP_ReceipeExpense_SubRpt | `ManufacturingPrintRequests.RptRecipeExpenseSubReportRequest` |

## PackingMaterial

| Template | GET / POST endpoint | Procedure | Request |
|---|---|---|---|
| 214-GRNPackingMaterialSlip.rpt | `/reports/print/214-grn-packing-material-slip` | Sp_InvGrn_StoreSlip_Rpt | `PackingMaterialPrintRequests.Rpt214GRNPackingMaterialSlipRequest` |
| 214_01_GrnPackingMaterialSlip.rpt | `/reports/print/214-01-grn-packing-material-slip` | Sp_InvGrn_StoreSlip_Rpt | `PackingMaterialPrintRequests.Rpt21401GrnPackingMaterialSlipRequest` |
| 231-InvRptPurchaseBillPackingMaterialSlip.rpt | `/reports/print/231-purchase-bill-packing-material-slip` | SP_CommisionAgentBillOtherExpense_SubRpt | `PackingMaterialPrintRequests.Rpt231PurchaseBillPackingMaterialSlipRequest` |
| 231_01_PurchaseBillPmSlipWithGrnDetail.rpt | `/reports/print/231-01-purchase-bill-pm-slip-with-grn-detail` | USp_InvPurchaseInvoice_PackingMaterialBill_Rpt | `PackingMaterialPrintRequests.Rpt23101PurchaseBillPmSlipWithGrnDetailRequest` |
| 294-InvRptSaleBillDirectWithoutSO.rpt | `/reports/print/294-sale-bill-direct-without-so` | SP_InvSaleInvoice_CustomerBillRice OthersExp_SubRep | `PackingMaterialPrintRequests.Rpt294SaleBillDirectWithoutSORequest` |
| 215-ExportReturnGrnSlip.rpt | `/reports/print/215-export-return-grn-slip` | [dbo].[USp_ExportReturnGrn_SlipAndRegister] | `PackingMaterialPrintRequests.Rpt215ExportReturnGrnSlipRequest` |
| 341-GrnPackingMaterialRegister.rpt | `/reports/print/341-grn-packing-material-register` | Sp_InvGrn_StoreSlip_Rpt | `PackingMaterialPrintRequests.Rpt341GrnPackingMaterialRegisterRequest` |
| 402-InvStockRptInventoryTransactionsA.rpt | `/reports/print/402-inv-stock-inventory-transactions-a` | Sp_InventoryTransactions_GenerateTransactionsLedgerStocks | `PackingMaterialPrintRequests.Rpt402InvStockInventoryTransactionsARequest` |
| 357-PurchaseOrderRegister_PM.rpt | `/reports/print/357-purchase-order-register-pm` | Sp_PurchaseOrder_PackingMaterial_Rpt | `PackingMaterialPrintRequests.Rpt357PurchaseOrderRegisterPMRequest` |
| 357_01-PurchaseOrderRegister_PM.rpt | `/reports/print/357-01-purchase-order-register-pm` | Sp_PurchaseOrder_PackingMaterial_Rpt | `PackingMaterialPrintRequests.Rpt35701PurchaseOrderRegisterPMRequest` |
| 215-PurchaseOrderPackingMaterialSlip.rpt | `/reports/print/215-purchase-order-packing-material-slip` | Sp_PurchaseOrder_GeneralOrderSlip_Rpt | `PackingMaterialPrintRequests.Rpt215PurchaseOrderPackingMaterialSlipRequest` |
| 215_StockAdjustmentPMSlip.rpt | `/reports/print/215-stock-adjustment-pm-slip` | Sp_StockAdjustmentSlipAndRegister | `PackingMaterialPrintRequests.Rpt215StockAdjustmentPMSlipRequest` |
| 415-InvStockTransferPackingMaterialAndStore_SlipandRegister.rpt | `/reports/print/415-inv-stock-transfer-packing-material-and-store-slipand-register` | Sp_InvStockTransferPackingMaterialAndStore_SlipandRegister | `PackingMaterialPrintRequests.Rpt415InvStockTransferPackingMaterialAndStoreSlipandRegisterRequest` |
| 419-PartyToPartyPackingMaterialSlip.rpt | `/reports/print/419-party-to-party-packing-material-slip` | USP_PmStockWithPartiesTransfer_SlipAndRegister | `PackingMaterialPrintRequests.Rpt419PartyToPartyPackingMaterialSlipRequest` |
| 665-InvStockConversionPackingMaterial-Summery.rpt | `/reports/print/665-inv-stock-conversion-packing-material-summery` | USP-InvStockConversionPackingMaterial_Summery_Rpt | `PackingMaterialPrintRequests.Rpt665InvStockConversionPackingMaterialSummeryRequest` |
| 671_01_PackingMaterialRequirementPlanning_Detail.rpt | `/reports/print/671-01-packing-material-requirement-planning-detail` | usp_PackingMaterialRequirementPlanning | `PackingMaterialPrintRequests.Rpt67101PackingMaterialRequirementPlanningDetailRequest` |
| 671_PackingMaterialRequirementPlanning.rpt | `/reports/print/671-packing-material-requirement-planning` | usp_PackingMaterialRequirementPlanning | `PackingMaterialPrintRequests.Rpt671PackingMaterialRequirementPlanningRequest` |

## PartyProcessing

| Template | GET / POST endpoint | Procedure | Request |
|---|---|---|---|
| 120_01_JobOrderPartyProcessingRegister.rpt | `/reports/print/120-01-job-order-party-processing-register` | [dbo].[USP_ProductionJobOrderPartyProcessing_SlipandRegister] | `PartyProcessingPrintRequests.Rpt12001JobOrderPartyProcessingRegisterRequest` |
| 123_01_PartyProcessingBillRegister.rpt | `/reports/print/123-01-party-processing-bill-register` | [dbo].[USP_ProductionProcessingBill_Register] | `PartyProcessingPrintRequests.Rpt12301PartyProcessingBillRegisterRequest` |
| 158-InvStockAdjustmentPartyProcessingSlip.rpt | `/reports/print/158-inv-stock-adjustment-party-processing-slip` | Sp_StockAdjustmentPartyProcessingSlipAndRegister | `PartyProcessingPrintRequests.Rpt158InvStockAdjustmentPartyProcessingSlipRequest` |
| 158_01_PartyProcessingStockAdjustmentRegister.rpt | `/reports/print/158-01-party-processing-stock-adjustment-register` | Sp_StockAdjustmentPartyProcessingSlipAndRegister | `PartyProcessingPrintRequests.Rpt15801PartyProcessingStockAdjustmentRegisterRequest` |
| 169-InvPartyProcessingGrnInfo.rpt | `/reports/print/169-inv-party-processing-grn-info` | Sp_InvPartyProcessingGrn_Info | `PartyProcessingPrintRequests.Rpt169InvPartyProcessingGrnInfoRequest` |
| 176_01_StockConversionRegisterWithActivityPartyProcessing.rpt | `/reports/print/176-01-stock-conversion-register-with-activity-party-processing` | [dbo].[USP_StockConversionPartyProcessingRegisterWithActivity] | `PartyProcessingPrintRequests.Rpt17601StockConversionRegisterWithActivityPartyProcessingRequest` |
| 176_02_StockConversionRegisterWithActivityPartyProcessing.rpt | `/reports/print/176-02-stock-conversion-register-with-activity-party-processing` | [dbo].[USP_StockConversionPartyProcessingRegisterWithActivity] | `PartyProcessingPrintRequests.Rpt17602StockConversionRegisterWithActivityPartyProcessingRequest` |
| 217_01_GrnPurchaseFromPPRegister.rpt | `/reports/print/217-01-grn-purchase-from-pp-register` | USP_GrnPurchaseFromPartyProcessing_Register | `PartyProcessingPrintRequests.Rpt21701GrnPurchaseFromPPRegisterRequest` |
| 217_GrnPurchaseFromPPRegister.rpt | `/reports/print/217-grn-purchase-from-pp-register` | USP_GrnPurchaseFromPartyProcessing_Register | `PartyProcessingPrintRequests.Rpt217GrnPurchaseFromPPRegisterRequest` |
| 220_StockTransferPartyProcessingSlip.rpt | `/reports/print/220-stock-transfer-party-processing-slip` | Sp_InvStockTransferPartyProcessing_SlipandRegister | `PartyProcessingPrintRequests.Rpt220StockTransferPartyProcessingSlipRequest` |
| 221_01_GdnSaleToPartyProcessingRegister.rpt | `/reports/print/221-01-gdn-sale-to-party-processing-register` | [dbo].[USP_GdnSaleToPartyProcessing_Register] | `PartyProcessingPrintRequests.Rpt22101GdnSaleToPartyProcessingRegisterRequest` |
| 299-InvRptPartyProcessingInwardGatePassSlip.rpt | `/reports/print/299-party-processing-inward-gate-pass-slip` | [dbo].[Sp_GatePassPartyProcessing_SlipAndRegister_Rpt] | `PartyProcessingPrintRequests.Rpt299PartyProcessingInwardGatePassSlipRequest` |
| 299-InvRptPartyProcessingOutwardGatePassSlip.rpt | `/reports/print/299-party-processing-outward-gate-pass-slip` | [dbo].[Sp_GatePassPartyProcessing_SlipAndRegister_Rpt] | `PartyProcessingPrintRequests.Rpt299PartyProcessingOutwardGatePassSlipRequest` |
| 300-RptPartyProcessingGatePassRegister.rpt | `/reports/print/300-party-processing-gate-pass-register` | [dbo].[Sp_GatePassPartyProcessing_SlipAndRegister_Rpt] | `PartyProcessingPrintRequests.Rpt300PartyProcessingGatePassRegisterRequest` |
| 332-PartyProcessingGRNSlip.rpt | `/reports/print/332-party-processing-grn-slip` | Sp_InvGrnGdnStorePartyProcessing_SlipandRegister | `PartyProcessingPrintRequests.Rpt332PartyProcessingGRNSlipRequest` |
| 332_01-PartyProcessingGDNSlip.rpt | `/reports/print/332-01-party-processing-gdn-slip` | Sp_InvGrnGdnStorePartyProcessing_SlipandRegister | `PartyProcessingPrintRequests.Rpt33201PartyProcessingGDNSlipRequest` |
| 333-InvRptGoodsReceiptsNotesRiceSlip.rpt | `/reports/print/333-goods-receipts-notes-rice-slip` | [dbo].[Sp_InvGrnPartyProcessing_RiceSlip_Rpt] | `PartyProcessingPrintRequests.Rpt333GoodsReceiptsNotesRiceSlipRequest` |
| 339-InvRptGoodsReceiptsNotesRiceSlipGDN.rpt | `/reports/print/339-goods-receipts-notes-rice-slip-gdn` | [dbo].[Sp_InvGrnPartyProcessing_RiceSlip_Rpt] | `PartyProcessingPrintRequests.Rpt339GoodsReceiptsNotesRiceSlipGDNRequest` |
| 414-InvStockRptInventoryTransactions.rpt | `/reports/print/414-inv-stock-inventory-transactions` | [dbo].[USP_InventoryTransactions_PartyProcessing] | `PartyProcessingPrintRequests.Rpt414InvStockInventoryTransactionsRequest` |
| 416_01-InvStockOpeningBalancePartyProcessing_Slip.rpt | `/reports/print/416-01-inv-stock-opening-balance-party-processing-slip` | [dbo].[Sp_InvStockOpeningBalancePartyProcessing_SlipandRegister] | `PartyProcessingPrintRequests.Rpt41601InvStockOpeningBalancePartyProcessingSlipRequest` |
| 466_01-ItemStockSummaryPartyProcessing.rpt | `/reports/print/466-01-item-stock-summary-party-processing` | Sp_InventoryTransactionsPartyProcessing_StockSummaryGeneralByWeight_Rpt | `PartyProcessingPrintRequests.Rpt46601ItemStockSummaryPartyProcessingRequest` |
| 466_02-ItemandWarehouseStockSummaryPartyProcessing.rpt | `/reports/print/466-02-itemand-warehouse-stock-summary-party-processing` | Sp_InventoryTransactionsPartyProcessing_StockSummaryGeneralByWeight_Rpt | `PartyProcessingPrintRequests.Rpt46602ItemandWarehouseStockSummaryPartyProcessingRequest` |
| 466_03-ItemandCropYearStockSummaryPartyProcessing.rpt | `/reports/print/466-03-itemand-crop-year-stock-summary-party-processing` | Sp_InventoryTransactionsPartyProcessing_StockSummaryGeneralByWeight_Rpt | `PartyProcessingPrintRequests.Rpt46603ItemandCropYearStockSummaryPartyProcessingRequest` |
| 466_04-JobLotandItemStockSummaryPartyProcessing.rpt | `/reports/print/466-04-job-lotand-item-stock-summary-party-processing` | Sp_InventoryTransactionsPartyProcessing_StockSummaryGeneralByWeight_Rpt | `PartyProcessingPrintRequests.Rpt46604JobLotandItemStockSummaryPartyProcessingRequest` |
| 466_05-ItemandCropYearandWarehouseStockSummaryPartyProcessing.rpt | `/reports/print/466-05-itemand-crop-yearand-warehouse-stock-summary-party-processing` | Sp_InventoryTransactionsPartyProcessing_StockSummaryGeneralByWeight_Rpt | `PartyProcessingPrintRequests.Rpt46605ItemandCropYearandWarehouseStockSummaryPartyProcessingRequest` |
| 466_06-WarehouseandJoblotandItemStockSummaryStockSummaryPartyProcessing.rpt | `/reports/print/466-06-warehouseand-joblotand-item-stock-summary-stock-summary-party-processing` | Sp_InventoryTransactionsPartyProcessing_StockSummaryGeneralByWeight_Rpt | `PartyProcessingPrintRequests.Rpt46606WarehouseandJoblotandItemStockSummaryStockSummaryPartyProcessingRequest` |
| 600-StockConversionPartyProcessing_Summery_Rpt.rpt | `/reports/print/600-stock-conversion-party-processing-summery` | SpInvStockConversionPartyProcessing_Summery_Rpt | `PartyProcessingPrintRequests.Rpt600StockConversionPartyProcessingSummeryRequest` |
| 601_01-FoodProductionTransactionSlip.rpt | `/reports/print/601-01-food-production-transaction-slip` | [dbo].[Sp_InvFoodProductionPartyProcessing_Rpt] | `PartyProcessingPrintRequests.Rpt60101FoodProductionTransactionSlipRequest` |
| 601_02-InvFoodPackingMaterialSlip.rpt | `/reports/print/601-02-inv-food-packing-material-slip` | [dbo].[USP_FoodProductionPartyProcessingPackingMaterial_SlipAndRegister] | `PartyProcessingPrintRequests.Rpt60102InvFoodPackingMaterialSlipRequest` |
| 608-InvRptProductionSummeryPartyProcessing.rpt | `/reports/print/608-production-summery-party-processing` | Sp_InvFoodProductionPartyProcessing_Summery_Rpt | `PartyProcessingPrintRequests.Rpt608ProductionSummeryPartyProcessingRequest` |
| 609-FoodProductionIssuancePartyProcessingGrnWiseByJobOrderId.rpt | `/reports/print/609-food-production-issuance-party-processing-grn-wise-by-job-order-id` | Sp_InvFoodProductionPartyProcessingIssuanceGrnWiseByJobOrderId_rpt | `PartyProcessingPrintRequests.Rpt609FoodProductionIssuancePartyProcessingGrnWiseByJobOrderIdRequest` |
| 611-ProductionPartyProcessingBill.rpt | `/reports/print/611-production-party-processing-bill` | spInvProductionProcessingBill_PartyProcessingModule_Report | `PartyProcessingPrintRequests.Rpt611ProductionPartyProcessingBillRequest` |
| 626-ProductionJobOrderPartyProcessing.rpt | `/reports/print/626-production-job-order-party-processing` | [dbo].[USP_ProductionJobOrderPartyProcessing_SlipandRegister] | `PartyProcessingPrintRequests.Rpt626ProductionJobOrderPartyProcessingRequest` |
| 670_01_ProductionRegisterWithActivityPartyProcessing.rpt | `/reports/print/670-01-production-register-with-activity-party-processing` | [dbo].[USP_ProductionPartyProcessingRegisterWithActivity] | `PartyProcessingPrintRequests.Rpt67001ProductionRegisterWithActivityPartyProcessingRequest` |
| 670_02_ProductionRegisterWithActivityPartyProcessing.rpt | `/reports/print/670-02-production-register-with-activity-party-processing` | [dbo].[USP_ProductionPartyProcessingRegisterWithActivity] | `PartyProcessingPrintRequests.Rpt67002ProductionRegisterWithActivityPartyProcessingRequest` |
| 670_ProductionRegisterWithActivity_OutPutByPackingMaterialPartyProcessing.rpt | `/reports/print/670-production-register-with-activity-out-put-by-packing-material-party-processing` | [dbo].[USP_ProductionPartyProcessingRegisterWithActivity] | `PartyProcessingPrintRequests.Rpt670ProductionRegisterWithActivityOutPutByPackingMaterialPartyProcessingRequest` |
| PartyProcessingBillByProductSubReport.rpt | `/reports/print/party-processing-bill-by-product-sub-report` | Usp_ProductionProcessingBillOutPut_SubReport | `PartyProcessingPrintRequests.RptPartyProcessingBillByProductSubReportRequest` |
| PartyProcessingBillSubReport.rpt | `/reports/print/party-processing-bill-sub-report` | spInvProductionProcessingBill_PartyProcessingModule_SubReport | `PartyProcessingPrintRequests.RptPartyProcessingBillSubReportRequest` |

## Pos

| Template | GET / POST endpoint | Procedure | Request |
|---|---|---|---|
| 324-SalesItemPricingList.rpt | `/reports/print/324-sales-item-pricing-list` | USP_GetFinalItemSalePriceList | `PosPrintRequests.Rpt324SalesItemPricingListRequest` |
| 324A_SalesPricingList.rpt | `/reports/print/324a-sales-pricing-list` | Sp_ItemPricingSchedule_GetAllMethod | `PosPrintRequests.Rpt324ASalesPricingListRequest` |
| 325-ItemPricingList.rpt | `/reports/print/325-item-pricing-list` | USP_GetItemSchedulePricingSlip | `PosPrintRequests.Rpt325ItemPricingListRequest` |
| 325_01-ItemPricingList.rpt | `/reports/print/325-01-item-pricing-list` | USP_GetItemPricingList | `PosPrintRequests.Rpt32501ItemPricingListRequest` |

## Production

| Template | GET / POST endpoint | Procedure | Request |
|---|---|---|---|
| 666-DailyPlantConsumedHours_FormHistoryReport.rpt | `/reports/print/666-daily-plant-consumed-hours-form-history-report` | [dbo].[USP_DailyPlantConsumedHours_FormHistoryReport] | `ProductionPrintRequests.Rpt666DailyPlantConsumedHoursFormHistoryReportRequest` |
| 627-ProductionOutputAllocationWithExportInvoice.rpt | `/reports/print/627-production-output-allocation-with-export-invoice` | [dbo].[usp_ProductionOutputAllocationWithExportInvoice_SlipAndRegister] | `ProductionPrintRequests.Rpt627ProductionOutputAllocationWithExportInvoiceRequest` |
| 601-InvFoodProductionSlip.rpt | `/reports/print/601-inv-food-production-slip` | Sp_InvFoodProduction_Rpt | `ProductionPrintRequests.Rpt601InvFoodProductionSlipRequest` |
| 606-InvFoodProductionPackingAndOverHeadReportByJobOrder.rpt | `/reports/print/606-inv-food-production-packing-and-over-head-report-by-job-order` | Sp_InvFoodProductionPackingAndOverHeadReportByJobOrder | `ProductionPrintRequests.Rpt606InvFoodProductionPackingAndOverHeadReportByJobOrderRequest` |
| 602A-InvRptProductionSummeryWithValues.rpt | `/reports/print/602a-production-summery-with-values` | Sp_InvFoodProduction_Summery_Rpt | `ProductionPrintRequests.Rpt602AProductionSummeryWithValuesRequest` |
| 613-InvRptProductionSummeryWithExpValues.rpt | `/reports/print/613-production-summery-with-exp-values` | Sp_InvFoodProduction_Summery2_Rpt | `ProductionPrintRequests.Rpt613ProductionSummeryWithExpValuesRequest` |
| 615-InvRptProductionSummery.rpt | `/reports/print/615-production-summery` | Sp_InvFoodProduction_Summery3_Rpt | `ProductionPrintRequests.Rpt615ProductionSummeryRequest` |
| 623-InvFoodProduction_ConsumptionReport.rpt | `/reports/print/623-inv-food-production-consumption-report` | [dbo].[USP_InvFoodProduction_ConsumptionReport] | `ProductionPrintRequests.Rpt623InvFoodProductionConsumptionReportRequest` |
| 620-ProductionJobOrderSlip.rpt | `/reports/print/620-production-job-order-slip` | Sp_InvProductionJobOrder_Slip_Rpt | `ProductionPrintRequests.Rpt620ProductionJobOrderSlipRequest` |
| 605-StockConversionSummaryNewRpt.rpt | `/reports/print/605-stock-conversion-summary-new` | SpInvStockConversion_Summery_Rpt | `ProductionPrintRequests.Rpt605StockConversionSummaryNewRequest` |
| 812-ProductionKamPackMaterialConsumption.rpt | `/reports/print/812-production-kam-pack-material-consumption` | Sp_InvProductionCumPackMaterialConsumption_Register | `ProductionPrintRequests.Rpt812ProductionKamPackMaterialConsumptionRequest` |
| 604-FoodProductionComparisonRpt.rpt | `/reports/print/604-food-production-comparison` | SpInvFoodProductionComparisons_Rpt | `ProductionPrintRequests.Rpt604FoodProductionComparisonRequest` |
| 625_FoodProduction_DocWiseSummeryReport.rpt | `/reports/print/625-food-production-doc-wise-summery-report` | [dbo].[USP_FoodProduction_DocWiseSummeryReport] | `ProductionPrintRequests.Rpt625FoodProductionDocWiseSummeryReportRequest` |
| 164-WagesSummaryByContractorAndJobOrder.rpt | `/reports/print/164-wages-summary-by-contractor-and-job-order` | USp_WagesRegister | `ProductionPrintRequests.Rpt164WagesSummaryByContractorAndJobOrderRequest` |
| 461-BoilerConsumption_Slip.rpt | `/reports/print/461-boiler-consumption-slip` | Sp_BoilerConsumption_Slip&Register | `ProductionPrintRequests.Rpt461BoilerConsumptionSlipRequest` |
| 553-ProductionRegisterWithActivity.rpt | `/reports/print/553-production-register-with-activity` | USP_ProductionRegisterWithActivity | `ProductionPrintRequests.Rpt553ProductionRegisterWithActivityRequest` |
| 554-ProductionRegisterWithActivity(OutPut By Packing Material).rpt | `/reports/print/554-production-register-with-activity-out-put-by-packing-material` | USP_ProductionRegisterWithActivity | `ProductionPrintRequests.Rpt554ProductionRegisterWithActivityOutPutByPackingMaterialRequest` |
| 562-ProductionRegisterWithActivity.rpt | `/reports/print/562-production-register-with-activity` | USP_ProductionRegisterWithActivity | `ProductionPrintRequests.Rpt562ProductionRegisterWithActivityRequest` |
| 602-InvRptProductionSummery.rpt | `/reports/print/602-production-summery` | Sp_InvFoodProduction_Summery_Rpt | `ProductionPrintRequests.Rpt602ProductionSummeryRequest` |
| 602_01_ProductionSummeryWithLab.rpt | `/reports/print/602-01-production-summery-with-lab` | Sp_InvFoodProduction_Summery_WithOutValue_Rpt | `ProductionPrintRequests.Rpt60201ProductionSummeryWithLabRequest` |
| 603-ProductionPendingInvoice.rpt | `/reports/print/603-production-pending-invoice` | Sp_InvPurchaseInvoice_PendingInvoiceForProduction_rpt | `ProductionPrintRequests.Rpt603ProductionPendingInvoiceRequest` |
| 607-FoodProductionIssuanceGrnWiseByJobOrderId.rpt | `/reports/print/607-food-production-issuance-grn-wise-by-job-order-id` | Sp_InvFoodProductionIssuanceGrnWiseByJobOrderId_rpt | `ProductionPrintRequests.Rpt607FoodProductionIssuanceGrnWiseByJobOrderIdRequest` |
| 613_01-ProductionBeforSettlement_Summery2.rpt | `/reports/print/613-01-production-befor-settlement-summery-2` | usp_ProductionBeforSettlement_Summery2_Rpt | `ProductionPrintRequests.Rpt61301ProductionBeforSettlementSummery2Request` |
| 613_02_ProductionSettlement_WithReferenceDocumentDetailReport.rpt | `/reports/print/613-02-production-settlement-with-reference-document-detail-report` | [dbo].[USP_ProductionSettlement_WithReferenceDocumentDetailReport] | `ProductionPrintRequests.Rpt61302ProductionSettlementWithReferenceDocumentDetailReportRequest` |
| 615_01-ProductionBeforSettlement_Summery3.rpt | `/reports/print/615-01-production-befor-settlement-summery-3` | usp_ProductionBeforSettlement_Summery3_Rpt | `ProductionPrintRequests.Rpt61501ProductionBeforSettlementSummery3Request` |
| 663-InvRptPurchaseOrderDetailRegisterRice.rpt | `/reports/print/663-purchase-order-detail-register-rice` | USP_GetGrnsPendingOrUsedinProduction | `ProductionPrintRequests.Rpt663PurchaseOrderDetailRegisterRiceRequest` |
| 666_01_MonthlyPlantConsumedHours_Report.rpt | `/reports/print/666-01-monthly-plant-consumed-hours-report` | [dbo].[USP_DailyPlantConsumedHours_FormHistoryReport] | `ProductionPrintRequests.Rpt66601MonthlyPlantConsumedHoursReportRequest` |
| 672_JobOrderSummaryReport.rpt | `/reports/print/672-job-order-summary-report` | [dbo].[usp_JobOrderSummary_Report] | `ProductionPrintRequests.Rpt672JobOrderSummaryReportRequest` |
| 921_I_01_ProductionCostingFormHistoryReport.rpt | `/reports/print/921-i-01-production-costing-form-history-report` | [dbo].[usp_ProductionCosting_FormHistory] | `ProductionPrintRequests.Rpt921I01ProductionCostingFormHistoryReportRequest` |
| 922_ProdcutionFohAllocateToJobOrderSlip.rpt | `/reports/print/922-prodcution-foh-allocate-to-job-order-slip` | [dbo].[USP_ProdcutionFohAllocateToJobOrder_SlipAndRegister] | `ProductionPrintRequests.Rpt922ProdcutionFohAllocateToJobOrderSlipRequest` |
| ProductionJobOrder_SubReport.rpt | `/reports/print/production-job-order-sub-report` | [dbo].[USP_ProductionJobOrder_SubReport] | `ProductionPrintRequests.RptProductionJobOrderSubReportRequest` |
| ProductionManualCosting_ByProductDetailSubReport.rpt | `/reports/print/production-manual-costing-by-product-detail-sub-report` | [dbo].[USP_ProductionManualCosting_ByProductDetailSubReport] | `ProductionPrintRequests.RptProductionManualCostingByProductDetailSubReportRequest` |
| ProductionManualCosting_InputDetailSubRepirt.rpt | `/reports/print/production-manual-costing-input-detail-sub-repirt` | [dbo].[USP_ProductionManualCosting_InputDetailSubReport] | `ProductionPrintRequests.RptProductionManualCostingInputDetailSubRepirtRequest` |
| RptInvProductionJobOrderSlipA.rpt | `/reports/print/inv-production-job-order-slip-a` | Sp_InvProductionJobOrder_GetAllMethod | `ProductionPrintRequests.RptInvProductionJobOrderSlipARequest` |
| usp_InvProductionJobOrderLabStandardDetail_SubReport.rpt | `/reports/print/usp-inv-production-job-order-lab-standard-detail-sub-report` | usp_InvProductionJobOrderLabStandardDetail_SubReport | `ProductionPrintRequests.RptUspInvProductionJobOrderLabStandardDetailSubReportRequest` |

## Purchase

| Template | GET / POST endpoint | Procedure | Request |
|---|---|---|---|
| 211-InvRptGoodsReceiptsNotesRiceSlip.rpt | `/reports/print/211-goods-receipts-notes-rice-slip` | Sp_InvGrn_RiceSlip_Rpt | `PurchasePrintRequests.Rpt211GoodsReceiptsNotesRiceSlipRequest` |
| 251-InvRptInwardGatePassSlip.rpt | `/reports/print/251-inward-gate-pass-slip` | Sp_GatePassInward_SlipAndRegister_Rpt | `PurchasePrintRequests.Rpt251InwardGatePassSlipRequest` |
| 203-InvRptPurchaseOrderRiceSlip.rpt | `/reports/print/203-purchase-order-rice-slip` | Sp_PurchaseOrderSlip_Rpt | `PurchasePrintRequests.Rpt203PurchaseOrderRiceSlipRequest` |
| 203A_PurchaseOrderSlip.rpt | `/reports/print/203a-purchase-order-slip` | Sp_PurchaseOrderSlip_Rpt | `PurchasePrintRequests.Rpt203APurchaseOrderSlipRequest` |
| 203_01_PurchaseOrderRiceSlip.rpt | `/reports/print/203-01-purchase-order-rice-slip` | Sp_PurchaseOrderSlip_Rpt | `PurchasePrintRequests.Rpt20301PurchaseOrderRiceSlipRequest` |
| 203_02_PurchaseOrderRiceSlip.rpt | `/reports/print/203-02-purchase-order-rice-slip` | Sp_PurchaseOrderSlip_Rpt | `PurchasePrintRequests.Rpt20302PurchaseOrderRiceSlipRequest` |
| 257-InwardGatePassWithWbAndLabSlip.rpt | `/reports/print/257-inward-gate-pass-with-wb-and-lab-slip` | Sp_GatePassInward_SlipAndRegister_Rpt | `PurchasePrintRequests.Rpt257InwardGatePassWithWbAndLabSlipRequest` |
| 653-RptInvLabPurchaseAnalysisSlip.rpt | `/reports/print/653-inv-lab-purchase-analysis-slip` | Sp_InvLabAnalysisPurchaseSlip_Rpt | `PurchasePrintRequests.Rpt653InvLabPurchaseAnalysisSlipRequest` |
| 225-InvRepPurchaseBillDirectWithoutPo.rpt | `/reports/print/225-inv-rep-purchase-bill-direct-without-po` | sp_InvpurchaseInvoiceDirectSlip | `PurchasePrintRequests.Rpt225InvRepPurchaseBillDirectWithoutPoRequest` |
| 220R-InvRptPurchaseBillReturnSupplierRiceSlip.rpt | `/reports/print/220r-purchase-bill-return-supplier-rice-slip` | Sp_InvPurchaseInvoiceReturn_SupplierBill_Rpt | `PurchasePrintRequests.Rpt220RPurchaseBillReturnSupplierRiceSlipRequest` |
| 103-AcRptPurchaseSalesVoucherSlip.rpt | `/reports/print/103-purchase-sales-voucher-slip` | Sp_Vouchers_GeneralJournalAcAndInventoryDetailSlip_Rpt | `PurchasePrintRequests.Rpt103PurchaseSalesVoucherSlipRequest` |
| 230-InvRptPurchaseBillStoreSlip.rpt | `/reports/print/230-purchase-bill-store-slip` | Sp_InvPurchaseInvoice_StoreBill_Rpt | `PurchasePrintRequests.Rpt230PurchaseBillStoreSlipRequest` |
| 1858-GrnRegister.rpt | `/reports/print/1858-grn-register` | [pcc].[USP_InvGrn_SlipAndRegister] | `PurchasePrintRequests.Rpt1858GrnRegisterRequest` |
| 253-InvRptGatePassInwardRegisterA.rpt | `/reports/print/253-gate-pass-inward-register-a` | Sp_GatePassInward_History | `PurchasePrintRequests.Rpt253GatePassInwardRegisterARequest` |
| 1863-PurchaseOrderRegister.rpt | `/reports/print/1863-purchase-order-register` | [pcc].[USP_PurchaseOrder_SlipAndRegister] | `PurchasePrintRequests.Rpt1863PurchaseOrderRegisterRequest` |
| 0228-InvPurchaseInvoice_SaleReturnRegister.rpt | `/reports/print/0228-inv-purchase-invoice-sale-return-register` | Sp_InvPurchaseInvoice_SaleReturnRegister | `PurchasePrintRequests.Rpt0228InvPurchaseInvoiceSaleReturnRegisterRequest` |
| 1203_GdnReturnable_Slip.rpt | `/reports/print/1203-gdn-returnable-slip` | Sp_InvGdn_SlipAndRegisterRice_Rpt | `PurchasePrintRequests.Rpt1203GdnReturnableSlipRequest` |
| 1600-PurchaseOrder_Slip_Engr.rpt | `/reports/print/1600-purchase-order-slip-engr` | USP_PurchaseOrder_Slip_Engr | `PurchasePrintRequests.Rpt1600PurchaseOrderSlipEngrRequest` |
| 1601-InwardGatePassSlipEngr.rpt | `/reports/print/1601-inward-gate-pass-slip-engr` | Sp_GatePassInward_SlipAndRegister_Rpt | `PurchasePrintRequests.Rpt1601InwardGatePassSlipEngrRequest` |
| 1602-GoodsReceiptsNotesSlip.rpt | `/reports/print/1602-goods-receipts-notes-slip` | [dbo].[USp_InvGrnRegisterAndSlip_Engr] | `PurchasePrintRequests.Rpt1602GoodsReceiptsNotesSlipRequest` |
| 1602_01-GoodsReceiptsNotesSlip.rpt | `/reports/print/1602-01-goods-receipts-notes-slip` | [dbo].[USp_InvGrnRegisterAndSlip_Engr] | `PurchasePrintRequests.Rpt160201GoodsReceiptsNotesSlipRequest` |
| 1603-PurchaseBillSupplier_Engr_ItemSlip.rpt | `/reports/print/1603-purchase-bill-supplier-engr-item-slip` | USP_InvPurchaseInvoice_PartySlip_Engr | `PurchasePrintRequests.Rpt1603PurchaseBillSupplierEngrItemSlipRequest` |
| 1603A-PurchaseInvoice_Engr_PartySlip.rpt | `/reports/print/1603a-purchase-invoice-engr-party-slip` | USP_InvPurchaseInvoice_PartySlip_Engr | `PurchasePrintRequests.Rpt1603APurchaseInvoiceEngrPartySlipRequest` |
| 1604-PurchaseInvoiceDirect_Engr_ItemSlip.rpt | `/reports/print/1604-purchase-invoice-direct-engr-item-slip` | USP_InvPurchaseInvoiceDirect_PartySlip_Engr | `PurchasePrintRequests.Rpt1604PurchaseInvoiceDirectEngrItemSlipRequest` |
| 1604A-PurchaseInvoiceDirect_Engr_PartySlip.rpt | `/reports/print/1604a-purchase-invoice-direct-engr-party-slip` | USP_InvPurchaseInvoiceDirect_PartySlip_Engr | `PurchasePrintRequests.Rpt1604APurchaseInvoiceDirectEngrPartySlipRequest` |
| 1612-InvGdn_Slip.rpt | `/reports/print/1612-inv-gdn-slip` | Sp_InvGdn_SlipAndRegisterRice_Rpt | `PurchasePrintRequests.Rpt1612InvGdnSlipRequest` |
| 1618-GoodsReceiptsNotesSlip.rpt | `/reports/print/1618-goods-receipts-notes-slip` | [dbo].[USp_InvGrnRegisterAndSlip_Engr] | `PurchasePrintRequests.Rpt1618GoodsReceiptsNotesSlipRequest` |
| 1618-RptGrnRegister.rpt | `/reports/print/1618-grn-register` | [dbo].[USp_InvGrnRegisterAndSlip_Engr] | `PurchasePrintRequests.Rpt1618GrnRegisterRequest` |
| 1618A-RptGrnRegister.rpt | `/reports/print/1618a-grn-register` | [dbo].[USp_InvGrnRegisterAndSlip_Engr] | `PurchasePrintRequests.Rpt1618AGrnRegisterRequest` |
| 1626-PurchaseBySupplier.rpt | `/reports/print/1626-purchase-by-supplier` | [dbo].[USP_PurchaseInvoice_WithActivitiesRegister_Engr] | `PurchasePrintRequests.Rpt1626PurchaseBySupplierRequest` |
| 1627-PurchaseBySupplier&Item.rpt | `/reports/print/1627-purchase-by-supplier-item` | [dbo].[USP_PurchaseInvoice_WithActivitiesRegister_Engr] | `PurchasePrintRequests.Rpt1627PurchaseBySupplierItemRequest` |
| 1628-PurchaseByItem,Category&ItemType.rpt | `/reports/print/1628-purchase-by-item-category-item-type` | [dbo].[USP_PurchaseInvoice_WithActivitiesRegister_Engr] | `PurchasePrintRequests.Rpt1628PurchaseByItemCategoryItemTypeRequest` |
| 1629-PurchaseByCategory.rpt | `/reports/print/1629-purchase-by-category` | [dbo].[USP_PurchaseInvoice_WithActivitiesRegister_Engr] | `PurchasePrintRequests.Rpt1629PurchaseByCategoryRequest` |
| 1630-PurchaseByItemType.rpt | `/reports/print/1630-purchase-by-item-type` | [dbo].[USP_PurchaseInvoice_WithActivitiesRegister_Engr] | `PurchasePrintRequests.Rpt1630PurchaseByItemTypeRequest` |
| 1631-PurchaseByPaymentTerms.rpt | `/reports/print/1631-purchase-by-payment-terms` | [dbo].[USP_PurchaseInvoice_WithActivitiesRegister_Engr] | `PurchasePrintRequests.Rpt1631PurchaseByPaymentTermsRequest` |
| 1632-PurchaseByCategory&PackSize.rpt | `/reports/print/1632-purchase-by-category-pack-size` | [dbo].[USP_PurchaseInvoice_WithActivitiesRegister_Engr] | `PurchasePrintRequests.Rpt1632PurchaseByCategoryPackSizeRequest` |
| 1654_PurchaseInvoice_CustomerBillDirectSlip_Engr.rpt | `/reports/print/1654-purchase-invoice-customer-bill-direct-slip-engr` | USP_InvPurchaseInvoiceDirect_PartySlip_Engr | `PurchasePrintRequests.Rpt1654PurchaseInvoiceCustomerBillDirectSlipEngrRequest` |
| 1654A_PurchaseInvoice_CustomerBillDirectSlip_Engr.rpt | `/reports/print/1654a-purchase-invoice-customer-bill-direct-slip-engr` | USP_InvPurchaseInvoiceDirect_PartySlip_Engr | `PurchasePrintRequests.Rpt1654APurchaseInvoiceCustomerBillDirectSlipEngrRequest` |
| 1804-InvRepPurchaseBillDirectWithoutPo.rpt | `/reports/print/1804-inv-rep-purchase-bill-direct-without-po` | [dbo].[USP_PurchaseInvoiceDirectForSaltSlip] | `PurchasePrintRequests.Rpt1804InvRepPurchaseBillDirectWithoutPoRequest` |
| 1804_01_PurchaseRegister.rpt | `/reports/print/1804-01-purchase-register` | [dbo].[USP_PurchaseFromEvaulationsWithActivitiesReportSalt] | `PurchasePrintRequests.Rpt180401PurchaseRegisterRequest` |
| 1804_02_PurchaseSummaryByItem.rpt | `/reports/print/1804-02-purchase-summary-by-item` | [dbo].[USP_PurchaseFromEvaulationsWithActivitiesReportSalt] | `PurchasePrintRequests.Rpt180402PurchaseSummaryByItemRequest` |
| 1804_03_PurchaseSummaryByItemAndWarehouse.rpt | `/reports/print/1804-03-purchase-summary-by-item-and-warehouse` | [dbo].[USP_PurchaseFromEvaulationsWithActivitiesReportSalt] | `PurchasePrintRequests.Rpt180403PurchaseSummaryByItemAndWarehouseRequest` |
| 1804_04_PurchaseSummaryBySupplier.rpt | `/reports/print/1804-04-purchase-summary-by-supplier` | [dbo].[USP_PurchaseFromEvaulationsWithActivitiesReportSalt] | `PurchasePrintRequests.Rpt180404PurchaseSummaryBySupplierRequest` |
| 1804_05_PurchaseSummaryByItemAndSupplier.rpt | `/reports/print/1804-05-purchase-summary-by-item-and-supplier` | [dbo].[USP_PurchaseFromEvaulationsWithActivitiesReportSalt] | `PurchasePrintRequests.Rpt180405PurchaseSummaryByItemAndSupplierRequest` |
| 1804_06_PurchaseSummaryByParentCategory.rpt | `/reports/print/1804-06-purchase-summary-by-parent-category` | [dbo].[USP_PurchaseFromEvaulationsWithActivitiesReportSalt] | `PurchasePrintRequests.Rpt180406PurchaseSummaryByParentCategoryRequest` |
| 1804_07_PurchaseSummaryByParentCategoryAndSupplier.rpt | `/reports/print/1804-07-purchase-summary-by-parent-category-and-supplier` | [dbo].[USP_PurchaseFromEvaulationsWithActivitiesReportSalt] | `PurchasePrintRequests.Rpt180407PurchaseSummaryByParentCategoryAndSupplierRequest` |
| 1804A-InvRepPurchaseBillDirectWithoutPo.rpt | `/reports/print/1804a-inv-rep-purchase-bill-direct-without-po` | [dbo].[USP_PurchaseInvoiceDirectForSaltSlip] | `PurchasePrintRequests.Rpt1804AInvRepPurchaseBillDirectWithoutPoRequest` |
| 1858-GoodsReceiptsNotes_Slip.rpt | `/reports/print/1858-goods-receipts-notes-slip` | Sp_InvGrn_RiceSlip_Rpt | `PurchasePrintRequests.Rpt1858GoodsReceiptsNotesSlipRequest` |
| 203-InvRptPurchaseOrderRiceSlip(A).rpt | `/reports/print/203-purchase-order-rice-slip-a` | Sp_PurchaseOrderSlip_Rpt | `PurchasePrintRequests.Rpt203PurchaseOrderRiceSlipARequest` |
| 205-InvRptPurchaseOrderDetailRegisterRice.rpt | `/reports/print/205-purchase-order-detail-register-rice` | Sp_PurchaseOrder_RegisterDetail_Rpt | `PurchasePrintRequests.Rpt205PurchaseOrderDetailRegisterRiceRequest` |
| 206-RptPurchaseOrderRegisterRice.rpt | `/reports/print/206-purchase-order-register-rice` | Sp_PurchaseOrder_RegisterDetail_Rpt | `PurchasePrintRequests.Rpt206PurchaseOrderRegisterRiceRequest` |
| 213-GoodsReceiptsNotesAgainstOrderSlip.rpt | `/reports/print/213-goods-receipts-notes-against-order-slip` | Sp_InvGrn_RiceSlip_Rpt | `PurchasePrintRequests.Rpt213GoodsReceiptsNotesAgainstOrderSlipRequest` |
| 220-InvRptPurchaseBillSupplierRiceSlip.rpt | `/reports/print/220-purchase-bill-supplier-rice-slip` | SP_InvPurchaseInvoice_ItemOthersAddLess_SubRpt | `PurchasePrintRequests.Rpt220PurchaseBillSupplierRiceSlipRequest` |
| 220A-InvRptPurchaseBillSupplierRiceSlip.rpt | `/reports/print/220a-purchase-bill-supplier-rice-slip` | Sp_InvPurchaseInvoice_SupplierBill_Rpt | `PurchasePrintRequests.Rpt220APurchaseBillSupplierRiceSlipRequest` |
| 220B-InvRptPurchaseBillSupplierRiceSummary.rpt | `/reports/print/220b-purchase-bill-supplier-rice-summary` | SP_InvPurchaseInvoice_ItemOthersAddLess_SubRpt | `PurchasePrintRequests.Rpt220BPurchaseBillSupplierRiceSummaryRequest` |
| 221-InvRptPurchaseInvoicePurchaseAvgRateByItem.rpt | `/reports/print/221-purchase-invoice-purchase-avg-rate-by-item` | Sp_InvPurchaseInvoice_AvgRatesComparisonsByItemSupplier_Rpt | `PurchasePrintRequests.Rpt221PurchaseInvoicePurchaseAvgRateByItemRequest` |
| 221_GdnForSaleToPartyProcssingSlip.rpt | `/reports/print/221-gdn-for-sale-to-party-procssing-slip` | Sp_InvGdn_SlipAndRegisterRice_Rpt | `PurchasePrintRequests.Rpt221GdnForSaleToPartyProcssingSlipRequest` |
| 222-InvRptPurchaseInvoicePurchaseAvgRateByItemSupplier.rpt | `/reports/print/222-purchase-invoice-purchase-avg-rate-by-item-supplier` | Sp_InvPurchaseInvoice_AvgRatesComparisonsByItemSupplier_Rpt | `PurchasePrintRequests.Rpt222PurchaseInvoicePurchaseAvgRateByItemSupplierRequest` |
| 224-InvRptPurchaseBillRegister.rpt | `/reports/print/224-purchase-bill-register` | Sp_InvPurchaseInvoice_Rpt | `PurchasePrintRequests.Rpt224PurchaseBillRegisterRequest` |
| 225A-InvRepPurchaseBillDirectWithoutPo.rpt | `/reports/print/225a-inv-rep-purchase-bill-direct-without-po` | sp_InvpurchaseInvoiceDirectSlip | `PurchasePrintRequests.Rpt225AInvRepPurchaseBillDirectWithoutPoRequest` |
| 227-RptInvPurchaseInvoiceRegister.rpt | `/reports/print/227-inv-purchase-invoice-register` | Sp_InvPurchaseInvoiceTrading_SupplierBill_Register | `PurchasePrintRequests.Rpt227InvPurchaseInvoiceRegisterRequest` |
| 232-SpInventoryTransactions_PurchasesingRicePaddy_NewRpt_Format-I.rpt | `/reports/print/232-sp-inventory-transactions-purchasesing-rice-paddy-new-format-i` | SpInventoryTransactions_PurchasesingRicePaddy_Rpt | `PurchasePrintRequests.Rpt232SpInventoryTransactionsPurchasesingRicePaddyNewFormatIRequest` |
| 232-SpInventoryTransactions_PurchasesingRicePaddy_NewRpt_Format-II.rpt | `/reports/print/232-sp-inventory-transactions-purchasesing-rice-paddy-new-format-ii` | SpInventoryTransactions_PurchasesingRicePaddy_Rpt | `PurchasePrintRequests.Rpt232SpInventoryTransactionsPurchasesingRicePaddyNewFormatIIRequest` |
| 234-PurchaseRegisterSummary.rpt | `/reports/print/234-purchase-register-summary` | Usp_InvPurchaseInvoice_PurchasingReports | `PurchasePrintRequests.Rpt234PurchaseRegisterSummaryRequest` |
| 235-PurchaseRegisterSummaryByItem&Customer.rpt | `/reports/print/235-purchase-register-summary-by-item-customer` | Usp_InvPurchaseInvoice_PurchasingReports | `PurchasePrintRequests.Rpt235PurchaseRegisterSummaryByItemCustomerRequest` |
| 236-PurchaseRegisterSummaryByItem.rpt | `/reports/print/236-purchase-register-summary-by-item` | Usp_InvPurchaseInvoice_PurchasingReports | `PurchasePrintRequests.Rpt236PurchaseRegisterSummaryByItemRequest` |
| 245_PurchaseInvoiceDirectPM_PartySlip.rpt | `/reports/print/245-purchase-invoice-direct-pm-party-slip` | USP_InvPurchaseInvoiceDirect_PartySlip_Engr | `PurchasePrintRequests.Rpt245PurchaseInvoiceDirectPMPartySlipRequest` |
| 246_SupplierDispatchSlip.rpt | `/reports/print/246-supplier-dispatch-slip` | [dbo].[USP_SupplierDispatch_SlipAndRegister] | `PurchasePrintRequests.Rpt246SupplierDispatchSlipRequest` |
| 251_SupplierDispatchPreBillSlip.rpt | `/reports/print/251-supplier-dispatch-pre-bill-slip` | [dbo].[USP_SupplierDispatch_SlipAndRegister] | `PurchasePrintRequests.Rpt251SupplierDispatchPreBillSlipRequest` |
| 253_01-InvRptGatePassInwardRegisterA.rpt | `/reports/print/253-01-gate-pass-inward-register-a` | Sp_GatePassInward_StockReservedAsAmanat | `PurchasePrintRequests.Rpt25301GatePassInwardRegisterARequest` |
| 254-RptInwardGatePassSlipGeneral.rpt | `/reports/print/254-inward-gate-pass-slip-general` | Sp_GatePassGeneralInward_rpt | `PurchasePrintRequests.Rpt254InwardGatePassSlipGeneralRequest` |
| 260A-DeliveryChallansByGDN.rpt | `/reports/print/260a-delivery-challans-by-gdn` | Sp_InvGdn_SlipAndRegisterRice_Rpt | `PurchasePrintRequests.Rpt260ADeliveryChallansByGDNRequest` |
| 261-InvRptGdnRegister.rpt | `/reports/print/261-gdn-register` | Sp_InvGdn_Register_Rpt | `PurchasePrintRequests.Rpt261GdnRegisterRequest` |
| 264-DeliveryChallanByDeliveryOrder.rpt | `/reports/print/264-delivery-challan-by-delivery-order` | Sp_InvDeliveryOrder_Slip | `PurchasePrintRequests.Rpt264DeliveryChallanByDeliveryOrderRequest` |
| 264-DeliveryChallanByDeliveryOrderTransfer.rpt | `/reports/print/264-delivery-challan-by-delivery-order-transfer` | Sp_InvDeliveryOrder_Slip | `PurchasePrintRequests.Rpt264DeliveryChallanByDeliveryOrderTransferRequest` |
| 271-InvRptSalesOrderRegister.rpt | `/reports/print/271-sales-order-register` | Sp_PurchaseOrder_History_Rpt | `PurchasePrintRequests.Rpt271SalesOrderRegisterRequest` |
| 297-InvRptPurchase BillActivity.rpt | `/reports/print/297-purchase-bill-activity` | Sp_InvPurchaseInvoice_Rpt | `PurchasePrintRequests.Rpt297PurchaseBillActivityRequest` |
| 330-InvSupplyOrderSlip.rpt | `/reports/print/330-inv-supply-order-slip` | Sp_InvSupplyOrder_SlipandRegister | `PurchasePrintRequests.Rpt330InvSupplyOrderSlipRequest` |
| 334_01-GrnAuditReport.rpt | `/reports/print/334-01-grn-audit-report` | usp_getDataForGrnAudit | `PurchasePrintRequests.Rpt33401GrnAuditReportRequest` |
| 335_01-GrnRegisterSummaryItemWise.rpt | `/reports/print/335-01-grn-register-summary-item-wise` | usp_GrnRegisterSummaryItemWise | `PurchasePrintRequests.Rpt33501GrnRegisterSummaryItemWiseRequest` |
| 358_01_PendingGatePassForGRN.rpt | `/reports/print/358-01-pending-gate-pass-for-grn` | USP_GatePassInward_PendingGpForUnloading | `PurchasePrintRequests.Rpt35801PendingGatePassForGRNRequest` |
| 387-GRNWeightAuditReport.rpt | `/reports/print/387-grn-weight-audit-report` | USP_GRNWeightAuditReport | `PurchasePrintRequests.Rpt387GRNWeightAuditReportRequest` |
| 391-OrderRegister.rpt | `/reports/print/391-order-register` | [dbo].[USP_PurchaseOrderSummaryRegister] | `PurchasePrintRequests.Rpt391OrderRegisterRequest` |
| 392-OrderSummaryByItem.rpt | `/reports/print/392-order-summary-by-item` | [dbo].[USP_PurchaseOrderSummaryRegister] | `PurchasePrintRequests.Rpt392OrderSummaryByItemRequest` |
| 393-OrderSummaryBySupplier.rpt | `/reports/print/393-order-summary-by-supplier` | [dbo].[USP_PurchaseOrderSummaryRegister] | `PurchasePrintRequests.Rpt393OrderSummaryBySupplierRequest` |
| 394-OrderSummaryByCustomerandItem.rpt | `/reports/print/394-order-summary-by-customerand-item` | [dbo].[USP_PurchaseOrderSummaryRegister] | `PurchasePrintRequests.Rpt394OrderSummaryByCustomerandItemRequest` |
| 413-PaddyPurchaseRegister.rpt | `/reports/print/413-paddy-purchase-register` | USP_PaddyGatePurchase_Register | `PurchasePrintRequests.Rpt413PaddyPurchaseRegisterRequest` |
| 56_01_PurchaseRegister.rpt | `/reports/print/56-01-purchase-register` | [dbo].[USP_PurchaseReportWithActivitiesFromEvaulations] | `PurchasePrintRequests.Rpt5601PurchaseRegisterRequest` |
| 56_02_PurchaseSummaryByItem.rpt | `/reports/print/56-02-purchase-summary-by-item` | [dbo].[USP_PurchaseReportWithActivitiesFromEvaulations] | `PurchasePrintRequests.Rpt5602PurchaseSummaryByItemRequest` |
| 56_03_PurchaseSummaryByItemAndWarehouse.rpt | `/reports/print/56-03-purchase-summary-by-item-and-warehouse` | [dbo].[USP_PurchaseReportWithActivitiesFromEvaulations] | `PurchasePrintRequests.Rpt5603PurchaseSummaryByItemAndWarehouseRequest` |
| 56_04_PurchaseSummaryBySupplier.rpt | `/reports/print/56-04-purchase-summary-by-supplier` | [dbo].[USP_PurchaseReportWithActivitiesFromEvaulations] | `PurchasePrintRequests.Rpt5604PurchaseSummaryBySupplierRequest` |
| 56_05_PurchaseSummaryByItemAndSupplier.rpt | `/reports/print/56-05-purchase-summary-by-item-and-supplier` | [dbo].[USP_PurchaseReportWithActivitiesFromEvaulations] | `PurchasePrintRequests.Rpt5605PurchaseSummaryByItemAndSupplierRequest` |
| 56_06_PurchaseSummaryByParentCategory.rpt | `/reports/print/56-06-purchase-summary-by-parent-category` | [dbo].[USP_PurchaseReportWithActivitiesFromEvaulations] | `PurchasePrintRequests.Rpt5606PurchaseSummaryByParentCategoryRequest` |
| 56_07_PurchaseSummaryByParentCategoryAndSupplier.rpt | `/reports/print/56-07-purchase-summary-by-parent-category-and-supplier` | [dbo].[USP_PurchaseReportWithActivitiesFromEvaulations] | `PurchasePrintRequests.Rpt5607PurchaseSummaryByParentCategoryAndSupplierRequest` |
| 56_08_PurchaseAndSaleDetailByJobLot.rpt | `/reports/print/56-08-purchase-and-sale-detail-by-job-lot` | [dbo].[usp_PurchaseAndSaleDetailByJobLot] | `PurchasePrintRequests.Rpt5608PurchaseAndSaleDetailByJobLotRequest` |
| 56_09_PurchaseSummaryByHsCode.rpt | `/reports/print/56-09-purchase-summary-by-hs-code` | [dbo].[USP_PurchaseReportWithActivitiesFromEvaulations] | `PurchasePrintRequests.Rpt5609PurchaseSummaryByHsCodeRequest` |
| 910_01_AdvanceDeliveryOrderSlip.rpt | `/reports/print/910-01-advance-delivery-order-slip` | Sp_InvDeliveryOrder_Slip | `PurchasePrintRequests.Rpt91001AdvanceDeliveryOrderSlipRequest` |
| 910_AdvanceDeliveryOrderSlip.rpt | `/reports/print/910-advance-delivery-order-slip` | Sp_InvDeliveryOrder_Slip | `PurchasePrintRequests.Rpt910AdvanceDeliveryOrderSlipRequest` |
| 910A_AdvanceDeliveryChallanSlip.rpt | `/reports/print/910a-advance-delivery-challan-slip` | Sp_InvDeliveryOrder_Slip | `PurchasePrintRequests.Rpt910AAdvanceDeliveryChallanSlipRequest` |
| 98_SaleInvoiceReturnItemSlip.rpt | `/reports/print/98-sale-invoice-return-item-slip` | sp_InvpurchaseInvoiceDirectSlip | `PurchasePrintRequests.Rpt98SaleInvoiceReturnItemSlipRequest` |
| 98A_SaleInvoiceReturnPartySlip.rpt | `/reports/print/98a-sale-invoice-return-party-slip` | sp_InvpurchaseInvoiceDirectSlip | `PurchasePrintRequests.Rpt98ASaleInvoiceReturnPartySlipRequest` |
| getLedgerforPrint.rpt | `/reports/print/get-ledgerfor-print` | usp_getLedgerforPrint | `PurchasePrintRequests.RptGetLedgerforPrintRequest` |
| GrnDetailEmptyBagsSubReport.rpt | `/reports/print/grn-detail-empty-bags-sub-report` | Sp_InvGrnDetailEmptyBagsSubReport | `PurchasePrintRequests.RptGrnDetailEmptyBagsSubReportRequest` |
| InvRptPurchaseBillSupplierOthers.rpt | `/reports/print/purchase-bill-supplier-others` | SP_InvSaleInvoice_CustomerBillRice OthersExp_SubRep | `PurchasePrintRequests.RptPurchaseBillSupplierOthersRequest` |
| InvRptPurchaseItemBillOthers.rpt | `/reports/print/purchase-item-bill-others` | SP_InvPurchaseInvoice_ItemOthersAddLess_SubRpt | `PurchasePrintRequests.RptPurchaseItemBillOthersRequest` |
| PurchaseOrder_SupplierDispatchSubReport.rpt | `/reports/print/purchase-order-supplier-dispatch-sub-report` | USP_PurchaseOrder_SupplierDispatchSubReport | `PurchasePrintRequests.RptPurchaseOrderSupplierDispatchSubReportRequest` |
| PurchaseOrder_WithGrnDetail_SubReport.rpt | `/reports/print/purchase-order-with-grn-detail-sub-report` | [dbo].[USP_PurchaseOrder_WithGrnDetail_Report] | `PurchasePrintRequests.RptPurchaseOrderWithGrnDetailSubReportRequest` |
| PurchaseOrderLabSampleSubReport.rpt | `/reports/print/purchase-order-lab-sample-sub-report` | [dbo].[USP_PurchaseOrderLabSample_SubReport] | `PurchasePrintRequests.RptPurchaseOrderLabSampleSubReportRequest` |
| PurchaseOrderSubReport.rpt | `/reports/print/purchase-order-sub-report` | USP-PurchaseOrderSubReport | `PurchasePrintRequests.RptPurchaseOrderSubReportRequest` |
| PurchaseOrderSupplierExpenseSubReport.rpt | `/reports/print/purchase-order-supplier-expense-sub-report` | [dbo].[USP_PurchaseOrderSupplierExpense_SubReport] | `PurchasePrintRequests.RptPurchaseOrderSupplierExpenseSubReportRequest` |
| SupplierBillReport.rpt | `/reports/print/supplier-bill-report` | SP_InvPurchaseInvoice_SupplierBillOthersAddLess_SubRpt | `PurchasePrintRequests.RptSupplierBillReportRequest` |

## Sale

| Template | GET / POST endpoint | Procedure | Request |
|---|---|---|---|
| 260-InvRptGdnRiceSlip.rpt | `/reports/print/260-gdn-rice-slip` | Sp_InvGdn_SlipAndRegisterRice_Rpt | `SalePrintRequests.Rpt260GdnRiceSlipRequest` |
| 703_Gdn_PurchaseReturnSlip.rpt | `/reports/print/703-gdn-purchase-return-slip` | [dbo].[USP_InvGdn_PurchaseReturnReport] | `SalePrintRequests.Rpt703GdnPurchaseReturnSlipRequest` |
| 703A_Gdn_PurchaseReturnSlip.rpt | `/reports/print/703a-gdn-purchase-return-slip` | [dbo].[USP_InvGdn_PurchaseReturnReport] | `SalePrintRequests.Rpt703AGdnPurchaseReturnSlipRequest` |
| 273-InvRptSaleOrderSlip.rpt | `/reports/print/273-sale-order-slip` | Sp_SaleOrder_RiceSlip_Rpt | `SalePrintRequests.Rpt273SaleOrderSlipRequest` |
| 273_01_SaleOrderSlip.rpt | `/reports/print/273-01-sale-order-slip` | Sp_SaleOrder_RiceSlip_Rpt | `SalePrintRequests.Rpt27301SaleOrderSlipRequest` |
| 07_RptGatepassOutwardDriverInfoSlipA.rpt | `/reports/print/07-gatepass-outward-driver-info-slip-a` | Sp_GatePassOutwardDriverInfo_rpt | `SalePrintRequests.Rpt07GatepassOutwardDriverInfoSlipARequest` |
| 258-OutwardGatePassWithWbAndLabSlip.rpt | `/reports/print/258-outward-gate-pass-with-wb-and-lab-slip` | Sp_GatePassOutward_SlipAndRegister_Rpt | `SalePrintRequests.Rpt258OutwardGatePassWithWbAndLabSlipRequest` |
| 1856-SaleInvoiceDirect_Slip.rpt | `/reports/print/1856-sale-invoice-direct-slip` | pcc.USP_InvSaleInvoice_DirectSlip | `SalePrintRequests.Rpt1856SaleInvoiceDirectSlipRequest` |
| 1862-SaleInvoiceRetrurn_Slip.rpt | `/reports/print/1862-sale-invoice-retrurn-slip` | pcc.USP_InvSaleInvoice_DirectSlip | `SalePrintRequests.Rpt1862SaleInvoiceRetrurnSlipRequest` |
| 1200-DeliveryOrderSlipReturnable.rpt | `/reports/print/1200-delivery-order-slip-returnable` | USP_DeliveryOrder_Slip_Engr | `SalePrintRequests.Rpt1200DeliveryOrderSlipReturnableRequest` |
| 1201_OutwardGatePassReturnable_Slip.rpt | `/reports/print/1201-outward-gate-pass-returnable-slip` | Sp_GatePassOutward_SlipAndRegister_Rpt | `SalePrintRequests.Rpt1201OutwardGatePassReturnableSlipRequest` |
| 122_03_SaleAnalyticsItemWise.rpt | `/reports/print/122-03-sale-analytics-item-wise` | spInventoryStockEvaluation_LocalSalesComaprisons_Report | `SalePrintRequests.Rpt12203SaleAnalyticsItemWiseRequest` |
| 122_04_SaleAnalyticsCustomerWise.rpt | `/reports/print/122-04-sale-analytics-customer-wise` | spInventoryStockEvaluation_LocalSalesComaprisons_Report | `SalePrintRequests.Rpt12204SaleAnalyticsCustomerWiseRequest` |
| 146-OutstandingOrderWithLedgerBalance.rpt | `/reports/print/146-outstanding-order-with-ledger-balance` | USP_GetOutstandingOrdersWithLedgerBalance | `SalePrintRequests.Rpt146OutstandingOrderWithLedgerBalanceRequest` |
| 1605-SaleOrderSlipAndRegister_Engr.rpt | `/reports/print/1605-sale-order-slip-and-register-engr` | USP_SaleOrderSlipAndRegister_Engr | `SalePrintRequests.Rpt1605SaleOrderSlipAndRegisterEngrRequest` |
| 1605A-InvRptSalesOrderRegister.rpt | `/reports/print/1605a-sales-order-register` | USP_SaleOrderSlipAndRegister_Engr | `SalePrintRequests.Rpt1605ASalesOrderRegisterRequest` |
| 1605B-InvRptSaleOderRegister.rpt | `/reports/print/1605b-sale-oder-register` | USP_SaleOrderSlipAndRegister_Engr | `SalePrintRequests.Rpt1605BSaleOderRegisterRequest` |
| 1606-DeliveryOrderSlip_Engr.rpt | `/reports/print/1606-delivery-order-slip-engr` | USP_DeliveryOrder_Slip_Engr | `SalePrintRequests.Rpt1606DeliveryOrderSlipEngrRequest` |
| 1606A-InvDeliveryOrderForApproval_Register.rpt | `/reports/print/1606a-inv-delivery-order-for-approval-register` | USP_InvDeliveryOrderForApproval_Engr | `SalePrintRequests.Rpt1606AInvDeliveryOrderForApprovalRegisterRequest` |
| 1608-SaleInvoice_CustomerBillDirect_Engr.rpt | `/reports/print/1608-sale-invoice-customer-bill-direct-engr` | USP_SaleInvoice_CustomerBillDirect_Engr | `SalePrintRequests.Rpt1608SaleInvoiceCustomerBillDirectEngrRequest` |
| 1608A-SaleInvoice_CustomerBillDirect_Engr.rpt | `/reports/print/1608a-sale-invoice-customer-bill-direct-engr` | USP_SaleInvoice_CustomerBillDirect_Engr | `SalePrintRequests.Rpt1608ASaleInvoiceCustomerBillDirectEngrRequest` |
| 1609-InvRepSaleBillCustomer.rpt | `/reports/print/1609-inv-rep-sale-bill-customer` | [dbo].[Sp_InvSaleInvoice_Slip_Eng] | `SalePrintRequests.Rpt1609InvRepSaleBillCustomerRequest` |
| 1609A-InvRepSaleBillCustomer-Format-II.rpt | `/reports/print/1609a-inv-rep-sale-bill-customer-format-ii` | [dbo].[Sp_InvSaleInvoice_Slip_Eng] | `SalePrintRequests.Rpt1609AInvRepSaleBillCustomerFormatIIRequest` |
| 1611-SaleInvoice_CustomerBillReturn_Engr.rpt | `/reports/print/1611-sale-invoice-customer-bill-return-engr` | USP_SaleInvoice_CustomerBillDirect_Engr | `SalePrintRequests.Rpt1611SaleInvoiceCustomerBillReturnEngrRequest` |
| 1611A-SaleInvoice_CustomerBillReturn_Engr.rpt | `/reports/print/1611a-sale-invoice-customer-bill-return-engr` | USP_SaleInvoice_CustomerBillDirect_Engr | `SalePrintRequests.Rpt1611ASaleInvoiceCustomerBillReturnEngrRequest` |
| 1612-GdnRegister_Eng.rpt | `/reports/print/1612-gdn-register-eng` | [dbo].[USP_GdnRegister_Eng] | `SalePrintRequests.Rpt1612GdnRegisterEngRequest` |
| 1656_SaleOrderSlip_Engr.rpt | `/reports/print/1656-sale-order-slip-engr` | USP_SaleOrderSlipAndRegister_Engr | `SalePrintRequests.Rpt1656SaleOrderSlipEngrRequest` |
| 1656A-InvRptSalesOrderRegister.rpt | `/reports/print/1656a-sales-order-register` | USP_SaleOrderSlipAndRegister_Engr | `SalePrintRequests.Rpt1656ASalesOrderRegisterRequest` |
| 1656B-InvRptSaleOderRegister.rpt | `/reports/print/1656b-sale-oder-register` | USP_SaleOrderSlipAndRegister_Engr | `SalePrintRequests.Rpt1656BSaleOderRegisterRequest` |
| 1657_DeliveryOrderSlip_Engr.rpt | `/reports/print/1657-delivery-order-slip-engr` | USP_DeliveryOrder_Slip_Engr | `SalePrintRequests.Rpt1657DeliveryOrderSlipEngrRequest` |
| 1657A-InvDeliveryOrderForApproval_Register.rpt | `/reports/print/1657a-inv-delivery-order-for-approval-register` | USP_InvDeliveryOrderForApproval_Engr | `SalePrintRequests.Rpt1657AInvDeliveryOrderForApprovalRegisterRequest` |
| 1660_SaleInvoiceSlipEngr.rpt | `/reports/print/1660-sale-invoice-slip-engr` | [dbo].[Sp_InvSaleInvoice_Slip_Eng] | `SalePrintRequests.Rpt1660SaleInvoiceSlipEngrRequest` |
| 1660A_SaleInvoiceSlipEngr.rpt | `/reports/print/1660a-sale-invoice-slip-engr` | [dbo].[Sp_InvSaleInvoice_Slip_Eng] | `SalePrintRequests.Rpt1660ASaleInvoiceSlipEngrRequest` |
| 1661_SaleInvoice_CustomerBillDirectSlip_Engr.rpt | `/reports/print/1661-sale-invoice-customer-bill-direct-slip-engr` | USP_SaleInvoice_CustomerBillDirect_Engr | `SalePrintRequests.Rpt1661SaleInvoiceCustomerBillDirectSlipEngrRequest` |
| 1661A_SaleInvoice_CustomerBillDirectSlip_Engr.rpt | `/reports/print/1661a-sale-invoice-customer-bill-direct-slip-engr` | USP_SaleInvoice_CustomerBillDirect_Engr | `SalePrintRequests.Rpt1661ASaleInvoiceCustomerBillDirectSlipEngrRequest` |
| 1662_SaleInvoiceReturn_Slip_Engr.rpt | `/reports/print/1662-sale-invoice-return-slip-engr` | USP_SaleInvoice_CustomerBillDirect_Engr | `SalePrintRequests.Rpt1662SaleInvoiceReturnSlipEngrRequest` |
| 1662A_SaleInvoiceReturn_Slip_Engr.rpt | `/reports/print/1662a-sale-invoice-return-slip-engr` | USP_SaleInvoice_CustomerBillDirect_Engr | `SalePrintRequests.Rpt1662ASaleInvoiceReturnSlipEngrRequest` |
| 1809-SaleInvoiceDirectSlip_ForSalt.rpt | `/reports/print/1809-sale-invoice-direct-slip-for-salt` | [dbo].[USP_SaleInvoiceDirectSlip_ForSalt] | `SalePrintRequests.Rpt1809SaleInvoiceDirectSlipForSaltRequest` |
| 1809_01_SalesRegisterSummary.rpt | `/reports/print/1809-01-sales-register-summary` | [dbo].[USP_SalesFromEvaulation_RegisterSalt] | `SalePrintRequests.Rpt180901SalesRegisterSummaryRequest` |
| 1809_02-SalesRegisterSummaryByCustomer.rpt | `/reports/print/1809-02-sales-register-summary-by-customer` | [dbo].[USP_SalesFromEvaulation_RegisterSalt] | `SalePrintRequests.Rpt180902SalesRegisterSummaryByCustomerRequest` |
| 1809_03-SalesRegisterSummaryByItem.rpt | `/reports/print/1809-03-sales-register-summary-by-item` | [dbo].[USP_SalesFromEvaulation_RegisterSalt] | `SalePrintRequests.Rpt180903SalesRegisterSummaryByItemRequest` |
| 1809_04-SalesRegisterSummaryByCustomer&Item.rpt | `/reports/print/1809-04-sales-register-summary-by-customer-item` | [dbo].[USP_SalesFromEvaulation_RegisterSalt] | `SalePrintRequests.Rpt180904SalesRegisterSummaryByCustomerItemRequest` |
| 1809_05-SalesRegisterSummaryByItemWithoutPacking.rpt | `/reports/print/1809-05-sales-register-summary-by-item-without-packing` | [dbo].[USP_SalesFromEvaulation_RegisterSalt] | `SalePrintRequests.Rpt180905SalesRegisterSummaryByItemWithoutPackingRequest` |
| 1809_06-SalesRegisterSummaryByWarehouse.rpt | `/reports/print/1809-06-sales-register-summary-by-warehouse` | [dbo].[USP_SalesFromEvaulation_RegisterSalt] | `SalePrintRequests.Rpt180906SalesRegisterSummaryByWarehouseRequest` |
| 1809_07-SalesSummaryByCustomer&City.rpt | `/reports/print/1809-07-sales-summary-by-customer-city` | [dbo].[USP_SalesFromEvaulation_RegisterSalt] | `SalePrintRequests.Rpt180907SalesSummaryByCustomerCityRequest` |
| 1809_09-SalesSummaryByCustomerItem&City.rpt | `/reports/print/1809-09-sales-summary-by-customer-item-city` | [dbo].[USP_SalesFromEvaulation_RegisterSalt] | `SalePrintRequests.Rpt180909SalesSummaryByCustomerItemCityRequest` |
| 1809_10-SalesSummaryByCustomer&PackSize.rpt | `/reports/print/1809-10-sales-summary-by-customer-pack-size` | [dbo].[USP_SalesFromEvaulation_RegisterSalt] | `SalePrintRequests.Rpt180910SalesSummaryByCustomerPackSizeRequest` |
| 1809_11-SalesSummaryByParentCategory.rpt | `/reports/print/1809-11-sales-summary-by-parent-category` | [dbo].[USP_SalesFromEvaulation_RegisterSalt] | `SalePrintRequests.Rpt180911SalesSummaryByParentCategoryRequest` |
| 1809_12-SalesSummaryByParentCategory&Item.rpt | `/reports/print/1809-12-sales-summary-by-parent-category-item` | [dbo].[USP_SalesFromEvaulation_RegisterSalt] | `SalePrintRequests.Rpt180912SalesSummaryByParentCategoryItemRequest` |
| 1809_13-SalesSummaryByParentCategory&Customer.rpt | `/reports/print/1809-13-sales-summary-by-parent-category-customer` | [dbo].[USP_SalesFromEvaulation_RegisterSalt] | `SalePrintRequests.Rpt180913SalesSummaryByParentCategoryCustomerRequest` |
| 1809_14-SalesSummaryByItemPackSize&City.rpt | `/reports/print/1809-14-sales-summary-by-item-pack-size-city` | [dbo].[USP_SalesFromEvaulation_RegisterSalt] | `SalePrintRequests.Rpt180914SalesSummaryByItemPackSizeCityRequest` |
| 1809_15-SalesSummaryByItem&City.rpt | `/reports/print/1809-15-sales-summary-by-item-city` | [dbo].[USP_SalesFromEvaulation_RegisterSalt] | `SalePrintRequests.Rpt180915SalesSummaryByItemCityRequest` |
| 1809A-SaleInvoiceDirectSlip_ForSalt.rpt | `/reports/print/1809a-sale-invoice-direct-slip-for-salt` | [dbo].[USP_SaleInvoiceDirectSlip_ForSalt] | `SalePrintRequests.Rpt1809ASaleInvoiceDirectSlipForSaltRequest` |
| 199-GetOutstandingOrdersWithLedgerBalance.rpt | `/reports/print/199-get-outstanding-orders-with-ledger-balance` | USP_GetOutstandingOrdersWithLedgerBalance | `SalePrintRequests.Rpt199GetOutstandingOrdersWithLedgerBalanceRequest` |
| 226-RptInvSaleInvoiceRegister.rpt | `/reports/print/226-inv-sale-invoice-register` | Sp_InvSaleInvoiceTrading_CustomerBill_Register | `SalePrintRequests.Rpt226InvSaleInvoiceRegisterRequest` |
| 263-InvDeliveryOrderForApprovel.rpt | `/reports/print/263-inv-delivery-order-for-approvel` | Sp_Inventory_ReadDashboardPendingForApproval | `SalePrintRequests.Rpt263InvDeliveryOrderForApprovelRequest` |
| 273-InvRptSaleOrderSlip(A).rpt | `/reports/print/273-sale-order-slip-a` | Sp_SaleOrder_RiceSlip_Rpt | `SalePrintRequests.Rpt273SaleOrderSlipARequest` |
| 274_PreBookingOrder_Slip.rpt | `/reports/print/274-pre-booking-order-slip` | usp_PreBookingOrder_Slip | `SalePrintRequests.Rpt274PreBookingOrderSlipRequest` |
| 275-InvRptSalesOrderGeneral.rpt | `/reports/print/275-sales-order-general` | Sp_SaleOrder_RiceSlip_Rpt | `SalePrintRequests.Rpt275SalesOrderGeneralRequest` |
| 282-SaleInvoice_DirectFlour.rpt | `/reports/print/282-sale-invoice-direct-flour` | sp_InvSaleInvoiceDirectSlip | `SalePrintRequests.Rpt282SaleInvoiceDirectFlourRequest` |
| 282A-SaleInvoice_DirectFlour.rpt | `/reports/print/282a-sale-invoice-direct-flour` | sp_InvSaleInvoiceDirectSlip | `SalePrintRequests.Rpt282ASaleInvoiceDirectFlourRequest` |
| 290-InvRptOutwardGatePassSlip.rpt | `/reports/print/290-outward-gate-pass-slip` | Sp_GatePassOutward_SlipAndRegister_Rpt | `SalePrintRequests.Rpt290OutwardGatePassSlipRequest` |
| 291-InvRptGatePassOutwardRegister.rpt | `/reports/print/291-gate-pass-outward-register` | Sp_GatePassOutward_SlipAndRegister_Rpt | `SalePrintRequests.Rpt291GatePassOutwardRegisterRequest` |
| 291A-SaleInvoice_AutoRated.rpt | `/reports/print/291a-sale-invoice-auto-rated` | sp_InvSaleInvoiceDirectSlip | `SalePrintRequests.Rpt291ASaleInvoiceAutoRatedRequest` |
| 294A-InvRptSaleBillDirectWithoutSO.rpt | `/reports/print/294a-sale-bill-direct-without-so` | sp_InvSaleInvoiceDirectSlip | `SalePrintRequests.Rpt294ASaleBillDirectWithoutSORequest` |
| 294B-InvRptSaleBillDirectWithoutSO.rpt | `/reports/print/294b-sale-bill-direct-without-so` | sp_InvSaleInvoiceDirectSlip | `SalePrintRequests.Rpt294BSaleBillDirectWithoutSORequest` |
| 294C_SaleBillDirectItemSlip.rpt | `/reports/print/294c-sale-bill-direct-item-slip` | sp_InvSaleInvoiceDirectSlip | `SalePrintRequests.Rpt294CSaleBillDirectItemSlipRequest` |
| 295-InvRptOutwardGatePassSlipWithItems.rpt | `/reports/print/295-outward-gate-pass-slip-with-items` | Sp_GatePassOutward_SlipAndRegister_Rpt | `SalePrintRequests.Rpt295OutwardGatePassSlipWithItemsRequest` |
| 299-SalesOrderRegistery.rpt | `/reports/print/299-sales-order-registery` | [dbo].[USP_SaleOrderDetailRegister_Eng] | `SalePrintRequests.Rpt299SalesOrderRegisteryRequest` |
| 301-InvRepSaleBillCustomer.rpt | `/reports/print/301-inv-rep-sale-bill-customer` | SP_InvSaleInvoice_CustomerBillRice OthersExp_SubRep | `SalePrintRequests.Rpt301InvRepSaleBillCustomerRequest` |
| 302-InvSaleInvoice.rpt | `/reports/print/302-inv-sale-invoice` | sp_InvSaleInvoiceDirectRegister | `SalePrintRequests.Rpt302InvSaleInvoiceRequest` |
| 303-InvRepSaleBillCustomer-Format-II.rpt | `/reports/print/303-inv-rep-sale-bill-customer-format-ii` | Sp_InvSaleInvoice_CustomerBillRice_Rpt | `SalePrintRequests.Rpt303InvRepSaleBillCustomerFormatIIRequest` |
| 303A_SaleInvoiceItemSlip.rpt | `/reports/print/303a-sale-invoice-item-slip` | Sp_InvSaleInvoice_CustomerBillRice_Rpt | `SalePrintRequests.Rpt303ASaleInvoiceItemSlipRequest` |
| 310-InvrptSalesInvoiceMHI.rpt | `/reports/print/310-sales-invoice-mhi` | Sp_InvSaleInvoiceTrading_CustomerBill | `SalePrintRequests.Rpt310SalesInvoiceMHIRequest` |
| 311-InvrptSalesInvoiceMHII.rpt | `/reports/print/311-sales-invoice-mhii` | Sp_InvSaleInvoiceTrading_CustomerBill | `SalePrintRequests.Rpt311SalesInvoiceMHIIRequest` |
| 312-SalesRegisterSummary.rpt | `/reports/print/312-sales-register-summary` | SpInventory_EvaulationDetailSalesReports | `SalePrintRequests.Rpt312SalesRegisterSummaryRequest` |
| 313-SalesRegisterSummaryByCustomer.rpt | `/reports/print/313-sales-register-summary-by-customer` | SpInventory_EvaulationDetailSalesReports | `SalePrintRequests.Rpt313SalesRegisterSummaryByCustomerRequest` |
| 314-SalesRegisterSummaryByItem.rpt | `/reports/print/314-sales-register-summary-by-item` | SpInventory_EvaulationDetailSalesReports | `SalePrintRequests.Rpt314SalesRegisterSummaryByItemRequest` |
| 315-SalesRegisterSummaryByCustomer&Item.rpt | `/reports/print/315-sales-register-summary-by-customer-item` | SpInventory_EvaulationDetailSalesReports | `SalePrintRequests.Rpt315SalesRegisterSummaryByCustomerItemRequest` |
| 316-SalesRegisterSummaryByItemWithoutPacking.rpt | `/reports/print/316-sales-register-summary-by-item-without-packing` | SpInventory_EvaulationDetailSalesReports | `SalePrintRequests.Rpt316SalesRegisterSummaryByItemWithoutPackingRequest` |
| 317-SalesRegisterSummaryByWarehouse.rpt | `/reports/print/317-sales-register-summary-by-warehouse` | SpInventory_EvaulationDetailSalesReports | `SalePrintRequests.Rpt317SalesRegisterSummaryByWarehouseRequest` |
| 318-InvRepSaleBillCustomer.rpt | `/reports/print/318-inv-rep-sale-bill-customer` | Sp_InvSaleInvoice_CustomerBillRice_Rpt | `SalePrintRequests.Rpt318InvRepSaleBillCustomerRequest` |
| 318A-InvRepSaleBillCustomer.rpt | `/reports/print/318a-inv-rep-sale-bill-customer` | Sp_InvSaleInvoice_CustomerBillRice_Rpt | `SalePrintRequests.Rpt318AInvRepSaleBillCustomerRequest` |
| 342-SalesSummaryByItem&City.rpt | `/reports/print/342-sales-summary-by-item-city` | SpInventory_EvaulationDetailSalesReports | `SalePrintRequests.Rpt342SalesSummaryByItemCityRequest` |
| 343-SalesSummaryByCustomer&City.rpt | `/reports/print/343-sales-summary-by-customer-city` | SpInventory_EvaulationDetailSalesReports | `SalePrintRequests.Rpt343SalesSummaryByCustomerCityRequest` |
| 344-SalesSummaryByItemPackSize&City.rpt | `/reports/print/344-sales-summary-by-item-pack-size-city` | SpInventory_EvaulationDetailSalesReports | `SalePrintRequests.Rpt344SalesSummaryByItemPackSizeCityRequest` |
| 345-SalesSummaryByCustomerItem&City.rpt | `/reports/print/345-sales-summary-by-customer-item-city` | SpInventory_EvaulationDetailSalesReports | `SalePrintRequests.Rpt345SalesSummaryByCustomerItemCityRequest` |
| 346-SalesSummaryByCustomer&PackSize.rpt | `/reports/print/346-sales-summary-by-customer-pack-size` | SpInventory_EvaulationDetailSalesReports | `SalePrintRequests.Rpt346SalesSummaryByCustomerPackSizeRequest` |
| 347-SalesSummaryByParentCategory.rpt | `/reports/print/347-sales-summary-by-parent-category` | SpInventory_EvaulationDetailSalesReports | `SalePrintRequests.Rpt347SalesSummaryByParentCategoryRequest` |
| 348-SalesSummaryByParentCategory&Item.rpt | `/reports/print/348-sales-summary-by-parent-category-item` | SpInventory_EvaulationDetailSalesReports | `SalePrintRequests.Rpt348SalesSummaryByParentCategoryItemRequest` |
| 349-SalesSummaryByParentCategory&Customer.rpt | `/reports/print/349-sales-summary-by-parent-category-customer` | SpInventory_EvaulationDetailSalesReports | `SalePrintRequests.Rpt349SalesSummaryByParentCategoryCustomerRequest` |
| 350-SalesSummaryByReferenceParty.rpt | `/reports/print/350-sales-summary-by-reference-party` | SpInventory_EvaulationDetailSalesReports | `SalePrintRequests.Rpt350SalesSummaryByReferencePartyRequest` |
| 351-SalesSummaryByReferenceParty&City.rpt | `/reports/print/351-sales-summary-by-reference-party-city` | SpInventory_EvaulationDetailSalesReports | `SalePrintRequests.Rpt351SalesSummaryByReferencePartyCityRequest` |
| 352-SalesSummaryByReferenceParty&PackSize.rpt | `/reports/print/352-sales-summary-by-reference-party-pack-size` | SpInventory_EvaulationDetailSalesReports | `SalePrintRequests.Rpt352SalesSummaryByReferencePartyPackSizeRequest` |
| 358-PendingGatePassForDelivery.rpt | `/reports/print/358-pending-gate-pass-for-delivery` | USP_PendingGatePassForDelivery | `SalePrintRequests.Rpt358PendingGatePassForDeliveryRequest` |
| 360-SalesRegisterSummary.rpt | `/reports/print/360-sales-register-summary` | USP_Sales_EvaulationDetailReports_Engr | `SalePrintRequests.Rpt360SalesRegisterSummaryRequest` |
| 361-SalesSummaryByItem.rpt | `/reports/print/361-sales-summary-by-item` | USP_Sales_EvaulationDetailReports_Engr | `SalePrintRequests.Rpt361SalesSummaryByItemRequest` |
| 362-SalesSummaryByItem&City.rpt | `/reports/print/362-sales-summary-by-item-city` | USP_Sales_EvaulationDetailReports_Engr | `SalePrintRequests.Rpt362SalesSummaryByItemCityRequest` |
| 363-SalesSummaryByItem&PackSize.rpt | `/reports/print/363-sales-summary-by-item-pack-size` | USP_Sales_EvaulationDetailReports_Engr | `SalePrintRequests.Rpt363SalesSummaryByItemPackSizeRequest` |
| 364-SalesSummaryByItem&Warehouse.rpt | `/reports/print/364-sales-summary-by-item-warehouse` | USP_Sales_EvaulationDetailReports_Engr | `SalePrintRequests.Rpt364SalesSummaryByItemWarehouseRequest` |
| 365-SalesSummaryByItemPackSize&City.rpt | `/reports/print/365-sales-summary-by-item-pack-size-city` | USP_Sales_EvaulationDetailReports_Engr | `SalePrintRequests.Rpt365SalesSummaryByItemPackSizeCityRequest` |
| 366-SalesSummaryByCustomer.rpt | `/reports/print/366-sales-summary-by-customer` | USP_Sales_EvaulationDetailReports_Engr | `SalePrintRequests.Rpt366SalesSummaryByCustomerRequest` |
| 366ASalesSummaryByCustomer&Invoice.rpt | `/reports/print/366a-366-a-sales-summary-by-customer-invoice` | USP_Sales_EvaulationDetailReports_Engr | `SalePrintRequests.Rpt366A366ASalesSummaryByCustomerInvoiceRequest` |
| 366B-SalesSummaryByCustomer&Invoice.rpt | `/reports/print/366b-sales-summary-by-customer-invoice` | USP_Sales_EvaulationDetailReports_Engr | `SalePrintRequests.Rpt366BSalesSummaryByCustomerInvoiceRequest` |
| 367-SalesSummaryByCustomer&Item.rpt | `/reports/print/367-sales-summary-by-customer-item` | USP_Sales_EvaulationDetailReports_Engr | `SalePrintRequests.Rpt367SalesSummaryByCustomerItemRequest` |
| 368-SalesSummaryByCustomer&City.rpt | `/reports/print/368-sales-summary-by-customer-city` | USP_Sales_EvaulationDetailReports_Engr | `SalePrintRequests.Rpt368SalesSummaryByCustomerCityRequest` |
| 369-SalesSummaryByCustomerItem&City.rpt | `/reports/print/369-sales-summary-by-customer-item-city` | USP_Sales_EvaulationDetailReports_Engr | `SalePrintRequests.Rpt369SalesSummaryByCustomerItemCityRequest` |
| 370-SalesSummaryByCustomer&PackSize.rpt | `/reports/print/370-sales-summary-by-customer-pack-size` | USP_Sales_EvaulationDetailReports_Engr | `SalePrintRequests.Rpt370SalesSummaryByCustomerPackSizeRequest` |
| 371-SalesSummaryByCity.rpt | `/reports/print/371-sales-summary-by-city` | USP_Sales_EvaulationDetailReports_Engr | `SalePrintRequests.Rpt371SalesSummaryByCityRequest` |
| 372-SalesSummaryByParentCategory.rpt | `/reports/print/372-sales-summary-by-parent-category` | USP_Sales_EvaulationDetailReports_Engr | `SalePrintRequests.Rpt372SalesSummaryByParentCategoryRequest` |
| 373-SalesSummaryByParentCategory&Item.rpt | `/reports/print/373-sales-summary-by-parent-category-item` | USP_Sales_EvaulationDetailReports_Engr | `SalePrintRequests.Rpt373SalesSummaryByParentCategoryItemRequest` |
| 374-SalesSummaryByParentCategory&Customer.rpt | `/reports/print/374-sales-summary-by-parent-category-customer` | USP_Sales_EvaulationDetailReports_Engr | `SalePrintRequests.Rpt374SalesSummaryByParentCategoryCustomerRequest` |
| 376-SalesSummaryByCustomer,ItemReferenceParty.rpt | `/reports/print/376-sales-summary-by-customer-item-reference-party` | SpInventory_EvaulationDetailSalesReports | `SalePrintRequests.Rpt376SalesSummaryByCustomerItemReferencePartyRequest` |
| 377-SalesSummaryByReferenceParty&Item.rpt | `/reports/print/377-sales-summary-by-reference-party-item` | SpInventory_EvaulationDetailSalesReports | `SalePrintRequests.Rpt377SalesSummaryByReferencePartyItemRequest` |
| 378-SalesSummaryByCustomer&ReferenceParty.rpt | `/reports/print/378-sales-summary-by-customer-reference-party` | SpInventory_EvaulationDetailSalesReports | `SalePrintRequests.Rpt378SalesSummaryByCustomerReferencePartyRequest` |
| 379-SalesSummaryByReferenceParty,Item&PackSize.rpt | `/reports/print/379-sales-summary-by-reference-party-item-pack-size` | SpInventory_EvaulationDetailSalesReports | `SalePrintRequests.Rpt379SalesSummaryByReferencePartyItemPackSizeRequest` |
| 397-SaleSummaryByHsCode.rpt | `/reports/print/397-sale-summary-by-hs-code` | SpInventory_EvaulationDetailSalesReports | `SalePrintRequests.Rpt397SaleSummaryByHsCodeRequest` |
| 460-InvRptDeliverySchedule.rpt | `/reports/print/460-delivery-schedule` | Sp_DeliverySchedule_Rpt | `SalePrintRequests.Rpt460DeliveryScheduleRequest` |
| 7862-PendingOutwardGatePassRegister.rpt | `/reports/print/7862-pending-outward-gate-pass-register` | Sp_GatePassOutward_PendingGatePass | `SalePrintRequests.Rpt7862PendingOutwardGatePassRegisterRequest` |
| 7864-PendingGdnRegister.rpt | `/reports/print/7864-pending-gdn-register` | Sp_InvGdn_PendingGdn | `SalePrintRequests.Rpt7864PendingGdnRegisterRequest` |
| 910-AdvanceDeliveryOrderRegister.rpt | `/reports/print/910-advance-delivery-order-register` | usp_AdvanceDeliveryOrderRegister | `SalePrintRequests.Rpt910AdvanceDeliveryOrderRegisterRequest` |
| 910_01_PendingReservedSaleInvoiceForAdvanceDeliveryOrder.rpt | `/reports/print/910-01-pending-reserved-sale-invoice-for-advance-delivery-order` | [dbo].[usp_PendingReservedSaleInvoiceForAdvanceDeliveryOrder] | `SalePrintRequests.Rpt91001PendingReservedSaleInvoiceForAdvanceDeliveryOrderRequest` |
| InvSaleInvoiceCustomerBillRiceJournalExpSubRep.rpt | `/reports/print/inv-sale-invoice-customer-bill-rice-journal-exp-sub-rep` | Sp_InvSaleInvoice_CustomerBillRice_JournalExp_SubRep | `SalePrintRequests.RptInvSaleInvoiceCustomerBillRiceJournalExpSubRepRequest` |
| RptGatepassOutwardDriverInfoSlipA.rpt | `/reports/print/gatepass-outward-driver-info-slip-a` | [dbo].[USP_driverBiodata_SlipAndRegister] | `SalePrintRequests.RptGatepassOutwardDriverInfoSlipARequest` |
| RptGdnSlip.rpt | `/reports/print/gdn-slip` | Sp_GatePassOutward_GetAllMethod | `SalePrintRequests.RptGdnSlipRequest` |
| SaleInvoiceItemExpense_SubReport.rpt | `/reports/print/sale-invoice-item-expense-sub-report-2` | [dbo].[USP_InvSaleInvoiceItemExpense_SubReport] | `SalePrintRequests.RptSaleInvoiceItemExpenseSubReportd0cd7874Request` |
| SaleOrderCustomerExpense_SubReport.rpt | `/reports/print/sale-order-customer-expense-sub-report` | [dbo].[USP_SaleOrderCustomerExpense_SubReport] | `SalePrintRequests.RptSaleOrderCustomerExpenseSubReportRequest` |
| SaleOrderExtraItemsDetail_SubReport.rpt | `/reports/print/sale-order-extra-items-detail-sub-report` | [dbo].[USP_SaleOrderExtraItemsDetail_SubReport] | `SalePrintRequests.RptSaleOrderExtraItemsDetailSubReportRequest` |

## Salt

| Template | GET / POST endpoint | Procedure | Request |
|---|---|---|---|
| 1818-ProductionSalt-Slip.rpt | `/reports/print/1818-production-salt-slip` | USP_ProductionSaltSlipAndRegister | `SaltPrintRequests.Rpt1818ProductionSaltSlipRequest` |
| 1818-ProductionSalt_Register.rpt | `/reports/print/1818-production-salt-register` | USP_ProductionSaltSlipAndRegister | `SaltPrintRequests.Rpt1818ProductionSaltRegisterRequest` |

## Steel

| Template | GET / POST endpoint | Procedure | Request |
|---|---|---|---|
| 1500-PurchaseOrderSlip_Rpt.rpt | `/reports/print/1500-purchase-order-slip` | [ST].[USP_PurchaseOrderSlip_Rpt] | `SteelPrintRequests.Rpt1500PurchaseOrderSlipRequest` |
| 1501-PurchaseOrder_Register.rpt | `/reports/print/1501-purchase-order-register` | [ST].[USP_PurchaseOrder_Register] | `SteelPrintRequests.Rpt1501PurchaseOrderRegisterRequest` |
| 1502-PurchaseOrder_Register.rpt | `/reports/print/1502-purchase-order-register` | [ST].[USP_PurchaseOrder_Register] | `SteelPrintRequests.Rpt1502PurchaseOrderRegisterRequest` |
| 1503-InvGrnSlip.rpt | `/reports/print/1503-inv-grn-slip` | [ST].[USp_InvGrnSlipAndRegister] | `SteelPrintRequests.Rpt1503InvGrnSlipRequest` |
| 1504-InvGrnSlipAndRegister.rpt | `/reports/print/1504-inv-grn-slip-and-register` | [ST].[USp_InvGrnSlipAndRegister] | `SteelPrintRequests.Rpt1504InvGrnSlipAndRegisterRequest` |
| 1505-InvPurchaseInvoiceRegister.rpt | `/reports/print/1505-inv-purchase-invoice-register` | [ST].[USP_InvPurchaseInvoiceRegister] | `SteelPrintRequests.Rpt1505InvPurchaseInvoiceRegisterRequest` |
| 1506-PurchaseInvoiceRegisterAvgRateItemSupplier.rpt | `/reports/print/1506-purchase-invoice-register-avg-rate-item-supplier` | [ST].[USP_InvPurchaseInvoiceRegister_AvgRatesComparisonsByItemSupplier] | `SteelPrintRequests.Rpt1506PurchaseInvoiceRegisterAvgRateItemSupplierRequest` |
| 1507-PurchaseInvoiceRegisterAvgRateByItem.rpt | `/reports/print/1507-purchase-invoice-register-avg-rate-by-item` | [ST].[Sp_InvPurchaseInvoiceRegister_AvgRatesComparisonsByItem] | `SteelPrintRequests.Rpt1507PurchaseInvoiceRegisterAvgRateByItemRequest` |
| 1508-PurchaseBillSupplierRiceItemSlip.rpt | `/reports/print/1508-purchase-bill-supplier-rice-item-slip` | [ST].[USP_InvPurchaseInvoice_PartySlip] | `SteelPrintRequests.Rpt1508PurchaseBillSupplierRiceItemSlipRequest` |
| 1508A-InvPurchaseInvoicePartySlip.rpt | `/reports/print/1508a-inv-purchase-invoice-party-slip` | [ST].[USP_InvPurchaseInvoice_PartySlip] | `SteelPrintRequests.Rpt1508AInvPurchaseInvoicePartySlipRequest` |
| 1509-SaleOrderSlipAndRegister.rpt | `/reports/print/1509-sale-order-slip-and-register` | [ST].[USP_SaleOrderSlipAndRegister] | `SteelPrintRequests.Rpt1509SaleOrderSlipAndRegisterRequest` |
| 1510-PurchaseInvoiceDirectPartySlip.rpt | `/reports/print/1510-purchase-invoice-direct-party-slip` | [ST].[USP_InvPurchaseInvoiceDirect_PartySlip] | `SteelPrintRequests.Rpt1510PurchaseInvoiceDirectPartySlipRequest` |
| 1510A-PurchaseInvoiceDirectPartySlip.rpt | `/reports/print/1510a-purchase-invoice-direct-party-slip` | [ST].[USP_InvPurchaseInvoiceDirect_PartySlip] | `SteelPrintRequests.Rpt1510APurchaseInvoiceDirectPartySlipRequest` |
| 1511-SaleOrderSlipAndRegister.rpt | `/reports/print/1511-sale-order-slip-and-register` | [ST].[USP_SaleOrderSlipAndRegister] | `SteelPrintRequests.Rpt1511SaleOrderSlipAndRegisterRequest` |
| 1512-FoodProductionSlip.rpt | `/reports/print/1512-food-production-slip` | [ST].[USP_FoodProduction_Slip] | `SteelPrintRequests.Rpt1512FoodProductionSlipRequest` |
| 1512-GatePassOutwardSlipAndRegisterSteel.rpt | `/reports/print/1512-gate-pass-outward-slip-and-register-steel` | [ST].[USp_GatePassOutward_SlipAndRegisterSteel_Rpt] | `SteelPrintRequests.Rpt1512GatePassOutwardSlipAndRegisterSteelRequest` |
| 1513-DeliveryOrderSlip.rpt | `/reports/print/1513-delivery-order-slip` | [ST].[USP_DeliveryOrder_SlipReport] | `SteelPrintRequests.Rpt1513DeliveryOrderSlipRequest` |
| 1513-FoodProductionSummery_WithOutValue.rpt | `/reports/print/1513-food-production-summery-with-out-value` | [ST].[USP_FoodProductionSummery_WithOutValue] | `SteelPrintRequests.Rpt1513FoodProductionSummeryWithOutValueRequest` |
| 1513A-DeliveryOrderSlip&Register.rpt | `/reports/print/1513a-delivery-order-slip-register` | [ST].[USp_DeliveryOrderRegister] | `SteelPrintRequests.Rpt1513ADeliveryOrderSlipRegisterRequest` |
| 1513A_ProductionSummeryWithValues.rpt | `/reports/print/1513a-production-summery-with-values` | [ST].[USP_FoodProduction_Summery_WithValues] | `SteelPrintRequests.Rpt1513AProductionSummeryWithValuesRequest` |
| 1514-FoodProductionOverHeadJobOrderWise_Slip.rpt | `/reports/print/1514-food-production-over-head-job-order-wise-slip` | [ST].[USP_FoodProductionOverHeadJobOrderWise_Slip] | `SteelPrintRequests.Rpt1514FoodProductionOverHeadJobOrderWiseSlipRequest` |
| 1514-InvRepSaleBillCustomer.rpt | `/reports/print/1514-inv-rep-sale-bill-customer` | [ST].[USP_InvSaleInvoice_CustomerBill] | `SteelPrintRequests.Rpt1514InvRepSaleBillCustomerRequest` |
| 1514A-SaleBillCustomer-Format-II.rpt | `/reports/print/1514a-sale-bill-customer-format-ii` | [ST].[USP_InvSaleInvoice_CustomerBill] | `SteelPrintRequests.Rpt1514ASaleBillCustomerFormatIIRequest` |
| 1515-InvRptGdnSlipAndRegister.rpt | `/reports/print/1515-gdn-slip-and-register` | [ST].[USP_InvGdn_SlipAndRegisterReport] | `SteelPrintRequests.Rpt1515GdnSlipAndRegisterRequest` |
| 1515A-InvRptGdnSlipDliveryChallan.rpt | `/reports/print/1515a-gdn-slip-dlivery-challan` | [ST].[USP_InvGdn_SlipAndRegisterReport] | `SteelPrintRequests.Rpt1515AGdnSlipDliveryChallanRequest` |
| 1516-SaleInvoiceCustomerBill.rpt | `/reports/print/1516-sale-invoice-customer-bill` | [ST].[USP_InvSaleInvoice_CustomerDirectBill] | `SteelPrintRequests.Rpt1516SaleInvoiceCustomerBillRequest` |
| 1516A-SaleBillDirectSupplierBill.rpt | `/reports/print/1516a-sale-bill-direct-supplier-bill` | [ST].[USP_InvSaleInvoice_CustomerDirectBill] | `SteelPrintRequests.Rpt1516ASaleBillDirectSupplierBillRequest` |
| 1517-DeliveryOrderRegister.rpt | `/reports/print/1517-delivery-order-register` | [ST].[USp_DeliveryOrderRegister] | `SteelPrintRequests.Rpt1517DeliveryOrderRegisterRequest` |
| 1520-InvRptGatePassOutwardRegister.rpt | `/reports/print/1520-gate-pass-outward-register` | [ST].[USp_GatePassOutward_SlipAndRegisterSteel_Rpt] | `SteelPrintRequests.Rpt1520GatePassOutwardRegisterRequest` |
| 1521-InvGdn_SlipAndRegisterReport.rpt | `/reports/print/1521-inv-gdn-slip-and-register-report` | [ST].[USP_InvGdn_SlipAndRegisterReport] | `SteelPrintRequests.Rpt1521InvGdnSlipAndRegisterReportRequest` |
| 1522-SalesRegisterSummary.rpt | `/reports/print/1522-sales-register-summary` | [ST].[USP-SaleInvoiceRegisterWithActivities] | `SteelPrintRequests.Rpt1522SalesRegisterSummaryRequest` |
| 1523-SalesRegisterSummaryByCustomer.rpt | `/reports/print/1523-sales-register-summary-by-customer` | [ST].[USP-SaleInvoiceRegisterWithActivities] | `SteelPrintRequests.Rpt1523SalesRegisterSummaryByCustomerRequest` |
| 1524-SalesRegisterSummaryByItem.rpt | `/reports/print/1524-sales-register-summary-by-item` | [ST].[USP-SaleInvoiceRegisterWithActivities] | `SteelPrintRequests.Rpt1524SalesRegisterSummaryByItemRequest` |
| 1524_04-PurchaseRegisterSummaryByItem&PackSize.rpt | `/reports/print/1524-04-purchase-register-summary-by-item-pack-size` | [ST].[USP-PurchaseInvoiceRegisterWithActivities] | `SteelPrintRequests.Rpt152404PurchaseRegisterSummaryByItemPackSizeRequest` |
| 1525-SalesRegisterSummaryByCustomer&Item.rpt | `/reports/print/1525-sales-register-summary-by-customer-item` | [ST].[USP-SaleInvoiceRegisterWithActivities] | `SteelPrintRequests.Rpt1525SalesRegisterSummaryByCustomerItemRequest` |
| 1526-SalesRegisterSummaryByItemWithoutPacking.rpt | `/reports/print/1526-sales-register-summary-by-item-without-packing` | [ST].[USP-SaleInvoiceRegisterWithActivities] | `SteelPrintRequests.Rpt1526SalesRegisterSummaryByItemWithoutPackingRequest` |
| 1527-SalesRegisterSummaryByWarehouse.rpt | `/reports/print/1527-sales-register-summary-by-warehouse` | [ST].[USP-SaleInvoiceRegisterWithActivities] | `SteelPrintRequests.Rpt1527SalesRegisterSummaryByWarehouseRequest` |
| 1528-PurchaseRegisterSummary.rpt | `/reports/print/1528-purchase-register-summary` | [ST].[USP-PurchaseInvoiceRegisterWithActivities] | `SteelPrintRequests.Rpt1528PurchaseRegisterSummaryRequest` |
| 1528-SalesSummaryByItem&City.rpt | `/reports/print/1528-sales-summary-by-item-city` | [ST].[USP-SaleInvoiceRegisterWithActivities] | `SteelPrintRequests.Rpt1528SalesSummaryByItemCityRequest` |
| 1528_01-PurchaseRegisterSummaryBySupplier.rpt | `/reports/print/1528-01-purchase-register-summary-by-supplier` | [ST].[USP-PurchaseInvoiceRegisterWithActivities] | `SteelPrintRequests.Rpt152801PurchaseRegisterSummaryBySupplierRequest` |
| 1528_02-PurchaseRegisterSummaryByItem.rpt | `/reports/print/1528-02-purchase-register-summary-by-item` | [ST].[USP-PurchaseInvoiceRegisterWithActivities] | `SteelPrintRequests.Rpt152802PurchaseRegisterSummaryByItemRequest` |
| 1528_03-PurchaseRegisterSummaryByItem&City.rpt | `/reports/print/1528-03-purchase-register-summary-by-item-city` | [ST].[USP-PurchaseInvoiceRegisterWithActivities] | `SteelPrintRequests.Rpt152803PurchaseRegisterSummaryByItemCityRequest` |
| 1528_05-SalesRegisterSummaryByItem&Warehouse.rpt | `/reports/print/1528-05-sales-register-summary-by-item-warehouse` | [ST].[USP-PurchaseInvoiceRegisterWithActivities] | `SteelPrintRequests.Rpt152805SalesRegisterSummaryByItemWarehouseRequest` |
| 1528_06-PurchaseSummaryByItemPackSize&City.rpt | `/reports/print/1528-06-purchase-summary-by-item-pack-size-city` | [ST].[USP-PurchaseInvoiceRegisterWithActivities] | `SteelPrintRequests.Rpt152806PurchaseSummaryByItemPackSizeCityRequest` |
| 1528_07-PurchaseRegisterSummaryBySupplier&Item.rpt | `/reports/print/1528-07-purchase-register-summary-by-supplier-item` | [ST].[USP-PurchaseInvoiceRegisterWithActivities] | `SteelPrintRequests.Rpt152807PurchaseRegisterSummaryBySupplierItemRequest` |
| 1528_08-PurchaseSummaryBySupplier&City.rpt | `/reports/print/1528-08-purchase-summary-by-supplier-city` | [ST].[USP-PurchaseInvoiceRegisterWithActivities] | `SteelPrintRequests.Rpt152808PurchaseSummaryBySupplierCityRequest` |
| 1528_09-PurchaseSummaryBySupplierItem&City.rpt | `/reports/print/1528-09-purchase-summary-by-supplier-item-city` | [ST].[USP-PurchaseInvoiceRegisterWithActivities] | `SteelPrintRequests.Rpt152809PurchaseSummaryBySupplierItemCityRequest` |
| 1528_10-PurchaseSummaryBySupplier&PackSize.rpt | `/reports/print/1528-10-purchase-summary-by-supplier-pack-size` | [ST].[USP-PurchaseInvoiceRegisterWithActivities] | `SteelPrintRequests.Rpt152810PurchaseSummaryBySupplierPackSizeRequest` |
| 1528_11-PurchaseSummaryByParentCategory.rpt | `/reports/print/1528-11-purchase-summary-by-parent-category` | [ST].[USP-PurchaseInvoiceRegisterWithActivities] | `SteelPrintRequests.Rpt152811PurchaseSummaryByParentCategoryRequest` |
| 1528_12-PurchaseSummaryByParentCategory&Item.rpt | `/reports/print/1528-12-purchase-summary-by-parent-category-item` | [ST].[USP-PurchaseInvoiceRegisterWithActivities] | `SteelPrintRequests.Rpt152812PurchaseSummaryByParentCategoryItemRequest` |
| 1528_13-PurchaseSummaryByParentCategory&Supplier.rpt | `/reports/print/1528-13-purchase-summary-by-parent-category-supplier` | [ST].[USP-PurchaseInvoiceRegisterWithActivities] | `SteelPrintRequests.Rpt152813PurchaseSummaryByParentCategorySupplierRequest` |
| 1529-SalesSummaryByCustomer&City.rpt | `/reports/print/1529-sales-summary-by-customer-city` | [ST].[USP-SaleInvoiceRegisterWithActivities] | `SteelPrintRequests.Rpt1529SalesSummaryByCustomerCityRequest` |
| 1530-SalesSummaryByItemPackSize&City.rpt | `/reports/print/1530-sales-summary-by-item-pack-size-city` | [ST].[USP-SaleInvoiceRegisterWithActivities] | `SteelPrintRequests.Rpt1530SalesSummaryByItemPackSizeCityRequest` |
| 1531-SalesSummaryByCustomerItem&City.rpt | `/reports/print/1531-sales-summary-by-customer-item-city` | [ST].[USP-SaleInvoiceRegisterWithActivities] | `SteelPrintRequests.Rpt1531SalesSummaryByCustomerItemCityRequest` |
| 1532-SalesSummaryByCustomer&PackSize.rpt | `/reports/print/1532-sales-summary-by-customer-pack-size` | [ST].[USP-SaleInvoiceRegisterWithActivities] | `SteelPrintRequests.Rpt1532SalesSummaryByCustomerPackSizeRequest` |
| 1533-SalesSummaryByParentCategory.rpt | `/reports/print/1533-sales-summary-by-parent-category` | [ST].[USP-SaleInvoiceRegisterWithActivities] | `SteelPrintRequests.Rpt1533SalesSummaryByParentCategoryRequest` |
| 1534-SalesSummaryByParentCategory&Item.rpt | `/reports/print/1534-sales-summary-by-parent-category-item` | [ST].[USP-SaleInvoiceRegisterWithActivities] | `SteelPrintRequests.Rpt1534SalesSummaryByParentCategoryItemRequest` |
| 1535-SalesSummaryByParentCategory&Customer.rpt | `/reports/print/1535-sales-summary-by-parent-category-customer` | [ST].[USP-SaleInvoiceRegisterWithActivities] | `SteelPrintRequests.Rpt1535SalesSummaryByParentCategoryCustomerRequest` |
| 1542-ItemStockSummary.rpt | `/reports/print/1542-item-stock-summary` | [ST].[USP-StockSummaryByQtyAndWeightReport] | `SteelPrintRequests.Rpt1542ItemStockSummaryRequest` |
| 1543-ItemandWarehouseStockSummary.rpt | `/reports/print/1543-itemand-warehouse-stock-summary` | [ST].[USP-StockSummaryByQtyAndWeightReport] | `SteelPrintRequests.Rpt1543ItemandWarehouseStockSummaryRequest` |
| 1544-WarehouseAndItemStockSummary.rpt | `/reports/print/1544-warehouse-and-item-stock-summary` | [ST].[USP-StockSummaryByQtyAndWeightReport] | `SteelPrintRequests.Rpt1544WarehouseAndItemStockSummaryRequest` |
| 1545-WarehouseandJoblotandItemStockSummary.rpt | `/reports/print/1545-warehouseand-joblotand-item-stock-summary` | [ST].[USP-StockSummaryByQtyAndWeightReport] | `SteelPrintRequests.Rpt1545WarehouseandJoblotandItemStockSummaryRequest` |
| 1546-ItemandPackSizeStockSummary.rpt | `/reports/print/1546-itemand-pack-size-stock-summary` | [ST].[USP-StockSummaryByQtyAndWeightReport] | `SteelPrintRequests.Rpt1546ItemandPackSizeStockSummaryRequest` |
| 1547-JobLotandItemStockSummary.rpt | `/reports/print/1547-job-lotand-item-stock-summary` | [ST].[USP-StockSummaryByQtyAndWeightReport] | `SteelPrintRequests.Rpt1547JobLotandItemStockSummaryRequest` |
| 1548-ItemandPackingTypeStockSummary.rpt | `/reports/print/1548-itemand-packing-type-stock-summary` | [ST].[USP-StockSummaryByQtyAndWeightReport] | `SteelPrintRequests.Rpt1548ItemandPackingTypeStockSummaryRequest` |
| 1549-ItemandPackSizeandPackingTypeStockSummary.rpt | `/reports/print/1549-itemand-pack-sizeand-packing-type-stock-summary` | [ST].[USP-StockSummaryByQtyAndWeightReport] | `SteelPrintRequests.Rpt1549ItemandPackSizeandPackingTypeStockSummaryRequest` |
| 1550-ItemStockSummary.rpt | `/reports/print/1550-item-stock-summary` | [ST].[USP_StockSummaryWithValuesReport] | `SteelPrintRequests.Rpt1550ItemStockSummaryRequest` |
| 1551-ItemandWarehouseStockSummary.rpt | `/reports/print/1551-itemand-warehouse-stock-summary` | [ST].[USP_StockSummaryWithValuesReport] | `SteelPrintRequests.Rpt1551ItemandWarehouseStockSummaryRequest` |
| 1552-WarehouseAndItemStockSummary.rpt | `/reports/print/1552-warehouse-and-item-stock-summary` | [ST].[USP_StockSummaryWithValuesReport] | `SteelPrintRequests.Rpt1552WarehouseAndItemStockSummaryRequest` |
| 1553-JobLotandItemStockSummary.rpt | `/reports/print/1553-job-lotand-item-stock-summary` | [ST].[USP_StockSummaryWithValuesReport] | `SteelPrintRequests.Rpt1553JobLotandItemStockSummaryRequest` |
| 1554-WarehouseandJoblotandItemStockSummary.rpt | `/reports/print/1554-warehouseand-joblotand-item-stock-summary` | [ST].[USP_StockSummaryWithValuesReport] | `SteelPrintRequests.Rpt1554WarehouseandJoblotandItemStockSummaryRequest` |
| 1555-ItemandPackSizeStockSummary.rpt | `/reports/print/1555-itemand-pack-size-stock-summary` | [ST].[USP_StockSummaryWithValuesReport] | `SteelPrintRequests.Rpt1555ItemandPackSizeStockSummaryRequest` |
| 1556-ItemandPackingTypeStockSummary.rpt | `/reports/print/1556-itemand-packing-type-stock-summary` | [ST].[USP_StockSummaryWithValuesReport] | `SteelPrintRequests.Rpt1556ItemandPackingTypeStockSummaryRequest` |
| 1557-ItemandPackSizeandPackingTypeStockSummary.rpt | `/reports/print/1557-itemand-pack-sizeand-packing-type-stock-summary` | [ST].[USP_StockSummaryWithValuesReport] | `SteelPrintRequests.Rpt1557ItemandPackSizeandPackingTypeStockSummaryRequest` |
| 1558-InvStockRptInventoryTransactionsA.rpt | `/reports/print/1558-inv-stock-inventory-transactions-a` | Sp_InventoryTransactions_GenerateTransactionsLedgerStocks | `SteelPrintRequests.Rpt1558InvStockInventoryTransactionsARequest` |
| 1559-InvStockRptInventoryTransactionsStocks.rpt | `/reports/print/1559-inv-stock-inventory-transactions-stocks` | [ST].[USP-InventoryTransactionsReport] | `SteelPrintRequests.Rpt1559InvStockInventoryTransactionsStocksRequest` |
| 1563-ProductionJobOrderSlip.rpt | `/reports/print/1563-production-job-order-slip` | [ST].[USP_ProductionJobOrder_REPORT] | `SteelPrintRequests.Rpt1563ProductionJobOrderSlipRequest` |
| 1853-DeliveryOrderRegister.rpt | `/reports/print/1853-delivery-order-register` | [ST].[USp_DeliveryOrderRegister] | `SteelPrintRequests.Rpt1853DeliveryOrderRegisterRequest` |
| 1853A-DeliveryOrderRegister.rpt | `/reports/print/1853a-delivery-order-register` | [ST].[USp_DeliveryOrderRegister] | `SteelPrintRequests.Rpt1853ADeliveryOrderRegisterRequest` |
| 630-PreCostProductionJobOrderSlip.rpt | `/reports/print/630-pre-cost-production-job-order-slip` | [ST].[USP_ProductionJobOrder_REPORT] | `SteelPrintRequests.Rpt630PreCostProductionJobOrderSlipRequest` |
| 7861-PendingInwardGatePassRegister.rpt | `/reports/print/7861-pending-inward-gate-pass-register` | Sp_InvGrn_GetPendingGrn | `SteelPrintRequests.Rpt7861PendingInwardGatePassRegisterRequest` |
| 7863-PendingGrnRegister.rpt | `/reports/print/7863-pending-grn-register` | Sp_InvGrn_GetPendingGrn | `SteelPrintRequests.Rpt7863PendingGrnRegisterRequest` |
| ProductionJobOrderInputSubReport.rpt | `/reports/print/production-job-order-input-sub-report` | [ST].[USP_ProductionJobOrderInput_REPORT] | `SteelPrintRequests.RptProductionJobOrderInputSubReportRequest` |
| ProductionJobOrderOutputSubReport.rpt | `/reports/print/production-job-order-output-sub-report` | [ST].[USP_ProductionJobOrderOutput_REPORT] | `SteelPrintRequests.RptProductionJobOrderOutputSubReportRequest` |
| ProductionJobOrderOverHeadsSubReport.rpt | `/reports/print/production-job-order-over-heads-sub-report` | [ST].[USP_ProductionJobOrderOverHeads_REPORT] | `SteelPrintRequests.RptProductionJobOrderOverHeadsSubReportRequest` |
| ProductionJobOrderPackingMaterialsSubReport.rpt | `/reports/print/production-job-order-packing-materials-sub-report` | USP_InvProductionJobOrderPackingMaterial_Report | `SteelPrintRequests.RptProductionJobOrderPackingMaterialsSubReportRequest` |
| PurchaseInvoiceItemOthersAddLessSubReport.rpt | `/reports/print/purchase-invoice-item-others-add-less-sub-report` | [ST].[USP_PurchaseInvoice_ItemOthersAddLess_SubReport] | `SteelPrintRequests.RptPurchaseInvoiceItemOthersAddLessSubReportRequest` |
| PurchaseInvoicePartyBillOthersAddLessSubReport.rpt | `/reports/print/purchase-invoice-party-bill-others-add-less-sub-report` | [ST].[USP_PurchaseInvoice_PartyBillOthersAddLess_SubReport] | `SteelPrintRequests.RptPurchaseInvoicePartyBillOthersAddLessSubReportRequest` |

## Stocks

| Template | GET / POST endpoint | Procedure | Request |
|---|---|---|---|
| 416-InvStockOpeningBalanceHeader_Register.rpt | `/reports/print/416-inv-stock-opening-balance-header-register` | Sp_InvStockOpeningBalanceHeader_RegisterRpt | `StocksPrintRequests.Rpt416InvStockOpeningBalanceHeaderRegisterRequest` |
| 202_ItemStockSummary.rpt | `/reports/print/202-item-stock-summary` | Sp_ItemStockReportWithValues_Store | `StocksPrintRequests.Rpt202ItemStockSummaryRequest` |
| 202_02_ItemandWarehouseStockSummary.rpt | `/reports/print/202-02-itemand-warehouse-stock-summary` | Sp_ItemStockReportWithValues_Store | `StocksPrintRequests.Rpt20202ItemandWarehouseStockSummaryRequest` |
| 202_03_ItemandPackSizeStockSummary.rpt | `/reports/print/202-03-itemand-pack-size-stock-summary` | Sp_ItemStockReportWithValues_Store | `StocksPrintRequests.Rpt20203ItemandPackSizeStockSummaryRequest` |
| 621-StockEvalautionDetailTransactionVehiclesWise.rpt | `/reports/print/621-stock-evalaution-detail-transaction-vehicles-wise` | USP_GetStockEvalautionDetailByRefRefIds | `StocksPrintRequests.Rpt621StockEvalautionDetailTransactionVehiclesWiseRequest` |
| 1560-StockRptInventoryTransactionsNew.rpt | `/reports/print/1560-stock-inventory-transactions-new` | USP_InventoryEvaluationItemLedger_Rpt | `StocksPrintRequests.Rpt1560StockInventoryTransactionsNewRequest` |
| 1561-InvStockRptInventoryTransactionsNew.rpt | `/reports/print/1561-inv-stock-inventory-transactions-new` | USP_InventoryEvaluationItemLedger_Rpt | `StocksPrintRequests.Rpt1561InvStockInventoryTransactionsNewRequest` |
| 160-WagesRegister.rpt | `/reports/print/160-wages-register` | USp_WagesRegister | `StocksPrintRequests.Rpt160WagesRegisterRequest` |
| 161-WagesbyContractor&DocumentType.rpt | `/reports/print/161-wagesby-contractor-document-type` | USp_WagesRegister | `StocksPrintRequests.Rpt161WagesbyContractorDocumentTypeRequest` |
| 162-WagesByContractor.rpt | `/reports/print/162-wages-by-contractor` | USp_WagesRegister | `StocksPrintRequests.Rpt162WagesByContractorRequest` |
| 163-WagesByDocumentType.rpt | `/reports/print/163-wages-by-document-type` | USp_WagesRegister | `StocksPrintRequests.Rpt163WagesByDocumentTypeRequest` |
| 165-ItemLedgerRetail.rpt | `/reports/print/165-item-ledger-retail` | SpInventoryStockEvalautionDetail_RetailItemLedgerReport | `StocksPrintRequests.Rpt165ItemLedgerRetailRequest` |
| 178-ItemStockSummary.rpt | `/reports/print/178-item-stock-summary` | Sp_InventoryTransactions_StockSummaryGeneralByWeight_Rpt | `StocksPrintRequests.Rpt178ItemStockSummaryRequest` |
| 179-ItemandWarehouseStockSummary.rpt | `/reports/print/179-itemand-warehouse-stock-summary` | Sp_InventoryTransactions_StockSummaryGeneralByWeight_Rpt | `StocksPrintRequests.Rpt179ItemandWarehouseStockSummaryRequest` |
| 180-ItemandWarehouseStockSummary.rpt | `/reports/print/180-itemand-warehouse-stock-summary` | Sp_InventoryTransactions_StockSummaryGeneralByWeight_Rpt | `StocksPrintRequests.Rpt180ItemandWarehouseStockSummaryRequest` |
| 181-ItemandCropYearStockSummary.rpt | `/reports/print/181-itemand-crop-year-stock-summary` | Sp_InventoryTransactions_StockSummaryGeneralByWeight_Rpt | `StocksPrintRequests.Rpt181ItemandCropYearStockSummaryRequest` |
| 182-ItemandCropYearandWarehouseStockSummary.rpt | `/reports/print/182-itemand-crop-yearand-warehouse-stock-summary` | Sp_InventoryTransactions_StockSummaryGeneralByWeight_Rpt | `StocksPrintRequests.Rpt182ItemandCropYearandWarehouseStockSummaryRequest` |
| 183-JobLotandItemStockSummary.rpt | `/reports/print/183-job-lotand-item-stock-summary` | Sp_InventoryTransactions_StockSummaryGeneralByWeight_Rpt | `StocksPrintRequests.Rpt183JobLotandItemStockSummaryRequest` |
| 184-WarehouseandJoblotandItemStockSummary.rpt | `/reports/print/184-warehouseand-joblotand-item-stock-summary` | Sp_InventoryTransactions_StockSummaryGeneralByWeight_Rpt | `StocksPrintRequests.Rpt184WarehouseandJoblotandItemStockSummaryRequest` |
| 185-ItemandPackSizeStockSummary.rpt | `/reports/print/185-itemand-pack-size-stock-summary` | Sp_InventoryTransactions_StockSummaryGeneralByWeight_Rpt | `StocksPrintRequests.Rpt185ItemandPackSizeStockSummaryRequest` |
| 186-ItemandPackingTypeStockSummary.rpt | `/reports/print/186-itemand-packing-type-stock-summary` | Sp_InventoryTransactions_StockSummaryGeneralByWeight_Rpt | `StocksPrintRequests.Rpt186ItemandPackingTypeStockSummaryRequest` |
| 187-ItemandPackSizeandPackingTypeStockSummary.rpt | `/reports/print/187-itemand-pack-sizeand-packing-type-stock-summary` | Sp_InventoryTransactions_StockSummaryGeneralByWeight_Rpt | `StocksPrintRequests.Rpt187ItemandPackSizeandPackingTypeStockSummaryRequest` |
| 188-ItemStockSummary.rpt | `/reports/print/188-item-stock-summary` | Sp_ItemStockReportWithValues_Rpt | `StocksPrintRequests.Rpt188ItemStockSummaryRequest` |
| 188_01-ItemStockSummary.rpt | `/reports/print/188-01-item-stock-summary` | Sp_ItemStockReportWithValues_Rpt | `StocksPrintRequests.Rpt18801ItemStockSummaryRequest` |
| 188_02-ItemStockSummary.rpt | `/reports/print/188-02-item-stock-summary` | Sp_ItemStockReportWithValues_Rpt | `StocksPrintRequests.Rpt18802ItemStockSummaryRequest` |
| 189-ItemandWarehouseStockSummary.rpt | `/reports/print/189-itemand-warehouse-stock-summary` | Sp_ItemStockReportWithValues_Rpt | `StocksPrintRequests.Rpt189ItemandWarehouseStockSummaryRequest` |
| 190-ItemandWarehouseStockSummary.rpt | `/reports/print/190-itemand-warehouse-stock-summary` | Sp_ItemStockReportWithValues_Rpt | `StocksPrintRequests.Rpt190ItemandWarehouseStockSummaryRequest` |
| 191-ItemandCropYearStockSummary.rpt | `/reports/print/191-itemand-crop-year-stock-summary` | Sp_ItemStockReportWithValues_Rpt | `StocksPrintRequests.Rpt191ItemandCropYearStockSummaryRequest` |
| 192-ItemandCropYearandWarehouseStockSummary.rpt | `/reports/print/192-itemand-crop-yearand-warehouse-stock-summary` | Sp_ItemStockReportWithValues_Rpt | `StocksPrintRequests.Rpt192ItemandCropYearandWarehouseStockSummaryRequest` |
| 193-JobLotandItemStockSummary.rpt | `/reports/print/193-job-lotand-item-stock-summary` | Sp_ItemStockReportWithValues_Rpt | `StocksPrintRequests.Rpt193JobLotandItemStockSummaryRequest` |
| 194-WarehouseandJoblotandItemStockSummary.rpt | `/reports/print/194-warehouseand-joblotand-item-stock-summary` | Sp_ItemStockReportWithValues_Rpt | `StocksPrintRequests.Rpt194WarehouseandJoblotandItemStockSummaryRequest` |
| 194A-WarehouseandItemandJoblotStockSummary.rpt | `/reports/print/194a-warehouseand-itemand-joblot-stock-summary` | Sp_ItemStockReportWithValues_Rpt | `StocksPrintRequests.Rpt194AWarehouseandItemandJoblotStockSummaryRequest` |
| 195-ItemandPackSizeStockSummary.rpt | `/reports/print/195-itemand-pack-size-stock-summary` | Sp_ItemStockReportWithValues_Rpt | `StocksPrintRequests.Rpt195ItemandPackSizeStockSummaryRequest` |
| 196-ItemandPackingTypeStockSummary.rpt | `/reports/print/196-itemand-packing-type-stock-summary` | Sp_ItemStockReportWithValues_Rpt | `StocksPrintRequests.Rpt196ItemandPackingTypeStockSummaryRequest` |
| 197-ItemandPackSizeandPackingTypeStockSummary.rpt | `/reports/print/197-itemand-pack-sizeand-packing-type-stock-summary` | Sp_ItemStockReportWithValues_Rpt | `StocksPrintRequests.Rpt197ItemandPackSizeandPackingTypeStockSummaryRequest` |
| 198-WIPStockPlantWiseSummary.rpt | `/reports/print/198-wip-stock-plant-wise-summary` | Sp_InventoryTransactions_StockSummaryGeneralByWeight_Rpt | `StocksPrintRequests.Rpt198WIPStockPlantWiseSummaryRequest` |
| 225_StockReservedRegister.rpt | `/reports/print/225-stock-reserved-register` | [dbo].[USP_InventoryStockReserved_SlipAndRegister] | `StocksPrintRequests.Rpt225StockReservedRegisterRequest` |
| 287-CurrentStockReportWithAvgRates.rpt | `/reports/print/287-current-stock-report-with-avg-rates` | SP_GetCurrentStockReportWithAvgRates | `StocksPrintRequests.Rpt287CurrentStockReportWithAvgRatesRequest` |
| 300-StockMonthlyClosingSlip.rpt | `/reports/print/300-stock-monthly-closing-slip` | usp_MonthlyClosingStock_Slip | `StocksPrintRequests.Rpt300StockMonthlyClosingSlipRequest` |
| 390_01_GetStockByFifo.rpt | `/reports/print/390-01-get-stock-by-fifo` | USP_GetStockByFifo_Report | `StocksPrintRequests.Rpt39001GetStockByFifoRequest` |
| 390_GetFIFODataForAudit.rpt | `/reports/print/390-get-fifo-data-for-audit` | USP_GetFIFODataForAudit | `StocksPrintRequests.Rpt390GetFIFODataForAuditRequest` |
| 401-StockEvalautionDetail_GenerateStocks_Register.rpt | `/reports/print/401-stock-evalaution-detail-generate-stocks-register` | Sp_InventoryStockEvalautionDetail_GenerateStocks | `StocksPrintRequests.Rpt401StockEvalautionDetailGenerateStocksRegisterRequest` |
| 403-InvStockRptInventoryTransactionsStocks.rpt | `/reports/print/403-inv-stock-inventory-transactions-stocks` | SpInventoryTransactions_History_Report | `StocksPrintRequests.Rpt403InvStockInventoryTransactionsStocksRequest` |
| 404-RptEBGLBySupplierandItem.rpt | `/reports/print/404-ebgl-by-supplierand-item` | Sp_InventoryEBGLBySupplierandItem_Rpt | `StocksPrintRequests.Rpt404EBGLBySupplierandItemRequest` |
| 408-StockTransferRegister.rpt | `/reports/print/408-stock-transfer-register` | Sp_InvStockTransfer_SlipandRegister | `StocksPrintRequests.Rpt408StockTransferRegisterRequest` |
| 410-InvStockAdjustmentRegister.rpt | `/reports/print/410-inv-stock-adjustment-register` | Sp_StockAdjustmentSlipAndRegister | `StocksPrintRequests.Rpt410InvStockAdjustmentRegisterRequest` |
| 411-InvStockRptInventoryTransactionsNew.rpt | `/reports/print/411-inv-stock-inventory-transactions-new` | USP_InventoryEvaluationItemLedger_Rpt | `StocksPrintRequests.Rpt411InvStockInventoryTransactionsNewRequest` |
| 412-StockRptInventoryTransactionsNew.rpt | `/reports/print/412-stock-inventory-transactions-new` | USP_InventoryEvaluationItemLedger_Rpt | `StocksPrintRequests.Rpt412StockInventoryTransactionsNewRequest` |
| 416_01_StockOpeningBalanceHeader_Register.rpt | `/reports/print/416-01-stock-opening-balance-header-register` | Sp_InvStockOpeningBalanceHeader_RegisterRpt | `StocksPrintRequests.Rpt41601StockOpeningBalanceHeaderRegisterRequest` |
| 421_01_ItemEvaluationLedger.rpt | `/reports/print/421-01-item-evaluation-ledger` | usp_ItemLedgerFromStockEvaluations | `StocksPrintRequests.Rpt42101ItemEvaluationLedgerRequest` |
| 421_02_ItemLedgerWithoutValue.rpt | `/reports/print/421-02-item-ledger-without-value` | usp_ItemLedgerFromStockTransactions | `StocksPrintRequests.Rpt42102ItemLedgerWithoutValueRequest` |
| 622-StockEvalaution_VehicleWiseTransaction.rpt | `/reports/print/622-stock-evalaution-vehicle-wise-transaction` | USP_StockEvalaution_VehicleWiseTransaction_Report | `StocksPrintRequests.Rpt622StockEvalautionVehicleWiseTransactionRequest` |
| 622A_StockEvalaution_VehicleWiseTransaction.rpt | `/reports/print/622a-stock-evalaution-vehicle-wise-transaction` | USP_StockEvalaution_VehicleWiseTransaction_Report | `StocksPrintRequests.Rpt622AStockEvalautionVehicleWiseTransactionRequest` |
| 842-StockBreakUpInOutSummary.rpt | `/reports/print/842-stock-break-up-in-out-summary` | usp_TotalStockBreakup_InAndOut_Summary | `StocksPrintRequests.Rpt842StockBreakUpInOutSummaryRequest` |
| 843-PaddyStockBreakUpItemWise.rpt | `/reports/print/843-paddy-stock-break-up-item-wise` | usp_TotalStockBreakup_InAndOut_Summary | `StocksPrintRequests.Rpt843PaddyStockBreakUpItemWiseRequest` |
| 844-StockClosingAndOpeningData.rpt | `/reports/print/844-stock-closing-and-opening-data` | usp_getStockClosingAndOpeningData | `StocksPrintRequests.Rpt844StockClosingAndOpeningDataRequest` |

## Store

| Template | GET / POST endpoint | Procedure | Request |
|---|---|---|---|
| 148_DeliveryChallanSlip.rpt | `/reports/print/148-delivery-challan-slip` | [dbo].[USP_DeliveryChallanHeader_Slip] | `StorePrintRequests.Rpt148DeliveryChallanSlipRequest` |
| 262-DeliveryOrderSlip.rpt | `/reports/print/262-delivery-order-slip` | Sp_InvDeliveryOrder_Slip | `StorePrintRequests.Rpt262DeliveryOrderSlipRequest` |
| 451-RptDepartmentRequestSlip.rpt | `/reports/print/451-department-request-slip` | Sp_DepartmentRequest_History | `StorePrintRequests.Rpt451DepartmentRequestSlipRequest` |
| 1615-DepartmentRequestToConsumableStoreSlip.rpt | `/reports/print/1615-department-request-to-consumable-store-slip` | Sp_DepartmentRequest_History | `StorePrintRequests.Rpt1615DepartmentRequestToConsumableStoreSlipRequest` |
| 212-InvRptGoodsReceiptsNotesStoreSlip.rpt | `/reports/print/212-goods-receipts-notes-store-slip` | Sp_InvGrn_StoreSlip_Rpt | `StorePrintRequests.Rpt212GoodsReceiptsNotesStoreSlipRequest` |
| 336-GrnStoreRegister.rpt | `/reports/print/336-grn-store-register` | USp_InvGrnStore_Register | `StorePrintRequests.Rpt336GrnStoreRegisterRequest` |
| 454-InvPurchaseDemanSlip.rpt | `/reports/print/454-inv-purchase-deman-slip` | Sp_InvPurchaseDemand_Rpt | `StorePrintRequests.Rpt454InvPurchaseDemanSlipRequest` |
| 145A-PurchaseInvoiceReturn_StorePartySlip.rpt | `/reports/print/145a-purchase-invoice-return-store-party-slip` | [dbo].[USP_PurchaseInvoiceReturn_StoreSip] | `StorePrintRequests.Rpt145APurchaseInvoiceReturnStorePartySlipRequest` |
| 145-PurchaseInvoiceReturn_StoreItemSlip.rpt | `/reports/print/145-purchase-invoice-return-store-item-slip` | [dbo].[USP_PurchaseInvoiceReturn_StoreSip] | `StorePrintRequests.Rpt145PurchaseInvoiceReturnStoreItemSlipRequest` |
| 147_PurchasePreBillSlip.rpt | `/reports/print/147-purchase-pre-bill-slip` | [dbo].[USP_PurchasePreBillHeader_Slip] | `StorePrintRequests.Rpt147PurchasePreBillSlipRequest` |
| 409-InvStockAdjustmentSlip.rpt | `/reports/print/409-inv-stock-adjustment-slip` | Sp_StockAdjustmentSlipAndRegister | `StorePrintRequests.Rpt409InvStockAdjustmentSlipRequest` |
| 406-InvStockTransferSlip.rpt | `/reports/print/406-inv-stock-transfer-slip` | Sp_InvStockTransfer_SlipandRegister | `StorePrintRequests.Rpt406InvStockTransferSlipRequest` |
| 407-InvStockTransferSlip.rpt | `/reports/print/407-inv-stock-transfer-slip` | Sp_InvStockTransfer_SlipandRegister | `StorePrintRequests.Rpt407InvStockTransferSlipRequest` |
| 452-RptInvGsStoreIssuanceHeader_Slip.rpt | `/reports/print/452-inv-gs-store-issuance-header-slip` | Sp_InvGsStoreIssuanceHeader_SlipandRegister | `StorePrintRequests.Rpt452InvGsStoreIssuanceHeaderSlipRequest` |
| 1616-StoreIssuanceToConsumableStore_Slip.rpt | `/reports/print/1616-store-issuance-to-consumable-store-slip` | Sp_InvGsStoreIssuanceHeader_SlipandRegister | `StorePrintRequests.Rpt1616StoreIssuanceToConsumableStoreSlipRequest` |
| 457-StoreIssuanceReturnSlip.rpt | `/reports/print/457-store-issuance-return-slip` | Sp_InvStoreRetrun_SlipandRegister | `StorePrintRequests.Rpt457StoreIssuanceReturnSlipRequest` |
| 458-StorePurchaseRegister.rpt | `/reports/print/458-store-purchase-register` | USP_StorePurchaseRegister | `StorePrintRequests.Rpt458StorePurchaseRegisterRequest` |
| 144-InvGdn_Slip.rpt | `/reports/print/144-inv-gdn-slip` | Sp_InvGdn_SlipAndRegisterRice_Rpt | `StorePrintRequests.Rpt144InvGdnSlipRequest` |
| 149-InvGdnStorePmSlip.rpt | `/reports/print/149-inv-gdn-store-pm-slip` | Sp_InvGdn_SlipAndRegisterRice_Rpt | `StorePrintRequests.Rpt149InvGdnStorePmSlipRequest` |
| 201-InvRptPurchaseOrderGeneralSlip.rpt | `/reports/print/201-purchase-order-general-slip` | Sp_PurchaseOrder_GeneralOrderSlip_Rpt | `StorePrintRequests.Rpt201PurchaseOrderGeneralSlipRequest` |
| 202_01_ItemandWarehouseStockSummary.rpt | `/reports/print/202-01-itemand-warehouse-stock-summary` | Sp_ItemStockReportWithValues_Store | `StorePrintRequests.Rpt20201ItemandWarehouseStockSummaryRequest` |
| 212_01_GoodsReceiptsNotesEmptyBagsSlip.rpt | `/reports/print/212-01-goods-receipts-notes-empty-bags-slip` | Sp_InvGrn_StoreSlip_Rpt | `StorePrintRequests.Rpt21201GoodsReceiptsNotesEmptyBagsSlipRequest` |
| 239-SaleInvoice_StoreBillWithTax.rpt | `/reports/print/239-sale-invoice-store-bill-with-tax` | SP_InvSaleInvoice_CustomerBillRice OthersExp_SubRep | `StorePrintRequests.Rpt239SaleInvoiceStoreBillWithTaxRequest` |
| 401-StockEvalautionDetail_GenerateStocks_Register -Store.rpt | `/reports/print/401-stock-evalaution-detail-generate-stocks-register-store` | Sp_InventoryStockEvalautionDetail_GenerateStocksStore | `StorePrintRequests.Rpt401StockEvalautionDetailGenerateStocksRegisterStoreRequest` |
| 402-InvStockRptInventoryTransactionsA-Store.rpt | `/reports/print/402-inv-stock-inventory-transactions-a-store` | Sp_InventoryTransactions_GenerateStocksReport_Store_Rpt | `StorePrintRequests.Rpt402InvStockInventoryTransactionsAStoreRequest` |
| 403-InvStockRptInventoryTransactionsStocks -Store.rpt | `/reports/print/403-inv-stock-inventory-transactions-stocks-store` | Sp_InventoryTransactions_GenerateStocksReport_Store_Rpt | `StorePrintRequests.Rpt403InvStockInventoryTransactionsStocksStoreRequest` |
| 422_01_StockReportWithoutValueDocumentWiseStore.rpt | `/reports/print/422-01-stock-report-without-value-document-wise-store` | USP_StockReport_WithoutValue_DocumentWiseStore | `StorePrintRequests.Rpt42201StockReportWithoutValueDocumentWiseStoreRequest` |
| 422_InventoryEvaluationLedgerStore.rpt | `/reports/print/422-inventory-evaluation-ledger-store` | USP_InventoryEvaluationLedgerStore | `StorePrintRequests.Rpt422InventoryEvaluationLedgerStoreRequest` |
| 450-RptDepartmentRequestRegister.rpt | `/reports/print/450-department-request-register` | Sp_DepartmentRequest_History | `StorePrintRequests.Rpt450DepartmentRequestRegisterRequest` |
| 456-PurchaseDemandRegister.rpt | `/reports/print/456-purchase-demand-register` | USP_StorePurchaseDemandRegister | `StorePrintRequests.Rpt456PurchaseDemandRegisterRequest` |
| 465-StoreSendReceipt_Slip.rpt | `/reports/print/465-store-send-receipt-slip` | USP_StoreSendReceipt_Slip | `StorePrintRequests.Rpt465StoreSendReceiptSlipRequest` |
| 475-RptInvGsStoreIssuanceHeader_Slip.rpt | `/reports/print/475-inv-gs-store-issuance-header-slip` | Sp_InvGsStoreIssuanceHeader_SlipandRegister | `StorePrintRequests.Rpt475InvGsStoreIssuanceHeaderSlipRequest` |
| 90-SaleOrderStoreAndPmSlip.rpt | `/reports/print/90-sale-order-store-and-pm-slip` | Sp_SaleOrder_RiceSlip_Rpt | `StorePrintRequests.Rpt90SaleOrderStoreAndPmSlipRequest` |
| DeliveryChallanHeader_ExpenseSubReport.rpt | `/reports/print/delivery-challan-header-expense-sub-report` | [dbo].[USP_DeliveryChallan_ExpenseSubReport] | `StorePrintRequests.RptDeliveryChallanHeaderExpenseSubReportRequest` |
| PurchasePreBillHeader_ExpenseSubReport.rpt | `/reports/print/purchase-pre-bill-header-expense-sub-report` | [dbo].[USP_PurchasePreBillHeader_ExpenseSubReport] | `StorePrintRequests.RptPurchasePreBillHeaderExpenseSubReportRequest` |
| StoreIssuanceSubReport.rpt | `/reports/print/store-issuance-sub-report` | Sp_InvGsStoreIssuanceHeader_SlipandRegister | `StorePrintRequests.RptStoreIssuanceSubReportRequest` |

## Tax

| Template | GET / POST endpoint | Procedure | Request |
|---|---|---|---|
| 389-GetPreInvoicesAndForwardingDateForCommercialInvoices.rpt | `/reports/print/389-get-pre-invoices-and-forwarding-date-for-commercial-invoices` | USP_GetPreInvoicesAndForwardingDateForCommercialInvoices | `TaxPrintRequests.Rpt389GetPreInvoicesAndForwardingDateForCommercialInvoicesRequest` |
| 546-GetProformaDataForInvoices.rpt | `/reports/print/546-get-proforma-data-for-invoices` | USP_GetProformaDataForInvoices | `TaxPrintRequests.Rpt546GetProformaDataForInvoicesRequest` |
| 568-CommissionAgentFcyLedger.rpt | `/reports/print/568-commission-agent-fcy-ledger` | usp_CommissionAgentFcyLedger | `TaxPrintRequests.Rpt568CommissionAgentFcyLedgerRequest` |
| 569-CommercialInvoice_Shipments.rpt | `/reports/print/569-commercial-invoice-shipments` | usp_CommercialInvoice_Shipments | `TaxPrintRequests.Rpt569CommercialInvoiceShipmentsRequest` |
| 570-ShipmentdataForBrokeryTax.rpt | `/reports/print/570-shipmentdata-for-brokery-tax` | usp_ShipmentdataForBrokeryTax | `TaxPrintRequests.Rpt570ShipmentdataForBrokeryTaxRequest` |
| 618_01_DoWeightWbWeightDiff.rpt | `/reports/print/618-01-do-weight-wb-weight-diff` | SpEximInvoice_DoWeightWbWeightDiff_Rpt | `TaxPrintRequests.Rpt61801DoWeightWbWeightDiffRequest` |

## Weighbridge

| Template | GET / POST endpoint | Procedure | Request |
|---|---|---|---|
| 280-InvRptWeighBridgeSlip.rpt | `/reports/print/280-weigh-bridge-slip` | Sp_WbTransactionsSlip_rpt | `WeighbridgePrintRequests.Rpt280WeighBridgeSlipRequest` |
| 281-InvRptWeighBridgeRegister.rpt | `/reports/print/281-weigh-bridge-register` | [dbo].[USP_WbTransactions_NewReport] | `WeighbridgePrintRequests.Rpt281WeighBridgeRegisterRequest` |
| 281-InvRptWeighBridgeSlipWithPics.rpt | `/reports/print/281-weigh-bridge-slip-with-pics` | Sp_WbTransactionsSlip_rpt | `WeighbridgePrintRequests.Rpt281WeighBridgeSlipWithPicsRequest` |
| WbTransationByGPIDSubReport.rpt | `/reports/print/wb-transation-by-gpid-sub-report` | [dbo].[USP_WbTransationByGPID_SubReport] | `WeighbridgePrintRequests.RptWbTransationByGPIDSubReportRequest` |
| WbTransationByOutwardGPID_SubReport.rpt | `/reports/print/wb-transation-by-outward-gpid-sub-report` | [dbo].[USP_WbTransationByOutwardGPID_SubReport] | `WeighbridgePrintRequests.RptWbTransationByOutwardGPIDSubReportRequest` |
