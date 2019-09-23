# Лаборатора робота №1
Ткаченко Валерія, КВ-72

## Ознайомлення з базовими операціями СУБД PostgreSQL

### Варіант №12

### Сутності:

#### Адміністратор
* ID
* ПІБ
* Зміна
* Зарплатня  

    CREATE TABLE administrator
    (
      AdminId SERIAL PRIMARY KEY,
      FullName VARCHAR(50),
      Shift INTEGER,
      Salary INTEGER
    );  


    

