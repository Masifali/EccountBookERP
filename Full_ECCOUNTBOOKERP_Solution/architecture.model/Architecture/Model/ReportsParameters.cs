using System;
using System.Collections.Generic;

namespace Architecture.Model;

public class ReportsParameters
{
	public int Id { get; set; }

	public long RecId { get; set; }

	public long DepartmentId { get; set; }

	public int DepartmentFromId { get; set; }

	public int DepartmentToId { get; set; }

	public int DesignationId { get; set; }

	public int PartyLocationId { get; set; }

	public int StockPartyId { get; set; }

	public int CompanyId { get; set; }

	public int ProductionTypeId { get; set; }

	public int GlAccountId { get; set; }

	public string ControlAccountIds { get; set; }

	public int LeaveQuotaId { get; set; }

	public int LeaveTypeProfileId { get; set; }

	public int CompanyIdCoa { get; set; }

	public int OrganizationId { get; set; }

	public int ScreenId { get; set; }

	public int ProductTypeId { get; set; }

	public int ExImInvoiceId { get; set; }

	public int SalePersonId { get; set; }

	public int EFormRegistrationId { get; set; }

	public int OrderId { get; set; }

	public int OrderCategoryId { get; set; }

	public int JobOrderId { get; set; }

	public string JobOrderIds { get; set; }

	public int ConversionType { get; set; }

	public int PlantId { get; set; }

	public int DiscountPolicyId { get; set; }

	public int BrandId { get; set; }

	public int InvConractorWagesAccountsId { get; set; }

	public string DocumentTypeIds { get; set; }

	public string AccountClassIdsNot { get; set; }

	public string AccountClassIds { get; set; }

	public string TransferType { get; set; }

	public int PmItemId { get; set; }

	public int RefDocIdNo { get; set; }

	public int RefRefDocumentTypeId { get; set; }

	public int BuyerId { get; set; }

	public int DeliverToPartyId { get; set; }

	public int SupplierCustomerId { get; set; }

	public int SalesPersonId { get; set; }

	public int InvJobOrderId { get; set; }

	public int DeliveryTypeId { get; set; }

	public int DistrictId { get; set; }

	public int RefSaleMan { get; set; }

	public int RefSaleManId { get; set; }

	public int ContractScheduleId { get; set; }

	public string ContractScheduleIds { get; set; }

	public int LoadingLocationId { get; set; }

	public int GhallaMandiId { get; set; }

	public int BillNoFrom { get; set; }

	public int BillNoTo { get; set; }

	public string BiltyNo { get; set; }

	public double RateFrom { get; set; }

	public double RateTo { get; set; }

	public int ServiceTypeId { get; set; }

	public int BillToPartyId { get; set; }

	public int BrokerOrServiceProviderId { get; set; }

	public int BrokerAgentId { get; set; }

	public int SubBrokerId { get; set; }

	public int CommissionAgentId { get; set; }

	public int AccountId { get; set; }

	public int StockAccountId { get; set; }

	public int PaidToAccount { get; set; }

	public int cheqStatus { get; set; }

	public int CreditAccountId { get; set; }

	public int FromWarehouseId { get; set; }

	public int ToWarehouseId { get; set; }

	public int ToJobLotId { get; set; }

	public int AccouuntClassId { get; set; }

	public int ItemId { get; set; }

	public int InvoiceId { get; set; }

	public long ImInvoiceId { get; set; }

	public int DeliveryTermId { get; set; }

	public int PaymentTermId { get; set; }

	public int VoucherHeadId { get; set; }

	public int VoucherDetailId { get; set; }

	public string WorkOrderNo { get; set; }

	public string CustomerName { get; set; }

	public string PaymentTerm { get; set; }

	public int FarmingTypeId { get; set; }

	public int TradeTypeId { get; set; }

	public int ProductinTypeId { get; set; }

	public int BOMHeaderId { get; set; }

	public int WorkStationFromId { get; set; }

	public int WorkOrderId { get; set; }

	public int CastingTypeId { get; set; }

	public int WorkStationToId { get; set; }

	public int InventoryParentCategories { get; set; }

	public int TranTypes { get; set; }

	public int RequestedById { get; set; }

	public int ApprovedById { get; set; }

	public int TypeId { get; set; }

	public int StoreRackId { get; set; }

	public int ReasonId { get; set; }

	public int ItemConditionId { get; set; }

