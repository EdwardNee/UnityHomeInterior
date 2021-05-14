using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ������ �������� �� ������ �������� ������.
/// </summary>
public class AddObjectScript : MonoBehaviour
{
    //������-��������� ��������.
    private Button btn;
    //������ ������� ProgramManager.
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
    /// ����� ������ ��������� ����������.
    /// </summary>
    private void AddObjectFunc()
    {
        programManagerScript.scrollView.SetActive(true);
    }
}
