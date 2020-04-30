using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BallController : MonoBehaviour
{
    Rigidbody rb;

    Vector2 lastMousePos;

    public float thrust = 100f;
    public float speed = 5f;

    public GameObject winPanel;

    [SerializeField] float wallDistance = 5f;
    [SerializeField] float minCamDistance = 4f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 deltaPos = Vector2.zero;

        if (Input.GetMouseButton(0))
        {
            Vector2 currentMousePos = Input.mousePosition;

            if (lastMousePos == Vector2.zero)
                lastMousePos = currentMousePos;

            deltaPos = currentMousePos - lastMousePos;

            lastMousePos = currentMousePos;

            Vector3 force = new Vector3(deltaPos.x, 0, deltaPos.y) * thrust;
            rb.AddForce(force);
        }
        else
        {
            lastMousePos = Vector2.zero;
        }
    }



    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + Vector3.forward * speed * Time.fixedDeltaTime);
        Camera.main.transform.position += Vector3.forward * speed * Time.fixedDeltaTime;
    }

    // Constraints
    private void LateUpdate()
    {
        Vector3 pos = transform.position;

        // If it's on the left edge
        if (pos.x < -wallDistance)
        {
            pos.x = -wallDistance;
        }
        // If it's on the right edge
        else if (pos.x > wallDistance)
        {
            pos.x = wallDistance;
        }

        if (pos.z < Camera.main.transform.position.z + minCamDistance)
        {
            pos.z = Camera.main.transform.position.z + minCamDistance;
        }

        transform.position = pos;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Evil")
            StartCoroutine(Die(2));
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Goal")
            StartCoroutine(Win(0.5f));
    }

    IEnumerator Die(float delayTime)
    {
        // Do stuff before replaying the level 
        Debug.Log("You're dead");

        // Stop the Player from moving
        speed = 0;
        thrust = 0;

        // Wait some seconds
        yield return new WaitForSeconds(delayTime);

        // Do stuff after waiting some seconds

        //Replay the Level
        SceneManager.LoadScene(0);
    }
    IEnumerator Win(float delayTime)
    {
        // Do stuff before replaying the level 
        Debug.Log("You're dead");

        // Stop the Player from moving
        speed = 0;
        thrust = 0;
        rb.velocity = Vector3.zero;
        
        // Wait some seconds
        yield return new WaitForSeconds(delayTime);

        // Do stuff after waiting some seconds

        winPanel.SetActive(true);

    }
}
