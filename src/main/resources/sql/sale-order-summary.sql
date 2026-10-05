-- Java Sale Order Summary. Ported from dbo.USP_SaleOrderSummaryRegister (2026-10-04).
-- Native filters/aggregates are retained. Each request owns its working rows; the desktop
-- shared table is read ONLY for its empty schema, never cleared, updated or populated here.
-- All values below are JDBC parameters supplied from validated filters and the signed-in user.
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
DECLARE @OrderItemId int = ?;
DECLARE @PackUom float = ?;
DECLARE @CropYear nvarchar(20) = ?;
DECLARE @JobLotId int = ?;
DECLARE @PackingTypeId int = ?;
DECLARE @OrderSupCustId int = ?;
DECLARE @CityId int = ?;
DECLARE @DistrictId int = ?;
DECLARE @ActionId int = ?;
DECLARE @ActivityName nvarchar(max) = ?;
DECLARE @ReferencePartyId int = ?;
DECLARE @OrderStatus NVARCHAR(50) = ?;
DECLARE @SkipZero int = ?;
DECLARE @IsOnQty int = ?;
DECLARE @BranchesIds NVARCHAR(MAX) = ?;
DECLARE @RefSalesManId int = ?;
DECLARE @CustomerGroupId int = ?;
DECLARE @ReferencePartyIds nvarchar(max) = ?;
DECLARE @PageSize int = ?;
DECLARE @PageNumber int = ?;
DECLARE @IsApproved BIT = ?;
DECLARE @AppId INT = ?;
DECLARE @UserId INT = ?;
DECLARE @BookingPersonId INT = ?;
DECLARE @CostCenterId INT = ?;
SELECT TOP (0) * INTO #SaleOrderRegisterTemp FROM dbo.SaleOrderRegisterTemp;


Set nocount on 
--******************New Working
IF ISNULL(@UserId,0) = 0
BEGIN
	RAISERROR('UserId not found',16,1,'Amir Hussain');
	RETURN
END

IF @AppId IN (4,6) -- MobileApp_CustomerPortal , Desktop_CustomerPortal
BEGIN
	SELECT @OrderSupCustId = SupplierCustomerId FROM UserAccount where ID = @UserId
END


--****************************************************************************

IF @PageNumber IS NULL OR @PageNumber<=0
		SET @PageNumber=1

IF @PageSize IS NULL
		SET @PageSize=1000000


DECLARE @AvgUom float=40
if (ISNULL(@IsOnQty,0) = 1)
	Set @AvgUom=1


IF @OrderStatus IS NULL 
	SET @OrderStatus = 'Open'
	
DECLARE @ReportParamCSV VARCHAR(max)
DECLARE @AND NVARCHAR(3)
DECLARE @ParentCategory nvarchar(max)
DECLARE @ItemCategory nvarchar(max)
DECLARE @ItemType nvarchar(max)
DECLARE @PackingType NVARCHAR(max)
DECLARE @District NVARCHAR(MAX)
DECLARE @CityName NVARCHAR(max)
DECLARE @JobLot nvarchar(max)
DECLARE @ItemName nvarchar(max)
DECLARE @CustomerName nvarchar(max)
DECLARE @CostCenterName nvarchar(max)
DECLARE @ReportType nvarchar(max)


SET @ReportParamCSV = ''
SET @AND = 'AND'

IF @InventoryParentCategories > 0
BEGIN
	SELECT @ParentCategory = InvParentCateDescription FROM InventoryParentCategories  WHERE Id = @InventoryParentCategories
END

IF @ItemCategoryId > 0
BEGIN
	SELECT @ItemCategory = CategoryDescription FROM ItemCategory WHERE Id = @ItemCategoryId
END

IF @ItemTypeId > 0
BEGIN
	SELECT @ItemType = TypeDescription FROM ItemType WHERE Id = @ItemTypeId
END


IF @PackingTypeId > 0
BEGIN
	SELECT @PackingType = PackTypeDesc FROM InvPackingType WHERE Id = @PackingTypeId
END

IF @DistrictId > 0
BEGIN
	SELECT @District = Description FROM District WHERE Id = @DistrictId
END

IF @CityId > 0
BEGIN
	SELECT @CityName = Description FROM City WHERE Id = @CityId
END

IF @JobLotId > 0
BEGIN
	SELECT @JobLot = JobLotDescription FROM JobLot WHERE Id = @JobLotId
END

IF @CostCenterId > 0
BEGIN
	SELECT @CostCenterName = CostCenterName FROM CostCenter WHERE Id = @CostCenterId
END

IF @OrderItemId > 0
BEGIN
	SELECT @ItemName = itemName FROM Item WHERE Id = @OrderItemId
END

IF @OrderSupCustId > 0
BEGIN
	SELECT @CustomerName = CompanyName FROM SupplierCustomer WHERE Id = @OrderSupCustId
END


IF @FromDate IS not NULL
SET @ReportParamCSV = @ReportParamCSV + 'FromDate: ' + CAST(FORMAT(@FromDate, 'dd/MMM/yyyy ') AS NVARCHAR(50)) 

IF @ToDate IS not NULL
SET @ReportParamCSV += ' ' + @AND + ' ToDate: ' + CAST(FORMAT(@ToDate, 'dd/MMM/yyyy ') AS NVARCHAR(50))

IF @DocNoFrom IS not NULL
SET @ReportParamCSV +=  ' ' + @AND + ' DocFrom: ' + CAST(@DocNoFrom AS NVARCHAR(50))

IF @DocNoTo IS not NULL
SET @ReportParamCSV = @ReportParamCSV + ' ' + @AND + ' DocTo: ' + CAST(@DocNoTo AS NVARCHAR(50))

IF @ParentCategory IS not NULL
SET @ReportParamCSV = @ReportParamCSV + ' ' + @AND + ' ParentCategory: ' + @ParentCategory

IF @ItemCategory IS not NULL
SET @ReportParamCSV = @ReportParamCSV + ' ' + @AND + ' ItemCategory: ' + @ItemCategory

IF @ItemType IS not NULL
SET @ReportParamCSV = @ReportParamCSV + ' ' + @AND + ' ItemType: ' + @ItemType

IF @CropYear IS not NULL
SET @ReportParamCSV = @ReportParamCSV + ' ' + @AND + ' CropYear: ' + @CropYear

IF @PackUom IS not NULL
SET @ReportParamCSV = @ReportParamCSV + ' ' + @AND + ' PackUOM: ' + CONVERT(VARCHAR(50),@PackUom)

IF @PackingType IS not NULL
SET @ReportParamCSV = @ReportParamCSV + ' ' + @AND + ' PackingType: ' + @PackingType

IF @District IS not NULL
SET @ReportParamCSV = @ReportParamCSV + ' ' + @AND + ' District: ' + @District

IF @CityName IS not NULL
SET @ReportParamCSV = @ReportParamCSV + ' ' + @AND + ' CityName: ' + @CityName

IF @JobLot IS not NULL
SET @ReportParamCSV = @ReportParamCSV + ' ' + @AND + ' JobLot: ' + @JobLot

IF @ItemName IS not NULL
SET @ReportParamCSV = @ReportParamCSV + ' ' + @AND + ' ItemName: ' + @ItemName

