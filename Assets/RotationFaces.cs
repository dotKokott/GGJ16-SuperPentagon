using UnityEngine;
using System.Collections;

public class RotationFaces : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
		iTween.RotateBy(gameObject, iTween.Hash("z", -1, "speed", 15.5f, "easetype", iTween.EaseType.linear));
 	//transform.Rotate();

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
