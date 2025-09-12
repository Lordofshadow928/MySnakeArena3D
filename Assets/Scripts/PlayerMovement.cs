using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float steerSpeed = 180.0f;
    public float bodySpeed = 5.0f;
    [SerializeField]private int gap = 9;

    private Rigidbody rb;
    private BoxCollider boxCollider;
    private MeshCollider meshCollider;
    private float horizontalInput;
    public GameObject bodyPrefab;
    public GameObject tailPrefab;

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
        growSnake();
        growSnake();
        growSnake();
        growSnake();
        growSnake();
        tailMove();
    }
    public void FixedUpdate()
    {
        transform.position +=transform.forward * moveSpeed * Time.deltaTime;
        float steerDirection = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up * steerDirection * steerSpeed * Time.deltaTime);

        positionHistory.Insert(0, transform.position);

        int index = 0;
        foreach (var body in bodyParts)
        {
            Vector3 point = positionHistory[Mathf.Min(index * gap, positionHistory.Count - 1)];
            Vector3 moveDirection = point - body.transform.position;
            body.transform.position += moveDirection * bodySpeed * Time.deltaTime;
            body.transform.LookAt(point);
            index++;
        }

        foreach (var tail in bodyParts)
        {
            Vector3 point = positionHistory[Mathf.Min(index * gap, positionHistory.Count - 1)];
            Vector3 moveDirection = point - tail.transform.position;
            tail.transform.position += moveDirection * bodySpeed * Time.deltaTime;
            tail.transform.LookAt(point);
            index++;
        }
    }

    private void growSnake()
    {
        GameObject body = Instantiate(bodyPrefab);
        bodyParts.Add(body);

    }
    private void tailMove()
    {
        GameObject tail = Instantiate(tailPrefab);
        bodyParts.Add(tail);

    }
}