	public int GroupId { get; set; }

	public int ImporterId { get; set; }

	public int FromStockPartyId { get; set; }

	public int ToStockPartyId { get; set; }

	public string Status { get; set; }

	public string CurrentStatusIds { get; set; }

	public string GatepassType { get; set; }

	public string BranchesIds { get; set; }

	public string WorkOrderIds { get; set; }

	public string InvoiceNo { get; set; }

	public string FDBCNo { get; set; }

	public string SelectedScreedIds { get; set; }

	public double FromDocNo { get; set; }

	public double DutyRoasterFrom { get; set; }

	public double DutyRoasterTo { get; set; }

	public double ServiceProviderId { get; set; }

	public double DocumentProviderId { get; set; }

	public double RequiredLevelId { get; set; }

	public double CriteriaDateTypeId { get; set; }

	public double ToDocNo { get; set; }

	public double BalanceFrom { get; set; }

	public double BalanceTo { get; set; }

	public int ShowAssetLiability { get; set; }

	public int GpSrNoF { get; set; }

	public int GpSrNoT { get; set; }

	public int GpDocumentTypeId { get; set; }

	public int AdjustmentTypeId { get; set; }

	public int GpId { get; set; }

	public int XGroupId { get; set; }

	public int YGroupId { get; set; }

	public int YItemId { get; set; }

	public int NewUserId { get; set; }

	public int EntryUser { get; set; }

	public int DocumentTypeId { get; set; }

	public int BaseDocumentTypeId { get; set; }

	public int OrderSupCustId { get; set; }

	public int ModifyUserId { get; set; }

	public int TransactionTypeId { get; set; }

	public int RefDocumentTypeId { get; set; }

	public int RefRefDocIdNo { get; set; }

	public int RefDocSubId { get; set; }

	public int OtherDocumentTypeId { get; set; }

	public int OtherDocNoId { get; set; }

	public int OtherSubDocNoId { get; set; }

	public int FolderId { get; set; }

	public int CalculationParameterId { get; set; }

	public int RevenueExpenseId { get; set; }

	public int ChargesIncentiveId { get; set; }

	public int CustomerId { get; set; }

	public int SupplierId { get; set; }

	public int ContractId { get; set; }

	public int ExporterId { get; set; }

	public int DebitAccountId { get; set; }

	public double MinFileSize { get; set; }

	public double MaxFileSize { get; set; }

	public double PackUomFrom { get; set; }

	public double PackUomTo { get; set; }

	public double FreightDiscountAmount { get; set; }

	public int ExImFCBankReceiptsId { get; set; }

	public int BankId { get; set; }

	public int ShippingLineId { get; set; }

	public int ExImEFormRegistrationId { get; set; }

	public int BankGlDrAc { get; set; }

	public int AgingDays { get; set; }

	public int IntervalDays { get; set; }

	public int ProjectsId { get; set; }

	public int PriceTypeId { get; set; }

	public int MasterDiscountTypeId { get; set; }

	public int BranchesId { get; set; }

	public int PestResultId { get; set; }

	public int PestStatusId { get; set; }

	public int ExImLcOrderId { get; set; }

	public int ExImLcOrderNo { get; set; }

	public int ReportConfigId { get; set; }

	public int DocNo { get; set; }

	public int DestinationPortId { get; set; }

	public int TransporterId { get; set; }

	public int FirstWeighBridgeId { get; set; }

	public int ForwarderId { get; set; }

	public int ShippingAgentId { get; set; }

	public int ClearingAgentId { get; set; }

	public int CarreierTypeId { get; set; }

	public int CheqId { get; set; }

	public int EximInvoiceId { get; set; }

	public int InvoiceDetailId { get; set; }

	public int LoadingPortId { get; set; }

	public int PaymenetTermId { get; set; }

	public int AppId { get; set; }

	public int PendingForView { get; set; }

	public int AppModuleId { get; set; }

	public int BagsConditionId { get; set; }

	public int ExpiryDays { get; set; }

	public int AnalysisGroupId { get; set; }

	public bool IsApproved { get; set; }

	public bool? ApprovedStatus { get; set; }

	public bool CanViewAllRecord { get; set; }

	public bool CheckConfig { get; set; }

	public bool ActionTypeId { get; set; }

	public bool IsTaxable { get; set; }

	public bool PostState { get; set; }

	public bool ReferedInSDS { get; set; }

