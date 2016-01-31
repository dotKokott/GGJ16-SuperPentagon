using UnityEngine;
using System.Collections;

public class Stage : MonoBehaviour {

    public float RotationSpeed;

    //-1 stays always
    public int StageNo = -1;
    public bool Stay = false;

    void Start() {
        Controller.OnBadCash += Controller_OnBadCash;
    }

    private void Controller_OnBadCash( object sender, System.EventArgs e ) {
        iTween.RotateBy( gameObject, iTween.Hash( "z", ( RotationSpeed / 360 ) * 2, "time", 2f, "easetype", iTween.EaseType.easeOutBounce ) );
    }

    // Update is called once per frame
    void Update() {
        //transform.RotateAround( Vector3.forward, RotationSpeed * Time.deltaTime );
        transform.Rotate( 0, 0, RotationSpeed * Time.deltaTime / 2 );
    }
}
