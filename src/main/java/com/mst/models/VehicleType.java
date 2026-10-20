package com.mst.models;

import java.io.Serializable;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;
import javax.persistence.Table;

import lombok.Getter;
import lombok.Setter;

/**
 * Mapped onto dbo.VehicleType (source: Sp_VehicleType_GetAllMethod, which is a bare SELECT * and names no columns).
 * Column names are taken from the existing project code that consumes this procedure
 * (PurchaseGrnLookupRepository binds the rows as "Id" / "VehicleDescription").
 */
@Entity
@Setter
@Getter
@Table(name = "VehicleType")
public class VehicleType implements Serializable {

    @Id
    @Column(name = "Id")
    private Integer id;

    @Column(name = "VehicleDescription")
    private String vehicleDescription;
}
