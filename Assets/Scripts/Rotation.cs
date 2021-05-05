using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rotation : MonoBehaviour
{
    public Button rotateBtn;
    private ProgramManager programManagerScript;
    private GameObject line;

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
    /// Корутина для "анимации" кнопки при нажатии.
    /// </summary>
    /// <param name="obj">Кнопка, которая будет увеличиваться при нажатии.</param>
    private IEnumerator ClickedCoroutine(Button obj)
    {
        float xVal = obj.transform.localScale.x;
        float yVal = obj.transform.localScale.y;
        float zVal = obj.transform.localScale.z;
        obj.transform.localScale = new Vector3(xVal + xVal * 0.1f, yVal + yVal * 0.1f, zVal + zVal * 0.1f);
        xVal = obj.transform.localScale.x;
        yVal = obj.transform.localScale.y;
        zVal = obj.transform.localScale.z;
        yield return new WaitForSeconds(0.15f);
        obj.transform.localScale = new Vector3(xVal - xVal * 0.1f, yVal - yVal * 0.1f, zVal - zVal * 0.1f);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
