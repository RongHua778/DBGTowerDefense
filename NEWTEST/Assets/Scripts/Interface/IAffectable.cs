using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAffectable
{
    void Affect(IEnumerable<MagicEffect> effectList);

}
