package com.mst.repositories;

import com.mst.repositories.support.DesktopProc;
import java.util.*;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

/** Dashboard BLL 0065:1125; frmApprovalCommentory.GridFill:130. */
@Repository
public class SaleReportApprovalRepository {
    private final JdbcTemplate jdbc;
    public SaleReportApprovalRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }
    public List<Map<String, Object>> history(int documentTypeId, int id) {
        return DesktopProc.rows(jdbc, "USp_ApprovalCommentory", DesktopProc.params(
                "RefDocumentTypeId", documentTypeId, "RefDocNoId", id));
    }
}
