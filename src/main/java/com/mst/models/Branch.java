package com.mst.models;

import javax.persistence.*;
import lombok.Data;

@Entity
@Table(name = "Branches")
@Data
public class Branch {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "Id")
    private Integer id;

    @Column(name = "BranchName", nullable = false, length = 150)
    private String branchName;

    @Column(name = "BranchCode", length = 50)
    private String branchCode;

    @Column(name = "BranchAddress", length = 300)
    private String branchAddress;

    @Column(name = "PhoneNo", length = 50)
    private String phoneNo;

    @Column(name = "CompanyId")
    private Integer companyId;

    @Column(name = "IsActive")
    private Boolean isActive = true;
}
