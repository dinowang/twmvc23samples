#r "System.Web"
#r "Microsoft.WindowsAzure.Storage"

using System;
using System.IO;
using System.Web;
using ImageResizer;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

public static void Run(ICloudBlob inputBlob, CloudBlobContainer container, TraceWriter log)
{
    log.Info($"Processing uploaded file: {inputBlob.Name}");

    // inputBlob.Name, 例： image.png
    var name = Path.GetFileNameWithoutExtension(inputBlob.Name);
    var ext = Path.GetExtension(inputBlob.Name);
    var mimeType = MimeMapping.GetMimeMapping(inputBlob.Name);

    using (var inputStream = new MemoryStream())
    using (var thumbStream = new MemoryStream())
    {
        // 讀取新進檔案
        inputBlob.DownloadToStream(inputStream);
        inputStream.Seek(0, SeekOrigin.Begin);

        // 圖片縮放處理
        var instructions = new Instructions
        {
            Width = 100,
            Mode = FitMode.Carve,
            Scale = ScaleMode.Both
        };

        var imageJob = new ImageJob(inputStream, thumbStream, instructions);
        imageJob.DisposeSourceObject = false;
        imageJob.Build();

        // thumbStream 指標歸零
        thumbStream.Seek(0, SeekOrigin.Begin);

        // 上傳 thumb 圖片
        var outputBlob = container.GetBlockBlobReference($"{name}-100.{ext}");
        outputBlob.UploadFromStream(thumbStream);

        // 設定 blob 物件屬性
        outputBlob.Properties.ContentType = mimeType;
        outputBlob.SetProperties();
    }
}
