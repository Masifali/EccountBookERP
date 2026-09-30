package com.mst.models;

import java.time.LocalDate;

/** Inputs of the five native Packing Material reports; tenant/user never come from the browser. */
public record PackingReportFilter(LocalDate fromDate, LocalDate toDate, String branches,
        Integer supplierId, Integer itemId, Integer warehouseId, String city,
        Integer fromNo, Integer toNo, Integer gpFrom, Integer gpTo, Integer orderFrom, Integer orderTo,
        Integer documentTypeId, Integer itemTypeId, Integer conditionId, Integer rackId,
        String status, String approval, String mode) {
    public void validate() {
        if (fromDate == null || toDate == null) throw new IllegalArgumentException("Select both dates");
        if (fromDate.isAfter(toDate)) throw new IllegalArgumentException("From date must not be after To date");
        Integer[] numbers = {supplierId,itemId,warehouseId,fromNo,toNo,gpFrom,gpTo,orderFrom,orderTo,documentTypeId,itemTypeId,conditionId,rackId};
        for (Integer number : numbers) if (number != null && number < 0) throw new IllegalArgumentException("Numbers cannot be negative");
        range(fromNo,toNo); range(gpFrom,gpTo); range(orderFrom,orderTo);
    }
    private static void range(Integer from, Integer to) {
        if (from != null && to != null && from > 0 && to > 0 && from > to)
            throw new IllegalArgumentException("From number must not exceed To number");
    }
}
