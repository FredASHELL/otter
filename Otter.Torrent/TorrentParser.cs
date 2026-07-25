using Otter.Bencode;
using System.Security.Cryptography;

namespace Otter.Torrent;

public static class TorrentParser
{
    public static TorrentFile Parse(
        BencodeDictionary root,
        byte[] originalBytes)
    {
        var announce =
            ((BencodeString)root.Values["announce"].Value)
            .AsString();

        var infoNode = root.Values["info"];

        var info =
            (BencodeDictionary)infoNode.Value;

        var name =
            ((BencodeString)info.Values["name"].Value)
            .AsString();

        var length =
            ((BencodeInteger)info.Values["length"].Value)
            .Value;

        var pieceLength =
            ((BencodeInteger)info.Values["piece length"].Value)
            .Value;

        var pieces =
            ((BencodeString)info.Values["pieces"].Value)
            .Value;

        var infoHashBytes =
            CalculateInfoHash(infoNode, originalBytes);

        return new TorrentFile(
            announce,
            name,
            length,
            pieceLength,
            pieces,
            infoHashBytes,
            originalBytes);
    }

    private static byte[] CalculateInfoHash(
        BencodeNode infoNode,
        byte[] originalBytes)
    {
        var infoBytes =
            originalBytes[infoNode.Start..infoNode.End];

        return SHA1.HashData(infoBytes);
    }
}
