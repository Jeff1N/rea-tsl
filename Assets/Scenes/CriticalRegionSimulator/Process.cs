using UnityEngine;
using UnityEngine.UI;

public class Process : Script {

    public ExplainScript explain;
    public int currentProcess;      // This process' number

    public Vector2 relativePos;     
    public Vector2 nextPos;      
    private bool moving = false;

    private Vector3 add;
    private Vector3 originalPos;
    private float distance;    

    [HideInInspector] public int state;     // Program Counter
    [HideInInspector] public int[] r;       // Process' Registers

    public InputField[] rInput; // Registers InputFields
    public InputField pcInput;  // Program Counter InputField
    public InputField cmpInput;

    public bool cmp;            // Compare flag

    public SpriteRenderer thoughtBubble;
    public SpriteRenderer poop;
    public SpriteRenderer toilet;
    public SpriteRenderer brush;
    public SpriteRenderer openLock;
    public SpriteRenderer closedLock;
    public SpriteRenderer yes;
    public SpriteRenderer no;
    public SpriteRenderer question;
    public SpriteRenderer door;

    public AudioClip toiletSound;
    public bool playSound = true;

    void Start () {
        state = 0;
        r = new int[4];
        cmp = false;
	}

    void Update() {
        if (moving) {
            if ((transform.position - originalPos).sqrMagnitude >= distance) {
                moving = false;
                transform.position = new Vector3(nextPos.x + relativePos.x, transform.position.y, nextPos.y + relativePos.y);
            }
            else {
                if (((transform.position + add * (10f / Simulator.sim.ClockTickTime) * Time.deltaTime) - originalPos).sqrMagnitude <= distance)
                    transform.position += add * (10f / Simulator.sim.ClockTickTime) * Time.deltaTime;
                else
                    transform.position = new Vector3(nextPos.x + relativePos.x, transform.position.y, nextPos.y + relativePos.y);
            }
        }
    }

    public void StartMoving(Vector3 newPos) {
        moving = true;
        nextPos = new Vector2(newPos.x, newPos.z);
        originalPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        add = new Vector3((nextPos.x + relativePos.x) - transform.position.x,
                0,
                (nextPos.y + relativePos.y) - transform.position.z);
        distance = add.sqrMagnitude;
        add.Normalize();
    }

    public void UpdateReg(int register) {
        //Stack the simulator current state so it can be undone
        UndoState undoState = new UndoState();
        undoState.Snapshot();
        Simulator.sim.undoStack.Push(undoState);

        if (!string.IsNullOrEmpty(rInput[register].text)) {
            r[register] = int.Parse(rInput[register].text);

            if (register == 0) {
                if (r[0] <= 0) {
                    r[0] = 0;
                    rInput[0].text = "0";
                    rInput[0].textComponent.color = Color.green;
                }
                else {
                    r[0] = 1;
                    rInput[0].text = "1";
                    rInput[0].textComponent.color = Color.red;
                }
            }
            else {
                r[register] = 0;
                rInput[register].text = "0";
                if (register == 0) rInput[0].textComponent.color = Color.green;
            }
        }
    }

    public void UpdatePC() {
        TextBox.textBox.Clear();

        //Stack the simulator current state so it can be undone
        UndoState undoState = new UndoState();
        undoState.Snapshot();
        Simulator.sim.undoStack.Push(undoState);

        if (state >= 0 && state <= 14 && Simulator.sim.currentProcess == currentProcess) {
            if (state >= 6 && state <= 8)
                Simulator.sim.codeLine[state].color = new Color(0.625f, 0.125f, 0.125f);
            else
                Simulator.sim.codeLine[state].color = Color.white;
        }

        if (!string.IsNullOrEmpty(pcInput.text)) {
            if (int.Parse(pcInput.text) >= 0 && int.Parse(pcInput.text) <= 15) {
                state = int.Parse(pcInput.text);

                if (state < 0) {
                    state = 0;
                    pcInput.text = "0";
                }
                if (state > 15) {
                    state = 15;
                    pcInput.text = "15";
                }
            }
        }
        else {
            state = 0;
            pcInput.text = "0";
        }

        if (state >= 0 && state <= 14 && Simulator.sim.currentProcess == currentProcess) {
            if (state >= 6 && state <= 8)
                Simulator.sim.codeLine[state].color = new Color(1f, 0.5f, 0.5f);
            else
                Simulator.sim.codeLine[state].color = new Color(1f, 1f, 0f);
        }

        Animate(state - 1);
    }

