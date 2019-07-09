using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{

    public Color lineColor;
    private List<Transform> pathNodes = new List<Transform>();
   

    void OnDrawGizmos()
    {
        Gizmos.color = lineColor;

        Transform[] pathLine = GetComponentsInChildren<Transform>();
        pathNodes = new List<Transform>();

        //count all the nodes
        for (int i = 0; i < pathLine.Length; i++)
        {
            //not the parent object node though
            if (pathLine[i] != transform)
            {
                pathNodes.Add(pathLine[i]);
            }
        }

        //draw a line between all path nodes in editor
        int count = pathNodes.Count;
        for (int i = 0; i < count; i++)
        {
            Vector3 currentPathNode = pathNodes[i].position;
            //extra check so the node marked as "0" doesnt go to "-1", but instead the final node
            Vector3 previousPathNode = pathNodes[(count - 1 + i) % count].position;
            //draw lines and spheres on nodes
            Gizmos.DrawLine(previousPathNode, currentPathNode);
            Gizmos.DrawWireSphere(currentPathNode,2f);
        }
    }

        // Start is called before the first frame update
        void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
