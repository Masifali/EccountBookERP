/* ============================================================================================
 * Screen 453 "Store Issuance Return Report" — StoreIssuanceReturnRegister.cs
 * (Architecture.WinApp.Inventory_Reports). Server: StoreReportsAController / StoreReportsAService
 * (desktop notes 11-14 and deviations there). Line numbers are StoreIssuanceReturnRegister.cs.
 * ============================================================================================ */
(function () {
    'use strict';
    var C = window.StoreCommon, $ = C.$id, esc = C.esc, ci = C.ci;
    var API = '/api/store/reports/store-issuance-return-register';

    var look = { fromDate: '', formats: { amountDecimals: 0, rateDecimals: 2 } };
    var dt = [];              // what the Print list prints (GridFill:309)

    var COMBOS = [
        ['CmbDepartmentName', 'departments'], ['CmbItemName', 'items'], ['CmbAssetName', 'assets'],
        ['CmbWareHouse', 'warehouses'], ['CmbAccountTitle', 'accounts']
    ];

    /* ---------------------------------------------------------------- helpers (page-local) */
    var MON = ['Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov','Dec'];
    function shortDate(v) {
        var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(String(v || ''));
        return m ? m[3] + '-' + MON[parseInt(m[2], 10) - 1] + '-' + m[1] : '';
    }
    function dateTime(v) {
        var m = /^(\d{4})-(\d{2})-(\d{2})[T ](\d{2}):(\d{2})/.exec(String(v || ''));
        if (!m) return shortDate(v);
        var h = parseInt(m[4], 10), ap = h >= 12 ? 'PM' : 'AM'; h = h % 12; if (h === 0) h = 12;
        return shortDate(v) + ' ' + String(h).padStart(2, '0') + ':' + m[5] + ' ' + ap;
    }
    function fmt(v, minD, maxD) { return C.num(v).toLocaleString('en-US', { minimumFractionDigits: minD, maximumFractionDigits: maxD }); }
    function toInt(v) { var n = parseInt(String(v === null || v === undefined ? '' : v).trim(), 10); return isNaN(n) ? 0 : n; }
    function val(id) { var e = $(id); return e ? e.value : ''; }
    function selVal(id) { return toInt(val(id)); }

    function digitsOnly() {                            // OnlytextNumberFunction
        document.querySelectorAll('.rpta-int').forEach(function (el) {
            el.addEventListener('input', function () { var d = el.value.replace(/[^0-9]/g, ''); if (d !== el.value) el.value = d; });
        });
    }

    /** BindDDL / BindDDLNew with ZeroIndex false: no "...Select Any Value..." row; nothing is selected, the
        text is empty (the blank option stands for that and reads 0). */
    function bind(id, rows) {
        var h = '<option value="0"></option>';
        (rows || []).forEach(function (r) { h += '<option value="' + esc(ci(r, 'Id')) + '">' + esc(ci(r, 'Name')) + '</option>'; });
        $(id).innerHTML = h;
        $(id).value = '0';
    }
    function fillCombos(c) {
        /* each fill binds only when its call returned rows (DepartmentNameFill:124 etc.); CreditAcDetailFill
           empties the Account Title combo when nothing came back (:220). */
        COMBOS.forEach(function (p) {
            var rows = c[p[1]] || [];
            if (rows.length) bind(p[0], rows);
            else if (p[0] === 'CmbAccountTitle') $(p[0]).innerHTML = '';
        });
    }

    /* ---------------------------------------------------------------- grid (GridFill:312 dtHistory + gridSetting:352) */
    function cols() {
        var f = look.formats || {}, amt = C.intOf(f.amountDecimals), rate = C.intOf(f.rateDecimals);
        return [
            { key: 'Print', btn: 'Print', w: 40 },             // Position 0, frozen
            { key: 'Voucher', btn: 'Voucher', w: 60 },         // Position 1, frozen (FrozenColumns = 2)
            /* widths: GridEX_Helper.GridWrappingAndColumnSettings(grdfrm, 2, 3) defaults (int 60, DateTime 80,
               double 90, *name* 150, other 100; int centred; header 2 lines), then gridSetting's own. */
            { key: 'DocDate', src: 'DocDate', t: 'date', w: 80 },
            { key: 'DocNo', src: 'DocNo', t: 'int', w: 60 },
            { key: 'ItemName', src: 'ItemName', w: 250 },
            { key: 'ItemCondition', src: 'ItemCondition', w: 100 },
            { key: 'UOMCode', src: 'UOMCode' },
            { key: 'ItemQty', src: 'ItemQty', t: 'num', min: 0, max: 3, sum: true, w: 90 },
            { key: 'ItemRate', src: 'ItemRate', t: 'num', min: rate, max: rate, w: 90 },
            { key: 'ItemAmount', src: 'ItemAmount', t: 'num', min: amt, max: amt, sum: true, w: 90 },
            { key: 'DepartmentName', src: 'DepartmentName', w: 200 },
            { key: 'AssetName', src: 'AssetName', w: 200 },
            { key: 'RemarksDetail', src: 'ReamarksDetail', w: 100 },
            { key: 'WareHouseName', src: 'WareHouseName', w: 150 },
            { key: 'AccountTitle', src: 'AccountTitle', w: 250 },
            { key: 'EntryDate', src: 'EntryDate', t: 'datetime', w: 80 },
            { key: 'UserName', src: 'UserName', w: 150 },
            { key: 'NoOfAttachments', src: 'NoOfAttachments', t: 'int', w: 60, link: true }
        ];
    }
    function cell(c, r) {
        var v = ci(r, c.src);
        if (c.t === 'date') return shortDate(v);
        if (c.t === 'datetime') return dateTime(v);
        if (c.t === 'num') return fmt(v, c.min, c.max);
        if (c.t === 'int') return v === null || v === undefined || v === '' ? '' : String(toInt(v));
        return v === null || v === undefined ? '' : String(v);
    }
    function clearGrid() {                             // grdfrm.ClearStructure() / DataSource = null
        var t = $('grdfrm');
        t.querySelector('thead').innerHTML = ''; t.querySelector('tbody').innerHTML = ''; t.querySelector('tfoot').innerHTML = '';
        $('grdfrmNavigator').innerHTML = '&nbsp;';
    }
    /* FrozenColumns = 2 (Print, Voucher); the second column's offset is set from the first's width after render. */
    function frozen(i) { return i < 2 ? ' class="rpta-frozen" data-frz="' + i + '" style="left:0"' : ''; }
    function render(rows) {
        var cs = cols(), t = $('grdfrm');
        t.querySelector('thead').innerHTML = '<tr>' + cs.map(function (c, k) {
            return '<th' + frozen(k) + (c.w ? ' data-w="' + c.w + '"' : '') + '>' + esc(c.key) + '</th>';
        }).join('') + '</tr>';
        t.querySelectorAll('thead th[data-w]').forEach(function (th) { th.style.minWidth = th.getAttribute('data-w') + 'px'; });
        t.querySelector('tbody').innerHTML = rows.map(function (r, i) {
            return '<tr>' + cs.map(function (c, k) {
                if (c.btn) return '<td' + frozen(k) + '><button type="button" class="win-btn-small rpta-cellbtn" data-i="' + i + '" data-btn="' + c.btn + '">' + esc(c.btn) + '</button></td>';
                var cls = c.t === 'num' ? ' class="num"' : (c.t === 'int' ? ' class="c"' : '');
                if (c.link && toInt(cell(c, r)) > 0) {       // ColumnType Link → LinkClicked:550
                    return '<td' + cls + '><a class="rpta-link" data-att="' + i + '">' + esc(cell(c, r)) + '</a></td>';
                }
                return '<td' + cls + '>' + esc(cell(c, r)) + '</td>';
            }).join('') + '</tr>';
        }).join('');
        t.querySelector('tfoot').innerHTML = '<tr>' + cs.map(function (c, k) {   // AggregateFunction Sum (:366, :372)
            if (!c.sum) return '<td' + frozen(k) + '></td>';
            return '<td class="num">' + esc(fmt(rows.reduce(function (a, r) { return a + C.num(ci(r, c.src)); }, 0), c.min, c.max)) + '</td>';
        }).join('') + '</tr>';
        t.querySelectorAll('button[data-btn]').forEach(function (b) {
            b.addEventListener('click', function () { columnButton(rows[toInt(b.getAttribute('data-i'))], b.getAttribute('data-btn')); });
        });
        t.querySelectorAll('a[data-att]').forEach(function (a) {
            a.addEventListener('click', function () {
                var r = rows[toInt(a.getAttribute('data-att'))];
                viewAttachments(toInt(ci(r, 'Id')), toInt(ci(r, 'DocumentTypeId')));
            });
        });
        var first = t.querySelector('thead th[data-frz="0"]'), w = first ? first.offsetWidth : 0;
        t.querySelectorAll('[data-frz="1"]').forEach(function (c) { c.style.left = w + 'px'; });
        $('grdfrmNavigator').textContent = 'Records: ' + rows.length;      // RecordNavigator
    }

    /** CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, DocumentTypeId) → AttachmentView. */
    function viewAttachments(id, documentTypeId) {
        var q = C.qs({ id: id, documentTypeId: documentTypeId });
        C.getJson(API + '/attachments' + q).then(function (rows) {
            rows = rows || [];
            if (!rows.length) return;
            var t = $('grdAttachments');
            t.tHead.innerHTML = '<tr><th>AttachmentName</th><th>CustomName</th><th>EntryDate</th></tr>';
            t.tBodies[0].innerHTML = rows.map(function (a) {
                return '<tr><td><a href="' + API + '/attachments/' + esc(a.Id) + q + '">' + esc(a.AttachmentName) + '</a></td><td>' +
                    esc(a.CustomName) + '</td><td>' + esc(C.gridDateTime(a.EntryDate || '', true)) + '</td></tr>';
            }).join('');
            C.openModal('dlgAttachment');
        }).catch(function (e) { alert(e.message); });
    }

    /* ---------------------------------------------------------------- printing (Crystal layout: D1 / D2) */
    function tableHtml(rows) {
        if (!rows || !rows.length) return '<p>No rows.</p>';
        var keys = Object.keys(rows[0]);
        return '<table><thead><tr>' + keys.map(function (k) { return '<th>' + esc(k) + '</th>'; }).join('') + '</tr></thead><tbody>' +
            rows.map(function (r) { return '<tr>' + keys.map(function (k) { return '<td>' + esc(r[k]) + '</td>'; }).join('') + '</tr>'; }).join('') +
            '</tbody></table>';
    }
    function printRows(title, rows) {
        var w = window.open('', '_blank');
        if (!w) { alert('Allow pop-ups to print.'); return; }
        w.document.write('<html><head><title>' + esc(title) + '</title><style>body{font-family:Verdana;font-size:10px}' +
            'table{border-collapse:collapse}td,th{border:1px solid #444;padding:2px 4px;white-space:nowrap}</style></head><body>' +
            '<h3>' + esc(title) + '</h3>' + tableHtml(rows) + '<script>window.print()<\/script></body></html>');
        w.document.close();
    }

    /** grdfrm_ColumnButtonClick:441 */
    function columnButton(r, key) {
        if (key === 'Voucher') {                          // CommonServices.VoucherReport_118
            C.getJson(API + '/voucher' + C.qs({ voucherHeadId: toInt(ci(r, 'VoucherHeadId')), documentTypeId: toInt(ci(r, 'DocumentTypeId')) }))
                .then(function (p) { printRows(p.template, p.rows); }).catch(function (e) { alert(e.message); });
        }
        if (key === 'Print') {                            // register by Id → 457-StoreIssuanceReturnSlip.rpt
            C.getJson(API + '/slip/' + toInt(ci(r, 'Id')))
                .then(function (p) { printRows(p.template, p.rows); }).catch(function (e) { alert(e.message); });
        }
    }

    /* ---------------------------------------------------------------- toolbar */
    function show() {                                   // btnSearch_Click → GridFill:290
        var q = {
            fromDate: val('FromDate'), toDate: val('ToDate'),
            departmentId: selVal('CmbDepartmentName'), itemId: selVal('CmbItemName'), assetId: selVal('CmbAssetName'),
            warehouseId: selVal('CmbWareHouse'), accountId: selVal('CmbAccountTitle'),
            fromDocNo: toInt(val('txtFromDocNo')), toDocNo: toInt(val('txtDocNoTo'))
        };
        C.getJson(API + C.qs(q)).then(function (res) {
            dt = (res && res.rows) || [];
            if (dt.length) { render(dt); return; }
            clearGrid();
            alert(res.message || 'Record Not found For Display');
        }).catch(function (e) { alert(e.message); });
    }
    function newForm() {                                // Reset:248 — dt is not cleared
        $('FromDate').value = look.fromDate || '';
        $('ToDate').value = C.today();
        ['CmbWareHouse', 'CmbAssetName', 'CmbItemName', 'CmbDepartmentName', 'CmbAccountTitle'].forEach(function (id) {
            $(id).selectedIndex = -1;                   // .Text = string.Empty — value reads 0
        });
        $('txtFromDocNo').value = '';
        $('txtDocNoTo').value = '';
        clearGrid();
        $('FromDate').focus();
    }
    function refresh() {                                // toolStripButton1_Click:568
        C.getJson(API + '/combos').then(fillCombos).catch(function (e) { alert(e.message); });
    }
    function printRegister() {                          // tsPrintDropDown_DropDownItemClicked:508 (see D2)
        if (!dt.length) { alert('Record Not Found For Display'); return; }
        printRows('Store Issuance Return Register', dt);
    }

    function gotoHistory() {                            // footer History button — the grid section
        $('historySection').scrollIntoView({ behavior: 'smooth', block: 'start' });
    }

    function init() {                                   // frmGatePassReport_Load:270
        digitsOnly();
        C.getJson(API + '/lookups').then(function (d) {
            look = d;
            $('FromDate').value = d.fromDate || '';
            $('ToDate').value = d.toDate || C.today();
            fillCombos(d);
            $('FromDate').focus();
        }).catch(function (e) { alert(e.message); });
    }

    window.RptSRR = { show: show, newForm: newForm, refresh: refresh, printRegister: printRegister, gotoHistory: gotoHistory };
    document.addEventListener('DOMContentLoaded', init);
})();
