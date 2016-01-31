using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using System;
using System.Text;
using System.IO;

public class HighscoreHandler : MonoBehaviour {

    private Text txt;
    public Text otxt;
    public GameObject label;
    public Text scoretxt;
    private bool settingName = false;

    private int insertAtThisIndex = -1;
    private int newscore = 0;

    private int changeIndex = -1;
    private int charIndex = 0;
    private List<string> names = new List<string>();
    private List<int> scores = new List<int>();

    private int letterIndex = 0;
    private char[] letters = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

    void Start() {
        txt = GetComponent<Text>();

        names = new List<string>();
        scores = new List<int>();

        if ( File.Exists( Application.streamingAssetsPath + "/highscores.txt" ) ) {
            var lines = File.ReadAllLines( Application.streamingAssetsPath + "/highscores.txt" );
            foreach ( var item in lines ) {
                var split = item.Split( ',' );
                names.Add( split[0] );
                scores.Add( int.Parse( split[1] ) );
            }
        } else {
            for ( int i = 0; i < 7; i++ ) {
                names.Add( "AAA" );
                scores.Add( 0 );
            }
        }

        newscore = PlayerPrefs.GetInt( "newscore", -1 );
        PlayerPrefs.DeleteKey( "newscore" );
        PlayerPrefs.Save();

        if ( newscore != -1 ) {
            for ( int i = 0; i < scores.Count; i++ ) {
                if ( newscore > scores[i] ) {
                    insertAtThisIndex = i;
                    txt.enabled = false;

                    otxt.gameObject.SetActive( true );
                    scoretxt.gameObject.SetActive( true );
                    scoretxt.text = string.Format( "SCORE: {0}", newscore );
                    label.SetActive( true );
                    settingName = true;
                    break;
                }
            }
        }

        WriteHS();
    }

    void Update() {
        if ( settingName ) {
            var builder = new StringBuilder( otxt.text );
            builder[charIndex] = letters[letterIndex];
            otxt.text = builder.ToString();

            if ( Input.GetKeyUp( KeyCode.Return ) ) {
                charIndex++;
                if ( charIndex >= 3 ) {
                    names.Insert( insertAtThisIndex, otxt.text );
                    scores.Insert( insertAtThisIndex, newscore );

                    while ( names.Count > 7 ) {
                        names.RemoveAt( names.Count - 1 );
                    }

                    while ( scores.Count > 7 ) {
                        scores.RemoveAt( scores.Count - 1 );
                    }

                    SaveHS();
                    txt.enabled = true;
                    otxt.gameObject.SetActive( false );
                    scoretxt.gameObject.SetActive( false );
                    label.SetActive( false );
                    settingName = false;

                    WriteHS();
                }
            } else if ( Input.GetKeyUp( KeyCode.UpArrow ) ) {
                letterIndex++;
                if ( letterIndex >= 26 ) {
                    letterIndex = 0;
                }
            } else if ( Input.GetKeyUp( KeyCode.DownArrow ) ) {
                letterIndex--;
                if ( letterIndex < 0 ) {
                    letterIndex = 25;
                }
            }
        } else {
            if ( Input.GetButtonUp( "Cash" ) ) {
                Application.LoadLevel( "Menu" );
            }
        }
    }

    private void SaveHS() {
        if ( !Directory.Exists( Application.streamingAssetsPath ) ) {
            Directory.CreateDirectory( Application.streamingAssetsPath );
        }

        var temp = new string[names.Count];
        for ( int i = 0; i < names.Count; i++ ) {
            temp[i] = string.Format( "{0},{1}", names[i], scores[i] );
        }

        File.WriteAllLines( Application.streamingAssetsPath + "/highscores.txt", temp );
    }

    private void WriteHS() {
        txt.text = "";
        for ( int i = 0; i < names.Count; i++ ) {
            if ( i == 0 ) {
                txt.text += string.Format( "{0}.  {1} - {2}\n", i + 1, names[i], scores[i] );
            } else {
                txt.text += string.Format( "{0}. {1} - {2}\n", i + 1, names[i], scores[i] );
            }
        }
    }
}
