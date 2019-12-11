
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteracteScript : MonoBehaviour {

    public GameObject indicator;
    private Vector3 startPosition;
    private RotationScript rotationScript;
    
    // Start is called before the first frame update
    void Start() {
        startPosition = transform.position;
        rotationScript = indicator.GetComponent<RotationScript>();
    }

    // Update is called once per frame
    void Update() {

        transform.position = startPosition;
        
        
        Matrix4x4 rotationMatrix = Matrix4x4.Rotate(indicator.transform.rotation);
        Matrix4x4 translationMatrix = Matrix4x4.Translate(indicator.transform.position);

        Matrix4x4 composition =  translationMatrix * rotationMatrix.inverse;

        Vector3 newCoordPos = composition.MultiplyVector(transform.position);

        if (newCoordPos.z > rotationScript.distance && newCoordPos.z < rotationScript.distance + rotationScript.width) {

            transform.position = startPosition + Vector3.up*2;
        }

    }
}
