/* ============================================================================================
 * Booking Order - screen 140, Architecture.WinApp.Sale.BookingOrder, DocumentTypeId 127
 * Desktop map: Load :437  DocumentNoFill :570  BranchSrNoFill :591  OrderCatagoryfill :614  GenerateOrderCategoryNo :633  PaymentTerms :663
 *   DeliveryTerms :697  OrderType :715  SupplierNameFill :732  CommissionTypeFill :785  CommissionUOMFill :806  ItemDetailFill :828
 *   bindRateUomAndItemPackUom :877  CropYear :999  GetCustomerDiscount :1083  combsalesman_Leave :1126  btnplus_Click :1329  grdSettings :1366
 *   FormValidationDetail :1260  grd_DoubleClick :1579  btnUpdateDetail_Click :1629  Insert :1676  ReadById :1979  Reset :2073  ResetDetail :2131
 *   txtduedays_TextChanged :2171  GetRegularItemDiscount :2188  GetPackingAddLess :2234  txtPackingAddless_TextChanged :2299
 *   combpttrm_Leave :2324  CalculateWeight :2370  TotalAmount :2409  ItemDiscountAmount :2458  TotalCommissionAmount :2497
 *   btnResetHistory_Click :2614  gridhistoryfill :2750  grdhistory_ColumnButtonClick :2948  GridDetailBind :2992  KeyDown :3285  CmbCropyr_Leave :3635
 * The server re-validates everything Insert checks; nothing here is trusted.
 * ============================================================================================ */
