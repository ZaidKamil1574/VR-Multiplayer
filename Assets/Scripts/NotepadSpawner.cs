using UnityEngine;

public class NotepadSpawner : MonoBehaviour
{
    public GameObject notepadPrefab;
    public GameObject penPrefab;
    public Transform notepadSpawnPoint;
    public Transform rightHandAttachPoint;

    private GameObject spawnedNotepad;
    private GameObject spawnedPen;

    private bool isNotepadActive = false;

    public void ToggleNotepadAndPen()
    {
        if (!isNotepadActive)
        {
            // Spawn
            spawnedNotepad = Instantiate(notepadPrefab, notepadSpawnPoint.position, notepadSpawnPoint.rotation);
            spawnedPen = Instantiate(penPrefab);
            spawnedPen.transform.SetParent(rightHandAttachPoint);
            spawnedPen.transform.localPosition = Vector3.zero;
            spawnedPen.transform.localRotation = Quaternion.identity;

            isNotepadActive = true;
        }
        else
        {
            // Despawn
            if (spawnedNotepad != null)
                Destroy(spawnedNotepad);
            if (spawnedPen != null)
                Destroy(spawnedPen);

            isNotepadActive = false;
        }
    }
}
