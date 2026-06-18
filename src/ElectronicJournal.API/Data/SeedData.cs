using System.Security.Cryptography;
using ElectronicJournal.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ElectronicJournal.API.Data;

public static class SeedData
{
    public static async Task InitializeAsync(ApplicationDbContext db)
    {
        if (await db.Users.AnyAsync())
            return;

        // --- Users ---
        var admin = new User { Email = "admin@technikum.ru", PasswordHash = HashPassword("Admin123!"), FirstName = "Системный", LastName = "Администратор", Role = UserRole.Admin };
        var teacher1User = new User { Email = "smirnov@technikum.ru", PasswordHash = HashPassword("Teacher123!"), FirstName = "Иван", LastName = "Смирнов", Patronymic = "Петрович", Role = UserRole.Teacher, Phone = "+7(900)111-11-11" };
        var teacher2User = new User { Email = "kozlova@technikum.ru", PasswordHash = HashPassword("Teacher123!"), FirstName = "Анна", LastName = "Козлова", Patronymic = "Викторовна", Role = UserRole.Teacher, Phone = "+7(900)222-22-22" };
        var student1User = new User { Email = "ivanov@technikum.ru", PasswordHash = HashPassword("Student123!"), FirstName = "Алексей", LastName = "Иванов", Patronymic = "Сергеевич", Role = UserRole.Student };
        var student2User = new User { Email = "petrova@technikum.ru", PasswordHash = HashPassword("Student123!"), FirstName = "Мария", LastName = "Петрова", Patronymic = "Андреевна", Role = UserRole.Student };
        var student3User = new User { Email = "sidorov@technikum.ru", PasswordHash = HashPassword("Student123!"), FirstName = "Дмитрий", LastName = "Сидоров", Patronymic = "Олегович", Role = UserRole.Student };
        var student4User = new User { Email = "kuznetsova@technikum.ru", PasswordHash = HashPassword("Student123!"), FirstName = "Елена", LastName = "Кузнецова", Role = UserRole.Student };
        var parentUser = new User { Email = "parent@technikum.ru", PasswordHash = HashPassword("Parent123!"), FirstName = "Сергей", LastName = "Иванов", Patronymic = "Владимирович", Role = UserRole.Parent };

        db.Users.AddRange(admin, teacher1User, teacher2User, student1User, student2User, student3User, student4User, parentUser);
        await db.SaveChangesAsync();

        // --- Teachers ---
        var teacher1 = new Teacher { UserId = teacher1User.Id, Department = "Информационные технологии" };
        var teacher2 = new Teacher { UserId = teacher2User.Id, Department = "Общеобразовательные дисциплины" };
        db.Teachers.AddRange(teacher1, teacher2);
        await db.SaveChangesAsync();

        // --- Groups ---
        var group1 = new Group { Name = "ИС-21", CourseNumber = 2, Specialty = "Информационные системы", CuratorId = teacher1.Id };
        var group2 = new Group { Name = "ПР-11", CourseNumber = 1, Specialty = "Программирование", CuratorId = teacher2.Id };
        db.Groups.AddRange(group1, group2);
        await db.SaveChangesAsync();

        // --- Students ---
        var student1 = new Student { UserId = student1User.Id, GroupId = group1.Id, ParentId = parentUser.Id };
        var student2 = new Student { UserId = student2User.Id, GroupId = group1.Id };
        var student3 = new Student { UserId = student3User.Id, GroupId = group1.Id };
        var student4 = new Student { UserId = student4User.Id, GroupId = group2.Id };
        db.Students.AddRange(student1, student2, student3, student4);
        await db.SaveChangesAsync();

        // --- Subjects ---
        var math = new Subject { Name = "Математика", Description = "Высшая математика", HoursTotal = 120 };
        var prog = new Subject { Name = "Программирование", Description = "Основы программирования на C#", HoursTotal = 144 };
        var db_ = new Subject { Name = "Базы данных", Description = "Проектирование и администрирование БД", HoursTotal = 96 };
        var russian = new Subject { Name = "Русский язык", Description = "Русский язык и культура речи", HoursTotal = 80 };
        var english = new Subject { Name = "Английский язык", Description = "Технический английский", HoursTotal = 80 };
        var physics = new Subject { Name = "Физика", HoursTotal = 96 };
        db.Subjects.AddRange(math, prog, db_, russian, english, physics);
        await db.SaveChangesAsync();

        // --- TeacherSubjects ---
        db.TeacherSubjects.AddRange(
            new TeacherSubject { TeacherId = teacher1.Id, SubjectId = math.Id, GroupId = group1.Id },
            new TeacherSubject { TeacherId = teacher1.Id, SubjectId = prog.Id, GroupId = group1.Id },
            new TeacherSubject { TeacherId = teacher1.Id, SubjectId = db_.Id, GroupId = group1.Id },
            new TeacherSubject { TeacherId = teacher2.Id, SubjectId = russian.Id, GroupId = group1.Id },
            new TeacherSubject { TeacherId = teacher2.Id, SubjectId = english.Id, GroupId = group1.Id },
            new TeacherSubject { TeacherId = teacher1.Id, SubjectId = prog.Id, GroupId = group2.Id },
            new TeacherSubject { TeacherId = teacher2.Id, SubjectId = math.Id, GroupId = group2.Id }
        );
        await db.SaveChangesAsync();

        // --- Schedule ---
        db.Schedules.AddRange(
            new Schedule { GroupId = group1.Id, SubjectId = math.Id, TeacherId = teacher1.Id, DayOfWeek = DayOfWeek.Monday, PairNumber = 1, Room = "301" },
            new Schedule { GroupId = group1.Id, SubjectId = prog.Id, TeacherId = teacher1.Id, DayOfWeek = DayOfWeek.Monday, PairNumber = 2, Room = "205" },
            new Schedule { GroupId = group1.Id, SubjectId = russian.Id, TeacherId = teacher2.Id, DayOfWeek = DayOfWeek.Monday, PairNumber = 3, Room = "108" },
            new Schedule { GroupId = group1.Id, SubjectId = english.Id, TeacherId = teacher2.Id, DayOfWeek = DayOfWeek.Tuesday, PairNumber = 1, Room = "115" },
            new Schedule { GroupId = group1.Id, SubjectId = prog.Id, TeacherId = teacher1.Id, DayOfWeek = DayOfWeek.Tuesday, PairNumber = 2, Room = "205" },
            new Schedule { GroupId = group1.Id, SubjectId = db_.Id, TeacherId = teacher1.Id, DayOfWeek = DayOfWeek.Tuesday, PairNumber = 3, Room = "205" },
            new Schedule { GroupId = group1.Id, SubjectId = math.Id, TeacherId = teacher1.Id, DayOfWeek = DayOfWeek.Wednesday, PairNumber = 1, Room = "301" },
            new Schedule { GroupId = group1.Id, SubjectId = prog.Id, TeacherId = teacher1.Id, DayOfWeek = DayOfWeek.Wednesday, PairNumber = 2, Room = "205" },
            new Schedule { GroupId = group1.Id, SubjectId = physics.Id, TeacherId = teacher2.Id, DayOfWeek = DayOfWeek.Thursday, PairNumber = 1, Room = "312" },
            new Schedule { GroupId = group1.Id, SubjectId = math.Id, TeacherId = teacher1.Id, DayOfWeek = DayOfWeek.Thursday, PairNumber = 2, Room = "301" },
            new Schedule { GroupId = group1.Id, SubjectId = russian.Id, TeacherId = teacher2.Id, DayOfWeek = DayOfWeek.Friday, PairNumber = 1, Room = "108" },
            new Schedule { GroupId = group1.Id, SubjectId = db_.Id, TeacherId = teacher1.Id, DayOfWeek = DayOfWeek.Friday, PairNumber = 2, Room = "205" }
        );
        await db.SaveChangesAsync();

        // --- Grades ---
        var random = new Random(42);
        var students = new[] { student1, student2, student3 };
        var subjects = new[] { math, prog, db_, russian, english };

        for (int weekOffset = 0; weekOffset < 4; weekOffset++)
        {
            foreach (var student in students)
            {
                foreach (var subject in subjects)
                {
                    var teacherId = (subject.Id == russian.Id || subject.Id == english.Id) ? teacher2.Id : teacher1.Id;
                    db.Grades.Add(new Grade
                    {
                        StudentId = student.Id,
                        SubjectId = subject.Id,
                        TeacherId = teacherId,
                        Value = random.Next(3, 6),
                        Date = DateTime.Today.AddDays(-weekOffset * 7 - random.Next(0, 5)),
                        GradeType = GradeType.Current
                    });
                }
            }
        }

        db.Grades.AddRange(
            new Grade { StudentId = student1.Id, SubjectId = math.Id, TeacherId = teacher1.Id, Value = 4, Date = DateTime.Today.AddDays(-30), GradeType = GradeType.Midterm, Comment = "Хорошая работа" },
            new Grade { StudentId = student2.Id, SubjectId = math.Id, TeacherId = teacher1.Id, Value = 5, Date = DateTime.Today.AddDays(-30), GradeType = GradeType.Midterm, Comment = "Отлично!" },
            new Grade { StudentId = student3.Id, SubjectId = math.Id, TeacherId = teacher1.Id, Value = 3, Date = DateTime.Today.AddDays(-30), GradeType = GradeType.Midterm, Comment = "Нужно подтянуть" }
        );
        await db.SaveChangesAsync();

        // --- Attendance ---
        for (int dayOffset = 0; dayOffset < 14; dayOffset++)
        {
            var date = DateTime.Today.AddDays(-dayOffset);
            if (date.DayOfWeek == DayOfWeek.Sunday || date.DayOfWeek == DayOfWeek.Saturday)
                continue;

            foreach (var student in students)
            {
                var status = random.Next(10) < 8 ? AttendanceStatus.Present :
                             random.Next(3) == 0 ? AttendanceStatus.Late : AttendanceStatus.Absent;
                db.Attendances.Add(new Attendance
                {
                    StudentId = student.Id,
                    SubjectId = subjects[random.Next(subjects.Length)].Id,
                    Date = date,
                    Status = status,
                    Note = status == AttendanceStatus.Absent ? "Без уважительной причины" : null
                });
            }
        }
        await db.SaveChangesAsync();

        // --- Chat messages ---
        db.ChatMessages.AddRange(
            new ChatMessage { SenderId = teacher1User.Id, ReceiverId = student1User.Id, Text = "Алексей, не забудьте сдать лабораторную работу №3 до пятницы.", SentAt = DateTime.UtcNow.AddHours(-5) },
            new ChatMessage { SenderId = student1User.Id, ReceiverId = teacher1User.Id, Text = "Добрый день! Да, я уже почти закончил, сдам завтра.", SentAt = DateTime.UtcNow.AddHours(-4), IsRead = true },
            new ChatMessage { SenderId = teacher1User.Id, ReceiverId = student1User.Id, Text = "Хорошо, жду.", SentAt = DateTime.UtcNow.AddHours(-3), IsRead = true },
            new ChatMessage { SenderId = teacher2User.Id, ReceiverId = student2User.Id, Text = "Мария, ваше сочинение — одно из лучших в группе. Молодец!", SentAt = DateTime.UtcNow.AddDays(-1) }
        );
        await db.SaveChangesAsync();

        // --- Notifications ---
        db.Notifications.AddRange(
            new Notification { UserId = student1User.Id, Title = "Новая оценка", Message = "Вам выставлена оценка 5 по предмету Программирование.", Type = NotificationType.Grade },
            new Notification { UserId = student1User.Id, Title = "Изменение расписания", Message = "Занятие по физике в четверг перенесено в аудиторию 315.", Type = NotificationType.Schedule },
            new Notification { UserId = student2User.Id, Title = "Новая оценка", Message = "Вам выставлена оценка 4 по предмету Математика.", Type = NotificationType.Grade },
            new Notification { UserId = teacher1User.Id, Title = "Системное уведомление", Message = "Началась зачётная неделя. Пожалуйста, выставьте промежуточные оценки.", Type = NotificationType.System }
        );
        await db.SaveChangesAsync();
    }

    private static string HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(16);
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, 100_000, HashAlgorithmName.SHA256, 32);
        return $"{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
    }
}
