package com.mst.models;

import java.io.Serializable;
import java.time.LocalDateTime;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
import javax.persistence.Table;

import lombok.Getter;
import lombok.Setter;

/** Mapped 1:1 onto dbo.ChequePrintList (source: Sp_VoucherDetail_Insert, cheque-printing block for DocumentTypeId = 2). Id is an IDENTITY column (proc never supplies it). */
@Entity
@Setter
@Getter
@Table(name = "ChequePrintList")
public class ChequePrintList implements Serializable {

    private static final long serialVersionUID = 1L;

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "Id")
    private Integer id;

    @Column(name = "VoucherId")
    private Integer voucherId;

    @Column(name = "VoucherDetailId")
    private Integer voucherDetailId;

    @Column(name = "AccountId")
    private Integer accountId;

    @Column(name = "BankId")
    private Integer bankId;

    @Column(name = "ChequeId")
    private Integer chequeId;

    @Column(name = "ChequeNumber")
    private String chequeNumber;

    @Column(name = "ChequeStatus")
    private Boolean chequeStatus;

    @Column(name = "ChequeType")
    private Integer chequeType;

    @Column(name = "ChequeDate")
    private LocalDateTime chequeDate;

    @Column(name = "PayeeTitle")
    private String payeeTitle;

    @Column(name = "PrintStatus")
    private Boolean printStatus;

    @Column(name = "EntryUser")
    private Integer entryUser;

    @Column(name = "EntryDate")
    private LocalDateTime entryDate;

    @Column(name = "ModifyUser")
    private Integer modifyUser;

    @Column(name = "ModifyDate")
    private LocalDateTime modifyDate;

    @Column(name = "OrganizationId")
    private Integer organizationId;

    @Column(name = "CompanyId")
    private Integer companyId;

    @Column(name = "D1")
    private String d1;

    @Column(name = "D2")
    private String d2;

    @Column(name = "M1")
    private String m1;

    @Column(name = "M2")
    private String m2;

    @Column(name = "Y1")
    private String y1;

    @Column(name = "Y2")
    private String y2;

    @Column(name = "Y3")
    private String y3;

    @Column(name = "Y4")
    private String y4;

    @Column(name = "Amount")
    private Double amount;

    @Column(name = "AmountInWords")
    private String amountInWords;

}
