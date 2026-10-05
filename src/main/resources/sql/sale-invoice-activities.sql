-- Java Sale Invoice Activities. Native SpInventory_EvaulationDetailSalesReports, 2026-10-04.
-- Work tables are private to this request; no persistent report or business rows are changed.
SET NOCOUNT ON;
DECLARE @OrganizationId int = ?;
DECLARE @CompanyId int = ?;
DECLARE @FromDate DATE = ?;
DECLARE @ToDate DATE = ?;
DECLARE @DocNoFrom int = ?;
DECLARE @DocNoTo int = ?;
DECLARE @InventoryParentCategories int = ?;
DECLARE @ItemCategoryId int = ?;
DECLARE @ItemTypeId int = ?;
DECLARE @ItemClassGroupId int = ?;
DECLARE @ItemId int = ?;
DECLARE @PackUom float = ?;
DECLARE @CropYear nvarchar(20) = ?;
DECLARE @JobLotId int = ?;
DECLARE @PackingTypeId int = ?;
DECLARE @SupplierCustomerId int = ?;
DECLARE @WarehouseId int = ?;
DECLARE @CityId int = ?;
DECLARE @DistrictId int = ?;
DECLARE @ActivityName nvarchar(max) = ?;
DECLARE @RefPartyId int = ?;
DECLARE @PageSize int = ?;
DECLARE @PageNumber int = ?;
DECLARE @BranchesIds nvarchar(max) = ?;
DECLARE @AppId INT = ?;
DECLARE @UserId INT = ?;
DECLARE @CostCenterId INT = ?;
DECLARE @OtherCategoryId INT = ?;
DECLARE @AccountId INT = ?;
DECLARE @SaleAccountId INT = ?;
DECLARE @StockAccountId INT = ?;
DECLARE @CGSAccountId INT = ?;
DECLARE @AccountsCustomGroupsId INT = ?;
DECLARE @CustomGroupIds NVARCHAR(MAX) = ?;
DECLARE @ActionId INT = ?;
DECLARE @BookingPersonId int = ?;
SELECT TOP (0) * INTO #InvSalesRegisterTemp FROM dbo.InvSalesRegisterTemp;
SET NOCOUNT ON;

DECLARE @getToDate DATE = DATEADD(DAY,1,@ToDate)

IF ISNULL(@UserId,0) = 0
BEGIN
	RAISERROR('UserId not found',16,1,'Amir Hussain');
	RETURN
END

IF @AppId IN (4,6) -- MobileApp_CustomerPortal , Desktop_CustomerPortal
BEGIN
	SELECT @SupplierCustomerId = SupplierCustomerId FROM UserAccount where ID = @UserId
END


CREATE TABLE #BrancheIds(BranchId INT)
INSERT INTO #BrancheIds
SELECT data from dbo.fnSplitString(@BranchesIds,',')

IF (@PageNumber <=0)
BEGIN
	SET @PageNumber = 1;
END
IF (@PageSize <=0)
BEGIN
	SET @PageSize = 1;
END
DECLARE @SkipRows int = (@PageNumber - 1) * @PageSize;
IF @PageSize IS NULL AND @PageNumber IS NULL 
BEGIN
	SET @SkipRows = 0;
	SET @PageSize = 1000000;
END

DECLARE @ReportParamCSV VARCHAR(max)
DECLARE @AND NVARCHAR(3)
DECLARE @ItemClassGroup NVARCHAR(max)
DECLARE @ParentCategory NVARCHAR(max)
DECLARE @ItemCategory NVARCHAR(max)
DECLARE @ItemType NVARCHAR(max)
DECLARE @WareHouse NVARCHAR(max)
DECLARE @JobLot NVARCHAR(max)
DECLARE @PartyName NVARCHAR(max)
DECLARE @ItemName NVARCHAR(max)
DECLARE @District NVARCHAR(max)
DECLARE @CityName NVARCHAR(max)
DECLARE @RefParty NVARCHAR(max)

DECLARE @CostCenterName nvarchar(max)

SET @ReportParamCSV = ''
SET @AND = 'AND'
IF @ItemClassGroupId > 0
BEGIN
	SELECT @ItemClassGroup = ClassGroupName FROM ItemClassGroup WHERE Id = @ItemClassGroup
END
IF @InventoryParentCategories > 0
BEGIN
	SELECT @ParentCategory = p.InvParentCateDescription FROM InventoryParentCategories p WHERE Id = @InventoryParentCategories
END
IF @ItemCategoryId> 0
BEGIN
	SELECT @itemCategory = p.CategoryDescription FROM ItemCategory p WHERE Id = @ItemCategoryId
END
IF @ItemTypeId> 0
BEGIN
	SELECT @itemCategory = p.TypeDescription FROM ItemType p WHERE Id = @ItemTypeId
END
IF @WarehouseId> 0
BEGIN
	SELECT @WareHouse = p.WareHouseName FROM InvWareHouse p WHERE Id = @WarehouseId
END
IF @JobLotId> 0
BEGIN
	SELECT @JobLot = p.JobLotDescription FROM JobLot p WHERE Id = @JobLotId
END
IF @SupplierCustomerId> 0
BEGIN
	SELECT @PartyName = p.CompanyName FROM SupplierCustomer p WHERE Id = @SupplierCustomerId
END
IF @ItemId> 0
BEGIN
	SELECT @ItemName = p.ItemName FROM Item p WHERE Id = @ItemId
END
IF @DistrictId> 0
BEGIN
	SELECT @District = p.Description FROM District p WHERE Id = @DistrictId
END
IF @CityId> 0
BEGIN
	SELECT @CityName = p.Description FROM City p WHERE Id = @CityId
END
IF @RefPartyId> 0
BEGIN
	SELECT @RefParty = p.ReferencePartyName FROM ReferenceParties p WHERE Id = @RefPartyId
END

IF @CostCenterId > 0
BEGIN
	SELECT @CostCenterName = CostCenterName FROM CostCenter WHERE Id = @CostCenterId
END


IF @FromDate IS not NULL
SET @ReportParamCSV = @ReportParamCSV + 'FromDate: ' + CAST(FORMAT(@FromDate, 'dd/MMM/yyyy ') AS NVARCHAR(50)) 
IF @ToDate IS not NULL
SET @ReportParamCSV += ' ' + @AND + ' ToDate: ' + CAST(FORMAT(@ToDate, 'dd/MMM/yyyy ') AS NVARCHAR(50))
IF @DocNoFrom IS not NULL
SET @ReportParamCSV +=  ' ' + @AND + ' OrderFrom: ' + CAST(@DocNoFrom AS NVARCHAR(50))
IF @DocNoTo IS not NULL
SET @ReportParamCSV = @ReportParamCSV + ' ' + @AND + ' OrderTo: ' + CAST(@DocNoTo AS NVARCHAR(50))
IF @ItemClassGroup IS not NULL
SET @ReportParamCSV = @ReportParamCSV + ' ' + @AND + ' ItemClass: ' + @ItemClassGroup
IF @ParentCategory IS not NULL
SET @ReportParamCSV = @ReportParamCSV + ' ' + @AND + ' ParentCategory: ' + @ParentCategory
IF @ItemCategory IS not NULL
SET @ReportParamCSV = @ReportParamCSV + ' ' + @AND + ' ItemCategory: ' + @ItemCategory
IF @ItemType IS not NULL
SET @ReportParamCSV = @ReportParamCSV + ' ' + @AND + ' ItemType: ' + @ItemType
IF @WareHouse IS not NULL
SET @ReportParamCSV = @ReportParamCSV + ' ' + @AND + ' WareHouse: ' + @WareHouse
IF @JobLot IS not NULL
SET @ReportParamCSV = @ReportParamCSV + ' ' + @AND + ' JobLot: ' + @JobLot
IF @PartyName IS not NULL
SET @ReportParamCSV = @ReportParamCSV + ' ' + @AND + ' PartyName: ' + @PartyName
IF @ItemName IS not NULL
SET @ReportParamCSV = @ReportParamCSV + ' ' + @AND + ' ItemName: ' + @ItemName
IF @PackUom IS not NULL
SET @ReportParamCSV = @ReportParamCSV + ' ' + @AND + ' PackUOM: ' + CONVERT(VARCHAR(50),@PackUom)
IF @CropYear IS not NULL
SET @ReportParamCSV = @ReportParamCSV + ' ' + @AND + ' CropYear: ' + @CropYear
IF @District IS not NULL
SET @ReportParamCSV = @ReportParamCSV + ' ' + @AND + ' District: ' + @District
IF @CityName IS not NULL
SET @ReportParamCSV = @ReportParamCSV + ' ' + @AND + ' CityName: ' + @CityName
IF @RefParty IS not NULL
SET @ReportParamCSV = @ReportParamCSV + ' ' + @AND + ' ReferenceParty: ' + @RefParty
IF @CostCenterName IS not NULL
SET @ReportParamCSV = @ReportParamCSV + ' ' + @AND + ' CostCenterName: ' + @CostCenterName
IF @ActivityName IS not NULL
SET @ReportParamCSV = @ReportParamCSV + ' ' + @AND + ' ActivityName: ' + @ActivityName


Delete from #InvSalesRegisterTemp

DECLARE @CreditAmountInOtherItemSaleGL BIT = 0;
DECLARE @SaleCostingJobOrderWise BIT = 0;
SET @CreditAmountInOtherItemSaleGL = CAST(dbo.GetConfigurationByConfigId(@OrganizationId,@CompanyId,1053) AS BIT);
SET @SaleCostingJobOrderWise = CAST(dbo.GetConfigurationByConfigId(@OrganizationId,@CompanyId,314) AS BIT);


CREATE TABLE #SaleRegister
(
    RefDocumentTypeId       INT,
	OtherCategoryId		    INT,
    RefDocIdNo              INT,
    DetailId                INT,
    SupplierCustomerId      INT,
    BookingPersonId      INT,
    CityId                  INT,
    DocDate                 DATE,
    DocCodeNo               INT,
    WarehouseId             INT,
    VehicleNo               NVARCHAR(100),
    GpNoDcNo                NVARCHAR(100),
    InventoryParentCategoriesId INT,
    ItemCategoryId          INT,
    ItemId                  INT,
    ItemCode                NVARCHAR(100),
    ItemName                NVARCHAR(500),
    HsCode                  NVARCHAR(100),
    ItemUom                 INT,
    UOMCode                 NVARCHAR(50),
    Equivalent              FLOAT(53),
    CropBatch               NVARCHAR(100),
    JobLotId                INT,
    InvPackingTypeId        INT,
    QtyOut                  FLOAT(53),
    BillWeightOut           FLOAT(53),
    StockWeightOut          FLOAT(53),
    ItemAmount              FLOAT(53),
    AmountOut               FLOAT(53),
    CGSAmount               FLOAT(53),
    ItemRate                FLOAT(53),
    ItemCgsRate             FLOAT(53),
    RateUom                 INT,
    RateEquivalent          FLOAT(53),
    RefPartyId              INT,
    InvGdnId                INT,
    InvGdnDetailId          INT,
    GdnNo                   INT,
    GdnDate                 DATE,
    SoNo                    INT,
    SoDate                  DATE,
    BranchSrNo              INT,
    TicketNos               NVARCHAR(MAX),
    BranchesId              INT,
    CostCenterId            INT,
    ManualBillNo            NVARCHAR(100),
    OtherAmount             FLOAT(53),
    FreightAmount           FLOAT(53),
    CommissionAmount        FLOAT(53),
    RateCut                 FLOAT(53),
    RateCutAmount           FLOAT(53)
);


