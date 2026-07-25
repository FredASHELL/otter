using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Otter.Peer;

public class PeerConnection
{
    private readonly string _ip;
    private readonly int _port;

    public PeerConnection(string ip, int port)
    {
        _ip = ip;
        _port = port;
    }

    public async Task Connect(
        byte[] infoHash,
        byte[] peerId)
    {
        using var client = new TcpClient();

        Console.WriteLine($"Connecting to {_ip}:{_port}...");

        await client.ConnectAsync(
            IPAddress.Parse(_ip),
            _port);

        Console.WriteLine("Connected!");

        using var stream = client.GetStream();

        await SendHandshake(
            stream,
            infoHash,
            peerId);

        Console.WriteLine("Handshake sent.");

        var response = new byte[68];

        var read = await stream.ReadAsync(response);

        if (read == 68)
        {
            Console.WriteLine("Handshake received!");

            var receivedHash =
                response[28..48];

            if (receivedHash.SequenceEqual(infoHash))
            {
                Console.WriteLine(
                    "Peer has the same torrent!");
            }
            else
            {
                Console.WriteLine(
                    "Peer has a different torrent.");
            }
        }
        else
        {
            Console.WriteLine(
                $"Unexpected handshake size: {read}");
        }
    }


    private static async Task SendHandshake(
        NetworkStream stream,
        byte[] infoHash,
        byte[] peerId)
    {
        using var ms = new MemoryStream();

        // Protocol string length
        ms.WriteByte(19);

        // "BitTorrent protocol"
        var protocol =
            Encoding.ASCII.GetBytes(
                "BitTorrent protocol");

        ms.Write(protocol);

        // Reserved bytes
        ms.Write(new byte[8]);

        // Torrent info hash
        ms.Write(infoHash);

        // Our peer ID
        ms.Write(peerId);

        await stream.WriteAsync(
            ms.ToArray());
    }
}
