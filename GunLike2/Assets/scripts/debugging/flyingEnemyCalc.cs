using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flyingEnemyCalc : MonoBehaviour
{
    public LayerMask layers;
    public float rayDistance;
    public float rayAngle;

    public bool multiTurn;
    public bool drawDebug;

    public float speed;
    public float maxSpeed;

    public float turnSpeed;

    Ray centerRay;
    float centerDistance;
    string centerTag;

    Ray upRay;
    float upDistance;
    string upTag;

    Ray downRay;
    float downDistance;
    string downTag;

    Ray leftRay;
    float leftDistance;
    string leftTag;

    Ray rightRay;
    float rightDistance;
    string rightTag;

    List<float> distances = new List<float>();
    string closestRay;

    GameObject eye;
    GameObject player;
    Rigidbody rb;

    void Start()
    {
        player = GameObject.Find("Player");
        eye = transform.GetChild(0).gameObject;
        rb = gameObject.GetComponent<Rigidbody>();

        centerRay = new Ray(eye.transform.position, transform.forward);
        upRay = new Ray(eye.transform.position, transform.forward + (transform.up / rayAngle));
        downRay = new Ray(eye.transform.position, transform.forward + (transform.up / -rayAngle));
        leftRay = new Ray(eye.transform.position, transform.forward + (transform.right / -rayAngle));
        rightRay = new Ray(eye.transform.position, transform.forward + (transform.right / rayAngle));
    }

    void Update()
    {
        UpdateRays();

        Turn();
    }

    void Move()
    {
        if(closestRay == "center")
        {
            rb.AddForce(transform.forward * -speed * 1.5f * Time.deltaTime, ForceMode.Acceleration);
        }
        else
        {
            rb.AddForce(transform.forward * speed * Time.deltaTime, ForceMode.Acceleration);
        }
        
        if(rb.velocity.magnitude > maxSpeed) { rb.velocity = rb.velocity / 1.1f; }
    }

    void Turn()
    {
        if(closestRay == "up" || multiTurn) { rb.AddTorque(Vector3.left * turnSpeed * ((rayDistance - upDistance) / rayDistance) * Time.deltaTime); }
        if(closestRay == "down" || multiTurn) { rb.AddTorque(Vector3.right * turnSpeed * ((rayDistance - downDistance) / rayDistance) * Time.deltaTime); }
        if(closestRay == "left" || multiTurn) { rb.AddTorque(Vector3.up * turnSpeed * ((rayDistance - leftDistance) / rayDistance) * Time.deltaTime); }
        if(closestRay == "right" || multiTurn) { rb.AddTorque(Vector3.down * turnSpeed * ((rayDistance - rightDistance) / rayDistance) * Time.deltaTime); }

        if(closestRay == "NONE") 
        {
            Vector3 curRotation = transform.eulerAngles;
            transform.LookAt(player.transform);
            Move();
            Vector3 tarRotation = transform.eulerAngles;
            transform.eulerAngles = curRotation;

            transform.Rotate(Vector3.Normalize(tarRotation - curRotation) * (turnSpeed * 15f) * Time.deltaTime);
            //transform.eulerAngles = tarRotation;
            //rb.AddTorque(Vector3.Normalize(tarRotation - curRotation) * turnSpeed * Time.deltaTime);
        }

        //transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
    }

    void DebugDraw()
    {
        if(closestRay == "center") { Debug.DrawRay(centerRay.origin, centerRay.direction * rayDistance / 2f, Color.cyan); }
        else if(centerTag != "NONE") { Debug.DrawRay(centerRay.origin, centerRay.direction * rayDistance / 2f, new Color(Mathf.Lerp(1f,0f,centerDistance/rayDistance), Mathf.Lerp(0f, 1f, centerDistance / rayDistance), 0f)); }
        else { Debug.DrawRay(centerRay.origin, centerRay.direction * rayDistance / 2f, Color.green); }

        if (closestRay == "up") { Debug.DrawRay(upRay.origin, upRay.direction * rayDistance, Color.cyan); }
        else if (upTag != "NONE") { Debug.DrawRay(upRay.origin, upRay.direction * rayDistance, new Color(Mathf.Lerp(1f, 0f, upDistance / rayDistance), Mathf.Lerp(0f, 1f, upDistance / rayDistance), 0f)); }
        else { Debug.DrawRay(upRay.origin, upRay.direction * rayDistance, Color.green); }

        if (closestRay == "down") { Debug.DrawRay(downRay.origin, downRay.direction * rayDistance, Color.cyan); }
        else if (downTag != "NONE") { Debug.DrawRay(downRay.origin, downRay.direction * rayDistance, new Color(Mathf.Lerp(1f, 0f, downDistance / rayDistance), Mathf.Lerp(0f, 1f, downDistance / rayDistance), 0f)); }
        else { Debug.DrawRay(downRay.origin, downRay.direction * rayDistance, Color.green); }

        if (closestRay == "left") { Debug.DrawRay(leftRay.origin, leftRay.direction * rayDistance, Color.cyan); }
        else if (leftTag != "NONE") { Debug.DrawRay(leftRay.origin, leftRay.direction * rayDistance, new Color(Mathf.Lerp(1f, 0f, leftDistance / rayDistance), Mathf.Lerp(0f, 1f, leftDistance / rayDistance), 0f)); }
        else { Debug.DrawRay(leftRay.origin, leftRay.direction * rayDistance, Color.green); }

        if (closestRay == "right") { Debug.DrawRay(rightRay.origin, rightRay.direction * rayDistance, Color.cyan); }
        else if (rightTag != "NONE") { Debug.DrawRay(rightRay.origin, rightRay.direction * rayDistance, new Color(Mathf.Lerp(1f, 0f, rightDistance / rayDistance), Mathf.Lerp(0f, 1f, rightDistance / rayDistance), 0f)); }
        else { Debug.DrawRay(rightRay.origin, rightRay.direction * rayDistance, Color.green); }
    }

    void UpdateRays()
    {
        if (drawDebug) { DebugDraw(); }
        centerRay = new Ray(eye.transform.position, transform.forward);
        upRay = new Ray(eye.transform.position, transform.forward + (transform.up / rayAngle));
        downRay = new Ray(eye.transform.position, transform.forward + (transform.up / -rayAngle));
        leftRay = new Ray(eye.transform.position, transform.forward + (transform.right / -rayAngle));
        rightRay = new Ray(eye.transform.position, transform.forward + (transform.right / rayAngle));

        distances.Clear();

        if (Physics.Raycast(centerRay, out RaycastHit hit, rayDistance / 2f, layers))
        {
            centerDistance = hit.distance;
            centerTag = hit.transform.gameObject.tag;
        }
        else
        {
            centerDistance = 999f;
            centerTag = "NONE";
        }
        distances.Add(centerDistance);

        if (Physics.Raycast(upRay, out hit, rayDistance, layers))
        {
            upDistance = hit.distance;
            upTag = hit.transform.gameObject.tag;
        }
        else
        {
            upDistance = 999f;
            upTag = "NONE";
        }
        distances.Add(upDistance);

        if (Physics.Raycast(downRay, out hit, rayDistance, layers))
        {
            downDistance = hit.distance;
            downTag = hit.transform.gameObject.tag;
        }
        else
        {
            downDistance = 999f;
            downTag = "NONE";
        }
        distances.Add(downDistance);

        if (Physics.Raycast(leftRay, out hit, rayDistance, layers))
        {
            leftDistance = hit.distance;
            leftTag = hit.transform.gameObject.tag;
        }
        else
        {
            leftDistance = 999f;
            leftTag = "NONE";
        }
        distances.Add(leftDistance);

        if (Physics.Raycast(rightRay, out hit, rayDistance, layers))
        {
            rightDistance = hit.distance;
            rightTag = hit.transform.gameObject.tag;
        }
        else
        {
            rightDistance = 999f;
            rightTag = "NONE";
        }
        distances.Add(rightDistance);

        distances.Sort();

        if(distances[0] != 999f)
        {
            if(centerDistance == distances[0]) { closestRay = "center"; }
            if(upDistance == distances[0]) { closestRay = "up"; }
            if(downDistance == distances[0]) { closestRay = "down"; }
            if(leftDistance == distances[0]) { closestRay = "left"; }
            if(rightDistance == distances[0]) { closestRay = "right"; }
        }
        else
        {
            closestRay = "NONE";
        }
    }
}
