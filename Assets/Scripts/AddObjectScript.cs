using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddObjectScript : MonoBehaviour
{
    private Button btn;
    private ProgramManager programManagerScript;

    // Start is called before the first frame update
    void Start()
    {
        programManagerScript = FindObjectOfType<ProgramManager>();
        btn = GetComponent<Button>();
        btn.onClick.AddListener(AddObjectFunc);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void AddObjectFunc()
    {
        programManagerScript.scrollView.SetActive(true);
        LoadAssetsData loadAssetsData = new LoadAssetsData();
    }
}