IF @CustomerName IS not NULL
SET @ReportParamCSV = @ReportParamCSV + ' ' + @AND + ' CustomerName: ' + @CustomerName

IF @CostCenterName IS not NULL
SET @ReportParamCSV = @ReportParamCSV + ' ' + @AND + ' CostCenterName: ' + @CostCenterName

IF @ReportType is not null
SET @ReportParamCSV = @ReportParamCSV + ' ' + @AND + ' ReportType: ' + @ReportType

IF @ActionId = 1
SET @ReportParamCSV = @ReportParamCSV + ' ' + @AND + ' Included Approved Do '


Delete from #SaleOrderRegisterTemp

SELECT	h.DocumentTypeId,h.Id,d.Id as OrderDetailId,h.OrderSupCustId,d.CityId, convert(date,h.DocDate) DocDate,h.DocNo
		,ic.InventoryParentCategoriesId, i.ItemCategoryId, d.OrderItemId,d.OrderItemUOMId,d.Crop,d.JobLotId,d.PackingTypeID
		,d.ReferencePartyId,i.ItemCode,i.ItemName,c.Description as CityName,Uom.Equivalent as PEquivalent,Uom.UOMCode as PackUom
		,d.OrderItemQty
		,d.NetWeight
		,0 as DispatchQty
		,0 as DispatchWeight
		,d.Amount
		,0 AS DispatchAmount
		,h.BranchesId
		,h.BranchSrNo,
		d.CostCenterId,h.BookingPersonId
INTO  #tmpResult
from	SaleOrderDetail d
		INNER JOIN SaleOrder h on d.SaleOrderId = h.Id and d.ActionTypeId <> 3
		INNER JOIN SupplierCustomer sp on h.OrderSupCustId = sp.Id
		INNER JOIN Item i on d.OrderItemId = i.id
		INNER JOIN V_UomScheduleAndUom Uom on d.OrderItemUOMId = uom.Id
		INNER JOIN ItemCategory ic on i.ItemCategoryId = ic.Id
		LEFT  JOIN InventoryParentCategories ipc on ic.InventoryParentCategoriesId = ipc.Id
		LEFT  JOIN City c on d.CityId = c.Id
		LEFT  JOIN Tehsil t on c.TehsilId = t.Id

Where	h.OrganizationId = @OrganizationId 
		and h.CompanyId = @CompanyId
		and (@FromDate is null or convert(date,h.docdate) >= convert(date,@FromDate)) 
		and (@ToDate is null or convert(date,h.docDate) <= convert(date,@ToDate))
		and (@DocNoFrom is null or h.DocNo >= @DocNoFrom) 
		and (@DocNoTo is null or h.DocNo <=@DocNoTo)
		and (@InventoryParentCategories is null or ic.InventoryParentCategoriesId = @InventoryParentCategories)
		and	(@ItemCategoryId is null or i.ItemCategoryId = @ItemCategoryId)
		and	(@ItemTypeId is null or i.ItemTypeId = @ItemTypeId)
		and (@OrderItemId is null or d.OrderItemId = @OrderItemId)
		and (@PackUom is null or Uom.Equivalent = @PackUom)
		and (@CropYear is null or d.Crop = @CropYear)
		and (@JobLotId is null or d.JobLotId=@JobLotId)
		and (@PackingTypeId is null or d.PackingTypeID = @PackingTypeId)
		and	(@OrderSupCustId is null or h.OrderSupCustId = @OrderSupCustId)
		and	(@CityId is null or d.CityId = @CityId)
		and	(@DistrictId is null or t.DistrictId = @DistrictId)
		and (@ReferencePartyId is null or d.ReferencePartyId = @ReferencePartyId)
		and (h.DocumentTypeId in (81,127,1605)) -- SALE ORDER,BOOKING ORDER,SALE ORDER ENG
		and	(@OrderStatus IS NULL OR h.OrderStatus = @OrderStatus)	
		and (@IsApproved is null or h.IsAproved = @IsApproved)
		and     (@BranchesIds IS NULL OR 
					EXISTS (
								SELECT 1 
								FROM fnSplitString(@BranchesIds, ',') AS dt
								WHERE dt.Data = h.BranchesId
							)
				)
		--and	(@RefSalesManId is null or h.RefSalesManId = @RefSalesManId)
		AND (@CustomerGroupId is null or  sp.CustomerGroupId = @CustomerGroupId)
		AND (@BookingPersonId is null or  h.BookingPersonId = @BookingPersonId)
		AND (@ReferencePartyIds is null or  d.ReferencePartyId in (Select Data From fnSplitString(@ReferencePartyIds,',')))	
		and	(
				(@AppId IS NULL OR @AppId <> 5)
				OR 
				(@AppId = 5 and d.CostCenterId = @CostCenterId)
			)

---=================================	CREATE TAMP TABLE FOR DISPATCH DATA 	--============================================
CREATE TABLE #TmpDispatches(SaleOrderId int,SaleOrderDetailId int,DispatchQty float,DispatchWeight float,DoRejWeight float)
---=================================	DO NOT REFERED IN GDN     --============================================
IF @ActionId = 1 
BEGIN

INSERT INTO #TmpDispatches
select d.SaleOrderId,d.SaleOrderDetailId
		,ISNULL(sum(d.LoadingQty),0) as DispatchQty
		,ISNULL(sum(d.LoadingWeight),0) as DispatchWeight
		,0 as DoRejWeight
from #tmpResult r
		inner join InvDeliveryOrderDetail d on r.Id = d.SaleOrderId and r.OrderDetailId = d.SaleOrderDetailId and d.ActionTypeId <> 3
		inner join InvDeliveryOrder h on d.InvDeliveryOrderId = h.Id and h.ActionId <> 3
		left  join GatePassOutward g on d.InvDeliveryOrderId = g.SaleOrderId and g.OtherSupCust = 'DeliverOrder' and g.Status <> 'Rejected'
where	h.DocumentTypeId = 84
and		h.DeliveryOrderType <> 'Export'
and		h.IsApproved = 1
and		(CONCAT(g.Id,d.SupplierCustomerId) NOT IN (SELECT CONCAT(OutwardGatePassId,SupplierCustomerId) FROM InvGdn WHERE OrganizationId = @OrganizationId AND CompanyId = @CompanyId))
GROUP BY d.SaleOrderId,d.SaleOrderDetailId

END
---=================================	 GET QTY AND WEIGHT FROM DIRECT INVOICE		--============================================
INSERT INTO #TmpDispatches
select d.SaleOrderId,d.SaleOrderDetailId
		,ISNULL(sum(d.ItemQty),0) as InvoiceQty
		,ISNULL(sum(d.NetBillWeight),0) as InvoiceWeight
		,0 as DoRejWeight
from #tmpResult r
		inner join InvSaleInvoiceDetail d on r.Id = d.SaleOrderId and r.OrderDetailId = d.SaleOrderDetailId
		inner join InvSaleInvoice h on d.InvSaleInvoiceId = h.Id
where	ISNULL(d.InvGdnId,0) = 0
GROUP BY d.SaleOrderId,d.SaleOrderDetailId
---=================================	 GET QTY AND WEIGHT FROM GDN AGAINST ORDER		--============================================
INSERT INTO #TmpDispatches
select R.Id,r.OrderDetailId
		,ISNULL(sum(d.ItemQty),0) as GdnQty
		,ISNULL(sum(d.NetBillWeight),0) as GdnWeight
		,0 as DoRejWeight
