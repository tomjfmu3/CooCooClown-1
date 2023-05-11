using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class collectible : MonoBehaviour
{

    //total ammo text
    private Text ammo;
    private Text healthText;

    private GameObject blue;
    private GameObject yellow;
    private GameObject red;

    [Header("Audio")]
    AudioSource soundFX;
    public AudioClip secretSFX;
    public AudioClip healthSFX;
    public AudioClip ammoSFX;


    //defines ammo text object and key ui sprites
    void Start(){
soundFX = GetComponent<AudioSource>(); 
        blue = GameObject.Find("keys").transform.Find( "blue" ).gameObject;
        yellow = GameObject.Find("keys").transform.Find( "yellow" ).gameObject;
        red = GameObject.Find("keys").transform.Find( "red" ).gameObject;

        
         ammo = GameObject.Find("totalAmmoText").GetComponent<Text>(); 
         healthText = GameObject.Find("Health").GetComponent<Text>(); 

         
    }

    
    //when player collides with collectiable
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // if collectible is an ammo pickup, increase ammo and adjust text accordingly
            if (gameObject.tag == "ammo")
            {
                soundFX.PlayOneShot(ammoSFX, 1);
                ShooterV2.totalAmmo += 50;
                ammo.text = ShooterV2.totalAmmo.ToString();
                ControllerH.pickUpSFX[1] = true;
                Destroy(gameObject);

                
            }
            else if (gameObject.tag == "health")
            {
                soundFX.PlayOneShot(healthSFX, 1);
                ControllerH.health += 20;
                healthText.text = ControllerH.health.ToString();
                ControllerH.pickUpSFX[0] = true;
                Destroy(gameObject);
                


            }
            // if collectible is a weapon pickup, set the bool that decides whether the player can use the weapon to true
            else if (gameObject.tag == "weapon")
            {
                if (gameObject.name == "smgPickUp")
                {
                    ShooterV2.currentWeapon = 0;
                    ShooterV2.canUseWeapons[1] = true;
                    GameObject.Find("Canvas").transform.Find("navBar").gameObject.transform.Find("macBtn").gameObject.SetActive(true);
                    Destroy(gameObject);

                }
                else if (gameObject.name == "shotgunPickUp")
                {
                     soundFX.PlayOneShot(secretSFX, 1);
                    ShooterV2.currentWeapon = 1;
                    ShooterV2.canUseWeapons[2] = true;
                    GameObject.Find("Canvas").transform.Find("navBar").gameObject.transform.Find("shotgunBtn").gameObject.SetActive(true);
                    GameObject.Find("Messages").transform.Find("secret").gameObject.SetActive(true);
                    Invoke("disableMsg", 2f);
                   
                }
                ShooterV2.pickUp = true;
            }
            else if (gameObject.tag == "key")
            {
                if (gameObject.name == "tentkey")
                {
                    blue.SetActive(true);
                    Destroy(gameObject);
                    ControllerH.hasKeys[0] = true;
                }
                else if (gameObject.name == "backstagekey")
                {
                    red.SetActive(true);
                    Destroy(gameObject);
                    ControllerH.hasKeys[1] = true;

                }
                else if (gameObject.name == "masterkey")
                {
                       soundFX.PlayOneShot(secretSFX, 1);
                    yellow.SetActive(true);
                    GameObject.Find("Messages").transform.Find("secret").gameObject.SetActive(true);
                    ControllerH.hasKeys[2] = true;
                    Invoke("disableMsg", 2f);
                    
                }
                ShooterV2.keyPickUp = true;
                ShooterV2.keyCount++;
            }
        }

    }


     public void disableMsg(){
        GameObject.Find("Messages").transform.Find( "key" ).gameObject.SetActive(false);
        GameObject.Find("Messages").transform.Find( "secret" ).gameObject.SetActive(false);
        //destroys collectible
            Destroy(gameObject);

    }
}
