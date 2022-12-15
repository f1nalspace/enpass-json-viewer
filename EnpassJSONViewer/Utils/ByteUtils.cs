using System;
using System.Collections.Immutable;
using System.DirectoryServices.ActiveDirectory;

namespace EnpassJSONViewer.Utils
{
    static class ByteUtils
    {
        public static ImmutableArray<byte> GetFromBase64(ReadOnlySpan<char> value)
        {
            if (value.Length > 0)
            {
                const int bitsEncodedPerChar = 6;
                int bytesExpected = (value.Length * bitsEncodedPerChar) >> 3;
                byte[] data = new byte[bytesExpected];
                if (Convert.TryFromBase64Chars(value, new Span<byte>(data), out int bytesWritten))
                    return data.ToImmutableArray();
            }
            return ImmutableArray<byte>.Empty;
        }
    }
}