	public bool IncludeExpired { get; set; }

	public string AccountLevels { get; set; }

	public string CustomGroupIds { get; set; }

	public string ApprovedFilter { get; set; }

	public string PendingForViewFilter { get; set; }

	public string ConfigDescription { get; set; }

	public string Password { get; set; }

	public string OldPassword { get; set; }

	public string AreaCity { get; set; }

	public string OtherCode { get; set; }

	public string VoucherType { get; set; }

	public string CashVoucher { get; set; }

	public string BankVoucher { get; set; }

	public string OrderDetailIds { get; set; }

	public string PackingExpiryDate { get; set; }

	public string ProductionNo { get; set; }

	public string AnalystStatus { get; set; }

	public string AnalystPerson { get; set; }

	public string AWBBillNo { get; set; }

	public int FinancialYearId { get; set; }

	public int UserId { get; set; }

	public int ChartOfAccountId { get; set; }

	public int OrganizationTemplateId { get; set; }

	public int Account_Level { get; set; }

	public int LanguageId { get; set; }

	public int PageNumber { get; set; }

	public int PageSize { get; set; }

	public int SpecialApprovalDocs { get; set; }

	public int JobLotId { get; set; }

	public int NoOfRecords { get; set; }

	public int IsSpecialApprovalRequired { get; set; }

	public int NotUploaded { get; set; }

	public int WithoutStockLogic { get; set; }

	public int ActionId { get; set; }

	public int StatusId { get; set; }

	public int ActivityId { get; set; }

	public int AccountTypeId { get; set; }

	public int CustomerGroupId { get; set; }

	public int ParentsSupCustId { get; set; }

	public string CustomerGroupIds { get; set; }

	public int CustomGroupId { get; set; }

	public int SaleAccountId { get; set; }

	public int CGSAccountId { get; set; }

	public int MonthValue { get; set; }

	public int AccountNoteId { get; set; }

	public int FcyId { get; set; }

	public int CurrencyId { get; set; }

	public int SubsidiaryAccountId { get; set; }

	public int SubsidiaryTypeId { get; set; }

	public int OrderNoFrom { get; set; }

	public int OrderNoTo { get; set; }

	public int SlabFrom1 { get; set; }

	public int SlabFrom2 { get; set; }

	public int SlabFrom3 { get; set; }

	public int SlabTo1 { get; set; }

	public int SlabTo2 { get; set; }

	public int SlabTo3 { get; set; }

	public int LineId { get; set; }

	public int ParentId { get; set; }

	public int ItemTypeOfTypeId { get; set; }

	public int ParentAccountId { get; set; }

	public int SortNo { get; set; }

	public DateTime GpDate { get; set; }

	public DateTime BiltyDate { get; set; }

	public DateTime FromDate { get; set; }

	public DateTime ToDate { get; set; }

	public DateTime GpDateF { get; set; }

	public DateTime GpDateT { get; set; }

	public DateTime DisPatchedDate { get; set; }

	public DateTime DisPatchedToDate { get; set; }

	public DateTime ETADate { get; set; }

