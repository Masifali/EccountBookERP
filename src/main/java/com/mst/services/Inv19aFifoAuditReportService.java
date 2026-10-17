package com.mst.services;

import com.mst.security.CurrentUserContext;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;

import java.util.*;

/**
 * frmFiFoAuditReport (Architecture.WinApp.Inventory_Reports/frmFiFoAuditReport.cs) - ScreenDefinition 290 "FIFO Audit Report"
 * (Inventory hub, module 19 Stock Reports). Line refs ":NNN" are that file.
 * Data: BLL InventoryStockEvalautionDetail.GetFIFODataForAudit -> dbo.USP_GetFIFODataForAudit (@OrganizationId,@CompanyId) - rows whose
 * InWeight - OutWeight < 0 (RefDocumentTypeId, RefDocIdNo, RefDocSubIdNo, DocumentTypeCode, DocCodeNo, ReferedDocNos, InQty, OutQty, BalQty, InWeight, OutWeight, BalWeight, CompLogoImage).
 * TranDetail button :273 opens StockTransactionDetail(RefDocumentTypeId, RefDocId, RefDocDetailId) - served here through
 * dbo.USP_GetTransactionDetailAgainstVehicle (the procedure the existing web StockTransactionDetail port, InventoryStockEvaluationVehicleWiseRepository, uses).
 */
@Service
public class Inv19aFifoAuditReportService {

    private final JdbcTemplate jdbc;
    private final CurrentUserContext context;

    public Inv19aFifoAuditReportService(JdbcTemplate jdbc, CurrentUserContext context) { this.jdbc = jdbc; this.context = context; }

    /** GridFill :85 - dt = GetFIFODataForAudit; the grid shows the twelve columns :99-110, the print (390_GetFIFODataForAudit.rpt) gets dt itself. */
    public List<Map<String, Object>> show() {
        return jdbc.queryForList("EXEC dbo.USP_GetFIFODataForAudit @OrganizationId=?,@CompanyId=?", context.currentOrganizationId(), context.currentCompanyId());
    }

    /** grdfrm_ColumnButtonClick :273 - StockTransactionDetail child form for the current row. */
    public List<Map<String, Object>> transactionDetail(int refDocumentTypeId, int refDocIdNo, int refDocSubIdNo) {
        return jdbc.queryForList("EXEC dbo.USP_GetTransactionDetailAgainstVehicle @OrganizationId=?,@CompanyId=?,@RefRefDocumentTypeId=?,@RefRefDocIdNo=?,@RefRefDocSubIdNo=?",
                context.currentOrganizationId(), context.currentCompanyId(), refDocumentTypeId, refDocIdNo, refDocSubIdNo);
    }
}