from #tmpResult r
		inner join InvGdnDetail d on r.Id = d.SaleOrderId and r.OrderDetailId = d.SaleOrderDetailId	
GROUP BY r.Id,r.OrderDetailId
---=================================	 GET QTY AND WEIGHT FROM GDN CANCELED GATEPASS AGAINST DO		--============================================
INSERT INTO #TmpDispatches
select d.SaleOrderId,d.SaleOrderDetailId
		,ISNULL(sum(d.LoadingQty),0) as DoRejQty
		,0
		,ISNULL(sum(d.LoadingWeight),0) as DoRejWeight
from #tmpResult r
		inner join InvDeliveryOrderDetail d on r.Id = d.SaleOrderId and r.OrderDetailId = d.SaleOrderDetailId and d.ActionTypeId <> 3
		inner join InvDeliveryOrder h on d.InvDeliveryOrderId = h.Id and h.ActionId <> 3
		inner join GatePassOutward g on d.InvDeliveryOrderId = g.SaleOrderId and g.OtherSupCust = 'DeliverOrder'
where	h.DocumentTypeId IN (84,1606) -- RICE AND ENGINEERING
and		h.DeliveryOrderType <> 'Export'
and		g.Status = 'Rejected' 
GROUP BY d.SaleOrderId,d.SaleOrderDetailId
---=================================	DISPATCH QTY & WEIGHT SUMS INSERT INTO OTHER TEMP	--============================================
SELECT a.SaleOrderId,a.SaleOrderDetailId
		,ISNULL(SUM(A.DispatchQty),0) AS DispatchQty
		,ISNULL(SUM(A.DispatchWeight),0) AS DispatchWeight
		,ISNULL(SUM(A.DoRejWeight),0) AS DoRejWeight
into #tmpDispatchWt
FROM #TmpDispatches a
GROUP BY a.SaleOrderId,a.SaleOrderDetailId
	---=================================	UPDATE DISPATCH WEIGHT		--=========================================
update r 
		set 
			r.DispatchQty = ISNULL(d.DispatchQty,0), 
			r.DispatchWeight = ISNULL(d.DispatchWeight,0) - ISNULL(d.DoRejWeight,0),
			r.DispatchAmount = (r.Amount / r.NetWeight) * (ISNULL(d.DispatchWeight,0) - ISNULL(d.DoRejWeight,0))
from #tmpResult r
		inner join #tmpDispatchWt d on r.Id = d.SaleOrderId and r.OrderDetailId = d.SaleOrderDetailId
--		Detail Register

if (@ActivityName = 'Order Register')
Begin
		INSERT into #SaleOrderRegisterTemp 
					(Id,DocumentTypeId,DocDate, DocumentTypeCode, DocNo,Customer,ReferencePartyDetail, ItemCode, ItemName, UOMCode, CropBatch, JobLotCode, PackTypeCode
					,CityName,OrderQty,DispatchQty,BalQty,OrderWeight,DispatchWeight,BalWeight
					,OrderAmount,DispatchAmount,BalAmount,AvgRate,PrcntOfTotal,BranchSrNo,BranchId,RecordNo,CostCenterId,CostCenterName,BookingPerson
					)

		SELECT		sr.Id,sr.DocumentTypeId,sr.DocDate,dt.DocumentTypeCode, sr.DocNo, sp.CompanyName,rp.ReferencePartyName,sr.ItemCode,sr.ItemName,sr.PackUom, sr.Crop, jb.JobLotCode,pt.PackTypeCode
					,sr.CityName,sr.OrderItemQty,sr.DispatchQty,sr.OrderItemQty - sr.DispatchQty,sr.NetWeight,sr.DispatchWeight,sr.NetWeight - sr.DispatchWeight
					,sr.Amount,sr.DispatchAmount,sr.Amount - sr.DispatchAmount
					,Case when sr.NetWeight > 0 and sr.Amount > 0 then sr.Amount / sr.NetWeight * sr.PEquivalent Else 0 End AvgRate
					,sr.Amount / Sum(sr.Amount) over() * 100 
					,sr.BranchSrNo,sr.BranchesId,
					ROW_NUMBER() OVER(ORDER BY sr.OrderDetailId) AS RNo,sr.CostCenterId,CC.CostCenterName,sp1.referencePartyName
		from		#tmpResult sr
					INNER JOIN SupplierCustomer sp on sr.OrderSupCustId = sp.Id
					INNER JOIN DocumentType dt on sr.DocumentTypeId = dt.Id
					LEFT  JOIN JobLot jb on sr.JobLotId = jb.Id
					LEFT  JOIN InvPackingType pt on sr.PackingTypeID = pt.Id
					LEFT  JOIN ReferenceParties rp on sr.ReferencePartyId = rp.Id
					LEFT JOIN CostCenter CC on sr.CostCenterId = CC.Id
					LEFT JOIN referenceparties sp1 on sr.BookingPersonId = sp1.Id
		WHERE	(@SkipZero IS NULL OR sr.NetWeight - sr.DispatchWeight > 0)
		End	
		
if (@ActivityName = 'Order Summary By Customer & OrderNo')
	Begin
			INSERT into #SaleOrderRegisterTemp 
						(Id,SupplierCustomerId,Customer, DocumentTypeId,DocDate, DocumentTypeCode, DocNo,
						 OrderQty,DispatchQty,BalQty,
						 OrderWeight,DispatchWeight,BalWeight,
						 OrderAmount,DispatchAmount,BalAmount,AvgRate,PrcntOfTotal,RecordNo
						)
			SELECT		sr.Id,
						sr.OrderSupCustId,
						sp.CompanyName,
						sr.DocumentTypeId,
						sr.DocDate,
						dt.DocumentTypeCode, 
						sr.DocNo,
						sum(sr.OrderItemQty),sum(sr.DispatchQty),sum(sr.OrderItemQty) - sum(sr.DispatchQty),
						sum(sr.NetWeight),sum(sr.DispatchWeight),sum(sr.NetWeight) - sum(sr.DispatchWeight),
						sum(sr.Amount),sum(sr.DispatchAmount),sum(sr.Amount) - sum(sr.DispatchAmount)
						, Case when sum(sr.NetWeight) > 0 and sum(sr.Amount) > 0 then round((sum(sr.Amount) / sum(sr.NetWeight) )* @AvgUom,  2) Else 0 End AvgRate
						, sum(sr.Amount) / sum(sum(sr.Amount)) over () * 100 as PrctofTotal,
						ROW_NUMBER() OVER(ORDER BY sr.Id, sr.OrderSupCustId) as RNo
			from		#tmpResult sr
						INNER JOIN SupplierCustomer sp on sr.OrderSupCustId = sp.Id
						INNER JOIN DocumentType dt on sr.DocumentTypeId = dt.Id
			Group By	sr.Id,sr.OrderSupCustId,sp.CompanyName,sr.DocumentTypeId,sr.DocDate,dt.DocumentTypeCode, sr.DocNo
			HAVING     (@SkipZero IS NULL OR sum(sr.NetWeight) - sum(sr.DispatchWeight) > 0)
End	
		
