using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    public Objective objective;
    public uiManager uiManagerRef;
    
    public Playe playerRef;

    private void QuestAccepted()
    {
        if (uiManagerRef != null && playerRef != null)
        {
            objective.isActive = true;
            uiManagerRef.UpdateObjectiveText(objective.title);
            playerRef.objective = objective;
        }
        else
        {
            return;
        }
        
    }
    void Start() {
        QuestAccepted();
    }
}
