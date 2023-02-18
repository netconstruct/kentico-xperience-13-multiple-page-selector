using CMS.Base;
using CMS.Base.Web.UI;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.DocumentEngine.Routing;
using CMS.FormEngine.Web.UI;
using CMS.Helpers;
using CMS.Membership;
using CMS.SiteProvider;
using System;
using System.Linq;

namespace CMSApp.CMSFormControls.MultiplePageSelector
{
    public partial class MultiplePageSelector : FormEngineUserControl
    {
        #region "Variables"

        private DialogConfiguration mConfig;
        private TreeProvider mTreeProvider;

        #endregion


        #region "Private properties"

        /// <summary>
        /// Gets Value display name.
        /// </summary>
        public override string ValueDisplayName
        {
            get
            {
                if (!ParentFormFieldIsInteger())
                {
                    return string.Empty;
                }

                return base.ValueDisplayName;
            }
        }


        /// <summary>
        /// Returns TreeProvider object.
        /// </summary>
        private TreeProvider TreeProvider
        {
            get
            {
                return mTreeProvider ?? (mTreeProvider = new TreeProvider(MembershipContext.AuthenticatedUser));
            }
        }


        /// <summary>
        /// Gets the configuration for Copy and Move dialog.
        /// </summary>
        private DialogConfiguration Config
        {
            get
            {
                if (mConfig == null)
                {
                    mConfig = new DialogConfiguration
                    {
                        HideLibraries = true,
                        HideAnchor = true,
                        HideAttachments = true,
                        HideContent = false,
                        HideEmail = true
                    };
                    mConfig.HideLibraries = true;
                    mConfig.HideWeb = true;
                    mConfig.EditorClientID = txtGuid.ClientID;
                    mConfig.ContentSelectedSite = SiteContext.CurrentSiteName;
                    mConfig.OutputFormat = OutputFormatEnum.Custom;
                    mConfig.CustomFormatCode = "selectpath";
                    mConfig.SelectableContent = SelectableContentEnum.AllContent;
                }
                return mConfig;
            }
        }

        #endregion


        #region "Public properties"

        /// <summary>
        /// Gets or sets the enabled state of the control.
        /// </summary>
        public override bool Enabled
        {
            get
            {
                return base.Enabled;
            }
            set
            {
                base.Enabled = value;
                btnSelect.Enabled = value;
            }
        }


        /// <summary>
        /// Gets or sets field value.
        /// </summary>
        public override object Value
        {
            get
            {
                return hdnValue.Value;
            }
            set
            {
                hdnValue.Value = value?.ToString();
            }
        }


        /// <summary>
        /// Gets ClientID of the textbox with path.
        /// </summary>
        public override string ValueElementID
        {
            get
            {
                return string.Empty;
            }
        }

        #endregion


        #region "Page events"

        protected void Page_Load(object sender, EventArgs e)
        {
            // Hide GUID textbox
            txtGuid.Attributes.Add("style", "display: none");

            // Register scripts
            RegisterScripts();

            btnSelect.Text = GetString("general.select");
            btnSelect.OnClientClick = "modalDialog('" + GetDialogUrl() + "','PathSelection', '90%', '85%'); return false;";

            txtGuid.TextChanged += txtGuid_TextChanged;

            pgrNodes.Grid = gvNodes;

            BindGrid();
        }

        /// <summary>
        /// Registers all required scripts
        /// </summary>
        private void RegisterScripts()
        {
            ScriptHelper.RegisterDialogScript(Page);
            const string script = @"
function DS_ClearDocument(txtClientID, hiddenClientId) { 
    document.getElementById(txtClientID).value = ''; 
    document.getElementById(hiddenClientId).value=''; 
    
    if(window.Changed != null) { 
        Changed(); 
    }
}";
            ScriptHelper.RegisterClientScriptBlock(this, typeof(string), "DS_Scripts", script, true);
        }


        /// <summary>
        /// Determines whether parent form field is integer.
        /// </summary>
        /// <returns>TRUE if parent form field is integer.</returns>
        private bool ParentFormFieldIsInteger()
        {
            return ((FieldInfo != null) && DataTypeManager.IsInteger(TypeEnum.Field, FieldInfo.DataType));
        }


