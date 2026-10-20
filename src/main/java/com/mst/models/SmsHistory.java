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

/**
 * Mapped 1:1 onto dbo.SmsHistory (source: Sp_ItemPricingSchedule_Insert, which inserts a "Price list published" row).
 * The procedure never supplies an Id, so the Id column is assumed to be an identity column.
 */
@Entity
@Setter
@Getter
@Table(name = "SmsHistory")
public class SmsHistory implements Serializable {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "Id")
    private Integer id;

    @Column(name = "SmsDate")
    private LocalDateTime smsDate;

    @Column(name = "SmsSourceDoc")
    private String smsSourceDoc;

    @Column(name = "SmsSourceId")
    private Integer smsSourceId;

    @Column(name = "SmsBody")
    private String smsBody;

    @Column(name = "SmsCellNo")
    private String smsCellNo;

    @Column(name = "SendStatus")
    private Integer sendStatus;

    @Column(name = "EntryDate")
    private LocalDateTime entryDate;

    @Column(name = "EntryUser")
    private Integer entryUser;

    @Column(name = "OrganizationId")
    private Integer organizationId;

    @Column(name = "CompanyId")
    private Integer companyId;

    @Column(name = "ScreenName")
    private String screenName;

    @Column(name = "ToUserId")
    private Integer toUserId;

    @Column(name = "ReceiverId")
    private Integer receiverId;

    @Column(name = "AutoManual")
    private String autoManual;

    @Column(name = "SupplierCustomerId")
    private String supplierCustomerId;
}
