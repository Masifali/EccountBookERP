/* ============================================================================
 * Labour Wages Manual  (frmWagesBillManual, screen 187, DocumentTypeId 810)
 * Ported method by method from Architecture.WinApp.Contractor_Wages\frmWagesBillManual.cs.
 * Server side: LabourWagesManualService (every procedure checked against procdure.sql).
 *
 * The desktop's MessageBox / confirm texts are used verbatim, and its quirks are kept on
 * purpose (each is listed in the project doc):
 *   - (FIXED) CalculateDetailAmount: the desktop computed the weight amount and then overwrote it with
 *     "0" by the else of the qty-mode if, so the "+" button answers "Amount Field Required".
 *   - grid CellUpdated: editing any column other than WagesAccount / Contractor / Qty /
 *     PackSize / Item / BillWeight zeroes the row's Rate, Amount and schedule.
 *   - F1 on Contractor / Wages Activity: the qty-mode amount branch tests !WagesAmountCalculateOnQty,
 *     so in qty mode the amount becomes 0; a cancelled picker writes 0 into the cell.
 *   - WareHouse To stays hidden for every document type (the "!= 68 || != 806" test).
 *   - Ctrl+T switches tabControl1, which has one page - it does nothing.
 *   - The Wages Type box after an Edit reflects the item that was in the Item box BEFORE the
 *     row was loaded (GetRateAmount runs when Pack Size is filled, before Item is).
 * ==========================================================================*/
