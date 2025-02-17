using Naninovel;
using UnityEngine;
using UnityEngine.UI;

public class GameButtonController : MonoBehaviour
{
    [SerializeField] private string hasArtifact = "hasArtifact";
    [SerializeField] private Button targetButton;

    private ICustomVariableManager variableManager;

    private void Awake()
    {
        variableManager = Engine.GetService<ICustomVariableManager>();
        variableManager.OnVariableUpdated += HandleVariableUpdate;
    }

    private void Start()
    {
        UpdateButtonState();
    }

    private void OnDestroy()
    {
        if (variableManager != null)
            variableManager.OnVariableUpdated -= HandleVariableUpdate;
    }

    private void HandleVariableUpdate(CustomVariableUpdatedArgs args)
    {
        if (args.Name == hasArtifact) 
            UpdateButtonState();
    }

    private void UpdateButtonState()
    {
        
        var HasArtifact = variableManager.GetVariableValue(hasArtifact);
        targetButton.interactable = HasArtifact != "True";
    }
}