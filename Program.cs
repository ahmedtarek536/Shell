

using Shell;
using System.Data;

List<string> SplitString(string input)
{
    input = input.Trim() + " ";
    List<string> strs = new List<string>();
    string word = "";
    bool flage = false;
    for(int i = 0; i < input.Length; i++)
    {
        if (!flage && input[i] == '"') flage = true; 
        else if (flage && input[i] == '"') flage = false;
        else if (flage) word += input[i];
        else if (input[i] != ' ') word += input[i];
        else if (word.Length > 0){
            strs.Add(word);
            word = "";
        }
    }
    return strs;
}

List<String> getTokens()
{
       Console.WriteLine();
       Console.Write($"{Settings.CurrentPath}>> ");
       string input = Console.ReadLine().Trim().ToString();
       var tokens = SplitString(input);
       return tokens;
}

string Command = "";
List<string> Flags = new List<String>();
List<string> Arguments = new List<String>();
var stateMachine = new StateMachine();

while (Settings.Running)
{
    Command = "";
    Flags.Clear();
    Arguments.Clear();
    var Tokens = getTokens();
    for(int i = 0; i < Tokens.Count ; i++)
    {
      if ( i == 0 ) Command = Tokens[i];
      else if (Tokens[i][0] == '-') Flags.Add(Tokens[i]);
      else Arguments.Add(Tokens[i]);
    }
    // execute
    stateMachine.SwitchState(Command);
    stateMachine.Process(Flags, Arguments);
}








