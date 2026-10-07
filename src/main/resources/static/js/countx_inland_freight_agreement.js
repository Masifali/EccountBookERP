/* InLand Freight Agreement - Architecture.WinApp.Account_Definition.InLandFreightAgreement (screen 8).
   Event-for-event port of InLandFreightAgreement.cs. */
(function ($) {
    'use strict';
    var API = '/accounts/inland-freight-agreement';
    var RecId = 0, uidSeq = 0, updateRow = null;
    var table = [];                 // the DataTable behind grdDetail
    var lstRemove = [];             // lstRemoveRecord
    var attSaved = [];              // Attachment.lst rows already stored (ReadById_Update)
    var attNew = [];                // files picked in the Attachment form since the last save
    var effTime = '00:00:00', docTime = '00:00:00';   // DateTimePicker.Value keeps the time of day

    function busy(btn, on) { var $b = $(btn); if (on) $b.prop('disabled', true).addClass('btn-busy'); else $b.removeClass('btn-busy').prop('disabled', false); }
    function isBusy(btn) { return $(btn).hasClass('btn-busy'); }
    function say(m) { if (m != null && m !== '') window.alert(m); }
    function failText(x) {
        try { var j = x.responseJSON || JSON.parse(x.responseText); if (j && j.message) return j.message; } catch (e) { /* ignore */ }
        return x && x.status ? 'HTTP ' + x.status : 'Request failed';
    }
    function call(method, path, data) {
        return new Promise(function (resolve, reject) {
            var o = { url: API + path, type: method, dataType: 'json', cache: false };
            if (method === 'POST') { o.contentType = 'application/json'; o.data = JSON.stringify(data); } else o.data = data || {};
            $.ajax(o).done(function (r) { if (r && r.success === false) reject(new Error(r.message || 'Error')); else resolve(r); })
                .fail(function (x) { reject(new Error(failText(x))); });
        });
    }
    function pad(n) { return (n < 10 ? '0' : '') + n; }
    function nowParts() { var d = new Date(); return { date: d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()), time: pad(d.getHours()) + ':' + pad(d.getMinutes()) + ':' + pad(d.getSeconds()) }; }
    /* Conversion.ToDouble / ToDecimal (string) */
    function toDouble(s) { var t = String(s == null ? '' : s).trim(); if (t === '') return 0; var n = Number(t); return isFinite(n) ? n : 0; }
    /* Conversion.ToInt = Convert.ToInt32(string): digits only, otherwise 0 */
    function toInt(s) { var t = String(s == null ? '' : s).trim(); if (!/^[+-]?\d+$/.test(t)) return 0; var n = parseInt(t, 10); return (n > 2147483647 || n < -2147483648) ? 0 : n; }
    /* Conversion.ToString(double): 15 significant digits */
    function netStr(n) { return String(parseFloat(Number(n).toPrecision(15))); }
    /* "#,##.##" : thousands separators, up to two decimals, zero shows nothing */
    function fmtHash(v) {
        var n = toDouble(v); if (n === 0) return '';
        var s = (Math.round(Math.abs(n) * 100) / 100).toFixed(2).replace(/\.?0+$/, '');
        var p = s.split('.'); p[0] = p[0].replace(/\B(?=(\d{3})+(?!\d))/g, ',');
        return (n < 0 ? '-' : '') + p.join('.');
    }
    /* "0,0" */
    function fmtZeroComma(v) {
        var n = Math.round(toDouble(v));
        return (n < 0 ? '-' : '') + String(Math.abs(n)).replace(/\B(?=(\d{3})+(?!\d))/g, ',');
    }
    /* 'yyyy-MM-dd HH:mm:ss' -> dd-MMM-yyyy style text of the DataTable cell (DateTime.ToString of the Janus column) */
    function fmtDt(v) {
        var m = /^(\d{4})-(\d{2})-(\d{2})(?:[ T](\d{2}):(\d{2}):(\d{2}))?/.exec(String(v == null ? '' : v));
        if (!m) return v == null ? '' : String(v);
        return (+m[2]) + '/' + (+m[3]) + '/' + m[1] + ' ' + (m[4] ? (+m[4] % 12 === 0 ? 12 : +m[4] % 12) : 12) + ':' + (m[5] || '00') + ':' + (m[6] || '00') + ' ' + ((m[4] ? +m[4] : 0) >= 12 ? 'PM' : 'AM');
    }
    function dtKey(v) { return String(v == null ? '' : v).replace('T', ' '); }
    function sizeMb(bytes) { return Math.round(bytes / 1048576 * 100) / 100; }

    /* ---------------------------------------------------------------- combos (DDL.BindDDL ..., ZeroIndex: false) */
    function fill(sel, rows, keyV, keyT) {
        if (rows && rows.length > 0) { AccF.fillSelect(sel, rows.map(function (r) { return { V: CJG.pick(r, keyV), T: CJG.pick(r, keyT) }; }), 'V', 'T', null); clearCombo(sel); }
    }
    function comboVal(sel) { var v = $(sel).val(); return v == null ? '' : String(v); }
    function comboText(sel) { return comboVal(sel) === '' ? '' : $(sel + ' option:selected').text(); }
    function setCombo(sel, v) {
        var s = v == null ? '' : String(v);
        if (!$(sel).find('option').filter(function () { return this.value === s; }).length) s = '';
        $(sel).val(s).trigger('change');
    }
    function clearCombo(sel) { $(sel).val('').trigger('change'); }
    function activateRow(sel, i) {
        var o = $(sel).find('option').eq(i);
        if (o.length) $(sel).val(o.val()).trigger('change');
    }
    function applyLoad(r) {
        if (r.parties && r.parties.length > 0) { fill('#CmbParty', r.parties, 'Id', 'CompanyName'); fill('#CmbTransporter', r.parties, 'Id', 'CompanyName'); }
        if (r.districts) fill('#cmbDistrict', r.districts, 'Id', 'District');
        if (r.cities) { fill('#cmbLoadingLocation', r.cities, 'Id', 'CityName'); fill('#cmbUnloadingLocation', r.cities, 'Id', 'CityName'); }
        if (r.freightTypes) { fill('#CmbFreightType', r.freightTypes, 'Id', 'Type'); activateRow('#CmbFreightType', 2); }
        if (r.docNo != null) $('#TxtDocNo').val(String(r.docNo));
    }

    /* ---------------------------------------------------------------- TextChanged chains */
    var depth = 0;
    function setTxt(sel, v) {
        if ($(sel).val() === v) return;
        $(sel).val(v);
        if (depth > 25) return;
        depth++;
        try {
            if (sel === '#txtFreightRate') onRate(); else if (sel === '#txtFreightUom') onUom(); else if (sel === '#txtFreightAmount') onAmount();
        } finally { depth--; }
    }
    function calculateAmount() {
        var rate = toDouble($('#txtFreightRate').val()), uom = toInt($('#txtFreightUom').val());
        if (rate > 0 && uom > 0) setTxt('#txtFreightAmount', netStr(rate * uom));
    }
    function calculateRate() {
        var amt = toDouble($('#txtFreightAmount').val()), uom = toInt($('#txtFreightUom').val());
        if (amt > 0 && uom > 0) setTxt('#txtFreightRate', netStr(amt / uom));
    }
    function onRate() { if (toDouble($('#txtFreightRate').val()) > 0) calculateAmount(); else setTxt('#txtFreightAmount', ''); }
    function onUom() { if (toDouble($('#txtFreightRate').val()) > 0) calculateAmount(); else calculateRate(); }
    function onAmount() { if (toDouble($('#txtFreightAmount').val()) > 0) calculateRate(); else setTxt('#txtFreightRate', ''); }
    /* CmbFreightType_TextChanged */
    function freightTypeChanged() {
        var t = comboText('#CmbFreightType');
        if (t === 'Lump Sum') { $('#txtFreightUom').prop('readOnly', true); setTxt('#txtFreightUom', '1'); $('#lblUom').text('Freight Uom'); }
        else if (t === 'By Weight') { $('#lblUom').text('Freight Uom Kg'); $('#txtFreightUom').prop('readOnly', false); setTxt('#txtFreightUom', ''); }
        else if (t === 'By Qty') { $('#lblUom').text('No. Of Bags'); $('#txtFreightUom').prop('readOnly', false); setTxt('#txtFreightUom', ''); }
    }
    /* CommonServices.OnlytextdecimelFunction: digits, one decimal point and control keys */
    function decimalKey(e) {
        if (e.ctrlKey || e.metaKey || e.altKey || e.key.length > 1) return;
        if (/\d/.test(e.key)) return;
        if (e.key === '.' && this.value.indexOf('.') < 0) return;
        e.preventDefault();
    }

    /* ---------------------------------------------------------------- grids */
    function rowOf(r) { for (var i = 0; i < table.length; i++) if (table[i].__uid === r.__uid) return table[i]; return null; }
    var grdDetail = CJG.create({
        table: '#grdDetail', nav: '#navDetail', autoResize: false,
        columns: [{ key: 'Id', hidden: true }, { key: 'DistrictId', hidden: true }, { key: 'DistrictName', caption: 'DistrictName', width: 150 },
                  { key: 'LoadingLocationId', hidden: true }, { key: 'LoadingLocationName', caption: 'LoadingLocationName', width: 110 },
                  { key: 'UnLoadingLocationId', hidden: true }, { key: 'UnLoadingLocationName', caption: 'UnLoadingLocationName', width: 120 },
                  { key: 'EffectiveDate', caption: 'EffectiveDate', width: 130, format: function (v) { return fmtDt(v); } },
                  { key: 'FreightTypeId', hidden: true }, { key: 'FreightType', caption: 'FreightType', width: 90 },
                  { key: 'FreightRate', caption: 'FreightRate', width: 90, num: true }, { key: 'FreightUom', caption: 'FreightUom', width: 90, num: true },
                  { key: 'PackSizeTo', caption: 'PackSizeTo', width: 90, num: true },
                  { key: 'FreightAmount', caption: 'FreightAmount', width: 100, num: true, sum: true, format: function (v) { return fmtHash(v); }, totalFormat: function (t) { return fmtHash(t); } },
                  { key: 'MinAmount', caption: 'MinAmount', width: 100, num: true, sum: true, format: function (v) { return fmtHash(v); }, totalFormat: function (t) { return fmtHash(t); } },
                  { key: 'RemarksDetail', caption: 'RemarksDetail', width: 200 },
                  { key: 'Delete', caption: 'X', button: 'X', width: 20 }],
        onButton: function (col, row) { if (col === 'Delete') grdDetail_Delete(row); },
        onDblClick: function (row) { grdDetail_DoubleClick(row); }
    });
    function bindDetail() { grdDetail.setRows(table); }
    var histCols = [];
    var grdHistory = CJG.create({
        table: '#GrdHistory', nav: '#navHistory', autoResize: true, columns: histCols,
        onButton: function (col, row) { GrdHistory_ColumnButtonClick(col, row); },
        onLink: function (col, row) { GrdHistory_LinkClicked(col, row); },
        onDblClick: function (row) { GrdHistory_DoubleClick(row); }
    });
    var grdHistoryDetail = CJG.create({
        table: '#GrdDetailHistory', nav: '#navHistoryDetail', autoResize: true,
        columns: [{ key: 'DistrictName', caption: 'DistrictName', width: 150 }, { key: 'LoadingLocationName', caption: 'LoadingLocationName', width: 150 },
                  { key: 'UnLoadingLocationName', caption: 'UnLoadingLocationName', width: 150 },
                  { key: 'EffectiveDate', caption: 'EffectiveDate', width: 130, format: function (v) { return fmtDt(v); } },
                  { key: 'FreightType', caption: 'FreightType', width: 90 }, { key: 'FreightRate', caption: 'FreightRate', width: 90, num: true },
                  { key: 'FreightUom', caption: 'FreightUom', width: 90, num: true }, { key: 'PackSizeTo', caption: 'PackSizeTo', width: 90, num: true },
                  { key: 'FreightAmount', caption: 'FreightAmount', width: 100, num: true, sum: true, format: function (v) { return fmtZeroComma(v); }, totalFormat: function (t) { return fmtZeroComma(t); } },
                  { key: 'MinAmount', caption: 'MinAmount', width: 100, num: true, sum: true, format: function (v) { return fmtZeroComma(v); }, totalFormat: function (t) { return fmtZeroComma(t); } },
                  { key: 'RemarksDetail', caption: 'RemarksDetail', width: 200 }]
    });

    /* ---------------------------------------------------------------- tabs */
    function selectTab(i) {
        $('#pageForm').toggleClass('on', i === 0); $('#pageHistory').toggleClass('on', i === 1);
        $('#tabF').toggleClass('on', i === 0); $('#tabH').toggleClass('on', i === 1);
        /* tabControl1_SelectedIndexChanged: SelectedIndex == 1 -> HistoryGridFill() */
        if (i === 1) historyFill(); else grdDetail.rerender();
    }
    function tabIndex() { return $('#pageHistory').hasClass('on') ? 1 : 0; }
    function historyFill() {
        return call('GET', '/history').then(function (r) {
            var rows = r.rows || [];
            if (rows.length > 0) {
                histCols.length = 0;
                Object.keys(rows[0]).forEach(function (k) {
                    if (k === 'Id') histCols.push({ key: k, hidden: true });
                    else if (k === 'NoOfAttachments') histCols.push({ key: k, caption: k, width: 100, link: true, num: true });
                    else histCols.push({ key: k, caption: k, width: 110, format: function (v) { return /^\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}$/.test(String(v)) ? fmtDt(v) : (v == null ? '' : String(v)); } });
                });
                histCols.push({ key: 'Detail', caption: 'Detail', button: 'Detail', width: 50 });
                histCols.push({ key: 'Edit', caption: 'Edit', button: 'Edit', width: 30, buttonHtml: '<svg viewBox="0 0 16 16" width="14" height="14"><path d="M2 14l1-4 8-8 3 3-8 8z" fill="#f2c94c" stroke="#7a5b00"/></svg>', buttonTitle: 'Edit' });
                histCols.push({ key: 'Print', caption: 'Print', button: 'Print', width: 30, buttonHtml: '<svg viewBox="0 0 16 16" width="14" height="14"><rect x="3" y="1.5" width="10" height="4" fill="#fff" stroke="#555"/><rect x="1.5" y="5.5" width="13" height="6" fill="#d9d9d9" stroke="#555"/><rect x="3.5" y="9.5" width="9" height="5" fill="#fff" stroke="#555"/></svg>', buttonTitle: 'Print' });
                grdHistory.setRows(rows);
            } else grdHistory.clear();              // GrdHistory.ClearStructure()
        }).catch(function (e) { say(e.message); });
    }

    /* ---------------------------------------------------------------- Reset / DetailFormReset */
    function setSaveMode(isNew) { $('#btnsave').prop('hidden', !isNew); $('#btnUpdate').prop('hidden', isNew); }
    function setDetailMode(editing) { $('#btnAddDetail').prop('hidden', editing); $('#btnUpdateDetail').prop('hidden', !editing); $('#btnCancelDetail').prop('hidden', !editing); }
    function mainFormReset() { $('#txtAgreementCode').val(''); clearCombo('#CmbParty'); clearCombo('#CmbTransporter'); }
    function detailFormReset() {
        clearCombo('#cmbDistrict'); clearCombo('#cmbLoadingLocation'); clearCombo('#cmbUnloadingLocation');
        activateRow('#CmbFreightType', 2);
        setTxt('#txtFreightRate', ''); setTxt('#txtFreightUom', ''); $('#txtPackSize').val(''); setTxt('#txtFreightAmount', ''); $('#txtMinAmount').val(''); $('#txtDetailRemarks').val('');
    }
    function generateCode() {
        return call('GET', '/code').then(function (r) { $('#TxtDocNo').val(String(r.docNo)); }).catch(function (e) { say(e.message); });
    }
    function reset() {
        RecId = 0; updateRow = null;
        mainFormReset(); detailFormReset();
        table = []; lstRemove = []; bindDetail();
        attSaved = []; attNew = [];
        setDetailMode(false); setSaveMode(true);
        return generateCode();
    }
    function btnnew_Click() { reset(); }

    /* ---------------------------------------------------------------- detail validation / add / update / cancel / edit / delete */
    function formValidationDetail() {
        if (comboVal('#cmbDistrict') === '') { say('District Field is Required'); AccF.focus('#cmbDistrict'); return false; }
        if (comboVal('#cmbLoadingLocation') === '') { say('Loading Location Field is Required'); AccF.focus('#cmbLoadingLocation'); return false; }
        if (comboVal('#cmbUnloadingLocation') === '') { say('Unloading Location Field is Required'); AccF.focus('#cmbUnloadingLocation'); return false; }
        if (comboVal('#CmbFreightType') === '') { say('Freight Type Field is Required'); AccF.focus('#CmbFreightType'); return false; }
        if (toDouble($('#txtFreightRate').val().trim()) === 0) { say('Freight Rate Field is Required'); $('#txtFreightRate').focus(); return false; }
        if (toDouble($('#txtFreightUom').val().trim()) === 0) { say('freight Uom Field is Required'); $('#txtFreightUom').focus(); return false; }
        if (comboText('#CmbFreightType') === 'By Qty' && toDouble($('#txtPackSize').val().trim()) === 0) { say('Pack Size Field is Required'); $('#txtPackSize').focus(); return false; }
        if (toDouble($('#txtFreightAmount').val().trim()) === 0) { say('Freight Amount Field is Required'); $('#txtFreightAmount').focus(); return false; }
        if (toDouble($('#txtMinAmount').val().trim()) === 0) { say('Min Amount Field is Required'); $('#txtMinAmount').focus(); return false; }
        return true;
    }
    function effValue() { var d = $('#txtEffectiveDate').val() || nowParts().date; return d + ' ' + effTime; }
    function fillRow(o) {
        o.DistrictId = comboVal('#cmbDistrict'); o.DistrictName = comboText('#cmbDistrict');
        o.LoadingLocationId = comboVal('#cmbLoadingLocation'); o.LoadingLocationName = comboText('#cmbLoadingLocation').trim();
        o.UnLoadingLocationId = comboVal('#cmbUnloadingLocation'); o.UnLoadingLocationName = comboText('#cmbUnloadingLocation').trim();
        o.EffectiveDate = effValue();
        o.FreightTypeId = comboVal('#CmbFreightType'); o.FreightType = comboText('#CmbFreightType').trim();
        o.FreightRate = toDouble($('#txtFreightRate').val().trim()); o.FreightUom = toInt($('#txtFreightUom').val().trim());
        o.PackSizeTo = toInt($('#txtPackSize').val().trim()); o.FreightAmount = toDouble($('#txtFreightAmount').val().trim());
        o.MinAmount = toDouble($('#txtMinAmount').val().trim()); o.RemarksDetail = $('#txtDetailRemarks').val().trim();
        return o;
    }
    function btnAddDetail_Click() {
        if (!formValidationDetail()) return;
        if (toDouble($('#txtMinAmount').val()) > toDouble($('#txtFreightAmount').val())) { say("Min Amount Can't be Greater Than Freight Amount"); $('#txtMinAmount').focus(); return; }
        if (table.length > 0) {
            if (dtKey(effValue()) <= dtKey(table[table.length - 1].EffectiveDate)) { say("Can't Add Contract Against this Selected Effective Date!"); return; }
        }
        table.push(fillRow({ Id: 0, __uid: ++uidSeq }));
        bindDetail(); detailFormReset();
    }
    function btnUpdateDetail_Click() {
        if (!formValidationDetail()) return;
        if (toDouble($('#txtMinAmount').val()) > toDouble($('#txtFreightAmount').val())) { say("Min Amount Can't be Greater Than Freight Amount"); $('#txtMinAmount').focus(); return; }
        if (table.length > 0) {
            if (effValue().substring(0, 10) < dtKey(table[table.length - 1].EffectiveDate).substring(0, 10)) { say("Can't update Contract Against this Selected Effective Date!"); return; }
        }
        if (updateRow) fillRow(updateRow);
        setDetailMode(false);
        detailFormReset();
        bindDetail();
    }
    function btnCancelDetail_Click() { setDetailMode(false); detailFormReset(); }
    function grdDetail_DoubleClick(r) {
        var row = rowOf(r); if (!row) return;
        updateRow = row;
        setCombo('#cmbDistrict', row.DistrictId); setCombo('#cmbLoadingLocation', row.LoadingLocationId); setCombo('#cmbUnloadingLocation', row.UnLoadingLocationId);
        var m = /^(\d{4}-\d{2}-\d{2})[ T](\d{2}:\d{2}:\d{2})/.exec(dtKey(row.EffectiveDate));
        if (m) { $('#txtEffectiveDate').val(m[1]); effTime = m[2]; }
        setCombo('#CmbFreightType', row.FreightTypeId);
        setTxt('#txtFreightRate', netStr(row.FreightRate)); setTxt('#txtFreightUom', String(row.FreightUom)); $('#txtPackSize').val(String(row.PackSizeTo));
        setTxt('#txtFreightAmount', netStr(row.FreightAmount)); $('#txtMinAmount').val(netStr(row.MinAmount)); $('#txtDetailRemarks').val(row.RemarksDetail == null ? '' : String(row.RemarksDetail));
        setDetailMode(true);
        AccF.focus('#cmbDistrict');
    }
    /* grdDetail_ColumnButtonClick ("Delete") */
    function grdDetail_Delete(r) {
        if (!window.confirm('Are you sure to Delete?')) return;
        var row = rowOf(r); if (!row) return;
        var i = table.indexOf(row);
        if (!$('#btnsave').prop('hidden')) { table.splice(i, 1); }
        else {
            var d = {}; for (var k in row) if (k.indexOf('__') !== 0) d[k] = row[k];
            d.ActionId = 3; lstRemove.push(d);
            table.splice(i, 1);
        }
        bindDetail();
    }

    /* ---------------------------------------------------------------- FormValidation / Insert (Save / Update) */
    function formValidation() {
        var doc = $('#TxtDocNo').val().trim();
        if (doc === '' || doc === '0') { say('DocNo Field is Required'); $('#TxtDocNo').focus(); return false; }
        if (comboVal('#CmbParty') === '') { say('Customer Field is Required'); AccF.focus('#CmbParty'); return false; }
        if (comboVal('#CmbTransporter') === '') { say('Transporter Field is Required'); AccF.focus('#CmbTransporter'); return false; }
        return true;
    }
    function plainRow(row) { var d = {}; for (var k in row) if (k.indexOf('__') !== 0) d[k] = row[k]; return d; }
    function insert(btn) {
        if (isBusy(btn)) return;
        if (!formValidation()) return;
        if (table.length === 0) { say('Grid Record Not Found'); return; }
        if (!window.confirm(RecId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        var req = {
            id: RecId, docDate: ($('#txtDocDate').val() || nowParts().date) + ' ' + docTime, agreementCode: $('#txtAgreementCode').val().trim(),
            docNo: $('#TxtDocNo').val().trim(), partyId: comboVal('#CmbParty'), transporterId: comboVal('#CmbTransporter'),
            detail: table.map(plainRow), removed: RecId > 0 ? lstRemove : [],
            keepAttachments: RecId > 0 ? attSaved.map(function (a) { return a.Id; }) : [],
            addAttachments: attNew.map(function (a) { return { name: a.name, base64: a.base64 }; })
        };
        busy(btn, true);
        call('POST', '/save', req).then(function (r) {
            busy(btn, false);
            say((RecId > 0 ? 'Record Update Successfully [ ' : 'Record Save Successfully  [ ') + r.docNo + ' ] ');
            return reset().then(function () { grdDetail.clear(); });
        }).catch(function (e) { busy(btn, false); window.alert(e.message); });
    }
    function btnsave_Click() { RecId = 0; insert('#btnsave'); }
    function btnUpdate_Click() { insert('#btnUpdate'); }
    function btnRefresh_Click() {
        busy('#btnRefresh', true);
        call('GET', '/load').then(function (r) { applyLoad(r); }).catch(function (e) { say(e.message); }).then(function () { busy('#btnRefresh', false); });
    }

    /* ---------------------------------------------------------------- ReadById_Update / history */
    function toGridRow(d) {
        return { Id: toInt(d.Id), DistrictId: d.LoadingDistrictId, DistrictName: d.DistrictName, LoadingLocationId: d.LoadingLocationId, LoadingLocationName: d.LoadingLocationName,
            UnLoadingLocationId: d.DestinationLocationId, UnLoadingLocationName: d.UnLoadingLocationName, EffectiveDate: d.EffectiveDate, FreightTypeId: d.FreightTypeId,
            FreightType: d.FreightType, FreightRate: toDouble(d.FreightRate), FreightUom: toInt(d.FreightUom), PackSizeTo: toInt(d.PackSizeTo),
            FreightAmount: toDouble(d.FregihtAmount), MinAmount: toDouble(d.MinAmount), RemarksDetail: d.RemarksDetail == null ? '' : d.RemarksDetail, __uid: ++uidSeq };
    }
    function readById_Update(id) {
        setSaveMode(false);
        RecId = toInt(id);
        return call('GET', '/' + RecId).then(function (r) {
            var h = r.header || {};
            selectTab(0);
            $('#TxtDocNo').val(h.DocNo == null ? '' : String(h.DocNo));
            var m = /^(\d{4}-\d{2}-\d{2})[ T](\d{2}:\d{2}:\d{2})/.exec(dtKey(h.AgreementDate));
            if (m) { $('#txtDocDate').val(m[1]); docTime = m[2]; }
            $('#txtAgreementCode').val(h.AgreementCode == null ? '' : h.AgreementCode);
            setCombo('#CmbParty', h.SupplierCustomerId); setCombo('#CmbTransporter', h.TransporterId);
            table = (r.details || []).map(toGridRow); lstRemove = [];
            bindDetail();
            attSaved = (r.attachments || []).map(function (a) { return { Id: a.Id, name: a.Attachment, size: a.UploadedFileSizeMb, date: a.EntryDate }; });
            attNew = [];
        }).catch(function (e) { say(e.message); });
    }
    function GrdHistory_ColumnButtonClick(col, row) {
        if (col === 'Edit') { reset().then(function () { RecId = toInt(row.Id); return readById_Update(RecId); }); }
        /* "Print" has no handler in the desktop form (_ = e.Column.Key == "Print") */
        if (col === 'Detail') {
            call('GET', '/' + toInt(row.Id)).then(function (r) {
                grdHistoryDetail.setRows((r.details || []).map(function (d) {
                    return { DistrictName: d.DistrictName, LoadingLocationName: d.LoadingLocationName, UnLoadingLocationName: d.UnLoadingLocationName, EffectiveDate: d.EffectiveDate,
                        FreightType: d.FreightType, FreightRate: toDouble(d.FreightRate), FreightUom: toInt(d.FreightUom), PackSizeTo: toInt(d.PackSizeTo),
                        FreightAmount: toDouble(d.FregihtAmount), MinAmount: toDouble(d.MinAmount), RemarksDetail: d.RemarksDetail };
                }));
            }).catch(function (e) { say(e.message); });
        }
    }
    function GrdHistory_DoubleClick(row) { reset().then(function () { RecId = toInt(row.Id); return readById_Update(RecId); }); }

    /* ---------------------------------------------------------------- Attachment form (AT.Show) / AttachmentView */
    function cardHtml(name, meta, extra) {
        var ext = (String(name).split('.').pop() || '').toUpperCase().substring(0, 4);
        return '<div class="card" ' + extra + '><div class="pv">' + CJG.esc(ext) + '</div><div class="nm">' + CJG.esc(name) + '</div><div class="mt">' + CJG.esc(meta) + '</div></div>';
    }
    function renderAtt() {
        var h = '';
        attSaved.forEach(function (a, i) { h += cardHtml(a.name, (a.size == null ? '' : a.size + ' MB') + ' ' + (a.date ? fmtDt(a.date) : ''), 'data-s="' + i + '"').replace('</div></div>', '</div><button type="button" class="del" data-s="' + i + '">Delete</button></div>'); });
        attNew.forEach(function (a, i) { h += cardHtml(a.name, sizeMb(a.size) + ' MB (new)', 'data-n="' + i + '"').replace('</div></div>', '</div><button type="button" class="del" data-n="' + i + '">Delete</button></div>'); });
        $('#attCards').html(h);
        $('#attNote').text(attSaved.length + attNew.length + ' file(s)');
    }
    function btnAttachment_Click() { renderAtt(); $('#ovAtt').addClass('on'); }
    function renderView(rows) {
        var h = '', mb = 0;
        rows.forEach(function (a) {
            mb += toDouble(a.UploadedFileSizeMb);
            h += cardHtml(a.Attachment, (a.UploadedFileSizeMb == null ? '' : a.UploadedFileSizeMb + ' MB'), 'data-id="' + a.Id + '"');
        });
        $('#viewCards').html(h); $('#vFiles').text(rows.length); $('#vSize').text(Math.round(mb * 100) / 100);
    }
    var viewId = 0;
    function GrdHistory_LinkClicked(col, row) {
        if (col !== 'NoOfAttachments') return;
        viewId = toInt(row.Id);
        call('GET', '/' + viewId + '/attachments').then(function (r) { renderView(r.rows || []); $('#ovView').addClass('on'); }).catch(function (e) { say(e.message); });
    }
    function pickFiles(files) {
        var list = Array.prototype.slice.call(files || []);
        if (attNew.length + list.length > 10) { say('Select at most ten files at once'); return; }
        list.forEach(function (f) {
            if (f.size === 0 || f.size > 5 * 1024 * 1024) { say(f.name + ': Attachment must be between 1 byte and 5 MB'); return; }
            var fr = new FileReader();
            fr.onload = function () {
                var s = String(fr.result), i = s.indexOf(',');
                attNew.push({ name: f.name, size: f.size, base64: i >= 0 ? s.substring(i + 1) : '' });
                renderAtt();
            };
            fr.readAsDataURL(f);
        });
    }

    /* ---------------------------------------------------------------- print (242 slip / 243 register) */
    function printKind(kind, id, btn) {
        if (isBusy(btn)) return;
        busy(btn, true);
        call('GET', '/print/' + kind + '/count', { id: id }).then(function (r) {
            if (!r.count) { say('Not Record Found For Display'); return; }
            window.open(API + '/print/' + kind + '?id=' + id, '_blank');
        }).catch(function (e) { say(e.message); }).then(function () { busy(btn, false); });
    }

    /* ---------------------------------------------------------------- wiring */
    $(function () {
        var n = nowParts();
        $('#txtDocDate').val(n.date); $('#txtEffectiveDate').val(n.date); docTime = effTime = n.time;
        $('#tabF').on('click', function () { selectTab(0); });
        $('#tabH').on('click', function () { selectTab(1); });
        $('#btnnew').on('click', btnnew_Click);
        $('#btnsave').on('click', btnsave_Click);
        $('#btnUpdate').on('click', btnUpdate_Click);
        $('#btnRefresh').on('click', btnRefresh_Click);
        $('#btnAttachment').on('click', btnAttachment_Click);
        $('#toolStripButton1').on('click', function () { printKind('slip', RecId, '#toolStripButton1'); });
        $('#btnGrnFormHistory').on('click', function () { printKind('register', 0, '#btnGrnFormHistory'); });
        $('#btnLoadAll').on('click', function () { /* no handler in the desktop form */ });
        $('#btnAddDetail').on('click', btnAddDetail_Click);
        $('#btnUpdateDetail').on('click', btnUpdateDetail_Click);
        $('#btnCancelDetail').on('click', btnCancelDetail_Click);
        $('#txtFreightRate').on('input', function () { if (depth === 0) { depth++; try { onRate(); } finally { depth--; } } }).on('keydown', decimalKey);
        $('#txtFreightUom').on('input', function () { if (depth === 0) { depth++; try { onUom(); } finally { depth--; } } }).on('keydown', decimalKey);
        $('#txtFreightAmount').on('input', function () { if (depth === 0) { depth++; try { onAmount(); } finally { depth--; } } });
        $('#CmbFreightType').on('change', freightTypeChanged);
        $('#txtDocDate').on('change', function () { /* picking a date keeps the time of day */ });
        $('#attChoose').on('click', function () { $('#attFile').val('').trigger('click'); });
        $('#attFile').on('change', function () { pickFiles(this.files); });
        $('#attClose').on('click', function () { $('#ovAtt').removeClass('on'); });
        $('#viewClose').on('click', function () { $('#ovView').removeClass('on'); });
        $('#attCards').on('click', '.del', function (e) {
            e.stopPropagation();
            var s = $(this).data('s'), nn = $(this).data('n');
            if (s != null) attSaved.splice(+s, 1); else if (nn != null) attNew.splice(+nn, 1);
            renderAtt();
        }).on('click', '.card', function () {
            var s = $(this).data('s');
            if (s != null && RecId > 0 && attSaved[+s]) window.open(API + '/' + RecId + '/attachments/' + attSaved[+s].Id, '_blank');
        });
        $('#viewCards').on('click', '.card', function () { window.open(API + '/' + viewId + '/attachments/' + $(this).data('id'), '_blank'); });
        $(document).on('keydown', function (e) {
            if (e.altKey && !e.ctrlKey) {
                var k = e.key.toLowerCase();
                if (k === 'n') { e.preventDefault(); $('#btnnew').click(); }
                else if (k === 's' && !$('#btnsave').prop('hidden')) { e.preventDefault(); $('#btnsave').click(); }
                else if (k === 'a') { e.preventDefault(); $('#btnAttachment').click(); }
            } else if (e.ctrlKey && tabIndex() === 1 && e.key.toLowerCase() === 'h') { e.preventDefault(); $('#btnGrnFormHistory').click(); }
        });
        /* InLandFreightAgreement_Load */
        call('GET', '/load').then(function (r) {
            applyLoad(r);
            $('#TxtDocNo').focus();
            setSaveMode(true); setDetailMode(false);
            bindDetail();
        }).catch(function (e) { say(e.message); });
    });
})(window.jQuery);
