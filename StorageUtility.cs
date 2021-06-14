using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
public static class StorageUtility {
    private static string account = Environment.GetEnvironmentVariable("StorageAccount");
    private static string key = Environment.GetEnvironmentVariable("StorageKey");
    private static StorageSharedKeyCredential credential = new StorageSharedKeyCredential(account, key);
    public static bool IsImage(HttpRequest req) => 
        req.ContentType.Contains("image");
    public static async Task<string> CopyToImageContainer(MemoryStream s){
        string name = DateTime.Now.Ticks.ToString();
        Uri locale = new Uri($"https://{account}.blob.core.windows.net/images/{name}.png");
        BlobClient client = new BlobClient(locale, credential);
        await client.UploadAsync(s);
        return $"{name}.png";
    }
}