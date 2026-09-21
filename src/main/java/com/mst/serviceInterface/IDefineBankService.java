package com.mst.serviceInterface;

import java.util.List;
import java.util.Map;
import com.mst.models.Bank;

public interface IDefineBankService {
    List<Map<String, Object>> getBankList();
    Bank getById(int id);
    Bank saveBank(Bank bank);
    void deleteBank(int id);
}
