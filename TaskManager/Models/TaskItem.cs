namespace TaskManager.Models;

public class TaskItem
{
    public int Id { get; set; }   // chiave primaria
    public string Title { get; set; } = string.Empty;
    public bool IsDone { get; set; } = false;
}