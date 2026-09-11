Number.prototype.toFixed = function(decimalPlaces) {
	return Number(Math.round(this + 'e' + decimalPlaces) + 'e-' + decimalPlaces);
}

$(document).ready(function() {
	
	//on load
	restrictFromAndToDate();
	//loadFromAndToAccounts();
	
	$("#companyId").on("change", function () {
		loadCompanyBranches();
    });
	
	function loadCompanyBranches() {
		
		if (!$("#companyId").val() || $("#companyId").val() == 0){
			return;
		}
	    
		$.get( "/vouchers/company_branches?companyId=" + $("#companyId").val(), function( data ) {
    		$("#branchId").empty();
    		$("#branchId").append("<option value='0'>ALL</option>");
            
            for (var i = 0, len = data.length; i < len; i++) {
            	var option = "<option value = " + data[i].id + ">" + data[i].name +  "</option>";
            	$("#branchId").append(option);
        	}
	    });
	}
	
	$("#level").on("change", function () {
		loadFromAndToAccounts();
    });
	
	function loadFromAndToAccounts() {
		
		if (!$("#level").val()){
			return;
		}
	    
		$.get( "/reports/load_from_and_to_Accounts?level=" + $("#level").val(), function( data ) {
    		
			$("#fromAccountCode").empty();
    		$("#toAccountCode").empty();
            
    		$.each(data, function(i, record){
            	var option = "<option value = " + record.code + ">" + record.formattedCode + "&emsp;" + record.accountName +  "</option>";
            	$("#fromAccountCode").append(option);
            	$("#toAccountCode").append(option);
            });
    		
    		$("#fromAccountCode option:first").attr("selected", "selected");
    		$("#toAccountCode option:last").attr("selected", "selected");
    		
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
		
		var openingBalanceTotal = parseFloat(0);
		var debitTotal = parseFloat(0);
		var creditTotal = parseFloat(0);
		var closingBalanceTotal = parseFloat(0);
		
		var formatter = new Intl.NumberFormat('ur-PK', {minimumFractionDigits: 2, maximumFractionDigits: 2});
		
		$("#tb_report_datatable tr").each(function() {
	    	if ($(this).find("td").length) {
	    		
	    		// SET COLORS
	    		if ($(this).find("td:eq(1)").text().trim().length == 2) {
		    		$(this).css("color", "WHITE");
		    		$(this).css("background-color", "BLACK");
		    	}
		    	if ($(this).find("td:eq(1)").text().trim().length == 5) {
		    		$(this).css("color", "WHITE");
		    		$(this).css("background-color", "DIMGRAY");
		    	}
		    	if ($(this).find("td:eq(1)").text().trim().length == 9) {
		    		$(this).css("color", "WHITE");
		    		$(this).css("background-color", "DARKGRAY");
		    	}
	    		
	    		if ($(this).find("td:eq(1)").text().trim().length == 2){
		    		openingBalanceTotal = openingBalanceTotal + parseFloat($(this).find("td:eq(3)").text().replace(/[^0-9\.-]+/g, "") || 0);
			    	debitTotal = debitTotal + parseFloat($(this).find("td:eq(4)").text().replace(/[^0-9\.-]+/g, "") || 0);
			    	creditTotal = creditTotal + parseFloat($(this).find("td:eq(5)").text().replace(/[^0-9\.-]+/g, "") || 0);
			    	closingBalanceTotal = closingBalanceTotal + parseFloat($(this).find("td:eq(6)").text().replace(/[^0-9\.-]+/g, "") || 0);
	    		}
	    	}
	    });
		
		$("#tb_report_datatable span#openingBalanceTotal").text(formatter.format(openingBalanceTotal));
		$("#tb_report_datatable span#debitTotal").text(formatter.format(debitTotal));
		$("#tb_report_datatable span#creditTotal").text(formatter.format(creditTotal));
		$("#tb_report_datatable span#closingBalanceTotal").text(formatter.format(closingBalanceTotal));
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
    	
    	var formatter = new Intl.NumberFormat('ur-PK', {minimumFractionDigits: 2, maximumFractionDigits: 2});
    	
    	// calculate all Totals
        calculateTotal();
    	
    	if (!$("#companyId").val() || !$("#voucherStatusId").val() || !$("#summaryDate").val()){
			alert("PLEASE SELECT ALL THE FIELDS");
    		return;
		}
    	
    	$('#tb_report_datatable').DataTable().clear();
    	$('#tb_report_datatable').DataTable().destroy();
    	var table = $('#tb_report_datatable').DataTable({
	        dom: 'Bfrtip',
	        'bPaginate': false,
	        /*initComplete : function(){
	        	$("#tb_report_datatable tbody tr").each(function() {
	        		if ($(this).find('td:eq(1)').text().trim() == '33-12-102'){
	        			$(this).addClass('notPrintable');
	        		}
	    	    });
	        	var api = this.api();
	        	var searchString = '(' + $('input[type=search]').val().split(' ').join('|') + ')';
	        	api.search(searchString, true).draw(true);
	            api.$('input[type=search]').on('keyup redraw', function() {
	                api.search(searchString, true).draw(true);
	            } );
	        },*/
	        buttons: [
	        	{
	                extend: 'copy',
	                text: 'COPY',
	                title: null,
	                footer: true,
	            },
	            $.extend(true, {}, getExcelBuilder(), {
	            	extend: 'excelHtml5',
	            	text: 'EXCEL',
	            	title: null,
	            	footer: true,
	            	exportOptions: {
	                    stripNewlines: false
	                },
	            }),
	            {
	                extend: 'pdfHtml5',
	                title: 'DAILY SUMMARY REPORT' + ' (' + getFormattedDate(new Date()) + ", " + new Date().toLocaleString().split(",")[1].trim() + ')',
	                footer: true,
	                messageTop: 'COMPANY : ' + $("#companyId option:selected").text()
	                + '\n' + 'BRANCH: ' + $("#branchId option:selected").text()
	                + '\n' + 'VOUCHER STATUS : ' + $("#voucherStatusId option:selected").text()
	                + '\n' + 'SUMMARY DATE : ' + getFormattedDate(new Date($("#summaryDate").val())),
	                messageBottom: null,
	                customize: function(doc){
	                	doc.defaultStyle.fontSize = 8;
	                	//pageMargins [left, top, right, bottom]
	                	doc.pageMargins = [5, 5, 5, 5];
	                	doc.styles.tableHeader.fontSize = 8;
	                	doc.styles.tableFooter.fontSize = 8;
	                	doc.defaultStyle.alignment = 'left';
	                    doc.styles.tableHeader.alignment = 'left';
	                    doc.styles.tableFooter.alignment = 'left';
	                }
	            },
	            {
	                extend: 'print',
	                text: 'VIEW',
	                title: 'DAILY SUMMARY REPORT' + ' (' + getFormattedDate(new Date()) + ", " + new Date().toLocaleString().split(",")[1].trim() + ')',
	                messageTop: 'COMPANY: ' + $("#companyId option:selected").text()
	                + '<br>' + 'BRANCH: ' + $("#branchId option:selected").text()
	                + '<br>' + 'VOUCHER STATUS: ' + $("#voucherStatusId option:selected").text()
	                + '<br>' + 'SUMMARY DATE: ' + getFormattedDate(new Date($("#summaryDate").val())),
	                messageBottom: null,
	                footer: true,
	                autoPrint: false,
	                exportOptions: {
	                    stripHtml: false,
	                    stripNewlines: false
	                },
	                customize: function (win) {
	                	
	                	$(win.document.body).find('h1').css('font-size', '15pt');
	                    $(win.document.body).find('h1').css('text-align', 'center'); 
	                    
	                	$(win.document.body).find('table tbody tr td,th').addClass('display').css('padding', '1px 2px 1px 2px');
	                    $(win.document.body).find('table tbody tr td,th').addClass('display').css('vertical-align', 'middle');
	                    
	                    $(win.document.body).find('table').addClass('display').css('font-size', '11px');
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
		                    if ($(this).find("td:eq(1)").text().trim().length == 2){
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
		                    		$(td).css("color", "WHITE");
						    		$(td).css("background-color", "DARKGRAY");
			                    });
		    	    		}
	                    });
	                    
	                }
	            },
	            {
	                extend: 'print',
	                action: function(e, dt, button, config) {
	                    
	                    // Add code to make changes to table here
	                	var searchString = '(33-12-102|22-12-101)';
	    	        	dt.search(searchString, true).draw(true);
	                    // Call the original action function afterwards to
	                    // continue the action.
	                    // Otherwise you're just overriding it completely.
	                    setTimeout(function(){
	                    	$.fn.dataTable.ext.buttons.print.action(e, dt, button, config);
	                      }, 200);
	                    /*setTimeout(function(){
	                    	dt.search("").draw(true);
	                      }, 500);*/
	                },
	                text: 'PRINT TRADE P/R',
	                title: 'DAILY SUMMARY REPORT' + ' (' + getFormattedDate(new Date()) + ", " + new Date().toLocaleString().split(",")[1].trim() + ')',
	                messageTop: 'COMPANY: ' + $("#companyId option:selected").text()
	                + '<br>' + 'BRANCH: ' + $("#branchId option:selected").text()
	                + '<br>' + 'VOUCHER STATUS: ' + $("#voucherStatusId option:selected").text()
	                + '<br>' + 'SUMMARY DATE: ' + getFormattedDate(new Date($("#summaryDate").val())),
	                messageBottom: null,
	                footer: true,
	                autoPrint: false,
	                exportOptions: {
	                    stripHtml: false,
	                    stripNewlines: false,
	                    /*rows: ':not(.notPrintable)'*/
	                },
	                customize: function (win) {
	                	
	                	$(win.document.body).find('h1').css('font-size', '15pt');
	                    $(win.document.body).find('h1').css('text-align', 'center'); 
	                    
	                	$(win.document.body).find('table tbody tr td,th').addClass('display').css('padding', '1px 2px 1px 2px');
	                    $(win.document.body).find('table tbody tr td,th').addClass('display').css('vertical-align', 'middle');
	                    
	                    $(win.document.body).find('table').addClass('display').css('font-size', '11px');
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
		                    if ($(this).find("td:eq(1)").text().trim().length == 2){
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
		                    		$(td).css("color", "WHITE");
						    		$(td).css("background-color", "DARKGRAY");
			                    });
		    	    		}
	                    });
	                    
	                }
	            }
	        ],
	        drawCallback: function () {
	        	var api = this.api();
	        	
	            // Remove the formatting to get integer data for summation
	            var intVal = function ( i ) {
	                return typeof i === 'string' ?
	                    i.replace(/[\$,]/g, '')*1 :
	                    typeof i === 'number' ?
	                        i : 0;
	            };
	            
	            /*$( api.table().footer() ).html(
	              api.column( 4, {page:'current'} ).data().sum()
	            );*/
	            var totalDebit = api.cells(function (index, data, node) {
	            			return api.row(index).data()[1].toString().length == 14 ?
                            true : false;
	            		}, 4, { search: 'applied' })
	            		.data().reduce(function (a, b) {
                            return intVal(a) + intVal(b);
                        }, 0)
                var totalCredit = api.cells(function (index, data, node) {
	            			return api.row(index).data()[1].toString().length == 14 ?
                            true : false;
	            		}, 5, { search: 'applied' })
	            		.data().reduce(function (a, b) {
                            return intVal(a) + intVal(b);
                        }, 0)
                $(api.column(4).footer()).html(formatter.format(parseFloat(totalDebit).toFixed(2)));
	            $(api.column(5).footer()).html(formatter.format(parseFloat(totalCredit).toFixed(2)));
	          },
	          
	    });
    	
    	// PREPARE FORM DATA    
    	var formData = {
    		companyId : $("#companyId").val(),
    		branchId : $("#branchId").val(),
    		voucherStatusId : $("#voucherStatusId").val(),
    		summaryDate : $("#summaryDate").val(),
    	}
    	
    	// DO POST
    	$.ajax({
			type : "POST",
			contentType : "application/json",
			url : $("#tbReportForm").attr("action"),
			data : JSON.stringify(formData),
			dataType : "json",
			success:function(data){
				
				// FILL TABLE ROWS
				var rowIndex = 0;
		        $.each(data, function (i, tBEntry) {
		        	rowIndex = rowIndex + 1;
		    		
		    		table.row.add([
		    			rowIndex,
                        tBEntry.accountFormattedCode,
                        tBEntry.accountName,
                        "",
                        formatter.format(tBEntry.debit),
                        formatter.format(tBEntry.credit),
                    ]);
		    		
		    		$.each(tBEntry.children, function (l, fourthLevel) {
		    			
		    			var voucherViewLink;
			        	if (fourthLevel.voucherCode){
				    		if (fourthLevel.voucherCode.indexOf("STV") >= 0){
				    			voucherViewLink = "<a style='text-decoration: underline;' target=_blank href="+location.protocol + "//" + location.host+"/stock_transfer_vouchers/"+fourthLevel.voucherId+">"+fourthLevel.voucherCode+"</a>";
				    		}
				    		if (fourthLevel.voucherCode.indexOf("PJV") >= 0){
				    			voucherViewLink = "<a style='text-decoration: underline;' target=_blank href="+location.protocol + "//" + location.host+"/purchase_journal_vouchers/"+fourthLevel.voucherId+">"+fourthLevel.voucherCode+"</a>";
				    		}
				    		else if (fourthLevel.voucherCode.indexOf("PRV") >= 0){
				    			voucherViewLink = "<a style='text-decoration: underline;' target=_blank href="+location.protocol + "//" + location.host+"/purchase_return_vouchers/"+fourthLevel.voucherId+">"+fourthLevel.voucherCode+"</a>";
				    		}
				    		else if (fourthLevel.voucherCode.indexOf("SJV") >= 0){
				    			voucherViewLink = "<a style='text-decoration: underline;' target=_blank href="+location.protocol + "//" + location.host+"/sale_journal_vouchers/"+fourthLevel.voucherId+">"+fourthLevel.voucherCode+"</a>";
				    		}
				    		else if (fourthLevel.voucherCode.indexOf("SRV") >= 0){
				    			voucherViewLink = "<a style='text-decoration: underline;' target=_blank href="+location.protocol + "//" + location.host+"/sale_return_vouchers/"+fourthLevel.voucherId+">"+fourthLevel.voucherCode+"</a>";
				    		}
				    		else if (fourthLevel.voucherCode.indexOf("STV") >= 0){
				    			voucherViewLink = "<a style='text-decoration: underline;' target=_blank href="+location.protocol + "//" + location.host+"/stock_transfer_vouchers/"+fourthLevel.voucherId+">"+fourthLevel.voucherCode+"</a>";
				    		}
				    		else{
				    			voucherViewLink = "<a style='text-decoration: underline;' target=_blank href="+location.protocol + "//" + location.host+"/vouchers/"+fourthLevel.voucherId+">"+fourthLevel.voucherCode+"</a>";
				    		}
			        	}
			        	
	    				rowIndex = rowIndex + 1;
			    		table.row.add([
			    			rowIndex,
	                        "<a style='text-decoration: underline;' target=_blank href="+location.protocol + "//" + location.host+"/reports/general_ledger/?code="+fourthLevel.accountCode+">"+fourthLevel.accountFormattedCode+"</a>",
	                        fourthLevel.accountName,
	                        voucherViewLink,
	                        formatter.format(fourthLevel.debit),
	                        formatter.format(fourthLevel.credit),
	                    ]);
			        });
		    		
		        });
		        
		        table.draw();
		        
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
    
    function getExcelBuilder(){
	    var xlsBuilder = {
			filename : 'DAILY SUMMARY REPORT_' + getFormattedDate(new Date($("#summaryDate").val())),
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
					v : 'DAILY SUMMARY REPORT' + ' ('
							+ getFormattedDate(new Date()) + ", " + new Date().toLocaleString().split(",")[1].trim()
							+ ')'
				} ]);
				var r2 = Addrow(2, [ {
					k : 'A',
					v : 'COMPANY :'
				}, {
					k : 'B',
					v : $("#companyId option:selected").text()
				} ]);
				var r3 = Addrow(3, [ {
					k : 'A',
					v : 'BRANCH :'
				}, {
					k : 'B',
					v : $("#branchId option:selected").text()
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
					v : 'SUMMARY DATE :'
				}, {
					k : 'B',
					v : getFormattedDate(new Date($("#summaryDate").val()))
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
                	if(index > 5){
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