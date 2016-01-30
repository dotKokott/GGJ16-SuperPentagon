using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public enum EPentaMode {
    Instant,
    Timed,
    Wait
}

[Serializable]
public class PentaInfo {
    public EPentaMode Mode;
    public float Hue;
    public float Time;
}

public class Pentagram : MonoBehaviour {

    [HideInInspector]
    public List<PentaInfo> Qu = new List<PentaInfo>();
    new private MeshRenderer renderer;

    void Start() {
        renderer = GetComponent<MeshRenderer>();

        StartCoroutine( HandleQueue() );
    }

    public void AddInstant( float hue ) {
        Qu.Add( new PentaInfo() {
            Mode = EPentaMode.Instant,
            Hue = hue
        } );
    }

    public void AddTimed( float hue, float time ) {
        Qu.Add( new PentaInfo() {
            Mode = EPentaMode.Timed,
            Time = time, Hue = hue
        } );
    }

    public void AddWait( float time ) {
        Qu.Add( new PentaInfo() {
            Mode = EPentaMode.Wait,
            Time = time
        } );
    }

    private IEnumerator HandleQueue() {
        var current = Qu.First();
        Qu.RemoveAt( 0 );

        while ( current != null ) {
            switch ( current.Mode ) {
                case EPentaMode.Instant:
                    yield return StartCoroutine( Instant( current.Hue ) );
                    break;
                case EPentaMode.Timed:
                    yield return StartCoroutine( Timed( current.Hue, current.Time ) );
                    break;
                case EPentaMode.Wait:
                    yield return new WaitForSeconds( current.Time );
                    break;
            }

            if ( Qu.Count > 0 ) {
                current = Qu.First();
                Qu.RemoveAt( 0 );
            } else {
                current = null;
            }
        }

        yield break;
    }

    private IEnumerator Instant( float hue ) {
        hue = Mathf.Min( hue, 0.99f );

        renderer.material.color = ColorExtension.HSVToRGB( hue, 1, 1 );
        yield break;
    }

    private IEnumerator Timed( float hue, float duration ) {
        hue = Mathf.Min( 0.99f, hue );

        var hsv = renderer.material.color.ToHSV();
        var diff = hue - hsv.x;
        var timefactor = 1f / duration;
        var time = 0f;

        while ( time < duration ) {
            time += Time.deltaTime;
            hsv.x += diff * ( Time.deltaTime * timefactor );
            renderer.material.color = ColorExtension.HSVToRGB( hsv.x, 1, 1 );
            yield return null;
        }

        yield break;
    }
}
