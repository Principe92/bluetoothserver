using System.Text;

namespace WatchForm
{
    internal static class Extensions
    {
        public static byte[] ToBytes(this int value)
        {
            return Encoding.ASCII.GetBytes($"{value}\n");
        }

        public static byte[] ToBytes(this Phase value)
        {
            return Encoding.ASCII.GetBytes($"{(int) value}\n");
        }
    }
}