using System;
using System.Collections.Generic;
using System.IO;

namespace LibTradutorNetFramework
{
    internal class LegendasTxt2
    {
        StreamWriter sw;
        static int contador;

        public void EscreverLegendas(string path, List<Frase> frases)
        {
            using (sw = new StreamWriter(path, true))
            {
                foreach (var item in frases)
                {
                    Escrever(item, sw);
                }
            }
        }

        /* 
         Dialogue: 0,1:30:09.88,1:30:15.31,Default,,0,0,0,,Eu sabia que era, mas qual versão minha- 


Começo: _,hora :Minuto :Segundo . fração do segundo ,
Fim: Hora :Minuto :Segundo .Fração do segundo 
,Default,,0,0,0,,a frase
             */



        private void Escrever(Frase frase, StreamWriter sw)
        {

            foreach (Legenda item in frase.Legendas)
            {
                string outros = ",Default,,0,0,0,,";

                contador += 1;
                string inicio = calularTempo(item.Inicio);
                string fim = calularTempo(item.Fim);
                string legenda = $"{item.SentencaPortugues}{Environment.NewLine}";

                sw.Write(legenda);
            }


        }

        private string calularTempo(double TempoEmSegundos)
        {
            string output = "";

            double segundos = Math.Round(TempoEmSegundos % 60, 2);

            int minutos = (int)TempoEmSegundos / (60) % (60 * 60);

            int horas = (int)TempoEmSegundos / (60 * 60);



            string segundosstr = "";

            if (segundos >= 10)
            {
                segundosstr = segundos.ToString();
                segundosstr = segundosstr.Replace(",", ".");
            }
            else
            {
                segundosstr = "0" + segundos.ToString();
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