INSERT INTO #SaleRegister
SELECT 
		h.DocumentTypeId as RefDocumentTypeId, 
		CASE WHEN H.OtherCategoryId = 0 THEN 54 ELSE ISNULL(H.OtherCategoryId,54) END AS OtherCategoryId, -- 54 --> SALES
		h.Id as RefDocIdNo, 
		se.Id as DetailId,
		h.SupplierCustomerId,So.BookingPersonId,
		se.CityId, 
		convert(date,h.DocDate) as DocDate, 
		h.DocNo as DocCodeNo, 
		se.WarehouseId,	
		se.VehicleNo, 
		se.GpNo as GpNoDcNo,	
		ic.InventoryParentCategoriesId, 
		i.ItemCategoryId, 
		se.ItemId,
		i.ItemCode,
		I.ItemName,
		i.HsCode,
		se.ItemUOMId as ItemUom,
		Uom.UOMCode,
		Uom.Equivalent,
		se.CropYear as CropBatch, 
		se.JobLotId, 
		se.PackingTypeId as InvPackingTypeId,	
		se.ItemQty as QtyOut,
		se.NetBillWeight as BillWeightOut,
		se.NetStockWeight as StockWeightOut, 
		se.ItemAmount ,
		se.BillAmount as AmountOut,
		CAST(0.00 AS decimal(18,2)) AS CGSAmount,
		se.ItemRate + ISNULL(se.RateCut,0) AS ItemRate, -- Total ItemRate
		se.ItemCgsRate,
		SE.UomScheduleIdRate as RateUom,
		RUom.Equivalent as RateEquivalent,
		se.RefPartyId,
		se.InvGdnId,
		se.InvGdnDetailId,
		0 GdnNo,
		convert(date,'') GdnDate,
		0 SoNo,
		convert(date,'') SoDate,
		h.BranchSrNo,
		CAST('' as nvarchar(max)) TicketNos,
		CASE WHEN @BranchesIds IS NULL THEN 0 ELSE h.BranchesId END AS BranchesId,se.CostCenterId,h.ManualBillNo,
		(CASE WHEN ISNULL(@CreditAmountInOtherItemSaleGL,0) = 0 THEN se.ExpenseAmount ELSE 0 END) AS OtherAmount,
		se.FreightAmount,
		se.CommissionAmount,
		se.RateCut,
		se.RateCutAmount
from	InvSaleInvoiceDetail se
		INNER JOIN InvSaleInvoice h on se.InvSaleInvoiceId = h.Id 
		INNER JOIN Item i on se.ItemId = i.id
		INNER JOIN V_UomScheduleAndUom Uom on se.ItemUOMId = uom.Id AND UOM.ItemId = SE.ItemId
		INNER JOIN ItemCategory ic on i.ItemCategoryId = ic.Id
		INNER JOIN InventoryParentCategories ipc on ic.InventoryParentCategoriesId = ipc.Id
		INNER JOIN V_UomScheduleAndUom RUom on se.UomScheduleIdRate = RUom.Id AND RUom.ItemId = SE.ItemId
		LEFT  JOIN City c on se.CityId = c.Id
		LEFT  JOIN Tehsil t on c.TehsilId = t.Id		
		LEFT  Join PartyCustomGroups ac on ac.SupplierCustomerId = h.SupplierCustomerId AND ac.companyId = h.CompanyId
		LEft Join SaleOrder So on So.Id = se.SaleOrderId
