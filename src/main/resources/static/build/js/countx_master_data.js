/* ============================================================================================
 * countx_master_data.js - the screens of App 19 "Master Data Definition" / module 2039
 * "System_Level" (Architecture.WinApp). One script; <body data-mdd="..."> picks the form.
 *
 *   country      DefineCountry.cs        province   DefineProvince.cs      district  DefineDistrict.cs
 *   tehsil       DefineTehsil.cs         city       DefineCity.cs          currency  DefineMultiCurrency.cs
 *   sea-ports    SeaPortsDefine.cs       date-lock  DateLock.cs            other-items InvOtherItems.cs
 *  module 45 "Accounts Definition" (2026-09-30b):
 *   pdc-bank PdcBank.cs   document-group DocumentGroup.cs   shipment-documents ExImShipmentDocuments.cs
 *   tax-lookup Lookups/TaxLookup.cs   bs-pl-setting Account_Definition/BsPlSettingForm.cs   bank AcfrmDefineBank.cs
 *
 * Every handler follows its desktop form line by line - validation wording and order, confirmations,
 * messages, what New / Save / Update / double-click do to the buttons and the grid. Desktop defects are
 * reproduced, not repaired; the ones that matter are named in the project doc
 * MASTER-DATA-DEFINITION-01-HUB-AND-NINE-SYSTEM-LEVEL-SCREENS.md:
 *  - District, Tehsil, City, Date Lock: the Update button is never made visible, and Ctrl+U runs only
 *    while it is visible; after a double-click Save is hidden too, so the form is edit-only via New.
 *  - Country, Province, Currency: an Update returns 0 (no result set), so "success >= 1" fails and no
 *    message is shown; City shows "Record Update Successfully...[0]".
 *  - City: Reset() after a save does not refill the grid.
 *  - Date Lock: the DAL does not copy the new Id back, so "Data Save Successfully....  0".
 * ============================================================================================ */
