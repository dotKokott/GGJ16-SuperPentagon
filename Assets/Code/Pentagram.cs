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

    public MeshRenderer[] Cubes;
    [HideInInspector]
    public List<PentaInfo> Qu = new List<PentaInfo>();
    new private SpriteRenderer renderer;
    private bool stopCurrent = false;


    void Start() {
        renderer = GetComponent<SpriteRenderer>();

        //var color = ColorExtension.HSVToRGB( 1, 1, 1 );
        //var hsv = color.ToHSV();

        //Debug.Log( ColorExtension.HSVToRGB( 1, 1, 1 ) );

        //Debug.Log( ColorExtension.HSVToRGB( 1, 0, 1 ) );

        AddInstant( 0.5f );
        AddTimed( 0, 2 );
        AddTimed( 0.2f, 2 );
        AddTimed( 0.9f, 2 );
        AddTimed( 0.3f, 2 );
        AddTimed( 0.7f, 2 );
        AddTimed( 0.2f, 2 );
        AddTimed( 0.5f, 2 );
        //AddTimed( 0.5f, 1 );
        //AddTimed( 1, 1 );
        //AddInstant( 0.5f );

        //AddTimed( 0f, 1 );
        //AddWait( 3f );

        //AddTimed( 0.2f, 1 );
        //AddWait( 3f );

        //AddTimed( 0.8f, 1 );
        //AddWait( 3f );

        //AddTimed( 1f, 10 );
        //AddTimed( 0f, 10 );
        //AddTimed( 1f, 10 );

        //AddWait( 3.5f );

        //AddTimed( 1, 2 );
        //AddWait( 3.5f );

        //AddTimed( 0.5f, 2 );

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

    public void Skip() {
        stopCurrent = true;
    }

    private IEnumerator HandleQueue() {
        foreach ( var item in Qu ) {
            IEnumerator currentIt = null;

            switch ( item.Mode ) {
                case EPentaMode.Instant:
                    currentIt = Instant( item.Hue );
                    break;
                case EPentaMode.Timed:
                    currentIt = Timed( item.Hue, item.Time );
                    break;
                case EPentaMode.Wait:
                    currentIt = Wait( item.Time );
                    break;
            }

            while ( currentIt.MoveNext() ) {
                if ( stopCurrent ) {
                    stopCurrent = false;

                    foreach ( var cube in Cubes ) {
                        cube.material.color = Color.black;
                    }

                    break;
                }

                yield return currentIt.Current;
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

        var halfDuration = duration / 2;

        renderer.material.color = ColorExtension.HSVToRGB( hue, 1, 1 );

        while ( time < halfDuration ) {
            time += Time.deltaTime;
            hsv.y -= Time.deltaTime * timefactor * 2;
            //renderer.material.color = ColorExtension.HSVToRGB( hsv.x, hsv.y, 1 );

            var cube = (int)Mathf.Floor(( Cubes.Length / duration ) * time);
            Cubes[cube].material.color = ColorExtension.HSVToRGB( hue, 1, 1 );

            yield return null;
        }

        hsv.x = hue;

        while ( time < duration ) {
            time += Time.deltaTime;
            hsv.y += Time.deltaTime * timefactor * 2;
            //renderer.material.color = ColorExtension.HSVToRGB( hsv.x, hsv.y, 1 );

            if(time < duration ) {
                var cube = (int)Mathf.Floor( ( (float)Cubes.Length / duration ) * time );
                Cubes[cube].material.color = ColorExtension.HSVToRGB( hue, 1, 1 );
            }


            yield return null;
        }

        foreach(var cube in Cubes ) {
            cube.material.color = Color.black;
        }

        yield break;
    }

    private IEnumerator Wait( float duration ) {
        yield return new WaitForSeconds( duration );
    }

    void Update() {
        if ( Input.GetButtonDown( "Cash" ) ) {
            
            Skip();

        }
    }
}
