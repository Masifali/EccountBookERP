package com.mst.services;

import com.mst.models.SaleOrderReportColumns;
import com.mst.repositories.SaleReportApprovalRepository;
import java.util.*;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;

@Service
public class SaleReportApprovalService {
    private static final String COLUMNS = "Id:Id,DocumentName:DocumentName,ApproveLevel:ApprovalLevel,UserName:UserName,UserComments:UserComments,Approved_Status:Approved_Status,ApprovalDate:ApproveDate,Rejected_Status:Rejected_Status,RejectedDate:RejectedDate,IsFinalApprover:IsFinalApprover,IsMandatory:IsMandatory";
    private final SaleReportApprovalRepository repo;
    public SaleReportApprovalService(SaleReportApprovalRepository repo) { this.repo = repo; }

    /** Caller supplies the original report result after its View/user/company/branch checks. */
    @SuppressWarnings("unchecked")
    public Map<String, Object> history(Map<String, Object> report, String rowKey, int documentTypeId, int id) {
        var rows = (List<Map<String, Object>>) report.get(rowKey);
        boolean present = id > 0 && documentTypeId > 0 && rows != null && rows.stream().anyMatch(row ->
                row.get("Id") instanceof Number record && record.intValue() == id
                && row.get("DocumentTypeId") instanceof Number type && type.intValue() == documentTypeId);
        if (!present) throw new AccessDeniedException("This document is not available in the selected report");
        // USp_ApprovalCommentory has no organization/company parameters. Never invoke it for an
        // arbitrary client Id; the report above supplies the document's authorized identity.
        return Map.of("columns", SaleOrderReportColumns.names(COLUMNS), "rows", repo.history(documentTypeId, id)
                .stream().map(row -> SaleOrderReportColumns.project(row, COLUMNS)).toList());
    }
}
