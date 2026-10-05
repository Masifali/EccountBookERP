-- Desktop sales lookups. Corrects the purchase booking-person insert to (Id, RefName, Activity).
SET NOCOUNT ON;
DECLARE @OrganizationId INT = ?;
DECLARE @CompanyId INT = ?;
DECLARE @Activity NVARCHAR(max) = ?;
DECLARE @ParentIds NVARCHAR(max) = ?;
DECLARE @DocType NVARCHAR(max) = ?;
DECLARE @BranchesIds NVARCHAR(max) = ?;
DECLARE @SupplierCustomerId INT = ?;
DECLARE @AppId INT = ?;
DECLARE @UserId INT = ?;
DECLARE @CostCenterId INT = ?;
DECLARE @CustomDocumentTypeIds NVARCHAR(max) = ?;
SET NOCOUNT ON;

DECLARE @RefDocumentTypeIds nvarchar(max) = null
IF @DocType = 'Sale'
BEGIN
   SET @RefDocumentTypeIds = '95,96,99,100,103,126,1509,1510,1608,1604,1609,1660,1661,1662,1856,1861,1862,1809,1860,211';
END

ELSE IF @DocType = 'Purchase'
BEGIN
   SET @RefDocumentTypeIds = '56,57,63,1502,1503,1604,1603,1865,1859,1653,1654';
END


CREATE TABLE #TmpResult
(
	Id INT,
	RefName NVARCHAR(MAX),
	Activity NVARCHAR(100)
)


select	Distinct 
		sd.ItemId, 
		i.ItemName,
		i.SaleGLAC,
		i.PurchaseGLAC,
		i.COGSGLAC,
		i.ItemTypeId,
		i.ItemCategoryId,
		ic.CategoryDescription,
		ic.InventoryParentCategoriesId,
		ip.InvParentCateDescription,
		ic.ItemClassGroupId,
		sd.WarehouseId,
		sd.JobLotId,
		sd.CropBatch,sd.InvPackingTypeId,
		sd.CityId,
		sd.RefPartyId,
		sd.SupplierCustomerId,
		sd.VehicleNo,
		sd.CastingTypeId,
		sd.VarientId,
		sd.ItemConditionId,
		sd.RefDocumentTypeId,
		sd.RefDocIdNo
INTO #TmpMainData
from	InventoryStockEvalautionDetail sd 
			Inner join Item i on sd.ItemId = i.Id
			inner join ItemCategory ic on i.ItemCategoryId = ic.Id
			inner join InventoryParentCategories ip on ic.InventoryParentCategoriesId = ip.Id
Where	sd.OrganizationId = @OrganizationId
and		sd.CompanyId = @CompanyId
and		(@SupplierCustomerId is null or sd.SupplierCustomerId = @SupplierCustomerId)
and		(@RefDocumentTypeIds IS NULL 
			OR EXISTS (select 1
						from dbo.fnSplitString (@RefDocumentTypeIds,',')
						Where sd.RefDocumentTypeId in (data)
						)
		) 
and		(@CustomDocumentTypeIds IS NULL 
			OR EXISTS (select 1
						from dbo.fnSplitString (@CustomDocumentTypeIds,',')
						Where sd.RefDocumentTypeId in (data)
						)
		) 
and		(@ParentIds IS NULL 
			OR EXISTS (select 1
						from dbo.fnSplitString (@ParentIds,',')
						Where ic.InventoryParentCategoriesId in (data)
						)
		)
and		(
			@BranchesIds IS NULL
			OR EXISTS (select 1
						from dbo.fnSplitString (@BranchesIds,',')
						Where sd.BranchesId in (data)
						)
		)


INSERT INTO #TmpResult
select	Distinct	
		sd.CastingTypeId,
		CT.CastingType,
		'GetCastingType'
from	#TmpMainData sd
			Inner join mfg.vCastingType CT on CT.CastingTypeId = sd.CastingTypeId
Where	(@Activity IS NULL OR 'GetCastingType' = @Activity)


INSERT INTO #TmpResult
select	Distinct	
		sd.VarientId,
		Iv.VarientDescription,
		'ModalDescription'
from	#TmpMainData sd
			Inner join ItemAttributeVarient Iv on Iv.Id = sd.VarientId
