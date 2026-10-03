/* ============================================================================================
 * Screen 84 "BS and PL Breakup" - desktop Architecture.WinApp.Account_Reports.BSandPLBreakup
 * (BSandPLBreakup.cs, base.Name "BSandPLBreakup", Text "EccountBook", ClientSize 1110x627, Maximized).
 * Rebuilt 2026-10-03 (group R-BALANCE-SHEET-BSPL) at the designer geometry:
 *   panel2 Dock Top 27   toolStrip3 (white): &Refresh (icons8_pencil_16, 70x24) | 158-Print (icons8_print_16, 79x24);
 *                        ctrlGrdBar1 (1065,0) 41x24 anchored Top|Right (the grid gear)
 *   panel3 Dock Top 42   rgb(10,110,110); label23 "BS Notes BreakUp" (10,8) Microsoft Sans Serif 14.25 white
 *   panel4 Dock Fill
 *     panel5 Dock Top 68 white: label27 "From Date" (5,7), fromdate (8,31) 98x21; label33 "To Date" (106,7),
 *            todate (109,31) 98x21; label34 "Account Notes" (207,7), cmbAccountNotes (210,28) 282x26
 *            MS Sans Serif 10.2; btnshow "Show" (497,28) 63x26 flat rgb(10,110,110) Segoe UI Semibold 11.25 bold white
 *     panel6 Dock Fill: grd (Janus GridEX, Office2007, AllowEdit false, FilterMode Automatic, RecordNavigator,
 *            TotalRow BottomFixed) - grd.LinkClicked
 *
 * The desktop form has only the constructor (UserAccount, DataTable): it is handed the parent's data table
 * (dtvoucher) and a NoteId by BalanceSheet.GetBsBreakup (Status 0) or frmProfitLossHararical.GetBsBreakup
 * (Status 1, FromDate) and never queries the report itself.
 *   Load     (VoucherValidation_Load) label23 "PL Notes BreakUp" + fromdate = FromDate when Status 1, else
 *            fromdate = ActiveYr.Start_Period; todate = ToDate; AccountTitleFill -> AccountNotes.ReadByBSNote(0) /
 *            ReadByPLNote() (SP_AccountNotes_ReadAllMethodBySPType), BindDDLNew Id/NoteTitle "Account Note";
 *            cmbAccountNotes.Value = NoteId when > 0; btnshow_Click; focus the combo.
 *   Show     rows of dtvoucher whose AccountNoteId == NoteId (the NoteId it was opened with - the combo's value
 *            is never read), columns Id (hidden) / AccountCode (= ParentAccountCode, Link) 100 / AccountTitle 250 /
 *            AccountNoteId (hidden) / AccountNotes 300 / Amount (Sum, right, stringFormatboth). No rows -> ClearStructure.
 *   Refresh  reset(): clears the combo text, focuses it.
 *   158-Print ShowReport(): the WHOLE dtvoucher (not the filtered rows) to 158-BalanceSheet.rpt, or
 *            "Record Not Found For Display" (here through the Jasper layer, POST /reports/print/grid).
 *   Link     grd_LinkClicked: SelectedTrialBalance (screen 81) with GroupId = Id, From/To = the two pickers.
 *
 * Two hosts:
 *   AccHBreakup.open(o)        the modal dialog (ShowDialog) the Balance Sheet / Profit & Loss pages open;
 *                              o = { status, noteId, rows, dec, fromDate, toDate }
 *   AccHBreakup.mount(host, o) the standalone page /accounts/reports/bs-pl-breakup (the hub card). There is no
 *                              parent table, so o.fetch({status, fromDate, toDate}) supplies dtvoucher (the parent's
 *                              own procedure) and Show reads the combo's note - see BsPlBreakupService.
 * ============================================================================================ */
