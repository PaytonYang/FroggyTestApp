CREATE TABLE AuthorityTable (
    AuthorityId INTEGER PRIMARY KEY,
    AuthorityName NVARCHAR(255) NOT NULL
    );

INSERT INTO AuthorityTable VALUES 
    (1, 'Guest'),
    (2, 'Registered'),
    (3, 'Admin');

CREATE TABLE UserTable (
    UserId INTEGER PRIMARY KEY AUTOINCREMENT, 
    UserName NVARCHAR(255),
    UserPassword NVARCHAR(255),
    UserEmail NVARCHAR(255),
    AuthorityId INT NOT NULL,
    FOREIGN KEY (AuthorityId) REFERENCES AuthorityTable(AuthorityId)
    );

