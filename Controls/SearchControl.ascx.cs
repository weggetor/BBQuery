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
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Bitboxx.DNNModules.BBQuery.Components;
using DotNetNuke.Entities.Modules;
using DotNetNuke.UI.UserControls;
using DotNetNuke.Web.UI.WebControls;

namespace Bitboxx.DNNModules.BBQuery.Controls
{
	public partial class SearchControl : PortalModuleBase
	{
		private int _tabModuleID;
		private List<ParameterInfo> _parameters;
		private List<ParameterInfo> _searchParameters;
		private readonly Dictionary<string, object> _searchFieldControls = new Dictionary<string, object>();
		private readonly Dictionary<string, object> _searchComparerControls = new Dictionary<string, object>();
		private readonly Dictionary<string, object> _searchCompareTypeControls = new Dictionary<string, object>();


		public event EventHandler SearchClicked;
		public int TabModuleID
		{
			get { return _tabModuleID; }
			set { _tabModuleID = value; }
		}
		public List<ParameterInfo> SearchParameters
		{
			get { return _searchParameters; }
		}

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            string fileName = System.IO.Path.GetFileNameWithoutExtension(this.AppRelativeVirtualPath);
            if (this.ID != null)
                //this will fix it when its placed as a ChildUserControl 
                this.LocalResourceFile = this.LocalResourceFile.Replace(this.ID, fileName);
            else
                // this will fix it when its dynamically loaded using LoadControl method 
                this.LocalResourceFile = this.LocalResourceFile + fileName + ".ascx.resx";
        }
        
        protected void Page_Load(object sender, EventArgs e)
		{
			BBQueryController cont = new BBQueryController();
			_parameters = cont.GetParameters(TabModuleID, true);

			if (_parameters.Count > 0)
			{
				int loop = 0;
				HtmlGenericControl div = new HtmlGenericControl("div");
				div.Attributes.Add("class","dnnFormItem");
				HtmlTable tbl = new HtmlTable();
				tbl.Attributes.Add("align","center");

				foreach (ParameterInfo parameter in _parameters)
				{
					loop++;

					HtmlTableRow tblRow = new HtmlTableRow();
					HtmlTableCell tblCell = new HtmlTableCell();

					RadioButtonList rbl = new RadioButtonList();
					rbl.ID = "rbl" + loop.ToString();
					rbl.Items.Add(new ListItem("AND"));
					rbl.Items.Add(new ListItem("OR"));
					rbl.EnableViewState = true;
					rbl.RepeatDirection = RepeatDirection.Horizontal;
					rbl.SelectedValue = "OR";

					_searchCompareTypeControls.Add(parameter.FieldName, rbl);
					if (loop > 1)
						tblCell.Controls.Add(rbl);

					tblRow.Cells.Add(tblCell);

					tblCell = new HtmlTableCell();
					//tblCell.Style.Add("padding-left","10px");
					LabelControl lbl = LoadControl("~/controls/LabelControl.ascx") as LabelControl;
					lbl.ID = "lbl" + loop.ToString();
					lbl.Text = parameter.FieldName;
					lbl.ControlName = "ctrl" + loop.ToString();
					tblCell.Controls.Add(lbl);
					tblRow.Cells.Add(tblCell);

					DropDownList ddl = new DropDownList();
					ddl.ID = "ddl" + loop.ToString();
					ddl.Width = new Unit(100);
					ddl.DataTextField = "Key";
					ddl.DataValueField = "Value";
					ddl.EnableViewState = true;
					ddl.SelectedValue = "0";

					switch (parameter.DataType.ToLower())
					{
						case "string":
							ddl.Items.Add(new ListItem(LocalizeString("Contains.Text"), "0"));
                            ddl.Items.Add(new ListItem(LocalizeString("Starts.Text"), "1"));
                            ddl.Items.Add(new ListItem(LocalizeString("Ends.Text"), "2"));
							tblCell = new HtmlTableCell();
							tblCell.Controls.Add(ddl);
							tblRow.Cells.Add(tblCell);
							_searchComparerControls.Add(parameter.FieldName, ddl);

							TextBox txtC = new TextBox();
							txtC.ID = "ctrl" + loop.ToString();
							txtC.EnableViewState = true;
							tblCell = new HtmlTableCell();
							tblCell.Controls.Add(txtC);
							tblRow.Cells.Add(tblCell);
							_searchFieldControls.Add(parameter.FieldName,txtC);
							break;

						case "integer":
						case "decimal":
							ddl.Items.Add(new ListItem("=", "0"));
							ddl.Items.Add(new ListItem("<", "1"));
							ddl.Items.Add(new ListItem(">", "2"));
							tblCell = new HtmlTableCell();
							tblCell.Controls.Add(ddl);
							tblRow.Cells.Add(tblCell);
							_searchComparerControls.Add(parameter.FieldName, ddl);

							TextBox txtN = new TextBox();
							txtN.ID = "ctrl" + loop.ToString();
							txtN.EnableViewState = true;
							tblCell = new HtmlTableCell();
							tblCell.Controls.Add(txtN);
							tblRow.Cells.Add(tblCell);
							_searchFieldControls.Add(parameter.FieldName,txtN);
							break;

						case "boolean":
							tblCell = new HtmlTableCell();
							tblRow.Cells.Add(tblCell);

							CheckBox chk = new CheckBox();
							chk.ID = "ctrl" + loop.ToString();
							chk.EnableViewState = true;
							tblCell = new HtmlTableCell();
							tblCell.Controls.Add(chk);
							tblRow.Cells.Add(tblCell);
							_searchFieldControls.Add(parameter.FieldName, chk);
							break;

						case "datetime":
							ddl.Items.Add(new ListItem("=", "0"));
							ddl.Items.Add(new ListItem("<", "1"));
							ddl.Items.Add(new ListItem(">", "2"));
							ddl.SelectedValue = "2";
							tblCell = new HtmlTableCell();
							tblCell.Controls.Add(ddl);
							tblRow.Cells.Add(tblCell);
							_searchComparerControls.Add(parameter.FieldName, ddl);
							
							DnnTimePicker dnnTimePicker = new DnnTimePicker();
							dnnTimePicker.ID = "ctrl" + loop.ToString();
							dnnTimePicker.EnableViewState = true;
							dnnTimePicker.MinDate = DateTime.MinValue;
							dnnTimePicker.SelectedDate = null;
							
							DnnDatePicker dnnDatePicker = new DnnDatePicker();
							dnnDatePicker.ID = "ctrl" + loop.ToString() + "a";
							dnnDatePicker.EnableViewState = true;
							dnnDatePicker.MinDate = DateTime.MinValue;
							dnnDatePicker.SelectedDate = null;
							dnnDatePicker.Controls.Add(dnnTimePicker);
							dnnDatePicker.Width = new Unit(230);
							tblCell = new HtmlTableCell();
							tblCell.Controls.Add(dnnDatePicker);
							tblRow.Cells.Add(tblCell);
							_searchFieldControls.Add(parameter.FieldName, dnnDatePicker);
							break;
					}
					tbl.Rows.Add(tblRow);
				}
				div.Controls.Add(tbl);
				phSearchControl.Controls.Add(div);
				pnlSearch.Visible = true;
			}
		}

