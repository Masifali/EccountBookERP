package com.mst.models.dto;

import java.util.ArrayList;
import java.util.List;

public class PurchaseOrderAttachmentsDto {
    public List<InventoryPosItemRequest.Upload> files = new ArrayList<>();
    public List<Integer> removeAttachmentIds = new ArrayList<>();
}
