using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HexMapEditor : MonoBehaviour {

	public Color[] colors;

    public HexGrid hexGrid;

    int activeElevation;

	Color activeColor;

	int brushSize;

	bool applyColor;
	bool applyElevation = true;

	public void SelectColor (int index) {
		applyColor = index >= 0;
		if (applyColor) {
			activeColor = colors[index];
		}
	}

	public void SetApplyElevation (bool toggle) {
		applyElevation = toggle;
	}

	public void SetElevation (float elevation) {
		activeElevation = (int)elevation;
	}

	public void SetBrushSize (float size) {
		brushSize = (int)size;
	}

	public void ShowUI (bool visible) {
		hexGrid.ShowUI(visible);
	}

	void Awake () {
		SelectColor(0);
	}

	void Update () {
		if (
			Input.GetMouseButton(0) &&
			!EventSystem.current.IsPointerOverGameObject()
		) {
			HandleInput();
		}
	}
    
    public static string TripleCoordinateFormat() {
        return "(" + State.CurX.ToString() + ", " + State.CurY.ToString() + ", " + State.CurZ.ToString() + ")";
    }

    public static string DoubleCoordinateFormat()
    {
        return "(" + State.CurX.ToString() + ", " + State.CurZ.ToString() + ")";
    }

    public static string TripleCoordinateFormatWithInput(HexCoordinates hexCords) {
        return "(" + hexCords.X + ", " + hexCords.Y + ", " + hexCords.Z + ")";
    }
    
    public static int XZtoDoorNumber(int x, int z) {
        // return 0 if cell clicked is current cell
        if (x == 0 && z == 0) return -1;
        // return 1-6 for corresponding neighbor if adjacent cell was clicked
        if (x == 0 && z == 1) return 0;
        if (x == 1 && z == 0) return 1;
        if (x == 1 && z == -1) return 2;
        if (x == 0 && z == -1) return 3;
        if (x == -1 && z == 0) return 4;
        if (x == -1 && z == 1) return 5;
        // otherwise return 7
        return 6;
    }

    public static bool IsNeighbor(int doorNumber)
    {
        // string comparison in C#?
        if (doorNumber == -1) return false;
        if (doorNumber >= 0 && doorNumber <= 5) return true;
        return false;
    }

    void HandleInput () {
		Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(inputRay, out hit)) {
            HexCell point = hexGrid.GetCell(hit.point);
            HexCoordinates hexCords = new HexCoordinates(point.coordinates.X, point.coordinates.Z);
            HexCell hexCell = hexGrid.GetCell(hit.point);

            Debug.Log("State --> Click:" + TripleCoordinateFormat() +" --> "+ TripleCoordinateFormatWithInput(hexCords));
            //Debug.Log("hexCell: " + hexCell.Position.ToString());

            int xDiff = hexCords.X - State.CurX;
            int zDiff = hexCords.Z - State.CurZ;
            int doorNumber = XZtoDoorNumber(xDiff, zDiff);

            if (IsNeighbor(doorNumber)) {
                //Debug.Log("Is a neighbor!");
                
                if (!hexCell.Knocked) {
                    // character knocks on door
                    hexCell.Color = Color.red;
                    hexCell.Knocked = true;
                    Debug.Log("doorNumber: " + doorNumber);
                    string knockRequestStr = "00014#" + doorNumber;
                    Debug.Log("Sending: " + knockRequestStr);
                    Communicator.sendMessage(knockRequestStr);
                } else {
                    // move character through door
                    State.CurX = hexCords.X;//13, -38, 25
                    State.CurY = hexCords.Y;
                    State.CurZ = hexCords.Z;
                    Communicator.sendMessage("00015#" + doorNumber);
                    Debug.Log("Sending: " + "00015#" + doorNumber);
                    Text pv = GameObject.Find("PositionVal").GetComponent<Text>();
                    //pv.text = TripleCoordinateFormat();
                    pv.text = DoubleCoordinateFormat();
                    EditCells(hexGrid.GetCell(hit.point));
                }
            }
            else {
                //Debug.Log("Not a neighbor!");
            }
		}
	}

    void EditAttack(int attack)
    {
        Text attackVal = GameObject.Find("AttackVal").GetComponent<Text>();
        attackVal.text = "moo";
    }

	void EditCells (HexCell center) {
		int centerX = center.coordinates.X;
		int centerZ = center.coordinates.Z;

		for (int r = 0, z = centerZ - brushSize; z <= centerZ; z++, r++) {
			for (int x = centerX - r; x <= centerX + brushSize; x++) {
				EditCell(hexGrid.GetCell(new HexCoordinates(x, z)));
			}
		}
		for (int r = 0, z = centerZ + brushSize; z > centerZ; z--, r++) {
			for (int x = centerX - brushSize; x <= centerX + r; x++) {
				EditCell(hexGrid.GetCell(new HexCoordinates(x, z)));
			}
		}
	}

	void EditCell (HexCell cell) {
		if (cell) {
			if (applyColor) {
				cell.Color = activeColor;
			}
			if (applyElevation) {
				cell.Elevation = activeElevation;
			}
		}
	}
}