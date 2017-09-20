using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls;

namespace WebComment.Commons
{
    public class StaticHelper
    {

        public static string ImageUrl(string pathImage, int size, bool logo)
        {
            string filename = Path.GetFileName(pathImage);
            if (!String.IsNullOrEmpty(filename))
            {
                if (logo)
                {
                    if (pathImage.Contains("~/Upload"))
                        pathImage = pathImage.Replace("~/Upload", "/img");
                    else
                        pathImage = pathImage.Replace("~/", "/img/");

                    return Globals.StaticUrl + pathImage.Replace(filename, size + "_" + filename);
                }
                else
                {
                    if (pathImage.Contains("~/Upload"))
                        pathImage = pathImage.Replace("~/Upload", "/image");
                    else
                        pathImage = pathImage.Replace("~/", "/image/");
                }
                return Globals.StaticUrl + pathImage.Replace(filename, size + "_" + filename);
            }
            else
            {
                filename = "";
                return filename;
            }

        }

    }
}