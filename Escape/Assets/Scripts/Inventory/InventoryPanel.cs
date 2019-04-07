using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPanel : MonoBehaviour
{
    [SerializeField] private GameObject GroundItemsPanel, EquippedItemsPanel, InventoryItemsPanel, icon;
    private List<GameObject> groundIconList = new List<GameObject>();
    private List<GameObject> backpackIconList = new List<GameObject>();
    private List<GameObject> equippedIconList = new List<GameObject>();

    public static bool gupdate, bupdate, eupdate;

    public static List<int> glist, blist, elist;
    public static int g, b, e;

    // Start is called before the first frame update


    public void updateGroundPanel()
    {
        Debug.Log("updating with " + glist.Count + "");
        for (int i = 0; i < groundIconList.Count; i++)
        {
            Destroy(groundIconList[i]);
        }
        int numItemsRec = g;
        for (int i = 0; i < numItemsRec; i++)
        {
            int itemRef = glist[4 * i];
            int itemType = glist[4 * i + 1];
            int itemLevel = glist[4 * i + 2];
            int itemValue = glist[4 * i + 3];
            // Create item icon using the above to add to ground list for cell
            GameObject instance = Instantiate(icon, GroundItemsPanel.transform);
            instance.GetComponent<Icon>().reference = itemRef;
            instance.GetComponent<Icon>().status = itemType;
            instance.GetComponent<Icon>().lvlval = itemLevel;
            instance.GetComponent<Icon>().val = itemValue;
            instance.GetComponent<Icon>().setIcon();
            backpackIconList.Add(instance);
        }
    }

    public void updateInventoryPanel()
    {
        for (int i = 0; i < backpackIconList.Count; i++)
        {
            Destroy(backpackIconList[i]);
        }
        int numItemsRec = b;
        for (int i = 0; i < numItemsRec; i++)
        {
            int itemRef = blist[4 * i];
            int itemType = blist[4 * i + 1];
            int itemLevel = blist[4 * i + 2];
            int itemValue = blist[4 * i + 3];
            // Create item icon using the above to add to ground list for cell
            GameObject instance = Instantiate(icon, InventoryItemsPanel.transform);
            instance.GetComponent<Icon>().reference = itemRef;
            instance.GetComponent<Icon>().status = itemType;
            instance.GetComponent<Icon>().lvlval = itemLevel;
            instance.GetComponent<Icon>().val = itemValue;
            instance.GetComponent<Icon>().setIcon();
            backpackIconList.Add(instance);

        }
    }

    public void updateEquippedPanel()
    {
        for (int i = 0; i < equippedIconList.Count; i++)
        {
            Destroy(equippedIconList[i]);
        }
        int numItemsRec = e;
        for (int i = 0; i < numItemsRec; i++)
        {
            int itemRef = elist[4 * i];
            int itemType = elist[4 * i + 1];
            int itemLevel = elist[4 * i + 2];
            int itemValue = elist[4 * i + 3];
            // Create item icon using the above to add to ground list for cell
            if (itemRef != -1)
            {
                GameObject instance = Instantiate(icon, EquippedItemsPanel.transform);
                instance.GetComponent<Icon>().reference = itemRef;
                instance.GetComponent<Icon>().status = itemType;
                instance.GetComponent<Icon>().lvlval = itemLevel;
                instance.GetComponent<Icon>().val = itemValue;
                instance.GetComponent<Icon>().setIcon();
                equippedIconList.Add(instance);
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (bupdate)
        {
            bupdate = false;
            updateInventoryPanel();
        }

        if (gupdate)
        {
            gupdate = false;
            updateGroundPanel();
        }

        if (eupdate)
        {
            eupdate = false;
            updateEquippedPanel();
        }
    }
}
