using CMS.Helpers;
using CMS.PortalEngine.Web.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace CMSApp.CMSFormControls.MultiplePageSelector
{
    public partial class MultiplePageSelectorPager : CMSAbstractWebPart
    {
        public GridView Grid { get; set; }

        private const int _pagingGroupSize = 5;

        public void BindPagination<T>(IEnumerable<T> ds)
        {
            pnlPagination.Visible = false;

            if (Grid.PageCount > 1)
            {
                pnlPagination.Visible = true;
                txtCurrentPage.Text = (Grid.PageIndex + 1).ToString();
                litMaxPages.Text = Grid.PageCount.ToString();

                //Creating our own paging so we can have the same look and feel as Kentico
                var pds = new PagedDataSource
                {
                    DataSource = ds.ToList(),
                    AllowPaging = Grid.AllowPaging,
                    PageSize = Grid.PageSize,
                    CurrentPageIndex = Grid.PageIndex
                };

                var pages = new List<int>();
                for (var i = 0; i < pds.PageCount; i++)
                    pages.Add(i + 1);

                plcPreviousGroup.Visible = plcNextGroup.Visible = false;

                if (Grid.PageCount > _pagingGroupSize)
                {
                    //We only want to show <groupSize> number of pages at once
                    //Aim to keep the current page in the middle of the group
                    //Therefore, assuming the current page is included, the leverage either way is minus one halved
                    const double leverage = (_pagingGroupSize - 1) / 2;
                    var leverageDown = (int)Math.Floor(leverage);
                    var leverageUp = (int)Math.Ceiling(leverage);

                    var pagesBehind = Grid.PageIndex;
                    var pagesInFront = Grid.PageCount - (Grid.PageIndex + 1);

                    if (leverageDown > pagesBehind)
                    {
                        int change;
                        if (pagesBehind == 0)
                            change = leverageDown;
                        else
                            change = Grid.PageIndex % leverageDown;

                        leverageUp += change;
                        leverageDown -= change;
                    }

                    if (leverageUp > pagesInFront)
                    {
                        int change;

                        if (pagesInFront == 0)
                            change = leverageUp;
                        else
                            change = Grid.PageIndex % leverageUp;

                        leverageDown += change;
                        leverageUp -= change;
                    }

                    var bottomPage = Grid.PageIndex + 1 - leverageDown;
                    var topPage = Grid.PageIndex + 1 + leverageUp;

                    pages = new List<int>();
                    for (var i = bottomPage; i <= topPage; i++)
                        pages.Add(i);

                    plcPreviousGroup.Visible = bottomPage > 1;
                    plcNextGroup.Visible = topPage < Grid.PageCount;
                }

                rptPager.DataSource = pages;
                rptPager.DataBind();
            }
        }
        protected void rptPager_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                var page = ValidationHelper.GetInteger(e.Item.DataItem, 0);

                var mvPage = (MultiView)e.Item.FindControl("mvPage");
                mvPage.ActiveViewIndex = page == Grid.PageIndex + 1 ? 1 : 0;
            }
        }

        protected void rptPager_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            Grid.PageIndex = ValidationHelper.GetInteger(e.CommandArgument, 1) - 1;
        }

        protected void hlPreviousPage_OnClick(object sender, EventArgs e)
        {
            if (Grid.PageIndex > 0)
                Grid.PageIndex--;
        }

        protected void hlPreviousGroup_OnClick(object sender, EventArgs e)
        {
            var newPageIndex = Grid.PageIndex - _pagingGroupSize;
            if (newPageIndex < 0)
                newPageIndex = 0;

            Grid.PageIndex = newPageIndex;
        }

        protected void hlNextGroup_OnClick(object sender, EventArgs e)
        {
            var newPageIndex = Grid.PageIndex + _pagingGroupSize;
            if (newPageIndex + 1 > Grid.PageCount)
                newPageIndex = Grid.PageCount - 1;

            Grid.PageIndex = newPageIndex;
        }

        protected void hlNextPage_OnClick(object sender, EventArgs e)
        {
            if (Grid.PageIndex < Grid.PageCount - 1)
                Grid.PageIndex++;
        }

        protected void txtCurrentPage_OnTextChanged(object sender, EventArgs e)
        {
            Grid.PageIndex = ValidationHelper.GetInteger(txtCurrentPage.Text, 1) - 1;
        }

        protected void ddlItemsPerPage_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            Grid.PageIndex = 0;
            Grid.PageSize = ValidationHelper.GetInteger(ddlItemsPerPage.SelectedValue, 10);
        }
    }
}