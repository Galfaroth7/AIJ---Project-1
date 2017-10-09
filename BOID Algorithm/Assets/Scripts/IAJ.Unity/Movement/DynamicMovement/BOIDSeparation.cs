using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement
{
    class BOIDSeparation : DynamicMovement
    {
        
        public List<DynamicCharacter> Flock { get; set; }
        public float Radius { get;  set; }
        public int SeparationFactor { get; set; }

        public BOIDSeparation()
        {
            Target = new KinematicData();
        }

        public override string Name
        {
            get
            {
                return "Separation";
            }
        }

        

        public override MovementOutput GetMovement()
        {
            var output = new MovementOutput();
            foreach (var boid in Flock)
            {
                if (!boid.KinematicData.Equals(Character))
                {
                    Vector3 direction = Character.Position - boid.KinematicData.Position;
                    float distance = direction.magnitude;
                    if (distance < Radius)
                    {
                        float separationStrength = Math.Min(SeparationFactor / (distance * distance), MaxAcceleration);
                        direction.Normalize();
                        output.linear += direction * separationStrength;
                    }
                }
            }
            if (output.linear.magnitude > MaxAcceleration)
            {
                output.linear.Normalize();
                output.linear *= MaxAcceleration;
            }
            return output;
        }
    }
}
