using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonologueTrigger : MonoBehaviour
{
    [SerializeField] MonologueManager monologueManager;

    public List<Monologue> monologues;

    public void TriggerMonologue(string stage)
    {
        for (int i = 0; i < monologues.Count; i++)
            if (monologues[i].stage.Equals(stage))
                monologueManager.StartMonologue(monologues[i]);
    }
}
