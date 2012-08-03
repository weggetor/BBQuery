<%@ Control language="C#" Inherits="Bitboxx.DNNModules.BBQuery.View" AutoEventWireup="true" Codebehind="View.ascx.cs" %>
<%@ Register TagPrefix="bb" TagName="SearchControl" Src="Controls/SearchControl.ascx" %> 
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/Controls/LabelControl.ascx" %>
<div class="dnnForm dnnBBQuery dnnClear">
	<asp:Panel runat="server" ID="pnlShowSql" Visible="False">
		<div class="dnnFormItem">
			<dnn:Label runat="server" ID="lblSql" />
			<asp:TextBox runat="server" ID="txtSql" Width="450" TextMode="MultiLine" Rows="4" ReadOnly="True" CssClass="dnnFormInput"></asp:TextBox>
		</div>
	</asp:Panel>
	<bb:SearchControl ID="ctrlSearch" runat="server"/>
	<br />
	<asp:Panel ID="pnlBrowseMode" runat="server">
		<asp:ImageButton runat="server" ID="imgInsert" OnClick="cmdInsert_Click" ImageUrl="~/images/add.gif"/>
		<asp:LinkButton runat="server" ID="cmdInsert" Resourcekey="cmdInsert.Text" onclick="cmdInsert_Click" />
		<asp:GridView ID="GridView1" runat="server" 
			onsorting="GridView1_Sorting"
			OnSelectedIndexChanged="GridView1_SelectedIndexChanged"
			OnPageIndexChanging="GridView1_PageIndexChanging"
			OnRowDataBound="GridView1_RowDataBound"
			OnRowCommand="GridView1_RowCommand"
			EnableViewState="True"
			CssClass="dnnGrid" onrowdeleting="GridView1_RowDeleting" >
			<AlternatingRowStyle CssClass="dnnGridAltItem" />
			<FooterStyle CssClass="dnnGridFooter" />
			<HeaderStyle CssClass="dnnGridHeader" />
			<PagerStyle CssClass="dnnGridPager" />
			<RowStyle CssClass="dnnGridItem" />
			<SelectedRowStyle CssClass="dnnGridSelectedItem" />
			<Columns>
				<asp:CommandField ShowSelectButton="True" ButtonType="Image" SelectImageUrl="~/images/edit.gif" />
				<asp:CommandField ShowDeleteButton="True" ButtonType="Image" DeleteImageUrl="~/images/delete.gif" />
			</Columns>
		</asp:GridView>
	</asp:Panel>
	<asp:Panel ID="pnlEditMode" runat="server">
		<asp:DetailsView runat="server" ID="DetailsView1" DefaultMode="Edit" 
			AutoGenerateEditButton="True" CssClass="dnnGrid" DataSourceID="ObjectDataSource1" 
			EnableModelValidation="True" onitemupdating="DetailsView1_ItemUpdating" 
			OnItemInserting="DetailsView1_ItemInserting"
			onmodechanging="DetailsView1_ModeChanging" AutoGenerateInsertButton="True">
			<EditRowStyle CssClass="dnnFormInput" />
			<FooterStyle CssClass="dnnGridFooter" />
			<HeaderStyle CssClass="dnnGridHeader" />
			<PagerStyle CssClass="dnnGridPager" />
			<RowStyle CssClass="dnnGridItem" />
		</asp:DetailsView>
	</asp:Panel>
	<asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
		onselecting="ObjectDataSource1_Selecting" SelectMethod="BBQuerySelectById" 
		TypeName="Bitboxx.DNNModules.BBQuery.BBQueryController">
		<SelectParameters>
			<asp:Parameter Name="provider" Type="String" />
			<asp:Parameter Name="connectionString" Type="String" />
			<asp:Parameter Name="sqlCommand" Type="String" />
			<asp:Parameter Name="idField" Type="String" />
			<asp:Parameter Name="idValue" Type="Object" />
		</SelectParameters>
	</asp:ObjectDataSource>
</div>

