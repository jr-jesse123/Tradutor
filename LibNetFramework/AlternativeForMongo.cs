using Google.Cloud.Speech.V1;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace LibTradutorNetFramework
{
    [BsonIgnoreExtraElements]
    public class AlternativeForMongo
    {
        public string transcript { get; set; }
        public double confidence { get; set; }
        public string Traducao;
        public double confidenceTraducao;
        public List<WordInfo> words = new List<WordInfo>();
        public List<Legenda> Legendas = new List<Legenda>();
        public double StartTime;
        public double EndTime;
        public double StartTimeAjustado;
        public double EndTimeAjustado;
        private const int MaxCaracteresPorLinhas = 50;
        private const int LinhasMax = 2;

        public void CriarLegenda()
        {

            string sentencaTemporaria = Traducao;
            string legenda = "";
            int cont = 0;

            foreach (var item in sentencaTemporaria)
            {
                if (sentencaTemporaria.Contains("Parábola da caverna, mas não vamos entrar nisso"))
                {
                    //System.Diagnostics.Debugger.Break();
                }

                cont += 1;
                if (item.Equals(' '))
                {
                    legenda += item;
                    if (cont >= MaxCaracteresPorLinhas)
                    {
                        int linhas = legenda.Split(new string[] { @"\N" },StringSplitOptions.RemoveEmptyEntries).Length;
                        
                        if (linhas >= LinhasMax) 
                        {
                            legenda += Environment.NewLine;
                            Legenda NovaLegenda = new Legenda(legenda){confidence = confidence};
                            Legendas.Add(NovaLegenda);
                            legenda = "";
                            cont = 0;
                        }
                        else
                        {
                            legenda += @"\N";
                            cont = 0;
                        }

                    }
                }
                
                else legenda += item;
            }

            if (legenda.Length>0)
            {
                legenda += Environment.NewLine;
                Legenda NovaLegenda = new Legenda(legenda)
                {
                    confidence = confidence
                };
                Legendas.Add(NovaLegenda);
                legenda = "";

            }


        }


        public void DistribuirTempo()
        {
            double Tempototal = calculartempo(words);

            long TotalCaracteres = 0;
            foreach (var legenda in Legendas)
            {
                TotalCaracteres += legenda.SentencaPortugues.Length;

              
            }
           

            double TPC = Tempototal / TotalCaracteres; //TEMPO POR CARACTER


          

            double tempoatual = words[0].StartTime.ToTimeSpan().TotalMilliseconds;
            

            foreach (var legenda in Legendas)
            {

          

                legenda.Inicio = tempoatual;
                legenda.InicioAjustado = tempoatual;

                double tempo = legenda.SentencaPortugues.Length * TPC;

                legenda.Fim = legenda.Inicio + tempo - (double)0.1;
                legenda.FimAjustado = legenda.Inicio + tempo - (double)0.1;

                tempoatual = legenda.Inicio + tempo;
            }
        }

        internal void EsticarTempoDeLegendasSolictarias(List<SpeechForMongo> listaFormatada, SpeechForMongo speech)
        {

            
            var IndiceAtual =  listaFormatada.IndexOf(speech);

            SpeechForMongo SpeechAnterior;
            SpeechForMongo ProximoSpeech;
            double UltimoTempo;
            double ProximoTempo;

            if (IndiceAtual != 0 & IndiceAtual != listaFormatada.Count - 1)
            {
                SpeechAnterior = listaFormatada[IndiceAtual - 1];
                
                UltimoTempo = SpeechAnterior.Possibilidades[SpeechAnterior.Possibilidades.Count-1]
                    .Legendas[SpeechAnterior.Possibilidades[SpeechAnterior.Possibilidades.Count - 1].Legendas.Count-1]
                    .FimAjustado;
                
                if (speech.Possibilidades[0].Legendas[0].InicioAjustado > UltimoTempo + 500)
                {
                    speech.Possibilidades[0].Legendas[0].InicioAjustado  -= 500;
                };


                ProximoSpeech = listaFormatada[IndiceAtual + 1];
                ProximoTempo = ProximoSpeech.Possibilidades[0].Legendas[0].Inicio;

                if (speech.Possibilidades[speech.Possibilidades.Count-1].
                    Legendas[speech.Possibilidades[speech.Possibilidades.Count-1].Legendas.Count-1]
                    .Fim < ProximoTempo - 500)
                {
                    speech.Possibilidades[0].Legendas[0].FimAjustado  += 500;
                }

                if (speech.Possibilidades[0].Legendas[0].FimAjustado != speech.Possibilidades[0].Legendas[0].Fim + 500 & speech.Possibilidades[0].Legendas[0].FimAjustado != speech.Possibilidades[0].Legendas[0].Fim  )
                {
                    System.Diagnostics.Debugger.Break();
                }
                else if (speech.Possibilidades[0].Legendas[0].InicioAjustado != speech.Possibilidades[0].Legendas[0].Inicio - 500 & speech.Possibilidades[0].Legendas[0].InicioAjustado != speech.Possibilidades[0].Legendas[0].Inicio)
                {
                    System.Diagnostics.Debugger.Break();
                }
                
            }
        }

        private double calculartempo(List<WordInfo> palavras)
        {  
            double _inicio= words[0].StartTime.ToTimeSpan().TotalMilliseconds;
            double _final = words[words.Count-1].EndTime.ToTimeSpan().TotalMilliseconds;

            return _final - _inicio;
        }
    }
}


