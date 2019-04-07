using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;
using System.Text;

public class Register : MonoBehaviour {
	public GameObject email;
	public GameObject password;
	public GameObject passwordConformation;
	public string Name;
	private string Email;
	private string Password;
	private string PasswordConformation;
	private string Form;
	private bool EmailValid = false;
    
    void Start(){
		email.transform.GetChild(2).GetComponent<CanvasRenderer>().SetAlpha(0f);
		password.transform.GetChild(2).GetComponent<CanvasRenderer>().SetAlpha(0f);
        
    }

    public void RegisterButton(){
		bool N = false;
		bool E = false;
		bool P = false;
		bool PC = false;
		EmailValid = false;
		
		if (Email != ""){
			EmailValid = Regex.IsMatch(Email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
			if (EmailValid == true){
				if(Email.Contains("@") == true){
					if(Email.Contains(".") == true){
						E = true;
						email.transform.GetChild(3).GetComponent<CanvasRenderer>().SetAlpha(0.7f);
						email.transform.GetChild(3).GetComponent<CanvasRenderer>().SetColor(Color.green);
					} else {
						email.transform.GetChild(3).GetComponent<CanvasRenderer>().SetColor(Color.red);
						email.transform.GetChild(3).GetComponent<CanvasRenderer>().SetAlpha(0.7f);
						Debug.LogWarning("Email Field is Incorrect");
					}
				} else {
					email.transform.GetChild(3).GetComponent<CanvasRenderer>().SetColor(Color.red);
					email.transform.GetChild(3).GetComponent<CanvasRenderer>().SetAlpha(0.7f);
					Debug.LogWarning("Email Field is Incorrect");
				}
			} else {
				email.transform.GetChild(3).GetComponent<CanvasRenderer>().SetColor(Color.red);
				email.transform.GetChild(3).GetComponent<CanvasRenderer>().SetAlpha(0.7f);
				Debug.LogWarning("Email Field is Incorrect");
			}
		} else {
			email.transform.GetChild(3).GetComponent<CanvasRenderer>().SetColor(Color.red);
			email.transform.GetChild(3).GetComponent<CanvasRenderer>().SetAlpha(0.7f);
			Debug.LogWarning("Email Field is empty or Incorrect");
		}
		if (Password != ""){
			if (Password.Length > 5){
				P = true;
			} else {
				password.transform.GetChild(3).GetComponent<CanvasRenderer>().SetColor(Color.red);
				password.transform.GetChild(3).GetComponent<CanvasRenderer>().SetAlpha(0.7f);
				Debug.LogWarning("Password Must at least be 6 characters long");
			}
		} else {
			password.transform.GetChild(3).GetComponent<CanvasRenderer>().SetColor(Color.red);
			password.transform.GetChild(3).GetComponent<CanvasRenderer>().SetAlpha(0.7f);
			Debug.LogWarning("Password Field is empty");
		}
		if (Password == PasswordConformation){
			PC = true;
			if (P == true){
				password.transform.GetChild(3).GetComponent<CanvasRenderer>().SetColor(Color.green);
				password.transform.GetChild(3).GetComponent<CanvasRenderer>().SetAlpha(0.7f);
			}
		} else {
			password.transform.GetChild(3).GetComponent<CanvasRenderer>().SetColor(Color.red);
			password.transform.GetChild(3).GetComponent<CanvasRenderer>().SetAlpha(0.7f);
			Debug.LogWarning("Passwords Don't Match");
		}
		if (N == true && E == true && P == true && PC == true){
            Debug.LogWarning("Correct Register");
            FireAuth.auth.CreateUserWithEmailAndPasswordAsync(Email, Password).ContinueWith(task => {
                if (task.IsCanceled)
                {
                    Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                    return;
                }

                // Firebase user has been created.
                Firebase.Auth.FirebaseUser newUser = task.Result;
                Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                    newUser.DisplayName, newUser.UserId);
                SceneManager.LoadScene("Scene");
            });

			email.GetComponent<InputField>().text = "";
			password.GetComponent<InputField>().text = "";
			passwordConformation.GetComponent<InputField>().text = "";
			email.transform.GetChild(3).GetComponent<CanvasRenderer>().SetAlpha(0f);
			password.transform.GetChild(3).GetComponent<CanvasRenderer>().SetAlpha(0f);
        }
	}
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.Tab)){
			if (email.GetComponent<InputField>().isFocused){
				password.GetComponent<InputField>().Select();
			}
			if (password.GetComponent<InputField>().isFocused){
				passwordConformation.GetComponent<InputField>().Select();
			}
		}
		if (Input.GetKeyDown(KeyCode.Return)){
			if (PasswordConformation != ""&&Password != ""&&Email != ""&&Name != ""){
				RegisterButton();
			}
		}
		Email = email.GetComponent<InputField>().text;
		Password = password.GetComponent<InputField>().text;
		PasswordConformation = passwordConformation.GetComponent<InputField>().text;
		
		
	}
}