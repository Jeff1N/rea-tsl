using UnityEngine;

public class Door : MonoBehaviour {
    bool playing = false;
    float angle = 0;
    
	void Update () {
	    if (playing) {
            angle += ( Mathf.PI / Simulator.sim.ClockTickTime ) * Time.deltaTime;
            transform.rotation = Quaternion.Euler (0, -80 * Mathf.Sin(angle), 0);
        }
        if (angle > Mathf.PI) {
            playing = false;
            angle = 0;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
	}

    public void Play() {
        playing = true;
    }
}
