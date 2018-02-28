$(function() {
$("#objectTest01").hide();
$("#dialogBusiness01").hide();
$("#mcamera01").hide();
$("#dialogCam01").hide();
$("#conoff01").hide();
$("#zonoff0101").hide();
$("#fly01").hide();
	var location = $('#scriptM>div>div:first');
	var path = $('#scriptM>div>div:eq(2)');
	var caozuoqi = $('#scriptM>div>div:last').prev('div');
	$("div[class='panel-header accordion-header']").click(function(item){
			location.css("background",
				"url('../image/location1.png')");
			path.css("background",
				"url('../image/path1.png')");
			caozuoqi.css("background",
				"url('../image/caozuoqi1.png')");
		});
		$("div[class='panel-header accordion-header accordion-header-selected']").click(function(item){
			location.css("background",
				"url('../image/location1.png')");
			path.css("background",
				"url('../image/path1.png')");
			caozuoqi.css("background",
				"url('../image/caozuoqi1.png')");
		});
	if (location) {
			location.css("background",
					"url('../image/location2.png')");

			location.toggle(function() {
				
					if($("#scriptM")[0].children[0].clientHeight > 100){
						location.css("background",
							"url('../image/location1.png')");
					}
					else{
						location.css("background",
							"url('../image/location2.png')");
					} 
				}, 
				
			 	function() {
					if($("#scriptM")[0].children[0].clientHeight > 100){
						location.css("background",
							"url('../image/location1.png')");
					}
					else{
						location.css("background",
							"url('../image/location2.png')");
					} 
				});
			
		}
		if (path) {
			path.css("background",
					"url('../image/path1.png')");

			path.toggle(function() {
					if($("#scriptM")[0].children[1].clientHeight > 100){
						path.css("background",
							"url('../image/path1.png')");
					}
					else{
						path.css("background",
							"url('../image/path2.png')");
					} 
				}, 
				
			 	function() {
					if($("#scriptM")[0].children[1].clientHeight > 100){
						path.css("background",
							"url('../image/path1.png')");
					}
					else{
						path.css("background",
							"url('../image/path2.png')");
					} 
				});
			
		}
		if (caozuoqi) {
			caozuoqi.css("background",
					"url('../image/caozuoqi1.png')");

			caozuoqi.toggle(function() {
					if($("#scriptM")[0].children[2].clientHeight > 100){
						caozuoqi.css("background",
							"url('../image/caozuoqi1.png')");
					}
					else{
						caozuoqi.css("background",
							"url('../image/caozuoqi2.png')");
					} 
				}, 
				
			 	function() {
					if($("#scriptM")[0].children[2].clientHeight > 100){
						caozuoqi.css("background",
							"url('../image/caozuoqi1.png')");
					}
					else{
						caozuoqi.css("background",
							"url('../image/caozuoqi2.png')");
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
	$('#scriptM').accordion('select',nihao.name);  
	if(zi01!=0){
	var nihao01 = document.getElementById(zi01);
	$("a[name='subMenu']").each(function(){
		$(this).css("color","#999999");
	});
	$('#'+zi01).css("color","#0066FF");}
}