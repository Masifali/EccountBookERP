package com.mst.repositories;

import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import com.mst.models.InwardGatePass;

/**
 * dbo.GatePassInward — source procedures: Sp_GatePassInward_Insert, Sp_GatePassInward_Update, Sp_GatePassInward_GetAllMethod.
 * Named ...JpaRepository because the project already has a JdbcTemplate-based InwardGatePassRepository.
 *
 * Pre-insert logic of Sp_GatePassInward_Insert (procedural, kept as KEEP-EXEC in spGatePassInwardInsert; helpers below
 * expose the scalars it computes for callers that use save()):
 *  - EXEC usp_FinancialsYears_Validations; GpDate must not be after server date; BiltyDate must not be after GpDate.
 *  - BranchesId = 0 → BranchesId = UserAccount.BranchesId of @EntryUser.
 *  - GpSrNo = MAX(GpSrNo)+1 within Company/Organization/DocumentType/FinancialYear/Branches (maxGpSrNo()).
 *  - GpTypeSrNo = MAX(GpTypeSrNo)+1 within the same keys + GatepassType (maxGpTypeSrNo()).
 *  - RefDocumentEntryNo = MAX(RefDocumentEntryNo)+1 within Company/Organization/DocumentType/FinancialYear/RefDocumentType
 *    (maxRefDocumentEntryNo()).
 *  - RefDocumentTypeId = 41: UPDATE SupplierDispatch.VehicleNo (updateSupplierDispatchVehicleNo()); GatepassType taken from
 *    InvOrderCategory of the PurchaseOrder; order-date / PurchaseOrderId / SupplierContractCode validations.
 *  - RefDocumentTypeId = 175 requires PurchaseOrderId; Status must be 'Open', NoOfPackages/PackUnit/WeightComparedToPoWt
 *    required unless DocumentTypeId = 1601; PO tolerance check through dbo.GetPurchaseOrderandGatepassQtyBalance (configs 36/5070/89).
 *  - IsStockReserved = 1 when Status = 'Accept As Reserved'; UserLogId defaults to UserAccount.LastUserLogId of @EntryUser.
 *  - Id = ISNULL(MAX(CONVERT(int,Id)),0)+1 (maxId()); OutDateTimeStamp/ModifyDate/ModifyUser are NOT inserted; SELECT @Id returned;
 *    EXEC USP_UserAudit_Insert (ActionId 1).
 *
 * Sp_GatePassInward_Update: all validations (financial year, lock date, lab analysis, weigh-bridge tickets, freight voucher,
 * PO tolerance, WbTransactions party sync, freight voucher GL re-pointing for 'Accept As Reserved') are procedural → KEEP-EXEC
 * in spGatePassInwardUpdate. Its standalone single-table statements are exposed as updateById(), and
 * InwardGatePassDetailRepository.deleteByGatePassInwardId() / deletePurchaseBreakUpByInwardGatePassId().
 */
@Repository
public interface InwardGatePassJpaRepository extends JpaRepository<InwardGatePass, Integer> {

    // =====================================================================================================
    // Sp_GatePassInward_Insert helpers
    // =====================================================================================================

    /** Sp_GatePassInward_Insert: Id = ISNULL(MAX(CONVERT(int,Id)),0)+1 (table-wide). */
    @Query("select coalesce(max(e.id),0) from InwardGatePass e")
    Integer maxId();

    /** Sp_GatePassInward_Insert: GpSrNo = ISNULL(MAX(GpSrNo),0)+1 within Company/Organization/DocumentType/FinancialYear/Branches. */
    @Query("select coalesce(max(e.gpSrNo),0) from InwardGatePass e where e.companyId = :companyId and e.organizationId = :organizationId "
         + "and e.documentTypeId = :documentTypeId and e.financialYearId = :financialYearId and e.branchesId = :branchesId")
    Integer maxGpSrNo(@Param("companyId") Integer companyId, @Param("organizationId") Integer organizationId,
                      @Param("documentTypeId") Integer documentTypeId, @Param("financialYearId") Integer financialYearId,
                      @Param("branchesId") Integer branchesId);

