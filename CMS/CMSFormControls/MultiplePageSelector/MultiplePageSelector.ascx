<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSApp.CMSFormControls.MultiplePageSelector.MultiplePageSelector" CodeBehind="MultiplePageSelector.ascx.cs" %>
<%@ Register Src="~/CMSFormControls/MultiplePageSelector/MultiplePageSelectorPager.ascx" TagPrefix="XperienceCommunity" TagName="MultiplePageSelectorPager" %>

<style>
    .table .unigrid-actions {
        width: 1px;
    }

    .table th, .table td {
        border: 0;
    }

    .cms-bootstrap .table .icon-only {
        text-decoration: none;
    }
</style>
<div class="cms-bootstrap">
    <cms:CMSUpdatePanel ID="pnlUpdateHidden" runat="server">
        <ContentTemplate>
            <div class="control-group-inline">
                <cms:CMSButton ID="btnSelect" runat="server" ButtonStyle="Default" />
                <cms:CMSTextBox ID="txtGuid" runat="server" AutoPostBack="true" CssClass="Hidden" />
                <cms:LocalizedLabel ID="lblGuid" runat="server" EnableViewState="false" Display="false" AssociatedControlID="txtGuid" ResourceString="development_formusercontrol_edit.lblforguid" />
            </div>
            <br />
            <asp:GridView ID="gvNodes" runat="server" AutoGenerateColumns="False" AllowPaging="True" AllowCustomPaging="False" PageSize="10"
                CssClass="table table-hover _nodivs" BorderWidth="0"
                OnRowDataBound="gvNodes_OnRowDataBound" OnRowCommand="gvNodes_OnRowCommand">
                <HeaderStyle CssClass="unigrid-head"></HeaderStyle>
                <Columns>
                    <asp:TemplateField HeaderText="Actions">
                        <ItemStyle CssClass="unigrid-actions" />
                        <ItemTemplate>
                            <asp:LinkButton runat="server" CommandName="Remove" CommandArgument='<%# Eval("NodeGuid") %>' CssClass="btn-unigrid-action icon-style-critical icon-only btn-icon btn"
                                OnClientClick=<%# string.Format("return confirm('{0}');", ResHelper.GetString("general.confirmdelete")) %>>
                        <i class="icon-bin" aria-hidden="true"></i>
                        <cms:LocalizedLabel runat="server" ResourceString="General.Delete" CssClass="sr-only"/>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" CommandName="MoveUp" CommandArgument='<%# Eval("NodeGuid") %>' CssClass="btn-unigrid-action icon-only btn-icon btn">
                        <i class="icon-chevron-up" aria-hidden="true"></i>
                        <cms:LocalizedLabel runat="server" ResourceString="General.Up" CssClass="sr-only"/>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" CommandName="MoveDown" CommandArgument='<%# Eval("NodeGuid") %>' CssClass="btn-unigrid-action icon-only btn-icon btn">
                        <i class="icon-chevron-down" aria-hidden="true"></i>
                        <cms:LocalizedLabel runat="server" ResourceString="General.Down" CssClass="sr-only"/>
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Document Name" DataField="DocumentName" />
                    <asp:BoundField HeaderText="Document Type" DataField="DocumentType" />
                    <asp:BoundField HeaderText="Node Alias Path" DataField="NodeAliasPath" />
                </Columns>
                <PagerSettings Visible="False"></PagerSettings>
            </asp:GridView>
            <XperienceCommunity:MultiplePageSelectorPager ID="pgrNodes" runat="server" />
            <asp:HiddenField ID="hdnValue" runat="server" />
        </ContentTemplate>
    </cms:CMSUpdatePanel>
</div>
