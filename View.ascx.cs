using DotNetNuke.Abstractions;
using DotNetNuke.Entities.Modules;
using Microsoft.Extensions.DependencyInjection;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using QRCoder;
using System;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using static QRCoder.PayloadGenerator;
using System.Text.RegularExpressions;
using DotNetNuke.Entities.Profile;
using DotNetNuke.Entities.Users;
using System.Windows.Shapes;
using System.Collections;
using System.Web.UI.WebControls;
using System.Web;

using DotNetNuke.Services.FileSystem;
using DotNetNuke.Entities.Portals;

using DotNetNuke.Common;



namespace GIBS.Modules.GIBS_QR_Code
{
    

    public partial class View : GIBS_QR_CodeModuleSettingsBase, IActionable
    {
        private readonly INavigationManager _navigationManager;

        public string _GoogleAPIKey;
        public string _QRCodeFilename = "";


        public View()
        {

            _navigationManager = DependencyProvider.GetRequiredService<INavigationManager>();


        }


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    //LabelEncodedString.Text = rblGoogleCodeType.SelectedItem.ToString();
                    _GoogleAPIKey = GoogleAPIKey.ToString();

                    bool DoesFolderExists = System.IO.Directory.Exists(PortalSettings.HomeDirectoryMapPath + "QRCode");

                    if (!DoesFolderExists)
                    {
                        CreateFolder("QRCode");
                    }


                    SetPanel(DefaultQRType);
                    ddlQRType.SelectedValue = DefaultQRType;

