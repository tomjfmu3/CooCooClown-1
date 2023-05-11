//using statements
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ShooterV2 : MonoBehaviour
{

    //declares components needed for shooting projectiles
    [Header("Projectile Spawning")]
    public Transform cam;
    public Transform attackPoint;
    public GameObject objectToThrow;
    public float lastfired;
    public GameObject chainProj;
    public float SMGFireRate = 70;
    public float ChainFireRate = 70;


    //declares component needed for setting UI text
    [Header("UI")]
    public Text totalAmmoText;

    //declares variables needed for mag capacity and time between shots
    [Header("Projectile Cooldown")]
    public int totalThrows;
    public float throwCooldown;

    //declares variables needed for shooting and setting projectile trajectory
    [Header("Throwing")]
    public KeyCode throwKey = KeyCode.Mouse0;
    private float throwForce= 80;
    private float throwUpwardForce = 12;
    bool readyToThrow;

    //decalres static variables that discern whether the user can go to their ship
    public static int keyCount = 0;


    //declares components for weapon sound effects
    [Header("Audio")]
    AudioSource soundFX;
    public AudioClip reload;
    public AudioClip pistolSFX;
    public AudioClip smgSFX;
    public AudioClip shotgunSFX;
    public AudioClip chainSFX;
    public AudioClip pickUpSFX;



    //array of weapon icons for showing current weapon on hud
    private List<GameObject> wepsIcons = new List<GameObject>();

    //bool used for playling animation when user picks up weapon or key
    public static bool pickUp = false;
    public static bool keyPickUp = false;


    //array of weapons
    private List<GameObject> weps = new List<GameObject>();

    //used for keeping track of what weapon is currently enabled
    public static int currentWeapon = 0;

    //keeps total ammo
    public static int totalAmmo { get; set; }
    public static bool[] canUseWeapons = { true, false, false, true };

    //keeps max ammount of ammo a given weapon can hold at one time
    private int magCap;

    private void Start()
    {
        //defines sound component
        soundFX = GetComponent<AudioSource>();

        //defines totalAmmo variable and bool that denotes whether the player can shoot
        totalAmmo = 50;
        readyToThrow = true;
        currentWeapon = 0;

        //puts all weapon gameobjects into array
        weps.Add(GameObject.Find("Main Camera").transform.Find("pistol").gameObject);
        weps.Add(GameObject.Find("Main Camera").transform.Find("smg").gameObject);
        weps.Add(GameObject.Find("Main Camera").transform.Find("shotgun").gameObject);
        weps.Add(GameObject.Find("Main Camera").transform.Find("chain").gameObject);

        totalAmmoText.text = totalAmmo.ToString() + "\\" + totalThrows.ToString();


        //puts all weapon icons into array
        wepsIcons.Add(GameObject.Find("curWeaponBg").transform.Find("pistolC").gameObject);
        wepsIcons.Add(GameObject.Find("curWeaponBg").transform.Find("macC").gameObject);
        wepsIcons.Add(GameObject.Find("curWeaponBg").transform.Find("shotgunC").gameObject);
        wepsIcons.Add(GameObject.Find("curWeaponBg").transform.Find("chainC").gameObject);

        if (SceneManager.GetActiveScene().name == "Level2") { canUseWeapons[1] = true; canUseWeapons[2] = true; }




    }

    private void Update()
    {

        if (pickUp)
        {
            switchWeps(currentWeapon);
            pickUp = false;
        }

        if (keyPickUp) 
        {
            soundFX.PlayOneShot(pickUpSFX, 2);
            keyPickUp = false;
        }


        //if user can shoot weapon
        if (Input.GetKeyDown(throwKey) && readyToThrow && totalThrows > 0 && currentWeapon == 0)
        {
            soundFX.PlayOneShot(pistolSFX, 1);
            //actually shoots weapon
            Throw(currentWeapon);

        }
        else if (Input.GetKey(throwKey) && readyToThrow && totalThrows > 0 && currentWeapon == 1)
        {
            if (readyToThrow && Time.time - lastfired > 1 / SMGFireRate)
            {
                lastfired = Time.time;
                soundFX.PlayOneShot(smgSFX, 2);
                //actually shoots weapon
                Throw(currentWeapon);
            }
        }
        else if (Input.GetKeyDown(throwKey) && readyToThrow && totalThrows > 0 && currentWeapon == 2)
        {
            soundFX.PlayOneShot(shotgunSFX, 1);
            //actually shoots weapon
            Throw(currentWeapon);
        }
        else if (Input.GetKey(throwKey) && readyToThrow && currentWeapon == 3)
        {
            if (Time.time - lastfired > 1 / ChainFireRate)
            {
                lastfired = Time.time;
                soundFX.PlayOneShot(chainSFX, 1);
                //actually shoots weapon
                Throw(currentWeapon);
            }
        }


        //if user is out of ammo in their magazine
        else if (Input.GetKeyDown(throwKey) && readyToThrow && totalThrows <= 0 && totalAmmo > 0){

            //sets magazine capacity depending on currently selected weapon
            if(currentWeapon == 0){ magCap = 12; }
            else if(currentWeapon == 1){ magCap = 30;}
            else if(currentWeapon == 2){ magCap = 6; }

            //if total ammo is greater than mag capacity, reload a full magazine
            if(totalAmmo >= magCap){
                totalAmmo -= magCap;
                totalThrows += magCap;
            }
            //if not, use up the rest of the ammo
            else{
                totalThrows += totalAmmo;
                totalAmmo -= totalAmmo;
            }
            //play reload sound effect and animation
            weps[currentWeapon].GetComponent<Animator>().Play("reload",  -1, 0f);
            soundFX.PlayOneShot(reload, 4);
            
        } 

        //update text
        totalAmmoText.text = totalAmmo.ToString() + "\\" + totalThrows.ToString();
       

        //switch weapon if user presses tab
        if(Input.GetKeyDown(KeyCode.Tab)){switchWeps(currentWeapon);}
    
     }



    //switches weapons
    public void switchWeps(int index){

        //play reload sound effect and animation
        weps[currentWeapon].GetComponent<Animator>().Play("exit",  -1, 0f);
        soundFX.PlayOneShot(reload, 4);

        //find next availiable weapon
        bool enabledCheck = false;
        while(!enabledCheck){
            currentWeapon += 1;   
            if(currentWeapon >= weps.Count){ currentWeapon = 0;}
            if (canUseWeapons[currentWeapon]){ enabledCheck = true; }
        }

        //makes it so that it switches from the last weapon to the first
        switchAmmoSettings(currentWeapon);
        
        //sets weapon and weapon icon to active
        weps[currentWeapon].SetActive(true);
        wepsIcons[currentWeapon].SetActive(true);

        


        //disables all other weapons not being used (and their icons)
        for (int i = 0; i < weps.Count; i++){
            if(weps[i] != weps[currentWeapon]){
                weps[i].SetActive(false);
                wepsIcons[i].SetActive(false);

            }
        }


        //play reload sound effect and animation
        weps[currentWeapon].GetComponent<Animator>().Play("enter",  -1, 0f);
        soundFX.PlayOneShot(reload, 4);
        
    }

    //sets the force and ammo for each weapon
    private void switchAmmoSettings(int index){

        //pistol
        if(index == 0){

            if(totalThrows > 12){totalThrows = totalThrows - (totalThrows - 12); }
            throwCooldown = .3f;
            throwForce = 80;

        }
        //smg
        else if(index == 1){

            if(totalThrows > 30){ totalThrows = totalThrows - (totalThrows - 30);}
            throwCooldown = .2f;
            throwForce = 120;

        }
        //shotgun
        else if(index == 2){

            if(totalThrows > 6){ totalThrows = totalThrows - (totalThrows - 6); }
            throwCooldown = .4f;
            throwForce = 80;

        }
        else if(index == 3){

            if(totalThrows > 12){totalThrows = totalThrows - (totalThrows - 12); }
            throwCooldown = .10f;
            throwForce = 5;

        }

        //updates text
        totalAmmoText.text = totalAmmo.ToString() + "\\" + totalThrows.ToString();

        

    }

    //actually fires weapon
    private void Throw(int index)
    {

        //plays animation
        weps[currentWeapon].GetComponent<Animator>().Play( weps[currentWeapon].name + "Shoot",  -1, 0f);

        readyToThrow = false;

        GameObject projectile;
        // instantiate object to throw
        if(currentWeapon == 3){ projectile = Instantiate(chainProj, attackPoint.position, cam.rotation);}
        else { projectile = Instantiate(objectToThrow, attackPoint.position, cam.rotation); }

        //disables colldier
        projectile.GetComponent<BoxCollider>().enabled = false;

        // get rigidbody component
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        // calculate direction
        Vector3 forceDirection = cam.transform.forward;
        RaycastHit hit;
        if(Physics.Raycast(cam.position, cam.forward, out hit, 500f)) {forceDirection = (hit.point - attackPoint.position).normalized;}

        // add force
        Vector3 forceToAdd = forceDirection * throwForce + transform.up * throwUpwardForce;
        projectileRb.AddForce(forceToAdd, ForceMode.Impulse);

        //subtracts ammo and updates text
        if(index != 3){
            totalThrows--;
            totalAmmoText.text = totalAmmo.ToString() + "\\" + totalThrows.ToString();
        }
        // implement throwCooldown
        Invoke(nameof(ResetThrow), throwCooldown);

        //re-enable collider 
        StartCoroutine(renableCol(projectile));
        

    }

    private IEnumerator renableCol(GameObject proj){
        yield return new WaitForSeconds(.1f);
        proj.GetComponent<BoxCollider>().enabled = true;
    }


    //sets bool so that user can fire weapon again
    private void ResetThrow(){ readyToThrow = true;  }
}
