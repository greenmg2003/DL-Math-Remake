using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CubeMovement : MonoBehaviour
{
    public GameObject Actor;
    public float speed;
    private Vector3 pos;
    public Material mat;
    private int loopCount = 1;
    public bool onGround = true;
    public bool isAlive = true;
    public float distFromGround = 0.6f;
    public Animator cam;
    public AudioClip MusicClip;
    public AudioSource MusicSource;
    public AudioSource music2;
    public AudioClip dieClip;
    public AudioSource dieSource;
    public AudioClip waterClip;
    public AudioSource waterSource;
    public Animator Endscreen;
    private bool Started = false;
    public Canvas EndScreen;
    Camera maincam;

    private void Awake()
    {
        maincam = Camera.main;
    }

    void Start()
    {
        speed = 12.936f;
        MusicSource.clip = MusicClip;
        loopCount = 1;
        onGround = true;
        isAlive = true;
        cam.enabled = false;
        Endscreen.enabled = false;
        EndScreen.enabled = false;
    }


    void Update()
    {
        if (Started == false)
        {
            if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                Started = true;
                MusicSource.Play();
            }
        }
        if (isAlive == true && Started == true) ///Control
        {
            cam.enabled = true;
            RenderSettings.fogColor = maincam.backgroundColor;
            onGround = isGrounded();
            pos = Actor.transform.position;
            Actor.transform.Translate((Vector3.right * speed) * Time.deltaTime);
            if (onGround == true)
            {
                GameObject Actor2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
                Actor2.transform.position = pos;
                Actor2.GetComponent<MeshRenderer>().material = mat;
                Actor2.GetComponent<BoxCollider>().isTrigger = true;
		        Actor2.tag = "cubes";
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.UpArrow))
                {
                    if (loopCount % 2 != 0)
                    {
                        Actor.transform.eulerAngles = new Vector3(0, 180, 0);
                        loopCount++;
                    }
                    else
                    {
                        Actor.transform.eulerAngles = new Vector3(0, 90, 0);
                        loopCount++;
                    }
                }
            }
        }
    }
    public bool isGrounded() ///Check whether the player is on ground
    {
        return Physics.Raycast(Actor.transform.position, Vector3.down, distFromGround);
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Obstacles")
        {
            isAlive = false;
            music2.Pause();
            dieSource.Play();
            cam.enabled = false;
            Endscreen.enabled = true;
            EndScreen.enabled = true;
            GameObject prime = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GameObject prime1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GameObject prime2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            prime.transform.position = new Vector3(Actor.transform.position.x, Actor.transform.position.y + 1, Actor.transform.position.z);
            prime1.transform.position = new Vector3(Actor.transform.position.x - 1, Actor.transform.position.y, Actor.transform.position.z);
            prime2.transform.position = new Vector3(Actor.transform.position.x + 1, Actor.transform.position.y, Actor.transform.position.z);
            prime.AddComponent<Rigidbody>().velocity = Random.onUnitSphere * 50;
            prime1.AddComponent<Rigidbody>().velocity = Random.onUnitSphere * 50;
            prime2.AddComponent<Rigidbody>().velocity = Random.onUnitSphere * 50;
            prime.GetComponent<MeshRenderer>().material = mat;
            prime1.GetComponent<MeshRenderer>().material = mat;
            prime2.GetComponent<MeshRenderer>().material = mat;
	        prime.tag = "cubes";
	        prime1.tag = "cubes";
	        prime2.tag = "cubes";
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Gem"))
        {
            other.gameObject.SetActive(false);
        }
        if (other.gameObject.CompareTag("Crown"))
        {
            other.gameObject.SetActive(false);
        }
        if (other.gameObject.CompareTag("Ending angle"))
        {
            {
                Actor.transform.eulerAngles = new Vector3(0, 135, 0);
                loopCount++;
            }
        }
        if (other.gameObject.CompareTag("End screen"))
        {
            {
                Endscreen.enabled = true;
                isAlive = false;
                EndScreen.enabled = true;
            }
        }
        if (other.gameObject.CompareTag("DisableCam"))
        {
            {
                cam.enabled = false;
            }
        }
    }
}
