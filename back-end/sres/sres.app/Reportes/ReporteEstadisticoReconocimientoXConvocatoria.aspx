<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReporteEstadisticoReconocimientoXConvocatoria.aspx.cs" Inherits="sres.app.Reportes.ReporteEstadisticoReconocimientoXConvocatoria" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link rel="stylesheet" href="~/Assets/css/style.css" />
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div class="row">
            <div class="col-sm-5">
                <div class="form-group">
                    <asp:DropDownList ID="ddlConvocatoria" runat="server" DataTextField="NOMBRE" DataValueField="ID_CONVOCATORIA" CssClass="form-control"></asp:DropDownList>
                </div>
            </div>
            <div class="col-sm-7">
                <div class="form-group">
                    <asp:Button ID="btnConsultar" runat="server" Text="Consultar" OnClick="btnConsultar_Click" CssClass="btn btn-success" />
                </div>
            </div>
        </div>
        <div class="row" style="overflow-y: scroll;">
            <rsweb:ReportViewer ID="rpwReporte" runat="server" Style="width: 100%" Visible="false">
                <LocalReport ReportPath="App_Data/Reportes/rptReporteEstadisticoReconocimientoXConvocatoria.rdlc"></LocalReport>
            </rsweb:ReportViewer>
        </div>
    </form>
</body>
</html>
