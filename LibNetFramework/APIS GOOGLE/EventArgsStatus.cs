using Google.Apis.Upload;
using System;

namespace LibTradutorNetFramework
{
    public class EventArgsStatus : EventArgs
    {
        public IUploadProgress progress;
    }

}
