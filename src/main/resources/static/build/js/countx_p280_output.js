/* ============================================================================================
 * Screen 280 "Production Against (Job Order)" - Output tab.
 * Desktop: Architecture.WinApp.Production/frmProductionOutput.cs (5,981 lines), DocumentTypeId 112,
 * with its two dialogs LoadOutPutPendingforRates.cs and frmPendingMoveOrderDocuments.
 *
 * Each function below is one C# method, named as the desktop names it, with its line number.
 * The desktop's calls are synchronous; here every server read is awaited in the same order, so
 * an event chain (e.g. CmbJobOrderNoOutput_Leave -> GetLastItemRateByJobOrder ->
 * CmbEntryType_Leave -> ItemFillOutPut) completes in the desktop's sequence.
 *
 * "Leave" events are raised from the combo's change (and the text boxes' blur); "TextChanged"
 * from input and from every programmatic assignment through setText(), which, like WinForms,
 * raises the handler only when the text actually changes.
 * ============================================================================================ */
(function () {
    'use strict';

    /* frmwagesBillHeader as a modal (ShowDialog): /production/wages-bill in an overlay iframe; the
       caller continues (reset) only after the dialog closes, as the desktop blocks until then. */
    function p280OpenWages(refDocTypeId, refDocId, grossWeightTotal, onClosed) {
        var ov = document.createElement('div');
        ov.style.cssText = 'position:fixed;inset:0;z-index:9800;background:rgba(0,0,0,.35);';
        var fr = document.createElement('iframe');
        fr.src = '/production/wages-bill?' + new URLSearchParams({ refDocTypeId: refDocTypeId, refDocId: refDocId,
                                                                  grossWeightTotal: grossWeightTotal || 0 });
        fr.style.cssText = 'position:absolute;inset:12px;width:calc(100% - 24px);height:calc(100% - 24px);border:1px solid #555;background:#fff;';
        ov.appendChild(fr); document.body.appendChild(ov);
        window.P280WagesClosed = function (r) { ov.remove(); window.P280WagesClosed = null; if (onClosed) onClosed(r); };
    }


    var API = '/api/production/production-against-job-order/output';
    var K = window.ReportKit;

    // ---------------------------------------------------------------------------- basics

    function $(id) { return document.getElementById(id); }
    function esc(s) {
        return String(s === null || s === undefined ? '' : s)
            .replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    }
    function col(row, name) {
        if (!row) return null;
        if (Object.prototype.hasOwnProperty.call(row, name)) return row[name];
        var l = name.toLowerCase();
        for (var k in row) if (Object.prototype.hasOwnProperty.call(row, k) && k.toLowerCase() === l) return row[k];
        return null;
    }
    /** Conversion.ToDouble - Convert.ToDouble(string) accepts thousands separators; bad text is 0. */
    function toD(v) {
        if (v === null || v === undefined || v === '') return 0;
        if (typeof v === 'number') return isFinite(v) ? v : 0;
        if (typeof v === 'boolean') return v ? 1 : 0;
        var s = String(v).trim().replace(/,/g, '');
        if (s === '' || !/^[-+]?(\d+\.?\d*|\.\d+)([eE][-+]?\d+)?$/.test(s)) return 0;
        var n = parseFloat(s);
        return isFinite(n) ? n : 0;
    }
    /** Conversion.ToInt - Convert.ToInt32: a number rounds half-to-even, a string must be an integer. */
    function toI(v) {
        if (v === null || v === undefined || v === '') return 0;
        if (typeof v === 'number') return roundEven(v, 0);
        if (typeof v === 'boolean') return v ? 1 : 0;
        var s = String(v).trim();
        return /^[-+]?\d+$/.test(s) ? parseInt(s, 10) : 0;
    }
    function toB(v) {
        if (v === true || v === false) return v;
        if (typeof v === 'number') return v !== 0;
        var s = String(v === null || v === undefined ? '' : v).trim().toLowerCase();
        return s === '1' || s === 'true' || s === 'yes';
    }
    /** Math.Round(x, d) - MidpointRounding.ToEven. */
    function roundEven(x, d) {
        var f = Math.pow(10, d || 0), n = x * f, r = Math.round(n);
        if (Math.abs(n % 1) === 0.5) r = 2 * Math.round(n / 2);
        return r / f;
    }
    /** C# double.ToString() (.NET Framework "G", 15 significant digits). */
    function cs(v) {
        if (v === null || v === undefined) return '';
        var n = typeof v === 'number' ? v : parseFloat(v);
        if (isNaN(n)) return 'NaN';
        if (!isFinite(n)) return n > 0 ? 'Infinity' : '-Infinity';
        if (n === 0) return '0';
        var s = n.toPrecision(15);
        if (s.indexOf('e') >= 0) return String(parseFloat(s));
        if (s.indexOf('.') >= 0) s = s.replace(/0+$/, '').replace(/\.$/, '');
        return s;
    }
    /** "#,##0.##" etc - ReportKit.num (max decimals, trailing zeros dropped). */
    function fN(v, dec) { return K.num(toD(v), dec); }
    /** "#,#" / "#,##" - grouping, no decimals, and ZERO FORMATS AS AN EMPTY STRING. */
    function fHash(v) {
        var n = Math.round(toD(v));
        if (n === 0) return '';
        return K.fixed(n, 0);
    }
    /** clsGlobalVariables.DecimalRateFormate: "#,#0." + n zeros. */
    function fRate(v) { return K.fixed(toD(v), S.cfg.rateDecimals === undefined ? 2 : S.cfg.rateDecimals); }
    function today() {
        var d = new Date();
        return d.getFullYear() + '-' + String(d.getMonth() + 1).padStart(2, '0') + '-' + String(d.getDate()).padStart(2, '0');
    }
    function addDays(iso, n) {
        var p = iso.split('-'), d = new Date(+p[0], +p[1] - 1, +p[2]);
        d.setDate(d.getDate() + n);
        return d.getFullYear() + '-' + String(d.getMonth() + 1).padStart(2, '0') + '-' + String(d.getDate()).padStart(2, '0');
    }
    function dateOnly(v) {
        var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(String(v || ''));
        return m ? m[1] + '-' + m[2] + '-' + m[3] : '';
    }
    function say(m) { var e = $('lblStatus'); if (e) e.textContent = m; }
    /** MessageBox.Show */
    function box(m) { window.alert(m); }
    /** MessageBox.Show(..., MessageBoxButtons.YesNo) == Yes */
    function yes(m) { return window.confirm(m); }

    function csrf(h) {
        var t = document.querySelector('meta[name="_csrf"]'), n = document.querySelector('meta[name="_csrf_header"]');
        if (t && n && t.getAttribute('content') && n.getAttribute('content')) h[n.getAttribute('content')] = t.getAttribute('content');
        return h;
    }
    function req(url, opt) {
        opt = opt || {};
        opt.credentials = 'same-origin';
        opt.headers = csrf(opt.headers || { 'Accept': 'application/json' });
        return fetch(url, opt).then(function (r) {
            return r.text().then(function (t) {
                var body = null;
                try { body = t ? JSON.parse(t) : null; } catch (e) { /* not json */ }
                if (!r.ok) throw new Error((body && (body.message || body.error)) || ('Request failed (' + r.status + ')'));
                return body;
            });
        });
    }
    function get(path, q) {
        var s = [];
        if (q) for (var k in q) if (q[k] !== undefined && q[k] !== null) s.push(encodeURIComponent(k) + '=' + encodeURIComponent(q[k]));
        return req(API + path + (s.length ? '?' + s.join('&') : ''));
    }
    function post(path, body) {
        return req(API + path, { method: 'POST', body: JSON.stringify(body),
            headers: { 'Accept': 'application/json', 'Content-Type': 'application/json' } });
    }

    /* Button contract: disabled with a spinner while running, duplicates ignored, re-enabled on
       success AND failure (to the state it had before). */
    function busy(btn, fn) {
        var b = typeof btn === 'string' ? $(btn) : btn;
        if (b) {
            if (b.classList.contains('is-busy')) return Promise.resolve();
            b.dataset.wasDisabled = b.disabled ? '1' : '';
            b.disabled = true;
            b.classList.add('is-busy');
        }
        var done = function () { if (b) { b.classList.remove('is-busy'); b.disabled = b.dataset.wasDisabled === '1'; } };
        var p;
        try { p = Promise.resolve(fn()); } catch (e) { p = Promise.reject(e); }
        return p.then(function (v) { done(); return v; }, function (e) { done(); box(e && e.message ? e.message : String(e)); });
    }
    /* try { ... } catch (Exception ex) { MessageBox.Show(ex.Message); } around an async handler. */
    function guard(fn) {
        return function () {
            var args = arguments;
            try {
                return Promise.resolve(fn.apply(null, args)).catch(function (e) { box(e && e.message ? e.message : String(e)); });
            } catch (e) { box(e && e.message ? e.message : String(e)); return Promise.resolve(); }
        };
    }

    // ---------------------------------------------------------------------------- combos

    var EMPTY = '';
    /** DDL.BindDDL: the list is replaced; an UltraCombo whose DataSource changes shows no row. */
    function bindCombo(id, rows, valueKey, textKey, attrs) {
        var sel = $(id);
        var h = ['<option value=""></option>'];
        (rows || []).forEach(function (r) {
            var a = '';
            if (attrs) for (var k in attrs) { var v = col(r, attrs[k]); a += ' data-' + k + '="' + esc(v === null ? '' : v) + '"'; }
            h.push('<option value="' + esc(col(r, valueKey)) + '"' + a + '>' + esc(col(r, textKey)) + '</option>');
        });
        sel.innerHTML = h.join('');
        sel.value = EMPTY;
    }
    /** DataSource = null */
    function clearCombo(id) { $(id).innerHTML = '<option value=""></option>'; $(id).value = EMPTY; }
    function cv(id) { return toI($(id).value); }
    function cvRaw(id) { return $(id).value; }
    /** UltraCombo.Text */
    function ct(id) { var s = $(id); return s.selectedIndex > 0 ? s.options[s.selectedIndex].textContent : ''; }
    /** ActiveRow != null */
    function active(id) { return $(id).value !== EMPTY; }
    function has(id, v) {
        var o = $(id).options;
        for (var i = 1; i < o.length; i++) if (o[i].value === String(v)) return true;
        return false;
    }
    /** UltraCombo.Value = v (a value not in the list leaves no active row). */
    function setCv(id, v) { $(id).value = (v !== null && v !== undefined && has(id, v)) ? String(v) : EMPTY; }
    /** UltraCombo.Text = t - picks the first row whose display text matches, else clears. */
    function setCt(id, t) {
        var o = $(id).options, s = String(t === null || t === undefined ? '' : t);
        for (var i = 1; i < o.length; i++) if (o[i].textContent === s) { $(id).selectedIndex = i; return; }
        $(id).value = EMPTY;
    }
    function selectedRow(list, id, key) {
        var v = String($(id).value);
        for (var i = 0; i < list.length; i++) if (String(col(list[i], key || 'Id')) === v) return list[i];
        return null;
    }
    function enable(id, on) { var e = $(id); e.disabled = !on; }
    function show(id, on) { $(id).classList.toggle('is-hidden', !on); }
    function visible(id) { return !$(id).classList.contains('is-hidden'); }

    // ---------------------------------------------------------------------------- TextChanged

    var ON_TEXT = {};
    var depth = 0;
    /** TextBox.Text = v : raises TextChanged only when the text changes, as WinForms does. */
    function setText(id, v) {
        var e = $(id);
        v = v === null || v === undefined ? '' : String(v);
        if (e.value === v) return;
        e.value = v;
        var h = ON_TEXT[id];
        if (h && depth < 25) { depth++; try { h(); } finally { depth--; } }
    }
    function tx(id) { return $(id).value; }

    // ---------------------------------------------------------------------------- state

    var ENTRY_TYPES = [
        { Id: 1, Name: 'ByProduct' }, { Id: 2, Name: 'FinishGoods' },
        { Id: 3, Name: 'ReturnToGodown' }, { Id: 4, Name: 'Wastage' }
    ];

    var S = {
        cfg: {}, rights: {}, fyStart: null,
        UpdateMode: false, RecIdOutPut: 0, VoucherHeadId: 0, updateDetailIndex: -1,
        AvgRate: 0, AvgRateWithoutExp: 0, OutputDetailRowsRemoveIds: '', ScheduleId: 0,
        dtCropGlobal: [], dtCropOutPut: [], dtJobGlobal: [], dtJobOutPut: [], dtuomlst: [],
        dtpacktype: [], dtItemOutPut: [], tableByProduct: [], dtReturnToGodown: [],
        dtItemOuPutGlobal: [], dtItemOuPutReturnToGodownGlobal: [],
        jobOrders: [], warehouses: [], plants: [], dtUom: [], exportContracts: [],
        currentRow: -1, history: [], histCurrent: -1
    };
    window.P280OutputState = S;

    function rateRight() { return !!S.rights.rateAndAmount; }
    function saveVisibleEnabled() { var b = $('btnSaveOutPut'); return visible('btnSaveOutPut') && !b.disabled; }

    // ============================================================================ Load (:399)

    var frmFoodProduction_Load = guard(function () {
        say('Loading...');
        return get('/load').then(function (d) {
            S.cfg = d.config || {};
            S.rights = d.rights || {};
            S.fyStart = dateOnly(d.financialYearStart);
            /* :412 */
            $('chkStockHoldForLabApproval').checked = !!(S.cfg.productionOutputLabApprovalRequired && S.cfg.stockHoldForLabApprovalAutoChecked);
            /* :436-450 */
            $('btnSaveOutPut').disabled = !S.rights.save;
            $('btnUpdateOutPut').disabled = !S.rights.update;
            $('btnVoucherOutPut').disabled = !S.rights.print;
            $('btnPrintOutPut').disabled = !S.rights.print;
            /* ctrlGrdBar7 / ctrlGrdBar11 (grdByProduct / grdOutputHistory): the shared GridBar, every item
               enabled - the desktop's :436-450 grid rights land on a bar that has no grid (reproduced as "all enabled"). */
            $('DocDateOutPut').value = today();
            /* :490 GenerateDocNumberOutPut */
            if (toI(d.docNo) > 0) $('txtDocNoOutPut').value = String(d.docNo);
            /* :491 CommondtForCombosFillOutPut */
            S.dtJobGlobal = d.jobLots || [];
            S.dtCropGlobal = d.cropYears || [];
            /* :493 UOMFill */
            S.dtuomlst = d.uoms || [];
            S.warehouses = d.warehouses || [];
            S.dtpacktype = (d.packingTypes || []).map(function (r) { return { Id: col(r, 'Id'), PackingType: col(r, 'PackTypeDesc') }; });
            S.jobOrders = d.jobOrders || [];
            JobLotOutPut();                               // :494
            CropYearOutPut();                             // :495
            var chain = EntryTypeFill();                  // :496 (ValueChanged -> CmbEntryType_Leave)
            return Promise.resolve(chain).then(function () {
                bagType();                                // :497
                Warehouse();                              // :498
                JobOrderNoFill(S.jobOrders);              // :499
                if (!S.cfg.outputItemsByJobOrderRateSchedule) S.dtItemOuPutGlobal = d.allItems || [];   // :500
                show('btnUpdateDetailOutPut', false);    // :504
                show('btnCancelOutPut', false);
                if (!rateRight()) {                       // :506
                    $('txtRemarksOutPut').style.width = '542px';
                    ['txtRateOutPut', 'lblRateOutPUt', 'lblRateUOMOUtPut', 'txtAmountOutPut', 'lblAmountOUtPUt', 'btnVoucherOutPut']
                        .forEach(function (x) { show(x, false); });
                    rateUomWrap().classList.add('is-hidden');
                }
                $('txtFromDateOutPuthistory').value = addDays(today(), -3);   // :517
                $('txttoDateOutPuthistory').value = today();
                bindHistoryJobOrders(d.historyJobOrders || []);                // :518
                gridsettingsOutPut();
                $('DocDateOutPut').focus();
                (d.errors || []).forEach(function (m) { box(m); });
                say('Ready');
            });
        });
    });

    function rateUomWrap() { var s = $('CmbRateUomOutPut'); return s.parentNode.classList.contains('dtcombo-wrap') ? s.parentNode : s; }

    /** Warehouse:531 - bound only when rows come back ("Issue From " caption). */
    function Warehouse() {
        if (S.warehouses.length > 0) bindCombo('CmbWarehouseOutput', S.warehouses, 'Id', 'WareHouseName');
    }
    /** bagType:576 */
    function bagType() {
        if (S.dtpacktype.length > 0) bindCombo('CmbPackTypeOutPut', S.dtpacktype, 'Id', 'PackingType');
    }
    /** JobOrderNoFill:598 - keeps the selected job order. Columns 2, 4 and DocumentTypeId hidden. */
    function JobOrderNoFill(rows) {
        var jobId = cv('CmbJobOrderNoOutput');
        if (rows.length > 0) {
            bindCombo('CmbJobOrderNoOutput', rows, 'Id', 'PlanCode', {
                wipac: 'WorkInProcessAc', wipitem: 'ItemName', fgrate: 'FinishGoodRate',
                wipwh: 'WipWareHouseId', whname: 'WareHouseName'
            });
            if (jobId > 0) setCv('CmbJobOrderNoOutput', jobId);
        }
    }
    function selectedJob() { return selectedRow(S.jobOrders, 'CmbJobOrderNoOutput'); }

    /** EntryTypeFill:667 - hard-coded on the desktop; row 0 activated when empty (ValueChanged -> Leave). */
    function EntryTypeFill() {
        bindCombo('CmbEntryType', ENTRY_TYPES, 'Id', 'Name');
        if (ct('CmbEntryType') === '') {
            $('CmbEntryType').value = '1';
            return CmbEntryType_Leave();
        }
    }

    /** OutPutPlantFillFromProductionInput:693 */
    function OutPutPlantFillFromProductionInput(id) {
        return get('/plants', { jobOrderId: id }).then(function (dt) {
            dt = dt || [];
            if (dt.length > 0) {
                if (active('CmbJobOrderNoOutput')) {
                    S.plants = dt;
                    bindCombo('CmbPlantFeederOutput', dt, 'PlantId', 'PlantName');
                    if (dt.length === 1) $('CmbPlantFeederOutput').value = String(col(dt[0], 'PlantId'));
                }
            } else {
                S.plants = [];
                clearCombo('CmbPlantFeederOutput');
            }
        });
    }

    /** GetVoucherHeadId:721 is part of the /{id} read. */

    // ============================================================= GetLastItemRateByJobOrder (:795)

    function GetLastItemRateByJobOrder() {
        var et = ct('CmbEntryType');
        if (et === 'Wastage') {
            enable('txtRateOutPut', true); enable('CmbRateUomOutPut', true);
        } else if (et === 'ByProduct') {
            enable('txtRateOutPut', !!S.cfg.byProductRateEditable);
            enable('CmbRateUomOutPut', !!S.cfg.byProductRateEditable);
        } else {
            enable('txtRateOutPut', false); enable('CmbRateUomOutPut', false);
        }
        var cur = S.currentRow >= 0 ? S.tableByProduct[S.currentRow] : null;
        function fromRow() {
            if (S.updateDetailIndex > -1 && cur && toD(cur.Rate) > 0) {
                setText('txtRateOutPut', cv('CmbItemOutput') === toI(cur.ItemId) ? fN(cur.Rate, 3) : '0');
            } else {
                setText('txtRateOutPut', '0');
            }
        }
        if (cv('CmbEntryType') !== 3) {
            if (cv('CmbJobOrderNoOutput') > 0 && cv('CmbEntryType') > 0 && cv('CmbItemOutput') > 0) {
                return get('/last-item-rate', {
                    jobOrderId: cv('CmbJobOrderNoOutput'), typeId: cv('CmbEntryType'),
                    itemId: cv('CmbItemOutput'), docDate: $('DocDateOutPut').value
                }).then(function (dt) {
                    if (dt && dt.length > 0) {
                        S.ScheduleId = toI(col(dt[0], 'Id'));
                        setText('txtRateOutPut', fRate(col(dt[0], 'ItemRate')));
                        return;
                    }
                    S.ScheduleId = 0;
                    if (ct('CmbEntryType') === 'FinishGoods') setText('txtRateOutPut', tx('txtAvgFinishGoodRate'));
                    else fromRow();
                });
            }
            S.ScheduleId = 0;
            if (cv('CmbJobOrderNoOutput') <= 0) return Promise.resolve();
            if (ct('CmbEntryType') === 'FinishGoods') setText('txtRateOutPut', tx('txtAvgFinishGoodRate'));
            else fromRow();
            return Promise.resolve();
        }
        return get('/avg-rate-return-to-godown', {
            jobOrderId: cv('CmbJobOrderNoOutput'), plantId: cv('CmbPlantFeederOutput'),
            itemId: cv('CmbItemOutput'), cropYear: ct('CmbCropYearOutPut')
        }).then(function (dt2) {
            if (dt2 && dt2.length > 0) {
                setText('txtRateOutPut', fRate(col(dt2[0], 'AvgRate')));
                setCv('CmbRateUomOutPut', toI(col(dt2[0], 'RateUomId')));
            }
        });
    }

    // ============================================================= toolbar

    /** btnNewOutPut_Click:930 */
    function btnNewOutPut_Click() {
        return resetOutPut().then(function () { resetDetailOutPut(); });
    }

    /** btnRefreshOutPut_Click:936 */
    function btnRefreshOutPut_Click() {
        return get('/switches').then(function (c) {
            S.cfg.wagesActiveForOutput = c.wagesActiveForOutput;
            S.cfg.saleCostingJobOrderWise = c.saleCostingJobOrderWise;
            S.cfg.outputItemsByJobOrderRateSchedule = c.outputItemsByJobOrderRateSchedule;
            return get('/uoms');
        }).then(function (u) {
            S.dtuomlst = u || [];
            if (cv('CmbEntryType') === 3) {
                return ItemBindForReturnToGodown().then(function () { return ItemFillOutPut(); });
            }
            var src = (S.cfg.outputItemsByJobOrderRateSchedule || S.cfg.jobOrderCreatewithoutRates)
                ? AllItemForOutPutAgainstJobOrder() : AllItemForOutPut();
            return src.then(function () { return ItemFillOutPut(); })
                      .then(function () { CropYearOutPut(); JobLotOutPut(); });
        }).then(function () { return get('/warehouses'); })
          .then(function (w) { S.warehouses = w || []; Warehouse(); return get('/packing-types'); })
          .then(function (p) {
              S.dtpacktype = (p || []).map(function (r) { return { Id: col(r, 'Id'), PackingType: col(r, 'PackTypeDesc') }; });
              bagType();
              return get('/job-orders');
          })
          .then(function (j) { S.jobOrders = j || []; JobOrderNoFill(S.jobOrders); return getProductionInputForReturnToGodown(); });
    }

    // ============================================================= validations

    /** FormValidationOutPut:975 */
    function FormValidationOutPut() {
        var doc = tx('txtDocNoOutPut');
        if (doc.trim() === '' || doc === '0') { box('DocNo Field Required'); $('txtDocNoOutPut').focus(); return false; }
        if (!active('CmbJobOrderNoOutput')) { box('Job Order Number Field Required'); focusCombo('CmbJobOrderNoOutput'); return false; }
        if (!active('CmbPlantFeederOutput')) { box('Plant Name Field Is Required'); focusCombo('CmbPlantFeederOutput'); return false; }
        var j = selectedJob() || {};
        if (toI(col(j, 'WorkInProcessAcId')) === 0) { box('WorkInProcess account not bind against selected job order'); focusCombo('CmbJobOrderNoOutput'); return false; }
        if (toI(col(j, 'WipItemId')) === 0) { box('WIP Item not bind against selected job order'); focusCombo('CmbJobOrderNoOutput'); return false; }
        return true;
    }

    /** FormValidationOfDetailPortionOutPut:1010 */
    function FormValidationOfDetailPortionOutPut() {
        if (!active('CmbEntryType')) { box('Entry Field Required'); focusCombo('CmbEntryType'); return false; }
        if (!active('CmbWarehouseOutput')) { box('Ware house Field Required'); focusCombo('CmbWarehouseOutput'); return false; }
        if (!active('CmbItemOutput')) { box('ItemName Field Required'); focusCombo('CmbItemOutput'); return false; }
        if (!active('CmbUomOutPut')) { box('UOM Field Required'); focusCombo('CmbUomOutPut'); return false; }
        if (!active('CmbCropYearOutPut')) { box('CropYear Field Required'); focusCombo('CmbCropYearOutPut'); return false; }
        if (!active('CmbLotOutPut')) { box('JobLot Field Required'); focusCombo('CmbLotOutPut'); return false; }
        if (!active('CmbPackTypeOutPut')) { box('Bag Type Field Required'); focusCombo('CmbPackTypeOutPut'); return false; }
        if (tx('txtQtyOutPut').trim() === '' || toD(tx('txtQtyOutPut').trim()) === 0) { box('Bag Quantity Field Required'); $('txtQtyOutPut').focus(); return false; }
        if (S.ScheduleId > 0 || rateRight()) {
            if (tx('txtRateOutPut').trim() === '' || toD(tx('txtRateOutPut').trim()) === 0) { box('Rate Field Required'); $('txtRateOutPut').focus(); return false; }
            if (!active('CmbRateUomOutPut') || toD(cvRaw('CmbRateUomOutPut')) === 0) { box('RateUom Field Required'); focusCombo('CmbRateUomOutPut'); return false; }
            if (tx('txtAmountOutPut').trim() === '' || toD(tx('txtAmountOutPut').trim()) === 0) { box('Amount Field Required'); $('txtAmountOutPut').focus(); return false; }
        }
        if (tx('txtNetWeightOutPut').trim() === '' || toD(tx('txtNetWeightOutPut').trim()) <= 0) { box('Net Weight Field Required'); $('txtNetWeightOutPut').focus(); return false; }
        return true;
    }

    function focusCombo(id) {
        var s = $(id), w = s.parentNode;
        var inp = w && w.classList.contains('dtcombo-wrap') ? w.querySelector('.dtcombo-input') : s;
        try { inp.focus(); } catch (e) { /* ignore */ }
    }

    /** ExportContractScheduleBind:1111 - BindAndRetainSelection(AllColumns, insertDefaultRow). */
    function ExportContractScheduleBind(jobOrderId) {
        var keep = cvRaw('CmbExportContractSchedule');
        return get('/export-contracts', { jobOrderId: jobOrderId, recId: S.RecIdOutPut }).then(function (dt) {
            S.exportContracts = dt || [];
            bindCombo('CmbExportContractSchedule', S.exportContracts, 'ContractScheduleId', 'ScheduleCode',
                      { contract: 'ContractNo', invoice: 'InvoiceNo' });
            /* insertDefaultRow - the "0" row. */
            var opt = document.createElement('option');
            opt.value = '0'; opt.textContent = '';
            $('CmbExportContractSchedule').insertBefore(opt, $('CmbExportContractSchedule').options[1] || null);
            if (keep !== '' && has('CmbExportContractSchedule', keep)) $('CmbExportContractSchedule').value = keep;
        });
    }

    /** JobLotOutPut:1129 */
    function JobLotOutPut() {
        var id = cv('CmbLotOutPut');
        if (cv('CmbEntryType') !== 3 && S.dtJobGlobal.length > 0) {
            S.dtJobOutPut = S.dtJobGlobal.map(function (r) { return { Id: col(r, 'Id'), JobLotDescription: col(r, 'JobLotDescription') }; });
        }
        if (S.dtJobOutPut.length > 0) {
            bindCombo('CmbLotOutPut', S.dtJobOutPut, 'Id', 'JobLotDescription');
            if (id > 0) setCv('CmbLotOutPut', id);
        } else {
            $('CmbLotOutPut').value = EMPTY;
        }
    }

    /** CropYearOutPut:1169 */
    function CropYearOutPut() {
        var id = cv('CmbCropYearOutPut');
        if (cv('CmbEntryType') !== 3 && S.dtCropGlobal.length > 0) {
            S.dtCropOutPut = S.dtCropGlobal.map(function (r) { return { Id: col(r, 'Id'), CropYear: col(r, 'CropYear') }; });
        }
        if (S.dtCropOutPut.length > 0) {
            bindCombo('CmbCropYearOutPut', S.dtCropOutPut, 'Id', 'CropYear');
            if (id > 0) setCv('CmbCropYearOutPut', id);
        } else {
            $('CmbCropYearOutPut').value = EMPTY;
        }
    }

    /** AllItemForOutPut:1209 */
    function AllItemForOutPut() {
        S.dtItemOuPutGlobal = [];
        return get('/items').then(function (r) { S.dtItemOuPutGlobal = r || []; });
    }
    /** AllItemForOutPutAgainstJobOrder:1227 */
    function AllItemForOutPutAgainstJobOrder() {
        S.dtItemOuPutGlobal = [];
        return get('/items-by-job-order', { jobOrderId: cv('CmbJobOrderNoOutput') }).then(function (r) { S.dtItemOuPutGlobal = r || []; });
    }

    /** ItemFillOutPut:1246 */
    function ItemFillOutPut() {
        var id = cv('CmbItemOutput');
        if (cv('CmbEntryType') !== 3 && S.dtItemOuPutGlobal.length > 0) {
            S.dtItemOutPut = [];
            if (S.cfg.outputItemsByJobOrderRateSchedule || S.cfg.jobOrderCreatewithoutRates) {
                if (cv('CmbJobOrderNoOutput') === 0) {
                    clearCombo('CmbItemOutput');
                    focusCombo('CmbJobOrderNoOutput');
                    box('Please Select JobOrder First!');
                    return;
                }
                var et = cv('CmbEntryType'), dt = [];
                if (et === 1 || et === 4) dt = S.dtItemOuPutGlobal.filter(function (r) { return toI(col(r, 'TransTypeId')) === 1; });
                else if (et === 2) dt = S.dtItemOuPutGlobal.filter(function (r) { return toI(col(r, 'TransTypeId')) === 2; });
                dt.forEach(function (r) { S.dtItemOutPut.push({ Id: col(r, 'Id'), ItemName: col(r, 'ItemName') }); });
            } else {
                S.dtItemOuPutGlobal.forEach(function (r) { S.dtItemOutPut.push({ Id: col(r, 'Id'), ItemName: col(r, 'ItemName') }); });
            }
        }
        if (S.dtItemOutPut.length > 0) {
            bindCombo('CmbItemOutput', S.dtItemOutPut, 'Id', 'ItemName');
            if (id > 0) setCv('CmbItemOutput', id);
        } else {
            clearCombo('CmbItemOutput');
        }
    }

    /** GenerateDocNumberOutPut:1323 */
    function GenerateDocNumberOutPut() {
        return get('/doc-no').then(function (r) { var c = toI(r && r.docNo); if (c > 0) $('txtDocNoOutPut').value = String(c); });
    }

    /** bindUomOutPut:1353 - Pack UOM and Rate UOM from the item's schedule; Rate UOM defaults to Equivalent 40. */
    function bindUomOutPut() {
        if (S.dtuomlst.length > 0) {
            var packUom = ct('CmbUomOutPut'); $('CmbUomOutPut').value = EMPTY;
            var rateUom = ct('CmbRateUomOutPut'); $('CmbRateUomOutPut').value = EMPTY;
            var itemId = cv('CmbItemOutput');
            S.dtUom = S.dtuomlst.filter(function (r) { return toI(col(r, 'ItemId')) === itemId; })
                .map(function (r) { return { Id: col(r, 'Id'), UomCode: col(r, 'UOMCode'), Equivalent: col(r, 'Equivalent') }; });
            if (S.dtUom.length > 0) {
                bindCombo('CmbUomOutPut', S.dtUom, 'Id', 'UomCode');
                bindCombo('CmbRateUomOutPut', S.dtUom, 'Id', 'UomCode');
                if (S.dtUom.some(function (r) { return r.UomCode === packUom; })) setCt('CmbUomOutPut', packUom); else $('CmbUomOutPut').value = EMPTY;
                if (S.dtUom.some(function (r) { return r.UomCode === rateUom; })) setCt('CmbRateUomOutPut', rateUom); else $('CmbRateUomOutPut').value = EMPTY;
            } else {
                clearCombo('CmbUomOutPut'); clearCombo('CmbRateUomOutPut');
            }
            var dr = S.dtUom.filter(function (r) { return toD(r.Equivalent) === 40; });
            if (dr.length !== 0) setCv('CmbRateUomOutPut', dr[0].Id); else $('CmbRateUomOutPut').value = EMPTY;
        } else {
            clearCombo('CmbUomOutPut'); clearCombo('CmbRateUomOutPut');
        }
    }
    function uomEq(id) {
        var r = selectedRow(S.dtUom, id);
        return r ? toD(r.Equivalent) : 0;
    }

    // ============================================================= Leave events

    /** CmbItemOutput_Leave:1427 */
    function CmbItemOutput_Leave() {
        OtherComboBindForReturnToGodownByItem();
        bindUomOutPut();
        return GetLastItemRateByJobOrder();
    }
    /** CmbUomOutPut_Leave:1441 */
    function CmbUomOutPut_Leave() { CalculateGrossWeight(); }

    /** Totals shared by both Leave handlers - they differ in whether "Wastage" counts as output. */
    function applyTotals(dt, withWastage) {
        if (dt && dt.length > 0) {
            var inQ = 0, inW = 0, rQ = 0, rW = 0, oQ = 0, oW = 0;
            dt.forEach(function (r) {
                var et = String(col(r, 'EntryType') || ''), eo = String(col(r, 'EntryTypeOutPut') || '');
                if (et === 'Input') { inQ += toD(col(r, 'ItemQty')); inW += toD(col(r, 'StockWeight')); }
                if (eo === 'ReturnToGodown') { rQ += toD(col(r, 'ItemQty')); rW += toD(col(r, 'StockWeight')); }
                else if (eo === 'ByProduct' || eo === 'FinishGoods' || (withWastage && eo === 'Wastage')) {
                    oQ += toD(col(r, 'ItemQty')); oW += toD(col(r, 'StockWeight'));
                }
            });
            inQ -= rQ; inW -= rW;
            $('txtInPutTotalQty').value = fHash(inQ);
            $('txtInPutTotalWeight').value = fHash(inW);
            $('txtOutQtyTotal').value = fHash(oQ);
            $('txtOutWeightTotal').value = fHash(oW);
            $('txtBalQtyTotal').value = fHash(inQ - oQ);
            $('txtBalWeightTotal').value = fHash(inW - oW);
        } else {
            ['txtInPutTotalQty', 'txtInPutTotalWeight', 'txtOutQtyTotal', 'txtOutWeightTotal', 'txtBalQtyTotal', 'txtBalWeightTotal']
                .forEach(function (x) { $(x).value = '0'; });
        }
    }

    /** CmbJobOrderNoOutput_Leave:1446 */
    function CmbJobOrderNoOutput_Leave() {
        var job = cv('CmbJobOrderNoOutput');
        return OutPutPlantFillFromProductionInput(job).then(function () {
            return get('/totals', { jobOrderId: job, plantId: cv('CmbPlantFeederOutput') });
        }).then(function (dt) {
            /* :1484 - this handler counts ByProduct and FinishGoods only; Wastage is NOT output here. */
            applyTotals(dt, false);
            if (active('CmbJobOrderNoOutput') || cv('CmbJobOrderNoOutput') > 0) {
                var j = selectedJob() || {};
                var fg = col(j, 'FinishGoodRate');
                if (saveVisibleEnabled()) setText('txtAvgFinishGoodRate', fg === null ? '' : cs(fg));
                else if (toD(tx('txtAvgFinishGoodRate').trim()) === 0) setText('txtAvgFinishGoodRate', fg === null ? '' : cs(fg));
                $('txtWIPAccount').value = col(j, 'WorkInProcessAc') || '';
                $('txtWIPItem').value = col(j, 'ItemName') || '';
            } else {
                setText('txtAvgFinishGoodRate', '0');
            }
            if (S.cfg.outputItemsByJobOrderRateSchedule || S.cfg.jobOrderCreatewithoutRates) return AllItemForOutPutAgainstJobOrder();
        }).then(function () { return GetLastItemRateByJobOrder(); })
          .then(function () { if (!S.dtReturnToGodown || S.dtReturnToGodown.length === 0) return getProductionInputForReturnToGodown(); })
          .then(function () { return CmbEntryType_Leave(); })
          .then(function () { return ExportContractScheduleBind(cv('CmbJobOrderNoOutput')); });
    }

    /** CmbEntryType_Leave:1545 (also ValueChanged) */
    function CmbEntryType_Leave() {
        var p = cv('CmbEntryType') === 3 ? ItemBindForReturnToGodown() : Promise.resolve(ItemFillOutPut());
        return p.then(function () { return GetLastItemRateByJobOrder(); });
    }

    /** getProductionInputForReturnToGodown:1565 - usp_GetProductionInPutDataByJobAndPlantId, doc 80. */
    function getProductionInputForReturnToGodown() {
        return get('/return-to-godown', { jobOrderId: cv('CmbJobOrderNoOutput'), plantId: cv('CmbPlantFeederOutput') })
            .then(function (r) { S.dtReturnToGodown = r || []; });
    }

    /** ItemBindForReturnToGodown:1577 */
    function ItemBindForReturnToGodown() {
        if (!S.dtReturnToGodown || S.dtReturnToGodown.length === 0) {
            clearCombo('CmbItemOutput'); clearCombo('CmbLotOutPut'); clearCombo('CmbCropYearOutPut');
            return Promise.resolve();
        }
        S.dtItemOutPut = [];
        /* dtItemOuPutReturnToGodownGlobal is never filled, so the "already there" test never
           matches and every input row adds its item - an item repeats once per input row. */
        S.dtReturnToGodown.forEach(function (r) {
            if (!S.dtItemOuPutReturnToGodownGlobal.some(function (x) { return toI(x.Id) === toI(col(r, 'ItemId')); })) {
                S.dtItemOutPut.push({ Id: col(r, 'ItemId'), ItemName: col(r, 'ItemName') });
            }
        });
        ItemFillOutPut();
        return CmbItemOutput_Leave();
    }

    /** OtherComboBindForReturnToGodownByItem:1608 - lots and crops from ALL input rows, not the item's. */
    function OtherComboBindForReturnToGodownByItem() {
        S.dtJobOutPut = [];
        S.dtCropOutPut = [];
        if (cv('CmbEntryType') === 3) {
            if (!active('CmbItemOutput') || cv('CmbItemOutput') === 0) {
                clearCombo('CmbLotOutPut'); clearCombo('CmbCropYearOutPut');
                return;
            }
            S.dtReturnToGodown.forEach(function (r) {
                if (!S.dtJobOutPut.some(function (x) { return toI(x.Id) === toI(col(r, 'JobLotId')); }))
                    S.dtJobOutPut.push({ Id: col(r, 'JobLotId'), JobLotDescription: col(r, 'JobLotDescription') });
                if (!S.dtCropOutPut.some(function (x) { return toI(x.Id) === toI(col(r, 'CropYearId')); }))
                    S.dtCropOutPut.push({ Id: col(r, 'CropYearId'), CropYear: col(r, 'CropBatch') });
            });
        }
        JobLotOutPut();
        CropYearOutPut();
    }

    // ============================================================= the detail grid

    /** grdByProduct_DoubleClick:1650 (also Ctrl+Enter) */
    function grdByProduct_DoubleClick() {
        var item = S.currentRow >= 0 ? S.tableByProduct[S.currentRow] : null;
        if (!item) return Promise.resolve();
        S.updateDetailIndex = S.currentRow;
        setCt('CmbEntryType', item.EntryType);
        return CmbEntryType_Leave().then(function () {
            setCv('CmbWarehouseOutput', item.WareHouseId); if (!active('CmbWarehouseOutput')) setCt('CmbWarehouseOutput', item.WareHouse);
            setCv('CmbLotOutPut', item.JobLotId); if (!active('CmbLotOutPut')) setCt('CmbLotOutPut', item.JobLot);
            setCv('CmbPackTypeOutPut', item.PackingTypeId); if (!active('CmbPackTypeOutPut')) setCt('CmbPackTypeOutPut', item.PackingType);
            setCv('CmbItemOutput', item.ItemId); if (!active('CmbItemOutput')) setCt('CmbItemOutput', item.Item);
            return CmbItemOutput_Leave();
        }).then(function () {
            setCv('CmbUomOutPut', item.ItemUOMId); if (!active('CmbUomOutPut')) setCt('CmbUomOutPut', item.ItemUOM);
            setCt('CmbCropYearOutPut', item.CropYear);
            setText('txtQtyOutPut', fN(item.Quantity, 2));
            setText('txtGrossWeight', fN(item.GrossWeight, 3));
            setText('txtEbUnit', fN(item.EbUnit, 4));
            setText('txtEbTotal', fN(item.EbTotal, 3));
            setText('txtNetWeightOutPut', fN(item.Weight, 3));
            setText('txtRateOutPut', fRate(item.Rate));
            setCv('CmbRateUomOutPut', item.RateUOMId); if (!active('CmbRateUomOutPut')) setCt('CmbRateUomOutPut', item.RateUOM);
            setText('txtAmountOutPut', fN(item.ItemAmount, 3));
            $('txtRemarksOutPut').value = item.Remarks || '';
            show('btnAddOutPut', false);
            show('btnUpdateDetailOutPut', true);
            show('btnCancelOutPut', true);
            focusCombo('CmbEntryType');
        });
    }

    /** btnUpdateDetailOutPut_Click:1694 - CropYearId, IssueWeight and StockAc are NOT updated. */
    function btnUpdateDetailOutPut_Click() {
        if (!FormValidationOfDetailPortionOutPut()) return;
        var r = S.tableByProduct[S.updateDetailIndex];
        if (!r) throw new Error('There is no row at position ' + S.updateDetailIndex + '.');
        if (!active('CmbRateUomOutPut')) throw new Error('Object reference not set to an instance of an object.');
        r.EntryType = ct('CmbEntryType').trim();
        r.WareHouseId = cvRaw('CmbWarehouseOutput');
        r.WareHouse = ct('CmbWarehouseOutput').trim();
        r.JobLotId = cvRaw('CmbLotOutPut');
        r.JobLot = ct('CmbLotOutPut').trim();
        r.PackingTypeId = cvRaw('CmbPackTypeOutPut');
        r.PackingType = ct('CmbPackTypeOutPut').trim();
        r.ItemId = cvRaw('CmbItemOutput');
        r.Item = ct('CmbItemOutput').trim();
        r.ItemUOMId = cvRaw('CmbUomOutPut');
        r.ItemUOM = ct('CmbUomOutPut').trim();
        r.ItemEquivalent = uomEq('CmbUomOutPut');
        r.CropYear = ct('CmbCropYearOutPut').trim();
        r.Quantity = toD(tx('txtQtyOutPut').trim());
        r.GrossWeight = toD(tx('txtGrossWeight').trim());
        r.EbUnit = toD(tx('txtEbUnit').trim());
        r.EbTotal = toD(tx('txtEbTotal').trim());
        r.Weight = toD(tx('txtNetWeightOutPut').trim());
        r.Rate = toD(tx('txtRateOutPut').trim());
        r.RateEquivalent = uomEq('CmbRateUomOutPut');
        r.RateUOM = ct('CmbRateUomOutPut').trim();
        r.RateUOMId = cvRaw('CmbRateUomOutPut');
        r.ItemAmount = toD(tx('txtAmountOutPut').trim());
        r.Remarks = tx('txtRemarksOutPut').trim();
        r.ScheduleId = S.ScheduleId;
        show('btnUpdateDetailOutPut', false);
        show('btnCancelOutPut', false);
        show('btnAddOutPut', true);
        resetDetailOutPut();
        renderByProduct();
    }

    /** btnCancelOutPut_Click:1738 */
    function btnCancelOutPut_Click() {
        show('btnAddOutPut', true);
        show('btnUpdateDetailOutPut', false);
        show('btnCancelOutPut', false);
        resetDetailOutPut();
    }

    /** btnAddOutPut_Click:1753 */
    function btnAddOutPut_Click() {
        if (!FormValidationOfDetailPortionOutPut()) return;
        var rateUom = active('CmbRateUomOutPut') ? uomEq('CmbRateUomOutPut') : 0;
        S.tableByProduct.push({
            Id: 0, EntryType: ct('CmbEntryType'), WareHouseId: cvRaw('CmbWarehouseOutput'), WareHouse: ct('CmbWarehouseOutput'),
            ItemId: cvRaw('CmbItemOutput'), Item: ct('CmbItemOutput'), ItemUOMId: cvRaw('CmbUomOutPut'), ItemUOM: ct('CmbUomOutPut'),
            ItemEquivalent: uomEq('CmbUomOutPut'), CropYearId: cvRaw('CmbCropYearOutPut'), CropYear: ct('CmbCropYearOutPut'),
            JobLotId: cvRaw('CmbLotOutPut'), JobLot: ct('CmbLotOutPut'), PackingTypeId: cvRaw('CmbPackTypeOutPut'),
            PackingType: ct('CmbPackTypeOutPut'), Quantity: toD(tx('txtQtyOutPut')), GrossWeight: toD(tx('txtGrossWeight')),
            EbUnit: toD(tx('txtEbUnit')), EbTotal: toD(tx('txtEbTotal')), Weight: toD(tx('txtNetWeightOutPut')),
            Rate: toD(tx('txtRateOutPut').trim()), RateUOMId: cv('CmbRateUomOutPut'), RateUOM: ct('CmbRateUomOutPut'),
            RateEquivalent: rateUom, ItemAmount: toD(tx('txtAmountOutPut')), Remarks: tx('txtRemarksOutPut'),
            StockAcId: 0, StockAc: '', IssueWeight: 0, ScheduleId: S.ScheduleId
        });
        if (S.ScheduleId > 0) enable('CmbJobOrderNoOutput', false);
        resetDetailOutPut();
        gridsettingsOutPut();
        focusCombo('CmbEntryType');
    }

    /* gridsettingsOutPut:1782 - visible columns in DataTable order; ids, equivalents, StockAc and
       ScheduleId hidden; Rate / RateUOM / ItemAmount hidden without the Rate right; "Weight" is
       captioned "Net Weight"; six totals; the X delete button last. */
    var BYPRODUCT_COLS = [
        { k: 'EntryType', c: 'EntryType' }, { k: 'WareHouse', c: 'WareHouse' }, { k: 'Item', c: 'Item' },
        { k: 'ItemUOM', c: 'ItemUOM' }, { k: 'CropYear', c: 'CropYear' }, { k: 'JobLot', c: 'JobLot' },
        { k: 'PackingType', c: 'PackingType' },
        { k: 'Quantity', c: 'Quantity', f: function (v) { return fN(v, 2); }, sum: true },
        { k: 'GrossWeight', c: 'GrossWeight', f: function (v) { return fN(v, 3); }, sum: true },
        { k: 'EbUnit', c: 'EbUnit', f: function (v) { return fN(v, 4); }, sum: true },
        { k: 'EbTotal', c: 'EbTotal', f: function (v) { return fN(v, 4); }, sum: true },
        { k: 'Weight', c: 'Net Weight', f: function (v) { return fN(v, 2); }, sum: true },
        { k: 'Rate', c: 'Rate', num: true, rate: true, f: function (v) { return cs(toD(v)); } },
        { k: 'RateUOM', c: 'RateUOM', rate: true },
        { k: 'ItemAmount', c: 'ItemAmount', f: function (v) { return fN(v, 3); }, sum: true, rate: true },
        { k: 'Remarks', c: 'Remarks' },
        { k: 'IssueWeight', c: 'IssueWeight', num: true, f: function (v) { return cs(toD(v)); } }
    ];
    var gridBuilt = false;
    function gridsettingsOutPut() { gridBuilt = true; renderByProduct(); }

    function renderByProduct() {
        var t = $('grdByProduct');
        if (!gridBuilt) { t.tHead.innerHTML = ''; t.tBodies[0].innerHTML = ''; t.tFoot.innerHTML = ''; return; }
        var cols = BYPRODUCT_COLS.filter(function (c) { return !c.rate || rateRight(); });
        var h = '<tr>' + cols.map(function (c) { return '<th' + (c.f || c.num ? ' class="num"' : '') + '>' + esc(c.c) + '</th>'; }).join('') + '<th style="width:20px;">X</th></tr>';
        h += '<tr class="filter-row">' + cols.map(function (c, i) { return '<td><input data-fcol="' + i + '"></td>'; }).join('') + '<td></td></tr>';
        t.tHead.innerHTML = h;
        var sums = {};
        var body = S.tableByProduct.map(function (r, i) {
            cols.forEach(function (c) { if (c.sum) sums[c.k] = (sums[c.k] || 0) + toD(r[c.k]); });
            return '<tr data-i="' + i + '"' + (i === S.currentRow ? ' class="is-current"' : (i % 2 ? ' class="alt"' : '')) + '>'
                + cols.map(function (c) {
                    var v = r[c.k];
                    return '<td' + (c.f || c.num ? ' class="num"' : '') + '>' + esc(c.f ? c.f(v) : (v === null || v === undefined ? '' : v)) + '</td>';
                }).join('')
                + '<td><button type="button" class="cellbtn" data-del="' + i + '">X</button></td></tr>';
        }).join('');
        t.tBodies[0].innerHTML = body;
        t.tFoot.innerHTML = S.tableByProduct.length ? '<tr>' + cols.map(function (c) {
            return '<td' + (c.sum ? ' class="num"' : '') + '>' + (c.sum ? esc(c.f(sums[c.k] || 0)) : '') + '</td>';
        }).join('') + '<td></td></tr>' : '';
        applyFilter(t);
    }

    /** GridEX FilterMode - Contains on each column. */
    function applyFilter(t) {
        var inputs = t.tHead.querySelectorAll('input[data-fcol]');
        var f = [];
        inputs.forEach(function (x) { f[+x.getAttribute('data-fcol')] = x.value.toLowerCase(); });
        Array.prototype.forEach.call(t.tBodies[0].rows, function (tr) {
            var ok = true;
            f.forEach(function (q, i) { if (q && tr.cells[i] && tr.cells[i].textContent.toLowerCase().indexOf(q) < 0) ok = false; });
            tr.style.display = ok ? '' : 'none';
        });
    }

    /** grdByProduct_ColumnButtonClick:1948 - the X column. */
    function deleteRow(i) {
        if (!S.tableByProduct[i]) return;
        if (!yes('Are you sure to Delete?')) return;
        var r = S.tableByProduct[i];
        if (toI(r.Id) !== 0) S.OutputDetailRowsRemoveIds = S.OutputDetailRowsRemoveIds + ',' + String(r.Id);
        S.tableByProduct.splice(i, 1);
        if (S.currentRow >= S.tableByProduct.length) S.currentRow = S.tableByProduct.length - 1;
        if (S.tableByProduct.length === 0) enable('CmbJobOrderNoOutput', true);
        renderByProduct();
    }

    // ============================================================= calculations

    /** CalculateGrossWeight:1986 - only while Save is the visible, enabled button. */
    function CalculateGrossWeight() {
        var qty = toD(tx('txtQtyOutPut').trim()) > 0 ? toD(tx('txtQtyOutPut').trim()) : 0;
        var uom = 0;
        if (saveVisibleEnabled()) {
            if (active('CmbUomOutPut') && qty > 0) uom = uomEq('CmbUomOutPut');
            setText('txtGrossWeight', fN(qty * uom, 3));
        }
    }

    /** Total:2008 - ActiveControl.Tag decides which empty-bag field is derived from the other. */
    function Total() {
        var qty = toD(tx('txtQtyOutPut').trim()) > 0 ? toD(tx('txtQtyOutPut').trim()) : 0;
        var gross = toD(tx('txtGrossWeight'));
        var ebUnit = toD(tx('txtEbUnit'));
        var ebTotal = toD(tx('txtEbTotal'));
        var tag = document.activeElement ? document.activeElement.getAttribute('data-tag') : null;
        if (tag === 'EbUnit') {
            ebTotal = ebUnit * qty;
            setText('txtEbTotal', fN(ebTotal, 3));
        } else if (tag === 'EbTotal') {
            ebUnit = ebTotal / qty;
            setText('txtEbUnit', isFinite(ebUnit) ? fN(ebUnit, 4) : (isNaN(ebUnit) ? 'NaN' : '∞'));
        } else {
            ebUnit = qty > 0 ? ebTotal / qty : 0;
            setText('txtEbUnit', fN(ebUnit, 4));
        }
        setText('txtNetWeightOutPut', cs(gross - ebTotal));   // NetWeight.ToString() - no format
    }

    /** TotalAmount:2041 */
    function TotalAmount() {
        if (ct('CmbUomOutPut') !== '' && tx('txtQtyOutPut') !== '' && tx('txtNetWeightOutPut') !== ''
            && tx('txtRateOutPut') !== '' && ct('CmbRateUomOutPut') !== '' && active('CmbRateUomOutPut')) {
            var rate = toD(tx('txtRateOutPut').trim());
            var net = toD(tx('txtNetWeightOutPut').trim());
            var ru = uomEq('CmbRateUomOutPut');
            setText('txtAmountOutPut', fN(net / ru * rate, 3));
        }
    }

    ON_TEXT.txtQtyOutPut = CalculateGrossWeight;       // txtQtyOutPut_TextChanged:2063
    ON_TEXT.txtRateOutPut = TotalAmount;               // :2068
    ON_TEXT.txtGrossWeight = Total;                    // :3074
    ON_TEXT.txtEbUnit = Total;                         // :3110
    ON_TEXT.txtEbTotal = Total;                        // :3122
    ON_TEXT.txtNetWeightOutPut = TotalAmount;          // :3025
    ON_TEXT.txtFGWeight = function () { txtFGWeight_TextChanged(); };

    /** ValidateMoveOrder:2093 */
    function ValidateMoveOrder() {
        if (cv('CmbMoveOrderTicket') > 0) {
            var total = S.tableByProduct.reduce(function (a, r) { return a + toD(r.GrossWeight); }, 0);
            var avail = toD(tx('txtMoveOrderBalWeight'));
            if (total > avail) {
                throw new Error("The total weight in the grid '" + cs(total) + "' exceeds the available Move Order balance weight '" + cs(avail) + "'.");
            }
        }
    }

    // ============================================================= ReadByIdOutPut (:2113)

    function ReadByIdOutPut(ID) {
        if (ID <= 0) return Promise.resolve();
        S.RecIdOutPut = ID;
        show('btnSaveOutPut', false);
        show('btnUpdateOutPut', true);
        S.OutputDetailRowsRemoveIds = '';
        return req(API + '/' + ID).then(function (d) {
            var SC = d && d.header;
            if (!SC) return;
            if (toB(col(SC, 'IsApproved'))) throw new Error("You Can't Update Approved Record");
            var status = col(SC, 'PlanStatus') === null ? '' : String(col(SC, 'PlanStatus'));
            if (status !== 'In Process') throw new Error("You Can't Update Record with " + status + ' JobPlanStatus');
            var det = d.details || [];
            $('txtDocNoOutPut').value = col(SC, 'DocCode') === null ? '' : String(col(SC, 'DocCode'));
            $('DocDateOutPut').value = dateOnly(col(SC, 'DocDate'));
            $('txtRemarksHeaderOutPut').value = col(SC, 'MainRemarks') || '';
            setCv('CmbJobOrderNoOutput', col(SC, 'InvJobOrderId'));
            $('chkStockHoldForLabApproval').checked = toB(col(SC, 'StockHoldForLabApproval'));
            if (det.length > 0) {
                var first = det[0];
                BindMoveOrderTicket(toI(col(first, 'MoveOrderDocId')), toI(col(first, 'WbTicketNo')));
                $('txtMoveOrderNetWeight').value = fHash(col(first, 'TotalWbWeight'));
                $('txtMoveOrderAlreadUsedWeight').value = fHash(col(first, 'UsedWbWeight'));
                $('txtMoveOrderBalWeight').value = fHash(col(first, 'AvailableWbWeight'));
            }
            return CmbJobOrderNoOutput_Leave().then(function () {
                setCv('CmbPlantFeederOutput', col(SC, 'PlantId'));
                setCv('CmbExportContractSchedule', col(SC, 'ContractScheduleId'));
                S.tableByProduct = [];
                det.forEach(function (x) {
                    if (col(x, 'EntryType') !== 'Issue') {
                        showView('form');
                        S.tableByProduct.push({
                            Id: col(x, 'Id'), EntryType: col(x, 'EntryType'), WareHouseId: col(x, 'WarehouseId'),
                            WareHouse: col(x, 'WareHouseName'), ItemId: col(x, 'ItemId'), Item: col(x, 'ItemName'),
                            ItemUOMId: col(x, 'ItemUomId'), ItemUOM: col(x, 'PackUom'), ItemEquivalent: col(x, 'Equivalent'),
                            /* :2165 - CropBatch goes into BOTH CropYearId and CropYear. */
                            CropYearId: col(x, 'CropBatch'), CropYear: col(x, 'CropBatch'),
                            JobLotId: col(x, 'JobLotId'), JobLot: col(x, 'JobLotDescription'),
                            PackingTypeId: col(x, 'PackingtypeId'), PackingType: col(x, 'PackTypeDesc'),
                            Quantity: toD(col(x, 'Qty')), GrossWeight: toD(col(x, 'GrossWeight')), EbUnit: toD(col(x, 'EbUnit')),
                            EbTotal: toD(col(x, 'EbTotal')), Weight: toD(col(x, 'Weight')),
                            Rate: roundEven(toD(col(x, 'NetRate')), 2), RateUOMId: toI(col(x, 'RateUOMId')),
                            RateUOM: col(x, 'RateUOM'), RateEquivalent: toD(col(x, 'RateEquivalent')),
                            ItemAmount: toD(col(x, 'TotalAmount')), Remarks: col(x, 'Remarks'),
                            StockAcId: col(x, 'StockAcId'), StockAc: col(x, 'AccountTitle'),
                            IssueWeight: toD(col(x, 'IssueWeight')), ScheduleId: toI(col(x, 'JobOrderScheduleId'))
                        });
                        if (col(x, 'EntryType') === 'FinishGoods' && toD(col(x, 'NetRate')) > 0) setText('txtAvgFinishGoodRate', cs(toD(col(x, 'NetRate'))));
                    }
                });
                if (det.some(function (x) { return toI(col(x, 'JobOrderScheduleId')) > 0 && !S.cfg.byProductRateEditable; })) {
                    enable('CmbJobOrderNoOutput', false);
                }
                S.currentRow = S.tableByProduct.length ? 0 : -1;
                gridsettingsOutPut();
                S.VoucherHeadId = toI(d.voucherHeadId);        // GetVoucherHeadId(RecIdOutPut, 112)
            });
        });
    }
    /* The shell's Transaction History "Edit" calls OutputForm.ReadByIdOutPut(...) (FoodProductionWithValues:1116). */
    window.P280ReadById = function (id) { return guard(ReadByIdOutPut)(toI(id)); };

    // ============================================================= Save / Update (:2187-2381)

    function btnSaveOutPut_Click() { S.RecIdOutPut = 0; return OutPutInsert(); }
    function btnUpdateOutPut_Click() { return OutPutInsert(); }

    function OutPutInsert() {
        if (!FormValidationOutPut()) return Promise.resolve();
        ValidateMoveOrder();
        if (S.RecIdOutPut > 0) { if (!yes('Are you sure to Update?')) return Promise.resolve(); }
        else if (!yes('Are you sure to Save?')) return Promise.resolve();
        var j = selectedJob() || {};
        if (toI(col(j, 'WipWareHouseId')) === 0) throw new Error('Please Add WareHouse In JobOrder');
        if (S.tableByProduct.length === 0) { box('Enter Detail First ...'); return Promise.resolve(); }
        var body = {
            recId: S.RecIdOutPut,
            docCode: tx('txtDocNoOutPut'),
            docDate: $('DocDateOutPut').value,
            remarks: tx('txtRemarksHeaderOutPut'),
            jobOrderId: cv('CmbJobOrderNoOutput'),
            plantId: cv('CmbPlantFeederOutput'),
            contractScheduleId: cv('CmbExportContractSchedule'),
            stockHold: $('chkStockHoldForLabApproval').checked,
            moveOrderDocId: cv('CmbMoveOrderTicket'),
            removeIds: S.OutputDetailRowsRemoveIds || '',
            rows: S.tableByProduct.map(function (r) {
                return {
                    id: toI(r.Id), entryType: r.EntryType, wareHouseId: r.WareHouseId, itemId: r.ItemId, item: r.Item,
                    itemUomId: r.ItemUOMId, cropYear: r.CropYear, jobLotId: r.JobLotId, packingTypeId: r.PackingTypeId,
                    quantity: toD(r.Quantity), grossWeight: toD(r.GrossWeight), ebUnit: toD(r.EbUnit), ebTotal: toD(r.EbTotal),
                    weight: toD(r.Weight), rate: toD(r.Rate), rateUomId: toI(r.RateUOMId), rateEquivalent: toD(r.RateEquivalent),
                    remarks: r.Remarks, stockAcId: toI(r.StockAcId), scheduleId: toI(r.ScheduleId)
                };
            })
        };
        return post('/save', body).then(function (res) {
            box(res.message);
            if (res.wages) {
                /* :2359-2367 - bill removed by reference on the server, then frmwagesBillHeader (112). */
                return new Promise(function (done) {
                    p280OpenWages(112, res.wagesRefDocId, res.grossWeightTotal, function () { done(); });
                }).then(function () { return resetOutPut().then(function () { resetDetailOutPut(); }); });
            }
            return resetOutPut().then(function () { resetDetailOutPut(); });
        });
    }

    /** resetOutPut:2383 - the job order stays selected (it is re-filled, not cleared). */
    function resetOutPut() {
        S.RecIdOutPut = 0;
        S.VoucherHeadId = 0;
        $('txtWIPAccount').value = '';
        $('txtWIPItem').value = '';
        S.OutputDetailRowsRemoveIds = '';
        ['txtInPutTotalQty', 'txtInPutTotalWeight', 'txtOutQtyTotal', 'txtOutWeightTotal', 'txtBalQtyTotal', 'txtBalWeightTotal']
            .forEach(function (x) { $(x).value = '0'; });
        show('btnSaveOutPut', true);
        show('btnUpdateOutPut', false);
        enable('CmbJobOrderNoOutput', true);
        S.tableByProduct = [];
        S.currentRow = -1;
        gridBuilt = false;
        renderByProduct();                                   // ClearStructure
        return GenerateDocNumberOutPut()
            .then(function () { return get('/job-orders'); })
            .then(function (j) {
                S.jobOrders = j || [];
                JobOrderNoFill(S.jobOrders);
                setText('txtAvgFinishGoodRate', '');
                return CmbJobOrderNoOutput_Leave();
            })
            .then(function () {
                clearCombo('CmbMoveOrderTicket');
                $('txtMoveOrderNetWeight').value = '';
                $('txtMoveOrderAlreadUsedWeight').value = '';
                $('txtMoveOrderBalWeight').value = '';
                show('btnAddOutPut', true);
                show('btnUpdateDetailOutPut', false);
                show('btnCancelOutPut', false);
            });
    }

    /** resetDetailOutPut:2422 - entry type, warehouse, crop, lot and pack type are kept. */
    function resetDetailOutPut() {
        S.ScheduleId = 0;
        S.AvgRate = 0;
        S.AvgRateWithoutExp = 0;
        setText('txtRateOutPut', '');
        setText('txtAmountOutPut', '');
        $('CmbRateUomOutPut').value = EMPTY;
        $('CmbItemOutput').value = EMPTY;
        $('CmbUomOutPut').value = EMPTY;
        setText('txtQtyOutPut', '');
        setText('txtGrossWeight', '');
        setText('txtEbUnit', '');
        setText('txtEbTotal', '');
        setText('txtNetWeightOutPut', '');
        $('txtRemarksOutPut').value = '';
        setText('txtRateOutPut', '');
        S.updateDetailIndex = -1;
    }

    // ============================================================= History tab

    /** jobOrderOutputHCombobind:2466 */
    function bindHistoryJobOrders(rows) {
        if (rows.length > 0) bindCombo('CmbJobOrderOutPuthistory', rows, 'Id', 'ReferenceName');
        else clearCombo('CmbJobOrderOutPuthistory');
    }
    function BtnRefereshOutPutHistory_Click() { return get('/history-job-orders').then(function (r) { bindHistoryJobOrders(r || []); }); }

    /** ResetOutputhistory:2535 */
    function BtnNewOutPuthistory_Click() {
        $('txtDocNoFromOutPuthistory').value = '';
        $('txtDocnotoOutPuthistory').value = '';
        $('CmbJobOrderOutPuthistory').value = EMPTY;
        if (S.fyStart) $('txtFromDateOutPuthistory').value = S.fyStart;
        $('txtFromDateOutPuthistory').focus();
    }

    var HISTORY_COLS = [
        { k: 'DocNo' }, { k: 'WagesNo' }, { k: 'DocDate', d: true }, { k: 'JobOrderNo' }, { k: 'PlantName' },
        { k: 'WbTicketNo' }, { k: 'TotalWbWeight', n: true }, { k: 'EntryType' }, { k: 'ContractSchedule' },
        { k: 'Remarks' }, { k: 'IsApproved' }, { k: 'StartDate', d: true }, { k: 'EndDate', d: true },
        { k: 'EntryDate', t: true }, { k: 'EntryUser' }, { k: 'ModifyDate', t: true }, { k: 'ModifyUser' }, { k: 'PlanStatus' }
    ];

    /** OutputGridHistory:2575 - no rows leaves the grid as it was. */
    function OutputGridHistory() {
        return get('/history', {
            fromDate: $('txtFromDateOutPuthistory').value, toDate: $('txttoDateOutPuthistory').value,
            docFrom: tx('txtDocNoFromOutPuthistory'), docTo: tx('txtDocnotoOutPuthistory'),
            jobOrderId: cv('CmbJobOrderOutPuthistory')
        }).then(function (dt) {
            if (!dt || dt.length <= 0) return;
            S.history = dt.map(function (r) {
                return {
                    Id: col(r, 'Id'), DocNo: col(r, 'DocCode'), WagesNo: col(r, 'WagesNo'), DocDate: dateOnly(col(r, 'DocDate')),
                    JobOrderNo: col(r, 'InvJobOrderNo'), PlantName: col(r, 'PlantName'), WbTicketNo: col(r, 'WbTicketNo'),
                    TotalWbWeight: col(r, 'TotalWbWeight'), EntryType: col(r, 'EntryType'), ContractSchedule: col(r, 'ScheduleCode'),
                    Remarks: col(r, 'MainRemarks'), IsApproved: col(r, 'JobOrderApprovedStatus'),
                    StartDate: dateOnly(col(r, 'StartDate')), EndDate: dateOnly(col(r, 'EndDate')),
                    EntryDate: col(r, 'EntryDate'), EntryUser: col(r, 'EntryUser'), ModifyDate: col(r, 'ModifyDate'),
                    ModifyUser: col(r, 'ModifyUser'), PlanStatus: col(r, 'PlanStatus'), WagesId: col(r, 'WagesId')
                };
            });
            S.histCurrent = -1;
            GridOutputSetting();
        });
    }

    /** "dd-MMM-yy hh:mm tt" */
    function dmyhmtt(v) {
        var m = /^(\d{4})-(\d{2})-(\d{2})[T ](\d{2}):(\d{2})/.exec(String(v || ''));
        if (!m) return v === null || v === undefined ? '' : String(v);
        var mon = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'][+m[2] - 1];
        var h = +m[4], tt = h < 12 ? 'AM' : 'PM', h12 = h % 12 === 0 ? 12 : h % 12;
        return m[3] + '-' + mon + '-' + m[1].slice(2) + ' ' + String(h12).padStart(2, '0') + ':' + m[5] + ' ' + tt;
    }

    /** GridOutputSetting:2645 - WagesPrint then Edit at positions 0 and 1, both frozen; Id and WagesId hidden. */
    function GridOutputSetting() {
        var t = $('grdOutputHistory');
        var h = '<tr><th class="frozen" style="left:0; min-width:70px;">WagesPrint</th><th class="frozen" style="left:70px; min-width:44px;">Edit</th>'
            + HISTORY_COLS.map(function (c) { return '<th' + (c.n ? ' class="num"' : '') + '>' + esc(c.k) + '</th>'; }).join('') + '</tr>';
        h += '<tr class="filter-row"><td class="frozen" style="left:0"></td><td class="frozen" style="left:70px"></td>'
            + HISTORY_COLS.map(function (c, i) { return '<td><input data-fcol="' + (i + 2) + '"></td>'; }).join('') + '</tr>';
        t.tHead.innerHTML = h;
        t.tBodies[0].innerHTML = S.history.map(function (r, i) {
            return '<tr data-h="' + i + '"' + (i === S.histCurrent ? ' class="is-current"' : (i % 2 ? ' class="alt"' : '')) + '>'
                + '<td class="frozen" style="left:0"><button type="button" class="cellbtn" data-wp="' + i + '">WagesPrint</button></td>'
                + '<td class="frozen" style="left:70px"><button type="button" class="cellbtn" data-ed="' + i + '">Edit</button></td>'
                + HISTORY_COLS.map(function (c) {
                    var v = r[c.k], s;
                    if (c.d) s = K.dMMMyyyy(v);
                    else if (c.t) s = dmyhmtt(v);
                    else if (c.n) s = v === null || v === undefined ? '' : cs(toD(v));
                    else s = v === null || v === undefined ? '' : String(v);
                    if (c.k === 'DocNo') return '<td><span class="doc-link" data-open="' + i + '" title="Open this document">' + esc(s) + '</span></td>';
                    return '<td' + (c.n ? ' class="num"' : '') + (c.k === 'Remarks' ? ' style="max-width:60px; overflow:hidden; text-overflow:ellipsis;"' : '') + '>' + esc(s) + '</td>';
                }).join('') + '</tr>';
        }).join('');
        applyFilter(t);
    }

    /** grdOutputHistory_DoubleClick:2709 / the Edit button (:2684) / the clickable DocNo. */
    function openHistoryRow(i) {
        var r = S.history[i];
        if (!r) return Promise.resolve();
        if (String(r.IsApproved === null ? '' : r.IsApproved) === 'Approved') throw new Error("You Can't Update Approved Record");
        var ps = String(r.PlanStatus === null || r.PlanStatus === undefined ? '' : r.PlanStatus);
        if (ps !== 'In Process') throw new Error("You Can't Update Record with " + ps + ' JobPlanStatus');
        return btnNewOutPut_Click().then(function () {
            S.RecIdOutPut = toI(r.Id);
            return ReadByIdOutPut(S.RecIdOutPut);
        });
    }

    /** grdOutputHistory "WagesPrint" -> CommonServices.ContractorWagesBill_SlipandRegister_002(WagesId). */
    function wagesPrint(i, btn) {
        var r = S.history[i];
        var id = toI(r && r.WagesId);
        if (id <= 0) throw new Error('No Record Found For Display');
        return CrystalPrint.open('wages-002', { id: id }, btn);
    }

    /** OutPutDetailByHeaderId:2925 - PackUom shows the pack's EQUIVALENT, as the desktop fills it. */
    function OutPutDetailByHeaderId() {
        var r = S.history[S.histCurrent];
        if (!r) return Promise.resolve();
        return req(API + '/' + toI(r.Id)).then(function (d) {
            var rows = (d.details || []).filter(function (x) { return col(x, 'EntryType') !== 'Issue'; });
            var cols = [
                { k: 'WareHouseName', s: 'WareHouseName' }, { k: 'ItemName', s: 'ItemName' }, { k: 'PackUom', s: 'Equivalent' },
                { k: 'CropYear', s: 'CropBatch' }, { k: 'JobLot', s: 'JobLotDescription' }, { k: 'PackingType', s: 'PackTypeDesc' },
                { k: 'Qty', s: 'Qty', f: 2, sum: 1 }, { k: 'GrossWeight', s: 'GrossWeight', f: 3, sum: 1 },
                { k: 'EbUnit', s: 'EbUnit', f: 3, sum: 1 }, { k: 'EbTotal', s: 'EbTotal', f: 3, sum: 1 },
                { k: 'NetWeight', s: 'Weight', f: 3, sum: 1 },
                { k: 'Rate', s: 'Rate', rate: 1, round: 1 }, { k: 'RateUom', s: 'RateUOM', rate: 1 },
                { k: 'Amount', s: 'Amount', f: 3, sum: 1, rate: 1 }, { k: 'Remarks', s: 'Remarks' }
            ].filter(function (c) { return !c.rate || rateRight(); });
            var t = $('grdOutPutDetail'), sums = {};
            t.tHead.innerHTML = '<tr>' + cols.map(function (c) { return '<th' + (c.f || c.round ? ' class="num"' : '') + '>' + c.k + '</th>'; }).join('') + '</tr>';
            t.tBodies[0].innerHTML = rows.map(function (x, i) {
                return '<tr' + (i % 2 ? ' class="alt"' : '') + '>' + cols.map(function (c) {
                    var v = col(x, c.s);
                    if (c.sum) sums[c.k] = (sums[c.k] || 0) + toD(v);
                    var s = c.f ? fN(v, c.f) : (c.round ? cs(roundEven(toD(v), 2)) : (v === null ? '' : String(v)));
                    return '<td' + (c.f || c.round ? ' class="num"' : '') + '>' + esc(s) + '</td>';
                }).join('') + '</tr>';
            }).join('');
            t.tFoot.innerHTML = rows.length ? '<tr>' + cols.map(function (c) { return '<td class="num">' + (c.sum ? esc(fN(sums[c.k], c.f)) : '') + '</td>'; }).join('') + '</tr>' : '';
        });
    }

    // ============================================================= prints

    /** btnPrintOutPut_Click:2775 -> CommonServices.ProductionInPutAndOutPut601(RecIdOutPut). */
    function btnPrintOutPut_Click() {
        if (S.RecIdOutPut === 0) throw new Error('No Record Found For Display');
        return CrystalPrint.open('prod-601', { id: S.RecIdOutPut }, $('btnPrintOutPut'));
    }
    /** btnVoucherOutPut_Click:2787 -> VoucherReport_118(VoucherHeadId). */
    function btnVoucherOutPut_Click() {
        if (S.VoucherHeadId === 0) throw new Error('VoucherId Not Found');
        return CrystalPrint.open('acc-118', { id: S.VoucherHeadId }, $('btnVoucherOutPut'));
    }

    /** btnPenForRateEntries_Click:2799 */
    function btnPenForRateEntries_Click() {
        return PendingRates.show('ByProduct').then(function (docId) {
            var p = Promise.resolve();
            if (docId !== 0) p = btnNewOutPut_Click().then(function () { return ReadByIdOutPut(docId); });
            return p.then(UpdateUomForLoadingPendingForRateEntriesOutPut);
        });
    }
    /** UpdateUomForLoadingPendingForRateEntriesOutPut:2820 - a row whose RateUom does not read as
        a number gets 40 written into its RateUom cell. */
    function UpdateUomForLoadingPendingForRateEntriesOutPut() {
        S.tableByProduct.forEach(function (r) { if (toI(r.RateUOM) === 0) r.RateUOM = 40; });
        renderByProduct();
    }

    /** btnApplyRate_Click:2841 / ProporationFinishGoods:2853 - the button is never visible. */
    function ProporationFinishGoods() {
        if (tx('txtFGWeight') === '' || tx('txtFGWeight') === '0') { box('Please Enter Finish Goods Weight'); return; }
        S.tableByProduct.forEach(function (r) {
            if (String(r.EntryType) === 'FinishGoods') {
                var w = toD(r.Weight), rateUom = toD(r.RateUOM), fg = S.AvgRate * rateUom;
                r.Rate = fg;
                r.ItemAmount = cs(roundEven(w / rateUom * toD(r.Rate), 2));
            }
        });
        renderByProduct();
    }
    /** txtFGWeight_TextChanged:2886 - the box is never visible. */
    function txtFGWeight_TextChanged() {
        return get('/summary-values', { jobOrderId: cv('CmbJobOrderNoOutput'), fgWeight: toD(tx('txtFGWeight').trim()) }).then(function (dt) {
            if (dt && dt.length > 0) {
                dt.forEach(function (r) {
                    if (String(col(r, 'TranType')) === 'Output Finish Goods') {
                        S.AvgRate = toD(col(r, 'AvgRate'));
                        $('txtFGRate').value = cs(roundEven(toD(col(r, 'AvgRate')), 3));
                    }
                });
            } else {
                $('txtFGRate').value = '0';
            }
        });
    }

    /** CmbPlantFeederOutput_Leave:3134 (also CmbExportContractSchedule.Leave) - Wastage IS output here. */
    function CmbPlantFeederOutput_Leave() {
        return get('/totals', { jobOrderId: cv('CmbJobOrderNoOutput'), plantId: cv('CmbPlantFeederOutput') }).then(function (dt) {
            applyTotals(dt, true);
            if (!S.dtReturnToGodown || S.dtReturnToGodown.length === 0) return getProductionInputForReturnToGodown();
        }).then(function () { return CmbEntryType_Leave(); });
    }

    /** btnDailyPlantConsumedHours_Click:3221 - new frmDailyPlantConsumedHours(UserAccount).Show(). */
    function btnDailyPlantConsumedHours_Click() {
        /* [877 wiring, round 2] .Show() = its own non-modal window, no context passed. */
        var w = window.open('/production/daily-plant-consumed-hours', '_blank');
        if (!w) box('The browser blocked the new window.');
    }

    /** BtnLoadMoveOrder_Click:3233 */
    function BtnLoadMoveOrder_Click() {
        return MoveOrders.show().then(LoadInGridDetail);
    }
    /** LoadInGridDetail:3248 */
    function LoadInGridDetail(dt) {
        if (!dt || dt.length === 0) return;
        var incoming = toI(col(dt[0], 'Id'));
        var current = cv('CmbMoveOrderTicket');
        if (current > 0 && current !== incoming
            && !yes('There is already a ticket selected. Are you sure you want to clear it and proceed with a new one?')) return;
        var ticketNo = toI(col(dt[0], 'TicketNo'));
        $('txtMoveOrderNetWeight').value = fHash(col(dt[0], 'NetWbWeight'));
        $('txtMoveOrderAlreadUsedWeight').value = fHash(col(dt[0], 'UsedWeight'));
        $('txtMoveOrderBalWeight').value = fHash(col(dt[0], 'BalWeight'));
        BindMoveOrderTicket(incoming, ticketNo);
        setCv('CmbMoveOrderTicket', incoming);
    }
    /** BindMoveOrderTicket:3266 - one row, retaining the current selection. */
    function BindMoveOrderTicket(id, name) {
        var keep = cvRaw('CmbMoveOrderTicket');
        bindCombo('CmbMoveOrderTicket', [{ Id: id, TicketNo: name }], 'Id', 'TicketNo');
        if (keep !== '' && has('CmbMoveOrderTicket', keep)) $('CmbMoveOrderTicket').value = keep;
    }
    /** BtnResetMoveOrder_Click:3275 */
    function BtnResetMoveOrder_Click() {
        clearCombo('CmbMoveOrderTicket');
        $('txtMoveOrderNetWeight').value = '';
        $('txtMoveOrderAlreadUsedWeight').value = '';
        $('txtMoveOrderBalWeight').value = '';
    }

    // ============================================================= tabs / keys

    function showView(v) {
        show('viewForm', v === 'form');
        show('viewHistory', v === 'history');
        $('tabForm').classList.toggle('is-active', v === 'form');
        $('tabHistory').classList.toggle('is-active', v === 'history');
        /* tabControl3_SelectedIndexChanged:2516 */
        if (v === 'history') $('txtFromDateOutPuthistory').focus(); else $('DocDateOutPut').focus();
    }
    function formTabActive() { return visible('viewForm'); }

    /** frmFoodProduction_KeyDown:733 */
    function onKeyDown(e) {
        if (anyModalOpen()) return;
        var k = e.key;
        if ((e.ctrlKey && (k === 'e' || k === 'E')) || k === 'Escape') {
            e.preventDefault();
            if (window.parent === window) K.close();        // Close()
            return;
        }
        if (!formTabActive()) return;
        if (e.ctrlKey && (k === 's' || k === 'S')) { e.preventDefault(); if (visible('btnSaveOutPut') && !$('btnSaveOutPut').disabled) $('btnSaveOutPut').click(); }
        else if (e.ctrlKey && (k === 'u' || k === 'U')) { e.preventDefault(); if (visible('btnUpdateOutPut') && !$('btnUpdateOutPut').disabled) $('btnUpdateOutPut').click(); }
        else if (e.ctrlKey && k === 'F5') { e.preventDefault(); $('DocDateOutPut').focus(); }
        else if (e.ctrlKey && k === 'ArrowDown') { e.preventDefault(); $('scrollByProduct').focus(); }
        else if (e.ctrlKey && k === 'Enter') { e.preventDefault(); guard(grdByProduct_DoubleClick)(); }
        else if (e.ctrlKey && (k === 'n' || k === 'N')) { e.preventDefault(); $('btnNewOutPut').click(); }
        else if (e.ctrlKey && (k === 'r' || k === 'R')) { e.preventDefault(); $('btnRefreshOutPut').click(); }
    }
    function anyModalOpen() { return !!document.querySelector('.po-modal.is-open'); }

    // ============================================================= dialogs

    function openModal(id) { $(id).classList.add('is-open'); }
    function closeModal(id) { $(id).classList.remove('is-open'); }

    /* LoadOutPutPendingforRates - ShowDialog; resolves with OutPutDocId (0 when closed). */
    var PendingRates = (function () {
        var resolveFn = null, rows = [], entryType = '', current = -1;
        function close(docId) { closeModal('dlgPendingRates'); var r = resolveFn; resolveFn = null; if (r) r(docId); }
        function show(et) {
            entryType = et;
            $('prTitle').textContent = et === 'Issue' ? 'Pending InPuts For Rates' : 'Pending OutPuts For Rates';
            $('prFromDate').value = S.fyStart || today();
            $('prToDate').value = today();
            $('prGrid').tHead.innerHTML = ''; $('prGrid').tBodies[0].innerHTML = '';
            $('prDetail').tHead.innerHTML = ''; $('prDetail').tBodies[0].innerHTML = '';
            openModal('dlgPendingRates');
            $('prFromDate').focus();
            var p = new Promise(function (res) { resolveFn = res; });
            /* LoadInvoices_Load: JobOrderFill then OutputGridHistory. */
            get('/pending-rates/job-orders').then(function (r) {
                if (r && r.length > 0) bindCombo('prJobOrder', r, 'Id', 'PlanCode');
            }).then(search).catch(function (e) { box(e.message); });
            return p;
        }
        /** OutputGridHistory - grouped by Id: Qty summed, Weight AVERAGED (as the desktop does). */
        function search() {
            return get('/pending-rates', { fromDate: $('prFromDate').value, toDate: $('prToDate').value,
                                           jobOrderId: cv('prJobOrder'), entryType: entryType }).then(function (dt) {
                if (!dt || dt.length <= 0) return;
                var groups = [], byId = {};
                dt.forEach(function (r) {
                    var id = String(col(r, 'Id'));
                    if (!byId[id]) { byId[id] = { first: r, rows: [] }; groups.push(byId[id]); }
                    byId[id].rows.push(r);
                });
                rows = groups.map(function (g) {
                    var f = g.first, q = 0, w = 0;
                    g.rows.forEach(function (r) { q += toD(col(r, 'Qty')); w += toD(col(r, 'Weight')); });
                    return { Id: col(f, 'Id'), JobOrderNo: col(f, 'JobOrderNo'), DocDate: dateOnly(col(f, 'DocDate')),
                             DocCode: col(f, 'DocCode'), MainRemarks: col(f, 'MainRemarks'), TotalQty: q,
                             TotalWeight: w / g.rows.length, EntryDate: dateOnly(col(f, 'EntryDate')),
                             EntryUser: col(f, 'EntryUser'), ModifyDate: dateOnly(col(f, 'ModifyDate')), ModifyUser: col(f, 'ModifyUser') };
                });
                current = -1;
                var cols = ['JobOrderNo', 'DocDate', 'DocCode', 'MainRemarks', 'TotalQty', 'TotalWeight', 'EntryDate', 'EntryUser', 'ModifyDate', 'ModifyUser'];
                $('prGrid').tHead.innerHTML = '<tr><th class="frozen" style="left:0">Load</th>' + cols.map(function (c) { return '<th>' + c + '</th>'; }).join('') + '</tr>';
                $('prGrid').tBodies[0].innerHTML = rows.map(function (r, i) {
                    return '<tr data-pr="' + i + '"' + (i % 2 ? ' class="alt"' : '') + '><td class="frozen" style="left:0"><button type="button" class="cellbtn" data-load="' + i + '">Load</button></td>'
                        + cols.map(function (c) {
                            var v = r[c];
                            if (c === 'DocDate' || c === 'EntryDate' || c === 'ModifyDate') v = K.dMMMyyyy(v);
                            else if (c === 'TotalQty' || c === 'TotalWeight') v = cs(v);
                            return '<td' + (c === 'TotalQty' || c === 'TotalWeight' ? ' class="num"' : '') + '>' + esc(v) + '</td>';
                        }).join('') + '</tr>';
                }).join('');
            });
        }
        /** grd_SelectionChanged - re-queries WITHOUT the entry type, then keeps the row's lines. */
        function selectRow(i) {
            current = i;
            Array.prototype.forEach.call($('prGrid').tBodies[0].rows, function (tr) { tr.classList.toggle('is-current', +tr.getAttribute('data-pr') === i); });
            var r = rows[i];
            return get('/pending-rates', { fromDate: $('prFromDate').value, toDate: $('prToDate').value, jobOrderId: cv('prJobOrder') }).then(function (dt) {
                if (!dt || dt.length <= 0) return;
                var cols = [['EntryType', 'EntryType'], ['ItemName', 'ItemName'], ['Cropyear', 'CropBatch'], ['WareHouseName', 'WareHouseName'],
                            ['JobLotDescription', 'JobLotDescription'], ['PackType', 'PackTypeDesc'], ['PackUom', 'PackUom'], ['Qty', 'Qty'], ['Weight', 'Weight']];
                $('prDetail').tHead.innerHTML = '<tr>' + cols.map(function (c) { return '<th>' + c[0] + '</th>'; }).join('') + '</tr>';
                $('prDetail').tBodies[0].innerHTML = dt.filter(function (x) { return toI(col(x, 'Id')) === toI(r.Id); }).map(function (x, j) {
                    return '<tr' + (j % 2 ? ' class="alt"' : '') + '>' + cols.map(function (c) {
                        var v = col(x, c[1]);
                        return '<td' + (c[0] === 'Qty' || c[0] === 'Weight' ? ' class="num"' : '') + '>' + esc(c[0] === 'Qty' || c[0] === 'Weight' ? cs(toD(v)) : (v === null ? '' : v)) + '</td>';
                    }).join('') + '</tr>';
                }).join('');
            });
        }
        /** btnReset_Click - searches with the current filters FIRST, then resets and empties both grids. */
        function reset() {
            return search().then(function () {
                $('prFromDate').value = S.fyStart || today();
                $('prFromDate').focus();
                $('prJobOrder').value = EMPTY;
                $('prGrid').tHead.innerHTML = ''; $('prGrid').tBodies[0].innerHTML = '';
                $('prDetail').tHead.innerHTML = ''; $('prDetail').tBodies[0].innerHTML = '';
                rows = [];
            });
        }
        function wire() {
            $('prSearch').addEventListener('click', function () { busy('prSearch', search); });
            $('prReset').addEventListener('click', function () { busy('prReset', reset); });
            $('prGrid').addEventListener('click', function (e) {
                var b = e.target.closest('[data-load]');
                if (b) { close(toI(rows[+b.getAttribute('data-load')].Id)); return; }
                var tr = e.target.closest('tr[data-pr]');
                if (tr) guard(selectRow)(+tr.getAttribute('data-pr'));
            });
        }
        return { show: show, close: close, wire: wire };
    }());

    /* frmPendingMoveOrderDocuments - ShowDialog; resolves with dtLoader (rows) or [] when closed. */
    var MoveOrders = (function () {
        var resolveFn = null, data = [], checked = {};
        function close(result) { closeModal('dlgMoveOrder'); var r = resolveFn; resolveFn = null; if (r) r(result || []); }
        function show() {
            /* InitializeComponentCustom: FromDate = Now - 7 (ToDate = today); then the async load. */
            $('moFromDate').value = addDays(today(), -7);
            $('moToDate').value = today();
            $('moDocNoFrom').value = ''; $('moDocNoTo').value = ''; $('moVehicleNo').value = ''; $('moBiltyNo').value = '';
            data = []; checked = {};
            render();
            openModal('dlgMoveOrder');
            $('moFromDate').focus();
            var p = new Promise(function (res) { resolveFn = res; });
            load().catch(function (e) { box('Error occurred during database call: ' + e.message); });
            return p;
        }
        /** PendingDataDbCall + GrdDataBind */
        function load() {
            $('moMsg').textContent = 'Loading...';
            return get('/move-orders', {
                fromDate: $('moFromDate').value, toDate: $('moToDate').value, docFrom: $('moDocNoFrom').value,
                docTo: $('moDocNoTo').value, vehicleNo: $('moVehicleNo').value, biltyNo: $('moBiltyNo').value
            }).then(function (r) { data = r || []; checked = {}; render(); });
        }
        var COLS = [['WbDocumentType'], ['DocDate', 'd'], ['TicketNo'], ['WorkingReportNo'], ['VehicleNo'], ['ItemQty', 'n'],
                    ['FirstWeight', 'n'], ['SecondWeight', 'n'], ['NetWbWeight', 'n'], ['UsedWeight', 'n'], ['BalWeight', 'n'],
                    ['ItemDescription'], ['WbRemarks'], ['Status'], ['EntryDate', 'd'], ['EntryUserName'], ['ModifyDate', 'd'], ['ModifyUserName']];
        function render() {
            var t = $('moGrid');
            $('moMsg').textContent = data.length + ' record(s)';
            if (!data.length) { t.tHead.innerHTML = ''; t.tBodies[0].innerHTML = ''; t.tFoot.innerHTML = ''; return; }
            t.tHead.innerHTML = '<tr><th class="frozen" style="left:0">Select</th>' + COLS.map(function (c) { return '<th' + (c[1] === 'n' ? ' class="num"' : '') + '>' + c[0] + '</th>'; }).join('') + '</tr>';
            var sums = {};
            t.tBodies[0].innerHTML = data.map(function (r, i) {
                return '<tr' + (i % 2 ? ' class="alt"' : '') + '><td class="frozen" style="left:0"><input type="checkbox" data-mo="' + i + '"' + (checked[i] ? ' checked' : '') + '></td>'
                    + COLS.map(function (c) {
                        var v = col(r, c[0]);
                        if (c[1] === 'n') { sums[c[0]] = (sums[c[0]] || 0) + toD(v); v = cs(toD(v)); }
                        else if (c[1] === 'd') v = K.dMMMyy(v);
                        return '<td' + (c[1] === 'n' ? ' class="num"' : '') + '>' + esc(v === null ? '' : v) + '</td>';
                    }).join('') + '</tr>';
            }).join('');
            t.tFoot.innerHTML = '<tr><td></td>' + COLS.map(function (c) { return '<td class="num">' + (c[1] === 'n' ? esc(cs(sums[c[0]] || 0)) : '') + '</td>'; }).join('') + '</tr>';
        }
        /** btnLoadOnInvoice_Click_1 */
        function loadSelected() {
            var idx = Object.keys(checked).filter(function (k) { return checked[k]; }).map(Number).sort(function (a, b) { return a - b; });
            if (idx.length === 0) { box('Check the row first'); return; }
            var first = toI(col(data[idx[0]], 'Id')), ids = [];
            for (var n = 0; n < idx.length; n++) {
                var r = data[idx[n]];
                if (toI(col(r, 'Id')) !== first) { box('Sorry! You can Only Check rows which have the same DocNo'); return; }
                if (String(col(r, 'Status') === null ? '' : col(r, 'Status')) !== 'Accepted') { box('Sorry! You can Only Load Accepted Record'); return; }
                ids.push(toI(col(r, 'Id')));
            }
            var loader = data.filter(function (r) { return ids.indexOf(toI(col(r, 'Id'))) >= 0; });
            if (ids.length > 0 && loader.length > 0) close(loader);
        }
        /** btnReset_Click */
        function reset() {
            $('moFromDate').focus();
            $('moFromDate').value = S.fyStart || $('moFromDate').value;
            $('moDocNoFrom').value = ''; $('moDocNoTo').value = ''; $('moVehicleNo').value = ''; $('moBiltyNo').value = '';
            return load();
        }
        function shortcuts() {
            var rows = [['Ctrl+E', 'For Close'], ['Ctrl+N', 'For New'], ['Ctrl+R', 'For Refresh'], ['Ctrl+L', 'To Press Load Button'],
                        ['Ctrl+S', 'For Search'], ['Ctrl+alt', 'To Show ShortCut Keys Form'],
                        ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]];
            $('shortcutsBody').innerHTML = '<table><tr><th>KeyCombination</th><th>Description</th></tr>'
                + rows.map(function (r) { return '<tr><td>' + esc(r[0]) + '</td><td>' + esc(r[1]) + '</td></tr>'; }).join('') + '</table>';
            openModal('dlgShortcuts');
        }
        function onKey(e) {
            if (!$('dlgMoveOrder').classList.contains('is-open') || $('dlgShortcuts').classList.contains('is-open')) return;
            var k = e.key;
            if (k === 'Escape' || (e.ctrlKey && (k === 'e' || k === 'E'))) { e.preventDefault(); close([]); }
            else if (e.ctrlKey && e.altKey) { e.preventDefault(); shortcuts(); }
            else if (e.ctrlKey && (k === 's' || k === 'S')) { e.preventDefault(); busy('moShow', load); }
            else if (e.ctrlKey && (k === 'l' || k === 'L')) { e.preventDefault(); loadSelected(); }
            else if (e.ctrlKey && (k === 'n' || k === 'N')) { e.preventDefault(); busy('moNew', reset); }
            else if (e.ctrlKey && (k === 'r' || k === 'R')) { e.preventDefault(); /* btnRefresh_Click is empty */ }
        }
        function wire() {
            $('moShow').addEventListener('click', function () { busy('moShow', load); });
            $('moNew').addEventListener('click', function () { busy('moNew', reset); });
            $('moRefresh').addEventListener('click', function () { /* btnRefresh_Click is an empty method */ });
            $('moLoad').addEventListener('click', loadSelected);
            $('moShortcuts').addEventListener('click', shortcuts);
            $('moGrid').addEventListener('change', function (e) {
                var c = e.target.closest('input[data-mo]');
                if (c) checked[+c.getAttribute('data-mo')] = c.checked;
            });
            document.addEventListener('keydown', onKey, true);
        }
        return { show: show, close: close, wire: wire };
    }());

    // ============================================================= wiring

    function on(id, ev, fn) { $(id).addEventListener(ev, fn); }
    function clickBusy(id, fn) { on(id, 'click', function () { busy(id, fn); }); }

    function wire() {
        clickBusy('btnNewOutPut', btnNewOutPut_Click);
        clickBusy('btnRefreshOutPut', btnRefreshOutPut_Click);
        clickBusy('btnSaveOutPut', btnSaveOutPut_Click);
        clickBusy('btnUpdateOutPut', btnUpdateOutPut_Click);
        on('btnPrintOutPut', 'click', guard(btnPrintOutPut_Click));
        on('btnVoucherOutPut', 'click', guard(btnVoucherOutPut_Click));
        clickBusy('btnPenForRateEntries', btnPenForRateEntries_Click);
        on('btnDailyPlantConsumedHours', 'click', btnDailyPlantConsumedHours_Click);
        clickBusy('BtnLoadMoveOrder', BtnLoadMoveOrder_Click);
        on('BtnResetMoveOrder', 'click', guard(BtnResetMoveOrder_Click));
        on('btnAddOutPut', 'click', guard(btnAddOutPut_Click));
        on('btnUpdateDetailOutPut', 'click', guard(btnUpdateDetailOutPut_Click));
        on('btnCancelOutPut', 'click', guard(btnCancelOutPut_Click));
        on('btnApplyRate', 'click', guard(ProporationFinishGoods));

        /* Leave events */
        on('CmbJobOrderNoOutput', 'change', guard(CmbJobOrderNoOutput_Leave));
        on('CmbPlantFeederOutput', 'change', guard(CmbPlantFeederOutput_Leave));
        on('CmbExportContractSchedule', 'change', guard(CmbPlantFeederOutput_Leave));   // designer :579
        on('CmbEntryType', 'change', guard(CmbEntryType_Leave));
        on('CmbItemOutput', 'change', guard(CmbItemOutput_Leave));
        on('CmbUomOutPut', 'change', guard(CmbUomOutPut_Leave));
        on('CmbCropYearOutPut', 'change', guard(GetLastItemRateByJobOrder));             // CmbCropYearOutPut_Leave
        on('CmbRateUomOutPut', 'change', guard(TotalAmount));                            // CmbRateUomOutPut_Leave

        /* TextChanged (typing) and Leave on the text boxes */
        ['txtQtyOutPut', 'txtRateOutPut', 'txtGrossWeight', 'txtEbUnit', 'txtEbTotal', 'txtNetWeightOutPut', 'txtFGWeight'].forEach(function (id) {
            on(id, 'input', function () { try { ON_TEXT[id](); } catch (e) { box(e.message); } });
        });
        on('txtQtyOutPut', 'blur', function () { try { Total(); TotalAmount(); } catch (e) { box(e.message); } });   // txtQtyOutPut_Leave
        on('txtNetWeightOutPut', 'blur', function () { try { TotalAmount(); } catch (e) { box(e.message); } });

        /* grid */
        var g = $('grdByProduct');
        g.addEventListener('click', function (e) {
            var d = e.target.closest('[data-del]');
            if (d) { guard(deleteRow)(+d.getAttribute('data-del')); return; }
            var tr = e.target.closest('tbody tr[data-i]');
            if (tr) { S.currentRow = +tr.getAttribute('data-i'); markCurrent(g, 'data-i', S.currentRow); }
        });
        g.addEventListener('dblclick', function (e) {
            var tr = e.target.closest('tbody tr[data-i]');
            if (tr) { S.currentRow = +tr.getAttribute('data-i'); guard(grdByProduct_DoubleClick)(); }
        });
        g.addEventListener('input', function (e) { if (e.target.matches('input[data-fcol]')) applyFilter(g); });
        $('scrollByProduct').addEventListener('keydown', function (e) {
            if (e.key === 'ArrowDown' || e.key === 'ArrowUp') {
                e.preventDefault();
                var n = S.currentRow + (e.key === 'ArrowDown' ? 1 : -1);
                if (n >= 0 && n < S.tableByProduct.length) { S.currentRow = n; markCurrent(g, 'data-i', n); }
            }
        });
        on('gbOutputFull', 'click', function () { $('boxByProduct').classList.toggle('fullscreen'); });

        /* History */
        on('tabForm', 'click', function () { showView('form'); });
        on('tabHistory', 'click', function () { showView('history'); });
        on('BtnNewOutPuthistory', 'click', guard(BtnNewOutPuthistory_Click));
        clickBusy('BtnRefereshOutPutHistory', BtnRefereshOutPutHistory_Click);
        clickBusy('BtnShowOutPuthistory', OutputGridHistory);
        var hg = $('grdOutputHistory');
        hg.addEventListener('click', function (e) {
            var wp = e.target.closest('[data-wp]');
            if (wp) { S.histCurrent = +wp.getAttribute('data-wp'); guard(wagesPrint)(S.histCurrent, wp); return; }
            var ed = e.target.closest('[data-ed]');
            if (ed) { S.histCurrent = +ed.getAttribute('data-ed'); busy(ed, function () { return openHistoryRow(S.histCurrent); }); return; }
            var op = e.target.closest('[data-open]');
            if (op) { S.histCurrent = +op.getAttribute('data-open'); guard(openHistoryRow)(S.histCurrent); return; }
            var tr = e.target.closest('tbody tr[data-h]');
            if (tr) {
                var i = +tr.getAttribute('data-h');
                if (i !== S.histCurrent) { S.histCurrent = i; markCurrent(hg, 'data-h', i); guard(OutPutDetailByHeaderId)(); }
            }
        });
        hg.addEventListener('dblclick', function (e) {
            var tr = e.target.closest('tbody tr[data-h]');
            if (tr && !e.target.closest('button')) { S.histCurrent = +tr.getAttribute('data-h'); guard(openHistoryRow)(S.histCurrent); }
        });
        hg.addEventListener('input', function (e) { if (e.target.matches('input[data-fcol]')) applyFilter(hg); });
        on('gbHistFull', 'click', function () { $('boxHistory').classList.toggle('fullscreen'); });

        /* dialogs */
        document.querySelectorAll('[data-close]').forEach(function (b) {
            b.addEventListener('click', function () {
                var id = b.getAttribute('data-close');
                if (id === 'dlgPendingRates') PendingRates.close(0);          // btnclose_Click: OutPutDocId = 0
                else if (id === 'dlgMoveOrder') MoveOrders.close([]);
                else closeModal(id);
            });
        });
        PendingRates.wire();
        MoveOrders.wire();

        document.addEventListener('keydown', onKeyDown);
        if (K && K.enterToTab) K.enterToTab();
        /* A WinForms TextBox entered by Tab/Enter selects its whole text, so typing replaces it. */
        document.addEventListener('focusin', function (e) {
            var t = e.target;
            if (t && t.tagName === 'INPUT' && t.type === 'text' && t.classList.contains('ctl') && !t.readOnly) {
                setTimeout(function () { try { t.select(); } catch (x) { /* ignore */ } }, 0);
            }
        });
    }

    function markCurrent(table, attr, i) {
        Array.prototype.forEach.call(table.tBodies[0].rows, function (tr) { tr.classList.toggle('is-current', +tr.getAttribute(attr) === i); });
    }

    /* Drop-grid column sets for this form (DDL.BindDDL shows every column after the value column,
       minus the ones the form hides). */
    function defineFamilies() {
        if (!window.DesktopCombo || !window.DesktopCombo.define) return;
        /* JobOrderNoFill:598 - cols 2 (WorkInProcessAcId), 4 (WipItemId) and DocumentTypeId hidden. */
        window.DesktopCombo.define('p280JobOrder', [
            { caption: 'Job Order No', flex: 2 },
            { caption: 'WorkInProcessAc', flex: 3, key: 'wipac' },
            { caption: 'ItemName', flex: 3, key: 'wipitem' },
            { caption: 'FinishGoodRate', flex: 1, key: 'fgrate', type: 'num' },
            { caption: 'WipWareHouseId', flex: 1, key: 'wipwh', type: 'num' },
            { caption: 'WareHouseName', flex: 2, key: 'whname' }
        ]);
        /* ExportContractScheduleBind:1111 - AllColumns, cols 2 (ExImLcOrderId) and 4 (ExImInvoiceId) hidden. */
        window.DesktopCombo.define('p280Contract', [
            { caption: 'Contract Schedule', flex: 2 },
            { caption: 'ContractNo', flex: 2, key: 'contract' },
            { caption: 'InvoiceNo', flex: 2, key: 'invoice' }
        ]);
    }

    document.addEventListener('DOMContentLoaded', function () {
        defineFamilies();
        wire();
        frmFoodProduction_Load();
    });
}());
