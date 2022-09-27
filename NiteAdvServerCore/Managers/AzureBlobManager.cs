using System;
using System.Collections;
using System.Reflection.Metadata;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using NiteAdvServerCore.Entities;

namespace NiteAdvServerCore.Managers;

public static class AzureBlobManager
{
    static string AZURE_STORAGE_CONNECTION_STRING = "DefaultEndpointsProtocol=https;AccountName=niteimagestorage;AccountKey=AAXB9j3GijeGl9ctKPrZgSXf6ddmUCtKz5AhLbhZOEyegDtDz6IFAxrkyMLbRGTBWWW3/0CQ16JkN7dEadXYWA==;EndpointSuffix=core.windows.net";
    static string containerNameImage = "images";//storage.bucket(firebaseConfig.storageBucket);
    static string containerNameBlob = "places-list";
    static string baseAddressImages = "https://niteimagestorage.blob.core.windows.net/images/";

    public async static Task<string> SaveImage(byte[] image, ImageContext fcontext)
    {
        string resultFileName = baseAddressImages;
        var blobServiceClient = new BlobServiceClient(AZURE_STORAGE_CONNECTION_STRING);

        var containerClient = blobServiceClient.GetBlobContainerClient(containerNameImage);
        //const blobName = "newblob" + new Date().getTime();
        var fileName = String.Empty;
        if (fcontext == ImageContext.Company)
            fileName = $"company-img-{Guid.NewGuid()}.jpg";
        else if(fcontext == ImageContext.User)
            fileName = $"user-img-{Guid.NewGuid()}.jpg";
        else if (fcontext == ImageContext.Event)
            fileName = $"event-img-{Guid.NewGuid()}.jpg";


        BlockBlobClient blockBlobClient = containerClient.GetBlockBlobClient("./" + fileName);
        BlobContentInfo uploadBlobResponse;
        try
        {
            var blobHttpHeader = new BlobHttpHeaders { ContentType = "image/jpeg" };

            uploadBlobResponse = await blockBlobClient.UploadAsync(new MemoryStream(image), new BlobUploadOptions { HttpHeaders = blobHttpHeader });
            if (uploadBlobResponse != null)
                resultFileName += fileName;
            else
                resultFileName = "";



        }
        catch (Exception ex)
        {
            resultFileName = "";
            Console.WriteLine("upload blob eror: " + ex.Message);
        }
        return resultFileName;
    }

    public enum ImageContext
    {
        Company,
        Event,
        User
    }
}
