-- ============================================
-- Электронный журнал для техникума
-- Скрипт создания базы данных
-- ============================================

CREATE DATABASE ElectronicJournal;
GO

USE ElectronicJournal;
GO

-- ============================================
-- Таблица пользователей
-- ============================================
CREATE TABLE Users (
    Id              INT IDENTITY(1,1) PRIMARY KEY,
    Email           NVARCHAR(256) NOT NULL UNIQUE,
    PasswordHash    NVARCHAR(512) NOT NULL,
    FirstName       NVARCHAR(100) NOT NULL,
    LastName        NVARCHAR(100) NOT NULL,
    Patronymic      NVARCHAR(100) NULL,
    Role            INT NOT NULL DEFAULT 2, -- 0=Admin, 1=Teacher, 2=Student, 3=Parent
    Phone           NVARCHAR(20) NULL,
    AvatarUrl       NVARCHAR(500) NULL,
    IsActive        BIT NOT NULL DEFAULT 1,
    CreatedAt       DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt       DATETIME2 NULL
);

CREATE INDEX IX_Users_Email ON Users(Email);
CREATE INDEX IX_Users_Role ON Users(Role);

-- ============================================
-- Группы
-- ============================================
CREATE TABLE Groups (
    Id              INT IDENTITY(1,1) PRIMARY KEY,
    Name            NVARCHAR(50) NOT NULL UNIQUE,
    CourseNumber    INT NOT NULL,
    Specialty       NVARCHAR(200) NOT NULL,
    CuratorId       INT NULL,
    CreatedAt       DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT FK_Groups_Curator FOREIGN KEY (CuratorId) REFERENCES Users(Id)
);

-- ============================================
-- Студенты (расширение пользователя)
-- ============================================
CREATE TABLE Students (
    Id              INT IDENTITY(1,1) PRIMARY KEY,
    UserId          INT NOT NULL UNIQUE,
    GroupId         INT NOT NULL,
    StudentTicket   NVARCHAR(50) NULL,
    ParentId        INT NULL,
    EnrollmentDate  DATE NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Students_User FOREIGN KEY (UserId) REFERENCES Users(Id),
    CONSTRAINT FK_Students_Group FOREIGN KEY (GroupId) REFERENCES Groups(Id),
    CONSTRAINT FK_Students_Parent FOREIGN KEY (ParentId) REFERENCES Users(Id)
);

CREATE INDEX IX_Students_GroupId ON Students(GroupId);

-- ============================================
-- Преподаватели (расширение пользователя)
-- ============================================
CREATE TABLE Teachers (
    Id              INT IDENTITY(1,1) PRIMARY KEY,
    UserId          INT NOT NULL UNIQUE,
    Department      NVARCHAR(200) NOT NULL,
    Position        NVARCHAR(200) NULL,
    HireDate        DATE NULL,
    CONSTRAINT FK_Teachers_User FOREIGN KEY (UserId) REFERENCES Users(Id)
);

-- ============================================
-- Предметы
-- ============================================
CREATE TABLE Subjects (
    Id              INT IDENTITY(1,1) PRIMARY KEY,
    Name            NVARCHAR(200) NOT NULL,
    ShortName       NVARCHAR(50) NULL,
    Description     NVARCHAR(1000) NULL,
    HoursTotal      INT NOT NULL DEFAULT 0
);

-- ============================================
-- Связь Преподаватель-Предмет-Группа
-- ============================================
CREATE TABLE TeacherSubjects (
    Id              INT IDENTITY(1,1) PRIMARY KEY,
    TeacherId       INT NOT NULL,
    SubjectId       INT NOT NULL,
    GroupId         INT NOT NULL,
    Semester        INT NOT NULL DEFAULT 1,
    AcademicYear    NVARCHAR(9) NOT NULL, -- '2025-2026'
    CONSTRAINT FK_TS_Teacher FOREIGN KEY (TeacherId) REFERENCES Teachers(Id),
    CONSTRAINT FK_TS_Subject FOREIGN KEY (SubjectId) REFERENCES Subjects(Id),
    CONSTRAINT FK_TS_Group FOREIGN KEY (GroupId) REFERENCES Groups(Id),
    CONSTRAINT UQ_TeacherSubjectGroup UNIQUE (TeacherId, SubjectId, GroupId, Semester, AcademicYear)
);

