namespace MapEditor.BlocksStruct
{
    public interface IBlock
    {
        string SerializationId();
        string GetName();
        string GetTexturePath();
    }
}