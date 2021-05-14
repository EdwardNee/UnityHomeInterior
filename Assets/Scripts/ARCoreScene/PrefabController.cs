using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

/// <summary>
/// ������-���������� ��� �������� �������.
/// </summary>
public class PrefabController : MonoBehaviourPun, IPunObservable
{
    //������� ��� ����������.
    public static int current_mat = -1;
    public static int old_mat = -1;

    /// <summary>
    /// ����������� ������� ������.
    /// </summary>
    public static PrefabController Instance
    {
        get; private set;
    }

    /// <summary>
    /// ������������ �������� �������.
    /// </summary>
    public static GameObject HittedObj { get; set; }

    //�������� �������� �������.
    public static string objName;

    //������ �������� �������.
    public GameObject prefab;
    //������ ���� ����������.
    public Material[] materials;

    public Text txt;

    // Start is called before the first frame update
    void Start()
    {
        txt = GameObject.FindWithTag("Logger").GetComponent<Text>();
        materials = MaterialsColor.Instance.Materials;
    }

    // Update is called once per frame
    void Update()
    {

        if (old_mat != current_mat)
        {
            SetMaterial(current_mat);
        }
    }


    /// <summary>
    /// ������������� �������� �� ����� �� ������� - ��������� �����.
    /// </summary>
    /// <param name="materialId">������ ���������.</param>
    private void SetMaterial(int materialId)
    {
        txt.text = "SETMATER" + current_mat;
        old_mat = current_mat = materialId;

        try
        {
            txt.text += objName;
            var transformArray = GameObject.Find(objName).GetComponentsInChildren<Transform>();

            foreach (var e in transformArray)
            {
                txt.text += e.name + " ";
                if (e.name.Equals("obj"))
                {
                    e.GetComponent<MeshRenderer>().material = materials[materialId];
                }
            }
            //for (int i = 0; i < mesh.Count; i++)
            //{
            //    mesh[i].GetComponent<MeshRenderer>().material = material[_mat];
            //    txt.text += "loop\n";
            //}
        }
        catch (System.Exception ex)
        {
            txt.text += ex.Message + "\n";
        }
    }

    /// <summary>
    /// ���������� 10 ��� � ������� ��� �������� ������.
    /// ����������� �� IPunObservable.
    /// </summary>
    /// <param name="stream">����� ����������.</param>
    /// <param name="info">���������� � ������� ��������.</param>
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //�� ����� ������
        if (stream.IsWriting)
        {
            stream.SendNext(current_mat);
            stream.SendNext(objName);
        }
        //����� ������.
        else
        {
            current_mat = (int)stream.ReceiveNext();
            objName = (string)stream.ReceiveNext();
        }
    }
}
