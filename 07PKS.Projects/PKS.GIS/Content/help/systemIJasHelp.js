$(function() {
$("#onoff01").hide();
$("#dmp01").hide();
$("#zuobiao01").hide();
$("#setSun01").hide();
$("#weather01").hide();
$("#widget01").hide();
$("#widgetstatus01").hide();
$("#setHintbar01").hide();
$("#setTool01").hide();
$("#setControlpanel01").hide();
$("#setFocuscross01").hide();
$("#dialogLanguage01").hide();
$("#setLanguage01").hide();
$("#setLonAndLatLine01").hide();


	var yunxing = $('#syst>div>div:first');
	var peizhi = $('#syst>div>div:eq(2)');
	var weather = $('#syst>div>div:eq(4)');
	var guajian = $('#syst>div>div:eq(6)');
	var internation = $('#syst>div>div:last').prev('div');
	$("div[class='panel-header accordion-header']").click(function(item){
			yunxing.css("background",
				"url('../image/yunxing1.png')");
			peizhi.css("background",
				"url('../image/peizhi1.png')");
			weather.css("background",
				"url('../image/weather1.png')");
			guajian.css("background",
				"url('../image/guajian1.png')");
			internation.css("background",
				"url('../image/internation1.png')");
		});
		$("div[class='panel-header accordion-header accordion-header-selected']").click(function(item){
			yunxing.css("background",
				"url('../image/yunxing1.png')");
			peizhi.css("background",
				"url('../image/peizhi1.png')");
			weather.css("background",
				"url('../image/weather1.png')");
			guajian.css("background",
				"url('../image/guajian1.png')");
			internation.css("background",
				"url('../image/internation1.png')");
		});
	if (yunxing) {
			yunxing.css("background",
					"url('../image/yunxing2.png')");

			yunxing.toggle(function() {
				
					if($("#syst")[0].children[0].clientHeight > 100){
						yunxing.css("background",
							"url('../image/yunxing1.png')");
					}
					else{
						yunxing.css("background",
							"url('../image/yunxing2.png')");
					} 
				}, 
				
			 	function() {
					if($("#syst")[0].children[0].clientHeight > 100){
						yunxing.css("background",
							"url('../image/yunxing1.png')");
					}
					else{
						yunxing.css("background",
							"url('../image/yunxing2.png')");
					} 
				});
			
		}
		if (peizhi) {
			peizhi.css("background",
					"url('../image/peizhi1.png')");

			peizhi.toggle(function() {
					if($("#syst")[0].children[1].clientHeight > 100){
						peizhi.css("background",
							"url('../image/peizhi1.png')");
					}
					else{
						peizhi.css("background",
							"url('../image/peizhi2.png')");
					} 
				}, 
				
			 	function() {
					if($("#syst")[0].children[1].clientHeight > 100){
						peizhi.css("background",
							"url('../image/peizhi1.png')");
					}
					else{
						peizhi.css("background",
							"url('../image/peizhi2.png')");
					} 
				});
			
		}
		if (weather) {
			weather.css("background",
					"url('../image/weather1.png')");

			weather.toggle(function() {
					if($("#syst")[0].children[2].clientHeight > 100){
						weather.css("background",
							"url('../image/weather1.png')");
					}
					else{
						weather.css("background",
							"url('../image/weather2.png')");
					} 
				}, 
				
			 	function() {
					if($("#syst")[0].children[2].clientHeight > 100){
						weather.css("background",
							"url('../image/weather1.png')");
					}
					else{
						weather.css("background",
							"url('../image/weather2.png')");
					} 
				});
			
		}
		if (guajian) {
			guajian.css("background",
					"url('../image/guajian1.png')");

			guajian.toggle(function() {
					if($("#syst")[0].children[3].clientHeight > 100){
						guajian.css("background",
							"url('../image/guajian1.png')");
					}
					else{
						guajian.css("background",
							"url('../image/guajian2.png')");
					} 
				}, 
				
			 	function() {
					if($("#syst")[0].children[3].clientHeight > 100){
						guajian.css("background",
							"url('../image/guajian1.png')");
					}
					else{
						guajian.css("background",
							"url('../image/guajian2.png')");
					} 
				});
			
		}
		if (internation) {
			internation.css("background",
					"url('../image/internation1.png')");

			internation.toggle(function() {
					if($("#syst")[0].children[4].clientHeight > 100){
						internation.css("background",
							"url('../image/internation1.png')");
					}
					else{
						internation.css("background",
							"url('../image/internation2.png')");
					} 
				}, 
				
			 	function() {
					if($("#syst")[0].children[4].clientHeight > 100){
						internation.css("background",
							"url('../image/internation1.png')");
					}
					else{
						internation.css("background",
							"url('../image/internationg2.png')");
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
 * 根据手风琴页id和链接id。控制手风琴页的开合，以及手风琴内链接的高亮
 * 注：父界面调用的方法
 */
function mtpp(zi,zi01){
	var nihao = document.getElementById(zi);
	$('#syst').accordion('select',nihao.name);  
	if(zi01!=0){
	var nihao01 = document.getElementById(zi01);
	$("a[name='subMenu']").each(function(){
		$(this).css("color","#999999");
	});
	$('#'+zi01).css("color","#0066FF");}
}