function getFormData($form) {
    var unindexed_array = $form.serializeArray();
    var indexed_array = {};

    $.map(unindexed_array, function (n, i) {
        indexed_array[n['name']] = n['value'];
    });

    return indexed_array;
}

Number.prototype.toFixed = function (decimalPlaces) {
    return Number(Math.round(this + 'e' + decimalPlaces) + 'e-' + decimalPlaces);
}


$(document).ready(function () {

    var now = new Date();

    var day = ("0" + now.getDate()).slice(-2);
    var month = ("0" + (now.getMonth() + 1)).slice(-2);

    var today = now.getFullYear() + "-" + (month) + "-" + (day);

    window.onload = function () {
        $('.entryDate').val(today);
    }

    $(".entry_add").on("click", function () {

        var data1 = $(this).closest("tr").find("td:eq(0) input[type='date']").val();
        var data2 = $(this).closest("tr").find("td:eq(1) select").val();
        var data3 = $(this).closest("tr").find("td:eq(2) select").val();
        var data4 = $(this).closest("tr").find(".rate").val();
        var data5 = $(this).closest("tr").find("td:eq(4) input[type='number']").val();
        var data6 = $(this).closest("tr").find("td:eq(5) textarea").val();

        console.log("My Value " + data4);

        if (parseInt(data4) == 0 || parseInt(data4) === null) {
            alert("Please! Enter Rate ");
            $(this).closest("tr").find("td:eq(3) input[type='number']").focus();
            return;
        }

        $(this).closest("tr").find(".select2_single").select2("destroy");

        var newRow = $(this).closest("tr").clone(true);

        newRow.children("td").children("input, span").each(function (index, element) {
            //console.log("my object is ", element);
            $(element).val("");
            $(element).text("");
        });

        newRow.children("td").each(function (index, element) {
            if ($(this).children().length < 1) {
                $(element).text("");
            }
            //$(element).find(".itemCategory:first").val("");
            $(element).find(".itemDef:first").val();
            $(element).find(".mill:first").val();

            $(element).find(".rate").val("0");
            $(element).find(".cashRate").val("0");
            $(element).find(".entryDate").val(today);
            $(element).find(".remarks").val("");
        });

        newRow.insertAfter($(this).closest("tr"));
        $("#mill_rate_table span#serial_no").each(function (index, element) {
            $(element).text(index + 1);
            $(element).closest("tr").find("input[name*='.id']").attr("id", "millRateEntries" + index + ".id");
            $(element).closest("tr").find("input[name*='.id']").attr("name", "millRateEntries[" + index + "].id");

            $(element).closest("tr").find("input[name*='.entryDate']").attr("id", "millRateEntries" + index + ".entryDate");
            $(element).closest("tr").find("input[name*='.entryDate']").attr("name", "millRateEntries[" + index + "].entryDate");

            //  console.log(index)
            $(element).closest("tr").find("select[name*='.itemDef']").attr("id", "millRateEntries" + index + ".itemDef");
            $(element).closest("tr").find("select[name*='.itemDef']").attr("name", "millRateEntries[" + index + "].itemDef");


            $(element).closest("tr").find("select[name*='.mill']").attr("id", "millRateEntries" + index + ".mill.id");
            $(element).closest("tr").find("select[name*='.mill']").attr("name", "millRateEntries[" + index + "].mill.id");

            $(element).closest("tr").find("input[name*='.rate']").attr("id", "millRateEntries" + index + ".rate");
            $(element).closest("tr").find("input[name*='.rate']").attr("name", "millRateEntries[" + index + "].rate");

            $(element).closest("tr").find("input[name*='.cashRate']").attr("id", "millRateEntries" + index + ".cashRate");
            $(element).closest("tr").find("input[name*='.cashRate']").attr("name", "millRateEntries[" + index + "].cashRate");


        });
        // again instrument select2 dropdowns
        $("#mill_rate_table .select2_single").select2({dropdownAutoWidth: true, width: '100%'});

        generateTabIndexing();
    });

    $(".entry_delete").on("click", function () {
        if ($(this).parents("tbody").find("tr").length > 1) {
            $(this).closest("tr").remove();
        }
        $("#mill_rate_table span#serial_no").each(function (index, element) {
            $(element).text(index + 1);
            $(element).closest("tr").find("input[name*='.id']").attr("id", "millRateEntries" + index + ".id");
            $(element).closest("tr").find("input[name*='.id']").attr("name", "millRateEntries[" + index + "].id");

            $(element).closest("tr").find("input[name*='.entryDate']").attr("id", "millRateEntries" + index + ".entryDate");
            $(element).closest("tr").find("input[name*='.entryDate']").attr("name", "millRateEntries[" + index + "].entryDate");

            //  console.log(index)
            $(element).closest("tr").find("select[name*='.itemDef']").attr("id", "millRateEntries" + index + ".itemDef");
            $(element).closest("tr").find("select[name*='.itemDef']").attr("name", "millRateEntries[" + index + "].itemDef");


            $(element).closest("tr").find("select[name*='.mill']").attr("id", "millRateEntries" + index + ".mill.id");
            $(element).closest("tr").find("select[name*='.mill']").attr("name", "millRateEntries[" + index + "].mill.id");

            $(element).closest("tr").find("input[name*='.rate']").attr("id", "millRateEntries" + index + ".rate");
            $(element).closest("tr").find("input[name*='.rate']").attr("name", "millRateEntries[" + index + "].rate");

            $(element).closest("tr").find("input[name*='.cashRate']").attr("id", "millRateEntries" + index + ".cashRate");
            $(element).closest("tr").find("input[name*='.cashRate']").attr("name", "millRateEntries[" + index + "].cashRate");


        });
    });
});

