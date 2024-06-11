using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SateMark : MonoBehaviour
{
    //public GameObject ExclamationMark;
    //public GameObject QuestionMark;

    public GameObject SateMarkGameObject;

    public Material[] materials;

    private MeshRenderer _meshRenderer;

    private void Start()
    {
        //ExclamationMark = GetComponent<GameObject>();
        //QuestionMark = GetComponent<GameObject>();

        //SateMarkGameObject = GetComponent<GameObject>();
        _meshRenderer = SateMarkGameObject.GetComponent<MeshRenderer>();
    }


    public void ShowExclamationMark()
    {
        CloseStateMark();
        SwitchMaterial(1);

        SateMarkGameObject.SetActive(true);
    }

    //public void ShowQuestionMark()
    //{
    //    CloseStateMark();

    //    SwitchMaterial(0);
    //    SateMarkGameObject.SetActive(true);


    //}

    public IEnumerator ShowQuestionMark()
    {
        CloseStateMark();
        SwitchMaterial(0);

        SateMarkGameObject.SetActive(true);

        yield return new WaitForSeconds(4f);

        CloseStateMark();
    }

    public void CloseStateMark()
    {
        SateMarkGameObject.SetActive(false);
        //QuestionMark.SetActive(false);

    }

    void SwitchMaterial(int index)
    {
        _meshRenderer.material = materials[index];
    }
}
