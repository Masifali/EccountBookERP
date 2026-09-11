package com.mst.models;

import java.time.LocalDateTime;
import javax.persistence.*;
import lombok.Data;

@Entity
@Table(name = "SupplierCustomerShipToAddress")
@Data
public class SupplierCustomerShipToAddress {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "Id")
    private Integer id;

    @Column(name = "SupplierCustomerId")
    private Integer supplierCustomerId;

    @Column(name = "AddressTitle", length = 100)
    private String addressTitle;

    @Column(name = "ContactPerson", length = 100)
    private String contactPerson;

    @Column(name = "PhoneNo", length = 50)
    private String phoneNo;

    @Column(name = "MobileNo", length = 50)
    private String mobileNo;

    @Column(name = "WhatsAppNo", length = 50)
    private String whatsAppNo;

    @Column(name = "CityId")
    private Integer cityId;

    @Column(name = "CountryId")
    private Integer countryId;

    @Column(name = "AddressLine1", length = 550)
    private String addressLine1;

    @Column(name = "AddressLine2", length = 100)
    private String addressLine2;

    @Column(name = "AddressLine3", length = 100)
    private String addressLine3;

    @Column(name = "AddressLine4", length = 100)
    private String addressLine4;

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
