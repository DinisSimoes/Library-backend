namespace Library.API.Responses
{
    /// <summary>
    /// Classe genérica para padronizar respostas da API.
    /// </summary>
    /// <typeparam name="T">Tipo do dado retornado na resposta.</typeparam>
    public class ApiResponse<T>
    {
        /// <summary>
        /// Indica se a operação foi bem-sucedida.
        /// </summary>
        public bool Success { get; set; } = true;
        
        /// <summary>
        /// Dados retornados na resposta.
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// Inicializa uma nova instância da classe ApiResponse com os dados fornecidos.
        /// </summary>
        /// <param name="data">Os dados da resposta.</param>
        public ApiResponse(T data)
        {
            Data = data;
        }
    }
}
