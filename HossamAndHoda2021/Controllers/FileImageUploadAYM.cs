using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace HossamAndHoda2021.Controllers
{
    public class FileImageUploadAYM
    {
        public static bool ImageISTrue(HttpPostedFileBase Category1ImgURL = null)
        {
            var result = false;
            if (Category1ImgURL.ContentType == "image/jpg" ||
                Category1ImgURL.ContentType == "image/jpeg" ||
                Category1ImgURL.ContentType == "image/png" ||
                Category1ImgURL.ContentType == "image/bmp")
            {
                result = true;
            }
            return result;
        }


        public static bool PDFISTrue(HttpPostedFileBase Category1ImgURL = null)
        {
            var result = false;
            if (Path.GetExtension(Category1ImgURL.FileName) == ".pdf" ||
                Path.GetExtension(Category1ImgURL.FileName) == ".PDF"
               )
            {
                result = true;
            }
            return result;
        }


        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var resultImage = new Bitmap(width, height);
            resultImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);
            using (var graphics = Graphics.FromImage(resultImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }
            return resultImage;
        }


        public static string ADDIMAGE(string OldIMGURL = null, HttpPostedFileBase fileNewIMG = null, string foldername = null, int width = 0, int height = 0)
        {

            string fullPath = System.Web.HttpContext.Current.Server.MapPath("~/Images/" + foldername + "/" + OldIMGURL);

            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }

            if (!Directory.Exists("~/Images/" + foldername))
            {
                Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath("~/Images/" + foldername));
            }

            //
            //get input Strean from file object and call the function as given below
            Image resizeImage = Image.FromStream(fileNewIMG.InputStream, true, true);
            resizeImage = ResizeImage(resizeImage, width, height);
            //
            string _FileName = foldername + Guid.NewGuid() + Path.GetExtension(fileNewIMG.FileName);
            string _path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Images/" + foldername + "/"), _FileName);
            resizeImage.Save(_path, System.Drawing.Imaging.ImageFormat.Jpeg);
            return _FileName;
        }

        public static void DeleteImagetoClean(string foldername = null, string IMGname = null)
        {
            string fullPath = System.Web.HttpContext.Current.Server.MapPath("~/Images/" + foldername + "/" + IMGname);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
        }


    }
}