using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float steerSpeed = 180.0f;
    public float bodySpeed = 5.0f;
    [SerializeField]private int gap = 3;

    private Rigidbody rb;
    private BoxCollider boxCollider;
    private MeshCollider meshCollider;
    private float horizontalInput;
    public GameObject bodyPrefab;
    public GameObject tailPrefab;
    //private GameObject tail;
    private List<GameObject> bodyParts = new List<GameObject>();
    private List<Vector3> positionHistory = new List<Vector3>();
    public void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
        boxCollider = this.GetComponent<BoxCollider>();
        meshCollider = this.GetComponent<MeshCollider>();
    }

    public void Start()
    {
        growSnake();
        tailMove();
    }
    public void FixedUpdate()
    {
        // Move the snake head forward
        transform.position += transform.forward * moveSpeed * Time.deltaTime;

        // Turn based on input
        float steerDirection = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up * steerDirection * steerSpeed * Time.deltaTime);

        // Save head position
        positionHistory.Insert(0, transform.position);

        // Move each body part to follow the previous segment
        for (int i = 0; i < bodyParts.Count; i++)
        {
            Vector3 point = positionHistory[Mathf.Min(i * gap, positionHistory.Count - 1)];
            Vector3 moveDirection = point - bodyParts[i].transform.position;
            bodyParts[i].transform.position += moveDirection * bodySpeed * Time.deltaTime;
            bodyParts[i].transform.LookAt(point);
        }
}

    private void growSnake()
    {
        Vector3 spawnPosition = bodyParts.Count > 0
            ? bodyParts[bodyParts.Count - 1].transform.position
            : transform.position;

        GameObject body = Instantiate(bodyPrefab, spawnPosition, Quaternion.identity);
        bodyParts.Add(body);
    }


    private void tailMove()
    {
        Vector3 spawnPosition = bodyParts.Count > 0
            ? bodyParts[bodyParts.Count - 1].transform.position
            : transform.position;

        GameObject tail = Instantiate(tailPrefab, spawnPosition, Quaternion.identity);
        bodyParts.Add(tail);
    }

}
