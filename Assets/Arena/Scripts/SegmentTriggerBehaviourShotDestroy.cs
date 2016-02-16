using UnityEngine;
using System.Collections;

/// <summary>
/// Defines behavioru that will destroy the segment when it is hit by a bullet
/// </summary>

public class SegmentTriggerBehaviourShotDestroy : SegmentTriggerBehaviour
{
	void SegmentTriggerBehaviour.Enter (Collider2D other, SegmentController segmentController)
  {
      if (other.gameObject.tag == "Shot" || other.gameObject.tag == "PiercingShot")
    {
        Vector3 explodePosition = UtilityScript.transformToCartesian(other.transform.position);
        GameObject explosionInstance = MonoBehaviour.Instantiate(segmentController.shotFart, explodePosition, Quaternion.identity) as GameObject;

        CamShakeManager.PlayTestShake(0.1f, 0.5f);
        SoundManager.PlayExplosionClip();
       // segmentController.mesh.transform.GetChild(0).gameObject.SetActive(false);
        explosionInstance.transform.SetParent(segmentController.mesh.transform);
    }
  }
}
