Number.prototype.toFixed = function (decimalPlaces) {
    return Number(Math.round(this + "e" + decimalPlaces) + "e-" + decimalPlaces);
};

$(document).ready(function () {
    var formatter4 = new Intl.NumberFormat("ur-PK", {minimumFractionDigits: 4, maximumFractionDigits: 4});
    var formatter0 = new Intl.NumberFormat("ur-PK", {minimumFractionDigits: 0, maximumFractionDigits: 0});


    $("#myTab a:first").tab("show");

    if ($("#dashboardAdmin").val() === 'true') {
        dailyAttendance();

        //console.log("i m index ....");
        myDefault();

        myCharts(1);
        //alert("i m start indexing");

        profitabilityTrendChart(0);

        topProducts(1);

        topCustomers(1);


    } else {
        //marketingAdmin

    }

    $("#salesTrendFilter").on("change", function () {
        console.log("Change Sales Trend Value " + $("#salesTrendFilter").val());
        myCharts($("#salesTrendFilter").val());
    });

    $("#profitabilityTrendFilter").on("change", function () {
        console.log("Change Sales Trend Value " + $("#profitabilityTrendFilter").val());
        profitabilityTrendChart($("#profitabilityTrendFilter").val());
    });

    $("#topProductFilter").on("change", function () {
        console.log("Change Sales Trend Value " + $("#topProductFilter").val());
        topProducts($("#topProductFilter").val());
    });

    $("#topCustomerFilter").on("change", function () {
        console.log("Change Sales Trend Value " + $("#topCustomerFilter").val());
        topCustomers($("#topCustomerFilter").val());
    });


    function myDefault() {
        //console.log("i m index loading ....");

        var profitabiltyFrom = {
            companyId: 0,
            branchId: 0,
            voucherStatusId: 0,
            financialYearId: 0,
            fromDate: "1900-01-01",
            toDate: "1900-01-01",
            itemDefId: 0,
            itemCategoryId: 0,
            accountCode: 0
        };
        // $("#loading").show();
        // DO POST
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: "/reports/sales_index_report",
            data: JSON.stringify(profitabiltyFrom),
            dataType: "json",
            success: function (data) {

                // FILL TABLE ROWS
                $.each(data, function (i, sLEntry) {
                    if (i === 0) {
                        $("#isalesYearly span").html(formatter0.format(parseFloat(sLEntry.allSales).toFixed(0)));
                    }

                    if (i === 1) {
                        $("#isalesToday span").html(formatter0.format(parseFloat(sLEntry.todaySales).toFixed(0)));
                    }

                    if (i === 2) {
                        $("#isalesAll span").html(formatter0.format(parseFloat(sLEntry.todayRsSales).toFixed(0)));
                    }

                    if (i === 3) {
                        $("#isalesYesterday span").html(formatter0.format(parseFloat(sLEntry.yesterdaySales).toFixed(0)));
                        //console.log("2: " + sLEntry.yesterdaySales);
                    }

                    if (i === 4) {
                        $("#isalesMonthly span").html(formatter0.format(parseFloat(sLEntry.monthlySales).toFixed(0)));
                        //console.log("3: " + sLEntry.monthlySales);
                    }

                    if (i === 5) {
                        $("#isalesCashYearly span").html(formatter0.format(parseFloat(sLEntry.allCashSales).toFixed(0)));
                    }

                    if (i === 6) {
                        $("#isalesCashToday span").html(formatter0.format(parseFloat(sLEntry.todayCashSales).toFixed(0)));
                    }

                    if (i === 7) {
                        $("#isalesCashAll span").html(formatter0.format(parseFloat(sLEntry.todayRsCashSales).toFixed(0)));
                    }

                    if (i === 8) {
                        $("#isalesCashYesterday span").html(formatter0.format(parseFloat(sLEntry.yesterdayCashSales).toFixed(0)));
                    }

                    if (i === 9) {
                        $("#isalesCashMonthly span").html(formatter0.format(parseFloat(sLEntry.monthlyCashSales).toFixed(0)));
                    }

                });
                // $("#loading").hide();
            },
            complete: function () {

            }
        });
        // PREPARE FORM DATA
        indexTableProfitability();


        // PREPARE FORM DATA
        var formDataSalesOrder = {
            company: {id: 0, name: ""},
            branch: {id: 0, name: ""},
            voucherStatus: {id: 0, name: ""},
            saleOrderCode: "",
            pending: 0,
            searchByDate: 1,
            fromDate: "1900-01-01",
            toDate: "1900-01-01"
        };

        var liquidAssets = 0;
        $("#loading").show();
        // DO POST
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: "/reports/sales_order_index_report",
            data: JSON.stringify(formDataSalesOrder),
            dataType: "json",
            success: function (data) {
                // FILL TABLE ROWS

                liquidAssets = 0;


                $.each(data, function (i, saleOrder) {
                    //console.log(saleOrder);
                    if (saleOrder.detail)
                        liquidAssetDetailFunction(saleOrder.detail);
                    if (i === 0) {
                        $("#isalesOrderAll span").html(formatter0.format(parseFloat(saleOrder.allSalesOrder).toFixed(0)));
                    }
                    if (i === 1) {
                        $("#todayWTSalesOrder span").html(formatter0.format(parseFloat(saleOrder.todayWTSalesOrder).toFixed(0)));
                    }
                    if (i === 2) {
                        $("#allCompletedSalesOrder span").html(formatter0.format(parseFloat(saleOrder.allCompletedSalesOrder).toFixed(0)));
                    }
                    if (i === 3) {
                        $("#allPendingSalesOrder span").html(formatter0.format(parseFloat(saleOrder.allPendingSalesOrder).toFixed(0)));
                    }
                    if (i === 4) {
                        $("#allMonthlySalesOrder span").html(formatter0.format(parseFloat(saleOrder.allMonthlySalesOrder).toFixed(0)));
                    }
                    if (i === 5) {
                        $("#mCashInHand span").html(formatter0.format(parseFloat(saleOrder.mCashInHand).toFixed(0)));
                        liquidAssets = parseFloat(liquidAssets) + parseFloat(saleOrder.mCashInHand);
                        console.log("1: " + liquidAssets);
                    }
                    if (i === 6) {
                        $("#mCashInBank span").html(formatter0.format(parseFloat(saleOrder.mCashInBank).toFixed(0)));
                        liquidAssets = parseFloat(liquidAssets) + parseFloat(saleOrder.mCashInBank);
                        console.log("2: " + liquidAssets);
                    }
                    if (i === 7) {
                        $("#mCheque span").html(formatter0.format(parseFloat(saleOrder.mCheque).toFixed(0)));
                        liquidAssets = parseFloat(liquidAssets) + parseFloat(saleOrder.mCheque);
                        console.log("3: " + liquidAssets);
                    }
                    if (i === 8) {
                        $("#mReceivables span").html(formatter0.format(parseFloat(saleOrder.mReceivables).toFixed(0)));
                    }
                    if (i === 9) {
                        $("#mPayables span").html(formatter0.format(parseFloat(saleOrder.mPayables).toFixed(0)));
                    }
                    if (i === 10) {
                        $("#mTodayPurchase span").html(formatter0.format(parseFloat(saleOrder.mTodayPurchase).toFixed(0)));
                    }

                    if (i === 11) {
                        $("#mStockRate span").html(formatter0.format(parseFloat(saleOrder.mStockRate).toFixed(0)));
                    }
                    if (i === 12) {
                        $("#mStockWeightage span").html(formatter0.format(parseFloat(saleOrder.mStockWeightage).toFixed(0)));
                    }
                    if (i === 13) {
                        $("#mPendingPOs span").html(formatter0.format(parseFloat(saleOrder.mPendingPOs).toFixed(0)));
                    }
                    if (i === 14) {
                        $("#mCollection span").html(formatter0.format(parseFloat(saleOrder.mCollection).toFixed(0)));
                    }
                    if (i === 15) {
                        $("#mCollectionMonth span").html(formatter0.format(parseFloat(saleOrder.mCollectionMonth).toFixed(0)));
                    }
                    if (i === 16) {
                        $("#mPaymentMonth span").html(formatter0.format(parseFloat(saleOrder.mPaymentMonth).toFixed(0)));
                    }
                    if (i === 17) {
                        $("#mPaymentYear span").html(formatter0.format(parseFloat(saleOrder.mPaymentYear).toFixed(0)));
                    }

                });

                $("#liquidAssets span").html(formatter0.format(parseFloat(liquidAssets).toFixed(0)));
                $("#loading").hide();
            },
            complete: function () {
                $("#loading").hide();
            }
        });
    }

    function myCharts(vOptions) {
        var mLabels = [];
        //var mLabels1 = [];
        var mData = [];
        var mChartLabel = "";

        var mChartData;

        // PREPARE FORM DATA
        var formData = {
            companyId: 0,
            branchId: 0,
            voucherStatusId: 0,
            financialYearId: 0,
            fromDate: "1900-01-01",
            toDate: "1900-01-01",
            itemDefId: 0,
            itemCategoryId: 0,
            accountCode: 0
        };

        //console.log("My Charts " + parseInt(vOptions));

        if (vOptions === 1 || parseInt(vOptions) === 1) {
            //	console.log("First Option is Called");
            mChartLabel = "Rs.";
            $("#loading").show();
            $.ajax({
                type: "POST",
                contentType: "application/json",
                url: "/reports/index_chart_sales1",
                data: JSON.stringify(formData),
                dataType: "json",
                success: function (data) {
                    mChartData = data;
                    //console.log("Start Working");
                    $.each(data, function (propName, propVal) {
                        //console.log(propName);
                        $.each(propVal, function (propName1, propVal1) {
                            if (propName1 === "aLabel") {
                                mLabels.push(propVal1);
                            }

                            if (propName1 === "aData") {
                                mData.push(propVal1);
                            }
                            //console.log(propName1, propVal1);
                        });
                    });

                    if ($("#canvas_salesTrend").length) {
                        //document.getElementById("canvas_salesTrend").clear();
                        var canvas = document.getElementById("canvas_salesTrend");
                        var context = canvas.getContext("2d");
                        //new Chart(document.getElementById("canvas_salesTrend"), {
                        new Chart(context, {
                            type: "line",
                            responsive: true,
                            animation: true,
                            data: {
                                labels: mLabels,
                                datasets: [{
                                    label: mChartLabel,
                                    data: mData,
                                    fill: false,
                                    borderColor: "rgb(189, 215, 238)",
                                    tension: 0.1
                                }]
                            }
                        });
                    }
                    $("#loading").hide();
                    //console.log("End Working");

                },
                complete: function () {
                    $("#loading").hide();
                }
            });
        } else if (vOptions === 2 || parseInt(vOptions) === 2) {
            //	console.log("Second Option is Called");
            mChartLabel = "Rs.";
            $("#loading").show();
            $.ajax({
                type: "POST",
                contentType: "application/json",
                url: "/reports/index_chart_sales2",
                data: JSON.stringify(formData),
                dataType: "json",
                success: function (data) {
                    mChartData = data;
                    //console.log("Start Working");
                    $.each(data, function (propName, propVal) {
                        //console.log(propName);
                        $.each(propVal, function (propName1, propVal1) {
                            if (propName1 === "aLabel") {
                                mLabels.push(propVal1);
                            }

                            if (propName1 === "aData") {
                                mData.push(propVal1);
                            }
                            //console.log(propName1, propVal1);
                        });
                    });

                    if ($("#canvas_salesTrend").length) {
                        //document.getElementById("canvas_salesTrend").clear();
                        //new Chart(document.getElementById("canvas_salesTrend"), {
                        var canvas1 = document.getElementById("canvas_salesTrend");
                        var context1 = canvas1.getContext("2d");
                        //new Chart(document.getElementById("canvas_salesTrend"), {
                        new Chart(context1, {
                            type: "line",
                            responsive: true,
                            animation: true,
                            data: {
                                labels: mLabels,
                                datasets: [{
                                    label: mChartLabel,
                                    data: mData,
                                    fill: false,
                                    borderColor: "rgb(189, 215, 238)",
                                    tension: 0.1
                                }]
                            }
                        });
                    }
                    $("#loading").hide();
                    //console.log("End Working");

                },
                complete: function () {
                    $("#loading").hide();
                }
            });
        } else if (vOptions === 3 || parseInt(vOptions) === 3) {
            // console.log("Third Option is Called");
            mChartLabel = "Rs.";
            $("#loading").show();
            $.ajax({
                type: "POST",
                contentType: "application/json",
                url: "/reports/index_chart_sales3",
                data: JSON.stringify(formData),
                dataType: "json",
                success: function (data) {
                    mChartData = data;
                    //console.log("Start Working");
                    $.each(data, function (propName, propVal) {
                        //console.log(propName);
                        $.each(propVal, function (propName1, propVal1) {
                            if (propName1 === "aLabel") {
                                mLabels.push(propVal1);
                            }

                            if (propName1 === "aData") {
                                mData.push(propVal1);
                            }
                            //console.log(propName1, propVal1);
                        });
                    });

                    if ($("#canvas_salesTrend").length) {
                        //document.getElementById("canvas_salesTrend").clear();
                        //new Chart(document.getElementById("canvas_salesTrend"), {
                        var canvas2 = document.getElementById("canvas_salesTrend");
                        var context2 = canvas2.getContext("2d");
                        //new Chart(document.getElementById("canvas_salesTrend"), {
                        new Chart(context2, {
                            type: "line",
                            responsive: true,
                            animation: true,
                            data: {
                                labels: mLabels,
                                datasets: [{
                                    label: mChartLabel,
                                    data: mData,
                                    fill: false,
                                    borderColor: "rgb(189, 215, 238)",
                                    tension: 0.1
                                }]
                            }
                        });
                    }
                    $("#loading").hide();
                },
                complete: function () {
                    $("#loading").hide();
                }
            });
        }
    }

    function profitabilityTrendChart(cOptions) {

        //console.log("Bar Chart");
        var mLabels = [];
        var mData = [];
        var mColor = [];
        var mChartLabel = "";

        var mChartData;

        // PREPARE FORM DATA
        var formData = {
            companyId: 0,
            branchId: 0,
            voucherStatusId: 0,
            financialYearId: 0,
            fromDate: "1900-01-01",
            toDate: "1900-01-01",
            itemDefId: 0,
            itemCategoryId: 0,
            accountCode: 0
        };
        var pfcanvas = document.getElementById("canvas_profitabilityTrend");
        var pfcontext = pfcanvas.getContext("2d");

        if (cOptions == 0 || parseInt(cOptions) == 0) {
            mChartLabel = "Rs.";
            $("#profitabilityLoading").show();
            $.ajax({
                type: "POST",
                contentType: "application/json",
                url: "/reports/index_chart_profitability1",
                data: JSON.stringify(formData),
                dataType: "json",
                success: function (data) {
                    mChartData = data;
                    //console.log("Start Working");
                    $.each(data, function (propName, propVal) {
                        //console.log(propName);
                        $.each(propVal, function (propName1, propVal1) {
                            if (propName1 == "aLabel") {
                                mLabels.push(propVal1);
                                mColor.push("rgb(91, 155, 213)");
                            }

                            if (propName1 == "aData") {
                                mData.push(propVal1);
                            }
                            console.log(propName1, propVal1);
                        });
                    });

                    // var labels =["JAN", "FEB","MAR","APR","MAY", "JUN", "JUL"];
                    var datasss = {
                        labels: mLabels,
                        responsive: true,
                        animation: true,
                        datasets: [{
                            label: "GP",
                            data: mData,
                            backgroundColor: mColor,
                            borderColor: mColor,
                            borderWidth: 1
                        }]
                    };

                    if ($("#canvas_profitabilityTrend").length) {

                        var canvas3 = document.getElementById("canvas_profitabilityTrend");
                        var context3 = canvas3.getContext("2d");
                        //new Chart(document.getElementById("canvas_salesTrend"), {
                        new Chart(context3, {
                            type: "line",
                            responsive: true,
                            animation: true,
                            data: {
                                labels: mLabels,
                                datasets: [{
                                    label: mChartLabel,
                                    data: mData,
                                    fill: false,
                                    borderColor: "#000000",
                                    tension: 0.1
                                }]
                            }
                        });

                        // new Chart(document.getElementById("canvas_profitabilityTrend"), {
                        // 	type: "line",
                        // 	responsive: true,
                        // 	animation: true,
                        // 	data: {
                        // 		labels: mLabels,
                        // 		datasets: [{
                        // 			label: mChartLabel,
                        // 			data: mData,
                        // 			fill: false,
                        // 			borderColor: "rgb(189, 215, 238)",
                        // 			tension: 0.1
                        // 		}]
                        // 	}
                        // });
                        //document.getElementById("canvas_profitabilityTrend").clear();
                        //new Chart(document.getElementById("canvas_profitabilityTrend"),{
                        //var canvas3 = document.getElementById("canvas_profitabilityTrend");
                        //var context3 = canvas3.getContext("2d");
                        // pfcanvas = document.getElementById("canvas_profitabilityTrend");
                        // pfcontext = pfcanvas.getContext("2d");
                        //
                        // // Store the current transformation matrix
                        // pfcontext.save();
                        //
                        // // Use the identity matrix while clearing the canvas
                        // pfcontext.setTransform(1, 0, 0, 1, 0, 0);
                        // pfcontext.clearRect(0, 0, pfcanvas.width, pfcanvas.height);
                        //
                        // // Restore the transform
                        // pfcontext.restore();
                        //
                        // new Chart(pfcontext, {
                        // 	type: "line",
                        // 	data: datasss,
                        // 	options: {
                        // 		plugins: {
                        // 			legend: {
                        // 				display: true,
                        // 				labels: {
                        // 					color: "rgb(91, 155, 213)"
                        // 				}
                        // 			}
                        // 		},
                        // 		scales: {
                        // 			y: {
                        // 				beginAtZero: true
                        // 			}
                        // 		}
                        // 	}
                        // });
                    }

                    $("#profitabilityLoading").hide();
                    //console.log("End Working");
                },
                complete: function () {
                    // $("#loading").hide();
                }
            });
        } else if (cOptions == 1 || parseInt(cOptions) == 1) {
            mChartLabel = "Rs.";
            mLabels = [];
            mData = [];
            $("#loading").show();
            $.ajax({
                type: "POST",
                contentType: "application/json",
                url: "/stocks/inventory-profitability-month-chart",
                data: JSON.stringify(formData),
                dataType: "json",
                success: function (data) {
                    mChartData = data;
                    //console.log("Start Working " + mChartData);
                    $.each(data, function (propName, propVal) {
                        //console.log(propName);
                        $.each(propVal, function (propName1, propVal1) {
                            if (propName1 == "aLabel") {
                                mLabels.push(propVal1);
                                mColor.push("rgb(91, 155, 213)");
                            }

                            if (propName1 == "aData") {
                                mData.push(propVal1);
                            }
                            console.log(propName1, propVal1);
                        });
                    });

                    // var labels =["JAN", "FEB","MAR","APR","MAY", "JUN", "JUL"];
                    // var data2 = {
                    // 	labels: mLabels,
                    // 	datasets: [{
                    // 		label: "GP",
                    // 		data: mData,
                    // 		backgroundColor: mColor,
                    // 		borderColor: mColor,
                    // 		borderWidth: 1
                    // 	}]
                    // };

                    var datasss1 = {
                        labels: mLabels,
                        responsive: true,
                        animation: true,
                        datasets: [{
                            label: "GP",
                            data: mData,
                            backgroundColor: mColor,
                            borderColor: mColor,
                            borderWidth: 1
                        }]
                    };

                    if ($("#canvas_profitabilityTrend").length) {

                        var canvas4 = document.getElementById("canvas_profitabilityTrend");
                        var context4 = canvas4.getContext("2d");
                        //new Chart(document.getElementById("canvas_salesTrend"), {
                        new Chart(context4, {
                            type: "line",
                            responsive: true,
                            animation: true,
                            data: {
                                labels: mLabels,
                                datasets: [{
                                    label: mChartLabel,
                                    data: mData,
                                    fill: false,
                                    borderColor: "#000000",
                                    tension: 0.1
                                }]
                            }
                        });

                        //document.getElementById("canvas_profitabilityTrend").clear();
                        //new Chart(document.getElementById("canvas_profitabilityTrend"),{
                        //var canvas4 = document.getElementById("canvas_profitabilityTrend");
                        //var context4 = canvas4.getContext("2d");
                        // pfcanvas = document.getElementById("canvas_profitabilityTrend");
                        // pfcontext = pfcanvas.getContext("2d");
                        //
                        // // Store the current transformation matrix
                        // pfcontext.save();
                        //
                        // // Use the identity matrix while clearing the canvas
                        // pfcontext.setTransform(1, 0, 0, 1, 0, 0);
                        // pfcontext.clearRect(0, 0, pfcanvas.width, pfcanvas.height);
                        //
                        // // Restore the transform
                        // pfcontext.restore();
                        //
                        // new Chart(pfcontext, {
                        // 	type: "line",
                        // 	data: datasss1,
                        // 	options: {
                        // 		plugins: {
                        // 			legend: {
                        // 				display: true,
                        // 				labels: {
                        // 					color: "rgb(91, 155, 213)"
                        // 				}
                        // 			}
                        // 		},
                        // 		scales: {
                        // 			y: {
                        // 				beginAtZero: true
                        // 			}
                        // 		}
                        // 	}
                        // });
                    }

                    $("#loading").hide();
                    //console.log("End Working");
                },
                complete: function () {
                    $("#loading").hide();
                }
            });
        }
    }

    function topProducts(vOptions) {

        // PREPARE FORM DATA
        var formData = {
            companyId: 0,
            branchId: 0,
            voucherStatusId: 0,
            financialYearId: 0,
            fromDate: "1900-01-01",
            toDate: "1900-01-01",
            itemDefId: 0,
            itemCategoryId: 0,
            accountCode: 0
        };

        $("#tblTopProducts").DataTable().clear();
        $("#tblTopProducts").DataTable().destroy();
        var table = $("#tblTopProducts").DataTable({
            //dom: "Bfrtip",
            deferRender: true,
            destroy: true,
            fixedHeader: true,
            bPaginate: false,
            searching: false,
            paging: false,
            info: false,
            columnDefs: [
                {
                    targets: [2],
                    style: "mso-number-format:0\.00000",
                    className: "text-right"
                }
            ]
        });

        if (vOptions == 1 || parseInt(vOptions) == 1) {
            $("#loading").show();
            // DO POST
            $.ajax({
                type: "POST",
                contentType: "application/json",
                url: "/reports/top_product_list1",
                data: JSON.stringify(formData),
                dataType: "json",
                success: function (data) {
                    var rowIndex = 1;
                    // FILL TABLE ROWS
                    $.each(data, function (i, sLEntry) {
                        table.row.add([
                            rowIndex++,
                            sLEntry.aCode + "<br />" + sLEntry.aName,
                            parseFloat(sLEntry.aAmount).toFixed(0)
                        ]);
                    });
                    table.draw();
                    $("#loading").hide();
                },
                complete: function () {
                    $("#loading").hide();
                }
            });
            $("#loading").hide();
        } else if (vOptions == 2 || parseInt(vOptions) == 2) {
            $("#loading").show();
            // DO POST
            $.ajax({
                type: "POST",
                contentType: "application/json",
                url: "/reports/top_product_list2",
                data: JSON.stringify(formData),
                dataType: "json",
                success: function (data) {
                    var rowIndex = 1;
                    // FILL TABLE ROWS
                    $.each(data, function (i, sLEntry) {
                        table.row.add([
                            rowIndex++,
                            sLEntry.aCode + "<br />" + sLEntry.aName,
                            parseFloat(sLEntry.aAmount).toFixed(0)
                        ]);
                    });
                    table.draw();
                    $("#loading").hide();
                },
                complete: function () {
                    $("#loading").hide();
                }
            });
            $("#loading").hide();
        } else if (vOptions == 3 || parseInt(vOptions) == 3) {
            $("#loading").show();
            // DO POST
            $.ajax({
                type: "POST",
                contentType: "application/json",
                url: "/reports/top_product_list3",
                data: JSON.stringify(formData),
                dataType: "json",
                success: function (data) {
                    var rowIndex = 1;
                    // FILL TABLE ROWS
                    $.each(data, function (i, sLEntry) {
                        table.row.add([
                            rowIndex++,
                            sLEntry.aCode + "<br />" + sLEntry.aName,
                            parseFloat(sLEntry.aAmount).toFixed(0)
                        ]);
                    });
                    table.draw();
                    $("#loading").hide();
                },
                complete: function () {
                    $("#loading").hide();
                }
            });
            $("#loading").hide();
        }
    }

    function topCustomers(vOptions) {

        // PREPARE FORM DATA
        var formData = {
            companyId: 0,
            branchId: 0,
            voucherStatusId: 0,
            financialYearId: 0,
            fromDate: "1900-01-01",
            toDate: "1900-01-01",
            itemDefId: 0,
            itemCategoryId: 0,
            accountCode: 0
        };

        $("#tblTopCustomers").DataTable().clear();
        $("#tblTopCustomers").DataTable().destroy();
        var tableCustomer = $("#tblTopCustomers").DataTable({
            //dom: "Bfrtip",
            deferRender: true,
            destroy: true,
            fixedHeader: true,
            bPaginate: false,
            searching: false,
            paging: false,
            info: false,
            columnDefs: [
                {
                    targets: [2],
                    style: "mso-number-format:0\.00000",
                    className: "text-right"

                }
            ]
        });

        if (vOptions == 1 || parseInt(vOptions) == 1) {
            $("#loading").show();
            // DO POST
            $.ajax({
                type: "POST",
                contentType: "application/json",
                url: "/reports/top_customer_list1",
                data: JSON.stringify(formData),
                dataType: "json",
                success: function (data) {
                    var rowIndex = 1;
                    // FILL TABLE ROWS
                    $.each(data, function (i, sLEntry) {
                        tableCustomer.row.add([
                            rowIndex++,
                            sLEntry.aCode + "<br />" + sLEntry.aName,
                            parseFloat(sLEntry.aAmount).toFixed(0)
                        ]);
                    });
                    tableCustomer.draw();
                    $("#loading").hide();
                },
                complete: function () {
                    $("#loading").hide();
                }
            });
            $("#loading").hide();
        } else if (vOptions == 2 || parseInt(vOptions) == 2) {
            $("#loading").show();
            // DO POST
            $.ajax({
                type: "POST",
                contentType: "application/json",
                url: "/reports/top_customer_list2",
                data: JSON.stringify(formData),
                dataType: "json",
                success: function (data) {
                    var rowIndex = 1;
                    // FILL TABLE ROWS
                    $.each(data, function (i, sLEntry) {
                        tableCustomer.row.add([
                            rowIndex++,
                            sLEntry.aCode + "<br />" + sLEntry.aName,
                            parseFloat(sLEntry.aAmount).toFixed(0)
                        ]);
                    });
                    tableCustomer.draw();
                    $("#loading").hide();
                },
                complete: function () {
                    $("#loading").hide();
                }
            });
            $("#loading").hide();
        } else if (vOptions == 3 || parseInt(vOptions) == 3) {
            $("#loading").show();
            // DO POST
            $.ajax({
                type: "POST",
                contentType: "application/json",
                url: "/reports/top_customer_list3",
                data: JSON.stringify(formData),
                dataType: "json",
                success: function (data) {
                    var rowIndex = 1;
                    // FILL TABLE ROWS
                    $.each(data, function (i, sLEntry) {
                        tableCustomer.row.add([
                            rowIndex++,
                            sLEntry.aCode + "<br />" + sLEntry.aName,
                            parseFloat(sLEntry.aAmount).toFixed(0)
                        ]);
                    });
                    tableCustomer.draw();
                    $("#loading").hide();
                },
                complete: function () {
                    $("#loading").hide();
                }
            });
            $("#loading").hide();
        }
    }

    function liquidAssetDetailFunction(data) {


        $("#liquidAssetTableId").DataTable().clear();
        $("#liquidAssetTableId").DataTable().destroy();
        var assetsDetailTable = $("#liquidAssetTableId").DataTable({
            //dom: "Bfrtip",
            deferRender: true,
            destroy: true,
            fixedHeader: true,
            bPaginate: false,
            searching: false,
            paging: false,
            info: false,
            columnDefs: [
                {
                    targets: [2],
                    style: "mso-number-format:0\.00000",
                    className: "text-right"

                }
            ]
        });
        $.each(data, function (i, accrountBalance) {
            assetsDetailTable.row.add([
                ++i,
                accrountBalance.name,
                parseFloat(accrountBalance.balance).toFixed(0)
            ]);
        });
        assetsDetailTable.draw();

    }

    function setCurrentDate() {
        var d = new Date().getDate();
        var m = new Date().getMonth() + 1; // JavaScript months are 0-11
        var y = new Date().getFullYear();
        var myDate = y + "-" + pad(m, 2) + "-" + pad(d, 2);
        return myDate;
        //$("#employee_date").val(y + "-" + pad(m,2) + "-" + pad(d,2));
    }

    function pad(str, max) {
        str = str.toString();
        return str.length < max ? pad("0" + str, max) : str;
    }

    function dailyAttendance() {
        var currentDateIs = setCurrentDate();
        // PREPARE FORM DATA
        var formData = {
            company: {id: 0, name: ""},
            branch: {id: 0, name: ""},
            department: {id: 0, name: ""},
            designation: {id: 0, name: ""},
            attendDate: currentDateIs
        };

        // $("#tblDailyAttendance").DataTable().clear();
        // $("#tblDailyAttendance").DataTable().destroy();
        // var tableDailyAttendance = $("#tblDailyAttendance").DataTable({
        // 	//dom: "Bfrtip",
        // 	deferRender: true,
        // 	destroy: true,
        // 	fixedHeader: true,
        // 	bPaginate: false,
        // 	searching: false,
        // 	paging: false,
        // 	info: false
        // });

        // DO POST
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: "/HumanResource/dailyAttendance",
            data: JSON.stringify(formData),
            dataType: "json",
            success: function (data) {
                // FILL TABLE ROWS

                //tableDailyAttendance.clear().draw();

                $.each(data, function (i, employee) {

                    var mColor = "#000000";
                    var bColor = "#FFFFFF";

                    if (employee.eStatus === "Absent") {
                        mColor = "#9C0006";
                        bColor = "#FFC7CF";
                    } else if (employee.eStatus === "Late") {
                        mColor = "#BF8F00";
                        bColor = "#FFE699";
                    } else if (employee.eStatus !== "In Time") {
                        mColor = "#006100";
                        bColor = "#C6EFCE";
                    }

                    var htmlRowAdd = "<tr>";
                    //htmlRowAdd = htmlRowAdd + "<td style='width: 5%; min-width: 5%; max-width: 5%;'><img src=\"" + employee.ePhoto + "\" height='50' width='50' style='border: 2px black solid; border-radius: 50%;'></td>" +
                    htmlRowAdd = htmlRowAdd + "<td style='color: " + mColor + "; background-color: " + bColor + "; width: 10%; min-width: 10%; max-width: 10%; font-size: medium; '>" + employee.eCode + "</td>" +
                        "<td style='color: " + mColor + "; background-color: " + bColor + "; width: 60%; min-width: 60%; max-width: 60%; font-size: medium; '>" + employee.eName + "</td>" +
                        "<td style='color: " + mColor + "; background-color: " + bColor + "; width: 15%; min-width: 15%; max-width: 15%; font-size: medium; '>" + employee.eTimeIn + "</td>" +
                        "<td style='color: " + mColor + "; background-color: " + bColor + "; width: 15%; min-width: 15%; max-width: 15%; font-size: medium; '>" + employee.eStatus + "</td>";
                    htmlRowAdd = htmlRowAdd + "</tr>";
                    $("#tblDailyAttendance").append(htmlRowAdd);
                    // tableDailyAttendance.row.add([
                    // 	"<img src=\"" + employee.ePhoto +"\" height='50' width='50' style='border: 2px black solid; border-radius: 50%;' />",
                    // 	"<div style='color: " + mColor + "; background-color: " + bColor + "; width: 100%; height: 70px; padding: 10px; font-size: medium;'>" + employee.eCode + "</div>",
                    // 	"<div style='color: " + mColor + "; background-color: " + bColor + "; width: 100%; height: 70px; padding: 10px; font-size: medium;'>" + employee.eName + "</div>",
                    // 	"<div style='color: " + mColor + "; background-color: " + bColor + "; width: 100%; height: 70px; padding: 10px; font-size: medium;'>" + employee.eTimeIn + "</div>",
                    // 	"<div style='color: " + mColor + "; background-color: " + bColor + "; width: 100%; height: 70px; padding: 10px; font-size: medium;'>" + employee.eStatus + "</div>"
                    // ]);
                });

                // tableDailyAttendance.draw();
                // enable submit button
            },
            complete: function () {
                // SOME ACTION HERE
            }
        });
        $("#lastPooledAttendance span").html("Last Synced: ");
        //console.log("Starting Pooling ....");
        /// Last Pooled Data
        $.ajax({
            type: "POST",
            //contentType : "application/json",
            url: "/HumanResource/machineLastPooled",
            //data : JSON.stringify(formData),
            //dataType : "json",
            success: function (data) {
                console.log("Pooled ....");
                $("#lastPooledAttendance span").html("Last Synced: " + data);
            },
            complete: function () {
                // SOME ACTION HERE+
            }
        });
        $("#activeEmployee span").html("-");
        //console.log("Starting Pooling ....");
        /// Last Pooled Data
        $.ajax({
            type: "POST",
            //contentType : "application/json",
            url: "/HumanResource/employeeCounts",
            //data : JSON.stringify(formData),
            //dataType : "json",
            success: function (data) {
                //console.log("Pooled ....");
                $("#activeEmployee span").html(data);
            },
            complete: function () {
                // SOME ACTION HERE+
            }
        });

    }

    function indexTableProfitability() {
        $("#profitLoading").show();
        var formData = {
            companyId: 0,
            branchId: 0,
            voucherStatusId: 0,
            financialYearId: 0,
            fromDate: "1900-01-01",
            toDate: "1900-01-01",
            itemDefId: 0,
            itemCategoryId: 0,
            accountCode: 0,
            isTradingOrAllProfit: $("#profitTypeId").val()
        };

        // DO POST
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: "/dash-board/index-table-profitability",//all-inventory-profitability",
            data: JSON.stringify(formData),
            dataType: "json",
            success: function (data) {
                $("#isTrafingProfitAll span").html(formatter0.format(parseFloat(data.allProfit).toFixed(0)));
                $("#isTrafingProfitToday span").html(formatter0.format(parseFloat(data.today).toFixed(0)));
                $("#isTrafingProfitYesterday span").html(formatter0.format(parseFloat(data.yesterday).toFixed(0)));
                $("#isTrafingProfitWeekly span").html(formatter0.format(parseFloat(data.week).toFixed(0)));
                $("#isTrafingProfitMonthly span").html(formatter0.format(parseFloat(data.month).toFixed(0)));
                $("#isTrafingProfitYearly span").html(formatter0.format(parseFloat(data.year).toFixed(0)));

                $("#profitLoading").hide();
            },
            complete: function () {
                $("#profitLoading").hide();
            }
        });
    }

    $("#profitTypeId").on("change", function () {
        indexTableProfitability();//    allInventoryProfitability();
    });
});