/*
 * Trial Balance family prints (screens 51 TrialBalance, 81 SelectedTrialBalance) - 2026-10-02, group L-TRIAL-BALANCE-FAMILY.
 *
 * The desktop's print drop-downs list the .rpt files of a report folder (CommonServices.DynamicReportsLoad(folder)) and
 * push the form's DataTable into the clicked one (Reporting.ShowReportWithDataTable(dt, item + ".rpt") with
 * CompanyName / CompanyAddress). Here:
 *   TbPrint.menu(button, folder, onPick)   fills the drop-down from GET /accounts/api/reports-desktop/tb-family/dynamic-reports
 *   TbPrint.rows(rpt, rows)                 POST /reports/print/grid {rpt, title, rows} -> the PDF (Jasper; converted template
 *                                          when its fields match the rows, otherwise a layout generated from them)
 * A numbered button whose .rpt has a traced contract (113A, 111A) prints through window.printRpt (print-rpt.js) with the
 * filters of the last Show instead - see the pages.
 */
(function (w, $) {
    'use strict';
    var BASE = '/accounts/api/reports-desktop/tb-family';

    /* ToolStripDropDownButton look: the button opens a white menu under it */
    $('<style>').text('.tbp-dd{position:relative;display:inline-block}.tbp-dd>.win-btn.tbp-open{background:#dcecf8;border-color:#7fb5d6}'
        + '.tbp-menu{position:absolute;left:0;top:100%;min-width:240px;background:#fff;border:1px solid #9aa9b8;box-shadow:2px 2px 4px rgba(0,0,0,.25);z-index:60;padding:2px 0}'
        + '.tbp-menu div{padding:3px 20px;font:12px Verdana,sans-serif;white-space:nowrap;cursor:pointer}.tbp-menu div:hover{background:#dcecf8}'
        + '.tbp-menu div.none{color:#888;font-style:italic;cursor:default}.tbp-menu div.none:hover{background:transparent}').appendTo(document.head);

    function closeAll() { $('.tbp-menu').prop('hidden', true); $('.tbp-dd > .win-btn').removeClass('tbp-open'); }
    $(document).on('click', closeAll);

    function menu(btn, folder, onPick) {
        var $b = $(btn), $m = $b.siblings('.tbp-menu'), loaded = false;
        function load() {
            return $.ajax({ url: BASE + '/dynamic-reports', data: { folder: folder }, dataType: 'json' }).then(function (r) {
                var items = (r && r.success && r.data) || [];
                $m.empty();
                items.forEach(function (n) { $('<div>').text(n).attr('data-item', n).appendTo($m); });
                if (!items.length) $('<div class="none">').text('No .rpt in the "' + folder + '" report folder').appendTo($m);
                loaded = true;
            }, function () { $m.empty(); $('<div class="none">').text('The report folder could not be read').appendTo($m); });
        }
        $b.on('click', function (e) {
            e.stopPropagation();
            var open = $m.prop('hidden');
            closeAll();
            if (!open) return;
            $m.prop('hidden', false); $b.addClass('tbp-open');
            if (!loaded) load();
        });
        $m.on('click', '[data-item]', function (e) {
            e.stopPropagation();
            closeAll();
            onPick($(this).attr('data-item'));
        });
        load();
    }

    function rows(rpt, data, btn) {
        var $b = btn ? $(btn).prop('disabled', true) : null;
        var win = w.open('about:blank', '_blank');
        return fetch('/reports/print/grid', { method: 'POST', credentials: 'same-origin', headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ rpt: rpt + '.rpt', title: rpt, rows: data }) })
            .then(function (r) {
                var type = r.headers.get('Content-Type') || '';
                if (r.ok && type.indexOf('application/pdf') === 0) return r.blob().then(function (b) { if (win) win.location = URL.createObjectURL(b); });
                return r.text().then(function (t) { try { win.close(); } catch (x) { } alert(t || ('Print failed (' + r.status + ')')); });
            })
            .catch(function (e) { try { win.close(); } catch (x) { } alert(e.message); })
            .then(function () { if ($b) $b.prop('disabled', false); });
    }

    w.TbPrint = { menu: menu, rows: rows };
})(window, window.jQuery);
