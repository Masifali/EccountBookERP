$(document).ready(function() {

	var buttionName ="";

	// SUBMIT FORM
    $("#SAHReportForm").submit(function(event) {
		// Prevent the form from submitting via the browser.
    	buttionName = $("#submitSAHForm").text(); 
    	userLog();
		event.preventDefault();
		
		sendAccountHistoryReport();
		
	});
        
    function sendAccountHistoryReport(){
    	
    	$("#submitSAHForm").attr("disabled", true);  // disable submit button 
    	$('#loading').show();  // show loading indicator
    	
    	
    	
    	$('#gl_report_datatable').DataTable().clear();
    	$('#gl_report_datatable').DataTable().destroy();
    	var table = $('#gl_report_datatable').DataTable({
	        dom: 'Bfrtip',
	        'bPaginate': false,
	        destroy: true,
	        buttons: [
	            {
	                extend: 'copy',
	                text: 'COPY',
	                title: null,
	                footer: true,
	                exportOptions: {
	                 //   columns: [0, 1, 2, 5, 7, 8, 9, 10],
	                },
	                customize: function(doc){
	                	buttionName ="COPY";
	                	userLog();
	                }
	            },
	            $.extend(true, {}, getExcelBuilder(), {
	            	extend: 'excelHtml5',
	            	text: 'EXCEL',
	            	title: null,
	            	footer: true,
	            	exportOptions: {
	                    stripNewlines: false,
	                //    columns: [0, 1, 2, 5, 7, 8, 9, 10],
	                },
	                customize: function(doc){
	                	buttionName ="Excel";
	                	userLog();
	                }
	            }),
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
	                title: 'SEND PARTY ACCOUNT TO CUSTIMER HISTORY' ,
	                messageTop: '',
	                messageBottom: null,
	                footer: true,
	                exportOptions: {
	                   // columns: [0, 1, 2, 5, 7, 8, 9, 10],
	                    stripNewlines: false,
	                },
	                customize: function(doc){
	                	doc.defaultStyle.fontSize = 7;
	                	doc.pageMargins = [10, 10, 10, 10];
	                	doc.styles.tableHeader.fontSize = 7;
	                	doc.styles.tableFooter.fontSize = 7;
	                	doc.defaultStyle.alignment = 'left';
	                    doc.styles.tableHeader.alignment = 'left';
	                    doc.styles.tableFooter.alignment = 'left';
	                    buttionName ="PDF";
	                	userLog();
	                    doc['footer']=(function(page, pages) {
	                        return {
	                            columns: [
	                                '',
	                                {
	                                    // This is the right column
	                                    alignment: 'right',
	                                    text: ['page ', { text: page.toString() },  ' of ', { text: pages.toString() }]
	                                }
	                            ],
	                            margin: [10,0]// [left or right , up or down]
	                        }
	                    });
	                }
	            },
	            {
	                extend: 'print',
	                text: 'VIEW',
	                title: '',
	                messageTop:'',
	                messageBottom: null,
	                footer: true,
	                autoPrint: false,
	                exportOptions: {
	                    stripHtml: false,
	                    stripNewlines: false,
	                    //columns: [0, 1, 2, 5, 7, 8, 9, 10],
	                },
	                customize: function (win) {
	                    $(win.document.body).find('table').addClass('display').css('font-size', '12px');
	                    $(win.document.body).find('tr:nth-child(odd) td').each(function(index){
	                        $(this).css('background-color','WHITESMOKE');
	                    });
	                    $(win.document.body).find('tr:nth-child(even) td').each(function(index){
	                        $(this).css('background-color','WHITE');
	                    });
	                    
	                    $(win.document.body).css('background-color','WHITE');
	                    $(win.document.body).css("color", "DIMGRAY");
	                    
	                    $(win.document.body).find('h1').css('text-align', 'center');
	                    //$(win.document.body).find('h1').css("color", "DIMGRAY");
	                    
	                    $(win.document.body).find('div:first').css('text-align', 'center');
	                    //$(win.document.body).find('div:first').css("color", "DIMGRAY");
	                    
	                    $(win.document.body).find('th').css("color", "WHITE");
	                    $(win.document.body).find('th').css("background-color", "DIMGRAY");
	                    buttionName ="VIEW";
	                	userLog();
	                    doc['footer']=(function(page, pages) {
	                        return {
	                            columns: [
	                                '',
	                                {
	                                    // This is the right column
	                                    alignment: 'right',
	                                    text: ['page ', { text: page.toString() },  ' of ', { text: pages.toString() }]
	                                }
	                            ],
	                            margin: [10,0]// [left or right , up or down]
	                        }
	                    });
	                }
	            }
	        ]
	    });
    	
    	if ($("#reports_download").val() === "false"){
    		table.buttons( '.dt-button' ).remove();
    	}
    	
    	// PREPARE FORM DATA    
    	var formData = {
			
    		fromDate : $("#fromDate").val(),
    		toDate : $("#toDate").val(),
    		code : $("#code").val(),
    	}
    	
    	// DO POST
    	$.ajax({
			type : "POST",
			contentType : "application/json",
			url : $("#SAHReportForm").attr("action"),
			data : JSON.stringify(formData),
			dataType : "json",
			success:function(data){
						
		        $.each(data, function (i, sendSMS) {
		        	var sms = sendSMS.sms.replace("Title:",":Title:").replace("Branch Code:",":Branch Code:").replace("Account No:",":Account No:").replace("Address:",":Address:");
		        	var smsParts = sms.split(":");
		        	var bankName = "bank Name:"+"<a style='color: #009900;text:BOLD;'>"+"<b>"+smsParts[0].toUpperCase()+"</b>"+"</a>";
		        	var title = smsParts[1]+":"+"<a style='color:  #ff3399;text:BOLD;'>"+"<b>"+smsParts[2].toUpperCase()+"</b>"+"</a>";
		        	var accountNo = smsParts[3]+":"+"<a style='color: #009900;text:BOLD;'>"+"<b>"+smsParts[4]+"</b>"+"</a>";
		        	var brachCode = smsParts[5]+":"+"<a style='color: #009900;text:BOLD;'>"+"<b>"+smsParts[6]+"</b>"+"</a>";
		        	var address = smsParts[7]+":"+"<a style='color: #009900;text:bold;'>"+"<b>"+smsParts[8].toUpperCase()+"</b>"+"</a>";
		        	
		        	table.row.add([
                        (i+1),
                        getFormattedDate(new Date(sendSMS.createdAt))+ ", " +new Date(sendSMS.createdAt).toLocaleTimeString(),
                        sendSMS.createdBy,
                        bankName+" "+title+"  "+accountNo+"  "+brachCode+"  "+address,// "<a style='color: red;text:BOLD;'>"+"<b>"+sendSMS.sms.split(":")[0].toUpperCase()+"<b>"+"<b>"+sms.split(":")[0]+"<b>"+"</a>",
                        sendSMS.status,
                        sendSMS.account.code+'&emsp;'+sendSMS.account.accountName+'&emsp;'+sendSMS.account.mobile,
                      
                       
                    ]);
		        });
		        
		        table.draw();
		     
		        $("#submitSAHForm").attr("disabled", false);  // enable submit button
		        $('#loading').hide();  // hide loading indicator
            },
            complete:function(){  
            	$("#submitSAHForm").attr("disabled", false);  // enable submit button
		    	$('#loading').hide();  // hide loading indicator
            },
	    });
    	
    	//highlight
		$('#gl_report_datatable tbody').on( 'mouseenter', 'td', function () {
			if (table instanceof $.fn.dataTable.Api) {
				table.rows().eq(0).each(function (index) {
	    			$(table.row(index).nodes()).removeClass('highlight');
	    		});
	    		$(table.cells().nodes()).removeClass('highlight');
	    		
	    		var rowIdx = table.cell(this).index().row;
	            var colIdx = table.cell(this).index().column;
	            
	            $(table.row(rowIdx).nodes()).addClass('highlight');
	            $(table.column(colIdx).nodes()).addClass('highlight');
			}
        });
        
    }
    function getFormattedDate(date) {
		var year = date.getFullYear();
		var month = (1 + date.getMonth()).toString();
		month = month.length > 1 ? month : '0' + month;
		var day = date.getDate().toString();
		day = day.length > 1 ? day : '0' + day;
		return day + '/' + month + '/' + year;
	}
	
    function getExcelBuilder(){
	    var xlsBuilder = {
			filename : 'GL_' + new Date().toLocaleString(),
			sheetName : 'sheet1',
			customize : function(xlsx) {
				var sheet = xlsx.xl.worksheets['sheet1.xml'];
				var downrows = 7;
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
					v : 'GENERAL LEDGER' + ' ('
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
				var r6 = Addrow(6, [ {
					k : 'A',
					v : 'ACCOUNT :'
				}, {
					k : 'B',
					v : $("#accountCode option:selected").text()
				} ]);
	
				sheet.childNodes[0].childNodes[1].innerHTML = r1
						+ r2
						+ r3
						+ r4
						+ r5
						+ r6
						+ sheet.childNodes[0].childNodes[1].innerHTML;
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
				
				account :  {code: $("#accountCode").val()},
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