using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Mist : MonoBehaviour {

    public float Speed = 10;

	void Start () {

	}

	void Update () {
        this.GetComponent<SpriteRenderer>().material.mainTextureOffset += Vector2.right * Speed * Time.deltaTime;
	}
}
