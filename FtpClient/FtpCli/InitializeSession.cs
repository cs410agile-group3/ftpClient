using System;

namespace FtpCli
{
  // The IntializeSession class prompts the user for session details
  // which are stored as a Data object
  public class InitializeSession
  {
      Data data;

      public InitializeSession() {
        data = new Data();
      }

    // prompt user for server, username, and commands (if provided)
    public Data initialPrompt(string[] args) {
      string response = "";

      do {
        response = this.promptUser("Enter Server: ");
      } while (!validateNonEmptyResponse(response));
      this.data.setServer(response);

      do {
        response = this.promptUser("Enter Username: ");
      } while (!validateNonEmptyResponse(response));
      this.data.setUser(response);

      response = this.promptUser("Enter Command (or `Enter` if none): ");
      this.data.setCommands(response);

      return this.data;
    }

    // prompts user and returns input
    public string promptUser(string prompt) {

      Console.WriteLine(prompt);
      string response = Console.ReadLine();
      return response.Trim();
    }

    // validates that the response contains valid string content
    public bool validateNonEmptyResponse(string response) 
    {
      // incorrect number of values passed in
      if (response == "") {
        return false;
      }

      // if more than one string value is passed in
      response.Trim();
      if (response.Contains(" ")) {
        return false;
      }
      return true;
    }
  }
}
