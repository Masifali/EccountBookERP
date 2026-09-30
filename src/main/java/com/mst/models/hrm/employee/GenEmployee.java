package com.mst.models.hrm.employee;

import com.mst.models.hrm.DesktopModel;

import java.time.LocalDateTime;

/**
 * Architecture.Model.HRM.EmployeeManagement.genEmployee (model 0707) - every non-virtual property, as
 * GenericProvider.SetProc sends it to Sp_genEmployee_Insert / Sp_genEmployee_Update (51 parameters,
 * checked against procdure.utf8.sql: every field below is declared by both procedures).
 * The virtual lookup names (EmployeeType, Gender ...) and the ten child lists are not parameters and
 * are not declared here; the children are separate models saved by the DAL in the same transaction.
 */
public class GenEmployee extends DesktopModel {
    public int EmployeeId;
    public int OrganizationId;
    public int CompanyId;
    public int ControlTypeProfileId;
    public int PersonId;
    public int PartyLocationId;
    public int BillCategoryProfileId;
    public int EmployeeTypeProfileId;
    public int EmployeeCategoryId;
    public int ConsultantTypeProfileId;
    public int EmployeeHistoryId;
    public int AddressId;
    public String AddressDetail;
    public int TitleId;
    public String EmployeeNo;
    public String FistName;
    public String MiddleName;
    public String LastName;
    public String CNIC;
    public LocalDateTime DOB;
    public LocalDateTime CNICIssueDate;
    public LocalDateTime CNICExpiryDate;
    public int GenderProfileId;
    public int NationalityProfileId;
    public int BloodGroupProfileId;
    public boolean NeedLogin;
    public String PictureFilePath;
    public String SignPictureFilePath;
    public String SignPictureString;
    public String EmployeePictureString;
    public String Signature;
    public String Mobile1;
    public String Mobile2;
    public String Email;
    public String LicenseNo;
    public String PassportNo;
    public int SubTitleId;
    public String RelationName;
    public String RelationMobile;
    public boolean Active;
    public boolean IsApproved;
    public int CreatedById;
    public LocalDateTime CreatedOn;
    public int AlteredById;
    public LocalDateTime AlteredOn;
    public int ActionTypeId;
    public int UserLogId;
    public boolean IsAllowShare;
    public double FlatSharePercent;
    public int BranchesId;
    public int ProjectId;
}
