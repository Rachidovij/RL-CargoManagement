using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using MLAgents.Sensor;
using Random=System.Random;
/**/


public enum Axel
{
    Front,
    Rear
}

[Serializable]
public struct Wheel
{
    public GameObject model;
    public WheelCollider collider;
    public Axel axel;
}

public class TagMaster : Agent
{


    [SerializeField]
    private float maxAcceleration = 20.0f;
    [SerializeField]
    private float turnSensitivity = 1.0f;
    [SerializeField]
    private float maxSteerAngle = 30.0f;
    [SerializeField]
    private Vector3 _centerOfMass;
    [SerializeField]
    private List<Wheel> wheels;

    private float inputX, inputY;
    public float speed = 10f;
    public float torque = 10f;
    public Random rnd = new Random();



    //private Rigidbody _rb;



	public float movementSpeed;
	public float rotationSpeed;
	private float rotation;
	private Rigidbody rBody;
	//private RayPerceptionSensorComponent3D rayPer;
	//private RayPerception rayPer;
	public bool useVerctorOrbs;


	PushBlockSettings m_PushBlockSettings;

	[SerializeField] private SceneController sceneController;
	[SerializeField] private bool isCarryingTruck = false;
	[SerializeField] GameObject carriedTruck; 
	[SerializeField] private int CarriedTruckID;

	public override void InitializeAgent() {  
		base.InitializeAgent(); 
		rBody= GetComponent<Rigidbody>();
		 rBody.centerOfMass = _centerOfMass;
		//rayPer = GetComponent<RayPerceptionSensorComponent3D>();
		//rayPer = GetComponent<RayPerception>();

		
	}

	void Awake()
    {
        m_PushBlockSettings = FindObjectOfType<PushBlockSettings>();
        //m_BlockRb = PushBlock.BlockListRB();

    }

	public override void AgentAction(float[] vectorAction) 

	{	AddReward(-1/agentParameters.maxStep);

		isCarryingTruck = carriedTruck != null;
		if (carriedTruck!=null) {
			carriedTruck.transform.position = new Vector3(transform.position.x, 1.5f, transform.position.z) ;
			
		}

		movementControl(vectorAction);
		AnimateWheels();

	}

	private void AnimateWheels()
	    {
	        foreach (var wheel in wheels)
	        {
	            Quaternion _rot;
	            Vector3 _pos;
	            wheel.collider.GetWorldPose(out _pos, out _rot);
	            wheel.model.transform.position = _pos;
	            wheel.model.transform.rotation = _rot;
	        }
	    }

   /// <summary>
    /// Moves the agent according to the selected action.
    /// </summary>
    public void movementControl(float[] act)
    {
        //var dirToGo = Vector3.zero;
        var dirToGo=0.0f;
        var turn = false;
        var rotateDir = 0.0f;

        var action = Mathf.FloorToInt(act[0]);
        int rndDir  = rnd.Next(0, 2);

        // Goalies and Strikers have slightly different action spaces.
        switch (action)
        {
            case 1:
                dirToGo =  1f;
                break;
            case 2:
            	dirToGo = -1f;
            	if (rndDir >0.5) { 
            		dirToGo = -1f;
            	}
                
                break;
            case 3:
            	dirToGo = -1f;
                rotateDir =  1f;
                turn = true;
                break;
            case 4:
            	if (rndDir > 0.1) {
                rotateDir = -1f;
                turn = true; }
                break;
            case 5:
                dirToGo =  -0.75f;
                break;
            case 0:
                dirToGo =  0.75f;
                break;
        }
        // transform.Rotate(rotateDir, Time.fixedDeltaTime * 2f);
        // rBody.AddForce(dirToGo * m_PushBlockSettings.agentRunSpeed,
        //     ForceMode.VelocityChange);

        foreach (var wheel in wheels)
        {
            wheel.collider.motorTorque = dirToGo * maxAcceleration * 500 ; // * Time.deltaTime;

        }
        if (turn == true) {
        	Turn( rotateDir);
        }

        if(Mathf.Abs(Vector3.Dot(transform.up, Vector3.down)) < 0.125f)
 {
     // Car is primarily neither up nor down, within 1/8 of a 90 degree rotation
 	AddReward(-3);
 	Done();
     // Therefore, check whether it's on either side. Otherwise, it's on front/back
     if(Mathf.Abs(Vector3.Dot(transform.right, Vector3.down)) > 0.825f)
     {
         // Car is within 1/8 of a 90 degree rotation of either side
     }
 }
    }


