<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ClrResponseReportUserControl.ascx.cs"
    Inherits="CLRSurvey.ClrResponseReport.ClrResponseReportUserControl" %>
<script src="/_layouts/CLRSurvey/jquery-1.4.1.js" type="text/javascript"></script>
<script src="/_controltemplates/Visifier/Visifire.js" type="text/javascript"></script>
<link href="../../../_layouts/CLRSurvey/main.css" rel="stylesheet" type="text/css" />
<script type="text/javascript" language="javascript">
    var CurrentDate;
    $(document).ready(function () {
        var dateTime = new Date();
        CurrentDate = dateTime.getMonth() + "/" + dateTime.getDate() + "/" + dateTime.getYear();
    });
</script>
<script type="text/javascript" language="javascript">
    function CallService() {
        var FromDate = document.getElementById('<%= dtcFromDate.Controls[0].ClientID %>').value;
        var ToDate = document.getElementById('<%= dtcToDate.Controls[0].ClientID %>').value;
        if (FromDate == "" || ToDate == "") {
            var dateTime = new Date();
            FromDate.value = CurrentDate
            ToDate.value = CurrentDate;
        }
        $.ajax({
            type: "POST",
            url: "/_layouts/CLRSurvey/ChartService.asmx/ClrResponseLoadChart",
            data: "{ 'FromDate': '" + FromDate + "' ,'ToDate': '" + ToDate + "' }",
            contentType: "application/json; charset=utf-8",
            dataType: "json", success: OnSuccess, error: OnError
        });
        return false;
    }
    function OnSuccess(data, status) {
        vChart1 = new Visifire('/_controltemplates/Visifier/SL.Visifire.Charts.xap', "MyChart0", 700, 500);
        //Set Chart data xml as string  
        vChart1.setDataXml(data.d);
        vChart1.preLoad = function (args) {
            var chart = args[0];
            for (var i = 0; i < chart.Series.length; i++) {
                chart.Series[i].MouseLeftButtonDown = function (e) {
                    alert(e.AxisXLabel); //alert(e.AxisXLabel);                
                }
            };
            // Attach Events to a Title. Title reference is received as event argument.
            chart.Titles[0].MouseLeftButtonUp = function (title) {
                alert(title.Text);
            }
        };
        // render the Chart
        vChart1.render("ChartDiv"); //display chart in this div...
    }
    function OnError(request, status, error) {
        alert(request.statusText);
    }    
</script>
<table border="0" align="center" cellpadding="0" cellspacing="0" class="table-border">
    <tbody class="spacing">
        <tr>
        <td colspan="5" class="form-sub-header">
            Followup Report
        </td>
    </tr>
        <tr>
        <td class="sub-label-bg">
            From Date
        </td>
        <td class="text-field-bg">
            <SharePoint:DateTimeControl ID="dtcFromDate" DateOnly="true" runat="server" />
        </td>
        <td class="text-field-bg">
        </td>
        <td class="sub-label-bg">
            To Date
        </td>
        <td class="text-field-bg">
            <SharePoint:DateTimeControl ID="dtcToDate" DateOnly="true" runat="server" />
        </td>
    </tr>
        <tr>
        <td align="center" colspan="5" class="text-field-bg">
            <asp:ImageButton ID="ImgbtnSubmit" runat="server" ImageUrl="../../../_layouts/images/CLRSurvey/submit.png"
                  OnClientClick="CallService(); return false;"/>
        </td>
    </tr>
    </tbody>
</table>
<div id="ChartDiv" align="center" style="width: 750; height: 500px;">
    <script type="text/javascript" language="javascript">
        CallService();
    </script>
</div>
