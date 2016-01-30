using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HueHelper : MonoBehaviour {

    public GameObject Bar;
    public bool IgnoreY;
    public Material BarMat;

    private float offset = 0;
    private Vector3 start;

	void Start () {
        var ren = Bar.GetComponent<SpriteRenderer>();
        offset = ren.sprite.textureRect.width / ren.sprite.pixelsPerUnit / 2 * Bar.transform.localScale.x;
        start = Bar.transform.position;
        if ( !IgnoreY ) {
            //start.y += 0.75f;
        } else {
            start = transform.position;
            //start.x += transform.position.x;
            var c = BarMat.color;
            c.a = 0;
            BarMat.color = c;

            Controller.OnBadCash += Controller_OnBadCash;
            Controller.OnGoodCash += Controller_OnGoodCash;
        }

    }

    private void Controller_OnGoodCash( object sender, System.EventArgs e ) {
        var c = BarMat.color;
        c.a += 0.25f;
        c.a = Mathf.Min( 1, c.a );
        BarMat.color = c;
    }

    private void Controller_OnBadCash( object sender, System.EventArgs e ) {
        var c = BarMat.color;
        c.a -= 0.25f;
        c.a = Mathf.Max( 0, c.a );
        BarMat.color = c;
    }

    void Update () {
        var vertical = Input.GetAxis( "Vertical" );
        var pos = start;
        pos.x += offset * vertical;
        transform.position = pos;
	}
}