    public void UpdateCmp() {
        //Stack the simulator current state so it can be undone
        UndoState undoState = new UndoState();
        undoState.Snapshot();
        Simulator.sim.undoStack.Push(undoState);

        if (!string.IsNullOrEmpty(cmpInput.text)) {
            if (int.Parse(cmpInput.text) >= 1) {
                cmp = true;
                cmpInput.text = "1";
            }
            else {
                cmp = false;
                cmpInput.text = "0";
            }
        }
        else {
            cmp = false;
            cmpInput.text = "0";
        }
    }

    void HideThoughts() {
        thoughtBubble.gameObject.SetActive(false);
        poop.gameObject.SetActive(false);
        toilet.gameObject.SetActive(false);
        brush.gameObject.SetActive(false);
        openLock.gameObject.SetActive(false);
        closedLock.gameObject.SetActive(false);
        yes.gameObject.SetActive(false);
        no.gameObject.SetActive(false);
        question.gameObject.SetActive(false);
        door.gameObject.SetActive(false);
    }

    public void Play(int currentState) {
        explain.StopExplain();

        switch (currentState) {
            case 0:     // waiting
                state++;
                pcInput.text = state.ToString();

                r[3]++;
                rInput[3].text = r[3].ToString();

                Animate(0);
                StartText("Process " + currentProcess + ": <color=#00ffffff>INC</color> R3", GetComponent<SpriteRenderer>().sprite);
                TextBox.textBox.SetClickable(false);
                break;

            case 1:     // use_toilet
                state++;
                pcInput.text = state.ToString();

                Animate(1);
                StartText("Process " + currentProcess + ": Use_Toilet", GetComponent<SpriteRenderer>().sprite);
                TextBox.textBox.SetClickable(false);
                break;

            case 2:     // enter_region:
                state++;
                pcInput.text = state.ToString();

                Animate(2);
                StartText("Process " + currentProcess + ": Enter_Region", GetComponent<SpriteRenderer>().sprite);
                TextBox.textBox.SetClickable(false);
                break;

            case 3:     // tsl r0, lock
                r[0] = Simulator.sim.lockVar;
                rInput[0].text = r[0].ToString();

                if (r[0] == 0) rInput[0].textComponent.color = Color.green;
                else rInput[0].textComponent.color = Color.red;

                Simulator.sim.lockVar = 1;
                Simulator.sim.lockInput.text = "1";

                state++;
                pcInput.text = state.ToString();

                Animate(3);
                StartText("Process " + currentProcess + ": <color=#00ffffff>TSL</color> R0, LOCK", GetComponent<SpriteRenderer>().sprite);
                TextBox.textBox.SetClickable(false);
                break;

            case 4:     // cmp r0, #0
                cmp = (r[0] == 0);
                if(cmp) cmpInput.text = "1";
                else    cmpInput.text = "0";

                state++;
                pcInput.text = state.ToString();

                Animate(4);
                StartText("Process " + currentProcess + ": <color=#00ffffff>CMP</color> R0, #0", GetComponent<SpriteRenderer>().sprite);
                TextBox.textBox.SetClickable(false);
                break;

            case 5:     // jne enter_region
                Animate(5);
                StartText("Process " + currentProcess + ": <color=#00ffffff>JNE</color> Enter_Region", GetComponent<SpriteRenderer>().sprite);
                TextBox.textBox.SetClickable(false);

                if (!cmp) {
                    state = 2;
                    pcInput.text = state.ToString();
                }
                else {
                    state++;
                    pcInput.text = state.ToString();
                }
                break;

            case 6:     // load r1, toilet
                r[1] = Simulator.sim.toilet;
                rInput[1].text = r[1].ToString();

                state++;
                pcInput.text = state.ToString();

                Animate(6);
                StartText("Process " + currentProcess + ": <color=#00ffffff>LOAD</color> R1, TOILET", GetComponent<SpriteRenderer>().sprite);
                TextBox.textBox.SetClickable(false);
                break;

            case 7:     // inc r1
                r[1]++;
                rInput[1].text = r[1].ToString();

                state++;
                pcInput.text = state.ToString();

                Animate(7);
                StartText("Process " + currentProcess + ": <color=#00ffffff>INC</color> R1", GetComponent<SpriteRenderer>().sprite);
                TextBox.textBox.SetClickable(false);
                break;

            case 8:     // store toilet, r1
                Simulator.sim.toilet = r[1];
                Simulator.sim.toiletInput.text = r[1].ToString();

                state++;
                pcInput.text = state.ToString();

                Animate(8);
                StartText("Process " + currentProcess + ": <color=#00ffffff>STORE</color> TOILET, R1", GetComponent<SpriteRenderer>().sprite);
                TextBox.textBox.SetClickable(false);
                break;

            case 9:     // leave_region:
                state++;
                pcInput.text = state.ToString();

                Animate(9);
                StartText("Process " + currentProcess + ": Leave_Region", GetComponent<SpriteRenderer>().sprite);
                TextBox.textBox.SetClickable(false);
                break;

            case 10:    // move lock, #0
                Simulator.sim.lockVar = 0;
                Simulator.sim.lockInput.text = "0";

                state++;
                pcInput.text = state.ToString();

                Animate(10);
                StartText("Process " + currentProcess + ": <color=#00ffffff>MOVE</color> LOCK, #0", GetComponent<SpriteRenderer>().sprite);
                TextBox.textBox.SetClickable(false);
                break;

            case 11:    // brush_teeth
                state++;
                pcInput.text = state.ToString();

                Animate(11);
                StartText("Process " + currentProcess + ": Brush_Teeth", GetComponent<SpriteRenderer>().sprite);
                TextBox.textBox.SetClickable(false);
                break;

            case 12:    // inc r2
                r[2]++;
                rInput[2].text = r[2].ToString();

                state++;
                pcInput.text = state.ToString();

                Animate(12);
                StartText("Process " + currentProcess + ": <color=#00ffffff>INC</color> R2", GetComponent<SpriteRenderer>().sprite);
                TextBox.textBox.SetClickable(false);
                break;

            case 13:    // leave_bathroom:
                state++;
                pcInput.text = state.ToString();

                Animate(13);
                StartText("Process " + currentProcess + ": Leave_Bathroom", GetComponent<SpriteRenderer>().sprite);
                TextBox.textBox.SetClickable(false);
                break;

            case 14:    // dec r3
                r[3]--;
                rInput[3].text = r[3].ToString();

                state++;
                pcInput.text = state.ToString();

                Animate(14);
                StartText("Process " + currentProcess + ": <color=#00ffffff>DEC</color> R3", GetComponent<SpriteRenderer>().sprite);
                TextBox.textBox.SetClickable(false);
                break;

            case 15:
                break;
        }
    }

