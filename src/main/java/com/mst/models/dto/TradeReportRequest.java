package com.mst.models.dto;

import java.math.BigDecimal;
import java.time.LocalDate;
import lombok.Data;
import org.springframework.format.annotation.DateTimeFormat;

@Data
public class TradeReportRequest {
    @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) private LocalDate fromDate;
    @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) private LocalDate toDate;
    private int accountClass = 3;
    private boolean balanceClassification;
    private int balanceClass;
    private boolean credit = true;
    private boolean debit;
    private boolean tradeOnly;
    private boolean approved;
    private BigDecimal balanceFrom = BigDecimal.ZERO;
    private BigDecimal balanceTo = BigDecimal.ZERO;
    private String controlAccounts = "";
    private String inventoryGroups = "";
    private String branches = "";
    private int cityId;
    private int customGroupId;
    private int accountId;
    private int subsidiaryId;
    private int languageId;
    private int costCenterId;
}
