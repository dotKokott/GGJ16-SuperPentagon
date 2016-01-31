using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class DeathHandler : MonoBehaviour {

    public GameObject Face;
    private DateTime start;

    void Start() {
        Controller.OnDeath += Controller_OnDeath;
        start = DateTime.Now;
    }

    private void Controller_OnDeath( object sender, System.EventArgs e ) {
        Camera.main.GetComponent<MusicSwitcher>().PlayDemonRelease();
        iTween.ShakePosition( Camera.main.gameObject, new Vector3( 0.25f, 0.25f, 0.25f ) * 1.5f, 2.5f );
        Face.SetActive( true );
        StartCoroutine( WaitAndZoom() );
    }

    private IEnumerator WaitAndZoom() {
        yield return new WaitForSeconds( 0.9f );
        while ( Camera.main.orthographicSize > 0 ) {
            Camera.main.orthographicSize -= Time.deltaTime * 5;
            yield return null;
        }

        var end = DateTime.Now;
        var span = TimeSpan.FromTicks( end.Ticks - start.Ticks );

        PlayerPrefs.SetInt( "newscore", (int)span.TotalSeconds );
        PlayerPrefs.Save();

        Application.LoadLevel( "Highscore" );
    }
}
