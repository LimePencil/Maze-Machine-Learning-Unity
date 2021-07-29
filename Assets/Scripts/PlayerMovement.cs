using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public float movementSpeed = 1f;
    public float rotationSpeed = 1f;
    public float movementInput;
    public float rotationInput;
    public bool isHuman = true;
    public Rigidbody player;
    private GameObject playerObj;
    private Vector3 spawnPoint;
    ToPython py;
    void Start()
    {
        player =  GetComponent<Rigidbody>();
        playerObj = GameObject.FindGameObjectWithTag("Player");
        py = this.gameObject.GetComponent<ToPython>();
        spawnPoint = new Vector3(0, 0.27f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (isHuman)
        {
            movementInput = Input.GetAxis("Vertical");
            rotationInput = Input.GetAxis("Horizontal");
        }
    }
    public void UseInput()
    {
        if(!isHuman)
        {
            float[] arr = py.getAgentInput();
            movementInput = arr[0];
            rotationInput = arr[1];
        }
        if (py.reset)
        {
            Debug.Log("Reset!");
            playerObj.transform.position = spawnPoint;
            py.reset = false;
        }
    }
    public void Movement()
    {
        Move();
        Rotate();
    }

    private void FixedUpdate()
    {
        Move();
        Rotate();
    }
    private void Move()
    {
        Vector3 movement = transform.forward *  movementSpeed * movementInput * Time.deltaTime;
        player.MovePosition(player.position + movement);
    }
    private void Rotate()
    {
        float turn = rotationInput * rotationSpeed * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        player.MoveRotation(player.rotation * turnRotation);
    }
}
