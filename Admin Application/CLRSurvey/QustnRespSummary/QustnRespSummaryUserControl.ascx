<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QustnRespSummaryUserControl.ascx.cs"
    Inherits="CLRSurvey.QustnRespSummary.QustnRespSummaryUserControl" %>
<link href="../../../_layouts/CLRSurvey/main.css" rel="stylesheet" type="text/css" />
<script src="/_layouts/CLRSurvey/jquery-1.4.1.min.js" type="text/javascript"></script>
<script type="text/javascript" src="/_controltemplates/Visifier/Visifire.js"></script>
<script type="text/javascript" language="javascript">
    function CallService(MethodName, entityID, locatnID, deptID, roomID, responstype, fromdate, enddate) {       
            $.ajax({
                type: "POST",
                url: MethodName,
                data: "{ 'entityID': '" + entityID + "', 'locatnID':'" + locatnID + "', 'deptID':'" + deptID + "','roomID':'" + roomID + "','responstype':'" + responstype + "','fromdate':'" + fromdate + "','enddate':'" + enddate + "'}",
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
        debugger;
        alert(request.statusText);
    }


</script>
<div id="Table" align="center">
    <table style="width: 790px" border="0" align="center" cellpadding="0" cellspacing="0"
        class="table-border">
        <tbody class="spacing">
            <tr>
                <td colspan="6" class="form-sub-header">
                    Question Response Summary
                </td>
            </tr>
            <tr>
                <td class="text-field-bg">
                </td>
                <td align="right" class="sub-label-bg">
                    Entity
                </td>
                <td align="left" colspan="4" class="text-field-bg">
                    <asp:DropDownList ID="ddlentity" runat="server" Width="163px" AutoPostBack="true"
                        OnSelectedIndexChanged="ddlentity_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td align="left" class="sub-label-bg">
                    Response Type
                </td>
                <td align="right" class="sub-label-bg">
                    Location
                </td>
                <td align="left" class="text-field-bg">
                    <asp:DropDownList ID="ddllocation" runat="server" Width="163px" AutoPostBack="true"
                        OnSelectedIndexChanged="ddllocation_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td align="right" class="sub-label-bg">
                    From Date
                </td>
                <td class="text-field-bg">
                    <SharePoint:DateTimeControl ID="frmDate" runat="server" DateOnly="true" />
                </td>
                <td class="text-field-bg">
                    <span id="Span2" style="color: Red; vertical-align: top;" runat="server">*</span>&nbsp;
                </td>
            </tr>
            <tr>
                <td align="center" class="text-field-bg">
                    <asp:DropDownList ID="ddlresponsetype" runat="server" Width="163px" AutoPostBack="true">
                    </asp:DropDownList>
                </td>
                <td align="right" class="sub-label-bg">
                    Department
                </td>
                <td align="left" class="text-field-bg">
                    <asp:DropDownList ID="ddldepartment" runat="server" Width="163px" AutoPostBack="true"
                        OnSelectedIndexChanged="ddldepartment_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td align="right" class="sub-label-bg">
                    To Date
                </td>
                <td class="text-field-bg">
                    <SharePoint:DateTimeControl ID="toDate" runat="server" DateOnly="true" />
                </td>
                <td class="text-field-bg">
                    <span id="Span1" style="color: Red; vertical-align: top;" runat="server">*</span>&nbsp;
                </td>
            </tr>
            <tr>
                <td class="text-field-bg">
                </td>
                <td align="right" class="sub-label-bg">
                    Room No.
                </td>
                <td align="left" colspan="5" class="text-field-bg">
                    <asp:DropDownList ID="ddlroom" runat="server" Width="163px" AutoPostBack="true">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="6" align="center" class="text-field-bg">
                    <asp:ImageButton ID="ImgbtnStatus" runat="server" OnClientClick="callServiceOnBtn(); return false;" ImageUrl="~/_layouts/images/CLRSurvey/get-report.png" />
                </td>
            </tr>
            <tr>
                <td colspan="6" align="center" class="text-field-bg">
                    <asp:Label ID="lblerrormsg" runat="server" Text=""></asp:Label>
                </td>
            </tr>
        </tbody>
    </table>
</div>
<script type="text/javascript" language="javascript">
    function callServiceOnBtn() {
        var entityID = document.getElementById('<%= ddlentity.ClientID %>').value;
        var responstype = document.getElementById('<%= ddlresponsetype.ClientID %>').value;
        var locationID = document.getElementById('<%= ddllocation.ClientID %>').value;
        var deptID = document.getElementById('<%= ddldepartment.ClientID %>').value;
        var roomID = document.getElementById('<%= ddlroom.ClientID %>').value;
        var fromdate = document.getElementById('<%=frmDate.Controls[0].ClientID%>').value;
        var todate = document.getElementById('<%=toDate.Controls[0].ClientID%>').value;
        if (fromdate != "" && todate != "") {
            CallService("/_layouts/CLRSurvey/ChartService.asmx/LoadQuestionResponceChart", entityID, locationID, deptID, roomID, responstype, fromdate, todate);
        }
        else {
            alert("Dates are required");
            return false;
        }
    }
                </script>
<div id="ChartDiv" align="center">
    <%--<table>
        <tr>
            <td>
            </td>
        </tr>
        <tr>
            <td>
                
            </td>
        </tr>
    </table>--%>
</div>