if (@ActivityName = 'Order Summary By Item & Pack Size')
	Begin
			INSERT into #SaleOrderRegisterTemp 
						(
							ItemCode, ItemName,UOMCode,OrderQty,DispatchQty,BalQty,
							OrderWeight,DispatchWeight,BalWeight,
							OrderAmount,DispatchAmount,BalAmount,AvgRate,PrcntOfTotal,RecordNo
						)
			SELECT		sr.ItemCode,sr.ItemName,sr.PackUom, sum(sr.OrderItemQty),sum(sr.DispatchQty),sum(sr.OrderItemQty) - sum(sr.DispatchQty),
						sum(sr.NetWeight),sum(sr.DispatchWeight),sum(sr.NetWeight) - sum(sr.DispatchWeight), 
						sum(sr.Amount),sum(sr.DispatchAmount),sum(sr.Amount) - sum(sr.DispatchAmount)
						,Case when sum(sr.NetWeight) > 0 and sum(sr.Amount) > 0 then round((sum(sr.Amount) / sum(sr.NetWeight) )* sr.PEquivalent,  2) ELSE 0 End AvgRate
						,sum(sr.Amount) / sum(sum(sr.Amount)) over () * 100 as PrctofTotal
						,ROW_NUMBER() OVER(ORDER BY sr.ItemCode,sr.ItemName,sr.PackUom) as RNo
			from		#tmpResult sr
						INNER JOIN SupplierCustomer sp on sr.OrderSupCustId = sp.Id
						INNER JOIN DocumentType dt on sr.DocumentTypeId = dt.Id
			Group By	sr.ItemCode, sr.ItemName, sr.PEquivalent,sr.PackUom
			HAVING     (@SkipZero IS NULL OR sum(sr.NetWeight) - sum(sr.DispatchWeight) > 0)
End	

if (@ActivityName = 'Order Summary By Item,Pack Size & City')
	Begin
			INSERT into #SaleOrderRegisterTemp 
						(CityName,ItemCode, ItemName,UOMCode,
						 OrderQty,DispatchQty,BalQty,
						 OrderWeight,DispatchWeight,BalWeight,
						 OrderAmount,DispatchAmount,BalAmount,AvgRate,PrcntOfTotal,RecordNo
						)
			SELECT		sr.CityName,sr.ItemCode,sr.ItemName,sr.PackUom,
						sum(sr.OrderItemQty),sum(sr.DispatchQty),sum(sr.OrderItemQty) - sum(sr.DispatchQty),
						sum(sr.NetWeight),sum(sr.DispatchWeight),sum(sr.NetWeight) - sum(sr.DispatchWeight),
						sum(sr.Amount),sum(sr.DispatchAmount),sum(sr.Amount) - sum(sr.DispatchAmount)
						, Case when sum(sr.NetWeight) > 0 and sum(sr.Amount) > 0 then round((sum(sr.Amount) / sum(sr.NetWeight) )* sr.PEquivalent,  2) ELSE 0 End AvgRate
						, sum(sr.Amount) / sum(sum(sr.Amount)) over () * 100 as PrctofTotal
						,ROW_NUMBER() OVER(ORDER BY sr.CityName,sr.ItemCode,sr.ItemName,sr.PackUom) as RNo
			from		#tmpResult sr
						INNER JOIN SupplierCustomer sp on sr.OrderSupCustId = sp.Id
						INNER JOIN DocumentType dt on sr.DocumentTypeId = dt.Id
			Group By	sr.CityName,sr.ItemCode,sr.ItemName,sr.PackUom,sr.PEquivalent
			HAVING     (@SkipZero IS NULL OR sum(sr.NetWeight) - sum(sr.DispatchWeight) > 0)
End	

if (@ActivityName = 'Order Summary By Customer & Pack Size')
	Begin
			INSERT into #SaleOrderRegisterTemp 
						(Customer,UOMCode, 
						 OrderQty,DispatchQty,BalQty,
						 OrderWeight,DispatchWeight,BalWeight,
						 OrderAmount,DispatchAmount,BalAmount,AvgRate,PrcntOfTotal,RecordNo
						)
			SELECT		sp.CompanyName,sr.PackUom, 
						sum(sr.OrderItemQty),sum(sr.DispatchQty),sum(sr.OrderItemQty) - sum(sr.DispatchQty),
						sum(sr.NetWeight),sum(sr.DispatchWeight),sum(sr.NetWeight) - sum(sr.DispatchWeight),
						sum(sr.Amount),sum(sr.DispatchAmount),sum(sr.Amount) - sum(sr.DispatchAmount)
						, Case when sum(sr.NetWeight) > 0 and sum(sr.Amount) > 0 then round((sum(sr.Amount) / sum(sr.NetWeight))  * sr.PEquivalent, 2) ELSE 0 End AvgRate
						, sum(sr.Amount) / sum(sum(sr.Amount)) over () * 100 as PrctofTotal
						,ROW_NUMBER() OVER(ORDER BY sp.CompanyName,sr.PackUom) as RNo
			from		#tmpResult sr
						INNER JOIN SupplierCustomer sp on sr.OrderSupCustId = sp.Id
						INNER JOIN DocumentType dt on sr.DocumentTypeId = dt.Id
			Group By	sp.CompanyName, sr.PEquivalent,sr.PackUom
			HAVING     (@SkipZero IS NULL OR sum(sr.NetWeight) - sum(sr.DispatchWeight) > 0)
End	

if (@ActivityName = 'Order Summary By Item')
	Begin
			INSERT into #SaleOrderRegisterTemp 
						(ItemCode, ItemName,
						 OrderQty,DispatchQty,BalQty,
						 OrderWeight,DispatchWeight,BalWeight,
						 OrderAmount,DispatchAmount,BalAmount,AvgRate,PrcntOfTotal,RecordNo
						)
			SELECT		sr.ItemCode,sr.ItemName, 
						sum(sr.OrderItemQty),sum(sr.DispatchQty),sum(sr.OrderItemQty) - sum(sr.DispatchQty),
						sum(sr.NetWeight),sum(sr.DispatchWeight),sum(sr.NetWeight) - sum(sr.DispatchWeight),
						sum(sr.Amount),sum(sr.DispatchAmount),sum(sr.Amount) - sum(sr.DispatchAmount)
						,Case when sum(sr.NetWeight) > 0 and sum(sr.Amount) > 0 then round((sum(sr.Amount) / sum(sr.NetWeight) ) * @AvgUom,2) Else 0 End AvgRate
						,sum(sr.Amount) / sum(sum(sr.Amount)) over () * 100 as PrctofTotal
						,ROW_NUMBER() OVER(ORDER BY sr.ItemCode,sr.ItemName) as RNo
			from		#tmpResult sr
						INNER JOIN SupplierCustomer sp on sr.OrderSupCustId = sp.Id
						INNER JOIN DocumentType dt on sr.DocumentTypeId = dt.Id
			Group By	sr.ItemCode,sr.ItemName
			HAVING     (@SkipZero IS NULL OR sum(sr.NetWeight) - sum(sr.DispatchWeight) > 0)
End

