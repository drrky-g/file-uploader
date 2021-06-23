using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
public static class StorageUtility {
    private static string account = Environment.GetEnvironmentVariable("StorageAccount");
    private static string key = Environment.GetEnvironmentVariable("StorageKey");
    private static StorageSharedKeyCredential credential = new StorageSharedKeyCredential(account, key);
    public static bool IsImage(HttpRequest req) => req.ContentType.ToLower().Contains("image");
    public static async Task<string> CopyToImageContainer(MemoryStream s) {
        string name = $"{DateTime.Now.Ticks}.png";
        Uri uri = new Uri($"https://{account}.blob.core.windows.net/images/{name}");
        BlobClient client = new BlobClient(uri, credential);
        await client.UploadAsync(s);
        return name;
    }
}