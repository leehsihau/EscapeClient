using UnityEngine;
using UnityEngine.UI;

public class bl_FriendInfo : MonoBehaviour {

    public Text NameText = null;
    public Text StatusText = null;
    public Image StatusImage = null;
    public GameObject JoinButton = null;
    public Text ButtonText;
    [Space(5)]
    public Color OnlineColor = new Color(0, 0.9f, 0, 0.9f);
    public Color OffLineColor = new Color(0.9f, 0, 0, 0.9f);
    private int roomName = -1;
    public string cacheName;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="info"></param>
    public void GetInfo(FriendInfo info)
    {
        if(info.Name.StartsWith("#NEW"))
        {
            ButtonText.text = "Accept";
        }
        cacheName = info.Name;
        NameText.text = info.Name;
        if (StatusText != null) { StatusText.text = (info.IsOnline) ? "[Online]" : "[OffLine]"; }
        StatusImage.color = (info.IsOnline) ? OnlineColor : OffLineColor;
        JoinButton.SetActive((info.IsOnline) ? true : false);
        roomName = info.Room;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="info"></param>
    public bl_FriendInfo RefreshInfo(FriendInfo[] infos)
    {
        FriendInfo info = FindMe(infos);
        if (info == null)
        {
            cacheName = "#";
            return this;
        }
        NameText.text = info.Name;
        if (StatusText != null) { StatusText.text = (info.IsOnline) ? "[Online]" : "[OffLine]"; }
        StatusImage.color = (info.IsOnline) ? OnlineColor : OffLineColor;
        JoinButton.SetActive((info.IsOnline) ? true : false);
        roomName = info.Room;
        return null;
    }

    private FriendInfo FindMe(FriendInfo[] info)
    {
        Debug.Log(cacheName);
        for(int i = 0; i < info.Length; i++)
        {
            if(info[i].Name.Equals(cacheName))
            {
                return info[i];
            }
        }
        Destroy(gameObject);
        return null;
    }

    /// <summary>
    /// 
    /// </summary>
    

    /// <summary>
    /// 
    /// </summary>
    public void Remove()
    {
        if (cacheName.StartsWith("#NEW"))
        {
            Communicator.sendMessage("00036#" + cacheName.Substring(5));
            FriendListUI.OnRemoveFriend(cacheName);
            FriendListUI.friendListUpdated = true;
        }
        else
        {
            FriendListUI manager = FindObjectOfType<FriendListUI>();
            manager.RemoveFriend(NameText.text);
        }
        PrivateChat.name = "Loading...";
        PrivateChat.nameChanged = true;
    }

    public void Chat()
    {
        if (cacheName.StartsWith("#NEW"))
        {
            Communicator.sendMessage("00035#" + cacheName.Substring(5));
            FriendListUI.OnRemoveFriend(cacheName);
            FriendListUI.friendListUpdated = true;
        }
        else
        {
            PrivateChat.name = cacheName;
            PrivateChat.nameChanged = true;
        }
    }
}