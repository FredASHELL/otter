namespace Otter.Bencode;

public abstract class BencodeValue
{
}

public sealed class BencodeInteger : BencodeValue
{
    public long Value { get; }

    public BencodeInteger(long value)
    {
        Value = value;
    }
}

public sealed class BencodeString : BencodeValue
{
    public byte[] Value { get; }

    public BencodeString(byte[] value)
    {
        Value = value;
    }

    public string AsString()
    {
        return System.Text.Encoding.UTF8.GetString(Value);
    }
}

public sealed class BencodeList : BencodeValue
{
    public List<BencodeValue> Values { get; }

    public BencodeList(List<BencodeValue> values)
    {
        Values = values;
    }
}

public sealed class BencodeDictionary : BencodeValue
{
    public Dictionary<string, BencodeNode> Values { get; }

    public BencodeDictionary(Dictionary<string, BencodeNode> values)
    {
        Values = values;
    }
}