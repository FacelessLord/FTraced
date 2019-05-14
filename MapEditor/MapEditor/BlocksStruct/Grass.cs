namespace MapEditor.BlocksStruct
{
    internal struct Grass : IBlock
    {
        public string SerializationId()
        {
            throw new NotImplementedException();
        }

        public string GetName()
        {
            return "grass";
        }

        public string GetTexturePath()
        {
            return @"textures\blocks\grass.png";
        }
    }
}