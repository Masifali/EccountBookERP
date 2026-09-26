package com.mst.models.dto;

import java.util.ArrayList;
import java.util.List;

/**
 * Screen 281 "Production Job Order" — the payload of
 * {@code Architecture.WinApp.Production.frmProductionJobOrderMain}.
 *
 * ---------------------------------------------------------------------------------------------
 * WHY THIS EXISTS ALONGSIDE ProductionJobOrderDto
 * ---------------------------------------------------------------------------------------------
 * {@link ProductionJobOrderDto} was written against {@code frmProductionJobOrder.cs}, which is a
 * DIFFERENT form. dbo.ScreenDefinition row 281 carries
 * {@code TargetUrl = Architecture.WinApp.Production.frmProductionJobOrderMain}, and TargetUrl is
 * what the desktop menu reflects on to open a screen
 * ({@code Activator.CreateInstance(Type.GetType(obj.TargetUrl), UserAccount)},
 * DashboardNew.cs:1571). The two forms share the same HEADER model but fill entirely different
 * child collections, so the header shape below is the same and the details are not.
 *
 * ---------------------------------------------------------------------------------------------
 * FIELD ORDER IS THE CONTRACT
 * ---------------------------------------------------------------------------------------------
 * {@code GenericProvider.SetProc} reflects over the model's NON-VIRTUAL properties and sends one
 * parameter per property, so the procedure's parameter list IS the property list. The header
 * fields below are in the declaration order of
 * {@code Architecture.Model.Production.InvProductionJobOrder} (model 0479), and the child fields
 * in the order of their own models. A {@code virtual} property is skipped by SetProc — those are
 * carried here for display only and named in each class's SKIP set.
 *
 * ---------------------------------------------------------------------------------------------
 * WHAT THIS SCREEN DOES **NOT** SEND
 * ---------------------------------------------------------------------------------------------
 * Insert():1764 initialises seven child lists and fills only three. Input, Output, Packing
 * Material, Overheads and Lab Standard stay EMPTY on this screen — it has no Input or Output grid
 * at all. The DAL loops over them regardless, so an empty list simply writes nothing. They are
 * deliberately absent from this DTO: adding them would let a caller post rows the desktop screen
 * cannot produce.
 */
public class ProductionJobOrderMainDto {

    // ---------------------------------------------------------------- header (model 0479 order)

    public Boolean IsApproved            = Boolean.FALSE;   // Insert():1755 - always false
    public String  EndDate;
    public String  EntryDate;
    public String  ModifyDate;
    public String  PlanDate;
    public String  StartDate;
    public Boolean PendingForView        = Boolean.FALSE;
    public Integer ActionId              = 0;               // 1 = insert, 2 = update (BLL 0335:18)
    public Integer DocumentTypeId        = 0;               // 403, forced by the service
    public Integer BranchesId            = 0;
    public Integer CompanyId             = 0;
    public Integer EntryUser             = 0;
    public Integer Id                    = 0;
    public Integer InvProductionPlantId  = 0;               // Plant / Feader
    public Integer ModifyUser            = 0;
    public Integer OrganizationId        = 0;
    public Integer PlanCode              = 0;
    public Integer PlanTypeSrNo          = 0;
    public Integer ProjectsId            = 0;
    public Integer RefSalesOrderId       = 0;
    public Integer SupplierCustomerId    = 0;               // Insert():1756 - always 0
    public String  DeliveryInstructions;
    public String  LotReference;                            // "Other Ref" on screen
    public String  OtherInstructions;
    public String  PackingInstructions;
    public String  PlanStatus;                              // caption text, not an id
    public String  PlanType;                                // caption text, not an id
    public String  ProductionInsturction;                   // the desktop's own spelling
    public String  ProductionType;                          // caption text, not an id
    public String  RefInvoiceNo;                            // "Production #"
    public Integer WorkInProccessAcId    = 0;
    public Integer FinishGoodsAcId       = 0;
    public Integer ByProductacId         = 0;
    public Integer JobLotId              = 0;
    public Integer WipItemId             = 0;
    public Integer WipWareHouseId        = 0;               // "Wip Warehouse"
    public Integer FinancialYearId       = 0;
    public Double  FinishGoodRate        = 0d;              // "Average Finish Rate"

