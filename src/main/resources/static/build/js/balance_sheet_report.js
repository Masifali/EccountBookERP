$(document).ready(function() {
	
	//on load
	restrictFromAndToDate();
	//loadFromAndToAccounts();
	
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
		
		$("#fromDate").attr("min", $("#financialYearId option:selected").attr("data-value").split("|")[0]);
		$("#fromDate").attr("max", $("#financialYearId option:selected").attr("data-value").split("|")[1]);
		$("#fromDate").val($("#financialYearId option:selected").attr("data-value").split("|")[0]);
		
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
		event.preventDefault();
		generateTBReport();
	});
        
    function generateTBReport(){
    	
    	// disable submit button 
    	$("#submitTBForm").attr("disabled", true);
    	
    	// calculate all Totals
        //calculateTotal();
    	
    	if (!$("#companyIds").val() || !$("#voucherStatusId").val() || !$("#financialYearId").val() || !$("#fromDate").val() || !$("#toDate").val()){
			alert("PLEASE SELECT ALL THE FIELDS")
    		return;
		}
    	
    	$('#tb_report_datatable').DataTable().clear();
    	$('#tb_report_datatable').DataTable().destroy();
    	var table = $('#tb_report_datatable').DataTable({
	        dom: 'Bfrtip',
	        bSort : false,
	        'bPaginate': false,
	        buttons: [
	        	{
	                extend: 'copy',
	                text: 'COPY',
	                title: null,
	                footer: true,
	            },
	            /*$.extend(true, {}, getExcelBuilder(), {
	            	extend: 'excelHtml5',
	            	text: 'EXCEL',
	            	title: null,
	            	footer: true,
	            	exportOptions: {
	                    stripNewlines: false
	                },
	            }),*/
	            /*{
	                extend: 'excelHtml5',
	                title: 'General Ledger' + ' (' + (new Date().toLocaleString()) + ')',
	                messageTop: 'COMPANY: ' + 'MST' + '\n' + 'ACCOUNT: ' + '111222333',
	                messageBottom: null,
	                stripNewlines: false
	            },*/
	            {
	                extend: 'pdfHtml5',
	                download: 'open',
	                orientation: 'landscape',
	                title: 'BALANCE SHEET REPORT' + ' (' + getFormattedDate(new Date()) + ", " + new Date().toLocaleString().split(",")[1].trim() + ')',
	                footer: true,
	                messageTop: 'COMPANY : ' + $('#companyIds option:selected').toArray().map(item => item.text).join(", ")
	                + '\n' + 'BRANCH: ' + $('#branchIds option:selected').toArray().map(item => item.text).join(", ")
	                + '\n' + 'VOUCHER STATUS : ' + $("#voucherStatusId option:selected").text()
	                + '\n' + 'FINANCIAL YEAR : ' + $("#financialYearId option:selected").text() + " (" + getFormattedDate(new Date($("#fromDate").val())) + " - " + getFormattedDate(new Date($("#toDate").val())) + ")",
	                messageBottom: null,
	                exportOptions: {
	                    stripNewlines: false,
	                    columns: [ 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12],
	                },
	                customize: function(doc){
	                	doc.defaultStyle.fontSize = 6;
	                	//pageMargins [left, top, right, bottom]
	                	//doc.content[2].table.body[1][1].text.length
	                	doc.pageMargins = [5, 5, 5, 5];
	                	doc.styles.tableHeader.fontSize = 6;
	                	doc.styles.tableFooter.fontSize = 6;
	                	doc.defaultStyle.alignment = 'left';
	                    doc.styles.tableHeader.alignment = 'left';
	                    doc.styles.tableFooter.alignment = 'left';
	                    
	                    /*doc.content[2].table.body[1].every( function ( rowIdx, tableLoop, rowLoop ) {
	                    	rowLoop[1].fillColor = 'black';
	                        // ... do something with data(), or this.node(), etc
	                    });*/
	                }
	            },
	            {
	                extend: 'print',
	                text: 'VIEW',
	                title: 'BALANCE SHEET REPORT' + ' (' + getFormattedDate(new Date()) + ", " + new Date().toLocaleString().split(",")[1].trim() + ')',
	                messageTop: 'COMPANY: ' + $('#companyIds option:selected').toArray().map(item => item.text).join(", ")
	                + '<br>' + 'BRANCH: ' + $('#branchIds option:selected').toArray().map(item => item.text).join(", ")
	                + '<br>' + 'VOUCHER STATUS: ' + $("#voucherStatusId option:selected").text()
	                + '<br>' + 'FINANCIAL YEAR: ' + $("#financialYearId option:selected").text() + " (" + getFormattedDate(new Date($("#fromDate").val())) + " - " + getFormattedDate(new Date($("#toDate").val())) + ")",
	                messageBottom: null,
	                footer: true,
	                autoPrint: false,
	                exportOptions: {
	                    stripHtml: false,
	                    stripNewlines: false,
	                    columns: [ 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12],
	                },
	                customize: function (win) {
	                    $(win.document.body).find('table').addClass('display').css('font-size', '12px');
	                    /*$(win.document.body).find('tr:nth-child(odd) td').each(function(index){
	                        $(this).css('background-color','WHITESMOKE');
	                    });
	                    $(win.document.body).find('tr:nth-child(even) td').each(function(index){
	                        $(this).css('background-color','WHITE');
	                    });*/
	                    
	                    $(win.document.body).css('background-color','WHITE');
	                    $(win.document.body).css("color", "DIMGRAY");
	                    $(win.document.body).find('h1').css('text-align', 'center');
	                    $(win.document.body).find('div:first').css('text-align', 'center');
	                    
	                    $(win.document.body).find("tr").each(function() {
	                    	if ($(this).find("td").length) {
	            	    		// SET COLORS
	            	    		if ($(this).find("span").length) {
	            		    		$(this).css("color", "WHITE");
	            		    		$(this).css("font-weight","bold");
	            		    		$(this).css("background-color", "DIMGRAY");
	            		    	}
	            	    	}
		                    /*if ($(this).find("td:eq(1)").text().trim().length == 2){
		                    	$(this).find("td").each(function(index, td){
		                    		$(td).css("color", "WHITE");
						    		$(td).css("background-color", "BLACK");
			                    });
		    	    		}
		                    if ($(this).find("td:eq(1)").text().trim().length == 5){
					    		$(this).find("td").each(function(index, td){
		                    		$(td).css("color", "WHITE");
						    		$(td).css("background-color", "DIMGRAY");
			                    });
		    	    		}
		                    if ($(this).find("td:eq(1)").text().trim().length == 9){
					    		$(this).find("td").each(function(index, td){
		                    		//$(td).css("color", "WHITE");
						    		$(td).css("background-color", "WHITE");
			                    });
		    	    		}*/
	                    });
	                    
	                }
	            }
	        ]
	    });
    	
    	// PREPARE FORM DATA    
    	var formData = {
    		companyIds : $("#companyIds").val(),
    		branchIds : $("#branchIds").val(),
    		level : $("#level").val(),
    		fromAccountCode : $("#fromAccountCode").val(),
    		toAccountCode : $("#toAccountCode").val(),
    		voucherStatusId : $("#voucherStatusId").val(),
    		financialYearId : $("#financialYearId").val(),
    		fromDate : $("#fromDate").val(),
    		toDate : $("#toDate").val(),
    	}
    	
    	// DO POST
    	$.ajax({
			type : "POST",
			contentType : "application/json",
			url : $("#tbReportForm").attr("action"),
			data : JSON.stringify(formData),
			dataType : "json",
			success:function(data){
				
				var formatter = new Intl.NumberFormat('ur-PK', {minimumFractionDigits: 2, maximumFractionDigits: 2});
				// FILL TABLE ROWS
				var rowIndex = 0;
		        $.each(data, function (i, tBEntry) {
		        	rowIndex = rowIndex + 1;
		    		
		        	if (tBEntry.bold){
		        		table.row.add([
			    			"<span style='font-weight: bold;'>"+tBEntry.accountName+"</span>",
			    			"<span style='font-weight: bold;'>"+formatter.format(tBEntry[7])+"</span>",
			    			"<span style='font-weight: bold;'>"+formatter.format(tBEntry[8])+"</span>",
			    			"<span style='font-weight: bold;'>"+formatter.format(tBEntry[9])+"</span>",
			    			"<span style='font-weight: bold;'>"+formatter.format(tBEntry[10])+"</span>",
			    			"<span style='font-weight: bold;'>"+formatter.format(tBEntry[11])+"</span>",
			    			"<span style='font-weight: bold;'>"+formatter.format(tBEntry[12])+"</span>",
			    			"<span style='font-weight: bold;'>"+formatter.format(tBEntry[1])+"</span>",
			    			"<span style='font-weight: bold;'>"+formatter.format(tBEntry[2])+"</span>",
			    			"<span style='font-weight: bold;'>"+formatter.format(tBEntry[3])+"</span>",
			    			"<span style='font-weight: bold;'>"+formatter.format(tBEntry[4])+"</span>",
			    			"<span style='font-weight: bold;'>"+formatter.format(tBEntry[5])+"</span>",
			    			"<span style='font-weight: bold;'>"+formatter.format(tBEntry[6])+"</span>",
	                    ]);
		        	}
		        	else{
		        		
		        		if (tBEntry.accountName){
		        			table.row.add([
				    			tBEntry.accountName,
		                        formatter.format(tBEntry[7]),
		                        formatter.format(tBEntry[8]),
		                        formatter.format(tBEntry[9]),
		                        formatter.format(tBEntry[10]),
		                        formatter.format(tBEntry[11]),
		                        formatter.format(tBEntry[12]),
		                        formatter.format(tBEntry[1]),
		                        formatter.format(tBEntry[2]),
		                        formatter.format(tBEntry[3]),
		                        formatter.format(tBEntry[4]),
		                        formatter.format(tBEntry[5]),
		                        formatter.format(tBEntry[6]),
		                    ]);
	        			}
		        		else{
		        			table.row.add(["&nbsp;", "&nbsp;", "&nbsp;", "&nbsp;", "&nbsp;", "&nbsp;", "&nbsp;", "&nbsp;", "&nbsp;", "&nbsp;" , "&nbsp;", "&nbsp;", "&nbsp;", "&nbsp;"]);
		        		}
		        		
		        	}
		    		
		        });
		        
		        table.draw();
		        
		        // calculate all Totals
		        calculateTotal();
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
    
    function getExcelBuilder(){
	    var xlsBuilder = {
			filename : 'BALANCE SHEET REPORT_' + new Date().toLocaleString(),
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
	
});