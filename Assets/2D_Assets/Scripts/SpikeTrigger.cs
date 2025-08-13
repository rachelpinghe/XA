using System.Collections.Generic;
using UnityEngine;

public class SpikeTrigger : MonoBehaviour
{
    public SpikeDropMover[] spikesToDrop; // 在 Inspector 中拖入带 SpikeDropMover 脚本的刺们

    private bool hasActivated = false;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("spike triggered");
        if (!hasActivated && other.CompareTag("Player"))
        {
            Debug.Log("the spike is falling");
            // spikesToDrop = FindObjectsOfType<SpikeDropMover>(); // 获取场景中所有 SpikeDropMover 脚本的实例
            foreach (var spike in spikesToDrop)
            {
                if (spike != null)
                {
                    spike.StartDrop(); // 调用每个刺的下落方法
                }
            }
            hasActivated = true;
        }
    }
}