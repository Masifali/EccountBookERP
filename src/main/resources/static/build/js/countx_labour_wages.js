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
    cfg: {},
    busy: false,
    formHistoryRows: [],
    historyRows: []
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

function msg(text, ok) {
    var m = el('cwMessage');
    if (!m) return;
    m.className = ok ? 'ok' : 'err';
    m.textContent = text;
    if (ok) { setTimeout(function () { if (m.textContent === text) m.className = ''; }, 6000); }
}
function clearMsg() { var m = el('cwMessage'); if (m) { m.className = ''; m.textContent = ''; } }

/* Button click -> disabled + loader immediately, re-enabled on success or failure,
   and a second click while in flight is dropped. */
function busy(btn, on) {
    S.busy = !!on;
    if (btn) { btn.disabled = !!on; btn.classList.toggle('btn-busy', !!on); }
    ['btnSave', 'btnUpdate', 'btnNew', 'btnRefresh'].forEach(function (id) {
        var b = el(id); if (b && b !== btn) b.disabled = !!on;
    });
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
var TABS = ['form', 'pending', 'regular', 'other', 'detail', 'history'];
function lwTab(name) {
    TABS.forEach(function (t) {
        var v = el('view' + t.charAt(0).toUpperCase() + t.slice(1));
        var b = el('tab'  + t.charAt(0).toUpperCase() + t.slice(1));
        if (v) v.style.display = (t === name) ? '' : 'none';
        if (b) b.classList.toggle('active', t === name);
    });
}

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
function columns() {
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
    if (addCondition()) cols.push({ k: 'RefLineId', c: 'RowNo', t: 'text', w: 55 });
    return cols;
}

/* ---------------------------------------------------------------- grid render */
function renderGrid(which) {
    var rows = which === 'regular' ? S.regular : S.other;
    var head = el(which === 'regular' ? 'grdRegularHead' : 'grdOtherHead');
    var body = el(which === 'regular' ? 'grdRegularBody' : 'grdOtherBody');
    var foot = el(which === 'regular' ? 'grdRegularFoot' : 'grdOtherFoot');
    if (!head || !body) return;

    var cols = columns();

    head.innerHTML = cols.map(function (c) {
        return '<th style="width:' + c.w + 'px;' + (c.t === 'num' ? 'text-align:right;' : '') + '">'
             + esc(c.c) + '</th>';
    }).join('') + '<th style="width:40px;">Add</th><th style="width:26px;">X</th>';

    if (!rows.length) {
        body.innerHTML = '<tr><td colspan="' + (cols.length + 2)
            + '" style="text-align:center; padding:14px; color:#777;">'
            + 'Load a reference document from the Pending tab, or press Add Row.</td></tr>';
        if (foot) foot.innerHTML = '';
        updateTotals();
        return;
    }

    body.innerHTML = rows.map(function (row, i) {
        var freeOfCost = txt(row.WagesType) === 'Free Of Cost';
        var tds = cols.map(function (c) {
            var style = (c.t === 'num' ? 'text-align:right;' : '')
                      + (c.k === 'WagesType' && freeOfCost ? 'color:#d00; font-weight:bold;' : '');
            if (c.t === 'contractor') {
                return '<td>' + selectHtml(which, i, 'SupplierId', row.SupplierId,
                                           S.contractors, 'Id', 'CompanyName') + '</td>';
            }
            if (c.t === 'activity') {
                return '<td>' + selectHtml(which, i, 'WagesId', row.WagesId,
                                           S.activities, 'Id', 'WagesAccountName') + '</td>';
            }
            if (c.edit) {
                return '<td style="' + style + '"><input type="number" step="any" class="win-input lw-cell"'
                     + ' data-g="' + which + '" data-i="' + i + '" data-k="' + c.k + '"'
                     + ' style="text-align:right; width:100%;" value="' + esc(row[c.k]) + '"></td>';
            }
            var shown = c.t === 'num' ? fmt(row[c.k], c.dec) : esc(row[c.k]);
            return '<td style="' + style + '">' + shown + '</td>';
        }).join('');
        return '<tr>' + tds
             + '<td style="text-align:center;"><button type="button" class="btn-add-green"'
             + ' onclick="lwAddRowAfter(\'' + which + '\',' + i + ')" title="Add a copy of this row">+</button></td>'
             + '<td style="text-align:center;"><button type="button" class="win-btn"'
             + ' style="padding:1px 6px;" onclick="lwDeleteRow(\'' + which + '\',' + i + ')" title="Remove this row">X</button></td>'
             + '</tr>';
    }).join('');

    if (foot) {
        foot.innerHTML = cols.map(function (c) {
            if (!c.sum) return '<td></td>';
            var s = rows.reduce(function (a, r) { return a + num(r[c.k]); }, 0);
            return '<td style="text-align:right; font-weight:bold;">' + fmt(s, c.dec) + '</td>';
        }).join('') + '<td></td><td></td>';
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

function selectHtml(which, i, key, value, list, idField, textField) {
    var out = '<select class="win-input lw-sel" data-g="' + which + '" data-i="' + i
            + '" data-k="' + key + '" style="width:100%;">';
    out += '<option value="">...Select Any Value...</option>';
    for (var n = 0; n < list.length; n++) {
        var id = f(list[n], idField);
        var tx = f(list[n], textField);
        out += '<option value="' + esc(id) + '"'
             + (String(id) === String(value) ? ' selected' : '') + '>' + esc(tx) + '</option>';
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
            var act = S.activities.filter(function (a) { return String(f(a, 'Id')) === String(value); })[0];
            row.WagesAccount = act ? txt(f(act, 'WagesAccountName')) : '';
            row.WagesTypeId  = act ? num(f(act, 'WagesTypeId')) : 0;
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

    if (warn) msg(warn, false);

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
        msg('Wages rate lookup failed: ' + e.message, false);
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
                msg('RateAddLess can not be grater than RateAdLess In config ' + cap, false);
            }
        } else {
            addLess = 0;
            row.RateAddLess = 0;
            msg('Please set RateAddLess Percentage in config first...', false);
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
window.lwAddRow = function (which) {
    var rows = which === 'regular' ? S.regular : S.other;
    rows.push(blankRow());
    renderGrid(which);
};
window.lwAddRowAfter = function (which, i) {
    var rows = which === 'regular' ? S.regular : S.other;
    var copy = JSON.parse(JSON.stringify(rows[i] || blankRow()));
    copy.DetailId = 0;              // a new row, never an update of the one it was copied from
    rows.splice(i + 1, 0, copy);
    renderGrid(which);
};
window.lwDeleteRow = function (which, i) {
    var rows = which === 'regular' ? S.regular : S.other;
    rows.splice(i, 1);
    renderGrid(which);
};

/* btnAddContractorValuesForAllDetailWages (:4587) - "Apply First Row Values for All other Rows".
   Only the contractor and the activity are copied; every other cell belongs to its own row. */
window.lwApplyAll = function (which) {
    var rows = which === 'regular' ? S.regular : S.other;
    if (!rows.length) { msg('There are no rows to apply to.', false); return; }
    var first = rows[0];
    if (!num(first.SupplierId) || !num(first.WagesId)) {
        msg('First row contains No values Of Contractor And Wages Account.', false);
        return;
    }
    rows.forEach(function (r, i) {
        if (i === 0) return;
        r.SupplierId     = first.SupplierId;
        r.ContractorName = first.ContractorName;
        r.WagesId        = first.WagesId;
        r.WagesAccount   = first.WagesAccount;
        r.WagesTypeId    = first.WagesTypeId;
    });
    renderGrid(which);
    /* Each row is then rated on its own date and pack size, as the desktop does at :3920. */
    rows.forEach(function (r, i) { if (i > 0) fetchRateAndPrice(which, i, r, 'WagesId'); });
};

/* ---------------------------------------------------------------- pending tab */
function loadPending() {
    var body = el('grdPendingBody');
    if (body) {
        body.innerHTML = '<tr><td colspan="11" style="text-align:center; padding:14px; color:#777;">Loading...</td></tr>';
    }
    getJson(API + '/pending?refDocumentTypeId=' + S.refDocTypeId + '&refDocId=0')
        .then(function (rows) { S.pending = rows || []; renderPending(); })
        .catch(function (e) {
            S.pending = [];
            if (body) {
                body.innerHTML = '<tr><td colspan="11" style="text-align:center; padding:14px; color:#a3160b;">'
                    + esc(e.message) + '</td></tr>';
            }
        });
}

function renderPending() {
    var body = el('grdPendingBody');
    if (!body) return;
    if (!S.pending.length) {
        body.innerHTML = '<tr><td colspan="11" style="text-align:center; padding:14px; color:#777;">'
            + 'No pending documents for this reference document type.</td></tr>';
        return;
    }
    body.innerHTML = S.pending.map(function (r, i) {
        return '<tr>'
          + '<td style="text-align:center;"><input type="checkbox" class="lw-pend" data-i="' + i + '"></td>'
          + '<td>' + esc(f(r, 'DocumentTypeDescription')) + '</td>'
          + '<td>' + esc(ymd(f(r, 'DocDate'))) + '</td>'
          /* The document code is clickable and opens its own record, per the brief. */
          + '<td><a href="#" onclick="lwLoadPending(' + i + '); return false;"'
          + ' style="color:#0f5959; font-weight:bold;">' + esc(f(r, 'DocNo')) + '</a></td>'
          + '<td>' + esc(f(r, 'SupplierCustomer')) + '</td>'
          + '<td>' + esc(f(r, 'GpNo')) + '</td>'
          + '<td>' + esc(f(r, 'VehicleNo')) + '</td>'
          + '<td>' + esc(f(r, 'BiltyNo')) + '</td>'
          + '<td style="text-align:right;">' + fmt(f(r, 'GrossWeight'), 3) + '</td>'
          + '<td style="text-align:right;">' + fmt(f(r, 'TotalQty'), 3) + '</td>'
          + '<td style="text-align:center;"><button type="button" class="win-btn" style="padding:1px 8px;"'
          + ' onclick="lwLoadPending(' + i + ')">Load</button></td>'
          + '</tr>';
    }).join('');
}

window.lwTogglePendingAll = function (box) {
    Array.prototype.forEach.call(document.querySelectorAll('.lw-pend'), function (c) {
        c.checked = box.checked;
    });
};

window.lwLoadAllPending = function () {
    /* The desktop loads one document at a time - gridPendingWagesSlip_ColumnButtonClick (:2025)
       calls LoadDataForWages() for the CURRENT row only, and the header fields it fills
       (Doc No, GRN Id, Gp No, Gross Weight, Total Qty) hold exactly one document. Rather than
       invent a multi-document save the desktop has no procedure for, the first ticked row is
       loaded and the user is told. */
    var ticked = Array.prototype.filter.call(document.querySelectorAll('.lw-pend'),
                                             function (c) { return c.checked; });
    if (!ticked.length) { msg('Tick a document first, or press its Load button.', false); return; }
    if (ticked.length > 1) {
        msg('One bill is raised against one document - loading the first ticked row ('
            + ticked.length + ' were ticked).', false);
    }
    window.lwLoadPending(parseInt(ticked[0].getAttribute('data-i'), 10));
};

window.lwCancelPending = function () {
    Array.prototype.forEach.call(document.querySelectorAll('.lw-pend'), function (c) { c.checked = false; });
    var all = el('chkPendingAll'); if (all) all.checked = false;
    clearMsg();
};

/* LoadDataForWages(), :2121-2300 */
window.lwLoadPending = function (i) {
    var p = S.pending[i];
    if (!p) return;
    clearMsg();

    var docTypeId = num(f(p, 'DocumentTypeId'));
    S.refDocTypeId = docTypeId;
    S.refDocId     = num(f(p, 'Id'));

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

    /* :2166 - 808 sends a numeric ReqType, everything else sends the text. */
    var reqType = S.reqType;
    if (docTypeId === 808) {
        if (S.reqType === 'Input')  reqType = '1';
        if (S.reqType === 'OutPut') reqType = '2';
    }

    var sel = el('cmbRefDocType');
    if (sel) {
        var found = false;
        for (var n = 0; n < sel.options.length; n++) {
            if (String(sel.options[n].value) === String(docTypeId)) {
                sel.selectedIndex = n; found = true; break;
            }
        }
        if (!found) {
            var o = document.createElement('option');
            o.value = docTypeId;
            o.textContent = txt(f(p, 'DocumentTypeDescription'));
            sel.appendChild(o);
            sel.value = String(docTypeId);
        }
        if (window.jQuery && jQuery(sel).data('select2')) jQuery(sel).trigger('change.select2');
    }

    busy(el('btnLoadAll'), true);

    /* :2179 - is this document already billed?  A hit turns the form into an Update. */
    getJson(API + '/by-ref-doc?refDocumentTypeId=' + docTypeId
                + '&refDocNoId=' + S.refDocId
                + '&reqType=' + encodeURIComponent(reqType))
    .then(function (d) {
        if (d && d.id) {
            S.id = d.id;
            return getJson(API + '/' + d.id).then(applyExisting);
        }
        S.id = 0;
        return loadDetailRows(docTypeId, S.refDocId, reqType);
    })
    .then(function () {
        setSaveMode();
        return loadGridLookups();
    })
    .then(function () {
        renderGrid('regular');
        renderGrid('other');
        lwTab('regular');
        busy(el('btnLoadAll'), false);
    })
    .catch(function (e) {
        busy(el('btnLoadAll'), false);
        msg('Could not load the document: ' + e.message, false);
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
        if (!detail.length) { msg('This document has no rows to bill.', false); return; }

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
            setVal('txtGrossWeight', r2(grossTotal));
            setVal('txtTotalQty',    r2(qtyTotal));
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

/* A document that already has a bill: header and rows come back from GetByID. */
function applyExisting(d) {
    var h = (d && d.header) || {};
    setVal('txtDocDate',      ymd(f(h, 'DocDate')));
    setVal('txtDocNo',        f(h, 'DocNo'));
    setVal('txtRefDocNo',     f(h, 'RefDocNo'));
    setVal('txtGrnId',        f(h, 'RefDocNoId'));
    setVal('txtGpNo',         f(h, 'ScaleSlipNo'));
    setVal('txtEntryType',    f(h, 'RefDocument'));
    setVal('txtOtherRemarks', f(h, 'OtherRemarks'));
    setVal('txtGrossWeight',  f(h, 'WeightTotal'));
    setVal('txtTotalQty',     f(h, 'QtyTotal'));
    var ap = el('chkIsApproved'); if (ap) ap.checked = !!f(h, 'IsAproved');
    S.refDocId = num(f(h, 'RefDocNoId'));
    S.reqType  = txt(f(h, 'RefDocument'));

    S.regular = ((d && d.details) || []).map(function (x) {
        var row = blankRow();
        row.DetailId        = num(f(x, 'Id'));
        row.SupplierId      = num(f(x, 'ContractorId'));
        row.WagesId         = num(f(x, 'InvConractorWagesAccountsId'));
        row.WagesScheduleId = num(f(x, 'InvContractorWagesScheduleId'));
        row.ItemId          = num(f(x, 'ItemId'));
        row.jobLotId        = num(f(x, 'JobLotId'));
        row.packingTypeId   = num(f(x, 'InvPackingTypeId'));
        row.WagesTypeId     = num(f(x, 'WagesTypeId'));
        row.MoveFromId      = num(f(x, 'WareHouseFromId'));
        row.MoveToId        = num(f(x, 'WareHouseToId'));
        row.RefLineId       = num(f(x, 'RefLineId'));
        row.Date            = ymd(f(x, 'RefDocDate'));
        row.RefDocQty       = num(f(x, 'RefDocQty'));
        row.RefDocWeight    = num(f(x, 'RefDocWeight'));
        row.PackSize        = num(f(x, 'PackSize'));
        row.Quantity        = num(f(x, 'Qty'));
        row.BillQty         = num(f(x, 'BillQty'));
        row.Weight          = num(f(x, 'Weight'));
        row.BillWeight      = num(f(x, 'BillWeight'));
        row.WeightCut       = num(f(x, 'WeightCut'));
        row.RateWithoutAddLess = num(f(x, 'WageRate'));
        row.RateAddLess     = num(f(x, 'RateAddLess'));
        row.Rate            = num(f(x, 'WageRate')) + num(f(x, 'RateAddLess'));
        row.Amount          = num(f(x, 'WagesAmount'));
        row.Crop            = txt(f(x, 'Crop'));
        row.WagesType       = f(x, 'FreeOfCost') ? 'Free Of Cost' : 'Regular';
        row.ContractorName  = txt(f(x, 'ContractorName'));
        row.WagesAccount    = txt(f(x, 'WagesAccountName'));
        row.Item            = txt(f(x, 'ItemName'));
        row.jobLot          = txt(f(x, 'JobLotDescription'));
        row.MoveFrom        = txt(f(x, 'WarehouseFrom'));
        row.MoveTo          = txt(f(x, 'WareHouseTo'));
        row.WarehouseType   = txt(f(x, 'WarehouseType'));
        return row;
    });
    S.other = [];
    renderPrevDetail(d && d.details);
}

function renderPrevDetail(details) {
    renderTable('grdPrevDetailHead', 'grdPrevDetailBody', details || []);
}

/* ---------------------------------------------------------------- lookups */
function loadGridLookups() {
    return getJson(API + '/grid-lookups?refDocumentTypeId=' + S.refDocTypeId)
        .then(function (d) {
            S.contractors = (d && d.contractors) || [];
            S.activities  = (d && d.wagesActivities) || [];
        })
        .catch(function (e) {
            S.contractors = []; S.activities = [];
            msg('Contractor / activity lists could not be loaded: ' + e.message, false);
        });
}

function loadDocumentTypes() {
    return getJson(API + '/document-types').then(function (rows) {
        rows = rows || [];
        ['cmbRefDocType', 'cmbHistDocType'].forEach(function (id) {
            var sel = el(id);
            if (!sel) return;
            sel.innerHTML = '<option value="">...Select Any Value...</option>'
                + rows.map(function (r) {
                    var v = f(r, 'Id');
                    if (v === null) v = f(r, 'DocumentTypeId');
                    var t = f(r, 'ReferenceDocType');
                    if (t === null) t = f(r, 'DocumentTypeDescription');
                    if (t === null) t = f(r, 'Description');
                    return '<option value="' + esc(v) + '">' + esc(t) + '</option>';
                }).join('');
        });
    }).catch(function (e) {
        msg('Reference document types could not be loaded: ' + e.message, false);
    });
}

window.lwRefDocTypeChanged = function () {
    S.refDocTypeId = num(val('cmbRefDocType'));
    S.refDocId = 0;
    S.id = 0;
    setSaveMode();
    generateDocNo();
    loadGridLookups().then(function () { renderGrid('regular'); renderGrid('other'); });
    loadPending();
};

window.lwLoadRefDoc = function () {
    /* Typing a reference document number finds it in the pending list rather than
       inventing a second lookup path the desktop does not have. */
    var no = num(val('txtRefDocNo'));
    if (!no) return;
    for (var i = 0; i < S.pending.length; i++) {
        if (num(f(S.pending[i], 'DocNo')) === no) { window.lwLoadPending(i); return; }
    }
    msg('Document ' + no + ' is not in the pending list for this reference document type.', false);
};

function generateDocNo() {
    if (!S.refDocTypeId) { setVal('txtDocNo', ''); return; }
    getJson(API + '/next-doc-no?refDocumentTypeId=' + S.refDocTypeId)
        .then(function (d) { setVal('txtDocNo', d && d.docNo ? d.docNo : ''); })
        .catch(function () { setVal('txtDocNo', ''); });
}

function setSaveMode() {
    var save = el('btnSave'), upd = el('btnUpdate'), lbl = el('lblEditing'), sel = el('cmbRefDocType');
    if (S.id > 0) {
        if (save) save.style.display = 'none';
        if (upd)  upd.style.display = '';
        if (lbl)  lbl.textContent = 'Editing an existing bill (Id ' + S.id + ') - Save is replaced by '
                                  + 'Update, and the reference document can no longer be changed.';
        if (sel)  sel.disabled = true;
    } else {
        if (save) save.style.display = '';
        if (upd)  upd.style.display = 'none';
        if (lbl)  lbl.textContent = '';
        if (sel)  sel.disabled = false;
    }
}

/* ---------------------------------------------------------------- save */
function buildPayload() {
    var rows = S.regular.concat(S.other);
    return {
        id: S.id,
        docNo: num(val('txtDocNo')),
        docDate: val('txtDocDate'),
        refDocumentTypeId: S.refDocTypeId,
        refDocNo: num(val('txtRefDocNo')),
        refDocNoId: num(val('txtGrnId')),
        refDocument: S.reqType,
        stockPartyId: 0,
        jobOrderId: 0,
        scaleSlipNo: num(val('txtGpNo')),
        projectsId: 0,
        otherRemarks: val('txtOtherRemarks'),
        details: rows.map(function (r) {
            return {
                id: num(r.DetailId),
                contractorId: num(r.SupplierId),
                wagesAccountId: num(r.WagesId),
                scheduleId: num(r.WagesScheduleId),
                itemId: num(r.ItemId),
                jobLotId: num(r.jobLotId),
                packingTypeId: num(r.packingTypeId),
                wagesTypeId: num(r.WagesTypeId),
                warehouseFromId: num(r.MoveFromId),
                warehouseToId: num(r.MoveToId),
                wbTransactionsIdDt: 0,
                jobOrderId: 0,
                refLineId: num(r.RefLineId),
                refDocDate: ymd(r.Date),
                refDocQty: num(r.RefDocQty),
                refDocWeight: num(r.RefDocWeight),
                packSize: num(r.PackSize),
                qty: num(r.Quantity),
                billQty: num(r.BillQty),
                weight: num(r.Weight),
                billWeight: num(r.BillWeight),
                weightCut: num(r.WeightCut),
                /* The procedure's WageRate is the schedule rate BEFORE add/less - the add/less
                   is its own column (:994-1010). Sending the combined rate would double-count it. */
                wageRate: num(r.RateWithoutAddLess),
                rateAddLess: num(r.RateAddLess),
                wagesAmount: num(r.Amount),
                freeOfCost: txt(r.WagesType) === 'Free Of Cost',
                isCompany: false,
                crop: txt(r.Crop),
                remarksDetail: '',
                wagesAccountName: txt(r.WagesAccount),
                itemName: txt(r.Item)
            };
        })
    };
}

/* FormValidation, :2572-2600 - the form's own messages, in its own order. */
function validate(p) {
    if (!p.docNo)             return 'document Number Field Required';
    if (!p.refDocumentTypeId) return 'ReferenceDocType Field Required';
    if (!p.refDocNo)          return 'Doc No Field Required';
    if (!p.refDocNoId)        return 'DocNoId Field Required';
    if (!p.details.length)    return 'First row contains No values Of Contractor And Wages Account.';
    for (var i = 0; i < p.details.length; i++) {
        if (!p.details[i].contractorId || !p.details[i].wagesAccountId) {
            return 'Row ' + (i + 1) + ' has no Contractor or Wages Account.';
        }
    }
    return null;
}

function doSave(btn, isUpdate) {
    if (S.busy) return;
    clearMsg();
    var p = buildPayload();
    var err = validate(p);
    if (err) { msg(err, false); return; }
    if (!confirm(isUpdate ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;

    busy(btn, true);
    postJson(API + '/save', p).then(function (res) {
        busy(btn, false);
        if (res && res.success) {
            S.id = num(res.id) || S.id;
            setSaveMode();
            msg(res.message || 'Saved SuccessFully', true);
        } else {
            msg((res && res.message) || 'Save failed.', false);
        }
    }).catch(function (e) {
        busy(btn, false);
        msg('Save failed: ' + e.message, false);
    });
}

window.lwSave   = function () { doSave(el('btnSave'), false); };
window.lwUpdate = function () { doSave(el('btnUpdate'), true); };

/* ---------------------------------------------------------------- new / refresh */
window.lwNew = function () {
    S.id = 0; S.refDocId = 0; S.reqType = '';
    S.regular = []; S.other = [];
    setVal('txtDocDate', today());
    setVal('txtRefDocNo', ''); setVal('txtGpNo', ''); setVal('txtEntryType', '');
    setVal('txtGrnId', ''); setVal('txtGrossWeight', 0); setVal('txtTotalQty', 0);
    setVal('txtOtherRemarks', '');
    var ap = el('chkIsApproved'); if (ap) ap.checked = false;
    setSaveMode();
    generateDocNo();
    renderGrid('regular'); renderGrid('other');
    renderPrevDetail([]);
    loadPending();
    clearMsg();
    lwTab('form');
};

window.lwRefresh = function () { window.lwNew(); };

window.lwNotImplemented = function (name) {
    msg(name + ' has not been ported yet. Its desktop form is a separate screen; '
        + 'nothing here was changed.', false);
};

window.lwToggleFullscreen = function () {
    var card = el('viewRegular') && el('viewRegular').style.display !== 'none' ? el('lwRegularCard')
             : el('viewOther')   && el('viewOther').style.display   !== 'none' ? el('lwOtherCard')
             : el('lwHistoryCard');
    if (card) card.classList.toggle('cw-fullscreen');
};

/* ---------------------------------------------------------------- history tab */
window.lwShowHistory = function () {
    var btn = el('btnShowHistory');
    if (S.busy) return;
    busy(btn, true);
    var q = '?fromDate='  + encodeURIComponent(val('histFromDate'))
          + '&toDate='    + encodeURIComponent(val('histToDate'))
          + '&fromDocNo=' + (num(val('histFromDocNo')) || 0)
          + '&toDocNo='   + (num(val('histToDocNo')) || 0);
    getJson(API + '/history' + q).then(function (rows) {
        busy(btn, false);
        S.historyRows = rows || [];
        renderTable('grdHistoryHead', 'grdHistoryBody', S.historyRows, function (r, i) {
            return 'onclick="lwHistoryRow(' + i + ')" style="cursor:pointer;"';
        });
        var c = el('histCount'); if (c) c.textContent = S.historyRows.length;
    }).catch(function (e) {
        busy(btn, false);
        msg('History failed: ' + e.message, false);
    });
};

window.lwResetHistory = function () {
    ['histFromDate', 'histToDate', 'histFromDocNo', 'histToDocNo'].forEach(function (id) { setVal(id, ''); });
    var s = el('cmbHistDocType'); if (s) s.value = '';
    S.historyRows = [];
    renderTable('grdHistoryHead', 'grdHistoryBody', []);
    renderTable('grdHistoryDetailHead', 'grdHistoryDetailBody', []);
    var c = el('histCount'); if (c) c.textContent = '0';
};

window.lwHistoryRow = function (i) {
    var r = S.historyRows[i];
    if (!r) return;
    var id = num(f(r, 'Id')) || num(f(r, 'HeaderId'));
    if (!id) { msg('That history row carries no header Id.', false); return; }
    getJson(API + '/' + id).then(function (d) {
        renderTable('grdHistoryDetailHead', 'grdHistoryDetailBody', (d && d.details) || []);
    }).catch(function (e) { msg('History detail failed: ' + e.message, false); });
};

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
        var m = el('lwFormHistoryModal'); if (m) m.style.display = 'block';
    }).catch(function (e) {
        busy(btn, false);
        msg('Form history failed: ' + e.message, false);
    });
};
window.lwCloseFormHistory = function () {
    var m = el('lwFormHistoryModal'); if (m) m.style.display = 'none';
};
window.lwOpenFromHistory = function (i) {
    var r = (S.formHistoryRows || [])[i];
    if (!r) return;
    var id = num(f(r, 'Id'));
    if (!id) { msg('That row carries no Id.', false); return; }
    getJson(API + '/' + id).then(function (d) {
        S.id = id;
        S.refDocTypeId = num(f((d && d.header) || {}, 'RefDocumentTypeId'));
        var sel = el('cmbRefDocType'); if (sel) sel.value = String(S.refDocTypeId);
        applyExisting(d);
        setSaveMode();
        return loadGridLookups();
    }).then(function () {
        renderGrid('regular'); renderGrid('other');
        window.lwCloseFormHistory();
        lwTab('form');
    }).catch(function (e) { msg('Could not open that bill: ' + e.message, false); });
};

/* Generic table renderer - the History grids show whatever columns the procedure returns,
   which is what the desktop does too (RetrieveStructure() off the DataTable). */
function renderTable(headId, bodyId, rows, rowAttrs) {
    var head = el(headId), body = el(bodyId);
    if (!head || !body) return;
    rows = rows || [];
    if (!rows.length) {
        head.innerHTML = '<th>No records</th>';
        body.innerHTML = '<tr><td style="text-align:center; padding:14px; color:#777;">No records.</td></tr>';
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

    /* Ctrl+S / Ctrl+U / Ctrl+N, as the form's KeyDown does. */
    document.addEventListener('keydown', function (e) {
        if (!e.ctrlKey) return;
        var k = String(e.key || '').toLowerCase();
        var save = el('btnSave'), upd = el('btnUpdate');
        if (k === 's') { e.preventDefault(); if (save && save.style.display !== 'none') window.lwSave(); }
        if (k === 'u') { e.preventDefault(); if (upd  && upd.style.display  !== 'none') window.lwUpdate(); }
        if (k === 'n') { e.preventDefault(); window.lwNew(); }
    });

    getJson(API + '/config-flags')
        .then(function (c) { S.cfg = c || {}; })
        .catch(function () { S.cfg = {}; })
        .then(loadDocumentTypes)
        .then(function () {
            setSaveMode();
            loadPending();
            renderGrid('regular');
            renderGrid('other');
            if (window.jQuery && jQuery.fn.select2) {
                jQuery('.lw-select2').filter(function () { return !jQuery(this).data('select2'); })
                    .select2({ dropdownAutoWidth: true, width: '100%' });
            }
        });
}

if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', boot);
else boot();

})();
