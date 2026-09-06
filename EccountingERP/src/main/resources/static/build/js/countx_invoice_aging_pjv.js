$(document).ready(function() {
	
	//on load
	restrictFromAndToDate();
	//loadCompanyBranches();
	var formatter = new Intl.NumberFormat('ur-PK', {minimumFractionDigits: 2, maximumFractionDigits: 2});
	var buttionName ="";
	$("#companyIds").on("change", function () {
		loadCompanyBranches();
    });
	
	 var opening   = parseFloat(0);
     var  oPer   = parseFloat(0);
	  var advance    = parseFloat(0);
	  var one    = parseFloat(0);
      var  two   = parseFloat(0);
	  var  three = parseFloat(0);
	  var  four  = parseFloat(0);
	  var  five  = parseFloat(0);
	  var  six   = parseFloat(0);
	  var  seven = parseFloat(0);
	  var  eight = parseFloat(0);
	  var  nine = parseFloat(0);
	  var  ten = parseFloat(0);
	  var  over = parseFloat(0);
	  var  total = parseFloat(0);
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
////		
//		$("#toDate").attr("min", $("#financialYearId option:selected").attr("data-value").split("|")[0]);
//		$("#toDate").attr("max", $("#financialYearId option:selected").attr("data-value").split("|")[1]);
//		$("#toDate").val($("#financialYearId option:selected").attr("data-value").split("|")[1]);
//		
	}
	
	let searchParams = new URLSearchParams(window.location.search)
	if (searchParams.has("code") || searchParams.has("mobileNumbers")){
		setTimeout(function() { $("#glReportForm").submit(); }, 1000);
	}
	
	$("#accountCode").on("change", function () {
		if ($("#accountCode").val().indexOf("3312102") !== -1){
			$("#show_sms_modal").show();
		}
		else{
			$("#show_sms_modal").hide();
		}
    });
	
	function calculateTotal(){
		
		var debitTotal = parseFloat(0);
		var creditTotal = parseFloat(0);
		//var balanceTotal = parseFloat(0);
		
		var formatter = new Intl.NumberFormat('ur-PK', {minimumFractionDigits: 2, maximumFractionDigits: 2});

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
    $("#glReportForm").submit(function(event) {
		// Prevent the form from submitting via the browser.
    	buttionName = $("#submitGLForm").text(); 
    //	userLog();
		event.preventDefault();
		generateGLReport();
	});
   
    function generateGLReport(){
    	
    	$("#submitGLForm").attr("disabled", true);  // disable submit button 
    	$('#pacMan').show();  // show loading indicator
    	console.log("asif");
    	// calculate all Totals
       
    	
    	if (!$("#companyIds").val() || !$("#voucherStatusId").val() || !$("#financialYearId").val() || !$("#fromDate").val() || !$("#toDate").val()){
			alert("PLEASE SELECT ALL THE FIELDS")
    		return;
		}
    	
    	$('#pvj_report_datatable').DataTable().clear();
    	$('#pvj_report_datatable').DataTable().destroy();
    	var table = $('#pvj_report_datatable').DataTable({
    		
	        dom: 'Bfrtip',
	        fixedHeader: true,
	        'bPaginate': false,
	        destroy: true,
	        buttons: [
	            {
	                extend: 'copy',
	                text: 'COPY',
	                title: null,
	                footer: true,
	                exportOptions: {
	                    //c///olumns: [0, 1, 2, 5, 7, 8, 9, 10],
	                },
	                customize: function(doc){
	                	buttionName ="COPY";
	                	userLog();
	                }
	            },

	            {
	                extend: 'pdfHtml5',
	                download: 'open',
	                title: 'AGING',
	                messageBottom: null,
	                footer: true,
	                exportOptions: {
	                    //columns: [0, 1, 2, 5, 7, 8, 9, 10],
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
	                title: 'INVOICE AGING',
	                messageBottom: null,
	                footer: true,
	                autoPrint: false,
	                exportOptions: {
	                    stripHtml: false,
	                    stripNewlines: false,
	                   // columns: [0, 1, 2, 5, 7, 8, 9, 10],
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
	        ],
	        "footerCallback": function ( row, data, start, end, display ) {
	            var api = this.api(), data;
	            
	            var colNumber = [3,4,11];
	            
	            // Remove the formatting to get integer data for summation
	            var intVal = function ( i ) {
	            	if (typeof i === 'string'){
	            		return i.replace(/[\$,]/g, '')*1;
	            	}
	            	else if (typeof i === 'number'){
	            		return parseFloat(i);
	            	}
	            	else{
	            		return parseFloat(0);
	            	}
	            };
	            
	            for (i = 0; i < colNumber.length; i++) {
	                var colNo = colNumber[i];
	                var total = api.column(colNo) .data().reduce(function (a, b) {
                        return intVal(a) + intVal(b);
                    }, 0);
	                //$(api.column(colNo).footer()).html(parseFloat(total).toFixed(2));
	                $(api.column(colNo).footer()).html(formatter.format(parseFloat(total).toFixed(2)));
	            }
	        },
	        
	       
	    });
    	
    	if ($("#reports_download").val() === "false"){
    		table.buttons( '.dt-button' ).remove();
    	}
    	  
    	// PREPARE FORM DATA    
    	var formData = {
			companyIds : $("#companyIds").val(),
    		branchIds : $("#branchIds").val(),
    		voucherStatusId : $("#voucherStatusId").val(),
    		financialYearId : $("#financialYearId").val(),
    		fromDate : $("#fromDate").val(),
    		toDate : $("#toDate").val(),
    		
    	}
    	
    	// DO POST
    	$.ajax({
			type : "POST",
			contentType : "application/json",
			url : $("#glReportForm").attr("action"),
			data : JSON.stringify(formData),
			dataType : "json",
			success:function(data){
				
				var formatter = new Intl.NumberFormat('ur-PK', {minimumFractionDigits: 0, maximumFractionDigits: 0});
				var oneCol ="";
				var onePercent ="";
				var twoCol ="";
				var twoPercent ="";
				var threeCol="";
				var threePercent ="";
				var fourCol ="";
				var fourPercent ="";
				var fiveCol="";
				var fivePercent ="";
				var sixCol="";
				var sevenCol="";
				var eightCol="";
				var nineCol="";
				var ninePercent="";
				var tenCol="";
				var overninty="";
				var overnintyPercent ="";
				
		        $.each(data, function (i, gLEntry) {
		       
		        	 opening = opening+parseFloat(gLEntry.greaterNinty);
		        	 oPer = oPer+parseFloat(gLEntry.greaterNintyPercent);
		        	 advance = advance+parseFloat(gLEntry.advance);
		        	 one = one+parseFloat(gLEntry.one);
		        	 two = two+parseFloat(gLEntry.onePercent);
		        	 three = three+parseFloat(gLEntry.two);
		        	 four = four+parseFloat(gLEntry.twoPercent);
		        	 five = five+parseFloat(gLEntry.three);
		        	 six = six+parseFloat(gLEntry.threePercent);
		        	 seven = seven+parseFloat(gLEntry.four);
		        	 eight = eight+parseFloat(gLEntry.fourPercent);
		        	 nine = nine+parseFloat(gLEntry.five); // greater than 4 days
		        	 ten = ten+parseFloat(gLEntry.fivePercent);
		        	 total = total+parseFloat(gLEntry.total);
		        	 
		        	oneCol ="<div title='"+gLEntry.oneTitle+"' style='background-color: #e6dce8;  padding: .2em .6em .3em; border-radius: .25em;'>"+ formatter.format(gLEntry.one)+"</div>";
		        	onePercent ="<div title='"+gLEntry.oneTitle+"' style='background-color: #e6dce8;  padding: .2em .6em .3em; border-radius: .25em;'>"+ formatter.format(gLEntry.onePercent)+"</div>";
		        	twoCol ="<div title='"+gLEntry.twoTitle+"' style=' background-color: #e6dce8 ; padding: .2em .6em .3em; border-radius: .25em;'>"+ formatter.format(gLEntry.two)+"</div>";
		        	twoPercent="<div title='"+gLEntry.twoTitle+"' style=' background-color: #e6dce8 ; padding: .2em .6em .3em; border-radius: .25em;'>"+ formatter.format(gLEntry.twoPercent)+"</div>";
		        	threeCol ="<div title='"+gLEntry.threeTitle+"' style='background-color: #e6dce8;  padding: .2em .6em .3em; border-radius: .25em;'>"+ formatter.format(gLEntry.three)+"</div>";
		        	threePercent="<div title='"+gLEntry.threeTitle+"' style='background-color: #e6dce8;  padding: .2em .6em .3em; border-radius: .25em;'>"+ formatter.format(gLEntry.threePercent)+"</div>";
		        	fourCol ="<div title='"+gLEntry.fourTitle+"' style='background-color: #e6dce8;  padding: .2em .6em .3em; border-radius: .25em;'>"+ formatter.format(gLEntry.four)+"</div>";
		        	fourPercent="<div title='"+gLEntry.fourTitle+"' style='background-color: #e6dce8;  padding: .2em .6em .3em; border-radius: .25em;'>"+ formatter.format(gLEntry.fourPercent)+"</div>";
		        	fiveCol ="<div title='"+gLEntry.fiveTitle+"' style='background-color: #e6dce8;  padding: .2em .6em .3em; border-radius: .25em;'>"+ formatter.format(gLEntry.five)+"</div>";
		        	fivePercent="<div title='"+gLEntry.fiveTitle+"' style='background-color: #e6dce8;  padding: .2em .6em .3em; border-radius: .25em;'>"+ formatter.format(gLEntry.fivePercent)+"</div>";
		        	overninty ="<div title='"+gLEntry.overninty+"' style='background-color: #e6dce8;  padding: .2em .6em .3em; border-radius: .25em;'>"+ formatter.format(gLEntry.greaterNinty)+"</div>";
		        	overnintyPercent ="<div title='"+gLEntry.overninty+"' style='background-color: #e6dce8;  padding: .2em .6em .3em; border-radius: .25em;'>"+ formatter.format(gLEntry.greaterNintyPercent)+"</div>";
		        	
		        	overninty ="<div title='"+gLEntry.overninty+"' style='background-color: #e6dce8;  padding: .2em .6em .3em; border-radius: .25em;'>"+ formatter.format(gLEntry.greaterNinty)+"</div>";
		        	
		    		table.row.add([
                        (++i),
                       //adeel shokat
                        "<a style='text-decoration: underline;' target=_blank href="+location.protocol + "//" + location.host+"/reports/general_ledger/?code="+gLEntry.accountCode+">"+gLEntry.accountFormattedCode+"</a>",
                        gLEntry.accountName,
                        overninty,
                        overnintyPercent,
                        
                        formatter.format(gLEntry.advance),
                        oneCol,
                        onePercent,
                        twoCol,
                        twoPercent,
                        threeCol,
                        threePercent,
                        fourCol,
                        fourPercent,
                        fiveCol,
                        fivePercent,
                        formatter.format(gLEntry.total),
                       
                      
                    ]);
		        });
		       
		        table.draw();
		       
		        setFootterValue();
		        $("#submitGLForm").attr("disabled", false);  // enable submit button
		        $('#pacMan').hide();  // hide loading indicator
            },
            complete:function(){  
            	$("#submitGLForm").attr("disabled", false);  // enable submit button
		    	
		    	
		    	   
            },
	    });
    	
    	//highlight
		$('#pvj_report_datatable tbody').on( 'mouseenter', 'td', function () {
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
    
    function getExcelBuilder(){
	   
	    return xlsBuilder;
    }
	

    function userLog() {
		//console.log("a "+$("#accountCode").val());
	
	}
//    $('a[data-toggle="tab"]').on("shown.bs.tab", function (e) {
//        $($.fn.dataTable.tables(true)).DataTable().fixedHeader.adjust();
//      });
    function setFootterValue(){
    	var formatter = new Intl.NumberFormat('ur-PK', {minimumFractionDigits: 2, maximumFractionDigits: 2});
    	
//    	
    	$("span#asfDue").text(formatter.format(opening));
    	$("span#asifPer").text(formatter.format(oPer));
    	
    	$("span#advance").text(formatter.format(advance));
    	$("span#one").text(formatter.format(one));
    	$("span#onePer").text(formatter.format(two));
    	$("span#two").text(formatter.format(three));
    	$("span#twoPer").text(formatter.format(four));
    	$("span#three").text(formatter.format(five));
    	$("span#threePer").text(formatter.format(six));
    	$("span#four").text(formatter.format(seven));
    	$("span#fourPer").text(formatter.format(eight));
    	$("span#five").text(formatter.format(nine));
    	
    	$("span#fivePer").text(formatter.format(ten));
    	$("span#total").text(formatter.format(total));
    }
    
   // $("#pvj_report_datatable span#_oneb").text("asif");
});