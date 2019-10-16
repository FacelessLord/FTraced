using System.Collections;
using System.IO;
using System.Net.Json;
using GlLib.Common.Io;
using GlLib.Common.Map;
using GlLib.Common.Registries;

namespace GlLib.Common
{
    internal class Stash
    {
        public static Hashtable blocks = new Hashtable();

        public static void UpdateStash(GameRegistry _registry)
        {
            if (!Directory.Exists(@"stash")) Directory.CreateDirectory("stash");

            if (!File.Exists(@"stash/StashedObjects.json"))
                File.Create(@"stash/StashedObjects.json").Close();

            using (var file = File.CreateText(@"stash/StashedObjects.json"))
            {
                //serialize object directly into file stream
                var blocks = _registry.blocks.Values;
                JsonCollection result = new JsonObjectCollection();
                foreach (var obj in blocks)
                {
                    var block = obj as TerrainBlock;
                    result.Add(block?.Serialize(block.GetInternalName()));
                }

                result.WriteTo(file);
            }
        }


        public static Hashtable GetBlocks()
        {
            UpdateBlocks();
            return blocks;
        }

        public static void UpdateBlocks()
        {
            if (!File.Exists(@"stash/StashedObjects.json"))
                SidedConsole.WriteLine("Can't take stashed blocks.");

            var currentBlocks = new JsonObjectCollection();

            using (var file = File.OpenText(@"stash/StashedObjects.json"))
            {
                currentBlocks = new JsonObjectCollection(file.ReadToEnd());
            }

            blocks = new Hashtable(blocks);
        }

        internal static TerrainBlock GetBlockFromName(string _value)
        {
            if (blocks.ContainsKey(_value))
                return (TerrainBlock) blocks[_value];
            return null;
        }
    }
}