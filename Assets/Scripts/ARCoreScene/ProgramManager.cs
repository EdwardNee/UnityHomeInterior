using System.Collections;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

/// <summary>
/// �������� ����������� ���. ����� �������� �������.
/// </summary>
public class ProgramManager : MonoBehaviour
{
    //���-������� ���� �������.
    public delegate void ColorChanged();
    //��������� ��������.
    public ColorChanged colorChangedDel;

    /*Scripts.*/
    private Rotation rotationScript;
    private Deletion deletetionScript;
    private DialogColorChange dialogColorChangeScript;
    private ARRaycastManager ARRaycastManagerScript;
    private MaterialSetterButton materialSetterButtonScript;
    private HintsScript hintsScript;

    //������ ������� ��� ���������� �� �����.
    [Header("Put your planemarker here.")]
    [SerializeField]
    private GameObject planeMarkerPrefab;

    //������� �������.
    private Vector2 touchPos;

    //������ ��� ������.
    public GameObject objToSpawn;

    //���������� ������.
    public bool isChoosing;

    //ScrollView - ��� ������� ��� ���������� � �������.
    [Header("Put your ScrollView here.")]
    public GameObject scrollView;

    //������� ������ �����.
    [SerializeField]
    private Camera ARCamera;

    //���� �������� �������, ������� ��������� ���.
    private List<ARRaycastHit> hits;

    //��������� ������.
    private GameObject selectedObject;

    // ��� ��������� ���� �� �.
    private Quaternion YRotQuat;
    //������ �� ���� ������ ������.
    public bool rotatable;

    //������ �� ���� ������ ������.
    public bool deletable;

    //��������, ������� ������� ���.
    private RaycastHit privateHitObject;

    /// <summary>
    /// ����������� ������� ������.
    /// </summary>
    public static ProgramManager Instance
    {
        get; private set;
    }

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        colorChangedDel += ChangeColorObject;
        hits = new List<ARRaycastHit>();
        rotationScript = Rotation.Instance;
        deletetionScript = Deletion.Instance;
        materialSetterButtonScript = MaterialSetterButton.Instance;
        scrollView.SetActive(false);

