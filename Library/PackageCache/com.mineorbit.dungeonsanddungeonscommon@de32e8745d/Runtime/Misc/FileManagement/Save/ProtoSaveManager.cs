using System.IO;
using Google.Protobuf;

namespace com.mineorbit.dungeonsanddungeonscommon
{
    public class ProtoSaveManager
    {
        public static T Load<T>(string path) where T : IMessage, new()
        {
            T t = new T();
            using (var input = File.OpenRead(path))
            {
                t.MergeFrom(input);
                input.Close();
            }

            return t;
        }

        public static void Save(string path, IMessage m)
        {
            using (var output = File.Create(path))
            {
                m.WriteTo(output);
                output.Close();
            }
        }
    }
}