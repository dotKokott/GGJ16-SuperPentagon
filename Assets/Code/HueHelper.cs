using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HueHelper : MonoBehaviour {

    public GameObject Bar;

    private float offset = 0;
    private Vector3 start;

	void Start () {
        var ren = Bar.GetComponent<SpriteRenderer>();
        offset = ren.sprite.textureRect.width / ren.sprite.pixelsPerUnit / 2 * Bar.transform.localScale.x;
        start = Bar.transform.position;
        start.y += 0.75f;

    }

	void Update () {
        var vertical = Input.GetAxis( "Vertical" );
        var pos = start;
        pos.x += offset * vertical;
        transform.position = pos;

	}
}
