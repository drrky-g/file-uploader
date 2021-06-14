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
    public static string[] formats = new string[] {
        ".jpeg", ".png", ".gif", ".jpeg"
    };
    public static bool IsImage(IFormFile file) => 
        file.ContentType.Contains("image") || 
        formats.Any(x => x == Path.GetExtension(file.FileName));
    public static async Task<string> CopyToImageContainer(IFormFile file){
        string ext = Path.GetExtension(file.FileName);
        string name = DateTime.Now.Ticks.ToString();
        using(Stream s = file.OpenReadStream()){
            Uri locale = new Uri($"https://{account}.blob.core.windows.net/images/{name}{ext}");
            BlobClient client = new BlobClient(locale, credential);
            await client.UploadAsync(s);
        }
        return name + ext;
    }
}