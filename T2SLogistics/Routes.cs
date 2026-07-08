namespace T2SLogistics;

/// <summary>Nomes de rota do Shell, num único sítio.</summary>
public static class Routes
{
    public const string Movements = "movements";       // lista reutilizável (Expedição/Receção/Inventário)
    public const string MovementDetail = "movement";   // detalhe reutilizável
    public const string ReceptionReading = "reception-reading"; // conferência por leitura da Receção
    public const string ManagePin = "manage-pin";      // definir/alterar o PIN do operador
}
