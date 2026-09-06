Number.prototype.toFixed = function (decimalPlaces) {
    return Number(Math.round(this + 'e' + decimalPlaces) + 'e-' + decimalPlaces);
}

$(document).ready(function () {

    $("#itemRecipeForm #company\\.id").on("change", function () {
        loadCompanyBranches();
    });

    $(".select2_single").on("select2:close", function () {
        $(this).focus();
    });

    generateTabIndexing();

    $("#submitForm").off().on("click", function (event) {

        $("#submitForm").addClass('disabled');
        setTimeout(function () {
            $("#submitForm").removeClass('disabled');
        }, 10000);

        // Prevent the form from submitting via the browser.
        event.preventDefault();
        event.stopImmediatePropagation();
        // Will immediately show the confirmation popup
        $.confirm({
            title: "CONFIRMATION REQUIRED",
            content: "ARE YOU SURE YOU WANT TO SUBMIT THIS RECIPE",
            buttons: {
                confirm: function () {
                    // SUBMIT FORM
                    $("#itemRecipeForm").submit();
                },
                cancel: function () {
                },
            }
        });
    });

    // SUBMIT FORM
    $("#itemRecipeForm").submit(function () {
        let itemEmpty = false;
        let itemAmountZero = false;
        let itemDuplicateExists = false;
        let itemDuplicate = "";
        let itemArray = [];

        $("#voucher_entry_table tbody tr").each(function () {
            if (!$(this).find(".recipeEntry_itemDef").val() || $(this).find(".recipeEntry_itemDef").val() == 0) {
                itemEmpty = true;
            }

            if (parseFloat($(this).find(".quantityUsed").text()) == 0) {
                itemAmountZero = true;
            }

            if ($(this).find(".itemDef").val() && $.inArray($(this).find(".itemDef").val(), itemArray) >= 0) {
                itemDuplicateExists = true;
                itemDuplicate = $(this).find(".itemDef option:selected").text();
            } else {
                itemArray.push($(this).find(".itemDef").val());
            }
        });

        if (itemEmpty) {
            $.confirm({
                title: "ENCOUNTERED AN ERROR!",
                content: "PLEASE SELECT ITEM FOR EACH RECIPE ENTRY",
                type: 'red',
                typeAnimated: true,
            });
            // Prevent the form from submitting via the browser.
            return false;
        }

        if (itemAmountZero) {
            $.confirm({
                title: "ENCOUNTERED AN ERROR!",
                content: "ITEM QUANTITY MUST BE GREATER THEN ZERO",
                type: 'red',
                typeAnimated: true,
            });
            // Prevent the form from submitting via the browser.
            return false;
        }

        if (itemDuplicateExists) {
            $.confirm({
                title: "ENCOUNTERED AN ERROR!",
                content: "DUPLICATE ITEM EXISTS: " + itemDuplicate,
                type: 'red',
                typeAnimated: true,
            });
            // Prevent the form from submitting via the browser.
            return false;
        }

        // prevent double submit
        if ($("#itemRecipeForm").data("submitted")) {
            return false;
        } else {
            $("#itemRecipeForm").data("submitted", true);
        }
    });

    $(".entry_add").on("click", function () {

        // un-instrument select2 dropdowns
        $(this).closest("tr").find(".select2_single").select2("destroy");

        let newRow = $(this).closest("tr").clone(true);

        //when you use clone, even the text is carried over. To remove it, do the following:
        newRow.children("td").children("input").each(function (index, element) {
            $(element).attr("readonly", false);
            $(element).val("");
        });
        newRow.children("td").children("select").each(function (index, element) {
            $(element).find("option").attr("disabled", false);
            //console.log(index)
            $(element).val(1);
        });

        newRow.insertAfter($(this).closest("tr"));

        // again instrument select2 dropdowns
        $("#voucher_entry_table .select2_single").select2({dropdownAutoWidth: true, width: '100%'});

        // update serial numbers
        $("#voucher_entry_table span.serial_no").each(function (index, element) {
            $(element).text(index + 1);
            $(element).closest("tr").find("input[name*='.sequence']:first").val(index + 1);

            $(element).closest("tr").find("input[name*='.sequence']").attr("id", "recipeEntries" + index + ".sequence");
            $(element).closest("tr").find("input[name*='.sequence']").attr("name", "recipeEntries[" + index + "].sequence");

            $(element).closest("tr").find("input[name*='.id']").attr("id", "recipeEntries" + index + ".id");
            $(element).closest("tr").find("input[name*='.id']").attr("name", "recipeEntries[" + index + "].id");
            $(element).closest("tr").find("input[name*='.id']").attr("value", 0);

            $(element).closest("tr").find("select[name*='.itemDef.id']").attr("id", "recipeEntries[" + index + "].itemDef.id");
            $(element).closest("tr").find("select[name*='.itemDef.id']").attr("name", "recipeEntries[" + index + "].itemDef.id");

            $(element).closest("tr").find("input[name*='.quantityUsed']").attr("id", "recipeEntries" + index + ".quantityUsed");
            $(element).closest("tr").find("input[name*='.quantityUsed']").attr("name", "recipeEntries[" + index + "].quantityUsed");
        });

        generateTabIndexing();
    });

    $(".entry_delete").on("click", function () {

        if ($(this).parents("tbody").find("tr").length > 2) {
            $(this).closest("tr").remove();
        }

        // update serial numbers
        $("#voucher_entry_table span.serial_no").each(function (index, element) {
            $(element).text(index + 1);
            $(element).closest("tr").find("input[name*='.sequence']:first").val(index + 1);

            $(element).closest("tr").find("input[name*='.sequence']").attr("id", "recipeEntries" + index + ".sequence");
            $(element).closest("tr").find("input[name*='.sequence']").attr("name", "recipeEntries[" + index + "].sequence");

            $(element).closest("tr").find("input[name*='.id']").attr("id", "recipeEntries" + index + ".id");
            $(element).closest("tr").find("input[name*='.id']").attr("name", "recipeEntries[" + index + "].id");

            $(element).closest("tr").find("select[name*='.itemDef.id']").attr("id", "recipeEntries[" + index + "].itemDef.id");
            $(element).closest("tr").find("select[name*='.itemDef.id']").attr("name", "recipeEntries[" + index + "].itemDef.id");

            $(element).closest("tr").find("input[name*='.quantityUsed']").attr("id", "recipeEntries" + index + ".quantityUsed");
            $(element).closest("tr").find("input[name*='.quantityUsed']").attr("name", "recipeEntries[" + index + "].quantityUsed");
        });

        generateTabIndexing();
        // calculate total on row delete
        // calculateTotal();
    });



    function generateTabIndexing() {
        let tabIndex = 0;
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


    function loadCompanyBranches() {

        if (!$("#company\\.id").val() || $("#company\\.id").val() == 0) {
            return;
        }

        $("#voucher_entry_table").find(".voucherEntry_branch").empty();

        $.get("/vouchers/company_branches?companyId=" + $("#company\\.id").val(), function (data) {
            $("#voucher_entry_table tr").each(function () {
                if ($(this).find("td").length) {

                    $(this).find(".branch").empty();
                    // $(this).find(".voucherEntry_branch").append("<option></option>");

                    for (let i = 0, len = data.length; i < len; i++) {
                        let option = "<option value = " + data[i].id + ">" + data[i].name + "</option>";
                        $(this).find(".branch").append(option);
                    }
                }
            });
        });
    }

    // Catch the keydown for the entire document
    $(document).keydown(function (e) {

        // Set self as the current item in focus
        let self = $(':focus'),
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
});