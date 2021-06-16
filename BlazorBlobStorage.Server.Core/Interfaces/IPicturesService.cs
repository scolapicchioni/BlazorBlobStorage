using BlazorBlobStorage.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorBlobStorage.Server.Core.Interfaces {
    public interface IPicturesService {
        Task<UploadReply> UploadPicture(Picture picture);
        Task<IEnumerable<Picture>> GetPictures();
    }
}
