using UnityEngine;

public class Explosion : MonoBehaviour
{

  // Use this for initialization
  void Start()
  {
    Destroy(gameObject, GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
  }
}