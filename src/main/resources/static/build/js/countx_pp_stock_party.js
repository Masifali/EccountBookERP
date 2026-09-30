/* ============================================================================================
 * countx_pp_stock_party.js - 684 Define Stock Party (Party Processing)
 * Architecture.WinApp.PartyProcessing.frmPartyProcessingDefineSupplier. Built on countx_hrm.js (window.HRM).
 * API /api/party-processing/stock-party-define/{setup|list|by-id|by-gl|save|print-check}
 * Prints: pp-293 (grid Print), pp-292_01 (292-Print) through countx_crystal_print.js.
 * ============================================================================================ */
(function () {
    'use strict';
    var API = '/api/party-processing/stock-party-define';
    var NO_REC = 'Record Not Found For Display';
    var P = {};
    window.PpStockParty = P;
    var RecId = 0, rights = {};

    HRM.close = function () {
        window.close();
        setTimeout(function () { if (!window.closed) { if (history.length > 1) history.back(); else location.href = '/party-processing'; } }, 150);
    };

    /** MaskedTextBox: digits only, the mask's literals inserted as the user types (Text keeps the literals). */
    function mask(id) {
        var el = HRM.$(id), m = el.getAttribute('data-mask');
        el.addEventListener('input', function () {
            var digits = el.value.replace(/\D/g, ''), out = '', di = 0;
            for (var x = 0; x < m.length && di < digits.length; x++) {
                if (m.charAt(x) === '0') out += digits.charAt(di++);
                else out += m.charAt(x);
            }
            el.value = out;
        });
    }
    mask('txtpermobile'); mask('txtcnicno');

    function btnCell(text, cls) { return '<button type="button" class="win-btn-action pp-cell-btn ' + cls + '">' + text + '</button>'; }

    var grd = new HRM.Grid('grdfrm', {
        columns: [
            { key: '_print', caption: 'Print', width: 50, render: function () { return btnCell('Print', 'pp-print'); } },
            { key: '_edit', caption: 'Edit', width: 50, render: function () { return btnCell('Edit', 'pp-edit'); } },
            { key: 'Id', hidden: true },
            { key: 'SupplierName', caption: 'SupplierName', width: 220 },
            { key: 'GlAccount', caption: 'GlAccount', width: 200 },
            { key: 'MobilePersonal', caption: 'MobilePersonal', width: 110 },
            { key: 'CountryName', caption: 'CountryName', width: 110 },
            { key: 'CityName', caption: 'CityName', width: 110 },
            { key: 'CNIC', caption: 'CNIC', width: 120 },
            { key: 'Address1', caption: 'Address1', width: 220 }
        ],
        filterRow: true,
        onDouble: function (r) { ReadById(HRM.int(r.Id)); }
    });
    /** grdfrm_ColumnButtonClick: Print -> 293 slip of the row, Edit -> ReadById. */
    HRM.$('grdfrm').addEventListener('click', function (e) {
        var b = e.target.closest('button.pp-cell-btn'); if (!b) return;
        var tr = b.closest('tr[data-i]'); if (!tr) return;
        var r = grd.rows()[+tr.getAttribute('data-i')];
        if (b.classList.contains('pp-print')) print293(b, HRM.int(r.Id));
        else ReadById(HRM.int(r.Id));
    });

    function print293(btn, id) {
        var win = window.CrystalPrint ? CrystalPrint.reserve() : null;
        return HRM.busy(btn, function () {
            return HRM.get(API + '/print-check', { supplierCustomerId: id }).then(function () {
                return CrystalPrint.open('pp-293', { supplierCustomerId: id }, null, win);
            }).catch(function (e) { if (win) CrystalPrint.release(win); HRM.fail(e); });
        }, 'pp-293');
    }

    function datagridviewform() {
        return HRM.get(API + '/list').then(function (rows) { if (rows && rows.length) grd.set(rows); }).catch(HRM.fail);
    }

    function buttons(update) { HRM.show('btnsave', !update); HRM.show('btnupdate', update); }

    function load(o) {
        RecId = HRM.int(o.Id);
        HRM.setCombo('cmbglac', o.GlAccountId);
        HRM.setVal('txtcompany', HRM.str(o.CompanyName));
        HRM.setCombo('cmbcity', o.CityId);
        HRM.setVal('txtpermobile', HRM.str(o.MobilePersonal));
        HRM.setCombo('cmbcountry', o.CountryId);
        HRM.setVal('txtcnicno', HRM.str(o.CNIC));
        HRM.setVal('txtaddress1', HRM.str(o.Address1));
        HRM.check('chkactivestatus', HRM.bool(o.Status));
        buttons(true);
        HRM.focus('txtcompany');
    }

    /** ReadById(ID). */
    function ReadById(id) {
        return HRM.loading(HRM.get(API + '/by-id', { id: id })).then(load).catch(HRM.fail);
    }

    /** cmbglac_Leave: the party (last one) whose GL account is the picked one, loaded for update. */
    HRM.$('cmbglac').addEventListener('change', function () {
        var gl = HRM.comboVal('cmbglac');
        HRM.get(API + '/by-gl', { glAccountId: gl }).then(function (o) { if (o && o.found) load(o); })
            .catch(function () { HRM.box('Id Not Found'); });
    });

    /** formvalidation(): the form's order and wording. */
    function formvalidation() {
        if (HRM.comboVal('cmbglac') === 0) { HRM.box('Please Select GL Account!!!'); HRM.focus('cmbglac'); return false; }
        if (HRM.val('txtcompany').trim() === '') { HRM.box('Please Enter Company Name!!!'); HRM.focus('txtcompany'); return false; }
        if (HRM.comboVal('cmbcountry') === 0) { HRM.box('Please Select Country!!!'); HRM.focus('cmbcountry'); return false; }
        if (HRM.val('txtpermobile').trim() === '') { HRM.box('Please Enter Phone Number'); HRM.focus('txtpermobile'); return false; }
        if (HRM.comboVal('cmbcity') === 0) { HRM.box('Please Select City!!!'); HRM.focus('cmbcity'); return false; }
        if (HRM.visible('btnsave') && !HRM.$('btnsave').disabled && !HRM.checked('chkactivestatus')) {
            HRM.box('Please check the Active Status!!!'); HRM.focus('chkactivestatus'); return false;
        }
        return true;
    }

    function Insert(btn) {
        if (!formvalidation()) return;
        if (!HRM.ask(RecId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        return HRM.busy(btn, function () {
            return HRM.post(API + '/save', {
                id: RecId, glAccountId: HRM.comboVal('cmbglac'), companyName: HRM.val('txtcompany'), cnic: HRM.val('txtcnicno'),
                address: HRM.val('txtaddress1'), status: HRM.checked('chkactivestatus'), countryId: HRM.comboVal('cmbcountry'),
                cityId: HRM.comboVal('cmbcity'), mobile: HRM.val('txtpermobile')
            }).then(function (d) { HRM.box(d.message); return Reset(); }).catch(HRM.fail);
        }, 'pp-stock-party-save');
    }

    /** Reset(): fields and combos cleared (Is Active keeps its tick), Save shown, RecId 0, grid rebound. */
    function Reset() {
        ['txtaddress1', 'txtcnicno', 'txtcompany', 'txtpermobile'].forEach(function (id) { HRM.setVal(id, ''); });
        ['cmbglac', 'cmbcity', 'cmbcountry'].forEach(function (id) { HRM.setCombo(id, 0); });
        buttons(false);
        RecId = 0;
        HRM.focus('txtcompany');
        return datagridviewform();
    }

    P.btnsave = function (btn) { RecId = 0; return Insert(btn || 'btnsave'); };
    P.btnupdate = function (btn) { return Insert(btn || 'btnupdate'); };
    P.btnnew = function () { Reset(); };
    P.browse = function () { HRM.box('Picture upload is not available on the web page yet.'); };

    /** btnPrint_Click: SupplierCustomerRegister (City / Country when set, group 12) -> 292_01. */
    P.btnPrint = function (btn) {
        var args = { cityId: HRM.comboVal('cmbcity'), countryId: HRM.comboVal('cmbcountry') };
        var win = window.CrystalPrint ? CrystalPrint.reserve() : null;
        return HRM.busy(btn || 'btnPrint', function () {
            return HRM.get(API + '/print-check', { cityId: args.cityId, countryId: args.countryId, register: true }).then(function () {
                return CrystalPrint.open('pp-292_01', args, null, win);
            }).catch(function (e) { if (win) CrystalPrint.release(win); HRM.fail(e); });
        });
    };

    /** MakeShortCutKeys(). */
    P.shortCuts = function () {
        var rows = [['Ctrl+S', 'For Save'], ['Ctrl+U', 'For Update'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'],
            ['Ctrl+P', 'For Print'], ['Ctrl+F5', 'For Focus on Comp Name'], ['Ctrl+T', 'For Tab Transfer'], ['Ctrl+alt', 'To Show ShortCut Keys Form'],
            ['Ctrl+ArrowDown', 'For Focus On Grid'], ['Ctrl+ArrowUp', 'For Focus on Comp Name'], ['Ctrl+Enter', 'When Focus On Any Grid For Update Record']];
        HRM.modal({ title: 'ShortCut Keys', width: 'min(520px, 96vw)', html: '<table class="win-grid hrm-grid"><thead><tr><th>KeyCombination</th><th>Description</th></tr></thead><tbody>' +
            rows.map(function (r) { return '<tr><td>' + HRM.esc(r[0]) + '</td><td>' + HRM.esc(r[1]) + '</td></tr>'; }).join('') + '</tbody></table>' });
    };

    HRM.keys({                                                  // supfrmDefineSupplier1_KeyDown (Enter -> {TAB})
        'ctrl+s': function () { if (HRM.visible('btnsave') && !HRM.$('btnsave').disabled) P.btnsave(); },
        'ctrl+n': function () { P.btnnew(); },
        'ctrl+e': HRM.close, 'esc': HRM.close,
        'ctrl+u': function () { if (HRM.visible('btnupdate') && !HRM.$('btnupdate').disabled) P.btnupdate(); },
        'ctrl+p': function () { if (!HRM.$('btnPrint').disabled) P.btnPrint(); },
        'ctrl+f5': function () { HRM.focus('txtcompany'); },
        'ctrl+arrowup': function () { HRM.focus('txtcompany'); },
        'ctrl+arrowdown': function () { var t = HRM.$('grdfrm').querySelector('tbody tr[data-i]'); if (t) t.focus(); },
        'ctrl+enter': function () { var r = grd.current(); if (r) ReadById(HRM.int(r.Id)); },
        'ctrl+alt+control': P.shortCuts, 'ctrl+alt+alt': P.shortCuts
    });
    HRM.footer(function (b) {
        return HRM.busy(b, function () {
            return datagridviewform().then(function () { var x = document.getElementById('historyBox'); if (x) x.scrollIntoView({ block: 'start', behavior: 'smooth' }); });
        });
    });

    /** supfrmDefineSupplier1_Load. */
    HRM.loading(HRM.get(API + '/setup')).then(function (d) {
        rights = d.rights || {};
        HRM.enable('btnsave', rights.save !== false);
        HRM.enable('btnPrint', rights.print !== false);
        HRM.enable('btnupdate', rights.update !== false);
        HRM.fill('cmbglac', d.glAccounts || [], 'Id', 'AccountTitle', { zero: '' });
        HRM.fill('cmbcountry', d.countries || [], 'Id', 'Description', { zero: '' });
        HRM.fill('cmbcity', d.cities || [], 'Id', 'CityName', { zero: '' });
        if (d.rows && d.rows.length) grd.set(d.rows);
        buttons(false);
        HRM.focus('txtcompany');
    }).catch(HRM.fail);
})();
