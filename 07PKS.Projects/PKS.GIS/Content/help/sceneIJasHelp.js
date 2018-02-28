
$(function() {
$("#dialogObject101").hide();
$("#vector101").hide();
$("#vectorCon01").hide();
$("#dd101").hide();
$("#objectTest301").hide();
$("#selectBall01").hide();
$("#modelName01").hide();
$("#dialogObject201").hide();
$("#dialogObject301").hide();
$("#drawPointMsl11101").hide();
$("#drawLineMsl11101").hide();
$("#drawSurfaceMsl11101").hide();
$("#seePanel12101").hide();
$("#hua11101").hide();
$("#objectTest1001").hide();
$("#objectTest1101").hide();
$("#objectTest1201").hide();
$("#objectTest1301").hide();
$("#objectTest1401").hide();
$("#objectTest1501").hide();
$("#objectTest1601").hide();
$("#objectTest1701").hide();
$("#runPanel01").hide();
$("#updatePanel01").hide();
$("#updateTip01").hide();
$("#updateText01").hide();
$("#setPanel01").hide();
$("#tipPanl01").hide();
$("#textPanl01").hide();
$("#htmlPanl01").hide();
$("#ee101").hide();
$("#qq301").hide();
$("#modelpao01").hide();
$("#modelchai01").hide();
$("#dialogObject401").hide();
$("#objectTest701").hide();
$("#hh101").hide();
$("#objectTest201").hide();
$("#highlight01").hide();
$("#objectTest801").hide();
$("#enterTest01").hide();
$("#dialogBusiness01").hide();
$("#enterTest201").hide();
$("#dialogBusiness101").hide();
$("#dd201").hide();
$("#qq401").hide();
$("#objectTest11PL01").hide();
$("#objectMove01").hide();
$("#newModelpao01").hide();
$("#popBusinessGroup01").hide();
	
	var jiegou = $('#scene>div>div:first');
	//var objectSelect = $('#scene>div>div:eq(2)');
	//var shuxing = $('#scene>div>div:eq(4)');
	//var visualObject = $('#scene>div>div:eq(6)');
	//var objectManager = $('#scene>div>div:eq(8)');
	var objectCreate = $('#scene>div>div:eq(2)');
	var objectDel = $('#scene>div>div:eq(4)');
	var objectShuxing = $('#scene>div>div:eq(6)');
	var state = $('#scene>div>div:last').prev('div');
	$("div[class='panel-header accordion-header']").click(function(item){
			jiegou.css("background",
				"url('../image/jiegou1.png')");
			/*objectSelect.css("background",
				"url('../image/objectSelect1.png')");
			shuxing.css("background",
				"url('../image/shuxing1.png')");
			visualObject.css("background",
			"url('../image/visualObject1.png')");
			objectManager.css("background",
			"url('../image/objectManager1.png')");*/
			objectCreate.css("background",
				"url('../image/objectCreate1.png')");
			objectDel.css("background",
				"url('../image/objectDel1.png')");
			objectShuxing.css("background",
				"url('../image/objectShuxing1.png')");
			state.css("background",
				"url('../image/state1.png')");
		});
		$("div[class='panel-header accordion-header accordion-header-selected']").click(function(item){
			jiegou.css("background",
				"url('../image/jiegou1.png')");
			/*objectSelect.css("background",
				"url('../image/objectSelect1.png')");
			shuxing.css("background",
				"url('../image/shuxing1.png')");
			visualObject.css("background",
			"url('../image/visualObject1.png')");
			objectManager.css("background",
			"url('../image/objectManager1.png')");*/
			objectCreate.css("background",
				"url('../image/objectCreate1.png')");
			objectDel.css("background",
				"url('../image/objectDel1.png')");
			objectShuxing.css("background",
				"url('../image/objectShuxing1.png')");
			state.css("background",
				"url('../image/state1.png')");
		});
	if (jiegou) {
			jiegou.css("background",
					"url('../image/jiegou2.png')");

			jiegou.toggle(function() {
				
					if($("#scene")[0].children[0].clientHeight > 100){
						jiegou.css("background",
							"url('../image/jiegou1.png')");
					}
					else{
						jiegou.css("background",
							"url('../image/jiegou2.png')");
					} 
				}, 
				
			 	function() {
					if($("#scene")[0].children[0].clientHeight > 100){
						jiegou.css("background",
							"url('../image/jiegou1.png')");
					}
					else{
						jiegou.css("background",
							"url('../image/jiegou2.png')");
					} 
				});
			
		}
		if (objectCreate) {
			objectCreate.css("background",
					"url('../image/objectCreate1.png')");

			objectCreate.toggle(function() {
					if($("#scene")[0].children[1].clientHeight > 100){
						objectCreate.css("background",
							"url('../image/objectCreate1.png')");
					}
					else{
						objectCreate.css("background",
							"url('../image/objectCreate2.png')");
					} 
				}, 
				
			 	function() {
					if($("#scene")[0].children[1].clientHeight > 100){
						objectCreate.css("background",
							"url('../image/objectCreate1.png')");
					}
					else{
						objectCreate.css("background",
							"url('../image/objectCreate2.png')");
					} 
				});
			
		}
		if (objectDel) {
			objectDel.css("background",
					"url('../image/objectDel1.png')");

			objectDel.toggle(function() {
					if($("#scene")[0].children[2].clientHeight > 100){
						objectDel.css("background",
							"url('../image/objectDel1.png')");
					}
					else{
						objectDel.css("background",
							"url('../image/objectDel2.png')");
					} 
				}, 
				
			 	function() {
					if($("#scene")[0].children[2].clientHeight > 100){
						objectDel.css("background",
							"url('../image/objectDel1.png')");
					}
					else{
						objectDel.css("background",
							"url('../image/objectDel2.png')");
					} 
				});
			
		}
		if (objectShuxing) {
			objectShuxing.css("background",
					"url('../image/objectShuxing1.png')");

			objectShuxing.toggle(function() {
					if($("#scene")[0].children[3].clientHeight > 100){
						objectShuxing.css("background",
							"url('../image/objectShuxing1.png')");
					}
					else{
						objectShuxing.css("background",
							"url('../image/objectShuxing2.png')");
					} 
				}, 
				
			 	function() {
					if($("#scene")[0].children[3].clientHeight > 100){
						objectShuxing.css("background",
							"url('../image/objectShuxing1.png')");
					}
					else{
						objectShuxing.css("background",
							"url('../image/objectShuxing2.png')");
					} 
				});
			
		}
		if (state) {
			state.css("background",
					"url('../image/state1.png')");

			state.toggle(function() {
					if($("#scene")[0].children[4].clientHeight > 100){
						state.css("background",
							"url('../image/state1.png')");
					}
					else{
						state.css("background",
							"url('../image/state2.png')");
					} 
				}, 
				
			 	function() {
					if($("#scene")[0].children[4].clientHeight > 100){
						state.css("background",
							"url('../image/state1.png')");
					}
					else{
						state.css("background",
							"url('../image/state2.png')");
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
	$('#scene').accordion('select',nihao.name);
	if(zi01!=0){
	var nihao01 = document.getElementById(zi01);
	$("a[name='subMenu']").each(function(){
		$(this).css("color","#999999");
	});
	$('#'+zi01).css("color","#0066FF");}
}