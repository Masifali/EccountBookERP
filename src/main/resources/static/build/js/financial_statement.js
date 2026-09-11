$(document).ready(function() {
	
	//on load
	restrictFromAndToDate();
	//loadFromAndToAccounts();
	var buttionNane="";
	console.log("a");
	$("#companyIds").on("change", function () {
		loadCompanyBranches();
    });
	
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
	
	$("#financialYearId").on("change", function () {
		restrictFromAndToDate();
    });
	
	function restrictFromAndToDate(){
		
		if (!$("#financialYearId").val()){
			return;
		}
		
		// $("#fromDate").attr("min", $("#financialYearId option:selected").attr("data-value").split("|")[0]);
		// $("#fromDate").attr("max", $("#financialYearId option:selected").attr("data-value").split("|")[1]);
		// $("#fromDate").val($("#financialYearId option:selected").attr("data-value").split("|")[0]);
		//
		$("#toDate").attr("min", $("#financialYearId option:selected").attr("data-value").split("|")[0]);
		$("#toDate").attr("max", $("#financialYearId option:selected").attr("data-value").split("|")[1]);
		$("#toDate").val($("#financialYearId option:selected").attr("data-value").split("|")[1]);
		
	}


	
	function calculateTotal(){
		
		$("#tb_report_datatable tr").each(function() {
	    	if ($(this).find("td").length) {
	    		// SET COLORS
	    		if ($(this).find("span").length) {
		    		$(this).css("color", "WHITE");
		    		$(this).css("font-weight","bold");
		    		$(this).css("background-color", "DIMGRAY");
		    	}
	    	}
	    });
		
		/*var openingBalanceTotal = parseFloat(0);
		var jul = parseFloat(0);
		var aug = parseFloat(0);
		var sep = parseFloat(0);
		var oct = parseFloat(0);
		var nov = parseFloat(0);
		var dec = parseFloat(0);
		var jan = parseFloat(0);
		var feb = parseFloat(0);
		var mar = parseFloat(0);
		var apr = parseFloat(0);
		var may = parseFloat(0);
		var jun = parseFloat(0);
		var closingBalanceTotal = parseFloat(0);
		
		var formatter = new Intl.NumberFormat('ur-PK', {minimumFractionDigits: 2, maximumFractionDigits: 2});
		
		$("#tb_report_datatable tr").each(function() {
	    	if ($(this).find("td").length) {
	    		
	    		// SET COLORS
	    		if ($(this).find("td:eq(16)").text().trim() === "true") {
		    		$(this).css("color", "WHITE");
		    		$(this).css("font-weight","bold");
		    	}
		    	if ($(this).find("td:eq(1)").text().trim().length == 5) {
		    		$(this).css("color", "WHITE");
		    		$(this).css("background-color", "DIMGRAY");
		    	}
		    	if ($(this).find("td:eq(1)").text().trim().length == 9) {
		    		//$(this).css("color", "WHITE");
		    		$(this).css("background-color", "WHITE");
		    	}
	    		
	    		if ($(this).find("td:eq(1)").text().trim().length == 2){
		    		openingBalanceTotal = openingBalanceTotal + parseFloat($(this).find("td:eq(3)").text().replace(/[^0-9\.-]+/g, "") || 0);
		    		jul = jul + parseFloat($(this).find("td:eq(4)").text().replace(/[^0-9\.-]+/g, "") || 0);
			    	aug = aug + parseFloat($(this).find("td:eq(5)").text().replace(/[^0-9\.-]+/g, "") || 0);
			    	sep = sep + parseFloat($(this).find("td:eq(6)").text().replace(/[^0-9\.-]+/g, "") || 0);
			    	oct = oct + parseFloat($(this).find("td:eq(7)").text().replace(/[^0-9\.-]+/g, "") || 0);
			    	nov = nov + parseFloat($(this).find("td:eq(8)").text().replace(/[^0-9\.-]+/g, "") || 0);
			    	dec = dec + parseFloat($(this).find("td:eq(9)").text().replace(/[^0-9\.-]+/g, "") || 0);
			    	jan = jan + parseFloat($(this).find("td:eq(10)").text().replace(/[^0-9\.-]+/g, "") || 0);
			    	feb = feb + parseFloat($(this).find("td:eq(11)").text().replace(/[^0-9\.-]+/g, "") || 0);
			    	mar = mar + parseFloat($(this).find("td:eq(12)").text().replace(/[^0-9\.-]+/g, "") || 0);
			    	apr = apr + parseFloat($(this).find("td:eq(13)").text().replace(/[^0-9\.-]+/g, "") || 0);
			    	may = may + parseFloat($(this).find("td:eq(14)").text().replace(/[^0-9\.-]+/g, "") || 0);
			    	jun = jun + parseFloat($(this).find("td:eq(15)").text().replace(/[^0-9\.-]+/g, "") || 0);
			    	closingBalanceTotal = closingBalanceTotal + parseFloat($(this).find("td:eq(16)").text().replace(/[^0-9\.-]+/g, "") || 0);
	    		}
	    	}
	    });
		
		$("#tb_report_datatable tfoot tr:eq(0) th:eq(3)").text(formatter.format(openingBalanceTotal));
		$("#tb_report_datatable tfoot tr:eq(0) th:eq(4)").text(formatter.format(jul));
		$("#tb_report_datatable tfoot tr:eq(0) th:eq(5)").text(formatter.format(aug));
		$("#tb_report_datatable tfoot tr:eq(0) th:eq(6)").text(formatter.format(sep));
		$("#tb_report_datatable tfoot tr:eq(0) th:eq(7)").text(formatter.format(oct));
		$("#tb_report_datatable tfoot tr:eq(0) th:eq(8)").text(formatter.format(nov));
		$("#tb_report_datatable tfoot tr:eq(0) th:eq(9)").text(formatter.format(dec));
		$("#tb_report_datatable tfoot tr:eq(0) th:eq(10)").text(formatter.format(jan));
		$("#tb_report_datatable tfoot tr:eq(0) th:eq(11)").text(formatter.format(feb));
		$("#tb_report_datatable tfoot tr:eq(0) th:eq(12)").text(formatter.format(mar));
		$("#tb_report_datatable tfoot tr:eq(0) th:eq(13)").text(formatter.format(apr));
		$("#tb_report_datatable tfoot tr:eq(0) th:eq(14)").text(formatter.format(may));
		$("#tb_report_datatable tfoot tr:eq(0) th:eq(15)").text(formatter.format(jun));
		$("#tb_report_datatable tfoot tr:eq(0) th:eq(16)").text(formatter.format(closingBalanceTotal));*/
	}

	// Saleem Added Function
	function addCommas(nStr) {
		nStr += '';
		x = nStr.split('.');
		x1 = x[0];
		x2 = x.length > 1 ? '.' + x[1] : '';
		var rgx = /(\d+)(\d{3})/;
		while (rgx.test(x1)) {
			x1 = x1.replace(rgx, '$1,$2');
		}
		return x1 + x2;
	}

	function formatNum(value) {
		value = parseFloat(value);
		var output = addCommas(value.toFixed(2));
		return value < 0 ? '(' + output.replace('-', '') + ')' : output;
	}
	// Saleem Add Number function
	
	function getFormattedDate(date) {
		var year = date.getFullYear();
		var month = (1 + date.getMonth()).toString();
		month = month.length > 1 ? month : '0' + month;
		var day = date.getDate().toString();
		day = day.length > 1 ? day : '0' + day;
		return day + '/' + month + '/' + year;
	}
	
	// SUBMIT FORM
    $("#tbReportForm").submit(function(event) {
		// Prevent the form from submitting via the browser.
		//console.log('Form Submission Start');
    	buttionName =  $("#submitTBForm").text();
    	userLog();
		event.preventDefault();
		//console.log('Form Submission Generate Report');
		generateTBReport();
	});
        
    function generateTBReport(){
		//console.log('Report Generation Start');
    	// disable submit button 
    	$("#submitTBForm").attr("disabled", true);
    	
    	// calculate all Totals
        //calculateTotal();
		//console.log('Report Generation Values Check');
		//!$("#fromDate").val() ||
    	if (!$("#companyIds").val() || !$("#voucherStatusId").val() || !$("#financialYearId").val() ||  !$("#toDate").val()){
			alert("PLEASE SELECT ALL THE FIELDS")
    		return;
		}

		$("span#currentTitle").text('');

		$("span#currentYear").text('');
		$("span#lastYear").text('');

		$("span#currentProperty").text('');
		$("span#lastProperty").text('');

		$("span#currentFurniture").text('');
		$("span#lastFurniture").text('');

		$("span#currentVehicles").text('');
		$("span#lastVehicles").text('');

		$("span#currentCapitalWork").text('');
		$("span#lastCapitalWork").text('');

		$("span#currentIt").text('');
		$("span#lastIt").text('');

		$("span#currentIntangibleAssets").text('');
		$("span#lastIntangibleAssets").text('');

		$("span#currentInvestments").text('');
		$("span#lastInvestments").text('');

		$("span#currentLongTermAdvanceDeposits").text('');
		$("span#lastLongTermAdvanceDeposits").text('');

		$("span#currentTotalOne").text('');
		$("span#lastTotalOne").text('');

		$("span#currentStoreSpares").text('');
		$("span#lastStoreSpares").text('');

		$("span#currentTradeReceivable").text('');
		$("span#lastTradeReceivable").text('');

		$("span#currentOtherReceivable").text('');
		$("span#lastOtherReceivable").text('');

		$("span#currentAdvancesToEmployees").text('');
		$("span#lastAdvancesToEmployees").text('');

		$("span#currentRefundsDue").text('');
		$("span#lastRefundsDue").text('');

		$("span#currentCashInHand").text('');
		$("span#lastCashInHand").text('');

		$("span#currentCashInBank").text('');
		$("span#lastCashInBank").text('');

		$("span#currentOtherAdvances").text('');
		$("span#lastOtherAdvances").text('');

		$("span#currentAdvancesToOtherParties").text('');
		$("span#lastAdvancesToOtherParties").text('');

		$("span#currentSecurityDeposits").text('');
		$("span#lastSecurityDeposits").text('');

		$("span#currentPrepayments").text('');
		$("span#lastPrepayments").text('');

		$("span#currentTotalAssets").text('');
		$("span#lastTotalAssets").text('');

		$("span#currentCapital").text('');
		$("span#lastCapital").text('');

		$("span#currentReserves").text('');
		$("span#lastReserves").text('');

		$("span#currentTotalTwo").text('');
		$("span#lastTotalTwo").text('');

		$("span#currentTotalThree").text('');
		$("span#lastTotalThree").text('');

		$("span#currentLoanFromDirectors").text('');
		$("span#lastLoanFromDirectors").text('');

		$("span#currentBankLoans").text('');
		$("span#lastBankLoans").text('');

		$("span#currentTotalFour").text('');
		$("span#lastTotalFour").text('');

		$("span#currentTradePayables").text('');
		$("span#lastTradePayables").text('');

		$("span#currentOtherPayables").text('');
		$("span#lastOtherPayables").text('');

		$("span#currentShortTermBorrowings").text('');
		$("span#lastShortTermBorrowings").text('');

		$("span#currentGovtLeviesDutiesPayable").text('');
		$("span#lastGovtLeviesDutiesPayable").text('');

		$("span#currentSalariesWagesPayable").text('');
		$("span#lastSalariesWagesPayable").text('');

		$("span#currentTotalFive").text('');
		$("span#lastTotalFive").text('');

		$("span#currentTotalEquity").text('');
		$("span#lastTotalEquity").text('');

    	// PREPARE FORM DATA    
    	var formData = {
    		companyIds : $("#companyIds").val(),
    		branchIds : $("#branchIds").val(),
    		level : $("#level").val(),
    		fromAccountCode : $("#fromAccountCode").val(),
    		toAccountCode : $("#toAccountCode").val(),
    		voucherStatusId : $("#voucherStatusId").val(),
    		financialYearId : $("#financialYearId").val(),
    		//fromDate : $("#fromDate").val(),
    		toDate : $("#toDate").val(),
    	}
		//console.log('Report Generation Form Data End Start Ajax Call');
    	// DO POST
    	$.ajax({
			type : "POST",
			contentType : "application/json",
			url : $("#tbReportForm").attr("action"),
			data : JSON.stringify(formData),
			dataType : "json",
			success:function(data){
				//console.log('Report Generation Ajax Call Start');
				var formatter = new Intl.NumberFormat('ur-PK', {style: 'currency', currency: 'PKR', currencySign: 'accounting', signDisplay: 'never', minimumFractionDigits: 2, maximumFractionDigits: 4});
				// FILL TABLE ROWS
				var rowIndex = 0;
				$.each(data, function (i, tBEntry) {

					$("span#currentTitle").text('AS ON DATE ' + tBEntry.currentTitle);

					$("span#currentYear").text(tBEntry.currentYear);
					$("span#lastYear").text(tBEntry.lastYear);
					// var currentValue = 0;
					// currentValue = tBEntry.currentProperty;
					// if(currentValue < 0 ){
					// 	currentValue = currentValue * -1;
					// 	currentValue = currentValue / 1000;
					// } else {
					// 	currentValue = currentValue / 1000;
					// }
					$("span#currentProperty").text(formatNum(tBEntry.currentProperty));

					$("span#lastProperty").text(formatNum(tBEntry.lastProperty));

					$("span#currentFurniture").text(formatNum(tBEntry.currentFurniture));
					$("span#lastFurniture").text(formatNum(tBEntry.lastFurniture));

					$("span#currentVehicles").text(formatNum(tBEntry.currentVehicles));
					$("span#lastVehicles").text(formatNum(tBEntry.lastVehicles));

					$("span#currentCapitalWork").text(formatNum(tBEntry.currentCapitalWork));
					$("span#lastCapitalWork").text(formatNum(tBEntry.lastCapitalWork));

					$("span#currentIt").text(formatNum(tBEntry.currentIt));
					$("span#lastIt").text(formatNum(tBEntry.lastIt));

					$("span#currentIntangibleAssets").text(formatNum(tBEntry.currentIntangibleAssets));
					$("span#lastIntangibleAssets").text(formatNum(tBEntry.lastIntangibleAssets));

					$("span#currentInvestments").text(formatNum(tBEntry.currentInvestments));
					$("span#lastInvestments").text(formatNum(tBEntry.lastInvestments));

					$("span#currentLongTermAdvanceDeposits").text(formatNum(tBEntry.currentLongTermAdvanceDeposits));
					$("span#lastLongTermAdvanceDeposits").text(formatNum(tBEntry.lastLongTermAdvanceDeposits));

					/*var TotalOne = 0;
					TotalOne = TotalOne + tBEntry.currentProperty + tBEntry.currentFurniture + tBEntry.currentVehicles + tBEntry.currentCapitalWork + tBEntry.currentIt;
					TotalOne = TotalOne + tBEntry.currentIntangibleAssets + tBEntry.currentInvestments + tBEntry.currentLongTermAdvanceDeposits
					console.log('DB ' + tBEntry.currentTotalOne + ' My Cal ' + TotalOne);///*/

					$("span#currentTotalOne").text(formatNum(tBEntry.currentTotalOne));
					$("span#lastTotalOne").text(formatNum(tBEntry.lastTotalOne));

					$("span#currentStoreSpares").text(formatNum(tBEntry.currentStoreSpares));
					$("span#lastStoreSpares").text(formatNum(tBEntry.lastStoreSpares));

					$("span#currentTradeReceivable").text(formatNum(tBEntry.currentTradeReceivable));
					$("span#lastTradeReceivable").text(formatNum(tBEntry.lastTradeReceivable));

					$("span#currentOtherReceivable").text(formatNum(tBEntry.currentOtherReceivable));
					$("span#lastOtherReceivable").text(formatNum(tBEntry.lastOtherReceivable));

					$("span#currentAdvancesToEmployees").text(formatNum(tBEntry.currentAdvancesToEmployees));
					$("span#lastAdvancesToEmployees").text(formatNum(tBEntry.lastAdvancesToEmployees));

					$("span#currentRefundsDue").text(formatNum(tBEntry.currentRefundsDue));
					$("span#lastRefundsDue").text(formatNum(tBEntry.lastRefundsDue));

					$("span#currentCashInHand").text(formatNum(tBEntry.currentCashInHand));
					$("span#lastCashInHand").text(formatNum(tBEntry.lastCashInHand));

					$("span#currentCashInBank").text(formatNum(tBEntry.currentCashInBank));
					$("span#lastCashInBank").text(formatNum(tBEntry.lastCashInBank));

					$("span#currentOtherAdvances").text(formatNum(tBEntry.currentOtherAdvances));
					$("span#lastOtherAdvances").text(formatNum(tBEntry.lastOtherAdvances));

					$("span#currentAdvancesToOtherParties").text(formatNum(tBEntry.currentAdvancesToOtherParties));
					$("span#lastAdvancesToOtherParties").text(formatNum(tBEntry.lastAdvancesToOtherParties));

					$("span#currentSecurityDeposits").text(formatNum(tBEntry.currentSecurityDeposits));
					$("span#lastSecurityDeposits").text(formatNum(tBEntry.lastSecurityDeposits));

					$("span#currentPrepayments").text(formatNum(tBEntry.currentPrepayments));
					$("span#lastPrepayments").text(formatNum(tBEntry.lastPrepayments));

					$("span#currentTotalTwo").text(formatNum(tBEntry.currentTotalTwo));
					$("span#lastTotalTwo").text(formatNum(tBEntry.lastTotalTwo));

					$("span#currentTotalAssets").text(formatNum(tBEntry.currentTotalAssets));
					$("span#lastTotalAssets").text(formatNum(tBEntry.lastTotalAssets));

					$("span#currentCapital").text(formatNum(tBEntry.currentCapital));
					$("span#lastCapital").text(formatNum(tBEntry.lastCapital));

					$("span#currentReserves").text(formatNum(tBEntry.currentReserves));
					$("span#lastReserves").text(formatNum(tBEntry.lastReserves));

					$("span#currentTotalThree").text(formatNum(tBEntry.currentTotalThree));
					$("span#lastTotalThree").text(formatNum(tBEntry.lastTotalThree));

					$("span#currentLoanFromDirectors").text(formatNum(tBEntry.currentLoanFromDirectors));
					$("span#lastLoanFromDirectors").text(formatNum(tBEntry.lastLoanFromDirectors));

					$("span#currentBankLoans").text(formatNum(tBEntry.currentBankLoans));
					$("span#lastBankLoans").text(formatNum(tBEntry.lastBankLoans));

					$("span#currentTotalFour").text(formatNum(tBEntry.currentTotalFour));
					$("span#lastTotalFour").text(formatNum(tBEntry.lastTotalFour));

					$("span#currentTradePayables").text(formatNum(tBEntry.currentTradePayables));
					$("span#lastTradePayables").text(formatNum(tBEntry.lastTradePayables));

					$("span#currentOtherPayables").text(formatNum(tBEntry.currentOtherPayables));
					$("span#lastOtherPayables").text(formatNum(tBEntry.lastOtherPayables));

					$("span#currentShortTermBorrowings").text(formatNum(tBEntry.currentShortTermBorrowings));
					$("span#lastShortTermBorrowings").text(formatNum(tBEntry.lastShortTermBorrowings));

					$("span#currentGovtLeviesDutiesPayable").text(formatNum(tBEntry.currentGovtLeviesDutiesPayable));
					$("span#lastGovtLeviesDutiesPayable").text(formatNum(tBEntry.lastGovtLeviesDutiesPayable));

					$("span#currentSalariesWagesPayable").text(formatNum(tBEntry.currentSalariesWagesPayable));
					$("span#lastSalariesWagesPayable").text(formatNum(tBEntry.lastSalariesWagesPayable));

					$("span#currentTotalFive").text(formatNum(tBEntry.currentTotalFive));
					$("span#lastTotalFive").text(formatNum(tBEntry.lastTotalFive));

					$("span#currentTotalEquity").text(formatNum(tBEntry.currentTotalEquity));
					$("span#lastTotalEquity").text(formatNum(tBEntry.lastTotalEquity));

				});

		        // $.each(data, function (i, tBEntry) {
		        // 	rowIndex = rowIndex + 1;
		    	// 	//$("span#currentSales").val(15000);
		        // 	// if (tBEntry.bold){
		        // 	// 	table.row.add([
			    // 	// 		"<span style='font-weight: bold;'>"+tBEntry.accountName+"</span>",
			    // 	// 		 "<span style='font-weight: bold;'>"+formatter.format(tBEntry.openingBalance)+"</span>",
			    // 	// 		"<span style='font-weight: bold;'>"+formatter.format(tBEntry[7])+"</span>",
			    // 	// 		"<span style='font-weight: bold;'>"+formatter.format(tBEntry[8])+"</span>",
			    // 	// 		"<span style='font-weight: bold;'>"+formatter.format(tBEntry[9])+"</span>",
			    // 	// 		"<span style='font-weight: bold;'>"+formatter.format(tBEntry[10])+"</span>",
			    // 	// 		"<span style='font-weight: bold;'>"+formatter.format(tBEntry[11])+"</span>",
			    // 	// 		"<span style='font-weight: bold;'>"+formatter.format(tBEntry[12])+"</span>",
			    // 	// 		"<span style='font-weight: bold;'>"+formatter.format(tBEntry[1])+"</span>",
			    // 	// 		"<span style='font-weight: bold;'>"+formatter.format(tBEntry[2])+"</span>",
			    // 	// 		"<span style='font-weight: bold;'>"+formatter.format(tBEntry[3])+"</span>",
			    // 	// 		"<span style='font-weight: bold;'>"+formatter.format(tBEntry[4])+"</span>",
			    // 	// 		"<span style='font-weight: bold;'>"+formatter.format(tBEntry[5])+"</span>",
			    // 	// 		"<span style='font-weight: bold;'>"+formatter.format(tBEntry[6])+"</span>",
			    // 	// 	    "<span style='font-weight: bold;'>"+formatter.format(tBEntry.closingBalance)+"</span>",
	            //     //         tBEntry.bold,
	            //     //     ]);
		        // 	// }
		        // 	// else{
		        // 	//
		        // 	// 	if (tBEntry.accountName){
		        // 	// 		table.row.add([
				//     // 			tBEntry.accountName,
		        //     //             formatter.format(tBEntry.openingBalance),
		        //     //             formatter.format(tBEntry[7]),
		        //     //             formatter.format(tBEntry[8]),
		        //     //             formatter.format(tBEntry[9]),
		        //     //             formatter.format(tBEntry[10]),
		        //     //             formatter.format(tBEntry[11]),
		        //     //             formatter.format(tBEntry[12]),
		        //     //             formatter.format(tBEntry[1]),
		        //     //             formatter.format(tBEntry[2]),
		        //     //             formatter.format(tBEntry[3]),
		        //     //             formatter.format(tBEntry[4]),
		        //     //             formatter.format(tBEntry[5]),
		        //     //             formatter.format(tBEntry[6]),
		        //     //             formatter.format(tBEntry.closingBalance),
		        //     //             tBEntry.bold,
		        //     //         ]);
	        	// 	// 	}
		        // 	// 	else{
		        // 	// 		table.row.add(["&nbsp;", "&nbsp;", "&nbsp;", "&nbsp;", "&nbsp;", "&nbsp;", "&nbsp;", "&nbsp;", "&nbsp;", "&nbsp;" , "&nbsp;", "&nbsp;", "&nbsp;", "&nbsp;", "&nbsp;", "&nbsp;"]);
		        // 	// 	}
		        // 	//
		        // 	// }
		    	//
		        // });
		        //
		        // table.draw();
		        
		        // calculate all Totals
		        //calculateTotal();
		        // enable submit button
		        $("#submitTBForm").attr("disabled", false);
            },
            complete:function(){  
		     // enable submit button 
		    	$("#submitTBForm").attr("disabled", false);
            },
	    });
    	
    	//highlight
        /*$('#tb_report_datatable tbody').on( 'mouseenter', 'td', function () {
            
    		table.rows().eq(0).each(function (index) {
    			$(table.row(index).nodes()).removeClass('highlight');
    		});
    		$(table.cells().nodes()).removeClass('highlight');
    		
    		var rowIdx = table.cell(this).index().row;
            var colIdx = table.cell(this).index().column;
            
            $(table.row(rowIdx).nodes()).addClass('highlight');
            $(table.column(colIdx).nodes()).addClass('highlight');
        });*/
    	
    }

	$("#financialPositionReport").click(function () {
		$("#statementPLReport").attr("disabled", true);

		location.replace("/reports/financial-position-print/?financialYearId=" + $("#financialYearId").val() + "&companyId=" + $("#companyIds").val() + "&branchId=" + $("#branchIds").val() + "&voucherStatusId=" + $("#voucherStatusId").val() + "&toDate=" + $("#toDate").val());

		console.log("Print Button");
	});
    
    function getExcelBuilder(){
	    var xlsBuilder = {
			filename : 'P&L REPORT_' + new Date().toLocaleString(),
			sheetName : 'sheet1',
			customize : function(xlsx) {
				var sheet = xlsx.xl.worksheets['sheet1.xml'];
				var downrows = 8;
				var clRow = $('row', sheet);
				var msg;
				// update Row
				clRow.each(function() {
					var attr = $(this).attr('r');
					var ind = parseInt(attr);
					ind = ind + downrows;
					$(this).attr("r", ind);
				});
	
				// Update row > c
				$('row c ', sheet).each(
						function() {
							var attr = $(this).attr('r');
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
					k : 'A',
					v : 'TRIAL BALANCE' + ' ('
							+ getFormattedDate(new Date()) + ", " + new Date().toLocaleString().split(",")[1].trim()
							+ ')'
				} ]);
				var r2 = Addrow(2, [ {
					k : 'A',
					v : 'COMPANY :'
				}, {
					k : 'B',
					v : $('#companyIds option:selected').toArray().map(item => item.text).join(", ")
				} ]);
				var r3 = Addrow(3, [ {
					k : 'A',
					v : 'BRANCH :'
				}, {
					k : 'B',
					v : $('#branchIds option:selected').toArray().map(item => item.text).join(", ")
				} ]);
				var r4 = Addrow(4, [ {
					k : 'A',
					v : 'VOUCHER STATUS :'
				}, {
					k : 'B',
					v : $("#voucherStatusId option:selected").text()
				} ]);
				var r5 = Addrow(5, [ {
					k : 'A',
					v : 'FINANCIAL YEAR :'
				}, {
					k : 'B',
					v : $("#financialYearId option:selected").text() + " (" + getFormattedDate(new Date($("#fromDate").val())) + " - " + getFormattedDate(new Date($("#toDate").val())) + ")"
				} ]);
	
				sheet.childNodes[0].childNodes[1].innerHTML = r1
						+ r2
						+ r3
						+ r4
						+ r5
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

    function userLog() {
		//console.log("a "+$("#accountCode").val());
		 var userLogObj ={
				idNumber : "",
				code: "",
				buttonClick: buttionName,
				windowName:$('h2').html(),
			}
		 
	    $.ajax({
			type : "POST",
			contentType : "application/json",
			url : "/viewSaleOrder",
			data : JSON.stringify(userLogObj),
			dataType : "json",
			success:function(data){
				successmessage = 'Data was succesfully captured';
			//	location.reload();
	          //  $("#customerAccount\\.balanceLimit").text(successmessage);
	        },
	        error: function(data) {
	            successmessage = 'Error';
	          //  $("#customerAccount\\.balanceLimit").text(successmessage);
	        },
	    });
	}

});