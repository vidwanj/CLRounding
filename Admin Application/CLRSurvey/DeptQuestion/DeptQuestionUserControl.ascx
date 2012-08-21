<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DeptQuestionUserControl.ascx.cs"
    Inherits="CLRSurvey.DeptQuestion.DeptQuestionUserControl" %>
<link href="../../../_layouts/CLRSurvey/main.css" rel="stylesheet" type="text/css" />
<div>
    <table width="780px" border="0" align="center" cellpadding="0" cellspacing="0" class="table-border">
        <tbody class="spacing">
            <tr>
                <td colspan="3" class="form-header">
                    Department Questions
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <div>
                        <table border="0" align="center" cellpadding="0" cellspacing="0" class="table-border">
                            <tbody class="spacing">
                                <tr>
                                    <td colspan="2" class="form-sub-header">
                                        Department Filter
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" class="sub-label-bg">
                                        Select Entity
                                    </td>
                                    <td align="left" class="text-field-bg">
                                        <asp:DropDownList ID="ddlEntity" runat="server" Width="150px" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlEntity_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" class="sub-label-bg">
                                        Select Location
                                    </td>
                                    <td align="left" class="text-field-bg">
                                        <asp:DropDownList ID="ddlLocation" runat="server" Width="150px" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td class="main-sub-title-bg">
                    Departments
                </td>
                <td >
                </td>
                <td class="main-sub-title-bg">
                    Questions
                </td>
            </tr>
            <tr>
                <td align="center" class="text-field-bg">
                    <asp:DropDownList ID="ddlDepartment" runat="server" Width="220px" AutoPostBack="true"
                        OnSelectedIndexChanged="ddlDepartment_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td class="bottom-border">
                    <div align="center" class="sub-title">
                        OR</div>
                </td>
                <td align="center" class="text-field-bg">
                    <asp:DropDownList ID="ddlQuestion" runat="server" Width="220px" AutoPostBack="true"
                        OnSelectedIndexChanged="ddlQuestion_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="5" align="center" style="height:10px" class="text-field-bg">
                    <asp:Label ID="lblDeptQuestionMsg" runat="server" Text=" " ForeColor="#33CC33" Style="font-weight: 700"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center" class="sub-label-bg">
                    <asp:Label ID="lblAvailable" runat="server" Text="Available"></asp:Label>
                </td>
                <td>
                </td>
                <td align="center" class="sub-label-bg">
                    <asp:Label ID="lblMapped" runat="server" Text="Mapped"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right" class="text-field-bg">
                    <asp:ListBox ID="lstAvailable" runat="server" Width="350px" Height="170px" SelectionMode="Multiple">
                    </asp:ListBox>
                </td>
                <td valign="middle" align="center" style="width: 50px;" class="text-field-bg">
                    <asp:ImageButton ID="ImgbtnAddItem" runat="server" ImageUrl="~/_layouts/images/CLRSurvey/right.png"
                        OnClick="ImgbtnAddItem_Click" />
                    <br />
                    <br />
                    <asp:ImageButton ID="ImgbtnRemoveItem" runat="server" ImageUrl="~/_layouts/images/CLRSurvey/left.png"
                        OnClick="ImgbtnRemoveItem_Click" />
                    <br />
                    <br />
                    <asp:ImageButton ID="ImgbtnAddAllItem" runat="server" ImageUrl="~/_layouts/images/CLRSurvey/all-right.png"
                        OnClick="ImgbtnAddAllItem_Click" />
                    <br />
                    <br />
                    <asp:ImageButton ID="ImgbtnRemoveAllItem" runat="server" ImageUrl="~/_layouts/images/CLRSurvey/all-left.png"
                        OnClick="ImgbtnRemoveAllItem_Click" />
                    <br />                    
                </td>
                <td align="center" class="text-field-bg">
                    <asp:ListBox ID="lstMapped" runat="server" Width="350px" Height="170px" SelectionMode="Multiple">
                    </asp:ListBox>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="3" class="text-field-bg">                    
                    <asp:ImageButton ID="ImgbtnSave" runat="server" ImageUrl="~/_layouts/images/CLRSurvey/save.png"
                        OnClick="ImgbtnSave_Click" />                    
                </td>
            </tr>
            <tr>
                <td colspan="3" align="center" class="text-field-bg">
                    <asp:Label ID="lblMsg" runat="server" ForeColor="Green" Font-Bold="true" Visible="false"></asp:Label>
                </td>
            </tr>
        </tbody>
    </table>
</div>
