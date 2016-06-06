using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Simulator : MonoBehaviour {

    public static Simulator sim;

    [HideInInspector] public int lockVar;
    [HideInInspector] public int toilet;
    [HideInInspector] public int currentProcess;

    public InputField lockInput;
    public InputField toiletInput;
    public InputField processInput;
    public InputField opsInput;
    public InputField clockTickInput;

    public Process[] process;
    public Image[] processPanel;

    public Image codePanel;

    [HideInInspector] public Text[] codeLine;

    //Play Button Vars
    [HideInInspector] public float time;
    [HideInInspector] public bool playing = false;
    [HideInInspector] public int opsCount = 0;

    public int OpsPerProcess = 4;
    public float ClockTickTime = 2f;
    public Button PlayPauseButton;
    public Sprite PlaySprite;
    public Sprite PauseSprite;

    public int ProcessCount = 4;

    public Transform outsideBathroom;
    public Transform insideBathroom;
    public Transform sink;
    public Transform outsideToilet1;
    public Transform outsideToilet2;
    public Transform insideToilet;

    public AudioClip fart;
    public AudioClip flush;
    public AudioClip right;
    public AudioClip wrong;
    public AudioClip zipper;
    public AudioClip tryOpen;
    public AudioClip unlock;

    public Text doorText;
    public GameObject toiletDoor;
    public Door doorAnim;
    public DoorKnob knobAnim;

    public Stack undoStack;

    void Awake () {
        if (sim == null) sim = this;
        else Destroy(gameObject);

        lockVar = 0;
        toilet = 0;
        currentProcess = 0;
        codeLine = codePanel.GetComponentsInChildren<Text>();

        undoStack = new Stack();
	}

    public void Update() {
        if (process[0].state >= 15 && process[1].state >= 15 && process[2].state >= 15 && process[3].state >= 15) {
            PauseSim();
        }

        //Keep the simulator running
        if (playing) {
            if (Time.time - time >= ClockTickTime) {
                PlayNext();
                time = Time.time;

                opsCount++;
                if (opsCount >= OpsPerProcess) {
                    opsCount = 0;

                    if (process[currentProcess].state < 15) {
                        if (process[currentProcess].state >= 6 && process[currentProcess].state <= 8)
                            codeLine[process[currentProcess].state].color = new Color(0.625f, 0.125f, 0.125f);
                        else
                            codeLine[process[currentProcess].state].color = Color.white;
                    }
                    processPanel[currentProcess].color = Color.white;

                    int count = 0;
                    do {
                        currentProcess++;
                        if (currentProcess >= 4) {
                            currentProcess = 0;
                        }
                        count++;
                    }
                    while (process[currentProcess].state >= 15 && count <= 7);

                    processInput.text = currentProcess.ToString();

                    if (process[currentProcess].state < 15) {
                        if (process[currentProcess].state >= 6 && process[currentProcess].state <= 8)
                            codeLine[process[currentProcess].state].color = new Color(1f, 0.5f, 0.5f);
                        else
                            codeLine[process[currentProcess].state].color = new Color(1f, 1f, 0f);
                    }
                    processPanel[currentProcess].color = new Color(1f, 1f, 0.5f);
                }
            }
        }
    }

    public void UpdateLock() {
        //Stack the simulator current state so it can be undone
        UndoState state = new UndoState();
        state.Snapshot();
        undoStack.Push(state);

        if (!string.IsNullOrEmpty(lockInput.text)) {
            lockVar = int.Parse(lockInput.text);
            doorText.text = "Lock\n" + lockVar;

            if (lockVar >= 1) toiletDoor.GetComponentInChildren<Renderer>().material.color = new Color(1f, 0.5f, 0.5f, 0.5f);
            else toiletDoor.GetComponentInChildren<Renderer>().material.color = new Color(0.5f, 1f, 0.5f, 0.5f);
        }
        else {
            lockInput.text = "0";
            doorText.text = "Lock\n0";
            lockVar = 0;
            toiletDoor.GetComponentInChildren<Renderer>().material.color = new Color(0.5f, 1f, 0.5f, 0.5f);
            
        }
    }

    public void UpdateToilet() {
        //Stack the simulator current state so it can be undone
        UndoState state = new UndoState();
        state.Snapshot();
        undoStack.Push(state);

        if (!string.IsNullOrEmpty(toiletInput.text)) {
            toilet = int.Parse(toiletInput.text);
        }
        else {
            toiletInput.text = "0";
            toilet = 0;
        }
    }

    public void UpdateOps() {
        if (!string.IsNullOrEmpty(opsInput.text)) {
            OpsPerProcess = int.Parse(opsInput.text);

            if (OpsPerProcess < 1) {
                OpsPerProcess = 1;
                opsInput.text = "1";
            }
            else if (OpsPerProcess > 15) {
                OpsPerProcess = 15;
                opsInput.text = "15";
            }
        }
        else {
            OpsPerProcess = 1;
            opsInput.text = "1";
        }
    }

    public void UpdateClock() {
        if (!string.IsNullOrEmpty(clockTickInput.text)) {
            if (float.Parse(clockTickInput.text) >= 0.5) {
                ClockTickTime = float.Parse(clockTickInput.text);
            }
            else {
                ClockTickTime = 0.5f;
                clockTickInput.text = "0.5";
            }
        }
        else {
            ClockTickTime = 0.5f;
            clockTickInput.text = "0.5";
        }
    }

    public void UpdateProcess() {
        //Stack the simulator current state so it can be undone
        UndoState state = new UndoState();
        state.Snapshot();
        undoStack.Push(state);

        if (!string.IsNullOrEmpty(toiletInput.text)) {
            if (process[currentProcess].state < 15) {
                if (process[currentProcess].state >= 6 && process[currentProcess].state <= 8)
                    codeLine[process[currentProcess].state].color = new Color(0.625f, 0.125f, 0.125f);
                else
                    codeLine[process[currentProcess].state].color = Color.white;
            }
            processPanel[currentProcess].color = Color.white;

            currentProcess = int.Parse(processInput.text);
            if (currentProcess < 0) currentProcess = 0;
            else if (currentProcess > 3) currentProcess = 3;
            processInput.text = currentProcess.ToString();

            if (process[currentProcess].state < 15) {
                if (process[currentProcess].state >= 6 && process[currentProcess].state <= 8)
                    codeLine[process[currentProcess].state].color = new Color(1f, 0.5f, 0.5f);
                else
                    codeLine[process[currentProcess].state].color = new Color(1f, 1f, 0f);
            }
            processPanel[currentProcess].color = new Color(1f, 1f, 0.5f);
        }
        else {
            if (process[currentProcess].state < 15) {
                if (process[currentProcess].state >= 6 && process[currentProcess].state <= 8)
                    codeLine[process[currentProcess].state].color = new Color(0.625f, 0.125f, 0.125f);
                else
                    codeLine[process[currentProcess].state].color = Color.white;
            }
            processPanel[currentProcess].color = Color.white;

            currentProcess = 0;

            if (process[currentProcess].state < 15) {
                if (process[currentProcess].state >= 6 && process[currentProcess].state <= 8)
                    codeLine[process[currentProcess].state].color = new Color(1f, 0.5f, 0.5f);
                else
                    codeLine[process[currentProcess].state].color = new Color(1f, 1f, 0f);
            }
            processPanel[currentProcess].color = new Color(1f, 1f, 0.5f);
        }
    }

    //Play current process' next 
    public void PlayNext() {
        if (process[currentProcess].state < 15) {
            //Stack the simulator current state so it can be undone
            UndoState state = new UndoState();
            state.Snapshot();
            undoStack.Push(state);

            if (process[currentProcess].state < 15) {
                if (process[currentProcess].state >= 6 && process[currentProcess].state <= 8)
                    codeLine[process[currentProcess].state].color = new Color(0.625f, 0.125f, 0.125f);
                else
                    codeLine[process[currentProcess].state].color = Color.white;
            }
            process[currentProcess].Play(process[currentProcess].state);

            if (process[currentProcess].state < 15) {
                if (process[currentProcess].state >= 6 && process[currentProcess].state <= 8)
                    codeLine[process[currentProcess].state].color = new Color(1f, 0.5f, 0.5f);
                else
                    codeLine[process[currentProcess].state].color = new Color(1f, 1f, 0f);
            }
        }
    }

    public void PlayPause() {
        if (playing) PauseSim();
        else PlaySim();
    }

    public void PlaySim() {
        time = Time.time;
        playing = true;
        PlayPauseButton.GetComponentsInChildren<Image>()[1].sprite = PauseSprite;
    }

    public void PauseSim() {
        playing = false;
        PlayPauseButton.GetComponentsInChildren<Image>()[1].sprite = PlaySprite;
    }

    public void Previous() {
        playing = false;
        PlayPauseButton.GetComponentsInChildren<Image>()[1].sprite = PlaySprite;
                
        if (undoStack.Count > 0) {
            TextBox.textBox.Clear();
            UndoState state = (UndoState)undoStack.Pop();

            //Unpaint
            if (process[currentProcess].state < 15) {
                if (process[currentProcess].state >= 6 && process[currentProcess].state <= 8)
                    codeLine[process[currentProcess].state].color = new Color(0.625f, 0.125f, 0.125f);
                else codeLine[process[currentProcess].state].color = Color.white;
            }
            processPanel[currentProcess].color = Color.white;
                        
            for (int i = 0; i < 4; i++) {
                process[i].nextPos = state.nextPos[i];

                process[i].state = state.pc[i];
                process[i].cmp = state.cmp[i];

                lockVar = state.lockVar;
                toilet = state.toilet;

                currentProcess = state.currentProcess;

                for (int j = 0; j < 4; j++) {
                    process[i].r[j] = state.r[i, j];
                }

                process[i].pcInput.text = state.pc[i].ToString();

                if (process[i].cmp) process[i].cmpInput.text = "1";
                else process[i].cmpInput.text = "0";

                lockInput.text = state.lockVar.ToString();
                toiletInput.text = state.toilet.ToString();

                processInput.text = state.currentProcess.ToString();

                for (int j = 0; j < 4; j++) {
                    process[i].rInput[j].text = state.r[i, j].ToString();
                }
                
                if (process[i].r[0] == 0) process[i].rInput[0].textComponent.color = Color.green;
                else process[i].rInput[0].textComponent.color = Color.red;

                process[i].playSound = false;
                process[i].Animate(process[i].state - 1);
                process[i].playSound = true;
            }

            if (process[currentProcess].state < 15) {
                if (process[currentProcess].state >= 6 && process[currentProcess].state <= 8)
                    codeLine[process[currentProcess].state].color = new Color(1f, 0.5f, 0.5f);
                else
                    codeLine[process[currentProcess].state].color = new Color(1f, 1f, 0f);
            }
            processPanel[currentProcess].color = new Color(1f, 1f, 0.5f);

            if (lockVar >= 1) toiletDoor.GetComponentInChildren<Renderer>().material.color = new Color(1f, 0.5f, 0.5f, 0.5f);
            else toiletDoor.GetComponentInChildren<Renderer>().material.color = new Color(0.5f, 1f, 0.5f, 0.5f);

            doorText.text = "Lock\n" + lockVar;
        }
    }
	
}

public class UndoState {
    public Vector2[] nextPos;

    public int[] pc;
    public bool[] cmp;
    public int[,] r;

    public int lockVar;
    public int toilet;

    public int currentProcess;

    public UndoState() {
        nextPos = new Vector2[4];

        pc = new int[4];
        cmp = new bool[4];
        r = new int[4,4];
    }

    public void Snapshot() {
        for (int i = 0; i < 4; i++) {
            nextPos[i] = Simulator.sim.process[i].nextPos;

            pc[i] = Simulator.sim.process[i].state;
            cmp[i] = Simulator.sim.process[i].cmp;
            lockVar = Simulator.sim.lockVar;
            toilet = Simulator.sim.toilet;

            currentProcess = Simulator.sim.currentProcess;

            for (int j = 0; j < 4; j++) {
                r[i, j] = Simulator.sim.process[i].r[j];
            }
        }
    }
}