(function (global) {
    'use strict';
    var H = global.AccH;
    var doc = global.document;

    var CSS = ''
        + '.bspl{position:relative;display:flex;flex-direction:column;height:100%;min-width:1110px;background:#fff;font:12px "Microsoft Sans Serif","Segoe UI",Tahoma,sans-serif;color:#000}'
        + '.bspl .p2{flex:0 0 27px;height:27px;position:relative;background:#fff;border-bottom:1px solid #d6dbe2}'
        + '.bspl .ts{position:absolute;left:0;top:0;right:48px;height:27px;display:flex;align-items:center;padding-left:2px;font:12px "Segoe UI",sans-serif;white-space:nowrap;overflow:hidden}'
        + '.bspl .ts button{display:inline-flex;align-items:center;gap:4px;height:24px;padding:0 5px;margin:0 1px;font:inherit;color:#000;background:transparent;border:1px solid transparent;cursor:pointer;white-space:nowrap}'
        + '.bspl .ts button:hover:not(:disabled){background:#cce8ff;border-color:#99d1ff}'
        + '.bspl .ts button:disabled{color:#a0a0a0;cursor:default}'
        + '.bspl .ts svg{width:16px;height:16px;flex:0 0 16px;margin:0 2px}'
        + '.bspl .gear{position:absolute;right:4px;top:0;width:41px;height:24px;display:flex;align-items:center;justify-content:flex-end}'
        + '.bspl .p3{flex:0 0 42px;height:42px;position:relative;background:rgb(10,110,110)}'
        + '.bspl .p3 span{position:absolute;left:10px;top:8px;font:19px "Microsoft Sans Serif","Segoe UI",sans-serif;line-height:24px;color:#fff;white-space:nowrap}'
        + '.bspl .p5{flex:0 0 68px;height:68px;position:relative;background:#fff}'
        + '.bspl .lb{position:absolute;font:12px "Microsoft Sans Serif","Segoe UI",sans-serif;line-height:15px;white-space:nowrap}'
        + '.bspl .dtp{position:absolute;box-sizing:border-box;height:21px;font:12px "Microsoft Sans Serif","Segoe UI",sans-serif;border:1px solid #7a7a7a;padding:0 1px;background:#fff}'
        + '.bspl .dtcombo-wrap .dtcombo-input{font:13.6px "Microsoft Sans Serif","Segoe UI",sans-serif !important}'
        + '.bspl .shw{position:absolute;left:497px;top:28px;width:63px;height:26px;box-sizing:border-box;padding:0;border:0;background:rgb(10,110,110);color:#fff;font:bold 15px "Segoe UI Semibold","Segoe UI",sans-serif;cursor:pointer;white-space:nowrap}'
        + '.bspl .shw:disabled{opacity:.75;cursor:progress}'
        + '.bspl .p6{flex:1 1 auto;display:flex;flex-direction:column;min-height:0}'
        + '.bspl .jg{flex:1 1 auto;display:flex;flex-direction:column;min-height:0;border:1px solid #9eb6ce}'
        + '.bspl .jg-wrap{flex:1 1 auto;overflow:auto;min-height:0;background:#fff}'
        + '.bspl .jg table{border-collapse:separate;border-spacing:0;table-layout:fixed;font:11px Verdana,sans-serif}'
        + '.bspl .jg th{position:sticky;top:0;z-index:3;height:20px;padding:0 4px;text-align:left;font-weight:normal;overflow:hidden;white-space:nowrap;text-overflow:ellipsis;background:linear-gradient(#f9fcfd,#d3e2f0);border-right:1px solid #9eb6ce;border-bottom:1px solid #9eb6ce}'
        + '.bspl .jg tr.flt td{position:sticky;top:21px;z-index:2;background:#fff;padding:1px;height:20px;border-bottom:1px solid #9eb6ce;border-right:1px solid #e3e9ef}'
        + '.bspl .jg tr.flt input{width:100%;height:17px;box-sizing:border-box;border:1px solid #c5d3e2;font:11px Verdana,sans-serif;padding:0 2px}'
        + '.bspl .jg td{height:19px;padding:0 4px;white-space:nowrap;overflow:hidden;text-overflow:ellipsis;box-sizing:border-box;border-right:1px solid #e3e9ef;border-bottom:1px solid #e3e9ef}'
        + '.bspl .jg td.r,.bspl .jg th.r{text-align:right}'
        + '.bspl .jg tbody tr.cur td{background:#ffe8a6}'
        + '.bspl .jg tfoot td{position:sticky;bottom:0;z-index:2;background:#e7eef6;border-top:1px solid #9eb6ce;font-weight:bold;height:20px}'
        + '.bspl .jg a.lnk{color:#0000ff;text-decoration:underline;cursor:pointer}'
        + '.bspl .jg-nav{flex:0 0 22px;height:22px;display:flex;align-items:center;gap:2px;padding:0 4px;background:linear-gradient(#f4f8fb,#dbe6f1);border-top:1px solid #9eb6ce;font:11px Verdana,sans-serif}'
        + '.bspl .jg-nav button{width:20px;height:18px;padding:0;border:1px solid transparent;background:transparent;cursor:pointer;font:10px Verdana}'
        + '.bspl .jg-nav button:hover:not(:disabled){border-color:#9eb6ce;background:#fff}'
        + '.bspl .jg-nav span{padding:0 6px;white-space:nowrap}'
        + '.bspl-dlg{position:relative;display:flex;flex-direction:column;width:1112px;height:657px;max-width:98vw;max-height:96vh;background:#fff;border:1px solid #4a6b8a;box-shadow:0 6px 24px rgba(0,0,0,.4)}'
        + '.bspl-dlg .wcap{flex:0 0 28px;height:28px;display:flex;align-items:center;justify-content:space-between;padding:0 0 0 8px;background:#fff;border-bottom:1px solid #d6dbe2;font:12px "Segoe UI",sans-serif}'
        + '.bspl-dlg .wcap button{width:44px;height:28px;border:0;background:transparent;font:14px "Segoe UI",sans-serif;cursor:pointer}'
        + '.bspl-dlg .wcap button:hover{background:#e81123;color:#fff}'
        + '.bspl-dlg .wbody{flex:1 1 auto;min-height:0;overflow-x:auto;overflow-y:hidden}';

    function injectCss() {
        if (doc.getElementById('bspl-css')) return;
        var s = doc.createElement('style'); s.id = 'bspl-css'; s.textContent = CSS; doc.head.appendChild(s);
    }

    var PENCIL = '<svg viewBox="0 0 16 16"><path d="M2 14l1-4 8-8 3 3-8 8z" fill="#f5c542" stroke="#8a6d1c"/><path d="M2 14l1-4 3 3z" fill="#555"/></svg>';
    var PRINTER = '<svg viewBox="0 0 16 16"><rect x="4" y="1.5" width="8" height="4" fill="#fff" stroke="#555"/><rect x="1.5" y="5.5" width="13" height="6" rx="1" fill="#6b7d8f"/><rect x="4" y="9.5" width="8" height="5" fill="#fff" stroke="#555"/></svg>';

    var seq = 0;
    function markup(id) {
        return '<div class="bspl">'
            + '<div class="p2"><div class="ts" role="toolbar">'
            + '<button type="button" data-a="new" title="Alt+R">' + PENCIL + '<span><u>R</u>efresh</span></button>'
            + '<button type="button" data-a="print">' + PRINTER + '<span>158-Print</span></button>'
            + '</div><div class="gear" data-f="gear"></div></div>'
            + '<div class="p3"><span data-f="cap">BS Notes BreakUp</span></div>'
            + '<div class="p5">'
            + '<span class="lb" style="left:5px;top:7px">From Date</span>'
            + '<input type="date" class="dtp" data-f="from" style="left:8px;top:31px;width:98px">'
            + '<span class="lb" style="left:106px;top:7px">To Date</span>'
            + '<input type="date" class="dtp" data-f="to" style="left:109px;top:31px;width:98px">'
            + '<span class="lb" style="left:207px;top:7px">Account Notes</span>'
            + '<select id="' + id + '_cmbAccountNotes" data-f="notes" data-dtcombo="single" data-dtcombo-caption="Account Note" style="position:absolute;left:210px;top:28px;width:282px;height:26px"></select>'
            + '<button type="button" class="shw" data-a="show">Show</button>'
            + '</div>'
            + '<div class="p6"><div class="jg">'
            + '<div class="jg-wrap" data-f="wrap"><table id="' + id + '_grd" data-f="grid" tabindex="0"></table></div>'
            + '<div class="jg-nav" data-f="nav"></div>'
            + '</div></div></div>';
    }

    var COLS = [
        { k: 'AccountCode', w: 100, link: true },
        { k: 'AccountTitle', w: 250 },
        { k: 'AccountNotes', w: 300 },
        { k: 'Amount', w: 120, num: true }
    ];

    function create(root, o, pageMode) {
        var status = o.status === 1 ? 1 : 0;
        var noteId = +o.noteId || 0;
        var rows = o.rows || [];               // dtvoucher
        var dec = o.dec || 0;
        var gridRows = null;                   // dtGrid (null = ClearStructure)
        var view = [], cur = -1, filters = {};
        function f(n) { return root.querySelector('[data-f="' + n + '"]'); }
        function busy(b, on) { b.disabled = !!on; b.classList.toggle('btn-busy', !!on); }

        f('cap').textContent = status === 1 ? 'PL Notes BreakUp' : 'BS Notes BreakUp';
        f('from').value = (o.fromDate || '').substring(0, 10);
        f('to').value = (o.toDate || '').substring(0, 10) || H.today();

        function comboInput() {
            var w = f('notes').closest('.dtcombo-wrap');
            return (w && w.querySelector('.dtcombo-input')) || f('notes');
        }
        function focusNotes() { setTimeout(function () { comboInput().focus(); }, 30); }

        /* ---------------------------------------------------------------- grid (Janus GridEX) */
        function fmt(c, v) { return c.num ? H.both(v, dec) : (v == null ? '' : String(v)); }
        function applyFilter() {
            view = (gridRows || []).filter(function (r) {
                return COLS.every(function (c) {
                    var q = (filters[c.k] || '').toLowerCase();
                    return !q || fmt(c, r[c.k]).toLowerCase().indexOf(q) >= 0;
                });
            });
        }
        function nav() {
            var n = view.length;
            f('nav').innerHTML = '<button type="button" data-nav="first" title="First">|&#9664;</button><button type="button" data-nav="prev" title="Previous">&#9664;</button>'
                + '<span>Record ' + (n ? cur + 1 : 0) + ' of ' + n + '</span><button type="button" data-nav="next" title="Next">&#9654;</button><button type="button" data-nav="last" title="Last">&#9654;|</button>';
        }
        function setCur(i) {
            if (!view.length) { cur = -1; nav(); return; }
            cur = Math.max(0, Math.min(view.length - 1, i));
            Array.prototype.forEach.call(f('grid').querySelectorAll('tbody tr'), function (tr) { tr.classList.toggle('cur', +tr.getAttribute('data-i') === cur); });
            nav();
        }
        function renderBody() {
            applyFilter();
            var total = 0, h = '';
            view.forEach(function (r, i) {
                total += H.toNum(r.Amount);
                h += '<tr data-i="' + i + '">' + COLS.map(function (c) {
                    var v = H.esc(fmt(c, r[c.k]));
                    return '<td' + (c.num ? ' class="r"' : '') + '>' + (c.link ? '<a class="lnk">' + v + '</a>' : v) + '</td>';
                }).join('') + '</tr>';
            });
            var t = f('grid');
            t.tBodies[0].innerHTML = h;
            t.tFoot.innerHTML = '<tr>' + COLS.map(function (c) { return '<td' + (c.num ? ' class="r"' : '') + '>' + (c.num ? H.esc(H.both(total, dec)) : '') + '</td>'; }).join('') + '</tr>';
            cur = view.length ? 0 : -1; setCur(cur);
        }
        function render() {
            var t = f('grid');
            if (gridRows === null) { t.innerHTML = ''; view = []; cur = -1; nav(); return; }   /* grd.ClearStructure() */
            t.style.width = COLS.reduce(function (a, c) { return a + c.w; }, 0) + 'px';
            t.innerHTML = '<colgroup>' + COLS.map(function (c) { return '<col style="width:' + c.w + 'px">'; }).join('') + '</colgroup>'
                + '<thead><tr>' + COLS.map(function (c) { return '<th data-col="' + c.k + '"' + (c.num ? ' class="r"' : '') + '>' + c.k + '</th>'; }).join('') + '</tr>'
                + '<tr class="flt">' + COLS.map(function (c) { return '<td><input type="text" data-k="' + c.k + '" value="' + H.esc(filters[c.k] || '') + '"></td>'; }).join('') + '</tr></thead>'
                + '<tbody></tbody><tfoot></tfoot>';
            renderBody();
        }
        f('grid').addEventListener('input', function (e) {
            var k = e.target.getAttribute && e.target.getAttribute('data-k'); if (!k) return;
            filters[k] = e.target.value; renderBody();
        });
        f('grid').addEventListener('click', function (e) {
            var tr = e.target.closest('tbody tr'); if (!tr) return;
            setCur(+tr.getAttribute('data-i'));
            if (e.target.closest('a.lnk')) {
                /* grd_LinkClicked: SelectedTrialBalance(GroupId = CurrentRow Id, FromDate / ToDate = the pickers).ShowDialog() */
                var r = view[cur];
                global.open('/accounts/reports/selected-trial-balance?groupAccountId=' + encodeURIComponent(r.Id == null ? '' : r.Id)
                    + '&fromDate=' + encodeURIComponent(f('from').value) + '&toDate=' + encodeURIComponent(f('to').value), '_blank');
            }
        });
        f('grid').addEventListener('keydown', function (e) {
            if (e.target.tagName === 'INPUT') return;
            if (e.key === 'ArrowDown') { e.preventDefault(); setCur(cur + 1); }
            else if (e.key === 'ArrowUp') { e.preventDefault(); setCur(cur - 1); }
        });
        f('nav').addEventListener('click', function (e) {
            var b = e.target.closest('button'); if (!b) return;
            var a = b.getAttribute('data-nav');
            setCur(a === 'first' ? 0 : a === 'last' ? view.length - 1 : a === 'prev' ? cur - 1 : cur + 1);
        });
        nav();

        /* ---------------------------------------------------------------- btnshow_Click */
        function filterRows() {
            if (!rows.length) { gridRows = null; render(); return; }
            gridRows = rows.filter(function (r) { return (+H.ci(r, 'AccountNoteId') || 0) === noteId; }).map(function (r) {
                return { Id: H.ci(r, 'Id'), AccountCode: H.ci(r, 'ParentAccountCode'), AccountTitle: H.ci(r, 'AccountTitle'),
                         AccountNoteId: H.ci(r, 'AccountNoteId'), AccountNotes: H.ci(r, 'AccountsNotes'), Amount: H.toNum(H.ci(r, 'Amount')) };
            });
            render();
        }
        function show(btn) {
            if (!pageMode) { filterRows(); return Promise.resolve(); }
            /* standalone page: no parent table - the note comes from the combo and dtvoucher from the parent's procedure */
            noteId = +f('notes').value || 0;
            var b = btn || root.querySelector('[data-a="show"]');
            if (b.classList.contains('btn-busy')) return Promise.resolve();
            busy(b, true);
            return o.fetch({ status: status, fromDate: f('from').value, toDate: f('to').value })
                .then(function (r) { rows = r || []; filterRows(); })
                .catch(function (e) { H.box(e.message); })
                .then(function () { busy(b, false); });
        }

        /* ---------------------------------------------------------------- ShowReport (158-Print) */
        function print(btn) {
            if (btn.classList.contains('btn-busy')) return;
            if (!rows.length) { H.box('Record Not Found For Display'); return; }
            var win = global.open('about:blank', '_blank');
            busy(btn, true);
            fetch('/reports/print/grid', { method: 'POST', credentials: 'same-origin', headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ rpt: '158-BalanceSheet.rpt', title: null, rows: rows }) })
                .then(function (r) {
                    var type = r.headers.get('Content-Type') || '';
                    if (r.ok && type.indexOf('application/pdf') === 0) return r.blob().then(function (bl) { win.location = URL.createObjectURL(bl); });
                    return r.text().then(function (x) { try { win.close(); } catch (e) { } H.box(x || ('Print failed (' + r.status + ')')); });
                })
                .catch(function (e) { try { win.close(); } catch (x) { } H.box(e.message); })
                .then(function () { busy(btn, false); });
        }

        root.addEventListener('click', function (e) {
            var b = e.target.closest('[data-a]'); if (!b || b.disabled) return;
            var a = b.getAttribute('data-a');
            if (a === 'show') show(b);
            else if (a === 'new') { f('notes').value = ''; f('notes').dispatchEvent(new Event('change', { bubbles: true })); focusNotes(); }
            else if (a === 'print') print(b);
        });
        root.addEventListener('keydown', function (e) {
            /* toolStrip mnemonic &Refresh; the form has no KeyDown handler */
            if (e.altKey && !e.ctrlKey && (e.key || '').toUpperCase() === 'R') { e.preventDefault(); root.querySelector('[data-a="new"]').click(); }
        });

        /* ctrlGrdBar1 (the grid gear, layout key BSandPLBreakup_grd_<user>) */
        if (global.GridBar && global.GridBar.attach) {
            try { global.GridBar.attach(f('wrap'), { form: 'BSandPLBreakup', grid: 'grd' }, { mount: f('gear'), title: f('cap').textContent, reorder: false }); } catch (x) { }
        }

        /* ---------------------------------------------------------------- VoucherValidation_Load */
        var sb = root.querySelector('[data-a="show"]');
        busy(sb, true);
        var ready = H.api('GET', '/accounts/api/reports/balance-sheet-62/breakup-notes?status=' + status).then(function (notes) {
            var s = f('notes');
            s.innerHTML = '<option value=""></option>' + (notes || []).map(function (n) {
                return '<option value="' + H.esc(n.Id) + '">' + H.esc(n.NoteTitle) + '</option>';
            }).join('');
            if (noteId > 0) s.value = String(noteId);
            if (global.DesktopCombo && global.DesktopCombo.enhance) global.DesktopCombo.enhance(s);
        }).catch(function (e) { H.box(e.message); }).then(function () { busy(sb, false); return show(sb); }).then(focusNotes);

        return { root: root, ready: ready };
    }

    /* BalanceSheet / frmProfitLossHararical: new BSandPLBreakup(UserAccount, dt) { ... }.ShowDialog() */
    function open(o) {
        injectCss();
        var id = 'bspl' + (++seq);
        var m = doc.createElement('div');
        m.className = 'h-modal';
        m.innerHTML = '<div class="bspl-dlg" role="dialog" aria-label="EccountBook"><div class="wcap"><span>EccountBook</span><button type="button" data-x title="Close">&#10005;</button></div>'
            + '<div class="wbody">' + markup(id) + '</div></div>';
        doc.body.appendChild(m);
        m.querySelector('[data-x]').addEventListener('click', function () { m.remove(); });
        return create(m.querySelector('.bspl'), o || {}, false);
    }

    /* the standalone page (hub card, screen 84) */
    function mount(host, o) {
        injectCss();
        host.innerHTML = markup('bsplPage');
        return create(host.querySelector('.bspl'), o || {}, true);
    }

    global.AccHBreakup = { open: open, mount: mount };
})(window);
