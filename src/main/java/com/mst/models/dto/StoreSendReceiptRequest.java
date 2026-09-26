package com.mst.models.dto;

import java.util.ArrayList;
import java.util.List;

/** Screen 504 "Store Send Receipt" - what {@code StoreSendReceipt.Insert()} (:590) reads. */
public class StoreSendReceiptRequest {
    public int Id;
    public String DocDate;
    public int SenderWarehouseId;
    public int ReceiverWarehouseId;
    public List<Line> lines = new ArrayList<>();
    public List<InventoryPosItemRequest.Upload> files = new ArrayList<>();
    public List<Integer> removeAttachmentIds = new ArrayList<>();

    /** One dtDetail row. Rate and amount are recomputed on the server. */
    public static class Line {
        public int Id;
        public int RefDocNoId;
        public int RefDocDetailId;
        public int RefDocumentTypeId;
        public int RecordNo;
        public int ItemId;
        public int ItemConditionId;
        public int PackingTypeId;
        public double ItemQty;
    }
}
