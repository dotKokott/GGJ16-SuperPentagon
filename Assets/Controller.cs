using UnityEngine;
using System.Collections;
using System;

public class Controller : MonoBehaviour {

    public MeshRenderer render;
    private Pentagram penta;
    private SpriteRenderer pentaRenderer;

    public static float STEP_SIZE = 0f;

    public static event EventHandler OnGoodCash;
    public static event EventHandler OnBadCash;

    void Start() {
        penta = GameObject.Find( "Symbol" ).GetComponent<Pentagram>();
        pentaRenderer = GameObject.Find( "Symbol" ).GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update() {
        float vertical = Input.GetAxis( "Vertical" );
        var t = ( vertical + 1 ) / 2f;
        if ( STEP_SIZE > 0 ) {
            var tInt = (int)( t * STEP_SIZE );
            t = tInt / STEP_SIZE;
        }

        render.material.color = HSVToRGB( t, 1, 1, true );

        if ( Input.GetButtonDown( "Cash" ) ) {
            var myCol = t;
            var penCol = pentaRenderer.material.color.ToHSV().x;

            var diff = Mathf.Abs( myCol - penCol );

            var percent = 100 - diff * 100;

            if ( percent >= 90 ) {
                OnGoodCash( this, null );
            } else {
                OnBadCash( this, null );
            }

            var sys = GameObject.Find( "DrBoom" ).GetComponentInChildren<ParticleSystem>();
            var alpha = sys.startColor.a;
            sys.startColor = pentaRenderer.material.color;
            sys.Emit( 10000 );

            GameObject.Find( "Text" ).GetComponent<TextMesh>().text = String.Format( "{0} %", (int)percent );
        }
    }

    private Color HSVToRGB( float H, float S, float V, bool hdr = true ) {
        Color white = Color.white;
        if ( (double)S == 0.0 ) {
            white.r = V;
            white.g = V;
            white.b = V;
        } else if ( (double)V == 0.0 ) {
            white.r = 0.0f;
            white.g = 0.0f;
            white.b = 0.0f;
        } else {
            white.r = 0.0f;
            white.g = 0.0f;
            white.b = 0.0f;
            float num1 = S;
            float num2 = V;
            float f = H * 6f;
            int num3 = (int)Mathf.Floor( f );
            float num4 = f - (float)num3;
            float num5 = num2 * ( 1f - num1 );
            float num6 = num2 * (float)( 1.0 - (double)num1 * (double)num4 );
            float num7 = num2 * (float)( 1.0 - (double)num1 * ( 1.0 - (double)num4 ) );
            switch ( num3 + 1 ) {
                case 0:
                    white.r = num2;
                    white.g = num5;
                    white.b = num6;
                    break;
                case 1:
                    white.r = num2;
                    white.g = num7;
                    white.b = num5;
                    break;
                case 2:
                    white.r = num6;
                    white.g = num2;
                    white.b = num5;
                    break;
                case 3:
                    white.r = num5;
                    white.g = num2;
                    white.b = num7;
                    break;
                case 4:
                    white.r = num5;
                    white.g = num6;
                    white.b = num2;
                    break;
                case 5:
                    white.r = num7;
                    white.g = num5;
                    white.b = num2;
                    break;
                case 6:
                    white.r = num2;
                    white.g = num5;
                    white.b = num6;
                    break;
                case 7:
                    white.r = num2;
                    white.g = num7;
                    white.b = num5;
                    break;
            }
            if ( !hdr ) {
                white.r = Mathf.Clamp( white.r, 0.0f, 1f );
                white.g = Mathf.Clamp( white.g, 0.0f, 1f );
                white.b = Mathf.Clamp( white.b, 0.0f, 1f );
            }
        }
        return white;
    }
}