/* ============================================================================================
 * Screen 84 "BS and PL Breakup" - BSandPLBreakup.cs, reproduced as the modal dialog the desktop
 * opens it as (ShowDialog from BalanceSheet.GetBsBreakup and frmProfitLossHararical.GetBsBreakup).
 *
 * The desktop form has only the constructor (UserAccount, DataTable): it is always handed the
 * parent's data table (dtvoucher) and a NoteId, and never queries the report itself.
 *   Load     label23 "BS Notes BreakUp" / "PL Notes BreakUp" (Status 1); fromdate = ActiveYr.Start_Period
 *            (Status 0) or the P&L From Date (Status 1); todate = parent To Date;
 *            AccountTitleFill -> ReadByBSNote(0) / ReadByPLNote(); cmbAccountNotes = NoteId; Show.
 *   Show     rows of dtvoucher whose AccountNoteId == NoteId (the NoteId it was opened with - the
 *            combo's value is never read), columns Id(hidden) / AccountCode (= ParentAccountCode,
 *            link) / AccountTitle / AccountNoteId(hidden) / AccountNotes / Amount (Sum).
 *   Refresh  clears the combo text only.
 *   158-Print the WHOLE dtvoucher (not the filtered rows), "Record Not Found For Display" when empty.
 *   Link     AccountCode -> SelectedTrialBalance(GroupId = Id, From/To = the two pickers).
 * ============================================================================================ */
(function (global) {
    'use strict';
    var H = global.AccH;

    function open(o) {
        var rows = o.rows || [];
        var noteId = +o.noteId || 0;
        var dec = o.dec || 0;
        var m = document.createElement('div');
        m.className = 'h-modal';
        /* countx_acc_rpt_f.css look: title bar, toolbar, teal header panel, "Filters" group box, grid */
        m.innerHTML =
            '<div class="h-dlg accf" style="width:1110px;height:627px;display:flex;flex-direction:column;max-height:96vh">'
          + '<div class="win-title-bar"><span>EccountBook</span><a data-a="x" title="Close" style="cursor:pointer">&#10005;</a></div>'
          + '<div class="win-toolbar"><button type="button" class="win-btn" data-a="new"><i class="fa fa-refresh text-success"></i> <span><u>R</u>efresh</span></button>'
          + '<button type="button" class="win-btn" data-a="print"><i class="fa fa-print"></i> 158-Print</button></div>'
          + '<div class="win-header-panel"><span data-f="cap"></span></div>'
          + '<div class="filters"><div class="gb" style="width:600px"><span class="gb-title">Filters</span>'
          + '<div class="fgrid" style="grid-template-columns: 110px 110px 1fr 70px;">'
          + '<span class="lbl">From Date</span><span class="lbl">To Date</span><span class="lbl">Account Notes</span><span></span>'
          + '<input type="date" class="inp" data-f="from"><input type="date" class="inp" data-f="to">'
          + '<select class="inp" data-f="notes" data-dtcombo="single" data-dtcombo-caption="Account Notes"></select>'
          + '<button type="button" class="win-btn-show" data-a="show"><i class="fa fa-eye"></i> Show</button>'
          + '</div></div></div>'
          + '<div class="grid-caption"><span>Filtered Records</span></div>'
          + '<div class="grid-wrap"><table class="wg" data-f="grid"></table></div></div>';
        document.body.appendChild(m);
        function f(n) { return m.querySelector('[data-f="' + n + '"]'); }
        f('cap').textContent = o.status === 1 ? 'PL Notes BreakUp' : 'BS Notes BreakUp';
        f('from').value = (o.fromDate || '').substring(0, 10);
        f('to').value = (o.toDate || '').substring(0, 10);

        /* the combo's visible field when countx_prod_combo.js has enhanced the select */
        function focusNotes() {
            setTimeout(function () {
                var w = f('notes').closest('.dtcombo-wrap'), i = w && w.querySelector('.dtcombo-input');
                (i || f('notes')).focus();
            }, 30);
        }

        function show() {
            var g = f('grid');
            if (!rows.length) { g.innerHTML = ''; return; }
            var list = rows.filter(function (r) { return (+H.ci(r, 'AccountNoteId') || 0) === noteId; });
            var total = 0;
            var h = '<thead><tr><th style="width:100px">AccountCode</th><th style="width:250px">AccountTitle</th><th style="width:300px">AccountNotes</th><th style="width:110px">Amount</th></tr></thead><tbody>';
            list.forEach(function (r, i) {
                total += H.toNum(H.ci(r, 'Amount'));
                h += '<tr><td><a data-i="' + i + '">' + H.esc(H.ci(r, 'ParentAccountCode')) + '</a></td><td>' + H.esc(H.ci(r, 'AccountTitle'))
                   + '</td><td>' + H.esc(H.ci(r, 'AccountsNotes')) + '</td><td class="num">' + H.esc(H.both(H.ci(r, 'Amount'), dec)) + '</td></tr>';
            });
            h += '</tbody><tfoot><tr><td></td><td></td><td></td><td class="num">' + H.esc(H.both(total, dec)) + '</td></tr></tfoot>';
            g.innerHTML = h;
            g.querySelectorAll('a[data-i]').forEach(function (a) {
                a.addEventListener('click', function () {
                    var r = list[+a.getAttribute('data-i')];
                    global.open('/accounts/reports/selected-trial-balance?groupAccountId=' + encodeURIComponent(H.ci(r, 'Id'))
                        + '&fromDate=' + encodeURIComponent(f('from').value) + '&toDate=' + encodeURIComponent(f('to').value), '_blank');
                });
            });
        }

        H.api('GET', '/accounts/api/reports/balance-sheet-62/breakup-notes?status=' + (o.status === 1 ? 1 : 0)).then(function (notes) {
            f('notes').innerHTML = '<option value=""></option>' + (notes || []).map(function (n) {
                return '<option value="' + H.esc(n.Id) + '">' + H.esc(n.NoteTitle) + '</option>';
            }).join('');
            if (noteId > 0) f('notes').value = String(noteId);
        }).catch(function (e) { H.box(e.message); }).then(function () { show(); focusNotes(); });

        m.addEventListener('click', function (e) {
            var b = e.target.closest('[data-a]'); if (!b) return;
            var a = b.getAttribute('data-a');
            if (a === 'x') m.remove();
            else if (a === 'show') show();
            else if (a === 'new') { f('notes').value = ''; focusNotes(); }
            else if (a === 'print') {
                if (!rows.length) { H.box('Record Not Found For Display'); return; }
                var cols = Object.keys(rows[0]);
                H.printRows('158 - BalanceSheet', 'Browser output of the parent form\'s data table (dtvoucher) - not the Crystal layout.', cols, rows);
            }
        });
    }

    global.AccHBreakup = { open: open };
})(window);
