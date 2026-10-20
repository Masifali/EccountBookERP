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

/** Mapped 1:1 onto dbo.ItemGroup_UOMSchedule (source: Sp_ItemGroup_UOMSchedule_Insert / _Update / _GetAllMethod, Sp_UOMSchedule_GetAllMethod). Id app-assigned (max(Id)+1). ItemGroupName/GroupId come from registry.json only. */
@Entity
@Setter
@Getter
@Table(name = "ItemGroup_UOMSchedule")
public class ItemGroup_UOMSchedule implements Serializable {

    @Id
    // app-assigned (proc computes max(Id)+1 or the table has no insert proc in this batch)
    @Column(name = "Id")
    private Integer id;

    @Column(name = "ItemGroupId")
    private Integer itemGroupId;

    @Column(name = "UOMId")
    private Integer uomId;

    @Column(name = "Equivalent")
    private String equivalent;

    @Column(name = "OrganizationId")
    private Integer organizationId;

    @Column(name = "CompanyId")
    private Integer companyId;

    @Column(name = "ItemGroupName")
    private String itemGroupName;

    @Column(name = "GroupId")
    private Integer groupId;
}
