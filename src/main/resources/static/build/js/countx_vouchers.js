Number.prototype.toFixed = function (decimalPlaces) {
    return Number(Math.round(this + 'e' + decimalPlaces) + 'e-' + decimalPlaces);
}


$(document).ready(function () {

    $("#voucherEntries0\\.account\\.code").focus();
    getAppParties();

    function getAppParties() {
        $.get("/app/getAllAccounts", function (data) {
            var selectElement = $('.voucherEntry_account');

            //selectElement.empty();
            for (var i = 0, len = data.length; i < len; i++) {

                var option = "<option value = " + data[i].code + ">" + data[i].formattedCode + " &emsp;" + data[i].accountName + "</option>";
                var option1 = "<option value = " + data[i].code + ">" + data[i].accountName + "</option>";
                $(".voucherEntry_account ").append(option);
            }
            //  $(".voucherEntry_account ").append(option).trigger('change');
        });

    }

    // Store active cheque book info globally so skip button can reuse it
    var activeChequeBookInfo = null;

    function renderSkipButton(chkInput) {
        // Remove any existing skip button for this input
        chkInput.siblings(".cheque-skip-btn").remove();
        var btn = $('<button type="button" class="btn btn-xs btn-warning cheque-skip-btn" title="Cancel this cheque and use the next available one" style="margin-left:4px; padding:2px 7px; font-size:11px;">&#8635; Cancel &amp; Next</button>');
        chkInput.after(btn);
    }

    function removeSkipButton(chkInput) {
        chkInput.siblings(".cheque-skip-btn").remove();
    }

    // Guard: prevents concurrent runs from corrupting the takenNumbers list
    var _fillInProgress = false;

    function updateChequeNumberFields() {
        var vType         = $("#voucherForm #voucherType\\.id").val();
        var isBankVoucher = (vType === "BPV" || vType === "BRV");
        var isBPV         = (vType === "BPV");

        if (!isBankVoucher) {
            activeChequeBookInfo = null;
            $("#chequeBookInfoBox").hide();
            $("#voucher_entry_table tbody tr").each(function () {
                var accVal   = $(this).find(".voucherEntry_account").val();
                var chkInput = $(this).find(".chequeNumber");
                if (accVal && (accVal.startsWith("3312107") || accVal.startsWith("3312104"))) {
                    chkInput.prop("readonly", false);
                } else {
                    chkInput.prop("readonly", true).val("");
                }
                removeSkipButton(chkInput);
            });
            return;
        }

        // Row 1 is the bank account row — always readonly & no skip button
        $("#voucher_entry_table tbody tr:first").find(".chequeNumber").prop("readonly", true).val("");
        $("#voucher_entry_table tbody tr:first").find(".chequeNumber").siblings(".cheque-skip-btn").remove();

        var bankAccount = $("#voucher_entry_table tbody tr:first").find(".voucherEntry_account").val();
        var branchId    = $("#voucher_entry_table tbody tr:first").find(".voucherEntry_branch").val();

        if (!bankAccount || bankAccount == 0 || !branchId || branchId == 0) {
            activeChequeBookInfo = null;
            $("#chequeBookInfoBox").hide();
            $("#voucher_entry_table tbody tr").each(function (index) {
                if (index > 0) { $(this).find(".chequeNumber").prop("readonly", false); }
            });
            return;
        }

        $.get("/vouchers/cheque-books/active-info?bankAccountCode=" + bankAccount + "&branchId=" + branchId, function (response) {
            if (!response.found) {
                activeChequeBookInfo = null;
                $("#chequeBookInfoBox").hide();
                $("#voucher_entry_table tbody tr").each(function (index) {
                    if (index > 0) { var ci = $(this).find(".chequeNumber"); ci.prop("readonly", false); removeSkipButton(ci); }
                });
                return;
            }

            activeChequeBookInfo = response;
            $("#cbInfoName").text(response.name);
            $("#cbInfoLastUsed").text(response.lastUsed);
            $("#cbInfoNextExpected").text(response.nextExpected);
            $("#chequeBookInfoBox").show();

            if (_fillInProgress) return;  // another fill cycle is already running; let it finish
            _fillInProgress = true;

            // ── STEP 1: Collect ALL already-filled cheque numbers across every row ──
            var takenNumbers = [];
            $("#voucher_entry_table tbody tr").each(function (index) {
                if (index > 0) {
                    var v = parseInt($(this).find(".chequeNumber").val(), 10);
                    if (!isNaN(v)) takenNumbers.push(v);
                }
            });

            // ── STEP 2: Collect empty rows that need a cheque number ──
            var emptyRows = [];
            $("#voucher_entry_table tbody tr").each(function (index) {
                if (index > 0) {
                    var chkInput = $(this).find(".chequeNumber");
                    chkInput.prop("readonly", false);
                    if (chkInput.val() === "" && response.nextExpected !== "Fully Used") {
                        emptyRows.push(chkInput);
                        // Show a spinner so the user knows it's being filled
                        chkInput.val("...").prop("readonly", true).css("color", "#aaa");
                    } else if (chkInput.val() !== "") {
                        if (isBPV) renderSkipButton(chkInput); else removeSkipButton(chkInput);
                    }
                }
            });

            if (emptyRows.length === 0) { _fillInProgress = false; return; }

            // ── STEP 3: Sequential AJAX chain — each row gets a unique next cheque ──
            // We search from nextExpected (not startNo) to skip the already-used range fast.
            var searchFrom = parseInt(response.nextExpected, 10) || parseInt(response.startNo, 10);

            function fillNextEmptyRow(idx) {
                if (idx >= emptyRows.length) {
                    _fillInProgress = false;  // all done
                    return;
                }

                var chkInput = emptyRows[idx];
                $.get(
                    "/vouchers/cheque-books/next-unused" +
                    "?bankAccountCode=" + bankAccount +
                    "&branchId="        + branchId    +
                    "&from="            + searchFrom  +
                    "&exclude="         + takenNumbers.join(","),
                    function (result) {
                        chkInput.prop("readonly", false).css("color", "");  // restore style
                        if (result.nextUnused !== null && result.nextUnused !== undefined) {
                            chkInput.val(result.nextUnused);
                            takenNumbers.push(result.nextUnused);   // exclude from every subsequent row
                            searchFrom = result.nextUnused + 1;     // start NEXT search after this one
                            if (isBPV) renderSkipButton(chkInput); else removeSkipButton(chkInput);
                        } else {
                            chkInput.val("");  // no more available cheques
                        }
                        fillNextEmptyRow(idx + 1);
                    }
                ).fail(function () {
                    chkInput.prop("readonly", false).css("color", "").val("");
                    fillNextEmptyRow(idx + 1);
                });
            }

            fillNextEmptyRow(0);
        });
    }

    // ══════════════════════════════════════════════════════════
    // "↻ Next" button  — opens Cancel Cheque modal
    // ══════════════════════════════════════════════════════════
    var _cancelTargetInput = null;   // the .chequeNumber input being operated on

    $(document).on("click", ".cheque-skip-btn", function () {
        var btn      = $(this);
        var chkInput = btn.siblings(".chequeNumber");
        var currentVal = parseInt(chkInput.val(), 10);

        if (!activeChequeBookInfo || !activeChequeBookInfo.found) {
            $.confirm({
                title:   "NO ACTIVE CHEQUE BOOK",
                content: "No active cheque book found for the selected bank account.",
                type: "red", typeAnimated: true,
                buttons: { ok: function(){} }
            });
            return;
        }

        if (isNaN(currentVal)) {
            $.confirm({
                title:   "NO CHEQUE NUMBER",
                content: "There is no cheque number in this field to cancel.",
                type: "orange", typeAnimated: true,
                buttons: { ok: function(){} }
            });
            return;
        }

        // Populate modal with the cheque number about to be cancelled
        _cancelTargetInput = chkInput;
        $("#cancelChequeNoDisplay").text(currentVal);
        $("#cancelChequeBookName").text(
            activeChequeBookInfo.name ? "Book: " + activeChequeBookInfo.name : ""
        );
        $("#cancelChequeReason").val("");
        $("#cancelChequeError").hide().text("");
        $("#cancelChequeConfirmBtn")
            .prop("disabled", false)
            .html('<i class="fa fa-times-circle"></i> Cancel &amp; Use Next');

        $("#cancelChequeModal").modal("show");
        setTimeout(function () { $("#cancelChequeReason").focus(); }, 400);
    });

    // ── Confirm button inside modal ──────────────────────────
    $("#cancelChequeConfirmBtn").on("click", function () {
        var confirmBtn = $(this);
        var chkInput   = _cancelTargetInput;
        if (!chkInput || !chkInput.length) {
            $("#cancelChequeModal").modal("hide");
            return;
        }

        var chequeNo    = parseInt(chkInput.val(), 10);
        var bankAccount = $("#voucher_entry_table tbody tr:first").find(".voucherEntry_account").val();
        var branchId    = $("#voucher_entry_table tbody tr:first").find(".voucherEntry_branch").val();
        var reason      = $.trim($("#cancelChequeReason").val()) || "CANCELLED";
        var startNo     = parseInt(activeChequeBookInfo.startNo, 10);

        // Collect other cheque numbers already in form
        var alreadyUsedInForm = [];
        $("#voucher_entry_table tbody tr").each(function (index) {
            if (index > 0) {
                var otherInput = $(this).find(".chequeNumber");
                if (otherInput[0] !== chkInput[0]) {
                    var v = parseInt(otherInput.val(), 10);
                    if (!isNaN(v)) alreadyUsedInForm.push(v);
                }
            }
        });

        // Spinner
        confirmBtn.prop("disabled", true)
            .html('<i class="fa fa-spinner fa-spin"></i> Saving…');
        $("#cancelChequeError").hide();

        // ── STEP 1: POST cancellation to DB ──────────────────
        $.ajax({
            type: "POST",
            url:  "/vouchers/cheque-books/cancel-cheque-ajax",
            data: {
                bankAccountCode: bankAccount,
                branchId:        branchId,
                chequeNumber:    chequeNo,
                reason:          reason
            },
            success: function (saveResult) {
                if (!saveResult.success) {
                    // Show inline error; keep modal open
                    $("#cancelChequeError").text(saveResult.message || "Could not cancel cheque.").show();
                    confirmBtn.prop("disabled", false)
                        .html('<i class="fa fa-times-circle"></i> Cancel &amp; Use Next');
                    return;
                }

                // ── STEP 2: Find next unused cheque from startNo ──
                $.get(
                    "/vouchers/cheque-books/next-unused" +
                    "?bankAccountCode=" + bankAccount +
                    "&branchId="        + branchId +
                    "&from="            + startNo +
                    "&exclude="         + alreadyUsedInForm.join(","),
                    function (result) {
                        $("#cancelChequeModal").modal("hide");

                        if (result.nextUnused !== null && result.nextUnused !== undefined) {
                            chkInput.val(result.nextUnused);
                            $("#cbInfoNextExpected").text(result.nextUnused);
                        } else {
                            chkInput.val("");
                            $.confirm({
                                title:   "NO MORE CHEQUES",
                                content: "All cheques in the active cheque book have been used or cancelled. Please activate a new cheque book.",
                                type: "red", typeAnimated: true,
                                buttons: { ok: function(){} }
                            });
                        }
                        // Refresh the info box
                        var ba = $("#voucher_entry_table tbody tr:first").find(".voucherEntry_account").val();
                        var br = $("#voucher_entry_table tbody tr:first").find(".voucherEntry_branch").val();
                        $.get("/vouchers/cheque-books/active-info?bankAccountCode=" + ba + "&branchId=" + br, function (info) {
                            if (info.found) {
                                activeChequeBookInfo = info;
                                $("#cbInfoLastUsed").text(info.lastUsed);
                                $("#cbInfoNextExpected").text(info.nextExpected);
                            }
                        });
                    }
                ).fail(function () {
                    $("#cancelChequeModal").modal("hide");
                    $.confirm({
                        title:   "ERROR",
                        content: "Cheque was cancelled but failed to fetch next number. Please refresh.",
                        type: "red", typeAnimated: true,
                        buttons: { ok: function(){} }
                    });
                });
            },
            error: function () {
                $("#cancelChequeError").text("Server error — please try again.").show();
                confirmBtn.prop("disabled", false)
                    .html('<i class="fa fa-times-circle"></i> Cancel &amp; Use Next');
            }
        });
    });


    $(document).on("change", ".voucherEntry_account, .voucherEntry_branch", function() {
        updateChequeNumberFields();
    });

    updateChequeNumberFields(); // initial run

    function setRowColor() {
        $("#voucher_entry_table tbody tr").each(function (index, element) {
            console.log($(this).closest("tr").find(".entryStatus").val());
            if ($(this).closest("tr").find(".entryStatus").val() === "true") {
                $(this).closest("tr").find(".voucherEntry_debit").css("background-color", "#4dff4d");
                $(this).closest("tr").find(".voucherEntry_credit").css("background-color", "#4dff4d");
                $(this).closest("tr").find(".voucherEntry_debit").prop("readonly", true);
                $(this).closest("tr").find(".select2_single").attr("readonly", true);
                $(this).closest("tr").find(".voucherEntry_credit").prop("readonly", true);
            }
        });
    }

    function loadCompanyBranches() {

        if (!$("#company\\.id").val() || $("#company\\.id").val() == 0) {
            return;
        }

        $("#voucher_entry_table").find(".voucherEntry_branch").empty();

        $.get("/vouchers/company_branches?companyId=" + $("#company\\.id").val(), function (data) {
            $("#voucher_entry_table tr").each(function () {
                if ($(this).find("td").length) {

                    $(this).find(".voucherEntry_branch").empty();
                    $(this).find(".voucherEntry_branch").append("<option></option>");

                    for (var i = 0, len = data.length; i < len; i++) {
                        var option = "<option value = " + data[i].id + ">" + data[i].name + "</option>";
                        $(this).find(".voucherEntry_branch").append(option);
                    }
                }
            });
        });
    }

    function loadAccountBranch(thisControl) {
        console.log(thisControl.val());
        if (!$(thisControl).val() || $(thisControl).val() == 0 || !$("#company\\.id").val() || $("#company\\.id").val() == 0) {
            return;
        }

        if ($(thisControl).val().substr(0, 2) == "44" || $(thisControl).val().substr(0, 2) == "55") {
            return;
        }


        $.get("/vouchers/account_branch?companyId=" + $("#company\\.id").val() + "&accountCode=" + $(thisControl).val(), function (data) {

            if (data != null && data.id > 0) {

                $(thisControl).closest("tr").find(".voucherEntry_branch option").attr("disabled", false);

                $(thisControl).closest("tr").find(".voucherEntry_branch").val(data.id);
                $(thisControl).closest("tr").find(".voucherEntry_branch").trigger("change");

//		    	if ($(thisControl).val().indexOf("2212101") !== -1 || $(thisControl).val().indexOf("3312102") !== -1){
//		    		//$(thisControl).closest("tr").find(".voucherEntry_branch :not(:selected)").attr("disabled", "disabled");
//		    	}
//		    	else{
//		    		$(thisControl).closest("tr").find(".voucherEntry_branch :not(:selected)").attr("disabled", false);
//		    	}

            }

        });
    }

    restrictVoucherDate();

    function restrictVoucherDate() {

        if (!$("#voucherForm #financialYear\\.id").val()) {
            return;
        }

        $("#voucherDate").attr("min", $("#voucherForm #financialYear\\.id option:selected").attr("data-value").split("|")[0]);
        $("#voucherDate").attr("max", $("#voucherForm #financialYear\\.id option:selected").attr("data-value").split("|")[1]);
    }

    getNextVoucherCode();

    function getNextVoucherCode() {

        $("#voucherForm #voucherCode").val("");

        if (!$("#voucherForm #voucherType\\.id").val() || !$("#voucherForm #voucherStatus\\.id").val() || !$("#voucherForm #financialYear\\.id").val() || $("#voucherForm #financialYear\\.id").val() == 0) {
            return;
        }

        var entries = [];
        // PREPARE FORM DATA
        var formData = {
            company: {id: $("#voucherForm #company\\.id").val(), name: ""},
            voucherType: {id: $("#voucherForm #voucherType\\.id").val(), name: ""},
            voucherStatus: {id: $("#voucherForm #voucherStatus\\.id").val(), name: ""},
            financialYear: {id: $("#voucherForm #financialYear\\.id").val(), name: ""},
            voucherDate: $("#voucherForm #voucherDate").val(),
            voucherCode: $("#voucherForm #voucherCode").val(),
            voucherNarration: $("#voucherForm #voucherNarration").val(),
            voucherEntries: entries
        }

        // DO POST
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: "/vouchers/next_voucher_code",
            data: JSON.stringify(formData),
            dataType: "json",
            success: function (data) {
                $("#voucherForm #voucherCode").val(data.nextVoucherCode);
            },
        });

    }

    $("#voucherForm #company\\.id").on("change", function () {
        loadCompanyBranches();
        getNextVoucherCode();
    });

    $("#voucherForm #voucherType\\.id").on("change", function () {

        getNextVoucherCode();

        // un-instrument select2 dropdown
        $("#voucher_entry_table tbody tr:first").find(".voucherEntry_account").select2("destroy");

        $("#voucher_entry_table tbody tr:first").find(".voucherEntry_account option").prop("disabled", false);

        if ($("#voucherForm #voucherType\\.id").val() == "BPV" || $("#voucherForm #voucherType\\.id").val() == "BRV") {
            $("#voucher_entry_table tbody tr:first").find(".voucherEntry_account option[value*='3312107']").prop("disabled", false);
            //$("#voucher_entry_table tbody tr:first").find(".voucherEntry_account option:not([value*='3312107'])").prop("disabled", "disabled");
        }

        if ($("#voucherForm #voucherType\\.id").val() == "CPV" || $("#voucherForm #voucherType\\.id").val() == "CRV") {
            $("#voucher_entry_table tbody tr:first").find(".voucherEntry_account option[value*='3312106']").prop("disabled", false);
            //$("#voucher_entry_table tbody tr:first").find(".voucherEntry_account option:not([value*='3312106'])").prop("disabled", "disabled");
        }

        // again instrument select2 dropdown
        $("#voucher_entry_table tbody tr:first").find(".voucherEntry_account").select2({
            dropdownAutoWidth: true,
            width: '100%'
        });
        updateChequeNumberFields();
        generateTabIndexing();

    });

    $("#voucherForm #voucherStatus\\.id").on("change", function () {
        getNextVoucherCode();
    });

    $("#voucherForm #financialYear\\.id").on("change", function () {
        restrictVoucherDate();
        getNextVoucherCode();
    });

    ////////////////////////////////////////////////////
    ////////////////////////////////////////////////////

    //instrument select2 dropdowns "33-12-107"
    $(".select2_single").on("select2:close", function () {
        $(this).focus();
    });

    $("#voucher_entry_table tbody").find(".voucherEntry_account").on("change", function () {
        //loadAccountBranch($(this));
        console.log("dsdd");
        //loadAccountInvoices($(this));
    });

    // load account related branch of each voucher entry
    $("#voucher_entry_table tr").each(function () {
        if ($(this).find("td").length) {
            //loadAccountBranch($(this).find(".voucherEntry_account"));
            //loadAccountInvoices($(this).find(".voucherEntry_account"));
        }
    });

    // calculate total on load
    calculateTotal();

    $(".voucherEntry_debit, .voucherEntry_credit").on("input", function () {
        calculateTotal();
    });

    function calculateTotal() {

        var debitTotal = parseFloat(0);
        var creditTotal = parseFloat(0);

        var formatter = new Intl.NumberFormat('ur-PK', {
            style: 'currency',
            currency: 'PKR',
            minimumFractionDigits: 2,
            maximumFractionDigits: 2
        })
        var formatter2 = new Intl.NumberFormat('ur-PK', {minimumFractionDigits: 0, maximumFractionDigits: 0})

        $("#voucher_entry_table tr").each(function () {
            if ($(this).find("td").length) {

                if ($(this).find(".voucherEntry_debit").val() != "" && parseFloat($(this).find(".voucherEntry_debit").val().replace(/,/g, '')) != 0) {
                    $(this).find(".voucherEntry_credit").prop("readonly", true);

                    $(this).find(".voucherEntry_debit").val(formatter2.format($(this).find(".voucherEntry_debit").val().replace(/,/g, '')));

                } else {
                    $(this).find(".voucherEntry_credit").prop("readonly", false);
                }

                if ($(this).find(".voucherEntry_credit").val() != "" && parseFloat($(this).find(".voucherEntry_credit").val().replace(/,/g, '')) != 0) {
                    $(this).find(".voucherEntry_debit").prop("readonly", true);
                    $(this).find(".voucherEntry_credit").val(formatter2.format($(this).find(".voucherEntry_credit").val().replace(/,/g, '')));
                } else {
                    $(this).find(".voucherEntry_debit").prop("readonly", false);
                }

                debitTotal = debitTotal + parseFloat($(this).find(".voucherEntry_debit").val().replace(/,/g, '') || 0);
                creditTotal = creditTotal + parseFloat($(this).find(".voucherEntry_credit").val().replace(/,/g, '') || 0);
            }
        });

         
        $("#voucher_entry_table span#debitTotal").text(formatter.format(debitTotal));
        $("#voucher_entry_table span#creditTotal").text(formatter.format(creditTotal));

        if ($("#voucher_entry_table span#debitTotal").text() == $("#voucher_entry_table span#creditTotal").text()) {
            $("#voucher_entry_table span#debitTotal").css("color", "#73879C");
            $("#voucher_entry_table span#creditTotal").css("color", "#73879C");
        } else {
            $("#voucher_entry_table span#debitTotal").css("color", "red");
            $("#voucher_entry_table span#creditTotal").css("color", "red");
        }
    }

    $(".entry_add").on("click", function () {

        // un-instrument select2 dropdowns
        $(this).closest("tr").find(".select2_single").select2("destroy");

        var newRow = $(this).closest("tr").clone(true);

        //when you use clone, even the text is carried over. To remove it, do the following:
        newRow.children("td").children("input").each(function (index, element) {
            $(element).attr("readonly", false);
            $(element).val("");
        });
        newRow.children("td").children("textarea").each(function (index, element) {
            $(element).attr("readonly", false);
            $(element).val("");
        });
        newRow.children("td").children("select").each(function (index, element) {
            $(element).find("option").attr("disabled", false);
            $(element).val(1);
        });

        // ── Fix binding errors on new rows ──────────────────
        // 1. id must be "0" not "" (Spring binds "" to long → NumberFormatException)
        newRow.find("input[name*='.id']").val("0");

        // 2. lockLine must be "false" not "" (Spring binds "" to boolean → IllegalArgumentException)
        newRow.find("input.entryStatus").val("false");

        // 3. Clear chequeNumber & remove stale skip button from cloned row
        //    so updateChequeNumberFields() sees it as truly empty and assigns
        //    the correct sequential cheque number without duplicates
        newRow.find(".chequeNumber").val("").prop("readonly", false);
        newRow.find(".cheque-skip-btn").remove();
        // ────────────────────────────────────────────────────

        newRow.insertAfter($(this).closest("tr"));

        // again instrument select2 dropdowns
        $("#voucher_entry_table .select2_single").select2({dropdownAutoWidth: true, width: '100%'});

        // update serial numbers
        $("#voucher_entry_table span.serial_no").each(function (index, element) {
            $(element).text(index + 1);
            $(element).closest("tr").find("input[name*='.sequence']:first").val(index + 1);

            $(element).closest("tr").find("input[name*='.sequence']").attr("id", "voucherEntries" + index + ".sequence");
            $(element).closest("tr").find("input[name*='.sequence']").attr("name", "voucherEntries[" + index + "].sequence");

            $(element).closest("tr").find("input[name*='.id']").attr("id", "voucherEntries" + index + ".id");
            $(element).closest("tr").find("input[name*='.id']").attr("name", "voucherEntries[" + index + "].id");

            $(element).closest("tr").find("select[name*='.branch']").attr("id", "voucherEntries" + index + ".branch");
            $(element).closest("tr").find("select[name*='.branch']").attr("name", "voucherEntries[" + index + "].branch");

            $(element).closest("tr").find("select[name*='.account.code']").attr("id", "voucherEntries[" + index + "].account.code");
            $(element).closest("tr").find("select[name*='.account.code']").attr("name", "voucherEntries[" + index + "].account.code");

            $(element).closest("tr").find("textarea[name*='.narration']").attr("id", "voucherEntries" + index + ".narration");
            $(element).closest("tr").find("textarea[name*='.narration']").attr("name", "voucherEntries[" + index + "].narration");
            $(element).closest("tr").find("input[name*='.narration']").attr("id", "voucherEntries" + index + ".narration");
            $(element).closest("tr").find("input[name*='.narration']").attr("name", "voucherEntries[" + index + "].narration");
            $(element).closest("tr").find("input[name*='.chequeNumber']").attr("id", "voucherEntries" + index + ".chequeNumber");
            $(element).closest("tr").find("input[name*='.chequeNumber']").attr("name", "voucherEntries[" + index + "].chequeNumber");

//			$(element).closest("tr").find( "input[name*='.recBookPageNumber']" ).attr("id", "voucherEntries" + index + ".recBookPageNumber");
//			$(element).closest("tr").find( "input[name*='.recBookPageNumber']" ).attr("name", "voucherEntries[" + index + "].recBookPageNumber");

            $(element).closest("tr").find("input[name*='.recBookPageNumber']").attr("name", "voucherEntries[" + index + "].recBookPageNumber");
            $(element).closest("tr").find("input[name*='.recBookPageNumber']").attr("name", "voucherEntries[" + index + "].recBookPageNumber");

            $(element).closest("tr").find("input[name*='.invoiceNumber']").attr("name", "voucherEntries[" + index + "].invoiceNumber");
            $(element).closest("tr").find("input[name*='.invoiceNumber']").attr("name", "voucherEntries[" + index + "].invoiceNumber");

            $(element).closest("tr").find("input[name*='.recBookNumber']").attr("id", "voucherEntries" + index + ".recBookNumber");
            $(element).closest("tr").find("input[name*='.recBookNumber']").attr("name", "voucherEntries[" + index + "].recBookNumber");
            $(element).closest("tr").find("input[name*='.lockLine']").attr("id", "voucherEntries" + index + ".lockLine");
            $(element).closest("tr").find("input[name*='.lockLine']").attr("name", "voucherEntries[" + index + "].lockLine");
//
            // chequeNumber state is updated dynamically outside the loop


        });

        updateChequeNumberFields();
        generateTabIndexing();
    });

    $(".entry_delete").on("click", function () {

        if ($(this).parents("tbody").find("tr").length > 2) {
            /*if($(this).closest("tr").find(".entryStatus").val()==="true")
                return;*/
            $(this).closest("tr").remove();
        }

        // update serial numbers
        $("#voucher_entry_table span.serial_no").each(function (index, element) {
            $(element).text(index + 1);
            $(element).closest("tr").find("input[name*='.sequence']:first").val(index + 1);

            $(element).closest("tr").find("input[name*='.sequence']").attr("id", "voucherEntries" + index + ".sequence");
            $(element).closest("tr").find("input[name*='.sequence']").attr("name", "voucherEntries[" + index + "].sequence");

            $(element).closest("tr").find("input[name*='.id']").attr("id", "voucherEntries" + index + ".id");
            $(element).closest("tr").find("input[name*='.id']").attr("name", "voucherEntries[" + index + "].id");

            $(element).closest("tr").find("select[name*='.branch']").attr("id", "voucherEntries" + index + ".branch");
            $(element).closest("tr").find("select[name*='.branch']").attr("name", "voucherEntries[" + index + "].branch");

            $(element).closest("tr").find("select[name*='.account.code']").attr("id", "voucherEntries[" + index + "].account.code");
            $(element).closest("tr").find("select[name*='.account.code']").attr("name", "voucherEntries[" + index + "].account.code");

            $(element).closest("tr").find("textarea[name*='.narration']").attr("id", "voucherEntries" + index + ".narration");
            $(element).closest("tr").find("textarea[name*='.narration']").attr("name", "voucherEntries[" + index + "].narration");
            $(element).closest("tr").find("input[name*='.narration']").attr("id", "voucherEntries" + index + ".narration");
            $(element).closest("tr").find("input[name*='.narration']").attr("name", "voucherEntries[" + index + "].narration");
            $(element).closest("tr").find("input[name*='.chequeNumber']").attr("id", "voucherEntries" + index + ".chequeNumber");
            $(element).closest("tr").find("input[name*='.chequeNumber']").attr("name", "voucherEntries[" + index + "].chequeNumber");

//			$(element).closest("tr").find( "input[name*='.recBookNumber']" ).attr("id", "voucherEntries" + index + ".recBookNumber");
//			$(element).closest("tr").find( "input[name*='.recBookNumber']" ).attr("name", "voucherEntries[" + index + "].recBookNumber");
//			
            $(element).closest("tr").find("input[name*='.recBookPageNumber']").attr("name", "voucherEntries[" + index + "].recBookPageNumber");
            $(element).closest("tr").find("input[name*='.recBookPageNumber']").attr("name", "voucherEntries[" + index + "].recBookPageNumber");

            $(element).closest("tr").find("input[name*='.invoiceNumber']").attr("name", "voucherEntries[" + index + "].invoiceNumber");
            $(element).closest("tr").find("input[name*='.invoiceNumber']").attr("name", "voucherEntries[" + index + "].invoiceNumber");

            $(element).closest("tr").find("input[name*='.recBookNumber']").attr("id", "voucherEntries" + index + ".recBookNumber");
            $(element).closest("tr").find("input[name*='.recBookNumber']").attr("name", "voucherEntries[" + index + "].recBookNumber");
            $(element).closest("tr").find("input[name*='.lockLine']").attr("id", "voucherEntries" + index + ".lockLine");
            $(element).closest("tr").find("input[name*='.lockLine']").attr("name", "voucherEntries[" + index + "].lockLine");
//			
            // chequeNumber state is updated dynamically outside the loop
        });

        updateChequeNumberFields();
        generateTabIndexing();
        // calculate total on row delete
        calculateTotal();
    });

    $("#submitForm").off().on("click", function (event) {

        // prevent double-click on submit
        /*if($("#submitForm").data('clicked')){
            return false;
        }

        else{
            $("#submitForm").data('clicked', true);
        }*/

        if ($("#submitForm").prop("disabled")) {
            return false;
        }

        $("#submitForm").prop("disabled", true);
        setTimeout(function () {
            $("#submitForm").prop("disabled", false);
        }, 10000);

        // Prevent the form from submitting via the browser.
        event.preventDefault();
        event.stopImmediatePropagation();
        // Will immediately show the confirmation popup
        $.confirm({
            title: "CONFIRMATION REQUIRED",
            content: "ARE YOU SURE YOU WANT TO SUBMIT THIS VOUCHER",
            buttons: {
                confirm: function () {
                    // SUBMIT FORM
                    $("#voucherForm").submit();
                },
                cancel: function () {
                },
            }
        });
    });

    // SUBMIT FORM
    $("#voucherForm").submit(function () {

        if ($("#voucherCode").val().length == 0) {
            $.confirm({
                title: "ENCOUNTERED AN ERROR!",
                content: "PLEASE SELECT ALL PARAMETERS TO GENERATE VOUCHER CODE",
                type: 'red',
                typeAnimated: true,
            });
            // Prevent the form from submitting via the browser.
            return false;
        }

        if ($("#voucherDate").val().length == 0 || !($("#voucherDate").val() >= $("#voucherDate").attr("min") && $("#voucherDate").val() <= $("#voucherDate").attr("max"))) {
            $.confirm({
                title: "ENCOUNTERED AN ERROR!",
                content: "VOUCHER DATE MUST LIE IN RANGE OF FINANCIAL YEAR",
                type: 'red',
                typeAnimated: true,
            });
            // Prevent the form from submitting via the browser.
            return false;
        }

        var branchEmpty = false;
        $("#voucher_entry_table tbody tr").each(function () {
            if (!$(this).find(".voucherEntry_branch").val() || $(this).find(".voucherEntry_branch").val() == 0) {
                branchEmpty = true;
            }
        });

        if (branchEmpty) {
            $.confirm({
                title: "ENCOUNTERED AN ERROR!",
                content: "PLEASE SELECT BRANCH FOR EACH VOUCHER ENTRY",
                type: 'red',
                typeAnimated: true,
            });
            // Prevent the form from submitting via the browser.
            return false;
        }

        var accountEmpty = false;
        $("#voucher_entry_table tbody tr").each(function () {
            if (!$(this).find(".voucherEntry_account").val() || $(this).find(".voucherEntry_account").val() == 0) {
                accountEmpty = true;
            }
        });

        if (accountEmpty) {
            $.confirm({
                title: "ENCOUNTERED AN ERROR!",
                content: "PLEASE SELECT ACCOUNT FOR EACH VOUCHER ENTRY",
                type: 'red',
                typeAnimated: true,
            });
            // Prevent the form from submitting via the browser.
            return false;
        }

        if ($("#voucher_entry_table span#debitTotal").text() !== $("#voucher_entry_table span#creditTotal").text()) {
            $.confirm({
                title: "ENCOUNTERED AN ERROR!",
                content: "DEBIT AMOUNT DOES NOT MATCH CREDIT AMOUNT",
                type: 'red',
                typeAnimated: true,
            });
            // Prevent the form from submitting via the browser.
            return false;
        }


        if (($("#voucherForm #voucherType\\.id").val() == "OJV" || $("#voucherForm #voucherType\\.id").val() == "BRV" || $("#voucherForm #voucherType\\.id").val() == "CRV") && (!$("#voucherForm #voucher_id").val() || $("#voucherForm #voucher_id").val() === "0")) {

            if ($("#voucherForm #voucher_entry_table tbody tr:first").find(".voucherEntry_credit").val() != "" && parseFloat($("#voucherForm #voucher_entry_table tbody tr:first").find(".voucherEntry_credit").val()) != 0) {
                $.confirm({
                    title: "ENCOUNTERED AN ERROR!",
                    content: $("#voucherForm #voucherType\\.id").val() + " FIRST ENTRY MUST BE DEBIT ENTRY",
                    type: 'red',
                    typeAnimated: true,
                });
                // Prevent the form from submitting via the browser.
                return false;
            }

            var debit_count = 0;
            $("#voucherForm #voucher_entry_table tbody tr").each(function () {
                if ($(this).find(".voucherEntry_debit").val() != "" && parseFloat($(this).find(".voucherEntry_debit").val()) != 0) {
                    debit_count = debit_count + 1;
                }
            });
            if (debit_count > 1) {
                $.confirm({
                    title: "ENCOUNTERED AN ERROR!",
                    content: $("#voucherForm #voucherType\\.id").val() + " CANNOT HAVE MORE THAN ONE DEBIT ENTRY",
                    type: 'red',
                    typeAnimated: true,
                });
                // Prevent the form from submitting via the browser.
                return false;
            }
        }
        if (($("#voucherForm #voucherType\\.id").val() == "BPV" || $("#voucherForm #voucherType\\.id").val() == "CPV") && (!$("#voucherForm #voucher_id").val() || $("#voucherForm #voucher_id").val() === "0")) {

            if ($("#voucherForm #voucher_entry_table tbody tr:first").find(".voucherEntry_debit").val() != "" && parseFloat($("#voucherForm #voucher_entry_table tbody tr:first").find(".voucherEntry_debit").val()) != 0) {
                $.confirm({
                    title: "ENCOUNTERED AN ERROR!",
                    content: $("#voucherForm #voucherType\\.id").val() + " FIRST ENTRY MUST BE CREDIT ENTRY",
                    type: 'red',
                    typeAnimated: true,
                });
                // Prevent the form from submitting via the browser.
                return false;
            }

            var credit_count = 0;
            $("#voucherForm #voucher_entry_table tbody tr").each(function () {
                if ($(this).find(".voucherEntry_credit").val() != "" && parseFloat($(this).find(".voucherEntry_credit").val()) != 0) {
                    credit_count = credit_count + 1;
                }
            });
            if (credit_count > 1) {
                $.confirm({
                    title: "ENCOUNTERED AN ERROR!",
                    content: $("#voucherForm #voucherType\\.id").val() + " CANNOT HAVE MORE THAN ONE CREDIT ENTRY",
                    type: 'red',
                    typeAnimated: true,
                });
                // Prevent the form from submitting via the browser.
                return false;
            }
        }

        // prevent double submit
        if ($("#voucherForm").data("submitted")) {
            return false;
        } else {
            $("#voucherForm").data("submitted", true);
        }
    });


    generateTabIndexing();

    function generateTabIndexing() {
        var tabIndex = 0;
        $("#voucher_entry_table tbody td").each(function (i) {
            if ($(this).find("select.select2_single").length) {
                tabIndex = tabIndex + 1;
                $(this).find("select.select2_single:eq(0)").attr('tabindex', tabIndex);
            } else if ($(this).find("select").length) {
                tabIndex = tabIndex + 1;
                $(this).find("select:eq(0)").attr('tabindex', tabIndex);
            } else if ($(this).find("textarea").length) {
                tabIndex = tabIndex + 1;
                $(this).find("textarea").attr('tabindex', tabIndex);
            } else if ($(this).find("input:not(:hidden)").length) {
                tabIndex = tabIndex + 1;
                $(this).find("input:not(:hidden):eq(0)").attr('tabindex', tabIndex);

            } else if ($(this).find("a").length) {
                tabIndex = tabIndex + 1;
                $(this).find("a:eq(0)").attr('tabindex', tabIndex);

            }
        });
    }

