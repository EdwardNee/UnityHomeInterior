using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ������ ��� ������ ��������.
/// </summary>
public class Deletion : MonoBehaviour
{
    //������ ��������.
    public Button deleteBtn;
    //������ ������� ProgramManager.
    private ProgramManager programManagerScript;
    //������������� �����.
    private GameObject line;

    /// <summary>
    /// ����������� ������� ������.
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
    /// ���� ������ ������, �� � ������ �������� ����.
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

    //���������� ��� ����������� Monobehaviour.
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
