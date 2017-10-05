using Assets.Scripts.IAJ.Unity.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement
{
    class BOIDCohesion : DynamicArrive
    {
        public List<DynamicCharacter> Flock { get; set; }
        public float Radius { get; set; }
        public float FanAngle { get; set; }
        public BOIDCohesion()
        {
            Target = new KinematicData();
        }

        public override MovementOutput GetMovement()
        {
            var massCenter = new Vector3();
            var closeBoids = 0;

            foreach (var boid in Flock)
            {
                if (!boid.KinematicData.Equals(Character))
                {
                    var direction = Character.Position - boid.KinematicData.Position;
                    var distance = direction.magnitude;
                    if (distance <= Radius)
                    {
                        var angle = MathHelper.ConvertVectorToOrientation(direction);
                        var angleDifference = MathHelper.ShortestAngleDifference(Character.Orientation, angle);

                        if(Math.Abs(angleDifference) <= FanAngle)
                        {
                            massCenter += boid.KinematicData.Position;
                            closeBoids++;
                        }
                    }
                }
            }
            if (closeBoids == 0) return new MovementOutput();
            massCenter /= closeBoids;
            Target.Position = massCenter;
            return base.GetMovement();
        }  
    }
}
