using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using System;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using System.Text;

namespace file_uploader {
    public static class FileUploader {
        [AllowAnonymous]
        [HttpPost]
        [FunctionName("upload")]
        public static async Task<IActionResult> Upload ([HttpTrigger(AuthorizationLevel.Anonymous, "post")][FromForm] HttpRequest req) {
            try{
                byte[] buffer = new byte[req.Body.Length];
                req.Body.Position = 0;
                await req.Body.ReadAsync(buffer, 0, buffer.Length);
                MemoryStream s = new MemoryStream(buffer);
                string fileName = await StorageUtility.CopyToImageContainer(s);
                return new OkObjectResult(fileName);

            }catch(Exception e) {
                return new BadRequestObjectResult(e.Message);
            }
        }
        
    }
}
