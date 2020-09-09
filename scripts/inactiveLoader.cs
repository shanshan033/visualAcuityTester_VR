using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inactiveLoader : MonoBehaviour
{
    public GameObject loader;
    // Start is called before the first frame update
    void Start()
    {
        loader.SetActive(false);
        
    }

}
