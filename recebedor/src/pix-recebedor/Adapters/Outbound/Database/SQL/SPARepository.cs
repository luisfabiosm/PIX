using Dapper;
using Domain.Core.Base;
using Domain.Core.Interfaces.Outbound;
using Domain.UseCases.CreditoDevolucao;
using Domain.UseCases.CreditoOrdemPagamento;
using Domain.UseCases.ValidarContaCliente;
using System.Data;

namespace Adapters.Outbound.Database.SQL
{
    public class SPARepository : BaseSQLRepository, ISPARepository
    {

        public SPARepository(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            
        }

        public async ValueTask<(string result, Exception exception)> RegistrarCreditoOrdemPagamento(TransactionCreditoOrdemPagamento transaction)
        {
            try
            {

                using var operationContext = _loggingAdapter.StartOperation("CreditoPagamento", transaction.CorrelationId);
                string _mensagemPixOut = string.Empty;

                _loggingAdapter.AddProperty("Recebedor Agencia", transaction.recebedor.nrAgencia);
                _loggingAdapter.AddProperty("Recebedor Conta", transaction.recebedor.nrConta);
                _loggingAdapter.AddProperty("Recebedor CPF-CNPJ", transaction.recebedor.cpfCnpj.ToString());
                _loggingAdapter.AddProperty("Chave Idempotencia", transaction.chaveIdempotencia);
                _loggingAdapter.AddProperty("endToEndId", transaction.endToEndId);
         
                await _dbConnection.ExecuteWithRetryAsync(async (_connection) =>
                {

                    var _parameters = new DynamicParameters();
                    _parameters.Add("@pvchOperador", "102020");
                    _parameters.Add("@ptinCanal", byte.Parse(transaction.canal.ToString()));
                    _parameters.Add("@psmlAgencia", 0);
                    _parameters.Add("@ptinPosto", 1);
                    _parameters.Add("@pvchEstacao", "pix-recebedor");
                    _parameters.Add("@pvchChvIdemPotencia", transaction.chaveIdempotencia);
                    _parameters.Add("@pvchMsgPixIN", transaction.getTransactionSerialization());

                    // Parâmetro de saída
                    _parameters.Add("@pvchMsgPixOUT", dbType: DbType.String, direction: ParameterDirection.InputOutput, size: 4000);

                    await _connection.ExecuteAsync("sps_PixCredito", _parameters,
                            commandTimeout: _dbsettings.Value.CommandTimeout,
                            commandType: CommandType.StoredProcedure);

                    _mensagemPixOut = _parameters.Get<string>("@pvchMsgPixOUT");
                });

                return (_mensagemPixOut, null);
            }
            catch (Exception ex)
            {
                return HandleException("RegistrarCreditoOrdemPagamento", ex);
            }
        
        }


        public async ValueTask<(string result, Exception exception)> ValidarConta(TransactionValidarContaCliente transaction)
        {
            try
            {

                using var operationContext = _loggingAdapter.StartOperation("ValidarConta", transaction.CorrelationId);
                string _mensagemPixOut = string.Empty;

                _loggingAdapter.AddProperty("Recebedor Agencia", transaction.recebedor.nrAgencia);
                _loggingAdapter.AddProperty("Recebedor Conta", transaction.recebedor.nrConta);
                _loggingAdapter.AddProperty("Recebedor CPF-CNPJ", transaction.recebedor.cpfCnpj.ToString());
                _loggingAdapter.AddProperty("Chave Idempotencia", transaction.chaveIdempotencia);


                await _dbConnection.ExecuteWithRetryAsync(async (_connection) =>
                {
                  
                    var _parameters = new DynamicParameters();
                    _parameters.Add("@pvchOperador", "102020");
                    _parameters.Add("@ptinCanal", byte.Parse(transaction.canal.ToString()));
                    _parameters.Add("@psmlAgencia", 0);
                    _parameters.Add("@ptinPosto", 1);
                    _parameters.Add("@pvchEstacao", "pix-recebedor");
                    _parameters.Add("@pvchMsgPixIN", transaction.getTransactionSerialization());

                    // Parâmetro de saída
                    _parameters.Add("@pvchMsgPixOUT", dbType: DbType.String, direction: ParameterDirection.InputOutput, size: 4000);

                    await _connection.ExecuteAsync("sps_PixValidaCredito", _parameters,
                            commandTimeout: _dbsettings.Value.CommandTimeout,
                            commandType: CommandType.StoredProcedure);

                    _mensagemPixOut = _parameters.Get<string>("@pvchMsgPixOUT");                 
                });

                return (_mensagemPixOut, null);
            }
            catch (Exception ex)
            {
                return HandleException("ValidarConta", ex);
            }
        }


        public async ValueTask<(string result, Exception exception)> RegistrarCreditoDevolucao(TransactionCreditoDevolucao transaction)
        {
            try
            {

                using var operationContext = _loggingAdapter.StartOperation("CreditoDevolucao", transaction.CorrelationId);
                string _mensagemPixOut = string.Empty;

                _loggingAdapter.AddProperty("Recebedor Agencia", transaction.recebedor.nrAgencia);
                _loggingAdapter.AddProperty("Recebedor Conta", transaction.recebedor.nrConta);
                _loggingAdapter.AddProperty("Recebedor CPF-CNPJ", transaction.recebedor.cpfCnpj.ToString());
                _loggingAdapter.AddProperty("Chave Idempotencia", transaction.chaveIdempotencia);
                _loggingAdapter.AddProperty("Codigo devolucao", transaction.codigoDevolucao);
                _loggingAdapter.AddProperty("endToEndIdOriginal", transaction.endToEndIdOriginal);
                _loggingAdapter.AddProperty("endToEndIdDevolucao", transaction.endToEndIdDevolucao);


                await _dbConnection.ExecuteWithRetryAsync(async (_connection) =>
                {

                    var _parameters = new DynamicParameters();
                    _parameters.Add("@pvchOperador", "102020");
                    _parameters.Add("@ptinCanal", byte.Parse(transaction.canal.ToString()));
                    _parameters.Add("@psmlAgencia", 0);
                    _parameters.Add("@ptinPosto", 1);
                    _parameters.Add("@pvchEstacao", "pix-recebedor");
                    _parameters.Add("@pvchMsgPixIN", transaction.getTransactionSerialization());

                    // Parâmetro de saída
                    _parameters.Add("@pvchMsgPixOUT", dbType: DbType.String, direction: ParameterDirection.InputOutput, size: 4000);

                    await _connection.ExecuteAsync("sps_PixDevolucaoDebito", _parameters,
                            commandTimeout: _dbsettings.Value.CommandTimeout,
                            commandType: CommandType.StoredProcedure);

                    _mensagemPixOut = _parameters.Get<string>("@pvchMsgPixOUT");
                });

                return (_mensagemPixOut, null);
      
            }
            catch (Exception ex)
            {
                return HandleException("RegistrarCreditoDevolucao",ex);         
            }
        }

    }
}
