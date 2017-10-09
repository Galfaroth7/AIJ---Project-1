using Assets.Scripts.IAJ.Unity.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement
{
    class DynamicAvoidObstacle : DynamicSeek
    {
        public float AvoidMargin;
        public float MaxLookAhead;
        public GameObject obstacle;
        public int FrameCounter = 0;
        public int MaxFrames = 30;
        KinematicData target = new KinematicData();
        RaycastHit hit;
        private Collider collider;

        public DynamicAvoidObstacle(GameObject obstacle)
        {
            this.DebugColor = Color.magenta;
            this.obstacle = obstacle;
            this.collider = obstacle.GetComponent<Collider>();
        }


        public override MovementOutput GetMovement()
        {
            
            Ray centralRay = new Ray(Character.Position, Character.GetOrientationAsVector());
            Ray leftWhisker = new Ray(Character.Position, MathHelper.ConvertOrientationToVector(Character.Orientation + 30));
            Ray rightWhisker = new Ray(Character.Position, MathHelper.ConvertOrientationToVector(Character.Orientation - 30));
           

                      
            if (collider.Raycast(centralRay, out hit, MaxLookAhead))
            {
                target.Position = hit.point + hit.normal * AvoidMargin;   
            }
            else if (collider.Raycast(leftWhisker, out hit, MaxLookAhead/3))
            {
                target.Position = hit.point + hit.normal * AvoidMargin;
            }
            else if (collider.Raycast(rightWhisker, out hit, MaxLookAhead/3))
            {
                target.Position = hit.point + hit.normal * AvoidMargin;
            }
            else
                target.Position = Character.Position;
            this.Target = target;
            
            return base.GetMovement();
            


        }
    }
}
