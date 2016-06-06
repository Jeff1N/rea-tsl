using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class criticalRegionScript : Script {

    private bool menuOpened = false;
    private int lang;

    public Text labelText;

	void Start () {
        ClearAll();
        lang = PlayerPrefs.GetInt("lang");

        if (lang == 0)
            labelText.text = ("<b>PC:</b> Program Counter\n\n" +
                               "<b>CMP:</b> Flag de comparação (0 - diferente, 1 - igual)\n\n" +
                               "<b>R0:</b> Cópia local de LOCK\n\n" +
                               "<b>R1:</b> Cópia local de TOILET\n\n" +
                               "<b>R2:</b> Já escovou os dentes? (0 - não, 1 - sim)\n\n" +
                               "<b>R3:</b> Está dentro do banheiro? (0 - não, 1 - sim)\n\n" +
                               "<b>LOCK:</b> A porta está trancada? (0 - não, 1 - sim)\n\n" +
                               "<b>TOILET:</b> Contador do número de descargas");
        else if (lang == 1)
            labelText.text = ("<b>PC:</b> Program Counter\n\n" +
                               "<b>CMP:</b> Compare flag (0 - different, 1 - equal)\n\n" +
                               "<b>R0:</b> LOCK local copy\n\n" +
                               "<b>R1:</b> TOILET local copy\n\n" +
                               "<b>R2:</b> Teeth already brused? (0 - no, 1 - yes)\n\n" +
                               "<b>R3:</b> Is inside bathroom? (0 - yes, 1 - no)\n\n" +
                               "<b>LOCK:</b> Is the door locker? (0 - yes, 1- no)\n\n" +
                               "<b>TOILET:</b> Flush counter");
    }

    public override void StartNodeSwitch(int startingNode) {
        switch (startingNode) {
            case 0:
                if (!menuOpened) StartNode(1);
                else StartNode(2);
                break;

            case 1:
                Simulator.sim.PauseSim();

                if (lang == 0) {
                    AddDecisionButton(0, "Resetar o simulador", 2);
                    AddDecisionButton("Casos Especiais", 10);
                    AddDecisionButton("Fechar o Menu", 2);
                    AddDecisionButton("Voltar para a Tela Inicial", 3);
                }else if (lang == 1) {
                    AddDecisionButton(0, "Reset the simulator", 2);
                    AddDecisionButton("Special cases", 10);
                    AddDecisionButton("Close this menu", 2);
                    AddDecisionButton("Return to the Main Menu", 3);
                }
                StartDecision();

                menuOpened = true;
                break;

            case 2:
                menuOpened = false;
                ClearAll();
                break;
            case 3:
                SceneManager.LoadScene("MainMenu");
                break;
                
            case 10:
                if (lang == 0) {
                    AddDecisionButton(1, "Todos na região crítica", 2);
                    AddDecisionButton(2, "Todos executando TSL", 2);
                    AddDecisionButton("Voltar", 1);
                }else if (lang == 1) {
                    AddDecisionButton(1, "Every process in the critical region", 2);
                    AddDecisionButton(2, "Every process running TSL", 2);
                    AddDecisionButton("Back", 1);
                }
                StartDecision();
                break;
        }
    }

    public override void ButtonActionSwitch(int buttonID) {
        switch (buttonID) {
            case 0: //Reset
                Simulator.sim.undoStack.Clear();
                Simulator.sim.PauseSim();

                //Repaint
                if (Simulator.sim.process[Simulator.sim.currentProcess].state < 15) {
                    if (Simulator.sim.process[Simulator.sim.currentProcess].state >= 6 &&
                        Simulator.sim.process[Simulator.sim.currentProcess].state <= 8)

                        Simulator.sim.codeLine[Simulator.sim.process[Simulator.sim.currentProcess].state].color = new Color(0.625f, 0.125f, 0.125f);
                    else
                        Simulator.sim.codeLine[Simulator.sim.process[Simulator.sim.currentProcess].state].color = Color.white;
                }
                Simulator.sim.processPanel[Simulator.sim.currentProcess].color = Color.white;
                
                Simulator.sim.codeLine[0].color = new Color(1f, 1f, 0f);
                Simulator.sim.processPanel[0].color = new Color(1f, 1f, 0.5f);

                Simulator.sim.toiletDoor.GetComponentInChildren<Renderer>().material.color = new Color(0.5f, 1f, 0.5f, 0.5f);

                //Resetting Simulator and Process Variables
                Simulator.sim.opsCount = 0;
                Simulator.sim.currentProcess = 0;
                Simulator.sim.lockVar = 0;
                Simulator.sim.toilet = 0;
                
                for (int i = 0; i < 4; i++) {
                    Simulator.sim.process[i].state = 0;
                    Simulator.sim.process[i].cmp = false;
                    for (int j = 0; j < 4; j++) {
                        Simulator.sim.process[i].r[j] = 0;
                    }
                }

                //Resetting InputFields 
                Simulator.sim.processInput.text = "0";
                Simulator.sim.lockInput.text = "0";
                Simulator.sim.toiletInput.text = "0";

                for (int i = 0; i < 4; i++) {
                    Simulator.sim.process[i].pcInput.text = "0";
                    Simulator.sim.process[i].cmpInput.text = "0";
                    Simulator.sim.process[i].rInput[0].textComponent.color = Color.green;
                    for (int j = 0; j < 4; j++) {
                        Simulator.sim.process[i].rInput[j].text = "0";
                    }
                    Simulator.sim.process[i].Animate(-1);
                }

                break;


            case 1: //Todos os processos na região crítica
                Simulator.sim.undoStack.Clear();
                Simulator.sim.PauseSim();

                //Repaint
                if (Simulator.sim.process[Simulator.sim.currentProcess].state < 15)
                    Simulator.sim.codeLine[Simulator.sim.process[Simulator.sim.currentProcess].state].color = Color.white;
                Simulator.sim.processPanel[Simulator.sim.currentProcess].color = Color.white;

                Simulator.sim.codeLine[6].color = new Color(1f, 1f, 0f);
                Simulator.sim.processPanel[0].color = new Color(1f, 1f, 0.5f);

                Simulator.sim.toiletDoor.GetComponentInChildren<Renderer>().material.color = new Color(0.625f, 0.125f, 0.125f);

                //Resetting Simulator and Process Variables
                Simulator.sim.opsCount = 0;
                Simulator.sim.currentProcess = 0;
                Simulator.sim.lockVar = 0;
                Simulator.sim.toilet = 0;
                Simulator.sim.OpsPerProcess = 2;

                for (int i = 0; i < 4; i++) {
                    Simulator.sim.process[i].state = 6;
                    Simulator.sim.process[i].cmp = true;
                    for (int j = 0; j < 4; j++) {
                        Simulator.sim.process[i].r[j] = 0;
                    }
                }

                //Resetting InputFields 
                Simulator.sim.processInput.text = "0";
                Simulator.sim.lockInput.text = "0";
                Simulator.sim.toiletInput.text = "0";
                Simulator.sim.opsInput.text = "2";

                for (int i = 0; i < 4; i++) {
                    Simulator.sim.process[i].pcInput.text = "6";
                    Simulator.sim.process[i].cmpInput.text = "1";
                    Simulator.sim.process[i].rInput[0].textComponent.color = Color.green;
                    for (int j = 0; j < 4; j++) {
                        Simulator.sim.process[i].rInput[j].text = "0";
                    }
                    Simulator.sim.process[i].Animate(5);
                }

                break;


            case 2: //Todos os processos executando TSL
                Simulator.sim.undoStack.Clear();
                Simulator.sim.PauseSim();

                //Repaint
                if (Simulator.sim.process[Simulator.sim.currentProcess].state < 15)
                    Simulator.sim.codeLine[Simulator.sim.process[Simulator.sim.currentProcess].state].color = Color.white;
                Simulator.sim.processPanel[Simulator.sim.currentProcess].color = Color.white;

                Simulator.sim.codeLine[3].color = new Color(1f, 1f, 0f);
                Simulator.sim.processPanel[0].color = new Color(1f, 1f, 0.5f);

                Simulator.sim.toiletDoor.GetComponentInChildren<Renderer>().material.color = new Color(0.5f, 1f, 0.5f, 0.5f);

                //Resetting Simulator and Process Variables
                Simulator.sim.opsCount = 0;
                Simulator.sim.currentProcess = 0;
                Simulator.sim.lockVar = 0;
                Simulator.sim.toilet = 0;
                Simulator.sim.OpsPerProcess = 2;
                
                for (int i = 0; i < 4; i++) {
                    Simulator.sim.process[i].state = 3;
                    Simulator.sim.process[i].cmp = false;
                    for (int j = 0; j < 4; j++) {
                        Simulator.sim.process[i].r[j] = 0;
                    }
                }

                //Resetting InputFields 
                Simulator.sim.processInput.text = "0";
                Simulator.sim.lockInput.text = "0";
                Simulator.sim.toiletInput.text = "0";
                Simulator.sim.opsInput.text = "2";

                for (int i = 0; i < 4; i++) {
                    Simulator.sim.process[i].pcInput.text = "3";
                    Simulator.sim.process[i].cmpInput.text = "0";
                    Simulator.sim.process[i].rInput[0].textComponent.color = Color.green;
                    for (int j = 0; j < 4; j++) {
                        Simulator.sim.process[i].rInput[j].text = "0";
                    }
                    Simulator.sim.process[i].Animate(2);
                }

                break;
        }
    }
}
