using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {


    public float moveSpeed = 2.0f;
    public float gravity = 9.8f;
    CharacterController mController;
	// Use this for initialization
	void Start () {
        mController = this.gameObject.GetComponent<CharacterController>();

    }
	
	// Update is called once per frame
	void Update () {

        Vector3 movZ = Input.GetAxis("Vertical")
                           * Vector3.forward * moveSpeed;

        Vector3 movX = Input.GetAxis("Horizontal")
                       * Vector3.right * moveSpeed;

        Vector3 mov = transform.TransformDirection(movZ + movX);

        mov.y -= gravity * Time.deltaTime;

        mController.Move(mov);


    }
}
