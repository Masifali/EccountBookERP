/* 630 Lab Purchase Analysis Periodic Report - Architecture.WinApp.Lab.LabPurchaseAnalyticPeriodic (module 1011 Lab Report).
   "P:nnn" = LabPurchaseAnalyticPeriodic.cs, "C:nnn" = DynamicCards/LabPurchaseAnalyticCard.cs,
   "I:nnn" = LabPurchaseAnalyticPeriodicItemWise.cs (the dialog a card title opens),
   "W:nnn" = LabPurchaseAnalyticPeriodicPartyAndItemWiseWithoutCards.cs (the dialog an item card opens).
   API /api/lab/reports/purchase-periodic. Built on pr_common.js (PR). */
(function () {
    'use strict';
    var $ = PR.$, API = '/api/lab/reports/purchase-periodic';
    var st = { season: null }, seq = 0;

    function num(key, width, fmt, agg) { return { key: key, width: width, type: 'num', fmt: fmt, agg: agg }; }

    /** PurchaseAnalyticCard_Load C:91-117 (RequestFor not set): widths, "#,##0.###" / "#,##0.####", two header lines. */
    function cardColumns() {
        return [{ key: 'Description', width: 100 }, num('NoofVehicles', 65, 'n3'), num('TotalQty', 63, 'n3'), num('Moisture', 60, 'n4'),
            num('GreenGrain', 60, 'n4'), num('EmptyShell', 60, 'n4'), num('Damage', 60, 'n4'), num('Sherivalled', 75, 'n4'),
            num('Fungus', 55, 'n4'), num('AGL', 55, 'n4'), num('Broken', 55, 'n4')];
    }
    /** C:118-125 RequestFor "Product_Wise": SortNo hidden, Description a link, AvgRate 85 in DecimalRateFormate. */
    function itemCardColumns() {
        var cols = cardColumns();
        cols[0].link = true;
        cols.unshift({ key: 'SortNo', hidden: true });
        cols.push(num('AvgRate', 85, 'rate'));
        return cols;
    }
    /** GetData W:80-131: widths, grouped by Description, Sum / Average totals in "#,##0.###", AvgRate in DecimalRateFormate. */
    function partyColumns() {
        return [{ key: 'Description', width: 100 }, { key: 'Supplier', width: 200 }, num('NoofVehicles', 80, 'n3', 'sum'), num('TotalQty', 80, 'n3', 'sum'),
            num('Moisture', 60, 'n3', 'avg'), num('GreenGrain', 55, 'n3', 'avg'), num('EmptyShell', 55, 'n3', 'avg'), num('Damage', 60, 'n3', 'avg'),
            num('Sherivalled', 75, 'n3', 'avg'), num('Fungus', 55, 'n3', 'avg'), num('AGL', 55, 'n3', 'avg'), num('Broken', 55, 'n3', 'avg'),
            num('AvgRate', 110, 'rate')];
    }

    /** A LabPurchaseAnalyticCard in a FlowLayoutPanel: teal title label (click) over the grid. */
    function addCard(flow, caption, columns, rows, onTitle, onLink) {
        var id = 'labCardGrid' + (++seq);
        var card = document.createElement('div'); card.className = 'lab-card';
        card.innerHTML = '<button type="button" class="lab-card-title"></button><div class="lab-card-grid"><table class="pr-grid" id="' + id + '"></table></div>';
        card.querySelector('.lab-card-title').textContent = caption;
        flow.appendChild(card);
        if (rows.length) new PR.Grid(id).show(columns, rows, { onLink: onLink });          // C:93 only when the table has rows
        card.querySelector('.lab-card-title').addEventListener('click', onTitle);
    }
    function lastDialog() { var all = document.querySelectorAll('dialog.pr-dialog'); return all[all.length - 1]; }

    /** GetSeasonScheduleDates P:67-98. */
    function applySeason(s, startId, endId, showId) {
        st.season = s;
        if (s && s.found) {
            $(startId).value = s.seasonStartDate; $(endId).value = s.seasonEndDate;
            $(showId).disabled = false;
        } else {
            $(showId).disabled = true;
            PR.box((s && s.message) || 'No Schedule found. Please Make a season year schedule first!');
        }
    }
    /** btnSeasonYearSchedule_Click P:198-206 - Lookups.SeasonYearSchedule (screen 436) is not ported; the dates are re-read as after the dialog. */
    function seasonYearSchedule(startId, endId, showId, button) {
        PR.notPorted('Architecture.WinApp.Lookups.SeasonYearSchedule', 'Define Season Year Schedule');
        return LabRep.run(button, function () { return PR.request(API + '/season').then(function (s) { applySeason(s, startId, endId, showId); }); });
    }

    /** btnshow_Click P:106-165. */
    function btnshow_Click() {
        if (!st.season || !st.season.found) return;                                       // P:112
        var flow = $('flowLayoutPanel1');
        return LabRep.run('btnshow', function () {
            flow.innerHTML = '';                                                          // flowLayoutPanel1.Controls.Clear()
            return PR.request(API + '/cards', { fromDate: $('DateFrom').value, toDate: $('DateTo').value }).then(function (res) {
                (res.cards || []).forEach(function (c) {
                    // UserControl_Click P:167-196: the card grid has no link column here, so only the title click acts.
                    addCard(flow, c.caption, cardColumns(), c.rows || [], function () { itemWise(c.id); }, null);
                });
            });
        });
    }

    /**
     * LabPurchaseAnalyticPeriodicItemWise opened by a card title (P:186-195): RequestedByOtherDocument, FromDate, ToDate,
     * ParentCategoryId. Its Load (I:53-70) copies the dates and runs Show.
     */
    function itemWise(parentCategoryId) {
        var p = 'labItemWise' + (++seq) + '_';
        var html = '<nav class="pr-toolstrip"><button type="button" id="' + p + 'new" title="The desktop button has no Click handler">New</button>'
            + '<button type="button" id="' + p + 'schedule">Define Season Year Schedule</button></nav>'
            + '<div style="height:75px;position:relative;background:#fff"><fieldset class="pr-filters lab-filters pr-abs" style="left:6px;top:3px;width:572px;height:70px;margin:0;background:#fff">'
            + '<legend>Filters</legend>'
            + '<label class="pr-abs" style="left:10px;top:20px" for="' + p + 'from">From Date</label><div class="pr-abs" style="left:10px;top:40px;width:121px"><input type="date" id="' + p + 'from"></div>'
            + '<label class="pr-abs" style="left:133px;top:20px" for="' + p + 'to">To Date</label><div class="pr-abs" style="left:133px;top:40px;width:119px"><input type="date" id="' + p + 'to"></div>'
            + '<label class="pr-abs" style="left:253px;top:20px" for="' + p + 'start">Season Start Date</label><div class="pr-abs" style="left:253px;top:40px;width:126px"><input type="date" id="' + p + 'start" disabled></div>'
            + '<label class="pr-abs" style="left:381px;top:20px" for="' + p + 'end">Season End Date</label><div class="pr-abs" style="left:381px;top:40px;width:125px"><input type="date" id="' + p + 'end" disabled></div>'
            + '<button type="button" class="lab-show pr-abs" id="' + p + 'show" style="left:508px;top:38px;width:56px;height:26px">Show</button>'
            + '</fieldset></div><div class="lab-flow" id="' + p + 'flow"></div>';
        PR.dialog('Purchase Analytic Periodic Item Wise Report', html);
        lastDialog().classList.add('lab-dialog');
        $(p + 'from').value = $('DateFrom').value; $(p + 'to').value = $('DateTo').value;  // I:63-64
        $(p + 'start').value = st.season.seasonStartDate; $(p + 'end').value = st.season.seasonEndDate;
        function show() {                                                                 // btnshow_Click I:113-178
            if (!st.season || !st.season.found) return;
            var flow = $(p + 'flow');
            return LabRep.run(p + 'show', function () {
                flow.innerHTML = '';
                return PR.request(API + '/item-wise', { fromDate: $(p + 'from').value, toDate: $(p + 'to').value, parentCategoryId: parentCategoryId }).then(function (res) {
                    (res.cards || []).forEach(function (c) {
                        var rows = c.rows || [];
                        addCard(flow, c.caption, itemCardColumns(), rows,
                            function () { if (rows.length) partyWise(p, c.itemId, 0); },              // title: I:196-211 (no SortNo)
                            function (col, r) { if (PR.num(r.TotalQty) > 0) partyWise(p, c.itemId, PR.int(r.SortNo)); });   // link: I:180-194
                    });
                });
            });
        }
        $(p + 'show').addEventListener('click', show);
        $(p + 'schedule').addEventListener('click', function () { seasonYearSchedule(p + 'start', p + 'end', p + 'show', p + 'schedule'); });
        show();                                                                           // I:65 btnshow_Click(null, null)
    }

    /** LabPurchaseAnalyticPeriodicPartyAndItemWiseWithoutCards: FromDate, ToDate, season dates, ItemId, SortNo (I:185-192 / I:201-207). */
    function partyWise(p, itemId, sortNo) {
        var id = 'labParty' + (++seq);
        PR.dialog('Lab Purchase Analytic Periodic Product And Party Wise Report',
            '<div class="pr-wrap lab-party-wrap"><table class="pr-grid lab-two-line" id="' + id + '"></table></div>');
        var d = lastDialog(); d.classList.add('lab-dialog');
        var sub = document.createElement('span'); sub.className = 'lab-sub'; d.querySelector('h2').appendChild(sub);   // lblItemName
        PR.run(function () {
            return PR.request(API + '/party-wise', { fromDate: $(p + 'from').value, toDate: $(p + 'to').value, itemId: itemId, sortNo: sortNo }).then(function (res) {
                if (!(res.rows || []).length) return;                                     // W:59-62
                sub.textContent = res.itemName;                                           // W:77
                new PR.Grid(id).show(partyColumns(), res.rows, { groupBy: 'Description' });   // Groups.Add("Description") W:96
            });
        });
    }

    /** PurchaseAnalyticPeriodic_Load P:53-65. */
    function load() {
        return PR.request(API + '/init').then(function (d) {
            PR.cfg.rateDecimals = d.rateDecimals;
            $('DateFrom').value = d.fromDate;                                             // DateTime.Now.AddDays(-30) P:60
            $('DateTo').value = d.toDate;
            applySeason(d.season, 'datSeasonStartDate', 'datSeasonEndDate', 'btnshow');
        });
    }

    $('btnshow').addEventListener('click', btnshow_Click);
    $('btnSeasonYearSchedule').addEventListener('click', function () { seasonYearSchedule('datSeasonStartDate', 'datSeasonEndDate', 'btnshow', 'btnSeasonYearSchedule'); });
    PR.run(load);
})();
