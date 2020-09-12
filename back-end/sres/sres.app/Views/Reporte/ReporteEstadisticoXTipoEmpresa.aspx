<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReporteEstadisticoXTipoEmpresa.aspx.cs" Inherits="sres.app.Views.Reporte.WebForm1" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="frmReporte" runat="server">
        <div class="row">
            <asp:DropDownList ID="ddlConvocatoria" runat="server" DataTextField="NOMBRE" DataValueField="ID_CONVOCATORIA"></asp:DropDownList>
            <asp:Button ID="btnConsultar" runat="server" Text="Button" OnClick="btnConsultar_Click" />
        </div>
        <div class="row">
            <rsweb:ReportViewer ID="rpwReporte" runat="server"></rsweb:ReportViewer>
        </div>
    </form>
</body>
</html>
