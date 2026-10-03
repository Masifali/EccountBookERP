/* =============================================================================================
 * Screen 700 "Cheque Printing" - port of Architecture.WinApp.ChequePrinting.ChequePrinting (2026-10-02).
 *
 * Desktop handlers -> here
 *   ChequePrinting_Load            BankFill() [cmbBanks Visible=false - not shown] + ChequeCountGridFill()   -> load()
 *   ChequePrinting_Shown           getReportPath(); VoucherId != 0 -> GetDetailData(0, VoucherId)
 *                                  (the static VoucherId is never assigned by any caller - nothing to do)
 *   grdChequeCount_DoubleClick     BankId = CurrentRow.Id; GetDetailData(BankId, 0)                            -> dblclick on a row
 *   btn294APrint_Click             PrintCheque()                                                             -> #btn294APrint
 *   btnCheckAll_Click              Select All / Deselect All of the cards of the current template            -> #btnCheckAll
 *   card cbSelect CheckedChanged   Tag = "FormCheckBox"; Click -> HandleChequeSelection                       -> card checkbox
 *   card button1 Click             Tag = "ButtonClicked"; HandleChequeSelection -> uncheckAll + PrintCheque   -> card Print
 *   btnLoad_Click / cmbBanks_Leave / cmbBanks_ValueChanged / BtnLoadAll_Click: controls hidden or empty handlers.
 * PrintCheque(): no selection -> "Please Select Cheque to print"; else POST /print (sp_ChequePrintList_Print ->
 * <ChequeTemplete>.rpt, then sp_ChequePrintList_UpdateStatus); finally ChequeCountGridFill() + GetDetailData(BankId, 0).
 * ============================================================================================= */
