namespace ElectronicJournal.API.Services;

public interface IReportService
{
    Task<byte[]> GenerateStudentReportAsync(int studentId);
}