		protected void cmdSearch_click(object sender, EventArgs e)
		{
			_searchParameters = new List<ParameterInfo>();

			foreach (ParameterInfo param in _parameters)
			{
				object searchFieldControl = null;
				object searchComparerControl = null;
				object searchCompareTypeControl = null;
				object value = null;
				int comparer = -1;
				string compareType = "";
				if (_searchFieldControls.TryGetValue(param.FieldName, out searchFieldControl))
				{
					Control ctrl = searchFieldControl as Control;
					switch (param.DataType.ToLower())
					{
						case "string":
						case "integer":
						case "decimal":
							TextBox txt = ctrl as TextBox;
							value = ParameterInfo.GetTypedValue(txt.Text.Trim(), param.DataType);
							break;
						case "boolean":
							CheckBox chk = ctrl as CheckBox;
							if (chk.Checked)
								value = true;
							break;
						case "datetime":
							DnnDatePicker dp = ctrl as DnnDatePicker;
							value = dp.SelectedDate;
							break;
					}
				}

				if (_searchComparerControls.TryGetValue(param.FieldName, out searchComparerControl))
				{
					Control ctrl = searchComparerControl as Control;
					switch (param.DataType.ToLower())
					{
						case "string":
						case "integer":
						case "decimal":
							DropDownList ddlC = ctrl as DropDownList;
							int.TryParse(ddlC.SelectedValue, out comparer);
							break;
						case "boolean":
							comparer = 0;
							break;
						case "datetime":
							DropDownList ddlD = ctrl as DropDownList;
							int.TryParse(ddlD.SelectedValue, out comparer);
							break;
					}
				}

				if (_searchCompareTypeControls.TryGetValue(param.FieldName, out searchCompareTypeControl))
				{
					RadioButtonList ctrl = searchCompareTypeControl as RadioButtonList;
					compareType = ctrl.SelectedValue;
				}

				if (value != null && comparer > -1)
				{
					param.Value = value;
					param.Comparer = comparer;
					param.CompareType = compareType;
					_searchParameters.Add(param);
				}
			}
			
			if(this.SearchClicked != null)
				this.SearchClicked(this, new EventArgs());
		}
	}
}