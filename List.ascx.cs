using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GIBS.Modules.GIBS_QR_Code
{
    public partial class List : GIBS_QR_CodeModuleSettingsBase
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
            LabelDebug.Text = count.ToString() + " files in: " + myFilePath;
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