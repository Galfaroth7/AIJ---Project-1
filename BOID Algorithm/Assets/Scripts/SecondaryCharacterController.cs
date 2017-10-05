using Assets.Scripts.IAJ.Unity.Movement;
using Assets.Scripts.IAJ.Unity.Movement.Arbitration;
using Assets.Scripts.IAJ.Unity.Movement.DynamicMovement;
using System.Collections.Generic;
using UnityEngine;

public class SecondaryCharacterController : MonoBehaviour {

    public const float X_WORLD_SIZE = 55;
    public const float Z_WORLD_SIZE = 32.5f;
    private const float MAX_ACCELERATION = 40.0f;
    private const float MAX_SPEED = 20.0f;
    private const float DRAG = 0.1f;

    public DynamicCharacter character;
    private PriorityMovement priorityMovement;
    public BlendedMovement blendedMovement;

    private const float AVOID_MARGIN = 100.0f;
    private const float MAX_LOOK_AHEAD = 15.0f;


    //early initialization
    void Awake()
    {
        this.character = new DynamicCharacter(gameObject);

        this.priorityMovement = new PriorityMovement
        {
            Character = this.character.KinematicData
        };
        this.blendedMovement = new BlendedMovement
        {
            Character = this.character.KinematicData
        };

    }

    // Use this for initialization
    void Start()
    {
    }

    public void InitializeMovement(GameObject[] obstacles, List<DynamicCharacter> characters)
    {
        //foreach (var obstacle in obstacles)
        //{
        //    //TODO: add your AvoidObstacle movement here
        //    var avoidObstacleMovement = new DynamicAvoidObstacle(obstacle)
        //    {
        //        MaxAcceleration = MAX_ACCELERATION,
        //        MaxLookAhead = MAX_LOOK_AHEAD,
        //        AvoidMargin = AVOID_MARGIN,
        //        Character = this.character.KinematicData,
        //        DebugColor = Color.magenta
        //    };

        //    this.priorityMovement.Movements.Add(avoidObstacleMovement);
        //}

        var separation = new BOIDSeparation()
        {
            Character = this.character.KinematicData,
            Flock = characters,
            MaxAcceleration = MAX_ACCELERATION,
            Radius = 5.0f,
            SeparationFactor = 50,
            DebugColor = Color.cyan
        };

        var cohesion = new BOIDCohesion()
        {
            Character = this.character.KinematicData,
            Flock = characters,
            Radius = 20.0f,
            FanAngle = 60,
            MaxAcceleration = MAX_ACCELERATION,
            MaxSpeed = MAX_SPEED,
            TargetRadius = 8.0f,
            SlowRadius = 20.0f,
            TimeToTarget = 0.1f,
            DebugColor = Color.yellow
        };

        var flockVelocityMatch = new BOIDVelocityMatch()
        {
            Character = this.character.KinematicData,
            Flock = characters,
            Radius = 10.0f,
            FanAngle = 15,
            DebugColor = Color.magenta
        };

        var mouseSeek = new BOIDMouseSeek()
        {
            MaxAcceleration = MAX_ACCELERATION
        };

        var kinData = new KinematicData();
        kinData.Position = new Vector3(10.0f, 0, 10.0f);
        var seek = new DynamicSeek
        {
            Character = this.character.KinematicData,
            Target = kinData,
            MaxAcceleration = MAX_ACCELERATION
        };
        this.blendedMovement.Movements.Add(new MovementWithWeight(separation, 40.0f));
        this.blendedMovement.Movements.Add(new MovementWithWeight(cohesion, 6.0f));
        this.blendedMovement.Movements.Add(new MovementWithWeight(flockVelocityMatch, 20.0f));
        this.blendedMovement.Movements.Add(new MovementWithWeight(mouseSeek, 8.0f));
        //this.blendedMovement.Movements.Add(new MovementWithWeight(seek, 10.0f));
        this.character.Movement = this.blendedMovement;

        /**
         * TODO: It should be blended
         * */

        //this.priorityMovement.Movements.Add(separation);

        //float shortestTime = float.MaxValue;
        //foreach (var otherCharacter in characters)
        //{
        //    if (otherCharacter != character)
        //    {
        //        //TODO: add your avoidCharacter movement here
        //        var avoidCharacter = new DynamicAvoidCharacter(otherCharacter.KinematicData, ref shortestTime)
        //        {
        //            Character = this.character.KinematicData,
        //            MaxAcceleration = MAX_ACCELERATION,
        //            AvoidMargin = 2.0f,
        //            DebugColor = Color.cyan,
        //            MaxLookAhead = 1.0f
        //        };

        //        this.priorityMovement.Movements.Add(avoidCharacter);
        //    }
        //}

        //var straightAhead = new DynamicStraightAhead
        //{
        //    Character = this.character.KinematicData,
        //    MaxAcceleration = MAX_ACCELERATION,
        //    DebugColor = Color.yellow
        //};

        //this.priorityMovement.Movements.Add(straightAhead);

    }


    // Update is called once per frame
    void Update () {
        UpdateMovingGameObject();
	}

    private void UpdateMovingGameObject()
    {
        if (character.Movement != null)
        {
            character.Update();
            character.KinematicData.ApplyWorldLimit(X_WORLD_SIZE, Z_WORLD_SIZE);
        }
    }
}
