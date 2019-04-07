using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStats : MonoBehaviour
{

    [SerializeField] private GameObject AttackVal;
    [SerializeField] private GameObject ArmorVal;
    [SerializeField] private GameObject TopLevelInventoryPanel;
    public static bool CharStatsValChanged = false;
    public static bool InventoryPanelActive = false;
    
    // Start is called before the first frame update
    void Start()
    {
        State.CurX = 13;
        State.CurY = -38;
        State.CurZ = 25;
        TopLevelInventoryPanel.gameObject.SetActive(InventoryPanelActive);
        //positionVal.GetComponent<Text>().text = TripleCoordinateFormat();
    }

    public void OnClickItemsButton()
    {
        InventoryPanelActive = !InventoryPanelActive;
        TopLevelInventoryPanel.gameObject.SetActive(InventoryPanelActive);
        Communicator.sendMessage("00029");
        Communicator.sendMessage("00021");
        Communicator.sendMessage("00031");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       
        if (CharStatsValChanged)
        {
            CharStatsValChanged = false;
            AttackVal.GetComponent<Text>().text = State.Attack.ToString();
            ArmorVal.GetComponent<Text>().text = State.Armor.ToString();
        }
    }
}
