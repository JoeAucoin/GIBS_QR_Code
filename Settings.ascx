<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Settings.ascx.cs" Inherits="GIBS.Modules.GIBS_QR_Code.Settings" %>

<%@ Register TagName="label" TagPrefix="dnn" Src="~/controls/labelcontrol.ascx" %>
<h2 id="dnnSitePanel-BasicSettings" class="dnnFormSectionHead">
    <a href="" class="dnnSectionExpanded">
        <%=LocalizeString("BasicSettings")%></a></h2>
<fieldset>
       <div class="dnnFormItem">
      <dnn:label ID="lblGoogleAPIKey" runat="server" ControlName="txtGoogleAPIKey" />
      <asp:TextBox ID="txtGoogleAPIKey" runat="server" />
  </div>

    <div class="dnnFormItem">
        <dnn:label ID="lblPageSize" runat="server" ControlName="txtPageSize" />
        <asp:TextBox ID="txtPageSize" runat="server" />
    </div>
 <div class="dnnFormItem">
     <dnn:label ID="lblDefaultQRType" runat="server" ControlName="ddlQRType">
     </dnn:label>
     <asp:DropDownList ID="ddlQRType" runat="server" CssClass="form-control">
    <asp:ListItem Text="VCard" Value="VCard"></asp:ListItem>
    <asp:ListItem Text="Calendar Event" Value="vEvent"></asp:ListItem>
      <asp:ListItem Text="Google Business Review or Business Map" Value="GoogleReview"></asp:ListItem>
    <asp:ListItem Text="WiFi Credentials" Value="WiFi"></asp:ListItem>
    <asp:ListItem Text="E-Mail Message" Value="Email"></asp:ListItem>
    <asp:ListItem Text="Other" Value="Other"></asp:ListItem>
</asp:DropDownList>
 </div>

    <div class="dnnFormItem">
        <dnn:label ID="lblShowLoadProfile" runat="server" ControlName="chkShowLoadProfile">
        </dnn:label>
        <asp:CheckBox ID="chkShowLoadProfile" runat="server" />
    </div>
     <div class="dnnFormItem">
     <dnn:label ID="lblSaveQRCodeImage" runat="server" ControlName="chkSaveQRCodeImage">
     </dnn:label>
     <asp:CheckBox ID="chkSaveQRCodeImage" runat="server" />
 </div>
</fieldset>

