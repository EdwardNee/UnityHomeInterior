using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ������ ��� ��������� ������������ �����������.
/// </summary>
public class HintsScript : MonoBehaviour
{
    //����� ��� ���������.
    [SerializeField]
    private Text textHint;
    //������ �������� ����.
    [SerializeField]
    private Button hide;
    //������ ���������.
    [SerializeField]
    private Transform hintPanel;

    //����� ���������� ��������� ��� ���.
    private static bool show = true;
    //���� ���������.
    public static int hintId;

    /// <summary>
    /// ����������� ������� ������.
    /// </summary>
    public static HintsScript Instance { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        Debug.Log("Insr");
        hide.onClick.AddListener(DoNotShow);
    }

    // Update is called once per frame
    void Update()
    {
    }

    /// <summary>
    /// �������� ��������� � �������� �������.
    /// </summary>
    /// <param name="hintText">���������.</param>
    /// <param name="hintId">������������� ���������.</param>
    public void DisplayHint(string hintText, int hintId)
    {
        if (show)
        {
            hintPanel.gameObject.SetActive(true);
            textHint.text = hintText;
            HintsScript.hintId = hintId;
        }
    }

    /// <summary>
    /// ��������� � ��� ������������ ��������� ����� �����.
    /// </summary>
    /// <param name="hintText">���������.</param>
    /// <param name="hintId">������������� ���������.</param>
    public void AddToTextHint(string hintText, int hintId)
    {
        textHint.text += hintText;
        HintsScript.hintId = hintId;
    }

    /// <summary>
    /// ��������� ������.
    /// </summary>
    public void Off()
    {
        hintPanel.gameObject.SetActive(false);
    }

    /// <summary>
    /// ������ � �� ���������� ������ ���������.
    /// </summary>
    public void DoNotShow()
    {
        show = false;
        Off();
    }
}
