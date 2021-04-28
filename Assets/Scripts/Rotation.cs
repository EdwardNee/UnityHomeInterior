using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rotation : MonoBehaviour
{
    public Button rotateBtn;
    private ProgramManager programManagerScript;

    // Start is called before the first frame update
    void Start()
    {
        programManagerScript = FindObjectOfType<ProgramManager>();
        rotateBtn = GetComponent<Button>();
        rotateBtn.onClick.AddListener(RotateAction);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void RotateAction()
    {
        Debug.Log("Rotate it");
        if (programManagerScript.rotatable)
        {
            programManagerScript.rotatable = false;
            rotateBtn.GetComponent<Image>().color = Color.gray;
        }
        else
        {
            programManagerScript.rotatable = true;
            rotateBtn.GetComponent<Image>().color = Color.blue;
        }
    }
}
