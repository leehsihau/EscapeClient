using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

// Use plugin namespace
using HybridWebSocket;
using System;
using UnityEngine.SceneManagement;

public class Communicator : MonoBehaviour
{
    //public static WebSocket ws = WebSocketFactory.CreateInstance("ws://159.65.33.86:8080/EscapeServer/bws");
    public static WebSocket ws = WebSocketFactory.CreateInstance("ws://68.183.151.44:8080/EscapeServer/bws");
    public static Firebase.Auth.FirebaseUser user;
    public static string name;
    public static string id;
    [SerializeField] private FriendListUI friendListUI;


    public static bool loggedin = false;
    public static string playerStatus;

    // Use this for initialization
    void Start()
    {

        // Add OnOpen event listener
        ws.OnOpen += () =>
        {
            Debug.Log("WS connected!");
            Debug.Log("WS state: " + ws.GetState().ToString());
            ws.Send(Encoding.UTF8.GetBytes("00001#" + id + "#" + name));
            ws.Send(Encoding.UTF8.GetBytes("00012#"));
        };

        // Add OnMessage event listener
        ws.OnMessage += (byte[] msg) =>
        {
            string sm = Encoding.UTF8.GetString(msg);
            Debug.Log("WS received message: " + sm);
            try
            {
                processMessage(sm);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                Debug.Log(e.StackTrace);
            }
        };

        // Add OnError event listener
        ws.OnError += (string errMsg) =>
        {
            Debug.Log("WS error: " + errMsg);
        };

        // Add OnClose event listener
        ws.OnClose += (WebSocketCloseCode code) =>
        {
            Debug.Log("WS closed with code: " + code.ToString());
        };

        // Connect to the server
        ws.Connect();

    }

    public static void sendMessage(string msg)
    {
        ws.Send(Encoding.UTF8.GetBytes(msg));
    }



    public void sleep()
    {
        sendMessage("00013#");
        ws.Close();
        SceneManager.LoadScene("Login");
    }

