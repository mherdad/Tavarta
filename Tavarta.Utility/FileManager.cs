using System;
using System.IO;

namespace Tavarta.Utility
{
    public static class FileManager
    {
        public static byte[] ConvertToByteArrary(this Stream stream, int length)
        {
            var fileData = new byte[length];
            stream.Read(fileData, 0, Convert.ToInt32(length));
            return fileData;
        }
    }
}
