using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MusicSwitcher : MonoBehaviour {

    public AudioSource source;
    public AudioClip[] clips;
    int index = -1;

    void Start() {
        source = GetComponent<AudioSource>();
        Controller.OnBadCash += Controller_OnBadCash;
        Controller_OnBadCash( null, null );
    }

    private void Controller_OnBadCash( object sender, System.EventArgs e ) {
        index++;
        if ( index < clips.Length ) {
            source.clip = clips[index];
            source.Play();
        }
    }
}
