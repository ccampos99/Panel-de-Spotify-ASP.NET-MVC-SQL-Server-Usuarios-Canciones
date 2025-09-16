create database spotifyEF
use spotifyEF


CREATE TABLE UserRoles (
    RoleId INT PRIMARY KEY IDENTITY(1,1),
    RoleName NVARCHAR(100) NOT NULL
);

CREATE TABLE Users (
    UserId INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(100) NOT NULL,
    Email NVARCHAR(255) NOT NULL,
    PasswordHash NVARCHAR(255) NOT NULL,
    RoleId INT,
    DateCreated DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (RoleId) REFERENCES UserRoles(RoleId)
);
select*from Users
CREATE TABLE Permissions (
    PermissionId INT PRIMARY KEY IDENTITY(1,1),
    PermissionName NVARCHAR(100) NOT NULL
);

CREATE TABLE RolePermissions (
    RolePermissionId INT PRIMARY KEY IDENTITY(1,1),
    RoleId INT,
    PermissionId INT,
    FOREIGN KEY (RoleId) REFERENCES UserRoles(RoleId),
    FOREIGN KEY (PermissionId) REFERENCES Permissions(PermissionId)
);

CREATE TABLE ActivityLogs (
    LogId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT,
    ActivityDescription NVARCHAR(500),
    ActivityDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserId) REFERENCES Users(UserId)
);

CREATE TABLE ContentManagement (
    ContentId INT PRIMARY KEY IDENTITY(1,1),
    ContentType NVARCHAR(50),
    ContentIdRef INT,
    UserId INT,
    Action NVARCHAR(50),
    ActionDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserId) REFERENCES Users(UserId)
);


INSERT INTO UserRoles (RoleName)
VALUES ('Administrator'), ('Moderator'), ('User');

INSERT INTO Users (Username, Email, PasswordHash, RoleId, DateCreated)
VALUES 
('admin', 'admin@example.com', 'hashedpassword1', 1, GETDATE()),
('mod1', 'mod1@example.com', 'hashedpassword2', 2, GETDATE()),
('user1', 'user1@example.com', 'hashedpassword3', 3, GETDATE());

INSERT INTO Permissions (PermissionName)
VALUES 
('Manage Users'), 
('Manage Content'), 
('View Analytics');

INSERT INTO RolePermissions (RoleId, PermissionId)
VALUES 
(1, 1),  -- El rol 'Administrator' tiene permiso para 'Manage Users'
(1, 2),  -- El rol 'Administrator' tiene permiso para 'Manage Content'
(1, 3),  -- El rol 'Administrator' tiene permiso para 'View Analytics'
(2, 2),  -- El rol 'Moderator' tiene permiso para 'Manage Content'
(3, 3);  -- El rol 'User' tiene permiso para 'View Analytics'

INSERT INTO ActivityLogs (UserId, ActivityDescription, ActivityDate)
VALUES 
(1, 'Admin logged in', GETDATE()),
(1, 'Admin added a new user', GETDATE());

INSERT INTO ContentManagement (ContentType, ContentIdRef, UserId, Action, ActionDate)
VALUES 
('Tyla', 1, 1, 'Agregar', GETDATE()),
('Maná', 2, 1, 'Modificar', GETDATE()),
('Michael Jackson', 3, 1, 'Agregar', GETDATE()),
('Selena', 4, 1, 'Modificar', GETDATE());
--listado de contenido

CREATE OR ALTER PROCEDURE usp_get_content_management
AS
BEGIN
    SELECT 
        ContentId,
        ContentType,
        ContentIdRef,
        UserId,
        Action,
        ActionDate
    FROM 
        ContentManagement
END
GO
EXEC usp_get_content_management;
-- select usuario
CREATE OR ALTER PROCEDURE usp_select_usuario 
    @UserId INT
AS 
BEGIN
    SELECT COUNT(*) 
    FROM Users 
    WHERE UserId = @UserId;
END
GO
-- agregar contenido
CREATE OR ALTER PROCEDURE usp_insert_content 
    @ContentType NVARCHAR(50),
    @ContentIdRef INT,
    @UserId INT,
    @Action NVARCHAR(50),
    @ActionDate DATETIME
AS 
BEGIN
    -- Verificar si el UserId existe
    IF EXISTS (SELECT 1 FROM Users WHERE UserId = @UserId)
    BEGIN
        INSERT INTO ContentManagement (
            ContentType, 
            ContentIdRef, 
            UserId, 
            Action, 
            ActionDate
        ) VALUES (
            @ContentType, 
            @ContentIdRef, 
            @UserId, 
            @Action, 
            @ActionDate
        );
    END
    ELSE
    BEGIN
        RAISERROR('El UserId no existe.', 16, 1);
    END
END
GO
create database spotifyEF
use spotifyEF


CREATE TABLE UserRoles (
    RoleId INT PRIMARY KEY IDENTITY(1,1),
    RoleName NVARCHAR(100) NOT NULL
);

