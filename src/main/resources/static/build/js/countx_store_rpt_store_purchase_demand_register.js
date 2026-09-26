/* ============================================================================================
 * Screen 452 "Store Purchase Demand Report" — StorePurchaseDemandRegister.cs
 * (Architecture.WinApp.Inventory_Reports). Server: StoreReportsAController / StoreReportsAService
 * (desktop notes 5-10 and deviations there). Line numbers are StorePurchaseDemandRegister.cs.
 * ============================================================================================ */
(function () {
    'use strict';
    var C = window.StoreCommon, $ = C.$id, esc = C.esc, ci = C.ci;
    var API = '/api/store/reports/store-purchase-demand-register';

    var look = { financialYearStart: '' };
    var dtGrid = [];            // what 456-Register prints (gridHisory:483)
    var approvedAtFill = false; // GridSettings:655 — Complete / Cancel exist only for an "Approved" fill

    var COMBOS = [
        ['CmbParentCategory', 'parentCategories'], ['CmbItemCategory', 'itemCategories'], ['CmbItemType', 'itemTypes'],
        ['CmbJobLotName', 'jobLots'], ['CmbItem', 'items'], ['CmbDepartment', 'departments'], ['cmbAssetsRef', 'assets']
    ];

    /* ---------------------------------------------------------------- helpers (page-local) */
    var MON = ['Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov','Dec'];
    function ddMMMyy(v) {
        var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(String(v || ''));
        return m ? m[3] + '-' + MON[parseInt(m[2], 10) - 1] + '-' + m[1].substring(2) : '';
    }
    function qty(v) { return C.num(v).toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: 3 }); }
    function toInt(v) { var n = parseInt(String(v === null || v === undefined ? '' : v).trim(), 10); return isNaN(n) ? 0 : n; }
    function val(id) { var e = $(id); return e ? e.value : ''; }
    function selVal(id) { return toInt(val(id)); }
    function iso(d) { return d.getFullYear() + '-' + String(d.getMonth() + 1).padStart(2, '0') + '-' + String(d.getDate()).padStart(2, '0'); }
    function selText(id) { var e = $(id); return e && e.selectedIndex >= 0 ? e.options[e.selectedIndex].text : ''; }

    function digitsOnly() {                            // OnlytextNumberFunction + ShortcutsEnabled=false
        document.querySelectorAll('.rpta-int').forEach(function (el) {
            el.addEventListener('input', function () { var d = el.value.replace(/[^0-9]/g, ''); if (d !== el.value) el.value = d; });
        });
    }

    /** DDL.BindDDLNew(..., true) — DropDownBind's "...Select Any Value..." row (value 0) first, selected. */
    function bind(id, rows) {
        var h = '<option value="0">...Select Any Value...</option>';
        (rows || []).forEach(function (r) { h += '<option value="' + esc(ci(r, 'Id')) + '">' + esc(ci(r, 'Name')) + '</option>'; });
        $(id).innerHTML = h;
        $(id).value = '0';
    }
    function fillCombos(c) {
        if (!c || !c.bound) return;                    // AllDropDownBind:179 — no rows, nothing rebound
        COMBOS.forEach(function (p) { bind(p[0], c[p[1]]); });
    }
    /** A list of the form's own literal rows. The blank option stands for a cleared combo text. */
    function bindText(id, rows, selectedId) {
        var h = '<option value=""></option>';
        (rows || []).forEach(function (r) {
            h += '<option value="' + esc(r.Name) + '"' + (r.Id === selectedId ? ' selected' : '') + '>' + esc(r.Name) + '</option>';
        });
        $(id).innerHTML = h;
        if (!selectedId) $(id).value = '';
    }

    /* ---------------------------------------------------------------- branch tick-list (BranchesFill:242)
       Filled and pre-ticked with the user's branch, but never read by gridHisory and its Leave handler is
       not wired (note 5). */
    function renderBranches(list, defaultId) {
        var box = $('CmbBranchNameList');
        var h = '<label class="hdr"><input type="checkbox" id="branchAll"> </label>';
        (list || []).forEach(function (b) {
            h += '<label><input type="checkbox" class="rpta-br" data-name="' + esc(b.BranchName) + '" value="' + esc(b.BranchId) + '"' +
                 (C.intOf(b.BranchId) === C.intOf(defaultId) ? ' checked' : '') + '> ' + esc(b.BranchName) + '</label>';
        });
        box.innerHTML = h;
        function text() {
            var n = Array.prototype.map.call(box.querySelectorAll('.rpta-br:checked'), function (c) { return c.getAttribute('data-name'); });
            $('CmbBranchNameText').textContent = n.length ? n.join(',') : ' ';
        }
        $('branchAll').addEventListener('change', function () {
            box.querySelectorAll('.rpta-br').forEach(function (c) { c.checked = $('branchAll').checked; });
            text();
        });
        box.querySelectorAll('.rpta-br').forEach(function (c) { c.addEventListener('change', text); });
        text();
    }

    /* ---------------------------------------------------------------- date type (cmbperemeter_ValueChanged:323) */
    function dateTypeChanged() {
        var v = selVal('cmbperemeter'), t = new Date(), today = iso(t);
        if (v === 1) { $('fromdate').value = today; $('ToDate').value = today; }
        else if (v === 2) { var w = new Date(t); w.setDate(w.getDate() - 7); $('fromdate').value = iso(w); $('ToDate').value = today; }
        else if (v === 3) {                            // DateTime.UtcNow — first day of the UTC month
            var u = new Date();
            $('fromdate').value = u.getUTCFullYear() + '-' + String(u.getUTCMonth() + 1).padStart(2, '0') + '-01';
            $('ToDate').value = today;
        }
        else if (v === 4) { $('fromdate').value = t.getFullYear() + '-01-01'; $('ToDate').value = today; }
        else if (v === 5) { $('fromdate').value = look.financialYearStart || ''; $('ToDate').value = today; }
    }

    /* ---------------------------------------------------------------- grid (gridHisory:486 + GridSettings:535) */
    function cols() {
        var c = [
            { key: 'Slip', btn: 'Slip', w: 50, frozen: true },
            { key: 'DocumentType', src: 'DocumentTypeCode', w: 80 },
            { key: 'DocDate', src: 'DocDate', t: 'date', w: 70 },
            { key: 'DocNo', src: 'DocNo', t: 'int', w: 40 },
            { key: 'RequestedBy', src: 'RequestBy', w: 120 },
            { key: 'ParentCategory', src: 'ParentCategory', w: 80 },
            { key: 'ItemCategory', src: 'ItemCategory', w: 80 },
            { key: 'ItemType', src: 'ItemType', w: 100 },
            { key: 'ItemName', src: 'ItemName', w: 150 },
            { key: 'PackUom', src: 'PackUom', w: 50 },
            { key: 'DepartmentName', src: 'DepartmentName', w: 90 },
            { key: 'AssetName', src: 'AssetName', w: 90 },
            { key: 'JobLotDescription', src: 'JobLotDescription', w: 60 },
            { key: 'RequiredQty', src: 'ItemQty', t: 'qty', sum: true, w: 70 },
            { key: 'ReceivedQty', src: 'ReceivedQty', t: 'qty', sum: true, w: 70 },
            { key: 'BalQty', src: 'BalQty', t: 'qty', sum: true, w: 70 },
            { key: 'Status', src: 'Status', w: 60 },
            { key: 'ApprovalStatus', src: 'ApprovalStatus', w: 90 },
            { key: 'RemarksHeader', src: 'RemarksHeader', w: 140 },
            { key: 'EntryDate', src: 'EntryDate', t: 'date', w: 70 },
            { key: 'EntryUserName', src: 'EntryUserName', w: 80 },
            { key: 'ModifyDate', src: 'ModifyDate', t: 'date', w: 70 },
            { key: 'ModifyUserName', src: 'ModifyUserName', w: 80 },
            { key: 'ApprovedDate', src: 'ApprovedDate', t: 'date', w: 70 },
            { key: 'ApprovedUserName', src: 'ApprovedUserName', w: 80 },
            { key: 'NoOfAttachments', src: 'NoOfAttachments', t: 'int', w: 80, link: true },
            { key: 'StatusRemarks', src: 'StatusRemarks', w: 140, edit: approvedAtFill }
        ];
        if (approvedAtFill) {
            c.push({ key: 'Complete', btn: 'Complete', w: 80 });
            c.push({ key: 'Cancel', btn: 'Cancel', w: 60 });
        }
        return c;
    }
    function cell(c, r) {
        var v = ci(r, c.src);
        if (c.t === 'date') return ddMMMyy(v);
        if (c.t === 'qty') return qty(v);
        if (c.t === 'int') return v === null || v === undefined || v === '' ? '' : String(toInt(v));
        return v === null || v === undefined ? '' : String(v);
    }
    function clearGrid() {
        var t = $('grdPurchaseDemandRegister');
        t.querySelector('thead').innerHTML = ''; t.querySelector('tbody').innerHTML = ''; t.querySelector('tfoot').innerHTML = '';
        $('gridNavigator').innerHTML = '&nbsp;';
    }
    function render(rows) {
        var cs = cols(), t = $('grdPurchaseDemandRegister');
        t.querySelector('thead').innerHTML = '<tr>' + cs.map(function (c) {
            return '<th' + (c.frozen ? ' class="rpta-frozen"' : '') + ' style="min-width:' + c.w + 'px">' + esc(c.key) + '</th>';
        }).join('') + '</tr>';
        t.querySelector('tbody').innerHTML = rows.map(function (r, i) {
            return '<tr>' + cs.map(function (c) {
                if (c.btn) return '<td' + (c.frozen ? ' class="rpta-frozen"' : '') + '><button type="button" class="win-btn-small rpta-cellbtn" data-i="' + i + '" data-btn="' + c.btn + '">' + esc(c.btn) + '</button></td>';
                if (c.edit) return '<td><input type="text" class="win-textbox rpta-remarks" data-i="' + i + '" value="' + esc(cell(c, r)) + '"></td>';
                var cls = (c.t === 'qty' || c.t === 'int') ? ' class="num"' : '';
                if (c.link && toInt(cell(c, r)) > 0) {       // ColumnType Link → LinkClicked:711
                    return '<td' + cls + '><a class="rpta-link" data-att="' + i + '">' + esc(cell(c, r)) + '</a></td>';
                }
                return '<td' + cls + '>' + esc(cell(c, r)) + '</td>';
            }).join('') + '</tr>';
        }).join('');
        t.querySelector('tfoot').innerHTML = '<tr>' + cs.map(function (c) {   // AggregateFunction Sum (:605-616)
            if (!c.sum) return '<td' + (c.frozen ? ' class="rpta-frozen"' : '') + '></td>';
            return '<td class="num">' + esc(qty(rows.reduce(function (a, r) { return a + C.num(ci(r, c.src)); }, 0))) + '</td>';
        }).join('') + '</tr>';
        t.querySelectorAll('button[data-btn]').forEach(function (b) {
            b.addEventListener('click', function () {
                var i = toInt(b.getAttribute('data-i'));
                columnButton(rows[i], b.getAttribute('data-btn'), t.querySelector('input.rpta-remarks[data-i="' + i + '"]'));
            });
        });
        t.querySelectorAll('a[data-att]').forEach(function (a) {
            a.addEventListener('click', function () { viewAttachments(toInt(ci(rows[toInt(a.getAttribute('data-att'))], 'Id'))); });
        });
        $('gridNavigator').textContent = 'Records: ' + rows.length;      // RecordNavigator
    }

    /** CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, 141) → AttachmentView. */
    function viewAttachments(id) {
        var q = C.qs({ id: id, documentTypeId: 141 });
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

    /* ---------------------------------------------------------------- printing (Crystal layout: D1) */
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

    /* ---------------------------------------------------------------- row buttons (grdPurchaseDemandRegister_ColumnButtonClick:679) */
    function columnButton(r, key, remarksInput) {
        var id = toInt(ci(r, 'Id'));
        if (key === 'Slip') {                           // CommonServices.PurchaseDemandSlip454
            C.getJson(API + '/slip/' + id).then(function (p) { printRows(p.template, p.rows); })
                .catch(function (e) { alert(e.message); });
            return;
        }
        if (key === 'Complete') {
            if (selText('CmbApproved') !== 'Approved') return;   // :695
            status(r, 'Complete', 0, remarksInput);
            return;
        }
        if (key === 'Cancel') status(r, 'Cancel', C.num(ci(r, 'ReceivedQty')), remarksInput);
    }
    /** CompleteStatus:728 / CancelStatus:775 — ReceivedQty, then remarks, then the confirm. */
    function status(r, reqType, receivedQty, remarksInput) {
        var id = toInt(ci(r, 'Id'));
        if (reqType === 'Cancel' && receivedQty > 0) {
            alert('Record Not Cancel because Received Qty greater than zero');
            reset();                                    // :789
            return;
        }
        var remarks = remarksInput ? remarksInput.value : '';
        if (remarks === '') { alert('StatusRemarks Required'); return; }
        if (!confirm('Are you sure to ' + reqType + ' Status?')) return;
        C.postJson(API + '/status', {
            Id: id, OrderDetailId: toInt(ci(r, 'OrderDetailId')), ReqType: reqType, StatusRemarks: remarks,
            ReceivedQty: receivedQty, ApprovedText: selText('CmbApproved')
        }).then(function (res) {
            if (res && res.reset) { alert(res.message); reset(); return; }   // CancelStatus:786-790 (server-side qty)
            if (!res || !res.done) return;
            alert(res.message);
            gridHisory();
        }).catch(function (e) { alert(e.message); });
    }

    /* ---------------------------------------------------------------- toolbar */
    function gridHisory() {                             // gridHisory:448
        var approved = selText('CmbApproved');
        var q = {
            fromDate: val('fromdate'), toDate: val('ToDate'),
            docNoFrom: toInt(val('txtGrnNoFrom')), docNoTo: toInt(val('txtGrnNoTo')),
            parentCategoryId: selVal('CmbParentCategory'), itemCategoryId: selVal('CmbItemCategory'),
            itemTypeId: selVal('CmbItemType'), itemId: selVal('CmbItem'),
            departmentId: selVal('CmbDepartment'), assetId: selVal('cmbAssetsRef'),
            status: selText('CmbStatus'), approved: approved
        };
        return C.getJson(API + C.qs(q)).then(function (rows) {
            dtGrid = rows || [];
            if (dtGrid.length) { approvedAtFill = approved === 'Approved'; render(dtGrid); }
            else clearGrid();
        }).catch(function (e) { alert(e.message); });
    }
    function reset() {                                  // reset:363
        ['CmbItem', 'CmbParentCategory', 'CmbItemCategory', 'CmbItemType'].forEach(function (id) {
            $(id).selectedIndex = -1;                   // .Text = string.Empty — no row, value reads 0
        });
        $('fromdate').focus();
        gridHisory();
    }
    function refresh() {                                // toolStripButton1_Click:397 → AllDropDownBind
        C.getJson(API + '/combos').then(fillCombos).catch(function (e) { alert(e.message); });
    }
    function printRegister() {                          // print_Click:828
        if (!dtGrid.length) { alert('Record Not Found'); return; }
        printRows('456-PurchaseDemandRegister.rpt', dtGrid);
    }

    function gotoHistory() {                            // footer History button — the grid section
        $('historySection').scrollIntoView({ behavior: 'smooth', block: 'start' });
    }

    function init() {                                   // frmSaleOrderHistory_Load:134
        digitsOnly();
        var det = $('CmbBranchName');
        document.addEventListener('click', function (e) { if (det.open && !det.contains(e.target)) det.open = false; });
        C.getJson(API + '/lookups').then(function (d) {
            look = d;
            fillCombos(d.combos);                                   // AllDropDownBind
            bindText('CmbStatus', d.statuses, d.defaultStatus);     // ComboStatusFill — Rows[3] "All"
            bindText('CmbApproved', d.approvals, 0);                // ComboApprovedfill — no row activated
            var h = '';
            (d.dateTypes || []).forEach(function (r) { h += '<option value="' + esc(r.Id) + '">' + esc(r.Name) + '</option>'; });
            $('cmbperemeter').innerHTML = h;                        // ParameterFill (ZeroIndex false)
            renderBranches(d.branches, d.defaultBranchId);          // BranchesFill
            $('cmbperemeter').value = '1';                          // Rows[0].Activate() → ValueChanged
            dateTypeChanged();
            $('cmbperemeter').focus();
        }).catch(function (e) { alert(e.message); });
    }

    window.RptPDR = { show: gridHisory, newForm: reset, refresh: refresh, printRegister: printRegister, dateTypeChanged: dateTypeChanged,
                      gotoHistory: gotoHistory };
    document.addEventListener('DOMContentLoaded', init);
})();
