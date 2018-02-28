/**
 * 文件描述: 登录页面js。
 * 
 * @author zhanggf
 * @version 1.0 创建时间： 2012-10-30 上午8:46:07
 */
$(function() {
	$.ajaxSetup({
		cache : false
	});
	initPageSize();
	changelanguage(); // 初始化页面中英文
	// 防止在iframe里出现登录页面
	if (window.parent != window) {
		window.parent.location.reload(true);
	}
	$('[blanktext]').inputDefault();
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
		// $("#usernamelabel").html("username:") ;
		// $("#passwordlabel").html("password:") ;
		// $("#languagelabel").html("language:") ;
		document.getElementById("login").src = "image/English.png";
	} else if (language == "zh_CN") {
		// $("#usernamelabel").html("用&nbsp;&nbsp;&nbsp;&nbsp;户：") ;
		// $("#passwordlabel").html("密&nbsp;&nbsp;&nbsp;&nbsp;码：") ;
		// $("#languagelabel").html("语&nbsp;&nbsp;&nbsp;&nbsp;言：") ;
		// document.getElementById("login").src="image/LoginButtonChinese.png";
		document.getElementById("login").src = "image/Chinese.png";
	} else {
		document.getElementById("login").src = "image/eyu.png";
	}
}

/**
 * 功能描述：登录
 */
function login() {
	$("#login").attr("disabled", "disabled");// 点击登录按钮时首先将按钮置为不可用，防止重复点击；如果登录失败，重新将按钮置为可用状态
	var userid = document.getElementById("userid").value;
	var pass = document.getElementById("pass").value;
	var i18n = document.getElementById("language").value;

	$.post("../login/login.do?", {
		logintype : 0,// 0为普通用户登录（外网用户登录），1为域用户登录（内网用户登录）
		userid : userid,
		pass : pass,
		i18n : i18n
	}, function(result) {
		if (result.success) {
			saveloginlog();// 记录登录日志
			window.location = './home.htm?userName=' + userid + '&i18n=' + i18n;
		} else {
			alert(result.msg);
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