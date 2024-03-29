﻿
using Google.Apis.Upload;
using Google.Cloud.Speech.V1;
using Google.Protobuf.Collections;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;



namespace LibTradutorNetFramework
{
    public class Tradutor
    {
        
        public event EventHandler EnviarLog;
        public event EventHandler Finalizado;
        public event EventHandler EnviarErro;
        public event EventHandler<int> Progresso;
        
        

        public List<SpeechForMongo> ListaFormatada = new List<SpeechForMongo>();
        public static List<Frase> frases = new List<Frase>();
        private List<Frase> listaFrasesSalvas = new List<Frase>();
        private string arquivo;
        private MongoDb mongo = new MongoDb("TRADUÇÃO");
        

        public void traduzirPorBlocos(string arquivo)
        {
            this.arquivo = arquivo;
            string path =  arquivo;

#if RELEASE
            try
#endif
            {

            EnviarLog?.Invoke(this, new eventargstring() { log = "Enviando Arquivos Para Núvem"});

            GoogleStorageApi GoogleStorageApi = new GoogleStorageApi();
                GoogleStorageApi.Evolucao += GoogleStorageApi_Evolucao;
            GoogleStorageApi.Armazenar(path, arquivo);

                EnviarLog?.Invoke(this, new eventargstring() { log = "Arquivos Enviados Para núvem, Solicitando Trancrição" });

                ApiGoogleSpeech.Progresso += ApiGoogleSpeech_Progresso;
            RepeatedField<SpeechRecognitionResult> data = ApiGoogleSpeech.AsyncRecognizeGcs(arquivo);


            ListaFormatada = new List<SpeechForMongo>();
                
            foreach (SpeechRecognitionResult item in data)
            {
                SpeechForMongo itemformatado = new SpeechForMongo(item);
                ListaFormatada.Add(itemformatado);
            }

            EnviarLog?.Invoke(this, new eventargstring() { log = "Transcrições recebidas, Iniciando Tradução"});
            TraduzirFrasesCompletas(ListaFormatada);


            foreach (var item in ListaFormatada)
            {
                mongo.InserRecord<SpeechForMongo>(arquivo, item);
            }

            EnviarLog?.Invoke(this, new eventargstring() { log = "Traduções Recebidas, Escrevendo Legendas...." });

            CriarLegendasCompletas(ListaFormatada);

            EscreverLegendas();
            }
#if RELEASE
            catch (Exception e)
            {
                EnviarErro.Invoke(this, new eventargstring() { log = e.Message + "    " + e.StackTrace });
            }
#endif
        }

        private void ApiGoogleSpeech_Progresso(object sender, int e)
        {
            Progresso?.Invoke(this, e);
        }

        private void GoogleStorageApi_Evolucao(object sender, int e)
        {
            Progresso?.Invoke(this, e);
        }

        private void CriarLegendasCompletas(List<SpeechForMongo> ListaFormatada)
        {
            foreach (var speech in ListaFormatada)
            {
                foreach (var possibilidade in speech.Possibilidades)
                {
                    possibilidade.CriarLegenda();
                    possibilidade.DistribuirTempo();
                }

            }

            foreach (var speech in ListaFormatada)
            {
                foreach (var possibilidade in speech.Possibilidades)
                {
                    possibilidade.EsticarTempoDeLegendasSolictarias(ListaFormatada, speech);
                }
            }
        }

        private void TraduzirFrasesCompletas(List<SpeechForMongo> listaFormatada)
        {
            

            foreach (var item in listaFormatada)
            {
                var a = listaFormatada.IndexOf(item);
                var b = listaFormatada.Count;
                
                int porcentagem = (int)((a / (double)b) * 100);
                Progresso?.Invoke(this, porcentagem);

                
                foreach (var possibilidade in item.Possibilidades)
                {
                    possibilidade.Traducao = ApiTransateGoogle.Traduzir(possibilidade.transcript);
                }
            }
        }

        public void resgatarTraducao(string NomeDoARquivo)
        {
               ListaFormatada = (List<SpeechForMongo>)mongo.LoadRecords<SpeechForMongo>(NomeDoARquivo);
            this.arquivo = Directory.GetCurrentDirectory() + @"\"+ NomeDoARquivo;

            CriarLegendasCompletas(ListaFormatada);

            EscreverLegendas();
        }

        private void EscreverLegendas()
        {
            string path =  arquivo;

            path = path.Replace(arquivo, arquivo.Replace(Path.GetExtension(arquivo), ".txt"));

            LegendasTxt legendasTxt = new LegendasTxt();
            legendasTxt.EscreverLegendas(path, ListaFormatada);            
            Finalizado?.Invoke(this, new EventArgs());

        }

        public void TraduzirVideo(string filepath)
        {
#if RELASE
            try
#endif
            {

                ConversorVideoAudio conversor = new ConversorVideoAudio();

                EnviarLog?.Invoke(this, new eventargstring() { log = "Convertendo Vídeo Para Áudio" });

                string NovoArquivo = conversor.ConverteerVideoAudio(filepath);

                EnviarLog?.Invoke(this, new eventargstring() { log = "Vídeo Convertido em áudio" });

                traduzirPorBlocos(NovoArquivo);

                Finalizado?.Invoke(this, EventArgs.Empty);
            }


#if RELEASE

            catch (Exception e)
            {
                EnviarErro.Invoke(this, new eventargstring() { log = e.Message + "    " + e.StackTrace });
            }

#endif
        
        }
    }
}
