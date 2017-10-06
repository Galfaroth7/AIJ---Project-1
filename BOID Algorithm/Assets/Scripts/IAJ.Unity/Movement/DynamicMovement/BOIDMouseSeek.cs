using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement
{
    class BOIDMouseSeek : DynamicSeek
    {
        public BOIDMouseSeek()
        {
            Target = new KinematicData();
        }
        public override MovementOutput GetMovement()
        {
            if (Input.GetAxis("Fire1") > 0f)
            {

                var mousePos = Input.mousePosition;
                mousePos.z = 55.8f;
                var worldPos = Camera.main.ScreenToWorldPoint(mousePos);
                worldPos.y = 0.0f;
                Target.Position = worldPos;

                return base.GetMovement();
            }
            else
            {
                this.Output.linear = this.Character.GetOrientationAsVector();

                if (this.Output.linear.sqrMagnitude > 0)
                {
                    this.Output.linear.Normalize();
                    this.Output.linear *= this.MaxAcceleration;
                }

                return this.Output;
            }
        }
    }
}