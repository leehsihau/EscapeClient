using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.IO;
using UnityEngine.SceneManagement;
using System.Text;

public class Login : MonoBehaviour {
	public GameObject userEmail;
	public GameObject password;
	private string Email;
	private string Password;
	public string[] lines;
	
	public void LoginButton(){
		bool N = false;
		bool P = false;
		if (Email != ""){
            N = true;
		} else {
			Debug.LogWarning("Username Field is empty");
		}
		if (Password != ""){
            P = true;
		} else {
			Debug.LogWarning("Password Field is empty");
		}
		if (N == true&& P == true){
            /*bool login = false;
            FireAuth.auth.SignInWithEmailAndPasswordAsync(Email, Password).ContinueWith(task => {
                if (task.IsCanceled)
                {
                    Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                    return;
                }
                
                Firebase.Auth.FirebaseUser newUser = task.Result;
                Communicator.user = newUser;
                Debug.LogFormat("User signed in successfully: {0} ({1})",
                    newUser.DisplayName, newUser.UserId);
                login = true;
            });*/
            Communicator.name = Email;
            Communicator.id = Password;
            SceneManager.LoadScene("Scene");
            Debug.Log("Login failed");
			userEmail.GetComponent<InputField>().text = "";
			password.GetComponent<InputField>().text = "";
            
        }
	}
	
	void FixedUpdate () {
		if (Input.GetKeyDown(KeyCode.Tab)){
			if (userEmail.GetComponent<InputField>().isFocused){
				password.GetComponent<InputField>().Select();
			}
		}
		if (Input.GetKeyDown(KeyCode.Return)){
			if (Password != ""&& Email != ""){
				LoginButton();
			}
		}
		Email = userEmail.GetComponent<InputField>().text;
		Password = password.GetComponent<InputField>().text;
	}
}
