using UnityEngine;
using System;
using System.Collections;

public interface ITalkable
{
    private void StartDialogue() { }
    private void NextLine() { }
    private void DisplayChoice(DialogueChoice choice) { }
    public void ChooseChoice(int nextIndex) { }
    private void DisplayCurrentLine() { }
    public void EndDialogue() { }
   
}
