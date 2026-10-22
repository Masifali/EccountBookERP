package com.mst.models;

import java.io.Serializable;
import java.time.LocalDate;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
import javax.persistence.Table;

import lombok.Getter;
import lombok.Setter;

/** Mapped 1:1 onto dbo.VoucherDetail_H, the history copy of VoucherDetail (source: Sp_VoucherDetail_H_Insert). Id is an IDENTITY column (proc returns SCOPE_IDENTITY()). */
@Entity
@Setter
@Getter
@Table(name = "VoucherDetail_H")
public class VoucherDetail_H implements Serializable {

    private static final long serialVersionUID = 1L;

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "Id")
    private Integer id;

    @Column(name = "VoucherHeadId")
    private Integer voucherHeadId;

    @Column(name = "AccountId")
    private Integer accountId;

    @Column(name = "AgainstAccountId")
    private Integer againstAccountId;

    @Column(name = "Comments")
    private String comments;

    @Column(name = "CommentsOtherLingo")
    private String commentsOtherLingo;

    @Column(name = "DebitAmount")
    private Double debitAmount;

    @Column(name = "CreditAmount")
    private Double creditAmount;

    @Column(name = "JobLotId")
    private Integer jobLotId;

    @Column(name = "RefInvoiceNo")
    private String refInvoiceNo;

    @Column(name = "TaxesTotalAmount")
    private Double taxesTotalAmount;

    @Column(name = "TaxesRemarks")
    private String taxesRemarks;

    @Column(name = "IsTaxable")
    private String isTaxable;

    @Column(name = "TaxTypeId")
    private Integer taxTypeId;

    @Column(name = "TaxPrcnt")
    private Double taxPrcnt;

    @Column(name = "DCheqDate")
    private LocalDate dCheqDate;

    @Column(name = "CheqNoDetail")
    private String cheqNoDetail;

    @Column(name = "DocumentTypeIdRef")
    private Integer documentTypeIdRef;

    @Column(name = "InvoiceNoRefId")
    private Integer invoiceNoRefId;

    @Column(name = "ItemId")
    private Integer itemId;

    @Column(name = "OrderNo")
    private Integer orderNo;

    @Column(name = "GpNo")
    private Integer gpNo;

    @Column(name = "VehicleNo")
    private String vehicleNo;

    @Column(name = "GpDate")
    private LocalDate gpDate;

    @Column(name = "QtyIn")
    private Double qtyIn;

    @Column(name = "QtyOut")
    private Double qtyOut;

    @Column(name = "WeightIn")
    private Double weightIn;

    @Column(name = "WeightOut")
    private Double weightOut;

    @Column(name = "SupplierCustomerId")
    private Integer supplierCustomerId;

    @Column(name = "ItemRate")
    private Double itemRate;

    @Column(name = "RateCut")
    private Double rateCut;

    @Column(name = "RateCutAmount")
    private Double rateCutAmount;

    @Column(name = "ItemAmount")
    private Double itemAmount;

    @Column(name = "Expenses")
    private Double expenses;

    @Column(name = "Freight")
    private Double freight;

    @Column(name = "Journal")
    private Double journal;

    @Column(name = "Commission")
    private Double commission;

    @Column(name = "DMultiCurrencyId")
    private Integer dMultiCurrencyId;

    @Column(name = "DConversionFormula")
    private String dConversionFormula;

    @Column(name = "DExchangeCurrencyRate")
    private Double dExchangeCurrencyRate;

    @Column(name = "DCurrencyAmount")
    private Double dCurrencyAmount;

    @Column(name = "PaymentType")
    private String paymentType;

    @Column(name = "AdvanceAmount")
    private Double advanceAmount;

    @Column(name = "WhtHolding")
    private Double whtHolding;

    @Column(name = "SaleTax")
    private Double saleTax;

    @Column(name = "ExTax")
    private Double exTax;

    @Column(name = "Adjustment")
    private Double adjustment;

    @Column(name = "ActionId")
    private Integer actionId;

    @Column(name = "TotalPaidAmount")
    private Integer totalPaidAmount;

    @Column(name = "InvoiceAmount")
    private Integer invoiceAmount;

    @Column(name = "PaymentTypeId")
    private Integer paymentTypeId;

    @Column(name = "TaxAmount")
    private Double taxAmount;

}
