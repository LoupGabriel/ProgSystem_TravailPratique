using UnityEngine;

[CreateAssetMenu(menuName ="Dialogue/NewDialogueData")]
public class DialogueTextData : ScriptableObject
{

    public string npcName;

    [TextArea(5,10)]
    public string[] dialogueLines;

    public float autoProgressDelay;
    public bool[] autoProgressLine;
    public bool[] endConversationLine;


    
    public float typingSpeed = 0.05f;
    public AudioClip voiceSound;
    public float voicePitch = 1f;
    public Sprite npc_portrait;
    public DialogueChoice[] choices;
}

[System.Serializable]
public class DialogueChoice
{


    public int dialogueIndex;
    public string[] choices;
    public int[] nextDialogueIndexes;





}