	public DateTime ETAToDate { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime DueDateFrom { get; set; }

	public DateTime DueDateTo { get; set; }

	public DateTime ReadyDateFrom { get; set; }

	public DateTime ReadyDateTo { get; set; }

	public DateTime EntryDateFrom { get; set; }

	public DateTime EntryDateTo { get; set; }

	public DateTime ModifyDateFrom { get; set; }

	public DateTime ModifyDateTo { get; set; }

	public DateTime ApprovedDateFrom { get; set; }

	public DateTime ApprovedDateTo { get; set; }

	public DateTime ValidityDateFrom { get; set; }

	public DateTime ValidityDateTo { get; set; }

	public DateTime StartDate { get; set; }

	public DateTime EndDate { get; set; }

	public DateTime CheqDateFrom { get; set; }

	public DateTime CheqDateTo { get; set; }

	public DateTime PostDate { get; set; }

	public DateTime CurrentDate { get; set; }

	public DateTime EffectedDate { get; set; }

	public DateTime EffectedDateTo { get; set; }

	public DateTime AsOnDate { get; set; }

	public string ItemName { get; set; }

	public int ShipToAddressId { get; set; }

	public string ShipToAddress { get; set; }

	public string CompanyAddress { get; set; }

	public string ParentAccountCode { get; set; }

	public string CompanyName { get; set; }

	public string AccountTitle { get; set; }

	public string UserName { get; set; }

	public string ExmLcPaymentTerm { get; set; }

	public string ReqType { get; set; }

	public string Remarks { get; set; }

	public string CropYear { get; set; }

	public string ScreenName { get; set; }

	public string ReportName { get; set; }

	public string DiscountName { get; set; }

	public string SaleInvoiceDocumentTypeIds { get; set; }

	public string PurchaseInvoiceIds { get; set; }

	public string GdnIds { get; set; }

	public string ItemIds { get; set; }

	public string Ids { get; set; }

	public string ParentCategoryIds { get; set; }

	public int StockReleaseFromFumigation { get; set; }

	public int StockReleaseHoldStatusId { get; set; }

	public int AvailableForFumigation { get; set; }

	public string AccountIds { get; set; }

	public string AccountTypeIds { get; set; }

	public string AccountTypeIdsNot { get; set; }

	public string BSNoteIds { get; set; }

	public string PLNoteIds { get; set; }

	public string WarehouseIds { get; set; }

	public string ReferencePartyName { get; set; }

	public string OrderNo { get; set; }

	public string RefDocumentTypeIds { get; set; }

	public int RefPartyId { get; set; }

	public int ReferencePartyId { get; set; }

	public string VehicleNo { get; set; }

	public string DriverName { get; set; }

	public string Activity { get; set; }

	public int TopCustomers { get; set; }

	public int TopSuppliers { get; set; }

	public string ManualBillNo { get; set; }

	public string WeighBridgeStatus { get; set; }

	public string Container1 { get; set; }

	public string Container2 { get; set; }

	public string Month { get; set; }

	public DateTime MonthDate { get; set; }

	public string DashboardObjectName { get; set; }

	public string DepartmentName { get; set; }

	public string PendingWorkName { get; set; }

	public string Keyword { get; set; }

	public string XML { get; set; }

	public string EntryType { get; set; }

	public string ItemTypeIds { get; set; }

	public int Year { get; set; }

	public bool ShowHeader { get; set; }

	public bool IsRestDay { get; set; }

	public int CwCoaId { get; set; }

	public int HeaderId { get; set; }

	public int GroupAccountId { get; set; }

	public int CountryId { get; set; }

	public int CityId { get; set; }

	public int LotNoRef { get; set; }

	public int ZeroBalanceType { get; set; }

	public int SkipCgsAccounts { get; set; }

	public int AllowWipItem { get; set; }

	public bool IsCreateSMSTemplate { get; set; }

	public int WarehouseId { get; set; }

	public int ProductionStageId { get; set; }

	public int RefWarehouseId { get; set; }

	public int CropYearId { get; set; }

	public int ItemTypeId { get; set; }

	public int ItemClassId { get; set; }

	public int PurchaseGLAC { get; set; }

	public int SaleGLAC { get; set; }

	public int COGSGLAC { get; set; }

	public int DiscountTypeId { get; set; }

	public bool ItemStatus { get; set; }

	public bool WithOrWithoutVariant { get; set; }

	public int AssetId { get; set; }

	public int TransactionFlowId { get; set; }

	public int ItemCategoryId { get; set; }

	public string ItemCategoryIds { get; set; }

	public int OtherCategoryId { get; set; }

	public int ItemUomId { get; set; }

	public int stockUOM { get; set; }

	public int VarientId { get; set; }

	public int PackingTypeId { get; set; }

	public int ParentCategoryId { get; set; }

	public int BookingPersonId { get; set; }

	public int ItemClassGroupId { get; set; }

	public int SONoFrom { get; set; }

	public int SONoTo { get; set; }

	public int RateUOM { get; set; }

	public int JobOrderFrom { get; set; }

	public int JobOrderTo { get; set; }

	public int LcOrderId { get; set; }

	public int WagesActivityId { get; set; }

	public int ExImInvoiceNoId { get; set; }

	public int EformNoId { get; set; }

	public int BillOfLadingNo { get; set; }

	public int LcOrderNo { get; set; }

	public long EmployeeId { get; set; }

	public int GetMonth { get; set; }

	public int GetYear { get; set; }

	public int SectionId { get; set; }

	public int ShiftId { get; set; }

	public int EmployeeCategoryId { get; set; }

	public int EmployeeGroupId { get; set; }

	public int StoreId { get; set; }

	public double NetWeight { get; set; }

	public double FirstWeight { get; set; }

	public double SecondWeight { get; set; }

	public decimal EbPUnitF { get; set; }

	public decimal EbPUnitT { get; set; }

	public double StockWeight { get; set; }

	public double RateEqvailent { get; set; }

	public double ExchangeRateUSD { get; set; }

	public double ExchangeRateEURO { get; set; }

	public double FcyRate { get; set; }

	public double ItemRate { get; set; }

	public double ItemQty { get; set; }

	public double Amount { get; set; }

	public double SlipAmount { get; set; }

	public double Debit { get; set; }

	public double Credit { get; set; }

	public string ReportType { get; set; }

	public string DeliveryTerm { get; set; }

	public string RefPartyIds { get; set; }

	public int ReportTypeId { get; set; }

	public int SkipZero { get; set; }

	public int IsOnQty { get; set; }

	public long DutyRoasterId { get; set; }

	public bool ReportStatus { get; set; }

	public string InspectionStatus { get; set; }

	public virtual int RefDocumentTypeIdgpi { get; set; }

	public virtual int RefDocumentTypeIdgpo { get; set; }

	public virtual int RefDocumentTypeIdMo { get; set; }

	public int PowerMeterId { get; set; }

	public int PowerTypeId { get; set; }

	public int PolicyTypeId { get; set; }

	public int ReadingTypeId { get; set; }

	public int ContractorId { get; set; }

	public int ContractorWagesActivityId { get; set; }

	public int WagesItemId { get; set; }

	public int PackSizeFrom { get; set; }

	public int PackSizeTo { get; set; }

	public int ProductionType { get; set; }

	public int RepresentativeAcId { get; set; }

	public int SenderAcId { get; set; }

	public int ReceiverAcId { get; set; }

	public int VoucherTypeId { get; set; }

	public int LocationId { get; set; }

	public int TaxItemId { get; set; }

	public int PartyTypeId { get; set; }

	public int ConversionTypeId { get; set; }

	public int DocumentStatusId { get; set; }

	public int MasterItemId { get; set; }

	public int MasterAppId { get; set; }

	public int CostCenterId { get; set; }

	public int NotReferred { get; set; }

	public int ModalDescriptionWise { get; set; }

	public int ProductionStageWise { get; set; }

	public int CastingTypeWise { get; set; }

	public int CropYearWise { get; set; }

	public int JobLotWise { get; set; }

	public int PackingTypeWise { get; set; }

	public int WarehouseWise { get; set; }

	public int BatchWise { get; set; }

	public int LabId { get; set; }

	public int WbId { get; set; }

	public int WorkOrderWise { get; set; }

	public int ItemWise { get; set; }

	public int CustomerWise { get; set; }

	public int UomWise { get; set; }

	public int CityWise { get; set; }

	public int DocumentTypeWise { get; set; }

	public int DocumentWise { get; set; }

	public string CompanyIds { get; set; }

	public string BranchIds { get; set; }

	public string TrackingNo { get; set; }

	public string MetricType { get; set; }

	public string BatchNo { get; set; }

	public string SortType { get; set; }

	public string SortingCol { get; set; }

	public double ResultValue { get; set; }

	public int RecordIdentityId { get; set; }

	public string AccountCode { get; set; }

	public string AccountCodeOperator { get; set; }

	public string AccountTitleFT { get; set; }

	public string ParentAccount { get; set; }

	public string ParentAccountFT { get; set; }

	public string AccountType { get; set; }

	public string AccountTypeFT { get; set; }

	public double Opening { get; set; }

	public string OpeningOperator { get; set; }

	public double IstInterval { get; set; }

	public string IstIntervalOperator { get; set; }

	public double ScnInterval { get; set; }

	public string ScnIntervalOperator { get; set; }

	public double ThirdInterval { get; set; }

	public string ThirdIntervalOperator { get; set; }

	public double Above { get; set; }

	public string AboveOperator { get; set; }

	public double Closing { get; set; }

	public string ClosingOperator { get; set; }

	public double CurrentBalance { get; set; }

	public string CurrentBalanceOperator { get; set; }

	public double OverDue { get; set; }

	public string OverDueOperator { get; set; }

	public double NotYetDue { get; set; }

	public string NotYetDueOperator { get; set; }

	public string ContactNo { get; set; }

	public virtual List<ReportsParameters> ReportsParameterslist { get; set; }
}
