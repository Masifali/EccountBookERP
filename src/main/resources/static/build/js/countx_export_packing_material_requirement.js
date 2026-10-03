/* ============================================================================================
 * countx_export_packing_material_requirement.js - PackingMaterialRequirement.cs (Architecture.WinApp.Export),
 * screen 219. Every desktop event has its counterpart with the desktop's messages and order:
 *   Load, CmbCustomerName_Leave, CmbSalesContract_Leave, grdSalesContractDetail_ColumnButtonClick ("Load"),
 *   cmbitem_ValueChanged, Add_Click (+), grdPMRequirement_DoubleClick, btnUpdateDetail_Click, btnCancelDetail_Click,
 *   btnNew_Click, btnSave_Click -> Insert, tabControl1_SelectedIndexChanged -> HistoryBind, grdhistory_DoubleClick.
 * The two DataTables of the form ("table" = grdPMRequirement, "dtmain" = grdMain) live here as arrays.
 * ============================================================================================ */
(function (global) {
    'use strict';

    var API = '/api/export/packing-material-requirement';

    function $id(id) { return document.getElementById(id); }
    function box(m) { global.alert(m); }
    function ask(m) { return global.confirm(m); }
    function esc(s) { return String(s === null || s === undefined ? '' : s).replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;'); }
    function str(v) { return v === null || v === undefined ? '' : String(v); }
    function netD(v) { var n = parseFloat(String(v === null || v === undefined ? '' : v).replace(/,/g, '')); return isFinite(n) ? n : 0; }
    function netI(v) { var n = parseInt(String(v === null || v === undefined ? '' : v), 10); return isFinite(n) ? n : 0; }
    /* FormatString "#,#" */
    function fmtInt(v) { var n = Math.round(netD(v)); return n === 0 ? '' : String(n).replace(/\B(?=(\d{3})+(?!\d))/g, ','); }
    function fmt(v) { var n = netD(v); var s = String(Math.round(n * 1000) / 1000); var p = s.split('.'); p[0] = p[0].replace(/\B(?=(\d{3})+(?!\d))/g, ','); return p.join('.'); }
    function busy(btn, fn) {
        var b = (typeof btn === 'string') ? $id(btn) : btn;
        if (b) { if (b.disabled || b.classList.contains('is-busy')) return Promise.resolve(); b.disabled = true; b.classList.add('is-busy'); }
        var done = function () { if (b) { b.disabled = false; b.classList.remove('is-busy'); } };
        var p;
        try { p = fn(); } catch (e) { done(); box(e.message); return Promise.resolve(); }
        if (p && typeof p.then === 'function') return p.then(done, function (e) { done(); if (e && e.message) box(e.message); });
        done(); return Promise.resolve(p);
    }
    function parse(r) {
        return r.text().then(function (t) {
            var b = null; try { b = t ? JSON.parse(t) : null; } catch (e) { /* not json */ }
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
    function bind(id, rows) {
        var s = $id(id); if (!s) return;
        var h = '<option value="0"></option>';
        (rows || []).forEach(function (r) { h += '<option value="' + esc(r.Id) + '">' + esc(r.name) + '</option>'; });
        s.innerHTML = h; s.value = '0'; refreshCombos();
    }
    function setVal(id, v) { var s = $id(id); if (!s) return; s.value = str(v); if (s.value !== str(v)) s.value = '0'; refreshCombos(); }
    function val(id) { var e = $id(id); return e ? e.value : ''; }
    function setText(id, v) { var e = $id(id); if (e) e.value = str(v); }
    function comboText(id) { var s = $id(id); if (!s || s.selectedIndex < 0) return ''; var o = s.options[s.selectedIndex]; return o && o.value !== '0' ? o.text : ''; }
    function setEnabled(id, on) { var e = $id(id); if (e) { e.disabled = !on; refreshCombos(); } }
    function focus(id) { var e = $id(id); if (!e) return; var w = e.closest && e.closest('.dtcombo-wrap'); var t = w ? w.querySelector('.dtcombo-input') : e; try { (t || e).focus(); } catch (x) { /* ignore */ } }

    // ------------------------------------------------------------------------------ state
    var PERM = { Save: true };
    var S = {
        table: [], dtmain: [], contractRows: [],
        mainItem: null,       /* CmbMainItem: {id, name} - bound only through Load / double-click */
        itemUom: null,        /* cmbItemUOM:  {id, name} */
        rowIndexGrd: -1, pendingPmUom: null
    };
    var HIST = [];

    // ------------------------------------------------------------------------------ tabs
    function tab(panelId) {
        document.querySelectorAll('.win-tabs .win-tab').forEach(function (b) { b.classList.toggle('is-active', b.getAttribute('data-tab') === panelId); });
        ['pmrForm', 'pmrHistory'].forEach(function (p) { $id(p).classList.toggle('is-active', p === panelId); });
        var onHist = panelId === 'pmrHistory';
        $id('btnFooterHistory').querySelector('span').textContent = onHist ? 'Form' : 'History';
        if (onHist) historyBind();
    }
    function onHistory() { return $id('pmrHistory').classList.contains('is-active'); }
    function toggleHistory() { tab(onHistory() ? 'pmrForm' : 'pmrHistory'); }

    // ------------------------------------------------------------------------------ grids
    var COLS = ['ItemName', 'PackUom', 'Qty', 'PackingItem', 'PmPackUom', 'ReqQty'];
    function drawPm(bodyId, footId, rows) {
        var sum = 0;
        $id(bodyId).innerHTML = rows.map(function (r, i) {
            sum += netD(r.ReqQty);
            return '<tr data-i="' + i + '">' + COLS.map(function (c) {
                if (c === 'ReqQty') return '<td class="num">' + esc(fmtInt(r[c])) + '</td>';
                if (c === 'Qty') return '<td class="num">' + esc(fmt(r[c])) + '</td>';
                return '<td>' + esc(str(r[c])) + '</td>';
            }).join('') + '</tr>';
        }).join('');
        $id(footId).innerHTML = rows.length ? '<tr><td class="lbl">&Sigma;</td><td class="lbl"></td><td class="lbl"></td><td class="lbl"></td><td class="lbl"></td><td>' + esc(fmtInt(sum)) + '</td></tr>' : '';
    }
    var SC_COLS = ['ContractDetailId', 'ItemName', 'ItemCode', 'PackSize', 'InvPackingMaterialTypeId', 'PackType', 'CropYearId', 'CropYear', 'Qty', 'MTon', 'RatePrice', 'IsApproved'];
    var SC_NUM = { ContractDetailId: 1, InvPackingMaterialTypeId: 1, CropYearId: 1, MTon: 1, RatePrice: 1 };
    function drawContract() {
        var sum = 0, rows = S.contractRows;
        $id('scBody').innerHTML = rows.map(function (r, i) {
            sum += netD(r.Qty);
            return '<tr data-i="' + i + '">' + SC_COLS.map(function (c) {
                var v = r[c];
                if (c === 'Qty') return '<td class="num">' + esc(fmtInt(v)) + '</td>';
                if (SC_NUM[c]) return '<td class="num">' + esc(fmt(v)) + '</td>';
                if (c === 'IsApproved') return '<td class="ctr"><input type="checkbox" disabled' + (v === true || v === 1 ? ' checked' : '') + '/></td>';
                if (c === 'ItemName') return '<td><a class="win-code" href="javascript:void(0)" data-load="' + i + '">' + esc(str(v)) + '</a></td>';
                return '<td>' + esc(str(v)) + '</td>';
            }).join('') + '<td class="win-cell-btn"><button type="button" class="win-edit" data-load="' + i + '">Load</button></td></tr>';
        }).join('');
        $id('scFoot').innerHTML = rows.length ? '<tr>' + SC_COLS.map(function (c, k) { return c === 'Qty' ? '<td>' + esc(fmtInt(sum)) + '</td>' : '<td class="lbl">' + (k === 0 ? '&Sigma;' : '') + '</td>'; }).join('') + '<td class="lbl"></td></tr>' : '';
    }
    function render() { drawPm('pmBody', 'pmFoot', S.table); drawPm('mainBody', 'mainFoot', S.dtmain); }
    function setMain(item, uom, qty) {
        S.mainItem = item; S.itemUom = uom;
        setText('CmbMainItem', item ? item.name : ''); setText('cmbItemUOM', uom ? uom.name : '');
        if (qty !== undefined) setText('txtItemQty', qty);
    }

    // ------------------------------------------------------------------------------ load / leave
    function load() {
        return getJson(API + '/setup').then(function (d) {
            d = d || {};
            if (d.permissions) PERM = d.permissions;
            $id('btnSave').disabled = !PERM.Save;
            if (d.pmItemsError) box(d.pmItemsError);
            if (d.customersError) box(d.customersError);
            if (d.pmItems && d.pmItems.length) bind('cmbPMitem', d.pmItems);
            if (d.customers && d.customers.length) bind('CmbCustomerName', d.customers);
            render();
            show('BtnAdd', true); show('btnUpdateDetail', false); show('btnCancelDetail', false);
            focus('CmbCustomerName');
        }).catch(function (e) { box(e.message || 'Error occurred during database call.'); });
    }
    /** CmbCustomerName_Leave -> SalesContractBind (re-binds only when rows come back). */
    function customerLeave() {
        return getJson(API + '/sales-contracts?supplierCustomerId=' + netI(val('CmbCustomerName'))).then(function (rows) {
            if (rows && rows.length) bind('CmbSalesContract', rows);
        }).catch(function (e) { box(e.message); });
    }
    /** SalesContractDetailByContractId. */
    function loadContractItems(id) {
        return getJson(API + '/contract-items?contractId=' + id).then(function (rows) {
            S.contractRows = rows || []; drawContract();
        }).catch(function (e) { box(e.message); });
    }
    function contractLeave() { return loadContractItems(netI(val('CmbSalesContract'))); }
    /** cmbitem_ValueChanged -> bindRateUomAndItemPackUom (re-binds only when rows come back). */
    function pmItemChanged() {
        return getJson(API + '/uoms?itemId=' + netI(val('cmbPMitem'))).then(function (rows) {
            if (rows && rows.length) bind('cmbPMItemUom', rows);
            if (S.pendingPmUom !== null) { setVal('cmbPMItemUom', S.pendingPmUom); S.pendingPmUom = null; }
        }).catch(function (e) { box(e.message); });
    }

    // ------------------------------------------------------------------------------ Load column
    /** grdSalesContractDetail_ColumnButtonClick("Load"). */
    function loadRow(i) {
        setEnabled('CmbSalesContract', false);
        setMain(null, null, '0');
        focus('cmbPMitem');
        var item = S.contractRows[i]; if (!item) return;
        setMain({ id: item.ItemId, name: item.ItemName }, { id: item.ItemUomId, name: item.PackSize }, str(item.Qty));
        /* rows of other items are deleted; when none was deleted the whole table is cleared; then dtmain's rows of
           this item are appended (rows of this item that survived the delete are therefore duplicated - desktop behaviour) */
        var flag = false;
        for (var k = S.table.length - 1; k >= 0; k--) if (netI(S.table[k].ItemId) !== netI(item.ItemId)) { flag = true; S.table.splice(k, 1); }
        if (!flag) S.table = [];
        S.dtmain.forEach(function (r) { if (netI(r.ItemId) === netI(item.ItemId)) S.table.push(Object.assign({}, r)); });
        render();
    }

    // ------------------------------------------------------------------------------ detail add / edit
    function formValidationDetail() {
        if (netI(val('cmbPMitem')) === 0) { box('Packing Item Field is Required'); focus('cmbPMitem'); return false; }
        if (netI(val('cmbPMItemUom')) === 0) { box('Packing Item Uom Field is Required'); focus('cmbPMItemUom'); return false; }
        if (netD(val('txtReqQty').trim()) === 0) { box('Required Qty Field is Required'); focus('txtReqQty'); return false; }
        return true;
    }
    function resetDetail() { setVal('cmbPMitem', '0'); setVal('cmbPMItemUom', '0'); setText('txtReqQty', ''); }
    /** Add_Click. */
    function add() {
        if (!formValidationDetail()) return;
        if (S.table.length !== 0) {
            var reqQty = 0, itemQty = 0;
            S.table.forEach(function (r) { reqQty += netD(r.ReqQty); });
            reqQty += netD(val('txtReqQty').trim());
            var mid = S.mainItem ? netI(S.mainItem.id) : 0;
            for (var k = 0; k < S.table.length; k++) if (netI(S.table[k].ItemId) === mid) { itemQty = netD(S.table[k].Qty); break; }
            if (reqQty > itemQty) { box('Required Qty cannot be greater than ItemQty'); return; }
        }
        var qtyText = val('txtItemQty').trim();
        if (qtyText === '' || !isFinite(parseFloat(qtyText))) { box('Input string was not in a correct format.Couldn\'t store <' + qtyText + '> in Qty Column.  Expected type is Double.'); return; }
        var row = {
            ItemId: S.mainItem ? S.mainItem.id : null, ItemName: val('CmbMainItem').trim(),
            PackUomId: S.itemUom ? S.itemUom.id : null, PackUom: val('cmbItemUOM').trim(),
            Qty: parseFloat(qtyText), PmItemId: netI(val('cmbPMitem')), PackingItem: comboText('cmbPMitem').trim(),
            PmUomId: netI(val('cmbPMItemUom')), PmPackUom: comboText('cmbPMItemUom').trim(), ReqQty: netD(val('txtReqQty').trim())
        };
        S.table.push(row);
        S.dtmain.push(Object.assign({}, row));
        render(); resetDetail(); focus('cmbPMitem');
    }
    /** grdPMRequirement_DoubleClick. */
    function pmDblClick(i) {
        var r = S.table[i]; if (!r) return;
        S.rowIndexGrd = i;
        setMain({ id: r.ItemId, name: r.ItemName }, { id: r.PackUomId, name: r.PackUom }, str(r.Qty));
        S.pendingPmUom = r.PmUomId;
        setVal('cmbPMitem', r.PmItemId);
        pmItemChanged();
        setText('txtReqQty', str(r.ReqQty));
        show('BtnAdd', false); show('btnUpdateDetail', true); show('btnCancelDetail', true);
        focus('cmbPMitem');
    }
    /** btnUpdateDetail_Click: the table row, then every dtmain row matching the UPDATED row's four keys. */
    function btnUpdateDetail() {
        var r = S.table[S.rowIndexGrd]; if (!r) { box('There is no row at position ' + S.rowIndexGrd + '.'); return; }
        r.ItemId = S.mainItem ? S.mainItem.id : null; r.ItemName = val('CmbMainItem');
        r.PackUomId = S.itemUom ? S.itemUom.id : null; r.PackUom = val('cmbItemUOM');
        r.Qty = netD(val('txtItemQty')); r.PmItemId = netI(val('cmbPMitem')); r.PackingItem = comboText('cmbPMitem');
        r.PmUomId = netI(val('cmbPMItemUom')); r.PmPackUom = comboText('cmbPMItemUom'); r.ReqQty = netD(val('txtReqQty'));
        for (var k = 0; k < S.dtmain.length; k++) {
            var m = S.dtmain[k];
            if (netI(m.ItemId) === netI(r.ItemId) && netI(m.PackUomId) === netI(r.PackUomId) && netI(m.PmItemId) === netI(r.PmItemId) && netI(m.PmUomId) === netI(r.PmUomId))
                S.dtmain[k] = Object.assign({}, r);
        }
        render();
        show('BtnAdd', true); show('btnUpdateDetail', false); show('btnCancelDetail', false);
        resetDetail();
    }
    function btnCancelDetail() { resetDetail(); show('BtnAdd', true); show('btnUpdateDetail', false); show('btnCancelDetail', false); }

    // ------------------------------------------------------------------------------ reset / save / read
    function reset() {
        show('btnSave', true); show('btnUpdate', false);
        S.table = []; S.dtmain = [];
        setEnabled('CmbSalesContract', true);
        setVal('CmbCustomerName', '0'); setVal('CmbSalesContract', '0');
        S.contractRows = []; drawContract(); render();
        show('BtnAdd', true); show('btnUpdateDetail', false); show('btnCancelDetail', false);
    }
    function btnNew() { reset(); }
    /** btnSave_Click -> Insert (no grid validation on the desktop). */
    function btnSave(btn) {
        return busy(btn, function () {
            if (!ask('Are you sure to Save?')) return Promise.resolve();
            return postJson(API + '/save', { salesContractId: netI(val('CmbSalesContract')), rows: S.dtmain }).then(function (d) {
                if (d && d.id > 0) box('Record Saved Successfully');
                reset();
            }).catch(function (e) { box(e.message); });
        });
    }
    /** btnUpdate has no Click handler on the desktop. */
    function btnUpdate() { }
    function readById(id) {
        return getJson(API + '/by-id?id=' + id).then(function (d) {
            show('btnSave', false); show('btnUpdate', true);
            tab('pmrForm');
            setVal('CmbSalesContract', d.ExImLcOrderId);
            S.contractRows = d.contractItems || []; drawContract();
            S.dtmain = d.rows || [];
            render();
        }).catch(function (e) { box(e.message); });
    }
    function historyBind() {
        return getJson(API + '/history').then(function (rows) {
            HIST = rows || [];
            $id('histBody').innerHTML = HIST.map(function (r, i) {
                return '<tr data-i="' + i + '"><td><a class="win-code" href="javascript:void(0)" data-i="' + i + '">' + esc(r.LcOrderNo) + '</a></td><td>' + esc(r.EntryDate) + '</td><td>' + esc(r.UserName) + '</td></tr>';
            }).join('');
            show('histEmpty', HIST.length === 0);
        }).catch(function (e) { box(e.message); });
    }

    // ------------------------------------------------------------------------------ wiring
    function on(id, ev, fn) { var e = $id(id); if (e) e.addEventListener(ev, fn); }
    document.addEventListener('DOMContentLoaded', function () {
        document.querySelectorAll('.win-tabs .win-tab').forEach(function (b) { b.addEventListener('click', function () { tab(b.getAttribute('data-tab')); }); });
        document.querySelectorAll('button[data-fullscreen]').forEach(function (b) { b.addEventListener('click', function () { var bx = $id(b.getAttribute('data-fullscreen')); if (bx) bx.classList.toggle('ex-fullscreen'); }); });
        on('CmbCustomerName', 'change', customerLeave);
        on('CmbSalesContract', 'change', contractLeave);
        on('cmbPMitem', 'change', pmItemChanged);
        $id('scBody').addEventListener('click', function (e) { var b = e.target.closest('[data-load]'); if (b) loadRow(+b.getAttribute('data-load')); });
        var pb = $id('pmBody');
        pb.addEventListener('dblclick', function (e) { var tr = e.target.closest('tr[data-i]'); if (tr) pmDblClick(+tr.getAttribute('data-i')); });
        pb.addEventListener('click', function (e) { var tr = e.target.closest('tr[data-i]'); if (!tr) return; pb.querySelectorAll('tr').forEach(function (x) { x.classList.toggle('is-current', x === tr); }); });
        var hb = $id('histBody');
        hb.addEventListener('click', function (e) { var a = e.target.closest('a.win-code'); if (a) { var r = HIST[+a.getAttribute('data-i')]; if (r) readById(r.Id); } });
        hb.addEventListener('dblclick', function (e) { var tr = e.target.closest('tr[data-i]'); if (tr) { var r = HIST[+tr.getAttribute('data-i')]; if (r) readById(r.Id); } });
        load();
    });

    global.ExportPmr = { btnNew: btnNew, btnSave: btnSave, btnUpdate: btnUpdate, add: add, btnUpdateDetail: btnUpdateDetail, btnCancelDetail: btnCancelDetail, toggleHistory: toggleHistory };
}(window));
