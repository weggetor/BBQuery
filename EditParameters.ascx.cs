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
using System.Web.UI;
using System.Web.UI.WebControls;
using Bitboxx.DNNModules.BBQuery.Components;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Localization;
using DotNetNuke.UI.Skins.Controls;

namespace Bitboxx.DNNModules.BBQuery
{
	public partial class EditParameters : PortalModuleBase
	{
		#region Private Members

		private BBQueryController _controller;
		private bool _editModeEnabled = false;

		#endregion

		#region Properties

		public BBQueryController Controller
		{
			get
			{
				if (_controller == null)
					_controller = new BBQueryController();
				return _controller;
			}
		}
		public bool EditModeEnabled
		{
			get { return _editModeEnabled; }
			set
			{
				_editModeEnabled = value;
				pnlEdit.Visible = _editModeEnabled;
				pnlSearch.Visible = !_editModeEnabled;
				cmdNew.Visible = !_editModeEnabled;
				cmdSave.Visible = _editModeEnabled;
				cmdCancel.Visible = _editModeEnabled;
			}
		}
		public Control MainControl { get; set; } 
		#endregion


		protected void Page_Load(object sender, EventArgs e)
		{
			Localization.LocalizeDataGrid(ref grdParameters, this.LocalResourceFile);
			if (!IsPostBack)
			{
				BindData();
			}
		}

		protected void grdParameters_ItemCommand(object source, DataGridCommandEventArgs e)
		{
			int parameterId = Convert.ToInt32(e.CommandArgument);
			switch (e.CommandName)
			{
				case "Edit":
					ParameterInfo parameter = Controller.GetParameter(parameterId);
					txtFieldName.Text = parameter.FieldName;
					ddlDataType.SelectedValue = parameter.DataType.ToLower();
					chkShowInSearch.Checked = parameter.ShowInSearch;
					hidParameterId.Value = parameter.ParameterID.ToString();
					EditModeEnabled = true;
					BindData();
					break;
				case "Delete":
					try
					{
						Controller.DeleteParameter(parameterId);
					}
					catch (Exception)
					{
						string message = Localization.GetString("DeleteParameter.Error", this.LocalResourceFile);
						DotNetNuke.UI.Skins.Skin.AddModuleMessage(this, message, ModuleMessage.ModuleMessageType.YellowWarning);
					}
					
					EditModeEnabled = false;
					BindData();
					break;
			}
		}

		protected void cmdNew_Click(object sender, EventArgs e)
		{
			txtFieldName.Text = "";
			ddlDataType.SelectedValue = "string";
			chkShowInSearch.Checked = true;
			hidParameterId.Value = "-1";
			EditModeEnabled = true;
			BindData();
		}

		protected void cmdSave_Click(object sender, EventArgs e)
		{
			ParameterInfo parameter = new ParameterInfo
				{
					ParameterID = Convert.ToInt32(hidParameterId.Value),
					FieldName = txtFieldName.Text,
					DataType = ddlDataType.SelectedValue.ToLower(),
					ShowInSearch = chkShowInSearch.Checked
				};

			Controller.SaveParameter(TabModuleId,parameter);
			BindData();
			EditModeEnabled = false;
		}

		protected void cmdCancel_Click(object sender, EventArgs e)
		{
			EditModeEnabled = false;
			BindData();
		}

		private void BindData()
		{
			List<ParameterInfo> allParams = Controller.GetParameters(TabModuleId,false);
			grdParameters.DataSource = allParams;
			grdParameters.DataBind();
		}
	}
}