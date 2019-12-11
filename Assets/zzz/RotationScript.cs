using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationScript : MonoBehaviour {

    public int distance = 5;
    public int width = 3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A)) {
            
            transform.Rotate(Vector3.up,-2);
            
        }
        if (Input.GetKey(KeyCode.D)) {
            
            transform.Rotate(Vector3.up,2);
            
        }
        if (Input.GetKey(KeyCode.W)) {
            
            transform.position +=(transform.forward * 2);
            
        }
        if (Input.GetKey(KeyCode.S)) {
            
            transform.position +=(-transform.forward * 2);
            
        }
      
    }
}