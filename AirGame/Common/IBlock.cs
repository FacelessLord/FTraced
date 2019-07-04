namespace GlLib.Common
{
    public interface IBlock
    {
        string SerializationId();
        string GetName();
        string GetTexturePath();
    }
}