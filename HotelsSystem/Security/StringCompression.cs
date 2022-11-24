namespace HotelsSystem.Security
{
    public static class StringCompression
    {
        public static byte[] Compress(string value)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Compress))
                {
                    gZipStream.Write(Encoding.UTF8.GetBytes(value));
                }
                return memoryStream.ToArray();
            }
        }

        public static string Decompress(byte[] bytes)
        {
            using (var memoryStream = new MemoryStream(bytes))
            using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
            using (var memoryStreamOutput = new MemoryStream())
            {
                gZipStream.CopyTo(memoryStreamOutput);
                var outputBytes = memoryStreamOutput.ToArray();

                string decompressed = Encoding.UTF8.GetString(outputBytes);
                return decompressed;
            }
        }
    }
}