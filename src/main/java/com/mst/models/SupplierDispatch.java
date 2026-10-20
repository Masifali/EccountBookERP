package com.mst.models;

import java.io.Serializable;
import java.math.BigDecimal;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
import javax.persistence.Table;

import lombok.Getter;
import lombok.Setter;

/**
 * Mapped 1:1 onto dbo.SupplierDispatch (source: USP_SupplierDispatch_GetNoForGpandGrn — read-only usage in this batch;
 * no Insert proc in the batch, so the Id is assumed to be an IDENTITY column).
 */
@Entity
@Setter
@Getter
@Table(name = "SupplierDispatch")
public class SupplierDispatch implements Serializable {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "Id")
    private Integer id;

    @Column(name = "DocNo")
    private Integer docNo;

    @Column(name = "VehicleNo")
    private String vehicleNo;

    @Column(name = "AdvanceFreight")
    private BigDecimal advanceFreight;

    @Column(name = "BiltyNo")
    private String biltyNo;

    @Column(name = "Freight")
    private BigDecimal freight;

    @Column(name = "SupplierWbWeight")
    private BigDecimal supplierWbWeight;

    @Column(name = "CityId")
    private Integer cityId;

    @Column(name = "ActionId")
    private Integer actionId;

    @Column(name = "SupplierDispatchId")
    private Integer supplierDispatchId;

    @Column(name = "SupplierId")
    private Integer supplierId;

    @Column(name = "OrganizationId")
    private Integer organizationId;

    @Column(name = "CompanyId")
    private Integer companyId;

    @Column(name = "Status")
    private String status;
}
