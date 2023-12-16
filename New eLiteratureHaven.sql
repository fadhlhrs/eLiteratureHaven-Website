CREATE DATABASE eLiteratureHaven;
go
USE eLiteratureHaven
go

CREATE TABLE users 
(
	id int NOT NULL PRIMARY KEY IDENTITY(1,1),
	username VARCHAR(20) NOT NULL,
	password VARCHAR(20) NOT NULL,
	email VARCHAR(255) NOT NULL,
	role VARCHAR(20) NOT NULL DEFAULT('customer'),
	full_name VARCHAR(255),
	phone_number VARCHAR(50),
);

go

INSERT INTO [dbo].[users] 
(
	[Username],
	[Password],
	[Email],
	[Role]
)
VALUES
(
	'adminaccount',
	'12345678',
	'admin@gmail.com',
	'admin'
)
GO


CREATE TABLE books 
(
	id int NOT NULL PRIMARY KEY IDENTITY(1,1),
	title VARCHAR(255) NOT NULL,
	author VARCHAR(255),
	publisher VARCHAR(255),
	publication_year INT,
	description VARCHAR(255),
	category VARCHAR(20),
	genre VARCHAR(255),
	image_path varchar(255),
	pdf_path varchar(255)
);
go



CREATE TABLE transactions
(
	id int NOT NULL PRIMARY KEY IDENTITY(1,1),
	create_date DATETIME NOT NULL DEFAULT(GetDate()),
	payment_date DATETIME,
	due_date DATETIME,
	transaction_status varchar(255) NOT NULL DEFAULT('pending'),
	book_id int,
	user_id int,
	
	FOREIGN KEY(book_id) REFERENCES books(id),
	FOREIGN KEY(user_id) REFERENCES users(id)
);
go

Create trigger DueDateUpdate
on transactions
After insert, update
as
begin
SET NOCOUNT ON;

    DECLARE @id INT;
    DECLARE @due_date DATE;

    -- Assuming that your transactions table has columns named 'TransactionID' and 'DueDate'
    SELECT @id = id, @due_date = due_date
    FROM INSERTED;

    -- Check if the due date has been reached
    IF @due_date <= GETDATE()
    BEGIN
        -- Update the transaction status to 'Completed' (you can modify this based on your actual status values)
        UPDATE eLiteratureHaven.dbo.transactions
        SET transaction_status = 'due'
        WHERE id = @id;
    END
END;

	
	
	

	
