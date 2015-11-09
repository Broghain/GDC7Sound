using UnityEngine;
using System.Collections;

public class DestroyAfterAnimation : MonoBehaviour {
    public void Destroy()
    {
        Destroy(this.gameObject);
    }
}
