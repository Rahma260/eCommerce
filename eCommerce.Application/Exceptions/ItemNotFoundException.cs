namespace eCommerce.Infrastructure.Exceptions
{
    //custom exception for item not found scenarios
    public class ItemNotFoundException(string message) : Exception(message)
    {
       
    }
}
