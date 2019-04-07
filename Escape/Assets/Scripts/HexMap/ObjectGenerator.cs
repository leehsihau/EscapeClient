using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGenerator : MonoBehaviour
{
    [SerializeField] private GameObject chest, monster, hexMapEditor;

    List<GameObject> objects = new List<GameObject>();
    private HexMapEditor hexMapEditorScript;
    private HexGrid hexGrid;
    private static int x, z, px, pz, ox, oz;
    private static Vector3 pos, ppos;
    private static bool newMonster = false;
    private static bool newChest = false;
    private static bool playerLocationChanged = false;
    [SerializeField] private GameObject player, hexmapcamera;
    // Start is called before the first frame update
    void Start()
    {
        hexMapEditorScript = hexMapEditor.GetComponent<HexMapEditor>();
        hexGrid = hexMapEditorScript.hexGrid;
        px = 0;
        pz = 0;
        ox = -100000;
        oz = -100000;
    }

    public static void generateObject(bool isMonster, int xcor, int zcor)
    {

        if (isMonster)
        {
            newMonster = true;
        }
        else
        {
            newChest = true;
        }
        x = xcor;
        z = zcor;
    }

    public static void setNewLocation(int xcor, int zcor)
    {
        if(px == xcor && pz == zcor)
        {
            return;
        }
        playerLocationChanged = true;
        ox = px;
        oz = pz;
        px = xcor;
        pz = zcor;
    }


    void FixedUpdate()
    {
        if (newMonster)
        {
            newMonster = false;
            HexCell cell = hexGrid.GetCellByPosition(new HexCoordinates(x - 13, z - 25));
            pos = cell.transform.position;
            GameObject monsterInstance = Instantiate(monster, pos, Quaternion.identity, cell.transform);
            
            objects.Add(monsterInstance);
        }
        if (newChest)
        {
            newChest = false;
            HexCell cell = hexGrid.GetCellByPosition(new HexCoordinates(x - 13, z - 25));
            pos = cell.transform.position;
            GameObject chestInstance = Instantiate(chest, pos, Quaternion.identity, cell.transform);
            chestInstance.transform.Rotate(new Vector3(-90, 0, 0));
            objects.Add(chestInstance);
        }
        if (playerLocationChanged)
        {
            
            HexCell oldCell = hexGrid.GetCellByPosition(new HexCoordinates(ox, oz));
            if(oldCell.transform.childCount > 0) oldCell.transform.GetChild(0).gameObject.SetActive(true);
           
            playerLocationChanged = false;
            HexCell cell = hexGrid.GetCellByPosition(new HexCoordinates(px, pz));
            if (cell.transform.childCount > 0) cell.transform.GetChild(0).gameObject.SetActive(false);

            cell.Elevation = 10;
            ppos = cell.transform.position;
            player.transform.SetPositionAndRotation(ppos, Quaternion.identity);
            hexmapcamera.transform.SetPositionAndRotation(ppos, Quaternion.identity);
        }
    }
}
