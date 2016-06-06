using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenuScript : Script {
    int lang;

    void Awake() {
        lang = PlayerPrefs.GetInt("lang");
    }

    public override void StartNodeSwitch(int startingNode) {
        
        switch (startingNode) {
            case 0:
                if (lang == 0)      StartText("Escolha uma opção", 1);
                else if (lang == 1) StartText("Choose an option", 1);

                break;

            case 1:
                if (lang == 0) {
                    AddDecisionButton("A Instrução TSL", 10);
                    AddDecisionButton("Simulador de exclusão mútua com TSL", 21);
                }else if (lang == 1) {
                    AddDecisionButton("The TSL instruction", 10);
                    AddDecisionButton("Mutual exclusion simulator using TSL", 21);
                }

                StartDecision();
                break;

            case 10:
                SceneManager.LoadScene("TslTheory");
                break;
            
            case 21:
                SceneManager.LoadScene("CriticalRegionSimulator");
                break;
        }
    }
}
