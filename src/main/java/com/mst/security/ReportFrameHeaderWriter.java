package com.mst.security;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import org.springframework.security.web.header.HeaderWriter;

/** The desktop-style report launcher embeds reports from this application's origin. */
public final class ReportFrameHeaderWriter implements HeaderWriter {
    @Override
    public void writeHeaders(HttpServletRequest request, HttpServletResponse response) {
        String path = request.getRequestURI().substring(request.getContextPath().length());
        boolean report = path.startsWith("/accounts/reports/")
                || path.startsWith("/accounts/accounts/reports/");
        response.setHeader("X-Frame-Options", report ? "SAMEORIGIN" : "DENY");
    }
}