    /** Sp_GatePassInward_Insert: GpTypeSrNo = ISNULL(MAX(GpTypeSrNo),0)+1 within Company/Organization/DocumentType/FinancialYear/GatepassType/Branches. */
    @Query("select coalesce(max(e.gpTypeSrNo),0) from InwardGatePass e where e.companyId = :companyId and e.organizationId = :organizationId "
         + "and e.documentTypeId = :documentTypeId and e.financialYearId = :financialYearId and e.gatepassType = :gatepassType "
         + "and e.branchesId = :branchesId")
    Integer maxGpTypeSrNo(@Param("companyId") Integer companyId, @Param("organizationId") Integer organizationId,
                          @Param("documentTypeId") Integer documentTypeId, @Param("financialYearId") Integer financialYearId,
                          @Param("gatepassType") String gatepassType, @Param("branchesId") Integer branchesId);

    /** Sp_GatePassInward_Insert: RefDocumentEntryNo = ISNULL(MAX(RefDocumentEntryNo),0)+1 within Company/Organization/DocumentType/FinancialYear/RefDocumentType. */
    @Query("select coalesce(max(e.refDocumentEntryNo),0) from InwardGatePass e where e.companyId = :companyId and e.organizationId = :organizationId "
         + "and e.documentTypeId = :documentTypeId and e.financialYearId = :financialYearId and e.refDocumentTypeId = :refDocumentTypeId")
    Integer maxRefDocumentEntryNo(@Param("companyId") Integer companyId, @Param("organizationId") Integer organizationId,
                                  @Param("documentTypeId") Integer documentTypeId, @Param("financialYearId") Integer financialYearId,
                                  @Param("refDocumentTypeId") Integer refDocumentTypeId);

    /**
     * Sp_GatePassInward_Insert / _Update (RefDocumentTypeId = 41 and SupplierDispatchId > 0):
     * "UPDATE SupplierDispatch SET VehicleNo = @VehicleNo WHERE Id = @SupplierDispatchId". Native: SupplierDispatch entity is
     * owned by batch D2 and does not exist yet.
     */
    @Modifying
    @Transactional
    @Query(value = "UPDATE SupplierDispatch SET VehicleNo = :vehicleNo WHERE Id = :supplierDispatchId", nativeQuery = true)
    int updateSupplierDispatchVehicleNo(@Param("vehicleNo") String vehicleNo, @Param("supplierDispatchId") Integer supplierDispatchId);

