<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="View.ascx.cs" Inherits="GIBS.Modules.GIBS_QR_Code.View" %>
<%@ Register TagPrefix="Portal" TagName="URL" Src="~/controls/URLControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>


<script type="text/javascript">
    function clearTextBox() { 
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
    }

    function clearURL() {
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
    }
    function clearEMAIL() {
        document.getElementById('<%= TextBoxEMAIL.ClientID %>').value = "";
            document.getElementById('<%= TextBoxURL.ClientID %>').value = "";
            document.getElementById('<%= TextBoxTEXT.ClientID %>').value = "";
            // Get the divs
            const hideURL = document.getElementById("divurl");
            const hideTEXT = document.getElementById("divtext");
            const hideEMAIL = document.getElementById("divemail");
            hideURL.style.display = "none";
            hideEMAIL.style.display = "block";
            hideTEXT.style.display = "none";    
        }
</script>

<div style="margin-bottom: 20px;">Select QR Code Type: 
    <asp:DropDownList ID="ddlQRType" runat="server" OnSelectedIndexChanged="ddlQRType_SelectedIndexChanged" AutoPostBack="true"  CssClass="form-control">
      
        <asp:ListItem Text="VCard" Value="VCard"></asp:ListItem>
        <asp:ListItem Text="Calendar Event" Value="vEvent"></asp:ListItem>
          <asp:ListItem Text="Google Business Review or Business Map" Value="GoogleReview"></asp:ListItem>
        <asp:ListItem Text="WiFi Credentials" Value="WiFi"></asp:ListItem>
        <asp:ListItem Text="E-Mail Message" Value="Email"></asp:ListItem>
        <asp:ListItem Text="Other" Value="Other"></asp:ListItem>
    </asp:DropDownList>


</div>


<asp:Panel ID="PanelWiFi" runat="server" Visible="false">
<div class="dnnForm">
    <fieldset>
        <div class="dnnFormItem">
            <dnn:Label runat="server" ControlName="txtWiFiSSID" Text="WiFi SSID" HelpText="WiFi SSIS" />
            <asp:TextBox ID="txtWiFiSSID"  runat="server"  />
        </div>

		<div class="dnnFormItem">
            <dnn:Label runat="server" ControlName="txtWiFiPassword" Text="WiFi Password" HelpText="WiFiPassword" />
            <asp:TextBox ID="txtWiFiPassword"  runat="server"  />
        </div>
		<div class="dnnFormItem">
            <dnn:Label runat="server" ControlName="txtWiFiAuthentication" Text="Authentication Mode" HelpText="Authentication Type" />
            <asp:TextBox ID="txtWiFiAuthentication" Text="WPA"  runat="server"  />
        </div>
		
		
    </fieldset>
</div>
</asp:Panel>


<asp:Panel ID="PanelVCard" runat="server">


<div style="float:right;">
<asp:LinkButton ID="LinkButtonLoadProfile" runat="server" OnClick="LinkButtonLoadProfile_Click" Visible="false" CssClass="btn btn-default btn-lg">Load Your Profile</asp:LinkButton></div>
    <h3>Create Your QR Code Business vCard</h3>
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

<asp:Panel ID="PanelvEvent" runat="server" Visible="false">

 <h3>Create Your QR Code Calendar Event</h3>
<div class="dnnForm">
    <fieldset>
        <div class="dnnFormItem">
            <dnn:Label runat="server" ControlName="txtEventName" Text="Event Name" HelpText="Event Name" />
            <asp:TextBox ID="txtEventName"  runat="server"  />
        </div>
		        <div class="dnnFormItem">
            <dnn:Label runat="server" ControlName="txtEventDesc" Text="Description" HelpText="Enter Event Description" />
            <asp:TextBox ID="txtEventDesc"  runat="server" TextMode="MultiLine" />
        </div>
		<div class="dnnFormItem">
            <dnn:Label runat="server" ControlName="txtEventStartDate" Text="Start" HelpText="Start Date" />
            <asp:TextBox ID="txtEventStartDate"  runat="server" TextMode="DateTimeLocal" ClientIDMode="Static" />
        </div>
		<div class="dnnFormItem">
            <dnn:Label runat="server" ID="lblEventEndDate" ControlName="txtEventEndDate" Text="End" HelpText="End Date" />
            <asp:TextBox ID="txtEventEndDate" TextMode="DateTimeLocal" ClientIDMode="Static"  runat="server"  />
        </div>
		<div class="dnnFormItem">
            <dnn:Label runat="server" ID="lblAllDayEvent" ControlName="CheckBoxAllDayEvent" Text="All Day Event" HelpText="All Day Event" />
            <asp:CheckBox ID="CheckBoxAllDayEvent" runat="server" />
		</div>
       <div class="dnnFormItem">
            <dnn:Label runat="server" ID="lblLocation" ControlName="txtLocation" Text="Event Location" HelpText="Event Location" />
            <asp:TextBox ID="txtEventLocation"  runat="server"  />
        </div>		
        
    </fieldset>
