using Assets.Scripts.IAJ.Unity.Util;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.IAJ.Unity.Movement.DynamicMovement;
using Assets.Scripts.IAJ.Unity.Movement.Arbitration;
using System.Collections.Generic;

public class MainCharacterController : MonoBehaviour {

    public const float X_WORLD_SIZE = 55;
    public const float Z_WORLD_SIZE = 32.5f;
    private const float MAX_ACCELERATION = 40.0f;
    private const float MAX_SPEED = 20.0f;
    private const float DRAG = 0.1f;
    private const float AVOID_MARGIN = 20.0f;
    private const float MAX_LOOK_AHEAD = 15.0f;



    public KeyCode stopKey = KeyCode.S;
    public KeyCode priorityKey = KeyCode.P;
    public KeyCode blendedKey = KeyCode.B;

    public GameObject movementText;
    public DynamicCharacter character;

    public PriorityMovement priorityMovement;
    public BlendedMovement blendedMovement;
    public DynamicWander wander;

    public GameObject debugTarget;

    private Text movementTextText;
    

    //early initialization
    void Awake()
    {
        this.character = new DynamicCharacter(this.gameObject);
        this.movementTextText = this.movementText.GetComponent<Text>();

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
    void Start ()
    {
    }

    public void InitializeMovement(GameObject[] obstacles, List<DynamicCharacter> characters)
    {
        foreach (var obstacle in obstacles)
        {
        //TODO: add your AvoidObstacle movement here
        var avoidObstacleMovement = new DynamicAvoidObstacle(obstacle)
        {
            MaxAcceleration = MAX_ACCELERATION,
            AvoidMargin = AVOID_MARGIN,
            MaxLookAhead = MAX_LOOK_AHEAD,
            Character = this.character.KinematicData,
            
        };

            this.blendedMovement.Movements.Add(new MovementWithWeight(avoidObstacleMovement, 10.0f));
            this.priorityMovement.Movements.Add(avoidObstacleMovement);
        }

        float shortestTime = float.MaxValue;
        foreach (var otherCharacter in characters)
        {
            if (otherCharacter != this.character)
            {
                //TODO: add your AvoidCharacter movement here
                var avoidCharacter = new DynamicAvoidCharacter(otherCharacter.KinematicData, ref shortestTime)
                {
                    Character = this.character.KinematicData,
                    MaxAcceleration = MAX_ACCELERATION,
                    AvoidMargin = 10.0f,
                    DebugColor = Color.cyan,
                    MaxLookAhead = MAX_LOOK_AHEAD
                };

                this.priorityMovement.Movements.Add(avoidCharacter);
            }
        }

        this.debugTarget.SetActive(true);
        this.wander = new DynamicWander

        {
            Character = this.character.KinematicData,
            TurnAngle = MathConstants.MATH_PI_4,
            WanderOffset = 20,
            //WanderRate = MathConstants.MATH_PI_4,
            WanderRate = 0.001f,
            DebugTarget = this.debugTarget,
            MaxAcceleration = MAX_ACCELERATION,
            DebugColor = Color.yellow
        };

        this.priorityMovement.Movements.Add(wander);
        this.blendedMovement.Movements.Add(new MovementWithWeight(wander,1));

        this.character.Movement = this.blendedMovement;
    }


    void Update()
    {
        if (Input.GetKeyDown(this.stopKey))
        {
            this.character.Movement = null;
        }
        else if (Input.GetKeyDown(this.blendedKey))
        {
            this.character.Movement = this.blendedMovement;
        }
        else if (Input.GetKeyDown(this.priorityKey))
        {
            this.character.Movement = this.priorityMovement;
        }

        drawWhiskers();

        this.UpdateMovingGameObject();
        this.UpdateMovementText();
    }

    private void drawWhiskers()
    {
        
        Vector3 forward = transform.TransformDirection(Vector3.forward) * MAX_LOOK_AHEAD;
        Vector3 left = transform.TransformDirection(Quaternion.AngleAxis(30, Vector3.up) * Vector3.forward) * MAX_LOOK_AHEAD / 3;
        Vector3 right = transform.TransformDirection(Quaternion.AngleAxis(330, Vector3.up) * Vector3.forward) * MAX_LOOK_AHEAD / 3;
        //Vector3 left = transform.TransformDirection(MathHelper.ConvertOrientationToVector(MathHelper.ConvertVectorToOrientation(Vector3.forward) + 90)) * MAX_LOOK_AHEAD / 3;
        //Vector3 right = transform.TransformDirection(MathHelper.ConvertOrientationToVector(MathHelper.ConvertVectorToOrientation(Vector3.forward) - 180)) * MAX_LOOK_AHEAD / 3;
        Debug.DrawRay(transform.position, forward, Color.green);    
        Debug.DrawRay(transform.position, left, Color.green);
        Debug.DrawRay(transform.position, right, Color.green);
    }

    void OnDrawGizmos()
    {
        if (this.character != null && this.character.Movement != null)
        {
            //var wander = this.character.Movement as BlendedMovement;
            if (this.wander != null)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(wander.CircleCenter, wander.WanderRadius);
            }
        }
    }

    private void UpdateMovingGameObject()
    {
        if (this.character.Movement != null)
        {
            this.character.Update();
            this.character.KinematicData.ApplyWorldLimit(X_WORLD_SIZE, Z_WORLD_SIZE);
        }
    }

    private void UpdateMovementText()
    {
        if (this.character.Movement == null)
        {
            this.movementTextText.text = this.name + " Movement: Stationary";
        }
        else
        {
            this.movementTextText.text = this.name + " Movement: " + this.character.Movement.Name;
        }
    } 

}
