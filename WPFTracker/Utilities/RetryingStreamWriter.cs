using System.IO;

namespace WPFTracker.Utilities
{
    static class RetryingStreamWriter
    {
        private const int MaxRetries = 3;

        public static bool Write(string file, params object[] content)
        {
            int count = 0;

            while (count < MaxRetries)
            {
                try
                {
                    using StreamWriter writer = new StreamWriter(file, true);
                    var contentString = string.Join(", ", content);
                    writer.WriteLine(contentString);
                    return true;
                }
                catch (IOException ex)
                {
                    count++;
                }
            }

            return false;
        }
    }
}
