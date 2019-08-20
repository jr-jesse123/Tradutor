using System;
using System.Collections.Generic;
using System.IO;

namespace LibTradutorNetFramework
{
    internal class LegendasTxt
    {
        StreamWriter sw;
        
        public void EscreverLegendas(string path, List<SpeechForMongo> ListaFormatada)
        {
            using (sw = new StreamWriter(path, true))
            {
                foreach (SpeechForMongo item in ListaFormatada)
                {
                    Escrever(item, sw);
                }
            }
        }


        private void Escrever(SpeechForMongo speech, StreamWriter sw)
        {

            foreach (AlternativeForMongo item in speech.Possibilidades)
            {
                if (item.Traducao.Contains("Qualquer vitalidade no campo de energia interior sugere algum dia começar com essas mãos e sentir que uma vida"))
                {
                    System.Diagnostics.Debugger.Break();
                }
                if (speech.Possibilidades.IndexOf(item) == 0)
                {
                    foreach (Legenda legenda in item.Legendas)
                    {
                        GerarLegenda(legenda);

                        if (item.confidence<.90)
                        {
                            GerarComentarioDaLegenda(legenda);
                        }

                        
                    }
                   
                }
                else
                {
                    foreach (Legenda legenda in item.Legendas)
                    {
                        GerarLegenda(legenda);
                        GerarLegendasAlternativas(legenda);
                    }   
                }
            }
        }

        private void GerarLegendasAlternativas(Legenda item)
        {
            string inicio = calularTempo(item.Inicio);
            string fim = calularTempo(item.Fim);

            string legenda = $"Comment: 0,{inicio},{fim},Default,,0,0,0,," +
                $"Alteranativa: {item.SentencaPortugues}{Environment.NewLine} Confiança: {Math.Round(item.confidence, 2)}{Environment.NewLine}";

            sw.Write(legenda);
        }

        private void GerarComentarioDaLegenda(Legenda item)
        {
        
            //contador += 1;

            string inicio = calularTempo(item.Inicio);
            string fim = calularTempo(item.Fim);

            //string DestaqueConfianca = AvaliarConfiancalegenda(item);
            
            string legenda = $"Comment: 0,{inicio},{fim},Default,,0,0,0,," +
                $"Confiança da Transcrição :{Math.Round(item.confidence, 2)}{Environment.NewLine}";
            
            sw.Write(legenda);
        }

        //private string AvaliarConfiancalegenda(Legenda item)
        //{
        //    string output ="";

            
        //    if (item.confidence <.78)
        //    {
        //        output = "***";
        //    }
        //    else if (item.confidence <.85)
        //    {
        //        output = "**";
        //    }
        //    else if (item.confidence < .88)
        //    {
        //        output = "*";
        //    }

        //    return output;
        //}

        private void GerarLegenda(Legenda item)
        {
            //contador += 1;

            //string DestaqueConfianca = AvaliarConfiancalegenda(item);

            string inicio = calularTempo(item.Inicio);
            string fim = calularTempo(item.Fim);

            //string inicioajustado = calularTempo(item.InicioAjustado);
            //string fimajustado = calularTempo(item.FimAjustado);

            string legenda = $"Dialogue: 0,{inicio},{fim},Default,,0,0,0,," +
                $"{item.SentencaPortugues}{Environment.NewLine}";

             //legenda += $"Dialogue: 0,{inicioajustado},{fimajustado},Default,,0,0,0,," +
             //  $"{item.SentencaPortugues}{Environment.NewLine}";

            sw.Write(legenda);
        }

        private string calularTempo(double TempoEmMiliSegundos)
        {

            double TempoEmSegundos = TempoEmMiliSegundos / 1000;

            string output = "";

            double segundos = Math.Round(TempoEmSegundos % 60, 2);

            var minutos = (int)(TempoEmSegundos  % (60 * 60) / 60);

            int horas = (int)(TempoEmSegundos / (60 * 60 ));




            string segundosstr = "";

            if (segundos >= 10)
            {
                segundosstr = segundos.ToString();
                segundosstr = segundosstr.Replace(",", ".");
            }
            else
            {
                segundosstr = "0" + segundos.ToString();
                segundosstr = segundosstr.Replace(",", ".");
            }

            string minutosstr = "";

            if (minutos >= 10)
            {
                minutosstr = minutos.ToString();
            }
            else
            {
                minutosstr = "0" + minutos.ToString();
            }


            string horasstr = "0" + horas.ToString();

            output = $"{horasstr}:{minutosstr}:{segundosstr}";


            return output;
        }
    }
}