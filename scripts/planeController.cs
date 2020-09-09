using System.Collections;
using System.Collections.Generic;
using Oculus.Platform;
using UnityEngine;
using UnityEngine.SceneManagement;

public class planeController : MonoBehaviour
{
    private float LEFTDIR = 1;
    private float RIGHTDIR = 2;
    private float UPDIR = 3;
    private float DOWNDIR = 4;
    static float TEST_DISTANCE = 4.0f;// 12m
    static float ORIGINAL_IMAGE_HEIGHT = 10.0f; // 10m

  

    float count;
    float wrongTime;
    float correctTime;
    float currentDirection;
    float choice;
    string optotypeMat;
    float visualAcuity; // the result of visual acuity 
    float _IPD;
    float _sceneId;
    string _username;

    GameObject optotype;
    public interactionCode gamePlayer;
    public DBHelper _dbHelper;

    
    // Start is called before the first frame update
    void Start()
    {
        // Initialize variables
        count = 0;
        wrongTime = 0;
        currentDirection = 2;
        choice = 0;
        visualAcuity = 0.01f;

        // Initialize gameobject
        optotype = GameObject.Find("optotype");
        _sceneId = SceneManager.GetActiveScene().buildIndex;
        print("scene id" + _sceneId);

        /*********************************************************************
         * Update the scale according to the visual acuity function
         *********************************************************************/
        float height = 10f * TEST_DISTANCE * Mathf.Tan(Mathf.PI / (visualAcuity * 120f * 180f)); // unit is meter
        float _scaleRate = height / ORIGINAL_IMAGE_HEIGHT;
        transform.localScale = new Vector3(_scaleRate, 1.0f, _scaleRate);

        /*********************************************************************
         * Update the IPD entered by participants before
         *********************************************************************/
        _IPD = DBManager.currentIPD;
        gamePlayer.updateIPD(_IPD);
        // Update the player name
        _username = DBManager.username;
        gamePlayer.updateName(_username);
        print("db manger in planecontroller " + _username);

    }

    /*********************************************************************
     * Set the choice that the user choose the direction of the optotype
     *********************************************************************/
    public void setChoice(float buttonId)
    {
        choice = buttonId;
    }



