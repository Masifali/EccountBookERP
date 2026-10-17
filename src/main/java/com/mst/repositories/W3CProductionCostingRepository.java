package com.mst.repositories;

import com.mst.repositories.support.DesktopProc;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.sql.Date;
import java.util.List;
import java.util.Map;

/**
 * 887 "Production Costing" - Architecture.WinApp.Production/frmCreateProductionCostingReport.cs
 * (DocumentTypeId 921) and its History tab, Architecture.WinApp.Production_Reports/frmProductionCostingReport.cs.
 *
 * Every read and write the form performs, traced form line -> BLL -> procedure. BLL guards are reproduced
 * literally (omitted, never sent as NULL/0); a null parameter is left out by {@link DesktopProc}. Every name
 * sent is declared by its procedure (checked against procdure.utf8.sql).
 */
@Repository
public class W3CProductionCostingRepository {

    /** base.Name - the rights screen name (SetRightsValueInRightsObject(ScreenName)). */
    public static final String SCREEN = "frmCreateProductionCostingReport";
    /** frmCreateProductionCostingReport ctor: DocumentTypeId = 921. */
    public static final int DOC_TYPE = 921;

    private static final String GET_ALL = "USP_ProductionManualCosting_GetAllMethod";

    @Autowired private JdbcTemplate jdbc;

    /* ============================================================================ pickers */

    /** InitializeComponentMethod:~293 -> InvFoodProduction.FoodProduction_JobOrderWithFinishOutPutPrcntObtained. */
    public List<Map<String, Object>> jobOrders(int org, int comp) {
        return DesktopProc.rows(jdbc, "USP_InvFoodProduction_JobOrderWithFinishOutPutPrcntObtained",
                DesktopProc.params("OrganizationId", org, "CompanyId", comp));
    }

    /** InitializeComponentMethod:~294 -> ExImInvoice.ExImInvoice_GetInvoicesData -> UPS_ExImInvoice_GetInvoicesData. */
    public List<Map<String, Object>> invoices(int org, int comp, int branch, int fy) {
        return DesktopProc.rows(jdbc, "UPS_ExImInvoice_GetInvoicesData", DesktopProc.params(
                "OrganizationId", org, "CompanyId", comp, "BranchesId", branch, "FinancialYearId", fy));
    }

    /** InputBindByJobOrder:617 -> FoodProduction_InputSummaryByJobOrder. */
    public List<Map<String, Object>> inputSummary(int org, int comp, int branch, int fy, int jobOrderId) {
        return DesktopProc.rows(jdbc, "USP_InvFoodProduction_InputSummaryByJobOrder", DesktopProc.params(
                "OrganizationId", org, "CompanyId", comp, "BranchesId", branch, "FinancialYearId", fy, "JobOrderId", jobOrderId));
    }

    /** ByProductBindByJobOrder:704 -> FoodProduction_ByProductSummaryByJobOrder. */
    public List<Map<String, Object>> byProductSummary(int org, int comp, int branch, int fy, int jobOrderId) {
        return DesktopProc.rows(jdbc, "USP_InvFoodProduction_ByProductSummaryByJobOrder", DesktopProc.params(
                "OrganizationId", org, "CompanyId", comp, "BranchesId", branch, "FinancialYearId", fy, "JobOrderId", jobOrderId));
    }

    /**
     * CmbInvoiceNoDetail_Leave:~490 -> ProductionManualCosting_GetDataByJobAndInvoiceId
     * -> [dbo].[USP_ProductionManualCosting_GetDataByJobAndInvoiceId]: all seven parameters, always sent.
     */
    public List<Map<String, Object>> dataByJobAndInvoice(int org, int comp, int branch, int fy, int jobOrderId, int invoiceId) {
        return DesktopProc.rows(jdbc, "USP_ProductionManualCosting_GetDataByJobAndInvoiceId", DesktopProc.params(
                "OrganizationId", org, "CompanyId", comp, "DocumentTypeId", DOC_TYPE, "FinancialYearId", fy,
                "BranchesId", branch, "JobOrderId", jobOrderId, "InvoiceId", invoiceId));
    }

    /* ================================================================================ read */

    /** ReadById:1172 -> ProductionManualCosting.ReadByJobOrderId: @JobOrderId, @Activity='ReadByJobOrderId'. */
    public List<Map<String, Object>> headerByJobOrder(int jobOrderId) {
        return DesktopProc.rows(jdbc, GET_ALL, DesktopProc.params("JobOrderId", jobOrderId, "Activity", "ReadByJobOrderId"));
    }