CREATE TABLE Users (
    UserId INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(100) NOT NULL,
    Email NVARCHAR(255) NOT NULL,
    PasswordHash NVARCHAR(255) NOT NULL,
    RoleId INT,
    DateCreated DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (RoleId) REFERENCES UserRoles(RoleId)
);
select*from Users
CREATE TABLE Permissions (
    PermissionId INT PRIMARY KEY IDENTITY(1,1),
    PermissionName NVARCHAR(100) NOT NULL
);

CREATE TABLE RolePermissions (
    RolePermissionId INT PRIMARY KEY IDENTITY(1,1),
    RoleId INT,
    PermissionId INT,
    FOREIGN KEY (RoleId) REFERENCES UserRoles(RoleId),
    FOREIGN KEY (PermissionId) REFERENCES Permissions(PermissionId)
);

CREATE TABLE ActivityLogs (
    LogId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT,
    ActivityDescription NVARCHAR(500),
    ActivityDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserId) REFERENCES Users(UserId)
);

CREATE TABLE ContentManagement (
    ContentId INT PRIMARY KEY IDENTITY(1,1),
    ContentType NVARCHAR(50),
    ContentIdRef INT,
    UserId INT,
    Action NVARCHAR(50),
    ActionDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserId) REFERENCES Users(UserId)
);


INSERT INTO UserRoles (RoleName)
VALUES ('Administrator'), ('Moderator'), ('User');

INSERT INTO Users (Username, Email, PasswordHash, RoleId, DateCreated)
VALUES 
('admin', 'admin@example.com', 'hashedpassword1', 1, GETDATE()),
('mod1', 'mod1@example.com', 'hashedpassword2', 2, GETDATE()),
('user1', 'user1@example.com', 'hashedpassword3', 3, GETDATE());

INSERT INTO Permissions (PermissionName)
VALUES 
('Manage Users'), 
('Manage Content'), 
('View Analytics');

INSERT INTO RolePermissions (RoleId, PermissionId)
VALUES 
(1, 1),  -- El rol 'Administrator' tiene permiso para 'Manage Users'
(1, 2),  -- El rol 'Administrator' tiene permiso para 'Manage Content'
(1, 3),  -- El rol 'Administrator' tiene permiso para 'View Analytics'
(2, 2),  -- El rol 'Moderator' tiene permiso para 'Manage Content'
(3, 3);  -- El rol 'User' tiene permiso para 'View Analytics'

INSERT INTO ActivityLogs (UserId, ActivityDescription, ActivityDate)
VALUES 
(1, 'Admin logged in', GETDATE()),
(1, 'Admin added a new user', GETDATE());

INSERT INTO ContentManagement (ContentType, ContentIdRef, UserId, Action, ActionDate)
VALUES 
('Tyla', 1, 1, 'Agregar', GETDATE()),
('Maná', 2, 1, 'Modificar', GETDATE()),
('Michael Jackson', 3, 1, 'Agregar', GETDATE()),
('Selena', 4, 1, 'Modificar', GETDATE());
--listado de contenido

CREATE OR ALTER PROCEDURE usp_get_content_management
AS
BEGIN
    SELECT 
        ContentId,
        ContentType,
        ContentIdRef,
        UserId,
        Action,
        ActionDate
    FROM 
        ContentManagement
END
GO

EXEC usp_get_content_management;
-- select usuario
CREATE OR ALTER PROCEDURE usp_select_usuario 
    @UserId INT
AS 
BEGIN
    SELECT COUNT(*) 
    FROM Users 
    WHERE UserId = @UserId;
END
GO
-- agregar contenido
CREATE OR ALTER PROCEDURE usp_insert_content1 
    @ContentType NVARCHAR(50),
    @ContentIdRef INT,
    @UserId INT,
    @Action NVARCHAR(50),
    @ActionDate DATETIME
AS 
BEGIN
    IF EXISTS (SELECT 1 FROM Users WHERE UserId = @UserId)
    BEGIN
        INSERT INTO ContentManagement (
            ContentType, 
            ContentIdRef, 
            UserId, 
            Action, 
            ActionDate
        ) VALUES (
            @ContentType, 
            @ContentIdRef, 
            @UserId, 
            @Action, 
            @ActionDate
        );
    END
    ELSE
    BEGIN
        RAISERROR('El UserId no existe.', 16, 1);
    END
END
GO

-- Llamada al procedimiento
EXEC usp_insert_content1
    @ContentType = 'Ariana Grande', 
    @ContentIdRef = 5, 
    @UserId = 1,  
    @Action = 'Agregar', 
    @ActionDate = '2024-10-26 12:34:56'
GO

--proc para actualizar contenido
CREATE OR ALTER PROCEDURE usp_update_content
    @ContentId INT,
    @ContentType NVARCHAR(50),
    @ContentIdRef INT,
    @UserId INT,
    @Action NVARCHAR(50),
    @ActionDate DATETIME
AS 
BEGIN
    -- Verificar que el contenido exista antes de intentar actualizarlo
    IF EXISTS (SELECT 1 FROM ContentManagement WHERE ContentId = @ContentId)
    BEGIN
        UPDATE ContentManagement
        SET 
            ContentType = @ContentType,
            ContentIdRef = @ContentIdRef,
            UserId = @UserId,
            Action = @Action,
            ActionDate = @ActionDate
        WHERE 
            ContentId = @ContentId;
    END
    ELSE
    BEGIN
        -- Lanza un error si el ContentId no existe
        RAISERROR('El ContentId no existe.', 16, 1);
    END
