using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum EPentaMode {
    Instant,
    Timed,
    Wait
}

public class PentaInfo {
    public EPentaMode Mode;
    public float Hue;
    public float Time;
}

public class Pentagram : MonoBehaviour {

    public Queue<PentaInfo> Qu = new Queue<PentaInfo>();
    new private MeshRenderer renderer;

    void Start() {
        renderer = GetComponent<MeshRenderer>();
        var color = ColorExtension.HSVToRGB( 1, 1, 1 );
        var hsv = color.ToHSV();

        AddInstant( 0.5f );

        //AddTimed( 0f, 1 );
        //AddWait( 3f );

        //AddTimed( 0.2f, 1 );
        //AddWait( 3f );

        //AddTimed( 0.8f, 1 );
        //AddWait( 3f );

        AddTimed( 1f, 10 );
        AddTimed( 0f, 10 );
        AddTimed( 1f, 10 );

        //AddWait( 3.5f );

        //AddTimed( 1, 2 );
        //AddWait( 3.5f );

        //AddTimed( 0.5f, 2 );

        StartCoroutine( HandleQueue() );
    }

    public void AddInstant( float hue ) {
        Qu.Enqueue( new PentaInfo() {
            Mode = EPentaMode.Instant,
            Hue = hue
        } );
    }

    public void AddTimed( float hue, float time ) {
        Qu.Enqueue( new PentaInfo() {
            Mode = EPentaMode.Timed,
            Time = time, Hue = hue
        } );
    }

    public void AddWait( float time ) {
        Qu.Enqueue( new PentaInfo() {
            Mode = EPentaMode.Wait,
            Time = time
        } );
    }
    
    private IEnumerator HandleQueue() {
        var current = Qu.Dequeue();

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
                current = Qu.Dequeue();
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

            var hsvx = hsv.x;

            if(Controller.STEP_SIZE > 0) {
                var hsvXInt = (int)( hsv.x * Controller.STEP_SIZE );
                hsvx = hsvXInt / Controller.STEP_SIZE;
            }


            renderer.material.color = ColorExtension.HSVToRGB( hsvx, 1, 1 );
            yield return null;
        }

        yield break;
    }
}
