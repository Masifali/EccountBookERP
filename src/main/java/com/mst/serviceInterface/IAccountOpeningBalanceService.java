package com.mst.serviceInterface;

import java.math.BigDecimal;
import java.util.List;
import com.mst.messages.OpeningBalanceRow;

public interface IAccountOpeningBalanceService {
    List<OpeningBalanceRow> listRows();
    void save(String accountCode, BigDecimal debit, BigDecimal credit);
    BigDecimal getOpeningDebit(String accountCode);
}