    /**
     * Sp_GatePassInward_Insert (whole procedure, all parameters; every parameter defaults to NULL in the proc except
     * @FinancialYearId which is mandatory). Returns the new Id via SELECT @Id → callers should use spGatePassInwardInsert
     * then read the inserted row (e.g. maxId()).
     * // KEEP-EXEC: multi-table procedural insert (financial-year validation proc, config-driven PO tolerance checks,
     * SupplierDispatch update, UserAccount lookups, USP_UserAudit_Insert, RAISERROR validations).
     */
    @Modifying
    @Transactional
    @Query(value = "EXEC dbo.Sp_GatePassInward_Insert @Id = :id, @DocumentTypeId = :documentTypeId, @GpSrNo = :gpSrNo, @GpDate = :gpDate, "
         + "@GatepassType = :gatepassType, @GpTypeSrNo = :gpTypeSrNo, @SupplierCustomerId = :supplierCustomerId, @OtherSupCust = :otherSupCust, "
         + "@VehicleType = :vehicleType, @VehicleNo = :vehicleNo, @BiltyNo = :biltyNo, @NoOfPackages = :noOfPackages, @OtherRemarks = :otherRemarks, "
         + "@RefDocumentTypeId = :refDocumentTypeId, @RefDocumentEntryNo = :refDocumentEntryNo, @InDateTimeStamp = :inDateTimeStamp, "
         + "@OutDateTimeStamp = :outDateTimeStamp, @DocAttachment = :docAttachment, @IsApproved = :isApproved, @EntryDate = :entryDate, "
         + "@EntryUser = :entryUser, @ModifyDate = :modifyDate, @ModifyUser = :modifyUser, @OrganizationId = :organizationId, @CompanyId = :companyId, "
         + "@PostUser = :postUser, @PostState = :postState, @PostDate = :postDate, @CityId = :cityId, @Freight = :freight, "
         + "@SupplierContractCode = :supplierContractCode, @Status = :status, @NetPaid = :netPaid, @SupplierWeight = :supplierWeight, "
         + "@FactoryWeight = :factoryWeight, @DifferenceWeight = :differenceWeight, @WeighBridgeId = :weighBridgeId, @WarehouseId = :warehouseId, "
         + "@VarietyName = :varietyName, @PurchaseOrderId = :purchaseOrderId, @Container = :container, @Container1 = :container1, "
         + "@PackUnit = :packUnit, @WeightComparedToPoWt = :weightComparedToPoWt, @FinancialYearId = :financialYearId, @GpQRCode = :gpQRCode, "
         + "@DestrictId = :destrictId, @TehsilId = :tehsilId, @SupplierCNICNO = :supplierCNICNO, @SupplierMobileNo = :supplierMobileNo, "
         + "@TransporterName = :transporterName, @DriverName = :driverName, @DriverCNICNO = :driverCNICNO, @DriverMobileNo = :driverMobileNo, "
         + "@WeightDiffComments = :weightDiffComments, @SystemErrorMessage = :systemErrorMessage, @FreightOn = :freightOn, "
         + "@GhallaMandiId = :ghallaMandiId, @LoadingContractorId = :loadingContractorId, @CarriageContractorId = :carriageContractorId, "
         + "@WorkingReportNo = :workingReportNo, @BiltyDate = :biltyDate, @BranchesId = :branchesId, @RateCut = :rateCut, "
         + "@RateCutRemarks = :rateCutRemarks, @AccessWeight = :accessWeight, @SupplierDispatchId = :supplierDispatchId, "
         + "@PackingTypeId = :packingTypeId, @AdvanceByFactory = :advanceByFactory, @AdvanceByParty = :advanceByParty, @ItemId = :itemId, "
         + "@ActionIdForSpecialApproval = :actionIdForSpecialApproval, @SpecialApprovalUserId = :specialApprovalUserId, "
         + "@SpecialApprovalDate = :specialApprovalDate, @SupplierFirstWeight = :supplierFirstWeight, @SupplierSecondWeight = :supplierSecondWeight, "
         + "@driverBioDataId = :driverBioDataId, @UserLogId = :userLogId", nativeQuery = true)
    void spGatePassInwardInsert(@Param("id") Integer id, @Param("documentTypeId") Integer documentTypeId, @Param("gpSrNo") Integer gpSrNo,
            @Param("gpDate") LocalDate gpDate, @Param("gatepassType") String gatepassType, @Param("gpTypeSrNo") Integer gpTypeSrNo,
            @Param("supplierCustomerId") Integer supplierCustomerId, @Param("otherSupCust") String otherSupCust,
            @Param("vehicleType") String vehicleType, @Param("vehicleNo") String vehicleNo, @Param("biltyNo") String biltyNo,
            @Param("noOfPackages") Integer noOfPackages, @Param("otherRemarks") String otherRemarks,
            @Param("refDocumentTypeId") Integer refDocumentTypeId, @Param("refDocumentEntryNo") Integer refDocumentEntryNo,
            @Param("inDateTimeStamp") LocalDateTime inDateTimeStamp, @Param("outDateTimeStamp") LocalDateTime outDateTimeStamp,
            @Param("docAttachment") String docAttachment, @Param("isApproved") Boolean isApproved,
            @Param("entryDate") LocalDateTime entryDate, @Param("entryUser") Integer entryUser,
            @Param("modifyDate") LocalDateTime modifyDate, @Param("modifyUser") Integer modifyUser,
            @Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId,
            @Param("postUser") Integer postUser, @Param("postState") Boolean postState, @Param("postDate") LocalDateTime postDate,
            @Param("cityId") Integer cityId, @Param("freight") Double freight, @Param("supplierContractCode") String supplierContractCode,
            @Param("status") String status, @Param("netPaid") Double netPaid, @Param("supplierWeight") Double supplierWeight,
            @Param("factoryWeight") Double factoryWeight, @Param("differenceWeight") Double differenceWeight,
            @Param("weighBridgeId") Integer weighBridgeId, @Param("warehouseId") Integer warehouseId,
            @Param("varietyName") String varietyName, @Param("purchaseOrderId") Integer purchaseOrderId,
            @Param("container") String container, @Param("container1") String container1, @Param("packUnit") Double packUnit,
            @Param("weightComparedToPoWt") Double weightComparedToPoWt, @Param("financialYearId") Integer financialYearId,
            @Param("gpQRCode") byte[] gpQRCode, @Param("destrictId") Integer destrictId, @Param("tehsilId") Integer tehsilId,
            @Param("supplierCNICNO") String supplierCNICNO, @Param("supplierMobileNo") String supplierMobileNo,
            @Param("transporterName") String transporterName, @Param("driverName") String driverName,
            @Param("driverCNICNO") String driverCNICNO, @Param("driverMobileNo") String driverMobileNo,
            @Param("weightDiffComments") String weightDiffComments, @Param("systemErrorMessage") String systemErrorMessage,
            @Param("freightOn") Integer freightOn, @Param("ghallaMandiId") Integer ghallaMandiId,
            @Param("loadingContractorId") Integer loadingContractorId, @Param("carriageContractorId") Integer carriageContractorId,
            @Param("workingReportNo") Integer workingReportNo, @Param("biltyDate") LocalDateTime biltyDate,
            @Param("branchesId") Integer branchesId, @Param("rateCut") Double rateCut, @Param("rateCutRemarks") String rateCutRemarks,
            @Param("accessWeight") Double accessWeight, @Param("supplierDispatchId") Integer supplierDispatchId,
            @Param("packingTypeId") Integer packingTypeId, @Param("advanceByFactory") Double advanceByFactory,
            @Param("advanceByParty") Double advanceByParty, @Param("itemId") Integer itemId,
            @Param("actionIdForSpecialApproval") Integer actionIdForSpecialApproval,
            @Param("specialApprovalUserId") Integer specialApprovalUserId, @Param("specialApprovalDate") LocalDateTime specialApprovalDate,
            @Param("supplierFirstWeight") Double supplierFirstWeight, @Param("supplierSecondWeight") Double supplierSecondWeight,
            @Param("driverBioDataId") Integer driverBioDataId, @Param("userLogId") Long userLogId);

