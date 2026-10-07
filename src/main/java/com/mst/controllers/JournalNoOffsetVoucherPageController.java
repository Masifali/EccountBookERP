package com.mst.controllers;

import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.GetMapping;

/**
 * Page of screen 914 JournalVoucher_New (Journal Voucher (without Offset)). The literal route wins over the
 * generic /accounts/vouchers/{voucherType} mapping.
 */
@Controller
public class JournalNoOffsetVoucherPageController {

    @GetMapping("/accounts/vouchers/journal-no-offset")
    public String journalNoOffset() {
        return "accounts/vouchers/journal_no_offset_voucher";
    }
}
