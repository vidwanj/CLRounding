<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CLR_SummaryReportUserControl.ascx.cs"
    Inherits="CLRSurvey.CLR_SummaryReport.CLR_SummaryReportUserControl" %>
<link href="../../../_layouts/CLRSurvey/main.css" rel="stylesheet" type="text/css" />
<script language="javascript" type="text/javascript">
    function openDiaWindow(roomDescr, todate) {
        var options;
        var strddlentityID = document.getElementById('<%=ddlentity.ClientID%>').value;
        var strddllocationID = document.getElementById('<%=ddllocation.ClientID%>').value;
        var strddldepartmentID = document.getElementById('<%=ddldepartment.ClientID%>').value;
        options = { url: "./CLRQuestionSummary.aspx?roomDescr=" + roomDescr + "&todate=" + todate + "&entity=" + strddlentityID + "&location=" + strddllocationID + "&dept=" + strddldepartmentID, width: 500, height: 400, title: "CLR Question Response Summary" };

        SP.UI.ModalDialog.showModalDialog(options);
    }
    function goback() {
        window.history.back();
        return false;
    }
</script>
<div id="Table" style="margin-left: 50px; margin-right: 50px">
    <table border="0" align="center" cellpadding="0" cellspacing="0" class="table-border">
        <tr>
            <td colspan="4" class="form-sub-header">
                CLR Summary Report
            </td>
        </tr>
        <tr>
            <td align="right" class="sub-label-bg">
                Select Entity
            </td>
            <td align="left" class="text-field-bg">
                <asp:DropDownList ID="ddlentity" runat="server" Width="163px" AutoPostBack="true"
                    OnSelectedIndexChanged="ddlentity_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td class="sub-label-bg">
                From Date
            </td>
            <td colspan="3" align="left" class="text-field-bg">
                <SharePoint:DateTimeControl ID="dtcfromdate" runat="server" DateOnly="True" />
                <span id="Span1" style="color: Red; vertical-align: top;" runat="server" visible="false">
                    *</span>
            </td>
        </tr>
        <tr>
            <td align="right" class="sub-label-bg">
                Select Location
            </td>
            <td align="left" class="text-field-bg">
                <asp:DropDownList ID="ddllocation" runat="server" Width="163px" AutoPostBack="true"
                    OnSelectedIndexChanged="ddllocation_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td class="sub-label-bg">
                To Date
            </td>
            <td align="left" class="text-field-bg">
                <SharePoint:DateTimeControl ID="dtctodate" runat="server" DateOnly="True" />
                <span id="Span2" style="color: Red; vertical-align: top;" runat="server" visible="false">
                    *</span>
            </td>
        </tr>
        <tr>
            <td align="right" class="sub-label-bg">
                Select Department
            </td>
            <td align="left" class="text-field-bg">
                <asp:DropDownList ID="ddldepartment" runat="server" Width="163px" AutoPostBack="true"
                    OnSelectedIndexChanged="ddldepartment_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td align="right" colspan="4" class="text-field-bg">
                <asp:ImageButton ID="ImgbtnSubmit" runat="server" ImageUrl="../../../_layouts/images/CLRSurvey/submit.png"
                    OnClick="ImgbtnSubmit_Click" />
            </td>
        </tr>
        <tr>
            <td colspan="4" align="center" class="text-field-bg">
                <asp:Label ID="lblerrormsg" runat="server" Text=" "></asp:Label>
            </td>
        </tr>
    </table>
</div>
<div id="dvgridview" align="center" style="margin-top: 5px">
    <table align="center" width="620px">
        <tbody class="spacing">
            <tr>
                <td align="right" id="tdBack" runat="server" style="padding-right: 5px;">
                </td>
            </tr>
        </tbody>
    </table>
    <table align="center" width="80%">
        <tr>
            <td align="center">
                <asp:GridView ID="gdvCLRSummary" runat="server" CellPadding="5" EnableModelValidation="True"
                    ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" OnRowDataBound="gdvCLRSummary_RowDataBound"
                    ShowFooter="True" RowStyle-Height="25px" HeaderStyle-Height="25px" FooterStyle-Height="25px">
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <Columns>
                        <asp:TemplateField HeaderText="Entity">
                            <ItemTemplate>
                                <asp:LinkButton ID="linkbtnEntity" runat="server" Text='<%# Bind("entity") %>' OnClick="linkbtnEntity_Click"></asp:LinkButton>
                            </ItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:Label ID="lblEntityTotal" runat="server" Text="Entity"></asp:Label>
                            </FooterTemplate>
                            <ItemStyle Width="100px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Location">
                            <ItemTemplate>
                                <asp:LinkButton ID="linkbtnLocation" runat="server" Text='<%# Bind("location") %>'
                                    OnClick="linkbtnLocation_Click"></asp:LinkButton>
                            </ItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:Label ID="lblLocationTotal" runat="server" Text="Location"></asp:Label>
                            </FooterTemplate>
                            <ItemStyle Width="100px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Department">
                            <ItemTemplate>
                                <asp:LinkButton ID="linkbtnDepartment" runat="server" Text='<%# Bind("department") %>'
                                    OnClick="linkbtnDepartment_Click"></asp:LinkButton>
                            </ItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:Label ID="lblDepartmentTotal" runat="server" Text="Department"></asp:Label>
                            </FooterTemplate>
                            <ItemStyle Width="100px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Room No">
                            <ItemTemplate>
                                <asp:LinkButton ID="linkbtnRoomNo" runat="server" Text=''></asp:LinkButton>
                                <asp:Label ID="lblRoomNo" runat="server" Text='<%# Bind("roomno") %>'></asp:Label>
                            </ItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:Label ID="lblRoomNoTotal" runat="server" Text="Room No"></asp:Label>
                            </FooterTemplate>
                            <ItemStyle Width="100px" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="roomcount" HeaderText="Rooms">
                            <ItemStyle Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="plannedcount" HeaderText="Planned">
                            <ItemStyle Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="completecount" HeaderText="Completed">
                            <ItemStyle Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="incompletecount" HeaderText="Incomplete">
                            <ItemStyle Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="notstarted" HeaderText="Not Started">
                            <ItemStyle Width="100px" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Percent (%)">
                            <ItemTemplate>
                                <asp:Label ID="lblPercent" runat="server" Text='<%# Eval("percentcount", "{0}%") %>'></asp:Label>
                            </ItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:Label ID="lblPercetTotal" runat="server" Text="92%"></asp:Label>
                            </FooterTemplate>
                            <ItemStyle Width="100px" />
                        </asp:TemplateField>
                    </Columns>
                    <EditRowStyle BackColor="#999999" />
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                </asp:GridView>
            </td>
        </tr>
    </table>
</div>
