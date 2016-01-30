using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class ExtraEffects : MonoBehaviour {
    private Fisheye fish;
	// Use this for initialization
	void Start () {
        fish = GetComponent<Fisheye>();
	}
	
    public void Wobble() {
        iTween.ValueTo( gameObject, iTween.Hash(
            "from", 0,
            "to", 0.2f,
            "time", 0.8f,
            "onupdate", "updateWobble",
            "oncomplete", "WobbleBack",
            "easetype", iTween.EaseType.easeOutElastic ) );
    }

    public void WobbleBack() {
        iTween.ValueTo( gameObject, iTween.Hash(
            "from", 0.2f,
            "to", 0,
            "time", 0.3f,
            "onupdate", "updateWobble",
            "easetype", iTween.EaseType.linear ) );
    }

    void updateWobble(float value) {
        fish.strengthX = value;
        fish.strengthY = value;
    }

	// Update is called once per frame
	void Update () {

	}
}
