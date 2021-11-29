using api_rest.DTOs;
using System.Threading.Tasks;


namespace api_rest.Interfaces
{
    public interface IEmail
    {
         Task Send(string emailAddress, string body, EmailOptionsDTO options);
    }
}