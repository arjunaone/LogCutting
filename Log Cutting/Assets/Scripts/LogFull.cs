using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogFull : MonoBehaviour
{
    public Main main;
    public float circlePoint;
    public GameObject circleObject;
    public float width;
    public bool collided;
    public bool slowed;
    public bool passed;
    public float proximity;
    public Rigidbody rigid;
    public GameObject logPrefab;
    public GameObject particlePrefab;
    public GameObject particlePrefab2;
    public float scale;
    public float radius;
    public MeshRenderer mesh;
    // Start is called before the first frame update
    void Start()
    {
        main = Main.instance;
        circlePoint = Random.Range(0.3f, 0.7f);
        circleObject.transform.localScale = new Vector3(1f, 1f / scale, 1f);
        circleObject.transform.localPosition = new Vector3(0f, 0f, -width / 2f + circlePoint * width );
        transform.localScale = new Vector3(radius, radius, scale);
    }

    // Update is called once per frame
    void Update()
    {
        if (!collided && !passed)
        {
            if (Mathf.Abs(transform.position.z - main.saw.transform.position.z) < proximity)
            {
                collided = true;
                float cutPoint = 0.5f + (main.saw.transform.position.x - transform.position.x) / (width * scale);
                if (cutPoint > 0f && cutPoint < 1f)
                {
                    main.sawCollider.gameObject.SetActive(true);
                    main.Invoke("DisableSawCollider", 0.35f);
                    CreateLogs(cutPoint);
                }
                if (Mathf.Abs(cutPoint - circlePoint) < 1.2f / (2f * width) / scale)
                {
                    main.score += 10;
                    main.scoreText.text = "SCORE\n" + main.score;
                }
                else
                {
                    main.lives--;
                    main.livesText.text = "LIVES\n" + main.lives;
                    if (main.lives < 1)
                    {
                        main.gameEnded = true;
                        main.menuPanel.SetActive(true);
                    }
                }
                Destroy(gameObject);
            }
        }
        if (!slowed)
        {
            float sawDistance = Mathf.Abs(transform.position.x - main.saw.transform.position.x);
            if (Mathf.Abs(transform.position.z - main.saw.transform.position.z) < proximity + 0.9f && !passed)
            {
                if (sawDistance < width * scale / 2f)
                {
                    float mass = scale * radius;
                    float shakePower = -0.01f + mass / 2.4f;
                    main.ShakeCamera(shakePower);
                    main.sawLocked = true;
                    main.Invoke("DisableSawLock", 0.8f);
                    main.soundPlayer.pitch = (1f-radius/2f + 0.5f ) * 0.8f + 0.1f;
                    main.soundPlayer.PlayOneShot(main.cutSound);
                    slowed = true;
                    rigid.velocity = rigid.velocity / 15f;
                    rigid.angularVelocity = rigid.angularVelocity / 24f;
                    GameObject particle = Instantiate(particlePrefab, new Vector3(main.saw.transform.position.x, main.particleSpawn.position.y, main.particleSpawn.position.z), Quaternion.Euler(-96f, 0f, 0f));
                    GameObject particle2 = Instantiate(particlePrefab2, new Vector3(main.saw.transform.position.x, main.particleSpawn.position.y, main.particleSpawn.position.z), Quaternion.Euler(-96f, 0f, 0f));
                }
                else
                {
                    main.lives--;
                    main.livesText.text = "LIVES\n" + main.lives;
                    if (main.lives < 1)
                    {
                        main.gameEnded = true;
                        main.menuPanel.SetActive(true);
                    }
                    passed = true;
                }
            }
        }
        if (transform.position.y < -50f)
        {
            Destroy(gameObject);
        }
    }

    public void CreateLogs(float cutPoint)
    {
        GameObject leftLog = Instantiate(logPrefab, transform.position, Quaternion.Euler(0f,90f,transform.rotation.eulerAngles.z));
        LogHalf logScr = leftLog.GetComponent<LogHalf>();
        logScr.origin = transform.position;
        logScr.side = 1f;
        logScr.yScale = 1f-cutPoint;
        logScr.saw = transform.position;
        logScr.angularVelocity = rigid.angularVelocity;
        logScr.velocity = rigid.velocity;
        logScr.circlePoint = circlePoint;
        logScr.scale = scale;
        logScr.radius = radius;
        logScr.origin = transform.position;
        logScr.mesh.material.mainTexture = mesh.material.mainTexture;
        GameObject leftLog2 = Instantiate(logPrefab, transform.position, Quaternion.Euler(0f, 90f, transform.rotation.eulerAngles.z));
        LogHalf logScr2 = leftLog2.GetComponent<LogHalf>();
        logScr2.origin = transform.position;
        logScr2.side = -1f;
        logScr2.yScale = cutPoint;
        logScr2.saw = transform.position;
        logScr2.angularVelocity = rigid.angularVelocity;
        logScr2.velocity = rigid.velocity;
        logScr2.circlePoint = circlePoint;
        logScr2.scale = scale;
        logScr2.radius = radius;
        logScr2.origin = transform.position;
        logScr2.mesh.material.mainTexture = mesh.material.mainTexture;
    }
}