    // ---------------------------------------------------------------- the three child collections

    public List<Plant>           InvProductionJobOrderPlantslist            = new ArrayList<>();
    public List<RateSchedule>    JobOrderRateSchedulelist                   = new ArrayList<>();
    public List<OrderAllocation> InvProductionJobOrderAndOrderAllocationList = new ArrayList<>();

    /**
     * "(Allocate Multiple Plants To a Job Order)" — model 0461
     * {@code Architecture.Model.Production.InvProductionJobOrderPlant}.
     *
     * NOTE this is the Production model, not
     * {@code Architecture.Model.Steel.Production.InvProductionJobOrderPlant} (model 0162), which
     * has BranchesId and ActionTypeId and no AssumedPlantCapacity. The two differ, and screen 281
     * is a Production screen.
     *
     * Written by {@code Sp_InvProductionJobOrderPlant_Insert}; read back by
     * {@code Sp_InvProductionJobOrder_GetAllMethod @Activity='GetPlantDetailByHeaderId'}.
     */
    public static class Plant {
        public Integer Id                   = 0;
        public Integer InvProductionJobOrderId = 0;         // set from the header id by the DAL
        public Integer PlantId              = 0;
        public java.math.BigDecimal AssumedPlantCapacity = java.math.BigDecimal.ZERO;
        public String  RemarksDetail;
        /** virtual — display only, never sent to the procedure. */
        public String  PlantName;
    }

    /**
     * The two rate grids, in ONE list — model 0460
     * {@code Architecture.Model.Production.JobOrderRateSchedule}.
     *
     * {@code TransTypeId} is the discriminator: **1 = By Product Rate Schedule,
     * 2 = Finish Goods Rate Schedule** (Insert():1798 and :1816). ReadById:1951 splits the list
     * back into the two grids on the same value.
     *
     * The parent key on this one is **{@code JobOrderId}**, not InvProductionJobOrderId
     * (DAL 0273:85) — the only child of the three that differs.
     *
     * Written by {@code USP_JobOrderRateSchedule_Insert}; read back by
     * {@code Sp_InvProductionJobOrder_GetAllMethod @Activity='GetJobScheduleByHeaderId'}.
     */
    public static class RateSchedule {
        public String  EffectiveDate;
        public Double  ItemRate   = 0d;                     // the grid's "Rate 40Kg"
        public Integer Id         = 0;
        public Integer ItemId     = 0;
        public Integer JobOrderId = 0;                      // parent — see above
        public Integer TransTypeId = 0;                     // 1 By Product | 2 Finish Goods
        public String  Remarks;
        /** virtual — display only. */
        public String  ItemName;
    }

    /**
     * "Export Schedule Info" — model 0451
     * {@code Architecture.Model.Production.InvProductionJobOrderAndOrderAllocation}.
     *
     * {@code OrderTypeId} is fixed at **1** by Insert():1825.
     *
     * Written by {@code USP_InvProductionJobOrderAndOrderAllocation_Insert_Update}; read back by
     * {@code [dbo].[Sp_InvProductionJobOrderAndOrderAllocation_GetAllMethod] @Activity='ReadById'}.
     */
    public static class OrderAllocation {
        public Double  MTon        = 0d;
        public Integer Id          = 0;
        public Integer InvProductionJobOrderId = 0;
        public Integer ItemId      = 0;
        public Integer OrderId     = 0;                     // the grid's ContractId
        public Integer OrderTypeId = 0;                     // always 1
        public Integer ScheduleId  = 0;
        public String  Remarks;
        /** virtual — display only. */
        public String  ItemName;
        public String  ContractNo;
        public String  ScheduleNo;
    }
}