if (@ActivityName = 'Order Summary By Item & City')
	Begin
			INSERT into #SaleOrderRegisterTemp 
						(CityName,ItemCode, ItemName, 
						 OrderQty,DispatchQty,BalQty,
						 OrderWeight,DispatchWeight,BalWeight,
						 OrderAmount,DispatchAmount,BalAmount,AvgRate,PrcntOfTotal,RecordNo
						)
			SELECT		sr.CityName,sr.ItemCode,sr.ItemName,
						sum(sr.OrderItemQty),sum(sr.DispatchQty),sum(sr.OrderItemQty) - sum(sr.DispatchQty),
						sum(sr.NetWeight),sum(sr.DispatchWeight),sum(sr.NetWeight) - sum(sr.DispatchWeight),
						sum(sr.Amount),sum(sr.DispatchAmount),sum(sr.Amount) - sum(sr.DispatchAmount)
						, Case when sum(sr.NetWeight) > 0 and sum(sr.Amount) > 0 then round((sum(sr.Amount) / sum(sr.NetWeight) ) * @AvgUom,  2) Else 0 End AvgRate
						, sum(sr.Amount) / sum(sum(sr.Amount)) over () * 100 as PrctofTotal
						,ROW_NUMBER() OVER(ORDER BY sr.CityName,sr.ItemCode,sr.ItemName) as RNo
			from		#tmpResult sr
						INNER JOIN SupplierCustomer sp on sr.OrderSupCustId = sp.Id
						INNER JOIN DocumentType dt on sr.DocumentTypeId = dt.Id
			Group By	sr.CityName,sr.ItemCode,sr.ItemName
			HAVING     (@SkipZero IS NULL OR sum(sr.NetWeight) - sum(sr.DispatchWeight) > 0)
End
	
if (@ActivityName = 'Order Summary By Customer')
	Begin
			INSERT into #SaleOrderRegisterTemp 
						(Customer,UOMCode, 
						 OrderQty,DispatchQty,BalQty,
						 OrderWeight,DispatchWeight,BalWeight,
						 OrderAmount,DispatchAmount,BalAmount,AvgRate,PrcntOfTotal,RecordNo
						)

			SELECT		sp.CompanyName,
						'***', 
						sum(sr.OrderItemQty),sum(sr.DispatchQty),sum(sr.OrderItemQty) - sum(sr.DispatchQty),
						sum(sr.NetWeight),sum(sr.DispatchWeight),sum(sr.NetWeight) - sum(sr.DispatchWeight),
						sum(sr.Amount),sum(sr.DispatchAmount),sum(sr.Amount) - sum(sr.DispatchAmount)
						,Case when sum(sr.NetWeight) > 0 and sum(sr.Amount) > 0 then round((sum(sr.Amount) / sum(sr.NetWeight))  * 40, 2) Else 0 End AvgRate
						,sum(sr.Amount) / sum(sum(sr.Amount)) over () * 100 as PrctofTotal
						,ROW_NUMBER() OVER(ORDER BY sp.CompanyName) as RNo
			from		#tmpResult sr
						INNER JOIN SupplierCustomer sp on sr.OrderSupCustId = sp.Id
						INNER JOIN DocumentType dt on sr.DocumentTypeId = dt.Id
			Group By	sp.CompanyName
			HAVING     (@SkipZero IS NULL OR sum(sr.NetWeight) - sum(sr.DispatchWeight) > 0)
End	

if (@ActivityName = 'Order Summary By Customer and ReferenceParty')
	Begin
			INSERT into #SaleOrderRegisterTemp 
						(Customer,ReferencePartyDetail,UOMCode, 
						 OrderQty,DispatchQty,BalQty,
						 OrderWeight,DispatchWeight,BalWeight,
						 OrderAmount,DispatchAmount,BalAmount,AvgRate,PrcntOfTotal,RecordNo
						)

			SELECT		sp.CompanyName,rp.ReferencePartyName,sr.PackUom, 
						sum(sr.OrderItemQty),sum(sr.DispatchQty),sum(sr.OrderItemQty) - sum(sr.DispatchQty),
						sum(sr.NetWeight),sum(sr.DispatchWeight),sum(sr.NetWeight) - sum(sr.DispatchWeight),
						sum(sr.Amount),sum(sr.DispatchAmount),sum(sr.Amount) - sum(sr.DispatchAmount)
						,Case when sum(sr.NetWeight) > 0 and sum(sr.Amount) > 0 then round((sum(sr.Amount) / sum(sr.NetWeight))  * sr.PEquivalent, 2) Else 0 End AvgRate
						,sum(sr.Amount) / sum(sum(sr.Amount)) over () * 100 as PrctofTotal
						,ROW_NUMBER() OVER(ORDER BY sp.CompanyName,rp.ReferencePartyName,sr.PackUom) as RNo
			from		#tmpResult sr
						INNER JOIN SupplierCustomer sp on sr.OrderSupCustId = sp.Id
						INNER JOIN DocumentType dt on sr.DocumentTypeId = dt.Id
						LEFT  JOIN ReferenceParties rp on sr.ReferencePartyId = rp.Id
			Group By	sp.CompanyName,rp.ReferencePartyName, sr.PEquivalent,sr.PackUom
			HAVING     (@SkipZero IS NULL OR sum(sr.NetWeight) - sum(sr.DispatchWeight) > 0)
End	

if (@ActivityName = 'Order Summary By Customer & City')
	Begin
			INSERT into #SaleOrderRegisterTemp 
						(CityName,Customer,UOMCode,
						 OrderQty,DispatchQty,BalQty,
						 OrderWeight,DispatchWeight,BalWeight,
						 OrderAmount,DispatchAmount,BalAmount,AvgRate,PrcntOfTotal,RecordNo
						)
			SELECT		sr.CityName,sp.CompanyName,sr.PackUom,
						sum(sr.OrderItemQty),sum(sr.DispatchQty),sum(sr.OrderItemQty) - sum(sr.DispatchQty),
						sum(sr.NetWeight),sum(sr.DispatchWeight),sum(sr.NetWeight) - sum(sr.DispatchWeight),
						sum(sr.Amount),sum(sr.DispatchAmount),sum(sr.Amount) - sum(sr.DispatchAmount)
						,Case when sum(sr.NetWeight) > 0 and sum(sr.Amount) > 0 then round((sum(sr.Amount) / sum(sr.NetWeight))  * sr.PEquivalent, 2) Else 0 End AvgRate
						,sum(sr.Amount) / sum(sum(sr.Amount)) over () * 100 as PrctofTotal
						,ROW_NUMBER() OVER(ORDER BY sr.CityName,sp.CompanyName,sr.PackUom) as RNo
			from		#tmpResult sr
						INNER JOIN SupplierCustomer sp on sr.OrderSupCustId = sp.Id
						INNER JOIN DocumentType dt on sr.DocumentTypeId = dt.Id
			Group By	sr.CityName,sp.CompanyName, sr.PEquivalent,sr.PackUom
			HAVING     (@SkipZero IS NULL OR sum(sr.NetWeight) - sum(sr.DispatchWeight) > 0)
End	

