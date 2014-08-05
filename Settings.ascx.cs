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
using System.Collections;
using System.Data;
using System.Data.Common;
using DotNetNuke.Security.Roles;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Localization;


namespace Bitboxx.DNNModules.BBQuery
{

	/// -----------------------------------------------------------------------------
	/// <summary>
	/// The Settings class manages Module Settings
	/// </summary>
	/// -----------------------------------------------------------------------------
    [DNNtc.PackageProperties("Bitboxx.BBQuery")]
    [DNNtc.ModuleProperties("Bitboxx.BBQuery")]
    [DNNtc.ModuleControlProperties("Settings", "Bitboxx BBQuery Settings", DNNtc.ControlType.Edit, "", true, true)]
	public partial class Settings : ModuleSettingsBase
	{

		#region Base Method Implementations

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			pnlAllowEdits.Visible = chkAllowEdits.Checked;
			pnlAllowInserts.Visible = chkAllowInserts.Checked;
			pnlAllowDeletes.Visible = chkAllowDeletes.Checked;
			pnlKeyNeeded.Visible = chkAllowEdits.Checked || chkAllowDeletes.Checked || chkAllowInserts.Checked;

			EditParameters ctrlParameters = LoadControl("EditParameters.ascx") as EditParameters;
			ctrlParameters.ModuleConfiguration = this.ModuleConfiguration;
			ctrlParameters.LocalResourceFile = Localization.GetResourceFile(ctrlParameters, ctrlParameters.GetType().BaseType.Name + ".ascx");
			ctrlParameters.MainControl = this;
			plParameters.Controls.Add(ctrlParameters);
		}
	
		
		/// -----------------------------------------------------------------------------
		/// <summary>
		/// LoadSettings loads the settings from the Database and displays them
		/// </summary>
		/// -----------------------------------------------------------------------------
		public override void LoadSettings()
		{
			try
			{
				if (!Page.IsPostBack)
				{
					// Retrieve the installed providers and factories.
					DataTable table = DbProviderFactories.GetFactoryClasses();

					ddlProvider.DataTextField = "Name";
					ddlProvider.DataValueField = "InvariantName";
					ddlProvider.DataSource = table;
					ddlProvider.DataBind();

					RoleController rc = new RoleController();
					ArrayList roleArr = rc.GetPortalRoles(PortalId);
					ddlRoleAllowEdits.DataSource = roleArr;
					ddlRoleAllowEdits.DataTextField = "RoleName";
					ddlRoleAllowEdits.DataValueField = "RoleName";
					ddlRoleAllowEdits.DataBind();

					ddlRoleAllowInserts.DataSource = roleArr;
					ddlRoleAllowInserts.DataTextField = "RoleName";
					ddlRoleAllowInserts.DataValueField = "RoleName";
					ddlRoleAllowInserts.DataBind();
					
					ddlRoleAllowDeletes.DataSource = roleArr;
					ddlRoleAllowDeletes.DataTextField = "RoleName";
					ddlRoleAllowDeletes.DataValueField = "RoleName";
					ddlRoleAllowDeletes.DataBind();
					
					if (TabModuleSettings["Provider"] != null)
						ddlProvider.SelectedValue = (string) TabModuleSettings["Provider"];
					
					if (TabModuleSettings["SelectCommand"] != null)
						txtSqlCommand.Text = (string) TabModuleSettings["SelectCommand"];

					if (TabModuleSettings["ConnectionString"] != null)
						txtConnectionString.Text = (string)TabModuleSettings["ConnectionString"];

					if (TabModuleSettings["ShowSql"] != null)
						chkSql.Checked = Convert.ToBoolean(TabModuleSettings["ShowSql"]);

					if (TabModuleSettings["HideModule"] != null)
						chkHideModule.Checked = Convert.ToBoolean(TabModuleSettings["HideModule"]);

					if (TabModuleSettings["LinkSource"] != null)
						txtLinkSource.Text = (string)TabModuleSettings["LinkSource"];

					if (TabModuleSettings["Key"] != null)
						txtKey.Text = (string)TabModuleSettings["Key"];

					if (TabModuleSettings["UpdateCommand"] != null)
						txtUpdate.Text = (string)TabModuleSettings["UpdateCommand"];

					if (TabModuleSettings["InsertCommand"] != null)
						txtInsert.Text = (string)TabModuleSettings["InsertCommand"];

					if (TabModuleSettings["DeleteCommand"] != null)
						txtDelete.Text = (string)TabModuleSettings["DeleteCommand"];

					chkAllowEdits.Checked = TabModuleSettings["AllowEdits"] != null && Convert.ToBoolean(TabModuleSettings["AllowEdits"]);
					chkAllowInserts.Checked = TabModuleSettings["AllowInserts"] != null && Convert.ToBoolean(TabModuleSettings["AllowInserts"]);
					chkAllowDeletes.Checked = TabModuleSettings["AllowDeletes"] != null && Convert.ToBoolean(TabModuleSettings["AllowDeletes"]);

					if (TabModuleSettings["RoleAllowEdits"] != null &&
						ddlRoleAllowEdits.Items.FindByValue((string)TabModuleSettings["RoleAllowEdits"]) != null)
					{
						ddlRoleAllowEdits.SelectedValue = (string) TabModuleSettings["RoleAllowEdits"];
					}

					if (TabModuleSettings["RoleAllowDeletes"] != null &&
						ddlRoleAllowEdits.Items.FindByValue((string)TabModuleSettings["RoleAllowDeletes"]) != null)
					{
						ddlRoleAllowDeletes.SelectedValue = (string) TabModuleSettings["RoleAllowDeletes"];
					}
				}
			}
			catch (Exception exc) //Module failed to load
			{
				Exceptions.ProcessModuleLoadException(this, exc);
			}
		}

