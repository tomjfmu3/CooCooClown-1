using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

	// make game manager public static so can access this from other scripts
	public static UIManager gm;
 
    //navbar
    GameObject navBar;

    //skybox materials
    public Material night;
    public Material day;

    //mouse state
    bool locked = true;

    // Start is called before the first frame update
    void Start(){ navBar = GameObject.Find("Canvas").transform.Find("navBar").gameObject; }

    // Update is called once per frame
    void Update()
    {
        //controls menu (M key enables and disables menu)
        if(Input.GetKeyDown(KeyCode.M))
        {
            //disables menu and locks cursor
            if(navBar.activeSelf == true){
                navBar.SetActive(false);
                GameObject.Find("Player").GetComponent<MouseLooker>().enabled = true;
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = false;
            }
            //enables menu and unlocks cursor
            else{
                navBar.SetActive(true);
                GameObject diffBtn = GameObject.Find("navBar").transform.Find( "diffBtn" ).gameObject;
                if(ControllerH.diff == 0){
                    diffBtn.transform.Find( "easyBtn" ).gameObject.SetActive(true); 
                    diffBtn.transform.Find( "medBtn" ).gameObject.SetActive(false); 
                    diffBtn.transform.Find( "hardBtn" ).gameObject.SetActive(false); 
                }
                else if(ControllerH.diff == 1){
                    diffBtn.transform.Find( "easyBtn" ).gameObject.SetActive(false); 
                    diffBtn.transform.Find( "medBtn" ).gameObject.SetActive(true); 
                    diffBtn.transform.Find( "hardBtn" ).gameObject.SetActive(false); 
                }
                else if(ControllerH.diff == 2){
                    diffBtn.transform.Find( "easyBtn" ).gameObject.SetActive(false); 
                    diffBtn.transform.Find( "medBtn" ).gameObject.SetActive(false); 
                    diffBtn.transform.Find( "hardBtn" ).gameObject.SetActive(true); 
                }
                
                
                //.gameObject.SetActive(true);

                GameObject.Find("Player").GetComponent<MouseLooker>().enabled = false;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

    }

   //changes skybox depending on current active skybox
   public void onClick_skybox(){

        //to day
        if(RenderSettings.skybox == night){ RenderSettings.skybox = day; }
        //to night
        else{ RenderSettings.skybox = night; }

    }

}
