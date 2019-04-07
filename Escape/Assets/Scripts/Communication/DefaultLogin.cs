using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DefaultLogin : MonoBehaviour
{
    // Start is called before the first frame update
    public void defaultLoginBoshengLi()
    {
        Communicator.name = "Bosheng Li";
        Communicator.id = "Bosheng Li";
        SceneManager.LoadScene("Scene");
    }
    public void defaultLoginRexSuter()
    {
        Communicator.name = "Rex Suter";
        Communicator.id = "Rex Suter";
        SceneManager.LoadScene("Scene");
    }
    public void defaultLoginJeremy()
    {
        Communicator.name = "Jeremy";
        Communicator.id = "Jeremy";
        SceneManager.LoadScene("Scene");
    }
    public void defaultLoginBobby()
    {
        Communicator.name = "Bobby";
        Communicator.id = "Bobby";
        SceneManager.LoadScene("Scene");
    }
    public void defaultLoginMauneel()
    {
        Communicator.name = "Mauneel";
        Communicator.id = "Mauneel";
        SceneManager.LoadScene("Scene");
    }
}