	private void Turn(float Dir)
	    {
	        foreach (var wheel in wheels)
	        {
	            if (wheel.axel == Axel.Front)
	            {	

	                var _steerAngle = Dir * turnSensitivity * maxSteerAngle;
	                wheel.collider.steerAngle = Mathf.Lerp(wheel.collider.steerAngle,_steerAngle,0.5f);

	            }
	        }
	    }

/*	void movementControl(float[] act) {
		print("yeesss");
		float action = act[0];
		switch (action) {
			case 1: {
				transform.position += transform.right * movementSpeed * Time.deltaTime;

			} 
			break;

			case 2: {
				transform.position -= transform.forward * movementSpeed * Time.deltaTime;
			} 
			break;

			case 3: {
				rotation += rotationSpeed * Time.deltaTime;
				transform.rotation =  Quaternion.Euler(0,0,rotation) ;
			} 
			break;

			case 4: {
				rotation -= rotationSpeed * Time.deltaTime;
				transform.rotation = Quaternion.Euler(0,0,rotation) ;
			} 
			break;
		}


	}*/

     public override float[] Heuristic()
    {
        if (Input.GetKey(KeyCode.D))
        {
            return new float[] { 3 };
        }
        if (Input.GetKey(KeyCode.P))
        {
            return new float[] { 1 };
        }
        if (Input.GetKey(KeyCode.K))
        {
            return new float[] { 4 };
        }
        if (Input.GetKey(KeyCode.S))
        {
            return new float[] { 2 };
        }
        return new float[] { 8 };
    }


	 public override void AgentReset() {


	 	sceneController.EmptySpawnPoints();
	 	sceneController.ResetCargoTrucks();
	 	sceneController.RestoreCargoToSpawn();
	 	sceneController.MoveAgentToRandomLocation();
	 	sceneController.randomlyPlaceCargoAreas();
	 	sceneController.despawnAllCargoes();


	 }


	void UpdateCarriedTruckID() {

		if(carriedTruck!=null) {
			Cargo Cargo = carriedTruck.GetComponent<Cargo>();
			CarriedTruckID = Cargo.CargoID;
			
				}
		else {
			CarriedTruckID = -1;
		}
	}


	private void onCollisionEnter(Collision collision) { 
		if (collision.gameObject.CompareTag("wall")) { 
        SetReward(-1f);
        print("Hit outer walls");
		Done(); }
	}

	private void OnTriggerEnter(Collider other) {
	
		if (other.gameObject == null && carriedTruck == null) return;

		if (other.CompareTag("Human")) {
			AddReward(-1f);

		}

		if (isCarryingTruck && carriedTruck.gameObject != null && other.CompareTag("Lane")) {
			AddReward(0.01f);
		}

		
		if ( isCarryingTruck == false && (other.CompareTag("smallCargo") || other.CompareTag("bigCargo")) ) {
			
			carriedTruck = other.gameObject;
			carriedTruck.tag += "_carried";
			other.GetComponent<BoxCollider>().enabled = false;
			AddReward(5f);
		}
		if (isCarryingTruck && carriedTruck.gameObject != null && (other.CompareTag("smallSize") || other.CompareTag("bigSize")) ) {

			if 	( (other.CompareTag("smallSize") && carriedTruck.CompareTag("smallCargo_carried")) || (other.CompareTag("bigSize") && carriedTruck.CompareTag("bigCargo_carried")) )
			{
				AddReward(1f);
				
				GoalAreaController GoalArea = other.GetComponent<GoalAreaController>();
				GoalArea.addCargo(carriedTruck);

			}


			else {
				AddReward(-0.05f);
			}
		}

			if ( other.CompareTag("wall") ) {
			AddReward(-0.05f);	
			print("Agent collide with wall !");	
			Done();

		}
	}

		private void OnTriggerExit(Collider other) {

		if (isCarryingTruck && carriedTruck.gameObject != null && other.CompareTag("Lane")) {
			AddReward(-0.01f);

		} }

	private void OnTriggerStay(Collider other) {
		if (isCarryingTruck && carriedTruck.gameObject != null && other.CompareTag("Lane")) {
			AddReward(0.005f);

		}

		if(sceneController.AllCargoAreasFilled()) {
			AddReward(2f);
			Done();
		}
	}


}
