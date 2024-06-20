using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RLAgent : MonoBehaviour
{
    NeuralNetwork deepRLNet = new NeuralNetwork();
    public float constantSpeed = 20.0f; 
    public float angle = 0.0f;
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

    private SphereCollider FL_WheelCollider; 
    private SphereCollider FR_WheelCollider; 
    private SphereCollider RL_WheelCollider; 
    private SphereCollider RR_WheelCollider; 

    public GameObject Camera;

    public GameObject Lane1;
    public GameObject Lane2;

    public bool test = false; 


    List<List<float>> states = new List<List<float>>();
    List<float> actions = new List<float>();
    List<float> rewards = new List<float>();

    Vector3 initialPos; 
    Quaternion initialRot; 

    int road; 

    private float negativeInf = -200; // -inf is arbitrary value for us, can tweak it experimentally 

    int epochs = 0; 

    float prevZ; 


    // Start is called before the first frame update
    void Start()
    {
        // it's a lane not a road but.. same thing. 
        road = LayerMask.NameToLayer("Road");
        rb = GetComponent<Rigidbody>(); 

        initialPos = transform.position;
        initialRot = transform.rotation; 

        prevZ = initialPos.z; 

        FL_WheelCollider = FL_Wheel.GetComponent<SphereCollider>();
        FR_WheelCollider = FR_Wheel.GetComponent<SphereCollider>();
        RL_WheelCollider = RL_Wheel.GetComponent<SphereCollider>();
        RR_WheelCollider = RR_Wheel.GetComponent<SphereCollider>();

        // List<float> initial_ = new List<float> {Vector3.Distance(transform.position, Lane1.transform.position), Vector3.Distance(transform.position, Lane2.transform.position)};
        // Debug.Log(initial_[0]);
        // Debug.Log(initial_[1]);

        // deepRLNet.loadWeights("Weights.txt"); // loads weights from pre-existing txt file. 

    
    }


    // Update is called once per frame
    // Per frame. 
    void FixedUpdate()
    {   
        // if(states.Count > 0){

        //     rewards.Add((transform.position.z - prevZ)); 

        //     prevZ = transform.position.z;
        // }

        float constantAngularSpeed = Mathf.Rad2Deg * (constantSpeed / wheelRadius); // RPS

        FL_Wheel.transform.Rotate(constantAngularSpeed * Time.deltaTime, 0.0f, 0.0f);
        FR_Wheel.transform.Rotate(constantAngularSpeed * Time.deltaTime, 0.0f, 0.0f);
        RL_Wheel.transform.Rotate(constantAngularSpeed * Time.deltaTime, 0.0f, 0.0f);
        RR_Wheel.transform.Rotate(constantAngularSpeed * Time.deltaTime, 0.0f, 0.0f);

        // FL_Wheel.transform.RotateAround(FL_WheelCollider.bounds.center, FL_Wheel.transform.right, constantAngularSpeed * Time.deltaTime);
        // FR_Wheel.transform.RotateAround(FR_WheelCollider.bounds.center, FR_Wheel.transform.right, constantAngularSpeed * Time.deltaTime);
        // RL_Wheel.transform.RotateAround(RL_WheelCollider.bounds.center, RL_Wheel.transform.right, constantAngularSpeed * Time.deltaTime);
        // RR_Wheel.transform.RotateAround(RR_WheelCollider.bounds.center, RR_Wheel.transform.right, constantAngularSpeed * Time.deltaTime);


        states.Add(new List<float> {Vector3.Distance(transform.position, Lane1.transform.position), Vector3.Distance(transform.position, Lane2.transform.position)});

        deepRLNet.evaluateP(states[states.Count - 1]); // evaluate the PDF fn... 

        angle = deepRLNet.sampleAngle();

    
        actions.Add(angle);

        transform.eulerAngles = new Vector3(0, angle, 0);

        angle += 90; 

        rb.velocity = new Vector3(constantSpeed * Mathf.Cos(angle * Mathf.Deg2Rad), 0, constantSpeed * Mathf.Sin(angle * Mathf.Deg2Rad));

        rewards.Add(transform.position.z); 

    }


    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == road)
        {
            // Don't train if test is set to true! 
            if(!test){
                epochs++; 
                if(rewards.Count == 0) {
                    epochs--; 
                    return; // Get out of this function -- this was a fluke. 
                }
                for(int i = 0; i < rewards.Count; i++){
                    rewards[i] =  transform.position.z - rewards[i]; 
                }

                // Run a train function 
                deepRLNet.train(states, actions, rewards); 

                Debug.Log("End of epoch " + epochs);
                Debug.Log("Car drove " + transform.position.z + "ft");
                deepRLNet.saveWeights(); 
            }

            transform.position = initialPos; 
            transform.rotation = initialRot;

            angle = 0.0f; 
            prevZ = initialPos.z;

            // reinit all lists...
            states = new List<List<float>>();
            actions = new List<float>();
            rewards = new List<float>();
        }

        // to do : make sure to rotate only the wheels. and add torque. 
    }


}