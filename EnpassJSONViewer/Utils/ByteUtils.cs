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

        /// <summary>
        /// Formats the specified size in bytes in human readable form.
        /// </summary>
        /// <param name="size">The size in bytes.</param>
        /// <returns>The resulting <see cref="string"/>.</returns>
        public static string FormatSize(ulong size)
        {
            const ulong KiloBytes = 1024L;
            const ulong MegaBytes = KiloBytes * 1024L;
            const ulong GigaBytes = MegaBytes * 1024L;
            const ulong TerraBytes = GigaBytes * 1024L;
            const ulong PetaBytes = TerraBytes * 1024L;
            string result;
            if (size < KiloBytes)
                result = FormattableString.Invariant($"{size} bytes");
            else if (size < MegaBytes)
            {
                double value = size / (double)KiloBytes;
                result = FormattableString.Invariant($"{value:F2} KB");
            }
            else if (size < GigaBytes)
            {
                double value = size / (double)MegaBytes;
                result = FormattableString.Invariant($"{value:F2} MB");
            }
            else if (size < TerraBytes)
            {
                double value = size / (double)GigaBytes;
                result = FormattableString.Invariant($"{value:F2} GB");
            }
            else if (size < PetaBytes)
            {
                double value = size / (double)TerraBytes;
                result = FormattableString.Invariant($"{value:F2} TB");
            }
            else
            {
                double value = size / (double)PetaBytes;
                result = FormattableString.Invariant($"{value:F2} PB");
            }
            return (result);
        }
    }
}
