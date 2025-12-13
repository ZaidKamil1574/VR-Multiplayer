using UnityEngine;

public class ModelSelectorUI : MonoBehaviour
{
    [Header("3D Models")]
    public GameObject cubeModel;
    public GameObject sphereModel;

    private GameObject currentModel;

    public void ActivateCube()
    {
        SetActiveModel(cubeModel);
    }

    public void ActivateSphere()
    {
        SetActiveModel(sphereModel);
    }

    private void SetActiveModel(GameObject model)
    {
        if (currentModel != null)
            currentModel.SetActive(false);

        model.SetActive(true);
        currentModel = model;
    }
}
