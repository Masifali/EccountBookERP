package com.mst.models;

import java.io.Serializable;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;
import javax.persistence.Table;

import lombok.Getter;
import lombok.Setter;

/** Mapped 1:1 onto dbo.GatePassType (source: Sp_GatePassType_GetAllMethod - the only columns the procedure reads are Id and GpTypeDescription). */
@Entity
@Setter
@Getter
@Table(name = "GatePassType")
public class GatePassType implements Serializable {

    @Id
    @Column(name = "Id")
    private Integer id;

    @Column(name = "GpTypeDescription")
    private String gpTypeDescription;
}
