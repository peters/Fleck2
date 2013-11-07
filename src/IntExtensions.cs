using System;

namespace Fleck2
{
    public static class IntExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static byte[] ToBigEndianBytes<T>(int source)
        {
            byte[] bytes;

            var type = typeof(T);
            if (type == typeof(ushort))
                bytes = BitConverter.GetBytes((ushort)source);
            else if (type == typeof(ulong))
                bytes = BitConverter.GetBytes((ulong)source);
            else if (type == typeof(int))
                bytes = BitConverter.GetBytes(source);
            else
                throw new InvalidCastException("Cannot be cast to T");

            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            return bytes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static int ToLittleEndianInt(byte[] source)
        {
            if (BitConverter.IsLittleEndian)
                Array.Reverse(source);

            if (source.Length == 2)
                return BitConverter.ToUInt16(source, 0);

            if (source.Length == 8)
                return (int)BitConverter.ToUInt64(source, 0);

            throw new ArgumentException("Unsupported Size");
        }
    }
}
