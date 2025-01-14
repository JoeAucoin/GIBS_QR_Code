<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Edit.ascx.cs" Inherits="GIBS.Modules.GIBS_QR_Code.Edit" %>



<h3><asp:Label ID="LabelDebug" runat="server" Text=""></asp:Label></h3>


<asp:GridView ID="GridView1" runat="server" AllowPaging="true" AutoGenerateColumns="false" HorizontalAlign="Center" HeaderStyle-CssClass="centerHeaderText" CssClass="table table-striped table-bordered table-list"  
    OnPageIndexChanging="GridView1_PageIndexChanging" >
        <PagerSettings Mode="NumericFirstLast" />
        <PagerStyle CssClass="pagination-ys" HorizontalAlign="Center" />
        <HeaderStyle Font-Bold="true" />
        <RowStyle HorizontalAlign="Center" />
    <Columns>
    <asp:BoundField DataField="Files" HeaderText="Files" HtmlEncode="False" />
        </Columns>
</asp:GridView>
