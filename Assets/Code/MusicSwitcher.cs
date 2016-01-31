using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MusicSwitcher : MonoBehaviour {

    public AudioSource source;
    public AudioClip[] clips;
    int index = -1;

    public AudioSource SFXSource;

    public AudioClip Boom;
    public AudioClip DemonRelease;

    public AudioClip[] Angry;
    public AudioClip[] Laugh;

    public AudioClip Negative;

    public void PlayBoom() {
        SFXSource.PlayOneShot( Boom );
    }

    public void PlayAngry() {
        SFXSource.PlayOneShot( Angry[Random.Range( 0, Angry.Length )] );
    }

    public void PlayLaugh() {
        SFXSource.PlayOneShot( Laugh[Random.Range( 0, Laugh.Length )] );
    }

    public void PlayDemonRelease() {
        source.PlayOneShot( DemonRelease );
    }

    public void PlayNegative() {
        SFXSource.PlayOneShot( Negative, 1 );
    }

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
