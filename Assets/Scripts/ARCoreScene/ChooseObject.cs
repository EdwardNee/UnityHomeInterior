using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Скрипт для выбора элемента мебели для установки.
/// </summary>
public class ChooseObject : MonoBehaviour
{
    //Экземпляр ProgramManager.
    private ProgramManager programManagerScript;

    //Кнопка мебели.
    private Button button;
    //Префаб выбранного объекта.
    public GameObject chosedObj;

    // Start is called before the first frame update
    void Start()
    {
        programManagerScript = ProgramManager.Instance;

        button = GetComponent<Button>();
        button.onClick.AddListener(ChooseObjectFunc);
    }

    // Update is called once per frame
    void Update()
    {
    }

    /// <summary>
    /// Срабатывает при нажатии на кнопку элемента мебели.
    /// </summary>
    private void ChooseObjectFunc()
    {
        //Присваиваем тому объекту тот, что выбрали.
        programManagerScript.objToSpawn = chosedObj;
        programManagerScript.scrollView.SetActive(false);
        programManagerScript.isChoosing = true;
    }
}
