package com.mst.models.dto;

import java.util.ArrayList;
import java.util.List;

/** Files are staged by the form and persisted with the invoice transaction. */
public class PurchaseInvoiceAttachmentsDto {
    public List<InventoryPosItemRequest.Upload> files=new ArrayList<>();
    public List<Integer> removeAttachmentIds=new ArrayList<>();
}
