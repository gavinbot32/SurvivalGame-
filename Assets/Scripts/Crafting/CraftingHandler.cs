using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingHandler : MonoBehaviour
{
    public Recipe[] recipes;

    public CraftingManager m_crafting { get; private set; }

    [SerializeField] private GameObject p_craftingUI;
    private UIManager m_ui;

    private void Awake()
    {
        m_ui = FindObjectOfType<UIManager>();
    }

    private void Start()
    {
        initialize();
    }
    public void initialize()
    {
        m_crafting = Instantiate(p_craftingUI, m_ui.craftingContainer.transform).GetComponent<CraftingManager>();
        m_crafting.initialize(this);
    }

    public void toggleCraft()
    {
        if (m_crafting.craftingUI.activeInHierarchy)
        {
            m_ui.ScreenOff();
        }
        else
        {
            m_ui.ScreenOn(m_crafting.craftingUI, true);
        }
    }

}
