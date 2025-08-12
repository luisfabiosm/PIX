using System.Data;
using System.Reflection;

namespace Domain.Core.Constant;


public static class OperationConstants
{
    public const string DEFAULT_OPERADOR = "102020";
    public const int DEFAULT_AGENCIA = 0;
    public const int DEFAULT_POSTO = 1;
    public const string DEFAULT_ESTACAO = "pix-pagador";

    public static string APP_NAME = Assembly.GetExecutingAssembly().GetName().Name;
    public static int CONNECTIONS_ACTIVE = 0;
    public static int CONNECTIONS_CLOSED = 0;
}
