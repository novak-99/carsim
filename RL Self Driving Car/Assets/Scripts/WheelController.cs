using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelController : MonoBehaviour
{
    public float constantSpeed = 1.0f; 

    public GameObject Car; 

    // Wheels
    public float wheelRadius = 0.01556486f;

    private SphereCollider wheelCollider; 
    // Start is called before the first frame update    
    int ground; 




    private void Awake(){
        ground = LayerMask.NameToLayer("Ground");
        wheelCollider = GetComponent<SphereCollider>();
    }

    void OnTriggerStay(Collider other) {
        float constantAngularSpeed = constantSpeed / wheelRadius; // RPS

        //if(GetComponent<Collider>().gameObject.layer == ground){
            Debug.Log("Hello");
            transform.RotateAround(wheelCollider.bounds.center, Car.transform.right, constantAngularSpeed * Time.deltaTime);
        //}
    }
}
