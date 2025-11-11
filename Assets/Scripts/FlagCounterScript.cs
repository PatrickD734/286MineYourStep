using UnityEngine;
using UnityEngine.UI;
public class FlagCounterScript : MonoBehaviour
{
    public Text flagCT;
    public MineSweeperGameHandler fl;
    void Update (){
        flagCT.text = "Flags left: " + fl.flagCount; 
    }
}
