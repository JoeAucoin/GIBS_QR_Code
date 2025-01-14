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
using System;

namespace GIBS.Modules.GIBS_QR_Code
{
    public class GIBS_QR_CodeModuleSettingsBase : ModuleSettingsBase
    {
        public int PageSize
        {
            get
            {
                if (Settings.Contains("PageSize"))
                    return Convert.ToInt32(Settings["PageSize"]);
                return 10;
            }
            set
            {
                var mc = new ModuleController();
                mc.UpdateModuleSetting(ModuleId, "PageSize", value.ToString());
            }
        }

        public string GoogleAPIKey
        {
            get
            {
                if (Settings.Contains("GoogleAPIKey"))
                {
                    return Settings["GoogleAPIKey"].ToString();
                }
                return "";
            }

            set
            {
                var mc = new ModuleController();
                mc.UpdateModuleSetting(ModuleId, "GoogleAPIKey", value.ToString());
            }

        }

        public string DefaultQRType
        {
            get
            {
                if (Settings.Contains("DefaultQRType"))
                {
                    return Settings["DefaultQRType"].ToString();
                }
                return "";
            }

            set
            {
                var mc = new ModuleController();
                mc.UpdateModuleSetting(ModuleId, "DefaultQRType", value.ToString());
            }

        }

        public bool ShowLoadProfile
        {
            get
            {
                if (Settings.Contains("ShowLoadProfile"))
                {
                    return Convert.ToBoolean(Settings["ShowLoadProfile"]);
                }
                return true;
            }

            set
            {
                var mc = new ModuleController();
                mc.UpdateModuleSetting(ModuleId, "ShowLoadProfile", value.ToString());
            }

        }
        //SaveQRCodeImage
        public bool SaveQRCodeImage
        {
            get
            {
                if (Settings.Contains("SaveQRCodeImage"))
                {
                    return Convert.ToBoolean(Settings["SaveQRCodeImage"]);
                }
                return true;
            }

            set
            {
                var mc = new ModuleController();
                mc.UpdateModuleSetting(ModuleId, "SaveQRCodeImage", value.ToString());
            }

        }


    }
}