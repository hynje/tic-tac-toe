using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public struct SignupData
{
    public string username;
    public string nickname;
    public string password;
}
public class SignupPanelController : MonoBehaviour
{
    [SerializeField] private TMP_InputField usernameInputField;
    [SerializeField] private TMP_InputField nicknameInputField;
    [SerializeField] private TMP_InputField passwordInputField;
    [SerializeField] private TMP_InputField confirmPasswordInputField;
    
    public void OnclickConfirmButton()
    {
        var username = usernameInputField.text;
        var nickname = nicknameInputField.text;
        var password = passwordInputField.text;
        var confirmPassword = confirmPasswordInputField.text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(nickname) || string.IsNullOrEmpty(password) ||
            string.IsNullOrEmpty(confirmPassword))
        {
            // 입력값이 비어있음을 알리는 팝업
            GameManager.Instance.OpenConfirmPanel("입력할 내용이 남았습니다.", () =>
            {
                
            });
            return;
        }

        if (password.Equals(confirmPassword))
        {
            SignupData signupData = new SignupData();
            signupData.username = username;
            signupData.nickname = nickname;
            signupData.password = password;
            
            // 서버로 Signup 데이터 전달하면서 회원가입 진행 
            StartCoroutine(NetworkManager.Instance.Signup(signupData, () =>
            {
                Destroy(gameObject);
            },
            () =>
            {
                usernameInputField.text = "";
                nicknameInputField.text = "";
                passwordInputField.text = "";
                confirmPasswordInputField.text = "";
            }));
        }
        else
        {
            GameManager.Instance.OpenConfirmPanel("비밀번호가 서로 다릅니다.", () =>
            {
                
            });
        }
    }
    
    public void OnclickCancelButton()
    {
        
    }
}
