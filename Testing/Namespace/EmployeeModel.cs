namespace Testing.Namespace;

public class EmployeeModel
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    
    public DateOnly BirthDate { get; set; }
    public DateOnly HireDate { get; set; }
}