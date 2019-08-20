using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NReco.VideoConverter;

namespace LibTradutorNetFramework
{
    public class ConversorVideoAudio
    {
        public String ConverteerVideoAudio(string FilePath)
        {

                var ffMpeg = new NReco.VideoConverter.FFMpegConverter();

                string Extensao = Path.GetExtension(FilePath);

                ffMpeg.ConvertMedia(FilePath, FilePath.Replace(Extensao, ".wav"), "wav");

                return FilePath.Replace(Extensao, ".wav");

        }

     
    }

}