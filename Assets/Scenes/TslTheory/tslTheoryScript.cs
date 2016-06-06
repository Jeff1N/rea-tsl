using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class tslTheoryScript : Script {
    public tslSimpleSimulator tslPanel;
    public CanvasRenderer codePanel;

    int lang;

    void Awake() {
        lang = PlayerPrefs.GetInt("lang");
    }

    //Override this function to define the behaviour of each node
    public override void StartNodeSwitch(int startingNode) {
        switch (startingNode) {
            case 0:
                if (lang == 0) AddDecisionButton("Computador, o que é TSL?", 2);
                else if (lang == 1) AddDecisionButton("Computer, what does TSL mean?", 2);

                StartDecision();
                break;
            case 2:
                StartText(Resources.Load<TextAsset>("Texts/tslTheory0_"+(lang == 0 ? "ptbr" : "en")).text, 3);
                break;
            case 3:
                ClearText();
                tslPanel.gameObject.SetActive(true);
                break;

            case 10:
                ClearText();
                tslPanel.gameObject.SetActive(false);

                if (lang == 0) AddDecisionButton("Hum, legal... E pra que isso serve?", 11);
                else if (lang == 1) AddDecisionButton("Hmm, cool.. But what is it for?", 11);

                StartDecision();
                break;
            case 11:
                StartText(Resources.Load<TextAsset>("Texts/tslTheory1_" + (lang == 0 ? "ptbr" : "en")).text, 12);
                break;
            case 12:
                ClearText();
                codePanel.gameObject.SetActive(false);

                if (lang == 0) {
                    AddDecisionButton("...O que é região crítica mesmo?", 13);
                    AddDecisionButton("Como exatamente TSL pode ser útil para isso?", 14);
                    AddDecisionButton("Repita a definição de TSL, por favor", 2);
                    AddDecisionButton(0, "Entendi. Por favor, volte ao menu principal");
                } else if (lang == 1) {
                    AddDecisionButton("...What is a critial region?", 13);
                    AddDecisionButton("How exactly can TSL solve this ?", 14);
                    AddDecisionButton("Could you please repeat the definition of TSL?", 2);
                    AddDecisionButton(0, "Got it, back to main menu, please");
                }

                StartDecision();
                break;
            case 13:
                StartText(Resources.Load<TextAsset>("Texts/tslTheory2_" + (lang == 0 ? "ptbr" : "en")).text, 12);
                break;
            case 14:
                codePanel.gameObject.SetActive(true);
                StartText(Resources.Load<TextAsset>("Texts/tslTheory3_" + (lang == 0 ? "ptbr" : "en")).text, 12);
                break;

            case 200:
                ClearAll();
                break;
        }
    }

    public override void ButtonActionSwitch(int buttonID) {
        switch (buttonID) {
            case 0:
                SceneManager.LoadScene("MainMenu");
                break;
        }
    }
}
