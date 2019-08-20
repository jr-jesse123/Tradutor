using Google.Apis.Upload;
using Google.Cloud.Storage.V1;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LibTradutorNetFramework
{
    class GoogleStorageApi
    {
        public  event EventHandler<IUploadProgress> Evolucao;   
        public void Armazenar(string Path, string NomeArquivo)
        {
            StorageClient storageClient = StorageClient.Create();
            FileStream stream = File.OpenRead(Path);
            var progress = new Progress<IUploadProgress>(p => this.OnUploadProgress(p));
            var upload = storageClient.UploadObjectAsync("audios_para_traducao", NomeArquivo, "audio/wav", stream, progress: progress);
            upload.Wait();
        }


         void OnUploadProgress(IUploadProgress progress)
        {
            Evolucao?.Invoke(this, progress);
            
        }
    }

}
