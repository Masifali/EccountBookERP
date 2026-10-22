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

/** Mapped 1:1 onto dbo.InvPurchaseInvoiceExpense (source: Sp_InvPurchaseInvoiceExpense_Insert, returns SCOPE_IDENTITY()). */
@Entity
@Setter
@Getter
@Table(name = "InvPurchaseInvoiceExpense")
public class InvPurchaseInvoiceExpense implements Serializable {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "Id")
    private Integer id;

    @Column(name = "InvPurchaseInvoiceId")
    private Integer invPurchaseInvoiceId;

    @Column(name = "PurchaseOrderId")
    private Integer purchaseOrderId;

    @Column(name = "PurchaseOrderSupplierExpId")
    private Integer purchaseOrderSupplierExpId;

    @Column(name = "InvRevExpItemId")
    private Integer invRevExpItemId;

    @Column(name = "Qty")
    private Double qty;

    @Column(name = "Rate")
    private Double rate;

    @Column(name = "Amount")
    private Double amount;

    @Column(name = "Remarks")
    private String remarks;

    @Column(name = "CustomRemarks")
    private String customRemarks;

    @Column(name = "GdnId")
    private Integer gdnId;

    @Column(name = "GdnExpId")
    private Integer gdnExpId;

    @Column(name = "GrnId")
    private Integer grnId;

    @Column(name = "SupplierDispatchId")
    private Integer supplierDispatchId;

    @Column(name = "SupplierDispatchExpenseId")
    private Integer supplierDispatchExpenseId;

}
