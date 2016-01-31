using UnityEngine;
using System.Collections;

public class Stage : MonoBehaviour {

    public float RotationSpeed;

    //-1 stays always
    public int StageNo = -1;
    public bool Stay = false;
    	
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //transform.RotateAround( Vector3.forward, RotationSpeed * Time.deltaTime );
        transform.Rotate( 0, 0, RotationSpeed * Time.deltaTime );
	}
}
