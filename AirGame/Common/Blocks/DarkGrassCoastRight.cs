using System;
using System.Collections.Generic;
using System.Text;
using GlLib.Common.Map;

namespace GlLib.Common.Blocks
{
    class DarkGrassCoastRight : TerrainBlock
    {
        public override string Name
        {
            get
                => "block.outdoor.darkgrasscoastright"
            ;
            protected set
            {
                //TODO
            }
        }

        public override string TextureName
        {
            get => Path + "DarkGrassCoastRight.png";
            internal set
            {
                //TODO
            }
        }

    }
}
