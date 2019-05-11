using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance = null;

    public AssemblyManager assemblyManager;
   
    public float CameraRotationSpeed = 4;
    public float CameraSpeed = 40;

    Camera MainCamera;
    float rotX = 0;
    float rotY = 0;

    void Awake()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }


    // Start is called before the first frame update
    void Start()
    {
        MainCamera = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
        //Cursor.lockState = CursorLockMode.Confined;
    }

    // Update is called once per frame
    void Update()
    {
        //Highlight Action
        RaycastHit hit;
        Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            Transform objectHit = hit.transform;
            assemblyManager.HighlightObject(hit.point);
        }

        //Camera Rotation
        rotX += Input.GetAxis("Mouse X") * CameraRotationSpeed;
        rotY += Input.GetAxis("Mouse Y") * CameraRotationSpeed;
        rotY = Mathf.Clamp(rotY, -90f, 90f);
        MainCamera.transform.rotation = Quaternion.Euler(-rotY, rotX, 0f);

        if (Input.GetMouseButtonDown(0))
            assemblyManager.PickObjectUp();

        if (Input.GetMouseButtonUp(0))
            assemblyManager.PlaceObject();

        //CameraMovement
        if (Input.GetKey(KeyCode.W))
        {
            MainCamera.transform.position += MainCamera.transform.forward * CameraSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.S))
        {
            MainCamera.transform.position += -MainCamera.transform.forward * CameraSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.D))
        {
            MainCamera.transform.position += MainCamera.transform.right * CameraSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A))
        {
            MainCamera.transform.position += -MainCamera.transform.right * CameraSpeed * Time.deltaTime;
        }

    }
}
