using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class Snake : MonoBehaviour
{
    // Score
    public int score;
    public Text TextScore;

    bool ate = false;

    public GameObject tailPrefab;

    public GenerateFood food;

    // Direction to right
    Vector2 dir = Vector2.right;

    // Snake body list
    List<Transform> tail = new List<Transform>();

    // Use this for initialization
    void Start()
    {
        // Move the Snake every 300ms
        InvokeRepeating("Move", 0, 0.3f);
        food.Generate();
    }

    // Update is called once per frame
    void Update()
    {
        if (dir != Vector2.left && Input.GetKey(KeyCode.RightArrow))
            dir = Vector2.right;
        else if (dir != Vector2.up && Input.GetKey(KeyCode.DownArrow))
            dir = -Vector2.up;
        else if (dir != Vector2.right && Input.GetKey(KeyCode.LeftArrow))
            dir = -Vector2.right;
        else if (dir != Vector2.down && Input.GetKey(KeyCode.UpArrow))
            dir = Vector2.up;
    }

    void Move()
    {
        Vector2 v = transform.position;
        transform.Translate(dir);

        // Have eatten something
        if (ate)
        {
            GameObject g = Instantiate<GameObject>(tailPrefab, v, Quaternion.identity);

            // Insert to body list
            tail.Insert(0, g.transform);

            ate = false;
        }

        if (tail.Count > 0)
        {
            // Move Last Tail Element to where the Head was
            tail.Last().position = v;

            // Add to front of list, remove from the back
            tail.Insert(0, tail.Last());
            tail.RemoveAt(tail.Count - 1);
        }
    }

    void OnTriggerEnter(Collider coll) // check collision
    {
        // Collision with Food
        if (coll.name.StartsWith("Food"))
        {
            ate = true;
            score++;
            TextScore.text = score.ToString();

            // Destroy the Food
            Destroy(coll.gameObject);

            // And generate again
            food.Generate();
        }
        // Collision with border OR itself
        else if (coll.name.StartsWith("Border") || coll.name.StartsWith("Tail"))
        {
            CancelInvoke("Move");
        }
    }
}
