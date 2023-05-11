using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScript : MonoBehaviour {

    //floats used for speed
    public float rotateSpeed = 100.0f;
    public float motionSpeed = 0.5f;

    //bools used for enabling rotation and motion
    public bool enableRotation = true;
    public bool enableMotion = false;


	
	// Update is called once per frame
	void Update () {
        // Rotate the GO around Y.
        this.gameObject.transform.Rotate(Vector3.up * rotateSpeed 
                                         * Time.deltaTime 
                                         * (enableRotation?1.0f:.0f) );


        this.gameObject.transform.Translate(Vector3.up * motionSpeed
                                            * Mathf.Cos(Time.timeSinceLevelLoad)
                                           * (enableMotion ? 1.0f : 0.0f));

    }
}