if (@ActivityName = 'Order Summary By Customer & Item')
	Begin
			INSERT into #SaleOrderRegisterTemp 
						(Customer, itemcode,UOMCode,ItemName,
						 OrderQty,DispatchQty,BalQty,
						 OrderWeight,DispatchWeight,BalWeight,
						 OrderAmount,DispatchAmount,BalAmount,AvgRate,PrcntOfTotal,RecordNo
						)
			SELECT		sp.CompanyName,sr.ItemCode,sr.PackUom,sr.ItemName,
						sum(sr.OrderItemQty),sum(sr.DispatchQty),sum(sr.OrderItemQty) - sum(sr.DispatchQty),
						sum(sr.NetWeight),sum(sr.DispatchWeight),sum(sr.NetWeight) - sum(sr.DispatchWeight),
						sum(sr.Amount),sum(sr.DispatchAmount),sum(sr.Amount) - sum(sr.DispatchAmount)
						,Case when sum(sr.NetWeight) > 0 and sum(sr.Amount) > 0 then round((sum(sr.Amount) / sum(sr.NetWeight) )* sr.PEquivalent,  2) Else 0 End AvgRate
						,SUM(sr.Amount) / SUM(SUM(sr.Amount)) over () * 100 as PrctofTotal
						,ROW_NUMBER() OVER(ORDER BY sp.CompanyName,sr.ItemCode,sr.PackUom,sr.ItemName) as RNo
			from		#tmpResult sr
						INNER JOIN SupplierCustomer sp on sr.OrderSupCustId = sp.Id
						INNER JOIN DocumentType dt on sr.DocumentTypeId = dt.Id
			Group By	sp.CompanyName,sr.ItemCode,sr.ItemName,sr.PEquivalent,sr.PackUom
			HAVING     (@SkipZero IS NULL OR sum(sr.NetWeight) - sum(sr.DispatchWeight) > 0)
End	

if (@ActivityName = 'Order Summary By ReferenceParty & Item')
	Begin
			INSERT into #SaleOrderRegisterTemp 
						(ReferencePartyDetail, itemcode,UOMCode,ItemName,
						 OrderQty,DispatchQty,BalQty,
						 OrderWeight,DispatchWeight,BalWeight,
						 OrderAmount,DispatchAmount,BalAmount,AvgRate,PrcntOfTotal,RecordNo
						)
			SELECT		sp.ReferencePartyName,sr.ItemCode,sr.PackUom,sr.ItemName,
						sum(sr.OrderItemQty),sum(sr.DispatchQty),sum(sr.OrderItemQty) - sum(sr.DispatchQty),
						sum(sr.NetWeight),sum(sr.DispatchWeight),sum(sr.NetWeight) - sum(sr.DispatchWeight),
						sum(sr.Amount),sum(sr.DispatchAmount),sum(sr.Amount) - sum(sr.DispatchAmount)
						, Case when sum(sr.NetWeight) > 0 and sum(sr.Amount) > 0 then round((sum(sr.Amount) / sum(sr.NetWeight) )* sr.PEquivalent,  2) Else 0 End AvgRate
						, sum(sr.Amount) / sum(sum(sr.Amount)) over () * 100 as PrctofTotal
						,ROW_NUMBER() OVER(ORDER BY sp.ReferencePartyName,sr.ItemCode,sr.PackUom,sr.ItemName) as RNo
			from		#tmpResult sr
						INNER JOIN DocumentType dt on sr.DocumentTypeId = dt.Id
						LEFT  JOIN ReferenceParties sp on sr.ReferencePartyId = sp.Id
			Group By	sp.ReferencePartyName,sr.ItemCode,sr.ItemName,sr.PEquivalent,sr.PackUom
			HAVING     (@SkipZero IS NULL OR sum(sr.NetWeight) - sum(sr.DispatchWeight) > 0)
End	

if (@ActivityName = 'Order Summary By Customer & Item & ReferenceParty')
	Begin
			INSERT into #SaleOrderRegisterTemp 
						(Customer,ReferencePartyDetail, itemcode,UOMCode,ItemName,
						 OrderQty,DispatchQty,BalQty,
						 OrderWeight,DispatchWeight,BalWeight,
						 OrderAmount,DispatchAmount,BalAmount,AvgRate,PrcntOfTotal,RecordNo
						)
			SELECT		sp.CompanyName,rp.ReferencePartyName,sr.ItemCode,sr.PackUom,sr.ItemName,
						sum(sr.OrderItemQty),sum(sr.DispatchQty),sum(sr.OrderItemQty) - sum(sr.DispatchQty),
						sum(sr.NetWeight),sum(sr.DispatchWeight),sum(sr.NetWeight) - sum(sr.DispatchWeight),
						sum(sr.Amount),sum(sr.DispatchAmount),sum(sr.Amount) - sum(sr.DispatchAmount)
						, Case when sum(sr.NetWeight) > 0 and sum(sr.Amount) > 0 then round((sum(sr.Amount) / sum(sr.NetWeight) )* sr.PEquivalent,  2) Else 0 End AvgRate
						, sum(sr.Amount) / sum(sum(sr.Amount)) over () * 100 as PrctofTotal
						,ROW_NUMBER() OVER(ORDER BY sp.CompanyName,rp.ReferencePartyName,sr.ItemCode,sr.PackUom,sr.ItemName) as RNo
			from		#tmpResult sr
						INNER JOIN SupplierCustomer sp on sr.OrderSupCustId = sp.Id
						INNER JOIN DocumentType dt on sr.DocumentTypeId = dt.Id
						LEFT  JOIN ReferenceParties rp on sr.ReferencePartyId = rp.Id
			Group By	sp.CompanyName,rp.ReferencePartyName,sr.ItemCode,sr.ItemName,sr.PEquivalent,sr.PackUom
			HAVING     (@SkipZero IS NULL OR sum(sr.NetWeight) - sum(sr.DispatchWeight) > 0)
End	

if (@ActivityName = 'Order Summary By Customer,Item & City')
	Begin
			INSERT into #SaleOrderRegisterTemp 
						(CityName,Customer, itemcode,UOMCode,ItemName,
						 OrderQty,DispatchQty,BalQty,
						 OrderWeight,DispatchWeight,BalWeight,
						 OrderAmount,DispatchAmount,BalAmount,AvgRate,PrcntOfTotal,RecordNo
						)
			SELECT		sr.CityName,sp.CompanyName,sr.ItemCode,sr.PackUom,sr.ItemName,
						sum(sr.OrderItemQty),sum(sr.DispatchQty),sum(sr.OrderItemQty) - sum(sr.DispatchQty),
						sum(sr.NetWeight),sum(sr.DispatchWeight),sum(sr.NetWeight) - sum(sr.DispatchWeight),
						sum(sr.Amount),sum(sr.DispatchAmount),sum(sr.Amount) - sum(sr.DispatchAmount)
						, Case when sum(sr.NetWeight) > 0 and sum(sr.Amount) > 0 then round((sum(sr.Amount) / sum(sr.NetWeight) )* sr.PEquivalent,  2) Else 0 End AvgRate
						, sum(sr.Amount) / sum(sum(sr.Amount)) over () * 100 as PrctofTotal
						,ROW_NUMBER() OVER(ORDER BY sr.CityName,sp.CompanyName,sr.ItemCode,sr.PackUom,sr.ItemName) as RNo
			from		#tmpResult sr
						INNER JOIN SupplierCustomer sp on sr.OrderSupCustId = sp.Id
						INNER JOIN DocumentType dt on sr.DocumentTypeId = dt.Id
			Group By	sr.CityName,sp.CompanyName, ItemCode, ItemName,sr.PEquivalent,sr.PackUom
			HAVING     (@SkipZero IS NULL OR sum(sr.NetWeight) - sum(sr.DispatchWeight) > 0)
