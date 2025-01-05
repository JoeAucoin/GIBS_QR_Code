/*
' Copyright (c) 2024  GIBS.com
'  All rights reserved.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' 
*/

using DotNetNuke.Entities.Modules;
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



namespace GIBS.Modules.GIBS_QR_Code
{
    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The View class displays the content
    /// 
    /// Typically your view control would be used to display content or functionality in your module.
    /// 
    /// View may be the only control you have in your project depending on the complexity of your module
    /// 
    /// Because the control inherits from GIBS_QR_CodeModuleBase you have access to any custom properties
    /// defined there, as well as properties from DNN such as PortalId, ModuleId, TabId, UserId and many more.
    /// 
    /// </summary>   GIBS_QR_CodeModuleBase 
    /// -----------------------------------------------------------------------------
    public partial class View : GIBS_QR_CodeModuleSettingsBase, IActionable
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (this.UserId > 0)
                    {
                        LinkButtonLoadProfile.Visible = true;
                        
                    }
                    

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

        // VGV4dEJveFRFWFQuVGV4dC5Ub1N0cmluZygp
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


        public void SaveVCard()
        {
            try
            {
                string address = txtStreet.Text.ToString();
                (string houseNumber, string streetName) = SplitAddress(address);
                
                ContactData generator = new ContactData(ContactData.ContactOutputType.VCard3, txtFirstName.Text.ToString(), txtLastName.Text.ToString(),
                    "", "", txtCellPhone.Text.ToString(), txtWorkPhone.Text.ToString(), txtEmail.Text.ToString(), null,txtWebsite.Text.ToString(),
                   streetName.ToString(), houseNumber.ToString(), txtCity.Text.ToString(), txtZip.Text.ToString(), null, null, 
                    txtState.Text.ToString(),ContactData.AddressOrder.Reversed,
                    txtBusiness.Text.ToString(), txtTitle.Text.ToString());
                
                string payload = generator.ToString();
                payload = payload.Replace("ADR;TYPE=HOME", "ADR;TYPE=WORK");
              //  payload = payload.Replace("TEL;TYPE=HOME", "TEL;TYPE=WORK");
              //  payload = payload.Replace("EMAIL:", "EMAIL;TYPE=INTERNET;TYPE=WORK:");
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
               
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);

                Bitmap qrCodeImage = qrCode.GetGraphic(20);
                
                string _QRCodeImage = PortalSettings.HomeDirectoryMapPath + txtLastName.Text.ToString().Replace(" ", "") + "_" + txtFirstName.Text.ToString().Replace(" ", "") + ".png";

              


                using (Bitmap bitMap = qrCode.GetGraphic(20))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        Image1.Visible = true;
                        bitMap.Save(ms, ImageFormat.Png);
                        //bitMap.Save(s, ImageFormat.Png);
                        byte[] byteImage = ms.ToArray();
                        Image1.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(byteImage);
                         
                        if (SaveQRCodeImage == true)
                        {

                            if (File.Exists(_QRCodeImage))
                            {
                                File.Delete(_QRCodeImage);
                            }

                            FileStream file = new FileStream(_QRCodeImage.ToString(), FileMode.Create, FileAccess.Write);

                            ms.WriteTo(file);
                            //img.Dispose();
                            
                            file.Close();
                            file.Dispose();
                        }
                        ms.Close();
                        ms.Dispose();

                    }

                   

                }

                string myEncodedString = Base64Encode(TextBoxTEXT.Text.ToString());
                LabelEncodedString.Text = payload.ToString();
               
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
                string email = TextBoxEMAIL.Text.ToString();

                if (email.Length > 0)
                {
                    email = "mailto:" + email;
                }


                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(TextBoxURL.Text.ToString()+ email.ToString() + TextBoxTEXT.Text.ToString(), QRCodeGenerator.ECCLevel.Q);
                
                QRCode qrCode = new QRCode(qrCodeData);

                Bitmap qrCodeImage = qrCode.GetGraphic(20);

                using (Bitmap bitMap = qrCode.GetGraphic(20))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        Image1.Visible = true;
                        bitMap.Save(ms, ImageFormat.Png);
                        byte[] byteImage = ms.ToArray();
                        Image1.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(byteImage);

                    }
                }

                string myEncodedString = Base64Encode(TextBoxTEXT.Text.ToString());
               
                LabelEncodedString.Text = TextBoxTEXT.Text.Replace(Environment.NewLine, "<br />").ToString();



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

        private string GeUserProfileString(string CustomProperty, int UserID)
        {
            UserInfo oUserInfo = UserController.GetUserById(PortalSettings.PortalId, UserID);
            string sCustomProperty = oUserInfo.Profile.GetPropertyValue(CustomProperty);
            return sCustomProperty;
        }




        protected void ddlQRType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(ddlQRType.SelectedValue == "Other")
            { 
                PanelOther.Visible = true;
                PanelVCard.Visible = false;
            }
            else
            {
                PanelOther.Visible = false;
                PanelVCard.Visible = true;
            }
        }

        protected void LinkButtonLoadProfile_Click(object sender, EventArgs e)
        {
            LoadProfile();
        }
    }
}