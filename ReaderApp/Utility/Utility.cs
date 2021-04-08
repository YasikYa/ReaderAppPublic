using System.IO;
using System.Reflection;

namespace ReaderApp.Utility
{
    public static class Util
    {
        public static string GetExecutingAssemblyDirectory() => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    }
}
