namespace Ambev.DeveloperEvaluation.WebApi.Exceptions;

public class ForbiddenAccessException(string message = "Você não possui permissão para este recurso.") : Exception(message);