                    if (UserInfo.IsInRole("Administator"))
                    {
                        HyperLinkViewImages.Visible = true;
                        HyperLinkViewImages.NavigateUrl = _navigationManager.NavigateURL(PortalSettings.ActiveTab.TabID, "List", "mid=" + this.ModuleId.ToString());
                    }
                    //

                }
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        public void LoadProfile()
        {
            try
            {
                DotNetNuke.Entities.Users.UserInfo userInfo = DotNetNuke.Entities.Users.UserController.GetUserById(PortalId, UserId);
                if (userInfo != null)
                {
                    txtFirstName.Text = userInfo.FirstName;
                    txtLastName.Text = userInfo.LastName;
                    txtEmail.Text = userInfo.Email;
                    txtWebsite.Text = userInfo.Profile.GetProperty("Website").PropertyValue;
                    txtStreet.Text = userInfo.Profile.GetProperty("Street").PropertyValue;
                    txtCity.Text = userInfo.Profile.GetPropertyValue("City");
                    txtState.Text = userInfo.Profile.GetPropertyValue("Region");
                    txtZip.Text = userInfo.Profile.GetPropertyValue("PostalCode");
                    txtCellPhone.Text = userInfo.Profile.GetPropertyValue("Cell");
                    txtWorkPhone.Text = userInfo.Profile.GetPropertyValue("Telephone");
                    txtBusiness.Text = userInfo.Profile.GetPropertyValue("Company");
                    txtTitle.Text = userInfo.Profile.GetPropertyValue("Title");

                    LinkButtonLoadProfile.Visible = false;
                }




            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

        public void CreateFolder(string myfolderName)
        {
            try
            {

                string subPath = myfolderName; // Your code goes here

                bool exists = System.IO.Directory.Exists(PortalSettings.HomeDirectoryMapPath + subPath);

                if (!exists)
                    System.IO.Directory.CreateDirectory(PortalSettings.HomeDirectoryMapPath + subPath);

                SynchronizeFiles();

            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }


        private void SynchronizeFiles()
        {
            var ctlPortal = new PortalController();
            var portals = ctlPortal.GetPortals();

            foreach (PortalInfo portal in portals)
            {
                SyncForPortal(this.PortalId);
            }
        }

        private void SyncForPortal(int portalId)
        {
            try
            {
                FolderManager.Instance.Synchronize(portalId);
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        protected void cmdDownLoad_Click(object sender, EventArgs e)
        {


            string strFileOnly = HiddenFieldFileName.Value.ToString();
            string strFile = Server.MapPath(@"~/Portals/" + this.PortalId + "/QRCode/" + strFileOnly.ToString());
            //  strFile = Server.MapPath(@"~/UpLoadFiles/" + strFileOnly);

            string sMineType = MimeMapping.GetMimeMapping(strFileOnly);
            Response.ContentType = sMineType;
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + strFileOnly);
            Response.TransmitFile(strFile);
            Response.End();
        }

        public ModuleActionCollection ModuleActions
        {
            get
            {
                var actions = new ModuleActionCollection
                    {
                        {
                            GetNextActionID(), Localization.GetString("EditModule", LocalResourceFile), "", "", "",
                            EditUrl(), false, SecurityAccessLevel.Edit, true, false
                        }
                    };
                return actions;
            }
        }

        protected void LinkButtonSUBMIT_Click(object sender, EventArgs e)
        {
            try
            {

                if (ddlQRType.SelectedValue == "Other")
                {
                    SaveOther();
                  
                }
                else if (ddlQRType.SelectedValue == "vEvent")
                {

                    SaveVEvent();
                }
                else if (ddlQRType.SelectedValue == "Email")
                {

                    SaveEmail();
                }
                else if (ddlQRType.SelectedValue == "WiFi")
                {

                    SaveWiFi();
                }

                else if (ddlQRType.SelectedValue == "GoogleReview")
                {

                    SaveGoogleReview();
                }
                else
                {
                    SaveVCard();
                }

            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }

        }


        public static (string HouseNumber, string StreetName) SplitAddress(string address)
        {
            if (string.IsNullOrEmpty(address))
            {
                return ("", ""); // Or throw an exception if appropriate
            }

            Match match = Regex.Match(address, @"^(\d+)\b\s*(.*)");

            if (match.Success)
            {
                string houseNumber = match.Groups[1].Value;
                string streetName = match.Groups[2].Value.Trim(); // Trim extra whitespace
                return (houseNumber, streetName);
            }
            else
            {
                return ("", address.Trim()); // Handle cases where no house number is found
            }
        }

        private void GenerateAndDisplayQRCode(string payload, string fileName)
        {
            _QRCodeFilename = fileName;
            HiddenFieldFileName.Value = _QRCodeFilename;
            string qrCodeImagePath = PortalSettings.HomeDirectoryMapPath + "QRCode\\" + _QRCodeFilename;

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);

            using (Bitmap bitMap = qrCode.GetGraphic(20))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    bitMap.Save(ms, ImageFormat.Png);
                    byte[] byteImage = ms.ToArray();

                    Image1.Visible = true;
                    cmdDownLoad.Visible = true;
                    Image1.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(byteImage);

                    if (SaveQRCodeImage)
                    {
                        if (File.Exists(qrCodeImagePath))
                        {
                            File.Delete(qrCodeImagePath);
                        }
                        File.WriteAllBytes(qrCodeImagePath, byteImage);
                    }
                }
            }

            LabelEncodedString.Text = payload;
        }


        public void SaveWiFi()
        {
            try
            {

                WiFi generator = new WiFi(txtWiFiSSID.Text, txtWiFiPassword.Text, WiFi.Authentication.WPA, false, false);
                string payload = generator.ToString();
                string fileName = "WiFi_" + txtWiFiSSID.Text.Replace(" ", "") + ".png";
                GenerateAndDisplayQRCode(payload, fileName);

            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

        public void SaveGoogleReview()
        {
            try
            {
                string googleReviewAddress;

                if (rblGoogleCodeType.SelectedValue == "Review")
                {
                    googleReviewAddress = "https://search.google.com/local/writereview?placeid=" + txtGRPlaceID.Text.Trim();
                }
                else
                {
                    googleReviewAddress = "https://www.google.com/maps/search/?api=1&query=Google&query_place_id=" + txtGRPlaceID.Text.Trim();
                }
                Url generator = new Url(googleReviewAddress);
                string payload = generator.ToString();
                string fileName = "GoogleReview_" + txtGRPlaceID.Text.Substring(0, 10) + ".png";
                GenerateAndDisplayQRCode(payload, fileName);

            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

        public void SaveVCard()
        {
            try
            {
                string address = txtStreet.Text;
                (string houseNumber, string streetName) = SplitAddress(address);

                ContactData generator = new ContactData(ContactData.ContactOutputType.VCard3, txtFirstName.Text, txtLastName.Text,
                    "", "", txtCellPhone.Text, txtWorkPhone.Text, txtEmail.Text, null, txtWebsite.Text,
                   streetName, houseNumber, txtCity.Text, txtZip.Text, null, null,
                    txtState.Text, ContactData.AddressOrder.Reversed,
                    txtBusiness.Text, txtTitle.Text);

                string payload = generator.ToString();
                payload = payload.Replace("ADR;TYPE=HOME", "ADR;TYPE=WORK");
                string fileName = "vCard_" + txtLastName.Text.Replace(" ", "") + "_" + txtFirstName.Text.Replace(" ", "") + ".png";
                GenerateAndDisplayQRCode(payload, fileName);

            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

        public void SaveEmail()
        {
            try
            {

                Mail generator = new Mail(txtEmailAddress.Text, txtEmailSubject.Text, txtEmailMessage.Text);
                string payload = generator.ToString();
                string fileName = "Email_" + txtEmailAddress.Text.Replace("@", "_AT_") + ".png";
                GenerateAndDisplayQRCode(payload, fileName);

            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }


        public void SaveVEvent()
        {
            try
            {
                DateTime starttime = DateTime.Parse(txtEventStartDate.Text);
                DateTime endtime = DateTime.Parse(txtEventEndDate.Text);
                bool alldayEvent = CheckBoxAllDayEvent.Checked;
                string location = txtEventLocation.Text;

                CalendarEvent generator = new CalendarEvent(txtEventName.Text, txtEventDesc.Text,
                    location, starttime, endtime, alldayEvent, CalendarEvent.EventEncoding.Universal);

                string payload = generator.ToString();
                string fileName = "Event_" + txtEventName.Text.Replace(" ", "") + ".png";
                GenerateAndDisplayQRCode(payload, fileName);

            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }


        public void SaveOther()
        {
            try
            {
                string email = TextBoxEMAIL.Text;
                if (email.Length > 0)
                {
                    email = "mailto:" + email;
                }

                string payload = TextBoxURL.Text + email + TextBoxTEXT.Text;
                string format1 = "Mddyyyyhhmmsstt";
                var myTimeStamp = DateTime.Now.ToString(format1);
                string fileName = "Other_" + myTimeStamp + ".png";
                GenerateAndDisplayQRCode(payload, fileName);

                LabelEncodedString.Text = TextBoxURL.Text + email + TextBoxTEXT.Text.Replace(Environment.NewLine, "<br />");

            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }


        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }


        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }


        private string GetDisplayName(int PortalId, int UserId)
        {
            string UserDisplayName = "";
            DotNetNuke.Entities.Users.UserInfo userInfo = DotNetNuke.Entities.Users.UserController.GetUserById(PortalId, UserId);
            if (userInfo != null)
                UserDisplayName = userInfo.DisplayName;

            return UserDisplayName;
        }

        private string GetUserProfileString(string CustomProperty, int UserID)
        {
            UserInfo oUserInfo = UserController.GetUserById(PortalSettings.PortalId, UserID);
            string sCustomProperty = oUserInfo.Profile.GetPropertyValue(CustomProperty);
            return sCustomProperty;
        }

        protected string GetMapUrl()
        {
            //  return ;

            return "https://maps.googleapis.com/maps/api/js?key=" + _GoogleAPIKey.ToString() + "&libraries=places&v=weekly";
        }


        protected void ddlQRType_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetPanel(ddlQRType.SelectedValue.ToString());

        }

        public void SetPanel(string whatPanel)
        {
            try
            {
                string myPanel = whatPanel.ToLower();

                switch (myPanel)
                {
                    case "vcard":
                        LinkButtonSUBMIT.ValidationGroup = "ValGroupvCard";
                        PanelOther.Visible = false;
                        PanelVCard.Visible = true;
                        PanelvEvent.Visible = false;
                        PanelEmail.Visible = false;
                        PanelGoogleReview.Visible = false;
                        PanelWiFi.Visible = false;
                        TextBoxURL.Text = string.Empty;
                        TextBoxTEXT.Text = string.Empty;
                        TextBoxEMAIL.Text = string.Empty;

                        if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                        {
                            LinkButtonLoadProfile.Visible = true;
                        }

                        break;

                    case "email":
                        LinkButtonSUBMIT.ValidationGroup = "ValGroupEmail";
                        LinkButtonLoadProfile.Visible = false;
                        PanelvEvent.Visible = false;
                        PanelOther.Visible = false;
                        PanelVCard.Visible = false;
                        PanelEmail.Visible = true;
                        PanelGoogleReview.Visible = false;
                        PanelWiFi.Visible = false;
                        TextBoxURL.Text = string.Empty;
                        TextBoxTEXT.Text = string.Empty;
                        TextBoxEMAIL.Text = string.Empty;

                        break;

                    case "wifi":
                        LinkButtonSUBMIT.ValidationGroup = "ValGroupWiFi";
                        LinkButtonLoadProfile.Visible = false;
                        PanelWiFi.Visible = true;
                        PanelvEvent.Visible = false;
                        PanelOther.Visible = false;
                        PanelVCard.Visible = false;
                        PanelEmail.Visible = false;
                        PanelGoogleReview.Visible = false;

                        TextBoxURL.Text = string.Empty;
                        TextBoxTEXT.Text = string.Empty;
                        TextBoxEMAIL.Text = string.Empty;

                        break;

                    case "googlereview":
                        LinkButtonSUBMIT.ValidationGroup = "ValGroupGoogleReview";
                        LinkButtonLoadProfile.Visible = false;
                        PanelGoogleReview.Visible = true;
                        PanelvEvent.Visible = false;
                        PanelOther.Visible = false;
                        PanelVCard.Visible = false;
                        PanelEmail.Visible = false;
                        PanelWiFi.Visible = false;
                        TextBoxURL.Text = string.Empty;
                        TextBoxTEXT.Text = string.Empty;
                        TextBoxEMAIL.Text = string.Empty;

                        break;

                    case "vevent":
                        LinkButtonSUBMIT.ValidationGroup = "ValGroupvEvent";
                        LinkButtonLoadProfile.Visible = false;
                        PanelvEvent.Visible = true;
                        PanelOther.Visible = false;
                        PanelVCard.Visible = false;
                        PanelEmail.Visible = false;
                        PanelGoogleReview.Visible = false;
                        PanelWiFi.Visible = false;
                        TextBoxURL.Text = string.Empty;
                        TextBoxTEXT.Text = string.Empty;
                        TextBoxEMAIL.Text = string.Empty;

                        break;

                    case "other":
                        LinkButtonSUBMIT.ValidationGroup = "ValGroupOther";
                        LinkButtonLoadProfile.Visible = false;
                        PanelOther.Visible = true;
                        PanelVCard.Visible = false;
                        PanelvEvent.Visible = false;
                        PanelGoogleReview.Visible = false;
                        PanelEmail.Visible = false;
                        PanelWiFi.Visible = false;
                        TextBoxURL.Text = string.Empty;
                        TextBoxTEXT.Text = string.Empty;
                        TextBoxEMAIL.Text = string.Empty;

                        break;

                    default:


                        break;
                }

            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }


        protected void LinkButtonLoadProfile_Click(object sender, EventArgs e)
        {
            LoadProfile();
        }
    }
}