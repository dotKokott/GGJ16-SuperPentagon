using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using System;

public class HighscoreHandler : MonoBehaviour {

    private Text txt;

    void Start() {
        txt = GetComponent<Text>();

        var newscore = PlayerPrefs.GetInt( "newscore", -1 );
        var highscores = PlayerPrefs.GetString( "highscores", "empty" );

        PlayerPrefs.DeleteKey( "newscore" );
        PlayerPrefs.Save();
        
        var scores = new List<int>();

        if ( highscores == "empty" ) {
            for ( int i = 0; i < 9; i++ ) {
                scores.Add( 0 );
            }
        } else {
            scores.Clear();

            var splits = highscores.Split( new[] { ';' }, StringSplitOptions.RemoveEmptyEntries );
            foreach ( var item in splits ) {
                scores.Add( int.Parse( item ) );
            }
        }

        if ( newscore != -1 ) {
            scores.Add( newscore );
        }

        scores = scores.OrderByDescending( p => p ).ToList();

        while ( scores.Count > 9 ) {
            scores.RemoveAt( scores.Count - 1 );
        }

        txt.text = "";
        for ( int i = 0; i < scores.Count; i++ ) {
            var span = TimeSpan.FromSeconds( scores[i] );
            txt.text += string.Format( "{0}. {1}:{2}\n", i + 1, span.Minutes, span.Seconds.ToString( "D2" ) );
        }

        var nh = "";
        foreach ( var item in scores ) {
            nh += item + ";";
        }

        PlayerPrefs.SetString( "highscores", nh );
        PlayerPrefs.Save();
    }
}
