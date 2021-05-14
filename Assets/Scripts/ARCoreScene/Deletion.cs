using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Скрипт для кнопки удаления.
/// </summary>
public class Deletion : MonoBehaviour
{
    //Кнопка удаления.
    public Button deleteBtn;
    //Объект скрипта ProgramManager.
    private ProgramManager programManagerScript;
    //Перечеркнутая линия.
    private GameObject line;

    /// <summary>
    /// Конструктор объекта класса.
    /// </summary>
    public static Deletion Instance
    {
        get; private set;
    }

    // Start is called before the first frame update
    void Start()
    {
        line = GameObject.Find($"{deleteBtn.name}/Line");
        programManagerScript = FindObjectOfType<ProgramManager>();
        deleteBtn = GetComponent<Button>();
        deleteBtn.onClick.AddListener(DeleteAction);
    }

    // Update is called once per frame
    void Update()
    {
    }

    /// <summary>
    /// Если кнопка нажата, то в скрипт передаем флаг.
    /// </summary>
    public void DeleteAction()
    {
        Debug.Log("Delete button");
        if (programManagerScript.deletable)
        {
            programManagerScript.deletable = false;
            line.SetActive(true);
        }
        else
        {
            if (programManagerScript.rotatable)
            {
                Debug.Log("Cannot delele. Rotation is ON.");
                return;
            }
            programManagerScript.deletable = true;
            line.SetActive(false);
        }
        StartCoroutine(ClickedCoroutine(deleteBtn));
    }

    /// <summary>
    /// Корутина для "анимации" кнопки при нажатии.
    /// </summary>
    /// <param name="obj">Кнопка, которая будет увеличиваться при нажатии.</param>
    private IEnumerator ClickedCoroutine(Button obj)
    {
        float xVal = obj.transform.localScale.x;
        float yVal = obj.transform.localScale.y;
        float zVal = obj.transform.localScale.z;
        obj.transform.localScale = new Vector3(xVal + xVal * 0.1f, yVal + yVal * 0.1f, zVal + zVal * 0.1f);
        xVal = obj.transform.localScale.x;
        yVal = obj.transform.localScale.y;
        zVal = obj.transform.localScale.z;
        yield return new WaitForSeconds(0.15f);
        obj.transform.localScale = new Vector3(xVal - xVal * 0.1f, yVal - yVal * 0.1f, zVal - zVal * 0.1f);
    }

    //Вызывается при уничтожении Monobehaviour.
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