    // =====================================================================================================
    // Sp_GatePassInward_Update
    // =====================================================================================================

    /**
     * Sp_GatePassInward_Update (whole procedure, all parameters; defaults NULL except mandatory @FinancialYearId).
     * // KEEP-EXEC: procedural validations + side effects (InvLabAnalysisPurchaseHeader re-link, WbTransactions party sync,
     * freight voucher GL re-pointing, UserAudit, detail/break-up deletes).
     */
    @Modifying
    @Transactional
    @Query(value = "EXEC dbo.Sp_GatePassInward_Update @Id = :id, @DocumentTypeId = :documentTypeId, @GpSrNo = :gpSrNo, @GpDate = :gpDate, "
         + "@GatepassType = :gatepassType, @GpTypeSrNo = :gpTypeSrNo, @SupplierCustomerId = :supplierCustomerId, @OtherSupCust = :otherSupCust, "
         + "@VehicleType = :vehicleType, @VehicleNo = :vehicleNo, @BiltyNo = :biltyNo, @NoOfPackages = :noOfPackages, @OtherRemarks = :otherRemarks, "
         + "@RefDocumentTypeId = :refDocumentTypeId, @RefDocumentEntryNo = :refDocumentEntryNo, @InDateTimeStamp = :inDateTimeStamp, "
         + "@OutDateTimeStamp = :outDateTimeStamp, @DocAttachment = :docAttachment, @IsApproved = :isApproved, @EntryDate = :entryDate, "
         + "@EntryUser = :entryUser, @ModifyDate = :modifyDate, @ModifyUser = :modifyUser, @OrganizationId = :organizationId, @CompanyId = :companyId, "
         + "@PostUser = :postUser, @PostState = :postState, @PostDate = :postDate, @CityId = :cityId, @Freight = :freight, "
         + "@SupplierContractCode = :supplierContractCode, @Status = :status, @NetPaid = :netPaid, @SupplierWeight = :supplierWeight, "
         + "@FactoryWeight = :factoryWeight, @DifferenceWeight = :differenceWeight, @WeighBridgeId = :weighBridgeId, @WarehouseId = :warehouseId, "
         + "@VarietyName = :varietyName, @PurchaseOrderId = :purchaseOrderId, @Container = :container, @Container1 = :container1, "
         + "@PackUnit = :packUnit, @WeightComparedToPoWt = :weightComparedToPoWt, @FinancialYearId = :financialYearId, @GpQRCode = :gpQRCode, "
         + "@DestrictId = :destrictId, @TehsilId = :tehsilId, @SupplierCNICNO = :supplierCNICNO, @SupplierMobileNo = :supplierMobileNo, "
         + "@TransporterName = :transporterName, @DriverName = :driverName, @DriverCNICNO = :driverCNICNO, @DriverMobileNo = :driverMobileNo, "
         + "@WeightDiffComments = :weightDiffComments, @SystemErrorMessage = :systemErrorMessage, @FreightOn = :freightOn, "
         + "@GhallaMandiId = :ghallaMandiId, @LoadingContractorId = :loadingContractorId, @CarriageContractorId = :carriageContractorId, "
         + "@WorkingReportNo = :workingReportNo, @BiltyDate = :biltyDate, @BranchesId = :branchesId, @RateCut = :rateCut, "
         + "@RateCutRemarks = :rateCutRemarks, @AccessWeight = :accessWeight, @SupplierDispatchId = :supplierDispatchId, "
         + "@PackingTypeId = :packingTypeId, @AdvanceByFactory = :advanceByFactory, @AdvanceByParty = :advanceByParty, @ItemId = :itemId, "
         + "@ActionIdForSpecialApproval = :actionIdForSpecialApproval, @SpecialApprovalUserId = :specialApprovalUserId, "
         + "@SpecialApprovalDate = :specialApprovalDate, @SupplierFirstWeight = :supplierFirstWeight, @SupplierSecondWeight = :supplierSecondWeight, "
         + "@driverBioDataId = :driverBioDataId, @UserLogId = :userLogId", nativeQuery = true)
    void spGatePassInwardUpdate(@Param("id") Integer id, @Param("documentTypeId") Integer documentTypeId, @Param("gpSrNo") Integer gpSrNo,
            @Param("gpDate") LocalDate gpDate, @Param("gatepassType") String gatepassType, @Param("gpTypeSrNo") Integer gpTypeSrNo,
            @Param("supplierCustomerId") Integer supplierCustomerId, @Param("otherSupCust") String otherSupCust,
            @Param("vehicleType") String vehicleType, @Param("vehicleNo") String vehicleNo, @Param("biltyNo") String biltyNo,
            @Param("noOfPackages") Integer noOfPackages, @Param("otherRemarks") String otherRemarks,
            @Param("refDocumentTypeId") Integer refDocumentTypeId, @Param("refDocumentEntryNo") Integer refDocumentEntryNo,
            @Param("inDateTimeStamp") LocalDateTime inDateTimeStamp, @Param("outDateTimeStamp") LocalDateTime outDateTimeStamp,
            @Param("docAttachment") String docAttachment, @Param("isApproved") Boolean isApproved,
            @Param("entryDate") LocalDateTime entryDate, @Param("entryUser") Integer entryUser,
            @Param("modifyDate") LocalDateTime modifyDate, @Param("modifyUser") Integer modifyUser,
            @Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId,
            @Param("postUser") Integer postUser, @Param("postState") Boolean postState, @Param("postDate") LocalDateTime postDate,
            @Param("cityId") Integer cityId, @Param("freight") Double freight, @Param("supplierContractCode") String supplierContractCode,
            @Param("status") String status, @Param("netPaid") Double netPaid, @Param("supplierWeight") Double supplierWeight,
            @Param("factoryWeight") Double factoryWeight, @Param("differenceWeight") Double differenceWeight,
            @Param("weighBridgeId") Integer weighBridgeId, @Param("warehouseId") Integer warehouseId,
            @Param("varietyName") String varietyName, @Param("purchaseOrderId") Integer purchaseOrderId,
            @Param("container") String container, @Param("container1") String container1, @Param("packUnit") Double packUnit,
            @Param("weightComparedToPoWt") Double weightComparedToPoWt, @Param("financialYearId") Integer financialYearId,
            @Param("gpQRCode") byte[] gpQRCode, @Param("destrictId") Integer destrictId, @Param("tehsilId") Integer tehsilId,
            @Param("supplierCNICNO") String supplierCNICNO, @Param("supplierMobileNo") String supplierMobileNo,
            @Param("transporterName") String transporterName, @Param("driverName") String driverName,
            @Param("driverCNICNO") String driverCNICNO, @Param("driverMobileNo") String driverMobileNo,
            @Param("weightDiffComments") String weightDiffComments, @Param("systemErrorMessage") String systemErrorMessage,
            @Param("freightOn") Integer freightOn, @Param("ghallaMandiId") Integer ghallaMandiId,
            @Param("loadingContractorId") Integer loadingContractorId, @Param("carriageContractorId") Integer carriageContractorId,
            @Param("workingReportNo") Integer workingReportNo, @Param("biltyDate") LocalDateTime biltyDate,
            @Param("branchesId") Integer branchesId, @Param("rateCut") Double rateCut, @Param("rateCutRemarks") String rateCutRemarks,
            @Param("accessWeight") Double accessWeight, @Param("supplierDispatchId") Integer supplierDispatchId,
            @Param("packingTypeId") Integer packingTypeId, @Param("advanceByFactory") Double advanceByFactory,
            @Param("advanceByParty") Double advanceByParty, @Param("itemId") Integer itemId,
            @Param("actionIdForSpecialApproval") Integer actionIdForSpecialApproval,
            @Param("specialApprovalUserId") Integer specialApprovalUserId, @Param("specialApprovalDate") LocalDateTime specialApprovalDate,
            @Param("supplierFirstWeight") Double supplierFirstWeight, @Param("supplierSecondWeight") Double supplierSecondWeight,
            @Param("driverBioDataId") Integer driverBioDataId, @Param("userLogId") Long userLogId);

