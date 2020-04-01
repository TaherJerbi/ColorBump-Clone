using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BallController : MonoBehaviour
{

    [SerializeField] float thrust = 150f;
    [SerializeField] float speed = 5f;
    Rigidbody rb;

    Vector2 lastMousePos;

    [SerializeField] float wallDistance=5f;
    [SerializeField] float minCamDistance = 4f;

    bool dead = false;

    public GameObject winPanel;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Swipe Movement
    void Update()
    {
        if (dead)
            return;

        Vector2 deltaPos = Vector2.zero;

        if(Input.GetMouseButton(0))
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
        if (dead)
            return;

        Vector3 pos = transform.position;

        if (pos.x < -wallDistance)
        {
            pos.x = -wallDistance;
        }else if(pos.x > wallDistance)
        {
            pos.x = wallDistance;
        }

        if (pos.z < Camera.main.transform.position.z + minCamDistance)
        {
            pos.z = Camera.main.transform.position.z + minCamDistance;
        }

        transform.position = pos;
    }

    private void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "Evil")
        {
            dead = true;
            StartCoroutine(EndGame(2f));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Goal")
            StartCoroutine(EndGame(2f));
    }
    IEnumerator EndGame(float delayTime)
    {
        if (dead)
            Debug.Log("you lost");
        else
        {
            winPanel.SetActive(true);
            thrust = 0;
            speed = 0;
        }

        yield return new WaitForSeconds(delayTime);
        if (dead)
            SceneManager.LoadScene(0);

        
    }
}
