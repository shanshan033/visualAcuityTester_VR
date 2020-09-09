using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class questionnaires : MonoBehaviour
{
    public GameObject answerGroup;
    public Text warningText;
    public Button submitButtonQ;
    // Start is called before the first frame update
    void Start()
    {
        warningText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void callNextStage()
    {
        warningText.text = "Loading... please wait...";
        submitButtonQ.interactable = false;
        StartCoroutine(nextStage());
    }

    IEnumerator nextStage() { 
        string answer = "";
        if (answerGroup.GetComponent<ToggleGroup> () != null)
        {
            for (int i = 0; i < answerGroup.transform.childCount; i++)
            {
                if (answerGroup.transform.GetChild(i).GetComponent<Toggle>().isOn)
                {
                    answer = answerGroup.transform.GetChild(i).Find("Label").GetComponent<Text>().text;
                    

                    break;
                }
            }
        }


        WWWForm form = new WWWForm();
        form.AddField("testCounts", DBManager.currentCount);
        form.AddField("name", DBManager.username);
        form.AddField("answer", answer);

        if (answer == "")
        {
            warningText.text = "The choice cannot be null";
            print("answer cannot be null");
        }
        else
        {
            WWW www = new WWW("http://students.cs.ucl.ac.uk/msc/shanshan.li/questionnaire.php", form);
            yield return www;
            print("this is after return www in questionnaire" + www.error + " " + www.text);
            if (www.text == "0")
            {
                print("save questionnaire result successfully");
                if (DBManager.testIPD.Count != 0)
                {
                    SceneManager.LoadScene("changeIPD", LoadSceneMode.Single);
                }
                else
                {
                    SceneManager.LoadScene("finishPage", LoadSceneMode.Single);
                }
            }
            else
            {
                Debug.Log("user create failed. Error #" + www.text);
                submitButtonQ.interactable = true;
            }
        }
    }
}
   
