using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintBookApp.Common.Interfaces
{
    public interface ICameraService
    {
        Task<bool> CheckPermissions();
        Task<MediaFile> TakePhotoAsync();
        Task<MediaFile> PickFileAsync();
    }
}
