//
//  Copyright (c) 2012 bitboxx solutions Torsten Weggen
//  http://www.bitboxx.net
//
//  Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
//  documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
//  the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, 
//  and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
//  The above copyright notice and this permission notice shall be included in all copies or substantial portions 
//  of the Software.
//
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
//  TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//  THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
//  CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//  DEALINGS IN THE SOFTWARE.
//

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.UI.WebControls;
using Bitboxx.DNNModules.BBQuery.Components;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Communications;
using DotNetNuke.Entities.Users;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.UI.Skins.Controls;

namespace Bitboxx.DNNModules.BBQuery
{

	/// <summary>
	/// The View class displays the content
	/// </summary>
    [DNNtc.PackageProperties("Bitboxx.BBQuery", 1, "Bitboxx BBQuery", "Define database queries with configurable parameters", "BBQuery.png", "Torsten Weggen", "bitboxx solutions", "http://www.bitboxx.net", "info@bitboxx.net", true)]
    [DNNtc.ModuleProperties("Bitboxx.BBQuery", "Bitboxx BBQuery", 0)]
	[DNNtc.ModuleDependencies(DNNtc.ModuleDependency.CoreVersion, "08.00.00")]
	[DNNtc.ModuleControlProperties("", "Bitboxx.BBQuery", DNNtc.ControlType.View, "", true, true)]
	public partial class View : PortalModuleBase, IModuleCommunicator, IModuleListener
	{
		private string sqlCommand;
		private string updCommand;
		private string delCommand;
		private string insCommand;
		private string connectionString;
		private string provider;
		private string key;
		private bool allowInserts = false;
		private bool allowEdits = false;
		private bool allowDeletes = false;
		private Dictionary<string,int> cellIndices = new Dictionary<string, int>();

		public string SortedDirection
		{
			get
			{
				if (ViewState["SortedDirection"] != null)
					return (string) ViewState["SortedDirection"];
				return "ASC";
			}
			set { ViewState["SortedDirection"] = value; }
		}
		public string Order
		{
			get
			{
				if (ViewState["Order"] != null)
					return (string)ViewState["Order"];
				return key;
			}
			set { ViewState["Order"] = value; }
		}
		public object SelectedKey
		{
			get
			{
				if (ViewState["SelectedKey"] != null)
					return ViewState["SelectedKey"];
				return null;
			}
			set { ViewState["SelectedKey"] = value; }
		}

		public List<ParameterInfo> SearchParameters
		{
			get
			{
				if (ViewState["SearchParameters"] != null)
					return (List<ParameterInfo>)ViewState["SearchParameters"];
				return new List<ParameterInfo>();
			}
			set { ViewState["SearchParameters"] = value; }
		}
		#region Event Handlers

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			ctrlSearch.TabModuleID = TabModuleId;
			ctrlSearch.SearchClicked += new EventHandler(ctrlSearch_SearchClicked);
		}
		
