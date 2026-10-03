/* ============================================================================================
 * countx_export_delivery_order.js - ExportDeliveryOrderB.cs (Architecture.WinApp.Export), screen 208
 * "Export Delivery Order" (DocumentTypeId 84, DeliveryOrderType "Export") with its Load Invoices dialog
 * (LoadCommercialInvoiceForDO.cs). Every button, TextChanged, Leave, grid CellUpdated / column button /
 * double-click and KeyDown of the desktop form has its counterpart here, with the desktop's messages and
 * order. Data: /api/export/delivery-order (ExportDeliveryOrderController -> ExportDeliveryOrderService ->
 * ExportDeliveryOrderRepository, the procedures of the desktop BLL/DAL).
 * ========================================================================================== */
(function (global) {
    'use strict';

    var API = '/api/export/delivery-order';
    var DOC_TYPE = 84;

    // ------------------------------------------------------------------------------ helpers
    function $id(id) { return document.getElementById(id); }
    function box(m) { if (m) global.alert(m); }
    function ask(m) { return global.confirm(m); }
    function esc(s) { return String(s === null || s === undefined ? '' : s).replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;').replace(/'/g, '&#39;'); }
    function str(v) { return v === null || v === undefined ? '' : String(v); }
    function netI(v) { var n = parseInt(String(v === null || v === undefined ? '' : v).replace(/,/g, ''), 10); return isFinite(n) ? n : 0; }
    function netD(v) { var n = parseFloat(String(v === null || v === undefined ? '' : v).replace(/,/g, '')); return isFinite(n) ? n : 0; }
    function fmt(v, dec) {
        var n = netD(v); if (dec === undefined) dec = 3;
        var s = n.toFixed(dec); if (dec > 0) s = s.replace(/\.?0+$/, '');
        var p = s.split('.'); p[0] = p[0].replace(/\B(?=(\d{3})+(?!\d))/g, ','); return p.join('.');
    }
    /* double.ToString() - plain text, at most 15 significant digits. */
    function raw(v) { var n = netD(v); return String(parseFloat(n.toPrecision(15))); }
    function rawText(v) { return v === null || v === undefined ? '' : String(v); }
    function pad(n) { return (n < 10 ? '0' : '') + n; }
    function today() { var d = new Date(); return d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()); }
    function daysAgo(n) { var d = new Date(); d.setDate(d.getDate() - n); return d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()); }
    function isoDate(v) { var s = str(v).trim(); var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(s); return m ? m[1] + '-' + m[2] + '-' + m[3] : ''; }
    function shortDate(v) { var s = isoDate(v); return s ? parseInt(s.substring(5, 7), 10) + '/' + parseInt(s.substring(8, 10), 10) + '/' + s.substring(0, 4) : ''; }
    var MON = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    /* "dd-MMM-yy hh:mm tt" */
    function ddMMMyyTime(v) {
        var s = str(v); var m = /^(\d{4})-(\d{2})-(\d{2})T?(\d{2})?:?(\d{2})?/.exec(s); if (!m) return '';
        var h = netI(m[4] || 0), ap = h >= 12 ? 'PM' : 'AM', h12 = h % 12 === 0 ? 12 : h % 12;
        return m[3] + '-' + MON[netI(m[2]) - 1] + '-' + m[1].substring(2) + ' ' + pad(h12) + ':' + (m[5] || '00') + ' ' + ap;
    }
    function busy(btn, fn) {
        var b = (typeof btn === 'string') ? $id(btn) : btn;
        if (b) { if (b.disabled || b.classList.contains('is-busy')) return Promise.resolve(); b.disabled = true; b.classList.add('is-busy'); }
        var done = function () { if (b) { b.disabled = false; b.classList.remove('is-busy'); applyRights(); } };
        var p; try { p = fn(); } catch (e) { done(); box(e.message); return Promise.resolve(); }
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
    function visible(id) { var e = $id(id); return !!e && !e.classList.contains('is-hidden'); }
    function col(row, name) {
        if (!row) return null;
        if (Object.prototype.hasOwnProperty.call(row, name)) return row[name];
        var l = name.toLowerCase();
        for (var k in row) if (Object.prototype.hasOwnProperty.call(row, k) && k.toLowerCase() === l) return row[k];
        return null;
    }
    /* DDL.BindDDL(dt, cmb, value, display, caption, ZeroIndex) - a blank first row; previous value kept when still listed. */
    function bind(id, rows, valueCol, textCol, extraCols, keepValue) {
        var s = $id(id); if (!s) return;
        var keep = keepValue === undefined ? s.value : str(keepValue);
        var h = '<option value=""></option>';
        (rows || []).forEach(function (r) {
            var extra = (extraCols || []).map(function (k) { return str(col(r, k)).replace(/\|/g, '/'); }).join('|');
            h += '<option value="' + esc(col(r, valueCol)) + '" data-extra="' + esc(extra) + '">' + esc(col(r, textCol)) + '</option>';
        });
        s.innerHTML = h; s.value = keep; if (s.value !== keep) s.value = '';
        refreshCombos();
    }
    function setVal(id, v) { var s = $id(id); if (!s) return; s.value = str(v); if (s.value !== str(v)) s.value = ''; refreshCombos(); }
    function val(id) { var e = $id(id); return e ? e.value : ''; }
    function setText(id, v) { var e = $id(id); if (e) e.value = str(v); }
    function selText(id) { var s = $id(id); return s && s.selectedIndex > 0 ? s.options[s.selectedIndex].textContent.trim() : ''; }
    function hasSel(id) { var s = $id(id); return !!s && s.value !== '' && s.value !== '0'; }
    function selExtra(id, i) { var s = $id(id); if (!s || s.selectedIndex <= 0) return ''; return str(s.options[s.selectedIndex].getAttribute('data-extra')).split('|')[i] || ''; }
    function enable(id, on) { var e = $id(id); if (e) { e.disabled = !on; refreshCombos(); } }
    function focus(id) { var e = $id(id); if (!e) return; var w = e.closest && e.closest('.dtcombo-wrap'); var t = w ? w.querySelector('.dtcombo-input') : e; try { (t || e).focus(); } catch (x) { /* ignore */ } }
    function cancel() { global.close(); setTimeout(function () { if (!global.closed) { if (history.length > 1) history.back(); else location.href = '/export'; } }, 150); }
    function errs(d, keys) { keys.forEach(function (k) { if (d && d[k + 'Error']) box(d[k + 'Error']); }); }


    // ------------------------------------------------------------------------------ state
    var PERM = { Save: true, Update: true, Print: true };
    var CFG = {};
    var S = {
        recId: 0, userBranchId: 0, branchCount: 0,
        rows: [], removed: [], others: [], updateIndex: -1, updateIndexOther: -1,
        wh: [], jobs: [], packs: [], otherCombo: [], items: [], uomM: [], uomI: [], innerBound: false,
        containers: [], contRead: [], hist: [], curHist: -1, curRow: -1, curOther: -1,
        ebMU: false, ebMT: false, ebIU: false, ebIT: false,
        ld: { data: [], grid: [], restrict: false, fromDate: '', loader: [] }
    };

    function applyRights() {
        $id('btnsave').disabled = !PERM.Save;
        $id('btnupdate').disabled = !PERM.Update;
        $id('btnprint').disabled = !PERM.Print;
        $id('btnExportDoSlip396').disabled = !PERM.Print;
    }

    /* double.ToString(fmt) incl. the infinity / NaN texts .NET writes for a division by zero. */
    function nf(x, dec) { if (isNaN(x)) return 'NaN'; if (!isFinite(x)) return x > 0 ? '∞' : '-∞'; return fmt(x, dec); }
    function f2(x) { return nf(x, 2); }
    function f3(x) { return nf(x, 3); }
    function f4(x) { return nf(x, 4); }
    /* Math.Round(decimal, 2) - banker's rounding. */
    function round2(x) { var v = x * 100, f = Math.floor(v), d = v - f; if (Math.abs(d - 0.5) < 1e-9) return (f % 2 === 0 ? f : f + 1) / 100; return Math.round(v) / 100; }
    /* "#,##.###" - nothing for zero and no leading zero. */
    function balFmt(x) { if (!x) return ''; var s = fmt(x, 3); return s.replace(/^(-?)0\./, '$1.'); }

    // ------------------------------------------------------------------------------ TextChanged emulation
    var TC = {};
    function setT(id, v) { var e = $id(id); if (!e) return; var s = str(v); if (e.value === s) return; e.value = s; var h = TC[id]; if (h) h(); }
    function activeTag() { var a = document.activeElement; return a && a.getAttribute ? str(a.getAttribute('data-tag')) : ''; }
    function guard(fn) { return function () { try { fn(); } catch (e) { box(e.message); } }; }

    // ------------------------------------------------------------------------------ tabs
    function tab(group, panelId) {
        var tabs = document.querySelector('.win-tabs[data-tabs="' + group + '"]'); if (!tabs) return;
        tabs.querySelectorAll('.win-tab').forEach(function (b) { b.classList.toggle('is-active', b.getAttribute('data-tab') === panelId); });
        var host = tabs.parentNode;
        Array.prototype.forEach.call(host.children, function (p) { if (p.classList.contains('win-tab-panel')) p.classList.toggle('is-active', p.id === panelId); });
        if (group === 'main') {
            var onHist = panelId === 'tabPage2';
            var fb = $id('btnFooterHistory');
            fb.querySelector('span').textContent = onHist ? 'Form' : 'History';
            fb.querySelector('i').className = onHist ? 'fa fa-file-text-o' : 'fa fa-history';
            /* tabControl1_SelectedIndexChanged */
            if (onHist) focus('FromDateHistory'); else focus('DocDate');
        }
    }
    function activeTab(group) { var t = document.querySelector('.win-tabs[data-tabs="' + group + '"] .win-tab.is-active'); return t ? t.getAttribute('data-tab') : ''; }
    function toggleHistory() { tab('main', activeTab('main') === 'tabPage2' ? 'tabPage1' : 'tabPage2'); }

    // ------------------------------------------------------------------------------ load
    function combosBind(d) {
        /* CmbExportPackingTypeFill: Rows[1].Activate() - the first packing type unless one was selected. */
        if (d.packingTypes) { S.packs = d.packingTypes; var pk = val('CmbPackingType'); bind('CmbPackingType', d.packingTypes, 'Id', 'PackTypeDesc'); if (!pk && d.packingTypes.length) setVal('CmbPackingType', col(d.packingTypes[0], 'Id')); }
        if (d.warehouses) { S.wh = d.warehouses; bind('CmbWareHouse', d.warehouses, 'Id', 'WareHouseName'); }
        if (d.jobLots) { S.jobs = d.jobLots; bind('CmbJobLot', d.jobLots, 'Id', 'JobLotDescription'); }
        if (d.invoices) bind('cmbInvoiceNo', d.invoices, 'Id', 'InvoiceNo');
        if (d.loadingPorts) bind('CmbLoadingPort', d.loadingPorts, 'Id', 'PortName');
        if (d.transporters) bind('cmbtransporter', d.transporters, 'Id', 'CompanyName');
        if (d.otherItems) { S.otherCombo = d.otherItems; bind('CmbOtherItem', d.otherItems, 'Id', 'ItemName'); }
        if (d.branches) { S.branchCount = d.branches.length; bind('CmbBranch', d.branches, 'Id', 'BranchName'); }
    }
    function historyCombosBind(h) { h = h || {}; bind('CmbCustomerHistory', h.customers, 'Id', 'Name'); bind('CmbInvoiceNoHistory', h.invoices, 'Id', 'Name'); }

    /* PurchsaeOrder_Load */
    function load() {
        return getJson(API + '/setup').then(function (d) {
            d = d || {};
            if (d.permissions) PERM = d.permissions;
            CFG = d.config || {};
            S.userBranchId = d.userBranchId || 0;
            combosBind(d);
            errs(d, ['invoices', 'packingTypes', 'docNo', 'warehouses', 'loadingPorts', 'transporters', 'jobLots', 'otherItems', 'branches', 'historyCombos']);
            setText('txtdocno', d.docNo || '');
            setText('DocDate', today());
            setText('txtInvoiceDate', today());
            show('btnupdate', false); show('btnsave', true); show('btnplus', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
            show('branchField', S.branchCount > 1);
            setVal('CmbBranch', S.userBranchId);
            defaultConfiguration();
            historyCombosBind(d.historyCombos);
            setText('FromDateHistory', daysAgo(CFG.DefaultDaysToLessFromHistoryFromDate > 0 ? CFG.DefaultDaysToLessFromHistoryFromDate : 3));
            setText('ToDateHistory', today());
            renderGrid(); renderOther(); applyRights(); focus('DocDate');
        }).catch(function (e) { box(e.message); });
    }

    /* defaultConfiquration: Job/Lot and Warehouse defaults. */
    function defaultConfiguration() {
        if (CFG.DefaultJobLotId > 0) setVal('CmbJobLot', CFG.DefaultJobLotId);
        if (CFG.DefaultWarehouseId > 0) setVal('CmbWareHouse', CFG.DefaultWarehouseId);
    }

    /* btnRefresh_Click */
    function refresh(btn) {
        return busy(btn, function () {
            return getJson(API + '/refresh?recId=' + S.recId).then(function (d) {
                d = d || {};
                if (d.config) CFG.SaleCostingJobOrderWise = d.config.SaleCostingJobOrderWise;
                combosBind(d);
                errs(d, ['branches', 'packingTypes', 'warehouses', 'jobLots', 'invoices', 'loadingPorts', 'transporters', 'otherItems']);
                renderGrid();
            });
        });
    }

    /* CmbInvoiceNoFill */
    function invoiceComboFill() {
        return getJson(API + '/invoices?recId=' + S.recId).then(function (rows) { bind('cmbInvoiceNo', rows, 'Id', 'InvoiceNo'); }).catch(function (e) { box(e.message); });
    }

    // ------------------------------------------------------------------------------ reset
    function resetOtherItems() {
        S.updateIndexOther = -1;
        setVal('CmbOtherItem', '');
        setT('txtoQty', '');
        setT('txtoRemarks', '');
        focus('CmbOtherItem');
    }
    function clearItemCrop() { S.items = []; bind('CmbItemName', [], 'InvoiceDetailId', 'ItemName'); bind('CmbCropYear', [], 'CropYearId', 'CropYear'); }
    /* ResetDetail */
    function resetDetail() {
        focus('cmbInvoiceNo');
        S.items = []; bind('CmbItemName', [], 'InvoiceDetailId', 'ItemName');
        S.uomM = []; bind('CmbPackUomMaster', [], 'PackUomId', 'PackUom');
        setT('txtQtyMaster', '');
        setT('txtNetWeight', '');
        setVal('CmbOrderNo', ''); clearItemCrop();              /* CmbOrderNo_TextChanged */
        setVal('CmbPackingType', '');
        setText('txtremarksdetail', '');
        setT('txtEbUnitMaster', '');
        setT('txtQtyInner', '');
        setText('txtGrossWeight', '');
        setT('txtEbUnitInner', '');
        setT('txtEbTotalInner', '');
        setText('txtInspectionRemarks', '');
        setText('txtContainerRemarks', '');
        setText('txtGrossWeight', '');
        setVal('CmbSupplierCustomer', '');
        S.containers = []; bind('cmbContainerNoDetail', [], 'ContainerId', 'ContainerNo');
        show('btnplus', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
        S.updateIndex = -1;
    }
    /* Reset (btnnew_Click). lstRemoveRecord is never cleared here - as on the desktop. */
    function newForm() {
        try {
            S.recId = 0;
            focus('DocDate');
            setVal('cmbtransporter', '');
            setT('txtItemWeight', '');
            setT('txtPackingWeight', '');
            setT('txtOtherWeight', '');
            setT('txtTotalGrossWeight', '');
            setText('txtOtherRemarks', '');
            setVal('CmbItemName', '');
            setVal('CmbPackUomMaster', '');
            setT('txtQtyMaster', '');
            setT('txtNetWeight', '');
            setVal('CmbSupplierCustomer', '');
            setVal('CmbOrderNo', ''); clearItemCrop();
            setVal('CmbPackingType', '');
            setText('txtremarks', '');
            setVal('cmbInvoiceNo', '');
            setText('txtVehicleNo', '');
            setText('txtInvoiceDate', today());
            S.contRead = [];
            bind('CmbSupplierCustomer', [], 'Id', 'CustomerName'); bind('CmbOrderNo', [], 'OrderId', 'OrderNo');
            show('btnsave', true); show('btnupdate', false); show('btnplus', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
            S.rows = []; renderGrid();
            enable('cmbInvoiceNo', true);
            ['txtInvoiceWeight', 'txtDoWeight', 'txtBalanceWeight', 'txtExportReturnWeight', 'txtrejectedWeight'].forEach(function (k) { setText(k, ''); });
            setVal('CmbBranch', S.userBranchId);
            resetOtherItems();
            S.others = []; renderOther();
            defaultConfiguration();
            resetDetail();
        } catch (e) { box(e.message); }
        /* DocumentNoFill + CmbInvoiceNoFill + defaultConfiquration */
        return getJson(API + '/reset').then(function (d) {
            d = d || {};
            setText('txtdocno', d.docNo || '');
            if (d.invoices) bind('cmbInvoiceNo', d.invoices, 'Id', 'InvoiceNo');
            if (d.config) { CFG.DefaultJobLotId = d.config.DefaultJobLotId; CFG.DefaultWarehouseId = d.config.DefaultWarehouseId; CFG.RestrictOneInvoiceOnDo = d.config.RestrictOneInvoiceOnDo; }
            errs(d, ['docNo', 'invoices']);
        }).catch(function (e) { box(e.message); });
    }

    // ------------------------------------------------------------------------------ detail combos
    function curItem() { var v = val('CmbItemName'); if (!v) return null; for (var i = 0; i < S.items.length; i++) if (str(col(S.items[i], 'InvoiceDetailId')) === v) return S.items[i]; return null; }

    function weightsBind(w) {
        w = w || {};
        setText('txtInvoiceWeight', str(w.InvoiceWeight)); setText('txtDoWeight', str(w.DoWeight)); setText('txtrejectedWeight', str(w.GpRejectedWeight));
        setText('txtBalanceWeight', str(w.WeightAvailableForDo)); setText('txtExportReturnWeight', str(w.ReturnWeight));
    }
    /* GetContainerNoFromCROByInvoiceId: the booking's containers + the ones read with the record; Rows[0] (the blank row) active. */
    function containersBind(rows) {
        var keep = netI(val('cmbContainerNoDetail'));
        if (rows && rows.length) {
            var list = rows.slice();
            S.contRead.forEach(function (r) { if (!list.some(function (x) { return netI(col(x, 'ContainerId')) === netI(r.ContainerId); })) list.push({ ContainerId: r.ContainerId, ContainerNo: r.ContainerNo }); });
            S.containers = list;
            bind('cmbContainerNoDetail', list, 'ContainerId', 'ContainerNo', null, '');
        } else { S.containers = []; bind('cmbContainerNoDetail', [], 'ContainerId', 'ContainerNo', null, ''); }
        if (keep > 0) setVal('cmbContainerNoDetail', keep);
    }
    function inspectionsBind(rows) { bind('CmbInspectionNoPackListDetail', rows || [], 'Id', 'InspectionNo'); }

    /* cmbInvoiceNo_Leave (GetDatabyInvoiceId, GetInvoicewiseDoWeightandBalanceWeight, GetContainerNoFromCROByInvoiceId, ThirdPartyNoBind). */
    function invoiceLeave() {
        var invoiceId = netI(val('cmbInvoiceNo'));
        var it = curItem();
        setVal('CmbSupplierCustomer', ''); bind('CmbSupplierCustomer', [], 'Id', 'CustomerName');
        setVal('CmbOrderNo', ''); bind('CmbOrderNo', [], 'OrderId', 'OrderNo'); clearItemCrop();
        return getJson(API + '/invoice?invoiceId=' + invoiceId + '&recId=' + S.recId + '&itemId=' + (it ? netI(col(it, 'ItemId')) : 0)).then(function (d) {
            d = d || {};
            var h = d.header || [];
            if (h.length) {
                setText('txtInvoiceDate', isoDate(col(h[0], 'DocDate')));
                bind('CmbSupplierCustomer', h, 'Id', 'CustomerName', null, col(h[0], 'Id'));
            }
            var c = d.contracts || [];
            var p = Promise.resolve();
            if (c.length) { bind('CmbOrderNo', c, 'OrderId', 'OrderNo', null, col(c[0], 'OrderId')); p = orderChanged(); }
            weightsBind(d.weights);
            if (invoiceId !== 0) containersBind(d.containers); else { S.containers = []; bind('cmbContainerNoDetail', [], 'ContainerId', 'ContainerNo'); }
            inspectionsBind(d.inspections);
            return p;
        }).catch(function (e) { box(e.message); });
    }

    /* CmbOrderNo_TextChanged: ExportInvoiceDetailByHeaderIdNew -> Item Name (one row: active) and CropYear (first row active). */
    function orderChanged() {
        clearItemCrop();
        var id = netI(val('cmbInvoiceNo')), orderId = netI(val('CmbOrderNo'));
        if (!(id > 0 && orderId > 0)) return Promise.resolve();
        return getJson(API + '/contract-items?invoiceId=' + id + '&orderId=' + orderId).then(function (rows) {
            rows = rows || [];
            if (!rows.length) return;
            S.items = rows.map(function (r) {
                return { ItemId: netI(col(r, 'ItemId')), ItemName: str(col(r, 'ItemName')), InvoiceDetailId: netI(col(r, 'InvoiceDetailId')), PackUomId: col(r, 'PackUomId'),
                    PackUom: str(col(r, 'PackUom')), PackingTypeId: netI(col(r, 'PackingMaterialTypeId')), BalMTon: netD(col(r, 'BalMTon')) };
            });
            var crops = [];
            rows.forEach(function (r) { var cid = netI(col(r, 'CropYearId')); if (!crops.some(function (x) { return x.CropYearId === cid; })) crops.push({ CropYearId: cid, CropYear: str(col(r, 'CropYear')) }); });
            bind('CmbItemName', S.items, 'InvoiceDetailId', 'ItemName', ['BalMTon'], S.items.length === 1 ? S.items[0].InvoiceDetailId : '');
            if (crops.length) bind('CmbCropYear', crops, 'CropYearId', 'CropYear', null, crops[0].CropYearId);
        }).catch(function (e) { box(e.message); });
    }

    /* CmbItemName Leave -> CmbItemName_TextChanged (BindPackUomAndPackingType, BindInnerPackUom, ThirdPartyNoBind, Bal Mton). */
    function itemChanged() {
        var it = curItem(), invoiceId = netI(val('cmbInvoiceNo'));
        if (!it) {
            inspectionsBind([]);
            $id('txtBalnceMtonOfSelectedItem').textContent = '';
            return Promise.resolve();
        }
        return getJson(API + '/item?invoiceDetailId=' + it.InvoiceDetailId + '&itemId=' + it.ItemId + '&invoiceId=' + invoiceId).then(function (d) {
            d = d || {};
            var dt = d.detail || [];
            setVal('CmbPackUomMaster', ''); setVal('CmbPackUomInner', '');
            if (dt.length) {
                setT('txtEbTotalInner', str(col(dt[0], 'TotalPackingWeight')));
                setT('txtEbUnitInner', str(col(dt[0], 'PackingWeight')));
                if (netI(col(dt[0], 'JobLotId')) > 0) setVal('CmbJobLot', col(dt[0], 'JobLotId'));
                S.uomM = dt.map(function (r) { return { PackUomId: col(r, 'PackUomId'), PackUom: str(col(r, 'PackUom')), Equivalent: netD(col(r, 'Equivalent')) }; });
                S.uomI = dt.map(function (r) { return { PackUomId: col(r, 'InnerPackUomId'), PackUom: str(col(r, 'InnerPackUom')), Equivalent: netD(col(r, 'InnerPackEquivalent')) }; })
                    .filter(function (x) { return x.PackUomId !== null && x.PackUomId !== undefined && x.PackUomId !== ''; });
                S.innerBound = true;
                bind('CmbPackUomMaster', S.uomM, 'PackUomId', 'PackUom', ['Equivalent'], S.uomM.length ? S.uomM[0].PackUomId : '');
                bind('CmbPackUomInner', S.uomI, 'PackUomId', 'PackUom', ['Equivalent'], S.uomI.length ? S.uomI[0].PackUomId : '');
            } else {
                S.uomM = []; S.uomI = []; S.innerBound = false;
                bind('CmbPackUomMaster', [], 'PackUomId', 'PackUom'); bind('CmbPackUomInner', [], 'PackUomId', 'PackUom');
            }
            if (!S.innerBound) {
                /* BindInnerPackUom(ItemId) - the item's UOM schedule; the previous inner UOM text kept when listed. */
                var prev = selText('CmbPackUomInner');
                var u = d.uoms || [];
                if (u.length) {
                    S.uomI = u.map(function (r) { return { PackUomId: col(r, 'Id'), PackUom: str(col(r, 'UOMCode')), Equivalent: netD(col(r, 'Equivalent')) }; });
                    S.innerBound = true;
                    var keep = ''; S.uomI.forEach(function (x) { if (x.PackUom === prev) keep = x.PackUomId; });
                    bind('CmbPackUomInner', S.uomI, 'PackUomId', 'PackUom', ['Equivalent'], keep);
                }
            }
            setVal('CmbPackingType', it.PackingTypeId);
            inspectionsBind(d.inspections);
            $id('txtBalnceMtonOfSelectedItem').textContent = balFmt(it.BalMTon);
        }).catch(function (e) { box(e.message); });
    }

    /* CmbInspectionNoPackListDetail_Leave -> ThirdPartySubLotNoBind */
    function inspectionLeave() {
        var id = netI(val('CmbInspectionNoPackListDetail'));
        return getJson(API + '/sub-lots?analysisId=' + id + '&invoiceId=' + netI(val('cmbInvoiceNo'))).then(function (rows) {
            bind('CmbInspectionSubLot', rows || [], 'Id', 'SubLot');
        }).catch(function (e) { box(e.message); });
    }

    // ------------------------------------------------------------------------------ weight calculations (desktop formulas)
    function eqM() { return netD(selExtra('CmbPackUomMaster', 0)); }
    function eqI() { return netD(selExtra('CmbPackUomInner', 0)); }
    /* CalculateNetWeight */
    function calcNet() {
        if (hasSel('CmbPackUomMaster') && netD(val('txtQtyMaster')) !== 0) setT('txtNetWeight', f2(eqM() * netD(val('txtQtyMaster'))));
        else setT('txtNetWeight', f2(0));
    }
    /* CalculaterEbMaster */
    function calcEbMaster() {
        var pw = netD(val('txtEbUnitMaster')), tpw = netD(val('txtEbTotalMaster')), q = netD(val('txtQtyMaster')), tag = activeTag();
        if (tag === 'TotalPackingWeightMaster') { setT('txtEbUnitMaster', f2(tpw / q)); S.ebMU = true; S.ebMT = false; }
        else if (tag === 'PackingWeightMaster') { setT('txtEbTotalMaster', f2(pw * q)); S.ebMT = true; S.ebMU = false; }
        else if (S.ebMU) setT('txtEbTotalMaster', f2(pw * q));
        else if (S.ebMT) setT('txtEbUnitMaster', f2(tpw / q));
    }
    /* CalculateEbUnitOrTotalOfInner */
    function calcEbInner() {
        var pw = netD(val('txtEbUnitInner')), tpw = netD(val('txtEbTotalInner')), q = netD(val('txtQtyInner')), tag = activeTag();
        if (tag === 'TotalPackingWeight') { setT('txtEbUnitInner', f2(tpw / q)); S.ebIT = true; S.ebIU = false; }
        else if (tag === 'PackingWeight') { setT('txtEbTotalInner', f2(pw * q)); S.ebIU = true; S.ebIT = false; }
        else if (S.ebIU) setT('txtEbTotalInner', f2(pw * q));
        else if (S.ebIT) setT('txtEbUnitInner', f2(tpw / q));
    }
    /* CalculateGrossWeight */
    function calcGross() {
        var net = netD(val('txtNetWeight'));
        if (net > 0) {
            var q = netD(val('txtQtyMaster'));
            var acc = netD(val('txtAccessWtSet')) * q / 1000;
            setText('txtGrossWeight', f2(net + acc + netD(val('txtEbTotalMaster')) + netD(val('txtEbTotalInner'))));
        } else setText('txtGrossWeight', f2(net));
    }
    /* calculateAutoInnerQty */
    function autoInnerQty() {
        var w = netD(val('txtNetWeight'));
        if (w > 0 && hasSel('CmbPackUomInner')) setT('txtQtyInner', f3(w / eqI()));
        else setT('txtQtyInner', '0');
    }
    /* txtQtyMaster_Leave */
    function qtyMasterLeave() {
        if (netD(val('txtQtyMaster')) !== 0 && netD(val('txtQtyInner')) === 0) { setT('txtQtyInner', f2(netD(val('txtQtyMaster')))); calcEbInner(); }
        calcNet();
    }
    /* CmbPackUomMaster_Leave */
    function uomMasterLeave() {
        calcEbMaster();
        if (netI(val('CmbPackUomMaster')) !== 0) {
            if (S.uomI.length >= 2) setVal('CmbPackUomInner', val('CmbPackUomMaster'));
            calcEbInner();
        }
        calcNet();
    }
    /* CalculaterOtherItem */
    function calcOther() {
        var pw = netD(val('txtWtPerQtyOtherItem')), tpw = netD(val('txtNetWeightOtherItem')), q = netD(val('txtoQty')), tag = activeTag();
        if (tag === 'NetWeight') setT('txtWtPerQtyOtherItem', f2(tpw / q));
        else setT('txtNetWeightOtherItem', f2(pw * q));
    }

    TC.txtQtyMaster = guard(function () { calcEbMaster(); calcNet(); });
    TC.txtEbUnitMaster = guard(function () { calcEbMaster(); calcGross(); });
    TC.txtEbTotalMaster = guard(function () {
        var net = netD(val('txtNetWeight'));
        setText('txtGrossWeight', net > 0 ? f2(net + netD(val('txtEbTotalMaster')) + netD(val('txtEbTotalInner'))) : f2(net));
        calcEbMaster();
    });
    TC.txtQtyInner = guard(function () { calcEbInner(); calcGross(); });
    TC.txtEbTotalInner = guard(function () { calcEbInner(); calcGross(); });
    TC.txtEbUnitInner = guard(function () { calcEbInner(); calcGross(); });
    TC.txtOtherWeight = guard(function () { calcTotals(); });
    TC.txtOtherItemTotalWt = guard(function () { calcTotals(); });
    TC.txtAccessWtSet = guard(function () { calcGross(); });
    TC.txtNetWeight = guard(function () { autoInnerQty(); });
    TC.txtoQty = guard(calcOther);
    TC.txtWtPerQtyOtherItem = guard(calcOther);
    TC.txtNetWeightOtherItem = guard(calcOther);

    // ------------------------------------------------------------------------------ totals
    function sum(list, k) { var t = 0; list.forEach(function (r) { t += netD(r[k]); }); return t; }
    /* CalculateTotalInformation */
    function calcTotals() {
        if (S.rows.length > 0) {
            var itemW = sum(S.rows, 'LoadWeight'), ebM = sum(S.rows, 'EbTotalMaster'), ebI = sum(S.rows, 'EbTotalInner');
            var other = S.others.length > 0 ? sum(S.others, 'NetWeight') : 0;
            var acc = 0; S.rows.forEach(function (r) { acc += netD(r.AccessWtSet) * netD(r.MasterQTY); });
            var gross = sum(S.rows, 'GrossWeight');
            if (netD(val('txtOtherWeight')) > 0) gross += netD(val('txtOtherWeight'));
            gross += other;
            setT('txtItemWeight', f2(round2(itemW)));
            setT('txtPackingWeight', f2(round2(ebM + ebI)));
            setT('txtTotalAccessWtSet', f2(round2(acc / 1000)));
            setT('txtOtherItemTotalWt', f2(round2(other)));
            setT('txtTotalGrossWeight', f2(round2(gross)));
        } else {
            ['txtItemWeight', 'txtPackingWeight', 'txtTotalAccessWtSet', 'txtOtherItemTotalWt', 'txtTotalGrossWeight'].forEach(function (k) { setT(k, '0'); });
        }
        otherWeightProportion();
    }
    /* OtherWeightProportion */
    function otherWeightProportion() {
        var net = S.rows.length > 0 ? sum(S.rows, 'NetWeight') : 0;
        var oh = netD(val('txtOtherWeight'));
        S.rows.forEach(function (r) { r.OtherWeight = oh > 0 ? oh / net * netD(r.NetWeight) : 0; });
        renderGrid();
    }

    // ------------------------------------------------------------------------------ detail grid
    var G_COLS = [
        ['InvoiceNo', 'InvoiceNo'], ['SupplierCustomer', 'Customer'], ['OrderNo', 'OrderNo'], ['WareHouseId', 'WareHouse', 'wh'], ['JobLotId', 'JobLot', 'job'],
        ['CropYear', 'CropYear'], ['Item', 'Item'], ['PackingTypeId', 'PackingType', 'pack'], ['MasterQTY', 'MasterQTY', 'n'], ['MasterUOM', 'MasterUOM'],
        ['EbUnitMaster', 'EbUnitMaster', 'n'], ['EbTotalMaster', 'EbTotalMaster', 'n'], ['InnerQTY', 'InnerQTY', 'n'], ['InnerUOM', 'InnerUOM', 'uom'],
        ['EbUnitInner', 'EbUnitInner', 'n'], ['EbTotalInner', 'EbTotalInner', 'n'], ['AccessWtSet', 'Access Wt Set(g)', 'n'], ['LoadQty', 'LoadQty', 'v'],
        ['LoadWeight', 'LoadWeight', 'v'], ['GrossWeight', 'GrossWeight', 'v'], ['OtherWeight', 'OtherWeight', 'v'], ['ThirdPartyAnalysisNo', 'ThirdPartyAnalysisNo'],
        ['ThirdPartyAnalysisSubNo', 'ThirdPartyAnalysisSubNo'], ['ContainerNo', 'ContainerNo', 'cont'], ['ContainerRemarks', 'ContainerRemarks', 't'],
        ['InspectionRemarks', 'InspectionRemarks', 't'], ['Remarks', 'Remarks', 't']];
    var G_SUM = { MasterQTY: 1, EbTotalMaster: 1, InnerQTY: 1, EbTotalInner: 1, LoadQty: 1, LoadWeight: 1, GrossWeight: 1, OtherWeight: 1 };
    function opts(list, idCol, txtCol, cur) {
        var h = '<option value=""></option>';
        list.forEach(function (x) { var v = str(col(x, idCol)); h += '<option value="' + esc(v) + '"' + (v === str(cur) ? ' selected' : '') + '>' + esc(col(x, txtCol)) + '</option>'; });
        return h;
    }
    function renderGrid() {
        $id('grdHead').innerHTML = '<tr><th>X</th><th>+</th><th>Edit</th>' + G_COLS.map(function (c) { return '<th' + (c[2] === 'n' || c[2] === 'v' ? ' class="num"' : '') + '>' + esc(c[1]) + '</th>'; }).join('') + '</tr>';
        $id('grdBody').innerHTML = S.rows.map(function (r, i) {
            return '<tr data-i="' + i + '"' + (i === S.curRow ? ' class="is-current"' : '') + '><td><button type="button" class="win-btn-small" data-del="' + i + '">X</button></td>'
                + '<td><button type="button" class="win-btn-small" data-add="' + i + '">+</button></td><td><button type="button" class="win-btn-small" data-edit="' + i + '">Edit</button></td>'
                + G_COLS.map(function (c) {
                    var k = c[0], t = c[2], v = r[k];
                    if (t === 'wh') return '<td><select class="do-cell" data-cell="' + k + '" data-i="' + i + '">' + opts(S.wh, 'Id', 'WareHouseName', v) + '</select></td>';
                    if (t === 'job') return '<td><select class="do-cell" data-cell="' + k + '" data-i="' + i + '">' + opts(S.jobs, 'Id', 'JobLotDescription', v) + '</select></td>';
                    if (t === 'pack') return '<td><select class="do-cell" data-cell="' + k + '" data-i="' + i + '">' + opts(S.packs, 'Id', 'PackTypeDesc', v) + '</select></td>';
                    if (t === 'n') return '<td><input type="text" class="do-cell num" data-guard="decimal" data-cell="' + k + '" data-i="' + i + '" value="' + esc(fmt(v, 4)) + '"/></td>';
                    if (t === 't') return '<td><input type="text" class="do-cell" data-cell="' + k + '" data-i="' + i + '" value="' + esc(v) + '"/></td>';
                    if (t === 'v') return '<td class="num">' + esc(fmt(v, 4)) + '</td>';
                    if (t === 'cont' || t === 'uom') return '<td><a class="win-code" href="javascript:void(0)" data-pick="' + t + '" data-i="' + i + '" title="F1 - pick from the list">' + (str(v) ? esc(v) : '&hellip;') + '</a></td>';
                    if (k === 'Item' || k === 'InvoiceNo') return '<td><a class="win-code" href="javascript:void(0)" data-edit="' + i + '">' + esc(v) + '</a></td>';
                    return '<td>' + esc(v) + '</td>';
                }).join('') + '</tr>';
        }).join('');
        $id('grdFoot').innerHTML = S.rows.length ? '<tr><td class="lbl" colspan="3">&Sigma;</td>' + G_COLS.map(function (c) { return G_SUM[c[0]] ? '<td class="num">' + esc(fmt(sum(S.rows, c[0]), 4)) + '</td>' : '<td></td>'; }).join('') + '</tr>' : '';
        setText('doFooterInfo', ''); $id('doFooterInfo').textContent = S.rows.length ? ('Detail rows: ' + S.rows.length) : '';
    }

    function rowFromForm(r) {
        var it = curItem();
        r.InvoiceId = netI(val('cmbInvoiceNo')); r.InvoiceNo = selText('cmbInvoiceNo');
        r.SupplierCustomerId = netI(val('CmbSupplierCustomer')); r.SupplierCustomer = selText('CmbSupplierCustomer');
        r.OrderId = netI(val('CmbOrderNo')); r.OrderNo = selText('CmbOrderNo');
        r.WareHouseId = netI(val('CmbWareHouse')); r.WareHouse = selText('CmbWareHouse');
        r.JobLotId = netI(val('CmbJobLot')); r.JobLot = selText('CmbJobLot');
        r.CropYearId = netI(val('CmbCropYear')); r.CropYear = selText('CmbCropYear');
        r.InvoiceDetailId = it ? it.InvoiceDetailId : 0; r.ItemId = it ? it.ItemId : 0; r.Item = selText('CmbItemName');
        r.PackingTypeId = netI(val('CmbPackingType')); r.PackingType = selText('CmbPackingType');
        r.MasterQTY = netD(val('txtQtyMaster')); r.MasterUOMId = netI(val('CmbPackUomMaster')); r.MasterUOM = selText('CmbPackUomMaster'); r.MasterUOMEquivalent = eqM();
        r.EbUnitMaster = netD(val('txtEbUnitMaster')); r.EbTotalMaster = netD(val('txtEbTotalMaster'));
        r.InnerQTY = netD(val('txtQtyInner'));
        if (hasSel('CmbPackUomInner')) { r.InnerUOMId = netI(val('CmbPackUomInner')); r.InnerUOM = selText('CmbPackUomInner'); r.InnerUOMEquivalent = eqI(); }
        else { r.InnerUOMId = 0; r.InnerUOM = ''; r.InnerUOMEquivalent = 0; }
        r.EbUnitInner = netD(val('txtEbUnitInner')); r.EbTotalInner = netD(val('txtEbTotalInner')); r.AccessWtSet = netD(val('txtAccessWtSet'));
        r.NetWeight = netD(val('txtNetWeight')); r.LoadQty = netD(val('txtQtyMaster')); r.LoadWeight = netD(val('txtNetWeight')); r.GrossWeight = netD(val('txtGrossWeight'));
        r.ThirdPartyAnalysisId = netI(val('CmbInspectionNoPackListDetail')); r.ThirdPartyAnalysisNo = selText('CmbInspectionNoPackListDetail');
        r.ThirdPartyAnalysisSubId = netI(val('CmbInspectionSubLot')); r.ThirdPartyAnalysisSubNo = selText('CmbInspectionSubLot');
        if (hasSel('cmbContainerNoDetail')) { r.ContainerId = netI(val('cmbContainerNoDetail')); r.ContainerNo = selText('cmbContainerNoDetail'); } else { r.ContainerId = 0; r.ContainerNo = ''; }
        r.ContainerRemarks = val('txtContainerRemarks'); r.InspectionRemarks = val('txtInspectionRemarks'); r.Remarks = val('txtremarksdetail');
        return r;
    }
    function need(cond, msg, id) { if (cond) { box(msg); focus(id); return true; } return false; }
    /* FormValidationDetail */
    function detailValid() {
        if (need(!hasSel('cmbInvoiceNo'), 'InvoiceNo field is required', 'cmbInvoiceNo')) return false;
        if (need(!hasSel('CmbSupplierCustomer'), 'Customer field is required', 'CmbSupplierCustomer')) return false;
        if (need(!hasSel('CmbOrderNo'), 'OrderNo field is required', 'CmbOrderNo')) return false;
        if (need(!hasSel('CmbWareHouse'), 'Warehouse field is required', 'CmbWareHouse')) return false;
        if (need(!hasSel('CmbItemName'), 'ItemName field is required', 'CmbItemName')) return false;
        if (need(!hasSel('CmbJobLot'), 'JobLot field is required', 'CmbJobLot')) return false;
        if (need(!hasSel('CmbCropYear'), 'CropYear field is required', 'CmbCropYear')) return false;
        if (need(!hasSel('CmbPackingType'), 'Packing type field is required', 'CmbPackingType')) return false;
        if (need(val('txtQtyMaster').trim() === '' || netD(val('txtQtyMaster')) === 0, 'Master Qty field is required', 'txtQtyMaster')) return false;
        if (need(!hasSel('CmbPackUomMaster'), 'Master PackUOM field is required', 'CmbPackUomMaster')) return false;
        if (need(val('txtNetWeight').trim() === '' || netD(val('txtNetWeight')) === 0, 'Net Weight field is required', 'txtNetWeight')) return false;
        if (need(CFG.SaleCostingJobOrderWise && netI(val('CmbInspectionNoPackListDetail')) === 0, 'Inspection No field is required', 'CmbInspectionNoPackListDetail')) return false;
        if (need(CFG.ContainerNoCompulsoryOnDeliveryOrderExport && !hasSel('cmbContainerNoDetail'), 'ContainerNo field is required', 'cmbContainerNoDetail')) return false;
        return true;
    }
    /* GridEX_Helper.IsAnyCellValueHaveSameValue(grd, "InvoiceId", value[, skipIndex]) */
    function anyInvoice(id, skip) { return S.rows.some(function (r, i) { return i !== skip && netI(r.InvoiceId) === id; }); }
    function inspectionChecks() {
        if (!CFG.IsMandatoryInspectionLotMappedOnDeliveryOrder) return true;
        if (!hasSel('CmbInspectionNoPackListDetail')) { focus('CmbInspectionNoPackListDetail'); box('Inspection No field is required'); return false; }
        if (CFG.IsInspectionLotMappedOnDeliveryOrderByInvoice && !hasSel('CmbInspectionSubLot')) { focus('CmbInspectionSubLot'); box('Inspection SubLot No field is required'); return false; }
        return true;
    }
    /* btnplus_Click */
    function detailAdd() {
        try {
            if (!detailValid()) return;
            if (S.rows.length > 0 && CFG.RestrictOneInvoiceOnDo && !anyInvoice(netI(val('cmbInvoiceNo')), -1)) throw new Error("Data Against another Invoice Exists in Grid,So you can't add different Invoice");
            if (!inspectionChecks()) return;
            var r = rowFromForm({ Id: 0, OtherWeight: 0 });
            S.rows.push(r);
            renderGrid();
            resetDetail();
            calcTotals();
        } catch (e) { box(e.message); }
    }
    /* btnUpdateDetail_Click */
    function detailUpdate() {
        try {
            if (!detailValid()) return;
            if (S.rows.length > 0 && CFG.RestrictOneInvoiceOnDo && !anyInvoice(netI(val('cmbInvoiceNo')), S.updateIndex)) throw new Error("Data Against another Invoice Exists in Grid,So you can't add different Invoice");
            if (!inspectionChecks()) return;
            var r = S.rows[S.updateIndex]; if (!r) return;
            rowFromForm(r);
            show('btnplus', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
            resetDetail();
            invoiceComboFill();
            calcTotals();
        } catch (e) { box(e.message); }
    }
    /* btnCancelUpdateDetial_Click */
    function detailCancel() { resetDetail(); invoiceComboFill(); }

    /* grd_DoubleClick: the row back into the Detail box (the desktop does not copy Container Remarks back). */
    function detailEdit(i) {
        var r = S.rows[i]; if (!r) return Promise.resolve();
        S.updateIndex = i;
        setVal('cmbInvoiceNo', r.InvoiceId);
        return invoiceLeave().then(function () {
            setVal('CmbSupplierCustomer', r.SupplierCustomerId);
            var before = val('CmbOrderNo');
            setVal('CmbOrderNo', r.OrderId);
            return val('CmbOrderNo') !== before ? orderChanged() : null;
        }).then(function () {
            setVal('CmbItemName', r.InvoiceDetailId);
            return itemChanged();
        }).then(function () {
            setVal('CmbPackingType', r.PackingTypeId);
            setVal('CmbCropYear', r.CropYearId);
            setT('txtQtyMaster', raw(r.MasterQTY));
            setVal('CmbPackUomMaster', r.MasterUOMId);
            setT('txtEbUnitMaster', raw(r.EbUnitMaster));
            setT('txtEbTotalMaster', raw(r.EbTotalMaster));
            setT('txtQtyInner', raw(r.InnerQTY));
            setVal('CmbPackUomInner', r.InnerUOMId);
            setT('txtEbUnitInner', raw(r.EbUnitInner));
            setT('txtEbTotalInner', raw(r.EbTotalInner));
            setT('txtAccessWtSet', raw(r.AccessWtSet));
            setT('txtNetWeight', raw(r.NetWeight));
            setT('txtQtyMaster', raw(r.LoadQty));
            setT('txtNetWeight', raw(r.LoadWeight));
            setText('txtGrossWeight', raw(r.GrossWeight));
            setVal('CmbWareHouse', r.WareHouseId);
            setVal('CmbJobLot', r.JobLotId);
            if (netI(r.ThirdPartyAnalysisId) > 0) setVal('CmbInspectionNoPackListDetail', r.ThirdPartyAnalysisId);
            return inspectionLeave();
        }).then(function () {
            if (netI(r.ThirdPartyAnalysisSubId) > 0) setVal('CmbInspectionSubLot', r.ThirdPartyAnalysisSubId);
            if (netI(r.ContainerId) > 0) setVal('cmbContainerNoDetail', r.ContainerId);
            setText('txtInspectionRemarks', r.InspectionRemarks);
            setText('txtremarksdetail', r.Remarks);
            show('btnplus', false); show('btnUpdateDetail', true); show('btnCancelUpdateDetial', true);
            focus('CmbWareHouse');
        }).catch(function (e) { box(e.message); });
    }

    /* grd_ColumnButtonClick "Delete" */
    function detailDelete(i) {
        try {
            var r = S.rows[i]; if (!r) return;
            if (S.updateIndex !== -1) throw new Error('Reset the Detail First...');
            if (netI(r.Id) > 0) {
                if (!ask('Are you sure to Delete?')) return;
                S.removed.push({
                    Id: netI(r.Id), InvoiceId: r.InvoiceId, SupplierCustomerId: r.SupplierCustomerId, OrderId: r.OrderId, WareHouseId: r.WareHouseId, JobLotId: r.JobLotId,
                    CropYearId: r.CropYearId, ItemId: r.ItemId, PackingTypeId: r.PackingTypeId, MasterUOMId: r.MasterUOMId, EbUnitMaster: r.EbUnitMaster, EbTotalMaster: r.EbTotalMaster,
                    InnerQTY: r.InnerQTY, InnerUOMId: r.InnerUOMId, EbUnitInner: r.EbUnitInner, EbTotalInner: r.EbTotalInner, AccessWtSet: r.AccessWtSet, LoadQty: r.LoadQty,
                    LoadWeight: r.LoadWeight, GrossWeight: r.GrossWeight, ThirdPartyAnalysisId: r.ThirdPartyAnalysisId, ThirdPartyAnalysisSubId: r.ThirdPartyAnalysisSubId,
                    Remarks: r.Remarks, InspectionRemarks: r.InspectionRemarks, ContainerRemarks: r.ContainerRemarks
                });
            }
            S.rows.splice(i, 1);
            if (S.curRow >= S.rows.length) S.curRow = -1;
            calcTotals();
        } catch (e) { box(e.message); }
    }
    /* grd_ColumnButtonClick "Add": a copy of the row (Id 0 only while Update is the visible, enabled button). */
    function detailCopy(i) {
        var r = S.rows[i]; if (!r) return;
        var c = JSON.parse(JSON.stringify(r));
        if (visible('btnupdate') && !$id('btnupdate').disabled) c.Id = 0;
        S.rows.push(c);
        calcTotals();
    }

    /* grd_CellUpdated */
    function cellUpdated(i, key) {
        var r = S.rows[i]; if (!r) return;
        var find = function (list, id, txt) { for (var k = 0; k < list.length; k++) if (netI(col(list[k], 'Id')) === netI(id)) return str(col(list[k], txt)); return null; };
        if (key === 'WareHouseId') { var w = find(S.wh, r.WareHouseId, 'WareHouseName'); if (w !== null) r.WareHouse = w; }
        if (key === 'JobLotId') { var j = find(S.jobs, r.JobLotId, 'JobLotDescription'); if (j !== null) r.JobLot = j; }
        if (key === 'PackingTypeId') { var p = find(S.packs, r.PackingTypeId, 'PackTypeDesc'); if (p !== null) r.PackingType = p; }
        var nets = function (qty, ebM, ebI, acc, grossFmt) {
            var eq = netD(r.MasterUOMEquivalent);
            if (eq !== 0 && qty !== 0) {
                var nw = eq * qty;
                r.NetWeight = netD(f4(nw)); r.LoadWeight = netD(f4(nw));
                r.GrossWeight = netD(grossFmt(nw + ebM + ebI + acc * qty));
            } else { r.NetWeight = 0; r.LoadWeight = 0; r.GrossWeight = 0; }
        };
        if (key === 'MasterQTY' || key === 'EbUnitMaster') {
            var q = netD(r.MasterQTY);
            r.LoadQty = netD(f2(q));
            var ebm = netD(r.EbUnitMaster) * q;
            r.EbTotalMaster = netD(f2(ebm));
            nets(q, ebm, netD(r.EbTotalInner), netD(r.AccessWtSet), f2);
        }
        if (key === 'EbTotalMaster') {
            var q2 = netD(r.MasterQTY), ebt = netD(r.EbTotalMaster);
            r.EbUnitMaster = netD(f2(q2 > 0 ? ebt / q2 : 0));
            nets(q2, ebt, netD(r.EbTotalInner), netD(r.AccessWtSet), f4);
        }
        if (key === 'InnerQTY' || key === 'EbUnitInner') {
            var ebi = netD(r.EbUnitInner) * netD(r.InnerQTY);
            r.EbTotalInner = netD(f2(ebi));
            nets(netD(r.MasterQTY), netD(r.EbTotalMaster), ebi, netD(r.AccessWtSet), f2);
        }
        if (key === 'EbTotalInner') {
            var eti = netD(r.EbTotalInner), iq = netD(r.InnerQTY);
            r.EbUnitInner = netD(f2(iq > 0 ? eti / iq : 0));
            nets(netD(r.MasterQTY), netD(r.EbTotalMaster), eti, netD(r.AccessWtSet), f2);
        }
        if (key === 'NetWeight' || key === 'AccessWtSet') {
            r.GrossWeight = netD(f4(netD(r.NetWeight) + netD(r.EbTotalMaster) + netD(r.EbTotalInner) + netD(r.AccessWtSet) * netD(r.MasterQTY)));
        }
        calcTotals();
    }

    /* grd_KeyDown F1 on ContainerNo / InnerUOM - the GrdPopUp lists as an in-cell list. */
    function pick(i, kind, cell) {
        var r = S.rows[i]; if (!r) return;
        var p;
        if (kind === 'cont') {
            var inv = netI(r.InvoiceId);
            if (inv <= 0) { box('InvoiceId not found'); return; }
            p = getJson(API + '/containers?invoiceId=' + inv + '&recId=' + S.recId).then(function (rows) {
                rows = rows || [];
                if (!rows.length) return null;
                var list = rows.slice();
                S.contRead.forEach(function (c) { if (!list.some(function (x) { return netI(col(x, 'ContainerId')) === netI(c.ContainerId); })) list.push({ ContainerId: c.ContainerId, ContainerNo: c.ContainerNo }); });
                return { list: list, id: 'ContainerId', txt: 'ContainerNo' };
            });
        } else {
            var itemId = netI(r.ItemId); if (itemId <= 0) return;
            p = getJson(API + '/item-uoms?itemId=' + itemId).then(function (rows) { return { list: rows || [], id: 'Id', txt: 'UOMCode' }; });
        }
        p.then(function (x) {
            if (!x) return;
            var td = cell.closest('td');
            td.innerHTML = '<select class="do-cell">' + opts(x.list, x.id, x.txt, kind === 'cont' ? r.ContainerId : r.InnerUOMId) + '</select>';
            var s = td.querySelector('select'); s.focus();
            var done = function () {
                var id = netI(s.value), name = s.selectedIndex > 0 ? s.options[s.selectedIndex].textContent : '';
                if (kind === 'cont') { r.ContainerId = id; r.ContainerNo = name; }
                else {
                    r.InnerUOMId = id; r.InnerUOM = name;
                    var f = null; x.list.forEach(function (u) { if (netI(col(u, 'Id')) === id) f = u; });
                    r.InnerUOMEquivalent = f ? netD(col(f, 'Equivalent')) : 0;
                }
                renderGrid();
            };
            s.addEventListener('change', done);
            s.addEventListener('blur', function () { renderGrid(); });
        }).catch(function (e) { box(e.message); });
    }

    // ------------------------------------------------------------------------------ other items
    function renderOther() {
        $id('otherBody').innerHTML = S.others.map(function (o, i) {
            return '<tr data-i="' + i + '"><td><button type="button" class="win-btn-small" data-odel="' + i + '">X</button></td><td><button type="button" class="win-btn-small" data-oadd="' + i + '">+</button></td>'
                + '<td><button type="button" class="win-btn-small" data-oedit="' + i + '">Edit</button></td><td>' + esc(o.InvoiceNo) + '</td>'
                + '<td><select class="do-cell" data-ocell="ItemId" data-i="' + i + '" title="F1 - pick the item">' + opts(S.otherCombo, 'Id', 'ItemName', o.ItemId)
                + (o.ItemId && !S.otherCombo.some(function (x) { return netI(col(x, 'Id')) === netI(o.ItemId); }) ? '<option value="' + esc(o.ItemId) + '" selected>' + esc(o.ItemName) + '</option>' : '') + '</select></td>'
                + '<td><input type="text" class="do-cell num" data-guard="decimal" data-ocell="Qty" data-i="' + i + '" value="' + esc(fmt(o.Qty, 4)) + '"/></td>'
                + '<td><input type="text" class="do-cell num" data-guard="decimal" data-ocell="WeightPerQty" data-i="' + i + '" value="' + esc(fmt(o.WeightPerQty, 4)) + '"/></td>'
                + '<td><input type="text" class="do-cell num" data-guard="decimal" data-ocell="NetWeight" data-i="' + i + '" value="' + esc(fmt(o.NetWeight, 4)) + '"/></td>'
                + '<td><input type="text" class="do-cell" data-ocell="Remarks" data-i="' + i + '" value="' + esc(o.Remarks) + '"/></td></tr>';
        }).join('');
        $id('otherFoot').innerHTML = S.others.length ? '<tr><td class="lbl" colspan="5">&Sigma;</td><td class="num">' + esc(fmt(sum(S.others, 'Qty'), 4)) + '</td><td class="num">' + esc(fmt(sum(S.others, 'WeightPerQty'), 4))
            + '</td><td class="num">' + esc(fmt(sum(S.others, 'NetWeight'), 4)) + '</td><td></td></tr>' : '';
    }
    function otherValid() {
        if (netI(val('CmbOtherItem')) === 0) { box('Item field required'); focus('CmbOtherItem'); return false; }
        if (netD(val('txtoQty')) === 0) { box('Qty field required'); focus('txtoQty'); return false; }
        return true;
    }
    /* btnoadd_Click */
    function otherAdd() {
        if (!otherValid()) return;
        S.others.push({ Id: 0, ExImInvoiceId: 0, InvoiceDetailId: 0, InvoiceNo: '0', ItemId: netI(val('CmbOtherItem')), ItemName: selText('CmbOtherItem'),
            Qty: netD(val('txtoQty')), WeightPerQty: netD(val('txtWtPerQtyOtherItem')), NetWeight: netD(val('txtNetWeightOtherItem')), Remarks: val('txtoRemarks').trim() });
        renderOther();
        resetOtherItems();
    }
    /* grdotheritems_DoubleClick */
    function otherEdit(i) {
        var o = S.others[i]; if (!o) return;
        S.updateIndexOther = i;
        setVal('CmbOtherItem', o.ItemId);
        setT('txtoQty', raw(o.Qty));
        setT('txtWtPerQtyOtherItem', raw(o.WeightPerQty));
        setT('txtNetWeightOtherItem', raw(o.NetWeight));
        setText('txtoRemarks', o.Remarks);
        show('btnoadd', false); show('btnoUpdate', true); show('btnoCancel', true);
        focus('CmbOtherItem');
    }
    /* btnoUpdate_Click */
    function otherUpdate() {
        if (!otherValid()) return;
        var o = S.others[S.updateIndexOther]; if (!o) return;
        o.ItemId = netI(val('CmbOtherItem')); o.ItemName = selText('CmbOtherItem');
        o.Qty = netD(val('txtoQty')); o.WeightPerQty = netD(val('txtWtPerQtyOtherItem')); o.NetWeight = netD(val('txtNetWeightOtherItem')); o.Remarks = val('txtoRemarks');
        renderOther();
        show('btnoadd', true); show('btnoUpdate', false); show('btnoCancel', false);
        resetOtherItems();
        focus('txtoQty');
    }
    /* btnoCancel_Click */
    function otherCancel() { show('btnoadd', true); show('btnoUpdate', false); show('btnoCancel', false); resetOtherItems(); }
    /* AddRowInvExpGrid */
    function otherBlank() { S.others.push({ Id: 0, ExImInvoiceId: 0, InvoiceDetailId: 0, InvoiceNo: '0', ItemId: 0, ItemName: '0', Qty: 0, WeightPerQty: 0, NetWeight: 0, Remarks: '' }); }
    function otherDelete(i) { S.others.splice(i, 1); if (!S.others.length) otherBlank(); renderOther(); calcTotals(); }
    /* grdotheritems_CellUpdated - the desktop tests the key "QTY" against the column "Qty", so a Qty edit does not recalculate. */
    function otherCellUpdated(i, key) {
        var o = S.others[i]; if (!o) return;
        if (key === 'WeightPerQty') o.NetWeight = netD(f2(netD(o.WeightPerQty) * netD(o.Qty)));
        if (key === 'NetWeight') { var q = netD(o.Qty); o.WeightPerQty = netD(f2(q > 0 ? netD(o.NetWeight) / q : 0)); }
        renderOther();
        calcTotals();
    }

    // ------------------------------------------------------------------------------ save
    /* FormValidation */
    function formValid() {
        if (need(visible('branchField') && !hasSel('CmbBranch'), 'Branch field is required', 'CmbBranch')) return false;
        if (need(val('txtdocno').trim() === '' || val('txtdocno').trim() === '0', 'DocNo Field is Required', 'txtdocno')) return false;
        if (need(netD(val('txtItemWeight')) === 0, 'Item Weight Field is Required', 'txtItemWeight')) return false;
        if (need(netD(val('txtTotalGrossWeight')) === 0, 'Total Gross Weight Field is Required', 'txtTotalGrossWeight')) return false;
        return true;
    }
    /* Insert() */
    function insert(btn) {
        if (!formValid()) return Promise.resolve();
        if (!S.rows.length) { box('Grid Record Not Found'); return Promise.resolve(); }
        if (!ask(S.recId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return Promise.resolve();
        otherWeightProportion();
        var body = {
            recId: S.recId, branchVisible: visible('branchField'), branchId: netI(val('CmbBranch')), docNo: val('txtdocno'), docDate: val('DocDate'),
            remarks: val('txtremarks'), invoiceId: netI(val('cmbInvoiceNo')), loadingPortId: netI(val('CmbLoadingPort')), transporterId: netI(val('cmbtransporter')),
            vehicleNo: val('txtVehicleNo'), itemWeight: netD(val('txtItemWeight')), packingWeight: netD(val('txtPackingWeight')), otherWeight: netD(val('txtOtherWeight')),
            totalGrossWeight: netD(val('txtTotalGrossWeight')), otherRemarks: val('txtOtherRemarks'), rows: S.rows, removed: S.removed, others: S.others
        };
        return busy(btn, function () {
            return postJson(API + '/save', body).then(function (d) {
                d = d || {};
                box(d.message);
                var docNo = d.docNo, id = d.id;
                return Promise.resolve(newForm()).then(function () {
                    openLinkedFormOnInsert(docNo);
                    if ($id('ChkPreview').checked) printSlip('262-deliveryorderslip', id, null);
                });
            });
        });
    }
    /* btnsave_Click */
    function save(btn) { S.recId = 0; return insert(btn); }
    /* btnupdate_Click */
    function update(btn) { return insert(btn); }

    /* OpenLinkedFormOnInsert(po.DocNo) - after Reset the Save button is the visible one again. */
    function openLinkedFormOnInsert(docNo) {
        if ($id('RadWBFormToOpenOnInsert').checked) { if (visible('btnsave') && !$id('btnsave').disabled) gatePassForm(); }
        else if ($id('RadGrnFormToOpenOnInsert').checked) forwardingForm();
        return docNo;
    }
    function gatePassForm() { global.open('/outward-gate-pass', '_blank'); }
    function forwardingForm() { global.open('/export/forwarding', '_blank'); }
    /* btnCroForm_Click */
    function croForm(btn) {
        var inv = netI(val('cmbInvoiceNo'));
        if (inv <= 0) { global.open('/export/shipping-booking-info', '_blank'); return Promise.resolve(); }
        var w = global.open('', '_blank');
        return busy(btn, function () {
            return getJson(API + '/cro-id?invoiceId=' + inv).then(function (id) {
                var url = '/export/shipping-booking-info' + (netI(id) > 0 ? '?id=' + netI(id) : '');
                if (w) w.location = url; else global.open(url, '_blank');
            }, function (e) { if (w) w.location = '/export/shipping-booking-info'; throw e; });
        });
    }
    function attachment() { box('Attachments (DMS) are not available in the web version of this screen.'); }

    // ------------------------------------------------------------------------------ prints
    function printSlip(key, id, btn) {
        if (!global.CrystalPrint) { box('Print is not available.'); return; }
        return global.CrystalPrint.open(key, { id: id, documentTypeId: DOC_TYPE }, btn);
    }
    /* Print_Click: InvDeliveryOrderSlipWithSubReport(RecId, 84) */
    function print262(btn) { return printSlip('262-deliveryorderslip', S.recId, btn); }
    /* btnExportDoSlip396_Click */
    function print396(btn) { if (S.recId > 0) return printSlip('396-exportdeliveryorderslip', S.recId, btn); box('No record found'); }
    /* CrystalReportPrint_Helper.OutwardGatePassPrintByInvoicesIds_295(null, InvoiceId) */
    function gpPrint(invoiceId, btn) { if (!global.CrystalPrint) { box('Print is not available.'); return; } return global.CrystalPrint.open('295-gp-by-invoice', { invoiceId: invoiceId }, btn); }
    /* CrystalReportPrint_Helper.DeliveryOrderPrintByInvoicesId(InvoiceId, 84) */
    function doPrintByInvoice(invoiceId, btn) { if (!global.CrystalPrint) { box('Print is not available.'); return; } return global.CrystalPrint.open('527-exportdeliveryorderbyinvoiceidslip', { eximInvoiceId: invoiceId, documentTypeId: DOC_TYPE }, btn); }

    // ------------------------------------------------------------------------------ read
    /* ReadById */
    function readById(id) {
        newForm();
        S.recId = id;
        return getJson(API + '/by-id?id=' + id).then(function (d) {
            d = d || {};
            var h = d.header || {};
            bind('cmbInvoiceNo', d.invoices || [], 'Id', 'InvoiceNo');
            tab('main', 'tabPage1');
            setText('DocDate', isoDate(col(h, 'DocDate')));
            setText('txtdocno', str(col(h, 'DocNo')));
            setVal('CmbBranch', col(h, 'BranchesId'));
            setText('txtremarks', str(col(h, 'LoadingInstructions')));
            setText('txtVehicleNo', str(col(h, 'VehicleNo')));
            setVal('CmbLoadingPort', netI(col(h, 'LoadingPortId')));
            setT('txtItemWeight', fmt(col(h, 'NetWeight'), 2));
            setT('txtPackingWeight', fmt(col(h, 'PackingWeight'), 2));
            setT('txtOtherWeight', fmt(col(h, 'OtherWeight'), 2));
            setT('txtTotalGrossWeight', fmt(col(h, 'GrossWeight'), 2));
            setText('txtOtherRemarks', str(col(h, 'OtherWeightRemarks')));
            if (netI(col(h, 'TransporterId')) > 0) setVal('cmbtransporter', col(h, 'TransporterId'));
            S.rows = (d.details || []).map(function (x) {
                S.contRead.push({ ContainerId: netI(col(x, 'ContainerId')), ContainerNo: str(col(x, 'Container')) });
                return {
                    Id: netI(col(x, 'Id')), InvoiceDetailId: netI(col(x, 'InvoiceDetailId')), InvoiceId: netI(col(x, 'ExImInvoiceId')), InvoiceNo: str(col(x, 'InvoiceNo')),
                    SupplierCustomerId: netI(col(x, 'SupplierCustomerId')), SupplierCustomer: str(col(x, 'SupplierCustomer')), OrderId: netI(col(x, 'SaleOrderId')), OrderNo: str(col(x, 'OrderNo')),
                    WareHouseId: netI(col(x, 'WarehouseId')), WareHouse: str(col(x, 'WareHouseName')), JobLotId: netI(col(x, 'JobLotId')), JobLot: str(col(x, 'JobLotDescription')),
                    CropYearId: netI(col(x, 'CropYearId')), CropYear: str(col(x, 'CropYear')), ItemId: netI(col(x, 'ItemId')), Item: str(col(x, 'ItemName')),
                    PackingTypeId: netI(col(x, 'InvPackingTypeId')), PackingType: str(col(x, 'PackTypeDesc')), MasterQTY: netD(col(x, 'DoQty')), MasterUOMId: netI(col(x, 'PackUomId')),
                    MasterUOM: str(col(x, 'PackUOM')), MasterUOMEquivalent: netD(col(x, 'PUomEquivalent')), EbUnitMaster: netD(col(x, 'PackingWeight')), EbTotalMaster: netD(col(x, 'OuterEbTotal')),
                    InnerQTY: netD(col(x, 'InnerQty')), InnerUOMId: netI(col(x, 'InnerUomId')), InnerUOM: str(col(x, 'InnerPackUom')), InnerUOMEquivalent: netD(col(x, 'InnerPackUomEquivalent')),
                    EbUnitInner: netD(col(x, 'InnerEbUnit')), EbTotalInner: netD(col(x, 'InnerEbTotal')), AccessWtSet: netD(col(x, 'AccessWtSet')), NetWeight: netD(col(x, 'DoWeight')),
                    LoadQty: netD(col(x, 'LoadingQty')), LoadWeight: netD(col(x, 'LoadingWeight')), GrossWeight: netD(col(x, 'GrossWeight')), OtherWeight: netD(col(x, 'OtherWeight')),
                    ThirdPartyAnalysisId: netI(col(x, 'ThirdPartyAnalysisId')), ThirdPartyAnalysisNo: str(col(x, 'ThirdPartyAnalysisNo')),
                    ThirdPartyAnalysisSubId: netI(col(x, 'ThirdPartyAnalysisSubId')), ThirdPartyAnalysisSubNo: str(col(x, 'ThirdPartyAnalysisSubNo')),
                    ContainerId: netI(col(x, 'ContainerId')), ContainerNo: str(col(x, 'Container')), ContainerRemarks: str(col(x, 'ContainerRemarks')),
                    InspectionRemarks: str(col(x, 'InspectionRemarks')), Remarks: str(col(x, 'LoadingRemarks'))
                };
            });
            renderGrid();
            calcTotals();
            S.others = (d.expenses || []).map(function (x) {
                return { Id: netI(col(x, 'Id')), ExImInvoiceId: netI(col(x, 'ExImInvoiceId')), InvoiceDetailId: netI(col(x, 'InvoiceOtherItemDetailId')), InvoiceNo: str(col(x, 'InvoiceNo')),
                    ItemId: netI(col(x, 'ItemId')), ItemName: str(col(x, 'OtherItemName')), Qty: netD(col(x, 'Qty')), WeightPerQty: netD(col(x, 'WeightPerQty')), NetWeight: netD(col(x, 'NetWeight')),
                    Remarks: str(col(x, 'Remarks')) };
            });
            renderOther();
            show('btnsave', false); show('btnupdate', true);
            applyRights();
        }).catch(function (e) { box(e.message); });
    }

    // ------------------------------------------------------------------------------ history
    /* btnResetHistory_Click (the Invoice filter is not cleared - as on the desktop). */
    function historyReset() {
        setText('FromDateHistory', daysAgo(3)); setText('ToDateHistory', today());
        setText('FromDocNoHistory', ''); setText('ToDocNoHistory', '');
        setVal('CmbCustomerHistory', '');
        S.hist = []; S.curHist = -1;
        $id('histHead').innerHTML = ''; $id('histBody').innerHTML = ''; $id('histFoot').innerHTML = '';
        $id('histDetHead').innerHTML = ''; $id('histDetBody').innerHTML = ''; $id('histDetFoot').innerHTML = '';
        show('histEmpty', false);
    }
    /* btnRefreshHistory_Click */
    function historyRefresh(btn) { return busy(btn, function () { return getJson(API + '/history-combos').then(historyCombosBind); }); }
    var H_COLS = ['DoDate', 'DoNo', 'InvoiceDate', 'InvoiceNo', 'InvoiceNetWeight', 'InvoiceGrossWeight', 'CustomerName', 'OrderNo', 'VehicleNo', 'Transporter', 'LoadingPort',
        'GpDate', 'GpSrNo', 'GpStatus', 'ForwardingNo', 'FactoryWeight', 'EntryDate', 'EntryUser', 'ModifyDate', 'ModifyUser', 'ApprovedDate', 'ApprovedUser', 'BranchName', 'NoOfAttachments'];
    function ddMMyyyyTime(v) {
        var s = str(v); var m = /^(\d{4})-(\d{2})-(\d{2})T?(\d{2})?:?(\d{2})?/.exec(s); if (!m) return '';
        var hh = netI(m[4] || 0), ap = hh >= 12 ? 'PM' : 'AM', h12 = hh % 12 === 0 ? 12 : hh % 12;
        return m[3] + '-' + m[2] + '-' + m[1] + ' ' + pad(h12) + ':' + (m[5] || '00') + ' ' + ap;
    }
    function ddMMMyy(v) { var s = isoDate(v); return s ? s.substring(8, 10) + '-' + MON[netI(s.substring(5, 7)) - 1] + '-' + s.substring(2, 4) : ''; }
    /* btnShowHistory_Click -> gridhistoryfill */
    function historyShow(btn) {
        var mode = document.querySelector('input[name="doDateMode"]:checked');
        var f = {
            fromChecked: $id('FromDateHistoryChk').checked, fromDate: val('FromDateHistory'), toChecked: $id('ToDateHistoryChk').checked, toDate: val('ToDateHistory'),
            dateMode: mode ? mode.value : 'doc', fromDocNo: netI(val('FromDocNoHistory')), toDocNo: netI(val('ToDocNoHistory')),
            customerId: netI(val('CmbCustomerHistory')), invoiceId: netI(val('CmbInvoiceNoHistory'))
        };
        return busy(btn, function () {
            return postJson(API + '/history', f).then(function (rows) {
                S.hist = rows || []; S.curHist = -1;
                if (!S.hist.length) {
                    $id('histHead').innerHTML = ''; $id('histBody').innerHTML = ''; $id('histFoot').innerHTML = '';
                    $id('histDetHead').innerHTML = ''; $id('histDetBody').innerHTML = ''; $id('histDetFoot').innerHTML = '';
                    show('histEmpty', true); return;
                }
                show('histEmpty', false);
                var cols = H_COLS.filter(function (c) { return c !== 'BranchName' || S.branchCount > 1; });
                $id('histHead').innerHTML = '<tr><th>Edit</th><th>Print_262</th><th>Print_396</th><th>GpPrint</th><th>DoPrintByInvoice</th>'
                    + cols.map(function (c) { return '<th' + (/Weight$/.test(c) ? ' class="num"' : '') + '>' + esc(c) + '</th>'; }).join('') + '</tr>';
                $id('histBody').innerHTML = S.hist.map(function (r, i) {
                    return '<tr data-i="' + i + '"><td><button type="button" class="win-btn-small" data-hedit="' + i + '">Edit</button></td>'
                        + '<td><button type="button" class="win-btn-small" data-hp262="' + i + '">Print_262</button></td>'
                        + '<td><button type="button" class="win-btn-small" data-hp396="' + i + '">Print_396</button></td>'
                        + '<td><button type="button" class="win-btn-small" data-hgp="' + i + '">GpPrint</button></td>'
                        + '<td><button type="button" class="win-btn-small" data-hdo="' + i + '">DoPrintByInvoice</button></td>'
                        + cols.map(function (c) {
                            var v = r[c];
                            if (c === 'DoNo') return '<td><a class="win-code" href="javascript:void(0)" data-hopen="' + i + '">' + esc(v) + '</a></td>';
                            if (c === 'DoDate') return '<td>' + esc(shortDate(v)) + '</td>';
                            if (c === 'InvoiceDate' || c === 'GpDate') return '<td>' + esc(ddMMMyy(v)) + '</td>';
                            if (c === 'EntryDate' || c === 'ModifyDate' || c === 'ApprovedDate') return '<td>' + esc(ddMMyyyyTime(v)) + '</td>';
                            if (/Weight$/.test(c)) return '<td class="num">' + esc(fmt(v, 4)) + '</td>';
                            if (c === 'NoOfAttachments') return '<td><a class="win-code" href="javascript:void(0)" data-hatt="' + i + '">' + esc(v) + '</a></td>';
                            return '<td>' + esc(v) + '</td>';
                        }).join('') + '</tr>';
                }).join('');
                $id('histFoot').innerHTML = '';
            });
        });
    }
    /* grdhistory_SelectionChanged -> GetDetailByHeaderId (the grid keeps the previous rows when the record has none). */
    var HD_COLS = [['InvocieNo', 'InvoiceNo'], ['CustomerName', 'SupplierCustomer'], ['OrderNo', 'OrderNo'], ['WareHouse', 'WareHouseName'], ['JobLot', 'JobLotDescription'],
        ['Item', 'ItemName'], ['ItemUOM', 'PackUOM'], ['PackingType', 'PackTypeDesc'], ['MasterQTY', 'DoQty', 1], ['MasterUOM', 'PackUOM'], ['EbUnitMaster', 'PackingWeight', 1],
        ['EbTotalMaster', 'OuterEbTotal', 1], ['InnerQTY', 'InnerQty', 1], ['InnerUOM', 'InnerPackUom'], ['EbUnitInner', 'InnerEbUnit', 1], ['EbTotalInner', 'InnerEbTotal', 1],
        ['AccessWtSet', 'AccessWtSet', 1], ['NetWeight', 'DoWeight', 1], ['LoadQty', 'LoadingQty', 1], ['LoadWeight', 'LoadingWeight', 1], ['GrossWeight', 'GrossWeight', 1],
        ['OtherWeight', 'OtherWeight', 1], ['ContainerNo', 'Container'], ['ContainerRemarks', 'ContainerRemarks'], ['InspectionRemarks', 'InspectionRemarks'], ['Remarks', 'LoadingRemarks']];
    var HD_ROWS = [];
    function histSelect(i) {
        var r = S.hist[i]; if (!r) return;
        S.curHist = i;
        getJson(API + '/history-detail?id=' + r.Id).then(function (rows) {
            rows = rows || [];
            if (!rows.length) return;
            HD_ROWS = rows;
            $id('histDetHead').innerHTML = '<tr><th>DoPrintByInvoice</th>' + HD_COLS.map(function (c) { return '<th' + (c[2] ? ' class="num"' : '') + '>' + esc(c[0]) + '</th>'; }).join('') + '</tr>';
            $id('histDetBody').innerHTML = rows.map(function (x, k) {
                return '<tr><td><button type="button" class="win-btn-small" data-ddo="' + k + '">DoPrintByInvoice</button></td>'
                    + HD_COLS.map(function (c) { var v = col(x, c[1]); return c[2] ? '<td class="num">' + esc(fmt(v, 4)) + '</td>' : '<td>' + esc(v) + '</td>'; }).join('') + '</tr>';
            }).join('');
            $id('histDetFoot').innerHTML = '<tr><td class="lbl">&Sigma;</td>' + HD_COLS.map(function (c) {
                if (!c[2]) return '<td></td>'; var t = 0; rows.forEach(function (x) { t += netD(col(x, c[1])); }); return '<td class="num">' + esc(fmt(t, 4)) + '</td>';
            }).join('') + '</tr>';
        }).catch(function (e) { box(e.message); });
    }

    // ------------------------------------------------------------------------------ Load Invoices (LoadCommercialInvoiceForDO)
    var LD_COMBOS = [['ldCmbInvoiceNo', 'Invoice'], ['ldCmbContractNo', 'ContractNo'], ['ldCmbCustomer', 'Customer'], ['ldcmbItem', 'Items'], ['ldCmbJobLot', 'JobLot'], ['ldCmbCrop', 'Crop']];
    function loaderCombosBind(d) { LD_COMBOS.forEach(function (c) { bind(c[0], d[c[1]] || [], 'Id', 'name'); }); }
    /* btnPurchaseOrderLoader_Click -> LoadCommercialInvoiceForDO (InitializeComponentCustom / InitializeComponentMethod) */
    function openLoader(btn) {
        S.ld = { data: [], grid: [], restrict: false, fromDate: '', loader: [] };
        LD_COMBOS.forEach(function (c) { bind(c[0], [], 'Id', 'name', null, ''); });
        $id('ldMainHead').innerHTML = ''; $id('ldMainBody').innerHTML = ''; $id('ldDetHead').innerHTML = ''; $id('ldDetBody').innerHTML = '';
        ['txtOrderQtyHeader', 'txtQrderWeightHeader', 'txtDispatchQtyHeader', 'txtDispatchWeightHeader', 'txtBalanceQtyHeader', 'txtBalanceWeightHeadrer'].forEach(function (k) { setText(k, ''); });
        setText('ldTodate', today());
        $id('loaderDialog').classList.add('is-open');
        return busy(btn, function () {
            return getJson(API + '/loader/setup').then(function (d) {
                d = d || {};
                S.ld.restrict = !!d.RestrictOneInvoiceOnDo;
                S.ld.fromDate = isoDate(d.fromDate);
                setText('ldFromDate', S.ld.fromDate);
                loaderCombosBind(d);
                return loaderSearch();
            }).then(function () { focus('ldFromDate'); }, function () { box('Error occurred during database call.'); });
        });
    }
    /* GridRecordsDBCall + GridRecordsFill (grouped by invoice) */
    function loaderSearch() {
        var f = { fromDate: val('ldFromDate'), toDate: val('ldTodate'), invoiceId: netI(val('ldCmbInvoiceNo')), contractId: netI(val('ldCmbContractNo')),
            customerId: netI(val('ldCmbCustomer')), itemId: netI(val('ldcmbItem')), jobLotId: netI(val('ldCmbJobLot')), cropYearId: netI(val('ldCmbCrop')) };
        return postJson(API + '/loader/search', f).then(function (rows) {
            S.ld.data = rows || [];
            var groups = [], map = {};
            S.ld.data.forEach(function (r) {
                var k = netI(col(r, 'ExImInvoiceId'));
                if (!map[k]) { map[k] = { ExImInvoiceId: k, InvoiceNo: str(col(r, 'InvoiceNo')), InvoiceDate: col(r, 'InvoiceDate'), SupplierCustomerId: netI(col(r, 'SupplierCustomerId')),
                    CustomerName: str(col(r, 'CustomerName')), InvoiceQty: 0, ReturnQty: 0, DOQty: 0, BalQty: 0, InvoiceWeight: 0, ReturnWeight: 0, DOWeight: 0, BalWeight: 0, checked: false }; groups.push(map[k]); }
                var g = map[k];
                g.InvoiceQty += netD(col(r, 'ItemQty')); g.DOQty += netD(col(r, 'DoQty')); g.ReturnQty += netD(col(r, 'ReturnQty')); g.BalQty += netD(col(r, 'BalQty'));
                g.InvoiceWeight += netD(col(r, 'NetWeight')); g.ReturnWeight += netD(col(r, 'ReturnWeight')); g.DOWeight += netD(col(r, 'DoNetWeight')); g.BalWeight += netD(col(r, 'BalWeight'));
            });
            S.ld.grid = groups;
            if (!groups.length) { $id('ldMainHead').innerHTML = ''; $id('ldMainBody').innerHTML = ''; $id('ldDetHead').innerHTML = ''; $id('ldDetBody').innerHTML = ''; return; }
            var cols = ['InvoiceNo', 'InvoiceDate', 'CustomerName', 'InvoiceQty', 'ReturnQty', 'DOQty', 'BalQty', 'InvoiceWeight', 'ReturnWeight', 'DOWeight', 'BalWeight'];
            $id('ldMainHead').innerHTML = '<tr><th>Select</th>' + cols.map(function (c) { return '<th' + (/Qty$|Weight$/.test(c) ? ' class="num"' : '') + '>' + esc(c) + '</th>'; }).join('') + '</tr>';
            $id('ldMainBody').innerHTML = groups.map(function (g, i) {
                return '<tr data-i="' + i + '"><td><input type="checkbox" data-ldm="' + i + '"/></td>' + cols.map(function (c) {
                    if (c === 'InvoiceDate') return '<td>' + esc(ddMMMyy(g[c])) + '</td>';
                    if (/Qty$|Weight$/.test(c)) return '<td class="num">' + esc(fmt(g[c], 4)) + '</td>';
                    return '<td>' + esc(g[c]) + '</td>';
                }).join('') + '</tr>';
            }).join('');
            $id('ldDetHead').innerHTML = ''; $id('ldDetBody').innerHTML = '';
        }).catch(function (e) { box(e.message); });
    }
    /* HandleMainRowSelectionOrCheck: CalculateTotal, DetailGridBind(checked invoices), CheckedAllDetailRows */
    function loaderMainChanged() {
        var checked = S.ld.grid.filter(function (g) { return g.checked; });
        var t = { iq: 0, iw: 0, dq: 0, dw: 0, rq: 0, rw: 0, bq: 0, bw: 0 };
        checked.forEach(function (g) { t.iq += g.InvoiceQty; t.iw += g.InvoiceWeight; t.dq += g.DOQty; t.dw += g.DOWeight; t.rq += g.ReturnQty; t.rw += g.ReturnWeight; t.bq += g.BalQty; t.bw += g.BalWeight; });
        setText('txtOrderQtyHeader', fmt(t.iq + t.rq, 2)); setText('txtQrderWeightHeader', fmt(t.iw + t.rw, 2));
        setText('txtDispatchQtyHeader', fmt(t.dq, 2)); setText('txtDispatchWeightHeader', fmt(t.dw, 2));
        setText('txtBalanceQtyHeader', fmt(t.bq, 2)); setText('txtBalanceWeightHeadrer', fmt(t.bw, 2));
        var ids = checked.map(function (g) { return g.ExImInvoiceId; });
        var rows = S.ld.data.filter(function (r) { return ids.indexOf(netI(col(r, 'ExImInvoiceId'))) >= 0; });
        if (!rows.length) { $id('ldDetHead').innerHTML = ''; $id('ldDetBody').innerHTML = ''; return; }
        var cols = ['InvoiceNo', 'InvoiceDate', 'CustomerName', 'ContractNo', 'ContractNoShipmentWise', 'JobLotDescription', 'CropYear', 'ItemName', 'PackTypeDesc', 'PackUom',
            'ItemQty', 'ReturnQty', 'DoQty', 'BalQty', 'NetWeight', 'ReturnWeight', 'DoNetWeight', 'BalWeight'];
        $id('ldDetHead').innerHTML = '<tr><th>Select</th>' + cols.map(function (c) { return '<th' + (/Qty$|Weight$/.test(c) ? ' class="num"' : '') + '>' + esc(c === 'PackTypeDesc' ? 'Packing Type' : c) + '</th>'; }).join('') + '</tr>';
        $id('ldDetBody').innerHTML = rows.map(function (r) {
            return '<tr><td><input type="checkbox" data-ldd="' + netI(col(r, 'InvoiceDetailId')) + '" data-inv="' + netI(col(r, 'ExImInvoiceId')) + '" checked/></td>' + cols.map(function (c) {
                var v = col(r, c);
                if (c === 'InvoiceDate') return '<td>' + esc(ddMMMyy(v)) + '</td>';
                if (/Qty$|Weight$/.test(c)) return '<td class="num">' + esc(fmt(v, 4)) + '</td>';
                return '<td>' + esc(v) + '</td>';
            }).join('') + '</tr>';
        }).join('');
    }
    /* btnLoadOnInvoice_Click_1 */
    function loaderLoad() {
        try {
            var checked = Array.prototype.slice.call($id('ldDetBody').querySelectorAll('input[data-ldd]:checked'));
            if (!checked.length) { box('Please check at least one row.'); return; }
            var inv = []; checked.forEach(function (c) { var v = netI(c.getAttribute('data-inv')); if (inv.indexOf(v) < 0) inv.push(v); });
            if (S.ld.restrict && inv.length > 1) { box('You can only select rows from the same Invoice.'); return; }
            var list = [];
            checked.forEach(function (c) {
                var id = netI(c.getAttribute('data-ldd')), m = null;
                for (var k = 0; k < S.ld.data.length; k++) if (netI(col(S.ld.data[k], 'InvoiceDetailId')) === id) { m = S.ld.data[k]; break; }
                if (m) list.push(m);
            });
            $id('loaderDialog').classList.remove('is-open');
            loadInGridDetail(list);
        } catch (e) { box('An error occurred: ' + e.message); }
    }
    /* Escape / Ctrl+E set dtLoader = null; LoadInGridDetail then fails reading it - the desktop shows the exception text. */
    function loaderClose(nulled) {
        $id('loaderDialog').classList.remove('is-open');
        if (nulled) box('Object reference not set to an instance of an object.');
    }
    function loaderReset(btn) {
        focus('ldFromDate');
        LD_COMBOS.forEach(function (c) { setVal(c[0], ''); });
        return busy(btn, function () { return getJson(API + '/loader/setup').then(function (d) { S.ld.restrict = !!(d || {}).RestrictOneInvoiceOnDo; return loaderSearch(); }); });
    }
    function loaderRefresh(btn) { return busy(btn, function () { return getJson(API + '/loader/setup').then(function (d) { d = d || {}; S.ld.restrict = !!d.RestrictOneInvoiceOnDo; loaderCombosBind(d); }); }); }
    function loaderShow(btn) { return busy(btn, loaderSearch); }

    /* LoadInGridDetail + LoadOtherItemsData */
    function loadInGridDetail(list) {
        if (!list || !list.length) return Promise.resolve();
        var ids = '';
        list.forEach(function (r) { ids += ',' + netI(col(r, 'ExImInvoiceId')); });
        try {
            var distinct = [];
            list.forEach(function (r) { var v = col(r, 'ExImInvoiceId'); if (v !== null && v !== undefined && distinct.indexOf(String(v)) < 0) distinct.push(String(v)); });
            if (CFG.RestrictOneInvoiceOnDo && distinct.length > 1) throw new Error('You have Selected Multiple different Invoices.but Only rows with the same Invoice are allowed.');
            list.forEach(function (r) {
                var flag = false;
                if (S.rows.length) {
                    flag = S.rows.some(function (g) { return netI(g.InvoiceDetailId) === netI(col(r, 'InvoiceDetailId')); });
                    if (CFG.RestrictOneInvoiceOnDo && !anyInvoice(netI(col(r, 'ExImInvoiceId')), -1)) throw new Error("Data Against another Invoice Exists in Grid,So you can't add different Invoice");
                }
                if (!flag) {
                    S.rows.push({
                        Id: 0, InvoiceDetailId: netI(col(r, 'InvoiceDetailId')), InvoiceId: netI(col(r, 'ExImInvoiceId')), InvoiceNo: str(col(r, 'InvoiceNo')),
                        SupplierCustomerId: netI(col(r, 'SupplierCustomerId')), SupplierCustomer: str(col(r, 'CustomerName')), OrderId: netI(col(r, 'ContractId')), OrderNo: str(col(r, 'ContractNo')),
                        WareHouseId: 0, WareHouse: '', JobLotId: netI(col(r, 'JobLotId')), JobLot: str(col(r, 'JobLotDescription')), CropYearId: netI(col(r, 'CropYearId')), CropYear: str(col(r, 'CropYear')),
                        ItemId: netI(col(r, 'ItemId')), Item: str(col(r, 'ItemName')), PackingTypeId: netI(col(r, 'PackingMaterialTypeId')), PackingType: str(col(r, 'PackTypeDesc')),
                        MasterQTY: netD(col(r, 'BalQty')), MasterUOMId: netI(col(r, 'PackUomId')), MasterUOM: str(col(r, 'PackUom')), MasterUOMEquivalent: netD(col(r, 'PackEquivalent')),
                        EbUnitMaster: 0, EbTotalMaster: 0, InnerQTY: 0, InnerUOMId: 0, InnerUOM: '', InnerUOMEquivalent: 0, EbUnitInner: 0, EbTotalInner: 0, AccessWtSet: 0,
                        NetWeight: netD(col(r, 'BalWeight')), LoadQty: netD(col(r, 'BalQty')), LoadWeight: netD(col(r, 'BalWeight')), GrossWeight: netD(col(r, 'BalWeight')), OtherWeight: 0,
                        ThirdPartyAnalysisId: netI(col(r, 'ThirdPartyAnalysisId')), ThirdPartyAnalysisNo: str(col(r, 'ThirdPartyAnalysisNo')), ThirdPartyAnalysisSubId: 0, ThirdPartyAnalysisSubNo: '',
                        ContainerId: 0, ContainerNo: '', ContainerRemarks: '', InspectionRemarks: '', Remarks: ''
                    });
                }
            });
        } catch (e) { renderGrid(); box(e.message); return Promise.resolve(); }
        renderGrid();
        return getJson(API + '/loader/other-items?ids=' + encodeURIComponent(ids)).then(function (rows) {
            (rows || []).forEach(function (o) {
                if (S.others.some(function (x) { return netI(x.InvoiceDetailId) === netI(col(o, 'Id')); })) return;
                var q = netD(col(o, 'oItemQty')), w = netD(col(o, 'oItemWeightKgs'));
                S.others.push({ Id: 0, ExImInvoiceId: netI(col(o, 'ExImInvoiceId')), InvoiceDetailId: netI(col(o, 'Id')), InvoiceNo: str(col(o, 'InvoiceNo')), ItemId: netI(col(o, 'ItemId')),
                    ItemName: str(col(o, 'ItemName')), Qty: q, WeightPerQty: w, NetWeight: w * q, Remarks: str(col(o, 'OtherItemRemarks')) });
            });
            renderOther();
        }).catch(function (e) { box(e.message); }).then(function () { try { calcTotals(); } catch (e) { box(e.message); } });
    }

    // ------------------------------------------------------------------------------ shortcut keys
    var KEYS = [['Ctrl+S', 'For Save'], ['Ctrl+U', 'For Update'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Alt+1', 'For Print 262'],
        ['Alt+2', 'For Print 396'], ['Ctrl+F5', 'For Focus on Doc Date'], ['Ctrl+F10', 'For Open Attachments'], ['Ctrl+T', 'For Tab Transfer'], ['Ctrl+alt', 'To Show ShortCut Keys Form'],
        ['Ctrl+ArrowDown', 'For Focus On Detail Grid'], ['Ctrl+ArrowUp', 'For Focus On Invoice no in Detail Box'], ['Ctrl+Enter', 'When Focus On Any Grid For Update Record'],
        ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]];
    var LD_KEYS = [['Ctrl+E', 'For Close'], ['Ctrl+N', 'For New'], ['Ctrl+R', 'For Refresh'], ['Ctrl+L', 'To Press Load Button'], ['Ctrl+S', 'For Search'],
        ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]];
    function showKeys(list) { $id('shortcutBody').innerHTML = list.map(function (k) { return '<tr><td>' + esc(k[0]) + '</td><td>' + esc(k[1]) + '</td></tr>'; }).join(''); $id('shortcutDialog').classList.add('is-open'); }
    function shortcuts() { showKeys(KEYS); }
    function loaderShortcuts() { showKeys(LD_KEYS); }
    function closeDialog(id) { $id(id).classList.remove('is-open'); }

    // ------------------------------------------------------------------------------ wiring
    function on(id, ev, fn) { var e = $id(id); if (e) e.addEventListener(ev, fn); }
    document.addEventListener('DOMContentLoaded', function () {
        document.querySelectorAll('.win-tabs .win-tab').forEach(function (b) { b.addEventListener('click', function () { tab(b.closest('.win-tabs').getAttribute('data-tabs'), b.getAttribute('data-tab')); }); });
        document.querySelectorAll('button[data-fullscreen]').forEach(function (b) { b.addEventListener('click', function () { var x = $id(b.getAttribute('data-fullscreen')); if (x) x.classList.toggle('ex-fullscreen'); }); });
        Object.keys(TC).forEach(function (id) { on(id, 'input', TC[id]); });
        on('txtQtyMaster', 'blur', guard(qtyMasterLeave));
        on('cmbInvoiceNo', 'change', invoiceLeave);                 /* TextChanged (weights) + Leave */
        on('CmbOrderNo', 'change', orderChanged);
        on('CmbItemName', 'change', itemChanged);
        on('CmbInspectionNoPackListDetail', 'change', inspectionLeave);
        on('CmbPackUomMaster', 'change', guard(uomMasterLeave));
        on('CmbPackUomInner', 'change', guard(autoInnerQty));

        var gb = $id('grdBody');
        gb.addEventListener('click', function (e) {
            var b = e.target.closest('[data-del]'); if (b) { detailDelete(+b.getAttribute('data-del')); return; }
            b = e.target.closest('[data-add]'); if (b) { detailCopy(+b.getAttribute('data-add')); return; }
            b = e.target.closest('[data-edit]'); if (b) { detailEdit(+b.getAttribute('data-edit')); return; }
            b = e.target.closest('[data-pick]'); if (b) { pick(+b.getAttribute('data-i'), b.getAttribute('data-pick'), b); return; }
            var tr = e.target.closest('tr[data-i]'); if (tr) { S.curRow = +tr.getAttribute('data-i'); gb.querySelectorAll('tr').forEach(function (x) { x.classList.toggle('is-current', x === tr); }); }
        });
        gb.addEventListener('dblclick', function (e) { var tr = e.target.closest('tr[data-i]'); if (tr && !e.target.closest('button, input, select')) detailEdit(+tr.getAttribute('data-i')); });
        gb.addEventListener('change', function (e) {
            var c = e.target.closest('[data-cell]'); if (!c) return;
            var i = +c.getAttribute('data-i'), k = c.getAttribute('data-cell'), r = S.rows[i]; if (!r) return;
            if (c.tagName === 'SELECT') r[k] = netI(c.value);
            else if (c.classList.contains('num')) r[k] = netD(c.value);
            else r[k] = c.value;
            try { cellUpdated(i, k); } catch (x) { box(x.message); }
        });
        gb.addEventListener('keydown', function (e) {
            var c = e.target.closest('td'); if (!c) return;
            var tr = c.closest('tr[data-i]'); if (!tr) return;
            var i = +tr.getAttribute('data-i');
            if (e.key === 'F1') { var p = c.querySelector('[data-pick]'); if (p) { e.preventDefault(); pick(i, p.getAttribute('data-pick'), p); } }
            if (e.ctrlKey && e.key === 'Enter') { e.preventDefault(); detailEdit(i); }
        });

        var ob = $id('otherBody');
        ob.addEventListener('click', function (e) {
            var b = e.target.closest('[data-odel]'); if (b) { otherDelete(+b.getAttribute('data-odel')); return; }
            b = e.target.closest('[data-oadd]'); if (b) { otherBlank(); renderOther(); calcTotals(); return; }
            b = e.target.closest('[data-oedit]'); if (b) { otherEdit(+b.getAttribute('data-oedit')); renderOther(); calcTotals(); return; }
            var tr = e.target.closest('tr[data-i]'); if (tr) S.curOther = +tr.getAttribute('data-i');
        });
        ob.addEventListener('dblclick', function (e) { var tr = e.target.closest('tr[data-i]'); if (tr && !e.target.closest('button, input, select')) otherEdit(+tr.getAttribute('data-i')); });
        ob.addEventListener('change', function (e) {
            var c = e.target.closest('[data-ocell]'); if (!c) return;
            var i = +c.getAttribute('data-i'), k = c.getAttribute('data-ocell'), o = S.others[i]; if (!o) return;
            if (k === 'ItemId') { o.ItemId = netI(c.value); o.ItemName = c.selectedIndex > 0 ? c.options[c.selectedIndex].textContent : ''; return; }   /* F1 GrdPopUp: no CellUpdated */
            o[k] = c.classList.contains('num') ? netD(c.value) : c.value;
            try { otherCellUpdated(i, k); } catch (x) { box(x.message); }
        });
        ob.addEventListener('keydown', function (e) {
            var tr = e.target.closest('tr[data-i]'); if (!tr) return;
            var i = +tr.getAttribute('data-i');
            if (e.ctrlKey && (e.key === 'd' || e.key === 'D')) { e.preventDefault(); otherBlank(); renderOther(); }
            else if (e.ctrlKey && e.key === 'Delete') { e.preventDefault(); S.others.splice(i, 1); renderOther(); }
        });

        var hb = $id('histBody');
        hb.addEventListener('click', function (e) {
            var b = e.target.closest('[data-hedit]'); if (b) { readById(S.hist[+b.getAttribute('data-hedit')].Id); return; }
            b = e.target.closest('[data-hopen]'); if (b) { readById(S.hist[+b.getAttribute('data-hopen')].Id); return; }
            b = e.target.closest('[data-hp262]'); if (b) { printSlip('262-deliveryorderslip', S.hist[+b.getAttribute('data-hp262')].Id, b); return; }
            b = e.target.closest('[data-hp396]'); if (b) { printSlip('396-exportdeliveryorderslip', S.hist[+b.getAttribute('data-hp396')].Id, b); return; }
            b = e.target.closest('[data-hgp]'); if (b) { gpPrint(S.hist[+b.getAttribute('data-hgp')].InvoiceId, b); return; }
            b = e.target.closest('[data-hdo]'); if (b) { doPrintByInvoice(S.hist[+b.getAttribute('data-hdo')].InvoiceId, b); return; }
            b = e.target.closest('[data-hatt]'); if (b) { attachment(); return; }
            var tr = e.target.closest('tr[data-i]'); if (tr) { hb.querySelectorAll('tr').forEach(function (x) { x.classList.toggle('is-current', x === tr); }); histSelect(+tr.getAttribute('data-i')); }
        });
        hb.addEventListener('dblclick', function (e) { var tr = e.target.closest('tr[data-i]'); if (tr && !e.target.closest('button, a')) readById(S.hist[+tr.getAttribute('data-i')].Id); });
        $id('histDetBody').addEventListener('click', function (e) {
            var b = e.target.closest('[data-ddo]'); if (b) doPrintByInvoice(netI(col(HD_ROWS[+b.getAttribute('data-ddo')], 'ExImInvoiceId')), b);
        });

        $id('ldMainBody').addEventListener('change', function (e) {
            var c = e.target.closest('[data-ldm]'); if (!c) return;
            var g = S.ld.grid[+c.getAttribute('data-ldm')]; if (g) g.checked = c.checked;
            loaderMainChanged();
        });
        $id('ldMainBody').addEventListener('click', function (e) {
            var tr = e.target.closest('tr[data-i]'); if (!tr || e.target.closest('input')) return;
            $id('ldMainBody').querySelectorAll('tr').forEach(function (x) { x.classList.toggle('is-current', x === tr); });
            loaderMainChanged();
        });

        /* PurchsaeOrder_KeyDown (and LoadPurchaseOrder_KeyDown while the loader is open) */
        document.addEventListener('keydown', function (e) {
            var k = (e.key || '').toLowerCase();
            if (e.key === 'Enter' && !e.ctrlKey && e.target && /^(INPUT|SELECT)$/.test(e.target.tagName) && e.target.type !== 'checkbox' && !e.target.closest('.win-grid')) {
                var f = Array.prototype.filter.call(document.querySelectorAll('input:not([type=hidden]):not(:disabled), select:not(:disabled), button:not(:disabled), textarea'), function (x) { return x.offsetParent !== null; });
                var i = f.indexOf(e.target); if (i >= 0 && i + 1 < f.length) { e.preventDefault(); f[i + 1].focus(); } return;
            }
            if ($id('loaderDialog').classList.contains('is-open')) {
                if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { e.preventDefault(); loaderClose(true); }
                else if (e.ctrlKey && e.altKey) loaderShortcuts();
                else if (e.ctrlKey && k === 's') { e.preventDefault(); loaderShow($id('ldbtngrnlod')); }
                else if (e.ctrlKey && k === 'l') { e.preventDefault(); loaderLoad(); }
                else if (e.ctrlKey && k === 'n') { e.preventDefault(); loaderReset($id('ldbtnNew')); }
                else if (e.ctrlKey && k === 'r') { e.preventDefault(); loaderRefresh($id('ldbtnRefresh')); }
                else if (e.ctrlKey && (e.key === 'F5' || e.key === 'ArrowUp')) { e.preventDefault(); focus('ldFromDate'); }
                return;
            }
            if ($id('shortcutDialog').classList.contains('is-open') && e.key === 'Escape') { closeDialog('shortcutDialog'); return; }
            if (e.ctrlKey && e.altKey) { shortcuts(); return; }
            if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { e.preventDefault(); cancel(); return; }
            if (e.ctrlKey && k === 't') { e.preventDefault(); toggleHistory(); return; }
            if (activeTab('main') === 'tabPage1') {
                if (e.ctrlKey && k === 's' && visible('btnsave') && !$id('btnsave').disabled) { e.preventDefault(); save($id('btnsave')); }
                if (e.ctrlKey && k === 'u' && visible('btnupdate') && !$id('btnupdate').disabled) { e.preventDefault(); update($id('btnupdate')); }
                if (e.ctrlKey && k === 'n') { e.preventDefault(); newForm(); }
                if (e.ctrlKey && k === 'r') { e.preventDefault(); refresh($id('btnRefresh')); }
                if (e.ctrlKey && e.key === 'F5') { e.preventDefault(); focus('DocDate'); }
                if (e.ctrlKey && e.key === 'ArrowDown') { e.preventDefault(); $id('grdBox').scrollIntoView(); }
                if (e.ctrlKey && e.key === 'ArrowUp') { e.preventDefault(); focus('cmbInvoiceNo'); }
                if (e.ctrlKey && e.key === 'F10') { e.preventDefault(); attachment(); }
                if (e.ctrlKey && k === 'p' && PERM.Print) { e.preventDefault(); print262($id('btnprint')); }
                if (e.altKey && (e.code === 'Digit1' || e.code === 'Numpad1') && PERM.Print) { e.preventDefault(); print262($id('btnprint')); }
                if (e.altKey && (e.code === 'Digit2' || e.code === 'Numpad2') && PERM.Print) { e.preventDefault(); print396($id('btnExportDoSlip396')); }
                return;
            }
            if (e.ctrlKey && k === 's') { e.preventDefault(); historyShow($id('btnShowHistory')); }
            if (e.ctrlKey && k === 'n') { e.preventDefault(); historyReset(); }
            if (e.ctrlKey && k === 'r') { e.preventDefault(); historyRefresh($id('btnRefreshHistory')); }
            if (e.ctrlKey && (e.key === 'F5' || e.key === 'ArrowUp')) { e.preventDefault(); focus('FromDateHistory'); }
            if (e.ctrlKey && e.key === 'ArrowDown') { e.preventDefault(); $id('grdhistoryBox').scrollIntoView(); }
            if (e.ctrlKey && e.key === 'ArrowRight') { e.preventDefault(); $id('grdDetailBox').scrollIntoView(); }
            if (e.ctrlKey && e.key === 'Enter' && S.curHist >= 0 && PERM.Update) { e.preventDefault(); readById(S.hist[S.curHist].Id); }
        });
        load();
    });

    global.ExportDo = {
        newForm: newForm, refresh: refresh, save: save, update: update, print262: print262, print396: print396, attachment: attachment, shortcuts: shortcuts,
        closeDialog: closeDialog, openLoader: openLoader, croForm: croForm, gatePassForm: gatePassForm, forwardingForm: forwardingForm,
        detailAdd: detailAdd, detailUpdate: detailUpdate, detailCancel: detailCancel, otherAdd: otherAdd, otherUpdate: otherUpdate, otherCancel: otherCancel,
        historyReset: historyReset, historyRefresh: historyRefresh, historyShow: historyShow, toggleHistory: toggleHistory,
        loaderClose: function () { loaderClose(false); }, loaderReset: loaderReset, loaderRefresh: loaderRefresh, loaderShortcuts: loaderShortcuts, loaderShow: loaderShow, loaderLoad: loaderLoad
    };
}(window));