Where	(@Activity IS NULL OR 'GetModalDescription' = @Activity)

INSERT INTO #TmpResult
select	Distinct	
		sd.ItemConditionId,
		Ic.ConditionStatus,
		'ItemCondition'
from	#TmpMainData sd
			Inner join V_ItemCondition Ic on Ic.ID = sd.ItemConditionId
Where	(@Activity IS NULL OR 'ItemCondition' = @Activity)


INSERT INTO #TmpResult
select	Distinct 
		sd.ItemId,
		sd.ItemName,
		'GetItems' as Activity
from #TmpMainData sd 
Where		(@Activity IS NULL OR 'GetItems' = @Activity)


INSERT INTO #TmpResult
select	Distinct 
		sd.SaleGLAC,
		c.AccountTitle,
		'GetSalesAccount' as Activity
from #TmpMainData sd 
		inner join ChartofAccount c on sd.SaleGLAC = c.Id
Where		(@Activity IS NULL OR 'GetSalesAccount' = @Activity)


INSERT INTO #TmpResult
select	Distinct 
		sd.COGSGLAC,
		c.AccountTitle,
		'GetCGSAccount' as Activity
from #TmpMainData sd 
		inner join ChartofAccount c on sd.COGSGLAC = c.Id
Where		(@Activity IS NULL OR 'GetCGSAccount' = @Activity)


INSERT INTO #TmpResult
select	Distinct 
		sd.PurchaseGLAC,
		c.AccountTitle,
		'GetStockAccount' as Activity
from #TmpMainData sd 
		inner join ChartofAccount c on sd.PurchaseGLAC = c.Id
Where		(@Activity IS NULL OR 'GetStockAccount' = @Activity)



INSERT INTO #TmpResult
select	Distinct	
		sd.ItemTypeId, 
		it.TypeDescription,'GetItemType'
from #TmpMainData sd
		inner join ItemType it on sd.ItemTypeId = it.Id
Where	(@Activity IS NULL OR 'GetItemType' = @Activity)



INSERT INTO #TmpResult
select	Distinct	
		sd.InventoryParentCategoriesId,
		sd.InvParentCateDescription,
		'GetParentCategory'
from #TmpMainData sd
Where	(@Activity IS NULL OR 'GetParentCategory' = @Activity)


INSERT INTO #TmpResult
select Distinct 
				d.BookingPersonId,
				sp.ReferencePartyName,'SaleBookingPerson'
from		#TmpMainData pih 
Inner Join InvSaleInvoiceDetail pd on pd.InvSaleInvoiceId = pih.RefDocIdNo
Inner Join SaleOrder d on d.Id = pd.SaleOrderId
				inner join  ReferenceParties sp on d.BookingPersonId  = sp.Id
where      (@Activity is null or 'SaleBookingPerson' = @Activity)




INSERT INTO #TmpResult
select	Distinct	
		sd.ItemCategoryId, 
		sd.CategoryDescription,
		'GetItemCategory'
from	#TmpMainData sd
Where	(@Activity IS NULL OR 'GetItemCategory' = @Activity)



INSERT INTO #TmpResult
select	Distinct	
		sd.ItemClassGroupId, 
		cg.ClassGroupName,
		'GetItemClass'
from	#TmpMainData sd
			inner join ItemClassGroup cg on sd.ItemClassGroupId = cg.Id
Where	(@Activity IS NULL OR 'GetItemClass' = @Activity)




INSERT INTO #TmpResult
select	Distinct	
		sd.InvPackingTypeId, 
		w.PackTypeDesc,
		'GetPackingType'
from	#TmpMainData sd
			Inner join InvPackingType w on sd.InvPackingTypeId = w.Id
Where (@Activity IS NULL OR 'GetPackingType' = @Activity)

INSERT INTO #TmpResult
select	Distinct	
		sd.WarehouseId, 
		w.WareHouseName,
		'GetWarehouse'
from	#TmpMainData sd
			Inner join InvWareHouse w on sd.WarehouseId = w.Id
Where (@Activity IS NULL OR 'GetWarehouse' = @Activity)



INSERT INTO #TmpResult
select	Distinct	
		sd.JobLotId, 
		j.JobLotDescription,
		'GetJobLot'
from	#TmpMainData sd
			Inner join JobLot j on sd.JobLotId = j.Id
