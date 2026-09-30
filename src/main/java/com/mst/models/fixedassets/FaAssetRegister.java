package com.mst.models.fixedassets;

import com.mst.models.hrm.DesktopModel;

import java.time.LocalDateTime;

/**
 * Architecture.Model.AssetSchema.AssetRegister (model 0738) - the NON-virtual properties, in declaration
 * order, spelled as the C# properties. GenericProvider.SetProc sends one @Parameter per field to
 * [Asset].[USP_AssetRegister_InsertAndUpdate] (44 parameters; @UomId is not a model property and is never
 * sent, so the procedure's default NULL applies - also on update, where it overwrites UomId).
 *
 * depreciationMethodScheduleId and depriciationrate are VIRTUAL on the model, so the desktop never sends
 * them (the form fills them, SetProc skips them). They are therefore not fields here.
 */
public class FaAssetRegister extends DesktopModel {
    public boolean isApproved;
    public boolean isDepreciable;
    public LocalDateTime approvedDate;
    public LocalDateTime entryDate;
    public LocalDateTime expiryDate;
    public LocalDateTime LastModifyDate;
    public LocalDateTime purchaseDate;
    public double currentValue;
    public double purchasePrice;
    public int accumulateddepriciationAcId;
    public int approvedUserId;
    public int branchesId;
    public int brandId;
    public int capitalWIPAccountId;
    public int ExpenseMaintenanceAccountId;
    public int companyId;
    public int departmentId;
    public int depriciationExpenseAcId;
    public int entryUserId;
    public int glAccountId;
    public int LastModifyById;
    public int organizationId;
    public int projectsId;
    public int useableLifeMonth;
    public long assetRegisterId;
    public long categoryId;
    public long itemId;
    public long manufacturerId;
    public long vendorId;
    public String assetCode;
    public String assetName;
    public String assetStatus;
    public String assetsTypeId;
    public String brandName;
    public int conditionId;
    public String locationBranch;
    public String makeDesc;
    public String manufacturerName;
    public String modelDesc;
    public String pic1Path;
    public String pic2Path;
    public String serialNo;
    public String vendorName;
}
