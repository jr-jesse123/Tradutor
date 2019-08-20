using Google.Cloud.Speech.V1;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibTradutorNetFramework
{
    [BsonIgnoreExtraElements]
    public class SpeechForMongo
    {
        public List<AlternativeForMongo>  Possibilidades = new List<AlternativeForMongo>();
        
        public SpeechForMongo(SpeechRecognitionResult speech)
        {
            foreach (SpeechRecognitionAlternative item in speech.Alternatives)
            {
                AlternativeForMongo AlternativeForMongo = new AlternativeForMongo()
                { confidence = item.Confidence, transcript = item.Transcript };

                foreach (WordInfo item2 in item.Words)
                {
                    AlternativeForMongo.words.Add(item2);
                }

                AlternativeForMongo.StartTime =  AlternativeForMongo.words[0].StartTime.ToTimeSpan().TotalSeconds;
                AlternativeForMongo.EndTime = AlternativeForMongo.words[AlternativeForMongo.words.Count-1].EndTime.ToTimeSpan().TotalSeconds;

                Possibilidades.Add(AlternativeForMongo);
            }
        }

    }
}


