using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Окно выбора цвета для мебели.
/// </summary>
public class DialogColorChange : MonoBehaviour
{
    //панель для задней тени, которая блокирует интерфейс.
    [SerializeField]
    private Transform shadowPanel;
    //Кнопка для закрытия окна.
    [SerializeField]
    private Button closeBtn;
    //Панель окна.
    private Transform panel;
    //Объект скрипта ProgramManager.
    private ProgramManager programManagerScript;

    /// <summary>
    /// Конструктор объекта класса.
    /// </summary>
    public static DialogColorChange Instance
    {
        get; private set;
    }

    // Start is called before the first frame update
    private void Start()
    {
        Instance = this;
        panel = GetComponent<Transform>();
        programManagerScript = FindObjectOfType<ProgramManager>();
        closeBtn.onClick.AddListener(CloseColorChooser);
        programManagerScript.colorChangedDel += CloseColorChooser;
        CloseColorChooser();
    }

    // Update is called once per frame
    void Update()
    {
    }

    /// <summary>
    /// Метод открытия окна выбора цвета.
    /// </summary>
    public void OpenColorChooser()
    {
        panel.gameObject.SetActive(true);
        shadowPanel.gameObject.SetActive(true);
    }

    /// <summary>
    /// Метод закрытия окна выбора цвета.
    /// </summary>
    public void CloseColorChooser()
    {
        if (panel.gameObject.activeSelf)
        {
            panel.gameObject.SetActive(false);
            shadowPanel.gameObject.SetActive(false);
        }
    }
}