(function () {
'use strict';

var API = '/api/contractor-wages/labour-wages-manual';

var S = {
    canSave: false, canUpdate: false, canPrint: false, party: false,
    onQty: false, amtDigits: 0, rateDec: 2,
    contractors: [], wagesAccounts: [], items: [], packingTypes: [], warehouses: [],
    cropYears: [], jobLots: [], documentTypes: [], stockParties: [], jobOrders: [],
    recId: 0,            // RECID
    rows: [],            // dtdetail
    ghost: 0,            // loaded rows deleted from dtdetail - still counted by dtdetail.Rows.Count
    editIndex: -1,       // updateDetailIndex
    scheduleId: 0,       // WagesScheduleId
    cur: -1, curKey: '',
    rateSeq: 0,
    hist: [], histCur: -1, histDetailRefType: 0,
    busy: false
};

/* ------------------------------------------------------------------ helpers */
function el(id) { return document.getElementById(id); }
function txt(v) { return v === null || v === undefined ? '' : String(v); }
function esc(v) { return txt(v).replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;'); }
function ci(o, k) {
    if (!o) return undefined;
    if (k in o) return o[k];
    var l = k.toLowerCase();
    for (var p in o) if (p.toLowerCase() === l) return o[p];
    return undefined;
}
/* Conversion.ToDecimal / ToDouble of a text box: "" or junk is 0, thousands separators allowed. */
function dec(v) {
    if (v === null || v === undefined) return 0;
    if (typeof v === 'number') return isFinite(v) ? v : 0;
    var s = String(v).trim().replace(/,/g, '');
    if (s === '' || !/^[+-]?(\d+\.?\d*|\.\d+)$/.test(s)) return 0;
    return parseFloat(s);
}
/* Conversion.ToInt: whole-number text only; numbers round half to even. */
function toInt(v) {
    if (v === null || v === undefined || v === '') return 0;
    if (typeof v === 'boolean') return v ? 1 : 0;
    if (typeof v === 'number') return roundEven(v, 0);
    var s = String(v).trim().replace(/,/g, '');
    return /^[+-]?\d+$/.test(s) ? parseInt(s, 10) : 0;
}
function toBool(v) { if (v === true || v === 1) return true; var s = txt(v).trim().toLowerCase(); return s === 'true' || s === '1'; }
function roundEven(x, d) {
    var m = Math.pow(10, d), v = x * m, f = Math.floor(v), diff = v - f, r;
    if (Math.abs(diff - 0.5) < 1e-9) r = (f % 2 === 0) ? f : f + 1; else r = Math.round(v);
    return r / m;
}
function roundAway(x, d) {
    if (!isFinite(x)) return x;
    var m = Math.pow(10, Math.max(0, Math.min(15, d)));
    return (x < 0 ? -1 : 1) * Math.round(Math.abs(x) * m + 1e-9) / m;
}
/* .NET custom numeric formats used by the form. */
function fmtNum(v, minDec, maxDec, leadingZero) {
    v = Number(v);
    if (!isFinite(v)) return v > 0 ? '∞' : (v < 0 ? '-∞' : '');
    var r = roundAway(v, maxDec), neg = r < 0, s = Math.abs(r).toFixed(maxDec);
    var parts = s.split('.'), ip = parts[0], fp = parts[1] || '';
    while (fp.length > minDec && fp.charAt(fp.length - 1) === '0') fp = fp.slice(0, -1);
    ip = ip.replace(/\B(?=(\d{3})+(?!\d))/g, ',');
    if (!leadingZero && ip === '0') ip = '';
    var out = ip + (fp ? '.' + fp : '');
    if (out === '' ) return '';
    return (neg && /[1-9]/.test(out) ? '-' : '') + out;
}
function fmtHash4(v) { return fmtNum(v, 0, 4, false); }              // "##,#.####"
function fmtHash3(v) { return fmtNum(v, 0, 3, false); }              // "##,#.###"
function fmtHash2(v) { return fmtNum(v, 0, 2, false); }              // "#,##.##"
function fmtEdit(v)  { return fmtNum(v, 0, 2, true); }               // "#,##0.##"
function fmtRate(v)  { return fmtNum(v, S.rateDec, S.rateDec, true); }          // DecimalRateFormate
function amtFmtDec() { return (S.amtDigits >= 1 && S.amtDigits <= 4) ? S.amtDigits : 0; }
function fmtAmt(v)   { return fmtNum(v, amtFmtDec(), amtFmtDec(), true); }      // stringFormatsingle
function plainNum(v) { var n = Number(v); if (v === null || v === undefined || v === '' || !isFinite(n)) return ''; return String(n); }

function pad(n) { return (n < 10 ? '0' : '') + n; }
var MON = ['Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov','Dec'];
function toDate(v) {
    if (v === null || v === undefined || v === '') return null;
    if (typeof v === 'number') return new Date(v);
    var s = String(v).replace(' ', 'T');
    var m = /^(\d{4})-(\d{2})-(\d{2})(?:T(\d{2}):(\d{2})(?::(\d{2}))?)?/.exec(s);
    if (m) return new Date(+m[1], +m[2] - 1, +m[3], +(m[4] || 0), +(m[5] || 0), +(m[6] || 0));
    var d = new Date(s); return isNaN(d) ? null : d;
}
function isoDate(d) { return d ? d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()) : ''; }
function isoDateTime(d) { return d ? isoDate(d) + 'T' + pad(d.getHours()) + ':' + pad(d.getMinutes()) + ':' + pad(d.getSeconds()) : ''; }
function ddMMMyyyy(d) { return d ? pad(d.getDate()) + '-' + MON[d.getMonth()] + '-' + d.getFullYear() : ''; }
function ddMMMyyyyhm(d) {
    if (!d) return '';
    var h = d.getHours(), ap = h < 12 ? 'AM' : 'PM'; h = h % 12; if (h === 0) h = 12;
    return ddMMMyyyy(d) + ' ' + pad(h) + ':' + pad(d.getMinutes()) + ' ' + ap;
}
/* txtDocdate.Value: the picked day with the current time of day. */
function docDateValue() {
    var v = el('txtDocDate').value, n = new Date();
    if (!v) return isoDateTime(n);
    return v + 'T' + pad(n.getHours()) + ':' + pad(n.getMinutes()) + ':' + pad(n.getSeconds());
}

function getJson(url) {
    return fetch(url, { credentials: 'same-origin', headers: { 'Accept': 'application/json' } })
        .then(function (r) {
            return r.json().catch(function () { return { success: false, message: 'HTTP ' + r.status }; })
                .then(function (j) { if (!r.ok) throw new Error((j && j.message) || ('HTTP ' + r.status)); return j; });
        });
}
function postJson(url, body) {
    return fetch(url, {
        method: 'POST', credentials: 'same-origin',
        headers: { 'Content-Type': 'application/json', 'Accept': 'application/json' },
        body: JSON.stringify(body)
    }).then(function (r) {
        return r.json().catch(function () { return { success: false, message: 'HTTP ' + r.status }; })
            .then(function (j) { if (!r.ok) throw new Error((j && j.message) || ('HTTP ' + r.status)); return j; });
    });
}
function qs(o) {
    var a = [];
    for (var k in o) if (o[k] !== undefined && o[k] !== null && o[k] !== '') a.push(encodeURIComponent(k) + '=' + encodeURIComponent(o[k]));
    return a.join('&');
}
function box(m) { window.alert(m); }                                   // MessageBox.Show

/* ------------------------------------------------------------------ combos (select2) */
function listText(list, idKey, textKey, id) {
    for (var i = 0; i < list.length; i++) if (String(ci(list[i], idKey)) === String(id)) return txt(ci(list[i], textKey));
    return null;
}
/* DDL.BindDDLNew / BindDDL, keeping the previous value when it is still in the list. */
function bind(id, list, idKey, textKey) {
    var sel = el(id), prev = sel.value;
    var h = ['<option value=""></option>'];
    list.forEach(function (r) { h.push('<option value="' + esc(ci(r, idKey)) + '">' + esc(ci(r, textKey)) + '</option>'); });
    sel.innerHTML = h.join('');
    var keep = prev && listText(list, idKey, textKey, prev) !== null;
    $(sel).val(keep ? prev : '').trigger('change.select2');
}
function setCombo(id, v) {
    var sel = el(id), has = false;
    for (var i = 0; i < sel.options.length; i++) if (sel.options[i].value === String(v)) { has = true; break; }
    $(sel).val(has && v !== null && v !== undefined ? String(v) : '').trigger('change.select2');
}
function comboVal(id) { return toInt(el(id).value); }
function comboText(id) { var s = el(id); return s.selectedIndex >= 0 && s.value !== '' ? s.options[s.selectedIndex].text.trim() : ''; }

/* ------------------------------------------------------------------ tabs */
window.wmTab = function (i) {
    el('viewForm').style.display = i === 0 ? '' : 'none';
    el('viewHistory').style.display = i === 1 ? '' : 'none';
    el('tabForm').classList.toggle('active', i === 0);
    el('tabHistory').classList.toggle('active', i === 1);
    if (i === 1) el('FromDateHistory').focus(); else el('txtDocDate').focus();        // tabControl1_SelectedIndexChanged
};

/* ------------------------------------------------------------------ load */
function applyLists(d) {
    if ('wagesAmountCalculateOnQty' in d) S.onQty = !!d.wagesAmountCalculateOnQty;
    if ('amountDecimals' in d) S.amtDigits = toInt(d.amountDecimals);
    if ('rateDecimals' in d) S.rateDec = toInt(d.rateDecimals);
    S.contractors = d.contractors || S.contractors;
    S.wagesAccounts = d.wagesAccounts || S.wagesAccounts;
    S.items = d.items || S.items;
    S.packingTypes = d.packingTypes || S.packingTypes;
    S.warehouses = d.warehouses || S.warehouses;
    S.cropYears = d.cropYears || S.cropYears;
    S.jobLots = d.jobLots || S.jobLots;
    if (d.documentTypes) S.documentTypes = d.documentTypes;
    bind('cmbContractor', S.contractors, 'Id', 'CompanyName');
    bind('cmbWagesAccount', S.wagesAccounts, 'Id', 'WagesAccountName');
    /* ItemFill: the display column is decided by rdbtnItemName at fill time only. */
    bind('comItem', S.items, 'Id', el('rdbtnItemName').checked ? 'ItemName' : 'ItemCode');
    bind('cmbPackingType', S.packingTypes, 'Id', 'PackTypeDesc');
    bind('cmbWarehouseFrom', S.warehouses, 'Id', 'WareHouseName');
    bind('cmbWareHouseTo', S.warehouses, 'Id', 'WareHouseName');
    bind('cmbCropYear', S.cropYears, 'Id', 'CropYear');
    bind('cmbJobLot', S.jobLots, 'Id', 'JobLotDescription');
    if (d.documentTypes) documentTypeFill(d.documentTypes);
    if (d.stockParties) { S.stockParties = d.stockParties; bind('cmbStockParty', S.stockParties, 'Id', 'CompanyName'); }
}
function documentTypeFill(list) {
    S.documentTypes = list;
    bind('cmbReferenceDocType', list, 'Id', 'DocumentType');
    bind('CmbDocumentTypeHistory', list, 'Id', 'DocumentType');
}

function load() {
    getJson(API + '/load').then(function (d) {
        S.canSave = !!d.canSave; S.canUpdate = !!d.canUpdate; S.canPrint = !!d.canPrint;
        S.party = !!d.partyProcessing;
        el('btnSave').disabled = !S.canSave;
        el('btnUpdate').disabled = !S.canUpdate;
        el('btnPrint').disabled = !S.canPrint;
        el('chkPreview').checked = S.canPrint;
        el('chkPreview').disabled = !S.canPrint;
        el('wrapIsCompany').classList.toggle('wm-hidden', !S.party);
        el('wrapStockParty').classList.toggle('wm-hidden', !S.party);
        applyLists(d);
        if (toInt(d.docNo) > 0) el('txtDocNo').value = String(d.docNo);
        renderDetail();
        el('txtDocDate').focus();
    }).catch(function (e) { box(e.message); });
}

/* ------------------------------------------------------------------ Document Type leave */
function refDocTypeLeave() {
    var t = comboVal('cmbReferenceDocType');
    if (t === 112 || t === 80) {
        showJobOrder(true);
        jobOrderNoFill();
    } else {
        showJobOrder(false);
        setCombo('cmbJobOrder', '');
    }
    /* (value != 68 || value != 806) is always true: WareHouse To stays hidden. */
    el('wrapWhTo').classList.add('wm-hidden');
    el('wrapWhFrom').style.width = '344px';
    renderDetail();
}
function showJobOrder(on) {
    el('labelJobOrder').classList.toggle('wm-hidden', !on);
    el('wrapJobOrder').classList.toggle('wm-hidden', !on);
    el('cmbJobOrder').disabled = !on;
}
function jobOrderNoFill(thenValue) {
    return getJson(API + '/job-orders').then(function (list) {
        S.jobOrders = list || [];
        bind('cmbJobOrder', S.jobOrders, 'Id', 'PlanCode');
        if (thenValue !== undefined) setCombo('cmbJobOrder', thenValue);
    }).catch(function (e) { box(e.message); });
}

/* ------------------------------------------------------------------ detail entry box */
function freeOfCost(date, refType, itemId, waId) {
    return getJson(API + '/free-of-cost?' + qs({ docDate: date, refDocumentTypeId: refType, itemId: itemId, wagesAccountId: waId }))
        .then(function (r) { return !!r.freeOfCost; });
}
function wagesRate(date, pack, waId, contractorId) {
    return getJson(API + '/wages-rate?' + qs({ docDate: date, packSize: pack, wagesAccountId: waId, contractorId: contractorId }));
}

/* GetRateAmount :1687, followed (as in every caller) by CalculateDetailWeight / CalculateDetailAmount. */
function getRateAmount() {
    var my = ++S.rateSeq;
    var wa = comboVal('cmbWagesAccount');
    calculateDetailWeight(); calculateDetailAmount();
    if (wa <= 0) {
        S.scheduleId = 0;
        el('txtWagesRate').value = fmtRate(0);
        calculateDetailAmount();
        return Promise.resolve();
    }
    var date = docDateValue(), refType = comboVal('cmbReferenceDocType'), item = comboVal('comItem');
    var pack = dec(el('txtPackSize').value), contractor = comboVal('cmbContractor');
    return freeOfCost(date, refType, item, wa).then(function (foc) {
        if (my !== S.rateSeq) return;
        el('txtWagesType').value = foc ? 'Free Of Cost' : 'Regular';
        if (foc) {
            el('txtWagesRate').value = fmtRate(0);
            calculateDetailAmount();
            el('txtAmount').value = fmtAmt(0);
            S.scheduleId = 0;
            calculateDetailWeight(); calculateDetailAmount();
            return;
        }
        S.scheduleId = 0;
        return wagesRate(date, pack, wa, contractor).then(function (d) {
            if (my !== S.rateSeq) return;
            S.scheduleId = toInt(d.scheduleId);
            el('txtWagesRate').value = fmtRate(dec(d.rate));
            calculateDetailWeight(); calculateDetailAmount();
        });
    }).catch(function (e) { if (my === S.rateSeq) box(e.message); });
}

/* CalculateDetailWeight :1628 - "##,#.###", and 0 formats as "" */
function calculateDetailWeight() {
    var qty = dec(el('txtQty').value), unit = dec(el('txtPackSize').value);
    el('txtWeight').value = (unit > 0 && qty > 0) ? fmtHash3(unit * qty) : fmtHash3(0);
}
/* CalculateDetailAmount :1652. FIXED (user request 2026-09-29): the desktop has two separate
   ifs, so the qty-mode else overwrote the weight-mode amount with "0" and weight mode could
   never add a row. Here the weight branch is an else-if of the same chain. */
function calculateDetailAmount() {
    var qty = dec(el('txtQty').value), unit = dec(el('txtPackSize').value);
    var rate = dec(el('txtWagesRate').value), w = dec(el('txtWeight').value), amount;
    if (rate > 0 && w > 0 && unit > 0 && !S.onQty) {
        amount = roundAway(w / unit * rate, S.amtDigits);
        el('txtAmount').value = fmtAmt(amount);
    } else if (rate > 0 && qty > 0 && S.onQty) {
        amount = roundAway(qty * rate, S.amtDigits);
        el('txtAmount').value = fmtAmt(amount);
    } else {
        el('txtAmount').value = '0';
    }
}

/* FormValidationDetail :1203 */
function formValidationDetail() {
    function empty0(id) { var t = el(id).value.trim(); return t === '' || t === '0'; }
    if (comboText('cmbContractor') === '' || comboVal('cmbContractor') === 0) { box('Contractor Field Required'); $('#cmbContractor').select2('open'); return false; }
    if (comboText('cmbWagesAccount') === '' || comboVal('cmbWagesAccount') === 0) { box('Wages Account Field Required'); $('#cmbWagesAccount').select2('open'); return false; }
    if (empty0('txtPackSize')) { box('Pack Size Field Required'); el('txtPackSize').focus(); return false; }
    if (empty0('txtQty')) { box('Qty Field Required'); el('txtQty').focus(); return false; }
    if (empty0('txtWeight')) { box('Weight Field Required'); el('txtWeight').focus(); return false; }
    if (empty0('txtWagesRate')) { box('WagesRate Field Required'); el('txtWagesRate').focus(); return false; }
    if (empty0('txtAmount')) { box('Amount Field Required'); el('txtAmount').focus(); return false; }
    return true;
}

function itemCodeOf(id) { var t = listText(S.items, 'Id', 'ItemCode', id); return t === null ? '' : t; }

function entryRow(wagesType) {
    var item = comboVal('comItem');
    return {
        date: docDateValue(),
        contractorId: comboVal('cmbContractor'),
        wagesAccountId: comboVal('cmbWagesAccount'),
        packSize: dec(el('txtPackSize').value),
        qty: dec(el('txtQty').value),
        billWeight: dec(el('txtWeight').value),
        rate: dec(el('txtWagesRate').value),
        amount: dec(el('txtAmount').value),
        itemId: item,
        itemCode: item > 0 ? itemCodeOf(item) : '',
        cropYearId: String(comboVal('cmbCropYear')),
        packingTypeId: comboVal('cmbPackingType'),
        jobLotId: comboVal('cmbJobLot'),
        wareHouseFromId: comboVal('cmbWarehouseFrom'),
        wareHouseToId: comboVal('cmbWareHouseTo'),
        wagesType: wagesType,
        isCompany: el('chkIsCompany').checked,
        wagesScheduleId: S.scheduleId
    };
}

/* btnAdd_Click :749 */
window.wmAdd = function () {
    if (!formValidationDetail()) return;
    freeOfCost(docDateValue(), comboVal('cmbReferenceDocType'), comboVal('comItem'), comboVal('cmbWagesAccount'))
        .then(function (foc) {
            var r = entryRow(foc ? 'Free Of Cost' : 'Regular');
            r.id = 0;
            S.rows.push(r);
            renderDetail();
            formRestDetail();
        }).catch(function (e) { box(e.message); });
};

/* btnUpdateDetail_Click :809 */
window.wmUpdateDetail = function () {
    if (!formValidationDetail()) return;
    var idx = S.editIndex;
    freeOfCost(docDateValue(), comboVal('cmbReferenceDocType'), comboVal('comItem'), comboVal('cmbWagesAccount'))
        .then(function (foc) {
            var r = S.rows[idx];
            if (!r) { box('There is no row at position ' + idx + '.'); return; }
            var n = entryRow(foc ? 'Free Of Cost' : 'Regular');
            for (var k in n) r[k] = n[k];
            el('btnAdd').style.display = '';
            el('btnUpdateDetail').style.display = 'none';
            el('btnCancelUpdateDetail').style.display = 'none';
            renderDetail();
            formRestDetail();
            $('#cmbContractor').select2('focus');
        }).catch(function (e) { box(e.message); });
};

/* btnCancelUpdateDetial_Click :853 - the boxes are left as they are. */
window.wmCancelUpdateDetail = function () {
    el('btnAdd').style.display = '';
    el('btnUpdateDetail').style.display = 'none';
    el('btnCancelUpdateDetail').style.display = 'none';
    S.editIndex = -1;
};

/* grdwagesDetail_DoubleClick :775 - the current row into the entry box. */
function editRow(i) {
    var r = S.rows[i];
    if (!r) return;
    S.editIndex = i;
    var prevItem = comboVal('comItem');
    setCombo('cmbContractor', r.contractorId);
    setCombo('cmbWagesAccount', r.wagesAccountId);
    /* txtPackSize.Text fires GetRateAmount with the combos just set and the OLD item. The rate
       and amount it writes are overwritten below, so only its Wages Type text survives. */
    S.rateSeq++;
    var wa = toInt(r.wagesAccountId);
    if (wa > 0) {
        var my = S.rateSeq;
        freeOfCost(docDateValue(), comboVal('cmbReferenceDocType'), prevItem, wa).then(function (foc) {
            if (my === S.rateSeq) el('txtWagesType').value = foc ? 'Free Of Cost' : 'Regular';
        }).catch(function () { /* the box only */ });
    }
    el('txtPackSize').value = fmtEdit(r.packSize);
    el('txtQty').value = fmtEdit(r.qty);
    el('txtWeight').value = fmtEdit(r.billWeight);
    el('txtWagesRate').value = fmtRate(r.rate);
    el('txtAmount').value = fmtAmt(r.amount);
    setCombo('comItem', r.itemId);
    setCombo('cmbCropYear', r.cropYearId);
    setCombo('cmbPackingType', r.packingTypeId);
    setCombo('cmbJobLot', r.jobLotId);
    setCombo('cmbWarehouseFrom', r.wareHouseFromId);
    setCombo('cmbWareHouseTo', r.wareHouseToId);
    el('chkIsCompany').checked = toBool(r.isCompany);
    S.scheduleId = toInt(r.wagesScheduleId);
    el('btnAdd').style.display = 'none';
    el('btnUpdateDetail').style.display = '';
    el('btnCancelUpdateDetail').style.display = '';
    $('#cmbContractor').select2('focus');
}

/* FormRestDetail :1294 */
function formRestDetail() {
    S.rateSeq++;
    S.editIndex = -1;
    S.scheduleId = 0;
    ['cmbContractor', 'cmbWagesAccount', 'comItem', 'cmbCropYear', 'cmbPackingType', 'cmbJobLot',
     'cmbWarehouseFrom', 'cmbWareHouseTo'].forEach(function (id) { setCombo(id, ''); });
    ['txtPackSize', 'txtQty', 'txtWeight', 'txtWagesRate', 'txtAmount', 'txtWagesType'].forEach(function (id) { el(id).value = ''; });
    el('btnAdd').style.display = '';
    el('btnUpdateDetail').style.display = 'none';
    el('btnCancelUpdateDetail').style.display = 'none';
}

/* ------------------------------------------------------------------ the Regular Wages grid */
var VLIST = {
    contractorId:   function () { return [S.contractors, 'Id', 'CompanyName']; },
    wagesAccountId: function () { return [S.wagesAccounts, 'Id', 'WagesAccountName']; },
    itemId:         function () { return [S.items, 'Id', 'ItemName']; },
    cropYearId:     function () { return [S.cropYears, 'Id', 'CropYear']; },
    packingTypeId:  function () { return [S.packingTypes, 'Id', 'PackTypeDesc']; },
    jobLotId:       function () { return [S.jobLots, 'Id', 'JobLotDescription']; },
    wareHouseFromId:function () { return [S.warehouses, 'Id', 'WareHouseName']; },
    wareHouseToId:  function () { return [S.warehouses, 'Id', 'WareHouseName']; }
};
/* The key each column had on the desktop grid - CellUpdated and F1 test these names. */
var DESK = { contractorId: 'ContractorId', wagesAccountId: 'WagesAccountId', packSize: 'PackSize', qty: 'Qty',
             billWeight: 'BillWeight', itemId: 'ItemId', cropYearId: 'CropYearId', packingTypeId: 'PackingTypeId',
             jobLotId: 'JobLotId', wareHouseFromId: 'WareHouseFromId', wareHouseToId: 'WareHouseToId', isCompany: 'IsCompany' };

function columns() {
    var c = [
        { k: 'date', t: 'Date' },
        { k: 'contractorId', t: 'Contractor Name', v: true },
        { k: 'wagesAccountId', t: 'Labour / Wages Activity', v: true },
        { k: 'packSize', t: 'PackSize', n: 'plain' },
        { k: 'qty', t: 'Qty', n: 'h4', sum: true },
        { k: 'billWeight', t: 'BillWeight', n: 'h4', sum: true },
        { k: 'rate', t: 'Rate', ro: 'rate', avg: true },
        { k: 'amount', t: 'Amount', ro: 'amt', sum: true },
        { k: 'itemId', t: 'Item Name', v: true },
        { k: 'itemCode', t: 'ItemCode', ro: 'text' },
        { k: 'cropYearId', t: 'Crop Year', v: true },
        { k: 'packingTypeId', t: 'Packing Type', v: true },
        { k: 'jobLotId', t: 'Job Lot', v: true },
        { k: 'wareHouseFromId', t: 'WareHouse From', v: true },
        { k: 'wagesType', t: 'WagesType', ro: 'text' }
    ];
    if (S.party) c.push({ k: 'isCompany', t: 'IsCompany', b: true });
    return c;
}
/* The text a value-list cell shows: the list's text, or the raw value when it is not in the list
   (a saved row's Crop is the crop TEXT, not an id). */
function cellText(k, v) {
    var L = VLIST[k]();
    var t = listText(L[0], L[1], L[2], v);
    if (t !== null) return t;
    var s = txt(v);
    return (s === '0') ? '' : s;
}

function renderDetail() {
    var cols = columns(), t = el('grdDetail');
    var h = '<tr><th>X</th><th>Add</th><th>Edit</th>' + cols.map(function (c) { return '<th>' + esc(c.t) + '</th>'; }).join('') + '</tr>';
    t.tHead.innerHTML = h;
    var body = [];
    S.rows.forEach(function (r, i) {
        var cls = (r.wagesType === 'Free Of Cost' ? 'foc ' : '') + (i === S.cur ? 'cur' : '');
        var tds = ['<td class="btncell"><button type="button" class="lw-row-btn" data-act="Delete" data-i="' + i + '">X</button></td>',
                   '<td class="btncell"><button type="button" class="lw-row-btn" data-act="Add" data-i="' + i + '">Add</button></td>',
                   '<td class="btncell"><button type="button" class="lw-row-btn" data-act="Edit" data-i="' + i + '">Edit</button></td>'];
        cols.forEach(function (c) {
            var v = r[c.k], inner;
            if (c.k === 'date') inner = esc(ddMMMyyyy(toDate(v)));
            else if (c.v) {
                var L = VLIST[c.k](), opts = ['<option value=""></option>'], found = false;
                L[0].forEach(function (o) {
                    var id = txt(ci(o, L[1])), sel = id === txt(v);
                    if (sel) found = true;
                    opts.push('<option value="' + esc(id) + '"' + (sel ? ' selected' : '') + '>' + esc(ci(o, L[2])) + '</option>');
                });
                if (!found && txt(v) !== '' && txt(v) !== '0')
                    opts.push('<option value="' + esc(v) + '" selected>' + esc(v) + '</option>');
                inner = '<select data-i="' + i + '" data-k="' + c.k + '" title="F1: pick from a list">' + opts.join('') + '</select>';
            } else if (c.n) {
                var shown = c.n === 'h4' ? fmtHash4(v) : plainNum(v);
                inner = '<input type="text" class="wm-dec" data-i="' + i + '" data-k="' + c.k + '" value="' + esc(shown) + '">';
            } else if (c.b) {
                inner = '<input type="checkbox" data-i="' + i + '" data-k="' + c.k + '"' + (toBool(v) ? ' checked' : '') + '>';
            } else if (c.ro === 'rate') inner = esc(fmtRate(v));
            else if (c.ro === 'amt') inner = esc(fmtAmt(v));
            else inner = esc(v);
            tds.push('<td class="' + ((c.n || c.ro === 'rate' || c.ro === 'amt') ? 'num' : '') + '">' + inner + '</td>');
        });
        body.push('<tr class="' + cls + '" data-i="' + i + '">' + tds.join('') + '</tr>');
    });
    t.tBodies[0].innerHTML = body.join('');
    /* totals: Qty / BillWeight / Amount sum, Rate average */
    var foot = '<tr><td colspan="3"></td>';
    cols.forEach(function (c) {
        var val = '';
        if (S.rows.length && (c.sum || c.avg)) {
            var s = 0; S.rows.forEach(function (r) { s += Number(r[c.k]) || 0; });
            if (c.avg) val = fmtRate(s / S.rows.length);
            else if (c.ro === 'amt') val = fmtAmt(s);
            else val = fmtHash4(s);
        }
        foot += '<td>' + esc(val) + '</td>';
    });
    t.tFoot.innerHTML = foot + '</tr>';
}

/* grdwagesDetail_CellUpdated :1012 */
function cellUpdated(i, key) {
    var r = S.rows[i];
    if (!r) return Promise.resolve();
    var K = DESK[key] || key;
    var rate = 0, sched = 0;
    var refType = comboVal('cmbReferenceDocType');
    var p = Promise.resolve();
    if (K === 'ContractorId' || K === 'WagesAccountId') {
        p = p.then(function () { return freeOfCost(r.date, refType, toInt(r.itemId), toInt(r.wagesAccountId)); })
             .then(function (foc) { r.wagesType = foc ? 'Free Of Cost' : 'Regular'; });
    }
    return p.then(function () {
        var qty = Number(r.qty) || 0, pack = Number(r.packSize) || 0, bw = Number(r.billWeight) || 0;
        if ((K === 'Qty' || K === 'PackSize') && qty > 0 && pack > 0) r.billWeight = roundEven(qty * pack, 2);
        if (K === 'BillWeight' && bw > 0 && pack > 0) r.qty = roundEven(bw / pack, 2);
        if (['WagesAccountId', 'ContractorId', 'Qty', 'PackSize', 'ItemId', 'BillWeight'].indexOf(K) >= 0) {
            if (r.wagesType === 'Free Of Cost') {
                r.rate = 0; r.amount = 0; r.wagesScheduleId = 0;
            } else {
                return wagesRate(r.date, Number(r.packSize) || 0, toInt(r.wagesAccountId), toInt(r.contractorId))
                    .then(function (d) { rate = dec(d.rate); sched = toInt(d.scheduleId); });
            }
        }
    }).then(function () {
        if (rate > 0) {
            r.rate = rate; r.wagesScheduleId = sched;
            var bw = Number(r.billWeight) || 0, qty = Number(r.qty) || 0, pack = Number(r.packSize) || 0;
            if (bw > 0 && !S.onQty) r.amount = roundEven(bw / pack * rate, 2);
            else if (qty > 0 && S.onQty) r.amount = roundEven(qty * rate, 2);
            else r.amount = 0;
        } else {
            r.rate = 0; r.amount = 0; r.wagesScheduleId = 0;
        }
        if (K === 'ItemId' && listText(S.items, 'Id', 'Id', toInt(r.itemId)) !== null) r.itemCode = itemCodeOf(toInt(r.itemId));
        renderDetail();
    }).catch(function (e) { renderDetail(); box(e.message); });
}

/* grdwagesDetail_ColumnButtonClick :1119 and the Ctrl+Space / Ctrl+Delete keys. */
function deleteRow(i) {
    var r = S.rows[i];
    if (!r) return;
    if (S.rows.length + S.ghost <= 1) { box('You Can Not Delete All rows'); return; }
    if (toInt(r.id) > 0) {
        if (!window.confirm('Are you sure to Delete?')) return;
        S.ghost++;               // DataRow.Delete() on a loaded row: still in dtdetail.Rows.Count
    }
    S.rows.splice(i, 1);
    if (S.cur >= S.rows.length) S.cur = S.rows.length - 1;
    renderDetail();
}
/* AddRowInGrid :1163 - a copy of the current row with Id 0. */
function addRowInGrid(i) {
    var r = S.rows[i];
    if (!r) return;
    var c = {}; for (var k in r) c[k] = r[k];
    c.id = 0;
    S.rows.push(c);
    renderDetail();
}

function gridEvents() {
    var t = el('grdDetail');
    t.addEventListener('click', function (e) {
        var tr = e.target.closest('tr[data-i]');
        if (tr) { S.cur = +tr.getAttribute('data-i'); markCur(); }
        var b = e.target.closest('button[data-act]');
        if (!b) return;
        var i = +b.getAttribute('data-i'), a = b.getAttribute('data-act');
        if (a === 'Delete') deleteRow(i);
        if (a === 'Add') addRowInGrid(i);
        if (a === 'Edit') editRow(i);
    });
    t.addEventListener('dblclick', function (e) {
        var tr = e.target.closest('tbody tr[data-i]');
        if (tr && !e.target.closest('select,input,button')) editRow(+tr.getAttribute('data-i'));
    });
    t.addEventListener('focusin', function (e) {
        var x = e.target.closest('[data-k],[data-act]');
        var tr = e.target.closest('tr[data-i]');
        if (tr) { S.cur = +tr.getAttribute('data-i'); markCur(); }
        S.curKey = x ? (x.getAttribute('data-k') || x.getAttribute('data-act')) : '';
    });
    t.addEventListener('change', function (e) {
        var x = e.target.closest('[data-k]');
        if (!x) return;
        var i = +x.getAttribute('data-i'), k = x.getAttribute('data-k'), r = S.rows[i];
        if (!r) return;
        if (x.type === 'checkbox') r[k] = x.checked;
        else if (x.tagName === 'SELECT') r[k] = k === 'cropYearId' ? x.value : toInt(x.value);
        else r[k] = dec(x.value);
        cellUpdated(i, k);
    });
    t.addEventListener('keydown', gridKeyDown);
}
function markCur() {
    Array.prototype.forEach.call(el('grdDetail').tBodies[0].rows, function (tr) {
        tr.classList.toggle('cur', +tr.getAttribute('data-i') === S.cur);
    });
}

/* grdwagesDetail_KeyDown :2390 */
function gridKeyDown(e) {
    var i = S.cur, r = S.rows[i];
    if (!r) return;
    var key = S.curKey;
    if (e.ctrlKey && (e.key === ' ' || e.code === 'Space')) {
        e.preventDefault();
        if (key === 'Delete') deleteRow(i);
        if (key === 'Edit') editRow(i);
        if (key === 'Add') addRowInGrid(i);
        return;
    }
    if (e.ctrlKey && e.key === 'Delete') { e.preventDefault(); deleteRow(i); return; }
    if (e.ctrlKey && e.key === 'Enter') { e.preventDefault(); e.stopPropagation(); editRow(i); return; }
    if (e.key === 'F1') {
        e.preventDefault();
        var K = DESK[key] || '';
        var map = { ContractorId: 1, WagesAccountId: 1, ItemId: 1, CropYearId: 1, PackingTypeId: 1, JobLotId: 1, WareHouseFromId: 1, WareHouseToId: 1 };
        if (!map[K]) return;
        var L = VLIST[key]();
        picker(L[0], L[1], L[2], function (id) {
            r[key] = key === 'cropYearId' ? String(id) : id;                    // a closed picker returns 0
            if (K === 'ItemId' && listText(S.items, 'Id', 'Id', toInt(r.itemId)) !== null) r.itemCode = itemCodeOf(toInt(r.itemId));
            if (K === 'ContractorId' || K === 'WagesAccountId') f1Rate(r).then(renderDetail);
            else renderDetail();
        });
    }
}
/* The F1 branch's own rate block (:2549) - its qty branch tests !WagesAmountCalculateOnQty. */
function f1Rate(r) {
    var rate = 0, sched = 0;
    return freeOfCost(r.date, comboVal('cmbReferenceDocType'), toInt(r.itemId), toInt(r.wagesAccountId)).then(function (foc) {
        r.wagesType = foc ? 'Free Of Cost' : 'Regular';
        if (foc) { r.rate = 0; r.amount = 0; return; }
        return wagesRate(r.date, Number(r.packSize) || 0, toInt(r.wagesAccountId), toInt(r.contractorId))
            .then(function (d) { rate = dec(d.rate); sched = toInt(d.scheduleId); });
    }).then(function () {
        if (rate > 0) {
            r.rate = rate; r.wagesScheduleId = sched;
            var bw = Number(r.billWeight) || 0, qty = Number(r.qty) || 0, pack = Number(r.packSize) || 0;
            if (bw > 0 && !S.onQty) r.amount = roundEven(bw / pack * rate, 2);
            else if (qty > 0 && !S.onQty) r.amount = roundEven(qty * rate, 2);
            else r.amount = 0;
        } else { r.rate = 0; r.amount = 0; r.wagesScheduleId = 0; }
    }).catch(function (e) { box(e.message); });
}

/* GrdPopUp - a searchable list; closing it returns 0 like an unset ReturnId. */
function picker(list, idKey, textKey, done) {
    var back = document.createElement('div');
    back.className = 'wm-modal-back';
    back.innerHTML = '<div class="wm-modal" role="dialog"><header><span>Select</span><span style="cursor:pointer" data-x>&times;</span></header>'
        + '<div class="body"><input type="text" class="win-input" placeholder="Search" style="width:100%;margin-bottom:4px;">'
        + '<table class="win-grid-table"><thead><tr><th>Id</th><th>Name</th></tr></thead><tbody></tbody></table></div></div>';
    document.body.appendChild(back);
    var inp = back.querySelector('input'), tb = back.querySelector('tbody'), sel = 0, shown = [];
    function draw() {
        var q = inp.value.trim().toLowerCase();
        shown = list.filter(function (o) { return !q || txt(ci(o, textKey)).toLowerCase().indexOf(q) >= 0; });
        if (sel >= shown.length) sel = shown.length - 1; if (sel < 0) sel = 0;
        tb.innerHTML = shown.map(function (o, j) {
            return '<tr data-j="' + j + '" class="' + (j === sel ? 'sel' : '') + '"><td>' + esc(ci(o, idKey)) + '</td><td>' + esc(ci(o, textKey)) + '</td></tr>';
        }).join('');
    }
    function close(id) { document.body.removeChild(back); done(id); }
    inp.addEventListener('input', function () { sel = 0; draw(); });
    inp.addEventListener('keydown', function (e) {
        if (e.key === 'ArrowDown') { sel++; draw(); e.preventDefault(); }
        else if (e.key === 'ArrowUp') { sel--; draw(); e.preventDefault(); }
        else if (e.key === 'Enter') { e.preventDefault(); e.stopPropagation(); close(shown[sel] ? toInt(ci(shown[sel], idKey)) : 0); }
        else if (e.key === 'Escape') { e.preventDefault(); e.stopPropagation(); close(0); }
    });
    tb.addEventListener('dblclick', function (e) { var tr = e.target.closest('tr[data-j]'); if (tr) close(toInt(ci(shown[+tr.getAttribute('data-j')], idKey))); });
    tb.addEventListener('click', function (e) { var tr = e.target.closest('tr[data-j]'); if (tr) { sel = +tr.getAttribute('data-j'); draw(); } });
    back.querySelector('[data-x]').addEventListener('click', function () { close(0); });
    draw(); inp.focus();
}

/* ------------------------------------------------------------------ New / Refresh / Save */
/* FormRest :1260 - Doc Date, Is Approved and the Job Order value are left as they are. */
function formRest() {
    S.recId = 0;
    S.editIndex = -1;
    setCombo('cmbReferenceDocType', '');
    setCombo('cmbStockParty', '');
    el('txtRemarks').value = '';
    getJson(API + '/doc-no').then(function (d) { if (toInt(d.docNo) > 0) el('txtDocNo').value = String(d.docNo); })
        .catch(function (e) { box(e.message); });
    el('btnSave').style.display = '';
    el('btnUpdate').style.display = 'none';
    S.rows = []; S.ghost = 0; S.cur = -1;
    renderDetail();
    formRestDetail();
    el('txtDocDate').focus();
    showJobOrder(false);
}
window.wmNew = function () { formRest(); };

/* btnRefresh_Click :1332 */
window.wmRefresh = function () {
    getJson(API + '/refresh').then(function (d) { applyLists(d); renderDetail(); }).catch(function (e) { box(e.message); });
};

/* FormValidation :1181 */
function formValidation() {
    var dn = el('txtDocNo').value.trim();
    if (dn === '' || dn === '0') { box('Document Number Field Required'); el('txtDocNo').focus(); return false; }
    if (comboText('cmbReferenceDocType') === '' || comboVal('cmbReferenceDocType') === 0) {
        box('Document Type Field Required'); $('#cmbReferenceDocType').select2('open'); return false;
    }
    var t = comboVal('cmbReferenceDocType');
    if ((t === 112 || t === 80) && comboVal('cmbJobOrder') === 0) {
        box('JobOrder Field Required when Document Type is Production'); $('#cmbJobOrder').select2('open'); return false;
    }
    return true;
}

/* Insert :1377 */
function insert() {
    if (S.busy) return;
    if (S.rows.length === 0) { box('Regular Wages Grid... Record Not Found'); return; }
    if (!formValidation()) return;
    if (!window.confirm(S.recId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
    var body = {
        id: S.recId,
        docNo: el('txtDocNo').value.trim(),
        docDate: docDateValue(),
        refDocumentTypeId: comboVal('cmbReferenceDocType'),
        refDocument: comboText('cmbReferenceDocType'),
        jobOrderId: comboVal('cmbJobOrder'),
        otherRemarks: el('txtRemarks').value,
        stockPartyId: comboVal('cmbStockParty'),
        isApproved: el('chkIsApprove').checked,
        details: S.rows.map(function (r) {
            return {
                id: toInt(r.id), date: r.date,
                contractorId: toInt(r.contractorId), contractorText: cellText('contractorId', r.contractorId),
                wagesAccountId: toInt(r.wagesAccountId), wagesAccountText: cellText('wagesAccountId', r.wagesAccountId),
                packSize: r.packSize, qty: r.qty, billWeight: r.billWeight, rate: r.rate,
                wagesType: r.wagesType, itemId: toInt(r.itemId), cropText: cellText('cropYearId', r.cropYearId),
                packingTypeId: toInt(r.packingTypeId), jobLotId: toInt(r.jobLotId),
                wareHouseFromId: toInt(r.wareHouseFromId), wareHouseToId: toInt(r.wareHouseToId),
                isCompany: toBool(r.isCompany), wagesScheduleId: toInt(r.wagesScheduleId)
            };
        })
    };
    var wasUpdate = S.recId > 0, btn = wasUpdate ? el('btnUpdate') : el('btnSave');
    S.busy = true; btn.classList.add('btn-busy'); btn.disabled = true;
    postJson(API + '/save', body).then(function (res) {
        box(res.message);
        formRest();
        if (el('chkPreview').checked) printSlip(toInt(res.id));
    }).catch(function (e) {
        box(e.message);
    }).then(function () {
        S.busy = false; btn.classList.remove('btn-busy');
        btn.disabled = wasUpdate ? !S.canUpdate : !S.canSave;
    });
}
window.wmSave = function () { S.recId = 0; insert(); };          // btnsave_Click sets RECID = 0
window.wmUpdate = function () { insert(); };

/* ReadById :1577 */
function readById(id) {
    S.recId = id;
    return getJson(API + '/' + id).then(function (d) {
        var h = d.header, dt = d.details || [];
        wmTab(0);
        el('txtDocDate').value = isoDate(toDate(ci(h, 'DocDate')));
        el('txtDocNo').value = txt(ci(h, 'DocNo'));
        setCombo('cmbReferenceDocType', ci(h, 'RefDocumentTypeId'));
        var t = comboVal('cmbReferenceDocType'), jo = ci(h, 'JobOrderId');
        if (t === 112 || t === 80) { showJobOrder(true); jobOrderNoFill(jo); }
        else { showJobOrder(false); setCombo('cmbJobOrder', jo); }
        setCombo('cmbStockParty', ci(h, 'StockPartyId'));
        el('txtRemarks').value = txt(ci(h, 'OtherRemarks'));
        el('chkIsApprove').checked = toBool(ci(h, 'IsAproved'));
        S.rows = dt.map(function (x) {
            var dd = toDate(ci(x, 'RefDocDate'));
            return {
                id: toInt(ci(x, 'Id')),
                date: dd ? isoDate(dd) + 'T00:00:00' : '',
                contractorId: toInt(ci(x, 'ContractorId')),
                wagesAccountId: toInt(ci(x, 'InvConractorWagesAccountsId')),
                packSize: Number(ci(x, 'PackSize')) || 0,
                qty: Number(ci(x, 'BillQty')) || 0,
                billWeight: Number(ci(x, 'BillWeight')) || 0,
                rate: Number(ci(x, 'WageRate')) || 0,
                amount: Number(ci(x, 'WagesAmount')) || 0,
                itemId: toInt(ci(x, 'ItemId')),
                itemCode: txt(ci(x, 'ItemCode')),
                cropYearId: txt(ci(x, 'Crop')),
                packingTypeId: toInt(ci(x, 'InvPackingTypeId')),
                jobLotId: toInt(ci(x, 'JobLotId')),
                wareHouseFromId: toInt(ci(x, 'WareHouseFromId')),
                wareHouseToId: toInt(ci(x, 'WareHouseToId')),
                wagesType: toBool(ci(x, 'FreeOfCost')) ? 'Free Of Cost' : 'Regular',
                isCompany: toBool(ci(x, 'IsCompany')),
                wagesScheduleId: toInt(ci(x, 'InvContractorWagesScheduleId'))
            };
        });
        S.ghost = 0; S.cur = -1;
        renderDetail();
        el('btnSave').style.display = 'none';
        el('btnUpdate').style.display = '';
        getRateAmount();                          // txtDocdate.Value = head.DocDate fires ValueChanged
    }).catch(function (e) {
        formRest();
        box(e.message || 'Record Not found');
    });
}

/* ------------------------------------------------------------------ prints */
function printVia(checkUrl) {
    var win = window.CrystalPrint ? CrystalPrint.reserve() : null;
    getJson(checkUrl).then(function (d) {
        CrystalPrint.open(d.key, d.args, null, win);
    }).catch(function (e) {
        if (window.CrystalPrint) CrystalPrint.release(win);
        box(e.message);
    });
}
/* ContractorWagesBillManualSlip004 */
function printSlip(id) {
    if (!(id > 0)) { box('No Record Found For Display'); return; }
    printVia(API + '/slip-check/' + id);
}
/* VoucherReport_118(VoucherHeadIdGet(Id, 810), 810) */
function printVoucher(id) { printVia(API + '/voucher-check/' + toInt(id)); }
window.wmPrint = function () { printSlip(S.recId); };            // Print_Click

/* ------------------------------------------------------------------ History */
window.wmNewHistory = function () {                               // btnNewHistory_Click
    var d = new Date(); d.setDate(d.getDate() - 3);
    el('FromDateHistory').value = isoDate(d);
    el('FromDocNoHistory').value = '';
    el('ToDocNoHistory').value = '';
    el('FromDateHistory').focus();
};
window.wmRefreshHistory = function () {                           // btnRefreshHistory_Click -> DocumentTypeFill
    getJson(API + '/document-types').then(documentTypeFill).catch(function (e) { box(e.message); });
};
window.wmShowHistory = function () {                              // bindHistory :1865
    var p = {
        fromDate: el('chkFromDate').checked ? el('FromDateHistory').value : '',
        toDate: el('chkToDate').checked ? el('ToDateHistory').value : '',
        fromDocNo: el('FromDocNoHistory').value.trim(),
        toDocNo: el('ToDocNoHistory').value.trim(),
        refDocumentTypeId: comboVal('CmbDocumentTypeHistory')
    };
    var b = el('btnShowHistory'); b.disabled = true; b.classList.add('btn-busy');
    getJson(API + '/history?' + qs(p)).then(function (rows) {
        S.hist = rows || []; S.histCur = -1;
        renderHistory();
        renderHistoryDetail(null);
    }).catch(function (e) { box(e.message); })
      .then(function () { b.disabled = false; b.classList.remove('btn-busy'); });
};
function histCols() {
    var c = [{ k: 'RecordNo', t: 'RecordNo' }, { k: 'DocumentType', t: 'DocumentType' }];
    if (S.party) c.push({ k: 'StockParty', t: 'StockParty' });
    return c.concat([
        { k: 'DocDate', t: 'DocDate', d: 'date' }, { k: 'DocNo', t: 'DocNo' },
        { k: 'QtyTotal', t: 'QtyTotal', f: fmtHash2, sum: true }, { k: 'WeightTotal', t: 'WeightTotal', f: fmtHash2, sum: true },
        { k: 'WagesAmount', t: 'WagesAmount', f: fmtAmt, sum: true }, { k: 'Remarks', t: 'Remarks' },
        { k: 'EntryDate', t: 'EntryDate', d: 'dt' }, { k: 'EntryUser', t: 'EntryUser' },
        { k: 'ModifyDate', t: 'ModifyDate', d: 'dt' }, { k: 'ModifyUser', t: 'ModifyUser' }]);
}
function renderHistory() {
    var t = el('grdHistory'), cols = histCols();
    if (!S.hist.length) { t.tHead.innerHTML = ''; t.tBodies[0].innerHTML = ''; t.tFoot.innerHTML = ''; return; }
    t.tHead.innerHTML = '<tr><th>Edit</th><th>Slip</th><th>Voucher</th>' + cols.map(function (c) { return '<th>' + esc(c.t) + '</th>'; }).join('') + '</tr>';
    t.tBodies[0].innerHTML = S.hist.map(function (r, i) {
        return '<tr data-i="' + i + '" tabindex="-1" class="' + (i === S.histCur ? 'cur' : '') + '">'
            + '<td class="btncell"><button type="button" class="lw-row-btn" data-act="Edit" data-i="' + i + '">Edit</button></td>'
            + '<td class="btncell"><button type="button" class="lw-row-btn" data-act="Slip" data-i="' + i + '">Slip</button></td>'
            + '<td class="btncell"><button type="button" class="lw-row-btn" data-act="Voucher" data-i="' + i + '">Voucher</button></td>'
            + cols.map(function (c) {
                var v = r[c.k], s;
                if (c.d === 'date') s = ddMMMyyyy(toDate(v));
                else if (c.d === 'dt') s = ddMMMyyyyhm(toDate(v));
                else if (c.f) s = c.f(v);
                else s = txt(v);
                return '<td class="' + (c.f ? 'num' : '') + '">' + esc(s) + '</td>';
            }).join('') + '</tr>';
    }).join('');
    var foot = '<tr><td colspan="3"></td>';
    cols.forEach(function (c) {
        var s = '';
        if (c.sum) { var n = 0; S.hist.forEach(function (r) { n += Number(r[c.k]) || 0; }); s = c.f(n); }
        foot += '<td>' + esc(s) + '</td>';
    });
    t.tFoot.innerHTML = foot + '</tr>';
}
/* DataGridHistory_SelectionChanged -> HistoryDetailGridBind :2033 */
function selectHistory(i) {
    if (i === S.histCur) return;
    S.histCur = i;
    Array.prototype.forEach.call(el('grdHistory').tBodies[0].rows, function (tr) { tr.classList.toggle('cur', +tr.getAttribute('data-i') === i); });
    var r = S.hist[i];
    if (!r) return;
    getJson(API + '/' + toInt(r.Id)).then(function (d) { if (S.histCur === i) renderHistoryDetail(d.details || []); })
        .catch(function () { /* GetByID with no rows leaves the grid as it is */ });
}
function renderHistoryDetail(rows) {
    var t = el('grdHistoryDetail');
    if (!rows) { t.tHead.innerHTML = ''; t.tBodies[0].innerHTML = ''; t.tFoot.innerHTML = ''; return; }
    var cols = [
        { k: 'CompanyName', t: 'Contractor' }, { k: 'WagesAccountName', t: 'Labour / Wages Activity' },
        { k: 'PackSize', t: 'PackSize', f: plainNum }, { k: 'BillQty', t: 'Qty', f: fmtHash4, sum: true },
        { k: 'BillWeight', t: 'BillWeight', f: fmtHash4, sum: true }, { k: 'WageRate', t: 'Rate', f: fmtRate, avg: true },
        { k: 'WagesAmount', t: 'Amount', f: fmtAmt, sum: true }, { k: 'ItemName', t: 'ItemName' }, { k: 'ItemCode', t: 'ItemCode' },
        { k: 'Crop', t: 'CropYear' }, { k: 'PackTypeDesc', t: 'PackingType' }, { k: 'JobLotDescription', t: 'JobLot' },
        { k: 'WareHouseFrom', t: 'WareHouseFrom' }, { k: 'FreeOfCost', t: 'WagesType', w: true }];
    if (S.party) cols.push({ k: 'IsCompany', t: 'IsCompany', b: true });
    t.tHead.innerHTML = '<tr>' + cols.map(function (c) { return '<th>' + esc(c.t) + '</th>'; }).join('') + '</tr>';
    t.tBodies[0].innerHTML = rows.map(function (r) {
        var foc = toBool(ci(r, 'FreeOfCost'));
        return '<tr class="' + (foc ? 'foc' : '') + '">' + cols.map(function (c) {
            var v = ci(r, c.k), s;
            if (c.w) s = foc ? 'Free Of Cost' : 'Regular';
            else if (c.b) s = toBool(v) ? '✔' : '';
            else if (c.f) s = c.f(v);
            else s = txt(v);
            return '<td class="' + (c.f ? 'num' : '') + '">' + esc(s) + '</td>';
        }).join('') + '</tr>';
    }).join('');
    t.tFoot.innerHTML = '<tr>' + cols.map(function (c) {
        if (!rows.length || !(c.sum || c.avg)) return '<td></td>';
        var n = 0; rows.forEach(function (r) { n += Number(ci(r, c.k)) || 0; });
        return '<td>' + esc(c.f(c.avg ? n / rows.length : n)) + '</td>';
    }).join('') + '</tr>';
}
function historyEvents() {
    var t = el('grdHistory'), curKey = '';
    t.addEventListener('click', function (e) {
        var tr = e.target.closest('tbody tr[data-i]');
        if (!tr) return;
        var i = +tr.getAttribute('data-i');
        selectHistory(i);
        var b = e.target.closest('button[data-act]');
        if (!b) return;
        var id = toInt(S.hist[i].Id), a = b.getAttribute('data-act');
        if (a === 'Voucher') printVoucher(id);
        if (a === 'Slip') printSlip(id);
        if (a === 'Edit') readById(id);
    });
    t.addEventListener('dblclick', function (e) {
        var tr = e.target.closest('tbody tr[data-i]');
        if (tr && !e.target.closest('button')) readById(toInt(S.hist[+tr.getAttribute('data-i')].Id));
    });
    t.addEventListener('focusin', function (e) {
        var b = e.target.closest('button[data-act]'), tr = e.target.closest('tbody tr[data-i]');
        curKey = b ? b.getAttribute('data-act') : '';
        if (tr) selectHistory(+tr.getAttribute('data-i'));
    });
    /* DataGridHistory_KeyDown :2615 */
    t.addEventListener('keydown', function (e) {
        var r = S.hist[S.histCur];
        if (!r) return;
        var id = toInt(r.Id);
        if (e.ctrlKey && (e.key === ' ' || e.code === 'Space')) {
            e.preventDefault();
            if (curKey === 'Edit' && S.canUpdate) readById(id);
            if (curKey === 'Voucher') printVoucher(id);
            if (curKey === 'Slip') printSlip(id);
        }
        if (e.ctrlKey && e.key === 'Enter' && S.canUpdate) { e.preventDefault(); e.stopPropagation(); readById(id); }
        if (e.ctrlKey && (e.key === 'p' || e.key === 'P') && S.canPrint) { e.preventDefault(); e.stopPropagation(); printSlip(id); }
    });
}

/* ------------------------------------------------------------------ keyboard */
window.wmShortcuts = function () {                                // MakeShortCutKeys :2355
    var rows = [['Ctrl+S', 'For Save'], ['Ctrl+U', 'For Update'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'],
        ['Ctrl+N', 'For New'], ['Ctrl+P', 'For Print'], ['Alt+1', 'For Print'], ['Ctrl+F5', 'For Focus on Doc Date'],
        ['Ctrl+T', 'For Tab Transfer'], ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+D', 'For Adding an row in Focused Grid'],
        ['Ctrl+Delete', 'For Deleting an row of Focused Grid'], ['Ctrl+ArrowDown', 'For Focus On Detail Grid'],
        ['Ctrl+ArrowUp', 'For Focus On Contractor Name in Detail Box'], ['Ctrl+ArrowRight', 'For Change Focus from one Grid To another Grid'],
        ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]];
    var back = document.createElement('div');
    back.className = 'wm-modal-back';
    back.innerHTML = '<div class="wm-modal"><header><span>ShortCut Keys</span><span style="cursor:pointer" data-x>&times;</span></header><div class="body">'
        + '<table class="win-grid-table"><thead><tr><th>KeyCombination</th><th>Description</th></tr></thead><tbody>'
        + rows.map(function (r) { return '<tr><td>' + esc(r[0]) + '</td><td>' + esc(r[1]) + '</td></tr>'; }).join('')
        + '</tbody></table></div></div>';
    back.addEventListener('click', function (e) { if (e.target === back || e.target.hasAttribute('data-x')) document.body.removeChild(back); });
    document.body.appendChild(back);
};
function modalOpen() { return !!document.querySelector('.wm-modal-back'); }
function select2Open() { return !!document.querySelector('.select2-container--open'); }
function focusFirstGridCell() { var x = el('grdDetail').querySelector('tbody [data-k],tbody button'); if (x) x.focus(); else el('grdDetail').focus(); }

/* frmWagesBillManualKeyDown :2252 */
function formKeyDown(e) {
    if (modalOpen()) return;
    var k = e.key, ctrl = e.ctrlKey, lower = (k || '').toLowerCase();
    var inGrid = !!(e.target.closest && e.target.closest('#grdDetail,#grdHistory'));
    if (k === 'Enter' && !ctrl && !e.altKey && !inGrid) {
        var tg = e.target;
        if (tg.tagName === 'INPUT' && tg.type !== 'button' && tg.type !== 'checkbox' || tg.tagName === 'SELECT') {
            e.preventDefault(); focusNext(tg);
        }
    }
    if (ctrl && lower === 't') { e.preventDefault(); return; }      // tabControl1 has one page
    if ((ctrl && lower === 'e') || (k === 'Escape' && !select2Open())) {
        e.preventDefault(); window.location.href = '/wages'; return;
    }
    if (ctrl && e.altKey && (k === 'Control' || k === 'Alt')) { wmShortcuts(); return; }
    /* tabControl1.SelectedIndex is always 0, so the Form-tab keys work on both tabs. */
    if (ctrl && lower === 's') { e.preventDefault(); if (el('btnSave').style.display !== 'none' && !el('btnSave').disabled) wmSave(); }
    if (ctrl && lower === 'u') { e.preventDefault(); if (el('btnUpdate').style.display !== 'none' && !el('btnUpdate').disabled) wmUpdate(); }
    if (ctrl && lower === 'n') { e.preventDefault(); wmNew(); }
    if (ctrl && lower === 'r') { e.preventDefault(); wmRefresh(); }
    if (ctrl && k === 'F5') { e.preventDefault(); el('txtDocDate').focus(); }
    if (ctrl && k === 'ArrowDown') { e.preventDefault(); focusFirstGridCell(); }
    if (ctrl && k === 'ArrowUp') { e.preventDefault(); $('#cmbContractor').select2('focus'); }
    if (ctrl && lower === 'p') { e.preventDefault(); wmPrint(); }
    if (e.altKey && (k === '1' || e.code === 'Numpad1' || e.code === 'Digit1')) { e.preventDefault(); wmPrint(); }
}
function focusNext(from) {
    var all = Array.prototype.filter.call(document.querySelectorAll('#viewForm input, #viewForm select, #viewForm textarea, #viewForm button, #viewForm .select2-selection'),
        function (x) { return !x.disabled && x.offsetParent !== null && x.tabIndex >= 0 && !(x.tagName === 'SELECT' && x.classList.contains('select2-hidden-accessible')); });
    var i = all.indexOf(from.classList.contains('select2-hidden-accessible') ? from.nextElementSibling : from);
    if (i >= 0 && all[i + 1]) all[i + 1].focus();
}

/* OnlytextdecimelFunction - digits, one point, control keys. */
function decimalOnly(e) {
    var t = e.target;
    if (!t.classList || !t.classList.contains('wm-dec')) return;
    if (e.ctrlKey || e.metaKey || e.altKey || e.key.length !== 1) return;
    if (/\d/.test(e.key)) return;
    if (e.key === '.' && t.value.indexOf('.') < 0) return;
    e.preventDefault();
}

/* ------------------------------------------------------------------ wiring */
$(function () {
    $('.wm-s2').each(function () { $(this).select2({ width: '100%', allowClear: true, placeholder: '' }); });
    var n = new Date();
    el('txtDocDate').value = isoDate(n);
    var f = new Date(); f.setDate(f.getDate() - 3);
    el('FromDateHistory').value = isoDate(f);                     // FromDateHistory = Now - 3
    el('ToDateHistory').value = isoDate(n);

    /* "Leave" of the two combos -> GetRateAmount + weight + amount */
    $('#cmbWagesAccount, #cmbContractor').on('select2:select select2:clear', function () { getRateAmount(); });
    $('#cmbReferenceDocType').on('select2:select select2:clear', refDocTypeLeave);
    el('txtDocDate').addEventListener('change', function () { getRateAmount(); });
    el('txtPackSize').addEventListener('input', function () { getRateAmount(); });
    el('txtQty').addEventListener('input', function () { calculateDetailWeight(); calculateDetailAmount(); });
    el('txtWeight').addEventListener('input', calculateDetailAmount);
    document.addEventListener('keypress', decimalOnly, true);
    document.addEventListener('keydown', formKeyDown);
    gridEvents();
    historyEvents();
    load();
});

})();
