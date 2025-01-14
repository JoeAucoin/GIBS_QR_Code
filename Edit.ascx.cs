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


using DotNetNuke.Services.Exceptions;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace GIBS.Modules.GIBS_QR_Code
{
    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The EditGIBS_QR_Code class is used to manage content
    /// 
    /// Typically your edit control would be used to create new content, or edit existing content within your module.
    /// The ControlKey for this control is "Edit", and is defined in the manifest (.dnn) file.
    /// 
    /// Because the control inherits from GIBS_QR_CodeModuleBase you have access to any custom properties
    /// defined there, as well as properties from DNN such as PortalId, ModuleId, TabId, UserId and many more.
    /// 
    /// </summary>
    /// -----------------------------------------------------------------------------
    public partial class Edit : GIBS_QR_CodeModuleSettingsBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                

                if (!IsPostBack)
                {
                    GridView1.PageSize = PageSize;
                }

                loadFileNames();
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }


        //PortalSettings.HomeDirectoryMapPath + "QRCode"

        private void loadFileNames()
        {
            string myPath = PortalSettings.HomeDirectoryMapPath + "QRCode";
            string myFilePath = PortalSettings.HomeDirectory + "QRCode/";
            
            String[] files = Directory.GetFiles(@myPath.ToString());
            int count = Directory.GetFiles(@myPath.ToString()).Length;
            LabelDebug.Text = count.ToString() +  " Files in: " + myFilePath;
            DataTable table = new DataTable();
                    
          //  table.Columns.Add(myPath + "<br />" + myFilePath);
         //   table.Columns.Add("Files in: " + myFilePath);
            table.Columns.Add("Files");
            table.Columns.Add("CreatedOn");
          
            for (int i = 0; i < files.Length; i++)
            {
                string myImage = "<img class='' alt='QR Code' width='350' height='350' border='1' src='";
                FileInfo file = new FileInfo(files[i]);
                DataRow dr = table.NewRow();
                dr[0] = myImage + myFilePath.ToString() + file.Name + "'><br />" + file.Name + " - " + file.CreationTime + "<br />&nbsp;";
                dr[0] = Context.Server.HtmlDecode(dr[0].ToString());
                dr[1] = file.CreationTime;
                table.Rows.Add(dr);
            }

            //     dataGridView1.DataSource = table;
            //     dataGridView1.DataBind();
            table.DefaultView.Sort = "CreatedOn DESC";
            GridView1.DataSource = table;
            GridView1.DataBind();

        }

        
        protected void GridView1_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            GridView1.DataBind();
        }
    }
}