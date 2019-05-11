using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEditor.BlocksStruct
{
    internal struct BlocksСonstructor : IBlock
    {
        private string TextureName { get; }
        private string BlockName { get;  }

        public BlocksСonstructor(string _textureName, string _blockName)
        {
            TextureName = _textureName;
            BlockName = _blockName;
        }

        public string SerializationId()
        {
            throw new NotImplementedException();
        }

        public string GetName()
        {
            return BlockName;
        }

        public string GetTexturePath()
        {
            return @"textures\blocks\" + TextureName;
        }
    }
}
