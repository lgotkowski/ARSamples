using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISelectable 
{
    void SetSelection(bool state);

    bool IsSelected();
}
