using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class adjustIPDScript : MonoBehaviour
{
    int adjustIPD;
    public Text hintIPD;
    public GameObject loader;
    public Button confirmButton;

    // Start is called before the first frame update
    void Start()
    {
        /***************************************************
         * Random the five testing IPD
         ***************************************************/
        //adjustIPD = DBManager.testIPD.Pop();
        int indexRandom = UnityEngine.Random.Range(0, DBManager.testIPD.Count);
        print("temp random scene count " + DBManager.testIPD.Count + " index random " + indexRandom + "name " + DBManager.username);
        adjustIPD = DBManager.testIPD[indexRandom];
        // remove the used id
        DBManager.testIPD.RemoveAt(indexRandom);
        //DBManager.testIPD = tempIPD;
        //DBManager.randomScene = tempScene;
        print("testIPD count " + DBManager.testIPD.Count + "new ipd from db manager: " + adjustIPD + "db name in adjustscript " + DBManager.username);
        hintIPD.text = "Adjust your IPD of the HMD to " + adjustIPD;
        loader.SetActive(false);
    }

    public void startTestWithNewIPD()
    {
        hintIPD.text = "Loading... please wait... (It may take a few seconds)";
        confirmButton.interactable = false;
        StartCoroutine(TestWithNewIPD());
    }

    IEnumerator TestWithNewIPD()
    {
        WWWForm form =new WWWForm();
        form.AddField("ipd", adjustIPD);
        form.AddField("name", DBManager.username);

        print("this is wwwform in adjust ipd script, name is " + DBManager.username);
        WWW www = new WWW("http://students.cs.ucl.ac.uk/msc/shanshan.li/calibration.php", form);
        yield return www;
        print("this is after return www" + www.error + www.text);
        if (www.text == "0")
        {
            print("update successfully");
            /*****************************************************
             * Assign ipd
             *****************************************************/
            DBManager.currentIPD = adjustIPD;
            /*****************************************************
             * Random the four testing scene with initialization
             *****************************************************/
            List<int> tempScene = new List<int>() { 1, 2, 3, 4 };
            DBManager.randomScene = new List<int>();
            
            int indexRandom = UnityEngine.Random.Range(0, tempScene.Count);
            print("temp random scene count " + tempScene.Count + " index random " + indexRandom + "new dbmanger length " + DBManager.randomScene.Count + "name " + DBManager.username) ;
            int _sceneId = tempScene[indexRandom];
            // remove the used id
            tempScene.RemoveAt(indexRandom);
            //DBManager.testIPD = tempIPD;
            DBManager.randomScene = tempScene;
            print("scene id is " + _sceneId + "dbmanger random scene count " + DBManager.randomScene.Count);
            UnityEngine.SceneManagement.SceneManager.LoadScene(_sceneId);
        }
        else
        {
            print("update ipd failed: error" + www.text);
            confirmButton.interactable = true;
        }
    }

}
