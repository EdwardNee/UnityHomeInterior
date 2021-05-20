using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ������, ���������� �� ������ ��������.
/// </summary>
public class Rotation : MonoBehaviour
{
    //������ ��������.
    public Button rotateBtn;
    //������ ������� ProgramManager.
    private ProgramManager programManagerScript;
    //������������� �����.
    private GameObject line;

    /// <summary>
    /// ����������� ������� ������.
    /// </summary>
    public static Rotation Instance
    {
        get; private set;
    }

    // Start is called before the first frame update
    void Start()
    {
        line = GameObject.Find($"{rotateBtn.name}/Line");
        programManagerScript = FindObjectOfType<ProgramManager>();
        rotateBtn = GetComponent<Button>();
        rotateBtn.onClick.AddListener(RotateAction);
    }

    // Update is called once per frame 
    void Update()
    {
    }

    /// <summary>
    /// ������������� ����� ��� �������� � ������ ����� ��������.
    /// </summary>
    public void RotateAction()
    {
        Debug.Log("Rotate button");
        if (programManagerScript.rotatable)
        {
            programManagerScript.rotatable = false;
            line.SetActive(true);
        }
        else
        {
            if (programManagerScript.deletable)
            {
                Debug.Log("Cannot rotate. Deletion is ON.");
                return;
            }
            programManagerScript.rotatable = true;
            line.SetActive(false);
        }
        StartCoroutine(ClickedCoroutine(rotateBtn));
    }

    /// <summary>
    /// �������� ��� "��������" ������ ��� �������.
    /// </summary>
    /// <param name="obj">������, ������� ����� ������������� ��� �������.</param>
    private IEnumerator ClickedCoroutine(Button obj)
    {
        float xVal_p = obj.transform.localScale.x;
        float yVal_p = obj.transform.localScale.y;
        float zVal_p = obj.transform.localScale.z;
        obj.transform.localScale = new Vector3(xVal_p + xVal_p * 0.1f, yVal_p + yVal_p * 0.1f, zVal_p + zVal_p * 0.1f);
        float xVal = obj.transform.localScale.x;
        float yVal = obj.transform.localScale.y;
        float zVal = obj.transform.localScale.z;
        yield return new WaitForSeconds(0.15f);
        obj.transform.localScale = new Vector3(xVal - xVal_p * 0.1f, yVal - yVal_p * 0.1f, zVal - zVal_p * 0.1f);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
