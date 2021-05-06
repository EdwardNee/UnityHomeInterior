using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetButtonsAnimator : MonoBehaviour
{
    [SerializeField]
    private Canvas panel;

    // Start is called before the first frame update
    void Start()
    {
        //�������� ���������-������, � ������� ���������.
        var btn = GetComponent<Button>();

        btn.onClick.AddListener(StartAnim);
    }

    // Update is called once per frame
    void Update()
    {
    }

    void StartAnim()
    {
        Debug.Log($"Animation GetButtonsAnimator.");
        Animator anim = panel.GetComponent<Animator>();
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
    }
}
