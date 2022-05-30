namespace SandboxMAUI.ViewModels;

public class SampleItem
{
    public int Id { get; set; }
    public string Name { get; set; }
    public override string ToString() => Name;
}
