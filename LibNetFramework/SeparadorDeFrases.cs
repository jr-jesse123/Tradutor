using Google.Cloud.Speech.V1;
using Google.Protobuf.Collections;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibTradutorNetFramework
{
    

    class SeparadorDeFrases
    {

        public static List<char> separadores = new List<char> { '.', '!', '?' };

        public static List<Frase> PrepararFrases(RepeatedField<WordInfo> ListaDePalavras)
        {
        List<Frase> frases = new List<Frase>();
        List<WordInfo> PalavrasDasentenca = new List<WordInfo>();
            foreach (var palavra in ListaDePalavras)
            {
                PalavrasDasentenca.Add(palavra);
                if (separadores.Contains(palavra.Word[palavra.Word.Length - 1]) | (ListaDePalavras.IndexOf(palavra)==ListaDePalavras.Count-1))
                {

                    WordInfo[] palavrasarray = PalavrasDasentenca.ToArray();
                    PalavrasDasentenca.CopyTo(palavrasarray);

                    frases.Add(new Frase(palavrasarray));
                    PalavrasDasentenca.Clear();
                }
            }
            
            return frases;
        }
        
    }
}

