using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Скрипт для подсказки пользованием интерфейсом.
/// </summary>
public class HintsScript : MonoBehaviour
{
    //Текст для подсказки.
    [SerializeField]
    private Text textHint;
    //Кнопка спрятать окно.
    [SerializeField]
    private Button hide;
    //Панель подсказок.
    [SerializeField]
    private Transform hintPanel;

    //Нужно показывать подсказку или нет.
    private static bool show = true;
    //Айди подсказки.
    public static int hintId;

    /// <summary>
    /// Конструктор объекта класса.
    /// </summary>
    public static HintsScript Instance { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        Debug.Log("Insr");
        hide.onClick.AddListener(DoNotShow);
    }

    // Update is called once per frame
    void Update()
    {
    }

    /// <summary>
    /// Показать подсказку с заданным текстом.
    /// </summary>
    /// <param name="hintText">Подсказка.</param>
    /// <param name="hintId">Идентификатор подсказки.</param>
    public void DisplayHint(string hintText, int hintId)
    {
        if (show)
        {
            hintPanel.gameObject.SetActive(true);
            textHint.text = hintText;
            HintsScript.hintId = hintId;
        }
    }

    /// <summary>
    /// Добавляет к уже существующей подсказке новый текст.
    /// </summary>
    /// <param name="hintText">Подсказка.</param>
    /// <param name="hintId">Идентификатор подсказки.</param>
    public void AddToTextHint(string hintText, int hintId)
    {
        textHint.text += hintText;
        HintsScript.hintId = hintId;
    }

    /// <summary>
    /// Выключить панель.
    /// </summary>
    public void Off()
    {
        hintPanel.gameObject.SetActive(false);
    }

    /// <summary>
    /// Скрыть и не показывать панель подсказок.
    /// </summary>
    public void DoNotShow()
    {
        show = false;
        Off();
    }
}
