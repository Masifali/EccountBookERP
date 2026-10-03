package com.mst.services.lab;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import com.mst.models.dto.InventoryPosItemRequest;

import java.util.ArrayList;
import java.util.List;

/**
 * The body of POST /api/lab/sale-analysis/save — what InvLabSaleAnalysis.Insert() (:584) reads from its
 * controls. No tenancy field: organization, company and user come from the session. The Document No is
 * not posted (the server generates / keeps it), and neither are the hidden boxes txtGpId, txtSaleOrderId,
 * txtSampleId and the Customer Code: the server reads them from the gate pass (Save) or from the stored
 * record (Update).
 */
@JsonIgnoreProperties(ignoreUnknown = true)
public class SaleAnalysisRequest {

    /** RecId — 0 for Save (btnSave_Click :575 resets it), the opened record for Update. */
    public int id;
    /** txtdocdate, yyyy-MM-dd. */
    public String docDate;
    /** txtgatepassno.Value / .Text (formvalidation :497 tests the text). */
    public int gatePassId;
    public String gatePassText;
    /** cmbitem.Value. */
    public int itemId;
    /** txtcrop.Text (:623 — the text is saved, not the Id). */
    public String cropText;
    /** cmbanalysisgroup.Value / .Text (formvalidation :509). */
    public int analysisGroupId;
    public String analysisGroupText;
    /** txtAnalystName. */
    public String analystName;
    /** txtAnalyzedBags text (Conversion.ToDouble on the server). */
    public String analyzedBags;
    /** txtcontainerno. */
    public String containerNo;
    /** txtremarks. */
    public String remarks;
    /** chkisaccepted. */
    public boolean accepted;
    /** grd.GetRows() (:636). */
    public List<Detail> details = new ArrayList<>();
    /** sampleanalysispicture: a picture chosen with Browse (ofd / fileSavePath, :1043). */
    public InventoryPosItemRequest.Upload analysisPic;
    /** Update: the loaded record's Analysis Pic is still shown (fileSavePath as ReadById left it, :741). */
    public boolean keepAnalysisPic;
    /** cookingpic: a picture chosen with Browse (ofdcookingpic / fileSavePathcookingpic, :1070). */
    public InventoryPosItemRequest.Upload cookingPic;
    public boolean keepCookingPic;
    /** Attachment dialog: files added with Browse. */
    public List<InventoryPosItemRequest.Upload> files = new ArrayList<>();
    /** Attachment dialog, Update: the stored attachments left in the list. */
    public List<Integer> keepAttachmentIds = new ArrayList<>();
    /** The answer to "Are you sure to Save?" / "Are you sure to Update?" (:602, :607). */
    public boolean confirm;

    @JsonIgnoreProperties(ignoreUnknown = true)
    public static class Detail {
        /** Cells["LabstandardanalysisId"] — InvLabGroupAnalysisStandards.Id. */
        public int standardsId;
        /** Cells["AnalysisResult"] as typed (Conversion.ToDouble on the server, :640). */
        public String result;
        /** Cells["Remarks"].Text (:641). */
        public String remarks;
    }
}
