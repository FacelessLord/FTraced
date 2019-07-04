using System;
using GlLib.Client.Input;
using GlLib.Common.Entities;
using OpenTK;
using OpenTK.Input;

namespace GlLib.Client.Api.Gui
{
    public class GuiBindButton : GuiButton
    {
        public Func<Player, bool> action;
        public Key key;

        public GuiBindButton(Key _key, Func<Player, bool> _action, int _x, int _y, int _width, int _height) :
            base(_key.ToString(), _x, _y, _width, _height)
        {
            key = _key;
            action = _action;
        }

        public GuiBindButton(Key _key, Func<Player, bool> _action, int _x, int _y, int _width, int _height,
            Color _color) :
            base(_key.ToString(), _x, _y, _width, _height, _color)
        {
            key = _key;
            action = _action;
        }

        public override void OnKeyDown(GuiFrame _gui, KeyboardKeyEventArgs _e)
        {
            base.OnKeyDown(_gui, _e);
            if (KeyBinds.binds.ContainsKey(key))
                KeyBinds.Rebind(_e.Key, action);
            else
                KeyBinds.RebindClick(_e.Key, action);
            key = _e.Key;
            text = key.ToString();
            releaseAction(_gui, this);
            _gui.focusedObject = null;
            state = ButtonState.Enabled;
        }

        public override bool UnfocusOnRelease()
        {
            return false;
        }
    }
}