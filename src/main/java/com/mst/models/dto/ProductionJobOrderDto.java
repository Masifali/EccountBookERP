package com.mst.models.dto;

import java.util.ArrayList;
import java.util.List;

/**
 * Production Job Order - frmProductionJobOrder.cs, DocumentTypeId 401.
 *
 * ---------------------------------------------------------------------------------------------
 * WHY THE FIELD NAMES LOOK LIKE DATABASE PARAMETERS
 * ---------------------------------------------------------------------------------------------
 * The desktop does not hand-write this procedure's parameter list. Its DAL calls
 *
 *     GenericProvider.SetProc(sqlTransaction, obj, "Sp_InvProductionJobOrder_Insert")
 *
 * and SetProc (DAL 0207, :283-312) reflects over the MODEL:
 *
 *     foreach (PropertyInfo p in typeof(T).GetProperties())
 *         if (!p.GetMethod.IsVirtual)
 *             cmd.Parameters.AddWithValue("@" + p.Name, p.GetValue(obj));
 *     return cmd.ExecuteScalar();          // the new Id
 *
 * So the procedure's parameters ARE the model's non-virtual property names, and the virtual ones
 * (the child lists and the display-only columns) are skipped. Every field below is therefore named
 * exactly as Architecture.Model.Production.InvProductionJobOrder names it - 38 header parameters,
 * 24 on the input row, 30 on the output row. Renaming one to something more Java-ish would send a
 * parameter the procedure does not have.
 *
 * ProductionInsturction is spelled that way in the desktop model. It is not a typo to fix here:
 * it is the parameter name.
 */
public class ProductionJobOrderDto {

    /* ---- identity / document ------------------------------------------------------------- */
    public Integer Id = 0;
    /** 401. Taken from the form (frmProductionJobOrder.cs:1624), never from the caller. */
    public Integer DocumentTypeId = 401;
    public Integer PlanCode = 0;
    public String  PlanDate;              // yyyy-MM-dd, never a shifted toISOString value
    public String  PlanType;
    public Integer PlanTypeSrNo = 0;
    public String  PlanStatus;
    public String  ProductionType;

    /* ---- references ----------------------------------------------------------------------- */
    public Integer InvProductionPlantId = 0;
    public String  RefInvoiceNo;
    public String  LotReference;
    public Integer RefSalesOrderId = 0;
    public Integer SupplierCustomerId = 0;   // the form always sends 0 (:1646)

    /* ---- accounts -------------------------------------------------------------------------- */
    public Integer WorkInProccessAcId = 0;
    public Integer FinishGoodsAcId = 0;
    public Integer ByProductacId = 0;
    public Integer JobLotId = 0;
    public Integer WipItemId = 0;
    public Integer WipWareHouseId = 0;
    public Double  FinishGoodRate = 0.0;

    /* ---- dates ----------------------------------------------------------------------------- */
    public String  StartDate;
    public String  EndDate;

    /* ---- instructions ---------------------------------------------------------------------- */
    public String  PackingInstructions;
    public String  ProductionInsturction;
    public String  DeliveryInstructions;
    public String  OtherInstructions;

    /* ---- server-owned. Never read from the request body. ----------------------------------- */
    public Integer OrganizationId = 0;
    public Integer CompanyId = 0;
    public Integer BranchesId = 0;
    public Integer FinancialYearId = 0;
    public Integer EntryUser = 0;
    public Integer ModifyUser = 0;
    public Integer ProjectsId = 0;
    public Integer ActionId = 0;          // 1 = insert, 2 = update (BLL 0335 Save)
    public Boolean IsApproved = Boolean.FALSE;   // the form saves FALSE (:1645) - not auto-approved
    public Boolean PendingForView = Boolean.FALSE;
    /* Set by the BLL immediately before the call (BLL 0335 Save: obj.EntryDate = DateTime.Now;
       obj.ModifyDate = DateTime.Now;). They are still model properties, so SetProc sends
       @EntryDate and @ModifyDate - omitting them would leave the procedure two parameters short.
       Stamped in the repository, never taken from the request. */
    public String  EntryDate;
    public String  ModifyDate;

    /* ---- children -------------------------------------------------------------------------- */
    public List<Input>  invProductionJobOrderInput  = new ArrayList<>();
    public List<Output> invProductionJobOrderOutput = new ArrayList<>();

    /** Sp_InvProductionJobOrderInput_Insert - 24 parameters. */
    public static class Input {
        public Integer Id = 0;
        public Integer InvProductionJobOrderId = 0;
        public Integer ItemId = 0;
        public Integer CropYearId = 0;
        public String  BatchCrop;
        public Integer JobLotId = 0;
        public Double  Qty = 0.0;
        public Integer UomSchIdQty = 0;
        public Double  NetWeight = 0.0;
        public Integer WarehouseId = 0;
        public Integer PackingTypeId = 0;
        public String  RemarksInputDt;
        /* present on the model, not set by this form - sent at their defaults so the parameter
           list still matches the procedure exactly */
        public Double  ItemRate = 0.0;
        public Double  ItemAmount = 0.0;
        public Double  WagesRate = 0.0;
        public Double  WagesAmount = 0.0;
        public Integer RateUomId = 0;
        public Integer WagesAccountId = 0;
        public Integer RefDocumentTypeId = 0;
        public Integer RefDocIdNo = 0;
        public Integer RefDocSubIdNo = 0;
        public String  DocDate;
        public Integer ActionTypeId = 0;
        public Integer DocNo = 0;
    }

    /** Sp_InvProductionJobOrderOutput_Insert - 30 parameters. */
    public static class Output {
        public Integer Id = 0;
        public Integer InvProductionJobOrderId = 0;
        public Integer ItemId = 0;
        public Integer InvPackingTypeId = 0;
        public Double  QtyInner = 0.0;
        public Integer UomSchIdInner = 0;
        public Double  QtyOuter = 0.0;
        public Integer UomSchIdOuter = 0;
        public Double  NetWeight = 0.0;
        public String  PackRemarks;
        public Integer CropYearId = 0;
        public Integer WarehouseId = 0;
        public Integer JobLotId = 0;
        /* model properties this form leaves at their defaults */
        public Double  ItemRate = 0.0;
        public Double  ItemAmount = 0.0;
        public Double  WagesRate = 0.0;
        public Double  WagesAmount = 0.0;
        public Double  PMRate = 0.0;
        public Double  PMAmount = 0.0;
        public Double  OHExpense = 0.0;
        public Double  PMExpense = 0.0;
        public Double  OHGeneralExpense = 0.0;
        public Double  PMGeneralExpense = 0.0;
        public Double  NetRate = 0.0;
        public Double  NetAmount = 0.0;
        public Integer RateUomId = 0;
        public Integer WagesAccountId = 0;
        public Integer PMItemId = 0;
        public Integer EntryTypeId = 0;
        public Integer ActionTypeId = 0;
    }
}
