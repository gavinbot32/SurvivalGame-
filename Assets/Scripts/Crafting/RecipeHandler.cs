using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using TMPro;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class RecipeHandler : MonoBehaviour
{
    [SerializeField] private GameObject leftSide;
    [SerializeField] private GameObject rightSide;
    [SerializeField] private Recipe recipe;
    [SerializeField] private GameObject ingredientPrefab;
    [SerializeField] private GameObject resultPrefab;
    [SerializeField] private TextMeshProUGUI buttonText;

    private CraftingManager craftingManager;
    

    private void Awake()
    {
        craftingManager = GetComponentInParent<CraftingManager>();
    }

    public void initialize(Recipe rec)
    {
        recipe = rec;

        for(int i = 0; i < recipe.ingredients.Length; i++) {
            Instantiate(ingredientPrefab, leftSide.transform).GetComponent<Ingredient>().initialize(recipe.ingredients[i].sprite, recipe.ingredient_count[i]);
        }
        Instantiate(resultPrefab, rightSide.transform).GetComponent<Ingredient>().initialize(recipe.result.sprite, recipe.result_count);
    }

    public void CraftRecipe()
    {
        craftingManager.Craft(recipe,buttonText);
    }
   

}
