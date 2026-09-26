package com.mst.models.dto;

import java.util.ArrayList;
import java.util.List;

/**
 * Stock Conversion — invfrmStockConversionProduction.cs, header DocTypeId 66.
 *
 * Field names are the desktop model's property names, exactly, because
 * {@code GenericProvider.SetProc} turns each non-virtual property into a parameter of the same
 * name. Renaming one here renames a stored-procedure parameter, so the schema's own spellings are
 * preserved rather than tidied:
 *
 *   BranchedId        not BranchesId
 *   DocSrNo           the document serial, not DocNo
 *   parentCategoryId  lower-case p, as the model declares it
 *   EBDepartmentId    the production department warehouse
 *
 * The virtual properties the desktop model carries — InputDetailRowsRemoveIds and the four child
 * lists — are NOT parameters: SetProc skips virtual members. InputDetailRowsRemoveIds is passed
 * separately to USP_InvStockConversionDetailRowsDeleteByIds, so it is kept here as a plain field
 * and excluded from the header parameter set by name.
 *
 * No column, table or procedure is added by this class. It is a shape for values that already
 * have a home in GoldenAcedb.
 */
public class StockConversionDto {

    // ---------------------------------------------------------------- header (25 parameters)
    public Integer Id = 0;
    public Integer DocTypeId = 66;
    public Integer DocSrNo = 0;
    public String  DocDate;                  // yyyy-MM-dd
    public String  DocManualRef;
    public String  ProductionNo;
    public String  Remarks;
    public Integer ConversionTypeId = 0;
    public Integer GainLossId = 0;
    public Integer parentCategoryId = 0;     // lower-case p on purpose - see the class note
    public Integer EBDepartmentId = 0;
    public Integer DifferenceAccountId = 0;
    public Integer ProjectsId = 0;
    public Boolean PostState = Boolean.FALSE;
    public String  PostDate;
    public Integer PostUser = 0;

    // server-owned; never taken from the request body
    public Integer OrganizationId = 0;
    public Integer CompanyId = 0;
    public Integer BranchedId = 0;
    public Integer FinancialYearId = 0;
    public Integer EntryUser = 0;
    public Integer ModifyUser = 0;
    public String  EntryDate;
    public String  ModifyDate;
    public Integer ActionId = 0;             // 1 = insert, 2 = update

    /** Virtual on the desktop model - passed to the row-delete procedure, not to the header. */
    public String InputDetailRowsRemoveIds;

    public List<Detail> invStockConversionDetails = new ArrayList<>();
    public List<PackingMaterial> invStockConversionPackings = new ArrayList<>();
    public List<AddExpense> invStockConversionAddExpenses = new ArrayList<>();

    // ---------------------------------------------------------------- detail (34 parameters)
    public static class Detail {
        public Integer Id = 0;
        public Integer InvStockConversionId = 0;
        public Integer ItemId = 0;
        public Integer ItemUomId = 0;
        public Integer JobLotId = 0;
        public Integer PackingtypeId = 0;    // desktop spelling: lower-case t
        public Integer PackUnit = 0;
        public Integer RateUOMId = 0;
        public Integer WarehouseId = 0;
        public Integer ItemConditionId = 0;
        public Integer RackId = 0;
        public Integer MoistureSlabId = 0;
        public Integer ProjectId = 0;
        public Integer LineId = 0;
        public Integer SortNo = 0;
        public Integer VoucherHeadId = 0;
        public Integer RefDocumentTypeId = 0;
        public Integer RefDocNoId = 0;
        public Integer RefDocSubId = 0;
        public Integer labIPmActivityLogId = 0;
        public Double  Qty = 0.0;
        public Double  Rate = 0.0;
        public Double  Amount = 0.0;
        public Double  Weight = 0.0;
        public Double  Moisture = 0.0;
        public Double  ExpenseAmount = 0.0;
        public Double  PackingMaterialAmount = 0.0;
        public Double  ItemPmCost = 0.0;
        public Double  ItemOhCost = 0.0;
        public Double  WagesAmount = 0.0;
        public Boolean IsOnHold = Boolean.FALSE;
        public String  CropBatch;
        /** "Issue" / "1" marks a row the inventory guard is run for - DAL 0275 :137. */
        public String  EntryType;
        public String  Remarks;
    }

    // ------------------------------------------------------- packing material (16 parameters)
    public static class PackingMaterial {
        public Integer Id = 0;
        public Integer InvStockConversionId = 0;
        public Integer ItemId = 0;
        public Integer WarehouseId = 0;
        public Integer ItemSchUOM = 0;
        public Integer LineId = 0;
        public Integer BrandItemId = 0;
        public Integer BrandItemUomId = 0;
        public Integer ItemConditionId = 0;
        public Integer ContractScheduleId = 0;
        public Integer ExImInvoiceId = 0;
        public Integer RackId = 0;
        public Double  ItemQty = 0.0;
        public Double  ItemRate = 0.0;
        public Double  ItemAmount = 0.0;
        public String  ChargeTo;
    }

    // ------------------------------------------------------------ add expense (8 parameters)
    public static class AddExpense {
        public Integer Id = 0;
        public Integer InvStockConversionId = 0;
        public Integer ChartOfAccountId = 0;
        public Integer BrandItemId = 0;
        public Integer BrandItemUomId = 0;
        public Double  ExpAmount = 0.0;
        public String  LedgerRemarks;
        public String  ChargeTo;
    }
}
