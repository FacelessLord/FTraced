using System;
using System.Collections.Generic;
using GlLib.Client.Input;
using GlLib.Common.Entities;
using GlLib.Utils;
using OpenTK.Input;

namespace GlLib.Server
{
    public class ServerBinds
    {
        
    public static Dictionary<string, Delegate> binds = new Dictionary<string, Delegate>();
        public static Dictionary<Delegate, string> delegates = new Dictionary<Delegate, string>();

        public static MoveLeftDelegate moveLeft = p =>
            p.acceleration += new PlanarVector(-p.accelValue, -p.accelValue);

        public static MoveUpDelegate moveUp = p => p.acceleration += new PlanarVector(-p.accelValue, p.accelValue);

        public static MoveRightDelegate moveRight = p =>
            p.acceleration += new PlanarVector(p.accelValue, p.accelValue);

        public static MoveDownDelegate moveDown = p =>
            p.acceleration += new PlanarVector(p.accelValue, -p.accelValue);

        public static void Register()
        {
            Bind("up", moveUp);
            Bind("left", moveLeft);
            Bind("down", moveDown);
            Bind("right", moveRight);
        }

        public static void Bind(string key, Delegate @delegate)
        {
            binds.Add(key, @delegate);
            delegates.Add(@delegate, key);
        }
    }

    public delegate void MoveLeftDelegate(Player p);

    public delegate void MoveUpDelegate(Player p);

    public delegate void MoveRightDelegate(Player p);

    public delegate void MoveDownDelegate(Player p);
}