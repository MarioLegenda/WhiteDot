namespace WhiteDot.Tests.Namespace;

public class EmployeeModel
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    
    public DateOnly BirthDate { get; set; }
    public DateOnly HireDate { get; set; }
}