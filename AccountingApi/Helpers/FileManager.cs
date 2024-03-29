﻿using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Helpers
{
    public class FileManager
    {
        public static string Upload(string FileRaw)
        {

            string data = "";
            try
            {
                data = FileRaw.Substring(0, 5);
            }
            catch (Exception)
            {

            }
            string ext = "";

            /*Define mimetype*/
            switch (data.ToUpper())
            {
                case "IVBOR":
                    ext = "png";
                    break;
                case "/9J/4":
                    ext = "jpg";
                    break;
                case "AAABA":
                    ext = "ico";
                    break;
                default:
                    return null;
            }
            string filename = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "." + ext;

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Uploads", filename);


            var imageDataByteArray = Convert.FromBase64String(FileRaw);

            if (imageDataByteArray.Length > 5000000)
            {
                return null;
            }
            ResizeOptions option = new ResizeOptions
            {
                Mode = ResizeMode.Max,
                Position = AnchorPositionMode.Center,
                Size = new SixLabors.Primitives.Size(250, 250)
            };

            using (Image<Rgba32> image = Image.Load(imageDataByteArray))
            {
                //while (image.size/time>100 && image.Height/time>100)
                //{
                //}
                image.Mutate(x => x
                     .Resize(option));
                image.Save(path);
            }

            //var imageDataStream = new MemoryStream(imageDataByteArray)
            //{
            //    Position = 0
            //};
            ////int i = FileName.LastIndexOf('.');
            ////FileName = FileName.Substring(0, i);
           

            //using (var file = File.Open(path, FileMode.Create, FileAccess.Write, FileShare.Read))
            //{
            //    imageDataStream.WriteTo(file);
            //}
            return filename;
        }

        public static bool Delete(string FileName)
        {
            var path = Path.Combine(
                          Directory.GetCurrentDirectory(), "wwwroot/Uploads",
                          FileName);
            if (File.Exists(path))
            {
                File.Delete(path);
                return true;
            }

            return false;
        }

        public static void FileSave(string filename, IFormFile file)
        {
            var stream = file.OpenReadStream();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", filename);

            FileStream filestream = new FileStream(path, FileMode.Create, System.IO.FileAccess.Write);
            stream.CopyTo(filestream);
        }
    }
}
