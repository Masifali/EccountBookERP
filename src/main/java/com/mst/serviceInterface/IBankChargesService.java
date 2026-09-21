package com.mst.serviceInterface;

import java.util.List;
import java.util.Map;
import com.mst.models.BankCharges;

public interface IBankChargesService {
    List<Map<String, Object>> getBankChargesList();
    BankCharges getById(int id);
    BankCharges saveBankCharges(BankCharges bankCharges);
    void deleteBankCharges(int id);
}
