package com.mst.models;

import java.io.Serializable;
import java.math.BigDecimal;
import java.time.LocalDate;
import java.time.LocalDateTime;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
import javax.persistence.Table;

import lombok.Getter;
import lombok.Setter;

/** Mapped 1:1 onto dbo.InvPurchaseInvoicePaymentTermsDetail (source: USP_InvPurchaseInvoicePaymentTermsDetail_Insert). Id is app-assigned: proc computes ISNULL(MAX(Id),0)+1 -> use InvPurchaseInvoicePaymentTermsDetailRepository.maxId(). */
@Entity
@Setter
@Getter
@Table(name = "InvPurchaseInvoicePaymentTermsDetail")
public class InvPurchaseInvoicePaymentTermsDetail implements Serializable {

    @Id
    @Column(name = "Id")
    private Integer id;

    @Column(name = "InvPurchaseInvoiceId")
    private Integer invPurchaseInvoiceId;

    @Column(name = "PaymentTermId")
    private Integer paymentTermId;

    @Column(name = "PrcntOfTotal")
    private BigDecimal prcntOfTotal;

    @Column(name = "Amount")
    private BigDecimal amount;

    @Column(name = "DueDays")
    private Integer dueDays;

    @Column(name = "PaymentRemarks")
    private String paymentRemarks;

    @Column(name = "SortNo")
    private Integer sortNo;

    @Column(name = "DueDate")
    private LocalDate dueDate;

    @Column(name = "PurchaseOrderId")
    private Integer purchaseOrderId;

    @Column(name = "SystemGeneratedRow")
    private Boolean systemGeneratedRow;

}
