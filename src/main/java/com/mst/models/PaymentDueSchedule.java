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

/**
 * Mapped 1:1 onto dbo.PaymentDueSchedule (source: Sp_PaymentDueSchedule_Insert / Sp_PaymentDueSchedule_ReadAll).
 * Primary key column is PaymentDueScheduleId (identity: the insert proc returns SCOPE_IDENTITY()).
 */
@Entity
@Setter
@Getter
@Table(name = "PaymentDueSchedule")
public class PaymentDueSchedule implements Serializable {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "PaymentDueScheduleId")
    private Integer paymentDueScheduleId;

    @Column(name = "DueDays")
    private Integer dueDays;

    @Column(name = "DueDate")
    private LocalDate dueDate;

    @Column(name = "DuePercentage")
    private Double duePercentage;

    @Column(name = "Amount")
    private Double amount;

    @Column(name = "refDocumentTypeId")
    private Integer refDocumentTypeId;

    @Column(name = "RefdocNoId")
    private Integer refdocNoId;

    @Column(name = "ActionId")
    private Integer actionId;
}
