namespace Otter.Bencode;

public class BencodeNode
{
    public BencodeValue Value { get; }
    public int Start { get; }
    public int End { get; }

    public BencodeNode(
        BencodeValue value,
        int start,
        int end)
    {
        Value = value;
        Start = start;
        End = end;
    }
}