    private void processMessage(string msg)
    {
        string[] resps = msg.Split('#');
        int res = int.Parse(resps[0]);
        if (res == 3)
        {
            playerStatus = msg;
            friendListUI.mynewname = resps[1];
            friendListUI.myNameChange = true;
            loggedin = true;
        }
        if (loggedin) switch (res)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    string username = resps[1];
                    int roomReference = int.Parse(resps[2]);
                    int roomLocationX = int.Parse(resps[3]);
                    int roomLocationZ = int.Parse(resps[4]);
                    ObjectGenerator.setNewLocation(roomLocationX, roomLocationZ);
                    State.CurX = roomLocationX + 13;
                    State.CurZ = roomLocationZ + 25;
                    int experience = int.Parse(resps[5]);
                    int level = int.Parse(resps[6]);
                    int curHealth = int.Parse(resps[7]);
                    int maxHealth = int.Parse(resps[8]);
                    int armor = int.Parse(resps[9]);
                    int attack = int.Parse(resps[10]);
                    int equipmentReference1 = int.Parse(resps[11]);
                    int equipmentReference2 = int.Parse(resps[12]);
                    int curScore = int.Parse(resps[13]);
                    int highScore = int.Parse(resps[14]);

                    State.CurHealth = curHealth;
                    State.MaxHealth = maxHealth;
                    State.Attack = attack;
                    State.Armor = armor;

                    CharacterStats.CharStatsValChanged = true;

                    //Debug.Log("Got a status update from the server. About to try to update the UI");
                    //CharacterStats.UpdateAttackArmorUI(attack, armor);
                    // Run script to update the UI references to these.
                    break;
                case 4:
                    break;
                case 5:
                    break;
                case 6:
                    Debug.Log("pass");
                    friendListUI.UpdateFriendList(resps);
                    break;
                case 7:
                    break;
                case 8:

                    break;
                case 9:
                    break;
                case 10:
                    break;
                case 11:
                    break;
                case 12:
                    break;
                case 13:
                    break;
                case 14:
                    break;
                case 15:
                    //  check backpack info
                    // max slot amount for backpack
                    // amount (num items? or num fields?)
                    // list of item refs
                    //   #itemRef #itemType #itemLevel #itemValue

                    int maxSlotAmt = int.Parse(resps[1]);
                    List<int> lists = new List<int>();
                    int numItemsRec = int.Parse(resps[2]);
                    InventoryPanel.b = numItemsRec;

                    for (int i = 0; i < numItemsRec; i++)
                    {
                        int itemRef = int.Parse(resps[3 + (4 * i)]);
                        lists.Add(itemRef);
                        int itemType = int.Parse(resps[4 + (4 * i)]);
                        lists.Add(itemType);
                        int itemLevel = int.Parse(resps[5 + (4 * i)]);
                        lists.Add(itemLevel);
                        int itemValue = int.Parse(resps[6 + (4 * i)]);
                        lists.Add(itemValue);
                        // Create item icon using the above to add to ground list for cell

                    }
                    InventoryPanel.blist = lists;
                    InventoryPanel.bupdate = true;
                    break;
                case 16:
                    break;
                case 17:
                    break;
                case 18:
                    break;
                case 19:
                    string resTypeStr = resps[1];
                    int knockRes = int.Parse(resTypeStr);
                    if (knockRes < 0)
                    {
                        // 00019#-1                  --> Error
                        Debug.Log("Error parsing: " + msg);
                    }
                    else if (knockRes >= 0)
                    {
                        int doorNumber = knockRes;
                        // 00019#(1-6)#(Enemy|Chest) --> knock response
                        string behindDoor = resps[2]; // String of "T" or "F"
                        Debug.Log(behindDoor + " behind door #" + doorNumber + ".");
                        bool isMonster = string.Equals(behindDoor, "T");
                        int x = State.CurX;
                        int z = State.CurZ;
                        switch (doorNumber)
                        {
                            case 0:
                                z++;
                                break;
                            case 1:
                                x++;
                                break;
                            case 2:
                                x++;
                                z--;
                                break;
                            case 3:
                                z--;
                                break;
                            case 4:
                                x--;
                                break;
                            case 5:
                                x--;
                                z++;
                                break;
                        }

                        ObjectGenerator.generateObject(isMonster, x, z);
                        // Lower the elevation?
                    }
                    break;
                case 20:
                    break;
                case 21:
                    break;
                case 22:
                    break;
                case 23:
                    break;
                case 24:
                    break;
                case 25:
                    break;
                case 26:
                    break;
                case 27:
                    break;
                case 28:
                    break;
                case 29:
                    break;
                case 30:
                    break;
                case 31:

                    PublicChat.newMessageText = resps[1];
                    PublicChat.newMessage = true;
                    break;
                case 32:
                    break;
                case 33:
                    break;
                case 34:
                    break;
                case 35:
                    break;
                case 36:
                    break;
                case 37://ground list
                    //first param: # num items received
                    // then list of item refs. 
                    List<int> listss = new List<int>();
                    int nuItemsRec = int.Parse(resps[1]);
                    InventoryPanel.g = nuItemsRec;
                    for (int i = 0; i < nuItemsRec; i++)
                    {
                        int itemRef = int.Parse(resps[2 + (4 * i)]);
                        listss.Add(itemRef);
                        int itemType = int.Parse(resps[3 + (4 * i)]);
                        listss.Add(itemType);
                        int itemLevel = int.Parse(resps[4 + (4 * i)]);
                        listss.Add(itemLevel);
                        int itemValue = int.Parse(resps[5 + (4 * i)]);
                        // Create item icon using the above to add to ground list for cell
                        listss.Add(itemValue);
                    }
                    InventoryPanel.glist = listss;
                    InventoryPanel.gupdate = true;
                    break;
                case 38:
                    friendListUI.newOnlinePlayer(resps[1]);
                    break;
                case 39:
                    friendListUI.newOfflinePlayer(resps[1]);
                    break;
                case 40:
                    friendListUI.friendName = "#NEW " + resps[1];
                    friendListUI.newFriendAdded = true;
                    friendListUI.log = "U have a friend request!";
                    friendListUI.showlog = true;
                    break;
                case 41:
                    if (resps[1].Equals("S"))
                    {
                        friendListUI.log = "Request sent";
                        friendListUI.showlog = true;
                    }
                    else
                    {
                        friendListUI.log = "Request not sent.";
                        friendListUI.showlog = true;
                    }
                    break;
                case 42:
                    if (resps[2].Equals("S"))
                    {
                        friendListUI.friendName = resps[1];
                        friendListUI.newFriendAdded = true;
                        friendListUI.log = resps[1] + " has accepted your request!";
                        friendListUI.showlog = true;
                    }
                    else
                    {
                        friendListUI.log = resps[1] + " has rejected your request.";
                        friendListUI.showlog = true;
                    }
                    break;
                case 43:
                    if (resps[1].Equals("S"))
                    {
                        friendListUI.friendName = resps[2];
                        friendListUI.newFriendAdded = true;

                    }
                    break;
                case 44:
                    break;
                case 45:
                    if (resps[1].Equals("S"))
                    {
                        friendListUI.log = "Friend deleted.";
                        friendListUI.showlog = true;
                        FriendListUI.OnRemoveFriend(resps[2]);
                        FriendListUI.friendListUpdated = true;
                    }
                    else
                    {
                        friendListUI.log = "Failed to delete.";
                        friendListUI.showlog = true;
                    }
                    break;
                case 46:
                    break;
                case 47:
                    break;
                case 48:
                    FriendListUI.OnRemoveFriend(resps[1]);
                    FriendListUI.friendListUpdated = true;
                    break;
                case 49:
                    friendListUI.newOfflinePlayer(resps[1]);
                    break;
                case 52:
                    List<int> listsss = new List<int>();
                    InventoryPanel.e = 2;

                    for (int i = 0; i < 8; i++)
                    {
                        listsss.Add(int.Parse(resps[i + 1]));
                    }
                    InventoryPanel.elist = listsss;
                    InventoryPanel.eupdate = true;
                    break;
                case 99998:
                    BattleHistoryUI.newMessageText = resps[1].Replace("<br>", "\n");
                    BattleHistoryUI.newMessage = true;
                    break;
                default:
                    break;
            }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
