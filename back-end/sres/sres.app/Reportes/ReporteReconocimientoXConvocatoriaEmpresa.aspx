<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReporteReconocimientoXConvocatoriaEmpresa.aspx.cs" Inherits="sres.app.Reportes.ReporteReconocimientoXConvocatoriaEmpresa" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

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
                    <label>CONVOCATORIA:</label>
                    <asp:DropDownList ID="ddlConvocatoria" runat="server" DataTextField="NOMBRE" DataValueField="ID_CONVOCATORIA" CssClass="form-control"></asp:DropDownList>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-5">
                <div class="form-group">
                    <label>EMPRESA:</label>
                    <asp:DropDownList ID="ddlEmpresa" runat="server" DataTextField="RAZON_SOCIAL" DataValueField="ID_INSTITUCION" CssClass="form-control"></asp:DropDownList>
                </div>
            </div>
            <div class="col-sm-7">
                <div class="form-group">
                    <asp:Button ID="btnConsultar" runat="server" Text="Consultar" OnClick="btnConsultar_Click" CssClass="btn btn-success" />
                </div>
            </div>
        </div>
        <div class="row">
            <rsweb:ReportViewer ID="rpwReporte" runat="server" Style="width: 100%" Visible="False" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt">
                <LocalReport EnableExternalImages="True" ReportPath="App_Data\Reportes\rptReconocimientoEmpresa.rdlc">
                </LocalReport>
            </rsweb:ReportViewer>
        </div>
    </form>
</body>
</html>
