using Google.Cloud.Speech.V1;
using Google.Protobuf.Collections;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibTradutorNetFramework
{
    class Program
    {
        
        static void Main(string[] args)
        {

            //try
            {
                Console.Write($"Olá, por favor digite abaixo o nome do arquivo que deseja traduzir com a extenção (.wav) {Environment.NewLine}" +
              $"Lembre-se que o arquivo precisa estar na mesma pasta do programa. Digite enter quando terminar." + Environment.NewLine);

                var arquivo = Console.ReadLine();

                Tradutor tradutor = new Tradutor();

                //tradutor.EnviandoArquivoNuvem += Tradutor_EnviandoArquivoNuvem;
                //tradutor.ArquivosEnviadosParaNuvem += Tradutor_ArquivosEnviadosParaNuvem;
                //tradutor.TranscricaoObtida += Tradutor_TranscricaoObtida;
                //tradutor.SolicitandoTraducao += Tradutor_SolicitandoTraducao;
                //tradutor.TranscricaoObtida += Tradutor_TranscricaoObtida1;
                //tradutor.Finalizado += Tradutor_Finalizado;
                //tradutor.traduzirPorBlocos(arquivo);
            }
          //  catch  (Exception e)
            {

            //    Console.Write(e.Message + Environment.NewLine + e.StackTrace);

              //  Console.ReadLine();
            }

           
         
        }

        private static void Tradutor_Finalizado(object sender, EventArgs e)
        {
            Console.WriteLine("Legendas escritas, aperte qualquer tecla pra encerrar");
            Console.ReadLine();
        }

        private static void Tradutor_TranscricaoObtida1(object sender, EventArgs e)
        {
            Console.Write("Traduções Obtidas, escrevendo arquivos de Legendas" + Environment.NewLine);
        }

        private static void Tradutor_SolicitandoTraducao(object sender, EventArgs e)
        {
            Console.Write("Frases Separadas, Solicitando Tradução...." + Environment.NewLine +
             "Esta etapa deve durar cerca de 30 minutos....");
        }

        private static void Tradutor_TranscricaoObtida(object sender, EventArgs e)
        {
            Console.Write("Trancrição Obtida, separando frases agora...." + Environment.NewLine);
        }

        private static void Tradutor_ArquivosEnviadosParaNuvem(object sender, EventArgs e)
        {
            Console.Write("Arquivo enviado, Solicitando Transcrição,..." + Environment.NewLine
         + "esta estapa deve durar cerca de 1 hora" + Environment.NewLine);
        }

        private static void Tradutor_EnviandoArquivoNuvem(object sender, EventArgs e)
        {
            Console.Write(Environment.NewLine + "Arquivo sendo enviado à núvem, esta etapa deve demorar cerca de 20 minutos....");
        }

        //private static async Task CheckForUpdates()
        //{
        //    using (var manager = new UpdateManager(@"c:/teste/"))
        //    {

        //    }
        //}
    }
}

