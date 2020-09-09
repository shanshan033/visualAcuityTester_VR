using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class interactionCode : MonoBehaviour
{
    public Text visualAcuity;
    public Canvas resultPlane;
    public Text count;
    public Text errorCount;
    public Text correctCount;
    public GameObject optotype;
    public Text IPDCount;
    public Text NameField;
    public Button changeSceneButton;
    private int modeCounter;
    string _material;

    private void Start()
    {
        resultPlane.enabled = false;
        modeCounter = 0;
        _material = null;
    }

    public void updateText(float result)
    {
        visualAcuity.text = "Your visual acuity is " + result;
        resultPlane.enabled = true;

        /************************************************
         * Random the four testing scene
         ************************************************/
        List<int> tempScene = DBManager.randomScene;
        if (tempScene.Count != 0)
        {
            int indexRandom = UnityEngine.Random.Range(0, tempScene.Count);
            int _sceneId = tempScene[indexRandom];
            // remove the used id
            tempScene.RemoveAt(indexRandom);
            DBManager.randomScene = tempScene;
            print("scene id is " + _sceneId + "dbmanger random scene " + string.Join(", ", DBManager.randomScene));
            changeSceneButton.onClick.AddListener(() => { sceneChange(_sceneId); });
        }
        else
        {
            changeSceneButton.onClick.AddListener(() => {
                SceneManager.LoadScene("questionnaire", LoadSceneMode.Single);
            });
      
        }

    }

    public void startExperiment(float result)
    {
        visualAcuity.text = "Your visual acuity is " + result + ". The training is end, click the button to enter the formal experiment.";
        resultPlane.enabled = true;

        changeSceneButton.onClick.AddListener(() => {
            SceneManager.LoadScene("changeIPD", LoadSceneMode.Single);
        });
    }

    public void updateCount(float resultCount)
    {
        count.text = "Count: " + resultCount + "/ 3";
    }

    public void updateWrong(float wrong)
    {
        errorCount.text = "Wrong: " + wrong;
    }
    public void updateCorrect(float right)
    {
        correctCount.text = "Correct: " + right;
    }

    public void updateName(string username)
    {
        NameField.text = "ID: " + username;
    }

    public void updateCharacterMaterial(Material newMat)
    {
        optotype.GetComponent<Renderer>().material = newMat;
        _material = newMat.name;
    }

    public void updateCharacterSize(float optotypeSize)
    {
        // change the scale of optotype
        Vector3 currentScale = optotype.transform.localScale;
        optotype.transform.localScale = currentScale * optotypeSize;
    }

    public void updateIPD(float iPD)
    {
        IPDCount.text = "IPD: " + iPD;

    }

    void sceneChange(float screenId)
    {

        if (screenId == 0)
        {
            SceneManager.LoadScene("calibrationIPD", LoadSceneMode.Single);
        }
        else if (screenId == 1)
        {
            SceneManager.LoadScene("VisualTestThumblingE", LoadSceneMode.Single);
        }
        else if (screenId == 2)
        {
            SceneManager.LoadScene("VisualTestLandoltC", LoadSceneMode.Single);
        }
        else if (screenId == 3)
        {
            SceneManager.LoadScene("VisualTestThumblingE_Dark", LoadSceneMode.Single);
        }
        else if (screenId == 4)
        {
            SceneManager.LoadScene("VisualTestLandoltC_Dark", LoadSceneMode.Single);
        }
        else if (screenId == 5)
        {
            print("DBManager.testIPD.Count " + DBManager.testIPD.Count);
            if (DBManager.testIPD.Count != 0)
            {
                SceneManager.LoadScene("changeIPD", LoadSceneMode.Single);
            }
            else
            {
                SceneManager.LoadScene("finishPage", LoadSceneMode.Single);
            }

        }
    }
}
