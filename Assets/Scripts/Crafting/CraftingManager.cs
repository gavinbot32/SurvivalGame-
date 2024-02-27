using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class CraftingManager : MonoBehaviour
{
    [SerializeField] private Recipe[] recipes;
    [SerializeField] private Transform recipeContainer;
    [SerializeField] private GameObject recipePrefab;

    public GameObject craftingUI;
    [SerializeField] private CraftingHandler h_crafting;
    private float craftingTime;
    private bool crafting = false;
    private UIManager uIManager;

    private void Awake()
    {
        uIManager = FindObjectOfType<UIManager>();
    }

    private void Start()
    {

    }

    public void initialize(CraftingHandler craftH)
    {
        h_crafting = craftH;
        recipes = h_crafting.recipes;
        foreach(Recipe recipe in recipes)
        {
            RecipeHandler rec = Instantiate(recipePrefab,recipeContainer).GetComponent<RecipeHandler>();
            rec.initialize(recipe);
        }
    }

    public void Craft(Recipe recipe, TextMeshProUGUI buttonText)
    {
        bool craftable = true;
        for(int i = 0; i < recipe.ingredients.Length; i++)
        {
            if (PlayerController.instance.inv.CheckForItem(recipe.ingredients[i], recipe.ingredient_count[i]) == false)
            {
                craftable = false;
            }
        }

        if (!craftable) return;

        for (int i = 0; i < recipe.ingredients.Length; i++)
        {
            PlayerController.instance.inv.RemoveItem(recipe.ingredients[i], recipe.ingredient_count[i]);
        }

        if (!crafting)
        {
            craftingTime = recipe.timeToCraft;
            StartCoroutine(CraftingTime(recipe));
            StartCoroutine(CraftingTimer(recipe,buttonText));

        }
    }
    IEnumerator CraftingTime(Recipe recipe)
    {
        crafting = true;
        yield return new WaitForSeconds(recipe.timeToCraft);
        PlayerController.instance.inv.AddItem(recipe.result);
        crafting = false;
    }

    IEnumerator CraftingTimer(Recipe recipe, TextMeshProUGUI buttonText)
    {
        for (int i = 0; i < recipe.timeToCraft; i++)
        {
            buttonText.text = craftingTime.ToString();
            craftingTime--;
            yield return new WaitForSeconds(1f);
        }
        buttonText.text = ">";
    }

    
}
