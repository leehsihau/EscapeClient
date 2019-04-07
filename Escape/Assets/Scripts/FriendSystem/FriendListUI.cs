using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendListUI : MonoBehaviour
{
    [Header("Settings")]
    public bool SortOnline = true;
    [Header("References")]
    public GameObject FriendUIPrefab = null;
    public Transform ViewPoint = null;
    public Text FriendsCountText = null;
    public Text MyName = null;
    public string mynewname;
    [SerializeField] private Image ArrowImage;
    [SerializeField] private Sprite UpArrowSprite;
    [SerializeField] private Sprite BottomArrowSprite;
    [SerializeField] private Text LogText;
    [SerializeField] private InputField friendNameInput;
    private static int OnlineCount = 0;
    private static bool onlineFirst = false;
    private static List<bl_FriendInfo> cacheFriendsInfo = new List<bl_FriendInfo>();
    private static FriendInfo[] friends;
    public bool myNameChange = false;
    public string friendName;
    private bool firstTime = true;
    public bool newFriendAdded = false;
    private bool friendListChanged = false;
    public static bool friendListUpdated = false;
    private bool infoChanged = false;
    public string log;
    public bool showlog = false;
    private static GameObject[] fs;
    // Start is called before the first frame update
    void Start()
    {
        onlineFirst = SortOnline;
        ArrowImage.sprite = (onlineFirst == true) ? UpArrowSprite : BottomArrowSprite;
        if (LogText != null)
        {
            LogText.canvasRenderer.SetAlpha(0);
        }
        friends = new FriendInfo[20];
        for(int i = 0; i < 20; i++)
        {
            friends[i] = new FriendInfo();
        }
    }

    void FixedUpdate()
    {
        if (myNameChange)
        {
            MyName.text = mynewname;
            myNameChange = false;
        }
        if (friendListChanged)
        {
            ViewPoint.DetachChildren();
            friendListChanged = false;
            InstanceFriend();
        }
        if (friendListUpdated)
        {
            int j = cacheFriendsInfo.Count;
            for (int i = 0; i < j; i++)
            {
                cacheFriendsInfo[i].RefreshInfo(friends);
                if (cacheFriendsInfo[i].cacheName.Equals("#"))
                {
                    cacheFriendsInfo.Remove(cacheFriendsInfo[i]);
                    i--;
                    j--;
                }
            }
            friendListUpdated = false;
        }
        if (newFriendAdded)
        {
            OnAddFriend();
            newFriendAdded = false;
        }
        if (showlog)
        {
            showlog = false;
            ShowLog(log);
        }
    }

    void InstanceFriend()
    {
        cacheFriendsInfo.Clear();
        for (int i = 0; i < friends.Length; i++)
        {
            if (!friends[i].Name.Equals("#"))
            {
                Debug.Log(friends[i].Name);
                GameObject f = Instantiate(FriendUIPrefab) as GameObject;
                bl_FriendInfo info = f.GetComponent<bl_FriendInfo>();
                info.GetInfo(friends[i]);
                cacheFriendsInfo.Add(info);
                f.name = friends[i].Name;
                f.transform.SetParent(ViewPoint, false);
            }
        }
    }

    void OnAddFriend()
    {
        for(int i = 0; i < friends.Length; i++)
        {
            if (friends[i].Name.Equals("#"))
            {
                friends[i].Name = friendName;
                GameObject f = Instantiate(FriendUIPrefab) as GameObject;
                bl_FriendInfo info = f.GetComponent<bl_FriendInfo>();
                info.GetInfo(friends[i]);
                cacheFriendsInfo.Add(info);
                f.name = friends[i].Name;
                f.transform.SetParent(ViewPoint, false);
                break;
            }
        }
        
    }

    public static void OnRemoveFriend(string name)
    {
        for (int i = 0; i < friends.Length; i++)
        {
            if (friends[i].Name.Equals(name))
            {
                friends[i].Name = "#";
                friends[i].IsOnline = true;
                
                friends[i].Room = 0;
                friendListUpdated = true;
                break;
            }
        }

    }

    public void ChangeOnlineFirst()
    {
        onlineFirst = !onlineFirst;
        RectTransform fui = transform.GetComponent<RectTransform>();
        if (onlineFirst)
        {
            
            ArrowImage.sprite = UpArrowSprite;
            fui.anchoredPosition = new Vector2(2f, -389f);
            
        }
        else
        {
            if (firstTime)
            {
                firstTime = false;
                ReqUpdateFriendList();
            }
            ArrowImage.sprite = BottomArrowSprite;
            fui.anchoredPosition = new Vector2(2f, 0f);
            
        }
        
    }

    public void ReqUpdateFriendList()
    {
        Communicator.sendMessage("00008");
    }

    public void UpdateFriendList(string[] resps)
    {
        Debug.Log(resps);
        for (int i = 0; i < 20; i++)
        {
            friends[i].Name = "#";
        }
        
        for(int i = 1; i < resps.Length; i++)
        {
            //get friend's location?
            friends[i - 1].Name = resps[i];
            
        }
        infoChanged = true;
        friendListChanged = true;
    }

    public void UpdateOnlineList()
    {

    }

    public void newOnlinePlayer(string name)
    {
        for(int i = 0; i < friends.Length; i++)
        {
            if (friends[i].Name.Equals(name))
            {
                friends[i].IsOnline = true;
                friendListUpdated = true;
                break;
            }
        }
    }

    public void newOfflinePlayer(string name)
    {
        for (int i = 0; i < friends.Length; i++)
        {
            if (friends[i].Name.Equals(name))
            {
                friends[i].IsOnline = false;
                friendListUpdated = true;
                break;
            }
        }
    }

    public void AddFriend(InputField field)
    {
        
        string t = field.text;
        if (string.IsNullOrEmpty(t))
            return;

        if (hasThisFriend(t))
        {
            ShowLog("Already has added this friend.");
            return;
        }
        Communicator.sendMessage("00009#" + t);
    }

    public void RemoveFriend(string name)
    {
        Communicator.sendMessage("00041#" + name);
    }

    

    public void Chat(string name)
    {

    }

    void HideLog()
    {
        if (LogText != null)
        {
            LogText.CrossFadeAlpha(0, 1, true);
        }
    }

    public void ShowLog(string log)
    {
        CancelInvoke("HideLog");
        if (LogText != null)
        {
            LogText.text = log;
            LogText.CrossFadeAlpha(1, 0.7f, true);
        }
        Invoke("HideLog", 3);
    }

    private bool hasThisFriend(string fname)
    {
        for (int i = 0; i < cacheFriendsInfo.Count; i++)
        {
            if (cacheFriendsInfo[i].name == fname)
            {
                return true;
            }
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
