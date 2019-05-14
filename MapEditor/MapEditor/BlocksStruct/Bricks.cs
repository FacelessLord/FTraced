namespace MapEditor.BlocksStruct
{
    public struct Bricks : IBlock
    {
        public string SerializationId()
        {
            throw new NotImplementedException();
        }

        public string GetName()
        {
            return "bricks";
        }

        public string GetTexturePath()
        {
            return @"textures\blocks\bricks_worn.png";
        }
    }
}