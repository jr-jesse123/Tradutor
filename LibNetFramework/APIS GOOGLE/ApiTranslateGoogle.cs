


//using System;
//using System.Collections.Generic;
//using System.Text;
//using TranslationsResource = Google.Apis.Translate.v2.Data.TranslationsResource;




using System.Collections.Generic;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Translation.V2;

namespace LibTradutorNetFramework
{
    public sealed class ApiTransateGoogle
    {
        public static string Traduzir(string frase)
        {
            var cred = GoogleCredential.FromFile("./credential.json");
            TranslationClient client = TranslationClient.Create(cred);
            var response = client.TranslateText(frase, LanguageCodes.Portuguese);
            return response.TranslatedText;
        }
    }
    }
//        private static TranslationManager mInstance = null;
//        private static object mSyncObj = new object();

//        public static TranslationManager Instance
//        {
//            get
//            {
//                if (mInstance == null)
//                {
//                    lock (mSyncObj)
//                    {
//                        mInstance = new TranslationManager();
//                    }
//                }
//                return mInstance;
//            }
//        }

//        private TranslationManager()
//        {

//        }

//        private string GetApiKey()
//        {
//            return "AIzaSyABlPS0Zzj-KLzSe95Dit5OOma6MCSLv9w";
//        }

//        public KeyValuePair<string, string> Translate(string srcText, string target_language = "pt")
//        {
//            string[] text = new string[1] { srcText };
//            Dictionary<string, KeyValuePair<string, string>> translation = Translate(text, target_language);
//            return translation[srcText];
//        }

//        public Dictionary<string, KeyValuePair<string, string>> Translate(string[] srcText, string target_language = "en")
//        {

//            // Create the service.
//            var service = new TranslateService(new BaseClientService.Initializer()
//            {
//                ApiKey = GetApiKey(),
//                ApplicationName = "Translate API Your App"
//            });



//            TranslationsListResponse response = service.Translations.List(srcText, target_language).Execute();
//            Dictionary<string, KeyValuePair<string, string>> translations = new Dictionary<string, KeyValuePair<string, string>>();

//            int counter = 0;
//            foreach (TranslationsResource translation in response.Translations)
//            {
//                translations[srcText[counter]] = new KeyValuePair<string, string>(translation.TranslatedText, translation.DetectedSourceLanguage);
//                counter++;
//            }

//            return translations;
//        }
//    }
//}

////TranslationClient client = TranslationClient.Create();
////var response = client.TranslateText(frase, LanguageCodes.Portuguese);
////    return response.TranslatedText;
