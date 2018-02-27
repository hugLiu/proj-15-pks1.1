<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="Jurassic.WebReport.WebForm1" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style type="text/css">
        #form1 {}
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" style="width:100%;height: 100%">
            <ContentTemplate>
                <rsweb:ReportViewer ID="ReportViewer1" runat="server" AsyncRendering="False" PageCountMode="Actual" Font-Names="Verdana" Font-Size="Small" 
                WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" BackColor="#EFF3F1"  SizeToReportContent="True" style="margin-bottom: 0px">
                    </rsweb:ReportViewer>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
