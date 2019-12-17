using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTExecutor : MonoBehaviour
{
    public List<BT.GenericDictionary> Contexts  = new List<BT.GenericDictionary>();
    public BT.BTGraph graph;

    static BTExecutor instance = null;
    public static BTExecutor Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<BTExecutor>();
            return instance;
        }
    }

    public void RegisterContext(BT.GenericDictionary context)
    {
        // We make sure we will not run the same AI twice
        if (!Contexts.Contains(context))
        {
            Contexts.Add(context);
        }
    }

    void Start()
    {
        StartCoroutine(UpdateGraph());
    }

    IEnumerator UpdateGraph()
    {
        graph.Init();

        while (true)
        {
            foreach (var context in Contexts)
            {
                graph.Run(context);
            }

            yield return new WaitForEndOfFrame();

        }

        yield return new WaitForEndOfFrame();

    }
}
