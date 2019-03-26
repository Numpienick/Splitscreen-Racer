using UnityEngine;

public class ModeSelector : MonoBehaviour {

    public string mode;
	void Start () {
       DontDestroyOnLoad(this);
	}
}
