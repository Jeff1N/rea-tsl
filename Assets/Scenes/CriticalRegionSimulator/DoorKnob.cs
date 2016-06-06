using UnityEngine;

public class DoorKnob : MonoBehaviour {
    bool playing = false;
    float angle = 0;

    void Update() {
        if (playing) {
            angle += (Mathf.PI / Simulator.sim.ClockTickTime) * Time.deltaTime;
            transform.rotation = Quaternion.Euler(-45 * Mathf.Abs(Mathf.Sin(4*angle)), 0, 0);
        }
        if (angle > Mathf.PI) {
            playing = false;
            angle = 0;
        }
    }

    public void Play() {
        playing = true;
    }
}