<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CLRQuestnRespSummaryUserControl.ascx.cs" Inherits="CLRSurvey.CLRQuestnRespSummary.CLRQuestnRespSummaryUserControl" %>
<link href="../../../_layouts/CLRSurvey/main.css" rel="stylesheet" type="text/css" />
<div id="Table">
    <table cellpadding="4" border="0" align="center"class="table-border">        
        <tr>
            <td>
                <asp:GridView ID="gdvCLRQuestnSummary" runat="server" CellPadding="4" 
                    EnableModelValidation="True" ForeColor="#333333" GridLines="None" 
                    AutoGenerateColumns="False" RowStyle-Height="25px" HeaderStyle-Height="25px" FooterStyle-Height="25px">
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <Columns>
                        <asp:BoundField DataField="QuestnID" HeaderText="Que. ID">
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="QuestnText" HeaderText="Question Text">
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="200px" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Answer">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="200px" />
                            <ItemTemplate>
                            <asp:Label ID="lblRoomNo" runat="server" Text='<%# Bind("Comment") %>'></asp:Label>
                            </ItemTemplate>
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
        <tr>
            <td align="center">
                <asp:Label ID="lblerrormsg" runat="server" Text=""></asp:Label>
            </td>
        </tr>
    </table>
</div>