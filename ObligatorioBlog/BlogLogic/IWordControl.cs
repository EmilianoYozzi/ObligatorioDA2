using BlogDomain;

namespace BlogServicesInterfaces
{
    public interface IWordControl
    {
        List<string> CheckOffensiveWords(string text);
    } 
}