using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseObject : MonoBehaviour
{
    private ProgramManager programManagerScript;

    private Button button;
    public GameObject chosedObj;

    // Start is called before the first frame update
    void Start()
    {
        programManagerScript = FindObjectOfType<ProgramManager>();

        button = GetComponent<Button>();
        button.onClick.AddListener(ChooseObjectFunc);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void ChooseObjectFunc()
    {
        //Присваиваем тому объекту тот, что выбрали.
        programManagerScript.objToSpawn = chosedObj;
        programManagerScript.scrollView.SetActive(false);
        programManagerScript.isChoosing = true;
    }
}
