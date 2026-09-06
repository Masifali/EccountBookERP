package com.accountbook.erp.service;

import com.accountbook.erp.model.BankDefinition;
import com.accountbook.erp.model.ScreenCard;
import com.accountbook.erp.model.SubModuleCard;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.List;
import java.util.stream.Collectors;

@Service
public class AccountsService {

    public List<SubModuleCard> getSubModules() {
        List<SubModuleCard> list = new ArrayList<>();
        list.add(new SubModuleCard(1L, "Account Definition", 8, "bank"));
        list.add(new SubModuleCard(2L, "Accounts Transaction", 18, "bank"));
        list.add(new SubModuleCard(3L, "Account Reports", 27, "bank"));
        list.add(new SubModuleCard(4L, "Banking Managment", 7, "bank"));
        return list;
    }

    public List<ScreenCard> getScreenCards(String submodule) {
        List<ScreenCard> list = new ArrayList<>();
        list.add(new ScreenCard("scr_1", "Define Bank", "/accounts/bank/define", "Banking Managment", false));
        list.add(new ScreenCard("scr_2", "Bank Payment Voucher", "/accounts/bank/payment-voucher", "Banking Managment", false));
        list.add(new ScreenCard("scr_3", "Bank Receipt Voucher", "/accounts/bank/receipt-voucher", "Banking Managment", false));
        list.add(new ScreenCard("scr_4", "Bank Balance Entry", "/accounts/bank/balance-entry", "Banking Managment", false));
        list.add(new ScreenCard("scr_5", "Cheque Printing", "/accounts/bank/cheque-printing", "Banking Managment", false));
        list.add(new ScreenCard("scr_6", "Bank Balances Report", "/accounts/bank/balances-report", "Banking Managment", false));
        list.add(new ScreenCard("scr_7", "General Ledger Report", "/accounts/reports/general-ledger", "Banking Managment", false));

        if (submodule != null && !submodule.isEmpty()) {
            return list.stream()
                    .filter(s -> s.getCategory().equalsIgnoreCase(submodule))
                    .collect(Collectors.toList());
        }
        return list;
    }

    public BankDefinition saveBankDefinition(BankDefinition bank) {
        // Business logic or database save
        return bank;
    }
}
