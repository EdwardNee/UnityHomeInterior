using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


public class ProgramManager : MonoBehaviour
{
    [Header("Put your planemarker here.")]
    [SerializeField]
    private GameObject planeMarkerPrefab;
    private ARRaycastManager ARRaycastManagerScript;

    private Vector2 touchPos;

    public GameObject objToSpawn;

    public bool isChoosing;

    [Header("Put your ScrollView here.")]
    public GameObject scrollView;

    [SerializeField]
    private Camera ARCamera;

    //���� �������� �������, ������� ��������� ���.
    List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private GameObject selectedObject;

    // ��� ��������� ���� �� �.
    private Quaternion YRotQuat;
    //������ �� ���� ������ ������.
    public bool rotatable;

    //������ �� ���� ������ ������.
    public bool deletable;

    // Start is called before the first frame update
    void Start()
    {
        rotationScript = FindObjectOfType<Rotation>();
        deletetionScript = FindObjectOfType<Deletion>();
        scrollView.SetActive(false);

        //������� ������.
        ARRaycastManagerScript = FindObjectOfType<ARRaycastManager>();

        planeMarkerPrefab.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isChoosing)
        {
            ShowMarkerToSetObject();
        }
        RotateObject();
        DeleteObject();
        MoveObject();
    }

    private void ShowMarkerToSetObject()
    {
        //������� ���, �� ����� � ������ ������.
        Vector2 vec = new Vector2(Screen.width / 2, Screen.height / 2);
        //�������� ������� � List<> hits, ��������� ���������.
        ARRaycastManagerScript.Raycast(vec, hits, TrackableType.Planes);

        if (hits.Count > 0)
        {
            //����������� �������� planeMarker ��� ��� ��������� � ����������.
            planeMarkerPrefab.transform.position = hits[0].pose.position;
            planeMarkerPrefab.SetActive(true);
        }

        //������ ������. �������� ������ ��� ���������
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            //������ ���, ��� ��� ��������� � ����������
            Instantiate(objToSpawn, hits[0].pose.position, objToSpawn.transform.rotation);
            isChoosing = false;
            planeMarkerPrefab.SetActive(false);
        }
    }


    /// <summary>
    /// ����� ��� ����������� ������� �� ����� � ������� �������� ������.
    /// </summary>
    private void MoveObject()
    {
        if (Input.touchCount == 1 && !rotatable && !deletable)
        {
            Touch touch = Input.GetTouch(0);
            touchPos = touch.position;

            //��� �������� ������.
            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = ARCamera.ScreenPointToRay(touch.position);

                //����� ������� ������� ���.
                RaycastHit hitObject;

                //���� ����� ������ � ������� ���, �� ������ ���.
                if (Physics.Raycast(ray, out hitObject))
                {
                    if (hitObject.collider.CompareTag("Unselected"))
                    {
                        hitObject.collider.gameObject.tag = "Selected";
                    }
                }
            }

            //��� ���������� "Selected" ������.
            if (touch.phase == TouchPhase.Moved)
            {
                ARRaycastManagerScript.Raycast(touchPos, hits, TrackableType.Planes);
                selectedObject = GameObject.FindWithTag("Selected");
                selectedObject.transform.position = hits[0].pose.position;
            }

            if (touch.phase == TouchPhase.Ended)
            {
                if (selectedObject.CompareTag("Selected"))
                {
                    selectedObject.tag = "Unselected";
                }
            }
        }
    }

    private Rotation rotationScript;
    /// <summary>
    /// ����� ��� �������� ������� �� �����.
    /// </summary>
    private void RotateObject()
    {
        if (Input.touchCount > 0)
        {
            //Rotation by two fingers.
            //Touch touch = Input.GetTouch(0);
            //touchPos = touch.position;

            //if (Input.touchCount == 2)
            //{
            //    //����� ����������� �������.
            //    Touch f1 = Input.touches[0];
            //    Touch f2 = Input.touches[1];

            //    //���� �����-�� �� ������� ��������.
            //    if (f1.phase == TouchPhase.Moved || f2.phase == TouchPhase.Moved)
            //    {
            //        float fingersDist = Vector2.Distance(f1.position, f2.position);
            //        //���������� �� �������.
            //        float prevFingersDist = Vector2.Distance(f1.position - f1.deltaPosition, f2.position - f2.deltaPosition);
            //        float delta = fingersDist - prevFingersDist;

            //        if (delta != 0)
            //        {
            //            //�������� ������ �� 0.1.
            //            delta *= delta > 0 ? 0.01f : -0.01f;
            //        }
            //        else if (delta == 0)
            //        {
            //            //������ �������� ��������.
            //            delta = fingersDist = 0;
            //        }

            //        YRotQuat = Quaternion.Euler(0f, -f1.deltaPosition.x * delta, 0f);
            //        selectedObject.transform.rotation = YRotQuat * selectedObject.transform.rotation;
            //    }
            //}

            Touch touch = Input.GetTouch(0);
            touchPos = touch.position;
            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = ARCamera.ScreenPointToRay(touch.position);
                RaycastHit hitObject;
                if (Physics.Raycast(ray, out hitObject))
                {
                    if (hitObject.collider.CompareTag("Unselected"))
                    {
                        hitObject.collider.gameObject.tag = "Selected";
                    }
                }
            }

            selectedObject = GameObject.FindWithTag("Selected");

            //���� �������� ����� �������.
            if (touch.phase == TouchPhase.Moved && Input.touchCount == 1)
            {
                if (rotatable)
                {
                    YRotQuat = Quaternion.Euler(0f, -touch.deltaPosition.x * 0.1f, 0f);
                    selectedObject.transform.rotation = YRotQuat * selectedObject.transform.rotation;
                }
            }

            //���������� ��� �������.
            if (touch.phase == TouchPhase.Ended)
            {
                if (selectedObject.CompareTag("Selected"))
                {
                    selectedObject.tag = "Unselected";
                }
            }
        }
    }

    private Deletion deletetionScript;

    /// <summary>
    /// �������� �������.
    /// </summary>
    private void DeleteObject()
    {
        if (Input.touchCount > 0 && deletable && !rotatable)
        {
            Touch touch = Input.GetTouch(0);
            touchPos = touch.position;

            //��� �������� ������.
            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = ARCamera.ScreenPointToRay(touch.position);

                //����� ������� ������� ���.
                RaycastHit hitObject;

                //���� ����� ������ � ������� ���, �� ������ ���.
                if (Physics.Raycast(ray, out hitObject))
                {
                    //if (hitObject.collider.CompareTag("Unselected"))  
                    //{
                    GameObject.DestroyImmediate(hitObject.collider.gameObject);
                    deletetionScript.DeleteAction();
                    //}
                }
            }
        }
    }
}
