using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DBGTD.DataStructs
{
    public interface IGraphNode
    {
        int GetDistance(IGraphNode other);

    }
}

