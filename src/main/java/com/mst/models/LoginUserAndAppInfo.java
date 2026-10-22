package com.mst.models;

import java.io.Serializable;
import java.time.LocalDateTime;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;
import javax.persistence.Table;

import lombok.Getter;
import lombok.Setter;

/**
 * Mapped 1:1 onto dbo.LoginUserAndAppInfo (source: USP_GetUserRightsForViewbyUserId, which upserts one row
 * per (CompanyId, UserId) with the AppId the user last logged in from). Id is app-assigned (ISNULL(MAX(Id),0)+1).
 */
@Entity
@Setter
@Getter
@Table(name = "LoginUserAndAppInfo")
public class LoginUserAndAppInfo implements Serializable {

    @Id
    @Column(name = "Id")
    private Integer id;

    @Column(name = "UserId")
    private Integer userId;

    @Column(name = "AppId")
    private Integer appId;

    @Column(name = "CompanyId")
    private Integer companyId;

    @Column(name = "EntryDate")
    private LocalDateTime entryDate;
}
