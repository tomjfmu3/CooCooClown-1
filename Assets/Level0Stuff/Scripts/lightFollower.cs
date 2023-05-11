using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightFollower : MonoBehaviour
{
	[SerializeField] Transform target;
    // Start is called before the first frame update
    void Start()
    {
        if (target == null) {

			if (GameObject.FindWithTag ("Player")!=null)
			{
				target = GameObject.FindWithTag ("Player").GetComponent<Transform>();
			}
		}
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
			return;
		
		transform.LookAt(target);
    }
}
