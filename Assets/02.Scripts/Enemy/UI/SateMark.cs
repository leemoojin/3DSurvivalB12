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
        SateMarkGameObject.SetActive(true);
        SwitchMaterial(1);
    }

    public void ShowQuestionMark()
    {
        SateMarkGameObject.SetActive(true);
        SwitchMaterial(0);

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
