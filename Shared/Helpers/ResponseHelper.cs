namespace Shared.Helpers;

public static class ResponseHelper
{
    public static string NotFound(string entity) =>
        $"{entity} not found.";

    public static string Created(string entity) =>
        $"{entity} created successfully.";

    public static string Updated(string entity) =>
        $"{entity} updated successfully.";

    public static string Deleted(string entity) =>
        $"{entity} deleted successfully.";
}