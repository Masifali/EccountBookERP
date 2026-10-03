/* ============================================================================================
 * countx_export_forwarding_new.js - frmForwardingNew.cs (Architecture.WinApp.Export), screen 793
 * "Export Forwarding (New)" (DocumentTypeId 210). Every button, TextChanged, Leave, CheckedChanged,
 * grid column button / link and KeyDown of the desktop form has its counterpart here, with the
 * desktop's messages and order. Data: /api/export/forwarding-new (ExportForwardingController ->
 * ExportForwardingNewService -> ExportForwardingRepository, same procedures as the desktop BLL/DAL).
 * ========================================================================================== */
(function (global) {
    'use strict';

    var API = '/api/export/forwarding-new';
    var DOC_TYPE = 210;

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
    function print507(id, btn) { if (global.CrystalPrint) return global.CrystalPrint.open('507-eximforwarding-slip', { id: id }, btn); box('Print is not available.'); }


    // ------------------------------------------------------------------------------ state
    var PERM = { Save: true, Update: true, Delete: true };
    var CFG = {};
    var S = { recId: 0, preStockWt: 0, updateMode: false, rows: [], removed: [], others: [], updateIndex: -1, updateIndexOther: -1,
        gpGrid: [], hist: [], curHist: -1, curDetail: -1, uoms: [], warehouses: [], ports: [] };

    function applyRights() {
        $id('btnSave').disabled = !PERM.Save;
        $id('btnUpdate').disabled = !PERM.Update;
        $id('btnDelete').disabled = !PERM.Delete;
    }
    function fcy(v) { var d = netI(CFG.FcyDecimals); var n = netD(v); var s = n.toFixed(d); var p = s.split('.'); p[0] = p[0].replace(/\B(?=(\d{3})+(?!\d))/g, ','); return p.join('.'); }

    // ------------------------------------------------------------------------------ tabs
    function tab(group, panelId) {
        var tabs = document.querySelector('.win-tabs[data-tabs="' + group + '"]'); if (!tabs) return;
        tabs.querySelectorAll('.win-tab').forEach(function (b) { b.classList.toggle('is-active', b.getAttribute('data-tab') === panelId); });
        Array.prototype.forEach.call(tabs.parentNode.children, function (p) { if (p.classList.contains('win-tab-panel')) p.classList.toggle('is-active', p.id === panelId); });
        if (group === 'main') {
            var onHist = panelId === 'tabHistory', fb = $id('btnFooterHistory');
            fb.querySelector('span').textContent = onHist ? 'Form' : 'History';
            fb.querySelector('i').className = onHist ? 'fa fa-file-text-o' : 'fa fa-history';
            if (onHist) focus('FromDateHistory'); else focus('DocDate');       // tabControl1_SelectedIndexChanged
        }
    }
    function activeTab(group) { var t = document.querySelector('.win-tabs[data-tabs="' + group + '"] .win-tab.is-active'); return t ? t.getAttribute('data-tab') : ''; }
    function toggleHistory() { tab('main', activeTab('main') === 'tabHistory' ? 'tabForm' : 'tabHistory'); }

    // ------------------------------------------------------------------------------ load
    /* ExpfrmForwarding_Load */
    function load() {
        return getJson(API + '/setup').then(function (d) {
            d = d || {};
            if (d.permissions) PERM = d.permissions;
            CFG = d.config || {};
            applyRights();
            errs(d, ['gatePasses', 'gatePassGrid', 'docNo', 'packingTypes', 'jobLots', 'warehouses', 'cropYears', 'transporterId', 'transporters', 'ports', 'items', 'lastWarehouseToId', 'historyCustomers']);
            gatePassesBind(d.gatePasses);
            S.gpGrid = d.gatePassGrid || []; gpRender();
            setText('txtdocno', netI(d.docNo) > 0 ? d.docNo : '');
            bind('cmbPackType', d.packingTypes, 'Id', 'PackTypeDesc');
            bind('CmbJobLot', d.jobLots, 'Id', 'JobLotDescription');
            warehousesBind(d.warehouses);
            bind('CmbCropYear', d.cropYears, 'Id', 'CropYear');
            bind('cmbtransporter', d.transporters, 'Id', 'CompanyName', [], netI(d.transporterId) > 0 ? d.transporterId : '');
            portsBind(d.ports);
            bind('cmbItem', d.items, 'Id', 'ItemName');
            if (netI(d.lastWarehouseToId) > 0) setVal('cmbWareHouseTo', d.lastWarehouseToId);
            bind('CmbCustomerHistory', d.historyCustomers, 'Id', 'Name');
            setText('DocDate', today()); setText('txtGPDate', today());
            show('btnDetailUpdate', false); show('btnDetailCancel', false);
            var days = netI(CFG.DefaultDaysToLessFromHistoryFromDate);
            setText('FromDateHistory', daysAgo(days > 0 ? days : 3)); setText('ToDateHistory', today());
            detailRender(); otherRender();
            $id('fnFooterInfo').textContent = 'frmForwardingNew  -  Document Type 210';
            focus('cmbGpNo');
        }).catch(function (e) { box(e.message || 'Error occurred during database call.'); });
    }
    /* WarehouseFill: the same list for the four warehouse combos. */
    function warehousesBind(rows) {
        S.warehouses = rows || [];
        ['CmbWareHouseFrom', 'cmbWareHouseTo', 'cmbWareHouseFromOtherItem', 'cmbWareHouseToOtherItem'].forEach(function (i) { bind(i, S.warehouses, 'Id', 'WareHouseName'); });
    }
    /* bindSeaPort: every port in both combos. */
    function portsBind(rows) { S.ports = rows || []; bind('cmbLoadingPort', S.ports, 'Id', 'PortName'); bind('cmbDestinationPort', S.ports, 'Id', 'PortName'); }
    /* gatepassGetAll */
    function gatePassesBind(rows) {
        bind('cmbGpNo', rows, 'Id', 'GpSrNo');
        if (!rows || !rows.length) {
            setVal('cmbGpNo', ''); setText('txtGPDate', today());
            ['txtBilityNo', 'txtVehicleNo', 'txtFreight', 'txtconainer1', 'txtContainer2', 'txtFactoryWeight'].forEach(function (i) { setText(i, ''); });
        }
    }
    /* btnRefresh_Click */
    function refresh(btn) {
        return busy(btn, function () {
            return getJson(API + '/refresh').then(function (d) {
                d = d || {}; CFG = d.config || CFG;
                errs(d, ['gatePasses', 'transporters', 'packingTypes', 'ports', 'jobLots', 'warehouses']);
                gatePassesBind(d.gatePasses);
                bind('cmbtransporter', d.transporters, 'Id', 'CompanyName');
                bind('cmbPackType', d.packingTypes, 'Id', 'PackTypeDesc');
                portsBind(d.ports);
                bind('CmbJobLot', d.jobLots, 'Id', 'JobLotDescription');
                warehousesBind(d.warehouses);
            });
        });
    }

    // ------------------------------------------------------------------------------ gate pass / invoice
    /* GetGatePassDataForForwarding */
    function applyGatePass(g) {
        if (g) {
            setText('txtVehicleNo', rawText(col(g, 'VehicleNo')));
            setText('txtBilityNo', rawText(col(g, 'BiltyNo')));
            setText('txtGPDate', isoDate(col(g, 'GpDate')) || today());
            setText('txtFreight', fcy(col(g, 'Freight')));
            var fc = netD(col(g, 'WbGrossWeight')) - netD(col(g, 'StockWeight')) + S.preStockWt;
            setText('txtFactoryWeight', fmt(col(g, 'WbGrossWeight')));
            setText('txtDispatchWeight', fmt(netD(col(g, 'StockWeight')) - S.preStockWt));
            setText('txtBalFcWeight', fmt(fc));
            setText('txtDoGrossWeight', fmt(col(g, 'DoGrossWeight')));
            setText('txtDoNetWeight', fmt(col(g, 'DoWeight')));
            setText('txtconainer1', rawText(col(g, 'Container')));
            setText('txtContainer2', rawText(col(g, 'Container1')));
            var c = [{ Id: rawText(col(g, 'Container')), Container: rawText(col(g, 'Container')) }, { Id: rawText(col(g, 'Container1')), Container: rawText(col(g, 'Container1')) }];
            bind('cmbcontainerNo', c, 'Id', 'Container', [], '');
        } else {
            ['txtDoNetWeight', 'txtVehicleNo', 'txtBilityNo', 'txtconainer1', 'txtContainer2'].forEach(function (i) { setText(i, ''); });
            setText('txtFactoryWeight', '0'); setText('txtFreight', '0');
        }
    }
    /* cmbGpNo_Leave: GetGatePassDataForForwarding + bindInvoiceNo. */
    function gpLeave() {
        var gp = netI(val('cmbGpNo'));
        return getJson(API + '/gate-pass?gpId=' + gp).then(function (d) {
            d = d || {};
            applyGatePass(d.gatePass);
            var inv = d.invoices || [];
            if (inv.length > 0) { setText('txtNoofInvoices', String(inv.length)); bind('cmbInvoiceNo', inv, 'Id', 'InvoiceNo'); }
            else bind('cmbInvoiceNo', [], 'Id', 'InvoiceNo');
        }).catch(function (e) { box(e.message); });
    }
    /* cmbInvoiceNo_Leave: GetDeliveryOrderbyGatepassId + ContractORDERNo. */
    function invoiceLeave() {
        var inv = netI(val('cmbInvoiceNo')), gp = netI(val('cmbGpNo'));
        return getJson(API + '/invoice?gpId=' + gp + '&invoiceId=' + inv).then(function (d) {
            d = d || {};
            var dos = d.deliveryOrder || [];
            if (dos.length > 0) {
                var totalNet = 0, totalGross = 0, items = [], newRows = [];
                dos.forEach(function (r) {
                    totalNet += netD(col(r, 'NetWeight')); totalGross += netD(col(r, 'GrossWeight'));
                    items.push({ Id: netI(col(r, 'ItemId')), ItemName: str(col(r, 'ItemName')) });
                    if (S.recId === 0 && !visible('btnUpdate')) {
                        var pw = netD(col(r, 'PackingWeight')), oq = netD(col(r, 'OuterQty')), nw = netD(col(r, 'NetWeight'));
                        newRows.push({
                            Id: 0, RefDocumentTypeId: netI(col(r, 'RefDocumentTypeId')), RefDocIdNo: netI(col(r, 'RefDocIdNo')), RefDocSubIdNo: netI(col(r, 'RefDocSubIdNo')),
                            ProformaDetailId: netI(col(r, 'ContractDetailId')), ProformaId: netI(col(r, 'ContractId')), ProformaNo: str(col(r, 'ProformaNo')),
                            PreInvoiceId: inv, PreInvoiceDetailId: netI(col(r, 'InvoiceDetailId')), ItemId: netI(col(r, 'ItemId')), ItemName: str(col(r, 'ItemName')),
                            CropYearId: netI(col(r, 'CropYearId')), CropYear: str(col(r, 'CropYear')), PackTypeId: netI(col(r, 'PackingMaterialTypeId')), Packtype: str(col(r, 'PackMaterilaType')),
                            NoOfBags: oq, OuterUOMId: netI(col(r, 'OuterQtyUomId')), OuterUOM: str(col(r, 'OuterUOM')), NetWeight: nw, EbUnit: pw, EbTotal: pw * oq, AddLess: 0,
                            GrossWeight: netD(col(r, 'GrossWeight')), StockWeight: netD(val('txtFactoryWeight')) / netD(val('txtDoNetWeight')) * (nw - pw * oq),
                            JobLotId: netI(col(r, 'JobLotId')), JobLot: str(col(r, 'JobLotName')), WarehouseId: netI(col(r, 'WarehouseId')), Warehouse: str(col(r, 'WareHouseName')),
                            /* WarehouseTo = the DO's own warehouse (desktop) - Save then asks for a different one per row. */
                            WarehouseToId: netI(col(r, 'WarehouseId')), WarehouseTo: str(col(r, 'WareHouseName')),
                            'Container#': '', 'Seal#': '', PackingExpiryDate: str(col(r, 'PackingExpiryDate')), ProductionNo: ''
                        });
                    }
                });
                S.rows = newRows; S.curDetail = -1;
                setText('txtInvoiceNetWeight', fmt(totalNet)); setText('txtDoGrossWeight', fmt(totalGross));
                detailRender();
                if (items.length) bind('cmbItem', items, 'Id', 'ItemName', [], items[0].Id);
                bind('CmbOtherItem', d.otherItemCombo, 'ItemId', 'ItemName');
                S.others = (d.otherItems || []).map(function (o) {
                    return { ItemId: o.ItemId, ItemName: o.ItemName, Qty: o.Qty, Rate: o.Rate, Amount: o.Amount, ContainerNo: val('txtconainer1'),
                        WarehouseFromId: 0, WarehouseFrom: '', WarehouseToId: 0, WarehouseTo: '', Remarks: o.Remarks };
                });
                otherRender();
            } else { S.rows = []; detailRender(); }
            /* ContractORDERNo */
            if (d.contractQueried && (d.contract || []).length) {
                var c = d.contract[0];
                bind('cmbCustomer', [{ Id: c.SupplierCustomerId, Name: c.Customer }], 'Id', 'Name', [], c.SupplierCustomerId);
                bind('cmbDestinationPort', [{ Id: c.DestinationPortId, PortName: c.DestinationPort }], 'Id', 'PortName', [], c.DestinationPortId); enable('cmbDestinationPort', false);
                bind('cmbLoadingPort', [{ Id: c.LoadingPortId, PortName: c.LoadingPort }], 'Id', 'PortName', [], c.LoadingPortId); enable('cmbLoadingPort', false);
            }
        }).catch(function (e) { box(e.message); });
    }

    // ------------------------------------------------------------------------------ detail calculations
    function uomEq() { return hasSel('cmbPackUOM') ? netD(selExtra('cmbPackUOM', 0)) : null; }
    /* WeightCalculation */
    function weightCalc() {
        var totalPck = 0;
        setText('txtGrossWeight', '0'); setText('txtStockWeight', '0');
        var fc = netD(val('txtFactoryWeight')), doTotal = netD(val('txtDoNetWeight')), doNet = netD(val('txtNetWeight'));
        var pw = netD(val('txtPackingWeight')), addLess = netD(val('txtAddLess')), bags = netD(val('txtNoOfBags'));
        var eq = uomEq(); if (eq === null) return;
        if (pw > 0 && eq > 0 && bags > 0) { totalPck = bags * pw; setText('txtPackingweightTotal', raw(totalPck)); } else setText('txtPackingweightTotal', '0');
        if (doNet > 0 && fc > 0 && doTotal > 0) {
            setText('txtGrossWeight', raw(doNet + totalPck + addLess));
            var stock = fc / doTotal * (doNet - (totalPck + addLess));
            setText('txtStockWeight', CFG.NetWeightAndStockWeightEqualOnForwarding ? val('txtNetWeight') : raw(Math.round(stock * 1000) / 1000));
        }
    }
    /* NetWeightCalculation */
    function netCalc() {
        var eq = uomEq();
        if (eq !== null) { var b = netD(val('txtNoOfBags')); setText('txtNetWeight', b > 0 && eq > 0 ? raw(b * eq) : '0'); }
        else setText('txtNetWeight', '0');
    }
    function bagsChanged() { netCalc(); weightCalc(); }               /* txtNoOfBags_TextChanged / cmbOuterUOM_ValueChanged */
    /* cmbItem_Leave: CommonServices.GetUomScheduleByItemId -> cmbPackUOM. */
    function itemLeave(keep) {
        return getJson(API + '/uoms?itemId=' + netI(val('cmbItem'))).then(function (rows) {
            S.uoms = rows || [];
            bind('cmbPackUOM', S.uoms, 'Id', 'UOMCode', ['Equivalent'], keep === undefined ? undefined : keep);
        }).catch(function (e) { box(e.message); });
    }

    // ------------------------------------------------------------------------------ detail grid
    var DETAIL_COLS = ['ItemName', 'CropYear', 'Packtype', 'NoOfBags', 'OuterUOM', 'NetWeight', 'EbUnit', 'EbTotal', 'AddLess', 'GrossWeight', 'StockWeight',
        'JobLot', 'Warehouse', 'WarehouseTo', 'Container#', 'Seal#', 'PackingExpiryDate', 'ProductionNo'];
    var CAP = { Packtype: 'Packing Type', OuterUOM: 'Pack Uom', EbUnit: 'Packing Weight', EbTotal: 'Total Packing Weight' };
    var NUM = { NoOfBags: 2, NetWeight: 3, EbUnit: 2, EbTotal: 2, AddLess: 2, GrossWeight: 2, StockWeight: 2 };
    function detailRender() {
        $id('detailHead').innerHTML = '<tr><th class="ctr">+</th><th class="ctr">X</th>' + DETAIL_COLS.map(function (c) { return '<th' + (NUM[c] ? ' class="num"' : '') + '>' + esc(CAP[c] || c) + '</th>'; }).join('') + '</tr>';
        var sums = {};
        $id('detailBody').innerHTML = S.rows.map(function (r, i) {
            var h = '<tr data-i="' + i + '"' + (i === S.curDetail ? ' class="is-current"' : '') + '><td class="win-cell-btn"><button type="button" class="win-edit" data-add="' + i + '">+</button></td>' +
                '<td class="win-cell-btn"><button type="button" class="win-x" data-del="' + i + '">X</button></td>';
            DETAIL_COLS.forEach(function (c) {
                var v = r[c];
                if (NUM[c]) { sums[c] = (sums[c] || 0) + netD(v); h += '<td class="num">' + esc(fmt(v, NUM[c])) + '</td>'; }
                else if (c === 'ItemName') h += '<td><a class="win-code" data-i="' + i + '" href="javascript:void(0)">' + esc(v) + '</a></td>';
                else h += '<td>' + esc(v) + '</td>';
            });
            return h + '</tr>';
        }).join('');
        $id('detailFoot').innerHTML = S.rows.length ? '<tr><td class="lbl">&Sigma;</td><td></td>' + DETAIL_COLS.map(function (c) { return '<td' + (NUM[c] ? '' : ' class="lbl"') + '>' + (NUM[c] ? esc(fmt(sums[c] || 0, 2)) : '') + '</td>'; }).join('') + '</tr>' : '';
    }
    /* grdDetail_DoubleClick */
    function detailEdit(i) {
        var it = S.rows[i]; if (!it) return;
        S.updateIndex = i;
        bind('cmbItem', [{ Id: it.ItemId, ItemName: it.ItemName }], 'Id', 'ItemName', [], it.ItemId);
        itemLeave(it.OuterUOMId).then(function () {
            setVal('CmbCropYear', it.CropYearId); setVal('cmbPackType', it.PackTypeId);
            setText('txtNoOfBags', fmt(it.NoOfBags, 2));
            setText('txtNetWeight', rawText(it.NetWeight));
            setText('txtPackingWeight', fmt(it.EbUnit, 2)); setText('txtPackingweightTotal', fmt(it.EbTotal, 2));
            setText('txtAddLess', rawText(it.AddLess)); setText('txtGrossWeight', rawText(it.GrossWeight)); setText('txtStockWeight', rawText(it.StockWeight));
            setVal('CmbJobLot', it.JobLotId); setVal('CmbWareHouseFrom', it.WarehouseId); setVal('cmbWareHouseTo', it.WarehouseToId);
            var cs = $id('cmbcontainerNo');
            if (it['Container#'] !== '' && !Array.prototype.some.call(cs.options, function (o) { return o.value === it['Container#']; })) {
                var o = document.createElement('option'); o.value = it['Container#']; o.textContent = it['Container#']; cs.appendChild(o);
            }
            setVal('cmbcontainerNo', it['Container#']);
            setText('txtSeal', it['Seal#']); setText('txtPackingExpiryDate', it.PackingExpiryDate); setText('txtProductionNo', it.ProductionNo);
            show('btnDetailUpdate', true); show('btnDetailCancel', true); show('btnAddinGrid', false);
            focus('cmbItem');
            weightCalc(); netCalc();
        });
    }
    /* formdetailvalidation */
    function detailValidation() {
        if (!hasSel('cmbItem')) { box('Please select Item'); focus('cmbItem'); return false; }
        if (!hasSel('CmbCropYear')) { box('Please Select CropYear'); focus('CmbCropYear'); return false; }
        if (!hasSel('cmbPackType')) { box('Please select Packing Type'); focus('cmbPackType'); return false; }
        if (val('txtNoOfBags').trim() === '' || !(netD(val('txtNoOfBags')) > 0)) { box('Please Insert No Of Bags'); focus('txtNoOfBags'); return false; }
        if (!hasSel('cmbPackUOM')) { box('Please Select Pack UOM'); focus('cmbPackUOM'); return false; }
        if (val('txtNetWeight').trim() === '' || netD(val('txtNetWeight')) === 0) { box('Please Insert NetWeight'); focus('txtNetWeight'); return false; }
        if (val('txtGrossWeight').trim() === '' || !(netD(val('txtGrossWeight')) > 0)) { box('Please Insert Gross weight'); focus('txtGrossWeight'); return false; }
        if (!hasSel('CmbJobLot')) { box('Please Select Job Lot'); focus('CmbJobLot'); return false; }
        if (!hasSel('CmbWareHouseFrom')) { box('Please Select Warehouse From'); focus('CmbWareHouseFrom'); return false; }
        if (!hasSel('cmbWareHouseTo')) { box('Please Select Warehouse To'); focus('cmbWareHouseTo'); return false; }
        if (netI(val('CmbWareHouseFrom')) === netI(val('cmbWareHouseTo'))) { box('WareHouse From And Warehouse To Can not be Same...'); focus('cmbWareHouseTo'); return false; }
        if (val('txtPackingExpiryDate') === '') { box('PackingExpiryDate Field Required'); focus('txtPackingExpiryDate'); return false; }
        if (val('txtProductionNo') === '') { box('Production No Field Required'); focus('txtProductionNo'); return false; }
        return true;
    }
    /* btnDetailUpdate_Click */
    function detailUpdate() {
        if (!detailValidation()) return;
        var r = S.rows[S.updateIndex]; if (!r) return;
        r.ItemId = netI(val('cmbItem')); r.ItemName = selText('cmbItem');
        r.PackTypeId = netI(val('cmbPackType')); r.Packtype = selText('cmbPackType');
        r.CropYearId = netI(val('CmbCropYear')); r.CropYear = selText('CmbCropYear');
        r.NoOfBags = netD(val('txtNoOfBags')); r.OuterUOMId = netI(val('cmbPackUOM')); r.OuterUOM = selText('cmbPackUOM');
        r.NetWeight = netD(val('txtNetWeight')); r.EbUnit = netD(val('txtPackingWeight')); r.EbTotal = netD(val('txtPackingweightTotal'));
        r.AddLess = netD(val('txtAddLess')); r.GrossWeight = netD(val('txtGrossWeight')); r.StockWeight = Math.round(netD(val('txtStockWeight')) * 100) / 100;
        r.WarehouseId = netI(val('CmbWareHouseFrom')); r.Warehouse = selText('CmbWareHouseFrom');
        r.WarehouseToId = netI(val('cmbWareHouseTo')); r.WarehouseTo = selText('cmbWareHouseTo');
        r.JobLotId = netI(val('CmbJobLot')); r.JobLot = selText('CmbJobLot');
        r['Container#'] = selText('cmbcontainerNo'); r['Seal#'] = val('txtSeal');
        r.PackingExpiryDate = val('txtPackingExpiryDate'); r.ProductionNo = val('txtProductionNo');
        detailRender(); detailReset(); focus('cmbItem');
    }
    /* FormDetailReset (cmbItem.ReadOnly = false) */
    function detailReset() {
        setVal('cmbItem', ''); setVal('cmbPackType', ''); setVal('cmbPackUOM', '');
        ['txtNetWeight', 'txtGrossWeight', 'txtPackingWeight', 'txtPackingweightTotal', 'txtAddLess', 'txtStockWeight', 'txtSeal', 'txtPackingExpiryDate', 'txtProductionNo'].forEach(function (i) { setText(i, ''); });
        setVal('CmbWareHouseFrom', ''); setVal('CmbJobLot', ''); setVal('cmbcontainerNo', '');
        show('btnDetailUpdate', false); show('btnDetailCancel', false);
        enable('cmbItem', true);
        S.updateIndex = -1;
    }
    function detailCancel() { detailReset(); }
    /* grdDetail_ColumnButtonClick "Delete": a saved row asks and is remembered (never sent by Insert - desktop). */
    function detailDelete(i) {
        var r = S.rows[i]; if (!r) return;
        if (netI(r.Id) > 0) { if (!ask('Are you sure to Delete?')) return; S.removed.push(JSON.parse(JSON.stringify(r))); }
        S.rows.splice(i, 1); detailRender();
    }
    function detailAdd(i) { var r = S.rows[i]; if (!r) return; var c = JSON.parse(JSON.stringify(r)); c.Id = 0; S.rows.push(c); detailRender(); }

    // ------------------------------------------------------------------------------ other items
    function otherRender() {
        var sq = 0, sa = 0;
        $id('otherBody').innerHTML = S.others.map(function (o, i) {
            sq += netD(o.Qty); sa += netD(o.Amount);
            return '<tr data-i="' + i + '"><td><a class="win-code" data-i="' + i + '" href="javascript:void(0)">' + esc(o.ItemName) + '</a></td><td class="num">' + esc(fmt(o.Qty, 0)) + '</td><td class="num">' + esc(fmt(o.Rate, 0)) +
                '</td><td class="num">' + esc(fmt(o.Amount, 0)) + '</td><td>' + esc(o.ContainerNo) + '</td><td>' + esc(o.WarehouseFrom) + '</td><td>' + esc(o.WarehouseTo) + '</td><td>' + esc(o.Remarks) + '</td></tr>';
        }).join('');
        $id('otherFoot').innerHTML = S.others.length ? '<tr><td class="lbl">&Sigma;</td><td>' + esc(fmt(sq, 2)) + '</td><td></td><td>' + esc(fmt(sa, 2)) + '</td><td></td><td></td><td></td><td></td></tr>' : '';
    }
    /* OtherItemsCalculations */
    function otherCalc() { var q = netD(val('txtoQty').trim()), r = netD(val('txtoRate').trim()); setText('txtoAmount', q > 0 && r > 0 ? fcy(q * r) : '0'); }
    /* grdotheritems_DoubleClick */
    function otherEdit(i) {
        var o = S.others[i]; if (!o) return;
        S.updateIndexOther = i;
        setVal('CmbOtherItem', o.ItemId); setVal('cmbWareHouseFromOtherItem', o.WarehouseFromId); setVal('cmbWareHouseToOtherItem', o.WarehouseToId);
        setText('txtoQty', fmt(o.Qty)); setText('txtoRate', fmt(o.Rate, 4)); setText('txtoAmount', fcy(o.Amount));
        setText('txtOContainer', o.ContainerNo); setText('txtoRemarks', o.Remarks);
        show('btnoadd', false); show('btnoUpdate', true); show('btnoCancel', true);
        focus('txtoQty');
    }
    /* OtherItemFormValidation (the WareHouse To check reads the detail panel's cmbWareHouseTo, and the "same" check
       compares with Conversion.ToInt of the combo control itself - 0 - as the desktop does). */
    function otherValidation() {
        if (!hasSel('CmbOtherItem')) { box('Item field required'); focus('CmbOtherItem'); return false; }
        if (netD(val('txtoQty').trim()) === 0) { box('Qty field required'); focus('txtoQty'); return false; }
        if (netD(val('txtoRate').trim()) === 0) { box('Rate field required'); focus('txtoRate'); return false; }
        if (netD(val('txtoAmount').trim()) === 0) { box('Amount field required'); focus('txtoAmount'); return false; }
        if (val('txtOContainer').trim() === '') { box('ContainerNo field required'); focus('txtOContainer'); return false; }
        if (!hasSel('cmbWareHouseFromOtherItem')) { box('WareHouseFrom field required'); focus('cmbWareHouseFromOtherItem'); return false; }
        if (!hasSel('cmbWareHouseTo')) { box('WareHouse To field required'); focus('cmbWareHouseTo'); return false; }
        if (netI(val('cmbWareHouseFromOtherItem')) === 0) { box('WareHouse From And Warehouse To Can not be Same...'); focus('cmbWareHouseToOtherItem'); return false; }
        return true;
    }
    /* btnoUpdate_Click */
    function otherUpdate() {
        if (!otherValidation()) return;
        var o = S.others[S.updateIndexOther]; if (!o) return;
        o.ItemId = netI(val('CmbOtherItem')); o.ItemName = selText('CmbOtherItem');
        o.Qty = netD(val('txtoQty')); o.Rate = netD(val('txtoRate')); o.Amount = netD(val('txtoAmount'));
        o.ContainerNo = val('txtOContainer');
        o.WarehouseToId = netI(val('cmbWareHouseToOtherItem')); o.WarehouseTo = selText('cmbWareHouseToOtherItem');
        o.WarehouseFromId = netI(val('cmbWareHouseFromOtherItem')); o.WarehouseFrom = selText('cmbWareHouseFromOtherItem');
        o.Remarks = val('txtoRemarks');
        otherRender();
        show('btnoadd', false); show('btnoUpdate', false); show('btnoCancel', false);
        otherReset(); focus('txtoQty');
    }
    function otherReset() { setVal('CmbOtherItem', ''); ['txtoQty', 'txtoRate', 'txtoAmount', 'txtOContainer', 'txtoRemarks'].forEach(function (i) { setText(i, ''); }); }
    function otherCancel() { show('btnoadd', false); show('btnoUpdate', false); show('btnoCancel', false); otherReset(); }

    // ------------------------------------------------------------------------------ gate pass grid
    var GP_COLS = ['GpDate', 'GpSrNo', 'Status', 'VehicleType', 'VehicleNo', 'BiltyNo', 'Container', 'Container1', 'SupplierWeight', 'FactoryWeight', 'DifferenceWeight'];
    var GP_NUM = { SupplierWeight: 1, FactoryWeight: 1, DifferenceWeight: 1 };
    function gpRender() {
        $id('gpHead').innerHTML = '<tr><th>Load</th>' + GP_COLS.map(function (c) { return '<th' + (GP_NUM[c] ? ' class="num"' : '') + '>' + c + '</th>'; }).join('') + '</tr>';
        var sums = {};
        $id('gpBody').innerHTML = S.gpGrid.map(function (r, i) {
            var h = '<tr data-i="' + i + '"><td class="win-cell-btn"><button type="button" class="win-edit" data-load="' + i + '">Load</button></td>';
            GP_COLS.forEach(function (c) {
                var v = col(r, c);
                if (GP_NUM[c]) { sums[c] = (sums[c] || 0) + netD(v); h += '<td class="num">' + esc(netD(v) === 0 ? '' : fmt(v, 2)) + '</td>'; }
                else if (c === 'GpDate') h += '<td>' + esc(shortDate(v)) + '</td>';
                else if (c === 'GpSrNo') h += '<td><a class="win-code" data-load="' + i + '" href="javascript:void(0)">' + esc(v) + '</a></td>';
                else h += '<td>' + esc(v) + '</td>';
            });
            return h + '</tr>';
        }).join('');
        $id('gpFoot').innerHTML = S.gpGrid.length ? '<tr><td class="lbl">&Sigma;</td>' + GP_COLS.map(function (c) { return '<td' + (GP_NUM[c] ? '' : ' class="lbl"') + '>' + (GP_NUM[c] ? esc(fmt(sums[c] || 0, 2)) : '') + '</td>'; }).join('') + '</tr>' : '';
    }
    /* grdgpInfo_ColumnButtonClick "Load": FormReset, cmbGpNo.Value = GP, cmbGpNo_Leave. */
    function gpLoad(i) { var r = S.gpGrid[i]; if (!r) return; var gp = netI(col(r, 'Id')); formReset().then(function () { setVal('cmbGpNo', gp); return gpLeave(); }); }

    // ------------------------------------------------------------------------------ reset / read
    /* FormReset */
    function formReset() {
        S.recId = 0; S.preStockWt = 0; S.removed = []; S.updateMode = false;
        enable('cmbGpNo', true); enable('cmbInvoiceNo', true);
        setVal('cmbtransporter', ''); setVal('cmbLoadingPort', ''); setVal('cmbDestinationPort', ''); bind('cmbInvoiceNo', [], 'Id', 'InvoiceNo');
        bind('cmbCustomer', [], 'Id', 'Name');
        setText('txtFactoryWeight', '0');
        ['txtBalFcWeight', 'txtNoofInvoices', 'txtVehicleNo', 'txtBilityNo', 'txtDriverName', 'txtDriverCellNo', 'txtCNICNO', 'txtOtherCharges', 'txtFreight', 'txtNoOfContainer',
         'txtconainer1', 'txtContainer2', 'txtDoGrossWeight', 'txtDoNetWeight', 'txtInvoiceNetWeight'].forEach(function (i) { setText(i, ''); });
        bind('cmbItem', [], 'Id', 'ItemName');
        S.rows = []; S.others = []; detailRender(); otherRender();
        show('btnSave', true); show('btnUpdate', false);
        focus('cmbGpNo');
        return getJson(API + '/reset').then(function (d) {
            d = d || {};
            errs(d, ['docNo', 'gatePasses', 'gatePassGrid']);
            setText('txtdocno', netI(d.docNo) > 0 ? d.docNo : val('txtdocno'));
            gatePassesBind(d.gatePasses); setVal('cmbGpNo', '');
            S.gpGrid = d.gatePassGrid || []; gpRender();
        }).catch(function (e) { box(e.message); });
    }
    function newForm() { return formReset().then(function () { detailReset(); }); }

    /* ReadById(Id) */
    function readById(id) {
        return getJson(API + '/by-id?id=' + id).then(function (d) {
            d = d || {};
            var h = d.header;
            if (!h) return formReset();
            S.recId = id; S.removed = [];
            show('btnSave', false); show('btnUpdate', true);
            tab('main', 'tabForm'); tab('detail', 'tabPage3');
            setText('txtdocno', col(h, 'DocNo')); setText('DocDate', isoDate(col(h, 'DocDate')));
            bind('cmbInvoiceNo', [{ Id: col(h, 'ExImInvoiceId'), InvoiceNo: col(h, 'InvoiceNo') }], 'Id', 'InvoiceNo', [], col(h, 'ExImInvoiceId'));
            bind('cmbGpNo', [{ Id: col(h, 'GatePassOutwardId'), GpSrNo: col(h, 'GpSrNo') }], 'Id', 'GpSrNo', [], col(h, 'GatePassOutwardId'));
            bind('cmbCustomer', [{ Id: col(h, 'SupplierCustomerId'), Name: col(h, 'Customer') }], 'Id', 'Name', [], col(h, 'SupplierCustomerId'));
            bind('cmbDestinationPort', [{ Id: col(h, 'DestinationPortId'), PortName: col(h, 'DestinationPort') }], 'Id', 'PortName', [], col(h, 'DestinationPortId'));
            bind('cmbLoadingPort', [{ Id: col(h, 'LoadingPortId'), PortName: col(h, 'LoadingPort') }], 'Id', 'PortName', [], col(h, 'LoadingPortId'));
            setText('txtNoofInvoices', d.noOfInvoices);
            setVal('cmbtransporter', netI(col(h, 'TransporterId')));
            setText('txtDriverCellNo', rawText(col(h, 'DriverCellNo'))); setText('txtDriverName', rawText(col(h, 'DriverName'))); setText('txtCNICNO', rawText(col(h, 'DriverCnicNo')));
            setText('txtGPDate', isoDate(col(h, 'GPDate')));
            setText('txtVehicleNo', rawText(col(h, 'VehicleNo'))); setText('txtBilityNo', rawText(col(h, 'BiltyNo')));
            setText('txtconainer1', rawText(col(h, 'Container'))); setText('txtContainer2', rawText(col(h, 'Container1')));
            setText('txtFactoryWeight', fmt(col(h, 'OtherCharges'))); setText('txtFreight', fcy(col(h, 'FreightAmt')));
            setText('txtRemarksHeader', rawText(col(h, 'OtherRemarks'))); setText('txtNoOfContainer', rawText(col(h, 'NoOfContainer')));
            bind('CmbOtherItem', d.otherItemCombo, 'ItemId', 'ItemName');
            var invWt = 0; S.preStockWt = 0;
            S.rows = (d.details || []).map(function (x) {
                invWt += netD(col(x, 'NetWeight')); S.preStockWt += netD(col(x, 'StockWeight'));
                return {
                    Id: netI(col(x, 'Id')), RefDocumentTypeId: netI(col(x, 'RefDocumentTypeId')), RefDocIdNo: netI(col(x, 'RefDocIdNo')), RefDocSubIdNo: netI(col(x, 'RefDocSubIdNo')),
                    ProformaDetailId: netI(col(x, 'ContractDetailId')), ProformaId: netI(col(x, 'ExImLcOrderId')), ProformaNo: str(col(x, 'LcOrderNo')),
                    PreInvoiceId: netI(col(x, 'InvoiceId')), PreInvoiceDetailId: netI(col(x, 'InvoiceDetailId')), ItemId: netI(col(x, 'ItemId')), ItemName: str(col(x, 'ItemName')),
                    CropYearId: netI(col(x, 'CropYearId')), CropYear: str(col(x, 'CropYear')), PackTypeId: netI(col(x, 'PackingMaterialId')), Packtype: str(col(x, 'Packtype')),
                    NoOfBags: netD(col(x, 'OuterQty')), OuterUOMId: netI(col(x, 'UOMScheduleIdOuter')), OuterUOM: str(col(x, 'OuterUOM')), NetWeight: netD(col(x, 'NetWeight')),
                    EbUnit: netD(col(x, 'EbUnit')), EbTotal: netD(col(x, 'EbTotal')), AddLess: netD(col(x, 'AdLsWeight')), GrossWeight: netD(col(x, 'GrossWeight')), StockWeight: netD(col(x, 'StockWeight')),
                    JobLotId: netI(col(x, 'LotJobId')), JobLot: str(col(x, 'JobLotName')), WarehouseId: netI(col(x, 'WarehouseId')), Warehouse: str(col(x, 'WareHouseName')),
                    WarehouseToId: netI(col(x, 'WarehouseToId')), WarehouseTo: str(col(x, 'WareHouseToName')), 'Container#': str(col(x, 'ContainerNo')), 'Seal#': str(col(x, 'SealNo')),
                    PackingExpiryDate: str(col(x, 'PackingExpiryDate')), ProductionNo: str(col(x, 'ProductionNo'))
                };
            });
            setText('txtInvoiceNetWeight', fmt(invWt));
            applyGatePass(d.gatePass);
            S.curDetail = -1; detailRender();
            S.others = (d.otherItems || []).map(function (o) {
                return { ItemId: netI(col(o, 'otherItemId')), ItemName: str(col(o, 'ItemName')), Qty: netD(col(o, 'oItemQty')), Rate: netD(col(o, 'oItemRate')), Amount: netD(col(o, 'oItemAmount')),
                    ContainerNo: str(col(o, 'ContainerNo')), WarehouseFromId: netI(col(o, 'WareHouseFromId')), WarehouseFrom: str(col(o, 'WareHouseFrom')),
                    WarehouseToId: netI(col(o, 'WarehouseToId')), WarehouseTo: str(col(o, 'WareHouseTo')), Remarks: str(col(o, 'OtherItemRemarks')) };
            });
            otherRender();
            S.updateMode = true;
            enable('cmbGpNo', false); enable('cmbInvoiceNo', false);
        }).catch(function (e) { box(e.message); });
    }

    // ------------------------------------------------------------------------------ save / delete / print
    /* formValidation */
    function formValidation() {
        if (val('txtdocno') === '' || val('txtdocno') === '0') { box('Please Check Doc No'); focus('txtdocno'); return false; }
        if (selText('cmbGpNo') === '' || netI(val('cmbGpNo')) === 0) { box('Please Select GatePass No'); focus('cmbGpNo'); return false; }
        if (selText('cmbInvoiceNo') === '' || netI(val('cmbInvoiceNo')) === 0) { box('Please Select Pre Invoice No'); focus('cmbInvoiceNo'); return false; }
        if (selText('cmbCustomer') === '' || netI(val('cmbCustomer')) === 0) { box('Please Select Customer/Importer'); focus('cmbCustomer'); return false; }
        if (val('txtDriverName').trim() === '') { box('Please Enter DriverName'); focus('txtDriverName'); return false; }
        if (val('txtDriverCellNo').trim() === '') { box('Please Enter Driver Cell No'); focus('txtDriverCellNo'); return false; }
        if (val('txtCNICNO').trim() === '') { box('Please Enter Driver CNIC No'); focus('txtCNICNO'); return false; }
        if (val('txtVehicleNo').trim() === '') { box('Please Check Vehicle No'); focus('txtVehicleNo'); return false; }
        if (val('txtFactoryWeight').trim() === '' || netD(val('txtFactoryWeight')) === 0) { box('Please Check Factory Weight'); focus('txtFactoryWeight'); return false; }
        if (val('txtDoGrossWeight').trim() === '' || netD(val('txtDoGrossWeight')) === 0) { box('Please Check Do GrossWeight'); focus('txtDoGrossWeight'); return false; }
        if (val('txtDoNetWeight').trim() === '' || netD(val('txtDoNetWeight')) === 0) { box('Please Check Do NetWeight'); focus('txtDoNetWeight'); return false; }
        return true;
    }
    function payload(auto) {
        return {
            recId: S.recId, autoUpdate: !!auto, docNo: val('txtdocno'), docDate: val('DocDate'),
            invoiceId: netI(val('cmbInvoiceNo')), invoiceText: selText('cmbInvoiceNo'), gpId: netI(val('cmbGpNo')), gpNo: selText('cmbGpNo'), gpDate: val('txtGPDate'),
            remarks: val('txtRemarksHeader'), transporterId: netI(val('cmbtransporter')), vehicleNo: val('txtVehicleNo'), biltyNo: val('txtBilityNo'),
            driverName: val('txtDriverName'), driverCellNo: val('txtDriverCellNo'), cnicNo: val('txtCNICNO'), container1: val('txtconainer1'), container2: val('txtContainer2'),
            freight: val('txtFreight'), factoryWeight: val('txtFactoryWeight'), customerId: netI(val('cmbCustomer')), customerText: selText('cmbCustomer'),
            loadingPortId: netI(val('cmbLoadingPort')), destinationPortId: netI(val('cmbDestinationPort')), noOfContainer: val('txtNoOfContainer'),
            invoiceNetWeight: val('txtInvoiceNetWeight'), balFcWeight: val('txtBalFcWeight'), doGrossWeight: val('txtDoGrossWeight'), doNetWeight: val('txtDoNetWeight'),
            noOfInvoices: val('txtNoofInvoices'), rows: S.rows, others: S.others
        };
    }
    /* Insert() */
    function insert(btn) {
        return busy(btn, function () {
            if (S.rows.length === 0) { box('Please Check Detail Grid'); return Promise.resolve(); }
            if (!formValidation()) return Promise.resolve();
            if (!ask(S.recId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return Promise.resolve();
            return postJson(API + '/save', payload(false)).then(function (d) {
                d = d || {};
                box(d.message);
                if (d.openWages) global.open('/production/wages-bill?refDocTypeId=' + DOC_TYPE + '&refDocId=' + netI(d.id) + '&grossWeightTotal=' + netD(d.grossWeight), '_blank');
                var preview = $id('ChkPrintPreview').checked;
                return formReset().then(function () { if (preview) print507(netI(d.id)); });
            }).catch(function (e) { box(e.message); });
        });
    }
    function save(btn) { S.recId = 0; return insert(btn); }
    function update(btn) { return insert(btn); }
    /* btnDelete_Click */
    function remove(btn) {
        return busy(btn, function () {
            if (S.recId <= 0) { box('Record Not Found'); return Promise.resolve(); }
            if (!ask('Are you sure to Delete?')) return Promise.resolve();
            return postJson(API + '/delete', { recId: S.recId }).then(function (d) { box((d && d.message) || 'Delete Record Successfully'); return formReset(); }).catch(function (e) { box(e.message); });
        });
    }
    /* BtnPrint_Click: CommonServices.ForwardingSlip507(RecId) - 0 -> "No Record Found For Display". */
    function print(btn) { if (S.recId === 0) { box('No Record Found For Display'); return; } return print507(S.recId, btn); }
    function attachment() { box('Attachments (DMS) are not available in the web version of this screen.'); }
    /* btnRecordsUpdate_Click */
    function autoUpdate(btn) {
        return busy(btn, function () {
            return getJson(API + '/auto-update-ids').then(function (ids) {
                var ok = false, chain = Promise.resolve(true);
                (ids || []).forEach(function (id) {
                    chain = chain.then(function (go) {
                        if (!go) return false;
                        return readById(id).then(function () {
                            if (S.rows.length === 0) { box('Please Check Detail Grid'); return false; }
                            if (!formValidation()) return false;
                            if (!ask('Are you sure to Update?')) return false;
                            return postJson(API + '/save', payload(true)).then(function () { ok = true; return true; }, function (e) { box(e.message); ok = false; return false; });
                        });
                    });
                });
                return chain.then(function (last) { if (last && ok) box('Records Update Successfully'); show('btnRecordsUpdate', false); });
            }).catch(function (e) { show('btnRecordsUpdate', false); box(e.message); });
        });
    }

    // ------------------------------------------------------------------------------ history
    var HIST_COLS = ['DocNo', 'DocDate', 'GPNo', 'GPDate', 'PreInvoice#', 'Customer/Importer', 'TransporterName', 'DriverName', 'DriverCellNo', 'DriverCNIC', 'FreightAmount',
        'VehicleNo', 'BiltyNo', 'Remarks', 'EntryUser', 'EntryDate', 'ModifyUser', 'ModifyDate', 'ApprovedStatus', 'ApprovedUser', 'ApprovedDate', 'NoOfAttachments'];
    function ddMMMyyyy(v) { var s = isoDate(v); return s ? s.substring(8, 10) + '-' + MON[netI(s.substring(5, 7)) - 1] + '-' + s.substring(0, 4) : ''; }
    function histRender() {
        $id('histHead').innerHTML = '<tr>' + HIST_COLS.map(function (c) { return '<th' + (c === 'FreightAmount' ? ' class="num"' : '') + '>' + esc(c) + '</th>'; }).join('') + '<th>Edit</th><th>Print</th></tr>';
        var sum = 0;
        $id('histBody').innerHTML = S.hist.map(function (r, i) {
            var h = '<tr data-i="' + i + '"' + (i === S.curHist ? ' class="is-current"' : '') + '>';
            HIST_COLS.forEach(function (c) {
                var v = r[c];
                if (c === 'FreightAmount') { sum += netD(v); h += '<td class="num">' + esc(netD(v) === 0 ? '' : fmt(v, 2)) + '</td>'; }
                else if (c === 'DocDate' || c === 'GPDate') h += '<td>' + esc(ddMMMyyyy(v)) + '</td>';
                else if (/(Entry|Modify|Approved)Date/.test(c)) h += '<td>' + esc(shortDate(v)) + '</td>';
                else if (c === 'DocNo') h += '<td><a class="win-code" data-open="' + i + '" href="javascript:void(0)">' + esc(v) + '</a></td>';
                else if (c === 'NoOfAttachments') h += '<td><a class="win-code" data-att="' + i + '" href="javascript:void(0)">' + esc(v) + '</a></td>';
                else h += '<td>' + esc(v) + '</td>';
            });
            return h + '<td class="win-cell-btn"><button type="button" class="win-edit" data-edit="' + i + '">Edit</button></td><td class="win-cell-btn"><button type="button" class="win-edit" data-print="' + i + '">Print</button></td></tr>';
        }).join('');
        $id('histFoot').innerHTML = S.hist.length ? '<tr>' + HIST_COLS.map(function (c, k) { return '<td' + (c === 'FreightAmount' ? '' : ' class="lbl"') + '>' + (c === 'FreightAmount' ? esc(fmt(sum, 2)) : (k === 0 ? '&Sigma;' : '')) + '</td>'; }).join('') + '<td></td><td></td></tr>' : '';
        show('histEmpty', S.hist.length === 0);
    }
    function historyShow(btn) {
        return busy(btn, function () {
            var mode = (document.querySelector('input[name="fnDateMode"]:checked') || {}).value || 'doc';
            return postJson(API + '/history', {
                fromChecked: $id('FromDateHistoryChk').checked, fromDate: val('FromDateHistory'), toChecked: $id('ToDateHistoryChk').checked, toDate: val('ToDateHistory'),
                dateMode: mode, fromDocNo: netI(val('FromDocNoHistory')), toDocNo: netI(val('ToDocNoHistory')), customerId: netI(val('CmbCustomerHistory'))
            }).then(function (rows) { S.hist = rows || []; S.curHist = -1; histRender(); clearHistDetail(); }).catch(function (e) { box(e.message); });
        });
    }
    function clearHistDetail() { $id('histDetHead').innerHTML = ''; $id('histDetBody').innerHTML = ''; $id('histDetFoot').innerHTML = ''; }
    function historyReset() {
        setText('FromDateHistory', daysAgo(3)); setText('ToDateHistory', today());
        setText('FromDocNoHistory', ''); setText('ToDocNoHistory', ''); setVal('CmbCustomerHistory', '');
        S.hist = []; histRender(); show('histEmpty', false); clearHistDetail();
    }
    function historyRefresh(btn) { return busy(btn, function () { return getJson(API + '/history-combos').then(function (r) { bind('CmbCustomerHistory', r, 'Id', 'Name'); }).catch(function (e) { box(e.message); }); }); }
    /* DataGridHistory_DoubleClick / Edit: FormReset then ReadById (no update right check on this form). */
    function histOpen(i) { var r = S.hist[i]; if (!r) return; formReset().then(function () { S.recId = netI(r.Id); return readById(S.recId); }); }
    function histPrint(i) { var r = S.hist[i]; if (!r) return; print507(netI(r.Id)); }
    var HD_COLS = ['ItemName', 'CropYear', 'PackingType', 'NoOfBags', 'PackUOM', 'NetWeight', 'PackingWeight', 'TotalPackingWeight', 'AddLess', 'GrossWeight', 'StockWeight', 'JobLot',
        'Warehouse', 'WarehouseTo', 'Container#', 'Seal#', 'PackingExpiryDate', 'ProductionNo'];
    var HD_NUM = { NoOfBags: 2, NetWeight: 3, PackingWeight: 2, TotalPackingWeight: 2, AddLess: 2, GrossWeight: 2, StockWeight: 2 };
    /* DataGridHistory_SelectionChanged -> GetDetailGrdByHeadId */
    function histSelect(i) {
        var r = S.hist[i]; if (!r) return; S.curHist = i;
        getJson(API + '/history-detail?id=' + netI(r.Id)).then(function (rows) {
            rows = (rows || []).map(function (x) {
                return { ItemName: col(x, 'ItemName'), CropYear: col(x, 'CropYear'), PackingType: col(x, 'Packtype'), NoOfBags: col(x, 'OuterQty'), PackUOM: col(x, 'OuterUOM'),
                    NetWeight: col(x, 'NetWeight'), PackingWeight: col(x, 'EbUnit'), TotalPackingWeight: col(x, 'EbTotal'), AddLess: col(x, 'AdLsWeight'), GrossWeight: col(x, 'GrossWeight'),
                    StockWeight: col(x, 'StockWeight'), JobLot: col(x, 'JobLotName'), Warehouse: col(x, 'WareHouseName'), WarehouseTo: col(x, 'WareHouseToName'),
                    'Container#': col(x, 'ContainerNo'), 'Seal#': col(x, 'SealNo'), PackingExpiryDate: col(x, 'PackingExpiryDate'), ProductionNo: col(x, 'ProductionNo') };
            });
            if (!rows.length) { clearHistDetail(); return; }
            $id('histDetHead').innerHTML = '<tr>' + HD_COLS.map(function (c) { return '<th' + (HD_NUM[c] ? ' class="num"' : '') + '>' + esc(c) + '</th>'; }).join('') + '</tr>';
            var sums = {};
            $id('histDetBody').innerHTML = rows.map(function (x) { return '<tr>' + HD_COLS.map(function (c) { if (HD_NUM[c]) { sums[c] = (sums[c] || 0) + netD(x[c]); return '<td class="num">' + esc(fmt(x[c], HD_NUM[c])) + '</td>'; } return '<td>' + esc(x[c]) + '</td>'; }).join('') + '</tr>'; }).join('');
            $id('histDetFoot').innerHTML = '<tr>' + HD_COLS.map(function (c, k) { return '<td' + (HD_NUM[c] ? '' : ' class="lbl"') + '>' + (HD_NUM[c] ? esc(fmt(sums[c] || 0, 2)) : (k === 0 ? '&Sigma;' : '')) + '</td>'; }).join('') + '</tr>';
        }).catch(function (e) { box(e.message); });
    }

    // ------------------------------------------------------------------------------ wiring
    function on(id, ev, fn) { var e = $id(id); if (e) e.addEventListener(ev, fn); }
    function onLeave(id, fn) { var s = $id(id); if (!s) return; var last = s.value; s.addEventListener('change', function () { if (s.value !== last) { last = s.value; fn(); } }); }
    document.addEventListener('DOMContentLoaded', function () {
        document.querySelectorAll('.win-tabs .win-tab').forEach(function (b) { b.addEventListener('click', function () { tab(b.closest('.win-tabs').getAttribute('data-tabs'), b.getAttribute('data-tab')); }); });
        document.querySelectorAll('button[data-fullscreen]').forEach(function (b) { b.addEventListener('click', function () { var x = $id(b.getAttribute('data-fullscreen')); if (x) x.classList.toggle('ex-fullscreen'); }); });
        onLeave('cmbGpNo', gpLeave);
        onLeave('cmbInvoiceNo', invoiceLeave);
        on('cmbItem', 'change', function () { itemLeave(); });
        on('cmbPackUOM', 'change', bagsChanged);
        on('txtNoOfBags', 'input', bagsChanged);
        on('txtPackingWeight', 'input', weightCalc);
        on('txtAddLess', 'input', weightCalc);
        on('txtoQty', 'input', otherCalc);
        on('txtoRate', 'input', otherCalc);
        $id('detailBody').addEventListener('click', function (e) {
            var b = e.target.closest('[data-del]'); if (b) { detailDelete(+b.getAttribute('data-del')); return; }
            b = e.target.closest('[data-add]'); if (b) { detailAdd(+b.getAttribute('data-add')); return; }
            b = e.target.closest('a.win-code[data-i]'); if (b) { detailEdit(+b.getAttribute('data-i')); return; }
            var tr = e.target.closest('tr[data-i]'); if (tr) { S.curDetail = +tr.getAttribute('data-i'); $id('detailBody').querySelectorAll('tr').forEach(function (x) { x.classList.toggle('is-current', x === tr); }); }
        });
        $id('detailBody').addEventListener('dblclick', function (e) { var tr = e.target.closest('tr[data-i]'); if (tr && !e.target.closest('button')) detailEdit(+tr.getAttribute('data-i')); });
        $id('otherBody').addEventListener('click', function (e) { var b = e.target.closest('a.win-code[data-i]'); if (b) otherEdit(+b.getAttribute('data-i')); });
        $id('otherBody').addEventListener('dblclick', function (e) { var tr = e.target.closest('tr[data-i]'); if (tr) otherEdit(+tr.getAttribute('data-i')); });
        $id('gpBody').addEventListener('click', function (e) { var b = e.target.closest('[data-load]'); if (b) gpLoad(+b.getAttribute('data-load')); });
        $id('histBody').addEventListener('click', function (e) {
            var b = e.target.closest('[data-edit]'); if (b) { histOpen(+b.getAttribute('data-edit')); return; }
            b = e.target.closest('[data-print]'); if (b) { histPrint(+b.getAttribute('data-print')); return; }
            b = e.target.closest('[data-open]'); if (b) { histOpen(+b.getAttribute('data-open')); return; }
            b = e.target.closest('[data-att]'); if (b) { box('Attachments (DMS) are not available in the web version of this screen.'); return; }
            var tr = e.target.closest('tr[data-i]'); if (tr) { $id('histBody').querySelectorAll('tr').forEach(function (x) { x.classList.toggle('is-current', x === tr); }); histSelect(+tr.getAttribute('data-i')); }
        });
        $id('histBody').addEventListener('dblclick', function (e) { var tr = e.target.closest('tr[data-i]'); if (tr && !e.target.closest('button')) histOpen(+tr.getAttribute('data-i')); });
        /* ExpfrmForwarding_KeyDown */
        document.addEventListener('keydown', function (e) {
            var k = (e.key || '').toLowerCase();
            show('btnRecordsUpdate', false);
            if (e.key === 'Enter' && !e.ctrlKey && e.target && /^(INPUT|SELECT)$/.test(e.target.tagName) && e.target.type !== 'checkbox') {
                var f = Array.prototype.filter.call(document.querySelectorAll('input:not([type=hidden]):not(:disabled), select:not(:disabled), button:not(:disabled), textarea'), function (x) { return x.offsetParent !== null; });
                var i = f.indexOf(e.target); if (i >= 0 && i + 1 < f.length) { e.preventDefault(); f[i + 1].focus(); } return;
            }
            if (e.ctrlKey && k === 's' && activeTab('main') === 'tabForm' && !S.updateMode) { e.preventDefault(); save($id('btnSave')); }
            if (e.ctrlKey && e.shiftKey && k === 'a') { e.preventDefault(); show('btnRecordsUpdate', true); }
            if (e.ctrlKey && k === 'n') { e.preventDefault(); newForm(); }
            if (e.ctrlKey && k === 't') { e.preventDefault(); toggleHistory(); }
            if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { e.preventDefault(); cancel(); }
            if (e.ctrlKey && k === 'u' && activeTab('main') === 'tabForm' && S.updateMode) { e.preventDefault(); update($id('btnUpdate')); }
        });
        load();
    });

    global.ExportFn = {
        newForm: newForm, refresh: refresh, save: save, update: update, remove: remove, print: print, attachment: attachment, autoUpdate: autoUpdate,
        detailUpdate: detailUpdate, detailCancel: detailCancel, otherUpdate: otherUpdate, otherCancel: otherCancel,
        historyReset: historyReset, historyRefresh: historyRefresh, historyShow: historyShow, toggleHistory: toggleHistory
    };
}(window));