        //������� ������.
        ARRaycastManagerScript = FindObjectOfType<ARRaycastManager>();
        planeMarkerPrefab.SetActive(false);





    }

    // Update is called once per frame
    void Update()
    {
        if (hintsScript == null)
        {
            hintsScript = HintsScript.Instance;
        }

        if (hintsScript != null && HintsScript.hintId < 1)
        {
            hintsScript.DisplayHint("��� ������ ������ � ����������� ����� �� ���� ��� ����� ������ ������.", 1);
        }
        if (isChoosing)
        {
            ShowMarkerToSetObject();
        }
        OpenChangeColorDialog();
        RotateObject();
        DeleteObject();
        MoveObject();
        ScaleObject();
    }

    /// <summary>
    /// ����� ��� ������ ������� - �����, ���� ����� ��������� ������.
    /// </summary>
    private void ShowMarkerToSetObject()
    {
        if (HintsScript.hintId < 3)
        {
            string hint = "������ ���� ���������. � ��������� ������ ���� ���������� �����." +
                " ������ ����������� �� ������� ��� ���������� �����, ���� ����� ���������� ������� ������.\n";
            hintsScript.DisplayHint(hint, 3);
        }
        //������� ���, �� ����� � ������ ������.
        Vector2 vec = new Vector2(Screen.width / 2, Screen.height / 2);
        //�������� ������� � List<> hits, ��������� ���������.
        ARRaycastManagerScript.Raycast(vec, hits, TrackableType.Planes);

        if (hits.Count > 0)
        {
            //����������� �������� planeMarker ��� ��� ��������� � ����������.
            planeMarkerPrefab.transform.position = hits[0].pose.position;
            planeMarkerPrefab.SetActive(true);

            if (HintsScript.hintId == 3)
            {
                string hint = "������� ���������. �� ������ ������ �� ����." +
                    " ��� �����, �� ������� ����� ���������� ������ ������. ��������� ��������� ������� �� ������.\n";
                hintsScript.DisplayHint(hint, 3);
            }
        }

        //������ ������. �������� ������ ��� ���������
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            //������ ���, ��� ��� ��������� � ����������
            try
            {

                txt.text += $"\nsw {hits[0].pose.position} {(hits == null).ToString()}\n";
                //���� ���� ����������� �� ����, �� ����� ������� ����� �����, ���� ��� - ��������.
                if (LobbyManagerUnity.IsNetwork)
                {
                    PhotonNetwork.Instantiate(objToSpawn.name, hits[0].pose.position, objToSpawn.transform.rotation);
                }
                else
                {
                    Instantiate(objToSpawn, hits[0].pose.position, objToSpawn.transform.rotation);
                }
            }
            catch (System.Exception ex)
            {
                txt.text += $"\n{ex.Message}\n";
            }
            //Instantiate(objToSpawn, hits[0].pose.position, objToSpawn.transform.rotation);
            isChoosing = false;
            planeMarkerPrefab.SetActive(false);
            if (HintsScript.hintId == 3)
            {
                hintsScript.Off();

                string hint = "������� ��� ����������. ������ ����� ��� ������� �������� ������� ����� � ������, �������, ���������� �� ������. ��� ���� �������� ����� ������ ��������������� ������.\n" +
                    "�������� ������ ����� ����� ��������. �������� ���� - ������ ����� �� ������.\n ����� ������ ��� �������� ���������.";
                hintsScript.DisplayHint(hint, 4);
            }
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

    /// <summary>
    /// ����� ��� �������� ������� �� �����.
    /// </summary>
    private void RotateObject()
    {
        if (Input.touchCount > 0)
        {
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
                    //GameObject.DestroyImmediate(hitObject.collider.gameObject);
                    PhotonNetwork.Destroy(hitObject.collider.gameObject);
                    deletetionScript.DeleteAction();
                    //}
                }
            }
        }
    }

    /// <summary>
    /// ������� ��������� �������� �������.
    /// </summary>
    private void ScaleObject()
    {
        if (Input.touchCount > 0)
        {
            //Rotation by two fingers.
            Touch touch = Input.GetTouch(0);
            touchPos = touch.position;

            if (Input.touchCount == 2)
            {
                //����� ����������� �������.
                Touch f1 = Input.touches[0];
                Touch f2 = Input.touches[1];

                //���� �����-�� �� ������� ��������.
                if (f1.phase == TouchPhase.Moved || f2.phase == TouchPhase.Moved)
                {
                    float fingersDist = Vector2.Distance(f1.position, f2.position);
                    //���������� �� �������.
                    float prevFingersDist = Vector2.Distance(f1.position - f1.deltaPosition, f2.position - f2.deltaPosition);
                    float delta = fingersDist - prevFingersDist;

                    //����� ������ �������.
                    float xVal = selectedObject.transform.localScale.x;
                    float yVal = selectedObject.transform.localScale.y;
                    float zVal = selectedObject.transform.localScale.z;

                    //���� ���������� ����� �������� ����� ������, �� ������ ���������, ����� �����������.
                    if (delta > 0)
                    {
                        selectedObject.transform.localScale = new Vector3(xVal + xVal * 0.05f, yVal + yVal * 0.05f, zVal + zVal * 0.05f);
                    }
                    else if (delta < 0)
                    {
                        selectedObject.transform.localScale = new Vector3(xVal - xVal * 0.05f, yVal - yVal * 0.05f, zVal - zVal * 0.05f);
                    }
                }
            }
        }
    }


    //������ � ������� �����.
    public Transform colorPan;

    /// <summary>
    /// ��������� ���� � ������� ����� �������.
    /// </summary>
    private void OpenChangeColorDialog()
    {
        if (Input.touchCount == 1 && !rotatable && !deletable)
        {
            Touch touch = Input.GetTouch(0);
            touchPos = touch.position;

            //��� �������� ������.
            if (touch.phase == TouchPhase.Stationary)
            {
                Ray ray = ARCamera.ScreenPointToRay(touch.position);

                //����� ������� ������� ���.
                RaycastHit hitObject;

                //���� ����� ������ � ������� ���, �� ������ ���.
                if (Physics.Raycast(ray, out hitObject))
                {
                    privateHitObject = hitObject;
                    colorPan.transform.gameObject.SetActive(true);
                    dialogColorChangeScript = DialogColorChange.Instance;
                    dialogColorChangeScript.OpenColorChooser();
                    //hitObject.collider.GetComponent<MeshRenderer>().material = materialSetterButtonScript.Material;
                    //if (hitObject.collider.CompareTag("Unselected"))
                    //{
                    //    hitObject.collider.gameObject.tag = "Selected";
                    //}
                }
            }
        }
    }
    public Text txt;

    /// <summary>
    /// ������� ��� ��������� ����� �� �������.
    /// </summary>
    private void ChangeColorObject()
    {
        Collider hittedObj = privateHitObject.collider;
        if (hittedObj != null)
        {
            PrefabController.objName = hittedObj.gameObject.name;
            PrefabController.HittedObj = hittedObj.gameObject;

            //���� ��� ����������� �� ���������.
            if (!LobbyManagerUnity.IsNetwork)
            {
                Transform[] transformArray = hittedObj.gameObject.GetComponentsInChildren<Transform>();
                foreach (var e in transformArray)
                {
                    if (e.name.StartsWith("obj"))   //Equals.
                    {
                        e.GetComponent<MeshRenderer>().material = MaterialSetterButton.Material;
                        Debug.Log(e.GetComponent<MeshRenderer>().materials[0] + " set");
                    }
                }
            }
        }
        else
        {
            Debug.Log("ProgramManager.ChangeColorObject: privateHitObject.collider is NULL.");
        }
    }
}
