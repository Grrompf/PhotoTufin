using PhotoTufin.Model;

namespace PhotoTufin.Data;

public interface ICustomerRepository
{
    Customer GetCustomer(long id);
    void SaveCustomer(Customer customer);
}
