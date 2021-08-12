using System;

[Serializable]
public class PronounSetting { 
    public string SubjectForm; // eg, he/she/they
    public string ObjectForm; // eg, him/her/them
    public string PossessiveAdjectiveForm; // eg, his/her/their
    public string PossessivePronounForm; // eg, his/hers/theirs
    public string ReflexiveForm; // eg, himself/herself/themselves
    public bool Plural; // does pronoun count as plural or singular for subj/verb agreement (is/are, 's' at end of verbs)
}

public static class DialogueProcesser
{
    // ------------------------------------------------------------------------
    // Constants
    // ------------------------------------------------------------------------
    public static readonly PronounSetting NeutralPronouns = new PronounSetting {
        SubjectForm = "they",
        ObjectForm = "them",
        PossessiveAdjectiveForm = "their",
        PossessivePronounForm = "theirs",
        ReflexiveForm = "themself",
        Plural = true
    };

    public static readonly PronounSetting HeHimPronouns = new PronounSetting {
        SubjectForm = "he",
        ObjectForm = "him",
        PossessiveAdjectiveForm = "his",
        PossessivePronounForm = "his",
        ReflexiveForm = "himself",
        Plural = false
    };

    public static readonly PronounSetting SheHerPronouns = new PronounSetting {
        SubjectForm = "she",
        ObjectForm = "her",
        PossessiveAdjectiveForm = "her",
        PossessivePronounForm = "hers",
        ReflexiveForm = "herself",
        Plural = false
    };

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    public static string PreprocessDialogue (
        string text,
        string nameReplace,
        PronounSetting pronouns
    ) {
        // find player name
        string output = text.Replace("[name]", nameReplace);

        // replace pronouns
        output = output.Replace("[they]", pronouns.SubjectForm);
        output = output.Replace("[them]", pronouns.ObjectForm);
        output = output.Replace("[their]", pronouns.PossessiveAdjectiveForm);
        output = output.Replace("[theirs]", pronouns.PossessivePronounForm);
        output = output.Replace("[themself]", pronouns.ReflexiveForm);
        
        string subjUppercase = UppercaseFirstLetter(pronouns.SubjectForm);
        string objUppercase = UppercaseFirstLetter(pronouns.ObjectForm);
        string posAdjUppercase = UppercaseFirstLetter(pronouns.PossessiveAdjectiveForm);
        string posProUppercase = UppercaseFirstLetter(pronouns.PossessivePronounForm);
        string reflexUppercase = UppercaseFirstLetter(pronouns.ReflexiveForm);
        output = output.Replace("[they-c]", subjUppercase);
        output = output.Replace("[them-c]", objUppercase);
        output = output.Replace("[their-c]", posAdjUppercase);
        output = output.Replace("[themself-c]", reflexUppercase);

        // replace for subject/verb agreement for plural vs. singular pronouns
        output = output.Replace("[s]", pronouns.Plural ? "" : "s");
        output = output.Replace("[are]", pronouns.Plural ? "are" : "is");
        
        return output;
    }

    // ------------------------------------------------------------------------
    public static string FormatTime (DateTime time) {
        return time.ToString("HH:mm tt");
    }

    // ------------------------------------------------------------------------
    public static string FormatDateTime (DateTime time) {
        string text = FormatTime(time); 
        // only add the date if this message was sent more than a day ago
        if(time.Day != DateTime.Now.Day) {
            text = time.ToString("d MMM ") + text;
        }
        return text;
    }

    // ------------------------------------------------------------------------
    private static string UppercaseFirstLetter (string t) {
        return char.ToUpper(t[0]) + t.Substring(1);
    }
}
