/* ============================================================================================
 * countx_export_forwarding.js - two desktop forms of Architecture.WinApp.Export that share the detail grid,
 * the weight calculations and ExImForwarding.Save:
 *   195 EximForwardingDirect             (body data-variant="direct")  /api/export/forwarding-direct
 *   196 EximForwardingWithoutWeighBridge (body data-variant="wb")      /api/export/forwarding-without-weighbridge
 * Every desktop event has its counterpart with the desktop's messages and order (Load, Leave events, TextChanged
 * weight calculations, + / X grid buttons, double-click edits, Save / Update with the confirm between FormValidation
 * and the row checks, prints, History Edit / Print / LoadAll, 195 Reference grid Load and Register, shortcuts).
 * Button contract: disabled + spinner while a request runs, duplicates ignored, re-enabled on success / failure.
 * ============================================================================================ */
(function (global) {
    'use strict';

    var DIRECT = (document.body && document.body.getAttribute('data-variant')) === 'direct';
    var API = DIRECT ? '/api/export/forwarding-direct' : '/api/export/forwarding-without-weighbridge';
    var PRINT_KEY = DIRECT ? '174-eximforwardingdirect-slipandregister' : '507-eximforwarding-slip';

    function $id(id) { return document.getElementById(id); }
    function box(m) { global.alert(m); }
    function ask(m) { return global.confirm(m); }
    function esc(s) { return String(s === null || s === undefined ? '' : s).replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;'); }
    function str(v) { return v === null || v === undefined ? '' : String(v); }
    function netD(v) { var n = parseFloat(String(v === null || v === undefined ? '' : v).replace(/,/g, '')); return isFinite(n) ? n : 0; }
    function netI(v) { var n = parseInt(String(v === null || v === undefined ? '' : v), 10); return isFinite(n) ? n : 0; }
    function pad(n) { return (n < 10 ? '0' : '') + n; }
    function today() { var d = new Date(); return d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()); }
    /* custom FormatString "#,##0.##" / "#,##0.###" */
    function fmtN(v, dec) {
        var n = netD(v), f = Math.pow(10, dec);
        var r = Math.round(Math.abs(n) * f) / f;
        var p = String(r).split('.'); p[0] = p[0].replace(/\B(?=(\d{3})+(?!\d))/g, ',');
        return (n < 0 && r !== 0 ? '-' : '') + p.join('.');
    }
    function fmt00(v) { var n = Math.round(netD(v)); var s = String(Math.abs(n)).replace(/\B(?=(\d{3})+(?!\d))/g, ','); if (Math.abs(n) < 10) s = '0' + s; return (n < 0 ? '-' : '') + s; }
    function round(v, d) { var f = Math.pow(10, d); return Math.round(v * f) / f; }
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
    function first(id) { var s = $id(id); if (s && s.options.length > 1) { s.selectedIndex = 1; refreshCombos(); } }
    function setVal(id, v) { var s = $id(id); if (!s) return; s.value = str(v); if (s.value !== str(v)) s.value = '0'; refreshCombos(); }
    function val(id) { var e = $id(id); return e ? e.value : ''; }
    function setText(id, v) { var e = $id(id); if (e) e.value = str(v); }
    function comboText(id) { var s = $id(id); if (!s || s.selectedIndex < 0) return ''; var o = s.options[s.selectedIndex]; return o && o.value !== '0' ? o.text : ''; }
    function setComboText(id, text) {
        var s = $id(id); if (!s) return; s.value = '0';
        for (var i = 0; i < s.options.length; i++) if (s.options[i].value !== '0' && s.options[i].text === str(text)) { s.selectedIndex = i; break; }
        refreshCombos();
    }
    function setEnabled(id, on) { var e = $id(id); if (e) { e.disabled = !on; refreshCombos(); } }
    function focus(id) { var e = $id(id); if (!e) return; var w = e.closest && e.closest('.dtcombo-wrap'); var t = w ? w.querySelector('.dtcombo-input') : e; try { (t || e).focus(); } catch (x) { /* ignore */ } }
    function cancel() { global.close(); setTimeout(function () { if (!global.closed) { if (history.length > 1) history.back(); else location.href = '/export'; } }, 150); }
    function errs(d, keys) { keys.forEach(function (k) { if (d && d[k + 'Error']) box(d[k + 'Error']); }); }

    // ------------------------------------------------------------------------------ state
    var PERM = { Save: true, Update: true, Print: true };
    var S = { recId: 0, updateMode: false, rows: [], others: [], detIdx: -1, othIdx: 0, lotcompletedate: '', preStockWt: 0 };
    var HIST = [], REF = [], REG = [];

    // ------------------------------------------------------------------------------ tabs
    var PANELS = DIRECT ? ['fwForm', 'fwHistory', 'fwRegister'] : ['fwForm', 'fwHistory'];
    function tab(panelId) {
        document.querySelectorAll('.win-tabs .win-tab[data-tab]').forEach(function (b) { b.classList.toggle('is-active', b.getAttribute('data-tab') === panelId); });
        PANELS.forEach(function (p) { $id(p).classList.toggle('is-active', p === panelId); });
        $id('btnFooterHistory').querySelector('span').textContent = panelId === 'fwHistory' ? 'Form' : 'History';
        /* tabControl1_SelectedIndexChanged: History -> GridBind(50 | 500) */
        if (panelId === 'fwHistory') gridBind(DIRECT ? 500 : 50);
    }
    function current() { for (var i = 0; i < PANELS.length; i++) if ($id(PANELS[i]).classList.contains('is-active')) return PANELS[i]; return 'fwForm'; }
    function toggleHistory() { tab(current() === 'fwHistory' ? 'fwForm' : 'fwHistory'); }
    function tab2(id) {
        document.querySelectorAll('.win-tab[data-tab2]').forEach(function (b) { b.classList.toggle('is-active', b.getAttribute('data-tab2') === id); });
        show('fwDetail', id === 'fwDetail'); show('fwOther', id === 'fwOther');
    }

    // ------------------------------------------------------------------------------ load / combos
    function bindCombos(d) {
        errs(d, ['docNo', 'packTypes', 'jobLots', 'projects', 'branches', 'warehouses', 'cropYears', 'ports', 'shippingParties', 'carrierTypes', 'customers', 'items', 'invoices']);
        if (d.docNo && netI(d.docNo) > 0) setText('txtdocno', String(netI(d.docNo)));
        if (d.packTypes && d.packTypes.length) bind('cmbPackType', d.packTypes);
        if (d.jobLots && d.jobLots.length) bind('CmbJobLot', d.jobLots);
        if (d.projects && d.projects.length) { bind('comproject', d.projects); first('comproject'); }
        if (d.branches && d.branches.length) { bind('combranches', d.branches); first('combranches'); }
        if (d.warehouses && d.warehouses.length) bind('CmbWareHouse', d.warehouses);
        if (d.cropYears && d.cropYears.length) bind('CmbCropYear', d.cropYears);
        if (d.ports && d.ports.length) {
            bind('cmbLoadingPort', d.ports); bind('cmbDestinationPort', d.ports);
            if (DIRECT) { bind('cmbPortDestinationRegister', d.ports); bind('cmbPortLoadingRegister', d.ports); }
        }
        if (d.shippingParties && d.shippingParties.length) {
            bind('cmbShippingAgent', d.shippingParties); bind('cmbShippingLIne', d.shippingParties); bind('cmbtransporter', d.shippingParties);
            if (DIRECT) { bind('cmbShippingLineRegister', d.shippingParties); bind('cmbShippingAgentRegister', d.shippingParties); bind('cmbTranspoterRegister', d.shippingParties); }
        }
        if (DIRECT) {
            if (d.carrierTypes) { bind('cmbcareiertype', d.carrierTypes); bind('cmbCarrierTypeRegister', d.carrierTypes); }
            if (d.customers && d.customers.length) { bind('cmbCustomer', d.customers); bind('cmbCustomerRegister', d.customers); }
            if (d.items && d.items.length) { bind('cmbItem', d.items); bind('cmbItemNameRegister', d.items); }
        } else if (d.invoices) {
            if (d.invoices.length) bind('cmbInvoiceNo', d.invoices); else bind('cmbInvoiceNo', []);
        }
    }
    function load() {
        return getJson(API + '/setup').then(function (d) {
            d = d || {};
            if (d.permissions) PERM = d.permissions;
            $id('btnSave').disabled = !PERM.Save; $id('btnUpdate').disabled = !PERM.Update; $id('BtnPrint').disabled = !PERM.Print;
            ['DocDate', 'txtGPDate', 'txtCompletedDate'].forEach(function (id) { setText(id, today()); });
            bindCombos(d);
            if (DIRECT) {
                setText('txtDateFromRegister', d.registerFrom || today()); setText('txtDateToRegister', today());
                show('btnDetailCancel', false); show('btnDetailUpdate', false);
                focus('txtReferenceNo');
            } else {
                $id('chkdateisactive').checked = false; setEnabled('txtCompletedDate', false);
                bind('cmbItem', []);
                focus('cmbInvoiceNo');
            }
            render();
            $id('fwFooterInfo').textContent = DIRECT ? 'EximForwardingDirect  -  Document Type 206' : 'EximForwardingWithoutWeighBridge  -  Document Type 205';
        }).catch(function (e) { box(e.message || 'Error occurred during database call.'); });
    }

    // ------------------------------------------------------------------------------ 196 invoice leave
    function applyBooking(b) {
        if (b) { setVal('cmbShippingAgent', b.ShippingAgentId); setVal('cmbShippingLIne', b.ShippingLineId); setVal('cmbtransporter', b.TransporterId); }
        else { first('cmbShippingAgent'); first('cmbShippingLIne'); first('cmbtransporter'); }
    }
    function bindFirstOrClear(id, rows) { bind(id, rows); if (rows.length) first(id); }
    /** cmbInvoiceNo_Leave. */
    function invoiceLeave() {
        return getJson(API + '/invoice?invoiceId=' + netI(val('cmbInvoiceNo'))).then(function (d) {
            d = d || {};
            if (d.bookingError) box(d.bookingError); else applyBooking(d.booking);
            if (d.contractError) box(d.contractError);
            else if (d.contract) {
                bindFirstOrClear('cmbContractNo', d.contract.contracts);
                bindFirstOrClear('cmbCustomer', d.contract.customers);
                bindFirstOrClear('cmbLoadingPort', d.contract.loadingPortCombo);
                bindFirstOrClear('cmbDestinationPort', d.contract.destinationPortCombo);
            }
            S.rows = d.rows || [];
            if (S.rows.length) {
                if (d.otherItemCombo && d.otherItemCombo.length) bind('CmbOtherItem', d.otherItemCombo);
                S.others = (d.otherItems || []).map(function (o) { o.ContainerNo = val('txtOContainer'); return o; });
            } else S.others = [];
            render();
            if (d.detailError) box(d.detailError);
        }).catch(function (e) { box(e.message); });
    }

    // ------------------------------------------------------------------------------ item / weights
    function itemLeave() {
        return getJson(API + '/uoms?itemId=' + netI(val('cmbItem'))).then(function (rows) {
            if (rows && rows.length) bind('cmbPackUOM', rows);
        }).catch(function (e) { box(e.message); });
    }
    function netWeightCalculation() {
        var bags = netD(val('txtNoOfBags')), uom = netD(comboText('cmbPackUOM'));
        if (bags > 0 && uom > 0) { var n = bags * uom; setText('txtNetWeight', String(n)); setText('txtStockWeight', String(n)); }
        else { setText('txtNetWeight', '0'); setText('txtStockWeight', '0'); }
    }
    function weightCalculation() {
        var net = netD(val('txtNetWeight')), eb = netD(val('txtEbUnit')), adl = netD(val('txtAddLess')), uom = netD(comboText('cmbPackUOM')), bags = netD(val('txtNoOfBags'));
        var tot = 0;
        if (eb > 0 && uom > 0 && bags > 0) { tot = bags * eb; setText('txtEbTotal', String(tot)); } else setText('txtEbTotal', '0');
        setText('txtGrossWeight', String(net + tot + adl));
        setText('txtStockWeight', String(round(net + adl, 3)));
    }
    function uomChanged() { netWeightCalculation(); weightCalculation(); }

    // ------------------------------------------------------------------------------ detail grid
    var COLS = DIRECT
        ? ['ItemName', 'Packtype', 'ItemDescription', 'NoOfBags', 'OuterUOM', 'NetWeight', 'EbUnit', 'EbTotal', 'AddLess', 'GrossWeight', 'StockWeight', 'Warehouse', 'JobLot', 'CropYear', 'Container#', 'Seal#']
        : ['ItemName', 'Packtype', 'ItemDescription', 'NoOfBags', 'OuterUOM', 'NetWeight', 'EbUnit', 'EbTotal', 'AddLess', 'GrossWeight', 'StockWeight', 'Warehouse', 'Container#', 'Seal#', 'JobLot', 'CropYear'];
    var NUM = { NoOfBags: 2, NetWeight: 3, EbUnit: 2, EbTotal: 2, AddLess: 2, GrossWeight: 2, StockWeight: 2 };
    function render() {
        var sums = {};
        $id('detBody').innerHTML = S.rows.map(function (r, i) {
            return '<tr data-i="' + i + '"' + (i === S.detIdx ? ' class="is-current"' : '') + '>' + COLS.map(function (c) {
                if (NUM[c] !== undefined) { sums[c] = (sums[c] || 0) + netD(r[c]); return '<td class="num">' + esc(fmtN(r[c], NUM[c])) + '</td>'; }
                return '<td>' + esc(str(r[c])) + '</td>';
            }).join('') + '<td class="win-cell-btn"><button type="button" class="win-edit" data-add="' + i + '">+</button></td>'
                + '<td class="win-cell-btn"><button type="button" class="win-x" data-del="' + i + '">X</button></td></tr>';
        }).join('');
        $id('detFoot').innerHTML = S.rows.length ? '<tr>' + COLS.map(function (c, k) {
            return NUM[c] !== undefined ? '<td>' + esc(fmtN(sums[c] || 0, 2)) + '</td>' : '<td class="lbl">' + (k === 0 ? '&Sigma;' : '') + '</td>';
        }).join('') + '<td class="lbl"></td><td class="lbl"></td></tr>' : '';
        if (!DIRECT) renderOthers();
    }
    function formDetailValidation() {
        if (netI(val('cmbItem')) === 0) { box('Please select Item'); focus('cmbItem'); return false; }
        if (netI(val('cmbPackType')) === 0) { box('Please select PackType'); focus('cmbPackType'); return false; }
        if (val('txtGrossWeight').trim() === '' || !(netD(val('txtGrossWeight')) > 0)) { box('Please Insert Gross weight'); focus('txtGrossWeight'); return false; }
        if (val('txtNetWeight').trim() === '' || netD(val('txtNetWeight')) === 0) { box('Please Insert NetWeight'); focus('txtNetWeight'); return false; }
        if (netI(val('cmbPackUOM')) === 0) { box('Please Select Pack UOM'); focus('cmbPackUOM'); return false; }
        if (val('txtNoOfBags').trim() === '' || !(netD(val('txtNoOfBags')) > 0)) { box('Please Insert No Of Bags'); focus('txtNoOfBags'); return false; }
        if (netI(val('CmbJobLot')) === 0) { box('Please Select Job Lot'); focus('CmbJobLot'); return false; }
        if (netI(val('CmbWareHouse')) === 0) { box('Please Select Warehouse'); focus('CmbWareHouse'); return false; }
        if (netI(val('CmbCropYear')) === 0) { box('Please Select CropYear'); focus('CmbCropYear'); return false; }
        if (val('txtContainerNo').trim() === '' || val('txtContainerNo') === '0') { box(DIRECT ? 'Please Insert  Container No' : 'Please Select  Container No'); focus('txtContainerNo'); return false; }
        if (val('txtSeal').trim() === '' || val('txtSeal') === '0') { box('Please Insert Seal No'); focus('txtSeal'); return false; }
        return true;
    }
    function rowFromForm() {
        return {
            ItemId: netI(val('cmbItem')), ItemName: comboText('cmbItem'), PackTypeId: netI(val('cmbPackType')), Packtype: comboText('cmbPackType'),
            ItemDescription: val('txtItemDesc'), NoOfBags: netD(val('txtNoOfBags').trim()), OuterUOMId: netI(val('cmbPackUOM')), OuterUOM: comboText('cmbPackUOM'),
            NetWeight: netD(val('txtNetWeight').trim()), EbUnit: netD(val('txtEbUnit').trim()), EbTotal: netD(val('txtEbTotal').trim()), AddLess: netD(val('txtAddLess').trim()),
            GrossWeight: netD(val('txtGrossWeight').trim()), StockWeight: netD(val('txtStockWeight').trim()),
            WarehouseId: netI(val('CmbWareHouse')), Warehouse: comboText('CmbWareHouse').trim(), 'Container#': val('txtContainerNo').trim(), 'Seal#': DIRECT ? val('txtSeal').trim() : val('txtSeal'),
            JobLotId: netI(val('CmbJobLot')), JobLot: comboText('CmbJobLot').trim(), CropYearId: netI(val('CmbCropYear')), CropYear: comboText('CmbCropYear').trim()
        };
    }
    function formDetailReset() {
        setVal('cmbItem', '0'); setVal('cmbPackType', '0'); setText('txtItemDesc', ''); setText('txtNetWeight', ''); setVal('cmbPackUOM', '0');
        if (DIRECT) { setText('txtNoOfBags', ''); setVal('CmbCropYear', '0'); }
        ['txtGrossWeight', 'txtEbUnit', 'txtEbTotal', 'txtAddLess', 'txtStockWeight', 'txtSeal', 'txtContainerNo'].forEach(function (id) { setText(id, ''); });
        setVal('CmbWareHouse', '0'); setVal('CmbJobLot', '0');
        show('btnDetailUpdate', false); show('btnDetailCancel', false); show('btnAddinGrid', true);
    }
    function btnAddinGrid() {
        if (!formDetailValidation()) return;
        S.rows.push(rowFromForm());
        formDetailReset(); render();
    }
    function detDblClick(i) {
        var r = S.rows[i]; if (!r) return;
        S.detIdx = i;
        if (!DIRECT) bind('cmbItem', [{ Id: r.ItemId, name: r.ItemName }]);
        setVal('cmbItem', r.ItemId);
        setVal('cmbPackType', r.PackTypeId);
        setText('txtItemDesc', r.ItemDescription); setText('txtGrossWeight', str(r.GrossWeight)); setText('txtNetWeight', str(r.NetWeight));
        setText('txtAddLess', str(r.AddLess)); setText('txtStockWeight', str(r.StockWeight));
        var s = $id('cmbPackUOM');
        if (s && !Array.prototype.some.call(s.options, function (o) { return o.value === str(r.OuterUOMId); })) {
            var o = document.createElement('option'); o.value = str(r.OuterUOMId); o.text = str(r.OuterUOM); s.appendChild(o);
        }
        setVal('cmbPackUOM', r.OuterUOMId);
        setText('txtNoOfBags', fmtN(r.NoOfBags, 2)); setText('txtEbUnit', fmtN(r.EbUnit, 2)); setText('txtEbTotal', fmtN(r.EbTotal, 2));
        setVal('CmbWareHouse', r.WarehouseId); setText('txtContainerNo', r['Container#']); setText('txtSeal', r['Seal#']);
        setVal('CmbJobLot', r.JobLotId); setVal('CmbCropYear', r.CropYearId);
        show('btnDetailUpdate', true); show('btnDetailCancel', true); show('btnAddinGrid', false);
        render(); focus('cmbItem');
    }
    function btnDetailUpdate() {
        if (!formDetailValidation()) return;
        var r = S.rows[S.detIdx]; if (!r) { box('There is no row at position ' + S.detIdx + '.'); return; }
        var n = rowFromForm();
        n.NetWeight = netD(val('txtNetWeight')); n.NoOfBags = netD(val('txtNoOfBags'));
        n.StockWeight = round(netD(val('txtStockWeight')), 2);
        n['Container#'] = val('txtContainerNo'); n['Seal#'] = val('txtSeal');
        n.Warehouse = comboText('CmbWareHouse'); n.JobLot = comboText('CmbJobLot'); n.CropYear = comboText('CmbCropYear');
        S.rows[S.detIdx] = n;
        show('btnDetailUpdate', false); show('btnDetailCancel', false); show('btnAddinGrid', true);
        formDetailReset(); render(); focus('cmbItem');
    }
    /** grdDetail_ColumnButtonClick: "Delete" removes the row; "Add" appends a copy of it. */
    function gridButton(kind, i) {
        var r = S.rows[i]; if (!r) return;
        if (kind === 'del') { S.rows.splice(i, 1); S.detIdx = -1; render(); return; }
        S.rows.push(Object.assign({}, r));
        formDetailReset(); render();
    }

    // ------------------------------------------------------------------------------ 196 other items
    function renderOthers() {
        var q = 0, a = 0;
        $id('othBody').innerHTML = S.others.map(function (o, i) {
            q += netD(o.Qty); a += netD(o.Amount);
            return '<tr data-i="' + i + '"><td>' + esc(o.ItemName) + '</td><td class="num">' + esc(fmt00(o.Qty)) + '</td><td class="num">' + esc(fmt00(o.Rate)) + '</td><td class="num">'
                + esc(fmt00(o.Amount)) + '</td><td>' + esc(str(o.ContainerNo)) + '</td><td>' + esc(str(o.Remarks)) + '</td></tr>';
        }).join('');
        $id('othFoot').innerHTML = S.others.length ? '<tr><td class="lbl">&Sigma;</td><td>' + esc(fmtN(q, 2)) + '</td><td class="lbl"></td><td>' + esc(fmtN(a, 2)) + '</td><td class="lbl"></td><td class="lbl"></td></tr>' : '';
    }
    function otherItemsCalculations() {
        var q = netD(val('txtoQty').trim()), r = netD(val('txtoRate').trim());
        setText('txtoAmount', q > 0 && r > 0 ? String(q * r) : '0');
    }
    function othDblClick(i) {
        var o = S.others[i]; if (!o) return;
        S.othIdx = i;
        setVal('CmbOtherItem', o.ItemId); setText('txtoQty', str(o.Qty)); setText('txtoRate', str(o.Rate)); setText('txtoAmount', str(o.Amount));
        setText('txtOContainer', str(o.ContainerNo)); setText('txtoRemarks', str(o.Remarks));
        show('btnoadd', false); show('btnoUpdate', true); show('btnoCancel', true);
        focus('txtoQty');
    }
    function resetOtherItems() { setVal('CmbOtherItem', '0'); ['txtoQty', 'txtoRate', 'txtoAmount', 'txtOContainer', 'txtoRemarks'].forEach(function (id) { setText(id, ''); }); }
    /** btnoUpdate_Click - updateDetailIndexOther starts at 0, so an Update without a double-click edits the first row (desktop). */
    function btnoUpdate() {
        if (netI(val('CmbOtherItem')) === 0) { box('Item field required'); focus('CmbOtherItem'); return; }
        if (netD(val('txtoQty').trim()) === 0) { box('Qty field required'); focus('txtoQty'); return; }
        if (netD(val('txtoRate').trim()) === 0) { box('Rate field required'); focus('txtoRate'); return; }
        if (netD(val('txtoAmount').trim()) === 0) { box('Amount field required'); focus('txtoAmount'); return; }
        if (val('txtOContainer').trim() === '') { box('ContainerNo field required'); focus('txtOContainer'); return; }
        var o = S.others[S.othIdx]; if (!o) { box('There is no row at position ' + S.othIdx + '.'); return; }
        o.ItemId = netI(val('CmbOtherItem')); o.ItemName = comboText('CmbOtherItem'); o.Qty = netD(val('txtoQty')); o.Rate = netD(val('txtoRate'));
        o.Amount = netD(val('txtoAmount')); o.ContainerNo = val('txtOContainer'); o.Remarks = val('txtoRemarks');
        renderOthers();
        show('btnoadd', true); show('btnoUpdate', false); show('btnoCancel', false);
        resetOtherItems(); focus('txtoQty');
    }
    function btnoCancel() { show('btnoadd', true); show('btnoUpdate', false); show('btnoCancel', false); resetOtherItems(); }

    // ------------------------------------------------------------------------------ reset / save / read
    function formReset() {
        S.recId = 0; S.preStockWt = 0;
        first('combranches'); first('comproject');
        ['cmbtransporter', 'cmbLoadingPort', 'cmbDestinationPort', 'cmbShippingLIne', 'cmbShippingAgent', 'cmbCustomer'].forEach(function (id) { setVal(id, '0'); });
        ['txtVehicleNo', 'txtBilityNo', 'txtDriverName', 'txtDriverCellNo', 'txtCNICNO', 'txtFreight', 'txtNoOfContainer'].forEach(function (id) { setText(id, ''); });
        if (DIRECT) setText('txtGpNo', '');
        else {
            setVal('cmbContractNo', '0'); setVal('cmbInvoiceNo', '0'); setText('txtCompletedDate', today());
            bind('cmbItem', []);
            setEnabled('cmbContractNo', true); setEnabled('cmbInvoiceNo', true);
        }
        setVal('cmbItem', '0');
        S.rows = []; S.others = []; S.detIdx = -1; render();
        show('btnSave', true); S.updateMode = false; show('btnUpdate', false);
        return getJson(API + '/refresh').then(function (d) {
            d = d || {};
            if (d.docNo && netI(d.docNo) > 0) setText('txtdocno', String(netI(d.docNo)));
            if (DIRECT && d.carrierTypes) bind('cmbcareiertype', d.carrierTypes);
        }).catch(function (e) { box(e.message); }).then(function () { if (DIRECT) focus('txtReferenceNo'); });
    }
    function btnNew() {
        return formReset().then(function () {
            formDetailReset();
            if (DIRECT) { focus('cmbCustomer'); return getJson(API + '/refresh').then(function (d) { bindCombos(d || {}); }).catch(function (e) { box(e.message); }); }
        });
    }
    function btnRefresh(btn) {
        return busy(btn, function () {
            return getJson(API + '/refresh').then(function (d) {
                d = d || {};
                var keepInvoice = val('cmbInvoiceNo');
                if (!DIRECT) delete d.docNo;
                bindCombos(d);
                if (!DIRECT) {
                    setVal('cmbInvoiceNo', keepInvoice);
                    return getJson(API + '/invoice?invoiceId=' + netI(keepInvoice)).then(function (x) { if (x && x.bookingError) box(x.bookingError); else applyBooking(x && x.booking); });
                }
                focus('cmbCustomer');
            }).catch(function (e) { box(e.message); });
        });
    }
    function headerPayload() {
        var h = {};
        ['txtdocno', 'DocDate', 'txtGPDate', 'txtVehicleNo', 'txtBilityNo', 'txtDriverName', 'txtDriverCellNo', 'txtCNICNO', 'txtFreight', 'txtNoOfContainer',
         'txtGpNo', 'txtReferenceNo', 'txtCompletedDate'].forEach(function (id) { h[id] = val(id); });
        ['combranches', 'comproject', 'cmbInvoiceNo', 'cmbContractNo', 'cmbtransporter', 'cmbShippingLIne', 'cmbShippingAgent', 'cmbCustomer',
         'cmbLoadingPort', 'cmbDestinationPort', 'cmbcareiertype'].forEach(function (id) { h[id] = netI(val(id)); h[id + 'Text'] = comboText(id); });
        if (!DIRECT) { h.chkdateisactive = $id('chkdateisactive').checked; h.lotcompletedate = S.lotcompletedate; }
        return h;
    }
    var FOCUS = {
        'Please Check Doc No': 'txtdocno', 'Please Insert  Reference No': 'txtReferenceNo', 'Please Select Customer': 'cmbCustomer', 'Please Enter GP No': 'txtGpNo',
        'Please Select Loading Port': 'cmbLoadingPort', 'Please Select Destination Port': 'cmbDestinationPort', 'Please Check No Of Container': 'txtNoOfContainer',
        'Please Enter Vehicle No': 'txtVehicleNo', 'Please Enter Bilty No': 'txtBilityNo', 'Please Select Invoice No': 'cmbInvoiceNo', 'Please Select Contract No': 'cmbContractNo',
        'Please Select Shippping Line': 'cmbShippingLIne', 'Please Select Shippping Agent': 'cmbShippingAgent', 'Please Select Transporter': 'cmbtransporter'
    };
    /** Insert(): validate (grid empty + FormValidation), the confirm, then the row checks and ExImForwarding.Save. */
    function insert(btn) {
        return busy(btn, function () {
            var body = { recId: S.recId, header: headerPayload(), rows: S.rows, otherItems: S.others };
            return postJson(API + '/validate', body).then(function (v) {
                if (!ask(v.confirm)) return;
                var win = ($id('ChkPrintPreview').checked && global.CrystalPrint) ? global.CrystalPrint.reserve() : null;
                return postJson(API + '/save', body).then(function (d) {
                    box(d.message);
                    if (win) global.CrystalPrint.open(PRINT_KEY, { id: d.id }, null, win);
                    return formReset().then(function () { if (DIRECT) return referenceLeave(); });
                }).catch(function (e) { if (win) global.CrystalPrint.release(win); box(e.message); });
            }).catch(function (e) { box(e.message); if (FOCUS[e.message]) focus(FOCUS[e.message]); });
        });
    }
    function btnSave(btn) { S.recId = 0; return insert(btn); }
    function btnUpdate(btn) { return insert(btn); }
    function btnPrint(btn) {
        if (!global.CrystalPrint) { box('Printing is not available on this page.'); return; }
        return global.CrystalPrint.open(PRINT_KEY, { id: S.recId }, btn);
    }
    function readById(id, fromReference) {
        show('btnSave', false); show('btnUpdate', true);
        S.recId = id;
        return getJson(API + '/by-id?id=' + id).then(function (d) {
            d = d || {};
            if (d.notFound) return formReset();
            var h = d.header || {};
            tab('fwForm'); if (!DIRECT) tab2('fwDetail');
            setText('txtdocno', h.DocNo); setText('txtGPDate', h.DocDate || today());
            if (DIRECT) {
                setComboText('cmbCustomer', h.Customer); setComboText('cmbDestinationPort', h.DestinationPort); setComboText('cmbLoadingPort', h.LoadingPort);
            } else {
                bind('cmbInvoiceNo', [{ Id: h.ExImInvoiceId, name: h.InvoiceNo }]); first('cmbInvoiceNo');
                bind('cmbContractNo', [{ Id: h.LCOrderId, name: h.LcOrderNo }]); first('cmbContractNo');
                bind('cmbCustomer', [{ Id: h.SupplierCustomerId, name: h.Customer }]); first('cmbCustomer');
                bind('cmbDestinationPort', [{ Id: h.DestinationPortId, name: h.DestinationPort }]); first('cmbDestinationPort');
                bind('cmbLoadingPort', [{ Id: h.LoadingPortId, name: h.LoadingPort }]); first('cmbLoadingPort');
            }
            setVal('comproject', h.ProjectsId); setVal('combranches', h.BranchesId); setVal('cmbShippingLIne', h.ShippingLineId);
            setVal('cmbShippingAgent', h.ShippingAgentId); setVal('cmbtransporter', h.TransporterId);
            if (DIRECT) setVal('cmbcareiertype', h.CarrierTypeId);
            setText('txtDriverCellNo', h.DriverCellNo); setText('txtDriverName', h.DriverName); setText('txtCNICNO', h.DriverCnicNo);
            setText('txtGPDate', h.GPDate || today());
            if (DIRECT) { setText('txtGpNo', h.GpNo); setText('txtReferenceNo', h.ReferenceNo); }
            setText('txtVehicleNo', h.VehicleNo); setText('txtBilityNo', h.BiltyNo); setText('txtFreight', h.FreightAmt); setText('txtNoOfContainer', h.NoOfContainer);
            if (!DIRECT) {
                setText('txtCompletedDate', h.LotCompletedDate || today());
                if (d.otherItemComboError) box(d.otherItemComboError); else if (d.otherItemCombo && d.otherItemCombo.length) bind('CmbOtherItem', d.otherItemCombo);
            }
            S.rows = d.rows || []; S.others = d.otherItems || []; S.detIdx = -1;
            S.preStockWt = 0; S.rows.forEach(function (r) { S.preStockWt += netD(r.StockWeight); });
            render();
            S.updateMode = true;
            if (!DIRECT) { setEnabled('cmbContractNo', false); setEnabled('cmbInvoiceNo', false); }
        }).catch(function (e) { box(e.message); });
    }

    // ------------------------------------------------------------------------------ 195 reference grid
    var REF_HIDE = { Id: 1, RecordNo: 1, FinancialYearId: 1, CarrierTypeId: 1, OtherCharges: 1, ShippedContainer: 1 };
    function dynGrid(headId, bodyId, rows, hide, buttons, codeCol) {
        var cols = rows.length ? Object.keys(rows[0]).filter(function (k) { return !hide[k]; }) : [];
        $id(headId).innerHTML = '<tr>' + buttons.map(function (b) { return '<th>' + esc(b[1]) + '</th>'; }).join('') + cols.map(function (c) { return '<th data-col="' + esc(c) + '">' + esc(c) + '</th>'; }).join('') + '</tr>';
        $id(bodyId).innerHTML = rows.map(function (r, i) {
            return '<tr data-i="' + i + '">' + buttons.map(function (b) { return '<td class="win-cell-btn"><button type="button" class="win-edit" data-act="' + b[0] + '" data-i="' + i + '">' + esc(b[1]) + '</button></td>'; }).join('')
                + cols.map(function (c) {
                    var v = r[c];
                    if (c === codeCol) return '<td><a class="win-code" href="javascript:void(0)" data-act="open" data-i="' + i + '">' + esc(str(v)) + '</a></td>';
                    if (typeof v === 'number') return '<td class="num">' + esc(fmtN(v, 3)) + '</td>';
                    if (typeof v === 'boolean') return '<td class="ctr"><input type="checkbox" disabled' + (v ? ' checked' : '') + '/></td>';
                    return '<td>' + esc(str(v)) + '</td>';
                }).join('') + '</tr>';
        }).join('');
    }
    /** txtReferenceNo_Leave -> GrdReferenceHistoryBind. */
    function referenceLeave() {
        if (!DIRECT) return Promise.resolve();
        return getJson(API + '/reference-history?referenceNo=' + encodeURIComponent(val('txtReferenceNo'))).then(function (rows) {
            REF = rows || [];
            dynGrid('refHead', 'refBody', REF, REF_HIDE, [['load', 'Load']], 'DocNo');
        }).catch(function (e) { box(e.message); });
    }
    /** GrdReferenceHistory "Load": ReadById, then Save mode, a new Doc No and RefereshForReferenceGridLoad. */
    function referenceLoad(i) {
        var r = REF[i]; if (!r) return;
        return readById(netI(r.Id), true).then(function () {
            show('btnSave', true); show('btnUpdate', false);
            return getJson(API + '/refresh').then(function (d) { if (d && d.docNo && netI(d.docNo) > 0) setText('txtdocno', String(netI(d.docNo))); });
        }).then(function () {
            setText('txtGPDate', today());
            ['txtGpNo', 'txtDriverCellNo', 'txtDriverName', 'txtCNICNO', 'txtBilityNo', 'txtVehicleNo'].forEach(function (id) { setText(id, ''); });
            S.rows.forEach(function (row) {
                ['GrossWeight', 'StockWeight', 'NoOfBags', 'NetWeight', 'EbUnit', 'EbTotal', 'AddLess'].forEach(function (c) { row[c] = 0; });
                row.ItemDescription = '0'; row['Seal#'] = '0'; row['Container#'] = '0';
            });
            render(); formDetailReset();
        }).catch(function (e) { box(e.message); });
    }

    // ------------------------------------------------------------------------------ history
    var HIST_HIDE = DIRECT ? REF_HIDE : { Id: 1, RecordNo: 1, FinancialYearId: 1 };
    function gridBind(n) {
        return getJson(API + '/history?noOfRecords=' + (n || 0)).then(function (rows) {
            HIST = rows || [];
            dynGrid('histHead', 'histBody', HIST, HIST_HIDE, [['edit', 'Edit'], ['print', 'Print']], 'DocNo');
            show('histEmpty', HIST.length === 0);
        }).catch(function (e) { box(e.message); });
    }
    function loadAll(btn) { return busy(btn, function () { return gridBind(0); }); }

    // ------------------------------------------------------------------------------ 195 register
    var REG_HIDE = { Id: 1, EntryDate: 1, ModifyDate: 1, EntryUser: 1, ApprovedDate: 1, ModifyUser: 1, DestinationPortId: 1, LoadingPortId: 1, Status: 1, OtherRemarks: 1 };
    function registerFilter() {
        return {
            referenceNo: val('txtReferenceNoRegister'), fromDate: val('txtDateFromRegister'), toDate: val('txtDateToRegister'),
            customerId: netI(val('cmbCustomerRegister')), shippingLineId: netI(val('cmbShippingLineRegister')), shippingAgentId: netI(val('cmbShippingAgentRegister')),
            transporterId: netI(val('cmbTranspoterRegister')), carrierTypeId: netI(val('cmbCarrierTypeRegister')), destinationPortId: netI(val('cmbPortDestinationRegister')),
            loadingPortId: netI(val('cmbPortLoadingRegister')), itemId: netI(val('cmbItemNameRegister'))
        };
    }
    function btnShow(btn) {
        return busy(btn, function () {
            REG = []; dynGrid('regHead', 'regBody', REG, REG_HIDE, [], null);
            return postJson(API + '/register', registerFilter()).then(function (rows) {
                REG = rows || [];
                dynGrid('regHead', 'regBody', REG, REG_HIDE, [], null);
                show('regEmpty', REG.length === 0);
            }).catch(function (e) { box(e.message); });
        });
    }
    function registerPrint(btn) {
        if (!REG.length) { box('Not Record Found For Display'); return; }
        if (!global.CrystalPrint) { box('Printing is not available on this page.'); return; }
        var f = registerFilter();
        return global.CrystalPrint.open('175-forwardingbyreferencenoregister', {
            manualBillNo: netI(f.referenceNo) !== 0 ? f.referenceNo : null, fromDate: f.fromDate, toDate: f.toDate, date: f.fromDate, documentTypeId: 206,
            supplierCustomerId: f.customerId, itemId: f.itemId, carreierTypeId: f.carrierTypeId, shippingLineId: f.shippingLineId, shippingAgentId: f.shippingAgentId,
            transporterId: f.transporterId, destinationPortId: f.destinationPortId, loadingPortId: f.loadingPortId
        }, btn);
    }
    function registerRefresh(btn) {
        return busy(btn, function () {
            focus('cmbCustomerRegister');
            return getJson(API + '/refresh').then(function (d) { d = d || {}; delete d.docNo; bindCombos(d); setText('txtReferenceNoRegister', ''); }).catch(function (e) { box(e.message); });
        });
    }

    // ------------------------------------------------------------------------------ wiring
    function on(id, ev, fn) { var e = $id(id); if (e) e.addEventListener(ev, fn); }
    document.addEventListener('DOMContentLoaded', function () {
        document.querySelectorAll('.win-tabs .win-tab[data-tab]').forEach(function (b) { b.addEventListener('click', function () { tab(b.getAttribute('data-tab')); }); });
        document.querySelectorAll('.win-tab[data-tab2]').forEach(function (b) { b.addEventListener('click', function () { tab2(b.getAttribute('data-tab2')); }); });
        document.querySelectorAll('button[data-fullscreen]').forEach(function (b) { b.addEventListener('click', function () { var bx = $id(b.getAttribute('data-fullscreen')); if (bx) bx.classList.toggle('ex-fullscreen'); }); });
        on('cmbItem', 'change', itemLeave);
        on('cmbPackUOM', 'change', uomChanged);
        on('txtNoOfBags', 'input', uomChanged);
        on('txtEbUnit', 'input', weightCalculation);
        on('txtAddLess', 'input', weightCalculation);
        if (DIRECT) {
            on('txtReferenceNo', 'change', referenceLeave);
            $id('refBody').addEventListener('click', function (e) { var b = e.target.closest('[data-act="load"]'); if (b) referenceLoad(+b.getAttribute('data-i')); });
        } else {
            on('cmbInvoiceNo', 'change', invoiceLeave);
            on('chkdateisactive', 'change', function () {
                /* chkdateisactive_CheckedChanged: enable the picker and capture its value at this moment */
                if ($id('chkdateisactive').checked) { setEnabled('txtCompletedDate', true); S.lotcompletedate = val('txtCompletedDate'); }
                else setEnabled('txtCompletedDate', false);
            });
            on('txtoQty', 'input', otherItemsCalculations);
            $id('othBody').addEventListener('dblclick', function (e) { var tr = e.target.closest('tr[data-i]'); if (tr) othDblClick(+tr.getAttribute('data-i')); });
        }
        var db = $id('detBody');
        db.addEventListener('click', function (e) {
            var a = e.target.closest('button[data-add]'); if (a) { gridButton('add', +a.getAttribute('data-add')); return; }
            var x = e.target.closest('button[data-del]'); if (x) { gridButton('del', +x.getAttribute('data-del')); return; }
            var tr = e.target.closest('tr[data-i]'); if (tr) db.querySelectorAll('tr').forEach(function (t) { t.classList.toggle('is-current', t === tr); });
        });
        db.addEventListener('dblclick', function (e) { if (e.target.closest('button')) return; var tr = e.target.closest('tr[data-i]'); if (tr) detDblClick(+tr.getAttribute('data-i')); });
        var hb = $id('histBody');
        hb.addEventListener('click', function (e) {
            var b = e.target.closest('[data-act]'); if (!b) return;
            var r = HIST[+b.getAttribute('data-i')]; if (!r) return;
            var act = b.getAttribute('data-act');
            if (act === 'edit' || act === 'open') { readById(netI(r.Id)).then(function () { if (DIRECT) return referenceLeave(); }); }
            if (act === 'print' && global.CrystalPrint) global.CrystalPrint.open(PRINT_KEY, { id: netI(r.Id) }, b);
        });
        hb.addEventListener('dblclick', function (e) {
            if (e.target.closest('button')) return;
            var tr = e.target.closest('tr[data-i]'); if (!tr) return;
            var r = HIST[+tr.getAttribute('data-i')]; if (r) readById(netI(r.Id)).then(function () { if (DIRECT) return referenceLeave(); });
        });
        document.addEventListener('keydown', function (e) {
            var k = (e.key || '').toLowerCase();
            if (e.key === 'Enter' && !e.ctrlKey && e.target && /^(INPUT|SELECT)$/.test(e.target.tagName) && e.target.type !== 'checkbox') {
                var f = Array.prototype.filter.call(document.querySelectorAll('input:not([type=hidden]):not(:disabled), select:not(:disabled), button:not(:disabled), textarea:not(:disabled)'), function (x) { return x.offsetParent !== null; });
                var i = f.indexOf(e.target); if (i >= 0 && i + 1 < f.length) { e.preventDefault(); f[i + 1].focus(); }
                return;
            }
            var saveVisible = !$id('btnSave').classList.contains('is-hidden') && !$id('btnSave').disabled;
            var updVisible = !$id('btnUpdate').classList.contains('is-hidden') && !$id('btnUpdate').disabled;
            if (e.ctrlKey && k === 's' && current() === 'fwForm' && !S.updateMode && (!DIRECT || saveVisible)) { e.preventDefault(); btnSave($id('btnSave')); }
            if (e.ctrlKey && k === 'u' && (S.updateMode || (DIRECT && updVisible))) { e.preventDefault(); btnUpdate($id('btnUpdate')); }
            if (e.ctrlKey && k === 'n') { e.preventDefault(); btnNew(); }
            if (e.ctrlKey && k === 't') { e.preventDefault(); tab(current() === 'fwHistory' ? 'fwForm' : 'fwHistory'); }
            if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { e.preventDefault(); cancel(); }
        });
        load();
    });

    global.ExportFw = {
        btnNew: btnNew, btnSave: btnSave, btnUpdate: btnUpdate, btnPrint: btnPrint, btnRefresh: btnRefresh, toggleHistory: toggleHistory,
        btnAddinGrid: btnAddinGrid, btnDetailUpdate: btnDetailUpdate, btnDetailCancel: formDetailReset,
        btnoUpdate: btnoUpdate, btnoCancel: btnoCancel, loadAll: loadAll,
        btnShow: btnShow, registerPrint: registerPrint, registerRefresh: registerRefresh,
        registerDropDown: function () { box('The register reports listed from the desktop report folder are not available on the web page; use 175-Print.'); },
        attachment: function () { box('Attachments are not available on the web page for this form.'); },
        seaPort: function () { box('Sea Ports are defined on the Sea Ports definition page.'); },
        carrierType: function () { box('Carrier Types are defined on the Inventory Lookup definition page.'); }
    };
}(window));
