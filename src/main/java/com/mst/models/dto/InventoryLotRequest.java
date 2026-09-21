package com.mst.models.dto;

import java.time.LocalDateTime;
import java.util.List;

/** AcfrmDefineLots uses JobLot, not the unrelated Lot inventory table. */
public class InventoryLotRequest {
    public int id, jobTypeId, accountId, refDocumentTypeId, refDocNoId;
    public String code, description, status;
    public LocalDateTime startDate, endDate;
    public boolean company = true, thirdParty = true;

    public static class Allocation {
        public int branchId;
        public List<Integer> lotIds;
        public boolean allocate;
    }
}
