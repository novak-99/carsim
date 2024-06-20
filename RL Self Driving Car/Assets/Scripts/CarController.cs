using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{

    public float constantSpeed = 1.0f; 
    public float angle = 90.0f;
    public float deltaAngle = 0.1f; 
    private Rigidbody rb; 
    
    public GameObject Car; 

    // Wheels
    private float wheelRadius = 0.01556486f;

    public float maxAngle = 155.0f; 
    public float minAngle = 25.0f; 
    
    public GameObject FL_Wheel; 
    public GameObject FR_Wheel; 
    public GameObject RL_Wheel; 
    public GameObject RR_Wheel; 

    public Transform FL_WheelTransform; 

    private SphereCollider FL_WheelCollider; 
    private SphereCollider FR_WheelCollider; 
    private SphereCollider RL_WheelCollider; 
    private SphereCollider RR_WheelCollider; 

    public GameObject Camera;


    List<float> states = new List<float>();
    List<float> actions = new List<float>();
    List<float> rewards = new List<float>();


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>(); 

        FL_WheelCollider = FL_Wheel.GetComponent<SphereCollider>();
        FR_WheelCollider = FR_Wheel.GetComponent<SphereCollider>();
        RL_WheelCollider = RL_Wheel.GetComponent<SphereCollider>();
        RR_WheelCollider = RR_Wheel.GetComponent<SphereCollider>();
        Debug.Log(FL_Wheel.transform.GetComponent<Renderer>().bounds.center); 
        Debug.Log(FR_Wheel.transform.GetComponent<Renderer>().bounds.center); 
        Debug.Log(RL_Wheel.transform.GetComponent<Renderer>().bounds.center); 
        Debug.Log(RR_Wheel.transform.GetComponent<Renderer>().bounds.center); 
    }

    // Update is called once per frame
    // Per frame. 
    void Update()
    {

        float constantAngularSpeed = Mathf.Rad2Deg * (constantSpeed / wheelRadius); // RPS

        Debug.Log(constantAngularSpeed); 

        //FL_Wheel.transform.RotateAround(FL_WheelCollider.bounds.center, FL_Wheel.transform.right, constantAngularSpeed * Time.deltaTime);
        // FR_Wheel.transform.RotateAround(FR_WheelCollider.bounds.center, Car.transform.right, constantAngularSpeed * Time.deltaTime);
        // RL_Wheel.transform.RotateAround(RL_WheelCollider.bounds.center, Car.transform.right, constantAngularSpeed * Time.deltaTime);
        // RR_Wheel.transform.RotateAround(RR_WheelCollider.bounds.center, Car.transform.right, constantAngularSpeed * Time.deltaTime);

        // FL_Wheel.transform.RotateAround(FL_WheelCollider.bounds.center, Vector3.up, angle - 90);
        // FR_Wheel.transform.RotateAround(FR_WheelCollider.bounds.center, Vector3.up, angle - 90);
        // RL_Wheel.transform.RotateAround(RL_WheelCollider.bounds.center, Vector3.up, angle - 90);
        // RR_Wheel.transform.RotateAround(RR_WheelCollider.bounds.center, Vector3.up, angle - 90);

        FL_Wheel.transform.Rotate(constantAngularSpeed * Time.deltaTime, 0.0f, 0.0f);
        FR_Wheel.transform.Rotate(constantAngularSpeed * Time.deltaTime, 0.0f, 0.0f);
        RL_Wheel.transform.Rotate(constantAngularSpeed * Time.deltaTime, 0.0f, 0.0f);
        RR_Wheel.transform.Rotate(constantAngularSpeed * Time.deltaTime, 0.0f, 0.0f);

        // FL_Wheel.transform.rotation = FL_Wheel.transform.rotation * Quaternion.Euler(constantAngularSpeed * Time.deltaTime, 0.0f, 0.0f); 
        // FR_Wheel.transform.rotation = FR_Wheel.transform.rotation * Quaternion.Euler(constantAngularSpeed * Time.deltaTime, 0.0f, 0.0f); 
        // RL_Wheel.transform.rotation = RL_Wheel.transform.rotation * Quaternion.Euler(constantAngularSpeed * Time.deltaTime, 0.0f, 0.0f); 
        // RR_Wheel.transform.rotation = RR_Wheel.transform.rotation * Quaternion.Euler(constantAngularSpeed * Time.deltaTime, 0.0f, 0.0f); 
        
        if(angle < maxAngle && ( Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))){
            angle+=deltaAngle; 

            Car.transform.Rotate(0, -deltaAngle, 0);

        }
        if(angle > minAngle && (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))){
            angle-=deltaAngle; 

            Car.transform.Rotate(0, deltaAngle, 0);

        }
        rb.velocity = new Vector3(constantSpeed * Mathf.Cos(angle * Mathf.Deg2Rad), 0, constantSpeed * Mathf.Sin(angle * Mathf.Deg2Rad));
    }

    // // ANY COLLISION is a collision with the walls. 
    // void OnCollisionEnter(Collision collision)
    // {
    //     Debug.Log("Restart!");
    //     Debug.Log(states);
    //     Debug.Log(actions);
    //     Debug.Log(rewards);

    //     // Run a train function 
    // }
}

