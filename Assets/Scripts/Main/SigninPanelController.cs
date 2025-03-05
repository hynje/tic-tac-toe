using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public struct SigninData
{
    public string username;
    public string password;
}

public struct SigninResult
{
    public int result;
}

public struct ScoreResult
{
    public string id;
    public string username;
    public string nickname;
    public int score;
}
public class SigninPanelController : MonoBehaviour
{
    [SerializeField] TMP_InputField usernameInputField;
    [SerializeField] TMP_InputField passwordInputField;

    public void OnClickSigninButton()
    {
        string username = usernameInputField.text;
        string password = passwordInputField.text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            // Todo : 누락된 값 입력 요청 팝업 
            return;
        }
        
        var signinData = new SigninData();
        signinData.username = username;
        signinData.password = password;
        
        StartCoroutine(NetworkManager.Instance.SignIn(signinData, () =>
        {
            Destroy(gameObject);
        },
        result =>
        {
            if (result == 0)
            {
                usernameInputField.text = "";
            }
            else if (result == 1)
            {
             passwordInputField.text = "";   
            }
        }));
    }
    
    public void OnClickSignupButton()
    {
        GameManager.Instance.OpenSignupPanel();
    }
}