(function () {
    'use strict';

    var KEY = document.body.getAttribute('data-mdd');
    var api = '/api/master-data/' + KEY;

    function $id(id) { return document.getElementById(id); }
    function box(m) { window.alert(m); }
    function ask(m) { return window.confirm(m); }
    function esc(s) {
        return String(s === null || s === undefined ? '' : s)
            .replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    }
    function col(row, name) {
        if (!row) return null;
        if (Object.prototype.hasOwnProperty.call(row, name)) return row[name];
        var l = name.toLowerCase();
        for (var k in row) if (Object.prototype.hasOwnProperty.call(row, k) && k.toLowerCase() === l) return row[k];
        return null;
    }
    function str(v) { return v === null || v === undefined ? '' : String(v); }
    function netI(v) {
        if (v === null || v === undefined || v === '') return 0;
        if (typeof v === 'number') return isFinite(v) ? Math.round(v) : 0;
        var s = String(v).trim();
        return /^[+-]?\d+$/.test(s) ? parseInt(s, 10) : 0;
    }
    function busy(btn, fn) {
        var b = (typeof btn === 'string') ? $id(btn) : btn;
        if (b) {
            if (b.disabled || b.classList.contains('is-busy')) return;
            b.disabled = true; b.classList.add('is-busy');
        }
        var done = function () { if (b) { b.disabled = false; b.classList.remove('is-busy'); } };
        var p;
        try { p = fn(); } catch (e) { done(); throw e; }
        if (p && typeof p.then === 'function') p.then(done, done); else done();
        return p;
    }
    function parse(r) {
        return r.text().then(function (t) {
            var b = null;
            try { b = t ? JSON.parse(t) : null; } catch (e) { /* not json */ }
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
    function refreshCombos() { if (window.DesktopCombo) window.DesktopCombo.refresh(); }
    function show(id, on) { var e = $id(id); if (e) e.classList.toggle('is-hidden', !on); }
    function visible(id) { var e = $id(id); return !!e && !e.classList.contains('is-hidden') && !e.disabled; }
    function val(id) { var e = $id(id); return e ? e.value : ''; }
    function setVal(id, v) { var e = $id(id); if (e) e.value = v; }
    function focus(id) { var e = $id(id); if (e) e.focus(); }
    /** BindDDLNew / BindDDL: value + display; ZeroIndex adds "...Select Any Value..." (0) first and selects it; otherwise no row is active. */
    function fill(id, rows, valueKey, textKey, zeroIndex) {
        var sel = $id(id);
        if (!sel) return;
        var html = zeroIndex ? '<option value="0">...Select Any Value...</option>' : '<option value="0"></option>';
        (rows || []).forEach(function (r) { html += '<option value="' + esc(col(r, valueKey)) + '">' + esc(col(r, textKey)) + '</option>'; });
        sel.innerHTML = html;
        sel.value = '0';
    }
    /** UltraCombo.Value = x: the row with that value, or no row. */
    function setValue(id, v) {
        var s = $id(id), x = String(v);
        if (!s) return;
        for (var i = 0; i < s.options.length; i++) if (s.options[i].value === x) { s.selectedIndex = i; return; }
        s.value = '0';
    }
    function activateFirstRow(id) { var s = $id(id); if (s && s.options.length > 1) s.selectedIndex = 1; }
    function comboText(id) { var s = $id(id); return (!s || s.selectedIndex < 0 || s.value === '0') ? '' : s.options[s.selectedIndex].textContent; }
    function shortDate(v) {
        var d = str(v).match(/^(\d{4})-(\d{2})-(\d{2})/);
        return d ? d[1] + '-' + d[2] + '-' + d[3] : '';
    }
    var MON = ['Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov','Dec'];
    /** Conversion.ToDateTime(x).ToString("dd-MMM-yyyy"). */
    function ddMMMyyyy(v) {
        var d = shortDate(v); if (!d) return '';
        var p = d.split('-'); return p[2] + '-' + MON[+p[1] - 1] + '-' + p[0];
    }
    function todayIso() { var d = new Date(), p = function (n) { return String(n).padStart(2, '0'); }; return d.getFullYear() + '-' + p(d.getMonth() + 1) + '-' + p(d.getDate()); }
    function close() {
        window.close();
        setTimeout(function () { if (!window.closed) { if (history.length > 1) history.back(); else location.href = '/master-data-definition'; } }, 150);
    }
    function openPage(path) { window.open(path, '_blank'); }

    // ------------------------------------------------------------------------------ grid

    var GRID = [], CUR = -1, COLS = [];
    function readCols(tableId) {
        COLS = [];
        document.querySelectorAll('#' + tableId + ' thead th').forEach(function (th) {
            COLS.push({ key: th.getAttribute('data-col'), hidden: th.style.display === 'none' });
        });
    }
    function draw() {
        var body = $id('gridBody');
        body.innerHTML = GRID.map(function (r, i) {
            return '<tr data-i="' + i + '"' + (i === CUR ? ' class="is-current"' : '') + '>' + COLS.map(function (c) {
                var v = r[c.key];
                return '<td' + (c.hidden ? ' style="display:none;"' : '') + (typeof v === 'number' ? ' class="md-num"' : '') + '>' + esc(v) + '</td>';
            }).join('') + '</tr>';
        }).join('');
    }
    function setGrid(rows) { GRID = rows; CUR = -1; draw(); }
    function clearGrid() { setGrid([]); }
    function wireGrid(onDouble) {
        var gb = $id('gridBody');
        gb.addEventListener('click', function (e) {
            var tr = e.target.closest('tr[data-i]'); if (!tr) return;
            CUR = +tr.getAttribute('data-i');
            gb.querySelectorAll('tr[data-i]').forEach(function (x) { x.classList.toggle('is-current', x === tr); });
        });
        gb.addEventListener('dblclick', function (e) {
            var tr = e.target.closest('tr[data-i]'); if (tr) { CUR = +tr.getAttribute('data-i'); onDouble(GRID[CUR]); }
        });
    }
    function current() { return CUR >= 0 ? GRID[CUR] : null; }

    var RecId = 0;
    var M = {};                      // the page's handlers, exported as window.MDD
    var keys = null;                 // KeyDown handler

    function saveMessage(d, wasUpdate, form) {
        var n = netI(d && d.id);
        form(n, wasUpdate);
    }

    // ============================================================================ Country (749)
    function country() {
        function gridFill() {   // CounytryDefineGridFill :80
            return getJson(api + '/list').then(function (rows) {
                setGrid((rows || []).map(function (r) { return { Id: col(r, 'Id'), CountryCode: str(col(r, 'Code')), Description: col(r, 'Description') }; }));
            }).catch(function (e) { box(e.message); });
        }
        function refresh() { setVal('txtcountrycode', ''); setVal('txtcountryName', ''); }
        function formValidation() {
            if (val('txtcountrycode').trim() === '') { box('Country Code Field Required'); focus('txtcountrycode'); return false; }
            if (val('txtcountryName').trim() === '') { box('Country Name Field Required'); focus('txtcountryName'); return false; }
            return true;
        }
        M.btnsave = function (btn) {          // saveCountry :45
            if (!formValidation()) return;
            return busy(btn || 'btnsave', function () {
                return postJson(api + '/save', { id: 0, code: val('txtcountrycode'), description: val('txtcountryName') }).then(function (d) {
                    var n = netI(d && d.id);
                    if (n >= 1) box('Record Save Successfully...[' + n + ']');
                    return gridFill().then(refresh);
                }).catch(function (e) { box(e.message); });
            });
        };
        M.btnUpdate = function (btn) {        // btnUpdate_Click :120
            if (!formValidation()) return;
            return busy(btn || 'btnUpdate', function () {
                return postJson(api + '/save', { id: RecId, code: val('txtcountrycode'), description: val('txtcountryName') }).then(function (d) {
                    var n = netI(d && d.id);
                    if (n >= 1) box('Record Update Successfully...[' + n + ']');
                    return gridFill().then(function () { refresh(); show('btnUpdate', false); });
                }).catch(function (e) { box(e.message); });
            });
        };
        M.btnnew = function () { refresh(); show('btnUpdate', false); };          // btnnew_Click :180
        M.btnDefineCity = function () { openPage('/master-data/city'); };        // toolStripButton1_Click - new DefineCity(UserAccount).Show()
        wireGrid(function (r) {                                                   // grdcountrydefine_DoubleClick :100
            show('btnsave', false);
            RecId = netI(r.Id);
            getJson(api + '/by-id?id=' + RecId).then(function (b) { setVal('txtcountrycode', str(col(b, 'Code'))); setVal('txtcountryName', str(col(b, 'Description'))); })
                .catch(function (e) { box(e.message); });
        });
        keys = function (e, k) {   // DefineCountry_KeyDown :160 - KeyPreview
            if (e.ctrlKey && k === 'n') { e.preventDefault(); M.btnnew(); }
            if (e.ctrlKey && k === 's') { e.preventDefault(); M.btnsave(); }
            if (e.ctrlKey && k === 'u') { e.preventDefault(); M.btnUpdate(); }
        };
        gridFill().then(function () { focus('txtcountrycode'); });               // DefineCountry_Load
    }

    // ============================================================================ Province (753)
    function province() {
        function gridFill(rows) {   // ProvienceDefineGridFill :62
            setGrid((rows || []).map(function (r) { return { Id: col(r, 'Id'), Country: col(r, 'CountryName'), Province: col(r, 'Description'), ProvinceCode: str(col(r, 'Code')) }; }));
        }
        function reload() { return getJson(api + '/list').then(gridFill).catch(function (e) { box(e.message); }); }
        function refresh() { setVal('txtProvienceName', ''); setVal('txtProviencecode', ''); setValue('cmbCountry', 0); refreshCombos(); }
        function formValidation() {
            if (val('txtProviencecode').trim() === '') { box('Provience Code Field Required'); focus('txtProviencecode'); return false; }
            if (val('txtProvienceName').trim() === '') { box('Provience Name Field Required'); focus('txtProvienceName'); return false; }
            if (comboText('cmbCountry').trim() === '' || val('cmbCountry') === '0') { box('Country Field Required'); focus('cmbCountry'); return false; }
            return true;
        }
        function payload(id) { return { id: id, code: val('txtProviencecode'), description: val('txtProvienceName'), countryId: netI(val('cmbCountry')) }; }
        M.btnsave = function (btn) {          // btnsave_Click :121
            if (!formValidation()) return;
            return busy(btn || 'btnsave', function () {
                return postJson(api + '/save', payload(0)).then(function (d) {
                    var n = netI(d && d.id);
                    if (n >= 1) box('Record Save Successfully...[' + n + ']');
                    return reload().then(refresh);
                }).catch(function (e) { box(e.message); });
            });
        };
        M.btnUpdate = function (btn) {        // btnUpdate_Click :90
            if (!formValidation()) return;
            return busy(btn || 'btnUpdate', function () {
                return postJson(api + '/save', payload(RecId)).then(function (d) {
                    var n = netI(d && d.id);
                    if (n >= 1) box('Record Update Successfully...[' + n + ']');
                    return reload().then(function () { refresh(); show('btnUpdate', false); });
                }).catch(function (e) { box(e.message); });
            });
        };
        M.btnnew = function () { show('btnUpdate', false); refresh(); };
        wireGrid(function (r) {               // grdProviencedefine_DoubleClick :170
            show('btnsave', false);
            RecId = netI(r.Id);
            getJson(api + '/by-id?id=' + RecId).then(function (b) {
                setVal('txtProviencecode', str(col(b, 'Code'))); setVal('txtProvienceName', str(col(b, 'Description')));
                setValue('cmbCountry', netI(col(b, 'CountryId'))); refreshCombos();
            }).catch(function (e) { box(e.message); });
        });
        keys = function (e, k) {
            if (e.ctrlKey && k === 'n') { e.preventDefault(); M.btnnew(); }
            if (e.ctrlKey && k === 's') { e.preventDefault(); M.btnsave(); }
            if (e.ctrlKey && k === 'u') { e.preventDefault(); M.btnUpdate(); }
        };
        getJson(api + '/setup').then(function (d) {     // DefineProvince_Load: CountryComboFill (BindDDL ZeroIndex true), grid
            d = d || {};
            if (d.countriesError) box(d.countriesError); else if ((d.countries || []).length) fill('cmbCountry', d.countries, 'Id', 'Description', true);
            refreshCombos();
            gridFill(d.provinces);
        }).catch(function (e) { box(e.message); });
    }

    // ============================================================================ District (756) / Tehsil (755)
    function districtOrTehsil(cfg) {
        // cfg: { combo, comboRows, comboValue, comboText, comboCaptionReq, name, nameReq, listKey, refreshUrl, define }
        function formHistory(rows) { if ((rows || []).length) setGrid(rows.map(function (r) { var o = {}; COLS.forEach(function (c) { o[c.key] = c.key.endsWith('Date') ? shortDate(col(r, c.key)) : col(r, c.key); }); return o; })); }
        function history() { return getJson(api + '/list').then(formHistory).catch(function (e) { box(e.message); }); }
        function reset() {   // Reset - focus combo, clear, combo text empty, Update hidden, FormHistory
            focus(cfg.combo); setVal(cfg.name, ''); setValue(cfg.combo, 0); refreshCombos(); show('btnUpdate', false); return history();
        }
        function formValidation() {
            if (val(cfg.combo) === '0' || val(cfg.combo) === '') { box(cfg.comboCaptionReq); focus(cfg.combo); return false; }
            if (val(cfg.name).trim() === '') { box(cfg.nameReq); focus(cfg.name); return false; }
            return true;
        }
        function insert(btn) {   // Insert()
            if (!formValidation()) return;
            if (RecId > 0) { if (!ask('Are you sure to Update?')) return; }
            else if (!ask('Are you sure to Save?')) return;
            var was = RecId, body = { id: RecId, name: val(cfg.name) }; body[cfg.bodyKey] = netI(val(cfg.combo));
            return busy(btn, function () {
                return postJson(api + '/save', body).then(function () {
                    box(was > 0 ? 'Record Update Successfully' : 'Record Save Successfully');
                    return reset();
                }).catch(function (e) { box(e.message); });
            });
        }
        M.btnsave = function (btn) { RecId = 0; return insert(btn || 'btnsave'); };
        M.btnUpdate = function (btn) { return insert(btn || 'btnUpdate'); };
        M.btnnew = function () { return reset(); };
        M.btnRefresh = function (btn) {   // btnRefresh_Click - the combo is rebound
            return busy(btn || 'btnRefresh', function () {
                return getJson(cfg.refreshUrl).then(function (rows) { if ((rows || []).length) fill(cfg.combo, rows, 'Id', cfg.comboText, false); refreshCombos(); })
                    .catch(function (e) { box(e.message); });
            });
        };
        M.btnDefineDistrict = function () { openPage('/master-data/district'); };   // DefineDistrict.ShowDialog()
        wireGrid(function (r) {   // grdcountrydefine_DoubleClick
            show('btnsave', false);
            RecId = netI(r.Id);
            setValue(cfg.combo, netI(r[cfg.comboValue])); refreshCombos();
            setVal(cfg.name, str(r[cfg.gridText]));
            focus(cfg.combo);
        });
        keys = function (e, k) {   // DefineCountry_KeyDown - Ctrl+S / Ctrl+U only while that button is visible and enabled
            if (e.ctrlKey && k === 'n') { e.preventDefault(); M.btnnew(); }
            if (e.ctrlKey && k === 's' && visible('btnsave')) { e.preventDefault(); RecId = 0; insert('btnsave'); }
            if (e.ctrlKey && k === 'u' && visible('btnUpdate')) { e.preventDefault(); insert('btnUpdate'); }
        };
        getJson(api + '/setup').then(function (d) {   // Load: bind combo (only when rows), FormHistory, focus combo, Update hidden
            d = d || {};
            if (d[cfg.comboRows + 'Error']) box(d[cfg.comboRows + 'Error']);
            else if ((d[cfg.comboRows] || []).length) fill(cfg.combo, d[cfg.comboRows], 'Id', cfg.comboText, false);
            refreshCombos();
            if (d[cfg.listKey + 'Error']) box(d[cfg.listKey + 'Error']); else formHistory(d[cfg.listKey]);
            focus(cfg.combo); show('btnUpdate', false);
        }).catch(function (e) { box(e.message); });
    }

    // ============================================================================ City (750)
    function city() {
        function gridFill(rows) {   // CityDefineGridFill - only when rows came back
            if ((rows || []).length) setGrid(rows.map(function (r) { return { Id: col(r, 'Id'), TehsilId: col(r, 'TehsilId'), Tehsil: col(r, 'Tehsil'), CityName: col(r, 'CityName') }; }));
        }
        function reset() { focus('CmbTehsil'); setValue('CmbTehsil', 0); refreshCombos(); setVal('txtcityname', ''); show('btnUpdate', false); }   // Reset :55 - no grid refill
        function formValidation() {
            if (val('txtcityname').trim() === '') { box('CityName Field is Required'); focus('txtcityname'); return false; }
            return true;
        }
        function insert(btn) {   // Insert() :80
            if (!formValidation()) return;
            if (RecId > 0) { if (!ask('Are you sure to Update?')) return; }
            else if (!ask('Are you sure to Save?')) return;
            var was = RecId;
            return busy(btn, function () {
                return postJson(api + '/save', { id: RecId, name: val('txtcityname'), tehsilId: netI(val('CmbTehsil')) }).then(function (d) {
                    var n = netI(d && d.id);
                    box(was > 0 ? 'Record Update Successfully...[' + n + ']' : 'Record Save Successfully...[' + n + ']');
                    reset();
                }).catch(function (e) { box(e.message); });
            });
        }
        M.btnsave = function (btn) { RecId = 0; return insert(btn || 'btnsave'); };
        M.btnUpdate = function (btn) { return insert(btn || 'btnUpdate'); };
        M.btnnew = function () { reset(); };
        M.btnRefresh = function (btn) {   // btnRefresh_Click - TehsilBind
            return busy(btn || 'btnRefresh', function () {
                return getJson(api + '/tehsils').then(function (rows) { if ((rows || []).length) fill('CmbTehsil', rows, 'Id', 'Tehsil', true); refreshCombos(); })
                    .catch(function (e) { box(e.message); });
            });
        };
        M.btnDefineTehsil = function () { openPage('/master-data/tehsil'); };   // DefineTehsil.ShowDialog()
        wireGrid(function (r) {   // grdProviencedefine_DoubleClick :150
            show('btnsave', false);
            RecId = netI(r.Id);
            setVal('txtcityname', str(r.CityName));
            setValue('CmbTehsil', netI(r.TehsilId)); refreshCombos();
        });
        keys = function (e, k) {
            if (e.ctrlKey && k === 'n') { e.preventDefault(); M.btnnew(); }
            if (e.ctrlKey && k === 's' && visible('btnsave')) { e.preventDefault(); M.btnsave(); }
            if (e.ctrlKey && k === 'u' && visible('btnUpdate')) { e.preventDefault(); M.btnUpdate(); }
        };
        getJson(api + '/setup').then(function (d) {   // DefineCity_Load: TehsilBind (ZeroIndex true), Update hidden, grid, focus combo
            d = d || {};
            if (d.tehsilsError) box(d.tehsilsError); else if ((d.tehsils || []).length) fill('CmbTehsil', d.tehsils, 'Id', 'Tehsil', true);
            refreshCombos();
            show('btnUpdate', false);
            if (d.citiesError) box(d.citiesError); else gridFill(d.cities);
            focus('CmbTehsil');
        }).catch(function (e) { box(e.message); });
    }

    // ============================================================================ Currency (751)
    function currency() {
        var UpdateMod = false;
        var F = ['txtCode', 'txtCurrencyName', 'txtcurrencyRate', 'txtCurrencySymbol', 'txtCurrencyURL'];
        function bindGrid() {   // BindGridCurrency :110
            return getJson(api + '/list').then(function (rows) {
                setGrid((rows || []).map(function (r) { return { Id: col(r, 'Id'), CurrencyCode: col(r, 'CurrencyCode'), CurrencyName: col(r, 'CurrencyName'), CurrencyRate: col(r, 'CurrencyRate'),
                    CurrencySymbol: col(r, 'CurrencySymbol'), URL: col(r, 'Url'), Status: col(r, 'CurrencyStatus') }; }));
            }).catch(function (e) { box(e.message); });
        }
        function formRefresh() { focus('txtCode'); F.forEach(function (f) { setVal(f, ''); }); return bindGrid(); }
        function formValidation() {
            if (val('txtCode').trim() === '') { box('Currency Code Field Required'); focus('txtCode'); return false; }
            if (val('txtCurrencyName').trim() === '') { box('Currency Name Field Required'); focus('txtCurrencyName'); return false; }
            if (val('txtcurrencyRate').trim() === '') { box('Currency Rate Field Required'); focus('txtcurrencyRate'); return false; }
            return true;
        }
        function payload(id) { return { id: id, code: val('txtCode'), name: val('txtCurrencyName'), rate: val('txtcurrencyRate'), symbol: val('txtCurrencySymbol'), url: val('txtCurrencyURL') }; }
        M.btnsave = function (btn) {   // btnsave_Click :39
            if (!formValidation()) return;
            return busy(btn || 'btnsave', function () {
                return postJson(api + '/save', payload(0)).then(function (d) {
                    var n = netI(d && d.id);
                    if (n >= 1) box('Record Save Successfully...[' + n + ']');
                    return bindGrid().then(formRefresh);
                }).catch(function (e) { box(e.message); });
            });
        };
        M.btnUpdate = function (btn) {   // btnupdate_Click :70 - the Update button is hidden only when success >= 1
            if (!formValidation()) return;
            return busy(btn || 'btnUpdate', function () {
                return postJson(api + '/save', payload(RecId)).then(function (d) {
                    var n = netI(d && d.id);
                    if (n >= 1) { box('Record Update Successfully...[' + n + ']'); show('btnUpdate', false); }
                    return bindGrid().then(formRefresh);
                }).catch(function (e) { box(e.message); });
            });
        };
        M.btnnew = function () { formRefresh(); show('btnUpdate', false); };
        wireGrid(function (r) {   // grdcurrency_DoubleClick :95
            show('btnsave', false);
            UpdateMod = true;
            RecId = netI(r.Id);
            getJson(api + '/by-id?id=' + RecId).then(function (b) {
                setVal('txtCode', str(col(b, 'CurrencyCode'))); setVal('txtCurrencyName', str(col(b, 'CurrencyName')));
                setVal('txtcurrencyRate', str(col(b, 'CurrencyRate'))); setVal('txtCurrencySymbol', str(col(b, 'CurrencySymbol')));
                setVal('txtCurrencyURL', str(col(b, 'Url')));
            }).catch(function (e) { box(e.message); });
        });
        keys = function (e, k) {   // DefineMultiCurrency_KeyDown
            if (e.ctrlKey && k === 'n') { e.preventDefault(); M.btnnew(); }
            if (e.ctrlKey && k === 's' && !UpdateMod) { e.preventDefault(); M.btnsave(); }
            if (e.ctrlKey && k === 'u' && UpdateMod) { e.preventDefault(); M.btnUpdate(); }
        };
        bindGrid().then(function () { focus('txtCode'); });   // DefineMultiCurrency_Load
    }

    // ============================================================================ Sea Ports (752)
    function seaPorts() {
        var canSave = true, canUpdate = true;
        function typeFill() {   // TypeFill :62 - a fixed two-row table, ZeroIndex true, the old value kept when it still exists
            var id = netI(val('CmbType'));
            fill('CmbType', [{ PortTypeId: 1, PortType: 'Destination' }, { PortTypeId: 2, PortType: 'Loading' }], 'PortTypeId', 'PortType', true);
            if (id > 0) setValue('CmbType', id);
            refreshCombos();
        }
        function bindGrid(rows) { if ((rows || []).length) setGrid(rows.map(function (r) { return { Id: col(r, 'Id'), PortName: col(r, 'PortName'), PortType: col(r, 'PortType') }; })); }
        function reload() { return getJson(api + '/list').then(bindGrid).catch(function (e) { box(e.message); }); }
        function reset() { RecId = 0; setVal('txtDescription', ''); show('btnupdate', false); $id('btnAdd').textContent = 'Save'; }   // Reset :160
        function insert(btn) {   // Insert() :74
            if (val('txtDescription') === '') { box('Description Field is Required'); focus('txtDescription'); return; }
            if (netI(val('CmbType')) === 0) { box('Type Field is Required'); focus('CmbType'); return; }
            if (RecId > 0) { if (!ask('Are you sure to Update?')) return; }
            else if (!ask('Are you sure to Save?')) return;
            var was = RecId;
            return busy(btn, function () {
                return postJson(api + '/save', { id: RecId, description: val('txtDescription'), portTypeId: netI(val('CmbType')) }).then(function () {
                    box(was > 0 ? 'Record Update Successfully' : 'Record Save Successfully');
                    reset();
                    return reload();
                }).catch(function (e) { box(e.message); });
            });
        }
        function readById(id) {   // ReadById :137
            RecId = id;
            return getJson(api + '/by-id?id=' + id).then(function (b) {
                if (!b) return;
                setVal('txtDescription', str(col(b, 'PortName')));
                setValue('CmbType', netI(col(b, 'PortTypeId'))); refreshCombos();
                show('btnSave', false);
                $id('btnAdd').textContent = 'Update';
            }).catch(function (e) { box(e.message); });
        }
        M.btnSave = function (btn) { RecId = 0; return insert(btn || 'btnSave'); };
        M.btnupdate = function (btn) { return insert(btn || 'btnupdate'); };
        M.btnAdd = function (btn) { return insert(btn || 'btnAdd'); };
        M.btnnew = function () { reset(); };
        M.btnRefreshMenu = function () { typeFill(); reset(); };   // refreshToolStripMenuItem_Click
        M.btnShortCutKeys = function () {   // MakeShortCutKeys - the ShortCutKeyPopUp table, as text
            box('Ctrl+S\tFor Save\nCtrl+U\tFor Update\nCtrl+E\tFor Close\nCtrl+R\tFor Refresh\nCtrl+N\tFor New\nCtrl+F5\tFor Focus on Description\nCtrl+alt\tTo Show ShortCut Keys Form\nCtrl+ArrowDown\tFor Focus On Grid\nCtrl+ArrowUp\tFor Focus On Description\nCtrl+Enter\tWhen Focus On Any Grid For Update Record');
        };
        wireGrid(function (r) { readById(netI(r.Id)); });   // grdfrm_DoubleClick
        keys = function (e, k) {   // SeaPortsDefine_KeyDown
            if (e.ctrlKey && k === 'n') { e.preventDefault(); M.btnnew(); }
            if (e.ctrlKey && k === 's' && visible('btnSave')) { e.preventDefault(); M.btnSave(); }
            if (e.ctrlKey && k === 'u' && visible('btnupdate')) { e.preventDefault(); M.btnupdate(); }
            if (e.ctrlKey && e.key === 'ArrowDown') { e.preventDefault(); var t = $id('gridBody').querySelector('tr'); if (t) { t.setAttribute('tabindex', '-1'); t.focus(); } }
            if (e.ctrlKey && (e.key === 'ArrowUp' || e.key === 'F5')) { e.preventDefault(); focus('txtDescription'); }
            if (e.ctrlKey && e.key === 'Enter') { e.preventDefault(); var c = current(); if (c) readById(netI(c.Id)); }
            if (e.ctrlKey && e.altKey && !e.repeat && k === 'alt') { e.preventDefault(); M.btnShortCutKeys(); }
        };
        getJson(api + '/setup').then(function (d) {   // DateLock_Load :54 (the form's Load keeps that name)
            d = d || {};
            canSave = d.canSave !== false; canUpdate = d.canUpdate !== false;
            $id('btnSave').disabled = !canSave; $id('btnAdd').disabled = !canSave; $id('btnupdate').disabled = !canUpdate;
            show('btnupdate', false);
            $id('btnAdd').textContent = 'Save';
            typeFill();
            if (d.portsError) box(d.portsError); else bindGrid(d.ports);
        }).catch(function (e) { box(e.message); });
    }

    // ============================================================================ Date Lock (754)
    function dateLock() {
        function bindGrid(rows) {   // BindGrid :95 - Date as dd-MMM-yyyy
            setGrid((rows || []).map(function (r) { return { Id: col(r, 'Id'), Date: ddMMMyyyy(col(r, 'Date')), Status: col(r, 'Status') }; }));
        }
        function reload() { return getJson(api + '/list').then(bindGrid).catch(function (e) { box(e.message); }); }
        function reset() { setVal('txtDate', todayIso()); RecId = 0; $id('cmbStatus').selectedIndex = 0; show('btnupdate', false); focus('cmbBranch'); return reload(); }   // Reset :190
        M.btnsave = function (btn) {   // btnSave_Click :70 - no validation
            if (RecId > 0) { if (!ask('Are you sure to Update?')) return; }
            else if (!ask('Are you sure to Save?')) return;
            var was = RecId;
            return busy(btn || 'btnsave', function () {
                return postJson(api + '/save', { id: RecId, date: val('txtDate'), status: val('cmbStatus'), branchesId: netI(val('cmbBranch')), projectsId: netI(val('cmbProject')) }).then(function () {
                    /* the DAL does not copy the new Id into dateLock.id, so the desktop prints 0 */
                    box(was > 0 ? 'Data Update Successfully.... ' : 'Data Save Successfully....  0');
                    return reset();
                }).catch(function (e) { box(e.message); });
            });
        };
        M.btnupdate = function (btn) { return M.btnsave(btn || 'btnupdate'); };   // btnUpdate_Click -> btnSave_Click
        M.btnnew = function () { return reset(); };
        wireGrid(function (r) {   // RetrivedData :150
            show('btnsave', false);
            RecId = netI(r.Id);
            getJson(api + '/by-id?id=' + RecId).then(function (b) {
                setVal('txtDate', shortDate(col(b, 'Date')));
                setValue('cmbBranch', netI(col(b, 'BranchesId'))); setValue('cmbProject', netI(col(b, 'ProjectsId'))); refreshCombos();
                var s = col(b, 'Status'); $id('cmbStatus').value = (s === true || s === 1 || s === '1' || s === 'true') ? 'Lock' : 'Unlock';
            }).catch(function (e) { box(e.message); });
        });
        keys = function (e, k) {   // DateLock_KeyDown - Enter -> Tab
            if (e.key === 'Enter' && !e.ctrlKey && e.target && e.target.tagName !== 'BUTTON') {
                e.preventDefault();
                var f = Array.prototype.filter.call(document.querySelectorAll('input, select, button'), function (x) { return !x.disabled && x.offsetParent !== null; });
                var i = f.indexOf(e.target); if (i >= 0 && i + 1 < f.length) f[i + 1].focus();
            }
            if (e.ctrlKey && k === 's' && visible('btnsave')) { e.preventDefault(); M.btnsave(); }
            if (e.ctrlKey && k === 'n') { e.preventDefault(); M.btnnew(); }
            if (e.ctrlKey && k === 'u' && visible('btnupdate')) { e.preventDefault(); M.btnupdate(); }
        };
        $id('cmbStatus').selectedIndex = 0;
        setVal('txtDate', todayIso());
        getJson(api + '/setup').then(function (d) {   // DateLock_Load: BindGrid, ProjectComboFill, BranchComboFill, Rows[0].Activate()
            d = d || {};
            if (d.locksError) box(d.locksError); else bindGrid(d.locks);
            if (d.projectsError) box(d.projectsError); else if ((d.projects || []).length) fill('cmbProject', d.projects, 'Id', 'ProjectName', false);
            if (d.branchesError) box(d.branchesError); else if ((d.branches || []).length) fill('cmbBranch', d.branches, 'Id', 'BranchName', false);
            activateFirstRow('cmbBranch'); activateFirstRow('cmbProject');
            refreshCombos();
        }).catch(function (e) { box(e.message); });
    }

    // ============================================================================ Other Items (742)
    function otherItems() {
        function bindGrid(rows) {   // BindGrid :150
            setGrid((rows || []).map(function (r) { return { id: col(r, 'Id'), 'Item Type': col(r, 'OtherItemType'), 'Item Name': col(r, 'OtherItemName'),
                StockGLAc: col(r, 'AccountTitleStock'), SaleGLAc: col(r, 'AccountTitleSales'), CogsGLAc: col(r, 'AccountTitleCgs') }; }));
        }
        function reload() { return getJson(api + '/list').then(bindGrid).catch(function (e) { box(e.message); }); }
        function reset() {   // Reset :60
            setVal('txtCode', ''); setVal('txtItemName', '');
            ['cmbCGSGL', 'cmbPurchaseGL', 'cmbSaleGL', 'cmbBranch', 'cmbProject'].forEach(function (c) { setValue(c, 0); });
            refreshCombos(); focus('txtCode'); show('btnupdate', false);
            return reload();
        }
        function formValidation() {   // FormValidation :36 - text == "" or "0"
            var t;
            t = val('txtCode'); if (t === '' || t === '0') { box('Code field required'); focus('txtCode'); return false; }
            t = val('txtItemName'); if (t === '' || t === '0') { box('Item Name field required'); focus('txtItemName'); return false; }
            t = comboText('cmbPurchaseGL'); if (t === '' || t === '0') { box('Purchase GL field required'); focus('cmbPurchaseGL'); return false; }
            t = comboText('cmbSaleGL'); if (t === '' || t === '0') { box('Sale GL field required'); focus('cmbSaleGL'); return false; }
            t = comboText('cmbCGSGL'); if (t === '' || t === '0') { box('CGS GL field required'); focus('cmbCGSGL'); return false; }
            return true;
        }
        function payload(id) {
            return { id: id, code: val('txtCode'), name: val('txtItemName'), stockGLAcId: netI(val('cmbPurchaseGL')), saleGLAcId: netI(val('cmbSaleGL')),
                cogsGLAcId: netI(val('cmbCGSGL')), branchesId: netI(val('cmbBranch')), projectsId: netI(val('cmbProject')) };
        }
        M.btnsave = function (btn) {   // btnsave_Click :96
            if (!formValidation()) return;
            return busy(btn || 'btnsave', function () {
                return postJson(api + '/save', payload(0)).then(function (d) {
                    if (netI(d && d.id) > 0) box('Saved Successfully');
                    return reset();
                }).catch(function (e) { box(e.message); });
            });
        };
        M.btnupdate = function (btn) {   // btnupdate_Click :139 - Update returns 0, so no message
            if (!formValidation()) return;
            return busy(btn || 'btnupdate', function () {
                return postJson(api + '/save', payload(RecId)).then(function (d) {
                    if (netI(d && d.id) > 0) box('Update Successfully');
                    return reset();
                }).catch(function (e) { box(e.message); });
            });
        };
        M.btnnew = function () { return reset(); };
        wireGrid(function (r) {   // RetrivedData :171
            show('btnsave', false);
            RecId = netI(r.id);
            getJson(api + '/by-id?id=' + RecId).then(function (b) {
                if (!b) return;
                setVal('txtCode', str(col(b, 'OtherItemType'))); setVal('txtItemName', str(col(b, 'OtherItemName')));
                setValue('cmbPurchaseGL', netI(col(b, 'StockGLAcId'))); setValue('cmbCGSGL', netI(col(b, 'CogsGLAcId'))); setValue('cmbSaleGL', netI(col(b, 'SaleGLAcId')));
                setValue('cmbBranch', netI(col(b, 'BranchesId'))); setValue('cmbProject', netI(col(b, 'ProjectsId')));
                refreshCombos();
            }).catch(function (e) { box(e.message); });
        });
        keys = null;   // InvOtherItems has no KeyDown handler
        getJson(api + '/setup').then(function (d) {   // InvOtherItems_Load
            d = d || {};
            show('btnupdate', false);
            if (d.accountsError) box(d.accountsError);
            else if ((d.accounts || []).length) { fill('cmbPurchaseGL', d.accounts, 'Id', 'AccountTitle', false); fill('cmbCGSGL', d.accounts, 'Id', 'AccountTitle', false); fill('cmbSaleGL', d.accounts, 'Id', 'AccountTitle', false); }
            if (d.itemsError) box(d.itemsError); else bindGrid(d.items);
            if (d.branchesError) box(d.branchesError); else if ((d.branches || []).length) fill('cmbBranch', d.branches, 'Id', 'BranchName', false);
            if (d.projectsError) box(d.projectsError); else if ((d.projects || []).length) fill('cmbProject', d.projects, 'Id', 'ProjectName', false);
            refreshCombos();
        }).catch(function (e) { box(e.message); });
    }


    // ============================================================================ 427 PDC Bank (PdcBank.cs)
    function pdcBank() {
        var UpdateMood = false;
        function bindBanks() {   // BindBanks :44 - the procedure's rows (Id hidden, BankName)
            return getJson(api + '/list').then(function (rows) { setGrid((rows || []).map(function (r) { return { Id: col(r, 'Id'), BankName: col(r, 'BankName') }; })); })
                .catch(function (e) { box(e.message); });
        }
        function formRefresh() { RecId = 0; setVal('txtBankName', ''); focus('txtBankName'); show('btnUpdate', false); }
        function inset(btn) {   // Inset() :29
            if (val('txtBankName').trim() === '') { box('Bank Name Field Required'); focus('txtBankName'); return; }
            var was = RecId;
            return busy(btn, function () {
                return postJson(api + '/save', { id: RecId, bankName: val('txtBankName') }).then(function (d) {
                    var n = netI(d && d.id);
                    box(was > 0 ? 'Record Update Successfully...[' + n + ']' : 'Record Save Successfully...[' + n + ']');
                    return bindBanks().then(formRefresh);
                }).catch(function (e) { box(e.message); });
            });
        }
        M.btnsave = function (btn) { RecId = 0; return inset(btn || 'btnsave'); };
        M.btnUpdate = function (btn) { return inset(btn || 'btnUpdate'); };
        M.btnnew = function () { formRefresh(); show('btnUpdate', false); };
        wireGrid(function (r) {   // grdcropyear_DoubleClick :58
            show('btnsave', false); UpdateMood = true; RecId = netI(r.Id);
            getJson(api + '/by-id?id=' + RecId).then(function (b) { setVal('txtBankName', str(col(b, 'BankName'))); }).catch(function (e) { box(e.message); });
        });
        keys = function (e, k) {
            if (e.ctrlKey && k === 'n') { e.preventDefault(); M.btnnew(); }
            if (e.ctrlKey && k === 's' && !UpdateMood) { e.preventDefault(); M.btnsave(); }
            if (e.ctrlKey && k === 'u' && UpdateMood) { e.preventDefault(); M.btnUpdate(); }
        };
        bindBanks().then(function () { show('btnUpdate', false); focus('txtBankName'); });   // DefineCropYear_Load
    }

    // ============================================================================ 429 Document Group / 430 Shipment Documents
    function codeName(cfg) {
        // cfg: { codeCol, nameCol, listMap } - DocumentGroup.cs and ExImShipmentDocuments.cs are the same form
        function bindGrid(rows) { if ((rows || []).length) setGrid(rows.map(cfg.listMap)); }   // only when rows came back
        function reload() { return getJson(api + '/list').then(bindGrid).catch(function (e) { box(e.message); }); }
        function reset() {   // Reset - RecId is NOT cleared on the desktop
            setVal('txtDescription', ''); setVal('txtCode', ''); focus('txtCode');
            $id('btnSave').querySelector('span').textContent = 'Save'; $id('btnAdd').textContent = 'Save';
        }
        function save(btn) {   // btnSave_Click - no validation
            var was = RecId;
            return busy(btn, function () {
                return postJson(api + '/save', { id: RecId, code: val('txtCode').trim(), description: val('txtDescription').trim() }).then(function () {
                    box(was > 0 ? 'Record Update Successfully' : 'Record Save Successfully');
                    reset();
                    return reload();
                }).catch(function (e) { box(e.message); });
            });
        }
        M.btnSave = function (btn) { return save(btn || 'btnSave'); };
        M.btnAdd = function (btn) { return save(btn || 'btnAdd'); };
        M.btnRefreshMenu = function () { reset(); };
        wireGrid(function (r) {   // grdfrm_DoubleClick
            RecId = netI(r.Id);
            getJson(api + '/by-id?id=' + RecId).then(function (b) {
                if (!b) return;
                $id('btnSave').querySelector('span').textContent = 'Update'; $id('btnAdd').textContent = 'Update';
                setVal('txtDescription', str(col(b, cfg.nameCol))); setVal('txtCode', str(col(b, cfg.codeCol)));
            }).catch(function (e) { box(e.message); });
        });
        keys = null;   // no KeyDown handler on either form
        $id('btnSave').querySelector('span').textContent = 'Save'; focus('txtCode');
        getJson(api + '/setup').then(bindGrid).catch(function (e) { box(e.message); });
    }

    // ============================================================================ 428 Tax Lookup (Lookups/TaxLookup.cs)
    function taxLookup() {
        var UpdateMode = false;
        function gridBind(rows) {   // GridBind :60 - Type column shows TaxLookUptypesId
            if ((rows || []).length) setGrid(rows.map(function (r) { return { Id: col(r, 'Id'), Code: col(r, 'Code'), Type: col(r, 'TaxLookUptypesId'), LookUpName: col(r, 'LookUpName') }; }));
        }
        function reload() { return getJson(api + '/list').then(gridBind).catch(function (e) { box(e.message); }); }
        function formReset() {   // FormReset :95
            setVal('txtCode', ''); setVal('txtLookupName', ''); setValue('cmbLookupTypeId', 0); refreshCombos();
            show('btnUpdate', false); UpdateMode = false; RecId = 0;
            return reload().then(function () { focus('cmbLookupTypeId'); });
        }
        function formValidation() {
            if (val('txtCode').trim() === '') { box('Code Field Required'); focus('txtCode'); return false; }
            if (val('txtLookupName').trim() === '') { box('LookUpName Field Required'); focus('txtLookupName'); return false; }
            if (comboText('cmbLookupTypeId').trim() === '') { box('ProfileName Field Required'); focus('cmbLookupTypeId'); return false; }
            return true;
        }
        function payload(id) { return { id: id, code: val('txtCode'), lookUpName: val('txtLookupName'), taxLookUptypesId: netI(val('cmbLookupTypeId')) }; }
        M.btnsave = function (btn) {
            if (!formValidation()) return;
            return busy(btn || 'btnsave', function () {
                return postJson(api + '/save', payload(0)).then(function (d) { if (netI(d && d.id) > 0) { box('Save Successfully'); return formReset(); } })
                    .catch(function (e) { box(e.message); });
            });
        };
        M.btnUpdate = function (btn) {   // Update returns 0 -> no message, no reset (desktop)
            if (!formValidation()) return;
            return busy(btn || 'btnUpdate', function () {
                return postJson(api + '/save', payload(RecId)).then(function (d) { if (netI(d && d.id) > 0) { box('Update Successfully'); return formReset(); } })
                    .catch(function (e) { box(e.message); });
            });
        };
        M.btnnew = function () { return formReset(); };
        wireGrid(function (r) {   // grdlookups_DoubleClick :104
            RecId = netI(r.Id);
            getJson(api + '/by-id?id=' + RecId).then(function (b) {
                setVal('txtCode', str(col(b, 'Code'))); setVal('txtLookupName', str(col(b, 'LookUpName')));
                setValue('cmbLookupTypeId', netI(col(b, 'TaxLookUptypesId'))); refreshCombos();
                show('btnsave', false); UpdateMode = true;
            }).catch(function (e) { box(e.message); });
        });
        /* cmbProfileName_Leave :130 - GenerateCode for the chosen type; code > 0 fills the box */
        $id('cmbLookupTypeId').addEventListener('change', function () {
            getJson(api + '/code?typeId=' + netI(val('cmbLookupTypeId'))).then(function (d) { var c = netI(d && d.code); if (c > 0) setVal('txtCode', String(c)); })
                .catch(function (e) { box(e.message); });
        });
        keys = function (e, k) {   // frmEduLookups_KeyDown - Enter -> Tab
            if (e.key === 'Enter' && !e.ctrlKey && e.target && e.target.tagName !== 'BUTTON') {
                e.preventDefault();
                var f = Array.prototype.filter.call(document.querySelectorAll('input, select, button'), function (x) { return !x.disabled && x.offsetParent !== null; });
                var i = f.indexOf(e.target); if (i >= 0 && i + 1 < f.length) f[i + 1].focus();
            }
            if (e.ctrlKey && k === 's' && !UpdateMode) { e.preventDefault(); M.btnsave(); }
            if (e.ctrlKey && k === 'n') { e.preventDefault(); M.btnnew(); }
            if (e.ctrlKey && k === 'u' && UpdateMode) { e.preventDefault(); M.btnUpdate(); }
        };
        getJson(api + '/setup').then(function (d) {   // frmEduLookups_Load: LookupTypeBind, GridBind
            d = d || {};
            if (d.typesError) box(d.typesError); else if ((d.types || []).length) fill('cmbLookupTypeId', d.types, 'Id', 'LookupTypeName', false);
            refreshCombos();
            if (d.lookupsError) box(d.lookupsError); else gridBind(d.lookups);
        }).catch(function (e) { box(e.message); });
    }

    // ============================================================================ 414 BS & PL Setting (BsPlSettingForm.cs)
    function bsPlSetting() {
        var ROWS = [], NOTES = [], NROWS = [], TAB = 0;
        function reqType() { return $id('radBS').checked ? 'BS' : 'PL'; }
        function classId() { return $id('radAssets').checked ? 2 : ($id('radLiablities').checked ? 3 : 0); }
        M.tab = function (i) {
            TAB = i;
            document.querySelectorAll('.md-tab').forEach(function (t, j) { t.classList.toggle('is-active', j === i); });
            show('tab1', i === 0); show('tab2', i === 1);
        };
        M.radChanged = function () {   // radPL_CheckedChanged :138 - Assets/Liabilities only for BS; BS checks Assets
            var bs = $id('radBS').checked;
            $id('classRadios').style.display = bs ? '' : 'none';
            if (bs) $id('radAssets').checked = true; else { $id('radAssets').checked = false; $id('radLiablities').checked = false; }
        };
        function drawBsPl() {   // grdBsPl: NoteTitle is a value-list column (Id -> NoteTitle); the original text is shown until a note is chosen
            $id('gridBody').innerHTML = ROWS.map(function (r, i) {
                var opts = '<option value="0">' + esc(r.NoteTitle) + '</option>' + NOTES.map(function (n) { return '<option value="' + esc(col(n, 'Id')) + '"' + (r.noteId === netI(col(n, 'Id')) ? ' selected' : '') + '>' + esc(col(n, 'NoteTitle')) + '</option>'; }).join('');
                return '<tr data-i="' + i + '"><td style="display:none;">' + esc(r.PlBsId) + '</td><td style="display:none;">' + esc(r.ChartofAccountId) + '</td><td>' + esc(r.AccountTitle) + '</td>'
                    + '<td><select class="md-cell" data-i="' + i + '">' + opts + '</select></td><td>' + esc(r.NoteRole) + '</td><td style="display:none;">' + esc(r.AccountClassId) + '</td><td>' + esc(r.ClassName) + '</td></tr>';
            }).join('');
            document.querySelector('#grdBsPl thead th[data-col="NoteTitle"]').textContent = reqType() === 'PL' ? 'PLNotes' : 'BSNotes';
        }
        function gridLoad(btn) {   // GridLoad :150
            if ($id('radBS').checked && classId() === 0) { box("Please select either the 'Assets' or 'Liabilities' button..."); focus('radAssets'); return; }
            return busy(btn, function () {
                return getJson(api + '/load?req=' + reqType() + '&accountClassId=' + classId()).then(function (d) {
                    d = d || {};
                    var rows = d.rows || [];
                    if (!rows.length) return;   // table refilled only when rows came back
                    NOTES = d.notes || [];
                    ROWS = rows.map(function (r) { return { PlBsId: col(r, 'PlBsId'), ChartofAccountId: col(r, 'Id'), AccountTitle: col(r, 'AccountTitle'), NoteTitle: col(r, 'NoteTitle'), NoteRole: col(r, 'NoteRole'), AccountClassId: col(r, 'AccountClass'), ClassName: col(r, 'ClassName'), noteId: 0 }; });
                    drawBsPl();
                }).catch(function (e) { box(e.message); });
            });
        }
        M.btnShow = function (btn) { return gridLoad(btn || 'btnShow'); };
        M.btnnew = function () { focus('radPL'); return gridLoad('btnnew'); };
        M.btnUpdate = function (btn) {   // btnUpdate_Click :213
            if (!ask('Are you sure to Update?')) return;
            var rows = ROWS.filter(function (r) { return r.noteId !== 0; }).map(function (r) { return { chartOfAccountId: netI(r.ChartofAccountId), noteId: r.noteId }; });
            return busy(btn || 'btnUpdate', function () {
                var p = rows.length ? postJson(api + '/update', { reqType: reqType(), rows: rows }).then(function () { box('Receord Update Successfully'); }) : Promise.resolve();
                return p.then(function () { return gridLoad(null); }).catch(function (e) { box(e.message); });
            });
        };
        M.BtnShortCutkeys = M.btnSkey = function () {
            var first = TAB === 0 ? 'For Focus on Radio Button PL' : 'For Focus on Radio Button All';
            box('Ctrl+S\tFor Show Grid Data\nCtrl+U\tFor Update\nCtrl+E\tFor Close\nCtrl+N\tFor New\nCtrl+F5\t' + first + '\nCtrl+ArrowUp\t' + first + '\nCtrl+T\tFor Tab Transfer\nCtrl+alt\tTo Show ShortCut Keys Form\nCtrl+ArrowDown\tFor Focus On Grid');
        };
        $id('gridBody').addEventListener('change', function (e) {
            var s = e.target.closest('select.md-cell'); if (!s) return;
            ROWS[+s.getAttribute('data-i')].noteId = netI(s.value);
        });
        // ---- tab 2: Change Note Title
        function noteFilter() { return $id('radBSNote').checked ? 'BS' : ($id('radPLNote').checked ? 'PL' : ''); }
        function drawNotes() {
            $id('gridBodyNote').innerHTML = NROWS.map(function (r, i) {
                return '<tr data-i="' + i + '"><td style="display:none;">' + esc(r.Id) + '</td><td><input class="md-cell" data-i="' + i + '" value="' + esc(r.NoteTitle) + '"/></td><td>' + esc(r.NoteRole) + '</td><td style="display:none;">' + esc(r.AccountClassId) + '</td><td>' + esc(r.ClassName) + '</td></tr>';
            }).join('');
        }
        function gridNoteTitleFill(btn) {   // GridNoteTitleFill :302
            return busy(btn, function () {
                return getJson(api + '/notes?note=' + noteFilter()).then(function (rows) {
                    rows = rows || [];
                    if (!rows.length) return;
                    NROWS = rows.map(function (r) { return { Id: col(r, 'Id'), NoteTitle: str(col(r, 'NoteTitle')), original: str(col(r, 'NoteTitle')), NoteRole: col(r, 'NoteRole'), AccountClassId: col(r, 'AccountClass'), ClassName: col(r, 'ClassName') }; });
                    drawNotes();
                }).catch(function (e) { box(e.message); });
            });
        }
        M.btnShowNote = function (btn) { return gridNoteTitleFill(btn || 'btnShowNote'); };
        M.btnNewNote = function () { focus('radPLNote'); return gridNoteTitleFill('btnNewNote'); };
        M.btnUpdateNote = function (btn) {   // btnUpdateNote_Click :334
            if (!ask('Are you sure to Update?')) return;
            var rows = NROWS.filter(function (r) { return netI(r.Id) !== 0 && r.NoteTitle !== r.original; }).map(function (r) { return { id: netI(r.Id), noteTitle: r.NoteTitle }; });
            return busy(btn || 'btnUpdateNote', function () {
                var p = rows.length ? postJson(api + '/update-notes', { rows: rows }).then(function () { box('Receord Update Successfully'); }) : Promise.resolve();
                return p.then(function () { return gridNoteTitleFill(null); }).catch(function (e) { box(e.message); });
            });
        };
        $id('gridBodyNote').addEventListener('input', function (e) {
            var s = e.target.closest('input.md-cell'); if (!s) return;
            NROWS[+s.getAttribute('data-i')].NoteTitle = s.value;
        });
        keys = function (e, k) {   // DefineCity_KeyDown :60 - Enter -> Tab, Ctrl+S = Show, Ctrl+T = tab transfer
            if (e.key === 'Enter' && !e.ctrlKey && e.target && e.target.tagName !== 'BUTTON' && !e.target.classList.contains('md-cell')) {
                e.preventDefault();
                var f = Array.prototype.filter.call(document.querySelectorAll('input, select, button'), function (x) { return !x.disabled && x.offsetParent !== null; });
                var i = f.indexOf(e.target); if (i >= 0 && i + 1 < f.length) f[i + 1].focus();
            }
            if (e.ctrlKey && k === 'n') { e.preventDefault(); if (TAB === 0) M.btnnew(); else M.btnNewNote(); }
            if (e.ctrlKey && k === 's') { e.preventDefault(); if (TAB === 0) M.btnShow(); else M.btnShowNote(); }
            if (e.ctrlKey && k === 'u') { e.preventDefault(); if (TAB === 0) M.btnUpdate(); else M.btnUpdateNote(); }
            if (e.ctrlKey && k === 't') { e.preventDefault(); M.tab(TAB === 0 ? 1 : 0); }
            if (e.ctrlKey && (e.key === 'F5' || e.key === 'ArrowUp')) { e.preventDefault(); focus(TAB === 0 ? 'radPL' : 'radAllNote'); }
            if (e.ctrlKey && e.key === 'ArrowDown') { e.preventDefault(); var t = document.querySelector(TAB === 0 ? '#gridBody .md-cell' : '#gridBodyNote .md-cell'); if (t) t.focus(); }
            if (e.ctrlKey && e.altKey && !e.repeat && k === 'alt') { e.preventDefault(); M.BtnShortCutkeys(); }
        };
        M.radChanged();   // Load: PL checked, Assets/Liabilities hidden, focus PL
        focus('radPL');
    }

    // ============================================================================ 413 Bank (AcfrmDefineBank.cs)
    function bank() {
        var canSave = true, canUpdate = true;
        var T = ['txtbankname', 'txtbranchcode', 'txtBankAccountno', 'txtAccountTitle', 'txtBankIBANNo', 'txtcontact1', 'txtcontact2', 'txtcellno', 'txtemail', 'txtemailalternate', 'txtotherinfor', 'txtaddress'];
        function bindCombos(d) {   // ChequeTempleteFill / CountryComboFill / CityFill (ZeroIndex true) / CombGlAccountNameFill (BindDDL ZeroIndex true)
            if (d.chequeTemplatesError) box(d.chequeTemplatesError); else if ((d.chequeTemplates || []).length) fill('CmbChequeTemplete', d.chequeTemplates, 'Id', 'type', true);
            if (d.countriesError) box(d.countriesError); else if ((d.countries || []).length) fill('combcountry', d.countries, 'Id', 'Description', true);
            if (d.citiesError) box(d.citiesError); else if ((d.cities || []).length) fill('combcity', d.cities, 'Id', 'CityName', true);
            if (d.glAccountsError) box(d.glAccountsError);
            else if ((d.glAccounts || []).length) fill('combGlaccount', d.glAccounts, 'Id', 'AccountTitle', true);
            else { $id('combGlaccount').innerHTML = ''; }
            refreshCombos();
        }
        function bindGrid(rows) {   // BankDefineGridFill :226 - only when rows came back; Edit button when the user has Update
            if (!(rows || []).length) return;
            GRID = rows.map(function (r) { return { Edit: '', Id: netI(col(r, 'Id')), BranchName: str(col(r, 'BranchName')), BranchCode: str(col(r, 'BranchCode')), BankAccountNo: str(col(r, 'BankAccountNo')), BankAccountTitle: str(col(r, 'BankAccountTitle')),
                BankIBANNo: str(col(r, 'BankIBANNo')), 'Phone No (1)': str(col(r, 'Contact1Tel')), 'Phone No (2)': str(col(r, 'Contact2Tel')), 'Mobile No': str(col(r, 'Contact3Mobile')), EmailAlternate: str(col(r, 'emailAlternate')),
                IsHomeland: str(col(r, 'IsHomeland')), CountryName: str(col(r, 'Country')), CityName: str(col(r, 'City')), Address: str(col(r, 'BranchAddress')), ChequeTemplete: str(col(r, 'ChequeTemplete')), SWIFTCode: str(col(r, 'OtherInfo')),
                ChartOfAccountId: netI(col(r, 'ChartOfAccountId')), AccountCode: str(col(r, 'AccountCode')), AccountTitle: str(col(r, 'AccountTitle')) }; });
            CUR = -1; draw();
            document.querySelector('#GrdBankDefine thead th[data-col="Edit"]').style.display = canUpdate ? '' : 'none';
            $id('gridBody').querySelectorAll('tr').forEach(function (tr) { var td = tr.firstElementChild; td.style.display = canUpdate ? '' : 'none'; td.innerHTML = '<button type="button" class="md-editbtn" data-act="edit">Edit</button>'; });
        }
        function reload() { return getJson(api + '/list').then(bindGrid).catch(function (e) { box(e.message); }); }
        function refresh() {   // BankFromRefresh :120
            RecId = 0; setValue('txtishomeland', 0); T.forEach(function (t) { setVal(t, ''); }); setValue('combcountry', 0); setValue('combGlaccount', 0); refreshCombos();
            show('btnUpdate', false);
            return reload().then(function () { focus('txtishomeland'); });
        }
        function formValidation() {   // FormValidation :95
            var hl = netI(val('txtishomeland'));
            if (comboText('txtishomeland').trim() === '' || hl === 0) { box('Home Land Name Field Required'); focus('txtishomeland'); return false; }
            if (val('txtbankname').trim() === '') { box('Bank Name Field Required'); focus('txtbankname'); return false; }
            if (val('txtbranchcode').trim() === '') { box('Branch Code Field Required'); focus('txtbranchcode'); return false; }
            if (val('txtBankAccountno').trim() === '') { box('Bank Account No Field Required'); focus('txtBankAccountno'); return false; }
            if (val('txtBankIBANNo').trim() === '') { box('BankIBANNo Field Required'); focus('txtBankIBANNo'); return false; }
            if (hl === 2 && netI(val('CmbChequeTemplete')) === 0) { box('Cheque Template Field Required'); focus('CmbChequeTemplete'); return false; }
            if (hl === 2 && netI(val('combGlaccount')) === 0) { box('GL Account Field Required'); focus('combGlaccount'); return false; }
            return true;
        }
        function insert(btn) {   // Insert() :150
            if (!formValidation()) return;
            if (RecId > 0) { if (!ask('Are you sure to Update?')) return; }
            else if (!ask('Are you sure to Save?')) return;
            var was = RecId;
            var body = { id: RecId, isHomelandId: netI(val('txtishomeland')), isHomelandText: comboText('txtishomeland'), bankName: val('txtbankname'), branchCode: val('txtbranchcode'),
                bankAccountNo: val('txtBankAccountno'), bankAccountTitle: val('txtAccountTitle'), bankIbanNo: val('txtBankIBANNo'), contact1Tel: val('txtcontact1'), contact2Tel: val('txtcontact2'),
                contact3Mobile: val('txtcellno'), emailPrimery: val('txtemail'), emailAlternate: val('txtemailalternate'), countryId: netI(val('combcountry')), branchCity: netI(val('combcity')),
                otherInfo: val('txtotherinfor'), chequeTemplate: comboText('CmbChequeTemplete'), chartOfAccountId: netI(val('combGlaccount')), branchAddress: val('txtaddress') };
            return busy(btn, function () {
                return postJson(api + '/save', body).then(function () {
                    box(was > 0 ? 'Record Update Successfully...' : 'Record Save Successfully...');
                    return refresh();
                }).catch(function (e) { box(e.message); });
            });
        }
        function readRow(r) {   // GrdBankDefine_DoubleClick_1 :186
            RecId = netI(r.Id);
            return getJson(api + '/by-id?id=' + RecId).then(function (b) {
                if (!b) return;
                show('btnsave', false);
                setText('txtishomeland', str(col(b, 'IsHomeland')));
                setVal('txtbranchcode', str(col(b, 'BranchCode'))); setVal('txtbankname', str(col(b, 'BranchName')));
                setVal('txtBankAccountno', str(col(b, 'BankAccountNo'))); setVal('txtAccountTitle', str(col(b, 'BankAccountTitle'))); setVal('txtBankIBANNo', str(col(b, 'BankIBANNo')));
                setVal('txtcontact1', str(col(b, 'Contact1Tel'))); setVal('txtcontact2', str(col(b, 'Contact2Tel'))); setVal('txtcellno', str(col(b, 'Contact3Mobile')));
                setVal('txtemail', str(col(b, 'emailPrimery'))); setVal('txtemailalternate', str(col(b, 'emailAlternate')));
                setValue('combcountry', netI(col(b, 'CountryId'))); setValue('combcity', netI(col(b, 'BranchCity')));
                setVal('txtotherinfor', str(col(b, 'OtherInfo'))); setText('CmbChequeTemplete', str(col(b, 'ChequeTemplete')));
                setValue('combGlaccount', netI(col(b, 'ChartOfAccountId'))); setVal('txtaddress', str(col(b, 'BranchAddress')));
                refreshCombos();
            }).catch(function (e) { box(e.message); });
        }
        function setText(id, text) {   // UltraCombo.Text = x
            var s = $id(id), t = String(text === null || text === undefined ? '' : text);
            if (!s) return;
            for (var i = 0; i < s.options.length; i++) if (t !== '' && s.options[i].value !== '0' && s.options[i].textContent === t) { s.selectedIndex = i; return; }
            s.value = '0';
        }
        M.btnsave = function (btn) { RecId = 0; return insert(btn || 'btnsave'); };
        M.btnUpdate = function (btn) { return insert(btn || 'btnUpdate'); };
        M.btnnew = function () { return refresh(); };
        M.btnRefresh = function (btn) { return busy(btn || 'btnRefresh', function () { return getJson(api + '/combos').then(bindCombos).catch(function (e) { box(e.message); }); }); };
        M.btnShortCutKeys = function () {
            box('Ctrl+S\tFor Save\nCtrl+U\tFor Update\nCtrl+N\tFor New\nCtrl+E\tFor Close\nCtrl+R\tFor Refresh\nCtrl+F5\tFor Focus on Is Home Land\nCtrl+Delete\tFor Delete\nCtrl+ArrowDown\tFor Focus On Grid\nCtrl+ArrowUp\tFor Focus on Is Home Land\nCtrl+Enter\tWhen Focus On Grid For Update Record\nCtrl+alt\tTo Show ShortCut Keys Form');
        };
        wireGrid(function (r) { readRow(r); });
        $id('gridBody').addEventListener('click', function (e) { if (e.target.closest('[data-act="edit"]')) { var tr = e.target.closest('tr[data-i]'); readRow(GRID[+tr.getAttribute('data-i')]); } });   // GrdBankDefine_ColumnButtonClick
        keys = function (e, k) {   // AcfrmDefineBank_KeyDown :300 - Enter -> Tab; Save/Update keys need the right and a visible button
            if (e.key === 'Enter' && !e.ctrlKey && e.target && e.target.tagName !== 'BUTTON' && e.target.tagName !== 'TEXTAREA') {
                e.preventDefault();
                var f = Array.prototype.filter.call(document.querySelectorAll('input, select, textarea, button'), function (x) { return !x.disabled && x.offsetParent !== null; });
                var i = f.indexOf(e.target); if (i >= 0 && i + 1 < f.length) f[i + 1].focus();
            }
            if (canSave && e.ctrlKey && k === 's' && visible('btnsave')) { e.preventDefault(); M.btnsave(); }
            if (canUpdate && e.ctrlKey && k === 'u' && visible('btnUpdate')) { e.preventDefault(); M.btnUpdate(); }
            if (e.ctrlKey && k === 'n') { e.preventDefault(); M.btnnew(); }
            if (e.ctrlKey && (e.key === 'F5' || e.key === 'ArrowUp')) { e.preventDefault(); focus('txtishomeland'); }
            if (e.ctrlKey && k === 'r') { e.preventDefault(); M.btnRefresh(); }
            if (e.ctrlKey && e.key === 'ArrowDown') { e.preventDefault(); var t = $id('gridBody').querySelector('tr'); if (t) { t.setAttribute('tabindex', '-1'); t.focus(); } }
            if (e.ctrlKey && e.altKey && !e.repeat && k === 'alt') { e.preventDefault(); M.btnShortCutKeys(); }
            if (canUpdate && e.ctrlKey && e.key === 'Enter') { e.preventDefault(); var c = current(); if (c) readRow(c); }
        };
        getJson(api + '/setup').then(function (d) {   // AcfrmDefineBank_Load :35
            d = d || {};
            canSave = d.canSave !== false; canUpdate = d.canUpdate !== false;
            $id('btnsave').disabled = !canSave; $id('btnUpdate').disabled = !canUpdate;
            fill('txtishomeland', [{ Id: 1, Name: 'Foreign Country' }, { Id: 2, Name: 'Home Country' }], 'Id', 'Name', true);   // CountryTypeFill
            bindCombos(d);
            if (d.banksError) box(d.banksError); else bindGrid(d.banks);
            show('btnUpdate', false);
            $id('txtishomeland').selectedIndex = 2;   // Rows[2].Activate() -> "Home Country"
            refreshCombos();
            focus('txtishomeland');
        }).catch(function (e) { box(e.message); });
    }

    // ------------------------------------------------------------------------------ boot

    function boot() {
        var table = document.querySelector('table.win-grid');
        readCols(table.id);
        M.close = close;
        switch (KEY) {
            case 'country': country(); break;
            case 'province': province(); break;
            case 'district': districtOrTehsil({ combo: 'CmbStateProvince', comboRows: 'provinces', comboText: 'Description', comboValue: 'StateProvinceId', gridText: 'District',
                comboCaptionReq: 'StateProvince Field is Required', name: 'txtDistrictName', nameReq: 'District Name Field is Required', listKey: 'districts', bodyKey: 'stateProvinceId', refreshUrl: null }); break;
            case 'tehsil': districtOrTehsil({ combo: 'CmbDistrict', comboRows: 'districts', comboText: 'District', comboValue: 'DistrictId', gridText: 'Tehsil',
                comboCaptionReq: 'District Field is Required', name: 'txtTehsilName', nameReq: 'Tehsil Name Field is Required', listKey: 'tehsils', bodyKey: 'districtId', refreshUrl: api + '/districts' }); break;
            case 'city': city(); break;
            case 'currency': currency(); break;
            case 'sea-ports': seaPorts(); break;
            case 'date-lock': dateLock(); break;
            case 'other-items': otherItems(); break;
            case 'pdc-bank': pdcBank(); break;
            case 'document-group': codeName({ codeCol: 'ExImDocGroupCode', nameCol: 'ExImDocGroupName', listMap: function (r) { return { Id: col(r, 'Id'), ExImDocGroupCode: col(r, 'ExImDocGroupCode'), ExImDocGroupName: col(r, 'ExImDocGroupName') }; } }); break;
            case 'shipment-documents': codeName({ codeCol: 'exImDocCode', nameCol: 'exImDocName', listMap: function (r) { return { Id: col(r, 'Id'), exImDocCode: col(r, 'exImDocCode'), exImDocName: col(r, 'exImDocName') }; } }); break;
            case 'tax-lookup': taxLookup(); break;
            case 'bs-pl-setting': bsPlSetting(); break;
            case 'bank': bank(); break;
        }
        /* Every form with a KeyDown handler has KeyPreview = true; Ctrl+E / Esc close on all of them. */
        document.addEventListener('keydown', function (e) {
            var k = (e.key || '').toLowerCase();
            if (keys) {
                if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { e.preventDefault(); close(); return; }
                keys(e, k);
            }
        });
    }

    window.MDD = M;
    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', boot);
    else boot();
}());
