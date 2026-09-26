package com.mst.security;

import org.junit.jupiter.api.Test;
import org.springframework.mock.web.MockHttpServletRequest;
import org.springframework.mock.web.MockHttpServletResponse;
import static org.junit.jupiter.api.Assertions.assertEquals;

class ReportFrameHeaderWriterTest {
    private String header(String path, String context) {
        MockHttpServletRequest request = new MockHttpServletRequest("GET", context + path);
        request.setContextPath(context);
        MockHttpServletResponse response = new MockHttpServletResponse();
        new ReportFrameHeaderWriter().writeHeaders(request, response);
        return response.getHeader("X-Frame-Options");
    }

    @Test void reportChildrenAllowOnlyTheSameOrigin() {
        assertEquals("SAMEORIGIN", header("/accounts/reports/receivables-receipt-schedule", ""));
        assertEquals("SAMEORIGIN", header("/accounts/reports/trade-payables", "/erp"));
        assertEquals("SAMEORIGIN", header("/accounts/accounts/reports/general-ledger", ""));
    }

    @Test void otherPagesRemainProtected() {
        assertEquals("DENY", header("/login", ""));
        assertEquals("DENY", header("/accounts/vouchers/cash-payment", ""));
        assertEquals("DENY", header("/api/accounts/trade-report", ""));
        assertEquals("DENY", header("/accounts/reports-other/test", ""));
    }
}
