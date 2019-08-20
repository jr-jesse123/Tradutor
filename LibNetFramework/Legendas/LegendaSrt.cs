using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LibTradutorNetFramework
{
    public  class LegendasSRT
    {
        StreamWriter sw ;
        static int contador;

        public void EscreverLegendas(string path, List<Frase> frases)
        {
            using (sw = new StreamWriter(path, true))
            {
                foreach (var item in frases)
                {
                    Escrever(item, sw);
                    EscreverOriginal(item, sw);
                }
            }
        }




        private void Escrever(Frase frase, StreamWriter sw )
        {
            foreach (Legenda item in frase.Legendas)
            {
                contador += 1;

                string inicio = calularTempo(item.InicioAjustado);
                string fim = calularTempo(item.FimAjustado);

                string legenda = contador.ToString() + Environment.NewLine + inicio + " --> " + fim + Environment.NewLine;
                legenda += item.SentencaPortugues + Environment.NewLine + Environment.NewLine;

                sw.Write(legenda);
            }

           
        }

        private void EscreverOriginal(Frase frase, StreamWriter sw)
        {
            foreach (Legenda item in frase.Legendas)
            {
                contador += 1;

                string inicio = calularTempo(item.Inicio);
                string fim = calularTempo(item.Fim);

                string legenda = contador.ToString() + Environment.NewLine + inicio + " --> " + fim + Environment.NewLine;
                legenda += item.SentencaPortugues + Environment.NewLine + Environment.NewLine;

                sw.Write(legenda);
            }


        }


        private string calularTempo(double TempoEmSegundos)
        {
            string output="";

            double segundos = Math.Round(TempoEmSegundos % 60, 2);

            int minutos = (int)TempoEmSegundos / (60) % (60 * 60);


            int horas = (int)TempoEmSegundos / (60 * 60);



            string segundosstr = "";

            if (segundos>=10)
            {
                segundosstr = segundos.ToString();
            }
            else
            {
                segundosstr = "0"+segundos.ToString();
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


            string horasstr = "0"+ horas.ToString();

            output = $"{horasstr}:{minutosstr}:{segundosstr}";


            return output;
        }
    }
}
