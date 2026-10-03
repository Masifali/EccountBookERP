/* ============================================================================================
 * countx_export_packing_detail.js - PackingDetailForCommercialInvoice.cs (Architecture.WinApp.Export), screen 193
 * "Packing Detail" ("Packing List for Commercial Invoice"). Form | History; API /api/export/packing-detail.
 * Every desktop event has its counterpart with the desktop's messages and order:
 *   Cmbinvoiceno TextChanged -> CommercialInvoiceDataByInvoiceId; grdDetail CellUpdated (Qty / PackingWeight /
 *   PackingWeightTotal), ColumnButtonClick (X = delete, only in save mode; Add = AddRowInGrid balance row);
 *   btnsave / btnupdate -> Insert; btnDelete; btnPrint (550); History tab -> HistoryGridFill(50); LoadAll; Edit / Slip /
 *   double-click; the form KeyDown (Ctrl+S / Ctrl+U call Insert without resetting RecId, as the desktop does).
 * Button contract: disabled + spinner while a request runs, no duplicates, re-enabled on success and failure.
 * ============================================================================================ */
(function (global) {
    'use strict';

    var API = '/api/export/packing-detail';

    function $id(id) { return document.getElementById(id); }
    function box(m) { global.alert(m); }
    function ask(m) { return global.confirm(m); }
    function esc(s) {
        return String(s === null || s === undefined ? '' : s)
            .replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    }
    function str(v) { return v === null || v === undefined ? '' : String(v); }
    function netI(v) { var n = parseInt(String(v === null || v === undefined ? '' : v).replace(/,/g, ''), 10); return isFinite(n) ? n : 0; }
    function netD(v) {
        if (typeof v === 'number') return isFinite(v) ? v : 0;
        var s = String(v === null || v === undefined ? '' : v).replace(/,/g, '').trim();
        if (!s || !/^[-+]?(\d+\.?\d*|\.\d+)([eE][-+]?\d+)?$/.test(s)) return 0;
        var n = parseFloat(s); return isFinite(n) ? n : 0;
    }
    function clr(v) { var n = netD(v); if (n === Math.round(n) && Math.abs(n) < 1e15) return String(n); return Number(n.toPrecision(15)).toString(); }
    function roundAway(n, d) { var p = Math.pow(10, d); var r = Math.round(Math.abs(n) * p + 1e-9) / p; return n < 0 ? -r : r; }
    /* Math.Round(x, 2) - round half to even. */
    function roundEven(n, d) {
        var p = Math.pow(10, d), x = n * p, f = Math.floor(x), diff = x - f;
        var r = (Math.abs(diff - 0.5) < 1e-9) ? (f % 2 === 0 ? f : f + 1) : Math.round(x);
        return r / p;
    }
    function group(ip) { return ip.replace(/\B(?=(\d{3})+(?!\d))/g, ','); }
    function fmtNet(v, pattern) {
        var n = netD(v);
        var m = /\.(#+|0+)$/.exec(pattern);
        var dec = m ? m[1].length : 0, fixed = m ? m[1].charAt(0) === '0' : false;
        var r = roundAway(n, dec), neg = r < 0; r = Math.abs(r);
        var parts = r.toFixed(dec).split('.'), ip = parts[0], fp = parts[1] || '';
        if (!fixed) fp = fp.replace(/0+$/, '');
        if (pattern.indexOf('#,#') === 0 && pattern.indexOf('#,##0') !== 0 && ip === '0') ip = '';
        if (pattern === '0,0' && ip.length < 2) ip = ('00' + ip).slice(-2);
        var out = group(ip) + (fp ? '.' + fp : '');
        if (out === '') return '';
        return (neg && out !== '0' ? '-' : '') + out;
    }
    function pad(n) { return (n < 10 ? '0' : '') + n; }
    function today() { var d = new Date(); return d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()); }
    function isoDate(v) { var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(str(v).trim()); return m ? m[1] + '-' + m[2] + '-' + m[3] : ''; }
    function shortDate(v) { var s = isoDate(v); if (!s) return ''; return parseInt(s.substring(5, 7), 10) + '/' + parseInt(s.substring(8, 10), 10) + '/' + s.substring(0, 4); }
    function busy(btn, fn) {
        var b = (typeof btn === 'string') ? $id(btn) : btn;
        if (b) {
            if (b.disabled || b.classList.contains('is-busy')) return Promise.resolve();
            b.disabled = true; b.classList.add('is-busy');
        }
        var done = function () { if (b) { b.disabled = false; b.classList.remove('is-busy'); } };
        var p;
        try { p = fn(); } catch (e) { done(); box(e.message); return Promise.resolve(); }
        if (p && typeof p.then === 'function') return p.then(done, function (e) { done(); if (e && e.message) box(e.message); });
        done();
        return Promise.resolve(p);
    }
    function parse(r) {
        return r.text().then(function (t) {
            var b = null;
            try { b = t ? JSON.parse(t) : null; } catch (e) { /* not json */ }
            if (!r.ok) throw new Error((b && b.message) || ('Request failed (' + r.status + ')'));
            return b;
        });
    }
    function getJson(url) { return fetch(url, { headers: { 'Accept': 'application/json' }, credentials: 'same-origin' }).then(parse); }
    function postJson(url, data) {
        var h = { 'Accept': 'application/json', 'Content-Type': 'application/json' };
        var t = document.querySelector('meta[name="_csrf"]'), n = document.querySelector('meta[name="_csrf_header"]');
        if (t && n && t.getAttribute('content') && n.getAttribute('content')) h[n.getAttribute('content')] = t.getAttribute('content');
        return fetch(url, { method: 'POST', headers: h, credentials: 'same-origin', body: JSON.stringify(data) }).then(parse);
    }
    function refreshCombos() { if (global.DesktopCombo) global.DesktopCombo.refresh(); }
    function show(id, on) { var e = $id(id); if (e) e.classList.toggle('is-hidden', !on); }
    function visible(id) { var e = $id(id); return !!e && !e.classList.contains('is-hidden'); }
    function bind(id, rows, valueCol, textCol, keep) {
        var s = $id(id); if (!s) return;
        var old = s.value, h = '<option value=""></option>';
        (rows || []).forEach(function (r) { h += '<option value="' + esc(r[valueCol]) + '">' + esc(r[textCol]) + '</option>'; });
        s.innerHTML = h;
        s.value = keep === false ? '' : old;
        if (s.value !== old && keep !== false) s.value = '';
        refreshCombos();
    }
    function setVal(id, v) { var s = $id(id); if (!s) return; var t = (v === null || v === undefined || v === 0 || v === '0') ? '' : String(v); s.value = t; if (s.value !== t) s.value = ''; refreshCombos(); }
    function val(id) { var e = $id(id); return e ? e.value : ''; }
    function setText(id, v) { var e = $id(id); if (e) e.value = str(v); }
    function setEnabled(id, on) { var e = $id(id); if (e) { e.disabled = !on; refreshCombos(); } }
    function focus(id) { var e = $id(id); if (!e) return; var w = e.closest && e.closest('.dtcombo-wrap'); var t = w ? w.querySelector('.dtcombo-input') : e; try { (t || e).focus(); } catch (x) { /* ignore */ } }
    function cancel() { global.close(); setTimeout(function () { if (!global.closed) { if (history.length > 1) history.back(); else location.href = '/export'; } }, 150); }

    // ------------------------------------------------------------------------------ state

    var PERM = { Save: true, Update: true, Delete: true, Print: true };
    var L = { warehouses: [], jobLots: [] };
    var S = { recId: 0, updateMode: false, rows: [], cur: -1, canDelete: false, invoiceId: 0 };
    var HIST = [], HCUR = -1;
    var CARRIER = [{ Id: 1, Type: 'By Sea' }, { Id: 2, Type: 'By Air' }, { Id: 3, Type: 'By Road' }];

    function tab(panelId) {
        document.querySelectorAll('.win-tabs[data-tabs="pd"] .win-tab').forEach(function (b) { b.classList.toggle('is-active', b.getAttribute('data-tab') === panelId); });
        ['pdForm', 'pdHistory'].forEach(function (p) { $id(p).classList.toggle('is-active', p === panelId); });
        var onHist = panelId === 'pdHistory';
        var fb = $id('btnPdFooterHistory');
        fb.querySelector('span').textContent = onHist ? 'Form' : 'History';
        fb.querySelector('i').className = onHist ? 'fa fa-file-text-o' : 'fa fa-history';
        /* tabControl1_SelectedIndexChanged: the History tab loads the last 50. */
        if (onHist) historyFill(50);
    }
    function onHistory() { return $id('pdHistory').classList.contains('is-active'); }
    function toggleHistory() { tab(onHistory() ? 'pdForm' : 'pdHistory'); }

    // ------------------------------------------------------------------------------ load

    function load() {
        return getJson(API + '/setup').then(function (d) {
            d = d || {};
            if (d.permissions) PERM = { Save: d.permissions.Save !== false, Update: d.permissions.Update !== false, Delete: d.permissions.Delete !== false, Print: d.permissions.Print !== false };
            $id('btnsave').disabled = !PERM.Save;
            $id('btnupdate').disabled = !PERM.Update;
            bind('cmbSupCust', d.customers, 'Id', 'CompanyName');
            bind('cmbdeliverytermnew', d.deliveryTerms, 'Id', 'Code');
            bind('cmbLoadingPort', d.ports, 'Id', 'PortName');
            bind('cmbDestinationPort', d.ports, 'Id', 'PortName');
            bind('cmbfcycode', d.currencies, 'Id', 'CurrencyName');
            bind('cmbcareiertype', CARRIER, 'Id', 'Type');
            var s = $id('cmbcareiertype'); if (s.options.length > 1) s.selectedIndex = 1;
            L.warehouses = d.warehouses || []; L.jobLots = d.jobLots || [];
            bind('Cmbinvoiceno', d.invoices, 'Id', 'InvoiceNo', false);
            setText('txtDocDate', today());
            render();
            $id('pdFooterInfo').textContent = 'PackingDetailForCommercialInvoice  -  Document Type 211';
            focus('Cmbinvoiceno');
        }).catch(function (e) { box(e.message || 'Error occurred during database call.'); });
    }

    function fillHeader(h) {
        setText('txtdocno', str(h.DocCode));
        setText('txtDocDate', isoDate(h.DocDate));
        setVal('cmbSupCust', h.SupplierCustomerId);
        setVal('cmbDestinationPort', h.DestinationPortId);
        setText('txtLotRef', h.LotNoRef);
        setVal('cmbdeliverytermnew', h.DeliveryTermId);
        setVal('cmbLoadingPort', h.LoadingPortId);
        var ct = null; CARRIER.forEach(function (c) { if (c.Type === str(h.CarierType)) ct = c.Id; }); setVal('cmbcareiertype', ct);
        setVal('cmbfcycode', h.FcurrencyId);
        setText('txtGrossWeight', h.GrossWeight);
        setText('txtNetWeight', h.NetWeight);
        setText('txtNoOfContainer', h.NoOfContainers);
        setText('txtGdNo', h.EFormNo);
        setText('txtIformDate', isoDate(h.EFormDate));
    }

    /** Cmbinvoiceno_TextChanged. */
    function invoiceChanged() {
        var id = netI(val('Cmbinvoiceno'));
        if (id !== 0 && !$id('Cmbinvoiceno').disabled) {
            show('panel3', true);
            return getJson(API + '/invoice?id=' + id).then(function (d) {
                d = d || {};
                S.invoiceId = id;
                fillHeader(d.header || {});
                setText('txttotalQtyHeaderForConditions', d.totalQty);
                S.rows = d.rows || []; S.cur = S.rows.length ? 0 : -1;
                render();
            }).catch(function (e) { box(e.message); });
        }
        if (id === 0) { S.rows = []; render(); show('panel3', false); }
        return Promise.resolve();
    }

    // ------------------------------------------------------------------------------ grid

    var COLS = ['ItemName', 'PackType', 'CropYear', 'WarehouseId', 'JobLotId', 'OuterQty', 'OuterPackUOM', 'NetWeight', 'PackingWeight', 'PackingWeightTotal', 'GrossWeight', 'ContainerNo', 'SealNo'];
    var FMT = { OuterQty: '#,##0.##', NetWeight: '#,##0.##', GrossWeight: '#,##0.###', PackingWeight: '#,##0.##', PackingWeightTotal: '#,##0.##' };
    var TOT = { OuterQty: '#,##0.###', NetWeight: '#,##0.##', GrossWeight: '#,##0.###', PackingWeight: '#,##0.##', PackingWeightTotal: '#,##0.##' };
    var EDIT_NUM = { OuterQty: 1, PackingWeight: 1, PackingWeightTotal: 1 };
    var EDIT_TXT = { ContainerNo: 1, SealNo: 1 };
    function options(list, valueCol, textCol, cur) {
        var h = '<option value=""></option>';
        list.forEach(function (r) { var v = str(r[valueCol]); h += '<option value="' + esc(v) + '"' + (netI(v) === netI(cur) ? ' selected' : '') + '>' + esc(r[textCol]) + '</option>'; });
        return h;
    }
    function render() {
        var sums = {};
        $id('pdBody').innerHTML = S.rows.map(function (r, i) {
            var h = '<tr data-i="' + i + '"' + (i === S.cur ? ' class="is-current"' : '') + '>';
            h += '<td class="win-cell-btn"><button type="button" class="win-x" data-act="Delete" data-i="' + i + '" title="Delete">X</button></td>';
            h += '<td class="win-cell-btn"><button type="button" class="win-edit" data-act="Add" data-i="' + i + '">Add</button></td>';
            COLS.forEach(function (c) {
                var v = r[c];
                if (TOT[c]) sums[c] = (sums[c] || 0) + netD(v);
                if (c === 'WarehouseId') h += '<td class="win-editable"><select class="win-combo dtcombo" data-dtcombo="single" data-c="WarehouseId" data-i="' + i + '">' + options(L.warehouses, 'Id', 'WareHouseName', v) + '</select></td>';
                else if (c === 'JobLotId') h += '<td class="win-editable"><select class="win-combo dtcombo" data-dtcombo="single" data-c="JobLotId" data-i="' + i + '">' + options(L.jobLots, 'Id', 'JobLotDescription', v) + '</select></td>';
                else if (EDIT_NUM[c]) h += '<td class="win-editable"><input type="text" class="num" data-guard="decimal" data-c="' + c + '" data-i="' + i + '" value="' + esc(clr(v)) + '"/></td>';
                else if (EDIT_TXT[c]) h += '<td class="win-editable"><input type="text" data-c="' + c + '" data-i="' + i + '" value="' + esc(str(v)) + '"/></td>';
                else if (c === 'ItemName') h += '<td><a class="win-code" data-i="' + i + '" href="javascript:void(0)">' + esc(v) + '</a></td>';
                else h += '<td' + (FMT[c] ? ' class="num"' : '') + '>' + esc(FMT[c] ? fmtNet(v, FMT[c]) : str(v)) + '</td>';
            });
            return h + '</tr>';
        }).join('');
        var foot = $id('pdFoot');
        if (!S.rows.length) foot.innerHTML = '';
        else {
            var t = '<tr><td class="lbl" colspan="2">&Sigma;</td>';
            COLS.forEach(function (c) { t += '<td' + (TOT[c] ? '' : ' class="lbl"') + '>' + (TOT[c] ? esc(fmtNet(sums[c] || 0, TOT[c])) : '') + '</td>'; });
            foot.innerHTML = t + '</tr>';
        }
        refreshCombos();
    }

    /** grdDetail_CellUpdated - the branches reachable from the editable columns (NetWeight / GrossWeight are EditType 0 on the desktop). */
    function cellUpdated(i, c, raw) {
        var r = S.rows[i]; if (!r) return;
        if (c === 'WarehouseId' || c === 'JobLotId') { r[c] = netI(raw); return; }
        if (EDIT_TXT[c]) { r[c] = raw; return; }
        var otherQty = 0;
        S.rows.forEach(function (x, k) { if (k !== i) otherQty += netD(x.OuterQty); });
        var regularQty = netD(val('txttotalQtyHeaderForConditions')) - otherQty;
        r[c] = netD(raw);
        if (c === 'PackingWeight' || c === 'OuterQty') {
            var eq = netD(r.OuterEquivalent), pw = netD(r.PackingWeight), q = netD(r.OuterQty);
            if (q > regularQty) {
                box('Qty Of this row can not be Greater than total Balance Qty');
                q = regularQty;
                r.OuterQty = roundEven(regularQty, 2);
            } else r.OuterQty = roundEven(q, 2);
            var pwt = q * pw;
            r.NetWeight = netD(clr(q * eq));
            r.PackingWeightTotal = pwt;
            r.GrossWeight = netD(clr(netD(r.NetWeight) + pwt));
        }
        if (c === 'PackingWeightTotal') {
            var net = netD(r.NetWeight), t = netD(r.PackingWeightTotal), oq = netD(r.OuterQty);
            r.GrossWeight = netD(clr(net + t));
            r.PackingWeight = netD(clr(t / oq));
        }
        render();
    }

    /** grdDetail_ColumnButtonClick. */
    function columnButton(act, i) {
        S.cur = i;
        if (act === 'Delete') {
            if (visible('btnsave') && !$id('btnsave').disabled) {
                if (!ask('Are you sure to Delete the row?')) return;
                S.rows.splice(i, 1);
                S.cur = -1;
                render();
            } else box('Can not Delete Row in Update Mode...');
        }
        if (act === 'Add') addRowInGrid(i);
    }
    /** AddRowInGrid: a new row with the invoice's balance quantity / weights, copied from the current row. */
    function addRowInGrid(i) {
        var cur = S.rows[i]; if (!cur) return;
        var tg = netD(val('txtGrossWeight')), tq = netD(val('txttotalQtyHeaderForConditions')), tn = netD(val('txtNetWeight'));
        var g = 0, n = 0, q = 0;
        S.rows.forEach(function (r) { g += netD(r.GrossWeight); n += netD(r.NetWeight); q += netD(r.OuterQty); });
        var gross = tg - g, net = tn - n, qty = tq - q;
        if ((gross > 0 && net > 0) || qty > 0) {
            if (gross > 0) {
                var row = {};
                for (var k in cur) if (Object.prototype.hasOwnProperty.call(cur, k)) row[k] = cur[k];
                row.Id = 0;
                row.OuterQty = roundEven(qty, 2);
                row.NetWeight = net;
                row.PackingWeight = netD(cur.PackingWeight);
                row.PackingWeightTotal = netD(cur.PackingWeight) * qty;
                row.GrossWeight = gross;
                S.rows.push(row);
                render();
            } else box('Please Check Grid GrossWeight and TotalGrossWeight');
        } else box('Please Check Grid GrossWeight and TotalGrossWeight');
    }

    // ------------------------------------------------------------------------------ reset / read / save / delete

    function formReset() {
        setEnabled('Cmbinvoiceno', true);
        show('btnsave', true); show('btnupdate', false); show('btnDelete', false);
        S.updateMode = false; S.recId = 0; S.invoiceId = 0;
        var s = $id('cmbcareiertype'); if (s.options.length > 1) s.selectedIndex = 1;
        setText('txtDocDate', today());
        ['cmbSupCust', 'cmbdeliverytermnew', 'cmbLoadingPort', 'cmbDestinationPort', 'cmbfcycode'].forEach(function (id) { setVal(id, ''); });
        ['txtLotRef', 'txtGrossWeight', 'txtNetWeight', 'txtNoOfContainer', 'txtGdNo'].forEach(function (id) { setText(id, ''); });
        S.rows = []; S.cur = -1; render();
        show('panel3', false);
        focus('Cmbinvoiceno');
        return getJson(API + '/refresh').then(function (rows) { bind('Cmbinvoiceno', rows, 'Id', 'InvoiceNo', false); }).catch(function (e) { box(e.message); });
    }
    function btnNew() { return formReset(); }
    function btnRefresh(btn) {
        return busy(btn, function () {
            return getJson(API + '/refresh').then(function (rows) { bind('Cmbinvoiceno', rows, 'Id', 'InvoiceNo'); }).catch(function (e) { box(e.message); });
        });
    }
    /** ReadById(Id). */
    function readById(id) {
        return getJson(API + '/by-id?id=' + id).then(function (d) {
            d = d || {};
            var h = d.header || {};
            show('btnsave', false); show('btnupdate', true);
            S.canDelete = !!d.canDelete; show('btnDelete', S.canDelete); $id('btnDelete').disabled = !S.canDelete;
            S.recId = id; S.invoiceId = id;
            bind('Cmbinvoiceno', [{ Id: h.Id, InvoiceNo: h.InvoiceNo }], 'Id', 'InvoiceNo', false);
            tab('pdForm');
            setEnabled('Cmbinvoiceno', false);
            setVal('Cmbinvoiceno', h.Id);
            fillHeader(h);
            show('panel3', true);
            setText('txttotalQtyHeaderForConditions', d.totalQty);
            S.rows = d.rows || []; S.cur = S.rows.length ? 0 : -1;
            render();
        }).catch(function (e) { box(e.message); });
    }
    /** Insert(). */
    function insert(btn) {
        return busy(btn, function () {
            if (!S.rows.length) { box('Grid Record not found'); return Promise.resolve(); }
            if (!ask(S.recId === 0 ? 'Are you sure to Save?' : 'Are you sure to Update?')) return Promise.resolve();
            var invoiceId = netI(val('Cmbinvoiceno'));
            var win = $id('ChkPrintPreview').checked && global.CrystalPrint ? global.CrystalPrint.reserve() : null;
            return postJson(API + '/save', { recId: S.recId, invoiceId: invoiceId, rows: S.rows }).then(function (d) {
                box((d && d.message) || (S.recId === 0 ? 'Save SuccessFully' : 'Update SuccessFully'));
                return formReset().then(function () {
                    if (win) global.CrystalPrint.open('550-commercialinvoicepackingdetaillist-slip', { id: invoiceId }, null, win);
                });
            }).catch(function (e) { if (win) global.CrystalPrint.release(win); box(e.message); });
        });
    }
    function btnsave(btn) { S.recId = 0; return insert(btn); }
    function btnupdate(btn) { return insert(btn); }
    /** btnDelete_Click. */
    function btnDelete(btn) {
        return busy(btn, function () {
            if (S.recId <= 0) { box('Record Not Found For Deletion'); return Promise.resolve(); }
            if (!ask('Are you sure to Delete?')) return Promise.resolve();
            return postJson(API + '/delete', { recId: S.recId }).then(function (d) {
                box((d && d.message) || 'Delete Export Packing list Successfully');
                return formReset();
            }).catch(function (e) { box(e.message); });
        });
    }
    /** btnPrint_Click: ExportCommercialInvoicePackingdetailListSlip(RecId). */
    function print(btn) {
        if (!global.CrystalPrint) { box('Printing is not available on this page.'); return; }
        if (S.recId === 0) { box('No Record Found For Display'); return; }
        return global.CrystalPrint.open('550-commercialinvoicepackingdetaillist-slip', { id: S.recId }, btn);
    }

    // ------------------------------------------------------------------------------ history

    var HCOLS = ['InvoiceNo', 'DocCode', 'DocDate', 'CustomerName', 'NoOfContainers', 'GrossWeight', 'NetWeight', 'FcyAmount', 'AddLess', 'TotalAmount', 'LoadingPort', 'DestinationPort'];
    var HFMT = { NoOfContainers: '0,0', AddLess: '0,0', GrossWeight: '#,#.###', NetWeight: '#,#.###', FcyAmount: '#,#.###', TotalAmount: '#,#.###' };
    function historyFill(n, btn) {
        var run = function () {
            return getJson(API + '/history?noOfRecords=' + n).then(function (rows) { HIST = rows || []; HCUR = -1; histRender(); }).catch(function (e) { box(e.message); });
        };
        return btn ? busy(btn, run) : run();
    }
    function histRender() {
        var sums = {};
        $id('pdHistBody').innerHTML = HIST.map(function (r, i) {
            var h = '<tr data-i="' + i + '"' + (i === HCUR ? ' class="is-current"' : '') + '>';
            h += '<td class="win-cell-btn"><button type="button" class="win-edit" data-hact="Edit" data-i="' + i + '">Edit</button></td>';
            h += '<td class="win-cell-btn"><button type="button" class="win-edit" data-hact="Slip" data-i="' + i + '">Slip</button></td>';
            HCOLS.forEach(function (c) {
                var v = r[c], inner;
                if (HFMT[c]) { sums[c] = (sums[c] || 0) + netD(v); inner = fmtNet(v, HFMT[c]); }
                else if (c === 'DocDate') inner = shortDate(v);
                else inner = str(v);
                inner = esc(inner);
                if (c === 'InvoiceNo') inner = '<a class="win-code" data-i="' + i + '" href="javascript:void(0)">' + inner + '</a>';
                h += '<td' + (HFMT[c] || c === 'DocCode' ? ' class="num"' : '') + '>' + inner + '</td>';
            });
            return h + '</tr>';
        }).join('');
        var foot = $id('pdHistFoot');
        if (!HIST.length) foot.innerHTML = '';
        else {
            var t = '<tr><td class="lbl" colspan="2">&Sigma;</td>';
            HCOLS.forEach(function (c) { t += '<td' + (HFMT[c] ? '' : ' class="lbl"') + '>' + (HFMT[c] ? esc(fmtNet(sums[c] || 0, HFMT[c])) : '') + '</td>'; });
            foot.innerHTML = t + '</tr>';
        }
        show('pdHistEmpty', HIST.length === 0);
        $id('pdHistDetailBody').innerHTML = '';
    }
    function historySelect(i) {
        HCUR = i;
        var r = HIST[i]; if (!r) return;
        getJson(API + '/history-detail?id=' + netI(r.Id)).then(function (rows) {
            $id('pdHistDetailBody').innerHTML = (rows || []).map(function (d) {
                return '<tr><td>' + esc(d.ItemName) + '</td><td>' + esc(d.PackType) + '</td><td>' + esc(d.CropYear) + '</td><td>' + esc(d.WarehouseName) + '</td><td>' + esc(d.JobLot)
                    + '</td><td class="num">' + esc(clr(d.OuterQty)) + '</td><td>' + esc(d.OuterPackUOM) + '</td><td class="num">' + esc(clr(d.NetWeight)) + '</td><td class="num">'
                    + esc(clr(d.PackingWeight)) + '</td><td class="num">' + esc(clr(d.PackingWeightTotal)) + '</td><td class="num">' + esc(clr(d.GrossWeight)) + '</td><td>'
                    + esc(d.ContainerNo) + '</td><td>' + esc(d.SealNo) + '</td></tr>';
            }).join('');
        }).catch(function (e) { box(e.message); });
    }
    function historyButton(act, i, btn) {
        var r = HIST[i]; if (!r) return;
        if (act === 'Edit') { readById(netI(r.Id)); return; }
        if (act === 'Slip') {
            if (!global.CrystalPrint) { box('Printing is not available on this page.'); return; }
            global.CrystalPrint.open('550-commercialinvoicepackingdetaillist-slip', { id: netI(r.Id) }, btn);
        }
    }

    // ------------------------------------------------------------------------------ wiring

    document.addEventListener('DOMContentLoaded', function () {
        document.querySelectorAll('.win-tabs[data-tabs="pd"] .win-tab').forEach(function (b) { b.addEventListener('click', function () { tab(b.getAttribute('data-tab')); }); });
        document.querySelectorAll('button[data-fullscreen]').forEach(function (b) {
            b.addEventListener('click', function () { var bx = $id(b.getAttribute('data-fullscreen')); if (bx) bx.classList.toggle('ex-fullscreen'); });
        });
        $id('Cmbinvoiceno').addEventListener('change', invoiceChanged);
        var gb = $id('pdBody');
        gb.addEventListener('click', function (e) {
            var b = e.target.closest('button[data-act]');
            if (b) { columnButton(b.getAttribute('data-act'), +b.getAttribute('data-i')); return; }
            var tr = e.target.closest('tr[data-i]'); if (!tr) return;
            S.cur = +tr.getAttribute('data-i');
            gb.querySelectorAll('tr[data-i]').forEach(function (x) { x.classList.toggle('is-current', x === tr); });
        });
        gb.addEventListener('change', function (e) {
            var el = e.target.closest('[data-c]'); if (!el) return;
            cellUpdated(+el.getAttribute('data-i'), el.getAttribute('data-c'), el.value);
        });
        var hb = $id('pdHistBody');
        hb.addEventListener('click', function (e) {
            var b = e.target.closest('button[data-hact]');
            if (b) { historyButton(b.getAttribute('data-hact'), +b.getAttribute('data-i'), b); return; }
            var a = e.target.closest('a.win-code');
            if (a) { historyButton('Edit', +a.getAttribute('data-i')); return; }
            var tr = e.target.closest('tr[data-i]'); if (!tr) return;
            hb.querySelectorAll('tr[data-i]').forEach(function (x) { x.classList.toggle('is-current', x === tr); });
            historySelect(+tr.getAttribute('data-i'));
        });
        hb.addEventListener('dblclick', function (e) { var tr = e.target.closest('tr[data-i]'); if (tr && !e.target.closest('button')) readById(netI(HIST[+tr.getAttribute('data-i')].Id)); });
        /* PackingDetailForCommercialInvoice_KeyDown */
        document.addEventListener('keydown', function (e) {
            var k = (e.key || '').toLowerCase();
            if (e.key === 'Enter' && !e.ctrlKey && e.target && /^(INPUT|SELECT)$/.test(e.target.tagName) && e.target.type !== 'checkbox') {
                var f = Array.prototype.filter.call(document.querySelectorAll('input:not([type=hidden]):not(:disabled), select:not(:disabled), button:not(:disabled)'), function (x) { return x.offsetParent !== null; });
                var i = f.indexOf(e.target);
                if (i >= 0 && i + 1 < f.length) { e.preventDefault(); f[i + 1].focus(); }
                return;
            }
            if (e.ctrlKey && k === 't') { e.preventDefault(); toggleHistory(); return; }
            if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { e.preventDefault(); cancel(); return; }
            if (onHistory()) return;
            if (e.ctrlKey && k === 's' && !S.updateMode) { e.preventDefault(); insert(visible('btnsave') ? $id('btnsave') : $id('btnupdate')); return; }
            if (e.ctrlKey && k === 'n') { e.preventDefault(); formReset(); return; }
            if (e.ctrlKey && k === 'u') { e.preventDefault(); insert(visible('btnupdate') ? $id('btnupdate') : $id('btnsave')); }
        });
        load();
    });

    global.ExportPd = {
        btnNew: btnNew, btnsave: btnsave, btnupdate: btnupdate, btnRefresh: btnRefresh, btnDelete: btnDelete, print: print,
        toggleHistory: toggleHistory, loadAll: function (btn) { return historyFill(0, btn); }
    };
}(window));
