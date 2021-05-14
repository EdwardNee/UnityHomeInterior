using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ���� ������ ����� ��� ������.
/// </summary>
public class DialogColorChange : MonoBehaviour
{
    //������ ��� ������ ����, ������� ��������� ���������.
    [SerializeField]
    private Transform shadowPanel;
    //������ ��� �������� ����.
    [SerializeField]
    private Button closeBtn;
    //������ ����.
    private Transform panel;
    //������ ������� ProgramManager.
    private ProgramManager programManagerScript;

    /// <summary>
    /// ����������� ������� ������.
    /// </summary>
    public static DialogColorChange Instance
    {
        get; private set;
    }

    // Start is called before the first frame update
    private void Start()
    {
        Instance = this;
        panel = GetComponent<Transform>();
        programManagerScript = FindObjectOfType<ProgramManager>();
        closeBtn.onClick.AddListener(CloseColorChooser);
        programManagerScript.colorChangedDel += CloseColorChooser;
        CloseColorChooser();
    }

    // Update is called once per frame
    void Update()
    {
    }

    /// <summary>
    /// ����� �������� ���� ������ �����.
    /// </summary>
    public void OpenColorChooser()
    {
        panel.gameObject.SetActive(true);
        shadowPanel.gameObject.SetActive(true);
    }

    /// <summary>
    /// ����� �������� ���� ������ �����.
    /// </summary>
    public void CloseColorChooser()
    {
        if (panel.gameObject.activeSelf)
        {
            panel.gameObject.SetActive(false);
            shadowPanel.gameObject.SetActive(false);
        }
    }
}
