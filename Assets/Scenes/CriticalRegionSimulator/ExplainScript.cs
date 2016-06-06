using UnityEngine;
using System.Collections;

public class ExplainScript : Script {
    int lang;

    public CanvasRenderer pLabels;
    public CanvasRenderer codeArrow;
    public CanvasRenderer varsArrow;
    public CanvasRenderer explainArrow;

    public bool isExecuting = false;

    void Awake() {
        lang = PlayerPrefs.GetInt("lang");
    }

    void Start() {
        ClearAll();
    }

    public override void StartNodeSwitch(int startingNode) {
        TextBox.textBox.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 900f);
        TextBox.textBox.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 225f);
        TextBox.textBox.renderSpeed = 30f;

        switch (startingNode) {
            case -1:
                StopExplain();
                break;

            case 0:
                Simulator.sim.PauseSim();

                if (isExecuting) {
                    StartNode(-1);
                    break;
                }

                isExecuting = true;

                if (lang == 0)
                    StartText("Este simulador simula o comportamento de 4\n"+
                              "processos competindo para entrar na região\n"+
                              "crítica. Os processos, numerados de 0 a 3,\n"+
                              "são representados, respectivamente, pelos", 1);
                else if (lang == 1)
                    StartText("This simulator shows the behaviour of 4\n" +
                              "processes competing to enter the critical\n" +
                              "region. The proccesses, numbererd from 0 to\n" +
                              "3, are representendo by the characters", 1);
                break;

            case 1:
                pLabels.gameObject.SetActive(true);

                if (lang == 0)
                    StartText("personagens Coragem (P0), Eustácio (P1),\n" +
                              "Muriel (P2) e Espírito da Lua Cheia (P3),\n" +
                              "cada um com seus program counters (pc),\n" +
                              "flags compare (cmp) e registradores de 0\n" +
                              "a 3 (r0~r3) representados na tabela\n"+
                              "abaixo.", 2);
                else if (lang == 1)
                    StartText("Courage (P0), Eustace (P1), Muriel (P2)\n" +
                              "and Spirit of the Harvest Moon(P3),\n" +
                              "each with their program counters (pc),\n" +
                              "compare flags (cmp) and registers from 0\n" +
                              "to 3 (r0~r3) shown in the table below.", 2);
                break;

            case 2:
                pLabels.gameObject.SetActive(false);
                codeArrow.gameObject.SetActive(true);

                if (lang == 0)
                    StartText("O código executado por cada processo é\n" +
                              "mostrado no canto inferior esquerdo,\n" +
                              "sendo que a linha realçada mostra a\n" +
                              "próxima instrução a ser realizada pelo\n" +
                              "processo atualmente em execução. A\n" +
                              "tabela do processo em execução também é\n" +
                              "realçada. As 3 linhas em vermelho representam\n" +
                              "a execução da região crítica", 3);
                else if (lang == 1)                    
                    StartText("The code executed by the processes is\n" +
                              "shown in the lower left corner, with the\n" +
                              "highlighted code line showing the next\n" +
                              "instruction to be executed by the currently\n" +
                              "active process. The active process' table\n" +
                              "is also highlited. The 3 red lines represent\n" +
                              "the critical region code\n", 3);
                break;

            case 3:
                codeArrow.gameObject.SetActive(false);
                varsArrow.gameObject.SetActive(true);

                if (lang == 0)
                    StartText("A tabela superior dentre as três do\n" +
                              "do canto inferior direito controla a\n" +
                              "execução do simulador. O primeiro botão\n" +
                              "desfaz o último passo, o segundo inicia\n" +
                              "a execução automática e o terceiro realiza\n" +
                              "um passo. Na execução automática, cada\n" +
                              "processo realiza um número determinado\n" +
                              "de ações e 'passa a vez' para o próximo,\n" +
                              "enquanto que o botão de um passo só muda\n" +
                              "de processo se o usuário fizer essa mudança\n" +
                              "manualmente.\n\n" +
                              
                              "A tabela do meio mostra, respectivamente,\n" + 
                              "o processo atual, o número de instruções\n" +
                              "executadas por processo na execução\n" +
                              "automática e a duração em segundos do tick\n" +
                              "de clock na execução automática.\n\n\n\n" +

                              "A tabela de baixo mostra os valores atuais\n" +
                              "das variáveis compartilhadas LOCK e TOILET\n\n\n"
                              , 4);
                else if (lang == 1)
                    StartText("The upper table of the three stacked\n" +
                              "tables on the lower right corner shows\n" +
                              "the execution buttons. The first is the\n" +
                              "undo button, the second is the play button\n" +
                              "and the third is the 'one step' button.\n" +
                              "When play is pressed, each process executes\n" +
                              "a number of instructions and then let the\n" +
                              "next one be the active process, while the\n" +
                              "one step button requires process change to\n" +
                              "be done manually.\n" +

                              "The middle table shows the current active\n" +
                              "process, the number of instructions executed\n" +
                              "by process when the simulator is played and\n" +
                              "how long a clock tick takes to happen in\n" +
                              "seconds.\n\n" +

                              "The lower table shows the current value of\n" +
                              "the shared variables LOCK and TOILET.\n\n\n"
                              , 4);
                break;

            case 4:
                varsArrow.gameObject.SetActive(false);
                explainArrow.gameObject.SetActive(true);

                if (lang == 0)
                    StartText("A tabela mais à direita explica o que\n" +
                              "cada registrador e variável faz no código\n" +
                              "executado, para facilitar seu entendimento\n", -1);
                else if (lang == 1)
                    StartText("The right most table explains what each\n" +
                              "register and variable do in the executed code\n" +
                              "to make it easier to understand.\n", -1);
                break;
        }
    }

    public void StopExplain() {
        ClearAll();
        isExecuting = false;        

        TextBox.textBox.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 700f);
        TextBox.textBox.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 100f);
        TextBox.textBox.renderSpeed = 0f;

        pLabels.gameObject.SetActive(false);
        codeArrow.gameObject.SetActive(false);
        varsArrow.gameObject.SetActive(false);
        explainArrow.gameObject.SetActive(false);
    }
}
