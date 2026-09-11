/**
 * countx_coa_definition.js
 * Interactive Client-Side Logic for Chart of Account Definition (Ditto Copy of C# Desktop ERP AcfrmDefCoa.cs)
 */
$(document).ready(function () {
    let currentAccountId = null;
    let currentSelectedParentCode = "0";
    let selectedLevel1 = "";
    let selectedLevel2 = "";
    let selectedLevel3 = "";
    let collapsedNodes = new Set(); // Tracks collapsed parent account codes in History Tree
    let allCustomerGroups = []; // Ditto cmbsupgroupsfill's full CustomerGroup.GetAll() cache

    // Grid Paging State (Janus GridEX replica)
    const gridState = {
        level1: { page: 1, pageSize: 5, data: [] },
        level2: { page: 1, pageSize: 5, data: [] },
        level3: { page: 1, pageSize: 5, data: [] },
        level4: { page: 1, pageSize: 5, data: [] },
        alloc: { page: 1, pageSize: 5, data: [] },
        child: { page: 1, pageSize: 10, data: [] },
        history: { page: 1, pageSize: 15, data: [] }
    };

    const accountClassMap = {
        1: "Capital",
        2: "Assets",
        3: "Liabilities",
        4: "Expenses",
        5: "Revenue"
    };

    // Hotkey Shortcuts (Ctrl+N, Ctrl+R, Ctrl+S, Ctrl+P)
    $(document).on("keydown", function (e) {
        if (e.ctrlKey && (e.key === "n" || e.key === "N")) {
            e.preventDefault();
            resetForm();
        } else if (e.ctrlKey && (e.key === "r" || e.key === "R")) {
            e.preventDefault();
            refreshScreen();
        } else if (e.ctrlKey && (e.key === "s" || e.key === "S")) {
            e.preventDefault();
            $("#coaForm").submit();
        } else if (e.ctrlKey && (e.key === "p" || e.key === "P")) {
            e.preventDefault();
            printReport();
        }
    });

    // Bottom Tab Strip Click (Form / History)
    $(".erp-tab-button").on("click", function () {
        $(".erp-tab-button").removeClass("active");
        $(this).addClass("active");
        const target = $(this).data("target");
        $(".tab-content .tab-pane").removeClass("active");
        $(target).addClass("active");
        if (target === "#tabHistory") {
            loadHistoryGrid();
        }
    });

    // 1. Initialize Screen
    initScreen();

    function initScreen() {
        loadParentDropdown();
        loadAccountTypesFilter();
        loadCities();
        loadCustomerGroups();
        loadCustomGroups();
        loadLocations();
        loadLevel1Grid();
        loadHistoryGrid();
        setupAutocompleteLookups();
        bindFormEvents();
    }

    function loadAccountTypesFilter() {
        // Ditto AcfrmDefCoa::CmbAccountType, which binds cmbactype AND CmbAccountTypeFilter
        // from the same dbo.AccountTypes list (key=Id, value=AccountType, e.g. "Cash Equivalent").
        $.get("/api/accounts/account-types", function (types) {
            let $select = $("#filterAccountType");
            $select.empty();
            $select.append('<option value="">All Types</option>');
            $.each(types || [], function (i, t) {
                $select.append(`<option value="${t.id}">${t.accountType}</option>`);
            });
        });
    }

    // 2. Parent Account Dropdown & Hierarchy Logic (ditto cmbparentac_Leave)
    function loadParentDropdown() {
        $.get("/api/accounts/parents", function (data) {
            let $select = $("#parentAccountCode");
            $select.empty();
            $select.append('<option value="0">0 - (Top Level / Root)</option>');
            $.each(data, function (i, item) {
                $select.append(`<option value="${item.accountCode}">${item.accountCode} - ${item.accountTitle}</option>`);
            });
        });
    }

    $("#parentAccountCode").on("change", function () {
        const parentCode = $(this).val();
        onParentSelected(parentCode);
    });

    function onParentSelected(parentCode) {
        if (!parentCode) parentCode = "0";
        currentSelectedParentCode = parentCode;

        $.get("/api/accounts/next-code", { parentCode: parentCode }, function (next) {
            $("#accountCode").val(next.accountCode || "");
            const lvl = next.accountLevel || 1;
            $("#accountLevel").val(lvl);

            const classId = next.accountClass;
            $("#accountClass").val(classId || "");
            $("#accountClassText").val(accountClassMap[classId] || "(Unset)");

            if (next.bsNoteId) $("#bsNoteId").val(next.bsNoteId);
            if (next.plNoteId) $("#plNoteId").val(next.plNoteId);
            if (next.customerGroupId) $("#customerGroupId").val(next.customerGroupId);

            const isDetail = lvl >= 4;
            $("#accountGroup").val(isDetail ? "Detail" : "Group");

            // Ditto cmbparentac_Leave: a new Detail (level>=4) account inherits & displays its
            // parent's Account Type (read-only, see updateFieldStates below); at level 3 the
            // user assigns the type themselves so it's left blank; below level 3 it's n/a.
            $("#accountTypeId").val(isDetail && next.accountTypeId != null ? next.accountTypeId : "");

            updateFieldStates(lvl, classId);
            loadChildGrid(parentCode);
        });
    }

    function updateFieldStates(level, accountClass) {
        const isDetail = level >= 4;

        if (level === 3) {
            $("#accountTypeId").prop("disabled", false);
            if (accountClass == 4 || accountClass == 5) {
                $("#plNoteId").prop("disabled", false);
                $("#bsNoteId").prop("disabled", true).val("");
            } else if (accountClass == 1 || accountClass == 2 || accountClass == 3) {
                $("#bsNoteId").prop("disabled", false);
                $("#plNoteId").prop("disabled", true).val("");
            } else {
                $("#plNoteId").prop("disabled", false);
                $("#bsNoteId").prop("disabled", false);
            }
        } else {
            $("#accountTypeId").prop("disabled", true);
            if (!isDetail) {
                $("#accountTypeId").val("");
            }
            $("#plNoteId").prop("disabled", true).val("");
            $("#bsNoteId").prop("disabled", true).val("");
        }

        $("#cityId").prop("disabled", !isDetail);
        $("#contactNo").prop("disabled", !isDetail);
        $("#openingBalance").prop("disabled", !isDetail);

        // Ditto cmbactype_Leave (AcfrmDefCoa.cs:1311-1326), which cmbparentac_Leave ALWAYS
        // calls last, AFTER the level-based rule above (AcfrmDefCoa.cs:1157-1187) - so the
        // real read-only/cleared state of Sup/Cust Group is driven by the CURRENT Account
        // Type value, not level alone, and OVERRIDES whatever the level rule just set, at
        // every level including Detail/level 4 (a non-3/22 inherited type still clears it).
        applyCustomerGroupRule();
    }

    // Ditto cmbactype_Leave (AcfrmDefCoa.cs:1308-1326): only Account Type 3 or 22 leaves
    // Sup/Cust Group editable; every other type clears its value and disables it, with no
    // level condition of its own (the level dependency lives entirely in cmbparentac_Leave's
    // earlier, overridden rule above).
    function applyCustomerGroupRule() {
        const typeVal = parseInt($("#accountTypeId").val());
        renderCustomerGroupOptions();
        if (typeVal === 3 || typeVal === 22) {
            $("#customerGroupId").prop("disabled", false);
        } else {
            $("#customerGroupId").prop("disabled", true).val("");
        }
    }

    $("#accountTypeId").on("change", function () {
        applyCustomerGroupRule();
    });


    // 3. Autocomplete Search Lookups (Ditto C# Desktop CmbAccountTitle)
    let cachedLevel4Accounts = null;

    function setupAutocompleteLookups() {
        let debounceTimer;

        // Filter Accounts In Child Grid -> Account Title (Ditto CmbAccountTitle / AccountTitleFill)
        $("#filterAccountTitle").on("focus click input", function () {
            const query = $(this).val() ? $(this).val().toLowerCase().trim() : "";
            clearTimeout(debounceTimer);
            debounceTimer = setTimeout(() => {
                showLevel4AccountPopup(query);
            }, 100);
        });

        // Account Information -> Account Title input autocomplete
        $("#accountTitleInput").on("input", function () {
            const query = $(this).val();
            if (query.length < 2) {
                $("#accountTitlePopup").hide();
                return;
            }
            clearTimeout(debounceTimer);
            debounceTimer = setTimeout(() => {
                showGeneralAccountPopup(query, "#accountTitleInput", "#accountTitlePopup", function (acc) {
                    $("#accountTitleInput").val(acc.accountTitle);
                });
            }, 250);
        });

        // Dropdown Arrow Button Clicks (UltraCombo replication)
        $("#btnFilterAccountTitleArrow").on("click", function (e) {
            e.stopPropagation();
            const $input = $("#filterAccountTitle");
            $input.focus();
            const query = $input.val() ? $input.val().toLowerCase().trim() : "";
            showLevel4AccountPopup(query);
        });

        $("#btnAccountTitleInputArrow").on("click", function (e) {
            e.stopPropagation();
            const $input = $("#accountTitleInput");
            $input.focus();
            const query = $input.val();
            showGeneralAccountPopup(query && query.length >= 1 ? query : "", "#accountTitleInput", "#accountTitlePopup", function (acc) {
                $("#accountTitleInput").val(acc.accountTitle);
            });
        });

        $(document).on("click", function (e) {
            if (!$(e.target).closest(".autocomplete-container").length) {
                $(".autocomplete-popup").hide();
            }
        });
    }

    function showLevel4AccountPopup(query) {
        if (!cachedLevel4Accounts) {
            $.get("/api/accounts/level4-accounts", function (data) {
                cachedLevel4Accounts = data || [];
                renderLevel4PopupRows(cachedLevel4Accounts, query);
            });
        } else {
            renderLevel4PopupRows(cachedLevel4Accounts, query);
        }
    }

    function renderLevel4PopupRows(allAccounts, query) {
        let $popup = $("#filterTitlePopup");
        $popup.empty();

        let filtered = allAccounts;
        if (query && query.length > 0) {
            filtered = allAccounts.filter(acc =>
                (acc.accountTitle && acc.accountTitle.toLowerCase().includes(query)) ||
                (acc.accountCode && acc.accountCode.toLowerCase().includes(query))
            );
        }

        if (!filtered || filtered.length === 0) {
            $popup.append('<div style="padding:6px; color:#94a3b8; font-style:italic;">No 4th level accounts found</div>');
            $popup.show();
            return;
        }

        let html = `<table class="erp-table autocomplete-table" style="width:100%;">
            <thead>
                <tr>
                    <th style="width:40%;">AccountTitle</th>
                    <th style="width:20%;">AccountCode</th>
                    <th style="width:20%;">AccountClass</th>
                    <th style="width:20%;">AccountType</th>
                </tr>
            </thead>
            <tbody>`;

        $.each(filtered, function (i, acc) {
            html += `<tr data-id="${acc.id}" data-code="${acc.accountCode}" class="autocomplete-row">
                <td style="font-weight:bold; color:#00796B;">${acc.accountTitle || ''}</td>
                <td>${acc.accountCode || ''}</td>
                <td>${acc.accountClass || ''}</td>
                <td>${acc.accountType || ''}</td>
            </tr>`;
        });

        html += `</tbody></table>`;
        $popup.html(html).show();

        $popup.find(".autocomplete-row").on("click", function () {
            const accId = $(this).data("id");
            const found = allAccounts.find(x => x.id == accId);
            if (found) {
                $("#filterAccountTitle").val(found.accountTitle);
                // Ditto CmbAccountTitle_Leave: fetch parent code by account id, select parent, trigger parent Leave
                $.get(`/api/accounts/parent-code-by-account-id/${found.id}`, function (res) {
                    const parentCode = res.parentAccountCode || found.parentAccountCode || "0";
                    $("#parentAccountCode").val(parentCode);
                    onParentSelected(parentCode);
                    loadCascadeLevels(parentCode);
                    $("#accountTitleInput").focus();
                }).fail(function () {
                    const parentCode = found.parentAccountCode || "0";
                    $("#parentAccountCode").val(parentCode);
                    onParentSelected(parentCode);
                    loadCascadeLevels(parentCode);
                    $("#accountTitleInput").focus();
                });
            }
            $popup.hide();
        });
    }

    function showGeneralAccountPopup(query, inputSelector, popupSelector, onSelect) {
        if (!query || query.length < 1) {
            $(popupSelector).hide();
            return;
        }
        $.get("/api/accounts/search", { query: query }, function (data) {
            let $popup = $(popupSelector);
            $popup.empty();

            if (!data || data.length === 0) {
                $popup.append('<div style="padding:6px; color:#94a3b8; font-style:italic;">No matching accounts found</div>');
                $popup.show();
                return;
            }

            let html = `<table class="erp-table autocomplete-table">
                <thead>
                    <tr>
                        <th>Account Title</th>
                        <th>Account Code</th>
                        <th>Class</th>
                    </tr>
                </thead>
                <tbody>`;

            $.each(data, function (i, acc) {
                html += `<tr data-code="${acc.accountCode}" class="autocomplete-row">
                    <td style="font-weight:bold; color:#00796B;">${acc.accountTitle}</td>
                    <td>${acc.accountCode}</td>
                    <td>${acc.accountClass || ''}</td>
                </tr>`;
            });

            html += `</tbody></table>`;
            $popup.html(html).show();

            $popup.find(".autocomplete-row").on("click", function () {
                const code = $(this).data("code");
                const found = data.find(x => x.accountCode == code);
                if (found) {
                    onSelect(found);
                }
                $popup.hide();
            });
        });
    }

    // 4. Cascading Level Tree Grids (Level 1 -> 2 -> 3 -> 4)
    function loadLevel1Grid() {
        loadCascadeLevels("0");
    }

    function loadCascadeLevels(parentCode) {
        if (!parentCode) parentCode = "0";
        selectedLevel1 = parentCode;
        $.get("/api/accounts/cascade-levels", { parentCode: parentCode }, function (res) {
            if (res.level1 && res.level1.length > 0) {
                gridState.level1.data = res.level1;
                gridState.level1.page = 1;
                renderLevelGrid(1, "#grdLevel1", "#pagingLevel1");
                const firstCode = res.level1[0].accountCode;
                $("#grdLevel1 tr.level-row").removeClass("selected-row");
                $(`#grdLevel1 tr.level-row[data-code='${firstCode}']`).addClass("selected-row");
            }

            gridState.level2.data = res.level2 || [];
            gridState.level2.page = 1;
            renderLevelGrid(2, "#grdLevel2", "#pagingLevel2");
            if (res.level2 && res.level2.length > 0) {
                const firstRowCode = res.level2[0].accountCode;
                $("#grdLevel2 tr.level-row").removeClass("selected-row");
                $(`#grdLevel2 tr.level-row[data-code='${firstRowCode}']`).addClass("selected-row");
            }

            gridState.level3.data = res.level3 || [];
            gridState.level3.page = 1;
            renderLevelGrid(3, "#grdLevel3", "#pagingLevel3");
            if (res.level3 && res.level3.length > 0) {
                const firstRowCode = res.level3[0].accountCode;
                $("#grdLevel3 tr.level-row").removeClass("selected-row");
                $(`#grdLevel3 tr.level-row[data-code='${firstRowCode}']`).addClass("selected-row");
            }

            gridState.level4.data = res.level4 || [];
            gridState.level4.page = 1;
            renderLevelGrid(4, "#grdLevel4", "#pagingLevel4");
            if (res.level4 && res.level4.length > 0) {
                const firstRowCode = res.level4[0].accountCode;
                $("#grdLevel4 tr.level-row").removeClass("selected-row");
                $(`#grdLevel4 tr.level-row[data-code='${firstRowCode}']`).addClass("selected-row");
            }
        }).fail(function () {
            readPreRenderedLevelRows(1, "#grdLevel1", "#pagingLevel1");
        });
    }

    function readPreRenderedLevelRows(levelNum, tableSelector, pagingSelector) {
        const rows = [];
        $(tableSelector).find("tbody tr.level-row").each(function () {
            const code = $(this).data("code");
            const id = $(this).data("id");
            const title = $(this).find("td:last").text().trim();
            if (code) {
                rows.push({ id: id, accountCode: code, accountTitle: title });
            }
        });
        if (rows.length > 0) {
            gridState["level" + levelNum].data = rows;
            gridState["level" + levelNum].page = 1;
            renderLevelGrid(levelNum, tableSelector, pagingSelector);
        }
    }

    function loadLevel2Grid(parentCode, autoCascade = true) {
        selectedLevel1 = parentCode;
        $.get("/api/accounts/level", { parentCode: parentCode }, function (data) {
            gridState.level2.data = data || [];
            gridState.level2.page = 1;
            renderLevelGrid(2, "#grdLevel2", "#pagingLevel2");
            if (autoCascade && data && data.length > 0) {
                const firstRowCode = data[0].accountCode;
                $("#grdLevel2 tr.level-row").removeClass("selected-row");
                $(`#grdLevel2 tr.level-row[data-code='${firstRowCode}']`).addClass("selected-row");
                loadLevel3Grid(firstRowCode, true);
            } else {
                clearLevelGrid(3);
                clearLevelGrid(4);
            }
        });
    }

    function loadLevel3Grid(parentCode, autoCascade = true) {
        selectedLevel2 = parentCode;
        $.get("/api/accounts/level", { parentCode: parentCode }, function (data) {
            gridState.level3.data = data || [];
            gridState.level3.page = 1;
            renderLevelGrid(3, "#grdLevel3", "#pagingLevel3");
            if (autoCascade && data && data.length > 0) {
                const firstRowCode = data[0].accountCode;
                $("#grdLevel3 tr.level-row").removeClass("selected-row");
                $(`#grdLevel3 tr.level-row[data-code='${firstRowCode}']`).addClass("selected-row");
                loadLevel4Grid(firstRowCode, true);
            } else {
                clearLevelGrid(4);
            }
        });
    }

    function loadLevel4Grid(parentCode, autoCascade = true) {
        selectedLevel3 = parentCode;
        $.get("/api/accounts/level", { parentCode: parentCode }, function (data) {
            gridState.level4.data = data || [];
            gridState.level4.page = 1;
            renderLevelGrid(4, "#grdLevel4", "#pagingLevel4");
            if (autoCascade && data && data.length > 0) {
                const firstRowCode = data[0].accountCode;
                $("#grdLevel4 tr.level-row").removeClass("selected-row");
                $(`#grdLevel4 tr.level-row[data-code='${firstRowCode}']`).addClass("selected-row");
            }
        });
    }

    function clearLevelGrid(levelNum) {
        gridState["level" + levelNum].data = [];
        gridState["level" + levelNum].page = 1;
        renderLevelGrid(levelNum, "#grdLevel" + levelNum, "#pagingLevel" + levelNum);
    }

    function renderLevelGrid(levelNum, tableSelector, pagingSelector) {
        const state = gridState["level" + levelNum];
        let $tbody = $(tableSelector).find("tbody");
        $tbody.empty();

        const total = state.data.length;
        const totalPages = Math.ceil(total / state.pageSize) || 1;
        if (state.page > totalPages) state.page = totalPages;

        const start = (state.page - 1) * state.pageSize;
        const pageData = state.data.slice(start, start + state.pageSize);

        if (pageData.length === 0) {
            $tbody.append(`<tr><td colspan="2" style="text-align:center; color:#94a3b8; padding:8px;">Select parent account</td></tr>`);
        } else {
            $.each(pageData, function (i, acc) {
                $tbody.append(`<tr class="level-row" data-level="${levelNum}" data-code="${acc.accountCode}" data-id="${acc.id}">
                    <td style="font-weight:bold; color:#00796B; width:30%;">${acc.accountCode}</td>
                    <td>${acc.accountTitle}</td>
                </tr>`);
            });
        }

        renderPagingBar(pagingSelector, state.page, totalPages, total, function (newPage) {
            state.page = newPage;
            renderLevelGrid(levelNum, tableSelector, pagingSelector);
        });
    }

    // Global Event Handlers for Level Grids (Row click & Selection)
    $(document).on("click", "#grdLevel1 tr.level-row", function () {
        const code = String($(this).data("code") || "").trim();
        if (code) {
            $("#grdLevel1 tr.level-row").removeClass("selected-row");
            $(this).addClass("selected-row");
            $("#parentAccountCode").val(code);
            onParentSelected(code);
            loadCascadeLevels(code);
        }
    });

    $(document).on("click", "#grdLevel2 tr.level-row", function () {
        const code = String($(this).data("code") || "").trim();
        if (code) {
            $("#grdLevel2 tr.level-row").removeClass("selected-row");
            $(this).addClass("selected-row");
            $("#parentAccountCode").val(code);
            onParentSelected(code);
            loadCascadeLevels(code);
        }
    });

    $(document).on("click", "#grdLevel3 tr.level-row", function () {
        const code = String($(this).data("code") || "").trim();
        if (code) {
            $("#grdLevel3 tr.level-row").removeClass("selected-row");
            $(this).addClass("selected-row");
            $("#parentAccountCode").val(code);
            onParentSelected(code);
            loadCascadeLevels(code);
        }
    });

    $(document).on("click", "#grdLevel4 tr.level-row", function () {
        const code = String($(this).data("code") || "").trim();
        if (code) {
            $("#grdLevel4 tr.level-row").removeClass("selected-row");
            $(this).addClass("selected-row");
            $("#parentAccountCode").val(code);
            onParentSelected(code);
        }
    });

    // 5. Child Of Selected Parent Grid (Req #8-#13)
    function loadChildGrid(parentCode) {
        currentSelectedParentCode = parentCode || "0";
        $.get("/api/accounts/level", { parentCode: currentSelectedParentCode }, function (data) {
            gridState.child.rawData = data || [];
            applyChildGridFilters();
        });
    }

    function loadChildGridBySearch(accountCode) {
        $.get("/api/accounts/search", { query: accountCode }, function (data) {
            gridState.child.rawData = data || [];
            applyChildGridFilters();
        });
    }

    function applyChildGridFilters() {
        // Ditto: neither filter locally narrows this grid on the real desktop - Account Type
        // (CmbAccountTypeFilter_Leave) replaces it outright via Get3rdLevelGroupAccounts, and
        // Account Title (CmbAccountTitle_Leave) jumps the Parent Account selector instead (see
        // their own handlers). This just (re)renders whatever was last loaded into it.
        gridState.child.data = gridState.child.rawData || [];
        gridState.child.page = 1;
        renderChildGrid();
    }

    $("#filterAccountType").on("change", function () {
        const typeId = $(this).val();
        if (!typeId) {
            // "All Types" - ditto: revert to the normal children-of-selected-parent list.
            loadChildGrid(currentSelectedParentCode);
            return;
        }
        $.get(`/api/accounts/third-level-by-type/${typeId}`, function (data) {
            gridState.child.rawData = data || [];
            gridState.child.data = gridState.child.rawData;
            gridState.child.page = 1;
            renderChildGrid();
        });
    });

    function renderChildGrid() {
        const state = gridState.child;
        let $tbody = $("#grdChildAccounts").find("tbody");
        $tbody.empty();

        const total = state.data.length;
        const totalPages = Math.ceil(total / state.pageSize) || 1;
        const start = (state.page - 1) * state.pageSize;
        const pageData = state.data.slice(start, start + state.pageSize);

        if (pageData.length === 0) {
            $tbody.append(`<tr><td colspan="6" style="text-align:center; color:#94a3b8; padding:10px;">No child accounts found under parent account (${currentSelectedParentCode}).</td></tr>`);
        } else {
            $.each(pageData, function (i, acc) {
                $tbody.append(`<tr class="child-grid-row" data-id="${acc.id}" style="cursor:pointer;">
                    <td style="font-weight:bold; color:#00796B;">${acc.accountCode}</td>
                    <td>${acc.accountTitle}</td>
                    <td>${acc.accountGroup || 'Group'}</td>
                    <td>${acc.accountTypeId || ''}</td>
                    <td>${acc.otherErpCode || ''}</td>
                    <td style="text-align:center;">
                        <button type="button" class="btn-erp btn-edit-account" data-id="${acc.id}" style="padding:1px 6px; color:#00796B;"><i class="fa fa-pencil"></i> Edit</button>
                    </td>
                </tr>`);
            });
        }

        renderPagingBar("#pagingChild", state.page, totalPages, total, function (newPage) {
            state.page = newPage;
            renderChildGrid();
        });

        // Row Click & Edit Button Click (Req #14 & #22)
        $("#grdChildAccounts tbody tr.child-grid-row").on("click", function (e) {
            const id = $(this).data("id");
            if (id) {
                $("#grdChildAccounts tbody tr.child-grid-row").removeClass("selected-row");
                $(this).addClass("selected-row");
                loadAccountForEdit(id);
            }
        });
    }

    // 6. Location Allocation Grid
    function loadLocations(accountId) {
        const url = accountId ? `/api/accounts/${accountId}/allocations` : "/api/accounts/locations";
        $.get("/api/accounts/search", { query: "" }, function () {
            $.get(url, function (data) {
                gridState.alloc.data = data || [];
                gridState.alloc.page = 1;
                renderAllocGrid();
            }).fail(function () {
                gridState.alloc.data = [{ companyId: 1, companyName: "GOLDEN ACE FOODS", allocated: true }];
                renderAllocGrid();
            });
        });
    }

    function renderAllocGrid() {
        const state = gridState.alloc;
        let $tbody = $("#grdAllocation").find("tbody");
        $tbody.empty();

        const total = state.data.length;
        const totalPages = Math.ceil(total / state.pageSize) || 1;
        const start = (state.page - 1) * state.pageSize;
        const pageData = state.data.slice(start, start + state.pageSize);

        if (pageData.length === 0) {
            $tbody.append(`<tr><td colspan="2" style="text-align:center; color:#94a3b8; padding:6px;">No locations</td></tr>`);
        } else {
            $.each(pageData, function (i, loc) {
                const checked = loc.allocated ? "checked" : "";
                $tbody.append(`<tr>
                    <td>${loc.companyName}</td>
                    <td style="text-align:center;"><input type="checkbox" class="alloc-chk" data-cid="${loc.companyId}" ${checked}/></td>
                </tr>`);
            });
        }

        renderPagingBar("#pagingAlloc", state.page, totalPages, total, function (newPage) {
            state.page = newPage;
            renderAllocGrid();
        });

        $(".alloc-chk").on("change", function () {
            const cid = $(this).data("cid");
            const isChecked = $(this).is(":checked");
            const found = gridState.alloc.data.find(x => x.companyId == cid);
            if (found) found.allocated = isChecked;
        });
    }

    // 7. Load Cities, Customer Groups, Custom Groups
    function loadCities() {
        $.get("/api/accounts/cities", function (cities) {
            let $select = $("#cityId");
            $select.empty();
            $select.append('<option value="">(Select City)</option>');
            $.each(cities, function (i, c) {
                $select.append(`<option value="${c.id}">${c.cityName}</option>`);
            });
        });
    }

    function loadCustomerGroups() {
        $.get("/api/accounts/customer-groups", function (groups) {
            allCustomerGroups = groups || [];
            renderCustomerGroupOptions();
        });
    }

    // Ditto cmbsupgroupsfill (AcfrmDefCoa.cs:945-987): the SAME full CustomerGroup list is
    // fetched once, then filtered by row Id depending on the current Account Type - Id 22
    // ("Sup/Cust Group" acting as a Customer group) only offers Ids 7/9/10; Id 3 (Supplier)
    // offers every OTHER Id; any other Account Type clears the list (moot: the field is
    // disabled by applyCustomerGroupRule() in that case anyway). Preserves the current
    // selection when it's still a valid option after rebuilding.
    function renderCustomerGroupOptions() {
        const typeVal = parseInt($("#accountTypeId").val());
        const current = $("#customerGroupId").val();
        let $select = $("#customerGroupId");
        $select.empty();
        $select.append('<option value="">(Select Group)</option>');

        let filtered = [];
        if (typeVal === 22) {
            filtered = allCustomerGroups.filter(g => [7, 9, 10].includes(g.id));
        } else if (typeVal === 3) {
            filtered = allCustomerGroups.filter(g => ![7, 9, 10].includes(g.id));
        }

        $.each(filtered, function (i, g) {
            $select.append(`<option value="${g.id}">${g.description}</option>`);
        });

        if (current && filtered.some(g => String(g.id) === String(current))) {
            $select.val(current);
        }
    }

    function loadCustomGroups() {
        $.get("/api/accounts/custom-groups", function (groups) {
            let $select = $("#customGroupId");
            $select.empty();
            $select.append('<option value="">(Select Custom Group)</option>');
            $.each(groups, function (i, g) {
                $select.append(`<option value="${g.id}">${g.acLookUpsDescription || g.description}</option>`);
            });
        });
    }

    // 8. History Tab Audit Tree Grid (Replicates Janus GridEX Tree View & Actions)
    function loadHistoryGrid() {
        const uptoLevel = parseInt($("input[name='rdUptoLevel']:checked").val()) || 4;
        const levels = [];
        for (let l = 1; l <= uptoLevel; l++) {
            levels.push(l);
        }

        const status = $("input[name='radStatus']:checked").val() || "All";
        const isExpand = $("input[name='rdExpandCollapse']:checked").val() === "Expand";

        if (!isExpand) {
            collapsedNodes.clear();
        }

        $.get("/api/accounts/history", { levels: levels.join(","), status: status }, function (data) {
            gridState.history.data = data || [];
            gridState.history.page = 1;
            renderHistoryGrid();
        });
    }

    $("#btnShowHistory, input[name='rdUptoLevel'], input[name='radStatus'], input[name='rdExpandCollapse']").on("change click", function (e) {
        if (e.type === "click" && this.id !== "btnShowHistory") return;
        loadHistoryGrid();
    });

    function renderHistoryGrid() {
        const state = gridState.history;
        let $tbody = $("#grdHistory").find("tbody");
        $tbody.empty();

        const total = state.data.length;
        const totalPages = Math.ceil(total / state.pageSize) || 1;
        const start = (state.page - 1) * state.pageSize;
        const pageData = state.data.slice(start, start + state.pageSize);

        if (pageData.length === 0) {
            $tbody.append(`<tr><td colspan="11" style="text-align:center; color:#94a3b8; padding:12px;">No account history records found</td></tr>`);
        } else {
            $.each(pageData, function (i, h) {
                const level = h.accountLevel || 1;
                const indent = (level - 1) * 16;
                const isGroup = h.accountGroup === "Group";
                const isCollapsed = collapsedNodes.has(h.accountCode);
                const toggleIcon = isGroup ? (isCollapsed ? '<i class="fa fa-plus-square-o tree-toggle" style="cursor:pointer; margin-right:4px; color:#00796B;"></i>' : '<i class="fa fa-minus-square-o tree-toggle" style="cursor:pointer; margin-right:4px; color:#00796B;"></i>') : '<span style="display:inline-block; width:14px;"></span>';
                
                const activeColor = h.isActive === 'True' ? '#15803d' : '#dc2626';

                $tbody.append(`<tr data-id="${h.id}" data-code="${h.accountCode}" data-level="${level}">
                    <td style="padding-left: ${indent + 6}px;">
                        ${toggleIcon}
                        <input type="text" class="form-control-erp edit-hist-title" value="${h.accountTitle}" style="display:inline-block; width:calc(100% - 24px); border:none; background:transparent; font-weight:${isGroup ? 'bold' : 'normal'};"/>
                    </td>
                    <td style="font-weight:bold; color:#00796B;">${h.accountCode}</td>
                    <td><input type="text" class="form-control-erp edit-hist-other" value="${h.otherErpCode || ''}" style="width:100%; border:none; background:transparent;"/></td>
                    <td>${h.accountGroup || 'Group'}</td>
                    <td>${h.accountLevel || 1}</td>
                    <td>
                        <select class="form-control-erp edit-hist-active" style="color: ${activeColor}; font-weight: bold; border:none; background:transparent;">
                            <option value="True" ${h.isActive === 'True' ? 'selected' : ''} style="color:#15803d;">True</option>
                            <option value="False" ${h.isActive === 'False' ? 'selected' : ''} style="color:#dc2626;">False</option>
                        </select>
                    </td>
                    <td>${h.accountClass || ''}</td>
                    <td>${h.accountType || ''}</td>
                    <td>${h.noteTitle || ''}</td>
                    <td style="text-align:center;">
                        <button type="button" class="btn-erp btn-hist-update" data-id="${h.id}" style="padding:1px 6px; font-size:10px;"><i class="fa fa-pencil"></i> Update</button>
                    </td>
                    <td style="text-align:center;">
                        <button type="button" class="btn-erp btn-hist-sibling" data-parent="${h.parentAccountCode || '0'}" style="padding:1px 4px; font-size:10px;" title="Add Sibling">Sibling</button>
                        ${level < 4 ? `<button type="button" class="btn-erp btn-hist-child" data-code="${h.accountCode}" style="padding:1px 4px; font-size:10px; margin-left:2px;" title="Add Child">Child</button>` : ''}
                    </td>
                </tr>`);
            });
        }

        renderPagingBar("#pagingHistory", state.page, totalPages, total, function (newPage) {
            state.page = newPage;
            renderHistoryGrid();
        });

        // Toggle Expand/Collapse Tree Rows
        $(".tree-toggle").on("click", function () {
            const $tr = $(this).closest("tr");
            const code = $tr.data("code");
            if (collapsedNodes.has(code)) {
                collapsedNodes.delete(code);
            } else {
                collapsedNodes.add(code);
            }
            renderHistoryGrid();
        });

        // Inline Update Row Action
        $(".btn-hist-update").on("click", function () {
            const $tr = $(this).closest("tr");
            const id = $(this).data("id");
            const newTitle = $tr.find(".edit-hist-title").val().trim();
            const newOther = $tr.find(".edit-hist-other").val().trim();
            const newActive = $tr.find(".edit-hist-active").val() === "True";

            if (!newTitle) {
                alert("Account Title Field Required");
                return;
            }

            $.ajax({
                url: "/api/accounts/save",
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify({
                    account: {
                        id: id,
                        accountTitle: newTitle,
                        otherErpCode: newOther,
                        isActive: newActive
                    }
                }),
                success: function () {
                    alert("Record Updated Successfully!");
                    loadHistoryGrid();
                },
                error: function (xhr) {
                    alert("Update Failed: " + (xhr.responseJSON ? xhr.responseJSON.message : "Error"));
                }
            });
        });

        // Add Sibling Action
        $(".btn-hist-sibling").on("click", function () {
            const parentCode = $(this).data("parent");
            $(".erp-tab-button[data-target='#tabForm']").click();
            $("#parentAccountCode").val(parentCode).trigger("change");
            $("#accountTitleInput").focus();
        });

        // Add Child Action
        $(".btn-hist-child").on("click", function () {
            const parentCode = $(this).data("code");
            $(".erp-tab-button[data-target='#tabForm']").click();
            $("#parentAccountCode").val(parentCode).trigger("change");
            $("#accountTitleInput").focus();
        });
    }

    // 9. Load Account for Editing (Req #14, #15, #19)
    function loadAccountForEdit(id) {
        $.get(`/api/accounts/${id}`, function (resp) {
            const acc = resp.account || resp;
            const opBal = resp.openingBalance != null ? resp.openingBalance : 0.0;

            currentAccountId = acc.id;
            $("#accountId").val(acc.id);
            $("#parentAccountCode").val(acc.parentAccountCode || "0");
            currentSelectedParentCode = acc.parentAccountCode || "0";

            $("#accountTitleInput").val(acc.accountTitle || "");
            $("#accountTitleOtherLingo").val(acc.accountTitleOtherLingo || "");
            $("#accountTypeId").val(acc.accountTypeId || "");
            $("#plNoteId").val(acc.plNoteId || "");
            $("#bsNoteId").val(acc.bsNoteId || "");
            $("#customerGroupId").val(acc.customerGroupId || "");
            $("#cityId").val(acc.cityId || "");
            $("#accountGroup").val(acc.accountGroup || "Group");
            $("#accountCode").val(acc.accountCode || "");
            $("#openingBalance").val(opBal !== 0 ? opBal : "");

            const classId = acc.accountClass;
            $("#accountClass").val(classId || "");
            $("#accountClassText").val(accountClassMap[classId] || "(Unset)");

            const lvl = acc.accountLevel || 1;
            $("#accountLevel").val(lvl);
            $("#otherErpCode").val(acc.otherErpCode || "");
            $("#contactNo").val(acc.contactNo || "");

            updateFieldStates(lvl, classId);
            loadLocations(acc.id);
            loadChildGrid(acc.parentAccountCode || "0");

            $("#btnSaveCoa").html('<i class="fa fa-pencil"></i> Update');
        });
    }

    // 10. Form Actions (Save, New, Refresh, Print, Define City)
    function bindFormEvents() {
        $("#coaForm").on("submit", function (e) {
            e.preventDefault();
            saveAccountDefinition();
        });

        $("#btnNew").on("click", resetForm);
        $("#btnRefresh").on("click", refreshScreen);
        $("#btnPrint").on("click", printReport);
        $("#btnDefineCity").on("click", openCityModal);
        $("#btnSaveCityModal").on("click", saveCityModal);
    }

    // Ditto formvalidation() (AcfrmDefCoa.cs:719-762) - same checks, same order, same
    // messages. Two desktop checks are intentionally NOT reproduced here because their
    // inputs don't exist anywhere in this Java port yet (no Configuration/ERPFeature
    // lookup has been built): "Please Select Custom Group" (cmbactype==3 AND server
    // config CustomGroupCompulsoryOnChartofAccount) and "Please Select Currency"
    // (MultiCurrencyFeature/feature 6 AND Detail). Faking either flag's value would be
    // inventing behavior, not porting it - see JAVA-PROGRESS.md.
    function saveAccountDefinition() {
        const title = $("#accountTitleInput").val().trim();
        if (!title) {
            alert("Please Enter Account Title");
            $("#accountTitleInput").focus();
            return;
        }

        const parentCode = $("#parentAccountCode").val() || "0";
        if (String(parentCode).trim() === "") {
            alert("Please Enter Parent Account");
            $("#parentAccountCode").focus();
            return;
        }

        const code = $("#accountCode").val();
        const level = parseInt($("#accountLevel").val()) || 1;
        const accountTypeVal = $("#accountTypeId").val();

        if (level === 3 && !String(accountTypeVal || "").trim()) {
            alert("Please Enter Account Type");
            $("#accountTypeId").focus();
            return;
        }

        if (!$("#plNoteId").prop("disabled") && level === 3 && !String($("#plNoteId").val() || "").trim()) {
            alert("Please Enter PL Notes");
            $("#plNoteId").focus();
            return;
        }

        if (!$("#bsNoteId").prop("disabled") && level === 3 && !String($("#bsNoteId").val() || "").trim()) {
            alert("Please Enter BS Notes");
            $("#bsNoteId").focus();
            return;
        }

        if (!$("#customerGroupId").prop("disabled") && level === 4 && !String($("#customerGroupId").val() || "").trim()) {
            alert("Please Select Customer Group");
            $("#customerGroupId").focus();
            return;
        }

        const accountObj = {
            id: currentAccountId,
            parentAccountCode: parentCode,
            accountTitle: title,
            accountTitleOtherLingo: $("#accountTitleOtherLingo").val(),
            accountTypeId: parseInt($("#accountTypeId").val()) || null,
            plNoteId: parseInt($("#plNoteId").val()) || null,
            bsNoteId: parseInt($("#bsNoteId").val()) || null,
            customerGroupId: parseInt($("#customerGroupId").val()) || null,
            cityId: parseInt($("#cityId").val()) || null,
            accountGroup: $("#accountGroup").val() || "Group",
            accountCode: code,
            accountClass: parseInt($("#accountClass").val()) || null,
            accountLevel: parseInt($("#accountLevel").val()) || 1,
            otherErpCode: $("#otherErpCode").val(),
            contactNo: $("#contactNo").val()
        };

        const openingBal = parseFloat($("#openingBalance").val()) || 0.0;
        const allocations = gridState.alloc.data || [];

        $.ajax({
            url: "/api/accounts/save",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify({ account: accountObj, openingBalance: openingBal, allocations: allocations }),
            success: function (resp) {
                alert("Account Saved Successfully!");
                const savedParentCode = parentCode;
                currentAccountId = null;
                $("#accountId").val("");
                $("#accountTitleInput").val("");
                $("#accountTitleOtherLingo").val("");
                $("#otherErpCode").val("");
                $("#contactNo").val("");
                $("#openingBalance").val("");
                $("#btnSaveCoa").html('<i class="fa fa-save"></i> Save');

                onParentSelected(savedParentCode);
                loadChildGrid(savedParentCode);
                loadHistoryGrid();
                loadLocations(null);
            },
            error: function (xhr) {
                alert("Save Failed: " + (xhr.responseJSON ? xhr.responseJSON.message : "Error occurred"));
            }
        });
    }

    function resetForm() {
        currentAccountId = null;
        $("#accountId").val("");
        $("#coaForm")[0].reset();
        $("#btnSaveCoa").html('<i class="fa fa-save"></i> Save');
        onParentSelected("0");
    }

    function refreshScreen() {
        loadParentDropdown();
        loadLevel1Grid();
        loadHistoryGrid();
        loadLocations();
        $("#filterAccountTitle").val("");
        loadChildGrid(currentSelectedParentCode);
    }

    function printReport() {
        const printWin = window.open("", "_blank", "width=900,height=600");
        $.get("/api/accounts/search", { query: "" }, function (data) {
            let html = `<html><head><title>114-Chart of Accounts Report</title>
                <style>
                    body { font-family: Arial, sans-serif; font-size: 11px; padding: 15px; }
                    h2 { color: #00796B; margin-bottom: 4px; }
                    table { width: 100%; border-collapse: collapse; margin-top: 10px; }
                    th { background: #00796B; color: white; padding: 4px; border: 1px solid #004d40; }
                    td { padding: 4px; border: 1px solid #ccc; }
                    tr:nth-child(even) { background: #f9f9f9; }
                </style>
            </head><body>
                <h2>Golden Ace Rice Mills (Pvt) Ltd.</h2>
                <h3>Chart of Accounts Report (114-AcRptChartOfAccounts.rpt)</h3>
                <table>
                    <thead>
                        <tr>
                            <th>Account Code</th>
                            <th>Account Title</th>
                            <th>Account Class</th>
                            <th>Account Group</th>
                            <th>Level</th>
                        </tr>
                    </thead>
                    <tbody>`;
            $.each(data, function (i, acc) {
                html += `<tr>
                    <td><b>${acc.accountCode}</b></td>
                    <td>${acc.accountTitle}</td>
                    <td>${acc.accountClass || ''}</td>
                    <td>${acc.accountGroup || 'Group'}</td>
                    <td>${acc.accountLevel || 1}</td>
                </tr>`;
            });
            html += `</tbody></table></body></html>`;
            printWin.document.write(html);
            printWin.document.close();
            printWin.print();
        });
    }

    function openCityModal() {
        $("#newCityName").val("");
        $("#cityModal").modal("show");
    }

    function saveCityModal() {
        const name = $("#newCityName").val().trim();
        if (!name) {
            alert("City Name is Required");
            return;
        }
        $.ajax({
            url: "/api/accounts/city/save",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify({ cityName: name }),
            success: function (city) {
                alert("City Saved Successfully!");
                $("#cityModal").modal("hide");
                loadCities();
            }
        });
    }

    // Helper: Janus GridEX Paging Bar Renderer (Record: |< < X Of Y (Total: N) > >|)
    function renderPagingBar(containerSelector, currentPage, totalPages, totalRecords, onPageChange) {
        let $container = $(containerSelector);
        $container.empty();

        let html = `<div class="erp-paging-bar">
            <span>Record: </span>
            <button type="button" class="btn-page btn-first" ${currentPage <= 1 ? 'disabled' : ''}><i class="fa fa-step-backward"></i></button>
            <button type="button" class="btn-page btn-prev" ${currentPage <= 1 ? 'disabled' : ''}><i class="fa fa-caret-left"></i></button>
            <span class="page-info"><b>${currentPage}</b> Of <b>${totalPages}</b> (Total: ${totalRecords.toLocaleString()})</span>
            <button type="button" class="btn-page btn-next" ${currentPage >= totalPages ? 'disabled' : ''}><i class="fa fa-caret-right"></i></button>
            <button type="button" class="btn-page btn-last" ${currentPage >= totalPages ? 'disabled' : ''}><i class="fa fa-step-forward"></i></button>
        </div>`;

        $container.html(html);

        $container.find(".btn-first").on("click", function () { if (currentPage > 1) onPageChange(1); });
        $container.find(".btn-prev").on("click", function () { if (currentPage > 1) onPageChange(currentPage - 1); });
        $container.find(".btn-next").on("click", function () { if (currentPage < totalPages) onPageChange(currentPage + 1); });
        $container.find(".btn-last").on("click", function () { if (currentPage < totalPages) onPageChange(totalPages); });
    }
});
