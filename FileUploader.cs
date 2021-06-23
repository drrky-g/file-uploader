using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
namespace file_uploader {
    public static class FileUploader {
        [FunctionName("upload")]
        public static async Task<IActionResult> Upload ([HttpTrigger(AuthorizationLevel.Anonymous, "post")]HttpRequest req) {
            if(!StorageUtility.IsImage(req)) return new UnsupportedMediaTypeResult();
            if(req.Body.Length > 6e6) return new StatusCodeResult(StatusCodes.Status413PayloadTooLarge);
            if(req.Body.Length == 0) return new StatusCodeResult(StatusCodes.Status204NoContent);

            try {

                req.Body.Position = 0;
                byte[] buffer = new byte[req.Body.Length];
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
