using HECSFramework.Core;
using Helpers;
using UnityEngine;

namespace Strategies
{
    [Documentation(Doc.Strategy, Doc.UniversalNodes, Doc.HECS, "this node gather spherecast result to hecs list")]
    public class GetEntitiesBySphereCast : GenericNode<HECSList<Entity>>
    {
        [Connection(ConnectionPointType.In, "<int> Target count")]
        public GenericNode<int> TargetsCount;

        [Connection(ConnectionPointType.In, "<Vector3> Point of cast")]
        public GenericNode<Vector3> PointOfCast;

        [Connection(ConnectionPointType.In, "<float> Radius of cast")]
        public GenericNode<float> RadiusOfCast;

        public override string TitleOfNode { get; } = "GetEntitiesBySphereCast";

        [Connection(ConnectionPointType.Out, "HECSList<Entity> Out")]
        public BaseDecisionNode Out;


        public override void Execute(Entity entity)
        {
        }

        public override HECSList<Entity> Value(Entity entity)
        {
            var context = entity.GetOrAddComponent<SphereCastContext>();
            context.Entities.ClearFast();
            context.CheckCount(TargetsCount.Value(entity));
            
            var count = Physics.SphereCastNonAlloc(PointOfCast.Value(entity), RadiusOfCast.Value(entity), new Vector3(0, 0, 0.03f), context.RaycastHits);

            for (int i = 0; i < count; i++)
            {
                if (context.RaycastHits[i].collider.TryGetActorFromCollision(out var actor))
                {
                    if (actor.Entity.IsAlive())
                    {
                        context.Entities.Add(actor.Entity);
                    }
                }
            }

            return context.Entities;
        }
    }

    public sealed class SphereCastContext : BaseComponent
    {
        public HECSList<Entity> Entities = new HECSList<Entity>(16);
        public RaycastHit[] RaycastHits = new RaycastHit[16];

        public void CheckCount(int count) 
        { 
            if (RaycastHits.Length < count)
            {
                Entities = new HECSList<Entity>(count);
                RaycastHits = new RaycastHit[count];
            }
        }
    }
}