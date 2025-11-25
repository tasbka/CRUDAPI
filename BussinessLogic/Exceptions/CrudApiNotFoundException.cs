namespace BussinessLogic.Exceptions;

public class CrudApiNotFoundException<T>() : CrudApiNotFoundExceptionBase($"{typeof(T).Name} not found");