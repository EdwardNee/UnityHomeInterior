using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ������ ��� ������ �������� ������ ��� ���������.
/// </summary>
public class ChooseObject : MonoBehaviour
{
    //��������� ProgramManager.
    private ProgramManager programManagerScript;

    //������ ������.
    private Button button;
    //������ ���������� �������.
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
    /// ����������� ��� ������� �� ������ �������� ������.
    /// </summary>
    private void ChooseObjectFunc()
    {
        //����������� ���� ������� ���, ��� �������.
        programManagerScript.objToSpawn = chosedObj;
        programManagerScript.scrollView.SetActive(false);
        programManagerScript.isChoosing = true;
    }
}
