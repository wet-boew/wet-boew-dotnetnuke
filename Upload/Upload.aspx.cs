using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.FileSystem;

public partial class DesktopModules_WET_Upload_Upload : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString["Folder"] != null)
            {
                //DNN upload when provided with a GUID of the folder ID being used
                DotNetNuke.Services.FileSystem.FolderInfo FI = new DotNetNuke.Services.FileSystem.FolderInfo();

                string folder = Request.QueryString["Folder"].ToString();

                if (!String.IsNullOrEmpty(folder))
                {
                    FI = (DotNetNuke.Services.FileSystem.FolderInfo)FolderManager.Instance.GetFolder(Guid.Parse(folder));
                    FileManager.Instance.AddFile(FI, Request.Files[0].FileName, Request.Files[0].InputStream);
                    Response.Write("{\"success\":true}");
                }
                else
                {
                    Response.Write("{\"error\":\"" + "string fileName: cannot be empty or null" + "\"}");
                }
            }
            else if (Request.QueryString["FilePath"] != null)
            {
                //Uses standard asp file upload
                string filePath = null;
                byte[] rawFile = null;

                using (BinaryReader binaryReader = new BinaryReader(Request.Files[0].InputStream))
                {
                    rawFile = binaryReader.ReadBytes(Request.Files[0].ContentLength);
                    filePath = Request.Files[0].FileName.ToLower();
                }

                //Check if the folder exists, and if it doesn't create it
                string folder = Request.QueryString["FilePath"].ToString();
                if (Directory.Exists(Server.MapPath(folder)) == false)
                {
                    Directory.CreateDirectory(Server.MapPath(folder));
                }

                string msAddOn = DateTime.Now.Millisecond.ToString();
                string fileName = Path.GetFileNameWithoutExtension(filePath) + "_" + msAddOn + Path.GetExtension(filePath);

                if (!String.IsNullOrEmpty(fileName))
                {
                    //Checks if it is an image or not
                    if (folder.ToLower().Contains("/photos/"))
                    {
                        //Get Format
                        ImageFormat MyFormat = ImageFormat.Jpeg;
                        if (fileName.EndsWith(".png"))
                        {
                            MyFormat = ImageFormat.Png;
                        }
                        else if (fileName.EndsWith(".bmp"))
                        {
                            MyFormat = ImageFormat.Bmp;
                        }

                        //Resize image and create a thumbnail for it
                        System.Drawing.Image rawFileImage = ByteArrayToImage(rawFile); //Convert rawFile Byte array to Image to process Image Resize
                        System.Drawing.Image rawThumbFileImage = ByteArrayToImage(rawFile);
                        ResizeImage((Bitmap)rawFileImage, 900, 900, fileName).Save(Server.MapPath(folder + fileName), MyFormat); //regular
                        ResizeImage((Bitmap)rawThumbFileImage, 200, 200, fileName).Save(Server.MapPath(folder + "thumb_" + fileName), MyFormat); //thumbnail

                    }
                    else if (folder.ToLower().Contains("/videos/"))
                    {
                        //if it's the poster
                        if (fileName.EndsWith(".png") || fileName.EndsWith(".jpg"))
                        {
                            System.Drawing.Image rawFileImage = ByteArrayToImage(rawFile);
                            ResizeImage((Bitmap)rawFileImage, 900, 900, fileName).Save(Server.MapPath(folder + fileName), ImageFormat.Jpeg);
                        }
                        else
                        {
                            //if it's the video
                            File.WriteAllBytes(Server.MapPath(folder + fileName), rawFile);
                        }
                    }
                    else if (folder.ToLower().Contains("/slides/"))
                    {
                        System.Drawing.Image rawFileImage = ByteArrayToImage(rawFile);
                        ResizeImage((Bitmap)rawFileImage, 900, 900, fileName).Save(Server.MapPath(folder + fileName), ImageFormat.Jpeg);
                    }
                    else
                    {
                        File.WriteAllBytes(Server.MapPath(folder + fileName), rawFile);
                    }
                    Response.Write("{\"success\":true, \"msAddOn\":" + msAddOn + "}");
                    //syncs files right away so they can be viewed in the html editor.
                    int PID = DotNetNuke.Entities.Portals.PortalController.Instance.GetCurrentPortalSettings().PortalId;
                    string syncpath = Server.MapPath(folder);
                    //syncs the folder the file was added to.
                    DotNetNuke.Services.FileSystem.FolderManager.Instance.Synchronize(PID, syncpath, true, true);
                    //syncs the whole portal.
                    DotNetNuke.Services.FileSystem.FolderManager.Instance.Synchronize(PID);
                }
                else
                {
                    Response.Write("{\"errors\":\"" + "string fileName: cannot be empty or null" + "\"}");
                }
            }

        }
        catch (Exception ex)
        {
            Exceptions.LogException(ex);
            Response.Write("{\"error\":\"" + ex.Message + "\"}");
        }
    }

    public System.Drawing.Image ByteArrayToImage(byte[] byteArrayIn)
    {
        var ms = new MemoryStream(byteArrayIn);
        var returnImage = System.Drawing.Image.FromStream(ms);
        return returnImage;
    }

    public static Bitmap ResizeImage(Bitmap fullsizeImage, int maxWidth, int maxHeight, string imageName)
    {
        fullsizeImage.RotateFlip(RotateFlipType.Rotate180FlipNone);
        fullsizeImage.RotateFlip(RotateFlipType.Rotate180FlipNone);

        int imageWidth;
        int imageHeight;

        var newWidth = maxHeight * fullsizeImage.Width / fullsizeImage.Height;
        if (newWidth > maxWidth)
        {
            imageHeight = fullsizeImage.Height * maxWidth / fullsizeImage.Width;
            imageWidth = maxWidth;
        }
        else
        {
            imageWidth = newWidth;
            imageHeight = maxHeight;
        }
        System.Drawing.Image newImage = fullsizeImage.GetThumbnailImage(imageWidth, imageHeight, null, IntPtr.Zero);
        fullsizeImage.Dispose();

        return (Bitmap)newImage;
    }

    public static Byte[] ConvertImageToByteArray(Bitmap image)
    {
        var ic = new ImageConverter();
        return ic.ConvertTo(image, typeof(byte[])) as byte[];
    }
}