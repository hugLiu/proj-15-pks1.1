<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="header.aspx.cs" Inherits="Discuz.Web.Redirect.header" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title></title>
    <!-- PKS公共头 -->
    <link href="<%=webApiSiteUrl%>/Content/bootstrap/css/bootstrap.min.css" rel="stylesheet">
    <link href="<%=webApiSiteUrl%>/Content/styles/site.css" rel="stylesheet" />
    <link href="<%=webApiSiteUrl%>/Content/styles/layout/header/head-nav-2.css" rel="stylesheet" />
    <!--<link href="<%=webApiSiteUrl%>/Content/xvision/xvision.css" rel="stylesheet" />-->
    <!-- PKS公共头 -->
</head>
<body>
    <div id="xt_header" class="header">
        <pks:header2 ref="content"></pks:header2>
        <pks:menu :apipath="apipath" :navigation="navigation"></pks:menu>
    </div>
    <!-- PKS WebAPI脚本 -->
    <script type="text/javascript" src="<%=webApiSiteUrl%>/Scripts/jquery-1.11.1.js"></script>
    <!--<script type="text/javascript" src="<%=webApiSiteUrl%>/Scripts/moment.2.18.1/moment.min.js"></script>-->
    <script type="text/javascript" src="<%=webApiSiteUrl%>/Scripts/PKSGlobalStore.js"></script>
    <script>
        pksGlobalStore.init("<%=webApiSiteUrl%>");
    </script>
    <!-- PKS WebAPI脚本 -->
    <script type="text/javascript" src="<%=webApiSiteUrl%>/Content/vendor.js"></script>
    <script type="text/javascript" src="<%=webApiSiteUrl%>/Content/portal.js"></script>
    <script type="text/javascript" src="<%=webApiSiteUrl%>/Scripts/layout/header.js">
    </script>
</body>
</html>
