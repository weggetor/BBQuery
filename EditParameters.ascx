<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditParameters.ascx.cs" Inherits="Bitboxx.DNNModules.BBQuery.EditParameters" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>
<div class="dnnForm dnnBBQUeryEdit dnnClear">

	<asp:Panel runat="server" ID="pnlSearch" Visible="true">
		<div class="dnnFormItem">
			<asp:DataGrid ID="grdParameters" runat="server" AllowSorting="True" DataKeyNames="ParameterId"
				AutoGenerateColumns="False" 
				BorderStyle="None" 
				GridLines="None"
				Width="98%" 
				CssClass="dnnGrid" 
				onitemcommand="grdParameters_ItemCommand">
				<ItemStyle CssClass="dnnGridItem" horizontalalign="Left" />
				<AlternatingItemStyle cssclass="dnnGridAltItem" />
				<Edititemstyle cssclass="dnnFormInput" />
				<SelectedItemStyle cssclass="dnnFormError" />
				<FooterStyle cssclass="dnnGridFooter" />
				<PagerStyle cssclass="dnnGridPager" />
				<HeaderStyle CssClass="dnnGridHeader" />
				<Columns>
					<asp:templatecolumn>
						<itemtemplate>
							<asp:ImageButton ID="cmdEdit" runat="server" CommandName="Edit" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"ParameterId") %>' IconKey="Edit"/>
						</itemtemplate>
					</asp:templatecolumn>
					<asp:templatecolumn>
						<itemtemplate>
							<asp:ImageButton ID="cmdDelete" runat="server" CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"ParameterId") %>' IconKey="Delete"/>
						</itemtemplate>
					</asp:templatecolumn>
					<asp:BoundColumn  DataField="FieldName" HeaderText="FieldName" />
					<asp:BoundColumn  DataField="DataType" HeaderText="DataType" />
					<asp:templatecolumn HeaderText="ShowInSearch">
						<itemtemplate>
							<dnn:DnnImage Runat="server" ID="Image1" IconKey="Checked" Visible='<%# DataBinder.Eval(Container.DataItem,"ShowInSearch") %>' />
							<dnn:DnnImage Runat="server" ID="Image2" IconKey="Unchecked" Visible='<%# !(bool)DataBinder.Eval(Container.DataItem,"ShowInSearch") %>' />
						</ItemTemplate>
					</asp:templatecolumn>
				</Columns>
			</asp:DataGrid>
		</div>
	</asp:Panel>
	<hr/>
	<asp:Panel runat="server" ID="pnlEdit" Visible="False">
		<fieldset>
			<div class="dnnFormItem">
				<dnn:Label id="lblFieldName" runat="server"  controlname="txtFieldName"  suffix=":" />
				<asp:TextBox id="txtFieldName" runat="server" CssClass="dnnFormInput dnnFormRequired" />
				<asp:RequiredFieldValidator ID="valFieldname" CssClass="dnnFormMessage dnnFormError"  ControlToValidate="txtFieldName" ResourceKey="valFieldName.Error" runat="server"/>
				<asp:HiddenField runat="server" ID="hidParameterId"/>
			</div>
			<div class="dnnFormItem">
				<dnn:Label ID="lblDataType" runat="server" controlname="ddlDataType" suffix=":" />
				<asp:DropDownList runat="server" ID="ddlDataType" CssClass="dnnFormInput dnnFormRequired" >
					<asp:ListItem ResourceKey="DataTypeString.Text" Value="string"/>
					<asp:ListItem ResourceKey="DataTypeInteger.Text" Value="integer"/>
					<asp:ListItem ResourceKey="DataTypeDecimal.Text" Value="decimal"/>
					<asp:ListItem ResourceKey="DataTypeBoolean.Text" Value="boolean"/>
					<asp:ListItem ResourceKey="DataTypeDateTime.Text" Value="datetime"/>
				</asp:DropDownList>
			</div>
			<div class="dnnFormItem">
				<dnn:Label ID="lblShowInSearch" runat="server"  controlname="chkShowInSearch" suffix=":" />
				<asp:CheckBox ID="chkShowInSearch" runat="server" CssClass="dnnFormInput" />
			</div>
		</fieldset>
	</asp:Panel>
	<ul class="dnnActions dnnClear">
		<li><asp:LinkButton ID="cmdNew" runat="server" class="dnnPrimaryAction" ResourceKey="cmdNew" Visible="True" onclick="cmdNew_Click" CausesValidation="False"/></li>
		<li><asp:LinkButton ID="cmdSave" runat="server" CssClass="dnnPrimaryAction" ResourceKey="cmdSave" Visible="False" onclick="cmdSave_Click" /></li>
		<li><asp:LinkButton ID="cmdCancel" runat="server" CssClass="dnnSecondaryAction" ResourceKey="cmdCancel" Visible="False" onclick="cmdCancel_Click" CausesValidation="False"/></li>
	</ul>
</div>