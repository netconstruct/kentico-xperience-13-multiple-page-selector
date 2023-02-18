<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MultiplePageSelectorPager.ascx.cs" Inherits="CMSApp.CMSFormControls.MultiplePageSelector.MultiplePageSelectorPager" ViewStateMode="Enabled" %>
<%@ Register TagPrefix="cms" Namespace="CMS.Base.Web.UI" Assembly="CMS.Base.Web.UI, Version=13.0.0.0, Culture=neutral, PublicKeyToken=834b12a258f213f9" %>

<asp:Panel ID="pnlPagination" runat="server" class="pagination" Visible="False" ViewStateMode="Enabled">
    <ul class="pagination-list">
        <li>
            <asp:LinkButton ID="hlPreviousPage" runat="server" OnClick="hlPreviousPage_OnClick">
                    <i class="icon-chevron-left" aria-hidden="true"></i>
                    <span class="sr-only">Previous page</span>
            </asp:LinkButton>
        </li>
        <asp:PlaceHolder ID="plcPreviousGroup" runat="server">
            <li>
                <asp:LinkButton ID="hlPreviousGroup" runat="server" Text="..." OnClick="hlPreviousGroup_OnClick" />
            </li>
        </asp:PlaceHolder>
        <asp:Repeater ID="rptPager" OnItemDataBound="rptPager_OnItemDataBound" OnItemCommand="rptPager_ItemCommand" runat="server">
            <ItemTemplate>
                <asp:MultiView ID="mvPage" runat="server" ActiveViewIndex="0">
                    <asp:View runat="server">
                        <li>
                            <asp:LinkButton Text='<%# Container.DataItem %>' CommandArgument="<%# Container.DataItem %>" runat="server" />
                        </li>
                    </asp:View>
                    <asp:View runat="server">
                        <li class="active">
                            <span><%# Container.DataItem %></span>
                        </li>
                    </asp:View>
                </asp:MultiView>
            </ItemTemplate>
        </asp:Repeater>
        <asp:PlaceHolder ID="plcNextGroup" runat="server">
            <li>
                <asp:LinkButton ID="hlNextGroup" runat="server" Text="..." OnClick="hlNextGroup_OnClick" />
            </li>
        </asp:PlaceHolder>
        <li>
            <asp:LinkButton ID="hlNextPage" runat="server" OnClick="hlNextPage_OnClick">
                    <i class="icon-chevron-right" aria-hidden="true"></i>
                    <span class="sr-only">Next page</span>
            </asp:LinkButton>
        </li>
    </ul>
    <div class="pagination-pages">
        <cms:LocalizedLabel runat="server" ResourceString="content.ui.page" AssociatedControlClientID="txtCurrentPage" />
        <asp:TextBox ID="txtCurrentPage" runat="server" CssClass="form-control" AutoPostBack="True" OnTextChanged="txtCurrentPage_OnTextChanged" />
        <span class="pages-max">/
                <asp:Literal ID="litMaxPages" runat="server" /></span>
    </div>
    <div class="pagination-items-per-page">
        <cms:LocalizedLabel runat="server" ResourceString="unigrid.itemsperpage" AssociatedControlID="ddlItemsPerPage" />
        <asp:DropDownList ID="ddlItemsPerPage" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlItemsPerPage_OnSelectedIndexChanged">
            <asp:ListItem>10</asp:ListItem>
            <asp:ListItem>25</asp:ListItem>
            <asp:ListItem>50</asp:ListItem>
            <asp:ListItem>100</asp:ListItem>
        </asp:DropDownList>
    </div>
</asp:Panel>
