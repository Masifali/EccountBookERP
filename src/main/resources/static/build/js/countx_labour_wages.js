/* ============================================================================
 * Labour Wages  (Contractor Wages Bill, DocumentTypeId 101)
 * Ported from Architecture.WinApp.Contractor_Wages\frmwagesBillHeader.cs (5,407 lines).
 *
 * Every rule below is taken from that form, with its line numbers, rather than invented:
 *
 *   :353-387   dtdetail's 33 columns, in this exact order
 *   :387-434   form load - PendingGrnAndGdn() when opened from a document, PendingTicket() otherwise
 *   :523-550   accountName(DocumentTypeId) - which wages activities this ref doc type allows
 *   :553-566   WagesAmountCalculationsConfigurations() - WagesAmountCalculateOnQty
 *   :690-820   DetailGridSettings() - which columns are visible and which are editable
 *   :830-850   WagesItemAndContractorRefresh() - the only two dropdown columns
 *   :855-1103  grdwagesDetail_CellUpdated - the whole calculation, verbatim
 *   :2121-2300 LoadDataForWages() - turning a reference document into wages rows
 *   :2572-2600 FormValidation
 *
 * 2026-10-01 recheck: three tab controls as on the desktop, cmbReferenceDocType bound to the one
 * loaded document, history from USP_ContractorWagesBillHeader_FormHistory, ReadById split into
 * Regular / Other, and the stored values of Insert() (:2702-3027) reproduced one by one.
 *
 * Amount is NOT guessed:  WagesAmountCalculateOnQty ? Quantity * Rate
 *                                                   : (BillWeight / PackSize) * Rate   (:1006)
 * ==========================================================================*/
