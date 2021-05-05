using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ProgramManager : MonoBehaviour
{
    /*Scripts.*/
    private Rotation rotationScript;
    private Deletion deletetionScript;
    private ARRaycastManager ARRaycastManagerScript;

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
    List<ARRaycastHit> hits;

    //Выбранный объект.
    private GameObject selectedObject;

    // Для изменения угла по У.
    private Quaternion YRotQuat;
    //Должен ли быть вращен объект.
    public bool rotatable;

    //Должен ли быть удален объект.
    public bool deletable;

    // Start is called before the first frame update
    void Start()
    {
        hits = new List<ARRaycastHit>();
        rotationScript = FindObjectOfType<Rotation>();
        deletetionScript = FindObjectOfType<Deletion>();
        materialSetterButtonScript = FindObjectOfType<MaterialSetterButton>();
        scrollView.SetActive(false);

        //Находим скрипт.
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
        ChangeColorObject();
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
        //Создаем луч, он будет с центра экрана.
        Vector2 vec = new Vector2(Screen.width / 2, Screen.height / 2);
        //Помещаем объекты в List<> hits, фиксируем плоскости.
        ARRaycastManagerScript.Raycast(vec, hits, TrackableType.Planes);

        if (hits.Count > 0)
        {
            //Присваиваем значению planeMarker где луч пересекся с плоскостью.
            planeMarkerPrefab.transform.position = hits[0].pose.position;
            planeMarkerPrefab.SetActive(true);
        }

        //Ставим объект. Работает только при удержании
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            //Ставим там, где луч пересекся с плоскостью
            Instantiate(objToSpawn, hits[0].pose.position, objToSpawn.transform.rotation);
            isChoosing = false;
            planeMarkerPrefab.SetActive(false);
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
            //Rotation by two fingers.
            //Touch touch = Input.GetTouch(0);
            //touchPos = touch.position;

            //if (Input.touchCount == 2)
            //{
            //    //Берем кооррдинаты пальцев.
            //    Touch f1 = Input.touches[0];
            //    Touch f2 = Input.touches[1];

            //    //Если какой-то из пальцев движется.
            //    if (f1.phase == TouchPhase.Moved || f2.phase == TouchPhase.Moved)
            //    {
            //        float fingersDist = Vector2.Distance(f1.position, f2.position);
            //        //Расстояние до касания.
            //        float prevFingersDist = Vector2.Distance(f1.position - f1.deltaPosition, f2.position - f2.deltaPosition);
            //        float delta = fingersDist - prevFingersDist;

            //        if (delta != 0)
            //        {
            //            //Изменяем дельту на 0.1.
            //            delta *= delta > 0 ? 0.01f : -0.01f;
            //        }
            //        else if (delta == 0)
            //        {
            //            //Просто обнуляем значения.
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
                    GameObject.DestroyImmediate(hitObject.collider.gameObject);
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


    public Button mater;

    private MaterialSetterButton materialSetterButtonScript;

    /// <summary>
    /// Функция для изменения цвета на объекте.
    /// </summary>
    private void ChangeColorObject()
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
                    //Instantiate(mater, GameObject.Find($"UIContainer").transform);

                    //hitObject.collider.material = 
                    //if (hitObject.collider.CompareTag("Unselected"))
                    //{
                    //    hitObject.collider.gameObject.tag = "Selected";
                    //}
                }
            }
        }
    }
}
