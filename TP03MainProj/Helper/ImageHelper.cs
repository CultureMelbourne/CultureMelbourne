using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;

namespace TP03MainProj.Helper
{
    public class ImageHelper
    {
        public static string GetImagePath(string title)
        {
            var imageExtensions = new List<string> { ".png", ".jpg", ".jpeg", ".webp" };
            var basePath = HttpContext.Current.Server.MapPath("~/Content/Images/events/");
            //title = title.Replace(" ", "_");

            foreach (var ext in imageExtensions)
            {
                var filePath = Path.Combine(basePath, title + ext);
                Debug.WriteLine("Checking file: " + filePath);

                if (File.Exists(filePath))
                {
                    Debug.WriteLine("File found: " + filePath);
                    return "~/Content/Images/events/" + title + ext;
                }
            }
            Debug.WriteLine("No matching file found for title: " + title);
            return "~/Content/Customer/images/services/1.webp"; // Path to a default image if no matching file is found
        }
    }
}