-- ============================================
-- Оценки
-- ============================================
CREATE TABLE Grades (
    Id              INT IDENTITY(1,1) PRIMARY KEY,
    StudentId       INT NOT NULL,
    SubjectId       INT NOT NULL,
    TeacherId       INT NOT NULL,
    Value           INT NOT NULL CHECK (Value >= 1 AND Value <= 5),
    Date            DATE NOT NULL,
    Comment         NVARCHAR(500) NULL,
    GradeType       INT NOT NULL DEFAULT 0, -- 0=Current, 1=Midterm, 2=Final, 3=Exam
    CreatedAt       DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt       DATETIME2 NULL,
    CONSTRAINT FK_Grades_Student FOREIGN KEY (StudentId) REFERENCES Students(Id),
    CONSTRAINT FK_Grades_Subject FOREIGN KEY (SubjectId) REFERENCES Subjects(Id),
    CONSTRAINT FK_Grades_Teacher FOREIGN KEY (TeacherId) REFERENCES Teachers(Id)
);

CREATE INDEX IX_Grades_StudentId ON Grades(StudentId);
CREATE INDEX IX_Grades_SubjectId ON Grades(SubjectId);
CREATE INDEX IX_Grades_Date ON Grades(Date);
CREATE INDEX IX_Grades_StudentSubject ON Grades(StudentId, SubjectId);

-- ============================================
-- Посещаемость
-- ============================================
CREATE TABLE Attendances (
    Id              INT IDENTITY(1,1) PRIMARY KEY,
    StudentId       INT NOT NULL,
    SubjectId       INT NOT NULL,
    Date            DATE NOT NULL,
    Status          INT NOT NULL DEFAULT 0, -- 0=Present, 1=Absent, 2=Late, 3=Excused
    Note            NVARCHAR(500) NULL,
    MarkedById      INT NOT NULL,
    CreatedAt       DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT FK_Att_Student FOREIGN KEY (StudentId) REFERENCES Students(Id),
    CONSTRAINT FK_Att_Subject FOREIGN KEY (SubjectId) REFERENCES Subjects(Id),
    CONSTRAINT FK_Att_MarkedBy FOREIGN KEY (MarkedById) REFERENCES Teachers(Id),
    CONSTRAINT UQ_Attendance UNIQUE (StudentId, SubjectId, Date)
);

CREATE INDEX IX_Attendances_StudentId ON Attendances(StudentId);
CREATE INDEX IX_Attendances_Date ON Attendances(Date);

-- ============================================
-- Расписание
-- ============================================
CREATE TABLE Schedules (
    Id              INT IDENTITY(1,1) PRIMARY KEY,
    GroupId         INT NOT NULL,
    SubjectId       INT NOT NULL,
    TeacherId       INT NOT NULL,
    DayOfWeek       INT NOT NULL CHECK (DayOfWeek >= 1 AND DayOfWeek <= 6), -- 1=Пн, 6=Сб
    PairNumber      INT NOT NULL CHECK (PairNumber >= 1 AND PairNumber <= 8),
    Room            NVARCHAR(50) NOT NULL,
    WeekType        INT NOT NULL DEFAULT 0, -- 0=Both, 1=Odd, 2=Even
    StartTime       TIME NULL,
    EndTime         TIME NULL,
    CONSTRAINT FK_Sch_Group FOREIGN KEY (GroupId) REFERENCES Groups(Id),
    CONSTRAINT FK_Sch_Subject FOREIGN KEY (SubjectId) REFERENCES Subjects(Id),
    CONSTRAINT FK_Sch_Teacher FOREIGN KEY (TeacherId) REFERENCES Teachers(Id)
);

