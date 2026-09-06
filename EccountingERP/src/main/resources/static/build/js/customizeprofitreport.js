$(document).ready(function() {
	//alert("i m here in customized profit");
	var buttonName = "";
	restrictFromAndToDate();
	//loadFromAndToAccounts();

	$("#level").val(3);

	loadFromAndToAccounts();

	$("#companyIds").on("change", function () {
		loadCompanyBranches();
	});

	$("#financialYearId").on("change", function () {
		restrictFromAndToDate();
	});

	$("#level").on("change", function () {
		loadFromAndToAccounts();
	});

	$("#tb_report_datatable").DataTable().clear();
	$("#tb_report_datatable").DataTable().destroy();
	var table = $("#tb_report_datatable").DataTable({
		dom: "Bfrtip",
		"bPaginate": false,
		columnDefs:[
			{  targets: [ 4,5,6 ], style: "mso-number-format:0\\.00000", className: "dt-right" }
		],
		buttons: [
			{
				extend: "copy",
				text: "COPY",
				title: null,
				footer: true,
				customize: function(doc){
					buttonName = "COPY";
					userLog();
				}
			},
			/*$.extend(true, {}, getExcelBuilder(), {
				extend: "excelHtml5",
				text: "EXCEL",
				title: null,
				footer: true,
				exportOptions: {
					stripNewlines: false
				},
				customize: function(doc){
					buttonName = "Excel";
					userLog();
				}
			}),*/
			/*{
                extend: 'excelHtml5',
                title: 'General Ledger' + ' (' + (new Date().toLocaleString()) + ')',
                messageTop: 'COMPANY: ' + 'MST' + '\n' + 'ACCOUNT: ' + '111222333',
                messageBottom: null,
                stripNewlines: false
            },*/
			{
				extend: "pdfHtml5",
				download: "open",
				extension:".tax",
				footer: true,
				filename:  "MST",
				text: '<i class="fa fa-file-pdf-o " style=color:green > PDF</i>',
				title: "CUSTOMIZED PROFIT REPORT",
				footer: true,
				messageTop: "MESSAGE TOP",
				messageBottom: null,
				messageTop: null,
				messageBottom: null,
				pageSize: "A4",
				exportOptions: {
					columns: [0, 1, 2, 3, 4, 5, 6],
					stripNewlines: false
				},
				customize: function(doc){
					doc.defaultStyle.fontSize = 8;
					//pageMargins [left, top, right, bottom]
					doc.pageMargins = [15, 15, 15, 25];
					doc.styles.tableHeader.fontSize = 8;
					doc.styles.tableFooter.fontSize = 8;
					doc.defaultStyle.alignment = "left";
					doc.styles.tableHeader.alignment = "left";
					doc.styles.tableFooter.alignment = "left";
					doc.content[1].layout = {
						hLineWidth: function(i, node) {
							return (i === 0 || i === node.table.body.length) ? 2 : 1;},
						vLineWidth: function(i, node) {
							return (i === 0 || i === node.table.widths.length) ? 2 : 1;},
						hLineColor: function(i, node) {
							return (i === 0 || i === node.table.body.length) ? "black" : "gray";},
						vLineColor: function(i, node) {
							return (i === 0 || i === node.table.widths.length) ? "black" : "gray";}
					};
//                    { data: "active", width: '7%', render: function(data, type, row){
//                        if(type === 'display'){
//                            return (data == true)? '<i class="fa fa-check text-green" aria-hidden="true"></i>' : '';
//                        }else if(type == 'export'){
//                            return (data == true)? '<span class="text-green">A</span>' : 'I';
//                        }else{
//                            return (data == true)? 'A' : 'I';
//                        }
//                    }, className: 'text-center'
//                };
					buttonName ="PDF";
					userLog();
					doc["footer"]=(function(page, pages) {
						return {
							columns: [
								"",
								{
									// This is the right column
									alignment: "right",
									text: ["page ", { text: page.toString() },  " of ", { text: pages.toString() }]
								}
							],
							margin: [50,0] // [left or right , up or down]
						}
					});
				}
			},
			{
				extend: "print",
				text: '<i class="fa fa-file-powerpoint-o " style=color:green > VIEW</i>',
				title: "CUSTOMIZED PROFIT REPORT",
				messageTop: "MESSAGE TOP",
				messageBottom: null,
				footer: true,
				autoPrint: false,
				exportOptions: {
					stripHtml: false,
					stripNewlines: false
				},
				customize: function (win) {
					$(win.document.body).find("table").addClass("display").css("font-size", "12px");
					/*$(win.document.body).find('tr:nth-child(odd) td').each(function(index){
                        $(this).css('background-color','WHITESMOKE');
                    });
                    $(win.document.body).find('tr:nth-child(even) td').each(function(index){
                        $(this).css('background-color','WHITE');
                    });*/

					$(win.document.body).css("background-color","WHITE");
					$(win.document.body).css("color", "DIMGRAY");
					$(win.document.body).find("h1").css("text-align", "center");
					$(win.document.body).find("div:first").css("text-align", "center");

					$(win.document.body).find("tr").each(function() {
						if ($(this).find("td:eq(1)").text().trim().length === 2){
							$(this).find("td").each(function(index, td){
								$(td).css("color", "WHITE");
								$(td).css("background-color", "BLACK");
							});
						}
						if ($(this).find("td:eq(1)").text().trim().length === 5){
							$(this).find("td").each(function(index, td){
								$(td).css("color", "WHITE");
								$(td).css("background-color", "DIMGRAY");
							});
						}
						if ($(this).find("td:eq(1)").text().trim().length === 9){
							$(this).find("td").each(function(index, td){
								$(td).css("color", "WHITE");
								$(td).css("background-color", "DARKGRAY");
							});
						}
					});
					buttonName ="VIEW";
					userLog();
					doc["footer"]=(function(page, pages) {
						return {
							columns: [
								"",
								{
									// This is the right column
									alignment: "right",
									text: ["page ", { text: page.toString() },  " of ", { text: pages.toString() }]
								}
							],
							margin: [10,0]// [left or right , up or down]
						}
					});
				}
			}
		]
	});


	$("#ex_report_datatable").DataTable().clear();
	$("#ex_report_datatable").DataTable().destroy();
	var table2 = $("#ex_report_datatable").DataTable({
		dom: "Bfrtip",
		"bPaginate": false,
		bFilter: false,
		bInfo: false,
		buttons: []
	});

	function loadFromAndToAccounts() {

		if (!$("#level").val()){
			return;
		}

		$.get( "/reports/load_expense_Accounts?level=" + $("#level").val(), function( data ) {

			$("#fromAccountCode").empty();
			//$("#toAccountCode").empty();

			$.each(data, function(i, record){
				var option = "<option value = " + record.code + ">" + record.formattedCode + "&emsp;" + record.accountName +  "</option>";
				$("#fromAccountCode").append(option);
				//$("#toAccountCode").append(option);
			});

			$("#fromAccountCode").val(5512101).change();
			//$("#fromAccountCode option:first").attr("selected", "selected");
			//$("#toAccountCode option:last").attr("selected", "selected");

		});
	}

	function loadCompanyBranches() {

		if (!$("#companyIds").val() || $("#companyIds").val() == 0){
			return;
		}

		$.get( "/reports/companies_branches?companyIds=" + $("#companyIds").val(), function( data ) {
			$("#branchIds").empty();
			$("#branchIds").append("<option value='0'>ALL</option>");

			for (var i = 0, len = data.length; i < len; i++) {
				var option = "<option value = " + data[i].id + ">" + data[i].fullName +  "</option>";
				$("#branchIds").append(option);
			}
		});
	}

	function restrictFromAndToDate(){

		if (!$("#financialYearId").val()){
			return;
		}

		$("#fromDate").attr("min", $("#financialYearId option:selected").attr("data-value").split("|")[0]);
		$("#fromDate").attr("max", $("#financialYearId option:selected").attr("data-value").split("|")[1]);
		$("#fromDate").val($("#financialYearId option:selected").attr("data-value").split("|")[0]);

		$("#toDate").attr("min", $("#financialYearId option:selected").attr("data-value").split("|")[0]);
		$("#toDate").attr("max", $("#financialYearId option:selected").attr("data-value").split("|")[1]);
		$("#toDate").val($("#financialYearId option:selected").attr("data-value").split("|")[1]);

	}

	function getFormattedDate(date) {
		var year = date.getFullYear();
		var month = (1 + date.getMonth()).toString();
		month = month.length > 1 ? month : "0" + month;
		var day = date.getDate().toString();
		day = day.length > 1 ? day : "0" + day;
		return day + "/" + month + "/" + year;
	}

	$("#submitTBForm").on("click",function(){
		//alert("I m the best");
		$("#cpReportForm").submit();
	});


	$("#exportExcel").on("click",function(){
		//alert("I m the best");
		getExcelBuilder();
	});

	// SUBMIT FORM
	$("#cpReportForm").submit(function(event) {

		$("#tb_report_datatable").DataTable().clear();
		// Prevent the form from submitting via the browser.
		buttonName = $("#submitTBForm").text();
		userLog();
		event.preventDefault();
		generateTBReport();
	});

	function generateTBReport(){

		// disable submit button
		$("#submitTBForm").attr("disabled", true);
		$("#bankSummaryLLoading").show();
		// calculate all Totals
		//calculateTotal();

		if (!$("#companyIds").val() || !$("#level").val() || !$("#financialYearId").val() || !$("#fromDate").val() || !$("#toDate").val()){
			alert("PLEASE SELECT ALL THE FIELDS");
			return;
		}

		//var radios =document.querySelectorAll('input[type="radio"]');
		//console.log("aa  "+radios[0].checked);

		// PREPARE FORM DATA
		var formData = {
			companyIds : $("#companyIds").val(),
			branchIds : $("#branchIds").val(),
			level : $("#level").val(),
			fromAccountCode : 0,
			toAccountCode : 0,
			voucherStatusId : "0",
			financialYearId : $("#financialYearId").val(),
			fromDate : $("#fromDate").val(),
			toDate : $("#toDate").val(),
			withZeroBalance: 0
		};

		//
		// $("select#fromAccountCode").children('option:selected').each( function() {
		// 	var $this = $(this);
		// 	console.log( "text:" + $this.text() + "value: " + $this.val());
		// });

		$("#ex_report_datatable").DataTable().clear();
		// $("#ex_report_datatable").DataTable().destroy();
		// table2 = $("#ex_report_datatable").DataTable({
		// 	dom: "Bfrtip",
		// 	"bPaginate": false,
		// 	bFilter: false,
		// 	bInfo: false,
		// 	buttons: []
		// });


		var sumRevenue = parseFloat(0);
		var sumExpense = parseFloat(0);

		var excludeRevenue = parseFloat(0);
		var excludeExpense = parseFloat(0);
		// DO POST
		$.ajax({
			type : "POST",
			contentType : "application/json",
			url : $("#cpReportForm").attr("action"),
			data : JSON.stringify(formData),
			dataType : "json",
			success:function(data){
				//console.log(data);
				 var formatter = new Intl.NumberFormat("ur-PK", {minimumFractionDigits: 2, maximumFractionDigits: 2});
				// // FILL TABLE ROWS
				 var rowIndex = 0;
				 var row2Index = 0;

				 var revenueRowID = 0;
				 var expenseRowID = 0;

				var revenueTotal = 0;
				var expenseTotal = 0;

				var selected = [];
				$("#fromAccountCode").children("option:selected").each( function() {
					var $this = $(this);
					//selected.push( { text: $this.text(), value: $this.val() });
					selected.push($this.val());
				});
				console.log("Array is " + selected + " Array Length :" + selected.length);
				 $.each(data, function (i, tBEntry) {
				 	rowIndex = rowIndex + 1;
					 //debit credit debitCreditBalance
					 //console.log("My Account is " + tBEntry.accountCode);
					if(tBEntry.accountCode === 44){
						sumRevenue = tBEntry.debitCreditBalance;
						revenueRowID = rowIndex;
						revenueTotal = tBEntry.debitCreditBalance;
						//console.log(" REVENUE " + parseFloat(sumRevenue));
					}
					if(tBEntry.accountCode === 55){
						sumExpense = tBEntry.debitCreditBalance;
						expenseRowID = rowIndex;
						expenseTotal = tBEntry.debitCreditBalance;
						//console.log( " EXPENSE " + parseFloat(sumExpense));
					}
					table.row.add([
						rowIndex,
						tBEntry.accountName,
						"",
						"",
						"",
						formatter.format(tBEntry.debitCreditBalance),
						""
					]);

					 if ($("#level").val() > 1){
						 $.each(tBEntry.children, function (j, secondLevel) {
							 //console.log("----- Second Code " + secondLevel.accountCode + " Second Level :" + findValueInArray(secondLevel.accountCode, selected));
							 if(selected.length > 0) {
								 var level2 = secondLevel.accountFormattedCode;
								 if(findValueInArray(secondLevel.accountCode, selected) == 0){
									 row2Index = row2Index + 1;
									 console.log("LEVEL 2 START CODE: " + level2.substring(0,2));
									 if(parseInt(level2.substring(0,2))  == 44){
										 excludeRevenue = excludeRevenue + secondLevel.debitCreditBalance;
									 }

									 if(parseInt(level2.substring(0,2))  == 55){
										 excludeExpense = excludeExpense + secondLevel.debitCreditBalance;
									 }
									 table2.row.add([
										 row2Index,
										 "",
										 secondLevel.accountFormattedCode,
										 secondLevel.accountName,
										 formatter.format(secondLevel.debitCreditBalance),
										 "",
										 ""
									 ]);
								 } else {
									 rowIndex = rowIndex + 1;
									 table.row.add([
										 rowIndex,
										 "",
										 secondLevel.accountFormattedCode,
										 secondLevel.accountName,
										 formatter.format(secondLevel.debitCreditBalance),
										 "",
										 ""
									 ]);
								 }
							 } else {
								 rowIndex = rowIndex + 1;
								 table.row.add([
									 rowIndex,
									 "",
									 secondLevel.accountFormattedCode,
									 secondLevel.accountName,
									 formatter.format(secondLevel.debitCreditBalance),
									 "",
									 ""
								 ]);
							 }

							 if ($("#level").val() > 2){
								 $.each(secondLevel.children, function (k, thirdLevel) {
									 //console.log("---------- Third Code " + thirdLevel.accountCode + " Third Level :" + findValueInArray(thirdLevel.accountCode, selected));
									 if(selected.length > 0) {
										 var level3 = thirdLevel.accountFormattedCode;
										 if(findValueInArray(thirdLevel.accountCode, selected) == 0){
											 row2Index = row2Index + 1;
											 console.log("LEVEL 3 START CODE: " + level3.substring(0,2));
											 if(parseInt(level3.substring(0,2))  == 44){
												 excludeRevenue = excludeRevenue + thirdLevel.debitCreditBalance;
											 }

											 if(parseInt(level3.substring(0,2))  == 55){
												 excludeExpense = excludeExpense + thirdLevel.debitCreditBalance;
											 }
											 table2.row.add([
												 row2Index,
												 "",
												 thirdLevel.accountFormattedCode,
												 thirdLevel.accountName,
												 formatter.format(thirdLevel.debitCreditBalance),
												 "",
												 ""
											 ]);
										 } else {
											 rowIndex = rowIndex + 1;
											 table.row.add([
												 rowIndex,
												 "",
												 thirdLevel.accountFormattedCode,
												 thirdLevel.accountName,
												 formatter.format(thirdLevel.debitCreditBalance),
												 "",
												 ""
											 ]);
										 }

									 } else {
										 rowIndex = rowIndex + 1;
										 table.row.add([
											 rowIndex,
											 "",
											 thirdLevel.accountFormattedCode,
											 thirdLevel.accountName,
											 formatter.format(thirdLevel.debitCreditBalance),
											 "",
											 ""
										 ]);
									 }

									 if ($("#level").val() > 3){
										 $.each(thirdLevel.children, function (l, fourthLevel) {
											 //console.log("--------------- Fourth Code " + fourthLevel.accountCode + " Fourth Level :" + findValueInArray(fourthLevel.accountCode, selected));
											 if(selected.length > 0) {
												 var level4 = thirdLevel.accountFormattedCode;

												 if(findValueInArray(fourthLevel.accountCode, selected) == 0){
													 row2Index = row2Index + 1;
													 //console.log("LEVEL 4 START CODE: " + level4.substring(0,2));

													 if(parseInt(level4.substring(0,2))  == 44){
														 excludeRevenue = excludeRevenue + fourthLevel.debitCreditBalance;
													 }

													 if(parseInt(level4.substring(0,2))  == 55){
														 excludeExpense = excludeExpense + fourthLevel.debitCreditBalance;
													 }

													 table2.row.add([
														 row2Index,
														 "",
														 fourthLevel.accountFormattedCode,
														 fourthLevel.accountName,
														 formatter.format(fourthLevel.debitCreditBalance),
														 "",
														 ""
													 ]);
												 } else {
													 rowIndex = rowIndex + 1;
													 table.row.add([
														 rowIndex,
														 "",
														 fourthLevel.accountFormattedCode,
														 fourthLevel.accountName,
														 formatter.format(fourthLevel.debitCreditBalance),
														 "",
														 ""
													 ]);
												 }
											 } else {
												 rowIndex = rowIndex + 1;
												 table.row.add([
													 rowIndex,
													 "",
													 fourthLevel.accountFormattedCode,
													 fourthLevel.accountName,
													 formatter.format(fourthLevel.debitCreditBalance),
													 "",
													 ""
												 ]);
											 }
										 });
									 }

								 });
							 }

						 });
					 }

				 });
				//
				table.draw();
				table2.draw();
				var netProfit = parseFloat(sumRevenue) + parseFloat(sumExpense);

				var netTakeSubTotal =  (parseFloat(excludeExpense) + parseFloat(excludeRevenue));
				var netTakeHome = parseFloat(netProfit) - (parseFloat(netTakeSubTotal));
				//console.log("Revenue Sum " + parseFloat(excludeExpense) + " Expense Sum " + parseFloat(excludeRevenue) + " SUM " + netTakeSubTotal);
				revenueRowID = revenueRowID - 1;
				expenseRowID = expenseRowID - 1;
				//var myRevenue = $("#tb_report_datatable tbody").find("tr:eq(" + revenueRowID + ") td:eq(5)").html();
				//var myExpense = $("#tb_report_datatable tbody").find("tr:eq(" + expenseRowID + ") td:eq(5)").html();

				console.log("Revenue " + revenueRowID + " is " + revenueTotal + " expense " +  expenseRowID + " is " + expenseTotal);
				var myRevenue = parseFloat(revenueTotal) - parseFloat(excludeRevenue);
				var myExpense = parseFloat(expenseTotal) - parseFloat(excludeExpense);

				$("#tb_report_datatable tbody").find("tr:eq(" + revenueRowID + ") td:eq(5)").html(formatter.format(myRevenue));
				$("#tb_report_datatable tbody").find("tr:eq(" + expenseRowID + ") td:eq(5)").html(formatter.format(myExpense));

				$("#tb_report_datatable span#netProfitTotal").text(formatter.format(netProfit));
				$("#ex_report_datatable span#netTakeSubTotal").text(formatter.format(netTakeSubTotal));
				$("#ex_report_datatable span#netTakeHomeTotal").text(formatter.format(netTakeHome));
				//
				// // calculate all Totals
				// calculateTotal();
				$("#bankSummaryLLoading").hide();
				// enable submit button
				$("#submitTBForm").attr("disabled", false);
			},
			complete:function(){
				// enable submit button
				$("#bankSummaryLLoading").hide();
				$("#submitTBForm").attr("disabled", false);
			}
		});
		// $("#bankSummaryLLoading").hide();
		// $("#submitTBForm").attr("disabled", false);

	}

	function getExcelBuilder(){
		//alert("i m excel");
		//return;

		if (!$("#companyIds").val() || !$("#level").val() || !$("#financialYearId").val() || !$("#fromDate").val() || !$("#toDate").val()){
			alert("PLEASE SELECT ALL THE FIELDS");
			return;
		}

		var myObject = [];

		$("#tb_report_datatable tbody tr").each(function() {
			var column0 = $(this).find("td:eq(0)").text();
			var column1 = $(this).find("td:eq(1)").text();
			var column2 = $(this).find("td:eq(2)").text();
			var column3 = $(this).find("td:eq(3)").text();
			var column4 = $(this).find("td:eq(4)").text();
			var column5 = $(this).find("td:eq(5)").text();
			var column6 = $(this).find("td:eq(6)").text();
			//console.log("Table Values Are " + column0 + " 1: " + column1 + " 2: " + column2 + " 3: " + column3 + " 4: " + column4 + " 5: " + column5 + " 6: " + column6);

			tmp = {
				"column0": column0,
				"column1": column1,
				"column2": column2,
				"column3": column3,
				"column4": column4,
				"column5": column5,
				"column6": column6

			};
			myObject.push(tmp);

		});

		console.log(myObject);

		 location.replace("/reports/download/customize_profit_report/?recordList=" + JSON.stringify(myObject) + "");
		// location.replace("/reports/download/customize_profit_report/?companyIds=" + $("#companyIds").val() + "&branchIds=" + $("#branchIds").val() + "&level=" + $("#level").val() + "&fromAccountCode=0&financialYearId="+$("#financialYearId").val() +"&fromDate=" + $("#fromDate").val() + "&toDate=" + $("#toDate").val() +"");
		// companyIds=" + $("#companyIds").val() + "&branchIds=" + $("#branchIds").val() + "&level=" + $("#level").val() + "&fromAccountCode=0&financialYearId=$("#financialYearId").val(),fromDate=$("#fromDate").val()&toDate=$("#toDate").val()
		return;

		// PREPARE FORM DATA
		var formData = {
			companyIds : $("#companyIds").val(),
			branchIds : $("#branchIds").val(),
			level : $("#level").val(),
			fromAccountCode : 0,
			toAccountCode : 0,
			voucherStatusId : "0",
			financialYearId : $("#financialYearId").val(),
			fromDate : $("#fromDate").val(),
			toDate : $("#toDate").val(),
			withZeroBalance: 0
		};

		/*$.ajax({
			type : "POST",
			contentType : "application/json",
			url : "/reports/download/customize_profit_report",
			data : JSON.stringify(formData),
			dataType : "json",
			success:function(){
				//
			},
			complete:function(){
				//
			}
		});*/
	}
	function oldgetExcelBuilder(){
		var xlsBuilder = {
			filename : "CPR_" + new Date().toLocaleString(),
			sheetName : "sheet1",
			customize : function(xlsx) {
				var sheet = xlsx.xl.worksheets["sheet1.xml"];
				var downrows = 8;
				var clRow = $("row", sheet);
				var msg;
				// update Row
				clRow.each(function() {
					var attr = $(this).attr("r");
					var ind = parseInt(attr);
					ind = ind + downrows;
					$(this).attr("r", ind);
				});

				// Update row > c
				$("row c ", sheet).each(
					function() {
						var attr = $(this).attr("r");
						var pre = attr.substring(0, 1);
						var ind = parseInt(attr.substring(
							1, attr.length));
						ind = ind + downrows;
						$(this).attr("r", pre + ind);
					});

				function Addrow(index, data) {

					msg = '<row xmlns="http://schemas.openxmlformats.org/spreadsheetml/2006/main" r="'
						+ index + '">';
					for (var i = 0; i < data.length; i++) {
						var key = data[i].k;
						var value = data[i].v;
						msg += '<c t="inlineStr" r="' + key
							+ index + '">';
						msg += '<is>';
						msg += '<t>' + value + '</t>';
						msg += '</is>';
						msg += '</c>';
					}
					msg += '</row>';
					return msg;
				}
				var r1 = Addrow(1, [ {
					k : "A",
					v : "TRIAL BALANCE" + " ("
						+ getFormattedDate(new Date()) + ", " + new Date().toLocaleString().split(",")[1].trim()
						+ ")"
				} ]);
				var r2 = Addrow(2, [ {
					k : "A",
					v : "COMPANY :"
				}, {
					k : "B",
					v : $('#companyIds option:selected').toArray().map(item => item.text).join(", ")
				} ]);
				var r3 = Addrow(3, [ {
					k : "A",
					v : "BRANCH :"
				}, {
					k : "B",
					v : $('#branchIds option:selected').toArray().map(item => item.text).join(", ")
				} ]);
				var r4 = Addrow(4, [ {
					k : "A",
					v : "VOUCHER STATUS :"
				}, {
					k : "B",
					v : $("#voucherStatusId option:selected").text()
				} ]);
				var r5 = Addrow(5, [ {
					k : "A",
					v : "FINANCIAL YEAR :"
				}, {
					k : "B",
					v : $("#financialYearId option:selected").text() + " (" + getFormattedDate(new Date($("#fromDate").val())) + " - " + getFormattedDate(new Date($("#toDate").val())) + ")"
				} ]);
				var r6 = Addrow(6, [ {
					k : "A",
					v : "FROM ACCOUNT :"
				}, {
					k : "B",
					v : $("#fromAccountCode option:selected").text(),
				} ]);
				var r7 = Addrow(7, [ {
					k : "A",
					v : "TO ACCOUNT :"
				}, {
					k : 'B',
					v : $("#toAccountCode option:selected").text(),
				} ]);

				sheet.childNodes[0].childNodes[1].innerHTML = r1
					+ r2
					+ r3
					+ r4
					+ r5
					+ r6
					+ r7
					+ sheet.childNodes[0].childNodes[1].innerHTML;

				// Loop over the cells in column `B`
				$('row', sheet).each( function (index, val) {
					// Get the value
					if(index > 7){
						if($(val).find('c[r^="B"]').text().trim().length == 2){
							$(val).find('c').attr( 's', '5');
						}
						if($(val).find('c[r^="B"]').text().trim().length == 5){
							$(val).find('c').attr( 's', '10');
						}
						if($(val).find('c[r^="B"]').text().trim().length == 9){
							$(val).find('c').attr( 's', '15');
						}
						if($(val).find('c[r^="B"]').text().trim().length == 14){
							$(val).find('c').attr( 's', '20');
						}
					}
				});
			},
			/*
             * exportOptions: { columns: [0, 1, 2, 3] }
             */
		}
		return xlsBuilder;
	}

	function findValueInArray(value,arr){
		var result = -1;

		for(var i=0; i<arr.length; i++){
			var name = arr[i];
			if(name == value){
				result = 0;
				break;
			}
		}
		return result;
	}

	function userLog() {

		var userLogObj = {
			idNumber : "",
			code: "",
			buttonClick: buttonName,
			windowName: $("h2").html()
		}

		$.ajax({
			type : "POST",
			contentType : "application/json",
			url : "/viewSaleOrder",
			data : JSON.stringify(userLogObj),
			dataType : "json",
			success:function(data){
				successmessage = "Data was successfully captured";
				//	location.reload();
				//  $("#customerAccount\\.balanceLimit").text(successmessage);
			},
			error: function(data) {
				successmessage = "Error";
				//  $("#customerAccount\\.balanceLimit").text(successmessage);
			},
		});
	}

});