/* ============================================================================================
 * countx_export_third_party_inspection.js - shared by two desktop forms that edit one record
 * (InvLabPreProductionExportLotInspectionHeader):
 *   857 frmThirdPartyInspection            page /export/third-party-inspection             (body data-tpi="857")
 *   858 frmLabAgainstThirdPartyInspection  page /export/lab-against-third-party-inspection (body data-tpi="858")
 * Every button, Leave, CheckedChanged, TextChanged, grid button, cell edit, double-click and shortcut of
 * the two forms has its counterpart here with the desktop's messages and order; data comes from
 * /api/export/<page>/... (ExportThirdPartyInspectionController -> ExportThirdPartyInspectionService ->
 * the desktop's own procedures). Prints: 514_ThirdPartyInspectionSlip.rpt (exp-514) and, on 857,
 * 514_01_InventoryStockReservedSlip.rpt (exp-514-01) through CrystalPrint.
 *
 * Button contract on every action: disabled + spinner while the request runs, duplicates ignored,
 * re-enabled on success and on failure (busy()).
 * ============================================================================================ */
(function (global) {
    'use strict';

    var MODE = 857;
    var API = '/api/export/third-party-inspection';

    function $id(id) { return document.getElementById(id); }
    function box(m) { global.alert(m); }
    function ask(m) { return global.confirm(m); }
    function esc(s) {
        return String(s === null || s === undefined ? '' : s)
            .replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    }
    function str(v) { return v === null || v === undefined ? '' : String(v); }
    function netI(v) { var n = parseInt(String(v === null || v === undefined ? '' : v).replace(/,/g, ''), 10); return isFinite(n) ? n : 0; }
    function netD(v) { var n = parseFloat(String(v === null || v === undefined ? '' : v).replace(/,/g, '')); return isFinite(n) ? n : 0; }
    /* .NET "#,##0.###"-style: max decimals, trailing zeros dropped, thousands separators. */
    function fmt(v, dec) {
        var n = netD(v);
        if (dec === undefined) dec = 3;
        var s = n.toFixed(dec);
        if (dec > 0) s = s.replace(/\.?0+$/, '');
        var parts = s.split('.');
        parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ',');
        return parts.join('.');
    }
    function pad(n) { return (n < 10 ? '0' : '') + n; }
    function isoOf(d) { return d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()); }
    function today() { return isoOf(new Date()); }
    function daysAgo(n) { var d = new Date(); d.setDate(d.getDate() - n); return isoOf(d); }
    function addDays(iso, n) { var p = /^(\d{4})-(\d{2})-(\d{2})/.exec(iso || ''); var d = p ? new Date(+p[1], +p[2] - 1, +p[3]) : new Date(); d.setDate(d.getDate() + n); return isoOf(d); }
    var MON = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    function isoDate(v) { var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(str(v).trim()); return m ? m[1] + '-' + m[2] + '-' + m[3] : ''; }
    function ddMMMyyyy(v) { var s = isoDate(v); if (!s) return ''; return s.substring(8, 10) + '-' + MON[parseInt(s.substring(5, 7), 10) - 1] + '-' + s.substring(0, 4); }
    function ddMMMHm(v) {
        var m = /^(\d{4})-(\d{2})-(\d{2})[T ](\d{2}):(\d{2})/.exec(str(v).trim());
        if (!m) return ddMMMyyyy(v);
        var h = parseInt(m[4], 10), tt = h >= 12 ? 'PM' : 'AM'; h = h % 12; if (h === 0) h = 12;
        return m[3] + '-' + MON[parseInt(m[2], 10) - 1] + '-' + m[1] + ' ' + pad(h) + ':' + m[5] + ' ' + tt;
    }
    function busy(btn, fn) {
        var b = (typeof btn === 'string') ? $id(btn) : btn;
        if (b) {
            if (b.disabled || b.classList.contains('is-busy')) return Promise.resolve();
            b.disabled = true; b.classList.add('is-busy');
        }
        var done = function () { if (b) { b.disabled = false; b.classList.remove('is-busy'); } };
        var p;
        try { p = fn(); } catch (e) { done(); box(e.message); return Promise.resolve(); }
        if (p && typeof p.then === 'function') return p.then(done, function (e) { done(); if (e && e.message) box(e.message); });
        done();
        return Promise.resolve(p);
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
    function refreshCombos() { if (global.DesktopCombo) global.DesktopCombo.refresh(); }
    function show(id, on) { var e = $id(id); if (e) e.classList.toggle('is-hidden', !on); }
    function col(row, name) {
        if (!row) return null;
        if (Object.prototype.hasOwnProperty.call(row, name)) return row[name];
        var l = name.toLowerCase();
        for (var k in row) if (Object.prototype.hasOwnProperty.call(row, k) && k.toLowerCase() === l) return row[k];
        return null;
    }
    /* bind(id, rows, value, text, blank) - blank row first unless blank === false (BindDDL ZeroIndex / insertDefaultRow). */
    function bind(id, rows, valueCol, textCol, blank) {
        var s = $id(id); if (!s) return;
        var keep = s.value;
        var h = blank === false ? '' : '<option value="0"></option>';
        (rows || []).forEach(function (r) { h += '<option value="' + esc(col(r, valueCol)) + '">' + esc(col(r, textCol)) + '</option>'; });
        s.innerHTML = h;
        s.value = keep;
        if (s.value !== keep) s.value = blank === false ? (s.options.length ? s.options[0].value : '') : '0';
        refreshCombos();
    }
    function setVal(id, v) { var s = $id(id); if (!s) return; s.value = str(v); if (s.value !== str(v)) { if (s.tagName === 'SELECT') s.value = s.querySelector('option[value="0"]') ? '0' : ''; } refreshCombos(); }
    function val(id) { var e = $id(id); return e ? e.value : ''; }
    function selText(id) { var s = $id(id); if (!s || s.selectedIndex < 0) return ''; var o = s.options[s.selectedIndex]; return o && o.value !== '0' ? o.text : ''; }
    function setText(id, v) { var e = $id(id); if (e) e.value = str(v); }
    function focus(id) { var e = $id(id); if (!e) return; var w = e.closest && e.closest('.dtcombo-wrap'); var t = w ? w.querySelector('.dtcombo-input') : e; try { (t || e).focus(); } catch (x) { /* ignore */ } }
    function radio(name) { var r = document.querySelector('input[name="' + name + '"]:checked'); return r ? r.value : ''; }
    function setRadio(name, v) { var r = document.querySelector('input[name="' + name + '"][value="' + v + '"]'); if (r) r.checked = true; }
    /* a ShowCheckBox DateTimePicker: <id>Chk + <id> */
    function dChecked(id) { var c = $id(id + 'Chk'); return !!(c && c.checked); }
    function dSet(id, iso) { var c = $id(id + 'Chk'); if (iso) { setText(id, iso); if (c) c.checked = true; } else if (c) c.checked = false; dState(id); }
    function dState(id) { var c = $id(id + 'Chk'), e = $id(id); if (c && e) e.classList.toggle('ex-unchecked', !c.checked); }
    function cancel() {
        global.close();
        setTimeout(function () { if (!global.closed) { if (history.length > 1) history.back(); else location.href = '/export'; } }, 150);
    }

    // ------------------------------------------------------------------------------ state

    var PERM = { Save: true, Update: true, Print: true, CanViewAllRecord: true };
    var CFG = { days: 0, countryOfOrigin: 0, mostReq: 0, mostHand: 0 };
    var L = { items: [], tracking: [], resultStatus: [], farming: [], contracts: [], invoices: [], schedules: [] };
    var S = { recId: 0, analysisGroupId: 0, removeIds: '', detail: [], params: [], sampling: [], updIndex: -1, mtonsTyped: false, exporterTyped: '' };
    var HIST = [], CUR = { hist: -1, grd: -1 };

    function lab() { return MODE === 858; }

    // ------------------------------------------------------------------------------ tabs

    function tab(panelId) {
        document.querySelectorAll('.win-tabs[data-tabs="tpi"] .win-tab').forEach(function (b) { b.classList.toggle('is-active', b.getAttribute('data-tab') === panelId); });
        ['tpiForm', 'tpiHistory'].forEach(function (p) { $id(p).classList.toggle('is-active', p === panelId); });
        var onHist = panelId === 'tpiHistory';
        var fb = $id('btnTpiFooterHistory');
        fb.querySelector('span').textContent = onHist ? 'Form' : 'History';
        fb.querySelector('i').className = onHist ? 'fa fa-file-text-o' : 'fa fa-history';
        /* tabControl1_SelectedIndexChanged */
        if (onHist) focus('FromDateHistory'); else focus('CmbLotTrackingNo');
    }
    function onHistory() { return $id('tpiHistory').classList.contains('is-active'); }
    function toggleHistory() { tab(onHistory() ? 'tpiForm' : 'tpiHistory'); }

    // ------------------------------------------------------------------------------ load

    function bindCombos(d) {
        bind('CmbRequestedBy', d.requestedBy, 'Id', 'name');
        bind('CmbSampleHandOver', d.requestedBy, 'Id', 'name');
        bind('CmbMedium', d.medium, 'Id', 'name');
        bind('CmbInspectionAgency', d.agency, 'Id', 'name');
        bind('CmbSamplingResponsibity', d.samplingResponsibility, 'Id', 'name');
        L.resultStatus = d.resultStatus || [];
        /* 857: insertDefaultRow true (blank first); 858: BindDDL ZeroIndex false (no blank row). */
        bind('CmbResultStatus', L.resultStatus, 'Id', 'name', !lab());
        bind('CmbResultRemarks', d.resultRemarks, 'Id', 'name');
        if (!lab()) bind('CmbCustomer', d.customers, 'Id', 'name');
        bind('CmbCountryOfOrigin', d.countries, 'Id', 'name');
        bind('CmbCountryOfInspection', d.countries, 'Id', 'name');
        bind('CmbRequiredAnalysisGroup', d.analysisGroups, 'Id', 'name', !lab() ? undefined : false);
        L.farming = d.farmingNTrade || [];
        bind('CmbFarmingNTrade', L.farming, 'Id', 'name');
        bind('CmbJobLot', d.jobLots, 'Id', 'name', false);
        L.items = d.items || [];
        itemNameBind();
        if (lab()) {
            /* SetComboValue(..., ActivateRow: true, 1) - first real row when nothing kept */
            if (netI(val('CmbMedium')) === 0) firstReal('CmbMedium');
            if (netI(val('CmbSamplingResponsibity')) === 0) firstReal('CmbSamplingResponsibity');
            if (netI(val('CmbCountryOfOrigin')) === 0) firstReal('CmbCountryOfOrigin');
        }
    }
    function firstReal(id) { var s = $id(id); if (!s) return; for (var i = 0; i < s.options.length; i++) if (s.options[i].value !== '0') { s.value = s.options[i].value; break; } refreshCombos(); }
    /* CmbResultStatus.Rows[n].Activate() - row n of the bound list (row 0 is the blank row on 857). */
    function resultStatusRow(n) {
        var s = $id('CmbResultStatus'); if (!s || !s.options.length) return;
        if (n < s.options.length) { s.selectedIndex = n; refreshCombos(); }
    }

    /** InitializeComponentMethod. */
    function load() {
        return getJson(API + '/setup').then(function (d) {
            d = d || {};
            if (d.permissions) PERM = d.permissions;
            CFG.days = netI(d.defaultDaysToLessFromHistoryFromDate);
            CFG.countryOfOrigin = netI(d.defaultExportInspectionCountryOfOriginId);
            CFG.mostReq = netI(d.mostUsedRequestedById);
            CFG.mostHand = netI(d.mostUsedSampleHandedOverId);
            if ($id('btnsave')) $id('btnsave').disabled = !PERM.Save;
            $id('btnPrint').disabled = !PERM.Print;
            $id('btnUpdate').disabled = !PERM.Update;
            bindCombos(d);
            trackingBind(d.trackingNos || []);
            historyComboFill(d);
            /* BindMostUsed */
            if (CFG.mostReq) setVal('CmbRequestedBy', CFG.mostReq);
            if (lab()) {
                if (CFG.mostHand) setVal('CmbSampleHandOver', CFG.mostHand);
                if (CFG.countryOfOrigin) setVal('CmbCountryOfOrigin', CFG.countryOfOrigin);
                resultStatusRow(0);
            } else {
                /* ((UltraGridBase)CmbResultStatus).Rows[1].Activate() */
                resultStatusRow(1);
            }
            setText('FromDateHistory', CFG.days > 0 ? daysAgo(CFG.days) : daysAgo(3));
            setText('ToDateHistory', today());
            setText('txtRequestLodgDate', today());
            ['txtSampleDispatchedDate', 'txtSampleATADesDate', 'txtReportDate', 'txtDateOfInspection', 'txtResultDate', 'txtStockReservedDate',
             'txtSampleTakenDate', 'txtStockSealedDate', 'txtHandOverDate', 'txtSampleETADesDate'].forEach(function (id) { if (!val(id)) setText(id, today()); dState(id); });
            etaDate();
            renderAll();
            $id('tpiFooterInfo').textContent = (lab() ? 'frmLabAgainstThirdPartyInspection' : 'frmThirdPartyInspection') + '  -  Screen ' + MODE;
            focus('CmbLotTrackingNo');
        }).catch(function (e) { box(e.message || 'Error occurred during database call.'); });
    }

    /** ItemNameBind - Name / Code radio picks the display member. */
    function itemNameBind() {
        var byName = $id('rdSearchByName').checked;
        bind('CmbItemMain', L.items, 'Id', byName ? 'ItemName' : 'ItemCode');
    }

    // ------------------------------------------------------------------------------ tracking no

    function trackingBind(rows) {
        L.tracking = rows || [];
        $id('dlTracking').innerHTML = L.tracking.map(function (r) { return '<option value="' + esc(col(r, 'name')) + '">' + esc(col(r, 'Status')) + '</option>'; }).join('');
    }
    function trackingId(text) {
        var t = str(text).trim(); if (!t) return 0;
        for (var i = 0; i < L.tracking.length; i++) if (str(col(L.tracking[i], 'name')) === t) return netI(col(L.tracking[i], 'Id'));
        return 0;
    }
    /** TackingDbCall: RadAll / RadNotReffered (Pending, default) / RadReffered (Complete). */
    function trackingReload() {
        return getJson(API + '/tracking-nos?status=' + encodeURIComponent(radio('tpiStatus'))).then(trackingBind).catch(function (e) { box(e.message); });
    }
    /** CmbLotTrackingNo_Leave. */
    function trackingLeave() {
        var id = trackingId(val('CmbLotTrackingNo'));
        if (id > 0) return readById(id);
        setText('CmbLotCurrentStage', 'Pending For Inspection');
        return Promise.resolve();
    }

    // ------------------------------------------------------------------------------ grids

    function sumRow(cols, rows, numCols, lead) {
        if (!rows.length) return '';
        var sums = {};
        rows.forEach(function (r) { numCols.forEach(function (c) { sums[c] = (sums[c] || 0) + netD(r[c]); }); });
        var t = '<tr>' + (lead ? '<td class="lbl" colspan="' + lead + '">&Sigma;</td>' : '');
        cols.forEach(function (c, i) {
            var isNum = numCols.indexOf(c) >= 0;
            t += '<td' + (isNum ? '' : ' class="lbl"') + '>' + (isNum ? esc(fmt(sums[c])) : (i === 0 && !lead ? '&Sigma;' : '')) + '</td>';
        });
        return t + '</tr>';
    }

    var DCOLS = ['Lot', 'Customer', 'ContractNo', 'ContractScheduleNo', 'InvoiceNo', 'MTons', 'SealNoForBuyer', 'SealNoForShipper', 'Remarks'];
    function renderDetail() {
        var cols = lab() ? DCOLS : DCOLS.concat(['ReferredStatus']);
        $id('grdBody').innerHTML = S.detail.map(function (r, i) {
            var h = '<tr data-i="' + i + '"' + (i === S.updIndex || i === CUR.grd ? ' class="is-current"' : '') + '>';
            if (!lab()) h += '<td class="win-cell-btn"><button type="button" class="win-x" data-del="' + i + '" title="Delete">X</button></td>';
            cols.forEach(function (c) {
                var v = r[c];
                if (c === 'MTons') h += '<td class="num">' + esc(fmt(v)) + '</td>';
                else if (c === 'Lot' && !lab()) h += '<td><a class="win-code" data-edit-detail="' + i + '" href="javascript:void(0)">' + esc(v) + '</a></td>';
                else h += '<td>' + esc(v) + '</td>';
            });
            return h + '</tr>';
        }).join('');
        $id('grdFoot').innerHTML = sumRow(cols, S.detail, ['MTons'], lab() ? 0 : 1);
    }

    function renderParams() {
        var dis = lab();
        $id('grdParametersBody').innerHTML = S.params.map(function (r, i) {
            return '<tr data-i="' + i + '"><td class="ctr"><input type="checkbox" data-pchk="' + i + '"' + (r.Checked ? ' checked' : '') + (dis ? ' disabled' : '') + '/></td>'
                + '<td>' + esc(r.Parameter) + '</td>'
                + '<td class="win-editable"><input type="text" data-prem="' + i + '" value="' + esc(r.Remarks) + '"' + (dis ? ' disabled' : '') + '/></td></tr>';
        }).join('');
        $id('grdParametersFoot').innerHTML = S.params.length ? '<tr><td class="lbl" colspan="3">' + S.params.length + '</td></tr>' : '';
        var all = $id('grdParametersAll'); if (all) { all.checked = S.params.length > 0 && S.params.every(function (p) { return p.Checked; }); all.disabled = dis; }
    }

    var SCOLS = ['ThirdPartyType', 'NoOfSample', 'SampleWeight_Kg', 'TotalWeight_Kg', 'SealNo'];
    function renderSampling() {
        var ed = lab();   /* 857: panel9 Enabled = false - read only */
        $id('grdLotSamplingBody').innerHTML = S.sampling.map(function (r, i) {
            var h = '<tr data-i="' + i + '">';
            if (ed) h += '<td class="win-cell-btn"><button type="button" class="win-x" data-sdel="' + i + '" title="Delete">X</button></td>'
                + '<td class="win-cell-btn"><button type="button" class="win-x" style="color:#004d40" data-sadd="' + i + '" title="Add">+</button></td>';
            SCOLS.forEach(function (c) {
                var v = r[c];
                var editable = ed && (c === 'NoOfSample' || c === 'SampleWeight_Kg' || c === 'SealNo');
                if (editable) h += '<td class="win-editable"><input type="text"' + (c === 'SealNo' ? '' : ' class="num" data-guard="decimal"') + ' data-si="' + i + '" data-sc="' + c + '" value="' + esc(str(v)) + '"/></td>';
                else if (c === 'NoOfSample' || c === 'SampleWeight_Kg' || c === 'TotalWeight_Kg') h += '<td class="num">' + esc(fmt(v)) + '</td>';
                else h += '<td>' + esc(v) + '</td>';
            });
            return h + '</tr>';
        }).join('');
        $id('grdLotSamplingFoot').innerHTML = sumRow(SCOLS, S.sampling, ['NoOfSample', 'SampleWeight_Kg', 'TotalWeight_Kg'], ed ? 2 : 0);
    }

    function renderAll() { renderDetail(); renderParams(); renderSampling(); }

    /* AddPartyTypeInLotSampling(types): append every type not already present (all of them when the grid is empty). */
    function addPartyTypes(types) {
        var have = {};
        S.sampling.forEach(function (r) { have[netI(r.ThirdPartyTypeId)] = true; });
        var empty = S.sampling.length === 0;
        (types || []).forEach(function (t) {
            var id = netI(col(t, 'ThirdPartyTypeId'));
            if (empty || !have[id]) S.sampling.push({ Id: 0, ThirdPartyTypeId: id, ThirdPartyType: str(col(t, 'ThirdPartyType')), NoOfSample: 0, SampleWeight_Kg: 0, TotalWeight_Kg: 0, SealNo: '', Remarks: '' });
        });
        renderSampling();
    }

    // ------------------------------------------------------------------------------ analysis group

    /** CmbRequiredAnalysisGroup_Leave. */
    function analysisGroupLeave(silent) {
        var id = netI(val('CmbRequiredAnalysisGroup'));
        if (id <= 0) return Promise.resolve();
        if (S.analysisGroupId > 0 && id !== S.analysisGroupId) {
            if (!silent && !ask('You are Changing Analysis group.Parameters Data Will be Reset.are you Sure to proceed?')) { setVal('CmbRequiredAnalysisGroup', S.analysisGroupId); return Promise.resolve(); }
            S.params = [];
        }
        if (id === S.analysisGroupId && S.params.length) return Promise.resolve();
        S.analysisGroupId = id;
        return getJson(API + '/group-parameters?groupId=' + id).then(function (rows) {
            (rows || []).forEach(function (r) {
                var pid = netI(col(r, 'ParameterId'));
                if (!S.params.some(function (p) { return netI(p.ParameterId) === pid; }))
                    S.params.push({ Id: 0, ParameterId: pid, Parameter: str(col(r, 'Parameter')), Remarks: '', Checked: false });
            });
            renderParams();
        }).catch(function (e) { box(e.message); });
    }

    // ------------------------------------------------------------------------------ 857 detail panel cascades

    /** CmbCustomer_Leave. */
    function customerLeave() {
        var cust = netI(val('CmbCustomer'));
        var p = Promise.resolve();
        if (cust > 0) {
            p = getJson(API + '/contracts?customerId=' + cust + '&recId=' + S.recId).then(function (rows) {
                L.contracts = rows || [];
                bind('cmbContractNo', L.contracts, 'Id', 'LcOrderNo');
                if (L.contracts.length) return contractLeave(true);
            });
        }
        return p.then(function () { return schedulesBind(netI(val('cmbContractNo')), cust); }).catch(function (e) { box(e.message); });
    }
    function contractRow() { var id = netI(val('cmbContractNo')); for (var i = 0; i < L.contracts.length; i++) if (netI(L.contracts[i].Id) === id) return L.contracts[i]; return null; }
    /** cmbContractNo_Leave (fromCustomer: called inside ContractBind - schedules are bound by the caller afterwards too). */
    function contractLeave(fromCustomer) {
        var cid = netI(val('cmbContractNo'));
        var p;
        if (cid !== 0) {
            p = getJson(API + '/invoices?contractId=' + cid).then(function (rows) {
                var keep = netI(val('CmbInvoiceNoDetail'));
                L.invoices = rows || [];
                bind('CmbInvoiceNoDetail', L.invoices, 'InvoiceId', 'InvoiceNo');
                if (keep > 0 && !L.invoices.some(function (r) { return netI(r.InvoiceId) === keep; })) setVal('CmbInvoiceNoDetail', '0');
                var r = contractRow();
                var m = r ? netD(r.BalanceMton) : 0;
                /* Mtons.ToString("#,##.###") - an empty string for 0 */
                setText('txtMtons', m === 0 ? '' : fmt(m));
            });
        } else {
            L.invoices = []; bind('CmbInvoiceNoDetail', [], 'InvoiceId', 'InvoiceNo');
            p = Promise.resolve();
        }
        return p.then(function () { return schedulesBind(cid, netI(val('CmbCustomer'))); }).catch(function (e) { box(e.message); });
    }
    /** BindGetContractSchedulesByContactID(contractId, customerId). */
    function schedulesBind(cid, cust) {
        if (!(cid > 0 && cust > 0)) { L.schedules = []; bind('CmbScheduleNo', [], 'ContractScheduleId', 'ContractScheduleNo'); return Promise.resolve(); }
        return getJson(API + '/schedules?contractId=' + cid + '&customerId=' + cust).then(function (rows) {
            var keep = netI(val('CmbScheduleNo'));
            L.schedules = rows || [];
            bind('CmbScheduleNo', L.schedules, 'ContractScheduleId', 'ContractScheduleNo');
            if (keep > 0 && !L.schedules.some(function (r) { return netI(r.ContractScheduleId) === keep; })) setVal('CmbScheduleNo', '0');
            /* dt.Rows.Count == 2 (blank row + one schedule) -> Rows[1].Activate() and CmbScheduleNo_Leave */
            if (L.schedules.length === 1) { setVal('CmbScheduleNo', L.schedules[0].ContractScheduleId); scheduleLeave(); }
        });
    }
    /** CmbScheduleNo_Leave: Cells[2] (ExImInvoiceId) selects the invoice; Cells[4] (MTons) through Conversion.ToInt fills M.Tons when it was not typed. */
    function scheduleLeave() {
        var sid = netI(val('CmbScheduleNo'));
        if (sid <= 0) return;
        var r = null; L.schedules.forEach(function (x) { if (netI(x.ContractScheduleId) === sid) r = x; });
        if (!r) return;
        var invId = netI(r.ExImInvoiceId);
        var m = Math.round(netD(r.MTons));   /* Conversion.ToInt - integer */
        if (!S.mtonsTyped && S.updIndex === -1) setText('txtMtons', fmt(m));
        if (L.invoices.length) {
            if (L.invoices.some(function (x) { return netI(x.InvoiceId) === invId; })) setVal('CmbInvoiceNoDetail', invId);
            else setVal('CmbInvoiceNoDetail', '0');
        }
    }
    /** CmbInvoiceNoDetail_Leave: Cells[3] (ContractScheduleId) selects the schedule. */
    function invoiceLeave() {
        var iid = netI(val('CmbInvoiceNoDetail'));
        if (iid <= 0) return;
        var r = null; L.invoices.forEach(function (x) { if (netI(x.InvoiceId) === iid) r = x; });
        if (!r) return;
        var sid = netI(r.ContractScheduleId);
        if (L.schedules.some(function (x) { return netI(x.ContractScheduleId) === sid; })) setVal('CmbScheduleNo', sid);
        else setVal('CmbScheduleNo', '0');
    }

    /**
     * DetailFormValidation - reproduced literally: when a contract is selected the desktop either throws
     * ("Entered MTon ... cannot exceed the contract balance ...") or returns false WITHOUT a message, so
     * a sub-lot is only added when no contract is chosen (desktop quirk, kept).
     */
    function detailValidation() {
        var sid = netI(val('CmbScheduleNo'));
        if (sid > 0 && L.invoices.some(function (r) { return netI(r.ContractScheduleId) === sid; }) && netI(val('CmbInvoiceNoDetail')) === 0) {
            box('Invoice No field is required'); focus('CmbInvoiceNoDetail'); return false;
        }
        if (netD(val('txtMtons').trim()) === 0) { box('M.Tons field is required'); focus('txtMtons'); return false; }
        var cid = netI(val('cmbContractNo'));
        var row = contractRow();
        if (cid > 0 && row) {
            var balance = netD(row.BalanceMton);
            var raw = val('txtMtons');
            if (!/^\s*-?[\d,]*\.?\d+\s*$/.test(raw)) { focus('txtMtons'); throw new Error('Invalid MTon value. Please enter a valid number.'); }
            var entered = netD(raw);
            if (entered > balance) {
                focus('txtMtons');
                throw new Error('Entered MTon (' + entered.toFixed(3).replace(/\B(?=(\d{3})+(?!\d))/g, ',') + ') cannot exceed the contract balance (' + balance.toFixed(3).replace(/\B(?=(\d{3})+(?!\d))/g, ',') + ').\n\nContract: ' + selText('cmbContractNo') + '\nPlease enter a value within the allowed limit.');
            }
            return false;
        }
        return true;
    }
    function detailRowFromPanel() {
        var inv = netI(val('CmbInvoiceNoDetail')), sch = netI(val('CmbScheduleNo'));
        return {
            Lot: val('txtSubLot'), CustomerId: netI(val('CmbCustomer')), Customer: selText('CmbCustomer'),
            ContractId: netI(val('cmbContractNo')), ContractNo: selText('cmbContractNo'),
            ContractScheduleId: sch, ContractScheduleNo: sch ? selText('CmbScheduleNo') : '',
            InvoiceId: inv, InvoiceNo: inv ? selText('CmbInvoiceNoDetail') : '',
            MTons: netD(val('txtMtons')), SealNoForBuyer: val('txtSealNoForBuyer'), SealNoForShipper: val('txtSealNoForShipper'),
            Remarks: val('txtDetailRemarks')
        };
    }
    /** btnAdd_Click. */
    function btnAdd() {
        try { if (!detailValidation()) return; } catch (e) { box(e.message); return; }
        var r = detailRowFromPanel();
        r.Id = 0; r.ReferredStatusId = 0; r.ReferredStatus = 'Not Referred';
        S.detail.push(r);
        renderDetail();
        resetDetail();
        focus('txtSubLot');
        return contractsInformation();
    }
    /** btnUpdateDetail_Click. */
    function btnUpdateDetail() {
        try { if (!detailValidation()) return; } catch (e) { box(e.message); return; }
        var r = S.detail[S.updIndex]; if (!r) return;
        var n = detailRowFromPanel();
        for (var k in n) if (Object.prototype.hasOwnProperty.call(n, k)) r[k] = n[k];
        resetDetail();
        renderDetail();
        focus('txtSubLot');
        return contractsInformation();
    }
    /** ResetDetail. */
    function resetDetail() {
        S.updIndex = -1; S.mtonsTyped = false;
        setText('txtSubLot', '');
        setVal('CmbCustomer', '0');
        L.contracts = []; bind('cmbContractNo', [], 'Id', 'LcOrderNo');
        L.invoices = []; bind('CmbInvoiceNoDetail', [], 'InvoiceId', 'InvoiceNo');
        L.schedules = []; bind('CmbScheduleNo', [], 'ContractScheduleId', 'ContractScheduleNo');
        ['txtMtons', 'txtSealNoForBuyer', 'txtSealNoForShipper', 'txtDetailRemarks'].forEach(function (id) { setText(id, ''); });
        show('btnAdd', true); show('btnUpdateDetail', false); show('btnCancelDetial', false);
        renderDetail();
    }
    /** grd_DoubleClick - load the sub-lot into the panel for Update. */
    function grdEdit(i) {
        var r = S.detail[i]; if (!r) return;
        S.updIndex = i;
        setText('txtSubLot', r.Lot);
        setVal('CmbCustomer', r.CustomerId);
        return customerLeave().then(function () {
            setVal('cmbContractNo', r.ContractId);
            setVal('CmbScheduleNo', r.ContractScheduleId);
            return contractLeave();
        }).then(function () {
            if (netI(r.InvoiceId) > 0) setVal('CmbInvoiceNoDetail', r.InvoiceId);
            setText('txtMtons', fmt(r.MTons));
            setText('txtSealNoForBuyer', r.SealNoForBuyer);
            setText('txtSealNoForShipper', r.SealNoForShipper);
            setText('txtDetailRemarks', r.Remarks);
            show('btnAdd', false); show('btnUpdateDetail', true); show('btnCancelDetial', true);
            renderDetail();
            focus('txtSubLot');
        });
    }
    /** grd_ColumnButtonClick (X): referred -> refuse; saved -> RemoveDetailIds += Id + ","; then DeleteDetailRow. */
    function grdDelete(i) {
        var r = S.detail[i]; if (!r) return;
        if (netI(r.Id) > 0 && netI(r.ReferredStatusId) === 1) { box('Record cannot be delete because record has been referred in delivery order'); return; }
        if (netI(r.Id) > 0 && netI(r.ReferredStatusId) === 0) S.removeIds += netI(r.Id) + ',';
        deleteDetailRow(i);
    }
    function deleteDetailRow(i) {
        if (S.updIndex !== -1) { box('Please Reset the Detail'); return; }
        S.detail.splice(i, 1);
        CUR.grd = -1;
        renderDetail();
    }
    /** GetContractsInformation - the right-hand contract balance grid. */
    function contractsInformation() {
        if (!S.detail.length) return Promise.resolve();
        var ids = [];
        S.detail.forEach(function (r) { var c = netI(r.ContractId); if (c && ids.indexOf(c) < 0) ids.push(c); });
        return getJson(API + '/contract-info?ids=' + encodeURIComponent(ids.join(','))).then(function (rows) {
            rows = rows || [];
            var cols = ['ContractNo', 'Mton', 'ShippedMton', 'BalanceMton'];
            $id('grdContractInfoBody').innerHTML = rows.map(function (r) {
                return '<tr>' + cols.map(function (c) { return c === 'ContractNo' ? '<td>' + esc(col(r, c)) + '</td>' : '<td class="num">' + esc(fmt(col(r, c))) + '</td>'; }).join('') + '</tr>';
            }).join('');
            $id('grdContractInfoFoot').innerHTML = sumRow(cols, rows, ['Mton', 'ShippedMton', 'BalanceMton'], 0);
        }).catch(function (e) { box(e.message); });
    }
    function clearContractInfo() { if ($id('grdContractInfoBody')) { $id('grdContractInfoBody').innerHTML = ''; $id('grdContractInfoFoot').innerHTML = ''; } }

    // ------------------------------------------------------------------------------ sampling (858)

    /** grdLotSampling_CellUpdated: NoOfSample / SampleWeight_Kg -> TotalWeight_Kg; negatives reset the row. */
    function samplingCell(e) {
        var inp = e.target.closest('input[data-sc]'); if (!inp) return;
        var r = S.sampling[+inp.getAttribute('data-si')]; if (!r) return;
        var c = inp.getAttribute('data-sc');
        if (c === 'SealNo') { r.SealNo = inp.value; return; }
        r[c] = c === 'NoOfSample' ? netI(inp.value) : netD(inp.value);
        if (e.type !== 'change') return;
        var n = netD(r.NoOfSample), w = netD(r.SampleWeight_Kg);
        if (n < 0 || w < 0) { box('No of Sample and Sample Weight cannot be negative.'); r.NoOfSample = 0; r.SampleWeight_Kg = 0; r.TotalWeight_Kg = 0; }
        else r.TotalWeight_Kg = (n === 0 || w === 0) ? 0 : n * w;
        renderSampling();
    }
    /** AddRowInDetailGridLotSampling: copy the row (Id 0 when in update mode). */
    function samplingAdd(i) {
        var r = S.sampling[i]; if (!r) return;
        var c = {}; for (var k in r) if (Object.prototype.hasOwnProperty.call(r, k)) c[k] = r[k];
        if (!$id('btnUpdate').classList.contains('is-hidden') && !$id('btnUpdate').disabled) c.Id = 0;
        S.sampling.push(c);
        renderSampling();
    }
    function samplingDel(i) { S.sampling.splice(i, 1); renderSampling(); }
    /** InstertMtonInHeader (858): header M.Tons = Σ detail M.Tons (the sampling grid key handlers call it). */
    function mtonInHeader() {
        if (!lab()) return;
        var t = 0; S.detail.forEach(function (r) { t += netD(r.MTons); });
        setText('txtHeaderMtons', fmt(t));
    }
    /** BtnRefreshRowsInSamplingGrid_Click: ThirdPartyType.FormHistory(..., ActiveOnly: true). */
    function refreshSamplingRows(btn) {
        return busy(btn, function () {
            return getJson(API + '/third-party-types?activeOnly=true').then(addPartyTypes).catch(function (e) { box(e.message); });
        });
    }

    // ------------------------------------------------------------------------------ ETA

    /** ETADate: ETA = Dispatched + Transit Days (or = Dispatched when the box is empty). */
    function etaDate() {
        var t = val('txtTransitDays').trim();
        var d = val('txtSampleDispatchedDate') || today();
        setText('txtSampleETADesDate', t !== '' ? addDays(d, netI(t)) : d);
    }

    // ------------------------------------------------------------------------------ ReadById / Reset / Insert

    /** ReadById(ID). */
    function readById(id) {
        return reset(true).then(function () {
            return getJson(API + '/by-id?id=' + id);
        }).then(function (d) {
            if (!d) return;
            var h = d.header || {};
            S.recId = id;
            tab('tpiForm');
            if ($id('btnsave')) show('btnsave', false);
            show('btnUpdate', true);
            /* RadAll.Checked = true -> RadAll_CheckedChanged -> TackingNoBind(TackingDbCall()) */
            setRadio('tpiStatus', 'all');
            trackingReload();
            setText('CmbLotTrackingNo', h.LotRefNo);
            setText('txtRequestLodgDate', isoDate(h.RequestDate) || today());
            setText('txtRequestReferenceNo', h.RequestRefNo);
            setVal('CmbRequestedBy', h.RequestedById);
            setVal('CmbMedium', h.RequestMediumId);
            if (netI(h.JobLotId) > 0) setVal('CmbJobLot', h.JobLotId);
            setText('txtExporterLotNo', h.ExporterLotRefNo);
            setVal('CmbItemMain', h.ItemId);
            setText('txtHeaderMtons', fmt(h.QtyKgs, 2));
            setVal('CmbInspectionAgency', h.InspectionAgencyId);
            setVal('CmbSamplingResponsibity', h.SamplingResponsibilityId);
            setVal('CmbCountryOfOrigin', h.CountryOfOriginId);
            setVal('CmbCountryOfInspection', h.CountryOfInspectionId);
            setText('txtCourierTrackingNo', h.CourierTrackingNo);
            dSet('txtSampleDispatchedDate', isoDate(h.SampleDispatchedDate));
            setText('txtTransitDays', str(netI(h.TransitDays)));
            dSet('txtSampleATADesDate', isoDate(h.SampleATADestinationDate));
            setText('txtInspectionRemarks', h.InspectionRemarks);
            setText('txtReportCertificateNo', h.ReportRefNo);
            dSet('txtReportDate', isoDate(h.ReportDate));
            dSet('txtDateOfInspection', isoDate(h.DateOfInspection));
            dSet('txtResultDate', isoDate(h.ResultDate));
            if (netI(h.ExImFarmingNTradeId) > 0) setVal('CmbFarmingNTrade', h.ExImFarmingNTradeId);
            /* CmbResultStatus.Text = ReportStatus */
            var rs = $id('CmbResultStatus'), found = false;
            for (var i = 0; i < rs.options.length; i++) if (rs.options[i].text === str(h.ReportStatus)) { rs.selectedIndex = i; found = true; break; }
            if (!found && !lab()) rs.value = '0';
            refreshCombos();
            setVal('CmbResultRemarks', h.ReportResultRemarksId);
            setText('CmbLotCurrentStage', h.LotCurrentStage);
            setVal('CmbRequiredAnalysisGroup', h.RequiredAnalysisId);
            dSet('txtStockReservedDate', isoDate(h.StockReservedDate));
            dSet('txtSampleTakenDate', isoDate(h.SampleTakenDate));
            dSet('txtStockSealedDate', isoDate(h.StockRSealedDate));
            setVal('CmbSampleHandOver', h.SampleHandedOverId);
            dSet('txtHandOverDate', isoDate(h.SampleHandedOverDate));
            setText('txtInstructionRemarks', h.LotInstructionsOrRemarks);
            setText('txtLabRemarks', h.LabRemarks);
            etaDateFromSaved(h);
            S.detail = (d.details || []).map(function (r) { return r; });
            S.sampling = (d.sampling || []).map(function (r) { return r; });
            var saved = {}; (d.parameters || []).forEach(function (p) { saved[netI(p.ParameterId)] = p; });
            renderDetail();
            return Promise.all([
                getJson(API + '/third-party-types?activeOnly=false').then(addPartyTypes),
                analysisGroupLeave(true).then(function () {
                    S.params.forEach(function (p) {
                        var sp = saved[netI(p.ParameterId)];
                        if (sp) { p.Checked = true; p.Remarks = str(sp.Remarks); } else { p.Checked = false; p.Remarks = ''; }
                    });
                    renderParams();
                })
            ]).then(function () {
                if (!lab()) return contractsInformation();
            }).then(function () { focus('txtRequestLodgDate'); });
        }).catch(function (e) { box(e.message); });
    }
    function etaDateFromSaved(h) { if (isoDate(h.SampleETADestinationDate)) setText('txtSampleETADesDate', isoDate(h.SampleETADestinationDate)); else etaDate(); }

    /** Reset. */
    function reset(keepTracking) {
        S.recId = 0; S.analysisGroupId = 0; S.removeIds = ''; S.exporterTyped = '';
        setText('CmbLotTrackingNo', '');
        ['txtRequestReferenceNo', 'txtExporterLotNo', 'txtCourierTrackingNo', 'txtTransitDays', 'txtInspectionRemarks', 'txtReportCertificateNo', 'txtLabRemarks'].forEach(function (id) { setText(id, ''); });
        setVal('CmbItemMain', '0');
        $id('CmbResultStatus').value = lab() ? ($id('CmbResultStatus').options.length ? $id('CmbResultStatus').options[0].value : '') : '0';
        setVal('CmbRequiredAnalysisGroup', lab() ? '' : '0');
        S.params = [];
        setVal('CmbSampleHandOver', '0');
        ['txtSampleDispatchedDate', 'txtSampleETADesDate', 'txtSampleATADesDate', 'txtReportDate', 'txtDateOfInspection', 'txtResultDate',
         'txtStockReservedDate', 'txtSampleTakenDate', 'txtStockSealedDate', 'txtHandOverDate'].forEach(function (id) { var c = $id(id + 'Chk'); if (c) c.checked = false; dState(id); });
        if (!lab()) { setVal('CmbFarmingNTrade', '0'); setText('txtInstructionRemarks', ''); resetDetail(); }
        S.detail = []; S.sampling = []; CUR.grd = -1;
        renderAll();
        if ($id('btnsave')) show('btnsave', !lab());
        show('btnUpdate', false);
        /* CmbResultStatus.Rows[2].Activate() */
        resultStatusRow(2);
        clearContractInfo();
        var p = keepTracking ? Promise.resolve() : trackingReload();
        return p.then(function () { focus('txtRequestLodgDate'); });
    }

    function payload() {
        var f = netI(val('CmbFarmingNTrade')), fr = null;
        L.farming.forEach(function (x) { if (netI(x.Id) === f) fr = x; });
        var b = {
            recId: S.recId,
            LotRefNo: val('CmbLotTrackingNo'),
            RequestDate: val('txtRequestLodgDate'),
            RequestRefNo: val('txtRequestReferenceNo'),
            RequestedById: netI(val('CmbRequestedBy')), RequestedBy: selText('CmbRequestedBy'),
            RequestMediumId: netI(val('CmbMedium')), RequestMedium: selText('CmbMedium'),
            JobLotId: netI(val('CmbJobLot')),
            ExporterLotRefNo: val('txtExporterLotNo'),
            ItemId: netI(val('CmbItemMain')), ProductSpecification: selText('CmbItemMain'),
            QtyKgs: netD(val('txtHeaderMtons')),
            InspectionAgencyId: netI(val('CmbInspectionAgency')),
            SamplingResponsibilityId: netI(val('CmbSamplingResponsibity')),
            CountryOfOriginId: netI(val('CmbCountryOfOrigin')),
            CountryOfInspectionId: netI(val('CmbCountryOfInspection')), CountryOfInspectionText: selText('CmbCountryOfInspection'),
            CourierTrackingNo: val('txtCourierTrackingNo'),
            TransitDays: netI(val('txtTransitDays')),
            InspectionRemarks: val('txtInspectionRemarks'),
            ReportRefNo: val('txtReportCertificateNo'),
            ResultStatusId: netI(val('CmbResultStatus')), ReportStatus: selText('CmbResultStatus'),
            ReportResultRemarksId: netI(val('CmbResultRemarks')),
            RequiredAnalysisId: netI(val('CmbRequiredAnalysisGroup')),
            SampleHandedOverId: netI(val('CmbSampleHandOver')),
            LabRemarks: val('txtLabRemarks'),
            ExImFarmingNTradeId: f, ExImFarmingTypeId: fr ? netI(fr.ExImFarmingTypeId) : 0, ExImTradeTypeId: fr ? netI(fr.ExImTradeTypeId) : 0,
            LotInstructionsOrRemarks: val('txtInstructionRemarks'),
            removeDetailIds: S.removeIds,
            details: S.detail, parameters: S.params, sampling: S.sampling
        };
        var dates = { SampleDispatchedDate: 'txtSampleDispatchedDate', SampleATADestinationDate: 'txtSampleATADesDate', ReportDate: 'txtReportDate',
            DateOfInspection: 'txtDateOfInspection', ResultDate: 'txtResultDate', StockReservedDate: 'txtStockReservedDate',
            SampleTakenDate: 'txtSampleTakenDate', StockRSealedDate: 'txtStockSealedDate', SampleHandedOverDate: 'txtHandOverDate' };
        Object.keys(dates).forEach(function (k) { b[k] = val(dates[k]); b[k + 'Checked'] = dChecked(dates[k]); });
        return b;
    }

    /** FormValidation repeated client-side so the message comes before the confirm, as on the desktop (the server repeats it). */
    function formValidation() {
        var b = payload();
        var chk = [
            [!str(b.LotRefNo).trim(), 'Lot Reference/tracking No is required.', 'CmbLotTrackingNo'],
            [b.RequestedById === 0, 'Requested By is required.', 'CmbRequestedBy'],
            [b.RequestMediumId === 0, 'Request Medium is required.', 'CmbMedium'],
            [b.ItemId === 0, 'Item selection is required.', 'CmbItemMain'],
            [b.QtyKgs <= 0, 'Quantity in M.Tons must be greater than 0.', 'txtHeaderMtons'],
            [b.InspectionAgencyId === 0, 'Inspection Agency is required.', 'CmbInspectionAgency'],
            [b.SamplingResponsibilityId === 0, 'Sampling Responsibility is required.', 'CmbSamplingResponsibity'],
            [b.CountryOfOriginId === 0, 'Country of Origin is required.', 'CmbCountryOfOrigin'],
            [!lab() && b.ResultStatusId === 0, 'Result Status is required.', 'CmbResultStatus']
        ];
        for (var i = 0; i < chk.length; i++) if (chk[i][0]) { box(chk[i][1]); focus(chk[i][2]); return null; }
        function d(k) { return b[k + 'Checked'] ? b[k] : ''; }
        var disp = d('SampleDispatchedDate'), ata = d('SampleATADestinationDate'), taken = d('SampleTakenDate'), res = d('StockReservedDate'),
            sealed = d('StockRSealedDate'), hand = d('SampleHandedOverDate');
        if (disp && ata && disp >= ata) { box("'Sample Dispatched Date' cannot be greater than or Equal 'Sample ATA Destination Date'"); focus('txtSampleDispatchedDate'); return null; }
        if (disp && taken && taken > disp) { box("'Sample Dispatched Date' cannot be lesser than 'Sample Taken Date'"); focus('txtSampleDispatchedDate'); return null; }
        if (lab() && !res && !taken && !sealed) { box("'Stock Reserved Date' or  'Sample Taken Date' or 'Stock Sealed Date' is Required"); focus('txtStockReservedDate'); return null; }
        if (res && taken && res > taken) { box("'Stock Reserved Date' cannot be greater than 'Sample Taken Date'"); focus('txtStockReservedDate'); return null; }
        if (sealed && taken && sealed < taken) { box("'Sample Taken Date' cannot be greater than 'Stock Sealed Date'"); focus('txtSampleTakenDate'); return null; }
        if (sealed && hand && sealed > hand) { box("'Stock Sealed Date' cannot be greater than 'Sample hand over Date'"); focus('txtStockSealedDate'); return null; }
        return b;
    }
    /** Insert(). */
    function insert(btn) {
        return busy(btn, function () {
            var b = formValidation();
            if (!b) return Promise.resolve();
            if (!ask(S.recId === 0 ? 'Are you sure to Save?' : 'Are you sure to Update?')) return Promise.resolve();
            return postJson(API + '/save', b).then(function (d) {
                box((d && d.message) || (S.recId === 0 ? 'Record Saved Successfully' : 'Record Update Successfully'));
                return reset(false);
            }).catch(function (e) { box(e.message); });
        });
    }
    /** btnsave_Click: RecId = 0 then Insert. */
    function btnsave(btn) { S.recId = 0; return insert(btn); }
    /** btnUpdate_Click. */
    function btnUpdate(btn) { if (S.recId === 0) { box('RecId not Found...'); return Promise.resolve(); } return insert(btn); }
    /** btnNew_Click. */
    function btnNew() { return reset(false); }
    /** btnRefresh_Click. */
    function btnRefresh(btn) {
        return busy(btn, function () {
            return getJson(API + '/refresh').then(function (d) { bindCombos(d || {}); }).catch(function (e) { box(e.message); });
        });
    }
    /** btnPrint_Click: GenerateReport(RecId) - CommonServices.ThirdPartyInspectionSlip514. */
    function print(btn) {
        if (S.recId <= 0) { box('No Record Found'); return; }
        return slip514(S.recId, btn);
    }
    function slip514(id, btn) {
        if (!global.CrystalPrint) { box('Printing is not available on this page.'); return; }
        return global.CrystalPrint.open('exp-514', { id: id }, btn);
    }
    /** BtnStockReservedForm_Click (857): frmStockReservedAgainstThirdPartyInspection(TrackingId) -> /export/stock-reserved-against-tpi. */
    function stockReservedForm() { global.open('/export/stock-reserved-against-tpi' + (S.recId > 0 ? '?trackingId=' + S.recId : ''), '_blank'); }

    /** BtnPrintStockReserved_Click (857): 514_01_InventoryStockReservedSlip.rpt. */
    function printStockReserved(btn) {
        if (S.recId === 0) { box('No Record Found For Display'); return; }
        if (!global.CrystalPrint) { box('Printing is not available on this page.'); return; }
        return global.CrystalPrint.open('exp-514-01', { id: S.recId }, btn);
    }
    function notPorted(name) { box(name + ' is a desktop pop-up form that is not available in the web version.'); }
    function openRoute(url) { global.open(url, '_blank'); }

    /** MakeShortCutKeys. */
    function shortcuts() {
        var rows = ['Ctrl+S  For Save', 'Ctrl+U  For Update', 'Ctrl+E  For Close', 'Ctrl+R  For Refresh', 'Ctrl+N  For New', 'Ctrl+P  For Print',
            'Ctrl+F5  For Focus on LotTrackingNo', 'Ctrl+F10  For Open Attachments', 'Ctrl+T  For Tab Transfer', 'Ctrl+alt  To Show ShortCut Keys Form'];
        if (!lab()) rows = rows.concat(['Ctrl+ArrowDown  For Focus On Detail Grid', 'Ctrl+ArrowUp  For Focus On SubLot in Detail Box']);
        rows = rows.concat(['Ctrl+Enter  When Focus On Any Grid For Update Record', "Ctrl+Space  When Focus On Any Grid To Call Function's On Button Or Link"]);
        box(rows.join('\n'));
    }

    // ------------------------------------------------------------------------------ history

    var HCOLS = ['LotRefNo', 'RequestDate', 'RequestRefNo', 'RequestedByName', 'RequestedBy', 'RequestMedium', 'ExporterLotRefNo', 'ItemName', 'MTon',
        'InspectionAgency', 'CountryOfOrigin', 'CountryOfInspection', 'PlaceOfInspection', 'CourierTrackingNo', 'SampleDispatchedDate', 'TransitDays',
        'SampleETADestinationDate', 'SampleATADestinationDate', 'InspectionRemarks', 'ReportRefNo', 'ReportDate', 'DateOfInspection', 'ReportStatus',
        'LotCurrentStage', 'RequiredAnalysisGroup', 'StockReservedDate', 'SampleTakenDate', 'StockRSealedDate', 'SampleHandedOver', 'SampleHandedOverDate',
        'LabRemarks', 'SampleStatus', 'EntryUserName', 'EntryDate', 'ModifyUserName', 'ModifyDate', 'ApprovedUser', 'ApprovedDate', 'NoOfAttachments'];
    var HDATE = { RequestDate: 1, SampleDispatchedDate: 1, SampleETADestinationDate: 1, SampleATADestinationDate: 1, ReportDate: 1, DateOfInspection: 1,
        StockReservedDate: 1, SampleTakenDate: 1, StockRSealedDate: 1, SampleHandedOverDate: 1 };
    var HDT = { EntryDate: 1, ModifyDate: 1, ApprovedDate: 1 };
    function histRender() {
        show('tpiHistEditTh', PERM.Update);
        show('tpiHistPrintTh', PERM.Print);
        var lead = (PERM.Update ? 1 : 0) + (PERM.Print ? 1 : 0);
        $id('tpiHistBody').innerHTML = HIST.map(function (r, i) {
            var h = '<tr data-i="' + i + '"' + (i === CUR.hist ? ' class="is-current"' : '') + '>';
            if (PERM.Update) h += '<td class="win-cell-btn"><button type="button" class="win-edit" data-edit="' + i + '">Edit</button></td>';
            if (PERM.Print) h += '<td class="win-cell-btn"><button type="button" class="win-edit" data-print="' + i + '">Print</button></td>';
            HCOLS.forEach(function (c) {
                var v = col(r, c), t;
                if (c === 'MTon') t = fmt(v);
                else if (c === 'TransitDays') t = str(netI(v));
                else if (HDATE[c]) t = ddMMMyyyy(v);
                else if (HDT[c]) t = ddMMMHm(v);
                else t = str(v);
                if (c === 'LotRefNo') h += '<td><a class="win-code" data-open="' + i + '" href="javascript:void(0)">' + esc(t) + '</a></td>';
                else if (c === 'NoOfAttachments') h += '<td class="num"><a class="win-code" data-att="' + i + '" href="javascript:void(0)">' + esc(netI(v)) + '</a></td>';
                else h += '<td' + (c === 'MTon' || c === 'TransitDays' ? ' class="num"' : '') + '>' + esc(t) + '</td>';
            });
            return h + '</tr>';
        }).join('');
        $id('tpiHistFoot').innerHTML = sumRow(HCOLS, HIST, ['MTon'], lead);
        show('tpiHistEmpty', HIST.length === 0);
        $id('grdDetailBody').innerHTML = ''; $id('grdDetailFoot').innerHTML = '';
    }
    /** btnShow_Click -> HistoryFill. */
    function historyShow(btn) {
        return busy(btn, function () {
            var f = {
                dateBy: radio('tpiDateBy'),
                fromChecked: $id('FromDateHistoryChk').checked, fromDate: val('FromDateHistory'),
                toChecked: $id('ToDateHistoryChk').checked, toDate: val('ToDateHistory'),
                trackingId: netI(val('CmbTrackingNoHistory')), itemId: netI(val('CmbItemHistory')),
                status: radio('tpiHistStatus')
            };
            return postJson(API + '/history', f).then(function (rows) { HIST = rows || []; CUR.hist = -1; histRender(); })
                .catch(function (e) { box(e.message); });
        });
    }
    /** btnResetHistory_Click: both dates today, combos cleared, Request Date radio. */
    function historyReset() {
        setText('FromDateHistory', today()); setText('ToDateHistory', today());
        setVal('CmbTrackingNoHistory', '0'); setVal('CmbItemHistory', '0');
        HIST = []; CUR.hist = -1; histRender(); show('tpiHistEmpty', false);
        setRadio('tpiDateBy', 'doc');
        focus('FromDateHistory');
    }
    function historyComboFill(d) {
        if ((d.historyItems || []).length + (d.historyTrackingNos || []).length === 0) return;   /* dt.Rows.Count <= 0 -> return */
        bind('CmbTrackingNoHistory', d.historyTrackingNos, 'Id', 'name');
        bind('CmbItemHistory', d.historyItems, 'Id', 'name');
    }
    /** BtnRefreshHistory_Click. */
    function historyRefresh(btn) {
        return busy(btn, function () { return getJson(API + '/history-combos').then(function (d) { historyComboFill(d || {}); }).catch(function (e) { box(e.message); }); });
    }
    /** DataGridHistory_SelectionChanged -> BindDetailByHistoryHeader. */
    var DDCOLS = ['Lot', 'Customer', 'ContractNo', 'ContractScheduleNo', 'InvoiceNo', 'MTons', 'SealNoForBuyer', 'SealNoForShipper', 'Remarks'];
    function historySelect(i) {
        CUR.hist = i;
        var r = HIST[i]; if (!r) return;
        getJson(API + '/history-detail?id=' + netI(col(r, 'Id'))).then(function (rows) {
            rows = rows || [];
            $id('grdDetailBody').innerHTML = rows.map(function (x) {
                return '<tr>' + DDCOLS.map(function (c) {
                    if (c === 'MTons') return '<td class="num">' + esc(fmt(x[c])) + '</td>';
                    /* format condition: InvoiceNo not null -> bold red */
                    if (c === 'InvoiceNo' && str(x[c])) return '<td style="color:red;font-weight:bold;">' + esc(x[c]) + '</td>';
                    return '<td>' + esc(x[c]) + '</td>';
                }).join('') + '</tr>';
            }).join('');
            $id('grdDetailFoot').innerHTML = sumRow(DDCOLS, rows, ['MTons'], 0);
        }).catch(function (e) { box(e.message); });
    }
    function historyOpen(i) {
        var r = HIST[i]; if (!r) return;
        readById(netI(col(r, 'Id')));
    }
    function historyAttachments(i) {
        var r = HIST[i]; if (!r) return;
        getJson(API + '/attachments?id=' + netI(col(r, 'Id'))).then(function (rows) {
            if (!rows || !rows.length) return;
            box(rows.map(function (a) { return str(a.AttachmentName) + '  |  ' + str(a.CustomName) + '  |  ' + ddMMMHm(a.EntryDate); }).join('\n'));
        }).catch(function (e) { box(e.message); });
    }

    // ------------------------------------------------------------------------------ wiring

    function on(id, ev, fn) { var e = $id(id); if (e) e.addEventListener(ev, fn); }

    document.addEventListener('DOMContentLoaded', function () {
        MODE = netI(document.body.getAttribute('data-tpi')) === 858 ? 858 : 857;
        API = MODE === 858 ? '/api/export/lab-against-third-party-inspection' : '/api/export/third-party-inspection';
        document.querySelectorAll('.win-tabs[data-tabs="tpi"] .win-tab').forEach(function (b) { b.addEventListener('click', function () { tab(b.getAttribute('data-tab')); }); });
        document.querySelectorAll('button[data-fullscreen]').forEach(function (b) {
            b.addEventListener('click', function () { var bx = $id(b.getAttribute('data-fullscreen')); if (bx) bx.classList.toggle('ex-fullscreen'); });
        });
        /* DateTimePicker check boxes */
        document.querySelectorAll('input[type=checkbox][id$="Chk"]').forEach(function (c) { var id = c.id.replace(/Chk$/, ''); c.addEventListener('change', function () { dState(id); }); dState(id); });
        on('CmbLotTrackingNo', 'change', trackingLeave);
        document.querySelectorAll('input[name="tpiStatus"]').forEach(function (r) { r.addEventListener('change', trackingReload); });   /* RadAll_CheckedChanged */
        document.querySelectorAll('input[name="tpiItemBy"]').forEach(function (r) { r.addEventListener('change', itemNameBind); });   /* rdSearchByName_CheckedChanged */
        on('txtTransitDays', 'input', etaDate);                      /* txtTransitDays_TextChanged */
        on('txtSampleDispatchedDate', 'change', etaDate);            /* txtSampleDispatchedDate_ValueChanged */
        on('CmbRequiredAnalysisGroup', 'change', function () { analysisGroupLeave(false); });
        /* CmbJobLot_Leave: Exporter Lot = Job Lot text when it was never typed or is empty */
        on('CmbJobLot', 'change', function () { if (S.exporterTyped === '' || val('txtExporterLotNo') === '') setText('txtExporterLotNo', selText('CmbJobLot')); });
        on('txtExporterLotNo', 'keypress', function () { S.exporterTyped = val('txtExporterLotNo') || ' '; });
        if (!lab()) {
            on('CmbCustomer', 'change', customerLeave);
            on('cmbContractNo', 'change', function () { contractLeave(false); });
            on('CmbScheduleNo', 'change', scheduleLeave);
            on('CmbInvoiceNoDetail', 'change', invoiceLeave);
            on('txtMtons', 'keypress', function () { S.mtonsTyped = true; });
            var gb = $id('grdBody');
            gb.addEventListener('click', function (e) {
                var del = e.target.closest('button[data-del]');
                if (del) { grdDelete(+del.getAttribute('data-del')); return; }
                var ed = e.target.closest('a[data-edit-detail]');
                if (ed) { grdEdit(+ed.getAttribute('data-edit-detail')); return; }
                var tr = e.target.closest('tr[data-i]'); if (tr) { CUR.grd = +tr.getAttribute('data-i'); gb.querySelectorAll('tr').forEach(function (x) { x.classList.toggle('is-current', x === tr); }); }
            });
            gb.addEventListener('dblclick', function (e) { var tr = e.target.closest('tr[data-i]'); if (tr) grdEdit(+tr.getAttribute('data-i')); });
        }
        /* parameters grid: selector column + Remarks */
        var pb = $id('grdParametersBody');
        pb.addEventListener('change', function (e) {
            var c = e.target.closest('input[data-pchk]'); if (c) { var p = S.params[+c.getAttribute('data-pchk')]; if (p) p.Checked = c.checked; renderParamsHeader(); }
        });
        pb.addEventListener('input', function (e) { var t = e.target.closest('input[data-prem]'); if (t) { var p = S.params[+t.getAttribute('data-prem')]; if (p) p.Remarks = t.value; } });
        on('grdParametersAll', 'change', function () { var on2 = $id('grdParametersAll').checked; S.params.forEach(function (p) { p.Checked = on2; }); renderParams(); });
        /* sampling grid (858 editable) */
        var sb = $id('grdLotSamplingBody');
        sb.addEventListener('input', samplingCell);
        sb.addEventListener('change', samplingCell);
        sb.addEventListener('click', function (e) {
            var d = e.target.closest('button[data-sdel]'); if (d) { samplingDel(+d.getAttribute('data-sdel')); return; }
            var a = e.target.closest('button[data-sadd]'); if (a) samplingAdd(+a.getAttribute('data-sadd'));
        });
        /* grdLotSampling_KeyDown: Ctrl+Delete removes, Ctrl+D copies; both recompute header M.Tons (InstertMtonInHeader) */
        sb.addEventListener('keydown', function (e) {
            var inp = e.target.closest('input[data-si]'); if (!inp) return;
            var i = +inp.getAttribute('data-si');
            if (e.ctrlKey && e.key === 'Delete') { e.preventDefault(); samplingDel(i); mtonInHeader(); }
            else if (e.ctrlKey && (e.key === 'd' || e.key === 'D')) { e.preventDefault(); samplingAdd(i); mtonInHeader(); }
        });
        /* history grid */
        var hb = $id('tpiHistBody');
        hb.addEventListener('click', function (e) {
            var ed = e.target.closest('button[data-edit]'); if (ed) { historyOpen(+ed.getAttribute('data-edit')); return; }
            var pr = e.target.closest('button[data-print]'); if (pr) { var r = HIST[+pr.getAttribute('data-print')]; if (r) slip514(netI(col(r, 'Id')), pr); return; }
            var op = e.target.closest('a[data-open]'); if (op) { historyOpen(+op.getAttribute('data-open')); return; }
            var at = e.target.closest('a[data-att]'); if (at) { historyAttachments(+at.getAttribute('data-att')); return; }
            var tr = e.target.closest('tr[data-i]'); if (!tr) return;
            hb.querySelectorAll('tr[data-i]').forEach(function (x) { x.classList.toggle('is-current', x === tr); });
            historySelect(+tr.getAttribute('data-i'));
        });
        hb.addEventListener('dblclick', function (e) { var tr = e.target.closest('tr[data-i]'); if (tr) historyOpen(+tr.getAttribute('data-i')); });
        /* frm..._KeyDown */
        document.addEventListener('keydown', function (e) {
            var k = (e.key || '').toLowerCase();
            if (e.key === 'Enter' && !e.ctrlKey && e.target && /^(INPUT|SELECT)$/.test(e.target.tagName) && e.target.type !== 'checkbox') {
                var f = Array.prototype.filter.call(document.querySelectorAll('input:not([type=hidden]):not(:disabled), select:not(:disabled), button:not(:disabled), textarea:not(:disabled)'), function (x) { return x.offsetParent !== null; });
                var i = f.indexOf(e.target);
                if (i >= 0 && i + 1 < f.length) { e.preventDefault(); f[i + 1].focus(); }
                return;
            }
            if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { e.preventDefault(); cancel(); return; }
            if (e.ctrlKey && e.altKey) { e.preventDefault(); shortcuts(); return; }
            if (e.ctrlKey && k === 'p' && PERM.Print) { e.preventDefault(); print($id('btnPrint')); return; }
            if (e.ctrlKey && k === 't') { e.preventDefault(); if (onHistory()) { tab('tpiForm'); focus('CmbLotTrackingNo'); } else { tab('tpiHistory'); } return; }
            if (e.ctrlKey && k === 'u' && !$id('btnUpdate').classList.contains('is-hidden') && S.recId > 0) { e.preventDefault(); btnUpdate($id('btnUpdate')); return; }
            if (!onHistory()) {
                if (e.ctrlKey && k === 's' && !lab() && !$id('btnsave').classList.contains('is-hidden')) { e.preventDefault(); btnsave($id('btnsave')); }
                if (e.ctrlKey && e.key === 'F5') { e.preventDefault(); focus('CmbLotTrackingNo'); }
                if (e.ctrlKey && e.key === 'F10') { e.preventDefault(); notPorted('Attachment'); }
                if (e.ctrlKey && k === 'n') { e.preventDefault(); btnNew(); }
                if (e.ctrlKey && k === 'r') { e.preventDefault(); btnRefresh($id('btnRefresh')); }
                if (e.ctrlKey && e.key === 'ArrowUp') { e.preventDefault(); focus(lab() ? 'txtStockReservedDate' : 'txtSubLot'); }
                if (e.ctrlKey && e.key === 'ArrowDown') { e.preventDefault(); var g = $id('grdBox'); if (g) g.scrollIntoView(); }
                return;
            }
            if (e.ctrlKey && e.key === 'Enter' && CUR.hist >= 0 && PERM.Update) { e.preventDefault(); historyOpen(CUR.hist); }
        });
        load();
    });

    function renderParamsHeader() { var all = $id('grdParametersAll'); if (all) all.checked = S.params.length > 0 && S.params.every(function (p) { return p.Checked; }); }

    global.ExportTpi = {
        btnNew: btnNew, btnRefresh: btnRefresh, btnsave: btnsave, btnUpdate: btnUpdate, print: print, printStockReserved: printStockReserved, stockReservedForm: stockReservedForm,
        btnAdd: btnAdd, btnUpdateDetail: btnUpdateDetail, btnCancelDetial: resetDetail, refreshSamplingRows: refreshSamplingRows,
        shortcuts: shortcuts, notPorted: notPorted, openRoute: openRoute,
        historyShow: historyShow, historyReset: historyReset, historyRefresh: historyRefresh, toggleHistory: toggleHistory
    };
}(window));
