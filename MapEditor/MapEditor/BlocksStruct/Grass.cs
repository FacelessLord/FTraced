using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
