# Управление ращработкой IT-проектов: API

Domain Driven Design (DDD), разбиение на уровни.

## Технологии
* EF Core + MS SQL Server
* ASP .NET Core

## Паттерны
* Entity Type Configuration (EF Core)
* Generic Repository
* Specification
* Data Transfer Object (DTO)
* Dependency Injection (DI)

## Фичи
* JWT-токены (авторизация)
* AutoMapper
* Асинхронность


## Generic Repository
```
public interface IRepository<T> : IDisposable where T : class
{
    public Task Create(T item);
    public Task<T> ReadOne(Specification<T> spec);
    public Task<List<T>> ReadMany(Specification<T> spec);
    public Task Update(Specification<T> spec, Action<T> func);
    public Task Delete(Specification<T> spec);
}
```