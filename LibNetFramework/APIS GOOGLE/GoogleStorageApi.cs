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
        private double size;
        public  event EventHandler<int> Evolucao;   
        public void Armazenar(string Path, string NomeArquivo)
        {
            size = new FileInfo(Path).Length;
            //***************************************
            StorageClient storageClient = StorageClient.Create();
            FileStream stream = File.OpenRead(Path);
            var progress = new Progress<IUploadProgress>(p => this.OnUploadProgress(p));
            var upload = storageClient.UploadObjectAsync("audios_para_traducao", NomeArquivo, "audio/wav", stream, progress: progress);
            upload.Wait();
            //***************************************
        }


        void OnUploadProgress(IUploadProgress progress)
        {
            

            int porcetagem = (int)((progress.BytesSent / size)*100);
            Evolucao?.Invoke(this, porcetagem);
        }
    }

}
