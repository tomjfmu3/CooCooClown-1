using UnityEngine;

//makes sprite always look at camera
public class Billboard : MonoBehaviour
{
    void Update() 
    {
        transform.LookAt(Camera.main.transform.position, Vector3.up);
    }
}