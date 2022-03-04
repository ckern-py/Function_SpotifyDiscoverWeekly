using SpotifyTypes.MetaData;

namespace Domain
{
    public interface IAuthorization
    {
        Token GetToken();
    }
}
