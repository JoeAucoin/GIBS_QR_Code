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
    /// </summary>
    /// -----------------------------------------------------------------------------
    public partial class View : GIBS_QR_CodeModuleBase, IActionable
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    ////    TextBox6.Text = GenerateAndGetString();

                    //    QRCodeGenerator qrGenerator = new QRCodeGenerator();
                    //    QRCodeData qrCodeData = qrGenerator.CreateQrCode(TextBoxTEXT.Text.ToString(), QRCodeGenerator.ECCLevel.Q);
                    //    QRCode qrCode = new QRCode(qrCodeData);

                    //    Bitmap qrCodeImage = qrCode.GetGraphic(20);

                    //    using (Bitmap bitMap = qrCode.GetGraphic(20))
                    //    {
                    //        using (MemoryStream ms = new MemoryStream())
                    //        {
                    //            Image1.Visible = true;
                    //            bitMap.Save(ms, ImageFormat.Png);
                    //            byte[] byteImage = ms.ToArray();
                    //            Image1.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(byteImage);
                    //        }
                    //    }
                    //}
                }
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
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


        public void SaveVCard()
        {
            try
            {
                ContactData generator = new ContactData(ContactData.ContactOutputType.VCard3, txtFirstName.Text.ToString(), txtLastName.Text.ToString(),
                    "", "", txtCellPhone.Text.ToString(), txtWorkPhone.Text.ToString(), txtEmail.Text.ToString(), null,txtWebsite.Text.ToString(),txtStreet.
                    Text.ToString(),null,txtCity.Text.ToString(), txtZip.Text.ToString(), null, null, txtState.Text.ToString(),ContactData.AddressOrder.Reversed,
                    txtBusiness.Text.ToString(), txtTitle.Text.ToString());
                string payload = generator.ToString();

                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                // QRCodeData qrCodeData = qrGenerator.CreateQrCode(TextBoxTEXT.Text.ToString(), QRCodeGenerator.ECCLevel.Q);
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.Q);
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
                LabelEncodedString.Text = payload.ToString();
                //    LabelEncodedString.Text = TextBoxTEXT.Text.Replace(Environment.NewLine, "<br />").ToString();


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
    }
}