/**
 * 
 * 文件描述: 登录页面js。
 * 
 * @author zhanggf
 * @version 1.0 创建时间： 2012-10-30 上午8:46:07
 */

$(function() {
	$.ajaxSetup({
		cache:false
	});
	initPageSize();
	changelanguage(); // 初始化页面中英文
	// 防止在iframe里出现登录页面
	if (window.parent != window) {
		window.parent.location.reload(true);
	}
});

function initPageSize() {
	var height = document.documentElement.clientHeight;
	document.getElementById("dl_main").style.height = height;
}

/**
 * 功能描述：根据用户选择的语言，改变页面元素语言
 */
function changelanguage() {
	var language = document.getElementById("language").value;
	if (language == "en") {
		$("#usernamelabel").html("username:");
		$("#passwordlabel").html("password:");
		$("#languagelabel").html("language:");
		document.getElementById("login").src = "image/LoginButtonEnglish.png";
	} else {
		$("#usernamelabel").html("用&nbsp;&nbsp;&nbsp;&nbsp;户：");
		$("#passwordlabel").html("密&nbsp;&nbsp;&nbsp;&nbsp;码：");
		$("#languagelabel").html("语&nbsp;&nbsp;&nbsp;&nbsp;言：");
		document.getElementById("login").src = "image/LoginButtonChinese.png";

	}
}

/**
 * 功能描述：登录
 */
function login() {
	$("#login").attr("disabled", "disabled");// 点击登录按钮时首先将按钮置为不可用，防止重复点击；如果登录失败，重新将按钮置为可用状态
	var userid = $("#userid").val();
	var pass = $("#pass").val();
	var i18n = $("#language").val();

	$.post("../login/login.do?", {
		logintype : 0,// 0为普通用户登录（外网用户登录），1为域用户登录（内网用户登录）
		userid : userid,
		pass : pass,
		i18n : i18n
	}, function(result) {
		if (result.success) {
			saveloginlog();// 记录登录日志
			//alert(getCookie("JSESSIONID"))
			window.location = './home.htm?userName=' + userid + '&i18n=' + i18n;
			
			// 采用弹出窗口的方法解决让窗口不显示状态栏菜单栏工具栏等，目前没有其他办法实现。
				/*if(window.name!='jasframework'){
					var win = window.open('./home.htm?userName='+userid + '&i18n=' + i18n, 'jasframework','fullscreen=yes alwaysRaised=yes toolbar=no,menubar=no,scrollbars=no,resizable=yes,location=yes,status=no',true);
					win.moveTo(0, 0);
					  win.resizeTo(window.screen.width, window.screen.height);
					  window.opener=null;
					  window.open('','_self');
					  window.close();
				}else{
					window.location = './home.htm?userName=' + userid + '&i18n=' + i18n;
				}*/
				
		} else {
			alert(result.msg);
			$("#login").removeAttr("disabled");
		}
	}, 'json');
}

/**
 * 功能描述：记录登录日志
 */
function saveloginlog() {
	$.post("../login/saveloginlog.do?", {}, function(result) {
	}, 'json');
}
