using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Oculus.Platform;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DBHelper : MonoBehaviour
{
    string playerUsername = "";    

    private void Start()
    {
        print("this is dbhelper, platform is " + UnityEngine.Application.platform);
       
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            callUserInfo();
        }

    }

    public void callUserInfo()
    {
        print("this is before startcoroutine in call user info");
        StartCoroutine(UserInfo());
    }

    IEnumerator UserInfo()
    {
        print("this is user info function");
        WWWForm form = new WWWForm();
        playerUsername = DBManager.username;
        print("player username is " + playerUsername);

        form.AddField("name", playerUsername);
        WWW www = new WWW("http://students.cs.ucl.ac.uk/msc/shanshan.li/userInfo.php", form);
        print("after www in userInfo");
        yield return www;
        print("this is after return request result in user info, error " + www.error + "text " + www.text);

        if (www.text[0] == '0')
        {
            DBManager.username = playerUsername;
            DBManager.currentCount = int.Parse(www.text.Split('\t')[1]);
            print("success. current IPD: " + DBManager.currentIPD + " current count " + DBManager.currentCount + " current username" + DBManager.username);

        } 
        else
        {
            print("user create failed. Error #" + www.text);
        }

    }

    public void CallSavedResult(List<string> resultArray)
    {
        print("this is before in save result");
        StartCoroutine(SavedResult(resultArray));
    }


    IEnumerator SavedResult(List<string> resultArray)
    {
        WWWForm form = new WWWForm();
        string username = DBManager.username;
        print("this is username in save result" + username);
        // visual acuity
        // visual test type: C, E
        // test round: total 5 times
        string testType = resultArray[0];
        string visualAcuity = resultArray[1];

        form.AddField("name", username);
        form.AddField("testType", testType);
        form.AddField("testCounts", DBManager.currentCount);
        form.AddField("visualAcuity", visualAcuity);
        form.AddField("currentIPD", DBManager.currentIPD);

        print("after adding field");
        WWW www = new WWW("http://students.cs.ucl.ac.uk/msc/shanshan.li/saveResult.php", form);

        yield return www;
        print("after return www data");
        if (www.text[0] == '0')
        {
            Debug.Log("save result successfully");
        }
        else
        {
            Debug.Log("user create failed. Error #" + www.text);
        }
    }


}