End	

if (@ActivityName = 'Order Summary By ReferenceParty')
	Begin
			INSERT into #SaleOrderRegisterTemp 
						(ReferencePartyDetail,UOMCode, 
						 OrderQty,DispatchQty,BalQty,
						 OrderWeight,DispatchWeight,BalWeight,
						 OrderAmount,DispatchAmount,BalAmount,AvgRate,PrcntOfTotal,RecordNo
						)

			SELECT		sp.ReferencePartyName,sr.PackUom, 
						sum(sr.OrderItemQty),sum(sr.DispatchQty),sum(sr.OrderItemQty) - sum(sr.DispatchQty),
						sum(sr.NetWeight),sum(sr.DispatchWeight),sum(sr.NetWeight) - sum(sr.DispatchWeight),
						sum(sr.Amount),sum(sr.DispatchAmount),sum(sr.Amount) - sum(sr.DispatchAmount)
						, Case when sum(sr.NetWeight) > 0 and sum(sr.Amount) > 0 then round((sum(sr.Amount) / sum(sr.NetWeight))  * sr.PEquivalent, 2) Else 0 End AvgRate
						, sum(sr.Amount) / sum(sum(sr.Amount)) over () * 100 as PrctofTotal
						,ROW_NUMBER() OVER(ORDER BY sp.ReferencePartyName,sr.PackUom) as RNo
			from		#tmpResult sr
						INNER JOIN ReferenceParties sp on sr.ReferencePartyId = sp.Id
						INNER JOIN DocumentType dt on sr.DocumentTypeId = dt.Id
			Group By	sp.ReferencePartyName, sr.PEquivalent,sr.PackUom
			HAVING     (@SkipZero IS NULL OR sum(sr.NetWeight) - sum(sr.DispatchWeight) > 0)
End	

if (@ActivityName = 'Order Summary By ReferenceParty & City')
	Begin
			INSERT into #SaleOrderRegisterTemp 
						(CityName,ReferencePartyDetail,UOMCode,
						 OrderQty,DispatchQty,BalQty,
						 OrderWeight,DispatchWeight,BalWeight,
						 OrderAmount,DispatchAmount,BalAmount,AvgRate,PrcntOfTotal,RecordNo
						)
			SELECT		sr.CityName,sp.ReferencePartyName,sr.PackUom,
						sum(sr.OrderItemQty),sum(sr.DispatchQty),sum(sr.OrderItemQty) - sum(sr.DispatchQty),
						sum(sr.NetWeight),sum(sr.DispatchWeight),sum(sr.NetWeight) - sum(sr.DispatchWeight),
						sum(sr.Amount),sum(sr.DispatchAmount),sum(sr.Amount) - sum(sr.DispatchAmount)
						,Case when sum(sr.NetWeight) > 0 and sum(sr.Amount) > 0 then round((sum(sr.Amount) / sum(sr.NetWeight))  * sr.PEquivalent, 2) Else 0 End AvgRate
						,SUM(sr.Amount) / sum(sum(sr.Amount)) over () * 100 as PrctofTotal
						,ROW_NUMBER() OVER(ORDER BY sr.CityName,sp.ReferencePartyName,sr.PackUom) as RNo
			from		#tmpResult sr
						INNER JOIN ReferenceParties sp on sr.ReferencePartyId = sp.Id
						INNER JOIN DocumentType dt on sr.DocumentTypeId = dt.Id
			Group By	sr.CityName,sp.ReferencePartyName, sr.PEquivalent,sr.PackUom
			HAVING     (@SkipZero IS NULL OR sum(sr.NetWeight) - sum(sr.DispatchWeight) > 0)
End	

IF @CostCenterId > 0
BEGIN
if (@ActivityName = 'Order Summary By CostCenter')
	Begin
			INSERT into #SaleOrderRegisterTemp 
						(CostCenterId,CostCenterName, 
						 OrderQty,DispatchQty,BalQty,
						 OrderWeight,DispatchWeight,BalWeight,
						 OrderAmount,DispatchAmount,BalAmount,AvgRate,PrcntOfTotal,RecordNo
						)

			SELECT		sr.CostCenterId,CC.CostCenterName,
						sum(sr.OrderItemQty),sum(sr.DispatchQty),sum(sr.OrderItemQty) - sum(sr.DispatchQty),
						sum(sr.NetWeight),sum(sr.DispatchWeight),sum(sr.NetWeight) - sum(sr.DispatchWeight),
						sum(sr.Amount),sum(sr.DispatchAmount),sum(sr.Amount) - sum(sr.DispatchAmount)
						,Case when sum(sr.NetWeight) > 0 and sum(sr.Amount) > 0 then round((sum(sr.Amount) / sum(sr.NetWeight))  * 40, 2) Else 0 End AvgRate
						,sum(sr.Amount) / sum(sum(sr.Amount)) over () * 100 as PrctofTotal
						,ROW_NUMBER() OVER(ORDER BY Sr.CostCenterId,CC.CostCenterName) as RNo
			from		#tmpResult sr
						INNER JOIN CostCenter CC on sr.CostCenterId = CC.Id
						INNER JOIN DocumentType dt on sr.DocumentTypeId = dt.Id
			Group By	sr.CostCenterId,CC.CostCenterName
			HAVING     (@SkipZero IS NULL OR sum(sr.NetWeight) - sum(sr.DispatchWeight) > 0)
End	
if (@ActivityName = 'Order Summary By CostCenter & Item')
	Begin
			INSERT into #SaleOrderRegisterTemp 
						(CostCenterId,CostCenterName, itemcode,UOMCode,ItemName,
						 OrderQty,DispatchQty,BalQty,
						 OrderWeight,DispatchWeight,BalWeight,
						 OrderAmount,DispatchAmount,BalAmount,AvgRate,PrcntOfTotal,RecordNo
						)
			SELECT		Sr.CostCenterId,CC.CostCenterName,sr.ItemCode,sr.PackUom,sr.ItemName,
						sum(sr.OrderItemQty),sum(sr.DispatchQty),sum(sr.OrderItemQty) - sum(sr.DispatchQty),
						sum(sr.NetWeight),sum(sr.DispatchWeight),sum(sr.NetWeight) - sum(sr.DispatchWeight),
						sum(sr.Amount),sum(sr.DispatchAmount),sum(sr.Amount) - sum(sr.DispatchAmount)
						,Case when sum(sr.NetWeight) > 0 and sum(sr.Amount) > 0 then round((sum(sr.Amount) / sum(sr.NetWeight) )* sr.PEquivalent,  2) Else 0 End AvgRate
						,SUM(sr.Amount) / SUM(SUM(sr.Amount)) over () * 100 as PrctofTotal
						,ROW_NUMBER() OVER(ORDER BY Sr.CostCenterId,CC.CostCenterName,sr.ItemCode,sr.PackUom,sr.ItemName) as RNo
			from		#tmpResult sr
						INNER JOIN CostCenter CC on sr.CostCenterId = CC.Id
						INNER JOIN DocumentType dt on sr.DocumentTypeId = dt.Id
			Group By	Sr.CostCenterId,CC.CostCenterName,sr.ItemCode,sr.ItemName,sr.PEquivalent,sr.PackUom
			HAVING     (@SkipZero IS NULL OR sum(sr.NetWeight) - sum(sr.DispatchWeight) > 0)
