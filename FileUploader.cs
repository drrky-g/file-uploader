using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using System;
using Microsoft.AspNetCore.Authorization;

namespace file_uploader {
    public static class FileUploader {
        [AllowAnonymous]
        [HttpPost]
        [FunctionName("upload")]
        public static async Task<IActionResult> Upload ([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req) {
            try{
                if(req.Form.Files.Count == 1 &&
                StorageUtility.IsImage(req.Form.Files[0]) &&
                req.Form.Files[0].Length > 0) {
                    if(req.Form.Files[0].Length >  9e6) { //9 megabytes
                        return new StatusCodeResult(StatusCodes.Status413PayloadTooLarge);
                    }
                    string fileName = await StorageUtility.CopyToImageContainer(req.Form.Files[0]);
                    return new OkObjectResult(fileName);
                }
                return new UnsupportedMediaTypeResult();
            }catch(Exception e) {
                return new BadRequestObjectResult(e.Message);
            }
        }
        
    }
}
