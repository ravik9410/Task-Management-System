namespace TaskManagementApp.Service.Contract
{
    public interface ITokenHandler
    {
        void SetToken(string token);
        string GetToken();
        void ClearToken();
    }
}
