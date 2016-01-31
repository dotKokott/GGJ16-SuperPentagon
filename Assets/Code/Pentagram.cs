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

    public ParticleSystemRenderer[] Cubes;
    private List<ParticleSystem> systems = new List<ParticleSystem>();

    [HideInInspector]
    public List<PentaInfo> Qu = new List<PentaInfo>();
    new public SpriteRenderer renderer;
    private bool stopCurrent = false;

    private DateTime start;
    public static int Candles = 0;

    void Start() {
        start = DateTime.Now;
        renderer = GetComponent<SpriteRenderer>();

        foreach ( var cube in Cubes ) {

            var sys = cube.GetComponent<ParticleSystem>();
            sys.startSize = 1;
            sys.Stop();
            systems.Add( sys );
        }

        AddTimed( UnityEngine.Random.Range( 0f, 0.99f ), 8 );
        AddTimed( UnityEngine.Random.Range( 0f, 0.99f ), 8 );
        AddTimed( UnityEngine.Random.Range( 0f, 0.99f ), 7 );
        AddTimed( UnityEngine.Random.Range( 0f, 0.99f ), 7 );
        AddTimed( UnityEngine.Random.Range( 0f, 0.99f ), 6 );
        AddTimed( UnityEngine.Random.Range( 0f, 0.99f ), 6 );

        //var color = ColorExtension.HSVToRGB( 1, 1, 1 );
        //var hsv = color.ToHSV();

        //Debug.Log( ColorExtension.HSVToRGB( 1, 1, 1 ) );

        //Debug.Log( ColorExtension.HSVToRGB( 1, 0, 1 ) );

        //AddInstant( 0.5f );
        //AddTimed( 0.25f, 9999 );

        //AddTimed( 0.2f, 4 );
        //AddWait( 1 );
        //AddTimed( 0.9f, 4 );
        //AddWait( 1 );
        //AddTimed( 0.3f, 4 );
        //AddWait( 1 );
        //AddTimed( 0.7f, 4 );
        //AddWait( 1 );
        //AddTimed( 0.2f, 4 );
        //AddWait( 1 );
        //AddTimed( 0.5f, 4 );

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
            Time = time,
            Hue = hue
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

            Candles = 0;
            while ( currentIt.MoveNext() ) {
                if ( stopCurrent ) {
                    stopCurrent = false;

                    for ( var i = 0; i < Cubes.Length; i++ ) {
                        systems[i].Stop();
                    }

                    break;
                }

                yield return currentIt.Current;
            }
        }

        start = DateTime.Now;

        while ( true ) {
            var it = Timed( UnityEngine.Random.Range( 0f, 1f ), GetDuration() );

            Candles = 0;
            while ( it.MoveNext() ) {
                if ( stopCurrent ) {
                    stopCurrent = false;

                    for ( var i = 0; i < Cubes.Length; i++ ) {
                        systems[i].Stop();
                    }

                    break;
                }

                yield return it.Current;
            }
        }

        yield break;
    }

    private float GetDuration() {
        var stamp = TimeSpan.FromTicks( DateTime.Now.Ticks - start.Ticks );
        var seconds = stamp.TotalSeconds;

        if ( seconds < 15 ) {
            return 5;
        } else if ( seconds < 30 ) {
            return 4;
        } else if ( seconds < 60 ) {
            return 3;
        } else {
            return 2;
        }
    }

    private IEnumerator Instant( float hue ) {
        hue = Mathf.Min( hue, 0.99f );

        renderer.material.color = ColorExtension.HSVToRGB( hue, 1, 1 );
        yield break;
    }

    private IEnumerator Timed( float hue, float duration ) {
        hue = Mathf.Min( 0.99f, hue );

        iTween.PunchScale( GameObject.Find( "StageCpmtaomer" ), new Vector3( 0.25f, 0.25f, 0.25f ), 1 );

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



            var cube = (int)Mathf.Floor( ( Cubes.Length / duration ) * time );

            var color = ColorExtension.HSVToRGB( hue, 1, 1 );

            if ( !systems[cube].isPlaying ) {
                systems[cube].Play();
                Candles++;
            }

            systems[cube].startColor = color;
            Cubes[cube].material.SetColor( "_TintColor", color );

            yield return null;
        }

        hsv.x = hue;

        while ( time < duration ) {
            time += Time.deltaTime;
            hsv.y += Time.deltaTime * timefactor * 2;
            //renderer.material.color = ColorExtension.HSVToRGB( hsv.x, hsv.y, 1 );

            if ( time < duration ) {
                var cube = (int)Mathf.Floor( ( (float)Cubes.Length / duration ) * time );

                var color = ColorExtension.HSVToRGB( hue, 1, 1 );

                if ( !systems[cube].isPlaying ) {
                    systems[cube].Play();
                    Candles++;
                }

                systems[cube].startColor = color;

                Cubes[cube].material.SetColor( "_TintColor", color );
            }


            yield return null;
        }

        for ( var i = 0; i < Cubes.Length; i++ ) {
            systems[i].Stop();
        }

        Controller.Die();

        yield break;
    }

    private IEnumerator Wait( float duration ) {
        yield return new WaitForSeconds( duration );
    }

    void Update() {
        if ( Input.GetButtonDown( "Cash" ) ) {
            iTween.ShakePosition( GameObject.Find( "Main Camera" ), new Vector3( 0.2f, 0.2f, 0.2f ), 0.4f );
            //iTween.PunchScale( GameObject.Find( "StageCpmtaomer" ), new Vector3( 0.25f, 0.25f, 0.25f ), 1 );
            Skip();
        }
    }
}
