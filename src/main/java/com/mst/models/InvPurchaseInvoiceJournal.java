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

/** Mapped 1:1 onto dbo.InvPurchaseInvoiceJournal (source: Sp_InvPurchaseInvoiceJournal_Insert, returns SCOPE_IDENTITY()). */
@Entity
@Setter
@Getter
@Table(name = "InvPurchaseInvoiceJournal")
public class InvPurchaseInvoiceJournal implements Serializable {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "Id")
    private Integer id;

    @Column(name = "InvPurchaseInvoiceId")
    private Integer invPurchaseInvoiceId;

    @Column(name = "ChartofAccountId")
    private Integer chartofAccountId;

    @Column(name = "JvRemarks")
    private String jvRemarks;

    @Column(name = "JvDebit")
    private Double jvDebit;

    @Column(name = "JvCredit")
    private Double jvCredit;

    @Column(name = "JvPrcnt")
    private Double jvPrcnt;

    @Column(name = "JvQty")
    private Double jvQty;

    @Column(name = "JvRate")
    private Double jvRate;

    @Column(name = "InvGrnId")
    private Integer invGrnId;

    @Column(name = "FreightId")
    private Integer freightId;

    @Column(name = "SupplierCustomerId")
    private Integer supplierCustomerId;

    @Column(name = "TransporterSupCustId")
    private Integer transporterSupCustId;

    @Column(name = "RowType")
    private Integer rowType;

    @Column(name = "JobLotCode")
    private String jobLotCode;

    @Column(name = "JobLotDescription")
    private String jobLotDescription;

    @Column(name = "RefInvoiceNo")
    private Integer refInvoiceNo;

    @Column(name = "OrganizationId")
    private Integer organizationId;

    @Column(name = "CompanyId")
    private Integer companyId;

    @Column(name = "IsSettled")
    private Boolean isSettled;

    @Column(name = "IsApproved")
    private Boolean isApproved;

}
