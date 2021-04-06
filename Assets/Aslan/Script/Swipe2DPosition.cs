using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Swipe2DPosition : MonoBehaviour
{
    private Vector3 touchPosition;
    private Vector3 direction;
    private Rigidbody2D rb;
    private float speed = 10f;

    public Text text;

    private Vector3 position;
    private float width;
    private float height;

    private void Awake()
    {
        width = (float)Screen.width / 2.0f;
        height = (float)Screen.height / 2.0f;
    }

    void Update()
    {/*
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved && transform.gameObject.tag == "Ans")
            {
                //Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                //Debug.Log("Ans");
                transform.position = new Vector3(transform.position.x + touch.deltaPosition.x * speed, transform.position.y + touch.deltaPosition.y * speed, transform.position.z);
                text.text = "Ans: " + transform.position;
            }
            if (touch.phase == TouchPhase.Moved && transform.gameObject.tag == "Position")
            {
                //Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                //Debug.Log("Ans");
                transform.position = new Vector3(transform.position.x - touch.deltaPosition.x * speed, transform.position.y - touch.deltaPosition.y * speed, transform.position.z);
                text.text = "Pos " + transform.position;
            }
            
            touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            touchPosition.z = 0;
            direction = touchPosition - transform.position;
            rb.velocity = new Vector2(direction.x, direction.y) * speed;

            if (touch.phase == TouchPhase.Ended)
                rb.velocity = Vector2.zero;
            
        }
        */
        // Handle screen touches.
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Move the cube if the screen has the finger moving.
            if (touch.phase == TouchPhase.Moved)
            {
                Vector2 pos = touch.position;
                pos.x = (pos.x - width) / width;
                pos.y = (pos.y - height) / height;
                position = new Vector3(-pos.x, pos.y, 0.0f);

                // Position the cube.
                transform.position = position;

                text.text = "Pos " + transform.position;
            }

            if (touch.phase == TouchPhase.Began)
            {
                // Construct a ray from the current touch coordinates
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray))
                {
                    // Create a particle if hit
                    //Instantiate(particle, transform.position, transform.rotation);
                }
            }
        }
        }
}
