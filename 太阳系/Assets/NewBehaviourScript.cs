using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {
    public Transform earth;
    
    // Use this for initialization
    void Start () {
        Transform[] tran = { earth };
        tran[0].position = new Vector3(0, 0, 0);
        Debug.Log(earth.position);
        Debug.Log(tran[0].position);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
