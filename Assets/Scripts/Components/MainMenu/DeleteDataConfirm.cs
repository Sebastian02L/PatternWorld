using UnityEngine;

public class DeleteDataConfirm : MonoBehaviour
{
    public void ConfirmDelete()
    {
        PlayerDataManager.Instance.DeleteData();
    }
}