(function () {
    'use strict';

    var API = '/accounts/api/banking/cheque-printing';

    /* Card user controls (Architecture.WinApp.DynamicCards.*) - InitializeComponent positions.
       amt = lblChequeAmount [x, y, back] (82x20, Verdana 7pt, MiddleRight + RightToLeft)
       pay = lblPayeeTitle [x, y, back]; no = lblChequeNumber [x, y, back]
       dx/dy = lblD1 D2 M1 M2 Y1 Y2 Y3 Y4 positions, dbg = their back colour (null = WhiteSmoke, the parent's)
       w = lblAmountinWords TextBox [x, y, w, h, back]; img = Properties.Resources picture (pictureBox1 1,21 429x209). */
    var X0 = [284, 301, 318, 335, 352, 369, 386, 403], X2 = [282, 299, 316, 333, 350, 367, 384, 401];
    function ys(y, last) { return [y, y, y, y, y, y, y, last == null ? y : last]; }
    var CARDS = {
        UBL:         { img: 'UBL.jpg',         amt: [302, 108, '#d3eef8'], pay: [36, 93, '#d3eef8'], dx: X0, dy: ys(80), dbg: null,      w: [48, 113, 244, 40, '#eff9fe'], no: [304, 60, '#d3eef8'] },
        ABL:         { img: 'ABL.jpg',         amt: [303, 102, '#f2cfb3'], pay: [34, 87, '#f2cfb3'], dx: X0, dy: ys(58, 59), dbg: null,  w: [48, 108, 244, 40, '#f6f2ee'], no: [351, 33, '#f2cfb3'] },
        AskariBank:  { img: 'AskariBank.jpg',  amt: [302, 111, '#d3eef8'], pay: [29, 94, '#d3eef8'], dx: X0, dy: ys(69), dbg: null,      w: [46, 115, 244, 40, '#eff9fe'], no: [352, 40, '#d3eef8'] },
        BankAlFalah: { img: 'BankAlFalah.jpg', amt: [302, 109, '#f7eee5'], pay: [36, 93, '#f7eee5'], dx: X0, dy: ys(67, 68), dbg: null,  w: [48, 113, 244, 40, '#f7eee5'], no: [323, 40, '#f7eee5'] },
        BankAlHabib: { img: 'BankAlHabib.jpg', amt: [300, 103, '#bdd0b6'], pay: [36, 80, '#bdd0b6'], dx: [281, 298, 315, 332, 349, 366, 383, 400], dy: ys(44), dbg: '#bdd0b6',
                       w: [42, 105, 228, 40, '#bdd0b6'], no: [313, 27, '#bdd0b6'] },
        FaysalBank:  { img: 'FaysalBank.png',  amt: [300, 136, '#ffffff'], pay: [33, 100, '#d6e4e6'], dx: [31, 48, 65, 82, 99, 116, 133, 150], dy: ys(71), dbg: null,
                       w: [42, 128, 244, 40, '#d6e4e6'], no: [116, 28, '#edecea'] },
        HabibMetro:  { img: 'HabibMetro.jpg',  amt: [322, 129, '#e4e9c9'], pay: [51, 92, '#f9fbed'], dx: X0, dy: ys(64), dbg: null,      w: [48, 113, 244, 40, '#dff3ce'], no: [344, 38, '#f9fbed'] },
        HBL:         { img: 'HBL.jpg',         amt: [311, 111, '#cce8dc'], pay: [41, 97, '#c3e6e0'], dx: X0, dy: ys(66, 67), dbg: null,  w: [56, 116, 240, 40, '#a4cdc3'], no: [318, 41, '#c3e6e0'] },
        MCB:         { img: 'MCB.jpg',         amt: [302, 107, '#94c19a'], pay: [36, 93, '#94c19a'], dx: X0, dy: ys(63), dbg: '#94c19a', w: [44, 113, 244, 40, '#acc8ae'], no: [344, 35, '#94c19a'] },
        MeezanBank:  { img: 'MeezanBank.jpg',  amt: [302, 108, '#dcc5cf'], pay: [31, 97, '#dcc5cf'], dx: X2, dy: ys(59), dbg: '#f1dde9', w: [43, 115, 244, 40, '#f1dde9'], no: [344, 34, '#dcc5cf'] },
        NBP:         { img: 'NBP.jpg',         amt: [298, 101, '#dcdbd1'], pay: [44, 85, '#dcdbd1'], dx: X0, dy: ys(56), dbg: '#dcdbd1', w: [41, 109, 244, 40, '#dcdbd1'], no: [326, 28, '#dcdbd1'] },
        NIB:         { img: 'NIB.jpg',         amt: [302, 105, '#fac0aa'], pay: [31, 89, '#fac0aa'], dx: X2, dy: ys(67, 68), dbg: null,  w: [39, 112, 244, 29, '#fdfdfc'], no: [346, 37, '#fac0aa'] },
        SaadiqStandardCharteredBank: { img: 'SaadiqStandardCharteredBank.jpg', amt: [283, 102, '#d1e3ed'], pay: [38, 85, '#d1e3ed'], dx: X0, dy: ys(54), dbg: null,
                       w: [16, 110, 256, 40, '#d1e3ed'], no: [65, 197, '#ffffff'] },
        SilkBank:    { img: 'SilkBank.jpg',    amt: [303, 118, '#7abfe1'], pay: [36, 102, '#c5dbe8'], dx: X0, dy: ys(77), dbg: null,     w: [39, 121, 244, 40, '#c5dbe8'], no: [345, 52, '#d3eef8'] },
        SoneriBank:  { img: 'SoneriBank.jpg',  amt: [302, 102, '#d8d1c7'], pay: [33, 86, '#d8d1c7'], dx: X0, dy: ys(65, 66), dbg: null,  w: [46, 109, 244, 40, '#e6e2d9'], no: [325, 37, '#d8d1c7'] }
    };
    var DATE_KEYS = ['D1', 'D2', 'M1', 'M2', 'Y1', 'Y2', 'Y3', 'Y4'];

    /* form state (the desktop fields) */
    var S = {
        BankId: 0,                 // set only by grdChequeCount_DoubleClick
        ChequeTemplate: '',
        selectall: false,
        SelectedChequesList: [],
        countRows: [],
        cur: -1,
        printing: false
    };

    function $(id) { return document.getElementById(id); }
    function esc(v) { return String(v == null ? '' : v).replace(/[&<>"']/g, function (c) { return '&#' + c.charCodeAt(0) + ';'; }); }

    function getJson(url) {
        return fetch(url, { credentials: 'same-origin', headers: { 'Accept': 'application/json' } }).then(function (r) {
            if (!r.ok) return r.text().then(function (t) { throw new Error(t || ('HTTP ' + r.status)); });
            return r.json();
        });
    }

    /* ------------------------------------------------------------------ ChequeCountGridFill */
    function chequeCountGridFill() {
        return getJson(API + '/counts').then(function (rows) {
            if (rows && rows.length > 0) {          // if (dtChequeCount.Rows.Count > 0) - otherwise the grid is left as it was
                S.countRows = rows;
                S.cur = -1;
                renderGrid();
            }
        }).catch(function (e) { alert(e.message); });
    }

    function filtered() {
        var f = {};
        Array.prototype.forEach.call(document.querySelectorAll('#grdChequeCount tr.flt input'), function (i) { f[i.getAttribute('data-col')] = i.value.trim().toLowerCase(); });
        return S.countRows.map(function (r, i) { return { r: r, i: i }; }).filter(function (x) {
            return Object.keys(f).every(function (k) { return !f[k] || String(x.r[k] == null ? '' : x.r[k]).toLowerCase().indexOf(f[k]) >= 0; });
        });
    }

    function renderGrid() {
        var rows = filtered(), html = '';
        rows.forEach(function (x, n) {
            html += '<tr data-i="' + x.i + '" data-n="' + n + '"' + (x.i === S.cur ? ' class="cur"' : '') + '><td title="' + esc(x.r.BranchName) + '">' +
                esc(x.r.BranchName) + '</td><td class="num">' + esc(x.r.Counts) + '</td></tr>';
        });
        $('grdChequeCount').tBodies[0].innerHTML = html;
        navText();
    }

    function visibleRows() { return Array.prototype.slice.call($('grdChequeCount').tBodies[0].rows); }

    function setCurrent(tr) {
        visibleRows().forEach(function (r) { r.classList.toggle('cur', r === tr); });
        S.cur = tr ? +tr.getAttribute('data-i') : -1;
        if (tr) tr.scrollIntoView({ block: 'nearest' });
        navText();
    }

    function navText() {
        var rows = visibleRows(), pos = -1;
        rows.forEach(function (r, n) { if (+r.getAttribute('data-i') === S.cur) pos = n; });
        $('navText').textContent = 'Record ' + (pos + 1) + ' of ' + rows.length;
    }

    /* grdChequeCount_DoubleClick */
    function gridDoubleClick() {
        if (S.cur < 0 || !S.countRows[S.cur]) return;
        S.BankId = parseInt(S.countRows[S.cur].Id, 10) || 0;
        getDetailData(S.BankId);
    }

    /* ------------------------------------------------------------------ GetDetailData(BankId, 0) */
    function getDetailData(bankId) {
        S.SelectedChequesList = [];
        return getJson(API + '/cards?bankId=' + encodeURIComponent(bankId)).then(function (res) {
            var cards = (res && res.cards) || [];
            if (cards.length === 0) { $('flowLayoutPanel1').innerHTML = ''; return; }
            S.ChequeTemplate = res.chequeTemplate == null ? '' : String(res.chequeTemplate);
            if (!CARDS[S.ChequeTemplate]) { alert('Template Not Available'); return; }
            renderCards(cards);
        }).catch(function (e) { alert(e.message); });
    }

    function lbl(cls, x, y, back, text) {
        return '<div class="lb ' + cls + '" style="left:' + x + 'px;top:' + y + 'px;background:' + (back || '#f5f5f5') + '">' + esc(text) + '</div>';
    }

    function renderCards(cards) {
        var t = CARDS[S.ChequeTemplate], html = '';
        cards.forEach(function (c) {
            var h = '<div class="card" data-id="' + c.Id + '" data-template="' + esc(S.ChequeTemplate) + '">';
            /* z-order: Controls.Add order is top-most first, so pictureBox1 (added last) is drawn first */
            h += '<img class="pic" alt="" src="/build/img/cheque/' + t.img + '" style="left:1px;top:21px;width:429px;height:209px">';
            h += '<div class="amt" style="left:' + t.amt[0] + 'px;top:' + t.amt[1] + 'px;width:82px;height:20px;background:' + t.amt[2] + '">' + esc(c.Amount) + '</div>';
            h += lbl('pay', t.pay[0], t.pay[1], t.pay[2], c.PayeeTitle);
            DATE_KEYS.forEach(function (k, i) { h += lbl('d', t.dx[i], t.dy[i], t.dbg, c[k]); });
            h += '<label class="sel" style="left:3px;top:236px"><input type="checkbox" class="cbSelect">Select</label>';
            h += '<button type="button" class="prt" style="left:347px;top:232px;width:75px;height:23px">Print</button>';
            h += '<textarea class="words" spellcheck="false" style="left:' + t.w[0] + 'px;top:' + t.w[1] + 'px;width:' + t.w[2] + 'px;height:' + t.w[3] + 'px;background:' + t.w[4] + '">' + esc(c.AmountInWords) + '</textarea>';
            h += lbl('no', t.no[0], t.no[1], t.no[2], c.ChequeNumber);
            h += '<textarea class="typ" spellcheck="false" style="left:-1px;top:0;width:431px;height:21px">' + esc(c.CheqType) + '</textarea>';
            html += h + '</div>';
        });
        $('flowLayoutPanel1').innerHTML = html;
    }

    function cardsOfTemplate() {
        if (!CARDS[S.ChequeTemplate]) return [];
        return Array.prototype.filter.call(document.querySelectorAll('#flowLayoutPanel1 .card'), function (el) {
            return el.getAttribute('data-template') === S.ChequeTemplate;
        });
    }

    /* ------------------------------------------------------------------ btnCheckAll_Click / uncheckAll */
    function checkAll() {
        if (!CARDS[S.ChequeTemplate]) return;      // "else if ... else return" - only the 15 known templates
        var cards = cardsOfTemplate();
        if (!S.selectall) {
            S.SelectedChequesList = [];
            cards.forEach(function (el) {
                el.querySelector('.cbSelect').checked = true;
                el._isSelected = true;
                S.SelectedChequesList.push(+el.getAttribute('data-id'));
                $('SelectedCount').textContent = String(S.SelectedChequesList.length);
            });
            S.selectall = true;
            $('btnCheckAll').textContent = 'Deselect All';
            return;
        }
        uncheckAll();
    }

    function uncheckAll() {
        if (!CARDS[S.ChequeTemplate]) return;
        S.selectall = false;
        S.SelectedChequesList = [];
        cardsOfTemplate().forEach(function (el) {
            el.querySelector('.cbSelect').checked = false;
            el._isSelected = false;
            $('SelectedCount').textContent = '0';
        });
        $('btnCheckAll').textContent = 'Select All';
    }

    /* ------------------------------------------------------------------ HandleChequeSelection */
    function handleChequeSelection(el, tag) {
        var objId = +el.getAttribute('data-id');
        if (tag === 'FormCheckBox') {
            var id = 0;
            if (S.SelectedChequesList.length > 0) {
                for (var i = 0; i < S.SelectedChequesList.length; i++) if (S.SelectedChequesList[i] === objId) { id = objId; break; }
            }
            if (el._isSelected === false) {
                if (id > 0) removeFirst(id);
            } else if (id <= 0) {
                S.SelectedChequesList.push(objId);
            } else {
                removeFirst(id);
            }
            $('SelectedCount').textContent = String(S.SelectedChequesList.length);
        } else if (tag === 'ButtonClicked') {
            S.SelectedChequesList = [];
            uncheckAll();
            S.SelectedChequesList.push(objId);
            printCheque(el.querySelector('.prt'));
        }
    }

    function removeFirst(v) {
        var i = S.SelectedChequesList.indexOf(v);
        if (i >= 0) S.SelectedChequesList.splice(i, 1);
    }

    /* ------------------------------------------------------------------ PrintCheque */
    function busy(btn, on) {
        if (!btn) return;
        btn.disabled = on;
        btn.classList.toggle('btn-busy', on);
    }

    function printCheque(cardBtn) {
        if (S.printing) return;                                   // no duplicate request
        if (S.SelectedChequesList.length === 0) {
            alert('Please Select Cheque to print');
            finallyRefresh();
            return;
        }
        S.printing = true;
        var btn = $('btn294APrint');
        busy(btn, true); busy(cardBtn, true);                      // btn294APrint.Enabled = false
        fetch(API + '/print', {
            method: 'POST', credentials: 'same-origin',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ bankId: S.BankId, ids: S.SelectedChequesList.slice() })
        }).then(function (r) {
            var ct = r.headers.get('Content-Type') || '';
            if (!r.ok) return r.text().then(function (t) { throw new Error(t || ('HTTP ' + r.status)); });
            if (ct.indexOf('application/pdf') >= 0) return r.blob().then(directPrint);
            return r.json().then(function (j) { if (j && j.message) alert(j.message); });
        }).catch(function (e) {
            alert(e.message);
        }).then(function () {
            S.printing = false;
            busy(btn, false); busy(cardBtn, false);               // btn294APrint.Enabled = true
            finallyRefresh();
        });
    }

    /* finally { ChequeCountGridFill(); GetDetailData(BankId, 0); } */
    function finallyRefresh() {
        chequeCountGridFill();
        getDetailData(S.BankId);
    }

    /* Reporting.ReportDirectPrint: the PDF goes straight to the print dialog (no preview window). */
    function directPrint(blob) {
        var url = URL.createObjectURL(blob), f = $('printFrame');
        f.onload = function () {
            try { f.contentWindow.focus(); f.contentWindow.print(); }
            catch (e) { window.open(url, '_blank'); }
        };
        f.src = url;
    }

    /* ------------------------------------------------------------------ wiring */
    function wire() {
        $('btn294APrint').addEventListener('click', function () { printCheque(null); });
        $('btnCheckAll').addEventListener('click', checkAll);

        var tbody = $('grdChequeCount').tBodies[0];
        tbody.addEventListener('click', function (e) { var tr = e.target.closest('tr'); if (tr) setCurrent(tr); });
        tbody.addEventListener('dblclick', function (e) { var tr = e.target.closest('tr'); if (tr) { setCurrent(tr); gridDoubleClick(); } });
        Array.prototype.forEach.call(document.querySelectorAll('#grdChequeCount tr.flt input'), function (i) { i.addEventListener('input', renderGrid); });
        $('grdChequeCountWrap').addEventListener('keydown', function (e) {
            var rows = visibleRows(); if (!rows.length) return;
            var pos = rows.findIndex(function (r) { return +r.getAttribute('data-i') === S.cur; });
            if (e.key === 'ArrowDown') { e.preventDefault(); setCurrent(rows[Math.min(rows.length - 1, pos + 1)]); }
            else if (e.key === 'ArrowUp') { e.preventDefault(); setCurrent(rows[Math.max(0, pos - 1)]); }
        });
        $('grdNav').addEventListener('click', function (e) {
            var b = e.target.closest('button'); if (!b) return;
            var rows = visibleRows(); if (!rows.length) return;
            var pos = rows.findIndex(function (r) { return +r.getAttribute('data-i') === S.cur; });
            var n = { first: 0, prev: Math.max(0, pos - 1), next: Math.min(rows.length - 1, pos + 1), last: rows.length - 1 }[b.getAttribute('data-nav')];
            setCurrent(rows[n]);
        });

        var flow = $('flowLayoutPanel1');
        /* cbSelect: CheckedChanged (IsSelected, Tag = "FormCheckBox") then Click -> UserControlClicked */
        flow.addEventListener('change', function (e) {
            if (!e.target.classList.contains('cbSelect')) return;
            var el = e.target.closest('.card');
            if (el.getAttribute('data-template') !== S.ChequeTemplate) return;
            el._isSelected = e.target.checked;
            handleChequeSelection(el, 'FormCheckBox');
        });
        /* button1: Tag = "ButtonClicked" then UserControlClicked */
        flow.addEventListener('click', function (e) {
            var b = e.target.closest('button.prt'); if (!b) return;
            var el = b.closest('.card');
            if (el.getAttribute('data-template') !== S.ChequeTemplate) return;
            handleChequeSelection(el, 'ButtonClicked');
        });
    }

    /* ChequePrinting_Load */
    function load() {
        wire();
        chequeCountGridFill();
    }

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', load); else load();
})();
