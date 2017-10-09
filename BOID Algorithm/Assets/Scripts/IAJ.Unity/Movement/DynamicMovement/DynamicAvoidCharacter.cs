using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement
{
    class DynamicAvoidCharacter : DynamicMovement
    {
        private float shortestTime;
        private float closestFutureDistance;
        private Vector3 closestDeltaPosition;
        private Vector3 avoidanceDirection;
        private KinematicData closestTarget;
        private Vector3 closestFutureDeltaPosition;
        private Vector3 closestDeltaPostion;
        private Vector3 closestDeltaVelocity;

        public DynamicAvoidCharacter(KinematicData kinematicData, ref float shortestTime)
        {
            this.Target = kinematicData;
            this.shortestTime = shortestTime;
        }

        public override string Name
        {
            get
            {
                return "Avoid Character";
            }
        }

        public float MaxLookAhead { get; set; }
        public float AvoidMargin { get; set; }

        public override MovementOutput GetMovement()
        {
            var output = new MovementOutput();
            
            Vector3 deltaPosition = Target.Position - Character.Position;
            Vector3 deltaVelocity = Target.velocity - Character.velocity;
            float deltaSpeed = deltaVelocity.magnitude;
            if (deltaSpeed == 0)
            {
                return output;
            }
            float dott = Vector3.Dot(deltaPosition, deltaVelocity);
            float positiveDot = dott * -1.0f;
            float timeToClosest = Math.Abs(dott) / (deltaSpeed * deltaSpeed);
            if (timeToClosest > MaxLookAhead)
            {
                return output;
            }
            Vector3 futureDeltaPosition = deltaPosition + deltaVelocity * timeToClosest;
            float futureDistance = futureDeltaPosition.magnitude;
            if (futureDistance > 2*AvoidMargin)
            {
                return output;
            }

            if (timeToClosest  > 0 && timeToClosest < shortestTime)
            {
                shortestTime = timeToClosest;
                closestTarget = Target;
                closestFutureDistance = futureDistance;
                closestFutureDeltaPosition = futureDeltaPosition;
                closestDeltaPostion = deltaPosition;
                closestDeltaVelocity = deltaVelocity;
            }

            if (shortestTime == float.MaxValue) return new MovementOutput();
            if (closestFutureDistance <= 0 || closestDeltaPosition.magnitude < 2 * AvoidMargin)
                avoidanceDirection = Character.Position - closestTarget.Position;
            else
                avoidanceDirection = closestFutureDeltaPosition * -1;
            output = new MovementOutput();
            output.linear = avoidanceDirection.normalized * MaxAcceleration;
            return output;
                

        }
    }
}
