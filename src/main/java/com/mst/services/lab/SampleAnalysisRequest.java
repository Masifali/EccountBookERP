package com.mst.services.lab;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;

import java.util.ArrayList;
import java.util.List;

/**
 * The body of POST /api/lab/sample-analysis/save — what InvLabSampleAnalysis.Insert() (:1035) reads
 * from its controls. No tenancy field: organization, company and user come from the session. The
 * Doc No is not posted either (the server generates / keeps it).
 */
@JsonIgnoreProperties(ignoreUnknown = true)
public class SampleAnalysisRequest {

    /** RecId — 0 for Save (Save_Click :1208 resets it), the opened record for Update. */
    public int id;
    /** txtdocdate, yyyy-MM-dd. */
    public String docDate;
    /** txtManaulReportNo. */
    public String reportNo;
    /** txtPartyLotRefNo. */
    public String partyLotRefNo;
    /** txtQty text (Conversion.ToDouble on the server). */
    public String qty;
    /** CmbSampleLogNo.Value. */
    public int sampleLogId;
    /** CmbSupplierName.Value. */
    public int supplierId;
    /** CmbCommissionAgent.Value. */
    public int commissionAgentId;
    /** CmbPurchaseOrder.Value. */
    public int orderId;
    /** CmbAnalysisType.Value / .Text (the text is what formvalidation :817 tests). */
    public int analysisTypeId;
    public String analysisTypeText;
    /** CmbAnalysisGroup.Value / .Text (formvalidation :823). */
    public int analysisGroupId;
    public String analysisGroupText;
    /** CmbItemName.Value. */
    public int itemId;
    /** CmbCropYear.Text (:1095 — the text is saved, not the Id). */
    public String cropText;
    /** CmbJobLot.Value. */
    public int jobLotId;
    /** CmbStatus.Text (:1098 — "Accepted" => IsAccepted). */
    public String statusText;
    /** txtremarks. */
    public String remarks;
    /** detaillst. */
    public List<Detail> details = new ArrayList<>();
    /** PicBAnalysisImage / fileSavePath + ofd. */
    public Picture analysisPic;
    /** PicBCookingImage / fileSavePathcookingpic + ofdcookingpic. */
    public Picture cookingPic;
    /** The answer to "Are you sure to Save?" / "Are you sure to Update?" (:1048, :1054). */
    public boolean confirm;

    @JsonIgnoreProperties(ignoreUnknown = true)
    public static class Detail {
        /** InvLabSampleAnalysisDetail.Id — 0 for a row built from the group, the stored id for a loaded row. */
        public int id;
        public int invLabAnalysisItemsId;
        public double resultValue;
        public String remarksDetail;
        public List<Sub> subs = new ArrayList<>();
    }

    @JsonIgnoreProperties(ignoreUnknown = true)
    public static class Sub {
        public int id;
        public int subParameterId;
        public double resultValue;
    }

    @JsonIgnoreProperties(ignoreUnknown = true)
    public static class Picture {
        /** "new" = a file was browsed; "keep" = the stored picture is still shown; anything else = no picture. */
        public String mode;
        /** Original file name of a browsed picture (only its extension is used, :1742). */
        public String name;
        /** Base64 content of a browsed picture. */
        public String data;
    }
}