		/// <summary>
		/// Runs when the control is loaded
		/// </summary>
		private void Page_Load(object sender, EventArgs e)
		{
			try
			{
				if (!IsPostBack)
					SearchParameters = new List<ParameterInfo>();
				
				// First check if we had querystring parameters
				BBQueryController cont = new BBQueryController();
				List<ParameterInfo> parameters = cont.GetParameters(TabModuleId, false);

				foreach (ParameterInfo parameter in parameters)
				{
					if (Request.QueryString[parameter.FieldName] != null)
					{
						bool found = false;
						foreach (ParameterInfo searchParameter in SearchParameters)
						{
							if (searchParameter.FieldName == parameter.FieldName)
							{
								searchParameter.Value = ParameterInfo.GetTypedValue(HttpUtility.HtmlDecode(Request.QueryString[parameter.FieldName]), parameter.DataType);
								found = true;
							}
						}
						if (!found)
						{
							parameter.Value = ParameterInfo.GetTypedValue(HttpUtility.HtmlDecode(Request.QueryString[parameter.FieldName]),parameter.DataType);
							SearchParameters.Add(parameter);
						}
					}
				}

				
				UserInfo user = UserController.GetCurrentUserInfo();
				if (Settings["AllowInserts"] != null &&
					Settings["RoleAllowInserts"] != null &&
					Request.IsAuthenticated &&
					Convert.ToBoolean(Settings["AllowInserts"]) &&
					user.IsInRole((string)Settings["RoleAllowInserts"]))
				{
					allowInserts = true;
				}
				else
				{
					cmdInsert.Visible = false;
					imgInsert.Visible = false;
				}

				if (Settings["AllowEdits"] != null && 
					Settings["RoleAllowEdits"] != null && 
					Request.IsAuthenticated && 
					Convert.ToBoolean(Settings["AllowEdits"]) &&
					user.IsInRole((string)Settings["RoleAllowEdits"]))
				{
					allowEdits = true;
				}

				if (Settings["AllowDeletes"] != null && 
					Settings["RoleAllowDeletes"] != null && 
					Request.IsAuthenticated &&
					Convert.ToBoolean(Settings["AllowDeletes"]) &&
					user.IsInRole((string)Settings["RoleAllowDeletes"]))
				{
					allowDeletes = true;
				}

				if (!IsPostBack)
				{
					pnlShowSql.Visible = Settings["ShowSql"] != null && Convert.ToBoolean(Settings["ShowSql"]);
				}

			    bool needsKey = allowInserts || allowEdits || allowDeletes;

                if (Settings["SelectCommand"] != null && Settings["Provider"] != null && (!needsKey || needsKey && Settings["Key"] != null ))
				{
					sqlCommand = (string)Settings["SelectCommand"];

                    if (String.IsNullOrEmpty((string) Settings["ConnectionString"]))
				        connectionString = ConfigurationManager.ConnectionStrings["SiteSqlServer"].ConnectionString;
				    else
				        connectionString = (string) Settings["ConnectionString"];

					key = (string)Settings["Key"];
					provider = (string) Settings["Provider"];
					updCommand = (string) Settings["UpdateCommand"];
					delCommand = (string)Settings["DeleteCommand"];
					insCommand = (string)Settings["InsertCommand"];
					
                    if (!Page.IsPostBack)
					{
						if (!String.IsNullOrEmpty(key))
							GridView1.DataKeyNames = key.Split(',');
						GridView1.AllowPaging = true;
						GridView1.PageSize = 10;
						GridView1.AllowSorting = true;

                        if (!String.IsNullOrEmpty(key))
                            DetailsView1.DataKeyNames = key.Split(',');
						
					}
					
				}
				else
				{
					string message = Localization.GetString("Configure.Message", this.LocalResourceFile);
					DotNetNuke.UI.Skins.Skin.AddModuleMessage(this, message, ModuleMessage.ModuleMessageType.YellowWarning);
				}

			}
			catch (Exception exc) //Module failed to load
			{
				Exceptions.ProcessModuleLoadException(this, exc);
			}
		}
		private void Page_PreRender(object sender, EventArgs e)
		{
			if (Settings["HideModule"] != null && Convert.ToBoolean(Settings["HideModule"]) && SearchParameters.Count == 0)
			{
				if (!DotNetNuke.Common.Globals.IsEditMode())
				{
					this.ContainerControl.Visible = false;
				}
				else
				{
					string message = Localization.GetString("Hidden.Message", this.LocalResourceFile);
					DotNetNuke.UI.Skins.Skin.AddModuleMessage(this, message, ModuleMessage.ModuleMessageType.YellowWarning);
					pnlBrowseMode.Visible = false;
					pnlEditMode.Visible = false;
					pnlShowSql.Visible = false;
					ctrlSearch.Visible = false;
				}
			}
			else
			{
				BindData();
			}
		}

		#endregion

		protected void GridView1_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
		{
			GridView1.PageIndex = 0;
			GridView1.SelectedIndex = -1;
			SelectedKey = null;
			SortedDirection = (SortedDirection == "DESC" ? "ASC" : "DESC");
			Order = e.SortExpression;
		}

		protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
		{
			SelectedKey = GridView1.SelectedIndex >= 0 ? GridView1.SelectedValue : null;
		}

		protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
		{
			GridView1.PageIndex = e.NewPageIndex;
			GridView1.SelectedIndex = -1;
			SelectedKey = null;
		}

		protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
		{
			try
			{
				e.Cancel = true;
				SelectedKey = null;
				BBQueryController cont = new BBQueryController();
				cont.BBQueryDelete(provider, connectionString, delCommand, GridView1.DataKeys[e.RowIndex].Values);
				//BindData();
			}
			catch (Exception exc) //Module failed to load
			{
				//Exceptions.ProcessModuleLoadException(exc.Message,this, exc);
				DotNetNuke.UI.Skins.Skin.AddModuleMessage(this, exc.Message, ModuleMessage.ModuleMessageType.RedError);
			}
		}

		protected void DetailsView1_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
		{
			try
			{
				e.Cancel = true;
				SelectedKey = null;
				BBQueryController cont = new BBQueryController();
				cont.BBQueryUpdate(provider, connectionString, updCommand, e.Keys, e.NewValues);
			}
			catch (Exception exc)
			{
				//Exceptions.ProcessModuleLoadException(exc.Message, this, exc);
				DotNetNuke.UI.Skins.Skin.AddModuleMessage(this, exc.Message, ModuleMessage.ModuleMessageType.RedError);
			}
		}

		protected void DetailsView1_ItemInserting(object sender, DetailsViewInsertEventArgs e)
		{
			try
			{
				e.Cancel = true;
				SelectedKey = null;
				BBQueryController cont = new BBQueryController();
				cont.BBQueryInsert(provider, connectionString, insCommand, e.Values);
				DetailsView1.ChangeMode(DetailsViewMode.Edit);
			}
			catch (Exception exc)
			{
				//Exceptions.ProcessModuleLoadException(exc.Message,this, exc);
				DotNetNuke.UI.Skins.Skin.AddModuleMessage(this, exc.Message, ModuleMessage.ModuleMessageType.RedError);
			}
		}

		protected void DetailsView1_ModeChanging(object sender, DetailsViewModeEventArgs e)
		{
			if (e.CancelingEdit)
			{
				SelectedKey = null;
				GridView1.SelectedIndex = -1;
				DetailsView1.ChangeMode(DetailsViewMode.Edit);
			}
			e.Cancel = true;
		}

		protected void ObjectDataSource1_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
		{
			if (SelectedKey != null)
			{
				e.InputParameters["provider"] = provider;
				e.InputParameters["connectionstring"] = connectionString;
				e.InputParameters["sqlcommand"] = sqlCommand;
				e.InputParameters["idField"] = key;
				e.InputParameters["idValue"] = SelectedKey;
			}
			else
			{
				e.Cancel = true;
			}
		}

		void ctrlSearch_SearchClicked(object sender, EventArgs e)
		{
			try
			{
				GridView1.PageIndex = 0;
				GridView1.SelectedIndex = -1;
				SelectedKey = null;
				List<string> param = new List<string>();
				SearchParameters = ctrlSearch.SearchParameters;
				if (ModuleCommunication != null)
				{
					ModuleCommunicationEventArgs args = new ModuleCommunicationEventArgs
						{Sender = "BBQuery", Target = "", Text = ModuleId.ToString(), Type = "", Value = SearchParameters};
					ModuleCommunication(this, args);
				}
			}
			catch (Exception exc)
			{
				Exceptions.ProcessModuleLoadException(this, exc);
			}
		}

		private void BindData()
		{
			try
			{
				BBQueryController cont = new BBQueryController();
				string orderby = (String.IsNullOrEmpty(Order) ? "" : " ORDER BY " + Order + " " + SortedDirection);

				if (SelectedKey != null)
				{
					pnlEditMode.Visible = true;
					DetailsView1.DataBind();
					pnlBrowseMode.Visible = false;
				}
				else
				{

					string selCmd = cont.GetSelect(sqlCommand + " {WHERE} " + orderby, SearchParameters);
					txtSql.Text = cont.ReplaceParameters(selCmd,SearchParameters);
					GridView1.DataSource = cont.BBQuerySelect(provider, connectionString, selCmd, SearchParameters);
					GridView1.DataBind();
					pnlBrowseMode.Visible = true;
					pnlEditMode.Visible = false;
					cmdInsert.Visible = allowInserts;
					imgInsert.Visible = allowInserts;
					GridView1.Columns[0].Visible = allowEdits;
					GridView1.Columns[1].Visible = allowDeletes;
				}
				pnlShowSql.Visible = pnlBrowseMode.Visible && Settings["ShowSql"] != null && Convert.ToBoolean(Settings["ShowSql"]);
				ctrlSearch.Visible = pnlBrowseMode.Visible;
			}
			catch (Exception exc)
			{
				//Exceptions.ProcessModuleLoadException(this, exc);
				DotNetNuke.UI.Skins.Skin.AddModuleMessage(this, exc.Message, ModuleMessage.ModuleMessageType.RedError);
			}
		}

