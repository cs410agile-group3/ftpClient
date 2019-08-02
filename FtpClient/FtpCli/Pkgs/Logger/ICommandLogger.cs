namespace FtpCli.Pkgs.Logger
{
  public interface ILogger
  {
    // Main functions available to a user
    //
    // Type: pulic
    string FilePath();
    void Log(string cmd);
    string PrevLogItem();
    string NextLogItem();
    void DeleteLogs();
  }
}
