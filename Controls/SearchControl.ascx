<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SearchControl.ascx.cs" Inherits="Bitboxx.DNNModules.BBQuery.Controls.SearchControl" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/Controls/LabelControl.ascx" %>
<asp:PlaceHolder runat="server" ID="phSearchControl" />
<asp:Panel runat="server" ID="pnlSearch" Visible="False" CssClass="dnnFormItem">
	<dnn:Label runat="server" ID="lblDummy"/>
	<asp:linkbutton id="cmdSearch" cssclass="dnnPrimaryAction" runat="server" OnClick="cmdSearch_click" Text="Search"/>
	<p>&nbsp;</p>
</asp:Panel>
