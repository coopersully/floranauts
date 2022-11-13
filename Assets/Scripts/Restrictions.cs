using System.Collections.Generic;
using UnityEngine;

public class Restrictions : MonoBehaviour
{
    private readonly List<GameObject> _restrictions = new();

    public bool IsRestricted()
    {
        return _restrictions.Count > 0;
    }

    public List<GameObject> GetRestrictions()
    {
        return _restrictions;
    }

    public void Add(GameObject restriction)
    {
        _restrictions.Add(restriction);
    }

    public void Remove(GameObject restriction)
    {
        _restrictions.Remove(restriction);
    }
}
