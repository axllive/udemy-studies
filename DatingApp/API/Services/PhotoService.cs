using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace API.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly DBContext _context;
        private readonly Cloudinary _cloudnary;
        public PhotoService(IOptions<CloudnarySettings> configurations, DBContext context)
        {
            Account acc = new (
                configurations.Value.CloudName,
                configurations.Value.ApiKey,
                configurations.Value.ApiSecret
            );
            _cloudnary = new Cloudinary(acc);
            _context = context;
        }
        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
        {
            ImageUploadResult uploadResult = new();

            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                    ImageUploadParams uploadParams = new(){
                        File = new FileDescription(file.FileName, stream),
                        Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face"),
                        Folder = "da-net7"
                    };
                    uploadResult = await _cloudnary.UploadAsync(uploadParams);
            }
            
            return uploadResult;
        }

        public async Task<DeletionResult> DeletePhotoAsync(Photo photo)
        {   
            if(photo != null){
                _context.Entry(photo).State = EntityState.Deleted;
            }
            DeletionParams deletionParams = new(photo.PublicId);
            return await _cloudnary.DestroyAsync(deletionParams);
        }
    }
}