CREATE INDEX IX_Schedules_GroupId ON Schedules(GroupId);
CREATE INDEX IX_Schedules_TeacherId ON Schedules(TeacherId);

-- ============================================
-- Сообщения чата
-- ============================================
CREATE TABLE ChatMessages (
    Id              INT IDENTITY(1,1) PRIMARY KEY,
    SenderId        INT NOT NULL,
    ReceiverId      INT NOT NULL,
    Text            NVARCHAR(2000) NOT NULL,
    SentAt          DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    IsRead          BIT NOT NULL DEFAULT 0,
    CONSTRAINT FK_Chat_Sender FOREIGN KEY (SenderId) REFERENCES Users(Id),
    CONSTRAINT FK_Chat_Receiver FOREIGN KEY (ReceiverId) REFERENCES Users(Id)
);

CREATE INDEX IX_Chat_SenderId ON ChatMessages(SenderId);
CREATE INDEX IX_Chat_ReceiverId ON ChatMessages(ReceiverId);
CREATE INDEX IX_Chat_SentAt ON ChatMessages(SentAt DESC);

-- ============================================
-- Уведомления
-- ============================================
CREATE TABLE Notifications (
    Id              INT IDENTITY(1,1) PRIMARY KEY,
    UserId          INT NOT NULL,
    Title           NVARCHAR(200) NOT NULL,
    Message         NVARCHAR(1000) NOT NULL,
    IsRead          BIT NOT NULL DEFAULT 0,
    Type            INT NOT NULL DEFAULT 0, -- 0=Info, 1=Grade, 2=Attendance, 3=Schedule, 4=System
    RelatedEntityId INT NULL,
    CreatedAt       DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT FK_Notif_User FOREIGN KEY (UserId) REFERENCES Users(Id)
);

CREATE INDEX IX_Notifications_UserId ON Notifications(UserId);
CREATE INDEX IX_Notifications_IsRead ON Notifications(UserId, IsRead);

-- ============================================
-- Таблица логов аудита
-- ============================================
CREATE TABLE AuditLogs (
    Id              INT IDENTITY(1,1) PRIMARY KEY,
    UserId          INT NULL,
    Action          NVARCHAR(100) NOT NULL,
    EntityType      NVARCHAR(100) NOT NULL,
    EntityId        INT NULL,
    OldValues       NVARCHAR(MAX) NULL,
    NewValues       NVARCHAR(MAX) NULL,
    IpAddress       NVARCHAR(45) NULL,
    CreatedAt       DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT FK_Audit_User FOREIGN KEY (UserId) REFERENCES Users(Id)
);

CREATE INDEX IX_AuditLogs_CreatedAt ON AuditLogs(CreatedAt DESC);

-- ============================================
-- Начальные данные - Администратор
-- ============================================
INSERT INTO Users (Email, PasswordHash, FirstName, LastName, Role)
VALUES ('admin@technikum.ru',
        -- BCrypt hash for 'Admin123!'
        '$2a$11$K6QKp8.h3rHGBx5xVq5X5.8VxqRk7Yx0xpVJWQj5YB5WO5KXWG2C',
        N'Администратор', N'Системный', 0);

-- ============================================
-- Тестовые предметы
-- ============================================
INSERT INTO Subjects (Name, ShortName, HoursTotal) VALUES
(N'Математика', N'Мат', 120),
(N'Информатика', N'Инф', 96),
(N'Русский язык', N'Рус', 80),
(N'Физика', N'Физ', 96),
(N'История', N'Ист', 64),
(N'Английский язык', N'Англ', 80),
(N'Программирование', N'Прог', 144),
(N'Базы данных', N'БД', 96),
(N'Операционные системы', N'ОС', 72),
(N'Компьютерные сети', N'КС', 72);

PRINT N'База данных ElectronicJournal успешно создана!';
GO
