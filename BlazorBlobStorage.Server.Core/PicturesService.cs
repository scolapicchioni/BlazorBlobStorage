using BlazorBlobStorage.Server.Core.Interfaces;
using BlazorBlobStorage.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorBlobStorage.Server.Core {
    public class PicturesService : IPicturesService {
        private readonly IPicturesRepository picturesRepository;
        public PicturesService(IPicturesRepository picturesRepository) => this.picturesRepository = picturesRepository;
        public Task<IEnumerable<Picture>> GetPictures() => picturesRepository.GetPictures();

        public Task<UploadReply> UploadPicture(Picture picture) => picturesRepository.UploadPicture(picture);
    }
}
