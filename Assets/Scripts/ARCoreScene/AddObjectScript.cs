using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Скрипт отвечает за кнопку добавить объект.
/// </summary>
public class AddObjectScript : MonoBehaviour
{
    //Кнопка-слушатель действия.
    private Button btn;
    //Объект скрипта ProgramManager.
    private ProgramManager programManagerScript;

    // Start is called before the first frame update
    void Start()
    {
        programManagerScript = ProgramManager.Instance;
        btn = GetComponent<Button>();
        btn.onClick.AddListener(AddObjectFunc);
    }

    // Update is called once per frame
    void Update()
    {
    }

    /// <summary>
    /// Метод делает скроллвью неактивным.
    /// </summary>
    private void AddObjectFunc()
    {
        programManagerScript.scrollView.SetActive(true);
    }
}
