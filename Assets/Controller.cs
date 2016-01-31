using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class Controller : MonoBehaviour {

    public SpriteRenderer render;
    private Pentagram penta;
    private SpriteRenderer pentaRenderer;
    public SpriteRenderer objRenderer;

    public static float STEP_SIZE = 0f;

    public static event EventHandler OnGoodCash;
    public static event EventHandler OnBadCash;
    public static event EventHandler OnDeath;

    public Stage[] StageObjects;
    public int CurrentStage = 1;
    public static int Score = 0;

    public GameObject help1;
    public GameObject help2;

    private bool first = true;

    public static void Die() {
        OnBadCash( null, null );
    }

    void Start() {
        Score = 0;
        penta = GameObject.Find( "Symbol" ).GetComponent<Pentagram>();
        pentaRenderer = GameObject.Find( "Symbol" ).GetComponent<SpriteRenderer>();

        OnBadCash += Controller_OnBadCash;
    }

    private void Controller_OnBadCash( object sender, EventArgs e ) {
        var musi = Camera.main.GetComponent<MusicSwitcher>();

        musi.PlayLaugh();
        musi.PlayNegative();

        CurrentStage++;

        if ( CurrentStage == 7 ) {
            OnDeath( this, null );
        }

        if ( CurrentStage > 6 ) return;

        foreach ( var obj in StageObjects ) {
            if ( obj.StageNo == CurrentStage ) {
                obj.gameObject.SetActive( true );
            } else {
                if ( !obj.Stay ) obj.gameObject.SetActive( false );
            }
        }
    }

    // Update is called once per frame
    void Update() {
        if ( Input.GetKeyDown( KeyCode.P ) ) {
            Controller_OnBadCash( this, EventArgs.Empty );
        }

        float vertical = Input.GetAxis( "Vertical" );
        var t = ( vertical + 1 ) / 2f;
        if ( STEP_SIZE > 0 ) {
            var tInt = (int)( t * STEP_SIZE );
            t = tInt / STEP_SIZE;
        }

        if ( first ) {
            var myCol = t;
            var penCol = pentaRenderer.material.color.ToHSV().x;

            if ( myCol > ( penCol - 0.1f ) && myCol < ( penCol + 0.1f ) ) {
                help1.SetActive( true );
                help2.SetActive( true );
            } else {
                help2.SetActive( false );
                help1.SetActive( false );
            }
        }


        render.material.color = HSVToRGB( t, 1, 1, true );
        objRenderer.material.color = HSVToRGB( t, 1, 1f, true );

        foreach ( var obj in StageObjects ) {
            obj.GetComponent<SpriteRenderer>().material.color = pentaRenderer.material.color;
        }

        if ( Input.GetButtonDown( "Cash" ) ) {
            var myCol = t;
            var penCol = pentaRenderer.material.color.ToHSV().x;

            if ( penCol < 0.1f && myCol > 0.9f ) {
                penCol = 1 - penCol;
            } else if ( penCol > 0.9f && myCol < 0.1f ) {
                penCol = 1 - penCol;
            }

            var diff = Mathf.Abs( myCol - penCol );

            var percent = 100 - diff * 100;


            var musi = Camera.main.GetComponent<MusicSwitcher>();

            if ( percent >= 90 ) {
                OnGoodCash( this, null );

                var points = 1000 * ( percent / 100f ) * ( 1 - ( Pentagram.Candles / 12f ) );
                points = Mathf.Max( points, 0 );
                Score += Mathf.FloorToInt( points );

                GameObject.Find( "ScoreText" ).GetComponent<Text>().text = "SCORE: " + Score;

                var sys = GameObject.Find( "DrBoom" ).GetComponentInChildren<ParticleSystem>();
                var alpha = sys.startColor.a;
                sys.startColor = pentaRenderer.material.color;
                sys.Emit( 10000 );

                musi.PlayBoom();
                musi.PlayAngry();
            } else {


                OnBadCash( this, null );
            }

            if ( first ) {
                first = false;
                help1.SetActive( false );
                help2.SetActive( false );
            }

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