Where	h.OrganizationId = @OrganizationId 
		and h.CompanyId = @CompanyId
		and (@FromDate is null or h.DocDate >= @FromDate) 
		and (@ToDate is null or h.DocDate < @getToDate)
		and (@DocNoFrom is null or h.DocNo >= @DocNoFrom) 
		and (@DocNoTo is null or h.DocNo <=@DocNoTo)
		and (@InventoryParentCategories is null or ic.InventoryParentCategoriesId = @InventoryParentCategories)
		and	(@ItemCategoryId is null or i.ItemCategoryId = @ItemCategoryId)	
		and	(@ItemTypeId is null or i.ItemTypeId = @ItemTypeId)
		and	(@ItemClassGroupId is null or ic.ItemClassGroupId = @ItemClassGroupId)
		and (@ItemId is null or se.ItemId = @ItemId)
		and (@AccountId IS NULL OR i.PurchaseGLAC = @AccountId)	
		and	(@SaleAccountId IS NULL OR i.SaleGLAC  = @SaleAccountId)

		and	(@StockAccountId IS NULL OR i.PurchaseGLAC  = @StockAccountId)
		and	(@BookingPersonId IS NULL OR so.BookingPersonId  = @BookingPersonId)
		and (@CGSAccountId IS NULL OR i.COGSGLAC = @CGSAccountId)
		and (@PackUom is null or Uom.Equivalent = @PackUom)
		and (@CropYear is null or se.CropYear = @CropYear)
		and (@JobLotId is null or se.JobLotId=@JobLotId)
		and (@PackingTypeId is null or se.PackingTypeId = @PackingTypeId)
		and	(@SupplierCustomerId is null or h.SupplierCustomerId = @SupplierCustomerId)
		and (@WarehouseId is null or se.WarehouseId = @WarehouseId)
		and	(@CityId is null or se.CityId = @CityId)
		and	(@DistrictId is null or t.DistrictId = @DistrictId)
		and	(@RefPartyId is null or se.RefPartyId = @RefPartyId)
		and (h.DocumentTypeId in (95,96,99,100,103,184,186,171,222))
		and (@BranchesIds is null or EXISTS(SELECT 1 FROM #BrancheIds WHERE BranchId = h.BranchesId))
		and (@CostCenterId is null or se.CostCenterId = @CostCenterId)
		and (@OtherCategoryId IS NULL OR (CASE WHEN H.OtherCategoryId = 0 THEN 54 ELSE ISNULL(h.OtherCategoryId,54) END) = @OtherCategoryId)
		and (@AccountsCustomGroupsId IS NULL OR ac.CustomGroupId = @AccountsCustomGroupsId)
		and	(
					@CustomGroupIds IS NULL
					OR EXISTS (select 1
								from dbo.fnSplitString (@CustomGroupIds,',')
								Where ac.CustomGroupId in (data)
								)
			)
		and (@ActionId IS NULL OR @ActionId = 1) -- SALES 
		--AND h.Id = 112338



Insert Into #SaleRegister		
Select	h.DocumentTypeId as RefDocumentTypeId, 
		0 AS OtherCategoryId,
		h.Id as RefDocIdNo, 
		se.Id as DetailId,
		h.SupplierCustomerId,0,
		0 CityId, 
		convert(date,h.DocDate) as DocDate, 
		h.DocNo as DocCodeNo, 
		se.WarehouseId,	
		se.VehicleNo, 
		se.GpNo as GpNoDcNo,	
		ic.InventoryParentCategoriesId, 
		i.ItemCategoryId, 
		se.ItemId,
		i.ItemCode,
		I.ItemName,
		i.HsCode,
		se.ItemUOMId as ItemUom,
		Uom.UOMCode,
		Uom.Equivalent,
		se.CropYear as CropBatch, 
		se.JobLotId, 
		se.PackingTypeId as InvPackingTypeId,	
		se.ItemQty as QtyOut,
		se.NetBillWeight as BillWeightOut, 
		se.NetStockWeight as StockWeightOut,
		se.ItemAmount ,
		se.BillAmount as AmountOut,
		CAST(0.00 AS decimal(18,2)) AS CGSAmount,
		se.ItemRate,
		se.ItemCgsRate,
		SE.UomScheduleIdRate as RateUom,
		RUom.Equivalent as RateEquivalent,
		0 RefPartyId,
		0 InvGdnId,
		0 InvGdnDetailId,
		0 GdnNo,
		convert(date,'') GdnDate,
		0 SoNo,
		convert(date,'') SoDate,
		h.BranchSrNo,
		CAST('' as nvarchar(max)) TicketNos,
		CASE WHEN @BranchesIds IS NULL THEN 0 ELSE h.BranchesId END AS BranchesId,se.CostCenterId,h.ManualBillNo,
		se.ExpenseAmount,
		se.FreightAmount,
		se.CommissionAmount,se.RateCut,se.RateCutAmount
from	InvPurchaseInvoiceDetail se
		INNER JOIN InvPurchaseInvoice h on se.InvPurchaseInvoiceId = h.Id 
		INNER JOIN Item i on se.ItemId = i.id
		INNER JOIN V_UomScheduleAndUom Uom on se.ItemUOMId = uom.Id AND UOM.ItemId = SE.ItemId
		INNER JOIN ItemCategory ic on i.ItemCategoryId = ic.Id
		INNER JOIN InventoryParentCategories ipc on ic.InventoryParentCategoriesId = ipc.Id
		INNER JOIN V_UomScheduleAndUom RUom on se.UomScheduleIdRate = RUom.Id AND RUom.ItemId = SE.ItemId
		LEFT  Join PartyCustomGroups ac on ac.SupplierCustomerId = h.SupplierCustomerId and ac.companyId = h.CompanyId
Where	h.OrganizationId = @OrganizationId 
		and h.CompanyId = @CompanyId
		and (@FromDate is null or h.DocDate >= @FromDate) 
		and (@ToDate is null or h.DocDate < @getToDate)
		and (@DocNoFrom is null or h.DocNo >= @DocNoFrom) 
		and (@DocNoTo is null or h.DocNo <=@DocNoTo)
		and (@InventoryParentCategories is null or ic.InventoryParentCategoriesId = @InventoryParentCategories)
		and	(@ItemCategoryId is null or i.ItemCategoryId = @ItemCategoryId)	
		and	(@ItemTypeId is null or i.ItemTypeId = @ItemTypeId)
		and	(@ItemClassGroupId is null or ic.ItemClassGroupId = @ItemClassGroupId)
		and (@ItemId is null or se.ItemId = @ItemId)
		and (@AccountId IS NULL OR i.PurchaseGLAC = @AccountId)
		and	(@SaleAccountId IS NULL OR i.SaleGLAC  = @SaleAccountId)
		and (@CGSAccountId IS NULL OR i.COGSGLAC = @CGSAccountId)
		and (@PackUom is null or Uom.Equivalent = @PackUom)
		and (@CropYear is null or se.CropYear = @CropYear)
		and (@JobLotId is null or se.JobLotId=@JobLotId)
		and (@PackingTypeId is null or se.PackingTypeId = @PackingTypeId)
		and	(@SupplierCustomerId is null or h.SupplierCustomerId = @SupplierCustomerId)
		and (@WarehouseId is null or se.WarehouseId = @WarehouseId)
		and (h.DocumentTypeId = 98)
		and (@BranchesIds is null or EXISTS(SELECT 1 FROM #BrancheIds WHERE BranchId = h.BranchesId))
		and (@CostCenterId is null or se.CostCenterId = @CostCenterId)
		and (@AccountsCustomGroupsId IS NULL OR ac.CustomGroupId = @AccountsCustomGroupsId)
		and	(
				@CustomGroupIds IS NULL
				OR EXISTS (select 1
							from dbo.fnSplitString (@CustomGroupIds,',')
							Where ac.CustomGroupId in (data)
						  )
			)
		and @ActionId = 2 -- SALES RETURN 


UPDATE R SET R.CGSAmount = T.CgsAmount,
			 R.ItemCgsRate = T.CGSRate * R.RateEquivalent
FROM #SaleRegister R
		INNER JOIN (SELECT RefDocumentTypeId,
						   RefDocIdNo,
						   RefDocSubIdNo,
						   SUM(CgsAmount) AS CgsAmount,
						   CASE WHEN SUM(CgsAmount) > 0 AND SUM(StockWeightOut) > 0 
								THEN ROUND(SUM(CgsAmount) / SUM(StockWeightOut),2) 
								ELSE 0 
						   END AS CGSRate
					FROM InventoryStockEvalautionDetail
					GROUP BY RefDocumentTypeId,
							 RefDocIdNo,
							 RefDocSubIdNo 
				   ) T ON  R.RefDocumentTypeId = T.RefDocumentTypeId 
					   AND R.RefDocIdNo = T.RefDocIdNo 
					   AND R.DetailId = T.RefDocSubIdNo
WHERE ISNULL(@SaleCostingJobOrderWise,0) = 0



--=============================================================================
if (@ActivityName = 'Sales Register')
Begin
--========================================UPDATE-------------------------
Select 
		se.RefDocumentTypeId,se.RefDocIdNo,gdh.DocNo GdnNo,gdh.DocDate GdnDate,
		Soh.DocNo SoNo,Soh.DocDate SoDate,gp.GpSrNo
Into #Dataaa
from		InvGdn gdh 
Inner Join	InvgdnDetail gdd on gdd.InvGdnId=Gdh.Id 
Inner Join  GatePassOutward gp on gp.Id=gdh.OutwardGatePassId
Inner Join	SaleOrder Soh on Soh.Id=gdd.SaleOrderId
Inner Join	#SaleRegister se on gdd.InvGdnId=se.InvGdnId and gdd.Id=se.InvGdnDetailId
Where		se.RefDocumentTypeId=95
Update se
Set		se.GdnNo =d.GdnNo,
		se.GdnDate=d.GdnDate,
		se.SoNo=d.SoNo,
		se.SoDate=d.SoDate,
		se.GpNoDcNo=d.GpSrNo
From #SaleRegister se
Inner join #Dataaa d on se.RefDocumentTypeId=d.RefDocumentTypeId and d.RefDocIdNo=se.RefDocIdNo
Where		se.RefDocumentTypeId=95

--=====================================Update TicketNo
Select
    DistinctTickets.InvGdnId,DistinctTickets.InvGdnDetailId,
    STRING_AGG(TicketNo, ',') AS TicketNos
Into #WbUpdate
from (
    Select DISTINCT
        gdd.InvGdnId,se.InvGdnDetailId,
        wbT.TicketNo
    from		InvGdn gdh 
	Inner Join	InvgdnDetail gdd on gdd.InvGdnId=Gdh.Id 
	Inner Join  GatePassOutward gp on gp.Id=gdh.OutwardGatePassId
	Inner Join	#SaleRegister se on gdd.InvGdnId=se.InvGdnId and gdd.Id=se.InvGdnDetailId
	Inner Join  WbTransactions wbT on gdh.OutwardGatePassId = wbT.ReferenceDocNoId and wbT.ReferenceDocTypeId = GP.DocumentTypeId
) AS DistinctTickets
Group By
    DistinctTickets.InvGdnId,DistinctTickets.InvGdnDetailId

Update R
Set R.TicketNos=D.TicketNos
From #SaleRegister R
Inner Join #WbUpdate D on R.InvGdnId = D.InvGdnId and D.InvGdnDetailId = R.InvGdnDetailId
--=====================================================================================
Insert into #InvSalesRegisterTemp 
			(RefDocIdNo,DocDate,RefDocumentTypeId, DocumentTypeCode, DocCodeNo,Customer,ReferencePartyName, ItemCode, ItemName, UOMCode, CropBatch, JobLotCode, PackTypeCode
			,VehicleNo,CityName,WarehouseName, QtyOut, BillWeightOut,StockWeight, AmountOut 
			,AvgRate,AvgRate40Kg,Expenses,Freight,Commission
			,PrcntOfTotal,PrcntOfTotalWeight,GdnNo,GdnDate,SoNo,SoDate,GpNoDcNo,BranchSrNo,ItemAmountWithoutExpense,TicketNos
			,BranchesId,costcenterid,costcentername,ReferenceNo,HsCode,ItemRate,RateCut,RateCutAmount,OtherCategoryId,BookingPersonId,BookingPersonName
			)

Select		 sr.RefDocIdNo,sr.DocDate,sr.RefDocumentTypeId,dt.DocumentTypeCode, sr.DocCodeNo,
				sp.CompanyName as Customer,rp.ReferencePartyName,sr.ItemCode,sr.ItemName,sr.UOMCode, sr.CropBatch, jb.JobLotCode
			,pt.PackTypeCode, sr.VehicleNo,c.Description as CityName
			,Wh.WareHouseName
			
			,CASE WHEN SR.RefDocumentTypeId IN (98) THEN -SR.QtyOut ELSE sr.QtyOut END
			,CASE WHEN SR.RefDocumentTypeId IN (98) THEN -SR.BillWeightOut ELSE sr.BillWeightOut END
			,CASE WHEN SR.RefDocumentTypeId IN (98) THEN -SR.StockWeightOut ELSE sr.StockWeightOut END
			,CASE WHEN SR.RefDocumentTypeId IN (98) THEN -SR.ItemAmount ELSE sr.AmountOut END
			,Case when sr.BillWeightOut > 0 and sr.AmountOut > 0 then sr.AmountOut / sr.BillWeightOut * sr.Equivalent Else 0 End AvgRate
			,Case when sr.BillWeightOut > 0 and sr.AmountOut > 0 then sr.AmountOut / sr.BillWeightOut * 40 Else 0 End AvgRate40Kg
			,CASE WHEN SR.RefDocumentTypeId IN (98) THEN -SR.OtherAmount ELSE sr.OtherAmount END OtherAmount
			,CASE WHEN SR.RefDocumentTypeId IN (98) THEN -SR.FreightAmount ELSE sr.FreightAmount END FreightAmount
			,CASE WHEN SR.RefDocumentTypeId IN (98) THEN -SR.CommissionAmount ELSE sr.CommissionAmount END CommissionAmount
			,sr.AmountOut / sum(sr.AmountOut) over() * 100 as PrcntOfTotal
			,sr.BillWeightOut / sum(sr.BillWeightOut) over () * 100 as PrctofTotalWeight
			,sr.GdnNo,sr.GdnDate,sr.SoNo,sr.SoDate,sr.GpNoDcNo,sr.BranchSrNo,sr.ItemAmount,sr.TicketNos
			,sr.BranchesId,sr.CostCenterId,CC.CostCenterName,sr.ManualBillNo,sr.HSCode
			,sr.ItemRate
			,sr.RateCut
			,sr.RateCutAmount
			,SR.OtherCategoryId,SR.BookingPersonId,bp.ReferencePartyName
from		#SaleRegister sr
			INNER JOIN DocumentType dt on sr.RefDocumentTypeId = dt.Id
			INNER JOIN InvWareHouse wh on sr.WarehouseId = wh.Id
			LEFT  JOIN SupplierCustomer sp on sr.SupplierCustomerId = sp.Id
			LEFT  JOIN InvPackingType pt on sr.InvPackingTypeId = pt.Id
			LEFT  JOIN JobLot jb on sr.JobLotId = jb.Id
			LEFT  JOIN City c on sr.CityId = c.Id
			LEFT  JOIN ReferenceParties rp on sr.RefPartyId = rp.Id			
			LEFT  JOIN CostCenter CC on sr.CostCenterId = CC.Id
LEFT Join   ReferenceParties BP on BP.Id = sr.BookingPersonId
	Order by sr.DocDate asc 
End	

IF @ActivityName = 'INVOICE WISE PROFITABLITY'
BEGIN
	Select 
		se.RefDocumentTypeId,
		se.RefDocIdNo,
		gdh.DocNo as GdnNo,
		gdh.DocDate as GdnDate,
		Soh.DocNo as SoNo,
		Soh.DocDate as SoDate,
		gp.GpSrNo
	Into #GdnData
	from  InvGdn gdh 
			Inner Join	InvgdnDetail gdd on gdd.InvGdnId=Gdh.Id and gdd.ActionTypeId <> 3
			Inner Join  GatePassOutward gp on gp.Id=gdh.OutwardGatePassId
			Inner Join	SaleOrder Soh on Soh.Id=gdd.SaleOrderId and soh.ActionId <> 3
			Inner Join	#SaleRegister se on gdd.InvGdnId=se.InvGdnId and gdd.Id=se.InvGdnDetailId
	Where	se.RefDocumentTypeId=95

	Update se
	Set		se.GdnNo =d.GdnNo,
			se.GdnDate=d.GdnDate,
			se.SoNo=d.SoNo,
			se.SoDate=d.SoDate,
			se.GpNoDcNo=d.GpSrNo
	From #SaleRegister se
			Inner join #GdnData d on se.RefDocumentTypeId=d.RefDocumentTypeId and d.RefDocIdNo=se.RefDocIdNo
	Where	se.RefDocumentTypeId=95


	Insert into #InvSalesRegisterTemp 
			(RefDocIdNo,
			 DocDate,
			 DocCodeNo,
			 GdnNo,
			 GdnDate,
			 SoNo,
			 SoDate,
			 RefDocumentTypeId, 
			 DocumentTypeCode, 
			 Customer,
			 ItemCode, 
			 ItemName, 
			 HsCode,
			 UOMCode, 
			 CropBatch, 
			 JobLotId,
			 JobLotCode, 
			 JobOrderNo,
			 VehicleNo,
			 WarehouseName, 
			 AvgRate,
			 AvgRate40Kg,
			 QtyOut, 
			 BillWeightOut, 
			 ItemAmountWithoutExpense,
			 CgsRate,
			 ProfitLossPerRate,
			 ProfitLoss,
			 ProfitLossPercent,
			 Expenses,
			 Freight,
			 Commission,
			 AmountOut,
			 PrcntOfTotal,
			 PrcntOfTotalWeight,
			 GpNoDcNo,
			 BranchSrNo,
			 BranchesId,
			 ReferenceNo,BookingPersonId,BookingPersonName
			)
	SELECT  
			sr.RefDocIdNo,
			sr.DocDate,
			sr.DocCodeNo,
			sr.GdnNo,
			sr.GdnDate,
			sr.SoNo,
			sr.SoDate,
			sr.RefDocumentTypeId,
			dt.DocumentTypeCode, 
			sp.CompanyName as Customer,
			sr.ItemCode,
			sr.ItemName,
			sr.HSCode,
			sr.UOMCode, 
			sr.CropBatch, 
			SR.JobLotId,
			jb.JobLotCode,
			J.RefInvoiceNo AS JobOrderNo,
			sr.VehicleNo
			,Wh.WareHouseName
			,sr.ItemRate as SaleRate
			,Case when sr.StockWeightOut > 0 and sr.ItemAmount > 0 then round(sr.ItemAmount/sr.StockWeightOut * 40,2) Else 0 End AvgRate40Kg
			,sr.QtyOut AS SaleQty
			,sr.BillWeightOut as SaleWeight
			,sr.ItemAmount as SaleAmount
			,sr.ItemCgsRate as CgsRate
			,sr.ItemRate - sr.ItemCgsRate as ProfitLossPerRate
			,(sr.StockWeightOut / sr.RateEquivalent) * (sr.ItemRate - sr.ItemCgsRate) as ProfitLoss
			,CASE WHEN (sr.StockWeightOut / sr.RateEquivalent) * (sr.ItemRate - sr.ItemCgsRate) <> 0
				  THEN (sr.StockWeightOut / sr.RateEquivalent) * (sr.ItemRate - sr.ItemCgsRate) / sr.ItemAmount * 100 
				  ELSE 0 
			 END AS ProfitLossPercent
			,sr.OtherAmount
			,sr.FreightAmount
			,sr.CommissionAmount
			,sr.AmountOut
			,sr.ItemAmount / sum(sr.ItemAmount) over() * 100 as PrcntOfTotal
			,sr.StockWeightOut / sum(sr.StockWeightOut) over () * 100 as PrctofTotalWeight,
			sr.GpNoDcNo,
			sr.BranchSrNo,
			sr.BranchesId,
			sr.ManualBillNo,sr.BookingPersonId,bp.ReferencePartyName
from		#SaleRegister sr
			INNER JOIN DocumentType dt on sr.RefDocumentTypeId = dt.Id
			INNER JOIN InvWareHouse wh on sr.WarehouseId = wh.Id
			INNER JOIN SupplierCustomer sp on sr.SupplierCustomerId = sp.Id
			INNER JOIN JobLot jb on sr.JobLotId = jb.Id
			LEFT  JOIN InvProductionJobOrder J ON JB.RefDocNoId = J.Id AND JB.RefDocumentTypeId = J.DocumentTypeId
			
LEFT Join   ReferenceParties BP on BP.Id = sr.BookingPersonId
	Order by sr.DocDate asc 
END

if (@ActivityName = 'Sales Summary By Item')
	Begin
			Insert into #InvSalesRegisterTemp 
						(BranchesId,CityName,ItemCode, ItemName,HsCode,UOMCode, QtyOut, BillWeightOut, AmountOut, AvgRate,AvgRate40Kg,PrcntOfTotal,PrcntOfTotalWeight)
			Select		sr.BranchesId,'****',sr.ItemCode, sr.ItemName,sr.HSCode,0 UOMCode, sum(sr.QtyOut) QtyOut, sum(sr.BillWeightOut) BillWeightOut, sum(sr.AmountOut) AmountOut
						, 0 As AvgRate
						, Case when sum(sr.BillWeightOut) > 0 and sum(sr.AmountOut) > 0 then round((sum(sr.AmountOut) / sum(sr.BillWeightOut) ) * 40,  2) Else 0 End AvgRate40Kg
						, sum(sr.AmountOut) / sum(sum(sr.AmountOut)) over () * 100 as PrctofTotal
						, sum(sr.BillWeightOut) / sum(sum(sr.BillWeightOut)) over () * 100 as PrctofTotalWeight
			from		#SaleRegister sr
			WHERE (@ActionId IS NULL 
				   OR 
				   (@ActionId = 1 AND SR.RefDocumentTypeId <> 98) -- SALES
				   OR
				   (@ActionId = 2 AND SR.RefDocumentTypeId = 98)  -- SALES RETURN
				  )
			Group By	sr.ItemCode, sr.ItemName,sr.BranchesId,sr.HSCode
End
if (@ActivityName = 'Sales Summary By Item & Warehouse')
	Begin

			Insert into #InvSalesRegisterTemp 
						(BranchesId,CityName,WarehouseName, ItemCode, ItemName,UOMCode, QtyOut, BillWeightOut, AmountOut, AvgRate, AvgRate40Kg
						,PrcntOfTotal,PrcntOfTotalWeight)

			Select		sr.BranchesId,'****',wh.WareHouseName, sr.ItemCode, sr.ItemName,0 UOMCode, sum(sr.QtyOut) QtyOut, sum(sr.BillWeightOut) BillWeightOut, sum(sr.AmountOut) AmountOut
						, 0 As AvgRate
						, Case when sum(sr.BillWeightOut) > 0 and sum(sr.AmountOut) > 0 then round((sum(sr.AmountOut) / sum(sr.BillWeightOut) ) * 40,  2) Else 0 End AvgRate40Kg
						, sum(sr.AmountOut) / sum(sum(sr.AmountOut)) over () * 100 as PrctofTotal
						, sum(sr.BillWeightOut) / sum(sum(sr.BillWeightOut)) over () * 100 as PrctofTotalWeight
			from		#SaleRegister sr
						INNER JOIN InvWareHouse wh on sr.WarehouseId = wh.Id
		WHERE (@ActionId IS NULL 
			   OR 
			   (@ActionId = 1 AND SR.RefDocumentTypeId <> 98) -- SALES
			   OR
			   (@ActionId = 2 AND SR.RefDocumentTypeId = 98)  -- SALES RETURN
			  )
		GROUP BY sr.ItemCode,sr.ItemName, WareHouseName,sr.BranchesId

End		

if (@ActivityName = 'Sales Summary By Item & City')
	Begin
			Insert into #InvSalesRegisterTemp 
						(BranchesId,CityName,ItemCode, ItemName,UOMCode, QtyOut, BillWeightOut, AmountOut, AvgRate,AvgRate40Kg,PrcntOfTotal,PrcntOfTotalWeight)
			Select		sr.BranchesId,C.Description,sr.ItemCode,sr.ItemName,0 UOMCode, sum(sr.QtyOut) QtyOut, sum(sr.BillWeightOut) BillWeightOut, sum(sr.AmountOut) AmountOut
						, 0 As AvgRate
						, Case when sum(sr.BillWeightOut) > 0 and sum(sr.AmountOut) > 0 then round((sum(sr.AmountOut) / sum(sr.BillWeightOut) ) * 40,  2) Else 0 End AvgRate40Kg
						, sum(sr.AmountOut) / sum(sum(sr.AmountOut)) over () * 100 as PrctofTotal
						, sum(sr.BillWeightOut) / sum(sum(sr.BillWeightOut)) over () * 100 as PrctofTotalWeight
			from		#SaleRegister sr
						LEFT  JOIN City c on sr.CityId = c.Id
		WHERE (@ActionId IS NULL 
			   OR 
			   (@ActionId = 1 AND SR.RefDocumentTypeId <> 98) -- SALES
			   OR
			   (@ActionId = 2 AND SR.RefDocumentTypeId = 98)  -- SALES RETURN
			  )
			Group By	C.Description,sr.ItemCode, sr.ItemName,sr.BranchesId
End
if (@ActivityName = 'Sales Summary By Item & Pack Size')
	Begin
			Insert into #InvSalesRegisterTemp 
						(BranchesId,CityName,ItemCode, ItemName,UOMCode, QtyOut, BillWeightOut, AmountOut, AvgRate,AvgRate40Kg,PrcntOfTotal,PrcntOfTotalWeight)
			Select		sr.BranchesId,'****',sr.ItemCode, sr.ItemName,sr.UOMCode, sum(sr.QtyOut) QtyOut, sum(sr.BillWeightOut) BillWeightOut, sum(sr.AmountOut) AmountOut
						, Case when sum(sr.BillWeightOut) > 0 and sum(sr.AmountOut) > 0 then round((sum(sr.AmountOut) / sum(sr.BillWeightOut) ) * sr.Equivalent,  2) Else 0 End AvgRate
						, Case when sum(sr.BillWeightOut) > 0 and sum(sr.AmountOut) > 0 then round((sum(sr.AmountOut) / sum(sr.BillWeightOut) )* 40,  2) Else 0 End AvgRate40Kg
						, sum(sr.AmountOut) / sum(sum(sr.AmountOut)) over () * 100 as PrctofTotal
						, sum(sr.BillWeightOut) / sum(sum(sr.BillWeightOut)) over () * 100 as PrctofTotalWeight
			from		#SaleRegister sr
		WHERE (@ActionId IS NULL 
			   OR 
			   (@ActionId = 1 AND SR.RefDocumentTypeId <> 98) -- SALES
			   OR
			   (@ActionId = 2 AND SR.RefDocumentTypeId = 98)  -- SALES RETURN
			  )
			Group By	sr.ItemCode, sr.ItemName, sr.Equivalent,sr.UOMCode,sr.BranchesId
End	
if (@ActivityName = 'Sales Summary By Item,Pack Size & City')
	Begin
			Insert into #InvSalesRegisterTemp 
						(BranchesId,CityName,ItemCode, ItemName,UOMCode, QtyOut, BillWeightOut, AmountOut, AvgRate,AvgRate40Kg,PrcntOfTotal,PrcntOfTotalWeight)
			Select		sr.BranchesId,c.Description,sr.ItemCode, sr.ItemName,sr.UOMCode, sum(sr.QtyOut) QtyOut, sum(sr.BillWeightOut) BillWeightOut, sum(sr.AmountOut) AmountOut
						, Case when sum(sr.BillWeightOut) > 0 and sum(sr.AmountOut) > 0 then round((sum(sr.AmountOut) / sum(sr.BillWeightOut) ) * sr.Equivalent,  2) Else 0 End AvgRate
						, Case when sum(sr.BillWeightOut) > 0 and sum(sr.AmountOut) > 0 then round((sum(sr.AmountOut) / sum(sr.BillWeightOut) )* 40,  2) Else 0 End AvgRate40Kg
						, sum(sr.AmountOut) / sum(sum(sr.AmountOut)) over () * 100 as PrctofTotal
						, sum(sr.BillWeightOut) / sum(sum(sr.BillWeightOut)) over () * 100 as PrctofTotalWeight
			from		#SaleRegister sr
						LEFT  JOIN City c on sr.CityId = c.Id
		WHERE (@ActionId IS NULL 
			   OR 
			   (@ActionId = 1 AND SR.RefDocumentTypeId <> 98) -- SALES
			   OR
			   (@ActionId = 2 AND SR.RefDocumentTypeId = 98)  -- SALES RETURN
			  )
			Group By	C.Description,sr.ItemCode,sr.ItemName, sr.Equivalent,sr.UOMCode,sr.BranchesId
End	

if (@ActivityName = 'Sales Summary By Customer')
	Begin
			Insert into #InvSalesRegisterTemp 
						(BranchesId,CityName,Customer,UOMCode, QtyOut, BillWeightOut, AmountOut, AvgRate,AvgRate40Kg,PrcntOfTotal,PrcntOfTotalWeight)

			Select		sr.BranchesId,'****',sp.CompanyName,0 AS UOMCode,sum(sr.QtyOut) QtyOut, sum(sr.BillWeightOut) BillWeightOut, sum(sr.AmountOut) AmountOut
						,0 As AvgRate
						, Case when sum(sr.BillWeightOut) > 0 and sum(sr.AmountOut) > 0 
							   then round((sum(sr.AmountOut) / sum(sr.BillWeightOut))  * 40, 2) 
							   Else 0 
						  End AvgRate40Kg
						, sum(sr.AmountOut) / sum(sum(sr.AmountOut)) over () * 100 as PrctofTotal
						, sum(sr.BillWeightOut) / sum(sum(sr.BillWeightOut)) over () * 100 as PrctofTotalWeight
			from		#SaleRegister sr
						INNER JOIN SupplierCustomer sp on sr.SupplierCustomerId = sp.Id
		WHERE (@ActionId IS NULL 
			   OR 
			   (@ActionId = 1 AND SR.RefDocumentTypeId <> 98) -- SALES
			   OR
			   (@ActionId = 2 AND SR.RefDocumentTypeId = 98)  -- SALES RETURN
			  )
			Group By	sp.CompanyName,sr.BranchesId
End	
if (@ActivityName = 'Sales Summary By Customer & ReferenceParty')
	Begin
			Insert into #InvSalesRegisterTemp 
						(BranchesId,ReferencePartyName,Customer,UOMCode, QtyOut, BillWeightOut, AmountOut, AvgRate,AvgRate40Kg,PrcntOfTotal,PrcntOfTotalWeight)
			Select		sr.BranchesId,C.ReferencePartyName,sp.CompanyName,iu.UOMCode, sum(sr.QtyOut) QtyOut, sum(sr.BillWeightOut) BillWeightOut, sum(sr.AmountOut) AmountOut
						, Case when sum(sr.BillWeightOut) > 0 and sum(sr.AmountOut) > 0 then round((sum(sr.AmountOut) / sum(sr.BillWeightOut) ) * iu.Equivalent,  2) Else 0 End AvgRate 
						, Case when sum(sr.BillWeightOut) > 0 and sum(sr.AmountOut) > 0 then round((sum(sr.AmountOut) / sum(sr.BillWeightOut))  * 40, 2) Else 0 End AvgRate40Kg
						, sum(sr.AmountOut) / sum(sum(sr.AmountOut)) over () * 100 as PrctofTotal
						, sum(sr.BillWeightOut) / sum(sum(sr.BillWeightOut)) over () * 100 as PrctofTotalWeight
			from		#SaleRegister sr
						INNER JOIN V_UomScheduleAndUom iu on sr.ItemUom = iu.Id
						INNER JOIN SupplierCustomer sp on sr.SupplierCustomerId = sp.Id
						LEFT  JOIN ReferenceParties c on sr.RefPartyId = c.Id
		WHERE (@ActionId IS NULL 
			   OR 
			   (@ActionId = 1 AND SR.RefDocumentTypeId <> 98) -- SALES
			   OR
			   (@ActionId = 2 AND SR.RefDocumentTypeId = 98)  -- SALES RETURN
			  )
			Group By	C.ReferencePartyName,sp.CompanyName, iu.Equivalent,iu.UOMCode,sr.BranchesId
End	
if (@ActivityName = 'Sales Summary By Customer & Item')
Begin
			Insert into #InvSalesRegisterTemp 
						(BranchesId,CityName,Customer, itemcode,UOMCode,ItemName ,QtyOut, BillWeightOut, AmountOut,
						 AvgRate,AvgRate40Kg,PrcntOfTotal,PrcntOfTotalWeight)
			Select		sr.BranchesId,'****',sp.CompanyName, sr.ItemCode,sr.UOMCode, sr.ItemName,sum(sr.QtyOut) QtyOut, sum(sr.BillWeightOut) BillWeightOut, sum(sr.AmountOut) AmountOut
						, Case when sum(sr.BillWeightOut) > 0 and sum(sr.AmountOut) > 0 then round((sum(sr.AmountOut) / sum(sr.BillWeightOut) ) * sr.Equivalent,  2) Else 0 End AvgRate
						, Case when sum(sr.BillWeightOut) > 0 and sum(sr.AmountOut) > 0 
							   then round((sum(sr.AmountOut) / sum(sr.BillWeightOut) )* 40,  2) 
							   Else 0
						  End AvgRate40Kg
						, sum(sr.AmountOut) / sum(sum(sr.AmountOut)) over () * 100 as PrctofTotal
						, sum(sr.BillWeightOut) / sum(sum(sr.BillWeightOut)) over () * 100 as PrctofTotalWeight
			from		#SaleRegister sr
						INNER JOIN SupplierCustomer sp on sr.SupplierCustomerId = sp.Id
		WHERE (@ActionId IS NULL 
			   OR 
			   (@ActionId = 1 AND SR.RefDocumentTypeId <> 98) -- SALES
			   OR
			   (@ActionId = 2 AND SR.RefDocumentTypeId = 98)  -- SALES RETURN
			  )
			Group By	sp.CompanyName, sr.ItemCode, sr.ItemName,sr.Equivalent,sr.UOMCode,sr.BranchesId
End	
if (@ActivityName = 'Sales Summary By Customer & City')
	Begin
			Insert into #InvSalesRegisterTemp 
						(BranchesId,CityName,Customer,UOMCode, QtyOut, BillWeightOut, AmountOut, AvgRate,AvgRate40Kg,PrcntOfTotal,PrcntOfTotalWeight)
			Select		sr.BranchesId,C.Description,sp.CompanyName,sr.UOMCode, sum(sr.QtyOut) QtyOut, sum(sr.BillWeightOut) BillWeightOut, sum(sr.AmountOut) AmountOut
						, Case when sum(sr.BillWeightOut) > 0 and sum(sr.AmountOut) > 0
							   then round((sum(sr.AmountOut) / sum(sr.BillWeightOut) ) * sr.Equivalent,  2) Else 0 End AvgRate
						, Case when sum(sr.BillWeightOut) > 0 and sum(sr.AmountOut) > 0 
						       then round((sum(sr.AmountOut) / sum(sr.BillWeightOut))  * 40, 2) Else 0 End AvgRate40Kg

						, sum(sr.AmountOut) / sum(sum(sr.AmountOut)) over () * 100 as PrctofTotal
						, sum(sr.BillWeightOut) / sum(sum(sr.BillWeightOut)) over () * 100 as PrctofTotalWeight
			from		#SaleRegister sr
						INNER JOIN SupplierCustomer sp on sr.SupplierCustomerId = sp.Id
						LEFT  JOIN City c on sr.CityId = c.Id
		WHERE (@ActionId IS NULL 
			   OR 
			   (@ActionId = 1 AND SR.RefDocumentTypeId <> 98) -- SALES
			   OR
			   (@ActionId = 2 AND SR.RefDocumentTypeId = 98)  -- SALES RETURN
			  )
			Group By	C.Description,sp.CompanyName, sr.Equivalent,sr.UOMCode,sr.BranchesId
End	
if (@ActivityName = 'Sales Summary By Customer & Pack Size')
	Begin
			Insert into #InvSalesRegisterTemp 
						(BranchesId,Customer,UOMCode, QtyOut, BillWeightOut, AmountOut, AvgRate,AvgRate40Kg,PrcntOfTotal,PrcntOfTotalWeight)
			Select		sr.BranchesId,sp.CompanyName,sr.UOMCode, sum(sr.QtyOut) QtyOut, sum(sr.BillWeightOut) BillWeightOut, sum(sr.AmountOut) AmountOut
						, Case when sum(sr.BillWeightOut) > 0 and sum(sr.AmountOut) > 0
						       then round((sum(sr.AmountOut) / sum(sr.BillWeightOut) ) * sr.Equivalent,  2) Else 0 End AvgRate
						, Case when sum(sr.BillWeightOut) > 0 and sum(sr.AmountOut) > 0 
						       then round((sum(sr.AmountOut) / sum(sr.BillWeightOut))  * 40, 2) Else 0 End AvgRate40Kg
						, sum(sr.AmountOut) / sum(sum(sr.AmountOut)) over () * 100 as PrctofTotal
						, sum(sr.BillWeightOut) / sum(sum(sr.BillWeightOut)) over () * 100 as PrctofTotalWeight
			from		#SaleRegister sr
						INNER JOIN SupplierCustomer sp on sr.SupplierCustomerId = sp.Id
		WHERE (@ActionId IS NULL 
			   OR 
			   (@ActionId = 1 AND SR.RefDocumentTypeId <> 98) -- SALES
			   OR
			   (@ActionId = 2 AND SR.RefDocumentTypeId = 98)  -- SALES RETURN
			  )
			Group By	sp.CompanyName, sr.Equivalent,sr.UOMCode,sr.BranchesId
End	
if (@ActivityName = 'Sales Summary By Customer,Item & City')
	Begin
			Insert into #InvSalesRegisterTemp 
						(BranchesId,CityName,Customer, itemcode,UOMCode,ItemName ,QtyOut, BillWeightOut, AmountOut,
						 AvgRate,AvgRate40Kg,PrcntOfTotal,PrcntOfTotalWeight)
			Select		sr.BranchesId,C.Description,sp.CompanyName, sr.ItemCode,sr.UOMCode, sr.ItemName,sum(sr.QtyOut) QtyOut, sum(sr.BillWeightOut) BillWeightOut, sum(sr.AmountOut) AmountOut
						, Case when sum(sr.BillWeightOut) > 0 and sum(sr.AmountOut) > 0 then round((sum(sr.AmountOut) / sum(sr.BillWeightOut) ) * sr.Equivalent,  2) Else 0 End AvgRate
						, Case when sum(sr.BillWeightOut) > 0 and sum(sr.AmountOut) > 0 
						       then round((sum(sr.AmountOut) / sum(sr.BillWeightOut) )* 40,  2) Else 0 End AvgRate40Kg
						, sum(sr.AmountOut) / sum(sum(sr.AmountOut)) over () * 100 as PrctofTotal
						, sum(sr.BillWeightOut) / sum(sum(sr.BillWeightOut)) over () * 100 as PrctofTotalWeight
			from		#SaleRegister sr
						INNER JOIN SupplierCustomer sp on sr.SupplierCustomerId = sp.Id
						LEFT  JOIN City c on sr.CityId = c.Id
		WHERE (@ActionId IS NULL 
			   OR 
			   (@ActionId = 1 AND SR.RefDocumentTypeId <> 98) -- SALES
			   OR
			   (@ActionId = 2 AND SR.RefDocumentTypeId = 98)  -- SALES RETURN
			  )
			Group By	C.Description,sp.CompanyName, sr.ItemCode, sr.ItemName,
						sr.Equivalent,sr.UOMCode,sr.BranchesId
End	
if (@ActivityName = 'Sales Summary By Customer,Item & ReferenceParty')
	Begin
			Insert into #InvSalesRegisterTemp 
						(sr.BranchesId,ReferencePartyName,Customer, itemcode,UOMCode,ItemName ,QtyOut, BillWeightOut, AmountOut, AvgRate,AvgRate40Kg,PrcntOfTotal,PrcntOfTotalWeight)
			Select		sr.BranchesId,C.ReferencePartyName,sp.CompanyName,sr.ItemCode,sr.UOMCode,sr.ItemName,sum(sr.QtyOut) QtyOut, sum(sr.BillWeightOut) BillWeightOut, sum(sr.AmountOut) AmountOut
						, Case when sum(sr.BillWeightOut) > 0 and sum(sr.AmountOut) > 0 then round((sum(sr.AmountOut) / sum(sr.BillWeightOut) ) * sr.Equivalent,  2) Else 0 End AvgRate
						, Case when sum(sr.BillWeightOut) > 0 and sum(sr.AmountOut) > 0 then round((sum(sr.AmountOut) / sum(sr.BillWeightOut) )* 40,  2) Else 0 End AvgRate40Kg
						, sum(sr.AmountOut) / sum(sum(sr.AmountOut)) over () * 100 as PrctofTotal
						, sum(sr.BillWeightOut) / sum(sum(sr.BillWeightOut)) over () * 100 as PrctofTotalWeight
			from		#SaleRegister sr
						INNER JOIN SupplierCustomer sp on sr.SupplierCustomerId = sp.Id
						LEFT  JOIN ReferenceParties c on sr.RefPartyId = c.Id
		WHERE (@ActionId IS NULL 
			   OR 
			   (@ActionId = 1 AND SR.RefDocumentTypeId <> 98) -- SALES
			   OR
			   (@ActionId = 2 AND SR.RefDocumentTypeId = 98)  -- SALES RETURN
			  )
			Group By	C.ReferencePartyName,sp.CompanyName,sr.ItemCode,sr.ItemName,
						sr.Equivalent,sr.UOMCode,sr.BranchesId
End	

if (@ActivityName = 'Sales Summary By Parent Category')
	Begin
			Insert into #InvSalesRegisterTemp 
						(BranchesId,ParentCategory,QtyOut, BillWeightOut, AmountOut, AvgRate,AvgRate40Kg,PrcntOfTotal,PrcntOfTotalWeight)

			Select		sr.BranchesId,ipc.InvParentCateDescription,sum(sr.QtyOut) QtyOut, sum(sr.BillWeightOut) BillWeightOut, sum(sr.AmountOut) AmountOut
						, 0 As AvgRate
						, Case when sum(sr.BillWeightOut) > 0 and sum(sr.AmountOut) > 0 
						       then round((sum(sr.AmountOut) / sum(sr.BillWeightOut))  * 40, 2) Else 0 End AvgRate40Kg
						, sum(sr.AmountOut) / sum(sum(sr.AmountOut)) over () * 100 as PrctofTotal
						, sum(sr.BillWeightOut) / sum(sum(sr.BillWeightOut)) over () * 100 as PrctofTotalWeight
			from		#SaleRegister sr
						INNER JOIN ItemCategory ic on sr.ItemCategoryId = ic.Id
						LEFT  JOIN InventoryParentCategories ipc on ic.InventoryParentCategoriesId = ipc.Id
		WHERE (@ActionId IS NULL 
			   OR 
			   (@ActionId = 1 AND SR.RefDocumentTypeId <> 98) -- SALES
			   OR
			   (@ActionId = 2 AND SR.RefDocumentTypeId = 98)  -- SALES RETURN
			  )
			Group By	ipc.InvParentCateDescription,sr.BranchesId
End
if (@ActivityName = 'Sales Summary By Parent Category & Customer')
	Begin
			Insert into #InvSalesRegisterTemp 
						(BranchesId,ParentCategory,Customer,QtyOut, BillWeightOut, AmountOut, AvgRate,AvgRate40Kg,PrcntOfTotal,PrcntOfTotalWeight)

			Select		sr.BranchesId,ipc.InvParentCateDescription,sp.CompanyName,sum(sr.QtyOut) QtyOut, sum(sr.BillWeightOut) BillWeightOut, sum(sr.AmountOut) AmountOut
						, 0 As AvgRate
						, Case when sum(sr.BillWeightOut) > 0 and sum(sr.AmountOut) > 0 
						       then round((sum(sr.AmountOut) / sum(sr.BillWeightOut))  * 40, 2) Else 0 End AvgRate40Kg
						, sum(sr.AmountOut) / sum(sum(sr.AmountOut)) over () * 100 as PrctofTotal
						, sum(sr.BillWeightOut) / sum(sum(sr.BillWeightOut)) over () * 100 as PrctofTotalWeight
			from		#SaleRegister sr
						INNER JOIN Item i on sr.ItemId = i.id
						INNER JOIN ItemCategory ic on i.ItemCategoryId = ic.Id
						INNER JOIN SupplierCustomer sp on sr.SupplierCustomerId = sp.Id
						LEFT  JOIN InventoryParentCategories ipc on ic.InventoryParentCategoriesId = ipc.Id
		WHERE (@ActionId IS NULL 
			   OR 
			   (@ActionId = 1 AND SR.RefDocumentTypeId <> 98) -- SALES
			   OR
			   (@ActionId = 2 AND SR.RefDocumentTypeId = 98)  -- SALES RETURN
			  )
			Group By	ipc.InvParentCateDescription,sp.CompanyName,sr.BranchesId
End
if (@ActivityName = 'Sales Summary By Parent Category & Item')
	Begin
			Insert into #InvSalesRegisterTemp 
						(BranchesId,ParentCategory,ItemName,QtyOut, BillWeightOut, AmountOut, AvgRate,AvgRate40Kg,PrcntOfTotal,PrcntOfTotalWeight)

			Select		sr.BranchesId,ipc.InvParentCateDescription,i.ItemName,sum(sr.QtyOut) QtyOut, sum(sr.BillWeightOut) BillWeightOut, sum(sr.AmountOut) AmountOut
						, 0 As AvgRate
						, Case when sum(sr.BillWeightOut) > 0 and sum(sr.AmountOut) > 0 
						       then round((sum(sr.AmountOut) / sum(sr.BillWeightOut))  * 40, 2) Else 0 End AvgRate40Kg
						, sum(sr.AmountOut) / sum(sum(sr.AmountOut)) over () * 100 as PrctofTotal
						, sum(sr.BillWeightOut) / sum(sum(sr.BillWeightOut)) over () * 100 as PrctofTotalWeight
			from		#SaleRegister sr
						INNER JOIN Item i on sr.ItemId = i.id
						INNER JOIN ItemCategory ic on i.ItemCategoryId = ic.Id
						LEFT  JOIN InventoryParentCategories ipc on ic.InventoryParentCategoriesId = ipc.Id
		WHERE (@ActionId IS NULL 
			   OR 
			   (@ActionId = 1 AND SR.RefDocumentTypeId <> 98) -- SALES
			   OR
			   (@ActionId = 2 AND SR.RefDocumentTypeId = 98)  -- SALES RETURN
			  )
			Group By	ipc.InvParentCateDescription,i.ItemName,sr.BranchesId
End

if (@ActivityName = 'Sales Summary By ReferenceParty')
	Begin
			Insert into #InvSalesRegisterTemp 
						(BranchesId,ReferencePartyName, QtyOut, BillWeightOut, AmountOut, AvgRate,AvgRate40Kg,PrcntOfTotal,PrcntOfTotalWeight)
			Select		sr.BranchesId,CASE WHEN rp.ReferencePartyName IS NULL THEN 'Other Reference Party' ELSE RP.ReferencePartyName END, sum(sr.QtyOut) QtyOut, sum(sr.BillWeightOut) BillWeightOut, sum(sr.AmountOut) AmountOut
						, 0 As AvgRate
						, Case when sum(sr.BillWeightOut) > 0 and sum(sr.AmountOut) > 0 
							   then round((sum(sr.AmountOut) / sum(sr.BillWeightOut))  * 40, 2) Else 0 End AvgRate40Kg
						, sum(sr.AmountOut) / sum(sum(sr.AmountOut)) over () * 100 as PrctofTotal
						, sum(sr.BillWeightOut) / sum(sum(sr.BillWeightOut)) over () * 100 as PrctofTotalWeight
			from		#SaleRegister sr
						LEFT  JOIN ReferenceParties rp on sr.RefPartyId = rp.Id
		WHERE (@ActionId IS NULL 
			   OR 
			   (@ActionId = 1 AND SR.RefDocumentTypeId <> 98) -- SALES
			   OR
			   (@ActionId = 2 AND SR.RefDocumentTypeId = 98)  -- SALES RETURN
			  )
			Group By	rp.ReferencePartyName,sr.BranchesId
End
if (@ActivityName = 'Sales Summary By ReferenceParty & City')
	Begin
			Insert into #InvSalesRegisterTemp 
						(BranchesId,ReferencePartyName,CityName, QtyOut, BillWeightOut, AmountOut, AvgRate,AvgRate40Kg,PrcntOfTotal,PrcntOfTotalWeight)
			Select		sr.BranchesId,CASE WHEN rp.ReferencePartyName IS NULL THEN 'Other Reference Party' ELSE RP.ReferencePartyName END,C.Description, sum(sr.QtyOut)		  QtyOut, sum(sr.BillWeightOut) BillWeightOut, sum(sr.AmountOut) AmountOut
						, 0 As AvgRate
						, Case when sum(sr.BillWeightOut) > 0 and sum(sr.AmountOut) > 0 
							   then round((sum(sr.AmountOut) / sum(sr.BillWeightOut))  * 40, 2) Else 0 End AvgRate40Kg
						, sum(sr.AmountOut) / sum(sum(sr.AmountOut)) over () * 100 as PrctofTotal
						, sum(sr.BillWeightOut) / sum(sum(sr.BillWeightOut)) over () * 100 as PrctofTotalWeight
			from		#SaleRegister sr
						INNER JOIN Item i on sr.ItemId = i.id
						LEFT  JOIN City c on sr.CityId = c.Id
						LEFT  JOIN ReferenceParties rp on sr.RefPartyId = rp.Id
		WHERE (@ActionId IS NULL 
			   OR 
			   (@ActionId = 1 AND SR.RefDocumentTypeId <> 98) -- SALES
			   OR
			   (@ActionId = 2 AND SR.RefDocumentTypeId = 98)  -- SALES RETURN
			  )
			Group By	rp.ReferencePartyName,c.Description,sr.BranchesId
End	
if (@ActivityName = 'Sales Summary By ReferenceParty & Pack Size')
	Begin
			Insert into #InvSalesRegisterTemp 
						(BranchesId,ReferencePartyName,UOMCode, QtyOut, BillWeightOut, AmountOut, AvgRate,AvgRate40Kg,PrcntOfTotal,PrcntOfTotalWeight)
			Select		sr.BranchesId,CASE WHEN rp.ReferencePartyName IS NULL THEN 'Other Reference Party' ELSE RP.ReferencePartyName END,iu.UOMCode, sum(sr.QtyOut)		  QtyOut, sum(sr.BillWeightOut) BillWeightOut, sum(sr.AmountOut) AmountOut
						, Case when sum(sr.BillWeightOut) > 0 and sum(sr.AmountOut) > 0 
							   then round((sum(sr.AmountOut) / sum(sr.BillWeightOut) ) * sr.Equivalent,  2) Else 0 End AvgRate
						, Case when sum(sr.BillWeightOut) > 0 and sum(sr.AmountOut) > 0 
						       then round((sum(sr.AmountOut) / sum(sr.BillWeightOut))  * 40, 2) Else 0 End AvgRate40Kg
						, sum(sr.AmountOut) / sum(sum(sr.AmountOut)) over () * 100 as PrctofTotal
						, sum(sr.BillWeightOut) / sum(sum(sr.BillWeightOut)) over () * 100 as PrctofTotalWeight
			from		#SaleRegister sr
						INNER JOIN V_UomScheduleAndUom iu on sr.ItemUom = iu.Id
						LEFT  JOIN ReferenceParties rp on sr.RefPartyId = rp.Id
		WHERE (@ActionId IS NULL 
			   OR 
			   (@ActionId = 1 AND SR.RefDocumentTypeId <> 98) -- SALES
			   OR
			   (@ActionId = 2 AND SR.RefDocumentTypeId = 98)  -- SALES RETURN
			  )
			-- The pack-rate expression uses sr.Equivalent; include that exact value in the group.
			Group By	rp.ReferencePartyName,iu.Equivalent,iu.UOMCode,sr.Equivalent,sr.BranchesId
End
if (@ActivityName = 'Sales Summary By ReferenceParty & Item')
	Begin
			Insert into #InvSalesRegisterTemp 
						(BranchesId,ReferencePartyName, itemcode,UOMCode,ItemName ,QtyOut,
						 BillWeightOut, AmountOut, AvgRate,AvgRate40Kg,PrcntOfTotal,PrcntOfTotalWeight)
			Select		sr.BranchesId,sp.ReferencePartyName,sr.ItemCode,sr.UOMCode,sr.ItemName,sum(sr.QtyOut) QtyOut,
						sum(sr.BillWeightOut) BillWeightOut, sum(sr.AmountOut) AmountOut
						, Case when sum(sr.BillWeightOut) > 0 and sum(sr.AmountOut) > 0 
						       then round((sum(sr.AmountOut) / sum(sr.BillWeightOut) ) * sr.Equivalent,  2) Else 0 End AvgRate
						, Case when sum(sr.BillWeightOut) > 0 and sum(sr.AmountOut) > 0
						       then round((sum(sr.AmountOut) / sum(sr.BillWeightOut) )* 40,  2) Else 0 End AvgRate40Kg
						, sum(sr.AmountOut) / sum(sum(sr.AmountOut)) over () * 100 as PrctofTotal
						, sum(sr.BillWeightOut) / sum(sum(sr.BillWeightOut)) over () * 100 as PrctofTotalWeight
			from		#SaleRegister sr
						Left JOIN ReferenceParties sp on sr.RefPartyId = sp.Id
		WHERE (@ActionId IS NULL 
			   OR 
			   (@ActionId = 1 AND SR.RefDocumentTypeId <> 98) -- SALES
			   OR
			   (@ActionId = 2 AND SR.RefDocumentTypeId = 98)  -- SALES RETURN
			  )
			Group By	sp.ReferencePartyName,sr.ItemCode, sr.ItemName,sr.Equivalent,sr.UOMCode,sr.BranchesId
End
if (@ActivityName = 'Sales Summary By ReferenceParty,Item & Pack Size')
	Begin
			Insert into #InvSalesRegisterTemp 
						(BranchesId,ReferencePartyName,ItemCode, ItemName,UOMCode,
						 QtyOut, BillWeightOut, AmountOut, AvgRate,AvgRate40Kg,PrcntOfTotal,PrcntOfTotalWeight)
			Select		sr.BranchesId,c.ReferencePartyName,i.ItemCode, i.ItemName,
						iu.UOMCode, sum(sr.QtyOut) QtyOut, sum(sr.BillWeightOut) BillWeightOut, sum(sr.AmountOut) AmountOut
						, Case when sum(sr.BillWeightOut) > 0 and sum(sr.AmountOut) > 0 then round((sum(sr.AmountOut) / sum(sr.BillWeightOut) ) * iu.Equivalent,  2) Else 0 End AvgRate
						, Case when sum(sr.BillWeightOut) > 0 and sum(sr.AmountOut) > 0 then round((sum(sr.AmountOut) / sum(sr.BillWeightOut) )* 40,  2) Else 0 End AvgRate40Kg
						, sum(sr.AmountOut) / sum(sum(sr.AmountOut)) over () * 100 as PrctofTotal
						, sum(sr.BillWeightOut) / sum(sum(sr.BillWeightOut)) over () * 100 as PrctofTotalWeight
			from		#SaleRegister sr
						INNER JOIN Item i on sr.ItemId = i.id
						INNER JOIN V_UomScheduleAndUom iu on sr.ItemUom = iu.Id
						LEFT  JOIN ReferenceParties c on sr.RefPartyId = c.Id
		WHERE (@ActionId IS NULL 
			   OR 
			   (@ActionId = 1 AND SR.RefDocumentTypeId <> 98) -- SALES
			   OR
			   (@ActionId = 2 AND SR.RefDocumentTypeId = 98)  -- SALES RETURN
			  )
			Group By	C.ReferencePartyName,I.ItemCode, i.ItemName, iu.Equivalent,iu.UOMCode,sr.BranchesId
End	
if @ActivityName = 'Sales Summary HsCode'
Begin
			Insert into #InvSalesRegisterTemp 
						(BranchesId,HsCode,QtyOut,BillWeightOut, AmountOut, AvgRate,AvgRate40Kg,PrcntOfTotal,PrcntOfTotalWeight)
			Select		sr.BranchesId,sr.HSCode,sum(sr.QtyOut) QtyOut, sum(sr.BillWeightOut) BillWeightOut, sum(sr.AmountOut) AmountOut
						, 0 As AvgRate 
						, Case when sum(sr.BillWeightOut) > 0 and sum(sr.AmountOut) > 0 then round((sum(sr.AmountOut) / sum(sr.BillWeightOut) ) * 40,  2) Else 0 End AvgRate40Kg
						, sum(sr.AmountOut) / sum(sum(sr.AmountOut)) over () * 100 as PrctofTotal
						, sum(sr.BillWeightOut) / sum(sum(sr.BillWeightOut)) over () * 100 as PrctofTotalWeight
			from		#SaleRegister sr
			WHERE (@ActionId IS NULL 
			   OR 
			   (@ActionId = 1 AND SR.RefDocumentTypeId <> 98) -- SALES
			   OR
			   (@ActionId = 2 AND SR.RefDocumentTypeId = 98)  -- SALES RETURN
			  )
			Group By	sr.HSCode,sr.BranchesId

End

IF @CostCenterId = 0
BEGIN
if (@ActivityName = 'Sales Summary By CostCenter')
	Begin
			Insert into #InvSalesRegisterTemp 
						(BranchesId,CostCenterId,CostCenterName, QtyOut, BillWeightOut, AmountOut, AvgRate,AvgRate40Kg,PrcntOfTotal,PrcntOfTotalWeight)

			Select		sr.BranchesId,sr.CostCenterId,CC.CostCenterName,sum(sr.QtyOut) QtyOut, sum(sr.BillWeightOut) BillWeightOut, sum(sr.AmountOut) AmountOut
						, 0 As AvgRate
						, Case when sum(sr.BillWeightOut) > 0 and sum(sr.AmountOut) > 0 
						       then round((sum(sr.AmountOut) / sum(sr.BillWeightOut))  * 40, 2) Else 0 End AvgRate40Kg
						, sum(sr.AmountOut) / sum(sum(sr.AmountOut)) over () * 100 as PrctofTotal
						, sum(sr.BillWeightOut) / sum(sum(sr.BillWeightOut)) over () * 100 as PrctofTotalWeight
			from		#SaleRegister sr
					LEFT JOIN CostCenter CC on sr.CostCenterId = CC.Id
		WHERE (@ActionId IS NULL 
			   OR 
			   (@ActionId = 1 AND SR.RefDocumentTypeId <> 98) -- SALES
			   OR
			   (@ActionId = 2 AND SR.RefDocumentTypeId = 98)  -- SALES RETURN
			  )
			Group By	sr.BranchesId,sr.CostCenterId,CC.CostCenterName
End	

if (@ActivityName = 'Sales Summary By CostCenter & Customer')
	Begin
			Insert into #InvSalesRegisterTemp 
						(BranchesId,CostCenterId,CostCenterName,Customer, QtyOut, BillWeightOut, AmountOut, AvgRate,AvgRate40Kg,PrcntOfTotal,PrcntOfTotalWeight)

			Select		sr.BranchesId,sr.CostCenterId,CC.CostCenterName,sp.CompanyName,sum(sr.QtyOut) QtyOut, sum(sr.BillWeightOut) BillWeightOut, sum(sr.AmountOut) AmountOut
			            , 0 As AvgRate
						, Case when sum(sr.BillWeightOut) > 0 and sum(sr.AmountOut) > 0 
						       then round((sum(sr.AmountOut) / sum(sr.BillWeightOut))  * 40, 2) Else 0 End AvgRate40Kg
						, sum(sr.AmountOut) / sum(sum(sr.AmountOut)) over () * 100 as PrctofTotal
						, sum(sr.BillWeightOut) / sum(sum(sr.BillWeightOut)) over () * 100 as PrctofTotalWeight
			from		#SaleRegister sr
					LEFT JOIN CostCenter CC on sr.CostCenterId = CC.Id											
					INNER JOIN SupplierCustomer sp on sr.SupplierCustomerId = sp.Id
		WHERE (@ActionId IS NULL 
			   OR 
			   (@ActionId = 1 AND SR.RefDocumentTypeId <> 98) -- SALES
			   OR
			   (@ActionId = 2 AND SR.RefDocumentTypeId = 98)  -- SALES RETURN
			  )
			Group By	sr.BranchesId,sr.CostCenterId,CC.CostCenterName,sp.CompanyName
End

if (@ActivityName = 'Sales Summary By CostCenter & Item')
	Begin
			Insert into #InvSalesRegisterTemp 
						(BranchesId,CostCenterId,CostCenterName,ItemName, QtyOut, BillWeightOut, AmountOut, AvgRate,AvgRate40Kg,PrcntOfTotal,PrcntOfTotalWeight)

			Select		sr.BranchesId,sr.CostCenterId,CC.CostCenterName,
						i.ItemName,sum(sr.QtyOut) QtyOut, sum(sr.BillWeightOut) BillWeightOut, sum(sr.AmountOut) AmountOut
						, 0 As AvgRate
						, Case when sum(sr.BillWeightOut) > 0 and sum(sr.AmountOut) > 0
						       then round((sum(sr.AmountOut) / sum(sr.BillWeightOut))  * 40, 2) Else 0 End AvgRate
						, sum(sr.AmountOut) / sum(sum(sr.AmountOut)) over () * 100 as PrctofTotal
						, sum(sr.BillWeightOut) / sum(sum(sr.BillWeightOut)) over () * 100 as PrctofTotalWeight
			from		#SaleRegister sr
			LEFT JOIN CostCenter CC on sr.CostCenterId = CC.Id
			INNER JOIN Item i on sr.ItemId = i.id
		WHERE (@ActionId IS NULL 
			   OR 
			   (@ActionId = 1 AND SR.RefDocumentTypeId <> 98) -- SALES
			   OR
			   (@ActionId = 2 AND SR.RefDocumentTypeId = 98)  -- SALES RETURN
			  )
			Group By	sr.BranchesId,sr.CostCenterId,CC.CostCenterName,i.ItemName
End

END



Update 	#InvSalesRegisterTemp
SET ReportCriteria= Coalesce(@ReportParamCSV,'')


SELECT 
    ROW_NUMBER() OVER (
        ORDER BY
            -- 1️⃣ Sales Register
            CASE WHEN @ActivityName = 'Sales Register' or @ActivityName = 'INVOICE WISE PROFITABLITY' THEN R.DocDate END,

            -- 2️⃣ Sales Summary By Item variations
            CASE WHEN @ActivityName = 'Sales Summary By Item' THEN R.[ItemName] END,
            CASE WHEN @ActivityName = 'Sales Summary By Item & Warehouse' THEN R.[ItemName] END,
            CASE WHEN @ActivityName = 'Sales Summary By Item & Warehouse' THEN R.[WarehouseName] END,
            CASE WHEN @ActivityName = 'Sales Summary By Item & City' THEN R.[ItemName] END,
            CASE WHEN @ActivityName = 'Sales Summary By Item & City' THEN R.[CityName] END,
            CASE WHEN @ActivityName = 'Sales Summary By Item & Pack Size' THEN R.[ItemName] END,
            CASE WHEN @ActivityName = 'Sales Summary By Item & Pack Size' THEN R.[ItemUom] END,
            CASE WHEN @ActivityName = 'Sales Summary By Item,Pack Size & City' THEN R.[ItemName] END,
            CASE WHEN @ActivityName = 'Sales Summary By Item,Pack Size & City' THEN R.[ItemUom] END,
            CASE WHEN @ActivityName = 'Sales Summary By Item,Pack Size & City' THEN R.[CityName] END,

            -- 3️⃣ Sales Summary By Customer variations
            CASE WHEN @ActivityName = 'Sales Summary By Customer' THEN R.[Customer] END,
            CASE WHEN @ActivityName = 'Sales Summary By Customer & ReferenceParty' THEN R.[Customer] END,
            CASE WHEN @ActivityName = 'Sales Summary By Customer & ReferenceParty' THEN R.[ReferencePartyName] END,
            CASE WHEN @ActivityName = 'Sales Summary By Customer & Item' THEN R.[Customer] END,
            CASE WHEN @ActivityName = 'Sales Summary By Customer & Item' THEN R.[ItemName] END,
            CASE WHEN @ActivityName = 'Sales Summary By Customer & City' THEN R.[Customer] END,
            CASE WHEN @ActivityName = 'Sales Summary By Customer & City' THEN R.[CityName] END,
            CASE WHEN @ActivityName = 'Sales Summary By Customer & Pack Size' THEN R.[Customer] END,
            CASE WHEN @ActivityName = 'Sales Summary By Customer & Pack Size' THEN R.[ItemUom] END,
            CASE WHEN @ActivityName = 'Sales Summary By Customer,Item & City' THEN R.[Customer] END,
            CASE WHEN @ActivityName = 'Sales Summary By Customer,Item & City' THEN R.[ItemName] END,
            CASE WHEN @ActivityName = 'Sales Summary By Customer,Item & City' THEN R.[CityName] END,
            CASE WHEN @ActivityName = 'Sales Summary By Customer,Item & ReferenceParty' THEN R.[Customer] END,
            CASE WHEN @ActivityName = 'Sales Summary By Customer,Item & ReferenceParty' THEN R.[ItemName] END,
            CASE WHEN @ActivityName = 'Sales Summary By Customer,Item & ReferenceParty' THEN R.[ReferencePartyName] END,

            -- 4️⃣ Sales Summary By Parent Category variations
            CASE WHEN @ActivityName = 'Sales Summary By Parent Category' THEN R.[ParentCategory] END,
            CASE WHEN @ActivityName = 'Sales Summary By Parent Category & Customer' THEN R.[ParentCategory] END,
            CASE WHEN @ActivityName = 'Sales Summary By Parent Category & Customer' THEN R.[Customer] END,
            CASE WHEN @ActivityName = 'Sales Summary By Parent Category & Item' THEN R.[ParentCategory] END,
            CASE WHEN @ActivityName = 'Sales Summary By Parent Category & Item' THEN R.[ItemName] END,

            -- 5️⃣ Sales Summary By Reference Party variations
            CASE WHEN @ActivityName = 'Sales Summary By ReferenceParty' THEN R.[ReferencePartyName] END,
            CASE WHEN @ActivityName = 'Sales Summary By ReferenceParty & City' THEN R.[ReferencePartyName] END,
            CASE WHEN @ActivityName = 'Sales Summary By ReferenceParty & City' THEN R.[CityName] END,
            CASE WHEN @ActivityName = 'Sales Summary By ReferenceParty & Pack Size' THEN R.[ReferencePartyName] END,
            CASE WHEN @ActivityName = 'Sales Summary By ReferenceParty & Pack Size' THEN R.[ItemUom] END,
            CASE WHEN @ActivityName = 'Sales Summary By ReferenceParty & Item' THEN R.[ReferencePartyName] END,
            CASE WHEN @ActivityName = 'Sales Summary By ReferenceParty & Item' THEN R.[ItemName] END,
            CASE WHEN @ActivityName = 'Sales Summary By ReferenceParty,Item & Pack Size' THEN R.[ReferencePartyName] END,
            CASE WHEN @ActivityName = 'Sales Summary By ReferenceParty,Item & Pack Size' THEN R.[ItemName] END,
            CASE WHEN @ActivityName = 'Sales Summary By ReferenceParty,Item & Pack Size' THEN R.[ItemUom] END,

            -- 6️⃣ Sales Summary By Cost Center variations
            CASE WHEN @ActivityName = 'Sales Summary By CostCenter' THEN R.CostCenterName END,
            CASE WHEN @ActivityName = 'Sales Summary By CostCenter & Customer' THEN R.CostCenterName END,
            CASE WHEN @ActivityName = 'Sales Summary By CostCenter & Customer' THEN R.[Customer] END,
            CASE WHEN @ActivityName = 'Sales Summary By CostCenter & Item' THEN R.CostCenterName END,
            CASE WHEN @ActivityName = 'Sales Summary By CostCenter & Item' THEN R.[ItemName] END,

            -- 7️⃣ Sales Summary By HsCode
            CASE WHEN @ActivityName = 'Sales Summary HsCode' THEN R.HsCode END
    ) AS RowNo
	  ,R.[RefDocumentTypeId]
	  ,R.OtherCategoryId
	  ,LP.LookupName AS OtherCategory
      ,R.[DocumentTypeCode]
      ,R.[ItemName]
      ,R.[UOMCode]
      ,R.[JobLotCode]
      ,R.[PackTypeCode]
      ,R.[RefDocIdNo]
      ,R.[SupplierCustomerId]
      ,R.[Customer],R.BookingPersonId,R.BookingPersonName
      ,R.[DocDate]
      ,R.[DocCodeNo]
      ,R.[WarehouseId]
      ,R.[VehicleNo]
      ,R.[GpNoDcNo]
      ,R.[InventoryParentCategoriesId]
      ,R.[ItemCategoryId]
      ,R.[ItemId]
      ,R.[ItemUom]
      ,R.[CropBatch]
      ,R.[JobLotId]
	  ,R.JobOrderNo
      ,R.[InvPackingTypeId]
      ,R.[QtyOut]
      ,R.[BillWeightOut]
	  ,R.StockWeight
	  ,R.AmountOut
      ,R.[AvgRate]
	  ,R.[AvgRate40Kg]
	  ,R.CgsRate
	  ,R.ProfitLossPerRate
	  ,R.ProfitLoss
	  ,R.ProfitLossPercent
      ,R.[ItemCode]
	  ,R.HsCode
      ,R.[WarehouseName]
      ,R.[CityName]
      ,R.[PrcntOfTotal]
      ,R.[ParentCategory]
      ,R.[ItemCategory]
      ,R.[ItemType]
      ,R.[PrcntOfTotalWeight]
      ,R.[ReferencePartyName]
      ,R.[CompLogoImage]
      ,R.[CompCountry]
      ,R.[CompContactPerson]
      ,R.[CompMobileA]
      ,R.[CompMobileB]
      ,R.[CompMobileC]
      ,R.[CompEmailA]
      ,R.[CompEmailB]
      ,R.[CompanyWebsite]
      ,R.[CompanyFaxNo]
      ,R.[OrgReportingRemarks]
      ,R.[ReferenceNo]
      ,R.[RefPartyAddress]
      ,R.[Distance]
      ,R.[TaxPercent]
      ,R.[TaxAmount]
      ,R.[TotalAmount]
      ,R.[ItemAmountWithTax]
      ,R.[BillAmount]
	  ,R.ItemAmountWithoutExpense
      ,R.[IsFOC]
      ,R.[ReportCriteria]
	  ,R.BranchSrNo
	  ,r.CostCenterId
	  ,r.CostCenterName
	  ,r.Expenses
	  ,r.Freight
	  ,r.Commission
      ,R.[GdnNo]
	  ,R.GdnDate
	  ,R.SoNo
	  ,R.SoDate
	  ,R.TicketNos
	  ,R.BranchesId
	  ,BR.BranchName,R.ItemRate,R.RateCut,R.RateCutAmount
	  ,sum(R.QtyOut) Over () TotalOfQtyOut
	  ,sum(R.BillWeightOut) Over () TotalOfBillWeightOut
	  ,sum(R.AmountOut) Over () TotalOfAmountOut
	  ,sum(R.PrcntOfTotal) Over () TotalOfPrcntOfTotal
	  ,sum(R.PrcntOfTotalWeight) Over () TotalOfPrcntOfTotalWeight
	  ,sum(R.TaxPercent) Over () TotalOfTaxPercent
	  ,sum(R.TaxAmount) Over () TotalOfTaxAmount
	  ,sum(R.TotalAmount) Over () TotalOfTotalAmount
	  ,sum(R.Expenses) Over () TotalOfExpenses
	  ,sum(R.Freight) Over () TotalOfFreight
	  ,sum(R.Commission) Over () TotalOfCommission
	  ,sum(R.[ItemAmountWithTax]) Over () TotalOfItemAmountWithTax
	  ,sum(R.BillAmount) Over () TotalOfBillAmount
	  ,COUNT(*) OVER() row_count
FROM #InvSalesRegisterTemp R
		LEFT JOIN Branches BR ON R.BranchesId = BR.Id
		LEFT JOIN InvLookUp LP ON R.OtherCategoryId = LP.Id
ORDER BY ROW_NUMBER() OVER (
        ORDER BY
            -- 1️⃣ Sales Register
            CASE WHEN @ActivityName = 'Sales Register' or @ActivityName = 'INVOICE WISE PROFITABLITY' THEN R.DocDate END,

            -- 2️⃣ Sales Summary By Item variations
            CASE WHEN @ActivityName = 'Sales Summary By Item' THEN R.[ItemName] END,
            CASE WHEN @ActivityName = 'Sales Summary By Item & Warehouse' THEN R.[ItemName] END,
            CASE WHEN @ActivityName = 'Sales Summary By Item & Warehouse' THEN R.[WarehouseName] END,
            CASE WHEN @ActivityName = 'Sales Summary By Item & City' THEN R.[ItemName] END,
            CASE WHEN @ActivityName = 'Sales Summary By Item & City' THEN R.[CityName] END,
            CASE WHEN @ActivityName = 'Sales Summary By Item & Pack Size' THEN R.[ItemName] END,
            CASE WHEN @ActivityName = 'Sales Summary By Item & Pack Size' THEN R.[ItemUom] END,
            CASE WHEN @ActivityName = 'Sales Summary By Item,Pack Size & City' THEN R.[ItemName] END,
            CASE WHEN @ActivityName = 'Sales Summary By Item,Pack Size & City' THEN R.[ItemUom] END,
            CASE WHEN @ActivityName = 'Sales Summary By Item,Pack Size & City' THEN R.[CityName] END,

            -- 3️⃣ Sales Summary By Customer variations
            CASE WHEN @ActivityName = 'Sales Summary By Customer' THEN R.[Customer] END,
            CASE WHEN @ActivityName = 'Sales Summary By Customer & ReferenceParty' THEN R.[Customer] END,
            CASE WHEN @ActivityName = 'Sales Summary By Customer & ReferenceParty' THEN R.[ReferencePartyName] END,
            CASE WHEN @ActivityName = 'Sales Summary By Customer & Item' THEN R.[Customer] END,
            CASE WHEN @ActivityName = 'Sales Summary By Customer & Item' THEN R.[ItemName] END,
            CASE WHEN @ActivityName = 'Sales Summary By Customer & City' THEN R.[Customer] END,
            CASE WHEN @ActivityName = 'Sales Summary By Customer & City' THEN R.[CityName] END,
            CASE WHEN @ActivityName = 'Sales Summary By Customer & Pack Size' THEN R.[Customer] END,
            CASE WHEN @ActivityName = 'Sales Summary By Customer & Pack Size' THEN R.[ItemUom] END,
            CASE WHEN @ActivityName = 'Sales Summary By Customer,Item & City' THEN R.[Customer] END,
            CASE WHEN @ActivityName = 'Sales Summary By Customer,Item & City' THEN R.[ItemName] END,
            CASE WHEN @ActivityName = 'Sales Summary By Customer,Item & City' THEN R.[CityName] END,
            CASE WHEN @ActivityName = 'Sales Summary By Customer,Item & ReferenceParty' THEN R.[Customer] END,
            CASE WHEN @ActivityName = 'Sales Summary By Customer,Item & ReferenceParty' THEN R.[ItemName] END,
            CASE WHEN @ActivityName = 'Sales Summary By Customer,Item & ReferenceParty' THEN R.[ReferencePartyName] END,

            -- 4️⃣ Sales Summary By Parent Category variations
            CASE WHEN @ActivityName = 'Sales Summary By Parent Category' THEN R.[ParentCategory] END,
            CASE WHEN @ActivityName = 'Sales Summary By Parent Category & Customer' THEN R.[ParentCategory] END,
            CASE WHEN @ActivityName = 'Sales Summary By Parent Category & Customer' THEN R.[Customer] END,
            CASE WHEN @ActivityName = 'Sales Summary By Parent Category & Item' THEN R.[ParentCategory] END,
            CASE WHEN @ActivityName = 'Sales Summary By Parent Category & Item' THEN R.[ItemName] END,

            -- 5️⃣ Sales Summary By Reference Party variations
            CASE WHEN @ActivityName = 'Sales Summary By ReferenceParty' THEN R.[ReferencePartyName] END,
            CASE WHEN @ActivityName = 'Sales Summary By ReferenceParty & City' THEN R.[ReferencePartyName] END,
            CASE WHEN @ActivityName = 'Sales Summary By ReferenceParty & City' THEN R.[CityName] END,
            CASE WHEN @ActivityName = 'Sales Summary By ReferenceParty & Pack Size' THEN R.[ReferencePartyName] END,
            CASE WHEN @ActivityName = 'Sales Summary By ReferenceParty & Pack Size' THEN R.[ItemUom] END,
            CASE WHEN @ActivityName = 'Sales Summary By ReferenceParty & Item' THEN R.[ReferencePartyName] END,
            CASE WHEN @ActivityName = 'Sales Summary By ReferenceParty & Item' THEN R.[ItemName] END,
            CASE WHEN @ActivityName = 'Sales Summary By ReferenceParty,Item & Pack Size' THEN R.[ReferencePartyName] END,
            CASE WHEN @ActivityName = 'Sales Summary By ReferenceParty,Item & Pack Size' THEN R.[ItemName] END,
            CASE WHEN @ActivityName = 'Sales Summary By ReferenceParty,Item & Pack Size' THEN R.[ItemUom] END,

            -- 6️⃣ Sales Summary By Cost Center variations
            CASE WHEN @ActivityName = 'Sales Summary By CostCenter' THEN R.CostCenterName END,
            CASE WHEN @ActivityName = 'Sales Summary By CostCenter & Customer' THEN R.CostCenterName END,
            CASE WHEN @ActivityName = 'Sales Summary By CostCenter & Customer' THEN R.[Customer] END,
            CASE WHEN @ActivityName = 'Sales Summary By CostCenter & Item' THEN R.CostCenterName END,
            CASE WHEN @ActivityName = 'Sales Summary By CostCenter & Item' THEN R.[ItemName] END,

            -- 7️⃣ Sales Summary By HsCode
            CASE WHEN @ActivityName = 'Sales Summary HsCode' THEN R.HsCode END
    ) ASC 
OFFSET @SkipRows ROWS 
    FETCH NEXT @PageSize ROWS ONLY
DROP TABLE IF EXISTS #InvSalesRegisterTemp;
DROP TABLE IF EXISTS #BrancheIds;
DROP TABLE IF EXISTS #SaleRegister;
DROP TABLE IF EXISTS #Dataaa;
DROP TABLE IF EXISTS #WbUpdate;
DROP TABLE IF EXISTS #GdnData;
