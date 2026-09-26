/* ============================================================================================
 * 458 "Store Issuance Report" — IssuanceHistory.cs, ScreenName StoreIssuenceHistory.
 * Filters → Sp_InvGsStoreIssuanceHeader_SlipandRegister; grid columns, captions, widths, formats
 * and totals as gridSetting() (IssuanceHistory.cs:304) sets them; row Print (452 slip) and
 * Voucher (118) buttons as grdfrm_ColumnButtonClick (:408) / grdfrm_KeyDown Ctrl+Space (:605).
 * Layout: designer geometry 1:1 (store_issuance_history.html).
 * ============================================================================================ */
(function () {
    'use strict';
    var C = window.StoreCommon, $id = C.$id, esc = C.esc, ci = C.ci;
    var API = '/api/store/reports/store-issuance-history';
    var look = { financialYearStart: null };
    var dtGrid = [];        // the last search's raw rows (IssuanceHistory.dtGrid) — what the Print dropdown pushes
    var gridRows = [];      // the projected DataTable (:261)
    var filters = {};

    /* ------------------------------------------------------------------ number formats */
    function fmt(v, maxDec) {
        var n = C.num(v);
        return n.toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: maxDec });
    }
    /** Convert.ToInt32(decimal): round half to even — the grid's Qty column is typed int (:272). */
    function toInt32(v) {
        var n = C.num(v), f = Math.floor(n), d = n - f;
        if (Math.abs(d - 0.5) < 1e-9) return (f % 2 === 0) ? f : f + 1;
        return Math.round(n);
    }
    /** Conversion.ToInt(text) — Convert.ToInt32 inside try/catch: anything that does not fit an
        Int32 (e.g. an 11-digit Doc No) is 0, i.e. no filter. */
    function convToInt(s) {
        var t = String(s || '').trim();
        if (!/^\d+$/.test(t)) return 0;
        var n = parseInt(t, 10);
        return n > 2147483647 ? 0 : n;
    }

    /* The visible columns, in DataTable order (:261), with Id / DocumentTypeId / VoucherHeadId /
       IssuanceType hidden (:308). Captions are the column names (RetrieveStructure); widths as
       gridSetting (:316-:335); Print / Voucher appended as button columns (:346 / :352). */
    var COLS = [
        { key: 'DocType', w: 60 },
        { key: 'DocNo', w: 60, align: 'center' },   // int column → centred by GridColumnSettings
        { key: 'DocDate', w: 90, date: 'd' },         // :313 "dd-MMM-yyyy"
        { key: 'Warehouse', w: 130 },
        { key: 'ItemName', w: 130 },
        { key: 'UOM', w: 70 },
        { key: 'ItemCondition', w: 100 },            // Constants.InventoryConstants.StringConstant (value not in source)
        { key: 'Qty', w: 70, num: 3, total: 2 },      // :336 Sum, "#,##0.###", total "#,##0.##"
        { key: 'Rate', w: 85, num: 3 },               // :340
        { key: 'Amount', w: 100, num: 3, total: 3 },  // :342
        { key: 'Department', w: 120 },
        { key: 'AssetName', w: 130 },
        { key: 'DebitAccount', w: 130 },
        { key: 'InvoiceNo', w: 110 },
        { key: 'EntryUser', w: 110 },
        { key: 'EntryDate', w: 145, date: 'dt' },     // :314 "dd-MMM-yyyy hh:mm tt"
        { key: 'ModifyUser', w: 110 },
        { key: 'ModifyDate', w: 145, date: 'dt' },
        { key: 'RemarksDetail', w: 150 },
        { key: 'Print', w: 40, button: 'Print' },     // :346
        { key: 'Voucher', w: 60, button: 'Voucher' }  // :352
    ];

    /* ------------------------------------------------------------------ load */
    function init() {
        onlyDigits('txtFromDocNo'); onlyDigits('txtDocNoTo');                          // :451 / :463
        /* frmGatePassReport_Load (:222): FromDate.Focus(); From = ActiveYr.Start_Period, To = now. */
        $id('FromDate').focus();
        $id('ToDate').value = C.today();
        C.getJson(API + '/lookups').then(function (d) {
            look = d || {};
            bindCombos(look.combos || {});
            $id('FromDate').value = C.isoDay(look.financialYearStart) || '';
        }).catch(function (e) { alert(e.message); });
        document.addEventListener('keydown', formKeyDown);
        render();
    }

    /** IssuanceHistory_KeyDown (:498) — KeyPreview is on, so these work from any control. */
    function formKeyDown(e) {
        if (document.querySelector('.cx-modal.is-open')) return;
        if (e.key === 'Enter' && !e.ctrlKey && !e.altKey) {                           // :502 SendKeys {TAB}
            var t = e.target;
            if (t && (t.tagName === 'INPUT' || t.tagName === 'SELECT') && t.closest('.rc-gb')) {
                e.preventDefault(); focusNext(t);
            }
            return;
        }
        if (e.ctrlKey && e.altKey && (e.key === 'Control' || e.key === 'Alt')) { shortcutKeys(); return; }  // :534
        if (!e.ctrlKey || e.altKey) return;
        var k = (e.key || '').toLowerCase();
        if (k === 'p') { e.preventDefault(); $id('btnReset').focus(); }                 // :506 toolStrip1.Focus()
        else if (k === 'n') { e.preventDefault(); reset(); }                            // :510
        else if (k === 'r') { e.preventDefault(); refresh(); }                          // :514
        else if (e.key === 'F5') { e.preventDefault(); $id('FromDate').focus(); }       // :522
        else if (k === 's') { e.preventDefault(); show(); }                             // :526
        else if (e.key === 'ArrowDown') { e.preventDefault(); focusGrid(); }            // :530
    }
    function focusNext(el) {
        var list = Array.prototype.slice.call(document.querySelectorAll('.rc-gb input, .rc-gb select, .rc-gb button'));
        var i = list.indexOf(el);
        if (i >= 0 && i + 1 < list.length) list[i + 1].focus();
    }
    function focusGrid() {
        var b = document.querySelector('#grdfrm tbody button');
        if (b) b.focus(); else $id('grdfrm').scrollIntoView({ block: 'nearest' });
    }

    function onlyDigits(id) {
        $id(id).addEventListener('input', function () {
            var v = this.value.replace(/\D/g, '');
            if (v !== this.value) this.value = v;
        });
    }

    /** FillAllDropDowns (:114): one combo per Activity; a group missing from the result leaves
        its combo untouched, and an empty result binds nothing (:127). DropDownBind.BindDDL with
        ZeroIndex false (:172): no "...Select Any Value..." row, the combo starts with empty text
        (the blank option, value 0 — Conversion.ToInt(null) = 0). */
    var MAP = { DepartmentFrom: 'CmbDepartmentName', Item: 'CmbItemName', Asset: 'CmbAssetName',
                Warehouse: 'CmbWareHouse', DebitAccount: 'CmbAccountTitle', ItemCondition: 'cmbItemCondition' };
    function bindCombos(combos) {
        Object.keys(MAP).forEach(function (act) {
            if (!combos[act]) return;
            C.fillSelect(MAP[act], combos[act], 'Id', 'Name');
        });
        /* a combo that never got a source still needs its empty text row */
        Object.keys(MAP).forEach(function (act) {
            var el = $id(MAP[act]);
            if (!el.options.length) el.innerHTML = '<option value="0"></option>';
        });
    }

    function comboInt(id) { return C.intOf($id(id).value); }

    /* ------------------------------------------------------------------ toolbar */
    /** Reset (:200) — Item Condition is NOT cleared on the desktop, and dtGrid is not cleared
        either (only grdfrm.DataSource = null), so Print still prints the last search. */
    function reset() {
        $id('FromDate').focus();
        $id('FromDate').value = C.isoDay(look.financialYearStart) || '';
        $id('ToDate').value = C.today();
        ['CmbWareHouse', 'CmbAssetName', 'CmbItemName', 'CmbDepartmentName', 'CmbAccountTitle'].forEach(function (id) {
            $id(id).value = '0';
        });
        $id('txtFromDocNo').value = '';
        $id('txtDocNoTo').value = '';
        gridRows = [];
        filters = {};
        render();
    }

    /** btnRefresh_Click (:396). */
    function refresh() {
        C.getJson(API + '/refresh').then(function (d) { bindCombos((d && d.combos) || {}); })
            .catch(function (e) { alert(e.message); });
    }

    /** btnSearch_Click → GridFill (:238). */
    function show() {
        var q = {
            fromDate: $id('FromDate').value, toDate: $id('ToDate').value,
            departmentId: comboInt('CmbDepartmentName'), itemId: comboInt('CmbItemName'),
            assetId: comboInt('CmbAssetName'), warehouseId: comboInt('CmbWareHouse'),
            accountId: comboInt('CmbAccountTitle'), itemConditionId: comboInt('cmbItemCondition'),
            fromDocNo: convToInt($id('txtFromDocNo').value), toDocNo: convToInt($id('txtDocNoTo').value)   // :256 / :257
        };
        C.getJson(API + '/search' + C.qs(q)).then(function (rows) {
            dtGrid = rows || [];
            if (!dtGrid.length) {                                                        // :295
                gridRows = [];
                filters = {};
                render();
                alert('Record Not found For Display');
                return;
            }
            gridRows = dtGrid.map(function (r) {                                         // :288
                return {
                    Id: ci(r, 'Id'), DocumentTypeId: ci(r, 'DocumentTypeId'), DocType: ci(r, 'DocumentTypeCode'),
                    VoucherHeadId: ci(r, 'VoucherHeadId'), DocNo: ci(r, 'DocNo'), DocDate: ci(r, 'DocDate'),
                    Warehouse: ci(r, 'WareHouseName'), ItemName: ci(r, 'ItemName'), UOM: ci(r, 'UOMCode'),
                    ItemCondition: ci(r, 'ItemCondition'), Qty: toInt32(ci(r, 'IssueQty')),
                    Rate: ci(r, 'ItemRate'), Amount: ci(r, 'ItemAmount'), Department: ci(r, 'FromDepartmentName'),
                    AssetName: ci(r, 'AssetName'), DebitAccount: ci(r, 'AccountTitle'), IssuanceType: ci(r, 'IssuanceType'),
                    InvoiceNo: ci(r, 'ExportInvoiceNoDetail'), EntryUser: ci(r, 'EntryUser'), EntryDate: ci(r, 'EntryDate'),
                    ModifyUser: ci(r, 'ModifyUser'), ModifyDate: ci(r, 'ModifyDate'), RemarksDetail: ci(r, 'ReamarksDetail')
                };
            });
            filters = {};
            render();
        }).catch(function (e) { alert(e.message); });
    }

    /** btnShortCutKey_Click → MakeShortCutKeys (:581). */
    function shortcutKeys() {
        C.pickFrom('ShortCut Keys', [
            { KeyCombination: 'Ctrl+E', Description: 'For Close' },
            { KeyCombination: 'Ctrl+N', Description: 'For New' },
            { KeyCombination: 'Ctrl+R', Description: 'For Refresh' },
            { KeyCombination: 'Ctrl+S', Description: 'For Showing Data' },
            { KeyCombination: 'Ctrl+F5', Description: 'For Focus on From Date' },
            { KeyCombination: 'Ctrl+ArrowDown', Description: 'For Focus On Grid' },
            { KeyCombination: 'Ctrl+alt', Description: 'To Show ShortCut Keys Form' }
        ], [{ key: 'KeyCombination', caption: 'KeyCombination' }, { key: 'Description', caption: 'Description' }]);
    }

    /** The footer's History button — the grid is on the same screen, under the filters. */
    function gotoHistory() {
        $id('historySection').scrollIntoView({ behavior: 'smooth', block: 'start' });
        focusGrid();
    }

    /* ------------------------------------------------------------------ grid */
    function cellText(c, r) {
        var v = r[c.key];
        if (c.button) return c.button;
        if (c.date === 'd') return v ? C.gridDate(v) : '';
        if (c.date === 'dt') return v ? C.gridDateTime(v, true) : '';
        if (c.num) return (v === null || v === undefined || v === '') ? '' : fmt(v, c.num);
        return v === null || v === undefined ? '' : String(v);
    }
    function visibleRows() {
        return gridRows.filter(function (r) {
            return COLS.every(function (c) {
                var f = filters[c.key];
                if (!f || c.button) return true;
                return cellText(c, r).toLowerCase().indexOf(f.toLowerCase()) >= 0;   // DefaultFilterRowComparison Contains
            });
        });
    }
    function render() {
        var t = $id('grdfrm');
        if (!gridRows.length) {                                                          // ClearStructure / DataSource = null
            t.querySelector('thead').innerHTML = ''; t.querySelector('tbody').innerHTML = ''; t.querySelector('tfoot').innerHTML = '';
            $id('grdfrmNavigator').innerHTML = '&nbsp;';
            return;
        }
        t.querySelector('thead').innerHTML =
            '<tr>' + COLS.map(function (c) { return '<th style="width:' + c.w + 'px">' + esc(c.key) + '</th>'; }).join('') + '</tr>' +
            '<tr class="cx-filter-row">' + COLS.map(function (c) {
                return c.button ? '<th></th>' : '<th><input type="text" class="win-textbox" data-f="' + esc(c.key) +
                    '" value="' + esc(filters[c.key] || '') + '"></th>';
            }).join('') + '</tr>';
        t.querySelectorAll('thead input[data-f]').forEach(function (inp) {
            inp.oninput = function () { filters[inp.getAttribute('data-f')] = inp.value; body(); };
        });
        body();
    }
    function body() {
        var t = $id('grdfrm'), rows = visibleRows();
        t.querySelector('tbody').innerHTML = rows.map(function (r) {
            return '<tr>' + COLS.map(function (c) {
                if (c.button) {
                    return '<td style="text-align:center"><button type="button" class="win-btn-small" data-b="' + c.key + '" data-i="' + gridRows.indexOf(r) + '">' + c.button + '</button></td>';
                }
                var txt = cellText(c, r);
                return '<td' + (c.num ? ' class="num" style="text-align:right"' : (c.align ? ' style="text-align:center"' : '')) +
                    ' title="' + esc(txt) + '">' + esc(txt) + '</td>';
            }).join('') + '</tr>';
        }).join('');
        t.querySelector('tfoot').innerHTML = '<tr>' + COLS.map(function (c) {
            if (!c.total) return '<td></td>';
            var s = rows.reduce(function (a, r) { return a + C.num(r[c.key]); }, 0);
            return '<td class="num" style="text-align:right"><b>' + esc(fmt(s, c.total)) + '</b></td>';
        }).join('') + '</tr>';
        t.querySelectorAll('tbody button[data-b]').forEach(function (b) {
            var r = gridRows[parseInt(b.getAttribute('data-i'), 10)];
            b.onclick = function () {
                if (b._ctrlSpace) { b._ctrlSpace = false; return; }
                columnButton(b.getAttribute('data-b'), r);
            };
            b.onkeydown = function (e) {                                                  // grdfrm_KeyDown Ctrl+Space (:605)
                if (e.ctrlKey && (e.key === ' ' || e.code === 'Space')) {
                    e.preventDefault(); b._ctrlSpace = true;
                    setTimeout(function () { b._ctrlSpace = false; }, 400);
                    columnButton(b.getAttribute('data-b'), r);
                }
            };
        });
        $id('grdfrmNavigator').textContent = 'Rows: ' + rows.length + (rows.length !== gridRows.length ? ' of ' + gridRows.length : '');
    }

    /** grdfrm_ColumnButtonClick (:408) / grdfrm_KeyDown Ctrl+Space (:605). */
    function columnButton(key, r) {
        if (key === 'Voucher') {
            var vh = C.intOf(r.VoucherHeadId), dt = C.intOf(r.DocumentTypeId);
            if (vh === 0) { alert('VoucherId Not Found'); return; }                   // CommonServices.cs:5658
            crystalOrRows('acc-118', { id: vh, documentTypeId: dt },
                API + '/voucher' + C.qs({ voucherHeadId: vh, documentTypeId: dt }), '118-AcRptVoucherSlip');
        } else if (key === 'Print') {
            var id = C.intOf(r.Id);
            crystalOrRows('452-rptinvgsstoreissuanceheader-slip', { id: id },
                API + '/slip' + C.qs({ id: id }), '452-RptInvGsStoreIssuanceHeader_Slip');
        }
    }

    /** tsPrintDropDown_DropDownItemClicked (:475) — pushes dtGrid (the raw rows) into the chosen .rpt.
        The .rpt list is the desktop's local Store folder; here the rows themselves are printed. */
    function printRegister() {
        if (!dtGrid.length) { alert('Record Not Found For Display'); return; }
        printRows(dtGrid, 'Store Issuance History');
    }

    /* ------------------------------------------------------------------ printing
       The project's Crystal endpoint renders the real .rpt when it is present and enabled; when it
       is not (404 / 503 / not a PDF), the report's own procedure rows are printed instead. */
    function crystalOrRows(key, args, rowsUrl, title) {
        var headers = { 'Content-Type': 'application/json', Accept: 'application/pdf' };
        var token = document.querySelector('meta[name="_csrf"]'), header = document.querySelector('meta[name="_csrf_header"]');
        if (token && header) headers[header.getAttribute('content')] = token.getAttribute('content');
        C.getJson(rowsUrl).then(function (rows) {
            if (!rows || !rows.length) { alert('No Record Found For Display'); return; }
            return fetch('/api/reports/' + encodeURIComponent(key) + '/print.pdf', {
                method: 'POST', credentials: 'same-origin', headers: headers, body: JSON.stringify(args)
            }).then(function (res) {
                var ct = res.headers.get('Content-Type') || '';
                if (res.ok && ct.indexOf('application/pdf') >= 0) {
                    return res.blob().then(function (b) { window.open(URL.createObjectURL(b), '_blank'); });
                }
                printRows(rows, title);
            }, function () { printRows(rows, title); });
        }).catch(function (e) { alert(e.message); });
    }

    function printRows(rows, title) {
        var cols = Object.keys(rows[0]);
        var w = window.open('', '_blank');
        if (!w) { alert('Allow pop-ups to print.'); return; }
        w.document.write('<html><head><title>' + esc(title) + '</title><style>body{font-family:Verdana;font-size:10px}' +
            'table{border-collapse:collapse}td,th{border:1px solid #444;padding:2px 4px;white-space:nowrap}</style></head><body>' +
            '<h3>' + esc(title) + '</h3><table><thead><tr>' + cols.map(function (c) { return '<th>' + esc(c) + '</th>'; }).join('') +
            '</tr></thead><tbody>' + rows.map(function (r) {
                return '<tr>' + cols.map(function (c) { return '<td>' + esc(r[c]) + '</td>'; }).join('') + '</tr>';
            }).join('') + '</tbody></table><script>window.print()<\/script></body></html>');
        w.document.close();
    }

    document.addEventListener('DOMContentLoaded', init);
    window.RptIssuance = { show: show, reset: reset, refresh: refresh, printRegister: printRegister,
                           shortcutKeys: shortcutKeys, gotoHistory: gotoHistory };
})();
