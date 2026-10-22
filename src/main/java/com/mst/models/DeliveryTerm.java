package com.mst.models;

import java.io.Serializable;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;
import javax.persistence.Table;

import lombok.Getter;
import lombok.Setter;

/** Mapped 1:1 onto dbo.DeliveryTerm (source: USP_DeliveryTerm_GetAllMethod). Read-only lookup; Id app-assigned. */
@Entity
@Setter
@Getter
@Table(name = "DeliveryTerm")
public class DeliveryTerm implements Serializable {

    @Id
    @Column(name = "Id")
    private Integer id;

    @Column(name = "Description")
    private String description;

    @Column(name = "ValueDescription")
    private String valueDescription;
}
