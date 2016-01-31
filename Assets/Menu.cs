using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

    public GameObject loading;
    public int currentitem = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetKeyDown(KeyCode.DownArrow)) {
            currentitem++;

            if(currentitem > 3) {
                currentitem = 3;
            } else {
                transform.position += Vector3.down * 1.1f;
            }


        }

        if ( Input.GetKeyDown( KeyCode.UpArrow ) ) {
            currentitem--;

            if ( currentitem < 0 ) {
                currentitem = 0;
            } else {
                transform.position += Vector3.up * 1.1f;
            }
        }

        if(Input.GetKeyDown(KeyCode.Space)) {
            if(currentitem == 0) {
                loading.SetActive( true );
                StartCoroutine( WaitAndLoad() );
            }

            if ( currentitem == 1 ) {
                Application.LoadLevel( "Highscore" );
            }

            if ( currentitem == 2 ) {
                Application.LoadLevel( "Credits" );
            }

            if ( currentitem == 3 ) {
                Application.Quit();
            }
        }
    }

    private IEnumerator WaitAndLoad() {
        yield return new WaitForSeconds( 3.5f );
        Application.LoadLevel( "Main" );
    }
}
