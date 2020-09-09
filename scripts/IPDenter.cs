using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class IPDenter : MonoBehaviour
{
    public Text ipdField;
    public Button submitButton;
    public Text hintField;

    private void Start()
    {
        submitButton.interactable = false;
    }

    private void Update()
    {
        if (ipdField.text.Length != 0)
        {
            submitButton.interactable = true;
        }
        else
        {
            submitButton.interactable = false;
        }
    }
    public void callCalibration()
    {
        StartCoroutine(Calibration());
        
    }

    IEnumerator Calibration()
    {
        /*********************************************************
         * Assign the username and current IPD
         *********************************************************/
        string username = DBManager.username;
        WWWForm form = new WWWForm();
        int userIPD = Int16.Parse(ipdField.text);
        DBManager.currentIPD = userIPD;

        if (userIPD < 60 || userIPD > 71)
        {
            hintField.text = "Sorry, the IPD is too big/small";
            ipdField.text = "";
        }
        else
        {
            hintField.text = "You are being directed to the training section...";
            form.AddField("ipd", userIPD.ToString());
            form.AddField("name", username);
            ipdField.text = "";
            WWW www = new WWW("http://students.cs.ucl.ac.uk/msc/shanshan.li/calibration.php", form);
            yield return www;
            
            if (www.text == "0")
            {
                List<int> tempIPD = new List<int>();
                tempIPD.Add(userIPD);

                if (userIPD < 62)
                {
                    tempIPD.Add(userIPD + 8);
                    tempIPD.Add(userIPD + 6);
                    tempIPD.Add(userIPD + 4);
                    tempIPD.Add(userIPD + 2); 
                }
                else if (userIPD >=62 && userIPD < 64)
                {
                    tempIPD.Add(userIPD + 6);
                    tempIPD.Add(userIPD + 4);
                    tempIPD.Add(userIPD + 2);
                    tempIPD.Add(userIPD - 2);
                }
                else if (userIPD > 69)
                {
                    tempIPD.Add(userIPD - 8);
                    tempIPD.Add(userIPD - 6);
                    tempIPD.Add(userIPD - 4);
                    tempIPD.Add(userIPD - 2);
                }else if (userIPD <= 69 && userIPD > 67)
                {
                    tempIPD.Add(userIPD - 6);
                    tempIPD.Add(userIPD - 4);
                    tempIPD.Add(userIPD - 2);
                    tempIPD.Add(userIPD + 2);
                }
                else
                {
                    tempIPD.Add(userIPD + 4);
                    tempIPD.Add(userIPD + 2);
                    tempIPD.Add(userIPD - 2);
                    tempIPD.Add(userIPD - 4);
                }
                Debug.Log("user create successfully");
                //DBManager.testIPD.Clear();
                /*********************************
                 * Insert the test IPDs into DBManager
                 *********************************/
                DBManager.testIPD = new List<int>();
                DBManager.testIPD = tempIPD;
                UnityEngine.SceneManagement.SceneManager.LoadScene("trainingScene");
            }
            else
            {
                Debug.Log("user create failed. Error #" + www.text);
            }

        }
    }


}

