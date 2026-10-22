package com.mst.projections;

import java.math.BigDecimal;
import java.sql.Date;

/** Row of Sp_InventoryEBGLBySupplierandItem_Rpt (single branch; native-EXEC, final SELECT from #AllData) */
public interface InventoryEbGlBySupplierAndItemRow {
    Integer getId();
    /** #AllData.Doc_Date is declared DATE */
    Date getDoc_Date();
    Integer getDocumentTypeId();
    /** dt.DocumentTypeCode AS [Type] */
    String getType();
    Integer getDoc_No();
    String getSupplier_Name();
    String getItem_Name();
    String getTran_Desc();
    BigDecimal getReceived();
    BigDecimal getIssued();
    /** running balance: SUM(Received - Issued) OVER (PARTITION BY SupplierCustomerId, ItemId ORDER BY Doc_Date, DocumentTypeId, Doc_No) */
    BigDecimal getCBalance();
    String getEmptyBagsType();
    String getBagsCondition();
    byte[] getCompLogoImage();
}
