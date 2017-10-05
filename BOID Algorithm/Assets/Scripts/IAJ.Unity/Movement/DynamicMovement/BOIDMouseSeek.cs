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
            //if (Input.GetMouseButtonDown(0))
            //{
            //    while (Input.GetMouseButton(0))
            //    {
            //Target.Position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //Target.Position = new Vector3(10.0f, 0 , 10.0f);
                    //return base.GetMovement();
            //    }
               
            //}
                return new MovementOutput();
        }
           
    }
}
