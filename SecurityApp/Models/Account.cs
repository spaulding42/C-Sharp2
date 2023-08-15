namespace SecurityApp.Models;
#pragma warning disable CS8618

public class Account
{
    public int AccountID {get; set; }
    public string AccountPassword {get; set; }
    public DateTime ContractStart {get; set; } = DateTime.Now;
    public DateTime ContractEnd {get; set; }
    public Customer CustomerData {get; set; }
    public Salesman Sales {get; set; }
    public Technician? Tech {get; set; }
    public List<Equipment> EquipmentList = new List<Equipment>();
}