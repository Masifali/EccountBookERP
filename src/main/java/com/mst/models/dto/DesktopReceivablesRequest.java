package com.mst.models.dto;

import java.math.BigDecimal;
import java.time.LocalDate;
import lombok.Data;
import org.springframework.format.annotation.DateTimeFormat;

@Data
public class DesktopReceivablesRequest {
    @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) private LocalDate fromDate;
    @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) private LocalDate toDate;
    @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) private LocalDate dueFrom;
    @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) private LocalDate dueTo;
    @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) private LocalDate saleFrom;
    @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) private LocalDate saleTo;
    private int parentId, customGroupId, cityId, partyGroupId, parentCategoryId, showAssetLiability;
    private String controls = "", groups = "", branches = "";
    private BigDecimal balanceFrom = BigDecimal.ZERO, balanceTo = BigDecimal.ZERO;
    private boolean tradeOnly, skipZero;
}
