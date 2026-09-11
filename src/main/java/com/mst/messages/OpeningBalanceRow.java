package com.mst.messages;

import java.math.BigDecimal;
import lombok.Data;

@Data
public class OpeningBalanceRow {
    private Integer accountId;
    private String accountCode;
    private String accountTitle;
    private Double debit;
    private Double credit;
    private BigDecimal openingDebit;
    private BigDecimal openingCredit;
}
