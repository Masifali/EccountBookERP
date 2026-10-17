package com.mst.repositories;

import com.mst.repositories.support.DesktopProc;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.sql.Date;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * 906 "Production FOH" - Architecture.WinApp.Production.Regular/frmProductionFohReportCreate.cs
 * (DocumentTypeId 922, tabs "FOH Allocation" and "FOH Expense"). Every procedure the form reaches through
 * its BLL calls (DailyPlantConsumedHours, ProdcutionFohAllocateToJobOrder), parameters exactly as the BLL
 * sends them (a guarded parameter is left out, never sent as NULL / 0). Names checked against procdure.utf8.sql.
 */
@Repository
public class W3CProductionFohRepository {

    /** The rights screen name (ctor ScreenName; at run time Conversion.ToString(base.Tag)). */
    public static final String SCREEN = "frmProductionFohReportCreate";
    /** ctor: DocumentTypeId = 922. */
    public static final int DOC_TYPE = 922;

    private static final String GET_ALL = "USP_ProdcutionFohAllocateToJobOrder_GetAllMethod";

    @Autowired private JdbcTemplate jdbc;

    /**
     * BindGrid:339 / FillForFohExpenseData:671 -> DailyPlantConsumedHours.FormHistoryAndReport(OrganizationId, CompanyId,
     * MonthDate) -> [dbo].[USP_DailyPlantConsumedHours_FormHistoryReport]. The procedure keeps only the month of @MonthDate.
     */
    public List<Map<String, Object>> monthlyHours(int org, int comp, Date monthDate) {
        return DesktopProc.rows(jdbc, "USP_DailyPlantConsumedHours_FormHistoryReport", DesktopProc.params(
                "OrganizationId", org, "CompanyId", comp, "MonthDate", monthDate));
    }

    /** AccountsBindByMonthDateDbCall:516 -> GetExpensesAccountForFOHAllocation -> [dbo].[usp_getExpensesAccountForFOHAllocation]. */
    public List<Map<String, Object>> expenseAccounts(int org, int comp, Date docDate) {
        return DesktopProc.rows(jdbc, "usp_getExpensesAccountForFOHAllocation", DesktopProc.params(
                "OrganizationId", org, "CompanyId", comp, "DocDate", docDate));
    }

    /** ProdcutionFohAllocateToJobOrder.GetByID: @Id, @Activity='ReadById'. */
    public List<Map<String, Object>> headerById(int id) {
        return DesktopProc.rows(jdbc, GET_ALL, DesktopProc.params("Id", id, "Activity", "ReadById"));
    }

    /** DAL GetData: @Id (the header Id), @Activity='ReadByHeaderId'. */
    public List<Map<String, Object>> detailsByHeaderId(int id) {
        return DesktopProc.rows(jdbc, GET_ALL, DesktopProc.params("Id", id, "Activity", "ReadByHeaderId"));
    }

    /**
     * HistoryFill:1188 -> ProdcutionFohAllocateToJobOrder.FormHistory. BranchesId is never set by the form (0 -> left out),
     * BranchesIds is null, JobOrderId / PlantId are 0 (left out). EntryUserId only when the user cannot view all records.
     */
    public List<Map<String, Object>> history(int org, int comp, int fy, boolean canViewAll, int userId,
                                             Date entryFrom, Date entryTo, Date modifyFrom, Date modifyTo,
                                             Date approvedFrom, Date approvedTo, String month) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", org);
        p.put("CompanyId", comp);
        p.put("DocumentTypeId", DOC_TYPE);
        p.put("FinancialYearId", fy);
        p.put("CanViewAllRecord", canViewAll ? 1 : 0);
        if (!canViewAll) p.put("EntryUserId", userId);
        p.put("EntryFromDate", entryFrom);
        p.put("EntryToDate", entryTo);
        p.put("ModifyFromDate", modifyFrom);
        p.put("ModifyToDate", modifyTo);
        p.put("ApprovedFromDate", approvedFrom);
        p.put("ApprovedToDate", approvedTo);
        if (month != null && !month.trim().isEmpty()) p.put("Month", month);
        p.put("Activity", "FormHistory");
        return DesktopProc.rows(jdbc, GET_ALL, p);
    }

    /**
     * CommonServices.ProdcutionFohAllocateToJobOrderPrint922:12987 -> ProdcutionFohAllocateToJobOrder_SlipAndRegister:
     * @Id only when non-zero; ApprovedFilter is never "All" so @IsApproved (false) is always sent.
     */
    public List<Map<String, Object>> slipRows(int org, int comp, int fy, int branch, int id) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", org);
        p.put("CompanyId", comp);
        p.put("FinancialYearId", fy);
        p.put("DocumentTypeId", DOC_TYPE);
        p.put("BranchesId", branch);
        if (id != 0) p.put("Id", id);
        p.put("IsApproved", Boolean.FALSE);
        return DesktopProc.rows(jdbc, "USP_ProdcutionFohAllocateToJobOrder_SlipAndRegister", p);
    }

    /** DAL SetData: [dbo].[USP_ProdcutionFohAllocateToJobOrder_InsertAndUpdate] (scalar Id on insert; nothing on update). */
    public int saveHeader(Map<String, Object> p) {
        return DesktopProc.setProc(jdbc, "USP_ProdcutionFohAllocateToJobOrder_InsertAndUpdate", p);
    }

    /** DAL SetData: [dbo].[USP_ProdcutionFohAllocateToJobOrderDetail_Insert] per detail row. */
    public int saveDetail(Map<String, Object> p) {
        return DesktopProc.setProc(jdbc, "USP_ProdcutionFohAllocateToJobOrderDetail_Insert", p);
    }

    /** BtnDeleteFohExpense_Click -> ProdcutionFohAllocateToJobOrder.DeleteByID: [dbo].[USP_ProductionFohAllocateToJobOrder_DeleteById]. */
    public void deleteById(int id, int entryUserId) {
        DesktopProc.rows(jdbc, "USP_ProductionFohAllocateToJobOrder_DeleteById", DesktopProc.params("EntryUserId", entryUserId, "Id", id));
    }
}
