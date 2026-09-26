/* ============================================================================================
 * ScreenDefinition 960 "Delivery Challan Against PreBill" — frmDeliveryChallanAgainstPurchasePreBill.cs
 * (DocumentTypeId 148, helper frmDeliveryChallanAgainstPurchasePreBill_Helper.cs) with its
 * Load Pre_Bill dialog frmPendingPurchasePreBillLoader.cs (pre-bills of type 147).
 *
 * Line refs are Form.cs unless marked "ldr:" (the loader) or "hlp:" (the helper). The page only
 * reproduces the form's own behaviour; the server re-reads every line / expense from the pending
 * loader or the saved document and re-checks everything (see DeliveryChallanPreBillService).
 * Global: DcPreBill.
 * ============================================================================================ */
(function () {
    'use strict';

    var C = window.StoreCommon;
    var $id = C.$id, esc = C.esc, num = C.num;
    var api = '/api/store/delivery-challan-prebill';

    var look = { rights: {}, deliveryTerms: [], cities: [], itemConditions: [], otherItems: [], historyCities: [], historyItems: [] };
    var st = null;             // FromReset state: RecId, grid rows, lstRemoveRecord, expense rows
    var firstLoad = true;
    var docDateFromOpened = false;  // txtDocDate set by ReadById (midnight); FromReset never resets it
    var hist = { rows: [], selectedId: 0 };
    var ldr = null;            // a new loader per click (btnPurchaseOrderLoader_Click:1922)

    /* ------------------------------------------------------------------ helpers */

    var MONTHS = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    function fail(e) { alert(e && e.message ? e.message : e); }
    function today() { return C.today(); }
    function addDays(n) {
        var d = new Date(); d.setDate(d.getDate() + n);
        return d.getFullYear() + '-' + String(d.getMonth() + 1).padStart(2, '0') + '-' + String(d.getDate()).padStart(2, '0');
    }
    /** Conversion.ToInt(text) — Convert.ToInt32(string): an integer or 0. */
    function intText(s) { var t = String(s === null || s === undefined ? '' : s).trim(); return /^[-+]?\d+$/.test(t) ? parseInt(t, 10) : 0; }
    /** .NET Math.Round / Convert.ToInt32(double) — half to even. */
    function roundEven(x) { var f = Math.floor(x), d = x - f; if (d > 0.5) return f + 1; if (d < 0.5) return f; return (f % 2 === 0) ? f : f + 1; }
    function netStr(v) { var n = num(v); return String(parseFloat(n.toPrecision(15))); }
    function caption(k) { return String(k).replace(/([a-z])([A-Z])/g, '$1 $2').replace(/([A-Z]+)([A-Z][a-z])/g, '$1 $2'); }
    /** Conversion.CheckDateTimeNull — no value, or the 1900-01-01 placeholder. */
    function nullDate(v) { var s = C.isoDay(v); return !s || s <= '1900-01-01'; }
    /** "dd-MMM-yy". */
    function dmy(v) {
        if (nullDate(v)) return '';
        var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(String(v));
        return m[3] + '-' + MONTHS[parseInt(m[2], 10) - 1] + '-' + m[1].substring(2);
    }
    /** "dd-MMM-yy hh:mm tt". */
    function dmyt(v) {
        if (nullDate(v)) return '';
        var m = /^(\d{4})-(\d{2})-(\d{2})[T ](\d{2}):(\d{2})/.exec(String(v));
        if (!m) return dmy(v);
        var h = parseInt(m[4], 10), ap = h >= 12 ? 'PM' : 'AM'; h = h % 12; if (h === 0) h = 12;
        return m[3] + '-' + MONTHS[parseInt(m[2], 10) - 1] + '-' + m[1].substring(2) + ' ' + String(h).padStart(2, '0') + ':' + m[5] + ' ' + ap;
    }
    function has(list, id) { return (list || []).some(function (r) { return String(r.Id) === String(id); }); }
    /** InfragisticsHelper.BindAndRetainSelection with insertDefaultRow — "-- Select --" (value 0) first. */
    function fillDefault(id, rows, textKey, defaultText) {
        var el = $id(id);
        el.innerHTML = '<option value="0">' + esc(defaultText || '-- Select --') + '</option>' + (rows || []).map(function (r) {
            return '<option value="' + esc(r.Id) + '">' + esc(r[textKey]) + '</option>';
        }).join('');
    }
    /** Retain the previous value when it is in the list; otherwise ActivateRow (rowIndex, default row counted) or blank. */
    function bindRetain(id, rows, textKey, activateRowIndex) {
        var keep = $id(id).value;
        fillDefault(id, rows, textKey);
        if (keep && keep !== '0' && has(rows, keep)) { $id(id).value = keep; return; }
        if (activateRowIndex !== null && activateRowIndex !== undefined && rows.length + 1 > activateRowIndex) {
            $id(id).value = activateRowIndex === 0 ? '0' : String(rows[activateRowIndex - 1].Id);
            return;
        }
        $id(id).value = '0';
    }
    function setCombo(id, value) {
        var el = $id(id), v = String(C.intOf(value));
        if (!Array.prototype.some.call(el.options, function (o) { return o.value === v; })) {
            var o = document.createElement('option'); o.value = v; o.textContent = v === '0' ? '-- Select --' : v; el.appendChild(o);
        }
        el.value = v;
    }
    function conditionText(id) {
        var c = (look.itemConditions || []).find(function (x) { return String(x.Id) === String(id); });
        return c ? c.Description : (C.intOf(id) ? String(id) : '');
    }
    function otherItemText(id) {
        var c = (look.otherItems || []).find(function (x) { return String(x.Id) === String(id); });
        return c ? c.OtherItemName : (C.intOf(id) ? String(id) : '');
    }
    function clearTable(id) { var t = $id(id); t.tHead.innerHTML = ''; t.tBodies[0].innerHTML = ''; }

    /* ------------------------------------------------------------------ state */

    function blankState() { return { id: 0, rows: [], removed: [], exp: [] }; }

    /* ------------------------------------------------------------------ detail grid (grd) */

    /** hlp:DetailGridCommonSetting + grdDetailSetting:590 — X button first; ThisQty / Remarks editable. */
    var GRID_COLS = ['PurchaseDemandNo', 'PurchasePreBillNo', 'ItemCode', 'ItemName', 'Uom', 'ItemConditionId', 'TotalDemandQty',
        'PreBillQty', 'UsedQty', 'BalanceQty', 'ThisQty', 'PartyBillNo', 'PartyBillDate', 'VendorSupplier', 'Remarks'];
    var CAPTIONS = { TotalDemandQty: 'Approved Demand Qty', UsedQty: 'Qty Delivered To Factory', ThisQty: 'Delivery Challan Qty', ItemConditionId: 'Item Condition' };
    var NUMERIC = { TotalDemandQty: 1, PreBillQty: 1, UsedQty: 1, BalanceQty: 1, ThisQty: 1, PurchaseDemandNo: 1, PurchasePreBillNo: 1 };

    function colCaption(k) { return CAPTIONS[k] || caption(k); }

    function renderGrid() {
        var t = $id('grd');
        t.tHead.innerHTML = '<tr><th>Delete</th>' + GRID_COLS.map(function (k) { return '<th>' + esc(colCaption(k)) + '</th>'; }).join('') + '</tr>';
        t.tBodies[0].innerHTML = st.rows.map(function (r, i) {
            return '<tr><td><button type="button" class="cx-x" data-del="' + i + '" title="Delete">X</button></td>' + GRID_COLS.map(function (k) {
                if (k === 'ThisQty') return '<td class="num"><input type="text" class="num" data-i="' + i + '" data-k="ThisQty" value="' + esc(netStr(r.ThisQty)) + '"></td>';
                if (k === 'Remarks') return '<td><input type="text" data-i="' + i + '" data-k="Remarks" value="' + esc(r.Remarks || '') + '"></td>';
                if (k === 'ItemConditionId') return '<td>' + esc(conditionText(r.ItemConditionId)) + '</td>';
                if (k === 'PartyBillDate') return '<td>' + esc(dmy(r.PartyBillDate)) + '</td>';
                if (NUMERIC[k]) return '<td class="num">' + esc(netStr(r[k])) + '</td>';
                return '<td>' + esc(r[k] === null || r[k] === undefined ? '' : r[k]) + '</td>';
            }).join('') + '</tr>';
        }).join('');
        t.querySelectorAll('input[data-i]').forEach(function (inp) {
            inp.onchange = function () { cellUpdated(parseInt(inp.getAttribute('data-i'), 10), inp.getAttribute('data-k'), inp.value); };
        });
        t.querySelectorAll('button[data-del]').forEach(function (b) {
            b.onclick = function () { deleteDetailRow(parseInt(b.getAttribute('data-del'), 10)); };
        });
    }

    /** grd_CellUpdated:543 → RecalculateRow:565. */
    function cellUpdated(i, key, value) {
        var row = st.rows[i]; if (!row) return;
        if (key === 'Remarks') { row.Remarks = value; return; }
        row.ThisQty = num(value);
        if (num(row.ThisQty) > num(row.BalanceQty)) {
            row.ThisQty = num(row.BalanceQty);
            renderGrid();
            alert("You can't add Qty more than Balance Qty");
            return;
        }
        renderGrid();
    }

    /** DeleteDetailRow:617 — a saved row asks first and goes to lstRemoveRecord. */
    function deleteDetailRow(i) {
        var r = st.rows[i]; if (!r) return;
        if (C.intOf(r.Id) !== 0) {
            if (!confirm('Are you sure to Delete?')) return;
            st.removed.push(C.intOf(r.Id));
        }
        st.rows.splice(i, 1);
        renderGrid();
    }

    /* ------------------------------------------------------------------ expense grid (grdInvExp) */

    /** grdInvExpSettings:674 — Id / PreBillId / PreBillExpenseId hidden; only Remarks editable; no add/delete. */
    var EXP_COLS = ['PreBillNo', 'ItemId', 'Qty', 'Rate', 'Amount', 'Remarks'];
    function renderExp() {
        var t = $id('grdInvExp');
        t.tHead.innerHTML = '<tr>' + EXP_COLS.map(function (k) { return '<th>' + esc(k === 'ItemId' ? 'Other Item Name' : caption(k)) + '</th>'; }).join('') + '</tr>';
        t.tBodies[0].innerHTML = st.exp.map(function (r, i) {
            return '<tr>' + EXP_COLS.map(function (k) {
                if (k === 'Remarks') return '<td><input type="text" data-e="' + i + '" value="' + esc(r.Remarks || '') + '"></td>';
                if (k === 'ItemId') return '<td>' + esc(otherItemText(r.ItemId)) + '</td>';
                if (k === 'PreBillNo') return '<td class="num">' + esc(C.intOf(r.PreBillNo) || '') + '</td>';
                return '<td class="num">' + esc(netStr(r[k])) + '</td>';
            }).join('') + '</tr>';
        }).join('');
        t.querySelectorAll('input[data-e]').forEach(function (inp) {
            inp.onchange = function () { var r = st.exp[parseInt(inp.getAttribute('data-e'), 10)]; if (r) r.Remarks = inp.value; };
        });
    }

    function renderAll() { renderGrid(); renderExp(); }

    /* ------------------------------------------------------------------ combos */

    /** DeliveryTerm:438 — ActivateRow true, index 2: a value not in the list selects the second term. */
    function bindDeliveryTerm() { bindRetain('CmbDeliveryTerm', look.deliveryTerms, 'Description', 2); }
    /** CityDtFillFromGlobalAndBind:454 — ActivateRow default (Rows[0], the default row). */
    function bindCity() { bindRetain('CmbCityName', look.cities, 'Description', 0); }
    /** GetConfigurationsFromGlobalAndBindValuesInColumns:382 — "City Area". */
    function cityDefault() { if (C.intOf(look.cityAreaId) > 0) setCombo('CmbCityName', look.cityAreaId); }
    /** HistoryComboBind:1136 — ActivateRow false. */
    function bindHistoryCombos() {
        bindRetain('CmbBillToPartyHistory', look.historyCities, 'Name', null);
        bindRetain('CmbItemHistory', look.historyItems, 'Name', null);
    }
    /** Datetypefill → InfragisticsHelper.BindComboDateType ("Select..." default, value 0). */
    function dateTypeFill() {
        $id('cmbDateTypeHistory').innerHTML = '<option value="0">Select...</option>' +
            ['This Day', 'This Week', 'This Month', 'This Year', 'Financial Year'].map(function (t, i) {
                return '<option value="' + (i + 1) + '">' + t + '</option>';
            }).join('');
        $id('cmbDateTypeHistory').value = '0';
    }

    function applyRights() {                                                                  // InitializeComponentMethod:357
        var r = look.rights || {};
        $id('btnsave').disabled = !r.save;
        $id('btnupdate').disabled = !r.update;
        $id('btnDelete').disabled = !r['delete'];
        $id('btnprint').disabled = !r.print;
        if (!r.view) {
            $id('rightsNote').textContent = 'You do not have the View right for Delivery Challan Against PreBill.';
            $id('rightsNote').classList.remove('is-hidden');
        }
    }

    /* ------------------------------------------------------------------ New / Refresh */

    function localReset() {                                                                    // FromReset:1033 (page part)
        st = blankState();
        $id('btnsave').classList.remove('is-hidden');
        $id('btnupdate').classList.add('is-hidden');
        $id('btnDelete').classList.add('is-hidden');
        $id('txtVehicleNo').value = '';
        $id('txtBiltyNo').value = '';
        $id('txtRemarksMain').value = '';
        renderAll();
        cityDefault();
    }

    /** btnnew_Click → FromReset: Doc Date and Delivery Term are kept. */
    function reset() {
        localReset();
        return C.getJson(api + '/doc-no').then(function (r) { $id('txtDocNo').value = r.docNo; }).catch(fail);
    }

    /** btnRefresh_Click:1065. */
    function refresh() {
        return C.getJson(api + '/lookups').then(function (l) {
            var rights = look.rights;
            look = l; look.rights = rights;                                                    // rights are a Form_Load thing
            bindDeliveryTerm();
            bindCity();
            cityDefault();
            renderAll();                                                                       // ExpenseGridCombBind / GridComboBind
        }).catch(fail);
    }

    /* ------------------------------------------------------------------ save / update / delete / print */

    function payload() {
        return {
            Id: st.id,
            DocNo: intText($id('txtDocNo').value),
            DocDate: $id('txtDocDate').value,
            DocDateFromOpened: docDateFromOpened,
            DeliveryTermId: C.intOf($id('CmbDeliveryTerm').value),
            CityId: C.intOf($id('CmbCityName').value),
            VehicleNo: $id('txtVehicleNo').value,
            BiltyNo: $id('txtBiltyNo').value,
            RemarksHeader: $id('txtRemarksMain').value,
            rows: st.rows.map(function (r) {
                return { Id: C.intOf(r.Id), PurchasePreBillHeaderId: C.intOf(r.PurchasePreBillHeaderId),
                    PurchasePreBillDetailId: C.intOf(r.PurchasePreBillDetailId), ThisQty: num(r.ThisQty), Remarks: r.Remarks || '' };
            }),
            removedIds: st.removed.slice(),
            expenses: st.exp.map(function (e) {
                return { Id: C.intOf(e.Id), PreBillId: C.intOf(e.PreBillId), PreBillExpenseId: C.intOf(e.PreBillExpenseId),
                    ItemId: C.intOf(e.ItemId), Remarks: e.Remarks || '' };
            })
        };
    }

    /** Insert():821 — the form checks, the confirmation, then the server (which re-checks all). */
    function insert() {
        if (!st.rows.length) { alert('Detail Record Not Found'); return; }                        // :830
        if (intText($id('txtDocNo').value) === 0) { alert('Doc No must be a non-zero number'); return; }
        if (!$id('txtVehicleNo').value.trim()) { alert('Vehicle No field is required'); $id('txtVehicleNo').focus(); return; }
        if (!confirm(st.id === 0 ? 'Are you sure to Save?' : 'Are you sure to Update?')) return;
        C.postJson(api + '/save', payload()).then(function (r) {
            alert(r.message);
            reset();                                                                            // FromReset:925
            if ($id('ChkBox').checked && look.rights.print) printDoc(r.id);                    // :926
        }).catch(fail);
    }

    /** btnsave_Click:767 — RecId = 0 first. */
    function save() { st.id = 0; insert(); }

    /** btnupdate_Click:781. */
    function update() {
        if (!st.id) { alert('Record Not Update because RecId Not Found'); return; }
        insert();
    }

    /** btnDelete_Click:797. */
    function remove() {
        if (!(st.id > 0)) { alert('No record found to Delete'); return; }
        if (!confirm('Are you sure to Delete?')) return;
        C.postJson(api + '/' + st.id + '/delete', {}).then(function (r) {
            alert(r.message);
            reset();
        }).catch(fail);
    }

    /** btnprint_Click:1591 → ShowPrint(RecId). */
    function print() { printDoc(st.id); }

    /** The slip with its expense sub-report — the procedures' data, not the Crystal layout. */
    function printWindow(title, data) {
        var rows = data.rows || [], exp = data.expenses || [];
        function table(list) {
            if (!list.length) return '<p>(no rows)</p>';
            var cols = Object.keys(list[0]);
            return '<table><thead><tr>' + cols.map(function (c) { return '<th>' + esc(c) + '</th>'; }).join('') + '</tr></thead><tbody>' +
                list.map(function (r) { return '<tr>' + cols.map(function (c) { return '<td>' + esc(r[c]) + '</td>'; }).join('') + '</tr>'; }).join('') +
                '</tbody></table>';
        }
        var w = window.open('', '_blank');
        if (!w) { alert('Allow pop-ups to print.'); return; }
        w.document.write('<html><head><title>' + esc(title) + '</title><style>body{font-family:Verdana;font-size:10px}' +
            'table{border-collapse:collapse;margin-bottom:10px}td,th{border:1px solid #444;padding:2px 4px;white-space:nowrap}</style></head><body>' +
            '<h3>' + esc(title) + '</h3>' + table(rows) + '<h4>Expenses</h4>' + table(exp) + '<script>window.print()<\/script></body></html>');
        w.document.close();
    }
    function printDoc(id) {
        C.getJson(api + '/' + C.intOf(id) + '/slip').then(function (d) { printWindow('148 - Delivery Challan Slip', d); }).catch(fail);
    }

    /* ------------------------------------------------------------------ ReadById */

    /** ReadById:940. */
    function readById(id) {
        return C.getJson(api + '/' + id).then(function (o) {
            localReset();
            st.id = C.intOf(o.Id);
            tab('tabForm');
            $id('btnsave').classList.add('is-hidden');
            $id('btnupdate').classList.remove('is-hidden');
            $id('btnDelete').classList.remove('is-hidden');
            $id('txtDocDate').value = C.isoDay(o.DocDate);
            docDateFromOpened = true;                                                            // Or.DocDate (a DATE: midnight)
            $id('txtDocNo').value = o.DocNo;
            $id('txtVehicleNo').value = o.VehicleNo || '';
            $id('txtBiltyNo').value = o.BiltyNo || '';
            setCombo('CmbDeliveryTerm', o.DeliveryTermId);
            setCombo('CmbCityName', o.CityId);
            $id('txtRemarksMain').value = o.RemarksHeader || '';
            st.rows = o.rows || [];
            st.exp = o.expenses || [];
            renderAll();
        }).catch(function (e) { alert(e && e.message ? e.message : 'No Record Found'); });
    }

    /* ------------------------------------------------------------------ history */

    function histDefaults() {                                                                   // :371
        var days = C.intOf(look.defaultDaysToLessFromHistoryFromDate);
        $id('FromDateHistory').value = addDays(-(days > 0 ? days : 3));
        $id('ToDateHistory').value = today();
    }

    /** cmbDateTypeHistory_ValueChanged:1180. */
    function dateTypeChanged() {
        var v = C.intOf($id('cmbDateTypeHistory').value), d = new Date();
        if (v === 1) $id('FromDateHistory').value = today();
        else if (v === 2) $id('FromDateHistory').value = addDays(-7);
        else if (v === 3) { $id('FromDateHistory').value = d.getFullYear() + '-' + String(d.getMonth() + 1).padStart(2, '0') + '-01'; $id('ToDateHistory').value = today(); }
        else if (v === 4) { $id('FromDateHistory').value = d.getFullYear() + '-01-01'; $id('ToDateHistory').value = today(); }
        else if (v === 5 && look.financialYearStart) $id('FromDateHistory').value = look.financialYearStart;
    }

    /** btnShow_Click → HistoryGridFill:1257. */
    function showHistory() {
        var dt = (document.querySelector('input[name=rdHist]:checked') || {}).value || 'doc';
        C.getJson(api + '/history' + C.qs({
            dateType: dt,
            fromChecked: $id('chkFromDateHistory').checked, fromDate: $id('FromDateHistory').value,
            toChecked: $id('chkToDateHistory').checked, toDate: $id('ToDateHistory').value,
            fromDocNo: $id('FromDocNoHistory').value, toDocNo: $id('ToDocNoHistory').value,
            cityId: C.intOf($id('CmbBillToPartyHistory').value), itemId: C.intOf($id('CmbItemHistory').value)
        })).then(function (rows) {
            hist.rows = rows || [];
            renderHistory();
        }).catch(fail);
    }

    /** HistoryGridSetting:1391 — Edit and Print buttons before DocNo; IsApproved a checkbox. */
    var HIST_COLS = ['DocNo', 'DocDate', 'ReferenceNo', 'DeliveryTerm', 'CityName', 'RemarksHeader', 'EntryDate', 'EntryUserName',
        'ModifyDate', 'ModifyUserName', 'IsApproved', 'ApprovedDate', 'ApprovalUserName', 'NoOfAttachments'];
    var HIST_TIMES = { EntryDate: 1, ModifyDate: 1, ApprovedDate: 1 };

    function renderHistory() {
        var t = $id('grdhistory');
        clearTable('grdhistoryDetail');
        if (!hist.rows.length) { clearTable('grdhistory'); return; }                            // :1325
        t.tHead.innerHTML = '<tr><th>Edit</th><th>Print</th>' + HIST_COLS.map(function (k) { return '<th>' + esc(caption(k)) + '</th>'; }).join('') + '</tr>';
        t.tBodies[0].innerHTML = hist.rows.map(function (r, i) {
            return '<tr data-i="' + i + '">' +
                '<td><button type="button" class="cx-link" data-a="edit">Edit</button></td>' +
                '<td><button type="button" class="cx-link" data-a="print">Print</button></td>' +
                HIST_COLS.map(function (k) {
                    var v = r[k];
                    if (k === 'DocNo') return '<td class="num"><a href="#" data-a="edit">' + esc(v) + '</a></td>';
                    if (k === 'DocDate') return '<td>' + esc(dmy(v)) + '</td>';
                    if (HIST_TIMES[k]) return '<td>' + esc(dmyt(v)) + '</td>';
                    if (k === 'IsApproved') return '<td style="text-align:center"><input type="checkbox" disabled' + (v ? ' checked' : '') + '></td>';
                    if (k === 'NoOfAttachments') return '<td class="num" title="Attachments are not ported">' + esc(v) + '</td>';
                    return '<td>' + esc(v === null || v === undefined ? '' : v) + '</td>';
                }).join('') + '</tr>';
        }).join('');
        t.tBodies[0].onclick = function (e) {
            var tr = e.target.closest('tr[data-i]'); if (!tr) return;
            var r = hist.rows[parseInt(tr.getAttribute('data-i'), 10)];
            var a = e.target.closest('[data-a]');
            if (a) {
                e.preventDefault();
                if (a.getAttribute('data-a') === 'print') {                                      // grdhistory_ColumnButtonClick:1443
                    if (!look.rights.print) { alert("you don't have print rights..."); return; }
                    printDoc(r.Id);
                } else {
                    if (!look.rights.update) { alert("you don't have update rights..."); return; }
                    readById(r.Id);
                }
                return;
            }
            t.tBodies[0].querySelectorAll('tr').forEach(function (x) { x.classList.toggle('is-selected', x === tr); });
            historySelection(r.Id);
        };
        t.tBodies[0].ondblclick = function (e) {                                                  // grdhistory_DoubleClick:1422
            var tr = e.target.closest('tr[data-i]'); if (!tr || e.target.closest('[data-a]')) return;
            if (!look.rights.update) { alert("ypu don't have updae rights..."); return; }
            readById(hist.rows[parseInt(tr.getAttribute('data-i'), 10)].Id);
        };
    }

    /** grdhistory_SelectionChanged:1493 → GetDetailGrdByHeadId:1510 / grddetailhistorySettings:1533. */
    var HDET_COLS = ['PurchaseDemandNo', 'PurchasePreBillNo', 'ItemCode', 'ItemName', 'Uom', 'ItemCondition', 'TotalDemandQty',
        'PreBillQty', 'ThisQty', 'PartyBillNo', 'PartyBillDate', 'VendorSupplier', 'Remarks'];
    function historySelection(id) {
        hist.selectedId = id;
        C.getJson(api + '/' + id).then(function (o) {
            if (hist.selectedId !== id) return;
            var rows = o.rows || [], t = $id('grdhistoryDetail');
            if (!rows.length) { clearTable('grdhistoryDetail'); return; }
            t.tHead.innerHTML = '<tr>' + HDET_COLS.map(function (k) { return '<th>' + esc(colCaption(k)) + '</th>'; }).join('') + '</tr>';
            t.tBodies[0].innerHTML = rows.map(function (r) {
                return '<tr>' + HDET_COLS.map(function (k) {
                    if (k === 'PartyBillDate') return '<td>' + esc(dmy(r[k])) + '</td>';
                    if (NUMERIC[k]) return '<td class="num">' + esc(netStr(r[k])) + '</td>';
                    return '<td>' + esc(r[k] === null || r[k] === undefined ? '' : r[k]) + '</td>';
                }).join('') + '</tr>';
            }).join('');
        }).catch(function (e) { clearTable('grdhistoryDetail'); fail(e); });
    }

    /** btnResetHistory_Click:1217 — From = Now-3 (not the configured days). */
    function resetHistory() {
        $id('cmbDateTypeHistory').value = '0';
        $id('FromDateHistory').value = addDays(-3);
        $id('ToDateHistory').value = today();
        $id('FromDocNoHistory').value = '';
        $id('ToDocNoHistory').value = '';
        $id('CmbBillToPartyHistory').value = '0';
        $id('CmbItemHistory').value = '0';
        hist.rows = [];
        clearTable('grdhistory');
        clearTable('grdhistoryDetail');
        $id('drdocdate').checked = true;
    }

    /** btnRefreshHistory_Click:1239 — Datetypefill + HistoryComboBind(HistoryComboDbCall()). */
    function refreshHistory() {
        C.getJson(api + '/lookups').then(function (l) {
            dateTypeFill();
            look.historyCities = l.historyCities; look.historyItems = l.historyItems;
            bindHistoryCombos();
        }).catch(fail);
    }

    function tab(id) {
        document.querySelectorAll('.win-tab').forEach(function (t) { t.classList.toggle('is-active', t.getAttribute('data-tab') === id); });
        document.querySelectorAll('.win-tab-panel').forEach(function (p) { p.classList.toggle('is-active', p.id === id); });
    }

    /* ------------------------------------------------------------------ loader (frmPendingPurchasePreBillLoader) */

    /** btnPurchaseOrderLoader_Click:1922 — a fresh dialog: From = Now-7, To = Now, combos empty. */
    function openLoader() {
        ldr = { rows: [], headers: [], checked: {}, detailChecked: {} };
        $id('ldrFromDate').value = addDays(-7);
        $id('ldrToDate').value = today();
        $id('txtDocNoFrom').value = '';
        $id('txtDocNoTo').value = '';
        fillDefault('CmbBillToParty', [], 'Name');
        fillDefault('CmbItemName', [], 'Name');
        clearTable('ldrGrd'); clearTable('ldrGrdDetail');
        C.openModal('dlgPreBill');
        var mine = ldr;
        C.getJson(api + '/loader/combos').then(function (c) {                                   // ldr:InitializeComponentMethod
            if (ldr !== mine) return;
            bindLoaderCombos(c);
            return loaderQuery();
        }).catch(fail);
    }
    function closeLoader() { ldr = null; C.closeModal('dlgPreBill'); }                        // X: dtLoader stays empty

    /** ldr:CombosFill — ActivateRow false. */
    function bindLoaderCombos(c) {
        bindRetain('CmbBillToParty', c.billToParties || [], 'Name', null);
        bindRetain('CmbItemName', c.items || [], 'Name', null);
    }

    /** ldr:PendingDataDbCall:211 + GrdDataBind:237. */
    function loaderQuery() {
        var mine = ldr;
        return C.getJson(api + '/loader' + C.qs({
            fromDate: $id('ldrFromDate').value, toDate: $id('ldrToDate').value,
            fromDocNo: $id('txtDocNoFrom').value, toDocNo: $id('txtDocNoTo').value,
            itemId: C.intOf($id('CmbItemName').value), billToPartyId: C.intOf($id('CmbBillToParty').value)
        })).then(function (rows) {
            if (ldr !== mine || !ldr) return;
            ldr.rows = rows || [];
            ldr.checked = {};
            var seen = {};
            ldr.headers = ldr.rows.filter(function (r) {
                var id = C.intOf(r.PurchasePreBillHeaderId);
                if (seen[id]) return false; seen[id] = 1; return true;
            });
            renderLoader();
            renderLoaderDetail();
        }).catch(fail);
    }
    function loaderShow() { if (ldr) loaderQuery(); }                                           // btngrnlod_Click
    /** ldr:btnReset_Click — From = financial year start; doc range and Item cleared (Bill To Party kept). */
    function loaderNew() {
        if (!ldr) return;
        if (look.financialYearStart) $id('ldrFromDate').value = look.financialYearStart;
        $id('txtDocNoFrom').value = '';
        $id('txtDocNoTo').value = '';
        $id('CmbItemName').value = '0';
        loaderQuery();
    }
    function loaderRefresh() {                                                                   // ldr:btnRefresh_Click
        C.getJson(api + '/loader/combos').then(bindLoaderCombos).catch(fail);
    }

    /** ldr:grdSettings — Select column first, DocNo a link (the 147 slip). */
    var LDR_COLS = ['DocNo', 'DocDate', 'BillToPartyName', 'VendorSupplierName', 'ReferencePartyName', 'PartyBillNo', 'PartyBillDate',
        'VehicleNo', 'BiltyNo', 'DeliveryTerm', 'CityName', 'RemarksHeader', 'EntryDate', 'EntryUserName', 'ModifyDate',
        'ModifyUserName', 'NoOfAttachments'];
    function renderLoader() {
        var t = $id('ldrGrd');
        if (!ldr.headers.length) { clearTable('ldrGrd'); return; }
        t.tHead.innerHTML = '<tr><th>Select</th>' + LDR_COLS.map(function (k) { return '<th>' + esc(caption(k)) + '</th>'; }).join('') + '</tr>';
        t.tBodies[0].innerHTML = ldr.headers.map(function (r, i) {
            var id = C.intOf(r.PurchasePreBillHeaderId);
            return '<tr data-i="' + i + '"><td style="text-align:center"><input type="checkbox" data-h="' + id + '"' + (ldr.checked[id] ? ' checked' : '') + '></td>' +
                LDR_COLS.map(function (k) {
                    var v = r[k];
                    if (k === 'DocNo') return '<td class="num"><a href="#" data-slip="' + id + '">' + esc(v) + '</a></td>';
                    if (k === 'DocDate' || k === 'PartyBillDate') return '<td>' + esc(dmy(v)) + '</td>';
                    if (k === 'EntryDate' || k === 'ModifyDate') return '<td>' + esc(dmyt(v)) + '</td>';
                    if (k === 'NoOfAttachments') return '<td class="num" title="Attachments are not ported">' + esc(v) + '</td>';
                    return '<td>' + esc(v === null || v === undefined ? '' : v) + '</td>';
                }).join('') + '</tr>';
        }).join('');
        t.querySelectorAll('input[data-h]').forEach(function (cb) {
            cb.onchange = function () {                                                          // ldr:grd_RowCheckStateChanged
                ldr.checked[C.intOf(cb.getAttribute('data-h'))] = cb.checked;
                renderLoaderDetail();
            };
        });
        t.querySelectorAll('a[data-slip]').forEach(function (a) {
            a.onclick = function (e) {                                                           // ldr:grd_LinkClicked DocNo
                e.preventDefault();
                C.getJson(api + '/prebill/' + a.getAttribute('data-slip') + '/slip')
                    .then(function (d) { printWindow('147 - Purchase Pre-Bill Slip', d); }).catch(fail);
            };
        });
    }

    /** ldr:DetailGridBind:423 / GridDetailSetting — every row of the checked headers, all checked. */
    var LDR_DET_COLS = ['PurchaseDemandNo', 'PurchaseDemandDate', 'PurchaseDemandQty', 'PurchaseOrderNo', 'PurchaseOrderDate',
        'PurchaseOrderQty', 'WareHouseName', 'ItemCode', 'ItemName', 'Uom', 'ItemCondition', 'Qty', 'QtyUsedInChallan',
        'BalanceQty', 'RemarksDetail'];
    function loaderDetailRows() {
        return ldr.rows.filter(function (r) { return ldr.checked[C.intOf(r.PurchasePreBillHeaderId)]; });
    }
    function renderLoaderDetail() {
        var t = $id('ldrGrdDetail'), rows = loaderDetailRows();
        ldr.detailChecked = {};
        if (!rows.length) { clearTable('ldrGrdDetail'); return; }
        rows.forEach(function (r) { ldr.detailChecked[C.intOf(r.DetailId)] = true; });           // CheckAllRows
        t.tHead.innerHTML = '<tr><th>Select</th>' + LDR_DET_COLS.map(function (k) { return '<th>' + esc(caption(k)) + '</th>'; }).join('') + '</tr>';
        t.tBodies[0].innerHTML = rows.map(function (r) {
            var v = {
                PurchaseDemandNo: r.PurchaseDemandDocNo, PurchaseDemandDate: dmy(r.PurchaseDemandDocDate),
                PurchaseDemandQty: roundEven(num(r.PurchaseDemandQty)), PurchaseOrderNo: r.PurchaseOrderDocNo,
                PurchaseOrderDate: dmy(r.PurchaseOrderDocDate), PurchaseOrderQty: roundEven(num(r.PurchaseOrderQty)),
                WareHouseName: r.WareHouseName, ItemCode: r.ItemCode, ItemName: r.ItemName, Uom: r.Uom, ItemCondition: r.ItemCondition,
                Qty: netStr(r.Qty), QtyUsedInChallan: netStr(r.QtyUsedInChallan), BalanceQty: netStr(r.BalanceQty), RemarksDetail: r.RemarksDetail
            };
            return '<tr><td style="text-align:center"><input type="checkbox" data-d="' + C.intOf(r.DetailId) + '" checked></td>' +
                LDR_DET_COLS.map(function (k) {
                    var numeric = /Qty$|No$/.test(k);
                    return '<td' + (numeric ? ' class="num"' : '') + '>' + esc(v[k] === null || v[k] === undefined ? '' : v[k]) + '</td>';
                }).join('') + '</tr>';
        }).join('');
        t.querySelectorAll('input[data-d]').forEach(function (cb) {
            cb.onchange = function () { ldr.detailChecked[C.intOf(cb.getAttribute('data-d'))] = cb.checked; };
        });
    }

    /** ldr:btnLoadOnInvoice_Click_1:575 — the checked detail rows, in the loader's own row order. */
    function loaderLoad() {
        if (!ldr) return;
        var ids = {};
        loaderDetailRows().forEach(function (r) { if (ldr.detailChecked[C.intOf(r.DetailId)]) ids[C.intOf(r.DetailId)] = 1; });
        if (!Object.keys(ids).length) { alert('Check the row first in Detail Grid'); return; }
        var dtLoader = ldr.rows.filter(function (r) { return ids[C.intOf(r.DetailId)]; });
        ldr = null;
        C.closeModal('dlgPreBill');
        loadInGridDetail(dtLoader);
    }

    /** LoadInGridDetail:1937. */
    function loadInGridDetail(dt) {
        if (!dt || !dt.length) return;
        var poIds = C.distinct(dt.filter(function (r) { return r.PurchasePreBillHeaderId !== null && r.PurchasePreBillHeaderId !== undefined; }),
            'PurchasePreBillHeaderId').map(function (r) { return C.intOf(r.PurchasePreBillHeaderId); });
        $id('txtVehicleNo').value = dt[0].VehicleNo || '';
        $id('txtBiltyNo').value = dt[0].BiltyNo || '';
        setCombo('CmbDeliveryTerm', dt[0].DeliveryTermId);
        setCombo('CmbCityName', dt[0].CityId);
        var existing = {};
        st.rows.forEach(function (r) { existing[C.intOf(r.PurchasePreBillDetailId)] = 1; });
        dt.forEach(function (row) {
            var detailId = C.intOf(row.DetailId);
            if (existing[detailId]) return;
            existing[detailId] = 1;
            var balance = num(row.BalanceQty);
            st.rows.push({
                Id: 0,
                PurchaseDemandHeaderId: row.PurchaseDemandHeaderId, PurchaseDemandDetailId: row.PurchaseDemandDetailId,
                PurchaseDemandNo: row.PurchaseDemandDocNo,
                PurchasePreBillHeaderId: row.PurchasePreBillHeaderId, PurchasePreBillDetailId: detailId, PurchasePreBillNo: row.DocNo,
                ItemId: row.ItemId, ItemCode: row.ItemCode, ItemName: row.ItemName, UomId: row.UomId, Uom: row.Uom,
                UomEquivalent: row.UomEquivalent, ItemConditionId: row.ItemConditionId,
                TotalDemandQty: num(row.PurchaseDemandQty), PreBillQty: num(row.Qty), UsedQty: num(row.QtyUsedInChallan),
                BalanceQty: balance, ThisQty: balance,
                PartyBillNo: row.PartyBillNo, PartyBillDate: row.PartyBillDate,
                VendorSupplierId: row.VendorSupplierId, ReferencePartyId: row.ReferencePartyId,
                VendorSupplier: C.intOf(row.VendorSupplierId) > 0 ? row.VendorSupplierName : row.ReferencePartyName,
                Remarks: row.RemarksDetail || ''
            });
        });
        renderGrid();
        if (!poIds.length) { renderExp(); return; }
        C.getJson(api + '/loader/expenses' + C.qs({ headerIds: poIds.join(',') })).then(function (items) {   // LoadExpData:2002
            var have = {};
            st.exp.forEach(function (e) { have[C.intOf(e.PreBillExpenseId)] = 1; });
            (items || []).forEach(function (x) {
                var expId = C.intOf(x.PurchasePreBillExpenseDetailId);
                if (have[expId]) return;
                have[expId] = 1;
                st.exp.push({ Id: 0, PreBillId: x.PurchasePreBillHeaderId, PreBillExpenseId: expId, PreBillNo: x.PreBillNo,
                    ItemId: x.OtherItemId, Qty: num(x.Qty), Rate: num(x.Rate), Amount: num(x.Amount), Remarks: x.Remarks || '' });
            });
            st.exp = st.exp.filter(function (e) {
                return !(C.intOf(e.PreBillId) === 0 && C.intOf(e.PreBillExpenseId) === 0 && C.intOf(e.ItemId) === 0 &&
                    num(e.Qty) === 0 && num(e.Rate) === 0 && num(e.Amount) === 0);
            });
            renderExp();
        }).catch(function (e) { renderExp(); fail(e); });
    }

    /* ------------------------------------------------------------------ init (Form_Load) */

    function init() {
        st = blankState();
        dateTypeFill();
        renderAll();
        return C.getJson(api + '/lookups').then(function (l) {
            look = l;
            applyRights();
            bindDeliveryTerm();
            bindCity();                                                                         // no "City Area" at Form_Load
            bindHistoryCombos();
            if (firstLoad) {
                if (!st.id) $id('txtDocNo').value = l.docNo;
                $id('txtDocDate').value = today();                                               // designer default: Now
                histDefaults();
                firstLoad = false;
            }
            renderAll();
        }).catch(fail);
    }

    window.DcPreBill = {
        reset: reset, refresh: refresh, save: save, update: update, remove: remove, print: print,
        openLoader: openLoader, closeLoader: closeLoader, loaderNew: loaderNew, loaderRefresh: loaderRefresh,
        loaderShow: loaderShow, loaderLoad: loaderLoad,
        showHistory: showHistory, resetHistory: resetHistory, refreshHistory: refreshHistory, dateTypeChanged: dateTypeChanged,
        tab: tab, open: readById
    };

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', init); else init();
})();
