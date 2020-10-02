using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    public GameObject saw;
    public GameObject sawCollider;
    public Transform particleSpawn;
    public Transform logSpawn;
    public int screenWidth;
    public GameObject logPrefab;
    public static Main instance;
    public bool sawLocked;
    public AudioSource soundPlayer;
    public AudioSource loopPlayer;
    public AudioClip cutSound;
    public bool trapOpen;
    public GameObject trap1;
    public GameObject trap2;
    public Transform[] trapTransforms;
    public Camera cam;
    public Texture2D[] textures;
    public GameObject menuPanel;
    public bool menuOpen;
    public int lives = 3;
    public int score = 0;
    public Text scoreText;
    public Text livesText;
    public bool gameEnded;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        screenWidth = Screen.width;
        StartCoroutine(LogSpawner(3f));
        StartCoroutine(DoorHandler(9f));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (menuOpen && !gameEnded)
            {
                menuOpen = false;
                menuPanel.SetActive(false);
            }
            else
            {
                menuOpen = true;
                menuPanel.SetActive(true);
            }
        }
        if (!sawLocked)
        {
            float sawPositionX = (-0.5f + (Input.mousePosition.x / (float)screenWidth)) * 18f;
            saw.transform.position = new Vector3(Mathf.Clamp(Mathf.Lerp(saw.transform.position.x, sawPositionX, Time.deltaTime*8f), -6f, 6f), saw.transform.position.y, saw.transform.position.z);
        }
        saw.transform.Rotate(new Vector3(0f, -Time.deltaTime*380f, 0f));
        if (trapOpen)
        {
            trap1.transform.rotation = Quaternion.Lerp(trap1.transform.rotation, trapTransforms[1].rotation, Time.deltaTime * 8f);
            trap2.transform.rotation = Quaternion.Lerp(trap2.transform.rotation, trapTransforms[3].rotation, Time.deltaTime * 8f);
        }
        else
        {
            trap1.transform.rotation = Quaternion.Lerp(trap1.transform.rotation, trapTransforms[0].rotation, Time.deltaTime * 8f);
            trap2.transform.rotation = Quaternion.Lerp(trap2.transform.rotation, trapTransforms[2].rotation, Time.deltaTime * 8f);
        }
    }

    public void DisableSawCollider()
    {
        sawCollider.SetActive(false);
        
    }

    public void DisableSawLock()
    {
        sawLocked = false;
    }

    public void ShakeCamera(float power)
    {
        cam.DOShakePosition(1f, power, 15, 90f, true);
    }

    public IEnumerator LogSpawner(float time)
    {
        float tmpScale = Random.Range(0.3f, 0.8f);
        GameObject tmpObj = Instantiate(logPrefab, logSpawn.position + new Vector3(Random.Range(-1.5f / tmpScale, 1.5f / tmpScale), 0f,0f), Quaternion.Euler(0f,90f,0f));
        LogFull log = tmpObj.GetComponent<LogFull>();
        log.scale = tmpScale;
        log.radius = Random.Range(0.2f, 0.5f) / log.scale;
        log.mesh.material.mainTexture = textures[Random.Range(0, 2)];
        yield return new WaitForSeconds(time);
        if (!gameEnded)
        {
            StartCoroutine(LogSpawner(2.5f));
        }
    }

    public IEnumerator DoorHandler(float time)
    {
        
        yield return new WaitForSeconds(time);
        if (trapOpen)
        {
            trapOpen = false;
            
            StartCoroutine(DoorHandler(10f));
        }
        else
        {
            trapOpen = true;
            
            StartCoroutine(DoorHandler(2.5f));
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
