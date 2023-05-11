using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]

public class ControllerH : MonoBehaviour
{
    public float speed = 7.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Camera playerCamera;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;

    public static int health = 100;

    public static int diff = 1;


    public static bool[] pickUpSFX = {false, false};
    public AudioClip healthSFX;
    public AudioClip ammoSFX;


    [Header("Audio")]
    AudioSource soundFX;
    public AudioClip hurt;
    public AudioClip explosionSFX;
    
    public Text healthText;
    public Text scoreText;

    public Transform expPoint;
public GameObject explosionPF;

    public static bool[] hasKeys = {false, false, false};
    public static int killCount; 
    public static int score;


    CharacterController characterController;
    [HideInInspector]
    public Vector3 moveDirection = Vector3.zero;
    Vector2 rotation = Vector2.zero;

    [HideInInspector]
    public bool canMove = true;

    void Start()
    {
        ControllerH.health = 100;
        killCount = 0;
score = 0;
hasKeys[0] = false; hasKeys[1] = false; hasKeys[2] = false;

        Destroy (GameObject.Find("Music"));
                soundFX = GetComponent<AudioSource>();

        characterController = GetComponent<CharacterController>();
        rotation.y = transform.eulerAngles.y;
        healthText.text = health.ToString();


Debug.Log("START");

    }

    void Update()
    {

                if(pickUpSFX[0] == true){ soundFX.PlayOneShot(healthSFX, 2); pickUpSFX[0] = false;}
                else if(pickUpSFX[1] == true){ soundFX.PlayOneShot(ammoSFX, 2); pickUpSFX[1] = false;}


                scoreText.text = score.ToString();

        if (characterController.isGrounded)
        {
            // We are grounded, so recalculate move direction based on axes
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);
            float curSpeedX = canMove ? speed * Input.GetAxis("Vertical") : 0;
            float curSpeedY = canMove ? speed * Input.GetAxis("Horizontal") : 0;
            moveDirection = (forward * curSpeedX) + (right * curSpeedY);

            if (Input.GetButton("Jump") && canMove)
            {
                moveDirection.y = jumpSpeed;
            }
        }

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        moveDirection.y -= gravity * Time.deltaTime;

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);

        // Player and Camera rotation
        if (canMove)
        {
            rotation.y += Input.GetAxis("Mouse X") * lookSpeed;
            rotation.x += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotation.x = Mathf.Clamp(rotation.x, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotation.x, 0, 0);
            transform.eulerAngles = new Vector2(0, rotation.y);
        }
    }

    // when collided with another gameObject
	void OnCollisionExit (Collision newCollision)
	{

		// only do stuff if hit by a projectile
		if (newCollision.gameObject.tag == "enp" || newCollision.gameObject.tag == "grenade") {

            soundFX.PlayOneShot(hurt, 2);
            

            if(newCollision.gameObject.tag == "enp"){  
                if(diff == 0) {health -= 5;}
                else if(diff == 1) {health -= 10;}
                else if(diff == 2) {health -= 15;}

                
                }
            else if(newCollision.gameObject.tag == "grenade"){health -= 20;soundFX.PlayOneShot(explosionSFX, 2);                          Instantiate (explosionPF, expPoint.position, transform.rotation);
}
			
            healthText.text = health.ToString();

            if(health <= 0){
// repurpose the timer to display a message to the player, and display restart and quit buttons
		healthText.text = "OVER";
		GameObject.Find("Canvas").transform.Find( "DeathScreen" ).gameObject.SetActive(true);
        GameObject.Find("Player").GetComponent<MouseLooker>().enabled = false;
                GameObject.Find("Main Camera").GetComponent<ShooterV2>().enabled = false;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

            Debug.Log(health);
            			Destroy (newCollision.gameObject);
                                        // destroy the projectile

				
			
		}
	}
}