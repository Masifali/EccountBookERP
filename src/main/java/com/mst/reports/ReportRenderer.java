package com.mst.reports;

import java.util.Map;

/**
 * Turns a report's rows into a printable document.
 *
 * There is an interface here because there are two honest ways to print these reports and the
 * choice is an operational one, not a code one:
 *
 *   CrystalBridgeRenderer  hands the rows to the REAL Crystal engine running against the REAL
 *                          .rpt, so the output is identical to the desktop - same fonts, same
 *                          bands, same formula fields, same group totals. Needs Windows with the
 *                          Crystal runtime.
 *
 *   (future) Jasper        a rebuilt template, pure Java, no Windows, but a redrawn layout.
 *
 * Both are fed by the same ReportDataService result, so swapping one for the other changes no
 * caller and no contract.
 */
public interface ReportRenderer {

    /** Is this renderer usable right now? Checked, never assumed. */
    boolean available();

    /** Why it is not usable, for the API and the page. Empty when it is. */
    String unavailableReason();

    /**
     * @param result exactly what ReportDataService.run returned - template, rows, subReports and
     *               reportParameters. Nothing is re-queried here.
     * @return the PDF bytes.
     */
    byte[] renderPdf(Map<String, Object> result) throws Exception;
}