Where	(@Activity IS NULL OR 'GetJobLot' = @Activity)


INSERT INTO #TmpResult
select	Distinct	
		sd.SupplierCustomerId, 
		sp.CompanyName,
		'GetSupplierCustomer'
from	#TmpMainData sd
			Inner join SupplierCustomer sp on sd.SupplierCustomerId = sp.Id
Where	(@Activity IS NULL OR 'GetSupplierCustomer' = @Activity)


INSERT INTO #TmpResult
select	Distinct	
		ac.CustomGroupId, 
		d.AcLookupsDescription,
		'GetCustomGroups'
from	#TmpMainData sd
		INNER Join PartyCustomGroups ac on ac.SupplierCustomerId = sd.SupplierCustomerId
		Inner Join AcLookups d on d.ID = ac.CustomGroupId
Where	(@Activity IS NULL OR 'GetCustomGroups' = @Activity)




INSERT INTO #TmpResult
select	Distinct	
		c.Id,
		sd.CropBatch,
		'GetCropYear'
from	#TmpMainData sd
			Inner join InvCropYear C on sd.CropBatch = c.CropYear and c.CompanyId=@CompanyId
Where	(@Activity IS NULL OR 'GetCropYear' = @Activity)



INSERT INTO #TmpResult
select	Distinct	
		sd.CityId,
		c.Description as CityName,
		'GetCity'
from	#TmpMainData sd
			Inner join City C on sd.CityId = c.Id
Where	(@Activity IS NULL OR 'GetCity' = @Activity)



INSERT INTO #TmpResult
select	Distinct	
		t.DistrictId,
		d.Description as District,
		'GetDistrict'
from	#TmpMainData sd
			Inner join City C on sd.CityId = c.Id
			inner join Tehsil t on c.TehsilId = t.Id
			inner join District d on t.DistrictId = d.Id
Where	(@Activity IS NULL OR 'GetDistrict' = @Activity)



INSERT INTO #TmpResult
select	Distinct	
		sd.RefPartyId, 
		rp.ReferencePartyName,
		'GetReferenceParties'
from	#TmpMainData sd
			Inner join ReferenceParties rp on sd.RefPartyId = rp.Id
Where	(@Activity IS NULL OR 'GetReferenceParties' = @Activity)



INSERT INTO #TmpResult
select Distinct d.BookingPersonId, sp.ReferencePartyName, 'PurchaseBookingPerson'
from		#TmpMainData pih 
Inner Join InvPurchaseInvoiceDetail pd on pd.InvPurchaseInvoiceId = pih.RefDocIdNo
Inner Join PurchaseOrder d on d.Id = pd.PurchaseOrderId
				inner join  ReferenceParties sp on d.BookingPersonId  = sp.Id
where      (@Activity is null or 'PurchaseBookingPerson' = @Activity)




INSERT INTO #TmpResult
select	Distinct	
	ROW_NUMBER() over(order by (select null)) as VehicleNoId, 
	sd.VehicleNo,
	'GetVehicleNos'
from		#TmpMainData sd
			Left join DefineVehicleWeight d on sd.VehicleNo = d.VehicleNo
Where	(@Activity IS NULL OR 'GetVehicleNos' = @Activity)

INSERT INTO #TmpResult
select	Distinct	
		sd.RefDocumentTypeId,
		DT.DocumentTypeDescription,
		'GetDocumentType'
from	#TmpMainData sd
			Inner join DocumentType DT on DT.Id = sd.RefDocumentTypeId
Where	(@Activity IS NULL OR 'GetDocumentType' = @Activity)


INSERT INTO #TmpResult
select	Distinct	
		H.OtherCategoryId,
		LP.LookupName,
		'OtherCategory'
from	#TmpMainData sd
			Inner join InvSaleInvoice h on h.Id = sd.RefDocIdNo and sd.RefDocumentTypeId = h.DocumentTypeId
			INNER JOIN InvLookUp LP ON H.OtherCategoryId = LP.Id
Where  h.DocumentTypeId = 99
and    (@Activity IS NULL OR 'OtherCategory' = @Activity)



SELECT	R.Id,
		R.RefName,
		R.Activity
FROM #TmpResult R
DROP TABLE #TmpResult;
DROP TABLE #TmpMainData;
