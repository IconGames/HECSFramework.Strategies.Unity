using Components;
using HECSFramework.Core;
using UnityEngine;

namespace Strategies
{
    [Documentation(Doc.Strategy, "RotateToTargetNode")]
    public sealed class RotateToTargetNode : InterDecision
    {
        [Connection(ConnectionPointType.In, "<Entity> Target")]
        public GenericNode<Entity> Target;
        [Connection(ConnectionPointType.In, "<float> Rotation Speed")]
        public GenericNode<float> RotationSpeed;
        [Connection(ConnectionPointType.In, "<Transform> Transform to rotation")]
        public GenericNode<Transform> TransformSource;


        [ExposeField]
        public bool FixYAxis = true;

        public override string TitleOfNode { get; } = "RotateToTargetNode";

        protected override void Run(Entity entity)
        {
            var targetTransform = Target.Value(entity).GetComponent<UnityTransformComponent>().Transform;
            var entityTransform = TransformSource.Value(entity);
            var posTarget = targetTransform.position;
            var pos = entityTransform.position;

            if (FixYAxis)
            {
                posTarget.y = pos.y;
            }

            var dir = posTarget - pos;
            var quaternion = Quaternion.LookRotation(dir, Vector3.up);
            entityTransform.rotation = Quaternion.RotateTowards(entityTransform.rotation, quaternion, RotationSpeed.Value(entity)*Time.deltaTime);
            Next.Execute(entity);
        }
    }
}
