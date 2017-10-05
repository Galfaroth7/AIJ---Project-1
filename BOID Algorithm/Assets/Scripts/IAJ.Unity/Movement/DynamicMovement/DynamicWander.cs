using Assets.Scripts.IAJ.Unity.Util;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement
{
    public class DynamicWander : DynamicSeek
    {
        public float WanderOffset { get; set; }
        public float WanderRadius { get; set; }
        public float WanderRate { get; set; }
        public float TurnAngle { get; set; }

        public Vector3 CircleCenter { get; private set; }

        public GameObject DebugTarget { get; set; }

        protected float WanderOrientation { get; set; }


        public DynamicWander()
        {
            this.Target = new KinematicData();
            this.WanderOrientation = 0;
            this.WanderRadius = 10.0f;
            this.DebugColor = Color.yellow;
        }

        public override string Name
        {
            get { return "Wander"; }
        }


        public override MovementOutput GetMovement()
        {
            WanderOrientation += RandomHelper.RandomBinomial() + WanderRate;
            Target.Orientation = WanderOrientation + this.Character.Orientation;
            Target.Position = this.Character.Position + WanderOffset * Character.GetOrientationAsVector();
            Target.Position += WanderRadius * Target.GetOrientationAsVector();
            CircleCenter = this.Character.Position + WanderOffset * Character.GetOrientationAsVector();
            if (this.DebugTarget != null)
            {
                this.DebugTarget.transform.position = this.Target.Position;

            }
            return base.GetMovement();
        }
    }
}
