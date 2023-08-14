using System;
using HECSFramework.Core;
using UnityEngine;

namespace Components
{
    [Serializable]
    [Documentation(Doc.HECS, Doc.Strategy, "Context for overlap sphere cast node")]
    public sealed class OverlapSphereCastContext : BaseComponent
    {
        public HECSList<Entity> Entities = new HECSList<Entity>(16);
        public Collider[] Colliders = new Collider[16];

        public void CheckCount(int count)
        {
            if (Colliders.Length < count)
            {
                Entities = new HECSList<Entity>(count);
                Colliders = new Collider[count];
            }
        }
    }
}