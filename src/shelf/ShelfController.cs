using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ShelfGui))]
[RequireComponent(typeof(ShelfModel))]
public abstract class ShelfController : MonoBehaviour
{
    protected ShelfGui m_ShelfGUi;
    protected ShelfModel m_ShelfModel;

    // Start is called before the first frame update
    protected void Start()
    {
        m_ShelfGUi = GetComponent<ShelfGui>();
        m_ShelfModel = GetComponent<ShelfModel>();

        m_ShelfGUi.SetModel(m_ShelfModel);
        m_ShelfModel.onSelectionIndexChanged += OnSelectionIndexChanged;
    }

    protected abstract void OnSelectionIndexChanged(object sender, int index);
}
