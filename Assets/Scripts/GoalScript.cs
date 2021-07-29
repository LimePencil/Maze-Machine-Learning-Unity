using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalScript : MonoBehaviour
{
    
    public static bool done = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            done = true;
            Debug.Log("Done!");
            GameObject gameObject = GameObject.FindWithTag("Player");
            gameObject.transform.position = new Vector3(0,0.27f,0);
        }
    }
}
