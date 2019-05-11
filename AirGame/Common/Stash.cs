using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using GlLib.Common.Map;

namespace GlLib.Common
{
    internal class Stash
    {
        public static Hashtable blocks = new Hashtable();
        public static void UpdateStash()
        {
            if (!Directory.Exists(@"stash")) Directory.CreateDirectory("stash");

            if (!File.Exists(@"stash/StashedObjects.json"))
                File.Create(@"stash/StashedObjects.json").Close();

            using (StreamWriter file = File.CreateText(@"stash/StashedObjects.json"))
            {
                //serialize object directly into file stream
                var blocks = Proxy.GetRegistry().blocks.Values;
                foreach (var obj in blocks)
                {
                    var block = obj as TerrainBlock;
                    file.WriteLine(block?.CreateJsonObject());
                }
            }
        }

        public static Hashtable GetBlocks()
        {
            //TODO
            return new Hashtable();
        }

        internal static TerrainBlock GetBlockFromName(string _value)
        {
            if (blocks.ContainsKey(_value))
                return (TerrainBlock)blocks[_value];
            return null;
        }
    }
}
