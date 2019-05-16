using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using GlLib.Common.Map;

namespace GlLib.Common.Blocks
{
    public class DarkGrass : TerrainBlock
    {
        public override string Name
        { get
            => "block.outdoor.darkgrass" 
            ;
            protected set
            {
                //TODO
            }
        }

        public override string TextureName
        {
            get => Path + "DarkGrass.png";
            internal set
            {
                //TODO
            }
        }
    }
}
