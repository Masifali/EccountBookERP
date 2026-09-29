package com.mst.security;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import org.springframework.security.web.header.HeaderWriter;

/**
 * X-Frame-Options per path: SAMEORIGIN only for the pages this application frames into its own
 * pages, DENY everywhere else.
 *
 *  - /accounts/reports/, /accounts/accounts/reports/ - the desktop-style report launcher.
 *  - /production/ - screen 280 "Production Against (Job Order)" hosts its Input, Output,
 *    PackingMaterial, Overhead, Summary (/production/reports/production-summary) and Settlement
 *    tabs as framed pages (FoodProductionWithValues drops each form into its panel), and the
 *    production screens open /production/wages-bill in a framed modal.
 *  - /accounts/vouchers/labour-wages - the Labour Wages voucher framed by Production Input.
 *
 * With DENY on those paths every framed tab rendered as Chrome's blocked-frame page.
 */
public final class ReportFrameHeaderWriter implements HeaderWriter {
    @Override
    public void writeHeaders(HttpServletRequest request, HttpServletResponse response) {
        String path = request.getRequestURI().substring(request.getContextPath().length());
        boolean framed = path.startsWith("/accounts/reports/")
                || path.startsWith("/accounts/accounts/reports/")
                || path.startsWith("/production/")
                || path.startsWith("/accounts/vouchers/labour-wages");
        response.setHeader("X-Frame-Options", framed ? "SAMEORIGIN" : "DENY");
    }
}
