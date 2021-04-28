using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Deletion : MonoBehaviour
{
    public Button deleteBtn;
    private ProgramManager programManagerScript;

    // Start is called before the first frame update
    void Start()
    {
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
        Debug.Log("Delete it");
        if (programManagerScript.deletable)
        {
            programManagerScript.deletable = false;
            GetComponent<Image>().color = Color.red;
        }
        else
        {
            programManagerScript.deletable = true;
            GetComponent<Image>().color = Color.green;
        }
    }
}
