package com.mst.jasperReport;

import com.mst.constants.MSTConstants;
import lombok.Getter;
import lombok.Setter;

import java.math.BigDecimal;
import java.util.Date;

@Setter
@Getter
public class PurchaseVoucher {
    String voucherCode;
    String completeVoucherCode;
    long accountCode;
    String branchName;
    String companyName;
    String branchAddress;
    String debitOrCredit;
    String formattedCode;
    String accountName;
    String partyMobile;
    String partyAddress;
    String itemSubCategoryName;
    String itemDefName;
    String bookNumber;
    String broker = "";
    BigDecimal previousBalance;
    String netBalance;
    String productName;
    String mobile;
    String ntn;
    String email;
    String vehicalNumber;
    String ReportTitle;
    String propName;
    String propMobile;
    String juteBagDescription;
    String plasticBagDescription;
    String printedBy;
    Date voucherDate;
    BigDecimal bags;
    BigDecimal commission;
    BigDecimal invoiceAmount;
    BigDecimal price;
    BigDecimal totalKg;
    int revisions;
    BigDecimal totalBagWeightDeduction;
    String mounds;
    String invoiceDebitOrCredit;
    BigDecimal govtKalta = MSTConstants.BIG_DECIMAL_ZERO;
    BigDecimal totalKatoti;
    BigDecimal safiKg;
    BigDecimal purchaseRate;
    BigDecimal amount;
    BigDecimal millTaxRate;
    BigDecimal millTaxAmount;
    BigDecimal bankTaxRate;
    BigDecimal bankTaxAmount;
    BigDecimal silaiAmount;
    BigDecimal silaiRate;
    BigDecimal bardanaRate;
    BigDecimal bardanaAmount;
    BigDecimal otherExp;
    BigDecimal freightCharges;
    BigDecimal netAmount;

    BigDecimal unloadingCharges;
    BigDecimal unloadingRate;
    BigDecimal brokriAmount = MSTConstants.BIG_DECIMAL_ZERO;
    BigDecimal brokriRate = MSTConstants.BIG_DECIMAL_ZERO;
    BigDecimal juteBag = MSTConstants.BIG_DECIMAL_ZERO;

    BigDecimal juteBagRate = MSTConstants.BIG_DECIMAL_ZERO;

    BigDecimal plasticBag = MSTConstants.BIG_DECIMAL_ZERO;
    BigDecimal plasticBagRate = MSTConstants.BIG_DECIMAL_ZERO;
    BigDecimal plasticBagAmount = MSTConstants.BIG_DECIMAL_ZERO;
    BigDecimal juteBagAmount = MSTConstants.BIG_DECIMAL_ZERO;
    BigDecimal accountClosingBalance = MSTConstants.BIG_DECIMAL_ZERO;
    String accountClosingBalanceInWords = "";
    String permintNumber = "";
    String narration = null;
    private Date createdDate;


}
