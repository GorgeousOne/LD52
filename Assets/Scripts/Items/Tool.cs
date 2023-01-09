using UnityEngine;

public  enum ToolType {
	Bucket,
	Scythe,
    Soul
}

public abstract class Tool : MonoBehaviour {
	
	public ToolType toolType;
	public abstract void Interact(Interactable i);
    public Tool distancesqr(Vector3 pos, ref float sqrdis)
    {
        float tooldistance = (pos - transform.position).sqrMagnitude;
        if (tooldistance < sqrdis)
        {
            sqrdis = tooldistance;
            return this;
        }
        return null;
    }
}