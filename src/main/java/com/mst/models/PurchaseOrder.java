package com.mst.models;

import javax.persistence.*;
import lombok.Data;

import java.util.Date;
import java.util.List;
import java.util.ArrayList;

@Entity
@Table(name = "PurchaseOrder")
@Data
public class PurchaseOrder {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "Id")
    private Integer id;

    @Column(name = "DocumentTypeId")
    private Integer documentTypeId;

    @Column(name = "DocNo")
    private Integer docNo;

    @Column(name = "DocDate")
    @Temporal(TemporalType.DATE)
    private Date docDate;

    @Column(name = "FinancialYearId")
    private Integer financialYearId;

    @Column(name = "SupplierCustomerId")
    private Integer supplierCustomerId;

    @Column(name = "OrganizationId")
    private Integer organizationId;

    @Column(name = "CompanyId")
    private Integer companyId;

    @Column(name = "BranchesId")
    private Integer branchesId;

    @Column(name = "IsApproved")
    private Boolean isApproved = false;

    @Column(name = "RemarksHeader")
    private String remarksHeader;

    @Column(name = "EntryDate")
    @Temporal(TemporalType.TIMESTAMP)
    private Date entryDate;

    @Column(name = "EntryUser")
    private Integer entryUser;

    @OneToMany(mappedBy = "purchaseOrderId", cascade = CascadeType.ALL, fetch = FetchType.LAZY)
    private List<PurchaseOrderDetail> details = new ArrayList<>();
}
