using System.Collections;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

/// <summary>
/// Основной программный код. Вызов основных функций.
/// </summary>
public class ProgramManager : MonoBehaviour
{
    //Тип-делегат цвет изменен.
    public delegate void ColorChanged();
    //Экземпляр делегата.
    public ColorChanged colorChangedDel;

    /*Scripts.*/
    private Rotation rotationScript;
    private Deletion deletetionScript;
    private DialogColorChange dialogColorChangeScript;
    private ARRaycastManager ARRaycastManagerScript;
    private MaterialSetterButton materialSetterButtonScript;
    private HintsScript hintsScript;

    //Префаб маркера для размещения на сцене.
    [Header("Put your planemarker here.")]
    [SerializeField]
    private GameObject planeMarkerPrefab;

    //Позиция касания.
    private Vector2 touchPos;

    //Объект для спавна.
    public GameObject objToSpawn;

    //Выбирается объект.
    public bool isChoosing;

    //ScrollView - его скрываю при размещении и вначале.
    [Header("Put your ScrollView here.")]
    public GameObject scrollView;

    //Главная камера сцены.
    [SerializeField]
    private Camera ARCamera;

    //Сюда попадают объекты, которые встречает луч.
    private List<ARRaycastHit> hits;

    //Выбранный объект.
    private GameObject selectedObject;

    // Для изменения угла по У.
    private Quaternion YRotQuat;
    //Должен ли быть вращен объект.
    public bool rotatable;

    //Должен ли быть удален объект.
    public bool deletable;

    //Элементы, которые пересек луч.
    private RaycastHit privateHitObject;

    /// <summary>
    /// Конструктор объекта класса.
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

        //Находим скрипт.
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
            hintsScript.DisplayHint("Для начала работы с интерфейсом НАЖМИ на меню три точки справа сверху.", 1);
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
    /// Метод для показа маркера - точки, куда будет ставиться объект.
    /// </summary>
    private void ShowMarkerToSetObject()
    {
        if (HintsScript.hintId < 3)
        {
            string hint = "Камера ищет плоскости. В помещении должно быть достаточно света." +
                " Поводи устройством по комнате для нахождения места, куда можно установить элемент мебели.\n";
            hintsScript.DisplayHint(hint, 3);
        }
        //Создаем луч, он будет с центра экрана.
        Vector2 vec = new Vector2(Screen.width / 2, Screen.height / 2);
        //Помещаем объекты в List<> hits, фиксируем плоскости.
        ARRaycastManagerScript.Raycast(vec, hits, TrackableType.Planes);

        if (hits.Count > 0)
        {
            //Присваиваем значению planeMarker где луч пересекся с плоскостью.
            planeMarkerPrefab.transform.position = hits[0].pose.position;
            planeMarkerPrefab.SetActive(true);

            if (HintsScript.hintId == 3)
            {
                string hint = "Найдена плоскость. Вы видите маркер на полу." +
                    " Это точка, на которой будет установлен объект мебели. Коснитесь свободной области на экране.\n";
                hintsScript.DisplayHint(hint, 3);
            }
        }

        //Ставим объект. Работает только при удержании
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            //Ставим там, где луч пересекся с плоскостью
            try
            {

                txt.text += $"\nsw {hits[0].pose.position} {(hits == null).ToString()}\n";
                //Если есть подключение по сети, то будем ставить через фотон, если нет - локально.
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

                string hint = "Элемент был установлен. Теперь можно его ВРАЩАТЬ свайпами пальцев влево и вправо, УДАЛИТЬ, коснувшись на объект. Для этих действий нужно нажать соответствующие кнопки.\n" +
                    "Изменить размер можно двумя пальцами. Изменить цвет - ЗАЖАТЬ палец на мебели.\n Нажми СКРЫТЬ для закрытия подсказки.";
                hintsScript.DisplayHint(hint, 4);
            }
        }
    }

    /// <summary>
    /// Метод для перемещения объекта по сцене с помощью зажатого пальца.
    /// </summary>
    private void MoveObject()
    {
        if (Input.touchCount == 1 && !rotatable && !deletable)
        {
            Touch touch = Input.GetTouch(0);
            touchPos = touch.position;

            //Тут выбираем обхект.
            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = ARCamera.ScreenPointToRay(touch.position);

                //Какие объекты пересек луч.
                RaycastHit hitObject;

                //Если видим объект и засекли его, то меняем тег.
                if (Physics.Raycast(ray, out hitObject))
                {
                    if (hitObject.collider.CompareTag("Unselected"))
                    {
                        hitObject.collider.gameObject.tag = "Selected";
                    }
                }
            }

            //Тут перемещаем "Selected" объект.
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
    /// Метод для вращения объекта на сцене.
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

            //Если движется одним пальцем.
            if (touch.phase == TouchPhase.Moved && Input.touchCount == 1)
            {
                if (rotatable)
                {
                    YRotQuat = Quaternion.Euler(0f, -touch.deltaPosition.x * 0.1f, 0f);
                    selectedObject.transform.rotation = YRotQuat * selectedObject.transform.rotation;
                }
            }

            //Возвращаем тег обратно.
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
    /// Удаление объекта.
    /// </summary>
    private void DeleteObject()
    {
        if (Input.touchCount > 0 && deletable && !rotatable)
        {
            Touch touch = Input.GetTouch(0);
            touchPos = touch.position;

            //Тут выбираем обхект.
            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = ARCamera.ScreenPointToRay(touch.position);

                //Какие объекты пересек луч.
                RaycastHit hitObject;

                //Если видим объект и засекли его, то меняем тег.
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
    /// Функция изменения размеров объекта.
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
                //Берем кооррдинаты пальцев.
                Touch f1 = Input.touches[0];
                Touch f2 = Input.touches[1];

                //Если какой-то из пальцев движется.
                if (f1.phase == TouchPhase.Moved || f2.phase == TouchPhase.Moved)
                {
                    float fingersDist = Vector2.Distance(f1.position, f2.position);
                    //Расстояние до касания.
                    float prevFingersDist = Vector2.Distance(f1.position - f1.deltaPosition, f2.position - f2.deltaPosition);
                    float delta = fingersDist - prevFingersDist;

                    //Берем размер объекта.
                    float xVal = selectedObject.transform.localScale.x;
                    float yVal = selectedObject.transform.localScale.y;
                    float zVal = selectedObject.transform.localScale.z;

                    //Если расстояние между пальцами стало меньше, то объект уменьшаем, иначе увеличиваем.
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


    //Панель с выбором цвета.
    public Transform colorPan;

    /// <summary>
    /// Открывает окно с выбором цвета объекта.
    /// </summary>
    private void OpenChangeColorDialog()
    {
        if (Input.touchCount == 1 && !rotatable && !deletable)
        {
            Touch touch = Input.GetTouch(0);
            touchPos = touch.position;

            //Тут выбираем обхект.
            if (touch.phase == TouchPhase.Stationary)
            {
                Ray ray = ARCamera.ScreenPointToRay(touch.position);

                //Какие объекты пересек луч.
                RaycastHit hitObject;

                //Если видим объект и засекли его, то меняем тег.
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
    /// Функция для изменения цвета на объекте.
    /// </summary>
    private void ChangeColorObject()
    {
        Collider hittedObj = privateHitObject.collider;
        if (hittedObj != null)
        {
            PrefabController.objName = hittedObj.gameObject.name;
            PrefabController.HittedObj = hittedObj.gameObject;

            //Если нет подключения по интернету.
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
