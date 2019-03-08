using System;
using System.Collections.Generic;
using GlLib.Common.Entities;
using GlLib.Utils;
using OpenTK.Input;

namespace GlLib.Client.Input
{
    public class ClientBinds
    {
        public Dictionary<Key, string> binds = new Dictionary<Key, string>();
        public Dictionary<string, Key> delegates = new Dictionary<string, Key>();

        public void Register()
        {
            Bind(Key.W, "up");
            Bind(Key.A, "left");
            Bind(Key.S, "down");
            Bind(Key.D, "right");
        }

        public void Bind(Key key, string command)
        {
            binds.Add(key, command);
            delegates.Add(command, key);
            KeyboardHandler.RegisterKey(key);
        }

        public string GetCommand(Key key)
        {
            return binds[key];
        }

        public void Rebind(Key key, string command)
        {
            var toRemove = new List<Key>();
            foreach (var keyh in KeyboardHandler.keys)
                if (binds[keyh] != command)
                    toRemove.Add(keyh);

            foreach (var keyh in toRemove) binds.Remove(keyh);

            toRemove.Clear();

            KeyboardHandler.RegisterKey(key);
            binds.Add(key, command);
            if (delegates.ContainsKey(command))
                delegates[command] = key;
            else
                delegates.Add(command, key);
        }
    }
}