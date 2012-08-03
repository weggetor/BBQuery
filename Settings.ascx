<%@ Control Language="C#" AutoEventWireup="false" Inherits="Bitboxx.DNNModules.BBQuery.Settings" Codebehind="Settings.ascx.cs" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>

<div class="dnnForm dnnBBQuerySettings dnnClear" id="bbquery-settings">
	<div class="dnnFormMessage dnnFormInfo dnnClear">
		<asp:Label ID="lblIntro" runat="server" ResourceKey="Intro" />
	</div>
	<h2 class="dnnFormSectionHead">
		<a class="dnnSectionExpanded"><%=LocalizeString("Select.Text")%></a>
	</h2>
	<fieldset>
		<div class="dnnFormItem">
			<dnn:Label id="lblProvider" runat="server"  controlname="ddlProvider" suffix=":" />
			<asp:DropDownList id="ddlProvider" runat="server" CssClass="dnnFormInput dnnFormRequired" />
		</div>
		<div class="dnnFormItem">
			<dnn:Label id="lblConnectionString" runat="server"  controlname="txtConnectionString" suffix=":" />
			<asp:TextBox id="txtConnectionString" runat="server" CssClass="dnnFormInput dnnFormRequired" Width="400" />
			<asp:RequiredFieldValidator ID="valConnectionString" CssClass="dnnFormMessage dnnFormError"  ControlToValidate="txtConnectionString" ResourceKey="valConnectionString.Error" runat="server"/>
		</div>
		<div class="dnnFormItem">
			<dnn:Label id="lblSqlCommand" runat="server"  controlname="txtSqlCommand" suffix=":" />
			<asp:TextBox id="txtSqlCommand" runat="server" CssClass="dnnFormInput dnnFormRequired"  TextMode="MultiLine" Width="400" Height="100" />
			<asp:RequiredFieldValidator ID="valSqlCommand" CssClass="dnnFormMessage dnnFormError"  ControlToValidate="txtSqlCommand" ResourceKey="valSqlCommand.Error" runat="server"/>
		</div>
		<div class="dnnFormItem">
			<dnn:Label id="lblSql" runat="server"  controlname="chkSql" suffix=":" />
			<asp:CheckBox runat="server" ID="chkSql" CssClass="dnnFormInput"/>
		</div>
		<div class="dnnFormItem">
			<dnn:Label id="lblHideModule" runat="server"  controlname="chkHideModule" suffix=":" />
			<asp:CheckBox runat="server" ID="chkHideModule" CssClass="dnnFormInput"/>
		</div>
	</fieldset>
	<h2 class="dnnFormSectionHead">
		<a class="dnnSectionExpanded"><%=LocalizeString("Link.Text")%></a>
	</h2>
	<fieldset>
		<div class="dnnFormItem">
			<dnn:Label id="lblLinkSource" runat="server"  controlname="ddlProvider" suffix=":" />
			<asp:TextBox id="txtLinkSource" runat="server" CssClass="dnnFormInput"  Width="150" />
		</div>
	</fieldset>
	<h2 class="dnnFormSectionHead">
		<a class="dnnSectionExpanded"><%=LocalizeString("Edit.Text")%></a>
	</h2>
	<fieldset>
		<div class="dnnFormItem">
			<dnn:Label id="lblAllowEdits" runat="server"  controlname="chkAllowEdits" suffix=":" />
			<asp:CheckBox ID="chkAllowEdits" runat="server" CssClass="dnnFormInput" AutoPostBack="True" />
		</div>
		<div class="dnnFormItem">
			<dnn:Label id="lblAllowInserts" runat="server"  controlname="chkAllowInserts" suffix=":" />
			<asp:CheckBox ID="chkAllowInserts" runat="server" CssClass="dnnFormInput" AutoPostBack="True" />
		</div>
		<div class="dnnFormItem">
			<dnn:Label id="lblAllowDeletes" runat="server"  controlname="chkAllowDeletes" suffix=":" />
			<asp:CheckBox ID="chkAllowDeletes" runat="server" CssClass="dnnFormInput" AutoPostBack="True" />
		</div>
		<asp:Panel runat="server" ID="pnlKeyNeeded">
			<div class="dnnFormItem">
				<hr/>
			</div>
			<div class="dnnFormItem">
				<dnn:Label id="lblKey" runat="server"  controlname="txtKey" suffix=":" />
				<asp:TextBox id="txtKey" runat="server" CssClass="dnnFormInput dnnFormRequired"  Width="150" />
				<asp:RequiredFieldValidator ID="valKey" CssClass="dnnFormMessage dnnFormError"  ControlToValidate="txtKey" ResourceKey="valKey.Error" runat="server"/>
			</div>
		</asp:Panel>
		<asp:Panel runat="server" ID="pnlAllowEdits">
			<div class="dnnFormItem">
				<hr/>
			</div>
			<div class="dnnFormItem">
				<dnn:Label id="lblRoleAllowEdits" runat="server"  controlname="ddlRoleAllowEdits" suffix=":" />
				<asp:DropDownList ID="ddlRoleAllowEdits" CssClass="dnnFormInput" runat="server" Width="200"/>
			</div>
			<div class="dnnFormItem">
				<dnn:Label id="lblUpdate" runat="server"  controlname="txtUpdate" suffix=":" />
				<asp:TextBox id="txtUpdate" runat="server" CssClass="dnnFormInput dnnFormRequired"  TextMode="MultiLine" Width="400" Height="100" />
				<asp:RequiredFieldValidator ID="valUpdate" CssClass="dnnFormMessage dnnFormError"  ControlToValidate="txtKey" ResourceKey="valUpdate.Error" runat="server"/>
			</div>
		</asp:Panel>
		<asp:Panel runat="server" ID="pnlAllowInserts">
			<div class="dnnFormItem">
				<hr/>
			</div>
			<div class="dnnFormItem">
				<dnn:Label id="lblRoleAllowInserts" runat="server"  controlname="ddlRoleAllowInserts" suffix=":" />
				<asp:DropDownList ID="ddlRoleAllowInserts" CssClass="dnnFormInput" runat="server" Width="200"/>
			</div>
			<div class="dnnFormItem">
				<dnn:Label id="lblInsert" runat="server"  controlname="txtInsert" suffix=":" />
				<asp:TextBox id="txtInsert" runat="server" CssClass="dnnFormInput dnnFormRequired"  TextMode="MultiLine" Width="400" Height="100" />
				<asp:RequiredFieldValidator ID="valInsert" CssClass="dnnFormMessage dnnFormError"  ControlToValidate="txtInsert" ResourceKey="valInsert.Error" runat="server"/>
			</div>
		</asp:Panel>
		<asp:Panel runat="server" ID="pnlAllowDeletes">
			<div class="dnnFormItem">
				<hr/>
			</div>
			<div class="dnnFormItem">
				<dnn:Label id="lblRoleAllowDeletes" runat="server"  controlname="ddlRoleAllowDeletes" suffix=":" />
				<asp:DropDownList ID="ddlRoleAllowDeletes" CssClass="dnnFormInput" runat="server" Width="200"/>
			</div>
			<div class="dnnFormItem">
				<dnn:Label id="lblDelete" runat="server"  controlname="txtDelete" suffix=":" />
				<asp:TextBox id="txtDelete" runat="server" CssClass="dnnFormInput dnnFormRequired"  TextMode="MultiLine" Width="400" Height="130" />
				<asp:RequiredFieldValidator ID="valDelete" CssClass="dnnFormMessage dnnFormError"  ControlToValidate="txtDelete" ResourceKey="valDelete.Error" runat="server"/>
			</div>
		</asp:Panel>
	</fieldset>
	<h2 class="dnnFormSectionHead">
		<a class="dnnSectionExpanded"><%=LocalizeString("Parameters.Text")%></a>
	</h2>
	<fieldset>
		<asp:PlaceHolder runat="server" ID="plParameters" />
	</fieldset>
</div>
