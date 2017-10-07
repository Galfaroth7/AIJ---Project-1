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
    //private PriorityMovement priorityMovement;
    public BlendedMovement blendedMovement;

    private const float AVOID_MARGIN = 88.0f;
    private const float MAX_LOOK_AHEAD = 10.0f;


    //early initialization
    void Awake()
    {
        this.character = new DynamicCharacter(gameObject);

        
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
        foreach (var obstacle in obstacles)
        {
            //AvoidObstacle movement
            var avoidObstacleMovement = new DynamicAvoidObstacle(obstacle)
            {
                MaxAcceleration = MAX_ACCELERATION,
                MaxLookAhead = MAX_LOOK_AHEAD,
                AvoidMargin = AVOID_MARGIN,
                Character = this.character.KinematicData,
                DebugColor = Color.magenta
            };
            this.blendedMovement.Movements.Add(new MovementWithWeight(avoidObstacleMovement, 160.0f));
            
        }

        float shortestTime = float.MaxValue;
        foreach (var otherCharacter in characters)
        {
            if (otherCharacter != character)
            {
                //avoidCharacter movement
                var avoidCharacter = new DynamicAvoidCharacter(otherCharacter.KinematicData, ref shortestTime)
                {
                    Character = this.character.KinematicData,
                    MaxAcceleration = MAX_ACCELERATION,
                    AvoidMargin = 2.0f,
                    DebugColor = Color.cyan,
                    MaxLookAhead = 1.0f
                };

                this.blendedMovement.Movements.Add(new MovementWithWeight(avoidCharacter, 28.0f));
            }
        }

        var separation = new BOIDSeparation()
        {
            Character = this.character.KinematicData,
            Flock = characters,
            MaxAcceleration = MAX_ACCELERATION,
            Radius = 15.0f,
            SeparationFactor = 50,
            DebugColor = Color.cyan
        };

        var cohesion = new BOIDCohesion()
        {
            Character = this.character.KinematicData,
            Flock = characters,
            Radius = 25.0f,
            FanAngle = 120,
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
            FanAngle = 120,
            DebugColor = Color.magenta
        };

        var mouseSeek = new BOIDMouseSeek()
        {
            MaxAcceleration = MAX_ACCELERATION
        };

       
        var straightAhead = new DynamicStraightAhead
        {
            Character = this.character.KinematicData,
            MaxAcceleration = MAX_ACCELERATION,
            DebugColor = Color.yellow
        };
        
        this.blendedMovement.Movements.Add(new MovementWithWeight(separation, 11.0f));
        this.blendedMovement.Movements.Add(new MovementWithWeight(cohesion, 12.0f));
        this.blendedMovement.Movements.Add(new MovementWithWeight(flockVelocityMatch, 4.0f));
        this.blendedMovement.Movements.Add(new MovementWithWeight(mouseSeek, 10.5f));
        this.blendedMovement.Movements.Add(new MovementWithWeight(straightAhead, 2.5f));
        this.character.Movement = this.blendedMovement;
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
