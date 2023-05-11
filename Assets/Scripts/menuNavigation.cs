//using statements
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour 
{
    //declares components needed for audio
    AudioSource soundEffect;
    
    void Start(){
        Debug.Log(ControllerH.diff);
    }

    //changes scene to the one given in the button onclick
    public void sceneChange(string name){

        //initializes and plays button sound effect before changing scene
        soundEffect = GetComponent<AudioSource>();
        StartCoroutine(WaitAndLoadScene(.2f, name));

    }

    //quits application
    public void close(){
        Application.Quit();
    }


    private IEnumerator WaitAndLoadScene(float clipLength, string sceneName)
    {
     //plays sound effect
     soundEffect.Play();
     
     //load the scene asynchrounously, it's important you set allowsceneactivation to false
     //in order to wait for the audioclip to finish playing
     AsyncOperation sceneLoading = SceneManager.LoadSceneAsync(sceneName);
     sceneLoading.allowSceneActivation = false;
     
     //wait for the audioclip to end
     yield return new WaitForSeconds(clipLength);
     //wait for the scene to finish loading (it will always stop at 0.9 when allowSceneActivation is false
     while (sceneLoading.progress < 0.9f) yield return null;
     //allow the scene to load
     sceneLoading.allowSceneActivation = true;
    }

    public void setDifficulty(int mode){
        if(mode == 0){
            ControllerH.diff = 0;
            GameObject.Find("buttonHolder").transform.Find( "easyMsg" ).gameObject.SetActive(true);
                                Invoke("disableMsg", 2f);

        }
        else if(mode == 1){
            ControllerH.diff = 1;
            GameObject.Find("buttonHolder").transform.Find( "mediumMsg" ).gameObject.SetActive(true);
                    Invoke("disableMsg", 2f);

        }
        else if(mode == 2){
            ControllerH.diff = 2;
            GameObject.Find("buttonHolder").transform.Find( "hardMsg" ).gameObject.SetActive(true);
                    Invoke("disableMsg", 2f);

        }

    }

    public void disableMsg(){
        GameObject.Find("buttonHolder").transform.Find( "easyMsg" ).gameObject.SetActive(false);
        GameObject.Find("buttonHolder").transform.Find( "mediumMsg" ).gameObject.SetActive(false);
        GameObject.Find("buttonHolder").transform.Find( "hardMsg" ).gameObject.SetActive(false);
        

    }

}
