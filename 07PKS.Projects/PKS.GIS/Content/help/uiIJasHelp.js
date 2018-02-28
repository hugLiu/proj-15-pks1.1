$(function() {
	
	$("#ee201").hide();
	var window = $('#ui>div>div:first');
	var linkMenu = $('#ui>div>div:eq(2)');
	var listMenu = $('#ui>div>div:eq(4)');
	var tableMenu = $('#ui>div>div:eq(6)');
	var toolMenu = $('#ui>div>div:eq(8)');
	var rightMenu = $('#ui>div>div:last').prev('div');
	//var munuMessage = $('#ui>div>div:last').prev('div');
	$("div[class='panel-header accordion-header']").click(function(item){
			window.css("background",
				"url('../image/window1.png')");
			linkMenu.css("background",
				"url('../image/linkMenu1.png')");
			listMenu.css("background",
				"url('../image/listMenu1.png')");
			tableMenu.css("background",
				"url('../image/tableMenu1.png')");
			toolMenu.css("background",
				"url('../image/toolMenu1.png')");
			rightMenu.css("background",
				"url('../image/rightMenu1.png')");
			//munuMessage.css("background",
				//"url('../image/munuMessage1.png')");
		});
		$("div[class='panel-header accordion-header accordion-header-selected']").click(function(item){
			window.css("background",
				"url('../image/window1.png')");
			linkMenu.css("background",
				"url('../image/linkMenu1.png')");
			listMenu.css("background",
				"url('../image/listMenu1.png')");
			tableMenu.css("background",
				"url('../image/tableMenu1.png')");
			toolMenu.css("background",
				"url('../image/toolMenu1.png')");
			rightMenu.css("background",
				"url('../image/rightMenu1.png')");
			//munuMessage.css("background",
				//"url('../image/munuMessage1.png')");
		});
	if (window) {
			window.css("background",
					"url('../image/window2.png')");

			window.toggle(function() {
				
					if($("#ui")[0].children[0].clientHeight > 100){
						window.css("background",
							"url('../image/window1.png')");
					}
					else{
						window.css("background",
							"url('../image/window2.png')");
					} 
				}, 
				
			 	function() {
					if($("#ui")[0].children[0].clientHeight > 100){
						window.css("background",
							"url('../image/window1.png')");
					}
					else{
						window.css("background",
							"url('../image/window2.png')");
					} 
				});
			
		}
		if (linkMenu) {
			linkMenu.css("background",
					"url('../image/linkMenu1.png')");

			linkMenu.toggle(function() {
					if($("#ui")[0].children[1].clientHeight > 100){
						linkMenu.css("background",
							"url('../image/linkMenu1.png')");
					}
					else{
						linkMenu.css("background",
							"url('../image/linkMenu2.png')");
					} 
				}, 
				
			 	function() {
					if($("#ui")[0].children[1].clientHeight > 100){
						linkMenu.css("background",
							"url('../image/linkMenu1.png')");
					}
					else{
						linkMenu.css("background",
							"url('../image/linkMenu2.png')");
					} 
				});
			
		}
		if (listMenu) {
			listMenu.css("background",
					"url('../image/listMenu1.png')");

			listMenu.toggle(function() {
					if($("#ui")[0].children[2].clientHeight > 100){
						listMenu.css("background",
							"url('../image/listMenu1.png')");
					}
					else{
						listMenu.css("background",
							"url('../image/listMenu2.png')");
					} 
				}, 
				
			 	function() {
					if($("#ui")[0].children[2].clientHeight > 100){
						listMenu.css("background",
							"url('../image/listMenu1.png')");
					}
					else{
						listMenu.css("background",
							"url('../image/listMenu2.png')");
					} 
				});
			
		}
		if (tableMenu) {
			tableMenu.css("background",
					"url('../image/tableMenu1.png')");

			tableMenu.toggle(function() {
					if($("#ui")[0].children[3].clientHeight > 100){
						tableMenu.css("background",
							"url('../image/tableMenu1.png')");
					}
					else{
						tableMenu.css("background",
							"url('../image/tableMenu2.png')");
					} 
				}, 
				
			 	function() {
					if($("#ui")[0].children[3].clientHeight > 100){
						tableMenu.css("background",
							"url('../image/tableMenu1.png')");
					}
					else{
						tableMenu.css("background",
							"url('../image/tableMenu2.png')");
					} 
				});
			
		}
		if (toolMenu) {
			toolMenu.css("background",
					"url('../image/toolMenu1.png')");

			toolMenu.toggle(function() {
					if($("#ui")[0].children[4].clientHeight > 100){
						toolMenu.css("background",
							"url('../image/toolMenu1.png')");
					}
					else{
						toolMenu.css("background",
							"url('../image/toolMenu2.png')");
					} 
				}, 
				
			 	function() {
					if($("#ui")[0].children[4].clientHeight > 100){
						toolMenu.css("background",
							"url('../image/toolMenu1.png')");
					}
					else{
						toolMenu.css("background",
							"url('../image/toolMenu2.png')");
					} 
				});
			
		}
		if (rightMenu) {
			rightMenu.css("background",
					"url('../image/rightMenu1.png')");

			rightMenu.toggle(function() {
					if($("#ui")[0].children[5].clientHeight > 100){
						rightMenu.css("background",
							"url('../image/rightMenu1.png')");
					}
					else{
						rightMenu.css("background",
							"url('../image/rightMenu2.png')");
					} 
				}, 
				
			 	function() {
					if($("#ui")[0].children[5].clientHeight > 100){
						rightMenu.css("background",
							"url('../image/rightMenu1.png')");
					}
					else{
						rightMenu.css("background",
							"url('../image/rightMenu2.png')");
					} 
				});
			
		}
	
		$("a[name='subMenu']").click(function(item){
			$("a[name='subMenu']").each(function(){
				$(this).css("color","#999999");
			});
			$(this).css("color","#0066FF");
		});
});
/**
 * 链式菜单的监听回调函数
 */