//	$("input[type=number]").on("focus", function() {
//	    $(this).on("keydown", function(event) {
//	        if (event.keyCode === 38 || event.keyCode === 40) {
//	            event.preventDefault();
//	        }
//	     });
//	 });
//	
//	$('input[type=number]').on('wheel', function(e){
//	    return false;
//	});
//	$('select').on('wheel', function(e){
//	    return false;
//	});

    //$("#sale_order_entry_table select, input:not(:hidden)").each(function (i) { $(this).attr('tabindex', i + 1); });


    // Catch the keydown for the entire document
    $(document).keydown(function (e) {

        // Set self as the current item in focus
        var self = $(':focus'),
            // Set the form by the current item in focus
            form = self.parents('form:eq(0)'),
            focusable;

        // Array of Indexable/Tab-able items
        //focusable = form.find('input').filter(':visible');
        focusable = form.find(':input:enabled:not([readonly], input:hidden, button:hidden, textarea:hidden), textarea:enabled:not([readonly] textarea:hidden), a.entry_add')
            .not(function () {   // do not include inputs with hidden parents
                return $(this).parent().is(':hidden');
            });

        function enterKey() {
            if (e.which === 13 && !self.is('textarea,div[contenteditable=true]')) { // [Enter] key

                // If not a regular hyperlink/button/textarea
                if ($.inArray(self, focusable) && (!self.is('button'))) {
                    // Then prevent the default [Enter] key behaviour from submitting the form
                    e.preventDefault();
                } // Otherwise follow the link/button as by design, or put new line in textarea

                if (self.is('a.entry_add')) {
                    self.click();
                    focusable = form.find(':input:enabled:not([readonly], input:hidden, button:hidden, textarea:hidden), textarea:enabled:not([readonly] textarea:hidden), a.entry_add')
                        .not(function () {   // do not include inputs with hidden parents
                            return $(this).parent().is(':hidden');
                        });
                }

                // Focus on the next item (either previous or next depending on shift)
                if (comingFromSelect2) {
                    focusable.eq(focusable.index(self) + (e.shiftKey ? -1 : 0)).focus();
                    comingFromSelect2 = false;
                } else {
                    if (focusable.index(self) < 0) {
                        return false;
                    }
                    focusable.eq(focusable.index(self) + (e.shiftKey ? -1 : 1)).focus();
                }
                //focusable.eq(focusable.index(self) + (e.shiftKey ? -1 : 1)).focus();

                return false;
            }
        }

        // We need to capture the [Shift] key and check the [Enter] key either way.
        if (e.shiftKey) {
            enterKey()
        } else {
            enterKey()
        }

        if (e.altKey && e.keyCode == 83) {
            $("#voucherForm").submit();
            //location.reload();
        }
    });
    setRowColor();
});
