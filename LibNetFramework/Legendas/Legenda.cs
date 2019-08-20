using System.Collections.Generic;

namespace LibTradutorNetFramework
{




public class Legenda
{
    public double Inicio;
    public double Fim;
        public double InicioAjustado;
        public double FimAjustado;
        public string SentencaPortugues;
    public string[] Comentarios;
    public List<int> NrDeCaracteresPorLinha;
    public double confidence;

    public Legenda(string SentencaPortugues)
    {

        this.SentencaPortugues = SentencaPortugues;
    }
}
}