function ll(a){
	var s;
	s = a.split("\"");
	var item = s[7];
	if(item=="item1"){
		var fullUrl = window.location.href;
		var a=fullUrl.lastIndexOf('/');
		fullUrl=fullUrl.substring(0,a);
		var b=fullUrl.lastIndexOf('/');
		fullUrl=fullUrl.substring(0,b);
		var Url=fullUrl+'/html/VedioList.html';
		parent.poptitlewindow('videos','100','50','200','200','温度',Url,'10');
	}
	else if(item=="item2"){
		var fullUrl = window.location.href;
		var a=fullUrl.lastIndexOf('/');
		fullUrl=fullUrl.substring(0,a);
		var b=fullUrl.lastIndexOf('/');
		fullUrl=fullUrl.substring(0,b);
		var Url=fullUrl+'/html/DataDisplay.html';
		parent.popnotitle('item3msl1','400','10','200','200',Url,'10')
		}
	else if(item=="item3"){
		var fullUrl = window.location.href;
		var a=fullUrl.lastIndexOf('/');
		fullUrl=fullUrl.substring(0,a);
		var b=fullUrl.lastIndexOf('/');
		fullUrl=fullUrl.substring(0,b);
		var Url=fullUrl+'/html/testWindow1.html';
		parent.poptitlewindow('testWindow1','400','10','200','200','接收窗口',Url,'10')
		}
	else if(item=="item4"){
		var fullUrl = window.location.href;
		var a=fullUrl.lastIndexOf('/');
		fullUrl=fullUrl.substring(0,a);
		var b=fullUrl.lastIndexOf('/');
		fullUrl=fullUrl.substring(0,b);
		var Url=fullUrl+'/html/testWindow2.html';
		parent.poptitlewindow('testWindow2','700','10','200','200','发送窗口',Url,'10')
		}
	else if(item=="item5"){
		var fullUrl = window.location.href;
		var a=fullUrl.lastIndexOf('/');
		fullUrl=fullUrl.substring(0,a);
		var b=fullUrl.lastIndexOf('/');
		fullUrl=fullUrl.substring(0,b);
		var Url=fullUrl+'/html/testNewWindowFunction.html';
		parent.poptitlewindow('newWindowFunction','820','0','200','380','窗口新的调用方式',Url,'10')
		}
} 


/**
 * 根据手风琴页id和链接id。控制手风琴页的开合，以及手风琴内链接的高亮
 * 注：父界面调用的方法 
 */
function mtpp(zi,zi01){
	var nihao = document.getElementById(zi);
	$('#ui').accordion('select',nihao.name);  
	if(zi01!=0){
	var nihao01 = document.getElementById(zi01);
	$("a[name='subMenu']").each(function(){
		$(this).css("color","#999999");
	});
	$('#'+zi01).css("color","#0066FF");
	}
}