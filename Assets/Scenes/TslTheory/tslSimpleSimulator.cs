using UnityEngine;
using UnityEngine.UI;

public class tslSimpleSimulator : MonoBehaviour {
    public int register = 0;
    public int lockVar  = 0;

    public InputField regText;
    public InputField lckText;

    public void TSL() {
        register    = lockVar;
        lockVar     = 1;

        regText.text = register.ToString();
        lckText.text = lockVar.ToString();
    }

    public void UpdateReg() {
        register = int.Parse(regText.text);
    }

    public void UpdateLock() {
        lockVar = int.Parse(lckText.text);
    }
}
