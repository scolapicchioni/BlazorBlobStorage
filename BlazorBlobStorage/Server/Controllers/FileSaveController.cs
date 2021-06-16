using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using BlazorBlobStorage.Server.Core.Interfaces;
using BlazorBlobStorage.Shared;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

[ApiController]
[Route("[controller]")]
public class FileSaveController : ControllerBase {
    private readonly IWebHostEnvironment env;
    private readonly ILogger<FileSaveController> logger;
    private readonly IPicturesService picturesService;

    public FileSaveController(IWebHostEnvironment env,
        ILogger<FileSaveController> logger, IPicturesService picturesService) {
        this.env = env;
        this.logger = logger;
        this.picturesService = picturesService;
    }

    [HttpPost]
    public async Task<UploadReply> PostFile([FromForm] IFormFile file) {
        Picture picture = new Picture() { 
            FileName = file.FileName,
            FileContent = file.OpenReadStream(),
            ContentType = file.ContentType
        };
        
        try {
            return await picturesService.UploadPicture(picture);
        } catch (Exception ex) {
            logger.LogError("{FileName} error on upload (Err: 3): {Message}", picture.FileName, ex.Message);
            return new UploadReply() { Message = $"{ex.GetType().Name}: {ex.Message}" };
        }
    }


}