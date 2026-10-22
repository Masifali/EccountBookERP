package com.mst.models;

import java.io.Serializable;
import java.time.LocalDateTime;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;
import javax.persistence.Table;

import lombok.Getter;
import lombok.Setter;

/** Mapped 1:1 onto dbo.TaxesTypes (source: Sp_TaxesTypes_GetAllMethod). No insert proc in this batch - Id left app-assigned. */
@Entity
@Setter
@Getter
@Table(name = "TaxesTypes")
public class TaxesTypes implements Serializable {

    @Id
    @Column(name = "Id")
    private Integer id;

    @Column(name = "TaxName")
    private String taxName;

    @Column(name = "TaxDescription")
    private String taxDescription;

    @Column(name = "TaxGLAccountId")
    private Integer taxGLAccountId;

    /** int on the procedure (@Type int). */
    @Column(name = "Type")
    private Integer type;

    @Column(name = "EntryDate")
    private LocalDateTime entryDate;

    @Column(name = "EntryUser")
    private Integer entryUser;

    @Column(name = "ModifyDate")
    private LocalDateTime modifyDate;

    @Column(name = "ModifyUser")
    private Integer modifyUser;

    @Column(name = "OrganizationId")
    private Integer organizationId;

    @Column(name = "CompanyId")
    private Integer companyId;
}
