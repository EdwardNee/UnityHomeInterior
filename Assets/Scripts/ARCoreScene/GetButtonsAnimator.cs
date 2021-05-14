using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ������ ��� ��������� �������� ��������� ������ �������������� � ��������� ������.
/// </summary>
public class GetButtonsAnimator : MonoBehaviour
{
    //������ �������� ������.
    [SerializeField]
    private Canvas panelObj;

    [SerializeField]
    private Canvas panelNetwork;

    [SerializeField]
    public Button create;
    [SerializeField]
    public Button join;
    [SerializeField]
    public Button leave;

    private HintsScript hintsScript;

    public static GetButtonsAnimator Instace { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        Instace = this;


        //�������� ���������-������, � ������� ���������.
        var btn = GetComponent<Button>();

        btn.onClick.AddListener(StartAnim);
    }

    // Update is called once per frame
    void Update()
    {
        if (hintsScript == null)
        {
            hintsScript = HintsScript.Instance;
        }
    }

    private IEnumerator AddTip()
    {
        string hint = "\n������ ��������� ������ ������ � �����. " +
              "����� �������: �������, ��������������, ����� �� �������.\n";
        hintsScript.AddToTextHint(hint, 2);
        yield return new WaitForSeconds(2f);
        hint = "������ ����� ������ ��� ������ � ��������� � �����. ������ ����: �������, ������� � �������� �������.\n" +
        "��� ��������� �������� ������ ����� �� ������ � ������ �������.\n";
        hintsScript.AddToTextHint(hint, 2);
    }

    /// <summary>
    /// ������������� ����� ��� �������� ����������� ������.
    /// </summary>
    private void StartAnim()
    {
        if (HintsScript.hintId < 2)
        {
            StartCoroutine(AddTip());
        }
        Debug.Log($"Animation GetButtonsAnimator.");

        //���� � ��������� ������ ��� ��������.
        Animator anim = panelObj.GetComponent<Animator>();
        Animator anim1 = GetComponent<Animator>();

        if (anim != null)
        {
            bool isOpen = anim.GetBool("opened");
            anim.SetBool("opened", !isOpen);
        }

        if (anim1 != null)
        {
            bool isOpen = anim1.GetBool("open");
            anim1.SetBool("open", !isOpen);
        }

        //���� � ��������� ������ ��� ����.
        Animator animNetwork = panelNetwork.GetComponent<Animator>();

        bool isOpened = false;
        if (animNetwork != null)
        {
            isOpened = animNetwork.GetBool("getopen");
            animNetwork.SetBool("getopen", !isOpened);
        }

        if (!isOpened)
        {
            /*������ ������������ �����������, �������, ���� �� �� �������,
             � ���������� �����, ���� �� �������.*/
            create.interactable = !LobbyManagerUnity.IsNetwork;
            join.interactable = !LobbyManagerUnity.IsNetwork;
            leave.interactable = LobbyManagerUnity.IsNetwork;
        }
    }

    //���������� ��� ����������� MonoBehaviour.
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