END
GO

EXEC dbo.usp_update_content 
    @ContentId = 1,  -- Suponiendo que este ContentId existe
    @ContentType = 'TYLA',
    @ContentIdRef = 1,
    @UserId = 1,      
    @Action = 'Update',
    @ActionDate = '2024-10-26 12:34:56';  

SELECT * FROM ContentManagement;
SELECT * FROM Users;
GO
--proc delete
CREATE OR ALTER PROCEDURE usp_delete_content
    @ContentId INT
AS
BEGIN
    DELETE FROM ContentManagement
    WHERE ContentId = @ContentId;
END
GO
--proc para details:
CREATE PROCEDURE usp_get_content_by_id
    @ContentId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        ContentId,
        ContentType,
        ContentIdRef,
        Action,
        ActionDate,
        UserId
    FROM 
        ContentManagement
    WHERE 
        ContentId = @ContentId;
END
DECLARE @ContentId INT = 1; 
EXEC usp_get_content_by_id @ContentId;


-- User procedures 

CREATE PROCEDURE usp_get_users
AS
BEGIN
    SELECT UserId, Username, Email, RoleId, DateCreated
    FROM Users;
END;

CREATE PROCEDURE usp_insert_user
    @Username NVARCHAR(100),
    @Email NVARCHAR(255),
    @PasswordHash NVARCHAR(255),
    @RoleId INT
AS
BEGIN
    INSERT INTO Users (Username, Email, PasswordHash, RoleId, DateCreated)
    VALUES (@Username, @Email, @PasswordHash, @RoleId, GETDATE());
END;

CREATE PROCEDURE usp_update_user
    @UserId INT,
    @Username NVARCHAR(100),
    @Email NVARCHAR(255),
    @PasswordHash NVARCHAR(255),
    @RoleId INT
AS
BEGIN
    UPDATE Users
    SET Username = @Username,
        Email = @Email,
        PasswordHash = @PasswordHash,
        RoleId = @RoleId
    WHERE UserId = @UserId;
END;

CREATE PROCEDURE usp_get_user_by_id
    @UserId INT
AS
BEGIN
    SELECT UserId, Username, Email, RoleId, DateCreated
    FROM Users
    WHERE UserId = @UserId;
END;

CREATE PROCEDURE usp_delete_user
    @UserId INT
AS
BEGIN
    DELETE FROM Users
    WHERE UserId = @UserId;
END;

-- proc rolesController

CREATE PROCEDURE usp_get_roles
AS
BEGIN
    SELECT RoleId, RoleName FROM UserRoles;
END;


CREATE PROCEDURE usp_insert_role
    @RoleName NVARCHAR(100)
AS
BEGIN
    INSERT INTO UserRoles (RoleName)
    VALUES (@RoleName);
END;

CREATE PROCEDURE usp_update_role
    @RoleId INT,
    @RoleName NVARCHAR(100)
AS
BEGIN
    UPDATE UserRoles
    SET RoleName = @RoleName
    WHERE RoleId = @RoleId;
END;


CREATE PROCEDURE usp_get_role_by_id
    @RoleId INT
AS
BEGIN
    SELECT RoleId, RoleName
    FROM UserRoles
    WHERE RoleId = @RoleId;
END;

CREATE PROCEDURE usp_delete_role
    @RoleId INT
AS
BEGIN
    DELETE FROM UserRoles
    WHERE RoleId = @RoleId;
END;

-- proc PermissionsController

CREATE PROCEDURE usp_get_permissions
AS
BEGIN
    SELECT PermissionId, PermissionName FROM Permissions;
END;

CREATE PROCEDURE usp_insert_permission
    @PermissionName NVARCHAR(100)
AS
BEGIN
    INSERT INTO Permissions (PermissionName)
    VALUES (@PermissionName);
END;

CREATE PROCEDURE usp_update_permission
    @PermissionId INT,
    @PermissionName NVARCHAR(100)
AS
BEGIN
    UPDATE Permissions
    SET PermissionName = @PermissionName
    WHERE PermissionId = @PermissionId;
END;

CREATE PROCEDURE usp_get_permission_by_id
    @PermissionId INT
AS
BEGIN
    SELECT PermissionId, PermissionName
    FROM Permissions
    WHERE PermissionId = @PermissionId;
END;

CREATE PROCEDURE usp_delete_permission
    @PermissionId INT
AS
BEGIN
    DELETE FROM Permissions
    WHERE PermissionId = @PermissionId;
END;

-- proc RolePermission

CREATE PROCEDURE usp_get_role_permissions
AS
BEGIN
    SELECT rp.RolePermissionId, rp.RoleId, r.RoleName, rp.PermissionId, p.PermissionName
    FROM RolePermissions rp
    INNER JOIN UserRoles r ON rp.RoleId = r.RoleId
    INNER JOIN Permissions p ON rp.PermissionId = p.PermissionId;
END;





