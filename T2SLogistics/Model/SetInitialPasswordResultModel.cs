using System.Collections.Generic;

namespace T2SLogistics.Model
{
    // Corpo de erro devolvido por auth/set-initial-password quando falha (400). A API devolve
    // { message } nas recusas de estado da conta, ou { errors } nas falhas de política de password.
    // No sucesso (200) o corpo é vazio → desserializa para null.
    public class SetInitialPasswordResultModel
    {
        public string? message { get; set; }
        public List<string>? errors { get; set; }
    }
}