(function () {
    'use strict';
    var api = '/sale/api/booking-order';
    var L = {}, perms = {}, cfg = {}, rounding = 0;
    var rows = [], removed = [], hist = [], uoms = [], crops = [];
    var RecId = 0, updateIndex = -1, detailId = 0, tab = 0, histLoaded = false, preOrderExist = false;
    var HB = { branches: [], checked: {} };

    function $id(x) { return document.getElementById(x); }
    function val(x) { var e = $id(x); return e ? e.value : ''; }
    function setVal(x, v) { var e = $id(x); if (e) e.value = (v === null || v === undefined) ? '' : v; }
    function box(m) { window.alert(m); }
    function esc(s) { return String(s === null || s === undefined ? '' : s).replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;'); }
    function num(v) { var n = parseFloat(String(v === null || v === undefined ? '' : v).replace(/,/g, '')); return isNaN(n) ? 0 : n; }
    function int(v) { var n = parseInt(String(v === null || v === undefined ? '' : v).replace(/,/g, ''), 10); return isNaN(n) ? 0 : n; }
    function col(row, name) {
        if (!row) return '';
        if (Object.prototype.hasOwnProperty.call(row, name)) return row[name];
        for (var k in row) if (Object.prototype.hasOwnProperty.call(row, k) && k.toLowerCase() === name.toLowerCase()) return row[k];
        return '';
    }
    function first(row, names) { for (var i = 0; i < names.length; i++) { var v = col(row, names[i]); if (v !== '' && v !== null && v !== undefined) return v; } return ''; }
    /* Math.Round(x, n, MidpointRounding.AwayFromZero) */
    function away(x, n) { x = num(x); var f = Math.pow(10, n || 0), y = Math.abs(x) * f; return (x < 0 ? -1 : 1) * Math.floor(y + 0.5 + 1e-9) / f; }
    /* Math.Round(x, n) - banker's rounding */
    function bank(x, n) {
        var f = Math.pow(10, n || 0), y = x * f, fl = Math.floor(y), d = y - fl;
        if (Math.abs(d - 0.5) < 1e-9) return (fl % 2 === 0 ? fl : fl + 1) / f;
        return Math.round(y) / f;
    }
    function clean(n) { return parseFloat(Number(n).toPrecision(15)); }
    function str(n) { return String(clean(n)); }                                    /* double.ToString() */
    function group(n) { return String(n).replace(/\B(?=(\d{3})+(?!\d))/g, ','); }
    function hash0(n) {                                                             /* ToString("#,#") : zero gives "" */
        var r = away(n, 0); if (r === 0 || !isFinite(r)) return '';
        var s = group(Math.abs(r)); return (r < 0 ? '-' : '') + s;
    }
    function hash2(n) {                                                             /* ToString("#,#.##") */
        var r = away(n, 2); if (r === 0 || !isFinite(r)) return '';
        var a = Math.abs(r), ip = Math.floor(a), fp = String(clean(a - ip)).replace(/^0/, '');
        return (r < 0 ? '-' : '') + (ip ? group(ip) : '') + (fp === '.0' || fp === '0' ? '' : fp);
    }
    function zero3(n) { var r = away(n, 3), a = Math.abs(r), ip = Math.floor(a), fp = String(clean(a - ip)).replace(/^0/, ''); return (r < 0 ? '-' : '') + group(ip) + (fp && fp !== '0' ? fp : ''); }  /* "#,##0.###" */
    function iso(d) { return d.getFullYear() + '-' + String(d.getMonth() + 1).padStart(2, '0') + '-' + String(d.getDate()).padStart(2, '0'); }
    function today() { return iso(new Date()); }
    function dateOnly(v) { if (!v) return ''; var m = String(v).match(/^(\d{4})-(\d{2})-(\d{2})/); if (m) return m[0]; var d = new Date(v); return isNaN(d.getTime()) ? '' : iso(d); }
    function stamp(v) {                                                             /* "dd-MM-yyyy hh:mm tt" */
        if (!v) return '';
        var d = new Date(String(v).replace(' ', 'T')); if (isNaN(d.getTime())) return String(v);
        var h = d.getHours(), ap = h >= 12 ? 'PM' : 'AM'; h = h % 12; if (h === 0) h = 12;
        return String(d.getDate()).padStart(2, '0') + '-' + String(d.getMonth() + 1).padStart(2, '0') + '-' + d.getFullYear() + ' ' + String(h).padStart(2, '0') + ':' + String(d.getMinutes()).padStart(2, '0') + ' ' + ap;
    }
    function http(method, url, body) {
        var opt = { method: method, headers: { 'Accept': 'application/json' }, credentials: 'same-origin' };
        if (body !== undefined) { opt.headers['Content-Type'] = 'application/json'; opt.body = JSON.stringify(body); }
        return fetch(url, opt).then(function (r) {
            return r.text().then(function (t) {
                var b = null; try { b = t ? JSON.parse(t) : null; } catch (e) { /* not json */ }
                if (!r.ok) throw new Error((b && b.message) || ('Request failed (' + r.status + ')'));
                return b;
            });
        });
    }
    function fill(x, list, v, t) {
        var sel = $id(x); if (!sel) return;
        var html = '<option value=""></option>';
        (list || []).forEach(function (r) { html += '<option value="' + esc(typeof v === 'function' ? v(r) : col(r, v)) + '">' + esc(typeof t === 'function' ? t(r) : col(r, t)) + '</option>'; });
        sel.innerHTML = html;
    }
    function text(x) { var s = $id(x); return s && s.selectedIndex >= 0 && s.options[s.selectedIndex] ? s.options[s.selectedIndex].text : ''; }
    function refreshCombos() { if (window.DesktopCombo && window.DesktopCombo.refresh) window.DesktopCombo.refresh(); }
    function show(x, on) { var e = $id(x); if (e) e.style.display = on ? '' : 'none'; }
    function enable(x, on) { var e = $id(x); if (e) e.disabled = !on; }
    function focus(x) { var e = $id(x); if (e) try { e.focus(); } catch (z) { /* ignore */ } }
    function on(x, ev, fn) { var e = $id(x); if (e) e.addEventListener(ev, fn); }
    function custName(r) { return first(r, ['CompanyName', 'CustomerName', 'Name']); }
    function byId(list, id, key) { for (var i = 0; i < (list || []).length; i++) if (int(col(list[i], key || 'Id')) === int(id)) return list[i]; return null; }
    function selectIndex(x, n) { var s = $id(x); if (s && s.options.length > n) s.selectedIndex = n; }       /* Rows[n].Activate() on a ZeroIndex list */
    function setByText(x, t) {
        var s = $id(x), f = ''; if (!s) return;
        for (var k = 0; k < s.options.length; k++) if (s.options[k].text === String(t)) f = s.options[k].value;
        s.value = f;
    }
    function saveMode() { return $id('btnsave').style.display !== 'none'; }
    function docDate() { return val('DocDate') || today(); }

    // ------------------------------------------------------------------ lists
    function itemLabel(r) { return $id('rdSearchByCode').checked ? col(r, 'ItemCode') : col(r, 'ItemName'); }
    function fillItems() {                                                            /* ItemDetailFill :828 */
        var keep = val('CmbItem'), seen = {}, list = [];
        (L.items || []).forEach(function (r) { var k = int(col(r, 'ItemId')); if (k && !seen[k]) { seen[k] = 1; list.push(r); } });
        fill('CmbItem', list, 'ItemId', itemLabel);
        if (keep && seen[int(keep)]) setVal('CmbItem', keep);
        refreshCombos();
    }
    function bindLists() {
        fill('combordercat', L.orderCategories, 'Id', 'OrderCategoryName');
        selectIndex('combordercat', 3);                                               /* Rows[3].Activate() */
        fill('CmbCustomer', L.customers, 'Id', custName);
        fill('combsalesman', L.customers, 'Id', custName);
        fill('CmbOrderType', [{ t: 'Fixed' }, { t: 'Open' }], 't', 't');
        fill('combpttrm', L.dueTerms, 'Id', 'TermsDescription');
        selectIndex('combpttrm', 2);                                                  /* Rows[2].Activate() */
        fill('combdeliverytrm', [{ t: 'Load' }, { t: 'Ponch' }], 't', 't'); selectIndex('combdeliverytrm', 1);
        fill('combcommtype', [{ t: 'Flat' }, { t: 'Percent' }, { t: 'Comm Weight' }], 't', 't'); selectIndex('combcommtype', 1);
        fill('combruom', [{ t: '40' }, { t: '50' }, { t: '60' }, { t: '100' }], 't', 't'); selectIndex('combruom', 1);
        fillItems();
        fill('cmbPackType', L.packingTypes, 'Id', 'PackTypeDesc');
        fill('combcityarea', L.cities, 'Id', function (r) { return first(r, ['CityName', 'City', 'Name']); });
        fill('cmbReferenceParty', L.referenceParties, 'Id', 'ReferencePartyName');
        fill('CmbCustomerHistory', L.historyCustomers, 'Id', 'Customer');
        refreshCombos();
    }

    // ------------------------------------------------------------------ numbers (DocumentNoFill / BranchSrNoFill / GenerateOrderCategoryNo)
    function numbers() {
        return http('GET', api + '/numbers?categoryId=' + int(val('combordercat'))).then(function (r) {
            if (int(r.docNo) > 0) setVal('txtdocno', r.docNo);
            if (int(r.branchSr) > 0) setVal('txtBranchSrNo', r.branchSr);
            if (int(r.catSr) > 0) setVal('txtcatsr', r.catSr);
        }).catch(function (e) { box(e.message); });
    }
    function categoryNo() {
        if (!int(val('combordercat'))) return;
        http('GET', api + '/numbers?categoryId=' + int(val('combordercat'))).then(function (r) { if (int(r.catSr) > 0) setVal('txtcatsr', r.catSr); }).catch(function (e) { box(e.message); });
    }

    // ------------------------------------------------------------------ calculations
    function packText() { return text('combitempck').trim(); }
    function calculateWeight() {                                                      /* CalculateWeight :2370 */
        if (packText() !== '' && val('txtqty') !== '') setVal('txtweight', str(num(packText()) * num(val('txtqty'))));
    }
    function totalAmount() {                                                          /* TotalAmount :2409 */
        var rateUom, packUom, itemRate, netRate, discAmt = 0, half = bank(rounding / 2, 2);
        if (val('txtqty') !== '' && val('txtweight') !== '' && val('txtItemPrice') !== '' && val('txtRateUom') !== '') {
            var weight = num(val('txtweight'));
            rateUom = num(val('txtRateUom')); packUom = num(packText()); itemRate = num(val('txtItemPrice'));
            var disc = num(val('txtPartyDiscPrct'));
            if (disc > 0 && itemRate > 0) discAmt = itemRate * disc / 100;
            netRate = itemRate - discAmt;
            if (rounding > 0) {
                var n2 = netRate % 100, cmp = bank(n2 / rounding, 0) * rounding, diff = n2 - cmp;
                netRate = !(diff > 0) ? Math.ceil(netRate / rounding) * rounding
                    : (!(diff >= half) ? Math.floor(netRate / rounding) * rounding : Math.ceil(netRate / rounding) * rounding);
            }
            var addless = num(val('txtPackingAddless'));
            netRate = (netRate / rateUom * packUom + addless) / packUom * rateUom;
            setVal('txtNetRate', hash0(netRate));
            setVal('txtPartDiscAmt', hash2(discAmt));
            setVal('txtamount', hash0(weight / rateUom * netRate));
        } else {
            setVal('txtamount', '0');
        }
        itemDiscountAmount();
    }
    function itemDiscountAmount() {                                                   /* ItemDiscountAmount :2458 */
        var amount = num(val('txtamount')), pct = num(val('txtItemDiscPrct')), d = 0;
        if (pct > 0 && amount > 0) { d = amount * pct / 100; setVal('txtItemDiscountAmt', hash0(d)); }
        if (d > 0) setVal('txtNetAmount', hash0(amount - d)); else setVal('txtNetAmount', val('txtamount'));
    }
    function gridTotal(key) { var s = 0; rows.forEach(function (r) { s += num(r[key]); }); return s; }
    function totalCommissionAmount() {                                                /* TotalCommissionAmount :2497 */
        if (val('txtCommrate') === '') return;
        var t = text('combcommtype'), rate = num(val('txtCommrate').trim());
        if (t === 'Flat') setVal('txtcommamount', str(rate));
        if (t === 'Percent' && rows.length) setVal('txtcommamount', zero3(away(gridTotal('amount') * rate / 100, 0)));
        if (t === 'Comm Weight') {
            var uom = num(text('combruom').trim()), w = gridTotal('weight');
            if (w > 0 && rate > 0 && uom > 0) setVal('txtcommamount', zero3(away(w / uom * rate, 0))); else setVal('txtcommamount', zero3(0));
        }
    }
    function packingAddlessChanged() {                                                /* txtPackingAddless_TextChanged :2299 */
        var v = num(val('txtPackingAddless'));
        if (v !== 0) {
            var pk = int(packText());
            if (int(val('cmbPackType')) === 2 && (pk === 50 || pk === 25)) setVal('txtPackingAddless', '-' + Math.abs(v)); else setVal('txtPackingAddless', str(v));
        }
        totalAmount(); totalCommissionAmount();
    }
    function selectedUom() { return byId(uoms, val('combitempck')); }
    function getPackingAddLess() {                                                    /* GetPackingAddLess :2234 */
        var u = selectedUom(), pa = 0;
        if (!u) { setVal('txtPackingAddless', ''); totalAmount(); return Promise.resolve(); }
        return http('GET', api + '/packing-add-less?uomId=' + int(col(u, 'ScheduleUnitId')) + '&customerId=' + int(val('CmbCustomer')) + '&date=' + encodeURIComponent(docDate())).then(function (r) {
            pa = num(r && r.packingAddLess);
            if (pa !== 0) { setVal('txtPackingAddless', str(pa)); packingAddlessChanged(); }
            else { setVal('txtPackingAddless', ''); totalAmount(); }
        }).catch(function (e) { box(e.message); });
    }
    function getCustomerDiscount() {                                                  /* GetCustomerDiscount :1083 */
        if (!int(val('CmbItem')) || !int(val('CmbCustomer'))) { totalAmount(); return Promise.resolve(); }
        return http('GET', api + '/customer-discount?customerId=' + int(val('CmbCustomer')) + '&itemId=' + int(val('CmbItem')) + '&date=' + encodeURIComponent(docDate())).then(function (r) {
            var d = num(r && r.discount); setVal('txtPartyDiscPrct', d > 0 ? str(d) : '0'); totalAmount();
        }).catch(function (e) { box(e.message); });
    }
    function packLeave() { getPackingAddLess().then(function () { calculateWeight(); totalAmount(); totalCommissionAmount(); }); }
    function qtyChanged() { calculateWeight(); totalAmount(); totalCommissionAmount(); }

    // ------------------------------------------------------------------ item / crop year / rate
    function loadUoms() {                                                             /* bindRateUomAndItemPackUom :877 */
        var keep = num(packText()), item = int(val('CmbItem'));
        if (!item) { uoms = []; fill('combitempck', [], 'Id', 'Equivalent'); refreshCombos(); return Promise.resolve(); }
        return http('GET', api + '/uoms?itemId=' + item).then(function (r) {
            uoms = r || [];
            fill('combitempck', uoms, 'Id', function (x) { return str(num(col(x, 'Equivalent'))); });
            if (keep > 0) {
                var f = ''; uoms.forEach(function (x) { if (num(col(x, 'Equivalent')) === keep) f = col(x, 'Id'); });
                setVal('combitempck', f);
            }
            refreshCombos();
        }).catch(function (e) { box(e.message); });
    }
    function loadCrops() {                                                            /* CropYear :999 */
        var keep = text('CmbCropyr'), item = int(val('CmbItem'));
        if (!item) { crops = []; fill('CmbCropyr', [], 'Id', 'CropYear'); refreshCombos(); return Promise.resolve(); }
        return http('GET', api + '/crop-years?itemId=' + item + '&date=' + encodeURIComponent(docDate())).then(function (r) {
            crops = r || [];
            fill('CmbCropyr', crops, 'Id', function (x) { return first(x, ['CropYear', 'CropYearName', 'Name']); });
            if (crops.length) {
                var f = ''; crops.forEach(function (x) { if (String(first(x, ['CropYear', 'CropYearName', 'Name'])) === keep) f = col(x, 'Id'); });
                if (f) setVal('CmbCropyr', f); else selectIndex('CmbCropyr', 1);        /* Rows[0].Activate() (ZeroIndex false) */
            }
            refreshCombos();
        }).catch(function (e) { box(e.message); });
    }
    function cropLeave() {                                                            /* CmbCropyr_Leave :3635 */
        var item = int(val('CmbItem')); if (!item) return Promise.resolve();
        return http('GET', api + '/pricing?itemId=' + item + '&cropYearId=' + int(val('CmbCropyr')) + '&customerId=' + int(val('CmbCustomer')) + '&date=' + encodeURIComponent(docDate())).then(function (r) {
            if (r.found) {
                if (int(val('CmbCustomer'))) { var d = num(r.customerDiscount); setVal('txtPartyDiscPrct', d > 0 ? str(d) : '0'); }
                setVal('txtPriceSchId', r.scheduleId); setVal('txtRateUomId', r.rateUomId);
                setVal('txtItemPrice', hash0(num(r.itemPrice))); setVal('txtRateUom', r.rateUom === null || r.rateUom === undefined ? '' : String(r.rateUom));
            } else {
                setVal('txtPriceSchId', '0'); setVal('txtRateUomId', '0'); setVal('txtItemPrice', '0'); setVal('txtRateUom', '0'); setVal('txtNetRate', '0'); setVal('txtPartyDiscPrct', '0');
            }
            var idisc = num(r.itemDiscount); setVal('txtItemDiscPrct', idisc ? str(idisc) : '');   /* GetRegularItemDiscount :2188 */
            totalAmount();
            $id('chkCommissionOnSale').checked = !!r.commOnSale;
        }).catch(function (e) { box(e.message); });
    }
    function itemChanged() { loadUoms().then(loadCrops).then(cropLeave); }

    // ------------------------------------------------------------------ payment term / due days
    function payTermLeave() {                                                         /* combpttrm_Leave :2324 */
        if (int(val('combpttrm')) === 1) { setVal('txtduedays', '0'); setVal('duedate', today()); enable('txtduedays', false); enable('duedate', false); }
        else { enable('txtduedays', true); enable('duedate', true); }
    }
    function dueDaysChanged() {                                                       /* txtduedays_TextChanged :2171 */
        var d = new Date();
        if (val('txtduedays').trim() !== '') d.setDate(d.getDate() + Math.floor(num(val('txtduedays'))));
        setVal('duedate', iso(d));
    }

    // ------------------------------------------------------------------ detail box
    function detailValid() {                                                          /* FormValidationDetail :1260 */
        if (!int(val('CmbItem'))) { box('ItemName Field is Required'); focus('CmbItem'); return false; }
        if (!int(val('CmbCustomer'))) { box('Customer Name Field is Required'); focus('CmbCustomer'); return false; }
        if (!int(val('combitempck'))) { box('PackUom Field is Required'); focus('combitempck'); return false; }
        if (!int(val('CmbCropyr'))) { box('Crop Year Field is Required'); focus('CmbCropyr'); return false; }
        if (!int(val('cmbPackType'))) { box('Pack Type Field is Required'); focus('cmbPackType'); return false; }
        if (val('txtqty').trim() === '' || val('txtqty').trim() === '0') { box('Qty Field is Required'); focus('txtqty'); return false; }
        if (val('txtweight').trim() === '' || val('txtweight').trim() === '0') { box('Weight Field is Required'); focus('txtweight'); return false; }
        if (val('txtItemPrice').trim() === '' || val('txtItemPrice').trim() === '0') { box('ItemPrice Field is Required'); focus('txtItemPrice'); return false; }
        if (val('txtNetRate').trim() === '' || val('txtNetRate').trim() === '0') { box('NetRate Field is Required'); focus('txtNetRate'); return false; }
        if (val('txtamount').trim() === '' || val('txtamount').trim() === '0') { box('Amount Field is Required'); focus('txtamount'); return false; }
        return true;
    }
    function detailToRow(r) {
        var it = byId(L.items, val('CmbItem'), 'ItemId') || {}, u = selectedUom() || {};
        r.scheduleId = int(val('txtPriceSchId')); r.itemId = int(val('CmbItem')); r.itemCode = col(it, 'ItemCode'); r.item = col(it, 'ItemName');
        r.cropYearId = val('CmbCropyr'); r.cropYear = text('CmbCropyr').trim();
        r.packingTypeId = int(val('cmbPackType')); r.packUomId = int(val('combitempck')); r.packScheduleUnitId = int(col(u, 'ScheduleUnitId')); r.packUom = packText();
        r.qty = num(val('txtqty').trim()); r.weight = num(val('txtweight').trim()); r.itemPrice = num(val('txtItemPrice').trim());
        r.packingAddLess = num(val('txtPackingAddless').trim()); r.rateDiscount = num(val('txtPartyDiscPrct').trim()); r.rateDiscountAmt = num(val('txtPartDiscAmt').trim());
        r.rate = num(val('txtNetRate').trim()); r.rateUomId = int(val('txtRateUomId')); r.rateUom = val('txtRateUom').trim();
        r.amount = num(val('txtamount')); r.itemDiscPrct = num(val('txtItemDiscPrct')); r.itemDiscAmt = num(val('txtItemDiscountAmt').trim()); r.totalAmount = num(val('txtNetAmount'));
        r.cityId = int(val('combcityarea')); r.refPartyId = int(val('cmbReferenceParty')); r.remarks = val('txtremarksdetail').trim();
        r.commOnSale = $id('chkCommissionOnSale').checked;
        return r;
    }
    function gridLock() {
        var has = rows.length > 0;
        enable('CmbCustomer', !has); enable('DocDate', !has); refreshCombos();
    }
    function btnplus_Click() {                                                        /* btnplus_Click :1329 */
        if (rows.length > 0 && int(rows[0].preOrderId) > 0) { box('You can not add manual Record because record against Pre order exist in Grid'); return; }
        if (!detailValid()) return;
        var r = detailToRow({ id: 0, preOrderId: 0, preOrderDetailId: 0, preOrder: 0, costCenterId: 0 });
        r.cityName = text('combcityarea').trim();
        rows.push(r);
        renderGrid();
        fill('CmbCropyr', [], 'Id', 'CropYear'); crops = [];
        focus('CmbItem'); resetDetail(); totalCommissionAmount();
        enable('CmbCustomer', false); enable('DocDate', false); refreshCombos();
    }
    function btnUpdateDetail_Click() {                                                /* btnUpdateDetail_Click :1629 */
        if (!detailValid()) return;
        var r = rows[updateIndex]; if (!r) return;
        r.id = detailId; detailToRow(r);
        renderGrid();
        show('btnplus', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
        resetDetail(); focus('CmbItem');
    }
    function resetDetail() {                                                          /* ResetDetail :2131 */
        updateIndex = -1;
        $id('chkCommissionOnSale').checked = false;
        setVal('CmbItem', ''); setVal('combitempck', '');
        ['txtqty', 'txtweight', 'txtItemPrice', 'txtNetRate', 'txtRateUom', 'txtamount', 'txtPackingAddless', 'txtPartyDiscPrct', 'txtPartDiscAmt',
            'txtItemDiscountAmt', 'txtItemDiscPrct', 'txtNetAmount', 'txtremarksdetail'].forEach(function (x) { setVal(x, ''); });
        refreshCombos();
    }
    function gridDoubleClick(i) {                                                     /* grd_DoubleClick :1579 */
        var r = rows[i]; if (!r) return;
        updateIndex = i; detailId = int(r.id);
        setVal('txtPriceSchId', r.scheduleId); setVal('CmbItem', r.itemId);
        loadUoms().then(loadCrops).then(function () {
            setVal('cmbPackType', r.packingTypeId); setVal('combitempck', r.packUomId);
            setByText('CmbCropyr', r.cropYear);
            setVal('txtqty', r.qty); setVal('txtweight', r.weight); setVal('txtItemPrice', r.itemPrice); setVal('txtPackingAddless', r.packingAddLess);
            setVal('txtPartyDiscPrct', r.rateDiscount); setVal('txtPartDiscAmt', r.rateDiscountAmt); setVal('txtNetRate', r.rate);
            setVal('txtRateUomId', r.rateUomId); setVal('txtRateUom', r.rateUom); setVal('txtamount', r.amount); setVal('txtItemDiscPrct', r.itemDiscPrct);
            setVal('txtItemDiscountAmt', r.itemDiscAmt); setVal('txtNetAmount', r.totalAmount); setVal('combcityarea', r.cityId);
            if (int(r.refPartyId) > 0) setVal('cmbReferenceParty', r.refPartyId);
            setVal('txtremarksdetail', r.remarks); $id('chkCommissionOnSale').checked = !!r.commOnSale;
            refreshCombos();
        });
        show('btnplus', false); show('btnUpdateDetail', true); show('btnCancelUpdateDetial', true);
        enable('CmbItem', !preOrderExist); refreshCombos(); focus('CmbItem');
    }
    function deleteRow(i) {
        if (updateIndex !== -1) { box('Reset the Detail Record First'); return; }
        var r = rows[i];
        if (r && int(r.id) > 0) { if (!window.confirm('Are you sure to Delete?')) return; removed.push(r); }
        rows.splice(i, 1); renderGrid(); totalCommissionAmount();
        if (!rows.length) { enable('CmbCustomer', true); enable('DocDate', true); refreshCombos(); }
    }

    // ------------------------------------------------------------------ grid (grdSettings :1366)
    function selectHtml(list, valueKey, textFn, current, key, i) {
        var h = '<select class="ct" data-k="' + key + '" data-i="' + i + '"><option value=""></option>';
        (list || []).forEach(function (x) { var v = col(x, valueKey); h += '<option value="' + esc(v) + '"' + (int(v) === int(current) ? ' selected' : '') + '>' + esc(textFn(x)) + '</option>'; });
        return h + '</select>';
    }
    function renderGrid() {
        var h = '<th style="width:20px">X</th>';
        if (preOrderExist) h += '<th>PreOrder</th>';
        ['ItemCode', 'Item Name', 'CropYear', 'PackingTypeID', 'PackUom', 'QTY', 'Weight', 'ItemPrice', 'Packing Exp/Disc', 'RateDiscount', 'RateDiscountAmt', 'Net Rate',
            'RateUom', 'Amount', 'ItemDiscPrct', 'ItemDiscAmt', 'TotalAmount', 'CityID', 'RefPartyId', 'Remarks'].forEach(function (c) { h += '<th>' + esc(c) + '</th>'; });
        $id('grdHead').innerHTML = h;
        var b = '';
        rows.forEach(function (r, i) {
            b += '<tr data-i="' + i + '"><td class="ctr"><a class="glink" data-del="' + i + '">X</a></td>';
            if (preOrderExist) b += '<td>' + esc(r.preOrder) + '</td>';
            b += '<td>' + esc(r.itemCode) + '</td><td>' + esc(r.item) + '</td><td>' + esc(r.cropYear) + '</td>';
            b += '<td>' + selectHtml(L.packingTypes, 'Id', function (x) { return col(x, 'PackTypeDesc'); }, r.packingTypeId, 'packingTypeId', i) + '</td>';
            b += '<td>' + esc(r.packUom) + '</td>';
            b += '<td class="n">' + hash0(r.qty) + '</td><td class="n">' + hash0(r.weight) + '</td><td class="n">' + hash0(r.itemPrice) + '</td><td class="n">' + esc(r.packingAddLess) + '</td>';
            b += '<td class="n">' + esc(r.rateDiscount) + '</td><td class="n">' + hash0(r.rateDiscountAmt) + '</td><td class="n">' + esc(r.rate) + '</td><td>' + esc(r.rateUom) + '</td>';
            b += '<td class="n">' + hash0(r.amount) + '</td><td class="n">' + esc(r.itemDiscPrct) + '</td><td class="n">' + hash0(r.itemDiscAmt) + '</td><td class="n">' + hash0(r.totalAmount) + '</td>';
            b += '<td>' + selectHtml(L.cities, 'Id', function (x) { return first(x, ['CityName', 'City', 'Name']); }, r.cityId, 'cityId', i) + '</td>';
            b += '<td>' + selectHtml(L.referenceParties, 'Id', function (x) { return col(x, 'ReferencePartyName'); }, r.refPartyId, 'refPartyId', i) + '</td>';
            b += '<td>' + esc(r.remarks) + '</td></tr>';
        });
        $id('grdBody').innerHTML = b;
        var f = '<td></td>' + (preOrderExist ? '<td></td>' : '') + '<td></td><td></td><td></td><td></td><td></td>';
        f += '<td class="n">' + hash0(gridTotal('qty')) + '</td><td class="n">' + hash0(gridTotal('weight')) + '</td><td></td><td></td><td></td><td class="n">' + hash0(gridTotal('rateDiscountAmt')) + '</td><td></td><td></td>';
        f += '<td class="n">' + hash0(gridTotal('amount')) + '</td><td></td><td class="n">' + hash0(gridTotal('itemDiscAmt')) + '</td><td class="n">' + hash0(gridTotal('totalAmount')) + '</td><td></td><td></td><td></td>';
        $id('grdFoot').innerHTML = rows.length ? f : '';
        Array.prototype.forEach.call($id('grdBody').querySelectorAll('tr'), function (tr) {
            tr.addEventListener('dblclick', function () { gridDoubleClick(int(tr.getAttribute('data-i'))); });
        });
        Array.prototype.forEach.call($id('grdBody').querySelectorAll('a[data-del]'), function (a) {
            a.addEventListener('click', function (ev) { ev.stopPropagation(); deleteRow(int(a.getAttribute('data-del'))); });
        });
        Array.prototype.forEach.call($id('grdBody').querySelectorAll('select[data-k]'), function (s) {
            s.addEventListener('change', function () { rows[int(s.getAttribute('data-i'))][s.getAttribute('data-k')] = int(s.value); });
            s.addEventListener('dblclick', function (ev) { ev.stopPropagation(); });
        });
    }

    // ------------------------------------------------------------------ save (Insert :1676)
    function payload() {
        var details = rows.map(function (r) {
            var o = {}; for (var k in r) if (Object.prototype.hasOwnProperty.call(r, k)) o[k] = r[k];
            o.cityName = (byId(L.cities, r.cityId) ? first(byId(L.cities, r.cityId), ['CityName', 'City', 'Name']) : ''); return o;
        });
        return {
            docNo: val('txtdocno'), branchSr: val('txtBranchSrNo'), docDate: docDate(), orderCategoryId: int(val('combordercat')), catSr: val('txtcatsr'),
            customerId: int(val('CmbCustomer')), supplierRefNo: val('txtsupprefno'), remarks: val('txtremarks'), orderType: text('CmbOrderType'),
            paymentTermId: int(val('combpttrm')), dueDays: int(val('txtduedays')), dueDate: val('duedate'), deliveryTerm: text('combdeliverytrm'),
            deliveryStartDate: val('deliverystartdate'), deliveryDays: int(val('txtdeliverydays')), salesmanId: int(val('combsalesman')),
            commType: text('combcommtype'), commRate: num(val('txtCommrate')), commUom: int(text('combruom')), commAmount: num(val('txtcommamount').trim()),
            commRemarks: val('txtcommremarks'), details: details,
            removed: removed
        };
    }
    function insert() {
        if (rows.length === 0) { box('Grid Record Not Found'); return; }
        var body = payload(); body.id = RecId;
        enable('btnsave', false); enable('btnupdate', false);
        var done = function () { enable('btnsave', true); enable('btnupdate', true); };
        http('POST', api + '/checks', body).then(function (c) {
            if (!window.confirm(RecId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) { done(); return; }
            var ok = true;
            (c.confirms || []).forEach(function (m) { if (ok && !window.confirm(m)) ok = false; });
            if (!ok) { done(); return; }
            return http(RecId > 0 ? 'PUT' : 'POST', RecId > 0 ? api + '/' + RecId : api, body).then(function (r) {
                box(r.message);
                var id = int(r.id), print = $id('ChkBox').checked;
                reset();
                if (print && id) printSlip(id);
                done();
            });
        }).catch(function (e) { box(e.message); done(); });
    }
    function printSlip(id) {                                                          /* CommonServices.BookOrderReports273A */
        if (!id) { box('Record not found...'); return; }
        window.open('/api/print/so-273-a/pdf?id=' + id, '_blank');
    }

    // ------------------------------------------------------------------ read (ReadById :1979)
    function mapDetail(d) {
        var price = num(col(d, 'ItemPrice')), rd = num(col(d, 'RateDiscount'));
        return {
            id: int(col(d, 'Id')), preOrderId: int(col(d, 'RefDocId')), preOrderDetailId: int(col(d, 'RefDocDetailId')), preOrder: col(d, 'RefDocNo'),
            scheduleId: int(col(d, 'PriceScheduleId')), itemId: int(col(d, 'OrderItemId')), itemCode: col(d, 'ItemCodeNew'), item: col(d, 'ItemName'),
            cropYearId: col(d, 'CropYearId'), cropYear: col(d, 'Crop'), packingTypeId: int(col(d, 'PackingTypeID')), packUomId: int(col(d, 'OrderItemUOMId')),
            packScheduleUnitId: int(col(d, 'PackScheduleUnitId')), packUom: col(d, 'UOMDescription'), qty: num(col(d, 'OrderItemQty')), weight: num(col(d, 'NetWeight')),
            itemPrice: price, packingAddLess: num(col(d, 'PackingAddLessOnRate')), rateDiscount: rd, rateDiscountAmt: price * rd / 100, rate: num(col(d, 'OrderItemRate')),
            rateUomId: int(col(d, 'OrderItemRateUOMId')), rateUom: col(d, 'EquivalentRate'), amount: num(col(d, 'Amount')), itemDiscPrct: num(col(d, 'ItemDiscount')),
            itemDiscAmt: num(col(d, 'ItemDiscountAmount')), totalAmount: num(col(d, 'TotalAmount')), cityId: int(col(d, 'CityId')), refPartyId: int(col(d, 'ReferencePartyId')),
            remarks: col(d, 'OrderRemarks'), commOnSale: col(d, 'CommOnSale') === true || String(col(d, 'CommOnSale')).toLowerCase() === 'true', costCenterId: int(col(d, 'CostCenterId'))
        };
    }
    function readById(id) {
        RecId = int(id);
        http('GET', api + '/' + RecId).then(function (po) {
            var list = po.details || [];
            if (!list.length) throw new Error('Record Not Found');
            showTab(0);
            setVal('DocDate', dateOnly(col(po, 'DocDate'))); setVal('txtdocno', col(po, 'DocNo')); setVal('txtBranchSrNo', col(po, 'BranchSrNo'));
            setVal('combordercat', col(po, 'OrderCatagoryId')); setVal('txtcatsr', col(po, 'CatagorySrNo')); setVal('CmbCustomer', col(po, 'OrderSupCustId'));
            setVal('txtsupprefno', col(po, 'SupplierRefNo')); setVal('txtremarks', col(po, 'RemarksHeader')); setByText('CmbOrderType', col(po, 'OrderType'));
            setVal('combpttrm', col(po, 'PaymentTermsId')); setVal('txtduedays', col(po, 'OrderDueDays')); setVal('duedate', dateOnly(col(po, 'OrderDueDate')));
            setByText('combdeliverytrm', col(po, 'DeliveryTerm')); setVal('deliverystartdate', dateOnly(col(po, 'DeliveryStartDate'))); setVal('txtdeliverydays', col(po, 'DeliveryDays'));
            setVal('combsalesman', int(col(po, 'BrokerAgentSupCustId')) || ''); setByText('combcommtype', col(po, 'CommissionType'));
            setVal('txtCommrate', col(po, 'CommRate')); setByText('combruom', String(col(po, 'UomScheduleIdCmRate'))); setVal('txtcommamount', col(po, 'CommAmount'));
            setVal('txtcommremarks', col(po, 'CommissionRemarks'));
            removed = []; rows = list.map(mapDetail);
            preOrderExist = rows.some(function (r) { return int(r.preOrderId) > 0; });
            enable('CmbCustomer', false); enable('DocDate', false);
            renderGrid(); refreshCombos();
            show('btnsave', false); show('btnupdate', true);
        }).catch(function (e) { box(e.message); RecId = 0; });
    }

    // ------------------------------------------------------------------ reset (Reset :2073)
    function reset() {
        updateIndex = -1; removed = []; rows = []; preOrderExist = false; RecId = 0; detailId = 0;
        enable('CmbCustomer', true); enable('DocDate', true);
        ['txtPriceSchId', 'txtRateUomId', 'txtRateUom', 'txtsupprefno', 'txtduedays', 'txtdeliverydays', 'txtCommrate', 'txtcommamount', 'txtcommremarks',
            'txtqty', 'txtweight', 'txtItemPrice', 'txtamount', 'txtremarks'].forEach(function (x) { setVal(x, ''); });
        setVal('CmbCustomer', ''); setVal('combsalesman', ''); setVal('combcommtype', ''); setVal('CmbItem', ''); setVal('combitempck', '');
        enable('CmbItem', true);
        show('btnsave', true); show('btnupdate', false); show('btnplus', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
        selectIndex('combordercat', 2);                                               /* Rows[2].Activate() */
        renderGrid(); refreshCombos();
        numbers(); focus('CmbCustomer');
    }

    // ------------------------------------------------------------------ toolbar
    function btnnew_Click() { showTab(0); reset(); categoryNo(); }
    function btnRefresh_Click() {                                                     /* btnRefresh_Click :2000 */
        http('GET', api + '/lookups').then(function (r) {
            var keep = { cat: val('combordercat'), cust: val('CmbCustomer'), sal: val('combsalesman'), item: val('CmbItem'), pt: val('combpttrm') };
            L = r || {}; bindLists();
            ['cat:combordercat', 'cust:CmbCustomer', 'sal:combsalesman', 'item:CmbItem', 'pt:combpttrm'].forEach(function (p) { var k = p.split(':'); if (keep[k[0]]) setVal(k[1], keep[k[0]]); });
            renderGrid(); refreshCombos();
        }).catch(function (e) { box(e.message); });
    }
    function btnsave_Click() { RecId = 0; insert(); }
    function btnupdate_Click() {
        if (RecId === 0) { box('Record Not Update because RecId Not Found'); return; }
        insert();
    }
    function btnDelete_Click() { /* hidden in the desktop form and has no body */ }
    function btnprint_Click() { printSlip(RecId); }
    function btnAttachment_Click() { box('Attachments are not available in the web version of this screen.'); }
    function btnPackingPrice_Click() { box('Packing Price (PackingChangePriceSchedule) is not available in the web version of this screen.'); }
    function btnLoadPre_Click() { box('Load Pre Booking Order is not available in the web version of this screen.'); }
    function btnShortcutKeys_Click() {
        box(['Ctrl+S\tFor Save in Form Tab and For Show History in History Tab', 'Ctrl+U\tFor Update', 'Ctrl+E\tFor Close', 'Ctrl+R\tFor Refresh', 'Ctrl+N\tFor New',
            'Ctrl+P\tFor Print 273A', 'Alt+1\tFor Print 273A', 'Ctrl+F5\tFor Focus on Doc Date', 'Ctrl+F10\tFor Open Attachments', 'Ctrl+T\tFor Tab Transfer',
            'Ctrl+ArrowDown\tFor Focus On Detail Grid when focus in form tab and for focus on history grid when in history tab',
            'Ctrl+ArrowUp\tFor Focus on Item in Detail Grid when in Form tab and For focus on FromDate in history tab',
            'Ctrl+Enter\tWhen Focus On Any Grid For Update Record'].join('\n'));
    }

    // ------------------------------------------------------------------ history (gridhistoryfill :2750)
    var HCOLS = [['BranchName', 'BranchName', '', 'b'], ['BranchSrNo', 'BranchSrNo', '', 'b'], ['DocNo', 'DocNo', ''], ['DocDate', 'DocDate', 'd'], ['CustomerName', 'CustomerName', ''],
        ['DueDate', 'DueDate', 'd'], ['OrderExpiryDate', 'OrderExpiryDate', 'd'], ['TermsDescription', 'PaymentTerm', ''], ['DeliveryTerm', 'DeliveryTerm', ''],
        ['OrderStatus', 'OrderStatus', ''], ['UserName', 'UserName', ''], ['EntryDate', 'EntryDate', 'd'], ['ModifyUserName', 'ModifyUser', ''], ['ModifyDate', 'ModifyDate', 'd'],
        ['ApprovedUser', 'ApprovedUser', ''], ['PostDate', 'ApprovedDate', 'd'], ['NoOfAttachments', 'NoOfAttachments', '']];
    function histMode() { return $id('rdentrydate').checked ? 'entry' : $id('rdmodifydate').checked ? 'modify' : $id('rdapproveddate').checked ? 'approved' : 'doc'; }
    function selectedBranchIds() { var s = []; HB.branches.forEach(function (b) { if (HB.checked[b.id]) s.push(b.id); }); return s.join(','); }
    function showHistory() {
        var q = '?mode=' + histMode() + '&from=' + encodeURIComponent(val('FromDateHistory')) + '&to=' + encodeURIComponent(val('ToDateHistory')) +
            '&fromNo=' + num(val('FromDocNoHistory')) + '&toNo=' + num(val('ToDocNoHistory')) + '&customerId=' + int(val('CmbCustomerHistory')) +
            '&branchIds=' + encodeURIComponent(selectedBranchIds());
        http('GET', api + '/history' + q).then(function (r) { hist = r || []; histLoaded = true; renderHistory(); renderHistDetail([]); }).catch(function (e) { box(e.message); });
    }
    function cell(v, kind) {
        if (kind === 'd') return '<td>' + esc(v ? stamp(v) : '') + '</td>';
        if (kind === 'n') return '<td class="n">' + esc(v === null || v === undefined || v === '' ? '' : Number(v).toLocaleString('en-US', { maximumFractionDigits: 0 })) + '</td>';
        return '<td>' + esc(v === null || v === undefined ? '' : v) + '</td>';
    }
    function renderHistory() {
        var h = '<th>Edit</th><th>Print</th>', cols = HCOLS.filter(function (c) { return !(c[3] === 'b' && cfg.branchWise); });
        cols.forEach(function (c) { h += '<th>' + esc(c[1]) + '</th>'; });
        $id('histHead').innerHTML = h;
        var b = '';
        hist.forEach(function (r, i) {
            b += '<tr data-i="' + i + '"><td class="ctr"><a class="glink" data-edit="' + i + '">Edit</a></td><td class="ctr"><a class="glink" data-print="' + i + '">Print</a></td>';
            cols.forEach(function (c) { b += cell(col(r, c[0]), c[2]); });
            b += '</tr>';
        });
        $id('histBody').innerHTML = b;
        Array.prototype.forEach.call($id('histBody').querySelectorAll('tr'), function (tr) {
            tr.addEventListener('click', function () { selectHist(int(tr.getAttribute('data-i'))); });
            tr.addEventListener('dblclick', function () { readById(int(col(hist[int(tr.getAttribute('data-i'))], 'Id'))); });
        });
        Array.prototype.forEach.call($id('histBody').querySelectorAll('a[data-edit]'), function (a) {
            a.addEventListener('click', function (ev) { ev.stopPropagation(); var id = int(col(hist[int(a.getAttribute('data-edit'))], 'Id')); reset(); readById(id); });
        });
        Array.prototype.forEach.call($id('histBody').querySelectorAll('a[data-print]'), function (a) {
            a.addEventListener('click', function (ev) { ev.stopPropagation(); printSlip(int(col(hist[int(a.getAttribute('data-print'))], 'Id'))); });
        });
    }
    var DCOLS = [['RefDocNo', 'PreOrder', ''], ['ItemCodeNew', 'ItemCode', ''], ['ItemName', 'ItemName', ''], ['Crop', 'CropYear', ''], ['UOMDescription', 'PackUom', ''],
        ['OrderItemQty', 'QTY', 'n'], ['NetWeight', 'Weight', 'n'], ['ItemPrice', 'ItemPrice', ''], ['PackingAddLessOnRate', 'Packing Exp/Disc', ''], ['RateDiscount', 'PartyDiscount', ''],
        ['OrderItemRate', 'NetRate', ''], ['EquivalentRate', 'RateUOM', ''], ['Amount', 'Amount', 'n'], ['ItemDiscount', 'ItemDiscPrct', 'n'], ['ItemDiscountAmount', 'ItemDiscAmount', 'n'],
        ['TotalAmount', 'NetAmount', 'n'], ['CityArea', 'CityName', ''], ['ReferencePartyName', 'ReferenceParty', ''], ['OrderRemarks', 'Remarks', ''], ['CommOnSale', 'CommOnSale', '']];
    function renderHistDetail(list) {
        var h = '', b = '';
        DCOLS.forEach(function (c) { h += '<th>' + esc(c[1]) + '</th>'; });
        $id('detHead').innerHTML = h;
        list.forEach(function (r) { b += '<tr>'; DCOLS.forEach(function (c) { b += cell(col(r, c[0]), c[2]); }); b += '</tr>'; });
        $id('detBody').innerHTML = b;
    }
    function selectHist(i) {
        Array.prototype.forEach.call($id('histBody').querySelectorAll('tr'), function (tr) { tr.className = int(tr.getAttribute('data-i')) === i ? 'sel' : ''; });
        http('GET', api + '/' + int(col(hist[i], 'Id'))).then(function (h) { renderHistDetail(h.details || []); }).catch(function (e) { box(e.message); });
    }
    function initHistoryDates() {
        var d = new Date(), days = int(cfg.historyDays) > 0 ? int(cfg.historyDays) : 3;
        setVal('ToDateHistory', iso(d)); d.setDate(d.getDate() - days); setVal('FromDateHistory', iso(d));
    }
    function btnResetHistory() {                                                      /* btnResetHistory_Click :2614 */
        var d = new Date(); setVal('ToDateHistory', iso(d)); d.setDate(d.getDate() - 3); setVal('FromDateHistory', iso(d));
        setVal('FromDocNoHistory', ''); setVal('ToDocNoHistory', ''); setVal('CmbCustomerHistory', '');
        hist = []; renderHistory(); renderHistDetail([]); $id('drdocdate').checked = true; refreshCombos();
    }
    function btnRefreshHistory_Click() {                                              /* btnRefreshHistory_Click :2633 */
        http('GET', api + '/lookups').then(function (r) { L = r; initBranches(); fill('CmbCustomerHistory', L.historyCustomers, 'Id', 'Customer'); refreshCombos(); }).catch(function (e) { box(e.message); });
    }
    function branchText() { var n = []; HB.branches.forEach(function (b) { if (HB.checked[b.id]) n.push(b.name); }); $id('hBranchText').value = n.join(','); }
    function initBranches() {                                                         /* HistoryBranchComboFill :2650 */
        HB.branches = (L.historyBranches || []).map(function (b) { return { id: String(col(b, 'Id')), name: String(col(b, 'BranchName')) }; });
        HB.checked = {}; HB.checked[String(L.userBranchId)] = true;
        var h = '';
        HB.branches.forEach(function (b) { h += '<label><input type="checkbox" data-b="' + b.id + '"' + (HB.checked[b.id] ? ' checked' : '') + '/> ' + esc(b.name) + '</label>'; });
        var l = $id('hBranchList'); l.innerHTML = h;
        Array.prototype.forEach.call(l.querySelectorAll('input'), function (c) {
            c.addEventListener('change', function () {
                HB.checked[c.getAttribute('data-b')] = c.checked; branchText();
                http('GET', api + '/history-customers?branchIds=' + encodeURIComponent(selectedBranchIds())).then(function (r) { fill('CmbCustomerHistory', r, 'Id', 'Customer'); refreshCombos(); });
            });
        });
        branchText();
    }
    function showTab(n) {
        tab = n;
        $id('tabPage1').style.display = n === 0 ? '' : 'none'; $id('tabPage2').style.display = n === 1 ? '' : 'none';
        $id('tabForm').className = n === 0 ? 'on' : ''; $id('tabHistory').className = n === 1 ? 'on' : '';
        if (n === 1 && !histLoaded) { histLoaded = true; showHistory(); }
    }

    // ------------------------------------------------------------------ keys (PurchsaeOrder_KeyDown :3285)
    function onKey(e) {
        var k = e.key, ctrl = e.ctrlKey;
        if (k === 'Enter' && !ctrl && e.target && /^(INPUT|SELECT)$/.test(e.target.tagName) && e.target.type !== 'checkbox' && e.target.type !== 'radio') {
            var f = Array.prototype.filter.call(document.querySelectorAll('#frm input:not([readonly]):not([disabled]):not([type=hidden]), #frm select:not([disabled])'), function (x) { return x.offsetParent !== null; });
            var i = f.indexOf(e.target);
            if (i >= 0 && i < f.length - 1) { e.preventDefault(); f[i + 1].focus(); }
            return;
        }
        if (e.altKey && (k === '1') && !ctrl && tab === 0 && perms.print !== false) { e.preventDefault(); btnprint_Click(); return; }
        if (!ctrl) return;
        var key = k.toLowerCase();
        if (key === 't') { e.preventDefault(); showTab(tab === 0 ? 1 : 0); focus(tab === 0 ? 'DocDate' : 'FromDateHistory'); return; }
        if (tab === 0) {
            if (key === 'n') { e.preventDefault(); btnnew_Click(); }
            else if (key === 's' && saveMode()) { e.preventDefault(); btnsave_Click(); }
            else if (key === 'r') { e.preventDefault(); btnRefresh_Click(); }
            else if (key === 'u' && $id('btnupdate').style.display !== 'none') { e.preventDefault(); btnupdate_Click(); }
            else if (k === 'F5') { e.preventDefault(); focus('DocDate'); }
            else if (k === 'F10') { e.preventDefault(); btnAttachment_Click(); }
            else if (k === 'ArrowDown') { e.preventDefault(); var g = $id('panel6'); if (g) { g.tabIndex = 0; g.focus(); } }
            else if (k === 'ArrowUp') { e.preventDefault(); focus('CmbItem'); }
            else if (key === 'p' && perms.print !== false) { e.preventDefault(); btnprint_Click(); }
        } else {
            if (key === 's') { e.preventDefault(); showHistory(); }
            else if (key === 'n') { e.preventDefault(); btnResetHistory(); }
            else if (key === 'r') { e.preventDefault(); btnRefreshHistory_Click(); }
            else if (k === 'F5' || k === 'ArrowUp') { e.preventDefault(); focus('FromDateHistory'); }
            else if (k === 'ArrowDown') { e.preventDefault(); var h = $id('tblHistory'); if (h && h.parentNode) { h.parentNode.tabIndex = 0; h.parentNode.focus(); } }
        }
    }

    // ------------------------------------------------------------------ init (BookingOrder_Load :437)
    function init() {
        renderGrid();
        setVal('DocDate', today()); setVal('duedate', today()); setVal('deliverystartdate', today());
        on('btnplus', 'click', btnplus_Click); on('btnUpdateDetail', 'click', btnUpdateDetail_Click);
        on('btnCancelUpdateDetial', 'click', function () {
            show('btnplus', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false); resetDetail();
        });
        on('btnShowHistory', 'click', showHistory);
        on('hBranchText', 'click', function () { var l = $id('hBranchList'); l.style.display = l.style.display === 'block' ? 'none' : 'block'; });
        on('txtqty', 'input', qtyChanged);
        on('txtCommrate', 'input', totalCommissionAmount); on('combcommtype', 'change', totalCommissionAmount); on('combruom', 'change', totalCommissionAmount);
        on('combsalesman', 'change', totalCommissionAmount);
        on('combitempck', 'change', packLeave); on('cmbPackType', 'change', function () { getPackingAddLess().then(function () { calculateWeight(); totalAmount(); totalCommissionAmount(); }); });
        on('CmbItem', 'change', itemChanged); on('CmbCropyr', 'change', cropLeave);
        on('CmbCustomer', 'change', function () { getCustomerDiscount().then(getPackingAddLess); });
        on('DocDate', 'change', function () { getCustomerDiscount().then(getPackingAddLess); });
        on('combordercat', 'change', categoryNo);
        on('combpttrm', 'change', payTermLeave);
        on('txtduedays', 'input', dueDaysChanged);
        on('rdSearchByName', 'change', fillItems); on('rdSearchByCode', 'change', fillItems);
        ['txtqty', 'txtCommrate'].forEach(function (x) {                                                       /* numeric entry only */
            on(x, 'keypress', function (e) { if (e.key.length === 1 && !/[0-9.]/.test(e.key)) e.preventDefault(); });
        });
        ['txtduedays', 'txtdeliverydays', 'FromDocNoHistory', 'ToDocNoHistory'].forEach(function (x) {
            on(x, 'keypress', function (e) { if (e.key.length === 1 && !/[0-9]/.test(e.key)) e.preventDefault(); });
        });
        document.addEventListener('keydown', onKey);
        http('GET', api + '/lookups').then(function (r) {
            L = r || {}; perms = L.rights || {}; cfg = L.config || {};
            rounding = num(cfg.roundingForItemPricing);
            $id('ChkBox').checked = perms.print !== false && !!perms.print;
            if (cfg.itemSearchByCode) $id('rdSearchByCode').checked = true; else $id('rdSearchByName').checked = true;
            setVal('txtdocno', L.nextNo); setVal('txtBranchSrNo', L.branchSr);
            bindLists(); initHistoryDates(); initBranches();
            categoryNo();
        }).catch(function (e) { box(e.message); });
    }

    window.SBO = {
        showTab: showTab, btnnew_Click: btnnew_Click, btnRefresh_Click: btnRefresh_Click, btnsave_Click: btnsave_Click, btnupdate_Click: btnupdate_Click,
        btnDelete_Click: btnDelete_Click, btnprint_Click: btnprint_Click, btnAttachment_Click: btnAttachment_Click, btnShortcutKeys_Click: btnShortcutKeys_Click,
        btnPackingPrice_Click: btnPackingPrice_Click, btnLoadPre_Click: btnLoadPre_Click, btnRefreshHistory_Click: btnRefreshHistory_Click
    };
    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', init); else init();
})();
