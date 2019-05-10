using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;

namespace TOFMapEditor.Client
{
    public interface ICameraMapEditorCamera
    {
        void Update(GameWindow _window);

        void PerformTranslation(GameWindow _window);
    }
}
