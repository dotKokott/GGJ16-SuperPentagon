using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HandBlinker : MonoBehaviour {

    public Vector3 Pos1;
    public Vector3 Pos2;

    void Start() {
        StartCoroutine( Blink() );
    }

    void Update() {

    }

    private IEnumerator Blink() {
        while ( true ) {
            transform.localPosition = Pos1;
            yield return new WaitForSeconds( 0.5f );
            transform.localPosition = Pos2;
            yield return new WaitForSeconds( 0.5f );
        }
    }
}