        private void txtGuid_TextChanged(object sender, EventArgs e)
        {
            int nodeId = ValidationHelper.GetInteger(txtGuid.Text, 0);

            if (ParentFormFieldIsInteger())
            {
                return;
            }

            if (nodeId > 0)
            {
                TreeNode node = TreeProvider.SelectSingleNode(nodeId, TreeProvider.ALL_CULTURES, true);
                if (node != null)
                {
                    string site = (node.NodeSiteID != SiteContext.CurrentSiteID ? ";" + node.NodeSiteName : "");
                    txtGuid.Text = node.NodeGUID + site;

                    // store added page to Value
                    var selectedValues = Value.ToString().Split(';')
                        .Where(x => !string.IsNullOrWhiteSpace(x))
                        .ToList();

                    selectedValues.Add(node.NodeGUID.ToString());

                    Value = string.Join(";", selectedValues);

                    BindGrid();
                }
            }
        }

        #endregion


        #region "Private methods"

        /// <summary>
        /// Returns Correct URL of the copy or move dialog.
        /// </summary>
        private string GetDialogUrl()
        {
            string url = CMSDialogHelper.GetDialogUrl(Config, false, null, false);

            url = URLHelper.RemoveParameterFromUrl(url, "hash");
            url = URLHelper.AddParameterToUrl(url, "selectionmode", "single");
            url = URLHelper.AddParameterToUrl(url, "hash", QueryHelper.GetHash(url));

            return url;
        }

        protected void gvNodes_OnRowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
            {
                gvNodes.HeaderRow.TableSection = System.Web.UI.WebControls.TableRowSection.TableHeader;
            }
        }

        protected void gvNodes_OnRowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            var nodeGuid = ValidationHelper.GetString(e.CommandArgument, string.Empty);

            var selectedValues = Value.ToString().Split(';')
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();

            switch (e.CommandName.ToLowerCSafe())
            {
                case "remove":
                    if (selectedValues.Contains(nodeGuid))
                        selectedValues.Remove(nodeGuid);
                    break;

                case "moveup":
                case "movedown":

                    var indexToMove = selectedValues.FindIndex(x => x == nodeGuid);

                    if (indexToMove < 0)
                    {
                        return;
                    }

                    string oldPosition;
                    switch (e.CommandName.ToLowerCSafe())
                    {
                        case "moveup":
                            if (indexToMove > 0)
                            {
                                oldPosition = selectedValues[indexToMove - 1];
                                selectedValues[indexToMove - 1] = selectedValues[indexToMove];
                                selectedValues[indexToMove] = oldPosition;
                            }
                            break;

                        case "movedown":
                            if (indexToMove < selectedValues.Count - 1)
                            {
                                oldPosition = selectedValues[indexToMove + 1];
                                selectedValues[indexToMove + 1] = selectedValues[indexToMove];
                                selectedValues[indexToMove] = oldPosition;
                            }
                            break;
                    }

                    break;
            }

            Value = string.Join(";", selectedValues);
            BindGrid();
        }

        /// <summary>
        /// Returns WHERE condition for selected form.
        /// </summary>
        public override string GetWhereCondition()
        {
            // Return correct WHERE condition for integer if none value is selected
            if (ParentFormFieldIsInteger())
            {
                int id = ValidationHelper.GetInteger(Value, 0);
                if (id > 0)
                {
                    return base.GetWhereCondition();
                }
            }
            return null;
        }

        #endregion

        protected void BindGrid()
        {
            var selectedValues = Value.ToString()
                .Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                .ToList();

            var treeNodes = DocumentHelper.GetDocuments()
                .OnSite(SiteContext.CurrentSiteID)
                .AllCultures()
                .WhereEqualsOrNull(nameof(TreeNode.NodeLinkedNodeID), 0)
                .WhereIn(nameof(TreeNode.NodeGUID), selectedValues)
                .WithPageUrlPaths()
                .TypedResult
                .GroupBy(x => x.NodeID)
                // Return the preferred UI culture's version of the document.
                .Select(x => x.OrderBy(y =>
                {
                    return y.DocumentCulture.Equals(CurrentUser.PreferredUICultureCode) ? 0 : 1;
                }).First());

            gvNodes.Visible = true;

            gvNodes.DataSource = treeNodes
                .Select(x => new
                {
                    DocumentType = x.ClassName,
                    NodeAliasPath = x.NodeAliasPath,
                    NodeGuid = x.NodeGUID,
                    DocumentName = x.DocumentName
                })
                .OrderBy(x => selectedValues.IndexOf(x.NodeGuid.ToString()))
                .ToList();

            gvNodes.DataBind();

            pgrNodes.Visible = true;
            pgrNodes.BindPagination(treeNodes);
        }
    }
}