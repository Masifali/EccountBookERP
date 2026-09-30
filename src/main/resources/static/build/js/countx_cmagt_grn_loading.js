/* =============================================================================================
 * GRN Loading Challan (Commission Trading) - Doc Type 1054
 * Desktop: Architecture.WinApp.Cmagt/frmGrnLoadingChallanCmagt.cs, GrnLoadingChallan_Helper.cs,
 *          frmLoadPurchaseOrderForGrnLoading.cs; BLL 0488, DAL 0540.
 * Every handler below names the desktop handler it ports (line numbers are frmGrnLoadingChallanCmagt.cs
 * unless stated).
 * ============================================================================================= */
(function () {
    'use strict';

    var API = '/api/commission/grn-loading-challan';
    var DD = '/api/commission/dropdowns';

    /* ------------------------------------------------------------------ state */
    var RecId = 0;
    var detailRows = [];          /* dtDetail */
    var removedRows = [];         /* lstRemoveRecordDetail */
    var ebRows = [], expRows = [];/* dtEmptyBags / dtExpGrid */
    var updateDetailIndex = -1;
    var calcByEbUnit = false, calcByEbTotal = false;
    var lastEbSource = null;
    var wbNetAccessible = '';     /* txtWbNetWeight.AccessibleDescription */
    var transporterNameTyped = false, deliverAddressTyped = false;
    var cfg = { tolerancePercentageForCmagtPoWeight: 0, grnLoadingAllowDifferentBiltyDate: false,
                grnLoadingDocDateAndBiltyDateSame: false, defaultDaysToLessFromHistoryFromDate: 0 };
    var LK = { parties: [], partyById: {}, cities: [], shipTo: [], items: [], parents: [],
               crop: [], packTypes: [], otherItems: [], ebItems: [], ebPackTypes: [], ebTerms: [] };
    var poTables = { pendingOrders: [], saleOrdersByPurchaseOrder: [], emptyBagsByOrder: [],
                     expensesByOrder: [], emptyBagCutByOrder: [] };
    var histCombo = [], histRows = [];
    var loaderRows = [], loaderCombo = [];
    var inFlight = {};

    /* ------------------------------------------------------------------ helpers */
    function col(row, name) {
        if (row == null) return null;
        if (row[name] !== undefined) return row[name];
        var k = Object.keys(row).find(function (x) { return x.toLowerCase() === name.toLowerCase(); });
        return k ? row[k] : null;
    }
    function toInt(v) { var n = parseInt(v, 10); return isFinite(n) ? n : 0; }
    function toNum(v) { if (v == null) return 0; var n = parseFloat(String(v).replace(/,/g, '')); return isFinite(n) ? n : 0; }
    function iOf(sel) { return toInt($(sel).val()); }
    function nOf(sel) { return toNum($(sel).val()); }
    function r3(x) { var n = Number(x); if (!isFinite(n)) return 0; return (n < 0 ? -1 : 1) * Math.round(Math.abs(n) * 1000) / 1000; }
    /* "#,##0.###" */
    function f3(x) { var n = r3(x); return n.toLocaleString('en-US', { maximumFractionDigits: 3 }); }
    function esc(v) {
        return String(v === undefined || v === null ? '' : v).replace(/&/g, '&amp;').replace(/</g, '&lt;')
            .replace(/>/g, '&gt;').replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    }
    function dOnly(v) { return v ? String(v).substring(0, 10) : ''; }
    function localYmd(d) {
        var p = function (n) { return (n < 10 ? '0' : '') + n; };
        return d.getFullYear() + '-' + p(d.getMonth() + 1) + '-' + p(d.getDate());
    }
    var MON = ['Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov','Dec'];
    function ddMMMyyyy(v) { var s = dOnly(v); if (!s) return ''; var p = s.split('-'); return p[2] + '-' + MON[toInt(p[1]) - 1] + '-' + p[0]; }
    function ddMMMyy(v) { var s = dOnly(v); if (!s) return ''; var p = s.split('-'); return p[2] + '-' + MON[toInt(p[1]) - 1] + '-' + p[0].substring(2); }
    function dateTime(v) {
        if (!v) return '';
        var s = String(v).replace('T', ' ');
        var d = ddMMMyy(s); var t = s.substring(11, 16);
        if (!t) return d;
        var h = toInt(t.substring(0, 2)), m = t.substring(3, 5), ap = h >= 12 ? 'PM' : 'AM';
        h = h % 12; if (h === 0) h = 12;
        return d + ' ' + (h < 10 ? '0' : '') + h + ':' + m + ' ' + ap;
    }
    function getJSON(url, data) { return $.ajax({ url: url, data: data, dataType: 'json', cache: false }); }
    function msg(t) { window.alert(t); }

    /* Button guard: disable at once, spinner, block duplicates, restore on success or failure. */
    function withButton(sel, work) {
        if (inFlight[sel]) return $.Deferred().resolve().promise();
        var b = $(sel);
        var was = b.prop('disabled');
        inFlight[sel] = true;
        b.prop('disabled', true).addClass('btn-busy');
        var done = function () { inFlight[sel] = false; b.removeClass('btn-busy').prop('disabled', was); };
        var r;
        try { r = work(); } catch (e) { done(); msg(e.message || e); return $.Deferred().resolve().promise(); }
        if (r && typeof r.always === 'function') { r.always(done); return r; }
        if (r && typeof r.then === 'function') { r.then(done, done); return r; }
        done();
        return $.Deferred().resolve(r).promise();
    }

    function setOptions(sel, list, valFn, textFn, attrFn, blankVal) {
        var el = $(sel), keep = el.val();
        var h = '<option value="' + (blankVal === undefined ? '0' : blankVal) + '"></option>';
        (list || []).forEach(function (d) {
            h += '<option value="' + esc(valFn(d)) + '"' + (attrFn ? attrFn(d) : '') + '>' + esc(textFn(d)) + '</option>';
        });
        el.html(h);
        if (keep != null && el.find('option[value="' + String(keep).replace(/"/g, '') + '"]').length) el.val(keep);
    }
    function partyAttr(d) {
        return ' data-code="' + esc(d.PartyCode || '') + '" data-city="' + esc(d.CityName || '') + '" data-mobile="' + esc(d.MobileNo || '') + '"';
    }
    function bindParty(sel) { setOptions(sel, LK.parties, function (d) { return d.Id; }, function (d) { return d.CompanyName || d.name; }, partyAttr); }
    function selText(sel) { var o = $(sel + ' option:selected'), v = o.val(); return (v && v !== '0') ? $.trim(o.text()) : ''; }
    function setVal(sel, v) {
        var el = $(sel);
        if (v == null || !el.find('option[value="' + String(v).replace(/"/g, '') + '"]').length) { el.val(el.find('option:first').val()); return false; }
        el.val(String(v)); return true;
    }

    /* ================================================================== load */
    $(function () {
        initDates();
        wireEvents();
        resetChildGrids();
        renderDetailGrid();
        loadLookups().always(function () {
            applyConfigDefaults(true);
            loadDocNo();
            loadPendingOrders();
            loadHistoryCombos();
            var q = new URLSearchParams(window.location.search);
            if (toInt(q.get('id')) > 0) readById(toInt(q.get('id')));
        });
    });

    /* InitializeComponentMethod (:559-625) */
    function loadLookups() {
        var reqs = [];
        reqs.push(getJSON(API + '/form-config').done(function (c) { cfg = $.extend(cfg, c || {}); applyRights(cfg.rights); }));
        reqs.push(getJSON(DD + '/companies').done(function (d) {
            setOptions('#companyId', d, function (x) { return x.id; }, function (x) { return x.name; });
        }));
        reqs.push(getJSON(DD + '/branches').done(function (d) {
            setOptions('#branchId', d, function (x) { return x.id; }, function (x) { return x.name; });
        }));
        /* SupplierBind (:778): CA, Supplier, Buyer, Deliver To Party and Transporter share dtSupplier. */
        reqs.push(getJSON(DD + '/suppliers', { useBusinessName: true }).done(function (d) {
            LK.parties = d || []; LK.partyById = {};
            LK.parties.forEach(function (p) { LK.partyById[toInt(p.Id)] = p; });
            ['#commissionAgentId', '#supplierId', '#transporterId', '#detBuyerId', '#detDeliverToPartyId'].forEach(bindParty);
        }));
        /* CityBindFromGlobal (:787): both city combos bind the same table. */
        reqs.push(getJSON(DD + '/cities').done(function (d) {
            LK.cities = d || [];
            ['#loadingCityId', '#unloadingCityId'].forEach(function (s) {
                setOptions(s, LK.cities, function (x) { return x.Id; }, function (x) { return x.CityName || x.name; });
            });
        }));
        reqs.push(getJSON(DD + '/delivery-terms').done(function (d) {
            setOptions('#deliveryTermId', d, function (x) { return x.id; }, function (x) { return x.name; });
        }));
        reqs.push(getJSON(DD + '/vehicle-types').done(function (d) {
            setOptions('#vehicleTypeId', d, function (x) { return x.id; }, function (x) { return x.name; });
        }));
        /* dtShipToAddress = AllShipToAddressDbCall (:568) */
        reqs.push(getJSON(DD + '/ship-to-addresses').done(function (d) { LK.shipTo = d || []; bindShipTo(0); }));
        /* ParentCategoryBindFromGlobal (:959) / ItemdtFillFromGlobal (:1038) */
        reqs.push(getJSON(DD + '/parent-categories').done(function (d) {
            LK.parents = d || [];
            setOptions('#detParentItemId', LK.parents, function (x) { return x.Id; }, function (x) { return x.InvParentCateDescription || x.name; });
        }));
        reqs.push(getJSON(DD + '/items').done(function (d) { LK.items = d || []; }));
        reqs.push(getJSON(DD + '/crop-years').done(function (d) {
            LK.crop = d || [];
            setOptions('#detCropYearId', LK.crop, function (x) { return x.Id; }, function (x) { return x.CropYear || x.name; });
        }));
        reqs.push(getJSON(DD + '/packing-types').done(function (d) {
            LK.packTypes = d || [];
            setOptions('#detPackingTypeId', LK.packTypes, function (x) { return x.Id; }, function (x) { return x.PackTypeDesc || x.name; });
        }));
        /* OtherItemsdtFillDbCall (:1513) / GetPackingMaterialItemsAllocateToFlow (:571) */
        reqs.push(getJSON(DD + '/other-items').done(function (d) { LK.otherItems = d || []; }));
        reqs.push(getJSON(DD + '/empty-bag-items', { transactionFlowId: 1 }).done(function (d) { LK.ebItems = d || []; }));
        /* BindViewCombos (:861): EmptyBagTypes -> CmbEmptyBagTerm; AllocatedPackingType -> EB grid. */
        reqs.push(getJSON(DD + '/view-combos').done(function (d) {
            LK.ebTerms = (d || []).filter(function (r) { return String(r.Activity || '') === 'EmptyBagTypes'; });
            LK.ebPackTypes = (d || []).filter(function (r) { return String(r.Activity || '') === 'AllocatedPackingType'; });
            setOptions('#detEmptyBagTermId', LK.ebTerms, function (x) { return x.Id; }, function (x) { return x.ReferenceName; });
        }));
        var all = $.Deferred();
        var left = reqs.length;
        reqs.forEach(function (r) { r.always(function () { if (--left === 0) { renderEbGrid(); renderExpGrid(); bindItemCombo(); all.resolve(); } }); });
        return all.promise();
    }

    /* InitializeComponentMethod :591-594 - formright = SetRightsValueInRightsObject(ScreenName):
       btnsave.Enabled = DoHaveSaveRight, btnUpdate.Enabled = DoHaveUpdateRights,
       btnDelete.Enabled = DoHaveCanDelete, btnPrint.Enabled = DoHavePrintRights.
       The server enforces Save/Update/Delete again; this only mirrors the button states. */
    function applyRights(r) {
        if (!r) return;
        $('#btnSave').prop('disabled', r.save !== true);
        $('#btnUpdate').prop('disabled', r.update !== true);
        $('#btnDelete').prop('disabled', r.delete !== true);
        $('#btnPrint').prop('disabled', r.print !== true);
    }

    /* CmbCompanyName.Value = UserAccount.CompanyId (:600) and
       GetCommissionAgentConfigurationsFromGlobalandBind (:661) - applied only when > 0. */
    function applyConfigDefaults(first) {
        if (first) {
            if (toInt(cfg.companyId) > 0) setVal('#companyId', cfg.companyId);
            if (toInt(cfg.branchId) > 0 && !iOf('#branchId')) setVal('#branchId', cfg.branchId);
        }
        if (toInt(cfg.commissionAgentId) > 0) setVal('#commissionAgentId', cfg.commissionAgentId);
        if (toInt(cfg.deliveryTermId) > 0) setVal('#deliveryTermId', cfg.deliveryTermId);
        if (toInt(cfg.cropYearId) > 0) setVal('#detCropYearId', cfg.cropYearId);
        if (toInt(cfg.packingTypeId) > 0) setVal('#detPackingTypeId', cfg.packingTypeId);
        if (toInt(cfg.loadingCityId) > 0) setVal('#loadingCityId', cfg.loadingCityId);
        if (toInt(cfg.unloadingCityId) > 0) setVal('#unloadingCityId', cfg.unloadingCityId);
    }

    /* Doc Date = today (picker default); Bilty Date follows it. History From = today -
       DefaultDaysToLessFromHistoryFromDate (else 3), To = today (:618-619). */
    function initDates() {
        var now = new Date(), today = localYmd(now);
        $('#docDate, #biltyDate, #histToDate, #ldToDate, #histValidityFrom, #histValidityTo').val(today);
        var f = new Date(now.getFullYear(), now.getMonth(), now.getDate()); f.setDate(f.getDate() - 3);
        $('#histFromDate').val(localYmd(f));
        var l = new Date(now.getFullYear(), now.getMonth(), now.getDate()); l.setDate(l.getDate() - 7);
        $('#ldFromDate').val(localYmd(l));   /* loader InitializeComponentCustom :129 */
        getJSON(API + '/form-config').done(function (c) {
            var days = toInt(c && c.defaultDaysToLessFromHistoryFromDate);
            var g = new Date(now.getFullYear(), now.getMonth(), now.getDate()); g.setDate(g.getDate() - (days > 0 ? days : 3));
            $('#histFromDate').val(localYmd(g));
        });
    }

    /* DocumentNoDbCall (:752) */
    function loadDocNo() {
        return getJSON(API + '/generate-no').done(function (r) { if (!RecId && r) $('#docNo').val(r.docNo || ''); });
    }

    /* OutstandingOrdersdtFillDbCall (:890): tenancy + RecId only, then OrderNoBind. */
    function loadPendingOrders() {
        return getJSON(API + '/pending-purchase-order-tables', { recId: RecId }).done(function (d) {
            poTables = $.extend({ pendingOrders: [], saleOrdersByPurchaseOrder: [], emptyBagsByOrder: [],
                                  expensesByOrder: [], emptyBagCutByOrder: [] }, d || {});
            orderNoBind();
        });
    }

    /* ================================================================== events */
    function wireEvents() {
        /* tabs + History button (right of footer) */
        $('#tab-form-link').on('click', function (e) { e.preventDefault(); showTab('form'); });
        $('#tab-history-link').on('click', function (e) { e.preventDefault(); showTab('history'); });
        /* tabControl3 (History): Detail | Other Expense | Empty Bags Weight Deduction */
        $('#histSubTabs').on('click', 'a', function (e) {
            e.preventDefault();
            $('#histSubTabs a').removeClass('active'); $(this).addClass('active');
            $('.hpane').hide(); $('#' + $(this).data('pane')).show();
        });
        $('#btnHistory').on('click', function () { showTab($('#tab-history').hasClass('active') ? 'form' : 'history'); });

        /* footer buttons */
        $('#btnNew').on('click', function () { reset(); });   /* btnnew_Click :2898 */
        $('#btnSave').on('click', function () { withButton('#btnSave', function () { RecId = 0; return insert(); }); });
        $('#btnUpdate').on('click', function () {
            withButton('#btnUpdate', function () { if (!RecId) { msg('Record not update because Id not found'); return; } return insert(); });
        });
        $('#btnDelete').on('click', function () { withButton('#btnDelete', deleteRecord); });
        $('#btnPrint').on('click', function () { openPrint(RecId, 'slip'); });
        $('#btnPrintChallan').on('click', function () { openPrint(RecId, 'challan'); });
        $('#btnRefresh').on('click', function () { withButton('#btnRefresh', refreshAll); });
        $('#btnShortcutKeys').on('click', showShortcutKeys);

        /* header */
        $('#supplierId').on('change', cmbSupplierLeave);
        $('#transporterId').on('change', function () {                     /* CmbTransporter_Leave :1687 */
            if (iOf('#transporterId') > 0 && !transporterNameTyped) $('#transporterName').val(selText('#transporterId'));
        });
        $('#transporterName').on('keypress input', function () {           /* txtTransporterName_KeyPress :1702 */
            transporterNameTyped = true;
            if (iOf('#transporterId')) $('#transporterId').val('0');
        });
        $('#docDate').on('change', function () { $('#biltyDate').val($('#docDate').val()); }); /* :4316 */
        $('#biltyFreight, #otherAdLesCharges').on('input change', calculateTotalFreight);      /* :3218/:3230 */
        $('#loadWeight, #tareWeight').on('input', calculateHeaderWeight);                      /* :3290/:3302 */
        $('#scaleNetWeight').on('change', wbNetWeightChanged);                                 /* :3333 */
        $('#biltyQty').on('input change', vehicleQtyChanged);                                  /* :4328 */
        vehicleNoGuards();

        /* detail */
        $('#detPoNo').on('change', cmbPoNoDetailLeave);
        $('#detParentItemId').on('change', cmbParentItemLeave);
        $('#detItemId').on('change', cmbItemNameLeave);
        $('#radItemName, #radItemCode').on('change', function () { bindItemCombo(); }); /* :1501 */
        $('#detPackingTypeId').on('change', cmbPackingTypeLeave);
        $('#detPackUomId').on('change', function () { ebCalculations(); calculateGrossWeight(); calculateNetWeight(); }); /* :3123 */
        $('#detLoadingQty').on('input', function () { ebCalculations(); calculateGrossWeight(); calculateNetWeight(); });  /* :3137 */
        $('#detGrossWeight').on('input', function () { ebCalculations(); calculateNetWeight(); });                         /* :3151 */
        $('#detEbUnit').on('input', function () { lastEbSource = 'unit'; ebCalculations(); lastEbSource = null; calculateNetWeight(); });   /* :3164 */
        $('#detEbTotal').on('input', function () { lastEbSource = 'total'; ebCalculations(); lastEbSource = null; calculateNetWeight(); }); /* :3177 */
        $('#detAddLessWeight').on('input', function () { ebCalculations(); calculateNetWeight(); });                       /* :3190 */
        $('#detBuyerId').on('change', cmbBuyerNameDetailLeave);
        $('#detDeliverToPartyId').on('change', cmbDeliveryToPartyLeave);
        $('#detShipToAddressId').on('change', cmbShipToAddressLeave);
        $('#detDeliverToAddress').on('keypress input', function () {       /* txtDeliveryToAddress_KeyPress :1674 */
            deliverAddressTyped = true;
            if (iOf('#detShipToAddressId')) $('#detShipToAddressId').val('0');
        });
        $('#btnAddDetail').on('click', btnAddClick);
        $('#btnUpdateDetail').on('click', btnUpdateDetailClick);
        $('#btnCancelDetail').on('click', resetDetail);
        $('#btnLoadPo').on('click', openLoader);
        /* btnDefineShipToAddress_Click (:4274) -> SupfrmShipToAddress.ShowDialog() */
        $('#btnDefineShipToAddress').on('click', function () { withButton('#btnDefineShipToAddress', stOpen); });
        $('#btnStClose').on('click', stClose);
        $('#btnStNew').on('click', stReset);                                     /* btnnew_Click -> Reset() :400 */
        $('#btnStRefresh').on('click', function () { withButton('#btnStRefresh', stCombos); });   /* btnRefresh_Click :434 */
        $('#btnStSave').on('click', function () { stSave('#btnStSave'); });      /* btnsave_Click -> Insert() */
        $('#btnStUpdate').on('click', function () { stSave('#btnStUpdate'); });  /* btnupdate_Click -> Insert() */
        $(document).on('dblclick', '#gridShipTo tbody tr[data-id]', function () { stEdit(toInt($(this).data('id'))); });  /* grdfrm_CellContentDoubleClick */
        $(document).on('click', '#gridShipTo tbody tr[data-id]', function () { $('#gridShipTo tbody tr').removeClass('sel'); $(this).addClass('sel'); });

        /* detail grid: X / Edit / double click (grdDetail_ColumnButtonClick :2117, DoubleClick :2005) */
        $(document).on('click', '#gridItems .d-del', function () { deleteDetailRow(toInt($(this).closest('tr').data('i'))); });
        $(document).on('click', '#gridItems .d-edit', function () { editDetailRow(toInt($(this).closest('tr').data('i'))); });
        $(document).on('dblclick', '#gridItems tbody tr', function () { editDetailRow(toInt($(this).data('i'))); });

        /* empty bags / expenses grids */
        wireChildGrids();

        /* history */
        $('#btnLoadHistory').on('click', function () { withButton('#btnLoadHistory', historyFill); });
        $('#btnNewHistory').on('click', function () {                        /* :3420 */
            $('#histItemId, #histCommissionAgentId, #histSupplierId').val('0');
        });
        $('#btnRefreshHistory').on('click', function () { withButton('#btnRefreshHistory', loadHistoryCombos); });
        $('#histParentIds').on('change', bindHistoryCombosAgainstParent);    /* CmbParentItemHistory_Leave :3408 */
        $(document).on('click', '#gridHistory .h-edit, #gridHistory .h-code', function () { readById(toInt($(this).closest('tr').data('id'))); });
        $(document).on('dblclick', '#gridHistory tbody tr', function () { readById(toInt($(this).data('id'))); });
        $(document).on('click', '#gridHistory .h-print', function () { openPrint(toInt($(this).closest('tr').data('id')), 'slip'); });
        $(document).on('click', '#gridHistory .h-printc', function () { openPrint(toInt($(this).closest('tr').data('id')), 'challan'); });
        $(document).on('click', '#gridHistory tbody tr', function () {       /* grdHistory_SelectionChanged :3940 */
            var id = toInt($(this).data('id')); if (!id) return;
            $('#gridHistory tbody tr').removeClass('sel'); $(this).addClass('sel');
            getDetailGrdByHeadId(id);
        });

        /* loader */
        $('#btnLoaderClose').on('click', function () { $('#loaderModal').removeClass('show'); });
        $('#btnLoaderSearch').on('click', function () { withButton('#btnLoaderSearch', loaderSearch); });
        $('#btnLoaderReset').on('click', loaderReset);
        $('#btnLoaderLoad').on('click', loaderLoad);
        /* btnRefresh_Click (:469): CombosFill(ComboDbCall()) only */
        $('#btnLoaderRefresh').on('click', function () { getJSON(API + '/loader-combos').done(function (d) { loaderCombo = d || []; loaderCombosFill(); }); });
        $('#btnLoaderShortcut').on('click', loaderShortcuts);
        /* grid header check-all + filter row */
        $('#ldChkAll').on('change', function () { $('#gridLoader tbody tr:visible .ld-chk').prop('checked', this.checked); });
        $('#gridLoader').on('input', '.ld-flt', loaderFilter);
        /* frmLoadLogisticAgreementForPO_KeyDown (:568) - active while the dialog is open */
        $(document).on('keydown', function (e) {
            if (!$('#loaderModal').hasClass('show')) return;
            var k = (e.key || '').toLowerCase();
            if (e.ctrlKey && e.altKey && (k === 'alt' || k === 'control')) { e.preventDefault(); e.stopImmediatePropagation(); loaderShortcuts(); return; }
            if (!e.ctrlKey || e.altKey) return;
            var act = { e: function () { $('#loaderModal').removeClass('show'); }, s: loaderSearch, l: loaderLoad, n: loaderReset,
                        r: function () { $('#btnLoaderRefresh').click(); } }[k];
            if (act) { e.preventDefault(); e.stopImmediatePropagation(); act(); }
            else if (k === 'f5' || e.key === 'ArrowUp') { e.preventDefault(); $('#ldFromDate').focus(); }
            else if (e.key === 'ArrowDown') { e.preventDefault(); $('#gridLoader .ld-chk').first().focus(); }
        });

        /* fullscreen toggles */
        $(document).on('click', '.grid-fs', function () { $('#' + $(this).data('target')).toggleClass('fs'); });

        /* frmGrnLoadingChallanCmagt_KeyDown (:4067) */
        $(document).on('keydown', function (e) {
            var onForm = $('#tab-form').hasClass('active');
            if (e.key === 'Escape') { $('.grid-wrap.fs').removeClass('fs'); $('#loaderModal').removeClass('show'); if ($('#shipToModal').hasClass('show')) stClose(); }
            if (!e.ctrlKey) return;
            var k = (e.key || '').toLowerCase();
            if (k === 't') { e.preventDefault(); showTab(onForm ? 'history' : 'form'); return; }
            if (onForm) {
                if (e.shiftKey && e.key === 'Delete') { e.preventDefault(); if ($('#btnDelete').is(':visible')) $('#btnDelete').click(); }
                else if (k === 's') { e.preventDefault(); if ($('#btnSave').is(':visible')) $('#btnSave').click(); }
                else if (k === 'u') { e.preventDefault(); if ($('#btnUpdate').is(':visible')) $('#btnUpdate').click(); }
                else if (k === 'p') { e.preventDefault(); $('#btnPrint').click(); }
                else if (k === 'n') { e.preventDefault(); $('#btnNew').click(); }
                else if (k === 'r') { e.preventDefault(); $('#btnRefresh').click(); }
                else if (e.key === 'ArrowDown') { e.preventDefault(); $('#detParentItemId').focus(); }
                else if (e.key === 'ArrowUp' || e.key === 'F5') { e.preventDefault(); $('#docDate').focus(); }
            } else if (k === 's') { e.preventDefault(); $('#btnLoadHistory').click(); }
            else if (e.key === 'ArrowUp') { e.preventDefault(); $('#histFromDate').focus(); }
        });
    }

    function showTab(which) {
        var form = which === 'form';
        $('#tab-form').toggleClass('show active', form);
        $('#tab-history').toggleClass('show active', !form);
        $('#tab-form-link').toggleClass('active', form);
        $('#tab-history-link').toggleClass('active', !form);
        $('#btnHistory').text(form ? 'History' : 'Form');
    }

    function showShortcutKeys() {   /* MakeShortCutKeys (:4173) */
        msg('Ctrl+S  Save (entry) / Show (history)\nCtrl+U  Update\nCtrl+Shift+Delete  Delete\nCtrl+R  Refresh\n'
          + 'Ctrl+N  New\nCtrl+P  Print\nCtrl+F5 / Ctrl+ArrowUp  Focus Doc Date\nCtrl+T  Tab Transfer\n'
          + 'Ctrl+ArrowDown  Focus Parent Item\nDouble-click a grid row  Edit');
    }

    /* BtnRefresh_Click (:2994) */
    function refreshAll() {
        return loadLookups().always(function () {
            loadPendingOrders();
            applyConfigDefaults(false);
        });
    }

    /* ================================================================== header handlers */
    /* CmbSupplierName_Leave (:813): OrderNoBind; Loading City from the supplier's city when empty. */
    function cmbSupplierLeave() {
        orderNoBind();
        var sid = iOf('#supplierId');
        if (!iOf('#loadingCityId') && sid > 0) {
            var p = LK.partyById[sid];
            if (p && toInt(p.CityId) > 0) setVal('#loadingCityId', p.CityId);
        }
    }

    function calculateTotalFreight() {   /* :3203 */
        $('#totalFreight').val(f3(nOf('#biltyFreight') + nOf('#otherAdLesCharges')));
    }

    /* CalculateHeaderWeight (:3314): |load - tare|, then txtWbNetWeight_TextChanged. */
    function calculateHeaderWeight() {
        var net = r3(Math.abs(nOf('#loadWeight') - nOf('#tareWeight')));
        $('#scaleNetWeight').val(net);
        wbNetAccessible = String(net);
        wbNetWeightChanged(true);
    }

    /* txtWbNetWeight_TextChanged (:3333): a typed net weight that differs from the computed one
       clears Load/Tare; Vehicle Qty defaults to net / average PackUomEquivalent; Avg Fill/Bag. */
    function wbNetWeightChanged(fromCalc) {
        var net = nOf('#scaleNetWeight');
        if (fromCalc !== true && toNum(wbNetAccessible) !== net) { $('#loadWeight, #tareWeight').val(''); }
        wbNetAccessible = String(net);
        var eqs = detailRows.map(function (r) { return toNum(r._packUomEquivalent); });
        var avgUom = eqs.length ? eqs.reduce(function (a, b) { return a + b; }, 0) / eqs.length : 0;
        if (nOf('#biltyQty') === 0) $('#biltyQty').val(avgUom > 0 ? r3(net / avgUom) : 0);
        var qty = nOf('#biltyQty');
        $('#avgFillingPerBag').val(f3(qty > 0 ? net / qty : 0));
    }

    function vehicleQtyChanged() {   /* :4328 */
        var qty = nOf('#biltyQty');
        $('#avgFillingPerBag').val(f3(qty > 0 ? nOf('#scaleNetWeight') / qty : 0));
    }

    /* txtvehicleno_KeyPress/Leave/KeyDown/KeyUp/TextChanged (:1530-1646): upper case, letters
       then '-' then digits, 1-6 each, dash inserted automatically. */
    function vehicleNoGuards() {
        var el = $('#vehicleNo');
        el.on('keypress', function (e) {
            var ch = String.fromCharCode(e.which || e.keyCode);
            if (!ch || e.ctrlKey || e.metaKey || e.which < 32) return;
            if (!/[A-Za-z0-9-]/.test(ch)) { e.preventDefault(); return; }
            var t = this.value, pos = this.selectionStart || 0;
            if (ch === '-' && t.indexOf('-') >= 0) { e.preventDefault(); return; }
            var fut = t.substring(0, pos) + ch + t.substring(this.selectionEnd || pos);
            var di = fut.indexOf('-');
            var letters = di >= 0 ? fut.substring(0, di) : fut;
            var digits = di >= 0 ? fut.substring(di + 1) : '';
            if (/[A-Za-z]/.test(ch) && letters.length > 6) { e.preventDefault(); return; }
            if (/\d/.test(ch) && di >= 0 && digits.length > 6) { e.preventDefault(); return; }
            if (/\d/.test(ch) && t.indexOf('-') < 0) {
                var lc = (t.match(/^[A-Za-z]*/) || [''])[0].length;
                if (lc >= 1 && lc <= 6) { this.value = t.substring(0, lc) + '-' + t.substring(lc); this.setSelectionRange(pos + 1, pos + 1); }
            }
        });
        el.on('input', function () {
            var p = this.selectionStart; this.value = this.value.toUpperCase(); try { this.setSelectionRange(p, p); } catch (x) { /* ignore */ }
        });
        el.on('paste', function () {
            var self = this;
            setTimeout(function () {
                var t = self.value.trim().toUpperCase().replace(/[^A-Z0-9]/g, '');
                var lc = (t.match(/^[A-Z]*/) || [''])[0].length;
                if (lc >= 1 && lc <= 6) t = t.substring(0, lc) + '-' + t.substring(lc);
                self.value = t;
            }, 0);
        });
        el.on('blur', function () {
            var t = this.value.trim().toUpperCase();
            if (!t) return;
            if (t.indexOf('-') < 0) { var lc = (t.match(/^[A-Z]*/) || [''])[0].length; if (lc >= 1 && lc <= 6) t = t.substring(0, lc) + '-' + t.substring(lc); }
            this.value = t;
        });
    }

    /* ================================================================== detail: combos */
    function poSelected() { return iOf('#detPoNo') > 0 && !!selectedPoRow(); }
    function selectedPoRow() {
        var id = iOf('#detPoNo'); if (!id) return null;
        return (poTables.pendingOrders || []).find(function (r) { return toInt(col(r, 'purchaseOrderMasterId')) === id; }) || null;
    }

    /* OrderNoBind (:923): one row per order, filtered by the header supplier. */
    function orderNoBind() {
        var sid = iOf('#supplierId'), seen = {}, list = [];
        (poTables.pendingOrders || []).forEach(function (r) {
            if (sid > 0 && toInt(col(r, 'supplierId')) !== sid) return;
            var id = toInt(col(r, 'purchaseOrderMasterId'));
            if (!id || seen[id]) return;
            seen[id] = 1; list.push(r);
        });
        setOptions('#detPoNo', list, function (r) { return col(r, 'purchaseOrderMasterId'); }, function (r) { return col(r, 'DocNo'); },
            function (r) { return ' data-start="' + esc(ddMMMyyyy(col(r, 'DeliveryStartDate'))) + '" data-expiry="' + esc(ddMMMyyyy(col(r, 'ValidityDate'))) + '"'; });
    }

    /* dtItem rows: ItemdtFillAgainstOder (:987) when an order is chosen, else
       ItemdtFillFromGlobal(parent) (:1038). */
    function currentItemRows() {
        if (poSelected()) {
            var oid = iOf('#detPoNo');
            return (poTables.pendingOrders || []).filter(function (r) { return toInt(col(r, 'purchaseOrderMasterId')) === oid; }).map(function (r) {
                return { MappingId: toInt(col(r, 'purchaseOrderSaleOrderMappingId')), ItemName: col(r, 'ItemName') || '', ItemCode: col(r, 'ItemCode') || '',
                    ItemId: toInt(col(r, 'itemId')), ItemQty: toNum(col(r, 'itemQty')), ReceivedQty: toNum(col(r, 'UsedQty')), BalanceQty: toNum(col(r, 'BalanceQty')),
                    ItemWeight: toNum(col(r, 'itemWeight')), ReceivedWeight: toNum(col(r, 'UsedWeight')), BalanceWeight: toNum(col(r, 'BalanceWeight')),
                    ItemRate: toNum(col(r, 'itemRate')), ParentCategoryId: toInt(col(r, 'inventoryParentCategoryId')), cropYearId: toInt(col(r, 'cropYearId')),
                    packingTypeId: toInt(col(r, 'packingTypeId')), packUomId: toInt(col(r, 'packUomId')), EbUnit: toNum(col(r, 'EbUnit')),
                    OrderDetailId: toInt(col(r, 'DetailId')), SoId: toInt(col(r, 'saleOrderMasterId')), SoDetailId: toInt(col(r, 'saleOrderDetailId')),
                    SoNo: toInt(col(r, 'SaleOrderNo')), BuyerId: toInt(col(r, 'buyerId')), DeliveryToPartyId: toInt(col(r, 'DeliveryToPartyId')),
                    ShipToAddressId: toInt(col(r, 'ShipToAddressId')), ShipToAddress: col(r, 'ShipToAddress') || '' };
            });
        }
        var pc = iOf('#detParentItemId');
        return (LK.items || []).filter(function (x) { return pc === 0 || toInt(x.InventoryParentCategoriesId) === pc; }).map(function (x) {
            return { MappingId: toInt(x.Id), ItemName: x.ItemName || '', ItemCode: x.ItemCode || '', ItemId: toInt(x.Id), ItemQty: 0, ReceivedQty: 0, BalanceQty: 0,
                ItemWeight: 0, ReceivedWeight: 0, BalanceWeight: 0, ItemRate: 0, ParentCategoryId: toInt(x.InventoryParentCategoriesId), cropYearId: 0,
                packingTypeId: 0, packUomId: 0, EbUnit: 0, OrderDetailId: 0, SoId: 0, SoDetailId: 0, SoNo: 0, BuyerId: 0, DeliveryToPartyId: 0,
                ShipToAddressId: 0, ShipToAddress: '' };
        });
    }

    /* ItemNameBind (:1087): value MappingId, text ItemName or ItemCode per the radio. */
    function bindItemCombo(keepMapping) {
        var byName = $('#radItemName').is(':checked');
        /* label15 stays "Item" on the desktop (:6813) whichever radio is checked. */
        $('#detItemId').attr('data-dtcombo-caption', byName ? 'Item Name' : 'Item Code');
        var rows = currentItemRows();
        var keep = keepMapping != null ? keepMapping : iOf('#detItemId');
        var el = $('#detItemId');
        var h = '<option value="0"></option>';
        var withPo = poSelected();
        rows.forEach(function (r, i) {
            h += '<option value="' + r.MappingId + '" data-idx="' + i + '" data-item-code="' + esc(byName ? r.ItemCode : r.ItemName) + '"'
               + (withPo ? ' data-item-qty="' + f3(r.ItemQty) + '" data-received-qty="' + f3(r.ReceivedQty) + '" data-balance-qty="' + f3(r.BalanceQty)
               + '" data-item-weight="' + f3(r.ItemWeight) + '" data-received-weight="' + f3(r.ReceivedWeight) + '" data-balance-weight="' + f3(r.BalanceWeight)
               + '" data-so-no="' + r.SoNo + '"' : '') + '>' + esc(byName ? r.ItemName : r.ItemCode) + '</option>';
        });
        el.html(h).data('rows', rows);
        if (keep && el.find('option[value="' + keep + '"]').length) el.val(String(keep));
    }
    function selectedItemRow() {
        var o = $('#detItemId option:selected'), rows = $('#detItemId').data('rows') || [];
        var i = o.data('idx');
        return (i === undefined || i === null || !toInt(o.val())) ? null : rows[i];
    }

    /* CmbPoNoDetail_Leave (:1300) */
    function cmbPoNoDetailLeave(noItemLeave) {
        var po = selectedPoRow();
        if (po) {
            $('#detParentItemId, #detEmptyBagTermId, #detBuyerId').prop('disabled', true);
            setVal('#detEmptyBagTermId', toInt(col(po, 'EBWeightDeductionTermId')));
            if (!iOf('#supplierId')) { setVal('#supplierId', toInt(col(po, 'supplierId'))); cmbSupplierLeave(); }
            if (!iOf('#commissionAgentId')) setVal('#commissionAgentId', toInt(col(po, 'commissionAgentId')));
        } else {
            $('#detParentItemId, #detEmptyBagTermId, #detBuyerId').prop('disabled', false);
            $('#detSaleOrderNo').val('');
        }
        bindItemCombo();
        if (noItemLeave !== true) cmbItemNameLeave();
    }

    /* cmbParentItem_Leave (:1330) */
    function cmbParentItemLeave() {
        bindItemCombo();
        if (poSelected()) cmbBuyerNameDetailLeave();
    }

    /* CmbItemName_Leave (:1351) */
    function cmbItemNameLeave() {
        var r = selectedItemRow();
        if (!r) return $.Deferred().resolve().promise();
        if (r.ItemId > 0) {
            var it = (LK.items || []).find(function (x) { return toInt(x.Id) === r.ItemId; });
            if (it) setVal('#detParentItemId', it.InventoryParentCategoriesId);
        }
        var uomReq = itemUomBind(r.ItemId);
        if (!(iOf('#detPoNo') > 0)) return uomReq;
        setVal('#detParentItemId', r.ParentCategoryId);
        $('#detPoTotalWeight').val(f3(r.ItemWeight));
        $('#detPoReceivedWeight').val(f3(r.ReceivedWeight));
        $('#detPoBalanceWeight').val(f3(r.BalanceWeight));
        $('#detSaleOrderNo').val(String(r.SoNo));
        setVal('#detBuyerId', r.BuyerId);
        setVal('#detDeliverToPartyId', r.DeliveryToPartyId);
        cmbDeliveryToPartyLeave();
        if (r.ShipToAddressId === 0) $('#detDeliverToAddress').val(r.ShipToAddress);
        else setVal('#detShipToAddressId', r.ShipToAddressId);
        var lq = nOf('#detLoadingQty');
        if (lq === 0 || lq > r.BalanceQty) $('#detLoadingQty').val(r3(r.BalanceQty));
        var gw = nOf('#detGrossWeight');
        if (gw === 0 || gw > r.BalanceWeight) $('#detGrossWeight').val(r3(r.BalanceWeight));
        if (updateDetailIndex === -1) {
            uomReq.always(function () {
                if ($('#detPackUomId option').length > 1) setVal('#detPackUomId', r.packUomId);
                calculateNetWeight();
            });
            if (LK.crop.length) setVal('#detCropYearId', r.cropYearId);
            if (LK.packTypes.length) { setVal('#detPackingTypeId', r.packingTypeId); cmbPackingTypeLeave(); }
        }
        calculateNetWeight();
        return uomReq;
    }

    /* CommonBindings.ItemUomFromGlobalBind: the item's own UOM schedule. */
    function itemUomBind(itemId) {
        var el = $('#detPackUomId');
        el.html('<option value="0"></option>');
        if (!itemId) return $.Deferred().resolve().promise();
        return getJSON(DD + '/item-uoms', { itemId: itemId }).done(function (u) {
            var h = '<option value="0"></option>';
            (u || []).forEach(function (x) {
                h += '<option value="' + x.Id + '" data-eq="' + esc(x.Equivalent) + '" data-base="' + (x.BaseRateUom ? 'true' : 'false')
                   + '" data-base-pack="' + (x.BasePackUom ? 'true' : 'false') + '">' + esc(x.UOMCode || '') + '</option>';
            });
            el.html(h);
        });
    }
    function packUomEquivalent() {
        var o = $('#detPackUomId option:selected');
        return iOf('#detPackUomId') > 0 ? toNum(o.attr('data-eq')) : 0;
    }

    /* CmbPackingType_Leave (:1442): EB/Unit = weightCutKg of (order, packing type). */
    function cmbPackingTypeLeave() {
        var pt = iOf('#detPackingTypeId'), po = iOf('#detPoNo');
        var row = (poTables.emptyBagCutByOrder || []).find(function (x) {
            return toInt(col(x, 'PurchaseOrderMasterId')) === po && toInt(col(x, 'PackingTypeId')) === pt;
        });
        $('#detEbUnit').val(row ? toNum(col(row, 'weightCutKg')) : 0);
        ebCalculations(); calculateNetWeight();
    }

    /* CmbBuyerNameDetail_Leave (:1460): Un-Loading City from the buyer's city when empty. */
    function cmbBuyerNameDetailLeave() {
        var b = iOf('#detBuyerId');
        if (!iOf('#unloadingCityId') && b > 0) {
            var p = LK.partyById[b];
            if (p && toInt(p.CityId) > 0) setVal('#unloadingCityId', p.CityId);
        }
    }

    /* CmbDeliveryToParty_Leave (:1488) -> BindShipToAddressAgainstBuyer. */
    function cmbDeliveryToPartyLeave() { bindShipTo(iOf('#detDeliverToPartyId')); }
    function bindShipTo(partyId) {
        var list = (LK.shipTo || []).filter(function (a) { return partyId <= 0 || toInt(a.SupplierCustomerId) === partyId; });
        setOptions('#detShipToAddressId', list, function (a) { return a.Id; }, function (a) { return a.AddressLine1 || a.name; },
            function (a) { return ' data-party-name="' + esc(a.CompanyName || '') + '" data-party-id="' + toInt(a.SupplierCustomerId) + '"'; });
    }

    /* CmbShipToAddress_Leave (:1648) */
    function cmbShipToAddressLeave() {
        var id = iOf('#detShipToAddressId'); if (!id) return;
        var pid = toInt($('#detShipToAddressId option:selected').attr('data-party-id'));
        if (pid > 0) setVal('#detDeliverToPartyId', pid);
        if (!deliverAddressTyped) $('#detDeliverToAddress').val(selText('#detShipToAddressId'));
    }

    /* ================================================================== detail: calculations */
    /* EbCalculations (:3043) */
    function ebCalculations() {
        var u = nOf('#detEbUnit'), t = nOf('#detEbTotal'), q = nOf('#detLoadingQty');
        if (q <= 0) { $('#detEbUnit').val(0); $('#detEbTotal').val(0); }
        else if (lastEbSource === 'unit') { $('#detEbTotal').val(r3(u > 0 ? u * q : 0)); calcByEbUnit = true; calcByEbTotal = false; }
        else if (lastEbSource === 'total') { $('#detEbUnit').val(r3(t > 0 ? t / q : 0)); calcByEbUnit = false; calcByEbTotal = true; }
        else if (calcByEbTotal) $('#detEbUnit').val(r3(t > 0 ? t / q : 0));
        else if (calcByEbUnit) $('#detEbTotal').val(r3(u > 0 ? u * q : 0));
    }
    function calculateGrossWeight() {   /* :3092 qty x PackUom Equivalent */
        $('#detGrossWeight').val(r3(nOf('#detLoadingQty') * packUomEquivalent()));
    }
    function calculateNetWeight() {     /* :3107 */
        $('#detNetBillWeight').val(f3(nOf('#detGrossWeight') - nOf('#detEbTotal') + nOf('#detAddLessWeight')));
    }

    /* ================================================================== detail: add / update */
    function need(sel, label, isNum) {
        var ok = isNum ? nOf(sel) > 0 : iOf(sel) > 0;
        if (!ok) { msg(label + ' Field Required'); $(sel).focus(); }
        return ok;
    }
    /* FormValidationDetail (:1715) */
    function formValidationDetail() {
        if (!need('#detParentItemId', 'Parent Item') || !need('#detItemId', 'Item Name') || !need('#detCropYearId', 'Crop Year')
            || !need('#detPackingTypeId', 'Packing Type') || !need('#detPackUomId', 'Pack Uom') || !need('#detLoadingQty', 'Qty', true)
            || !need('#detGrossWeight', 'Weight', true) || !need('#detEmptyBagTermId', 'Empty Bag Term')) return false;
        var po = selectedPoRow();
        if (po) {
            var exp = dOnly(col(po, 'ValidityDate'));
            if (exp && exp < $('#docDate').val()) { msg('Selected Purchase Order is Expired On: ' + ddMMMyyyy(exp)); $('#detPoNo').focus(); return false; }
            var it = selectedItemRow();
            var bal = it ? it.BalanceWeight : 0;
            var tol = toNum(cfg.tolerancePercentageForCmagtPoWeight);
            var allow = bal + bal * (tol > 0 ? tol / 100 : 0);
            if (nOf('#detGrossWeight') > allow) {
                msg("Weight can't be greater than Allowed Weight: " + allow.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 })
                    + ' (Tolerance: ' + tol + '%)');
                $('#detGrossWeight').focus(); return false;
            }
        }
        return true;
    }
    /* BtnAdd_Click / BtnUpdateDetail_Click shared PO date checks (:1800-1825) */
    function poDateChecks() {
        var po = selectedPoRow();
        if (!po) return true;
        var doc = $('#docDate').val();
        if (dOnly(col(po, 'ValidityDate')) === doc) {
            $('#detWarningRemarks').prop('disabled', false);
            if (!$.trim($('#detWarningRemarks').val())) {
                msg('Document Date is the same as the selected Purchase Order Expiry Date.\nPlease enter warning remarks before proceeding.');
                $('#detWarningRemarks').focus(); return false;
            }
        } else $('#detWarningRemarks').prop('disabled', true);
        var st = dOnly(col(po, 'DeliveryStartDate'));
        if (st && st > doc) { msg('Document Date is the Less then DeliveryStartDate of selected Purchase Order.'); $('#docDate').focus(); return false; }
        return true;
    }
    function duplicateCheck(skipIndex, suffix) {
        var it = selectedItemRow(); if (!it) return true;
        var dup = detailRows.some(function (r, i) { return i !== skipIndex && toInt(r._itemId) === it.ItemId && toInt(r._mappingId) === it.MappingId; });
        if (dup) { msg($('#detItemId option:selected').text() + " already in detail. So can't add this item" + suffix); $('#detItemId').focus(); }
        return !dup;
    }

    function btnAddClick() {                 /* :1792 */
        if (!formValidationDetail() || !poDateChecks() || !duplicateCheck(-1, '')) return;
        var row = { grnSupplierLoadingDetailId: 0 };
        fillCustomerDetailRow(row);
        detailRows.push(row);
        afterDetailChange();
    }
    function btnUpdateDetailClick() {        /* :1945 */
        if (!formValidationDetail() || updateDetailIndex < 0 || updateDetailIndex >= detailRows.length) return;
        if (!poDateChecks() || !duplicateCheck(updateDetailIndex, '.')) return;
        fillCustomerDetailRow(detailRows[updateDetailIndex]);
        afterDetailChange();
    }
    function afterDetailChange() {
        var poId = iOf('#detPoNo');
        renderDetailGrid();
        resetDetail();
        supplierEnabledState();
        getEmptyBagDetailOnBaseOfPO(poId);
        getExpensesOnBaseOfPO(poId);
        checkPurchaseOrderInDetailAndExpenses();
    }

    /* FillCustomerDetailRow (:1853) - grid row in the desktop's own columns, mapped to the
       grnSupplierLoadingDetail properties FillDetailListCommonForInsertAndDelete sends. */
    function fillCustomerDetailRow(row) {
        var po = selectedPoRow(), it = selectedItemRow() || {}, hasPo = !!po;
        row.purchaseOrderMasterId = iOf('#detPoNo');
        row._poNo = toInt($('#detPoNo option:selected').text());
        row.purchaseOrderDetailId = hasPo ? toInt(it.OrderDetailId) : 0;
        row._poExpiry = hasPo ? dOnly(col(po, 'ValidityDate')) : localYmd(new Date());
        row._poStart = hasPo ? dOnly(col(po, 'DeliveryStartDate')) : localYmd(new Date());
        row.inventoryParentCategoryId = iOf('#detParentItemId');
        row._parentItem = selText('#detParentItemId');
        row._itemId = toInt(it.ItemId);
        row.itemId = row._itemId;
        row.itemName = row._itemId > 0 ? it.ItemName : '';
        row._itemCode = row._itemId > 0 ? it.ItemCode : '';
        row.cropYearId = iOf('#detCropYearId');
        row.cropYear = selText('#detCropYearId');
        row.packingTypeId = iOf('#detPackingTypeId');
        row._packingType = selText('#detPackingTypeId');
        row.packUomId = iOf('#detPackUomId');
        row._packUom = selText('#detPackUomId');
        row._packUomEquivalent = packUomEquivalent();
        row.loadingQty = nOf('#detLoadingQty');
        row.wbGrossWeight = nOf('#detGrossWeight');
        row.ebwPerUnit = nOf('#detEbUnit');
        row.ebwTotal = nOf('#detEbTotal');
        row.addLessWeight = nOf('#detAddLessWeight');
        row.netBillWeight = toNum($('#detNetBillWeight').val());
        row.EBWeightDeductionTermId = iOf('#detEmptyBagTermId');
        row._emptyBagTerm = selText('#detEmptyBagTermId');
        row._mappingId = hasPo ? toInt(it.MappingId) : 0;
        row.purchaseOrderSaleOrderMappingId = row._mappingId;
        row.saleOrderMasterId = hasPo ? toInt(it.SoId) : 0;
        row.saleOrderDetailId = hasPo ? toInt(it.SoDetailId) : 0;
        row._soNo = hasPo ? toInt(it.SoNo) : 0;
        row.buyerId = iOf('#detBuyerId');
        row._buyerName = selText('#detBuyerId');
        row.DeliverToPartyId = iOf('#detDeliverToPartyId');
        row._deliverTo = selText('#detDeliverToPartyId');
        row.DeliverToAddressId = iOf('#detShipToAddressId');
        row.DeliverToAddress = $('#detDeliverToAddress').val();
        row.remarks = $('#detRemarks').val();
        row.warningRemarks = $('#detWarningRemarks').val();
    }

    /* grdDetail_DoubleClick (:2005) */
    function editDetailRow(i) {
        var r = detailRows[i]; if (!r) return;
        updateDetailIndex = i;
        var chain = $.Deferred().resolve().promise();
        if (toInt(r.purchaseOrderMasterId) > 0) { setVal('#detPoNo', r.purchaseOrderMasterId); cmbPoNoDetailLeave(true); }
        else { setVal('#detPoNo', 0); cmbPoNoDetailLeave(true); }
        setVal('#detParentItemId', r.inventoryParentCategoryId);
        bindItemCombo(toInt(r._mappingId) > 0 ? r._mappingId : r._itemId);
        chain = cmbItemNameLeave() || chain;
        setVal('#detCropYearId', r.cropYearId);
        setVal('#detPackingTypeId', r.packingTypeId);
        $('#detLoadingQty').val(r3(r.loadingQty));
        $('#detGrossWeight').val(r3(r.wbGrossWeight));
        $('#detEbUnit').val(r3(r.ebwPerUnit));
        $('#detEbTotal').val(r3(r.ebwTotal));
        $('#detAddLessWeight').val(r3(r.addLessWeight));
        $('#detNetBillWeight').val(f3(r.netBillWeight));
        setVal('#detEmptyBagTermId', r.EBWeightDeductionTermId);
        if (toInt(r.buyerId) > 0) setVal('#detBuyerId', r.buyerId);
        if (toInt(r.DeliverToPartyId) > 0) { setVal('#detDeliverToPartyId', r.DeliverToPartyId); cmbDeliveryToPartyLeave(); }
        if (toInt(r.DeliverToAddressId) > 0) setVal('#detShipToAddressId', r.DeliverToAddressId);
        $('#detDeliverToAddress').val(r.DeliverToAddress || '');
        $('#detRemarks').val(r.remarks || '');
        $('#detWarningRemarks').val(r.warningRemarks || '');
        chain.always(function () { setVal('#detPackUomId', r.packUomId); });
        $('#btnAddDetail').hide(); $('#btnUpdateDetail, #btnCancelDetail').show();
        $('#detPoNo').focus();
    }

    /* DeleteDetailRow (:2142) */
    function deleteDetailRow(i) {
        var r = detailRows[i]; if (!r) return;
        if (updateDetailIndex !== -1) { msg('Please Reset the Detail first..'); return; }
        if (toInt(r.grnSupplierLoadingDetailId) !== 0) {
            if (!window.confirm('Are you sure to Delete?')) return;
            removedRows.push($.extend({}, r, { actionTypeId: 3 }));
        }
        detailRows.splice(i, 1);
        renderDetailGrid();
        supplierEnabledState();
        checkPurchaseOrderInDetailAndExpenses();
    }

    /* CmbSupplierName.Enabled = !IsAnyCellValueGreaterThanZero(grdDetail, "PurchaseOrderId") */
    function supplierEnabledState() {
        $('#supplierId').prop('disabled', detailRows.some(function (r) { return toInt(r.purchaseOrderMasterId) > 0; }));
    }

    /* ResetDetail (:2957) */
    function resetDetail() {
        $('#detPoTotalWeight, #detPoReceivedWeight, #detPoBalanceWeight, #detSaleOrderNo').val('');
        $('#detItemId').val('0');
        $('#detPackUomId').html('<option value="0"></option>');
        $('#detLoadingQty, #detGrossWeight, #detEbUnit, #detEbTotal, #detNetBillWeight').val('');
        $('#detEmptyBagTermId').val('0').prop('disabled', false);
        $('#detBuyerId').val('0').prop('disabled', false);
        $('#detShipToAddressId').val('0');
        $('#detDeliverToAddress, #detRemarks, #detWarningRemarks').val('');
        $('#detWarningRemarks').prop('disabled', true);
        deliverAddressTyped = false;
        calcByEbUnit = calcByEbTotal = false;
        updateDetailIndex = -1;
        $('#btnAddDetail').show(); $('#btnUpdateDetail, #btnCancelDetail').hide();
        if (poSelected()) { $('#detParentItemId, #detEmptyBagTermId, #detBuyerId').prop('disabled', true); setVal('#detEmptyBagTermId', toInt(col(selectedPoRow(), 'EBWeightDeductionTermId'))); }
        $('#detPoNo').focus();
    }

    /* grdDetailSetting (:2080) + DetailGridCommonSetting */
    function renderDetailGrid() {
        var showPo = detailRows.some(function (r) { return toInt(r.purchaseOrderMasterId) > 0; });
        $('#gridItems thead .col-po').toggle(showPo);
        var tb = $('#gridItems tbody').empty();
        var t = { q: 0, g: 0, eu: 0, et: 0, al: 0, n: 0 };
        detailRows.forEach(function (r, i) {
            t.q += toNum(r.loadingQty); t.g += toNum(r.wbGrossWeight); t.eu += toNum(r.ebwPerUnit);
            t.et += toNum(r.ebwTotal); t.al += toNum(r.addLessWeight); t.n += toNum(r.netBillWeight);
            var po = showPo ? '' : ' style="display:none"';
            tb.append('<tr data-i="' + i + '"' + (i === updateDetailIndex ? ' class="sel"' : '') + '>'
                + '<td><button type="button" class="btn btn-sm btn-outline-danger py-0 d-del">X</button></td>'
                + '<td><button type="button" class="btn btn-sm btn-outline-primary py-0 d-edit">Edit</button></td>'
                + '<td' + po + '>' + (toInt(r.purchaseOrderMasterId) > 0 ? esc(r._poNo) : '') + '</td>'
                + '<td' + po + '>' + esc(ddMMMyy(r._poStart)) + '</td><td' + po + '>' + esc(ddMMMyy(r._poExpiry)) + '</td>'
                + '<td>' + esc(r._parentItem) + '</td><td>' + esc(r._itemCode) + '</td><td>' + esc(r.itemName) + '</td>'
                + '<td>' + esc(r.cropYear) + '</td><td>' + esc(r._packingType) + '</td><td>' + esc(r._packUom) + '</td>'
                + '<td class="num">' + f3(r.loadingQty) + '</td><td class="num">' + f3(r.wbGrossWeight) + '</td>'
                + '<td class="num">' + f3(r.ebwPerUnit) + '</td><td class="num">' + f3(r.ebwTotal) + '</td>'
                + '<td class="num">' + f3(r.addLessWeight) + '</td><td class="num">' + f3(r.netBillWeight) + '</td>'
                + '<td>' + esc(r._emptyBagTerm) + '</td><td' + po + '>' + (toInt(r._soNo) || '') + '</td>'
                + '<td>' + esc(r._buyerName) + '</td><td>' + esc(r._deliverTo) + '</td><td>' + esc(r.DeliverToAddress) + '</td>'
                + '<td>' + esc(r.remarks) + '</td><td>' + esc(r.warningRemarks) + '</td></tr>');
        });
        if (!detailRows.length) tb.append('<tr><td colspan="24" class="text-center text-muted">No rows.</td></tr>');
        var lead = showPo ? 11 : 8;
        $('#detailTotals').html('<td colspan="' + lead + '" class="text-end">Total</td><td class="num">' + f3(t.q) + '</td><td class="num">' + f3(t.g)
            + '</td><td class="num">' + f3(t.eu) + '</td><td class="num">' + f3(t.et) + '</td><td class="num">' + f3(t.al) + '</td><td class="num">'
            + f3(t.n) + '</td><td colspan="' + (showPo ? 7 : 6) + '"></td>');
        recNav('#gridItems', '#navDetail');
    }

    /* ================================================================== SupfrmShipToAddress
     * Same contract as the PO screen's "+" (PurchaseOrderCmagtRestController /ship-to/*):
     * suppliers = SupplierCustomerGetforComboServiceBind, countries = Country.GetAll, cities = City.GetAll(org, company),
     * grid = SupplierCustomerShipToAddress.FormHistory(org, company), save = BLL Save (Insert/Update by RecId). */
    var ST_API = '/api/commission/purchase-order/ship-to';
    function stOpen() {
        $('#shipToModal').addClass('show');
        stReset();
        return $.when(stCombos(), stGrid());
    }
    function stClose() {
        $('#shipToModal').removeClass('show');
        /* the combo on the form is rebound so a newly defined address can be picked (AllShipToAddressDbCall :568) */
        getJSON(DD + '/ship-to-addresses').done(function (d) { LK.shipTo = d || []; bindShipTo(iOf('#detDeliverToPartyId')); });
    }
    function stCombos() {
        return $.when(
            getJSON(DD + '/suppliers').done(function (d) {
                setOptions('#stParty', d, function (x) { return x.Id; }, function (x) { return x.CompanyName || x.name; });
            }),
            getJSON(ST_API + '/countries').done(function (d) {
                setOptions('#stCountry', d, function (x) { return col(x, 'Id'); }, function (x) { return col(x, 'Description') || col(x, 'CountryName'); });
            }),
            getJSON(ST_API + '/cities').done(function (d) {                 /* cmbcountry_Leave binds all cities of the company */
                setOptions('#stCity', d, function (x) { return col(x, 'Id'); }, function (x) { return col(x, 'CityName'); });
            }));
    }
    function stGrid() {
        return getJSON(ST_API + '/history').done(function (rows) {
            var tb = $('#gridShipTo tbody').empty();
            (rows || []).forEach(function (r) {
                tb.append('<tr data-id="' + toInt(col(r, 'Id')) + '"><td>' + esc(col(r, 'PartyName')) + '</td><td>' + esc(col(r, 'AddressTitle')) + '</td><td>'
                    + esc(col(r, 'CountryName')) + '</td><td>' + esc(col(r, 'CityName')) + '</td><td>' + esc(col(r, 'ContactPerson')) + '</td><td>'
                    + esc(col(r, 'PhoneNo')) + '</td><td>' + esc(col(r, 'MobileNo')) + '</td><td>' + esc(col(r, 'WhatsAppNo')) + '</td><td>'
                    + esc(dateTime(col(r, 'EntryDate'))) + '</td><td>' + esc(col(r, 'EntryUser')) + '</td><td>' + esc(dateTime(col(r, 'ModifyDate')))
                    + '</td><td>' + esc(col(r, 'ModifyUser')) + '</td><td>' + esc(col(r, 'AddressLine1')) + '</td></tr>');
            });
            if (!(rows || []).length) tb.append('<tr><td colspan="13" class="text-center text-muted">No rows.</td></tr>');
        });
    }
    function stReset() {
        $('#stId').val(0);
        $('#stParty, #stCountry, #stCity').val('0');
        $('#stTitle, #stAddress, #stContact, #stPhone, #stMobile, #stWhatsApp').val('');
        $('#btnStUpdate').hide(); $('#btnStSave').show();
    }
    function stEdit(id) {
        if (!id) return;
        getJSON(ST_API + '/' + id).done(function (b) {
            $('#btnStSave').hide(); $('#btnStUpdate').show();
            $('#stId').val(id);
            setVal('#stParty', toInt(col(b, 'SupplierCustomerId')));
            setVal('#stCountry', toInt(col(b, 'CountryId')));
            setVal('#stCity', toInt(col(b, 'CityId')));
            $('#stAddress').val(col(b, 'AddressLine1') || ''); $('#stTitle').val(col(b, 'AddressTitle') || '');
            $('#stWhatsApp').val(col(b, 'WhatsAppNo') || ''); $('#stContact').val(col(b, 'ContactPerson') || '');
            $('#stMobile').val(col(b, 'MobileNo') || ''); $('#stPhone').val(col(b, 'PhoneNo') || '');
        }).fail(function (x) { msg((x.responseJSON && x.responseJSON.message) || 'Could not read the address.'); });
    }
    function stSave(btn) {
        /* FormValidation (:117) - same order and messages */
        if (!iOf('#stParty')) { msg('Please Select Supplier'); return; }
        if (!$.trim($('#stAddress').val())) { msg('Please Enter Address'); return; }
        if (!$.trim($('#stTitle').val())) { msg('Please Enter Address Title'); return; }
        if (!iOf('#stCountry')) { msg('Please Select Country'); return; }
        if (!iOf('#stCity')) { msg('Please Select City'); return; }
        withButton(btn, function () {
            return $.ajax({ url: ST_API + '/save', type: 'POST', contentType: 'application/json', dataType: 'json',
                data: JSON.stringify({ Id: iOf('#stId'), SupplierCustomerId: iOf('#stParty'), CountryId: iOf('#stCountry'), CityId: iOf('#stCity'),
                    AddressLine1: $('#stAddress').val(), AddressTitle: $('#stTitle').val(), PhoneNo: $('#stPhone').val(),
                    MobileNo: $('#stMobile').val(), WhatsAppNo: $('#stWhatsApp').val(), ContactPerson: $('#stContact').val() }) })
                .done(function (d) {
                    if (!d || !d.success) { msg((d && d.message) || 'Save refused.'); return; }
                    msg(d.message || (iOf('#stId') ? 'Update Successfully' : 'Save Successfully'));
                    stReset(); stGrid();
                })
                .fail(function (x) { msg((x.responseJSON && x.responseJSON.message) || 'Save failed.'); });
        });
    }

    /* ================================================================== empty bags / expenses */
    function newEbRow() { return { Id: 0, PoId: 0, PoEbId: 0, PurchaseOrderNo: 0, PackingType: 0, PurchaseRate: 0, EmptyBagItem: 0 }; }
    function newExpRow() { return { Id: 0, PoId: 0, PoExpenseId: 0, PurchaseOrderNo: 0, ItemId: 0, Qty: 0, Rate: 0, Amount: 0, Remarks: '' }; }
    function resetChildGrids() { ebRows = [newEbRow()]; expRows = [newExpRow()]; renderEbGrid(); renderExpGrid(); }
    function opts(list, vk, tk, sel) {
        var h = '<option value="0"></option>';
        (list || []).forEach(function (d) {
            var v = toInt(col(d, vk));
            h += '<option value="' + v + '"' + (v === toInt(sel) ? ' selected' : '') + '>' + esc(col(d, tk) || '') + '</option>';
        });
        return h;
    }
    function renderEbGrid() {
        var showPo = ebRows.some(function (r) { return toInt(r.PoId) > 0; });
        $('#gridEmptyBags thead .col-pono').toggle(showPo);
        var tb = $('#gridEmptyBags tbody').empty();
        ebRows.forEach(function (r, i) {
            tb.append('<tr data-i="' + i + '"><td><button type="button" class="btn btn-sm btn-outline-danger py-0 eb-del">X</button></td>'
                + '<td><button type="button" class="btn btn-sm btn-outline-primary py-0 eb-add">+</button></td>'
                + '<td' + (showPo ? '' : ' style="display:none"') + '>' + (toInt(r.PoId) > 0 ? esc(r.PurchaseOrderNo) : '') + '</td>'
                + '<td><select class="form-select form-select-sm eb-pt" data-dtcombo="single" data-dtcombo-caption="Packing Type">' + opts(LK.ebPackTypes, 'Id', 'ReferenceName', r.PackingType) + '</select></td>'
                + '<td><input type="number" step="any" class="form-control form-control-sm text-end eb-rate" value="' + toNum(r.PurchaseRate) + '"></td>'
                + '<td><select class="form-select form-select-sm eb-item" data-dtcombo="single" data-dtcombo-caption="Empty Bag Item">' + opts(LK.ebItems, 'ItemId', 'ItemName', r.EmptyBagItem) + '</select></td></tr>');
        });
        ebTotals();
        recNav('#gridEmptyBags', '#navEb');
    }
    /* grdEmptyBags.TotalRow = True (:5710): numeric columns summed (GridEX_Helper.GridWrappingAndColumnSettings, 3 decimals). */
    function ebTotals() {
        var showPo = ebRows.some(function (r) { return toInt(r.PoId) > 0; }), t = 0;
        ebRows.forEach(function (r) { t += toNum(r.PurchaseRate); });
        $('#ebTotals').html('<td colspan="' + (showPo ? 4 : 3) + '">Total</td><td class="num">' + f3(t) + '</td><td></td>');
    }
    /* grdInvExp.TotalRow = True (:5661) */
    function expTotals() {
        var showPo = expRows.some(function (r) { return toInt(r.PoId) > 0; }), q = 0, rt = 0, a = 0;
        expRows.forEach(function (r) { q += toNum(r.Qty); rt += toNum(r.Rate); a += toNum(r.Amount); });
        $('#expTotals').html('<td colspan="' + (showPo ? 4 : 3) + '">Total</td><td class="num">' + f3(q) + '</td><td class="num">' + f3(rt)
            + '</td><td class="num">' + f3(a) + '</td><td></td>');
    }
    /* GridEX RecordNavigator = true (grdDetail :5628, grdInvExp :5659, grdEmptyBags :5708): |< < Record n of N > >| */
    var navPos = {};
    function recNav(gridSel, navSel, pos) {
        var rows = $(gridSel + ' tbody tr[data-i]'), n = rows.length;
        var p = pos != null ? pos : (navPos[gridSel] != null ? navPos[gridSel] : 0);
        if (p >= n) p = n - 1; if (p < 0) p = 0;
        navPos[gridSel] = p;
        rows.removeClass('rn-cur'); if (n) rows.eq(p).addClass('rn-cur');
        $(navSel).html('<button type="button" data-g="' + gridSel + '" data-n="' + navSel + '" data-go="first"' + (p <= 0 ? ' disabled' : '') + '>|&lt;</button>'
            + '<button type="button" data-g="' + gridSel + '" data-n="' + navSel + '" data-go="prev"' + (p <= 0 ? ' disabled' : '') + '>&lt;</button>'
            + '<span class="rn-txt">Record: ' + (n ? p + 1 : 0) + ' of ' + n + '</span>'
            + '<button type="button" data-g="' + gridSel + '" data-n="' + navSel + '" data-go="next"' + (p >= n - 1 ? ' disabled' : '') + '>&gt;</button>'
            + '<button type="button" data-g="' + gridSel + '" data-n="' + navSel + '" data-go="last"' + (p >= n - 1 ? ' disabled' : '') + '>&gt;|</button>');
    }
    function renderExpGrid() {
        var showPo = expRows.some(function (r) { return toInt(r.PoId) > 0; });
        $('#gridExpenses thead .col-pono').toggle(showPo);
        var tb = $('#gridExpenses tbody').empty();
        expRows.forEach(function (r, i) {
            tb.append('<tr data-i="' + i + '"><td><button type="button" class="btn btn-sm btn-outline-danger py-0 ex-del">X</button></td>'
                + '<td><button type="button" class="btn btn-sm btn-outline-primary py-0 ex-add">+</button></td>'
                + '<td' + (showPo ? '' : ' style="display:none"') + '>' + (toInt(r.PoId) > 0 ? esc(r.PurchaseOrderNo) : '') + '</td>'
                + '<td><select class="form-select form-select-sm ex-item" data-dtcombo="single" data-dtcombo-caption="Other Item Name">' + opts(LK.otherItems, 'Id', 'OtherItemName', r.ItemId) + '</select></td>'
                + '<td><input type="number" step="any" class="form-control form-control-sm text-end ex-qty" value="' + toNum(r.Qty) + '"></td>'
                + '<td><input type="number" step="any" class="form-control form-control-sm text-end ex-rate" value="' + toNum(r.Rate) + '"></td>'
                + '<td><input type="number" step="any" class="form-control form-control-sm text-end ex-amt" value="' + toNum(r.Amount) + '"></td>'
                + '<td><input type="text" class="form-control form-control-sm ex-rem" value="' + esc(r.Remarks || '') + '"></td></tr>');
        });
        expTotals();
        recNav('#gridExpenses', '#navExp');
    }
    function rowIdx(el) { return toInt($(el).closest('tr').attr('data-i')); }
    function wireChildGrids() {
        $(document).on('click', '.recnav button', function () {
            var g = $(this).data('g'), n = $(this).data('n'), c = $(g + ' tbody tr[data-i]').length, p = navPos[g] || 0;
            var go = $(this).data('go');
            p = go === 'first' ? 0 : go === 'prev' ? p - 1 : go === 'next' ? p + 1 : c - 1;
            recNav(g, n, p);
        });
        $(document).on('click focusin', '#gridItems tbody tr[data-i], #gridEmptyBags tbody tr[data-i], #gridExpenses tbody tr[data-i]', function () {
            var g = '#' + $(this).closest('table').attr('id');
            recNav(g, g === '#gridItems' ? '#navDetail' : g === '#gridEmptyBags' ? '#navEb' : '#navExp', toInt($(this).attr('data-i')));
        });
        $(document).on('input change', '#gridEmptyBags .eb-rate', function () { setTimeout(ebTotals, 0); });
        $(document).on('change', '#gridExpenses .ex-qty, #gridExpenses .ex-rate, #gridExpenses .ex-amt', function () { setTimeout(expTotals, 0); });
        /* grdEmptyBags_ColumnButtonClick (:2475) / DeleteRowInEmptyBagGrid (:2458) */
        $(document).on('click', '#gridEmptyBags .eb-add', function () { ebRows.push(newEbRow()); renderEbGrid(); });
        $(document).on('click', '#gridEmptyBags .eb-del', function () { ebRows.splice(rowIdx(this), 1); if (!ebRows.length) ebRows.push(newEbRow()); renderEbGrid(); });
        $(document).on('change', '#gridEmptyBags .eb-pt', function () { ebRows[rowIdx(this)].PackingType = toInt(this.value); });
        $(document).on('change', '#gridEmptyBags .eb-item', function () { ebRows[rowIdx(this)].EmptyBagItem = toInt(this.value); });
        $(document).on('input change', '#gridEmptyBags .eb-rate', function () { ebRows[rowIdx(this)].PurchaseRate = toNum(this.value); });
        /* grdInvExp_ColumnButtonClick (:2323) / DeleteRowInExpenseGrid (:2239) */
        $(document).on('click', '#gridExpenses .ex-add', function () { expRows.push(newExpRow()); renderExpGrid(); });
        $(document).on('click', '#gridExpenses .ex-del', function () { expRows.splice(rowIdx(this), 1); if (!expRows.length) expRows.push(newExpRow()); renderExpGrid(); });
        $(document).on('change', '#gridExpenses .ex-item', function () { expRows[rowIdx(this)].ItemId = toInt(this.value); });
        $(document).on('input change', '#gridExpenses .ex-rem', function () { expRows[rowIdx(this)].Remarks = this.value; });
        /* grdInvExp_CellUpdated (:2348): Qty/Rate -> Amount (UpdateAmount); Amount -> Rate (UpdateRate). */
        $(document).on('change', '#gridExpenses .ex-qty, #gridExpenses .ex-rate, #gridExpenses .ex-amt', function () {
            var tr = $(this).closest('tr'), r = expRows[rowIdx(this)];
            r.Qty = toNum(tr.find('.ex-qty').val()); r.Rate = toNum(tr.find('.ex-rate').val()); r.Amount = toNum(tr.find('.ex-amt').val());
            if ($(this).hasClass('ex-amt')) { r.Rate = r.Qty === 0 ? 0 : r.Amount / r.Qty; tr.find('.ex-rate').val(r.Rate); }
            else { r.Amount = r.Qty * r.Rate; tr.find('.ex-amt').val(r.Amount); }
        });
    }
    /* GetEmptyBagDetailOnBaseOfPO (:1190) */
    function getEmptyBagDetailOnBaseOfPO(poId) {
        if (!ebRows.some(function (r) { return toNum(r.PurchaseRate) > 0; })) ebRows = [];
        var keys = {};
        ebRows.forEach(function (r) { keys[toInt(r.PoId) + '|' + toInt(r.PoEbId)] = 1; });
        (poTables.emptyBagsByOrder || []).forEach(function (x) {
            var pid = toInt(col(x, 'purchaseOrderMasterId')); if (pid !== poId) return;
            var eb = toInt(col(x, 'purchaseOrderEmptyBagDetailId')); if (keys[pid + '|' + eb]) return;
            keys[pid + '|' + eb] = 1;
            ebRows.push({ Id: 0, PoId: pid, PoEbId: eb, PurchaseOrderNo: toInt(col(x, 'PurchaseOrderNo')), PackingType: toInt(col(x, 'PackingTypeId')),
                          PurchaseRate: toNum(col(x, 'Rate')), EmptyBagItem: toInt(col(x, 'emptyBagPackingMaterialItemId')) });
        });
    }
    /* GetExpensesOnBaseOfPO (:1223) */
    function getExpensesOnBaseOfPO(poId) {
        if (!expRows.some(function (r) { return toNum(r.Amount) > 0; })) expRows = [];
        var keys = {};
        expRows.forEach(function (r) { keys[toInt(r.PoId) + '|' + toInt(r.PoExpenseId)] = 1; });
        (poTables.expensesByOrder || []).forEach(function (x) {
            var pid = toInt(col(x, 'purchaseOrderMasterId')); if (pid !== poId) return;
            var ex = toInt(col(x, 'purchaseOrderSupplierExpenseDetailId')); if (keys[pid + '|' + ex]) return;
            keys[pid + '|' + ex] = 1;
            expRows.push({ Id: 0, PoId: pid, PoExpenseId: ex, PurchaseOrderNo: toInt(col(x, 'PurchaseOrderNo')), ItemId: toInt(col(x, 'ItemId')),
                           Qty: toNum(col(x, 'Qty')), Rate: toNum(col(x, 'rate')), Amount: toNum(col(x, 'amount')), Remarks: col(x, 'remarks') || '' });
        });
    }
    /* CheckPurchaseOrderInDetailAndExpenses (:1273) + RemoveInvalidRows (:1257) */
    function checkPurchaseOrderInDetailAndExpenses() {
        var valid = {};
        detailRows.forEach(function (r) { valid[toInt(r.purchaseOrderMasterId)] = 1; });
        var keep = function (r) { var p = toInt(r.PoId); return !(p > 0 && !valid[p]); };
        expRows = expRows.filter(keep); ebRows = ebRows.filter(keep);
        if (!expRows.length) expRows.push(newExpRow());
        if (!ebRows.length) ebRows.push(newEbRow());
        renderEbGrid(); renderExpGrid();
    }

    /* ================================================================== save (Insert :2539) */
    function reqHeader() {
        var list = [['#companyId', 'Company Name', 'i'], ['#branchId', 'Branch Name', 'i'], ['#docNo', 'Doc No', 'i'],
            ['#commissionAgentId', 'Commission Agent', 'i'], ['#supplierId', 'Supplier Name', 'i'], ['#loadingCityId', 'Loading City', 'i'],
            ['#unloadingCityId', 'Un-Loading City', 'i'], ['#vehicleTypeId', 'Vehicle Type', 'i'], ['#vehicleNo', 'Vehicle No', 's'],
            ['#biltyNo', 'Bilty No', 's'], ['#deliveryTermId', 'Delivery Term', 'i'], ['#biltyQty', 'Vehicle Qty', 'n'], ['#scaleNetWeight', 'Net Wb Weight', 'n']];
        for (var i = 0; i < list.length; i++) {
            var s = list[i][0], t = list[i][2];
            var ok = t === 's' ? !!$.trim($(s).val()) : t === 'n' ? nOf(s) > 0 : iOf(s) > 0;
            if (!ok) { msg(list[i][1] + ' Field Required'); $(s).focus(); return false; }
        }
        return true;
    }

    function insert() {
        if (!detailRows.length) { msg('Detail Record Not Found'); return; }
        if (toNum($('#totalFreight').val()) > 0 && !iOf('#transporterId') && !$.trim($('#transporterName').val())) {
            msg('Please Select Transporter or Fill Transporter Name'); $('#transporterId').focus(); return;
        }
        var v = $.trim($('#vehicleNo').val()).toUpperCase();
        if (!/^[A-Z]{1,6}-\d{1,6}$/.test(v)) { msg('Vehicle no is not valid. Please check!'); $('#vehicleNo').focus(); return; }
        var vp = v.split('-');
        if ((vp[0].length <= 2 || vp[1].length <= 2)
            && !window.confirm('The vehicle number is unusually short (letters or digits less than or equal to 2).\nAre you sure you want to proceed?')) {
            $('#vehicleNo').focus(); return;
        }
        var doc = $('#docDate').val(), bilty = $('#biltyDate').val();
        if (cfg.grnLoadingDocDateAndBiltyDateSame && doc !== bilty) { msg("Doc Date And Bilty Date Can't be different"); $('#biltyDate').focus(); return; }
        if (cfg.grnLoadingAllowDifferentBiltyDate && doc !== bilty
            && !window.confirm('Document Date and Bilty Date are different.\nDo you want to continue?')) { $('#biltyDate').focus(); return; }
        if (!reqHeader()) return;
        if (!window.confirm(RecId === 0 ? 'Are you sure to Save' : 'Are you sure to Update')) return;

        var gross = 0, details = [];
        for (var i = 0; i < detailRows.length; i++) {
            var r = detailRows[i];
            var id = RecId !== 0 ? toInt(r.grnSupplierLoadingDetailId) : 0;
            var labels = [['inventoryParentCategoryId', 'Parent Category'], ['itemId', 'Item Name'], ['cropYearId', 'Crop Year'], ['packingTypeId', 'Packing Type'],
                          ['packUomId', 'Pack Uom'], ['loadingQty', 'Loading Qty'], ['wbGrossWeight', 'Gross Weight'], ['netBillWeight', 'Net Bill Weight']];
            for (var j = 0; j < labels.length; j++) {
                if (toNum(r[labels[j][0]]) === 0) { msg(labels[j][1] + ' is required in detail row ' + (i + 1)); return; }
            }
            gross += toNum(r.wbGrossWeight);
            if (toInt(r.purchaseOrderMasterId) > 0) {
                if (doc === r._poExpiry && !$.trim(r.warningRemarks || '')) {
                    msg('Vehicle has reached late compared to PO Expiry Date.\n\nRemarks are required to proceed.');
                    var w = window.prompt('Late Vehicle Remarks', '');
                    if (w === null) { $('#docDate').focus(); return; }
                    if (!$.trim(w)) { msg('Remarks are required for late vehicle entry.'); $('#docDate').focus(); return; }
                    r.warningRemarks = w;
                    renderDetailGrid();
                } else {
                    if (r._poExpiry && doc > r._poExpiry) { msg('Grn Date is greater than Purchase Order Expiry date.'); $('#docDate').focus(); return; }
                    if (r._poStart && doc < r._poStart) { msg('Grn Date is less than Purchase Order Delivery Start date.'); $('#docDate').focus(); return; }
                }
            }
            details.push(detailPayload(r, id, id <= 0 ? 1 : 2));
        }
        var net = nOf('#scaleNetWeight');
        if (r3(net) !== r3(gross)) {
            msg('Header Net Weight:' + f3(net) + ' not equal to detail gross weight:' + f3(gross) + '. please Check!'); return;
        }
        var removed = (RecId > 0) ? removedRows.map(function (x) { return detailPayload(x, toInt(x.grnSupplierLoadingDetailId), 3); }) : [];
        var freight = toNum($('#totalFreight').val()) > 0;
        var payload = {
            grnSupplierLoadingMasterId: RecId,
            companyId: iOf('#companyId'), branchId: iOf('#branchId'),
            docNo: iOf('#docNo'), docDate: doc,
            commissionAgentId: iOf('#commissionAgentId'), supplierId: iOf('#supplierId'),
            loadingCityId: iOf('#loadingCityId'), unloadingCityId: iOf('#unloadingCityId'),
            transporterId: freight ? iOf('#transporterId') : 0,
            transporterName: freight ? $('#transporterName').val() : null,
            biltyFreight: freight ? nOf('#biltyFreight') : 0,
            otherAdLesCharges: freight ? nOf('#otherAdLesCharges') : 0,
            totalFreight: freight ? toNum($('#totalFreight').val()) : 0,
            vehicleTypeId: iOf('#vehicleTypeId'), vehicleNo: $.trim($('#vehicleNo').val()),
            biltyDate: bilty, biltyNo: $.trim($('#biltyNo').val()),
            deliveryTermId: iOf('#deliveryTermId'), biltyQty: nOf('#biltyQty'),
            loadWeight: nOf('#loadWeight'), tareWeight: nOf('#tareWeight'), scaleNetWeight: net,
            remarksHeader: $('#remarksHeader').val(),
            grnSupplierLoadingDetailList: removed.concat(details),
            grnSupplierLoadingEmptyBagDetailList: buildEmptyBagList(),
            grnSupplierLoadingExpenseDetailList: buildExpenseList()
        };
        var wasUpdate = RecId > 0;
        return $.ajax({ url: API + '/save', type: 'POST', contentType: 'application/json', data: JSON.stringify(payload) })
            .done(function (res) {
                if (res && res.status === 'SUCCESS') {
                    msg(wasUpdate ? 'Update Successfully' : 'Save Successfully');
                    var newId = toInt(res.id);
                    reset();
                    if ($('#chkPrint').is(':checked')) openPrint(newId, 'slip');
                } else msg((res && res.message) || 'Save failed');
            })
            .fail(function (x) { msg((x && x.responseJSON && x.responseJSON.message) || 'Server error while saving.'); });
    }

    function detailPayload(r, id, actionTypeId) {
        return { grnSupplierLoadingDetailId: id, actionTypeId: actionTypeId,
            purchaseOrderMasterId: toInt(r.purchaseOrderMasterId), purchaseOrderDetailId: toInt(r.purchaseOrderDetailId),
            inventoryParentCategoryId: toInt(r.inventoryParentCategoryId), itemId: toInt(r.itemId),
            cropYearId: toInt(r.cropYearId), cropYear: r.cropYear, packingTypeId: toInt(r.packingTypeId), packUomId: toInt(r.packUomId),
            loadingQty: toNum(r.loadingQty), wbGrossWeight: toNum(r.wbGrossWeight), ebwPerUnit: toNum(r.ebwPerUnit), ebwTotal: toNum(r.ebwTotal),
            addLessWeight: toNum(r.addLessWeight), netBillWeight: toNum(r.netBillWeight), EBWeightDeductionTermId: toInt(r.EBWeightDeductionTermId),
            purchaseOrderSaleOrderMappingId: toInt(r.purchaseOrderSaleOrderMappingId), saleOrderMasterId: toInt(r.saleOrderMasterId),
            saleOrderDetailId: toInt(r.saleOrderDetailId), buyerId: toInt(r.buyerId), DeliverToPartyId: toInt(r.DeliverToPartyId),
            DeliverToAddressId: toInt(r.DeliverToAddressId), DeliverToAddress: r.DeliverToAddress || '',
            remarks: r.remarks || '', warningRemarks: r.warningRemarks || '' };
    }
    /* Insert() :2711-2725 */
    function buildEmptyBagList() {
        return ebRows.filter(function (r) { return toInt(r.EmptyBagItem) > 0 && toInt(r.PackingType) > 0; }).map(function (r) {
            return { grnSupplierLoadingEmptyBagDetailId: 0, purchaseOrderMasterId: toInt(r.PoId), purchaseOrderEmptyBagDetailId: toInt(r.PoEbId),
                     PackingTypeId: toInt(r.PackingType), Rate: toNum(r.PurchaseRate), emptyBagPackingMaterialItemId: toInt(r.EmptyBagItem),
                     weightCutKg: 0, sortNo: 0 };
        });
    }
    /* Insert() :2726-2744 */
    function buildExpenseList() {
        var txt = function (id) { var o = (LK.otherItems || []).find(function (d) { return toInt(col(d, 'Id')) === id; }); return o ? $.trim(col(o, 'OtherItemName') || '') : ''; };
        return expRows.filter(function (r) { return toInt(r.ItemId) !== 0 && toNum(r.Amount) > 0; }).map(function (r) {
            var rem = $.trim(r.Remarks || '');
            if (rem === '0' || rem === '') rem = 'Expense : ' + txt(toInt(r.ItemId)) + '  Qty' + toNum(r.Qty) + '  @' + toNum(r.Rate);
            return { grnSupplierLoadingExpenseDetailId: 0, purchaseOrderMasterId: toInt(r.PoId), purchaseOrderExpenseDetailId: toInt(r.PoExpenseId),
                     ItemId: toInt(r.ItemId), Qty: toNum(r.Qty), rate: toNum(r.Rate), amount: toNum(r.Amount), remarks: rem, sortNo: 0 };
        });
    }

    /* btnDelete_Click (:2874) */
    function deleteRecord() {
        if (!(RecId > 0)) { msg('No record found to Delete'); return; }
        if (!window.confirm('Are you sure to Delete?')) return;
        return $.post(API + '/' + RecId + '/delete').done(function (res) {
            if (res && res.status === 'SUCCESS') { msg('Delete Record Successfully'); reset(); }
            else msg((res && res.message) || 'Delete failed');
        }).fail(function () { msg('Server error while deleting.'); });
    }

    /* OpenPrint (:3369) / OpenChallanPrint (:3391) - GrnSupplierLoadingSlip_1054 and
       GrnSupplierLoadingChallanSlip_1054_01 are not registered in the web report engine yet. */
    function openPrint(id, kind) {
        if (!(id > 0)) { msg('No Data found to display'); return; }
        msg((kind === 'challan' ? 'Loading Challan print (1054_01)' : 'GRN Supplier Loading Slip (1054)')
            + ' is not available on the web yet.');
    }

    /* ================================================================== reset / read */
    /* Reset (:2910) */
    function reset(forRead) {
        RecId = 0;
        $('#grnSupplierLoadingMasterId').val(0);
        $('#btnSave').show(); $('#btnUpdate, #btnDelete').hide();
        $('#commissionAgentId, #supplierId, #loadingCityId, #unloadingCityId, #transporterId, #detDeliverToPartyId').val('0');
        $('#transporterName, #biltyFreight, #otherAdLesCharges, #totalFreight, #vehicleNo, #biltyNo, #biltyQty, #loadWeight, #tareWeight, #scaleNetWeight, #avgFillingPerBag, #remarksHeader').val('');
        wbNetAccessible = ''; transporterNameTyped = false;
        resetDetail();
        detailRows = []; removedRows = [];
        renderDetailGrid();
        resetChildGrids();
        $('#supplierId').prop('disabled', false);
        $('#docNo').val('');
        if (forRead === true) return;   /* ReadById fills everything and re-reads the loader itself */
        loadPendingOrders();
        loadDocNo();
        applyConfigDefaults(false);
        $('#docDate').focus();
    }

    /* ReadById (:2810) */
    function readById(id) {
        if (!(id > 0)) return;
        return getJSON(API + '/' + id).done(function (res) {
            if (!(res && res.status === 'SUCCESS' && res.data)) { msg((res && res.message) || 'Record Not Found'); return; }
            reset(true);
            var d = res.data;
            RecId = id;
            $('#grnSupplierLoadingMasterId').val(id);
            showTab('form');
            $('#btnSave').hide(); $('#btnUpdate, #btnDelete').show();
            $('#docNo').val(col(d, 'docNo'));
            $('#docDate').val(dOnly(col(d, 'docDate')));
            setVal('#companyId', col(d, 'companyId'));
            setVal('#branchId', col(d, 'branchId'));
            setVal('#commissionAgentId', col(d, 'commissionAgentId'));
            setVal('#supplierId', col(d, 'supplierId'));
            setVal('#loadingCityId', col(d, 'loadingCityId'));
            setVal('#unloadingCityId', col(d, 'unloadingCityId'));
            if (toInt(col(d, 'transporterId')) > 0) setVal('#transporterId', col(d, 'transporterId'));
            $('#transporterName').val(col(d, 'transporterName') || '');
            $('#biltyFreight').val(r3(col(d, 'biltyFreight')));
            $('#otherAdLesCharges').val(r3(col(d, 'otherAdLesCharges')));
            $('#totalFreight').val(f3(col(d, 'totalFreight')));
            setVal('#vehicleTypeId', col(d, 'vehicleTypeId'));
            $('#vehicleNo').val(col(d, 'vehicleNo') || '');
            $('#biltyNo').val(col(d, 'biltyNo') || '');
            $('#biltyDate').val(dOnly(col(d, 'biltyDate')));
            setVal('#deliveryTermId', col(d, 'deliveryTermId'));
            $('#loadWeight').val(r3(col(d, 'loadWeight')));
            $('#tareWeight').val(r3(col(d, 'tareWeight')));
            $('#scaleNetWeight').val(r3(col(d, 'scaleNetWeight')));
            wbNetAccessible = String(r3(col(d, 'scaleNetWeight')));
            $('#biltyQty').val(r3(col(d, 'biltyQty')));
            vehicleQtyChanged();
            $('#remarksHeader').val(col(d, 'remarksHeader') || '');

            /* FillDetailtableFromListCommonForReadById */
            detailRows = (d.grnSupplierLoadingDetailList || []).map(function (x) { return detailFromDb(x); });
            removedRows = [];
            renderDetailGrid();
            ebRows = (d.grnSupplierLoadingEmptyBagDetailList || []).map(function (x) {
                return { Id: toInt(col(x, 'grnSupplierLoadingEmptyBagDetailId')), PoId: toInt(col(x, 'purchaseOrderMasterId')),
                         PoEbId: toInt(col(x, 'purchaseOrderEmptyBagDetailId')), PurchaseOrderNo: toInt(col(x, 'PurchaseOrderNo')),
                         PackingType: toInt(col(x, 'PackingTypeId')), PurchaseRate: toNum(col(x, 'Rate')),
                         EmptyBagItem: toInt(col(x, 'emptyBagPackingMaterialItemId')), _pt: col(x, 'PackingType'), _item: col(x, 'EmptyBagItem') };
            });
            expRows = (d.grnSupplierLoadingExpenseDetailList || []).map(function (x) {
                return { Id: toInt(col(x, 'grnSupplierLoadingExpenseDetailId')), PoId: toInt(col(x, 'purchaseOrderMasterId')),
                         PoExpenseId: toInt(col(x, 'purchaseOrderExpenseDetailId')), PurchaseOrderNo: toInt(col(x, 'PurchaseOrderNo')),
                         ItemId: toInt(col(x, 'ItemId')), Qty: toNum(col(x, 'Qty')), Rate: toNum(col(x, 'rate')), Amount: toNum(col(x, 'amount')),
                         Remarks: col(x, 'remarks') || '', _item: col(x, 'OtherItemName') };
            });
            if (!ebRows.length) ebRows.push(newEbRow());
            if (!expRows.length) expRows.push(newExpRow());
            renderEbGrid(); renderExpGrid();
            loadPendingOrders();
            supplierEnabledState();
            $('#docDate').focus();
        }).fail(function () { msg('Record Not Found'); });
    }

    function detailFromDb(x) {
        return {
            grnSupplierLoadingDetailId: toInt(col(x, 'grnSupplierLoadingDetailId')),
            purchaseOrderMasterId: toInt(col(x, 'purchaseOrderMasterId')), purchaseOrderDetailId: toInt(col(x, 'purchaseOrderDetailId')),
            _poNo: toInt(col(x, 'PurchaseOrderNo')), _poStart: dOnly(col(x, 'PODeliveryStartDate')), _poExpiry: dOnly(col(x, 'POExpiryDate')),
            inventoryParentCategoryId: toInt(col(x, 'inventoryParentCategoryId')), _parentItem: col(x, 'InvParentCateDescription') || '',
            itemId: toInt(col(x, 'itemId')), _itemId: toInt(col(x, 'itemId')), _itemCode: col(x, 'ItemCode') || '', itemName: col(x, 'ItemName') || '',
            cropYearId: toInt(col(x, 'cropYearId')), cropYear: col(x, 'cropYear') || '',
            packingTypeId: toInt(col(x, 'packingTypeId')), _packingType: col(x, 'PackingType') || '',
            packUomId: toInt(col(x, 'packUomId')), _packUom: col(x, 'PackSize') || '', _packUomEquivalent: toNum(col(x, 'PEquivalent')),
            loadingQty: toNum(col(x, 'loadingQty')), wbGrossWeight: toNum(col(x, 'wbGrossWeight')), ebwPerUnit: toNum(col(x, 'ebwPerUnit')),
            ebwTotal: toNum(col(x, 'ebwTotal')), addLessWeight: toNum(col(x, 'addLessWeight')), netBillWeight: toNum(col(x, 'netBillWeight')),
            EBWeightDeductionTermId: toInt(col(x, 'EBWeightDeductionTermId')), _emptyBagTerm: col(x, 'EBWeightDeductionTerm') || '',
            purchaseOrderSaleOrderMappingId: toInt(col(x, 'purchaseOrderSaleOrderMappingId')), _mappingId: toInt(col(x, 'purchaseOrderSaleOrderMappingId')),
            saleOrderMasterId: toInt(col(x, 'saleOrderMasterId')), saleOrderDetailId: toInt(col(x, 'saleOrderDetailId')), _soNo: toInt(col(x, 'SaleOrderNo')),
            buyerId: toInt(col(x, 'buyerId')), _buyerName: col(x, 'BuyerName') || '',
            DeliverToPartyId: toInt(col(x, 'DeliverToPartyId')), _deliverTo: col(x, 'DeliverToPartyName') || '',
            DeliverToAddressId: toInt(col(x, 'DeliverToAddressId')), DeliverToAddress: col(x, 'DeliverToAddress') || '',
            remarks: col(x, 'remarks') || '', warningRemarks: col(x, 'warningRemarks') || ''
        };
    }

    /* ================================================================== history */
    /* HistoryComboDbCall (:627) + HistoryComboBind (:3454) */
    function loadHistoryCombos() {
        return getJSON(API + '/history-combos').done(function (d) {
            histCombo = d || [];
            var parents = [], seen = {}, most = histCombo.find(function (r) { return col(r, 'Activity') === 'GetMostUsedParentCategoryId'; });
            histCombo.forEach(function (r) {
                if (col(r, 'Activity') !== 'ParentCategories') return;
                var id = toInt(col(r, 'Id')); if (seen[id]) return; seen[id] = 1; parents.push(r);
            });
            var keepSel = $('#histParentIds').val() || [];
            $('#histParentIds').html(parents.map(function (r) {
                var id = toInt(col(r, 'Id'));
                var sel = keepSel.length ? keepSel.indexOf(String(id)) >= 0 : (most && toInt(col(most, 'Id')) === id);
                return '<option value="' + id + '"' + (sel ? ' selected' : '') + '>' + esc(col(r, 'ReferenceName')) + '</option>';
            }).join(''));
            bindHistoryCombosAgainstParent();
        });
    }
    /* BindDropdownsAgainstParentCategory (:3565) */
    function bindHistoryCombosAgainstParent() {
        var parents = ($('#histParentIds').val() || []).map(String);
        var buckets = { CommissionAgent: [], SupplierName: [], Item: [], DeliveryToParty: [], DeliverToAddress: [] }, seen = {};
        histCombo.forEach(function (r) {
            var a = col(r, 'Activity'); if (!buckets[a]) return;
            if (parents.length && parents.indexOf(String(toInt(col(r, 'ParentCategoryId')))) < 0) return;
            var k = a + '|' + toInt(col(r, 'Id')) + (a === 'DeliverToAddress' ? '|' + col(r, 'ReferenceName') : '');
            if (a !== 'Item' && seen[k]) return; seen[k] = 1;
            buckets[a].push(r);
        });
        var idF = function (r) { return toInt(col(r, 'Id')); }, nmF = function (r) { return col(r, 'ReferenceName') || ''; };
        setOptions('#histCommissionAgentId', buckets.CommissionAgent, idF, nmF);
        setOptions('#histSupplierId', buckets.SupplierName, idF, nmF);
        setOptions('#histItemId', buckets.Item, idF, nmF);
        setOptions('#histDeliveryToPartyId', buckets.DeliveryToParty, idF, nmF);
        setOptions('#histShipToAddress', buckets.DeliverToAddress, nmF, nmF, null, '');
    }

    /* HistoryFill (:3691) */
    function historyFill() {
        var q = { dateType: $('input[name=histDateType]:checked').val() || 'doc',
                  commissionAgentId: iOf('#histCommissionAgentId'), supplierId: iOf('#histSupplierId'),
                  deliveryToPartyId: iOf('#histDeliveryToPartyId'), shipToAddress: $('#histShipToAddress').val() || '' };
        if ($('#histFromChk').is(':checked')) q.fromDate = $('#histFromDate').val();
        if ($('#histToChk').is(':checked')) q.toDate = $('#histToDate').val();
        return getJSON(API + '/history', q).done(function (rows) {
            histRows = rows || [];
            var tb = $('#gridHistory tbody').empty();
            $('#gridHistoryDetail thead, #gridHistoryDetail tbody, #gridHistoryEb tbody, #gridHistoryExp tbody').empty();
            if (!histRows.length) { tb.append('<tr><td colspan="22" class="text-center text-muted">No records found.</td></tr>'); $('#historyTotals').empty(); return; }
            var tf = 0, to = 0, tt = 0;
            histRows.forEach(function (r) {
                var id = toInt(col(r, 'grnSupplierLoadingMasterId'));
                tf += toNum(col(r, 'biltyFreight')); to += toNum(col(r, 'otherAdLesCharges')); tt += toNum(col(r, 'totalFreight'));
                tb.append('<tr data-id="' + id + '"><td><button type="button" class="btn btn-sm btn-outline-primary py-0 h-edit">Edit</button></td>'
                    + '<td><span class="clickable-code h-code">' + toInt(col(r, 'docNo')) + '</span></td>'
                    + '<td>' + ddMMMyyyy(col(r, 'docDate')) + '</td><td>' + esc(col(r, 'CommissionAgentName')) + '</td><td>' + esc(col(r, 'SupplierName')) + '</td>'
                    + '<td>' + esc(col(r, 'LoadingCity')) + '</td><td>' + esc(col(r, 'UnloadingCity')) + '</td>'
                    + '<td class="num">' + f3(col(r, 'biltyFreight')) + '</td><td class="num">' + f3(col(r, 'otherAdLesCharges')) + '</td><td class="num">' + f3(col(r, 'totalFreight')) + '</td>'
                    + '<td>' + esc(col(r, 'VehicleType')) + '</td><td>' + esc(col(r, 'vehicleNo')) + '</td><td>' + ddMMMyyyy(col(r, 'biltyDate')) + '</td>'
                    + '<td>' + esc(col(r, 'biltyNo')) + '</td><td>' + esc(col(r, 'DeliveryTerm')) + '</td><td>' + esc(col(r, 'EntryUserName')) + '</td>'
                    + '<td>' + dateTime(col(r, 'entryDate')) + '</td><td>' + esc(col(r, 'ModifyUserName')) + '</td><td>' + dateTime(col(r, 'modifyDate')) + '</td>'
                    + '<td class="num">' + toInt(col(r, 'NoOfAttachments')) + '</td>'
                    + '<td><button type="button" class="btn btn-sm btn-outline-dark py-0 h-print">Print</button></td>'
                    + '<td><button type="button" class="btn btn-sm btn-outline-dark py-0 h-printc">Print Challan</button></td></tr>');
            });
            $('#historyTotals').html('<td colspan="7" class="text-end">Total (' + histRows.length + ')</td><td class="num">' + f3(tf) + '</td><td class="num">'
                + f3(to) + '</td><td class="num">' + f3(tt) + '</td><td colspan="12"></td>');
        });
    }

    /* GetDetailGrdByHeadId (:3957) */
    function getDetailGrdByHeadId(id) {
        getJSON(API + '/' + id).done(function (res) {
            var d = res && res.data;
            var rows = d ? (d.grnSupplierLoadingDetailList || []).map(detailFromDb) : [];
            var showPo = rows.some(function (r) { return r.purchaseOrderMasterId > 0; });
            var cols = (showPo ? ['PurchaseOrderNo', 'PODeliveryStartDate', 'POExpiryDate'] : []).concat(['ParentItem', 'ItemCode', 'ItemName', 'CropYear', 'PackingType', 'PackUom',
                'Qty', 'GrossWeight', 'EbUnit', 'EbTotal', 'AddLessWeight', 'NetBillWeight', 'EmptyBagTerm']).concat(showPo ? ['SaleOrderNo'] : [])
                .concat(['BuyerName', 'DeliveryToParty', 'ShipToAddress', 'Remarks', 'WarningRemarks']);
            $('#gridHistoryDetail thead').html('<tr>' + cols.map(function (c) { return '<th>' + c + '</th>'; }).join('') + '</tr>');
            $('#gridHistoryDetail tbody').html(rows.map(function (r) {
                var v = { PurchaseOrderNo: r._poNo, PODeliveryStartDate: ddMMMyy(r._poStart), POExpiryDate: ddMMMyy(r._poExpiry), ParentItem: r._parentItem,
                    ItemCode: r._itemCode, ItemName: r.itemName, CropYear: r.cropYear, PackingType: r._packingType, PackUom: r._packUom,
                    Qty: f3(r.loadingQty), GrossWeight: f3(r.wbGrossWeight), EbUnit: f3(r.ebwPerUnit), EbTotal: f3(r.ebwTotal), AddLessWeight: f3(r.addLessWeight),
                    NetBillWeight: f3(r.netBillWeight), EmptyBagTerm: r._emptyBagTerm, SaleOrderNo: r._soNo || '', BuyerName: r._buyerName,
                    DeliveryToParty: r._deliverTo, ShipToAddress: r.DeliverToAddress, Remarks: r.remarks, WarningRemarks: r.warningRemarks };
                return '<tr>' + cols.map(function (c) { return '<td>' + esc(v[c]) + '</td>'; }).join('') + '</tr>';
            }).join(''));
            $('#gridHistoryEb tbody').html((d ? d.grnSupplierLoadingEmptyBagDetailList || [] : []).map(function (x) {
                return '<tr><td>' + (toInt(col(x, 'purchaseOrderMasterId')) > 0 ? toInt(col(x, 'PurchaseOrderNo')) : '') + '</td><td>' + esc(col(x, 'PackingType')) + '</td><td class="num">'
                    + f3(col(x, 'Rate')) + '</td><td>' + esc(col(x, 'EmptyBagItem')) + '</td></tr>';
            }).join(''));
            $('#gridHistoryExp tbody').html((d ? d.grnSupplierLoadingExpenseDetailList || [] : []).map(function (x) {
                return '<tr><td>' + (toInt(col(x, 'purchaseOrderMasterId')) > 0 ? toInt(col(x, 'PurchaseOrderNo')) : '') + '</td><td>' + esc(col(x, 'OtherItemName')) + '</td><td class="num">'
                    + f3(col(x, 'Qty')) + '</td><td class="num">' + f3(col(x, 'rate')) + '</td><td class="num">' + f3(col(x, 'amount')) + '</td><td>' + esc(col(x, 'remarks')) + '</td></tr>';
            }).join(''));
        });
    }

    /* ================================================================== loader (frmLoadPurchaseOrderForGrnLoading) */
    function openLoader() {
        $('#loaderModal').addClass('show');
        var jobs = [];
        if (!loaderCombo.length) jobs.push(getJSON(API + '/loader-combos').done(function (d) { loaderCombo = d || []; loaderCombosFill(); }));
        jobs.push(loaderSearch());
        return $.when.apply($, jobs);
    }
    /* CombosFill (:193) */
    function loaderCombosFill() {
        var b = { CommissionAgent: [], SupplierName: [], Item: [], DeliveryToParty: [], DeliverToAddress: [] }, seen = {};
        loaderCombo.forEach(function (r) {
            var a = col(r, 'Activity'); if (!b[a]) return;
            var k = a + '|' + toInt(col(r, 'Id')) + (a === 'DeliverToAddress' ? '|' + col(r, 'ReferenceName') : '');
            if (seen[k]) return; seen[k] = 1; b[a].push(r);
        });
        var idF = function (r) { return toInt(col(r, 'Id')); }, nmF = function (r) { return col(r, 'ReferenceName') || ''; };
        setOptions('#ldCommissionAgentId', b.CommissionAgent, idF, nmF);
        setOptions('#ldSupplierId', b.SupplierName, idF, nmF);
        setOptions('#ldItemId', b.Item, idF, nmF);
        setOptions('#ldDeliveryToPartyId', b.DeliveryToParty, idF, nmF);
        setOptions('#ldShipToAddress', b.DeliverToAddress, nmF, nmF, null, '');
    }
    /* btngrnlod_Click -> PendingDataDbCall (:288) + GrdDataBind (:333) */
    function loaderSearch() {
        var q = { fromDate: $('#ldFromDate').val(), toDate: $('#ldToDate').val(), fromDocNo: iOf('#ldDocNoFrom'), toDocNo: iOf('#ldDocNoTo'),
                  commissionAgentId: iOf('#ldCommissionAgentId'), supplierId: iOf('#ldSupplierId'), itemId: iOf('#ldItemId'),
                  deliveryToPartyId: iOf('#ldDeliveryToPartyId'), shipToAddress: $('#ldShipToAddress').val() || '' };
        return getJSON(API + '/pending-purchase-orders', q).done(function (rows) {
            loaderRows = rows || [];
            var already = iOf('#detPoNo');
            var tb = $('#gridLoader tbody').empty();
            $('#ldChkAll').prop('checked', false);
            if (!loaderRows.length) { tb.append('<tr><td colspan="21" class="text-center text-muted">No pending orders.</td></tr>'); return; }
            /* GrdDataBind (:333) column order; grdSettings (:387) hides Id, DetailId, ItemId, DeliveryToPartyId */
            loaderRows.forEach(function (r, i) {
                var pid = toInt(col(r, 'purchaseOrderMasterId'));
                tb.append('<tr><td class="ld-cb"><input type="checkbox" class="ld-chk" data-i="' + i + '"' + (already > 0 && pid === already ? ' checked' : '') + '></td>'
                    + '<td>' + toInt(col(r, 'docNo')) + '</td><td>' + ddMMyyyy(col(r, 'docDate')) + '</td><td>' + esc(col(r, 'CommissionAgentName')) + '</td>'
                    + '<td>' + esc(col(r, 'SupplierName')) + '</td><td>' + esc(col(r, 'DeliveryToPartyName')) + '</td><td>' + esc(col(r, 'ShipToAddress')) + '</td>'
                    + '<td>' + esc(col(r, 'ItemName')) + '</td><td class="num">' + f3(col(r, 'itemQty')) + '</td><td class="num">' + f3(col(r, 'UsedQty')) + '</td>'
                    + '<td class="num">' + f3(col(r, 'BalanceQty')) + '</td><td class="num">' + f3(col(r, 'itemWeight')) + '</td><td class="num">' + f3(col(r, 'UsedWeight')) + '</td>'
                    + '<td class="num">' + f3(col(r, 'BalanceWeight')) + '</td><td class="num">' + f3(col(r, 'itemRate')) + '</td><td>' + esc(col(r, 'EntryUserName')) + '</td>'
                    + '<td>' + dateTime(col(r, 'EntryDate')) + '</td><td>' + dateTime(col(r, 'ModifyDate')) + '</td><td>' + esc(col(r, 'ModifyUserName')) + '</td>'
                    + '<td>' + esc(col(r, 'RemarksHeader')) + '</td><td class="num">' + toInt(col(r, 'NoOfAttachments')) + '</td></tr>');
            });
            loaderFilter();
        });
    }
    /* dd/MM/yyyy - the Janus grid's default for a DateTime column */
    function ddMMyyyy(v) { var s = dOnly(v); if (!s) return ''; var p = s.split('-'); return p[2] + '/' + p[1] + '/' + p[0]; }
    /* GridEX filter row: case-insensitive "contains" per column */
    function loaderFilter() {
        var f = $('#gridLoader .ld-flt').map(function () { return { c: toInt($(this).data('c')), v: ($(this).val() || '').toLowerCase() }; }).get()
            .filter(function (x) { return x.v; });
        $('#gridLoader tbody tr').each(function () {
            var tds = this.cells, ok = true;
            if (tds.length > 1) f.forEach(function (x) { if (ok && (tds[x.c] ? tds[x.c].textContent : '').toLowerCase().indexOf(x.v) < 0) ok = false; });
            this.style.display = ok ? '' : 'none';
        });
    }
    /* MakeShortCutKeys (:532) */
    function loaderShortcuts() {
        msg(['Ctrl+E  For Close', 'Ctrl+N  For New', 'Ctrl+R  For Refresh', 'Ctrl+L  To Press Load Button', 'Ctrl+S  For Search',
             'Ctrl+alt  To Show ShortCut Keys Form', "Ctrl+Space  When Focus On Any Grid To Call Function's On Button Or Link"].join('\n'));
    }
    /* btnReset_Click (:451): From = financial-year start on the desktop; the web has no year
       start on the page, so the dialog's open default (today - 7) is used. */
    function loaderReset() {
        $('#ldFromDate').focus();
        $('#ldDocNoFrom, #ldDocNoTo').val('');
        setVal('#ldSupplierId', 0);          /* only CmbSupplier is cleared on the desktop */
        return loaderSearch();               /* PendingDataDbCall + GrdDataBind */
    }
    /* btnLoadOnInvoice_Click (:481) + BtnLoadOrder_Click tail (:4287) */
    function loaderLoad() {
        var picked = $('#gridLoader .ld-chk:checked').map(function () { return loaderRows[toInt($(this).data('i'))]; }).get();
        if (!picked.length) { msg('Check the row first'); return; }
        var first = picked[0];
        var sid = toInt(col(first, 'SupplierId'));
        if (iOf('#supplierId') > 0 && iOf('#supplierId') !== sid) { msg('you can only load order of supplier:' + selText('#supplierId')); return; }
        $('#loaderModal').removeClass('show');
        var newId = toInt(col(first, 'purchaseOrderMasterId'));
        var apply = function () {
            if (iOf('#detPoNo') !== newId) { setVal('#detPoNo', newId); cmbPoNoDetailLeave(); }
            $('#detPoNo').focus();
        };
        /* the order may be newer than the form's cached pending list */
        if (!(poTables.pendingOrders || []).some(function (r) { return toInt(col(r, 'purchaseOrderMasterId')) === newId; })) loadPendingOrders().always(apply);
        else apply();
    }
}());
