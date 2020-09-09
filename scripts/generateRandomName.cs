using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class generateRandomName : MonoBehaviour
{

    /*********************************************************************
     * Generate a random alphanumeric strings to identify the test result
     *********************************************************************/
    private static System.Random random = new System.Random();

    public static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
          .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    // Start is called before the first frame update
    void Start()
    {
        // generate a random name
        DBManager.username = RandomString(8);
    }
}
