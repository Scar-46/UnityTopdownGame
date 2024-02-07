using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    /// <summary>
    /// Object interaction.
    /// </summary>
    string Interact();

    /// <summary>
    /// Description of the interaction.
    /// </summary>
    string GetDescription();

}