(function () {
'use strict';

var API = '/api/contractor-wages/labour-wages';

/* ---------------------------------------------------------------- state */
var S = {
    id: 0,                 // RECID - 0 means Save, > 0 means Update
    refDocTypeId: 0,
    refDocId: 0,           // txtGRNId - the reference document's own Id
    reqType: '',           // txtEntryType - "Input" / "OutPut" / "MoveOrder" / a supplier name
    regular: [],           // dtdetail
    other: [],             // dtStiching
    pending: [],
    contractors: [],
    activities: [],
    otherActivities: [],     // dtStichingItems - the Other Wages grid's own activity list
    otherIds: '35',          // its default ids: "35", or "34" for MoveOrder / fill-in rows
    isApproved: false,       // chkisapprove (Visible = false) - only ever set by reading a bill back
    cfg: {},
    busy: false,
    formHistoryRows: [],
    historyRows: [],
    isReferred: false,     // IsReferred - set from the history row (:3390-3398); blocks grid row add/delete
    prevHeaderQty: null,   // grdDataRetrieve HeaderQty / HeaderWeight (ValidationOnformClose :2600-2604)
    prevHeaderWeight: null
};

/* GlobalVariables_Helper config values, read through /config-flags. */
function cfgBool(n) { var v = S.cfg[n]; if (v === undefined || v === null) return false;
                      v = String(v).trim().toLowerCase(); return v === 'true' || v === '1'; }
function cfgNum(n)  { var v = parseFloat(S.cfg[n]); return isNaN(v) ? 0 : v; }
function onQty()    { return cfgBool('WagesAmountCalculateOnQty'); }
function addLessOn(){ return cfgBool('EnableAddLessOnWagesRegular'); }

/* ---------------------------------------------------------------- helpers */
function num(v)  { var n = parseFloat(v); return isNaN(n) ? 0 : n; }
function r2(v)   { return Math.round(num(v) * 100) / 100; }
function txt(v)  { return v === null || v === undefined ? '' : String(v); }
function esc(v)  { return txt(v).replace(/&/g, '&amp;').replace(/</g, '&lt;')
                                .replace(/>/g, '&gt;').replace(/"/g, '&quot;'); }
function fmt(v, d) { var n = num(v); return n.toLocaleString(undefined,
                     { minimumFractionDigits: d === undefined ? 2 : d,
                       maximumFractionDigits: d === undefined ? 2 : d }); }
/* "#,##0.####" - up to d decimals, none forced. */
function fmtN(v, d) { return num(v).toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: d === undefined ? 4 : d }); }
/* dd-MMM-yyyy [hh:mm tt] without going through toISOString() */
var MON = ['Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov','Dec'];
function dmy(v, withTime) {
    if (!v) return '';
    var m = String(v).match(/^(\d{4})-(\d{2})-(\d{2})(?:[T ](\d{2}):(\d{2}))?/);
    if (!m) return String(v);
    var out = m[3] + '-' + MON[parseInt(m[2], 10) - 1] + '-' + m[1];
    if (withTime && m[4] !== undefined) {
        var h = parseInt(m[4], 10), ap = h >= 12 ? 'PM' : 'AM';
        h = h % 12; if (h === 0) h = 12;
        out += ' ' + ('0' + h).slice(-2) + ':' + m[5] + ' ' + ap;
    }
    return out;
}
function el(id)  { return document.getElementById(id); }
function val(id) { var e = el(id); return e ? e.value : ''; }
function setVal(id, v) { var e = el(id); if (e) e.value = (v === null || v === undefined) ? '' : v; }

/* Dates never go through toISOString(): at UTC+5 that turns a local midnight into
   the previous day. The value is cut out of the string the server sent. */
function ymd(v) {
    if (!v) return '';
    var s = String(v);
    var m = s.match(/^(\d{4})-(\d{2})-(\d{2})/);
    if (m) return m[1] + '-' + m[2] + '-' + m[3];
    var d = new Date(s);
    if (isNaN(d.getTime())) return '';
    return d.getFullYear() + '-' + ('0' + (d.getMonth() + 1)).slice(-2)
                           + '-' + ('0' + d.getDate()).slice(-2);
}
function today() {
    var d = new Date();
    return d.getFullYear() + '-' + ('0' + (d.getMonth() + 1)).slice(-2)
                           + '-' + ('0' + d.getDate()).slice(-2);
}
/* Case-tolerant column read: the procedures do not agree on casing. */
function f(row, name) {
    if (!row) return null;
    if (row[name] !== undefined) return row[name];
    var lower = String(name).toLowerCase();
    for (var k in row) { if (k.toLowerCase() === lower) return row[k]; }
    return null;
}

/* Every desktop message is a MessageBox; so is every message here. */
function msg(text) { window.alert(text); }
function clearMsg() { }

/* Button click -> disabled + loader immediately, re-enabled on success or failure,
   and a second click while in flight is dropped. */
function busy(btn, on) {
    S.busy = !!on;
    if (btn) { btn.disabled = !!on; btn.classList.toggle('is-busy', !!on); }
    ['btnSave', 'btnUpdate', 'btnNew', 'btnRefresh'].forEach(function (id) {
        var b = el(id); if (b && b !== btn) b.disabled = !!on;
    });
    if (!on) applyRights();
}

function getJson(url) {
    return fetch(url, { headers: { 'Accept': 'application/json' } })
        .then(function (r) { if (!r.ok) throw new Error('HTTP ' + r.status + ' on ' + url); return r.json(); });
}
function postJson(url, body) {
    return fetch(url, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json', 'Accept': 'application/json' },
        body: JSON.stringify(body)
    }).then(function (r) { return r.json().catch(function () { return { success: false, message: 'HTTP ' + r.status }; }); });
}

/* ---------------------------------------------------------------- tabs */
/* The desktop has THREE tab controls, not one strip:
 *   tabIncomingLabourwages : Form | History
 *   tabControl1 (panel6)   : Regular Wages | Other Wages
 *   tabControl2 (panel5)   : Pending Data For Load | Wages Detail
 * Other Wages and Wages Detail are removed at load (:338-339) and added back only when the
 * loaded document calls for them, so they are hidden here until then. */
var TAB_GROUPS = [['form', 'history'], ['regular', 'other'], ['pending', 'detail']];
function cap(t) { return t.charAt(0).toUpperCase() + t.slice(1); }
function lwTab(name) {
    TAB_GROUPS.forEach(function (g) {
        if (g.indexOf(name) < 0) return;
        g.forEach(function (t) {
            var v = el('view' + cap(t)), b = el('tab' + cap(t));
            if (v) v.classList.toggle('is-hidden', t !== name);
            if (b) b.classList.toggle('act', t === name);
        });
    });
    if (name !== 'form' && name !== 'history') {
        var vf = el('viewForm');
        if (vf && vf.classList.contains('is-hidden')) lwTab('form');
    }
    if (name === 'history') { var fd = el('histFromDate'); if (fd) fd.focus(); }   /* :3236 */
}
/* TabPages.Add / TabPages.Remove */
function tabShown(name, on) {
    var b = el('tab' + cap(name));
    if (!b) return;
    b.classList.toggle('is-hidden', !on);
    if (!on && b.classList.contains('act')) {
        TAB_GROUPS.forEach(function (g) {
            if (g.indexOf(name) < 0) return;
            var other = g[0] === name ? g[1] : g[0];
            var ob = el('tab' + cap(other));
            if (ob && !ob.classList.contains('is-hidden')) lwTab(other);
            else { var v = el('view' + cap(name)); if (v) v.classList.add('is-hidden'); b.classList.remove('act'); }
        });
    }
}
function onFormTab() { var v = el('viewHistory'); return !v || v.classList.contains('is-hidden'); }
/* A message box, as the desktop shows one - not a banner that scrolls away. */
function box(text) { window.alert(text); }

/* ---------------------------------------------------------------- the cap exemptions
 * :896 and every sibling condition: these reference document types are exempt from the
 * "cannot exceed the document's total" checks, some of them only when their own config
 * flag is on. AddCondition (:731) is the same set, and it is also what shows RowNo.
 */
function addCondition() {
    var d = S.refDocTypeId;
    return d === 112 || d === 66
        || (cfgBool('ContractorWageComparisonbyActivityForGRN') && d === 46)
        || (cfgBool('ContractorWageComparisonbyActivityForGDN') && d === 86)
        || (cfgBool('ContractorWageComparisonbyActivityForForwarding') && d === 205)
        || (cfgBool('ContractorWageComparisonbyActivityForStockTransfer') && (d === 68 || d === 806))
        || (cfgBool('ContractorWageComparisonbyActivityForProductionInput') && d === 80)
        || (cfgBool('ContractorWageComparisonbyActivityForProductionConsumption') && d === 181);
}

/* ---------------------------------------------------------------- grid columns
 * DetailGridSettings() :696-760. Hidden on the desktop and therefore not rendered:
 * PurchaseGLAC, WarehouseType, ContractorName, WagesAccount, ItemId, jobLotId,
 * packingTypeId, MoveFromId, MoveToId, BillQty, RefDocQty, RefDocWeight, WagesScheduleId.
 * They stay on the row object because the save needs them.
 *
 * MoveTo: the desktop hides it unconditionally - its test is
 * "DocTypeValue != 68 || DocTypeValue != 806" (:712), which no number can fail. Reproduced as
 * written so the two screens show the same columns.
 */
function columns(which) {
    var cols = [
        { k: 'SupplierId',  c: 'Contractor Name',         t: 'contractor', w: 200 },
        { k: 'WagesId',     c: 'Labour / Wages Activity', t: 'activity',   w: 200 },
        { k: 'WagesType',   c: 'Wages Type',   t: 'text',  w: 90 },
        { k: 'Date',        c: 'Date',         t: 'text',  w: 90 },
        { k: 'packingType', c: 'Packing Type', t: 'text',  w: 95 },
        { k: 'Weight',      c: 'Weight',       t: 'num',   w: 90,  edit: true, dec: 4, sum: true },
        { k: 'PackSize',    c: 'Pack Size',    t: 'num',   w: 70,  dec: 4 },
        { k: 'Quantity',    c: 'Quantity',     t: 'num',   w: 80,  edit: true, dec: 4, sum: true },
        { k: 'WeightCut',   c: 'Wt Cut',       t: 'num',   w: 70,  edit: true, dec: 3 },
        { k: 'BillWeight',  c: 'Bill Weight',  t: 'num',   w: 90,  dec: 4, sum: true }
    ];
    if (addLessOn()) {
        cols.push({ k: 'RateWithoutAddLess', c: 'Rate w/o Add-Less', t: 'num', w: 90, dec: 4 });
        cols.push({ k: 'RateAddLess',        c: 'Rate Add/Less',     t: 'num', w: 80, edit: true, dec: 4 });
    }
    cols.push({ k: 'Rate',     c: 'Rate',      t: 'num',  w: 80,  dec: 4 });
    cols.push({ k: 'Amount',   c: 'Amount',    t: 'num',  w: 100, dec: 4, sum: true });
    cols.push({ k: 'Item',     c: 'Item',      t: 'text', w: 190 });
    cols.push({ k: 'jobLot',   c: 'Job Lot',   t: 'text', w: 110 });
    cols.push({ k: 'Crop',     c: 'Crop',      t: 'text', w: 70 });
    cols.push({ k: 'MoveFrom', c: 'Move From', t: 'text', w: 130 });
    /* Regular grid: AddCondition (:731). Other grid: 112 / 66 only (:1895). */
    var d = S.refDocTypeId;
    if (which === 'other' ? (d === 112 || d === 66) : addCondition()) cols.push({ k: 'RefLineId', c: 'RowNo', t: 'text', w: 55 });
    return cols;
}

/* ---------------------------------------------------------------- grid render */
function renderGrid(which) {
    var rows = which === 'regular' ? S.regular : S.other;
    var tbl  = el(which === 'regular' ? 'grdRegular' : 'grdOther');
    var head = el(which === 'regular' ? 'grdRegularHead' : 'grdOtherHead');
    var body = el(which === 'regular' ? 'grdRegularBody' : 'grdOtherBody');
    var foot = el(which === 'regular' ? 'grdRegularFoot' : 'grdOtherFoot');
    if (!head || !body) return;

    /* grdwagesDetail.ClearStructure() - an empty grid has no columns at all. */
    if (!rows.length) {
        head.innerHTML = ''; body.innerHTML = ''; if (foot) foot.innerHTML = '';
        if (tbl) tbl.style.width = '';
        updateTotals();
        return;
    }

    var cols = columns(which);
    var buttons = !S.isReferred;             /* Add / X columns exist only when !IsReferred (:806, :1930) */
    var acts = which === 'regular' ? S.activities : S.otherActivities;
    var width = cols.reduce(function (a, c) { return a + c.w; }, 0) + (buttons ? 60 : 0);
    if (tbl) tbl.style.width = width + 'px';

    head.innerHTML = cols.map(function (c) {
        return '<th style="width:' + c.w + 'px;">' + esc(c.c) + '</th>';
    }).join('') + (buttons ? '<th style="width:40px;">Add</th><th style="width:20px;">X</th>' : '');

    body.innerHTML = rows.map(function (row, i) {
        var freeOfCost = txt(row.WagesType) === 'Free Of Cost';
        var tds = cols.map(function (c) {
            var cls = c.t === 'num' ? ' class="num"' : '';
            /* GridEXFormatCondition :819 - a Free Of Cost row's Wages Type is red. */
            var style = (c.k === 'WagesType' && freeOfCost) ? ' style="color:#f00;"' : '';
            if (c.t === 'contractor') {          /* editable in both lock states (:634, :664) */
                return '<td class="ed">' + selectHtml(which, i, 'SupplierId', row.SupplierId,
                                           S.contractors, 'Id', 'CompanyName', row.ContractorName) + '</td>';
            }
            if (c.t === 'activity') {
                if (S.isReferred) return '<td>' + esc(row.WagesAccount) + '</td>';     /* :663 EditType 0 */
                return '<td class="ed">' + selectHtml(which, i, 'WagesId', row.WagesId,
                                           acts, 'Id', 'WagesAccountName', row.WagesAccount) + '</td>';
            }
            if (c.edit && !S.isReferred) {
                return '<td class="ed"><input type="text" inputmode="decimal" class="lw-cell"'
                     + ' data-g="' + which + '" data-i="' + i + '" data-k="' + c.k + '"'
                     + ' value="' + esc(row[c.k]) + '"></td>';
            }
            var shown = c.t === 'num' ? fmtN(row[c.k], c.dec)
                      : c.k === 'Date' ? esc(dmy(row[c.k])) : esc(row[c.k]);
            return '<td' + cls + style + '>' + shown + '</td>';
        }).join('');
        return '<tr>' + tds + (buttons
             ? '<td class="btncell"><button type="button" class="cb"'
             + ' onclick="lwAddRowAfter(\'' + which + '\',' + i + ')">Add</button></td>'
             + '<td class="btncell"><button type="button" class="cb"'
             + ' onclick="lwDeleteRow(\'' + which + '\',' + i + ')">X</button></td>' : '')
             + '</tr>';
    }).join('');

    if (foot) {
        foot.innerHTML = cols.map(function (c) {
            if (!c.sum) return '<td></td>';
            var s = rows.reduce(function (a, r) { return a + num(r[c.k]); }, 0);
            return '<td class="num">' + fmtN(s, c.dec) + '</td>';
        }).join('') + (buttons ? '<td></td><td></td>' : '');
    }

    /* Edits are bound after the markup exists, never inline, so a re-render cannot
       leave two handlers on one cell. */
    Array.prototype.forEach.call(body.querySelectorAll('.lw-cell'), function (inp) {
        inp.addEventListener('change', function () {
            cellUpdated(inp.getAttribute('data-g'), parseInt(inp.getAttribute('data-i'), 10),
                        inp.getAttribute('data-k'), inp.value);
        });
    });
    Array.prototype.forEach.call(body.querySelectorAll('.lw-sel'), function (sel) {
        sel.addEventListener('change', function () {
            cellUpdated(sel.getAttribute('data-g'), parseInt(sel.getAttribute('data-i'), 10),
                        sel.getAttribute('data-k'), sel.value);
        });
    });

    updateTotals();
}

function selectHtml(which, i, key, value, list, idField, textField, savedText) {
    var out = '<select class="lw-sel" data-g="' + which + '" data-i="' + i + '" data-k="' + key + '">';
    out += '<option value=""></option>';
    var hit = false;
    for (var n = 0; n < list.length; n++) {
        var id = f(list[n], idField);
        var tx = f(list[n], textField);
        var on = String(id) === String(value);
        if (on) hit = true;
        out += '<option value="' + esc(id) + '"' + (on ? ' selected' : '') + '>' + esc(tx) + '</option>';
    }
    /* A saved row whose contractor / activity is no longer in the list still shows its name,
       as the desktop cell does - it is never silently blanked. */
    if (!hit && num(value) > 0) {
        out += '<option value="' + esc(value) + '" selected>' + esc(savedText || value) + '</option>';
    }
    return out + '</select>';
}

function updateTotals() {
    var amt = S.regular.concat(S.other).reduce(function (a, r) { return a + num(r.Amount); }, 0);
    var e;
    if ((e = el('lblRegularCount'))) e.textContent = S.regular.length;
    if ((e = el('lblOtherCount')))   e.textContent = S.other.length;
    if ((e = el('lblGrandAmount')))  e.textContent = fmt(amt, 2);
}

/* ---------------------------------------------------------------- the calculation
 * grdwagesDetail_CellUpdated, :855-1103. Kept in the same order as the form, including
 * which caps apply under which config, because a reordering changes the result.
 */
function cellUpdated(which, i, key, value) {
    var rows = which === 'regular' ? S.regular : S.other;
    var row = rows[i];
    if (!row) return;

    if (key === 'SupplierId' || key === 'WagesId') {
        row[key] = value === '' ? 0 : parseInt(value, 10);
        if (key === 'WagesId') {
            var act = (which === 'regular' ? S.activities : S.otherActivities).filter(function (a) { return String(f(a, 'Id')) === String(value); })[0];
            row.WagesAccount = act ? txt(f(act, 'WagesAccountName')) : '';
        } else {
            var con = S.contractors.filter(function (c) { return String(f(c, 'Id')) === String(value); })[0];
            row.ContractorName = con ? txt(f(con, 'CompanyName')) : '';
        }
    } else {
        row[key] = num(value);
    }

    /* Totals of the OTHER rows, :861-873 - the caps are against what is left of the
       document, not against the document's whole total. */
    var otherQty = 0, otherBillWeight = 0, otherWeight = 0;
    rows.forEach(function (r, n) {
        if (n === i) return;
        otherQty        += num(r.Quantity);
        otherBillWeight += num(r.BillWeight);
        otherWeight     += num(r.Weight);
    });
    var headGross = num(val('txtGrossWeight'));
    var headQty   = num(val('txtTotalQty'));
    var regularTotalWeight = headGross - otherBillWeight;
    var regularGrossWeight = headGross - otherWeight;
    var regularTotalQty    = headQty   - otherQty;

    var exempt = addCondition();
    var warn = null;

    /* :885 - the activity or contractor changed, so ask again whether this item is free of
       cost. The answer is applied when it returns; the rest of the calculation does not wait. */
    if (key === 'SupplierId' || key === 'WagesId') {
        askFreeOfCost(which, i, row);
    }

    if ((key === 'Quantity' || key === 'WeightCut') && num(row.Quantity) > 0 && num(row.PackSize) > 0) {
        var qty = num(row.Quantity);
        if (onQty() && qty > regularTotalQty && !exempt) {
            warn = 'Wages Total Qty can not be Greater than Wages total Qty';
            qty = r2(regularTotalQty);
        }
        row.Quantity = r2(qty);

        var packSize = num(row.PackSize);
        var weight = qty * packSize;
        if (!onQty() && weight > regularGrossWeight && !exempt) {
            warn = 'Wages Weight can not be Greater than Wages Gross Weight';
            weight = r2(regularGrossWeight);
            qty = r2(weight / packSize);
            row.Quantity = qty;
        }
        row.Weight = weight;

        var billWeight = weight - Math.abs(qty * num(row.WeightCut));
        if (!onQty() && billWeight > regularTotalWeight && !exempt) {
            row.Weight     = r2(regularGrossWeight);
            row.BillWeight = r2(regularTotalWeight);
            row.Quantity   = r2(qty * packSize);
            warn = 'Wages Total Weight can not be Greater than Wages total Weight';
        } else {
            row.BillWeight = r2(billWeight);
        }
    }

    if ((key === 'Weight' || key === 'WeightCut') && num(row.Weight) > 0 && num(row.PackSize) > 0) {
        var w = num(row.Weight);
        if (!onQty() && w > regularGrossWeight && !exempt) {
            row.Weight = r2(regularGrossWeight);
            warn = 'Wages Weight can not be Greater than Wages Gross Weight';
            w = r2(regularGrossWeight);
        }
        var ps = num(row.PackSize);
        var q2 = w / ps;
        row.Quantity = r2(q2);
        if (onQty() && q2 > regularTotalQty && !exempt) {
            warn = 'Wages Total Qty can not be Greater than Wages total Qty';
            q2 = r2(regularTotalQty);
            row.Quantity = q2;
            row.Weight = r2(q2 * ps);
        }
        var bw2 = num(row.Weight) - Math.abs(num(row.WeightCut) * q2);
        if (!onQty() && bw2 > regularTotalWeight && !exempt) {
            row.Weight     = r2(regularGrossWeight);
            row.BillWeight = r2(regularTotalWeight);
            warn = 'Wages Total Weight can not be Greater than Wages total Weight';
        } else {
            row.BillWeight = r2(bw2);
        }
    }

    if (warn) msg(warn);

    /* :976 - free of cost, or a Dryer warehouse, is never rated. */
    if (txt(row.WagesType) === 'Free Of Cost' || txt(row.WarehouseType) === 'Dryer') {
        row.WagesScheduleId = 0; row.Rate = 0; row.RateWithoutAddLess = 0; row.Amount = 0;
        renderGrid(which);
        return;
    }

    if (key === 'WagesId' || key === 'SupplierId' || key === 'Quantity'
     || key === 'Weight'  || key === 'WeightCut'  || key === 'RateAddLess') {
        fetchRateAndPrice(which, i, row, key);
    } else {
        priceRow(row, key);
        renderGrid(which);
    }
}

/* CommonServices.GetWagesRate (:977) - the schedule rate for this date, pack size,
   wages activity and contractor. A miss leaves the row at rate 0, as :1055 does. */
function fetchRateAndPrice(which, i, row, key) {
    if (!num(row.WagesId) || !num(row.SupplierId)) {
        row.WagesScheduleId = 0; row.Rate = 0; row.RateWithoutAddLess = 0; row.Amount = 0;
        renderGrid(which);
        return;
    }
    var url = API + '/wages-rate?docDate=' + encodeURIComponent(ymd(row.Date))
            + '&packSize=' + num(row.PackSize)
            + '&wagesAccountId=' + num(row.WagesId)
            + '&contractorId=' + num(row.SupplierId);
    getJson(url).then(function (d) {
        var rate = num(d && d.wagesRate);
        if (txt(row.WagesType) === 'Free Of Cost' || txt(row.WarehouseType) === 'Dryer') rate = 0;
        if (rate > 0) {
            row.RateWithoutAddLess = rate;
            row.WagesScheduleId = num(d.scheduleId);
            priceRow(row, key);
        } else {
            /* :1055 - no schedule row means no rate. Nothing is defaulted to 1, and the
               previous rate is not kept. */
            row.WagesScheduleId = 0; row.Rate = 0; row.RateWithoutAddLess = 0; row.Amount = 0;
        }
        renderGrid(which);
    }).catch(function (e) {
        msg('Wages rate lookup failed: ' + e.message);
        renderGrid(which);
    });
}

/* :994-1012 - Rate = RateWithoutAddLess + RateAddLess, with the add-less cap, then
   Amount = onQty ? Quantity * Rate : (BillWeight / PackSize) * Rate. */
function priceRow(row, key) {
    var base = num(row.RateWithoutAddLess);
    if (base <= 0 || num(row.BillWeight) <= 0) {
        row.Amount = 0;
        if (base <= 0) row.Rate = 0;
        return;
    }

    var addLess = num(row.RateAddLess);
    if (addLessOn() && key === 'RateAddLess') {
        var pct = cfgNum('PercentageForRateAddLess');
        if (pct > 0) {
            var cap = pct * base / 100;
            if (Math.abs(addLess) > cap) {
                addLess = addLess > 0 ? cap : -cap;
                row.RateAddLess = addLess;
                msg('RateAddLess can not be grater than RateAdLess In config ' + cap);
            }
        } else {
            addLess = 0;
            row.RateAddLess = 0;
            msg('Please set RateAddLess Percentage in config first...');
        }
    }

    var rate = base + addLess;
    row.Rate = rate;
    var amount = onQty() ? num(row.Quantity) * rate
                         : (num(row.BillWeight) / num(row.PackSize)) * rate;
    row.Amount = r2(amount);
}

/* USP_CheckItemsFreeofcostforWages, asked again at :885 with the chosen activity. */
function askFreeOfCost(which, i, row) {
    var url = API + '/free-of-cost?docDate=' + encodeURIComponent(ymd(row.Date))
            + '&refDocumentTypeId=' + S.refDocTypeId
            + '&itemId=' + num(row.ItemId)
            + '&wagesAccountId=' + num(row.WagesId);
    getJson(url).then(function (d) {
        var free = !!(d && d.isFreeOfCost);
        var want = free ? 'Free Of Cost' : 'Regular';
        if (txt(row.WagesType) !== want) {
            row.WagesType = want;
            if (free) { row.WagesScheduleId = 0; row.Rate = 0; row.RateWithoutAddLess = 0; row.Amount = 0; }
            renderGrid(which);
        }
    }).catch(function () { /* the row keeps the value the document load gave it */ });
}

/* ---------------------------------------------------------------- rows */
function blankRow() {
    return {
        SupplierId: 0, ContractorName: '', WagesId: 0, WagesAccount: '', WagesType: 'Regular',
        Date: val('txtDocDate'), packingTypeId: 0, packingType: '', Weight: 0, PackSize: 0,
        Quantity: 0, WeightCut: 0, BillQty: 0, BillWeight: 0, RateWithoutAddLess: 0,
        RateAddLess: 0, Rate: 0, Amount: 0, ItemId: 0, Item: '', jobLotId: 0, jobLot: '',
        Crop: '', MoveFromId: 0, MoveFrom: '', MoveToId: 0, MoveTo: '', PurchaseGLAC: '',
        WarehouseType: '', RefDocQty: 0, RefDocWeight: 0, RefLineId: 0, WagesScheduleId: 0,
        WagesTypeId: 0, DetailId: 0
    };
}
/* AddRowInGLGrid :1134-1208 / AddRowInStichingGrid :1227-1299 - "Add" button, Ctrl+D. A new row is a
   copy of the CURRENT row carrying what is LEFT of the document (TotalQty / GrossWeight minus the
   grid); when nothing is left it is allowed only for the exempt reference types, else the
   desktop's own message. (Regular grid: the full exempt set; Other grid: 112 / 66 only.) */
function addRowInGrid(which, i) {
    if (S.isReferred) return;
    var rows = which === 'regular' ? S.regular : S.other;
    var cur = rows[i] || rows[rows.length - 1];
    if (!cur) { msg('Please Check Grid GrossWeight and TotalGrossWeight'); return; }
    var TotalWeight = num(val('txtGrossWeight')), TotalQty = num(val('txtTotalQty'));
    var Weight = 0, WeightCut = 0, Qty = 0;
    rows.forEach(function (r) { Weight += num(r.Weight); WeightCut = num(r.WeightCut); Qty += num(r.Quantity); });
    var GrossWeight = TotalWeight - Weight, GrossQty = TotalQty - Qty;
    var d = S.refDocTypeId;
    var other = which === 'regular' ? addCondition() : (d === 112 || d === 66);
    if (!(GrossWeight > 0 || GrossQty > 0 || other)) { msg('Please Check Grid GrossWeight and TotalGrossWeight'); return; }
    var copy = JSON.parse(JSON.stringify(cur)); copy.DetailId = 0;
    if (onQty()) {
        if (GrossQty > 0) {
            var ItemWeight = GrossQty * num(cur.PackSize);
            copy.Weight = r2(ItemWeight); copy.Quantity = r2(GrossQty); copy.BillWeight = ItemWeight;
            copy.Amount = r2(GrossQty * num(cur.Rate));
        } else if (!other) { msg('Please Check Grid Qty and TotalQty'); return; }
    } else if (GrossWeight > 0) {
        var ItemQty = GrossWeight / num(cur.PackSize);
        var BillWeightPartal = GrossWeight - ItemQty * WeightCut;
        copy.Weight = GrossWeight; copy.Quantity = r2(ItemQty); copy.BillWeight = BillWeightPartal;
        copy.Amount = r2(BillWeightPartal / num(cur.PackSize) * num(cur.Rate));
    } else if (!other) { msg('Please Check Grid GrossWeight and TotalGrossWeight'); return; }
    rows.push(copy);                                   /* dtdetail.Rows.Add - appended at the end */
    renderGrid(which);
}
window.lwAddRow = function (which) { addRowInGrid(which, -1); };
window.lwAddRowAfter = function (which, i) { addRowInGrid(which, i); };
/* grdwagesDetail_ColumnButtonClick "Delete" :1104-1124 / grdStiching :1301-1321 */
window.lwDeleteRow = function (which, i) {
    if (S.isReferred) return;
    var rows = which === 'regular' ? S.regular : S.other;
    if (rows.length <= 1) { msg(which === 'regular' ? 'You Can Not Delete All rows' : 'You Can Not Delete All rows....'); return; }
    rows.splice(i, 1);
    renderGrid(which);
};

/* btnAddContractorValuesForAllDetailWages (:4587) - "Apply First Row Values for All other Rows".
   Only the contractor and the activity are copied; every other cell belongs to its own row. */
window.lwApplyAll = function (which) {
    var rows = which === 'regular' ? S.regular : S.other;
    if (!rows.length) return;                                    /* :3883 - nothing happens */
    var first = rows[0];
    /* :3889 - either value is enough (the test is OR, not AND) */
    if (!num(first.SupplierId) && !num(first.WagesId)) {
        msg('First row contains No values Of Contractor And Wages Account.');
        return;
    }
    var snap = { SupplierId: first.SupplierId, ContractorName: first.ContractorName,
                 WagesId: first.WagesId, WagesAccount: first.WagesAccount };
    rows.forEach(function (r) {
        r.SupplierId     = snap.SupplierId;
        r.ContractorName = snap.ContractorName;
        r.WagesId        = snap.WagesId;
        r.WagesAccount   = snap.WagesAccount;
    });
    renderGrid(which);
    /* :3899-3990 - every row (the first included) is asked again whether it is free of cost,
       then rated on its own date and pack size. */
    rows.forEach(function (r, i) { askFreeOfCost(which, i, r); fetchRateAndPrice(which, i, r, 'WagesId'); });
};

/* ---------------------------------------------------------------- pending tab */
/* PendingTicket() :2040-2090 - every pending document of this branch; the reference document
   type is NOT a filter here (RefDocTypeId / RefDocId are 0 unless the form was opened from a
   document). */
function loadPending() {
    return getJson(API + '/pending?refDocumentTypeId=0&refDocId=0')
        .then(function (rows) { S.pending = rows || []; renderPending(); })
        .catch(function (e) { S.pending = []; renderPending(); msg('Pending documents could not be loaded: ' + e.message); });
}

/* GridPendingTicketSetting() :2092-2119 - the tick column (Admin only) and Load sit BEFORE
   Document Type Description and are frozen; Id, DocumentTypeId, RefDocQty, RefDocWeight and
   RefLineId are hidden. */
function isAdmin() { return S.cfg.isAdmin === true || S.cfg.isAdmin === 'true'; }
function renderPending() {
    var tbl = el('grdPending'), head = el('grdPendingHead'), body = el('grdPendingBody');
    if (!head || !body) return;
    if (!S.pending.length) { head.innerHTML = ''; body.innerHTML = ''; if (tbl) tbl.style.width = ''; return; }   /* DataSource = null */
    var admin = isAdmin();
    var cols = [['Document Type Description', 150], ['Doc Date', 85], ['Doc No', 65], ['Supplier Customer', 200],
                ['Gp No', 65], ['Vehicle No', 110], ['Bilty No', 110], ['Gross Weight', 100], ['Total Qty', 100]];
    if (tbl) tbl.style.width = (cols.reduce(function (a, c) { return a + c[1]; }, 0) + 50 + (admin ? 30 : 0)) + 'px';
    head.innerHTML = (admin ? '<th style="width:30px;"><input type="checkbox" id="chkPendingAll" onclick="lwTogglePendingAll(this)"></th>' : '')
        + '<th style="width:50px;">Load</th>'
        + cols.map(function (c) { return '<th style="width:' + c[1] + 'px;">' + c[0] + '</th>'; }).join('');
    body.innerHTML = S.pending.map(function (r, i) {
        return '<tr data-i="' + i + '">'
          + (admin ? '<td class="btncell"><input type="checkbox" class="lw-pend" data-i="' + i + '"></td>' : '')
          + '<td class="btncell"><button type="button" class="cb" onclick="lwLoadPending(' + i + ')">Load</button></td>'
          + '<td>' + esc(f(r, 'DocumentTypeDescription')) + '</td>'
          + '<td>' + esc(dmy(f(r, 'DocDate'))) + '</td>'
          + '<td class="num">' + esc(f(r, 'DocNo')) + '</td>'
          + '<td>' + esc(f(r, 'SupplierCustomer')) + '</td>'
          + '<td class="num">' + esc(f(r, 'GpNo')) + '</td>'
          + '<td>' + esc(f(r, 'VehicleNo')) + '</td>'
          + '<td>' + esc(f(r, 'BiltyNo')) + '</td>'
          + '<td class="num">' + fmtN(f(r, 'GrossWeight'), 3) + '</td>'
          + '<td class="num">' + fmtN(f(r, 'TotalQty'), 3) + '</td>'
          + '</tr>';
    }).join('');
    Array.prototype.forEach.call(body.querySelectorAll('tr'), function (tr) {
        tr.addEventListener('click', function () {
            S.pendingCur = parseInt(tr.getAttribute('data-i'), 10);
            Array.prototype.forEach.call(body.querySelectorAll('tr.sel'), function (x) { x.classList.remove('sel'); });
            tr.classList.add('sel');
        });
    });
}

window.lwTogglePendingAll = function (box) {
    Array.prototype.forEach.call(document.querySelectorAll('.lw-pend'), function (c) {
        c.checked = box.checked;
    });
};

window.lwLoadAllPending = function () { /* btnLoadAll: Visible = false and an empty Click (:3535, :5324) */ };

/* BtnCancelPendingRecords_Click :4037-4070 - Admin only (:434) */
window.lwCancelPending = function () {
    var ticked = Array.prototype.filter.call(document.querySelectorAll('.lw-pend'), function (c) { return c.checked; });
    if (!ticked.length) { msg('Please select check box first'); return; }
    if (!confirm('Are you sure to Cancel Selected Pending Records?')) return;
    var rows = ticked.map(function (c) {
        var p = S.pending[parseInt(c.getAttribute('data-i'), 10)] || {};
        return { refDocumentTypeId: num(f(p, 'DocumentTypeId')), refDocId: num(f(p, 'Id')) };
    });
    var btn = el('btnCancelPending'); busy(btn, true);
    postJson(API + '/cancel-pending', { rows: rows }).then(function (res) {
        busy(btn, false);
        if (!res || !res.success) { msg((res && res.message) || 'Cancel failed.'); return; }
        alert('Record Approve Successfully');
        window.lwNew();                                 /* FormRest() */
    }).catch(function (e) { busy(btn, false); msg(e.message); });
};

/* cmbReferenceDocType is never a list to choose from: it holds exactly ONE row - the document
   type of the pending row that was loaded (:2130-2135) or of the bill that was read (:3070-3080)
   - and is empty after New. */
function bindRefDocType(id, text) {
    var sel = el('cmbRefDocType');
    if (!sel) return;
    sel.innerHTML = id ? '<option value="' + esc(id) + '">' + esc(text) + '</option>' : '';
    sel.value = id ? String(id) : '';
}

/* LoadDataForWages(), :2121-2290 */
window.lwLoadPending = function (i) {
    var p = S.pending[i];
    if (!p) return;
    /* :2125 */
    if (S.id > 0) { msg('Record Not Loaded Please Reset Form First'); return; }
    if (S.busy) return;

    tabShown('other', false);                                  /* :2136 */
    var docTypeId = num(f(p, 'DocumentTypeId'));
    S.refDocTypeId = docTypeId;
    S.refDocId     = num(f(p, 'Id'));
    bindRefDocType(docTypeId, txt(f(p, 'DocumentTypeDescription')));

    setVal('txtRefDocNo',    f(p, 'DocNo'));
    setVal('txtGrnId',       f(p, 'Id'));
    setVal('txtGpNo',        f(p, 'GpNo'));
    setVal('txtGrossWeight', f(p, 'GrossWeight'));
    setVal('txtTotalQty',    f(p, 'TotalQty'));

    /* :2150 - for these four types the entry type is the party, otherwise the document type. */
    S.reqType = (docTypeId === 66 || docTypeId === 68 || docTypeId === 808 || docTypeId === 806)
        ? txt(f(p, 'SupplierCustomer')).trim()
        : txt(f(p, 'DocumentTypeDescription')).trim();
    setVal('txtEntryType', S.reqType);

    /* :2162-2174 - ReqType is sent for 66 and 808 only; 808 sends "1" / "2". */
    var reqType = '';
    if (docTypeId === 66 || docTypeId === 808) {
        reqType = S.reqType;
        if (docTypeId === 808 && S.reqType === 'Input')  reqType = '1';
        if (docTypeId === 808 && S.reqType === 'OutPut') reqType = '2';
    }

    S.otherIds = ((docTypeId === 68 || docTypeId === 806) && S.reqType === 'MoveOrder') ? '34' : '35';
    busy(null, true);

    /* :2181 - is this document already billed?  A hit turns the form into an Update and fills the
       header from the saved bill (:2189-2201); the ROWS still come fresh from the document. */
    getJson(API + '/by-ref-doc?refDocumentTypeId=' + docTypeId
                + '&refDocNoId=' + S.refDocId
                + '&reqType=' + encodeURIComponent(reqType))
    .then(function (d) {
        S.id = (d && d.id) ? num(d.id) : 0;
        if (!S.id) return null;
        return getJson(API + '/' + S.id).then(function (b) {
            var h = (b && b.header) || {};
            setVal('txtDocDate',      ymd(f(h, 'DocDate')));
            setVal('txtDocNo',        f(h, 'DocNo'));
            setVal('txtRefDocNo',     f(h, 'RefDocNo'));
            setVal('txtGrnId',        f(h, 'RefDocNoId'));
            setVal('txtOtherRemarks', f(h, 'OtherRemarks'));
            setVal('txtGpNo',         f(h, 'ScaleSlipNo'));
            S.reqType = txt(f(h, 'RefDocument'));
            setVal('txtEntryType', S.reqType);
            S.isApproved = !!f(h, 'IsAproved');
        });
    })
    .then(function () { return loadDetailRows(docTypeId, S.refDocId, reqType); })
    .then(function () { return loadGridLookups(); })
    .then(function () {
        setSaveMode();
        renderGrid('regular');
        renderGrid('other');
        tabShown('other', S.other.length > 0 || ((docTypeId === 68 || docTypeId === 806) && S.reqType === 'MoveOrder'));
        lwTab('regular');
        busy(null, false);
    })
    .catch(function (e) {
        busy(null, false);
        msg('Could not load the document: ' + e.message);
    });
};

function loadDetailRows(docTypeId, refDocId, reqType) {
    return getJson(API + '/load-ref-doc?refDocumentTypeId=' + docTypeId
                       + '&refDocId=' + refDocId
                       + '&reqType=' + encodeURIComponent(reqType))
    .then(function (d) {
        if (d && d.error) throw new Error(d.error);
        var detail = (d && d.detail) || [];
        S.regular = [];
        S.other = [];
        if (!detail.length) return;                 /* :2203 - nothing is bound, nothing is said */

        var grossTotal = 0, qtyTotal = 0;
        detail.forEach(function (g, idx) {
            var equivalent = num(f(g, 'Equivalent'));
            var qty        = num(f(g, 'Qty'));
            var gross      = num(f(g, 'GrossWeight'));
            var free       = !!f(g, 'IsFreeOfCost');

            var row = blankRow();
            row.WagesType     = free ? 'Free Of Cost' : 'Regular';
            row.Date          = ymd(f(g, 'DocDate'));
            row.packingTypeId = num(f(g, 'PackingTypeId'));
            row.packingType   = txt(f(g, 'PackTypeCode'));
            row.PackSize      = equivalent;
            row.ItemId        = num(f(g, 'ItemId'));
            row.Item          = txt(f(g, 'ItemName'));
            row.jobLotId      = num(f(g, 'JlId'));
            row.jobLot        = txt(f(g, 'JobLotDescription'));
            row.Crop          = txt(f(g, 'CropYear'));
            row.MoveFromId    = num(f(g, 'WhFId'));
            row.MoveFrom      = txt(f(g, 'WarehouseFrom'));
            row.MoveToId      = num(f(g, 'WhTId'));
            row.MoveTo        = txt(f(g, 'WareHouseTo'));
            row.PurchaseGLAC  = txt(f(g, 'PurchaseGLAC'));
            row.WarehouseType = txt(f(g, 'WarehouseType'));
            row.RefLineId     = idx + 1;

            /* :2212 vs :2221 - the two branches differ in exactly these five values. */
            if (onQty()) {
                row.Weight       = qty * equivalent;
                row.Quantity     = qty;
                row.BillWeight   = qty * equivalent;
                row.RefDocQty    = qty;
                row.RefDocWeight = qty * equivalent;
            } else {
                row.Weight       = gross;
                row.Quantity     = equivalent ? gross / equivalent : 0;
                row.BillWeight   = gross;
                row.RefDocQty    = equivalent ? gross / equivalent : 0;
                row.RefDocWeight = gross;
            }
            grossTotal += gross;
            qtyTotal   += qty;
            S.regular.push(row);
        });

        /* :2229 - when calculating on quantity the header totals are recomputed from the rows. */
        if (onQty()) {
            setVal('txtGrossWeight', grossTotal);
            setVal('txtTotalQty',    qtyTotal);
        }

        /* :2240 - a pack size under 100 also goes to the Other Wages grid for 112 and 66;
           :2262 - for 68 / 806 with entry type MoveOrder, every row does. */
        if (docTypeId === 112 || docTypeId === 66) {
            S.other = S.regular.filter(function (r) { return num(r.PackSize) < 100; })
                               .map(function (r) { return JSON.parse(JSON.stringify(r)); });
        } else if ((docTypeId === 68 || docTypeId === 806) && S.reqType === 'MoveOrder') {
            S.other = S.regular.map(function (r) { return JSON.parse(JSON.stringify(r)); });
        }

        /* :2280 - the document's own date becomes the bill date. */
        if (S.regular.length && S.regular[0].Date) setVal('txtDocDate', S.regular[0].Date);
    });
}

/* ReadById(ID), :3054-3183 - History "Edit" / double-click. */
function detailToRow(x) {
    var row = blankRow();
    row.DetailId        = num(f(x, 'Id'));
    row.SupplierId      = num(f(x, 'ContractorId'));
    row.ContractorName  = txt(f(x, 'CompanyName') !== null ? f(x, 'CompanyName') : f(x, 'ContractorName'));
    row.WagesId         = num(f(x, 'InvConractorWagesAccountsId'));
    row.WagesAccount    = txt(f(x, 'WagesAccountName'));
    row.WagesTypeId     = num(f(x, 'WagesTypeId'));
    row.WagesType       = f(x, 'FreeOfCost') ? 'Free Of Cost' : 'Regular';
    row.Date            = ymd(f(x, 'RefDocDate'));
    row.packingTypeId   = num(f(x, 'InvPackingTypeId'));
    row.packingType     = txt(f(x, 'PackTypeDesc'));
    row.Weight          = num(f(x, 'Weight'));
    row.PackSize        = num(f(x, 'PackSize'));
    row.Quantity        = num(f(x, 'Qty'));
    row.WeightCut       = num(f(x, 'WeightCut'));
    row.BillQty         = num(f(x, 'BillQty'));
    row.BillWeight      = num(f(x, 'BillWeight'));
    /* :3121 - the stored WageRate is the FULL rate; RateWithoutAddLess = WageRate - RateAddLess. */
    row.RateAddLess     = num(f(x, 'RateAddLess'));
    row.Rate            = num(f(x, 'WageRate'));
    row.RateWithoutAddLess = num(f(x, 'WageRate')) - num(f(x, 'RateAddLess'));
    row.Amount          = num(f(x, 'WagesAmount'));
    row.ItemId          = num(f(x, 'ItemId'));
    row.Item            = txt(f(x, 'ItemName'));
    row.jobLotId        = num(f(x, 'JobLotId'));
    row.jobLot          = txt(f(x, 'JobLotDescription'));
    row.Crop            = txt(f(x, 'Crop'));
    row.MoveFromId      = num(f(x, 'WareHouseFromId'));
    row.MoveFrom        = txt(f(x, 'WareHouseFrom') !== null ? f(x, 'WareHouseFrom') : f(x, 'WarehouseFrom'));
    row.MoveToId        = num(f(x, 'WareHouseToId'));
    row.MoveTo          = txt(f(x, 'WareHouseTo'));
    row.PurchaseGLAC    = txt(f(x, 'PurchaseGLAC'));
    row.glUnknown       = f(x, 'PurchaseGLAC') === null;   /* the read did not carry the column at all */
    row.WarehouseType   = '';
    row.RefDocQty       = num(f(x, 'RefDocQty'));
    row.RefDocWeight    = num(f(x, 'RefDocWeight'));
    row.RefLineId       = num(f(x, 'RefLineId'));
    row.WagesScheduleId = num(f(x, 'InvContractorWagesScheduleId'));
    return row;
}
/* The blank Other Wages row the desktop adds for a regular row that has none (:3144, :3162). */
function fillInRow(r) {
    var o = JSON.parse(JSON.stringify(r));
    o.DetailId = 0; o.SupplierId = 0; o.ContractorName = ''; o.WagesId = 0; o.WagesAccount = '';
    o.RateWithoutAddLess = 0; o.RateAddLess = 0; o.Rate = 0; o.Amount = 0; o.WagesScheduleId = 0;
    return o;
}
function readById(id, isReferred) {
    return getJson(API + '/' + id).then(function (d) {
        var h = (d && d.header) || null, details = (d && d.details) || [];
        if (!h || !details.length) return false;               /* :3060 - silently nothing */
        S.id = id;
        S.isReferred = !!isReferred;
        tabShown('detail', false); tabShown('other', false);   /* :3067-3068 */
        lwTab('form');                                          /* :3069 */

        S.refDocTypeId = num(f(h, 'RefDocumentTypeId'));
        bindRefDocType(S.refDocTypeId, txt(f(h, 'DocumentTypeDescription')));
        setVal('txtDocDate',      ymd(f(h, 'DocDate')));
        setVal('txtDocNo',        f(h, 'DocNo'));
        setVal('txtRefDocNo',     f(h, 'RefDocNo'));
        setVal('txtGrnId',        f(h, 'RefDocNoId'));
        setVal('txtTotalQty',     f(h, 'QtyTotal'));
        setVal('txtGrossWeight',  f(h, 'WeightTotal'));
        setVal('txtOtherRemarks', f(h, 'OtherRemarks'));
        setVal('txtGpNo',         f(h, 'ScaleSlipNo'));
        S.refDocId = num(f(h, 'RefDocNoId'));
        S.reqType  = txt(f(h, 'RefDocument'));
        setVal('txtEntryType', S.reqType);
        S.isApproved = !!f(h, 'IsAproved');

        /* :3098-3125 - WagesTypeId 2 rows are the Other Wages grid; the rest are Regular, and the
           Gross Weight box becomes the sum of the regular rows' Weight. */
        var t = S.refDocTypeId, gross = 0, anyRegular = false;
        S.regular = []; S.other = []; S.otherIds = '35';
        details.forEach(function (x) {
            var row = detailToRow(x);
            if (num(f(x, 'WagesTypeId')) === 2) {
                if (t === 68 || t === 806) S.otherIds = '34';
                row.WarehouseType = txt(f(x, 'WarehouseType'));
                S.other.push(row);
            } else {
                S.regular.push(row);
                gross += row.Weight; anyRegular = true;
            }
        });
        if (anyRegular) setVal('txtGrossWeight', gross);

        /* :3132-3165 - no Other Wages rows were saved: offer blank ones for the regular rows. */
        if (!S.other.length && ((t === 66 && S.reqType !== 'Issue') || t === 112)) {
            S.otherIds = '34';
            S.other = S.regular.filter(function (r) { return num(r.PackSize) < 100; }).map(fillInRow);
        } else if (!S.other.length && (t === 68 || t === 806)) {
            S.otherIds = '34';
            S.other = S.regular.map(fillInRow);
        }
        return loadGridLookups().then(function () {
            setSaveMode();
            renderGrid('regular'); renderGrid('other');
            tabShown('other', S.other.length > 0);              /* :3168 */
            lwTab('regular');
            return true;
        });
    });
}

/* ---------------------------------------------------------------- lookups */
function loadGridLookups() {
    return getJson(API + '/grid-lookups?refDocumentTypeId=' + S.refDocTypeId + '&otherIds=' + encodeURIComponent(S.otherIds || '35'))
        .then(function (d) {
            S.contractors     = (d && d.contractors) || [];
            S.activities      = (d && d.wagesActivities) || [];
            S.otherActivities = (d && d.otherWagesActivities) || [];
        })
        .catch(function (e) {
            S.contractors = []; S.activities = []; S.otherActivities = [];
            msg('Contractor / activity lists could not be loaded: ' + e.message);
        });
}

/* DocumentTypeFillForCombo() :569-606 - the HISTORY tab's Document Type combo only
   (Usp_AllComboAgainstContractorWages, Activity = RefDocumentType -> Id / ReferenceName). */
function loadHistoryDocumentTypes() {
    return getJson(API + '/history-document-types').then(function (rows) {
        var sel = el('cmbHistDocType');
        if (!sel) return;
        var keep = sel.value;
        sel.innerHTML = '<option value=""></option>' + (rows || []).map(function (r) {
            return '<option value="' + esc(f(r, 'Id')) + '">' + esc(f(r, 'name')) + '</option>';
        }).join('');
        sel.value = keep;
    }).catch(function (e) { msg('Document types could not be loaded: ' + e.message); });
}

/* GenerateDocNo() :465-491 - RefDocumentTypeId is always 101 here, whatever the document. */
function generateDocNo() {
    return getJson(API + '/next-doc-no?refDocumentTypeId=101')
        .then(function (d) { if (d && num(d.docNo) > 0) setVal('txtDocNo', d.docNo); })
        .catch(function () { /* the box keeps its value, as :480 does on a miss */ });
}

/* CommonServices.SetRightsValueInRightsObject(base.Name) :344-347 */
function applyRights() {
    var r = S.rights;
    if (!r) return;
    var s = el('btnSave'), u = el('btnUpdate'), p = el('btnPrint002');
    if (s && r.save === false)   s.disabled = true;
    if (u && r.update === false) u.disabled = true;
    if (p && r.print === false)  p.disabled = true;
}

function setSaveMode() {
    var save = el('btnSave'), upd = el('btnUpdate'), sel = el('cmbRefDocType');
    if (save) save.classList.toggle('is-hidden', S.id > 0);
    if (upd)  upd.classList.toggle('is-hidden', !(S.id > 0));
    if (sel)  sel.disabled = S.id > 0;                 /* cmbReferenceDocType.Enabled = false (:2197, :3086) */
}

/* ---------------------------------------------------------------- save */
/* Insert(), :2702-3027 - every check, message and stored value in the form's own order.
   Returns the payload, or throws an Error carrying the desktop's message. */
function buildPayload() {
    var t = S.refDocTypeId, on = onQty();
    var stichingCompulsory = t === 112 ? cfgBool('StichingWagesCompulsory')
                           : t === 66  ? cfgBool('OtherWagesCompulsoryForStockConversion') : false;   /* :442-463 */
    var details = [];
    function need(bad, text) { if (bad) throw new Error(text); }
    function common(r) {
        var free = txt(r.WagesType) === 'Free Of Cost' || txt(r.WarehouseType) === 'Dryer';
        var d = {
            id: num(r.DetailId),
            contractorId: num(r.SupplierId),
            wagesAccountId: num(r.WagesId),
            scheduleId: free ? 0 : num(r.WagesScheduleId),
            itemId: num(r.ItemId), jobLotId: num(r.jobLotId), packingTypeId: num(r.packingTypeId),
            warehouseFromId: num(r.MoveFromId), warehouseToId: num(r.MoveToId),
            wbTransactionsIdDt: 0, jobOrderId: 0,
            refLineId: num(r.RefLineId), refDocDate: ymd(r.Date),
            refDocQty: num(r.RefDocQty), refDocWeight: num(r.RefDocWeight),
            packSize: num(r.PackSize),
            qty: num(r.Quantity),
            billQty: num(r.BillQty),                 /* :2877 - the BillQty cell wins over :2799 */
            weight: num(r.Weight),
            billWeight: num(r.Weight),               /* :2806 - wd.BillWeight is the WEIGHT cell */
            weightCut: num(r.WeightCut),
            /* :2826 - WageRate is the Rate cell, i.e. schedule rate + add/less; RateAddLess is
               stored beside it, and ReadById takes it off again (:3121). */
            wageRate: free ? 0 : num(r.Rate),
            rateAddLess: free ? 0 : num(r.RateAddLess),
            freeOfCost: txt(r.WagesType) === 'Free Of Cost' || free,
            isCompany: false,
            crop: txt(r.Crop), remarksDetail: '',
            wagesAccountName: txt(r.WagesAccount), itemName: txt(r.Item)
        };
        return d;
    }
    function amount(d) {                             /* :2846-2852 - from Weight, rounded to 2 */
        if (d.freeOfCost) return 0;
        return on ? r2(d.qty * d.wageRate) : r2(d.weight / d.packSize * d.wageRate);
    }

    need(!S.regular.length, 'Regular Wages Grid... Record Not Found');
    var docDate = val('txtDocDate');
    S.regular.forEach(function (r) {
        docDate = ymd(r.Date) || docDate;            /* :2786 - obj.DocDate follows the row's Date */
        need(!num(r.WagesId), 'WagesAccount Field required In Regular Wages Grid...');
        need(!Math.trunc(num(r.Quantity)), 'Quantity Field required In Regular Wages Grid...');
        need(!num(r.Weight), 'Weight Field required In Regular Wages Grid...');
        need(!num(r.PackSize), 'PackSize Field required In Regular Wages Grid...');
        var d = common(r);
        need(!d.freeOfCost && d.wageRate === 0, 'Rate Field required In Regular Wages Grid...');
        d.freeOfCost = txt(r.WagesType) === 'Free Of Cost';           /* :2832-2839 */
        d.wagesAmount = amount(d);
        need(!num(r.SupplierId), 'ContractorAccount Field required In Regular Wages Grid...');
        need(!num(r.ItemId), 'Item Name Field required In Regular Wages Grid...');
        need(!r.glUnknown && !Math.trunc(num(r.PurchaseGLAC)), 'PurchaseGLAC Field required In Regular Wages Grid...');
        d.wagesTypeId = 0;                           /* :2872 - ToInt of the WagesType TEXT is 0 */
        details.push(d);
    });

    /* :2885-2896 */
    if (!S.other.length && stichingCompulsory && ((t === 66 && S.reqType !== 'Issue') || t === 68 || t === 112 || t === 806)) {
        need(S.regular.some(function (r) { return num(r.PackSize) < 100; }),
             'Stitching Wages Grid... Record Not Found\n Stitching Wages Compulsory Configuration is On');
    }
    var G = 'Other Wages Grid ...', mo = S.reqType === 'MoveOrder';
    S.other.forEach(function (r) {
        /* :2901 - an untouched row is skipped unless the configuration makes it compulsory */
        if (S.reqType === 'Issue' || (!stichingCompulsory && !num(r.WagesId) && !num(r.SupplierId))) return;
        /* the desktop's "(type != 68 || type != 806)" is always true, so only MoveOrder exempts */
        need(!num(r.WagesId) && !mo, 'WagesAccount Field required In ' + G);
        need(!Math.trunc(num(r.Quantity)) && !mo, 'Quantity Field required In ' + G);
        need(!num(r.Weight) && !mo, 'Weight Field required In ' + G);
        need(!num(r.PackSize) && !mo, 'PackSize Field Required In ' + G);
        var d = common(r);
        need(!d.freeOfCost && d.wageRate === 0, 'Rate Field required In ' + G);
        if (!d.freeOfCost) d.wagesAmount = on ? r2(d.qty * d.wageRate) : r2(d.weight / d.packSize * d.wageRate);
        else d.wagesAmount = 0;
        need(!num(r.SupplierId) && !mo, 'ContractorAccount Field required In ' + G);
        need(!num(r.ItemId) && !mo, 'Item Name Field required In ' + G);
        need(!r.glUnknown && !Math.trunc(num(r.PurchaseGLAC)) && !mo, 'PurchaseGLAC Field required In ' + G);
        d.billQty = Math.trunc(num(r.BillQty));       /* :2988 ToInt */
        d.freeOfCost = txt(r.WagesType) === 'Free Of Cost';
        d.wagesTypeId = 2;                            /* :3000 */
        details.push(d);
    });

    return {
        id: S.id,
        docNo: num(val('txtDocNo')),
        docDate: docDate,
        refDocumentTypeId: S.refDocTypeId,
        refDocNo: num(val('txtRefDocNo')),
        refDocNoId: num(val('txtGrnId')),
        refDocument: txt(val('txtEntryType')).trim(),
        stockPartyId: 0, jobOrderId: 0,
        scaleSlipNo: num(val('txtGpNo')),
        projectsId: 0,
        otherRemarks: val('txtOtherRemarks'),
        qtyTotal: num(val('txtTotalQty')),            /* :3003 */
        weightTotal: num(val('txtGrossWeight')),      /* :3004 */
        isApproved: S.isApproved,
        details: details
    };
}

/* ValidationforRefRowQty / ValidationforRefRowWeight, :2634-2700 */
function refRowCheck(rows, gridName) {
    var on = onQty(), groups = {};
    rows.forEach(function (r) {
        var cap = on ? num(r.RefDocQty) : num(r.RefDocWeight);
        var k = num(r.WagesId) + '|' + num(r.RefLineId) + '|' + cap;
        if (!groups[k]) groups[k] = { wagesId: num(r.WagesId), line: num(r.RefLineId), cap: cap, total: 0 };
        groups[k].total += on ? num(r.Quantity) : num(r.BillWeight);
    });
    Object.keys(groups).forEach(function (k) {
        var g = groups[k];
        if (g.total <= g.cap) return;
        var a = S.activities.filter(function (x) { return num(f(x, 'Id')) === g.wagesId; })[0];
        var name = a ? txt(f(a, 'WagesAccountName')) : '';
        if (on) throw new Error('TotalQty against Reference RowNo and Wages Account Should be Equal to or less than Reference Row Qty\n'
            + 'Here TotalQty (' + g.total + ') exceeds Reference Row Qty (' + g.cap + ') for WagesAccount (' + name + ') and RowNo ' + g.line + ' in ' + gridName + ' Grid');
        throw new Error('TotalBillWeight against Reference RowNo and Account Should be Equal to or less than Reference Row Weight\n'
            + 'Here TotalBillWeight (' + g.total + ') exceeds Reference Row Weight (' + g.cap + ') for WagesAccount (' + name + ') and RowNo ' + g.line + '  in ' + gridName + ' Grid');
    });
}

/* FormValidation, :2568-2595 */
function formValidation() {
    if (!num(val('txtDocNo')))    { msg('document Number Field Required'); return false; }
    if (!S.refDocTypeId || !val('cmbRefDocType')) { msg('ReferenceDocType Field Required'); var c = el('cmbRefDocType'); if (c) c.focus(); return false; }
    if (!num(val('txtRefDocNo'))) { msg('Doc No Field Required'); return false; }
    if (!num(val('txtGrnId')))    { msg('DocNoId Field Required'); return false; }
    return true;
}

function doSave(btn, isUpdate) {
    if (S.busy) return;
    if (!formValidation()) return;
    var t = S.refDocTypeId;
    /* :2714 */
    if (!onQty() && (t === 68 || t === 806)) {
        var ow = S.other.reduce(function (a, r) { return a + num(r.Weight); }, 0);
        if (ow !== num(val('txtGrossWeight'))) { msg('OutPut Grid Total Weight and GrossWeight not equal please check!'); return; }
    }
    if (!window.confirm(S.id > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
    var p;
    try {
        if (addCondition()) {                         /* :2734-2746 */
            refRowCheck(S.regular, 'Regular_Wages');
            refRowCheck(S.other, 'Other_Wages');
        }
        p = buildPayload();
    } catch (e) { msg(e.message); return; }

    busy(btn, true);
    postJson(API + '/save', p).then(function (res) {
        busy(btn, false);
        if (res && res.success) {
            msg((S.id > 0 ? 'Updated SuccessFully  [' : 'Saved SuccessFully  [') + p.docNo + ']');   /* :3008-3013 */
            window.lwNew();                           /* FormRest() :3020 */
        } else {
            msg((res && res.message) || 'Save failed.');
        }
    }).catch(function (e) {
        busy(btn, false);
        msg('Save failed: ' + e.message);
    });
}

window.lwSave   = function () { doSave(el('btnSave'), false); };
window.lwUpdate = function () { doSave(el('btnUpdate'), true); };
/* Print_Click :3817-3827 -> SlipPrint_002(RECID) -> CommonServices.ContractorWagesBill_SlipandRegister_002 */
window.lwPrint002 = function () {
    return slipPrint002(S.id, 'btnPrint002');
};
/* SlipPrint_002(PrintId) :3829 */
function slipPrint002(id, btn) {
    if (!window.CrystalPrint) { msg('countx_crystal_print.js is not loaded.'); return; }
    return window.CrystalPrint.open('wages-002', { id: num(id) }, btn || null);
}
/* btnnew_Click :2444 / Ctrl+E - ValidationOnformClose :2597-2632 */
function validationOnFormClose(resetOrClose) {
    var rows = S.regular;
    if (!(rows.length > 0 && S.prevHeaderQty !== null)) return Promise.resolve(true);
    var curQty = num(val('txtTotalQty')), curW = num(val('txtGrossWeight'));
    if (onQty()) {
        if (curQty !== num(S.prevHeaderQty)) {
            if (!confirm('Current Qty Does not match with Previous Qty\nIf You ' + resetOrClose + ' the Form Previous Saved Record Will Be Deleted\nAre you Sure to Do So???')) return Promise.resolve(false);
            return postJson(API + '/delete-by-ref?refDocumentTypeId=' + S.refDocTypeId + '&refDocId=' + S.refDocId, {}).then(function () { return true; });
        }
    } else if (curW !== num(S.prevHeaderWeight)) {
        if (!confirm('Current Weight Does not match with Previous Weight\nIf You ' + resetOrClose + ' the Form Previous Saved Record Will Be Deleted\nAre you Sure to Do So???')) return Promise.resolve(false);
        /* :2624 - the desktop passes RECID here, not RefDocId */
        return postJson(API + '/delete-by-ref?refDocumentTypeId=' + S.refDocTypeId + '&refDocId=' + S.id, {}).then(function () { return true; });
    }
    return Promise.resolve(true);
}
window.lwNewClick = function () { validationOnFormClose('Reset').then(function (ok) { if (ok) window.lwNew(); }); };
window.lwClose = function () {
    /* :3752 - RefDocTypeId / RefDocId are the form's OPEN arguments; this page is never opened from a document. */
    validationOnFormClose('Close').then(function (ok) { if (ok) window.location.href = '/accounts/vouchers/contractor-wages-dashboard'; });
};

/* ---------------------------------------------------------------- new / refresh */
/* FormRest(), :2459-2499 */
window.lwNew = function () {
    S.id = 0; S.refDocTypeId = 0; S.refDocId = 0; S.reqType = ''; S.isReferred = false; S.isApproved = false;
    S.regular = []; S.other = []; S.otherIds = '35';
    S.prevHeaderQty = null; S.prevHeaderWeight = null;
    bindRefDocType(0, '');
    setVal('txtRefDocNo', ''); setVal('txtGpNo', ''); setVal('txtOtherRemarks', '');
    setVal('txtTotalQty', ''); setVal('txtGrossWeight', ''); setVal('txtGrnId', ''); setVal('txtEntryType', '');
    generateDocNo();
    setSaveMode();
    renderGrid('regular'); renderGrid('other');
    loadPending();                                    /* PendingTicket() */
    tabShown('other', false); tabShown('detail', false);
    lwTab('regular'); lwTab('pending');
};

/* btnRefresh_Click, :2531-2547 - configuration and the two grid lists are read again; the form
   and its rows are NOT reset. */
window.lwRefresh = function () {
    if (S.busy) return;
    var btn = el('btnRefresh'); busy(btn, true);
    getJson(API + '/config-flags').then(function (c) { var adm = S.cfg.isAdmin; S.cfg = c || {}; if (S.cfg.isAdmin === undefined) S.cfg.isAdmin = adm; })
        .catch(function () { })
        .then(loadGridLookups)
        .then(function () { busy(btn, false); renderGrid('regular'); renderGrid('other'); });
};

/* ---------------------------------------------------------------- history tab */
/* bindHistory() :3247-3310 + HistoryGridSettings() :3312-3381. Edit, Slip and Voucher are the
   first three (frozen) columns; Id, DocumentTypeId, RefDocumentTypeId, RefDocNoId, JobOrderId and
   IsReferred are hidden; the three totals are summed. */
var HIST_COLS = [
    { k: 'DocumentTypeDescription', c: 'Document Type', w: 150 },
    { k: 'DocDate',     c: 'Doc Date',     w: 95,  date: 1 },
    { k: 'DocNo',       c: 'Doc No',       w: 70,  num: 1, plain: 1 },
    { k: 'RefDocNo',    c: 'Ref Doc No',   w: 70,  num: 1, plain: 1 },
    { k: 'QtyTotal',    c: 'Qty Total',    w: 90,  num: 1, sum: 1 },
    { k: 'WeightTotal', c: 'Weight Total', w: 100, num: 1, sum: 1 },
    { k: 'WagesAmount', c: 'Wages Amount', w: 110, num: 1, sum: 1 },
    { k: 'OtherRemarks', c: 'Other Remarks', w: 200 },
    { k: 'JobOrderNo',  c: 'Job Order No', w: 100 },
    { k: 'EntryDate',   c: 'Entry Date',   w: 150, date: 2 },
    { k: 'UserName',    c: 'Entry User',   w: 120 },
    { k: 'ModifyDate',  c: 'Modify Date',  w: 150, date: 2 },
    { k: 'ModifyUser',  c: 'Modify User',  w: 120 }
];
function renderHistory() {
    var tbl = el('grdHistory'), head = el('grdHistoryHead'), body = el('grdHistoryBody'), foot = el('grdHistoryFoot');
    if (!head || !body) return;
    var rows = S.historyRows || [];
    if (!rows.length) {                                /* DataGridHistory.ClearStructure() */
        head.innerHTML = ''; body.innerHTML = ''; if (foot) foot.innerHTML = ''; if (tbl) tbl.style.width = '';
        renderHistoryDetail(null);
        return;
    }
    if (tbl) tbl.style.width = (HIST_COLS.reduce(function (a, c) { return a + c.w; }, 0) + 140) + 'px';
    head.innerHTML = '<th style="width:40px;">Edit</th><th style="width:40px;">Slip</th><th style="width:60px;">Voucher</th>'
        + HIST_COLS.map(function (c) { return '<th style="width:' + c.w + 'px;">' + c.c + '</th>'; }).join('');
    body.innerHTML = rows.map(function (r, i) {
        return '<tr data-i="' + i + '">'
            + '<td class="btncell"><button type="button" class="cb" data-act="Edit">Edit</button></td>'
            + '<td class="btncell"><button type="button" class="cb" data-act="Slip">Slip</button></td>'
            + '<td class="btncell"><button type="button" class="cb" data-act="Voucher">Voucher</button></td>'
            + HIST_COLS.map(function (c) {
                var v = f(r, c.k);
                if (c.date) v = dmy(v, c.date === 2);
                else if (c.num && !c.plain) v = fmtN(v, 0);            /* "#,#" */
                return '<td' + (c.num ? ' class="num"' : '') + '>' + esc(v) + '</td>';
            }).join('') + '</tr>';
    }).join('');
    if (foot) {
        foot.innerHTML = '<td></td><td></td><td></td>' + HIST_COLS.map(function (c) {
            if (!c.sum) return '<td></td>';
            return '<td class="num">' + fmtN(rows.reduce(function (a, r) { return a + num(f(r, c.k)); }, 0), 0) + '</td>';
        }).join('');
    }
}
function historySelect(i) {
    var body = el('grdHistoryBody');
    Array.prototype.forEach.call(body.querySelectorAll('tr.sel'), function (x) { x.classList.remove('sel'); });
    var tr = body.querySelector('tr[data-i="' + i + '"]'); if (tr) tr.classList.add('sel');
    if (S.historyCur === i) return;
    S.historyCur = i;
    var r = S.historyRows[i]; if (!r) return;
    /* DataGridHistory_SelectionChanged :3539 -> HistoryDetailGridBind(Id) */
    getJson(API + '/' + num(f(r, 'Id'))).then(function (d) { if (S.historyCur === i) renderHistoryDetail((d && d.details) || []); })
        .catch(function (e) { msg('History detail failed: ' + e.message); });
}
/* HistoryDetailGridBind / HistoryDetailGridSettings :3445-3533 */
function renderHistoryDetail(details) {
    var tbl = el('grdHistoryDetail'), head = el('grdHistoryDetailHead'), body = el('grdHistoryDetailBody'), foot = el('grdHistoryDetailFoot');
    if (!head || !body) return;
    if (!details || !details.length) { head.innerHTML = ''; body.innerHTML = ''; if (foot) foot.innerHTML = ''; if (tbl) tbl.style.width = ''; return; }
    var al = addLessOn();
    var cols = [
        { c: 'Contractor Name', w: 150, v: function (x) { var n = f(x, 'CompanyName'); return txt(n !== null ? n : f(x, 'ContractorName')); } },
        { c: 'Labour / Wages Activity', w: 200, v: function (x) { return txt(f(x, 'WagesAccountName')); } },
        { c: 'Packing Type', w: 95, v: function (x) { return txt(f(x, 'PackTypeDesc')); } },
        { c: 'Weight', w: 90, n: 0, sum: 1, v: function (x) { return num(f(x, 'Weight')); } },
        { c: 'Pack Size', w: 70, v: function (x) { return txt(f(x, 'PackSize')); } },
        { c: 'Quantity', w: 80, n: 0, sum: 1, v: function (x) { return num(f(x, 'Qty')); } },
        { c: 'Weight Cut', w: 70, v: function (x) { return txt(f(x, 'WeightCut')); } },
        { c: 'Bill Weight', w: 90, n: 0, sum: 1, v: function (x) { return num(f(x, 'BillWeight')); } }
    ];
    if (al) {
        cols.push({ c: 'Rate Without Add Less', w: 100, n: 2, v: function (x) { return num(f(x, 'WageRate')) - num(f(x, 'RateAddLess')); } });
        cols.push({ c: 'Rate Add Less', w: 80, n: 2, sum: 1, v: function (x) { return num(f(x, 'RateAddLess')); } });
    }
    cols.push({ c: 'Rate', w: 80, right: 1, v: function (x) { return txt(f(x, 'WageRate')); } });
    cols.push({ c: 'Amount', w: 100, n: 0, sum: 1, v: function (x) { return num(f(x, 'WagesAmount')); } });
    if (tbl) tbl.style.width = cols.reduce(function (a, c) { return a + c.w; }, 0) + 'px';
    head.innerHTML = cols.map(function (c) { return '<th style="width:' + c.w + 'px;">' + c.c + '</th>'; }).join('');
    body.innerHTML = details.map(function (x) {
        return '<tr>' + cols.map(function (c) {
            var v = c.v(x);
            return '<td' + ((c.n !== undefined || c.right) ? ' class="num"' : '') + '>' + (c.n !== undefined ? fmtN(v, c.n) : esc(v)) + '</td>';
        }).join('') + '</tr>';
    }).join('');
    if (foot) foot.innerHTML = cols.map(function (c) {
        if (!c.sum) return '<td></td>';
        return '<td class="num">' + fmtN(details.reduce(function (a, x) { return a + num(c.v(x)); }, 0), c.n) + '</td>';
    }).join('');
}
function historyEdit(i) {
    var r = S.historyRows[i]; if (!r) return;
    /* DataGridHistory_DoubleClick / "Edit" :3383-3443 */
    readById(num(f(r, 'Id')), num(f(r, 'IsReferred')) === 1)
        .catch(function (e) { msg('Could not open that bill: ' + e.message); });
}
function historyVoucher(i) {
    var r = S.historyRows[i]; if (!r) return;
    /* VoucherReport_118(VoucherHeadIdGet(Id, 101)) :3417 */
    var win = window.CrystalPrint ? CrystalPrint.reserve() : null;
    getJson(API + '/voucher-head/' + num(f(r, 'Id'))).then(function (d) {
        var vh = num(d && d.voucherHeadId);
        if (!vh) { if (window.CrystalPrint) CrystalPrint.release(win); msg('VoucherId Not Found'); return; }
        CrystalPrint.open('acc-118', { id: vh, documentTypeId: 101 }, null, win);
    }).catch(function (e) { if (window.CrystalPrint) CrystalPrint.release(win); msg(e.message); });
}

window.lwShowHistory = function () {
    var btn = el('btnShowHistory');
    if (S.busy) return;
    busy(btn, true);
    var q = '?fromDate='  + encodeURIComponent(val('histFromDate'))
          + '&toDate='    + encodeURIComponent(val('histToDate'))
          + '&fromDocNo=' + (num(val('histFromDocNo')) || 0)
          + '&toDocNo='   + (num(val('histToDocNo')) || 0)
          + '&refDocumentTypeId=' + (num(val('cmbHistDocType')) || 0);
    getJson(API + '/history' + q).then(function (rows) {
        busy(btn, false);
        S.historyRows = rows || []; S.historyCur = -1;
        renderHistory();
        if (S.historyRows.length) historySelect(0);
    }).catch(function (e) {
        busy(btn, false);
        msg('History failed: ' + e.message);
    });
};

/* btnResetHistory_Click :3568-3584 */
window.lwResetHistory = function () {
    setVal('histFromDate', today()); setVal('histToDate', today());
    setVal('histFromDocNo', ''); setVal('histToDocNo', '');
    var s = el('cmbHistDocType'); if (s) s.value = '';
    S.historyRows = []; S.historyCur = -1;
    renderHistory();
};
/* btnRefreshHistory_Click :3556 */
window.lwRefreshHistory = function () { loadHistoryDocumentTypes(); };

/* The footer History button (right-hand side, per the brief). */
window.lwFormHistory = function () {
    var btn = el('btnFormHistory');
    if (S.busy) return;
    busy(btn, true);
    getJson(API + '/form-history').then(function (rows) {
        busy(btn, false);
        S.formHistoryRows = rows || [];
        renderTable('grdFormHistoryHead', 'grdFormHistoryBody', S.formHistoryRows, function (r, i) {
            return 'onclick="lwOpenFromHistory(' + i + ')" style="cursor:pointer;"';
        });
        var m = el('lwFormHistoryModal'); if (m) m.classList.remove('is-hidden');
    }).catch(function (e) {
        busy(btn, false);
        msg('Form history failed: ' + e.message);
    });
};
window.lwCloseFormHistory = function () {
    var m = el('lwFormHistoryModal'); if (m) m.classList.add('is-hidden');
};
window.lwOpenFromHistory = function (i) {
    var r = (S.formHistoryRows || [])[i];
    if (!r) return;
    var id = num(f(r, 'Id'));
    if (!id) { msg('That row carries no Id.'); return; }
    readById(id, num(f(r, 'IsReferred')) === 1)
        .then(function () { window.lwCloseFormHistory(); })
        .catch(function (e) { msg('Could not open that bill: ' + e.message); });
};

/* Generic table renderer - the History grids show whatever columns the procedure returns,
   which is what the desktop does too (RetrieveStructure() off the DataTable). */
function renderTable(headId, bodyId, rows, rowAttrs) {
    var head = el(headId), body = el(bodyId);
    if (!head || !body) return;
    rows = rows || [];
    if (!rows.length) {
        head.innerHTML = ''; body.innerHTML = '';
        return;
    }
    var keys = Object.keys(rows[0]);
    head.innerHTML = keys.map(function (k) { return '<th>' + esc(k) + '</th>'; }).join('');
    body.innerHTML = rows.map(function (r, i) {
        return '<tr ' + (rowAttrs ? rowAttrs(r, i) : '') + '>' + keys.map(function (k) {
            var v = r[k];
            if (String(k).toLowerCase().indexOf('date') >= 0) v = ymd(v);
            else if (typeof v === 'number') v = fmt(v, 2);
            return '<td>' + esc(v) + '</td>';
        }).join('') + '</tr>';
    }).join('');
}

window.lwTab = lwTab;

/* ---------------------------------------------------------------- boot */
function boot() {
    setVal('txtDocDate', today());
    setVal('histFromDate', today()); setVal('histToDate', today());

    /* History grid: one click selects (detail grid follows), double-click edits, buttons act. */
    var hb = el('grdHistoryBody');
    if (hb) {
        hb.addEventListener('click', function (e) {
            var tr = e.target.closest('tr'); if (!tr) return;
            var i = parseInt(tr.getAttribute('data-i'), 10);
            historySelect(i);
            var a = e.target.getAttribute && e.target.getAttribute('data-act');
            if (a === 'Edit') historyEdit(i);
            if (a === 'Slip') slipPrint002(f(S.historyRows[i], 'Id'));
            if (a === 'Voucher') historyVoucher(i);
        });
        hb.addEventListener('dblclick', function (e) {
            if (e.target.getAttribute && e.target.getAttribute('data-act')) return;
            var tr = e.target.closest('tr'); if (tr) historyEdit(parseInt(tr.getAttribute('data-i'), 10));
        });
    }
    /* FromDocNoHistory / ToDocNoHistory KeyPress :4013-4035 - digits only */
    ['histFromDocNo', 'histToDocNo'].forEach(function (id) {
        var x = el(id); if (x) x.addEventListener('input', function () { x.value = x.value.replace(/[^0-9]/g, ''); });
    });

    /* frmwagesBillHeader_KeyDown :3706-3815 */
    document.addEventListener('keydown', function (e) {
        if (e.defaultPrevented) return;
        var k = String(e.key || '').toLowerCase();
        var onForm = onFormTab();
        var save = el('btnSave'), upd = el('btnUpdate'), prt = el('btnPrint002');
        function live(b) { return b && !b.classList.contains('is-hidden') && !b.disabled; }
        if (e.key === 'Enter' && e.target && /^(INPUT|SELECT)$/.test(e.target.tagName) && e.target.type !== 'button') {
            var fl = Array.prototype.filter.call(document.querySelectorAll('input,select,textarea,button'), function (x) { return !x.disabled && x.offsetParent !== null && x.tabIndex >= 0; });
            var ix = fl.indexOf(e.target); if (ix >= 0 && ix + 1 < fl.length) { e.preventDefault(); fl[ix + 1].focus(); }
            return;
        }
        if (!e.ctrlKey && k !== 'escape') return;
        if (e.ctrlKey && k === 's') { e.preventDefault(); if (onForm) { if (live(save)) window.lwSave(); } else window.lwShowHistory(); }
        if (e.ctrlKey && k === 'n') { e.preventDefault(); if (onForm) window.lwNewClick(); else window.lwResetHistory(); }
        if (e.ctrlKey && k === 't') { e.preventDefault(); if (onForm) lwTab('history'); else { lwTab('form'); var rd = el('cmbRefDocType'); if (rd) rd.focus(); } }
        if ((e.ctrlKey && k === 'e') || k === 'escape') { e.preventDefault(); window.lwClose(); }
        if (e.ctrlKey && k === 'u') { e.preventDefault(); if (live(upd)) window.lwUpdate(); }
        if (e.ctrlKey && k === 'p') { e.preventDefault(); if (live(prt)) window.lwPrint002(); }
        if (e.ctrlKey && k === 'd' && onForm && !S.isReferred) {
            e.preventDefault();
            var g = e.target.closest ? e.target.closest('[data-grid]') : null;
            if (g) addRowInGrid(g.getAttribute('data-grid'), -1);
        }
        if (e.ctrlKey && k === 'arrowdown') { e.preventDefault(); var gd = el(onForm ? 'wrapPending' : 'wrapHistory'); if (gd) gd.focus(); }
        if (e.ctrlKey && k === 'arrowup')   { e.preventDefault(); var gu = el(onForm ? 'wrapRegular' : 'histFromDate'); if (gu) gu.focus(); }
        if (e.ctrlKey && k === 'f5' && onForm) { e.preventDefault(); var dd = el('txtDocDate'); if (dd) dd.focus(); }
        /* Ctrl+L - LoadDataForWages() on the pending grid's current row */
        if (e.ctrlKey && k === 'l' && onForm) { e.preventDefault(); if (S.pendingCur >= 0) window.lwLoadPending(S.pendingCur); var gr = el('wrapRegular'); if (gr) gr.focus(); }
    });

    /* frmwagesBillHeader_Load :321-440, standalone (no RefDocTypeId / RefDocId) */
    tabShown('other', false); tabShown('detail', false);
    S.pendingCur = -1; S.historyCur = -1;
    getJson(API + '/config-flags')
        .then(function (c) { S.cfg = c || {}; })
        .catch(function () { S.cfg = {}; })
        .then(function () {
            /* :434 - Cancel Records is for Admin, and only when not opened from a document */
            var cb = el('btnCancelPending'); if (cb) cb.classList.toggle('is-hidden', !isAdmin());
            /* :344-347 - form rights; an Admin has them all */
            return getJson('/api/contractor-wages/schedule/rights?screen=frmwagesBillHeader')
                .then(function (r) { S.rights = isAdmin() ? null : (r || null); })
                .catch(function () { S.rights = null; });
        })
        .then(function () {
            applyRights();
            setSaveMode();
            generateDocNo();
            loadGridLookups();
            loadPending();                              /* PendingTicket() */
            loadHistoryDocumentTypes();                 /* DocumentTypeFillForCombo() */
            renderGrid('regular');
            renderGrid('other');
        });
}

if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', boot);
else boot();

})();
