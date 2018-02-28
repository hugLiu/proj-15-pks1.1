<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="user.aspx.cs" Inherits="Discuz.Web.Redirect.user" %>
<!DOCTYPE html>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <%--<title>系统设置 - Powered by Discuz!NT</title>--%>
    <title>登录论坛</title>
</head>
<body>
    <div>正在登录论坛，请稍候...</div>
    <script type="text/javascript">
        var returnUrl = "<%=returnUrl%>";
        var count = 0;
        function jumpTo() {
            if (document.cookie.indexOf("dnt=userid=") >= 0 || count > 10) {
                top.location.href = returnUrl;
                return;
            }
            waitFor();
        }
        function waitFor() {
            setTimeout(jumpTo, 500);
        }
        jumpTo();
    </script>
</body>
</html>
