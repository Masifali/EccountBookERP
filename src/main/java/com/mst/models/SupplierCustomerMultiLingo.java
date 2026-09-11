package com.mst.models;

import javax.persistence.*;
import lombok.Data;

@Entity
@Table(name = "SupplierCustomerMultiLingo")
@Data
public class SupplierCustomerMultiLingo {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "Id")
    private Integer id;

    @Column(name = "SupplierCustomerId", nullable = false)
    private Integer supplierCustomerId;

    @Column(name = "MultiLanguagesId", nullable = false)
    private Integer multiLanguagesId;

    @Column(name = "PartyName", columnDefinition = "NVARCHAR(MAX)", nullable = false)
    private String partyName;
}
