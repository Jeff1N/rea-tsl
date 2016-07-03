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
                    AddDecisionButton("Simulador de exclusão mútua com TSL", 11);
                    AddDecisionButton("Créditos", 12);
                    AddDecisionButton("Mudar a linguagem", 13);
                }
                else if (lang == 1) {
                    AddDecisionButton("The TSL instruction", 10);
                    AddDecisionButton("Mutual exclusion simulator using TSL", 11);
                    AddDecisionButton("Credits", 12);
                    AddDecisionButton("Change language", 13);
                }

                StartDecision();
                break;

            case 10:
                SceneManager.LoadScene("TslTheory");
                break;
            
            case 11:
                SceneManager.LoadScene("CriticalRegionSimulator");
                break;
            case 12:
                if (lang == 0) SceneManager.LoadScene("Credits_ptbr");
                else if (lang == 1) SceneManager.LoadScene("Credits_en");
                break;
            case 13:
                SceneManager.LoadScene("LangSelect");
                break;
        }
    }
}
