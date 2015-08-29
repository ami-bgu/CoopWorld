using UnityEngine;
using System.Collections;

public class CoopRobot : MonoBehaviour {

    public GameObject SpawnEffect;

	// Use this for initialization
	void Start () {
        Instantiate(SpawnEffect,this.transform.position,Quaternion.identity);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
