using Google.Cloud.Speech.V1;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace LibTradutorNetFramework
{
    [BsonIgnoreExtraElements]
    public class Frase
    {   
        public string SentencaIngles;
        public string SentencaPorugues;
        public List<Legenda> Legendas = new List<Legenda>();
        public WordInfo[] Palavras;
        private  const int LinhasMax = 3;
        private const int MaxCaracteresPorLinhas = 60;

        public Frase(WordInfo[] Palavras)
        {
            foreach (var palavra in Palavras)
            {
                SentencaIngles += palavra.Word + " ";
            }
            this.Palavras = Palavras;
        }

        public void CriarLegendas()
        {
            string sentencaTemporaria = SentencaPorugues;
            string legenda = "";
            int cont = 0;

            foreach (var item in sentencaTemporaria)
            {
                cont += 1;
                if (item.Equals(' '))
                {
                    legenda += item;
                    if (cont >= MaxCaracteresPorLinhas)
                    {
                        legenda += @"\N";
                        cont = 0;

                        var X = (Char.ConvertFromUtf32(10));
                        int linhas = legenda.Split(new string[] { @"\N" }, StringSplitOptions.RemoveEmptyEntries).Length - 1;
                        if (linhas >= LinhasMax)
                        {
                            Legenda NovaLegenda = new Legenda(legenda);
                            Legendas.Add(NovaLegenda);
                            legenda = "";
                            cont = 0;
                        }
                    }
                }
                else legenda += item;
            }

            int SomaCaracteres =0;
            foreach (var item in Legendas)
            {
                SomaCaracteres += item.SentencaPortugues.Length;
            }

            if (SentencaPorugues.Length > SomaCaracteres)
            {
                legenda =  SentencaPorugues.Substring(SomaCaracteres, SentencaPorugues.Length - SomaCaracteres);
                Legenda NovaLegenda = new Legenda(legenda);
                Legendas.Add(NovaLegenda);
                legenda = "";
            }


        }
        public void DistribuirTempos()
        {
            double Tempototal = calculartempo(Palavras);

            long TotalCaracteres = 0;
                foreach (var legenda in Legendas)
            {
                TotalCaracteres += legenda.SentencaPortugues.Length;
            }

            double TPC = Tempototal / TotalCaracteres; //TEMPO POR CARACTER

            
            double tempoatual = Palavras[0].StartTime.Seconds;
            foreach (var legenda in Legendas)
            {
                legenda.Inicio = tempoatual;

                double tempo = legenda.SentencaPortugues.Length * TPC;

                legenda.Fim = legenda.Inicio + tempo - (double)0.1;

                tempoatual = legenda.Inicio + tempo;
            }
        }

        private double calculartempo(WordInfo[] palavras)
        {

            List<int> inicionumeros = new List<int>();
            List<int> finalnumeros = new List<int>();

            string[] inicio = Palavras[0].StartTime.Seconds.ToString().Split(new char[] { 's', '.' , '"' });
            string[] fim = Palavras[palavras.Length-1].EndTime.ToString().Split(new char[] { 's', '.', '"' });

            foreach (var item in inicio)
            {
                if (item != string.Empty)
                {
                    var x = Convert.ToInt32(item);
                    inicionumeros.Add(x);
                }
            }
            
            foreach (var item in fim)
            {
                if (item != string.Empty)
                {
                    var x = Convert.ToInt32(item);
                    finalnumeros.Add(x);
                }
            }
            
            double _inicio;

            if (inicionumeros.Count>1)
            {
                _inicio = Convert.ToInt32(inicionumeros[0]) + (Convert.ToInt32(inicionumeros[1]) * 0.001);
            }
            else if (inicionumeros.Count == 1)
            {
                _inicio = Convert.ToInt32(inicionumeros[0]);
            }
            else 
            {
                _inicio = 0;
            }
            
            double _final =0;

            if (finalnumeros.Count>1)
            {
                _final = Convert.ToInt32(finalnumeros[0]) + (Convert.ToInt32(finalnumeros[1]) * 0.001);
            }
            else if (finalnumeros.Count == 1)
            {
                _final = Convert.ToInt32(finalnumeros[0]);
            }
            else
            {
                _final = 0;
            }


            return _final - _inicio;

        }
    }
}
