/*
 * Weigh bridge satellite screens — one file, one module per page (body[data-page]):
 *
 *   general   WeighBridgeGeneralLookups        (Architecture.WinApp)
 *   vehicle   VehicleWeightLookUp              (Architecture.WinApp.Lookups)
 *   rejected  frmWeighBridgeRejectedTicketNos  (Inventory_Reports, screen 359)
 *   history   frmWeightBridgeHistory           (Inventory_Reports, screen 360)
 *
 * Every method name in the comments is the desktop handler it reproduces.
 */
(function () {
    'use strict';

    var API = '/api/weighbridge-extras';

    // ================================================================= common helpers

    function $id(id) { return document.getElementById(id); }
    function esc(v) {
        return String(v === null || v === undefined ? '' : v)
            .replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;');
    }
    function str(v) { return v === null || v === undefined ? '' : String(v); }
    function num(v) { var n = parseFloat(str(v).replace(/,/g, '')); return isNaN(n) ? 0 : n; }
    function intOf(v) { var n = parseInt(str(v).replace(/,/g, ''), 10); return isNaN(n) ? 0 : n; }
    function ci(r, k) {
        if (!r) return undefined;
        if (k in r) return r[k];
        var lk = k.toLowerCase();
        for (var p in r) if (p.toLowerCase() === lk) return r[p];
        return undefined;
    }
    function box(m) { window.alert(m); }
    function ask(m) { return window.confirm(m); }
    function val(id) { var e = $id(id); return e ? e.value : ''; }
    function setVal(id, v) { var e = $id(id); if (e) e.value = v === null || v === undefined ? '' : v; }
    function show(id, on) { var e = $id(id); if (e) e.classList.toggle('is-hidden', !on); }
    function focus(id) { var e = $id(id); if (e && e.focus) e.focus(); }

    function fetchJson(url, opts) {
        return fetch(url, Object.assign({ credentials: 'same-origin', headers: { 'Accept': 'application/json' } }, opts || {}))
            .then(function (r) {
                return r.text().then(function (t) {
                    var body = null;
                    try { body = t ? JSON.parse(t) : null; } catch (e) { body = null; }
                    if (!r.ok) throw new Error(body && body.message ? body.message : ('Request failed (' + r.status + ')'));
                    return body;
                });
            });
    }
    function get(path, params) {
        return fetchJson(API + path + (params ? '?' + new URLSearchParams(params).toString() : ''));
    }
    function post(path, body) {
        return fetchJson(API + path, {
            method: 'POST',
            headers: { 'Accept': 'application/json', 'Content-Type': 'application/json' },
            body: JSON.stringify(body || {})
        });
    }

    function bindSelect(id, rows, valueKey, textKey, blank) {
        var h = blank === false ? '' : '<option value=""></option>';
        (rows || []).forEach(function (r) {
            h += '<option value="' + esc(ci(r, valueKey)) + '">' + esc(ci(r, textKey)) + '</option>';
        });
        $id(id).innerHTML = h;
    }
    function selectedText(id) {
        var s = $id(id); if (!s || s.selectedIndex < 0) return '';
        var o = s.options[s.selectedIndex]; return o && o.value !== '' ? o.textContent : '';
    }
    /** Select the option whose TEXT matches, the way the desktop sets combo.Text. */
    function setByText(id, text) {
        var s = $id(id); if (!s) return;
        for (var i = 0; i < s.options.length; i++) {
            if (s.options[i].textContent === str(text)) { s.selectedIndex = i; return; }
        }
        s.value = '';
    }

    var MON = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    function parseDate(v) {
        if (!v) return null;
        var m = /^(\d{4})-(\d{2})-(\d{2})(?:[T ](\d{2}):(\d{2})(?::(\d{2}))?)?/.exec(str(v));
        if (!m) return null;
        return new Date(+m[1], +m[2] - 1, +m[3], +(m[4] || 0), +(m[5] || 0), +(m[6] || 0));
    }
    function two(n) { return (n < 10 ? '0' : '') + n; }
    function fmtDate(v) {
        var d = parseDate(v); if (!d) return '';
        return two(d.getDate()) + '-' + MON[d.getMonth()] + '-' + String(d.getFullYear()).slice(2);
    }
    /** "dd-MMM-yy hh:mm tt" */
    function fmtDateTime(v) {
        var d = parseDate(v); if (!d) return '';
        var h = d.getHours() % 12; if (h === 0) h = 12;
        return fmtDate(v) + ' ' + two(h) + ':' + two(d.getMinutes()) + ' ' + (d.getHours() < 12 ? 'AM' : 'PM');
    }
    /** "#,##0.##" */
    function fmtNum(v) { return num(v).toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: 2 }); }
    /** "0,0" */
    function fmtInt(v) { return Math.round(num(v)).toLocaleString('en-US'); }
    function isoDay(d) { return d.getFullYear() + '-' + two(d.getMonth() + 1) + '-' + two(d.getDate()); }

    /**
     * Render a grid.
     * cols: [{key, caption, type: 'num'|'int'|'date'|'datetime'|'text', sum: bool, fmt: fn, button: text}]
     * opts: {onButton(key,row,index), onDblClick(row,index), empty}
     */
    function renderGrid(hostId, rows, cols, opts) {
        opts = opts || {};
        var host = $id(hostId);
        if (!rows || !rows.length) { host.innerHTML = opts.empty === false ? '' : '<div style="padding:6px;color:#666;">No records.</div>'; return; }
        var h = '<table class="win-grid"><thead><tr>';
        cols.forEach(function (c) { h += '<th>' + esc(c.caption || c.key) + '</th>'; });
        h += '</tr></thead><tbody>';
        rows.forEach(function (r, i) {
            h += '<tr data-i="' + i + '">';
            cols.forEach(function (c) {
                if (c.button) { h += '<td class="int"><button type="button" class="grid-btn" data-btn="' + esc(c.key) + '">' + esc(c.button) + '</button></td>'; return; }
                var v = r[c.key], cls = '', t;
                if (c.fmt) t = c.fmt(v, r);
                else if (c.type === 'date') t = fmtDate(v);
                else if (c.type === 'datetime') t = fmtDateTime(v);
                else if (c.type === 'num') { t = fmtNum(v); cls = 'num'; }
                else if (c.type === 'int') { t = fmtInt(v); cls = 'num'; }
                else t = str(v);
                if (c.type === 'num' || c.type === 'int') cls = 'num';
                h += '<td' + (cls ? ' class="' + cls + '"' : '') + '>' + esc(t) + '</td>';
            });
            h += '</tr>';
        });
        h += '</tbody>';
        if (cols.some(function (c) { return c.sum; })) {
            h += '<tfoot><tr>';
            cols.forEach(function (c) {
                if (!c.sum) { h += '<td></td>'; return; }
                var s = 0; rows.forEach(function (r) { s += num(r[c.key]); });
                h += '<td class="num">' + esc(c.type === 'int' ? fmtInt(s) : fmtNum(s)) + '</td>';
            });
            h += '</tr></tfoot>';
        }
        h += '</table>';
        host.innerHTML = h;
        host.querySelectorAll('tbody tr').forEach(function (tr) {
            var i = +tr.getAttribute('data-i');
            tr.addEventListener('click', function (e) {
                host.querySelectorAll('tr.is-current').forEach(function (x) { x.classList.remove('is-current'); });
                tr.classList.add('is-current');
                var b = e.target.closest('button[data-btn]');
                if (b && opts.onButton) opts.onButton(b.getAttribute('data-btn'), rows[i], i);
            });
            if (opts.onDblClick) tr.addEventListener('dblclick', function () { opts.onDblClick(rows[i], i); });
        });
    }

    function openPrint(title, companyName, companyAddress, bodyHtml) {
        var w = window.open('', '_blank');
        if (!w) { box('The browser blocked the print window. Allow pop-ups for this site.'); return; }
        var h = '<!DOCTYPE html><html><head><meta charset="utf-8"><title>' + esc(title) + '</title>'
            + '<style>body{font-family:Verdana,sans-serif;font-size:10px;margin:14px;}h1{font-size:14px;margin:2px 0 6px;}'
            + '.co{font-weight:bold;font-size:13px;}.addr{margin-bottom:4px;}.crit{margin:4px 0 8px;font-style:italic;}'
            + 'table{border-collapse:collapse;width:100%;}td,th{border:1px solid #444;padding:2px 4px;text-align:left;}'
            + 'th{background:#eee;}td.n{text-align:right;}tfoot td{font-weight:bold;background:#f3f3f3;}'
            + '.kv td:first-child{width:32%;font-weight:bold;background:#f6f6f6;}'
            + '@media print{button{display:none}}</style></head><body>'
            + '<div class="co">' + esc(companyName) + '</div><div class="addr">' + esc(companyAddress) + '</div>'
            + '<h1>' + esc(title) + '</h1><button onclick="window.print()">Print</button>' + bodyHtml + '</body></html>';
        w.document.open(); w.document.write(h); w.document.close();
    }

    /** Checked-list dropdown (the desktop's CheckedListSettings combos). */
    function bindChecks(id, rows, valueKey, textKey, checkedValues) {
        var host = $id(id);
        var set = {};
        (checkedValues || []).forEach(function (v) { set[str(v)] = true; });
        var h = '';
        (rows || []).forEach(function (r) {
            var v = str(ci(r, valueKey));
            h += '<label class="chk"><input type="checkbox" value="' + esc(v) + '"' + (set[v] ? ' checked' : '') + '> '
                + esc(ci(r, textKey)) + '</label>';
        });
        host.querySelector('.chk-list').innerHTML = h || '<div style="color:#666">No rows.</div>';
        host.querySelectorAll('input').forEach(function (i) { i.addEventListener('change', function () { chkSummary(id); }); });
        chkSummary(id);
    }
    function checkedIds(id) {
        var out = [];
        $id(id).querySelectorAll('.chk-list input:checked').forEach(function (i) { out.push(intOf(i.value)); });
        return out;
    }
    function chkSummary(id) {
        var names = [];
        $id(id).querySelectorAll('.chk-list input:checked').forEach(function (i) { names.push(i.parentNode.textContent.trim()); });
        $id(id).querySelector('summary').textContent = names.length ? names.join(',') : ' ';
    }

    function closeWindow() { if (window.history.length > 1) window.history.back(); else window.close(); }

    // ================================================================ WeighBridgeGeneralLookups

    var General = (function () {
        var recId = 0;

        /** DefineCity_Load */
        function init() {
            get('/general-lookups').then(function (d) {
                bindSelect('CmbType', d.types, 'type', 'type');     // DDL.BindDDL(dt, CmbType, "Id", "type", ...)
                grid(d.rows);
                show('btnUpdate', false);
                focus('CmbType');
            }).catch(function (e) { box(e.message); });
        }

        function grid(rows) {
            renderGrid('grd', rows, [{ key: 'Type' }, { key: 'Description' }], { onDblClick: edit });
        }

        /** Reset() */
        function reset() {
            recId = 0;
            show('btnUpdate', false);
            show('btnsave', true);
            setVal('txtDescription', '');
            setVal('CmbType', '');
            get('/general-lookups').then(function (d) { grid(d.rows); }).catch(function (e) { box(e.message); });
        }

        /** grdProviencedefine_DoubleClick */
        function edit(r) {
            show('btnsave', false);
            show('btnUpdate', true);
            recId = intOf(r.Id);
            setByText('CmbType', r.Type);
            setVal('txtDescription', str(r.Description));
        }

        /** FormValidation() */
        function validate() {
            if (selectedText('CmbType').trim() === '') { box('Type Field Required'); focus('CmbType'); return false; }
            if (val('txtDescription').trim() === '') { box('Description Field Required'); focus('txtDescription'); return false; }
            return true;
        }

        /** Insert() */
        function insert() {
            if (!validate()) return;
            if (!ask(recId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
            post('/general-lookups/save', { id: recId, type: selectedText('CmbType').trim(), description: val('txtDescription') })
                .then(function (d) { box(d.message); reset(); })
                .catch(function (e) { box(e.message); });
        }

        function save() { recId = 0; insert(); }       // btnsave_Click
        function update() { insert(); }                 // btnUpdate_Click

        function keys(e) {
            var k = e.key.toUpperCase();
            if (e.ctrlKey && k === 'N') { e.preventDefault(); reset(); }
            if (e.ctrlKey && k === 'S' && !$id('btnsave').classList.contains('is-hidden')) { e.preventDefault(); save(); }
            if (e.ctrlKey && k === 'U' && !$id('btnUpdate').classList.contains('is-hidden')) { e.preventDefault(); update(); }
            if ((e.ctrlKey && k === 'E') || e.key === 'Escape') { e.preventDefault(); closeWindow(); }
        }

        return { init: init, reset: reset, save: save, update: update, keys: keys, close: closeWindow };
    })();

    // ====================================================================== VehicleWeightLookUp

    var Vehicle = (function () {
        var recId = 0;

        /** VehicleWeightLookUp_Load: VehicleTypeFill(), GridFill() */
        function init() {
            get('/vehicle-weights').then(function (d) {
                fillTypes(d.vehicleTypes);
                grid(d.rows);
                focus('CmbVehicleType');
            }).catch(function (e) { box(e.message); });
            $id('txtVehicleNetWeight').addEventListener('keypress', function (e) {
                // CommonServices.OnlytextdecimelFunction — digits and one decimal point
                if (e.ctrlKey || e.key.length > 1) return;
                if (/[0-9]/.test(e.key)) return;
                if (e.key === '.' && this.value.indexOf('.') < 0) return;
                e.preventDefault();
            });
        }

        /** VehicleTypeFill() — keep the current selection when it still exists. */
        function fillTypes(rows) {
            var current = val('CmbVehicleType');
            bindSelect('CmbVehicleType', rows, 'Id', 'VehicleDescription');
            if (current) { setVal('CmbVehicleType', current); if (val('CmbVehicleType') !== current) setVal('CmbVehicleType', ''); }
        }

        function grid(rows) {
            renderGrid('Grd', rows, [
                { key: 'Edit', button: 'Edit' },
                { key: 'VehicleType' }, { key: 'VehicleNo' },
                { key: 'NetWeight', type: 'num', sum: true },
                { key: 'Remarks' }
            ], {
                onButton: function (k, r) { if (k === 'Edit') edit(r); },
                onDblClick: edit
            });
        }

        /** Reset() — the vehicle type is left as it is, as on the desktop. */
        function reset() {
            focus('CmbVehicleType');
            setVal('txtVehicleNo', '');
            setVal('txtVehicleNetWeight', '');
            setVal('txtRemarks', '');
            show('btnUpdate', false);
            show('btnsave', true);
        }

        /** Grd_DoubleClick */
        function edit(r) {
            recId = intOf(r.Id);
            setByText('CmbVehicleType', r.VehicleType);
            setVal('txtVehicleNo', str(r.VehicleNo));
            setVal('txtVehicleNetWeight', str(r.NetWeight));
            setVal('txtRemarks', str(r.Remarks));
            show('btnsave', false);
            show('btnUpdate', true);
        }

        /** FormValidation() */
        function validate() {
            if (selectedText('CmbVehicleType').trim() === '') { box('Vehicle Type Field is Required'); focus('CmbVehicleType'); return false; }
            if (val('txtVehicleNo').trim() === '') { box('Vehicle No Field is Required'); focus('txtVehicleNo'); return false; }
            return true;
        }

        /** Insert() — the duplicate check runs on the server against the same grid rows. */
        function insert() {
            if (!validate()) return;
            if (!ask(recId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
            post('/vehicle-weights/save', {
                id: recId,
                vehicleType: selectedText('CmbVehicleType'),
                vehicleNo: val('txtVehicleNo'),
                netWeight: val('txtVehicleNetWeight'),
                remarks: val('txtRemarks')
            }).then(function (d) { box(d.message); reset(); grid(d.rows); })
              .catch(function (e) { box(e.message); });
        }

        function save() { recId = 0; insert(); }
        function update() { insert(); }

        /** btnRefresh_Click — VehicleTypeFill() only. */
        function refresh() { get('/vehicle-types').then(fillTypes).catch(function (e) { box(e.message); }); }

        function shortcuts() {
            var rows = [['Ctrl+S', 'For Save When on Entry form and For Show Data when on History Form'], ['Ctrl+U', 'For Update'],
                ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Ctrl+P', 'For Print'],
                ['Ctrl+F5', 'For Focus on Vehicle Type'], ['Ctrl+alt', 'To Show ShortCut Keys Form'],
                ['Ctrl+ArrowDown', 'For Focus On Detail Grid'], ['Ctrl+ArrowUp', 'For Focus On on Vehicle Type'],
                ['Ctrl+Enter', 'When Focus On Any Grid For Update Record'],
                ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]];
            box(rows.map(function (r) { return r[0] + '  —  ' + r[1]; }).join('\n'));
        }

        function keys(e) {
            var k = e.key.toUpperCase();
            if (e.ctrlKey && k === 'N') { e.preventDefault(); reset(); }
            if (e.ctrlKey && k === 'S' && !$id('btnsave').classList.contains('is-hidden')) { e.preventDefault(); save(); }
            if (e.ctrlKey && k === 'U' && !$id('btnUpdate').classList.contains('is-hidden')) { e.preventDefault(); update(); }
            if ((e.ctrlKey && k === 'E') || e.key === 'Escape') { e.preventDefault(); closeWindow(); }
            if (e.ctrlKey && e.altKey) { e.preventDefault(); shortcuts(); }
            if (e.ctrlKey && (e.key === 'ArrowUp' || e.key === 'F5')) { e.preventDefault(); focus('CmbVehicleType'); }
            if (e.ctrlKey && e.key === 'ArrowDown') { e.preventDefault(); var b = $id('Grd').querySelector('button'); if (b) b.focus(); }
        }

        return { init: init, reset: reset, save: save, update: update, refresh: refresh, shortcuts: shortcuts, keys: keys, close: closeWindow };
    })();

    // ========================================================== 359 frmWeighBridgeRejectedTicketNos

    var Rejected = (function () {
        var COLS = [
            { key: 'DocDate', type: 'date' }, { key: 'TicketNo' }, { key: 'DocumentTypeDescription' },
            { key: 'ItemQty', type: 'int', sum: true }, { key: 'FirstWeight', type: 'int', sum: true },
            { key: 'SecondWeight', type: 'int', sum: true }, { key: 'NetWbWeight', type: 'int', sum: true },
            { key: 'VehicleNo' }, { key: 'BiltyNo' }, { key: 'ItemDescription' }, { key: 'WbRemarks' },
            { key: 'WbCharges', type: 'int', sum: true }, { key: 'Reject', button: 'Reject' }
        ];

        /** frmStockWithSupplierHistory_Load / Reset() / btnnew_Click → GridFill() */
        function gridFill() {
            get('/rejected-ticket-nos').then(render).catch(function (e) { box(e.message); });
        }
        function render(rows) {
            renderGrid('grdHistory', rows, COLS, { onButton: onButton, empty: false });
            $id('lblCount').textContent = rows && rows.length ? '(' + rows.length + ')' : '';
        }

        /** grdHistory_ColumnButtonClick */
        function onButton(key, r) {
            if (key !== 'Reject') return;
            if (!ask('Are you sure to reject record?')) return;
            post('/rejected-ticket-nos/' + intOf(r.Id) + '/reject')
                .then(function (d) { box(d.message); render(d.rows); })
                .catch(function (e) { box(e.message); });
        }

        function keys(e) { if (e.key === 'Escape') { e.preventDefault(); closeWindow(); } }

        return { init: gridFill, reset: gridFill, keys: keys, close: closeWindow };
    })();

    // =================================================================== 360 frmWeightBridgeHistory

    var History = (function () {
        var L = null;
        var lstwb = [];

        var COLS = [
            { key: 'Print', button: 'Print' },
            { key: 'BranchName' }, { key: 'DocumentTypeDescription' }, { key: 'WeighBridgeType' },
            { key: 'DocDate', type: 'date' }, { key: 'TicketNo.' }, { key: 'OrderNo' }, { key: 'GpNo' },
            { key: 'PartyName' }, { key: 'VehicleNo' }, { key: 'BiltyNo' }, { key: 'ItemDescription' },
            { key: 'ItemQty', type: 'num', sum: true }, { key: 'SupplierWeight', type: 'num', sum: true },
            { key: 'WbCharges', type: 'num', sum: true }, { key: 'FirstWeight', type: 'num', sum: true },
            { key: 'SecondWeight', type: 'num', sum: true }, { key: 'NetWbWeight', type: 'num', sum: true },
            { key: 'FirstDateTime', type: 'datetime' }, { key: 'SecondDateTime', type: 'datetime' },
            { key: 'WbRemarks' }, { key: 'EntryUser' }, { key: 'EntryDate', type: 'datetime' }
        ];

        /** frmWeightBridgeHistory_Load */
        function init() {
            var d7 = new Date(); d7.setDate(d7.getDate() - 7);
            setVal('FromDate', isoDay(d7));                 // FromDate.Value = DateTime.Now.AddDays(-7.0)
            setVal('ToDate', isoDay(new Date()));
            ['txtTicketNoFrom', 'txtTicketNoTo', 'GPNoFrom', 'GpNoTo', 'txtOrderNoFrom', 'txtOrderNoTo'].forEach(function (id) {
                $id(id).addEventListener('keypress', function (e) {   // OnlytextNumberFunction
                    if (e.ctrlKey || e.key.length > 1) return;
                    if (!/[0-9]/.test(e.key)) e.preventDefault();
                });
            });
            get('/history/lookups').then(function (d) {
                L = d;
                // BranchesFill: CmbBranch.Text = UserAccount.BranchName → the user's own branch checked
                bindChecks('CmbBranch', d.branches, 'BranchId', 'BranchName', d.userBranchId ? [d.userBranchId] : []);
                bindChecks('CmbGatePassType', d.gatePassTypes, 'Id', 'Name', []);
                bindSelect('CmbWeighBridgeType', d.weighBridgeTypes, 'Id', 'GpTypeDescription');
                bindSelect('CmbPartyName', d.parties, 'Id', 'CompanyName');
                bindSelect('CmbWeighBridgeParty', d.wbParties, 'Id', 'PartyName');
                bindSelect('CmbDocType', d.docTypes, 'Id', 'DocType');
            }).catch(function (e) { box(e.message); });
        }

        /** grdweightBridge() */
        function load() {
            var body = {
                fromDate: val('FromDate'), toDate: val('ToDate'),
                ticketNoFrom: val('txtTicketNoFrom'), ticketNoTo: val('txtTicketNoTo'),
                gpNoFrom: val('GPNoFrom'), gpNoTo: val('GpNoTo'),
                vehicleNo: val('TxtVehicleNumber'),
                weighBridgeType: selectedText('CmbWeighBridgeType'),
                supplierCustomerId: intOf(val('CmbPartyName')),
                orderNoFrom: val('txtOrderNoFrom'), orderNoTo: val('txtOrderNoTo'),
                documentTypeId: intOf(val('CmbDocType')),
                wbPartyName: selectedText('CmbWeighBridgeParty'),
                gatePassTypeIds: checkedIds('CmbGatePassType'),
                branchIds: checkedIds('CmbBranch')
            };
            return post('/history', body).then(function (d) {
                lstwb = d.rows || [];
                renderGrid('DataGridHistory', d.grid, COLS, { onButton: onButton, empty: false });
                $id('lblCount').textContent = d.grid && d.grid.length ? '(' + d.grid.length + ')' : '';
            }).catch(function (e) { box(e.message); });
        }

        /** Reset() */
        function reset() {
            setVal('txtTicketNoFrom', '0');
            setVal('txtTicketNoTo', '');
            setVal('TxtVehicleNumber', '');
            setVal('GPNoFrom', '');
            setVal('GpNoTo', '');
            if (L && L.yearStart) setVal('FromDate', L.yearStart);   // ActiveYr.Start_Period
            setVal('ToDate', isoDay(new Date()));
        }

        /** btnNew_Click — grdweightBridge() runs first, then Reset(), as written. */
        function newClick() { load().then(reset); }

        /** toolStripButton1_Click — PartyNameFill(), WbType(). */
        function refresh() {
            get('/history/refresh').then(function (d) {
                var p = val('CmbPartyName'), t = val('CmbWeighBridgeType');
                bindSelect('CmbPartyName', d.parties, 'Id', 'CompanyName');
                bindSelect('CmbWeighBridgeType', d.weighBridgeTypes, 'Id', 'GpTypeDescription');
                setVal('CmbPartyName', p); setVal('CmbWeighBridgeType', t);
            }).catch(function (e) { box(e.message); });
        }

        /** DataGridHistory_ColumnButtonClick "Print" — 280-InvRptWeighBridgeSlip for the row. */
        function onButton(key, r) {
            if (key !== 'Print') return;
            get('/history/slip/' + intOf(r.Id), { documentTypeId: intOf(r.DocTypeId) }).then(function (rows) {
                if (!rows || !rows.length) { box('Not Record Found For Display'); return; }
                var h = '<table class="kv">';
                Object.keys(rows[0]).forEach(function (k) {
                    if (/Pic|Image/i.test(k)) return;
                    h += '<tr><td>' + esc(k) + '</td><td>' + esc(rows[0][k]) + '</td></tr>';
                });
                h += '</table>';
                openPrint('280 - Weigh Bridge Slip', L ? L.companyName : '', L ? L.companyAddress : '', h);
            }).catch(function (e) { box(e.message); });
        }

        /** print_Click — the 281 register over lstwb. */
        function printRegister() {
            if (!lstwb || lstwb.length === 0) { box('Record Not Found For Display'); return; }
            var cols = [
                ['BranchName', 'Branch'], ['DocumentTypeDescription', 'Ref Doc'], ['WeighBridgeType', 'WB Type'],
                ['DocDate', 'Date', 'date'], ['TicketNo', 'Ticket#'], ['OrderNo', 'Order#'], ['GpSrNo', 'GP#'],
                ['PartyName', 'Party'], ['VehicleNo', 'Vehicle'], ['BiltyNo', 'Bilty'], ['ItemDescription', 'Item'],
                ['ItemQty', 'Qty', 'n'], ['SupplierWeight', 'Supplier Wt', 'n'], ['FirstWeight', 'First Wt', 'n'],
                ['SecondWeight', 'Second Wt', 'n'], ['NetWbWeight', 'Net Wt', 'n'], ['PackingTotalWeight', 'Packing Wt', 'n'],
                ['FinalWeight', 'Final Wt', 'n'], ['WbCharges', 'Charges', 'n'], ['WbRemarks', 'Remarks']
            ];
            var sums = {};
            var h = '<div class="crit">' + esc(ci(lstwb[0], 'ReportCriteria')) + '</div><table><thead><tr>';
            cols.forEach(function (c) { h += '<th>' + esc(c[1]) + '</th>'; });
            h += '</tr></thead><tbody>';
            lstwb.forEach(function (r) {
                h += '<tr>';
                cols.forEach(function (c) {
                    var v = ci(r, c[0]);
                    if (c[2] === 'n') { sums[c[0]] = (sums[c[0]] || 0) + num(v); h += '<td class="n">' + esc(fmtNum(v)) + '</td>'; }
                    else if (c[2] === 'date') h += '<td>' + esc(fmtDate(v)) + '</td>';
                    else h += '<td>' + esc(v) + '</td>';
                });
                h += '</tr>';
            });
            h += '</tbody><tfoot><tr>';
            cols.forEach(function (c) { h += '<td class="n">' + (c[2] === 'n' ? esc(fmtNum(sums[c[0]])) : '') + '</td>'; });
            h += '</tr></tfoot></table>';
            openPrint('281 - Weigh Bridge Register', L ? L.companyName : '', L ? L.companyAddress : '', h);
        }

        function keys(e) {
            var k = e.key.toUpperCase();
            if (e.ctrlKey && k === 'P') { e.preventDefault(); printRegister(); }
            if ((e.ctrlKey && k === 'E') || e.key === 'Escape') { e.preventDefault(); closeWindow(); }
            if (e.ctrlKey && k === 'N') { e.preventDefault(); newClick(); }
            if (e.ctrlKey && e.key === 'F5') { e.preventDefault(); focus('FromDate'); }
        }

        return { init: init, show: load, newClick: newClick, refresh: refresh, printRegister: printRegister, keys: keys, close: closeWindow };
    })();

    // ============================== WeighBridge_WeightUpdate (Gear → Admin Panel, Admin only)

    var WeightUpdate = (function () {
        var L = null;
        var lstwb = [];
        var grid = [];

        var COLS = [
            ['BranchName'], ['DocumentTypeDescription'], ['DocDate', 'date'], ['TicketNo.'], ['OrderNo'], ['GpNo'],
            ['PartyName'], ['VehicleNo'], ['BiltyNo'], ['SupplierWeight', 'num', true],
            ['FirstWeight', 'edit', true], ['SecondWeight', 'edit', true], ['NetWbWeight', 'num', true],
            ['FirstDateTime', 'datetime'], ['SecondDateTime', 'datetime'], ['ItemDescription'],
            ['ItemQty', 'num', true], ['WeighBridgeType'], ['WbCharges', 'num', true], ['WbRemarks'],
            ['EntryUser'], ['EntryDate', 'datetime']
        ];

        /** frmWeightBridgeHistory_Load — BranchesFill() and FromDate = today − 7; nothing else. */
        function init() {
            var d7 = new Date(); d7.setDate(d7.getDate() - 7);
            setVal('FromDate', isoDay(d7));
            setVal('ToDate', isoDay(new Date()));
            ['txtTicketNoFrom', 'txtTicketNoTo', 'GPNoFrom', 'GpNoTo', 'txtOrderNoFrom', 'txtOrderNoTo'].forEach(function (id) {
                $id(id).addEventListener('keypress', function (e) {
                    if (e.ctrlKey || e.key.length > 1) return;
                    if (!/[0-9]/.test(e.key)) e.preventDefault();
                });
            });
            get('/weight-update/lookups').then(function (d) {
                L = d;
                bindChecks('CmbBranch', d.branches, 'BranchId', 'BranchName', d.userBranchId ? [d.userBranchId] : []);
            }).catch(function (e) { $id('denied').textContent = e.message; show('denied', true); });
        }

        /** grdweightBridge() */
        function load() {
            return post('/weight-update/search', {
                fromDate: val('FromDate'), toDate: val('ToDate'),
                ticketNoFrom: val('txtTicketNoFrom'), ticketNoTo: val('txtTicketNoTo'),
                gpNoFrom: val('GPNoFrom'), gpNoTo: val('GpNoTo'),
                vehicleNo: val('TxtVehicleNumber'),
                orderNoFrom: val('txtOrderNoFrom'), orderNoTo: val('txtOrderNoTo'),
                branchIds: checkedIds('CmbBranch')
            }).then(function (d) {
                lstwb = d.rows || [];
                grid = d.grid || [];
                render();
            }).catch(function (e) { box(e.message); });
        }

        function render() {
            var host = $id('DataGridHistory');
            $id('lblCount').textContent = grid.length ? '(' + grid.length + ')' : '';
            if (!grid.length) { host.innerHTML = ''; return; }
            var h = '<table class="win-grid"><thead><tr><th><input type="checkbox" id="chkAll" title="Select all"></th><th>Print</th><th>Update</th>';
            COLS.forEach(function (c) { h += '<th>' + esc(c[0]) + '</th>'; });
            h += '</tr></thead><tbody>';
            grid.forEach(function (r, i) {
                h += '<tr data-i="' + i + '"><td class="int"><input type="checkbox" class="sel"></td>'
                    + '<td class="int"><button type="button" class="grid-btn" data-btn="Print">Print</button></td>'
                    + '<td class="int"><button type="button" class="grid-btn" data-btn="Update">Update</button></td>';
                COLS.forEach(function (c) {
                    var v = r[c[0]];
                    if (c[1] === 'edit') h += '<td class="num"><input type="text" class="wt" data-k="' + c[0] + '" value="' + esc(str(v)) + '" inputmode="decimal" style="width:90px;text-align:right;"></td>';
                    else if (c[1] === 'num') h += '<td class="num" data-k="' + c[0] + '">' + esc(fmtNum(v)) + '</td>';
                    else if (c[1] === 'date') h += '<td>' + esc(fmtDate(v)) + '</td>';
                    else if (c[1] === 'datetime') h += '<td>' + esc(fmtDateTime(v)) + '</td>';
                    else h += '<td>' + esc(v) + '</td>';
                });
                h += '</tr>';
            });
            h += '</tbody><tfoot><tr><td></td><td></td><td></td>';
            COLS.forEach(function (c) { h += '<td class="num" data-sum="' + (c[2] ? c[0] : '') + '"></td>'; });
            h += '</tr></tfoot></table>';
            host.innerHTML = h;
            totals();
            $id('chkAll').addEventListener('change', function () {
                var on = this.checked;
                host.querySelectorAll('tbody input.sel').forEach(function (x) { x.checked = on; });
            });
            host.querySelectorAll('tbody tr').forEach(function (tr) {
                var i = +tr.getAttribute('data-i');
                tr.addEventListener('click', function (e) {
                    host.querySelectorAll('tr.is-current').forEach(function (x) { x.classList.remove('is-current'); });
                    tr.classList.add('is-current');
                    var b = e.target.closest('button[data-btn]');
                    if (!b) return;
                    if (b.getAttribute('data-btn') === 'Print') printSlip(grid[i]);
                    else updateSingle(i);
                });
                tr.querySelectorAll('input.wt').forEach(function (inp) {
                    // DataGridHistory_UpdatingCell: numbers only
                    inp.addEventListener('change', function () {
                        if (this.value.trim() !== '' && isNaN(Number(this.value.replace(/,/g, '')))) {
                            box('Please Type Only Numeric Value');
                            this.value = str(grid[i][this.getAttribute('data-k')]);
                        }
                        cellUpdated(i, tr);
                    });
                    inp.addEventListener('input', function () { cellUpdated(i, tr); });
                });
            });
        }

        /** DataGridHistory_CellUpdated — NetWbWeight = |First − Second|. */
        function cellUpdated(i, tr) {
            var f = num(tr.querySelector('input[data-k="FirstWeight"]').value);
            var s = num(tr.querySelector('input[data-k="SecondWeight"]').value);
            tr.querySelector('td[data-k="NetWbWeight"]').textContent = fmtNum(Math.abs(f - s));
            totals();
        }

        function rowValue(tr, key, i) {
            var inp = tr.querySelector('input[data-k="' + key + '"]');
            if (inp) return num(inp.value);
            var td = tr.querySelector('td[data-k="' + key + '"]');
            if (td && key === 'NetWbWeight') return num(td.textContent);
            return num(grid[i][key]);
        }

        function totals() {
            var host = $id('DataGridHistory');
            host.querySelectorAll('tfoot td[data-sum]').forEach(function (td) {
                var k = td.getAttribute('data-sum'); if (!k) return;
                var s = 0;
                host.querySelectorAll('tbody tr').forEach(function (tr) { s += rowValue(tr, k, +tr.getAttribute('data-i')); });
                td.textContent = fmtNum(s);
            });
        }

        function edits(tr, i) {
            return {
                id: intOf(grid[i].Id),
                firstWeight: tr.querySelector('input[data-k="FirstWeight"]').value,
                secondWeight: tr.querySelector('input[data-k="SecondWeight"]').value
            };
        }

        /** UpdateSingleRecord() — the row's Update button. */
        function updateSingle(i) {
            var tr = $id('DataGridHistory').querySelector('tbody tr[data-i="' + i + '"]');
            post('/weight-update/update', { single: true, comments: val('txtComments'), rows: [edits(tr, i)] })
                .then(function (d) { box(d.message); return load(); })
                .catch(function (e) { box(e.message); if (/Comments/.test(e.message)) focus('txtComments'); });
        }

        /** btnUpdate_Click — every checked row. */
        function updateChecked() {
            var rows = [];
            $id('DataGridHistory').querySelectorAll('tbody tr').forEach(function (tr) {
                if (tr.querySelector('input.sel').checked) rows.push(edits(tr, +tr.getAttribute('data-i')));
            });
            post('/weight-update/update', { single: false, comments: val('txtComments'), rows: rows })
                .then(function (d) {
                    if (d.message === null || d.message === undefined) return;   // nothing changed: the desktop says nothing
                    return load().then(function () { box(d.message); });
                })
                .catch(function (e) { box(e.message); if (/Comments/.test(e.message)) focus('txtComments'); });
        }

        /** Reset() */
        function reset() {
            setVal('txtTicketNoFrom', '0');
            setVal('txtTicketNoTo', '');
            setVal('TxtVehicleNumber', '');
            setVal('GPNoFrom', '');
            setVal('GpNoTo', '');
            if (L && L.yearStart) setVal('FromDate', L.yearStart);
            setVal('ToDate', isoDay(new Date()));
        }

        /** btnNew_Click — search first, then Reset(), as written. */
        function newClick() { load().then(reset); }

        /** toolStripButton1_Click — BranchesFill() only. */
        function refresh() {
            var keep = checkedIds('CmbBranch');
            get('/weight-update/branches').then(function (rows) {
                bindChecks('CmbBranch', rows, 'BranchId', 'BranchName', keep.length ? keep : (L && L.userBranchId ? [L.userBranchId] : []));
            }).catch(function (e) { box(e.message); });
        }

        function printSlip(r) {
            get('/history/slip/' + intOf(r.Id), { documentTypeId: intOf(r.DocTypeId) }).then(function (rows) {
                if (!rows || !rows.length) { box('Not Record Found For Display'); return; }
                var h = '<table class="kv">';
                Object.keys(rows[0]).forEach(function (k) {
                    if (/Pic|Image/i.test(k)) return;
                    h += '<tr><td>' + esc(k) + '</td><td>' + esc(rows[0][k]) + '</td></tr>';
                });
                openPrint('280 - Weigh Bridge Slip', L ? L.companyName : '', L ? L.companyAddress : '', h + '</table>');
            }).catch(function (e) { box(e.message); });
        }

        /** print_Click — 281 register over lstwb. */
        function printRegister() {
            if (!lstwb || lstwb.length === 0) { box('Record Not Found For Display'); return; }
            var cols = [['BranchName', 'Branch'], ['DocumentTypeDescription', 'Ref Doc'], ['WeighBridgeType', 'WB Type'],
                ['DocDate', 'Date', 'date'], ['TicketNo', 'Ticket#'], ['OrderNo', 'Order#'], ['GpSrNo', 'GP#'],
                ['PartyName', 'Party'], ['VehicleNo', 'Vehicle'], ['ItemDescription', 'Item'], ['ItemQty', 'Qty', 'n'],
                ['SupplierWeight', 'Supplier Wt', 'n'], ['FirstWeight', 'First Wt', 'n'], ['SecondWeight', 'Second Wt', 'n'],
                ['NetWbWeight', 'Net Wt', 'n'], ['WbCharges', 'Charges', 'n'], ['WbRemarks', 'Remarks']];
            var sums = {}, h = '<table><thead><tr>';
            cols.forEach(function (c) { h += '<th>' + esc(c[1]) + '</th>'; });
            h += '</tr></thead><tbody>';
            lstwb.forEach(function (r) {
                h += '<tr>';
                cols.forEach(function (c) {
                    var v = ci(r, c[0]);
                    if (c[2] === 'n') { sums[c[0]] = (sums[c[0]] || 0) + num(v); h += '<td class="n">' + esc(fmtNum(v)) + '</td>'; }
                    else if (c[2] === 'date') h += '<td>' + esc(fmtDate(v)) + '</td>';
                    else h += '<td>' + esc(v) + '</td>';
                });
                h += '</tr>';
            });
            h += '</tbody><tfoot><tr>';
            cols.forEach(function (c) { h += '<td class="n">' + (c[2] === 'n' ? esc(fmtNum(sums[c[0]])) : '') + '</td>'; });
            openPrint('281 - Weigh Bridge Register', L ? L.companyName : '', L ? L.companyAddress : '', h + '</tr></tfoot></table>');
        }

        function keys(e) {
            var k = e.key.toUpperCase();
            if (e.ctrlKey && k === 'P') { e.preventDefault(); printRegister(); }
            if ((e.ctrlKey && k === 'E') || e.key === 'Escape') { e.preventDefault(); closeWindow(); }
            if (e.ctrlKey && k === 'N') { e.preventDefault(); newClick(); }
            if (e.ctrlKey && e.key === 'F5') { e.preventDefault(); focus('FromDate'); }
        }

        return { init: init, show: load, newClick: newClick, refresh: refresh, printRegister: printRegister,
                 updateChecked: updateChecked, keys: keys, close: closeWindow };
    })();

    // ================================================================================ boot

    var pages = { general: General, vehicle: Vehicle, rejected: Rejected, history: History, weightupdate: WeightUpdate };
    var page = pages[document.body.getAttribute('data-page')];
    window.WbExtras = page;
    if (page) {
        document.addEventListener('keydown', page.keys);
        if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', page.init);
        else page.init();
    }
})();
