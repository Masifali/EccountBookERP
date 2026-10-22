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

/** Mapped 1:1 onto dbo.MultiLanguages (source: Sp_MultiLanguages_GetAll, *_MultiLanguage_History procs). RTL inferred as bit flag. */
@Entity
@Setter
@Getter
@Table(name = "MultiLanguages")
public class MultiLanguages implements Serializable {

    @Id
    // app-assigned (proc computes max(Id)+1 or the table has no insert proc in this batch)
    @Column(name = "Id")
    private Integer id;

    @Column(name = "LanguageName")
    private String languageName;

    @Column(name = "LanguageDescription")
    private String languageDescription;

    @Column(name = "FlagImage")
    private String flagImage;

    @Column(name = "RTL")
    private Boolean rtl;
}
