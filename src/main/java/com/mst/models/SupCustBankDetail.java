package com.mst.models;

import java.time.LocalDateTime;
import javax.persistence.*;
import lombok.Data;

@Entity
@Table(name = "SupCustBankDetail")
@Data
public class SupCustBankDetail {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "Id")
    private Integer id;

    @Column(name = "SupCustId")
    private Integer supCustId;

    @Column(name = "BankId")
    private Integer bankId;

    @Column(name = "AcTitle", length = 200)
    private String acTitle;

    @Column(name = "AcNumber", length = 50)
    private String acNumber;

    @Column(name = "BranchCode", length = 50)
    private String branchCode;

    @Column(name = "CityId")
    private Integer cityId;

    @Column(name = "CountryId")
    private Integer countryId;

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

    @Column(name = "BranchesId")
    private Integer branchesId;

    @Column(name = "ProjectsId")
    private Integer projectsId;
}
