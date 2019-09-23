# Лаборатора робота №1
Ткаченко Валерія, КВ-72

## Ознайомлення з базовими операціями СУБД PostgreSQL

### Варіант №12

### Сутності:

#### 1. Адміністратор
* ID
* ПІБ
* Зміна
* Зарплатня  

```
CREATE TABLE administrator
 (
   AdminId SERIAL PRIMARY KEY,
   FullName VARCHAR(50),
   Shift INTEGER,
   Salary INTEGER
 ); 
 ```
 
 #### 2. Касир
* ID
* ПІБ
* Зміна
* Зарплатня  

```
CREATE TABLE cashier
(
	CashierId SERIAL PRIMARY KEY,
	AdminId INTEGER,
	FullName VARCHAR(50),
	Shift INTEGER,
	Salary INTEGER,
	FOREIGN KEY (AdminId) REFERENCES administrator (AdminId) ON DELETE SET NULL,
	UNIQUE(CashierId,AdminId)
);
 ```
  #### 3. Відвідувач
* ID
* Категорія
* Пільга
* Розмір пільги 

```
CREATE TABLE customer
(
	CustomerId SERIAL PRIMARY KEY,
	CashierId INTEGER,
	Category VARCHAR(50),
	Privilege VARCHAR(50),
	PrivilegeSize VARCHAR(50),
	
	FOREIGN KEY (CashierId) REFERENCES cashier (CashierId) ON DELETE SET NULL,
	UNIQUE(CustomerId, CashierId)
);

 ```
 
  #### 4. Фільм
* Назва
* Жанр
* Дата релізу

```
CREATE TABLE film
(
	Title VARCHAR(50) PRIMARY KEY,
	Genre VARCHAR(50),
	ReleaseDate DATE NOT NULL DEFAULT CURRENT_DATE
);
 ```
 
   #### 5. Кінозал
* Номер
* Кількість місць
```
CREATE TABLE cinema_hall
(
	Hall_number SERIAL PRIMARY KEY,
	Number_of_seats INTEGER	
);
 ```


    

