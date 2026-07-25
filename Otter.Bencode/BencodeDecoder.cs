namespace Otter.Bencode;

public class BencodeDecoder
{
    private readonly byte[] _data;
    private int _position;

    public BencodeDecoder(byte[] data)
    {
        _data = data;
        _position = 0;
    }

    public BencodeNode Decode()
    {
        return DecodeNode();
    }

    private BencodeNode DecodeNode()
    {
        int start = _position;

        BencodeValue value = _data[_position] switch
        {
            (byte)'i' => DecodeInteger(),
            (byte)'l' => DecodeList(),
            (byte)'d' => DecodeDictionary(),
            _ => DecodeString()
        };

        return new BencodeNode(
            value,
            start,
            _position);
    }

    private BencodeInteger DecodeInteger()
    {
        _position++;

        int start = _position;

        while (_data[_position] != (byte)'e')
        {
            _position++;
        }

        var number = long.Parse(
            System.Text.Encoding.ASCII.GetString(
                _data,
                start,
                _position - start));

        _position++;

        return new BencodeInteger(number);
    }

    private BencodeString DecodeString()
    {
        int start = _position;

        while (_data[_position] != (byte)':')
        {
            _position++;
        }

        int length = int.Parse(
            System.Text.Encoding.ASCII.GetString(
                _data,
                start,
                _position - start));

        _position++;

        byte[] value = new byte[length];

        Array.Copy(
            _data,
            _position,
            value,
            0,
            length);

        _position += length;

        return new BencodeString(value);
    }

    private BencodeList DecodeList()
    {
        _position++;

        var values = new List<BencodeValue>();

        while (_data[_position] != (byte)'e')
        {
            values.Add(DecodeNode().Value);
        }

        _position++;

        return new BencodeList(values);
    }

    private BencodeDictionary DecodeDictionary()
    {
        _position++;

        var values = new Dictionary<string, BencodeNode>();

        while (_data[_position] != (byte)'e')
        {
            var key = DecodeString().AsString();

            var value = DecodeNode();

            values[key] = value;
        }

        _position++;

        return new BencodeDictionary(values);
    }
}
