/* ============================================================================================
 * Screen 333 "Store Purchase Register" — StorePurchaseRegister.cs (Architecture.WinApp.Inventory_Reports).
 * Server: StoreReportsAController / StoreReportsAService (desktop notes and deviations there).
 * Desktop line numbers are StorePurchaseRegister.cs unless named otherwise.
 * ============================================================================================ */
(function () {
    'use strict';
    var C = window.StoreCommon, $ = C.$id, esc = C.esc, ci = C.ci;
    var API = '/api/store/reports/store-purchase-register';

    var look = { fromDate: '', formats: { amountDecimals: 0, rateDecimals: 2 } };
    var branches = [];
    var raw = [];            // dt — what Print-458 prints (GridFill:360)

    var COMBOS = [
        ['CmbSupplier', 'suppliers'], ['CmbItemType', 'itemTypes'], ['CmbItemCategory', 'itemCategories'],
        ['CmbItemParentCategory', 'parentCategories'], ['CmbWarehouseName', 'warehouses'], ['CmbItemName', 'items']
    ];

    /* ---------------------------------------------------------------- small helpers (page-local) */
    var MON = ['Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov','Dec'];
    function ddMMMyy(v) {
        var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(String(v || ''));
        return m ? m[3] + '-' + MON[parseInt(m[2], 10) - 1] + '-' + m[1].substring(2) : '';
    }
    function fmt(v, minD, maxD) {
        var n = C.num(v);
        return n.toLocaleString('en-US', { minimumFractionDigits: minD, maximumFractionDigits: maxD });
    }
    function toInt(v) { var n = parseInt(String(v === null || v === undefined ? '' : v).trim(), 10); return isNaN(n) ? 0 : n; }
    function val(id) { var e = $(id); return e ? e.value : ''; }
    function selVal(id) { return toInt(val(id)); }

    /** CommonServices.OnlytextNumberFunction + ShortcutsEnabled=false: digits only. */
    function digitsOnly() {
        document.querySelectorAll('.rpta-int').forEach(function (el) {
            el.addEventListener('input', function () { var d = el.value.replace(/[^0-9]/g, ''); if (d !== el.value) el.value = d; });
        });
    }

    /** DDL.BindDDL(..., ZeroIndex: true) + Rows[0].Activate(): DropDownBind's "...Select Any Value..."
        row first (its value reads 0 through Conversion.ToInt), selected. */
    var SELECT_ANY = '...Select Any Value...';
    function bind(id, rows) {
        var h = '<option value="0">' + SELECT_ANY + '</option>';
        (rows || []).forEach(function (r) { h += '<option value="' + esc(ci(r, 'Id')) + '">' + esc(ci(r, 'Name')) + '</option>'; });
        $(id).innerHTML = h;
        $(id).value = '0';
    }
    function fillCombos(c) {
        if (!c || !c.bound) return;                    // ALlDropDown:236 — nothing returned, nothing rebound
        COMBOS.forEach(function (p) { bind(p[0], c[p[1]]); });
    }
    function clearCombos() {                           // cmbBranchName_Leave:842-853
        COMBOS.forEach(function (p) { $(p[0]).innerHTML = ''; });
    }

    /* ---------------------------------------------------------------- branch tick-list (BranchesFill:164) */
    function renderBranches(list, defaultId) {
        branches = list || [];
        var box = $('cmbBranchNameList');
        var h = '<label class="hdr"><input type="checkbox" id="branchAll"> </label>';
        branches.forEach(function (b) {
            h += '<label><input type="checkbox" class="rpta-br" value="' + esc(b.BranchId) + '"' +
                 (C.intOf(b.BranchId) === C.intOf(defaultId) ? ' checked' : '') + '> ' + esc(b.BranchName) + '</label>';
        });
        box.innerHTML = h;
        $('branchAll').addEventListener('change', function () {
            var on = $('branchAll').checked;
            box.querySelectorAll('.rpta-br').forEach(function (c) { c.checked = on; });
            branchText();
        });
        box.querySelectorAll('.rpta-br').forEach(function (c) { c.addEventListener('change', branchText); });
        branchText();
    }
    function tickedBranches() {
        return Array.prototype.map.call(document.querySelectorAll('#cmbBranchNameList .rpta-br:checked'), function (c) { return c.value; });
    }
    function branchText() {                            // ListSeparator ","
        var names = [];
        document.querySelectorAll('#cmbBranchNameList .rpta-br:checked').forEach(function (c) {
            var b = branches.find(function (x) { return String(x.BranchId) === c.value; });
            if (b) names.push(b.BranchName);
        });
        $('cmbBranchNameText').textContent = names.length ? names.join(',') : ' ';
    }
    /** cmbBranchName_Leave:833 — closing the list is the Leave. */
    function branchLeave() {
        if (tickedBranches().length) {
            C.getJson(API + '/combos').then(fillCombos).catch(function (e) { alert(e.message); });
        } else {
            clearCombos();
        }
    }

    /* ---------------------------------------------------------------- grid (GridFill:363 dtHistory + gridSetting:418) */
    /* Visible columns in dtHistory order; Id, DetailId, DocumentTypeId, OrderId, GrnId hidden (:422-426),
       InvParentCateDescription / CategoryDescription / TypeDescription hidden through ColumnSets (:427-429).
       Captions are the column names (Janus default). PrintWithTax and Print are added last (:473-484). */
    function cols() {
        var f = look.formats || {};
        var amt = C.intOf(f.amountDecimals), rate = C.intOf(f.rateDecimals);
        return [
            { key: 'SupplierName', src: 'SupplierName', w: 130 },
            { key: 'OrderNo', src: 'OrderNo', t: 'int', w: 50, link: 'order' },
            { key: 'OrderDate', src: 'OrderDate', t: 'date', w: 70 },
            { key: 'GrnNo', src: 'GrnNo', t: 'int', w: 40, link: 'grn' },
            { key: 'GrnDate', src: 'GrnDate', t: 'date', w: 70 },
            { key: 'GpNo', src: 'GpNo', t: 'int', w: 40 },
            { key: 'InvoiceNo', src: 'DocNo', t: 'int', w: 60, link: 'invoice' },
            { key: 'InvoiceDate', src: 'DocDate', t: 'date', w: 70 },
            { key: 'Warehouse', src: 'WareHouseName', w: 130 },
            { key: 'ItemName', src: 'ItemName', w: 130 },
            { key: 'UOM', src: 'UOMCode', w: 50 },
            { key: 'ItemQty', src: 'ItemQty', t: 'num', min: 0, max: 3, sum: true, w: 70 },
            { key: 'ItemRate', src: 'ItemRate', t: 'num', min: rate, max: rate, w: 70 },
            { key: 'ItemAmount', src: 'ItemAmount', t: 'num', min: amt, max: amt, sum: true, w: 80 },
            { key: 'EntryUser', src: 'EntryUserName', w: 70 },
            { key: 'EntryDate', src: 'EntryDate', t: 'date', w: 80 },
            { key: 'ModifyUser', src: 'ModifyUsername', w: 70 },
            { key: 'ModifyDate', src: 'ModifyDate', t: 'date', w: 80 },
            { key: 'ApprovedUser', src: 'ApprovedUserName', w: 70 },
            { key: 'PostDate', src: 'PostDate', t: 'date', w: 90 },
            { key: 'ApprovalStatus', src: 'ApprovalStatus', w: 100 },
            { key: 'NoOfAttachments', src: 'NoOfAttachments', t: 'raw', w: 90, link: 'attachments' },
            { key: 'RemarksHeader', src: 'RemarksHeader', w: 150 },
            { key: 'Print With Tax', btn: 'printWithTax', text: 'Print With Tax', w: 95 },
            { key: 'Print', btn: 'print', text: 'Print', w: 50 }
        ];
    }
    function cell(c, r) {
        var v = ci(r, c.src);
        if (c.t === 'int') return String(toInt(v));          // Conversion.ToInt — DBNull shows 0
        if (c.t === 'date') return ddMMMyy(v);
        if (c.t === 'num') return fmt(v, c.min, c.max);
        if (c.t === 'raw') return v === null || v === undefined ? '' : String(v);   // copied without Conversion
        return v === null || v === undefined ? '' : String(v);
    }
    function clearGrid() {                             // grdfrm.DataSource = null
        $('grdfrm').querySelector('thead').innerHTML = '';
        $('grdfrm').querySelector('tbody').innerHTML = '';
        $('grdfrm').querySelector('tfoot').innerHTML = '';
        $('grdfrmNavigator').innerHTML = '&nbsp;';
    }
    function render(rows) {
        var cs = cols(), t = $('grdfrm');
        t.querySelector('thead').innerHTML = '<tr>' + cs.map(function (c) {
            return '<th style="min-width:' + c.w + 'px">' + esc(c.key) + '</th>';
        }).join('') + '</tr>';
        t.querySelector('tbody').innerHTML = rows.map(function (r, i) {
            return '<tr>' + cs.map(function (c) {
                if (c.btn) return '<td><button type="button" class="win-btn-small rpta-cellbtn" data-i="' + i + '" data-btn="' + c.btn + '">' + esc(c.text) + '</button></td>';
                var text = esc(cell(c, r));
                var cls = (c.t === 'num' || c.t === 'int' || c.t === 'raw') ? ' class="num"' : '';
                if (c.link && toInt(cell(c, r)) !== 0 && (c.link !== 'attachments' || toInt(cell(c, r)) > 0)) {
                    return '<td' + cls + '><a class="rpta-link" data-i="' + i + '" data-link="' + c.link + '">' + text + '</a></td>';
                }
                return '<td' + cls + '>' + text + '</td>';
            }).join('') + '</tr>';
        }).join('');
        /* TotalRow (designer: TotalRow true, bottom) — ItemQty and ItemAmount summed (:463, :469). */
        t.querySelector('tfoot').innerHTML = '<tr>' + cs.map(function (c) {
            if (!c.sum) return '<td></td>';
            var s = rows.reduce(function (a, r) { return a + C.num(ci(r, c.src)); }, 0);
            return '<td class="num">' + esc(fmt(s, c.min, c.max)) + '</td>';
        }).join('') + '</tr>';
        t.querySelectorAll('button[data-btn]').forEach(function (b) {
            b.addEventListener('click', function () { rowButton(rows[toInt(b.getAttribute('data-i'))], b.getAttribute('data-btn')); });
        });
        t.querySelectorAll('a[data-link]').forEach(function (a) {
            a.addEventListener('click', function () { rowLink(rows[toInt(a.getAttribute('data-i'))], a.getAttribute('data-link')); });
        });
        $('grdfrmNavigator').textContent = 'Records: ' + rows.length;      // RecordNavigator
    }

    /* ---------------------------------------------------------------- printing (Crystal layout: see D1) */
    function tableHtml(rows) {
        if (!rows || !rows.length) return '<p>No rows.</p>';
        var keys = Object.keys(rows[0]);
        return '<table><thead><tr>' + keys.map(function (k) { return '<th>' + esc(k) + '</th>'; }).join('') + '</tr></thead><tbody>' +
            rows.map(function (r) { return '<tr>' + keys.map(function (k) { return '<td>' + esc(r[k]) + '</td>'; }).join('') + '</tr>'; }).join('') +
            '</tbody></table>';
    }
    function printRows(title, rows, sub) {
        var w = window.open('', '_blank');
        if (!w) { alert('Allow pop-ups to print.'); return; }
        w.document.write('<html><head><title>' + esc(title) + '</title><style>body{font-family:Verdana;font-size:10px}' +
            'table{border-collapse:collapse;margin-bottom:10px}td,th{border:1px solid #444;padding:2px 4px;white-space:nowrap}</style></head><body>' +
            '<h3>' + esc(title) + '</h3>' + tableHtml(rows) +
            (sub ? '<h4>InvRptPurchaseBillSupplierOthers</h4>' + tableHtml(sub) : '') +
            '<script>window.print()<\/script></body></html>');
        w.document.close();
    }
    function slip(kind, id, documentTypeId) {
        return C.getJson(API + '/slip/' + kind + '/' + id + C.qs({ documentTypeId: documentTypeId || '' }))
            .then(function (p) { printRows(p.template, p.rows, p.subReport); })
            .catch(function (e) { alert(e.message); });
    }
    /** The shared Crystal service (ReportPrintController /api/reports/{key}/print.pdf), rows on any failure. */
    function crystalThenRows(key, args, fallback) {
        var headers = { 'Content-Type': 'application/json' };
        var token = document.querySelector('meta[name="_csrf"]'), header = document.querySelector('meta[name="_csrf_header"]');
        if (token && header) headers[header.getAttribute('content')] = token.getAttribute('content');
        fetch('/api/reports/' + key + '/print.pdf', { method: 'POST', credentials: 'same-origin', headers: headers, body: JSON.stringify(args) })
            .then(function (r) {
                var ct = r.headers.get('Content-Type') || '';
                if (!r.ok || ct.indexOf('application/pdf') < 0) throw new Error('no pdf');
                return r.blob();
            })
            .then(function (b) { window.open(URL.createObjectURL(b), '_blank'); })
            .catch(function () { fallback(); });
    }

    /** grdfrm_ColumnButtonClick:493 */
    function rowButton(r, btn) {
        var id = toInt(ci(r, 'Id'));
        if (btn === 'print') slip('print', id);                 // PurchaseInvoicePMSlip_231
        if (btn === 'printWithTax') slip('printWithTax', id);   // PurchaseInvoiceStoreBillWithtax_238
    }
    /** grdfrm_LinkClicked:518 */
    function rowLink(r, link) {
        if (link === 'grn') slip('grn', toInt(ci(r, 'GrnId')));                        // GrnSlipReport212(GrnId)
        if (link === 'order') {                                                         // PurchaseOrderSlipReport201(OrderId)
            var orderId = toInt(ci(r, 'OrderId'));
            if (orderId === 0) { alert('No Record Found For Display'); return; }
            crystalThenRows('po-201', { id: orderId }, function () { slip('order', orderId); });
        }
        if (link === 'invoice') slip('invoice', toInt(ci(r, 'Id')), toInt(ci(r, 'DocumentTypeId'))); // 233 / 237
        if (link === 'attachments') viewAttachments(toInt(ci(r, 'Id')), toInt(ci(r, 'DocumentTypeId')));
    }

    /** CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, DocumentTypeId) → AttachmentView. */
    function viewAttachments(id, documentTypeId) {
        var q = C.qs({ id: id, documentTypeId: documentTypeId });
        C.getJson(API + '/attachments' + q).then(function (rows) {
            rows = rows || [];
            if (!rows.length) return;                    // dMSList.Count == 0 — nothing is shown
            var t = $('grdAttachments');
            t.tHead.innerHTML = '<tr><th>AttachmentName</th><th>CustomName</th><th>EntryDate</th></tr>';
            t.tBodies[0].innerHTML = rows.map(function (a) {
                return '<tr><td><a href="' + API + '/attachments/' + esc(a.Id) + q + '">' + esc(a.AttachmentName) + '</a></td><td>' +
                    esc(a.CustomName) + '</td><td>' + esc(C.gridDateTime(a.EntryDate || '', true)) + '</td></tr>';
            }).join('');
            C.openModal('dlgAttachment');
        }).catch(function (e) { alert(e.message); });
    }

    /* ---------------------------------------------------------------- toolbar */
    function show() {                                   // btnSearch_Click → GridFill:323
        raw = [];                                       // dt.Rows.Clear() (:329) — before the branch check
        var q = {
            fromDate: val('FromDate'), toDate: val('ToDate'),
            parentCategoryId: selVal('CmbItemParentCategory'), itemCategoryId: selVal('CmbItemCategory'),
            itemTypeId: selVal('CmbItemType'), itemId: selVal('CmbItemName'),
            grnNoFrom: toInt(val('txtGrnNoFrom')), grnNoTo: toInt(val('txtGrnNoTo')),
            gpNoFrom: toInt(val('GpsNoFrom')), gpNoTo: toInt(val('GpsNoTo')),
            poNoFrom: toInt(val('txtOrderNoFrm')), poNoTo: toInt(val('txtOrderNoTo')),
            supplierId: selVal('CmbSupplier'), warehouseId: selVal('CmbWarehouseName'),
            branchIds: tickedBranches().join(',')
        };
        C.getJson(API + C.qs(q)).then(function (rows) {
            raw = rows || [];
            if (raw.length) render(raw); else clearGrid();
        }).catch(function (e) {
            if (/Select Branch First/.test(e.message)) $('cmbBranchName').open = true;   // cmbBranchName.Focus()
            alert(e.message);
        });
    }
    function newForm() {                                // Reset:292
        $('FromDate').value = look.fromDate || '';
        $('ToDate').value = C.today();
        ['CmbItemCategory', 'CmbItemName', 'CmbItemParentCategory', 'CmbItemType'].forEach(function (id) {
            $(id).selectedIndex = -1;                   // .Text = string.Empty — no row, value reads 0
        });
        clearGrid();
        $('FromDate').focus();
    }
    function refresh() {                                // btnRefresh_Click:552 — BranchesFill + ALlDropDown
        C.getJson(API + '/refresh').then(function (d) {
            renderBranches(d.branches, d.defaultBranchId);
            fillCombos(d.combos);
        }).catch(function (e) { alert(e.message); });
    }
    function printRegister() {                          // BtnPrintRegister_Click:595
        if (!raw.length) { alert('Record Not Found For Display'); return; }
        printRows('458-StorePurchaseRegister.rpt', raw);
    }

    function gotoHistory() {                            // footer History button — the grid section
        $('historySection').scrollIntoView({ behavior: 'smooth', block: 'start' });
    }

    function init() {                                   // frmGatePassReport_Load:142
        digitsOnly();
        var det = $('cmbBranchName');
        det.addEventListener('toggle', function () { if (!det.open) branchLeave(); });
        document.addEventListener('click', function (e) { if (det.open && !det.contains(e.target)) det.open = false; });
        C.getJson(API + '/lookups').then(function (d) {
            look = d;
            $('FromDate').value = d.fromDate || '';
            $('ToDate').value = d.toDate || C.today();
            renderBranches(d.branches, d.defaultBranchId);
            fillCombos(d.combos);
            $('FromDate').focus();
        }).catch(function (e) { alert(e.message); });
    }

    window.RptSPR = { show: show, newForm: newForm, refresh: refresh, printRegister: printRegister, gotoHistory: gotoHistory };
    document.addEventListener('DOMContentLoaded', init);
})();