    /**
     * Sp_GatePassInward_Update: the final "UPDATE [dbo].[GatePassInward] SET ... WHERE [Id] = @Id" statement, every SET column kept
     * (GpSrNo, RefDocumentEntryNo, InDateTimeStamp, EntryDate, EntryUser, RateCut, RateCutRemarks are NOT updated by the proc).
     * Native (not JPQL) because DocAttachment is @Transient on the existing entity. Callers must apply the proc's pre-update
     * adjustments themselves (Status='Open' → FactoryWeight=0, DifferenceWeight=0; NetPaid overridden by paid freight; etc.).
     */
    @Modifying
    @Transactional
    @Query(value = "UPDATE [dbo].[GatePassInward] SET [DocumentTypeId] = :documentTypeId, [GpDate] = :gpDate, [GatepassType] = :gatepassType, "
         + "[GpTypeSrNo] = :gpTypeSrNo, [SupplierCustomerId] = :supplierCustomerId, [OtherSupCust] = :otherSupCust, [VehicleType] = :vehicleType, "
         + "[VehicleNo] = :vehicleNo, [BiltyNo] = :biltyNo, [NoOfPackages] = :noOfPackages, [OtherRemarks] = :otherRemarks, "
         + "[RefDocumentTypeId] = :refDocumentTypeId, [OutDateTimeStamp] = :outDateTimeStamp, [DocAttachment] = :docAttachment, "
         + "[IsApproved] = :isApproved, [ModifyDate] = :modifyDate, [ModifyUser] = :modifyUser, [OrganizationId] = :organizationId, "
         + "[CompanyId] = :companyId, [PostUser] = :postUser, [PostState] = :postState, [PostDate] = :postDate, [CityId] = :cityId, "
         + "[Freight] = :freight, [SupplierContractCode] = :supplierContractCode, [Status] = :status, [NetPaid] = :netPaid, "
         + "[SupplierWeight] = :supplierWeight, [FactoryWeight] = :factoryWeight, [DifferenceWeight] = :differenceWeight, "
         + "[WeighBridgeId] = :weighBridgeId, [WarehouseId] = :warehouseId, [VarietyName] = :varietyName, Container = :container, "
         + "Container1 = :container1, PackUnit = :packUnit, WeightComparedToPoWt = :weightComparedToPoWt, FinancialYearId = :financialYearId, "
         + "GpQRCode = :gpQRCode, DestrictId = :destrictId, TehsilId = :tehsilId, SupplierCNICNO = :supplierCNICNO, "
         + "SupplierMobileNo = :supplierMobileNo, TransporterName = :transporterName, DriverName = :driverName, DriverCNICNO = :driverCNICNO, "
         + "DriverMobileNo = :driverMobileNo, WeightDiffComments = :weightDiffComments, SystemErrorMessage = :systemErrorMessage, "
         + "FreightOn = :freightOn, GhallaMandiId = :ghallaMandiId, LoadingContractorId = :loadingContractorId, "
         + "CarriageContractorId = :carriageContractorId, WorkingReportNo = :workingReportNo, BiltyDate = :biltyDate, BranchesId = :branchesId, "
         + "SupplierDispatchId = :supplierDispatchId, PurchaseOrderId = :purchaseOrderId, PackingTypeId = :packingTypeId, "
         + "AdvanceByFactory = :advanceByFactory, AdvanceByParty = :advanceByParty, ItemId = :itemId, "
         + "ActionIdForSpecialApproval = :actionIdForSpecialApproval, AccessWeight = :accessWeight, SupplierFirstWeight = :supplierFirstWeight, "
         + "SupplierSecondWeight = :supplierSecondWeight, IsStockReserved = :isStockReserved, driverBioDataId = :driverBioDataId, "
         + "UserLogId = :userLogId WHERE [Id] = :id", nativeQuery = true)
    int updateById(@Param("id") Integer id, @Param("documentTypeId") Integer documentTypeId, @Param("gpDate") LocalDate gpDate,
            @Param("gatepassType") String gatepassType, @Param("gpTypeSrNo") Integer gpTypeSrNo,
            @Param("supplierCustomerId") Integer supplierCustomerId, @Param("otherSupCust") String otherSupCust,
            @Param("vehicleType") String vehicleType, @Param("vehicleNo") String vehicleNo, @Param("biltyNo") String biltyNo,
            @Param("noOfPackages") Integer noOfPackages, @Param("otherRemarks") String otherRemarks,
            @Param("refDocumentTypeId") Integer refDocumentTypeId, @Param("outDateTimeStamp") LocalDateTime outDateTimeStamp,
            @Param("docAttachment") String docAttachment, @Param("isApproved") Boolean isApproved,
            @Param("modifyDate") LocalDateTime modifyDate, @Param("modifyUser") Integer modifyUser,
            @Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId,
            @Param("postUser") Integer postUser, @Param("postState") Boolean postState, @Param("postDate") LocalDateTime postDate,
            @Param("cityId") Integer cityId, @Param("freight") Double freight, @Param("supplierContractCode") String supplierContractCode,
            @Param("status") String status, @Param("netPaid") Double netPaid, @Param("supplierWeight") Double supplierWeight,
            @Param("factoryWeight") Double factoryWeight, @Param("differenceWeight") Double differenceWeight,
            @Param("weighBridgeId") Integer weighBridgeId, @Param("warehouseId") Integer warehouseId,
            @Param("varietyName") String varietyName, @Param("container") String container, @Param("container1") String container1,
            @Param("packUnit") Double packUnit, @Param("weightComparedToPoWt") Double weightComparedToPoWt,
            @Param("financialYearId") Integer financialYearId, @Param("gpQRCode") byte[] gpQRCode,
            @Param("destrictId") Integer destrictId, @Param("tehsilId") Integer tehsilId,
            @Param("supplierCNICNO") String supplierCNICNO, @Param("supplierMobileNo") String supplierMobileNo,
            @Param("transporterName") String transporterName, @Param("driverName") String driverName,
            @Param("driverCNICNO") String driverCNICNO, @Param("driverMobileNo") String driverMobileNo,
            @Param("weightDiffComments") String weightDiffComments, @Param("systemErrorMessage") String systemErrorMessage,
            @Param("freightOn") Integer freightOn, @Param("ghallaMandiId") Integer ghallaMandiId,
            @Param("loadingContractorId") Integer loadingContractorId, @Param("carriageContractorId") Integer carriageContractorId,
            @Param("workingReportNo") Integer workingReportNo, @Param("biltyDate") LocalDateTime biltyDate,
            @Param("branchesId") Integer branchesId, @Param("supplierDispatchId") Integer supplierDispatchId,
            @Param("purchaseOrderId") Integer purchaseOrderId, @Param("packingTypeId") Integer packingTypeId,
            @Param("advanceByFactory") Double advanceByFactory, @Param("advanceByParty") Double advanceByParty,
            @Param("itemId") Integer itemId, @Param("actionIdForSpecialApproval") Integer actionIdForSpecialApproval,
            @Param("accessWeight") Double accessWeight, @Param("supplierFirstWeight") Double supplierFirstWeight,
            @Param("supplierSecondWeight") Double supplierSecondWeight, @Param("isStockReserved") Boolean isStockReserved,
            @Param("driverBioDataId") Integer driverBioDataId, @Param("userLogId") Long userLogId);

    /** Sp_GatePassInward_Update: "delete GatePassInwardPurchaseBreakUp where InwardGatePassId=@Id" (standalone statement). */
    @Modifying
    @Transactional
    @Query("DELETE FROM InwardGatePassPurchaseBreakUp b WHERE b.inwardGatePassId = :inwardGatePassId")
    int deletePurchaseBreakUpByInwardGatePassId(@Param("inwardGatePassId") Integer inwardGatePassId);
}
