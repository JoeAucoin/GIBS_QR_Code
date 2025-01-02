<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="View.ascx.cs" Inherits="GIBS.Modules.GIBS_QR_Code.View" %>
<%@ Register TagPrefix="Portal" TagName="URL" Src="~/controls/URLControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>


<script type="text/javascript">
    function clearTextBox() {
       // alert("here");
        document.getElementById('<%= TextBoxEMAIL.ClientID %>').value = "";
        document.getElementById('<%= TextBoxURL.ClientID %>').value = "";
        document.getElementById('<%= TextBoxTEXT.ClientID %>').value = "";
        // Get the divs
        const hideURL = document.getElementById("divurl"); 
        const hideTEXT = document.getElementById("divtext"); 
        const hideEMAIL = document.getElementById("divemail"); 
        hideURL.style.display = "none";
        hideEMAIL.style.display = "none";
        hideTEXT.style.display = "block";
      //  alert("here2");
    }

    function clearURL() {
        // alert("here");
        document.getElementById('<%= TextBoxEMAIL.ClientID %>').value = "";
        document.getElementById('<%= TextBoxURL.ClientID %>').value = "";
        document.getElementById('<%= TextBoxTEXT.ClientID %>').value = "";
        // Get the divs
        const hideURL = document.getElementById("divurl");
        const hideTEXT = document.getElementById("divtext");
        const hideEMAIL = document.getElementById("divemail");
        hideURL.style.display = "block";
        hideEMAIL.style.display = "none";
        hideTEXT.style.display = "none";
        //  alert("here2");
    }
    function clearEMAIL() {
        // alert("here");
        document.getElementById('<%= TextBoxEMAIL.ClientID %>').value = "";
            document.getElementById('<%= TextBoxURL.ClientID %>').value = "";
            document.getElementById('<%= TextBoxTEXT.ClientID %>').value = "";
            // Get the div element by its ID
            const hideURL = document.getElementById("divurl");
            const hideTEXT = document.getElementById("divtext");
            const hideEMAIL = document.getElementById("divemail");
            hideURL.style.display = "none";
            hideEMAIL.style.display = "block";
            hideTEXT.style.display = "none";
            //  alert("here2");
        }
</script>

<div>
    <asp:DropDownList ID="ddlQRType" runat="server" OnSelectedIndexChanged="ddlQRType_SelectedIndexChanged" AutoPostBack="true">
        <asp:ListItem Text="VCard" Value="VCard" Selected="True"></asp:ListItem>
        <asp:ListItem Text="Other" Value="Other"></asp:ListItem>
    </asp:DropDownList>
</div>

<asp:Panel ID="PanelVCard" runat="server">

    <div class="dnnForm">
        <fieldset>
            <div class="dnnFormItem">
                <dnn:label runat="server" ControlName="txtFirstName" Text="First Name" />
                <asp:TextBox ID="txtFirstName" runat="server" />
            </div>
            <div class="dnnFormItem">
                <dnn:label runat="server" ControlName="txtLastName" Text="Last Name" />
                <asp:TextBox ID="txtLastName" runat="server" />
            </div>	
            <div class="dnnFormItem">
                <dnn:label runat="server" ControlName="txtCellPhone" Text="Cell Phone" />
                <asp:TextBox ID="txtCellPhone" runat="server" />
            </div>
        <div class="dnnFormItem">
            <dnn:label runat="server" ControlName="txtWorkPhone" Text="Work Phone" />
            <asp:TextBox ID="txtWorkPhone" runat="server" />
        </div>	            
            <div class="dnnFormItem">
                <dnn:Label runat="server" ControlName="txtEmail" Text="Email" />
                <asp:TextBox ID="txtEmail"  runat="server" />
            </div>		
            <div class="dnnFormItem">
                <dnn:Label runat="server" ControlName="txtStreet" Text="Street" />
                <asp:TextBox ID="txtStreet"  runat="server" />
            </div>
            <div class="dnnFormItem">
                <dnn:Label runat="server" ControlName="txtCity" Text="City" />
                <asp:TextBox ID="txtCity"  runat="server" />
            </div>

            <div class="dnnFormItem">
                <dnn:Label runat="server" ControlName="txtState" Text="State" />
                <asp:TextBox ID="txtState"  runat="server" />
            </div>
 
            <div class="dnnFormItem">
                <dnn:Label runat="server" ControlName="txtZip" Text="Zip" />
                <asp:TextBox ID="txtZip"  runat="server" />
            </div>	
            <div class="dnnFormItem">
                <dnn:Label runat="server" ControlName="txtWebsite" Text="Website" />
                <asp:TextBox ID="txtWebsite"  runat="server" />
            </div>
            
        <div class="dnnFormItem">
            <dnn:Label runat="server" ControlName="txtBusiness" Text="Company" />
            <asp:TextBox ID="txtBusiness"  runat="server" />
        </div>	
        <div class="dnnFormItem">
            <dnn:Label runat="server" ControlName="txtTitle" Text="Title" />
            <asp:TextBox ID="txtTitle"  runat="server" />
        </div>	
        </fieldset>
    </div>

</asp:Panel>

<asp:Panel ID="PanelOther" runat="server" Visible="false">

<ul class="nav justify-content-center">
  <li class="nav-item">
    <a class="nav-link" aria-current="page" href="#" onclick="javascript:clearURL();">URL</a>
  </li>
  <li class="nav-item">
    <a class="nav-link" href="#" onclick="javascript:clearTextBox();">Text</a>
  </li>
  <li class="nav-item">
    <a class="nav-link" href="#" onclick="javascript:clearEMAIL();">Email</a>
  </li>

</ul>



<div class="dnnForm">
    <fieldset>
        <div class="dnnFormItem" id="divurl">
            <dnn:label runat="server" ControlName="TextBoxURL" Text="URL" HelpText="Enter URL" />
            <asp:TextBox ID="TextBoxURL" runat="server" CliendIDMode="Static" />
        </div>
        <div class="dnnFormItem" id="divtext" style="display:none;">
            <dnn:Label runat="server" ControlName="TextBoxTEXT" Text="Text" HelpText="Enter Any Text" />
            <asp:TextBox ID="TextBoxTEXT"  runat="server" TextMode="MultiLine" CssClass="formattextbox" CliendIDMode="Static" />
        </div>
        <div class="dnnFormItem" id="divemail" style="display:none;">
            <dnn:Label runat="server" ControlName="TextBoxEMAIL" Text="E-Mail" HelpText="Enter An E-Mail Address" />
            <asp:TextBox ID="TextBoxEMAIL" runat="server" CliendIDMode="Static" />
        </div>	
    </fieldset>
</div>
</asp:Panel>



<div class="nav justify-content-center"><asp:LinkButton ID="LinkButtonSUBMIT" runat="server" OnClick="LinkButtonSUBMIT_Click" CssClass="btn btn-lg btn-default">Create QR Code</asp:LinkButton></div>

<asp:Label ID="LabelEncodedString" runat="server" Text="" Visible="false"></asp:Label>
<br />
<asp:Image ID="Image1" runat="server" Visible="false" Width="300px" />


