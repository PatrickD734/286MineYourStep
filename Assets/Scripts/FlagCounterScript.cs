using UnityEngine;
using UnityEngine.UI;
public class FlagCounterScript : MonoBehaviour
{
    public Text flagCT;
    public MineSweeperGameHandler fl;

    private void Start()
    {
        fl = GameObject.FindFirstObjectByType<MineSweeperGameHandler>();
    }
    void Update (){
        if (fl != null)
        {
            flagCT.text = "Flags left: " + fl.flagCount;
        }
    }
}
