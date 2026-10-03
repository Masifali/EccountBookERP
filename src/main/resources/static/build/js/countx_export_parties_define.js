/* ============================================================================================
 * countx_export_parties_define.js - ExportPartyDefines.cs (Architecture.WinApp.Export), screen 206
 * "Export Parties Define". Tabs "Define Party" | "Define Consignee/Notify"; every button, Leave, grid
 * double-click, grid button (Select / Update / Print), multi-row Update and KeyDown shortcut of the desktop
 * form has its counterpart here with the desktop's texts; data from /api/export/parties-define
 * (ExportPartyDefinesController -> ExportPartyDefinesService -> the desktop's own procedures).
 *
 * Desktop quirks kept: Status is saved from the checkbox CAPTION (chkactivestatus.Text " " / ChkConsigneeStatus
 * .Text "Active" -> false; "True"/"False" after a ReadById) - the page tracks that caption as STATUS_TEXT;
 * ReadByIdConsignee loads GetByID(RecId) of the PARTY tab; cmbglac_Leave looks the account up in the
 * non-export party list.
 * ============================================================================================ */
(function (global) {
    'use strict';

    var F = global.ExportF, $id = F.$id, box = F.box, ask = F.ask, str = F.str, netI = F.netI, val = F.val, setText = F.setText,
        setVal = F.setVal, bind = F.bind, busy = F.busy, getJson = F.getJson, postJson = F.postJson, show = F.show, col = F.col, esc = F.esc, focus = F.focus;
    var API = '/api/export/parties-define';

    var PERM = { Save: true, Update: true, Print: true };
    var GL = [], PARENTS = [], COUNTRIES = [], PROVINCES = [], CITIES = [];
    var PARTY = { recId: 0, statusText: ' ', pictureUrl: '', rows: [], cur: -1 };
    var CONS = { recId: 0, statusText: 'Active', rows: [], cur: -1 };

    // ------------------------------------------------------------------------------ combos

    function bindGl() { bind('cmbglac', GL, 'Id', 'AccountTitle', ['AccountCode']); bind('CmbAdvanceAc', GL, 'Id', 'AccountTitle', ['AccountCode']); }
    function bindParents() { bind('CmbParentAcConsignee', PARENTS, 'Id', 'CompanyName', ['PartyCode']); }
    function bindGeo() {
        bind('cmbcountry', COUNTRIES, 'Id', 'Name', []); bind('CmbCountryNameConsignee', COUNTRIES, 'Id', 'Name', []);
        bind('cmbprovince', PROVINCES, 'Id', 'Name', []);
        bind('cmbcity', CITIES, 'Id', 'Name', []); bind('CmbCityNameConsignee', CITIES, 'Id', 'Name', []);
    }
    function options(list, v) {
        var h = '<option value="0"></option>';
        list.forEach(function (r) { h += '<option value="' + esc(r.Id) + '"' + (netI(r.Id) === netI(v) ? ' selected' : '') + '>' + esc(r.Name) + '</option>'; });
        return h;
    }
    function applyData(d) {
        ['glAccounts', 'parentAccounts', 'countries', 'provinces', 'cities', 'history', 'consigneeHistory'].forEach(function (k) { if (d[k + 'Error']) box(d[k + 'Error']); });
        GL = d.glAccounts || []; bindGl();
        PARENTS = d.parentAccounts || []; bindParents();
        COUNTRIES = d.countries || []; PROVINCES = d.provinces || []; CITIES = d.cities || []; bindGeo();
        PARTY.rows = d.history || []; PARTY.cur = -1; partyRender();
        CONS.rows = d.consigneeHistory || []; CONS.cur = -1; consRender();
    }

    // ------------------------------------------------------------------------------ load

    /** supfrmDefineSupplier1_Load. */
    function load() {
        return getJson(API + '/setup').then(function (d) {
            d = d || {};
            if (d.permissions) PERM = { Save: d.permissions.Save !== false, Update: d.permissions.Update !== false, Print: d.permissions.Print !== false };
            $id('btnsave').disabled = !PERM.Save; $id('btnSaveConsignee').disabled = !PERM.Save;
            $id('btnupdate').disabled = !PERM.Update; $id('btnUpdateConsignee').disabled = !PERM.Update;
            $id('btnPrint').disabled = !PERM.Print; $id('btnPrintConsignee').disabled = !PERM.Print;
            show('btnUpdateRecordsFromGrid', PERM.Update); show('btnMultiRecordsUpdateConsignee', PERM.Update);
            applyData(d);
            $id('partyFooterInfo').textContent = 'ExportPartyDefines  -  CustomerGroupId 7';
            $id('consFooterInfo').textContent = 'Consignee / Notify  -  sub parties of an export party';
            setText('txtcnicexp', F.today());
            focus('cmbglac');
        }).catch(function (e) { box(e.message || 'Error occurred during database call.'); });
    }
    /** btnRefresh_Click: global party list, every combo, both grids. */
    function refresh(btn) { return busy(btn, function () { return getJson(API + '/refresh').then(applyData).catch(function (e) { box(e.message); }); }); }
    /** btnRefreshConsignee_Click: parents, countries, cities, consignee history (the same read; the party grid is kept as is). */
    function refreshConsignee(btn) {
        return busy(btn, function () {
            return getJson(API + '/refresh').then(function (d) {
                PARENTS = d.parentAccounts || []; bindParents();
                COUNTRIES = d.countries || []; CITIES = d.cities || []; bindGeo();
                CONS.rows = d.consigneeHistory || []; CONS.cur = -1; consRender();
            }).catch(function (e) { box(e.message); });
        });
    }

    // ============================================================================== Define Party

    /** Reset(). */
    function partyNew() {
        ['txtaddress1', 'txtcnicno', 'txtcompany', 'txtemail', 'txtfirstname', 'txtlastname', 'txtntnno', 'txtofficemobile', 'txtpermobile', 'txtphone',
            'txtstrnno', 'txttitle', 'txttown', 'txtwebsite', 'txtzipcode'].forEach(function (id) { setText(id, ''); });
        ['cmbglac', 'cmbcity', 'cmbcountry', 'CmbAdvanceAc', 'cmbprovince'].forEach(function (id) { setVal(id, '0'); });
        show('btnsave', true); show('btnupdate', false); show('btnAdd', true);
        PARTY.recId = 0; PARTY.pictureUrl = ''; $id('boxName').textContent = '';
        /* the caption stays whatever it was (" " on a fresh form, "True"/"False" after a ReadById) - as on the desktop */
        focus('cmbglac');
        return getJson(API + '/gl-accounts?recId=0').then(function (rows) { GL = rows || []; bindGl(); }).catch(function (e) { box(e.message); });
    }
    /** formvalidation(). */
    function partyValidation() {
        if (!F.hasSel('cmbglac')) { box('GL Account Field is Required!!!'); focus('cmbglac'); return false; }
        if (!val('txtcompany').trim()) { box('Company Name Field is Required!!!'); focus('txtcompany'); return false; }
        if (!val('txtfirstname').trim()) { box('Person Name Field is Required!!!'); focus('txtfirstname'); return false; }
        if (!F.checked('chkactivestatus') && PARTY.recId === 0) { box('Please check the Active Status!!!'); focus('chkactivestatus'); return false; }
        return true;
    }
    function partyBody() {
        return {
            recId: PARTY.recId, glAccountId: netI(val('cmbglac')), glAccountText: F.selText('cmbglac'), companyName: val('txtcompany').trim(),
            ntnNo: val('txtntnno').trim(), strnNo: val('txtstrnno').trim(), title: val('txttitle').trim(), firstName: val('txtfirstname').trim(),
            lastName: val('txtlastname').trim(), cnic: val('txtcnicno').trim(), cnicExpiry: val('txtcnicexp'), address1: val('txtaddress1').trim(),
            activeChecked: F.checked('chkactivestatus'), statusText: PARTY.statusText, countryId: netI(val('cmbcountry')), cityId: netI(val('cmbcity')),
            town: val('txttown').trim(), zipCode: val('txtzipcode').trim(), phone: val('txtphone').trim(), mobileOffice: val('txtofficemobile').trim(),
            mobilePersonal: val('txtpermobile').trim(), email: val('txtemail').trim(), webPage: val('txtwebsite').trim(),
            advanceGlAcId: netI(val('CmbAdvanceAc')), stateProvinceId: netI(val('cmbprovince')), pictureUrl: PARTY.pictureUrl
        };
    }
    /** Insert() - btnsave (RecId = 0 first), btnAdd (= btnsave_Click), btnupdate. */
    function partyInsert(btn) {
        return busy(btn, function () {
            if (!partyValidation()) return Promise.resolve();
            if (!ask(PARTY.recId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return Promise.resolve();
            return postJson(API + '/save', partyBody()).then(function (d) {
                box(d && d.message ? d.message : 'Data Save Successfully.... ');
                return getJson(API + '/refresh').then(function (r) { PARTY.rows = r.history || []; PARTY.cur = -1; partyRender(); CONS.rows = r.consigneeHistory || []; consRender(); PARENTS = r.parentAccounts || []; bindParents(); })
                    .then(function () { return partyNew(); });
            }).catch(function (e) { box(e.message); });
        });
    }
    function partySave(btn) { PARTY.recId = 0; return partyInsert(btn); }
    function partyUpdate(btn) { return partyInsert(btn); }

    /** ReadById(ID). */
    function partyReadById(id) {
        return getJson(API + '/by-id?id=' + encodeURIComponent(id)).then(function (d) {
            var r = d.record || {};
            show('btnsave', false); show('btnupdate', true); show('btnAdd', false);
            PARTY.recId = netI(r.Id);
            setText('txtcompany', r.CompanyName);
            GL = d.glAccounts || GL; bindGl();
            setVal('cmbglac', r.GlAccountId);
            setText('txtntnno', r.NTN_No); setText('txtstrnno', r.STRN_No); setText('txttitle', r.Title);
            setText('txtfirstname', r.FirstName); setText('txtlastname', r.LastName); setText('txtcnicno', r.CNIC);
            if (r.CNIC_EXPIRY_DATE) setText('txtcnicexp', r.CNIC_EXPIRY_DATE);
            setText('txtaddress1', r.Address1);
            PARTY.statusText = str(r.StatusText);                 /* chkactivestatus.Text = Conversion.ToString(Status) */
            setVal('cmbcountry', r.CountryId); setVal('cmbcity', r.CityId);
            setText('txttown', r.Town); setText('txtzipcode', r.ZipCode); setText('txtphone', r.Phone);
            setText('txtofficemobile', r.MobileOffice); setText('txtpermobile', r.MobilePersonal);
            setText('txtemail', r.Email); setText('txtwebsite', r.WebPage);
            setVal('CmbAdvanceAc', r.AdvanceGlAcId); setVal('cmbprovince', r.StateProvinceId);
            PARTY.pictureUrl = str(r.PictureURL);
            $id('boxName').textContent = PARTY.pictureUrl ? PARTY.pictureUrl.replace(/^.*[\\\/]/, '') : '';
            PARTY.rows = d.history || PARTY.rows; PARTY.cur = -1; partyRender();
        }).catch(function (e) { box(e.message); });
    }
    /** cmbglac_Leave: a party on that GL account (non-export list) is loaded; then focus Company. */
    function glLeave() {
        var gl = netI(val('cmbglac'));
        return getJson(API + '/gl-leave?glAccountId=' + gl).then(function (d) {
            if (d && netI(d.id) > 0) { PARTY.recId = netI(d.id); return partyReadById(PARTY.recId); }
        }).catch(function () { box('Id Not Found'); }).then(function () { focus('txtcompany'); });
    }

    var PARTY_COLS = [
        { key: 'GlAccount' }, { key: 'CompanyName', edit: 'text' }, { key: 'PersonName', edit: 'text' }, { key: 'Mobile', edit: 'text' },
        { key: 'Country', edit: 'select', options: function (v) { return options(COUNTRIES, v); } },
        { key: 'Province', edit: 'select', options: function (v) { return options(PROVINCES, v); } },
        { key: 'City', edit: 'select', options: function (v) { return options(CITIES, v); } },
        { key: 'Email', edit: 'text' }, { key: 'EORI', edit: 'text' }, { key: 'VATNo', edit: 'text' }, { key: 'ZipCode', edit: 'text' },
        { key: 'WebPage', edit: 'text' }, { key: 'Address', edit: 'text' }, { key: 'ParentAccountTitle' }, { key: 'Status' },
        { key: 'EntryDate', fmt: 'datetime' }, { key: 'EntryUser' }, { key: 'ModifyDate', fmt: 'datetime' }, { key: 'ModifyUser' }];
    function leadCells(i, r) {
        return '<td class="ctr"><input type="checkbox" class="exf-row-chk" data-i="' + i + '"' + (r._checked ? ' checked' : '') + (PERM.Update ? '' : ' disabled') + '/></td>' +
            '<td class="win-cell-btn"><button type="button" class="win-edit" data-btn="Update" data-i="' + i + '"' + (PERM.Update ? '' : ' disabled') + '>Update</button></td>' +
            '<td class="win-cell-btn"><button type="button" class="win-edit" data-btn="Print" data-i="' + i + '"' + (PERM.Print ? '' : ' disabled') + '>Print</button></td>';
    }
    function partyRender() {
        F.drawGrid('partyBody', null, PARTY.rows, PARTY_COLS, PARTY.cur, function (r, i) { return leadCells(i, r); });
        show('partyEmpty', PARTY.rows.length === 0);
    }
    /** grdfrm ColumnButtonClick "Update" -> RecordUpdate(item). */
    function partyRowUpdate(i, btn) {
        var r = PARTY.rows[i]; if (!r) return;
        return busy(btn, function () {
            return postJson(API + '/update-row', r).then(function (d) {
                box((d && d.message) || 'Record Updated Successfully!');
                return getJson(API + '/refresh').then(function (x) { PARTY.rows = x.history || []; PARTY.cur = -1; partyRender(); });
            }).catch(function (e) { box(e.message); });
        });
    }
    /** btnUpdateRecordsFromGrid_Click. */
    function partyUpdateChecked(btn) {
        return busy(btn, function () {
            var rows = PARTY.rows.filter(function (r) { return r._checked; });
            if (!rows.length) { box('Please Select Rows first to update multi rows'); return Promise.resolve(); }
            if (!ask('Are you sure to Update the Records?')) return Promise.resolve();
            return postJson(API + '/update-rows', { rows: rows }).then(function (d) {
                box((d && d.message) || "Record's Updated Successfully!");
                return getJson(API + '/refresh').then(function (x) { PARTY.rows = x.history || []; PARTY.cur = -1; partyRender(); });
            }).catch(function (e) { box(e.message); });
        });
    }
    /** CommonServices.InvRptSupplierSlip_293(Id) - seeded contract 293-invrptsupplierslip. */
    function printRow(id) { F.printSeeded('293-invrptsupplierslip', { id: id }); }
    /** btnPrint_Click -> CommonServices.SupplierResgister_292(0): Sp_SupplierCustomerHistory_rpt @GroupId 7, 292-InvRptSupplierRegister.rpt. */
    function print292(btn) { if (!PERM.Print) return; F.printSeeded('292-invrptsupplierregister', {}); }
    /** btnPrintConsignee_Click -> CommonServices.ConsigneeRegister_292_01(): @GroupId 7, @ActionId 1, 292_01-InvRptSupplierRegister.rpt. */
    function print292_01(btn) { if (!PERM.Print) return; F.printSeeded('292-01-invrptsupplierregister', {}); }

    // ============================================================================== Define Consignee/Notify

    /** ResetConsigneeFields(). */
    function consigneeNew() {
        setVal('CmbParentAcConsignee', '0'); setText('txtConsigneeName', '');
        setVal('CmbCountryNameConsignee', '0'); setVal('CmbCityNameConsignee', '0');
        ['txtStrnNoConsignee', 'txtNtnNoConsignee', 'txtEmailConsignee', 'txtWebPageConsignee', 'txtZipCodeConsignee', 'txtAddressConsignee'].forEach(function (id) { setText(id, ''); });
        show('btnSaveConsignee', true); show('btnUpdateConsignee', false);
        CONS.recId = 0;
        focus('CmbParentAcConsignee');
    }
    /** formValidationConsignee() - note the desktop focuses txtcompany (party tab) on a missing name. */
    function consigneeValidation() {
        if (!F.hasSel('CmbParentAcConsignee')) { box('Parent Account Field is Required!!!'); focus('CmbParentAcConsignee'); return false; }
        if (!val('txtConsigneeName').trim()) { box('Consignee Name Field is Required!!!'); focus('txtConsigneeName'); return false; }
        if (!F.checked('ChkConsigneeStatus') && CONS.recId === 0) { box('Please check the Active Status!!!'); focus('ChkConsigneeStatus'); return false; }
        return true;
    }
    /** InsertConsignee(). */
    function consigneeInsert(btn) {
        return busy(btn, function () {
            if (!consigneeValidation()) return Promise.resolve();
            if (!ask(CONS.recId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return Promise.resolve();
            return postJson(API + '/consignee/save', {
                recIdConsignee: CONS.recId, parentId: netI(val('CmbParentAcConsignee')), parentText: F.selText('CmbParentAcConsignee'),
                consigneeName: val('txtConsigneeName').trim(), countryId: netI(val('CmbCountryNameConsignee')), cityId: netI(val('CmbCityNameConsignee')),
                strnNo: val('txtStrnNoConsignee').trim(), ntnNo: val('txtNtnNoConsignee').trim(), email: val('txtEmailConsignee').trim(),
                webPage: val('txtWebPageConsignee').trim(), zipCode: val('txtZipCodeConsignee').trim(), address1: val('txtAddressConsignee').trim(),
                activeChecked: F.checked('ChkConsigneeStatus'), statusText: CONS.statusText
            }).then(function (d) {
                box(d && d.message ? d.message : 'Data Save Successfully.... ');
                return getJson(API + '/refresh').then(function (r) { CONS.rows = r.consigneeHistory || []; CONS.cur = -1; consRender(); PARENTS = r.parentAccounts || []; bindParents(); })
                    .then(consigneeNew);
            }).catch(function (e) { box(e.message); });
        });
    }
    function consigneeSave(btn) { CONS.recId = 0; return consigneeInsert(btn); }
    function consigneeUpdate(btn) { return consigneeInsert(btn); }
    /** ReadByIdConsignee(ID): fields from GetByID(RecId) - the party tab's record (desktop bug kept); cmbcity / txtzipcode of the PARTY tab are set. */
    function consigneeReadById(id) {
        return getJson(API + '/consignee/by-id?id=' + encodeURIComponent(id) + '&mainRecId=' + PARTY.recId).then(function (d) {
            show('btnSaveConsignee', false); show('btnUpdateConsignee', true);
            CONS.recId = netI(d.recIdConsignee);
            var r = d.record || {};
            setVal('CmbParentAcConsignee', r.ParentsSupCustId);
            setText('txtConsigneeName', r.CompanyName);
            setVal('CmbCountryNameConsignee', r.CountryId);
            setVal('cmbcity', r.CityId);                      /* desktop writes the party tab's city combo */
            setText('txtStrnNoConsignee', r.STRN_No); setText('txtNtnNoConsignee', r.NTN_No);
            setText('txtzipcode', r.ZipCode);                 /* and the party tab's postal code */
            setText('txtEmailConsignee', r.Email); setText('txtWebPageConsignee', r.WebPage); setText('txtAddressConsignee', r.Address1);
            CONS.statusText = str(r.StatusText);
        }).catch(function (e) { show('btnSaveConsignee', false); show('btnUpdateConsignee', true); CONS.recId = netI(id); box(e.message); });
    }
    var CONS_COLS = [
        { key: 'ParentAccountTitle' }, { key: 'GlAccount' }, { key: 'CompanyName', edit: 'text' },
        { key: 'Country', edit: 'select', options: function (v) { return options(COUNTRIES, v); } },
        { key: 'City', edit: 'select', options: function (v) { return options(CITIES, v); } },
        { key: 'EORI', edit: 'text' }, { key: 'VATNo', edit: 'text' }, { key: 'Email', edit: 'text' }, { key: 'WebPage', edit: 'text' },
        { key: 'ZipCode', edit: 'text' }, { key: 'Address', edit: 'text' }, { key: 'Status' },
        { key: 'EntryDate', fmt: 'datetime' }, { key: 'EntryUser' }, { key: 'ModifyDate', fmt: 'datetime' }, { key: 'ModifyUser' }];
    function consLead(i, r) {
        return '<td class="ctr"><input type="checkbox" class="exf-row-chk" data-i="' + i + '"' + (r._checked ? ' checked' : '') + '/></td>' +
            '<td class="win-cell-btn"><button type="button" class="win-edit" data-btn="Print" data-i="' + i + '"' + (PERM.Print ? '' : ' disabled') + '>Print</button></td>' +
            '<td class="win-cell-btn"><button type="button" class="win-edit" data-btn="Update" data-i="' + i + '"' + (PERM.Update ? '' : ' disabled') + '>Update</button></td>';
    }
    function consRender() {
        F.drawGrid('consBody', null, CONS.rows, CONS_COLS, CONS.cur, function (r, i) { return consLead(i, r); });
        show('consEmpty', CONS.rows.length === 0);
    }
    function consRowUpdate(i, btn) {
        var r = CONS.rows[i]; if (!r) return;
        return busy(btn, function () {
            return postJson(API + '/consignee/update-row', r).then(function (d) {
                box((d && d.message) || 'Record Updated Successfully!');
                return getJson(API + '/refresh').then(function (x) { CONS.rows = x.consigneeHistory || []; CONS.cur = -1; consRender(); });
            }).catch(function (e) { box(e.message); });
        });
    }
    function consigneeUpdateChecked(btn) {
        return busy(btn, function () {
            var rows = CONS.rows.filter(function (r) { return r._checked; });
            if (!rows.length) { box('Please Select Rows first to update multi rows'); return Promise.resolve(); }
            if (!ask('Are you sure to Update the Records?')) return Promise.resolve();
            return postJson(API + '/consignee/update-rows', { rows: rows }).then(function (d) {
                box((d && d.message) || "Record's Updated Successfully!");
                return getJson(API + '/refresh').then(function (x) { CONS.rows = x.consigneeHistory || []; CONS.cur = -1; consRender(); });
            }).catch(function (e) { box(e.message); });
        });
    }

    // ============================================================================== popups / shortcuts

    /** DefineCountry / DefineProvince / DefineCity (Master Data Definition pages) open in a new window; combos re-read on return. */
    function define(which) {
        var url = which === 'country' ? '/master-data/define-country' : which === 'province' ? '/master-data/define-province' : '/master-data/define-city';
        var w = global.open(url, '_blank');
        var t = setInterval(function () { if (!w || w.closed) { clearInterval(t); getJson(API + '/refresh').then(function (d) { COUNTRIES = d.countries || []; PROVINCES = d.provinces || []; CITIES = d.cities || []; bindGeo(); }).catch(function () { /* ignore */ }); } }, 800);
    }
    /** SupfrmBankDetail / SupfrmShipToAddress / SupfrmSupplierLimits are Inventory popups not in this port's scope. */
    function popup(name) { box(name + ' is a separate Inventory screen (not part of the Export port).'); }
    function shortcuts(which) {
        if (which === 'party') {
            F.shortcutPopup([['Ctrl+S', 'For Save'], ['Ctrl+U', 'For Update'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Ctrl+P', 'For Print'],
                ['Ctrl+F5', 'For Focus on GL Account'], ['Ctrl+F1', 'For Show Bank Detail'], ['Ctrl+F2', 'For Show Ship To Address'], ['Ctrl+F3', 'For Show Supplier Limits'],
                ['Ctrl+T', 'For Tab Transfer'], ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowDown', 'For Focus On Grid'],
                ['Ctrl+ArrowUp', 'For Focus On GL Account in Export Party Define'], ['Ctrl+Enter', 'When Focus On Any Grid For Update Record'],
                ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]]);
        } else {
            F.shortcutPopup([['Ctrl+S', 'For Save'], ['Ctrl+U', 'For Update'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Ctrl+P', 'For Print'],
                ['Ctrl+F5', 'For Focus on Parent Account'], ['Ctrl+T', 'For Tab Transfer'], ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowDown', 'For Focus On Grid'],
                ['Ctrl+ArrowUp', 'For Focus On Parent Account'], ['Ctrl+Enter', 'When Focus On Any Grid For Update Record'],
                ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]]);
        }
    }
    /** browse_Click: the desktop copies a local image into the configured "Attachment Folder Path"; the web keeps the stored file name only. */
    function browse() { box('Please Map the Path in Configration'); }
    function scrollHistory(boxId) { var b = $id(boxId); if (b) b.scrollIntoView({ behavior: 'smooth' }); }
    function tab(id) { F.innerTab('main', id, null, function (p) { focus(p === 'tabPage1' ? 'cmbglac' : 'CmbParentAcConsignee'); }); }

    // ============================================================================== wiring

    document.addEventListener('DOMContentLoaded', function () {
        F.wireTabs(function (p) { focus(p === 'tabPage1' ? 'cmbglac' : 'CmbParentAcConsignee'); });
        F.on('cmbglac', 'change', glLeave);
        F.on('cmbcountry', 'change', function () { F.setEnabled('cmbcity', true); });     /* cmbcountry_Leave */
        F.on('chkactivestatus', 'blur', function () { focus('cmbglac'); });             /* chkactivestatus_Leave */
        $id('partyBody').addEventListener('change', function (e) {
            var c = e.target.closest('.exf-row-chk'); if (c) { var r = PARTY.rows[+c.getAttribute('data-i')]; if (r) r._checked = c.checked; }
        });
        $id('consBody').addEventListener('change', function (e) {
            var c = e.target.closest('.exf-row-chk'); if (c) { var r = CONS.rows[+c.getAttribute('data-i')]; if (r) r._checked = c.checked; }
        });
        F.wireGrid('partyBody', {
            select: function (i) { PARTY.cur = i; },
            open: function (i) { var r = PARTY.rows[i]; if (r) partyReadById(r.Id); },
            btn: function (name, i) { if (name === 'Update') partyRowUpdate(i, $id('partyBody').querySelector('button[data-btn="Update"][data-i="' + i + '"]')); else if (name === 'Print') printRow(PARTY.rows[i].Id); },
            cell: function (i, k, v) { var r = PARTY.rows[i]; if (r) r[k] = /^(Country|Province|City)$/.test(k) ? netI(v) : v; }   /* grdfrm_CellUpdated -> UpdateData */
        });
        F.wireGrid('consBody', {
            select: function (i) { CONS.cur = i; },
            open: function (i) { var r = CONS.rows[i]; if (r) consigneeReadById(r.Id); },
            btn: function (name, i) { if (name === 'Update') consRowUpdate(i, $id('consBody').querySelector('button[data-btn="Update"][data-i="' + i + '"]')); else if (name === 'Print') printRow(CONS.rows[i].Id); },
            cell: function (i, k, v) { var r = CONS.rows[i]; if (r) r[k] = /^(Country|City)$/.test(k) ? netI(v) : v; }
        });
        /* supfrmDefineSupplier1_KeyDown */
        document.addEventListener('keydown', function (e) {
            if (F.enterMoves(e)) return;
            var k = (e.key || '').toLowerCase();
            if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { e.preventDefault(); F.cancel(); return; }
            var party = F.activeInner('main') !== 'tabPage2';
            if (e.ctrlKey && k === 't') { e.preventDefault(); tab(party ? 'tabPage2' : 'tabPage1'); return; }
            if (e.ctrlKey && e.altKey) { e.preventDefault(); shortcuts(party ? 'party' : 'consignee'); return; }
            if (party) {
                if (e.ctrlKey && k === 's' && !$id('btnsave').classList.contains('is-hidden')) { e.preventDefault(); partySave($id('btnsave')); }
                if (e.ctrlKey && k === 'n') { e.preventDefault(); partyNew(); }
                if (e.ctrlKey && k === 'u' && !$id('btnupdate').classList.contains('is-hidden')) { e.preventDefault(); partyUpdate($id('btnupdate')); }
                if (e.ctrlKey && k === 'p' && PERM.Print) { e.preventDefault(); print292(); }
                if (e.ctrlKey && k === 'r') { e.preventDefault(); refresh($id('btnRefresh')); }
                if (e.ctrlKey && e.key === 'ArrowDown') { e.preventDefault(); var tr = $id('partyBody').querySelector('tr'); if (tr) tr.scrollIntoView(); }
                if (e.ctrlKey && (e.key === 'ArrowUp' || e.key === 'F5')) { e.preventDefault(); focus('cmbglac'); }
                if (e.ctrlKey && e.key === 'Enter' && PARTY.cur >= 0) { e.preventDefault(); partyReadById(PARTY.rows[PARTY.cur].Id); focus('cmbglac'); }
                if (e.ctrlKey && e.key === ' ' && PARTY.cur >= 0 && PERM.Update) { e.preventDefault(); partyRowUpdate(PARTY.cur, null); }
                if (e.ctrlKey && /^F[123]$/.test(e.key)) { e.preventDefault(); popup(e.key === 'F1' ? 'SupfrmBankDetail' : e.key === 'F2' ? 'SupfrmShipToAddress' : 'SupfrmSupplierLimits'); }
            } else {
                if (e.ctrlKey && k === 's' && !$id('btnSaveConsignee').classList.contains('is-hidden')) { e.preventDefault(); consigneeSave($id('btnSaveConsignee')); }
                if (e.ctrlKey && k === 'n') { e.preventDefault(); consigneeNew(); }
                if (e.ctrlKey && k === 'u' && !$id('btnupdate').classList.contains('is-hidden')) { e.preventDefault(); consigneeUpdate($id('btnUpdateConsignee')); }   /* desktop tests btnupdate.Visible */
                if (e.ctrlKey && k === 'p' && PERM.Print) { e.preventDefault(); print292_01(); }
                if (e.ctrlKey && k === 'r') { e.preventDefault(); refreshConsignee($id('btnRefreshConsignee')); }
                if (e.ctrlKey && e.key === 'ArrowDown') { e.preventDefault(); var tr2 = $id('consBody').querySelector('tr'); if (tr2) tr2.scrollIntoView(); }
                if (e.ctrlKey && (e.key === 'ArrowUp' || e.key === 'F5')) { e.preventDefault(); focus('CmbParentAcConsignee'); }
                if (e.ctrlKey && e.key === ' ' && CONS.cur >= 0 && PERM.Update) { e.preventDefault(); consRowUpdate(CONS.cur, null); }
            }
        });
        load();
    });

    global.ExportParties = {
        partyNew: partyNew, partySave: partySave, partyUpdate: partyUpdate, partyUpdateChecked: partyUpdateChecked, refresh: refresh, print292: print292,
        consigneeNew: consigneeNew, consigneeSave: consigneeSave, consigneeUpdate: consigneeUpdate, consigneeUpdateChecked: consigneeUpdateChecked,
        refreshConsignee: refreshConsignee, print292_01: print292_01, define: define, popup: popup, shortcuts: shortcuts, browse: browse, scrollHistory: scrollHistory
    };
}(window));
