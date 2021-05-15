using Google.Apis.Auth.OAuth2;
using Google.Cloud.Speech.V1;
using Google.LongRunning;
using Google.Protobuf.Collections;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LibTradutorNetFramework
{
    public class ApiGoogleSpeech
        
    {
        public static event EventHandler<int> Progresso;
        public static RepeatedField<SpeechRecognitionResult> AsyncRecognizeGcs(string NomeDoArquivo)
       {

            var cred = GoogleCredential.FromFile("./credential.json");
            var builder = new SpeechClientBuilder() { CredentialsPath = "./credential.json" };
            var speech = builder.Build();
            //var speech = SpeechClient.Create();
            Operation<LongRunningRecognizeResponse, LongRunningRecognizeMetadata>  longOperation;
            try
            {
                longOperation = speech.LongRunningRecognize(new RecognitionConfig()
                {
                    LanguageCode = "en",
                    EnableWordTimeOffsets = true,
                    EnableAutomaticPunctuation = true,
                    AudioChannelCount = 2,
                }, RecognitionAudio.FromStorageUri($@"gs://audios_para_traducao/{NomeDoArquivo}")); //($@"gs://audios_para_traducao/{NomeDoArquivo}"
            }
            catch (Exception)
            {
                longOperation = speech.LongRunningRecognize(new RecognitionConfig()
                {
                    LanguageCode = "en",
                    EnableWordTimeOffsets = true,
                    EnableAutomaticPunctuation = true,
                    AudioChannelCount = 1,
                }, RecognitionAudio.FromStorageUri($@"gs://audios_para_traducao/{NomeDoArquivo}")); //($@"gs://audios_para_traducao/{NomeDoArquivo}"
            }


            while (!longOperation.IsCompleted)
            {
                longOperation = longOperation.PollOnce();
                var porcentagem = longOperation.Metadata.ProgressPercent;
                Progresso?.Invoke(null, porcentagem);
                Thread.Sleep(100);
            }


            var response = longOperation.Result;
            
            return response.Results;
            
        }
    }

}
    