End	
if (@ActivityName = 'Order Summary By CostCenter & Customer')
	Begin
			INSERT into #SaleOrderRegisterTemp 
						(CostCenterId,CostCenterName,SupplierCustomerId,Customer,
						 OrderQty,DispatchQty,BalQty,
						 OrderWeight,DispatchWeight,BalWeight,
						 OrderAmount,DispatchAmount,BalAmount,AvgRate,PrcntOfTotal,RecordNo
						)
			SELECT		Sr.CostCenterId,CC.CostCenterName,sr.OrderSupCustId,sp.CompanyName,
						sum(sr.OrderItemQty),sum(sr.DispatchQty),sum(sr.OrderItemQty) - sum(sr.DispatchQty),
						sum(sr.NetWeight),sum(sr.DispatchWeight),sum(sr.NetWeight) - sum(sr.DispatchWeight),
						sum(sr.Amount),sum(sr.DispatchAmount),sum(sr.Amount) - sum(sr.DispatchAmount)
						,Case when sum(sr.NetWeight) > 0 and sum(sr.Amount) > 0 then round((sum(sr.Amount) / sum(sr.NetWeight) )* sr.PEquivalent,  2) Else 0 End AvgRate
						,SUM(sr.Amount) / SUM(SUM(sr.Amount)) over () * 100 as PrctofTotal
						,ROW_NUMBER() OVER(ORDER BY Sr.CostCenterId,CC.CostCenterName,sr.OrderSupCustId,sp.CompanyName) as RNo
			from		#tmpResult sr
						INNER JOIN CostCenter CC on sr.CostCenterId = CC.Id
						INNER JOIN SupplierCustomer sp on sr.OrderSupCustId = sp.Id
			Group By	Sr.CostCenterId,CC.CostCenterName,sr.OrderSupCustId,sp.CompanyName
			HAVING     (@SkipZero IS NULL OR sum(sr.NetWeight) - sum(sr.DispatchWeight) > 0)
End	
END

Update r
set  		   r.ReportCriteria = @ReportParamCSV 
			  ,r.CompCountry=co.CompCountry
			  ,r.CompContactPerson=co.CompContactPerson
			  ,r.CompMobileA=co.CompMobileA
			  ,r.CompMobileB=co.CompMobileB
			  ,r.CompMobileC=co.CompMobileC
			  ,r.CompEmailA=co.CompEmailA
			  ,r.CompEmailB=co.CompEmailB
			  ,r.CompLogoImage=co.CompLogoImage
			  ,r.CompanyWebsite=co.CompanyWebsite
			  ,r.CompanyFaxNo=co.CompanyFaxNo
			  ,r.OrgReportingRemarks=org.ReportingRemarks
from #SaleOrderRegisterTemp r
		Inner join Company co on co.Id=@CompanyId
		Inner join Organization org on org.Id=co.OrgCompanyTypeId

SELECT R.[Id]
      ,R.[DocumentTypeId]
      ,R.[DocumentTypeCode]
      ,R.[DocDate]
      ,R.[DocNo]
      ,R.[SupplierCustomerId]
      ,R.[Customer]
      ,R.[ItemId]
      ,R.[ItemCode]
      ,R.[ItemName]
      ,R.[CropBatch]
      ,R.[JobLotId]
      ,R.[JobLotCode]
      ,R.[ItemUom]
      ,R.[UOMCode]
      ,R.[InvPackingTypeId]	
      ,R.[PackTypeCode]
      ,R.[InventoryParentCategoriesId]
      ,R.[ItemCategoryId]
      ,R.[CityName]
      ,R.[OrderQty]
      ,R.[DispatchQty]
      ,R.[BalQty]
      ,R.[OrderWeight]
      ,R.[DispatchWeight]
      ,R.[BalWeight]
      ,R.[OrderAmount]
      ,R.[DispatchAmount]
      ,R.[BalAmount]
      ,R.[AvgRate]
      ,R.[PrcntOfTotal]
      ,R.[ReferencePartyDetailId]
      ,R.[ReferencePartyDetail]
      ,R.[ReferencePartyHeaderId]
      ,R.[ReferencePartyHeader]
      ,R.[CompCountry]
      ,R.[CompContactPerson]
      ,R.[CompMobileA]
      ,R.[CompMobileB]
      ,R.[CompMobileC]
      ,R.[CompEmailA]
      ,R.[CompEmailB]
      ,R.[CompLogoImage]
      ,R.[CompanyWebsite]
      ,R.[CompanyFaxNo]
      ,R.[OrgReportingRemarks]
      ,R.[DiscountAmount]
      ,R.[LabourAmount]
      ,R.[CarriageAmount]
      ,R.[CommissionAmount]
      ,R.[NetOrderAmount]
      ,R.[ReferenceNo]
      ,R.[CommissionAgent]
      ,R.[CommissionAgentId]
      ,R.[UomEquivalent]
      ,R.[ReportCriteria]
      ,R.[RefPartyCellNo]
      ,R.[RefPartyAddress]
      ,R.[Distance]
      ,R.[BuildingStorey]
      ,R.[BuildingHeight]
      ,R.[BuildingArea]
      ,R.[BranchSrNo]
      ,R.[BranchId]
      ,R.[RecordNo]
	  ,bb.BranchName
	  ,R.CostCenterId
	  ,R.CostCenterName,R.BookingPerson
	  ,SUM(R.OrderQty) OVER() AS TotalOrderQty
      ,SUM(R.DispatchQty) OVER() AS TotalDispatchQty
      ,SUM(R.BalQty) OVER() AS TotalBalQty
      ,SUM(R.OrderWeight) OVER() AS TotalOrderWeight
      ,SUM(R.DispatchWeight) OVER() AS TotalDispatchWeight
	  ,SUM(R.BalWeight) OVER() AS TotalBalWeight
      ,SUM(R.OrderAmount) OVER() AS TotalOrderAmount
      ,SUM(R.DispatchAmount) OVER() AS TotalDispatchAmount
      ,SUM(R.BalAmount) OVER() AS TotalBalAmount
      ,SUM(R.DiscountAmount) OVER() AS TotalDiscountAmount
      ,SUM(R.LabourAmount) OVER() AS TotalLabourAmount
	  ,SUM(R.CommissionAmount) OVER() AS TotalCommissionAmount
	  ,SUM(R.CarriageAmount) OVER() AS TotalCarriageAmount
	  ,SUM(R.NetOrderAmount) OVER() AS TotalNetOrderAmount
	  ,COUNT(R.RecordNo) OVER() AS row_count

from #SaleOrderRegisterTemp R
		LEFT JOIN Branches bb on bb.Id=r.BranchId
ORDER BY R.RecordNo ASC 
OFFSET (@PageNumber-1)*@PageSize ROWS FETCH NEXT @PageSize ROWS ONLY

DROP TABLE #SaleOrderRegisterTemp;
DROP TABLE #tmpResult;
DROP TABLE #TmpDispatches;
DROP TABLE #tmpDispatchWt;
