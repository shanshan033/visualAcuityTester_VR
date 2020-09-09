using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public Canvas enterCanvas;
    public Canvas keyboardCanvas;
    public Canvas projectInfoCanvas;
    public Canvas consentCanvas;
    public Canvas consentCanvas1;
    public Canvas consentCanvas2;

    // Start is called before the first frame update
    void Start()
    {
        enterCanvas.enabled = false;
        keyboardCanvas.enabled = false;
        projectInfoCanvas.enabled = false;
        consentCanvas.enabled = true;
        consentCanvas1.enabled = true;
        consentCanvas2.enabled = true;

    }

    public void showAskCanvas()
    {
        print("click ask canvas");
        enterCanvas.enabled = false;
        keyboardCanvas.enabled = false;
        projectInfoCanvas.enabled = false;
        consentCanvas.enabled = false;
        consentCanvas1.enabled = false;
        consentCanvas2.enabled = false;
    }


    public void showEnterCanvas()
    {
       
        enterCanvas.enabled = true;
        keyboardCanvas.enabled = true;
        projectInfoCanvas.enabled = false;
        consentCanvas.enabled = false;
        consentCanvas1.enabled = false;
        consentCanvas2.enabled = false;
    }

    public void showCalibrationCanvas()
    {
        enterCanvas.enabled = false;
        enterCanvas.enabled = false;
        projectInfoCanvas.enabled = false;
        consentCanvas.enabled = false;
    }

    public void showProjectInfoCanvas()
    {
        print("click project info canvas");
        projectInfoCanvas.enabled = true;
        enterCanvas.enabled = false;
        enterCanvas.enabled = false;
        consentCanvas.enabled = false;
        consentCanvas1.enabled = false;
        consentCanvas2.enabled = false;
    }

    public void refuseExperiment()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("finishPage");
    }

}
