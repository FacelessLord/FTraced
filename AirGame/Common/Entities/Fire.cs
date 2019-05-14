using System;
using System.Collections.Generic;
using System.Text;
using GlLib.Client.Graphic.Renderers;
using GlLib.Common.Map;
using GlLib.Utils;

namespace GlLib.Common.Entities
{
    public class Fire : Entity
    {

        private const short BaseVelocity = 8;
        public Fire(World _world, RestrictedVector3D _position, PlanarVector _velocity)
            : base(_world, _position)
        {
            velocity = _velocity * BaseVelocity;
            SetCustomRenderer(new FireRenderer());
        }
    }
}