    /** ProductionManualCosting.GetByID: @Id, @Activity='ReadById' (used to fence an Id before an update). */
    public List<Map<String, Object>> headerById(int id) {
        return DesktopProc.rows(jdbc, GET_ALL, DesktopProc.params("Id", id, "Activity", "ReadById"));
    }

    /** DAL GetData:3 -> 'ProductionManualCostingInputDetail_ReadHeaderById' (the last assignment of the input list). */
    public List<Map<String, Object>> inputDetails(int id) {
        return DesktopProc.rows(jdbc, GET_ALL, DesktopProc.params("Id", id, "Activity", "ProductionManualCostingInputDetail_ReadHeaderById"));
    }

    /** DAL GetData:4 -> 'ProductionManualCostingByProductDetail_ReadHeaderById'. */
    public List<Map<String, Object>> byProductDetails(int id) {
        return DesktopProc.rows(jdbc, GET_ALL, DesktopProc.params("Id", id, "Activity", "ProductionManualCostingByProductDetail_ReadHeaderById"));
    }

    /** DAL GetData:5 -> 'ProductionManualCostingInvoiceDetail_ReadHeaderById'. */
    public List<Map<String, Object>> invoiceDetails(int id) {
        return DesktopProc.rows(jdbc, GET_ALL, DesktopProc.params("Id", id, "Activity", "ProductionManualCostingInvoiceDetail_ReadHeaderById"));
    }

    /** OpenPrint:1538 -> ProductionManualCosting_SlipAndRegister: @OrganizationId, @CompanyId, @JobOrderId, @DocumentTypeId. */
    public List<Map<String, Object>> slipRows(int org, int comp, int jobOrderId) {
        return DesktopProc.rows(jdbc, "USP_ProductionManualCosting_SlipAndRegister", DesktopProc.params(
                "OrganizationId", org, "CompanyId", comp, "JobOrderId", jobOrderId > 0 ? jobOrderId : null, "DocumentTypeId", DOC_TYPE));
    }

    /* ============================================================================ history */

    /** frmProductionCostingReport.gridHisory:~150 -> ProductionCosting_FormHistoryReport -> usp_ProductionCosting_FormHistory. */
    public List<Map<String, Object>> history(int comp, Date from, Date to) {
        return DesktopProc.rows(jdbc, "usp_ProductionCosting_FormHistory", DesktopProc.params(
                "CompanyId", comp, "FromDate", from, "ToDate", to));
    }

    /* ======================================================== By-Product item wise pop-up */

    /** BtnByProductSettlementPopUp_Click:1834 -> InvFoodProduction.getProductionByProductDataByJobOrderId. */
    public List<Map<String, Object>> byProductItemWise(int comp, int jobOrderId) {
        return DesktopProc.rows(jdbc, "usp_getProductionByProductDataByJobOrderId",
                DesktopProc.params("CompanyId", comp, "JobOrderId", jobOrderId));
    }

    /** InvFoodProduction.ProductionByProductRateUpdateByJobOrderId: one call per row, @RatePerKg float. */
    public void byProductRateUpdate(int comp, int jobOrderId, int itemId, double ratePerKg) {
        DesktopProc.rows(jdbc, "usp_ProductionByProductRateUpdateByJobOrderId", DesktopProc.params(
                "CompanyId", comp, "JobOrderId", jobOrderId, "ItemId", itemId, "RatePerKg", ratePerKg));
    }

    /* ================================================================================ write */

    /** DAL SetData:~60 - the header: [dbo].[USP_ProductionManualCosting_InsertAndUpdate] (scalar Id). */
    public int saveHeader(Map<String, Object> p) {
        return DesktopProc.setProc(jdbc, "USP_ProductionManualCosting_InsertAndUpdate", p);
    }

    public int saveInputDetail(Map<String, Object> p) {
        return DesktopProc.setProc(jdbc, "USP_ProductionManualCostingInputDetail_Insert", p);
    }

    public int saveByProductDetail(Map<String, Object> p) {
        return DesktopProc.setProc(jdbc, "USP_ProductionManualCostingByProductDetail_Insert", p);
    }

    public int saveInvoiceDetail(Map<String, Object> p) {
        return DesktopProc.setProc(jdbc, "USP_ProductionManualCostingInvoiceDetail_Insert", p);
    }
}
