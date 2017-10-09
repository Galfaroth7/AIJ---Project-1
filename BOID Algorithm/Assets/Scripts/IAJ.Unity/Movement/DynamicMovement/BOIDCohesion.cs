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
            Vector3 massCenter = new Vector3();
            int closeBoids = 0;

            foreach (var boid in Flock)
            {
                if (!boid.KinematicData.Equals(Character))
                {
                    Vector3 direction = Character.Position - boid.KinematicData.Position;
                    float distance = direction.magnitude;
                    if (distance <= Radius)
                    {
                        float angle = MathHelper.ConvertVectorToOrientation(direction);
                        float angleDifference = MathHelper.ShortestAngleDifference(Character.Orientation, angle);

                        if(Math.Abs(angleDifference) <= FanAngle)
                        {
                            massCenter += boid.KinematicData.Position;
                            closeBoids++;
                        }
                    }
                }
            }
            if (closeBoids == 0)
                return new MovementOutput();
            massCenter /= closeBoids;
            Target.Position = massCenter;
            return base.GetMovement();
        }  
    }
}