    public void Animate(int currentState) {
        switch (currentState) {
            case -1:     // waiting
                StartMoving(Simulator.sim.outsideBathroom.position);
                GetComponent<Renderer>().material.color = Color.white;
                break;

            case 0:     // waiting
                StartMoving(Simulator.sim.insideBathroom.position);
                GetComponent<Renderer>().material.color = Color.white;
                HideThoughts();
                break;

            case 1:     // use_toilet
                StartMoving(Simulator.sim.outsideToilet2.position);
                GetComponent<Renderer>().material.color = Color.white;

                HideThoughts();
                thoughtBubble.gameObject.SetActive(true);
                toilet.gameObject.SetActive(true);
                question.gameObject.SetActive(true);
                break;

            case 2:     // enter_region:
                StartMoving(Simulator.sim.outsideToilet1.position);
                GetComponent<Renderer>().material.color = Color.white;

                HideThoughts();
                thoughtBubble.gameObject.SetActive(true);
                toilet.gameObject.SetActive(true);
                break;

            case 3:     // tsl r0, lock
                Simulator.sim.doorText.text = "Lock\n1";
                Simulator.sim.toiletDoor.GetComponentInChildren<Renderer>().material.color = new Color(1f, 0.5f, 0.5f, 0.5f);
                
                StartMoving(Simulator.sim.outsideToilet1.position);

                if (r[0] == 0) GetComponent<Renderer>().material.color = Color.green;
                else GetComponent<Renderer>().material.color = Color.red;

                HideThoughts();
                thoughtBubble.gameObject.SetActive(true);
                toilet.gameObject.SetActive(true);
                door.gameObject.SetActive(true);

                Simulator.sim.GetComponent<AudioSource>().clip = Simulator.sim.tryOpen;
                if (playSound) Simulator.sim.GetComponent<AudioSource>().Play();

                if (playSound) Simulator.sim.knobAnim.Play();

                break;

            case 4:     // cmp r0, #0
                StartMoving(Simulator.sim.outsideToilet1.position);

                HideThoughts();
                thoughtBubble.gameObject.SetActive(true);
                openLock.gameObject.SetActive(true);
                question.gameObject.SetActive(true);
                
                if (cmp) GetComponent<Renderer>().material.color = Color.green;
                else     GetComponent<Renderer>().material.color = Color.red;

                break;

            case 5:     // jne enter_region
                if (!cmp) {
                    StartMoving(Simulator.sim.outsideToilet2.position);

                    HideThoughts();
                    thoughtBubble.gameObject.SetActive(true);
                    openLock.gameObject.SetActive(true);
                    no.gameObject.SetActive(true);

                    GetComponent<Renderer>().material.color = Color.white;

                    Simulator.sim.GetComponent<AudioSource>().clip = Simulator.sim.wrong;                    
                }
                else {
                    StartMoving(Simulator.sim.insideToilet.position);

                    HideThoughts();
                    thoughtBubble.gameObject.SetActive(true);
                    openLock.gameObject.SetActive(true);
                    yes.gameObject.SetActive(true);

                    GetComponent<Renderer>().material.color = Color.green;

                    Simulator.sim.GetComponent<AudioSource>().clip = Simulator.sim.right;
                    Simulator.sim.doorAnim.Play();

                    GetComponent<AudioSource>().clip = toiletSound;
                    if (playSound) GetComponent<AudioSource>().PlayDelayed(1);
                }
                if (playSound) Simulator.sim.GetComponent<AudioSource>().Play();
                break;

            case 6:     // load r1, toilet
                StartMoving(Simulator.sim.insideToilet.position);

                HideThoughts();
                thoughtBubble.gameObject.SetActive(true);
                toilet.gameObject.SetActive(true);

                GetComponent<Renderer>().material.color = Color.green;

                Simulator.sim.GetComponent<AudioSource>().clip = Simulator.sim.zipper;
                if (playSound) Simulator.sim.GetComponent<AudioSource>().Play();

                break;

            case 7:     // inc r1
                StartMoving(Simulator.sim.insideToilet.position);

                HideThoughts();
                thoughtBubble.gameObject.SetActive(true);
                poop.gameObject.SetActive(true);

                GetComponent<Renderer>().material.color = Color.green;
                
                break;

            case 8:     // store toilet, r1
                StartMoving(Simulator.sim.insideToilet.position);

                HideThoughts();
                thoughtBubble.gameObject.SetActive(true);
                toilet.gameObject.SetActive(true);

                GetComponent<Renderer>().material.color = Color.green;

                Simulator.sim.GetComponent<AudioSource>().clip = Simulator.sim.flush;
                if (playSound) Simulator.sim.GetComponent<AudioSource>().Play();

                break;

            case 9:     // leave_region:
                StartMoving(Simulator.sim.outsideToilet1.position);

                HideThoughts();
                thoughtBubble.gameObject.SetActive(true);
                openLock.gameObject.SetActive(true);

                GetComponent<Renderer>().material.color = Color.green;

                Simulator.sim.doorAnim.Play();

                break;

            case 10:    // move lock, #0
                Simulator.sim.doorText.text = "Lock\n0";
                Simulator.sim.toiletDoor.GetComponentInChildren<Renderer>().material.color = new Color (0.5f, 1f, 0.5f, 0.5f);

                StartMoving(Simulator.sim.outsideToilet1.position);
                
                HideThoughts();
                thoughtBubble.gameObject.SetActive(true);
                openLock.gameObject.SetActive(true);
                yes.gameObject.SetActive(true);

                GetComponent<Renderer>().material.color = Color.white;

                Simulator.sim.GetComponent<AudioSource>().clip = Simulator.sim.unlock;
                if (playSound) Simulator.sim.GetComponent<AudioSource>().Play();

                break;

            case 11:    // brush_teeth
                StartMoving(Simulator.sim.sink.position);

                HideThoughts();
                thoughtBubble.gameObject.SetActive(true);
                brush.gameObject.SetActive(true);

                GetComponent<Renderer>().material.color = Color.white;
                break;

            case 12:    // inc r2
                StartMoving(Simulator.sim.sink.position);

                HideThoughts();
                thoughtBubble.gameObject.SetActive(true);
                brush.gameObject.SetActive(true);
                yes.gameObject.SetActive(true);

                GetComponent<Renderer>().material.color = Color.white;
                break;

            case 13:    // leave_bathroom:
                StartMoving(Simulator.sim.insideBathroom.position);

                HideThoughts();
                thoughtBubble.gameObject.SetActive(true);
                door.gameObject.SetActive(true);
                question.gameObject.SetActive(true);

                GetComponent<Renderer>().material.color = Color.white;
                break;

            case 14:    // dec r3
                StartMoving(Simulator.sim.outsideBathroom.position);

                HideThoughts();
                thoughtBubble.gameObject.SetActive(true);
                door.gameObject.SetActive(true);
                yes.gameObject.SetActive(true);

                GetComponent<Renderer>().material.color = Color.white;
                break;

            case 15:
                break;
        }
    }

}
