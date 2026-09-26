/* ============================================================================================
 * "Production Output Allocation With Export Invoice".
 * Desktop: Architecture.WinApp.Production/ProductionOutputAllocationWithExportInvoice.cs (1,724 lines).
 * Opened from screen 280's Settlement tab (frmProductionSettlement.btnAllocateExportInvoiceToJobOrder_Click
 * :2206, `new ProductionOutputAllocationWithExportInvoice(UserAccount).Show()` - its own window).
 *
 * Line numbers below are ProductionOutputAllocationWithExportInvoice.cs's. The grid arithmetic
 * (grd_CellUpdated, the "+" split, the "X" delete) runs here as it runs in the desktop grid;
 * every database step and every refusal Insert() makes runs on the server.
 * ============================================================================================ */
(function () {
    'use strict';

    var API = '/api/production/output-allocation-export-invoice';
    var K = window.ReportKit;

    /* ---------------------------------------------------------------- form fields */
    var RecId = 0;
    var rights = { save: false, update: false };
    var dtdetail = [];          /* Id, ItemId, ItemName, OutPutQty, OutPutWeight, Qty, NetWeightKg, ExportInvoiceId */
    var grdBound = false;       /* grd.DataSource (dtdetail) vs null */
    var dtInvoice = [];
    var lstRemoveRecord = [];
    var historyRows = null;     /* DataGridHistory.DataSource */
    var cur = -1, hcur = -1;
    var histTime = timePart(nowIso());

    function $id(id) { return document.getElementById(id); }
    function esc(s) {
        return String(s === null || s === undefined ? '' : s).replace(/&/g, '&amp;').replace(/</g, '&lt;')
            .replace(/>/g, '&gt;').replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    }
    function say(m) { $id('lblStatus').textContent = m || ''; }
    function pad(n) { return (n < 10 ? '0' : '') + n; }
    function nowIso() {
        var d = new Date();
        return d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()) + 'T'
            + pad(d.getHours()) + ':' + pad(d.getMinutes()) + ':' + pad(d.getSeconds());
    }
    function timePart(iso) { return iso && iso.length >= 19 ? iso.substring(10, 19) : 'T00:00:00'; }
    function col(row, name) {
        if (!row) return null;
        if (Object.prototype.hasOwnProperty.call(row, name)) return row[name];
        var l = name.toLowerCase();
        for (var k in row) if (Object.prototype.hasOwnProperty.call(row, k) && k.toLowerCase() === l) return row[k];
        return null;
    }
    function toD(v) {
        if (v === null || v === undefined || v === '') return 0;
        if (typeof v === 'number') return v;
        var n = Number(String(v).trim().replace(/,/g, ''));
        return isNaN(n) ? 0 : n;
    }
    function toI(v) {
        if (v === null || v === undefined || v === '') return 0;
        if (typeof v === 'number') return isFinite(v) ? Math.round(v) : 0;
        var s = String(v).trim();
        return /^[+-]?\d+$/.test(s) ? parseInt(s, 10) : 0;
    }
    function str15(x) {
        if (x === null || x === undefined || x === '') return '';
        var n = typeof x === 'number' ? x : Number(x);
        if (isNaN(n)) return 'NaN';
        if (!isFinite(n)) return n > 0 ? 'Infinity' : '-Infinity';
        return String(Number(n.toPrecision(15)));
    }
    function cssEsc(v) { return window.CSS && CSS.escape ? CSS.escape(v) : String(v).replace(/"/g, '\\"'); }

    /* ------------------------------------------------------------------ MessageBox */
    var ICON = { error: 'fa-times-circle" style="color:#c00' };
    function dialog(text, caption, icon, buttons) {
        return new Promise(function (resolve) {
            var m = document.createElement('div');
            m.className = 'pa-modal pa-msg';
            m.innerHTML = '<div class="pa-dlg" role="dialog"><div class="cap"><span>' + esc(caption || '') + '</span></div>'
                + '<div class="msg">' + (icon ? '<i class="fa ' + ICON[icon] + '"></i>' : '') + '<div>' + esc(text) + '</div></div>'
                + '<div class="btns">' + buttons.map(function (b, i) {
                    return '<button type="button" data-i="' + i + '">' + esc(b) + '</button>';
                }).join('') + '</div></div>';
            document.body.appendChild(m);
            var first = m.querySelector('button');
            if (first) first.focus();
            function done(i) { document.removeEventListener('keydown', key, true); m.remove(); resolve(i); }
            function key(e) {
                if (e.key === 'Escape') { e.preventDefault(); e.stopPropagation(); done(buttons.length === 1 ? 0 : 1); }
            }
            document.addEventListener('keydown', key, true);
            m.addEventListener('click', function (e) {
                var b = e.target.closest('button[data-i]');
                if (b) done(parseInt(b.getAttribute('data-i'), 10));
            });
        });
    }
    function msg(text, caption, icon) { return dialog(text, caption, icon, ['OK']); }
    function ask(text, caption) { return dialog(text, caption, null, ['Yes', 'No']).then(function (i) { return i === 0; }); }
    function modalOpen() { return !!document.querySelector('.pa-msg, .pa-modal:not(.is-hidden), .rk-modal.is-open'); }

    /* ------------------------------------------------------------------ requests */
    function headers(h) {
        var t = document.querySelector('meta[name="_csrf"]'), hn = document.querySelector('meta[name="_csrf_header"]');
        if (t && hn && t.getAttribute('content')) h[hn.getAttribute('content')] = t.getAttribute('content');
        return h;
    }
    function handle(r) {
        return r.text().then(function (t) {
            var body = null;
            try { body = t ? JSON.parse(t) : null; } catch (e) { body = null; }
            if (!r.ok) throw new Error(body && body.message ? body.message : (t || ('Request failed (' + r.status + ')')));
            return body;
        });
    }
    function getJson(url) {
        return fetch(url, { credentials: 'same-origin', headers: { 'Accept': 'application/json' } }).then(handle);
    }
    function postJson(url, body) {
        return fetch(url, { method: 'POST', credentials: 'same-origin',
            headers: headers({ 'Content-Type': 'application/json', 'Accept': 'application/json' }),
            body: JSON.stringify(body) }).then(handle);
    }
    function qs(o) {
        return Object.keys(o).filter(function (k) { return o[k] !== null && o[k] !== undefined && o[k] !== ''; })
            .map(function (k) { return encodeURIComponent(k) + '=' + encodeURIComponent(o[k]); }).join('&');
    }
    function busy(btn, fn) {
        var b = typeof btn === 'string' ? $id(btn) : btn;
        if (b && b.classList.contains('is-busy')) return Promise.resolve();
        var was = b ? b.disabled : false;
        if (b) { b.classList.add('is-busy'); b.disabled = true; }
        var done = function () { if (b) { b.classList.remove('is-busy'); b.disabled = was; } };
        var p;
        try { p = Promise.resolve(fn()); } catch (e) { p = Promise.reject(e); }
        return p.then(function (v) { done(); return v; }, function (e) { done(); throw e; });
    }

    /* ------------------------------------------------------------------ combos */
    if (window.DesktopCombo) {
        /* DropDownBind.BindDDL(dt, CmbJobOrder, "Id", "PlanCode", "Job Order No"): Id hidden, every
           other column GetJobOrderNoAll returns stays visible (same list the Settlement tab binds). */
        window.DesktopCombo.define('poaJobOrder', [
            { caption: 'Job Order No',   flex: 3 },
            { caption: 'ApprovalStatus', flex: 2, key: 'approval' },
            { caption: 'SettledStatus',  flex: 2, key: 'settled' },
            { caption: 'SettlementDate', flex: 3, key: 'sdate' },
            { caption: 'JobDocNo',       flex: 1, key: 'docno', type: 'num' }
        ]);
    }
    function fillJobOrders(rows) {
        var sel = $id('CmbJobOrder');
        var h = '<option value=""></option>';
        (rows || []).forEach(function (r) {
            var sd = col(r, 'SettlementDate');
            h += '<option value="' + esc(col(r, 'Id')) + '" data-approval="' + esc(col(r, 'ApprovalStatus'))
                + '" data-settled="' + esc(col(r, 'SettledStatus')) + '" data-sdate="' + esc(sd ? K.dmyhm(sd) : '')
                + '" data-docno="' + esc(col(r, 'JobDocNo')) + '">' + esc(col(r, 'PlanCode')) + '</option>';
        });
        sel.innerHTML = h;
        sel.value = '';
    }
    /** combo.Value = v: selects the row, or holds v without an ActiveRow (UltraCombo). */
    function setValue(sel, v) {
        var s = String(v === null || v === undefined ? '' : v);
        Array.prototype.forEach.call(sel.querySelectorAll('option[data-orphan]'), function (o) { o.remove(); });
        if (s === '' || s === '0') { sel.value = ''; return; }
        if (!sel.querySelector('option[value="' + cssEsc(s) + '"]')) {
            var o = document.createElement('option');
            o.value = s; o.textContent = s; o.setAttribute('data-orphan', '1');
            sel.appendChild(o);
        }
        sel.value = s;
    }
    function activeRow(sel) {
        var o = sel.options[sel.selectedIndex];
        return o && o.value !== '' && !o.hasAttribute('data-orphan') ? o : null;
    }
    function setComboEnabled(sel, on) {
        sel.disabled = !on;
        var w = sel.closest('.dtcombo-wrap');
        if (w) Array.prototype.forEach.call(w.querySelectorAll('input, button'), function (x) { x.disabled = !on; });
    }
    function focusCombo(sel) {
        var w = sel.closest('.dtcombo-wrap');
        var inp = w ? w.querySelector('input') : null;
        (inp || sel).focus();
    }
    function onLeave(sel, fn) {
        setTimeout(function () {
            var wrap = sel.closest('.dtcombo-wrap') || sel;
            wrap.addEventListener('focusout', function () {
                setTimeout(function () {
                    var a = document.activeElement;
                    if (a && (wrap.contains(a) || (a.closest && a.closest('.dtcombo-pop')))) return;
                    fn();
                }, 0);
            });
        }, 0);
    }

    /* ============================================================ InitializeComponentMethod:158 */
    function load() {
        return getJson(API + '/load').then(function (d) {
            rights = d.rights || rights;
            /* :181 btnsave.Enabled = DoHaveSaveRight; btnUpdate.Enabled = DoHaveUpdateRights. */
            $id('btnsave').disabled = !rights.save;
            $id('btnUpdate').disabled = !rights.update;
            /* :185-190 ctrlGrdBar1 (grd) items from the rights object. */
            if (window.GridBar) {
                window.GridBar.attach($id('grd'), null, {
                    canChooseFields: rights.fieldChooser !== false, canSaveLayout: rights.saveLayout !== false,
                    canPrint: !!rights.gridPrint, canExport: !!rights.gridExport,
                    canGroupCollapse: rights.groupCollapse !== false, canGroupExpand: rights.groupExpand !== false
                });
            }
            if ((d.jobOrders || []).length > 0) fillJobOrders(d.jobOrders);
            else { $id('CmbJobOrder').innerHTML = '<option value=""></option>'; }
            bindHistoryCombo(d.historyJobOrders);
            dtInvoice = d.invoices || [];
            /* dtdetail has its columns: the grid is bound (empty) and set up. */
            grdBound = true;
            renderGrid();
        }).catch(function () { return msg('Error occurred during database call.'); });
    }
    function bindHistoryCombo(rows) {
        var sel = $id('CmbJobOrderNoHistory');
        var h = '<option value=""></option>';
        (rows || []).forEach(function (r) {
            h += '<option value="' + esc(col(r, 'Id')) + '">' + esc(col(r, 'ReferenceName')) + '</option>';
        });
        sel.innerHTML = h;
        sel.value = '';
    }

    /* ======================================================================== the form grid */
    /* grdSettings:419 - visible: X (pos 0), + (pos 1), ItemName (250), Qty (70, "#,##0.00", Sum),
       NetWeightKg (100, "#,##0.000", Sum), ExportInvoiceId ("Export Invoice", 150, value list).
       Id, ItemId, OutPutQty, OutPutWeight hidden. FrozenColumns = 2. TotalRow on. */
    function fmtQty(v) { return K.fixed(toD(v), 2); }
    function fmtWt(v) { return K.fixed(toD(v), 3); }
    function invoiceOptions(selected) {
        var s = String(selected === null || selected === undefined ? '' : selected);
        var h = '<option value=""></option>', found = false;
        dtInvoice.forEach(function (r) {
            var id = String(col(r, 'Id'));
            if (id === s) found = true;
            h += '<option value="' + esc(id) + '"' + (id === s ? ' selected' : '') + '>' + esc(col(r, 'InvoiceNo')) + '</option>';
        });
        if (s && s !== '0' && !found) h += '<option value="' + esc(s) + '" selected>' + esc(s) + '</option>';
        return h;
    }
    function renderGrid() {
        var t = $id('grd');
        if (!grdBound) { t.innerHTML = ''; return; }
        var h = '<colgroup><col style="width:22px"><col style="width:22px"><col style="width:250px"><col style="width:70px">'
            + '<col style="width:100px"><col style="width:150px"></colgroup>'
            + '<thead><tr><th class="frz" style="left:0"></th><th class="frz" style="left:22px"></th>'
            + '<th>ItemName</th><th>Qty</th><th>NetWeightKg</th><th>Export Invoice</th></tr></thead><tbody>';
        var sq = 0, sw = 0;
        dtdetail.forEach(function (r, i) {
            sq += toD(r.Qty); sw += toD(r.NetWeightKg);
            h += '<tr data-i="' + i + '"' + (i === cur ? ' class="cur"' : '') + '>'
                + '<td class="frz" style="left:0"><button type="button" class="cb" data-btn="Delete">X</button></td>'
                + '<td class="frz" style="left:22px"><button type="button" class="cb" data-btn="Add">+</button></td>'
                + '<td title="' + esc(r.ItemName) + '">' + esc(r.ItemName) + '</td>'
                + '<td class="num"><input class="edit" data-col="Qty" data-guard="decimal" value="' + esc(fmtQty(r.Qty)) + '"></td>'
                + '<td class="num"><input class="edit" data-col="NetWeightKg" data-guard="decimal" value="' + esc(fmtWt(r.NetWeightKg)) + '"></td>'
                + '<td><select class="edit" data-col="ExportInvoiceId">' + invoiceOptions(r.ExportInvoiceId) + '</select></td></tr>';
        });
        h += '</tbody><tfoot><tr><td class="frz" style="left:0"></td><td class="frz" style="left:22px"></td><td></td>'
            + '<td class="num">' + esc(fmtQty(sq)) + '</td><td class="num">' + esc(fmtWt(sw)) + '</td><td></td></tr></tfoot>';
        t.innerHTML = h;
    }
    function sumBy(itemId, key) {
        var s = 0;
        dtdetail.forEach(function (r) { if (String(r.ItemId) === String(itemId)) s += toD(r[key]); });
        return s;
    }

    /** grd_CellUpdated:475 - literally, including Rem being computed with this row's own value. */
    function grd_CellUpdated(i, key) {
        var r = dtdetail[i];
        if (!r) return;
        var OutPutQty = toD(r.OutPutQty), OutPutWeight = toD(r.OutPutWeight);
        var uom = OutPutWeight / OutPutQty;
        var Qty = toD(r.Qty), NetWeightKg = toD(r.NetWeightKg);
        var SumOfQty = sumBy(r.ItemId, 'Qty'), SumOfWeight = sumBy(r.ItemId, 'NetWeightKg');
        if (SumOfQty > OutPutQty || SumOfWeight > OutPutWeight) {
            var Rem = OutPutQty - SumOfQty;
            Qty = Rem < 0 ? 0 : Rem;
            r.Qty = Qty;
            r.NetWeightKg = 0;
        }
        if (key === 'Qty') r.NetWeightKg = Qty * uom;
        else if (key === 'NetWeightKg') r.Qty = NetWeightKg / uom;
    }

    /** grd_ColumnButtonClick:670 - "X" and "+". */
    function columnButton(i, key) {
        var r = dtdetail[i];
        if (!r) return;
        if (key === 'Delete') {
            if (dtdetail.length > 1) DeleteRow(i);
            renderGrid();
            return;
        }
        if (key !== 'Add') return;
        var OutputQty = toD(r.OutPutQty), num = toD(r.OutPutWeight);
        var Rem = OutputQty - sumBy(r.ItemId, 'Qty');
        var RemWeight = num - sumBy(r.ItemId, 'NetWeightKg');
        if (Rem > 0.0 && RemWeight > 0.0) {
            var dr = Object.assign({}, r);                           /* dtdetail.Rows.Add(dr.ItemArray) */
            var upd = $id('btnUpdate');
            if (!upd.classList.contains('is-hidden') && !upd.disabled) dr.Id = 0;
            dr.Qty = Rem;
            dr.NetWeightKg = RemWeight;
            dtdetail.push(dr);
            renderGrid();
        } else {
            msg("Can't break further because Total Qty is Equal To Output Qty:" + K.num(OutputQty, 3)
                + ".\n If you want to break further first break any row of same Detail", 'Error', 'error');
        }
    }

    /** DeleteRow:721 - a saved row (Id > 0) goes to lstRemoveRecord with ActionId 3. */
    function DeleteRow(i) {
        var r = dtdetail[i];
        if (toI(r.Id) > 0) {
            lstRemoveRecord.push({ Id: toI(r.Id), ItemId: toI(r.ItemId), Qty: toD(r.Qty),
                                   NetWeightKg: toD(r.NetWeightKg), ExportInvoiceId: toI(r.ExportInvoiceId) });
        }
        dtdetail.splice(i, 1);
        if (cur >= dtdetail.length) cur = dtdetail.length - 1;
    }

    /* ======================================================================== CmbJobOrder_Leave:622 */
    function CmbJobOrder_Leave() {
        var id = toI($id('CmbJobOrder').value);
        if (id > 0) return GetDataAgainstJobOrderFromProduction(id);
    }
    function GetDataAgainstJobOrderFromProduction(jobOrderId) {
        dtdetail = [];
        return getJson(API + '/output-items?jobOrderId=' + jobOrderId).then(function (rows) {
            if (rows && rows.length > 0) {
                rows.forEach(function (x) {
                    dtdetail.push({ Id: 0, ItemId: col(x, 'ItemId'), ItemName: col(x, 'ItemName'),
                                    OutPutQty: col(x, 'Qty'), OutPutWeight: col(x, 'Weight'),
                                    Qty: col(x, 'Qty'), NetWeightKg: col(x, 'Weight'), ExportInvoiceId: null });
                });
                grdBound = true;
            } else {
                grdBound = false;
            }
            cur = -1;
            renderGrid();
        }).catch(function (e) { return msg(e.message); });
    }

    /* ============================================================================== Insert():304 */
    function Insert() {
        if (!grdBound || dtdetail.length === 0) return msg('Grid Record Not Found');
        /* FormValidation:528 */
        if (!activeRow($id('CmbJobOrder'))) {
            return msg('Job Order Field Required').then(function () { focusCombo($id('CmbJobOrder')); });
        }
        return ask(RecId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?', 'Confirm').then(function (yes) {
            if (!yes) return;
            return postJson(API + '/save', {
                recId: RecId,
                jobOrderId: toI($id('CmbJobOrder').value),
                rows: dtdetail.map(function (r) {
                    return { Id: toI(r.Id), ItemId: toI(r.ItemId), ItemName: r.ItemName, OutPutQty: toD(r.OutPutQty),
                             OutPutWeight: toD(r.OutPutWeight), Qty: toD(r.Qty), NetWeightKg: toD(r.NetWeightKg),
                             ExportInvoiceId: toI(r.ExportInvoiceId) };
                }),
                removed: lstRemoveRecord
            }).then(function () {
                return msg(RecId > 0 ? 'Record Update Successfully' : 'Record Save Successfully').then(Reset);
            });
        }).catch(function (e) { return msg(e.message); });
    }
    /** btnsave_Click:291 (RecId = 0 first) and btnUpdate_Click:548. */
    function btnsave_Click() { return busy('btnsave', function () { RecId = 0; return Insert(); }); }
    function btnUpdate_Click() { return busy('btnUpdate', Insert); }

    /** Reset():401 */
    function Reset() {
        $id('btnsave').classList.remove('is-hidden');
        $id('btnUpdate').classList.add('is-hidden');
        lstRemoveRecord = [];
        setComboEnabled($id('CmbJobOrder'), true);
        setValue($id('CmbJobOrder'), '');
        dtdetail = [];
        grdBound = false;
        cur = -1;
        renderGrid();
    }

    /* ================================================================================ ReadById:948 */
    function ReadById(jobOrderId) {
        Reset();
        RecId = jobOrderId;                    /* set before the read: it stays even when nothing comes back */
        return getJson(API + '/read-by-id?jobOrderId=' + jobOrderId).then(function (lst) {
            if (lst && lst.length > 0) {
                $id('btnsave').classList.add('is-hidden');
                $id('btnUpdate').classList.remove('is-hidden');
                selectTab(0);
                setComboEnabled($id('CmbJobOrder'), false);
                setValue($id('CmbJobOrder'), col(lst[0], 'InvProductionJobOrderId'));
                lst.forEach(function (x) {
                    dtdetail.push({ Id: col(x, 'Id'), ItemId: col(x, 'ItemId'), ItemName: col(x, 'ItemName'),
                                    OutPutQty: col(x, 'OutPutQty'), OutPutWeight: col(x, 'OutPutWeight'),
                                    Qty: col(x, 'Qty'), NetWeightKg: col(x, 'NetWeight'), ExportInvoiceId: col(x, 'ExImInvoiceId') });
                });
                grdBound = true;
                renderGrid();
            }
        }).catch(function (e) { return msg(e.message); });
    }

    /* ================================================================================ Slip:1018 */
    function Slip(jobOrderId, btn) {
        if (!(jobOrderId > 0)) return msg('Record Not Found For DisPlay');
        var w = window.CrystalPrint.reserve();
        return busy(btn, function () {
            return getJson(API + '/print-rows?jobOrderId=' + jobOrderId).then(function (d) {
                if (!d || !(d.rows > 0)) { window.CrystalPrint.release(w); return msg('Record Not Found For DisPlay'); }
                return window.CrystalPrint.open('poa-627', { jobOrderId: jobOrderId }, null, w);
            }, function (e) { window.CrystalPrint.release(w); return msg(e.message); });
        });
    }

    /* ============================================================================ GetHistoryData:768 */
    function histDate(chk, inp) { return $id(chk).checked && $id(inp).value ? $id(inp).value + histTime : null; }
    function GetHistoryData() {
        var q = { fromDate: histDate('chkFromDateHistory', 'FromDateHistory'),
                  toDate: histDate('chkToDateHistory', 'ToDateHistory'),
                  jobOrderId: toI($id('CmbJobOrderNoHistory').value) };
        return getJson(API + '/history?' + qs(q)).then(function (rows) {
            historyRows = rows && rows.length > 0 ? rows : null;
            hcur = -1;
            renderHistory();
            say(historyRows ? historyRows.length + ' record(s)' : '');
        }).catch(function (e) { return msg(e.message); });
    }
    /* HistoryGridSetting:828 - Edit (pos 0) and Print (pos 1) buttons, frozen; the rest as listed,
       Id / JobOrderId / ItemId / InvoiceId hidden; doubles right-aligned "#,##0.##" with a sum. */
    var HCOLS = [
        { k: 'JobOrderNo', w: 120, link: true }, { k: 'ItemName', w: 180 }, { k: 'Qty', w: 75, num: true },
        { k: 'NetWeight', w: 80, num: true }, { k: 'InvoiceNo', w: 120 }, { k: 'InvoiceDate', w: 80, d: 'd' },
        { k: 'EntryDate', w: 135, d: 't' }, { k: 'EntryUser', w: 120 }, { k: 'ModifyDate', w: 135, d: 't' },
        { k: 'ModifyUser', w: 120 }
    ];
    function dMMMyyhm(v) {
        if (!v) return '';
        var s = K.dmyhm(v);                    /* dd-MM-yyyy hh:mm tt */
        var p = /^(\d{2})-(\d{2})-(\d{4}) (.*)$/.exec(s);
        return p ? K.dMMMyy(String(v)) + ' ' + p[4] : s;
    }
    function hcell(c, v) {
        if (v === null || v === undefined || v === '') return '';
        if (c.num) return K.num(v, 2);
        if (c.d === 'd') return K.dMMMyy(v);
        if (c.d === 't') return dMMMyyhm(v);
        return String(v);
    }
    function renderHistory() {
        var t = $id('DataGridHistory');
        if (!historyRows) { t.innerHTML = ''; return; }
        var h = '<colgroup><col style="width:50px"><col style="width:50px">' + HCOLS.map(function (c) {
            return '<col style="width:' + c.w + 'px">';
        }).join('') + '</colgroup><thead><tr><th class="frz" style="left:0">Edit</th><th class="frz" style="left:50px">Print</th>'
            + HCOLS.map(function (c) { return '<th>' + esc(c.k) + '</th>'; }).join('') + '</tr></thead><tbody>';
        var sq = 0, sw = 0;
        historyRows.forEach(function (r, i) {
            sq += toD(r.Qty); sw += toD(r.NetWeight);
            h += '<tr data-i="' + i + '"' + (i === hcur ? ' class="cur"' : '') + '>'
                + '<td class="frz" style="left:0"><button type="button" class="cb" data-hbtn="Edit">Edit</button></td>'
                + '<td class="frz" style="left:50px"><button type="button" class="cb" data-hbtn="Print">Print</button></td>'
                + HCOLS.map(function (c) {
                    var txt = esc(hcell(c, r[c.k]));
                    if (c.link && txt) txt = '<a class="lnk" data-hbtn="Edit">' + txt + '</a>';
                    return '<td' + (c.num ? ' class="num"' : '') + '>' + txt + '</td>';
                }).join('') + '</tr>';
        });
        h += '</tbody><tfoot><tr><td class="frz" style="left:0"></td><td class="frz" style="left:50px"></td>' + HCOLS.map(function (c) {
            return '<td class="num">' + (c.k === 'Qty' ? esc(K.num(sq, 2)) : c.k === 'NetWeight' ? esc(K.num(sw, 2)) : '') + '</td>';
        }).join('') + '</tr></tfoot>';
        t.innerHTML = h;
    }

    /* ============================================================================== GrdPopUp (F1) */
    var popupRow = -1;
    function openInvoicePopup(i) {
        popupRow = i;
        var dlg = $id('dlgInvoice');
        var cols = dtInvoice.length ? Object.keys(dtInvoice[0]).filter(function (k) { return k.toLowerCase() !== 'id'; }) : ['InvoiceNo'];
        function draw(f) {
            f = (f || '').toLowerCase();
            $id('grdInvoicePopup').innerHTML = '<thead><tr>' + cols.map(function (c) { return '<th>' + esc(c) + '</th>'; }).join('')
                + '</tr></thead><tbody>' + dtInvoice.map(function (r, k) {
                    var cells = cols.map(function (c) { return hcellPopup(c, r[c]); });
                    if (f && cells.join(' ').toLowerCase().indexOf(f) < 0) return '';
                    return '<tr data-k="' + k + '" tabindex="0">' + cells.map(function (x) { return '<td>' + esc(x) + '</td>'; }).join('') + '</tr>';
                }).join('') + '</tbody>';
        }
        draw('');
        $id('txtInvoiceFilter').value = '';
        $id('txtInvoiceFilter').oninput = function () { draw(this.value); };
        dlg.classList.remove('is-hidden');
        $id('txtInvoiceFilter').focus();
    }
    function hcellPopup(c, v) {
        if (v === null || v === undefined) return '';
        if (/date/i.test(c)) return K.dMMMyy(v);
        if (typeof v === 'number') return str15(v);
        return String(v);
    }
    /** grdPopUp.ReturnId -> Cells["ExportInvoiceId"].Value (0 when the popup closes without a pick). */
    function closeInvoicePopup(id) {
        $id('dlgInvoice').classList.add('is-hidden');
        var r = dtdetail[popupRow];
        if (r) {
            r.ExportInvoiceId = id;
            renderGrid();
        }
        popupRow = -1;
    }

    /* ================================================================================== tabs */
    function selectTab(n) {
        $id('tabPage1').classList.toggle('is-hidden', n !== 0);
        $id('tabPage2').classList.toggle('is-hidden', n !== 1);
        Array.prototype.forEach.call(document.querySelectorAll('#tabControl1 button'), function (b) {
            b.classList.toggle('on', toI(b.getAttribute('data-tab')) === n);
        });
    }
    function closeForm() {
        if (window.opener && !window.opener.closed) { window.close(); return; }
        K.close();
    }

    /* ================================================================================ events */
    function wire() {
        onLeave($id('CmbJobOrder'), CmbJobOrder_Leave);
        $id('btnnew').addEventListener('click', Reset);
        $id('btnsave').addEventListener('click', btnsave_Click);
        $id('btnUpdate').addEventListener('click', btnUpdate_Click);
        $id('btnPrint627').addEventListener('click', function () { Slip(toI($id('CmbJobOrder').value), 'btnPrint627'); });
        $id('btnShow').addEventListener('click', function () { busy('btnShow', GetHistoryData); });
        $id('btnResetHistory').addEventListener('click', function () {
            var today = nowIso().substring(0, 10);
            $id('FromDateHistory').value = today;
            $id('ToDateHistory').value = today;
            histTime = timePart(nowIso());
            $id('CmbJobOrderNoHistory').value = '';
            historyRows = null;
            renderHistory();
        });
        $id('btnRefreshHistory').addEventListener('click', function () {
            busy('btnRefreshHistory', function () {
                return getJson(API + '/history-job-orders').then(bindHistoryCombo).catch(function (e) { return msg(e.message); });
            });
        });
        ['chkFromDateHistory', 'chkToDateHistory'].forEach(function (id) {
            var inp = $id(id === 'chkFromDateHistory' ? 'FromDateHistory' : 'ToDateHistory');
            $id(id).addEventListener('change', function () { inp.disabled = !$id(id).checked; });
        });
        Array.prototype.forEach.call(document.querySelectorAll('#tabControl1 button'), function (b) {
            b.addEventListener('click', function () { selectTab(toI(b.getAttribute('data-tab'))); });
        });

        /* ---- the form grid */
        var g = $id('grd');
        g.addEventListener('click', function (e) {
            var tr = e.target.closest('tbody tr');
            if (!tr) return;
            var i = toI(tr.getAttribute('data-i'));
            cur = i;
            Array.prototype.forEach.call(g.querySelectorAll('tbody tr'), function (x) { x.classList.toggle('cur', x === tr); });
            var b = e.target.closest('button[data-btn]');
            if (b) columnButton(i, b.getAttribute('data-btn'));
        });
        g.addEventListener('focusin', function (e) {
            var inp = e.target.closest('input.edit');
            if (inp) {
                var r = dtdetail[toI(inp.closest('tr').getAttribute('data-i'))];
                if (r) { inp.value = str15(toD(r[inp.getAttribute('data-col')])); inp.select(); }
            }
            var tr = e.target.closest('tbody tr');
            if (tr) { cur = toI(tr.getAttribute('data-i')); }
        });
        g.addEventListener('change', function (e) {
            var el = e.target.closest('[data-col]');
            if (!el) return;
            var i = toI(el.closest('tr').getAttribute('data-i'));
            var key = el.getAttribute('data-col');
            var r = dtdetail[i];
            if (!r) return;
            r[key] = key === 'ExportInvoiceId' ? toI(el.value) : toD(el.value);
            grd_CellUpdated(i, key);
            var focusKey = key;
            renderGrid();
            var next = g.querySelector('tr[data-i="' + i + '"] [data-col="' + focusKey + '"]');
            if (next && document.activeElement === document.body) next.focus();
        });
        g.addEventListener('focusout', function (e) {
            var inp = e.target.closest('input.edit');
            if (!inp) return;
            var r = dtdetail[toI(inp.closest('tr').getAttribute('data-i'))];
            if (r) inp.value = inp.getAttribute('data-col') === 'Qty' ? fmtQty(r.Qty) : fmtWt(r.NetWeightKg);
        });
        $id('wrapGrd').addEventListener('keydown', function (e) {
            /* grd_KeyDown:1048 - F1 on the Export Invoice cell opens GrdPopUp. */
            if (e.key === 'F1') {
                var el = e.target.closest && e.target.closest('[data-col="ExportInvoiceId"]');
                if (el) { e.preventDefault(); openInvoicePopup(toI(el.closest('tr').getAttribute('data-i'))); }
                return;
            }
            /* AllowDelete = True: the Delete key removes the current row straight from dtdetail
               (not through DeleteRow, so a saved row is NOT queued for deletion). */
            if (e.key === 'Delete' && !(e.target.closest && e.target.closest('input, select')) && cur >= 0 && dtdetail[cur]) {
                e.preventDefault();
                dtdetail.splice(cur, 1);
                cur = Math.min(cur, dtdetail.length - 1);
                renderGrid();
            }
        });

        /* ---- the invoice popup */
        $id('dlgInvoice').addEventListener('click', function (e) {
            if (e.target.hasAttribute('data-close')) { closeInvoicePopup(0); return; }
        });
        $id('grdInvoicePopup').addEventListener('dblclick', function (e) {
            var tr = e.target.closest('tbody tr');
            if (tr) closeInvoicePopup(toI(col(dtInvoice[toI(tr.getAttribute('data-k'))], 'Id')));
        });
        $id('dlgInvoice').addEventListener('keydown', function (e) {
            if (e.key === 'Escape') { e.preventDefault(); e.stopPropagation(); closeInvoicePopup(0); }
            else if (e.key === 'Enter') {
                var tr = e.target.closest && e.target.closest('tbody tr');
                if (!tr) tr = $id('grdInvoicePopup').querySelector('tbody tr');
                if (tr) { e.preventDefault(); closeInvoicePopup(toI(col(dtInvoice[toI(tr.getAttribute('data-k'))], 'Id'))); }
            }
        });

        /* ---- the history grid */
        var hg = $id('DataGridHistory');
        hg.addEventListener('click', function (e) {
            var tr = e.target.closest('tbody tr');
            if (!tr) return;
            hcur = toI(tr.getAttribute('data-i'));
            Array.prototype.forEach.call(hg.querySelectorAll('tbody tr'), function (x) { x.classList.toggle('cur', x === tr); });
            var b = e.target.closest('[data-hbtn]');
            if (!b) return;
            var r = historyRows[hcur];
            var jo = toI(r.JobOrderId);
            if (b.getAttribute('data-hbtn') === 'Edit') ReadById(jo);
            else Slip(jo, b);
        });
        $id('wrapHistory').addEventListener('keydown', function (e) {
            /* DataGridHistory_KeyDown:916 */
            if (!historyRows) return;
            if (e.key === 'ArrowDown' && !e.ctrlKey) { e.preventDefault(); hcur = Math.min(historyRows.length - 1, hcur + 1); renderHistory(); }
            else if (e.key === 'ArrowUp' && !e.ctrlKey) { e.preventDefault(); hcur = Math.max(0, hcur - 1); renderHistory(); }
            else if (e.ctrlKey && (e.key === 'Enter' || e.key === ' ') && hcur >= 0) {
                e.preventDefault();
                /* Ctrl+Space reads only on the Edit column; Ctrl+Enter always reads. */
                if (e.key === ' ' && !(e.target.closest && e.target.closest('[data-hbtn="Edit"]'))) return;
                ReadById(toI(historyRows[hcur].JobOrderId));
            }
        });

        /* DefineCountry_KeyDown:560 (KeyPreview). Ctrl+S / Ctrl+U call the handlers directly - not
           gated on the buttons' Visible/Enabled, exactly as the desktop does. */
        document.addEventListener('keydown', function (e) {
            if (e.defaultPrevented || modalOpen()) return;
            var k = e.key;
            if (e.ctrlKey && (k === 'n' || k === 'N')) { e.preventDefault(); Reset(); }
            if (e.ctrlKey && (k === 's' || k === 'S')) { e.preventDefault(); btnsave_Click(); }
            if (e.ctrlKey && (k === 'u' || k === 'U')) { e.preventDefault(); btnUpdate_Click(); }
            if ((e.ctrlKey && (k === 'e' || k === 'E')) || k === 'Escape') { e.preventDefault(); closeForm(); }
        });
    }

    document.addEventListener('DOMContentLoaded', function () {
        var today = nowIso().substring(0, 10);
        $id('FromDateHistory').value = today;
        $id('ToDateHistory').value = today;
        $id('CmbJobOrder').innerHTML = '<option value=""></option>';
        $id('CmbJobOrderNoHistory').innerHTML = '<option value=""></option>';
        wire();
        load().then(function () { focusCombo($id('CmbJobOrder')); });
    });
}());