</div>

</asp:Panel>


<asp:Panel ID="PanelEmail" runat="server" Visible="false">

     <h3>Create Your QR Code Email Link</h3>
<div class="dnnForm">
    <fieldset>
        <div class="dnnFormItem">
            <dnn:Label runat="server" ControlName="txtEmailAddress" Text="E-Mail Address" HelpText="Recipient E-Mail Address" />
            <asp:TextBox ID="txtEmailAddress"  runat="server"  />
        </div>
		<div class="dnnFormItem">
            <dnn:Label runat="server" ControlName="txtEmailSubject" Text="Subject" HelpText="Enter Email Subject" />
            <asp:TextBox ID="txtEmailSubject"  runat="server" />
        </div>
		 <div class="dnnFormItem">
            <dnn:Label runat="server" ControlName="txtEmailMessage" Text="Message" HelpText="Enter E-Mail Message" />
            <asp:TextBox ID="txtEmailMessage"  runat="server" TextMode="MultiLine" />
        </div>
		
		
        
    </fieldset>
</div>

</asp:Panel>



<asp:Panel ID="PanelGoogleReview" runat="server" Visible="false">
    
     <h3>Create Your Google Business Review or Google Maps QR Code</h3>
<div class="dnnForm">
    <fieldset>
        		 <div class="dnnFormItem">
            
        </div>
        <div class="dnnFormItem">
            <dnn:Label runat="server" ControlName="txtGRPlaceID" Text="Google Place ID" HelpText="Google Place ID" />
            <asp:TextBox ID="txtGRPlaceID"  runat="server"  />
        </div>
		<div class="dnnFormItem">


           
           <p>Are you looking for the place ID of a specific Place or Business? Use the  
               <a href="https://developers.google.com/maps/documentation/javascript/examples/places-placeid-finder" target="_blank">Google Place ID Finder</a> to 
               search for your place/business and get your 
                <a href="https://developers.google.com/maps/documentation/javascript/examples/places-placeid-finder" target="_blank">Google Place ID</a>. 
             Return to this page with the Google Place ID.</p>
  	          </div>
        <div class="dnnFormItem"><dnn:Label runat="server" ControlName="rblGoogleCodeType" Text="Google Review or Map" HelpText="Google Review or Map" />
            <asp:RadioButtonList ID="rblGoogleCodeType" runat="server">
                <asp:ListItem Text="Review Request" Selected="True" Value="Review"></asp:ListItem>
                <asp:ListItem Text="Business Map" Value="Map"></asp:ListItem>
            </asp:RadioButtonList>
            </div>
		

        
    </fieldset>
</div>
   

</asp:Panel>




<asp:Panel ID="PanelOther" runat="server" Visible="false">

    <h3>Create a Basic QR Code</h3>

<ul class="othernav justify-content-center">
  <li class="othernavli">
    <a aria-current="page" href="#URL" onclick="javascript:clearURL();">URL</a>
  </li>
  <li class="othernavli">
    <a href="#Text" onclick="javascript:clearTextBox();">Text</a>
  </li>
  <li class="othernavli">
    <a href="#Email" onclick="javascript:clearEMAIL();">Email</a>
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



<p class="text-center"><asp:LinkButton ID="LinkButtonSUBMIT" runat="server" OnClick="LinkButtonSUBMIT_Click" CssClass="btn btn-lg btn-info">Create QR Code</asp:LinkButton></p>


<p class="text-center">
<asp:Image ID="Image1" runat="server" Visible="false" Width="300px" />
     <asp:Button ID="cmdDownLoad" runat="server" Text="Download" CssClass="btn"
      OnClick="cmdDownLoad_Click" Visible="false" />

</p>
    <div>
<asp:Label ID="LabelEncodedString" runat="server" CssClass="text-left" Text="" Visible="true"></asp:Label>
    </div>