		/// -----------------------------------------------------------------------------
		/// <summary>
		/// UpdateSettings saves the modified settings to the Database
		/// </summary>
		/// -----------------------------------------------------------------------------
		public override void UpdateSettings()
		{
			try
			{
				ModuleController modules = new ModuleController();
				modules.UpdateTabModuleSetting(this.TabModuleId, "SelectCommand", txtSqlCommand.Text);
				modules.UpdateTabModuleSetting(this.TabModuleId, "Provider", ddlProvider.SelectedValue);
				modules.UpdateTabModuleSetting(this.TabModuleId, "ConnectionString", txtConnectionString.Text);
				modules.UpdateTabModuleSetting(this.TabModuleId, "Key", txtKey.Text);
				modules.UpdateTabModuleSetting(this.TabModuleId, "ShowSql", chkSql.Checked.ToString());
				modules.UpdateTabModuleSetting(this.TabModuleId, "HideModule", chkHideModule.Checked.ToString());
				modules.UpdateTabModuleSetting(this.TabModuleId, "LinkSource", txtLinkSource.Text);
				modules.UpdateTabModuleSetting(this.TabModuleId, "UpdateCommand", txtUpdate.Text);
				modules.UpdateTabModuleSetting(this.TabModuleId, "InsertCommand", txtInsert.Text);
				modules.UpdateTabModuleSetting(this.TabModuleId, "DeleteCommand", txtDelete.Text);
				modules.UpdateTabModuleSetting(this.TabModuleId, "AllowEdits", chkAllowEdits.Checked.ToString());
				modules.UpdateTabModuleSetting(this.TabModuleId, "AllowInserts", chkAllowInserts.Checked.ToString());
				modules.UpdateTabModuleSetting(this.TabModuleId, "AllowDeletes", chkAllowDeletes.Checked.ToString());
				modules.UpdateTabModuleSetting(this.TabModuleId, "RoleAllowEdits", ddlRoleAllowEdits.SelectedValue);
				modules.UpdateTabModuleSetting(this.TabModuleId, "RoleAllowInserts", ddlRoleAllowInserts.SelectedValue);
				modules.UpdateTabModuleSetting(this.TabModuleId, "RoleAllowDeletes", ddlRoleAllowDeletes.SelectedValue);

			}
			catch (Exception exc) //Module failed to load
			{
				Exceptions.ProcessModuleLoadException(this, exc);
			}
		}

		#endregion

	}

}