		protected void cmdInsert_Click(object sender, EventArgs e)
		{
			GridView1.SelectedIndex = 0;
			SelectedKey = GridView1.SelectedValue;
			DetailsView1.ChangeMode(DetailsViewMode.Insert);
		}

		protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			if (e.Row.RowType == DataControlRowType.Header)
			{
				cellIndices.Clear();
				if (Settings["LinkSource"] != null)
				{
					string[] linkFields = ((string) Settings["LinkSource"]).Split(',');
					foreach (string linkField in linkFields)
					{
						for (int i = 0; i < e.Row.Cells.Count - 1; i++)
						{
							object cf = ((DataControlFieldCell) e.Row.Cells[i]).ContainingField;
							if (cf is BoundField)
							{
								BoundField field = cf as BoundField;
								if (field.DataField == linkField)
									cellIndices.Add(linkField,i);
							}
						}
					}
				}
			}


			if (e.Row.RowType == DataControlRowType.DataRow)
			{
				if (Settings["LinkSource"] != null)
				{
					foreach (KeyValuePair<string, int> cellIndex in cellIndices)
					{
						LinkButton lnk = new LinkButton
							{
								CommandName = "Select",
								CommandArgument = cellIndex.Key + "=" + HttpUtility.UrlEncode(e.Row.Cells[cellIndex.Value].Text),
								Text = e.Row.Cells[cellIndex.Value].Text,
								EnableViewState = true
							};

						List<string>paras = new List<string>();
						paras.Add(cellIndex.Key + "=" + HttpUtility.UrlEncode(e.Row.Cells[cellIndex.Value].Text));
						foreach (ParameterInfo searchParameter in SearchParameters)
						{
							paras.Add(searchParameter.FieldName+"="+HttpUtility.UrlEncode(searchParameter.Value.ToString()));
						}
						//foreach (string key in Request.QueryString.Keys)
						//{
						//    paras.Add(key + "=" + Request.QueryString[key]);
						//}

						string link = DotNetNuke.Common.Globals.NavigateURL("", paras.ToArray());
						e.Row.Cells[cellIndex.Value].Text = "<a href=\"" + link + "\">" + e.Row.Cells[cellIndex.Value].Text + "</a>";

						//e.Row.Cells[cellIndex.Value].Text = "";
						//e.Row.Cells[cellIndex.Value].Controls.Add(lnk);
					}
				}
				//DataBinder.Eval(e.Row.DataItem, "Quantity")
				// Display the company name in italics.
				//e.Row.Cells[1].Text = "<i>" + e.Row.Cells[1].Text + "</i>";

			}
		}

		#region Implementation of IModuleCommunicator

		public event ModuleCommunicationEventHandler ModuleCommunication;

		#endregion

		#region Implementation of IModuleListener
		public void OnModuleCommunication(object s, ModuleCommunicationEventArgs e)
		{
			if (e.Sender=="BBQuery")
			{
				int senderModuleID = Convert.ToInt32(e.Text);
				if (senderModuleID != ModuleId)
				{
					List<ParameterInfo> senderParams = (List<ParameterInfo>) e.Value;
					SearchParameters = new List<ParameterInfo>();
					foreach (ParameterInfo senderParam in senderParams)
					{
						bool found = false;
						foreach (ParameterInfo searchParameter in SearchParameters)
						{
							if (searchParameter.FieldName == senderParam.FieldName)
							{
								searchParameter.Value = senderParam.Value;
								found = true;
							}
						}
						if (!found)
						{
							BBQueryController cont = new BBQueryController();
							List<ParameterInfo> parameters = cont.GetParameters(TabModuleId, false);
							foreach (ParameterInfo parameter in parameters)
							{
								if (parameter.FieldName == senderParam.FieldName)
								{
									parameter.Value = senderParam.Value;
									SearchParameters.Add(parameter);
								}
							}
						}
					}
				}
			}
		}
		#endregion

		protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
		{

		}
	}

}
