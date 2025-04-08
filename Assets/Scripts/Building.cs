using UnityEngine;

public class Building : MonoBehaviour
{
    public void DisableBuilding()
    {
        gameObject.SetActive(false);
    }

    public void EnableBuilding()
    {
        gameObject.SetActive(true);
    }

    public bool IsActive()
    {
        return gameObject.activeSelf;
    }
}
