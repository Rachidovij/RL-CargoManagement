/*using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;


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

public class TruckController : MonoBehaviour
{

    [SerializeField]
    private float maxAcceleration = 20.0f;
    [SerializeField]
    private float turnSensitivity = 1.0f;
    [SerializeField]
    private float maxSteerAngle = 45.0f;
    [SerializeField]
    private Vector3 _centerOfMass;
    [SerializeField]
    private List<Wheel> wheels;

    private float inputX, inputY;

    private Rigidbody _rb;

    
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.centerOfMass = _centerOfMass;
    }

    
    private void Update()
    {
        AnimateWheels();
        GetInputs();
    }

    private void LateUpdate()
    {
        Move();
        Turn();
    }

    private void GetInputs()
    {
        inputX = Input.GetAxis("Horizontal");
        inputY = Input.GetAxis("Vertical");
    }

    private void Move()
    {
        foreach (var wheel in wheels)
        {
            wheel.collider.motorTorque = inputY * maxAcceleration * 500 * Time.deltaTime;

        }
    }

    private void Turn()
    {
        foreach (var wheel in wheels)
        {
            if (wheel.axel == Axel.Front)
            {	
            	print("yes!!!");
                var _steerAngle = inputX * turnSensitivity * maxSteerAngle;
                print(_steerAngle);
                wheel.collider.steerAngle = Mathf.Lerp(wheel.collider.steerAngle,_steerAngle,0.5f);
            }
        }
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
}


/*using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;



public enum Axel{
	Front,
	Rear
}

[Serializable]
public struct Wheel {

	public GameObject model;
	public WheelCollider collider;
	public Axel Axel;
}


public class TruckController : MonoBehaviour
{
	[SerializeField]
	private float maxAcceleration = 20.0f;
	[SerializeField]
	private float turnSensitivity = 1.0f;
	[SerializeField]
	private float maxSteerAngle = 45.0f;
	[SerializeField]
	private List<Wheel> Wheels;
	[SerializeField]
	private Vector3 centerOfMass;

	private float inputX, inputY;
	private Rigidbody rb; 


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass;
    }

    // Update is called once per frame
    void Update()
    {
        GetInputs();
        AnimateWheels();
    }

    void GetInputs() {

    	inputX = Input.GetAxis("Horizontal");
    	inputY = Input.GetAxis("Vertical");
    }

    private void LateUpdate(){
    	//Move();
    	Turn();
     }

     private void Move(){

     	foreach(var wheel in Wheels )
     	{
     		wheel.collider.motorTorque = inputY * maxAcceleration * 5000 * Time.deltaTime;
     	}
     }

     private void Turn() {
     	foreach (var wheel in Wheels){
     		if (wheel.Axel == Axel.Front )
     		{
     			var _steerAngle = inputX * turnSensitivity * maxSteerAngle;
     			wheel.collider.steerAngle = _steerAngle; //Mathf.Lerp(wheel.collider.steerAngle, _steerAngle, 0.5f);
     		}
     	}
     }

     private void AnimateWheels(){
     	foreach (var wheel in Wheels) {

     		Quaternion _rot;
     		Vector3 _pos; 
     		wheel.collider.GetWorldPose(out _pos, out _rot);
     		wheel.model.transform.position = _pos;
     		wheel.model.transform.rotation = _rot;
     	}
     }

}
*/