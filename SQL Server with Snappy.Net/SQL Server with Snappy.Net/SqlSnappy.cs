using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlTypes;
using Snappy;

public class SQLSnappy
{
    public static SqlBinary Compress(SqlString srcText)
    {
        MemoryStream ms = null;

        try
        {
            ms = new MemoryStream();

            using (var ss = new SnappyStream(ms, CompressionMode.Compress))
            {
                ms = null;

                var srcBytes = Encoding.UTF8.GetBytes(srcText.Value);
                ss.Write(srcBytes, 0, srcBytes.Length);
                ss.Flush();
                return ms.ToArray();
            }
        }
        catch
        {
            return SqlBinary.Null;
        }
        finally
        {
            ms?.Dispose();
        }
    }

    public static SqlString Decompress(SqlBinary srcBinary)
    {
        MemoryStream ms = null;

        try
        {
            ms = new MemoryStream();

            ms.Write(srcBinary.Value, 0, srcBinary.Length);
            ms.Flush();
            ms.Seek(0, SeekOrigin.Begin);

            using (var ss = new SnappyStream(ms, CompressionMode.Decompress))
            {
                ms = null;

                var decompress = new List<byte>(srcBinary.Length * 2);
                var buffer = new byte[1024];

                while (true)
                {
                    var nRead = ss.Read(buffer, 0, buffer.Length);
                    if (nRead > 0)
                    {
                        decompress.AddRange(new ArraySegment<byte>(buffer, 0, nRead));
                    }
                    else
                    {
                        break;
                    }
                }

                return Encoding.UTF8.GetString(decompress.ToArray());
            }
        }
        catch (Exception e)
        {
            return e.Message;
        }
        finally
        {
            ms?.Dispose();
        }
    }
}
