using System;
using System.Collections;
using UnityEngine;

public static class Utils{
    public static void RunAfterDelay(MonoBehaviour monoBehaviour, float delay, Action task){
        monoBehaviour.StartCoroutine(RunAfterDelayRoutine(delay, task));
    }

    public static IEnumerator RunAfterDelayRoutine(float delay, Action task){
        yield return new WaitForSeconds(delay);
        task.Invoke();
    }
}