$("#submitGLForm").on("click", function () {

    var millRateList = [];
    $("#mill_rate_table tbody tr").each(function () {

        var data1 = $(this).find(".entryDate").val();
        var data2 = $(this).find(".itemDef").val();
        var data3 = $(this).find(".mill").val();
        var data4 = $(this).find(".rate").val();
        var data5 = $(this).find(".cashRate").val();
        var data6 = $(this).find(".remarks").val();
        // var data3 = $(this).find("td:eq(1) input[type='date']").val();
        var myData = {
            entryDate: data1,
            itemDef: {id: data2},
            mill: {id: data3},
            rate: data4,
            cashRate: data5,
            remarks: data6
        }
        millRateList.push(myData);
        //console.log("Date: " + data1 + " Item: " + data2 + " Mill: " + data3 + " Rate: " + data4 + " Cash Rate: " + data5 + " Remarks: " + data6);
    });
    console.log(millRateList);
    $.ajax({

        type: "POST",
        contentType: "application/json",
        url: "/stock/mill-rates",
        data: JSON.stringify(millRateList),
        dataType: "text",
        success: function (data) {
            console.log(data);
            window.location.reload();
        },
        error: function () {
            console.log();
            location.reload();
        }
    });

    generateTabIndexing();
});

generateTabIndexing();

function generateTabIndexing() {
    var tabIndex = 0;
    $("#mill_rate_table tbody td").each(function (i) {
        //  console.log($(this).find(".supplierAccount").val());
        if ($(this).find("select.select2_single").length) {
            tabIndex = tabIndex + 1;
            $(this).find("select.select2_single:eq(0)").attr('tabindex', tabIndex);

        } else if ($(this).find("input:not(:hidden)").length) {
            tabIndex = tabIndex + 1;
            $(this).find("input:not(:hidden):eq(0)").attr('tabindex', tabIndex);

        } else if ($(this).find("a").length) {
            tabIndex = tabIndex + 1;
            $(this).find("a:eq(0)").attr('tabindex', tabIndex);

        }
    });
}

$("input[type=number]").on("focus", function () {
    $(this).on("keydown", function (event) {
        if (event.keyCode === 38 || event.keyCode === 40) {
            event.preventDefault();
        }
    });
});

$('input[type=number]').on('wheel', function (e) {
    return false;
});


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
});
