<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CLRStatusSummaryUserControl.ascx.cs"
    Inherits="CLRSurvey.CLRStatusSummary.CLRStatusSummaryUserControl" %>
<script src="/_layouts/CLRSurvey/jquery-1.4.1.js" type="text/javascript"></script>
<script src="/_controltemplates/Visifier/Visifire.js" type="text/javascript"></script>
<link href="../../../_layouts/CLRSurvey/main.css" rel="stylesheet" type="text/css" />
<script type="text/javascript" language="javascript">

    function CallService(MethodName, entity, frmDate, toDate, LocVal) {
        $.ajax({
            type: "POST",
            url: MethodName,
            data: "{ 'entity': '" + entity + "', 'frmDate':'" + frmDate + "', 'toDate':'" + toDate + "','LocVal':'" + LocVal + "'}",
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
<div>
    <table border="0" align="center" cellpadding="0" cellspacing="0" class="table-border">
        <tbody class="spacing">
            <tr>
                <td colspan="5" class="form-sub-header">
                    CLR Status Summary
                </td>
            </tr>
            <tr>
                <td align="right" class="sub-label-bg">
                    Select Entity
                </td>
                <td align="left" class="text-field-bg">
                    <asp:DropDownList ID="ddlentity" runat="server" Width="163px" AutoPostBack="true"
                        OnSelectedIndexChanged="ddlentity_SelectedIndexChanged">
                        <asp:ListItem Value="0">--Select--</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td class="sub-label-bg" align="right">
                    From Date
                </td>
                <td class="text-field-bg">
                    <SharePoint:DateTimeControl ID="frmDate" runat="server" DateOnly="true" IsRequiredField="true" />
                    <asp:CompareValidator ID="CompValFrmDate" runat="server" ForeColor="Red" ControlToValidate="frmDate$frmDateDate"
                        Type="Date" Operator="DataTypeCheck" ErrorMessage="Please enter valid date" Display="Dynamic"></asp:CompareValidator>
                </td>
                <td class="text-field-bg">
                    <span id="Span1" style="color: Red; vertical-align: top;" runat="server">*</span>&nbsp;
                </td>
            </tr>
            <tr>
                <td align="right" class="sub-label-bg">
                    Select Location
                </td>
                <td align="left" class="text-field-bg">
                    <asp:DropDownList ID="ddllocation" runat="server" Width="163px">
                        <asp:ListItem Value="0">--Select--</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td class="sub-label-bg" align="right">
                    To Date
                </td>
                <td class="text-field-bg">
                    <SharePoint:DateTimeControl ID="toDate" runat="server" DateOnly="true" IsRequiredField="True" />
                    <asp:CompareValidator ID="CompValToDate" runat="server" ForeColor="Red" ControlToValidate="toDate$toDateDate"
                        Type="Date" Operator="DataTypeCheck" ErrorMessage="Please enter valid date" SetFocusOnError="true"
                        Display="Dynamic"></asp:CompareValidator>
                </td>
                <td class="text-field-bg">
                    <span id="Span2" style="color: Red; vertical-align: top;" runat="server">*</span>&nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="5" align="center" class="text-field-bg">
                    <asp:ImageButton ID="ImgbtnStatus" runat="server" OnClientClick="callServiceOnBtn(); return false;" ImageUrl="~/_layouts/images/CLRSurvey/get-report.png" />
                </td>
            </tr>
            <tr>
                <td class="text-field-bg" align="center" colspan="5">
                    <asp:Label ID="lblMsg" runat="server" Visible="false"></asp:Label>
                </td>
            </tr>
        </tbody>
    </table>
</div>
<div id="ChartDiv" align="center" style="width: 750; height: 500px; float: none;">
    <script type="text/javascript" language="javascript">
        function callServiceOnBtn() {

            //var ParamObject = new Array();
            var entval = document.getElementById('<%= ddlentity.ClientID %>').value;
            var selectedFrmDate = document.getElementById('<%=frmDate.Controls[0].ClientID%>').value;
            var selectedToDate = document.getElementById('<%=toDate.Controls[0].ClientID%>').value;
            var LocVal = document.getElementById('<%= ddllocation.ClientID %>').value;
            if (!selectedFrmDate || !selectedToDate) {
                alert("Dates are required");
                return false;
            }
            else if (selectedFrmDate.length == 0 || selectedToDate.length == 0) {
                alert("one or more Date values empty");
                return false;
            }

            else {
                CallService("/_layouts/CLRSurvey/ChartService.asmx/LoadChart", entval, selectedFrmDate, selectedToDate, LocVal);
            }

        }
        
    </script>
</div>
