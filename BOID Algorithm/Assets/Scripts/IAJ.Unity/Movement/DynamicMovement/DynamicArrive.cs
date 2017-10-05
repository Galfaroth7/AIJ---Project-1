using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement
{
    class DynamicArrive : DynamicMovement
    {
        private Vector3 direction;
        private float distance;
        private float targetSpeed;
        private Vector3 targetVelocity;

        public float MaxSpeed { get; set; }
        public float TargetRadius { get; set; }
        public float SlowRadius { get; set; }
        public float TimeToTarget { get; set; }

        public DynamicArrive()
        {
            this.Output = new MovementOutput();
        }

        public override string Name
        {
            get
            {
                return "Arrive";
            }
        }

        public override MovementOutput GetMovement()
        {
            direction = Target.Position - Character.Position;
            distance = direction.magnitude;

            if (distance < TargetRadius)
                targetSpeed = 0;

            if (distance > SlowRadius)
                targetSpeed = MaxSpeed;
            else
                targetSpeed = MaxSpeed * distance / SlowRadius;

            targetVelocity = direction.normalized;
            targetVelocity *= targetSpeed;

            this.Output.linear = targetVelocity - this.Character.velocity;
            this.Output.linear /= TimeToTarget;

            if (this.Output.linear.magnitude > MaxAcceleration)
            {
                this.Output.linear.Normalize();
                this.Output.linear *= MaxAcceleration;
            }

            this.Output.angular = 0;
            return this.Output;

        }
    }
}