    /*********************************************************************
     * Find the test type according to the scene id
     *********************************************************************/
    string findTestType(float sceneId)
    {
        if(sceneId == 1)
        {
            return "E";
        }
        else if (sceneId == 2)
        {
            return "C";
        }
        else if (sceneId == 3)
        {
            return "E_D";
        }
        else
        {
            return "C_D";
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*********************************************************************
         * Show the IPD value that participants enter
         *********************************************************************/
        _IPD = DBManager.currentIPD;
        gamePlayer.updateIPD(_IPD);

        /*********************************************************************
         * Show the current random generated name of participants 
         *********************************************************************/
        _username = DBManager.username;
        gamePlayer.updateName(_username);

        if (choice != 0)
        {
            print("db manger in planecontroller update " + _username);

            count += 1;

            if (currentDirection == choice)
            {
                correctTime += 1.0f;
                Debug.Log("CORRECT time" + correctTime.ToString());
            }
            else
            {
                // keep same
                wrongTime += 1.0f;
                Debug.Log("WRONG time" + wrongTime.ToString());
            }

            // Return a random direction of optotype
            currentDirection = Random.Range(1, 5);
            optotypeMat = randomRotation(currentDirection, _sceneId);
            // Assigns a material named "Assets/Resources/Materials/xxx to the object
            Material newMat = Resources.Load("Materials/"+optotypeMat, typeof(Material)) as Material;
            optotype.GetComponent<Renderer>().material = newMat;
            
            print("count is " + count + " choice " + choice);
            choice = 0;
            // change the scale of optotype
            if (count >= 3)
            {
                if (wrongTime >= 2)
                {
                    // keep same
                    // determine the visual acuity
                    Debug.Log("visual acuity:" + visualAcuity);
                    visualAcuity = (float)System.Math.Round((double)(visualAcuity), 3);

                    /*********************************************************************
                    * Pass the user's test result to the datbase through DBHelper
                    *********************************************************************/
                    if (_sceneId != 8)
                    {
                        List<string> resultArray = new List<string>();
                        // Find and pass the current test type
                        string testType = findTestType(_sceneId);
                        resultArray.Add(testType);

                        // Pass the visual acuity
                        resultArray.Add(visualAcuity.ToString());

                        // Pass all information to the database

                        _dbHelper.CallSavedResult(resultArray);

                        gamePlayer.updateText(visualAcuity);
                    }
                    else
                    {
                        gamePlayer.startExperiment(visualAcuity);
                    }
                }
                else
                {
                    // resize decreasely
                    // at the begining, the resize gap could be larger 
                    if(visualAcuity <= 0.1)
                    {
                        visualAcuity += 0.03f;
                    }
                    else
                    {
                        visualAcuity += 0.01f;
                    }
                    
                    Debug.Log("correct resizing");

                    /*********************************************************************
                    * Calculate the optotype size using the given function
                    *********************************************************************/
                    float height = 10f * TEST_DISTANCE * Mathf.Tan(Mathf.PI / (visualAcuity * 120f * 180f)); // unit is meter
                    float _scaleRate = height / ORIGINAL_IMAGE_HEIGHT;
                    _scaleRate = (float)System.Math.Round((double)(_scaleRate), 4);
                    transform.localScale= new Vector3 (_scaleRate, 1.0f, _scaleRate);
                    Debug.Log("current scacle" + transform.localScale.ToString());
                }
                // reset a set of optotype
               
                count = 0;
                wrongTime = 0;
                correctTime = 0;

            }

            /*********************************************************************
             * Update the correct, wrong, total number of the test
             *********************************************************************/
            gamePlayer.updateCount(count + 1);
            gamePlayer.updateWrong(wrongTime);
            gamePlayer.updateCorrect(correctTime);

        }

    }
    /*********************************************************************
    * Generate a material according to the direction of the optotype and
    * current scene.
    *********************************************************************/
    string randomRotation(float randomDirection, float scenceId)
    {
        string material = null;

        if (randomDirection == LEFTDIR)
        {
            if (scenceId == 1)
            {
                material = "optotypeLeftE";
            }
            else if(scenceId == 2)
            {
                material = "optotypeLeftC";
            }
            else if (scenceId == 4)
            {
                material = "optotypeLeftC_dark";
            }
            else if (scenceId == 3)
            {
                material = "optotypeLeftE_dark";
            }
            else if (scenceId == 8)
            {
                material = "optotypeLeftC";
            }
        }
        else if (randomDirection == RIGHTDIR)
        {
            if (scenceId == 1)
            {
                material = "optotypeRightE";
            }
            else if (scenceId == 2)
            {
                material = "optotypeRightC";
            }else if (scenceId == 4)
            {
                material = "optotypeRightC_dark";
            }
            else if(scenceId == 3)
            {
                material = "optotypeRightE_dark";
            }
            else if (scenceId == 8)
            {
                material = "optotypeRightC";
            }
        }
        else if (randomDirection == UPDIR)
        {
            if (scenceId == 1)
            {
                material = "optotypeUpE";
            }
            else if (scenceId == 2)
            {
                material = "optotypeUpC";
            }
            else if (scenceId == 4)
            {
                material = "optotypeUpC_dark";
            }
            else if (scenceId == 3)
            {
                material = "optotypeUpE_dark";
            }
            else if (scenceId == 8)
            {
                material = "optotypeUpC";
            }
        }
        else if (randomDirection == DOWNDIR)
        {
            if (scenceId == 1)
            {
                material = "optotypeDownE";
            }
            else if (scenceId == 2)
            {
                material = "optotypeDownC";
            }
            else if (scenceId == 4)
            {
                material = "optotypeDownC_dark";
            }
            else if (scenceId == 3)
            {
                material = "optotypeDownE_dark";
            }
            else if (scenceId == 8)
            {
                material = "optotypeDownC";
            }
        }
        return material;
    }
}
