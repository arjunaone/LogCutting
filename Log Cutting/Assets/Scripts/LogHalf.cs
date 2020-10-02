using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogHalf : MonoBehaviour
{
    public MeshRenderer mesh;
    public float yScale;
    public float yOffset;
    public GameObject circle;
    public float side;
    public Vector3 origin;
    public float circlePoint;
    public Rigidbody rigid;
    public Vector3 saw;
    public Vector3 angularVelocity;
    public Vector3 velocity;
    public float logWidth;
    public float scale;
    public float radius;
    // Start is called before the first frame update
    void Start()
    {
        rigid.maxAngularVelocity = 2000f;
        transform.localScale = new Vector3(radius, radius, yScale*scale);
        yOffset = (1f - yScale) * (side + 1f) * 0.5f;
        if (side == -1)
        {
            transform.position = origin + new Vector3((logWidth * scale / 2f - yScale * logWidth * scale / 2f) * side, 0f, 0f);
            float circleStart = -logWidth/2f + (circlePoint/yScale)* logWidth - (0.5f / yScale/ scale);
            float circleWidth = Mathf.Clamp(logWidth / 2f - circleStart , 0f, 1f/yScale / scale);
            float circleCenter = circleStart + circleWidth / 2f; 
            circle.transform.localPosition = new Vector3(circle.transform.localPosition.x, circle.transform.localPosition.y, circleCenter);
            circle.transform.localScale = new Vector3(circle.transform.localScale.x, circleWidth, circle.transform.localScale.z);
        }
        else
        {
            transform.position = origin + new Vector3((logWidth * scale / 2f - yScale * logWidth * scale / 2f) * side, 0f, 0f);
            float circleEnd = circlePoint + (0.5f / logWidth / scale);
            float circleWidth = Mathf.Clamp(circleEnd - 1f + yScale, 0f, (1f / logWidth / scale)) ;
            float circleCenter = circleEnd -(1f - yScale / 2f) - circleWidth / 2f;
            circle.transform.localPosition = new Vector3(circle.transform.localPosition.x, circle.transform.localPosition.y, circleCenter * logWidth / yScale);
            circle.transform.localScale = new Vector3(circle.transform.localScale.x, circleWidth * logWidth / yScale, circle.transform.localScale.z);
        }
        mesh.materials[0].SetTextureScale("_MainTex", new Vector2(1f, yScale));
        mesh.materials[0].SetTextureOffset("_MainTex", new Vector2(1f, yOffset));
        rigid.angularVelocity = angularVelocity;
        rigid.velocity = velocity;
        rigid.AddTorque(Vector3.up * Random.Range(500f,900f) * side * (1f- yScale));
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -50f)
        {
            Destroy(gameObject);
        }
    }
}
