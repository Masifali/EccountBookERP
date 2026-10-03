package com.mst.services.lab;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;

import java.util.ArrayList;
import java.util.List;

/**
 * Screen 160 "Purchase Analysis" (InvLabPurchaseAnalysis.cs, DocumentTypeId 303) — what the page posts to
 * /api/lab/purchase-analysis/save.
 *
 * Field names are the desktop control names / model property names (public fields: Jackson uses the field
 * name as-is). Boxes the desktop reads AS TEXT (Conversion.ToInt / ToDouble / ToDecimal of .Text) are posted
 * as the text the operator sees, so the checks see what the desktop saw. A combo whose ActiveRow the desktop
 * tests for null is posted as null when nothing is selected.
 *
 * Never taken from here: organization, company, branch, financial year, users, dates of entry, Doc No,
 * the supplier (the gate pass's), the order no (SupCustCode), the purchase order id, the parameter rows'
 * standards ids / limits / status ids — the service reads them from the session, the gate pass and the
 * standards (or the stored record on update).
 */
@JsonIgnoreProperties(ignoreUnknown = true)
public class PurchaseAnalysisDto {

    public Integer Id = 0;                       // RecId (0 = new; btnSave_Click forces 0, :1968)
    /** true once the operator answered Yes to "Are you sure to Save?" / "Are you sure to Update?" (:1816/:1822). */
    public Boolean confirm = false;

    public String  DocDate;                      // txtdocdate, "yyyy-MM-dd"
    public Integer GatePassInwardId;             // cmbGatePassNo.Value (null = no ActiveRow)
    public Integer AnalysisGroupId;              // cmbanalysisgroup.Value (null = no ActiveRow)
    public Integer ItemId;                       // cmbitem.Value (null = no ActiveRow)
    public Integer PoDetailId = 0;               // cmbitem.SelectedRow.Cells["PoDetailId"]
    public String  NoofBagsInspection;           // txtNoofBagsInspection (text)
    public Integer PackingTypeId;                // CmbPackingType.Value (null = no ActiveRow)
    public Integer RateUomId;                    // CmbRateUom.Value (null = no ActiveRow)
    public String  ItemRate;                     // txtItemRate (text)
    public String  AddRate;                      // txtAddRate (text)       -> Premium
    public String  LessRate;                     // txtLessRate (text)      -> DeductionRate when RefDocumentTypeId = 106
    public String  RateCut;                      // txtRateCut (text)       -> DeductionRate otherwise
    public String  NetRate;                      // txtNetRate (text; validated only)
    public String  ScheduleId;                   // txtScheduleId (hidden box, text) -> PricingScheduleId
    public String  AnaylstName;                  // txtAnaylstName
    public String  WeightCut;                    // txtWeightCut (text)     -> DeductionWeight
    public String  WeightCutUom;                 // txtWeightCutUom (text)
    public String  QtyForWtCut;                  // txtQtyForWtCut (text)
    public Integer WarehouseId;                  // CmbWarehouse.Value (null = no ActiveRow)
    public Integer WeightCutOnId;                // CmbWtCuton.Value (null = no ActiveRow)
    public Integer LabStatusId;                  // CmbLabStatus.Value (null = no ActiveRow)
    public String  LabStatusText;                // CmbLabStatus.Text ("Accepted" -> IsAccepted)
    public Integer PestResultId;                 // CmbPestResult.Value (null = no ActiveRow)
    public Integer PestStatusId;                 // CmbPestStatus.Value (null = no ActiveRow)
    public Integer JobLotId = 0;                 // CmbJobLot.Value
    public Integer SampleAnalysisId = 0;         // CmbSampleAnaylsis.Value -> InvLabSampleLogRegisterId
    public String  Crop;                         // cmbCropYear.Text
    public String  PartyLotRefNo;                // PartyLotRefNo
    public String  Remarks;                      // txtremarks
    public Boolean WtCutWillApply = false;       // ChkWtCutWillApply.Checked
    public Boolean RateCutWillApply = false;     // ChkRateCutWillApply.Checked

    /** Analysis Pic (LiveViewPicBox / UniqFileName) and Cooking Pic (cookingpic / UniqFileName2). */
    public Picture AnalysisPic = new Picture();
    public Picture CookingPic = new Picture();

    /** detaillst — the rows still in grd (the grid allows a row to be deleted, designer AllowDelete True). */
    public List<Row> rows = new ArrayList<>();

    @JsonIgnoreProperties(ignoreUnknown = true)
    public static class Picture {
        /** "keep" (the stored name stays), "clear" (Reset button: the name becomes ""), "new" (Browse: a new file). */
        public String action = "keep";
        public String fileName;                  // the chosen file's name (its extension is kept)
        public String dataBase64;                // the chosen file's bytes
    }

    @JsonIgnoreProperties(ignoreUnknown = true)
    public static class Row {
        public Integer InvLabAnalysisItemsId = 0;
        public Double  InAnalysisResult = 0d;
        public String  RemarksDetail;
        public List<Sub> subs = new ArrayList<>();
    }

    @JsonIgnoreProperties(ignoreUnknown = true)
    public static class Sub {
        public Integer SubParameterId = 0;
        public Double  ResultValue = 0d;
    }

    /** One edited "Editable After Approval" parameter of the History detail grid (:2680-2686). */
    @JsonIgnoreProperties(ignoreUnknown = true)
    public static class ParamResult {
        public Integer Id = 0;
        public Double  ResultValue = 0d;
        public String  Remarks;
    }

    /** The History grid's Update button (:2602-2621). */
    @JsonIgnoreProperties(ignoreUnknown = true)
    public static class Cuts {
        public String WeightCut;
        public String RateCut;
    }
}
