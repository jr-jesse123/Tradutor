
using System;

public class eventargstring: EventArgs
{
    public string log;
}





//if (VerificarFrasesSalvasNoBanco())
//{
//    goto escreverlegendas;
//}




//private bool VerificarFrasesSalvasNoBanco()
//{
//    ListaFormatada = (List<SpeechForMongo>)mongo.LoadRecords<SpeechForMongo>(arquivo);
//    if (ListaFormatada.Count > 0)
//    {
//        Console.Write("Existe uma tradução com este nome presente no banco de Dados, deseja utilizá-la? (S)/(N)" + Environment.NewLine);
//        var opcao = Console.ReadLine();

//        if (opcao.Contains("S") | opcao.Contains("s"))
//        {
//            return true;
//        }
//        return false;
//    }
//    EnviandoArquivoNuvem?.Invoke(this, new EventArgs());
//    return false;
//}