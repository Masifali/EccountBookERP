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

/** Mapped 1:1 onto dbo.ItemTypeMultiLingo (source: Sp_ItemTypeMultiLingo_Insert, usp_ItemTypeMultiLanguage_History). Insert uses SCOPE_IDENTITY() -> IDENTITY. */
@Entity
@Setter
@Getter
@Table(name = "ItemTypeMultiLingo")
public class ItemTypeMultiLingo implements Serializable {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "Id")
    private Integer id;

    @Column(name = "ItemTypeId")
    private Integer itemTypeId;

    @Column(name = "MultiLanguagesId")
    private Integer multiLanguagesId;

    @Column(name = "TypeDescription")
    private String typeDescription;
}
