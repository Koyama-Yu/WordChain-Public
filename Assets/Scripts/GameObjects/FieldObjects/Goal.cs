using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //StageEventManager.Instance.OnTriggerStageCleared();

            //TODO! 今はFindObjectOfTypeでStageControllerを取得しているが、のちに直す
            // StageEventManager の参照を StageController 経由で取得
            StageController stage = FindObjectOfType<StageController>();
            stage?.EventManager.OnTriggerStageCleared();
        }
    }
}
