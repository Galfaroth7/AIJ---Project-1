using Assets.Scripts.IAJ.Unity.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement
{
    class BOIDVelocityMatch : DynamicVelocityMatch
    {
        public List<DynamicCharacter> Flock { get; set; }
        public float Radius { get; set; }
        public float FanAngle { get; set; }
        public BOIDVelocityMatch()
        {
            Target = new KinematicData();
        }

        public override MovementOutput GetMovement()
        {
            var averageVelocity = new Vector3();
            var closeBoids = 0;
            
            foreach (var boid in Flock)
            {
                if (!boid.KinematicData.Equals(Character))
                {
                    var direction = boid.KinematicData.Position - Character.Position;
                    if (direction.magnitude <= Radius)
                    {
                        var angle = MathHelper.ConvertVectorToOrientation(direction);
                        var angleDifference = MathHelper.ShortestAngleDifference(Character.Orientation, angle);

                        if (Math.Abs(angleDifference) <= FanAngle)
                        {
                            averageVelocity += boid.KinematicData.velocity;
                            closeBoids++;
                        }
                    }
                }
            }
            if (closeBoids == 0) return new MovementOutput();

            averageVelocity /= closeBoids;
            Target.velocity = averageVelocity;

            return base.GetMovement();
        }
    }
}
