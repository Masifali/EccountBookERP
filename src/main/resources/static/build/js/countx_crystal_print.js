/* ============================================================================================
 * CrystalPrint - one helper for every desktop "ShowReportWithDataTable..." button.
 *
 *   CrystalPrint.open(key, args [, button [, win]])
 *   CrystalPrint.reserve()   -> a blank tab opened NOW, inside the click; pass it as `win` when
 *                               the page must first check for rows asynchronously (the desktop's
 *                               "Record Not Found For Display" runs before the viewer opens).
 *                               Close it with CrystalPrint.release(win) if nothing is printed.
 *
 * POSTs the arg: values to /api/reports/{key}/print.pdf (ReportPrintController), where the
 * traced procedure runs with session tenancy and the real .rpt is rendered by the Crystal
 * bridge. A PDF opens in a new tab; anything else (Crystal disabled, template missing, no rows)
 * comes back as plain text and is shown as the message, never as a broken PDF.
 *
 * The tab is opened BEFORE the request so a popup blocker treats it as part of the click.
 * The optional button follows the page contract: disabled + spinner while running, duplicates
 * ignored, re-enabled on success and failure.
 * ============================================================================================ */
(function (global) {
    'use strict';

    function csrfHeaders(h) {
        var token = document.querySelector('meta[name="_csrf"]');
        var header = document.querySelector('meta[name="_csrf_header"]');
        if (token && header) h[header.getAttribute('content')] = token.getAttribute('content');
        return h;
    }

    function reserve() {
        var w = null;
        try { w = global.open('', '_blank'); } catch (e) { w = null; }
        if (w) {
            try { w.document.write('<p style="font:13px Segoe UI,sans-serif;padding:16px">Preparing report...</p>'); } catch (e) { /* ignore */ }
        }
        return w;
    }
    function release(w) { try { if (w) w.close(); } catch (e) { /* ignore */ } }

    function open(key, args, button, win) {
        var b = (typeof button === 'string') ? document.getElementById(button) : button;
        if (b) {
            if (b.classList.contains('is-busy')) return Promise.resolve();
            b.dataset.cpWasDisabled = b.disabled ? '1' : '';
            b.disabled = true;
            b.classList.add('is-busy');
        }
        var done = function () {
            if (!b) return;
            b.classList.remove('is-busy');
            b.disabled = b.dataset.cpWasDisabled === '1';
        };
        var w = win || reserve();
        return fetch('/api/reports/' + encodeURIComponent(key) + '/print.pdf', {
            method: 'POST',
            credentials: 'same-origin',
            headers: csrfHeaders({ 'Content-Type': 'application/json', 'Accept': 'application/pdf, text/plain' }),
            body: JSON.stringify(args || {})
        }).then(function (r) {
            var type = r.headers.get('Content-Type') || '';
            if (r.ok && type.indexOf('application/pdf') >= 0) {
                return r.blob().then(function (blob) {
                    var url = URL.createObjectURL(blob);
                    if (w) w.location.href = url; else global.open(url, '_blank');
                });
            }
            return r.text().then(function (t) {
                if (w) w.close();
                global.alert(t || ('Print failed (' + r.status + ')'));
            });
        }).catch(function (e) {
            if (w) w.close();
            global.alert(e.message);
        }).then(done, done);
    }

    global.CrystalPrint = { open: open, reserve: reserve, release: release };
}(window));
